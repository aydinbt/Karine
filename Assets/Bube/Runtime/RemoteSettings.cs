using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Bube {

// Mağaza onayı beklemeden müdahale edilebilen üç ayar. Kod değil yalnız ayardır,
// bu yüzden mağaza kurallarına uygundur. Kaynak herkese açık okunan tek Firestore
// belgesi: config/live. Alanlar (hepsi metin, hepsi isteğe bağlı):
//   minVersion     — bundan eski sürüm "güncelle" kâğıdı görür (ör. "1.0.3")
//   pausedCases    — virgülle ayrılmış vaka kimlikleri; düzeltme gelene dek açılmaz
//   notice_tr / notice_en / notice_de / notice_fr — ana menüde tek satırlık duyuru
//   storeUrl_android / storeUrl_ios — güncelle düğmesinin gideceği mağaza sayfası
// Ağ yoksa son okunan değerler kullanılır; hiç okunmadıysa hiçbir şey kapanmaz.
public static class RemoteSettings {
 const string CacheKey = "karine.remote.live";
 static Snapshot current = Load();

 public static event Action Changed;
 public static string MinVersion => current.minVersion ?? "";
 public static bool Outdated => IsOlder(Application.version, MinVersion);
 public static bool CasePaused(string caseId) => !string.IsNullOrEmpty(caseId) &&
  (current.pausedCases ?? "").Split(',').Select(s => s.Trim()).Contains(caseId);
 public static string Notice(string language) {
  var text = language == "tr" ? current.notice_tr : language == "de" ? current.notice_de : language == "fr" ? current.notice_fr : current.notice_en;
  return string.IsNullOrWhiteSpace(text) ? current.notice_en ?? "" : text;
 }
 public static string StoreUrl {
  get {
   var url = Application.platform == RuntimePlatform.IPhonePlayer ? current.storeUrl_ios : current.storeUrl_android;
   if (!string.IsNullOrEmpty(url)) return url;
   return Application.platform == RuntimePlatform.Android ? "market://details?id=" + Application.identifier : "";
  }
 }

 public static void Fetch(string apiKey, string projectId) {
  if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(projectId)) return;
  var url = "https://firestore.googleapis.com/v1/projects/" + projectId + "/databases/(default)/documents/config/live?key=" + apiKey;
  var req = UnityWebRequest.Get(url); req.timeout = 10;
  req.SendWebRequest().completed += _ => {
   long code = req.responseCode; string body = req.downloadHandler?.text; req.Dispose();
   // 404: belge yok → her şey açık. Başka hata: son bilinen değerler kalır.
   if (code == 404) Apply(new Snapshot());
   else if (code == 200 && body != null) Apply(Parse(body));
  };
 }

 public static void Apply(Snapshot next) {
  current = next ?? new Snapshot();
  PlayerPrefs.SetString(CacheKey, JsonUtility.ToJson(current)); PlayerPrefs.Save();
  Changed?.Invoke();
 }

 static Snapshot Load() {
  var raw = PlayerPrefs.GetString(CacheKey, "");
  return raw.Length == 0 ? new Snapshot() : JsonUtility.FromJson<Snapshot>(raw) ?? new Snapshot();
 }

 static Snapshot Parse(string body) {
  var f = JsonUtility.FromJson<Doc>(body)?.fields;
  if (f == null) return new Snapshot();
  return new Snapshot {
   minVersion = f.minVersion?.stringValue, pausedCases = f.pausedCases?.stringValue,
   notice_tr = f.notice_tr?.stringValue, notice_en = f.notice_en?.stringValue, notice_de = f.notice_de?.stringValue, notice_fr = f.notice_fr?.stringValue,
   storeUrl_android = f.storeUrl_android?.stringValue, storeUrl_ios = f.storeUrl_ios?.stringValue,
  };
 }

 // "1.0.10" > "1.0.9": parça parça sayı olarak karşılaştırılır; boş en düşük sürüm hiçbir şeyi kapatmaz.
 public static bool IsOlder(string version, string minimum) {
  if (string.IsNullOrWhiteSpace(minimum)) return false;
  var a = Parts(version); var b = Parts(minimum);
  for (int i = 0; i < Math.Max(a.Length, b.Length); i++) {
   int x = i < a.Length ? a[i] : 0, y = i < b.Length ? b[i] : 0;
   if (x != y) return x < y;
  }
  return false;
 }
 static int[] Parts(string v) => (v ?? "").Split('.').Select(p => int.TryParse(new string(p.TakeWhile(char.IsDigit).ToArray()), out var n) ? n : 0).ToArray();

 [Serializable] public class Snapshot { public string minVersion, pausedCases, notice_tr, notice_en, notice_de, notice_fr, storeUrl_android, storeUrl_ios; }
 [Serializable] class Doc { public Fields fields; }
 [Serializable] class Fields { public Value minVersion, pausedCases, notice_tr, notice_en, notice_de, notice_fr, storeUrl_android, storeUrl_ios; }
 [Serializable] class Value { public string stringValue; }
}
}
