using System.Collections.Generic;
using UnityEngine;

namespace Bube {

// Oyunun tek ses çıkışı. Üç kanal var ve karışmazlar:
//  · müzik   — ana menü döngüsü, sinematik altı; tek seferde bir tane, döngülü
//  · ortam   — odanın sesi (masa, görüşme odası, CCTV terminali); döngülü
//  · efekt   — düğme, sayfa, daktilo, mühür; üst üste binebilir
//
// **Ses dosyası yokken de doğru çalışır.** Eksik klip oyunu durdurmaz, sessiz
// geçer ve bir kez not düşer — videoların davranışıyla aynı. Böylece ses
// dosyaları geldiğinde tek iş dosyayı `Resources/Bube/Audio/` içine koymaktır,
// ekranlara geri dönmek gerekmez.
public sealed class AudioDirector : MonoBehaviour {

 public const string Folder = "Bube/Audio/";

 // Ekranların kullandığı adlar. Dosya adı da bunlardır; ekranlar yol yazmaz.
 public const string Press        = "ui_press";
 public const string Page         = "ui_page";
 public const string Typewriter   = "ui_typewriter";
 public const string Stamp        = "ui_stamp";
 public const string Notification = "ui_notification";

 AudioSource music, ambience, effects;
 readonly Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();
 readonly HashSet<string> reported = new HashSet<string>();
 string musicId, ambienceId;

 // Hangi sesler eksik: doğrulama ve hata ayıklama okuyabilsin.
 public IEnumerable<string> MissingClips => reported;

 public static AudioDirector Attach(GameObject host) {
  var director = host.GetComponent<AudioDirector>() ?? host.AddComponent<AudioDirector>();
  director.Wake();
  return director;
 }

 void Wake() {
  if (effects != null) return;
  music    = Channel("Music",    loop: true);
  ambience = Channel("Ambience", loop: true);
  effects  = Channel("Effects",  loop: false);
  ApplyLevels();
 }

 AudioSource Channel(string label, bool loop) {
  var child = new GameObject("Audio." + label);
  child.transform.SetParent(transform, false);
  var source = child.AddComponent<AudioSource>();
  source.playOnAwake = false;
  source.loop = loop;
  source.spatialBlend = 0f; // arayüz sesi; sahnede bir yerden gelmiyor
  return source;
 }

 // Ayar değişince çalan sesler de anında değişir; yeniden başlatma gerekmez.
 public void ApplyLevels() {
  if (music == null) return;
  music.volume = SoundSettings.MusicGain;
  ambience.volume = SoundSettings.MusicGain;
  effects.volume = SoundSettings.SfxGain;
  if (SoundSettings.MusicGain <= 0f) { music.Pause(); ambience.Pause(); }
  else {
   if (music.clip != null && !music.isPlaying) music.UnPause();
   if (ambience.clip != null && !ambience.isPlaying) ambience.UnPause();
  }
 }

 public void Play(string id) {
  if (effects == null || SoundSettings.SfxGain <= 0f) return;
  var clip = Clip(id);
  if (clip != null) effects.PlayOneShot(clip, SoundSettings.SfxGain);
 }

 public void PlayMusic(string id) => Loop(music, id, ref musicId);
 public void PlayAmbience(string id) => Loop(ambience, id, ref ambienceId);
 public void StopMusic() => Loop(music, null, ref musicId);

 void Loop(AudioSource channel, string id, ref string current) {
  if (channel == null || current == id) return;
  current = id;
  channel.Stop();
  channel.clip = string.IsNullOrEmpty(id) ? null : Clip(id);
  channel.volume = SoundSettings.MusicGain;
  if (channel.clip != null && SoundSettings.MusicGain > 0f) channel.Play();
 }

 AudioClip Clip(string id) {
  if (string.IsNullOrEmpty(id)) return null;
  if (cache.TryGetValue(id, out var clip)) return clip;
  clip = Resources.Load<AudioClip>(Folder + id);
  cache[id] = clip;
  if (clip == null && reported.Add(id))
   Debug.Log("Ses dosyası yok, sessiz geçiliyor: " + Folder + id);
  return clip;
 }
}
}
