using UnityEngine;

namespace Bube {
// Oyuncunun seçtiği sabit kare hızı. Mobilde vSync yok sayılır; hızı
// targetFrameRate belirler. Ekran desteklemiyorsa cihaz kendi tavanında kalır.
public static class FrameRate {
 public const string Key = "karine.fps";
 public static readonly int[] Options = { 30, 60, 120 };
 public static int Current { get; private set; } = 120;

 public static void Load() {
  int value = PlayerPrefs.GetInt(Key, 120);
  Current = System.Array.IndexOf(Options, value) >= 0 ? value : 120;
  Apply();
 }

 public static void Set(int value) {
  if (System.Array.IndexOf(Options, value) < 0) return;
  Current = value;
  PlayerPrefs.SetInt(Key, value);
  Apply();
 }

 static void Apply() {
  QualitySettings.vSyncCount = 0;
  Application.targetFrameRate = Current;
  RequestRefresh();
 }

 // Android ekranı varsayılan olarak 60 Hz'de açar; targetFrameRate tek başına
 // tavanı aşamaz. Seçilen hıza en yakın desteklenen yenileme hızı istenir.
 // Editor'de Game görünümü monitörün hızına bağlıdır, orada istek yapılmaz.
 static void RequestRefresh() {
#if UNITY_ANDROID && !UNITY_EDITOR
  var now = Screen.currentResolution;
  RefreshRate best = now.refreshRateRatio;
  double bestGap = double.MaxValue;
  foreach (var r in Screen.resolutions) {
   if (r.width != now.width || r.height != now.height) continue;
   double hz = r.refreshRateRatio.value, gap = System.Math.Abs(hz - Current);
   if (hz + .5 >= Current && gap < bestGap) { bestGap = gap; best = r.refreshRateRatio; }
  }
  if (bestGap == double.MaxValue) foreach (var r in Screen.resolutions) if (r.refreshRateRatio.value > best.value) best = r.refreshRateRatio;
  Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, best);
#endif
 }
}
}
