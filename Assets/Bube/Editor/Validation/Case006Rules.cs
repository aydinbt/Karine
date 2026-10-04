using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #006 "Son Mesaj" — iki katmanlı rapor. Kayboluş Defne'nin planı, 23.41
// mesajı Cem'in, takip Halil'in. "Kendi isteğiyle" tek başına doğru ama eksik:
// diğer iki sütun yanlışsa rapor desteklenmiş sayılmaz. Kırılma kayıtları
// (garson ifadesi, zarf, arama dökümü) oyuncunun sorduğu soruyla açılır.
public static class Case006Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot));
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static string Src(CaseData data, string id) { var n = N(data, id); return n.kind == "cctv" ? id + "#" + n.cctvEvents[0].id : id; }

 static void Gate(CaseContext context, string node, string question, string message) {
  var inv = From(context); inv.State.asked.Remove("case006." + question);
  context.Report.Forbid(inv.Discovered(N(context.Data, node)), message);
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;

  Gate(context, "waiter", "onur.talk", "Garson ifadesi Onur'a ne konuştuklarını sormadan açılıyor.");
  Gate(context, "envelope", "onur_follow.envelope", "Zarf Onur'a zarf sorulmadan açılıyor.");
  Gate(context, "calllog", "cem_third.who", "Arama dökümü Cem'e 'Kimin için?' sorulmadan açılıyor.");

  var first = data.verdicts.First(v => v.correct);
  var sender = data.methods.First(m => m.correct);
  var organizer = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(first.id == "voluntary" && sender.id == "cem_msg" && organizer.id == "halil_order",
   "Doğru rapor: kendi isteğiyle + mesaj Cem + takip Halil.");
  report.Require(sender.supportingSourceIds.Contains("signal"), "Mesaj sütunu baz kaydıyla desteklenmeli.");
  report.Require(organizer.supportingSourceIds.Contains("calllog"), "Takip sütunu arama dökümüyle desteklenmeli.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   Src(data, first.supportingSourceIds[0]), Src(data, sender.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
   c, Src(data, organizer.supportingSourceIds[0]));

  var correct = From(context);
  if (!report.Step(Submit(correct, first.id, sender.id, organizer.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  // "Defne her şeyi sahneledi" kolay açıklaması: kayboluş doğru, mesaj Defne'ye yüklenmiş.
  var staged = From(context);
  report.Require(Submit(staged, first.id, "defne_msg", organizer.id) && staged.Career.pendingReviews[0].evaluationType != "supported",
   "'Mesajı Defne attı' doğru sayılıyor.");
  var alone = From(context);
  report.Require(Submit(alone, first.id, sender.id, "cem_alone") && alone.Career.pendingReviews[0].evaluationType != "supported",
   "'Cem kendi başına' doğru sayılıyor.");
 }
}
}
