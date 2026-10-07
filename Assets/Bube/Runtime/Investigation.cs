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
 // Vaka başına dil dosyasını ortak dosyanın üstüne ekler. Çakışan anahtarda
 // ortak dosya kazanır: bir vaka ortak metni sessizce değiştirmesin.
 public void Absorb(Locale other) {
  if (other == null || other.entries == null) return;
  Get(string.Empty); // dizini kurar
  foreach (var entry in other.entries)
   if (entry != null && entry.key != null && !index.ContainsKey(entry.key)) index[entry.key] = entry.value;
 }
 // Birleştirmeden sonra hangi anahtarların var olduğunu bilmek gerekir
 // (kullanılmayan metin denetimi bunu okur).
 public System.Collections.Generic.IEnumerable<string> Keys { get { Get(string.Empty); return index.Keys; } }
}
[Serializable] public class GameConfig { public string title; public string locale; public string initialCase; public string investigatorKey; public WorldIntro[] worldIntros; public string reportSendVideo; public CornerMark reportSendMark; }
// Uretici filigrani filmin sag alt kosesinde duruyor. "Gec" dugmesini tam
// oraya koyup ustunu ortuyoruz; koordinatlar filmin kendi karesine oranlidir
// (0..1), boylece her video kendi filigran yerini soyleyebilir.
[Serializable] public class CornerMark { public float x=.9055f; public float y=.8333f; public float w=.0609f; public float h=.1056f; }
[Serializable] public class WorldIntro { public string id; public string firstCaseId; public string countryKey; public string locationKey; public string flagResource; public string videoPath; public string backdropResource; public bool graphicsEmbedded; public bool skipCoversCornerMark; public CornerMark skipMark; public bool deskArrival; public string deskArrivalVideo; public CornerMark deskArrivalMark; }
[Serializable] public class CaseData { public string id; public string titleKey; public bool draft; public Node[] nodes; public TimelineClue[] timelineClues; public Verdict[] verdicts; public Choice[] methods; public Choice[] evidence; public string[] conclusionRequires; public Choice[] custody; public string custodyLabelKey; public string suspectLabelKey; public string methodLabelKey; public int successfulReportTrustGain; public int failedReportTrustLoss; public string nextCaseId; public string ambienceId; public string closedStampKey; public string closedNoteKey; public string coldCaseTitleKey; public string coldCaseStatusKey; public int lineSlots; public int lineReopenPenaltySeconds; public string envelopeTitleKey; public string envelopeStampKey; public string envelopeBodyKey; public bool grantsAuthority; public ReconCard[] reconstruction; public ChapterFinale chapterFinale;
 // Masanın hâli: vakanın masada geçtiği saat (0-23, -1 belirsiz) ve hava ("rain" ya da boş). Yalnız atmosfer; oynanışa girmez.
 public int deskHour = -1; public string deskDate; public string weather; public CaseSummary summary;
 // Mekânın durağan kareleri (Resources yolları): vaka ilk açıldığında kart öncesi oynar. Boşsa atlanır.
 public string[] locationFrames;
 // Bölüm başı kartının yeri (anahtar) ve kapanış sonrası epilog görseli (Resources yolu) ile satırı. Boşsa atlanır.
 public string openingPlaceKey; public string epilogueImage; public string epilogueKey; }
[Serializable] public class TimelineClue { public string id; public string timeKey; public string noteKey; public string sourceKey; public int sortMinute; public string[] requiresRead; public string[] requiresAsked; }
[Serializable] public class CaseSummary { public string locationKey; public string truthKey; public string evidenceKey; public string lessonKey; }
[Serializable] public class CareerRules { public int initialTrust = 60; public int strongGain = 5; public int incompleteLoss = 5; public int falseAccusationLoss = 15; public int unsolvedLoss = 2; public int endThreshold = 0; public int[] statusThresholds = {80,60,40,20,1}; }
[Serializable] public class PendingReview { public string caseId; public bool correct; public string evaluationType; public int trustDelta; public int successGain; public int failureLoss; public long readyAtUtcTicks; public string suspectId; public string methodId; public string proofId; public string suspectSourceId; public string methodSourceId; public string proofSourceId; public bool suspectSupported; public bool methodSupported; public bool proofSupported; public string custodyId; public string custodySourceId; public bool custodySupported; public bool hasRecon; public bool reconSupported; }
[Serializable] public class FaxReview { public string caseId; public bool correct; public bool reopened; public bool trustRefunded; public string evaluationType; public long evaluatedAtUtcTicks; public int trustChange; public int trustAfter; public string suspectId; public string methodId; public string proofId; public string suspectSourceId; public string methodSourceId; public string proofSourceId; public bool suspectSupported; public bool methodSupported; public bool proofSupported; public string custodyId; public string custodySourceId; public bool custodySupported; public bool hasRecon; public bool reconSupported; }
[Serializable] public class CareerProgress { public int version = 1; public int departmentTrust = 60; public int retirementThreshold = 0; public string activeCaseId; public List<string> seenWorldIntros = new List<string>(); public List<PendingReview> pendingReviews = new List<PendingReview>(); public bool faxReleased; public FaxReview lastFax; public List<FaxReview> reviewHistory = new List<FaxReview>(); public string careerRankId = "investigator"; public bool retired; }
// Kişinin PNG portresi yoksa piksel portre çizilir. Tonlar eskiden kodda
// `personId=="hasan"` diye seçiliyordu, yani yeni vakanın yeni kişisi C#
// düzenlemesi istiyordu. Artık vaka verisinden gelir; alan boşsa varsayılan
// kullanılır ve hiçbir vaka bunu yazmak zorunda değildir.
[Serializable] public class PortraitStyle {
 public string hairHex; public string skinHex; public string shirtHex; public bool longHair; public bool moustache;
 public static readonly PortraitStyle Default = new PortraitStyle { hairHex="#2E211E", skinHex="#AE785A", shirtHex="#1F2426" };
}
[Serializable] public class FileMeta { public string labelKey; public string valueKey; }
// Raporun metninde adı geçen eşyalar; dosya ekranında fotoğraflı küçük kartlar olarak görünür.
[Serializable] public class RelatedItem { public string nameKey; public string detailKey; public string imageResource; }
[Serializable] public class AnswerVariant { public string answerKey; public string[] requiresAsked; public string[] requiresRead; public string[] excludesAsked; }
[Serializable] public class PresentedAnswer { public string sourceId; public string answerKey; }
[Serializable] public class Question { public string id; public string topicKey; public string[] aboutPersonIds; public string promptKey; public string answerKey; public string[] requiresAsked; public string[] requiresAnyAsked; public string[] excludesAsked; public string[] requiresRead; public string presentedSourceId; public string[] presentedSourceIds; public PresentedAnswer[] presentedAnswers; public PresentedAnswer[] decoyAnswers; public AnswerVariant[] answerVariants; }
[Serializable] public class Node { public FileMeta[] fileMeta; public RelatedItem[] relatedItems; public string imageResource; public string imageCaptionKey; public string id; public string kind; public string titleKey; public string bodyKey; public string[] requires; public string[] requiresAny; public string[] requiresAsked; public string[] requiresAnyAsked; public bool requestable; public string requestLabelKey; public int requestDelaySeconds; public string personId; public PortraitStyle portrait; public string personNameKey; public string personInfoKey; public string personQuoteKey; public Question[] questions; public string[] completionQuestionIds; public string cctvSourceKey; public string cctvOverlayKey; public string cctvPeriodKey; public CctvEvent[] cctvEvents; public string deflectAnswerKey; public bool notPresentable; public bool notReportSource; public string[] aboutPersonIds; public WarrantPath[] warrant; public int warrantSlots; public string requestKind; public string reopenYear; public string reopenNoteKey; public string line;
 // Görüşmede kişi sigara içiyor mu: yalnız görüntü (ince duman). Her soruda aynı kalır.
 public bool smokes;
 // Baskı: listedeki kaynakların hepsi dosyaya girdiğinde bu görüşme henüz istenmemişse artık istenemez
 // (kişi şehirden ayrıldı, avukatı görüşmeyi kesti). Doğrulayıcı doğru sonucun dayandığı hiçbir
 // kaynağın bu yolla kapanmasına izin vermez; kapanan yalnız ek bir okuma yoludur.
 public string[] closesAfterRead; public string closedNoteKey;
 // Olay yeri planı (Dosya #015'ten itibaren): kuşbakışı çizim, engeller ve tanıkların durduğu yerler.
 public ScenePlan scenePlan;
 // JsonUtility boş bir plan nesnesi kurar; işaretsiz plan yok sayılır.
 public bool HasScenePlan => scenePlan!=null && scenePlan.markers!=null && scenePlan.markers.Length>0; }
[Serializable] public class CctvEvent { public string id; public string textKey; public string[] aboutPersonIds; public bool notPresentable; public string overlayTimeKey; public string glitchKey; public string signalKey; public string videoPath; public string[] framePaths; public string[] frameTimes; public int frameMs; public int delayMs;
 // Görüntü ya kare dizisidir (`framePaths`, Resources yolları; `frameTimes` her karenin damgası) ya da videodur.
 public bool HasFootage => (framePaths != null && framePaths.Length > 0) || !string.IsNullOrEmpty(videoPath); }
// Olay yeri planı. Koordinatlar plan görselinin içinde 0–1 arasıdır (sol üst 0,0). Bir işaret,
// kilidi açıldığında (`requiresAsked` / `requiresRead`) planda belirir; oyuncu dokununca o noktadan
// görüş konisi çizilir ve engeller gölge bırakır. Oyun hiçbir işaretin doğru ya da yalan olduğunu söylemez.
[Serializable] public class PlanPoint { public float x; public float y; }
[Serializable] public class PlanOccluder { public string id; public PlanPoint[] points; }
[Serializable] public class PlanMarker { public string id; public string personId; public string labelKey; public float x; public float y; public float facing; public float fov; public string kind; public string[] requiresAsked; public string[] requiresRead; }
[Serializable] public class ScenePlan { public string imagePath; public PlanPoint incident; public string incidentLabelKey; public PlanOccluder[] occluders; public PlanMarker[] markers; }
[Serializable] public class Choice { public string id; public string labelKey; public bool correct; public string[] supportingSourceIds; public string epilogueKey; }
[Serializable] public class Verdict { public string id; public string labelKey; public string feedbackKey; public string epilogueKey; public bool correct; public string[] requires; public string[] supportingSourceIds; }
[Serializable] public class InterviewRequest { public string nodeId; public long readyAtUtcTicks; }
[Serializable] public class DocumentRequest { public string nodeId; public long readyAtUtcTicks; }
// İnceleme izni (Dosya #007'den itibaren): kabul edilen bir dayanak yolu; içindeki her kaynak seçilmiş olmalı.
// Kaynak düğüm kimliğidir ("parking"), kamera kaydı için kayıt noktasıyla ("street#car_stop").
[Serializable] public class WarrantPath { public string[] sources; }
[Serializable] public class WarrantDenial { public string nodeId; public long readyAtUtcTicks; public string[] basis; public bool seen; }
// Oyuncunun defteri: iki kaynağı kendisi yan yana koyup kendi hükmünü yazar. Oyun hükmün doğru
// olup olmadığını hiçbir zaman söylemez; not yalnız oyuncunun aklı içindir.
[Serializable] public class NotebookEntry { public string leftId; public string rightId; public string mark; }
[Serializable] public class InterviewTurn { public string nodeId; public string questionId; public string promptKey; public string answerKey; public string sourceId; public long askedAtUtcTicks; }
[Serializable] public class Progress { public int version = 1; public string caseId; public bool caseAccepted; public List<string> read = new List<string>(); public List<string> asked = new List<string>(); public List<InterviewRequest> interviewRequests = new List<InterviewRequest>(); public List<DocumentRequest> documentRequests = new List<DocumentRequest>(); public List<WarrantDenial> warrantDenials = new List<WarrantDenial>(); public List<string> closedLines = new List<string>(); public List<DocumentRequest> lineReopens = new List<DocumentRequest>(); public List<ReconPlacement> recon = new List<ReconPlacement>(); public List<InterviewTurn> interviewTurns = new List<InterviewTurn>(); public List<string> triedSources = new List<string>(); public List<string> decoyHolds = new List<string>(); public List<string> seenRequests = new List<string>(); public List<string> timelinePinned = new List<string>(); public List<NotebookEntry> notebook = new List<NotebookEntry>(); public List<string> highlights = new List<string>(); public int seenInterviewTurns; public bool closed; public string reportSuspect; public string reportMethod; public string reportProof; public string reportSuspectSource; public string reportMethodSource; public string reportProofSource; public string reportCustody; public string reportCustodySource; public long submittedAtUtcTicks; }
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

public sealed partial class Investigation {
 public CaseData Data { get; }
 // Ad geçme kuralı metne bakar; metin olmadan hiçbir kaynak kişiyle eşleşmez.
 public Locale Text { get; set; }
 // Kuralın okuduğu metin: oyuncu hangi dilde oynarsa oynasın ad eşleşmesi
 // kanon (Türkçe) metinden yapılır; çeviri kayıt sunma hakkını değiştirmesin.
 public Locale RuleText { get => ruleText ?? Text; set => ruleText = value; }
 Locale ruleText;
 public string RuleString(params string[] keys) =>
  RuleText==null ? "" : string.Join(" ", keys.Where(k=>!string.IsNullOrEmpty(k)).Select(k=>RuleText.Get(k)));
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
  State.notebook=(State.notebook ?? new List<NotebookEntry>()).Where(e=>e!=null && NotebookMarks.Contains(e.mark) && e.leftId!=e.rightId && data.nodes.Any(n=>n.id==e.leftId) && data.nodes.Any(n=>n.id==e.rightId)).ToList();
  State.highlights=(State.highlights ?? new List<string>()).Where(h=>h!=null && data.nodes.Any(n=>h.StartsWith(n.id+":",StringComparison.Ordinal))).Distinct().ToList();
  State.interviewTurns=(State.interviewTurns ?? new List<InterviewTurn>()).Where(turn=>turn!=null && data.nodes.Any(n=>n.id==turn.nodeId && (n.questions ?? new Question[0]).Any(q=>q.id==turn.questionId && q.promptKey==turn.promptKey)) && !string.IsNullOrEmpty(turn.answerKey)).ToList();
  State.seenInterviewTurns=Math.Max(0,Math.Min(State.seenInterviewTurns,State.interviewTurns.Count));
  State.interviewRequests = (State.interviewRequests ?? new List<InterviewRequest>()).Where(r => r != null && data.nodes.Any(n => n.id == r.nodeId && n.kind == "interview")).GroupBy(r => r.nodeId).Select(g => g.First()).ToList();
  State.closedLines=(State.closedLines ?? new List<string>()).Where(id=>data.nodes.Any(n=>n.id==id && IsLine(n))).Distinct().ToList();
  State.lineReopens=(State.lineReopens ?? new List<DocumentRequest>()).Where(r=>r!=null && data.nodes.Any(n=>n.id==r.nodeId && IsLine(n))).GroupBy(r=>r.nodeId).Select(g=>g.Last()).ToList();
  State.recon=(State.recon ?? new List<ReconPlacement>()).Where(p=>p!=null && (data.reconstruction ?? new ReconCard[0]).Any(c=>c.id==p.cardId)).GroupBy(p=>p.cardId).Select(g=>g.First()).ToList();
  State.warrantDenials=(State.warrantDenials ?? new List<WarrantDenial>()).Where(d=>d!=null && data.nodes.Any(n=>n.id==d.nodeId && IsWarrant(n))).GroupBy(d=>d.nodeId).Select(g=>g.First()).ToList();
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
 public static readonly string[] NotebookMarks = { "conflict", "agree", "question" };
 // Deftere yalnız oyuncunun gerçekten açtığı kaynaklar girer: okunan belge/kayıt ya da konuşulan kişi.
 public bool NotebookSource(string id) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  return n!=null && (State.read.Contains(id) || n.kind=="interview" && State.interviewTurns.Any(t=>t.nodeId==id));
 }
 public NotebookEntry FindNote(string a,string b) => State.notebook.FirstOrDefault(e=>e.leftId==a && e.rightId==b || e.leftId==b && e.rightId==a);
 // Aynı çift için ikinci not yazılmaz, eski hüküm değişir; aynı hüküm tekrar seçilirse not silinir.
 public bool MarkNote(string left,string right,string mark) {
  if(State.closed || left==right || !NotebookMarks.Contains(mark) || !NotebookSource(left) || !NotebookSource(right))return false;
  var existing=FindNote(left,right);
  if(existing!=null && existing.mark==mark){State.notebook.Remove(existing);return true;}
  if(existing!=null){existing.mark=mark;return true;}
  State.notebook.Add(new NotebookEntry { leftId=left, rightId=right, mark=mark });return true;
 }
 public bool RemoveNote(NotebookEntry entry) => !State.closed && entry!=null && State.notebook.Remove(entry);
 public static string HighlightId(string nodeId,int sentence) => nodeId+":"+sentence;
 // Altı çizilen satır: yalnız okunmuş belgede. Oyun hangi satırın önemli olduğunu işaretlemez.
 public bool ToggleHighlight(string nodeId,int sentence) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==nodeId);
  if(State.closed || n==null || n.kind=="interview" || sentence<0 || !State.read.Contains(nodeId))return false;
  var id=HighlightId(nodeId,sentence);
  if(!State.highlights.Remove(id))State.highlights.Add(id);
  return true;
 }
 // Belge metnini cümlelere böler; altı çizme ve defter aynı bölmeyi kullanır.
 public static string[] Sentences(string text) {
  if(string.IsNullOrEmpty(text))return new string[0];
  var parts=new List<string>();
  foreach(var paragraph in text.Split('\n')) {
   int start=0;
   for(int i=0;i<paragraph.Length;i++) {
    char c=paragraph[i];
    if((c=='.' || c=='!' || c=='?' || c=='…') && (i+1==paragraph.Length || paragraph[i+1]==' ')) {
     var piece=paragraph.Substring(start,i+1-start).Trim();if(piece.Length>0)parts.Add(piece);start=i+1;
    }
   }
   var rest=paragraph.Substring(start).Trim();if(rest.Length>0)parts.Add(rest);
  }
  return parts.ToArray();
 }
 // Baskı: kişi artık görüşmeye gelmiyor. Daha önce istenmiş ya da yapılmış görüşme kapanmaz.
 public bool Closed(Node n) => n.kind=="interview" && n.closesAfterRead!=null && n.closesAfterRead.Length>0
  && n.closesAfterRead.All(State.read.Contains) && !Requested(n) && !State.read.Contains(n.id) && !State.interviewTurns.Any(t=>t.nodeId==n.id);
 public bool Requested(Node n) => State.interviewRequests.Any(r => r.nodeId == n.id);
 public bool Pending(Node n) => n.kind == "interview" && Requested(n) && !State.read.Contains(n.id) && !Available(n);
 public bool CanRequest(Node n) => n.kind == "interview" && Discovered(n) && !Requested(n) && !State.read.Contains(n.id) && !Closed(n);
 public bool RequestInterview(string id, double waitSeconds = 4) {
  var n = Data.nodes.FirstOrDefault(x => x.id == id);
  if (n == null || !CanRequest(n)) return false;
  State.interviewRequests.Add(new InterviewRequest { nodeId = id, readyAtUtcTicks = DateTime.UtcNow.AddSeconds(waitSeconds).Ticks });
  return true;
 }
 public bool CanRequestDocument(Node n) => n.kind=="document" && n.requestable && !IsWarrant(n) && Discovered(n) && !State.read.Contains(n.id) && !State.documentRequests.Any(r=>r.nodeId==n.id)
  && (string.IsNullOrEmpty(n.line) || LineActive(n.line));
 public bool RequestDocument(string id,double? waitSeconds=null) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  if(n==null || !CanRequestDocument(n))return false;
  State.documentRequests.Add(new DocumentRequest { nodeId=id,readyAtUtcTicks=DateTime.UtcNow.AddSeconds(Math.Max(0,waitSeconds ?? n.requestDelaySeconds)).Ticks });
  return true;
 }
 // İnceleme izni: oyuncu dayanak kaynaklarını kendisi seçer. Yeterliyse belge talebi
 // gibi gecikmeyle gelir; değilse aynı gecikmeyle "ek dayanak gerekiyor" döner.
 // Ret hangi kaynağın eksik olduğunu söylemez ve ceza değildir; yeniden gönderilebilir.
 public static bool IsWarrant(Node n) => n!=null && n.warrant!=null && n.warrant.Length>0;
 public int WarrantSlots(Node n) => n.warrantSlots>0?n.warrantSlots:2;
 List<WarrantDenial> Denials => State.warrantDenials ??= new List<WarrantDenial>();
 public WarrantDenial Denial(Node n) => Denials.FirstOrDefault(d=>d.nodeId==n.id);
 public bool WarrantDenied(Node n) => Denial(n)?.readyAtUtcTicks<=DateTime.UtcNow.Ticks;
 public bool WarrantDenialPending(Node n) { var d=Denial(n); return d!=null && d.readyAtUtcTicks>DateTime.UtcNow.Ticks; }
 public bool CanRequestWarrant(Node n) => IsWarrant(n) && n.kind=="document" && n.requestable && Discovered(n) && !State.read.Contains(n.id)
  && !State.documentRequests.Any(r=>r.nodeId==n.id) && !WarrantDenialPending(n) && (!IsLine(n) || LineSlotFree);
 public bool WarrantBasisAvailable(string id) => ReportSourceAvailable(id) || !id.Contains("#") && State.read.Contains(id) && Data.nodes.Any(x=>x.id==id && x.kind=="cctv" && !x.notReportSource);
 public bool WarrantSatisfied(Node n,IEnumerable<string> basis) {
  var chosen=basis.ToArray();
  bool Has(string need)=>chosen.Any(b=>b==need || !need.Contains("#") && b.StartsWith(need+"#",StringComparison.Ordinal));
  return n.warrant.Any(p=>p.sources!=null && p.sources.Length>0 && p.sources.All(Has));
 }
 public bool SubmitWarrant(string id,string[] basis,double? waitSeconds=null) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  if(n==null || !CanRequestWarrant(n) || basis==null)return false;
  var chosen=basis.Where(b=>!string.IsNullOrEmpty(b)).Distinct().ToArray();
  if(chosen.Length!=WarrantSlots(n) || !chosen.All(WarrantBasisAvailable))return false;
  Denials.RemoveAll(d=>d.nodeId==id);
  double wait=Math.Max(0,waitSeconds ?? n.requestDelaySeconds+(IsLine(n)?ClosedLines.Count*Math.Max(0,Data.lineReopenPenaltySeconds):0));
  long ready=DateTime.UtcNow.AddSeconds(wait).Ticks;
  if(WarrantSatisfied(n,chosen))State.documentRequests.Add(new DocumentRequest { nodeId=id,readyAtUtcTicks=ready });
  else Denials.Add(new WarrantDenial { nodeId=id,readyAtUtcTicks=ready,basis=chosen });
  return true;
 }
 public bool UnseenWarrantDenial(Node n) { var d=Denial(n); return d!=null && !d.seen && d.readyAtUtcTicks<=DateTime.UtcNow.Ticks; }
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
 // Bu soruda bir kez öne sürülen kayıt (tutsa da tutmasa da) o soruda bir daha listelenmez.
 // Kişi bazında değil soru bazında: aynı kayıt aynı kişinin başka sorusunda gerekebilir.
 public bool SourceTried(Node n,Question q,string sourceId) =>
  SourceAlreadyPresented(n,q,sourceId) || (State.triedSources ?? new List<string>()).Contains(n.id+"/"+q.id+"/"+sourceId);
 public void MarkSourceTried(Node n,Question q,string sourceId) {
  State.triedSources ??= new List<string>();
  var key=n.id+"/"+q.id+"/"+sourceId;if(!State.triedSources.Contains(key))State.triedSources.Add(key);
 }
 // Yanitlanan soru listeden cikar. Bir soruyu birden cok kaynak kapatabilir
 // (`presentedSourceIds`), ama kisi cevabini bir kez verdikten sonra ayni seyi
 // ikinci bir kayitla tekrar sormak oyuncuya "bir sey eksik kaldi" izlenimi
 // veriyordu; oysa mesele kapanmisti.
 public bool MarkerAvailable(PlanMarker m) => m!=null && Meets(m.requiresRead) && (m.requiresAsked==null || m.requiresAsked.All(State.asked.Contains));
 public bool CanAskQuestion(Node n,Question q) => QuestionAvailable(n,q) && !State.asked.Contains(q.id) && !DecoyHeld(n,q);
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
  if(RuleText==null || string.IsNullOrEmpty(sourceText))return false;
  var haystack=Fold(sourceText);
  foreach(var name in PersonNames(personId))if(NameOccurs(haystack,Fold(name)))return true;
  return false;
 }
 IEnumerable<string> PersonNames(string personId) {
  foreach(var n in Data.nodes) {
   if(n.personId!=personId || string.IsNullOrEmpty(n.personNameKey))continue;
   var full=RuleText.Get(n.personNameKey);
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
  State.interviewTurns.Add(new InterviewTurn { nodeId=n.id, questionId=q.id, promptKey=q.promptKey, answerKey=answerKey, sourceId=sourceId, askedAtUtcTicks=DateTime.UtcNow.Ticks });
  if(InterviewComplete(n) && !State.read.Contains(n.id))State.read.Add(n.id);
  return true;
 }
 public bool Read(string id) {
  var n = Data.nodes.FirstOrDefault(x => x.id == id);
  if (n == null || !Available(n) || n.kind == "interview" && !InterviewComplete(n)) return false;
  if (!State.read.Contains(id)) State.read.Add(id);
  return true;
 }
 // Raporda yalnız dinlenen kişi önerilebilir: seçeneğin kimliği (ya da "_" önündeki kısmı) bir
 // görüşmeye denk geliyorsa o görüşme okunmuş olmalı. Kişi olmayan seçenekler ("kaza", yöntemler) hep açıktır.
 public bool ChoiceKnown(Choice c) => c!=null && ChoiceKnown(c.id);
 public bool ChoiceKnown(Verdict v) => v!=null && ChoiceKnown(v.id);
 public bool ChoiceKnown(string id) {
  if(string.IsNullOrEmpty(id))return false;
  var person=id.Split('_')[0];
  var node=Data.nodes.FirstOrDefault(n=>n.id==person && n.kind=="interview");
  return node==null || State.read.Contains(node.id);
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
  // Olay tespit tutanağı soruşturmanın **başlangıcıdır**, gerekçesi değil:
  // failin adını, yöntemi ya da parayı kimin aldığını hiçbir zaman göstermez.
  // Vaka onu `notReportSource` ile işaretler ve rapora kaynak olarak yazılamaz.
  if(node!=null && node.notReportSource)return false;
  if(node==null || !(State.read.Contains(nodeId) || node.kind=="interview" && State.interviewTurns.Any(t=>t.nodeId==nodeId)))return false;
  if(node.kind=="cctv")return separator>=0 && (node.cctvEvents ?? new CctvEvent[0]).Any(e=>e.id==id.Substring(separator+1));
  if(node.kind=="interview")return separator<0 || InterviewSourceTurn(id)!=null;
  return separator<0;
 }
 bool Supports(string[] sourceIds,string selectedId) => ReportSourceAvailable(selectedId) &&
  (sourceIds==null || sourceIds.Length==0 || sourceIds.Any(id=>id==selectedId ||
   !id.Contains("#") && selectedId.StartsWith(id+"#",StringComparison.Ordinal)));
 // Üç sütunlu rapor (fail / yöntem / kanıt) vakaların çoğunda yeter. Dosya #002
 // ise tek olayda **iki ayrı sorumluluk** taşıyor: yaralayan ile parayı alan
 // aynı kişi değil. Bu yüzden vaka verisi isterse dördüncü bir sütun açar
 // (`custody`). Sütunu olmayan vakada hiçbir şey değişmez — eski çağrı da,
 // eski kayıt da olduğu gibi çalışır.
 public bool HasCustody => (Data.custody ?? new Choice[0]).Length > 0;

 // 3 Ekim 2026: rapor yalnız iki iddiadır — kim ve ne ile/nasıl (gözaltı sütunu olan vakada üçüncüsü).
 // Oyuncu kaynak satırı seçmez; gerekçe, doğru seçeneği gösteren kaynağı soruşturmada
 // açmış olmasıdır. Açmadan doğru tahmin "eksik" sayılır, yanlış kişi asılsız suçlamadır.
 public bool SubmitReport(string suspect,string method,string custody) {
  string Found(Choice c)=>c==null||!c.correct?null:(c.supportingSourceIds??new string[0]).FirstOrDefault(id=>ReportSourceAvailable(id)||State.read.Contains(id));
  string FoundVerdict(Verdict v)=>v==null||!v.correct?null:(v.supportingSourceIds??new string[0]).FirstOrDefault(id=>ReportSourceAvailable(id)||State.read.Contains(id));
  if(!CanConclude || !ReconComplete || !Data.verdicts.Any(v=>v.id==suspect) || !Data.methods.Any(v=>v.id==method))return false;
  if(HasCustody && !Data.custody.Any(v=>v.id==custody))return false;
  if(!HasCustody)custody=null;
  var verdict=Data.verdicts.First(v=>v.id==suspect);var how=Data.methods.First(v=>v.id==method);
  var held=HasCustody?Data.custody.First(v=>v.id==custody):null;
  string suspectSource=FoundVerdict(verdict),methodSource=Found(how),custodySource=Found(held);
  State.reportSuspect=suspect;State.reportMethod=method;State.reportProof=null;
  State.reportSuspectSource=suspectSource;State.reportMethodSource=methodSource;State.reportProofSource=null;
  State.reportCustody=custody;State.reportCustodySource=custodySource;
  State.submittedAtUtcTicks=DateTime.UtcNow.Ticks;State.closed=true;
  Career.faxReleased=Career.pendingReviews.Any(r=>r.readyAtUtcTicks>0);
  bool personSupported=suspectSource!=null,methodSupported=methodSource!=null;
  bool custodyCorrect=!HasCustody||held.correct,custodySupported=!HasCustody||custodySource!=null;
  bool correct=personSupported&&methodSupported&&custodySupported&&ReconSupported;
  string evaluationType=!verdict.correct||!custodyCorrect?"falseAccusation":correct?"supported":"incomplete";
  int trustDelta=evaluationType=="supported"?Rules.strongGain:evaluationType=="incomplete"?-Rules.incompleteLoss:-Rules.falseAccusationLoss;
  Career.pendingReviews.Add(new PendingReview {
   caseId=Data.id,correct=correct,evaluationType=evaluationType,trustDelta=trustDelta,
   successGain=Math.Max(0,Data.successfulReportTrustGain),failureLoss=Math.Max(0,Data.failedReportTrustLoss),
   suspectId=suspect,methodId=method,suspectSourceId=suspectSource,methodSourceId=methodSource,
   suspectSupported=personSupported,methodSupported=methodSupported,proofSupported=true,
   custodyId=custody,custodySourceId=custodySource,custodySupported=HasCustody&&custodySupported,hasRecon=HasReconstruction,reconSupported=ReconSupported
  });
  return true;
 }
 public bool SubmitFinalReport(string suspect,string method,string proof,string suspectSource,string methodSource,string proofSource) =>
  SubmitFinalReport(suspect,method,proof,suspectSource,methodSource,proofSource,null,null);

 public bool SubmitFinalReport(string suspect,string method,string proof,string suspectSource,string methodSource,string proofSource,string custody,string custodySource) {
  if(!CanConclude || !ReconComplete || !Data.verdicts.Any(v=>v.id==suspect) || !Data.methods.Any(v=>v.id==method) || !Data.evidence.Any(v=>v.id==proof) || !State.read.Contains(proof) || !ReportSourceAvailable(suspectSource) || !ReportSourceAvailable(methodSource) || !ReportSourceAvailable(proofSource))return false;
  if(HasCustody && (!Data.custody.Any(v=>v.id==custody) || !ReportSourceAvailable(custodySource)))return false;
  if(!HasCustody){custody=null;custodySource=null;}
  State.reportSuspect=suspect;State.reportMethod=method;State.reportProof=proof;
  State.reportSuspectSource=suspectSource;State.reportMethodSource=methodSource;State.reportProofSource=proofSource;
  State.reportCustody=custody;State.reportCustodySource=custodySource;
  State.submittedAtUtcTicks=DateTime.UtcNow.Ticks;
  State.closed=true;
  Career.faxReleased=Career.pendingReviews.Any(r=>r.readyAtUtcTicks>0);
  bool suspectCorrect=Data.verdicts.Any(v=>v.id==suspect && v.correct);
  bool personSupported=Data.verdicts.Any(v=>v.id==suspect && v.correct && Supports(v.supportingSourceIds,suspectSource));
  bool methodSupported=Data.methods.Any(v=>v.id==method && v.correct && Supports(v.supportingSourceIds,methodSource));
  bool proofSupported=Data.evidence.Any(v=>v.id==proof && v.correct && Supports(v.supportingSourceIds,proofSource));
  // Dördüncü sütun da bir **kişiyi** adlandırır. Yanlış kişi yazmak, failde
  // olduğu gibi asılsız suçlamadır; doğru kişiyi kaynaksız yazmak eksiktir.
  bool custodyCorrect=!HasCustody || Data.custody.Any(v=>v.id==custody && v.correct);
  bool custodySupported=!HasCustody || Data.custody.Any(v=>v.id==custody && v.correct && Supports(v.supportingSourceIds,custodySource));
  bool correct=personSupported && methodSupported && proofSupported && custodySupported && ReconSupported;
  string evaluationType=!suspectCorrect || !custodyCorrect?"falseAccusation":!correct?"incomplete":"supported";
  int trustDelta=evaluationType=="supported"?Rules.strongGain:evaluationType=="incomplete"?-Rules.incompleteLoss:-Rules.falseAccusationLoss;
  Career.pendingReviews.Add(new PendingReview {
   caseId=Data.id,correct=correct,evaluationType=evaluationType,trustDelta=trustDelta,
   successGain=Math.Max(0,Data.successfulReportTrustGain),
   failureLoss=Math.Max(0,Data.failedReportTrustLoss),
   suspectId=suspect,methodId=method,proofId=proof,
   suspectSourceId=suspectSource,methodSourceId=methodSource,proofSourceId=proofSource,
   suspectSupported=personSupported,methodSupported=methodSupported,proofSupported=proofSupported,
   custodyId=custody,custodySourceId=custodySource,custodySupported=HasCustody && custodySupported,hasRecon=HasReconstruction,reconSupported=ReconSupported
  });
  return true;
 }
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
  Career.retired=Career.departmentTrust<=Career.retirementThreshold;
  State.closed=false;
  State.reportSuspect=State.reportMethod=State.reportProof=null;
  State.reportSuspectSource=State.reportMethodSource=State.reportProofSource=null;
  State.reportCustody=State.reportCustodySource=null;
  State.submittedAtUtcTicks=0;
  return true;
 }
 public string TrustStatusKey => StatusKeyFor(Career.departmentTrust,Rules);
 // Kademe yalnız güven değerinden türer; sicil geçmişi de aynı kuralla hesaplar.
 public static string StatusKeyFor(int value,CareerRules rules) {
  rules=rules??new CareerRules();var t=rules.statusThresholds;if(t==null || t.Length!=5)t=new[]{80,60,40,20,1};
  if(value<=rules.endThreshold)return "career.status.ended";
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
  int before=Career.departmentTrust;
  int delta=string.IsNullOrEmpty(pending.evaluationType) ? (pending.correct?pending.successGain:-pending.failureLoss) : pending.trustDelta;
  Career.departmentTrust=Math.Max(0,Math.Min(100,before+delta));
  Career.retired=Career.departmentTrust<=Career.retirementThreshold;
  Career.lastFax=new FaxReview {
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
