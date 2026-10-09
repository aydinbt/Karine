using NUnit.Framework;
namespace Bube.Tests {
public class RemoteSettingsTests {
 [Test]
 public void VersionsCompareNumerically() {
  Assert.IsTrue(RemoteSettings.IsOlder("1.0.9", "1.0.10"));
  Assert.IsFalse(RemoteSettings.IsOlder("1.0.10", "1.0.9"));
  Assert.IsFalse(RemoteSettings.IsOlder("1.0", "1.0.0"));
  Assert.IsFalse(RemoteSettings.IsOlder("1.0", ""), "Boş en düşük sürüm hiçbir şeyi kapatmamalı.");
 }
 [Test]
 public void PausedCasesAreReadFromList() {
  RemoteSettings.Apply(new RemoteSettings.Snapshot { pausedCases = "case002, case014" });
  try {
   Assert.IsTrue(RemoteSettings.CasePaused("case014"));
   Assert.IsFalse(RemoteSettings.CasePaused("case001"));
  } finally { RemoteSettings.Apply(new RemoteSettings.Snapshot()); }
 }
}
}
