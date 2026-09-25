using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bube {

// Bölüm seçicinin verisi. Ülke ve dosya listesi koda yazılmaz: yeni bir ülke
// ya da yeni bir dosya `Bube/Worlds.json`a satır eklemekle gelir.
[Serializable] public class WorldSlot {
 public string caseId;    // boş: dosya henüz yazılmadı (yer tutucu)
 public string titleKey;
 public string image;     // boş: görsel yok, kart yazıyla durur
}

[Serializable] public class WorldCountry {
 public string id;
 public string nameKey;
 public string cityKey;
 public string descriptionKey;
 public string image;
 public List<WorldSlot> slots = new List<WorldSlot>();
}

[Serializable] public class WorldAtlas {
 public List<WorldCountry> countries = new List<WorldCountry>();
}

// Dosyanın oyuncu açısından durumu. Tek yerde türetilir; ekran bunu yalnız
// boyar. Kilidin sebebi **ilerleme**dir, gizli bir oyun durumu değil.
public enum WorldSlotState { Completed, Active, Locked, Unwritten }

public static class Worlds {
 public const string Resource = "Bube/Worlds";

 public static WorldAtlas Load() {
  var asset = Resources.Load<TextAsset>(Resource);
  if (asset == null) return new WorldAtlas();
  var atlas = JsonUtility.FromJson<WorldAtlas>(asset.text);
  return atlas ?? new WorldAtlas();
 }

 // Kapanmış dosyalar: kariyer geçmişi neyi kapattıysa o tamamlanmıştır.
 public static HashSet<string> Closed(CareerProgress career) =>
  career == null || career.reviewHistory == null
   ? new HashSet<string>()
   : new HashSet<string>(career.reviewHistory.Select(review => review.caseId));

 public static int CompletedIn(WorldCountry country, ICollection<string> closed) =>
  country == null ? 0 : country.slots.Count(slot => !string.IsNullOrEmpty(slot.caseId) && closed.Contains(slot.caseId));

 public static bool Finished(WorldCountry country, ICollection<string> closed) =>
  country != null && country.slots.Count > 0 && CompletedIn(country, closed) == country.slots.Count;

 // Ülke sırası: ilki her zaman açık, sonraki ancak önceki bitince açılır.
 public static bool CountryUnlocked(WorldAtlas atlas, int index, ICollection<string> closed) {
  if (atlas == null || index < 0 || index >= atlas.countries.Count) return false;
  for (int earlier = 0; earlier < index; earlier++)
   if (!Finished(atlas.countries[earlier], closed)) return false;
  return true;
 }

 public static WorldSlotState SlotState(WorldCountry country, int index, ICollection<string> closed, bool countryUnlocked) {
  if (country == null || index < 0 || index >= country.slots.Count) return WorldSlotState.Locked;
  var slot = country.slots[index];
  if (!string.IsNullOrEmpty(slot.caseId) && closed.Contains(slot.caseId)) return WorldSlotState.Completed;
  if (!countryUnlocked) return WorldSlotState.Locked;
  for (int earlier = 0; earlier < index; earlier++) {
   var before = country.slots[earlier];
   if (string.IsNullOrEmpty(before.caseId) || !closed.Contains(before.caseId)) return WorldSlotState.Locked;
  }
  return string.IsNullOrEmpty(slot.caseId) ? WorldSlotState.Unwritten : WorldSlotState.Active;
 }

 public static int TotalCases(WorldAtlas atlas) =>
  atlas == null ? 0 : atlas.countries.Sum(country => country.slots.Count);

 public static int TotalCompleted(WorldAtlas atlas, ICollection<string> closed) =>
  atlas == null ? 0 : atlas.countries.Sum(country => CompletedIn(country, closed));

 // Oyuncunun kaldığı ülke: bitmemiş ilk açık ülke. Seçici oraya açılır ama
 // oyuncuyu hiçbir yere götürmez — liste elle gezilir.
 public static int Resume(WorldAtlas atlas, ICollection<string> closed) {
  if (atlas == null) return 0;
  for (int index = 0; index < atlas.countries.Count; index++)
   if (CountryUnlocked(atlas, index, closed) && !Finished(atlas.countries[index], closed)) return index;
  return 0;
 }
}
}
