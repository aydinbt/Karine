using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #027 "Pachinko" — soğuk depo ölüm saatini kaydırdı; kasiyer öldürdü.
// Kamera ve otopsi kimseyi adıyla anmaz; kasa araması ve faturalar Takeda'yı anar.
public static class Case027Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "counter" })
   foreach (var person in new[] { "yuki", "hideo", "sato" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "tray", "ledger" })
   report.Require(probe.MentionsPerson("yuki", Body(src)), $"{src} yuki adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "yuki" && how.id == "strap" && after.id == "cold" && proof.id == "thermo", "Doğru rapor: yuki + strap + cold + thermo.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "hideo", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: hideo");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "hands", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: hands");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "dawn") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: dawn");
 }
}
}
