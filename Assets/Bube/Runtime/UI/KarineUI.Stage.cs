using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Anlatının sahnelenmesi: vaka açılış kartı, mekânın durağan kareleri,
// görüşmenin bitişi, masada çok yavaş dönen ışık; görüşme odasında kameranın
// nefesi, kişinin kendi bekleme döngüsü ve kayıt cihazının süre sayacı.
//
// Hepsi zamana ya da oyuncunun kendi geçişine bağlıdır; hiçbiri konuşmanın
// içeriğine, doğru ya da yanlış kayda tepki vermez.
public static partial class KarineUI {
 // Vaka açılışı: kâğıt masaya düşer, numara ve başlık daktiloyla yazılır.
 public static void CaseOpening(VisualElement root,string number,string title,Action done) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"CaseOpeningVeil",.9f);
  var sheet=new VisualElement {pickingMode=PickingMode.Ignore};
  sheet.style.width=420;sheet.style.paddingLeft=28;sheet.style.paddingRight=28;sheet.style.paddingTop=26;sheet.style.paddingBottom=30;
  sheet.style.backgroundColor=KarineTheme.Paper.Sheet;Border(sheet,1,KarineTheme.Paper.Edge);veil.Add(sheet);
  var numberLabel=Technical(sheet,string.Empty,KarineTheme.Office.TitleSize);numberLabel.style.color=KarineTheme.Paper.Stamp;numberLabel.style.letterSpacing=4;
  var titleLabel=Body_(sheet,string.Empty,KarineTheme.Office.TitleSize);titleLabel.style.color=KarineTheme.Paper.Ink;titleLabel.style.whiteSpace=WhiteSpace.Normal;
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.35f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  bool landed=false;int typedNumber=0,typedTitle=0;
  KarineMotion.Run(veil,S.OpeningSeconds,t=> {
   float fall=Mathf.Clamp01(t/.18f);
   sheet.style.translate=new Translate(0,Length.Percent((1-fall*fall)*-60));
   sheet.style.rotate=new Rotate(Angle.Degrees((1-fall)*-8+2));
   if(fall>=1 && !landed){landed=true;Cue("folder");}
   int wantNumber=Mathf.FloorToInt(Mathf.Clamp01((t-.22f)/.18f)*number.Length);
   if(wantNumber>typedNumber){typedNumber=wantNumber;numberLabel.text=number.Substring(0,typedNumber);Cue("key");}
   int wantTitle=Mathf.FloorToInt(Mathf.Clamp01((t-.42f)/.35f)*title.Length);
   if(wantTitle>typedTitle){typedTitle=wantTitle;titleLabel.text=title.Substring(0,typedTitle);if(typedTitle%2==0)Cue("key");}
  },()=>veil.schedule.Execute(finish).StartingIn(900));
 }

 // Mekânın durağan kareleri: her kare yavaşça kayar ve büyür, sonraki ona karışır.
 // Kare yoksa hiçbir şey olmaz. Dokunmak geçer.
 public static void LocationReel(VisualElement root,Texture2D[] frames,Action done) {
  if(root==null || frames==null || frames.Length==0 || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"LocationReel",1f);veil.style.overflow=Overflow.Hidden;
  var a=new Image {scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  var b=new Image {scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  foreach(var image in new[]{a,b}){image.style.position=Position.Absolute;image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;veil.Add(image);}
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.4f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  float each=S.ReelSeconds;
  KarineMotion.Run(veil,each*frames.Length,t=> {
   float at=t*frames.Length;int index=Mathf.Min(frames.Length-1,Mathf.FloorToInt(at));float local=at-index;
   a.image=frames[index];
   a.style.scale=new Scale(Vector3.one*(1.04f+local*.08f));
   a.style.translate=new Translate(Length.Percent((index%2==0?-1:1)*local*2.5f),0);
   bool blend=local>.82f && index<frames.Length-1;
   b.image=blend?frames[index+1]:null;b.style.opacity=blend?(local-.82f)/.18f:0;b.style.scale=new Scale(Vector3.one*1.04f);
  },finish);
 }

 // Görüşme biter: sandalye geri itilir; oda ışığı sönmüş gelir, ofis karanlıktan açılır.
 public static void LeaveRoom(VisualElement root) {
  Sound?.Invoke("ui_chair");
  if(root==null || !Fx.On)return;
  var veil=Veil(root,"LeaveRoomVeil",.92f);veil.pickingMode=PickingMode.Ignore;
  KarineMotion.Run(veil,S.EndSeconds,t=>veil.style.backgroundColor=new Color(0,0,0,.92f*(1-Mathf.SmoothStep(0,1,t))),()=>veil.RemoveFromHierarchy());
 }

 // Masada geçen süreyle ışık çok yavaş değişir: ilk dakikalarda fark edilmez,
 // yarım saatte belirgin bir akşam tonuna varır. Yalnız atmosfer.
 static float deskSince=-1;
 public static void DayDrift(VisualElement stage) {
  var back=stage?.Q("OfficeBack");
  if(back==null || !Fx.On)return;
  if(deskSince<0)deskSince=Time.realtimeSinceStartup;
  var tone=new VisualElement {name="OfficeDrift",pickingMode=PickingMode.Ignore};
  tone.style.position=Position.Absolute;tone.style.left=0;tone.style.right=0;tone.style.top=0;tone.style.bottom=0;back.Add(tone);
  Action paint=()=> {
   float minutes=(Time.realtimeSinceStartup-deskSince)/60f;
   float k=Mathf.Clamp01(minutes/S.DriftMinutes);
   tone.style.backgroundColor=KarineTheme.Alpha(S.DriftTone,S.DriftAlpha*k*Fx.Amount);
  };
  paint();tone.schedule.Execute(paint).Every(5000);
 }
 // Masada geçen dakika: kahve buharı buna göre azalır.
 public static float DeskMinutes => deskSince<0?0:(Time.realtimeSinceStartup-deskSince)/60f;

 // Kameranın nefesi: arka plan çok hafif kayar, el kamerası gibi.
 public static void CameraBreath(VisualElement background) {
  if(background==null || !Fx.On)return;
  float start=Time.realtimeSinceStartup;
  background.style.scale=new Scale(Vector3.one*1.03f);
  background.schedule.Execute(()=> {
   float t=Time.realtimeSinceStartup-start;
   background.style.translate=new Translate(Mathf.Sin(t*.37f)*S.BreathPx*Fx.Amount,Mathf.Sin(t*.23f+1.1f)*S.BreathPx*.6f*Fx.Amount);
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Kişinin kendi bekleme döngüsü: kimliğinden türeyen sabit hız ve genlik.
 // Yanıta, soruya ya da kayda göre değişmez.
 public static void Idle(VisualElement portrait,string personId) {
  if(portrait==null || !Fx.On || string.IsNullOrEmpty(personId))return;
  int seed=0;foreach(char c in personId)seed=seed*31+c;
  var random=new System.Random(seed);
  float period=3.2f+(float)random.NextDouble()*2.6f,sway=.4f+(float)random.NextDouble()*.8f,lean=(float)random.NextDouble()*.6f;
  float start=Time.realtimeSinceStartup;
  portrait.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(100));
  portrait.schedule.Execute(()=> {
   float t=(Time.realtimeSinceStartup-start)*Mathf.PI*2/period;
   portrait.style.rotate=new Rotate(Angle.Degrees(Mathf.Sin(t)*sway*Fx.Amount));
   portrait.style.translate=new Translate(Mathf.Sin(t*.5f)*lean*Fx.Amount,0);
  }).Every(KarineTheme.Motion.TickMs*2);
 }
}
}
