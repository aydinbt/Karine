using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Bube.Editor {

// Çeviri kuralları. Türkçe kanondur; her çeviri Türkçede geçerli olan kuralların
// **aynısına** uyar, oyunu dile göre değiştirmez:
//  · kanonda olmayan anahtar yoktur (yazım hatası sessizce ölü metin olur);
//  · `{0}` gibi yer tutucular aynı kalır;
//  · kişi adları değişmez ve bir metin kanonda hangi kişiyi anıyorsa çeviride
//    de yalnız onu anar — "kayıt ancak adı geçiyorsa öne sürülür" kuralı
//    kanondan hesaplanır, ekranda gördüğü metin de oyuncuya aynı şeyi söylemeli;
//  · gerçek kurum adı yoktur, davranış satırı hüküm vermez, ödüllü ipucu vakanın
//    gerçeğinden söz etmez (Türkçedeki denetimlerin o dildeki karşılığı).
// Eksik satır sorun değil nottur: oyun önce İngilizceye, sonra Türkçeye düşer.
public static class TranslationRules {
 static readonly Regex Placeholder = new Regex(@"\{\d+\}", RegexOptions.Compiled);

 static readonly Dictionary<string, string[]> Institutions = new Dictionary<string, string[]> {
  ["*"] = new[] { "interpol", "europol", "fbi", "cia", "nsa", "dea", "atf", "mi5", "mi6", "nypd", "lapd",
   "scotland yard", "met police", "metropolitan police", "rcmp", "bka", "lka", "bnd", "dgsi", "dgse",
   "carabinieri", "guardia civil", "policía nacional", "mossos", "ertzaintza", "gendarmerie",
   "police nationale", "polizia di stato", "guardia di finanza", "afp", "nca", "ofsted", "cps" },
  ["en"] = new[] { "crown prosecution service", "home office", "department of justice", "homeland security" },
  ["de"] = new[] { "bundespolizei", "bundeskriminalamt", "landeskriminalamt", "staatsanwaltschaft", "kripo" },
  ["fr"] = new[] { "police judiciaire", "parquet de paris", "ministère de l'intérieur", "brigade criminelle" },
  ["it"] = new[] { "polizia di stato", "procura della repubblica", "ministero dell'interno", "squadra mobile" },
  ["es"] = new[] { "ministerio del interior", "fiscalía", "audiencia nacional" },
  ["pt-BR"] = new[] { "polícia federal", "polícia civil", "polícia militar", "ministério público" },
 };

 // Türkçe `VerdictWords` listesinin karşılıkları: gözlem satırı hüküm vermez.
 static readonly Dictionary<string, HashSet<string>> Verdicts = new Dictionary<string, HashSet<string>> {
  ["en"] = Set("lie", "lies", "lying", "liar", "lied", "contradiction", "contradictory", "truth", "guilty",
   "innocent", "hiding", "hides", "concealing", "sincere", "insincere", "genuine", "fear", "afraid", "panicked", "relieved"),
  ["de"] = Set("lüge", "lügt", "lügner", "gelogen", "widerspruch", "widersprüchlich", "wahrheit", "schuldig",
   "unschuldig", "verbirgt", "verheimlicht", "aufrichtig", "unaufrichtig", "angst", "panik", "erleichtert"),
  ["fr"] = Set("mensonge", "ment", "menteur", "menteuse", "menti", "contradiction", "contradictoire", "vérité",
   "coupable", "innocent", "innocente", "cache", "dissimule", "sincère", "peur", "paniqué", "paniquée", "soulagé", "soulagée"),
  ["it"] = Set("bugia", "mente", "bugiardo", "bugiarda", "mentito", "contraddizione", "contraddittorio", "verità",
   "colpevole", "innocente", "nasconde", "sincero", "sincera", "paura", "panico", "sollevato", "sollevata"),
  ["es"] = Set("mentira", "miente", "mentiroso", "mentirosa", "mintió", "contradicción", "contradictorio", "verdad",
   "culpable", "inocente", "oculta", "esconde", "sincero", "sincera", "miedo", "pánico", "aliviado", "aliviada"),
  ["pt-BR"] = Set("mentira", "mente", "mentiroso", "mentirosa", "mentiu", "contradição", "contraditório", "verdade",
   "culpado", "culpada", "inocente", "esconde", "oculta", "sincero", "sincera", "medo", "pânico", "aliviado", "aliviada"),
 };

 static HashSet<string> Set(params string[] words) => new HashSet<string>(words);

 public static void Validate(IReadOnlyList<KeyValuePair<string, Locale>> canonFiles,
  IReadOnlyList<CaseData> cases, ValidationReport report) {
  var canon = Merge(canonFiles);
  foreach (var code in Languages.All) {
   if (code == Languages.Canon) continue;
   var files = Files(code);
   if (files.Count == 0) continue;
   report.Scope("çeviri:" + code);
   Validate(code, canon, files, cases, report);
  }
  report.Scope(null);
 }

 public static void Validate(string code, Dictionary<string, string> canon,
  IReadOnlyList<KeyValuePair<string, Locale>> files, IReadOnlyList<CaseData> cases, ValidationReport report) {
  var seen = new HashSet<string>();
  var names = CaseNames(canon, cases);
  var institutions = Institutions["*"].Concat(Institutions.TryGetValue(code, out var own) ? own : new string[0]).ToArray();
  Verdicts.TryGetValue(code, out var verdicts);
  foreach (var file in files)
  foreach (var entry in file.Value?.entries ?? new Entry[0]) {
   if (entry == null || string.IsNullOrEmpty(entry.key)) { report.Problem("Anahtarı olmayan giriş (" + file.Key + ")."); continue; }
   if (!seen.Add(entry.key)) { report.Problem("Yinelenen anahtar: " + entry.key); continue; }
   if (!canon.TryGetValue(entry.key, out var source)) { report.Problem("Kanonda olmayan anahtar: " + entry.key); continue; }
   var value = entry.value ?? "";
   if (value.Trim().Length == 0 && source.Trim().Length > 0) report.Problem("Boş çeviri: " + entry.key);
   if (!Placeholders(source).SequenceEqual(Placeholders(value)))
    report.Problem("Yer tutucular kanonla aynı değil: " + entry.key);
   var lower = value.ToLowerInvariant();
   var institution = institutions.FirstOrDefault(i => ContainsWord(lower, i));
   if (institution != null) report.Problem("Gerçek kurum adı kullanılamaz (\"" + institution + "\"): " + entry.key);
   if (verdicts != null && entry.key.EndsWith(".demeanor")) {
    var verdict = LocaleRules.Words(value).FirstOrDefault(verdicts.Contains);
    if (verdict != null) report.Problem("Davranış satırı hüküm veriyor (\"" + verdict + "\"): " + entry.key);
   }
   if (IsPersonName(entry.key, cases) && !value.EndsWith(Bare(source), StringComparison.Ordinal))
    report.Problem("Kişi adı çevrilmez (unvan çevrilebilir): " + entry.key + " (\"" + source + "\" → \"" + value + "\")");
   var caseId = CaseOf(entry.key);
   if (caseId != null && names.TryGetValue(caseId, out var people)) {
    var expected = people.Where(n => MentionsCanon(source, n)).ToArray();
    var actual = people.Where(n => Mentions(value, n)).ToArray();
    foreach (var missing in expected.Except(actual))
     report.Problem("Kanonda geçen ad çeviride yok (\"" + missing + "\"): " + entry.key);
    foreach (var extra in actual.Except(expected))
     report.Problem("Kanonda geçmeyen ad çeviriye eklenmiş (\"" + extra + "\"): " + entry.key);
   }
  }
  LocaleRules.ValidateGuidance(files, cases, report);
  int missingCount = canon.Keys.Count(k => !seen.Contains(k));
  if (missingCount > 0) report.Note(code + ": " + missingCount + " / " + canon.Count + " satır çevrilmemiş (İngilizceye/Türkçeye düşer).");
 }

 public static List<KeyValuePair<string, Locale>> Files(string code) {
  var list = new List<KeyValuePair<string, Locale>>();
  var shared = Resources.Load<TextAsset>(LocaleLoader.Folder + code);
  if (shared == null) return list;
  list.Add(new KeyValuePair<string, Locale>(code + ".json", JsonUtility.FromJson<Locale>(shared.text)));
  foreach (var asset in LocaleLoader.CaseAssets(code))
   list.Add(new KeyValuePair<string, Locale>(asset.name + ".json", JsonUtility.FromJson<Locale>(asset.text)));
  return list;
 }

 public static Dictionary<string, string> Merge(IReadOnlyList<KeyValuePair<string, Locale>> files) {
  var merged = new Dictionary<string, string>();
  foreach (var file in files)
  foreach (var entry in file.Value?.entries ?? new Entry[0])
   if (entry?.key != null && !merged.ContainsKey(entry.key)) merged[entry.key] = entry.value ?? "";
  return merged;
 }

 static string[] Placeholders(string text) =>
  Placeholder.Matches(text ?? "").Cast<Match>().Select(m => m.Value).OrderBy(v => v).ToArray();

 static bool ContainsWord(string haystack, string phrase) =>
  Regex.IsMatch(haystack, @"(?<![\p{L}\p{N}])" + Regex.Escape(phrase) + @"(?![\p{L}\p{N}])");

 // Ad büyük harfle yazılır; önünde harf olmamalı (Türkçedeki sınır kuralı).
 // Ardından harf gelemez: çeviride ek yoktur, "Anna" "Annabel"in içinde aranmaz.
 static bool Mentions(string text, string name) =>
  Regex.IsMatch(text ?? "", @"(?<!\p{L})" + Regex.Escape(name) + @"(?!\p{Ll})");

 // Türkçede ek kesmesiz de gelir ("Hasanla"); kanon tarafta ardından harf serbest.
 static bool MentionsCanon(string text, string name) =>
  Regex.IsMatch(text ?? "", @"(?<!\p{L})" + Regex.Escape(name));

 // Unvan çevrilir ("Komiser" → "Inspector"), ad çevrilmez.
 static readonly HashSet<string> Titles = new HashSet<string> { "Dr.", "Doç.", "Prof.", "Komiser", "Av." };
 static string Bare(string full) => string.Join(" ", (full ?? "").Split(' ').SkipWhile(Titles.Contains));

 static string CaseOf(string key) {
  var dot = key.IndexOf('.');
  return dot > 0 && key.StartsWith("case", StringComparison.Ordinal) ? key.Substring(0, dot) : null;
 }

 static bool IsPersonName(string key, IReadOnlyList<CaseData> cases) =>
  cases.Any(c => (c.nodes ?? new Node[0]).Any(n => n.personNameKey == key));

 // Her vakanın kişi adları (tam ad ve ilk ad), kanondan.
 static Dictionary<string, string[]> CaseNames(Dictionary<string, string> canon, IReadOnlyList<CaseData> cases) {
  var result = new Dictionary<string, string[]>();
  foreach (var data in cases) {
   var names = new HashSet<string>();
   foreach (var node in data.nodes ?? new Node[0]) {
    if (string.IsNullOrEmpty(node.personNameKey) || !canon.TryGetValue(node.personNameKey, out var full)) continue;
    var first = Bare(full).Split(' ')[0].Trim('.', ',');
    if (first.Length > 2 && char.IsUpper(first[0])) names.Add(first);
   }
   result[data.id] = names.ToArray();
  }
  return result;
 }
}
}
