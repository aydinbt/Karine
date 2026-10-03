using System.Linq;
using NUnit.Framework;
using UnityEngine;
namespace Bube.Tests {
// Raporda yalnız dinlenen kişiler seçilebilir; kişi olmayan seçenekler hep açıktır.
public class KnownChoicesTests {
 static Investigation Load(string id) {
  var data=JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/"+id).text);
  return new Investigation(data);
 }
 [Test] public void UnheardPerson_IsNotOffered() {
  var game=Load("case003");
  Assert.IsFalse(game.ChoiceKnown(game.Data.verdicts.First(v=>v.id=="ozan")));
  Assert.IsFalse(game.ChoiceKnown(game.Data.custody.First(v=>v.id=="ozan_usb")));
  game.State.read.Add("ozan");
  Assert.IsTrue(game.ChoiceKnown(game.Data.verdicts.First(v=>v.id=="ozan")));
  Assert.IsTrue(game.ChoiceKnown(game.Data.custody.First(v=>v.id=="ozan_usb")));
 }
 [Test] public void NonPersonChoices_AreAlwaysOffered() {
  var game=Load("case003");
  Assert.IsTrue(game.ChoiceKnown(game.Data.verdicts.First(v=>v.id=="accident")));
  Assert.IsTrue(game.ChoiceKnown(game.Data.custody.First(v=>v.id=="none")));
  Assert.IsTrue(game.Data.methods.All(m=>game.ChoiceKnown(m)));
 }
}
}
