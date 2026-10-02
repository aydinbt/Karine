using UnityEngine;

namespace Bube {

// KARINE UI/UX Kit'in tek kaynağı. Kit görseli (`Docs/Reference/UI_KIT.png`)
// bağlayıcı spesifikasyondur; buradaki her değer oradan okunmuştur.
//
// Kural: ekranların içine renk, punto, boşluk veya süre **yazılmaz**; hepsi
// buradan alınır. Böylece kit değişirse tek dosya değişir ve bütün ekranlar
// birlikte kayar. Yeni bir renk gerekiyorsa önce kitte karşılığı aranır.
public static class KarineTheme {
 public static class Motion {
  public const float PaperArrivalSeconds=.7f, PageSeconds=.2f;
  public const int PageOffset=18, TabLift=8;
  public const float TabletSeconds=.45f, CloseSeconds=.3f, PaperSeconds=.28f, PressScale=.975f;
  public const int TickMs=16, ReleaseMs=120, PaperOffset=28;
 }

 // İkinci kademe efektleri (`KarineUI.Effects`, `BubeApp.Effects`).
 public static class Effects {
  public const float SlideSeconds=.38f, SlideTilt=5f, SlideMaxWidth=.36f, SlideTargetX=.445f, SlideTargetY=.5f;
  public const float SwipeDistance=110f;
  public const int PrintChars=4, PrintSoundEvery=5;
  public const float SignalSeconds=.32f; public const int SignalBands=7;
  public const float TurnSeconds=.26f, FoldWidth=14f, FoldAlpha=.28f;
  // Nefes: dört saniyelik döngü, binde birkaçlık genişleme. Herkeste aynı.
  public const float BreathSeconds=4.2f, BreathWidth=.004f, BreathHeight=.008f;
  // Yanıttan önceki duraksama: her yanıtta aynı süre.
  public const int AnswerPauseMs=380;
  // Kare dizisinden CCTV: kare başına süre (kayıt kendi `frameMs`ini verebilir),
  // kare değişimindeki titreme.
  public const int CctvFrameMs=650;
  public const float CctvFlickerSeconds=.12f, CctvJitter=3f;
 }

 public static class Settings {
  public const int SideWidth=210, TabHeight=84, ChoiceHeight=74, HeadingSize=26, TextSize=17;
  public const int Inset=14, Top=12, Bottom=6, NarrowInset=4;
 }


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

 // Ana menü: sinematik videonun üstünde duran, yeniden kullanılabilir
 // arayüz parçalarının ölçüleri. Yatay referans düzeni 1280×720'dir.
 public static class MainMenu {
  public const int LogoWidth = 455;
  public const int ColumnWidth = 355;
  public const int RowHeight = 48;
  public const int RowGap = 5;
  public const int PortraitSize = 78;
  public const int IdentityHeight = 108;
  public const int TaglineSize = 14;
  public const int RowTextSize = 18;
 }

 public static class CaseBrowser {
  public const int SidebarWidth=218, LogoWidth=194, Portrait=100;
  public const int TitleSize=36, TextSize=15, SmallSize=12;
  public const int CountryWidth=124, CountryHeight=132;
  public const int CardWidth=118, CardHeight=194, PhotoHeight=78;
  public const int MainLeft=254, Top=48, Bottom=28;
  public const int TapeWidth=38, TapeHeight=9, BannerHeight=94, HeadingHeight=80, ProgressWidth=260, ProgressHeight=6;
 }

 public static class CctvArchive {
  public const int TitleSize=19, TextSize=17, MetaSize=14, RowHeight=40, TimeWidth=96;
  public const int CameraRow=96, CameraTile=68, HeadingSize=21;
  public const float SidebarWidth=29;
  public const string CameraIcon="cctv"; // simge gelene kadar binoculars'a düşer
 }
 public static class Button {
  public const int Slice=22;
  public const float SliceScale=.75f, PressDepth=1;
  public static readonly Color PressedTint=new Color(.78f,.74f,.68f,1), DisabledTint=new Color(.5f,.5f,.5f,.7f);
 }
 public static class Requests {
  public const int Portrait=76, DetailPortrait=118, RowHeight=104, TitleSize=21, BodySize=15, Badge=24, Chevron=18;
  // Talep tableti üst çubuğun (yüzde 11) altına iner; genişlik de aynı oranda küçülür ki görsel bozulmasın.
  public const float HeaderClearance=11;
  public const float RailWidth=21, ListWidth=46, DetailWidth=33;
 }
 public static class Inbox {
  public const int RowHeight=76, LogoWidth=145, HeadingSize=25, BodySize=17;
  public static readonly Rect List=new Rect(6,15,32,78);
  public static readonly Rect Paper=new Rect(40,16,54,76);
  // Yeni görünüm (2 Ekim 2026): gece ofisi sahnesi, karton dosya, eskimiş kâğıt, ataş.
  public static readonly Rect Folder=new Rect(38.6f,11.5f,57.2f,83);
  public const float SceneVeil=.38f;
  public const int BrandLogo=46, ClipWidth=30;
 }
 public static class Dossier {
  public const int LogoWidth=120, TitleSize=28, BodySize=17, MetaSize=14, PhotoHeight=230;
  public const int HeaderHeight=76, PortraitSize=64, TabGap=4;
  public static readonly Rect Sheet=new Rect(7,14,73,79);
  public static readonly Rect Tabs=new Rect(80,19,18,73);
  // Yeni görünüm (2 Ekim 2026): kâğıdın arkasındaki karton dosya, polaroid fotoğraf.
  public static readonly Rect Folder=new Rect(4.5f,11.5f,78,86);
  public const float PhotoTilt=2.5f;
  public const int PhotoCaptionPad=34, MetaIcon=22, SectionIcon=24, TapeWidth=90, ItemHeight=74;
 }

 public static class Office {
  public const float Aspect=1672f/941f;
  public const int BrandWidth=185,TitleSize=19,LabelSize=14,SmallSize=12;
  public const int BadgeSize=24,HeaderHeight=80,HeaderActionWidth=96,PortraitWidth=68,PortraitHeight=72;
  public const int BlinkMs=520;
  // Coordinates in the reusable room plate; all props share the same stage.
  public static readonly Rect Window=new Rect(30.3f,7.8f,39.3f,35.9f);
  public static readonly Rect Board=new Rect(65,14,27,28);
  public static readonly Rect Lamp=new Rect(2,22,24,43);
  public static readonly Rect Inbox=new Rect(8,56,21,27);
  public static readonly Rect Phone=new Rect(28,57,17,25);
  public static readonly Rect Folder=new Rect(32,66,29,26);
  public static readonly Rect Monitor=new Rect(62,41,28,32);
  public static readonly Rect Evidence=new Rect(73,74,26,20);
  public static readonly Rect InboxLabel=new Rect(9.4f,51.3f,15.2f,6.7f);
  public static readonly Rect PhoneLabel=new Rect(30.3f,51.3f,15.4f,6.4f);
  public static readonly Rect FolderLabel=new Rect(40.6f,73.4f,15.1f,6.6f);
  public static readonly Rect MonitorLabel=new Rect(67.3f,52.4f,16.9f,6.9f);
  public static readonly Rect EvidenceLabel=new Rect(82,77.9f,15.1f,6.7f);
  // Masanın havası (`KarineUI.OfficeAtmosphere`). Işık ve kararma kit paletinden
  // boyanır; buradaki sayılar yalnız miktar ve hızdır.
  public static class Atmosphere {
   public static readonly Rect LightPool=new Rect(-14,26,62,74);
   public static readonly Rect DustArea=new Rect(4,24,34,48);
   public const float LightAlpha=.20f, LightBreath=.06f, LightBreathSpeed=.7f;
   public const float VignetteAlpha=.62f;
   public const int DustCount=18;
   public const float DustMin=2f, DustMax=4.5f, DustRise=1.6f, DustAlpha=.45f;
   public const float Reach=.011f, BackDepth=.5f, BackScale=1.03f;
   public const float Follow=.07f, TiltGain=2.2f, TiltRecenter=.004f;
   public const float PressSeconds=.07f, ReleaseSeconds=.26f, PressDepth=2f, PressScale=.03f, PopScale=.02f, ShadowAlpha=.25f;
  }
 }

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
