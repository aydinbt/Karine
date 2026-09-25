using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Dosya #001'in soruşturma mantığı Investigation.cs içinde ve Unity'den bağımsız.
// Bu testler Faz 2 oynanış betiğinin (Docs/PLAYTEST_001.md) 1–3. bölümlerinin
// mantık tarafını otomatikleştirir. Görsel taraf (video, düzen, glif, dokunma
// hedefi, kare hızı) hâlâ gözle ve cihazda doğrulanır.
public sealed class CaseFlowTests {

 static CaseData Case001() => Case001Walk.Case001();
 static Investigation Fresh() => Case001Walk.Fresh();
 static Node Node(Investigation game, string id) => Case001Walk.Node(game, id);
 static bool Discovered(Investigation game, string id) => Case001Walk.Discovered(game, id);
 static void OpenInterview(Investigation game, string id) => Case001Walk.OpenInterview(game, id);
 static void Ask(Investigation game, string node, string question, string source = null) =>
  Case001Walk.Ask(game, node, question, source);
 static Investigation WalkToReportReady() => Case001Walk.WalkToReportReady();

 [Test] public void ForwardRoute_ReachesASupportedReport() {
  var game = WalkToReportReady();
  Assert.IsTrue(game.CanConclude, "Rapor gönderilemiyor.");
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"),
   "Doğru üçlü kabul edilmedi.");
  Assert.IsTrue(game.State.closed, "Vaka kapanmadı.");
  Assert.AreEqual(1, game.Career.pendingReviews.Count);
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("supported", review.evaluationType);
  Assert.IsTrue(review.correct);
  Assert.AreEqual(game.Rules.strongGain, review.trustDelta);
 }

 // Kilit zinciri: her kaynak yalnız kendi koşulu karşılandığında erişilebilir olmalı.
 [Test] public void Gates_OpenOnlyWhenTheirPrerequisiteIsMet() {
  var game = Fresh();
  Assert.IsFalse(Discovered(game, "mert"), "Dosya okunmadan mert açılmamalı.");
  game.Read("report");
  Assert.IsTrue(Discovered(game, "mert"));
  Assert.IsFalse(Discovered(game, "elif"), "mert.key sorulmadan elif açılmamalı.");
  Assert.IsFalse(Discovered(game, "hasan"), "mert.neighbor sorulmadan hasan açılmamalı.");
  Assert.IsFalse(Discovered(game, "camera"), "hasan.camera sorulmadan kamera açılmamalı.");

  OpenInterview(game, "mert");
  Ask(game, "mert", "mert.day");
  Ask(game, "mert", "mert.key");
  Assert.IsTrue(Discovered(game, "elif"), "mert.key sorulunca elif açılmalı.");
  Assert.IsFalse(Discovered(game, "hasan"));
  Ask(game, "mert", "mert.neighbor");
  Assert.IsTrue(Discovered(game, "hasan"), "mert.neighbor sorulunca hasan açılmalı.");

  OpenInterview(game, "hasan");
  Ask(game, "hasan", "hasan.sighting");
  Assert.IsFalse(Discovered(game, "camera"));
  Ask(game, "hasan", "hasan.camera");
  Assert.IsTrue(Discovered(game, "camera"), "hasan.camera sorulunca kamera açılmalı.");

  Assert.IsFalse(Discovered(game, "mert_follow"), "Kamera okunmadan takip görüşmesi açılmamalı.");
  Assert.IsFalse(Discovered(game, "recovery"), "Kamera okunmadan eşya raporu açılmamalı.");
  game.Read("camera");
  // mert_follow'un iki kapısı var: mert+camera okunmuş olmalı VE
  // requiresAnyAsked ile elif_follow.footage ya da isteğe bağlı mert.lock sorulmuş olmalı.
  Assert.IsFalse(Discovered(game, "mert_follow"),
   "mert.lock ya da elif_follow.footage olmadan mert_follow açılmamalı.");
  Ask(game, "mert", "mert.lock");
  Assert.IsTrue(Discovered(game, "mert_follow"),
   "mert.lock ikinci rotayı açmalı (requiresAnyAsked).");

  Assert.IsFalse(Discovered(game, "recovery"),
   "Eşya raporu üç takip görüşmesinden en az biri okunmadan açılmamalı.");
  OpenInterview(game, "mert_follow");
  Ask(game, "mert_follow", "mert_follow.spare");
  Assert.IsTrue(Discovered(game, "recovery"), "Bir takip görüşmesi yetmeli (requiresAny).");
  // İki kapı birlikte çalışır: bu testte hasan görüşmesi tamamlanmadı, bu yüzden
  // mert_follow.spare requiresAnyAsked'i karşılasa bile requires kapısı kapalı kalmalı.
  Assert.IsFalse(game.State.read.Contains("hasan"), "hasan görüşmesi bu rotada tamamlanmadı.");
  Assert.IsFalse(Discovered(game, "hasan_follow"),
   "requiresAnyAsked tek başına yetmemeli; requires da karşılanmalı.");
  Ask(game, "hasan", "hasan.where");
  Assert.IsTrue(game.State.read.Contains("hasan"));
  Assert.IsTrue(Discovered(game, "hasan_follow"),
   "İki kapı da karşılanınca hasan_follow açılmalı.");
 }

 // Takip görüşmelerine iki ayrı rota var; ikincisi isteğe bağlı mert.lock sorusundan geçer.
 // ProjectSetup doğrulayıcısı da bu iki rotayı simüle eder.
 [Test] public void FollowUps_HaveTwoIndependentRoutes() {
  var viaElif = Fresh();
  viaElif.Read("report");
  OpenInterview(viaElif, "mert");
  Ask(viaElif, "mert", "mert.day"); Ask(viaElif, "mert", "mert.key"); Ask(viaElif, "mert", "mert.neighbor");
  OpenInterview(viaElif, "elif");
  Ask(viaElif, "elif", "elif.relationship"); Ask(viaElif, "elif", "elif.visit");
  OpenInterview(viaElif, "hasan");
  Ask(viaElif, "hasan", "hasan.sighting"); Ask(viaElif, "hasan", "hasan.camera"); Ask(viaElif, "hasan", "hasan.where");
  viaElif.Read("camera");
  Assert.IsFalse(Discovered(viaElif, "mert_follow"), "Henüz iki kapıdan biri de açılmadı.");
  Assert.IsFalse(Discovered(viaElif, "hasan_follow"));
  OpenInterview(viaElif, "elif_follow");
  Ask(viaElif, "elif_follow", "elif_follow.footage", "camera#elif_in");
  Assert.IsTrue(Discovered(viaElif, "mert_follow"), "Görüntü sunumu mert_follow'u açmalı.");
  Assert.IsTrue(Discovered(viaElif, "hasan_follow"), "Görüntü sunumu hasan_follow'u açmalı.");
  Assert.IsFalse(viaElif.State.asked.Contains("mert.lock"),
   "Bu rota isteğe bağlı mert.lock sorusunu gerektirmemeli.");
 }

 // Yarım bırakılan görüşme okundu sayılmamalı ve bağlı kapıyı açmamalı.
 [Test] public void IncompleteInterview_DoesNotCountAsRead() {
  var game = Fresh();
  game.Read("report");
  OpenInterview(game, "mert");
  Ask(game, "mert", "mert.day");
  Ask(game, "mert", "mert.key");
  Assert.IsFalse(game.State.read.Contains("mert"), "Eksik görüşme okundu sayılmamalı.");
  Assert.IsFalse(game.Read("mert"), "Eksik görüşme elle okunamamalı.");
  OpenInterview(game, "elif");
  Ask(game, "elif", "elif.relationship");
  game.Read("camera");
  Assert.IsFalse(Discovered(game, "elif_follow"),
   "elif tamamlanmadan elif_follow açılmamalı.");
 }

 // Yanlış kaynak sunma reddedilmeli ve ilerlemeyi bozmamalı.
 [Test] public void WrongSource_IsRejectedWithoutSideEffects() {
  var game = Fresh();
  game.Read("report");
  OpenInterview(game, "mert");
  Ask(game, "mert", "mert.day"); Ask(game, "mert", "mert.key"); Ask(game, "mert", "mert.neighbor");
  OpenInterview(game, "elif");
  Ask(game, "elif", "elif.relationship"); Ask(game, "elif", "elif.visit");
  OpenInterview(game, "hasan");
  Ask(game, "hasan", "hasan.sighting"); Ask(game, "hasan", "hasan.camera"); Ask(game, "hasan", "hasan.where");
  game.Read("camera");
  OpenInterview(game, "elif_follow");

  int askedBefore = game.State.asked.Count;
  int turnsBefore = game.State.interviewTurns.Count;
  Assert.IsFalse(game.Ask("elif_follow", "elif_follow.footage", "camera#gap"),
   "İlgisiz CCTV anı kabul edilmemeli.");
  Assert.IsFalse(game.Ask("elif_follow", "elif_follow.footage", "report"),
   "İlgisiz belge kabul edilmemeli.");
  Assert.IsFalse(game.Ask("elif_follow", "elif_follow.footage", null),
   "Kaynak isteyen soru kaynaksız sorulmamalı.");
  Assert.IsFalse(game.Ask("elif_follow", "elif_follow.footage", "camera#kayitYok"),
   "Var olmayan CCTV anı kabul edilmemeli.");
  Assert.AreEqual(askedBefore, game.State.asked.Count, "Reddedilen sunum ilerlemeyi değiştirmemeli.");
  Assert.AreEqual(turnsBefore, game.State.interviewTurns.Count);

  Ask(game, "elif_follow", "elif_follow.footage", "camera#elif_out");
  Assert.AreEqual(turnsBefore + 1, game.State.interviewTurns.Count);
 }

 // Talep edilen belge gecikmesi dolmadan gelen kutusuna düşmemeli.
 [Test] public void RequestedDocument_WaitsForItsDelay() {
  var game = Fresh();
  game.Read("report");
  OpenInterview(game, "mert");
  Ask(game, "mert", "mert.day"); Ask(game, "mert", "mert.key"); Ask(game, "mert", "mert.neighbor");
  OpenInterview(game, "hasan");
  Ask(game, "hasan", "hasan.sighting"); Ask(game, "hasan", "hasan.camera"); Ask(game, "hasan", "hasan.where");
  game.Read("camera");
  Ask(game, "mert", "mert.lock");
  OpenInterview(game, "mert_follow");
  Ask(game, "mert_follow", "mert_follow.spare");

  Assert.IsTrue(game.RequestDocument("recovery", 120), "Eşya raporu talep edilemedi.");
  Assert.IsFalse(game.HasIncomingDocument, "Gecikme dolmadan gelen kutusuna düşmemeli.");
  Assert.IsFalse(game.ReceiveDocument("recovery"), "Gecikme dolmadan teslim alınmamalı.");
  Assert.IsFalse(game.RequestDocument("recovery", 0), "Aynı belge iki kez talep edilmemeli.");
 }

 // Uygulamayı kapatıp açma: kayıt gidip geldiğinde ilerleme ve kapılar korunmalı.
 [Test] public void SaveRoundTrip_PreservesProgressAndGates() {
  var game = WalkToReportReady();
  var reloaded = new Investigation(Case001(),
   JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State)),
   JsonUtility.FromJson<CareerProgress>(JsonUtility.ToJson(game.Career)));

  Assert.AreEqual(game.State.read.Count, reloaded.State.read.Count, "Okunanlar kaybolmamalı.");
  Assert.AreEqual(game.State.asked.Count, reloaded.State.asked.Count, "Sorulanlar kaybolmamalı.");
  Assert.AreEqual(game.State.interviewTurns.Count, reloaded.State.interviewTurns.Count,
   "Görüşme dökümü kaybolmamalı.");
  Assert.IsTrue(reloaded.State.caseAccepted, "Dosya kabulü korunmalı.");
  Assert.IsTrue(reloaded.CanConclude, "Yeniden yüklemeden sonra rapor gönderilebilmeli.");
  Assert.IsTrue(reloaded.ReportSourceAvailable("mert_follow"), "Kaynaklar korunmalı.");
  Assert.IsTrue(reloaded.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"),
   "Yeniden yüklenen kayıtla doğru rapor gönderilemedi.");
 }

 [Test] public void WrongSuspect_IsScoredAsFalseAccusation() {
  var game = WalkToReportReady();
  Assert.IsTrue(game.SubmitFinalReport("elif", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("falseAccusation", review.evaluationType);
  Assert.IsFalse(review.correct);
  Assert.AreEqual(-game.Rules.falseAccusationLoss, review.trustDelta);
 }

 [Test] public void RightSuspectWrongMethod_IsScoredAsIncomplete() {
  var game = WalkToReportReady();
  Assert.IsTrue(game.SubmitFinalReport("hasan", "forced", "recovery", "recovery", "mert_follow", "recovery"));
  var review = game.Career.pendingReviews[0];
  Assert.AreEqual("incomplete", review.evaluationType);
  Assert.IsFalse(review.correct);
  Assert.AreEqual(-game.Rules.incompleteLoss, review.trustDelta);
 }

 [Test] public void UnreadEvidence_CannotBeSubmitted() {
  var game = Fresh();
  game.Read("report");
  Assert.IsTrue(game.CanConclude, "conclusionRequires yalnız dosyayı istiyor.");
  Assert.IsFalse(game.SubmitFinalReport("hasan", "spare", "recovery", "report", "report", "report"),
   "Okunmamış kanıt raporda kullanılamamalı.");
  Assert.IsFalse(game.State.closed, "Reddedilen rapor vakayı kapatmamalı.");
 }

 [Test] public void ClosedCase_AcceptsNoFurtherWork() {
  var game = WalkToReportReady();
  Assert.IsTrue(game.SubmitFinalReport("hasan", "spare", "recovery", "recovery", "mert_follow", "recovery"));
  Assert.IsFalse(game.CanConclude, "Kapanan vaka yeniden gönderilmemeli.");
  Assert.IsFalse(game.Ask("hasan_follow", "hasan_follow.memory"), "Kapanan vakada soru sorulmamalı.");
  Assert.IsFalse(Discovered(game, "mert"), "Kapanan vakada kaynak açık kalmamalı.");
 }

 // Rapor kaynağı doğrulaması: okunmamış düğüm ve olay kimliği olmayan CCTV reddedilir.
 [Test] public void ReportSources_RejectUnreadAndMalformedReferences() {
  var game = Fresh();
  Assert.IsFalse(game.ReportSourceAvailable("report"), "Okunmadan kaynak olmamalı.");
  Assert.IsFalse(game.ReportSourceAvailable(null));
  Assert.IsFalse(game.ReportSourceAvailable("yokBoyleDugum"));
  game.Read("report");
  Assert.IsTrue(game.ReportSourceAvailable("report"));
  Assert.IsFalse(game.ReportSourceAvailable("report#birSey"),
   "Belge kaynağı alt kimlik almamalı.");
 }
}
}
