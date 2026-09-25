using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Marka kimliği: tek logo varlığı, sabit oran, sabit doku yoğunluğu.
public sealed class BrandTests {

 [Test]
 public void LogoAsset_ExistsAndMatchesDeclaredAspect() {
  var art = Resources.Load<Texture2D>(KarineLogo.BaseResource);
  Assert.IsNotNull(art, "KARINE logosu Resources altında bulunamadı: " + KarineLogo.BaseResource);
  Assert.AreEqual(KarineLogo.AspectRatio, (float)art.width / art.height, 0.01f,
   "Logo oranı koddaki sabitle uyuşmuyor; esnetme buradan başlar.");
 }

 [Test]
 public void Logo_KeepsAspectAtEverySize() {
  // Ana menü (büyük), başlık (kompakt) ve dar bir telefon genişliği.
  foreach (var width in new[] { 380f, 190f, 96f }) {
   var holder = new VisualElement();
   var logo = KarineLogo.Build(holder, width, Color.white);
   Assert.AreEqual(width, logo.style.width.value.value, 0.01f);
   Assert.AreEqual(width / KarineLogo.AspectRatio, logo.style.height.value.value, 0.01f,
    "Yükseklik genişlikten türemeli; sabit bir yükseklik esnetme demektir.");
  }
 }

 [Test]
 public void Logo_HasTwoNamedLayers() {
  var logo = KarineLogo.Build(new VisualElement(), 200f, Color.white);
  Assert.AreEqual(2, logo.childCount, "Yapı: LogoBase + DistressOverlay.");
  Assert.AreEqual("LogoBase", logo[0].name);
  Assert.AreEqual("DistressOverlay", logo[1].name);
 }

 // Doku yoğunluğu ekranlar arasında değişmemeli: aynı çağrı her boyda aynı
 // saydamlığı vermeli.
 [Test]
 public void DistressOpacity_DoesNotVaryWithSize() {
  var small = KarineLogo.Build(new VisualElement(), 96f, Color.white)[1].style.opacity.value;
  var large = KarineLogo.Build(new VisualElement(), 380f, Color.white)[1].style.opacity.value;
  Assert.AreEqual(small, large, 0.001f);
 }

 [Test]
 public void FontRoles_AlwaysResolveEvenWhenFilesAreMissing() {
  var set = FontSet.Load();
  Assert.IsNotNull(set.Mono, "IBM Plex Mono projede olmalı.");
  foreach (var font in new[] { set.Heading, set.Body, set.BodyBold, set.MonoBold })
   Assert.IsNotNull(font, "Her rol bir yazı tipine düşmeli; ekran yazısız kalmamalı.");
 }
}
}
