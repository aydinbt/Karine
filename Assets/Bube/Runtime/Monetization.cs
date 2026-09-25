using System;
using UnityEngine;

namespace Bube {

// Reklamın **dikişi**. Ağ eklentisi (LevelPlay, AdMob) burada değil: onlar
// `IAdProvider`ı gerçekleyen birer parça olarak sonra takılır ve oyunun geri
// kalanı bunu bilmez. Bugün oyunda hiçbir reklam yok; kurallar yine de
// yazılı ve testli, çünkü asıl risk ağ değil, reklamın **yanlış ana** düşmesi.
public enum AdPlacement {
 CaseInterval,      // vaka kapandıktan sonra, araya giren tam ekran
 RewardedRetry,     // ödüllü: başarısız vakayı güven kaybı olmadan yeniden aç
 RewardedGuidance,  // ödüllü: yöntem hatırlatması + oyuncunun kendi kapsamı
}

// Oyunun hangi anında olduğumuz. Reklam kararı "hangi ekran" değil "hangi an"
// sorusuna bakar; ekran adı değişir, an değişmez.
public enum AdMoment { Menu, CaseClosed, ReportRejected, Investigation, Interview, Cctv, Cinematic }

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

public enum AdConsent { Unknown = 0, Granted = 1, Denied = 2 }

public static class AdGateway {

 public const string ConsentKey = "bube.ads.consent";
 public const string NoAdsKey   = "bube.ads.removed";

 public static IAdProvider Provider = new NoAdProvider();
 public static AdConsent Consent { get; private set; }
 // Satın alma (IAP) sonra gelir; bayrak bugünden var, çünkü bütün kurallar
 // ona bakar ve sonradan eklenen bir bayrak her karar noktasını yeniden açar.
 public static bool AdsRemoved { get; private set; }

 public static void Load() {
  int consent = PlayerPrefs.GetInt(ConsentKey, (int)AdConsent.Unknown);
  Consent = consent == (int)AdConsent.Granted ? AdConsent.Granted
   : consent == (int)AdConsent.Denied ? AdConsent.Denied : AdConsent.Unknown;
  AdsRemoved = PlayerPrefs.GetInt(NoAdsKey, 0) == 1;
 }

 public static void SetConsent(AdConsent value) {
  Consent = value;
  PlayerPrefs.SetInt(ConsentKey, (int)value);
  PlayerPrefs.Save();
 }

 public static void SetAdsRemoved(bool removed) {
  AdsRemoved = removed;
  PlayerPrefs.SetInt(NoAdsKey, removed ? 1 : 0);
  PlayerPrefs.Save();
 }

 // Kuralın tamamı burada ve tek yerde:
 //  · Reklam kaldırıldıysa hiçbir reklam yok — ödüllü olan bile; ödülü ise
 //    oyuncu reklamsız alır (`RewardEarnedWithoutAd`).
 //  · Onay verilmemişse (Unknown/Denied) reklam gösterilmez. Onay ekranı
 //    reklamdan **önce** gelir.
 //  · Araya giren reklam yalnız vaka kapandıktan sonraki değerlendirme anında
 //    olur. Soruşturmanın, sorgunun, CCTV'nin ve sinematiğin içi kapalıdır:
 //    oyuncunun düşündüğü an kesilmez.
 //  · Ödüllü reklam yalnız oyuncunun kendi istediği anda: raporu geri
 //    döndükten sonra.
 public static bool MayShow(AdPlacement placement, AdMoment moment) {
  if (AdsRemoved || Consent != AdConsent.Granted) return false;
  switch (placement) {
   case AdPlacement.CaseInterval: return moment == AdMoment.CaseClosed;
   case AdPlacement.RewardedRetry:
   case AdPlacement.RewardedGuidance: return moment == AdMoment.ReportRejected;
   default: return false;
  }
 }

 // Reklam kaldırıldıysa ödül reklamsız verilir; oyuncu para ödediği için
 // ödülden mahrum kalmaz.
 public static bool RewardEarnedWithoutAd(AdPlacement placement) =>
  AdsRemoved && placement != AdPlacement.CaseInterval;

 // Tek çağrı noktası. Sonuç geri dönene kadar oyun akışı beklemez; ödül
 // verilmediyse ekran hiçbir şey değiştirmez.
 public static void Request(AdPlacement placement, AdMoment moment, Action<bool> finished) {
  if (RewardEarnedWithoutAd(placement)) { finished?.Invoke(true); return; }
  if (!MayShow(placement, moment) || !Provider.Ready(placement)) { finished?.Invoke(false); return; }
  Provider.Show(placement, finished);
 }
}
}
