using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bube.Editor {

// Metin anahtarı kuralları. Yinelenen anahtar sorundur: `Locale.Get` ilk girişi
// döndürür, yani ikinci çeviri sessizce yok sayılır. Ölü anahtar sorun değildir,
// not olarak listelenir — kod anahtarların bir kısmını çalışma anında birleştirerek
// ürettiği için (`"kind."+n.kind` gibi) bu liste kesin değildir.
public static class LocaleRules {
 // `T("bir.anahtar")` ve `locale.Get("bir.anahtar")`
 static readonly Regex ExactKey = new Regex(@"(?:\bT|\.Get)\(""([^""]+)""\)", RegexOptions.Compiled);
 // `T("kind."+n.kind)` — sabit önek kullanılıyor, tam anahtar kodda görünmüyor.
 static readonly Regex PrefixKey = new Regex(@"(?:\bT|\.Get)\(""([^""]+)""\s*\+", RegexOptions.Compiled);

 // Oyun içinde gerçek resmî kurum, kuruluş ve mevzuat adları **kullanılmaz**.
 // Kurgusal kurum "bube Polis / BPS"tir; kurumsal gönderici "ilgili birim" gibi
 // genel ifadelerle anılır. Bu liste yalnız tam sözcük eşleşmesine bakar, böylece
 // "birim" ya da "müdür" gibi genel sözcükler yanlış alarm üretmez.
 static readonly string[] ForbiddenInstitutions = {
  "emniyet", "emniyet müdürlüğü", "emniyet genel müdürlüğü", "egm",
  "jandarma", "jandarma genel komutanlığı", "polis merkezi", "karakol",
  "asayiş şube", "asayiş şubesi", "savcılık", "başsavcılık", "cumhuriyet savcısı",
  "adalet bakanlığı", "içişleri bakanlığı", "valilik", "kaymakamlık",
  "interpol", "europol", "fbi", "türk polis teşkilatı",
  "türk ceza kanunu", "tck", "cmk", "kvkk", "türkiye cumhuriyeti",
 };

 static readonly Regex WordBreak = new Regex(@"[^\p{L}\p{N}]+", RegexOptions.Compiled);

 // Davranış satırı dedektifin gördüğünü söyler; kişinin gizli durumunu
 // söylemez. Bu sözcükler oyuncunun kurması gereken çıkarımı hazır verir.
 static readonly HashSet<string> VerdictWords = new HashSet<string> {
  "yalan", "yalancı", "yalanladı", "çelişki", "çelişkili", "gerçeği", "doğruyu",
  "suçlu", "masum", "gizliyor", "saklıyor", "sakladığı", "uyduruyor", "samimi",
  "samimiyetsiz", "içten", "tedirginliği", "korkusu", "panikledi", "rahatlamış",
 };

 static string[] Words(string text) =>
  string.IsNullOrEmpty(text) ? new string[0]
   : WordBreak.Split(text.ToLowerInvariant()).Where(w => w.Length > 0).ToArray();

 // Tek dosyalık kısa yol: testler ve tek dilli doğrulama için.
 public static void Validate(Locale locale, IReadOnlyList<CaseData> cases, ValidationReport report) =>
  Validate(new[] { new KeyValuePair<string, Locale>("(metin)", locale) }, cases, report);

 // Metin artık birden çok dosyada: ortak `tr.json` ve vaka başına `tr.<vaka>.json`.
 // Her dosya kendi içinde, yinelenen anahtar ise **dosyalar arasında** da aranır —
 // bir vaka ortak bir metni sessizce değiştirmesin.
 public static void Validate(IReadOnlyList<KeyValuePair<string, Locale>> files,
  IReadOnlyList<CaseData> cases, ValidationReport report) {
  report.Scope("metin");
  if (!report.Step(files != null && files.Count > 0 && files[0].Value?.entries != null,
   "Metin dosyası okunamadı.")) return;

  var seen = new HashSet<string>();
  var owner = new Dictionary<string, string>();
  foreach (var file in files)
  foreach (var entry in file.Value?.entries ?? new Entry[0]) {
   if (entry == null || string.IsNullOrEmpty(entry.key)) { report.Problem("Anahtarı olmayan metin girişi var (" + file.Key + ")."); continue; }
   if (!seen.Add(entry.key))
    report.Problem("Yinelenen anahtar (ikinci değer sessizce yok sayılır): " + entry.key +
     " — " + owner[entry.key] + " ve " + file.Key);
   else owner[entry.key] = file.Key;
   var institution = ForbiddenInstitution(entry.value);
   if (institution != null)
    report.Problem("Gerçek kurum/mevzuat adı kullanılamaz (\"" + institution + "\"): " + entry.key);
   if (entry.key.EndsWith(".demeanor")) {
    var verdict = Words(entry.value).FirstOrDefault(VerdictWords.Contains);
    if (verdict != null)
     report.Problem("Davranış satırı yorum yapıyor, yalnız gözlem olmalı (\"" + verdict + "\"): " + entry.key);
    if (Words(entry.value).Length > 16)
     report.Problem("Davranış satırı uzun; yanıtı gölgede bırakır: " + entry.key);
   }
  }

  var used = new HashSet<string>();
  var prefixes = new HashSet<string>();
  foreach (var file in SourceFiles()) {
   var text = File.ReadAllText(file);
   foreach (Match match in ExactKey.Matches(text)) used.Add(match.Groups[1].Value);
   foreach (Match match in PrefixKey.Matches(text)) prefixes.Add(match.Groups[1].Value);
  }
  foreach (var data in cases) {
   CollectKeys(data, used);
   foreach (var node in data.nodes ?? new Node[0])
    if (!string.IsNullOrEmpty(node.kind)) used.Add("kind." + node.kind);
  }

  var dead = seen
   .Where(key => !used.Contains(key) && !prefixes.Any(prefix => key.StartsWith(prefix, StringComparison.Ordinal)))
   .OrderBy(key => key).ToArray();
  if (dead.Length > 0)
   report.Note(dead.Length + " anahtar hiçbir yerde kullanılmıyor görünüyor (çalışma anında birleştirilen " +
    "anahtarlar bu listeye yanlışlıkla girebilir): " + string.Join(", ", dead));
 }

 static IEnumerable<string> SourceFiles() =>
  Directory.Exists("Assets/Bube")
   ? Directory.GetFiles("Assets/Bube", "*.cs", SearchOption.AllDirectories).Where(path => !path.Contains("Tests"))
   : Enumerable.Empty<string>();

 // Vaka verisindeki anahtarlar yansımayla toplanır: adı `Key` ile biten her string
 // alan bir metin anahtarıdır. Böylece veri şemasına yeni bir alan eklendiğinde
 // bu kontrol kendiliğinden kapsar.
 static void CollectKeys(object value, HashSet<string> into) {
  if (value == null) return;
  if (value is IEnumerable sequence && !(value is string)) {
   foreach (var item in sequence) CollectKeys(item, into);
   return;
  }
  var type = value.GetType();
  if (type.IsPrimitive || value is string || !type.FullName.StartsWith("Bube.", StringComparison.Ordinal)) return;
  foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance)) {
   var content = field.GetValue(value);
   if (field.FieldType == typeof(string)) {
    if (field.Name.EndsWith("Key", StringComparison.Ordinal) && !string.IsNullOrEmpty((string)content))
     into.Add((string)content);
   } else CollectKeys(content, into);
  }
 }
 // Oyuncuya görünen metinde yasaklı kurum adı var mı? Tam sözcük dizisi arar;
 // Türkçe büyük/küçük harf farkı ve noktalama önemsizdir.
 public static string ForbiddenInstitution(string text) {
  if (string.IsNullOrEmpty(text)) return null;
  var words = WordBreak.Split(text.ToLowerInvariant()).Where(word => word.Length > 0).ToArray();
  var normalized = " " + string.Join(" ", words) + " ";
  foreach (var name in ForbiddenInstitutions)
   if (normalized.Contains(" " + name + " ")) return name;
  return null;
 }

}
}
