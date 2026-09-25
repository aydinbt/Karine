using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Yeni dosya masaya bırakılır. Dünyanın kendi varış filmi yalnız o dünyanın
// ilk dosyasında oynadığı için, aynı dünyadaki sonraki dosyalar çizilmiş
// bırakılışla gelir; bu test o anın gerçekten oynadığını gösterir.
public sealed class CaseArrivalTests {

 BubeApp app;

 static object Field(object target, string name) =>
  target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
   .GetValue(target);

 string Text(string key) => (string)app.GetType()
  .GetMethod("T", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(app, new object[] { key });

 VisualElement Root => (VisualElement)Field(app, "root");

 static string[] Labels(VisualElement root) =>
  root.Query<Label>().ToList().Select(l => l.text).Where(t => !string.IsNullOrEmpty(t)).ToArray();

 IEnumerator Arrive() {
  var game = Field(app, "game");
  var career = (CareerProgress)game.GetType().GetProperty("Career").GetValue(game);
  var config = (GameConfig)Field(app, "config");
  // Dünya filmi görülmüş sayılır: sıradaki dosyanın geldiği durum bu.
  foreach (var world in config.worldIntros ?? new WorldIntro[0])
   if (!career.seenWorldIntros.Contains(world.id)) career.seenWorldIntros.Add(world.id);
  ((Progress)game.GetType().GetProperty("State").GetValue(game)).caseAccepted = false;
  app.GetType().GetMethod("MaybeWorldIntro", BindingFlags.Instance | BindingFlags.NonPublic)
   .Invoke(app, new object[] { (Action)(() => { }) });
  for (int frame = 0; frame < 30; frame++) yield return null;
 }

 [UnitySetUp] public IEnumerator Setup() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  yield return null;
  for (int frame = 0; frame < 20; frame++) yield return null;
  app = UnityEngine.Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
 }

 [UnityTest] public IEnumerator NewCase_IsDroppedOnTheDesk() {
  yield return Arrive();
  var labels = Labels(Root);
  // Şerit kariyerin ilk dosyasında farklı yazar; ikisi de bırakılışın oynadığını gösterir.
  Assert.IsTrue(labels.Contains(Text("intro.firstFile")) || labels.Contains(Text("intro.newFile")),
   "Dosya bırakılış animasyonu oynamadı, bulunan: " + string.Join(" | ", labels));
 }

 [UnityTest] public IEnumerator AcceptedCase_IsNotDroppedAgain() {
  var game = Field(app, "game");
  ((Progress)game.GetType().GetProperty("State").GetValue(game)).caseAccepted = true;
  var career = (CareerProgress)game.GetType().GetProperty("Career").GetValue(game);
  var config = (GameConfig)Field(app, "config");
  foreach (var world in config.worldIntros ?? new WorldIntro[0])
   if (!career.seenWorldIntros.Contains(world.id)) career.seenWorldIntros.Add(world.id);
  bool continued = false;
  app.GetType().GetMethod("MaybeWorldIntro", BindingFlags.Instance | BindingFlags.NonPublic)
   .Invoke(app, new object[] { (Action)(() => continued = true) });
  yield return null;
  Assert.IsTrue(continued, "Kabul edilmiş vakada akış beklemeden sürmeli.");
  var labels = Labels(Root);
  Assert.IsFalse(labels.Contains(Text("intro.firstFile")) || labels.Contains(Text("intro.newFile")),
   "Kabul edilmiş vakada bırakılış tekrar oynamamalı.");
 }
}
}
