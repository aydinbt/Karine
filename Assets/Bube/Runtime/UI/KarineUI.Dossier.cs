using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 public static VisualElement DossierSheet(VisualElement parent) {
  var cover=new VisualElement {name="DossierFolder",pickingMode=PickingMode.Ignore};OfficePlace(cover,KarineTheme.Dossier.Folder);
  Stretched(cover,"Bube/UI/paper_folder");parent.Add(cover);
  var paper=new VisualElement {name="DossierPaper"};OfficePlace(paper,KarineTheme.Dossier.Sheet);
  Stretched(paper,"Bube/UI/paper_sheet");
  paper.style.paddingLeft=KarineTheme.SpaceXl;paper.style.paddingRight=KarineTheme.SpaceXl;
  paper.style.paddingTop=KarineTheme.SpaceLg;paper.style.paddingBottom=KarineTheme.SpaceLg;
  parent.Add(paper);return paper;
 }
 // Görseli öğenin tamamına gerer (kâğıt, karton, sekme kartı).
 public static void Stretched(VisualElement element,string resource) {
  var texture=Resources.Load<Texture2D>(resource);
  if(texture==null)return;
  element.style.backgroundImage=new StyleBackground(texture);
  element.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(Length.Percent(100),Length.Percent(100)));
 }
 static Image Mark(VisualElement parent,string resource,int size) {
  var mark=new Image {image=Resources.Load<Texture2D>(resource),scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
  mark.style.width=size;mark.style.height=size;mark.style.flexShrink=0;parent.Add(mark);return mark;
 }
 // Bölüm başlığı: simge, büyük harf başlık ve altında ince çizgi.
 public static void DossierSection(VisualElement parent,string icon,string title) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Paper.Faded;row.style.paddingBottom=KarineTheme.SpaceXs;
  row.style.marginBottom=KarineTheme.SpaceMd;parent.Add(row);
  Icon(row,icon,KarineTheme.Paper.Ink,KarineTheme.Dossier.SectionIcon).style.marginRight=KarineTheme.SpaceMd;
  var text=Subtitle(row,title.ToUpperInvariant(),KarineTheme.Dossier.BodySize);text.style.color=KarineTheme.Paper.Ink;text.style.marginBottom=0;
 }
 public static VisualElement DossierHeader(VisualElement parent,string back,string department,Action close) {
  var bar=new VisualElement();OfficePlace(bar,new Rect(0,0,100,11));
  bar.style.backgroundColor=KarineTheme.GlassDeep;bar.style.flexDirection=FlexDirection.Row;
  bar.style.alignItems=Align.Center;bar.style.paddingLeft=KarineTheme.SpaceXl;bar.style.paddingRight=KarineTheme.SpaceXl;parent.Add(bar);
  IconButton(bar,"nav_prev",close,back);
  var brand=KarineLogo.Hero(bar,KarineTheme.Dossier.LogoWidth);brand.style.marginLeft=KarineTheme.SpaceXl;brand.style.marginRight=KarineTheme.SpaceXl;
  Mark(bar,"Bube/UI/bube_logo_light",KarineTheme.IconButtonSize-KarineTheme.SpaceSm);
  var label=Body_(bar,department,KarineTheme.Dossier.BodySize);label.style.marginLeft=KarineTheme.SpaceSm;label.style.marginBottom=0;label.style.flexGrow=1;
  return bar;
 }
 public static Button DossierTab(VisualElement parent,string icon,string title,bool active,Action action,bool animate=false) {
  var tab=PaperButton(parent,"",action,KarinePaperKind.Choice,active);
  tab.tooltip=title;tab.style.flexGrow=1;tab.style.minHeight=KarineTheme.TouchTarget;
  tab.style.marginBottom=KarineTheme.Dossier.TabGap;tab.style.flexDirection=FlexDirection.Row;tab.style.alignItems=Align.Center;
  tab.style.backgroundColor=active?KarineTheme.Paper.Sheet:KarineTheme.Paper.Edge;
  Stretched(tab,"Bube/UI/paper_sheet");
  if(!active)tab.style.unityBackgroundImageTintColor=KarineTheme.Paper.Tint;
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
 public static VisualElement DossierMetaGrid(VisualElement parent) {
  var grid=new VisualElement {name="DossierMetaGrid"};grid.style.flexDirection=FlexDirection.Row;grid.style.flexWrap=Wrap.Wrap;
  grid.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Tint,.55f);Round(grid,KarineTheme.Radius);
  grid.style.paddingLeft=KarineTheme.SpaceMd;grid.style.paddingRight=KarineTheme.SpaceMd;
  grid.style.paddingTop=KarineTheme.SpaceMd;grid.style.marginTop=KarineTheme.SpaceMd;parent.Add(grid);return grid;
 }
 public static void DossierMeta(VisualElement parent,string icon,string label,string value) {
  var cell=new VisualElement();cell.style.flexDirection=FlexDirection.Row;cell.style.width=Length.Percent(50);
  cell.style.marginBottom=KarineTheme.SpaceMd;cell.style.paddingRight=KarineTheme.SpaceSm;parent.Add(cell);
  Icon(cell,icon,KarineTheme.Paper.Ink,KarineTheme.Dossier.MetaIcon).style.marginRight=KarineTheme.SpaceSm;
  var text=new VisualElement();text.style.flexShrink=1;cell.Add(text);
  var key=Subtitle(text,label,KarineTheme.Dossier.MetaSize);key.style.color=KarineTheme.Paper.Ink;key.style.marginBottom=0;
  DossierText(text,value,KarineTheme.Dossier.MetaSize).style.color=KarineTheme.Paper.Faded;
 }
 public static VisualElement DossierRow(VisualElement parent) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginBottom=KarineTheme.SpaceLg;parent.Add(row);return row;
 }
 public static VisualElement DossierColumn(VisualElement row,float percent) {
  var column=new VisualElement();column.style.width=Length.Percent(percent);column.style.paddingRight=KarineTheme.SpaceLg;row.Add(column);return column;
 }
 // Polaroid: açık renk çerçeve, altta yazı payı, hafif eğik, üstünde ataş.
 public static void DossierPhoto(VisualElement parent,Texture2D texture,string caption) {
  var frame=new VisualElement {name="DossierPolaroid"};frame.style.paddingLeft=KarineTheme.SpaceMd;frame.style.paddingRight=KarineTheme.SpaceMd;
  frame.style.paddingTop=KarineTheme.SpaceMd;frame.style.paddingBottom=KarineTheme.SpaceSm;
  frame.style.backgroundColor=KarineTheme.Paper.Light;Border(frame,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  frame.style.rotate=new Rotate(KarineTheme.Dossier.PhotoTilt);frame.style.marginTop=KarineTheme.SpaceMd;parent.Add(frame);
  var photo=new Image {image=texture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};photo.style.height=KarineTheme.Dossier.PhotoHeight;frame.Add(photo);
  var label=DossierText(frame,caption,KarineTheme.Dossier.MetaSize);label.style.minHeight=KarineTheme.Dossier.PhotoCaptionPad;label.style.marginTop=KarineTheme.SpaceSm;label.style.marginBottom=0;
  var clip=Resources.Load<Texture2D>("Bube/UI/paperclip");
  if(clip!=null) {
   var pin=new Image {image=clip,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   pin.style.position=Position.Absolute;pin.style.width=KarineTheme.Inbox.ClipWidth;
   pin.style.height=KarineTheme.Inbox.ClipWidth*clip.height/(float)clip.width;
   pin.style.right=Length.Percent(10);pin.style.top=-KarineTheme.SpaceXl;frame.Add(pin);
  }
  var tape=Resources.Load<Texture2D>("Bube/UI/tape");
  if(tape!=null) {
   var strip=new Image {image=tape,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   strip.style.position=Position.Absolute;strip.style.width=KarineTheme.Dossier.TapeWidth;
   strip.style.height=KarineTheme.Dossier.TapeWidth*tape.height/(float)tape.width;
   strip.style.left=-KarineTheme.SpaceLg;strip.style.top=KarineTheme.SpaceSm;strip.style.rotate=new Rotate(-38);frame.Add(strip);
  }
 }
 public static void DossierItem(VisualElement parent,Texture2D texture,string name,string detail) {
  var card=new VisualElement {name="DossierItem",pickingMode=PickingMode.Ignore};card.style.flexGrow=1;card.style.flexBasis=0;
  card.style.marginRight=KarineTheme.SpaceSm;parent.Add(card);
  var photo=new Image {image=texture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  photo.style.height=KarineTheme.Dossier.ItemHeight;Round(photo,KarineTheme.Radius);card.Add(photo);
  var title=Subtitle(card,name,KarineTheme.Dossier.MetaSize);title.style.color=KarineTheme.Paper.Ink;title.style.marginTop=KarineTheme.SpaceXs;title.style.marginBottom=0;
  DossierText(card,detail,KarineTheme.Dossier.MetaSize-2).style.color=KarineTheme.Paper.Faded;
 }
 public static void DossierPerson(VisualElement parent,Texture2D texture,string name,string info) {
  var row=DossierRow(parent);row.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Tint,.55f);
  row.style.alignItems=Align.Center;row.style.marginBottom=KarineTheme.SpaceSm;Round(row,KarineTheme.Radius);
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingTop=KarineTheme.SpaceSm;row.style.paddingBottom=KarineTheme.SpaceSm;
  var portrait=new Image {image=texture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  portrait.style.width=KarineTheme.Dossier.PortraitSize;portrait.style.height=KarineTheme.Dossier.PortraitSize;portrait.style.flexShrink=0;row.Add(portrait);
  var body=new VisualElement();body.style.paddingLeft=KarineTheme.SpaceSm;body.style.flexGrow=1;row.Add(body);
  DossierText(body,name,KarineTheme.Dossier.BodySize);DossierText(body,info,KarineTheme.Dossier.MetaSize);
 }
}
}
