using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Evrak katmanı, sinematik, modal, bildirim, ilerleme ve tooltip.
// `KarineUI` tek bileşen kitaplığıdır; bu dosya onun bir parçasıdır.
public static partial class KarineUI {

 // Sinematik arka plan üzerindeki menü satırı. Aynı ikon, kenar ve tipografi
 // her menüde yeniden kullanılabilir; arka plan sanatından bağımsızdır.
 public static Button MenuAction(VisualElement parent,string icon,string label,
                                 Action onClick,bool primary) {
  var row=new Button(Sounded(onClick));
  row.name=primary?"MenuActionPrimary":"MenuAction";
  row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;
  row.style.width=Length.Percent(100);
  row.style.height=KarineTheme.MainMenu.RowHeight;
  row.style.minHeight=KarineTheme.TouchTarget;
  row.style.marginLeft=0;row.style.marginRight=0;
  row.style.marginTop=0;row.style.marginBottom=KarineTheme.MainMenu.RowGap;
  row.style.paddingLeft=KarineTheme.SpaceLg;
  row.style.paddingRight=KarineTheme.SpaceMd;
  var tone=primary?KarineTheme.OnPrimary:KarineTheme.Primary;
  row.style.backgroundColor=primary?KarineTheme.Primary:KarineTheme.Alpha(KarineTheme.Background,.88f);
  Border(row,KarineTheme.BorderWidth,primary?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Secondary,.45f));
  Round(row,KarineTheme.Radius);
  var mark=Icon(row,icon,tone,KarineTheme.IconSize);
  mark.style.marginRight=KarineTheme.SpaceLg;
  var text=new Label(label);
  text.style.color=tone;
  text.style.fontSize=Typography.Snap(KarineTheme.MainMenu.RowTextSize);
  text.style.flexGrow=1;
  text.style.unityTextAlign=TextAnchor.MiddleLeft;
  ApplyFont(text,primary?BodyBold:Body);
  row.Add(text);
  var next=Icon(row,"nav_next",tone,KarineTheme.IconSize);
  next.pickingMode=PickingMode.Ignore;
  var normal=primary?KarineTheme.Primary:KarineTheme.Alpha(KarineTheme.Background,.88f);
  var pressed=Color.Lerp(normal,KarineTheme.Accent,.28f);
  row.RegisterCallback<PointerDownEvent>(_=>row.style.backgroundColor=pressed);
  row.RegisterCallback<PointerUpEvent>(_=>row.style.backgroundColor=normal);
  parent?.Add(row);
  return row;
 }

 // Sol alttaki personel kartı. Karakter portresi ayrı sprite, metin ve şeritler
 // kodla çizilir; animasyonlu menü videosunun içine gömülmez.
 public static Button MenuIdentity(VisualElement parent,Texture2D portrait,
                                   string name,string role,string unit,string location,
                                   Action onClick) {
  var card=new Button(Sounded(onClick));
  card.name="MenuIdentity";
  card.style.flexDirection=FlexDirection.Row;
  card.style.alignItems=Align.Center;
  card.style.height=KarineTheme.MainMenu.IdentityHeight;
  card.style.minHeight=KarineTheme.TouchTarget;
  card.style.paddingLeft=KarineTheme.SpaceMd;
  card.style.paddingRight=KarineTheme.SpaceMd;
  card.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.90f);
  Border(card,KarineTheme.BorderWidth,KarineTheme.Alpha(KarineTheme.Secondary,.50f));
  Round(card,KarineTheme.Radius);
  var portraitFrame=new VisualElement();
  portraitFrame.style.width=KarineTheme.MainMenu.PortraitSize;
  portraitFrame.style.height=KarineTheme.MainMenu.PortraitSize;
  portraitFrame.style.flexShrink=0;
  portraitFrame.style.marginRight=KarineTheme.SpaceLg;
  portraitFrame.style.backgroundColor=KarineTheme.Panel2;
  card.Add(portraitFrame);
  if(portrait!=null) {
   var face=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   face.style.position=Position.Absolute;
   face.style.left=0;face.style.right=0;face.style.top=0;face.style.bottom=0;
   portraitFrame.Add(face);
  }
  var detail=new VisualElement();detail.style.flexGrow=1;card.Add(detail);
  var title=Body_(detail,name,19);
  title.style.marginBottom=KarineTheme.SpaceXs;
  ApplyFont(title,Heading);
  foreach(var line in new[]{role,unit,location}) {
   var item=Body_(detail,line,13);
   item.style.color=KarineTheme.Secondary;
   item.style.marginBottom=0;
  }
  parent?.Add(card);
  return card;
 }
 // --- Evrak katmanı --------------------------------------------------------

 // Kit §13: oyun dünyasının evrakı (dosya, rapor, arşiv, terminal çıktısı)
 // HUD'dan **ayrı** bir katmandır ve kendi kâğıt tonlarını taşır. O katmandaki
 // düğmeler de bileşendir; renkleri ekranların içine elle yazılmaz.
 //
 //   Action — kâğıdın üstündeki asıl eylem (gönder, devam et). Damga tonu.
 //   Choice — liste seçeneği, sekme, geri dönüş. Açık kâğıt tonu.
 //   Quiet  — ikincil, kâğıdın kendisiyle aynı tonda duran bağlantı.
 public static Button PaperButton(VisualElement parent, string label, Action onClick,
                                  KarinePaperKind kind = KarinePaperKind.Choice,
                                  bool leftAlign = false) {
  var button = new Button(Sounded(onClick)) { text = label };
  Color fill, ink;
  switch (kind) {
   case KarinePaperKind.Action: fill = KarineTheme.Paper.Stamp; ink = KarineTheme.Primary; break;
   case KarinePaperKind.Quiet:  fill = KarineTheme.Paper.Sheet; ink = KarineTheme.Paper.Ink; break;
   default:                     fill = KarineTheme.Paper.Tint;  ink = KarineTheme.Paper.Ink; break;
  }
  button.style.backgroundColor = fill;
  button.style.color = ink;
  button.style.minHeight = KarineTheme.TouchTarget;
  button.style.fontSize = Typography.Snap(20);
  button.style.whiteSpace = WhiteSpace.Normal;
  button.style.unityTextAlign = leftAlign ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
  if (leftAlign) button.style.paddingLeft = KarineTheme.SpaceMd;
  button.style.marginLeft = 0; button.style.marginRight = 0;
  button.style.marginTop = 0; button.style.marginBottom = 0;
  Round(button, KarineTheme.Radius);
  Border(button, KarineTheme.BorderWidth, KarineTheme.Paper.Edge);
  ApplyFont(button, kind == KarinePaperKind.Action ? BodyBold : Body);
  // Damga dokusu (yırtık kırmızı şerit) yazıyı okunmaz yapıyordu: düz kâğıt, ince kenar,
  // seçimlerde solda kırmızı çizgi; işlem düğmesi dolu kırmızı, açık yazı.
  button.style.paddingLeft = Mathf.Max(KarineTheme.SpaceLg, (int)button.style.paddingLeft.value.value);
  button.style.paddingRight = KarineTheme.SpaceLg;
  button.style.paddingTop = KarineTheme.SpaceSm; button.style.paddingBottom = KarineTheme.SpaceSm;
  button.style.marginBottom = KarineTheme.SpaceSm; button.style.flexShrink = 0;
  if (kind == KarinePaperKind.Choice) {
   fill = KarineTheme.Paper.Light; button.style.backgroundColor = fill;
   button.style.borderLeftWidth = KarineTheme.PrimaryEdgeWidth; button.style.borderLeftColor = KarineTheme.Paper.Stamp;
  }
  // Basınca o anki zemin saklanır, bırakınca geri gelir: sonradan rengi değiştirilen düğme
  // (dosya sekmeleri) parmak üstünden geçince başka renge dönmesin. Yalnız basılıyken çalışır.
  Color? before = null;
  button.RegisterCallback<PointerDownEvent>(_ => { before = button.style.backgroundColor.value; button.style.backgroundColor = Color.Lerp(before.Value, KarineTheme.Paper.Stamp, .30f); });
  EventCallback<EventBase> release = _ => { if (before.HasValue) { button.style.backgroundColor = before.Value; before = null; } };
  button.RegisterCallback<PointerUpEvent>(e => release(e)); button.RegisterCallback<PointerLeaveEvent>(e => release(e));
  button.RegisterCallback<PointerCancelEvent>(e => release(e));
  parent?.Add(button);
  return button;
 }

 // Kapatma her ekranda aynı olmalı: kare, dokunma hedefi kadar, aynı glif.
 // Kit "aynı işlev = aynı simge" der; bu yüzden tek yerde durur.
 public static Button CloseButton(VisualElement parent, Action onClick,
                                  string tooltip = null, bool paper = false) {
  var button = new Button(Sounded(onClick)) { text = "×" };
  button.style.width = KarineTheme.IconButtonSize;
  button.style.height = KarineTheme.IconButtonSize;
  button.style.flexShrink = 0;
  button.style.fontSize = Typography.Snap(24);
  button.style.unityTextAlign = TextAnchor.MiddleCenter;
  button.style.marginLeft = 0; button.style.marginRight = 0;
  button.style.marginTop = 0; button.style.marginBottom = 0;
  button.style.paddingLeft = 0; button.style.paddingRight = 0;
  button.style.backgroundColor = paper ? KarineTheme.Paper.Tint : KarineTheme.Panel;
  button.style.color = paper ? KarineTheme.Paper.Ink : KarineTheme.Primary;
  Round(button, KarineTheme.Radius);
  Border(button, KarineTheme.BorderWidth, paper ? KarineTheme.Paper.Edge : KarineTheme.Panel2);
  ApplyFont(button, Body);
  if (!string.IsNullOrEmpty(tooltip)) button.tooltip = tooltip;
  parent?.Add(button);
  return button;
 }

 // --- Sinematik ---------------------------------------------------------

 // Sinematikte tek bir denetim vardır: GEÇ. Duraklatma, ilerleme çubuğu ve
 // hızlandırma **yoktur** — film ya izlenir ya geçilir; ara kademeler oyuncuya
 // karar verdirmez, yalnız kareyi kalabalıklaştırır. Kare ilerletme ve
 // duraklatma CCTV izlemede anlamlıdır ve orada kendi denetimleri vardır.
 //
 // Düğme kit'in **birincil** eylemidir: sinematikte tek eylem odur. Kit'in
 // birincil dili birebir uygulanır — dolu krem zemin, koyu yazı, sol eylem
 // kenarı. Zemin **saydam değildir**: filmin üstünde bile düğme düğme gibi
 // durur. Metin düğmenin kendi `text`i değil, ayrı bir etikettir — UI
 // Toolkit'te bir `Button`un metni ile çocukları **üst üste biner**, simge
 // ancak böyle yanına oturur.
 public static Button SkipButton(VisualElement parent, string label, Action onClick) {
  var button = new Button(Sounded(onClick));
  button.text = null;
  button.style.flexDirection = FlexDirection.Row;
  button.style.alignItems = Align.Center;
  button.style.justifyContent = Justify.Center;
  button.style.minHeight = KarineTheme.TouchTargetComfortable;
  button.style.paddingLeft = KarineTheme.SpaceLg; button.style.paddingRight = KarineTheme.SpaceLg;
  button.style.paddingTop = KarineTheme.SpaceSm; button.style.paddingBottom = KarineTheme.SpaceSm;
  button.style.marginLeft = 0; button.style.marginRight = 0;
  button.style.marginTop = 0; button.style.marginBottom = 0;
  var fill = KarineTheme.Primary;
  button.style.backgroundColor = fill;
  Round(button, KarineTheme.Radius);
  Border(button, KarineTheme.BorderWidth, KarineTheme.Accent);
  button.style.borderLeftWidth = KarineTheme.PrimaryEdgeWidth;
  button.style.borderLeftColor = KarineTheme.Accent;

  var text = new Label(label);
  text.style.color = KarineTheme.OnPrimary;
  text.style.fontSize = Typography.Snap(19);
  text.style.letterSpacing = 2;
  text.style.marginBottom = 0; text.style.marginRight = KarineTheme.SpaceSm;
  ApplyFont(text, Body);
  button.Add(text);
  button.Add(Icon(null, "cine_skip", KarineTheme.OnPrimary));

  var pressed = Color.Lerp(fill, KarineTheme.Accent, .35f);
  // Basınca o anki zemin saklanır, bırakınca geri gelir: sonradan rengi değiştirilen düğme
  // (dosya sekmeleri) parmak üstünden geçince başka renge dönmesin. Yalnız basılıyken çalışır.
  Color? before = null;
  button.RegisterCallback<PointerDownEvent>(_ => { before = button.style.backgroundColor.value; button.style.backgroundColor = Color.Lerp(before.Value, KarineTheme.Paper.Stamp, .30f); });
  EventCallback<EventBase> release = _ => { if (before.HasValue) { button.style.backgroundColor = before.Value; before = null; } };
  button.RegisterCallback<PointerUpEvent>(e => release(e)); button.RegisterCallback<PointerLeaveEvent>(e => release(e));
  button.RegisterCallback<PointerCancelEvent>(e => release(e));
  parent?.Add(button);
  return button;
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
  veil.style.backgroundColor = KarineTheme.Veil(.72f);
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
}
}
