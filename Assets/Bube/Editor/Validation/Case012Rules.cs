using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #012 "Son Sefer Değil" — biniş dökümünden zaman çizelgesi. Döküm ve
// sokak kamerası kimsenin adını anmaz; ·4417 kartını Liam'a bağlayan banka
// hareketleridir. Kieran'ın kart dökümü onu aklar. Helen'ı gösteren Owen'ın
// taslağı ve Liam'ın telefon kaydıdır. Kieran'ı, "yalnız kin"i ya da "Liam tek
// başına"yı seçen rapor desteklenmiş sayılmaz.
public static class Case012Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var p in new[] { "liam", "kieran", "helen" })
   report.Forbid(probe.MentionsPerson(p, Body("taps")), $"Biniş dökümü {p} adını anıyor; döküm adsız olmalı.");
  report.Forbid(probe.MentionsPerson("liam", Body("street")), "Sokak kamerası Liam'ın adını anıyor.");
  report.Require(probe.MentionsPerson("liam", Body("bank")), "Banka hareketleri Liam'ın adını anmıyor.");
  report.Require(probe.MentionsPerson("kieran", Body("kieran_card")), "Kart dökümü Kieran'ın adını anmıyor.");
  report.Require(probe.MentionsPerson("kieran", Body("report")), "Olay raporu Kieran'ı anmıyor; yanlış iz kurulmuyor.");
  report.Require(probe.MentionsPerson("helen", Body("lphone")), "Liam'ın telefon kaydı Helen'ın adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "liam" && how.id == "incited" && after.id == "helen" && proof.id == "bank",
   "Doğru rapor: Liam + kışkırtma + Helen + biniş dökümü ve banka hareketleri.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var kieran = From(context);
  report.Require(Submit(kieran, "kieran", how.id, after.id) && kieran.Career.pendingReviews[0].evaluationType != "supported",
   "Kieran'ı seçen rapor doğru sayılıyor.");
  var grudge = From(context);
  report.Require(Submit(grudge, who.id, "grudge", after.id) && grudge.Career.pendingReviews[0].evaluationType != "supported",
   "'Yalnız kin' doğru sayılıyor.");
  var alone = From(context);
  report.Require(Submit(alone, who.id, how.id, "none") && alone.Career.pendingReviews[0].evaluationType != "supported",
   "'Liam tek başına' doğru sayılıyor.");
 }
}
}
