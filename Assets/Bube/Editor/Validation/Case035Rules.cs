using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #035 "Metro" — 'kahraman' yolcu acil çağrıyı itmeden yedi saniye önce başlattı.
// Kabin kamerası ve otopsi kimseyi adıyla anmaz; çağrı, kart ve telefon kayıtları Mallet'yi anar. Telefon gerekçeli izinle açılır.
public static class Case035Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "cab" })
   foreach (var person in new[] { "benhamou", "odile", "mallet", "royer" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "calllog", "navigo", "phone", "ruling" })
   report.Require(probe.MentionsPerson("mallet", Body(src)), $"{src} mallet adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("phone"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "phone");
    report.Require(inv.SubmitWarrant("phone", new[] { "royer", "autopsy" }, 0) && inv.WarrantDenied(N(data, "phone")) && !inv.State.documentRequests.Any(r => r.nodeId == "phone"), "phone zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("phone", new[] { "calllog", "cab" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "phone"), "phone doğru dayanakla onaylanmıyor."); }

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "mallet" && how.id == "push" && after.id == "before" && proof.id == "calllog", "Doğru rapor: mallet + push + before + calllog.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "royer", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: royer");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "crowd", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: crowd");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "after") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: after");
 }
}
}
