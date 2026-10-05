using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #063 "Le Sous-Sol" — kulüp sahibi, ‘kilit kişi’ poliçesini geri almak isteyen kontrbasçıyı adaptörle çarptırdı.
// Yem: kulüp sahibinin kendi imzaladığı elektrik denetim raporu.
public static class Case063Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "stage", "backstage" })
   foreach (var person in new[] { "marchand", "adeyemi", "jules", "zoe", "nowak", "amara", "deschamps" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "office", "inspect" })
   report.Require(probe.MentionsPerson("marchand", Body(src)), $"{src} marchand adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("office"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "office");
    report.Require(inv.SubmitWarrant("office", new[] { "jules", "autopsy" }, 0) && inv.WarrantDenied(N(data, "office")) && !inv.State.documentRequests.Any(r => r.nodeId == "office"), "office zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("office", new[] { "backstage", "stage" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "office"), "office doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "marchand").questions.First(x => x.id == "case063.marchand.end");
    report.Require(probe.DecoyAnswerKey(q, "inspect") != null && !probe.SourceMatchesQuestion(q, "inspect"), "marchand.end: inspect yem olmalı, çözmemeli."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "marchand" && how.id == "shock" && after.id == "safe" && proof.id == "office", "Doğru rapor: marchand + shock + safe + office.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "adeyemi", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: adeyemi");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "heart", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: heart");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "trash") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: trash");
 }
}
}
