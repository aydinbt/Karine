using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Bube {

// Uygulama silinince kaybolmayan küçük anahtar deposu: iOS Keychain, Android Google
// Block Store. Yalnız misafirin bulut anahtarı için. Editor'de ve diğer platformlarda
// hiçbir şey tutmaz (okuma boş döner), oyun yine PlayerPrefs ile çalışır.
public static class DurableStore {
 static readonly Queue<Action> main = new Queue<Action>();

 // Android sonucu kendi iş parçacığında döner; Unity'ye ana döngüde aktarılır.
 public static void Pump() {
  while (true) {
   Action next;
   lock (main) { if (main.Count == 0) return; next = main.Dequeue(); }
   next();
  }
 }

#if UNITY_IOS && !UNITY_EDITOR
 [DllImport("__Internal")] static extern string KarineKeychainRead(string key);
 [DllImport("__Internal")] static extern void KarineKeychainWrite(string key, string value);
 [DllImport("__Internal")] static extern void KarineKeychainDelete(string key);
 public static void Read(string key, Action<string> done) => done(KarineKeychainRead(key));
 public static void Write(string key, string value) => KarineKeychainWrite(key, value);
 public static void Delete(string key) => KarineKeychainDelete(key);
#elif UNITY_ANDROID && !UNITY_EDITOR
 const string Bridge = "com.bubedigital.karine.blockstore.BlockStoreBridge";
 sealed class Callback : AndroidJavaProxy {
  readonly Action<string> then;
  public Callback(Action<string> then) : base(Bridge + "$Callback") { this.then = then; }
  public void done(string value) { lock (main) main.Enqueue(() => then(value)); }
 }
 static AndroidJavaObject Context() {
  using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) return player.GetStatic<AndroidJavaObject>("currentActivity");
 }
 static void Call(string method, params object[] args) {
  try { using (var bridge = new AndroidJavaClass(Bridge)) bridge.CallStatic(method, args); }
  catch (Exception e) { Debug.LogWarning("Block Store unavailable: " + e.Message); }
 }
 public static void Read(string key, Action<string> done) {
  try { using (var bridge = new AndroidJavaClass(Bridge)) bridge.CallStatic("read", Context(), key, new Callback(done)); }
  catch (Exception e) { Debug.LogWarning("Block Store unavailable: " + e.Message); done(null); }
 }
 public static void Write(string key, string value) => Call("write", Context(), key, value);
 public static void Delete(string key) => Call("delete", Context(), key);
#else
 public static void Read(string key, Action<string> done) => done(null);
 public static void Write(string key, string value) {}
 public static void Delete(string key) {}
#endif
}
}
