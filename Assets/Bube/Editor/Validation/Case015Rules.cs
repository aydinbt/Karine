using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #015 "Kapanış Saati" — dört izinli polis aynı cümleyle bir düşüş anlatır.
// Olay yeri planı, sigara alanındaki üç beyanın görüş hattını yan duvarın, kaldırımdaki
// dördüncüsünü kamyonetin kestiğini; barmenin penceresinin ise olay noktasını gördüğünü
// geometriyle göstermek zorunda. Plan ve taksi kamerası kimsenin adını anmaz. Ryan'ı,
// "kavgada itildi"yi ya da "gerçekten gördüler"i seçen rapor desteklenmiş sayılmaz.
public static class Case015Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var person in new[] { "garrett", "gareth", "paul", "steve", "ryan", "josie", "frank" })
   report.Forbid(probe.MentionsPerson(person, Body("plan")), $"Olay yeri planı {person} adını anıyor.");
  report.Require(probe.MentionsPerson("gareth", Body("till")), "Bar terminali Gareth'in tam adını anmıyor.");
  report.Require(probe.MentionsPerson("steve", Body("call999")) && probe.MentionsPerson("ryan", Body("call999")), "999 kaydı Steve ve Ryan'ı anmıyor.");
  report.Require(probe.MentionsPerson("frank", Body("cloud")) && probe.MentionsPerson("garrett", Body("cloud")), "Bulut yedeği Frank ve Garrett'ı anmıyor.");
  foreach (var person in new[] { "garrett", "gareth", "paul", "steve" })
   report.Require(probe.MentionsPerson(person, Body("staff")), $"Personel kaydı {person} adını anmıyor.");

  // Planın kendisi: beyanlar olay noktasını göremez, barmenin penceresi görür.
  var plan = N(data, "plan").scenePlan;
  var incident = new Vector2(plan.incident.x, plan.incident.y);
  PlanMarker Mk(string id) => plan.markers.First(m => m.id == id);
  foreach (var id in new[] { "gareth_claim", "paul_claim", "garrett_claim", "steve_claim", "gareth_till" })
   report.Forbid(SightLine.Sees(Mk(id), incident, plan.occluders), $"Plan: {id} olay noktasını görüyor.");
  report.Require(SightLine.Sees(Mk("josie_window"), incident, plan.occluders), "Plan: barmenin penceresi olay noktasını görmüyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "garrett" && how.id == "robbed" && after.id == "colluded" && proof.id == "plan",
   "Doğru rapor: Garrett + telefon için vuruldu + birlikte kurdular + olay yeri planı.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);

  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  var ryan = From(context);
  report.Require(Submit(ryan, "ryan", how.id, after.id) && ryan.Career.pendingReviews[0].evaluationType != "supported",
   "Ryan'ı seçen rapor doğru sayılıyor.");
  var pushed = From(context);
  report.Require(Submit(pushed, who.id, "pushed", after.id) && pushed.Career.pendingReviews[0].evaluationType != "supported",
   "'Kavgada itildi' doğru sayılıyor.");
  var saw = From(context);
  report.Require(Submit(saw, who.id, how.id, "saw") && saw.Career.pendingReviews[0].evaluationType != "supported",
   "'Gerçekten gördüler' doğru sayılıyor.");
 }
}
}
