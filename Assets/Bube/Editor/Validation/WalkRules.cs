using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Vakayı baştan sona otomatik oynayan bölüm: her düğümü açar, açılan her soruyu
// sorar, belge taleplerini gelen kutusundan geçirir. Sonunda bütün düğümler
// okunmuş ve rapor gönderilebilir olmalı — erişilemeyen içeriğin tek kontrolü bu.
public static class WalkRules {

 public static bool BeforeAccept(CaseData data, Investigation game, ValidationReport report) {
  if (!report.Step(!game.Available(data.nodes[0]), "Vaka kabul edilmeden kaynak açık.")) return false;
  if (!report.Step(!game.CanConclude && !game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"),
   "Vaka kabul edilmeden rapor gönderilebiliyor.")) return false;
  return report.Step(game.AcceptCase(), "Vaka kabulü başarısız.");
 }

 public static void Walk(CaseData data, Investigation game, ValidationReport report) {
  for (int i = 0; i < data.nodes.Length; i++)
   foreach (var node in data.nodes.Where(game.Discovered)) {
    if (node.kind == "interview" && game.CanRequest(node)) game.RequestInterview(node.id, 0);
    if (game.CanRequestDocument(node))
     report.Require(game.RequestDocument(node.id, 0) && !game.Available(node) && game.HasIncomingDocument &&
      game.ReceiveDocument(node.id) && !game.ReceiveDocument(node.id),
      "Belge talebi/gelen kutusu akışı geçersiz: " + node.id);
    foreach (var question in node.questions ?? new Question[0])
     if (game.QuestionAvailable(node, question)) {
      var chosen = question.presentedSourceIds != null && question.presentedSourceIds.Length > 0
       ? question.presentedSourceIds[0] : question.presentedSourceId;
      report.Forbid(game.QuestionNeedsSource(question) && game.ReportSourceAvailable(chosen) &&
       game.Ask(node.id, question.id, "report"), "İlgisiz kaynak kabul edildi: " + question.id);
      game.Ask(node.id, question.id, chosen);
     }
    if (game.Available(node)) game.Read(node.id);
   }
  report.Require(game.State.read.Count == data.nodes.Length && game.CanConclude,
   "Erişilemeyen içerik: okunan " + game.State.read.Count + "/" + data.nodes.Length +
   (game.CanConclude ? "" : ", rapor gönderilemiyor"));
 }

 public static void AfterWalk(CaseData data, Locale locale, Investigation game, ValidationReport report) {
  var timeline = data.timelineClues ?? new TimelineClue[0];
  if (timeline.Length > 0) {
   var clue = timeline[0];
   if (report.Step(game.TimelineAvailable(clue) && game.PinTimeline(clue.id) && !game.PinTimeline(clue.id),
    "Zaman çizelgesi iğnelemesi başarısız.")) {
    var restored = JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
    report.Require(restored.timelinePinned.Contains(clue.id) && game.UnpinTimeline(clue.id) && !game.UnpinTimeline(clue.id),
     "Zaman çizelgesi kaydı/kaldırması başarısız.");
   }
  }

  foreach (var request in game.State.documentRequests) {
   var restored = JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
   report.Require(restored.documentRequests.Any(r => r.nodeId == request.nodeId && r.readyAtUtcTicks == request.readyAtUtcTicks),
    "Belge talebi kayıt/yüklemeden sağ çıkmadı: " + request.nodeId);
  }

  report.Forbid(game.State.interviewTurns.Count == 0 || game.State.interviewTurns.Any(turn =>
   string.IsNullOrEmpty(turn.answerKey) || locale.Get(turn.answerKey).StartsWith("[")),
   "Görüşme dökümünde kayıtlı yanıt eksik.");
  report.Forbid(game.State.interviewTurns.Any(turn => data.nodes.SelectMany(n => n.questions ?? new Question[0])
    .Any(q => q.id == turn.questionId && (!string.IsNullOrEmpty(q.presentedSourceId) && q.presentedSourceId != turn.sourceId ||
     q.presentedSourceIds != null && q.presentedSourceIds.Length > 0 && !q.presentedSourceIds.Contains(turn.sourceId)))),
   "Sunulan kaynak dökümde yok.");

  var restoredProgress = JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
  report.Require(restoredProgress.interviewTurns.Count == game.State.interviewTurns.Count &&
   !restoredProgress.interviewTurns.Where((turn, index) => turn.answerKey != game.State.interviewTurns[index].answerKey).Any(),
   "Görüşme dökümü kayıt/yüklemeden sağ çıkmadı.");

  report.Forbid(data.nodes.Any(n => n.kind == "cctv") && !data.nodes.Any(n => n.kind == "cctv" && game.Meets(n.requires)),
   "Kamera erişilemez.");
 }
}
}
