using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 // Tablet talep ekranı (yeni görünüm, 2 Ekim 2026): solda BDS rayı ve sayaçlı iki sekme,
 // ortada başlık + yönerge + kişi/inceleme listesi, sağda kâğıt ayrıntı kartı.
 public static void RequestLayout(VisualElement parent,string interviews,string investigations,bool active,Action showPeople,Action showDocuments,out VisualElement list,out VisualElement detail,
  string railSubtitle=null,string hint=null,int interviewCount=0,int investigationCount=0) {
  parent.style.flexDirection=FlexDirection.Row;parent.style.minHeight=0;
  var rail=new VisualElement();rail.style.width=Length.Percent(KarineTheme.Requests.RailWidth);rail.style.paddingRight=KarineTheme.SpaceMd;
  rail.style.borderRightWidth=1;rail.style.borderRightColor=KarineTheme.GlassLift;rail.style.marginRight=KarineTheme.SpaceMd;parent.Add(rail);
  Title(rail,"BDS",KarineTheme.Requests.TitleSize+KarineTheme.SpaceMd).style.marginBottom=0;
  if(!string.IsNullOrEmpty(railSubtitle)){var sub=Body_(rail,railSubtitle,KarineTheme.Dossier.MetaSize);sub.style.color=KarineTheme.Secondary;sub.style.marginBottom=KarineTheme.SpaceLg;}
  RequestTab(rail,"people",interviews,interviewCount,active,showPeople);
  RequestTab(rail,"document",investigations,investigationCount,!active,showDocuments);
  var middle=new VisualElement();middle.style.width=Length.Percent(KarineTheme.Requests.ListWidth);middle.style.paddingRight=KarineTheme.SpaceMd;parent.Add(middle);
  Subtitle(middle,active?interviews:investigations,KarineTheme.Requests.TitleSize).style.marginBottom=0;
  if(!string.IsNullOrEmpty(hint)){var h=Body_(middle,hint,KarineTheme.Dossier.MetaSize);h.style.color=KarineTheme.Secondary;h.style.marginBottom=KarineTheme.SpaceMd;}
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;middle.Add(scroll);list=scroll;
  detail=new VisualElement {name="RequestDetail"};detail.style.width=Length.Percent(KarineTheme.Requests.DetailWidth);
  // Koyu kart: beyaz kâğıt tablet ekranında sırıtıyordu. İçerik kâğıt mürekkebiyle yazılır;
  // yerleşim bitince (ilk çizimden önce) mürekkep tonları ekran tonlarına çevrilir.
  detail.style.backgroundColor=KarineTheme.GlassDeep;Border(detail,KarineTheme.BorderWidth,KarineTheme.Panel2);Round(detail,KarineTheme.Radius);
  var card=detail;card.RegisterCallback<GeometryChangedEvent>(_=>OnScreenInk(card));
  detail.style.paddingLeft=KarineTheme.SpaceLg;detail.style.paddingRight=KarineTheme.SpaceLg;
  detail.style.paddingTop=KarineTheme.SpaceLg;detail.style.paddingBottom=KarineTheme.SpaceLg;parent.Add(detail);
 }
 // Simge ve sayaç üstte, yazı altta tam genişlikte: yan yana dar sütunda "GÖRÜŞME/LER" diye bölünüyordu.
 static void RequestTab(VisualElement rail,string icon,string title,int count,bool active,Action click) {
  var tab=Button_(rail,"",click,active?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  tab.style.flexDirection=FlexDirection.Column;tab.style.alignItems=Align.Stretch;tab.style.marginBottom=KarineTheme.SpaceSm;
  tab.style.paddingLeft=KarineTheme.SpaceSm;tab.style.paddingRight=KarineTheme.SpaceSm;tab.style.paddingTop=KarineTheme.SpaceSm;tab.style.paddingBottom=KarineTheme.SpaceSm;
  var ink=active?KarineTheme.Paper.Ink:KarineTheme.Primary;
  var top=new VisualElement {pickingMode=PickingMode.Ignore};top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.Center;top.style.justifyContent=Justify.SpaceBetween;top.style.marginBottom=KarineTheme.SpaceXs;tab.Add(top);
  Icon(top,icon,ink,KarineTheme.IconSize);
  if(count>0) {
   var badge=Body_(top,count.ToString(),KarineTheme.Dossier.MetaSize);badge.pickingMode=PickingMode.Ignore;
   badge.style.color=KarineTheme.Primary;badge.style.backgroundColor=KarineTheme.Danger;badge.style.marginBottom=0;
   badge.style.minWidth=KarineTheme.Requests.Badge;badge.style.height=KarineTheme.Requests.Badge;badge.style.unityTextAlign=TextAnchor.MiddleCenter;
   Round(badge,KarineTheme.Radius);
  }
  var label=Body_(tab,title,KarineTheme.Dossier.MetaSize);label.style.color=ink;label.style.marginBottom=0;
  label.style.whiteSpace=WhiteSpace.NoWrap;label.style.unityTextAlign=TextAnchor.MiddleLeft;
 }
 public static Button RequestItem(VisualElement list,Texture2D portrait,string title,string info,bool selected,Action click,string status=null,bool fresh=false,string quote=null) {
  var button=Button_(list,"",click,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.minHeight=KarineTheme.Requests.RowHeight;button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;
  button.style.marginBottom=KarineTheme.SpaceSm;button.style.paddingLeft=KarineTheme.SpaceSm;button.style.paddingRight=KarineTheme.SpaceSm;
  if(portrait!=null) {var photo=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop};photo.style.width=KarineTheme.Requests.Portrait;photo.style.height=KarineTheme.Requests.Portrait;photo.style.flexShrink=0;photo.style.marginRight=KarineTheme.SpaceMd;button.Add(photo);}
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;words.style.unityTextAlign=TextAnchor.MiddleLeft;button.Add(words);
  var name=Subtitle(words,title,KarineTheme.Requests.BodySize+2);name.style.color=selected?KarineTheme.Paper.Ink:KarineTheme.Primary;name.style.marginBottom=0;
  var faded=selected?KarineTheme.Paper.Faded:KarineTheme.Secondary;
  if(!string.IsNullOrEmpty(status)) {
   var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.Center;words.Add(head);
   if(fresh){var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=KarineTheme.SpaceSm+2;dot.style.height=KarineTheme.SpaceSm+2;dot.style.backgroundColor=KarineTheme.Danger;Round(dot,KarineTheme.SpaceSm);dot.style.marginRight=KarineTheme.SpaceXs;head.Add(dot);}
   var chip=Body_(head,status.TrimStart('●',' '),KarineTheme.Dossier.MetaSize);chip.style.color=faded;chip.style.marginBottom=0;
  }
  var detail=Body_(words,info,KarineTheme.Dossier.MetaSize);detail.style.color=faded;detail.style.marginBottom=0;
  if(!string.IsNullOrEmpty(quote)){var q=Body_(words,quote,KarineTheme.Dossier.MetaSize);q.style.color=faded;q.style.marginBottom=0;q.style.unityFontStyleAndWeight=FontStyle.Italic;}
  var chevron=Icon(button,"nav_next",faded,KarineTheme.Requests.Chevron);chevron.style.marginLeft=KarineTheme.SpaceSm;chevron.pickingMode=PickingMode.Ignore;
  return button;
 }
 static void OnScreenInk(VisualElement card) {
  card.Query<Label>().ForEach(l=> {
   var c=l.resolvedStyle.color;
   if(c==KarineTheme.Paper.Ink)l.style.color=KarineTheme.Primary;
   else if(c==KarineTheme.Paper.Faded)l.style.color=KarineTheme.Secondary;
  });
 }
 public static VisualElement RequestDetail(VisualElement parent,Texture2D portrait,string title,string info,string status) {
  parent.Clear();var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;parent.Add(scroll);
  // Portre üstte, yazı altta: yan yana dar sütunda kelimeler bölünüyordu.
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Column;top.style.marginBottom=KarineTheme.SpaceMd;scroll.Add(top);
  if(portrait!=null) {var photo=new Image{image=portrait,scaleMode=ScaleMode.ScaleAndCrop};photo.style.width=KarineTheme.Requests.DetailPortrait;photo.style.height=KarineTheme.Requests.DetailPortrait;photo.style.flexShrink=0;photo.style.marginBottom=KarineTheme.SpaceSm;top.Add(photo);}
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;top.Add(words);
  var name=Subtitle(words,title,KarineTheme.Requests.TitleSize);name.style.color=KarineTheme.Paper.Ink;
  DossierText(words,info,KarineTheme.Requests.BodySize).style.color=KarineTheme.Paper.Faded;
  DossierText(words,status,KarineTheme.Dossier.MetaSize);
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Panel2;rule.style.marginBottom=KarineTheme.SpaceMd;scroll.Add(rule);
  return scroll;
 }
}
}
