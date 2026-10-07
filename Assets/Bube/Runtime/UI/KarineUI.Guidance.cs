using System;
using UnityEngine;
using UnityEngine.UIElements;
using G = Bube.KarineTheme.Guidance;
namespace Bube {
// Yöntem hatırlatması ve dosyayı yeniden açma (3 Ekim 2026 maketleri,
// Docs/Reference/UI_GUIDANCE_2026-10.png, UI_RETRY_2026-10.png). Her vakada aynı
// düzen; içinde vakanın gerçeği yok: işin kuralları ve oyuncunun kendi sayıları.
public static partial class KarineUI {
 // Ataçlı krem kâğıt; içerik kabını döndürür.
 public static VisualElement GuidancePaper(VisualElement parent,Rect rect,float tilt) {
  var paper=new VisualElement {name="GuidancePaper"};OfficePlace(paper,rect);paper.style.rotate=new Rotate(tilt);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceXl*2;paper.style.paddingTop=KarineTheme.SpaceXl*2;paper.style.paddingBottom=KarineTheme.SpaceXl;parent.Add(paper);
  var clip=new VisualElement {pickingMode=PickingMode.Ignore};clip.style.position=Position.Absolute;clip.style.left=Length.Percent(6);clip.style.top=-G.Clip/3;
  clip.style.width=G.Clip/3;clip.style.height=G.Clip;Border(clip,3,KarineTheme.Alpha(KarineTheme.Secondary,.8f));Round(clip,G.Clip/6);clip.style.rotate=new Rotate(12);paper.Add(clip);
  KarineMotion.Paper(paper);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;paper.Add(scroll);
  return scroll.contentContainer;
 }
 public static void GuidanceHeading(VisualElement parent,string title,string intro) {
  var h=Write(parent,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,G.TitleSize,Heading);h.style.marginBottom=KarineTheme.SpaceXs;
  var rule=new VisualElement();rule.style.height=3;rule.style.backgroundColor=KarineTheme.Paper.Ink;rule.style.marginBottom=KarineTheme.SpaceMd;parent.Add(rule);
  if(string.IsNullOrEmpty(intro))return;
  var l=Typed(parent,intro,G.IntroSize);l.style.unityFontStyleAndWeight=FontStyle.Italic;l.style.whiteSpace=WhiteSpace.Normal;l.style.marginBottom=KarineTheme.SpaceSm;
 }
 // Numaralı madde: koyu kutuda "01", dikey çizgi, daktilo metin; üstünde ince çizgi.
 public static void GuidanceItem(VisualElement parent,int number,string text) {
  var row=new VisualElement {name="GuidanceItem"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.borderTopWidth=1;row.style.borderTopColor=KarineTheme.Alpha(KarineTheme.Paper.Ink,.4f);row.style.paddingTop=row.style.paddingBottom=KarineTheme.SpaceMd;parent.Add(row);
  var box=new VisualElement();box.style.width=G.Number;box.style.height=G.Number;box.style.flexShrink=0;box.style.backgroundColor=KarineTheme.Paper.Ink;
  box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;row.Add(box);
  var n=Write(box,number.ToString("00"),KarineTheme.Paper.Sheet,G.NumberSize,Heading);n.style.marginBottom=0;
  var bar=new VisualElement();bar.style.width=2;bar.style.alignSelf=Align.Stretch;bar.style.backgroundColor=KarineTheme.Paper.Ink;bar.style.marginLeft=bar.style.marginRight=KarineTheme.SpaceLg;row.Add(bar);
  var t=Typed(row,text,G.ItemSize);t.style.whiteSpace=WhiteSpace.Normal;t.style.flexShrink=1;t.style.marginBottom=0;
 }
 // Sağdaki koyu cam panel: başlık ve çizgi; içerik kabı panelin kendisi.
 public static VisualElement GuidanceSide(VisualElement parent,string heading) {
  var panel=new VisualElement {name="GuidanceSide"};OfficePlace(panel,G.Side);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.95f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Alpha(KarineTheme.Accent,.6f));Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=panel.style.paddingRight=KarineTheme.SpaceXl;panel.style.paddingTop=KarineTheme.SpaceXl;panel.style.paddingBottom=KarineTheme.SpaceXl;parent.Add(panel);
  Write(panel,heading.ToUpper(TextCulture),KarineTheme.Primary,G.SideHead,Heading).style.marginBottom=KarineTheme.SpaceSm;
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Border;rule.style.marginBottom=KarineTheme.SpaceLg;panel.Add(rule);
  return panel;
 }
 // Ölçü satırı: solda ikon kutusu; sağda etiket, "6/9" ve kehribar çubuk.
 public static void GuidanceMeter(VisualElement parent,string icon,string label,int done,int total) {
  total=Mathf.Max(total,done);
  var row=new VisualElement {name="GuidanceMeter"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Stretch;row.style.marginBottom=KarineTheme.SpaceMd;
  Border(row,KarineTheme.BorderWidth,KarineTheme.Border);Round(row,KarineTheme.Radius);row.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.6f);parent.Add(row);
  var box=new VisualElement();box.style.width=G.MeterIconBox;box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;
  box.style.borderRightWidth=KarineTheme.BorderWidth;box.style.borderRightColor=KarineTheme.Border;row.Add(box);Icon(box,icon,KarineTheme.Accent,G.MeterIcon);
  var body=new VisualElement();body.style.flexGrow=1;body.style.paddingLeft=body.style.paddingRight=KarineTheme.SpaceLg;body.style.paddingTop=body.style.paddingBottom=KarineTheme.SpaceMd;row.Add(body);
  var line=new VisualElement();line.style.flexDirection=FlexDirection.Row;line.style.justifyContent=Justify.SpaceBetween;body.Add(line);
  var l=Write(line,label,KarineTheme.Primary,G.MeterSize,Typewriter);l.style.marginBottom=0;l.style.flexShrink=1;
  var c=Write(line,done+"/"+total,KarineTheme.Primary,G.MeterSize,Typewriter);c.style.marginBottom=0;
  var track=new VisualElement();track.style.height=G.Bar;track.style.marginTop=KarineTheme.SpaceSm;Round(track,G.Bar/2);track.style.backgroundColor=KarineTheme.Border;body.Add(track);
  var fill=new VisualElement();fill.style.height=Length.Percent(100);fill.style.width=Length.Percent(total<=0?100:100f*done/total);Round(fill,G.Bar/2);fill.style.backgroundColor=KarineTheme.Accent;track.Add(fill);
 }
 public static Label GuidanceText(VisualElement parent,string text,int size) {
  var l=Typed(parent,text,size);l.style.whiteSpace=WhiteSpace.Normal;return l;
 }
 // Kâğıda çapraz basılmış mürekkep damgası.
 public static void GuidanceStamp(VisualElement parent,string text) {
  var row=new VisualElement {pickingMode=PickingMode.Ignore};row.style.alignItems=Align.FlexEnd;row.style.marginBottom=KarineTheme.SpaceMd;parent.Add(row);
  var stamp=new VisualElement {name="GuidanceStamp",pickingMode=PickingMode.Ignore};Border(stamp,4,KarineTheme.Alpha(KarineTheme.Paper.Stamp,.85f));Round(stamp,KarineTheme.Radius);
  stamp.style.paddingLeft=stamp.style.paddingRight=KarineTheme.SpaceLg;stamp.style.rotate=new Rotate(-8);row.Add(stamp);
  var t=Write(stamp,text.ToUpper(TextCulture),KarineTheme.Alpha(KarineTheme.Paper.Stamp,.9f),G.StampSize,Heading);t.style.marginBottom=0;
 }
 // Kâğıt üstünde koyu durum kutusu: ikon, küçük başlık, büyük kehribar durum.
 public static void GuidanceStatus(VisualElement parent,string icon,string heading,string status,int trend) {
  var box=new VisualElement {name="GuidanceStatus"};box.style.flexDirection=FlexDirection.Row;box.style.alignItems=Align.Center;box.style.alignSelf=Align.Center;
  box.style.backgroundColor=KarineTheme.GlassDeep;Border(box,KarineTheme.BorderWidth,KarineTheme.Accent);Round(box,KarineTheme.Radius);box.style.marginTop=KarineTheme.SpaceMd;box.style.marginBottom=KarineTheme.SpaceMd;parent.Add(box);
  var ib=new VisualElement();ib.style.width=ib.style.height=G.StatusBox;ib.style.alignItems=Align.Center;ib.style.justifyContent=Justify.Center;
  ib.style.borderRightWidth=KarineTheme.BorderWidth;ib.style.borderRightColor=KarineTheme.Border;box.Add(ib);Icon(ib,icon,KarineTheme.Accent,G.StatusIcon);
  var col=new VisualElement();col.style.paddingLeft=col.style.paddingRight=KarineTheme.SpaceLg;box.Add(col);
  Write(col,heading.ToUpper(TextCulture),KarineTheme.Primary,G.MeterSize,Heading).style.marginBottom=0;
  var s=Write(col,status.ToUpper(TextCulture)+(trend>0?"  ↑":trend<0?"  ↓":""),KarineTheme.Accent,G.StatusSize,Heading);s.style.marginBottom=0;
 }
}
}
