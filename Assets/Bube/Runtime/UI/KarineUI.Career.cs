using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 // Kariyer ekranı (yeni görünüm, 2 Ekim 2026): kutular, sayaç kartları, halka grafik ve dünya sıralaması.
 public static VisualElement CareerBox(VisualElement parent,string title) {
  var box=new VisualElement();box.style.backgroundColor=KarineTheme.GlassDeep;Border(box,KarineTheme.BorderWidth,KarineTheme.Panel2);Round(box,KarineTheme.Radius);
  box.style.paddingLeft=KarineTheme.SpaceMd;box.style.paddingRight=KarineTheme.SpaceMd;box.style.paddingTop=KarineTheme.SpaceSm;box.style.paddingBottom=KarineTheme.SpaceSm;
  if(!string.IsNullOrEmpty(title)) Subtitle(box,title,KarineTheme.Career.HeadingSize).style.marginBottom=KarineTheme.SpaceSm;
  parent.Add(box);return box;
 }
 public static VisualElement CareerStat(VisualElement parent,string icon,string label,string value) {
  var box=CareerBox(parent,null);box.style.flexGrow=1;box.style.flexBasis=0;box.style.marginRight=KarineTheme.SpaceSm;
  Icon(box,icon,KarineTheme.Primary,KarineTheme.IconSize+KarineTheme.SpaceXs);
  var l=Body_(box,label,KarineTheme.CaseBrowser.SmallSize+1);l.style.color=KarineTheme.Secondary;l.style.marginBottom=0;l.style.marginTop=KarineTheme.SpaceXs;
  Title(box,value,KarineTheme.Career.ValueSize).style.marginBottom=0;
  return box;
 }
 public static void CareerFigure(VisualElement parent,string value,string label,bool divider) {
  var cell=new VisualElement();cell.style.flexGrow=1;cell.style.paddingLeft=KarineTheme.SpaceMd;
  if(divider){cell.style.borderLeftWidth=1;cell.style.borderLeftColor=KarineTheme.Panel2;}
  Title(cell,value,KarineTheme.Career.ValueSize).style.marginBottom=0;
  var l=Body_(cell,label,KarineTheme.CaseBrowser.SmallSize+1);l.style.color=KarineTheme.Secondary;l.style.marginBottom=0;
  parent.Add(cell);
 }
 public static void CareerLegend(VisualElement parent,Color color,string label,int count) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Panel2;row.style.paddingTop=KarineTheme.SpaceXs;row.style.paddingBottom=KarineTheme.SpaceXs;parent.Add(row);
  var dot=new VisualElement();dot.style.width=10;dot.style.height=10;Round(dot,5);dot.style.backgroundColor=color;dot.style.marginRight=KarineTheme.SpaceSm;row.Add(dot);
  var l=Body_(row,label,KarineTheme.CaseBrowser.TextSize);l.style.flexGrow=1;l.style.marginBottom=0;
  Technical(row,count.ToString(),KarineTheme.CaseBrowser.TextSize).style.marginBottom=0;
 }
 public static Button CareerWorldRow(VisualElement parent,int rank,string id,Texture2D art,string name,string count,float progress,bool locked,Action click) {
  var row=new Button(Sounded(click)) {name="CareerWorld-"+id};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.backgroundColor=Color.clear;Border(row,0,Color.clear);row.style.marginLeft=0;row.style.marginRight=0;row.style.paddingLeft=0;row.style.paddingRight=0;
  row.style.marginBottom=KarineTheme.SpaceXs;row.style.minHeight=KarineTheme.Career.WorldRow;
  var no=Technical(row,rank.ToString(),KarineTheme.CaseBrowser.TextSize);no.style.width=KarineTheme.Career.WorldRank;no.style.height=KarineTheme.Career.WorldRow;no.style.marginBottom=0;
  no.style.unityTextAlign=TextAnchor.MiddleCenter;no.style.backgroundColor=rank==1?KarineTheme.Primary:KarineTheme.Panel2;no.style.color=rank==1?KarineTheme.OnPrimary:KarineTheme.Primary;
  var thumb=new VisualElement();thumb.style.width=KarineTheme.Career.WorldThumb;thumb.style.height=KarineTheme.Career.WorldRow;thumb.style.overflow=Overflow.Hidden;
  thumb.style.alignItems=Align.Center;thumb.style.justifyContent=Justify.Center;row.Add(thumb);
  CountryPostcard(thumb,id,art).style.opacity=locked?.45f:1f;
  if(locked) Icon(thumb,IconOr("lock","close"),KarineTheme.Primary,KarineTheme.IconSize);
  var words=new VisualElement();words.style.flexGrow=1;words.style.paddingLeft=KarineTheme.SpaceSm;words.style.paddingRight=KarineTheme.SpaceSm;row.Add(words);
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;words.Add(top);
  var n=Body_(top,name,KarineTheme.CaseBrowser.TextSize);n.style.flexGrow=1;n.style.marginBottom=0;
  var c=Technical(top,count,KarineTheme.CaseBrowser.SmallSize);c.style.color=KarineTheme.Secondary;c.style.marginBottom=0;
  BrowserMeter(words,progress);
  Icon(row,"nav_next",KarineTheme.Secondary,KarineTheme.IconSize).pickingMode=PickingMode.Ignore;
  parent.Add(row);return row;
 }
 // Halka: dilimler sayılarla orantılı; hiç kayıt yoksa boş bir iz.
 public sealed class CareerRing:VisualElement {
  readonly int[] counts;readonly Color[] colors;
  public CareerRing(int[] values,Color[] tones,string total,string caption) {
   counts=values;colors=tones;pickingMode=PickingMode.Ignore;
   style.width=KarineTheme.Career.Ring;style.height=KarineTheme.Career.Ring;style.flexShrink=0;
   style.alignItems=Align.Center;style.justifyContent=Justify.Center;
   generateVisualContent+=Draw;
   Title(this,total,KarineTheme.Career.ValueSize).style.marginBottom=0;
   Body_(this,caption,KarineTheme.CaseBrowser.SmallSize).style.marginBottom=0;
  }
  void Draw(MeshGenerationContext context) {
   var p=context.painter2D;var r=contentRect;var c=r.center;float radius=Mathf.Min(r.width,r.height)*.5f-KarineTheme.Career.RingWidth*.5f;
   p.lineWidth=KarineTheme.Career.RingWidth;p.lineCap=LineCap.Butt;
   p.strokeColor=KarineTheme.Panel2;p.BeginPath();p.Arc(c,radius,0,360);p.Stroke();
   int sum=0;foreach(var v in counts)sum+=v;if(sum==0)return;
   float angle=-90;
   for(int i=0;i<counts.Length;i++){if(counts[i]==0)continue;float sweep=360f*counts[i]/sum;
    p.strokeColor=colors[i];p.BeginPath();p.Arc(c,radius,angle,angle+sweep);p.Stroke();angle+=sweep;}
  }
 }
 public static Button CareerNav(VisualElement parent,string icon,string title,bool selected,Action click) {
  var b=Button_(parent,"",click,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;b.style.minHeight=KarineTheme.Career.NavHeight;b.style.marginBottom=KarineTheme.SpaceXs;
  b.style.paddingLeft=KarineTheme.SpaceMd;b.style.paddingRight=KarineTheme.SpaceSm;
  var ink=selected?KarineTheme.Paper.Ink:KarineTheme.Primary;
  Icon(b,icon,ink,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Body_(b,title,KarineTheme.CaseBrowser.TextSize+1);l.style.color=ink;l.style.flexGrow=1;l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleLeft;
  Icon(b,"nav_next",ink,KarineTheme.IconSize);
  return b;
 }
}
}
