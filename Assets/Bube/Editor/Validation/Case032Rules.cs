using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #032 "Lot 47" — bölüm başkanı katalogcuyu kasada öldürdü; taşıyıcının kartını panodan aldı.
// Kamera ve otopsi kimseyi adıyla anmaz; arama ve mektup incelemesi Mercier'yi anar. Ofis araması gerekçeli izinle açılır.
public static class Case032Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "vault" })
   foreach (var person in new[] { "mercier", "benali", "hugo", "colette", "bastien" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "search", "letter", "board" })
   report.Require(probe.MentionsPerson("mercier", Body(src)), $"{src} mercier adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("search"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "search");
    report.Require(inv.SubmitWarrant("search", new[] { "autopsy", "hugo" }, 0) && inv.WarrantDenied(N(data, "search")) && !inv.State.documentRequests.Any(r => r.nodeId == "search"), "search zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("search", new[] { "recall", "letter" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "search"), "search doğru dayanakla onaylanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "mercier" && how.id == "bookend" && after.id == "fake" && proof.id == "search", "Doğru rapor: mercier + bookend + fake + search.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "benali", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: benali");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "fall", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fall");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "real") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: real");
 }
}
}
