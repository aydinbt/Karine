using System;
using UnityEngine;
using UnityEngine.UIElements;
using D = Bube.KarineTheme.Dossier;
namespace Bube {
// Dosya Detay çerçevesi (3 Ekim 2026 maketleri, Docs/Reference/UI_FILE_*_2026-10.png):
// üstte koyu şerit, masada açık karton klasör, solda karton sekmeler, yanında koyu
// belge listesi, sağda kâğıt. İçerik `BubeApp.FilePage` tarafından doldurulur.
public static partial class KarineUI {
 // Üst şerit: solda ok kutusu ve "Masaya dön", ortada dosya adı ve birim, sağda `tools`.
 public static VisualElement DossierBar(VisualElement parent,string back,string title,string department,Action close,out VisualElement tools) {
  var bar=new VisualElement {name="DossierBar"};OfficePlace(bar,D.Bar);
  bar.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);bar.style.borderBottomWidth=KarineTheme.BorderWidth;
  bar.style.borderBottomColor=KarineTheme.Border;bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;
  bar.style.paddingLeft=KarineTheme.SpaceXl;bar.style.paddingRight=KarineTheme.SpaceXl;parent.Add(bar);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.position=Position.Absolute;
  words.style.left=0;words.style.right=0;words.style.top=0;words.style.bottom=0;
  words.style.alignItems=Align.Center;words.style.justifyContent=Justify.Center;bar.Add(words);
  var t=Write(words,title.ToUpper(TextCulture),KarineTheme.Primary,D.BarTitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceSm;
  t.style.whiteSpace=WhiteSpace.NoWrap;t.style.letterSpacing=1;
  var s=Write(words,department.ToUpper(TextCulture),KarineTheme.Secondary,D.BarSubSize,Heading);s.style.marginBottom=0;s.style.letterSpacing=2;
  var backButton=new Button(Sounded(close)) {name="DossierBack",tooltip=back};
  backButton.style.height=KarineTheme.TouchTarget+KarineTheme.SpaceSm;backButton.style.flexShrink=0;
  backButton.style.flexDirection=FlexDirection.Row;backButton.style.alignItems=Align.Stretch;
  backButton.style.paddingLeft=0;backButton.style.paddingRight=0;backButton.style.marginLeft=0;Unskin(backButton,Color.clear);
  var arrow=new VisualElement {pickingMode=PickingMode.Ignore};arrow.style.width=KarineTheme.TouchTarget+KarineTheme.SpaceSm;
  arrow.style.alignItems=Align.Center;arrow.style.justifyContent=Justify.Center;BarBox(arrow);backButton.Add(arrow);
  Icon(arrow,"nav_prev",KarineTheme.Primary,KarineTheme.IconSize);
  var box=new VisualElement {pickingMode=PickingMode.Ignore};box.style.justifyContent=Justify.Center;BarBox(box);box.style.marginLeft=-1;
  box.style.paddingLeft=KarineTheme.SpaceLg;box.style.paddingRight=KarineTheme.SpaceLg;backButton.Add(box);
  var label=Write(box,back.ToUpper(TextCulture),KarineTheme.Primary,D.BackLabelSize,Heading);label.style.marginBottom=0;label.style.whiteSpace=WhiteSpace.NoWrap;
  bar.Add(backButton);
  var gap=new VisualElement {pickingMode=PickingMode.Ignore};gap.style.flexGrow=1;bar.Add(gap);
  tools=new VisualElement {pickingMode=PickingMode.Ignore};tools.style.flexDirection=FlexDirection.Row;tools.style.alignItems=Align.Center;bar.Add(tools);
  return bar;
 }
 static void BarBox(VisualElement box) {
  box.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.9f);Border(box,KarineTheme.BorderWidth,KarineTheme.Border);Round(box,KarineTheme.Radius);
 }
 // Üst şeridin sağındaki kare düğme (ara, karşılaştır, sonuç, ayarlar).
 public static Button DossierTool(VisualElement tools,string icon,string title,Action action) {
  var button=IconButton(tools,icon,action,title);button.style.marginRight=0;button.style.marginLeft=KarineTheme.SpaceSm;
  button.style.width=KarineTheme.TouchTarget+KarineTheme.SpaceSm;button.style.height=KarineTheme.TouchTarget+KarineTheme.SpaceSm;
  Border(button,KarineTheme.BorderWidth,KarineTheme.Border);button.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.9f);
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
 // Karton sekme: ikon üstte, büyük harf etiket altta; seçili olan açık kâğıt, solunda amber şerit.
 public static Button DossierFolderTab(VisualElement column,string icon,string title,bool active,bool unread,Action action) {
  var tab=new Button(Sounded(action)) {name="DossierTab",tooltip=title};
  tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.minHeight=KarineTheme.TouchTarget;tab.style.marginBottom=KarineTheme.SpaceXs;
  tab.style.marginLeft=active?-KarineTheme.SpaceSm:0;tab.style.marginRight=0;
  tab.style.alignItems=Align.Center;tab.style.justifyContent=Justify.Center;tab.style.paddingLeft=KarineTheme.SpaceXs;tab.style.paddingRight=KarineTheme.SpaceXs;
  Unskin(tab,active?KarineTheme.Paper.Light:KarineTheme.Paper.Edge);Border(tab,KarineTheme.BorderWidth,KarineTheme.Alpha(KarineTheme.Paper.FolderDeep,.5f));
  Round(tab,KarineTheme.Radius);tab.style.borderTopRightRadius=0;tab.style.borderBottomRightRadius=0;
  if(active){tab.style.borderLeftWidth=KarineTheme.PrimaryEdgeWidth+2;tab.style.borderLeftColor=KarineTheme.Accent;}
  Icon(tab,icon,KarineTheme.Paper.Ink,D.FolderTabIcon).style.marginBottom=KarineTheme.SpaceXs;
  var text=Write(tab,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,D.FolderTabSize,Heading);text.style.marginBottom=0;
  text.style.unityTextAlign=TextAnchor.MiddleCenter;text.style.whiteSpace=WhiteSpace.Normal;
  if(unread){var dot=Dot(tab,KarineTheme.Accent);dot.style.position=Position.Absolute;dot.style.top=KarineTheme.SpaceSm;dot.style.right=KarineTheme.SpaceSm;}
  column.Add(tab);return tab;
 }
 // Koyu belge listesi; başlık verilirse üstte büyük harf başlık, `add` verilirse yanında amber artı.
 public static ScrollView DossierList(VisualElement parent,string head=null,Action add=null,string addTitle=null) {
  var panel=new VisualElement {name="DossierList"};OfficePlace(panel,D.List);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);Border(panel,KarineTheme.BorderWidth,KarineTheme.Paper.FolderDeep);
  Round(panel,KarineTheme.Radius);panel.style.paddingTop=KarineTheme.SpaceLg;panel.style.paddingBottom=KarineTheme.SpaceSm;
  panel.style.paddingLeft=KarineTheme.SpaceMd;panel.style.paddingRight=KarineTheme.SpaceMd;parent.Add(panel);
  if(!string.IsNullOrEmpty(head)) {
   var top=new VisualElement {pickingMode=PickingMode.Ignore};top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.Center;
   top.style.marginBottom=KarineTheme.SpaceMd;top.style.paddingLeft=KarineTheme.SpaceSm;panel.Add(top);
   var h=Write(top,head.ToUpper(TextCulture),KarineTheme.Primary,D.ListHeadSize,Heading);h.style.marginBottom=0;h.style.flexGrow=1;h.style.letterSpacing=1;
   if(add!=null)PlusButton(top,"+",add,addTitle);
  }
  var scroll=new KarineScrollView(ScrollViewMode.Vertical);scroll.style.flexGrow=1;panel.Add(scroll);return scroll;
 }
 // Amber kare artı / koyu kare eksi: ekle ve çıkar düğmeleri, yazısız.
 public static Button PlusButton(VisualElement parent,string glyph,Action action,string title,bool dark=false) {
  var button=new Button(Sounded(action)) {name=glyph=="+"?"DossierAdd":"DossierRemove",tooltip=title};
  int size=KarineTheme.TouchTarget;button.style.width=size;button.style.height=size;button.style.flexShrink=0;
  button.style.paddingLeft=0;button.style.paddingRight=0;button.style.marginLeft=KarineTheme.SpaceSm;button.style.marginRight=0;
  button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  Unskin(button,dark?KarineTheme.GlassDeep:KarineTheme.Accent);Round(button,KarineTheme.Radius);
  var mark=Write(button,glyph,dark?KarineTheme.Primary:KarineTheme.OnPrimary,D.ListHeadSize+6,Heading);mark.style.marginBottom=0;mark.style.unityTextAlign=TextAnchor.MiddleCenter;
  parent.Add(button);return button;
 }
 // Belge satırı: okunmamışsa amber nokta, ikon, büyük harf başlık, altında alt yazı, sağ altta tarih.
 // İçerikten ipucu yok.
 public static Button DossierListRow(VisualElement list,string icon,string title,string sub,string date,bool selected,bool unread,Action action) {
  var row=new Button(Sounded(action)) {name="DossierRow",tooltip=title};
  row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.minHeight=KarineTheme.TouchTarget+KarineTheme.SpaceXl;
  row.style.marginLeft=0;row.style.marginRight=0;row.style.marginBottom=KarineTheme.SpaceSm;
  row.style.paddingLeft=KarineTheme.SpaceXs;row.style.paddingRight=KarineTheme.SpaceMd;row.style.paddingTop=KarineTheme.SpaceSm;row.style.paddingBottom=KarineTheme.SpaceSm;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Accent,.14f):KarineTheme.Alpha(KarineTheme.Background,.6f));
  Border(row,KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.7f));Round(row,KarineTheme.Radius);
  if(selected){row.style.borderLeftWidth=KarineTheme.PrimaryEdgeWidth+1;}
  var mark=new VisualElement {pickingMode=PickingMode.Ignore};mark.style.width=D.RowDot+KarineTheme.SpaceMd;mark.style.flexShrink=0;
  mark.style.alignItems=Align.Center;row.Add(mark);
  if(unread)Dot(mark,KarineTheme.Accent);
  if(!string.IsNullOrEmpty(icon))Icon(row,icon,KarineTheme.Secondary,D.RowIcon).style.marginRight=KarineTheme.SpaceMd;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;words.style.minWidth=0;row.Add(words);
  var t=Write(words,title,KarineTheme.Primary,D.RowTitleSize,Heading);t.style.marginBottom=0;t.style.whiteSpace=WhiteSpace.Normal;t.style.unityTextAlign=TextAnchor.MiddleLeft;
  if(!string.IsNullOrEmpty(sub)){var s=Body_(words,sub,D.RowSubSize);s.style.color=KarineTheme.Secondary;s.style.marginBottom=0;s.style.unityTextAlign=TextAnchor.MiddleLeft;}
  if(!string.IsNullOrEmpty(date)){var d=Technical(row,date,D.RowSubSize);d.style.color=KarineTheme.Accent;d.style.marginBottom=0;
   d.style.alignSelf=Align.FlexEnd;d.style.flexShrink=0;d.style.marginLeft=KarineTheme.SpaceSm;}
  list.Add(row);return row;
 }
 static VisualElement Dot(VisualElement parent,Color color) {
  var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=D.RowDot;dot.style.height=D.RowDot;dot.style.flexShrink=0;
  Round(dot,D.RowDot/2);dot.style.backgroundColor=color;parent.Add(dot);return dot;
 }
 // Kâğıt: düz krem yaprak, ince kenar, altında gölge, sağ üstte ataş.
 public static VisualElement DossierPage(VisualElement parent) {
  var shadow=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(shadow,new Rect(D.Page.x+.4f,D.Page.y+.8f,D.Page.width,D.Page.height));
  shadow.style.backgroundColor=KarineTheme.Alpha(Color.black,.35f);Round(shadow,KarineTheme.Radius);parent.Add(shadow);
  var paper=new VisualElement {name="DossierPaper"};OfficePlace(paper,D.Page);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(paper,KarineTheme.Radius);
  paper.style.paddingLeft=KarineTheme.SpaceXl+KarineTheme.SpaceMd;paper.style.paddingRight=KarineTheme.SpaceXl;
  paper.style.paddingTop=KarineTheme.SpaceXl;paper.style.paddingBottom=KarineTheme.SpaceLg;paper.style.overflow=Overflow.Hidden;
  parent.Add(paper);return paper;
 }
}
}
