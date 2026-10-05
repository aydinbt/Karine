using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #014 "İkinci Görüş" — iki dürüst uzman raporu çelişir çünkü iki ayrı
// tabloya bakılmıştır. İkinci görüş, iki rapor ve avlu kamerası Ashby'nin
// adını anmaz; onu adıyla gösteren giriş kaydı, telefon, fatura ve defterin
// grafit sayfasıdır. Rebecca'yı, "intihar"ı ya da "müzayededeki tablo sahte"yi
// seçen rapor desteklenmiş sayılmaz.
public static class Case014Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var id in new[] { "second", "hugh_report", "clara_report", "yard" })
   report.Forbid(probe.MentionsPerson("julian", Body(id)), $"{id} Ashby'nin adını anıyor.");
  foreach (var id in new[] { "entry", "cphone", "invoice", "notebook" })
   report.Require(probe.MentionsPerson("julian", Body(id)), $"{id} Ashby'nin adını anmıyor; ona öne sürülemez.");
  report.Require(probe.MentionsPerson("noah", Body("framer")), "Çerçeveci kaydı Noah'yı anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "julian" && how.id == "poisoned" && after.id == "swapped" && proof.id == "second",
   "Doğru rapor: Ashby + düzenlenmiş zehirleme + değiştirilen kopya + ikinci görüş.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var rebecca = From(context);
  report.Require(Submit(rebecca, "rebecca", how.id, after.id) && rebecca.Career.pendingReviews[0].evaluationType != "supported",
   "Rebecca'yı seçen rapor doğru sayılıyor.");
  var suicide = From(context);
  report.Require(Submit(suicide, who.id, "suicide", after.id) && suicide.Career.pendingReviews[0].evaluationType != "supported",
   "'İntihar' doğru sayılıyor.");
  var auction = From(context);
  report.Require(Submit(auction, who.id, how.id, "auction") && auction.Career.pendingReviews[0].evaluationType != "supported",
   "'Müzayededeki tablo sahte' doğru sayılıyor.");
 }
}
}
