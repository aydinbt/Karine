using NUnit.Framework;
using System.Linq;

namespace Bube.Tests {

// Kademeli ipucu yalnız oyuncunun kendi çalışmasından seçilir ve sonucu söylemez.
public sealed class HintTests {

 [Test] public void FirstCase_HasNoHints() =>
  Assert.IsFalse(Hints.Allowed(Case001Walk.Fresh()), "Dosya #001'de ipucu olmamalı.");

 [Test] public void FreshCase_PointsAtTheUnreadReport() {
  var game = Case001Walk.Fresh();
  var first = Hints.For(game, 1);
  Assert.AreEqual(1, first.Tier);
  Assert.AreEqual("report", first.NodeId);
  Assert.AreEqual(2, Hints.For(game, 2).Tier);
 }

 // 3. kademe yalnız oyuncunun okuduğu bir kaydı, henüz sormadığı ve
 // erişebildiği bir soruya eşler.
 [Test] public void CompareTier_UsesOnlyHeldRecordsAndOpenQuestions() {
  var game = Case001Walk.Fresh();
  game.Read("report");
  for (int step = 0; step < 40; step++) {
   var hint = Hints.For(game, 3);
   if (hint.Empty) break;
   if (hint.Tier == 3) {
    Assert.IsTrue(game.State.read.Contains(hint.SourceId.Split('#')[0]), "Okunmamış kayıt önerildi.");
    var node = Case001Walk.Node(game, hint.NodeId);
    Assert.IsTrue(node.questions.Any(q => !game.State.asked.Contains(q.id) && game.QuestionAvailable(node, q)));
   }
   // Oyuncu ipucunu izler: okur ya da ilk açık soruyu sorar.
   var target = Case001Walk.Node(game, hint.NodeId);
   if (!game.State.read.Contains(target.id)) { if (target.kind == "interview") Case001Walk.OpenInterview(game, target.id); else game.Read(target.id); continue; }
   var open = target.questions.First(q => !game.State.asked.Contains(q.id) && game.QuestionAvailable(target, q));
   game.Ask(target.id, open.id, hint.Tier == 3 && game.QuestionNeedsSource(open) ? hint.SourceId : null);
   if (game.State.asked.Contains(open.id)) continue;
   game.Ask(target.id, open.id);
  }
 }
}
}
