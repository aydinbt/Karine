using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;
using O = Bube.KarineTheme.Office;

namespace Bube {
// Masanın yaşayan eşyaları (Z): lamba düğmesi, telefon kablosu, uzak şehir
// pencereleri, takvim yaprağı ve yeri hatırlanan eşyalar. Yalnız atmosfer;
// hiçbiri bir kaynağa, kayda ya da sıradaki adıma işaret etmez.
public static partial class KarineUI {
 const string LampKey="karine.lampOff";

 // Lambaya dokununca masa kararır ya da aydınlanır; tercih saklanır.
 public static void LampSwitch(VisualElement stage) {
  var back=stage?.Q("OfficeBack");var front=stage?.Q("OfficeFront") ?? stage;
  if(back==null)return;
  var shade=new VisualElement {name="LampShade",pickingMode=PickingMode.Ignore};
  shade.style.position=Position.Absolute;shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;back.Add(shade);
  Action paint=()=>shade.style.backgroundColor=new Color(0,0,0,PlayerPrefs.GetInt(LampKey,0)==1?S.LampOffAlpha:0);
  paint();
  var hit=new VisualElement {name="LampSwitch"};OfficePlace(hit,S.LampSwitch);hit.pickingMode=PickingMode.Position;front.Add(hit);
  hit.RegisterCallback<ClickEvent>(_=> {
   PlayerPrefs.SetInt(LampKey,PlayerPrefs.GetInt(LampKey,0)==1?0:1);
   Sound?.Invoke("ui_lamp");Fx.Buzz(Haptic.Tick);paint();
  });
 }

 // Telefonun kıvrımlı kablosu: ahize kalkınca sallanır, durulur.
 static VisualElement cord;
 public static void PhoneCord(VisualElement stage) {
  var front=stage?.Q("OfficeFront");
  if(front==null || !Fx.On)return;
  cord=new VisualElement {name="PhoneCord",pickingMode=PickingMode.Ignore};OfficePlace(cord,S.Cord);
  cord.style.transformOrigin=new TransformOrigin(Length.Percent(100),0);front.Add(cord);
  for(int i=0;i<7;i++) {
   var loop=new VisualElement {pickingMode=PickingMode.Ignore};
   loop.style.position=Position.Absolute;loop.style.left=Length.Percent(80-i*12);loop.style.top=Length.Percent(i*13);
   loop.style.width=Length.Percent(22);loop.style.height=Length.Percent(14);Round(loop,6);Border(loop,1,KarineTheme.Alpha(Color.black,.45f));
   cord.Add(loop);
  }
 }
 public static void CordSwing() {
  if(cord==null || cord.panel==null || !Fx.On)return;
  var c=cord;
  KarineMotion.Run(c,S.CordSeconds,t=>c.style.rotate=new Rotate(Angle.Degrees(Mathf.Sin(t*18)*(1-t)*9)),()=>c.style.rotate=StyleKeyword.Null);
 }

 // Pencerede uzak şehir: birkaç küçük pencere rastgele yanar, söner.
 public static void CityWindows(VisualElement stage) {
  var back=stage?.Q("OfficeBack");
  if(back==null || !Fx.On)return;
  var city=new VisualElement {name="CityWindows",pickingMode=PickingMode.Ignore};OfficePlace(city,O.Window);back.Add(city);
  var random=new System.Random(41);int count=Fx.Count(14);
  var lights=new VisualElement[count];
  for(int i=0;i<count;i++) {
   var w=new VisualElement {pickingMode=PickingMode.Ignore};
   w.style.position=Position.Absolute;w.style.left=Length.Percent(4+(float)random.NextDouble()*90);w.style.top=Length.Percent(45+(float)random.NextDouble()*45);
   w.style.width=3;w.style.height=4;w.style.backgroundColor=KarineTheme.Alpha(O.Weather.BeamColor,.35f*Fx.Amount);
   w.style.opacity=random.NextDouble()<.5?1:0;city.Add(w);lights[i]=w;
  }
  if(count==0)return;
  IVisualElementScheduledItem task=null;
  task=city.schedule.Execute(()=> {
   var w=lights[UnityEngine.Random.Range(0,count)];w.style.opacity=w.resolvedStyle.opacity>.5f?0:1;
   task.ExecuteLater(UnityEngine.Random.Range(S.WindowMinMs,S.WindowMaxMs));
  });
  task.ExecuteLater(S.WindowMinMs);
 }

 // Takvim: her yeni vakada üstteki yaprak koparılır. `number` vaka numarası.
 public static void Calendar(VisualElement stage,string number,bool tear) {
  var front=stage?.Q("OfficeFront");
  if(front==null)return;
  var pad=new VisualElement {name="DeskCalendar",pickingMode=PickingMode.Ignore};OfficePlace(pad,S.Calendar);
  pad.style.backgroundColor=KarineTheme.Paper.Light;Border(pad,1,KarineTheme.Paper.Edge);
  pad.style.alignItems=Align.Center;pad.style.justifyContent=Justify.Center;front.Add(pad);
  var head=new VisualElement {pickingMode=PickingMode.Ignore};head.style.position=Position.Absolute;head.style.left=0;head.style.right=0;head.style.top=0;
  head.style.height=Length.Percent(20);head.style.backgroundColor=KarineTheme.Paper.Stamp;pad.Add(head);
  var label=Technical(pad,number,O.SmallSize);label.style.color=KarineTheme.Paper.Ink;
  if(!tear || !Fx.On)return;
  var leaf=new VisualElement {pickingMode=PickingMode.Ignore};
  leaf.style.position=Position.Absolute;leaf.style.left=0;leaf.style.right=0;leaf.style.top=0;leaf.style.bottom=0;
  leaf.style.backgroundColor=KarineTheme.Paper.Light;Border(leaf,1,KarineTheme.Paper.Edge);
  leaf.style.transformOrigin=new TransformOrigin(0,0);pad.Add(leaf);
  pad.schedule.Execute(()=> {
   Cue("paper");
   KarineMotion.Run(leaf,S.TearSeconds,t=> {
    leaf.style.rotate=new Rotate(Angle.Degrees(t*-35));leaf.style.translate=new Translate(Length.Percent(-t*40),Length.Percent(t*t*260));
    leaf.style.opacity=1-t;
   },()=>leaf.RemoveFromHierarchy());
  }).StartingIn(1200);
 }

 // Yeri hatırlanan eşya: sürüklenir, bırakıldığı yer saklanır, dönüşte orada durur.
 public static void Keepable(VisualElement element,string key) {
  if(element==null)return;
  string prefs="karine.deskPos."+key;
  var saved=PlayerPrefs.GetString(prefs,"");
  var parts=saved.Split(',');
  if(parts.Length==2 && float.TryParse(parts[0],System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out var x)
   && float.TryParse(parts[1],System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out var y)) {
   element.style.left=Length.Percent(x);element.style.top=Length.Percent(y);
  }
  element.pickingMode=PickingMode.Position;
  Vector2 grab=Vector2.zero;bool dragging=false;
  element.RegisterCallback<PointerDownEvent>(e=>{dragging=true;grab=e.position;element.CapturePointer(e.pointerId);});
  element.RegisterCallback<PointerMoveEvent>(e=> {
   if(!dragging || element.parent==null)return;
   var size=element.parent.layout.size;if(size.x<1 || size.y<1)return;
   Vector2 delta=(Vector2)e.position-grab;grab=e.position;
   element.style.left=Length.Percent(Mathf.Clamp(element.layout.x/size.x*100+delta.x/size.x*100,0,95));
   element.style.top=Length.Percent(Mathf.Clamp(element.layout.y/size.y*100+delta.y/size.y*100,0,95));
  });
  element.RegisterCallback<PointerUpEvent>(e=> {
   if(!dragging)return;dragging=false;element.ReleasePointer(e.pointerId);
   var size=element.parent.layout.size;if(size.x<1)return;var c=System.Globalization.CultureInfo.InvariantCulture;
   PlayerPrefs.SetString(prefs,(element.layout.x/size.x*100).ToString(c)+","+(element.layout.y/size.y*100).ToString(c));
  });
 }
}
}
