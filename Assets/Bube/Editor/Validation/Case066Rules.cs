using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #066 "Laurentide" — Kanada bölüm finali. Başkan yardımcısı, ‘özel programlar’ raporunu sunacak baş aktüeri arşive kilitleyip gazı boşalttı.
// Yem: yönetim kurulu tutanağı onu ‘Toronto’dan’ bağlı gösterir.
public static class Case066Rules {

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

  foreach (var src in new[] { "autopsy", "floor" })
   foreach (var person in new[] { "whitfield", "leduc", "obi", "renaud", "mirela" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "locker", "minutes" })
   report.Require(probe.MentionsPerson("whitfield", Body(src)), $"{src} whitfield adını anmıyor.");
  { var inv = From(context); inv.State.read.Remove("locker"); inv.State.documentRequests.RemoveAll(r => r.nodeId == "locker");
    report.Require(inv.SubmitWarrant("locker", new[] { "leduc", "autopsy" }, 0) && inv.WarrantDenied(N(data, "locker")) && !inv.State.documentRequests.Any(r => r.nodeId == "locker"), "locker zayıf dayanakla onaylanıyor.");
    report.Require(inv.SubmitWarrant("locker", new[] { "floor", "obi" }, 0) && inv.State.documentRequests.Any(r => r.nodeId == "locker"), "locker doğru dayanakla onaylanmıyor."); }
  { var q = N(data, "whitfield").questions.First(x => x.id == "case066.whitfield.end");
    report.Require(probe.DecoyAnswerKey(q, "minutes") != null && !probe.SourceMatchesQuestion(q, "minutes"), "whitfield.end: minutes yem olmalı, çözmemeli."); }
  report.Require(data.reconstruction != null && data.reconstruction.Length == 7, "Rekonstrüksiyon 7 kart olmalı.");
  report.Require(data.chapterFinale != null && data.chapterFinale.nextFileKey == "finale.file.067", "Bölüm finali finale.file.067 açmalı.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "whitfield" && how.id == "gas" && after.id == "squash" && proof.id == "locker", "Doğru rapor: whitfield + gas + squash + locker.");

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
  report.Require(Submit(w0, "leduc", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: leduc");
  var w1 = From(context); Fill(w1, data.reconstruction);
  report.Require(Submit(w1, who.id, "fault", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fault");
  var w2 = From(context); Fill(w2, data.reconstruction);
  report.Require(Submit(w2, who.id, how.id, "panel") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: panel");
 }
}
}
