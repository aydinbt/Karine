using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #002 (taslak) — arşiv kayıtlarıyla çalışır, CCTV kullanmaz.
public static class Case002Rules {

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report; var snapshot = context.ReadySnapshot;

  report.Require(!data.nodes.Any(n => n.kind == "cctv") && data.nodes.Any(n => n.kind == "bps"),
   "Dosya #002 CCTV yerine arşiv kaydı kullanmalı.");

  var proof = data.evidence.First(e => e.correct);
  var suspect = data.verdicts.First(v => v.correct);
  var method = data.methods.First(m => m.correct);

  var correctGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  if (!report.Step(correctGame.SubmitFinalReport(suspect.id, method.id, proof.id, "parcel", "access", "parcel"),
   "Desteklenen rapor reddedildi.")) return;
  correctGame.BeginNextCaseReview(7);
  correctGame.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correctGame.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.proofSupported,
   "Kaynak değerlendirmesi başarısız.");

  var falseGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  report.Require(falseGame.SubmitFinalReport("deniz", method.id, proof.id, "access", "access", "parcel") &&
   falseGame.Career.pendingReviews[0].evaluationType == "falseAccusation",
   "Yanlış suçlama işleyişi başarısız.");
 }
}
}
