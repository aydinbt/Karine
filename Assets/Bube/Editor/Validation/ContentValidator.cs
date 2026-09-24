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
 };

 public static ValidationReport Run() {
  var report = new ValidationReport();
  report.Scope("proje");
  ProjectRules.Validate(report);

  var config = JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text);
  var localeAsset = Resources.Load<TextAsset>("Bube/Locales/" + config.locale);
  if (localeAsset == null) { report.Problem("Dil dosyası yok: " + config.locale); return report; }
  var locale = JsonUtility.FromJson<Locale>(localeAsset.text);

  var cases = Resources.LoadAll<TextAsset>("Bube/Cases")
   .Select(asset => JsonUtility.FromJson<CaseData>(asset.text)).ToList();

  report.Scope("zincir");
  CaseChainRules.Validate(cases, config, report);
  report.Scope("dil");
  LocaleRules.Validate(locale, cases, report);

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
