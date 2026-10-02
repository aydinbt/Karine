using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Anlatı kartları (AC): bölüm başı saat-yer kartı, okunan son satırın kenarda
// kalan soluk yankısı, kapanış sonrası sepya fotoğraf, ilk vakadan sonra jenerik.
// Yankı yalnız oyuncunun kendi okuduğunu tekrar eder; hiçbir şeyi seçmez.
public static partial class KarineUI {
 public static void ChapterCard(VisualElement root,string time,string place,Action done) {
  if(root==null || !Fx.On || string.IsNullOrEmpty(time) && string.IsNullOrEmpty(place)){done?.Invoke();return;}
  var veil=Veil(root,"ChapterCard",1f);
  var timeLabel=Technical(veil,"",KarineTheme.Office.TitleSize);timeLabel.style.color=KarineTheme.Primary;timeLabel.style.letterSpacing=6;
  var placeLabel=Technical(veil,"",KarineTheme.Office.LabelSize);placeLabel.style.color=KarineTheme.Secondary;placeLabel.style.marginTop=KarineTheme.SpaceSm;
  time=time ?? "";place=place ?? "";
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.5f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  int a=0,b=0;
  KarineMotion.Run(veil,S.ChapterSeconds,t=> {
   int wantA=Mathf.FloorToInt(Mathf.Clamp01(t/.3f)*time.Length);if(wantA>a){a=wantA;timeLabel.text=time.Substring(0,a);Cue("key");}
   int wantB=Mathf.FloorToInt(Mathf.Clamp01((t-.35f)/.4f)*place.Length);if(wantB>b){b=wantB;placeLabel.text=place.Substring(0,b);if(b%2==0)Cue("key");}
  },()=>veil.schedule.Execute(finish).StartingIn(600));
 }

 // Okunan son satırın yankısı: sayfa kapanınca kenarda soluk kalır, kaybolur.
 public static void Echo(VisualElement root,string text) {
  if(root==null || !Fx.On || string.IsNullOrEmpty(text))return;
  root.Q("ReadingEcho")?.RemoveFromHierarchy();
  if(text.Length>90)text=text.Substring(0,90).TrimEnd()+"…";
  var label=Body_(root,"“"+text+"”",KarineTheme.Office.SmallSize);label.name="ReadingEcho";label.pickingMode=PickingMode.Ignore;
  label.style.position=Position.Absolute;label.style.left=Length.Percent(2);label.style.top=Length.Percent(88);label.style.width=Length.Percent(36);
  label.style.whiteSpace=WhiteSpace.Normal;label.style.unityFontStyleAndWeight=FontStyle.Italic;label.style.color=KarineTheme.Alpha(KarineTheme.Secondary,.55f);
  KarineMotion.Run(label,S.EchoSeconds,t=>label.style.opacity=t<.8f?1:1-(t-.8f)/.2f,()=>label.RemoveFromHierarchy());
 }

 // Kapanış sonrası epilog: tek sepya kare ve bir satır. Görsel yoksa atlanır.
 public static void Epilogue(VisualElement root,Texture2D image,string line,Action done) {
  if(root==null || image==null){done?.Invoke();return;}
  var veil=Veil(root,"Epilogue",1f);
  var photo=new Image {image=image,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore,tintColor=new Color(1f,.86f,.66f,1)};
  photo.style.width=Length.Percent(60);photo.style.height=Length.Percent(60);Border(photo,8,KarineTheme.Paper.Light);veil.Add(photo);
  var label=Body_(veil,line ?? "",KarineTheme.Office.LabelSize);label.style.color=KarineTheme.Secondary;label.style.marginTop=KarineTheme.SpaceMd;
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.6f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(veil,S.EpilogueSeconds,t=>photo.style.opacity=Mathf.Clamp01(t*3),finish);
 }

 // Jenerik: satırlar aşağıdan yukarı kayar. Dokunmak geçer.
 public static void Credits(VisualElement root,string[] lines,Action done) {
  if(root==null || lines==null || lines.Length==0){done?.Invoke();return;}
  var veil=Veil(root,"Credits",1f);veil.style.overflow=Overflow.Hidden;
  var column=new VisualElement {pickingMode=PickingMode.Ignore};column.style.position=Position.Absolute;column.style.left=0;column.style.right=0;column.style.alignItems=Align.Center;veil.Add(column);
  for(int i=0;i<lines.Length;i++){var l=Technical(column,lines[i],i==0?KarineTheme.Office.TitleSize+8:KarineTheme.Office.LabelSize);l.style.color=i==0?KarineTheme.Primary:KarineTheme.Secondary;l.style.marginBottom=KarineTheme.SpaceLg;}
  bool finished=false;
  Action finish=()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.6f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(veil,S.CreditsSeconds,t=>column.style.top=Length.Percent(Mathf.Lerp(100,-60,t)),finish);
 }
}
}
