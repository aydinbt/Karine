using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #069 "Yüzen Ev" — marina sahibi, 2004 kirlenmesini yeniden kanıtlayan mühendisi kürekle öldürdü.
// Arşiv ve yem birlikte; kapı kaydı yemdir. Otopsi kimseyi adıyla anmaz.
public static class Case069Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy" })
   foreach (var person in new[] { "fenwick", "jarrah", "eloise", "quoc" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "boatshed", "gate", "archive" })
   report.Require(probe.MentionsPerson("fenwick", Body(src)), $"{src} fenwick adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "autopsy", "gate" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "quoc", "jarrah" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("boatshed"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "boatshed");
    report.Require(inv.SubmitWarrant("boatshed", new[] { "quoc", "autopsy" }, 0) && inv.WarrantDenied(N(data, "boatshed")) && !inv.State.documentRequests.Any(r => r.nodeId == "boatshed"), "boatshed zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("boatshed", new[] { "archive", "jarrah" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "boatshed"), "boatshed doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "2004" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 2004 arşiv kaydı olarak yeniden açılmalı.");
  { var q = N(data, "fenwick").questions.First(x => x.id == "case069.fenwick.end");
    report.Require(probe.DecoyAnswerKey(q, "gate") != null && !probe.SourceMatchesQuestion(q, "gate"), "fenwick.end: gate yem olmalı, çözmemeli."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "fenwick" && how.id == "oar" && after.id == "office" && proof.id == "boatshed", "Doğru rapor: fenwick + oar + office + boatshed.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "quoc", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: quoc");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "slip", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: slip");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "river") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: river");
 }
}
}
