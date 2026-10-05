using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #060 "Simit Fırını" — müdür, sahte poliçeyi fark eden fırın sahibini kürekle öldürdü.
// Kanada mekaniği: mesai kartı yemdir; katile öne sürülünce kendi hikâyesini anlatır, meseleyi bitirmez.
public static class Case060Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "oven" })
   foreach (var person in new[] { "tremblay", "rivka", "dmitri", "gagnon", "luc", "bouchard" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "home", "clock" })
   report.Require(probe.MentionsPerson("tremblay", Body(src)), $"{src} tremblay adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("home"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "home");
    report.Require(inv.SubmitWarrant("home", new[] { "gagnon", "autopsy" }, 0) && inv.WarrantDenied(N(data, "home")) && !inv.State.documentRequests.Any(r => r.nodeId == "home"), "home zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("home", new[] { "alley", "oven" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "home"), "home doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "tremblay").questions.First(x => x.id == "case060.tremblay.end");
    report.Require(probe.DecoyAnswerKey(q, "clock") != null && !probe.SourceMatchesQuestion(q, "clock"), "tremblay.end: clock yem olmalı, çözmemeli."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "tremblay" && how.id == "peel" && after.id == "ash" && proof.id == "home", "Doğru rapor: tremblay + peel + ash + home.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "rivka", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: rivka");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "slip", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: slip");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "woodpile") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: woodpile");
 }
}
}
