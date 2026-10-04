using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #008 "Kopya" — olay bağlantısı ve arşiv taraması. Olay C, A ile B
// bağlanmadan dosyaya düşmez; zayıf bağlantı ve tarama dayanağı reddedilir.
// Rapor üç ayrı eli ayırır: zarflar Burak'ın, ev girişi Zeynep'in, 2006'da
// itme düşme değil. Kolay "tek fail" raporları desteklenmez.
public static class Case008Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot));
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);
 static string Src(CaseData data, string id) { var n = N(data, id); return n.kind == "cctv" ? id + "#" + n.cctvEvents[0].id : id; }

 static void Retry(CaseContext context, string node, string[] weak, string[] strong, string label) {
  var data = context.Data; var report = context.Report; var inv = From(context);
  inv.State.read.Remove(node); inv.State.documentRequests.RemoveAll(r => r.nodeId == node);
  report.Require(inv.SubmitWarrant(node, weak, 0) && inv.WarrantDenied(N(data, node)) && !inv.State.documentRequests.Any(r => r.nodeId == node),
   label + " zayıf dayanakla onaylanıyor.");
  report.Require(inv.SubmitWarrant(node, strong, 0) && inv.State.documentRequests.Any(r => r.nodeId == node),
   label + " doğru dayanakla onaylanmıyor.");
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;

  var unlinked = From(context); unlinked.State.read.Remove("link_ab");
  report.Forbid(unlinked.Discovered(N(data, "event_c")), "Olay C, A ile B bağlanmadan açılıyor.");
  report.Require(N(data, "archive").reopenYear == "2006", "Arşiv kaydı 'yeniden açıldı' kartını taşımalı.");
  report.Require(!string.IsNullOrEmpty(data.closedStampKey) && !string.IsNullOrEmpty(data.coldCaseTitleKey), "Dosya 'ayrıldı' sonu ve soğuk dosya kartı taşımalı.");

  Retry(context, "link_ab", new[] { "env_a", "tolga" }, new[] { "env_a", "env_b" }, "A ↔ B bağlantısı");
  Retry(context, "archive", new[] { "photo", "env_a" }, new[] { "photo", "aylin" }, "Arşiv taraması");
  Retry(context, "search", new[] { "plate", "zeynep" }, new[] { "plate", "scene" }, "Araç araması");

  var first = data.verdicts.First(v => v.correct);
  var entry = data.methods.First(m => m.correct);
  var past = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(first.id == "burak" && entry.id == "zeynep" && past.id == "push_fall", "Doğru rapor: Burak + Zeynep + itme düşme değil.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   Src(data, first.supportingSourceIds[0]), Src(data, entry.supportingSourceIds[0]), Src(data, proof.supportingSourceIds[0]),
   c, Src(data, past.supportingSourceIds[0]));

  var correct = From(context);
  if (!report.Step(Submit(correct, first.id, entry.id, past.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported,
   "Doğru rapor faksta desteklenmedi.");

  // "Tek intikamcı" kolay açıklaması: eve de Burak girdi.
  var avenger = From(context);
  report.Require(Submit(avenger, first.id, "burak", past.id) && avenger.Career.pendingReviews[0].evaluationType != "supported",
   "'Eve Burak girdi' doğru sayılıyor.");
  var pushed = From(context);
  report.Require(Submit(pushed, first.id, entry.id, "push_down") && pushed.Career.pendingReviews[0].evaluationType != "supported",
   "'Tolga aşağı itti' doğru sayılıyor.");
 }
}
}
