using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Bube {

// Misafirin bulut kaydı: Firebase anonim kimliği + Firestore, SDK'sız (REST).
// Oyuncudan ad, e-posta veya cihaz kimliği alınmaz; sunucuda yalnız rastgele bir
// oyuncu kimliği ve oyunun kendi kayıt dosyaları durur (KVKK: veri en aza indirilir).
// Kayıt yolu: users/{uid}/saves/{anahtar} → { entry: CloudEntry JSON }.
// Silme: önce tüm kayıt belgeleri, sonra kimliğin kendisi; ikisi de geri alınamaz.
// Apple/Google girişi geldiğinde aynı kimliğe `signInWithIdp` ile bağlanacak.
public sealed class FirebaseAccountProvider : IAccountProvider {
 const string RefreshKey = "karine.firebase.refresh";
 const string Identity = "https://identitytoolkit.googleapis.com/v1/accounts:";
 const string SecureToken = "https://securetoken.googleapis.com/v1/token?key=";
 readonly string apiKey, documents;
 string uid, idToken; float expiresAt;

 public FirebaseAccountProvider(string apiKey, string projectId) {
  this.apiKey = apiKey;
  documents = "https://firestore.googleapis.com/v1/projects/" + projectId + "/databases/(default)/documents/";
 }

 public string CloudId => uid;
 public bool CloudReady => !string.IsNullOrEmpty(uid);
 public bool Supports(AccountKind kind) => kind == AccountKind.Guest;
 public void Tick() {}
 public void SignIn(AccountKind kind, bool link, Action<string,string> done) => done(null, "account.error.unavailable");
 // Misafirin çıkışı yok; kimlik cihazda kalır ki ilerleme kaybolmasın.
 public void SignOut() {}

 // Kayıtlı yenileme anahtarı varsa aynı oyuncu döner; yoksa yeni anonim kimlik açılır.
 // Ağ yoksa oyun yerel kayıtla açılır, bulut bir sonraki açılışta denenir.
 public void Restore(Action<AccountKind,string> done) {
  var refresh = PlayerPrefs.GetString(RefreshKey, "");
  if (refresh.Length > 0) Refresh(refresh, ok => { if (ok) done(AccountKind.Guest, null); else SignUp(_ => done(AccountKind.Guest, null)); });
  else SignUp(_ => done(AccountKind.Guest, null));
 }

 void SignUp(Action<bool> done) {
  Send(Post(Identity + "signUp?key=" + apiKey, "{\"returnSecureToken\":true}"), (code, body) => {
   if (code != 200) { done(false); return; }
   var r = JsonUtility.FromJson<SignUpReply>(body);
   Keep(r.localId, r.idToken, r.refreshToken, r.expiresIn); done(true);
  });
 }

 void Refresh(string refresh, Action<bool> done) {
  var form = "grant_type=refresh_token&refresh_token=" + UnityWebRequest.EscapeURL(refresh);
  Send(Post(SecureToken + apiKey, form, "application/x-www-form-urlencoded"), (code, body) => {
   // 400: kimlik sunucuda silinmiş ya da geçersiz; eski anahtar atılır.
   if (code == 400) PlayerPrefs.DeleteKey(RefreshKey);
   if (code != 200) { done(false); return; }
   var r = JsonUtility.FromJson<RefreshReply>(body);
   Keep(r.user_id, r.id_token, r.refresh_token, r.expires_in); done(true);
  });
 }

 void Keep(string id, string token, string refresh, string expiresIn) {
  uid = id; idToken = token;
  expiresAt = Time.realtimeSinceStartup + (float.TryParse(expiresIn, out var s) ? s : 3600f) - 120f;
  PlayerPrefs.SetString(RefreshKey, refresh); PlayerPrefs.Save();
 }

 // Kimlik anahtarı bir saat geçerli; süresi dolmuşsa istekten önce yenilenir.
 void Authed(Action<bool> then) {
  if (!CloudReady) { then(false); return; }
  if (Time.realtimeSinceStartup < expiresAt) { then(true); return; }
  Refresh(PlayerPrefs.GetString(RefreshKey, ""), then);
 }

 public void Push(IDictionary<string,string> entries) {
  var copy = new Dictionary<string,string>(entries);
  Authed(ok => {
   if (!ok) return;
   foreach (var pair in copy) {
    var body = "{\"fields\":{\"entry\":{\"stringValue\":" + Quote(pair.Value) + "}}}";
    var req = new UnityWebRequest(Doc(pair.Key), "PATCH") { uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body)), downloadHandler = new DownloadHandlerBuffer() };
    req.SetRequestHeader("Content-Type", "application/json");
    Send(Bearer(req), (code, _) => { if (code != 200) Debug.LogWarning("Cloud save push failed: " + code); });
   }
  });
 }

 public void PullAll(Action<Dictionary<string,string>> done) {
  var result = new Dictionary<string,string>();
  Authed(ok => {
   if (!ok) { done(result); return; }
   Send(Bearer(UnityWebRequest.Get(Saves() + "?pageSize=300")), (code, body) => {
    if (code == 200) foreach (var d in JsonUtility.FromJson<ListReply>(body).documents ?? new DocReply[0])
     if (d.fields?.entry?.stringValue != null) result[d.name.Substring(d.name.LastIndexOf('/') + 1)] = d.fields.entry.stringValue;
    done(result);
   });
  });
 }

 // Mağaza ve KVKK silme hakkı: buluttaki her kayıt, sonra kimlik silinir. Bir adım
 // başarısız olursa hata döner ve yerel kayıt silinmez; oyuncu tekrar deneyebilir.
 public void Delete(Action<string> done) {
  if (!CloudReady) { done(null); return; }
  Authed(ok => {
   if (!ok) { done("account.error.offline"); return; }
   Send(Bearer(UnityWebRequest.Get(Saves() + "?pageSize=300")), (code, body) => {
    if (code != 200 && code != 404) { done(Error(code)); return; }
    var names = new List<string>();
    if (code == 200) foreach (var d in JsonUtility.FromJson<ListReply>(body).documents ?? new DocReply[0]) names.Add(d.name);
    DeleteDocs(names, 0, error => {
     if (error != null) { done(error); return; }
     Send(Post(Identity + "delete?key=" + apiKey, "{\"idToken\":" + Quote(idToken) + "}"), (c, _) => {
      if (c != 200) { done(Error(c)); return; }
      uid = idToken = null; PlayerPrefs.DeleteKey(RefreshKey); PlayerPrefs.Save();
      done(null);
     });
    });
   });
  });
 }

 void DeleteDocs(List<string> names, int i, Action<string> done) {
  if (i >= names.Count) { done(null); return; }
  Send(Bearer(UnityWebRequest.Delete("https://firestore.googleapis.com/v1/" + names[i])), (code, _) => {
   if (code != 200 && code != 404) done(Error(code)); else DeleteDocs(names, i + 1, done);
  });
 }

 string Saves() => documents + "users/" + uid + "/saves";
 string Doc(string key) => Saves() + "/" + UnityWebRequest.EscapeURL(key);
 static string Error(long code) => code == 0 ? "account.error.offline" : "account.error.failed";
 static string Quote(string s) => "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";

 UnityWebRequest Bearer(UnityWebRequest req) { req.SetRequestHeader("Authorization", "Bearer " + idToken); return req; }
 static UnityWebRequest Post(string url, string body, string type = "application/json") {
  var req = new UnityWebRequest(url, "POST") { uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body)), downloadHandler = new DownloadHandlerBuffer() };
  req.SetRequestHeader("Content-Type", type);
  return req;
 }
 // Ağ hatasında kod 0 döner. Yanıt geri çağrıya verilir, istek sonra bırakılır.
 static void Send(UnityWebRequest req, Action<long,string> done) {
  req.timeout = 20;
  req.SendWebRequest().completed += _ => {
   long code = req.result == UnityWebRequest.Result.ConnectionError ? 0 : req.responseCode;
   string text = req.downloadHandler?.text;
   req.Dispose();
   done(code, text);
  };
 }

 [Serializable] class SignUpReply { public string localId, idToken, refreshToken, expiresIn; }
 [Serializable] class RefreshReply { public string user_id, id_token, refresh_token, expires_in; }
 [Serializable] class ListReply { public DocReply[] documents; }
 [Serializable] class DocReply { public string name; public Fields fields; }
 [Serializable] class Fields { public StringValue entry; }
 [Serializable] class StringValue { public string stringValue; }
}
}
