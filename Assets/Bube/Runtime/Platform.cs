using System;
using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
using AOT;
#endif

namespace Bube {

// İşletim sistemine özgü iki çağrı: iOS reklam takip izni (ATT) ve oyun içi puan isteme.
// Editor'de ve diğer platformlarda: takip "izin yok" sayılmaz, sorulmadan geçilir; puan yok.
public static class Platform {
#if UNITY_IOS && !UNITY_EDITOR
 delegate void TrackingCallback(int status);
 [DllImport("__Internal")] static extern void KarineRequestTracking(TrackingCallback callback);
 [DllImport("__Internal")] static extern void KarineRequestReview();
 static Action trackingDone;
 [MonoPInvokeCallback(typeof(TrackingCallback))]
 static void OnTracking(int status) { var done = trackingDone; trackingDone = null; done?.Invoke(); }
 // Apple 5.1.2: kişiselleştirilmiş reklamdan önce izin sorulur. Sonuç ne olursa olsun
 // reklam ağı durumu kendisi okur (izin yoksa kişiselleştirilmemiş reklam).
 public static void RequestTracking(Action done) { trackingDone = done; KarineRequestTracking(OnTracking); }
 public static void RequestReview() => KarineRequestReview();
#elif UNITY_ANDROID && !UNITY_EDITOR
 public static void RequestTracking(Action done) => done();
 public static void RequestReview() {
  try {
   using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
   using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
   using (var bridge = new AndroidJavaClass("com.bubedigital.karine.review.ReviewBridge"))
    bridge.CallStatic("request", activity);
  } catch (Exception e) { Debug.LogWarning("In-app review unavailable: " + e.Message); }
 }
#else
 public static void RequestTracking(Action done) => done();
 public static void RequestReview() {}
#endif
}
}
