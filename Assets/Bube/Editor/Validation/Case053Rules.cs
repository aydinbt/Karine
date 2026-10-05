using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #053 "Tablao" — İspanya açılışı. Tablao sahibi, 1994 ifadesini geri çekecek gitaristi boğdu.
// Otopsi kimseyi adıyla anmaz; kasa araması ve 1994 dosyası Joaquín'i anar. 1994 dosyası gerekçeli arşiv talebiyle açılır.
public static class Case053Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy" })
   foreach (var person in new[] { "joaquin", "triana", "curro", "montoya", "pilar", "mateo", "kaya" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "safe", "archive" })
   report.Require(probe.MentionsPerson("joaquin", Body(src)), $"{src} joaquin adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "triana", "mateo" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "curro", "pilar" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("safe"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "safe");
    report.Require(inv.SubmitWarrant("safe", new[] { "triana", "autopsy" }, 0) && inv.WarrantDenied(N(data, "safe")) && !inv.State.documentRequests.Any(r => r.nodeId == "safe"), "safe zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("safe", new[] { "archive", "kaya" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "safe"), "safe doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "1994" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 1994 arşiv kaydı olarak yeniden açılmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "joaquin" && how.id == "string" && after.id == "safe" && proof.id == "safe", "Doğru rapor: joaquin + string + safe + safe.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "triana", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: triana");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "self", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: self");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "river") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: river");
 }
}
}
