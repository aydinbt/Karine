using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Stagecraft;

namespace Bube {
// Kâğıdın, mührün ve sahnenin dokunuşları: belgedeki leke ve kat izi, faksın
// mürekkep sızması, makineden çıkan kâğıt, elle çizilen çizgi, mühür inişi,
// faksın üstüne düşen ışık, menünün yağmuru, logonun yanışı ve karşıdaki
// kişinin duruşu.
//
// Kural: kâğıt kusurları belgeye **sabittir** (kimliğinden türetilir), her
// açılışta aynı yerdedir; rastgele olsaydı oyuncu bir lekeyi ipucu sanardı.
// Kişinin duruşu ve göz kırpması herkeste aynı sıklıkta ve rastgele saatte olur.
public static partial class KarineUI {
 static Texture2D ring;

 // Fincan dibi izi: içi boş, kenarı koyu, bir yeri açık halka.
 static Texture2D Ring() {
  if(ring!=null)return ring;
  const int size=96;
  ring=new Texture2D(size,size,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Clamp,hideFlags=HideFlags.DontSave};
  var pixels=new Color32[size*size];
  for(int y=0;y<size;y++)for(int x=0;x<size;x++) {
   float dx=(x+.5f)/size*2-1,dy=(y+.5f)/size*2-1,r=Mathf.Sqrt(dx*dx+dy*dy);
   float edge=Mathf.Clamp01(1-Mathf.Abs(r-.8f)/.09f),inner=r<.8f?.12f:0;
   float gap=Mathf.Clamp01((Mathf.Atan2(dy,dx)+2.2f)*1.5f);
   pixels[y*size+x]=new Color32(255,255,255,(byte)(Mathf.Clamp01(edge*gap+inner)*255));
  }
  ring.SetPixels32(pixels);ring.Apply(false,true);return ring;
 }

 static uint Hash(string id) {
  uint h=2166136261;foreach(char c in id??string.Empty){h^=c;h*=16777619;}return h;
 }

 // Belgenin kusurları: bazı belgelerde fincan izi, çoğunda kat izi, hepsinde
 // sol üstte zımba delikleri. Hangisinin nerede olduğu belge kimliğinden gelir.
 public static void PaperWear(VisualElement paper,string id) {
  if(paper==null || !Fx.On)return;
  uint h=Hash(id);
  var wear=new VisualElement {name="PaperWear",pickingMode=PickingMode.Ignore};
  wear.style.position=Position.Absolute;wear.style.left=0;wear.style.right=0;wear.style.top=0;wear.style.bottom=0;
  wear.style.overflow=Overflow.Hidden;paper.Add(wear);
  for(int i=0;i<2;i++) {
   var hole=new VisualElement {pickingMode=PickingMode.Ignore};
   hole.style.position=Position.Absolute;hole.style.width=S.HoleSize;hole.style.height=S.HoleSize;Round(hole,S.HoleSize);
   hole.style.left=S.HoleInset;hole.style.top=Length.Percent(18+i*12);
   hole.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.FolderDeep,S.HoleAlpha);wear.Add(hole);
  }
  if(h%3!=0) {
   var crease=new VisualElement {pickingMode=PickingMode.Ignore};
   crease.style.position=Position.Absolute;crease.style.left=0;crease.style.right=0;crease.style.height=1;
   crease.style.top=Length.Percent(30+h%35);
   crease.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,S.CreaseAlpha*Fx.Amount);wear.Add(crease);
   var shine=new VisualElement {pickingMode=PickingMode.Ignore};
   shine.style.position=Position.Absolute;shine.style.left=0;shine.style.right=0;shine.style.height=1;
   shine.style.top=Length.Percent(30+h%35);shine.style.marginTop=1;
   shine.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Light,S.CreaseAlpha*Fx.Amount);wear.Add(shine);
  }
  if(h%4==1) {
   var stain=new Image {image=Ring(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
    tintColor=KarineTheme.Alpha(S.CoffeeColor,S.CoffeeAlpha*Fx.Amount)};
   stain.style.position=Position.Absolute;stain.style.width=S.CoffeeSize;stain.style.height=S.CoffeeSize;
   stain.style.right=Length.Percent(4+(h>>4)%18);stain.style.bottom=Length.Percent(4+(h>>9)%22);
   stain.style.rotate=new Rotate(Angle.Degrees((h>>13)%360));wear.Add(stain);
  }
 }

 // Faks bozulması: yazı hafif yayılır (mürekkep sızması), kâğıtta gri
 // benekler durur ve bir satır bir iki piksel kaymış basılır. Faksa sabittir.
 public static void FaxWear(VisualElement body,string id) {
  if(body==null || !Fx.On)return;
  uint h=Hash(id);int index=0,shifted=(int)(h%5)+1;
  foreach(var child in body.Children()) {
   if(!(child is Label label))continue;
   label.style.textShadow=new TextShadow{offset=new Vector2(.4f,.3f),blurRadius=S.BleedBlur,color=KarineTheme.Alpha(KarineTheme.Paper.Ink,S.BleedAlpha*Fx.Amount)};
   if(++index==shifted)label.style.translate=new Translate(S.LineShift,0);
  }
  var specks=new VisualElement {name="FaxSpecks",pickingMode=PickingMode.Ignore};
  specks.style.position=Position.Absolute;specks.style.left=0;specks.style.right=0;specks.style.top=0;specks.style.bottom=0;
  body.Add(specks);
  var random=new System.Random((int)h);
  for(int i=0;i<Fx.Count(S.Specks);i++) {
   var speck=new VisualElement {pickingMode=PickingMode.Ignore};float size=1+(float)random.NextDouble()*2.5f;
   speck.style.position=Position.Absolute;speck.style.width=size;speck.style.height=size;Round(speck,3);
   speck.style.left=Length.Percent((float)random.NextDouble()*100);speck.style.top=Length.Percent((float)random.NextDouble()*100);
   speck.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Faded,.25f+(float)random.NextDouble()*.3f);specks.Add(speck);
  }
 }

 // Faks makineden çıkar: kâğıt yukarıdan aşağı satır satır sürülür, alt kenarı
 // hafif kıvrık gölge taşır, yerine oturunca gölge kaybolur.
 public static void FeedOut(VisualElement paper) {
  if(paper==null)return;
  // Makine önce ısınır: kâğıt yuvasında titrer, röle tıklar, sonra sürülür.
  Cue("warm");
  if(!Fx.On){Sound?.Invoke("ui_paper");return;}
  paper.style.translate=new Translate(0,-S.FeedOffset);
  KarineMotion.Run(paper,KarineTheme.Scene.WarmSeconds,t=>paper.style.translate=new Translate(Mathf.Sin(t*90)*.6f,-S.FeedOffset),()=>{
   Sound?.Invoke("ui_paper");Feed(paper);});
 }
 static void Feed(VisualElement paper) {
  var curl=new VisualElement {name="PaperCurl",pickingMode=PickingMode.Ignore};
  curl.style.position=Position.Absolute;curl.style.left=0;curl.style.right=0;curl.style.bottom=0;curl.style.height=S.CurlHeight;
  curl.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.FolderDeep,.22f);
  curl.style.borderTopWidth=1;curl.style.borderTopColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.6f);paper.Add(curl);
  KarineMotion.Run(paper,S.FeedSeconds,t=> {
   // Basamaklı ilerleme: makine kâğıdı tık tık iter.
   float step=Mathf.Floor(t*S.FeedSteps)/S.FeedSteps;
   paper.style.translate=new Translate(0,-S.FeedOffset*(1-step));
   curl.style.opacity=1-t;
  },()=>{paper.style.translate=new Translate(0,0);curl.RemoveFromHierarchy();});
 }

 // Elle çizilen çizgi: kalem soldan sağa geçer. Bittiğinde `done` kalıcı
 // biçimi uygular, çizim katmanı kaldırılır.
 public static void InkStroke(VisualElement line,Action done) {
  if(line==null || !Fx.On){done?.Invoke();return;}
  Sound?.Invoke("ui_pen");
  var ink=new VisualElement {name="InkStroke",pickingMode=PickingMode.Ignore};
  ink.style.position=Position.Absolute;ink.style.left=0;ink.style.bottom=0;ink.style.height=2;
  ink.style.backgroundColor=KarineTheme.Paper.Stamp;line.Add(ink);
  var random=new System.Random();float wobble=(float)random.NextDouble()*.6f;
  KarineMotion.Run(ink,S.InkSeconds,t=> {
   ink.style.width=Length.Percent(100*t);
   ink.style.rotate=new Rotate(Angle.Degrees(Mathf.Sin(t*6+wobble)*.25f));
  },()=>{ink.RemoveFromHierarchy();done?.Invoke();});
 }

 // Mühür iner: gölgesi büyür, kâğıda vurur, ekran bir kez sarsılır, mürekkep
 // izi kalır. Her raporda aynı; sonucu söylemez, yalnız gönderildiğini.
 public static void StampDown(VisualElement root,string label,Action done) {
  Sound?.Invoke("ui_stamp");Fx.Buzz(Haptic.Thud);
  if(root==null || !Fx.On){done?.Invoke();return;}
  root.schedule.Execute(()=>ChromaShake(root)).StartingIn(260);
  var veil=new VisualElement {name="StampVeil"};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;root.Add(veil);
  var mark=new Label(label) {pickingMode=PickingMode.Ignore};
  mark.style.color=KarineTheme.Alpha(KarineTheme.Paper.Stamp,.92f);mark.style.fontSize=Typography.Snap(S.StampText);
  mark.style.unityFontStyleAndWeight=FontStyle.Bold;mark.style.letterSpacing=6;
  Border(mark,4,KarineTheme.Alpha(KarineTheme.Paper.Stamp,.92f));Round(mark,6);
  mark.style.paddingLeft=26;mark.style.paddingRight=26;mark.style.paddingTop=10;mark.style.paddingBottom=10;
  mark.style.rotate=new Rotate(Angle.Degrees(-8));veil.Add(mark);
  if(Fonts?.MonoBold!=null)mark.style.unityFontDefinition=FontDefinition.FromFont(Fonts.MonoBold);
  KarineMotion.Run(veil,S.StampSeconds,t=> {
   float drop=Mathf.Clamp01(t/.35f);
   mark.style.scale=new Scale(Vector3.one*Mathf.Lerp(2.4f,1,drop*drop));
   mark.style.opacity=drop;
   veil.style.backgroundColor=new Color(0,0,0,.35f*drop);
   // Vuruş anında tek bir sarsıntı.
   float shake=t>.35f && t<.5f?Mathf.Sin((t-.35f)*90)*S.Shake*(1-(t-.35f)/.15f):0;
   veil.style.translate=new Translate(shake,shake*.4f);
  },()=>{veil.RemoveFromHierarchy();done?.Invoke();});
 }

 // Faksın üstüne düşen ışık: oda kararır, yalnız kâğıt aydınlık kalır.
 // İlk okumada bir kez; dokunmaya engel olmaz.
 public static void Spotlight(VisualElement root,VisualElement target) {
  if(root==null || target==null || !Fx.On)return;
  var dark=new Image {name="FaxSpotlight",image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=new Color(0,0,0,S.SpotAlpha*Fx.Amount)};
  dark.style.position=Position.Absolute;root.Add(dark);
  Action place=()=> {
   if(target.panel==null || root.panel==null)return;
   var box=root.WorldToLocal(target.worldBound);
   float growX=box.width*.55f,growY=box.height*.55f;
   dark.style.left=box.x-growX;dark.style.top=box.y-growY;dark.style.width=box.width+growX*2;dark.style.height=box.height+growY*2;
  };
  target.RegisterCallback<GeometryChangedEvent>(_=>place());place();
  KarineMotion.Run(dark,S.SpotSeconds,t=>dark.style.opacity=t);
 }

 // Menünün arka planı yavaşça yaklaşır ve kayar; üstünde ince yağmur iner.
 // Durağan menü görselinin canlı katmanı: çok yavaş kamera nefesi, yalnız pencere
 // camının içinde yağan yağmur, masa lambasının hafif titrek sıcak halesi.
 // Konumlar görselin oranlarına göre yüzde olarak `KarineTheme.Stage`dadır.
 public static void MenuDrift(VisualElement film,VisualElement root) {
  if(film==null || !Fx.On)return;
  float start=Time.realtimeSinceStartup;
  film.style.transformOrigin=new TransformOrigin(Length.Percent(60),Length.Percent(40));
  film.schedule.Execute(()=> {
   float t=(Time.realtimeSinceStartup-start)/S.DriftSeconds;
   float wave=.5f-.5f*Mathf.Cos(t*Mathf.PI*2);
   film.style.scale=new Scale(Vector3.one*(1.01f+S.DriftZoom*wave));
   film.style.translate=new Translate(-S.DriftPan*wave,S.DriftPan*.3f*wave);
  }).Every(KarineTheme.Motion.TickMs*2);
  int at=root.IndexOf(film)+1;
  // Pencere: yağmur yalnız camın arkasında görünür, odanın içine yağmaz.
  var glass=new VisualElement {name="MenuRain",pickingMode=PickingMode.Ignore};
  glass.style.position=Position.Absolute;glass.style.overflow=Overflow.Hidden;
  glass.style.left=Length.Percent(S.WindowLeft);glass.style.top=Length.Percent(S.WindowTop);
  glass.style.width=Length.Percent(S.WindowWidth);glass.style.height=Length.Percent(S.WindowHeight);
  root.Insert(at++,glass);
  var random=new System.Random(11);var streaks=new List<(VisualElement e,float x,float speed,float phase)>();
  for(int i=0;i<Fx.Count(S.MenuRain);i++) {
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
  // Lamba: sıcak hale, nefes gibi yavaş ve arada küçük bir titreme.
  var halo=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  halo.style.position=Position.Absolute;
  halo.style.left=Length.Percent(S.LampX-S.LampSize/2);halo.style.top=Length.Percent(S.LampY-S.LampSize/2);
  halo.style.width=Length.Percent(S.LampSize);halo.style.height=Length.Percent(S.LampSize*1.6f);
  root.Insert(at,halo);
  halo.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   float breath=.5f+.5f*Mathf.Sin(time*.9f);
   float flicker=Mathf.PerlinNoise(time*7f,.3f)>.82f?.5f:1f;
   halo.tintColor=KarineTheme.Alpha(KarineTheme.Accent,(.10f+.06f*breath)*flicker*Fx.Amount);
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Logo neon gibi yanar: iki kısa çakma, sonra sabit ışık. Oturumda bir kez.
 // Çakmalar saniyede üçün altında kalır.
 static bool logoLit;
 public static void NeonIgnite(VisualElement logo) {
  if(logo==null || logoLit)return;
  logoLit=true;
  if(!Fx.On)return;
  KarineMotion.Run(logo,S.NeonSeconds,t=> {
   float on=t<.12f?0:t<.2f?.85f:t<.45f?.15f:t<.55f?1:t<.7f?.4f:1;
   logo.style.opacity=on;
  },()=>logo.style.opacity=StyleKeyword.Null);
 }

 // Karşıdaki kişi ağırlığını verir: 6-14 saniyede bir çok hafif yana kayar ve
 // geri döner. Herkeste aynı genlik, saat rastgele; yanıta, soruya bağlı değil.
 public static void Posture(VisualElement figure) {
  if(figure==null || !Fx.On)return;
  var random=new System.Random();
  Action plan=null;
  plan=()=>figure.schedule.Execute(()=> {
   if(figure.panel==null)return;
   float side=random.Next(2)==0?-1:1;
   KarineMotion.Run(figure,S.PostureSeconds,t=> {
    float lean=Mathf.Sin(t*Mathf.PI);
    figure.style.translate=new Translate(side*S.PostureShift*lean,0);
    figure.style.rotate=new Rotate(Angle.Degrees(side*S.PostureTilt*lean));
   });
   plan();
  }).StartingIn(random.Next(S.PostureMinMs,S.PostureMaxMs));
  plan();
 }

 // Göz kırpma: yalnız piksel portrelerde (göz hücreleri bilinir). 3-7 saniyede
 // bir, herkeste aynı. Çizilmiş portrelerde gözün yeri bilinmediği için yok.
 public static void Blink(IList<VisualElement> eyes,Color lid) {
  if(eyes==null || eyes.Count==0 || !Fx.On)return;
  var random=new System.Random();var open=new List<StyleColor>();foreach(var e in eyes)open.Add(e.style.backgroundColor);
  Action plan=null;
  plan=()=>eyes[0].schedule.Execute(()=> {
   if(eyes[0].panel==null)return;
   foreach(var e in eyes)e.style.backgroundColor=lid;
   eyes[0].schedule.Execute(()=>{for(int i=0;i<eyes.Count;i++)eyes[i].style.backgroundColor=open[i];}).StartingIn(S.BlinkMs);
   plan();
  }).StartingIn(random.Next(S.BlinkMinMs,S.BlinkMaxMs));
  plan();
 }

 // Sessizlik: yanıttan önceki duraksamada üç nokta belirir. Süre her yanıtta
 // ve her kişide aynıdır; içerikten bilgi taşımaz.
 public static void Ellipsis(Label speech,int ms) {
  if(speech==null)return;
  int dots=0;
  var task=speech.schedule.Execute(()=>{dots=dots%3+1;speech.text=new string('.',dots);}).Every(Mathf.Max(60,ms/4));
  speech.schedule.Execute(()=>task.Pause()).StartingIn(ms-10);
 }

 // Kâğıt sürüklenirken masadan kalkar: biraz büyür, eğilir, altında koyu bir
 // gölge kenarı belirir. Bırakılınca hepsi geri iner.
 public static void Lift(VisualElement paper,float amount) {
  if(paper==null)return;
  amount=Mathf.Clamp01(amount);
  paper.style.scale=new Scale(Vector3.one*(1+S.LiftScale*amount));
  paper.style.borderBottomWidth=Mathf.Round(S.LiftShadow*amount);
  paper.style.borderBottomColor=KarineTheme.Alpha(Color.black,.45f*amount);
 }
}
}
