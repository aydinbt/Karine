using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #016 "Başkasının Adı" — kanalda ölen "Adrian Pell" aslında Lewis Hart'tır.
// Kredi dökümü, büro kartı ve kamera kimseyi adıyla anmaz; pasaport, e-posta, telefon ve
// DNA Craig'i anar. Parmak izi sonucu Leah'ya öne sürülebilmek için onun adını anmalı.
// Leah'yı, "sarhoş düştü"yü ya da "Adrian Pell"i seçen rapor desteklenmiş sayılmaz.
public static class Case016Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "loans", "badge", "towpath" })
   foreach (var person in new[] { "leah", "craig", "darren", "rachel", "edith" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "passport", "email", "phone", "dna" })
   report.Require(probe.MentionsPerson("craig", Body(src)), $"{src} Craig'i adıyla anmıyor.");
  report.Require(probe.MentionsPerson("leah", Body("prints")), "Parmak izi sonucu Leah'yı anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "craig" && how.id == "pushed" && after.id == "hart" && proof.id == "prints",
   "Doğru rapor: Craig + kanala itildi + Lewis Hart + parmak izi.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var leah = From(context);
  report.Require(Submit(leah, "leah", how.id, after.id) && leah.Career.pendingReviews[0].evaluationType != "supported",
   "Leah'yı seçen rapor doğru sayılıyor.");
  var drunk = From(context);
  report.Require(Submit(drunk, who.id, "drunk", after.id) && drunk.Career.pendingReviews[0].evaluationType != "supported",
   "'Sarhoş düştü' doğru sayılıyor.");
  var adrian = From(context);
  report.Require(Submit(adrian, who.id, how.id, "adrian") && adrian.Career.pendingReviews[0].evaluationType != "supported",
   "'Adrian Pell' doğru sayılıyor.");
 }
}
}
