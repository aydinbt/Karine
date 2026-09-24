using System.Collections.Generic;
using System.Linq;

namespace Bube.Editor {

// Doğrulayıcı eskiden ilk sorunda `throw` ediyordu: bir hatayı düzeltip yeniden
// koşmak, sıradakini görmenin tek yoluydu. Bulgular artık burada toplanır ve
// koşumun sonunda hep birlikte raporlanır.
//
// Sorun (Problem) koşumu başarısız yapar. Not (Note) yapmaz — ölü metin anahtarı
// ya da üretilmiş yedek portre gibi, bilinmesi gereken ama yayını engellemeyen
// şeyler için.
public sealed class ValidationReport {
 readonly List<string> problems = new List<string>();
 readonly List<string> notes = new List<string>();
 string scope = "";

 public IReadOnlyList<string> Problems => problems;
 public IReadOnlyList<string> Notes => notes;
 public bool HasProblems => problems.Count > 0;

 // Bulgular hangi vakadan geldiğini söylemeli; yoksa "Unknown prerequisite"
 // gibi bir satır üç vakalı bir projede işe yaramaz.
 public void Scope(string value) => scope = string.IsNullOrEmpty(value) ? "" : value + ": ";

 public void Problem(string message) => problems.Add(scope + message);
 public void Note(string message) => notes.Add(scope + message);

 public void Require(bool condition, string message) { if (!condition) Problem(message); }
 public void Forbid(bool condition, string message) { if (condition) Problem(message); }

 // Bir adım ancak önkoşulu tuttuğunda anlamlı olduğunda kullanılır: iddia
 // düşerse hem sorun kaydedilir hem de çağıran erken çıkabilir.
 public bool Step(bool condition, string message) { if (!condition) Problem(message); return condition; }

 public string Summary() {
  var lines = new List<string>();
  if (problems.Count > 0) {
   lines.Add(problems.Count + " sorun:");
   lines.AddRange(problems.Select((p, i) => "  " + (i + 1) + ". " + p));
  }
  if (notes.Count > 0) {
   lines.Add(notes.Count + " not:");
   lines.AddRange(notes.Select(n => "  - " + n));
  }
  return lines.Count == 0 ? "Sorun yok." : string.Join("\n", lines);
 }
}
}
