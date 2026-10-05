using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #037 "Atölye" — pres ölümden sonra telefonla açıldı.
// Otopsi, pres kaydı ve avlu kamerası kimseyi adıyla anmaz; arama Lenoir'ı açar.
public static class Case037Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "presslog", "court" })
   foreach (var person in new[] { "clemence", "lenoir", "diallo", "aubert" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "badges", "search" })
   report.Require(probe.MentionsPerson("lenoir", Body(src)), $"{src} lenoir adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("search"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "search");
    report.Require(inv.SubmitWarrant("search", new[] { "clemence", "autopsy" }, 0) && inv.WarrantDenied(N(data, "search")) && !inv.State.documentRequests.Any(r => r.nodeId == "search"), "search zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("search", new[] { "presslog", "aubert" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "search"), "search doğru dayanakla onaylanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "lenoir" && how.id == "strangle" && after.id == "staged" && proof.id == "search", "Doğru rapor: lenoir + strangle + staged + search.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "clemence", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: clemence");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "shock", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: shock");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "forgot") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: forgot");
 }
}
}
