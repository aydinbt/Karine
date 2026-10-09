using NUnit.Framework;

namespace Bube.Tests {
// 3 Ekim 2026: rapor yalnız şüpheli ve ne ile/nasıl. Kaynak satırı seçilmez;
// gerekçe, doğru seçeneği gösteren kaynağın soruşturmada açılmış olmasıdır.
public sealed class TwoClaimReportTests {
 [Test] public void WalkedCase_RightPair_IsSupported() {
  var game = Case001Walk.WalkToReportReady();
  Assert.IsTrue(game.SubmitReport("hasan", "spare", null));
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("supported", review.evaluationType);
  Assert.IsNull(game.State.reportProof);
  Assert.IsNotNull(review.suspectSourceId, "Dayanak oyuncunun açtığı kaynaktan bulunmalı.");
 }
 [Test] public void RightPairGuessedWithoutSources_IsLucky() {
  var game = Case001Walk.Fresh();
  game.Read("report");
  Assert.IsTrue(game.SubmitReport("hasan", "spare", null));
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("lucky", review.evaluationType);
  Assert.IsTrue(review.correct);
  Assert.IsFalse(review.suspectSupported);
  Assert.Greater(review.trustDelta, 0);
  Assert.Less(review.trustDelta, game.TrustDeltaFor("supported", game.Difficulty));
 }
 [Test] public void WrongSuspect_IsFalseAccusation() {
  var game = Case001Walk.WalkToReportReady();
  Assert.IsTrue(game.SubmitReport("elif", "spare", null));
  Assert.AreEqual("falseAccusation", game.Career.pendingReviews[0].evaluationType);
 }
 [Test] public void UnknownChoice_IsRejected() {
  var game = Case001Walk.WalkToReportReady();
  Assert.IsFalse(game.SubmitReport("hasan", "yok", null));
  Assert.IsFalse(game.State.closed);
 }
}
}
