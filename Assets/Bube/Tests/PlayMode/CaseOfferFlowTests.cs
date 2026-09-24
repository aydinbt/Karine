using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Vaka teklifi artık ayrı bir tam ekran değil: dosya masadaki gelen evrak
// tepsisinde durur, rozet yanıp söner, oyuncu tepsiyi kendisi açar ve önizlemeyi
// okuyup kabul eder. Bu test o akışı gerçek arayüz üzerinde koşturur.
//
// Yansıma kullanılıyor çünkü `BubeApp` tek parça ve ekran yöntemleri özel.
// Faz 4'te sınıf bölündüğünde bu testin de sadeleşmesi beklenir.
public sealed class CaseOfferFlowTests {

 BubeApp app;
 object game;

 static object Field(object target, string name) =>
  target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
   .GetValue(target);

 // `InboxPage`'in iki aşırı yüklemesi var; parametresiz olanı seçiyoruz.
 static void Call(object target, string name) =>
  target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
   .First(m => m.Name == name && m.GetParameters().Length == 0)
   .Invoke(target, null);

 VisualElement Root => (VisualElement)Field(app, "root");

 static string[] Tooltips(VisualElement root) =>
  root.Query<Button>().ToList().Select(b => b.tooltip).Where(t => !string.IsNullOrEmpty(t)).ToArray();

 static Button ButtonWithText(VisualElement root, string text) =>
  root.Query<Button>().ToList().FirstOrDefault(b => b.text != null && b.text.Contains(text));

 [UnitySetUp] public IEnumerator Setup() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  yield return null;
  for (int frame = 0; frame < 20; frame++) yield return null;
  app = Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
  game = Field(app, "game");
  Assert.IsNotNull(game, "Investigation kurulmadı.");
 }

 Progress State => (Progress)game.GetType().GetProperty("State").GetValue(game);

 [UnityTest] public IEnumerator UnacceptedCase_LeavesOnlyTheInboxOpenOnTheDesk() {
  State.caseAccepted = false;
  Call(app, "Desk");
  yield return null;

  var tooltips = Tooltips(Root);
  var inbox = (string)app.GetType()
   .GetMethod("T", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(app, new object[] { "desk.inbox" });
  Assert.Contains(inbox, tooltips, "Gelen evrak tepsisi açık olmalı.");
  // Dosya, görüşme ve terminal kabul edilmeden erişilemez: oyuncu önce evrağı fark eder.
  Assert.AreEqual(2, tooltips.Length,
   "Kabul edilmeden yalnız tepsi ve ana ekran erişilebilir olmalı, bulunan: " + string.Join(" | ", tooltips));
 }

 [UnityTest] public IEnumerator Inbox_ShowsThePreviewAndAcceptingOpensTheDesk() {
  State.caseAccepted = false;
  Call(app, "InboxPage");
  yield return null;

  var accept = ButtonWithText(Root, "Dosyayı kabul et");
  Assert.IsNotNull(accept, "Tepside kabul düğmesi yok.");
  Assert.IsTrue(Root.Query<Label>().ToList().Any(l => l.text != null && l.text.Contains("KONUT HIRSIZLIĞI")),
   "Dosya önizlemesi tepside okunmuyor.");

  using (var click = new NavigationSubmitEvent()) { click.target = accept; accept.SendEvent(click); }
  yield return null;
  yield return null;

  Assert.IsTrue(State.caseAccepted, "Kabul düğmesi dosyayı kabul etmedi.");
  var tooltips = Tooltips(Root);
  Assert.Greater(tooltips.Length, 2, "Kabulden sonra masa tamamen açılmalı: " + string.Join(" | ", tooltips));
 }

 // Kaldırılan tam ekran teklifin geri gelmediğini sabitler.
 [Test] public void CaseOfferScreen_NoLongerExists() {
  Assert.IsNull(typeof(BubeApp).GetMethod("CaseOffer", BindingFlags.Instance | BindingFlags.NonPublic),
   "CaseOffer() geri gelmiş; teklif masadaki tepside durmalı.");
 }
}
}
