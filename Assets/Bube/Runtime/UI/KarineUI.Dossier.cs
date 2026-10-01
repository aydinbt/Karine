using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 public static VisualElement DossierSheet(VisualElement parent) {
  var cover=new VisualElement();OfficePlace(cover,new Rect(5,12,88,83));
  cover.style.backgroundColor=KarineTheme.Paper.Folder;Border(cover,KarineTheme.PrimaryEdgeWidth,KarineTheme.Paper.FolderDeep);parent.Add(cover);
  for(int i=0;i<3;i++) {
   var leaf=new VisualElement();OfficePlace(leaf,new Rect(6+i*.4f,13+i*.4f,74,80));
   leaf.style.backgroundColor=KarineTheme.Paper.Tint;Border(leaf,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);parent.Add(leaf);
  }
  var paper=new VisualElement {name="DossierPaper"};OfficePlace(paper,KarineTheme.Dossier.Sheet);
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  var texture=Resources.Load<Texture2D>("Bube/Art/DossierPaper");
  if(texture!=null)paper.style.backgroundImage=new StyleBackground(texture);
  paper.style.paddingLeft=KarineTheme.SpaceXl;paper.style.paddingRight=KarineTheme.SpaceXl;
  paper.style.paddingTop=KarineTheme.SpaceLg;paper.style.paddingBottom=KarineTheme.SpaceLg;
  parent.Add(paper);return paper;
 }
 public static VisualElement DossierHeader(VisualElement parent,string back,string department,Action close) {
  var bar=new VisualElement();OfficePlace(bar,new Rect(0,0,100,11));
  bar.style.backgroundColor=KarineTheme.GlassDeep;bar.style.flexDirection=FlexDirection.Row;
  bar.style.alignItems=Align.Center;bar.style.paddingLeft=KarineTheme.SpaceXl;bar.style.paddingRight=KarineTheme.SpaceXl;parent.Add(bar);
  IconButton(bar,"nav_prev",close,back);
  var brand=KarineLogo.Hero(bar,KarineTheme.Dossier.LogoWidth);brand.style.marginLeft=KarineTheme.SpaceXl;brand.style.marginRight=KarineTheme.SpaceXl;
  Icon(bar,"people",KarineTheme.Primary,KarineTheme.IconSize);
  var label=Body_(bar,department,KarineTheme.Dossier.BodySize);label.style.marginLeft=KarineTheme.SpaceSm;label.style.marginBottom=0;label.style.flexGrow=1;
  return bar;
 }
 public static Button DossierTab(VisualElement parent,string icon,string title,bool active,Action action,bool animate=false) {
  var tab=PaperButton(parent,"",action,KarinePaperKind.Choice,active);
  tab.tooltip=title;tab.style.flexGrow=1;tab.style.minHeight=KarineTheme.TouchTarget;
  tab.style.marginBottom=KarineTheme.Dossier.TabGap;tab.style.flexDirection=FlexDirection.Row;tab.style.alignItems=Align.Center;
  tab.style.backgroundColor=active?KarineTheme.Paper.Sheet:KarineTheme.Paper.Edge;
  tab.style.borderLeftWidth=KarineTheme.PrimaryEdgeWidth;tab.style.borderLeftColor=active?KarineTheme.Paper.Stamp:KarineTheme.Paper.Edge;
  Icon(tab,icon,KarineTheme.Paper.Ink,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var text=Body_(tab,title,KarineTheme.Dossier.MetaSize);text.style.color=KarineTheme.Paper.Ink;text.style.marginBottom=0;
  if(active) {
   if(animate)KarineMotion.Run(tab,KarineTheme.Motion.PageSeconds,t=>tab.style.translate=new Translate(-KarineTheme.Motion.TabLift*t,0));
   else tab.style.translate=new Translate(-KarineTheme.Motion.TabLift,0);
  }
  return tab;
 }
 public static Label DossierText(VisualElement parent,string value,int size) {
  var text=Body_(parent,value,size);text.style.color=KarineTheme.Paper.Ink;text.style.marginBottom=KarineTheme.SpaceSm;return text;
 }
 public static void DossierTitle(VisualElement parent,string title) {
  var parts=title.Split(new[]{'—'},2);
  if(parts.Length>1) {
   var number=Technical(parent,parts[0].Trim(),KarineTheme.Dossier.BodySize);number.style.color=KarineTheme.Paper.Ink;
   var name=Subtitle(parent,parts[1].Trim(),KarineTheme.Dossier.TitleSize);name.style.color=KarineTheme.Paper.Ink;
   var rule=new VisualElement();rule.style.height=KarineTheme.PrimaryEdgeWidth;rule.style.width=Length.Percent(64);
   rule.style.backgroundColor=KarineTheme.Paper.Stamp;rule.style.marginBottom=KarineTheme.SpaceSm;parent.Add(rule);
  } else DossierText(parent,title,KarineTheme.Dossier.TitleSize);
 }
 public static void DossierMeta(VisualElement parent,string label,string value) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.paddingTop=KarineTheme.SpaceSm;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceSm;
  row.style.backgroundColor=KarineTheme.Paper.Tint;parent.Add(row);
  var key=DossierText(row,label,KarineTheme.Dossier.MetaSize);key.style.width=Length.Percent(40);
  var text=DossierText(row,value,KarineTheme.Dossier.MetaSize);text.style.width=Length.Percent(60);
 }
 public static VisualElement DossierRow(VisualElement parent) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginBottom=KarineTheme.SpaceLg;parent.Add(row);return row;
 }
 public static VisualElement DossierColumn(VisualElement row,float percent) {
  var column=new VisualElement();column.style.width=Length.Percent(percent);column.style.paddingRight=KarineTheme.SpaceLg;row.Add(column);return column;
 }
 public static void DossierPhoto(VisualElement parent,Texture2D texture,string caption) {
  var frame=new VisualElement();frame.style.paddingLeft=KarineTheme.SpaceSm;frame.style.paddingRight=KarineTheme.SpaceSm;
  frame.style.paddingTop=KarineTheme.SpaceSm;frame.style.backgroundColor=KarineTheme.Paper.Light;Border(frame,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);parent.Add(frame);
  var photo=new Image {image=texture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};photo.style.height=KarineTheme.Dossier.PhotoHeight;frame.Add(photo);
  DossierText(frame,caption,KarineTheme.Dossier.MetaSize);
 }
 public static void DossierPerson(VisualElement parent,Texture2D texture,string name,string info) {
  var row=DossierRow(parent);row.style.backgroundColor=KarineTheme.Paper.Tint;
  var portrait=new Image {image=texture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  portrait.style.width=KarineTheme.Dossier.PortraitSize;portrait.style.height=KarineTheme.Dossier.PortraitSize;portrait.style.flexShrink=0;row.Add(portrait);
  var body=new VisualElement();body.style.paddingLeft=KarineTheme.SpaceSm;body.style.flexGrow=1;row.Add(body);
  DossierText(body,name,KarineTheme.Dossier.BodySize);DossierText(body,info,KarineTheme.Dossier.MetaSize);
 }
}
}
