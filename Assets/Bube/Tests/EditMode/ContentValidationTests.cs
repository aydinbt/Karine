using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// Faz 0 dumanı: test assembly'sinin gerçekten derlendiğini ve var olan
// içerik doğrulayıcısının test koşucusundan çalıştırılabildiğini kanıtlar.
// Ayrıntılı şema/erişilebilirlik/kayıt testleri Faz 1'in işidir.
public sealed class ContentValidationTests {

 [Test]
 public void ValidateContent_Passes() {
  Assert.DoesNotThrow(() => Bube.Editor.ProjectSetup.Validate(),
   "Bube/Validate Content doğrulayıcısı hata verdi.");
 }

 [Test]
 public void Config_NamesTheGame() {
  var asset = Resources.Load<TextAsset>("Bube/config");
  Assert.IsNotNull(asset, "Bube/config.json bulunamadı.");
  var config = JsonUtility.FromJson<GameConfig>(asset.text);
  Assert.AreEqual("Karine", config.title);
  Assert.AreEqual("tr", config.locale);
  Assert.IsFalse(string.IsNullOrEmpty(config.initialCase));
 }

 [Test]
 public void Locale_HasNoDuplicateKeys() {
  var asset = Resources.Load<TextAsset>("Bube/Locales/tr");
  Assert.IsNotNull(asset, "tr.json bulunamadı.");
  var locale = JsonUtility.FromJson<Locale>(asset.text);
  var seen = new System.Collections.Generic.HashSet<string>();
  foreach (var entry in locale.entries)
   Assert.IsTrue(seen.Add(entry.key), "Yinelenen metin anahtarı: " + entry.key);
 }
}
}
