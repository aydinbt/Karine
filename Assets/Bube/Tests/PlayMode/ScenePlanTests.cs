using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {
// Olay yeri planı dosyada açılır, yalnız kilidi açılmış işaretleri gösterir ve bir işarete
// dokununca altındaki satır o işaretin etiketine döner. Doğru/yalan hiçbir yerde yazmaz.
public sealed class ScenePlanTests {
 const BindingFlags Any=BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public;

 [UnityTest] public IEnumerator Plan_ShowsOnlyUnlockedMarkersAndAnswersTap() {
  SceneManager.LoadScene("BootScene",LoadSceneMode.Single);
  yield return null;yield return null;
  var app=Object.FindFirstObjectByType<BubeApp>();
  SaveSandbox.ResetToInitialCase(app);
  var type=typeof(BubeApp);
  var loc=(Locale)type.GetField("locale",Any).GetValue(app);
  var rules=(CareerRules)type.GetField("careerRules",Any).GetValue(app);
  var data=JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/case015").text);
  var game=new Investigation(data,null,null,rules){Text=loc};game.Career.activeCaseId="case015";game.AcceptCase();
  game.State.read.Add("report");game.State.read.Add("plan");game.State.asked.Add("case015.gareth.where");
  type.GetField("game",Any).SetValue(app,game);
  type.GetField("selectedFileSection",Any).SetValue(app,"evidence");
  type.GetField("selectedFileNode",Any).SetValue(app,"plan");
  type.GetMethod("FilePage",Any,null,new System.Type[0],null).Invoke(app,null);
  yield return null;yield return null;

  var root=Object.FindObjectsByType<UIDocument>(FindObjectsSortMode.None).Select(d=>d.rootVisualElement).First(r=>r.Q("ScenePlan")!=null);
  var markers=root.Query<Button>().Where(b=>b.name.StartsWith("PlanMarker_")).ToList();
  Assert.That(markers.Select(b=>b.name),Is.EquivalentTo(new[]{"PlanMarker_gareth_claim"}));
  var caption=root.Q<Label>("ScenePlanCaption");
  string before=caption.text;
  using(var e=NavigationSubmitEvent.GetPooled()){e.target=markers[0];markers[0].SendEvent(e);}
  yield return null;
  Assert.That(caption.text,Is.Not.EqualTo(before));
  StringAssert.Contains("Gareth Hollis",caption.text);
  StringAssert.DoesNotContain("yalan",caption.text.ToLowerInvariant());
 }
}
}
