using System;
using System.Linq;
namespace Bube {
// Kariyer: faks değerlendirmesi, seri, gözetimli masa görevi ve ödüllü yeniden deneme.
public sealed partial class Investigation {
 // Ödüllü yeniden deneme. Geri verilen tek şey güvendir: o faksın götürdüğü
 // puan iade edilir ve gerekirse görevden ayrılma kalkar. Faks geçmişi
 // başarısızlığı saklar — kayıt silinmez, "yeniden açıldı" diye işaretlenir.
 // Soruşturmada bulunanlar da silinmez ve hiçbir ipucu verilmez; yalnız rapor
 // alanları boşalır, yani vaka ikinci kez gerekçeli sonuç göndermeye açılır.
 public bool MayReopen => State.closed && Career.lastFax!=null && Career.lastFax.caseId==Data.id
  && !Career.lastFax.correct && !Career.lastFax.reopened
  && !Career.pendingReviews.Any(r=>r.caseId==Data.id);
 public bool ReopenForRetry() {
  if(!MayReopen)return false;
  var fax=Career.lastFax;
  fax.reopened=true;fax.trustRefunded=true;
  // Kayıt ile `lastFax` aynı örnektir, ama kayıttan yüklendiğinde iki ayrı
  // nesne olur; o yüzden vaka ve değerlendirme anıyla eşleştirilir.
  foreach(var record in Career.reviewHistory)
   if(record.caseId==fax.caseId && record.evaluatedAtUtcTicks==fax.evaluatedAtUtcTicks) {
    record.reopened=true;record.trustRefunded=true;
   }
  Career.departmentTrust=Math.Max(0,Math.Min(100,Career.departmentTrust-fax.trustChange));
  // Faksın getirdiği gözetim de geri alınır; seri, faks öncesindeki hâline döner.
  if(fax.startedProbation){Career.probation=false;Career.probationCount=Math.Max(0,Career.probationCount-1);}
  Career.streak=fax.streakBefore;
  State.closed=false;
  State.reportSuspect=State.reportMethod=State.reportProof=null;
  State.reportSuspectSource=State.reportMethodSource=State.reportProofSource=null;
  State.reportCustody=State.reportCustodySource=null;
  State.submittedAtUtcTicks=0;
  return true;
 }
 public string TrustStatusKey => Career.probation?"career.status.probation":StatusKeyFor(Career.departmentTrust,Rules);
 // Kademe yalnız güven değerinden türer; sicil geçmişi de aynı kuralla hesaplar.
 public static string StatusKeyFor(int value,CareerRules rules) {
  rules=rules??new CareerRules();var t=rules.statusThresholds;if(t==null || t.Length!=5)t=new[]{80,60,40,20,1};
  if(value<=rules.endThreshold)return "career.status.risk";
  return value>=t[0]?"career.status.high":value>=t[1]?"career.status.reliable":value>=t[2]?"career.status.monitored":value>=t[3]?"career.status.review":"career.status.risk";
 }
 public bool HasIncomingFax => Career.faxReleased && Career.pendingReviews.Any(r=>r.readyAtUtcTicks>0 && r.readyAtUtcTicks<=DateTime.UtcNow.Ticks);
 public void BeginNextCaseReview(double delaySeconds=7) {
  var pending=Career.pendingReviews.FirstOrDefault(r=>r.readyAtUtcTicks==0);
  if(pending==null)return;
  Career.faxReleased=true;
  if(pending.readyAtUtcTicks==0)pending.readyAtUtcTicks=DateTime.UtcNow.AddSeconds(Math.Max(5,Math.Min(10,delaySeconds))).Ticks;
 }
 public FaxReview DeliverNextFax() {
  if(!HasIncomingFax)return null;
  var pending=Career.pendingReviews.FirstOrDefault(r=>r.readyAtUtcTicks>0 && r.readyAtUtcTicks<=DateTime.UtcNow.Ticks);
  if(pending==null)return null;
  Career.pendingReviews.Remove(pending);
  Career.faxReleased=Career.pendingReviews.Any(r=>r.readyAtUtcTicks>0);
  int before=Career.departmentTrust,streakBefore=Career.streak;
  int delta=string.IsNullOrEmpty(pending.evaluationType) ? (pending.correct?pending.successGain:-pending.failureLoss) : pending.trustDelta;
  // Seri: arka arkaya gerekçeli doğru raporlar birim güvenini daha hızlı toplar.
  // Yanlış ya da eksik rapor seriyi sıfırlar.
  if(pending.correct){Career.streak++;if(Career.streak>=Math.Max(1,Rules.streakLength))delta+=Rules.streakBonus;}
  else Career.streak=0;
  int after=Math.Max(0,Math.Min(100,before+delta));
  bool startedProbation=false,endedProbation=false;
  // Kariyer bitmez. Güven tükenince Bora aynı birimde gözetimli masa görevine
  // alınır; sıradaki dosyayı gerekçeli kapatırsa göreve döner.
  if(Career.probation) {
   if(pending.correct){Career.probation=false;endedProbation=true;after=Math.Max(after,Rules.reinstateTrust);}
   else after=Math.Max(after,Rules.probationTrust);
  } else if(after<=Career.retirementThreshold) {
   Career.probation=true;Career.probationCount++;startedProbation=true;after=Rules.probationTrust;
  }
  Career.departmentTrust=after;Career.retired=false;
  Career.lastFax=new FaxReview {
   startedProbation=startedProbation,endedProbation=endedProbation,streakBefore=streakBefore,
   caseId=pending.caseId,correct=pending.correct,evaluationType=string.IsNullOrEmpty(pending.evaluationType)?(pending.correct?"supported":"falseAccusation"):pending.evaluationType,
   evaluatedAtUtcTicks=DateTime.UtcNow.Ticks,trustChange=Career.departmentTrust-before,trustAfter=Career.departmentTrust,
   suspectId=pending.suspectId,methodId=pending.methodId,proofId=pending.proofId,
   suspectSourceId=pending.suspectSourceId,methodSourceId=pending.methodSourceId,proofSourceId=pending.proofSourceId,
   suspectSupported=pending.suspectSupported,methodSupported=pending.methodSupported,proofSupported=pending.proofSupported,
   custodyId=pending.custodyId,custodySourceId=pending.custodySourceId,custodySupported=pending.custodySupported,
   hasRecon=pending.hasRecon,reconSupported=pending.reconSupported
  };
  // Bir vaka geçmişte birden çok satır tutabilir: ödüllü yeniden deneme eski
  // başarısızlığı silmez, yanına ikinci denemeyi yazar. Yeniden açılmamış bir
  // kayıt varsa aynı vaka ikinci kez eklenmez.
  if(!Career.reviewHistory.Any(r=>r.caseId==Career.lastFax.caseId && !r.reopened))Career.reviewHistory.Add(Career.lastFax);
  return Career.lastFax;
 }
}
}
