using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #040 "Yangın" — damat uyku ilacı verip evi ateşe verdi.
// Otopsi ve kapı kamerası kimseyi adıyla anmaz; laboratuvar ve mali kayıt Duffy'yi anar.
public static class Case040Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "door" })
   foreach (var person in new[] { "megan", "duffy", "okafor", "ramirez" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "prints", "policy" })
   report.Require(probe.MentionsPerson("duffy", Body(src)), $"{src} duffy adını anmıyor.");
  { var inv = From(context); inv.State.closedLines?.Clear(); inv.State.lineReopens?.Clear();
    inv.State.read.Remove("line_archive"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "line_archive");
    report.Require(inv.LineActive("line_fire") && inv.LineActive("line_money"), "line_fire ve line_money hatları açık değil.");
    report.Forbid(inv.CanRequestWarrant(N(data, "line_archive")), "Üçüncü hat yuvalar doluyken açılabiliyor.");
    report.Require(inv.CloseLine("line_fire") && !inv.LineActive("line_fire") && inv.CanRequestWarrant(N(data, "line_archive")), "Hat kapatılınca yuva boşalmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "duffy" && how.id == "sedate" && after.id == "removed" && proof.id == "prints", "Doğru rapor: duffy + sedate + removed + prints.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "megan", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: megan");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "smoke", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: smoke");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "dead") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: dead");
 }
}
}
