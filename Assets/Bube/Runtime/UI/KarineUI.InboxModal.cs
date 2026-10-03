using System;
using UnityEngine;
using UnityEngine.UIElements;
using I = Bube.KarineTheme.InboxModal;

namespace Bube {
// Gelen evrak modalı, 3 Ekim 2026 maketi (Docs/Reference/UI_INBOX_2026-10.png):
// solda sekmeli evrak listesi, sağda masa zemininde hafif eğik dosya kâğıdı,
// altta Geri ve evrakın kendi eylemi.
public static partial class KarineUI {
 static Font typewriter;
 // Kâğıt üstündeki daktilo yazısı; dosya yoksa gövde yazısına düşer.
 public static Font Typewriter=>typewriter=typewriter??Resources.Load<Font>("Bube/Fonts/IBMPlexMono-Regular");

 public static VisualElement InboxTabs(VisualElement parent,params (string label,bool selected,Action click)[] tabs) {
  var row=new VisualElement {name="InboxTabs"};row.style.flexDirection=FlexDirection.Row;row.style.flexShrink=0;
  row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Border;row.style.marginBottom=KarineTheme.SpaceMd;parent.Add(row);
  foreach(var tab in tabs) {
   var button=new Button(Sounded(tab.click)) {name="InboxTab"};button.style.flexGrow=1;button.style.flexBasis=0;button.style.height=I.TabHeight;
   button.style.marginLeft=0;button.style.marginRight=0;button.style.marginTop=0;button.style.marginBottom=0;
   Unskin(button,Color.clear);Border(button,0,Color.clear);
   button.style.borderBottomWidth=3;button.style.borderBottomColor=tab.selected?KarineTheme.Accent:Color.clear;
   var text=Write(button,tab.label.ToUpper(Tr),tab.selected?KarineTheme.Accent:KarineTheme.Secondary,I.TabSize,Heading);
   text.style.marginBottom=0;text.style.letterSpacing=1;text.style.unityTextAlign=TextAnchor.MiddleCenter;text.style.flexGrow=1;
   row.Add(button);
  }
  return row;
 }

 // Evrak satırı: seçiliyse amber sol kenar ve amber tonlu dolgu; yeni evrakta
 // amber nokta ve "YENİ" etiketi, değilse gri durum yazısı.
 public static Button InboxRow(VisualElement parent,string title,string status,bool fresh,string date,bool unread,bool selected,Action select) {
  var row=new Button(Sounded(select)) {name="InboxItem",tooltip=title};
  row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.minHeight=I.RowHeight;row.style.flexShrink=0;
  row.style.marginLeft=0;row.style.marginRight=0;row.style.marginTop=0;row.style.marginBottom=KarineTheme.SpaceSm;
  row.style.paddingLeft=KarineTheme.SpaceMd;row.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Accent,.16f):KarineTheme.Alpha(KarineTheme.Background,.5f));
  Border(row,KarineTheme.BorderWidth,selected?KarineTheme.Alpha(KarineTheme.Accent,.6f):KarineTheme.Border);Round(row,KarineTheme.Radius);
  row.style.borderLeftWidth=4;row.style.borderLeftColor=selected?KarineTheme.Accent:KarineTheme.Border;
  var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=10;dot.style.height=10;dot.style.flexShrink=0;Round(dot,5);
  dot.style.backgroundColor=unread?KarineTheme.Accent:Color.clear;dot.style.marginRight=KarineTheme.SpaceMd;row.Add(dot);
  Icon(row,"document",KarineTheme.Primary,KarineTheme.IconSize+6).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;words.style.minWidth=0;words.style.justifyContent=Justify.Center;row.Add(words);
  var t=Write(words,title.ToUpper(Tr),KarineTheme.Primary,I.RowTitleSize,Heading);t.style.marginBottom=0;t.style.letterSpacing=1;Left(t);
  // Başlık kesilmez, sığmazsa alt satıra iner; satır yüksekliği buna göre uzar.
  t.style.whiteSpace=WhiteSpace.Normal;
  var line=new VisualElement {pickingMode=PickingMode.Ignore};line.style.flexDirection=FlexDirection.Row;line.style.alignItems=Align.Center;words.Add(line);
  if(fresh) {
   var pill=new VisualElement {pickingMode=PickingMode.Ignore};pill.style.backgroundColor=KarineTheme.Accent;Round(pill,10);
   pill.style.paddingLeft=KarineTheme.SpaceSm;pill.style.paddingRight=KarineTheme.SpaceSm;line.Add(pill);
   var p=Write(pill,status.ToUpper(Tr),KarineTheme.OnPrimary,I.PillSize,Heading);p.style.marginBottom=0;p.style.letterSpacing=1;
  } else {
   var s=Body_(line,status,I.RowMetaSize);s.style.color=KarineTheme.Secondary;s.style.marginBottom=0;s.style.flexShrink=1;s.style.minWidth=0;
   s.style.whiteSpace=WhiteSpace.Normal;
  }
  var gap=new VisualElement();gap.style.flexGrow=1;line.Add(gap);
  if(!string.IsNullOrEmpty(date)){var d=Technical(line,date,I.RowMetaSize);d.style.color=KarineTheme.Secondary;d.style.marginBottom=0;d.style.flexShrink=0;}
  parent.Add(row);return row;
 }

 // Sağ taraf: masa görseli üstünde hafif eğik eskimiş kâğıt ve ataş. Dönen
 // eleman kâğıdın kaydırılabilir içidir.
 public static VisualElement InboxDesk(VisualElement parent,out VisualElement paper) {
  var desk=new VisualElement {name="InboxDesk"};desk.style.flexGrow=1;desk.style.minHeight=0;desk.style.overflow=Overflow.Hidden;
  desk.style.backgroundImage=new StyleBackground(Resources.Load<Texture2D>("Bube/UI/bg_office"));
  desk.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Cover));
  desk.style.paddingLeft=KarineTheme.SpaceMd;desk.style.paddingRight=KarineTheme.SpaceMd;desk.style.paddingTop=KarineTheme.SpaceLg;desk.style.paddingBottom=KarineTheme.SpaceSm;
  parent.Add(desk);
  var veil=new VisualElement {pickingMode=PickingMode.Ignore};veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(I.SceneVeil);desk.Add(veil);
  paper=new VisualElement {name="InboxPaper"};paper.style.flexGrow=1;paper.style.minHeight=0;paper.style.rotate=new Rotate(I.PaperTilt);
  paper.style.backgroundImage=new StyleBackground(Resources.Load<Texture2D>("Bube/UI/paper_sheet"));
  paper.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(Length.Percent(100),Length.Percent(100)));
  // Kâğıt görselinin kenarları yırtık: yazı yüzdeyle içeride tutulur, eğim yok (eğik yazı bulanıklaşıyordu).
  paper.style.paddingLeft=Length.Percent(I.PaperInsetX);paper.style.paddingRight=Length.Percent(I.PaperInsetX);
  paper.style.paddingTop=Length.Percent(I.PaperInsetY);paper.style.paddingBottom=Length.Percent(I.PaperInsetY);desk.Add(paper);
  var clip=Resources.Load<Texture2D>("Bube/UI/paperclip");
  if(clip!=null) {
   var pin=new Image {name="InboxClip",image=clip,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   pin.style.position=Position.Absolute;pin.style.width=KarineTheme.Inbox.ClipWidth;
   pin.style.height=KarineTheme.Inbox.ClipWidth*clip.height/(float)clip.width;pin.style.left=Length.Percent(8);pin.style.top=-KarineTheme.SpaceLg;paper.Add(pin);
  }
  return desk;
 }

 // Kâğıt başlığı: yuvarlak soyut kurum işareti ve birim adı, altında çift çizgi.
 public static void PaperLetterhead(VisualElement parent,string unit) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;parent.Add(row);
  var seal=new VisualElement {pickingMode=PickingMode.Ignore};seal.style.width=I.Seal;seal.style.height=I.Seal;Round(seal,I.Seal/2);
  Border(seal,3,KarineTheme.Paper.Ink);seal.style.alignItems=Align.Center;seal.style.justifyContent=Justify.Center;seal.style.marginRight=KarineTheme.SpaceMd;row.Add(seal);
  Icon(seal,"folder",KarineTheme.Paper.Ink,I.Seal/2);
  PaperText(row,unit.ToUpper(Tr),I.PaperSubSize).style.letterSpacing=2;
  for(int i=0;i<2;i++){var rule=new VisualElement();rule.style.height=i==0?3:1;rule.style.backgroundColor=KarineTheme.Paper.Ink;rule.style.marginTop=i==0?KarineTheme.SpaceSm:2;parent.Add(rule);}
 }
 public static Label PaperText(VisualElement parent,string text,int size) {
  var label=new Label(text) {pickingMode=PickingMode.Ignore};label.style.color=KarineTheme.Paper.Ink;label.style.fontSize=Typography.Snap(size);
  label.style.whiteSpace=WhiteSpace.Normal;label.style.marginBottom=0;ApplyFont(label,Typewriter??Fonts?.Body);parent.Add(label);return label;
 }
 // Künye: yan yana "ETİKET:" ve değer kutuları, aralarında ince çizgi.
 public static void PaperFields(VisualElement parent,params (string label,string value)[] fields) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginTop=KarineTheme.SpaceMd;
  row.style.borderTopWidth=1;row.style.borderBottomWidth=1;row.style.borderTopColor=KarineTheme.Paper.Ink;row.style.borderBottomColor=KarineTheme.Paper.Ink;
  row.style.paddingTop=KarineTheme.SpaceSm;row.style.paddingBottom=KarineTheme.SpaceSm;parent.Add(row);
  for(int i=0;i<fields.Length;i++) {
   var cell=new VisualElement();cell.style.flexGrow=1;cell.style.flexBasis=0;cell.style.paddingLeft=i==0?0:KarineTheme.SpaceLg;
   if(i>0){cell.style.borderLeftWidth=1;cell.style.borderLeftColor=KarineTheme.Paper.Ink;}
   PaperText(cell,fields[i].label.ToUpper(Tr)+":",I.PaperLabelSize);PaperText(cell,fields[i].value,I.PaperBodySize);row.Add(cell);
  }
 }
 // Eğik mürekkep damgası, kâğıdın sağ üst köşesinde.
 public static void PaperStamp(VisualElement paper,string text) {
  var stamp=new VisualElement {name="PaperStamp",pickingMode=PickingMode.Ignore};stamp.style.position=Position.Absolute;
  stamp.style.right=Length.Percent(6);stamp.style.top=Length.Percent(7);stamp.style.rotate=new Rotate(I.StampTilt);stamp.style.opacity=.82f;
  var ink=new Color(.68f,.12f,.1f);Border(stamp,4,ink);Round(stamp,4);
  stamp.style.paddingLeft=KarineTheme.SpaceMd;stamp.style.paddingRight=KarineTheme.SpaceMd;
  var t=Write(stamp,text.ToUpper(Tr),ink,I.StampSize,Heading);t.style.marginBottom=0;t.style.letterSpacing=2;paper.Add(stamp);
 }
}
}
