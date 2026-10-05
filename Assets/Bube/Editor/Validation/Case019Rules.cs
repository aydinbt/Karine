using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #019 "Canlı Yayın" — yayın dosyadan oynatıldı; ekrandaki Felix iki saattir ölüydü.
// Yayın dökümü, hava kaydı, sunucu kaydı, kamera ve otopsi kimseyi adıyla anmaz; bar, sohbet oturumu,
// konum ve kurgu kaydı Tobias'ı anar. Sophie'yi, yayın sonrasını ya da "düştü"yü seçen rapor desteklenmez.
public static class Case019Rules {

 static Investigation From(CaseContext context) =>
  new Investigation(context.Data, JsonUtility.FromJson<Progress>(context.ReadySnapshot)) { Text = context.Locale };
 static Node N(CaseData data, string id) => data.nodes.First(n => n.id == id);

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var report = context.Report;
  var probe = From(context);
  string Body(string id) => context.Locale.Get(N(data, id).bodyKey);

  foreach (var src in new[] { "transcript", "weather", "encoder", "door", "autopsy" })
   foreach (var person in new[] { "sophie", "ines", "tobias" })
    report.Forbid(probe.MentionsPerson(person, Body(src)), $"{src} {person} adını anıyor.");
  foreach (var src in new[] { "bar", "chatlog", "tphone", "edit" })
   report.Require(probe.MentionsPerson("tobias", Body(src)), $"{src} tobias adını anmıyor.");
  foreach (var src in new[] { "report" })
   report.Require(probe.MentionsPerson("sophie", Body(src)), $"{src} sophie adını anmıyor.");

  var who = data.verdicts.First(v => v.correct);
  var how = data.methods.First(m => m.correct);
  var after = data.custody.First(c => c.correct);
  var proof = data.evidence.First(e => e.correct);
  report.Require(who.id == "tobias" && how.id == "struck" && after.id == "before" && proof.id == "encoder", "Doğru rapor: tobias + struck + before + encoder.");

  bool Submit(Investigation inv, string s, string m, string c) => inv.SubmitFinalReport(s, m, proof.id,
   who.supportingSourceIds[0], how.supportingSourceIds[0], proof.supportingSourceIds[0], c, after.supportingSourceIds[0]);
  var correct = From(context);
  if (!report.Step(Submit(correct, who.id, how.id, after.id), "Desteklenen rapor reddedildi.")) return;
  correct.BeginNextCaseReview(8);
  correct.Career.pendingReviews[0].readyAtUtcTicks = System.DateTime.UtcNow.AddTicks(-1).Ticks;
  var fax = correct.DeliverNextFax();
  report.Require(fax != null && fax.correct && fax.suspectSupported && fax.methodSupported && fax.custodySupported, "Doğru rapor faksta desteklenmedi.");
  var w0 = From(context);
  report.Require(Submit(w0, "sophie", how.id, after.id) && w0.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: sophie");
  var w1 = From(context);
  report.Require(Submit(w1, who.id, how.id, "after") && w1.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: after");
  var w2 = From(context);
  report.Require(Submit(w2, who.id, "fell", after.id) && w2.Career.pendingReviews[0].evaluationType != "supported", "Yanlış rapor doğru sayılıyor: fell");
 }
}
}
