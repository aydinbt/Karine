using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #056 "Guadalquivir" — kulüp başkanı, 2009 boğulmasını anlatacak antrenörün teknesini batırdı.
// Otopsi ve tekne incelemesi kimseyi adıyla anmaz; oda araması ve 2009 dosyası Ramón'u anar.
public static class Case056Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "boat" })
   foreach (var person in new[] { "ramon", "gonzalo", "prieto", "iker", "valeria", "hamid" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "office", "archive" })
   report.Require(probe.MentionsPerson("ramon", Body(src)), $"{src} ramon adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "iker", "autopsy" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "prieto", "boat" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("office"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "office");
    report.Require(inv.SubmitWarrant("office", new[] { "gonzalo", "autopsy" }, 0) && inv.WarrantDenied(N(data, "office")) && !inv.State.documentRequests.Any(r => r.nodeId == "office"), "office zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("office", new[] { "archive", "boat" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "office"), "office doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "2009" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 2009 arşiv kaydı olarak yeniden açılmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "ramon" && how.id == "plug" && after.id == "cup" && proof.id == "office", "Doğru rapor: ramon + plug + cup + office.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "gonzalo", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: gonzalo");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "cramp", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: cramp");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "river") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: river");
 }
}
}
