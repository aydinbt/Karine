using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Masanın havası: eşyalar telefonla kayan ön katmanda, kararma düğmelerin
// altında, dokunulan eşya masadan kalkıp bırakınca yerine oturuyor.
public sealed class DeskAtmosphereTests {

 BubeApp app;
 int reducedBefore;

 static object Field(object target, string name) =>
  target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
   .GetValue(target);

 VisualElement Root => (VisualElement)Field(app, "root");

 [UnitySetUp] public IEnumerator Setup() {
  reducedBefore = PlayerPrefs.GetInt("karine.reducedMotion", 0);
  PlayerPrefs.SetInt("karine.reducedMotion", 0);
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  for (int frame = 0; frame < 20; frame++) yield return null;
  app = Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
  SaveSandbox.ResetToInitialCase(app);
  var game = Field(app, "game");
  ((Progress)game.GetType().GetProperty("State").GetValue(game)).caseAccepted = true;
  app.GetType().GetMethod("Desk", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(app, null);
  for (int frame = 0; frame < 10; frame++) yield return null;
 }

 [UnityTearDown] public IEnumerator Teardown() {
  PlayerPrefs.SetInt("karine.reducedMotion", reducedBefore);
  yield return null;
 }

 [UnityTest] public IEnumerator Desk_IsLayered() {
  var front = Root.Q("OfficeFront");
  Assert.IsNotNull(front, "Ön katman kurulmadı.");
  Assert.IsNotNull(Root.Q("OfficeBack")?.Q("OfficeRoom"), "Oda plakası arka katmanda değil.");
  var file = front.Q<Button>("DeskFile");
  Assert.IsNotNull(file, "Dosya düğmesi ön katmanda değil; telefonla kaymaz ve eşyasından ayrılır.");
  Assert.IsNotNull(front.Q("BlankCaseFolder"), "Dosya eşyası ön katmanda değil.");
  Assert.IsNotNull(front.Q("OfficeLight"), "Lamba ışığı yok.");
  Assert.IsNotNull(front.Q("OfficeDust"), "Toz yok.");
  var shade = front.Q("OfficeVignette");
  Assert.IsNotNull(shade, "Kararma yok.");
  Assert.Less(front.IndexOf(shade), front.IndexOf(file), "Kararma düğmelerin üstüne düşmüş; düğme okunmaz.");
  Assert.AreEqual(PickingMode.Ignore, shade.pickingMode, "Kararma dokunuşu yutuyor.");
  yield return null;
 }

 [UnityTest] public IEnumerator PressedProp_LiftsAndSettles() {
  var file = Root.Q<Button>("DeskFile");
  var folder = Root.Q("BlankCaseFolder");
  using (var down = PointerDownEvent.GetPooled()) { down.target = file; file.SendEvent(down); }
  float until = Time.realtimeSinceStartup + .3f;
  while (Time.realtimeSinceStartup < until) yield return null;
  Assert.Less(folder.resolvedStyle.translate.y, -1f, "Basılan dosya masadan kalkmadı.");
  using (var up = PointerUpEvent.GetPooled()) { up.target = file; file.SendEvent(up); }
  until = Time.realtimeSinceStartup + .4f;
  while (Time.realtimeSinceStartup < until) yield return null;
  Assert.AreEqual(0f, folder.resolvedStyle.translate.y, .5f, "Bırakılan dosya yerine oturmadı.");
 }
}
}
