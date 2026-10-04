using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #009 "Emanet" — soruşturma hatları. Aynı anda yalnız iki hat açık kalır;
// üçüncüsü için biri kapatılır, kapanan hat gecikmeyle yeniden açılır. Zayıf
// dayanaklı hat reddedilir. Kolay "borçlu memur" ve "meşru el koyma" raporları desteklenmez.
public static class Case009Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot));
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static string Src(CaseData data, string id) { var n = N(data, id); return n.kind == "cctv" ? id + "#" + n.cctvEvents[0].id : id; }

 static void Retry(CaseContext context, string node, string[] weak, string[] strong, string label) {
  var data = context.Data; var report = context.Report; var inv = From(context);
  inv.State.read.Remove(node); inv.State.documentRequests.RemoveAll(r => r.nodeId == node);
  inv.State.lineReopens.Clear();
  inv.State.closedLines = data.nodes.Where(n => Investigation.IsLine(n) && n.id != node).Select(n => n.id).ToList();
  report.Require(inv.SubmitWarrant(node, weak, 0) && inv.WarrantDenied(N(data, node)) && !inv.State.documentRequests.Any(r => r.nodeId == node),
   label + " zayıf dayanakla onaylanıyor.");
  report.Require(inv.SubmitWarrant(node, strong, 0) && inv.State.documentRequests.Any(r => r.nodeId == node),
   label + " doğru dayanakla onaylanmıyor.");
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  report.Require(data.lineSlots == 2 && data.grantsAuthority && !string.IsNullOrEmpty(data.envelopeBodyKey),
   "Dosya iki hat yuvası, iç denetim zarfı ve yetki genişlemesi taşımalı.");

  // Yuva sınırı: A ve B açıkken C açılamaz; biri kapatılınca açılır.
  var slots = From(context);
  slots.State.closedLines.Clear(); slots.State.lineReopens.Clear();
  slots.State.read.Remove("line_c"); slots.State.documentRequests.RemoveAll(r => r.nodeId == "line_c");
  report.Require(slots.LineActive("line_a") && slots.LineActive("line_b"), "A ve B hatları açık değil.");
  report.Forbid(slots.CanRequestWarrant(N(data, "line_c")), "Üçüncü hat yuvalar doluyken açılabiliyor.");
  report.Forbid(slots.CanRequestDocument(N(data, "c_radio")), "Açılmamış hattın kaydı istenebiliyor.");
  report.Require(slots.CloseLine("line_a") && !slots.LineActive("line_a") && slots.CanRequestWarrant(N(data, "line_c")),
   "Hat kapatılınca yuva açılmıyor.");
  slots.State.read.Remove("a_bank"); slots.State.documentRequests.RemoveAll(r => r.nodeId == "a_bank");
  report.Forbid(slots.CanRequestDocument(N(data, "a_bank")), "Kapalı hattın kaydı istenebiliyor.");
  report.Require(slots.SubmitWarrant("line_c", new[] { "op_report", "delivery_form" }, 0) && slots.ReceiveDocument("line_c"),
   "Yuva boşalınca üçüncü hat açılmıyor.");
  report.Forbid(slots.CanReopenLine(N(data, "line_a")), "Yuvalar doluyken kapalı hat yeniden açılabiliyor.");
  report.Require(slots.CloseLine("line_b") && slots.ReopenLine("line_a") && !slots.LineActive("line_a") && slots.LineReopening("line_a") != null,
   "Kapalı hat gecikmeli olarak yeniden açılmıyor.");

  Retry(context, "line_a", new[] { "op_report", "corridor" }, new[] { "delivery_form", "emanet_scan" }, "Arda hattı");
  Retry(context, "office", new[] { "b_syslog", "a_bank" }, new[] { "b_syslog", "b_keycard" }, "Amirlik araması");
  Retry(context, "link_e17", new[] { "office", "a_vehicle" }, new[] { "office", "c_photos" }, "27-118 bağlantısı");

  var first = data.verdicts.First(v => v.correct);
  var entry = data.methods.First(m => m.correct);
  var path = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(first.id == "hakan" && entry.id == "ceren" && path.id == "took_back" && proof.id == "lab",
   "Doğru rapor: Hakan + Ceren + geri alma + laboratuvar.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   Src(data, first.supportingSourceIds[0]), Src(data, entry.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
   c, Src(data, path.supportingSourceIds[0]));

  var correct = From(context);
  if (!report.Step(Submit(correct, first.id, entry.id, path.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var debt = From(context);
  report.Require(Submit(debt, first.id, entry.id, "arda_kept") && debt.Career.pendingReviews[0].evaluationType != "supported",
   "'Arda zimmetine geçirdi' doğru sayılıyor.");
  var legit = From(context);
  report.Require(Submit(legit, "legit", entry.id, path.id) && legit.Career.pendingReviews[0].evaluationType != "supported",
   "'Meşru el koyma' doğru sayılıyor.");
 }
}
}
