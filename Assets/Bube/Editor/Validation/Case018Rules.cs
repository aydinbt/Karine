using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #018 "Birinci Kat" — tanık "bir alt kattan" dedi, tercüman eski kiracı listesine bakıp
// "zemin kat" yazdı. Uygulama kaydı, kiracı listesi, kamera ve otopsi kimseyi adıyla anmaz;
// kapı kamerası ve çöp araması Markus'u anar. Uwe'yi, "düştü"yü ya da zemin katı seçen rapor desteklenmez.
public static class Case018Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "app", "tenants", "yard", "autopsy" })
   foreach (var person in new[] { "ayten", "uwe", "markus", "lena", "selin" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "doorcam", "bin" })
   report.Require(probe.MentionsPerson("markus", Body(src)), $"{src} markus adını anmıyor.");
  foreach (var src in new[] { "shelter", "bin" })
   report.Require(probe.MentionsPerson("lena", Body(src)), $"{src} lena adını anmıyor.");
  foreach (var src in new[] { "translation", "audio" })
   report.Require(probe.MentionsPerson("ayten", Body(src)), $"{src} ayten adını anmıyor.");
  foreach (var src in new[] { "translation", "audio" })
   report.Require(probe.MentionsPerson("selin", Body(src)), $"{src} selin adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "markus" && how.id == "pushed" && after.id == "first" && proof.id == "audio", "Doğru rapor: markus + pushed + first + audio.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "uwe", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: uwe");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fell", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fell");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "ground") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: ground");
 }
}
}
