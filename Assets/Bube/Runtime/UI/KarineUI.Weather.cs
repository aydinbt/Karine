using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using W = Bube.KarineTheme.Office.Weather;

namespace Bube {
// Masanın hâli: vakanın saatine göre ışık tonu, yağmurlu vakada camdaki
// yağmur, gece pencereden geçen far, fincandan yükselen buhar ve lambanın
// seyrek titremesi.
//
// Hepsi vaka verisinden gelir (`CaseData.deskHour`, `CaseData.weather`) ve
// oyunun ilerleyişinden **bağımsızdır**: far, titreme ve buhar kendi rastgele
// saatlerinde olur; hiçbir okuma, soru ya da kayıt onları tetiklemez.
public static partial class KarineUI {
 // Konumlu ses: kimlik, sol-sağ (-1..1), kazanç. `BubeApp` bağlar.
 public static Action<string,float,float> SoundAt;

 // Masa sahnesinin x yüzdesini sol-sağ değerine çevirir.
 public static float PanOf(Rect box) => Mathf.Clamp((box.x+box.width*.5f)/50f-1f,-1,1)*W.PanWidth;

 public static void OfficeWeather(VisualElement stage,int hour,string weather,bool arriving) {
  var back=stage.Q("OfficeBack");var front=stage.Q("OfficeFront");var light=stage.Q<Image>("OfficeLight");
  if(back==null || front==null)return;
  HourTone(front,hour);
  if(arriving)LampOn(light);
  if(!Fx.On)return;
  if(weather=="rain")Rain(back);
  Steam(front);
  bool night=hour<0 || hour>=19 || hour<6;
  if(night)Headlights(back);
  LampFlicker(light);
 }

 // Saat tonu: masa resmi gecedir; saat ışığın rengini ve koyuluğunu değiştirir.
 // Gün içi daha açık ve nötr, akşam sıcak, gece yarısından sonra soğuk ve koyu.
 static void HourTone(VisualElement front,int hour) {
  if(hour<0)return;
  Color tone;float alpha;
  if(hour>=6 && hour<17){tone=W.DayTone;alpha=W.DayAlpha;}
  else if(hour>=17 && hour<22){tone=W.EveningTone;alpha=W.EveningAlpha;}
  else {tone=W.LateTone;alpha=W.LateAlpha;}
  var wash=new VisualElement {name="OfficeHourTone",pickingMode=PickingMode.Ignore};
  OfficePlace(wash,new Rect(0,0,100,100));wash.style.backgroundColor=KarineTheme.Alpha(tone,alpha);
  int firstButton=-1;for(int i=0;i<front.childCount;i++)if(front[i] is Button){firstButton=i;break;}
  if(firstButton<0)front.Add(wash);else front.Insert(firstButton,wash);
 }

 // Masaya dönüş: lamba tıkla yanar, ışık iki kez kekeleyip oturur.
 static void LampOn(Image light) {
  if(light==null)return;
  Sound?.Invoke("ui_lamp");
  if(!Fx.On)return;
  // Opaklık masanın nefesine ait; yanma ve titreme ışığın kendi tonunu kısar.
  var lit=light.tintColor;
  KarineMotion.Run(light,W.LampOnSeconds,t=> {
   float stutter=t<.18f?0:t<.26f?.6f:t<.36f?.1f:1f;
   light.tintColor=KarineTheme.Alpha(lit,lit.a*stutter*Mathf.Lerp(.7f,1,t));
  },()=>light.tintColor=lit);
 }

 // Lambanın seyrek titremesi: 25-70 saniyede bir, bir iki kısa sönme.
 static void LampFlicker(Image light) {
  if(light==null)return;
  var random=new System.Random();
  Action plan=null;
  plan=()=> {
   light.schedule.Execute(()=> {
    if(light.panel!=null && Fx.On && Fx.MayFlash()) {
     var lit=light.tintColor;
     light.tintColor=KarineTheme.Alpha(lit,lit.a*W.FlickerLow);
     light.schedule.Execute(()=>light.tintColor=lit).StartingIn(W.FlickerMs);
    }
    plan();
   }).StartingIn(random.Next(W.FlickerMinMs,W.FlickerMaxMs));
  };
  plan();
 }

 // Yağmur: camda eğik ince çizgiler düşer, birkaç damla camdan yavaşça süzülür.
 static void Rain(VisualElement back) {
  var glass=new VisualElement {name="OfficeRain",pickingMode=PickingMode.Ignore};
  OfficePlace(glass,KarineTheme.Office.Window);glass.style.overflow=Overflow.Hidden;back.Add(glass);
  var random=new System.Random(23);
  var streaks=new List<(VisualElement e,float x,float speed,float phase)>();
  for(int i=0;i<Fx.Count(W.RainStreaks);i++) {
   var streak=new VisualElement {pickingMode=PickingMode.Ignore};
   streak.style.position=Position.Absolute;streak.style.width=1;streak.style.height=Length.Percent(W.StreakLength);
   streak.style.backgroundColor=KarineTheme.Alpha(W.RainColor,W.StreakAlpha*(.5f+(float)random.NextDouble()*.5f));
   streak.style.rotate=new Rotate(Angle.Degrees(W.RainSlant));glass.Add(streak);
   streaks.Add((streak,(float)random.NextDouble()*110-5,W.RainSpeed*(.7f+(float)random.NextDouble()*.6f),(float)random.NextDouble()*100));
  }
  var drops=new List<(VisualElement e,float x,float speed,float phase,float size)>();
  for(int i=0;i<Fx.Count(W.RainDrops);i++) {
   float size=2+(float)random.NextDouble()*3;
   var drop=new VisualElement {pickingMode=PickingMode.Ignore};
   drop.style.position=Position.Absolute;drop.style.width=size;drop.style.height=size*1.4f;Round(drop,Mathf.CeilToInt(size));
   drop.style.backgroundColor=KarineTheme.Alpha(W.RainColor,W.DropAlpha);glass.Add(drop);
   drops.Add((drop,(float)random.NextDouble()*100,W.DropSpeed*(.4f+(float)random.NextDouble()),(float)random.NextDouble()*100,size));
  }
  float start=Time.realtimeSinceStartup;
  glass.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   foreach(var s in streaks) {
    float y=Mathf.Repeat(s.phase+time*s.speed,130f)-20f;
    s.e.style.left=Length.Percent(s.x-y*.12f);s.e.style.top=Length.Percent(y);
   }
   foreach(var d in drops) {
    // Damla durur, birden kayar, yine durur: camdaki gerçek damla gibi.
    float cycle=Mathf.Repeat(d.phase+time*d.speed,100f);
    float y=cycle<60?cycle*.25f:15+(cycle-60)*2.1f;
    d.e.style.left=Length.Percent(d.x);d.e.style.top=Length.Percent(y);
    d.e.style.opacity=y>95?0:1;
   }
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Far: gece seyrek bir araba geçer, ışığı odanın duvarını süpürür, sesi
 // pencerenin bir yanından öbür yanına geçer.
 static void Headlights(VisualElement back) {
  var beam=new Image {name="OfficeHeadlight",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=KarineTheme.Alpha(W.BeamColor,W.BeamAlpha*Fx.Amount)};
  OfficePlace(beam,new Rect(-40,0,W.BeamWidth,100));beam.style.opacity=0;back.Add(beam);
  var random=new System.Random();
  Action plan=null;
  plan=()=>beam.schedule.Execute(()=> {
   if(beam.panel==null)return;
   if(Fx.On) {
    bool leftToRight=random.Next(2)==0;
    SoundAt?.Invoke("amb_car",leftToRight?-.6f:.6f,W.CarGain);
    KarineMotion.Run(beam,W.BeamSeconds,t=> {
     float x=leftToRight?Mathf.Lerp(-W.BeamWidth,100,t):Mathf.Lerp(100,-W.BeamWidth,t);
     beam.style.left=Length.Percent(x);beam.style.opacity=Mathf.Sin(t*Mathf.PI);
    },()=>beam.style.opacity=0);
   }
   plan();
  }).StartingIn(random.Next(W.BeamMinMs,W.BeamMaxMs));
  plan();
 }

 // Buhar: masanın solundaki fincandan yükselen, kıvrılıp sönen ince duman.
 static void Steam(VisualElement front) {
  var area=new VisualElement {name="OfficeSteam",pickingMode=PickingMode.Ignore};
  OfficePlace(area,W.SteamArea);
  int firstButton=-1;for(int i=0;i<front.childCount;i++)if(front[i] is Button){firstButton=i;break;}
  if(firstButton<0)front.Add(area);else front.Insert(firstButton,area);
  var random=new System.Random(5);
  var puffs=new List<(Image e,float phase,float speed,float sway)>();
  for(int i=0;i<Fx.Count(W.SteamPuffs);i++) {
   var puff=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(KarineTheme.Paper.Light,W.SteamAlpha)};
   puff.style.position=Position.Absolute;puff.style.width=Length.Percent(60);puff.style.height=Length.Percent(22);area.Add(puff);
   puffs.Add((puff,(float)random.NextDouble(),.12f+(float)random.NextDouble()*.08f,(float)random.NextDouble()*6));
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
