using System;
using UnityEngine;
using UnityEngine.UIElements;
using F = Bube.KarineTheme.Film;

namespace Bube {
// Üçüncü kademe: bütün ekranın üstündeki film dokusu ve CCTV'nin cihaz izleri.
//
// Hepsi tek bir **dokuyla** yapılır: açılışta bir kez üretilen gürültü resmi
// her karede başka yerinden gösterilir. Kamera sonrası işleme (shader) UI
// Toolkit panelinin üstüne düşmüyor; panelin içindeki bir doku katmanı hem her
// ekranı kapsıyor hem de mobilde neredeyse bedava.
//
// Kural aynı: efekt hissettirir, yönlendirmez. Parazit, kar ve kayma hiçbir
// kaydı öne çıkarmaz; sinyal satırlarının hepsinde aynıdır.
public static partial class KarineUI {
 static Texture2D grain;

 // Tekrarlanan, yumuşak gri gürültü. Saydamlık pikselden piksele değişir.
 public static Texture2D Grain() {
  if(grain!=null)return grain;
  const int size=128;
  grain=new Texture2D(size,size,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Repeat,filterMode=FilterMode.Point,hideFlags=HideFlags.DontSave};
  var random=new System.Random(7);var pixels=new Color32[size*size];
  for(int i=0;i<pixels.Length;i++) {
   byte v=(byte)random.Next(0,256);
   pixels[i]=new Color32(v,v,v,(byte)random.Next(0,256));
  }
  grain.SetPixels32(pixels);grain.Apply(false,true);return grain;
 }

 static Image Cover(VisualElement parent,string name,Texture2D texture,Color tint) {
  var image=new Image {name=name,image=texture,scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=tint};
  image.style.position=Position.Absolute;image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  parent.Add(image);return image;
 }

 // Ekranın üstünde duran film katmanı: hafif tonlama ve oynayan gren.
 // `BubeApp` her karede bunu kökün en üstünde tutar; ekranlar onu bilmez.
 public static VisualElement FilmLayer() {
  var layer=new VisualElement {name="FilmLayer",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  var tone=new VisualElement {name="FilmTone",pickingMode=PickingMode.Ignore};
  tone.style.position=Position.Absolute;tone.style.left=0;tone.style.right=0;tone.style.top=0;tone.style.bottom=0;
  layer.Add(tone);
  var noise=Cover(layer,"FilmGrain",Grain(),Color.white);
  var random=new System.Random();
  layer.schedule.Execute(()=> {
   float amount=Fx.Amount;
   layer.style.display=amount<=0?DisplayStyle.None:DisplayStyle.Flex;
   if(amount<=0)return;
   tone.style.backgroundColor=KarineTheme.Alpha(F.Tone,F.ToneAlpha*amount);
   noise.tintColor=new Color(1,1,1,F.GrainAlpha*amount);
   // Gren ekran başına birkaç kez döşenir ve her adımda başka yerden okunur.
   float tiles=Mathf.Max(1,layer.contentRect.width/F.GrainPixel/128f);
   noise.uv=new Rect((float)random.NextDouble(),(float)random.NextDouble(),tiles,tiles*Mathf.Max(.1f,layer.contentRect.height/Mathf.Max(1,layer.contentRect.width)));
  }).Every(F.GrainMs);
  return layer;
 }

 // CCTV ekranı açılır: önce ortada ince beyaz bir çizgi, sonra görüntü
 // yukarıdan ve aşağıdan açılır. Tüp ısınmasının kısa hâli.
 public static void CrtOn(VisualElement frame) {
  if(frame==null || !Fx.On)return;
  var tube=CrtShell(frame,out var line);
  Run(tube,F.CrtSeconds,t=> {
   float open=Mathf.Clamp01((t-.25f)/.75f);
   line.style.height=Length.Percent(Mathf.Lerp(.6f,100,open*open));
   line.style.top=Length.Percent(50-Mathf.Lerp(.3f,50,open*open));
   line.style.opacity=1-open*.85f;
   line.style.width=Length.Percent(Mathf.Lerp(8,100,Mathf.Clamp01(t/.3f)));
   line.style.left=Length.Percent(50-Mathf.Lerp(4,50,Mathf.Clamp01(t/.3f)));
   tube.style.backgroundColor=new Color(0,0,0,1-open);
  },()=>tube.RemoveFromHierarchy());
 }
 // Kapanış: görüntü yatay bir çizgiye, çizgi bir noktaya söner.
 public static void CrtOff(VisualElement over) {
  if(over==null || over.panel==null || !Fx.On)return;
  var tube=CrtShell(over,out var line);
  Run(tube,F.CrtSeconds*.8f,t=> {
   float squash=Mathf.Clamp01(t/.55f),shrink=Mathf.Clamp01((t-.55f)/.45f);
   line.style.top=Length.Percent(50*squash);line.style.height=Length.Percent(Mathf.Max(.6f,100*(1-squash)));
   line.style.left=Length.Percent(50*shrink);line.style.width=Length.Percent(Mathf.Max(.8f,100*(1-shrink)));
   line.style.opacity=1-shrink;tube.style.backgroundColor=new Color(0,0,0,Mathf.Min(.9f,squash));
  },()=>tube.RemoveFromHierarchy());
 }
 static VisualElement CrtShell(VisualElement frame,out VisualElement line) {
  var tube=new VisualElement {name="Crt",pickingMode=PickingMode.Ignore};
  tube.style.position=Position.Absolute;tube.style.left=0;tube.style.right=0;tube.style.top=0;tube.style.bottom=0;
  tube.style.backgroundColor=Color.black;tube.style.overflow=Overflow.Hidden;frame.Add(tube);
  line=new VisualElement {pickingMode=PickingMode.Ignore};line.style.position=Position.Absolute;
  line.style.backgroundColor=F.Phosphor;tube.Add(line);
  return tube;
 }

 static void Run(VisualElement owner,float seconds,Action<float> update,Action done) => KarineMotion.Run(owner,seconds,update,done);

 // Kar: sinyal kesildiği an görüntü bir an karlanır. Yalnız sinyal satırında
 // ve her sinyal satırında aynı biçimde; satır zaten kırmızıyla yazılıyor,
 // kar yeni bilgi vermez, yalnız o anı duyurur.
 public static void Snow(VisualElement frame,float seconds) {
  if(frame==null || !Fx.MayFlash())return;
  var snow=Cover(frame,"Snow",Grain(),new Color(1,1,1,F.SnowAlpha));
  snow.style.backgroundColor=new Color(.04f,.06f,.07f,.55f);
  var random=new System.Random();
  Run(snow,seconds,t=> {
   snow.uv=new Rect((float)random.NextDouble(),(float)random.NextDouble(),3,2);
   snow.style.opacity=t<.7f?1:(1-t)/.3f;
  },()=>snow.RemoveFromHierarchy());
 }

 // Bant izi: görüntünün renk kanalları bir iki piksel ayrılır, altta bozuk
 // "takip" şeridi oynar, ara sıra bütün resim yatay sarsılır. Video ve kare
 // dizisi aynı katmanı paylaşır; `source` görüntünün kendisidir.
 public static void VhsTrace(VisualElement frame,Image source) {
  if(frame==null || source==null || !Fx.On)return;
  var red=Ghost(frame,source,"VhsRed",F.ChromaRed);var cyan=Ghost(frame,source,"VhsCyan",F.ChromaCyan);
  var tracking=Cover(frame,"VhsTracking",Grain(),new Color(1,1,1,F.TrackingAlpha*Fx.Amount));
  tracking.style.top=StyleKeyword.Auto;tracking.style.height=Length.Percent(F.TrackingHeight);
  var random=new System.Random();
  frame.schedule.Execute(()=> {
   if(source.panel==null)return;
   red.image=cyan.image=source.image;
   red.style.translate=new Translate(F.ChromaShift,0);cyan.style.translate=new Translate(-F.ChromaShift,0);
   tracking.uv=new Rect((float)random.NextDouble(),(float)random.NextDouble(),4,.3f);
   tracking.style.translate=new Translate(((float)random.NextDouble()-.5f)*F.TrackingJitter,0);
   // Seyrek sarsıntı: çakma sınırından izin alır.
   if(random.NextDouble()<F.JerkChance*Fx.Amount && Fx.MayFlash()) {
    float dx=((float)random.NextDouble()-.5f)*F.JerkPixels;
    source.style.translate=new Translate(dx,0);
    source.schedule.Execute(()=>source.style.translate=new Translate(0,0)).StartingIn(F.JerkMs);
   }
  }).Every(F.GrainMs);
 }
 static Image Ghost(VisualElement frame,Image source,string name,Color tint) {
  var ghost=new Image {name=name,image=source.image,scaleMode=source.scaleMode,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(tint,tint.a*Fx.Amount)};
  ghost.style.position=Position.Absolute;ghost.style.left=0;ghost.style.right=0;ghost.style.top=0;ghost.style.bottom=0;
  frame.Insert(frame.IndexOf(source)+1,ghost);return ghost;
 }

 // Zaman damgası atlaması: sinyal satırında saat bir an karışık rakamlarla
 // döner, sonra kayıttaki saate oturur. Son değer her zaman metindeki değerdir.
 public static void TimecodeSkip(Label stamp,string final,Action done=null) {
  if(stamp==null || !Fx.On){if(stamp!=null)stamp.text=final;done?.Invoke();return;}
  var random=new System.Random();
  Run(stamp,F.TimecodeSeconds,t=> {
   if(t>=1){stamp.text=final;return;}
   var chars=final.ToCharArray();
   for(int i=0;i<chars.Length;i++)if(char.IsDigit(chars[i]))chars[i]=(char)('0'+random.Next(10));
   stamp.text=new string(chars);
  },()=>{stamp.text=final;done?.Invoke();});
 }

 // Kareyi dondur ve büyüt: iki parmakla ya da tekerlekle yaklaşılır, sürükleyerek
 // gezinilir, çift dokunuş yakınlaştırıp sıfırlar. Yaklaşınca görüntü **pikselleşir**
 // (yumuşatma kapanır), yani kamera çözünürlüğünden fazlası çıkmaz — plaka ve yüz
 // okunmaz kalır. Tavan `ZoomMax`. Yakınlaşmak oynatmayı durdurur (`freeze`).
 public static void Zoomable(VisualElement frame,Image image,Action freeze) {
  if(frame==null || image==null)return;
  float zoom=1;Vector2 pan=Vector2.zero;
  var touches=new System.Collections.Generic.Dictionary<int,Vector2>();float pinchFrom=0,zoomFrom=1;Vector2 last=Vector2.zero;
  image.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(50));
  Action apply=()=> {
   zoom=Mathf.Clamp(zoom,1,F.ZoomMax);
   var size=frame.contentRect.size;float limitX=size.x*(zoom-1)*.5f,limitY=size.y*(zoom-1)*.5f;
   pan=new Vector2(Mathf.Clamp(pan.x,-limitX,limitX),Mathf.Clamp(pan.y,-limitY,limitY));
   image.style.scale=new Scale(new Vector3(zoom,zoom,1));image.style.translate=new Translate(pan.x,pan.y);
   if(image.image is Texture texture)texture.filterMode=zoom>1.05f?FilterMode.Point:FilterMode.Bilinear;
  };
  Func<EventBase,bool> mine=evt=>evt.target==frame || evt.target is Image;
  frame.RegisterCallback<PointerDownEvent>(evt=> {
   if(!mine(evt))return;
   touches[evt.pointerId]=evt.position;last=evt.position;
   if(touches.Count==2){var p=new System.Collections.Generic.List<Vector2>(touches.Values);pinchFrom=Vector2.Distance(p[0],p[1]);zoomFrom=zoom;}
  });
  frame.RegisterCallback<PointerMoveEvent>(evt=> {
   if(!touches.ContainsKey(evt.pointerId))return;
   touches[evt.pointerId]=evt.position;
   if(touches.Count==2 && pinchFrom>1) {
    var p=new System.Collections.Generic.List<Vector2>(touches.Values);
    zoom=zoomFrom*Vector2.Distance(p[0],p[1])/pinchFrom;if(zoom>1.05f)freeze?.Invoke();
   } else if(zoom>1) {pan+=(Vector2)evt.position-last;last=evt.position;}
   apply();
  });
  EventCallback<PointerUpEvent> up=evt=>touches.Remove(evt.pointerId);
  frame.RegisterCallback(up);
  frame.RegisterCallback<PointerCancelEvent>(evt=>touches.Remove(evt.pointerId));
  frame.RegisterCallback<WheelEvent>(evt=>{zoom*=evt.delta.y<0?1.15f:1/1.15f;if(zoom>1.05f)freeze?.Invoke();apply();evt.StopPropagation();});
  frame.RegisterCallback<ClickEvent>(evt=> {
   if(evt.clickCount!=2 || !mine(evt))return;
   if(zoom>1.05f){zoom=1;pan=Vector2.zero;}
   else {
    zoom=F.ZoomDouble;freeze?.Invoke();
    var size=frame.contentRect.size;pan=(size*.5f-(Vector2)evt.localPosition)*(zoom-1);
   }
   apply();
  });
 }
}
}
