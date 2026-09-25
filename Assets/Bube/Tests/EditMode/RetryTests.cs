using System.Linq;
using NUnit.Framework;

namespace Bube.Tests {

// Ödüllü yeniden deneme. Kullanıcı kararı: güven geri verilir, faks geçmişi
// başarısızlığı saklar. Buradaki testler bu iki cümlenin kodda karşılığı.
public sealed class RetryTests {

 // Yanlış üçlü gönderip faksı teslim alır; dönen faks başarısızdır.
 static Investigation Failed(out FaxReview fax) {
  var game = Case001Walk.WalkToReportReady();
  Assert.IsTrue(game.SubmitFinalReport("elif", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  game.BeginNextCaseReview(5);
  game.Career.pendingReviews[0].readyAtUtcTicks = 1;
  fax = game.DeliverNextFax();
  Assert.IsNotNull(fax, "Faks teslim edilmedi.");
  Assert.IsFalse(fax.correct, "Bu rapor başarısız olmalıydı.");
  return game;
 }

 [Test] public void Reopening_RefundsExactlyWhatTheFaxTook() {
  var game = Failed(out var fax);
  int afterFax = game.Career.departmentTrust;
  Assert.Less(fax.trustChange, 0, "Yanlış suçlama güven düşürmeliydi.");
  Assert.IsTrue(game.MayReopen, "Başarısız vaka yeniden açılabilir olmalı.");
  Assert.IsTrue(game.ReopenForRetry());
  Assert.AreEqual(afterFax - fax.trustChange, game.Career.departmentTrust,
   "Geri verilen puan, o faksın götürdüğü kadar olmalı.");
 }

 // Kullanıcı kararı: kayıt silinmez. Faks geçmişinde satır durur ve yeniden
 // açıldığı yazar; sayımlar da bu satırı görmeye devam eder.
 [Test] public void Reopening_KeepsTheFailureInTheFaxHistory() {
  var game = Failed(out var fax);
  Assert.IsTrue(game.ReopenForRetry());
  var record = game.Career.reviewHistory.SingleOrDefault(r => r.caseId == game.Data.id);
  Assert.IsNotNull(record, "Başarısızlık kaydı geçmişten silinmiş.");
  Assert.AreEqual(fax.evaluationType, record.evaluationType, "Değerlendirme türü değişmemeli.");
  Assert.IsTrue(record.reopened, "Kayıt 'yeniden açıldı' diye işaretlenmeli.");
  Assert.IsTrue(record.trustRefunded, "Puanın geri verildiği kayıtta durmalı.");
 }

 // Yeniden açılan vaka ikinci kez gönderilebilir, ama geçmişteki eski satır
 // yerinde kalır: aynı vaka iki satır tutar.
 [Test] public void SecondAttempt_AddsItsOwnLineWithoutErasingTheFirst() {
  var game = Failed(out var first);
  Assert.IsTrue(game.ReopenForRetry());
  Assert.IsTrue(game.CanConclude, "Yeniden açılan vakada rapor gönderilebilmeli.");
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  game.BeginNextCaseReview(5);
  game.Career.pendingReviews[0].readyAtUtcTicks = 1;
  var second = game.DeliverNextFax();
  Assert.IsNotNull(second);
  Assert.IsTrue(second.correct, "İkinci deneme doğru üçlüyle başarılı olmalı.");
  var lines = game.Career.reviewHistory.Where(r => r.caseId == game.Data.id).ToList();
  Assert.AreEqual(2, lines.Count, "Vaka iki satır tutmalı: başarısızlık ve ikinci deneme.");
  Assert.AreEqual(first.evaluationType, lines[0].evaluationType, "İlk satır bozulmamalı.");
  Assert.IsTrue(lines[0].reopened);
  Assert.IsFalse(lines[1].reopened);
 }

 // Yeniden açmak soruşturmayı sıfırlamaz ve ipucu vermez: bulunanlar durur,
 // yalnız rapor alanları boşalır.
 [Test] public void Reopening_KeepsTheInvestigationAndClearsOnlyTheReport() {
  var game = Failed(out _);
  int read = game.State.read.Count, asked = game.State.asked.Count;
  Assert.IsTrue(game.ReopenForRetry());
  Assert.AreEqual(read, game.State.read.Count, "Okunanlar silinmemeli.");
  Assert.AreEqual(asked, game.State.asked.Count, "Sorulanlar silinmemeli.");
  Assert.IsFalse(game.State.closed, "Vaka yeniden açık olmalı.");
  Assert.IsNull(game.State.reportSuspect);
  Assert.IsNull(game.State.reportMethod);
  Assert.IsNull(game.State.reportProof);
  Assert.AreEqual(0, game.State.submittedAtUtcTicks);
 }

 [Test] public void AnAlreadyReopenedFax_CannotBeReopenedAgain() {
  var game = Failed(out _);
  Assert.IsTrue(game.ReopenForRetry());
  Assert.IsFalse(game.MayReopen, "Aynı faks ikinci kez iade etmemeli.");
  Assert.IsFalse(game.ReopenForRetry());
 }

 [Test] public void ASupportedReport_IsNotOfferedARetry() {
  var game = Case001Walk.WalkToReportReady();
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  game.BeginNextCaseReview(5);
  game.Career.pendingReviews[0].readyAtUtcTicks = 1;
  Assert.IsTrue(game.DeliverNextFax().correct);
  Assert.IsFalse(game.MayReopen, "Başarılı vaka yeniden açılmaz.");
 }

 // Güven iadesi görevden ayrılmayı da kaldırır: yoksa ödül hiçbir işe yaramaz.
 [Test] public void Refund_LiftsRetirementWhenTrustComesBack() {
  var game = Case001Walk.WalkToReportReady();
  game.Career.departmentTrust = game.Rules.falseAccusationLoss;
  Assert.IsTrue(game.SubmitFinalReport("elif", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  game.BeginNextCaseReview(5);
  game.Career.pendingReviews[0].readyAtUtcTicks = 1;
  game.DeliverNextFax();
  Assert.IsTrue(game.Career.retired, "Sıfıra düşen güven görevi bitirmeliydi.");
  Assert.IsTrue(game.ReopenForRetry());
  Assert.IsFalse(game.Career.retired, "Puan geri verildiyse görev de geri gelmeli.");
 }
}
}
