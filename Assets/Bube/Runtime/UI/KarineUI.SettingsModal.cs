using System;
using UnityEngine;
using UnityEngine.UIElements;
using M = Bube.KarineTheme.SettingsModal;

namespace Bube {
// Ayarlar modalı, 3 Ekim 2026 maketi (Docs/Reference/UI_SETTINGS_2026-10.png):
// açıldığı ekranın üstünde durur, kapanınca o ekrana döner. Solda kategori
// sekmeleri, sağda satırlar (başlık + açıklama, sağda anahtar / kaydırıcı /
// seçim kartları), altta Varsayılana dön ve Kaydet.
public static partial class KarineUI {

 // Ortak modal çerçevesi: karartma, ortada panel, üstte ikon + başlık + alt
 // satır + kapatma, altında ayırıcı. Dönen panel içeriği alır.
 public static VisualElement ModalFrame(VisualElement root,string name,string panelName,string icon,string title,string subtitle,Action close) {
  root.Q(name)?.RemoveFromHierarchy();
  var veil=new VisualElement {name=name};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(M.VeilAlpha);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;
  // Arkadaki ekrana dokunuş geçmez.
  veil.RegisterCallback<PointerDownEvent>(e=>e.StopPropagation());
  root.Add(veil);
  // Altındaki ekran bazı parçalarını bir an sonra ekliyor; modal hep en üstte kalır.
  veil.schedule.Execute(()=>veil.BringToFront()).ExecuteLater(30);veil.schedule.Execute(()=>veil.BringToFront()).ExecuteLater(400);
  var panel=new VisualElement {name=panelName};close=KarineMotion.Leave(veil,panel,close);
  panel.style.width=Length.Percent(M.Width);panel.style.height=Length.Percent(M.Height);
  panel.style.backgroundColor=KarineTheme.Panel;Border(panel,KarineTheme.BorderWidth,KarineTheme.Border);Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=KarineTheme.SpaceLg;panel.style.paddingRight=KarineTheme.SpaceLg;panel.style.paddingTop=KarineTheme.SpaceMd;panel.style.paddingBottom=KarineTheme.SpaceMd;
  veil.Add(panel);KarineMotion.Paper(panel);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;header.style.alignItems=Align.Center;header.style.flexShrink=0;panel.Add(header);
  Icon(header,icon,KarineTheme.Primary,M.HeaderIcon).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement();words.style.flexGrow=1;header.Add(words);
  var t=Write(words,title.ToUpper(TextCulture),KarineTheme.Primary,M.TitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceMd;t.style.letterSpacing=1;
  var sub=Body_(words,subtitle,M.SubSize);sub.style.color=KarineTheme.Secondary;sub.style.marginBottom=0;
  var x=new Button(Sounded(close)) {name=name+"Close"};x.style.width=M.Close;x.style.height=M.Close;
  Unskin(x,Color.clear);Border(x,KarineTheme.BorderWidth,KarineTheme.Border);Round(x,KarineTheme.Radius);
  x.style.alignItems=Align.Center;x.style.justifyContent=Justify.Center;Icon(x,"close",KarineTheme.Primary,KarineTheme.IconSize);header.Add(x);
  ModalRule(panel);
  return panel;
 }
 public static void ModalRule(VisualElement parent){var rule=new VisualElement();rule.style.height=1;rule.style.flexShrink=0;rule.style.backgroundColor=KarineTheme.Border;rule.style.marginTop=KarineTheme.SpaceMd;rule.style.marginBottom=KarineTheme.SpaceMd;parent.Add(rule);}

 public static VisualElement SettingsModal(VisualElement root,string title,string subtitle,Action close,
                                           out VisualElement tabs,out VisualElement body,out VisualElement footer) {
  var panel=ModalFrame(root,"SettingsModal","SettingsPanel","gear",title,subtitle,close);
  var columns=new VisualElement();columns.style.flexDirection=FlexDirection.Row;columns.style.flexGrow=1;columns.style.minHeight=0;panel.Add(columns);
  tabs=new VisualElement {name="SettingsTabs"};tabs.style.width=M.TabWidth;tabs.style.flexShrink=0;
  tabs.style.borderRightWidth=1;tabs.style.borderRightColor=KarineTheme.Border;tabs.style.paddingRight=KarineTheme.SpaceMd;columns.Add(tabs);
  var scroll=new KarineScrollView {name="SettingsBody"};scroll.style.flexGrow=1;scroll.style.minHeight=0;scroll.style.paddingLeft=KarineTheme.SpaceXl;
  scroll.contentViewport.style.overflow=Overflow.Hidden;columns.Add(scroll);body=scroll.contentContainer;
  ModalRule(panel);
  footer=new VisualElement();footer.style.flexDirection=FlexDirection.Row;footer.style.justifyContent=Justify.SpaceBetween;footer.style.flexShrink=0;panel.Add(footer);
  return panel.parent;
 }

 public static Button SettingsTab(VisualElement parent,string icon,string title,string hint,bool selected,Action click) {
  var tab=new Button(Sounded(click)) {name="SettingsTab"};
  tab.style.flexDirection=FlexDirection.Row;tab.style.alignItems=Align.Center;tab.style.height=M.TabHeight;
  tab.style.marginLeft=0;tab.style.marginRight=0;tab.style.marginTop=0;tab.style.marginBottom=KarineTheme.SpaceXs;
  tab.style.paddingLeft=KarineTheme.SpaceLg;tab.style.paddingRight=KarineTheme.SpaceSm;
  Unskin(tab,selected?KarineTheme.Alpha(KarineTheme.Accent,.14f):Color.clear);Border(tab,0,Color.clear);
  tab.style.borderLeftWidth=4;tab.style.borderLeftColor=selected?KarineTheme.Accent:KarineTheme.Panel2;
  var ink=selected?KarineTheme.Accent:KarineTheme.Primary;
  Icon(tab,icon,ink,M.TabIcon).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexShrink=1;words.style.minWidth=0;tab.Add(words);
  var t=Write(words,title.ToUpper(TextCulture),ink,M.TabTitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceXs;t.style.letterSpacing=1;Left(t);
  if(!string.IsNullOrEmpty(hint)){var h=Body_(words,hint,M.TabHintSize);h.style.color=KarineTheme.Secondary;h.style.marginBottom=0;Left(h);}
  parent.Add(tab);return tab;
 }

 // Satır: solda başlık + açıklama; sağdaki denetim dönen kaba eklenir.
 // `stacked` ise denetim satırın altına, tam genişlikte gelir (seçim kartları).
 public static VisualElement SettingRow(VisualElement parent,string title,string hint,bool stacked=false) {
  var row=new VisualElement {name="SettingRow"};row.style.flexDirection=stacked?FlexDirection.Column:FlexDirection.Row;
  row.style.alignItems=stacked?Align.Stretch:Align.Center;row.style.paddingTop=M.RowGap;row.style.paddingBottom=M.RowGap;
  row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Border;parent.Add(row);
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;words.style.minWidth=0;row.Add(words);
  var t=Write(words,title.ToUpper(TextCulture),KarineTheme.Primary,M.RowTitleSize,Heading);t.style.marginBottom=0;t.style.letterSpacing=1;
  if(!string.IsNullOrEmpty(hint)){var h=Body_(words,hint,M.RowHintSize);h.style.color=KarineTheme.Secondary;h.style.marginBottom=0;}
  var control=new VisualElement {name="SettingControl"};control.style.flexShrink=0;
  if(stacked){control.style.flexDirection=FlexDirection.Row;control.style.flexWrap=Wrap.Wrap;control.style.marginTop=KarineTheme.SpaceSm;}
  else control.style.marginLeft=KarineTheme.SpaceLg;
  row.Add(control);return control;
 }

 // Anahtar: amber açık, gri kapalı; topuz sağda ya da solda.
 public static Button SettingSwitch(VisualElement parent,bool on,Action flip) {
  var track=new Button(Sounded(flip)) {name="SettingSwitch"};
  track.style.width=M.SwitchWidth;track.style.height=M.SwitchHeight;track.style.marginLeft=0;track.style.marginRight=0;
  track.style.paddingLeft=3;track.style.paddingRight=3;track.style.justifyContent=Justify.Center;
  track.style.alignItems=on?Align.FlexEnd:Align.FlexStart;
  Unskin(track,on?KarineTheme.Accent:KarineTheme.Panel2);Border(track,0,Color.clear);Round(track,M.SwitchHeight/2);
  var knob=new VisualElement {pickingMode=PickingMode.Ignore};int k=M.SwitchHeight-6;knob.style.width=k;knob.style.height=k;
  knob.style.backgroundColor=on?KarineTheme.Primary:KarineTheme.Secondary;Round(knob,k/2);track.Add(knob);
  parent.Add(track);return track;
 }

 // Seçim kartı: seçiliyse amber kenar ve sağ üstte onay dairesi.
 public static Button SettingCard(VisualElement parent,string icon,string title,string detail,bool selected,Action pick) {
  var card=new Button(Sounded(pick)) {name="SettingCard"};
  card.style.flexGrow=1;card.style.flexBasis=0;card.style.minWidth=0;card.style.minHeight=M.CardHeight;
  card.style.flexDirection=FlexDirection.Row;card.style.alignItems=Align.Center;
  card.style.marginLeft=0;card.style.marginTop=0;card.style.marginBottom=KarineTheme.SpaceXs;card.style.marginRight=KarineTheme.SpaceMd;
  card.style.paddingLeft=KarineTheme.SpaceMd;card.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(card,KarineTheme.Alpha(KarineTheme.Background,.6f));
  Border(card,selected?2:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Border);Round(card,KarineTheme.Radius);
  if(!string.IsNullOrEmpty(icon))Icon(card,icon,KarineTheme.Primary,KarineTheme.IconSize+6).style.marginRight=KarineTheme.SpaceMd;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;words.style.minWidth=0;card.Add(words);
  var t=Write(words,title.ToUpper(TextCulture),KarineTheme.Primary,M.CardTitleSize,Heading);t.style.marginBottom=0;t.style.letterSpacing=1;Left(t);
  if(!string.IsNullOrEmpty(detail)){var d=Body_(words,detail,M.RowHintSize);d.style.color=KarineTheme.Secondary;d.style.marginBottom=0;Left(d);}
  if(selected) {
   var mark=new VisualElement {pickingMode=PickingMode.Ignore};mark.style.width=26;mark.style.height=26;mark.style.flexShrink=0;
   mark.style.backgroundColor=KarineTheme.Accent;Round(mark,13);mark.style.alignItems=Align.Center;mark.style.justifyContent=Justify.Center;
   Icon(mark,"check",KarineTheme.OnPrimary,16);card.Add(mark);
  }
  parent.Add(card);return card;
 }

 // Kaydırıcı: amber dolu iz, krem topuz, sağda okunan değer. `steps` > 0 ise
 // değer o kadar eşit adıma oturur. Değer 0–1 arasıdır.
 public static VisualElement SettingSlider(VisualElement parent,float value,int steps,Func<float,string> readout,Action<float> change) {
  var box=new VisualElement {name="SettingSlider"};box.style.flexDirection=FlexDirection.Row;box.style.alignItems=Align.Center;parent.Add(box);
  var track=new VisualElement();track.style.width=M.SliderWidth;track.style.height=M.Knob;track.style.justifyContent=Justify.Center;box.Add(track);
  var rail=new VisualElement {pickingMode=PickingMode.Ignore};rail.style.height=M.TrackHeight;rail.style.backgroundColor=KarineTheme.Panel2;Round(rail,M.TrackHeight/2);track.Add(rail);
  var fill=new VisualElement {pickingMode=PickingMode.Ignore};fill.style.position=Position.Absolute;fill.style.left=0;fill.style.top=0;fill.style.bottom=0;
  fill.style.backgroundColor=KarineTheme.Accent;Round(fill,M.TrackHeight/2);rail.Add(fill);
  var knob=new VisualElement {pickingMode=PickingMode.Ignore};knob.style.position=Position.Absolute;knob.style.width=M.Knob;knob.style.height=M.Knob;
  knob.style.backgroundColor=KarineTheme.Primary;Round(knob,M.Knob/2);track.Add(knob);
  var label=Write(box,readout(value),KarineTheme.Primary,M.RowTitleSize,Heading);label.style.marginBottom=0;label.style.marginLeft=KarineTheme.SpaceMd;
  label.style.width=56;label.style.unityTextAlign=TextAnchor.MiddleRight;
  Action<float> show=v=>{fill.style.width=Length.Percent(v*100);knob.style.left=Length.Percent(v*100);knob.style.translate=new Translate(-M.Knob/2,0);label.text=readout(v);};
  show(value);
  bool dragging=false;
  Action<Vector2> set=p=> {
   float v=Mathf.Clamp01(p.x/Mathf.Max(1,track.contentRect.width));
   if(steps>0)v=Mathf.Round(v*steps)/steps;
   if(!Mathf.Approximately(v,value)){value=v;show(v);change(v);}
  };
  track.RegisterCallback<PointerDownEvent>(e=>{dragging=true;track.CapturePointer(e.pointerId);set(e.localPosition);});
  track.RegisterCallback<PointerMoveEvent>(e=>{if(dragging)set(e.localPosition);});
  track.RegisterCallback<PointerUpEvent>(e=>{dragging=false;track.ReleasePointer(e.pointerId);});
  return box;
 }

 // Dört kartlık satırlar ikişerli sarılır: "Lamba: yeşil banker" tek satıra sığmaz.
 // Kartın başında küçük renk yuvarlağı (lamba rengi gibi seçeneklerde).
 public static void Swatch(VisualElement card,Color color) {
  var dot=new VisualElement {name="SettingSwatch",pickingMode=PickingMode.Ignore};
  dot.style.width=dot.style.height=KarineTheme.IconSize;dot.style.flexShrink=0;dot.style.marginRight=KarineTheme.SpaceMd;
  dot.style.backgroundColor=color;Round(dot,(int)(KarineTheme.IconSize/2));Border(dot,KarineTheme.BorderWidth,KarineTheme.Border);
  card.Insert(0,dot);
 }
 public static void Quarter(VisualElement card){card.style.flexBasis=Length.Percent(46);card.style.flexGrow=1;}

 public static Button SettingsFooterButton(VisualElement parent,string icon,string title,string detail,bool primary,Action click) {
  var button=new Button(Sounded(click)) {name=primary?"SettingsSave":"SettingsReset"};
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.height=M.FooterHeight;
  button.style.minWidth=KarineTheme.Settings.ActionWidth+60;button.style.marginLeft=0;button.style.marginRight=0;
  button.style.paddingLeft=KarineTheme.SpaceXl;button.style.paddingRight=KarineTheme.SpaceXl;button.style.justifyContent=Justify.Center;
  Unskin(button,primary?KarineTheme.Accent:Color.clear);Border(button,KarineTheme.BorderWidth,primary?KarineTheme.Accent:KarineTheme.Border);Round(button,KarineTheme.Radius);
  var ink=primary?KarineTheme.OnPrimary:KarineTheme.Primary;
  Icon(button,icon,ink,KarineTheme.IconSize+6).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};button.Add(words);
  // Bebas'ın satır yüksekliği geniş: başlık ile açıklama bitişir, blok ikonla ortalanır.
  words.style.justifyContent=Justify.Center;
  var t=Write(words,title.ToUpper(TextCulture),ink,M.FooterTitleSize,Heading);t.style.letterSpacing=1;Left(t);
  // Tek satırlıksa (Geri gibi) negatif boşluk yazıyı yukarı kaydırır; yalnız açıklama varken bitiştirilir.
  t.style.marginBottom=string.IsNullOrEmpty(detail)?0:-KarineTheme.SpaceSm;
  if(!string.IsNullOrEmpty(detail)){var d=Body_(words,detail,M.TabHintSize);d.style.color=primary?KarineTheme.Paper.Ink:KarineTheme.Secondary;d.style.marginBottom=0;Left(d);}
  parent.Add(button);return button;
 }
}
}
