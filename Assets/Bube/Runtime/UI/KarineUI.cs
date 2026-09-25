using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {

// Kit'teki düğme hiyerarşisi. Ekranlar bunun dışında bir düğme biçimi üretmez.
public enum KarineButtonKind { Primary, Secondary, Ghost, Danger }

// Rozet/durum göstergesi tonu: kit'te kırmızı yalnız "yeni/kritik", teal
// "sistem/aktif", krem "nötr".
public enum KarineTone { Neutral, Active, Danger }

// KARINE UI/UX Kit'in ortak bileşenleri. Yeni ekran yazarken önce buraya
// bakılır; buradaki bir bileşen işi görüyorsa yenisi yazılmaz.
//
// Prefab yok (proje arayüzü kodla kurar), bu yüzden "prefab" burada
// `VisualElement` döndüren yeniden kullanılabilir üretici demektir.
public static class KarineUI {

 // Yazı tipi rolleri bir kez kurulur; her bileşen buradan okur. Kurulmamışsa
 // bileşenler yine çizilir, yalnız panelin varsayılan yazı tipiyle.
 public static FontSet Fonts;

 static void ApplyFont(VisualElement element, Font font) {
  if (font != null) element.style.unityFontDefinition = FontDefinition.FromFont(font);
 }

 static Font Heading => Fonts != null ? Fonts.Heading : null;
 static Font Body => Fonts != null ? Fonts.Body : null;
 static Font Mono => Fonts != null ? Fonts.Mono : null;

 // --- Yazı -----------------------------------------------------------------

 public static Label Title(VisualElement parent, string value, int size = 28) =>
  Write(parent, value, KarineTheme.Primary, size, Heading);

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
 public static Button Button_(VisualElement parent, string label, Action onClick,
                              KarineButtonKind kind = KarineButtonKind.Secondary, bool enabled = true) {
  var button = new Button(onClick) { text = label };
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
  var button = new Button(onClick);
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

 // --- Sekme ----------------------------------------------------------------

 public static VisualElement Tabs(VisualElement parent, string[] labels, int selected, Action<int> onSelect,
                                 bool stretch = false) {
  var strip = Row(parent);
  strip.style.backgroundColor = KarineTheme.Panel;
  strip.style.marginBottom = KarineTheme.SpaceMd;
  Border(strip, KarineTheme.BorderWidth, KarineTheme.Panel2);
  for (int index = 0; index < labels.Length; index++) {
   int captured = index;
   var tab = new Button(() => onSelect?.Invoke(captured)) { text = labels[index] };
   tab.style.minHeight = KarineTheme.TouchTarget;
   tab.style.fontSize = Typography.Snap(15);
   if (stretch) { tab.style.flexGrow = 1; tab.style.whiteSpace = WhiteSpace.Normal; }
   tab.style.marginLeft = 0; tab.style.marginRight = 0;
   tab.style.marginTop = 0; tab.style.marginBottom = 0;
   Border(tab, 0, Color.clear);
   Round(tab, KarineTheme.Radius);
   bool active = index == selected;
   tab.style.backgroundColor = active ? KarineTheme.Primary : Color.clear;
   tab.style.color = active ? KarineTheme.OnPrimary : KarineTheme.Secondary;
   ApplyFont(tab, Body);
   strip.Add(tab);
  }
  return strip;
 }

 // --- Seçim: radyo ve anahtar ----------------------------------------------

 // Kit'teki SEÇİM RADYO: dolu halka seçili, boş halka değil. Birbirini dışlayan
 // ayarlar için; iki düğmeyi birden birincil yapmak yerine bu kullanılır.
 public static Button Radio(VisualElement parent, string label, bool selected, Action onSelect) {
  var row = new Button(onSelect);
  row.style.flexDirection = FlexDirection.Row;
  row.style.alignItems = Align.Center;
  row.style.minHeight = KarineTheme.TouchTarget;
  row.style.backgroundColor = Color.clear;
  row.style.marginLeft = 0; row.style.marginRight = 0;
  row.style.marginTop = 0; row.style.marginBottom = KarineTheme.SpaceXs;
  row.style.paddingLeft = 0;
  row.style.unityTextAlign = TextAnchor.MiddleLeft;
  Border(row, 0, Color.clear);
  parent?.Add(row);

  var ring = new VisualElement();
  ring.style.width = 20; ring.style.height = 20; ring.style.flexShrink = 0;
  ring.style.alignItems = Align.Center; ring.style.justifyContent = Justify.Center;
  ring.style.marginRight = KarineTheme.SpaceMd;
  Border(ring, KarineTheme.BorderWidth, selected ? KarineTheme.Primary : KarineTheme.Muted);
  Round(ring, 10);
  row.Add(ring);
  if (selected) {
   var core = new VisualElement();
   core.style.width = 10; core.style.height = 10;
   core.style.backgroundColor = KarineTheme.Primary;
   Round(core, 5);
   ring.Add(core);
  }

  var text = new Label(label);
  text.style.color = selected ? KarineTheme.Primary : KarineTheme.Secondary;
  text.style.fontSize = Typography.Snap(17);
  ApplyFont(text, Body);
  row.Add(text);
  return row;
 }

 // --- Durum göstergesi -----------------------------------------------------

 // Kit'in "DURUM GÖSTERGELERİ" kutusu: ikon + etiket + çubuk (Kurum Güveni)
 // ya da ikon + etiket + sayı (Tamamlanan Vaka 3 / 70). Sayı monospace'tir.
 public static VisualElement Meter(VisualElement parent, string icon, string label, float ratio) {
  var card = Panel(parent, true);
  card.style.flexDirection = FlexDirection.Row;
  card.style.alignItems = Align.Center;
  Icon(card, icon, KarineTheme.Primary).style.marginRight = KarineTheme.SpaceMd;
  var column = new VisualElement();
  column.style.flexGrow = 1;
  card.Add(column);
  var name = Technical(column, label, 13);
  name.style.marginBottom = KarineTheme.SpaceXs;
  Progress(column, ratio);
  return card;
 }

 public static VisualElement Counter(VisualElement parent, string icon, string label, string value) {
  var card = Panel(parent, true);
  card.style.flexDirection = FlexDirection.Row;
  card.style.alignItems = Align.Center;
  Icon(card, icon, KarineTheme.Primary).style.marginRight = KarineTheme.SpaceMd;
  var column = new VisualElement();
  column.style.flexGrow = 1;
  card.Add(column);
  Technical(column, label, 13).style.marginBottom = KarineTheme.SpaceXs;
  var number = Technical(column, value, 19);
  number.style.color = KarineTheme.Primary;
  number.style.marginBottom = 0;
  return card;
 }

 // --- Evrak gezintisi ------------------------------------------------------

 // `<  03 / 07  >` — sayfa sayacı monospace, oklar ikon düğme.
 public static VisualElement DocumentNav(VisualElement parent, int index, int count,
                                         Action onPrev, Action onNext) {
  var nav = Row(parent);
  nav.style.justifyContent = Justify.Center;
  var back = IconButton(nav, "nav_prev", onPrev);
  back.SetEnabled(index > 1);
  var counter = Technical(nav, index.ToString("00") + " / " + count.ToString("00"), 21);
  counter.style.marginBottom = 0;
  counter.style.marginLeft = KarineTheme.SpaceLg;
  counter.style.marginRight = KarineTheme.SpaceLg;
  counter.style.color = KarineTheme.Primary;
  var next = IconButton(nav, "nav_next", onNext);
  next.SetEnabled(index < count);
  next.style.marginRight = 0;
  return nav;
 }

 // --- Sinematik kontroller -------------------------------------------------

 // Kit §7: PAUSE / PROGRESS / TIME / İLERİ SAR / GEÇ. Bütün oyunda aynı çubuk.
 // İLERİ SAR ve GEÇ **ayrı** eylemlerdir: biri sahneyi hızlandırır, öteki atlar.
 // Sinematik sırasında UI en azda kalsın diye çubuk saydam koyu bir şerittir.
 public static VisualElement CinematicControls(
   VisualElement parent, Func<bool> playing, Action togglePlay,
   Action fastForward, string fastForwardLabel,
   Action skip, string skipLabel,
   Func<float> progress, Func<string> time) {
  var bar = Row(parent);
  bar.style.backgroundColor = new Color(0, 0, 0, .55f);
  bar.style.paddingLeft = KarineTheme.SpaceMd; bar.style.paddingRight = KarineTheme.SpaceMd;
  bar.style.paddingTop = KarineTheme.SpaceSm; bar.style.paddingBottom = KarineTheme.SpaceSm;
  Border(bar, KarineTheme.BorderWidth, KarineTheme.Panel2);

  var play = IconButton(bar, "cine_pause", togglePlay);
  var track = Progress(bar, 0f);
  track.style.flexGrow = 1;
  track.style.marginLeft = KarineTheme.SpaceMd; track.style.marginRight = KarineTheme.SpaceMd;
  var clock = Technical(bar, "00:00 / 00:00", 15);
  clock.style.marginBottom = 0; clock.style.marginRight = KarineTheme.SpaceMd;

  if (fastForward != null) {
   var forward = Button_(bar, fastForwardLabel, fastForward, KarineButtonKind.Secondary);
   forward.style.minHeight = KarineTheme.TouchTarget;
   forward.style.marginBottom = 0;
   forward.Insert(0, Icon(null, "cine_forward", KarineTheme.Primary));
  }
  if (skip != null) {
   var skipButton = Button_(bar, skipLabel, skip, KarineButtonKind.Secondary);
   skipButton.style.minHeight = KarineTheme.TouchTarget;
   skipButton.style.marginBottom = 0; skipButton.style.marginRight = 0;
   skipButton.Insert(0, Icon(null, "cine_skip", KarineTheme.Primary));
  }

  // Dört kez saniyede yenilemek yeter: zaman yazısı saniye çözünürlüğünde.
  bar.schedule.Execute(() => {
   var fill = track.childCount > 0 ? track[0] : null;
   if (fill != null) fill.style.width = Length.Percent(Mathf.Clamp01(progress()) * 100f);
   clock.text = time();
   var glyph = play.childCount > 0 ? play[0] : null;
   if (glyph != null) {
    var art = Resources.Load<Texture2D>("Bube/Art/Icons/" + (playing() ? "cine_pause" : "nav_next"));
    if (art != null) glyph.style.backgroundImage = new StyleBackground(art);
   }
  }).Every(250);
  return bar;
 }

 // "00:12 / 01:24" — kit'in zaman biçimi.
 public static string Clock(double seconds, double total) =>
  Stamp(seconds) + " / " + Stamp(total);

 static string Stamp(double seconds) {
  if (double.IsNaN(seconds) || double.IsInfinity(seconds) || seconds < 0) seconds = 0;
  int whole = (int)seconds;
  return (whole / 60).ToString("00") + ":" + (whole % 60).ToString("00");
 }

 // --- Modal ----------------------------------------------------------------

 // Başlık → açıklama → ikincil eylem → birincil eylem. Kit'in sırası budur;
 // onay düğmesi hep sağda ve tek birincil eylemdir.
 public static VisualElement Modal(VisualElement parent, string title, string explanation,
                                   string cancelLabel, Action onCancel,
                                   string confirmLabel, Action onConfirm,
                                   bool destructive = false) {
  var veil = new VisualElement();
  veil.style.position = Position.Absolute;
  veil.style.left = 0; veil.style.top = 0; veil.style.right = 0; veil.style.bottom = 0;
  veil.style.backgroundColor = new Color(0, 0, 0, .72f);
  veil.style.alignItems = Align.Center;
  veil.style.justifyContent = Justify.Center;
  parent?.Add(veil);

  var card = Panel(veil, true);
  card.style.width = Length.Percent(56);
  card.style.maxWidth = 640;
  Border(card, KarineTheme.BorderWidth, destructive ? KarineTheme.Danger : KarineTheme.Accent);

  var head = Row(card);
  if (destructive) Icon(head, "alert", KarineTheme.Danger).style.marginRight = KarineTheme.SpaceMd;
  Title(head, title, 21).style.marginBottom = 0;
  Body_(card, explanation, 15).style.color = KarineTheme.Muted;

  var actions = Row(card);
  actions.style.justifyContent = Justify.FlexEnd;
  Button_(actions, cancelLabel, onCancel, KarineButtonKind.Secondary);
  var confirm = Button_(actions, confirmLabel, onConfirm,
   destructive ? KarineButtonKind.Danger : KarineButtonKind.Primary);
  confirm.style.marginRight = 0;

  Enter(veil, KarineTheme.ModalMs);
  return veil;
 }

 // --- Bildirim -------------------------------------------------------------

 // "YENİ EVRAK VAR / Masana yeni bir dosya gönderildi. [GÖRÜNTÜLE]" — ekranın
 // küçük bir köşesi; tam ekran kaplamaz.
 public static VisualElement Notification(VisualElement parent, string title, string detail,
                                          string actionLabel, Action onAction, Action onDismiss) {
  var card = Panel(parent, true);
  card.style.flexDirection = FlexDirection.Row;
  card.style.maxWidth = 420;
  Border(card, KarineTheme.BorderWidth, KarineTheme.Accent);

  Icon(card, "document", KarineTheme.Primary, 34).style.marginRight = KarineTheme.SpaceMd;

  var column = new VisualElement();
  column.style.flexGrow = 1;
  card.Add(column);
  Title(column, title, 17).style.marginBottom = KarineTheme.SpaceXs;
  Body_(column, detail, 13).style.color = KarineTheme.Muted;
  if (!string.IsNullOrEmpty(actionLabel)) {
   var action = Button_(column, actionLabel, onAction, KarineButtonKind.Primary);
   action.style.minHeight = KarineTheme.TouchTarget;
   action.style.alignSelf = Align.FlexStart;
  }
  if (onDismiss != null) {
   var close = IconButton(card, "close", onDismiss);
   close.style.width = KarineTheme.TouchTarget; close.style.height = KarineTheme.TouchTarget;
   close.style.marginRight = 0;
  }
  Enter(card, KarineTheme.NoticeMs);
  return card;
 }

 // --- Yükleme ve ilerleme --------------------------------------------------

 public static VisualElement Progress(VisualElement parent, float ratio, KarineTone tone = KarineTone.Active) {
  var track = new VisualElement();
  track.style.height = 6;
  track.style.backgroundColor = KarineTheme.Panel2;
  Round(track, KarineTheme.Radius);
  var fill = new VisualElement();
  fill.style.height = 6;
  fill.style.width = Length.Percent(Mathf.Clamp01(ratio) * 100f);
  fill.style.backgroundColor = Tone(tone);
  Round(fill, KarineTheme.Radius);
  track.Add(fill);
  parent?.Add(track);
  return track;
 }

 // --- Tooltip --------------------------------------------------------------

 public static VisualElement Tooltip(VisualElement parent, string text) {
  var tip = Panel(parent, true);
  tip.style.flexDirection = FlexDirection.Row;
  tip.style.maxWidth = 380;
  tip.style.paddingTop = KarineTheme.SpaceMd; tip.style.paddingBottom = KarineTheme.SpaceMd;
  Icon(tip, "info", KarineTheme.Active).style.marginRight = KarineTheme.SpaceMd;
  Body_(tip, text, 13).style.marginBottom = 0;
  return tip;
 }

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
