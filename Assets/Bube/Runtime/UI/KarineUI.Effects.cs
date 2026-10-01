using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// İkinci kademe efektleri: kamera değişiminde sinyal bozulması ve sayfa çevirme.
// Kural birinci kademeyle aynı: efekt hissettirir, yönlendirmez; doğru ya da
// yanlış seçim için ayrı efekt yoktur.
public static partial class KarineUI {

 // Kamera değişti: görüntü birkaç kare kayar, yatay parazit bantları çakar ve
 // söner. Bu bir arayüz yüzeyi değil, **kameranın resmidir** — renkler kit
 // paletinden değil, kamera camının yeşilinden gelir.
 public static void SignalSwitch(VisualElement frame) {
  if(frame==null || KarineMotion.Reduced)return;
  var noise=new VisualElement {name="SignalSwitch",pickingMode=PickingMode.Ignore};
  noise.style.position=Position.Absolute;noise.style.left=0;noise.style.right=0;noise.style.top=0;noise.style.bottom=0;
  noise.style.overflow=Overflow.Hidden;
  noise.style.backgroundColor=new Color(.02f,.05f,.06f,.85f);frame.Add(noise);
  var bands=new VisualElement[KarineTheme.Effects.SignalBands];
  for(int i=0;i<bands.Length;i++) {
   var band=new VisualElement {pickingMode=PickingMode.Ignore};
   band.style.position=Position.Absolute;band.style.left=0;band.style.right=0;
   band.style.backgroundColor=new Color(.82f,.95f,.91f,1);noise.Add(band);bands[i]=band;
  }
  var random=new System.Random();
  KarineMotion.Run(noise,KarineTheme.Effects.SignalSeconds,t=> {
   noise.style.opacity=1-t;
   foreach(var band in bands) {
    band.style.top=Length.Percent((float)random.NextDouble()*100);
    band.style.height=1+(float)random.NextDouble()*6;
    band.style.opacity=(float)random.NextDouble()*.35f*(1-t);
    band.style.translate=new Translate(((float)random.NextDouble()-.5f)*40*(1-t),0);
   }
  },()=>noise.RemoveFromHierarchy());
 }

 // Karşıdaki kişi nefes alır: portre göğüs hizasından çok hafif genişler ve
 // daralır. Genlik ve hız **herkeste aynı**; kişiye, yanıta ya da öne sürülen
 // kayda göre değişmez — değişseydi oyuncu onu gizli bir durum diye okurdu.
 // Yalnız başlangıç anı rastgeledir, böylece her girişte aynı karede başlamaz.
 public static void Breathe(VisualElement figure) {
  if(figure==null || KarineMotion.Reduced)return;
  figure.name="BreathingPortrait";
  figure.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(100));
  float phase=UnityEngine.Random.value*Mathf.PI*2,start=Time.realtimeSinceStartup;
  figure.schedule.Execute(()=> {
   float breath=Mathf.Sin((Time.realtimeSinceStartup-start)*Mathf.PI*2/KarineTheme.Effects.BreathSeconds+phase);
   figure.style.scale=new Scale(new Vector3(1+KarineTheme.Effects.BreathWidth*breath,1+KarineTheme.Effects.BreathHeight*breath,1));
  }).Every(KarineTheme.Motion.TickMs);
 }

 // Sayfa çevrilir: gölgeli bir kenar sağdan sola sayfanın üstünden geçer.
 public static void PageTurn(VisualElement paper) {
  if(paper==null || KarineMotion.Reduced)return;
  var fold=new VisualElement {name="PageTurn",pickingMode=PickingMode.Ignore};
  fold.style.position=Position.Absolute;fold.style.top=0;fold.style.bottom=0;
  fold.style.width=Length.Percent(KarineTheme.Effects.FoldWidth);
  fold.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.FolderDeep,KarineTheme.Effects.FoldAlpha);
  fold.style.borderLeftWidth=2;fold.style.borderLeftColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.5f);
  paper.Add(fold);
  KarineMotion.Run(fold,KarineTheme.Effects.TurnSeconds,t=> {
   fold.style.left=Length.Percent(100-(100+KarineTheme.Effects.FoldWidth)*t);
   fold.style.opacity=1-t*t;
  },()=>fold.RemoveFromHierarchy());
 }
}
}
