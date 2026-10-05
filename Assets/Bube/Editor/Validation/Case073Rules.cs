using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #073 "İrtibat" — Avustralya bölüm finali ve son dosya. Hukuk müşaviri, defterini şubeye götürecek koordinatörü insülinle öldürdü ve defteri Budapeşte'ye yolladı.
// Yem: bina kart kaydı onu 19.02'de çıkmış gösterir. Kuruluş arşivi dokuz ülkeyi birleştirir.
public static class Case073Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static void Fill(Investigation inv, System.Collections.Generic.IEnumerable<ReconCard> order) {
  inv.Recon.Clear();
  foreach (var c in order) { inv.ReconPlace(c.id); inv.ReconSource(c.id, c.supportingSourceIds[0]); }
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "autopsy", "desk" })
   foreach (var person in new[] { "quinlan", "marlowe", "naidoo", "tamsin", "okeke", "brodie", "yindi" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "panel", "courier", "badges" })
   report.Require(probe.MentionsPerson("quinlan", Body(src)), $"{src} quinlan adını anmıyor.");
  report.Require(N(data, "archive").requestKind == "archive" && N(data, "archive").reopenYear == "1989" && !string.IsNullOrEmpty(N(data, "archive").reopenNoteKey), "archive 1989 arşiv kaydı olarak yeniden açılmalı.");
  { var q = N(data, "quinlan").questions.First(x => x.id == "case073.quinlan.end");
    report.Require(probe.DecoyAnswerKey(q, "badges") != null && !probe.SourceMatchesQuestion(q, "badges"), "quinlan.end: badges yem olmalı, çözmemeli."); }
  { var inv = From(context); inv.State.closedLines?.Clear(); inv.State.lineReopens?.Clear();
    inv.State.read.Remove("line_programme"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "line_programme");
    report.Require(inv.LineActive("line_building") && inv.LineActive("line_money"), "line_building ve line_money hatları açık değil.");
    report.Forbid(inv.CanRequestWarrant(N(data, "line_programme")), "Üçüncü hat yuvalar doluyken açılabiliyor.");
    report.Require(inv.CloseLine("line_building") && !inv.LineActive("line_building") && inv.CanRequestWarrant(N(data, "line_programme")), "Hat kapatılınca yuva boşalmıyor."); }
  report.Require(data.reconstruction != null && data.reconstruction.Length == 7, "Rekonstrüksiyon 7 kart olmalı.");
  report.Require(data.chapterFinale != null && data.chapterFinale.nextFileKey == "finale.file.end", "Bölüm finali finale.file.end açmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "quinlan" && how.id == "insulin" && after.id == "budapest" && proof.id == "panel", "Doğru rapor: quinlan + insulin + budapest + panel.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var empty = From(context); empty.Recon.Clear();
  report.Forbid(Submit(empty, who.id, how.id, after.id), "Rekonstrüksiyon boşken rapor gönderilebiliyor.");
  var correct = From(context); Fill(correct, data.reconstruction);
  report.Require(correct.ReconComplete && correct.ReconSupported, "Doğru sıra ve kaynaklar desteklenmiyor.");
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported && fax.reconSupported, "Doğru rapor faksta desteklenmedi.");
  var swapped = From(context);
  var order = data.reconstruction.ToList(); (order[2], order[3]) = (order[3], order[2]);
  Fill(swapped, order);
  report.Require(Submit(swapped, who.id, how.id, after.id) && swapped.Career.pendingReviews[0].evaluationType != "supported", "Yanlış sıralı rekonstrüksiyon doğru sayılıyor.");
  var w0 = From(context); Fill(w0, data.reconstruction);
  report.Require(Submit(w0, "yindi", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: yindi");
  var w1 = From(context); Fill(w1, data.reconstruction);
  report.Require(Submit(w1, who.id, "self", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: self");
  var w2 = From(context); Fill(w2, data.reconstruction);
  report.Require(Submit(w2, who.id, how.id, "safe") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: safe");
 }
}
}
