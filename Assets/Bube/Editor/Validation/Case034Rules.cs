using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #034 "Tamirci" — kuzen telefonunu kurye gence verip konum alibisi kurdu.
// Kamera ve otopsi kimseyi adıyla anmaz; konum kaydı, defter ve arama Yacine'i anar. Daire gerekçeli izinle açılır.
public static class Case034Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "street" })
   foreach (var person in new[] { "amira", "yacine", "lea", "popescu", "zhou", "theo" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "track", "simbook", "flat" })
   report.Require(probe.MentionsPerson("yacine", Body(src)), $"{src} yacine adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("flat"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "flat");
    report.Require(inv.SubmitWarrant("flat", new[] { "popescu", "autopsy" }, 0) && inv.WarrantDenied(N(data, "flat")) && !inv.State.documentRequests.Any(r => r.nodeId == "flat"), "flat zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("flat", new[] { "theo", "track" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "flat"), "flat doğru dayanakla onaylanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "yacine" && how.id == "knife" && after.id == "theo" && proof.id == "flat", "Doğru rapor: yacine + knife + theo + flat.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "popescu", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: popescu");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "robbery", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: robbery");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "yacine") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: yacine");
 }
}
}
