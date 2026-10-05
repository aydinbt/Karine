using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #050 "Sipario" — sahne amiri ip ustasını uzaklaştırıp kum torbasını baritonun üstüne bıraktı.
// Askı incelemesi kimseyi adıyla anmaz; masa araması ve replik defteri Nicola'yı anar. Misafir şef askı incelemesi okununca Tokyo'ya döner.
public static class Case050Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "rigging" })
   foreach (var person in new[] { "nicola", "sofia", "amedeo", "chiara", "kenji", "pasquale" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "desk", "cues" })
   report.Require(probe.MentionsPerson("nicola", Body(src)), $"{src} nicola adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("desk"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "desk");
    report.Require(inv.SubmitWarrant("desk", new[] { "sofia", "pasquale" }, 0) && inv.WarrantDenied(N(data, "desk")) && !inv.State.documentRequests.Any(r => r.nodeId == "desk"), "desk zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("desk", new[] { "rigging", "cues" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "desk"), "desk doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("kenji"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "kenji"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "kenji");
    foreach (var id in N(data, "kenji").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "kenji")), "kenji kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "nicola" && how.id == "sandbag" && after.id == "drawer" && proof.id == "desk", "Doğru rapor: nicola + sandbag + drawer + desk.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "amedeo", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: amedeo");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "forgot", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: forgot");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "never") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: never");
 }
}
}
