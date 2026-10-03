using System;
using UnityEngine;
using UnityEngine.UIElements;
using D = Bube.KarineTheme.Dossier;
namespace Bube {
// Dosya Detay çerçevesi (3 Ekim 2026 maketi, Docs/Reference/UI_FILE_2026-10.png):
// üstte koyu şerit, masada açık karton klasör, solda karton sekmeler, yanında koyu
// belge listesi, sağda kâğıt. İçerik `BubeApp.FilePage` tarafından doldurulur.
public static partial class KarineUI {
 // Üst şerit: solda "Masaya dön", ortada dosya adı ve birim, sağda ek düğmeler için `tools`.
 public static VisualElement DossierBar(VisualElement parent,string back,string title,string department,Action close,out VisualElement tools) {
  var bar=new VisualElement {name="DossierBar"};OfficePlace(bar,D.Bar);
  bar.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.96f);Border(bar,KarineTheme.BorderWidth,KarineTheme.Border);
  Round(bar,KarineTheme.Radius);bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;
  bar.style.paddingLeft=KarineTheme.SpaceMd;bar.style.paddingRight=KarineTheme.SpaceMd;parent.Add(bar);
  var backButton=new Button(Sounded(close)) {name="DossierBack",tooltip=back};
  backButton.style.width=D.BackWidth;backButton.style.height=KarineTheme.TouchTarget;backButton.style.flexShrink=0;
  backButton.style.flexDirection=FlexDirection.Row;backButton.style.alignItems=Align.Center;
  backButton.style.paddingLeft=0;backButton.style.paddingRight=0;backButton.style.marginLeft=0;
  Unskin(backButton,KarineTheme.Alpha(KarineTheme.Panel,.9f));Border(backButton,KarineTheme.BorderWidth,KarineTheme.Border);Round(backButton,KarineTheme.Radius);
  var arrow=new VisualElement {pickingMode=PickingMode.Ignore};arrow.style.width=KarineTheme.TouchTarget;arrow.style.alignSelf=Align.Stretch;
  arrow.style.alignItems=Align.Center;arrow.style.justifyContent=Justify.Center;arrow.style.borderRightWidth=KarineTheme.BorderWidth;
  arrow.style.borderRightColor=KarineTheme.Border;backButton.Add(arrow);Icon(arrow,"nav_prev",KarineTheme.Primary,KarineTheme.IconSize);
  var label=Write(backButton,back.ToUpper(Tr),KarineTheme.Primary,D.BackLabelSize,Heading);label.style.marginBottom=0;
  label.style.flexGrow=1;label.style.unityTextAlign=TextAnchor.MiddleCenter;label.style.whiteSpace=WhiteSpace.NoWrap;
  bar.Add(backButton);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;words.style.minWidth=0;
  words.style.alignItems=Align.Center;words.style.justifyContent=Justify.Center;bar.Add(words);
  var t=Write(words,title.ToUpper(Tr),KarineTheme.Primary,D.BarTitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceSm;
  t.style.whiteSpace=WhiteSpace.NoWrap;t.style.unityTextAlign=TextAnchor.MiddleCenter;
  var s=Body_(words,department,D.BarSubSize);s.style.color=KarineTheme.Secondary;s.style.marginBottom=0;s.style.unityTextAlign=TextAnchor.MiddleCenter;
  tools=new VisualElement {pickingMode=PickingMode.Ignore};tools.style.flexDirection=FlexDirection.Row;tools.style.alignItems=Align.Center;
  tools.style.flexShrink=0;tools.style.minWidth=D.BackWidth;tools.style.justifyContent=Justify.FlexEnd;bar.Add(tools);
  return bar;
 }
 // Üst şeridin sağındaki kare düğme (ara, karşılaştır, sonuç, ayarlar).
 public static Button DossierTool(VisualElement tools,string icon,string title,Action action) {
  var button=IconButton(tools,icon,action,title);button.style.marginRight=0;button.style.marginLeft=KarineTheme.SpaceSm;
  Border(button,KarineTheme.BorderWidth,KarineTheme.Border);button.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Panel,.9f);
  return button;
 }
 // Masadaki karton klasör; sekmeler, liste ve kâğıt bunun üstüne oturur.
 public static void DossierCase(VisualElement parent) {
  var cover=new VisualElement {name="DossierFolder",pickingMode=PickingMode.Ignore};OfficePlace(cover,D.Case);
  Stretched(cover,"Bube/UI/paper_folder");parent.Add(cover);
 }
 public static VisualElement DossierTabColumn(VisualElement parent) {
  var column=new VisualElement {name="DossierTabs"};OfficePlace(column,D.TabColumn);parent.Add(column);return column;
 }
 // Karton sekme: ikon ve büyük harf etiket; seçili olan açık kâğıt renginde ve kâğıda doğru uzar.
 public static Button DossierFolderTab(VisualElement column,string icon,string title,bool active,bool unread,Action action) {
  var tab=new Button(Sounded(action)) {name="DossierTab",tooltip=title};
  tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.minHeight=KarineTheme.TouchTarget;tab.style.marginBottom=KarineTheme.SpaceSm;
  tab.style.marginLeft=0;tab.style.marginRight=active?-KarineTheme.SpaceSm:0;
  tab.style.flexDirection=FlexDirection.Row;tab.style.alignItems=Align.Center;tab.style.paddingLeft=KarineTheme.SpaceMd;tab.style.paddingRight=KarineTheme.SpaceSm;
  Unskin(tab,active?KarineTheme.Paper.Light:KarineTheme.Paper.Tint);Border(tab,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  Round(tab,KarineTheme.Radius);tab.style.borderTopRightRadius=0;tab.style.borderBottomRightRadius=0;
  Icon(tab,icon,KarineTheme.Paper.Ink,D.FolderTabIcon).style.marginRight=KarineTheme.SpaceSm;
  var text=Write(tab,title.ToUpper(Tr),KarineTheme.Paper.Ink,D.FolderTabSize,Heading);text.style.marginBottom=0;
  text.style.flexShrink=1;text.style.whiteSpace=WhiteSpace.Normal;
  if(unread)Dot(tab,KarineTheme.Paper.Stamp).style.marginLeft=KarineTheme.SpaceXs;
  column.Add(tab);return tab;
 }
 public static ScrollView DossierList(VisualElement parent) {
  var panel=new VisualElement {name="DossierList"};OfficePlace(panel,D.List);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Paper.FolderDeep);
  Round(panel,KarineTheme.Radius);panel.style.paddingTop=KarineTheme.SpaceSm;panel.style.paddingBottom=KarineTheme.SpaceSm;
  panel.style.paddingLeft=KarineTheme.SpaceXs;panel.style.paddingRight=KarineTheme.SpaceXs;parent.Add(panel);
  var scroll=new KarineScrollView(ScrollViewMode.Vertical);scroll.style.flexGrow=1;panel.Add(scroll);return scroll;
 }
 // Belge satırı: okunmamışsa amber nokta, büyük başlık, altında varsa tarih. İçerikten ipucu yok.
 public static Button DossierListRow(VisualElement list,string title,string sub,bool selected,bool unread,Action action) {
  var row=new Button(Sounded(action)) {name="DossierRow",tooltip=title};
  row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.FlexStart;row.style.minHeight=KarineTheme.TouchTarget+KarineTheme.SpaceLg;
  row.style.marginLeft=0;row.style.marginRight=0;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceSm;row.style.paddingTop=KarineTheme.SpaceMd;row.style.paddingBottom=KarineTheme.SpaceMd;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Panel,.95f):Color.clear);
  Border(row,selected?KarineTheme.BorderWidth+1:0,selected?KarineTheme.Accent:Color.clear);Round(row,KarineTheme.Radius);
  row.style.borderBottomWidth=selected?KarineTheme.BorderWidth+1:1;row.style.borderBottomColor=selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f);
  var mark=new VisualElement {pickingMode=PickingMode.Ignore};mark.style.width=D.RowDot+KarineTheme.SpaceMd;mark.style.flexShrink=0;
  mark.style.paddingTop=KarineTheme.SpaceSm;row.Add(mark);
  if(unread)Dot(mark,KarineTheme.Accent);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;row.Add(words);
  var t=Write(words,title,selected?KarineTheme.Primary:KarineTheme.Secondary,D.RowTitleSize,Heading);t.style.marginBottom=0;t.style.whiteSpace=WhiteSpace.Normal;
  if(!string.IsNullOrEmpty(sub)){var s=Body_(words,sub,D.RowSubSize);s.style.color=selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Secondary,.7f);s.style.marginBottom=0;}
  list.Add(row);return row;
 }
 static VisualElement Dot(VisualElement parent,Color color) {
  var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=D.RowDot;dot.style.height=D.RowDot;dot.style.flexShrink=0;
  Round(dot,D.RowDot/2);dot.style.backgroundColor=color;parent.Add(dot);return dot;
 }
 // Kâğıt: düz krem yaprak, ince kenar ve altında gölge.
 public static VisualElement DossierPage(VisualElement parent) {
  var shadow=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(shadow,new Rect(D.Page.x+.4f,D.Page.y+.8f,D.Page.width,D.Page.height));
  shadow.style.backgroundColor=KarineTheme.Alpha(Color.black,.35f);Round(shadow,KarineTheme.Radius);parent.Add(shadow);
  var paper=new VisualElement {name="DossierPaper"};OfficePlace(paper,D.Page);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(paper,KarineTheme.Radius);
  paper.style.paddingLeft=KarineTheme.SpaceXl+KarineTheme.SpaceSm;paper.style.paddingRight=KarineTheme.SpaceXl;
  paper.style.paddingTop=KarineTheme.SpaceXl;paper.style.paddingBottom=KarineTheme.SpaceLg;paper.style.overflow=Overflow.Hidden;
  parent.Add(paper);return paper;
 }
}
}
