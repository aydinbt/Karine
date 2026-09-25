using UnityEngine;

namespace Bube {

// KARINE UI/UX Kit'in tek kaynağı. Kit görseli (`Docs/Reference/UI_KIT.png`)
// bağlayıcı spesifikasyondur; buradaki her değer oradan okunmuştur.
//
// Kural: ekranların içine renk, punto, boşluk veya süre **yazılmaz**; hepsi
// buradan alınır. Böylece kit değişirse tek dosya değişir ve bütün ekranlar
// birlikte kayar. Yeni bir renk gerekiyorsa önce kitte karşılığı aranır.
public static class KarineTheme {

 // --- Palet (kit görselindeki etiketli sekiz kutu) ---------------------------
 public const string BackgroundHex = "#0B0F14";
 public const string PanelHex      = "#1B2228";
 public const string Panel2Hex     = "#2F3A3F";
 public const string PrimaryHex    = "#E8DCC4";
 public const string SecondaryHex  = "#C9B38C";
 public const string AccentHex     = "#8F7A5A";
 public const string ActiveHex     = "#29D3C3";
 public const string DangerHex     = "#E94F4F";

 public static readonly Color Background = Hex(BackgroundHex);
 public static readonly Color Panel      = Hex(PanelHex);
 public static readonly Color Panel2     = Hex(Panel2Hex);
 public static readonly Color Primary    = Hex(PrimaryHex);   // krem: birincil eylem, seçili durum
 public static readonly Color Secondary  = Hex(SecondaryHex); // ikincil metin, kâğıt tonu
 public static readonly Color Accent     = Hex(AccentHex);    // sıcak kahve: vurgu, çizgi
 public static readonly Color Active     = Hex(ActiveHex);    // teal: yalnız terminal/dijital sistem
 public static readonly Color Danger     = Hex(DangerHex);    // yalnız yıkıcı eylem ve uyarı

 // Kit'te ayrı kutusu yok ama gerekli iki türev: sönük metin ve devre dışı yüzey.
 // Paletin dışına çıkmazlar, paletten üretilirler.
 public static readonly Color Muted    = Mix(Secondary, Panel, .45f);
 public static readonly Color Disabled = Mix(Secondary, Panel, .70f);

 // Birincil düğmenin üstündeki yazı: koyu zemin rengi. "Cream background,
 // dark text" kuralı buradan gelir, ekranlar kendi koyusunu seçmez.
 public static readonly Color OnPrimary = Background;

 // --- Örtü ve cam ----------------------------------------------------------
 // Ekranlar kendi koyusunu seçmez. Bir katman açıldığında altındaki sahneyi
 // örten perde tek yerden gelir; yoğunluk (alfa) sahneye göre değişir.
 public static Color Veil(float alpha) => new Color(.02f, .025f, .03f, alpha);

 // Terminalin/tabletin cam yüzeyi. Paletin koyu ucundan türer, ayrı bir renk
 // ailesi değildir: Background ile Panel arasında üç durak.
 public static readonly Color GlassDeep = Mix(Background, Panel, .25f); // en dip: tablet, kayıt listesi
 public static readonly Color Glass     = Mix(Background, Panel, .55f); // gövde: sorgu şeridi, başlık
 public static readonly Color GlassLift = Mix(Panel, Panel2, .45f);     // üstteki kart, seçim satırı

 // Masadaki dokunulabilir noktanın üstüne gelince görünen sıcak iz. Kit'in
 // vurgu kahvesinden üretilir, kendi sarısını uydurmaz.
 public static Color HotspotHover => Alpha(Accent, .18f);

 // --- Boşluk ---------------------------------------------------------------
 // Tek ölçek; ara değer kullanılmaz.
 public const int SpaceXs = 4;
 public const int SpaceSm = 8;
 public const int SpaceMd = 12;
 public const int SpaceLg = 18;
 public const int SpaceXl = 24;

 // --- Kenar ve köşe --------------------------------------------------------
 // Kit'in dili neredeyse dik köşedir: fiziksel evrak hissi yuvarlak karttan
 // gelmez. 2 pikselin üstüne çıkılmaz.
 public const int Radius = 2;
 public const int BorderWidth = 1;
 public const int PrimaryEdgeWidth = 4; // birincil düğmenin sol kenar şeridi

 // --- Dokunma --------------------------------------------------------------
 // Kit: "yaklaşık 48–56 dp". Görsel ikon küçülebilir, hedef küçülemez.
 public const int TouchTarget = 48;
 public const int TouchTargetComfortable = 56;
 public const int IconSize = 22;       // çizgi ikonun kendisi
 public const int IconButtonSize = 48; // ikonu taşıyan kare düğme

 // --- Devinim (saniye) -----------------------------------------------------
 // Kit'in verdiği aralıkların ortası. Bounce/elastic yok.
 public const float PressMs = 0.10f;
 public const float PanelMs = 0.20f;
 public const float ModalMs = 0.22f;
 public const float NoticeMs = 0.25f;

 // --- Diegetic evrak katmanı ----------------------------------------------
 // Kit §13: oyun dünyasının parçası olan şeyler (dosya, evrak, faks, terminal)
 // HUD paletiyle boyanmaz — masadaki kâğıt kâğıt gibi görünür. Bu katman da
 // tek yerde tanımlıdır; ekranların içinde tekrar yazılmaz.
 public static class Paper {
  public static readonly Color Sheet = Hex("#E8D9BA"); // kâğıdın kendisi
  public static readonly Color Ink   = Hex("#212933"); // kâğıdın üstündeki yazı
  public static readonly Color Faded = Hex("#635C4F"); // ikincil satır, tarih
  public static readonly Color Stamp = Hex("#733A2E"); // mühür/arma kahvesi
  public static readonly Color Light = Hex("#FAEDD4"); // kâğıdın aydınlık yeri
  public static readonly Color Tint  = Hex("#D1C2A6"); // kâğıdın üstündeki kart/şerit
  public static readonly Color Edge  = Hex("#9E927C"); // kâğıt üstü çizgi ve kenar

  // Kâğıdın altındaki fiziksel malzeme: dosya kabı, klasör sırtı, mukavva.
  // Ekranlar bu kahveleri tek tek uydurmasın diye üç durak yeter.
  public static readonly Color Folder     = Hex("#4A2921"); // dosya kabının yüzü
  public static readonly Color FolderEdge = Hex("#4F3024"); // kabın üst kenarı, logo gölgesi
  public static readonly Color FolderDeep = Hex("#1F1411"); // sırtın gölgesi, klasör dibi
  public static readonly Color Board      = Hex("#291F1A"); // mukavva/pano yüzeyi
  public static readonly Color Approved   = Hex("#29634F"); // kabul mührünün yeşili
 }

 // "#RRGGBB" → Color. Ayrıştırılamayan değer sessizce siyaha düşmez, magenta
 // olur: yanlış yazılmış bir sabit ekranda hemen görülsün.
 public static Color Hex(string value) =>
  ColorUtility.TryParseHtmlString(value, out var color) ? color : Color.magenta;

 static Color Mix(Color a, Color b, float t) => Color.Lerp(a, b, t);

 // Paletteki bir rengi saydamlaştırmak için: ekran kendi RGB'sini yazmasın.
 public static Color Alpha(Color value, float alpha) =>
  new Color(value.r, value.g, value.b, alpha);
}
}
