using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace Bube {
// AdMob gerçeklemesi. Ne zaman gösterileceğine `AdGateway` karar verir; burası
// yalnız yükler ve gösterir. Yayın derlemesi gerçek birimleri (platforma göre) kullanır;
// geliştirme derlemesi ve Editor Google'ın test birimlerini: kendi gerçek reklamına
// tıklamak AdMob hesabını kapattırabilir. Uygulama kimlikleri GoogleMobileAdsSettings'tedir.
public sealed class AdMobProvider : IAdProvider {
 const string TestInterstitial="ca-app-pub-3940256099942544/1033173712",TestRewarded="ca-app-pub-3940256099942544/5224354917";
#if UNITY_IOS
 const string LiveInterstitial="ca-app-pub-7630097524526316/1817857936",LiveRewarded="ca-app-pub-7630097524526316/6718830822";
#else
 const string LiveInterstitial="ca-app-pub-7630097524526316/2430075333",LiveRewarded="ca-app-pub-7630097524526316/8031912492";
#endif
 static string InterstitialId=>Debug.isDebugBuild?TestInterstitial:LiveInterstitial;
 static string RewardedId=>Debug.isDebugBuild?TestRewarded:LiveRewarded;

 InterstitialAd interstitial;
 RewardedAd rewarded;
 bool started;

 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
 static void Install() {
  if(!Application.isMobilePlatform)return;
  var provider=new AdMobProvider();
  AdGateway.Provider=provider;
  provider.AskGoogleConsent();
 }

 // AB/İngiltere için Google'ın izin formu (UMP). Gerekmiyorsa hiçbir şey göstermez.
 void AskGoogleConsent() {
  // iOS'ta önce Apple'ın takip izni (ATT), sonra Google'ın bölgesel izin formu.
  Platform.RequestTracking(()=>ConsentInformation.Update(new ConsentRequestParameters(),updateError=> {
   if(updateError!=null){Start();return;}
   ConsentForm.LoadAndShowConsentFormIfRequired(_=>Start());
  }));
 }

 void Start() {
  if(started || !ConsentInformation.CanRequestAds())return;
  started=true;
  MobileAds.Initialize(_=>{LoadInterstitial();LoadRewarded();});
 }

 void LoadInterstitial() {
  interstitial?.Destroy();interstitial=null;
  InterstitialAd.Load(InterstitialId,new AdRequest(),(ad,error)=>{if(error==null)interstitial=ad;});
 }

 void LoadRewarded() {
  rewarded?.Destroy();rewarded=null;
  RewardedAd.Load(RewardedId,new AdRequest(),(ad,error)=>{if(error==null)rewarded=ad;});
 }

 static bool IsRewarded(AdPlacement p) => p==AdPlacement.RewardedRetry || p==AdPlacement.RewardedGuidance
  || p==AdPlacement.RewardedCosmetic || p==AdPlacement.RewardedSkipWait;

 public bool Ready(AdPlacement placement) => IsRewarded(placement)
  ? rewarded!=null && rewarded.CanShowAd()
  : interstitial!=null && interstitial.CanShowAd();

 // Reklam olayları ana iş parçacığında gelmeyebilir; sonucu bir sonraki karede veririz.
 public void Show(AdPlacement placement,Action<bool> finished) {
  if(!Ready(placement)){finished?.Invoke(false);return;}
  if(IsRewarded(placement)) {
   var ad=rewarded;bool earned=false;
   ad.OnAdFullScreenContentClosed+=()=>MainThread.Post(()=>{finished?.Invoke(earned);LoadRewarded();});
   ad.OnAdFullScreenContentFailed+=_=>MainThread.Post(()=>{finished?.Invoke(false);LoadRewarded();});
   ad.Show(_=>earned=true);
  } else {
   var ad=interstitial;
   ad.OnAdFullScreenContentClosed+=()=>MainThread.Post(()=>{finished?.Invoke(false);LoadInterstitial();});
   ad.OnAdFullScreenContentFailed+=_=>MainThread.Post(()=>{finished?.Invoke(false);LoadInterstitial();});
   ad.Show();
  }
 }
}

// Yan iş parçacığından gelen geri çağrıları ana döngüye taşır.
sealed class MainThread : MonoBehaviour {
 static MainThread instance;
 static readonly System.Collections.Generic.Queue<Action> queue=new();
 public static void Post(Action a) {
  lock(queue)queue.Enqueue(a);
 }
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
 static void Create() {
  if(instance!=null)return;
  var go=new GameObject("AdMainThread");DontDestroyOnLoad(go);go.hideFlags=HideFlags.HideAndDontSave;
  instance=go.AddComponent<MainThread>();
 }
 void Update() {
  while(true){Action a;lock(queue){if(queue.Count==0)return;a=queue.Dequeue();}a?.Invoke();}
 }
}
}
