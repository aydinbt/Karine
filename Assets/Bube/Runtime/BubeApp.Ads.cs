using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Reklam yerlerinin oyuna bağlandığı yer (kuralları `AdGateway`de): menüye dönüş,
// masa lambası rengi (ödüllü görünüm) ve uzun bekleyişi atlama. Hiçbiri
// soruşturmanın içine girmez, ödül hiçbir zaman ipucu ya da kaynak değildir.
public sealed partial class BubeApp {
 const string LampTintKey="karine.lamp.tint",LampUnlockKey="karine.lamp.unlocked.";
 bool homeSeen;
 int draftLamp;

 static int LampTint => Mathf.Clamp(PlayerPrefs.GetInt(LampTintKey,0),0,KarineTheme.Scene.LampTints.Length-1);
 bool LampUnlocked(int i) => i==0 || AdGateway.AdsRemoved || game.Career.unlockedLamps.Contains(i);

 // Ana menüye oyundan dönülünce (ilk açılışta değil) araya giren reklam; menü beklemez.
 void MenuReturnAd() {
  if(!homeSeen){homeSeen=true;return;}
  AdGateway.Request(AdPlacement.MenuReturn,AdMoment.Menu,null);
 }

 // Ayarlar > Görüntü: lamba rengi. Her kart rengini gösterir; kilitli renge basmak onu masadaki
 // lambada **önizler**. Renkler sırayla açılır: yalnız sıradaki kilitli renk reklamla açılabilir.
 // Reklamsız oyuncuya hepsi açık. Önizlenen renk açılmadan kaydedilmez.
 int NextLamp() {
  for(int i=1;i<KarineTheme.Scene.LampTints.Length;i++)if(!LampUnlocked(i))return i;
  return -1;
 }
 void LampOptions(VisualElement control) {
  for(int i=0;i<KarineTheme.Scene.LampTints.Length;i++) {
   int v=i;bool open=LampUnlocked(v);
   var card=KarineUI.SettingCard(control,open?null:"lock",T("settings.lamp."+v),open?null:T(v==NextLamp()?"settings.lamp.next":"settings.lamp.later"),draftLamp==v,()=>{draftLamp=v;PreviewLamp(v);RenderSettings();});
   KarineUI.Swatch(card,KarineTheme.Scene.LampTints[v]);
   KarineUI.Quarter(card);
  }
  if(LampUnlocked(draftLamp))return;
  int next=NextLamp();
  if(draftLamp!=next){KarineUI.Body_(control,string.Format(T("settings.lamp.order"),T("settings.lamp."+next)),KarineTheme.SettingsModal.RowHintSize).style.color=KarineTheme.Secondary;return;}
  var unlock=KarineUI.SettingRow(control,T("settings.lamp.preview"),T("settings.lamp.preview.hint"));
  if(!AdGateway.MayShow(AdPlacement.RewardedCosmetic,AdMoment.Menu))return;
  Button(unlock,T("settings.lamp.unlock"),()=>AdGateway.Request(AdPlacement.RewardedCosmetic,AdMoment.Menu,granted=> {
   if(!granted)return;
   if(!game.Career.unlockedLamps.Contains(next))game.Career.unlockedLamps.Add(next);Save();RenderSettings();
  }));
 }
 // Masadaki lambaya geçici renk; ayarlar kapanınca kayıtlı renge döner.
 void PreviewLamp(int i) {
  var light=root.Q<Image>("OfficeLight");
  if(light==null)return;
  if(!lampBase.HasValue)lampBase=light.tintColor;
  var c=i==0?lampBase.Value:KarineTheme.Alpha(KarineTheme.Scene.LampTints[i],lampBase.Value.a);
  light.tintColor=c;
 }
 Color? lampBase;
 void EndLampPreview(){if(lampBase.HasValue){PreviewLamp(LampTint);lampBase=null;}}
 void LoadLampDraft()=>draftLamp=LampTint;
 void SaveLampDraft()=>PlayerPrefs.SetInt(LampTintKey,LampUnlocked(draftLamp)?draftLamp:0);

 // Uzun bekleyiş: her ödüllü reklam kalan süreden `PriorityStepSeconds` düşer, yani 20 dakikalık
 // inceleme 2, 30 dakikalık 3 reklamdır. Yarıda bırakılan reklam boşa gitmez; kalan süre kısalmış olur.
 const double PriorityStepSeconds=600;
 void SkipWait(VisualElement parent,Func<long> ready,Action<long> setReady,Action refresh) {
  double remaining=(new DateTime(ready(),DateTimeKind.Utc)-DateTime.UtcNow).TotalSeconds;
  if(!AdGateway.MaySkipWait(remaining))return;
  int ads=(int)Math.Ceiling(remaining/PriorityStepSeconds);
  KarineUI.RequestAction(parent,"clock",string.Format(T(AdGateway.AdsRemoved?"ads.priority.free":"ads.priority"),ads),false,()=>AdGateway.Request(AdPlacement.RewardedSkipWait,AdMoment.Waiting,granted=> {
   if(!granted)return;
   setReady(Math.Max(DateTime.UtcNow.Ticks,ready()-TimeSpan.FromSeconds(PriorityStepSeconds).Ticks));Save();refresh?.Invoke();
  }));
 }
 void SkipWait(VisualElement parent,InterviewRequest request,Action refresh)=>SkipWait(parent,()=>request.readyAtUtcTicks,t=>request.readyAtUtcTicks=t,refresh);
 // İnceleme talebi ya da ret bekliyorsa öncelik teklifi.
 void PrioritySkip(VisualElement paper,Node n) {
  var request=game.State.documentRequests.FirstOrDefault(r=>r.nodeId==n.id && r.readyAtUtcTicks>DateTime.UtcNow.Ticks);
  if(request!=null){SkipWait(paper,()=>request.readyAtUtcTicks,t=>request.readyAtUtcTicks=t,()=>InvestigationRequests(false));return;}
  var denial=game.Denial(n);
  if(denial!=null && game.WarrantDenialPending(n))SkipWait(paper,()=>denial.readyAtUtcTicks,t=>denial.readyAtUtcTicks=t,()=>InvestigationRequests(false));
 }
 // Ayarlar › Oyun: tek seferlik "Reklamları kaldır" ve "Satın alımları geri yükle".
 string storeNotice;bool storeBusy;
 void StoreRows(VisualElement body) {
  ProductRow(body,Store.NoAdsProduct,"store.noads");
  ProductRow(body,Store.PriorityProduct,"store.priority");
  var restore=KarineUI.SettingRow(body,T("store.restore"),T("store.restore.hint"));
  Button(restore,T("store.restore.action"),()=>StoreCall(done=>Store.Provider.Restore(done),"store.notice.restored"));
  if(!string.IsNullOrEmpty(storeNotice))KarineUI.Body_(restore,storeNotice,KarineTheme.SettingsModal.RowHintSize).style.color=KarineTheme.Secondary;
 }
 void ProductRow(VisualElement body,string product,string key) {
  if(Store.Owned(product)){KarineUI.SettingRow(body,T(key),T(key+".owned"));return;}
  var price=Store.Provider.Price(product);
  Button(KarineUI.SettingRow(body,T(key),price==null?T(key+".hint"):T(key+".hint")+"  ·  "+price),T("store.buy"),()=>StoreCall(done=>Store.Provider.Buy(product,done),"store.notice.bought"));
 }
 void StoreCall(Action<Action<string>> call,string success) {
  if(storeBusy)return;storeBusy=true;
  call(error=>{storeBusy=false;storeNotice=T(error??success);if(root.Q("SettingsModal")!=null)RenderSettings();});
 }
}
}
