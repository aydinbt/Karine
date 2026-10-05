using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #054 "Azulejo" — müteahhit, 2003 yangınını bilen çini ustasını alçı kalıpla öldürdü.
// Otopsi ve atölye incelemesi kimseyi adıyla anmaz; kamyonet araması ve 2003 dosyası Álvaro'yu anar.
public static class Case054Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "workshop" })
   foreach (var person in new[] { "alvaro", "rocio", "tomas", "nuria" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "van", "archive" })
   report.Require(probe.MentionsPerson("alvaro", Body(src)), $"{src} alvaro adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "rocio", "autopsy" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "tomas", "nuria" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("van"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "van");
    report.Require(inv.SubmitWarrant("van", new[] { "nuria", "autopsy" }, 0) && inv.WarrantDenied(N(data, "van")) && !inv.State.documentRequests.Any(r => r.nodeId == "van"), "van zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("van", new[] { "archive", "workshop" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "van"), "van doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "2003" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 2003 arşiv kaydı olarak yeniden açılmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "alvaro" && how.id == "mould" && after.id == "slip" && proof.id == "van", "Doğru rapor: alvaro + mould + slip + van.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "rocio", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: rocio");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "smoke", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: smoke");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "kiln") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: kiln");
 }
}
}
