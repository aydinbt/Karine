using UnityEngine;

namespace Bube {
// Karışım (AD): gerçek müzik dosyası varsa sentez katmanın yerine geçer, oda
// başına ayak sesi tınısı, okurken ortamın kısılması, kulaklıkta geniş stereo ve
// seslendirme kancası. Hiçbiri soruşturmanın durumunu okumaz.
public sealed partial class AudioDirector {
 // Bestelenmiş parça `Resources/Bube/Music/<ad>` altına konursa sentez yerine o çalar.
 public const string MusicFolder = "Bube/Music/";
 // Seslendirme: `Resources/Bube/Voice/<cümle anahtarı>`; yoksa sessiz geçer, not düşmez.
 public const string VoiceFolder = "Bube/Voice/";
 const float DuckGain = .45f, DuckSeconds = .4f;
 float duck = 1f, duckTarget = 1f;
 AudioSource voice;
 static int headphones = -1;
 static float headphonesCheckedAt = -100f;

 AudioClip Composed(string id) {
  var clip = Resources.Load<AudioClip>(MusicFolder + id);
  return clip;
 }

 // Okunan bir belge açıkken ortam ve katman kısılır; kapanınca geri gelir.
 public void Duck(bool on) => duckTarget = on ? DuckGain : 1f;
 void StepDuck() {
  if (Mathf.Approximately(duck, duckTarget)) return;
  duck = Mathf.MoveTowards(duck, duckTarget, Time.unscaledDeltaTime / DuckSeconds);
  if (ambience != null) ambience.volume = SoundSettings.MusicGain * duck;
  LevelSpace();
 }

 // Kulaklık: kablolu, USB ya da Bluetooth çıkışı. Android'de AudioManager'a sorulur,
 // 5 saniyede bir yenilenir. Editor'de ve başka platformda kapalı sayılır.
 public static bool Headphones {
  get {
   if (Time.unscaledTime - headphonesCheckedAt < 5f && headphones >= 0) return headphones == 1;
   headphonesCheckedAt = Time.unscaledTime; headphones = 0;
#if UNITY_ANDROID && !UNITY_EDITOR
   try {
    using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
    using (var manager = activity.Call<AndroidJavaObject>("getSystemService", "audio")) {
     var devices = manager.Call<AndroidJavaObject[]>("getDevices", 2);
     foreach (var device in devices) {
      int type = device.Call<int>("getType");
      if (type == 3 || type == 4 || type == 8 || type == 22 || type == 26) { headphones = 1; break; }
     }
    }
   } catch (System.Exception) { headphones = 0; }
#endif
   return headphones == 1;
  }
 }
 static float Widen(float pan) => Headphones ? Mathf.Clamp(pan * 1.5f, -1f, 1f) : pan;

 // Seslendirme kancası: dosya varsa oynar, yoksa hiçbir şey.
 public void Voice(string key) {
  if (string.IsNullOrEmpty(key) || SoundSettings.SfxGain <= 0f) return;
  var clip = Resources.Load<AudioClip>(VoiceFolder + key);
  if (clip == null) return;
  if (voice == null) voice = Channel("Voice", loop: false);
  voice.Stop(); voice.clip = clip; voice.volume = SoundSettings.SfxGain; voice.Play();
 }
 public void StopVoice() { if (voice != null) voice.Stop(); }

 // Oda başına ayak sesi: görüşme odasında fayans, ofiste koridor.
 static string StepsFor(string kind) => kind == "interview" ? "amb_steps_tile" : "amb_steps";
}
}
