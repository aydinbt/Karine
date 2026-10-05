using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #072 "Pho" — imar müdürü, son parseli satmayan dükkân sahibine karides ezmesi verdi.
// Arşiv, yem ve ayrılan tanık birlikte. Kasa fişi yemdir; garson otopsi okununca gider.
public static class Case072Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy" })
   foreach (var person in new[] { "sinclair", "hoang", "kalani", "vy", "duncan" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "car", "receipt", "archive" })
   report.Require(probe.MentionsPerson("sinclair", Body(src)), $"{src} sinclair adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "autopsy", "receipt" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "hoang", "kalani" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("car"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "car");
    report.Require(inv.SubmitWarrant("car", new[] { "hoang", "autopsy" }, 0) && inv.WarrantDenied(N(data, "car")) && !inv.State.documentRequests.Any(r => r.nodeId == "car"), "car zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("car", new[] { "archive", "duncan" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "car"), "car doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "2012" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 2012 arşiv kaydı olarak yeniden açılmalı.");
  { var q = N(data, "sinclair").questions.First(x => x.id == "case072.sinclair.end");
    report.Require(probe.DecoyAnswerKey(q, "receipt") != null && !probe.SourceMatchesQuestion(q, "receipt"), "sinclair.end: receipt yem olmalı, çözmemeli."); }
  { var inv = From(context); inv.State.read.Remove("vy"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "vy"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "vy");
    foreach (var id in N(data, "vy").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "vy")), "vy kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "sinclair" && how.id == "paste" && after.id == "car" && proof.id == "car", "Doğru rapor: sinclair + paste + car + car.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "hoang", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: hoang");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "cross", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: cross");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "bin") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: bin");
 }
}
}
