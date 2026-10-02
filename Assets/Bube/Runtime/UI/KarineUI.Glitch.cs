using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Bozulmalar: bant aşınması, terminal satırının bir an çöpe dönmesi, büyük
// geçişte film yanığı ve mühürde renk kayması.
//
// Bozulma rastgele zamanda ve rastgele satırda olur; hiçbir kaydı ya da
// satırı işaret etmez, metnin kendisi hiçbir zaman değişmez.
public static partial class KarineUI {
 public static void TapeWear(VisualElement frame,int views) {
  if(frame==null || !Fx.On || views<2)return;
  float alpha=Mathf.Min(.12f,(views-1)*.015f)*Fx.Amount;
  var wear=new Image {name="TapeWear",image=Grain(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=new Color(1,1,1,alpha)};
  wear.style.position=Position.Absolute;wear.style.left=0;wear.style.right=0;wear.style.top=0;wear.style.bottom=0;frame.Add(wear);
  var random=new System.Random(views);
  wear.schedule.Execute(()=>wear.uv=new Rect((float)random.NextDouble(),(float)random.NextDouble(),3,2)).Every(KarineTheme.Motion.TickMs*3);
 }

 // Terminal: arada bir, rastgele bir satır bir an karakter çöpüne döner ve geri gelir.
 const string Junk="░▒▓█▌▐#%&@$";
 public static void Corrupt(VisualElement area) {
  if(area==null || !Fx.On)return;
  var random=new System.Random();
  Action plan=null;
  plan=()=>area.schedule.Execute(()=> {
   if(area.panel==null)return;
   var labels=area.Query<Label>().ToList();
   if(labels.Count>0 && Fx.MayFlash()) {
    var label=labels[random.Next(labels.Count)];string text=label.text;
    if(!string.IsNullOrEmpty(text) && text.Length>4 && !text.Contains("<")) {
     var chars=text.ToCharArray();int from=random.Next(chars.Length-3),length=Mathf.Min(chars.Length-from,3+random.Next(6));
     for(int i=from;i<from+length;i++)if(chars[i]!=' ')chars[i]=Junk[random.Next(Junk.Length)];
     label.text=new string(chars);
     label.schedule.Execute(()=>{if(label.text==new string(chars))label.text=text;}).StartingIn(110);
    }
   }
   plan();
  }).StartingIn(9000+random.Next(14000));
  plan();
 }

 // Film yanığı: kenardan kavuran turuncu, sonra beyaz, sonra karanlık.
 public static void FilmBurn(VisualElement root,Action done) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  Cue("burn");
  var layer=new VisualElement {name="FilmBurn",pickingMode=PickingMode.Position};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;root.Add(layer);
  var hot=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=new Color(1,.55f,.15f,0)};
  hot.style.position=Position.Absolute;hot.style.right=Length.Percent(-30);hot.style.top=Length.Percent(-30);
  hot.style.width=Length.Percent(10);hot.style.height=Length.Percent(10);layer.Add(hot);
  KarineMotion.Run(layer,S.BurnSeconds,t=> {
   float grow=Mathf.Clamp01(t/.6f);
   hot.style.width=hot.style.height=Length.Percent(10+grow*190);
   hot.tintColor=Color.Lerp(new Color(1,.55f,.15f,.9f),new Color(1,.97f,.9f,1),Mathf.Clamp01((t-.4f)/.3f));
   layer.style.backgroundColor=new Color(0,0,0,Mathf.Clamp01((t-.7f)/.3f));
  },()=>{layer.RemoveFromHierarchy();done?.Invoke();});
 }

 // Mühür iner: ekran bir an kırmızı ve camgöbeği kayar.
 public static void ChromaShake(VisualElement root) {
  if(root==null || !Fx.On || !Fx.MayFlash())return;
  var red=Tint(root,KarineTheme.Film.ChromaRed);var cyan=Tint(root,KarineTheme.Film.ChromaCyan);
  KarineMotion.Run(red,S.ChromaSeconds,t=> {
   float k=(1-t)*6*Fx.Amount;
   red.style.translate=new Translate(k,0);cyan.style.translate=new Translate(-k,0);
   red.style.opacity=cyan.style.opacity=1-t;
  },()=>{red.RemoveFromHierarchy();cyan.RemoveFromHierarchy();});
 }
 static VisualElement Tint(VisualElement root,Color color) {
  var e=new VisualElement {pickingMode=PickingMode.Ignore};
  e.style.position=Position.Absolute;e.style.left=0;e.style.right=0;e.style.top=0;e.style.bottom=0;e.style.backgroundColor=color;root.Add(e);return e;
 }
}
}
