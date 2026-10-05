using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #065 "Silo" — proje müdürü, korunması gereken iskeleti bulan miras danışmanını hazneden düşürdü.
// Yem: restoran fişi ve kart hareketi proje müdürüne 21.15’te bir masa verir.
public static class Case065Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "gate" })
   foreach (var person in new[] { "beaulieu", "kwan", "rousseau", "farah", "morin", "gendron" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "site", "receipt" })
   report.Require(probe.MentionsPerson("beaulieu", Body(src)), $"{src} beaulieu adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("site"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "site");
    report.Require(inv.SubmitWarrant("site", new[] { "farah", "receipt" }, 0) && inv.WarrantDenied(N(data, "site")) && !inv.State.documentRequests.Any(r => r.nodeId == "site"), "site zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("site", new[] { "gate", "autopsy" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "site"), "site doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "beaulieu").questions.First(x => x.id == "case065.beaulieu.end");
    report.Require(probe.DecoyAnswerKey(q, "receipt") != null && !probe.SourceMatchesQuestion(q, "receipt"), "beaulieu.end: receipt yem olmalı, çözmemeli."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "beaulieu" && how.id == "hatch" && after.id == "model" && proof.id == "site", "Doğru rapor: beaulieu + hatch + model + site.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "farah", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: farah");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "slip", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: slip");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "river") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: river");
 }
}
}
