using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Bube {

// Hesabın **dikişi**. Giriş servisi (Unity Gaming Services + Google Play Games /
// Sign in with Apple) burada değil: `Bube.Accounts` derlemesi `IAccountProvider`ı
// gerçekleyip kendini takar; paket yoksa oyun misafir olarak, yerel kayıtla oynar.
// Mağaza kuralları: giriş **zorunlu değildir** (misafir her zaman var), giriş
// yapan oyuncu oyunun içinden çıkış yapabilir ve hesabını silebilir.
public enum AccountKind { Guest, Google, Apple }

public interface IAccountProvider {
 bool Supports(AccountKind kind);
 // Önceki oturum sessizce geri yüklenir; yoksa misafir döner.
 void Restore(Action<AccountKind,string> done);
 // `link`: misafir ilerlemesi bu hesaba taşınır. Sonuç: (oyuncu kimliği, hata anahtarı).
 void SignIn(AccountKind kind, bool link, Action<string,string> done);
 void SignOut();
 // Sunucudaki hesabı ve bulut kaydını siler. Sonuç: hata anahtarı ya da null.
 void Delete(Action<string> done);
 void Push(IDictionary<string,string> entries);
 void PullAll(Action<Dictionary<string,string>> done);
 void Tick();
}

// Servis yokken: yalnız misafir, bulut yok.
public sealed class LocalAccountProvider : IAccountProvider {
 public bool Supports(AccountKind kind) => kind == AccountKind.Guest;
 public void Restore(Action<AccountKind,string> done) => done(AccountKind.Guest, null);
 public void SignIn(AccountKind kind, bool link, Action<string,string> done) => done(null, "account.error.unavailable");
 public void SignOut() {}
 public void Delete(Action<string> done) => done(null);
 public void Push(IDictionary<string,string> entries) {}
 public void PullAll(Action<Dictionary<string,string>> done) => done(new Dictionary<string,string>());
 public void Tick() {}
}

// Buluta giden her kayıt bu zarfla gider: hangisi daha yeni, saatle karar verilir.
[Serializable] public class CloudEntry { public long ticks; public string json; }

public static class Accounts {
 public const string KindKey = "karine.account.kind", IdKey = "karine.account.id", ChosenKey = "karine.account.chosen";
 public const string GuestFolder = "guest";
 public static IAccountProvider Provider = new LocalAccountProvider();
 public static AccountKind Kind { get; private set; } = AccountKind.Guest;
 public static string Id { get; private set; }
 public static bool SignedIn => Kind != AccountKind.Guest && !string.IsNullOrEmpty(Id);
 // Giriş ekranı bir kez görülür; misafir seçen oyuncuya tekrar sorulmaz.
 public static bool Chosen => PlayerPrefs.GetInt(ChosenKey, 0) == 1;
 public static bool OffersSignIn => Provider.Supports(AccountKind.Google) || Provider.Supports(AccountKind.Apple);
 public static Func<string> Base = () => Application.persistentDataPath;

 public static void Load() {
  Kind = (AccountKind)PlayerPrefs.GetInt(KindKey, 0);
  Id = PlayerPrefs.GetString(IdKey, "");
  if (Kind != AccountKind.Guest && string.IsNullOrEmpty(Id)) Kind = AccountKind.Guest;
 }
 public static void MarkChosen() { PlayerPrefs.SetInt(ChosenKey, 1); PlayerPrefs.Save(); }
 public static void Set(AccountKind kind, string id) {
  Kind = id == null ? AccountKind.Guest : kind; Id = Kind == AccountKind.Guest ? "" : id;
  PlayerPrefs.SetInt(KindKey, (int)Kind); PlayerPrefs.SetString(IdKey, Id); PlayerPrefs.Save();
 }

 // Hesap başına ayrı klasör: aynı cihazda iki hesap birbirinin kaydını görmez.
 public static string Folder => SignedIn ? Safe(Id) : GuestFolder;
 public static string Root => FolderPath(Folder);
 public static string FolderPath(string folder) {
  var path = Path.Combine(Base(), "accounts", folder);
  Directory.CreateDirectory(path);
  return path;
 }
 static string Safe(string id) {
  var chars = id.ToCharArray();
  for (int i = 0; i < chars.Length; i++) if (!char.IsLetterOrDigit(chars[i]) && chars[i] != '-' && chars[i] != '_') chars[i] = '_';
  return new string(chars);
 }

 // Hesap sisteminden önceki kayıtlar kökte durur; misafir klasörüne taşınır.
 public static int MigrateLegacy() {
  int moved = 0;
  var guest = FolderPath(GuestFolder);
  foreach (var file in Directory.GetFiles(Base(), "bube-*")) {
   var target = Path.Combine(guest, Path.GetFileName(file));
   try { if (File.Exists(target)) continue; File.Move(file, target); moved++; }
   catch (Exception e) { Debug.LogWarning("Legacy save could not be moved: " + e.Message); }
  }
  return moved;
 }

 // Misafir bir hesaba bağlanınca ilerlemesi o hesabın klasörüne geçer. Hedefte
 // aynı adlı kayıt varsa dokunulmaz: hesabın kendi kaydı misafirinkini ezmez.
 public static void Adopt(string from, string to) {
  if (from == to) return;
  var source = FolderPath(from); var target = FolderPath(to);
  foreach (var file in Directory.GetFiles(source)) {
   var dest = Path.Combine(target, Path.GetFileName(file));
   try { if (!File.Exists(dest)) File.Move(file, dest); }
   catch (Exception e) { Debug.LogWarning("Save could not be adopted: " + e.Message); }
  }
 }

 public static void Wipe(string folder) {
  var path = Path.Combine(Base(), "accounts", folder);
  try { if (Directory.Exists(path)) Directory.Delete(path, true); }
  catch (Exception e) { Debug.LogWarning("Account data could not be wiped: " + e.Message); }
 }

 // Bulut anahtarı dosya adından türer: "bube-career-v1.json" → "bube-career-v1".
 public static string CloudKey(string file) => Path.GetFileNameWithoutExtension(file).Replace('.', '_');

 public static Dictionary<string,string> LocalEntries() {
  var entries = new Dictionary<string,string>();
  foreach (var file in Directory.GetFiles(Root, "bube-*.json")) {
   try { entries[CloudKey(file)] = JsonUtility.ToJson(new CloudEntry { ticks = File.GetLastWriteTimeUtc(file).Ticks, json = File.ReadAllText(file) }); }
   catch (Exception e) { Debug.LogWarning("Save could not be read for upload: " + e.Message); }
  }
  return entries;
 }

 // Buluttaki kopya yereldekinden yeniyse yereli ezer; eski olan hiçbir zaman kazanmaz.
 public static int Merge(Dictionary<string,string> cloud) {
  int written = 0;
  foreach (var pair in cloud) {
   CloudEntry entry;
   try { entry = JsonUtility.FromJson<CloudEntry>(pair.Value); } catch { continue; }
   if (entry == null || string.IsNullOrEmpty(entry.json) || !pair.Key.StartsWith("bube-")) continue;
   var path = Path.Combine(Root, pair.Key + ".json");
   if (File.Exists(path) && File.GetLastWriteTimeUtc(path).Ticks >= entry.ticks) continue;
   try { File.WriteAllText(path, entry.json); File.SetLastWriteTimeUtc(path, new DateTime(entry.ticks, DateTimeKind.Utc)); written++; }
   catch (Exception e) { Debug.LogWarning("Cloud save could not be written: " + e.Message); }
  }
  return written;
 }
}
}
