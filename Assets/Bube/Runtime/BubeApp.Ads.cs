using System;
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
 static bool LampUnlocked(int i) => i==0 || AdGateway.AdsRemoved || PlayerPrefs.GetInt(LampUnlockKey+i,0)==1;

 // Ana menüye oyundan dönülünce (ilk açılışta değil) araya giren reklam; menü beklemez.
 void MenuReturnAd() {
  if(!homeSeen){homeSeen=true;return;}
  AdGateway.Request(AdPlacement.MenuReturn,AdMoment.Menu,null);
 }

 // Ayarlar > Görüntü: lamba rengi. Kilitli renk ödüllü reklamla açılır; reklamsız
 // oyuncuya hepsi açık. Ağ hazır değilse kilitli renk yalnız görünür, açılmaz.
 void LampOptions(VisualElement control) {
  for(int i=0;i<KarineTheme.Scene.LampTints.Length;i++) {
   int v=i;bool open=LampUnlocked(v);
   var card=KarineUI.SettingCard(control,open?null:"lock",T("settings.lamp."+v),null,draftLamp==v,()=> {
    if(LampUnlocked(v)){draftLamp=v;RenderSettings();return;}
    AdGateway.Request(AdPlacement.RewardedCosmetic,AdMoment.Menu,granted=> {
     if(!granted)return;
     PlayerPrefs.SetInt(LampUnlockKey+v,1);PlayerPrefs.Save();draftLamp=v;RenderSettings();
    });
   });
   KarineUI.Quarter(card);
   if(!open && !AdGateway.MayShow(AdPlacement.RewardedCosmetic,AdMoment.Menu))card.SetEnabled(false);
  }
 }
 void LoadLampDraft()=>draftLamp=LampTint;
 void SaveLampDraft()=>PlayerPrefs.SetInt(LampTintKey,LampUnlocked(draftLamp)?draftLamp:0);

 // Uzun bekleyiş: kalan süre `AdGateway.MinSkipSeconds`dan uzunsa ödüllü atlama
 // teklif edilir. Bugünkü vakalarda bekleyişler 4–10 saniye; bu düğme görünmez.
 void SkipWait(VisualElement parent,InterviewRequest request,Action refresh) {
  double remaining=(new DateTime(request.readyAtUtcTicks,DateTimeKind.Utc)-DateTime.UtcNow).TotalSeconds;
  if(!AdGateway.MaySkipWait(remaining))return;
  KarineUI.PaperButton(parent,T("ads.skipWait"),()=>AdGateway.Request(AdPlacement.RewardedSkipWait,AdMoment.Waiting,granted=> {
   if(!granted)return;
   request.readyAtUtcTicks=DateTime.UtcNow.Ticks;Save();refresh?.Invoke();
  }));
 }
}
}
