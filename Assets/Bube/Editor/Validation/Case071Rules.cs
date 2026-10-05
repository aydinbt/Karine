using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #071 "Zaman Topu" — liman başkanı, manifestoları bulan arşivciyi gevşetilmiş korkuluktan düşürdü.
// Yem ve ayrılan tanık birlikte; telsiz kaydı yemdir, rehber kule incelemesi okununca susar.
public static class Case071Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "tower" })
   foreach (var person in new[] { "lachlan", "ruby", "bao" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "office", "radio" })
   report.Require(probe.MentionsPerson("lachlan", Body(src)), $"{src} lachlan adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("office"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "office");
    report.Require(inv.SubmitWarrant("office", new[] { "radio", "autopsy" }, 0) && inv.WarrantDenied(N(data, "office")) && !inv.State.documentRequests.Any(r => r.nodeId == "office"), "office zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("office", new[] { "bao", "tower" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "office"), "office doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "lachlan").questions.First(x => x.id == "case071.lachlan.end");
    report.Require(probe.DecoyAnswerKey(q, "radio") != null && !probe.SourceMatchesQuestion(q, "radio"), "lachlan.end: radio yem olmalı, çözmemeli."); }
  { var inv = From(context); inv.State.read.Remove("ruby"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "ruby"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "ruby");
    foreach (var id in N(data, "ruby").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "ruby")), "ruby kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "lachlan" && how.id == "rail" && after.id == "office" && proof.id == "office", "Doğru rapor: lachlan + rail + office + office.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "ruby", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: ruby");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "rot", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: rot");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "sea") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: sea");
 }
}
}
