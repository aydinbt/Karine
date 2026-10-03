using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube.Tests {

// KARINE UI/UX Kit sözleşmesi. Kit görseli bağlayıcı spesifikasyon olduğu için
// buradaki testler "kod derleniyor mu"yu değil, **kit'in kurallarını** sorar:
// palet, düğme hiyerarşisi, dokunma hedefi, tipografi rolleri, modal sırası.
public sealed class UiKitTests {

 [SetUp] public void Setup() => KarineUI.Fonts = FontSet.Load();

 static VisualElement Host() => new VisualElement();

 // Kit görselindeki sekiz etiketli kutu. Bir renk değişirse burada da
 // değişmelidir — sessizce kaymasın.
 // Düğmeler dokulu görsel taşır (2 Ekim 2026); hangi görselin bağlandığını döndürür.
 static string Skin(VisualElement button) {
  var image = button.style.backgroundImage.value.texture;
  return image == null ? null : image.name;
 }

 [Test]
 public void Palette_MatchesTheKitSwatches() {
  Assert.AreEqual("#0E0F11", KarineTheme.BackgroundHex);
  Assert.AreEqual("#1A1D22", KarineTheme.PanelHex);
  Assert.AreEqual("#252A32", KarineTheme.Panel2Hex);
  Assert.AreEqual("#E6E1D3", KarineTheme.PrimaryHex);
  Assert.AreEqual("#9AA0A6", KarineTheme.SecondaryHex);
  Assert.AreEqual("#D99A2B", KarineTheme.AccentHex);
  Assert.AreEqual("#D99A2B", KarineTheme.ActiveHex);
  Assert.AreEqual("#C64040", KarineTheme.DangerHex);
 }

 [Test]
 public void Hex_ThatCannotBeParsed_IsLoudNotBlack() =>
  Assert.AreEqual(Color.magenta, KarineTheme.Hex("kirik-deger"));

 // PRIMARY = dolu kehribar + koyu yazı (3 Ekim 2026 panosu). Tek dominant eylem kuralı buna dayanır.
 [Test]
 public void PrimaryButton_IsAmberWithDarkText() {
  var button = KarineUI.Button_(Host(), "DEVAM ET", null, KarineButtonKind.Primary);
  Assert.IsNull(Skin(button));
  Assert.AreEqual(KarineTheme.Accent, button.style.backgroundColor.value);
  Assert.AreEqual(KarineTheme.OnPrimary, button.style.color.value);
 }

 // SECONDARY = koyu cam + ince gri kenar + açık yazı.
 [Test]
 public void SecondaryButton_IsDarkGlassWithGreyBorder() {
  var button = KarineUI.Button_(Host(), "GERİ", null, KarineButtonKind.Secondary);
  Assert.IsNull(Skin(button));
  Assert.AreEqual(KarineTheme.Border, button.style.borderTopColor.value);
  Assert.AreEqual(KarineTheme.Primary, button.style.color.value);
 }

 [Test]
 public void GhostButton_HasNoFill() {
  var button = KarineUI.Button_(Host(), "GEÇ", null, KarineButtonKind.Ghost);
  Assert.AreEqual(Color.clear, button.style.backgroundColor.value);
 }

 [Test]
 public void DangerButton_UsesDangerOnlyForItself() {
  var button = KarineUI.Button_(Host(), "SİL", null, KarineButtonKind.Danger);
  Assert.AreEqual(KarineTheme.Danger, button.style.color.value);
  Assert.AreNotEqual(KarineTheme.Danger, button.style.backgroundColor.value,
   "Kırmızı dekoratif zemin değildir.");
 }

 [Test]
 public void DisabledButton_IsFlatAndNotClickable() {
  var button = KarineUI.Button_(Host(), "DEVAM ET", null, KarineButtonKind.Primary, false);
  Assert.IsFalse(button.enabledSelf);
  Assert.AreEqual(KarineTheme.Panel2, button.style.backgroundColor.value, "Devre dışı düğme kehribar taşımaz.");
 }

 // Kit: yaklaşık 48–56 dp. Dokunma hedefi hiçbir bileşende bunun altına inmez.
 [Test]
 public void EveryInteractiveComponent_MeetsTheTouchTarget() {
  Assert.GreaterOrEqual(KarineUI.Button_(Host(), "X", null).style.minHeight.value.value, KarineTheme.TouchTarget);
  Assert.GreaterOrEqual(KarineUI.IconButton(Host(), "close", null).style.height.value.value, KarineTheme.TouchTarget);
  var tabs = KarineUI.Tabs(Host(), new[] { "DOSYALAR", "GÖRÜŞMELER" }, 0, null);
  foreach (var tab in tabs.Children())
   Assert.GreaterOrEqual(tab.style.minHeight.value.value, KarineTheme.TouchTarget);
 }

 // Seçili sekme krem, ötekiler değil — "selected state = cream" kuralı.
 [Test]
 public void SelectedTab_IsTheCreamOne() {
  var tabs = KarineUI.Tabs(Host(), new[] { "A", "B", "C" }, 1, null).Children().ToList();
  Assert.AreEqual(KarineTheme.Primary, tabs[1].style.backgroundColor.value);
  Assert.AreNotEqual(KarineTheme.Primary, tabs[0].style.backgroundColor.value);
 }

 // Teknik metin monospace, başlık serif. Logo fontu hiçbirine girmez.
 [Test]
 public void TypographyRoles_GoToTheRightFonts() {
  var fonts = FontSet.Load();
  var technical = KarineUI.Technical(Host(), "DOSYA #001");
  var title = KarineUI.Title(Host(), "KURUM GÜVENİ", 21);
  if (fonts.Mono != null)
   Assert.AreEqual(fonts.Mono, technical.style.unityFontDefinition.value.font);
  if (fonts.Heading != null)
   Assert.AreEqual(fonts.Heading, title.style.unityFontDefinition.value.font);
 }

 // Modal hiyerarşisi: başlık → açıklama → ikincil eylem → birincil eylem.
 // Onay her zaman sonuncudur; vazgeç onun solunda kalır.
 [Test]
 public void Modal_PutsConfirmLastAndMakesItTheOnlyPrimary() {
  var host = Host();
  KarineUI.Modal(host, "BU İŞLEMİ ONAYLIYOR MUSUN?", "Geri alınamaz.",
   "VAZGEÇ", null, "ONAYLA", null);
  var buttons = host.Query<Button>().ToList();
  Assert.AreEqual(2, buttons.Count, "Modal'da tek ikincil, tek birincil eylem olur.");
  Assert.AreEqual("VAZGEÇ", buttons[0].text);
  Assert.AreEqual("ONAYLA", buttons[1].text);
  Assert.AreEqual(KarineTheme.Accent, buttons[1].style.backgroundColor.value);
  Assert.AreEqual(KarineTheme.Panel, buttons[0].style.backgroundColor.value);
 }

 [Test]
 public void DestructiveModal_ConfirmIsDangerNotCream() {
  var host = Host();
  KarineUI.Modal(host, "SİLİNSİN Mİ?", "Geri alınamaz.", "VAZGEÇ", null, "SİL", null, true);
  var confirm = host.Query<Button>().ToList()[1];
  Assert.AreEqual(KarineTheme.Danger, confirm.style.color.value);
 }

 // Kit'in köşe dili neredeyse diktir; yuvarlak kart uygulaması gibi görünmemeli.
 [Test]
 public void CornerLanguage_StaysAlmostSquare() {
  Assert.LessOrEqual(KarineTheme.Radius, 2);
  var panel = KarineUI.Panel(Host());
  Assert.LessOrEqual(panel.style.borderTopLeftRadius.value.value, 2);
 }

 // Devinim: hızlı, sönümlü, zıplamasız. Üst sınır kit'in verdiği aralıktır.
 [Test]
 public void Motion_StaysInsideTheKitDurations() {
  Assert.LessOrEqual(KarineTheme.PressMs, 0.12f);
  Assert.LessOrEqual(KarineTheme.PanelMs, 0.25f);
  Assert.LessOrEqual(KarineTheme.ModalMs, 0.25f);
  Assert.LessOrEqual(KarineTheme.NoticeMs, 0.30f);
 }

 // Aynı işlev = aynı ikon. Ortak ikon kümesinin tamamı yüklenebilmeli.
 [Test]
 public void KitIcons_AreAllPresent() {
  foreach (var name in Bube.Editor.ProjectRules.KitIcons)
   Assert.IsNotNull(Resources.Load<Texture2D>("Bube/Art/Icons/" + name), "Kit ikonu yok: " + name);
 }

 // Evrak gezintisi: `<  03 / 07  >`. Sayaç monospace, ilk sayfada geri kapalı.
 [Test]
 public void DocumentNav_ReadsLikeAPhysicalDocument() {
  var host = Host();
  KarineUI.DocumentNav(host, 1, 7, null, null);
  var counter = host.Query<Label>().ToList().First(label => label.text.Contains("/"));
  Assert.AreEqual("01 / 07", counter.text);
  var arrows = host.Query<Button>().ToList();
  Assert.IsFalse(arrows[0].enabledSelf, "İlk sayfada geri oku kapalı olmalı.");
  Assert.IsTrue(arrows[1].enabledSelf);
 }

 [Test]
 public void DocumentNav_LastPage_ClosesTheForwardArrow() {
  var host = Host();
  KarineUI.DocumentNav(host, 7, 7, null, null);
  var arrows = host.Query<Button>().ToList();
  Assert.IsTrue(arrows[0].enabledSelf);
  Assert.IsFalse(arrows[1].enabledSelf);
 }

 // Bildirim ekranı kaplamaz.
 [Test]
 public void Notification_StaysSmall() {
  var card = KarineUI.Notification(Host(), "YENİ EVRAK VAR", "Masana yeni bir dosya gönderildi.",
   "GÖRÜNTÜLE", null, null);
  Assert.LessOrEqual(card.style.maxWidth.value.value, 480);
 }

 // Sinematikte tek denetim GEÇ'tir: duraklatma, ilerleme ve hızlandırma yok.
 // Tek eylem olduğu için kit'in birincil düğmesidir: dolu krem zemin, koyu
 // yazı, sol eylem kenarı. Zemin **saydam olamaz** — filmin üstünde bile.
 [Test]
 public void SkipButton_IsTheOnlyCinematicControl() {
  var host = Host();
  var skip = KarineUI.SkipButton(host, "GEÇ", () => { });
  Assert.AreEqual(1, host.Query<Button>().ToList().Count, "Sinematikte tek düğme olmalı.");
  Assert.IsTrue(string.IsNullOrEmpty(skip.text), "Metin ayrı etiketten gelmeli.");
  var label = skip.Query<Label>().ToList().SingleOrDefault(item => item.text == "GEÇ");
  Assert.IsNotNull(label, "GEÇ yazısı yok.");
  Assert.AreEqual(KarineTheme.OnPrimary, label.style.color.value,
   "Birincil düğmede yazı koyu olmalı.");
  Assert.AreEqual(KarineTheme.Primary, skip.style.backgroundColor.value,
   "Zemin kit'in birincil kremi olmalı.");
  Assert.GreaterOrEqual(skip.style.minHeight.value.value, KarineTheme.TouchTargetComfortable,
   "GEÇ rahat dokunma hedefinde olmalı.");
  Assert.AreEqual(KarineTheme.PrimaryEdgeWidth, skip.style.borderLeftWidth.value,
   "Birincil eylem kenarı yok: düz kare düğme gibi duruyor.");
  Assert.AreEqual(KarineTheme.Accent, skip.style.borderTopColor.value, "Kit çerçevesi yok.");
  Assert.AreEqual(1f, skip.style.backgroundColor.value.a,
   "GEÇ saydam olmamalı: kit düğmesi dolu zemin taşır.");
 }

 // Bütün yazı rolleri Chakra Petch ailesine bağlıdır.
 [Test]
 public void Typography_UsesChakraPetchForAllTextRoles() {
  var set = FontSet.Load();
  Assert.IsNotNull(set.Display, "Display rolü yok.");
  Assert.AreSame(set.Mono, set.Body, "Teknik metin ve gövde Chakra Petch Regular kullanmalı.");
  Assert.AreNotSame(set.Mono, set.Heading, "Başlık mono olmamalı.");

  var host = Host();
  var big = KarineUI.Title(host, "DOSYA", KarineUI.DisplayFrom);
  var small = KarineUI.Title(host, "DOSYA", 21);
  var technical = KarineUI.Technical(host, "00:12");
  var body = KarineUI.Body_(host, "Gövde");
  Assert.AreEqual(FontDefinition.FromFont(set.Display), big.style.unityFontDefinition.value,
   "Büyük başlık Chakra Petch Bold kullanmalı.");
  Assert.AreEqual(FontDefinition.FromFont(set.Heading), small.style.unityFontDefinition.value,
   "Küçük başlık Chakra Petch Bold kullanmalı.");
  Assert.AreEqual(FontDefinition.FromFont(set.Mono), technical.style.unityFontDefinition.value,
   "Teknik metin Chakra Petch kullanmalı.");
  Assert.AreEqual(FontDefinition.FromFont(set.Body), body.style.unityFontDefinition.value,
   "Gövde Chakra Petch Regular olmalı.");
 }

 // Radyo: seçili olan dolu halka ve krem yazı; birbirini dışlayan ayarlarda
 // iki birincil düğme yerine bu kullanılır.
 [Test]
 public void Radio_ShowsExactlyOneFilledRing() {
  var host = Host();
  var chosen = KarineUI.Radio(host, "Anında", true, null);
  var other = KarineUI.Radio(host, "Normal", false, null);
  Assert.AreEqual(1, chosen[0].childCount, "Seçili radyonun içi dolu olmalı.");
  Assert.AreEqual(0, other[0].childCount);
  Assert.GreaterOrEqual(chosen.style.minHeight.value.value, KarineTheme.TouchTarget);
 }

 // Durum göstergesi: çubuk oranı kırpılır, sayı monospace kalır.
 [Test]
 public void Meter_ClampsAndStaysTechnical() {
  var host = Host();
  var meter = KarineUI.Meter(host, "gear", "KURUM GÜVENİ", 1.4f);
  var fill = meter.Query<VisualElement>().ToList().First(element =>
   element.style.width.value.unit == LengthUnit.Percent && element.style.width.value.value > 0);
  Assert.AreEqual(100f, fill.style.width.value.value, "Oran %100'ü aşamaz.");
  var counter = KarineUI.Counter(Host(), "folder", "TAMAMLANAN VAKA", "3 / 70");
  var number = counter.Query<Label>().ToList().Last();
  Assert.AreEqual("3 / 70", number.text);
 }

 // Diegetic katman ayrı: kâğıdın rengi HUD panelinin rengi değildir.
 [Test]
 public void PaperLayer_IsNotTheHudPalette() {
  Assert.AreNotEqual(KarineTheme.Panel, KarineTheme.Paper.Sheet);
  Assert.AreNotEqual(KarineTheme.Primary, KarineTheme.Paper.Ink);
 }
}
}
