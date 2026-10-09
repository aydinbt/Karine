using System;
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
 void CloudDirty(){ if(Accounts.SignedIn && cloudDueAt<0)cloudDueAt=Time.unscaledTime+CloudPushDelay; }
 void FlushCloud(){ if(cloudDueAt>=0){cloudDueAt=-1;Accounts.Provider.Push(Accounts.LocalEntries());} }

 void AccountBoot() {
  Accounts.Provider.Restore((kind,id)=>{
   if(kind!=Accounts.Kind || (id??"")!=(Accounts.Id??"")){Accounts.Set(kind,id);ReloadAccount();}
   else if(Accounts.SignedIn)PullCloud();
   if(Accounts.Chosen)return;
   if(!Accounts.OffersSignIn){Accounts.MarkChosen();return;}
   // Giriş kağıdı açılış yüklemesi bittikten sonra çıkar.
   root.schedule.Execute(()=>{ if(root.Q("LoadingScreen")==null && root.Q("AccountPaper")==null && !Accounts.Chosen)AccountPaper(); })
    .Every(300).Until(()=>Accounts.Chosen || root.Q("AccountPaper")!=null);
  });
 }

 // Buluttaki yeni kopyalar yereli ezer, sonra yereldeki yeni kopyalar buluta gider.
 void PullCloud() {
  Accounts.Provider.PullAll(cloud=>{
   if(Accounts.Merge(cloud)>0)ReloadAccount();
   Accounts.Provider.Push(Accounts.LocalEntries());
  });
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
   // Editor'de servis yok ama düğmeler görünür kalır: ekran tasarımı denetlenebilsin, basınca "kullanılamıyor" der.
   if(Accounts.Provider.Supports(kind)||Application.isEditor){var k=kind;options.Add(new KarineUI.AccountOption(Key(k),T("account.signin."+Key(k)),()=>AccountSignIn(k)));}
  options.Add(new KarineUI.AccountOption(null,T("account.guest"),()=>{Accounts.MarkChosen();root.Q("AccountPaper")?.RemoveFromHierarchy();}));
  KarineUI.AccountPaper(root,T("account.tagline"),T("account.or"),options,accountNotice,
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

 void AccountSignOut() {
  Accounts.Provider.SignOut();Accounts.Set(AccountKind.Guest,null);
  accountNotice=T("account.notice.signedOut");ReloadAccount();
 }

 // Mağaza şartı (App Store 5.1.1(v), Google Play hesap silme): oyunun içinden,
 // onaylı, geri alınamaz silme. Misafirde yalnız cihazdaki ilerleme silinir.
 void AskDeleteAccount() {
  KarineUI.ConfirmPaper(root,T("account.delete.form"),T("account.delete.title"),
   T(Accounts.SignedIn?"account.delete.body":"account.delete.guestBody"),T("account.delete.stamp"),
   T("account.delete.cancel"),()=>{},T("account.delete.confirm"),DeleteAccount);
 }
 void DeleteAccount() {
  root.Q("ConfirmVeil")?.RemoveFromHierarchy();
  if(!Accounts.SignedIn){Accounts.Wipe(Accounts.GuestFolder);accountNotice=T("account.notice.deleted");ReloadAccount();return;}
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
  string status=Accounts.SignedIn?T("account.status."+Key(Accounts.Kind)):T("account.status.guest");
  var state=KarineUI.SettingRow(body,T("settings.account.status"),status);
  if(!string.IsNullOrEmpty(accountNotice))KarineUI.Body_(state,accountNotice,KarineTheme.SettingsModal.RowHintSize).style.color=KarineTheme.Secondary;
  if(!Accounts.SignedIn) {
   foreach(var kind in new[]{AccountKind.Apple,AccountKind.Google})
    // Editor'de servis yok ama düğmeler görünür kalır: ekran tasarımı denetlenebilsin, basınca "kullanılamıyor" der.
   if(Accounts.Provider.Supports(kind)||Application.isEditor){var k=kind;Button(KarineUI.SettingRow(body,T("account.link."+Key(k)),T("account.link.hint")),T("account.signin."+Key(k)),()=>AccountSignIn(k));}
  } else Button(KarineUI.SettingRow(body,T("account.signout"),T("account.signout.hint")),T("account.signout"),AccountSignOut);
  Button(KarineUI.SettingRow(body,T("account.delete"),T(Accounts.SignedIn?"account.delete.hint":"account.delete.guestHint")),T("account.delete"),AskDeleteAccount);
  if(!string.IsNullOrEmpty(config.accountDeletionUrl))
   Button(KarineUI.SettingRow(body,T("account.deletionPage"),T("account.deletionPage.hint")),T("account.open"),OpenDeletionPage);
  var privacy=KarineUI.SettingRow(body,T("account.privacy"),T("account.privacy.hint"));
  if(!string.IsNullOrEmpty(config.privacyUrl))Button(privacy,T("account.open"),OpenPrivacy);
 }
}
}
