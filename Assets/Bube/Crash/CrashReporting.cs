using Firebase;
using Firebase.Crashlytics;
using UnityEngine;

namespace Bube {
// Çökme raporu: Firebase Crashlytics paketi kurulunca derlenir (Bube.Crash.asmdef).
// Yalnız çökme ve yakalanmamış hatalar gider; oyuncu kimliği ya da oyun içi iz eklenmez.
static class CrashReporting {
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
 static void Install() {
  if (!Application.isMobilePlatform) return;
  FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
   if (task.Result != DependencyStatus.Available) { Debug.LogWarning("Crashlytics unavailable: " + task.Result); return; }
   Crashlytics.ReportUncaughtExceptionsAsFatal = true;
  });
 }
}
}
