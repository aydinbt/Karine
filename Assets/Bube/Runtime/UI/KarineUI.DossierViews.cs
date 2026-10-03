using System;
using UnityEngine;
using UnityEngine.UIElements;
using D = Bube.KarineTheme.Dossier;
namespace Bube {
// Dosya kâğıdının içi (3 Ekim 2026 maketleri): daktilo başlık, künye, döküm satırları,
// zaman çizgisi, eklenebilir olay şeridi ve kareli defter yaprağı.
public static partial class KarineUI {
 static Font typewriterBold;
 static Font TypewriterBold=>typewriterBold=typewriterBold??Resources.Load<Font>("Bube/Fonts/IBMPlexMono-SemiBold");
 static Label Typed(VisualElement parent,string value,int size,bool bold=false,Color? color=null) {
  var label=Write(parent,value,color??KarineTheme.Paper.Ink,size,bold?TypewriterBold:Typewriter);label.style.marginBottom=0;return label;
 }
 // Kâğıt başlığı: solda küçük üst satır, büyük daktilo başlık ve çizgi; sağda polaroid.
 // Dönen sütun künye satırları içindir.
 public static VisualElement DossierPageHead(VisualElement paper,string kicker,string title,Texture2D photo,string icon=null,string date=null) {
  var head=new VisualElement {name="DossierPageHead"};head.style.flexDirection=FlexDirection.Row;head.style.flexShrink=0;
  head.style.marginBottom=KarineTheme.SpaceMd;paper.Add(head);
  var left=new VisualElement();left.style.flexGrow=1;left.style.flexShrink=1;left.style.minWidth=0;left.style.paddingRight=KarineTheme.SpaceLg;head.Add(left);
  if(!string.IsNullOrEmpty(kicker))Typed(left,kicker.ToUpper(Tr),D.PageKickerSize,true).style.marginBottom=KarineTheme.SpaceXs;
  var line=new VisualElement();line.style.flexDirection=FlexDirection.Row;line.style.alignItems=Align.Center;left.Add(line);
  if(!string.IsNullOrEmpty(icon))Icon(line,icon,KarineTheme.Paper.Ink,D.PageTitleSize).style.marginRight=KarineTheme.SpaceMd;
  var t=Typed(line,title.ToUpper(Tr),D.PageTitleSize,true);t.style.flexGrow=1;t.style.flexShrink=1;
  if(!string.IsNullOrEmpty(date))Typed(line,date.ToUpper(Tr),D.PageKickerSize,false,KarineTheme.Paper.Faded).style.alignSelf=Align.FlexEnd;
  PaperRule(left,false);
  if(photo!=null)DossierPolaroid(head,photo);
  return left;
 }
 public static void PaperRule(VisualElement parent,bool doubled) {
  var rule=new VisualElement {pickingMode=PickingMode.Ignore};rule.style.height=doubled?4:1;rule.style.flexShrink=0;
  rule.style.borderTopWidth=1;rule.style.borderTopColor=KarineTheme.Paper.Ink;
  if(doubled){rule.style.borderBottomWidth=1;rule.style.borderBottomColor=KarineTheme.Paper.Ink;}
  rule.style.marginTop=KarineTheme.SpaceSm;rule.style.marginBottom=KarineTheme.SpaceMd;parent.Add(rule);
 }
 // Künye satırı: "ANAHTAR  :  değer", daktilo.
 public static void DossierKeyValue(VisualElement parent,string key,string value) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginBottom=KarineTheme.SpaceXs;parent.Add(row);
  var k=Typed(row,key.ToUpper(Tr),D.PageMetaSize,true);k.style.width=D.KeyWidth;k.style.flexShrink=0;
  Typed(row,":  ",D.PageMetaSize);
  Typed(row,value,D.PageMetaSize).style.flexShrink=1;
 }
 // Döküm satırı: konuşan kalın, iki nokta, metin; `stamp` verilirse yanına kırmızı mühür.
 public static VisualElement DossierLine(VisualElement parent,string speaker,string text,string stamp=null) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginBottom=KarineTheme.SpaceMd;parent.Add(row);
  var s=Typed(row,speaker.ToUpper(Tr),D.PageBodySize,true);s.style.width=D.SpeakerWidth;s.style.flexShrink=0;
  Typed(row,":  ",D.PageBodySize);
  var body=Typed(row,text,D.PageBodySize);body.style.flexShrink=1;body.style.flexGrow=1;
  if(!string.IsNullOrEmpty(stamp)) {
   var seal=Typed(row,stamp.ToUpper(Tr),D.StampSize,true,KarineTheme.Danger);seal.style.alignSelf=Align.Center;seal.style.flexShrink=0;
   Border(seal,2,KarineTheme.Danger);Round(seal,KarineTheme.Radius);seal.style.marginLeft=KarineTheme.SpaceMd;
   seal.style.paddingLeft=KarineTheme.SpaceSm;seal.style.paddingRight=KarineTheme.SpaceSm;seal.style.rotate=new Rotate(D.StampTilt);seal.style.opacity=.85f;
  }
  return row;
 }
 public static Label DossierParagraph(VisualElement parent,string text,Color? color=null) {
  var p=Typed(parent,text,D.PageBodySize,false,color);p.style.marginBottom=KarineTheme.SpaceMd;p.style.whiteSpace=WhiteSpace.Normal;return p;
 }
 // Polaroid: açık çerçeve, hafif eğik, üstünde ataş.
 public static VisualElement DossierPolaroid(VisualElement parent,Texture2D texture) {
  var frame=new VisualElement {name="DossierPolaroid",pickingMode=PickingMode.Ignore};frame.style.width=D.PolaroidWidth;frame.style.flexShrink=0;
  frame.style.paddingLeft=KarineTheme.SpaceSm;frame.style.paddingRight=KarineTheme.SpaceSm;frame.style.paddingTop=KarineTheme.SpaceSm;frame.style.paddingBottom=KarineTheme.SpaceLg;
  frame.style.backgroundColor=KarineTheme.Paper.Light;Border(frame,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);frame.style.rotate=new Rotate(D.PolaroidTilt);
  parent.Add(frame);
  var photo=new Image {image=texture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};photo.style.height=D.PolaroidHeight;frame.Add(photo);
  var clip=Resources.Load<Texture2D>("Bube/UI/paperclip");
  if(clip!=null) {
   var pin=new Image {image=clip,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   pin.style.position=Position.Absolute;pin.style.width=KarineTheme.Inbox.ClipWidth;
   pin.style.height=KarineTheme.Inbox.ClipWidth*clip.height/(float)clip.width;
   pin.style.right=Length.Percent(12);pin.style.top=-KarineTheme.SpaceXl;frame.Add(pin);
  }
  return frame;
 }
 // Zaman çizgisi satırı: solda saat, ortada düğüm ve dikey çizgi, sağda not kartı, en sağda koyu eksi.
 public static VisualElement DossierTimeRow(VisualElement parent,string time,string note,bool first,bool last,bool selected,Action select,Action remove,string removeTitle) {
  var row=new VisualElement {name="TimeRow"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=D.LineRowHeight;row.style.marginBottom=KarineTheme.SpaceSm;parent.Add(row);
  var t=Typed(row,time,D.LineTimeSize,true);t.style.width=D.KeyWidth-KarineTheme.SpaceXl;t.style.flexShrink=0;
  var rail=new VisualElement {pickingMode=PickingMode.Ignore};rail.style.width=D.LineNode+KarineTheme.SpaceLg;rail.style.alignSelf=Align.Stretch;
  rail.style.alignItems=Align.Center;rail.style.justifyContent=Justify.Center;row.Add(rail);
  var line=new VisualElement {pickingMode=PickingMode.Ignore};line.style.position=Position.Absolute;line.style.width=3;
  line.style.top=first?Length.Percent(50):-KarineTheme.SpaceSm;line.style.bottom=last?Length.Percent(50):-KarineTheme.SpaceSm;
  line.style.backgroundColor=KarineTheme.Paper.Ink;rail.Add(line);
  var node=new VisualElement {pickingMode=PickingMode.Ignore};node.style.width=D.LineNode;node.style.height=D.LineNode;Round(node,D.LineNode/2);
  node.style.backgroundColor=selected?KarineTheme.Accent:KarineTheme.Paper.Light;Border(node,3,selected?KarineTheme.Paper.Stamp:KarineTheme.Paper.Ink);rail.Add(node);
  var card=new Button(Sounded(select)) {tooltip=note};card.style.flexGrow=1;card.style.flexShrink=1;card.style.minHeight=D.LineRowHeight;
  card.style.marginLeft=KarineTheme.SpaceSm;card.style.marginRight=0;card.style.paddingLeft=KarineTheme.SpaceLg;card.style.justifyContent=Justify.Center;
  Unskin(card,KarineTheme.Alpha(KarineTheme.Paper.Tint,selected?.6f:.35f));Round(card,KarineTheme.Radius);row.Add(card);
  var n=Typed(card,note,D.PageMetaSize);n.style.whiteSpace=WhiteSpace.Normal;
  if(remove!=null)PlusButton(row,"−",remove,removeTitle,true);
  return row;
 }
 // Kâğıdın altındaki koyu şerit: başlık, açıklama ve yatay kayan eklenebilir kartlar.
 public static VisualElement DossierAddStrip(VisualElement paper,string title,string help) {
  var strip=new VisualElement {name="DossierAddStrip"};strip.style.flexShrink=0;strip.style.marginTop=KarineTheme.SpaceMd;
  strip.style.marginLeft=-KarineTheme.SpaceLg;strip.style.marginRight=-KarineTheme.SpaceLg;
  strip.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.96f);Round(strip,KarineTheme.Radius);
  strip.style.paddingLeft=KarineTheme.SpaceMd;strip.style.paddingRight=KarineTheme.SpaceMd;strip.style.paddingTop=KarineTheme.SpaceSm;strip.style.paddingBottom=KarineTheme.SpaceMd;
  paper.Add(strip);
  var h=Write(strip,title.ToUpper(Tr),KarineTheme.Primary,D.RowTitleSize,Heading);h.style.marginBottom=0;h.style.letterSpacing=1;
  var s=Body_(strip,help,D.RowSubSize-1);s.style.color=KarineTheme.Secondary;s.style.marginBottom=KarineTheme.SpaceSm;
  var scroll=new KarineScrollView(ScrollViewMode.Horizontal);scroll.contentContainer.style.flexDirection=FlexDirection.Row;strip.Add(scroll);
  return scroll.contentContainer;
 }
 public static void DossierAddCard(VisualElement strip,string time,string title,Action add,string addTitle) {
  var card=new VisualElement {name="DossierAddCard"};card.style.width=D.AddCardWidth;card.style.flexShrink=0;card.style.flexDirection=FlexDirection.Row;
  card.style.alignItems=Align.Center;card.style.marginRight=KarineTheme.SpaceMd;card.style.paddingLeft=KarineTheme.SpaceMd;card.style.paddingRight=KarineTheme.SpaceSm;
  card.style.paddingTop=KarineTheme.SpaceSm;card.style.paddingBottom=KarineTheme.SpaceSm;
  card.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.8f);Border(card,KarineTheme.BorderWidth,KarineTheme.Border);Round(card,KarineTheme.Radius);strip.Add(card);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;card.Add(words);
  var t=Write(words,time,KarineTheme.Primary,D.RowTitleSize,Heading);t.style.marginBottom=0;
  var n=Body_(words,title,D.RowSubSize-1);n.style.color=KarineTheme.Secondary;n.style.marginBottom=0;
  PlusButton(card,"+",add,addTitle);
 }
 // Kareli defter yaprağı: solda spiral, zeminde ızgara. Dönen öğe yaprağın içidir.
 public static VisualElement DossierNotebookSheet(VisualElement paper) {
  paper.style.backgroundImage=new StyleBackground(GridTile());
  paper.style.backgroundSize=new StyleBackgroundSize(new BackgroundSize(D.GridStep,D.GridStep));
  paper.style.backgroundRepeat=new StyleBackgroundRepeat(new BackgroundRepeat(Repeat.Repeat,Repeat.Repeat));
  paper.style.backgroundColor=KarineTheme.Paper.Light;paper.style.paddingLeft=D.SpiralWidth+KarineTheme.SpaceXl;paper.style.overflow=Overflow.Visible;
  var spiral=new VisualElement {name="NotebookSpiral",pickingMode=PickingMode.Ignore};spiral.style.position=Position.Absolute;
  spiral.style.left=-D.SpiralWidth/2;spiral.style.top=KarineTheme.SpaceLg;spiral.style.bottom=KarineTheme.SpaceLg;spiral.style.width=D.SpiralWidth;
  spiral.style.justifyContent=Justify.SpaceBetween;paper.Add(spiral);
  for(int i=0;i<16;i++) {
   var ring=new VisualElement {pickingMode=PickingMode.Ignore};ring.style.height=D.GridStep/2;ring.style.width=D.SpiralWidth;
   Round(ring,D.GridStep/4);Border(ring,3,KarineTheme.Paper.FolderDeep);ring.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.4f);spiral.Add(ring);
  }
  return paper;
 }
 static Texture2D gridTile;
 static Texture2D GridTile() {
  if(gridTile!=null)return gridTile;
  const int n=32;gridTile=new Texture2D(n,n,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Repeat,filterMode=FilterMode.Bilinear,name="NotebookGrid"};
  var line=KarineTheme.Alpha(KarineTheme.Paper.Edge,.45f);var clear=KarineTheme.Alpha(KarineTheme.Paper.Edge,0);
  var px=new Color[n*n];
  for(int y=0;y<n;y++)for(int x=0;x<n;x++)px[y*n+x]=x==0||y==0?line:clear;
  gridTile.SetPixels(px);gridTile.Apply();return gridTile;
 }
 // Defterin altındaki koyu araç şeridi; düğmeler `SettingsFooterButton` gibi çağıran tarafından eklenir.
 public static VisualElement DossierToolStrip(VisualElement paper) {
  var strip=new VisualElement {name="DossierToolStrip"};strip.style.flexShrink=0;strip.style.flexDirection=FlexDirection.Row;strip.style.marginTop=KarineTheme.SpaceMd;
  strip.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.96f);Round(strip,KarineTheme.Radius);
  strip.style.paddingLeft=KarineTheme.SpaceSm;strip.style.paddingRight=KarineTheme.SpaceSm;strip.style.paddingTop=KarineTheme.SpaceSm;strip.style.paddingBottom=KarineTheme.SpaceSm;
  paper.Add(strip);return strip;
 }
 public static Button DossierStripButton(VisualElement strip,string icon,string title,Action action,bool primary=false) {
  var button=new Button(Sounded(action)) {tooltip=title};button.style.flexGrow=1;button.style.flexBasis=0;button.style.height=KarineTheme.TouchTarget+KarineTheme.SpaceXs;
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  button.style.marginLeft=KarineTheme.SpaceXs;button.style.marginRight=KarineTheme.SpaceXs;
  Unskin(button,KarineTheme.Alpha(primary?KarineTheme.Accent:KarineTheme.Background,primary?.16f:.9f));
  Border(button,KarineTheme.BorderWidth,primary?KarineTheme.Accent:KarineTheme.Border);Round(button,KarineTheme.Radius);
  if(!string.IsNullOrEmpty(icon))Icon(button,icon,KarineTheme.Primary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(button,title.ToUpper(Tr),KarineTheme.Primary,D.RowTitleSize,Heading);l.style.marginBottom=0;l.style.letterSpacing=1;
  strip.Add(button);return button;
 }
}
}
