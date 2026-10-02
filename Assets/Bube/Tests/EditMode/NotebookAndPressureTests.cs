using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Oyuncunun defteri, altı çizilen satırlar, faks akıbeti ve kapanan görüşme.
// Hiçbiri oyuncuya doğruyu söylemez: defter hükmü onaylamaz, akıbet yalnız
// faksla gelir, kapanan görüşme doğru sonucun yolunu kesemez.
public sealed class NotebookAndPressureTests {

 static CaseData Load(string id) => JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/" + id).text);

 static Investigation With(string caseId, params string[] read) {
  var progress = new Progress { caseId = caseId, caseAccepted = true };
  progress.read.AddRange(read);
  return new Investigation(Load(caseId), progress);
 }

 [Test]
 public void Notebook_OnlyTakesOpenedSources_AndNeverJudges() {
  var game = With("case001", "report");
  Assert.IsFalse(game.MarkNote("report", "camera", "conflict"), "Açılmamış kaynak deftere girmemeli.");
  game.State.read.Add("camera");
  Assert.IsTrue(game.MarkNote("report", "camera", "conflict"));
  Assert.AreEqual(1, game.State.notebook.Count);
  Assert.IsTrue(game.MarkNote("camera", "report", "agree"), "Aynı çift ters sırayla da aynı nottur.");
  Assert.AreEqual(1, game.State.notebook.Count, "Çift için ikinci not açılmamalı.");
  Assert.AreEqual("agree", game.State.notebook[0].mark);
  Assert.IsTrue(game.MarkNote("report", "camera", "agree"), "Aynı hüküm yeniden seçilince not kalkar.");
  Assert.AreEqual(0, game.State.notebook.Count);
  Assert.IsFalse(game.MarkNote("report", "report", "conflict"), "Kaynak kendisiyle eşlenmez.");
  Assert.IsFalse(game.MarkNote("report", "camera", "guilty"), "Tanımsız hüküm yazılmaz.");
 }

 [Test]
 public void Notebook_SurvivesSave_AndDropsUnknownSources() {
  var game = With("case001", "report", "camera");
  Assert.IsTrue(game.MarkNote("report", "camera", "question"));
  Assert.IsTrue(game.ToggleHighlight("report", 0));
  var json = JsonUtility.ToJson(game.State);
  var restored = new Investigation(Load("case001"), JsonUtility.FromJson<Progress>(json));
  Assert.AreEqual(1, restored.State.notebook.Count);
  Assert.Contains("report:0", restored.State.highlights);

  var broken = JsonUtility.FromJson<Progress>(json);
  broken.notebook.Add(new NotebookEntry { leftId = "nope", rightId = "report", mark = "conflict" });
  broken.highlights.Add("nope:3");
  var cleaned = new Investigation(Load("case001"), broken);
  Assert.AreEqual(1, cleaned.State.notebook.Count, "Bilinmeyen kaynaklı not atılmalı.");
  Assert.IsFalse(cleaned.State.highlights.Contains("nope:3"));
 }

 [Test]
 public void Highlight_NeedsReadDocument_AndToggles() {
  var game = With("case001", "report");
  Assert.IsFalse(game.ToggleHighlight("camera", 0), "Okunmamış kaynağın altı çizilmez.");
  Assert.IsFalse(game.ToggleHighlight("mert", 0), "Görüşmenin metni cümle cümle işaretlenmez.");
  Assert.IsTrue(game.ToggleHighlight("report", 2));
  Assert.Contains("report:2", game.State.highlights);
  Assert.IsTrue(game.ToggleHighlight("report", 2));
  Assert.IsFalse(game.State.highlights.Contains("report:2"));
  game.State.closed = true;
  Assert.IsFalse(game.ToggleHighlight("report", 1), "Kapanan dosyada işaretleme yapılmaz.");
 }

 [Test]
 public void Sentences_KeepClockTimesWhole() {
  var parts = Investigation.Sentences("Saat 23.35'te çıktı. Kapı açıktı!\nYeni satır");
  CollectionAssert.AreEqual(new[] { "Saat 23.35'te çıktı.", "Kapı açıktı!", "Yeni satır" }, parts);
  Assert.AreEqual(0, Investigation.Sentences("").Length);
 }

 [Test]
 public void ClosedInterview_CannotBeRequested_ButEarlierRequestStands() {
  var data = Load("case003");
  var node = data.nodes.First(n => n.id == "baris_follow");
  CollectionAssert.IsNotEmpty(node.closesAfterRead, "Dosya #003'te kapanan görüşme tanımlı olmalı.");
  var everything = data.nodes.Where(n => n.kind != "interview").Select(n => n.id)
   .Concat(new[] { "ozan", "baris", "seda", "ozan_follow", "ozan_third" }).ToArray();
  var late = With("case003", everything);
  Assert.IsTrue(late.Closed(node), "Kapanış kaynakları okununca görüşme kapanmalı.");
  Assert.IsFalse(late.CanRequest(node));
  Assert.IsFalse(late.RequestInterview(node.id, 0));

  var early = With("case003", everything.Where(id => id != "ozan_third").ToArray());
  Assert.IsTrue(early.RequestInterview(node.id, 0), "Kapanmadan önce istenebilmeli.");
  early.State.read.Add("ozan_third");
  Assert.IsFalse(early.Closed(node), "Önceden istenen görüşme sonradan kapanmaz.");
 }

 [Test]
 public void EveryCaseWithEpilogues_CoversEveryChoice() {
  foreach (var id in new[] { "case001", "case002", "case003" }) {
   var data = Load(id);
   Assert.IsTrue(data.verdicts.All(v => !string.IsNullOrEmpty(v.epilogueKey)), id + ": her şüphelinin akıbeti olmalı.");
   Assert.IsTrue((data.custody ?? new Choice[0]).All(c => !string.IsNullOrEmpty(c.epilogueKey)), id + ": her ikinci sorumluluğun akıbeti olmalı.");
  }
 }
}
}
