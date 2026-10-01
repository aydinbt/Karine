using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {

// İkinci kademe efektleri: evrak basımı, kaydın karşıdakine kayması, kamera
// değişiminde sinyal bozulması ve sayfa çevirme. Hepsi bitince iz bırakmaz.
public sealed class EffectsTests {

 BubeApp app;
 int reducedBefore;
 const BindingFlags Any = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

 VisualElement Root => (VisualElement)typeof(BubeApp).GetField("root", Any).GetValue(app);
 object Call(string name, params object[] args) => typeof(BubeApp).GetMethod(name, Any).Invoke(app, args);

 static IEnumerator Wait(float seconds) {
  float until = Time.realtimeSinceStartup + seconds;
  while (Time.realtimeSinceStartup < until) yield return null;
 }

 [UnitySetUp] public IEnumerator Setup() {
  reducedBefore = PlayerPrefs.GetInt("karine.reducedMotion", 0);
  PlayerPrefs.SetInt("karine.reducedMotion", 0);
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  // Grafiksiz koşuda açılış videosu hazırlanamaz ve akış gecikmeli olarak masayı
  // kendisi kurar; o gelmeden konan test öğeleri silinir.
  yield return Wait(2f);
  app = UnityEngine.Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
  SaveSandbox.ResetToInitialCase(app);
  typeof(BubeApp).GetField("instantText", Any).SetValue(app, false);
  // Açılış akışı ekranı kendi kurar; test öğeleri onun bittiği masanın üstüne konur.
  var game = typeof(BubeApp).GetField("game", Any).GetValue(app);
  ((Progress)game.GetType().GetProperty("State").GetValue(game)).caseAccepted = true;
  Call("Desk");
  for (int frame = 0; frame < 10; frame++) yield return null;
 }

 [UnityTearDown] public IEnumerator Teardown() {
  PlayerPrefs.SetInt("karine.reducedMotion", reducedBefore);
  yield return null;
 }

 [UnityTest] public IEnumerator PrintOut_KeepsLayoutAndFinishesOnTap() {
  var body = new VisualElement();
  body.style.width = 400; Root.Add(body);
  var line = new Label("Değerlendirme: rapor dosyaya işlendi.");
  body.Add(line);
  var button = new Button { text = "Tamam" }; body.Add(button);
  yield return null;
  float height = line.layout.height;
  Call("PrintOut", body, "test:fax");
  Assert.AreEqual(Visibility.Hidden, button.resolvedStyle.visibility, "Basılmamış düğme dokunulabilir duruyor.");
  StringAssert.Contains("<alpha=#00>", line.text, "Basılmamış kısım yerinde tutulmuyor.");
  yield return null;
  Assert.AreEqual(height, line.layout.height, .5f, "Sayfa basılırken satır boyu değişti.");
  using (var tap = PointerDownEvent.GetPooled()) { tap.target = body; body.SendEvent(tap); }
  yield return null;
  Assert.AreEqual("Değerlendirme: rapor dosyaya işlendi.", line.text, "Dokununca basım bitmedi.");
  Assert.AreEqual(Visibility.Visible, button.resolvedStyle.visibility, "Dokununca düğme açılmadı.");
  // Aynı kâğıt ikinci kez basılmaz.
  Call("PrintOut", body, "test:fax");
  Assert.AreEqual("Değerlendirme: rapor dosyaya işlendi.", line.text, "Aynı kâğıt yeniden basıldı.");
 }

 [UnityTest] public IEnumerator PrintOut_RunsToTheEndByItself() {
  var body = new VisualElement(); Root.Add(body);
  var line = new Label("Kısa satır."); body.Add(line);
  Call("PrintOut", body, "test:document");
  yield return Wait(.5f);
  Assert.AreEqual("Kısa satır.", line.text, "Basım kendiliğinden bitmedi.");
 }

 [UnityTest] public IEnumerator PresentedRecord_SlidesToThePerson() {
  var button = new Button { text = "Kaydı öne sür" };
  button.style.position = Position.Absolute; button.style.left = 600; button.style.top = 300; button.style.width = 260;
  Root.Add(button);
  yield return null; yield return null;
  bool sent = false;
  Call("SlideToPerson", button, "Kamera · 22.58", (Action)(() => sent = true));
  Assert.IsNotNull(Root.Q("PresentedPaper"), "Öne sürülen kâğıt görünmedi.");
  Assert.IsNotNull(Root.Q("PresentBlocker"), "Kâğıt kayarken ekran dokunmaya açık.");
  Assert.IsFalse(sent, "Kâğıt kaymadan yanıt geldi.");
  var paper = Root.Q("PresentedPaper");
  yield return Wait(.6f);
  Assert.IsTrue(sent, "Kâğıt kaydıktan sonra yanıt gelmedi. panel=" + (paper.panel != null) +
   " opacity=" + paper.resolvedStyle.opacity + " left=" + paper.resolvedStyle.left + " rootChildren=" + Root.childCount + " first=" + (Root.childCount > 0 ? Root[0].name + "/" + Root[0].childCount : "-") +
   " scene=" + SceneManager.GetActiveScene().name);
  Assert.IsNull(Root.Q("PresentedPaper"), "Kâğıt ekranda kaldı.");
  Assert.IsNull(Root.Q("PresentBlocker"), "Kayma bitti ama ekran dokunmaya kapalı kaldı.");
 }

 [UnityTest] public IEnumerator SignalSwitch_AndPageTurn_LeaveNoTrace() {
  var frame = new VisualElement(); frame.style.width = 300; frame.style.height = 200; Root.Add(frame);
  KarineUI.SignalSwitch(frame);
  KarineUI.PageTurn(frame);
  Assert.IsNotNull(frame.Q("SignalSwitch"), "Sinyal bozulması oynamadı.");
  Assert.IsNotNull(frame.Q("PageTurn"), "Sayfa çevirme oynamadı.");
  Assert.AreEqual(PickingMode.Ignore, frame.Q("SignalSwitch").pickingMode, "Bozulma dokunuşu yutuyor.");
  yield return Wait(.5f);
  Assert.IsNull(frame.Q("SignalSwitch"), "Sinyal bozulması ekranda kaldı.");
  Assert.IsNull(frame.Q("PageTurn"), "Sayfa kenarı ekranda kaldı.");
 }

 // Nefes herkeste aynı genlikte: hiçbir kişi daha hızlı ya da derin nefes almaz.
 [UnityTest] public IEnumerator Portrait_BreathesWithinTheSameSmallRange() {
  var a = new VisualElement(); var b = new VisualElement();
  Root.Add(a); Root.Add(b);
  KarineUI.Breathe(a); KarineUI.Breathe(b);
  float moved = 0, widest = 0;
  float until = Time.realtimeSinceStartup + 2.5f;
  while (Time.realtimeSinceStartup < until) {
   yield return null;
   foreach (var figure in new[] { a, b }) {
    float y = Mathf.Abs(figure.resolvedStyle.scale.value.y - 1);
    moved = Mathf.Max(moved, y); widest = Mathf.Max(widest, y);
   }
  }
  Assert.Greater(moved, .002f, "Portre nefes almıyor.");
  Assert.LessOrEqual(widest, KarineTheme.Effects.BreathHeight + .0005f, "Nefes sınırı aşıldı; göze batar.");
 }

 [UnityTest] public IEnumerator ReducedMotion_SkipsStraightToTheAnswer() {
  PlayerPrefs.SetInt("karine.reducedMotion", 1);
  var button = new Button(); Root.Add(button);
  yield return null;
  bool sent = false;
  Call("SlideToPerson", button, "x", (Action)(() => sent = true));
  Assert.IsTrue(sent, "Hareketi azalt açıkken yanıt beklemeden gelmeli.");
  var frame = new VisualElement(); Root.Add(frame);
  KarineUI.SignalSwitch(frame);
  Assert.IsNull(frame.Q("SignalSwitch"), "Hareketi azalt açıkken bozulma oynamamalı.");
  KarineUI.Breathe(frame);
  yield return Wait(.5f);
  Assert.AreEqual(1f, frame.resolvedStyle.scale.value.y, .0001f, "Hareketi azalt açıkken portre kıpırdamamalı.");
 }
}
}
