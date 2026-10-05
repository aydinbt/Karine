using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #058 "Kayar Raflar" — restoratör, 2015 harita hırsızlığını bulan arşivciyi rafların arasında ezdi.
// Otopsi, depo ve kamera kimseyi adıyla anmaz; atölye araması ve 2015 dosyası Sergio'yu anar.
public static class Case058Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "stack", "corridor" })
   foreach (var person in new[] { "sergio", "aurora", "fermin", "naomi", "blanca", "osman", "candela" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "workshop", "archive" })
   report.Require(probe.MentionsPerson("sergio", Body(src)), $"{src} sergio adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "fermin", "autopsy" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "candela", "naomi" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("workshop"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "workshop");
    report.Require(inv.SubmitWarrant("workshop", new[] { "fermin", "autopsy" }, 0) && inv.WarrantDenied(N(data, "workshop")) && !inv.State.documentRequests.Any(r => r.nodeId == "workshop"), "workshop zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("workshop", new[] { "archive", "corridor" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "workshop"), "workshop doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "2015" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 2015 arşiv kaydı olarak yeniden açılmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "sergio" && how.id == "shelf" && after.id == "press" && proof.id == "workshop", "Doğru rapor: sergio + shelf + press + workshop.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "fermin", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fermin");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fall", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fall");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "bin") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: bin");
 }
}
}
