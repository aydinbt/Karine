using System.IO;
using System.Text.RegularExpressions;
using Bube;
using NUnit.Framework;

public class TypographyTests {

 [Test]
 public void Snap_LandsOnTheScale_AndIsIdempotent() {
  for (int size = 8; size <= 90; size++) {
   int snapped = Typography.Snap(size);
   Assert.Contains(snapped, Typography.Steps, size + " ölçek dışı bir değere oturdu.");
   Assert.AreEqual(snapped, Typography.Snap(snapped), "Snap eşgüçlü değil: " + size);
  }
 }

 // Eşit uzaklıkta büyük basamak seçilir; telefonda okunaklılık önceliklidir.
 [Test]
 public void Snap_PrefersTheLargerStepOnATie() {
  Assert.AreEqual(15, Typography.Snap(14));
  Assert.AreEqual(17, Typography.Snap(16));
  Assert.AreEqual(19, Typography.Snap(18));
  Assert.AreEqual(21, Typography.Snap(20));
 }

 // Asıl kural bu: arayüzde ölçeği atlayan çıplak punto kalmamalı. `Text`/`Button`
 // yardımcıları boyutu zaten `Snap`'ten geçirir, doğrudan atamalar da geçmeli.
 [Test]
 public void NoScreenSetsARawFontSize() {
  var source = File.ReadAllText("Assets/Bube/Runtime/BubeApp.cs");
  var raw = Regex.Matches(source, @"style\.fontSize=\d+");
  Assert.AreEqual(0, raw.Count,
   "Ölçek dışı punto: " + string.Join(", ", System.Linq.Enumerable.Select(
    System.Linq.Enumerable.Cast<Match>(raw), m => m.Value)));
 }

}
