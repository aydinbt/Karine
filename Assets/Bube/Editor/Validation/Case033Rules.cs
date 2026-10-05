using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #033 "Ekmek" — zehirli un Auguste Delorme içindi; fırıncı tadıp öldü.
// Toksikoloji ve kamera kimseyi adıyla anmaz; un analizi, form ve minibüs Garnier'i anar. Minibüs gerekçeli izinle açılır.
public static class Case033Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "tox", "backdoor" })
   foreach (var person in new[] { "nadine", "rayan", "garnier" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "flour", "order", "van" })
   report.Require(probe.MentionsPerson("garnier", Body(src)), $"{src} garnier adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("van"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "van");
    report.Require(inv.SubmitWarrant("van", new[] { "tox", "nadine" }, 0) && inv.WarrantDenied(N(data, "van")) && !inv.State.documentRequests.Any(r => r.nodeId == "van"), "van zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("van", new[] { "flour", "order" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "van"), "van doğru dayanakla onaylanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "garnier" && how.id == "aconite" && after.id == "delorme" && proof.id == "flour", "Doğru rapor: garnier + aconite + delorme + flour.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "rayan", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: rayan");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "heart", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: heart");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "didier") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: didier");
 }
}
}
