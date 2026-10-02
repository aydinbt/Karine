using UnityEngine;

namespace Bube {
// Efektlerin ortak kuralları: oyuncunun seçtiği yoğunluk, titreşim, ışık
// çakması sınırı ve zayıf cihazda kendiliğinden hafifleme.
//
// Hiçbir efekt oyunun durumundan bilgi taşımaz: yoğunluk yalnız miktarı
// değiştirir, neyin olduğunu değil. Doğru ya da yanlış seçime ayrı efekt yoktur.
public enum FxLevel { Off, Light, Full }
public enum Haptic { Tick, Press, Thud }

public static class Fx {
 public const string LevelKey = "karine.fx", HapticsKey = "karine.haptics";

 public static FxLevel Level { get; private set; } = FxLevel.Full;
 public static bool Haptics { get; private set; } = true;
 // Zayıf cihaz, düşük pil ya da düşen kare hızı: parçacık sayısı yarıya iner.
 public static bool Degraded { get; private set; }

 // Atmosfer efektleri açık mı? "Hareketi azalt" her şeyi yener.
 public static bool On => Level != FxLevel.Off && !KarineMotion.Reduced;
 // Miktar çarpanı: parçacık sayısı, saydamlık, sıklık.
 public static float Amount => !On ? 0f : (Level == FxLevel.Light ? .5f : 1f) * (Degraded ? .5f : 1f);
 public static int Count(int full) => Mathf.Max(0, Mathf.RoundToInt(full * Amount));

 public static void Load() {
  int level = PlayerPrefs.GetInt(LevelKey, (int)FxLevel.Full);
  Level = level >= 0 && level <= 2 ? (FxLevel)level : FxLevel.Full;
  Haptics = PlayerPrefs.GetInt(HapticsKey, 1) == 1;
  // Bellek ve çekirdek sayısı düşükse baştan hafif başlanır.
  Degraded = SystemInfo.systemMemorySize > 0 && SystemInfo.systemMemorySize < 3000 || SystemInfo.processorCount <= 4;
 }

 public static void Set(FxLevel level, bool haptics) {
  Level = level; Haptics = haptics;
  PlayerPrefs.SetInt(LevelKey, (int)level);
  PlayerPrefs.SetInt(HapticsKey, haptics ? 1 : 0);
 }

 // Işığa duyarlılık: hiçbir çakma, titreme ya da parazit saniyede üç kezden
 // sık olamaz (WCAG 2.3.1). Bütün yanıp sönen efektler buradan izin ister;
 // izin yoksa o çakma atlanır, efekt yine de "sakin" hâliyle sürer.
 const float FlashGap = .34f;
 static float lastFlash = -10f;
 public static bool MayFlash() {
  if (!On) return false;
  float now = Time.realtimeSinceStartup;
  if (now - lastFlash < FlashGap) return false;
  lastFlash = now; return true;
 }

 // Kare süresi izlenir: hedefin bir buçuk katını üç saniye boyunca aşarsa ya
 // da pil %20'nin altında boşalıyorsa efektler hafifler. Geri dönüş yok: bir
 // oturumda iki yana sallanmak, sabit kalmaktan daha çok göze batar.
 static float slowFor;
 public static void Watch(float deltaTime) {
  if (Degraded) return;
  if (SystemInfo.batteryStatus == BatteryStatus.Discharging && SystemInfo.batteryLevel >= 0f && SystemInfo.batteryLevel < .2f) { Degraded = true; return; }
  float target = 1f / Mathf.Max(30, FrameRate.Current);
  slowFor = deltaTime > target * 1.5f ? slowFor + deltaTime : 0f;
  if (slowFor > 3f) Degraded = true;
 }

 // Kısa titreşim. Android'de süre milisaniyeyle verilir; iOS yalnız tek tip
 // titreşim tanır, orada yalnız "Thud" titrer.
 public static void Buzz(Haptic kind) {
  if (!Haptics) return;
#if UNITY_ANDROID && !UNITY_EDITOR
  long ms = kind == Haptic.Tick ? 12 : kind == Haptic.Press ? 22 : 45;
  try {
   using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
   using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
   using (var vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
    vibrator?.Call("vibrate", ms);
  } catch (System.Exception) { }
#elif UNITY_IOS && !UNITY_EDITOR
  if (kind == Haptic.Thud) Handheld.Vibrate();
#endif
 }

 // Yalnız Unity'nin titreşim iznini derlemeye eklemesi için: `Handheld.Vibrate`
 // kodda geçmezse Android bildirgesine VIBRATE izni yazılmaz.
 static void PermissionAnchor() { if (Time.frameCount < 0) Handheld.Vibrate(); }
}
}
