using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #051 "Procida" — feribot kaptanı denetim uzmanını küpeşteden itti, tableti can yeleği dolabına sakladı.
// Otopsi ve kıç kamerası kimseyi adıyla anmaz; kabin araması Gianni'yi anar. Yolcu kamera kaydı okununca İsveç'e döner.
public static class Case051Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "stern" })
   foreach (var person in new[] { "gianni", "tommaso", "holm" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "cabin" })
   report.Require(probe.MentionsPerson("gianni", Body(src)), $"{src} gianni adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("cabin"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "cabin");
    report.Require(inv.SubmitWarrant("cabin", new[] { "holm", "autopsy" }, 0) && inv.WarrantDenied(N(data, "cabin")) && !inv.State.documentRequests.Any(r => r.nodeId == "cabin"), "cabin zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("cabin", new[] { "stern", "tommaso" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "cabin"), "cabin doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("holm"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "holm"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "holm");
    foreach (var id in N(data, "holm").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "holm")), "holm kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "gianni" && how.id == "overboard" && after.id == "locker" && proof.id == "cabin", "Doğru rapor: gianni + overboard + locker + cabin.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "tommaso", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: tommaso");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "slip", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: slip");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "sea") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: sea");
 }
}
}
