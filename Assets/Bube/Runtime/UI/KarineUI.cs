using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {

// Kit'teki düğme hiyerarşisi. Ekranlar bunun dışında bir düğme biçimi üretmez.
// Scroll bars never occupy the game's visual surfaces. Unity's native touch,
// wheel and programmatic scrolling remain available in both directions.
public sealed class KarineScrollView : ScrollView {
 public KarineScrollView(ScrollViewMode mode = ScrollViewMode.Vertical) : base(mode) {
  horizontalScrollerVisibility = ScrollerVisibility.Hidden;
  verticalScrollerVisibility = ScrollerVisibility.Hidden;
 }
}

// Unscaled UI motion; each owner cancels pending work when detached.
public static class KarineMotion {
 public static bool Reduced => PlayerPrefs.GetInt("karine.reducedMotion",0)==1;
 public static void Run(VisualElement owner,float seconds,Action<float> update,Action complete=null) {
  if(Reduced){update(1);complete?.Invoke();return;}
  float start=Time.realtimeSinceStartup;
  IVisualElementScheduledItem task=null;
  EventCallback<DetachFromPanelEvent> detach=null;
  detach=evt=>{task?.Pause();owner.UnregisterCallback(detach);};
  owner.RegisterCallback(detach);
  update(0);
  task=owner.schedule.Execute(()=>{
   float t=Mathf.Clamp01((Time.realtimeSinceStartup-start)/seconds);
   update(t*t*(3-2*t));
   if(t>=1){task.Pause();owner.UnregisterCallback(detach);complete?.Invoke();}
  }).Every(KarineTheme.Motion.TickMs);
 }
 public static void Page(VisualElement paper) {
  Run(paper,KarineTheme.Motion.PageSeconds,t=>{
   paper.style.translate=new Translate(KarineTheme.Motion.PageOffset*(1-t),0);
   paper.style.opacity=.8f+.2f*t;
  });
 }
 public static void Paper(VisualElement paper) {
  Run(paper,KarineTheme.Motion.PaperSeconds,t=>{
   paper.style.translate=new Translate(0,KarineTheme.Motion.PaperOffset*(1-t));
   paper.style.opacity=.65f+.35f*t;
  });
 }
 public static void InstallPressFeedback(VisualElement root) {
  Button pressed=null;
  Action release=()=>{if(pressed!=null)pressed.style.scale=new Scale(Vector3.one);pressed=null;};
  root.RegisterCallback<PointerDownEvent>(evt=>{
   release();if(Reduced)return;
   var target=evt.target as VisualElement;
   while(target!=null && !(target is Button))target=target.parent;
   var button=target as Button;if(button==null||!button.enabledInHierarchy)return;
   pressed=button;button.style.scale=new Scale(Vector3.one*KarineTheme.Motion.PressScale);
   button.schedule.Execute(()=>{button.style.scale=new Scale(Vector3.one);}).StartingIn(KarineTheme.Motion.ReleaseMs);
  },TrickleDown.TrickleDown);
  root.RegisterCallback<PointerUpEvent>(_=>release(),TrickleDown.TrickleDown);
  root.RegisterCallback<PointerCancelEvent>(_=>release(),TrickleDown.TrickleDown);
  root.RegisterCallback<PointerLeaveEvent>(_=>release());
 }
}

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

 public static VisualElement SettingsShell(VisualElement root,out VisualElement navigation,out VisualElement body) {
  var veil=new VisualElement();OfficePlace(veil,new Rect(0,0,100,100));veil.style.backgroundColor=KarineTheme.Veil(.65f);root.Add(veil);
  var frame=Panel(root,true);frame.name="SettingsPanel";frame.style.position=Position.Absolute;
  bool narrow=Screen.height>=Screen.width;
  frame.style.left=Length.Percent(narrow?KarineTheme.Settings.NarrowInset:KarineTheme.Settings.Inset);
  frame.style.right=frame.style.left;
  frame.style.top=Length.Percent(KarineTheme.Settings.Top);frame.style.bottom=Length.Percent(KarineTheme.Settings.Bottom);
  frame.style.flexDirection=narrow?FlexDirection.Column:FlexDirection.Row;
  frame.style.backgroundColor=KarineTheme.Background;Border(frame,2,KarineTheme.Accent);
  navigation=new VisualElement();navigation.style.width=narrow?StyleKeyword.Auto:new StyleLength(KarineTheme.Settings.SideWidth);
  navigation.style.flexShrink=0;navigation.style.flexDirection=narrow?FlexDirection.Row:FlexDirection.Column;
  navigation.style.marginRight=KarineTheme.SpaceXl;frame.Add(navigation);
  body=new VisualElement();body.style.flexGrow=1;body.style.minWidth=0;body.style.minHeight=0;frame.Add(body);return frame;
 }
 public static Button SettingsChoice(VisualElement parent,string title,string detail,bool selected,Action pick,string icon=null) {
  var button=Button_(parent,"",pick,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.minHeight=KarineTheme.Settings.ChoiceHeight;button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;
  button.style.flexGrow=1;button.style.flexBasis=0;button.style.minWidth=0;
  var color=selected?KarineTheme.OnPrimary:KarineTheme.Primary;
  if(icon!=null)Icon(button,icon,color,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var copy=new VisualElement();copy.style.flexGrow=1;copy.style.minWidth=0;button.Add(copy);
  var heading=Body_(copy,title,KarineTheme.Settings.TextSize);heading.style.color=color;heading.style.unityTextAlign=TextAnchor.MiddleLeft;heading.style.marginBottom=KarineTheme.SpaceXs;
  if(!string.IsNullOrEmpty(detail)){var hint=Body_(copy,detail,KarineTheme.CaseBrowser.SmallSize);hint.style.unityTextAlign=TextAnchor.MiddleLeft;hint.style.color=selected?KarineTheme.Paper.Ink:KarineTheme.Secondary;hint.style.marginBottom=0;}
  return button;
 }

 // --- Yazı -----------------------------------------------------------------

 // Başlık boyutu rolleri korunur; her iki rol Chakra Petch Bold kullanır.
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
  button.style.color = text;
  // Yeni görünüm (2 Ekim 2026): düğmeler dokulu, gerdirilebilir görseldir —
  // koyu deri/pirinç, seçili/ana için krem kâğıt/pirinç. Ghost düz kalır.
  if (kind != KarineButtonKind.Ghost && Skin(button, kind == KarineButtonKind.Primary && enabled ? "btn_primary" : "btn_dark")) {
   if (!enabled) button.style.unityBackgroundImageTintColor = KarineTheme.Button.DisabledTint;
   if (enabled) Pressable(button);
   return;
  }
  button.style.backgroundColor = fill;
  Border(button, kind == KarineButtonKind.Ghost ? 0 : KarineTheme.BorderWidth, edge);
  if (!enabled) return;
  var normal = fill;
  var pressed = Color.Lerp(fill, KarineTheme.Accent, .25f);
  button.RegisterCallback<PointerDownEvent>(_ => button.style.backgroundColor = pressed);
  button.RegisterCallback<PointerUpEvent>(_ => button.style.backgroundColor = normal);
  button.RegisterCallback<PointerLeaveEvent>(_ => button.style.backgroundColor = normal);
 }

 // Düğme görselini dokuz parçalı gerer: köşeler ve pirinç kenar sabit, orta uzar.
 public static bool Skin(VisualElement button, string name) {
  var art = Resources.Load<Texture2D>("Bube/UI/" + name);
  if (art == null) return false;
  button.style.backgroundColor = Color.clear;
  Border(button, 0, Color.clear);
  button.style.backgroundImage = new StyleBackground(art);
  int slice = KarineTheme.Button.Slice;
  button.style.unitySliceLeft = slice; button.style.unitySliceRight = slice;
  button.style.unitySliceTop = slice; button.style.unitySliceBottom = slice;
  button.style.unitySliceScale = KarineTheme.Button.SliceScale;
  button.style.unityBackgroundImageTintColor = Color.white;
  return true;
 }

 // Basma geri bildirimi: görsel bir an kararır ve bir piksel içe iner. Her düğmede aynı.
 public static void Pressable(VisualElement button) {
  button.RegisterCallback<PointerDownEvent>(_ => { button.style.unityBackgroundImageTintColor = KarineTheme.Button.PressedTint; button.style.translate = new Translate(0, KarineTheme.Button.PressDepth); }, TrickleDown.TrickleDown);
  EventCallback<EventBase> release = _ => { button.style.unityBackgroundImageTintColor = Color.white; button.style.translate = new Translate(0, 0); };
  button.RegisterCallback<PointerUpEvent>(e => release(e));
  button.RegisterCallback<PointerLeaveEvent>(e => release(e));
 }

 // Yalnız ikon: kare koyu düğme + krem çizgi ikon. İkon 22 px, hedef 48 px.
 public static Button IconButton(VisualElement parent, string icon, Action onClick, string tooltip = null,
                                string soundId = AudioDirector.Press) {
  var button = new Button(Sounded(onClick, soundId));
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
