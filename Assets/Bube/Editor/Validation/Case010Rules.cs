using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #010 "Kırık Zincir" — kapsam genişletme, olay rekonstrüksiyonu ve bölüm finali.
// Eksik rekonstrüksiyon raporu kilitler; yanlış sıra desteklenmez. Kolay "Serkan
// öldürdü" ve "Nehir öldürdü" raporları desteklenmez.
public static class Case010Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot));
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static string Src(CaseData data, string id) { var n = N(data, id); return n.kind == "cctv" ? id + "#" + n.cctvEvents[0].id : id; }

 static void Fill(Investigation inv, CaseData data, System.Collections.Generic.IEnumerable<ReconCard> order) {
  inv.Recon.Clear();
  foreach (var c in order) { inv.ReconPlace(c.id); inv.ReconSource(c.id, Src(data, c.supportingSourceIds[0])); }
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  report.Require(data.lineSlots == 2 && data.reconstruction != null && data.reconstruction.Length == 7 && data.chapterFinale != null,
   "Dosya iki hat yuvası, yedi rekonstrüksiyon kartı ve bölüm finali taşımalı.");

  // Kapsam: zayıf dayanakla yangın kaydı dosyaya alınmaz.
  var scope = From(context);
  scope.State.read.Remove("fire_link"); scope.State.documentRequests.RemoveAll(r => r.nodeId == "fire_link");
  report.Require(scope.SubmitWarrant("fire_link", new[] { "event_fire", "scene" }, 0) && scope.WarrantDenied(N(data, "fire_link")),
   "Araç yangını zayıf dayanakla dosyaya alınıyor.");
  report.Require(scope.SubmitWarrant("fire_link", new[] { "event_fire", "nehir" }, 0) && scope.State.documentRequests.Any(r => r.nodeId == "fire_link"),
   "Araç yangını doğru dayanakla dosyaya alınmıyor.");

  var first = data.verdicts.First(v => v.correct);
  var entry = data.methods.First(m => m.correct);
  var path = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(first.id == "pelin" && entry.id == "nehir" && path.id == "restaurant" && proof.id == "restaurant",
   "Doğru rapor: Pelin + Nehir + lokanta + lokanta kaydı.");

  bool Submit(Investigation inv, string s, string m) => inv.SubmitFinalReport(s, m, proof.id,
   Src(data, first.supportingSourceIds[0]), Src(data, entry.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
   path.id, Src(data, path.supportingSourceIds[0]));

  var empty = From(context); empty.Recon.Clear();
  report.Forbid(Submit(empty, first.id, entry.id), "Rekonstrüksiyon boşken rapor gönderilebiliyor.");

  var correct = From(context); Fill(correct, data, data.reconstruction);
  report.Require(correct.ReconComplete && correct.ReconSupported, "Doğru sıra ve kaynaklar desteklenmiyor.");
  if (!report.Step(Submit(correct, first.id, entry.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.hasRecon && fax.reconSupported, "Doğru rapor faksta desteklenmedi.");

  var swapped = From(context);
  var order = data.reconstruction.ToList(); (order[3], order[4]) = (order[4], order[3]);
  Fill(swapped, data, order);
  report.Require(swapped.ReconComplete && !swapped.ReconSupported, "Yanlış sıra destekleniyor.");
  report.Require(Submit(swapped, first.id, entry.id) && swapped.Career.pendingReviews[0].evaluationType != "supported",
   "Yanlış sıralı rekonstrüksiyon doğru sayılıyor.");

  foreach (var wrong in new[] { "serkan", "nehir" }) {
   var inv = From(context); Fill(inv, data, data.reconstruction);
   report.Require(Submit(inv, wrong, entry.id) && inv.Career.pendingReviews[0].evaluationType != "supported",
    $"'{wrong} öldürdü' doğru sayılıyor.");
  }
 }
}
}
