using System;
using System.Collections;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Bube.Tests {

// Ekran görüntüsü karşılaştırması: ana ekranlar kurulur, görüntüsü alınır ve
// referansla karşılaştırılır. Referans yoksa yazılır ve test geçer; sonraki
// koşular ona göre ölçülür. Arayüz sabit boyutlu bir dokuya çizilir: batchmode'da
// kare sonu beklemesi hiç tetiklenmediği için ekran yakalama kullanılmaz. Grafiksiz
// koşuda (`-nographics`) çizim yoktur, test o zaman atlanır.
//
// Efektler (gren, yağmur, rastgele far) görüntüyü her koşuda biraz değiştirir;
// bu yüzden testte efektler kapatılır ve eşik ortalama piksel farkıdır.
public sealed class ScreenshotTests {
 const BindingFlags Any = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
 const float Threshold = .04f;
 BubeApp app; RenderTexture target;
 readonly System.Collections.Generic.List<(UnityEngine.UIElements.UIDocument, UnityEngine.UIElements.PanelSettings)> swapped = new System.Collections.Generic.List<(UnityEngine.UIElements.UIDocument, UnityEngine.UIElements.PanelSettings)>();
 FxLevel levelBefore; bool hapticsBefore;

 static string Folder => Environment.GetEnvironmentVariable("KARINE_BASELINES") is string env && env.Length > 0
  ? env : Path.Combine(Application.dataPath, "..", "Tools", "Baselines");

 [UnitySetUp] public IEnumerator Setup() {
  if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) Assert.Ignore("Grafik aygıtı yok; ekran görüntüsü alınamaz.");
  levelBefore = Fx.Level; hapticsBefore = Fx.Haptics;
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  float until = Time.realtimeSinceStartup + 2f;
  while (Time.realtimeSinceStartup < until) yield return null;
  app = UnityEngine.Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
  target = new RenderTexture(2400, 1080, 24, RenderTextureFormat.ARGB32); target.Create();
  foreach (var document in UnityEngine.Object.FindObjectsByType<UnityEngine.UIElements.UIDocument>(FindObjectsSortMode.None)) {
   swapped.Add((document, document.panelSettings));
   var settings = UnityEngine.Object.Instantiate(document.panelSettings); settings.targetTexture = target; settings.clearColor = true;
   document.panelSettings = settings;
  }
  SaveSandbox.ResetToInitialCase(app);
  Fx.Set(FxLevel.Off, hapticsBefore);
 }

 [UnityTearDown] public IEnumerator Teardown() {
  foreach (var (document, settings) in swapped) if (document != null) document.panelSettings = settings;
  swapped.Clear();
  if (target != null) { target.Release(); UnityEngine.Object.Destroy(target); target = null; }
  Fx.Set(levelBefore, hapticsBefore); yield return null;
 }

 [UnityTest] public IEnumerator Home_MatchesBaseline() => Check("Home", "home");
 [UnityTest] public IEnumerator Desk_MatchesBaseline() => Check("Desk", "desk");
 [UnityTest] public IEnumerator Settings_MatchesBaseline() => Check("SettingsPage", "settings");

 IEnumerator Check(string screen, string name) {
  typeof(BubeApp).GetMethod(screen, Any, null, Type.EmptyTypes, null).Invoke(app, null);
  for (int i = 0; i < 30; i++) yield return null;
  var before = RenderTexture.active; RenderTexture.active = target;
  var shot = new Texture2D(target.width, target.height, TextureFormat.RGBA32, false);
  shot.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0); shot.Apply();
  RenderTexture.active = before;
  try {
   Directory.CreateDirectory(Folder);
   string path = Path.Combine(Folder, name + "_" + shot.width + "x" + shot.height + ".png");
   if (!File.Exists(path)) { File.WriteAllBytes(path, shot.EncodeToPNG()); Assert.Pass("Referans yazıldı: " + path); }
   var baseline = new Texture2D(2, 2); baseline.LoadImage(File.ReadAllBytes(path));
   float diff = Difference(shot, baseline);
   if (diff > Threshold) File.WriteAllBytes(Path.ChangeExtension(path, ".actual.png"), shot.EncodeToPNG());
   Assert.LessOrEqual(diff, Threshold, name + " ekranı referanstan farklı (ortalama fark " + diff.ToString("0.000") + "). Yeni görüntü: .actual.png");
  } finally { UnityEngine.Object.Destroy(shot); }
 }

 static float Difference(Texture2D a, Texture2D b) {
  if (a.width != b.width || a.height != b.height) return 1f;
  var x = a.GetPixels32(); var y = b.GetPixels32();
  double sum = 0; int step = 7, count = 0;
  for (int i = 0; i < x.Length; i += step, count++)
   sum += (Math.Abs(x[i].r - y[i].r) + Math.Abs(x[i].g - y[i].g) + Math.Abs(x[i].b - y[i].b)) / (3.0 * 255);
  return (float)(sum / Math.Max(1, count));
 }
}
}
