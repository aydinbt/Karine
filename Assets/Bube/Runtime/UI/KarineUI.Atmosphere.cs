using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using M = Bube.KarineTheme.Office.Atmosphere;

namespace Bube {
// Masanın havası: lamba ışığı, köşe kararması, ışıkta süzülen toz, telefon
// eğildikçe kayan katmanlar ve dokunulan eşyanın masadan kalkması.
//
// Kural: bu katman yalnız **hissettirir**, yönlendirmez. Hiçbir efekt bir
// kaynağa dikkat çekmez; kalkma yalnız oyuncunun dokunduğu eşyada olur.
// Renk, punto ve çerçeve dili değişmez (Kit §24) — eklenen yalnız ışık ve hareket.
public static partial class KarineUI {

 // Masaya ait, telefonla birlikte kayan nesneler. Başlık ve bildirimler HUD'dur,
 // yerinde kalır.
 static readonly HashSet<string> DeskFront=new HashSet<string> {
  "DeskLamp","InboxTray","DeskPhone","BlankCaseFolder","CctvMonitor","EvidencePile",
  "DeskInbox","DeskFile","DeskInterviews","DeskTerminal","DeskEvidence","OfficeMonitorTitle",
 };
 // Masa düğmesi → altındaki eşya. Basılan düğmenin eşyası masadan kalkar.
 static readonly Dictionary<string,string> PropOf=new Dictionary<string,string> {
  {"DeskInbox","InboxTray"},{"DeskFile","BlankCaseFolder"},{"DeskInterviews","DeskPhone"},
  {"DeskTerminal","CctvMonitor"},{"DeskEvidence","EvidencePile"},
 };

 static Texture2D glow, vignette;

 // Merkezde dolu, kenarda sönen yuvarlak leke. Işık havuzu da gölge de bundan.
 static Texture2D Glow() {
  if(glow!=null)return glow;
  const int size=128;
  glow=new Texture2D(size,size,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Clamp,hideFlags=HideFlags.DontSave};
  var pixels=new Color32[size*size];
  for(int y=0;y<size;y++)for(int x=0;x<size;x++) {
   float dx=(x+.5f)/size*2-1,dy=(y+.5f)/size*2-1;
   float a=Mathf.Clamp01(1-Mathf.Sqrt(dx*dx+dy*dy));a=a*a*(3-2*a);
   pixels[y*size+x]=new Color32(255,255,255,(byte)(a*255));
  }
  glow.SetPixels32(pixels);glow.Apply(false,true);return glow;
 }
 // Ortası açık, köşeleri koyulaşan çerçeve.
 static Texture2D Vignette() {
  if(vignette!=null)return vignette;
  const int size=128;
  vignette=new Texture2D(size,size,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Clamp,hideFlags=HideFlags.DontSave};
  var pixels=new Color32[size*size];
  for(int y=0;y<size;y++)for(int x=0;x<size;x++) {
   float dx=(x+.5f)/size*2-1,dy=(y+.5f)/size*2-1;
   float a=Mathf.Clamp01((Mathf.Sqrt(dx*dx*.8f+dy*dy)-.55f)/.75f);
   pixels[y*size+x]=new Color32(255,255,255,(byte)(a*a*255));
  }
  vignette.SetPixels32(pixels);vignette.Apply(false,true);return vignette;
 }

 static Image Soft(VisualElement parent,string name,Texture2D texture,Rect box,Color tint) {
  var image=new Image {name=name,image=texture,scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=tint};
  OfficePlace(image,box);parent.Add(image);return image;
 }

 // Masa kurulduktan sonra çağrılır: eşyaları ön katmana alır, ışığı ve tozu ekler.
 public static void OfficeAtmosphere(VisualElement stage) {
  var back=new VisualElement {name="OfficeBack",pickingMode=PickingMode.Ignore};
  var front=new VisualElement {name="OfficeFront",pickingMode=PickingMode.Ignore};
  OfficePlace(back,new Rect(0,0,100,100));OfficePlace(front,new Rect(0,0,100,100));

  // Katmanlar: oda + pencere arkada, masadaki eşyalar önde. Sıra korunur.
  var room=stage.Q("OfficeRoom");
  VisualElement window=null;foreach(var child in stage.Children())if(child.name!=null&&child.name.StartsWith("OfficeWindow"))window=child;
  int at=room==null?0:stage.IndexOf(room);
  stage.Insert(at,back);stage.Insert(at+1,front);
  if(room!=null)back.Add(room);if(window!=null)back.Add(window);
  // Kenar boşluğu görünmesin diye arka plaka biraz büyük tutulur.
  back.style.scale=new Scale(Vector3.one*M.BackScale);

  var moving=new List<VisualElement>();
  foreach(var child in stage.Children())if(child.name!=null&&DeskFront.Contains(child.name))moving.Add(child);
  // Lamba ışığı eşyaların altında, masanın üstünde durur.
  var light=Soft(front,"OfficeLight",Glow(),M.LightPool,KarineTheme.Alpha(KarineTheme.Paper.Light,M.LightAlpha));
  foreach(var element in moving) {
   if(element is Image && PropOf.ContainsValue(element.name)) {
    var shadow=Soft(front,element.name+"Shadow",Glow(),Rect.zero,KarineTheme.Alpha(KarineTheme.Paper.FolderDeep,1));
    shadow.style.left=element.style.left;shadow.style.top=element.style.top;
    shadow.style.width=element.style.width;shadow.style.height=element.style.height;
    shadow.style.opacity=0;
   }
   front.Add(element);
  }
  // Kararma eşyaların üstünde, masa düğmelerinin altında: düğmeler okunur kalır.
  var shade=new Image {name="OfficeVignette",image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=KarineTheme.Alpha(KarineTheme.Background,M.VignetteAlpha)};
  OfficePlace(shade,new Rect(0,0,100,100));shade.style.scale=new Scale(Vector3.one*M.BackScale*1.04f);
  int firstButton=-1;for(int i=0;i<front.childCount;i++)if(front[i] is Button){firstButton=i;break;}
  if(firstButton<0)front.Add(shade);else front.Insert(firstButton,shade);

  foreach(var pair in PropOf) {
   var button=front.Q<Button>(pair.Key);var prop=front.Q(pair.Value);var shadow=front.Q(pair.Value+"Shadow");
   if(button!=null&&prop!=null)Liftable(button,prop,shadow);
  }
  if(KarineMotion.Reduced)return;

  // Toz: lamba ışığının içinde yavaşça yükselen, nefes alır gibi parlayıp sönen zerreler.
  var dust=new VisualElement {name="OfficeDust",pickingMode=PickingMode.Ignore};
  OfficePlace(dust,M.DustArea);front.Add(dust);
  var random=new System.Random(41);
  var motes=new List<(VisualElement e,float x,float y,float speed,float phase,float size)>();
  for(int i=0;i<M.DustCount;i++) {
   float size=M.DustMin+(float)random.NextDouble()*(M.DustMax-M.DustMin);
   var mote=new VisualElement {pickingMode=PickingMode.Ignore};
   mote.style.position=Position.Absolute;mote.style.width=size;mote.style.height=size;Round(mote,Mathf.CeilToInt(size));
   mote.style.backgroundColor=KarineTheme.Paper.Light;dust.Add(mote);
   motes.Add((mote,(float)random.NextDouble()*100,(float)random.NextDouble()*100,
    M.DustRise*(.5f+(float)random.NextDouble()),(float)random.NextDouble()*Mathf.PI*2,size));
  }

  // Eğme: cihazda ivmeölçer, Editör'de fare. Taban yavaşça yeni duruşa yaklaşır,
  // böylece telefon nasıl tutulursa tutulsun sahne kendi merkezine döner.
  Vector2 pointer=Vector2.zero,current=Vector2.zero,baseline=Input.acceleration;
  bool tilt=SystemInfo.supportsAccelerometer;
  stage.RegisterCallback<PointerMoveEvent>(evt=> {
   var r=stage.contentRect;if(r.width<=0)return;
   pointer=new Vector2(evt.localPosition.x/r.width*2-1,evt.localPosition.y/r.height*2-1);
  },TrickleDown.TrickleDown);
  float start=Time.realtimeSinceStartup;
  stage.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   Vector2 target;
   if(tilt) {
    Vector2 acc=Input.acceleration;baseline=Vector2.Lerp(baseline,acc,M.TiltRecenter);
    target=new Vector2(Mathf.Clamp((acc.x-baseline.x)*M.TiltGain,-1,1),Mathf.Clamp(-(acc.y-baseline.y)*M.TiltGain,-1,1));
   } else target=pointer;
   current=Vector2.Lerp(current,target,M.Follow);
   float reach=stage.contentRect.width*M.Reach;
   back.style.translate=new Translate(-current.x*reach*M.BackDepth,-current.y*reach*M.BackDepth*.6f);
   front.style.translate=new Translate(-current.x*reach,-current.y*reach*.6f);
   light.style.opacity=1+M.LightBreath*Mathf.Sin(time*M.LightBreathSpeed);
   foreach(var m in motes) {
    float y=Mathf.Repeat(m.y-time*m.speed,100f);
    float x=m.x+Mathf.Sin(time*.4f+m.phase)*3f;
    m.e.style.left=Length.Percent(x);m.e.style.top=Length.Percent(y);
    // Alanın tepesinde ve dibinde söner, ışığın içinde belirir.
    float edge=Mathf.Clamp01(Mathf.Min(y,100-y)/18f);
    m.e.style.opacity=edge*M.DustAlpha*(.55f+.45f*Mathf.Sin(time*.9f+m.phase*2));
   }
  }).Every(KarineTheme.Motion.TickMs);
 }

 // Dokunulan eşya masadan bir parmak kalkar, gölgesi altına yayılır; bırakınca yerine oturur.
 static void Liftable(Button button,VisualElement prop,VisualElement shadow) {
  float lifted=0;
  Action<float> to=target=> {
   if(KarineMotion.Reduced)return;
   float from=lifted;
   KarineMotion.Run(prop,target>from?M.LiftSeconds:M.SettleSeconds,t=> {
    lifted=Mathf.Lerp(from,target,t);
    prop.style.translate=new Translate(0,-M.LiftOffset*lifted);
    prop.style.scale=new Scale(Vector3.one*(1+M.LiftScale*lifted));
    if(shadow!=null) {
     shadow.style.opacity=M.ShadowAlpha*lifted;
     shadow.style.translate=new Translate(0,M.LiftOffset*.5f*lifted);
     shadow.style.scale=new Scale(Vector3.one*(1+M.LiftScale*2*lifted));
    }
   });
  };
  button.RegisterCallback<PointerDownEvent>(_=>to(1),TrickleDown.TrickleDown);
  button.RegisterCallback<PointerUpEvent>(_=>to(0));
  button.RegisterCallback<PointerLeaveEvent>(_=>{if(lifted>0)to(0);});
  button.RegisterCallback<PointerCancelEvent>(_=>to(0));
 }
}
}
