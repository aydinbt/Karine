using System;
using System.Collections;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace Bube {
// com.unity.mobile.notifications gerçeklemesi. Android 13+ ve iOS izni işletim
// sisteminin penceresiyle sorulur; izin yoksa kurulan bildirim gösterilmez.
public sealed class MobileNotifier : MonoBehaviour, INotifier {
 const string Channel = "karine.desk";

 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
 static void Install() {
  if (!Application.isMobilePlatform) return;
  var host = new GameObject("KarineNotifier") { hideFlags = HideFlags.HideAndDontSave };
  DontDestroyOnLoad(host);
  var notifier = host.AddComponent<MobileNotifier>();
#if UNITY_ANDROID
  AndroidNotificationCenter.Initialize();
  AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel(Channel, "Karine", "Desk reminders", Importance.Default));
#endif
  Notifications.Provider = notifier;
 }

 public void RequestPermission(Action<bool> done) => StartCoroutine(Ask(done));

 IEnumerator Ask(Action<bool> done) {
#if UNITY_ANDROID
  var request = new PermissionRequest();
  while (request.Status == PermissionStatus.RequestPending) yield return null;
  done(request.Status == PermissionStatus.Allowed);
#elif UNITY_IOS
  using (var request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Sound, false)) {
   while (!request.IsFinished) yield return null;
   done(request.Granted);
  }
#else
  yield return null; done(false);
#endif
 }

 public void Schedule(int id, string title, string body, TimeSpan after) {
#if UNITY_ANDROID
  AndroidNotificationCenter.SendNotificationWithExplicitID(new AndroidNotification(title, body, DateTime.Now.Add(after)), Channel, id);
#elif UNITY_IOS
  iOSNotificationCenter.ScheduleNotification(new iOSNotification {
   Identifier = "karine." + id, Title = title, Body = body, ShowInForeground = false,
   Trigger = new iOSNotificationTimeIntervalTrigger { TimeInterval = after, Repeats = false }
  });
#endif
 }

 public void CancelAll() {
#if UNITY_ANDROID
  AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS
  iOSNotificationCenter.RemoveAllScheduledNotifications();
  iOSNotificationCenter.RemoveAllDeliveredNotifications();
  iOSNotificationCenter.ApplicationBadge = 0;
#endif
 }
}
}
