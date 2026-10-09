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
  // İzin gerektiren inceleme bekliyorsa sonuç geldiği an bir kez haber verilir.
  var ready=ReadyRequestTicks();
  if(ready>DateTime.UtcNow.Ticks)Notifications.Provider.Schedule(3,T("notify.title"),T("notify.ready"),new DateTime(ready,DateTimeKind.Utc)-DateTime.UtcNow);
  string key=NotifyKey();
  if(key==null)return;
  for(int i=0;i<NotifyAfter.Length;i++)Notifications.Provider.Schedule(i+1,T("notify.title"),T(key+"."+(i+1)),NotifyAfter[i]);
 }
 long ReadyRequestTicks() {
  if(game.State.closed)return 0;
  var waits=game.State.documentRequests.Select(r=>r.readyAtUtcTicks).Concat((game.State.warrantDenials??new System.Collections.Generic.List<WarrantDenial>()).Select(d=>d.readyAtUtcTicks))
   .Where(t=>t>DateTime.UtcNow.Ticks+TimeSpan.FromMinutes(2).Ticks).ToArray();
  return waits.Length==0?0:waits.Min();
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
