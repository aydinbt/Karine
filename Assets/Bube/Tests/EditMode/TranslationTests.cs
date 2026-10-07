using System.Collections.Generic;
using System.Linq;
using Bube.Editor;
using NUnit.Framework;

namespace Bube.Tests {

// Çeviri oyunu değiştirmez: ad kuralı kanondan hesaplanır, çeviri denetimi
// Türkçedeki kuralların aynısını uygular.
public sealed class TranslationTests {
 static Locale L(params (string k, string v)[] pairs) =>
  new Locale { entries = pairs.Select(p => new Entry { key = p.k, value = p.v }).ToArray() };

 static CaseData Case() => new CaseData { id = "case900", nodes = new[] {
  new Node { id = "hasan", kind = "interview", personId = "hasan", personNameKey = "case900.hasan.name" },
  new Node { id = "log", kind = "document", titleKey = "case900.log.title", bodyKey = "case900.log.body" } } };

 [Test]
 public void NameRule_ReadsCanon_NotDisplayedLanguage() {
  var data = Case();
  var canon = L(("case900.hasan.name", "Hasan Yıldız"), ("case900.log.body", "Hasan'ın kartı 21.04'te okundu."));
  var shown = L(("case900.hasan.name", "Hasan Yıldız"), ("case900.log.body", "The card was read at 21.04."));
  var game = new Investigation(data) { Text = shown, RuleText = canon };
  Assert.IsTrue(game.SourceConcernsPerson(data.nodes[0], null, game.RuleString("case900.log.body")));
  Assert.AreEqual("Hasan'ın kartı 21.04'te okundu.", game.RuleString("case900.log.body"));
 }

 [Test]
 public void Translation_BreakingCanonRules_IsReported() {
  var canon = new Dictionary<string, string> {
   ["case900.hasan.name"] = "Hasan Yıldız",
   ["case900.log.body"] = "Hasan'ın kartı {0}'te okundu.",
   ["case900.log.title"] = "Kart kaydı",
   ["case900.hasan.demeanor"] = "Gözünü masaya indirdi.",
  };
  var files = new List<KeyValuePair<string, Locale>> { new KeyValuePair<string, Locale>("en.case900.json", L(
   ("case900.hasan.name", "Hassan Yildiz"),
   ("case900.log.body", "The card was read by Mert."),
   ("case900.hasan.demeanor", "He lies, looking down."),
   ("case900.log.typo", "x"),
   ("case900.log.title", "Card log, sent to the FBI"))) };
  var report = new ValidationReport();
  TranslationRules.Validate("en", canon, files, new[] { Case() }, report);
  var all = string.Join("\n", report.Problems);
  StringAssert.Contains("Kişi adı çevrilmez", all);
  StringAssert.Contains("Yer tutucular", all);
  StringAssert.Contains("Kanonda geçen ad çeviride yok", all);
  StringAssert.Contains("hüküm", all);
  StringAssert.Contains("Kanonda olmayan anahtar", all);
  StringAssert.Contains("fbi", all);
 }

 [Test]
 public void FaithfulTranslation_Passes() {
  var canon = new Dictionary<string, string> { ["case900.hasan.name"] = "Hasan Yıldız", ["case900.log.body"] = "Hasanla {0}'te konuştu." };
  var files = new List<KeyValuePair<string, Locale>> { new KeyValuePair<string, Locale>("de.case900.json", L(
   ("case900.hasan.name", "Hasan Yıldız"), ("case900.log.body", "Sprach um {0} mit Hasan."))) };
  var report = new ValidationReport();
  TranslationRules.Validate("de", canon, files, new[] { Case() }, report);
  Assert.IsFalse(report.HasProblems, string.Join("\n", report.Problems));
 }
}
}
