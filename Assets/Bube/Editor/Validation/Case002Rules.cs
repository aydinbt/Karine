using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #002 "Son Sefer" — tek olay, iki ayrı eylem, iki ayrı sorumluluk.
// Yaralayan ile parayı alan aynı kişi değil; raporun dördüncü sütunu bunun
// için var. Bu kurallar o ayrımın veride korunduğunu doğrular.
public static class Case002Rules {

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report; var snapshot = context.ReadySnapshot;

  report.Require(data.nodes.Any(n => n.kind == "cctv"),
   "Dosya #002 kayıt dökümü olmadan çözülemez.");
  if (!report.Step((data.custody ?? new Choice[0]).Length > 0,
   "Dosya #002 dördüncü rapor sütununu tanımlamalı.")) return;

  var suspect = data.verdicts.First(v => v.correct);
  var method = data.methods.First(m => m.correct);
  var proof = data.evidence.First(e => e.correct);
  var taker = data.custody.First(c => c.correct);
  // İki sorumluluğun ayrı kalması vakanın **tasarımı**dır: aynı kişiye
  // çıkarsa dosya yeniden tek eylemli bir vakaya döner.
  report.Forbid(suspect.id == taker.id, "Yaralayan ile parayı alan aynı kişi olamaz.");

  var correctGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  if (!report.Step(correctGame.SubmitFinalReport(suspect.id, method.id, proof.id,
   suspect.supportingSourceIds[0], method.supportingSourceIds[0], proof.supportingSourceIds[0],
   taker.id, taker.supportingSourceIds[0]), "Desteklenen rapor reddedildi.")) return;
  correctGame.BeginNextCaseReview(7);
  correctGame.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correctGame.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported
   && fax.proofSupported && fax.custodySupported, "Kaynak değerlendirmesi başarısız: " + (fax == null ? "faks gelmedi"
    : fax.evaluationType + " fail=" + fax.suspectSupported + " yöntem=" + fax.methodSupported
      + " kanıt=" + fax.proofSupported + " para=" + fax.custodySupported));

  // Parayı yanlış kişiye yazmak, faili yanlış yazmakla aynı ağırlıktadır.
  var wrongTaker = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  report.Require(wrongTaker.SubmitFinalReport(suspect.id, method.id, proof.id,
    suspect.supportingSourceIds[0], method.supportingSourceIds[0], proof.supportingSourceIds[0],
    data.custody.First(c => !c.correct).id, taker.supportingSourceIds[0]) &&
   wrongTaker.Career.pendingReviews[0].evaluationType == "falseAccusation",
   "Yanlış suçlama işleyişi başarısız.");

  var wrongSuspect = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  report.Require(wrongSuspect.SubmitFinalReport(data.verdicts.First(v => !v.correct).id, method.id, proof.id,
    suspect.supportingSourceIds[0], method.supportingSourceIds[0], proof.supportingSourceIds[0],
    taker.id, taker.supportingSourceIds[0]) &&
   wrongSuspect.Career.pendingReviews[0].evaluationType == "falseAccusation",
   "Yanlış fail işleyişi başarısız.");
 }
}
}
