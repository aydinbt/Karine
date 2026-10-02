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

 // Dosya kapandı: klasör sağdaki rafa kayar, kart yazılır. Dokunmak geçer.
 public static void CaseClosed(VisualElement root,string number,string title,string closedText,Action done) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"CaseClosedVeil",.92f);
  var shelf=new VisualElement {pickingMode=PickingMode.Ignore};
  shelf.style.position=Position.Absolute;shelf.style.right=Length.Percent(6);shelf.style.width=Length.Percent(22);
  shelf.style.top=Length.Percent(58);shelf.style.height=6;shelf.style.backgroundColor=KarineTheme.Paper.FolderDeep;veil.Add(shelf);
  var folder=new VisualElement {pickingMode=PickingMode.Ignore};
  folder.style.position=Position.Absolute;folder.style.width=180;folder.style.height=130;
  folder.style.backgroundColor=KarineTheme.Paper.Folder;Border(folder,2,KarineTheme.Paper.Edge);
  folder.style.paddingLeft=12;folder.style.paddingTop=10;veil.Add(folder);
  var tab=Technical(folder,number,KarineTheme.Office.LabelSize);tab.style.color=KarineTheme.Paper.Ink;
  var name=Body_(folder,title,KarineTheme.Office.LabelSize);name.style.color=KarineTheme.Paper.Ink;name.style.whiteSpace=WhiteSpace.Normal;
  var card=Technical(veil,string.Empty,KarineTheme.Office.TitleSize+6);card.style.color=KarineTheme.Primary;card.style.letterSpacing=6;
  card.style.position=Position.Absolute;card.style.top=Length.Percent(72);
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.35f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  bool shelved=false;int typed=0;
  KarineMotion.Run(veil,S.ArchiveSeconds,t=> {
   float move=Mathf.Clamp01((t-.15f)/.4f);float e=move*move*(3-2*move);
   folder.style.left=Length.Percent(Mathf.Lerp(42,74,e));folder.style.top=Length.Percent(Mathf.Lerp(30,58-26,e));
   folder.style.scale=new Scale(Vector3.one*Mathf.Lerp(1,.62f,e));
   if(move>=1 && !shelved){shelved=true;Cue("shelf");}
   int want=Mathf.FloorToInt(Mathf.Clamp01((t-.6f)/.3f)*closedText.Length);
   if(want>typed){typed=want;card.text=closedText.Substring(0,typed);Cue("key");}
  },()=>veil.schedule.Execute(finish).StartingIn(600));
 }

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
