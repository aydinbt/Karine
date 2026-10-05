using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #068 "Ahır" — antrenör, numune tüpünü geri vermeyen müfettişi nal çekiciyle öldürdü.
// Otopsi ve bölme incelemesi kimseyi adıyla anmaz; römork araması ve bahis dökümü Ainsley'i anar. Veteriner otopsi okununca ayrılır.
public static class Case068Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "stall" })
   foreach (var person in new[] { "ainsley", "rafferty", "keogh", "deng", "harriet", "sione", "mai" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "float", "bets" })
   report.Require(probe.MentionsPerson("ainsley", Body(src)), $"{src} ainsley adını anmıyor.");
  { var inv = From(context); inv.State.closedLines?.Clear(); inv.State.lineReopens?.Clear();
    inv.State.read.Remove("line_owner"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "line_owner");
    report.Require(inv.LineActive("line_stable") && inv.LineActive("line_bets"), "line_stable ve line_bets hatları açık değil.");
    report.Forbid(inv.CanRequestWarrant(N(data, "line_owner")), "Üçüncü hat yuvalar doluyken açılabiliyor.");
    report.Require(inv.CloseLine("line_stable") && !inv.LineActive("line_stable") && inv.CanRequestWarrant(N(data, "line_owner")), "Hat kapatılınca yuva boşalmıyor."); }
  { var inv = From(context); inv.State.read.Remove("harriet"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "harriet"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "harriet");
    foreach (var id in N(data, "harriet").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "harriet")), "harriet kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "ainsley" && how.id == "mallet" && after.id == "tube" && proof.id == "float", "Doğru rapor: ainsley + mallet + tube + float.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "sione", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: sione");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "kick", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: kick");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "lab") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: lab");
 }
}
}
