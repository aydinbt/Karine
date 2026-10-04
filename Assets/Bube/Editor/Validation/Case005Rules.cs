using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #005 "Son Görüldüğü Yer" — fail seçtirmeyen rapor. Vakanın iki kırılma
// anı (00.08 kaydı, emanet kaydı) kendiliğinden düşmemeli; oyuncunun sorduğu
// bir soruyla erişilebilir olmalı. "Saldırı yok" cevabı kayda dayanmalı.
public static class Case005Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot));
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 // Kamera kaynağı rapora kayıt noktasıyla yazılır ("parking_late#ece_leaves").
 static string Src(CaseData data, string id) { var n = N(data, id); return n.kind == "cctv" ? id + "#" + n.cctvEvents[0].id : id; }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;

  // Kırılma anları: soru sorulmadan kayıt açılmaz.
  var noLeft = From(context); noLeft.State.asked.Remove("case005.murat_third.left");
  report.Forbid(noLeft.Discovered(N(data, "parking_late")),
   "00.08 kaydı Murat'ın 'Siz çıktığınızda Ece neredeydi?' sorusu sorulmadan açılıyor.");
  var noLawyer = From(context); noLawyer.State.asked.Remove("case005.asli_follow.lawyer");
  report.Forbid(noLawyer.Discovered(N(data, "deposit")),
   "Emanet kaydı Aslı'nın avukat sorusu sorulmadan açılıyor.");

  var nature = data.verdicts.First(v => v.correct);
  var link = data.methods.First(m => m.correct);
  var attack = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(attack.supportingSourceIds.Contains("parking_late"),
   "'Saldırı yok' cevabı otopark ek dökümüyle desteklenmeli.");
  report.Require(nature.id == "voluntary" && link.id.StartsWith("murat_"),
   "Doğru rapor: kendi isteğiyle uzaklaşma + Murat bağlantısı.");

  var correct = From(context);
  if (!report.Step(correct.SubmitFinalReport(nature.id, link.id, proof.id,
   Src(data, nature.supportingSourceIds[0]), Src(data, link.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
   attack.id, Src(data, attack.supportingSourceIds[0])), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(7);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  // Murat'ı doğru seçip yanlış niteliği yüklemek de yanılgıdır.
  var wrongLink = From(context);
  report.Require(wrongLink.SubmitFinalReport(nature.id, "murat_held", proof.id,
    Src(data, nature.supportingSourceIds[0]), Src(data, link.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
    attack.id, Src(data, attack.supportingSourceIds[0])) && wrongLink.Career.pendingReviews[0].evaluationType != "supported",
   "'Murat zorla alıkoydu' doğru sayılıyor.");
  var wrongAttack = From(context);
  report.Require(wrongAttack.SubmitFinalReport(nature.id, link.id, proof.id,
    Src(data, nature.supportingSourceIds[0]), Src(data, link.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
    "murat_attack", Src(data, attack.supportingSourceIds[0])) && wrongAttack.Career.pendingReviews[0].evaluationType != "supported",
   "'Murat saldırdı' doğru sayılıyor.");
 }
}
}
