using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Tüm içerik doğrulamasının tek girişi. Genel kurallar her vakaya uygulanır;
// vakaya özel iddialar aşağıdaki sözlükten gelir, böylece üçüncü vaka genel
// yolda hiçbir değişiklik gerektirmez. İlk sorunda durulmaz — bütün bulgular
// tek koşuda toplanır.
public static class ContentValidator {

 static readonly Dictionary<string, Action<CaseContext>> EarlyHooks = new Dictionary<string, Action<CaseContext>> {
  { "case001", Case001Rules.Early },
 };

 static readonly Dictionary<string, Action<CaseContext>> AfterWalkHooks = new Dictionary<string, Action<CaseContext>> {
  { "case001", Case001Rules.AfterWalk },
  { "case002", Case002Rules.AfterWalk },
  { "case005", Case005Rules.AfterWalk },
  { "case006", Case006Rules.AfterWalk },
  { "case007", Case007Rules.AfterWalk },
  { "case008", Case008Rules.AfterWalk },
  { "case009", Case009Rules.AfterWalk },
  { "case010", Case010Rules.AfterWalk },
  { "case011", Case011Rules.AfterWalk },
  { "case012", Case012Rules.AfterWalk },
  { "case013", Case013Rules.AfterWalk },
  { "case014", Case014Rules.AfterWalk },
  { "case015", Case015Rules.AfterWalk },
  { "case016", Case016Rules.AfterWalk },
  { "case017", Case017Rules.AfterWalk },
  { "case018", Case018Rules.AfterWalk },
  { "case019", Case019Rules.AfterWalk },
  { "case020", Case020Rules.AfterWalk },
  { "case021", Case021Rules.AfterWalk },
  { "case022", Case022Rules.AfterWalk },
  { "case023", Case023Rules.AfterWalk },
  { "case024", Case024Rules.AfterWalk },
  { "case025", Case025Rules.AfterWalk },
  { "case026", Case026Rules.AfterWalk },
  { "case027", Case027Rules.AfterWalk },
  { "case028", Case028Rules.AfterWalk },
  { "case029", Case029Rules.AfterWalk },
  { "case030", Case030Rules.AfterWalk },
  { "case031", Case031Rules.AfterWalk },
  { "case032", Case032Rules.AfterWalk },
  { "case033", Case033Rules.AfterWalk },
  { "case034", Case034Rules.AfterWalk },
  { "case035", Case035Rules.AfterWalk },
  { "case036", Case036Rules.AfterWalk },
  { "case037", Case037Rules.AfterWalk },
  { "case038", Case038Rules.AfterWalk },
  { "case039", Case039Rules.AfterWalk },
  { "case040", Case040Rules.AfterWalk },
  { "case041", Case041Rules.AfterWalk },
  { "case042", Case042Rules.AfterWalk },
  { "case043", Case043Rules.AfterWalk },
  { "case044", Case044Rules.AfterWalk },
  { "case045", Case045Rules.AfterWalk },
  { "case046", Case046Rules.AfterWalk },
  { "case047", Case047Rules.AfterWalk },
  { "case048", Case048Rules.AfterWalk },
  { "case049", Case049Rules.AfterWalk },
  { "case050", Case050Rules.AfterWalk },
  { "case051", Case051Rules.AfterWalk },
  { "case052", Case052Rules.AfterWalk },
  { "case053", Case053Rules.AfterWalk },
  { "case054", Case054Rules.AfterWalk },
  { "case055", Case055Rules.AfterWalk },
  { "case056", Case056Rules.AfterWalk },
  { "case057", Case057Rules.AfterWalk },
  { "case058", Case058Rules.AfterWalk },
  { "case059", Case059Rules.AfterWalk },
  { "case060", Case060Rules.AfterWalk },
  { "case061", Case061Rules.AfterWalk },
  { "case062", Case062Rules.AfterWalk },
  { "case063", Case063Rules.AfterWalk },
  { "case064", Case064Rules.AfterWalk },
  { "case065", Case065Rules.AfterWalk },
  { "case066", Case066Rules.AfterWalk },
 };

 public static ValidationReport Run() {
  var report = new ValidationReport();
  report.Scope("proje");
  ProjectRules.Validate(report);

  var config = JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text);
  var localeAsset = Resources.Load<TextAsset>(LocaleLoader.Folder + config.locale);
  if (localeAsset == null) { report.Problem("Dil dosyası yok: " + config.locale); return report; }
  // Ortak metin + vaka başına metin. Doğrulama hem birleşmiş hâli (anahtar var mı)
  // hem dosyaları ayrı ayrı (yinelenme, kurum adı) görmek zorunda.
  var localeFiles = new List<KeyValuePair<string, Locale>> {
   new KeyValuePair<string, Locale>(config.locale + ".json", JsonUtility.FromJson<Locale>(localeAsset.text)),
  };
  foreach (var asset in LocaleLoader.CaseAssets(config.locale))
   localeFiles.Add(new KeyValuePair<string, Locale>(asset.name + ".json", JsonUtility.FromJson<Locale>(asset.text)));
  var locale = LocaleLoader.Load(config.locale);

  var cases = Resources.LoadAll<TextAsset>("Bube/Cases")
   .Select(asset => JsonUtility.FromJson<CaseData>(asset.text)).ToList();

  report.Scope("zincir");
  CaseChainRules.Validate(cases, config, report);
  report.Scope("dil");
  LocaleRules.Validate(localeFiles, cases, report);
  WorldRules.Validate(report, locale);

  foreach (var data in cases) {
   report.Scope(data.id);
   ValidateCase(data, locale, report);
  }
  report.Scope(null);
  return report;
 }

 static void ValidateCase(CaseData data, Locale locale, ValidationReport report) {
  int before = report.Problems.Count;
  CaseRules.ValidateStructure(data, locale, report);
  // Yapı bozuksa gezinti yanıltıcı ikincil hatalar üretir; orada kesilir ama
  // öteki vakalar yine doğrulanır.
  if (report.Problems.Count != before) return;

  var game = new Investigation(data);
  if (!WalkRules.BeforeAccept(data, game, report)) return;

  var context = new CaseContext { Data = data, Locale = locale, Game = game, Report = report };
  if (EarlyHooks.TryGetValue(data.id, out var early)) early(context);

  WalkRules.Walk(data, game, report);
  if (report.Problems.Count != before) return;

  WalkRules.AfterWalk(data, locale, game, report);
  context.ReadySnapshot = JsonUtility.ToJson(JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State)));
  if (AfterWalkHooks.TryGetValue(data.id, out var after)) after(context);
 }
}
}
