using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Bölüm finali (#010): kararan ekran ve telefon, personel değerlendirme belgesi,
// "ÜLKE — 10 / 10" ve "TAMAMLANDI", gelen evraka düşen yeni ülkenin ilk dosyası.
// Yıldız, puan ya da başarı yüzdesi yok; yalnız resmî belge ve sessizlik.
// Her parça dokununca geçer; Fx kapalıyken doğrudan `done`.
public static partial class KarineUI {
 static Action Finisher(VisualElement veil,Action done) {
  bool finished=false;
  return ()=>{if(finished)return;finished=true;KarineMotion.Run(veil,.45f,t=>veil.style.opacity=1-t,()=>{veil.RemoveFromHierarchy();done?.Invoke();});};
 }
 static Label FinaleLine(VisualElement parent,string text,int size,Color color) {
  var l=Technical(parent,text,size);l.style.color=color;l.style.whiteSpace=WhiteSpace.Normal;
  l.style.unityTextAlign=TextAnchor.MiddleCenter;l.style.maxWidth=S.FinaleSheetWidth;l.style.opacity=0;return l;
 }

 // Ekran kararır, telefon çalar, kısa konuşma satır satır belirir.
 public static void FinaleCall(VisualElement root,string heading,string[] lines,Action done) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"FinaleCallVeil",1);var finish=Finisher(veil,done);
  var head=FinaleLine(veil,heading,S.FinaleLineSize,KarineTheme.Paper.Faded);head.style.marginBottom=KarineTheme.SpaceLg*2;
  var labels=new Label[lines.Length];
  for(int i=0;i<lines.Length;i++){labels[i]=FinaleLine(veil,lines[i],S.FinaleLineSize,KarineTheme.Paper.Light);labels[i].style.marginBottom=KarineTheme.SpaceMd;}
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(head,.8f,t=>head.style.opacity=t);
  veil.schedule.Execute(()=>{Sound?.Invoke("amb_phone");Fx.Buzz(Haptic.Thud);}).StartingIn(1200);
  for(int i=0;i<labels.Length;i++){var l=labels[i];veil.schedule.Execute(()=>KarineMotion.Run(l,.4f,t=>l.style.opacity=t)).StartingIn(2600+i*S.FinaleLineMs);}
  veil.schedule.Execute(()=>{foreach(var l in labels)l.style.opacity=0;head.style.opacity=0;}).StartingIn(2600+lines.Length*S.FinaleLineMs+600);
  veil.schedule.Execute(finish).StartingIn(2600+lines.Length*S.FinaleLineMs+1600);
 }

 // Personel dosyası: başlık, dosya kayıtları (ad · durum), değerlendirme metni ve yeni görevlendirme.
 public static void PersonnelReview(VisualElement root,string title,(string,string)[] rows,string body,string program,string country,string closeLabel,Action done) {
  if(root==null){done?.Invoke();return;}
  var veil=Veil(root,"PersonnelVeil",.94f);var finish=Finisher(veil,done);
  var sheet=new VisualElement {name="PersonnelSheet"};sheet.style.width=S.FinaleSheetWidth;sheet.style.maxWidth=Length.Percent(92);sheet.style.maxHeight=Length.Percent(94);
  sheet.style.backgroundColor=KarineTheme.Paper.Sheet;Border(sheet,2,KarineTheme.Paper.Edge);
  sheet.style.paddingLeft=sheet.style.paddingRight=sheet.style.paddingTop=sheet.style.paddingBottom=KarineTheme.SpaceLg;veil.Add(sheet);
  var scroll=new KarineScrollView(ScrollViewMode.Vertical);scroll.style.flexGrow=1;scroll.style.flexShrink=1;sheet.Add(scroll);
  var t=Technical(scroll,title,KarineTheme.Office.LabelSize+4);t.style.color=KarineTheme.Paper.Ink;t.style.letterSpacing=3;t.style.marginBottom=KarineTheme.SpaceMd;
  foreach(var (name,state) in rows) {
   var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Paper.Edge;
   row.style.paddingTop=row.style.paddingBottom=KarineTheme.SpaceXs;scroll.Add(row);
   var n=Technical(row,name,KarineTheme.Office.LabelSize);n.style.color=KarineTheme.Paper.Ink;n.style.flexGrow=1;n.style.flexShrink=1;n.style.whiteSpace=WhiteSpace.Normal;n.style.marginBottom=0;
   var s=Technical(row,state,KarineTheme.Office.LabelSize);s.style.color=KarineTheme.Paper.Faded;s.style.marginBottom=0;
  }
  var b=Technical(scroll,body,KarineTheme.Office.LabelSize);b.style.color=KarineTheme.Paper.Ink;b.style.whiteSpace=WhiteSpace.Normal;b.style.marginTop=KarineTheme.SpaceLg;
  var p=Technical(scroll,program,KarineTheme.Office.LabelSize+2);p.style.color=KarineTheme.Paper.Stamp;p.style.marginTop=KarineTheme.SpaceLg;p.style.letterSpacing=3;
  Border(p,3,KarineTheme.Paper.Stamp);Round(p,4);p.style.alignSelf=Align.Center;p.style.paddingLeft=p.style.paddingRight=KarineTheme.SpaceMd;p.style.rotate=new Rotate(-4);p.style.opacity=0;
  var c=Technical(scroll,country,S.FinaleTitleSize);c.style.color=KarineTheme.Paper.Ink;c.style.alignSelf=Align.Center;c.style.marginTop=KarineTheme.SpaceMd;c.style.opacity=0;
  var close=PaperButton(sheet,closeLabel,finish,KarinePaperKind.Action);close.style.marginTop=KarineTheme.SpaceMd;close.style.minHeight=KarineTheme.TouchTarget;
  Cue("paper");
  KarineMotion.Run(sheet,.5f,x=>sheet.style.translate=new Translate(0,Mathf.Lerp(380,0,Smooth(x))));
  sheet.schedule.Execute(()=>KarineMotion.Run(p,.25f,x=>{p.style.opacity=x;p.style.scale=new Scale(Vector3.one*Mathf.Lerp(2,1,x*x));},()=>{Sound?.Invoke("ui_stamp");Fx.Buzz(Haptic.Thud);})).StartingIn(1600);
  sheet.schedule.Execute(()=>KarineMotion.Run(c,.6f,x=>c.style.opacity=x)).StartingIn(2200);
 }

 // "TÜRKİYE — 10 / 10", sonra "TÜRKİYE TAMAMLANDI".
 public static void ChapterComplete(VisualElement root,string first,string second,Action done) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"ChapterCompleteVeil",1);var finish=Finisher(veil,done);
  var a=FinaleLine(veil,first,S.FinaleTitleSize,KarineTheme.Paper.Light);a.style.letterSpacing=6;
  var b=FinaleLine(veil,second,S.FinaleLineSize,KarineTheme.Paper.Faded);b.style.marginTop=KarineTheme.SpaceLg;b.style.letterSpacing=4;
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  KarineMotion.Run(a,1.2f,t=>a.style.opacity=Smooth(t));
  veil.schedule.Execute(()=>KarineMotion.Run(b,1f,t=>b.style.opacity=Smooth(t))).StartingIn(2200);
  veil.schedule.Execute(finish).StartingIn(5600);
 }

 // Gelen evrak tepsisine yeni dosya düşer: mekanik ses, mühürlü klasör.
 public static void NewFileDrop(VisualElement root,string file,string country,string stampText,Action done) {
  if(root==null || !Fx.On){done?.Invoke();return;}
  var veil=Veil(root,"NewFileVeil",.9f);var finish=Finisher(veil,done);
  var folder=new VisualElement {pickingMode=PickingMode.Ignore};folder.style.width=S.ClosedFolderWidth;folder.style.height=S.ClosedFolderHeight*.7f;
  folder.style.backgroundColor=KarineTheme.Paper.Folder;Stretched(folder,"Bube/UI/paper_folder");Border(folder,2,KarineTheme.Paper.Edge);
  folder.style.justifyContent=Justify.Center;folder.style.alignItems=Align.Center;veil.Add(folder);
  var f=Technical(folder,file,S.FinaleTitleSize);f.style.color=KarineTheme.Paper.Ink;f.style.letterSpacing=4;
  var c=Technical(folder,country,KarineTheme.Office.LabelSize+2);c.style.color=KarineTheme.Paper.Ink;
  var stamp=Technical(folder,stampText,KarineTheme.Office.LabelSize);stamp.style.color=KarineTheme.Paper.Stamp;stamp.style.position=Position.Absolute;
  stamp.style.bottom=Length.Percent(10);Border(stamp,3,KarineTheme.Paper.Stamp);Round(stamp,4);stamp.style.letterSpacing=3;
  stamp.style.paddingLeft=stamp.style.paddingRight=KarineTheme.SpaceMd;stamp.style.rotate=new Rotate(-6);stamp.style.opacity=0;
  veil.RegisterCallback<PointerDownEvent>(_=>finish());
  Cue("drawer");
  bool stamped=false;
  KarineMotion.Run(folder,1.4f,t=> {
   float d=Smooth(t/.3f);folder.style.translate=new Translate(0,Mathf.Lerp(-520,0,d));folder.style.rotate=new Rotate(Mathf.Lerp(9,-3,d));
   float s=Mathf.Clamp01((t-.55f)/.12f);stamp.style.opacity=s;stamp.style.scale=new Scale(Vector3.one*Mathf.Lerp(2.2f,1,s*s));
   if(s>=1 && !stamped){stamped=true;Sound?.Invoke("ui_stamp");Fx.Buzz(Haptic.Thud);}
  },()=>veil.schedule.Execute(finish).StartingIn(3200));
 }
}
}
