using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Uzaktan ayarın oyundaki karşılığı: sürüm kâğıdı ve durdurulan vaka (bkz. RemoteSettings).
public sealed partial class BubeApp {
 // Uzaktan ayar değişti: menüdeysek duyuru ve sürüm kâğıdı yeniden kurulsun.
 void OnRemoteSettings() {
  if(root!=null && root.Q("MenuExit")!=null && !askingToQuit)Home();
 }
 // Desteklenmeyen sürüm: oyun açılmaz, yalnız mağazaya yönlendirilir.
 bool ShowUpdateRequired() {
  if(!RemoteSettings.Outdated)return false;
  KarineUI.ConfirmPaper(root,T("update.form"),T("update.title"),T("update.body"),T("update.stamp"),
   T("update.quit"),QuitGame,
   T("update.confirm"),()=>{var url=RemoteSettings.StoreUrl;if(!string.IsNullOrEmpty(url))Application.OpenURL(url);});
  return true;
 }
 // Uzaktan durdurulan açık vaka: kayıt korunur, masaya girilmez.
 bool ShowCasePaused() {
  if(game.State.closed || !RemoteSettings.CasePaused(game.Data.id))return false;
  Home();
  KarineUI.ConfirmPaper(root,T("paused.form"),T("paused.title"),T("paused.body"),T("paused.stamp"),
   T("paused.back"),Home,T("paused.chapters"),WorldPage);
  return true;
 }
}
}
