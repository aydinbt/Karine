using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
#if KARINE_CLOUD
using Unity.Services.CloudSave;
#endif
#if KARINE_GPGS && UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
#if KARINE_APPLE && UNITY_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif

namespace Bube {
// Unity Gaming Services gerçeklemesi. Kimlik: UGS Authentication (anonim = misafir,
// Google Play Games ve Sign in with Apple = kalıcı hesap). Kayıt: UGS Cloud Save.
// Google yalnız Android'de, Apple yalnız iOS'ta sunulur; iOS'ta Google girişi
// olmadığı için App Store 4.8 kendiliğinden sağlanır, yine de Apple hep vardır.
// Servis kurulamazsa (proje bağlı değil, ağ yok) oyun yerel misafire düşer.
public sealed class UgsAccountProvider : IAccountProvider {
 bool ready;
#if KARINE_APPLE && UNITY_IOS
 AppleAuthManager apple;
#endif

 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
 static void Install() {
  if (!Application.isMobilePlatform) return;
  var provider = new UgsAccountProvider();
#if KARINE_GPGS && UNITY_ANDROID
  PlayGamesPlatform.Activate();
#endif
#if KARINE_APPLE && UNITY_IOS
  if (AppleAuthManager.IsCurrentPlatformSupported) provider.apple = new AppleAuthManager(new PayloadDeserializer());
#endif
  Accounts.Provider = provider;
 }

 public bool CloudReady => Accounts.SignedIn;
 public string CloudId => Accounts.Id;
 public bool Supports(AccountKind kind) {
  switch (kind) {
#if KARINE_GPGS && UNITY_ANDROID
   case AccountKind.Google: return true;
#endif
#if KARINE_APPLE && UNITY_IOS
   case AccountKind.Apple: return apple != null;
#endif
   case AccountKind.Guest: return true;
   default: return false;
  }
 }

 public void Tick() {
#if KARINE_APPLE && UNITY_IOS
  apple?.Update();
#endif
 }

 async Task<bool> Ready() {
  if (ready) return true;
  try { if (UnityServices.State != ServicesInitializationState.Initialized) await UnityServices.InitializeAsync(); ready = true; }
  catch (Exception e) { Debug.LogWarning("UGS unavailable: " + e.Message); }
  return ready;
 }
 static IAuthenticationService Auth => AuthenticationService.Instance;

 public async void Restore(Action<AccountKind,string> done) {
  // Kayıtlı oturum yoksa misafir kalınır; anonim oyuncu ancak bağlanırken açılır.
  // Ağ yokken ya da oturum düşmüşse hesap bırakılmaz: oyuncu kendi klasöründe
  // oynamaya devam eder, bulut eşitlemesi bir sonraki girişe kalır.
  if (Accounts.Kind == AccountKind.Guest || !await Ready() || !Auth.SessionTokenExists) { done(Accounts.Kind, Accounts.Id); return; }
  try { await Auth.SignInAnonymouslyAsync(); done(Accounts.Kind, Auth.PlayerId); }
  catch (Exception e) { Debug.LogWarning("Session restore failed: " + e.Message); done(Accounts.Kind, Accounts.Id); }
 }

 public void SignIn(AccountKind kind, bool link, Action<string,string> done) {
  Token(kind, async (token, error) => {
   if (token == null) { done(null, error ?? "account.error.cancelled"); return; }
   if (!await Ready()) { done(null, "account.error.offline"); return; }
   try {
    if (link) {
     if (!Auth.IsSignedIn) await Auth.SignInAnonymouslyAsync();
     try { await LinkAsync(kind, token); done(Auth.PlayerId, null); return; }
     catch (AuthenticationException e) when (e.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked) {
      // Hesap başka bir oyuncuya bağlı: o oyuncuya girilir, misafir ilerlemesi cihazda kalır.
      Auth.SignOut(true);
      // Kod bir kez kullanılır; yeniden istenir.
      Token(kind, async (again, err) => {
       if (again == null) { done(null, err ?? "account.error.cancelled"); return; }
       try { await SignInAsync(kind, again); done(Auth.PlayerId, "account.notice.existing"); }
       catch (Exception ex) { Debug.LogWarning(ex.Message); done(null, "account.error.failed"); }
      });
      return;
     }
    }
    if (Auth.IsSignedIn) Auth.SignOut(true);
    await SignInAsync(kind, token);
    done(Auth.PlayerId, null);
   } catch (Exception e) { Debug.LogWarning("Sign-in failed: " + e.Message); done(null, "account.error.failed"); }
  });
 }

 static Task SignInAsync(AccountKind kind, string token) =>
  kind == AccountKind.Apple ? Auth.SignInWithAppleAsync(token) : Auth.SignInWithGooglePlayGamesAsync(token);
 static Task LinkAsync(AccountKind kind, string token) =>
  kind == AccountKind.Apple ? Auth.LinkWithAppleAsync(token) : Auth.LinkWithGooglePlayGamesAsync(token);

 // Platformdan tek kullanımlık kimlik: Google'da sunucu yetki kodu, Apple'da kimlik belirteci.
 void Token(AccountKind kind, Action<string,string> done) {
#if KARINE_GPGS && UNITY_ANDROID
  if (kind == AccountKind.Google) {
   PlayGamesPlatform.Instance.ManuallyAuthenticate(status => {
    if (status != SignInStatus.Success) { done(null, "account.error.cancelled"); return; }
    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => done(string.IsNullOrEmpty(code) ? null : code, "account.error.failed"));
   });
   return;
  }
#endif
#if KARINE_APPLE && UNITY_IOS
  if (kind == AccountKind.Apple && apple != null) {
   apple.LoginWithAppleId(LoginOptions.None, credential => {
    var id = credential as IAppleIDCredential;
    done(id?.IdentityToken == null ? null : System.Text.Encoding.UTF8.GetString(id.IdentityToken), "account.error.failed");
   }, _ => done(null, "account.error.cancelled"));
   return;
  }
#endif
  done(null, "account.error.unavailable");
 }

 public void SignOut() {
  if (ready && Auth.IsSignedIn) Auth.SignOut(true);
 }

 public async void Delete(Action<string> done) {
  if (!await Ready() || !Auth.IsSignedIn) { done("account.error.offline"); return; }
  try {
#if KARINE_CLOUD
   try { await CloudSaveService.Instance.Data.Player.DeleteAllAsync(); } catch (Exception e) { Debug.LogWarning("Cloud wipe: " + e.Message); }
#endif
   await Auth.DeleteAccountAsync();
   done(null);
  } catch (Exception e) { Debug.LogWarning("Account delete failed: " + e.Message); done("account.error.failed"); }
 }

 public async void Push(IDictionary<string,string> entries) {
#if KARINE_CLOUD
  if (entries.Count == 0 || !ready || !Auth.IsSignedIn) return;
  try { await CloudSaveService.Instance.Data.Player.SaveAsync(entries.ToDictionary(p => p.Key, p => (object)p.Value)); }
  catch (Exception e) { Debug.LogWarning("Cloud push failed: " + e.Message); }
#endif
 }

 public async void PullAll(Action<Dictionary<string,string>> done) {
  var result = new Dictionary<string,string>();
#if KARINE_CLOUD
  if (ready && Auth.IsSignedIn) {
   try { foreach (var pair in await CloudSaveService.Instance.Data.Player.LoadAllAsync()) result[pair.Key] = pair.Value.Value.GetAsString(); }
   catch (Exception e) { Debug.LogWarning("Cloud pull failed: " + e.Message); }
  }
#endif
  done(result);
 }
}
}
