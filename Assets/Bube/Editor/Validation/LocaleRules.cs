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

 public static void Validate(Locale locale, IReadOnlyList<CaseData> cases, ValidationReport report) {
  report.Scope("metin");
  if (!report.Step(locale?.entries != null, "Metin dosyası okunamadı.")) return;

  var seen = new HashSet<string>();
  foreach (var entry in locale.entries) {
   if (entry == null || string.IsNullOrEmpty(entry.key)) { report.Problem("Anahtarı olmayan metin girişi var."); continue; }
   if (!seen.Add(entry.key))
    report.Problem("Yinelenen anahtar (ikinci değer sessizce yok sayılır): " + entry.key);
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
}
}
