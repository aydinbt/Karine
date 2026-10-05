using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #046 "Basso" — İtalya bölümü açılışı. Torun büyükannesini yastıkla boğup liman fişlerini sattı.
// Otopsi kimseyi adıyla anmaz; arama, kurye kaydı ve banka Gennaro'yu anar. Turist otopsi okununca şehri terk eder.
public static class Case046Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy" })
   foreach (var person in new[] { "gennaro", "assunta", "pieter" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "scooter", "orders", "bank" })
   report.Require(probe.MentionsPerson("gennaro", Body(src)), $"{src} gennaro adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("pieter"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "pieter"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "pieter");
    foreach (var id in N(data, "pieter").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "pieter")), "pieter kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "gennaro" && how.id == "pillow" && after.id == "seat" && proof.id == "scooter", "Doğru rapor: gennaro + pillow + seat + scooter.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "assunta", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: assunta");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "heart", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: heart");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "men") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: men");
 }
}
}
