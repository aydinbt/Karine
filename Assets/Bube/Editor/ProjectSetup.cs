using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Bube.Editor {
public static class ProjectSetup {
 [MenuItem("Bube/Validate Content")]
 public static void Validate() {
  var report=ContentValidator.Run();
  foreach(var note in report.Notes)Debug.LogWarning(note);
  if(report.HasProblems)throw new Exception(report.Summary());
  Debug.Log("BUBE VALIDATION PASSED");
 }
 public static void Setup() {
  PlayerSettings.productName=JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text).title;
  PlayerSettings.companyName="bubeGames";
  PlayerSettings.defaultInterfaceOrientation=UIOrientation.AutoRotation;
  PlayerSettings.allowedAutorotateToLandscapeLeft=true; PlayerSettings.allowedAutorotateToLandscapeRight=true;
  PlayerSettings.allowedAutorotateToPortrait=false; PlayerSettings.allowedAutorotateToPortraitUpsideDown=false;
  PlayerSettings.defaultScreenWidth=1280; PlayerSettings.defaultScreenHeight=720;
  System.IO.Directory.CreateDirectory("Assets/Bube/Scenes");
  const string legacyPath="Assets/Bube/Scenes/Bootstrap.unity";
  if(!System.IO.File.Exists(legacyPath)) {
   var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   new GameObject("Bube Application").AddComponent<BubeApp>();
   EditorSceneManager.SaveScene(scene,legacyPath);
  }
  var scenePaths=ProjectRules.SceneOrder.Select(name=>"Assets/Bube/Scenes/"+name+".unity").ToArray();
  if(scenePaths.Any(path=>!System.IO.File.Exists(path)))throw new Exception("Required scene missing");
  EditorBuildSettings.scenes=scenePaths.Select(path=>new EditorBuildSettingsScene(path,true)).ToArray();
  Validate();
  AssetDatabase.SaveAssets(); Debug.Log("BUBE SETUP COMPLETE");
 }
}
}
