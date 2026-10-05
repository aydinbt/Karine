using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #028 "Karaoke" — menajer uzaktan istek ve bir yıllık kayıtla alibi kurdu.
// Koridor ve otopsi kimseyi adıyla anmaz; sistem kaydı, inhaler ve merdiven Kanda'yı anar.
public static class Case028Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "corridor" })
   foreach (var person in new[] { "nana", "reina", "kanda", "yamato", "haru", "miki", "goto" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "syslog", "inhalerlab", "cuff" })
   report.Require(probe.MentionsPerson("kanda", Body(src)), $"{src} kanda adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "kanda" && how.id == "asthma" && after.id == "remote" && proof.id == "syslog", "Doğru rapor: kanda + asthma + remote + syslog.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "haru", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: haru");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "natural", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: natural");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "live") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: live");
 }
}
}
