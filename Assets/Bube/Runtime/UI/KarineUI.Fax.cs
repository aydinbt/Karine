using System;
using UnityEngine;
using UnityEngine.UIElements;
using F = Bube.KarineTheme.Fax;
namespace Bube {
// Faks / değerlendirme (3 Ekim 2026 maketi, Docs/Reference/UI_FAX_2026-10.png): termal faks kâğıdı
// (gönderen satırı, büyük sonuç, konu–açıklama–değerlendirme tablosu, not, kurul mührü) ve sağda
// kurum güveni paneli. Puan, yıldız ya da kutlama yok; resmî yazışma.
public static partial class KarineUI {
 public static VisualElement FaxPaper(VisualElement parent,string meta,string title,out Label heading) {
  var paper=new VisualElement {name="FaxPaper"};OfficePlace(paper,F.Paper);paper.style.rotate=new Rotate(F.Tilt);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceXl*2;paper.style.paddingTop=KarineTheme.SpaceXl;paper.style.paddingBottom=KarineTheme.SpaceLg;parent.Add(paper);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;paper.Add(scroll);var c=scroll.contentContainer;
  var m=Typed(c,meta.ToUpper(TextCulture),F.MetaSize);m.style.unityTextAlign=TextAnchor.MiddleCenter;m.style.marginBottom=KarineTheme.SpaceSm;
  PaperRule(c,true);
  heading=Write(c,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,F.TitleSize,Heading);heading.name="FaxTitle";
  heading.style.unityTextAlign=TextAnchor.MiddleCenter;heading.style.marginTop=KarineTheme.SpaceSm;heading.style.marginBottom=KarineTheme.SpaceSm;heading.style.letterSpacing=2;
  PaperRule(c,true);
  return c;
 }
 static VisualElement FaxCell(VisualElement row,string text,float width,bool head,bool bold,bool last) {
  var cell=new VisualElement();if(width>0){cell.style.width=Length.Percent(width);cell.style.flexShrink=0;}else{cell.style.flexGrow=1;cell.style.flexShrink=1;}
  cell.style.paddingLeft=cell.style.paddingRight=KarineTheme.SpaceSm;cell.style.paddingTop=cell.style.paddingBottom=KarineTheme.SpaceXs+2;cell.style.justifyContent=Justify.Center;
  if(!last){cell.style.borderRightWidth=1;cell.style.borderRightColor=KarineTheme.Paper.Ink;}
  row.Add(cell);
  var l=head||bold?Write(cell,text.ToUpper(TextCulture),KarineTheme.Paper.Ink,head?F.HeadSize:F.HeadSize+1,Heading):Typed(cell,text,F.CellSize);
  l.style.marginBottom=0;l.style.whiteSpace=WhiteSpace.Normal;
  return cell;
 }
 public static VisualElement FaxTable(VisualElement parent,string key,string text,string verdict) {
  var table=new VisualElement {name="FaxTable"};Border(table,1,KarineTheme.Paper.Ink);table.style.marginTop=KarineTheme.SpaceLg;table.style.marginBottom=KarineTheme.SpaceLg;parent.Add(table);
  FaxRow(table,key,text,verdict,true);return table;
 }
 public static void FaxRow(VisualElement table,string key,string text,string verdict,bool head=false) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Stretch;
  if(table.childCount>0){row.style.borderTopWidth=1;row.style.borderTopColor=KarineTheme.Paper.Ink;}
  table.Add(row);
  FaxCell(row,key,F.KeyWidth,head,true,false);FaxCell(row,text,0,head,false,false);FaxCell(row,verdict,F.VerdictWidth,head,false,true);
 }
 public static void FaxNote(VisualElement parent,string text) {
  var l=Typed(parent,text,F.NoteSize);l.style.unityFontStyleAndWeight=FontStyle.Italic;l.style.whiteSpace=WhiteSpace.Normal;l.style.marginBottom=KarineTheme.SpaceLg;
 }
 // Kurul mührü: soluk mürekkep halkası ve içinde kurul adı; paraf çizgisi yanında.
 public static void FaxSeal(VisualElement parent,string text) {
  var row=new VisualElement {pickingMode=PickingMode.Ignore};row.style.flexDirection=FlexDirection.Row;row.style.justifyContent=Justify.FlexEnd;row.style.alignItems=Align.Center;parent.Add(row);
  var seal=new VisualElement {name="FaxSeal"};seal.style.width=seal.style.height=F.Stamp;Round(seal,F.Stamp/2);Border(seal,3,KarineTheme.Alpha(KarineTheme.Paper.Stamp,.55f));
  seal.style.alignItems=Align.Center;seal.style.justifyContent=Justify.Center;seal.style.rotate=new Rotate(-12);row.Add(seal);
  var t=Write(seal,text.ToUpper(TextCulture),KarineTheme.Alpha(KarineTheme.Paper.Stamp,.6f),F.HeadSize-2,Heading);t.style.marginBottom=0;t.style.unityTextAlign=TextAnchor.MiddleCenter;t.style.maxWidth=F.Stamp-16;
  var sign=new VisualElement();sign.style.width=F.Stamp;sign.style.height=2;sign.style.marginLeft=KarineTheme.SpaceLg;sign.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Ink,.6f);sign.style.rotate=new Rotate(-14);row.Add(sign);
 }
 public static VisualElement FaxSide(VisualElement parent,string heading,string status,int trend,string note) {
  var panel=new VisualElement {name="FaxSide"};OfficePlace(panel,F.Side);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.95f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Alpha(KarineTheme.Accent,.6f));Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=panel.style.paddingRight=KarineTheme.SpaceXl;panel.style.paddingTop=KarineTheme.SpaceXl;panel.style.paddingBottom=KarineTheme.SpaceXl;parent.Add(panel);
  Write(panel,heading.ToUpper(TextCulture),KarineTheme.Primary,F.SideHead,Heading).style.marginBottom=KarineTheme.SpaceSm;
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Border;rule.style.marginBottom=KarineTheme.SpaceLg;panel.Add(rule);
  var box=new VisualElement {name="FaxTrust"};box.style.flexDirection=FlexDirection.Row;box.style.alignItems=Align.Center;Border(box,KarineTheme.BorderWidth,KarineTheme.Border);Round(box,KarineTheme.Radius);
  box.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.6f);panel.Add(box);
  var icon=new VisualElement();icon.style.width=icon.style.height=F.StatusBox;icon.style.alignItems=Align.Center;icon.style.justifyContent=Justify.Center;
  icon.style.borderRightWidth=KarineTheme.BorderWidth;icon.style.borderRightColor=KarineTheme.Border;box.Add(icon);Icon(icon,"chart",KarineTheme.Accent,F.StatusIcon);
  var s=Write(box,status+(trend>0?"  ↑":trend<0?"  ↓":""),KarineTheme.Primary,F.StatusSize,Heading);s.style.marginBottom=0;s.style.marginLeft=KarineTheme.SpaceLg;
  if(!string.IsNullOrEmpty(note)){var n=Write(panel,note,KarineTheme.Secondary,F.SideNote,Typewriter);n.style.marginTop=KarineTheme.SpaceMd;}
  var gap=new VisualElement {pickingMode=PickingMode.Ignore};gap.style.flexGrow=1;panel.Add(gap);
  return panel;
 }
 public static Button FaxLink(VisualElement parent,string icon,string label,Action action) {
  var b=new Button(Sounded(action)) {name="FaxLink",tooltip=label};b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;b.style.height=F.LinkHeight;
  b.style.marginLeft=b.style.marginRight=b.style.marginTop=0;b.style.marginBottom=KarineTheme.SpaceMd;b.style.paddingLeft=b.style.paddingRight=KarineTheme.SpaceLg;
  Unskin(b,KarineTheme.Alpha(KarineTheme.Background,.6f));Border(b,KarineTheme.BorderWidth,KarineTheme.Border);Round(b,KarineTheme.Radius);
  Icon(b,icon,KarineTheme.Primary,KarineTheme.IconSize+4).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(b,label.ToUpper(TextCulture),KarineTheme.Primary,F.LinkSize,Heading);l.style.marginBottom=0;l.style.flexGrow=1;
  Icon(b,"nav_next",KarineTheme.Primary,KarineTheme.IconSize);
  parent.Add(b);return b;
 }
}
}
