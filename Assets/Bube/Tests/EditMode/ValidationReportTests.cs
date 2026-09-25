using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Bube.Editor;

namespace Bube.Tests {

// Faz 1'in asıl kazancı: doğrulayıcı ilk sorunda durmuyor. Bu testler bilerek
// bozulmuş bir bellek içi vaka besleyip bulguların **hepsinin** raporlandığını
// kanıtlar; eskiden ilk `throw` gerisini gizliyordu.
public sealed class ValidationReportTests {

 static Locale Locale(params string[] keys) =>
  new Locale { entries = keys.Select(key => new Entry { key = key, value = key }).ToArray() };

 [Test]
 public void Report_CollectsEveryProblem_NotJustTheFirst() {
  var data = new CaseData {
   id = "broken",
   nodes = new[] {
    new Node { id = "a", kind = "document", titleKey = "yok.baslik", bodyKey = "yok.govde",
     requires = new[] { "hayalet" }, requiresAsked = new[] { "hayalet.soru" } },
    new Node { id = "a", kind = "document", titleKey = "yok.baslik2", bodyKey = "yok.govde2" },
   },
   verdicts = new[] { new Verdict { id = "v1", labelKey = "yok.etiket", correct = true },
                      new Verdict { id = "v2", labelKey = "yok.etiket2", correct = true } },
   methods = new[] { new Choice { id = "m1", labelKey = "m1", correct = false } },
   evidence = new[] { new Choice { id = "e1", labelKey = "e1", correct = true,
    supportingSourceIds = new[] { "hayalet" } } },
   conclusionRequires = new[] { "hayalet" },
   failedReportTrustLoss = -1,
  };

  var report = new ValidationReport();
  CaseRules.ValidateStructure(data, Locale("kind.document"), report);

  Assert.Greater(report.Problems.Count, 6, "Bulgular tek tek toplanmalı:\n" + report.Summary());
  var all = string.Join("\n", report.Problems);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Yinelenen düğüm")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Bilinmeyen önkoşul")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Bilinmeyen soru önkoşulu")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Bilinmeyen rapor dayanağı")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Bilinmeyen kapanış önkoşulu")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Kariyer etkisi")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Vaka özeti")), all);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("Metin eksik")), all);
 }

 // Rapor sihirbazının her sütunu tam olarak bir doğru seçenek taşımalı: sıfırsa
 // vaka çözülemez, birden fazlaysa değerlendirme keyfîleşir.
 [Test]
 public void ExactlyOneCorrect_IsRequiredPerColumn() {
  var report = new ValidationReport();
  CaseRules.ValidateStructure(new CaseData {
   id = "counts", nodes = new[] { new Node { id = "a", kind = "document", titleKey = "t", bodyKey = "b" } },
   verdicts = new[] { new Verdict { id = "v1", labelKey = "t", correct = true },
                      new Verdict { id = "v2", labelKey = "t", correct = true } },
   methods = new Choice[0],
   evidence = new[] { new Choice { id = "e", correct = true } },
  }, Locale("t", "b", "kind.document"), report);

  Assert.IsTrue(report.Problems.Any(p => p.Contains("doğru şüpheli olmalı, 2 var")), report.Summary());
  Assert.IsTrue(report.Problems.Any(p => p.Contains("doğru yöntem olmalı, 0 var")), report.Summary());
  Assert.IsFalse(report.Problems.Any(p => p.Contains("doğru kanıt")), report.Summary());
 }

 [Test]
 public void Notes_DoNotFailTheRun() {
  var report = new ValidationReport();
  report.Note("ölü anahtar");
  Assert.IsFalse(report.HasProblems);
  report.Problem("gerçek sorun");
  Assert.IsTrue(report.HasProblems);
 }

 // Yinelenen dil anahtarı: `Locale.Get` ilk girişi döndürdüğü için ikinci çeviri
 // sessizce yok sayılır — bu yüzden sorun, not değil.
 [Test]
 public void DuplicateLocaleKey_IsReportedAsAProblem() {
  var locale = new Locale { entries = new[] {
   new Entry { key = "ayni.anahtar", value = "ilk" },
   new Entry { key = "ayni.anahtar", value = "ikinci" },
  } };
  var report = new ValidationReport();
  LocaleRules.Validate(locale, new CaseData[0], report);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("ayni.anahtar")),
   "Yinelenen anahtar bildirilmeli:\n" + report.Summary());
 }

 [Test]
 public void CaseChain_RejectsMissingTargetAndCycles() {
  var config = new GameConfig { initialCase = "a" };

  var missing = new ValidationReport();
  CaseChainRules.Validate(new[] {
   new CaseData { id = "a", nextCaseId = "yok" },
  }, config, missing);
  Assert.IsTrue(missing.Problems.Any(p => p.Contains("yok")), missing.Summary());

  var cycle = new ValidationReport();
  CaseChainRules.Validate(new[] {
   new CaseData { id = "a", nextCaseId = "b" },
   new CaseData { id = "b", nextCaseId = "a" },
  }, config, cycle);
  Assert.IsTrue(cycle.HasProblems, "Döngü bildirilmeli.");

  var selfRef = new ValidationReport();
  CaseChainRules.Validate(new[] { new CaseData { id = "a", nextCaseId = "a" } }, config, selfRef);
  Assert.IsTrue(selfRef.HasProblems, "Kendine gönderen vaka bildirilmeli.");
 }

 [Test]
 public void CaseChain_AcceptsShippedContent() {
  var config = JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text);
  var cases = Resources.LoadAll<TextAsset>("Bube/Cases")
   .Select(asset => JsonUtility.FromJson<CaseData>(asset.text)).ToArray();
  var report = new ValidationReport();
  CaseChainRules.Validate(cases, config, report);
  Assert.IsFalse(report.HasProblems, report.Summary());
 }

 // Oyun içinde gerçek resmî kurum adı kullanılmaz; kurum kurgusaldır (bube Polis / BPS).
 [Test]
 public void RealInstitutionNames_AreRejected() {
  var locale = new Locale { entries = new[] {
   new Entry { key = "faks.gonderen", value = "Gönderen: İstanbul Emniyet Müdürlüğü" },
  } };
  var report = new ValidationReport();
  LocaleRules.Validate(locale, new CaseData[0], report);
  Assert.IsTrue(report.Problems.Any(p => p.Contains("faks.gonderen")),
   "Gerçek kurum adı bildirilmeli:\n" + report.Summary());
 }

 [Test]
 public void GenericWords_AreNotFalseAlarms() {
  // "birim", "müdür", "kurumsal" gibi genel sözcükler oyunda bilinçli kullanılıyor.
  foreach (var text in new[] {
   "İlgili birim, sunduğunuz sonuç ile dayanaklarını değerlendirdi.",
   "bube Polis · İstanbul / Beşiktaş Şubesi",
   "KURUMSAL DEĞERLENDİRME",
   "Birim güveni tükendi.",
  }) Assert.IsNull(LocaleRules.ForbiddenInstitution(text), "Yanlış alarm: " + text);
 }

 // DİKKAT: bu test yalnız `tr.json` metnini tarar. Görsellerin içine çizilmiş
 // yazıyı göremez — masa görselinin içine gömülü yazılar bunun dışındadır
 // (25 Eylül 2026'da bulundu). Görseller elle denetlenmelidir.
 [Test]
 public void ShippedText_UsesNoRealInstitutionName() {
  var config = JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text);
  var locale = JsonUtility.FromJson<Locale>(Resources.Load<TextAsset>("Bube/Locales/" + config.locale).text);
  var offenders = locale.entries
   .Where(e => LocaleRules.ForbiddenInstitution(e.value) != null)
   .Select(e => e.key + " → " + LocaleRules.ForbiddenInstitution(e.value)).ToArray();
  Assert.IsEmpty(offenders, "Gerçek kurum adı geçen metinler:\n" + string.Join("\n", offenders));
 }

 // Asıl içerik her koşuda bu yolla doğrulanır; tek çağrıda tüm vakalar gezilir.
 [Test]
 public void ShippedContent_HasNoProblems() {
  var report = ContentValidator.Run();
  Assert.IsFalse(report.HasProblems, report.Summary());
 }
}
}
