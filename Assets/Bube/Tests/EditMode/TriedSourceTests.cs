using NUnit.Framework;
namespace Bube.Tests {
public class TriedSourceTests {
 // Öne sürülen kayıt o soruda bir daha listelenmez; aynı kişinin başka sorusunda listelenir.
 [Test]
 public void TriedSource_IsHiddenOnlyForThatQuestion() {
  var game=new Investigation(new CaseData{nodes=new[]{new Node{id="p",kind="interview",questions=new[]{new Question{id="a"},new Question{id="b"}}}}},new Progress());
  var node=game.Data.nodes[0];
  Assert.IsFalse(game.SourceTried(node,node.questions[0],"camera#x"));
  game.MarkSourceTried(node,node.questions[0],"camera#x");
  Assert.IsTrue(game.SourceTried(node,node.questions[0],"camera#x"));
  Assert.IsFalse(game.SourceTried(node,node.questions[1],"camera#x"));
 }
}
}
