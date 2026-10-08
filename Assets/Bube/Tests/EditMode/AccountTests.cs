using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {
// Hesap katmanı: eski kök kayıtların misafire taşınması, misafirin hesaba
// devri ve bulut birleştirmesinde eski kopyanın yeniyi ezmemesi.
public sealed class AccountTests {
 string dir; Func<string> keptBase;

 [SetUp] public void Sandbox() {
  dir = Path.Combine(Path.GetTempPath(), "karine-accounts-" + Guid.NewGuid().ToString("N"));
  Directory.CreateDirectory(dir);
  keptBase = Accounts.Base; Accounts.Base = () => dir;
 }
 [TearDown] public void Cleanup() { Accounts.Base = keptBase; Directory.Delete(dir, true); }

 [Test] public void LegacyRootSavesMoveIntoGuestFolder() {
  File.WriteAllText(Path.Combine(dir, "bube-career-v1.json"), "{}");
  Assert.AreEqual(1, Accounts.MigrateLegacy());
  Assert.IsTrue(File.Exists(Path.Combine(dir, "accounts", Accounts.GuestFolder, "bube-career-v1.json")));
  Assert.IsFalse(File.Exists(Path.Combine(dir, "bube-career-v1.json")));
 }

 [Test] public void AdoptNeverOverwritesTheAccountsOwnSave() {
  File.WriteAllText(Path.Combine(Accounts.FolderPath("guest"), "bube-a-v1.json"), "guest");
  File.WriteAllText(Path.Combine(Accounts.FolderPath("guest"), "bube-b-v1.json"), "guest");
  File.WriteAllText(Path.Combine(Accounts.FolderPath("p1"), "bube-a-v1.json"), "account");
  Accounts.Adopt("guest", "p1");
  Assert.AreEqual("account", File.ReadAllText(Path.Combine(dir, "accounts", "p1", "bube-a-v1.json")));
  Assert.AreEqual("guest", File.ReadAllText(Path.Combine(dir, "accounts", "p1", "bube-b-v1.json")));
 }

 [Test] public void MergeKeepsTheNewerCopy() {
  var path = Path.Combine(Accounts.Root, "bube-career-v1.json");
  File.WriteAllText(path, "local");
  File.SetLastWriteTimeUtc(path, new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc));
  string Entry(int day, string json) => JsonUtility.ToJson(new CloudEntry { ticks = new DateTime(2026, 10, day, 0, 0, 0, DateTimeKind.Utc).Ticks, json = json });
  Assert.AreEqual(0, Accounts.Merge(new Dictionary<string,string> { { "bube-career-v1", Entry(1, "old") } }));
  Assert.AreEqual("local", File.ReadAllText(path));
  Assert.AreEqual(1, Accounts.Merge(new Dictionary<string,string> { { "bube-career-v1", Entry(5, "cloud") } }));
  Assert.AreEqual("cloud", File.ReadAllText(path));
 }

 [Test] public void RegionPicksLanguageOnlyForKnownCountries() {
  Assert.AreEqual("tr", Languages.FromRegion("tr"));
  Assert.AreEqual("de", Languages.FromRegion("AT"));
  Assert.IsNull(Languages.FromRegion("JP"));
 }
}
}
