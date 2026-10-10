using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Arşiv (UI_ARCHIVE / UI_ARCHIVE_CASE maketleri): kariyer ekranından açılır, her vakada aynı
// düzen. Salt okunur: oyuncunun gönderdiği rapor, gelen değerlendirme ve kendi açtığı kayıtlar.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 void ArchiveBar(string subtitle,Action redraw) {
  // Arşiv kariyer sayfasından açılır; geri oraya döner, masaya değil.
  Back(StatisticsPage);root.Clear();KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("menu.row.career"),T("archive.screen"),subtitle,StatisticsPage,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(redraw));
 }
 void ArchivePage() {
  var cases=ClosedCases();
  ArchiveBar(T("archive.count")+" · "+cases.Length,ArchivePage);
  var drawer=KarineUI.ArchiveDrawer(root,T("archive.plate"));
  if(cases.Length==0){KarineUI.ArchiveEmpty(drawer,T("archive.empty"));return;}
  foreach(var item in cases) {
   var picked=item;
   var date=T("archive.reportAt")+": "+(item.progress.submittedAtUtcTicks>0
    ?new DateTime(item.progress.submittedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm")
    :T("summary.unknownDate"));
   // Yeniden deneme aynı vaka için ikinci değerlendirme yazabilir; damga en sonuncusudur.
   var fax=game.Career.reviewHistory.LastOrDefault(r=>r.caseId==item.data.id);
   bool reopened=game.Career.reviewHistory.Any(r=>r.caseId==item.data.id && r.reopened);
   KarineUI.ArchiveCard(drawer,T(item.data.titleKey),date,
    fax==null?null:T(fax.correct?"archive.stamp.approved":"fax.unsupported"),fax!=null && fax.correct,
    reopened?T("retry.recordShort"):null,()=>ArchiveCasePage(picked.data.id,null));
   // Ayrılan dosyanın doğurduğu soğuk dosya: salt kart, henüz oynanmaz.
   if(fax!=null && fax.correct && !string.IsNullOrEmpty(item.data.coldCaseTitleKey))
    KarineUI.ArchiveCard(drawer,T(item.data.coldCaseTitleKey),T(item.data.coldCaseStatusKey),T("archive.stamp.cold"),false,null,()=>{});
  }
 }
 void ArchiveCasePage(string caseId,string sourceId,string focusReference=null) {
  var item=ClosedCases().FirstOrDefault(entry=>entry.data.id==caseId);
  if(item==null){ArchivePage();return;}
  var data=item.data;var progress=item.progress;
  ArchiveBar(T(data.titleKey),()=>ArchiveCasePage(caseId,sourceId,focusReference));
  var sources=KarineUI.ArchiveSources(root,T("archive.sources"));
  KarineUI.ArchiveSource(sources,"document",T("archive.reportAndReview"),sourceId==null,()=>ArchiveCasePage(caseId,null));
  if(data.timelineClues!=null && data.timelineClues.Length>0)
   KarineUI.ArchiveSource(sources,"clock",T("file.tab.timeline"),sourceId==ArchiveTimelineId,()=>ArchiveCasePage(caseId,ArchiveTimelineId));
  var available=data.nodes.Where(node=>node.id!="report" && (progress.read.Contains(node.id) ||
   node.kind=="interview" && progress.interviewTurns.Any(turn=>turn.nodeId==node.id))).ToArray();
  foreach(var node in available) {
   var id=node.id;
   KarineUI.ArchiveSource(sources,node.kind=="cctv"?"cctv":node.kind=="interview"?"people":"document",T(node.titleKey),sourceId==id,()=>ArchiveCasePage(caseId,id));
  }
  var paper=KarineUI.GuidancePaper(root,KarineTheme.Archive.Sheet,0);
  var selected=available.FirstOrDefault(node=>node.id==sourceId);
  if(sourceId==ArchiveTimelineId) {
   KarineUI.ArchiveHeading(paper,T("timeline.title"));
   var pinned=(data.timelineClues ?? new TimelineClue[0])
    .Where(clue=>progress.timelinePinned.Contains(clue.id) &&
     (clue.requiresRead==null || clue.requiresRead.All(progress.read.Contains)) &&
     (clue.requiresAsked==null || clue.requiresAsked.All(progress.asked.Contains)))
    .OrderBy(clue=>clue.sortMinute).ThenBy(clue=>clue.id).ToArray();
   if(pinned.Length==0)KarineUI.GuidanceText(paper,T("timeline.empty"),KarineTheme.Archive.FieldSize);
   foreach(var clue in pinned)KarineUI.ArchiveField(paper,T(clue.timeKey),T(clue.noteKey));
  } else if(selected==null)ArchiveReport(paper,item);
  else if(selected.kind=="interview") {
   var turns=progress.interviewTurns.Where(turn=>turn.nodeId==selected.id).ToArray();
   // Tarih yalnız kaydı tutulan görüşmede yazılır (3 Ekim 2026'dan önceki kayıtlarda saat yok).
   var first=turns.FirstOrDefault(turn=>turn.askedAtUtcTicks>0);
   KarineUI.ArchiveHeading(paper,T("archive.transcript"),first==null?null:
    T("archive.date")+": "+new DateTime(first.askedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy  ·  HH:mm"));
   foreach(var turn in turns) {
    KarineUI.ArchiveField(paper,T("interview.bora"),T(turn.promptKey));
    var answer=KarineUI.ArchiveField(paper,T(selected.personNameKey),T(turn.answerKey),focusReference==game.InterviewTurnReference(turn));
    if(!string.IsNullOrEmpty(turn.sourceId))ArchiveSourceLink(paper,item,turn.sourceId,T("archive.presented")+" ");
    KarineUI.ArchiveRule(paper);
    if(focusReference==game.InterviewTurnReference(turn))ArchiveFocus(answer);
   }
  } else if(selected.kind=="cctv") {
   KarineUI.ArchiveHeading(paper,T(selected.titleKey),T(selected.cctvPeriodKey));
   KarineUI.GuidanceText(paper,T(selected.cctvSourceKey),KarineTheme.Archive.SmallSize);
   foreach(var record in selected.cctvEvents ?? new CctvEvent[0]) {
    var time=string.IsNullOrEmpty(record.overlayTimeKey)?"·":T(record.overlayTimeKey);
    var row=KarineUI.ArchiveField(paper,time,T(record.textKey),focusReference==selected.id+"#"+record.id);
    if(focusReference==selected.id+"#"+record.id)ArchiveFocus(row);
   }
  } else {
   KarineUI.ArchiveHeading(paper,T(selected.titleKey));
   KarineUI.GuidanceText(paper,T(selected.bodyKey),KarineTheme.Archive.FieldSize);
  }
  var space=new VisualElement();space.style.flexGrow=1;paper.Add(space);
  KarineUI.InterviewAction(paper,null,T("archive.back"),ArchivePage);
 }
 // Varsayılan görünüm: solda gönderilen rapor (dayanak bağlantılarıyla), sağda değerlendirme.
 void ArchiveReport(VisualElement paper,ArchivedCase item) {
  var data=item.data;var progress=item.progress;
  var columns=new VisualElement();columns.style.flexDirection=FlexDirection.Row;paper.Add(columns);
  var left=new VisualElement();left.style.flexGrow=1;left.style.flexBasis=0;left.style.paddingRight=KarineTheme.SpaceXl;columns.Add(left);
  var right=new VisualElement();right.style.flexGrow=1;right.style.flexBasis=0;right.style.paddingLeft=KarineTheme.SpaceXl;
  right.style.borderLeftWidth=2;right.style.borderLeftColor=KarineTheme.Paper.Ink;columns.Add(right);
  KarineUI.ArchiveHeading(left,T("archive.report"));
  var suspect=data.verdicts.FirstOrDefault(v=>v.id==progress.reportSuspect);
  var method=data.methods.FirstOrDefault(v=>v.id==progress.reportMethod);
  var custody=(data.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==progress.reportCustody);
  // Kanıt adımı 3 Ekim'de kalktı; yalnız eski kayıtlarda dolu olduğu için o zaman gösterilir.
  var proof=data.evidence.FirstOrDefault(v=>v.id==progress.reportProof);
  var fax=game.Career.reviewHistory.LastOrDefault(review=>review.caseId==data.id);
  Action<string,string,string> claim=(key,label,source)=>{
   if(label==null)return;
   KarineUI.ArchiveField(left,key,T(label));
   if(!string.IsNullOrEmpty(source))ArchiveSourceLink(left,item,source,null);
   KarineUI.ArchiveRule(left);
  };
  claim(T(SuspectKey(data)),suspect?.labelKey,progress.reportSuspectSource);
  claim(T(MethodKey(data)),method?.labelKey,progress.reportMethodSource);
  claim(ReportCustodyHeading(data),custody?.labelKey,progress.reportCustodySource);
  claim(T("conclude.evidence"),proof?.labelKey,progress.reportProofSource);
  KarineUI.ArchiveHeading(right,T("archive.fax"));
  if(fax==null){KarineUI.GuidanceText(right,T("archive.pendingReview"),KarineTheme.Archive.FieldSize);return;}
  KarineUI.InkStamp(right,EvaluationTitle(fax),fax.correct?KarineTheme.Paper.Approved:KarineTheme.Paper.Stamp,KarineTheme.Archive.HeadSize).style.marginBottom=KarineTheme.SpaceMd;
  Action<string,object,bool> verdict=(key,choice,ok)=>{if(choice!=null)KarineUI.ArchiveField(right,key,Verdict(ok),false,!ok);};
  verdict(T(SuspectKey(data)),suspect,fax.suspectSupported);
  verdict(T(MethodKey(data)),method,fax.methodSupported);
  verdict(ReportCustodyHeading(data),custody,fax.custodySupported);
  verdict(T("conclude.evidence"),proof,fax.proofSupported);
  KarineUI.GuidanceStatus(right,"chart",T("career.trust"),T(TrustStatusKey(fax.trustAfter)),fax.trustChange);
  if(game.Career.reviewHistory.Any(r=>r.caseId==data.id && r.reopened))KarineUI.GuidanceText(right,T("retry.recordNote"),KarineTheme.Archive.SmallSize);
 }
 // Dayanak bağlantısı: oyuncu kaydı açmışsa o kayda gider, açmamışsa soluk yazı kalır.
 void ArchiveSourceLink(VisualElement parent,ArchivedCase item,string reference,string prefix) {
  bool available=ArchiveReferenceAvailable(item,reference,out var source);
  var turn=source!=null && source.kind=="interview"
   ?item.progress.interviewTurns.FirstOrDefault(t=>t.nodeId==source.id && game.InterviewTurnReference(t)==reference):null;
  var label=turn==null?ReviewSourceTitle(item.data,reference):T(source.personNameKey)+" · "+T(turn.promptKey);
  var link=KarineUI.ArchiveLink(parent,(prefix??"")+label,available?()=>ArchiveCasePage(item.data.id,source.id,reference):(Action)null);
 }
 void ArchiveFocus(VisualElement target) {
  var scroll=target.GetFirstAncestorOfType<ScrollView>();
  scroll?.schedule.Execute(()=>scroll.ScrollTo(target)).ExecuteLater(1);
 }
}
}
