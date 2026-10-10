using System;
using UnityEngine;

namespace Bube {

// Reklamın **dikişi**. Ağ eklentisi (LevelPlay, AdMob) burada değil: onlar
// `IAdProvider`ı gerçekleyen birer parça olarak sonra takılır ve oyunun geri
// kalanı bunu bilmez. Bugün oyunda hiçbir reklam yok; kurallar yine de
// yazılı ve testli, çünkü asıl risk ağ değil, reklamın **yanlış ana** düşmesi.
public enum AdPlacement {
 CaseInterval,      // vaka kapandıktan sonra, araya giren tam ekran
 RewardedRetry,     // ödüllü: başarısız vakayı yeniden aç, güveni geri ver
 RewardedGuidance,  // ödüllü: yöntem hatırlatması + oyuncunun kendi kapsamı
 CaseStart,         // vaka teklifi kabul edildikten sonra, soruşturma başlamadan
 MenuReturn,        // oyundan ana menüye dönerken
 RewardedCosmetic,  // ödüllü: masa için görünüm (lamba rengi); oynanışa dokunmaz
 RewardedSkipWait,  // ödüllü: uzun bir bekleyişi atla (talep, faks); yalnız bekleme uzunsa
}

// Oyunun hangi anında olduğumuz. Reklam kararı "hangi ekran" değil "hangi an"
// sorusuna bakar; ekran adı değişir, an değişmez.
public enum AdMoment { Menu, CaseClosed, ReportRejected, Investigation, Interview, Cctv, Cinematic, CaseAccepted, Waiting }

public interface IAdProvider {
 bool Ready(AdPlacement placement);
 // `granted` yalnız ödüllü reklamda anlamlıdır: ödül hak edildi mi.
 void Show(AdPlacement placement, Action<bool> finished);
}

// Ağ yokken kullanılan gerçekleme. Hiçbir şey göstermez ve **ödül vermez**:
// "reklam yok" sessizce bedava ödül anlamına gelmemeli, yoksa ağ takıldığı gün
// oyun dengesi değişir.
public sealed class NoAdProvider : IAdProvider {
 public bool Ready(AdPlacement placement) => false;
 public void Show(AdPlacement placement, Action<bool> finished) => finished?.Invoke(false);
}

public static class AdGateway {

 public const string NoAdsKey   = "bube.ads.removed";

 public static IAdProvider Provider = new NoAdProvider();
 // Satın alma (IAP) sonra gelir; bayrak bugünden var, çünkü bütün kurallar
 // ona bakar ve sonradan eklenen bir bayrak her karar noktasını yeniden açar.
 public static bool AdsRemoved { get; private set; }

 // Araya giren reklamların ortak sıklık sınırı: ikisi arasında en az bu kadar saniye.
 public const float InterstitialGapSeconds = 240f;
 // Bekleme atlama yalnız bu kadar ya da daha uzun beklemede teklif edilir; kısa
 // bekleyişe reklam koymak oyuncuyu reklama iter.
 public const double MinSkipSeconds = 60;
 public static Func<float> Now = () => Time.realtimeSinceStartup;
 static float lastInterstitial = float.NegativeInfinity;
 // İlk vakasını henüz kapatmamış oyuncu vaka arası dışında araya giren reklam görmez.
 public static bool Seasoned;
 public static void ResetInterstitialClock() => lastInterstitial = float.NegativeInfinity;
 static bool Interstitial(AdPlacement p) => p == AdPlacement.CaseInterval || p == AdPlacement.CaseStart || p == AdPlacement.MenuReturn;

 public static void Load() {
  AdsRemoved = PlayerPrefs.GetInt(NoAdsKey, 0) == 1;
 }

 public static void SetAdsRemoved(bool removed) {
  AdsRemoved = removed;
  PlayerPrefs.SetInt(NoAdsKey, removed ? 1 : 0);
  PlayerPrefs.Save();
 }

 // Kuralın tamamı burada ve tek yerde:
 //  · Reklam kaldırıldıysa hiçbir reklam yok — ödüllü olan bile; ödülü ise
 //    oyuncu reklamsız alır (`RewardEarnedWithoutAd`).
 //  · Oyunun kendi izin penceresi yok (3 Ekim 2026): reklam kişiselleştirilmemiş
 //    gösterilir; bölgeye göre gereken onayı reklam ağının kendi formu (UMP) sorar.
 //  · Araya giren reklam yalnız vaka kapandıktan sonraki değerlendirme anında
 //    olur. Soruşturmanın, sorgunun, CCTV'nin ve sinematiğin içi kapalıdır:
 //    oyuncunun düşündüğü an kesilmez.
 //  · Ödüllü reklam yalnız oyuncunun kendi istediği anda: raporu geri
 //    döndükten sonra, masa görünümü için menüde, uzun bir bekleyişte.
 //  · Vaka başı ve menüye dönüş reklamları yalnız ilk vakasını kapatmış
 //    oyuncuya; tüm araya giren reklamlar arasında en az 4 dakika.
 public static bool MayShow(AdPlacement placement, AdMoment moment) {
  if (AdsRemoved) return false;
  if (Interstitial(placement) && Now() - lastInterstitial < InterstitialGapSeconds) return false;
  switch (placement) {
   case AdPlacement.CaseInterval: return moment == AdMoment.CaseClosed;
   case AdPlacement.CaseStart: return Seasoned && moment == AdMoment.CaseAccepted;
   case AdPlacement.MenuReturn: return Seasoned && moment == AdMoment.Menu;
   case AdPlacement.RewardedCosmetic: return moment == AdMoment.Menu;
   case AdPlacement.RewardedSkipWait: return moment == AdMoment.Waiting;
   case AdPlacement.RewardedRetry:
   case AdPlacement.RewardedGuidance: return moment == AdMoment.ReportRejected;
   default: return false;
  }
 }

 // Reklam kaldırıldıysa ödül reklamsız verilir; oyuncu para ödediği için
 // ödülden mahrum kalmaz.
 public static bool RewardEarnedWithoutAd(AdPlacement placement) =>
  AdsRemoved && !Interstitial(placement);

 // Bekleme atlama teklif edilebilir mi: kalan süre yeterince uzun ve an uygun.
 public static bool MaySkipWait(double remainingSeconds) =>
  remainingSeconds >= MinSkipSeconds && (AdsRemoved || MayShow(AdPlacement.RewardedSkipWait, AdMoment.Waiting));

 // Tek çağrı noktası. Sonuç geri dönene kadar oyun akışı beklemez; ödül
 // verilmediyse ekran hiçbir şey değiştirmez.
 public static void Request(AdPlacement placement, AdMoment moment, Action<bool> finished) {
  if (RewardEarnedWithoutAd(placement)) { finished?.Invoke(true); return; }
  if (!MayShow(placement, moment) || !Provider.Ready(placement)) { finished?.Invoke(false); return; }
  if (Interstitial(placement)) lastInterstitial = Now();
  Provider.Show(placement, finished);
 }
}
}

namespace Bube {
// Mağaza satın alması: tek ürün, tek seferlik "Reklamları kaldır". Gerçek mağaza
// `Bube.Purchasing` derlemesinde (Unity IAP); paket yoksa düğmeler "kullanılamıyor" der.
public interface IStore {
 bool Ready { get; }
 // Ürünün mağazadaki yerel fiyat metni ("₺49,99"); hazır değilse null.
 string Price(string product);
 // Sonuç: hata anahtarı ya da null (başarılı).
 void Buy(string product, System.Action<string> done);
 // Apple zorunluluğu: önceki satın almaları geri yükle. Sonuç: hata anahtarı ya da null.
 void Restore(System.Action<string> done);
}

public sealed class NoStore : IStore {
 public bool Ready => false;
 public string Price(string product) => null;
 public void Buy(string product, System.Action<string> done) => done("store.error.unavailable");
 public void Restore(System.Action<string> done) => done("store.error.unavailable");
}

public static class Store {
 public const string NoAdsProduct = "com.bubegames.karine.noads";
 public const string PriorityProduct = "com.bubegames.karine.priority";
 public static IStore Provider = new NoStore();
 // Makbuzu gelen ürünün etkisi tek yerde.
 public static void Grant(string product) {
  if (product == NoAdsProduct) AdGateway.SetAdsRemoved(true);
  else if (product == PriorityProduct) Priority.Set(true);
 }
 public static bool Owned(string product) => product == NoAdsProduct ? AdGateway.AdsRemoved : product == PriorityProduct && Priority.Owned;
}

// "Kalıcı öncelik": izin gerektiren incelemelerin uzun bekleyişi kalkar, talep kendi
// kısa süresiyle gelir. Bilgi vermez, yalnız zamanı kısaltır.
public static class Priority {
 const string Key = "karine.priority";
 public static bool Owned => UnityEngine.PlayerPrefs.GetInt(Key, 0) == 1;
 public static void Set(bool owned) { UnityEngine.PlayerPrefs.SetInt(Key, owned ? 1 : 0); UnityEngine.PlayerPrefs.Save(); }
}
}
