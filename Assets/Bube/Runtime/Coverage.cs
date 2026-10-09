using System.Linq;

namespace Bube {

// Oyuncunun **kendi çalışmasının** ölçüsü. Vakanın gerçeğiyle hiçbir ilgisi yok:
// burada fail, çelişki, yalan, gizli durum veya sıradaki adım yoktur; yalnız
// "önüne açılmış şu kadar kaynaktan şu kadarını açtın" vardır.
//
// Ödüllü ipucu ekranı bunu okur. Sayı vermek kanonu bozmaz; oyuncuya kendi
// çalışmasını göstermek, ona cevabı söylemek değildir.
public struct Coverage {
 public int SourcesOpen, SourcesAvailable, QuestionsAsked, QuestionsAvailable, CluesPinned, CluesAvailable;

 // whole: rapor gittikten sonra payda dosyanın tamamıdır (artık ipucu değil, karne);
 // soruşturma sürerken yalnız bugüne dek açılmış olan sayılır ki gizli soru sayısı sızmasın.
 public static Coverage Of(Investigation game, bool whole = false) {
  var data = game.Data;
  var state = game.State;
  var coverage = new Coverage();

  foreach (var node in data.nodes ?? new Node[0]) {
   bool open = state.read.Contains(node.id);
   // Açılmış bir kaynak kapanmaz; "erişilebilir" olmayı bugünün kapısına sorar.
   if (whole || open || game.Available(node)) coverage.SourcesAvailable++;
   if (open) coverage.SourcesOpen++;
   foreach (var question in node.questions ?? new Question[0]) {
    bool asked = state.asked.Contains(question.id);
    if (whole || asked || game.QuestionAvailable(node, question)) coverage.QuestionsAvailable++;
    if (asked) coverage.QuestionsAsked++;
   }
  }

  foreach (var clue in data.timelineClues ?? new TimelineClue[0]) {
   if (!whole && !game.TimelineAvailable(clue)) continue;
   coverage.CluesAvailable++;
   if (state.timelinePinned.Contains(clue.id)) coverage.CluesPinned++;
  }
  return coverage;
 }

 // Hepsini gezmiş bir oyuncuya "daha çok bak" demek yanlış olur. Ama hiç
 // başlamamış olmak da "hepsini gezmiş" değildir: açılabilir hiçbir şey yokken
 // 0/0 görünür ve bu tamamlanma sayılmaz.
 public bool Complete => SourcesAvailable > 0 &&
  SourcesOpen >= SourcesAvailable && QuestionsAsked >= QuestionsAvailable;
}
}
