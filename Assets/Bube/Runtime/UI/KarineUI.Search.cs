using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Search;
namespace Bube {
// Dosyada gezin ekranının parçaları (3 Ekim 2026 maketi, Docs/Reference/UI_SEARCH_2026-10.png).
public static partial class KarineUI {
 public static VisualElement SearchFilters(VisualElement parent) {
  var panel=new KarineScrollView(ScrollViewMode.Vertical) {name="SearchFilters"};OfficePlace(panel,S.Filters);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.96f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Border);Round(panel,KarineTheme.Radius);
  var c=panel.contentContainer;c.style.paddingLeft=KarineTheme.SpaceSm;c.style.paddingRight=KarineTheme.SpaceSm;c.style.paddingTop=KarineTheme.SpaceMd;c.style.paddingBottom=KarineTheme.SpaceMd;
  parent.Add(panel);return c;
 }
 public static void SearchGroup(VisualElement panel,string title,bool first) {
  var g=Write(panel,title.ToUpper(Tr),KarineTheme.Primary,S.GroupSize,Heading);g.style.marginLeft=KarineTheme.SpaceXs;g.style.marginBottom=KarineTheme.SpaceSm;
  if(!first){g.style.marginTop=KarineTheme.SpaceLg;g.style.paddingTop=KarineTheme.SpaceMd;g.style.borderTopWidth=1;g.style.borderTopColor=KarineTheme.Alpha(KarineTheme.Border,.6f);}
 }
 // Süzgeç satırı: ikon ya da yuvarlak portre ve ad; seçili olan amber çerçeveli.
 public static Button SearchFilter(VisualElement panel,string icon,Texture2D portrait,string label,bool selected,Action action) {
  var row=new Button(Sounded(action)) {name="SearchFilter",tooltip=label};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=KarineTheme.TouchTarget+KarineTheme.SpaceXs;row.style.marginLeft=0;row.style.marginRight=0;row.style.marginBottom=KarineTheme.SpaceSm;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceXs;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Accent,.12f):KarineTheme.Alpha(KarineTheme.Background,.7f));
  Border(row,selected?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(row,KarineTheme.Radius);
  if(portrait!=null) {
   var face=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};face.style.width=S.Portrait;face.style.height=S.Portrait;face.style.flexShrink=0;
   Round(face,S.Portrait/2);Border(face,KarineTheme.BorderWidth,KarineTheme.Border);face.style.marginRight=KarineTheme.SpaceSm;row.Add(face);
  } else Icon(row,icon,selected?KarineTheme.Accent:KarineTheme.Secondary,S.FilterIcon).style.marginRight=KarineTheme.SpaceSm;
  var l=Write(row,portrait!=null?label:label.ToUpper(Tr),selected?KarineTheme.Primary:KarineTheme.Secondary,S.FilterSize,Heading);l.style.marginBottom=0;l.style.flexShrink=1;
  panel.Add(row);return row;
 }
 public static VisualElement SearchPaper(VisualElement parent) {
  var paper=new VisualElement {name="SearchPaper"};OfficePlace(paper,S.Paper);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(paper,KarineTheme.Radius);
  paper.style.paddingLeft=KarineTheme.SpaceXl;paper.style.paddingRight=KarineTheme.SpaceXl;paper.style.paddingTop=KarineTheme.SpaceLg;paper.style.paddingBottom=KarineTheme.SpaceMd;
  parent.Add(paper);return paper;
 }
 public static void SearchHead(VisualElement paper,string title,string count) {
  var line=new VisualElement();line.style.flexDirection=FlexDirection.Row;line.style.alignItems=Align.FlexEnd;line.style.flexShrink=0;paper.Add(line);
  Typed(line,title.ToUpper(Tr),S.TitleSize,true).style.flexGrow=1;
  Typed(line,count.ToUpper(Tr),S.CountSize,true).style.marginRight=KarineTheme.SpaceXl;
  PaperRule(paper,false);
 }
 // Son aramalar: koyu çipler; çipe dokununca süzgeç geri gelir, ✕ onu listeden siler.
 public static VisualElement SearchRecentRow(VisualElement paper,string title) {
  Typed(paper,title.ToUpper(Tr),S.GroupSize-2,true).style.marginBottom=KarineTheme.SpaceSm;
  var row=new VisualElement {name="SearchRecent"};row.style.flexDirection=FlexDirection.Row;row.style.flexWrap=Wrap.Wrap;row.style.flexShrink=0;row.style.marginBottom=KarineTheme.SpaceMd;
  paper.Add(row);return row;
 }
 public static void SearchRecentChip(VisualElement row,string label,Action pick,Action remove,string removeTitle) {
  var chip=new VisualElement {name="SearchRecentChip"};chip.style.flexDirection=FlexDirection.Row;chip.style.alignItems=Align.Center;chip.style.height=S.RecentHeight;
  chip.style.marginRight=KarineTheme.SpaceSm;chip.style.marginBottom=KarineTheme.SpaceSm;
  chip.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Border(chip,KarineTheme.BorderWidth,KarineTheme.Border);Round(chip,KarineTheme.Radius);row.Add(chip);
  var main=new Button(Sounded(pick)) {tooltip=label};Unskin(main,Color.clear);main.style.height=S.RecentHeight;main.style.marginLeft=0;main.style.marginRight=0;
  main.style.paddingLeft=KarineTheme.SpaceMd;main.style.paddingRight=KarineTheme.SpaceXs;chip.Add(main);
  Write(main,label,KarineTheme.Primary,S.RecentSize,Typewriter).style.marginBottom=0;
  var x=new Button(Sounded(remove)) {tooltip=removeTitle};Unskin(x,Color.clear);x.style.height=S.RecentHeight;x.style.width=S.RecentHeight;x.style.marginLeft=0;x.style.marginRight=0;
  x.style.alignItems=Align.Center;x.style.justifyContent=Justify.Center;chip.Add(x);
  Icon(x,"close",KarineTheme.Secondary,S.RecentSize+2);
 }
 // Sonuç kartı: ikon, kalın kaynak adı ve tarih, eşleşen cümle (kişinin adı amber vurgulu), koyu "AÇ ›".
 public static Button SearchResult(VisualElement list,string icon,string title,string date,string excerpt,string openLabel,Action open) {
  var card=new Button(Sounded(open)) {name="SearchResult",tooltip=title};card.style.flexDirection=FlexDirection.Row;card.style.alignItems=Align.Center;
  card.style.marginLeft=0;card.style.marginRight=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceMd;card.style.paddingRight=KarineTheme.SpaceMd;card.style.paddingTop=KarineTheme.SpaceSm;card.style.paddingBottom=KarineTheme.SpaceSm;
  Unskin(card,KarineTheme.Alpha(KarineTheme.Paper.Light,.8f));Border(card,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(card,KarineTheme.Radius);list.Add(card);
  var badge=new VisualElement {pickingMode=PickingMode.Ignore};badge.style.width=S.ResultIcon+KarineTheme.SpaceXl;badge.style.alignSelf=Align.Stretch;badge.style.alignItems=Align.Center;badge.style.justifyContent=Justify.Center;
  badge.style.borderRightWidth=1;badge.style.borderRightColor=KarineTheme.Paper.Edge;badge.style.marginRight=KarineTheme.SpaceMd;card.Add(badge);
  Icon(badge,icon,KarineTheme.Paper.Ink,S.ResultIcon);
  var text=new VisualElement {pickingMode=PickingMode.Ignore};text.style.flexGrow=1;text.style.flexShrink=1;card.Add(text);
  var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.Center;head.style.marginBottom=KarineTheme.SpaceXs;text.Add(head);
  var t=Typed(head,title,S.ResultTitleSize,true);t.style.flexGrow=1;t.style.flexShrink=1;
  if(!string.IsNullOrEmpty(date))Typed(head,date,S.DateSize,false,KarineTheme.Paper.Faded).style.marginLeft=KarineTheme.SpaceMd;
  var body=Typed(text,excerpt,S.ResultTextSize);body.enableRichText=true;body.style.whiteSpace=WhiteSpace.Normal;
  var go=new VisualElement {pickingMode=PickingMode.Ignore};go.style.flexDirection=FlexDirection.Row;go.style.alignItems=Align.Center;go.style.justifyContent=Justify.Center;
  go.style.width=S.OpenWidth;go.style.height=KarineTheme.TouchTarget-4;go.style.marginLeft=KarineTheme.SpaceMd;go.style.flexShrink=0;
  go.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Round(go,KarineTheme.Radius);card.Add(go);
  Write(go,openLabel.ToUpper(Tr),KarineTheme.Primary,S.ResultTitleSize,Heading).style.marginBottom=0;
  Icon(go,"nav_next",KarineTheme.Primary,S.ResultTitleSize).style.marginLeft=KarineTheme.SpaceXs;
  return card;
 }
 public static void SearchEmpty(VisualElement paper,string text) {
  var box=new VisualElement {name="SearchEmpty",pickingMode=PickingMode.Ignore};box.style.flexGrow=1;box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;paper.Add(box);
  Icon(box,"search",KarineTheme.Alpha(KarineTheme.Paper.Faded,.6f),S.EmptyIcon).style.marginBottom=KarineTheme.SpaceLg;
  var l=Typed(box,text,S.EmptySize);l.style.unityTextAlign=TextAnchor.MiddleCenter;
 }
}
}
