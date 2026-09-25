using UnityEngine;

namespace Bube {

// Sesin **kararları** burada, çalması `AudioDirector`da. Böylece ses düzeyi
// mantığı Editor testinde AudioSource olmadan sınanabilir.
//
// Kit'te kaydırıcı (slider) yoktur ve icat edilmez: ses düzeyi **kademelidir**
// ve ayarlarda kit'in radyo grubuyla seçilir. Üç kademe ince ayar için yetmedi
// (kullanıcı kararı), beşe çıktı.
//
// Sayı doğrudan yüzdedir; böylece kayıtta duran değer okunduğunda ne olduğu
// belli olur ve kademe eklemek eski kaydı bozmaz. Eski üç kademeli kayıt
// (0/1/2) `Read` içinde çevrilir.
public enum SoundLevel { Off = 0, Quarter = 25, Half = 50, Most = 75, Full = 100 }

public static class SoundSettings {

 public const string MusicKey = "bube.sound.music";
 public const string SfxKey   = "bube.sound.sfx";

 // Varsayılan: müzik kısık, efektler açık. Dedektiflik oyunu sessiz odada
 // oynanır; müzik öne çıkmaz.
 public static SoundLevel Music { get; private set; } = SoundLevel.Half;
 public static SoundLevel Sfx   { get; private set; } = SoundLevel.Full;

 // Kademeler ekranda da bu sırayla durur.
 public static readonly SoundLevel[] Levels = {
  SoundLevel.Off, SoundLevel.Quarter, SoundLevel.Half, SoundLevel.Most, SoundLevel.Full,
 };

 public static float Gain(SoundLevel level) => (int)level / 100f;

 public static float MusicGain => Gain(Music);
 public static float SfxGain   => Gain(Sfx);

 public static void Load() {
  Music = Read(MusicKey, SoundLevel.Half);
  Sfx   = Read(SfxKey,   SoundLevel.Full);
 }

 public static void SetMusic(SoundLevel level) { Music = level; Write(MusicKey, level); }
 public static void SetSfx(SoundLevel level)   { Sfx   = level; Write(SfxKey,   level); }

 // Kayıtta saçma bir sayı varsa varsayılana düşer; ses ayarı yüzünden oyun açılmaz olmaz.
 static SoundLevel Read(string key, SoundLevel fallback) {
  int value = PlayerPrefs.GetInt(key, (int)fallback);
  // Eski üç kademeli kayıt: 0 kapalı, 1 kısık, 2 açık.
  if (value == 1) return SoundLevel.Half;
  if (value == 2) return SoundLevel.Full;
  foreach (var level in Levels)
   if (value == (int)level) return level;
  return fallback;
 }

 static void Write(string key, SoundLevel level) {
  PlayerPrefs.SetInt(key, (int)level);
  PlayerPrefs.Save();
 }

 // Ayarlar ekranındaki satır adı. Yalnız uçların sözü var ("Kapalı", "Tam");
 // aradaki kademeler yüzdeyle anılır ve yüzde çevrilecek bir metin değil, o
 // yüzden dil dosyasına girmiyor — çağıran `null` görürse "%50" yazar.
 public static string LabelKey(SoundLevel level) =>
  level == SoundLevel.Off ? "settings.sound.off" :
  level == SoundLevel.Full ? "settings.sound.full" : null;
}
}
