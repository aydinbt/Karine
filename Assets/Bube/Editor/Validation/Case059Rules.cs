using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #059 "Santa Ana" — İspanya bölüm finali. Müdür yardımcısı vakıf direktörünü sarnıca itti.
// Otopsi ve avlu kamerası kimseyi adıyla anmaz; oda araması ve 1989 dosyası Mercedes'i anar.
public static class Case059Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static void Fill(Investigation inv, System.Collections.Generic.IEnumerable<ReconCard> order) {
  inv.Recon.Clear();
  foreach (var c in order) { inv.ReconPlace(c.id); inv.ReconSource(c.id, c.supportingSourceIds[0]); }
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "patio" })
   foreach (var person in new[] { "mercedes", "amaia", "isidro", "bermejo" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "office" })
   report.Require(probe.MentionsPerson("mercedes", Body(src)), $"{src} mercedes adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "autopsy", "patio" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "isidro", "bermejo" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("office"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "office");
    report.Require(inv.SubmitWarrant("office", new[] { "amaia", "autopsy" }, 0) && inv.WarrantDenied(N(data, "office")) && !inv.State.documentRequests.Any(r => r.nodeId == "office"), "office zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("office", new[] { "archive", "patio" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "office"), "office doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "1989" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 1989 arşiv kaydı olarak yeniden açılmalı.");
  report.Require(data.reconstruction != null && data.reconstruction.Length == 7, "Rekonstrüksiyon 7 kart olmalı.");
  report.Require(data.chapterFinale != null && data.chapterFinale.nextFileKey == "finale.file.060", "Bölüm finali finale.file.060 açmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "mercedes" && how.id == "cistern" && after.id == "drawer" && proof.id == "office", "Doğru rapor: mercedes + cistern + drawer + office.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var empty = From(context); empty.Recon.Clear();
  report.Forbid(Submit(empty, who.id, how.id, after.id), "Rekonstrüksiyon boşken rapor gönderilebiliyor.");
  var correct = From(context); Fill(correct, data.reconstruction);
  report.Require(correct.ReconComplete && correct.ReconSupported, "Doğru sıra ve kaynaklar desteklenmiyor.");
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported && fax.reconSupported, "Doğru rapor faksta desteklenmedi.");
  var swapped = From(context);
  var order = data.reconstruction.ToList(); (order[2], order[3]) = (order[3], order[2]);
  Fill(swapped, order);
  report.Require(Submit(swapped, who.id, how.id, after.id) && swapped.Career.pendingReviews[0].evaluationType != "supported", "Yanlış sıralı rekonstrüksiyon doğru sayılıyor.");
  var w0 = From(context); Fill(w0, data.reconstruction);
  report.Require(Submit(w0, "amaia", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: amaia");
  var w1 = From(context); Fill(w1, data.reconstruction);
  report.Require(Submit(w1, who.id, "slip", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: slip");
  var w2 = From(context); Fill(w2, data.reconstruction);
  report.Require(Submit(w2, who.id, how.id, "well") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: well");
 }
}
}
