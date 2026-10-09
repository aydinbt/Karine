using System.Linq;
using NUnit.Framework;

namespace Bube.Tests {

// Yem kayıt soruyu kapatmaz: kişi savuşturur, oyuncu aynı soruda başka kayıtla hemen ilerler.
public class DecoyHoldTests {

 [Test]
 public void DecoyKeepsQuestionOpen() {
  var game = Case001Walk.WalkToReportReady();
  var node = Case001Walk.Node(game, "hasan_follow");
  var sale = node.questions.First(q => q.id == "hasan_follow.sale");
  Assert.IsTrue(game.CanAskQuestion(node, sale), "Ön koşul: soru açık olmalı.");
  if (sale.decoyAnswers != null && sale.decoyAnswers.Length > 0)
   game.MarkSourceTried(node, sale, sale.decoyAnswers[0].sourceId);
  Assert.IsTrue(game.CanAskQuestion(node, sale), "Yemden sonra soru bekletiliyor.");
 }
}
}
