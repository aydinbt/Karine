using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #022 "Gece Treni" — üç tanık üç ayrı cezayı anlatıyor; fail ortak kontrolör.
// Plan, bisiklet bölmesinin genç tanığın arka kapıyı görmesini engellediğini, Okan'ın gördüğünü göstermeli.
public static class Case022Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "plan", "autopsy" })
   foreach (var person in new[] { "sven", "nele", "jannik", "okan", "petra" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "svenlog", "audit", "plastic" })
   report.Require(probe.MentionsPerson("sven", Body(src)), $"{src} sven adını anmıyor.");
  var plan = N(data, "plan").scenePlan;
  var incident = new Vector2(plan.incident.x, plan.incident.y);
  PlanMarker Mk(string id) => plan.markers.First(m => m.id == id);
  report.Forbid(SightLine.Sees(Mk("jannik_claim"), incident, plan.occluders), "Plan: jannik_claim olay noktasını görüyor.");
  report.Require(SightLine.Sees(Mk("okan_claim"), incident, plan.occluders), "Plan: okan_claim olay noktasını görmüyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "sven" && how.id == "device" && after.id == "three" && proof.id == "plastic", "Doğru rapor: sven + device + three + plastic.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "jannik", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: jannik");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "pole", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: pole");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "liar") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: liar");
 }
}
}
