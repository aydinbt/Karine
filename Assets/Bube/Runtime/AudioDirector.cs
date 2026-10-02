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
 public const string Fax = "ui_fax";
 public const string Press        = "ui_press";
 public const string Typewriter   = "ui_typewriter";
 public const string Stamp        = "ui_stamp";
 public const string Notification = "ui_notification";
// Görüşmede cümle belirirken duyulan blip: hiçbir şeyin taklidi değil,
 // yalnızca "yeni bir satır geldi". İki varyant, çünkü tek klip tekrar
 // ederse konuşma değil sinyal olur.
 public static readonly string[] Chat = { "ui_chat", "ui_chat_low" };

 // Odanın dışından gelen seyrek sesler: uzak siren, köpek, telsiz cızırtısı.
 // Saatleri rastgeledir; oyunun hiçbir anına bağlı değildir.
 public static readonly string[] Distant = { "amb_siren", "amb_dog", "amb_radio", "amb_phone", "amb_typing" };

 AudioSource music, ambience, effects;
 // Konumlu sesler için küçük havuz: her biri kendi sol-sağ değerini taşır.
 readonly AudioSource[] placed = new AudioSource[4];
 int nextPlaced;
 Coroutine scatter;
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
  for (int i = 0; i < placed.Length; i++) placed[i] = Channel("Placed" + i, loop: false);
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

 public void Play(string id) => Play(id, 1f, 1f);

 // `pitch` konuşma için: tek klip, kişiye göre perde. `gain` bir sesin diğerine
 // göre ağırlığı (konuşma tıkırtıdan alçak durmalı). Perde paylaşılan kanalda
 // durduğu için her seferinde yazılır.
 public void Play(string id, float pitch, float gain) {
  if (effects == null || SoundSettings.SfxGain <= 0f) return;
  var clip = Clip(id);
  if (clip == null) return;
  effects.pitch = Mathf.Clamp(pitch, 0.5f, 2f);
  effects.PlayOneShot(clip, SoundSettings.SfxGain * Mathf.Clamp01(gain));
 }

 // Sesin masadaki yeri: -1 sol, 1 sağ. Faks tepsisi solda, monitör sağda duyulur.
 // `ambient` sesler müzik/ortam düzeyini izler, efekt düzeyini değil.
 public void PlayAt(string id, float pan, float gain, bool ambient = false) {
  float level = ambient ? SoundSettings.MusicGain : SoundSettings.SfxGain;
  if (effects == null || level <= 0f) return;
  var clip = Clip(id);
  if (clip == null) return;
  var source = placed[nextPlaced];
  nextPlaced = (nextPlaced + 1) % placed.Length;
  source.panStereo = Mathf.Clamp(pan, -1f, 1f);
  source.pitch = 1f;
  source.PlayOneShot(clip, level * Mathf.Clamp01(gain));
 }

 // Uzak sesler açılır ya da kapanır. Açıkken 25-70 saniyede bir, rastgele
 // bir yönden, alçak bir ses gelir.
 public void Scatter(bool on) {
  if (scatter != null) { StopCoroutine(scatter); scatter = null; }
  if (on && isActiveAndEnabled) scatter = StartCoroutine(ScatterLoop());
 }
 System.Collections.IEnumerator ScatterLoop() {
  while (true) {
   yield return new WaitForSecondsRealtime(Random.Range(25f, 70f));
   PlayAt(Distant[Random.Range(0, Distant.Length)], Random.Range(-.8f, .8f), .5f, ambient: true);
  }
 }

 public void PlayMusic(string id) => Loop(music, id, ref musicId);
 public void PlayAmbience(string id) => Loop(ambience, id, ref ambienceId);
 public void StopMusic() => Loop(music, null, ref musicId);

 void Loop(AudioSource channel, string id, ref string current) {
  if (channel == null || current == id) return;
  current = id;
  channel.Stop();
  var next = string.IsNullOrEmpty(id) ? null : Clip(id);
  // Akış klibi yüklenmeden `Play()` çağrılırsa ses **hiç** gelmez ve hata da
  // düşmez: ana menü müziğinin duyulmamasının sebebi buydu. İçe aktarım artık
  // önceden yüklüyor, bu satır da ikinci kapı.
  if (next != null && next.loadState != AudioDataLoadState.Loaded) next.LoadAudioData();
  channel.clip = next;
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
