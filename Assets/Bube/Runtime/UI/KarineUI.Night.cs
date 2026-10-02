using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;
using W = Bube.KarineTheme.Office.Weather;

namespace Bube {
// Gecenin ofisi: pencereden düşen ay ışığı, yağmurlu vakada uzak gök gürültüsü
// ve pencerenin bir an aydınlanması, perdenin gölgesi, isteğe bağlı kül
// tablası dumanı. Hepsi kendi saatinde olur; oyunun hiçbir anına bağlı değil.
public static partial class KarineUI {
 public static bool AshSmoke;

 public static void OfficeNight(VisualElement stage,int hour,string weather) {
  var back=stage?.Q("OfficeBack");var front=stage?.Q("OfficeFront");
  if(back==null || front==null || !Fx.On)return;
  bool night=hour<0 || hour>=19 || hour<6;
  if(night)Moonlight(back);
  Curtain(back);
  if(weather=="rain")Thunder(back);
  if(AshSmoke)Wisp(front,"OfficeAsh",S.AshArea,KarineTheme.Alpha(KarineTheme.Paper.Faded,.12f),3);
 }

 // Ay ışığı: pencereden aşağı-sola eğik, soğuk ve çok soluk bir ışık, yavaş nefes.
 static void Moonlight(VisualElement back) {
  var beam=new Image {name="OfficeMoon",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=KarineTheme.Alpha(S.Moon,S.MoonAlpha*Fx.Amount)};
  var w=KarineTheme.Office.Window;
  OfficePlace(beam,new Rect(w.x-12,w.y+w.height*.4f,w.width*1.1f,70));
  beam.style.rotate=new Rotate(Angle.Degrees(14));beam.style.transformOrigin=new TransformOrigin(Length.Percent(60),0);
  back.Add(beam);
  float start=Time.realtimeSinceStartup;
  beam.schedule.Execute(()=>beam.style.opacity=.8f+.2f*Mathf.Sin((Time.realtimeSinceStartup-start)*.3f)).Every(KarineTheme.Motion.TickMs*4);
 }

 // Perde gölgesi: pencerenin sağ kenarında yavaşça salınan koyu bir şerit.
 static void Curtain(VisualElement back) {
  var w=KarineTheme.Office.Window;
  var shade=new Image {name="OfficeCurtain",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=new Color(0,0,0,S.CurtainAlpha*Fx.Amount)};
  OfficePlace(shade,new Rect(w.x+w.width-6,w.y,9,w.height));back.Add(shade);
  float start=Time.realtimeSinceStartup;
  shade.schedule.Execute(()=> {
   float t=(Time.realtimeSinceStartup-start)*Mathf.PI*2/S.CurtainSeconds;
   shade.style.translate=new Translate(Length.Percent(Mathf.Sin(t)*8),0);
   shade.style.scale=new Scale(new Vector3(1+.15f*Mathf.Sin(t*1.3f),1,1));
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Gök gürültüsü: önce pencere bir an aydınlanır (ışığa duyarlılık sınırıyla),
 // ses biraz sonra uzaktan gelir.
 static void Thunder(VisualElement back) {
  var flash=new VisualElement {name="OfficeLightning",pickingMode=PickingMode.Ignore};
  OfficePlace(flash,KarineTheme.Office.Window);flash.style.backgroundColor=new Color(.85f,.9f,1f,1);flash.style.opacity=0;back.Add(flash);
  var random=new System.Random();
  Action plan=null;
  plan=()=>flash.schedule.Execute(()=> {
   if(flash.panel==null)return;
   if(Fx.On) {
    if(Fx.MayFlash()) {
     flash.style.opacity=S.FlashAlpha*Fx.Amount;
     flash.schedule.Execute(()=>flash.style.opacity=0).StartingIn(90);
    }
    float pan=(float)random.NextDouble()*1.4f-.7f;
    flash.schedule.Execute(()=>SoundAt?.Invoke("amb_thunder",pan,S.ThunderGain)).StartingIn(700+random.Next(900));
   }
   plan();
  }).StartingIn(random.Next(S.ThunderMinMs,S.ThunderMaxMs));
  plan();
 }

 // İnce duman: alanın dibinden yükselir, kıvrılır, söner. Buhar da bunu kullanır.
 static void Wisp(VisualElement front,string name,Rect box,Color color,int count) {
  var area=new VisualElement {name=name,pickingMode=PickingMode.Ignore};
  OfficePlace(area,box);
  int firstButton=-1;for(int i=0;i<front.childCount;i++)if(front[i] is Button){firstButton=i;break;}
  if(firstButton<0)front.Add(area);else front.Insert(firstButton,area);
  var random=new System.Random(name.Length*31);
  var puffs=new System.Collections.Generic.List<(Image e,float phase,float speed,float sway)>();
  for(int i=0;i<Fx.Count(count);i++) {
   var puff=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=color};
   puff.style.position=Position.Absolute;puff.style.width=Length.Percent(60);puff.style.height=Length.Percent(22);area.Add(puff);
   puffs.Add((puff,(float)random.NextDouble(),.10f+(float)random.NextDouble()*.06f,(float)random.NextDouble()*6));
  }
  float start=Time.realtimeSinceStartup;
  area.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   foreach(var p in puffs) {
    float life=Mathf.Repeat(p.phase+time*p.speed,1f);
    p.e.style.top=Length.Percent(78-life*80);
    p.e.style.left=Length.Percent(20+Mathf.Sin(time*.8f+p.sway)*14*life);
    p.e.style.scale=new Scale(Vector3.one*(.5f+life));
    p.e.style.opacity=Mathf.Sin(life*Mathf.PI)*.9f;
   }
  }).Every(KarineTheme.Motion.TickMs*2);
 }
}
}
