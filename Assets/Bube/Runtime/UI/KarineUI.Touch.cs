using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Dokunsal ayrıntılar: fotoğrafı ışığa tutmak (cihaz eğilince parlama kayar)
// ve defterde oyuncunun kendi işaretinin kalemle yazılması.
//
// Parlama yalnız yüzeydir: fotoğrafta gizli bir şey göstermez, her fotoğrafta aynıdır.
public static partial class KarineUI {
 public static void Tilt(VisualElement frame) {
  if(frame==null || !Fx.On)return;
  var shine=new Image {name="PhotoShine",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=new Color(1,1,1,.16f*Fx.Amount)};
  shine.style.position=Position.Absolute;shine.style.width=Length.Percent(60);shine.style.height=Length.Percent(140);
  shine.style.top=Length.Percent(-20);shine.style.rotate=new Rotate(Angle.Degrees(-20));frame.Add(shine);
  float start=Time.realtimeSinceStartup;Vector2 smooth=Vector2.zero;
  shine.schedule.Execute(()=> {
   // Cihazda ivmeölçer; masaüstünde ivme sıfırdır, parlama yavaşça kendi süzülür.
   var a=Input.acceleration;
   Vector2 want=a.sqrMagnitude>.01f?new Vector2(a.x,a.y+.5f):new Vector2(Mathf.Sin((Time.realtimeSinceStartup-start)*.4f)*.6f,0);
   smooth=Vector2.Lerp(smooth,want,.12f);
   shine.style.left=Length.Percent(20+Mathf.Clamp(smooth.x,-1,1)*55);
   shine.style.opacity=.5f+Mathf.Clamp01(Mathf.Abs(smooth.y))*.5f;
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Kalem: yazı eğik durur, altına mürekkep çizgisi soldan sağa çekilir.
 public static void Handwrite(Label label,Color ink) {
  if(label==null)return;
  label.style.unityFontStyleAndWeight=FontStyle.Italic;
  var line=new VisualElement {pickingMode=PickingMode.Ignore};
  line.style.position=Position.Absolute;line.style.left=0;line.style.bottom=1;line.style.height=1.5f;
  line.style.backgroundColor=KarineTheme.Alpha(ink,.7f);line.style.width=Length.Percent(Fx.On?0:100);label.Add(line);
  if(!Fx.On)return;
  Cue("pen");
  KarineMotion.Run(line,.45f,t=>line.style.width=Length.Percent(100*(1-(1-t)*(1-t))),null);
 }
}
}
