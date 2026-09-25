using System;
using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Dosya #001'e özel iddialar. Genel doğrulayıcı bunları bilmez; buradaki her
// kontrol yalnız bu vakanın tasarımını sabitler.
public static class Case001Rules {

 // Gezintiden önce: kaynakların oyuncu onları bulmadan açılmadığını kanıtlar.
 public static void Early(CaseContext context) {
  var data = context.Data; var locale = context.Locale; var report = context.Report;

  // Sinyal boşluğunun görüntüsü olmamalı — tasarım kanonu.
  foreach (var node in data.nodes.Where(n => n.kind == "cctv"))
   report.Forbid((node.cctvEvents ?? new CctvEvent[0]).Any(e => (e.id == "gap" || e.id == "lost") && !string.IsNullOrEmpty(e.videoPath)),
    "Dosya #001 sinyal boşluğunun görüntüsü olmamalı.");

  var pace = new Investigation(data);
  pace.AcceptCase();
  pace.Read("report");
  var byId = data.nodes.ToDictionary(n => n.id);
  report.Forbid(pace.Discovered(byId["elif"]) || pace.Discovered(byId["hasan"]) || pace.Discovered(byId["camera"]),
   "Kaynaklar oyuncu bulmadan açıldı.");
  if (!report.Step(pace.RequestInterview("mert", 0) && pace.Ask("mert", "mert.day") && pace.Ask("mert", "mert.key"),
   "Mert tanıtımı başarısız.")) return;
  report.Forbid(!pace.Discovered(byId["elif"]) || pace.Discovered(byId["mert_follow"]) || pace.Discovered(byId["hasan"]),
   "Anahtar rotası temposu bozuk.");
  report.Require(pace.Ask("mert", "mert.neighbor") && pace.Discovered(byId["hasan"]) && !pace.Discovered(byId["camera"]),
   "Komşu rotası temposu bozuk.");
  report.Require(pace.RequestInterview("hasan", 0) && pace.Ask("hasan", "hasan.sighting") &&
   !pace.Discovered(byId["camera"]) && pace.Ask("hasan", "hasan.camera") && pace.Discovered(byId["camera"]),
   "Kamera rotası temposu bozuk.");
  report.Require(!pace.Discovered(byId["mert_follow"]) && pace.Read("camera") && !pace.Discovered(byId["mert_follow"]),
   "Takip görüşmesi ilgili bir iddia olmadan açıldı.");

  var elifPath = new Investigation(data, JsonUtility.FromJson<Progress>(JsonUtility.ToJson(pace.State)));
  report.Require(elifPath.RequestInterview("elif", 0) && elifPath.Ask("elif", "elif.relationship") &&
   elifPath.Ask("elif", "elif.visit") && elifPath.RequestInterview("elif_follow", 0) &&
   elifPath.Ask("elif_follow", "elif_follow.footage", "camera#elif_in") && elifPath.Discovered(byId["mert_follow"]),
   "Elif'in açıklaması Mert takibini açmadı.");
  report.Require(locale.Get("elif_follow.footage.entryAnswer").Contains("saksı"),
   "Elif'in giriş yanıtı anahtar ipucunu atlıyor.");
  report.Require(pace.Ask("mert", "mert.lock") && pace.Discovered(byId["mert_follow"]),
   "Kilitli kapı rotası Mert takibini açmadı.");
  report.Forbid(pace.Discovered(byId["recovery"]), "Kurtarma talebi takip kaynağından önce açıldı.");

  report.Require(CaseSearch.Find(context.Game, locale, "12.37").Length == 0, "Okunmamış CCTV aramaya sızdı.");
 }

 public static void AfterWalk(CaseContext context) {
  var data = context.Data; var locale = context.Locale; var game = context.Game;
  var report = context.Report; var snapshot = context.ReadySnapshot;

  Search(data, locale, game, report);
  Presenting(data, snapshot, report);
  Routes(data, snapshot, report);
  Reporting(data, snapshot, report);
 }

 static void Search(CaseData data, Locale locale, Investigation game, ValidationReport report) {
  report.Require(CaseSearch.Find(game, locale, "12.37").Any(hit => hit.nodeId == "camera" && hit.eventId == "gap"),
   "İncelenen CCTV satırı aramada yok.");
  report.Require(CaseSearch.Find(game, locale, "Geçen ay kapıda kalmıştım")
   .Any(hit => hit.turnReference == "mert_follow#mert_follow.spare"), "Görüşme araması yanıtı bulamadı.");
  report.Require(CaseSearch.Find(game, locale, "BİLGİSAYAR").Any(), "Türkçe büyük/küçük harf duyarsız arama başarısız.");
  var privateAnswer = JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
  privateAnswer.interviewTurns.RemoveAll(t => t.questionId == "mert_follow.spare");
  report.Forbid(CaseSearch.Find(new Investigation(data, privateAnswer), locale, "Geçen ay kapıda kalmıştım").Any(),
   "Sorulmamış yanıt aramaya sızdı.");
 }

 static void Presenting(CaseData data, string snapshot, ValidationReport report) {
  var witnessState = JsonUtility.FromJson<Progress>(snapshot);
  witnessState.asked.Remove("hasan_follow.mertStatement");
  witnessState.interviewTurns.RemoveAll(t => t.questionId == "hasan_follow.mertStatement");
  var witnessGame = new Investigation(data, witnessState);
  const string statement = "mert_follow#mert_follow.spare";
  if (report.Step(witnessGame.ReportSourceAvailable(statement) &&
   !witnessGame.Ask("hasan_follow", "hasan_follow.mertStatement", "mert_follow") &&
   witnessGame.Ask("hasan_follow", "hasan_follow.mertStatement", statement),
   "Tanık yanıtı Hasan'a sunulamadı.")) {
   var turn = witnessGame.State.interviewTurns.Last(t => t.questionId == "hasan_follow.mertStatement");
   report.Require(turn.sourceId == statement && turn.answerKey == "hasan_follow.mertStatement.answer",
    "Tanık yüzleştirmesi kaydedilmedi.");
  }

  var absentState = JsonUtility.FromJson<Progress>(snapshot);
  absentState.asked.Remove("hasan_follow.mertStatement");
  absentState.interviewTurns.RemoveAll(t => t.questionId == "hasan_follow.mertStatement" || t.questionId == "mert_follow.spare");
  var absentGame = new Investigation(data, absentState);
  report.Forbid(absentGame.ReportSourceAvailable(statement) ||
   absentGame.Ask("hasan_follow", "hasan_follow.mertStatement", statement), "Erişilemeyen tanık yanıtı sunuldu.");

  var repeatState = JsonUtility.FromJson<Progress>(snapshot);
  repeatState.asked.Remove("elif_follow.footage");
  repeatState.interviewTurns.RemoveAll(t => t.questionId == "elif_follow.footage");
  var repeatGame = new Investigation(data, repeatState);
  var followNode = data.nodes.First(n => n.id == "elif_follow");
  var followQuestion = followNode.questions.First(q => q.id == "elif_follow.footage");
  // Soruyu birden çok kaynak kapatabilir, ama kişi cevabını bir kez verir:
  // kapanan soru listeden çıkar ve ikinci belirleyici kaynak da geri çevrilir.
  report.Require(repeatGame.Ask(followNode.id, followQuestion.id, "camera#elif_in") &&
   !repeatGame.CanAskQuestion(followNode, followQuestion), "Yanıtlanan soru kapanmadı.");
  report.Require(!repeatGame.Ask(followNode.id, followQuestion.id, "camera#elif_out"),
   "Kapanan soru ikinci kaynakla yeniden soruldu.");
  var repeatTurns = repeatGame.State.interviewTurns.Where(t => t.questionId == followQuestion.id).ToArray();
  report.Require(repeatTurns.Length == 1, "Kapanan soru için tek döküm satırı beklenir.");
  var repeatSaved = JsonUtility.FromJson<Progress>(JsonUtility.ToJson(repeatGame.State));
  report.Require(repeatSaved.interviewTurns.Count(t => t.questionId == followQuestion.id) == 1,
   "Takip dökümü kayıttan sağ çıkmadı.");

  foreach (var sourceId in new[] { "camera#elif_in", "camera#elif_out" }) {
   var turnState = JsonUtility.FromJson<Progress>(snapshot);
   turnState.asked.Remove("elif_follow.footage");
   turnState.interviewTurns.RemoveAll(t => t.questionId == "elif_follow.footage");
   var turnGame = new Investigation(data, turnState);
   if (!report.Step(!turnGame.Ask("elif_follow", "elif_follow.footage", "camera#gap") &&
    turnGame.Ask("elif_follow", "elif_follow.footage", sourceId),
    "CCTV satırı Elif takibini yönetmedi: " + sourceId)) continue;
   var recorded = turnGame.State.interviewTurns.Last(t => t.questionId == "elif_follow.footage");
   report.Require(recorded.sourceId == sourceId && recorded.answerKey ==
    (sourceId == "camera#elif_in" ? "elif_follow.footage.entryAnswer" : "elif_follow.footage.exitAnswer"),
    "CCTV yanıt çeşidi kaydedilmedi: " + sourceId);
  }
 }

 // Dosya #001'in iki bağımsız rotası: Hasan takibi ya Elif'in ya Mert'in
 // ifadesiyle açılmalı — biri kaldırıldığında diğeri hâlâ yetmeli.
 static void Routes(CaseData data, string snapshot, ValidationReport report) {
  var fromElif = JsonUtility.FromJson<Progress>(snapshot);
  fromElif.asked.Remove("mert_follow.spare"); fromElif.read.Remove("mert_follow");
  report.Require(new Investigation(data, fromElif).Discovered(data.nodes.First(n => n.id == "hasan_follow")),
   "Elif'in ifadesi Hasan takibini açmadı.");
  var fromMert = JsonUtility.FromJson<Progress>(snapshot);
  fromMert.asked.Remove("elif_follow.footage"); fromMert.read.Remove("elif_follow");
  report.Require(new Investigation(data, fromMert).Discovered(data.nodes.First(n => n.id == "hasan_follow")),
   "Mert'in ifadesi Hasan takibini açmadı.");
 }

 static void Reporting(CaseData data, string snapshot, ValidationReport report) {
  var correctSuspect = data.verdicts.First(v => v.correct).id;

  var wrongGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  var wrongSuspect = data.verdicts.First(v => !v.correct).id;
  if (!report.Step(wrongGame.SubmitFinalReport(wrongSuspect, "spare", "recovery", "recovery", "mert_follow", "recovery") &&
   wrongGame.State.closed, "Yanlış rapor vakayı kapatmadı.")) return;
  report.Require(wrongGame.Career.departmentTrust == 60 && wrongGame.Career.pendingReviews.Count == 1,
   "Değerlendirme çok erken uygulandı.");
  wrongGame.BeginNextCaseReview(7);
  report.Require(wrongGame.DeliverNextFax() == null, "Faks gecikmeden önce geldi.");
  wrongGame.Career.pendingReviews[0].readyAtUtcTicks = DateTime.UtcNow.AddTicks(-1).Ticks;
  var wrongFax = wrongGame.DeliverNextFax();
  if (!report.Step(wrongFax != null, "Başarısız rapor faksı gelmedi.")) return;
  report.Require(!wrongFax.correct && !wrongFax.suspectSupported && wrongFax.methodSupported && wrongFax.proofSupported &&
   wrongGame.Career.departmentTrust == 60 - data.failedReportTrustLoss, "Başarısız faks etkisi geçersiz.");
  report.Require(wrongGame.DeliverNextFax() == null && wrongGame.Career.reviewHistory.Count == 1, "Yinelenen faks değerlendirmesi.");
  var restoredCareer = JsonUtility.FromJson<CareerProgress>(JsonUtility.ToJson(wrongGame.Career));
  var restoredGame = new Investigation(data, wrongGame.State, restoredCareer);
  report.Require(restoredGame.Career.reviewHistory.Count == 1 && restoredGame.DeliverNextFax() == null &&
   restoredGame.Career.departmentTrust == wrongGame.Career.departmentTrust, "Kariyer kaydı faksı yeniden oynattı.");

  var rightGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  report.Forbid(rightGame.SubmitFinalReport(correctSuspect, "spare", "recovery", "missing", "mert_follow", "recovery"),
   "Görülmemiş rapor kaynağı kabul edildi.");
  report.Require(rightGame.SubmitFinalReport(correctSuspect, "spare", "recovery", "recovery", "mert_follow", "recovery") &&
   rightGame.State.closed, "Doğru rapor vakayı kapatmadı.");
  rightGame.BeginNextCaseReview(7);
  rightGame.Career.pendingReviews[0].readyAtUtcTicks = DateTime.UtcNow.AddTicks(-1).Ticks;
  var rightFax = rightGame.DeliverNextFax();
  if (report.Step(rightFax != null, "Başarılı rapor faksı gelmedi.")) {
   report.Require(rightFax.correct && rightFax.suspectSupported && rightFax.methodSupported && rightFax.proofSupported &&
    rightGame.Career.departmentTrust == 60 + data.successfulReportTrustGain, "Başarılı faks geçersiz.");
   report.Require(rightFax.suspectSourceId == "recovery" && rightFax.methodSourceId == "mert_follow" &&
    rightFax.proofSourceId == "recovery", "Rapor kaynak zinciri fakssa kaydedilmedi.");
  }

  var unlinkedGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  report.Require(unlinkedGame.SubmitFinalReport(correctSuspect, "spare", "recovery", "report", "mert_follow", "recovery") &&
   unlinkedGame.Career.pendingReviews[0].evaluationType != "supported",
   "Desteklenmeyen kaynak zinciri desteklenmiş sayıldı.");

  var cameraGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  report.Require(!cameraGame.ReportSourceAvailable("camera") && cameraGame.ReportSourceAvailable("camera#gap") &&
   !cameraGame.ReportSourceAvailable("camera#unknown"), "CCTV olay kaynağı doğrulaması başarısız.");
  report.Require(cameraGame.SubmitFinalReport(correctSuspect, "spare", "recovery", "recovery", "camera#gap", "recovery") &&
   cameraGame.State.reportMethodSource == "camera#gap", "Seçilen CCTV satırı raporda kaydedilmedi.");

  var partialGame = new Investigation(data, JsonUtility.FromJson<Progress>(snapshot));
  var wrongMethod = data.methods.First(v => !v.correct).id;
  if (!report.Step(partialGame.SubmitFinalReport(correctSuspect, wrongMethod, "recovery", "recovery", "mert_follow", "recovery"),
   "Kısmi rapor reddedildi.")) return;
  partialGame.BeginNextCaseReview(7);
  partialGame.Career.pendingReviews[0].readyAtUtcTicks = DateTime.UtcNow.AddTicks(-1).Ticks;
  var partialFax = partialGame.DeliverNextFax();
  report.Require(partialFax != null && partialFax.evaluationType == "incomplete" && partialFax.trustChange < 0 &&
   wrongFax != null && partialFax.trustChange > wrongFax.trustChange, "Kısmi rapor ağırlığı geçersiz.");
 }
}
}
