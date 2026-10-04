using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Görüşme odasının nesneleri: sallanan tavan lambası, aynalı camdaki belli
// belirsiz siluet ve duvar saati. Hepsi zamana bağlıdır, konuşmaya değil:
// lamba her yanıtta aynı salınır.
public static partial class KarineUI {
 static readonly float roomStart=Time.realtimeSinceStartup;

 static VisualElement RoomBox(VisualElement parent,string name,Rect box) {
  var element=new VisualElement {name=name,pickingMode=PickingMode.Ignore};
  OfficePlace(element,box);parent.Add(element);return element;
 }

 // Arka planın hemen üstüne (arayüzün altına) yerleşir. `hour` vaka saati;
 // saat oradan başlayıp gerçek zamanla ilerler.
 public static void InterviewRoom(VisualElement root,int insertAt,int hour) {
  if(root==null || !Fx.On)return;
  var layer=new VisualElement {name="InterviewRoomFx",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  root.Insert(Mathf.Clamp(insertAt,0,root.childCount),layer);
  Mirror(layer);Lamp(layer);Clock(layer,hour);
  int below=root.IndexOf(layer)-1;if(below>=0 && root[below] is Image background)CameraBreath(background);
 }

 static void Lamp(VisualElement layer) {
  var pivot=RoomBox(layer,"RoomLamp",S.RoomLamp);
  pivot.style.transformOrigin=new TransformOrigin(Length.Percent(50),0);
  var cord=new VisualElement {pickingMode=PickingMode.Ignore};
  cord.style.position=Position.Absolute;cord.style.left=Length.Percent(50);cord.style.top=0;cord.style.width=1;cord.style.height=Length.Percent(22);
  cord.style.backgroundColor=KarineTheme.Alpha(Color.black,.5f);pivot.Add(cord);
  var glow=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=KarineTheme.Alpha(KarineTheme.Office.Weather.BeamColor,S.LampAlpha*Fx.Amount)};
  glow.style.position=Position.Absolute;glow.style.left=Length.Percent(-60);glow.style.right=Length.Percent(-60);
  glow.style.top=Length.Percent(10);glow.style.bottom=Length.Percent(-90);pivot.Add(glow);
  // Gölge lambanın tersine kayar: oda ışıkla birlikte sallanıyormuş gibi.
  var shadow=new Image {image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=new Color(0,0,0,.18f*Fx.Amount)};
  shadow.style.position=Position.Absolute;shadow.style.left=Length.Percent(-4);shadow.style.right=Length.Percent(-4);
  shadow.style.top=0;shadow.style.bottom=0;layer.Insert(0,shadow);
  pivot.schedule.Execute(()=> {
   float swing=Mathf.Sin((Time.realtimeSinceStartup-roomStart)*Mathf.PI*2/S.LampSwaySeconds);
   pivot.style.rotate=new Rotate(Angle.Degrees(swing*S.LampSway));
   shadow.style.translate=new Translate(Length.Percent(-swing*1.5f),0);
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Aynalı cam: oyuncunun (Bora'nın) silueti, çok soluk, nefes alır gibi kıpırdar.
 static void Mirror(VisualElement layer) {
  var glass=RoomBox(layer,"RoomMirror",S.RoomMirror);
  var tint=KarineTheme.Alpha(Color.black,S.MirrorAlpha*4*Fx.Amount);
  var head=new VisualElement {pickingMode=PickingMode.Ignore};
  head.style.position=Position.Absolute;head.style.left=Length.Percent(36);head.style.width=Length.Percent(28);
  head.style.top=Length.Percent(18);head.style.height=Length.Percent(24);head.style.backgroundColor=tint;Round(head,60);glass.Add(head);
  var shoulders=new VisualElement {pickingMode=PickingMode.Ignore};
  shoulders.style.position=Position.Absolute;shoulders.style.left=Length.Percent(8);shoulders.style.right=Length.Percent(8);
  shoulders.style.top=Length.Percent(44);shoulders.style.bottom=0;shoulders.style.backgroundColor=tint;
  shoulders.style.borderTopLeftRadius=shoulders.style.borderTopRightRadius=40;glass.Add(shoulders);
  glass.style.opacity=.25f;
  glass.schedule.Execute(()=> {
   float t=Time.realtimeSinceStartup-roomStart;
   glass.style.translate=new Translate(Length.Percent(Mathf.Sin(t*.21f)*1.5f),0);
  }).Every(KarineTheme.Motion.TickMs*3);
 }

 // Duvar saati: vaka saatinden başlar, gerçek zamanla ilerler.
 static void Clock(VisualElement layer,int hour) {
  var face=RoomBox(layer,"RoomClock",S.RoomClock);
  face.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.75f);Round(face,60);
  Border(face,2,KarineTheme.Alpha(KarineTheme.GlassDeep,.9f));
  VisualElement Hand(float length,float width,Color color) {
   var hand=new VisualElement {pickingMode=PickingMode.Ignore};
   hand.style.position=Position.Absolute;hand.style.left=Length.Percent(50);hand.style.bottom=Length.Percent(50);
   hand.style.width=width;hand.style.marginLeft=-width*.5f;hand.style.height=Length.Percent(length);
   hand.style.backgroundColor=color;hand.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(100));
   face.Add(hand);return hand;
  }
  var hours=Hand(26,3,KarineTheme.GlassDeep);var minutes=Hand(38,2,KarineTheme.GlassDeep);var seconds=Hand(40,1,KarineTheme.Danger);
  double baseMinutes=(hour<0?DateTime.Now.Hour:hour)*60+(hour<0?DateTime.Now.Minute:7);
  face.schedule.Execute(()=> {
   double elapsed=Time.realtimeSinceStartup-roomStart;
   double total=baseMinutes+elapsed/60.0;
   hours.style.rotate=new Rotate(Angle.Degrees((float)(total/720.0*360.0)));
   minutes.style.rotate=new Rotate(Angle.Degrees((float)(total%60/60.0*360.0)));
   seconds.style.rotate=new Rotate(Angle.Degrees((float)Math.Floor(elapsed%60)*6f));
  }).Every(250);
 }
}
}
