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
 }
}
}
