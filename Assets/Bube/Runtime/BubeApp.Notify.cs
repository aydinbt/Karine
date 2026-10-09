using System;
using System.Linq;

namespace Bube {
// Geri çağırma bildirimleri (9 Ekim 2026). Yalnız masada bekleyen bir şey varsa kurulur
// ve sıradaki adımı söylemez: "masanda bir dosya var" der, nereye bakacağını demez.
// En fazla iki hatırlatma (ertesi gün, üç gün sonra); oyuncu dönünce hepsi silinir.
public sealed partial class BubeApp {
 static readonly TimeSpan[] NotifyAfter={TimeSpan.FromHours(20),TimeSpan.FromDays(3)};

 // Arkaya atılınca: önce eskiler silinir, sonra durum hâlâ bekliyorsa yenileri kurulur.
 void ScheduleReturn() {
  Notifications.Provider.CancelAll();
  if(!Notifications.Enabled || game==null || game.Career.retired)return;
  string key=NotifyKey();
  if(key==null)return;
  for(int i=0;i<NotifyAfter.Length;i++)Notifications.Provider.Schedule(i+1,T("notify.title"),T(key+"."+(i+1)),NotifyAfter[i]);
 }
 string NotifyKey() {
  if(game.State.caseAccepted && !game.State.closed)return "notify.open";
  if(HasIncomingFax || game.Career.pendingReviews.Any(r=>r.caseId==game.Data.id))return "notify.fax";
  if(!game.State.caseAccepted || AvailableAssignment()!=null)return "notify.new";
  return null;
 }
 // İzin bağlamında sorulur: ilk rapor gönderilip özet açılınca, bir kez.
 void AskNotifyOnce() {
  if(!Notifications.Asked)Notifications.Ask();
 }
}
}
