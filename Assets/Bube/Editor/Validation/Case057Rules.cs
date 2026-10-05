using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #057 "Portakal Bahçesi" — kooperatif müdürü, 1999 kazasını anlatacak toplayıcıyı termosundan zehirledi.
// Otopsi ve kamera kimseyi adıyla anmaz; araba araması ve 1999 dosyası Beatriz'i anar.
public static class Case057Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "canteen" })
   foreach (var person in new[] { "beatriz", "esteban", "dolores", "karim", "anabel" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "car", "archive" })
   report.Require(probe.MentionsPerson("beatriz", Body(src)), $"{src} beatriz adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "karim", "autopsy" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "dolores", "esteban" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("car"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "car");
    report.Require(inv.SubmitWarrant("car", new[] { "karim", "autopsy" }, 0) && inv.WarrantDenied(N(data, "car")) && !inv.State.documentRequests.Any(r => r.nodeId == "car"), "car zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("car", new[] { "archive", "canteen" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "car"), "car doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "1999" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 1999 arşiv kaydı olarak yeniden açılmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "beatriz" && how.id == "vial" && after.id == "carpocket" && proof.id == "car", "Doğru rapor: beatriz + vial + carpocket + car.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "esteban", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: esteban");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "spray", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: spray");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "bin") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: bin");
 }
}
}
