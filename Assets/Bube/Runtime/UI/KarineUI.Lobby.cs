using System;
using UnityEngine;
using UnityEngine.UIElements;
using W = Bube.KarineTheme.Office.Weather;

namespace Bube {
// Menü ve kariyer: ana menünün arkasında yaşayan ofis (camda yağmur, arada
// geçen bir far), kariyer duvarındaki gazete kupürleri, rütbe töreni.
public static partial class KarineUI {
 public static void LiveMenu(VisualElement root) {
  if(root==null || !Fx.On)return;
  var layer=new VisualElement {name="LiveMenu",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  layer.style.overflow=Overflow.Hidden;root.Add(layer);
  var random=new System.Random(17);int count=Fx.Count(28);
  var drops=new (VisualElement e,float x,float phase,float speed)[count];
  for(int i=0;i<count;i++) {
   var d=new VisualElement {pickingMode=PickingMode.Ignore};d.style.position=Position.Absolute;d.style.width=1;d.style.height=Length.Percent(6);
   d.style.backgroundColor=KarineTheme.Alpha(W.RainColor,.18f);d.style.rotate=new Rotate(Angle.Degrees(10));layer.Add(d);
   drops[i]=(d,(float)random.NextDouble()*110,(float)random.NextDouble()*100,40+(float)random.NextDouble()*30);
  }
  var beam=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(W.BeamColor,0)};
  beam.style.position=Position.Absolute;beam.style.top=Length.Percent(20);beam.style.width=Length.Percent(40);beam.style.height=Length.Percent(60);layer.Add(beam);
  float start=Time.realtimeSinceStartup,nextBeam=6+(float)random.NextDouble()*8,beamAt=-100;
  layer.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   foreach(var d in drops){float y=Mathf.Repeat(d.phase+time*d.speed,120)-10;d.e.style.left=Length.Percent(d.x-y*.17f);d.e.style.top=Length.Percent(y);}
   if(time>nextBeam){beamAt=time;nextBeam=time+14+(float)random.NextDouble()*16;}
   float b=(time-beamAt)/2.4f;
   if(b>=0 && b<=1){beam.style.left=Length.Percent(-40+b*140);beam.tintColor=KarineTheme.Alpha(W.BeamColor,Mathf.Sin(b*Mathf.PI)*.10f*Fx.Amount);}
   else beam.tintColor=KarineTheme.Alpha(W.BeamColor,0);
  }).Every(KarineTheme.Motion.TickMs*2);
 }

 // Kariyer duvarı: kapanan her vaka için iğneli bir gazete kupürü.
 public static VisualElement Clipping(VisualElement parent,string headline,string line,int index,Action open) {
  var card=new Button(Sounded(open)) {name="Clipping"};
  card.style.width=150;card.style.minHeight=92;card.style.marginRight=KarineTheme.SpaceMd;card.style.marginTop=10;
  card.style.paddingLeft=10;card.style.paddingRight=10;card.style.paddingTop=12;card.style.paddingBottom=8;
  card.style.backgroundColor=KarineTheme.Paper.Light;Border(card,1,KarineTheme.Paper.Edge);
  card.style.rotate=new Rotate(Angle.Degrees(index%3==0?-2.5f:index%3==1?1.8f:-.8f));
  card.style.flexDirection=FlexDirection.Column;card.style.alignItems=Align.Stretch;
  var head=Body_(card,headline,KarineTheme.CaseBrowser.SmallSize);head.style.color=KarineTheme.Paper.Ink;head.style.whiteSpace=WhiteSpace.Normal;
  head.style.unityFontStyleAndWeight=FontStyle.Bold;
  var sub=Technical(card,line,KarineTheme.Office.SmallSize);sub.style.color=KarineTheme.Paper.Faded;sub.style.whiteSpace=WhiteSpace.Normal;
  var pin=new VisualElement {pickingMode=PickingMode.Ignore};pin.style.position=Position.Absolute;pin.style.top=-5;pin.style.left=Length.Percent(46);
  pin.style.width=10;pin.style.height=10;Round(pin,5);pin.style.backgroundColor=KarineTheme.Paper.Stamp;card.Add(pin);
  parent.Add(card);return card;
 }

 // Rütbe töreni: yeni güven durumu ortada belirir, rozet iner. Dokunmak geçer.
 public static void RankCeremony(VisualElement root,string status,Action done) {
  Cue("rank");
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"RankCeremony",.85f);
  var badge=new VisualElement {pickingMode=PickingMode.Ignore};
  badge.style.width=120;badge.style.height=120;Round(badge,60);Border(badge,3,KarineTheme.Primary);
  badge.style.backgroundColor=KarineTheme.GlassDeep;badge.style.alignItems=Align.Center;badge.style.justifyContent=Justify.Center;veil.Add(badge);
  var star=Technical(badge,"★",KarineTheme.Office.TitleSize+14);star.style.color=KarineTheme.Primary;
  var label=Technical(veil,status,KarineTheme.Office.TitleSize);label.style.color=KarineTheme.Primary;label.style.letterSpacing=4;label.style.marginTop=KarineTheme.SpaceLg;
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.35f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(veil,1.2f,t=> {
   float drop=Mathf.Clamp01(t/.35f);
   badge.style.scale=new Scale(Vector3.one*Mathf.Lerp(1.8f,1,drop*drop));
   label.style.opacity=Mathf.Clamp01((t-.4f)/.3f);
  },()=>{BadgeShine(badge);veil.schedule.Execute(finish).StartingIn(1600);});
 }
}
}
