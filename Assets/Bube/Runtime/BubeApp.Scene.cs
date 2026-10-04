using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Sahne katmanının (H–O) oyuna bağlandığı yer: masadaki eşyadan açılan geçiş,
// terminal bağlantısı, dosya kapandı kartı, güven rozeti, şekil işaretleri,
// ayarlardaki yeni seçenekler ve geliştirici kare sayacı. `BubeApp`in parçası.
public sealed partial class BubeApp {
 const string AshKey="karine.ashSmoke",ShapesKey="karine.shapes",MeterKey="karine.devMeter",SeenRankKey="karine.seenRank";
 PanelSettings panel;
 VisualElement deskStage,pressedScreen,meter;
 string pendingProp;
 bool coverOpened,dialed,typeTitle;
 float meterFrames,meterTime,pendingAt;

 static bool Shapes => PlayerPrefs.GetInt(ShapesKey,1)==1;
 static bool DevMeterAllowed => Debug.isDebugBuild || Application.isEditor;

 // Her karede: dokunulan eşyanın ekranı kurulduysa geçişi oynat; kare sayacını yaz.
 void SceneTick() {
  if(pendingProp!=null && Time.unscaledTime-pendingAt>1.5f){pendingProp=null;pressedScreen=null;}
  if(pendingProp!=null) {
   if(pressedScreen==null)pressedScreen=deskStage;
   else if(deskStage!=pressedScreen || pressedScreen.panel==null) {
    string prop=pendingProp;pendingProp=null;pressedScreen=null;
    if(prop!="Phone")SceneVeil();
   }
  }
  DevMeter();
  StageTick();
 }

 // Terminal oturumda ilk açıldığında hat bağlanır ve başlık tuş tuş yazılır.
 void Dial() {
  if(dialed)return;dialed=true;typeTitle=true;
  root.schedule.Execute(()=>KarineUI.Connecting(root,T("terminal.connecting"))).StartingIn(0);
 }
 void TerminalTitle(Label title) {
  if(!typeTitle)return;typeTitle=false;
  title.schedule.Execute(()=>{KarineUI.TerminalType(title,title.text);KarineUI.Persist(title);}).StartingIn(260);
 }

 // Onay faksı gelince, her vakada: klasör kapanır, "KAPANDI" damgası iner. Vaka başına bir kez.
 void ClosedCard(string caseId,Action then) {
  string key="karine.closedCard."+caseId;
  if(PlayerPrefs.GetInt(key,0)==1){then();return;}
  PlayerPrefs.SetInt(key,1);PlayerPrefs.Save();
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  // Ayrılan dosya (#008): damga "AYRILDI", altında ayrı soruşturma notu.
  bool split=data!=null && !string.IsNullOrEmpty(data.closedStampKey);
  KarineUI.CaseClosed(root,data==null?caseId:T(data.titleKey),T(split?data.closedStampKey:"case.closedStamp"),then,
   split && !string.IsNullOrEmpty(data.closedNoteKey)?T(data.closedNoteKey):null);
 }

 // Kapanış sonrası (epilog, jenerik) vaka başına bir kez; sonra `again` kaldığı yerden sürer.
 bool ShowCaseClosed(Action again) {
  if(!game.State.closed)return false;
  string key="karine.caseClosed."+game.Data.id;
  if(PlayerPrefs.GetInt(key,0)==1)return false;
  PlayerPrefs.SetInt(key,1);PlayerPrefs.Save();
  audioDirector?.Sting();
  AfterClosing(again);
  return true;
 }

 // Güven kutusu parlar; durum son görülenden farklıysa mühür gibi iner.
 void TrustBadge(VisualElement badge) {
  KarineUI.BadgeShine(badge);
  string seen=PlayerPrefs.GetString(SeenRankKey,string.Empty),now=game.TrustStatusKey;
  if(seen==now)return;
  PlayerPrefs.SetString(SeenRankKey,now);PlayerPrefs.Save();
  if(seen.Length>0)badge.schedule.Execute(()=>KarineUI.RankCeremony(root,T(now),()=>KarineUI.RankStamp(badge))).StartingIn(300);
 }

 // Renk körü için şekil: rengin söylediğini bir şekil de söyler. Yalnız
 // oyuncunun kendi seçtiği işarete ve faksın açık sonucuna eklenir.
 static string Shape(string mark) {
  if(!Shapes)return string.Empty;
  switch(mark){case "conflict":return "▲ ";case "agree":return "● ";default:return "■ ";}
 }
 string Verdict(bool supported)=>(Shapes?(supported?"● ":"▲ "):string.Empty)+T(supported?"fax.supported":"fax.unsupported");

 // Kare dizisinde ve videoda bir kare geri.
 void StepCctvBack() {
  if(cctvFrames!=null){PlayCctvFrames(false);cctvReachedEnd=false;ShowCctvFrame(cctvFrameIndex-1);return;}
  if(cctvPlayer==null || !cctvPlayer.isPrepared || !cctvPlayer.canSetTime)return;
  if(cctvPlayer.isPlaying)cctvPlayer.Pause();
  cctvPlaybackButton.text=T("cctv.videoPlay");cctvReachedEnd=false;
  cctvPlayer.frame=Math.Max(0L,cctvPlayer.frame-1L);
 }

 // Geliştirici kare sayacı: yalnız geliştirme derlemesinde ve açıkken.
 void DevMeter() {
  bool on=DevMeterAllowed && PlayerPrefs.GetInt(MeterKey,0)==1;
  if(!on){if(meter!=null){meter.RemoveFromHierarchy();meter=null;}return;}
  if(meter==null) {
   meter=KarineUI.Technical(new VisualElement(),"-- fps",KarineTheme.Office.SmallSize);
   meter.RemoveFromHierarchy();meter.pickingMode=PickingMode.Ignore;
   meter.style.position=Position.Absolute;meter.style.right=6;meter.style.top=4;
   meter.style.color=KarineTheme.Film.Phosphor;meter.style.backgroundColor=KarineTheme.GlassDeep;
  }
  if(meter.parent!=root || root.IndexOf(meter)!=root.childCount-1)root.Add(meter);
  meterFrames++;meterTime+=Time.unscaledDeltaTime;
  if(meterTime*1000>=KarineTheme.Scene.MeterMs) {
   ((Label)meter).text=Mathf.RoundToInt(meterFrames/meterTime)+" fps  "+Mathf.RoundToInt(meterTime/meterFrames*1000)+" ms"+(Fx.Budgeted?"  bütçe:hafif":Fx.Degraded?"  hafif":"");
   meterFrames=0;meterTime=0;
  }
 }

 // Ayarlar: okuma (yazı boyu, şekil işaretleri) ve oynanış (CRT, kül, sayaç).
 void LoadSceneDraft() {
  draftScaleValue=Typography.Scale;
  LoadAccessDraft();LoadPolishDraft();
  draftShapes=Shapes;draftCrt=CrtPass.Enabled;draftAsh=KarineUI.AshSmoke;draftMeter=PlayerPrefs.GetInt(MeterKey,0)==1;
 }
 void ResetSceneDraft(){ResetAccessDraft();ResetPolishDraft();draftScaleValue=1f;draftShapes=true;draftCrt=true;draftAsh=true;draftMeter=false;}
 void SaveSceneDraft() {
  SaveAccessDraft();
  Typography.Set(draftScaleValue);
  PlayerPrefs.SetInt(ShapesKey,draftShapes?1:0);PlayerPrefs.SetInt(AshKey,draftAsh?1:0);KarineUI.AshSmoke=draftAsh;
  PlayerPrefs.SetInt(MeterKey,draftMeter?1:0);
  PlayerPrefs.SetInt(CrtPass.Key,draftCrt?1:0);SavePolishDraft();GetComponent<CrtPass>()?.Apply(draftCrt);
 }
}
}
