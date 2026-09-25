using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {

// PlayMode testleri gercek oyunu acar, yani gercek kayit klasorune yazar:
// `Application.persistentDataPath` proje kopyasinda da aynidir, cunku yolu
// sirket ve urun adi belirler. Bu yuzden bir test kosusu gelistiricinin kendi
// oynayisini bozabiliyordu — vaka kabul edilmis, sonraki dosya acilmis olarak
// kaydediliyor ve oyun bir daha o ani oynamiyordu.
//
// Burasi tum PlayMode testlerinden once kayitlari bellege alip klasoru
// bosaltir, testler bitince geri koyar. `SetUpFixture` ad alanindaki her
// testten once bir kez calisir.
[SetUpFixture] public sealed class SaveSandbox {

 // `BubeApp` DontDestroyOnLoad'dir: BootScene yeniden yuklense de ilk ornek
 // yasamaya devam eder, yani bir testin actigi vaka bir sonraki teste sizar.
 // Her test bu yuzden oyunu ilk vakaya, bos ilerlemeye dondurerek baslar.
 public static void ResetToInitialCase(BubeApp app) {
  var type = typeof(BubeApp);
  const BindingFlags Any = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
  var config = (GameConfig)type.GetField("config", Any).GetValue(app);
  var locale = (Locale)type.GetField("locale", Any).GetValue(app);
  var rules = (CareerRules)type.GetField("careerRules", Any).GetValue(app);
  var data = JsonUtility.FromJson<CaseData>(
   Resources.Load<TextAsset>("Bube/Cases/" + config.initialCase).text);
  var game = new Investigation(data, null, null, rules) { Text = locale };
  game.Career.activeCaseId = config.initialCase;
  type.GetField("game", Any).SetValue(app, game);
 }

 static readonly Dictionary<string,byte[]> kept = new Dictionary<string,byte[]>();

 static string[] SaveFiles() => Directory.Exists(Application.persistentDataPath)
  ? Directory.GetFiles(Application.persistentDataPath,"bube-*")
  : new string[0];

 [OneTimeSetUp] public void KeepPlayerSaves() {
  kept.Clear();
  foreach (var file in SaveFiles()) {
   kept[Path.GetFileName(file)] = File.ReadAllBytes(file);
   File.Delete(file);
  }
 }

 [OneTimeTearDown] public void RestorePlayerSaves() {
  foreach (var file in SaveFiles()) File.Delete(file);
  foreach (var pair in kept)
   File.WriteAllBytes(Path.Combine(Application.persistentDataPath,pair.Key),pair.Value);
  kept.Clear();
 }
}
}
