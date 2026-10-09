using System;
using UnityEngine;
using UnityEngine.UIElements;
using L = Bube.KarineTheme.Loading;

namespace Bube {
// Açılış yükleme katmanı ve stüdyo imzası. Katman ana menünün **üstüne**
// biner; menü altında hazır durur, yükleme bitince katman söner.
public static partial class KarineUI {

 // Stüdyo imzası: `Bube/Art/BubeGamesLogo` varsa o çizilir; yoksa bugünkü
 // işaret + "BUBE GAMES" yazısı. Görsel gelince kod değişmez.
 public const string StudioResource = "Bube/Art/BubeGamesLogo";
 public static VisualElement StudioMark(VisualElement parent,int height) {
  var box=new VisualElement {name="StudioMark",pickingMode=PickingMode.Ignore};
  box.style.flexDirection=FlexDirection.Row;box.style.alignItems=Align.Center;parent.Add(box);
  var art=Resources.Load<Texture2D>(StudioResource);
  if(art!=null) {
   var image=new Image {image=art,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   image.style.height=height;image.style.width=height*(float)art.width/art.height;box.Add(image);return box;
  }
  var mark=new Image {image=Resources.Load<Texture2D>("Bube/UI/bube_logo_light"),scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
  mark.style.width=height;mark.style.height=height;mark.style.marginRight=KarineTheme.SpaceSm;box.Add(mark);
  var name=Write(box,"BUBE GAMES",KarineTheme.Primary,L.StudioTextSize,Heading);name.style.marginBottom=0;name.style.letterSpacing=2;
  return box;
 }

 // Yükleme katmanı: logo, ilerleme çubuğu, dönen ipucu ve köşede stüdyo.
 // `progress` 0–1 gerçek ilerlemeyi alır; çubuk ona yumuşakça yetişir.
 public static VisualElement LoadingScreen(VisualElement root,string label,string[] tips,out Action<float> progress,out Action finish,float tipSeconds=L.TipSeconds) {
  var layer=new VisualElement {name="LoadingScreen"};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  layer.style.backgroundColor=KarineTheme.Background;layer.style.alignItems=Align.Center;layer.style.justifyContent=Justify.Center;
  layer.RegisterCallback<PointerDownEvent>(e=>e.StopPropagation());
  root.Add(layer);
  // Karartma dokusu beyazdır; renk tintle verilir (verilmezse kenarlar griye boğulur).
  var vignette=new Image {image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(KarineTheme.Background,.9f)};vignette.style.position=Position.Absolute;
  vignette.style.left=0;vignette.style.right=0;vignette.style.top=0;vignette.style.bottom=0;layer.Add(vignette);
  var logo=KarineLogo.Hero(layer,L.LogoWidth);
  var bar=new VisualElement();bar.style.width=Length.Percent(L.BarWidth);bar.style.height=L.BarHeight;bar.style.marginTop=KarineTheme.SpaceXl;
  bar.style.backgroundColor=KarineTheme.Panel2;Round(bar,L.BarHeight/2);bar.style.overflow=Overflow.Hidden;layer.Add(bar);
  var fill=new VisualElement();fill.style.height=Length.Percent(100);fill.style.width=0;fill.style.backgroundColor=KarineTheme.Accent;Round(fill,L.BarHeight/2);bar.Add(fill);
  var status=Technical(layer,label,L.LabelSize);status.style.color=KarineTheme.Secondary;status.style.marginTop=KarineTheme.SpaceSm;status.style.letterSpacing=2;
  var tip=Body_(layer,tips.Length>0?tips[UnityEngine.Random.Range(0,tips.Length)]:"",L.TipSize);
  tip.style.color=KarineTheme.Primary;tip.style.marginTop=KarineTheme.SpaceXl;tip.style.maxWidth=Length.Percent(60);
  tip.style.unityTextAlign=TextAnchor.MiddleCenter;tip.style.whiteSpace=WhiteSpace.Normal;
  var studio=StudioMark(layer,L.StudioHeight);studio.style.position=Position.Absolute;studio.style.right=Length.Percent(3);studio.style.bottom=Length.Percent(4);

  float target=0,shown=0;int index=Array.IndexOf(tips,tip.text);float nextTip=Time.realtimeSinceStartup+tipSeconds;
  layer.schedule.Execute(()=> {
   shown=Mathf.MoveTowards(shown,target,Time.unscaledDeltaTime*1.5f);
   fill.style.width=Length.Percent(shown*100);
   status.text=label+"  %"+Mathf.RoundToInt(shown*100);
   if(tips.Length>1 && Time.realtimeSinceStartup>nextTip) {
    nextTip=Time.realtimeSinceStartup+tipSeconds;index=(index+1)%tips.Length;var text=tips[index];
    Run(tip,.5f,t=>tip.style.opacity=1-t,()=>{tip.text=text;Run(tip,.5f,t=>tip.style.opacity=t,null);});
   }
  }).Every(16);
  progress=v=>target=Mathf.Max(target,Mathf.Clamp01(v));
  finish=()=>Run(layer,L.FadeSeconds,t=>layer.style.opacity=1-t,()=>layer.RemoveFromHierarchy());
  return layer;
 }
 // Açılış ekranı (9 Ekim 2026): video üstünde sırayla KARINE logosu, ardından Bube Games;
 // ikisi de ortada belirir, kalır, söner. Yükleme göstergesi sağ altta küçük durur.
 // `SplashSeconds` sıranın toplam süresidir; katman bundan önce kapanmaz.
 public static float SplashSeconds => 2*(2*L.SplashFade+L.SplashHold)+L.SplashGap;
 public static VisualElement SplashScreen(VisualElement root,string label,out Action<float> progress,out Action finish) {
  var layer=new VisualElement {name="LoadingScreen"};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  layer.style.backgroundColor=KarineTheme.Background;layer.style.alignItems=Align.Center;layer.style.justifyContent=Justify.Center;
  layer.RegisterCallback<PointerDownEvent>(e=>e.StopPropagation());root.Add(layer);
  var vignette=new Image {image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(KarineTheme.Background,.9f)};
  vignette.style.position=Position.Absolute;vignette.style.left=0;vignette.style.right=0;vignette.style.top=0;vignette.style.bottom=0;layer.Add(vignette);
  VisualElement Centered(){var box=new VisualElement {pickingMode=PickingMode.Ignore};box.style.position=Position.Absolute;box.style.left=0;box.style.right=0;box.style.top=0;box.style.bottom=0;
   box.style.alignItems=Align.Center;box.style.justifyContent=Justify.Center;box.style.opacity=0;layer.Add(box);return box;}
  var brand=Centered();KarineLogo.Hero(brand,L.SplashLogoWidth);
  var studio=Centered();StudioMark(studio,L.SplashStudioHeight);
  var corner=new VisualElement {pickingMode=PickingMode.Ignore};corner.style.position=Position.Absolute;corner.style.right=Length.Percent(3);corner.style.bottom=Length.Percent(5);corner.style.alignItems=Align.FlexEnd;layer.Add(corner);
  var status=Technical(corner,label,L.LabelSize);status.style.color=KarineTheme.Secondary;status.style.letterSpacing=2;status.style.marginBottom=KarineTheme.SpaceXs;
  var bar=new VisualElement();bar.style.width=L.CornerBarWidth;bar.style.height=L.CornerBarHeight;bar.style.backgroundColor=KarineTheme.Panel2;Round(bar,L.CornerBarHeight/2);bar.style.overflow=Overflow.Hidden;corner.Add(bar);
  var fill=new VisualElement();fill.style.height=Length.Percent(100);fill.style.width=0;fill.style.backgroundColor=KarineTheme.Accent;bar.Add(fill);
  float start=Time.realtimeSinceStartup,target=0,shown=0,one=2*L.SplashFade+L.SplashHold;
  float Curve(float t)=>t<0||t>one?0:Mathf.Min(1,Mathf.Min(t/L.SplashFade,(one-t)/L.SplashFade));
  layer.schedule.Execute(()=> {
   float age=Time.realtimeSinceStartup-start;
   brand.style.opacity=Curve(age);studio.style.opacity=Curve(age-one-L.SplashGap);
   shown=Mathf.MoveTowards(shown,target,Time.unscaledDeltaTime*1.5f);
   fill.style.width=Length.Percent(shown*100);status.text=label+"  %"+Mathf.RoundToInt(shown*100);
  }).Every(16);
  progress=v=>target=Mathf.Max(target,Mathf.Clamp01(v));
  finish=()=>Run(layer,L.FadeSeconds,t=>layer.style.opacity=1-t,()=>layer.RemoveFromHierarchy());
  return layer;
 }
}
}
