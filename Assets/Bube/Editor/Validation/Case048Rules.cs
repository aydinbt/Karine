using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #048 "Funicolare" — bakım şefi etiketi kaldırıp kabini teknisyenin üstüne sürdü.
// Otopsi ve kumanda odası kamerası kimseyi adıyla anmaz; dolap araması ve raporlar Marino'yu anar. Temizlikçi otopsi okununca ülkesine döner.
public static class Case048Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "control" })
   foreach (var person in new[] { "marino", "conti", "ilaria", "yusuf", "greta", "dario", "kofi" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "locker", "forms" })
   report.Require(probe.MentionsPerson("marino", Body(src)), $"{src} marino adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("locker"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "locker");
    report.Require(inv.SubmitWarrant("locker", new[] { "greta", "autopsy" }, 0) && inv.WarrantDenied(N(data, "locker")) && !inv.State.documentRequests.Any(r => r.nodeId == "locker"), "locker zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("locker", new[] { "control", "kofi" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "locker"), "locker doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("kofi"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "kofi"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "kofi");
    foreach (var id in N(data, "kofi").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "kofi")), "kofi kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "marino" && how.id == "cabin" && after.id == "bin" && proof.id == "locker", "Doğru rapor: marino + cabin + bin + locker.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "conti", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: conti");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fall", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fall");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "never") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: never");
 }
}
}
