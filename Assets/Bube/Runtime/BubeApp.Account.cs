using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Hesap akışı: ilk açılışta giriş kağıdı (Google / Apple / misafir), ayarlarda
// "Hesap" sekmesi (bağla, çıkış, sil, gizlilik). Kayıt hesap klasöründe durur;
// giriş yapılmışsa değişiklikler birkaç saniye sonra buluta da yazılır.
// Bu dosya `BubeApp`in parçasıdır.
public sealed partial class BubeApp {
 const float CloudPushDelay=4f;
 float cloudDueAt=-1;bool accountBusy;string accountNotice;

 void AccountTick() {
  Accounts.Provider.Tick();
  if(cloudDueAt>=0 && Time.unscaledTime>=cloudDueAt){cloudDueAt=-1;Accounts.Provider.Push(Accounts.LocalEntries());}
 }
 // Buluta anlık iz yazılmaz: yalnız kariyerde bir dönüm noktası olunca (rapor gönderildi,
 // dosya değerlendirildi, rütbe ya da ödül değişti) bir kez yazılır.
 string cloudMark;
 string CareerMark(){var c=game.Career;return c.reviewHistory.Count+"|"+c.pendingReviews.Count+"|"+c.careerRankId+"|"+c.retired+"|"+c.unlockedLamps.Count;}
 void CloudDirty(){
  if(!Accounts.Provider.CloudReady || game?.Career==null)return;
  var mark=CareerMark();if(mark==cloudMark)return;
  cloudMark=mark;if(cloudDueAt<0)cloudDueAt=Time.unscaledTime+CloudPushDelay;
 }
 void FlushCloud(){ if(cloudDueAt>=0){cloudDueAt=-1;Accounts.Provider.Push(Accounts.LocalEntries());} }

 void AccountBoot() {
  Accounts.Provider.Restore((kind,id)=>{
   if(kind!=Accounts.Kind || (id??"")!=(Accounts.Id??"")){Accounts.Set(kind,id);ReloadAccount();}
   else if(Accounts.Provider.CloudReady)PullCloud();
   if(Accounts.Chosen)return;
   // Giriş kağıdı zorunlu: oyuncu Apple, Google ya da misafirden birini seçmeden oyuna
   // giremez. Açılış yüklemesi bittikten sonra çıkar; seçim bir kez yapılır, hatırlanır.
   root.schedule.Execute(()=>{ if(root.Q("LoadingScreen")==null && root.Q("AccountPaper")==null && !Accounts.Chosen)AccountPaper(); })
    .Every(300).Until(()=>Accounts.Chosen || root.Q("AccountPaper")!=null);
  });
 }

 // Buluttaki yeni kopyalar yereli ezer, sonra yereldeki yeni kopyalar buluta gider.
 void PullCloud() {
  Accounts.Provider.PullAll(cloud=>{
   if(Accounts.Merge(cloud)>0)ReloadAccount();
   var newer=Accounts.NewerThan(cloud);if(newer.Count>0)Accounts.Provider.Push(newer);
  });
 }

 // Eski sürümler bu ilerlemeyi cihazın ayar deposuna yazıyordu; ilk yüklenen kariyere
 // bir kez taşınır ve cihazdan silinir ki başka bir hesaba ikinci kez geçmesin.
 void AdoptDevicePrefs() {
  var c=game.Career;bool moved=false;
  if(PlayerPrefs.HasKey(PlayKey)){c.playSeconds=Mathf.Max(c.playSeconds,PlayerPrefs.GetFloat(PlayKey));PlayerPrefs.DeleteKey(PlayKey);moved=true;}
  if(PlayerPrefs.HasKey(SeenRankKey)){if(string.IsNullOrEmpty(c.seenRank))c.seenRank=PlayerPrefs.GetString(SeenRankKey);PlayerPrefs.DeleteKey(SeenRankKey);moved=true;}
  if(PlayerPrefs.HasKey(CreditsKey)){c.creditsSeen|=PlayerPrefs.GetInt(CreditsKey)==1;PlayerPrefs.DeleteKey(CreditsKey);moved=true;}
  for(int i=0;i<KarineTheme.Scene.LampTints.Length;i++) if(PlayerPrefs.HasKey(LampUnlockKey+i)){
   if(PlayerPrefs.GetInt(LampUnlockKey+i)==1 && !c.unlockedLamps.Contains(i))c.unlockedLamps.Add(i);
   PlayerPrefs.DeleteKey(LampUnlockKey+i);moved=true;}
  if(moved)PlayerPrefs.Save();
 }

 // Bozuk kayıt (yarım yazılmış, elle bozulmuş) yeni kayıtla ezilmez: ".corrupt" diye
 // yana kaldırılır ve bir önceki sağlam kopya (".bak", her kayıtta tutulur) okunur.
 static T ReadSave<T>(string path) where T:class {
  if(!File.Exists(path))return null;
  try { var value=JsonUtility.FromJson<T>(File.ReadAllText(path)); if(value!=null)return value; }
  catch(Exception e) { Debug.LogWarning("Save is corrupt: "+e.Message); }
  try { File.Copy(path,path+".corrupt",true); } catch {}
  var backup=path+".bak";
  try { if(File.Exists(backup)){var value=JsonUtility.FromJson<T>(File.ReadAllText(backup));if(value!=null){File.Copy(backup,path,true);return value;}} }
  catch(Exception e) { Debug.LogWarning("Backup save is corrupt too: "+e.Message); }
  return null;
 }

 void ReloadAccount() {
  bool redirected;LoadSaves(out redirected);if(redirected)Save();
  assignmentCacheId=null;
  if(root.Q("SettingsModal")!=null){CloseSettings();}
  root.Q("AccountPaper")?.RemoveFromHierarchy();
  Home();
 }

 void AccountPaper() {
  var options=new System.Collections.Generic.List<KarineUI.AccountOption>();
  foreach(var kind in new[]{AccountKind.Apple,AccountKind.Google})
   // Servis henüz bağlı değilse de düğme durur; basınca "kullanılamıyor" der, oyuncu misafiri seçer.
   {var k=kind;options.Add(new KarineUI.AccountOption(Key(k),T("account.signin."+Key(k)),()=>AccountSignIn(k)));}
  options.Add(new KarineUI.AccountOption(null,T("account.guest"),()=>{Accounts.MarkChosen();root.Q("AccountPaper")?.RemoveFromHierarchy();}));
  // Logo altı: hukuk ilkeleri ("|" ile ayrılmış); ekran açıkken sırayla değişir.
  KarineUI.AccountPaper(root,T("account.tagline").Split('|'),T("account.or"),options,accountNotice,
   T("account.privacy"),string.IsNullOrEmpty(config.privacyUrl)?null:(Action)OpenPrivacy);
 }

 static string Key(AccountKind kind)=>kind.ToString().ToLowerInvariant();

 void AccountSignIn(AccountKind kind) {
  if(accountBusy)return;accountBusy=true;
  bool link=!Accounts.SignedIn;string from=Accounts.Folder;
  Accounts.Provider.SignIn(kind,link,(id,error)=>{
   accountBusy=false;
   if(id==null){accountNotice=T(error);RefreshAccountUi();return;}
   Accounts.Set(kind,id);Accounts.MarkChosen();
   // Bağlama başarılıysa misafir ilerlemesi hesabın klasörüne geçer; hesap
   // zaten başka bir oyuncuya aitse misafir kaydı cihazda olduğu gibi kalır.
   if(link && error==null)Accounts.Adopt(from,Accounts.Folder);
   accountNotice=T(error??"account.notice.signedIn");
   ReloadAccount();PullCloud();
  });
 }

// Misafir Dosya #001'i bitirip #002'yi kabul edince bir kez: ilerlemesini korumak için
 // hesap bağlaması önerilir. "Daha sonra" akışı sürdürür; "Bağla" Ayarlar › Hesap'ı açar.
 const string LinkNudgeCase="case002";
 void LinkNudgeThen(Action next) {
  if(game.Data.id!=LinkNudgeCase || Accounts.SignedIn || game.Career.linkNudgeSeen){next();return;}
  game.Career.linkNudgeSeen=true;Save();
  KarineUI.ConfirmPaper(root,T("account.nudge.form"),T("account.nudge.title"),T("account.nudge.body"),T("account.nudge.stamp"),
   T("account.nudge.later"),next,T("account.nudge.link"),()=>{
    root.Q("ConfirmVeil")?.RemoveFromHierarchy();
    // Önce masaya geçilir; ayarlar kapanınca oyuncu kabul ettiği vakanın masasında olur.
    LoadThen(()=>{Desk();SettingsFrom(Desk);settingsTab=Array.IndexOf(SettingsTabs,"account");RenderSettings();});
   });
 }

 void AccountSignOut() {
  Accounts.Provider.SignOut();Accounts.Set(AccountKind.Guest,null);
  accountNotice=T("account.notice.signedOut");ReloadAccount();
 }

 // Mağaza şartı (App Store 5.1.1(v), Google Play hesap silme): oyunun içinden,
 // onaylı, geri alınamaz silme. Misafirde yalnız cihazdaki ilerleme silinir.
 void AskDeleteAccount() {
  KarineUI.ConfirmPaper(root,T("account.delete.form"),T("account.delete.title"),
   T(Accounts.SignedIn?"account.delete.body":Accounts.Provider.CloudReady?"account.delete.guestCloudBody":"account.delete.guestBody"),T("account.delete.stamp"),
   T("account.delete.cancel"),()=>{},T("account.delete.confirm"),DeleteAccount);
 }
 void DeleteAccount() {
  root.Q("ConfirmVeil")?.RemoveFromHierarchy();
  // Bulutsuz misafir: yalnız cihaz. Bulutlu misafirde önce sunucu silinir, sonra cihaz.
  if(!Accounts.SignedIn && !Accounts.Provider.CloudReady){Accounts.Wipe(Accounts.GuestFolder);accountNotice=T("account.notice.deleted");ReloadAccount();return;}
  if(accountBusy)return;accountBusy=true;
  Accounts.Provider.Delete(error=>{
   accountBusy=false;
   if(error!=null){accountNotice=T(error);RefreshAccountUi();return;}
   var folder=Accounts.Folder;Accounts.Set(AccountKind.Guest,null);Accounts.Wipe(folder);
   accountNotice=T("account.notice.deleted");ReloadAccount();
  });
 }

 void RefreshAccountUi() {
  if(root.Q("AccountPaper")!=null){root.Q("AccountPaper").RemoveFromHierarchy();AccountPaper();}
  else if(root.Q("SettingsModal")!=null)RenderSettings();
 }

 void OpenPrivacy(){ if(!string.IsNullOrEmpty(config.privacyUrl))Application.OpenURL(config.privacyUrl); }
 void OpenDeletionPage(){ if(!string.IsNullOrEmpty(config.accountDeletionUrl))Application.OpenURL(config.accountDeletionUrl); }
 void OpenSupport(){ if(!string.IsNullOrEmpty(config.supportEmail))Application.OpenURL("mailto:"+config.supportEmail); }

 void AccountSettings(VisualElement body) {
  string status=Accounts.SignedIn?T("account.status."+Key(Accounts.Kind)):T(Accounts.Provider.CloudReady?"account.status.guestCloud":"account.status.guest");
  var state=KarineUI.SettingRow(body,T("settings.account.status"),status);
  // Oyuncu kimliği: destek veya web silme talebinde oyuncunun kendini tanıtabilmesi için.
  if(Accounts.Provider.CloudReady)KarineUI.SettingRow(body,T("account.playerId"),Accounts.Provider.CloudId);
  if(!string.IsNullOrEmpty(accountNotice))KarineUI.Body_(state,accountNotice,KarineTheme.SettingsModal.RowHintSize).style.color=KarineTheme.Secondary;
  if(!Accounts.SignedIn) {
   foreach(var kind in new[]{AccountKind.Apple,AccountKind.Google})
    // Servis henüz bağlı değilse de satır durur; basınca "kullanılamıyor" der.
   {var k=kind;Button(KarineUI.SettingRow(body,T("account.link."+Key(k)),T("account.link.hint")),T("account.signin."+Key(k)),()=>AccountSignIn(k));}
  } else Button(KarineUI.SettingRow(body,T("account.signout"),T("account.signout.hint")),T("account.signout"),AccountSignOut);
  Button(KarineUI.SettingRow(body,T("account.delete"),T(Accounts.SignedIn?"account.delete.hint":Accounts.Provider.CloudReady?"account.delete.guestCloudHint":"account.delete.guestHint")),T("account.delete"),AskDeleteAccount);
  if(!string.IsNullOrEmpty(config.accountDeletionUrl))
   Button(KarineUI.SettingRow(body,T("account.deletionPage"),T("account.deletionPage.hint")),T("account.open"),OpenDeletionPage);
  var privacy=KarineUI.SettingRow(body,T("account.privacy"),T("account.privacy.hint"));
  if(!string.IsNullOrEmpty(config.privacyUrl))Button(privacy,T("account.open"),OpenPrivacy);
 }
}
}
