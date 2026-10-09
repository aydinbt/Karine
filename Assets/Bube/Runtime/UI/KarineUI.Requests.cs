using System;
using UnityEngine;
using UnityEngine.UIElements;
using Q = Bube.KarineTheme.Requests;
namespace Bube {
public enum RequestTone { Ready, Waiting, Done, Open, Gone }
public static partial class KarineUI {
 // Talepler ekranı (3 Ekim 2026 maketleri, Docs/Reference/UI_REQUESTS*_2026-10.png): solda koyu panel ve
 // iki sekme, sağda ataşlı kâğıt. Tam ekran; masadaki tablet çerçevesi kaldırıldı.
 public static VisualElement RequestPanel(VisualElement parent,out VisualElement tabs) {
  var panel=new VisualElement {name="RequestPanel"};OfficePlace(panel,Q.Panel);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.96f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Border);Round(panel,KarineTheme.Radius);
  parent.Add(panel);
  tabs=new VisualElement();tabs.style.flexDirection=FlexDirection.Row;tabs.style.flexShrink=0;tabs.style.height=Q.TabHeight;
  tabs.style.borderBottomWidth=1;tabs.style.borderBottomColor=KarineTheme.Border;panel.Add(tabs);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;panel.Add(scroll);
  var c=scroll.contentContainer;c.style.paddingLeft=KarineTheme.SpaceSm;c.style.paddingRight=KarineTheme.SpaceSm;c.style.paddingTop=KarineTheme.SpaceSm;
  return c;
 }
 public static void RequestTab(VisualElement tabs,string icon,string label,int count,bool active,Action action) {
  var tab=new Button(Sounded(action)) {name="RequestTab",tooltip=label};Unskin(tab,active?KarineTheme.Alpha(KarineTheme.Accent,.10f):Color.clear);
  tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.flexDirection=FlexDirection.Row;tab.style.alignItems=Align.Center;tab.style.justifyContent=Justify.Center;
  tab.style.marginLeft=0;tab.style.marginRight=0;tab.style.borderBottomWidth=3;tab.style.borderBottomColor=active?KarineTheme.Accent:Color.clear;tabs.Add(tab);
  Icon(tab,icon,active?KarineTheme.Accent:KarineTheme.Secondary,Q.TabIcon).style.marginRight=KarineTheme.SpaceSm;
  Write(tab,label.ToUpper(TextCulture),active?KarineTheme.Primary:KarineTheme.Secondary,Q.TabSize,Heading).style.marginBottom=0;
  if(count<=0)return;
  var badge=new VisualElement {pickingMode=PickingMode.Ignore};badge.style.width=Q.Badge;badge.style.height=Q.Badge;badge.style.marginLeft=KarineTheme.SpaceSm;
  badge.style.alignItems=Align.Center;badge.style.justifyContent=Justify.Center;badge.style.backgroundColor=KarineTheme.Accent;Round(badge,Q.Badge/2);tab.Add(badge);
  Write(badge,count.ToString(),KarineTheme.OnPrimary,Q.TabSize-2,Heading).style.marginBottom=0;
 }
 // Liste satırı: kare portre ya da ikon kutusu, ad, bilgi, son söz; sağda durum etiketi. Seçili satır amber çerçeveli.
 public static Button RequestRow(VisualElement list,Texture2D portrait,string icon,string title,string info,string quote,bool selected,bool fresh,string status,RequestTone tone,Action action) {
  var row=new Button(Sounded(action)) {name="RequestRow",tooltip=title};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=Q.RowHeight;row.style.marginLeft=0;row.style.marginRight=0;row.style.marginBottom=KarineTheme.SpaceSm;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceMd;row.style.paddingTop=KarineTheme.SpaceXs;row.style.paddingBottom=KarineTheme.SpaceXs;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Accent,.08f):KarineTheme.Alpha(KarineTheme.Background,.7f));
  Border(row,selected?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(row,KarineTheme.Radius);list.Add(row);
  var box=new VisualElement {pickingMode=PickingMode.Ignore};box.style.width=Q.Portrait;box.style.height=Q.Portrait;box.style.flexShrink=0;box.style.marginRight=KarineTheme.SpaceMd;
  box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;box.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.9f);
  Border(box,KarineTheme.BorderWidth,KarineTheme.Border);Round(box,KarineTheme.Radius);row.Add(box);
  if(portrait!=null) {
   var face=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};face.style.width=Length.Percent(100);face.style.height=Length.Percent(100);
   if(tone==RequestTone.Gone)face.tintColor=KarineTheme.Alpha(KarineTheme.Secondary,.6f);box.Add(face);
  } else Icon(box,icon??"document",tone==RequestTone.Done?KarineTheme.Secondary:KarineTheme.Accent,Q.Portrait/2);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;words.style.alignItems=Align.FlexStart;row.Add(words);
  var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.Center;words.Add(head);
  var t=Write(head,title,KarineTheme.Primary,Q.NameSize,Typewriter);t.style.marginBottom=0;t.style.flexShrink=1;t.style.unityFontStyleAndWeight=FontStyle.Bold;
  if(fresh){var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=Q.Dot;dot.style.height=Q.Dot;dot.style.marginLeft=KarineTheme.SpaceMd;dot.style.backgroundColor=KarineTheme.Accent;Round(dot,Q.Dot/2);head.Add(dot);}
  if(!string.IsNullOrEmpty(info))Write(words,info,KarineTheme.Secondary,Q.InfoSize,Typewriter).style.marginBottom=0;
  if(!string.IsNullOrEmpty(quote)){var q=Write(words,quote,KarineTheme.Secondary,Q.InfoSize,Typewriter);q.style.marginBottom=0;q.style.whiteSpace=WhiteSpace.NoWrap;q.style.overflow=Overflow.Hidden;q.style.textOverflow=TextOverflow.Ellipsis;}
  words.Query<Label>().ForEach(l=>l.style.unityTextAlign=TextAnchor.MiddleLeft);
  RequestPill(row,status,tone).style.marginLeft=KarineTheme.SpaceSm;
  return row;
 }
 // Durum etiketi: hazır amber dolu, bekleyen saatli çerçeve, biten soluk tikli, kapalı kırmızı çerçeve.
 public static VisualElement RequestPill(VisualElement parent,string label,RequestTone tone) {
  var pill=new VisualElement {name="RequestPill",pickingMode=PickingMode.Ignore};pill.style.flexDirection=FlexDirection.Row;pill.style.alignItems=Align.Center;pill.style.justifyContent=Justify.Center;
  pill.style.minWidth=Q.PillWidth;pill.style.height=Q.PillHeight;pill.style.flexShrink=0;pill.style.paddingLeft=KarineTheme.SpaceSm;pill.style.paddingRight=KarineTheme.SpaceSm;Round(pill,KarineTheme.Radius);
  Color ink=tone==RequestTone.Ready?KarineTheme.OnPrimary:tone==RequestTone.Done?KarineTheme.Alpha(KarineTheme.Secondary,.7f):tone==RequestTone.Gone?KarineTheme.Danger:KarineTheme.Primary;
  pill.style.backgroundColor=tone==RequestTone.Ready?KarineTheme.Accent:tone==RequestTone.Done?KarineTheme.Alpha(KarineTheme.Border,.4f):KarineTheme.Alpha(KarineTheme.GlassDeep,.9f);
  Border(pill,KarineTheme.BorderWidth,tone==RequestTone.Ready?KarineTheme.Accent:tone==RequestTone.Gone?KarineTheme.Danger:tone==RequestTone.Waiting?KarineTheme.Border:tone==RequestTone.Done?KarineTheme.Alpha(KarineTheme.Border,.5f):KarineTheme.Primary);
  if(tone==RequestTone.Waiting||tone==RequestTone.Done)Icon(pill,tone==RequestTone.Waiting?"clock":"check",ink,Q.PillSize+2).style.marginRight=KarineTheme.SpaceXs;
  var l=Write(pill,label.TrimStart('●',' ').ToUpper(TextCulture),ink,Q.PillSize,Heading);l.name="RequestPillText";l.style.marginBottom=0;l.style.whiteSpace=WhiteSpace.NoWrap;
  parent.Add(pill);return pill;
 }
 public static VisualElement RequestPaper(VisualElement parent) {
  var paper=new VisualElement {name="RequestDetail"};OfficePlace(paper,Q.Paper);
  // Yalnız yıpranmış kağıt görseli: arkasında düz zemin ya da çerçeve yok, yırtık kenarlar açıkta kalır.
  Stretched(paper,"Bube/UI/paper_sheet");parent.Add(paper);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;paper.Add(scroll);
  var c=scroll.contentContainer;c.style.paddingLeft=KarineTheme.SpaceXl;c.style.paddingRight=KarineTheme.SpaceXl;c.style.paddingTop=KarineTheme.SpaceLg;c.style.paddingBottom=KarineTheme.SpaceLg;
  return c;
 }
 // Kişi kartı başı: ataşlı polaroid, ad, bilgi ve sağ üstte durum.
 public static void RequestPersonHead(VisualElement paper,Texture2D portrait,string name,string info,string status,RequestTone tone) {
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;top.style.marginBottom=KarineTheme.SpaceMd;paper.Add(top);
  var frame=new VisualElement {pickingMode=PickingMode.Ignore};frame.style.width=Q.Polaroid;frame.style.height=Q.Polaroid*1.1f;frame.style.flexShrink=0;frame.style.marginRight=KarineTheme.SpaceLg;
  frame.style.paddingLeft=KarineTheme.SpaceXs+2;frame.style.paddingRight=KarineTheme.SpaceXs+2;frame.style.paddingTop=KarineTheme.SpaceXs+2;frame.style.paddingBottom=KarineTheme.SpaceXs+2;
  frame.style.backgroundColor=KarineTheme.Paper.Light;Border(frame,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);frame.style.rotate=new Rotate(-2);top.Add(frame);
  if(portrait!=null){var face=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop};face.style.flexGrow=1;if(tone==RequestTone.Gone)face.tintColor=KarineTheme.Alpha(KarineTheme.Secondary,.6f);frame.Add(face);}
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;top.Add(words);
  var line=new VisualElement();line.style.flexDirection=FlexDirection.Row;line.style.alignItems=Align.FlexStart;words.Add(line);
  var n=Typed(line,name.ToUpper(TextCulture),Q.HeadSize,true);n.style.flexGrow=1;n.style.flexShrink=1;n.style.whiteSpace=WhiteSpace.Normal;
  RequestPill(line,status,tone);
  if(!string.IsNullOrEmpty(info))Typed(words,info,Q.BodySize,false,KarineTheme.Paper.Faded);
  PaperRule(words,false);
 }
 // İnceleme kartı başı: büyük belge ikonu ve başlık; altında talebin adı ve çizgi.
 public static void RequestDocumentHead(VisualElement paper,string icon,string heading,string title) {
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.Center;top.style.marginBottom=KarineTheme.SpaceSm;paper.Add(top);
  Icon(top,icon,KarineTheme.Paper.Ink,Q.HeadIcon).style.marginRight=KarineTheme.SpaceMd;
  Typed(top,heading.ToUpper(TextCulture),Q.HeadSize,true);
  PaperRule(paper,false);
  Typed(paper,title.ToUpper(TextCulture),Q.SectionSize,true).style.marginBottom=KarineTheme.SpaceXs;
  PaperRule(paper,false);
 }
 public static void RequestSection(VisualElement paper,string heading,string text) {
  Typed(paper,heading.ToUpper(TextCulture),Q.SectionSize,true).style.marginTop=KarineTheme.SpaceSm;
  var b=Typed(paper,text,Q.BodySize);b.style.whiteSpace=WhiteSpace.Normal;b.style.marginBottom=KarineTheme.SpaceMd;
 }
 // DURUM / SONUÇ gibi anahtar–değer satırları; anahtar sütunu hafif gölgeli.
 public static void RequestFact(VisualElement paper,string key,string value) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.marginBottom=2;paper.Add(row);
  var k=new VisualElement();k.style.width=Q.FactKey;k.style.paddingLeft=KarineTheme.SpaceSm;k.style.paddingTop=2;k.style.paddingBottom=2;k.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.35f);row.Add(k);
  Typed(k,key.ToUpper(TextCulture),Q.BodySize,true).style.marginBottom=0;
  var v=Typed(row,":  "+value,Q.BodySize);v.style.marginLeft=KarineTheme.SpaceMd;v.style.marginBottom=0;v.style.flexShrink=1;v.style.whiteSpace=WhiteSpace.Normal;
 }
 // İnceleme izni dayanağı: kâğıt üstünde işaret kutulu satır. İşaretli satır mürekkep dolu kutu, kalın yazı.
 public static Button RequestBasis(VisualElement paper,string label,bool chosen,Action action) {
  var row=new Button(Sounded(action)) {name="RequestBasis",tooltip=label};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.marginLeft=0;row.style.marginRight=0;row.style.marginBottom=KarineTheme.SpaceXs;row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceSm;
  row.style.paddingTop=KarineTheme.SpaceXs;row.style.paddingBottom=KarineTheme.SpaceXs;
  Unskin(row,chosen?KarineTheme.Alpha(KarineTheme.Accent,.14f):KarineTheme.Alpha(KarineTheme.Paper.Edge,.12f));Round(row,KarineTheme.Radius);
  var box=new VisualElement {pickingMode=PickingMode.Ignore};box.style.width=Q.Dot*2;box.style.height=Q.Dot*2;box.style.flexShrink=0;box.style.marginRight=KarineTheme.SpaceMd;
  box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;Border(box,KarineTheme.BorderWidth+1,KarineTheme.Paper.Ink);
  if(chosen){box.style.backgroundColor=KarineTheme.Paper.Ink;Icon(box,"check",KarineTheme.Paper.Sheet,Q.Dot*2-2);}
  row.Add(box);
  var t=Typed(row,label,Q.BodySize,chosen);t.style.marginBottom=0;t.style.flexShrink=1;t.style.whiteSpace=WhiteSpace.Normal;t.pickingMode=PickingMode.Ignore;
  paper.Add(row);return row;
 }
 // Büyük eylem: amber dolu ana düğme ya da koyu ikincil (reklamla beklemeyi atla).
 public static Button RequestAction(VisualElement paper,string icon,string label,bool primary,Action action) {
  var b=new Button(Sounded(action)) {name="RequestAction",tooltip=label};b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;b.style.justifyContent=Justify.Center;
  b.style.height=Q.ActionHeight;b.style.marginLeft=0;b.style.marginRight=0;b.style.marginTop=KarineTheme.SpaceMd;
  Unskin(b,primary?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.GlassDeep,.95f));Border(b,KarineTheme.BorderWidth,primary?KarineTheme.Paper.Ink:KarineTheme.Border);Round(b,KarineTheme.Radius);
  var ink=primary?KarineTheme.OnPrimary:KarineTheme.Primary;
  if(!string.IsNullOrEmpty(icon))Icon(b,icon,ink,Q.ActionSize+4).style.marginRight=KarineTheme.SpaceMd;
  Write(b,label.TrimEnd('›',' ').ToUpper(TextCulture),ink,Q.ActionSize,Heading).style.marginBottom=0;
  paper.Add(b);return b;
 }
 // Bekleme hali: saat, başlık, açıklama ve büyük geri sayım; geri sayım her saniye kendini yazar.
 public static void RequestWait(VisualElement paper,string title,string text,DateTime readyUtc) {
  var box=new VisualElement {name="RequestWait"};box.style.alignItems=Align.Center;box.style.marginTop=KarineTheme.SpaceMd;paper.Add(box);
  Icon(box,"clock",KarineTheme.Paper.Ink,Q.WaitIcon).style.marginBottom=KarineTheme.SpaceSm;
  Typed(box,title.ToUpper(TextCulture),Q.SectionSize+2,true);
  var t=Typed(box,text,Q.BodySize,false,KarineTheme.Paper.Faded);t.style.whiteSpace=WhiteSpace.Normal;t.style.unityTextAlign=TextAnchor.MiddleCenter;
  var clock=Typed(box,Countdown(readyUtc),Q.CountdownSize,true);clock.name="RequestCountdown";
  clock.schedule.Execute(()=>clock.text=Countdown(readyUtc)).Every(1000);
 }
 public static string Countdown(DateTime readyUtc) {
  var left=readyUtc-DateTime.UtcNow;if(left<TimeSpan.Zero)left=TimeSpan.Zero;
  return ((int)left.TotalMinutes).ToString("00")+":"+left.Seconds.ToString("00");
 }
}
}
