using System;
using UnityEngine;
using UnityEngine.UIElements;
using B = Bube.KarineTheme.CareerBoard;

namespace Bube {
// Kariyer panosu, 3 Ekim 2026 maketi (Docs/Reference/UI_CAREER_2026-10.png):
// üstte sekmeler, geniş ilerleme paneli, dört sayaç, sonuç halkası ve ülke
// ilerlemesi. Yalnız kayıtlı kariyerden okunan sayılar gösterilir; maketteki
// süre ve madalya gibi oyunda karşılığı olmayan alanlar yoktur.
public static partial class KarineUI {

 public static VisualElement CareerTabs(VisualElement parent,params (string label,bool selected,Action click)[] tabs) {
  var row=new VisualElement {name="CareerTabs"};row.style.flexDirection=FlexDirection.Row;row.style.flexShrink=0;
  row.style.marginBottom=KarineTheme.SpaceMd;parent.Add(row);
  foreach(var tab in tabs) {
   var button=new Button(Sounded(tab.click)) {name="CareerTab"};
   button.style.width=B.TabWidth;button.style.height=B.TabHeight;button.style.marginLeft=0;button.style.marginTop=0;button.style.marginBottom=0;
   button.style.marginRight=KarineTheme.SpaceXs;
   Unskin(button,tab.selected?KarineTheme.Alpha(KarineTheme.Background,.92f):KarineTheme.Alpha(KarineTheme.Panel,.92f));
   Border(button,KarineTheme.BorderWidth,KarineTheme.Border);Round(button,KarineTheme.Radius);
   if(tab.selected){button.style.borderBottomWidth=3;button.style.borderBottomColor=KarineTheme.Accent;}
   var text=Write(button,tab.label.ToUpper(Tr),tab.selected?KarineTheme.Accent:KarineTheme.Secondary,B.TabSize,Heading);
   text.style.marginBottom=0;text.style.letterSpacing=1;text.style.unityTextAlign=TextAnchor.MiddleCenter;text.style.flexGrow=1;
   row.Add(button);
  }
  return row;
 }

 // Koyu panel: başlık, altında ince çizgi.
 public static VisualElement BoardPanel(VisualElement parent,string title) {
  var panel=new VisualElement();panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Panel,.9f);
  Border(panel,KarineTheme.BorderWidth,KarineTheme.Border);Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=KarineTheme.SpaceLg;panel.style.paddingRight=KarineTheme.SpaceLg;
  panel.style.paddingTop=KarineTheme.SpaceSm;panel.style.paddingBottom=KarineTheme.SpaceMd;
  if(!string.IsNullOrEmpty(title)) {
   var head=Write(panel,title.ToUpper(Tr),KarineTheme.Primary,B.PanelTitleSize,Heading);head.style.marginBottom=KarineTheme.SpaceXs;head.style.letterSpacing=1;
   var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Border;rule.style.marginBottom=KarineTheme.SpaceSm;panel.Add(rule);
  }
  parent.Add(panel);return panel;
 }

 public static void BoardFigure(VisualElement parent,string value,string label,string sub,bool divider) {
  var cell=new VisualElement();cell.style.flexGrow=1;cell.style.flexBasis=0;cell.style.alignItems=Align.Center;
  if(divider){cell.style.borderLeftWidth=1;cell.style.borderLeftColor=KarineTheme.Border;}
  Write(cell,value,KarineTheme.Primary,B.FigureSize,Heading).style.marginBottom=0;
  var l=Write(cell,label.ToUpper(Tr),KarineTheme.Primary,B.FigureLabelSize,Heading);l.style.marginBottom=0;l.style.marginTop=-KarineTheme.SpaceXs;l.style.letterSpacing=1;
  if(!string.IsNullOrEmpty(sub)){var s=Technical(cell,sub,B.FigureSubSize);s.style.color=KarineTheme.Secondary;s.style.marginBottom=0;}
  parent.Add(cell);
 }

 public static VisualElement BoardBar(VisualElement parent,float ratio,int height) {
  var track=new VisualElement {name="BoardBar",pickingMode=PickingMode.Ignore};track.style.height=height;
  track.style.backgroundColor=KarineTheme.Panel2;Round(track,height/2);track.style.overflow=Overflow.Hidden;
  var fill=new VisualElement {pickingMode=PickingMode.Ignore};fill.style.height=Length.Percent(100);
  fill.style.width=Length.Percent(Mathf.Clamp01(ratio)*100);fill.style.backgroundColor=KarineTheme.Accent;Round(fill,height/2);
  track.Add(fill);parent.Add(track);return track;
 }

 public static VisualElement BoardTile(VisualElement parent,string icon,string value,string label) {
  var tile=new VisualElement {name="CareerTile"};tile.style.flexDirection=FlexDirection.Row;tile.style.alignItems=Align.Center;
  tile.style.flexGrow=1;tile.style.flexBasis=0;tile.style.height=B.TileHeight;tile.style.marginRight=KarineTheme.SpaceSm;
  tile.style.paddingLeft=KarineTheme.SpaceLg;tile.style.paddingRight=KarineTheme.SpaceSm;
  tile.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Panel,.9f);Border(tile,KarineTheme.BorderWidth,KarineTheme.Border);Round(tile,KarineTheme.Radius);
  Icon(tile,icon,KarineTheme.Primary,B.TileIcon).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement();words.style.flexShrink=1;words.style.minWidth=0;tile.Add(words);
  // Bebas'ın satır yüksekliği geniş: sayı ile etiket arasındaki boşluk kapatılır, blok ikonla ortalanır.
  words.style.justifyContent=Justify.Center;
  var number=Write(words,value,KarineTheme.Primary,B.TileNumberSize,Heading);number.style.marginBottom=-KarineTheme.SpaceSm;
  number.style.unityTextAlign=TextAnchor.MiddleLeft;
  var l=Write(words,label.ToUpper(Tr),KarineTheme.Primary,B.TileLabelSize,Heading);l.style.marginBottom=0;l.style.letterSpacing=1;
  l.style.whiteSpace=WhiteSpace.NoWrap;l.style.overflow=Overflow.Hidden;l.style.textOverflow=TextOverflow.Ellipsis;l.style.unityTextAlign=TextAnchor.UpperLeft;
  parent.Add(tile);return tile;
 }

 public static void BoardLegend(VisualElement parent,Color color,string label,int count,int percent) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.paddingTop=KarineTheme.SpaceXs;row.style.paddingBottom=KarineTheme.SpaceXs;parent.Add(row);
  var dot=new VisualElement();dot.style.width=14;dot.style.height=14;Round(dot,7);dot.style.backgroundColor=color;dot.style.marginRight=KarineTheme.SpaceSm;row.Add(dot);
  var l=Body_(row,label,B.LegendSize);l.style.flexGrow=1;l.style.flexShrink=1;l.style.minWidth=0;l.style.marginBottom=0;
  l.style.whiteSpace=WhiteSpace.NoWrap;l.style.overflow=Overflow.Hidden;l.style.textOverflow=TextOverflow.Ellipsis;
  var n=Write(row,count.ToString(),KarineTheme.Primary,B.LegendSize+4,Heading);n.style.marginBottom=0;n.style.width=28;n.style.unityTextAlign=TextAnchor.MiddleRight;
  var p=Write(row,"%"+percent,KarineTheme.Primary,B.LegendSize+4,Heading);p.style.marginBottom=0;p.style.width=48;p.style.unityTextAlign=TextAnchor.MiddleRight;
 }

 public static Button BoardCountry(VisualElement parent,string id,Texture2D art,string name,string count,float ratio,bool locked,Action click) {
  var row=new Button(Sounded(click)) {name="CareerWorld-"+id};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.height=B.CountryRow;row.style.flexShrink=0;row.style.marginLeft=0;row.style.marginRight=0;row.style.marginTop=0;
  row.style.marginBottom=KarineTheme.SpaceXs;row.style.paddingLeft=0;row.style.paddingRight=0;row.style.paddingTop=0;row.style.paddingBottom=0;
  Unskin(row,Color.clear);row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Border;
  var photo=new VisualElement {pickingMode=PickingMode.Ignore};photo.style.width=B.CountryPhotoWidth;photo.style.height=B.CountryRow-KarineTheme.SpaceXs;
  photo.style.overflow=Overflow.Hidden;photo.style.alignItems=Align.Center;photo.style.justifyContent=Justify.Center;Round(photo,KarineTheme.Radius);row.Add(photo);
  var image=CountryPostcard(photo,id,art);image.style.position=Position.Absolute;image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  image.style.opacity=locked?.3f:1f;
  if(locked)Icon(photo,"lock",KarineTheme.Primary,KarineTheme.IconSize+4);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.paddingLeft=KarineTheme.SpaceLg;words.style.paddingRight=KarineTheme.SpaceSm;row.Add(words);
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.Center;top.style.marginBottom=KarineTheme.SpaceXs;words.Add(top);
  var n=Write(top,name.ToUpper(Tr),KarineTheme.Primary,B.CountryNameSize,Heading);n.style.flexGrow=1;n.style.marginBottom=0;n.style.letterSpacing=1;n.style.unityTextAlign=TextAnchor.MiddleLeft;
  var c=Write(top,count,KarineTheme.Primary,B.CountryNameSize,Heading);c.style.marginBottom=0;c.style.unityTextAlign=TextAnchor.MiddleRight;
  BoardBar(words,ratio,B.BarHeight-4);
  parent.Add(row);return row;
 }

 // Halka: dilimler sayılarla orantılı; hiç kayıt yoksa boş bir iz. Ortada "TOPLAM / n / VAKA".
 public sealed class CareerRing:VisualElement {
  readonly int[] counts;readonly Color[] colors;
  public CareerRing(int[] values,Color[] tones,string over,string total,string under) {
   counts=values;colors=tones;pickingMode=PickingMode.Ignore;
   style.width=B.Ring;style.height=B.Ring;style.flexShrink=0;
   style.alignItems=Align.Center;style.justifyContent=Justify.Center;
   generateVisualContent+=Draw;
   Write(this,over,KarineTheme.Primary,B.TileLabelSize,Heading).style.marginBottom=0;
   Write(this,total,KarineTheme.Primary,B.RingTotalSize,Heading).style.marginBottom=0;
   Write(this,under,KarineTheme.Primary,B.TileLabelSize,Heading).style.marginBottom=0;
  }
  void Draw(MeshGenerationContext context) {
   var p=context.painter2D;var r=contentRect;var c=r.center;float radius=Mathf.Min(r.width,r.height)*.5f-B.RingWidth*.5f;
   p.lineWidth=B.RingWidth;p.lineCap=LineCap.Butt;
   p.strokeColor=KarineTheme.Panel2;p.BeginPath();p.Arc(c,radius,0,360);p.Stroke();
   int sum=0;foreach(var v in counts)sum+=v;if(sum==0)return;
   float angle=-90;
   for(int i=0;i<counts.Length;i++){if(counts[i]==0)continue;float sweep=360f*counts[i]/sum;
    p.strokeColor=colors[i];p.BeginPath();p.Arc(c,radius,angle,angle+sweep);p.Stroke();angle+=sweep;}
  }
 }
}
}
