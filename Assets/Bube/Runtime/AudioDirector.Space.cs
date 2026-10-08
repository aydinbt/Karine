using System;
using UnityEngine;

namespace Bube {
// Sesin mekânı: odaya göre yankı, uzak seslerin boğukluğu ve yönü, masada
// süreyle açılan müzik katmanı, görüşme odasının havalandırması ve vaka
// motifi. Hiçbiri oyunun durumuna bağlı değil: katman yalnız geçen süreyle
// açılır, soruşturmanın ilerleyişiyle değil.
public sealed partial class AudioDirector {
 // Ortam sesi çaldığında altyazı isteyen dinler (ses betimlemesi).
 public static event Action<string> Heard;
 public static readonly string[] InterviewDistant = { StepsFor("interview") };
 string[] distantSet = Distant;
 AudioSource layer, bed;
 float layerSince = -1f;
 const float LayerRampSeconds = 240f, LayerMax = .55f, BedGain = .5f;

 void WakeSpace() {
  layer = Channel("Layer", loop: true);
  bed = Channel("Bed", loop: true);
 }

 // Uzaktaki ses boğuk gelir; yakın arayüz sesi açık. Kulaklıkta yön belirgin olsun
 // diye uzak sesin sol-sağ değeri biraz genişletilir.
 void Distance(AudioSource source, bool far) {
  // Unity bileşeninde `??` çalışmaz (yok edilmiş nesne null sayılmaz); açık karşılaştırma.
  var filter = source.GetComponent<AudioLowPassFilter>();
  if (filter == null) filter = source.gameObject.AddComponent<AudioLowPassFilter>();
  filter.cutoffFrequency = far ? 2400f : 22000f;
  if (far) source.panStereo = Mathf.Clamp(source.panStereo * 1.25f, -1f, 1f);
 }

 // Oda: "office", "interview" ya da "menu". Yankı, uzak ses listesi, havalandırma, katman.
 public void Room(string kind) {
  if (effects == null) return;
  var preset = kind == "interview" ? AudioReverbPreset.Room : kind == "office" ? AudioReverbPreset.Livingroom : AudioReverbPreset.Off;
  foreach (var source in placed) Reverb(source, kind == "office" ? AudioReverbPreset.Hallway : preset);
  Reverb(effects, preset);
  distantSet = kind == "interview" ? InterviewDistant : Distant;
  SetLoop(bed, kind == "interview" ? "amb_vent" : null);
  SetLoop(layer, kind == "office" ? "desk_layer" : null);
  layerSince = kind == "office" ? Time.unscaledTime : -1f;
  LevelSpace();
 }
 static void Reverb(AudioSource source, AudioReverbPreset preset) {
  var filter = source.GetComponent<AudioReverbFilter>();
  if (preset == AudioReverbPreset.Off) { if (filter != null) filter.enabled = false; return; }
  if (filter == null) filter = source.gameObject.AddComponent<AudioReverbFilter>();
  filter.enabled = true; filter.reverbPreset = preset;
 }
 void SetLoop(AudioSource channel, string id) {
  var clip = string.IsNullOrEmpty(id) ? null : Clip(id);
  if (channel.clip == clip && (clip == null || channel.isPlaying)) return;
  channel.Stop(); channel.clip = clip;
  if (clip != null) { if (clip.loadState != AudioDataLoadState.Loaded) clip.LoadAudioData(); channel.Play(); }
 }

 void LevelSpace() {
  if (bed == null) return;
  bed.volume = SoundSettings.MusicGain * BedGain * Bed;
  float ramp = layerSince < 0 ? 0 : Mathf.Clamp01((Time.unscaledTime - layerSince) / LayerRampSeconds);
  layer.volume = SoundSettings.MusicGain * LayerMax * ramp * ramp * Bed;
 }

 void Update() { StepDuck(); if (layerSince >= 0) LevelSpace(); }

 // Vaka motifi: açılış kartında ve dosya kapandığında aynı üç nota.
 public void Sting() => Play("case_sting", 1f, .9f);
}
}
