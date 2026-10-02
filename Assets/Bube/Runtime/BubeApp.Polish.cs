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
 const string CreditsKey="karine.credits";
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
 void ColorOptions(VisualElement scroll) {
  // Alt alta: dört kart yan yana sığmıyor. Açıklama yalnız ilkinde, tekrar etmez.
  for(int i=0;i<4;i++){int v=i;var o=KarineUI.SettingsOption(scroll,T("settings.color."+v),v==0?T("settings.color.hint"):null,draftColor==v,()=>{draftColor=v;RenderSettings();});o.style.flexGrow=0;o.style.flexBasis=StyleKeyword.Auto;o.style.marginBottom=KarineTheme.SpaceSm;}
 }
 void TextScaleSlider(VisualElement scroll) {
  var slider=new Slider(Typography.MinScale,Typography.MaxScale) {name="TextScaleSlider",value=draftScaleValue};
  slider.style.marginTop=KarineTheme.SpaceSm;slider.style.marginBottom=KarineTheme.SpaceMd;
  var readout=KarineUI.Technical(scroll,Mathf.RoundToInt(draftScaleValue*100)+"%",KarineTheme.Office.SmallSize);readout.style.color=KarineTheme.Secondary;
  slider.RegisterValueChangedCallback(e=>{draftScaleValue=Mathf.Round(e.newValue*20)/20f;readout.text=Mathf.RoundToInt(draftScaleValue*100)+"%";});
  scroll.Add(slider);
 }

 // Masa: lamba, kablo, şehir pencereleri, takvim (yeri hatırlanır).
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
 string ChapterTime()=>game.Data.deskHour<0?null:game.Data.deskHour.ToString("00")+":00";
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
   if(PlayerPrefs.GetInt(CreditsKey,0)==1){again?.Invoke();return;}
   PlayerPrefs.SetInt(CreditsKey,1);PlayerPrefs.Save();
   KarineUI.Credits(root,new[]{T("credits.0"),T("credits.1"),T("credits.2")},again);
  });
 }

 // Geliştirici: yeni efektlerin denemesi, efekt kaydı ve kare süresi profili.
 IEnumerable<(string label,Action run)> PolishTests(VisualElement stage) {
  yield return ("Bölüm",()=>KarineUI.ChapterCard(root,"23:00",T(game.Data.titleKey),null));
  yield return ("Yazıcı",()=>KarineUI.Printout(root,new[]{"KAYIT 0001","—","Deneme çıktısı"},null));
  yield return ("Bant",()=>KarineUI.TapeInsert(root));
  yield return ("Karışma",()=>{var v=KarineUI.Shuffle(root);root.schedule.Execute(()=>v?.RemoveFromHierarchy()).StartingIn(2400);});
  yield return ("Yankı",()=>KarineUI.Echo(root,T(game.Data.titleKey)));
  yield return ("Jenerik",()=>KarineUI.Credits(root,new[]{T("credits.0"),T("credits.1"),T("credits.2")},null));
  yield return ("Kaydet",()=>StartCoroutine(RecordFx()));
  yield return ("Profil",()=>StartCoroutine(Profile(stage)));
 }

 // Efekt kaydı: 2,4 saniye boyunca saniyede 15 kare PNG; mağaza görselleri için.
 System.Collections.IEnumerator RecordFx() {
  string folder=Path.Combine(Application.persistentDataPath,"fxrec",DateTime.Now.ToString("yyyyMMdd-HHmmss"));
  Directory.CreateDirectory(folder);
  for(int i=0;i<36;i++) {
   yield return new WaitForEndOfFrame();
   ScreenCapture.CaptureScreenshot(Path.Combine(folder,i.ToString("000")+".png"));
   yield return new WaitForSecondsRealtime(1f/15f);
  }
  Debug.Log("Efekt kaydı: "+folder);
 }

 // Kare süresi profili: 60 saniye, ortalama / %95 / en kötü ve CSV.
 System.Collections.IEnumerator Profile(VisualElement stage) {
  var label=KarineUI.Technical(stage,"profil: 60 s…",KarineTheme.Office.SmallSize);label.style.color=KarineTheme.Film.Phosphor;
  var samples=new List<float>();float until=Time.realtimeSinceStartup+60f;
  while(Time.realtimeSinceStartup<until){samples.Add(Time.unscaledDeltaTime*1000f);yield return null;}
  samples.Sort();
  float average=samples.Average(),p95=samples[Mathf.Min(samples.Count-1,(int)(samples.Count*.95f))],worst=samples[samples.Count-1];
  string summary=$"kare {samples.Count}  ort {average:0.0} ms  %95 {p95:0.0} ms  en kötü {worst:0.0} ms  hedef {1000f/Mathf.Max(1,Application.targetFrameRate):0.0} ms";
  string path=Path.Combine(Application.persistentDataPath,"perf_"+DateTime.Now.ToString("yyyyMMdd-HHmmss")+".csv");
  File.WriteAllText(path,"ms\n"+string.Join("\n",samples.Select(s=>s.ToString("0.00",System.Globalization.CultureInfo.InvariantCulture))));
  label.text=summary;Debug.Log(summary+"  → "+path);
 }
}
}
