using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Bube.Tests {

// Bölüm seçicinin kilit kuralı. Kilit oyuncuya bir şey söylemez, yalnız
// ilerlemeyi gösterir: dosyalar sırayla, ülkeler önceki ülke bitince açılır.
public sealed class WorldTests {

 static WorldCountry Country(string id, params string[] caseIds) =>
  new WorldCountry {
   id = id, nameKey = "world." + id + ".name",
   slots = caseIds.Select(caseId => new WorldSlot { caseId = caseId, titleKey = caseId + ".title" }).ToList(),
  };

 static WorldAtlas Atlas() => new WorldAtlas {
  countries = new List<WorldCountry> { Country("tr", "a", "b", "c"), Country("uk", "d", "e", "f") },
 };

 [Test]
 public void FirstCountryIsOpen_AndTheNextOneWaits() {
  var atlas = Atlas();
  var closed = new HashSet<string>();
  Assert.IsTrue(Worlds.CountryUnlocked(atlas, 0, closed), "İlk ülke her zaman açık olmalı.");
  Assert.IsFalse(Worlds.CountryUnlocked(atlas, 1, closed), "Sonraki ülke baştan açık olmamalı.");

  closed = new HashSet<string> { "a", "b", "c" };
  Assert.IsTrue(Worlds.Finished(atlas.countries[0], closed));
  Assert.IsTrue(Worlds.CountryUnlocked(atlas, 1, closed), "Önceki ülke bitince sonraki açılmalı.");
 }

 [Test]
 public void SlotsOpenInOrder() {
  var atlas = Atlas();
  var country = atlas.countries[0];
  var closed = new HashSet<string> { "a" };
  Assert.AreEqual(WorldSlotState.Completed, Worlds.SlotState(country, 0, closed, true));
  Assert.AreEqual(WorldSlotState.Active, Worlds.SlotState(country, 1, closed, true));
  Assert.AreEqual(WorldSlotState.Locked, Worlds.SlotState(country, 2, closed, true),
   "Aradaki dosya kapanmadan sonraki açılmamalı.");
  Assert.AreEqual(WorldSlotState.Locked, Worlds.SlotState(country, 1, closed, false),
   "Ülke kilitliyse dosya da kilitli olmalı.");
 }

 // Kapanmış dosya, ülke kilitli sayılsa bile tamamlanmış görünmelidir: kayıt
 // geçmişi silinmez.
 [Test]
 public void ClosedCaseStaysCompleted() {
  var country = Country("tr", "a", "b");
  var closed = new HashSet<string> { "a" };
  Assert.AreEqual(WorldSlotState.Completed, Worlds.SlotState(country, 0, closed, false));
 }

 // Yuvası olan ama henüz yazılmamış dosya kilitli değil, **yok**: oyuncuya
 // "kilidi aç" diye bir iş verilmemeli.
 [Test]
 public void EmptySlotIsUnwritten() {
  var country = new WorldCountry {
   id = "uk", slots = new List<WorldSlot> { new WorldSlot { caseId = "", titleKey = "x" } },
  };
  Assert.AreEqual(WorldSlotState.Unwritten, Worlds.SlotState(country, 0, new HashSet<string>(), true));
 }

 [Test]
 public void CountersMatchTheSlots() {
  var atlas = Atlas();
  var closed = new HashSet<string> { "a", "b" };
  Assert.AreEqual(6, Worlds.TotalCases(atlas));
  Assert.AreEqual(2, Worlds.TotalCompleted(atlas, closed));
  Assert.AreEqual(2, Worlds.CompletedIn(atlas.countries[0], closed));
 }

 // Seçici oyuncunun kaldığı yere açılır ama oraya **götürmez**.
 [Test]
 public void ResumeIsTheFirstUnfinishedOpenCountry() {
  var atlas = Atlas();
  Assert.AreEqual(0, Worlds.Resume(atlas, new HashSet<string>()));
  Assert.AreEqual(1, Worlds.Resume(atlas, new HashSet<string> { "a", "b", "c" }));
 }

 // Gerçek veri: ülke ve yuva sayıları ile ilk yuvanın oynanan vaka olması
 // `WorldRules` içinde denetlenir; burada dosyanın okunabildiği sınanır.
 [Test]
 public void ShippedAtlasLoads() {
  var atlas = Worlds.Load();
  Assert.Greater(atlas.countries.Count, 0, "Bölüm seçici verisi okunamadı.");
  // Türkiye 10 dosyalık öğretici bölüm; sonraki her ülke 7 dosya (5 Ekim 2026).
  Assert.AreEqual(10, atlas.countries[0].slots.Count, "Türkiye 10 dosya yuvası göstermeli.");
  foreach (var country in atlas.countries.Skip(1))
   Assert.AreEqual(7, country.slots.Count, "Ülke 7 dosya yuvası göstermeli: " + country.id);
 }
}
}
