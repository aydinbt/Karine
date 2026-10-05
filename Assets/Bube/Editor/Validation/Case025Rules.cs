using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #025 "Kapsül" — iki yabancı kapsül değiştirdi; katil doğru kapsüle, yanlış adama girdi.
// Kamera ve otopsi kimseyi adıyla anmaz; mesajlar, taksi ve DNA Ōta'yı, dolap Mori'yi anar.
public static class Case025Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "camera", "autopsy" })
   foreach (var person in new[] { "sakai", "ishii", "kenta", "ota" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "sakaiphone", "taxi", "dna" })
   report.Require(probe.MentionsPerson("ota", Body(src)), $"{src} ota adını anmıyor.");
  foreach (var src in new[] { "locker" })
   report.Require(probe.MentionsPerson("kenta", Body(src)), $"{src} kenta adını anmıyor.");
  foreach (var src in new[] { "cardlog" })
   report.Require(probe.MentionsPerson("sakai", Body(src)), $"{src} sakai adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "ota" && how.id == "pillow" && after.id == "kenta" && proof.id == "dna", "Doğru rapor: ota + pillow + kenta + dna.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "sakai", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: sakai");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "heart", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: heart");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "tanabe") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: tanabe");
 }
}
}
