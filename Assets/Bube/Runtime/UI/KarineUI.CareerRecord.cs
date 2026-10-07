using UnityEngine;
using UnityEngine.UIElements;
using C = Bube.KarineTheme.CareerRecord;
namespace Bube {
// Kariyer kaydı (3 Ekim 2026 maketi, Docs/Reference/UI_CAREER_RECORD_2026-10.png): tek ataşlı sicil kâğıdı,
// damga, gönderilen raporun tablosu, sonrası ve altta koyu güven şeridi.
public static partial class KarineUI {
 public static VisualElement RecordPaper(VisualElement parent) {
  var paper=new VisualElement {name="CareerRecordPaper"};OfficePlace(paper,C.Paper);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(paper,KarineTheme.Radius);parent.Add(paper);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;paper.Add(scroll);
  var c=scroll.contentContainer;c.style.paddingLeft=KarineTheme.SpaceXl*2;c.style.paddingRight=KarineTheme.SpaceXl*2;c.style.paddingTop=KarineTheme.SpaceLg;c.style.paddingBottom=KarineTheme.SpaceLg;
  return c;
 }
 public static void RecordHead(VisualElement paper,string title,string date,string stamp,bool rejected) {
  var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.FlexStart;paper.Add(head);
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;head.Add(words);
  Typed(words,title.ToUpper(TextCulture),C.TitleSize,true).style.whiteSpace=WhiteSpace.Normal;
  if(!string.IsNullOrEmpty(date))Typed(words,date,C.BodySize);
  var s=new VisualElement {name="CareerRecordStamp",pickingMode=PickingMode.Ignore};s.style.rotate=new Rotate(C.StampTilt);s.style.opacity=.85f;s.style.flexShrink=0;
  var ink=rejected?KarineTheme.Danger:KarineTheme.Paper.Stamp;Border(s,4,ink);Round(s,4);s.style.paddingLeft=KarineTheme.SpaceMd;s.style.paddingRight=KarineTheme.SpaceMd;head.Add(s);
  var t=Write(s,stamp.ToUpper(TextCulture),ink,C.StampSize,Heading);t.style.marginBottom=0;t.style.letterSpacing=2;
  PaperRule(paper,false);
 }
 public static void RecordHeading(VisualElement paper,string text) {
  Typed(paper,text.ToUpper(TextCulture),C.HeadingSize,true).style.marginTop=KarineTheme.SpaceSm;
 }
 // Rapor satırı: gölgeli anahtar, seçim (şüphelide küçük polaroid), sağda soluk dayanak.
 public static void RecordRow(VisualElement paper,string key,string value,Texture2D portrait,string basis) {
  var row=new VisualElement {name="CareerRecordRow"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.minHeight=C.RowHeight;
  row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.8f);paper.Add(row);
  var k=new VisualElement();k.style.width=C.KeyWidth;k.style.alignSelf=Align.Stretch;k.style.justifyContent=Justify.Center;k.style.paddingLeft=KarineTheme.SpaceSm;
  k.style.marginTop=2;k.style.marginBottom=2;k.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.35f);row.Add(k);
  Typed(k,key.ToUpper(TextCulture),C.BodySize,true).style.marginBottom=0;
  var v=new VisualElement();v.style.width=Length.Percent(C.ValueWidth);v.style.flexDirection=FlexDirection.Row;v.style.alignItems=Align.Center;v.style.marginLeft=KarineTheme.SpaceMd;row.Add(v);
  var l=Typed(v,":  "+value,C.ValueSize);l.style.marginBottom=0;l.style.flexGrow=1;l.style.flexShrink=1;l.style.whiteSpace=WhiteSpace.Normal;
  if(portrait!=null) {
   var frame=new VisualElement {pickingMode=PickingMode.Ignore};frame.style.width=C.Portrait;frame.style.height=C.Portrait;frame.style.flexShrink=0;frame.style.rotate=new Rotate(3);
   frame.style.paddingLeft=3;frame.style.paddingRight=3;frame.style.paddingTop=3;frame.style.paddingBottom=3;frame.style.backgroundColor=KarineTheme.Paper.Light;
   Border(frame,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);v.Add(frame);
   var face=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop};face.style.flexGrow=1;frame.Add(face);
  }
  if(string.IsNullOrEmpty(basis)){v.style.width=StyleKeyword.Auto;v.style.flexGrow=1;v.style.flexShrink=1;return;}
  var b=Typed(row,basis,C.BasisSize,false,KarineTheme.Paper.Faded);b.style.flexGrow=1;b.style.flexShrink=1;b.style.marginBottom=0;b.style.whiteSpace=WhiteSpace.Normal;
  b.style.paddingLeft=KarineTheme.SpaceMd;b.style.borderLeftWidth=1;b.style.borderLeftColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.8f);
 }
 public static void RecordAfter(VisualElement paper,string text) {
  var l=Typed(paper,text,C.ValueSize);l.style.unityFontStyleAndWeight=FontStyle.Italic;l.style.whiteSpace=WhiteSpace.Normal;
 }
 // Alttaki koyu şerit: güven kademesi (kötüyse kırmızı) ve varsa yeniden açılma notu.
 public static void RecordTrust(VisualElement paper,string label,string status,bool bad,string note) {
  var bar=new VisualElement {name="CareerRecordTrust"};bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;bar.style.marginTop=KarineTheme.SpaceLg;
  bar.style.minHeight=C.TrustHeight;bar.style.paddingLeft=KarineTheme.SpaceLg;bar.style.paddingRight=KarineTheme.SpaceLg;
  bar.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.95f);Round(bar,KarineTheme.Radius);paper.Add(bar);
  Write(bar,label.ToUpper(TextCulture),KarineTheme.Secondary,C.TrustSize,Heading).style.marginBottom=0;
  var ink=bad?KarineTheme.Danger:KarineTheme.Accent;
  var pill=new VisualElement();pill.style.minWidth=C.PillWidth;pill.style.height=C.TrustHeight-KarineTheme.SpaceLg;pill.style.marginLeft=KarineTheme.SpaceLg;
  pill.style.alignItems=Align.Center;pill.style.justifyContent=Justify.Center;pill.style.paddingLeft=KarineTheme.SpaceMd;pill.style.paddingRight=KarineTheme.SpaceMd;
  Border(pill,KarineTheme.BorderWidth+1,ink);Round(pill,KarineTheme.Radius);bar.Add(pill);
  Write(pill,status.ToUpper(TextCulture),ink,C.TrustSize+2,Heading).style.marginBottom=0;
  var gap=new VisualElement();gap.style.flexGrow=1;bar.Add(gap);
  if(!string.IsNullOrEmpty(note)){var n=Write(bar,note,KarineTheme.Secondary,C.NoteSize,Typewriter);n.style.marginBottom=0;n.style.flexShrink=1;n.style.whiteSpace=WhiteSpace.Normal;}
 }
}
}
