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

 // Oynanan dil: seçilen dilde eksik bir satır önce İngilizceye, o da yoksa
 // kanon Türkçeye düşer. Yarım bir çeviri oyuncuya "[anahtar]" göstermez.
 public static Locale LoadPlayable(string code) {
  Locale result=null;
  foreach(var c in new[]{code,Languages.Fallback,Languages.Canon}.Distinct()) {
   var next=Load(c);
   if(next==null)continue;
   if(result==null)result=next;else result.Absorb(next);
  }
  return result;
 }

 static Locale Parse(TextAsset asset) =>
  asset == null ? null : JsonUtility.FromJson<Locale>(asset.text);
}

// Desteklenen diller. Türkçe kanondur: kural metni (ad eşleşmesi) hep oradan
// okunur. Ayarlarda yalnız ortak dosyası bulunan diller listelenir.
public static class Languages {
 public const string Canon = "tr";
 public const string Fallback = "en";
 public const string PrefKey = "karine.language";
 public static readonly string[] All = {"tr","en","de","fr","it","es","pt-BR"};
 // Dil adı her zaman kendi dilinde yazılır; oyuncu tanımadığı dilde kendi dilini bulabilsin.
 public static string Endonym(string code) {
  switch(code) {
   case "tr": return "Türkçe"; case "en": return "English"; case "de": return "Deutsch";
   case "fr": return "Français"; case "it": return "Italiano"; case "es": return "Español";
   case "pt-BR": return "Português (Brasil)"; default: return code;
  }
 }
 public static bool Installed(string code) => Resources.Load<TextAsset>(LocaleLoader.Folder + code) != null;
 public static string[] Available() => All.Where(Installed).ToArray();
 public static string FromSystem(SystemLanguage language) {
  switch(language) {
   case SystemLanguage.Turkish: return "tr"; case SystemLanguage.German: return "de";
   case SystemLanguage.French: return "fr"; case SystemLanguage.Italian: return "it";
   case SystemLanguage.Spanish: return "es"; case SystemLanguage.Portuguese: return "pt-BR";
   default: return Fallback;
  }
 }
 // Cihaz dili desteklenmiyorsa ülkeye bakılır: dili Kürtçe ya da Azerice olan
 // Türkiye'deki oyuncu Türkçe, Avusturya'daki Almanca görür. Dil yine önce gelir.
 public static string FromRegion(string region) {
  switch((region??"").ToUpperInvariant()) {
   case "TR": case "CY": return "tr"; case "DE": case "AT": case "LI": return "de";
   case "FR": case "MC": return "fr"; case "IT": case "SM": return "it";
   case "ES": case "MX": case "AR": case "CO": case "CL": case "PE": return "es"; case "BR": return "pt-BR";
   default: return null;
  }
 }
 static string DeviceRegion() {
  try { return System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName; } catch { return null; }
 }
 // Kayıtlı seçim → cihaz dili → ülke → İngilizce. Kurulu olmayan dil seçilmez.
 public static string Current() {
  var saved = PlayerPrefs.GetString(PrefKey, "");
  if(Installed(saved)) return saved;
  var device = FromSystem(Application.systemLanguage);
  if(Installed(device) && (device!=Fallback || Application.systemLanguage==SystemLanguage.English)) return device;
  var region = FromRegion(DeviceRegion());
  if(Installed(region)) return region;
  return Installed(Fallback) ? Fallback : Canon;
 }
}
}
