using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Görüşme odasında varlık (AA): göz kırpma, floresan titremesi, aynada kayan
// yansıma, sigara dumanı, masaya bırakılan kaydın sesi. Kırpma aralığı kişinin
// kimliğinden türer, titreme zamana bağlıdır; hiçbiri soruya ya da yanıta tepki vermez.
public static partial class KarineUI {
 // Göz kırpma: `Bube/Characters/<kişi>_blink` görseli varsa kısa süre onunla değişir.
 // Görsel yoksa hiçbir şey olmaz.
 public static void Blink(Image art,string personId) {
  if(art==null || !Fx.On || string.IsNullOrEmpty(personId))return;
  var closed=Resources.Load<Texture2D>("Bube/Characters/"+personId+"_blink");
  if(closed==null)return;
  int seed=0;foreach(char c in personId)seed=seed*31+c;
  var random=new System.Random(seed);
  IVisualElementScheduledItem task=null;Texture open=null;
  task=art.schedule.Execute(()=> {
   open=art.image;art.image=closed;
   art.schedule.Execute(()=>art.image=open).StartingIn(120);
   task.ExecuteLater(random.Next(S.BlinkMinMs,S.BlinkMaxMs));
  });
  task.ExecuteLater(random.Next(S.BlinkMinMs,S.BlinkMaxMs));
 }

 // Tavan floresanı arada bir titrer: iki üç kısa kararma, sonra sakin.
 public static void Fluorescent(VisualElement root) {
  var layer=root?.Q("InterviewRoomFx");
  if(layer==null || !Fx.MayFlash())return;
  var dim=new VisualElement {name="RoomFlicker",pickingMode=PickingMode.Ignore};
  dim.style.position=Position.Absolute;dim.style.left=0;dim.style.right=0;dim.style.top=0;dim.style.bottom=0;layer.Add(dim);
  IVisualElementScheduledItem task=null;
  task=dim.schedule.Execute(()=> {
   int beats=UnityEngine.Random.Range(2,4);
   KarineMotion.Run(dim,.5f,t=>dim.style.backgroundColor=new Color(0,0,0,Mathf.Sin(t*Mathf.PI*beats*2)>.3f?.18f*Fx.Amount:0),
    ()=>dim.style.backgroundColor=Color.clear);
   task.ExecuteLater(UnityEngine.Random.Range(S.FlickerMinMs,S.FlickerMaxMs));
  });
  task.ExecuteLater(UnityEngine.Random.Range(S.FlickerMinMs,S.FlickerMaxMs));
 }

 // Tek yönlü camda çok soluk, yavaş kayan bir parlama.
 public static void MirrorSheen(VisualElement root) {
  var mirror=root?.Q("RoomMirror");
  if(mirror==null || !Fx.On)return;
  mirror.style.overflow=Overflow.Hidden;
  var sheen=new VisualElement {pickingMode=PickingMode.Ignore};
  sheen.style.position=Position.Absolute;sheen.style.top=Length.Percent(-20);sheen.style.bottom=Length.Percent(-20);sheen.style.width=Length.Percent(18);
  sheen.style.rotate=new Rotate(Angle.Degrees(18));sheen.style.backgroundColor=KarineTheme.Alpha(Color.white,.06f*Fx.Amount);mirror.Add(sheen);
  float start=Time.realtimeSinceStartup;
  sheen.schedule.Execute(()=>sheen.style.left=Length.Percent(Mathf.Repeat((Time.realtimeSinceStartup-start)*4f,160)-40)).Every(KarineTheme.Motion.TickMs*2);
 }

 // Sigara dumanı: kişinin verisinde `smokes` işaretliyse görüşme boyunca hep aynı ince duman.
 public static void Smoke(VisualElement holder) {
  if(holder==null || !Fx.On)return;
  var layer=new VisualElement {name="PersonSmoke",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=Length.Percent(62);layer.style.width=Length.Percent(20);
  layer.style.top=Length.Percent(10);layer.style.height=Length.Percent(55);holder.Add(layer);
  int count=Fx.Count(6);var puffs=new VisualElement[count];
  for(int i=0;i<count;i++) {
   var p=new VisualElement {pickingMode=PickingMode.Ignore};p.style.position=Position.Absolute;
   p.style.width=Length.Percent(40);p.style.height=Length.Percent(10);Round(p,30);
   p.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.07f);layer.Add(p);puffs[i]=p;
  }
  float start=Time.realtimeSinceStartup;
  layer.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   for(int i=0;i<count;i++) {
    float k=Mathf.Repeat(time/S.SmokeSeconds+i/(float)Mathf.Max(1,count),1);
    puffs[i].style.top=Length.Percent(90-k*90);puffs[i].style.left=Length.Percent(30+Mathf.Sin(k*6+i)*20);
    puffs[i].style.scale=new Scale(Vector3.one*(.6f+k*1.4f));puffs[i].style.opacity=Mathf.Sin(k*Mathf.PI);
   }
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Kayıt masaya bırakılınca türüne göre ses: fotoğraf, dosya, delil torbası.
 // Ses kaydın türünü söyler, önemini değil.
 public static void DropSound(string kind,bool hasImage) {
  Cue(kind=="evidence"?"drop_bag":hasImage?"drop_photo":"drop_file");
 }
}
}
