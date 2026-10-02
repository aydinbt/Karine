using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Dokunuşun hissi (Y): parmağın altında soluk halka, düğmenin basılınca inmesi,
// basılı tutunca dolan çember, kartın büyüyüp sayfaya dönüşmesi, beklerken
// karışan evrak. Hepsi her dokunuşta aynıdır; neye dokunulduğunu yorumlamaz.
public static partial class KarineUI {
 // Kök öğeye bir kez bağlanır: her dokunuşta halka, her düğmede basılma derinliği.
 public static void TouchFeel(VisualElement root) {
  if(root==null || root.ClassListContains("karine-feel"))return;
  root.AddToClassList("karine-feel");
  root.RegisterCallback<PointerDownEvent>(e=> {
   Press(e.target as VisualElement,true);
   if(!Fx.On)return;
   var local=root.WorldToLocal((Vector2)e.position);
   var ring=new VisualElement {name="TouchRing",pickingMode=PickingMode.Ignore};
   ring.style.position=Position.Absolute;Round(ring,40);Border(ring,2,KarineTheme.Alpha(KarineTheme.Primary,.35f*Fx.Amount));
   root.Add(ring);
   KarineMotion.Run(ring,S.RingSeconds,t=> {
    float size=Mathf.Lerp(8,S.RingPx,1-(1-t)*(1-t));
    ring.style.left=local.x-size*.5f;ring.style.top=local.y-size*.5f;ring.style.width=size;ring.style.height=size;
    ring.style.opacity=1-t;
   },()=>ring.RemoveFromHierarchy());
  },TrickleDown.TrickleDown);
  root.RegisterCallback<PointerUpEvent>(e=>Press(e.target as VisualElement,false),TrickleDown.TrickleDown);
  root.RegisterCallback<PointerCancelEvent>(e=>Press(e.target as VisualElement,false),TrickleDown.TrickleDown);
  root.RegisterCallback<PointerMoveEvent>(e=>{if(pressed!=null && ((Vector2)e.deltaPosition).sqrMagnitude>4)Press(null,false);},TrickleDown.TrickleDown);
 }
 static VisualElement pressed;
 // Son basılan düğmenin ekrandaki yeri: kart → sayfa geçişi buradan büyür.
 public static Rect LastPressed { get; private set; }
 static void Press(VisualElement target,bool down) {
  if(!down){if(pressed!=null){pressed.style.translate=StyleKeyword.Null;pressed=null;}return;}
  for(var e=target;e!=null;e=e.parent)if(e is Button b && b.enabledInHierarchy){pressed=b;LastPressed=b.worldBound;b.style.translate=new Translate(0,S.PressPx);return;}
 }

 // Basılı tutma: çember dolar; bırakılırsa iptal, dolarsa `action` çalışır.
 public static void Hold(VisualElement element,float seconds,Action action) {
  if(element==null)return;
  var ring=new VisualElement {name="HoldRing",pickingMode=PickingMode.Ignore};
  ring.style.position=Position.Absolute;ring.style.left=0;ring.style.right=0;ring.style.bottom=0;ring.style.height=3;
  ring.style.backgroundColor=KarineTheme.Primary;ring.style.width=Length.Percent(0);element.Add(ring);
  IVisualElementScheduledItem task=null;float start=0;
  element.RegisterCallback<PointerDownEvent>(_=> {
   start=Time.realtimeSinceStartup;task?.Pause();
   task=element.schedule.Execute(()=> {
    float k=Mathf.Clamp01((Time.realtimeSinceStartup-start)/seconds);
    ring.style.width=Length.Percent(k*100);
    if(k>=1){task.Pause();ring.style.width=Length.Percent(0);Fx.Buzz(Haptic.Press);action?.Invoke();}
   }).Every(KarineTheme.Motion.TickMs);
  });
  EventCallback<EventBase> cancel=_=>{task?.Pause();ring.style.width=Length.Percent(0);};
  element.RegisterCallback<PointerUpEvent>(e=>cancel(e));element.RegisterCallback<PointerLeaveEvent>(e=>cancel(e));
 }

 // Ortak öğe geçişi: dokunulan kartın yerinden bir kâğıt büyüyüp ekranı kaplar, sonra çözülür.
 public static void MorphFrom(VisualElement root,Rect world) {
  if(root==null || !Fx.On || world.width<1)return;
  var from=root.WorldToLocal(world);var size=root.layout.size;
  var sheet=new VisualElement {name="MorphSheet",pickingMode=PickingMode.Ignore};
  sheet.style.position=Position.Absolute;sheet.style.backgroundColor=KarineTheme.Paper.Sheet;root.Add(sheet);
  KarineMotion.Run(sheet,S.MorphSeconds,t=> {
   float e=1-(1-t)*(1-t)*(1-t);
   sheet.style.left=Mathf.Lerp(from.x,0,e);sheet.style.top=Mathf.Lerp(from.y,0,e);
   sheet.style.width=Mathf.Lerp(from.width,size.x,e);sheet.style.height=Mathf.Lerp(from.height,size.y,e);
   sheet.style.opacity=t<.6f?1:1-(t-.6f)/.4f;
  },()=>sheet.RemoveFromHierarchy());
 }

 // Beklerken: dönen simge yerine üç kâğıt üst üste karışır. `done` gelince kalkar.
 public static VisualElement Shuffle(VisualElement root) {
  if(root==null)return null;
  var veil=Veil(root,"PaperShuffle",.55f);veil.pickingMode=PickingMode.Ignore;
  var sheets=new VisualElement[3];
  for(int i=0;i<3;i++) {
   var s=new VisualElement {pickingMode=PickingMode.Ignore};
   s.style.position=Position.Absolute;s.style.width=90;s.style.height=120;
   s.style.backgroundColor=i==2?KarineTheme.Paper.Sheet:KarineTheme.Paper.Light;Border(s,1,KarineTheme.Paper.Edge);
   veil.Add(s);sheets[i]=s;
  }
  float start=Time.realtimeSinceStartup;int lastSwap=-1;
  veil.schedule.Execute(()=> {
   float t=(Time.realtimeSinceStartup-start)/S.ShuffleSeconds;int swap=Mathf.FloorToInt(t);float k=t-swap;
   if(swap!=lastSwap){lastSwap=swap;Cue("paper");}
   for(int i=0;i<3;i++) {
    bool moving=i==2;float lift=moving?Mathf.Sin(k*Mathf.PI):0;
    sheets[i].style.translate=new Translate((i-1)*6+lift*70,-i*3-lift*10);
    sheets[i].style.rotate=new Rotate(Angle.Degrees((i-1)*4+lift*12));
   }
  }).Every(KarineTheme.Motion.TickMs);
  return veil;
 }
}
}
