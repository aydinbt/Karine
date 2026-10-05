using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #049 "Presepe" — antikacı, sahte figürlerin bilirkişisini arsenikli pigmentle zehirledi.
// Toksikoloji kimseyi adıyla anmaz; dükkân araması ve ihracat belgeleri Vittoria'yı anar. Kahve çocuğu toksikoloji okununca şehirden ayrılır.
public static class Case049Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "tox" })
   foreach (var person in new[] { "vittoria", "matteo", "teresa", "bruno" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "shop", "export" })
   report.Require(probe.MentionsPerson("vittoria", Body(src)), $"{src} vittoria adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("shop"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "shop");
    report.Require(inv.SubmitWarrant("shop", new[] { "bruno", "teresa" }, 0) && inv.WarrantDenied(N(data, "shop")) && !inv.State.documentRequests.Any(r => r.nodeId == "shop"), "shop zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("shop", new[] { "tox", "matteo" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "shop"), "shop doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("bruno"); inv.State.interviewTurns.RemoveAll(t => t.nodeId == "bruno"); inv.State.interviewRequests.RemoveAll(r => r.nodeId == "bruno");
    foreach (var id in N(data, "bruno").closesAfterRead) if (!inv.State.read.Contains(id)) inv.State.read.Add(id);
    report.Require(inv.Closed(N(data, "bruno")), "bruno kaynak okununca kapanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "vittoria" && how.id == "arsenic" && after.id == "bag" && proof.id == "shop", "Doğru rapor: vittoria + arsenic + bag + shop.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "matteo", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: matteo");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fumes", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fumes");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "posted") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: posted");
 }
}
}
