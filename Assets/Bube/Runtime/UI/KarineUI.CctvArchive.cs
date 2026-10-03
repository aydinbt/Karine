using System;
using UnityEngine;
using UnityEngine.UIElements;
using A = Bube.KarineTheme.CctvArchive;
namespace Bube {
// Güvenlik kamerası arşivi (3 Ekim 2026 maketleri, Docs/Reference/UI_CCTV_2026-10.png ve UI_CCTV_PLAYER_2026-10.png):
// solda koyu kamera listesi, sağda monitör; dökümde saat, metin, küçük kare ve eylem sütunu.
public static partial class KarineUI {
 static Color CctvGreen=>A.Signal;
 public static VisualElement CctvCameras(VisualElement parent,string title) {
  var panel=new VisualElement {name="CctvCameras"};OfficePlace(panel,A.Cameras);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Border);Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=KarineTheme.SpaceSm;panel.style.paddingRight=KarineTheme.SpaceSm;panel.style.paddingTop=KarineTheme.SpaceMd;panel.style.paddingBottom=KarineTheme.SpaceSm;parent.Add(panel);
  var t=Write(panel,title.ToUpper(Tr),KarineTheme.Secondary,A.ListTitleSize,Heading);t.style.marginLeft=KarineTheme.SpaceXs;t.style.marginBottom=KarineTheme.SpaceSm;
  var list=new KarineScrollView(ScrollViewMode.Vertical);list.style.flexGrow=1;panel.Add(list);return list;
 }
 // Kamera kartı: kamera ikonu, ad, yer ve aralık. Amber nokta yalnız "henüz incelenmedi" demektir.
 public static Button CctvCameraItem(VisualElement list,string title,string place,string period,bool selected,bool unread,Action click) {
  var button=new Button(Sounded(click)) {name="CctvCameraItem",tooltip=title};button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;
  button.style.minHeight=A.CameraRow-20;button.style.marginLeft=0;button.style.marginRight=0;button.style.marginBottom=KarineTheme.SpaceSm;
  button.style.paddingLeft=KarineTheme.SpaceMd;button.style.paddingRight=KarineTheme.SpaceSm;
  Unskin(button,selected?KarineTheme.Alpha(KarineTheme.Accent,.12f):KarineTheme.Alpha(KarineTheme.Background,.7f));
  Border(button,selected?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(button,KarineTheme.Radius);
  Icon(button,A.CameraIcon,selected?KarineTheme.Accent:KarineTheme.Secondary,KarineTheme.IconSize+KarineTheme.SpaceSm).style.marginRight=KarineTheme.SpaceMd;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;button.Add(words);
  var name=Write(words,title.ToUpper(Tr),KarineTheme.Primary,A.TextSize-1,Heading);name.style.marginBottom=0;
  foreach(var line in new[]{place,period}){if(string.IsNullOrEmpty(line))continue;Write(words,line,KarineTheme.Secondary,A.MetaSize-1,Typewriter).style.marginBottom=0;}
  if(unread){var dot=Dot(button,KarineTheme.Accent);dot.style.alignSelf=Align.FlexStart;dot.style.marginTop=KarineTheme.SpaceSm;}
  list.Add(button);return button;
 }
 // Monitör: koyu çerçeve ve içinde döküm sütunu. Görüntü izleyicisi çerçevenin üstüne açılır.
 public static VisualElement CctvMonitor(VisualElement parent) {
  var frame=new VisualElement {name="CctvMonitor"};OfficePlace(frame,A.Monitor);
  frame.style.backgroundColor=new Color(.04f,.075f,.08f,.98f);Border(frame,KarineTheme.BorderWidth+1,KarineTheme.Border);Round(frame,KarineTheme.Radius);parent.Add(frame);
  var content=new VisualElement {name="CctvArchiveRecords"};content.style.flexGrow=1;content.style.minHeight=0;
  content.style.paddingLeft=KarineTheme.SpaceLg;content.style.paddingRight=KarineTheme.SpaceLg;content.style.paddingTop=KarineTheme.SpaceMd;content.style.paddingBottom=KarineTheme.SpaceMd;
  frame.Add(content);return content;
 }
 // "KAMERA 01 · BİNA GİRİŞİ   ● REC   ● SİNYAL İYİ" ve altında yeşil aralık satırı. Dönen etiket sinyal durumudur.
 public static Label CctvMonitorHead(VisualElement content,string title,string period,string signal,string rec) {
  var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.Center;head.style.flexShrink=0;content.Add(head);
  var t=Write(head,title.ToUpper(Tr),KarineTheme.Primary,A.MonitorTitleSize,Heading);t.style.marginBottom=0;t.style.flexGrow=1;t.style.flexShrink=1;
  var recRow=new VisualElement {pickingMode=PickingMode.Ignore};recRow.style.flexDirection=FlexDirection.Row;recRow.style.alignItems=Align.Center;recRow.style.marginRight=KarineTheme.SpaceXl;head.Add(recRow);
  Dot(recRow,KarineTheme.Danger).style.marginRight=KarineTheme.SpaceSm;Write(recRow,rec.ToUpper(Tr),KarineTheme.Danger,A.StatusSize,Heading).style.marginBottom=0;
  var sigRow=new VisualElement {pickingMode=PickingMode.Ignore};sigRow.style.flexDirection=FlexDirection.Row;sigRow.style.alignItems=Align.Center;head.Add(sigRow);
  Dot(sigRow,CctvGreen).style.marginRight=KarineTheme.SpaceSm;
  var s=Write(sigRow,signal.TrimStart('●',' ').ToUpper(Tr),CctvGreen,A.StatusSize,Heading);s.style.marginBottom=0;
  if(!string.IsNullOrEmpty(period)){var p=Write(content,period.ToUpper(Tr),CctvGreen,A.MetaSize,Typewriter);p.style.marginBottom=KarineTheme.SpaceMd;}
  return s;
 }
 public static VisualElement CctvRecordPanel(VisualElement parent) {
  var panel=new VisualElement {name="CctvRecordPanel"};panel.style.flexGrow=1;panel.style.minHeight=0;parent.Add(panel);return panel;
 }
 // Döküm satırı: koyu şerit; sinyal satırı kırmızıya çalar. Sütunlar çağıran tarafından eklenir.
 public static VisualElement CctvRecordRow(VisualElement parent) {
  var row=new VisualElement {name="CctvRecordRow"};row.style.display=DisplayStyle.None;row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;row.style.minHeight=A.RowHeight-6;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceMd;row.style.paddingRight=KarineTheme.SpaceXs;
  row.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Panel,.55f);Border(row,1,KarineTheme.Alpha(KarineTheme.Border,.5f));Round(row,KarineTheme.Radius);
  parent.Add(row);return row;
 }
 public static void CctvRowAlert(VisualElement row) {
  row.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Danger,.18f);Border(row,1,KarineTheme.Alpha(KarineTheme.Danger,.5f));
 }
 // Küçük kare: kaydın ilk karesi; yoksa tarama çizgili koyu karo.
 public static void CctvThumb(VisualElement row,Texture2D still,bool show) {
  var tile=new VisualElement {pickingMode=PickingMode.Ignore};tile.style.width=A.ThumbWidth;tile.style.height=A.ThumbHeight;tile.style.flexShrink=0;tile.style.marginLeft=KarineTheme.SpaceMd;
  row.Add(tile);if(!show)return;
  tile.style.backgroundColor=new Color(.09f,.12f,.12f);Border(tile,1,KarineTheme.Alpha(KarineTheme.Secondary,.4f));
  if(still!=null){var img=new Image {image=still,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};img.style.flexGrow=1;tile.Add(img);}
  else for(int i=1;i<6;i++){var l=new VisualElement {pickingMode=PickingMode.Ignore};l.style.position=Position.Absolute;l.style.left=0;l.style.right=0;l.style.top=Length.Percent(i*16);l.style.height=1;l.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Secondary,.18f);tile.Add(l);}
 }
 public static VisualElement CctvActionSlot(VisualElement row) {
  var slot=new VisualElement();slot.style.width=A.ActionWidth;slot.style.flexShrink=0;slot.style.alignItems=Align.Center;slot.style.justifyContent=Justify.Center;slot.style.marginLeft=KarineTheme.SpaceMd;
  row.Add(slot);return slot;
 }
 // Satırın eylem düğmesi (İZLE, YENİDEN ÇÖZÜMLE): koyu zemin; `strong` olanı amber çerçeveli.
 public static Button CctvRowButton(VisualElement slot,string icon,string label,bool strong,Action action) {
  var b=new Button(Sounded(action)) {tooltip=label};b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;b.style.justifyContent=Justify.Center;
  b.style.alignSelf=Align.Stretch;b.style.height=A.ThumbHeight+4;b.style.marginLeft=0;b.style.marginRight=0;b.style.marginBottom=0;
  Unskin(b,KarineTheme.Alpha(KarineTheme.GlassDeep,.97f));Border(b,strong?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,strong?KarineTheme.Accent:KarineTheme.Border);Round(b,KarineTheme.Radius);
  Icon(b,icon,KarineTheme.Accent,A.LineSize).style.marginRight=KarineTheme.SpaceSm;
  Write(b,label.ToUpper(Tr),KarineTheme.Accent,A.LineSize,Heading).style.marginBottom=0;
  slot.Add(b);return b;
 }
 // Monitörün altındaki şerit: önce "Kayıtları incele", tararken bekleme yazısı, bitince amber tik.
 public static VisualElement CctvFooter(VisualElement content) {
  var bar=new VisualElement {name="CctvFooter"};bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;bar.style.flexShrink=0;
  bar.style.minHeight=A.FooterHeight;bar.style.marginTop=KarineTheme.SpaceSm;bar.style.paddingLeft=KarineTheme.SpaceMd;bar.style.paddingRight=KarineTheme.SpaceMd;
  Border(bar,KarineTheme.BorderWidth,KarineTheme.Border);Round(bar,KarineTheme.Radius);content.Add(bar);return bar;
 }
 public static void CctvFooterDone(VisualElement bar,string done,string total) {
  bar.Clear();Border(bar,KarineTheme.BorderWidth+1,KarineTheme.Accent);bar.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Accent,.08f);
  Icon(bar,"check",KarineTheme.Accent,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(bar,done.ToUpper(Tr),KarineTheme.Accent,A.FooterSize,Heading);l.style.marginBottom=0;l.style.flexGrow=1;
  Write(bar,total.ToUpper(Tr),KarineTheme.Secondary,A.MetaSize-1,Heading).style.marginBottom=0;
 }
 public static Label CctvFooterText(VisualElement bar,string text) {
  bar.Clear();var l=Write(bar,text.ToUpper(Tr),KarineTheme.Accent,A.FooterSize,Heading);l.style.marginBottom=0;return l;
 }
 // Oynatıcı denetimi: koyu kutu, ikon ve etiket; `primary` amber dolu (OYNAT/DURAKLAT).
 public static Button CctvControl(VisualElement bar,string icon,string label,bool primary,Action action) {
  var b=new Button(Sounded(action)) {name="CctvControl",tooltip=label};b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;b.style.justifyContent=Justify.Center;
  b.style.height=A.ControlHeight;b.style.flexShrink=0;b.style.marginLeft=0;b.style.marginRight=KarineTheme.SpaceSm;b.style.marginBottom=0;
  b.style.paddingLeft=KarineTheme.SpaceMd;b.style.paddingRight=KarineTheme.SpaceMd;
  var ink=primary?KarineTheme.OnPrimary:KarineTheme.Primary;
  Unskin(b,primary?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.GlassDeep,.97f));Border(b,KarineTheme.BorderWidth,primary?KarineTheme.Accent:KarineTheme.Border);Round(b,KarineTheme.Radius);
  if(!string.IsNullOrEmpty(icon))Icon(b,icon,ink,KarineTheme.IconSize).style.marginRight=string.IsNullOrEmpty(label)?0:KarineTheme.SpaceSm;
  if(!string.IsNullOrEmpty(label)){var l=Write(b,label.ToUpper(Tr),ink,A.ControlSize,Heading);l.style.marginBottom=0;l.name="CctvControlLabel";}
  bar.Add(b);return b;
 }
 public static void CctvControlText(Button b,string label) { var l=b.Q<Label>("CctvControlLabel");if(l!=null)l.text=label.ToUpper(Tr); }
 // İnce ilerleme çizgisi: amber dolgu ve topuz. Yalnız gösterir; sarmak kare düğmeleriyle yapılır.
 public static VisualElement CctvProgress(VisualElement bar,out VisualElement fill) {
  var track=new VisualElement {name="CctvProgress",pickingMode=PickingMode.Ignore};track.style.flexGrow=1;track.style.height=A.ProgressHeight;track.style.marginLeft=KarineTheme.SpaceMd;track.style.marginRight=KarineTheme.SpaceMd;
  track.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Secondary,.35f);Round(track,A.ProgressHeight);bar.Add(track);
  fill=new VisualElement {pickingMode=PickingMode.Ignore};fill.style.height=A.ProgressHeight;fill.style.width=Length.Percent(0);fill.style.backgroundColor=KarineTheme.Accent;Round(fill,A.ProgressHeight);track.Add(fill);
  var knob=new VisualElement {pickingMode=PickingMode.Ignore};knob.style.position=Position.Absolute;knob.style.right=-A.KnobSize/2;knob.style.top=(A.ProgressHeight-A.KnobSize)/2;
  knob.style.width=A.KnobSize;knob.style.height=A.KnobSize;knob.style.backgroundColor=KarineTheme.Accent;Round(knob,A.KnobSize/2);fill.Add(knob);
  return track;
 }
 // İzlenen kaydın döküm satırı, oynatıcının altında: belge ikonu, saat, metin.
 public static void CctvCaption(VisualElement parent,string time,string text) {
  var row=new VisualElement {name="CctvCaption"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.flexShrink=0;row.style.minHeight=A.FooterHeight;
  row.style.marginTop=KarineTheme.SpaceSm;row.style.paddingLeft=KarineTheme.SpaceMd;row.style.paddingRight=KarineTheme.SpaceMd;
  row.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Panel,.55f);Border(row,1,KarineTheme.Alpha(KarineTheme.Border,.5f));Round(row,KarineTheme.Radius);parent.Add(row);
  Icon(row,"document",KarineTheme.Secondary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceLg;
  if(!string.IsNullOrEmpty(time)){var t=Write(row,time,KarineTheme.Primary,A.CaptionSize,Typewriter);t.style.marginBottom=0;t.style.width=A.TimeWidth/2+KarineTheme.SpaceXl;}
  var b=Write(row,text,KarineTheme.Primary,A.CaptionSize,Typewriter);b.style.marginBottom=0;b.style.flexShrink=1;
 }
 // Döküm satırını saat ve metin olarak ayırır: "08.27 — metin" ya da ayrı saat anahtarı.
 public static void SplitCctvLine(string text,string time,out string stamp,out string body) {
  stamp=time??string.Empty;body=text??string.Empty;
  if(stamp.Length>0){if(body.StartsWith(stamp+" — ",StringComparison.Ordinal))body=body.Substring(stamp.Length+3);return;}
  int dash=body.IndexOf(" — ",StringComparison.Ordinal);
  if(dash>0 && dash<=13){stamp=body.Substring(0,dash);body=body.Substring(dash+3);}
 }
}
}
