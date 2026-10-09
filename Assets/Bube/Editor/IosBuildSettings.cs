#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Bube.Editor {
// iOS derlemesine: takip izni açıklaması (Info.plist, Apple 5.1.2 zorunlu) ve
// ATT ile puan isteme için çerçeveler. Metin tek yerde; mağaza formuyla aynı olmalı.
static class IosBuildSettings {
 const string TrackingText = "Used to show ads that are more relevant to you. If you decline, the game works exactly the same; ads are simply not personalised.";

 [PostProcessBuild(100)]
 static void OnPostprocessBuild(BuildTarget target, string path) {
  if (target != BuildTarget.iOS) return;
  var plistPath = Path.Combine(path, "Info.plist");
  var plist = new PlistDocument(); plist.ReadFromFile(plistPath);
  plist.root.SetString("NSUserTrackingUsageDescription", TrackingText);
  plist.WriteToFile(plistPath);

  var projectPath = PBXProject.GetPBXProjectPath(path);
  var project = new PBXProject(); project.ReadFromFile(projectPath);
  var framework = project.GetUnityFrameworkTargetGuid();
  project.AddFrameworkToProject(framework, "AppTrackingTransparency.framework", true);
  project.AddFrameworkToProject(framework, "StoreKit.framework", false);
  project.WriteToFile(projectPath);
 }
}
}
#endif
