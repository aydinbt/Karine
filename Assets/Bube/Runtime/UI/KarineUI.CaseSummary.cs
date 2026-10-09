using System;
using UnityEngine;
using UnityEngine.UIElements;
using C = Bube.KarineTheme.CaseSummary;
namespace Bube {
// Vaka özeti (3 Ekim 2026 maketi, Docs/Reference/UI_CASE_SUMMARY_2026-10.png): ataşlı olay yeri
// fotoğrafı, "rapor gönderildi" damgası, gönderilen raporun tablosu, bantlı kaynak notu ve faks notu.
// Sonucu söylemez; sıradaki görev masadaki bildirimle gelir.
public static partial class KarineUI {
 public static VisualElement SummaryPaper(VisualElement parent,Texture2D photo,out VisualElement right,out VisualElement footer) {
  var paper=new VisualElement {name="CaseSummaryPaper"};OfficePlace(paper,C.Paper);
  Stretched(paper,"Bube/UI/paper_sheet");
  paper.style.flexDirection=FlexDirection.Row;paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceXl*2;
  paper.style.paddingTop=paper.style.paddingBottom=KarineTheme.SpaceXl;parent.Add(paper);
  if(photo!=null) {
  var left=new VisualElement();left.style.width=Length.Percent(C.PhotoWidth);left.style.flexShrink=0;paper.Add(left);
  var frame=new VisualElement {pickingMode=PickingMode.Ignore};frame.style.rotate=new Rotate(C.PhotoTilt);
  frame.style.paddingLeft=frame.style.paddingRight=frame.style.paddingTop=C.PhotoBorder;frame.style.paddingBottom=C.PhotoBorder*3;
  frame.style.backgroundColor=KarineTheme.Paper.Light;Border(frame,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);left.Add(frame);
  var image=new Image {image=photo,scaleMode=ScaleMode.ScaleAndCrop};image.style.height=C.PhotoHeight;
  image.style.backgroundColor=KarineTheme.Paper.Edge;frame.Add(image);
  var clip=new VisualElement {pickingMode=PickingMode.Ignore};clip.style.width=C.Clip;clip.style.height=C.Clip*2;Stretched(clip,"Bube/UI/paperclip");left.Add(clip);clip.style.position=Position.Absolute;clip.style.top=-C.Clip/2;clip.style.left=C.Clip;
  }
  // Sağ sütun kayar; "Masaya dön" her zaman altta görünür kalır.
  var side=new VisualElement();side.style.flexGrow=1;side.style.flexShrink=1;side.style.minHeight=0;
  if(photo!=null)side.style.marginLeft=KarineTheme.SpaceXl*2;paper.Add(side);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.flexShrink=1;scroll.style.minHeight=0;side.Add(scroll);
  right=scroll.contentContainer;footer=side;
  return paper;
 }
 public static void SummaryStamp(VisualElement parent,string stamp,string line) {
  var t=Write(parent,stamp.ToUpper(TextCulture),KarineTheme.Paper.StampInk,C.StampSize,Heading);t.name="CaseSummaryStamp";t.pickingMode=PickingMode.Ignore;
  t.style.alignSelf=Align.FlexStart;t.style.rotate=new Rotate(C.StampTilt);t.style.opacity=.9f;t.style.letterSpacing=3;
  Border(t,4,KarineTheme.Paper.StampInk);Round(t,4);t.style.unityTextAlign=TextAnchor.MiddleCenter;
  t.style.paddingLeft=t.style.paddingRight=KarineTheme.SpaceLg;t.style.paddingTop=t.style.paddingBottom=KarineTheme.SpaceSm;
  t.style.marginTop=KarineTheme.SpaceMd;t.style.marginLeft=KarineTheme.SpaceSm;t.style.marginBottom=0;
  var l=Typed(parent,line,C.LineSize);l.style.marginTop=KarineTheme.SpaceMd;l.style.whiteSpace=WhiteSpace.Normal;
  PaperRule(parent,false);
 }
 public static void SummarySources(VisualElement parent,string heading,string[] names,string faxNote) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.FlexEnd;row.style.marginTop=KarineTheme.SpaceLg;parent.Add(row);
  var note=new VisualElement {name="CaseSummarySources"};note.style.flexGrow=1;note.style.flexShrink=1;note.style.rotate=new Rotate(C.NoteTilt);
  note.style.paddingLeft=note.style.paddingRight=KarineTheme.SpaceLg;note.style.paddingTop=note.style.paddingBottom=KarineTheme.SpaceMd;
  note.style.backgroundColor=KarineTheme.Paper.Light;Border(note,1,KarineTheme.Paper.Edge);row.Add(note);
  var tape=new VisualElement {pickingMode=PickingMode.Ignore};tape.style.position=Position.Absolute;tape.style.right=-KarineTheme.SpaceMd;tape.style.top=-KarineTheme.SpaceMd;
  tape.style.width=C.Clip*1.5f;tape.style.height=C.Clip/2;tape.style.rotate=new Rotate(40);Stretched(tape,"Bube/UI/tape");note.Add(tape);
  Typed(note,heading.ToUpper(TextCulture),C.HeadingSize,true);
  foreach(var name in names){var l=Typed(note,"•  "+name,C.SourceSize);l.style.marginBottom=2;l.style.borderBottomWidth=1;l.style.borderBottomColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.6f);}
  var fax=new VisualElement {name="CaseSummaryFax"};fax.style.width=Length.Percent(C.FaxWidth);fax.style.flexShrink=0;fax.style.marginLeft=KarineTheme.SpaceLg;
  fax.style.flexDirection=FlexDirection.Row;fax.style.alignItems=Align.Center;fax.style.rotate=new Rotate(C.FaxTilt);
  fax.style.paddingLeft=fax.style.paddingRight=KarineTheme.SpaceMd;fax.style.paddingTop=fax.style.paddingBottom=KarineTheme.SpaceMd;
  fax.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.45f);row.Add(fax);
  Icon(fax,"document",KarineTheme.Paper.Ink,C.FaxIcon);
  var f=Typed(fax,faxNote,C.SourceSize);f.style.marginLeft=KarineTheme.SpaceSm;f.style.flexShrink=1;f.style.whiteSpace=WhiteSpace.Normal;f.style.marginBottom=0;
 }
}
}
