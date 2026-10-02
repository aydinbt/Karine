using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Terminal ve kayıt cihazı: geri sarma, tuşla yazılan başlık ve yanıp sönen
// imleç, satır satır akan sonuçlar, bağlantı sesi ve ekranın camındaki yansıma.
//
// Arama sonuçları her aramada aynı hızla akar; hiçbir satır diğerinden önce
// ya da parlak gelmez.
public static partial class KarineUI {
 // Bant geri sarılır: görüntünün üstünden yukarı akan çizgiler, hafif çekme.
 public static void Rewind(VisualElement frame) {
  Cue("rewind");
  if(frame==null || !Fx.On)return;
  var layer=new VisualElement {name="RewindFx",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  layer.style.overflow=Overflow.Hidden;frame.Add(layer);
  var random=new System.Random(41);var lines=new VisualElement[Mathf.Max(3,Fx.Count(12))];var phase=new float[lines.Length];
  for(int i=0;i<lines.Length;i++) {
   var line=new VisualElement {pickingMode=PickingMode.Ignore};
   line.style.position=Position.Absolute;line.style.left=0;line.style.right=0;line.style.height=1+random.Next(3);
   line.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Film.Phosphor,.15f+(float)random.NextDouble()*.25f);
   layer.Add(line);lines[i]=line;phase[i]=(float)random.NextDouble();
  }
  KarineMotion.Run(layer,S.RewindSeconds,t=> {
   for(int i=0;i<lines.Length;i++)lines[i].style.top=Length.Percent(100-Mathf.Repeat(phase[i]+t*3,1)*100);
   layer.style.translate=new Translate(Mathf.Sin(t*40)*2,0);
  },()=>layer.RemoveFromHierarchy());
 }

 // Başlık tuş tuş yazılır, sonunda imleç yanıp söner (saniyede birden az).
 public static void TerminalType(Label label,string text) {
  if(label==null)return;
  const string cursor="▌";
  if(!Fx.On){label.text=text+" "+cursor;Blink();return;}
  int shown=0;label.text=cursor;
  IVisualElementScheduledItem task=null;
  task=label.schedule.Execute(()=> {
   if(shown>=text.Length){task.Pause();Blink();return;}
   shown++;label.text=text.Substring(0,shown)+cursor;
   if(text[shown-1]!=' ')Cue("key");
  }).Every(S.KeyMs);
  void Blink() {
   bool on=true;
   label.schedule.Execute(()=>{on=!on;label.text=text+" "+(on?cursor:" ");}).Every(S.CursorMs);
  }
 }

 // Sonuçlar satır satır akar: her satır sırayla belirir, arada tek tuş sesi.
 public static void Stream(VisualElement list) {
  if(list==null || !Fx.On)return;
  var rows=new System.Collections.Generic.List<VisualElement>(list.Children());
  for(int i=0;i<rows.Count;i++) {
   var row=rows[i];row.style.opacity=0;
   row.schedule.Execute(()=>{row.style.opacity=1;if(rows.IndexOf(row)%3==0)Cue("key");}).StartingIn(i*S.StreamMs);
  }
 }

 // Bağlantı: terminal ilk açıldığında "bağlanıyor…" ve modem sesi. Dokunmak geçer.
 public static void Connecting(VisualElement root,string text) {
  if(root==null)return;
  Cue("modem");
  if(!Fx.On)return;
  var veil=new VisualElement {name="TerminalConnecting"};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=new Color(.02f,.04f,.05f,.94f);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;
  root.Add(veil);
  var label=Technical(veil,text,KarineTheme.Office.TitleSize);label.style.color=KarineTheme.Film.Phosphor;
  int dots=0;label.schedule.Execute(()=>{dots=dots%3+1;label.text=text+new string('.',dots);}).Every(300);
  bool done=false;
  Action finish=()=>{if(done)return;done=true;KarineMotion.Run(veil,.25f,t=>veil.style.opacity=1-t,()=>veil.RemoveFromHierarchy());};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  veil.schedule.Execute(finish).StartingIn((long)(S.ModemSeconds*1000));
 }

 // Ekran camı: köşeden süzülen soluk yansıma ve üst kenarda ofis ışığı.
 public static void Glare(VisualElement frame) {
  if(frame==null || !Fx.On)return;
  var glare=new Image {name="ScreenGlare",image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,
   tintColor=new Color(1,1,1,S.GlareAlpha*Fx.Amount)};
  glare.style.position=Position.Absolute;glare.style.left=Length.Percent(-20);glare.style.top=Length.Percent(-40);
  glare.style.width=Length.Percent(70);glare.style.height=Length.Percent(90);glare.style.rotate=new Rotate(Angle.Degrees(-18));
  frame.Add(glare);
  var edge=new VisualElement {pickingMode=PickingMode.Ignore};
  edge.style.position=Position.Absolute;edge.style.left=Length.Percent(6);edge.style.right=Length.Percent(6);edge.style.top=2;edge.style.height=1;
  edge.style.backgroundColor=new Color(1,.95f,.85f,S.GlareAlpha*1.5f*Fx.Amount);frame.Add(edge);
 }
}
}
