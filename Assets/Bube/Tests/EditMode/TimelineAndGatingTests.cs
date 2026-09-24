using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Zaman çizelgesi iğnelemesi ve soru açılma koşulları. Oyuncu güdümlü soruşturma
// kuralı gereği bir ipucu ya da soru yalnız önkoşulları karşılandığında açılır;
// hiçbiri oyuncuya sıradaki adımı söylemez, yalnız erişimi açar.
public sealed class TimelineAndGatingTests {

 static CaseData Case001() => JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/case001").text);

 // Kapıların kendisini sınamak istediğimizde rotayı yeniden oynamak gereksiz;
 // ilerleme doğrudan kurulur. Kayıt biçimi testleri SaveSchemaTests'te.
 static Investigation With(params string[] read) {
  var progress = new Progress { caseId = "case001", caseAccepted = true };
  progress.read.AddRange(read);
  return new Investigation(Case001(), progress);
 }

 static Investigation AtDesk() { var game = With(); Assert.IsTrue(game.Read("report")); return game; }

 static TimelineClue Clue(Investigation game, string id) {
  var clue = game.Data.timelineClues.FirstOrDefault(c => c.id == id);
  Assert.IsNotNull(clue, "İpucu yok: " + id);
  return clue;
 }

 [Test]
 public void Timeline_IsLockedBeforeItsSourceIsRead() {
  var locked = With("report");
  var clue = Clue(locked, "camera_gap");
  Assert.IsFalse(locked.TimelineAvailable(clue), "Kamera okunmadan CCTV ipucu açılmamalı.");
  Assert.IsFalse(locked.PinTimeline(clue.id), "Erişilemeyen ipucu iğnelenemez.");

  var opened = With("report", "camera");
  Assert.IsTrue(opened.TimelineAvailable(Clue(opened, "camera_gap")), "Kamera okunduktan sonra ipucu açılmalı.");
 }

 [Test]
 public void Timeline_IsLockedBeforeItsQuestionIsAsked() {
  var game = AtDesk();
  var clue = Clue(game, "mert_out_claim");
  Assert.IsFalse(game.TimelineAvailable(clue), "İfade sorulmadan ipucu açılmamalı.");
  Assert.IsTrue(game.RequestInterview("mert", 0));
  Assert.IsTrue(game.Ask("mert", "mert.day"));
  Assert.IsTrue(game.TimelineAvailable(clue), "İfade sorulduktan sonra ipucu açılmalı.");
 }

 [Test]
 public void PinAndUnpin_AreIdempotentAndSaved() {
  var game = With("report", "camera");
  var clue = Clue(game, "camera_gap");
  Assert.IsTrue(game.PinTimeline(clue.id), "İlk iğneleme başarılı olmalı.");
  Assert.IsFalse(game.PinTimeline(clue.id), "Aynı ipucu iki kez iğnelenmemeli.");
  Assert.Contains(clue.id, game.State.timelinePinned);

  var restored = JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
  Assert.Contains(clue.id, restored.timelinePinned, "İğneleme kayıttan sağ çıkmalı.");

  Assert.IsTrue(game.UnpinTimeline(clue.id));
  Assert.IsFalse(game.UnpinTimeline(clue.id), "Zaten kaldırılmış ipucu yine kaldırılamaz.");
  Assert.IsFalse(game.State.timelinePinned.Contains(clue.id));
 }

 [Test]
 public void UnknownTimelineId_IsRejected() {
  var game = With("report", "camera");
  Assert.IsFalse(game.PinTimeline("boyle-bir-ipucu-yok"));
  Assert.IsFalse(game.UnpinTimeline("boyle-bir-ipucu-yok"));
 }

 // Kaynak düğümü de aynı kurala tabi: kamera, Hasan onu anmadan açılmaz.
 [Test]
 public void Source_StaysClosedUntilAClaimNamesIt() {
  var game = AtDesk();
  var camera = game.Data.nodes.First(n => n.id == "camera");
  Assert.IsFalse(game.Discovered(camera), "Kamera bir iddia olmadan bulunmuş sayılmamalı.");
  Assert.IsFalse(game.Read("camera"), "Bulunmamış kaynak okunamaz.");
 }

 [Test]
 public void Question_StaysClosedUntilItsPrerequisiteQuestionIsAsked() {
  var game = AtDesk();
  var node = game.Data.nodes.First(n => n.kind == "interview" &&
   (n.questions ?? new Question[0]).Any(q => q.requiresAsked != null && q.requiresAsked.Length > 0));
  var gated = node.questions.First(q => q.requiresAsked != null && q.requiresAsked.Length > 0);
  Assert.IsTrue(game.RequestInterview(node.id, 0), "Görüşme talebi kabul edilmeli: " + node.id);
  Assert.IsFalse(game.QuestionAvailable(node, gated), "Önkoşul sorusu sorulmadan soru açılmamalı.");
  Assert.IsFalse(game.Ask(node.id, gated.id), "Kapalı soru sorulamaz.");
  foreach (var id in gated.requiresAsked) {
   var owner = game.Data.nodes.First(n => (n.questions ?? new Question[0]).Any(q => q.id == id));
   if (owner.kind == "interview" && game.CanRequest(owner)) game.RequestInterview(owner.id, 0);
   Assert.IsTrue(game.Ask(owner.id, id), "Önkoşul sorusu sorulamadı: " + id);
  }
  Assert.IsTrue(game.QuestionAvailable(node, gated), "Önkoşul sorulduktan sonra soru açılmalı.");
 }

 [Test]
 public void AskingTwice_DoesNotDuplicateTheTranscript() {
  var game = AtDesk();
  Assert.IsTrue(game.RequestInterview("mert", 0));
  Assert.IsTrue(game.Ask("mert", "mert.day"));
  Assert.IsFalse(game.Ask("mert", "mert.day"), "Kaynaksız soru iki kez sorulamaz.");
  Assert.AreEqual(1, game.State.interviewTurns.Count(t => t.questionId == "mert.day"));
 }
}
}
