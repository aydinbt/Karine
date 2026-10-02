using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Reklamın ağı henüz yok; kuralları var. Asıl risk ağ değil, reklamın **yanlış
// ana** düşmesi: soruşturmanın ortasında, sorgunun içinde, filmin üstünde.
public sealed class MonetizationTests {

 sealed class AlwaysReady : IAdProvider {
  public int Shown;
  public bool Ready(AdPlacement placement) => true;
  public void Show(AdPlacement placement, System.Action<bool> finished) { Shown++; finished?.Invoke(true); }
 }

 [SetUp] public void Setup() {
  PlayerPrefs.DeleteKey(AdGateway.ConsentKey);
  PlayerPrefs.DeleteKey(AdGateway.NoAdsKey);
  AdGateway.Load();
  AdGateway.Provider = new NoAdProvider();
  AdGateway.Seasoned = false;
  AdGateway.ResetInterstitialClock();
  AdGateway.Now = () => clock;
  clock = 1000f;
 }
 float clock;

 [TearDown] public void TearDown() {
  PlayerPrefs.DeleteKey(AdGateway.ConsentKey);
  PlayerPrefs.DeleteKey(AdGateway.NoAdsKey);
  AdGateway.Load();
  AdGateway.Provider = new NoAdProvider();
  AdGateway.Seasoned = false;
  AdGateway.ResetInterstitialClock();
  AdGateway.Now = () => Time.realtimeSinceStartup;
 }

 // Onay alınmadan hiçbir reklam gösterilmez. Varsayılan "sorulmadı"dır ve
 // sorulmamış olmak izin değildir.
 [Test]
 public void WithoutConsent_NothingIsShown() {
  Assert.AreEqual(AdConsent.Unknown, AdGateway.Consent);
  foreach (AdPlacement placement in System.Enum.GetValues(typeof(AdPlacement)))
  foreach (AdMoment moment in System.Enum.GetValues(typeof(AdMoment)))
   Assert.IsFalse(AdGateway.MayShow(placement, moment),
    "Onay yokken reklam gösterilemez: " + placement + " / " + moment);

  AdGateway.SetConsent(AdConsent.Denied);
  Assert.IsFalse(AdGateway.MayShow(AdPlacement.CaseInterval, AdMoment.CaseClosed),
   "Onay reddedildiğinde de gösterilemez.");
 }

 // Kanonun kilidi: oyuncunun düşündüğü an kesilmez. Araya giren reklam yalnız
 // vaka kapandıktan sonraki değerlendirme anında olur.
 [Test]
 public void Interstitial_OnlyAfterTheCaseIsClosed() {
  AdGateway.SetConsent(AdConsent.Granted);
  Assert.IsTrue(AdGateway.MayShow(AdPlacement.CaseInterval, AdMoment.CaseClosed));
  foreach (var moment in new[] { AdMoment.Investigation, AdMoment.Interview, AdMoment.Cctv, AdMoment.Cinematic, AdMoment.Menu })
   Assert.IsFalse(AdGateway.MayShow(AdPlacement.CaseInterval, moment),
    "Soruşturmanın içi kapalı olmalı: " + moment);
 }

 // Ödüllü reklam oyuncunun kendi istediği anda olur: raporu geri döndükten
 // sonra. Soruşturmanın ortasında ödül teklif etmek oyuncuyu oradan koparır.
 [Test]
 public void Rewarded_OnlyWhenTheReportCameBack() {
  AdGateway.SetConsent(AdConsent.Granted);
  foreach (var placement in new[] { AdPlacement.RewardedGuidance, AdPlacement.RewardedRetry }) {
   Assert.IsTrue(AdGateway.MayShow(placement, AdMoment.ReportRejected), placement + " reddedilen raporda açık olmalı.");
   Assert.IsFalse(AdGateway.MayShow(placement, AdMoment.Investigation), placement + " soruşturmada kapalı olmalı.");
   Assert.IsFalse(AdGateway.MayShow(placement, AdMoment.CaseClosed), placement + " kapanış anına ait değil.");
  }
 }

 // Reklam kaldırıldıysa hiçbir reklam gösterilmez; ama ödül oyuncudan
 // alınmaz — para ödeyen oyuncu ödülden mahrum kalmamalı.
 [Test]
 public void RemovingAds_StopsAdsButKeepsRewards() {
  AdGateway.SetConsent(AdConsent.Granted);
  AdGateway.SetAdsRemoved(true);
  var provider = new AlwaysReady();
  AdGateway.Provider = provider;

  bool guidance = false;
  AdGateway.Request(AdPlacement.RewardedGuidance, AdMoment.ReportRejected, granted => guidance = granted);
  Assert.IsTrue(guidance, "Reklamsız oyuncu ödülü almalı.");

  bool interstitial = true;
  AdGateway.Request(AdPlacement.CaseInterval, AdMoment.CaseClosed, granted => interstitial = granted);
  Assert.IsFalse(interstitial, "Araya giren reklam gösterilmemeli.");
  Assert.AreEqual(0, provider.Shown, "Reklam kaldırıldıysa ağ hiç çağrılmamalı.");
 }

 // Ağ yokken "reklam yok" bedava ödül anlamına gelmemeli, yoksa ağ takıldığı
 // gün oyun dengesi sessizce değişir.
 [Test]
 public void WithoutANetwork_RewardIsNotGranted() {
  AdGateway.SetConsent(AdConsent.Granted);
  bool granted = true;
  AdGateway.Request(AdPlacement.RewardedGuidance, AdMoment.ReportRejected, result => granted = result);
  Assert.IsFalse(granted, "Ağ yokken ödül verilmemeli.");
 }

 [Test]
 public void Consent_SurvivesReload() {
  AdGateway.SetConsent(AdConsent.Granted);
  AdGateway.Load();
  Assert.AreEqual(AdConsent.Granted, AdGateway.Consent);
 }

 // İpucu ekranı oyuncunun **kendi** çalışmasını sayar; vakanın gerçeğini değil.
 [Test]
 public void Coverage_CountsOnlyWhatThePlayerDid() {
  var data = JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/case001").text);
  var game = new Investigation(data);
  var fresh = Coverage.Of(game);
  Assert.AreEqual(0, fresh.SourcesOpen, "Yeni vakada açılmış kaynak olmamalı.");
  Assert.IsFalse(fresh.Complete, "Hiç çalışmamış oyuncu için kapsama tam olamaz.");

  game.State.caseAccepted = true;
  var first = data.nodes.First(node => game.Available(node));
  game.Read(first.id);
  var after = Coverage.Of(game);
  Assert.AreEqual(1, after.SourcesOpen, "Açılan kaynak sayılmalı.");
  Assert.GreaterOrEqual(after.SourcesAvailable, after.SourcesOpen, "Açılabilir sayısı açılandan küçük olamaz.");
 }

 // Vaka başı ve menüye dönüş reklamı yeni oyuncuya gösterilmez; yalnız kendi anında.
 [Test]
 public void NewInterstitials_OnlyForSeasonedPlayers_AndOnlyAtTheirMoment() {
  AdGateway.SetConsent(AdConsent.Granted);
  Assert.IsFalse(AdGateway.MayShow(AdPlacement.CaseStart, AdMoment.CaseAccepted), "İlk vakasını kapatmamış oyuncuya vaka başı reklamı yok.");
  Assert.IsFalse(AdGateway.MayShow(AdPlacement.MenuReturn, AdMoment.Menu), "İlk vakasını kapatmamış oyuncuya menü reklamı yok.");
  AdGateway.Seasoned = true;
  Assert.IsTrue(AdGateway.MayShow(AdPlacement.CaseStart, AdMoment.CaseAccepted));
  Assert.IsTrue(AdGateway.MayShow(AdPlacement.MenuReturn, AdMoment.Menu));
  foreach (var moment in new[] { AdMoment.Investigation, AdMoment.Interview, AdMoment.Cctv, AdMoment.Cinematic, AdMoment.Waiting })
   foreach (var placement in new[] { AdPlacement.CaseStart, AdPlacement.MenuReturn, AdPlacement.RewardedCosmetic })
    Assert.IsFalse(AdGateway.MayShow(placement, moment), placement + " / " + moment + " kapalı olmalı.");
 }

 // Araya giren reklamlar arasında en az dört dakika; ödüllü reklam sınırdan etkilenmez.
 [Test]
 public void Interstitials_ShareAFrequencyCap() {
  AdGateway.SetConsent(AdConsent.Granted);
  AdGateway.Seasoned = true;
  var provider = new AlwaysReady();
  AdGateway.Provider = provider;
  AdGateway.Request(AdPlacement.CaseInterval, AdMoment.CaseClosed, null);
  Assert.AreEqual(1, provider.Shown);
  clock += 60f;
  Assert.IsFalse(AdGateway.MayShow(AdPlacement.MenuReturn, AdMoment.Menu), "Bir dakika sonra yeni araya giren reklam olmamalı.");
  Assert.IsFalse(AdGateway.MayShow(AdPlacement.CaseStart, AdMoment.CaseAccepted));
  Assert.IsTrue(AdGateway.MayShow(AdPlacement.RewardedCosmetic, AdMoment.Menu), "Ödüllü reklam sınırdan etkilenmemeli.");
  clock += AdGateway.InterstitialGapSeconds;
  Assert.IsTrue(AdGateway.MayShow(AdPlacement.MenuReturn, AdMoment.Menu), "Süre dolunca yeniden açılmalı.");
 }

 // Kısa bekleyişe reklam teklif edilmez.
 [Test]
 public void SkipWait_OnlyForLongWaits() {
  AdGateway.SetConsent(AdConsent.Granted);
  Assert.IsFalse(AdGateway.MaySkipWait(10), "10 saniyelik bekleyiş için reklam yok.");
  Assert.IsTrue(AdGateway.MaySkipWait(AdGateway.MinSkipSeconds));
  AdGateway.SetConsent(AdConsent.Denied);
  Assert.IsFalse(AdGateway.MaySkipWait(600), "Onay yoksa teklif yok.");
  AdGateway.SetAdsRemoved(true);
  Assert.IsTrue(AdGateway.MaySkipWait(600), "Reklamsız oyuncu atlamayı reklamsız alır.");
 }

 // Reklamsız oyuncu görünüm ödülünü de reklamsız alır; araya giren yenileri görmez.
 [Test]
 public void RemovingAds_CoversNewPlacements() {
  AdGateway.SetConsent(AdConsent.Granted);
  AdGateway.Seasoned = true;
  AdGateway.SetAdsRemoved(true);
  Assert.IsTrue(AdGateway.RewardEarnedWithoutAd(AdPlacement.RewardedCosmetic));
  Assert.IsTrue(AdGateway.RewardEarnedWithoutAd(AdPlacement.RewardedSkipWait));
  Assert.IsFalse(AdGateway.RewardEarnedWithoutAd(AdPlacement.CaseStart));
  Assert.IsFalse(AdGateway.MayShow(AdPlacement.MenuReturn, AdMoment.Menu));
 }
}
}
