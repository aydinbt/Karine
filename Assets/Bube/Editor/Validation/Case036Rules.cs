using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #036 "Mahzen" — kilitli mahzenden şarap asansörüyle çıkan sommelier.
// Otopsi ve koridor kamerası kimseyi adıyla anmaz; asansör incelemesi, kâhya ve depo araması Petit'yi anar. Depo gerekçeli izinle açılır.
public static class Case036Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "cellar" })
   foreach (var person in new[] { "helene", "gaspard", "petit", "pinto", "caron", "doyle", "brassard" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "liftlog", "storage" })
   report.Require(probe.MentionsPerson("petit", Body(src)), $"{src} petit adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("storage"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "storage");
    report.Require(inv.SubmitWarrant("storage", new[] { "gaspard", "autopsy" }, 0) && inv.WarrantDenied(N(data, "storage")) && !inv.State.documentRequests.Any(r => r.nodeId == "storage"), "storage zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("storage", new[] { "caron", "liftlog" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "storage"), "storage doğru dayanakla onaylanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "petit" && how.id == "magnum" && after.id == "lift" && proof.id == "storage", "Doğru rapor: petit + magnum + lift + storage.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "gaspard", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: gaspard");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fall", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fall");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "door") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: door");
 }
}
}
