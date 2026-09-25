using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {

// Kit'teki düğme hiyerarşisi. Ekranlar bunun dışında bir düğme biçimi üretmez.
public enum KarineButtonKind { Primary, Secondary, Ghost, Danger }

// Rozet/durum göstergesi tonu: kit'te kırmızı yalnız "yeni/kritik", teal
// "sistem/aktif", krem "nötr".
public enum KarineTone { Neutral, Active, Danger }
public enum KarinePaperKind { Action, Choice, Quiet }

// KARINE UI/UX Kit'in ortak bileşenleri. Yeni ekran yazarken önce buraya
// bakılır; buradaki bir bileşen işi görüyorsa yenisi yazılmaz.
//
// Prefab yok (proje arayüzü kodla kurar), bu yüzden "prefab" burada
// `VisualElement` döndüren yeniden kullanılabilir üretici demektir.
public static partial class KarineUI {

 // Yazı tipi rolleri bir kez kurulur; her bileşen buradan okur. Kurulmamışsa
 // bileşenler yine çizilir, yalnız panelin varsayılan yazı tipiyle.
 public static FontSet Fonts;

 static void ApplyFont(VisualElement element, Font font) {
  if (font != null) element.style.unityFontDefinition = FontDefinition.FromFont(font);
 }

 static Font Display => Fonts != null ? Fonts.Display : null;
 static Font Heading => Fonts != null ? Fonts.Heading : null;
 static Font Body => Fonts != null ? Fonts.Body : null;
 static Font BodyBold => Fonts != null ? Fonts.BodyBold : null;
 static Font Mono => Fonts != null ? Fonts.Mono : null;

 // --- Yazı -----------------------------------------------------------------

 // Büyük başlıklar logonun diline yakın ağır slab'ı kullanır; küçük başlıklar
 // Roboto Slab'da kalır. Eşik `DisplayFrom`: ahşap dizgi küçük puntoda
 // okunmaz, kit'in okunabilirlik kuralı orada ağır basar.
 public const int DisplayFrom = 28;
 public static Label Title(VisualElement parent, string value, int size = 28) =>
  Write(parent, value, KarineTheme.Primary, size, size >= DisplayFrom ? Display : Heading);

 public static Label Subtitle(VisualElement parent, string value, int size = 19) =>
  Write(parent, value, KarineTheme.Secondary, size, Heading);

 public static Label Body_(VisualElement parent, string value, int size = 17) =>
  Write(parent, value, KarineTheme.Primary, size, Body);

 // Teknik metin: dosya numarası, tarih/saat, kurum güveni, CCTV zaman damgası.
 // Kit bunları monospace ister; oyuncunun "kayıt" olarak okuduğu her sayı buraya girer.
 public static Label Technical(VisualElement parent, string value, int size = 15) =>
  Write(parent, value, KarineTheme.Secondary, size, Mono);

 static Label Write(VisualElement parent, string value, Color color, int size, Font font) {
  var label = new Label(value);
  label.style.whiteSpace = WhiteSpace.Normal;
  label.style.color = color;
  label.style.fontSize = Typography.Snap(size);
  label.style.marginBottom = KarineTheme.SpaceMd;
  ApplyFont(label, font);
  parent?.Add(label);
  return label;
 }

 // --- Yüzeyler -------------------------------------------------------------

 public static VisualElement Panel(VisualElement parent, bool raised = false) {
  var panel = new VisualElement();
  panel.style.backgroundColor = raised ? KarineTheme.Panel2 : KarineTheme.Panel;
  Border(panel, KarineTheme.BorderWidth, KarineTheme.Panel2);
  Round(panel, KarineTheme.Radius);
  panel.style.paddingLeft = KarineTheme.SpaceXl;
  panel.style.paddingRight = KarineTheme.SpaceXl;
  panel.style.paddingTop = KarineTheme.SpaceLg;
  panel.style.paddingBottom = KarineTheme.SpaceLg;
  panel.style.marginBottom = KarineTheme.SpaceMd;
  parent?.Add(panel);
  return panel;
 }

 public static VisualElement Row(VisualElement parent, Align align = Align.Center) {
  var row = new VisualElement();
  row.style.flexDirection = FlexDirection.Row;
  row.style.alignItems = align;
  parent?.Add(row);
  return row;
 }

 public static VisualElement Rule(VisualElement parent, int width = 0) {
  var rule = new VisualElement();
  rule.style.height = 1;
  if (width > 0) rule.style.width = width; else rule.style.width = Length.Percent(100);
  rule.style.backgroundColor = KarineTheme.Accent;
  rule.style.marginTop = KarineTheme.SpaceSm;
  rule.style.marginBottom = KarineTheme.SpaceSm;
  parent?.Add(rule);
  return rule;
 }

 // --- Düğmeler -------------------------------------------------------------

 // Kit'in dört biçimi. Başka biçim yok; "sadece bu ekranda farklı görünsün"
 // isteği buraya yeni bir `kind` olarak gelir, ekranın içine değil.
 // Kit'in her düğmesi basıldığında buradan haber verir; sesi `AudioDirector`
 // çalar. Ses kit'in içine gömülmez, çünkü `KarineUI` saf arayüzdür ve
 // testlerde AudioSource olmadan kurulur. Kimse dinlemiyorsa sessizdir.
 public static Action<string> Sound;

 // Basma sesini eyleme ekler. Düğme kurucularının hepsi bundan geçer, böylece
 // yeni bir ekran ses eklemeyi unutamaz.
 public static Action Sounded(Action onClick, string id = AudioDirector.Press) =>
  () => { Sound?.Invoke(id); onClick?.Invoke(); };

 public static Button Button_(VisualElement parent, string label, Action onClick,
                              KarineButtonKind kind = KarineButtonKind.Secondary, bool enabled = true) {
  var button = new Button(Sounded(onClick)) { text = label };
  button.style.minHeight = KarineTheme.TouchTargetComfortable;
  button.style.fontSize = Typography.Snap(19);
  button.style.unityTextAlign = TextAnchor.MiddleCenter;
  button.style.paddingLeft = KarineTheme.SpaceLg;
  button.style.paddingRight = KarineTheme.SpaceLg;
  button.style.marginLeft = 0; button.style.marginRight = KarineTheme.SpaceSm;
  button.style.marginTop = 0; button.style.marginBottom = KarineTheme.SpaceSm;
  Round(button, KarineTheme.Radius);
  ApplyFont(button, Body);
  Paint(button, kind, enabled);
  parent?.Add(button);
  return button;
 }

 // Durumlar tek yerde: NORMAL / PRESSED / DISABLED. Basma geri bildirimi
 // renk kaymasıdır — ölçek/zıplama yok.
 public static void Paint(Button button, KarineButtonKind kind, bool enabled) {
  button.SetEnabled(enabled);
  Color fill, text, edge;
  switch (kind) {
   case KarineButtonKind.Primary:
    fill = KarineTheme.Primary; text = KarineTheme.OnPrimary; edge = KarineTheme.Accent; break;
   case KarineButtonKind.Danger:
    fill = KarineTheme.Background; text = KarineTheme.Danger; edge = KarineTheme.Danger; break;
   case KarineButtonKind.Ghost:
    fill = Color.clear; text = KarineTheme.Secondary; edge = Color.clear; break;
   default:
    fill = KarineTheme.Background; text = KarineTheme.Primary; edge = KarineTheme.Primary; break;
  }
  if (!enabled) { fill = KarineTheme.Disabled; text = KarineTheme.Panel; edge = KarineTheme.Disabled; }
  button.style.backgroundColor = fill;
  button.style.color = text;
  Border(button, kind == KarineButtonKind.Ghost ? 0 : KarineTheme.BorderWidth, edge);
  if (kind == KarineButtonKind.Primary && enabled) {
   button.style.borderLeftWidth = KarineTheme.PrimaryEdgeWidth;
   button.style.borderLeftColor = KarineTheme.Accent;
  }
  if (!enabled) return;
  var normal = fill;
  var pressed = Color.Lerp(fill, KarineTheme.Accent, kind == KarineButtonKind.Primary ? .35f : .25f);
  button.RegisterCallback<PointerDownEvent>(_ => button.style.backgroundColor = pressed);
  button.RegisterCallback<PointerUpEvent>(_ => button.style.backgroundColor = normal);
  button.RegisterCallback<PointerLeaveEvent>(_ => button.style.backgroundColor = normal);
 }

 // Yalnız ikon: kare koyu düğme + krem çizgi ikon. İkon 22 px, hedef 48 px.
 public static Button IconButton(VisualElement parent, string icon, Action onClick, string tooltip = null) {
  var button = new Button(Sounded(onClick));
  button.style.width = KarineTheme.IconButtonSize;
  button.style.height = KarineTheme.IconButtonSize;
  button.style.marginRight = KarineTheme.SpaceSm;
  button.style.marginBottom = 0; button.style.marginTop = 0; button.style.marginLeft = 0;
  button.style.paddingLeft = 0; button.style.paddingRight = 0;
  button.style.backgroundColor = KarineTheme.Background;
  button.style.alignItems = Align.Center;
  button.style.justifyContent = Justify.Center;
  Border(button, KarineTheme.BorderWidth, KarineTheme.Primary);
  Round(button, KarineTheme.Radius);
  if (!string.IsNullOrEmpty(tooltip)) button.tooltip = tooltip;
  button.Add(Icon(null, icon, KarineTheme.Primary));
  parent?.Add(button);
  return button;
 }

 // --- İkon -----------------------------------------------------------------

 // Aynı işlev her ekranda aynı ikon: `Bube/Art/Icons/<name>`. Dosya yoksa alan
 // korunur, ekran kaymaz.
 public static VisualElement Icon(VisualElement parent, string name, Color tint, int size = 0) {
  var icon = new VisualElement();
  if (size <= 0) size = KarineTheme.IconSize;
  icon.style.width = size; icon.style.height = size;
  icon.style.flexShrink = 0;
  var art = Resources.Load<Texture2D>("Bube/Art/Icons/" + name);
  if (art != null) {
   icon.style.backgroundImage = new StyleBackground(art);
   icon.style.backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain));
   icon.style.unityBackgroundImageTintColor = tint;
  }
  parent?.Add(icon);
  return icon;
 }

 // --- Rozet ve durum -------------------------------------------------------

 public static Label Badge(VisualElement parent, string text, KarineTone tone = KarineTone.Danger) {
  var badge = new Label(text);
  badge.style.fontSize = Typography.Snap(13);
  badge.style.color = tone == KarineTone.Neutral ? KarineTheme.OnPrimary : KarineTheme.Background;
  badge.style.backgroundColor = Tone(tone);
  badge.style.paddingLeft = KarineTheme.SpaceSm; badge.style.paddingRight = KarineTheme.SpaceSm;
  badge.style.paddingTop = KarineTheme.SpaceXs; badge.style.paddingBottom = KarineTheme.SpaceXs;
  badge.style.unityTextAlign = TextAnchor.MiddleCenter;
  Round(badge, KarineTheme.Radius);
  ApplyFont(badge, Mono);
  parent?.Add(badge);
  return badge;
 }

 public static VisualElement Dot(VisualElement parent, KarineTone tone) {
  var dot = new VisualElement();
  dot.style.width = 10; dot.style.height = 10;
  dot.style.backgroundColor = Tone(tone);
  Round(dot, 5);
  parent?.Add(dot);
  return dot;
 }

 public static Color Tone(KarineTone tone) =>
  tone == KarineTone.Danger ? KarineTheme.Danger :
  tone == KarineTone.Active ? KarineTheme.Active : KarineTheme.Primary;

 // --- Ortak biçim yardımcıları ---------------------------------------------

 public static void Border(VisualElement element, int width, Color color) {
  element.style.borderTopWidth = width; element.style.borderBottomWidth = width;
  element.style.borderLeftWidth = width; element.style.borderRightWidth = width;
  element.style.borderTopColor = color; element.style.borderBottomColor = color;
  element.style.borderLeftColor = color; element.style.borderRightColor = color;
 }

 public static void Round(VisualElement element, int radius) {
  element.style.borderTopLeftRadius = radius; element.style.borderTopRightRadius = radius;
  element.style.borderBottomLeftRadius = radius; element.style.borderBottomRightRadius = radius;
 }

 // Giriş devinimi: yalnız sönümlü belirme. Zıplama, esneme, ölçek patlaması yok.
 public static void Enter(VisualElement element, float seconds) {
  element.style.opacity = 0f;
  element.schedule.Execute(() => {
   element.style.transitionProperty = new StyleList<StylePropertyName>(
    new System.Collections.Generic.List<StylePropertyName> { new StylePropertyName("opacity") });
   element.style.transitionDuration = new StyleList<TimeValue>(
    new System.Collections.Generic.List<TimeValue> { new TimeValue(seconds, TimeUnit.Second) });
   element.style.opacity = 1f;
  }).StartingIn(16);
 }
}
}
