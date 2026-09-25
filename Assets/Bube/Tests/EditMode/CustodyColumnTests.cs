using NUnit.Framework;

namespace Bube.Tests {

// Rapor normalde üç sütunludur. Bir vaka tek olayda **iki ayrı sorumluluk**
// taşıyorsa (yaralayan ile parayı alan aynı kişi değil) dördüncü sütunu açar.
// Sütun isteğe bağlıdır: tanımlamayan vaka hiç değişmez.
public sealed class CustodyColumnTests {

 static Investigation WithCustody() {
  var game = Case001Walk.WalkToReportReady();
  game.Data.custodyLabelKey = "conclude.custody";
  game.Data.custody = new[] {
   new Choice{ id="taker", labelKey="conclude.custody", correct=true, supportingSourceIds=new[]{"recovery"} },
   new Choice{ id="other", labelKey="conclude.custody", correct=false }
  };
  return game;
 }

 [Test] public void CaseWithoutColumn_KeepsThreeAnswerReport() {
  var game = Case001Walk.WalkToReportReady();
  Assert.IsFalse(game.HasCustody);
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  Assert.AreEqual("supported", game.Career.pendingReviews[0].evaluationType);
 }

 [Test] public void MissingFourthAnswer_IsRefused() {
  var game = WithCustody();
  Assert.IsTrue(game.HasCustody);
  Assert.IsFalse(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"),
   "Dördüncü sütun açıkken üç cevaplı rapor gönderilememeli.");
 }

 [Test] public void WrongFourthAnswer_IsFalseAccusation() {
  var game = WithCustody();
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery", "other", "recovery"));
  Assert.AreEqual("falseAccusation", game.Career.pendingReviews[0].evaluationType);
 }

 [Test] public void UnsupportedFourthAnswer_IsIncomplete() {
  var game = WithCustody();
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery", "taker", "mert_follow"));
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("incomplete", review.evaluationType);
  Assert.IsFalse(review.custodySupported);
 }

 [Test] public void SupportedFourthAnswer_ClosesTheCase() {
  var game = WithCustody();
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery", "taker", "recovery"));
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("supported", review.evaluationType);
  Assert.AreEqual("taker", review.custodyId);
  Assert.IsTrue(review.custodySupported);
  Assert.AreEqual("taker", game.State.reportCustody);
 }
}
}
