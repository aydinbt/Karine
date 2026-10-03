using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Stagecraft;

namespace Bube {
// Ana menünün derinliği: karanlıktan açılış, satırların sırayla gelişi, satır
// üstünde amber çizgi, pencerede şimşek, lamba ışığında süzülen toz, kenar
// kararması. Hepsi hissettirir; hiçbir satırı ötekinden çok öne çıkarmaz.
public static partial class KarineUI {

 // Ekran siyahtan açılır, logo hafif büyüyerek belirir, satırlar yukarıdan
 // aşağı sırayla soldan kayar. "Hareketi azalt"ta yalnız son durum çizilir.
 public static void MenuIntro(VisualElement root,VisualElement logo,VisualElement column) {
  var curtain=new VisualElement {name="MenuCurtain",pickingMode=PickingMode.Ignore};
  curtain.style.position=Position.Absolute;curtain.style.left=0;curtain.style.right=0;curtain.style.top=0;curtain.style.bottom=0;
  curtain.style.backgroundColor=Color.black;root.Add(curtain);
  KarineMotion.Run(curtain,S.IntroSeconds,t=>curtain.style.opacity=1-t,()=>curtain.RemoveFromHierarchy());
  if(logo!=null)KarineMotion.Run(logo,S.IntroSeconds,t=>logo.style.scale=new Scale(Vector3.one*Mathf.Lerp(S.LogoFrom,1,t)),
   ()=>logo.style.scale=StyleKeyword.Null);
  if(column==null)return;
  int i=0;
  foreach(var child in column.Children()) {
   var row=child;int order=i++;
   if(KarineMotion.Reduced)continue;
   row.style.opacity=0;
   row.schedule.Execute(()=>KarineMotion.Run(row,S.RowSeconds,t=> {
    row.style.opacity=t;row.style.translate=new Translate(-S.RowSlide*(1-t),0);
   },()=>{row.style.opacity=StyleKeyword.Null;row.style.translate=StyleKeyword.Null;})).StartingIn((long)(S.IntroSeconds*500)+order*S.RowStaggerMs);
  }
 }

 // Satırın üstüne gelince hafifçe sağa kayar, sol kenarda ince amber çizgi yanar.
 static void MenuHover(Button row,bool primary) {
  var bar=new VisualElement {name="MenuHoverBar",pickingMode=PickingMode.Ignore};
  bar.style.position=Position.Absolute;bar.style.left=0;bar.style.top=0;bar.style.bottom=0;bar.style.width=S.HoverBar;
  bar.style.backgroundColor=primary?KarineTheme.OnPrimary:KarineTheme.Accent;bar.style.opacity=0;row.Add(bar);
  float level=0,goal=0;IVisualElementScheduledItem task=null;
  Action apply=()=>{bar.style.opacity=level;row.style.translate=new Translate(S.HoverShift*level,0);};
  Action<float> aim=target=> {
   goal=target;
   if(KarineMotion.Reduced){level=goal;bar.style.opacity=level;return;}
   if(task==null)task=row.schedule.Execute(()=> {
    level=Mathf.MoveTowards(level,goal,KarineTheme.Motion.TickMs/1000f/S.HoverSeconds);apply();
    if(Mathf.Approximately(level,goal))task.Pause();
   }).Every(KarineTheme.Motion.TickMs);
   else task.Resume();
  };
  row.RegisterCallback<PointerEnterEvent>(_=>aim(1));
  row.RegisterCallback<PointerLeaveEvent>(_=>aim(0));
  row.RegisterCallback<FocusInEvent>(_=>aim(1));
  row.RegisterCallback<FocusOutEvent>(_=>aim(0));
 }

 // Pencerede şimşek (ışığa duyarlılık sınırıyla), lambanın altında süzülen
 // toz zerrecikleri ve kenarları koyulaştıran çerçeve. Konumlar `Stagecraft`ta.
 public static void MenuStorm(VisualElement root) {
  var frame=new Image {name="MenuVignette",image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=new Color(0,0,0,S.VignetteAlpha)};
  frame.style.position=Position.Absolute;frame.style.left=0;frame.style.right=0;frame.style.top=0;frame.style.bottom=0;
  root.Insert(Mathf.Min(root.childCount,FirstAfter(root,"MenuRain")),frame);
  if(!Fx.On)return;
  int at=FirstAfter(root,"MenuRain");
  Lightning(root,at,new Rect(S.WindowLeft,S.WindowTop,S.WindowWidth,S.WindowHeight));
  Dust(root,at,new Rect(S.LampX-S.LampSize/2,S.LampY,S.LampSize,S.LampSize*1.4f));
 }

 // Pencerede iki kısa çakma (ilki güçlü, ikincisi yankı), ardından uzaktan gök gürültüsü.
 public static void Lightning(VisualElement root,int at,Rect box) {
  var flash=new VisualElement {name="StageLightning",pickingMode=PickingMode.Ignore};
  Percent(flash,box);flash.style.backgroundColor=new Color(.85f,.9f,1f,1);flash.style.opacity=0;
  root.Insert(Mathf.Min(at,root.childCount),flash);
  var random=new System.Random();
  Action plan=null;
  plan=()=>flash.schedule.Execute(()=> {
   if(flash.panel==null)return;
   if(Fx.On && Fx.MayFlash()) {
    KarineMotion.Run(flash,.45f,t=>flash.style.opacity=S.MenuFlashAlpha*Fx.Amount*(t<.25f?1-t*2:t<.45f?.6f*(1-(t-.25f)*5):0),
     ()=>flash.style.opacity=0);
    float pan=(float)random.NextDouble()*.8f-.4f;
    flash.schedule.Execute(()=>SoundAt?.Invoke("amb_thunder",pan,KarineTheme.Scene.ThunderGain)).StartingIn(600+random.Next(900));
   }
   plan();
  }).StartingIn(random.Next(S.MenuThunderMinMs,S.MenuThunderMaxMs));
  plan();
 }

 // Işık konisinde yavaşça süzülen sıcak toz; ortada parlak, kenarda söner.
 public static void Dust(VisualElement root,int at,Rect box) {
  var cone=new VisualElement {name="StageDust",pickingMode=PickingMode.Ignore};
  Percent(cone,box);root.Insert(Mathf.Min(at,root.childCount),cone);
  var seed=new System.Random(23);var motes=new List<(VisualElement e,float x,float y,float speed,float sway,float phase)>();
  for(int i=0;i<Fx.Count(S.Motes);i++) {
   var mote=new VisualElement {pickingMode=PickingMode.Ignore};
   float size=1.5f+(float)seed.NextDouble()*1.5f;
   mote.style.position=Position.Absolute;mote.style.width=size;mote.style.height=size;Round(mote,3);
   mote.style.backgroundColor=KarineTheme.Accent;cone.Add(mote);
   motes.Add((mote,(float)seed.NextDouble()*100,(float)seed.NextDouble()*100,.6f+(float)seed.NextDouble()*1.2f,2+(float)seed.NextDouble()*4,(float)seed.NextDouble()*6));
  }
  float start=Time.realtimeSinceStartup;
  cone.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   foreach(var m in motes) {
    float y=Mathf.Repeat(m.y-time*m.speed,100);
    float x=m.x+Mathf.Sin(time*.4f+m.phase)*m.sway;
    m.e.style.left=Length.Percent(x);m.e.style.top=Length.Percent(y);
    float edge=1-Mathf.Abs(x-50)/55f;
    m.e.style.opacity=S.MoteAlpha*Fx.Amount*Mathf.Clamp01(edge)*(.6f+.4f*Mathf.Sin(time*1.3f+m.phase));
   }
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Paralaks: fare/parmak konumu ya da telefonun eğimi arka planı birkaç piksel kaydırır.
 // `MenuDrift` bu değeri kendi kaymasına ekler.
 public static Vector2 Lean;
 public static void MenuParallax(VisualElement root) {
  Lean=Vector2.zero;
  if(!Fx.On || KarineMotion.Reduced)return;
  Vector2 aim=Vector2.zero;
  root.RegisterCallback<PointerMoveEvent>(e=> {
   var size=root.contentRect.size;if(size.x<=0)return;
   aim=new Vector2(.5f-e.localPosition.x/size.x,.5f-e.localPosition.y/size.y)*2*S.Parallax;
  });
  root.schedule.Execute(()=> {
   var tilt=Input.acceleration;
   if(tilt.sqrMagnitude>.01f)aim=new Vector2(-Mathf.Clamp(tilt.x,-.5f,.5f),Mathf.Clamp(tilt.y+.5f,-.5f,.5f))*2*S.Parallax;
   Lean=Vector2.Lerp(Lean,aim,S.ParallaxEase);
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Pencerenin dışı: sokak lambaları nefes alır, arada bir araba farları sokaktan
 // geçer, camdan iri damlalar yavaşça süzülür.
 public static void MenuStreet(VisualElement root) {
  if(!Fx.On)return;
  int at=FirstAfter(root,"MenuRain");
  float start=Time.realtimeSinceStartup;
  var lamps=new List<Image>();
  foreach(var p in S.StreetLamps) {
   var glow=new Image {name="StreetLamp",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
   Percent(glow,new Rect(p.x-2.5f,p.y-4,5,8));root.Insert(at,glow);lamps.Add(glow);
  }
  root.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   for(int i=0;i<lamps.Count;i++) {
    float buzz=Mathf.PerlinNoise(time*(3+i),i*1.7f)>.88f?.4f:1f;
    lamps[i].tintColor=KarineTheme.Alpha(KarineTheme.Accent,(.22f+.06f*Mathf.Sin(time*1.1f+i))*buzz*Fx.Amount);
   }
  }).Every(KarineTheme.Motion.TickMs*2);
  // Araba: iki sıcak far ya da iki kırmızı stop lambası soldan sağa veya sağdan sola.
  var car=new VisualElement {name="StreetCar",pickingMode=PickingMode.Ignore};
  Percent(car,new Rect(S.StreetLeft,S.StreetY,6,3));car.style.flexDirection=FlexDirection.Row;car.style.justifyContent=Justify.SpaceBetween;
  car.style.opacity=0;root.Insert(at,car);
  var lights=new List<Image>();
  for(int i=0;i<2;i++) {
   var light=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
   light.style.width=Length.Percent(40);light.style.height=Length.Percent(100);car.Add(light);lights.Add(light);
  }
  var random=new System.Random();
  System.Action drive=null;
  drive=()=>car.schedule.Execute(()=> {
   if(car.panel==null)return;
   if(Fx.On) {
    bool right=random.Next(2)==0,tail=random.Next(2)==0;
    var tint=tail?new Color(1f,.15f,.1f,.8f):new Color(1f,.9f,.7f,.9f);
    foreach(var l in lights)l.tintColor=tint*Fx.Amount;
    SoundAt?.Invoke("amb_car",right?-.3f:.3f,.25f);
    KarineMotion.Run(car,S.CarSeconds,t=> {
     float x=right?Mathf.Lerp(S.StreetLeft,S.StreetRight,t):Mathf.Lerp(S.StreetRight,S.StreetLeft,t);
     car.style.left=Length.Percent(x);car.style.opacity=Mathf.Sin(t*Mathf.PI);
    },()=>car.style.opacity=0);
   }
   drive();
  }).StartingIn(random.Next(S.CarMinMs,S.CarMaxMs));
  drive();
  // Camdaki iri damlalar: bir süre durur, sonra hızlanarak aşağı kayar.
  var glass=root.Q("MenuRain");if(glass==null)return;
  var seed=new System.Random(41);var drops=new List<(VisualElement e,float x,float phase,float speed)>();
  for(int i=0;i<Fx.Count(S.Drops);i++) {
   var drop=new VisualElement {pickingMode=PickingMode.Ignore};
   drop.style.position=Position.Absolute;drop.style.width=3;drop.style.height=5;Round(drop,2);
   drop.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.22f);glass.Add(drop);
   drops.Add((drop,(float)seed.NextDouble()*96,(float)seed.NextDouble(),.05f+(float)seed.NextDouble()*.05f));
  }
  glass.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   foreach(var d in drops) {
    float life=Mathf.Repeat(d.phase+time*d.speed,1f);
    float y=life<.4f?life*10:4+Mathf.Pow((life-.4f)/.6f,2)*100;
    d.e.style.left=Length.Percent(d.x);d.e.style.top=Length.Percent(y);
    d.e.style.height=life<.4f?5:9;
   }
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 static void Percent(VisualElement e,Rect box) {
  e.style.position=Position.Absolute;
  e.style.left=Length.Percent(box.x);e.style.top=Length.Percent(box.y);
  e.style.width=Length.Percent(box.width);e.style.height=Length.Percent(box.height);
 }

 static int FirstAfter(VisualElement root,string name) {
  for(int i=0;i<root.childCount;i++)if(root[i].name==name)return i+1;
  return root.childCount;
 }
}
}
