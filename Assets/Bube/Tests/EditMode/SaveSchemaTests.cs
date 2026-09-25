using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Kayıt şeması gidiş-dönüşü ve göç. Eski sürüm yükseltilir, yeni sürüm
// devralınmaz ama silinmez; her iki sonuç da `SaveOutcome` ile adlandırılır.
public sealed class SaveSchemaTests {

 static CaseData Case001() => JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/case001").text);

 static Progress Roundtrip(Progress source) => JsonUtility.FromJson<Progress>(JsonUtility.ToJson(source));

 [Test]
 public void Progress_SurvivesRoundTripWithEveryList() {
  var source = new Progress { caseId = "case001", caseAccepted = true, seenInterviewTurns = 3,
   reportSuspect = "mert", reportMethodSource = "camera#gap", submittedAtUtcTicks = 12345 };
  source.read.Add("report"); source.asked.Add("mert.day");
  source.timelinePinned.Add("t1");
  source.interviewRequests.Add(new InterviewRequest { nodeId = "mert", readyAtUtcTicks = 7 });
  source.documentRequests.Add(new DocumentRequest { nodeId = "recovery", readyAtUtcTicks = 9 });
  source.interviewTurns.Add(new InterviewTurn { nodeId = "mert", questionId = "mert.day",
   answerKey = "mert.day.answer", sourceId = "camera#gap" });

  var restored = Roundtrip(source);
  Assert.AreEqual(1, restored.version);
  Assert.AreEqual(source.caseId, restored.caseId);
  Assert.IsTrue(restored.caseAccepted);
  Assert.AreEqual(source.read, restored.read);
  Assert.AreEqual(source.asked, restored.asked);
  Assert.AreEqual(source.timelinePinned, restored.timelinePinned);
  Assert.AreEqual(7, restored.interviewRequests[0].readyAtUtcTicks);
  Assert.AreEqual(9, restored.documentRequests[0].readyAtUtcTicks);
  Assert.AreEqual("camera#gap", restored.interviewTurns[0].sourceId);
  Assert.AreEqual("camera#gap", restored.reportMethodSource);
  Assert.AreEqual(12345, restored.submittedAtUtcTicks);
 }

 [Test]
 public void CareerProgress_SurvivesRoundTripIncludingHistory() {
  var source = new CareerProgress { departmentTrust = 45, activeCaseId = "case001", faxReleased = true,
   careerRankId = "inspector", retired = false };
  source.seenWorldIntros.Add("world01");
  source.pendingReviews.Add(new PendingReview { caseId = "case001", evaluationType = "incomplete",
   trustDelta = -5, readyAtUtcTicks = 11, suspectSupported = true });
  source.reviewHistory.Add(new FaxReview { caseId = "case001", correct = true, trustChange = 5, trustAfter = 50 });
  source.lastFax = source.reviewHistory[0];

  var restored = JsonUtility.FromJson<CareerProgress>(JsonUtility.ToJson(source));
  Assert.AreEqual(1, restored.version);
  Assert.AreEqual(45, restored.departmentTrust);
  Assert.AreEqual("inspector", restored.careerRankId);
  Assert.IsTrue(restored.faxReleased);
  Assert.AreEqual(new[] { "world01" }, restored.seenWorldIntros.ToArray());
  Assert.AreEqual("incomplete", restored.pendingReviews[0].evaluationType);
  Assert.IsTrue(restored.pendingReviews[0].suspectSupported);
  Assert.AreEqual(1, restored.reviewHistory.Count);
  Assert.AreEqual(50, restored.lastFax.trustAfter);
 }

 [Test]
 public void MissingSave_StartsFresh() {
  var game = new Investigation(Case001(), null);
  Assert.AreEqual("case001", game.State.caseId);
  Assert.IsFalse(game.State.caseAccepted);
  Assert.IsEmpty(game.State.read);
  Assert.AreEqual(60, game.Career.departmentTrust);
 }

 [Test]
 public void EmptyJsonSave_LoadsWithUsableLists() {
  var blank = JsonUtility.FromJson<Progress>("{}");
  Assert.IsNotNull(blank.read, "JsonUtility boş JSON'da alan başlatıcılarını korur.");
  var game = new Investigation(Case001(), blank);
  // caseId boş olduğu için bugünkü kod kaydı atıp sıfırdan başlıyor.
  Assert.AreEqual("case001", game.State.caseId);
  Assert.IsFalse(game.State.caseAccepted);
 }

 [Test]
 public void SaveForAnotherCase_IsNotAdopted() {
  var foreignSave = new Progress { caseId = "case002", caseAccepted = true };
  foreignSave.read.Add("report");
  var game = new Investigation(Case001(), foreignSave);
  Assert.AreEqual("case001", game.State.caseId);
  Assert.IsEmpty(game.State.read, "Başka vakanın kaydı devralınmamalı.");
 }

 // Göç: sürüm alanı hiç yazılmamış (0) eski kayıt atılmaz, bugünkü şemaya
 // yükseltilir ve ilerleme korunur.
 [Test]
 public void OlderVersionSave_IsMigratedNotDiscarded() {
  var old = new Progress { version = 0, caseId = "case001", caseAccepted = true };
  old.read.Add("report");
  var game = new Investigation(Case001(), old);
  Assert.AreEqual(SaveOutcome.Migrated, game.StateOutcome);
  Assert.AreEqual(SaveMigration.ProgressVersion, game.State.version);
  Assert.IsTrue(game.State.caseAccepted);
  Assert.AreEqual(new[] { "report" }, game.State.read.ToArray());
 }

 [Test]
 public void OlderCareerSave_KeepsTrustAndRank() {
  var old = new CareerProgress { version = 0, departmentTrust = 42, careerRankId = "inspector" };
  var game = new Investigation(Case001(), null, old);
  Assert.AreEqual(SaveOutcome.Migrated, game.CareerOutcome);
  Assert.AreEqual(SaveMigration.CareerVersion, game.Career.version);
  Assert.AreEqual(42, game.Career.departmentTrust, "Göç güveni sıfırlamamalı.");
  Assert.AreEqual("inspector", game.Career.careerRankId);
 }

 // Gelecekten gelen kayıt çevrilemez; devralınmaz ama bu bir veri kaybı olarak
 // görünür olmalı. `BubeApp` bu sonucu görünce dosyayı silmeyip yana kaldırır.
 [Test]
 public void NewerVersionSave_IsReportedAsFromFuture() {
  var future = new Progress { version = SaveMigration.ProgressVersion + 1, caseId = "case001", caseAccepted = true };
  var game = new Investigation(Case001(), future);
  Assert.AreEqual(SaveOutcome.FromFuture, game.StateOutcome);
  Assert.IsFalse(game.State.caseAccepted);
  Assert.AreEqual(SaveOutcome.FromFuture,
   new Investigation(Case001(), null, new CareerProgress { version = SaveMigration.CareerVersion + 1, departmentTrust = 9 }).CareerOutcome);
 }

 [Test]
 public void LoadOutcomes_NameWhyTheSaveWasNotAdopted() {
  Assert.AreEqual(SaveOutcome.Fresh, new Investigation(Case001(), null).StateOutcome);
  Assert.AreEqual(SaveOutcome.Loaded,
   new Investigation(Case001(), new Progress { caseId = "case001" }).StateOutcome);
  Assert.AreEqual(SaveOutcome.OtherCase,
   new Investigation(Case001(), new Progress { caseId = "case002" }).StateOutcome);
 }
}
}