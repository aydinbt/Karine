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

  // Ödüllü ipucu ekranının metni **vakanın gerçeğinden** bir şey söylemez.
  // Kural elle korunamaz, çünkü ileride biri iyi niyetle "Hasan'ın ifadesine
  // bak" yazar ve oyunun çekirdeği gider. Bu yüzden `guidance.` ile başlayan
  // her metinde hiçbir kişi adı, kaynak başlığı ve karar etiketi geçmemeli.
  ValidateGuidance(files, cases, report);

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

 // Yasak olan şeyler vakanın kendi metninden **türetilir**, elle listelenmez.
 // İki ayrı biçimde aranır, çünkü riskleri farklıdır:
 //  · kişi adı — tek sözcük yeter ("Hasan"), o yüzden büyük harfle başlayan
 //    adlar ayrı ayrı yasaklanır;
 //  · kaynak başlığı ve karar etiketi — bunlar "yöntem", "kayıt" gibi sıradan
 //    sözcükler içerir, o yüzden yalnız **tam ifade** olarak aranır. Aksi hâlde
 //    kural her masum cümlede yanlış alarm verir.
 static void ValidateGuidance(IReadOnlyList<KeyValuePair<string, Locale>> files,
  IReadOnlyList<CaseData> cases, ValidationReport report) {
  var merged = new Dictionary<string, string>();
  foreach (var file in files)
  foreach (var entry in file.Value?.entries ?? new Entry[0])
   if (entry?.key != null && !merged.ContainsKey(entry.key)) merged[entry.key] = entry.value;

  var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
  var phrases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
  foreach (var data in cases) {
   foreach (var node in data.nodes ?? new Node[0]) {
    foreach (var word in Value(merged, node.personNameKey).Split(' '))
     if (word.Length > 2 && char.IsUpper(word[0])) names.Add(word.Trim('.', ',', ':', '—', '-'));
    Add(phrases, Value(merged, node.titleKey));
   }
   foreach (var verdict in data.verdicts ?? new Verdict[0]) Add(phrases, Value(merged, verdict.labelKey));
   foreach (var choice in (data.methods ?? new Choice[0]).Concat(data.evidence ?? new Choice[0]))
    Add(phrases, Value(merged, choice.labelKey));
  }

  foreach (var pair in merged) {
   if (!pair.Key.StartsWith("guidance.", StringComparison.Ordinal)) continue;
   var text = pair.Value ?? string.Empty;
   var words = new HashSet<string>(Words(text));
   var leak = names.FirstOrDefault(name => words.Contains(name.ToLowerInvariant()))
    ?? phrases.FirstOrDefault(phrase => text.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0);
   if (leak != null)
    report.Problem("İpucu metni vakadan bir şey söylüyor (\"" + leak + "\"): " + pair.Key +
     ". İpucu yalnız yöntemi ve oyuncunun kendi kapsamını anlatır.");
  }
 }

 static void Add(HashSet<string> into, string value) {
  // Tek sözcüklü etiketler ("Kayıt") tam ifade olarak da çok geneldir; onlar
  // kişi adı değilse aranmaz.
  if (!string.IsNullOrEmpty(value) && value.Trim().Contains(" ")) into.Add(value.Trim());
 }

 static string Value(Dictionary<string, string> merged, string key) =>
  !string.IsNullOrEmpty(key) && merged.TryGetValue(key, out var value) && value != null ? value : string.Empty;

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
