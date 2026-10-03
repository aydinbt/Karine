using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UIElements;

namespace Bube {
// Sonuç gönderme, rapor kaynakları, vaka özeti ve faks.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 void Conclusion() { ConclusionStep(0); }
 // Rapor sütunları vaka verisinden gelir. Üç sütun (fail / yöntem / kanıt)
 // her vakada vardır; bir vaka `custody` tanımlarsa dördüncü sütun — olayda
 // ikinci bir sorumluluk — araya girer. Sihirbaz adım sayısını buradan sayar,
 // bu yüzden hiçbir yerde sabit "04" yoktur.
 sealed class ReportColumn {
  public string headingKey, sourceHeadingKey;
  // Sütunun seçenekleri iki ayrı tipten gelebilir (`Verdict`, `Choice`); bu
  // ekranın ihtiyacı olan yalnız kimlik ve etikettir.
  public List<KeyValuePair<string,string>> choices;
  public Func<string> pick, source;
  public Action<string> setPick, setSource;
 }
 static List<KeyValuePair<string,string>> Options(IEnumerable<KeyValuePair<string,string>> items) => items.ToList();
 List<ReportColumn> ReportColumns() {
  var columns=new List<ReportColumn> {
   new ReportColumn{headingKey=SuspectKey(game.Data),sourceHeadingKey="conclude.suspectSource",choices=Options(game.Data.verdicts.Where(v=>game.ChoiceKnown(v)).Select(v=>new KeyValuePair<string,string>(v.id,v.labelKey))),
    pick=()=>selectedSuspect,setPick=v=>selectedSuspect=v,source=()=>selectedSuspectSource,setSource=v=>selectedSuspectSource=v},
   new ReportColumn{headingKey=MethodKey(game.Data),sourceHeadingKey="conclude.methodSource",choices=Options(game.Data.methods.Where(v=>game.ChoiceKnown(v)).Select(v=>new KeyValuePair<string,string>(v.id,v.labelKey))),
    pick=()=>selectedMethod,setPick=v=>selectedMethod=v,source=()=>selectedMethodSource,setSource=v=>selectedMethodSource=v}
  };
  if(game.HasCustody)columns.Add(new ReportColumn{
   headingKey=string.IsNullOrEmpty(game.Data.custodyLabelKey)?"conclude.custody":game.Data.custodyLabelKey,
   sourceHeadingKey="conclude.custodySource",choices=Options(game.Data.custody.Where(v=>game.ChoiceKnown(v)).Select(v=>new KeyValuePair<string,string>(v.id,v.labelKey))),
   pick=()=>selectedCustody,setPick=v=>selectedCustody=v,source=()=>selectedCustodySource,setSource=v=>selectedCustodySource=v});
  return columns;
 }
 // 3 Ekim 2026 maketi: solda adım sütunu, sağda form kâğıdı. Adım mantığı aynı kaldı.
 void ConclusionStep(int step) {
  if(!game.CanConclude){FilePage();return;}
  var columns=ReportColumns();
  int last=columns.Count;
  step=Mathf.Clamp(step,0,last);
  showingInterviewList=false;
  Func<ReportColumn,bool> done=c=>c.pick()!=null;
  Desk();
  KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("back.file"),T(game.Data.titleKey),T("file.unit"),FilePage,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(()=>ConclusionStep(step)));
  Back(FilePage);
  // Bir adıma ancak öncekilerin hepsi tamamsa atlanır; sütun yalnız ilerlemeyi gösterir.
  int reachable=0;
  while(reachable<last && done(columns[reachable]))reachable++;
  var rail=KarineUI.ReportRail(root);
  for(int i=0;i<=last;i++) {
   int target=i;
   string label=i==last?T("conclude.step.send"):T(columns[i].headingKey);
   string icon=i==last?"document":i==0?"person":"fingerprint";
   KarineUI.ReportStep(rail,(i+1).ToString("00"),label,icon,i<last && done(columns[i]),i==step,i<=reachable,()=>ConclusionStep(target));
  }
  var paper=KarineUI.ReportPaper(root);
  KarineUI.ReportHead(paper,T("conclude"),CaseNumber(),(step+1).ToString("00")+" / "+(last+1).ToString("00"),
   T(step==last?"conclude.reviewHelp":"conclude.stepHelp"));
  var scroll=new KarineScrollView(ScrollViewMode.Vertical);scroll.style.flexGrow=1;paper.Add(scroll);
  bool ready;
  if(step<last) {
   var column=columns[step];
   KarineUI.ReportHeading(scroll,T(column.headingKey));
   bool suspects=column.headingKey==SuspectKey(game.Data);
   var wrap=new VisualElement();wrap.style.flexDirection=FlexDirection.Row;wrap.style.flexWrap=Wrap.Wrap;
   if(suspects)wrap.style.justifyContent=Justify.Center;
   wrap.style.marginBottom=KarineTheme.SpaceMd;scroll.Add(wrap);
   int index=0;
   foreach(var v in column.choices) {
    var id=v.Key;var label=T(v.Value);
    Action choose=()=>{ if(column.pick()!=id){column.setPick(id);column.setSource(null);ConclusionStep(step);} };
    if(suspects)KarineUI.ReportPortrait(wrap,Resources.Load<Texture2D>("Bube/Characters/"+id),label,column.pick()==id,index++,choose);
    else KarineUI.ReportChoiceCard(wrap,label,column.pick()==id,choose);
   }
   ready=done(column);
  } else {
   foreach(var column in columns) {
    var chosen=column.choices.FirstOrDefault(v=>v.Key==column.pick());
    string icon=column==columns[0]?"person":"fingerprint";
    var target=columns.IndexOf(column);
    KarineUI.ReportSummaryRow(scroll,icon,T(column.headingKey),T(chosen.Value==null?"conclude.unselected":chosen.Value),null,null,()=>ConclusionStep(target));
   }
   ready=columns.All(c=>done(c));
  }
  var nav=KarineUI.ReportNav(paper);
  if(step>0)KarineUI.ReportNavButton(nav,T("conclude.previous"),false,true,()=>ConclusionStep(step-1),"nav_prev");
  var gap=new VisualElement {pickingMode=PickingMode.Ignore};gap.style.flexGrow=1;nav.Add(gap);
  if(step==last)KarineUI.ReportNavButton(nav,T("conclude.submit"),true,ready,ConfirmSubmit,"document");
  else KarineUI.ReportNavButton(nav,T("conclude.next"),true,ready,()=>ConclusionStep(step+1),"nav_next",true);
 }
 // Gönderilen rapor geri alınamaz; onay kartı yalnız kararı sorar, ne seçileceğini söylemez.
 void ConfirmSubmit() {
  VisualElement card=null;
  card=KarineUI.ReportConfirm(root,T("conclude.confirm.title"),T("conclude.confirm.body"),
   T("conclude.confirm.cancel"),()=>card.RemoveFromHierarchy(),
   T("conclude.confirm.send"),Result);
 }
 // Vaka kendi sütun başlığını verebilir (Dosya #003: "Ölümden kim sorumlu?"); boşsa ortak başlık.
 static string SuspectKey(CaseData data)=>string.IsNullOrEmpty(data?.suspectLabelKey)?"conclude.suspect":data.suspectLabelKey;
 static string MethodKey(CaseData data)=>string.IsNullOrEmpty(data?.methodLabelKey)?"conclude.method":data.methodLabelKey;
 string ReportCustodyHeading(CaseData data) =>
  T(string.IsNullOrEmpty(data.custodyLabelKey)?"conclude.custody":data.custodyLabelKey);
 string ReportSourceLabel(string id) {
  if(string.IsNullOrEmpty(id))return T("conclude.chooseSource");
  int separator=id.IndexOf('#');
  var node=game.Data.nodes.FirstOrDefault(n=>n.id==(separator<0?id:id.Substring(0,separator)));
  if(node==null)return T("conclude.sourceUnknown");
  if(node.kind=="interview" && separator>=0) {
   var turn=game.InterviewSourceTurn(id);
   if(turn!=null)return T(node.personNameKey)+" · "+T(turn.promptKey);
  }
  if(node.kind=="cctv" && separator>=0) {
   var record=(node.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e=>e.id==id.Substring(separator+1));
   if(record!=null)return T(node.titleKey)+" · "+T(record.textKey);
  }
  return T(node.titleKey);
 }
 string CompactReportSourceLabel(string id) {
  var label=ReportSourceLabel(id);
  return label.Length>76?label.Substring(0,76)+"…":label;
 }
 string SourceTitle(string id) {
  if(string.IsNullOrEmpty(id))return T("conclude.chooseSource");
  return ReviewSourceTitle(game.Data,id);
 }
 string ReviewSourceTitle(CaseData data,string id) {
  if(string.IsNullOrEmpty(id))return T("conclude.sourceUnknown");
  int separator=id.IndexOf('#');
  var source=data.nodes.FirstOrDefault(n=>n.id==(separator<0?id:id.Substring(0,separator)));
  if(source==null)return T("conclude.sourceUnknown");
  if(separator<0)return T(source.titleKey);
  if(source.kind=="interview") {
   var turn=data.id==game.Data.id?game.InterviewSourceTurn(id):null;
   if(turn!=null)return T(source.personNameKey)+" · "+T(turn.answerKey);
   var questionId=id.Substring(separator+1).Split('|')[0];
   var question=(source.questions ?? new Question[0]).FirstOrDefault(q=>q.id==questionId);
   return question==null?T("conclude.sourceUnknown"):T(source.personNameKey)+" · "+T(question.promptKey);
  }
  var record=(source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e=>e.id==id.Substring(separator+1));
  return record==null?T("conclude.sourceUnknown"):T(source.titleKey)+" · "+T(record.textKey);
 }
 void SummaryContents(VisualElement body,CaseData source=null) {
  var dark=KarineTheme.Paper.Ink;
  var summary=(source ?? game.Data).summary;
  if(summary==null)return;
  Text(body,T("result.truth"),dark,19);
  Text(body,T(summary.truthKey),dark,16);
  Text(body,T("result.evidence"),dark,19);
  Text(body,T(summary.evidenceKey),dark,16);
  Text(body,T("result.lesson"),dark,19);
  Text(body,T(summary.lessonKey),dark,16);
 }
 string ReportedConclusion() {
  var suspect=game.Data.verdicts.FirstOrDefault(v=>v.id==game.State.reportSuspect);
  var method=game.Data.methods.FirstOrDefault(v=>v.id==game.State.reportMethod);
  var evidence=game.Data.evidence.FirstOrDefault(v=>v.id==game.State.reportProof);
  return suspect!=null && method!=null && evidence!=null
   ?T(suspect.labelKey)+" · "+T(method.labelKey)+" · "+T(evidence.labelKey):T("result.status");
 }
 void SummaryField(VisualElement parent,string label,string value) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginBottom=6;parent.Add(row);
  var dark=KarineTheme.Paper.Ink;
  var name=Text(row,label, KarineTheme.Paper.Faded,15);name.style.width=150;name.style.marginBottom=0;
  var detail=Text(row,":  "+value,dark,15);detail.style.flexGrow=1;detail.style.marginBottom=0;
 }
 // Vaka özeti: kariyer kaydıyla aynı tam ekran kâğıt. Sonucu söylemez; sıradaki
 // görev masadaki bildirimle gelir, bu yüzden burada yalnız masaya dönüş var.
 void CaseSummary() {
  if(!game.State.closed){Desk();return;}
  Back(Desk);root.Clear();KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("back.desk"),T("summary.title"),T("file.unit"),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(CaseSummary));
  var report=game.Data.nodes.FirstOrDefault(n=>n.id=="report");
  // Her vakada aynı düzen: olay raporunun resmi yoksa vakanın kapak resmi kullanılır.
  var photo=(report==null || string.IsNullOrEmpty(report.imageResource)?null:Resources.Load<Texture2D>(report.imageResource))
   ?? Resources.Load<Texture2D>("Bube/Art/Covers/"+game.Data.id);
  KarineUI.SummaryPaper(root,photo,out var card,out var footer);
  KarineUI.SummaryStamp(card,T("summary.stamp"),T(game.Data.titleKey)+" · "+T(game.Data.summary.locationKey));
  KarineUI.RecordHeading(card,T("career.sentReport"));
  var suspect=game.Data.verdicts.FirstOrDefault(v=>v.id==game.State.reportSuspect);
  var method=game.Data.methods.FirstOrDefault(v=>v.id==game.State.reportMethod);
  var custody=(game.Data.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==game.State.reportCustody);
  if(suspect!=null)KarineUI.RecordRow(card,T(SuspectKey(game.Data)),T(suspect.labelKey),null,"");
  if(method!=null)KarineUI.RecordRow(card,T(MethodKey(game.Data)),T(method.labelKey),null,"");
  if(custody!=null)KarineUI.RecordRow(card,ReportCustodyHeading(game.Data),T(custody.labelKey),null,"");
  KarineUI.RecordRow(card,T("summary.investigator"),T("summary.bora"),null,"");
  if(game.State.submittedAtUtcTicks>0)
   KarineUI.RecordRow(card,T("summary.sentAt"),new DateTime(game.State.submittedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm"),null,"");
  var sourceNames=game.Data.nodes.Where(n=>game.State.read.Contains(n.id)).Select(n=>T(n.titleKey)).Distinct().Take(4).ToArray();
  if(sourceNames.Length==0)sourceNames=new[]{T("summary.noSources")};
  bool reviewed=game.Career.reviewHistory.Any(r=>r.caseId==game.Data.id);
  KarineUI.SummarySources(card,T("summary.sources").TrimStart('⌕',' '),sourceNames,T(reviewed?"summary.faxAvailable":"summary.faxLater"));
  var actions=new VisualElement();actions.style.flexDirection=FlexDirection.Row;actions.style.marginTop=KarineTheme.SpaceLg;footer.Add(actions);
  var back=KarineUI.PaperButton(actions,T("back.desk"),Desk);back.style.flexGrow=1;back.style.minHeight=48;
  NextStep(actions,CaseSummary);
 }
 // Vaka sonrası akış kendi kendine ilerlemez: sıradaki iş bir düğmeyle açılır, oyuncu kaçırmaz.
 // Faks hazır değilse düğme beklediğini söyler ve hazır olunca kendini yeniler.
 void NextStep(VisualElement parent,Action refresh) {
  string step=StepState();
  Button next=null;
  if(step=="fax")next=KarineUI.PaperButton(parent,T("next.openFax")+"  →",()=>InboxPage("fax:new","all"),KarinePaperKind.Action);
  else if(step=="waiting"){next=KarineUI.PaperButton(parent,T("next.waitingFax"),()=>{});next.SetEnabled(false);}
  else if(step=="assignment")next=KarineUI.PaperButton(parent,T("next.openAssignment")+"  →",ContinueToNextCase,KarinePaperKind.Action);
  // Durum değişince (faks geldi, faks okundu) düğme yenilenir; açık bir pencerenin altından yenilenmez.
  parent.schedule.Execute(()=>{if(parent.panel!=null && root.Q("InboxModal")==null && StepState()!=step)refresh();}).Every(1000);
  if(next==null)return;
  // Yan yana dizide eşit genişlik; masadaki dikey panelde yalnız kendi boyu (yoksa paneli doldurup dev oluyordu).
  bool row=parent.style.flexDirection==FlexDirection.Row;
  next.style.flexGrow=row?1:0;next.style.height=48;next.style.marginLeft=row?KarineTheme.SpaceMd:0;next.style.marginTop=row?0:KarineTheme.SpaceMd;
 }
 string StepState() {
  if(HasIncomingFax)return "fax";
  if(game.Career.pendingReviews.Any(r=>r.caseId==game.Data.id))return "waiting";
  return AvailableAssignment()!=null?"assignment":"none";
 }
 void Result() {
  if(game.SubmitReport(selectedSuspect,selectedMethod,selectedCustody)) {
   game.BeginNextCaseReview(7);
   Save();
   // Mühür her raporda aynı biçimde iner; sonucu faks söyler.
   KarineUI.EnvelopeSeal(root,envelope=>KarineUI.StampDown(root,T("report.stamp"),()=>{envelope?.RemoveFromHierarchy();CaseSummary();}));
  }
 }
 void ContinueToNextCase() {
  if(ShowCaseClosed(ContinueToNextCase))return;
  if(game.Career.retired){Desk();return;}
  var nextData=AvailableAssignment();
  if(nextData!=null){OpenAssignment(nextData);return;}
  Desk();
  var pending=Panel(root);pending.style.position=Position.Absolute;
  pending.style.left=Length.Percent(34);pending.style.top=Length.Percent(23);
  Text(pending,T("next.pending"),Ink,17);
  Button(pending,T("back.desk"),Desk);
 }
 void OpenAssignment(CaseData nextData) {
  if(nextData==null || nextData.draft || !game.State.closed || game.Data.nextCaseId!=nextData.id)return;
  var nextId=nextData.id;
   // Yeni görev her zaman ilk kez başlıyormuş gibi açılır: diskte kalmış eski bir kayıt
   // (sıfırlanmış kariyerden kalan, hatta kapatılmış) yüklenmez; yoksa vaka oynanmadan atlanıyordu.
   // Önceki dosyanın değerlendirmesi faksla bu vakanın içinde gelir.
   Progress progress=null;
   ForgetCaseMoments(nextId);
   game=new Investigation(nextData,progress,game.Career,careerRules){Text=locale};
   game.Career.activeCaseId=nextId;
   game.BeginNextCaseReview(7);
   selectedSuspect=selectedMethod=selectedEvidence=selectedCustody=null;
   selectedSuspectSource=selectedMethodSource=selectedEvidenceSource=selectedCustodySource=null;
   Save();
   // Vaka arası: araya giren reklamın **tek** yeri burasıdır. Ağ yokken hiçbir
   // şey olmaz ve akış beklemez; reklam gösterilse de sonra aynı yere devam eder.
   AdGateway.Request(AdPlacement.CaseInterval,AdMoment.CaseClosed,_=>{
    if(game.State.caseAccepted)Desk();else MaybeWorldIntro(Desk);
   });
 }
 void FaxPage() {
  if(!HasIncomingFax){Desk();return;}
  var fax=game.DeliverNextFax();
  if(fax==null){Desk();return;}
  Save();
  // Gerçek işleyiş: rapor gönderilince dosya değerlendirmeye gider; ancak onaylanırsa kapanır.
  // Onay faksı ilk açıldığında o dosyanın "KAPANDI" kartı oynar, faks arkasından basılır.
  if(fax.correct && !fax.reopened){ClosedCard(fax.caseId,()=>FaxSheet(fax));return;}
  FaxSheet(fax);
 }
 // Faks tablosundaki açıklama: masadaki faksla aynı gerekçe metni.
 string FaxReason(string sourceId,bool supported,string claim) {
  var src=(sourceId??"").Split('#')[0];
  return T(supported?"fax.reason.supported":src=="report"?"fax.reason."+claim+".report":"fax.reason."+claim+".other");
 }
 void FaxSheet(FaxReview fax) {
  Back(Desk);root.Clear();KarineUI.InboxScene(root);
  var reviewedAsset=Resources.Load<TextAsset>("Bube/Cases/"+fax.caseId);
  var reviewed=reviewedAsset==null?null:JsonUtility.FromJson<CaseData>(reviewedAsset.text);
  KarineUI.DossierBar(root,T("back.desk"),T("fax.screen"),reviewed!=null?T(reviewed.titleKey):T("file.unit"),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(()=>FaxSheet(fax)));
  var when=fax.evaluatedAtUtcTicks>0?new DateTime(fax.evaluatedAtUtcTicks,DateTimeKind.Utc).ToLocalTime():DateTime.Now;
  var meta=T("fax.from")+"  ·  "+when.ToString("dd.MM.yyyy HH:mm",CultureInfo.InvariantCulture)+"  ·  S.1/1";
  var body=KarineUI.FaxPaper(root,meta,EvaluationTitle(fax),out var heading);
  // Faks **basılıyor**: daktilo sesinin tek yeri burası.
  Typewriter(heading,EvaluationTitle(fax));
  if(reviewed!=null) {
   var table=KarineUI.FaxTable(body,T("fax.col.topic"),T("fax.col.detail"),T("fax.col.verdict"));
   var person=reviewed.verdicts.FirstOrDefault(v=>v.id==fax.suspectId);
   var method=reviewed.methods.FirstOrDefault(v=>v.id==fax.methodId);
   var proof=reviewed.evidence.FirstOrDefault(v=>v.id==fax.proofId);
   var custody=(reviewed.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==fax.custodyId);
   if(person!=null)KarineUI.FaxRow(table,T(SuspectKey(reviewed)),T(person.labelKey)+"\n"+FaxReason(fax.suspectSourceId,fax.suspectSupported,"suspect"),Verdict(fax.suspectSupported));
   if(method!=null)KarineUI.FaxRow(table,T(MethodKey(reviewed)),T(method.labelKey)+"\n"+FaxReason(fax.methodSourceId,fax.methodSupported,"method"),Verdict(fax.methodSupported));
   if(custody!=null)KarineUI.FaxRow(table,ReportCustodyHeading(reviewed),T(custody.labelKey)+"\n"+FaxReason(fax.custodySourceId,fax.custodySupported,"custody"),Verdict(fax.custodySupported));
   if(proof!=null)KarineUI.FaxRow(table,T("conclude.evidence"),T(proof.labelKey)+"\n"+FaxReason(fax.proofSourceId,fax.proofSupported,"evidence"),Verdict(fax.proofSupported));
   // Yalnız elimizdeki metin yazılır: dosyanın akıbeti (vaka verisi) ve kapanış notu.
   foreach(var key in new[]{person?.epilogueKey,custody?.epilogueKey}.Where(k=>!string.IsNullOrEmpty(k) && locale.Has(k)))KarineUI.FaxNote(body,T(key));
  }
  KarineUI.FaxNote(body,T("fax.closing"));
  KarineUI.FaxSeal(body,T("fax.seal"));
  var side=KarineUI.FaxSide(root,T("career.trust"),T(TrustStatusKey(fax.trustAfter)),fax.trustChange,null);
  KarineUI.FaxLink(side,"folder",T("career.openRecord"),StatisticsPage);
  if(RetryOffered(fax))KarineUI.FaxLink(side,"refresh",T("retry.watch"),OfferRetry);
  if(game.Career.retired)Text(side,T("career.ended"),KarineTheme.Danger,18);
  else if(game.State.closed)KarineUI.InterviewAction(side,"document",T("next.openAssignment"),ContinueToNextCase);
 }

}
}
