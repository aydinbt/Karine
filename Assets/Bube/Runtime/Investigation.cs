using System;
using System.Collections.Generic;
using System.Linq;
namespace Bube {
[Serializable] public class Entry { public string key; public string value; }
[Serializable] public class Locale {
 public Entry[] entries;
 [NonSerialized] Dictionary<string,string> index;
 public string Get(string key) {
  if (index == null) {
   index = new Dictionary<string,string>(entries != null ? entries.Length : 0);
   if (entries != null) foreach (var entry in entries) if (entry != null && entry.key != null && !index.ContainsKey(entry.key)) index[entry.key] = entry.value;
  }
  return index.TryGetValue(key, out var value) && value != null ? value : "[" + key + "]";
 }
 // İsteğe bağlı metinler için: anahtar yoksa "[anahtar]" basmak yerine atlanır.
 public bool Has(string key) => !string.IsNullOrEmpty(key) && Get(key)[0] != '[';
}
[Serializable] public class GameConfig { public string title; public string locale; public string initialCase; public string investigatorKey; public WorldIntro[] worldIntros; }
// Uretici filigrani filmin sag alt kosesinde duruyor. "Gec" dugmesini tam
// oraya koyup ustunu ortuyoruz; koordinatlar filmin kendi karesine oranlidir
// (0..1), boylece her video kendi filigran yerini soyleyebilir.
[Serializable] public class CornerMark { public float x=.9055f; public float y=.8333f; public float w=.0609f; public float h=.1056f; }
[Serializable] public class WorldIntro { public string id; public string firstCaseId; public string countryKey; public string locationKey; public string flagResource; public string videoPath; public bool graphicsEmbedded; public bool skipCoversCornerMark; public CornerMark skipMark; public bool deskArrival; public string deskArrivalVideo; public CornerMark deskArrivalMark; }
[Serializable] public class CaseData { public string id; public string titleKey; public bool draft; public Node[] nodes; public TimelineClue[] timelineClues; public Verdict[] verdicts; public Choice[] methods; public Choice[] evidence; public string[] conclusionRequires; public int successfulReportTrustGain; public int failedReportTrustLoss; public string nextCaseId; public CaseSummary summary; }
[Serializable] public class TimelineClue { public string id; public string timeKey; public string noteKey; public string sourceKey; public int sortMinute; public string[] requiresRead; public string[] requiresAsked; }
[Serializable] public class CaseSummary { public string locationKey; public string truthKey; public string evidenceKey; public string lessonKey; }
[Serializable] public class CareerRules { public int initialTrust = 60; public int strongGain = 5; public int incompleteLoss = 5; public int falseAccusationLoss = 15; public int unsolvedLoss = 2; public int endThreshold = 0; public int[] statusThresholds = {80,60,40,20,1}; }
[Serializable] public class PendingReview { public string caseId; public bool correct; public string evaluationType; public int trustDelta; public int successGain; public int failureLoss; public long readyAtUtcTicks; public string suspectId; public string methodId; public string proofId; public string suspectSourceId; public string methodSourceId; public string proofSourceId; public bool suspectSupported; public bool methodSupported; public bool proofSupported; }
[Serializable] public class FaxReview { public string caseId; public bool correct; public string evaluationType; public long evaluatedAtUtcTicks; public int trustChange; public int trustAfter; public string suspectId; public string methodId; public string proofId; public string suspectSourceId; public string methodSourceId; public string proofSourceId; public bool suspectSupported; public bool methodSupported; public bool proofSupported; }
[Serializable] public class CareerProgress { public int version = 1; public int departmentTrust = 60; public int retirementThreshold = 0; public string activeCaseId; public List<string> seenWorldIntros = new List<string>(); public List<PendingReview> pendingReviews = new List<PendingReview>(); public bool faxReleased; public FaxReview lastFax; public List<FaxReview> reviewHistory = new List<FaxReview>(); public string careerRankId = "investigator"; public bool retired; }
[Serializable] public class FileMeta { public string labelKey; public string valueKey; }
[Serializable] public class AnswerVariant { public string answerKey; public string[] requiresAsked; public string[] requiresRead; public string[] excludesAsked; }
[Serializable] public class PresentedAnswer { public string sourceId; public string answerKey; }
[Serializable] public class Question { public string id; public string topicKey; public string[] aboutPersonIds; public string promptKey; public string answerKey; public string[] requiresAsked; public string[] requiresAnyAsked; public string[] excludesAsked; public string[] requiresRead; public string presentedSourceId; public string[] presentedSourceIds; public PresentedAnswer[] presentedAnswers; public PresentedAnswer[] decoyAnswers; public AnswerVariant[] answerVariants; }
[Serializable] public class Node { public FileMeta[] fileMeta; public string imageResource; public string imageCaptionKey; public string id; public string kind; public string titleKey; public string bodyKey; public string[] requires; public string[] requiresAny; public string[] requiresAsked; public string[] requiresAnyAsked; public bool requestable; public string requestLabelKey; public int requestDelaySeconds; public string personId; public string personNameKey; public string personInfoKey; public string personQuoteKey; public Question[] questions; public string[] completionQuestionIds; public string cctvSourceKey; public string cctvOverlayKey; public string cctvPeriodKey; public CctvEvent[] cctvEvents; public string deflectAnswerKey; public bool notPresentable; public string[] aboutPersonIds; }
[Serializable] public class CctvEvent { public string id; public string textKey; public string[] aboutPersonIds; public bool notPresentable; public string overlayTimeKey; public string glitchKey; public string signalKey; public string videoPath; public int delayMs; }
[Serializable] public class Choice { public string id; public string labelKey; public bool correct; public string[] supportingSourceIds; }
[Serializable] public class Verdict { public string id; public string labelKey; public string feedbackKey; public bool correct; public string[] requires; public string[] supportingSourceIds; }
[Serializable] public class InterviewRequest { public string nodeId; public long readyAtUtcTicks; }
[Serializable] public class DocumentRequest { public string nodeId; public long readyAtUtcTicks; }
[Serializable] public class InterviewTurn { public string nodeId; public string questionId; public string promptKey; public string answerKey; public string sourceId; }
[Serializable] public class Progress { public int version = 1; public string caseId; public bool caseAccepted; public List<string> read = new List<string>(); public List<string> asked = new List<string>(); public List<InterviewRequest> interviewRequests = new List<InterviewRequest>(); public List<DocumentRequest> documentRequests = new List<DocumentRequest>(); public List<InterviewTurn> interviewTurns = new List<InterviewTurn>(); public List<string> timelinePinned = new List<string>(); public int seenInterviewTurns; public bool closed; public string reportSuspect; public string reportMethod; public string reportProof; public string reportSuspectSource; public string reportMethodSource; public string reportProofSource; public long submittedAtUtcTicks; }
// Kayit gocu. Eski surumden gelen kayit atilmaz, bugunku semaya yukseltilir;
// gelecekten gelen (daha yeni surumlu) kayit cevrilemez ama silinmez de — oldugu
// gibi birakilir ve oyuncuya soylenir.
public enum SaveOutcome { Fresh, Loaded, Migrated, FromFuture, OtherCase }

public static class SaveMigration {
 public const int ProgressVersion = 1;
 public const int CareerVersion = 1;

 static SaveOutcome Compare(int version,int current) =>
  version==current ? SaveOutcome.Loaded : version>current ? SaveOutcome.FromFuture : SaveOutcome.Migrated;

 // Sema buyudugunde buraya bir basamak eklenir: `if(save.version<2){...;save.version=2;}`.
 // Basamaklar sirayla kosar, boylece cok eski bir kayit da bugune kadar tirmanir.
 public static SaveOutcome Migrate(Progress save) {
  if(save==null)return SaveOutcome.Fresh;
  var outcome=Compare(save.version,ProgressVersion);
  if(outcome!=SaveOutcome.Migrated)return outcome;
  // 0 = surum alani hic yazilmamis ilk kayitlar; sema aynidir, damgalamak yeter.
  if(save.version<1)save.version=1;
  return SaveOutcome.Migrated;
 }

 public static SaveOutcome Migrate(CareerProgress save) {
  if(save==null)return SaveOutcome.Fresh;
  var outcome=Compare(save.version,CareerVersion);
  if(outcome!=SaveOutcome.Migrated)return outcome;
  if(save.version<1)save.version=1;
  return SaveOutcome.Migrated;
 }
}

public sealed class Investigation {
 public CaseData Data { get; }
 // Ad geçme kuralı metne bakar; metin olmadan hiçbir kaynak kişiyle eşleşmez.
 public Locale Text { get; set; }
 public Progress State { get; }
 public CareerProgress Career { get; }
 public CareerRules Rules { get; }
 // Kayit gocunun sonucu. `FromFuture` olan kayit devralinmaz ve uzerine yazilmaz.
 public SaveOutcome StateOutcome { get; }
 public SaveOutcome CareerOutcome { get; }
 public Investigation(CaseData data, Progress progress = null, CareerProgress career = null, CareerRules rules = null) {
  Rules=rules ?? new CareerRules();
  CareerOutcome=SaveMigration.Migrate(career);
  bool adoptCareer=CareerOutcome==SaveOutcome.Loaded || CareerOutcome==SaveOutcome.Migrated;
  Career = adoptCareer ? career : new CareerProgress();
  if(!adoptCareer)Career.departmentTrust=Rules.initialTrust;
  Career.departmentTrust = Math.Max(0,Math.Min(100,Career.departmentTrust));
  Career.retirementThreshold=Rules.endThreshold;
  Career.seenWorldIntros=Career.seenWorldIntros ?? new List<string>();
  Career.reviewHistory=(Career.reviewHistory ?? new List<FaxReview>()).Where(r=>r!=null && !string.IsNullOrEmpty(r.caseId)).GroupBy(r=>r.caseId).Select(g=>g.First()).ToList();
  if(Career.lastFax!=null && !Career.reviewHistory.Any(r=>r.caseId==Career.lastFax.caseId))Career.reviewHistory.Add(Career.lastFax);
  if(string.IsNullOrEmpty(Career.careerRankId))Career.careerRankId="investigator";
  Career.pendingReviews = (Career.pendingReviews ?? new List<PendingReview>()).Where(r=>r!=null && !string.IsNullOrEmpty(r.caseId)).ToList();
  var stateOutcome=SaveMigration.Migrate(progress);
  if((stateOutcome==SaveOutcome.Loaded || stateOutcome==SaveOutcome.Migrated) && progress.caseId!=data.id)stateOutcome=SaveOutcome.OtherCase;
  StateOutcome=stateOutcome;
  bool adoptState=stateOutcome==SaveOutcome.Loaded || stateOutcome==SaveOutcome.Migrated;
  Data = data; State = adoptState ? progress : new Progress { caseId = data.id };
  State.read = (State.read ?? new List<string>()).Where(id => data.nodes.Any(n => n.id == id)).Distinct().ToList();
  State.asked = (State.asked ?? new List<string>()).Where(id => data.nodes.Any(n => (n.questions ?? new Question[0]).Any(q => q.id == id))).Distinct().ToList();
  State.timelinePinned=(State.timelinePinned ?? new List<string>()).Where(id=>(data.timelineClues ?? new TimelineClue[0]).Any(c=>c.id==id)).Distinct().ToList();
  State.interviewTurns=(State.interviewTurns ?? new List<InterviewTurn>()).Where(turn=>turn!=null && data.nodes.Any(n=>n.id==turn.nodeId && (n.questions ?? new Question[0]).Any(q=>q.id==turn.questionId && q.promptKey==turn.promptKey)) && !string.IsNullOrEmpty(turn.answerKey)).ToList();
  State.seenInterviewTurns=Math.Max(0,Math.Min(State.seenInterviewTurns,State.interviewTurns.Count));
  State.interviewRequests = (State.interviewRequests ?? new List<InterviewRequest>()).Where(r => r != null && data.nodes.Any(n => n.id == r.nodeId && n.kind == "interview")).GroupBy(r => r.nodeId).Select(g => g.First()).ToList();
  State.documentRequests=(State.documentRequests ?? new List<DocumentRequest>()).Where(r=>r!=null && data.nodes.Any(n=>n.id==r.nodeId && n.kind=="document" && n.requestable)).GroupBy(r=>r.nodeId).Select(g=>g.First()).ToList();
  if(State.read.Count>0 || State.asked.Count>0 || State.timelinePinned.Count>0 || State.interviewRequests.Count>0 || State.documentRequests.Count>0 || State.closed)State.caseAccepted=true;
 }
 public bool Meets(string[] ids) => ids == null || ids.All(State.read.Contains);
 public bool TimelineAvailable(TimelineClue clue) => clue!=null && State.caseAccepted && Meets(clue.requiresRead) && (clue.requiresAsked==null || clue.requiresAsked.All(State.asked.Contains));
 public bool PinTimeline(string id) {
  var clue=(Data.timelineClues ?? new TimelineClue[0]).FirstOrDefault(c=>c.id==id);
  if(State.closed || !TimelineAvailable(clue) || State.timelinePinned.Contains(id))return false;
  State.timelinePinned.Add(id);return true;
 }
 public bool UnpinTimeline(string id) {
  if(State.closed)return false;
  return State.timelinePinned.Remove(id);
 }
 public bool AcceptCase() { if(Career.retired || State.closed || State.caseAccepted)return false; State.caseAccepted=true; return true; }
 public bool Discovered(Node n) => State.caseAccepted && !Career.retired && !State.closed && Meets(n.requires) && (n.requiresAny == null || n.requiresAny.Length == 0 || n.requiresAny.Any(State.read.Contains)) && (n.requiresAsked==null || n.requiresAsked.All(State.asked.Contains)) && (n.requiresAnyAsked==null || n.requiresAnyAsked.Length==0 || n.requiresAnyAsked.Any(State.asked.Contains));
 public bool Requested(Node n) => State.interviewRequests.Any(r => r.nodeId == n.id);
 public bool Pending(Node n) => n.kind == "interview" && Requested(n) && !State.read.Contains(n.id) && !Available(n);
 public bool CanRequest(Node n) => n.kind == "interview" && Discovered(n) && !Requested(n) && !State.read.Contains(n.id);
 public bool RequestInterview(string id, double waitSeconds = 4) {
  var n = Data.nodes.FirstOrDefault(x => x.id == id);
  if (n == null || !CanRequest(n)) return false;
  State.interviewRequests.Add(new InterviewRequest { nodeId = id, readyAtUtcTicks = DateTime.UtcNow.AddSeconds(waitSeconds).Ticks });
  return true;
 }
 public bool CanRequestDocument(Node n) => n.kind=="document" && n.requestable && Discovered(n) && !State.read.Contains(n.id) && !State.documentRequests.Any(r=>r.nodeId==n.id);
 public bool RequestDocument(string id,double? waitSeconds=null) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  if(n==null || !CanRequestDocument(n))return false;
  State.documentRequests.Add(new DocumentRequest { nodeId=id,readyAtUtcTicks=DateTime.UtcNow.AddSeconds(Math.Max(0,waitSeconds ?? n.requestDelaySeconds)).Ticks });
  return true;
 }
 public bool IncomingDocument(Node n) => n.kind=="document" && n.requestable && !State.read.Contains(n.id) && State.documentRequests.Any(r=>r.nodeId==n.id && r.readyAtUtcTicks<=DateTime.UtcNow.Ticks);
 public bool HasIncomingDocument => Data.nodes.Any(IncomingDocument);
 public bool ReceiveDocument(string id) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  if(n==null || !IncomingDocument(n))return false;
  State.read.Add(id);return true;
 }
 public bool Available(Node n) => Discovered(n) && (n.kind != "interview" || State.read.Contains(n.id) || State.interviewRequests.Any(r => r.nodeId == n.id && r.readyAtUtcTicks <= DateTime.UtcNow.Ticks)) && (!n.requestable || n.kind!="document" || State.read.Contains(n.id));
 public bool QuestionAvailable(Node n, Question q) => Available(n) && (q.requiresAsked == null || q.requiresAsked.All(State.asked.Contains)) && (q.requiresAnyAsked == null || q.requiresAnyAsked.Length==0 || q.requiresAnyAsked.Any(State.asked.Contains)) && (q.excludesAsked == null || !q.excludesAsked.Any(State.asked.Contains)) && Meets(q.requiresRead);
 public string AnswerKey(Question q,string sourceId=null) => (q.presentedAnswers ?? new PresentedAnswer[0]).FirstOrDefault(v=>v.sourceId==sourceId)?.answerKey
  ?? (q.answerVariants ?? new AnswerVariant[0]).FirstOrDefault(v => (v.requiresAsked==null || v.requiresAsked.All(State.asked.Contains)) && Meets(v.requiresRead) && (v.excludesAsked==null || !v.excludesAsked.Any(State.asked.Contains)))?.answerKey ?? q.answerKey;
 // Yem kaynak: gerçekten o kişiyle ilgili, öne sürmesi mantıklı, ama soruyu
 // kapatmayan kaynak. Karşılığında baştan savma bir cümle değil, gerçek bir
 // yanıt gelir — doğru ama yanıltıcı. Oyuncu kafasında birden çok okuma
 // taşısın diye vardır. `presentedSourceIds` "bu mesele biter" demektir;
 // yem oraya konmaz, yoksa vakayı çözmüş sayılırız.
 public string DecoyAnswerKey(Question q,string sourceId) =>
  string.IsNullOrEmpty(sourceId) ? null :
  (q.decoyAnswers ?? new PresentedAnswer[0]).FirstOrDefault(v=>v.sourceId==sourceId)?.answerKey;
 public bool QuestionNeedsSource(Question q) => !string.IsNullOrEmpty(q.presentedSourceId) || q.presentedSourceIds!=null && q.presentedSourceIds.Length>0;
 public bool SourceMatchesQuestion(Question q,string sourceId) => !QuestionNeedsSource(q) || ReportSourceAvailable(sourceId) &&
  (sourceId==q.presentedSourceId || q.presentedSourceIds!=null && q.presentedSourceIds.Contains(sourceId));
 public bool SourceAlreadyPresented(Node n,Question q,string sourceId) => State.interviewTurns.Any(t=>t.nodeId==n.id && t.questionId==q.id && t.sourceId==sourceId);
 // Yanitlanan soru listeden cikar. Bir soruyu birden cok kaynak kapatabilir
 // (`presentedSourceIds`), ama kisi cevabini bir kez verdikten sonra ayni seyi
 // ikinci bir kayitla tekrar sormak oyuncuya "bir sey eksik kaldi" izlenimi
 // veriyordu; oysa mesele kapanmisti.
 public bool CanAskQuestion(Node n,Question q) => QuestionAvailable(n,q) && !State.asked.Contains(q.id);
 // Temel kural: **adı geçtiyse cevap verme hakkı doğar.** Bir kaydı ancak
 // karşımızdaki kişinin adı orada geçiyorsa öne sürebiliriz; geçmiyorsa o kayıt
 // onu ilgilendirmez. Kural metinden türetilir, elle etiketlemeye bağlı değildir,
 // böylece yeni vakalarda kendiliğinden işler.
 //
 // `aboutPersonIds` elle konan **ek**tir, metin kuralının yerini almaz: yalnız
 // kaydın kişiden adını anmadan söz ettiği yerler için vardır. "Kadın şahıs
 // binaya girdi" Elif'i anlatır ama adını anmaz; Hasan'ın gördüğü "kadın" da
 // öyle. Ekleme olduğu için adı geçenler her hâlükârda görebilir.
 //
 // Bu bir **doğruluk** süzgeci değildir: bir kişiyle ilgili birçok kayıt kalır,
 // hangisinin belirleyici olduğunu oyuncu bulur.
 public bool SourceConcernsPerson(Node subject,string[] aboutPersonIds,string sourceText) {
  if(string.IsNullOrEmpty(subject?.personId))return true;
  return aboutPersonIds!=null && aboutPersonIds.Contains(subject.personId)
   || MentionsPerson(subject.personId,sourceText);
 }
 public bool MentionsPerson(string personId,string sourceText) {
  if(Text==null || string.IsNullOrEmpty(sourceText))return false;
  var haystack=Fold(sourceText);
  foreach(var name in PersonNames(personId))if(NameOccurs(haystack,Fold(name)))return true;
  return false;
 }
 IEnumerable<string> PersonNames(string personId) {
  foreach(var n in Data.nodes) {
   if(n.personId!=personId || string.IsNullOrEmpty(n.personNameKey))continue;
   var full=Text.Get(n.personNameKey);
   if(full.Length==0 || full[0]=='[')continue;
   yield return full;
   var space=full.IndexOf(' ');
   if(space>0)yield return full.Substring(0,space);
  }
 }
 // Türkçe küçültme: `ToLowerInvariant` "I"yı "i"ye çevirir, biz "ı" isteriz.
 static string Fold(string value) =>
  value.Replace('İ','i').Replace('I','ı').ToLowerInvariant();
 // Ad sınırı: önünde harf olmamalı. Ardından harf gelebilir, çünkü Türkçede ek
 // kesmesiz de yazılır ("Hasanla"); "Mert'in" zaten kesmeyle ayrılır.
 static bool NameOccurs(string haystack,string name) {
  for(int i=haystack.IndexOf(name,StringComparison.Ordinal);i>=0;
      i=haystack.IndexOf(name,i+1,StringComparison.Ordinal))
   if(i==0 || !char.IsLetterOrDigit(haystack[i-1]))return true;
  return false;
 }
 public Question FindQuestion(Node n,string questionId) => n?.questions?.FirstOrDefault(q=>q.id==questionId);
 public bool InterviewComplete(Node n) => n.kind == "interview" && n.completionQuestionIds != null && n.completionQuestionIds.Length > 0 && n.completionQuestionIds.All(State.asked.Contains);
 public bool Ask(string nodeId, string questionId,string sourceId=null) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==nodeId);
  var q=n?.questions?.FirstOrDefault(x=>x.id==questionId);
  if(n==null || n.kind!="interview" || q==null || !CanAskQuestion(n,q))return false;
  if(!SourceMatchesQuestion(q,sourceId) || QuestionNeedsSource(q) && SourceAlreadyPresented(n,q,sourceId))return false;
  var answerKey=AnswerKey(q,sourceId);
  if(!State.asked.Contains(q.id))State.asked.Add(q.id);
  State.interviewTurns.Add(new InterviewTurn { nodeId=n.id, questionId=q.id, promptKey=q.promptKey, answerKey=answerKey, sourceId=sourceId });
  if(InterviewComplete(n) && !State.read.Contains(n.id))State.read.Add(n.id);
  return true;
 }
 public bool Read(string id) {
  var n = Data.nodes.FirstOrDefault(x => x.id == id);
  if (n == null || !Available(n) || n.kind == "interview" && !InterviewComplete(n)) return false;
  if (!State.read.Contains(id)) State.read.Add(id);
  return true;
 }
 public bool CanConclude => !Career.retired && !State.closed && Meets(Data.conclusionRequires);
 public string InterviewTurnReference(InterviewTurn turn) => turn.nodeId+"#"+turn.questionId+(string.IsNullOrEmpty(turn.sourceId)?"":"|"+turn.sourceId);
 public InterviewTurn InterviewSourceTurn(string reference) {
  if(string.IsNullOrEmpty(reference))return null;
  int separator=reference.IndexOf('#');
  if(separator<0)return null;
  string nodeId=reference.Substring(0,separator);
  return State.interviewTurns.FirstOrDefault(t=>t.nodeId==nodeId && InterviewTurnReference(t)==reference);
 }
 public bool ReportSourceAvailable(string id) {
  if(string.IsNullOrEmpty(id))return false;
  int separator=id.IndexOf('#');
  string nodeId=separator<0?id:id.Substring(0,separator);
  var node=Data.nodes.FirstOrDefault(n=>n.id==nodeId);
  if(node==null || !(State.read.Contains(nodeId) || node.kind=="interview" && State.interviewTurns.Any(t=>t.nodeId==nodeId)))return false;
  if(node.kind=="cctv")return separator>=0 && (node.cctvEvents ?? new CctvEvent[0]).Any(e=>e.id==id.Substring(separator+1));
  if(node.kind=="interview")return separator<0 || InterviewSourceTurn(id)!=null;
  return separator<0;
 }
 bool Supports(string[] sourceIds,string selectedId) => ReportSourceAvailable(selectedId) &&
  (sourceIds==null || sourceIds.Length==0 || sourceIds.Any(id=>id==selectedId ||
   !id.Contains("#") && selectedId.StartsWith(id+"#",StringComparison.Ordinal)));
 public bool SubmitFinalReport(string suspect,string method,string proof,string suspectSource,string methodSource,string proofSource) {
  if(!CanConclude || !Data.verdicts.Any(v=>v.id==suspect) || !Data.methods.Any(v=>v.id==method) || !Data.evidence.Any(v=>v.id==proof) || !State.read.Contains(proof) || !ReportSourceAvailable(suspectSource) || !ReportSourceAvailable(methodSource) || !ReportSourceAvailable(proofSource))return false;
  State.reportSuspect=suspect;State.reportMethod=method;State.reportProof=proof;
  State.reportSuspectSource=suspectSource;State.reportMethodSource=methodSource;State.reportProofSource=proofSource;
  State.submittedAtUtcTicks=DateTime.UtcNow.Ticks;
  State.closed=true;
  Career.faxReleased=Career.pendingReviews.Any(r=>r.readyAtUtcTicks>0);
  bool suspectCorrect=Data.verdicts.Any(v=>v.id==suspect && v.correct);
  bool personSupported=Data.verdicts.Any(v=>v.id==suspect && v.correct && Supports(v.supportingSourceIds,suspectSource));
  bool methodSupported=Data.methods.Any(v=>v.id==method && v.correct && Supports(v.supportingSourceIds,methodSource));
  bool proofSupported=Data.evidence.Any(v=>v.id==proof && v.correct && Supports(v.supportingSourceIds,proofSource));
  bool correct=personSupported && methodSupported && proofSupported;
  string evaluationType=!suspectCorrect?"falseAccusation":!personSupported || !methodSupported || !proofSupported?"incomplete":"supported";
  int trustDelta=evaluationType=="supported"?Rules.strongGain:evaluationType=="incomplete"?-Rules.incompleteLoss:-Rules.falseAccusationLoss;
  Career.pendingReviews.Add(new PendingReview {
   caseId=Data.id,correct=correct,evaluationType=evaluationType,trustDelta=trustDelta,
   successGain=Math.Max(0,Data.successfulReportTrustGain),
   failureLoss=Math.Max(0,Data.failedReportTrustLoss),
   suspectId=suspect,methodId=method,proofId=proof,
   suspectSourceId=suspectSource,methodSourceId=methodSource,proofSourceId=proofSource,
   suspectSupported=personSupported,methodSupported=methodSupported,proofSupported=proofSupported
  });
  return true;
 }
 public string TrustStatusKey { get {
  int value=Career.departmentTrust;var t=Rules.statusThresholds;
  if(value<=Rules.endThreshold)return "career.status.ended";
  if(t==null || t.Length!=5)t=new[]{80,60,40,20,1};
  return value>=t[0]?"career.status.high":value>=t[1]?"career.status.reliable":value>=t[2]?"career.status.monitored":value>=t[3]?"career.status.review":"career.status.risk";
 } }
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
  int before=Career.departmentTrust;
  int delta=string.IsNullOrEmpty(pending.evaluationType) ? (pending.correct?pending.successGain:-pending.failureLoss) : pending.trustDelta;
  Career.departmentTrust=Math.Max(0,Math.Min(100,before+delta));
  Career.retired=Career.departmentTrust<=Career.retirementThreshold;
  Career.lastFax=new FaxReview {
   caseId=pending.caseId,correct=pending.correct,evaluationType=string.IsNullOrEmpty(pending.evaluationType)?(pending.correct?"supported":"falseAccusation"):pending.evaluationType,
   evaluatedAtUtcTicks=DateTime.UtcNow.Ticks,trustChange=Career.departmentTrust-before,trustAfter=Career.departmentTrust,
   suspectId=pending.suspectId,methodId=pending.methodId,proofId=pending.proofId,
   suspectSourceId=pending.suspectSourceId,methodSourceId=pending.methodSourceId,proofSourceId=pending.proofSourceId,
   suspectSupported=pending.suspectSupported,methodSupported=pending.methodSupported,proofSupported=pending.proofSupported
  };
  if(!Career.reviewHistory.Any(r=>r.caseId==Career.lastFax.caseId))Career.reviewHistory.Add(Career.lastFax);
  return Career.lastFax;
 }

}
}
