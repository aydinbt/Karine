using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #020 "Karartılmış Satır" — muhbir "Lerche" kardeş Günter değil, Werner'in kendisi.
// Karartılmış dosya, asansör kamerası ve otopsi kimseyi adıyla anmaz (dosya sahibi Werner kişi değildir);
// iş takvimi Günter'i, erişim kaydı, telefon, DNA ve arama Stefan'ı anar.
public static class Case020Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "file", "lift", "autopsy" })
   foreach (var person in new[] { "ingrid", "gunter", "petra", "stefan" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "access", "wphone", "dna", "search" })
   report.Require(probe.MentionsPerson("stefan", Body(src)), $"{src} stefan adını anmıyor.");
  foreach (var src in new[] { "calendar" })
   report.Require(probe.MentionsPerson("gunter", Body(src)), $"{src} gunter adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "stefan" && how.id == "pillow" && after.id == "werner" && proof.id == "dna", "Doğru rapor: stefan + pillow + werner + dna.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "gunter", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: gunter");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "heart", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: heart");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "gunter") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: gunter");
 }
}
}
