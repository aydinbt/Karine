using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Play Mode duman testi: BubeApp gerçekten açılıyor mu, arayüz kuruluyor mu,
// Update() kare başına hata üretmeden koşuyor mu. Faz 2 perf düzeltmeleri
// Update() ve kök ögenin stil yazımlarına dokunduğu için burası onları da yakalar.
// Görsel doğrulama (video, düzen, glif, dokunma hedefi) bu testin kapsamı dışında.
public sealed class BootSmokeTests {

 readonly List<string> errors = new List<string>();

 void OnLog(string message, string stack, LogType type) {
  if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
   errors.Add(type + ": " + message);
 }

 [SetUp] public void Setup() { errors.Clear(); Application.logMessageReceived += OnLog; }
 [TearDown] public void Teardown() { Application.logMessageReceived -= OnLog; }

 [UnityTest] public IEnumerator BootScene_BuildsUiWithoutErrors() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  yield return null;
  for (int frame = 0; frame < 30; frame++) yield return null;

  var app = Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp sahnede bulunamadı.");
  // Kalıcılığı doğrudan sına: sahne değişince aynı örnek ayakta kalmalı.
  int id = app.GetInstanceID();
  SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
  for (int frame = 0; frame < 10; frame++) yield return null;
  var survivor = Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(survivor, "BubeApp sahne geçişinde yok oldu.");
  Assert.AreEqual(id, survivor.GetInstanceID(),
   "BubeApp sahne başına yeniden kuruluyor — DontDestroyOnLoad beklenir.");
  Assert.AreEqual(1, Object.FindObjectsByType<BubeApp>(FindObjectsSortMode.None).Length,
   "Birden fazla BubeApp örneği oluştu.");
  app = survivor;

  var doc = app.GetComponent<UIDocument>();
  Assert.IsNotNull(doc, "UIDocument kurulmadı.");
  Assert.IsNotNull(doc.rootVisualElement, "Kök görsel öge yok.");
  Assert.Greater(doc.rootVisualElement.childCount, 0, "Arayüz hiç öge üretmedi.");

  CollectionAssert.IsEmpty(errors, "Açılışta hata/özel durum oluştu: " + string.Join(" | ", errors));
 }

 // Faz 2: güvenli alan yazımları artık koşullu. Kök ögenin kenar boşlukları
 // yine de kurulmuş olmalı — koşul yanlışsa burada çıplak kalırlar.
 [UnityTest] public IEnumerator SafeAreaInsets_AreAppliedToRoot() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  for (int frame = 0; frame < 30; frame++) yield return null;

  var doc = Object.FindFirstObjectByType<BubeApp>().GetComponent<UIDocument>();
  var root = doc.rootVisualElement;
  Assert.AreEqual(StyleKeyword.Undefined, root.style.left.keyword,
   "Güvenli alan sol kenarı yazılmamış — koşullu yazım hiç tetiklenmemiş olabilir.");
  Assert.AreEqual(LengthUnit.Percent, root.style.left.value.unit);
  Assert.AreEqual(LengthUnit.Percent, root.style.bottom.value.unit);

  CollectionAssert.IsEmpty(errors, "Hata oluştu: " + string.Join(" | ", errors));
 }
 // Marka gercekten ekrana ciliyor mu: ana menude KARINE logosu duruyor,
 // oran bozulmamis ve iki katman yerinde mi.
 [UnityTest] public IEnumerator MainMenu_ShowsKarineLogoWithIntactAspect() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  for (int frame = 0; frame < 30; frame++) yield return null;

  var root = Object.FindFirstObjectByType<BubeApp>().GetComponent<UIDocument>().rootVisualElement;
  var logo = root.Q("KarineLogo");
  Assert.IsNotNull(logo, "Ana menude KARINE logosu yok.");
  Assert.AreEqual(2, logo.childCount, "Katmanlar: LogoBase + DistressOverlay.");
  Assert.IsNotNull(logo.Q("LogoBase"));
  Assert.IsNotNull(logo.Q("DistressOverlay"));

  yield return null;
  var box = logo.layout;
  Assert.Greater(box.width, 0, "Logo hic yer kaplamamis.");
  // Tolerans pano olceklemesinin tam piksele yuvarlamasini kapsar (~%1);
   // gercek bir esnetme bundan cok daha buyuk sapar.
  Assert.AreEqual(KarineLogo.AspectRatio, box.width / box.height, 0.06f,
   "Ekranda olculen oran bozulmus — logo esnetiliyor.");

  CollectionAssert.IsEmpty(errors, "Hata olustu: " + string.Join(" | ", errors));
 }


 // Ana menu videosunun ustunde bes bagimsiz eylem ve Bora karti ciziliyor mu?
 [UnityTest] public IEnumerator MainMenu_ShowsMockupRows() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  for (int frame = 0; frame < 30; frame++) yield return null;

  var root = Object.FindFirstObjectByType<BubeApp>().GetComponent<UIDocument>().rootVisualElement;
  var labels = root.Query<Label>().ToList().Select(label => label.text).Where(text => !string.IsNullOrEmpty(text)).ToList();

  CollectionAssert.Contains(labels, "SORULARIN İZİ, GERÇEĞE YAKLAŞTIRIR", "Marka alt basligi yok.");
  Assert.IsTrue(labels.Any(text => text == "Oyuna Başla" || text == "Devam Et"),
   "Kayit durumuna uygun ana eylem yok.");
  foreach (var row in new[] { "Vakalar", "Kariyer", "Ayarlar", "Hakkında" })
   CollectionAssert.Contains(labels,row,"Menu satiri yok: "+row);
  CollectionAssert.Contains(labels, "Bora", "Personel karti yok.");

  // Simgeler: her satirin solunda maketten gelen gorsel duruyor mu. Glif
  // kullanilsaydi bunlar Label olurdu; arka plan gorseli aranir.
  var rows = root.Query<Button>().ToList()
   .Where(button => button.name == "MenuAction" || button.name == "MenuActionPrimary").ToList();
  Assert.AreEqual(5, rows.Count, "Bes menu satiri bulunamadi.");
  foreach (var row in rows)
   Assert.IsTrue(row.Children().Any(child => child.resolvedStyle.backgroundImage.texture != null),
    "Menu satirinda simge gorseli yok.");

  CollectionAssert.IsEmpty(errors, "Hata olustu: " + string.Join(" | ", errors));
 }

 [UnityTest] public IEnumerator CaseBrowser_CountrySwitchKeepsCardsScrollableAndLocksIntact() {
  SceneManager.LoadScene("BootScene",LoadSceneMode.Single);
  for(int frame=0;frame<30;frame++) yield return null;
  var app=Object.FindFirstObjectByType<BubeApp>();
  var method=typeof(BubeApp).GetMethod("WorldPage",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
  method.Invoke(app,null);
  for(int frame=0;frame<5;frame++) yield return null;
  var root=app.GetComponent<UIDocument>().rootVisualElement;
  var countries=root.Q<ScrollView>("CountryStrip");
  var cases=root.Q<ScrollView>("CaseStrip");
  Assert.IsNotNull(countries);Assert.IsNotNull(cases);
  Assert.AreEqual(Worlds.Load().countries.Count,countries.contentContainer.childCount);
  Assert.AreEqual(7,cases.contentContainer.childCount);
  Assert.IsNotNull(root.Q("CaseBrowserSidebar"));
  Assert.Greater(cases.contentContainer.layout.width,cases.contentViewport.layout.width,"Case cards must scroll, not shrink.");
  // Selecting another country is presentation only, and cannot open its cases.
  typeof(BubeApp).GetField("worldPick",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance).SetValue(app,1);
  method.Invoke(app,null);
  for(int frame=0;frame<5;frame++) yield return null;
  cases=root.Q<ScrollView>("CaseStrip");
  Assert.AreEqual(7,cases.contentContainer.childCount);
  foreach(var card in cases.contentContainer.Children()) {
   Assert.IsFalse(card.focusable,"Unreleased cases cannot take keyboard focus.");
   Assert.AreEqual(PickingMode.Ignore,card.pickingMode,"Locked cards cannot be pressed.");
  }
  CollectionAssert.IsEmpty(errors,string.Join(" | ",errors));
 }

 // Turkce buyuk harf karsilastirmasi; testin kaynagi aksansiz kalsin.
 static string Fold(string value) => value
  .Replace('\u0130', 'I').Replace('\u00C7', 'C').Replace('\u015E', 'S')
  .Replace('\u011E', 'G').Replace('\u00DC', 'U').Replace('\u00D6', 'O');

}
}
