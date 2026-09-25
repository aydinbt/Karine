using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bube.Editor {

// Vakadan bağımsız proje kuralları: sahne sırası, fontlar, zorunlu görseller.
public static class ProjectRules {
 public static readonly string[] SceneOrder = { "BootScene", "MainMenuScene", "OfficeScene", "InterviewScene" };

 static readonly (string resource, string label)[] RequiredTextures = {
  ("Bube/DeskV2", "masa arka planı"),
  ("Bube/CctvTabletHands", "CCTV tablet görseli"),
  ("Bube/MainMenuNight", "ana menü arka planı"),
  ("Bube/InterviewRoom", "görüşme odası arka planı"),
  ("Bube/Characters/bora", "Bora sprite'ı"),
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

  // Sinematikler Resources'ta degil StreamingAssets'ta durur; eksik bir video
  // oyunu durdurmaz ama o anin sessizce kaybolmasi fark edilmelidir.
  foreach (var video in new[] { "world01_intro.mp4", "case001_arrival.mp4" })
   report.Require(File.Exists("Assets/StreamingAssets/Bube/" + video), "Sinematik video yok: " + video);
 }
}
}
