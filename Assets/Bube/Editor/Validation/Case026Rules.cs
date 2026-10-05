using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #026 "Kintsugi" — usta fırın kulübesinde karbonmonoksitten öldü; çırak cesedi taşıyıp hırsızlık sahneledi.
// Otopsi, kan gazı ve kamera kimseyi adıyla anmaz; iz incelemesi ve kulübe araması Aoi'yi anar.
public static class Case026Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "co", "path" })
   foreach (var person in new[] { "aoi", "emi", "fujita", "kuroda", "okabe", "shimizu" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "drag", "shed" })
   report.Require(probe.MentionsPerson("aoi", Body(src)), $"{src} aoi adını anmıyor.");
  foreach (var src in new[] { "xrf" })
   report.Require(probe.MentionsPerson("shimizu", Body(src)), $"{src} shimizu adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "nobody" && how.id == "co" && after.id == "seiji" && proof.id == "co", "Doğru rapor: nobody + co + seiji + co.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "aoi", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: aoi");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "blow", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: blow");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "fujita") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fujita");
 }
}
}
