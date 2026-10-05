using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #047 "Forno" — ortak, pizzacıyı kolu sökülmüş soğuk odaya kapattı.
// Otopsi, soğuk oda ve sokak kamerası kimseyi adıyla anmaz; araba araması ve kasa defteri Enzo'yu anar. Seyyar satıcı soğuk oda incelemesi okununca şehri terk eder.
public static class Case047Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "coldroom", "street" })
   foreach (var person in new[] { "enzo", "rosaria", "moussa", "rocco", "lucia" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "car", "ledger" })
   report.Require(probe.MentionsPerson("enzo", Body(src)), $"{src} enzo adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("car"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "car");
    report.Require(inv.SubmitWarrant("car", new[] { "rosaria", "autopsy" }, 0) && inv.WarrantDenied(N(data, "car")) && !inv.State.documentRequests.Any(r => r.nodeId == "car"), "car zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("car", new[] { "coldroom", "street" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "car"), "car doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("moussa"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "moussa"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "moussa");
    foreach (var id in N(data, "moussa").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "moussa")), "moussa kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "enzo" && how.id == "cold" && after.id == "glovebox" && proof.id == "car", "Doğru rapor: enzo + cold + glovebox + car.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "rocco", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: rocco");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "trapped", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: trapped");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "broken") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: broken");
 }
}
}
