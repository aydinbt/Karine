using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Kapanış ve menü: raporun zarfa girmesi, rafa kalkan klasör ve "dosya
// kapandı" kartı, sicilde kuruyan mürekkep, neonun kekelemesi, kariyer
// rozetinin parlaması ve yeni güven durumunun damgası, vaka kartının raftan
// çekilmesi, ayarlardaki efekt önizlemesi.
//
// Zarf, raf ve kart her vakada aynıdır; sonucun iyi ya da kötü olduğunu söylemez.
public static partial class KarineUI {
 static VisualElement Veil(VisualElement root,string name,float alpha) {
  var veil=new VisualElement {name=name};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=new Color(0,0,0,alpha);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;
  root.Add(veil);return veil;
 }

 // Rapor zarfa kayar, kapak katlanır. Zarf ekranda kalır: mühür onun üstüne
 // iner, sonra `then` zarfı kaldırır.
 public static void EnvelopeSeal(VisualElement root,Action<VisualElement> then) {
  if(root==null || !Fx.On){then?.Invoke(null);return;}
  Cue("envelope");
  var veil=Veil(root,"EnvelopeVeil",.45f);
  var envelope=new VisualElement {pickingMode=PickingMode.Ignore};
  envelope.style.width=360;envelope.style.height=210;envelope.style.overflow=Overflow.Hidden;
  envelope.style.backgroundColor=KarineTheme.Paper.Folder;Border(envelope,2,KarineTheme.Paper.Edge);veil.Add(envelope);
  var sheet=new VisualElement {pickingMode=PickingMode.Ignore};
  sheet.style.position=Position.Absolute;sheet.style.left=Length.Percent(10);sheet.style.right=Length.Percent(10);
  sheet.style.height=Length.Percent(90);sheet.style.backgroundColor=KarineTheme.Paper.Sheet;envelope.Add(sheet);
  for(int i=0;i<5;i++) {
   var line=new VisualElement {pickingMode=PickingMode.Ignore};line.style.height=2;line.style.marginTop=14;
   line.style.marginLeft=16;line.style.marginRight=16+i*9;line.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Ink,.25f);sheet.Add(line);
  }
  var flap=new VisualElement {pickingMode=PickingMode.Ignore};
  flap.style.position=Position.Absolute;flap.style.left=Length.Percent(10);flap.style.width=Length.Percent(80);
  flap.style.height=Length.Percent(80);flap.style.top=Length.Percent(-40);flap.style.rotate=new Rotate(Angle.Degrees(45));
  flap.style.backgroundColor=KarineTheme.Paper.FolderDeep;flap.style.transformOrigin=new TransformOrigin(Length.Percent(50),Length.Percent(50));
  flap.style.opacity=0;envelope.Add(flap);
  KarineMotion.Run(envelope,S.EnvelopeSeconds,t=> {
   float slide=Mathf.Clamp01(t/.6f);
   sheet.style.top=Length.Percent(Mathf.Lerp(-95,8,slide*slide));
   float fold=Mathf.Clamp01((t-.6f)/.4f);
   flap.style.opacity=fold>0?1:0;
   flap.style.scale=new Scale(new Vector3(.71f,.71f*Mathf.Lerp(-1,1,fold),1));
  },()=>then?.Invoke(veil));
 }

 // Dosya kapandı (3 Ekim 2026 maketi, Docs/Reference/UI_CASE_CLOSED_2026-10.png): rapor açık
 // klasörde, kapak kapanır, "KAPANDI" damgası iner ve geçiş orada biter. Her vakada aynı;
 // sonucu söylemez. Dokunmak geçer.
 public static void CaseClosed(VisualElement root,string label,string stampText,Action done,string note=null) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"CaseClosedVeil",.92f);
  var folder=new VisualElement {pickingMode=PickingMode.Ignore};
  folder.style.width=S.ClosedFolderWidth;folder.style.height=S.ClosedFolderHeight;
  folder.style.backgroundColor=KarineTheme.Paper.FolderDeep;Border(folder,2,KarineTheme.Paper.Edge);veil.Add(folder);
  var sheet=new VisualElement {pickingMode=PickingMode.Ignore};sheet.style.position=Position.Absolute;
  sheet.style.left=sheet.style.right=sheet.style.top=sheet.style.bottom=Length.Percent(6);
  sheet.style.backgroundColor=KarineTheme.Paper.Sheet;sheet.style.paddingLeft=sheet.style.paddingRight=sheet.style.paddingTop=KarineTheme.SpaceLg;folder.Add(sheet);
  for(int i=0;i<7;i++) {
   var line=new VisualElement {pickingMode=PickingMode.Ignore};line.style.height=2;line.style.marginBottom=KarineTheme.SpaceMd;
   line.style.marginRight=i*11%40;line.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Ink,i==0?.6f:.22f);sheet.Add(line);
  }
  var tab=new VisualElement {pickingMode=PickingMode.Ignore};tab.style.position=Position.Absolute;tab.style.right=-S.ClosedTab;
  tab.style.top=Length.Percent(10);tab.style.width=S.ClosedTab;tab.style.height=Length.Percent(60);
  tab.style.backgroundColor=KarineTheme.Paper.Light;Border(tab,1,KarineTheme.Paper.Edge);tab.style.justifyContent=Justify.Center;tab.style.alignItems=Align.Center;folder.Add(tab);
  var tabText=Technical(tab,label,KarineTheme.Office.LabelSize);tabText.style.color=KarineTheme.Paper.Ink;tabText.style.rotate=new Rotate(90);
  tabText.style.whiteSpace=WhiteSpace.NoWrap;tabText.style.position=Position.Absolute;
  var cover=new VisualElement {pickingMode=PickingMode.Ignore};cover.style.position=Position.Absolute;
  cover.style.left=cover.style.right=cover.style.top=cover.style.bottom=0;cover.style.backgroundColor=KarineTheme.Paper.Folder;
  Stretched(cover,"Bube/UI/paper_folder");Border(cover,2,KarineTheme.Paper.Edge);
  cover.style.transformOrigin=new TransformOrigin(0,Length.Percent(50));cover.style.scale=new Scale(new Vector3(0,1,1));folder.Add(cover);
  var stamp=Technical(folder,stampText,S.ClosedStampSize);stamp.style.color=KarineTheme.Paper.Stamp;stamp.style.position=Position.Absolute;
  stamp.style.alignSelf=Align.Center;stamp.style.top=Length.Percent(38);Border(stamp,4,KarineTheme.Paper.Stamp);Round(stamp,4);
  stamp.style.paddingLeft=stamp.style.paddingRight=KarineTheme.SpaceLg;stamp.style.letterSpacing=4;stamp.style.rotate=new Rotate(-10);stamp.style.opacity=0;
  Label noteText=null;
  if(!string.IsNullOrEmpty(note)) {
   noteText=Technical(veil,note,KarineTheme.Office.LabelSize);noteText.style.color=KarineTheme.Paper.Light;
   noteText.style.marginTop=KarineTheme.SpaceLg;noteText.style.maxWidth=S.ClosedFolderWidth;noteText.style.whiteSpace=WhiteSpace.Normal;
   noteText.style.unityTextAlign=TextAnchor.MiddleCenter;noteText.style.opacity=0;
  }
  bool finished=false,stamped=false,closed=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.35f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(veil,S.ArchiveSeconds,t=> {
   if(noteText!=null)noteText.style.opacity=Mathf.Clamp01((t-.8f)/.15f);
   float shut=Smooth((t-.2f)/.3f);cover.style.scale=new Scale(new Vector3(shut,1,1));
   if(shut>=1 && !closed){closed=true;Cue("shelf");}
   float drop=Mathf.Clamp01((t-.6f)/.12f);
   stamp.style.opacity=drop;stamp.style.scale=new Scale(Vector3.one*Mathf.Lerp(2.4f,1,drop*drop));
   if(drop>=1 && !stamped){stamped=true;Sound?.Invoke("ui_stamp");Fx.Buzz(Haptic.Thud);}
   float shake=t>.72f && t<.8f?Mathf.Sin((t-.72f)*200)*3f:0;
   folder.style.rotate=new Rotate(shake);
  },()=>veil.schedule.Execute(finish).StartingIn(note==null?900:2600));
 }

 // Eski dosya yeniden açıldı (#008): sararmış bir klasör masaya kayar, üstünde yıl ve
 // "YENİDEN AÇILDI" damgası, altında devir notu. Dokununca kapanır; ipucu taşımaz.
 public static void FileReopened(VisualElement root,string year,string title,string stampText,string note,Action done=null) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"FileReopenedVeil",.9f);
  var folder=new VisualElement {pickingMode=PickingMode.Ignore};
  folder.style.width=S.ClosedFolderWidth;folder.style.height=S.ClosedFolderHeight;folder.style.backgroundColor=KarineTheme.Paper.Folder;
  Stretched(folder,"Bube/UI/paper_folder");Border(folder,2,KarineTheme.Paper.Edge);
  folder.style.unityBackgroundImageTintColor=new Color(.86f,.74f,.52f);folder.style.justifyContent=Justify.Center;folder.style.alignItems=Align.Center;veil.Add(folder);
  var yearText=Technical(folder,year,S.ClosedStampSize);yearText.style.color=KarineTheme.Alpha(KarineTheme.Paper.Ink,.75f);yearText.style.letterSpacing=6;
  var titleText=Technical(folder,title,KarineTheme.Office.LabelSize);titleText.style.color=KarineTheme.Paper.Ink;
  titleText.style.whiteSpace=WhiteSpace.Normal;titleText.style.unityTextAlign=TextAnchor.MiddleCenter;titleText.style.maxWidth=Length.Percent(80);
  var stamp=Technical(folder,stampText,KarineTheme.Office.LabelSize);stamp.style.color=KarineTheme.Paper.Stamp;stamp.style.position=Position.Absolute;
  stamp.style.bottom=Length.Percent(12);Border(stamp,3,KarineTheme.Paper.Stamp);Round(stamp,4);stamp.style.letterSpacing=3;
  stamp.style.paddingLeft=stamp.style.paddingRight=KarineTheme.SpaceMd;stamp.style.rotate=new Rotate(-8);stamp.style.opacity=0;
  var noteText=Technical(veil,note??string.Empty,KarineTheme.Office.LabelSize);noteText.style.color=KarineTheme.Paper.Light;
  noteText.style.marginTop=KarineTheme.SpaceLg;noteText.style.maxWidth=S.ClosedFolderWidth;noteText.style.whiteSpace=WhiteSpace.Normal;
  noteText.style.unityTextAlign=TextAnchor.MiddleCenter;noteText.style.opacity=0;
  bool finished=false,stamped=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.35f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  Cue("shelf");
  KarineMotion.Run(veil,S.ArchiveSeconds,t=> {
   float slide=Smooth(t/.35f);folder.style.translate=new Translate(0,Mathf.Lerp(420,0,slide));folder.style.rotate=new Rotate(Mathf.Lerp(-7,-2,slide));
   float drop=Mathf.Clamp01((t-.5f)/.12f);stamp.style.opacity=drop;stamp.style.scale=new Scale(Vector3.one*Mathf.Lerp(2.2f,1,drop*drop));
   if(drop>=1 && !stamped){stamped=true;Sound?.Invoke("ui_stamp");Fx.Buzz(Haptic.Thud);}
   noteText.style.opacity=Mathf.Clamp01((t-.7f)/.2f);
  },()=>veil.schedule.Execute(finish).StartingIn(2800));
 }
 static float Smooth(float x){x=Mathf.Clamp01(x);return x*x*(3-2*x);}

 // Mürekkep kurur: yazı önce ıslak ve parlak, sonra olağan rengine oturur.
 public static void InkDry(VisualElement page) {
  if(page==null || !Fx.On)return;
  var labels=page.Query<Label>().ToList();
  foreach(var label in labels)label.style.textShadow=new TextShadow{offset=new Vector2(0,-.6f),blurRadius=1.5f,color=new Color(1,1,1,.55f)};
  KarineMotion.Run(page,S.DrySeconds,t=> {
   foreach(var label in labels) {
    label.style.textShadow=new TextShadow{offset=new Vector2(0,-.6f),blurRadius=1.5f,color=new Color(1,1,1,.55f*(1-t))};
    label.style.opacity=Mathf.Lerp(.8f,1,t);
   }
  },()=>{foreach(var label in labels){label.style.textShadow=StyleKeyword.Null;label.style.opacity=StyleKeyword.Null;}});
 }

 // Neon arada bir kekeler: 20-50 saniyede bir, ışığa duyarlılık sınırıyla.
 public static void NeonStutter(VisualElement logo) {
  if(logo==null)return;
  var random=new System.Random();
  Action plan=null;
  plan=()=>logo.schedule.Execute(()=> {
   if(logo.panel==null)return;
   if(Fx.On && Fx.MayFlash()) {
    logo.style.opacity=.35f;
    logo.schedule.Execute(()=>logo.style.opacity=StyleKeyword.Null).StartingIn(70);
   }
   plan();
  }).StartingIn(random.Next(S.NeonMinMs,S.NeonMaxMs));
  plan();
 }

 // Rozet parlaması: kutunun üstünden soldan sağa geçen ince ışık, seyrek.
 public static void BadgeShine(VisualElement badge) {
  if(badge==null || !Fx.On)return;
  badge.style.overflow=Overflow.Hidden;
  var shine=new VisualElement {name="BadgeShine",pickingMode=PickingMode.Ignore};
  shine.style.position=Position.Absolute;shine.style.top=Length.Percent(-20);shine.style.bottom=Length.Percent(-20);shine.style.width=18;
  shine.style.backgroundColor=new Color(1,1,1,.10f*Fx.Amount);shine.style.rotate=new Rotate(Angle.Degrees(20));shine.style.left=Length.Percent(-20);
  badge.Add(shine);
  badge.schedule.Execute(()=>{if(shine.panel!=null)KarineMotion.Run(shine,S.ShineSeconds,t=>shine.style.left=Length.Percent(Mathf.Lerp(-20,120,t)));})
   .Every(S.ShineEveryMs).StartingIn(1200);
 }

 // Yeni güven durumu: kutu mühür gibi iner. Yalnız durum değiştiğinde, bir kez.
 public static void RankStamp(VisualElement badge) {
  if(badge==null)return;
  Cue("rank");
  if(!Fx.On)return;
  KarineMotion.Run(badge,.5f,t=> {
   float drop=Mathf.Clamp01(t/.4f);
   badge.style.scale=new Scale(Vector3.one*Mathf.Lerp(1.5f,1,drop*drop));
   badge.style.rotate=new Rotate(Angle.Degrees((1-drop)*-6));
  },()=>{badge.style.scale=StyleKeyword.Null;badge.style.rotate=StyleKeyword.Null;});
 }

 // Vaka kartı raftan çekilir: basılınca yukarı kalkar, bırakınca iner.
 public static void PullOut(VisualElement card) {
  if(card==null)return;
  card.RegisterCallback<PointerDownEvent>(_=>{if(Fx.On){card.style.translate=new Translate(0,-S.PullLift);Lift(card,.6f);}},TrickleDown.TrickleDown);
  EventCallback<EventBase> back=_=>{card.style.translate=StyleKeyword.Null;Lift(card,0);};
  card.RegisterCallback<PointerUpEvent>(e=>back(e));card.RegisterCallback<PointerLeaveEvent>(e=>back(e));
 }

 // Ayarlarda efekt önizlemesi: seçili yoğunlukta gren ve yağmurdan küçük bir pencere.
 public static void FxPreview(VisualElement parent,FxLevel level) {
  float amount=level==FxLevel.Off || KarineMotion.Reduced?0:level==FxLevel.Light?.5f:1f;
  var box=new VisualElement {name="FxPreview",pickingMode=PickingMode.Ignore};
  box.style.height=96;box.style.marginTop=KarineTheme.SpaceSm;box.style.overflow=Overflow.Hidden;
  box.style.backgroundColor=new Color(.06f,.08f,.11f);Border(box,1,KarineTheme.Accent);parent.Add(box);
  var grainImage=new Image {image=Grain(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=new Color(1,1,1,KarineTheme.Film.GrainAlpha*2*amount)};
  grainImage.style.position=Position.Absolute;grainImage.style.left=0;grainImage.style.right=0;grainImage.style.top=0;grainImage.style.bottom=0;box.Add(grainImage);
  var random=new System.Random(3);int count=Mathf.RoundToInt(18*amount);
  var streaks=new (VisualElement e,float x,float phase)[count];
  for(int i=0;i<count;i++) {
   var s=new VisualElement {pickingMode=PickingMode.Ignore};s.style.position=Position.Absolute;s.style.width=1;s.style.height=Length.Percent(14);
   s.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Office.Weather.RainColor,.35f);s.style.rotate=new Rotate(Angle.Degrees(8));box.Add(s);
   streaks[i]=(s,(float)random.NextDouble()*110,(float)random.NextDouble()*100);
  }
  float start=Time.realtimeSinceStartup;
  box.schedule.Execute(()=> {
   float time=Time.realtimeSinceStartup-start;
   grainImage.uv=new Rect((float)random.NextDouble(),(float)random.NextDouble(),4,1);
   foreach(var s in streaks){float y=Mathf.Repeat(s.phase+time*90,130)-20;s.e.style.left=Length.Percent(s.x-y*.14f);s.e.style.top=Length.Percent(y);}
  }).Every(KarineTheme.Motion.TickMs*2);
  if(amount<=0) {
   var off=Technical(box,"—",KarineTheme.Office.LabelSize);off.style.color=KarineTheme.Secondary;
   off.style.position=Position.Absolute;off.style.left=0;off.style.right=0;off.style.top=Length.Percent(35);off.style.unityTextAlign=TextAnchor.MiddleCenter;
  }
 }
}
}
