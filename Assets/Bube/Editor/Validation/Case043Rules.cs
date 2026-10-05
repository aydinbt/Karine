using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #043 "Blues" — yeğen mikrofon standını amfiye köprüleyip elektriklendirdi.
// Otopsi ve koridor kamerası kimseyi adıyla anmaz; elektrik incelemesi ve kasa kaydı Reggie'yi anar.
public static class Case043Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "panel" })
   foreach (var person in new[] { "reggie", "banks", "monroe", "romano", "pope" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "elec", "ledger" })
   report.Require(probe.MentionsPerson("reggie", Body(src)), $"{src} reggie adını anmıyor.");
  { var inv = From(context); inv.State.closedLines?.Clear(); inv.State.lineReopens?.Clear();
    inv.State.read.Remove("line_union"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "line_union");
    report.Require(inv.LineActive("line_elec") && inv.LineActive("line_cash"), "line_elec ve line_cash hatları açık değil.");
    report.Forbid(inv.CanRequestWarrant(N(data, "line_union")), "Üçüncü hat yuvalar doluyken açılabiliyor.");
    report.Require(inv.CloseLine("line_elec") && !inv.LineActive("line_elec") && inv.CanRequestWarrant(N(data, "line_union")), "Hat kapatılınca yuva boşalmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "reggie" && how.id == "shock" && after.id == "rigged" && proof.id == "elec", "Doğru rapor: reggie + shock + rigged + elec.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "banks", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: banks");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "wiring", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: wiring");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "old") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: old");
 }
}
}
