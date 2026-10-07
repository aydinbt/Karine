using System.Linq;
using NUnit.Framework;

namespace Bube.Tests {

// Yem kayda yanıt alınan soru o an kapanır, dosyaya yeni bir şey girince geri gelir.
public class DecoyHoldTests {

 [Test]
 public void DecoyClosesQuestionUntilNewProgress() {
  var game = Case001Walk.WalkToReportReady();
  var node = Case001Walk.Node(game, "hasan_follow");
  var sale = node.questions.First(q => q.id == "hasan_follow.sale");
  Assert.IsTrue(game.CanAskQuestion(node, sale), "Ön koşul: soru açık olmalı.");

  game.HoldAfterDecoy(node, sale);
  Assert.IsFalse(game.CanAskQuestion(node, sale), "Yemden sonra soru hemen yeniden sorulabiliyor.");

  var other = game.Data.nodes.Where(n => n.kind == "interview" && game.Available(n))
   .SelectMany(n => (n.questions ?? new Question[0]).Select(q => (n, q)))
   .First(p => p.q != sale && game.CanAskQuestion(p.n, p.q));
  Case001Walk.Ask(game, other.n.id, other.q.id);
  Assert.IsTrue(game.CanAskQuestion(node, sale), "Yeni yanıttan sonra soru geri gelmedi.");
 }
}
}
