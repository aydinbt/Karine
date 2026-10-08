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

 // İzleyici siyahtan açılır: kısa bir karanlık, sonra görüntü belirir.
 // `done` görüntü tam açılınca çağrılır; kare dizisi ancak o an başlar,
 // böylece ilk kareler geçişin altında kaybolmaz. (Kaset animasyonu kaldırıldı.)
 public static void FromBlack(VisualElement host,Action done) {
  var black=new VisualElement {name="CctvFromBlack",pickingMode=PickingMode.Ignore};black.style.position=Position.Absolute;
  black.style.left=0;black.style.right=0;black.style.top=0;black.style.bottom=0;black.style.backgroundColor=Color.black;host.Add(black);
  if(KarineMotion.Reduced){black.RemoveFromHierarchy();done?.Invoke();return;}
  KarineMotion.Run(black,S.BlackHoldSeconds+S.BlackFadeSeconds,t=> {
   float k=Mathf.Clamp01((t*(S.BlackHoldSeconds+S.BlackFadeSeconds)-S.BlackHoldSeconds)/S.BlackFadeSeconds);black.style.opacity=1-k;
  },()=>{black.RemoveFromHierarchy();done?.Invoke();});
 }
}
}
