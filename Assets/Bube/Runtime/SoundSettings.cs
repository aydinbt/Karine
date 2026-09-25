using UnityEngine;

namespace Bube {

// Sesin **kararları** burada, çalması `AudioDirector`da. Böylece ses düzeyi
// mantığı Editor testinde AudioSource olmadan sınanabilir.
//
// Kit'te kaydırıcı (slider) yoktur ve icat edilmez: ses düzeyi üç kademedir ve
// ayarlarda kit'in radyo grubuyla seçilir.
public enum SoundLevel { Off = 0, Low = 1, Full = 2 }

public static class SoundSettings {

 public const string MusicKey = "bube.sound.music";
 public const string SfxKey   = "bube.sound.sfx";

 // Varsayılan: müzik kısık, efektler açık. Dedektiflik oyunu sessiz odada
 // oynanır; müzik öne çıkmaz.
 public static SoundLevel Music { get; private set; } = SoundLevel.Low;
 public static SoundLevel Sfx   { get; private set; } = SoundLevel.Full;

 public static float Gain(SoundLevel level) =>
  level == SoundLevel.Off ? 0f : level == SoundLevel.Low ? .55f : 1f;

 public static float MusicGain => Gain(Music);
 public static float SfxGain   => Gain(Sfx);

 public static void Load() {
  Music = Read(MusicKey, SoundLevel.Low);
  Sfx   = Read(SfxKey,   SoundLevel.Full);
 }

 public static void SetMusic(SoundLevel level) { Music = level; Write(MusicKey, level); }
 public static void SetSfx(SoundLevel level)   { Sfx   = level; Write(SfxKey,   level); }

 // Kayıtta saçma bir sayı varsa varsayılana düşer; ses ayarı yüzünden oyun açılmaz olmaz.
 static SoundLevel Read(string key, SoundLevel fallback) {
  int value = PlayerPrefs.GetInt(key, (int)fallback);
  return value == (int)SoundLevel.Off || value == (int)SoundLevel.Low || value == (int)SoundLevel.Full
   ? (SoundLevel)value : fallback;
 }

 static void Write(string key, SoundLevel level) {
  PlayerPrefs.SetInt(key, (int)level);
  PlayerPrefs.Save();
 }

 // Ayarlar ekranındaki satır adı.
 public static string LabelKey(SoundLevel level) =>
  level == SoundLevel.Off ? "settings.sound.off" : level == SoundLevel.Low ? "settings.sound.low" : "settings.sound.full";
}
}
