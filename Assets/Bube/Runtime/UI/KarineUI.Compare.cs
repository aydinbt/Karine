using System;
using UnityEngine;
using UnityEngine.UIElements;
using C = Bube.KarineTheme.Compare;
namespace Bube {
// Kaynak karşılaştırma ekranının parçaları (3 Ekim 2026 maketi, Docs/Reference/UI_COMPARE_2026-10.png).
public static partial class KarineUI {
 public static VisualElement CompareSheet(VisualElement parent,bool left) {
  var paper=new VisualElement {name=left?"CompareLeft":"CompareRight"};OfficePlace(paper,left?C.Left:C.Right);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(paper,KarineTheme.Radius);paper.style.rotate=new Rotate(left?C.LeftTilt:C.RightTilt);
  paper.style.paddingLeft=KarineTheme.SpaceXl;paper.style.paddingRight=KarineTheme.SpaceXl;paper.style.paddingTop=KarineTheme.SpaceLg;paper.style.paddingBottom=KarineTheme.SpaceLg;
  parent.Add(paper);return paper;
 }
 // Kâğıdın üstündeki koyu seçici çubuğu: belge ikonu, kaynağın adı, aşağı ok.
 public static Button ComparePicker(VisualElement sheet,string label,bool open,Action toggle) {
  var bar=new Button(Sounded(toggle)) {name="ComparePicker",tooltip=label};bar.style.height=C.PickerHeight;bar.style.flexShrink=0;
  bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;bar.style.marginLeft=0;bar.style.marginRight=0;bar.style.marginBottom=KarineTheme.SpaceLg;
  bar.style.paddingLeft=KarineTheme.SpaceMd;bar.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(bar,KarineTheme.Alpha(KarineTheme.GlassDeep,.97f));Border(bar,KarineTheme.BorderWidth,open?KarineTheme.Accent:KarineTheme.Border);Round(bar,KarineTheme.Radius);
  Icon(bar,"document",KarineTheme.Primary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(bar,label.ToUpper(Tr),KarineTheme.Primary,C.PickerSize,Heading);l.style.marginBottom=0;l.style.flexGrow=1;l.style.flexShrink=1;
  l.style.unityTextAlign=TextAnchor.MiddleLeft;l.style.whiteSpace=WhiteSpace.NoWrap;l.style.overflow=Overflow.Hidden;l.style.textOverflow=TextOverflow.Ellipsis;
  var chevron=Icon(bar,"nav_next",KarineTheme.Primary,KarineTheme.IconSize);chevron.style.rotate=new Rotate(open?-90:90);
  sheet.Add(bar);return bar;
 }
 // Seçicinin altında açılan koyu liste; gruplar ve satırlar çağıran tarafından eklenir.
 public static ScrollView CompareMenu(VisualElement sheet) {
  var menu=new KarineScrollView(ScrollViewMode.Vertical) {name="CompareMenu"};menu.style.position=Position.Absolute;
  menu.style.left=KarineTheme.SpaceXl;menu.style.right=KarineTheme.SpaceXl;menu.style.top=KarineTheme.SpaceLg+C.PickerHeight;menu.style.maxHeight=C.MenuMax;
  menu.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.98f);Border(menu,KarineTheme.BorderWidth,KarineTheme.Border);Round(menu,KarineTheme.Radius);
  menu.contentContainer.style.paddingTop=KarineTheme.SpaceSm;menu.contentContainer.style.paddingBottom=KarineTheme.SpaceSm;
  menu.contentContainer.style.paddingLeft=KarineTheme.SpaceSm;menu.contentContainer.style.paddingRight=KarineTheme.SpaceSm;
  sheet.Add(menu);return menu;
 }
 public static void CompareGroup(VisualElement menu,string title) {
  var g=Write(menu,title.ToUpper(Tr),KarineTheme.Accent,C.GroupSize,Heading);g.style.marginTop=KarineTheme.SpaceSm;g.style.marginBottom=KarineTheme.SpaceXs;g.style.marginLeft=KarineTheme.SpaceSm;
 }
 public static Button CompareOption(VisualElement menu,string title,bool selected,Action action) {
  var row=new Button(Sounded(action)) {name="CompareOption",tooltip=title};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=KarineTheme.TouchTarget;row.style.marginLeft=0;row.style.marginRight=0;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceMd;row.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Accent,.14f):KarineTheme.Alpha(KarineTheme.Background,.7f));
  Border(row,KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(row,KarineTheme.Radius);
  Icon(row,"document",KarineTheme.Secondary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(row,title,KarineTheme.Primary,C.OptionSize,Typewriter);l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleLeft;l.style.flexShrink=1;
  menu.Add(row);return row;
 }
 // Boş kâğıt: ortada soluk belge simgesi ve daktilo yönerge.
 public static void CompareEmpty(VisualElement sheet,string text) {
  var box=new VisualElement {pickingMode=PickingMode.Ignore};box.style.flexGrow=1;box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;sheet.Add(box);
  var icon=Icon(box,"document",KarineTheme.Alpha(KarineTheme.Paper.Faded,.6f),C.EmptyIcon);icon.style.marginBottom=KarineTheme.SpaceLg;
  var l=Write(box,text,KarineTheme.Paper.Faded,KarineTheme.Dossier.PageBodySize+2,Typewriter);l.style.unityTextAlign=TextAnchor.MiddleCenter;l.style.maxWidth=Length.Percent(70);
 }
 public static VisualElement CompareMarks(VisualElement parent) {
  var strip=new VisualElement {name="CompareMarks"};OfficePlace(strip,C.Marks);strip.style.justifyContent=Justify.SpaceBetween;parent.Add(strip);return strip;
 }
 // Hüküm düğmesi: büyük işaret ve altında etiket; seçili olan amber çerçeveli. Oyuncunun kendi notudur.
 public static Button CompareMark(VisualElement strip,string glyph,Color tone,string label,bool selected,bool enabled,Action action) {
  var button=new Button(Sounded(action)) {name="CompareMark",tooltip=label};button.SetEnabled(enabled);
  button.style.flexGrow=1;button.style.flexBasis=0;button.style.marginLeft=0;button.style.marginRight=0;button.style.marginBottom=KarineTheme.SpaceMd;
  button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  Unskin(button,KarineTheme.Alpha(KarineTheme.GlassDeep,.96f));Border(button,selected?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Border);Round(button,KarineTheme.Radius);
  var color=enabled?tone:KarineTheme.Alpha(KarineTheme.Secondary,.5f);
  var g=Write(button,glyph,color,C.MarkGlyph,Heading);g.style.marginBottom=0;g.style.unityTextAlign=TextAnchor.MiddleCenter;
  var l=Write(button,label.ToUpper(Tr),enabled?KarineTheme.Primary:KarineTheme.Alpha(KarineTheme.Secondary,.6f),C.MarkLabel,Heading);l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleCenter;
  strip.Add(button);return button;
 }
}
}
