using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #013 "Kiracı" — bakım kayıtları: 9 Mart'ta temiz olan baca 23 Mart'ta
// tıkalı; aradaki tek kayıtlı giriş Simon'ın. Olay yeri notu, kamera ve kira
// defteri Simon'ın adını anmaz; adını taşıyan anahtar kaydı ve Ellie'nin
// mesajlarıdır. Victor'ı, "ihmal"i ya da hedef olarak Martin'i seçen rapor
// desteklenmiş sayılmaz.
public static class Case013Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var id in new[] { "scene", "alley", "ledger", "service" })
   report.Forbid(probe.MentionsPerson("simon", Body(id)), $"{id} Simon'ın adını anıyor; anahtar kaydı gereksizleşiyor.");
  report.Require(probe.MentionsPerson("simon", Body("keylog")), "Anahtar kaydı Simon'ın adını anmıyor.");
  report.Require(probe.MentionsPerson("simon", Body("messages")), "Mesajlar Simon'ın adını anmıyor.");
  report.Require(probe.MentionsPerson("ellie", Body("tenants")), "Kiracı listesi Ellie'yi anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "simon" && how.id == "blocked" && after.id == "ellie" && proof.id == "keylog",
   "Doğru rapor: Simon + tıkanmış baca + hedef Ellie + anahtar kaydı.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var victor = From(context);
  report.Require(Submit(victor, "victor", how.id, after.id) && victor.Career.pendingReviews[0].evaluationType != "supported",
   "Victor'ı seçen rapor doğru sayılıyor.");
  var neglect = From(context);
  report.Require(Submit(neglect, who.id, "neglect", after.id) && neglect.Career.pendingReviews[0].evaluationType != "supported",
   "'İhmal' doğru sayılıyor.");
  var martin = From(context);
  report.Require(Submit(martin, who.id, how.id, "martin") && martin.Career.pendingReviews[0].evaluationType != "supported",
   "Hedef olarak Martin doğru sayılıyor.");
 }
}
}
