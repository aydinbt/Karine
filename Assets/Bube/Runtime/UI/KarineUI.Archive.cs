using System;
using UnityEngine;
using UnityEngine.UIElements;
using A = Bube.KarineTheme.Archive;
namespace Bube {
// Arşiv (3 Ekim 2026 maketleri, Docs/Reference/UI_ARCHIVE_2026-10.png, UI_ARCHIVE_CASE_2026-10.png):
// metal dolap çekmecesinde dosya kartları; arşivdeki dosyada solda kaynaklar, sağda krem kâğıt.
public static partial class KarineUI {
 // Açık çekmece: koyu metal kasa, içi kaydırılabilir, önünde "KAPANMIŞ DOSYALAR" plakası.
 public static VisualElement ArchiveDrawer(VisualElement parent,string plate) {
  var drawer=new VisualElement {name="ArchiveDrawer"};OfficePlace(drawer,A.Drawer);
  drawer.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Border(drawer,A.Frame,KarineTheme.Alpha(KarineTheme.Border,.9f));Round(drawer,KarineTheme.Radius);
  drawer.style.paddingLeft=drawer.style.paddingRight=KarineTheme.SpaceXl*2;drawer.style.paddingTop=KarineTheme.SpaceXl;drawer.style.paddingBottom=KarineTheme.SpaceLg;parent.Add(drawer);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;drawer.Add(scroll);
  var front=new VisualElement {pickingMode=PickingMode.Ignore};front.style.alignItems=Align.Center;front.style.marginTop=KarineTheme.SpaceLg;drawer.Add(front);
  var label=new VisualElement();label.style.paddingLeft=label.style.paddingRight=KarineTheme.SpaceXl;label.style.paddingTop=label.style.paddingBottom=KarineTheme.SpaceXs;
  label.style.backgroundColor=KarineTheme.Paper.Edge;Border(label,2,KarineTheme.Secondary);Round(label,KarineTheme.Radius);front.Add(label);
  Write(label,plate.ToUpper(TextCulture),KarineTheme.Paper.Ink,A.PlateSize,Heading).style.marginBottom=0;
  var handle=new VisualElement();handle.style.width=A.Handle;handle.style.height=A.Handle/6;handle.style.marginTop=KarineTheme.SpaceSm;
  Border(handle,3,KarineTheme.Secondary);handle.style.borderTopWidth=0;front.Add(handle);
  return scroll.contentContainer;
 }
 // Çekmecedeki dosya kartı: klasör ikonu, ad, rapor tarihi, mürekkep damgası, ok.
 public static Button ArchiveCard(VisualElement parent,string title,string date,string stamp,bool approved,string note,Action action) {
  var card=new Button(Sounded(action)) {name="ArchiveCard",tooltip=title};card.style.flexDirection=FlexDirection.Row;card.style.alignItems=Align.Center;
  card.style.marginLeft=card.style.marginRight=card.style.marginTop=0;card.style.marginBottom=KarineTheme.SpaceMd;card.style.paddingLeft=card.style.paddingTop=card.style.paddingBottom=0;card.style.paddingRight=KarineTheme.SpaceXl;
  Unskin(card,KarineTheme.Paper.Sheet);Stretched(card,"Bube/UI/paper_sheet");Border(card,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);parent.Add(card);
  var box=new VisualElement();box.style.width=A.CardIcon;box.style.alignSelf=Align.Stretch;box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;box.style.backgroundColor=KarineTheme.GlassDeep;card.Add(box);
  Icon(box,"folder",KarineTheme.Accent,A.CardIcon/2);
  var text=new VisualElement();text.style.flexGrow=1;text.style.flexShrink=1;text.style.paddingLeft=KarineTheme.SpaceXl;text.style.paddingTop=text.style.paddingBottom=KarineTheme.SpaceMd;card.Add(text);
  var t=Write(text,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,A.CardTitle,Heading);t.style.marginBottom=KarineTheme.SpaceXs;t.style.whiteSpace=WhiteSpace.Normal;
  Typed(text,date,A.CardDate).style.marginBottom=0;
  if(!string.IsNullOrEmpty(stamp)) {
   var col=new VisualElement {pickingMode=PickingMode.Ignore};col.style.alignItems=Align.Center;col.style.marginRight=KarineTheme.SpaceLg;card.Add(col);
   InkStamp(col,stamp,approved?KarineTheme.Paper.Approved:KarineTheme.Paper.Stamp,A.StampSize);
   if(!string.IsNullOrEmpty(note)){var n=Typed(col,note,A.CardDate);n.style.color=KarineTheme.Paper.Stamp;n.style.unityFontStyleAndWeight=FontStyle.Italic;n.style.marginBottom=0;}
  }
  Icon(card,"nav_next",KarineTheme.Paper.Ink,KarineTheme.IconSize+4);
  return card;
 }
 // Çapraz mürekkep damgası: çerçeve ve harfler aynı soluk renkte.
 public static VisualElement InkStamp(VisualElement parent,string text,Color ink,int size) {
  var stamp=new VisualElement {name="InkStamp",pickingMode=PickingMode.Ignore};Border(stamp,3,KarineTheme.Alpha(ink,.85f));Round(stamp,KarineTheme.Radius);
  stamp.style.paddingLeft=stamp.style.paddingRight=KarineTheme.SpaceLg;stamp.style.rotate=new Rotate(-5);stamp.style.alignSelf=Align.FlexStart;parent.Add(stamp);
  var t=Write(stamp,text.ToUpper(TextCulture),KarineTheme.Alpha(ink,.9f),size,Heading);t.style.marginBottom=0;
  return stamp;
 }
 public static void ArchiveEmpty(VisualElement parent,string text) {
  var card=new VisualElement {name="ArchiveEmpty"};card.style.alignSelf=Align.Center;card.style.width=Length.Percent(60);card.style.marginTop=KarineTheme.SpaceXl;
  card.style.paddingTop=card.style.paddingBottom=KarineTheme.SpaceXl*2;card.style.alignItems=Align.Center;
  card.style.backgroundColor=KarineTheme.Paper.Sheet;Stretched(card,"Bube/UI/paper_sheet");Border(card,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);parent.Add(card);
  Icon(card,"folder",KarineTheme.Paper.Ink,A.EmptyIcon).style.marginBottom=KarineTheme.SpaceMd;
  var l=Typed(card,text,A.CardDate+2);l.style.unityTextAlign=TextAnchor.MiddleCenter;l.style.whiteSpace=WhiteSpace.Normal;
 }
 // Arşivdeki dosyanın solundaki kaynak sütunu.
 public static VisualElement ArchiveSources(VisualElement parent,string heading) {
  var panel=new VisualElement {name="ArchiveSources"};OfficePlace(panel,A.Sources);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.95f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Border);Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=panel.style.paddingRight=KarineTheme.SpaceMd;panel.style.paddingTop=KarineTheme.SpaceLg;panel.style.paddingBottom=KarineTheme.SpaceMd;parent.Add(panel);
  var h=Write(panel,heading.ToUpper(TextCulture),KarineTheme.Primary,A.SourceHead,Heading);h.style.marginBottom=KarineTheme.SpaceMd;h.style.marginLeft=KarineTheme.SpaceSm;
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;panel.Add(scroll);
  return scroll.contentContainer;
 }
 public static Button ArchiveSource(VisualElement parent,string icon,string label,bool selected,Action action) {
  var b=new Button(Sounded(action)) {name="ArchiveSource",tooltip=label};b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;b.style.minHeight=A.SourceRow;
  b.style.marginLeft=b.style.marginRight=b.style.marginTop=0;b.style.marginBottom=KarineTheme.SpaceXs;b.style.paddingLeft=b.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(b,selected?KarineTheme.Alpha(KarineTheme.Accent,.12f):KarineTheme.Alpha(KarineTheme.Background,.5f));
  Border(b,KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(b,KarineTheme.Radius);
  Icon(b,icon,selected?KarineTheme.Accent:KarineTheme.Secondary,A.SourceIcon).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(b,label,selected?KarineTheme.Accent:KarineTheme.Primary,A.SourceSize,Typewriter);l.style.marginBottom=0;l.style.flexShrink=1;l.style.whiteSpace=WhiteSpace.Normal;
  parent.Add(b);return b;
 }
 // Kâğıt başlığı: büyük başlık, sağda küçük tarih, altında çizgi.
 public static void ArchiveHeading(VisualElement parent,string title,string side=null) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.FlexEnd;row.style.justifyContent=Justify.SpaceBetween;parent.Add(row);
  Write(row,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,A.HeadSize,Heading).style.marginBottom=KarineTheme.SpaceXs;
  if(!string.IsNullOrEmpty(side))Typed(row,side,A.SmallSize).style.marginBottom=KarineTheme.SpaceXs;
  var rule=new VisualElement();rule.style.height=2;rule.style.backgroundColor=KarineTheme.Paper.Ink;rule.style.marginBottom=KarineTheme.SpaceMd;parent.Add(rule);
 }
 // "Sorumlu  :  Emre Koç" satırı; `highlight` fosforlu kalem gibi vurgular, `muted` değerini kırmızı mürekkeple yazar.
 public static VisualElement ArchiveField(VisualElement parent,string key,string value,bool highlight=false,bool alarm=false) {
  var row=new VisualElement {name="ArchiveField"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.FlexStart;row.style.marginBottom=KarineTheme.SpaceSm;parent.Add(row);
  var k=Write(row,key,KarineTheme.Paper.Ink,A.FieldSize,Typewriter);k.style.width=Length.Percent(A.KeyWidth);k.style.flexShrink=0;k.style.marginBottom=0;k.style.whiteSpace=WhiteSpace.Normal;
  Write(row,":",KarineTheme.Paper.Ink,A.FieldSize,Typewriter).style.marginRight=KarineTheme.SpaceMd;
  var v=Write(row,value,alarm?KarineTheme.Paper.Stamp:KarineTheme.Paper.Ink,A.FieldSize,Typewriter);v.style.flexShrink=1;v.style.marginBottom=0;v.style.whiteSpace=WhiteSpace.Normal;
  if(highlight){v.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Accent,.55f);v.style.paddingLeft=v.style.paddingRight=KarineTheme.SpaceXs;}
  return row;
 }
 // Kâğıt üstünde kehribar bağlantı: "→ Adli muayene ön raporu". `action` yoksa soluk yazı.
 public static VisualElement ArchiveLink(VisualElement parent,string label,Action action) {
  if(action==null){var l=Typed(parent,"→ "+label,A.SmallSize);l.style.color=KarineTheme.Paper.Faded;l.style.whiteSpace=WhiteSpace.Normal;return l;}
  var b=new Button(Sounded(action)) {name="ArchiveLink",tooltip=label};b.style.alignSelf=Align.FlexStart;
  b.style.marginLeft=b.style.marginRight=b.style.marginTop=0;b.style.marginBottom=KarineTheme.SpaceSm;b.style.paddingLeft=b.style.paddingRight=b.style.paddingTop=b.style.paddingBottom=0;
  Unskin(b,Color.clear);b.style.borderTopWidth=b.style.borderBottomWidth=b.style.borderLeftWidth=b.style.borderRightWidth=0;
  var t=Write(b,"→ "+label,KarineTheme.Paper.Link,A.SmallSize,Typewriter);t.style.marginBottom=0;t.style.unityFontStyleAndWeight=FontStyle.Bold;
  t.style.borderBottomWidth=1;t.style.borderBottomColor=KarineTheme.Alpha(KarineTheme.Paper.Link,.6f);t.style.whiteSpace=WhiteSpace.Normal;
  parent.Add(b);return b;
 }
 public static void ArchiveRule(VisualElement parent) {
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Ink,.4f);rule.style.marginTop=rule.style.marginBottom=KarineTheme.SpaceMd;parent.Add(rule);
 }
}
}
