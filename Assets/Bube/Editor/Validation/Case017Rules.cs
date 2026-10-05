using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #017 "Köprü" — Birleşik Krallık finali. Olay yeri planı "kendi atladı" diyen
// bekçinin görüşünü merdiven kulesinin kestiğini göstermek zorunda; rekonstrüksiyon
// yedi kartla sıralanır ve bölüm finali Almanya'yı açar. Mesaj Bora'nın hattından
// görünür: Bora'yı, Aisha'yı ya da "atladı"yı seçen rapor desteklenmiş sayılmaz.
public static class Case017Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static void Fill(Investigation inv, System.Collections.Generic.IEnumerable<ReconCard> order) {
  inv.Recon.Clear();
  foreach (var c in order) { inv.ReconPlace(c.id); inv.ReconSource(c.id, c.supportingSourceIds[0]); }
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  report.Require(data.reconstruction != null && data.reconstruction.Length == 7 && data.chapterFinale != null
   && data.chapterFinale.nextFileKey == "finale.file.018", "Dosya yedi rekonstrüksiyon kartı ve Almanya'yı açan bölüm finali taşımalı.");
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "plan", "cam", "autopsy" })
   foreach (var person in new[] { "joan", "tommy", "aisha", "garrett", "graham" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "mailroom", "drafts", "switchboard", "holt_phone", "search", "bank" })
   report.Require(probe.MentionsPerson("graham", Body(src)), $"{src} Graham Holt'u adıyla anmıyor.");
  report.Require(probe.MentionsPerson("garrett", Body("letter")), "Taslak mektup Neil Garrett'ı anmıyor.");
  report.Require(probe.MentionsPerson("aisha", Body("aisha_log")), "Plaka kaydı Aisha'yı anmıyor.");

  var plan = N(data, "plan").scenePlan;
  var incident = new Vector2(plan.incident.x, plan.incident.y);
  PlanMarker Mk(string id) => plan.markers.First(m => m.id == id);
  report.Forbid(SightLine.Sees(Mk("tommy_claim"), incident, plan.occluders), "Plan: bekçinin mavnası köprüdeki noktayı görüyor.");
  report.Require(SightLine.Sees(Mk("warehouse_cam"), incident, plan.occluders), "Plan: depo kamerası köprüdeki noktayı görmüyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "graham" && how.id == "struck" && after.id == "graham" && proof.id == "switchboard",
   "Doğru rapor: Holt + vurulup itildi + Holt Bora'nın hattıyla + santral kaydı.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var empty = From(context); empty.Recon.Clear();
  report.Forbid(Submit(empty, who.id, how.id, after.id), "Rekonstrüksiyon boşken rapor gönderilebiliyor.");

  var correct = From(context); Fill(correct, data.reconstruction);
  report.Require(correct.ReconComplete && correct.ReconSupported, "Doğru sıra ve kaynaklar desteklenmiyor.");
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(10);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.hasRecon && fax.reconSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");

  var swapped = From(context);
  var order = data.reconstruction.ToList(); (order[3], order[4]) = (order[4], order[3]);
  Fill(swapped, order);
  report.Require(Submit(swapped, who.id, how.id, after.id) && swapped.Career.pendingReviews[0].evaluationType != "supported",
   "Yanlış sıralı rekonstrüksiyon doğru sayılıyor.");

  foreach (var (s, m, c, what) in new[] { ("aisha", how.id, after.id, "Aisha"), (who.id, "jumped", after.id, "'Atladı'"), (who.id, how.id, "bora", "'Bora'nın hattı'") }) {
   var inv = From(context); Fill(inv, data.reconstruction);
   report.Require(Submit(inv, s, m, c) && inv.Career.pendingReviews[0].evaluationType != "supported", what + " doğru sayılıyor.");
  }
 }
}
}
