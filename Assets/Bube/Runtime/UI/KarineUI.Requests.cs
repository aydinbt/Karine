using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 public static void RequestLayout(VisualElement parent,string interviews,string investigations,bool active,Action showPeople,Action showDocuments,out VisualElement list,out VisualElement detail) {
  parent.style.flexDirection=FlexDirection.Row;parent.style.minHeight=0;
  var rail=new VisualElement();rail.style.width=Length.Percent(KarineTheme.Requests.RailWidth);rail.style.paddingRight=KarineTheme.SpaceSm;parent.Add(rail);
  Title(rail,"BDS",KarineTheme.Requests.TitleSize);
  Button_(rail,interviews,showPeople,active?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  Button_(rail,investigations,showDocuments,active?KarineButtonKind.Secondary:KarineButtonKind.Primary);
  var middle=new VisualElement();middle.style.width=Length.Percent(KarineTheme.Requests.ListWidth);middle.style.paddingRight=KarineTheme.SpaceMd;parent.Add(middle);
  Subtitle(middle,active?interviews:investigations,KarineTheme.Requests.TitleSize);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;middle.Add(scroll);list=scroll;
  detail=new VisualElement {name="RequestDetail"};detail.style.width=Length.Percent(KarineTheme.Requests.DetailWidth);
  detail.style.backgroundColor=KarineTheme.Paper.Sheet;detail.style.paddingLeft=KarineTheme.SpaceMd;detail.style.paddingRight=KarineTheme.SpaceMd;
  detail.style.paddingTop=KarineTheme.SpaceMd;detail.style.paddingBottom=KarineTheme.SpaceMd;parent.Add(detail);
 }
 public static Button RequestItem(VisualElement list,Texture2D portrait,string title,string info,bool selected,Action click) {
  var button=Button_(list,"",click,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.minHeight=KarineTheme.Requests.RowHeight;button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;
  if(portrait!=null) {var photo=new Image {image=portrait,scaleMode=ScaleMode.ScaleToFit};photo.style.width=KarineTheme.Requests.Portrait;photo.style.height=KarineTheme.Requests.Portrait;photo.style.flexShrink=0;photo.style.marginRight=KarineTheme.SpaceSm;button.Add(photo);}
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;words.style.unityTextAlign=TextAnchor.MiddleLeft;button.Add(words);
  var name=Body_(words,title,KarineTheme.Requests.BodySize);name.style.color=selected?KarineTheme.Paper.Ink:KarineTheme.Primary;
  var status=Body_(words,info,KarineTheme.Dossier.MetaSize);status.style.color=selected?KarineTheme.Paper.Faded:KarineTheme.Secondary;status.style.marginBottom=0;
  return button;
 }
 public static VisualElement RequestDetail(VisualElement parent,Texture2D portrait,string title,string info,string status) {
  parent.Clear();var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;parent.Add(scroll);
  if(portrait!=null) {var photo=new Image{image=portrait,scaleMode=ScaleMode.ScaleToFit};photo.style.height=KarineTheme.Requests.DetailPortrait;scroll.Add(photo);}
  DossierText(scroll,title,KarineTheme.Requests.TitleSize);DossierText(scroll,info,KarineTheme.Requests.BodySize);
  DossierText(scroll,status,KarineTheme.Requests.BodySize);return scroll;
 }
}
}
