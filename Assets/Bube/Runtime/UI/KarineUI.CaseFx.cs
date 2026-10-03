using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Stagecraft;

namespace Bube {
// Vakalar sayfasının canlılığı: arka plan yavaşça nefes alır, soldaki camda
// yağmur ve şimşek, asılı lamba hafifçe salınır ve ışığında toz süzülür.
// Kartlar masaya sırayla düşer. Her kart aynı hareketi alır; hiçbiri öne çıkmaz.
public static partial class KarineUI {

 public static void CaseBrowserStage(VisualElement backdrop,VisualElement root) {
  if(backdrop==null)return;
  int at=root.IndexOf(backdrop)+1;
  var frame=new Image {name="CaseVignette",image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=new Color(0,0,0,S.VignetteAlpha)};
  Percent(frame,new Rect(0,0,100,100));root.Insert(at,frame);
  if(!Fx.On)return;
  float start=Time.realtimeSinceStartup;
  // Kamera nefesi: ana menüdekiyle aynı yavaş yaklaşma ve kayma.
  backdrop.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(30));
  backdrop.schedule.Execute(()=> {
   float t=(Time.realtimeSinceStartup-start)/S.DriftSeconds;
   float wave=.5f-.5f*Mathf.Cos(t*Mathf.PI*2);
   backdrop.style.scale=new Scale(Vector3.one*(1.01f+S.DriftZoom*wave));
   backdrop.style.translate=new Translate(S.DriftPan*wave,S.DriftPan*.3f*wave);
  }).Every(KarineTheme.Motion.TickMs*2);
  // Pencere camında yağmur.
  var glass=new VisualElement {name="CaseRain",pickingMode=PickingMode.Ignore};
  Percent(glass,S.CaseWindow);glass.style.overflow=Overflow.Hidden;root.Insert(at,glass);
  var random=new System.Random(5);var streaks=new List<(VisualElement e,float x,float speed,float phase)>();
  for(int i=0;i<Fx.Count(S.CaseRain);i++) {
   var streak=new VisualElement {pickingMode=PickingMode.Ignore};
   streak.style.position=Position.Absolute;streak.style.width=1;streak.style.height=Length.Percent(4+(float)random.NextDouble()*5);
   streak.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.06f+(float)random.NextDouble()*.10f);
   streak.style.rotate=new Rotate(Angle.Degrees(6));glass.Add(streak);
   streaks.Add((streak,(float)random.NextDouble()*110,70+(float)random.NextDouble()*50,(float)random.NextDouble()*100));
  }
  glass.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   foreach(var s in streaks){float y=Mathf.Repeat(s.phase+time*s.speed,120)-10;s.e.style.left=Length.Percent(s.x-y*.1f);s.e.style.top=Length.Percent(y);}
  }).Every(KarineTheme.Motion.TickMs*2);
  Lightning(root,at+1,S.CaseWindow);
  // Asılı lamba: sıcak hale tavandan sarkıyormuş gibi hafifçe sağa sola salınır.
  var halo=new Image {name="CaseLamp",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  Percent(halo,new Rect(S.CaseLampX-S.CaseLampSize/2,S.CaseLampY-S.CaseLampSize*.4f,S.CaseLampSize,S.CaseLampSize*1.3f));
  halo.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(0));root.Insert(at+1,halo);
  Dust(root,at+2,S.CaseDust);
  halo.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   float swing=Mathf.Sin(time*Mathf.PI*2/S.LampSwingSeconds);
   halo.style.rotate=new Rotate(Angle.Degrees(swing*S.LampSwing*2));
   halo.style.translate=new Translate(Length.Percent(swing*S.LampSwing),0);
   float flicker=Mathf.PerlinNoise(time*6f,.7f)>.85f?.6f:1f;
   halo.tintColor=KarineTheme.Alpha(KarineTheme.Accent,(.14f+.04f*Mathf.Sin(time*.8f))*flicker*Fx.Amount);
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Kartlar masaya sırayla düşer: biraz yukarıdan, hafif eğik başlayıp düzelir.
 public static void CardsDrop(VisualElement strip) {
  if(strip==null || KarineMotion.Reduced)return;
  int i=0;
  foreach(var child in strip.Children()) {
   var card=child;int order=i++;float tilt=(order%2==0?-1:1)*S.DropTilt;
   card.style.opacity=0;
   card.schedule.Execute(()=>KarineMotion.Run(card,S.DropSeconds,t=> {
    card.style.opacity=Mathf.Clamp01(t*2);
    card.style.translate=new Translate(0,-S.DropFrom*(1-t));
    card.style.rotate=new Rotate(Angle.Degrees(tilt*(1-t)));
   },()=>{card.style.opacity=StyleKeyword.Null;card.style.translate=StyleKeyword.Null;card.style.rotate=StyleKeyword.Null;})).StartingIn(order*S.DropStaggerMs);
  }
 }

 // Üstüne gelince kart kalkar, altında gölge belirir, fotoğraf hafifçe büyür.
 static void CardHover(VisualElement card,VisualElement photo) {
  card.RegisterCallback<PointerEnterEvent>(_=> {
   if(KarineMotion.Reduced)return;
   card.style.translate=new Translate(0,-S.HoverLift);Lift(card,.5f);
   if(photo!=null)photo.style.scale=new Scale(Vector3.one*S.PhotoZoom);
  });
  card.RegisterCallback<PointerLeaveEvent>(_=> {
   card.style.translate=StyleKeyword.Null;Lift(card,0);
   if(photo!=null)photo.style.scale=StyleKeyword.Null;
  });
 }

 // Kilitli kart dokununca "açılmıyor" der gibi kısa sarsılır.
 static void LockedShake(VisualElement card) {
  card.RegisterCallback<PointerDownEvent>(_=> {
   Cue("drawer");
   KarineMotion.Run(card,S.ShakeSeconds,t=>card.style.translate=new Translate(Mathf.Sin(t*Mathf.PI*6)*S.ShakePx*(1-t),0),
    ()=>card.style.translate=StyleKeyword.Null);
  });
 }

 // Kimlik kartındaki polaroid üstüne gelince düzelir.
 static void Straighten(VisualElement card,VisualElement frame) {
  card.RegisterCallback<PointerEnterEvent>(_=>{if(!KarineMotion.Reduced)frame.style.rotate=new Rotate(Angle.Degrees(0));});
  card.RegisterCallback<PointerLeaveEvent>(_=>frame.style.rotate=new Rotate(Angle.Degrees(S.PolaroidTilt)));
 }
}
}
