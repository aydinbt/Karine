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
  var mark=new Image {image=Resources.Load<Texture2D>("Bube/UI/bube_logo"),scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
  mark.style.width=KarineTheme.Inbox.BrandLogo;mark.style.height=KarineTheme.Inbox.BrandLogo;
  mark.style.marginRight=KarineTheme.SpaceMd;mark.style.flexShrink=0;row.Add(mark);
  Body_(row,brand,KarineTheme.Inbox.BodySize);return panel;
 }
 public static VisualElement InboxPaper(VisualElement parent) {
  var folder=new VisualElement {name="InboxFolder",pickingMode=PickingMode.Ignore};OfficePlace(folder,KarineTheme.Inbox.Folder);
  folder.style.backgroundImage=new StyleBackground(Resources.Load<Texture2D>("Bube/UI/paper_folder"));
  folder.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(Length.Percent(100),Length.Percent(100)));
  parent.Add(folder);
  var paper=new VisualElement {name="InboxPaper"};OfficePlace(paper,KarineTheme.Inbox.Paper);
  paper.style.backgroundImage=new StyleBackground(Resources.Load<Texture2D>("Bube/UI/paper_sheet"));
  paper.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(Length.Percent(100),Length.Percent(100)));
  paper.style.paddingLeft=KarineTheme.SpaceXl;paper.style.paddingRight=KarineTheme.SpaceXl;
  paper.style.paddingTop=KarineTheme.SpaceXl;paper.style.paddingBottom=KarineTheme.SpaceLg;parent.Add(paper);
  var clip=Resources.Load<Texture2D>("Bube/UI/paperclip");
  if(clip!=null) {
   var pin=new Image {name="InboxClip",image=clip,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   pin.style.position=Position.Absolute;pin.style.width=KarineTheme.Inbox.ClipWidth;
   pin.style.height=KarineTheme.Inbox.ClipWidth*clip.height/(float)clip.width;
   pin.style.right=Length.Percent(4);pin.style.top=-KarineTheme.SpaceLg;paper.Add(pin);
  }
  return paper;
 }
 // Masanın üstüne gece ofisi sahnesi; hafif karartma panelleri okunur tutar.
 public static void InboxScene(VisualElement parent) {
  var scene=new VisualElement {name="InboxScene"};
  scene.style.position=Position.Absolute;scene.style.left=0;scene.style.right=0;scene.style.top=0;scene.style.bottom=0;
  scene.style.backgroundColor=KarineTheme.Background;
  scene.style.backgroundImage=new StyleBackground(Resources.Load<Texture2D>("Bube/UI/bg_office"));
  scene.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Cover));
  parent.Add(scene);
  var veil=new VisualElement {pickingMode=PickingMode.Ignore};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(KarineTheme.Inbox.SceneVeil);scene.Add(veil);
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
