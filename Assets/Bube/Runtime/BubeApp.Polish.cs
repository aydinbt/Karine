using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Cila katmanının (Y–AF) oyuna bağlandığı yer: dokunuş hissi, okurken sesin
// kısılması, tek elle erişim, renk körlüğü ve yazı boyu ayarları, masadaki yaşayan
// eşyalar, bölüm kartı, yazıcı, yankı, epilog ve jenerik; geliştirici için efekt
// kaydı ve kare süresi profili. `BubeApp`in parçasıdır.
public sealed partial class BubeApp {
 const string CreditsKey="karine.credits";const int ReviewAfterFiles=3;
 int draftColor;
 bool reachDown;
 string echoNode;
 readonly HashSet<string> printed=new HashSet<string>();

 void PolishTick() {
  AdGateway.Seasoned=game.Career.reviewHistory.Count>0;
  KarineUI.TouchFeel(root);
  audioDirector?.Duck(root.Q("DossierPaper")!=null);
  Reach();
 }

 // Tek elle erişim: tüm ekran bir dokunuşla aşağı kayar, üstteki düğmeler başparmağa iner.
 void Reach() {
  var button=root.Q<Button>("ReachButton");
  if(!OneHand){button?.RemoveFromHierarchy();if(reachDown){reachDown=false;root.style.translate=StyleKeyword.Null;}return;}
  if(button!=null){if(root.IndexOf(button)!=root.childCount-1)root.Add(button);return;}
  button=new Button(KarineUI.Sounded(()=> {
   reachDown=!reachDown;
   KarineMotion.Run(root,.25f,t=>root.style.translate=new Translate(0,Length.Percent((reachDown?t:1-t)*KarineTheme.Scene.ReachPercent)));
  })) {name="ReachButton",text="⇕"};
  KarineUI.Paint(button,KarineButtonKind.Secondary,true);
  button.style.position=Position.Absolute;button.style.left=Length.Percent(2);button.style.bottom=Length.Percent(3);
  button.style.minHeight=KarineTheme.TouchTarget;button.style.minWidth=KarineTheme.TouchTarget;root.Add(button);
 }

 // Ayarlar: renk körlüğü düzeltmesi ve kaydırıcıyla yazı boyu.
 void LoadPolishDraft(){draftColor=CrtPass.ColorFilter;LoadLampDraft();}
 void ResetPolishDraft(){draftColor=0;draftLamp=0;}
 void SavePolishDraft(){PlayerPrefs.SetInt(CrtPass.ColorKey,draftColor);SaveLampDraft();}

 // Masa: lamba, kablo, şehir pencereleri, takvim (yeri hatırlanır).
 void LampTintOnly(VisualElement stage) {
  var light=stage.Q<Image>("OfficeLight");
  if(light!=null && LampTint>0)light.tintColor=KarineTheme.Alpha(KarineTheme.Scene.LampTints[LampTint],light.tintColor.a);
 }
 void DeskPolish(VisualElement stage) {
  KarineUI.LampSwitch(stage);KarineUI.PhoneCord(stage);KarineUI.CityWindows(stage);
  var light=stage.Q<Image>("OfficeLight");
  if(light!=null && LampTint>0)light.tintColor=KarineTheme.Alpha(KarineTheme.Scene.LampTints[LampTint],light.tintColor.a);
  string tearKey="karine.calendar."+game.Data.id;bool tear=game.State.caseAccepted && PlayerPrefs.GetInt(tearKey,0)==0;
  if(tear)PlayerPrefs.SetInt(tearKey,1);
  KarineUI.Calendar(stage,T(game.Data.titleKey).Split(new[]{'—'},2)[0].Trim(),tear);
  KarineUI.Keepable(stage.Q("DeskCalendar"),"calendar");
 }

 // Bölüm kartı: vaka saati ve (verisi varsa) yer.
 // Açılış kartı saatle birlikte vakanın tarihini de yazar: "21 Kasım 2026 · 23:00".
 string ChapterTime()=>game.Data.deskHour<0?null:(string.IsNullOrEmpty(game.Data.deskDate)?"":game.Data.deskDate+"  ·  ")+game.Data.deskHour.ToString("00")+":00";
 string ChapterPlace()=>string.IsNullOrEmpty(game.Data.openingPlaceKey)?null:T(game.Data.openingPlaceKey);

 // Kayıt masaya bırakılırken türünün sesi.
 void DropFor(string sourceId) {
  if(string.IsNullOrEmpty(sourceId))return;
  string id=sourceId.Split(':','|','#')[0];
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==id);
  KarineUI.DropSound(source?.kind,source!=null && !string.IsNullOrEmpty(source.imageResource));
 }

 // Dosyada başka sayfaya geçilince önceki sayfanın ilk satırı kenarda bir süre kalır.
 void EchoLeaving(Node current) {
  string id=current?.id;
  if(echoNode!=null && echoNode!=id) {
   var previous=game.Data.nodes.FirstOrDefault(n=>n.id==echoNode);
   if(previous!=null && !string.IsNullOrEmpty(previous.bodyKey))KarineUI.Echo(root,T(previous.bodyKey).Split('\n')[0]);
  }
  echoNode=id;
 }

 // Terminal kaydı ilk açılışında yazıcıdan çıkar (oturum başına bir kez).
 void PrintOnce(Node node) {
  if(node==null || !printed.Add(node.id))return;
  var lines=new[]{T(node.titleKey)}.Concat(T(node.bodyKey).Split('\n').Where(l=>l.Trim().Length>0).Take(3)).ToArray();
  root.schedule.Execute(()=>KarineUI.Printout(root,lines,null)).StartingIn(0);
 }

 // Kapanış kartından sonra: epilog (varsa), ilk kapanan vakadan sonra bir kez jenerik.
 void AfterClosing(Action again) {
  var image=string.IsNullOrEmpty(game.Data.epilogueImage)?null:Resources.Load<Texture2D>(game.Data.epilogueImage);
  KarineUI.Epilogue(root,image,string.IsNullOrEmpty(game.Data.epilogueKey)?null:T(game.Data.epilogueKey),()=> {
   // Üçüncü dosya kapanınca bir kez mağaza puanı istenir; pencereyi işletim sistemi gösterir.
   if(!game.Career.reviewAsked && game.Career.reviewHistory.Count>=ReviewAfterFiles){game.Career.reviewAsked=true;Save();Platform.RequestReview();}
   if(game.Career.creditsSeen){again?.Invoke();return;}
   game.Career.creditsSeen=true;Save();
   KarineUI.Credits(root,new[]{T("credits.0"),T("credits.1"),T("credits.2")},again);
  });
 }
}
}
