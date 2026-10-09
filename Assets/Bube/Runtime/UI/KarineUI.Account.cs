using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using A = Bube.KarineTheme.Account;

namespace Bube {
// Giriş ekranı, 9 Ekim 2026 maketi (Docs/Reference/UI_LOGIN_2026-10.webp): tam ekran
// karanlık masa; solda KARINE logosu ve tek satır açıklama, sağda ataçlı krem form
// (Apple / Google marka düğmeleri, "veya", misafir bağlantısı, gizlilik), sağ altta
// stüdyo imzası. Kapatma düğmesi yok: misafir seçeneği zaten "geç" demektir.
//
// Marka logoları: `Bube/Art/Icons/apple` ve `Bube/Art/Icons/google` resmî PNG olarak
// konulursa çizilir (Google'ınki boyanmaz, dört renklidir). Yoksa düğme yalnız
// yazıyla çıkar — uydurma logo çizilmez.
// Arka plan: `Bube/Art/LoginBackdrop` varsa o, yoksa ofis masası.
public static partial class KarineUI {
 public readonly struct AccountOption {
  // Brand: "apple", "google" ya da null (misafir).
  public readonly string Brand, Label; public readonly Action Pick;
  public AccountOption(string brand, string label, Action pick) { Brand = brand; Label = label; Pick = pick; }
 }

 public const string LoginBackdropResource = "Bube/Art/LoginBackdrop";

 public static VisualElement AccountPaper(VisualElement parent,string[] taglines,string orLabel,IList<AccountOption> options,string notice,string privacyLabel,Action privacy) {
  var screen=new VisualElement {name="AccountPaper"};Fill(screen);screen.style.backgroundColor=KarineTheme.Background;parent?.Add(screen);
  screen.RegisterCallback<PointerDownEvent>(e=>e.StopPropagation());
  var art=Resources.Load<Texture2D>(LoginBackdropResource)??Resources.Load<Texture2D>("Bube/Art/OfficeDesk");
  Image back=null;if(art!=null){back=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};Fill(back);screen.Add(back);}
  var lamp=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=new Color(1f,.72f,.38f,A.LampAlpha)};
  lamp.style.position=Position.Absolute;lamp.style.left=Length.Percent(A.Lamp.x);lamp.style.top=Length.Percent(A.Lamp.y);lamp.style.width=Length.Percent(A.Lamp.width);lamp.style.height=Length.Percent(A.Lamp.height);screen.Add(lamp);
  var dust=new VisualElement {pickingMode=PickingMode.Ignore};Fill(dust);screen.Add(dust);
  var shade=new VisualElement {pickingMode=PickingMode.Ignore};Fill(shade);shade.style.backgroundColor=KarineTheme.Veil(.35f);screen.Add(shade);
  var vignette=new Image {image=Vignette(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(KarineTheme.Background,.9f)};Fill(vignette);screen.Add(vignette);

  // Sol: logo ve açıklama, dikeyde ortada.
  var left=new VisualElement {pickingMode=PickingMode.Ignore};left.style.position=Position.Absolute;left.style.top=0;left.style.bottom=0;
  left.style.left=Length.Percent(A.LogoLeft);left.style.width=Length.Percent(A.LogoWidth);left.style.justifyContent=Justify.Center;left.style.alignItems=Align.Center;screen.Add(left);
  var logo=KarineLogo.Hero(left,A.LogoPx);
  var line=new VisualElement {pickingMode=PickingMode.Ignore};line.style.height=3;line.style.width=Length.Percent(92);line.style.backgroundColor=KarineTheme.Danger;line.style.marginTop=KarineTheme.SpaceXs;line.style.marginBottom=KarineTheme.SpaceMd;left.Add(line);
  int maxim=UnityEngine.Random.Range(0,taglines.Length);
  var t=Write(left,taglines[maxim],KarineTheme.Primary,A.TaglineSize,Mono);t.style.unityTextAlign=TextAnchor.MiddleCenter;t.style.letterSpacing=1;

  // Sağ: ataçlı form.
  var column=new VisualElement();column.style.position=Position.Absolute;column.style.top=0;column.style.bottom=0;
  column.style.left=Length.Percent(A.PaperLeft);column.style.width=Length.Percent(A.PaperWidth);column.style.justifyContent=Justify.Center;screen.Add(column);
  var paper=new VisualElement();Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceXl*1.5f;paper.style.paddingTop=A.Clip*2.2f;paper.style.minHeight=A.MinHeight;paper.style.justifyContent=Justify.Center;paper.style.paddingBottom=KarineTheme.SpaceXl*1.5f;
  paper.style.rotate=new Rotate(A.PaperTilt);column.Add(paper);
  var clip=new VisualElement {pickingMode=PickingMode.Ignore};clip.style.position=Position.Absolute;clip.style.width=A.Clip;clip.style.height=A.Clip*2;clip.style.top=-A.Clip/2;clip.style.left=A.Clip/2;
  Stretched(clip,"Bube/UI/paperclip");paper.Add(clip);

  if(!string.IsNullOrEmpty(notice)){var n=Write(paper,notice,KarineTheme.Paper.Stamp,A.PrivacySize+2,Mono);n.style.unityTextAlign=TextAnchor.MiddleCenter;}
  AccountOption? guest=null;
  foreach(var o in options) { if(o.Brand==null){guest=o;continue;} BrandButton(paper,o); }
  if(guest!=null) {
   var or=new VisualElement {pickingMode=PickingMode.Ignore};or.style.flexDirection=FlexDirection.Row;or.style.alignItems=Align.Center;or.style.marginTop=KarineTheme.SpaceLg;or.style.marginBottom=KarineTheme.SpaceLg;paper.Add(or);
   Hair(or);var w=Write(or,orLabel,KarineTheme.Paper.Faded,A.OrSize,Mono);w.style.marginBottom=0;w.style.marginLeft=w.style.marginRight=KarineTheme.SpaceMd;Hair(or);
   var g=guest.Value;TextLink(paper,"AccountOption",g.Label,g.Pick,A.GuestInk,A.GuestSize,2);
  }
  if(privacy!=null){var p=TextLink(paper,"PrivacyLink",privacyLabel,privacy,KarineTheme.Paper.Ink,A.PrivacySize,1);p.style.marginTop=KarineTheme.SpaceXl;}

  var studio=StudioMark(screen,A.StudioHeight);studio.style.position=Position.Absolute;studio.style.right=Length.Percent(3);studio.style.bottom=Length.Percent(4);
  Enter(screen,KarineTheme.ModalMs);KarineMotion.Paper(paper);
  if(Fx.On)Animate(screen,back,lamp,dust,logo,line,t,paper,taglines,maxim);
  return screen;
 }

 // Ekran sabit durmasın: hepsi tek zamanlayıcıda; efektler kapalıyken (Fx.On yanlış) hiç kurulmaz.
 static void Animate(VisualElement screen,Image back,Image lamp,VisualElement dust,VisualElement logo,VisualElement line,Label tagline,VisualElement paper,string[] taglines,int maxim) {
  var motes=new List<(VisualElement dot,float x,float y,float speed,float phase)>();
  for(int i=0,n=Fx.Count(A.Motes);i<n;i++) {
   var dot=new VisualElement {pickingMode=PickingMode.Ignore};float size=UnityEngine.Random.Range(2f,5f);
   dot.style.position=Position.Absolute;dot.style.width=dot.style.height=size;Round(dot,3);dot.style.backgroundColor=new Color(1f,.85f,.6f,1f);dust.Add(dot);
   motes.Add((dot,UnityEngine.Random.Range(2f,40f),UnityEngine.Random.Range(5f,70f),UnityEngine.Random.Range(.6f,1.6f),UnityEngine.Random.value*6.28f));
  }
  float start=Time.realtimeSinceStartup,nextMaxim=start+A.MaximSeconds,flicker=1f,flickerTarget=1f;
  logo.style.opacity=0;line.style.width=0;tagline.style.opacity=0;
  screen.schedule.Execute(()=> {
   float now=Time.realtimeSinceStartup,age=now-start;
   float intro=Mathf.Clamp01(age/A.IntroSeconds);float ease=1-(1-intro)*(1-intro);
   logo.style.opacity=ease;logo.style.scale=new Scale(Vector3.one*(.96f+.04f*ease));
   line.style.width=Length.Percent(92*Mathf.Clamp01((age-A.IntroSeconds*.5f)/A.IntroSeconds));
   if(age<A.IntroSeconds*1.5f)tagline.style.opacity=Mathf.Clamp01((age-A.IntroSeconds)/(A.IntroSeconds*.5f));
   if(back!=null)back.style.scale=new Scale(Vector3.one*(1+A.DriftScale*(.5f-.5f*Mathf.Cos(age*6.283f/A.DriftSeconds))));
   if(UnityEngine.Random.value<.04f)flickerTarget=UnityEngine.Random.value<.12f?.55f:UnityEngine.Random.Range(.88f,1.05f);
   flicker=Mathf.Lerp(flicker,flickerTarget,.25f);lamp.tintColor=new Color(1f,.72f,.38f,A.LampAlpha*flicker);
   foreach(var m in motes) {
    float y=Mathf.Repeat(m.y-age*m.speed,75f),x=m.x+Mathf.Sin(age*.4f+m.phase)*2f;
    m.dot.style.left=Length.Percent(x);m.dot.style.top=Length.Percent(y);
    m.dot.style.opacity=(.25f+.25f*Mathf.Sin(age*1.3f+m.phase))*Mathf.Clamp01((75f-y)/15f)*Mathf.Clamp01(y/10f)*flicker;
   }
   float sway=Mathf.Sin(age*6.283f/A.SwaySeconds);
   paper.style.rotate=new Rotate(A.PaperTilt+A.SwayDegrees*sway);paper.style.translate=new Translate(0,3f*Mathf.Sin(age*6.283f/(A.SwaySeconds*1.3f)));
   if(taglines.Length>1 && now>nextMaxim) {
    nextMaxim=now+A.MaximSeconds;maxim=(maxim+1)%taglines.Length;var text=taglines[maxim];
    Run(tagline,.5f,k=>tagline.style.opacity=1-k,()=>{tagline.text=text;Run(tagline,.5f,k=>tagline.style.opacity=k,null);});
   }
  }).Every(16);
 }

 static void Fill(VisualElement e){e.style.position=Position.Absolute;e.style.left=e.style.top=e.style.right=e.style.bottom=0;}
 static void Hair(VisualElement row){var h=new VisualElement {pickingMode=PickingMode.Ignore};h.style.flexGrow=1;h.style.height=1;h.style.backgroundColor=KarineTheme.Paper.Edge;row.Add(h);}

 static void BrandButton(VisualElement paper,AccountOption o) {
  bool apple=o.Brand=="apple";
  var btn=new Button(Sounded(o.Pick)) {name="AccountOption"};Unskin(btn,apple?A.AppleFill:A.GoogleFill);
  btn.style.height=A.ButtonHeight;btn.style.flexDirection=FlexDirection.Row;btn.style.justifyContent=Justify.Center;btn.style.alignItems=Align.Center;
  btn.style.marginBottom=KarineTheme.SpaceMd;Round(btn,8);if(!apple)Border(btn,1,A.GoogleEdge);paper.Add(btn);
  var mark=Resources.Load<Texture2D>("Bube/Art/Icons/"+o.Brand);
  if(mark!=null){var img=new Image {image=mark,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore,tintColor=apple?A.AppleText:Color.white};
   img.style.width=img.style.height=A.BrandIcon;img.style.marginRight=KarineTheme.SpaceMd;btn.Add(img);}
  var label=new Label(o.Label) {pickingMode=PickingMode.Ignore};label.style.color=apple?A.AppleText:A.GoogleText;label.style.fontSize=Typography.Snap(A.ButtonSize);
  label.style.unityFontStyleAndWeight=FontStyle.Bold;ApplyFont(label,Body);btn.Add(label);
 }

 // Altı çizili yazı bağlantısı (misafir, gizlilik): çizgi alt kenarlıktır.
 static Button TextLink(VisualElement parent,string name,string text,Action pick,Color ink,int size,int underline) {
  var b=new Button(Sounded(pick)) {name=name};Unskin(b,Color.clear);b.style.alignSelf=Align.Center;b.style.minHeight=KarineTheme.TouchTarget;b.style.justifyContent=Justify.Center;parent.Add(b);
  var l=new Label(text) {pickingMode=PickingMode.Ignore};l.style.color=ink;l.style.fontSize=Typography.Snap(size);ApplyFont(l,Mono);
  l.style.borderBottomWidth=underline;l.style.borderBottomColor=ink;l.style.paddingBottom=1;b.Add(l);
  return b;
 }
}
}
