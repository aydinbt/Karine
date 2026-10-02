using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
public static partial class KarineUI {
 public static void IncomingPaper(VisualElement stage) {
  var paper=new VisualElement {name="IncomingPaper",pickingMode=PickingMode.Ignore};
  var box=KarineTheme.Office.Inbox;
  OfficePlace(paper,new Rect(box.x+2,box.y+1,box.width-4,box.height/3));
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);stage.Add(paper);
  KarineMotion.Run(paper,KarineTheme.Motion.PaperArrivalSeconds,t=>{
   paper.style.translate=new Translate(0,-KarineTheme.Motion.PaperOffset*2*(1-t));
   // Kâğıt hafif yan düşer ve tepsiye oturdukça düzelir.
   paper.style.rotate=new Rotate(Angle.Degrees(-6f*(1-t)-1f));
   paper.style.opacity=t<.8f?1:Mathf.Clamp01((1-t)/.2f);
  },()=>paper.RemoveFromHierarchy());
 }

 public static void OfficePlace(VisualElement element,Rect box) {
  element.style.position=Position.Absolute;
  element.style.left=Length.Percent(box.x);element.style.top=Length.Percent(box.y);
  element.style.width=Length.Percent(box.width);element.style.height=Length.Percent(box.height);
 }
 public static VisualElement OfficeStage(VisualElement parent,string country) {
  var stage=new VisualElement {name="OfficeStage"};parent.Add(stage);
  stage.style.position=Position.Absolute;stage.style.overflow=Overflow.Hidden;
  Action fit=()=> {
   var size=parent.contentRect.size;
   float width=Mathf.Min(size.x,size.y*KarineTheme.Office.Aspect),height=width/KarineTheme.Office.Aspect;
   stage.style.width=width;stage.style.height=height;
   stage.style.left=(size.x-width)*.5f;stage.style.top=(size.y-height)*.5f;
  };
  EventCallback<GeometryChangedEvent> resize=_=>fit();
  parent.RegisterCallback(resize);
  stage.RegisterCallback<DetachFromPanelEvent>(_=>parent.UnregisterCallback(resize));
  fit();
  // Boş masa plakası + ayrı eşya kesitleri (2 Ekim 2026). Pencere manzarası plakanın içinde.
  var room=new Image {name="OfficeRoom",image=Resources.Load<Texture2D>("Bube/Art/OfficeDesk"),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  OfficePlace(room,new Rect(0,0,100,100));stage.Add(room);
  var window=new VisualElement {name="OfficeWindow-"+country,pickingMode=PickingMode.Ignore};
  OfficePlace(window,KarineTheme.Office.Window);stage.Add(window);
  OfficeProp(stage,KarineTheme.Office.Lamp,"DeskLamp","lamp");
  OfficeProp(stage,KarineTheme.Office.Inbox,"InboxTray","inbox");
  OfficeProp(stage,KarineTheme.Office.Phone,"DeskPhone","phone");
  OfficeProp(stage,KarineTheme.Office.Folder,"BlankCaseFolder","folder");
  OfficeProp(stage,KarineTheme.Office.Monitor,"CctvMonitor","tablet");
  OfficeProp(stage,KarineTheme.Office.Evidence,"EvidencePile","evidence");
  return stage;
 }
 // Boş masa plakasının üstüne konan ayrı eşya kesiti; dokununca tıklama hissi buna uygulanır.
 static void OfficeProp(VisualElement stage,Rect box,string name,string art) {
  var image=new Image {name=name,image=Resources.Load<Texture2D>("Bube/Art/Desk/"+art),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  OfficePlace(image,box);stage.Add(image);
 }
 public static Button OfficeAction(VisualElement stage,string name,string icon,string label,Rect box,Action action) {
  var button=new Button(Sounded(action)) {name=name,tooltip=label};
  OfficePlace(button,box);button.style.minHeight=KarineTheme.TouchTarget;
  button.style.marginLeft=0;button.style.marginRight=0;button.style.marginTop=0;button.style.marginBottom=0;
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;
  button.style.justifyContent=Justify.Center;
  button.style.paddingLeft=KarineTheme.SpaceSm;button.style.paddingRight=KarineTheme.SpaceSm;
  button.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);
  Border(button,KarineTheme.BorderWidth,KarineTheme.Accent);
  Icon(button,icon,KarineTheme.Primary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceSm;
  var text=Body_(button,label,KarineTheme.Office.LabelSize);text.style.marginBottom=0;
  text.style.flexGrow=1;
  Icon(button,"nav_next",KarineTheme.Primary,KarineTheme.IconSize);
  text.style.whiteSpace=WhiteSpace.Normal;text.style.unityTextAlign=TextAnchor.MiddleLeft;
  button.RegisterCallback<PointerEnterEvent>(_=>button.style.backgroundColor=KarineTheme.GlassLift);
  button.RegisterCallback<PointerLeaveEvent>(_=>button.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f));
  stage.Add(button);return button;
 }
 public static void OfficePortrait(VisualElement board,Texture2D art,string person) {
  var card=new VisualElement();card.style.width=KarineTheme.Office.PortraitWidth;
  card.style.marginRight=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceXs;card.style.paddingRight=KarineTheme.SpaceXs;
  card.style.paddingTop=KarineTheme.SpaceXs;card.style.backgroundColor=KarineTheme.Paper.Tint;
  var image=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  image.style.height=KarineTheme.Office.PortraitHeight;card.Add(image);
  var label=Body_(card,person,KarineTheme.Office.SmallSize);
  label.style.color=KarineTheme.Paper.Ink;label.style.marginBottom=KarineTheme.SpaceXs;
  board.Add(card);
 }
 public static void OfficeBrand(VisualElement parent,string title,Action action) {
  var button=new Button(Sounded(action)) {tooltip=title};
  button.style.width=KarineTheme.Office.BrandWidth;button.style.flexShrink=0;
  button.style.paddingLeft=0;button.style.paddingRight=0;button.style.paddingTop=0;button.style.paddingBottom=0;
  button.style.backgroundColor=Color.clear;Border(button,0,Color.clear);
  KarineLogo.Hero(button,KarineTheme.Office.BrandWidth);parent.Add(button);
 }
 public static Button OfficeHeaderAction(VisualElement parent,string icon,string title,Action action,bool selected=false) {
  var button=new Button(Sounded(action)) {tooltip=title};
  button.style.width=KarineTheme.Office.HeaderActionWidth;
  button.style.height=KarineTheme.TouchTargetComfortable;
  button.style.paddingLeft=0;button.style.paddingRight=0;
  button.style.flexShrink=0;button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  button.style.backgroundColor=KarineTheme.GlassDeep;
  Border(button,selected?KarineTheme.BorderWidth:0,KarineTheme.Accent);
  Icon(button,icon,KarineTheme.Primary,KarineTheme.IconSize);
  var label=Body_(button,title,KarineTheme.Office.SmallSize);
  label.style.marginTop=KarineTheme.SpaceXs;label.style.marginBottom=0;
  label.style.whiteSpace=WhiteSpace.NoWrap;label.style.width=Length.Percent(100);
  label.style.unityTextAlign=TextAnchor.MiddleCenter;
  parent.Add(button);return button;
 }
}
}
