using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #011 "Sis Altında" — Birleşik Krallık'ın ilk dosyası. Merkezde özet ile
// döküm: devriye özeti "Daniel adı geçti" der, tam döküm bağıranın Thomas
// olduğunu gösterir. Kart kaydı Sam'in adını anmaz; geçersiz kartı Sam'e
// bağlayan kart listesidir. Daniel'i ya da "tek başına düştü"yü seçen rapor
// desteklenmiş sayılmaz.
public static class Case011Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  report.Require(probe.MentionsPerson("daniel", Body("note")), "Tutanak özeti Daniel'in adını anmıyor; yanlış iz kurulmuyor.");
  report.Forbid(probe.MentionsPerson("sam", Body("gate")), "Kart kaydı Sam'in adını anıyor; kart listesi gereksizleşiyor.");
  report.Require(probe.MentionsPerson("sam", Body("staff")), "Kart listesi Sam'in adını anmıyor.");
  report.Forbid(probe.MentionsPerson("sam", Body("transcript")), "Döküm Sam'in adını anıyor; tanık onu görmedi.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "sam" && how.id == "push_fall" && after.id == "fled" && proof.id == "transcript",
   "Doğru rapor: Sam + itişmede düşme + yardım çağırmadan kaçtı + tam döküm.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var daniel = From(context);
  report.Require(Submit(daniel, "daniel", how.id, after.id) && daniel.Career.pendingReviews[0].evaluationType != "supported",
   "Daniel'i seçen rapor doğru sayılıyor.");
  var alone = From(context);
  report.Require(Submit(alone, who.id, "accident", after.id) && alone.Career.pendingReviews[0].evaluationType != "supported",
   "'Tek başına kaza' doğru sayılıyor.");
  var pushed = From(context);
  report.Require(Submit(pushed, who.id, how.id, "pushed_body") && pushed.Career.pendingReviews[0].evaluationType != "supported",
   "'Suya itti' doğru sayılıyor.");
 }
}
}
