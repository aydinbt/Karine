using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #042 "Fırtına" — taşeron şoför egzozu bezle tıkadı, yükü çaldı.
// Otopsi, kamera ve kurbanın seyir kaydı kimseyi adıyla anmaz; inceleme ve mesajlar Tanner'ı anar.
public static class Case042Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "lot", "eld" })
   foreach (var person in new[] { "tanner", "hollis", "patel" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "ragcheck", "texts" })
   report.Require(probe.MentionsPerson("tanner", Body(src)), $"{src} tanner adını anmıyor.");
  { var inv = From(context); inv.State.closedLines?.Clear(); inv.State.lineReopens?.Clear();
    inv.State.read.Remove("line_cargo"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "line_cargo");
    report.Require(inv.LineActive("line_truck") && inv.LineActive("line_phone"), "line_truck ve line_phone hatları açık değil.");
    report.Forbid(inv.CanRequestWarrant(N(data, "line_cargo")), "Üçüncü hat yuvalar doluyken açılabiliyor.");
    report.Require(inv.CloseLine("line_truck") && !inv.LineActive("line_truck") && inv.CanRequestWarrant(N(data, "line_cargo")), "Hat kapatılınca yuva boşalmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "tanner" && how.id == "co" && after.id == "plug" && proof.id == "ragcheck", "Doğru rapor: tanner + co + plug + ragcheck.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "hollis", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: hollis");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "cold", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: cold");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "storm") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: storm");
 }
}
}
