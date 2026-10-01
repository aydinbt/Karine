using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 public static VisualElement CctvArchiveLayout(VisualElement parent,string title,out VisualElement cameras) {
  parent.name="CctvArchiveLayout";parent.style.flexDirection=FlexDirection.Row;parent.style.minHeight=0;
  var side=new VisualElement();side.style.width=Length.Percent(KarineTheme.CctvArchive.SidebarWidth);
  side.style.paddingRight=KarineTheme.SpaceMd;parent.Add(side);
  Icon(side,"binoculars",KarineTheme.Primary,KarineTheme.IconSize);
  Subtitle(side,title,KarineTheme.CctvArchive.TitleSize);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;side.Add(scroll);cameras=scroll;
  var main=new VisualElement {name="CctvArchiveRecords"};main.style.flexGrow=1;main.style.minWidth=0;
  main.style.width=Length.Percent(100-KarineTheme.CctvArchive.SidebarWidth);parent.Add(main);return main;
 }
 public static VisualElement CctvRecordPanel(VisualElement parent) {
  var panel=new VisualElement();panel.style.flexGrow=1;panel.style.minHeight=0;
  panel.style.backgroundColor=KarineTheme.GlassDeep;Border(panel,KarineTheme.BorderWidth,KarineTheme.Panel2);
  panel.style.paddingLeft=KarineTheme.SpaceSm;panel.style.paddingRight=KarineTheme.SpaceSm;parent.Add(panel);return panel;
 }
 public static VisualElement CctvRecordRow(VisualElement parent) {
  var row=new VisualElement();row.style.display=DisplayStyle.None;row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;row.style.minHeight=KarineTheme.CctvArchive.RowHeight;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceSm;
  row.style.borderBottomWidth=KarineTheme.BorderWidth;row.style.borderBottomColor=KarineTheme.Panel2;parent.Add(row);return row;
 }
}
}
