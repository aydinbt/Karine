using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #021 "Kürek" — ölen Lukas, yaşayan Moritz kardeşinin adını aldı. Plan, bakımcının
// penceresinin iskeleyi görmediğini, balıkçının gördüğünü göstermek zorunda. Plan, kamera ve otopsi
// kimseyi adıyla anmaz; ortopedi kaydı, röntgen ve parmak izi "Lukas" adını anar.
public static class Case021Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "plan", "slip", "autopsy" })
   foreach (var person in new[] { "lukas", "anja", "holger", "rolf" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "medical", "xray", "prints" })
   report.Require(probe.MentionsPerson("lukas", Body(src)), $"{src} lukas adını anmıyor.");
  var plan = N(data, "plan").scenePlan;
  var incident = new Vector2(plan.incident.x, plan.incident.y);
  PlanMarker Mk(string id) => plan.markers.First(m => m.id == id);
  report.Forbid(SightLine.Sees(Mk("holger_claim"), incident, plan.occluders), "Plan: holger_claim olay noktasını görüyor.");
  report.Forbid(SightLine.Sees(Mk("slip_cam"), incident, plan.occluders), "Plan: slip_cam olay noktasını görüyor.");
  report.Require(SightLine.Sees(Mk("rolf_claim"), incident, plan.occluders), "Plan: rolf_claim olay noktasını görmüyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "survivor" && how.id == "oar" && after.id == "lukas" && proof.id == "prints", "Doğru rapor: survivor + oar + lukas + prints.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "holger", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: holger");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, "capsized", after.id) && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: capsized");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, how.id, "moritz") && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: moritz");
 }
}
}
