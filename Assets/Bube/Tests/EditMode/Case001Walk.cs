using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Dosya #001'in ileri rotası. Hem akış testleri hem ödüllü yeniden deneme
// testleri aynı yürüyüşü kullanır; iki kopya tutulmaz.
public static class Case001Walk {

 public static CaseData Case001() {
  var asset = Resources.Load<TextAsset>("Bube/Cases/case001");
  Assert.IsNotNull(asset, "case001.json bulunamadı.");
  return JsonUtility.FromJson<CaseData>(asset.text);
 }

 public static Investigation Fresh() {
  var game = new Investigation(Case001());
  Assert.IsTrue(game.AcceptCase(), "Dosya kabul edilemedi.");
  return game;
 }

 public static Node Node(Investigation game, string id) {
  foreach (var n in game.Data.nodes) if (n.id == id) return n;
  Assert.Fail("Düğüm yok: " + id);
  return null;
 }

 public static bool Discovered(Investigation game, string id) => game.Discovered(Node(game, id));

 // Görüşmeye soru sormak için önce randevu gerekir (Available(n) bunu şart koşar).
 public static void OpenInterview(Investigation game, string id) {
  Assert.IsTrue(game.RequestInterview(id, 0), "Görüşme talep edilemedi: " + id);
  Assert.IsTrue(game.Available(Node(game, id)), "Görüşme açılmadı: " + id);
 }

 public static void Ask(Investigation game, string node, string question, string source = null) =>
  Assert.IsTrue(game.Ask(node, question, source), "Soru sorulamadı: " + node + "/" + question);

 // Betiğin 2. bölümündeki ileri rota, adım adım.
 public static Investigation WalkToReportReady() {
  var game = Fresh();
  Assert.IsTrue(game.Read("report"), "Dosya okunamadı.");

  OpenInterview(game, "mert");
  Ask(game, "mert", "mert.day");
  Ask(game, "mert", "mert.key");
  Ask(game, "mert", "mert.neighbor");
  Assert.Contains("mert", game.State.read, "mert görüşmesi tamamlanınca okundu sayılmalı.");

  OpenInterview(game, "elif");
  Ask(game, "elif", "elif.relationship");
  Ask(game, "elif", "elif.visit");

  OpenInterview(game, "hasan");
  Ask(game, "hasan", "hasan.sighting");
  Ask(game, "hasan", "hasan.camera");
  Ask(game, "hasan", "hasan.where");

  Assert.IsTrue(game.Read("camera"), "CCTV okunamadı.");

  OpenInterview(game, "elif_follow");
  Ask(game, "elif_follow", "elif_follow.footage", "camera#elif_in");
  Ask(game, "elif_follow", "elif_follow.why");

  OpenInterview(game, "mert_follow");
  Ask(game, "mert_follow", "mert_follow.spare");

  OpenInterview(game, "hasan_follow");
  Ask(game, "hasan_follow", "hasan_follow.spare");
  Ask(game, "hasan_follow", "hasan_follow.gap", "camera#gap");

  Assert.IsTrue(game.RequestDocument("recovery", 0), "Eşya raporu talep edilemedi.");
  Assert.IsTrue(game.ReceiveDocument("recovery"), "Eşya raporu teslim alınamadı.");
  return game;
 }
}
}
