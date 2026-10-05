using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #061 "Buz Pisti" — pist müdürü, sahte soğutma yenilemesini fark eden buz ustasını amonyakla öldürdü.
// Yem: bakım defteri müdürün ‘normal’ satırını taşır; öne sürülünce müdür onu kendi lehine okur.
public static class Case061Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "room" })
   foreach (var person in new[] { "lavoie", "ouellet", "anika" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "office", "log" })
   report.Require(probe.MentionsPerson("lavoie", Body(src)), $"{src} lavoie adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("office"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "office");
    report.Require(inv.SubmitWarrant("office", new[] { "anika", "log" }, 0) && inv.WarrantDenied(N(data, "office")) && !inv.State.documentRequests.Any(r => r.nodeId == "office"), "office zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("office", new[] { "room", "ouellet" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "office"), "office doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "lavoie").questions.First(x => x.id == "case061.lavoie.end");
    report.Require(probe.DecoyAnswerKey(q, "log") != null && !probe.SourceMatchesQuestion(q, "log"), "lavoie.end: log yem olmalı, çözmemeli."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "lavoie" && how.id == "ammonia" && after.id == "drawer" && proof.id == "office", "Doğru rapor: lavoie + ammonia + drawer + office.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "ouellet", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: ouellet");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "leak", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: leak");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "ice") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: ice");
 }
}
}
