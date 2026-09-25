using System.Linq;
using UnityEngine;

namespace Bube {

// Dil metni tek dosyada başladı; vaka sayısı arttıkça o dosya her vakanın
// metnini taşıyan paylaşılan bir dosyaya dönüşüyordu. Artık ortak metin
// `Bube/Locales/<dil>.json`da, vaka metni ise `Bube/Locales/<dil>.<vaka>.json`
// dosyalarında durur ve yüklemede birleştirilir.
//
// Böylece yeni vaka eklemek paylaşılan dosyaya dokunmaz: kendi JSON'u, kendi
// dil dosyası, kendi varlıkları. Çakışma olursa **ortak dosya kazanır** —
// bir vaka ortak bir anahtarı sessizce değiştirmesin.
public static class LocaleLoader {

 public const string Folder = "Bube/Locales/";

 public static Locale Load(string code) {
  var shared = Parse(Resources.Load<TextAsset>(Folder + code));
  if (shared == null) return null;
  foreach (var asset in CaseAssets(code)) shared.Absorb(Parse(asset));
  return shared;
 }

 // `tr.case001` gibi adlar; `tr` ile `tr-TR`i karıştırmamak için nokta aranır.
 public static TextAsset[] CaseAssets(string code) =>
  Resources.LoadAll<TextAsset>("Bube/Locales")
   .Where(asset => asset.name.StartsWith(code + ".", System.StringComparison.Ordinal))
   .OrderBy(asset => asset.name, System.StringComparer.Ordinal)
   .ToArray();

 static Locale Parse(TextAsset asset) =>
  asset == null ? null : JsonUtility.FromJson<Locale>(asset.text);
}
}
