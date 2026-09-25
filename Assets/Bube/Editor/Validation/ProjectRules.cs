using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bube.Editor {

// Vakadan bağımsız proje kuralları: sahne sırası, fontlar, zorunlu görseller.
public static class ProjectRules {
 public static readonly string[] SceneOrder = { "BootScene", "MainMenuScene", "OfficeScene", "InterviewScene" };

 static readonly (string resource, string label)[] RequiredTextures = {
  ("Bube/DeskReference", "masa arka planı"),
  ("Bube/CctvTabletHands", "CCTV tablet görseli"),
  ("Bube/MainMenuNight", "ana menü arka planı"),
  ("Bube/InterviewRoom", "görüşme odası arka planı"),
  ("Bube/Characters/bora", "Bora sprite'ı"),
  (KarineLogo.BaseResource, "KARINE logosu"),
 };

 public static void Validate(ValidationReport report) {
  report.Scope("proje");
  var expected = SceneOrder.Select(name => "Assets/Bube/Scenes/" + name + ".unity").ToArray();
  foreach (var path in expected)
   report.Require(File.Exists(path), "Zorunlu sahne yok: " + path);

  var enabled = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
  report.Require(enabled.SequenceEqual(expected),
   "Derleme sahne sırası Boot, Main Menu, Office, Interview olmalı. Şu an: " +
   (enabled.Length == 0 ? "(boş)" : string.Join(", ", enabled)));

  foreach (var font in new[] { "IBMPlexMono-Regular", "IBMPlexMono-SemiBold" })
   report.Require(Resources.Load<Font>("Bube/Fonts/" + font) != null, "Arayüz fontu yok: " + font);

  foreach (var (resource, label) in RequiredTextures)
   report.Require(Resources.Load<Texture2D>(resource) != null, "Görsel yok (" + label + "): " + resource);

  // Marka oranı: logo dosyası değişirse `KarineLogo.AspectRatio` da değişmeli,
  // yoksa oran sessizce bozulur.
  var logo = Resources.Load<Texture2D>(KarineLogo.BaseResource);
  if (logo != null)
   report.Require(Mathf.Abs((float)logo.width / logo.height - KarineLogo.AspectRatio) < 0.01f,
    "Logo oranı `KarineLogo.AspectRatio` ile uyuşmuyor: görsel " + logo.width + "×" + logo.height);

  // İçe aktarım oranı: Unity'nin varsayılanı (`nPOTScale: 1`) ikinin kuvveti
  // olmayan her görseli en yakın kuvvete **çeker** ve iki ekseni ayrı ayrı
  // ölçeklediği için oranı bozar — 1672×941 ekrana 2048×1024 olarak gider,
  // 96×64 bayrak 128×64 olur. Metin denetimi görselin içini göremez ama içe
  // aktarma ayarını görebilir; kural budur.
  foreach (var meta in Directory.GetFiles("Assets/Bube/Resources", "*.png.meta", SearchOption.AllDirectories))
   report.Require(!File.ReadAllText(meta).Contains("nPOTScale: 1"),
    "Görsel içe aktarımda oranı bozulacak (nPOTScale: 1): " + meta);

  // Yazı tipi rolleri: eksik dosya oyunu durdurmaz (mono'ya düşer) ama not edilir.
  var missing = FontSet.Load().Missing;
  if (missing.Length > 0)
   report.Note("Yazı tipi rolü mono'ya düşüyor: " + string.Join(", ", missing));

  // Sinematikler Resources'ta degil StreamingAssets'ta durur; eksik bir video
  // oyunu durdurmaz ama o anin sessizce kaybolmasi fark edilmelidir.
  foreach (var video in new[] { "world01_intro.mp4", "case001_arrival.mp4" })
   report.Require(File.Exists("Assets/StreamingAssets/Bube/" + video), "Sinematik video yok: " + video);
 }
}
}
