using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 public static VisualElement InboxHeader(VisualElement parent,string title,string back,Action close) {
  var bar=new VisualElement();OfficePlace(bar,new Rect(0,0,100,13));
  bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;
  bar.style.paddingLeft=KarineTheme.SpaceXl;bar.style.paddingRight=KarineTheme.SpaceXl;
  bar.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.92f);parent.Add(bar);
  var logo=KarineLogo.Hero(bar,KarineTheme.Inbox.LogoWidth);logo.style.marginRight=KarineTheme.SpaceXl;
  IconButton(bar,"nav_prev",close,back);
  Icon(bar,"document",KarineTheme.Primary,KarineTheme.IconSize).style.marginLeft=KarineTheme.SpaceXl;
  var heading=Subtitle(bar,title,KarineTheme.Inbox.HeadingSize);heading.style.color=KarineTheme.Primary;
  heading.style.marginLeft=KarineTheme.SpaceMd;heading.style.marginBottom=0;heading.style.flexGrow=1;
  return bar;
 }
 public static VisualElement InboxList(VisualElement parent,string brand) {
  var panel=new VisualElement {name="InboxListPanel"};OfficePlace(panel,KarineTheme.Inbox.List);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Accent);
  panel.style.paddingLeft=KarineTheme.SpaceMd;panel.style.paddingRight=KarineTheme.SpaceMd;
  panel.style.paddingTop=KarineTheme.SpaceLg;panel.style.paddingBottom=KarineTheme.SpaceMd;parent.Add(panel);
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;panel.Add(row);
  Icon(row,"people",KarineTheme.Primary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  Body_(row,brand,KarineTheme.Inbox.BodySize);return panel;
 }
 public static VisualElement InboxPaper(VisualElement parent) {
  for(int i=0;i<3;i++) {
   var layer=new VisualElement();OfficePlace(layer,new Rect(39+i*.4f,14+i*.6f,55,79));
   layer.style.backgroundColor=i==0?KarineTheme.Paper.Folder:KarineTheme.Paper.Tint;
   Border(layer,KarineTheme.BorderWidth,KarineTheme.Paper.FolderDeep);parent.Add(layer);
  }
  var paper=new VisualElement {name="InboxPaper"};OfficePlace(paper,KarineTheme.Inbox.Paper);
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  paper.style.backgroundImage=new StyleBackground(Resources.Load<Texture2D>("Bube/Art/DossierPaper"));
  paper.style.paddingLeft=KarineTheme.SpaceXl;paper.style.paddingRight=KarineTheme.SpaceXl;
  paper.style.paddingTop=KarineTheme.SpaceXl;paper.style.paddingBottom=KarineTheme.SpaceLg;parent.Add(paper);return paper;
 }
 public static Button InboxItem(VisualElement parent,string title,string status,string date,bool unread,bool selected,Action select) {
  var row=Button_(parent,"",select,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  row.name="InboxItem";row.tooltip=title;row.style.minHeight=KarineTheme.Inbox.RowHeight;
  row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceMd;row.style.paddingRight=KarineTheme.SpaceSm;
  row.style.borderLeftWidth=KarineTheme.PrimaryEdgeWidth;row.style.borderLeftColor=unread?KarineTheme.Danger:KarineTheme.GlassLift;
  var ink=selected?KarineTheme.Paper.Ink:KarineTheme.Primary;
  Icon(row,"document",ink,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var text=new VisualElement();text.style.flexGrow=1;text.style.flexShrink=1;text.style.unityTextAlign=TextAnchor.MiddleLeft;row.Add(text);
  var name=Body_(text,title,KarineTheme.Inbox.BodySize);name.style.color=ink;name.style.marginBottom=KarineTheme.SpaceXs;
  var detail=Body_(text,status,KarineTheme.Dossier.MetaSize);detail.style.color=selected?KarineTheme.Paper.Faded:KarineTheme.Secondary;detail.style.marginBottom=0;
  if(!string.IsNullOrEmpty(date)) {var when=Technical(text,date,KarineTheme.Dossier.MetaSize);when.style.color=detail.style.color;when.style.marginBottom=0;}
  if(unread) {var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=KarineTheme.SpaceSm;dot.style.height=KarineTheme.SpaceSm;dot.style.flexShrink=0;dot.style.backgroundColor=KarineTheme.Danger;Round(dot,KarineTheme.SpaceSm);row.Add(dot);}
  return row;
 }
}
}
