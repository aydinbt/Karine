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
  "cine_skip",
 };

 // `AudioDirector`ın ve sahne sesinin beklediği klipler. Vakanın kendi ortam
 // sesi (`CaseData.ambienceId`) burada değil, vaka kuralında denetlenir.
 public static readonly string[] RequiredClips = new[] {
  AudioDirector.Press, AudioDirector.Typewriter,
  AudioDirector.Stamp, AudioDirector.Notification,
  "menu_theme", "room_office", "room_interview",
 }.Concat(AudioDirector.Chat).ToArray();

 // Ham renk borcu. 156 ile başladı; ekranlar bileşenlere taşınırken 8'e indi.
 // Kalan sekiz renk oyun **sanatıdır**, arayüz paleti değil: piksel portrenin göz
 // rengi (ten/saç/giysi artık vaka verisinden gelir) ve CCTV'nin cam, tarama,
 // parazit ve köşe işareti efektleri (`BubeApp.Cctv.cs`). İkisi de kit
 // paletinden gelmemeli. Yalnız aşağı çekilir.
 const int RawColorBudget = 8;

 public static void Validate(ValidationReport report) {
  report.Scope("proje");
  var expected = SceneOrder.Select(name => "Assets/Bube/Scenes/" + name + ".unity").ToArray();
  foreach (var path in expected)
   report.Require(File.Exists(path), "Zorunlu sahne yok: " + path);

  var enabled = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
  report.Require(enabled.SequenceEqual(expected),
   "Derleme sahne sırası Boot, Main Menu, Office, Interview olmalı. Şu an: " +
   (enabled.Length == 0 ? "(boş)" : string.Join(", ", enabled)));

  foreach (var font in new[] { "IBMPlexMono-Regular", "IBMPlexMono-SemiBold",
   "RobotoSlab-ExtraBold", "RobotoSlab-Bold", "Inter-Regular", "Inter-SemiBold",
   "AlfaSlabOne-Regular" })
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

  // Ses varlıkları. `AudioDirector` eksik klibi **sessiz** geçer — bu doğru
  // davranış (oyun ses dosyası olmadan da çalışır) ama aynı zamanda bir sesin
  // silinmesinin hiçbir yerde duyulmaması demek. Kural odur: ekranların
  // kullandığı sesler dosya olarak var mı.
  foreach (var clip in RequiredClips)
   report.Require(Resources.Load<AudioClip>(AudioDirector.Folder + clip) != null,
    "Ses dosyası yok: " + AudioDirector.Folder + clip);

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
  // Dosya boyu kilidi. `BubeApp` 3.125 satırlık tek dosyaydı; konu başına
  // parçalara ayrıldı. Yeni ekran eklerken yine tek dosyaya yığılmasın diye
  // en uzun çalışma zamanı dosyası 560 satırla sınırlanır. Yalnız aşağı çekilir.
  const int LongestRuntimeFile = 560;
  var longest = Directory.GetFiles("Assets/Bube/Runtime", "*.cs", SearchOption.AllDirectories)
   .Select(path => new { path, lines = File.ReadAllLines(path).Length })
   .OrderByDescending(item => item.lines).First();
  report.Require(longest.lines <= LongestRuntimeFile,
   "Çalışma zamanı dosyası çok uzadı (" + longest.lines + " > " + LongestRuntimeFile + "): " +
   longest.path.Replace('\\', '/') + ". Konu başına ayır.");

  var rawColors = Directory.GetFiles("Assets/Bube/Runtime", "*.cs", SearchOption.AllDirectories)
   .Where(path => !path.Replace('\\', '/').Contains("Runtime/UI/"))
   .Sum(path => File.ReadAllText(path).Split(new[] { "new Color(" }, System.StringSplitOptions.None).Length - 1);
  report.Require(rawColors <= RawColorBudget,
   "Ekranların içine yeni ham renk yazılmış (" + rawColors + " > " + RawColorBudget +
   "). Renk `KarineTheme`den alınır.");
  if (rawColors < RawColorBudget)
   report.Note("Ham renk borcu azalmış (" + rawColors + "/" + RawColorBudget +
    "); `RawColorBudget` bu sayıya çekilebilir.");

  // Kit'in düğmesi ses kapısını atlamamalı: `Runtime/UI` içindeki her
  // `new Button(` çağrısı `Sounded(` ile sarılır. Aksi hâlde yeni bir kit
  // bileşeni sessiz kalır ve bunu kimse fark etmez.
  foreach (var path in Directory.GetFiles("Assets/Bube/Runtime/UI", "*.cs", SearchOption.AllDirectories)) {
   var lines = File.ReadAllLines(path);
   for (int index = 0; index < lines.Length; index++) {
    if (!lines[index].Contains("new Button(")) continue;
    report.Require(lines[index].Contains("new Button(Sounded("),
     "Kit düğmesi ses kapısını atlıyor: " + path.Replace('\\', '/') + ":" + (index + 1) +
     ". `new Button(Sounded(...))` kullan.");
   }
  }

  // Punto da ekranın içine elle yazılmaz: her `style.fontSize` ataması tek
  // kapıdan, `Typography.Snap`ten geçer (CCTV'nin görüntüyle ölçeklenen kamera
  // yazısı `Mathf.Clamp` ile kendi ölçeğini kullanır).
  foreach (var path in Directory.GetFiles("Assets/Bube/Runtime", "*.cs", SearchOption.AllDirectories)) {
   var file = path.Replace('\\', '/');
   if (file.Contains("Runtime/UI/")) continue;
   var lines = File.ReadAllLines(path);
   for (int index = 0; index < lines.Length; index++) {
    if (!lines[index].Contains("style.fontSize=") && !lines[index].Contains("style.fontSize =")) continue;
    if (lines[index].Contains("Typography.Snap") || lines[index].Contains("Mathf.Clamp")) continue;
    report.Require(false, "Elle punto yazılmış: " + file + ":" + (index + 1) +
     ". Punto `Typography.Snap`ten geçer.");
   }
  }

  // Sinematikler Resources'ta degil StreamingAssets'ta durur; eksik bir video
  // oyunu durdurmaz ama o anin sessizce kaybolmasi fark edilmelidir.
  foreach (var video in new[] { "world01_intro.mp4", "case001_arrival.mp4", "main_menu_loop.mp4" })
   report.Require(File.Exists("Assets/StreamingAssets/Bube/" + video), "Sinematik video yok: " + video);
 }
}
}
