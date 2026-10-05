using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Bölüm seçicinin verisi: ülkeler, dosya yuvaları ve bunların metin anahtarları.
// Ekran veriden çizildiği için eksik bir anahtar ya da yanlış bir vaka kimliği
// yalnız oyun açılınca görülür; kural onu derlemeden önce görür.
public static class WorldRules {
 public static void Validate(ValidationReport report, Locale locale) {
  report.Scope("bölüm seçici");
  var asset = Resources.Load<TextAsset>(Worlds.Resource);
  if (asset == null) { report.Problem("Bölüm seçici verisi yok: " + Worlds.Resource); return; }

  var atlas = Worlds.Load();
  if (atlas.countries.Count == 0) { report.Problem("Bölüm seçicide hiç ülke yok."); return; }

  var ids = new HashSet<string>();
  var cases = new HashSet<string>();
  int slots = 0;
  foreach (var country in atlas.countries) {
   report.Require(!string.IsNullOrEmpty(country.id), "Ülkenin kimliği boş.");
   if (!ids.Add(country.id)) report.Problem("Ülke kimliği iki kez geçiyor: " + country.id);
   foreach (var key in new[] { country.nameKey, country.cityKey, country.descriptionKey })
    report.Require(locale.Has(key), "Ülke metni yok (" + country.id + "): " + key);
   // İlk ülke (Türkiye) 10 dosyalık öğretici bölümdür; sonraki ülkelerin yuva
   // sayısı birbirine eşittir (5 Ekim 2026 kullanıcı kararı: 7).
   if (country != atlas.countries[0] && slots == 0) slots = country.slots.Count;
   report.Require(country == atlas.countries[0] || country.slots.Count == slots,
    "Ülkenin dosya yuvası sayısı farklı (" + country.id + "): " + country.slots.Count + " ≠ " + slots);
   foreach (var slot in country.slots) {
    report.Require(locale.Has(slot.titleKey), "Dosya başlığı yok (" + country.id + "): " + slot.titleKey);
    if (string.IsNullOrEmpty(slot.caseId)) continue;
    if (!cases.Add(slot.caseId)) report.Problem("Aynı vaka iki yuvada: " + slot.caseId);
    report.Require(File.Exists("Assets/Bube/Resources/Bube/Cases/" + slot.caseId + ".json"),
     "Yuvada olmayan vaka kimliği (" + country.id + "): " + slot.caseId);
   }
  }

  // Oyunun başladığı vaka seçicinin ilk yuvasında durmalı, yoksa oyuncu
  // oynadığı dosyayı listede bulamaz.
  var config = JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text);
  var first = atlas.countries[0].slots.FirstOrDefault();
  report.Require(first != null && first.caseId == config.initialCase,
   "İlk yuva `config.initialCase` ile aynı olmalı: " + (first == null ? "(yok)" : first.caseId) +
   " ≠ " + config.initialCase);

  // Yazılmış her vaka seçicide bir yere oturmalı: listede olmayan vaka
  // oyuncunun asla göremeyeceği vakadır.
  foreach (var path in Directory.GetFiles("Assets/Bube/Resources/Bube/Cases", "*.json"))
   report.Require(cases.Contains(Path.GetFileNameWithoutExtension(path)),
    "Vaka bölüm seçicide yok: " + Path.GetFileNameWithoutExtension(path));
 }
}
}
