using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
public static partial class KarineUI {
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
  var room=new Image {image=Resources.Load<Texture2D>("Bube/Art/OfficeRoom"),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  OfficePlace(room,new Rect(0,0,100,100));stage.Add(room);
  var window=new VisualElement {name="OfficeWindow-"+country,pickingMode=PickingMode.Ignore};
  OfficePlace(window,KarineTheme.Office.Window);window.style.overflow=Overflow.Hidden;stage.Add(window);
  CountryPostcard(window,country);
  // Blinds and mullion are reusable geometry over the country postcard.
  var tint=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(tint,new Rect(0,0,100,100));
  tint.style.backgroundColor=KarineTheme.Veil(.22f);window.Add(tint);
  var mullion=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(mullion,new Rect(29,0,2,100));
  mullion.style.backgroundColor=KarineTheme.GlassDeep;window.Add(mullion);
  for(int i=0;i<5;i++) {
   var slat=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(slat,new Rect(0,i*5,100,2));
   slat.style.backgroundColor=KarineTheme.GlassDeep;window.Add(slat);
  }
  Border(window,KarineTheme.PrimaryEdgeWidth,KarineTheme.Paper.FolderDeep);
  OfficeProp(stage,4,KarineTheme.Office.Lamp,"DeskLamp");
  OfficeProp(stage,3,KarineTheme.Office.Inbox,"InboxTray");
  OfficeProp(stage,2,KarineTheme.Office.Phone,"DeskPhone");
  OfficeProp(stage,1,KarineTheme.Office.Folder,"BlankCaseFolder");
  OfficeProp(stage,0,KarineTheme.Office.Monitor,"CctvMonitor");
  OfficeProp(stage,5,KarineTheme.Office.Evidence,"EvidencePile");
  return stage;
 }
 static void OfficeProp(VisualElement stage,int index,Rect box,string name) {
  // Pixel bounds prevent atlas neighbours bleeding into another prop.
  Rect[] cells={new Rect(25,5,505,500),new Rect(530,90,495,390),new Rect(1020,70,516,420),
   new Rect(15,530,570,475),new Rect(590,480,420,515),new Rect(1010,555,526,469)};
  var r=cells[index];
  var image=new Image {name=name,image=Resources.Load<Texture2D>("Bube/Art/OfficeProps"),
   scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   uv=new Rect(r.x/1536f,1-(r.y+r.height)/1024f,r.width/1536f,r.height/1024f)};
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
 public static Button OfficeHeaderAction(VisualElement parent,string icon,string title,Action action) {
  var button=new Button(Sounded(action)) {tooltip=title};
  button.style.width=KarineTheme.Office.HeaderActionWidth;
  button.style.height=KarineTheme.TouchTargetComfortable;
  button.style.paddingLeft=0;button.style.paddingRight=0;
  button.style.flexShrink=0;button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  button.style.backgroundColor=KarineTheme.GlassDeep;
  Border(button,0,KarineTheme.GlassDeep);
  Icon(button,icon,KarineTheme.Primary,KarineTheme.IconSize);
  var label=Body_(button,title,KarineTheme.Office.SmallSize);
  label.style.marginTop=KarineTheme.SpaceXs;label.style.marginBottom=0;
  label.style.whiteSpace=WhiteSpace.NoWrap;label.style.width=Length.Percent(100);
  label.style.unityTextAlign=TextAnchor.MiddleCenter;
  parent.Add(button);return button;
 }
}
}
