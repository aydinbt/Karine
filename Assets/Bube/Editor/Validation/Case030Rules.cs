using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #030 "Tatbikat" — tahliye sorumlusu öldürdü ve kurbanın yoklamasını kendisi tikledi.
// Kamera ve otopsi kimseyi adıyla anmaz; uygulama kaydı, yoklama, arama ve dosya Nishida'yı anar.
public static class Case030Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "lobby" })
   foreach (var person in new[] { "nishida", "hirano", "ono" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "qr", "sheet", "copy", "file" })
   report.Require(probe.MentionsPerson("nishida", Body(src)), $"{src} nishida adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "nishida" && how.id == "ext" && after.id == "forged" && proof.id == "copy", "Doğru rapor: nishida + ext + forged + copy.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "hirano", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: hirano");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "push", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: push");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "genuine") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: genuine");
 }
}
}
