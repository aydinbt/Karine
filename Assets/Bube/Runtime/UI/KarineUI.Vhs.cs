using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Kamera kaydı ve terminal (AB): zaman damgasının titremesi, yazıcı çıktısı,
// fosfor kalıntısı, bant takılması. Her kayıtta aynı kurallar; hiçbiri bir
// kaydı ya da satırı öne çıkarmaz ve metni değiştirmez.
public static partial class KarineUI {
 // Zaman damgası arada 1 px kayar, eski bant gibi.
 public static void StampJitter(VisualElement stamp) {
  if(stamp==null || !Fx.On)return;
  stamp.schedule.Execute(()=> {
   bool jump=UnityEngine.Random.value<.08f*Fx.Amount;
   stamp.style.translate=jump?new Translate(UnityEngine.Random.Range(-1,2),UnityEngine.Random.Range(-1,2)):new Translate(0,0);
  }).Every(KarineTheme.Motion.TickMs*3);
 }

 // Bir sonraki kare yerine aynı kare: tüm kayıtlarda aynı küçük olasılık.
 public static bool FrameRepeats() => Fx.On && UnityEngine.Random.value<S.RepeatChance*Fx.Amount;

 // Yazıcı: kâğıt üstten çıkar, satırlar basılır, kâğıt masaya düşer. Dokunmak geçer.
 public static void Printout(VisualElement root,string[] lines,Action done) {
  if(root==null || !Fx.On || lines==null || lines.Length==0){done?.Invoke();return;}
  Sound?.Invoke("ui_printer");
  var veil=Veil(root,"Printout",.4f);veil.style.justifyContent=Justify.FlexStart;
  var paper=new VisualElement {pickingMode=PickingMode.Ignore};
  paper.style.width=300;paper.style.paddingLeft=16;paper.style.paddingRight=16;paper.style.paddingTop=12;paper.style.paddingBottom=14;
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,1,KarineTheme.Paper.Edge);veil.Add(paper);
  var labels=new Label[lines.Length];
  for(int i=0;i<lines.Length;i++){labels[i]=Technical(paper,lines[i],KarineTheme.Office.SmallSize);labels[i].style.color=KarineTheme.Paper.Ink;labels[i].style.opacity=0;labels[i].style.whiteSpace=WhiteSpace.Normal;}
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.3f,t=>{veil.style.opacity=1-t;paper.style.translate=new Translate(0,Length.Percent(t*80));},()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(veil,S.PrintSeconds,t=> {
   paper.style.translate=new Translate(0,Length.Percent((t-1)*100));
   for(int i=0;i<labels.Length;i++)labels[i].style.opacity=t>(i+1f)/(labels.Length+1)?1:0;
  },()=>veil.schedule.Execute(finish).StartingIn(700));
 }

 // Fosfor kalıntısı: yazı değişince eskisi bir an soluk kalır.
 public static void Persist(Label label) {
  if(label==null || !Fx.On)return;
  string last=label.text;
  label.schedule.Execute(()=> {
   if(label.text==last || label.parent==null)return;
   var ghost=new Label(last) {pickingMode=PickingMode.Ignore};
   ghost.style.position=Position.Absolute;ghost.style.left=label.layout.x;ghost.style.top=label.layout.y;ghost.style.width=label.layout.width;
   ghost.style.fontSize=label.resolvedStyle.fontSize;ghost.style.unityFont=label.resolvedStyle.unityFont;ghost.style.whiteSpace=label.resolvedStyle.whiteSpace;
   ghost.style.color=KarineTheme.Alpha(KarineTheme.Film.Phosphor,.3f*Fx.Amount);label.parent.Add(ghost);last=label.text;
   KarineMotion.Run(ghost,S.GhostSeconds,t=>ghost.style.opacity=1-t,()=>ghost.RemoveFromHierarchy());
  }).Every(50);
 }

 // Bant takılıyor: kaset yuvaya kayar, tık sesi.
 public static void TapeInsert(VisualElement root) {
  if(root==null || !Fx.On)return;
  var veil=Veil(root,"TapeInsert",.6f);veil.pickingMode=PickingMode.Ignore;
  var slot=new VisualElement {pickingMode=PickingMode.Ignore};slot.style.width=200;slot.style.height=16;slot.style.backgroundColor=Color.black;
  Border(slot,1,KarineTheme.Alpha(KarineTheme.Paper.Light,.4f));veil.Add(slot);
  var tape=new VisualElement {pickingMode=PickingMode.Ignore};tape.style.position=Position.Absolute;tape.style.width=180;tape.style.height=110;
  tape.style.backgroundColor=KarineTheme.GlassDeep;Border(tape,2,KarineTheme.Alpha(KarineTheme.Paper.Light,.5f));Round(tape,6);veil.Add(tape);
  for(int i=0;i<2;i++){var reel=new VisualElement {pickingMode=PickingMode.Ignore};reel.style.position=Position.Absolute;reel.style.top=30;reel.style.left=30+i*80;
   reel.style.width=40;reel.style.height=40;Round(reel,20);Border(reel,2,KarineTheme.Alpha(KarineTheme.Paper.Light,.5f));tape.Add(reel);}
  KarineMotion.Run(veil,S.InsertSeconds,t=> {
   float e=Mathf.Clamp01(t/.7f);tape.style.translate=new Translate(0,Mathf.Lerp(160,0,e*e));
   tape.style.scale=new Scale(new Vector3(1,Mathf.Lerp(1,.12f,Mathf.Clamp01((t-.55f)/.3f)),1));
   if(t>.85f)veil.style.opacity=1-(t-.85f)/.15f;
  },()=>{Cue("tape");veil.RemoveFromHierarchy();});
 }
}
}
