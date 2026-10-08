using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Kayıt öne sürme ve evrak basımı. Bu dosya `BubeApp`in bir parçasıdır.
public sealed partial class BubeApp {
 // Bir kez basılan kâğıt bir daha basılmaz; masaya her dönüşte tekrar yazılmasın.
 readonly HashSet<string> printedPapers=new HashSet<string>();

 // Seçilen kayıt kâğıt olarak masada karşıdaki kişiye doğru kayar. Doğru
 // kaynakta da, yemde de, ilgisiz kayıtta da **aynı** hareket: sonucu yanıt söyler.
 void SlideToPerson(VisualElement from,string label,Action then) {
  if(KarineMotion.Reduced || from.panel==null){then();return;}
  var start=root.WorldToLocal(from.worldBound);
  float width=Mathf.Min(start.width,root.layout.width*KarineTheme.Effects.SlideMaxWidth);
  var card=new VisualElement {name="PresentedPaper",pickingMode=PickingMode.Ignore};
  card.style.position=Position.Absolute;card.style.width=width;
  card.style.paddingLeft=12;card.style.paddingRight=12;card.style.paddingTop=9;card.style.paddingBottom=9;
  card.style.backgroundColor=KarineTheme.Paper.Sheet;
  card.style.borderLeftWidth=3;card.style.borderLeftColor=KarineTheme.Paper.Stamp;
  var text=Text(card,label,KarineTheme.Paper.Ink,14);text.style.marginBottom=0;
  // Kâğıt kayarken ekran dokunmaya kapalı: yarıda başka bir düğmeye basılırsa
  // öne sürme kaybolurdu.
  var blocker=new VisualElement {name="PresentBlocker"};
  blocker.style.position=Position.Absolute;blocker.style.left=0;blocker.style.right=0;
  blocker.style.top=0;blocker.style.bottom=0;root.Add(blocker);
  root.Add(card);
  from.style.opacity=0;
  audioDirector?.Play(AudioDirector.Press,.8f,.9f);
  float endX=root.layout.width*KarineTheme.Effects.SlideTargetX-width*.5f;
  float endY=root.layout.height*KarineTheme.Effects.SlideTargetY;
  KarineMotion.Run(card,KarineTheme.Effects.SlideSeconds,t=> {
   card.style.left=Mathf.Lerp(start.x,endX,t);card.style.top=Mathf.Lerp(start.y,endY,t);
   card.style.rotate=new Rotate(Angle.Degrees(-KarineTheme.Effects.SlideTilt*t));
   card.style.scale=new Scale(Vector3.one*(1-.18f*t));
   card.style.opacity=t<.75f?1:(1-t)/.25f;
  },()=>{card.RemoveFromHierarchy();blocker.RemoveFromHierarchy();then();});
 }

 // Düğme parmakla karşıdaki kişiye (sola) doğru sürülebilir. Eşik geçilince
 // kayıt öne sürülür; geçilmezse düğme yerine döner. Dokunmak da aynı işi görür.
 void DragToPresent(Button button,Action present) {
  float startX=0;bool dragging=false;
  button.RegisterCallback<PointerDownEvent>(evt=>{startX=evt.position.x;dragging=!KarineMotion.Reduced;if(dragging)KarineUI.Lift(button,.5f);},TrickleDown.TrickleDown);
  button.RegisterCallback<PointerMoveEvent>(evt=>{
   if(!dragging)return;
   float dx=Mathf.Min(0,evt.position.x-startX);
   button.style.translate=new Translate(dx,0);
   float reach=Mathf.Min(KarineTheme.Effects.SwipeDistance,button.layout.width*.4f);
   button.style.rotate=new Rotate(Angle.Degrees(dx*.035f));
   KarineUI.Lift(button,.5f+.5f*Mathf.Clamp01(-dx/Mathf.Max(1,reach)));
   if(-dx>reach){dragging=false;KarineUI.Lift(button,0);Fx.Buzz(Haptic.Press);present();}
  });
  Action settle=()=>{
   if(!dragging)return;dragging=false;
   float from=button.resolvedStyle.translate.x;KarineUI.Lift(button,0);
   KarineMotion.Run(button,KarineTheme.Motion.CloseSeconds,t=>{
    button.style.translate=new Translate(from*(1-t),0);button.style.rotate=new Rotate(Angle.Degrees(from*.02f*(1-t)));
   });
  };
  button.RegisterCallback<PointerUpEvent>(_=>settle());
  button.RegisterCallback<PointerCancelEvent>(_=>settle());
  button.RegisterCallback<PointerLeaveEvent>(_=>settle());
 }

 // Kâğıt satır satır basılarak çıkar ("Yazarak göster" açıkken). Düzen baştan
 // kurulur: basılmamış kısım saydam yazıyla yerinde durur, sayfa zıplamaz.
 // İç içe satırlar da (künye, kişi kartı) sırayla basılır. Kâğıda dokunmak
 // basımı bitirir. Her kâğıt oturumda bir kez basılır.
 void PrintOut(VisualElement body,string key) {
  if(instantText || KarineMotion.Reduced || !printedPapers.Add(key))return;
  var labels=body.Query<Label>().ToList().Where(l=>l.enableRichText && !string.IsNullOrEmpty(l.text) && l.text.IndexOf('<')<0).ToList();
  if(labels.Count==0)return;
  var texts=labels.ToDictionary(l=>l,l=>l.text);
  foreach(var l in labels)l.text=Unprinted(l.text,0);
  // Basılmamış kâğıttaki düğme görünmez **ve** dokunulmaz; basım bitince açılır.
  var buttons=body.Query<Button>().ToList();
  foreach(var b in buttons)b.style.visibility=Visibility.Hidden;
  int index=0,shown=0,tick=0;bool done=false;
  IVisualElementScheduledItem task=null;
  Action finish=()=>{
   if(done)return;done=true;task?.Pause();
   foreach(var pair in texts)pair.Key.text=pair.Value;
   foreach(var b in buttons)b.style.visibility=StyleKeyword.Null;
  };
  body.RegisterCallback<PointerDownEvent>(_=>finish(),TrickleDown.TrickleDown);
  task=body.schedule.Execute(()=>{
   if(done)return;
   if(index>=labels.Count || body.panel==null){finish();return;}
   var label=labels[index];var full=texts[label];
   shown=Mathf.Min(full.Length,shown+KarineTheme.Effects.PrintChars);
   label.text=Unprinted(full,shown);
   if(audioDirector!=null && ++tick%KarineTheme.Effects.PrintSoundEvery==0)
    audioDirector.Play(AudioDirector.Typewriter,.9f+.1f*UnityEngine.Random.value,.55f);
   if(shown>=full.Length){index++;shown=0;}
  }).Every(KarineTheme.Motion.TickMs);
 }

 static string Unprinted(string full,int shown) =>
  shown>=full.Length?full:full.Substring(0,shown)+"<alpha=#00>"+full.Substring(shown);
}
}
