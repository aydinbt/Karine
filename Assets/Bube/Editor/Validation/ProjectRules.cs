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


 // `Docs/Reference/UI_KIT.png` içinden kesilmiş ortak ikonlar.
 public static readonly string[] KitIcons = {
  "folder", "document", "gear", "binoculars", "pin", "people", "chart", "more",
  "close", "alert", "info", "nav_prev", "nav_next", "menu_quit",
 };

 // 25 Eylül 2026'da ölçülen borç. Yalnız aşağı çekilir.
 const int RawColorBudget = 156;

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

  // Ana menü simgeleri: eksik bir dosya satırı simgesiz bırakır, bu sessizce
  // maketten uzaklaşmak demektir.
  // Kit'in ortak ikon dili (§15): aynı işlev her ekranda aynı ikon. Eksik bir
  // dosya, o işlevin ekranda simgesiz kalması demektir.
  foreach (var icon in KitIcons)
   report.Require(Resources.Load<Texture2D>("Bube/Art/Icons/" + icon) != null,
    "Kit ikonu yok: " + icon);

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

  // Kit görseli deponun içinde durmalı: bağlayıcı spesifikasyon, sohbete
  // iliştirilmiş bir ek değil.
  report.Require(File.Exists("Docs/Reference/UI_KIT.png"),
   "KARINE UI Kit referans görseli yok: Docs/Reference/UI_KIT.png");

  // Ham renk kilidi (ratchet). Ekranların içindeki `new Color(...)` çağrıları
  // kit'ten önce yazılmış eski borçtur; hepsini tek oturumda temizlemek yerine
  // **büyümesi** engelleniyor. Yeni kod rengi `KarineTheme`den alır; bu sayı
  // ancak borç azaldıkça düşürülür, asla yükseltilmez.
  var rawColors = Directory.GetFiles("Assets/Bube/Runtime", "*.cs", SearchOption.AllDirectories)
   .Where(path => !path.Replace('\\', '/').Contains("Runtime/UI/"))
   .Sum(path => File.ReadAllText(path).Split(new[] { "new Color(" }, System.StringSplitOptions.None).Length - 1);
  report.Require(rawColors <= RawColorBudget,
   "Ekranların içine yeni ham renk yazılmış (" + rawColors + " > " + RawColorBudget +
   "). Renk `KarineTheme`den alınır.");
  if (rawColors < RawColorBudget)
   report.Note("Ham renk borcu azalmış (" + rawColors + "/" + RawColorBudget +
    "); `RawColorBudget` bu sayıya çekilebilir.");

  // Sinematikler Resources'ta degil StreamingAssets'ta durur; eksik bir video
  // oyunu durdurmaz ama o anin sessizce kaybolmasi fark edilmelidir.
  foreach (var video in new[] { "world01_intro.mp4", "case001_arrival.mp4", "main_menu_loop.mp4" })
   report.Require(File.Exists("Assets/StreamingAssets/Bube/" + video), "Sinematik video yok: " + video);
 }
}
}
