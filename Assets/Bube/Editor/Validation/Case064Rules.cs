using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #064 "Kar Fırtınası" — kar temizleme şirketinin sahibi, şişirilmiş faturaları sayan denetçiyi kepçeyle karın altına gömdü.
// Yem: filo GPS dökümü sahibin kamyonetini depoda gösterir.
public static class Case064Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy" })
   foreach (var person in new[] { "paquette", "oneill", "lachance", "yusra" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "depot", "gps" })
   report.Require(probe.MentionsPerson("paquette", Body(src)), $"{src} paquette adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("depot"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "depot");
    report.Require(inv.SubmitWarrant("depot", new[] { "lachance", "gps" }, 0) && inv.WarrantDenied(N(data, "depot")) && !inv.State.documentRequests.Any(r => r.nodeId == "depot"), "depot zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("depot", new[] { "autopsy", "oneill" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "depot"), "depot doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "paquette").questions.First(x => x.id == "case064.paquette.end");
    report.Require(probe.DecoyAnswerKey(q, "gps") != null && !probe.SourceMatchesQuestion(q, "gps"), "paquette.end: gps yem olmalı, çözmemeli."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "paquette" && how.id == "loader" && after.id == "salt" && proof.id == "depot", "Doğru rapor: paquette + loader + salt + depot.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "oneill", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: oneill");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "blower", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: blower");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "snow") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: snow");
 }
}
}
