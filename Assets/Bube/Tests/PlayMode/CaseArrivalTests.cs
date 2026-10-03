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

 // Bu testler kayit yazan yollari (`OpenAssignment` → `Save`) calistiriyor.
 // Birakilan kayit sonraki testin acilis durumunu degistirir; oyuncunun kendi
 // kaydi `SaveSandbox` tarafindan korunur.
 static void ClearSaves() {
  foreach (var file in System.IO.Directory.GetFiles(Application.persistentDataPath, "bube-*"))
   try { System.IO.File.Delete(file); } catch (Exception) { }
 }

 [UnityTearDown] public IEnumerator Teardown() { ClearSaves(); yield return null; }

 [UnitySetUp] public IEnumerator Setup() {
  ClearSaves();
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  yield return null;
  for (int frame = 0; frame < 20; frame++) yield return null;
  app = UnityEngine.Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
  SaveSandbox.ResetToInitialCase(app);
 }

 [UnityTest] public IEnumerator NewCase_IsDroppedOnTheDesk() {
  yield return Arrive();
  var labels = Labels(Root);
  // Şerit kariyerin ilk dosyasında farklı yazar; ikisi de bırakılışın oynadığını gösterir.
  Assert.IsTrue(labels.Contains(Text("intro.firstFile")) || labels.Contains(Text("intro.newFile")),
   "Dosya bırakılış animasyonu oynamadı, bulunan: " + string.Join(" | ", labels));
 }

 // Gercek bolum gecisi: kapali bir vakadan sonraki dosyanin acilmasi. Yukaridaki
 // test yalnizca `MaybeWorldIntro`u dogrudan cagiriyordu; burasi oyuncunun
 // gelen evrak tepsisinden izledigi yolun ta kendisi.
 [UnityTest] public IEnumerator NextAssignment_DropsTheFile() {
  var game = Field(app, "game");
  var state = (Progress)game.GetType().GetProperty("State").GetValue(game);
  var career = (CareerProgress)game.GetType().GetProperty("Career").GetValue(game);
  var config = (GameConfig)Field(app, "config");
  foreach (var world in config.worldIntros ?? new WorldIntro[0])
   if (!career.seenWorldIntros.Contains(world.id)) career.seenWorldIntros.Add(world.id);
  state.caseAccepted = true;
  state.closed = true;
  var data = (CaseData)game.GetType().GetProperty("Data").GetValue(game);
  Assert.IsNotNull(data, "Vaka verisi yok.");
  Assert.IsFalse(string.IsNullOrEmpty(data.nextCaseId), "Sıradaki vaka tanımlı değil.");
  var asset = Resources.Load<TextAsset>("Bube/Cases/" + data.nextCaseId);
  Assert.IsNotNull(asset, "Sıradaki vaka dosyası yüklenemedi: " + data.nextCaseId);
  var next = JsonUtility.FromJson<CaseData>(asset.text);
  app.GetType().GetMethod("OpenAssignment", BindingFlags.Instance | BindingFlags.NonPublic)
   .Invoke(app, new object[] { next });
  for (int frame = 0; frame < 30; frame++) yield return null;
  var labels = Labels(Root);
  Assert.IsTrue(labels.Contains(Text("intro.firstFile")) || labels.Contains(Text("intro.newFile")),
   "Sonraki dosya bırakılmadan geldi, bulunan: " + string.Join(" | ", labels));
 }

 // Oyuncunun gercekten bastigi dugme: tepsideki "Dosyayi ac". Yukaridaki test
 // `OpenAssignment`i dogrudan cagiriyor; burasi tikllamayi taklit eder, cunku
 // bildirilen hata tam bu dugmeden sonra animasyonun gelmemesiydi.
 [UnityTest] public IEnumerator InboxOpenButton_DropsTheFile() {
  var game = Field(app, "game");
  var state = (Progress)game.GetType().GetProperty("State").GetValue(game);
  var career = (CareerProgress)game.GetType().GetProperty("Career").GetValue(game);
  var config = (GameConfig)Field(app, "config");
  foreach (var world in config.worldIntros ?? new WorldIntro[0])
   if (!career.seenWorldIntros.Contains(world.id)) career.seenWorldIntros.Add(world.id);
  state.caseAccepted = true;
  state.closed = true;
  var data = (CaseData)game.GetType().GetProperty("Data").GetValue(game);
  app.GetType().GetMethod("InboxPage", BindingFlags.Instance | BindingFlags.NonPublic,
   null, new[] { typeof(string), typeof(string) }, null)
   .Invoke(app, new object[] { "assignment:" + data.nextCaseId, "all" });
  yield return null;
  var open = Root.Query<Button>("InboxAction").ToList().FirstOrDefault(b => b.Query<Label>().ToList().Any(l => l.text == Text("next.assignment.open").ToUpper(new System.Globalization.CultureInfo("tr-TR"))));
  Assert.IsNotNull(open, "Tepside \"Dosyayı aç\" düğmesi yok.");
  using (var click = new NavigationSubmitEvent()) { click.target = open; open.SendEvent(click); }
  for (int frame = 0; frame < 30; frame++) yield return null;
  var labels = Labels(Root);
  Assert.IsTrue(labels.Contains(Text("intro.firstFile")) || labels.Contains(Text("intro.newFile")),
   "Tepsiden açılan dosya bırakılmadan geldi, bulunan: " + string.Join(" | ", labels));
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
