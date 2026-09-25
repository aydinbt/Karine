using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Ses henüz dosyasız: sistem kurulu, klipler yok. Bu yüzden testler "ses
// duyuldu mu"yu değil, **sesin kararlarını** ve kancanın bağlı olduğunu sorar.
public sealed class SoundTests {

 [SetUp] public void Setup() {
  KarineUI.Fonts = FontSet.Load();
  PlayerPrefs.DeleteKey(SoundSettings.MusicKey);
  PlayerPrefs.DeleteKey(SoundSettings.SfxKey);
  SoundSettings.Load();
 }

 [TearDown] public void TearDown() => KarineUI.Sound = null;

 // Kit'te kaydırıcı yok; ses üç kademedir ve kademeler kazanç değerlerine
 // karşılık gelir. Kapalı gerçekten sıfır olmalı, "çok kısık" değil.
 [Test]
 public void ThreeLevels_MapToGains_AndOffIsSilent() {
  Assert.AreEqual(0f, SoundSettings.Gain(SoundLevel.Off), "Kapalı sessiz olmalı.");
  Assert.Less(SoundSettings.Gain(SoundLevel.Low), SoundSettings.Gain(SoundLevel.Full), "Kısık, açıktan düşük olmalı.");
  Assert.AreEqual(1f, SoundSettings.Gain(SoundLevel.Full), "Açık tam olmalı.");
 }

 // Varsayılan: müzik kısık, efekt açık. Dedektiflik oyunu sessiz odada oynanır.
 [Test]
 public void Defaults_KeepMusicQuiet() {
  Assert.AreEqual(SoundLevel.Low, SoundSettings.Music);
  Assert.AreEqual(SoundLevel.Full, SoundSettings.Sfx);
 }

 [Test]
 public void Level_SurvivesReload() {
  SoundSettings.SetMusic(SoundLevel.Off);
  SoundSettings.SetSfx(SoundLevel.Low);
  SoundSettings.Load();
  Assert.AreEqual(SoundLevel.Off, SoundSettings.Music);
  Assert.AreEqual(SoundLevel.Low, SoundSettings.Sfx);
 }

 // Kayıtta saçma bir sayı varsa ses ayarı yüzünden oyun açılmaz olmamalı.
 [Test]
 public void BrokenPreference_FallsBackToDefault() {
  PlayerPrefs.SetInt(SoundSettings.MusicKey, 99);
  SoundSettings.Load();
  Assert.AreEqual(SoundLevel.Low, SoundSettings.Music);
 }

 // Asıl kilit bu: ekranlar ses çalmayı unutamasın diye ses kit düğmesinin
 // **kurucusundan** çıkar. Burada sözleşme sınanır; kit'in hiçbir düğmesinin
 // bu kapıyı atlamadığını ise doğrulayıcı kaynak üstünde kilitler
 // (`ProjectRules`: `Runtime/UI` içinde her `new Button(` `Sounded(` ile sarılı).
 [Test]
 public void PressGate_AsksForTheRightSound() {
  var heard = new System.Collections.Generic.List<string>();
  KarineUI.Sound = id => heard.Add(id);

  KarineUI.Sounded(null)();
  Assert.AreEqual(new[] { AudioDirector.Press }, heard.ToArray(), "Basma sesi istenmedi.");

  heard.Clear();
  KarineUI.Sounded(null, AudioDirector.Page)();
  Assert.AreEqual(new[] { AudioDirector.Page }, heard.ToArray(), "Kâğıt sesi istenmedi.");

  // Ses isteği eylemi yutmamalı: düğme hem ses çıkarır hem işini yapar.
  bool ran = false;
  KarineUI.Sounded(() => ran = true)();
  Assert.IsTrue(ran, "Düğmenin kendi eylemi çalışmadı.");
 }

 // Konuşma perdesi kişiden türer: aynı kişi her zaman aynı sesle konuşur,
 // farklı kişiler farklı perdeye düşer ve perde makul aralıkta kalır.
 [Test]
 public void VoicePitch_IsStablePerPersonAndStaysInRange() {
  Assert.AreEqual(AudioDirector.VoicePitch("hasan"), AudioDirector.VoicePitch("hasan"),
   "Aynı kişi iki farklı perdeyle konuşmamalı.");
  var pitches = new[] { "mert", "elif", "hasan" }
   .Select(AudioDirector.VoicePitch).ToArray();
  Assert.AreEqual(3, pitches.Distinct().Count(), "Üç kişi üç perde olmalı.");
  foreach (var pitch in pitches) {
   Assert.GreaterOrEqual(pitch, 0.85f, "Perde fazla alçak: ses tanınmaz olur.");
   Assert.LessOrEqual(pitch, 1.15f, "Perde fazla yüksek: çizgi film olur.");
  }
  // Kişisi olmayan satır (anlatıcı, faks) doğal perdede kalır.
  Assert.AreEqual(1f, AudioDirector.VoicePitch(null));
 }

 // Kimse dinlemiyorsa (testler, başsız koşu) ses istemek patlamamalı.
 [Test]
 public void NoListener_IsNotAnError() {
  KarineUI.Sound = null;
  Assert.DoesNotThrow(() => KarineUI.Sounded(null)());
 }
}
}
