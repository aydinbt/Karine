using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #055 "Hermandad" — kardeşlik başkanı, 1987 zümrüt hırsızlığını bulan saymanı şamdanla öldürdü.
// Otopsi ve şapel incelemesi kimseyi adıyla anmaz; ev araması ve 1987 dosyası Ignacio'yu anar.
public static class Case055Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "chapel" })
   foreach (var person in new[] { "ignacio", "paco", "amparo" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "house", "archive" })
   report.Require(probe.MentionsPerson("ignacio", Body(src)), $"{src} ignacio adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "archive");
    report.Require(inv.SubmitWarrant("archive", new[] { "autopsy", "chapel" }, 0) && inv.WarrantDenied(N(data, "archive")) && !inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("archive", new[] { "amparo", "paco" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "archive"), "archive doğru dayanakla onaylanmıyor."); }
  { var inv = From(context); inv.State.read.Remove("house"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "house");
    report.Require(inv.SubmitWarrant("house", new[] { "amparo", "autopsy" }, 0) && inv.WarrantDenied(N(data, "house")) && !inv.State.documentRequests.Any(r => r.nodeId == "house"), "house zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("house", new[] { "archive", "chapel" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "house"), "house doğru dayanakla onaylanmıyor."); }
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "1987" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 1987 arşiv kaydı olarak yeniden açılmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "ignacio" && how.id == "candle" && after.id == "altar" && proof.id == "house", "Doğru rapor: ignacio + candle + altar + house.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "paco", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: paco");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fall", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fall");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "river") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: river");
 }
}
}
