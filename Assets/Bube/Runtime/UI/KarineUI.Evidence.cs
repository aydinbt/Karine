using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Kanıt masası dokunuşları: deftere işlenen iki kaynak arasındaki kırmızı ip,
// panoya batan raptiye, beliren fotoğraf, büyüteç, oyuncunun kendi kalemi ve
// parmakla esneyen kâğıt.
//
// Kural: ip her hükümde aynı gerilir, oyun bağlantının doğru olup olmadığını
// söylemez. Fotoğraf her fotoğrafta aynı belirir. Kalem izleri oyuncunundur;
// oyun onları okumaz, değerlendirmez.
public static partial class KarineUI {
 // Kırmızı ip: iki raptiye arasında soldan sağa gerilir, sonra `done`.
 public static void RedString(VisualElement host,Action done) {
  if(host==null || !Fx.On){done?.Invoke();return;}
  var layer=new VisualElement {name="RedString",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=Length.Percent(8);layer.style.right=Length.Percent(8);
  layer.style.top=Length.Percent(48);layer.style.height=14;host.Add(layer);
  VisualElement Pin(float x) {
   var pin=new VisualElement {pickingMode=PickingMode.Ignore};
   pin.style.position=Position.Absolute;pin.style.left=Length.Percent(x);pin.style.top=0;pin.style.width=12;pin.style.height=12;
   pin.style.marginLeft=-6;Round(pin,6);pin.style.backgroundColor=KarineTheme.Danger;layer.Add(pin);return pin;
  }
  Pin(0);var end=Pin(100);end.style.opacity=0;
  var thread=new VisualElement {pickingMode=PickingMode.Ignore};
  thread.style.position=Position.Absolute;thread.style.left=0;thread.style.top=5;thread.style.height=2;
  thread.style.backgroundColor=KarineTheme.Paper.Stamp;layer.Add(thread);
  KarineMotion.Run(layer,S.StringSeconds,t=> {
   thread.style.width=Length.Percent(100*t);
   // İp gerilirken hafifçe sarkar, sonda düzleşir.
   thread.style.translate=new Translate(0,Mathf.Sin(t*Mathf.PI)*3);
  },()=>{end.style.opacity=1;Cue("pin");layer.schedule.Execute(()=>{layer.RemoveFromHierarchy();done?.Invoke();}).StartingIn(180);});
 }

 // Raptiye panoya batar: satır bir kez sarsılır, sol üstünde raptiye kalır.
 public static void PinShake(VisualElement row) {
  if(row==null || !Fx.On)return;
  var pin=new VisualElement {name="TimelinePin",pickingMode=PickingMode.Ignore};
  pin.style.position=Position.Absolute;pin.style.left=-5;pin.style.top=-5;pin.style.width=10;pin.style.height=10;Round(pin,5);
  pin.style.backgroundColor=KarineTheme.Danger;row.Add(pin);
  KarineMotion.Run(row,.35f,t=>row.style.rotate=new Rotate(Angle.Degrees(Mathf.Sin(t*30)*S.PinShake*(1-t)*.3f)),
   ()=>row.style.rotate=StyleKeyword.Null);
 }

 // Fotoğraf belirir: soluk beyazdan renge, beyaz Polaroid çerçevesiyle.
 // Oturumda her fotoğraf bir kez belirir; sonra hep hazır gelir.
 static readonly HashSet<string> developed=new HashSet<string>();
 public static void Develop(Image photo,string id) {
  if(photo==null)return;
  photo.style.borderLeftWidth=photo.style.borderRightWidth=photo.style.borderTopWidth=8;photo.style.borderBottomWidth=24;
  photo.style.borderLeftColor=photo.style.borderRightColor=photo.style.borderTopColor=photo.style.borderBottomColor=KarineTheme.Paper.Light;
  if(!developed.Add(id) || !Fx.On)return;
  Cue("polaroid");
  KarineMotion.Run(photo,S.DevelopSeconds,t=> {
   float e=t*t;
   photo.tintColor=Color.Lerp(new Color(1.6f,1.5f,1.3f,.25f),Color.white,e);
  },()=>photo.tintColor=Color.white);
 }

 // Büyüteç ve kalem: fotoğraf bir çerçeveye alınır; iki parmakla ya da çift
 // dokunuşla büyütülür, kalem açıkken parmak üstüne çizer. Çizimler oturum
 // boyunca belgeye bağlı kalır.
 static readonly Dictionary<string,List<List<Vector2>>> inks=new Dictionary<string,List<List<Vector2>>>();
 public static VisualElement Inspectable(VisualElement parent,Image photo,string id,string drawLabel,string clearLabel) {
  var frame=new VisualElement {name="Inspectable"};
  frame.style.overflow=Overflow.Hidden;frame.style.height=photo.style.height;frame.style.marginTop=photo.style.marginTop;
  photo.style.marginTop=0;photo.style.height=Length.Percent(100);
  parent.Add(frame);frame.Add(photo);
  Zoomable(frame,photo,null);
  if(!inks.TryGetValue(id,out var strokes))inks[id]=strokes=LoadInk(id);
  var ink=new InkLayer(strokes,()=>SaveInk(id,strokes));frame.Add(ink);
  var tools=new VisualElement();tools.style.flexDirection=FlexDirection.Row;tools.style.justifyContent=Justify.FlexEnd;tools.style.marginTop=4;parent.Add(tools);
  Button draw=null;
  draw=PaperButton(tools,drawLabel,()=>{ink.Drawing=!ink.Drawing;draw.style.backgroundColor=ink.Drawing?KarineTheme.Paper.Stamp:KarineTheme.Paper.Sheet;
   draw.style.color=ink.Drawing?KarineTheme.Paper.Sheet:KarineTheme.Paper.Ink;},KarinePaperKind.Choice);
  draw.style.marginRight=6;draw.style.minHeight=40;draw.style.fontSize=Typography.Snap(13);
  var clear=PaperButton(tools,clearLabel,()=>{strokes.Clear();SaveInk(id,strokes);ink.MarkDirtyRepaint();},KarinePaperKind.Quiet);
  clear.style.minHeight=40;clear.style.fontSize=Typography.Snap(13);
  return frame;
 }

 // Çizimler kayıtta kalır: oyuncunun kendi notudur, oyun okumaz.
 static string InkKey(string id)=>"karine.ink."+id;
 static List<List<Vector2>> LoadInk(string id) {
  var strokes=new List<List<Vector2>>();
  foreach(var part in PlayerPrefs.GetString(InkKey(id),string.Empty).Split('|')) {
   if(part.Length==0)continue;var stroke=new List<Vector2>();
   foreach(var point in part.Split(';')){var xy=point.Split(',');if(xy.Length==2 && float.TryParse(xy[0],System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out var x) && float.TryParse(xy[1],System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out var y))stroke.Add(new Vector2(x,y));}
   if(stroke.Count>1)strokes.Add(stroke);
  }
  return strokes;
 }
 static void SaveInk(string id,List<List<Vector2>> strokes) {
  var c=System.Globalization.CultureInfo.InvariantCulture;
  PlayerPrefs.SetString(InkKey(id),string.Join("|",strokes.ConvertAll(s=>string.Join(";",s.ConvertAll(p=>p.x.ToString("0.###",c)+","+p.y.ToString("0.###",c))))));
  PlayerPrefs.Save();
 }

 sealed class InkLayer : VisualElement {
  readonly List<List<Vector2>> strokes;readonly Action changed;List<Vector2> current;bool drawing;
  public bool Drawing { get=>drawing; set{drawing=value;pickingMode=value?PickingMode.Position:PickingMode.Ignore;} }
  public InkLayer(List<List<Vector2>> strokes,Action changed) {
   this.strokes=strokes;this.changed=changed;name="InkLayer";pickingMode=PickingMode.Ignore;
   style.position=Position.Absolute;style.left=0;style.right=0;style.top=0;style.bottom=0;
   generateVisualContent+=Paint;
   RegisterCallback<PointerDownEvent>(e=>{if(!drawing)return;current=new List<Vector2>{Norm(e.localPosition)};strokes.Add(current);
    this.CapturePointer(e.pointerId);Cue("pen");e.StopPropagation();});
   RegisterCallback<PointerMoveEvent>(e=>{if(current==null)return;current.Add(Norm(e.localPosition));MarkDirtyRepaint();e.StopPropagation();});
   RegisterCallback<PointerUpEvent>(e=>{if(current!=null)changed?.Invoke();current=null;this.ReleasePointer(e.pointerId);});
  }
  // Koordinatlar yüzde olarak saklanır: ekran boyu değişse de çizim yerinde kalır.
  Vector2 Norm(Vector3 p) => new Vector2(p.x/Mathf.Max(1,contentRect.width),p.y/Mathf.Max(1,contentRect.height));
  void Paint(MeshGenerationContext context) {
   var painter=context.painter2D;var size=contentRect.size;
   painter.strokeColor=KarineTheme.Alpha(KarineTheme.Danger,.85f);painter.lineWidth=S.InkWidth;
   painter.lineJoin=LineJoin.Round;painter.lineCap=LineCap.Round;
   foreach(var stroke in strokes) {
    if(stroke.Count<2)continue;
    painter.BeginPath();painter.MoveTo(Vector2.Scale(stroke[0],size));
    for(int i=1;i<stroke.Count;i++)painter.LineTo(Vector2.Scale(stroke[i],size));
    painter.Stroke();
   }
  }
 }

 // Kâğıt parmağı izler: yatay sürüklemede hafifçe döner, bırakınca yaylanıp
 // yerine oturur. Dikey kaydırmaya karışmaz.
 public static void Flex(VisualElement paper) {
  if(paper==null)return;
  float startX=0;bool down=false;
  paper.RegisterCallback<PointerDownEvent>(e=>{startX=e.position.x;down=Fx.On;},TrickleDown.TrickleDown);
  paper.RegisterCallback<PointerMoveEvent>(e=> {
   if(!down)return;
   float dx=e.position.x-startX;
   paper.style.rotate=new Rotate(Angle.Degrees(Mathf.Clamp(dx*.012f,-S.FlexMax,S.FlexMax)));
   paper.style.translate=new Translate(Mathf.Clamp(dx*.3f,-60,60),Mathf.Abs(dx)*-.02f);
  },TrickleDown.TrickleDown);
  Action release=()=> {
   if(!down)return;down=false;
   float from=paper.resolvedStyle.rotate.angle.value,fromX=paper.resolvedStyle.translate.x;
   if(Mathf.Abs(fromX)>8)Cue("paper");
   KarineMotion.Run(paper,.5f,t=>{float k=Mathf.Cos(t*Mathf.PI*2.5f)*(1-t);paper.style.rotate=new Rotate(Angle.Degrees(from*k));paper.style.translate=new Translate(fromX*k,0);},
    ()=>{paper.style.rotate=StyleKeyword.Null;paper.style.translate=StyleKeyword.Null;});
  };
  paper.RegisterCallback<PointerUpEvent>(_=>release(),TrickleDown.TrickleDown);
  paper.RegisterCallback<PointerLeaveEvent>(_=>release());
 }
}
}
