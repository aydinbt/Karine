using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #007 "Kül Payı" — inceleme izni. Araç, kart ve servis koridoru ancak
// yeterli dayanakla açılır; zayıf dayanak reddedilir. Rapor üç katmanlı:
// darbe Serdar'ın, yangın kasıtlı, ilk amaç sigorta. "Düştü" ve "delilleri
// örtmek" kolay açıklamaları desteklenmiş sayılmaz.
public static class Case007Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot));
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static string Src(CaseData data, string id) { var n = N(data, id); return n.kind == "cctv" ? id + "#" + n.cctvEvents[0].id : id; }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;

  // Zayıf dayanak: plaka kaydı + Serdar'ın konum kaydı aracı Serdar'a bağlamaz.
  var weak = From(context);
  weak.State.read.Remove("vehicle"); weak.State.documentRequests.RemoveAll(r => r.nodeId == "vehicle");
  report.Require(weak.SubmitWarrant("vehicle", new[] { "plate", "sphone" }, 0) && weak.WarrantDenied(N(data, "vehicle"))
   && !weak.State.documentRequests.Any(r => r.nodeId == "vehicle"), "Araç izni zayıf dayanakla (plaka + konum) onaylanıyor.");
  report.Require(weak.SubmitWarrant("vehicle", new[] { "plate", "street#car_stop" }, 0) && weak.State.documentRequests.Any(r => r.nodeId == "vehicle")
   && weak.Denial(N(data, "vehicle")) == null, "Ret sonrası doğru dayanakla yeni talep onaylanmıyor.");

  var first = data.verdicts.First(v => v.correct);
  var nature = data.methods.First(m => m.correct);
  var purpose = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(first.id == "serdar_blow" && nature.id == "deliberate" && purpose.id == "insurance",
   "Doğru rapor: Serdar darbe + kasıtlı + sigorta.");

  bool Submit(Investigation inv, string s, string c) => inv.SubmitFinalReport(s, nature.id, proof.id,
   Src(data, first.supportingSourceIds[0]), Src(data, nature.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
   c, Src(data, purpose.supportingSourceIds[0]));

  var correct = From(context);
  if (!report.Step(Submit(correct, first.id, purpose.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var fall = From(context);
  report.Require(Submit(fall, "serdar_fall", purpose.id) && fall.Career.pendingReviews[0].evaluationType != "supported",
   "'Tartışmada düştü' doğru sayılıyor.");
  var cover = From(context);
  report.Require(Submit(cover, first.id, "cover") && cover.Career.pendingReviews[0].evaluationType != "supported",
   "'Delilleri yok etmek' doğru sayılıyor.");
 }
}
}
