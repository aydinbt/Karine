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
// Soruşturma talepleri, zaman çizelgesi, dosya ve karşılaştırma ekranları.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 // Talepler tam ekrandır (3 Ekim 2026): masa üstünde koyu liste paneli ve kâğıt, üstte "Masaya dön".
 VisualElement RequestFrame(bool interviews,Action self) {
  showingInterviewList=false;showingInvestigationRequests=false;
  Desk();KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("back.desk"),T(game.Data.titleKey),T("file.unit"),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(self));
  Back(Desk);
  var list=KarineUI.RequestPanel(root,out var tabs);
  KarineUI.RequestTab(tabs,"people",T("tablet.tab.interviews"),InterviewBadgeCount(),interviews,()=>InterviewRequests(false));
  KarineUI.RequestTab(tabs,"search",T("tablet.tab.investigations"),InvestigationBadgeCount(),!interviews,()=>InvestigationRequests(false));
  return list;
 }
 // Bekleyen etiketin süresi her saniye yeniden yazılır.
 void LiveWait(VisualElement pill,string label,long readyTicks) {
  var text=pill.Q<Label>("RequestPillText");var ready=new DateTime(readyTicks,DateTimeKind.Utc);
  Action write=()=>text.text=(label+" · "+KarineUI.Countdown(ready)).ToUpper(KarineUI.TextCulture);
  write();text.schedule.Execute(write).Every(1000);
 }
 string selectedRequestPerson,selectedRequestDocument;
 readonly List<string> warrantBasis=new List<string>();
 void InterviewRequests(bool lift=true) {
  var list=RequestFrame(true,()=>InterviewRequests(false));
  lastPendingCount=game.Data.nodes.Count(pendingPredicate);showingInterviewList=true;
  var groups=game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId).ToArray();
  var nodes=groups.Select(g=>g.FirstOrDefault(n=>!game.State.read.Contains(n.id))??g.Last()).ToArray();
  var selected=nodes.FirstOrDefault(n=>n.personId==selectedRequestPerson)??nodes.FirstOrDefault();
  if(selected==null){Text(list,T("desk.none"),Muted,KarineTheme.Requests.BodySize);return;}
  // Listede görülen, henüz istenmemiş kişi bir daha sayılmaz.
  game.State.seenRequests??=new List<string>();
  foreach(var n in nodes)if(game.CanRequest(n)&&!Seen("person:"+n.id))game.State.seenRequests.Add("person:"+n.id);
  Save();
  Node Said(Node node)=>groups.First(g=>g.Key==node.personId).LastOrDefault(n=>game.State.read.Contains(n.id)&&!string.IsNullOrEmpty(n.personQuoteKey));
  foreach(var node in nodes) {
   var target=node;var key=InterviewStatusKey(node);var said=Said(node);
   var row=KarineUI.RequestRow(list,Portrait(node.personId),null,T(node.personNameKey),T(node.personInfoKey),
    said==null?null:"“"+T(said.personQuoteKey)+"”",node==selected,key=="interview.status.ready",T(key),InterviewTone(node),
    ()=>{selectedRequestPerson=target.personId;InterviewRequests(false);});
   if(game.Pending(node))LiveWait(row.Q("RequestPill"),T("requests.waiting"),WaitTicks(node));
  }
  var paper=KarineUI.RequestPaper(root);
  string statusKey=InterviewStatusKey(selected);
  KarineUI.RequestPersonHead(paper,Portrait(selected.personId),T(selected.personNameKey),T(selected.personInfoKey),T(statusKey),InterviewTone(selected));
  if(game.Pending(selected))LiveWait(paper.Q("RequestPill"),T("requests.waiting"),WaitTicks(selected));
  if(game.Pending(selected)) {
   var request=game.State.interviewRequests.First(r=>r.nodeId==selected.id);
   KarineUI.RequestWait(paper,T("requests.waitTitle"),T("requests.waitText"),new DateTime(request.readyAtUtcTicks,DateTimeKind.Utc));
   SkipWait(paper,request,()=>InterviewRequests(false));
   return;
  }
  var quoted=Said(selected);
  KarineUI.RequestSection(paper,T("requests.lastStatement"),quoted!=null?"“"+T(quoted.personQuoteKey)+"”":T("interview.noStatement"));
  if(game.Closed(selected)){DoorClosed(selected);KarineUI.RequestSection(paper,T("requests.note"),locale.Has(selected.closedNoteKey)?T(selected.closedNoteKey):T("interview.goneNote"));}
  if(game.CanRequest(selected))KarineUI.RequestAction(paper,"people",T("interview.request"),true,()=>{
   if(game.RequestInterview(selected.id)){audioDirector?.Play("ui_dial");Save();InterviewRequests(false);}
  });
  else if(game.Available(selected))KarineUI.RequestAction(paper,"people",T(game.State.read.Contains(selected.id)?"interview.resume":"interview.begin"),true,()=>InterviewPage(selected));
 }
 long WaitTicks(Node node)=>game.State.interviewRequests.First(r=>r.nodeId==node.id).readyAtUtcTicks;
 RequestTone InterviewTone(Node node) {
  var key=InterviewStatusKey(node);
  return key=="interview.status.gone"?RequestTone.Gone:key=="interview.status.unrequested"?RequestTone.Open:key=="interview.pending"?RequestTone.Waiting:
   key=="interview.status.complete"?RequestTone.Done:RequestTone.Ready;
 }
 string InterviewStatusKey(Node node) =>
  game.Closed(node)?"interview.status.gone":game.CanRequest(node)?"interview.status.unrequested":game.Pending(node)?"interview.pending":game.State.read.Contains(node.id)?
   ((node.questions??new Question[0]).Any(q=>game.CanAskQuestion(node,q))?"interview.status.followup":"interview.status.complete"):"interview.status.ready";
 // Sekme sayacı: görüşmeye hazır kişi ve yeni açılmış, listede henüz görülmemiş kişi.
 int InterviewBadgeCount() => game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId)
  .Count(g=>g.Any(n=>game.Available(n)&&!game.State.read.Contains(n.id) || game.CanRequest(n)&&!Seen("person:"+n.id)));
 // Gelen ama okunmamış inceleme raporları ile yeni açılmış ama listede henüz görülmemiş incelemeler.
 // Yalnız "listede yeni bir satır var" der; hangisinin önemli olduğunu söylemez.
 int InvestigationBadgeCount() => game.Data.nodes.Count(n=>n.kind=="document"&&n.requestable&&!game.State.read.Contains(n.id)&&
  (game.IncomingDocument(n) || game.UnseenWarrantDenial(n) || game.Discovered(n)&&!game.State.documentRequests.Any(r=>r.nodeId==n.id)&&!(game.State.seenRequests??new List<string>()).Contains(n.id)));
 void InvestigationRequests(bool lift=true) {
  var list=RequestFrame(false,()=>InvestigationRequests(false));
  lastIncomingDocumentCount=game.Data.nodes.Count(incomingDocumentPredicate);showingInvestigationRequests=true;
  var documents=game.Data.nodes.Where(n=>n.kind=="document"&&n.requestable&&(game.Discovered(n)||game.State.documentRequests.Any(r=>r.nodeId==n.id))).ToArray();
  var selected=documents.FirstOrDefault(n=>n.id==selectedRequestDocument)??documents.FirstOrDefault();
  if(selected==null){Text(list,T("tablet.noInvestigations"),Muted,KarineTheme.Requests.BodySize);return;}
  if(selectedRequestDocument!=selected.id)warrantBasis.Clear();
  selectedRequestDocument=selected.id;
  // Listede görülen yeni inceleme bir daha sayılmaz; rozet sonraki açılışta düşer.
  game.State.seenRequests??=new List<string>();
  foreach(var d in documents)if(!game.State.seenRequests.Contains(d.id))game.State.seenRequests.Add(d.id);
  Save();
  string Status(Node node)=>Investigation.IsLine(node) && game.LineClosed(node.id)?"tablet.lineClosed":
   Investigation.IsLine(node) && game.LineReopening(node.id)!=null?"tablet.investigationPending":
   Investigation.IsLine(node) && game.LineActive(node.id)?"tablet.lineActive":
   !string.IsNullOrEmpty(node.line) && !game.State.read.Contains(node.id) && !game.State.documentRequests.Any(r=>r.nodeId==node.id) && !game.LineActive(node.line)?"tablet.lineClosed":
   game.State.read.Contains(node.id)?"tablet.investigationFiled":game.IncomingDocument(node)?"tablet.investigationArrived":
   game.State.documentRequests.Any(r=>r.nodeId==node.id)||game.WarrantDenialPending(node)?"tablet.investigationPending":
   game.WarrantDenied(node)?"tablet.warrantDenied":"tablet.investigationAvailable";
  long ReadyTicks(Node node)=>game.LineReopening(node.id)?.readyAtUtcTicks ?? game.State.documentRequests.FirstOrDefault(r=>r.nodeId==node.id)?.readyAtUtcTicks ?? game.Denial(node).readyAtUtcTicks;
  RequestTone Tone(string key)=>key=="tablet.investigationFiled"||key=="tablet.lineClosed"?RequestTone.Done:key=="tablet.lineActive"?RequestTone.Open:key=="tablet.investigationArrived"||key=="tablet.warrantDenied"?RequestTone.Open:
   key=="tablet.investigationPending"?RequestTone.Waiting:RequestTone.Ready;
  foreach(var node in documents) {
   var target=node;var key=Status(node);
   var row=KarineUI.RequestRow(list,null,Investigation.IsLine(node)?"search":Investigation.IsWarrant(node)?"lock":"document",T(node.titleKey),null,null,node==selected,
    key=="tablet.investigationArrived"||game.UnseenWarrantDenial(node),T(key),Tone(key),
    ()=>{selectedRequestDocument=target.id;InvestigationRequests(false);});
   if(key=="tablet.investigationPending")LiveWait(row.Q("RequestPill"),T(key),ReadyTicks(node));
  }
  var paper=KarineUI.RequestPaper(root);var status=Status(selected);
  bool warrant=Investigation.IsWarrant(selected);
  KarineUI.RequestDocumentHead(paper,Investigation.IsLine(selected)?"search":warrant?"lock":"document",warrant?WT(selected,"Heading"):T("requests.docHeading"),T(selected.titleKey));
  KarineUI.RequestSection(paper,T("requests.description"),warrant?WT(selected,"Help"):T("tablet.investigationHelp"));
  KarineUI.RequestFact(paper,T("requests.status"),T(status));
  if(!warrant)KarineUI.RequestFact(paper,T("requests.result"),T("requests.resultText"));
  if(status=="tablet.investigationPending")KarineUI.RequestWait(paper,warrant?WT(selected,"WaitTitle"):T("requests.docWaitTitle"),warrant?WT(selected,"WaitText"):T("requests.docWaitText"),
   new DateTime(ReadyTicks(selected),DateTimeKind.Utc));
  if(status=="tablet.investigationPending")PrioritySkip(paper,selected);
  if(warrant && status=="tablet.warrantDenied") {
   KarineUI.RequestSection(paper,WT(selected,"DeniedTitle"),WT(selected,"DeniedText"));
   var denial=game.Denial(selected);if(!denial.seen){denial.seen=true;Save();}
  }
  bool line=Investigation.IsLine(selected);
  if(line) KarineUI.RequestFact(paper,T("requests.lineSlots"),string.Format(T("requests.lineSlotsValue"),game.ActiveLines,game.LineSlots));
  if(line && game.CanCloseLine(selected)) {
   KarineUI.RequestSection(paper,T("requests.lineOpenTitle"),T("requests.lineOpenText"));
   KarineUI.RequestAction(paper,"close",T("requests.lineClose"),true,()=>{if(game.CloseLine(selected.id)){Save();InvestigationRequests(false);}});
  } else if(line && game.LineClosed(selected.id)) {
   KarineUI.RequestSection(paper,T("requests.lineClosedTitle"),T("requests.lineClosedText"));
   bool free=game.CanReopenLine(selected);
   KarineUI.RequestAction(paper,"refresh",free?T("requests.lineReopen"):T("requests.lineFull"),free,()=>{if(game.ReopenLine(selected.id)){Save();InvestigationRequests(false);}});
  } else if(line && !game.State.read.Contains(selected.id) && !game.State.documentRequests.Any(r=>r.nodeId==selected.id) && !game.LineSlotFree)
   KarineUI.RequestSection(paper,T("requests.lineFullTitle"),T("requests.lineFullText"));
  if(warrant && game.CanRequestWarrant(selected)) WarrantForm(paper,selected);
  else if(game.CanRequestDocument(selected))KarineUI.RequestAction(paper,"document",T(selected.requestLabelKey),true,()=>{
   if(game.RequestDocument(selected.id)){Save();InvestigationRequests(false);}
  });
  else if(status=="tablet.investigationArrived")KarineUI.RequestAction(paper,"document",T("requests.openReport"),true,InboxPage);
 }
 // İnceleme izni formu: okunmuş kaynaklar arasından dayanak seçilir. Liste her okunmuş kaynağı
 // aynı biçimde gösterir; hangisinin işe yarayacağını söylemez.
 // İzin, olay bağlantısı ve arşiv taraması aynı motoru paylaşır; yalnız metinler ayrılır.
 string WT(Node n,string suffix) {
  string kind=string.IsNullOrEmpty(n.requestKind)?"warrant":n.requestKind,key="requests."+kind+suffix;
  return locale.Has(key)?T(key):T("requests.warrant"+suffix);
 }
 void WarrantForm(VisualElement paper,Node selected) {
  int slots=game.WarrantSlots(selected);
  KarineUI.RequestSection(paper,WT(selected,"Basis"),string.Format(T("requests.warrantBasisHelp"),slots));
  foreach(var id in WarrantCandidates()) {
   var source=id;bool chosen=warrantBasis.Contains(source);
   KarineUI.RequestBasis(paper,CompactReportSourceLabel(source),chosen,()=>{
    if(warrantBasis.Contains(source))warrantBasis.Remove(source);
    else if(warrantBasis.Count<slots)warrantBasis.Add(source);
    InvestigationRequests(false);
   });
  }
  bool ready=warrantBasis.Count==slots;
  KarineUI.RequestAction(paper,"lock",ready?WT(selected,"Send"):string.Format(T("requests.warrantChoose"),slots-warrantBasis.Count),ready,()=>{
   if(ready && game.SubmitWarrant(selected.id,warrantBasis.ToArray())){warrantBasis.Clear();Save();InvestigationRequests(false);}
  });
 }
 IEnumerable<string> WarrantCandidates() {
  foreach(var n in game.Data.nodes) {
   if(n.notReportSource)continue;
   if(n.kind=="cctv"&&game.State.read.Contains(n.id)){foreach(var e in n.cctvEvents??new CctvEvent[0])yield return n.id+"#"+e.id;}
   else if(n.kind=="interview"&&game.ReportSourceAvailable(n.id))yield return n.id;
   else if(n.kind=="document"&&game.State.read.Contains(n.id))yield return n.id;
  }
 }
 void FilePage() {
  var previousPaper=root.Q("DossierPaper");
  bool openingFile=previousPaper==null;
  if(selectedFileSection=="notebook")selectedFileSection="report";
  bool switchingSection=!openingFile && (previousPaper.userData as string)!=selectedFileSection;
  Back(Desk); // dosya masasının üstünde açılır
  showingInterviewList=false;
  var report=game.Data.nodes.First(n=>n.id=="report");
  var all=game.Data.nodes.Where(n=>game.Available(n) || game.State.closed && (game.State.read.Contains(n.id) || game.State.interviewTurns.Any(turn=>turn.nodeId==n.id))).ToArray();
  var interviewPages=all.Where(n=>n.kind=="interview" && (game.State.read.Contains(n.id) || game.State.interviewTurns.Any(turn=>turn.nodeId==n.id))).ToArray();
  var evidencePages=all.Where(n=>n.kind=="document" && n.id!=report.id).ToArray();
  Node[] pages=selectedFileSection=="interview"?interviewPages:selectedFileSection=="evidence"?evidencePages:selectedFileSection=="timeline"?new Node[0]:new[]{report};
  var current=pages.FirstOrDefault(n=>n.id==selectedFileNode) ?? pages.FirstOrDefault();
  if(current!=null) {
   selectedFileNode=current.id;
   if(!game.State.read.Contains(current.id) && game.Read(current.id)) {
    Save();
    // Arşivden gelen eski dosya ilk açılışta masaya "yeniden açıldı" kartıyla düşer.
    if(!string.IsNullOrEmpty(current.reopenYear)) {
     var reopened=current;
     root.schedule.Execute(()=>KarineUI.FileReopened(root,reopened.reopenYear,T(reopened.titleKey),T("file.reopenedStamp"),
      string.IsNullOrEmpty(reopened.reopenNoteKey)?null:T(reopened.reopenNoteKey))).StartingIn(250);
    }
   }
  }
  if(selectedFileSection=="interview" && game.State.seenInterviewTurns!=game.State.interviewTurns.Count) {
   game.State.seenInterviewTurns=game.State.interviewTurns.Count;
   Save();
  }
  Desk();
  KarineUI.InboxScene(root);
  KarineUI.DossierCase(root);
  var paper=KarineUI.DossierPage(root);
  paper.userData=selectedFileSection;
  KarineUI.PaperWear(paper,current!=null?current.id:selectedFileSection);
  // Masadan açılış siyah perdeyle; dosyanın içinde sekme ve sayfa değişince yalnız kâğıt
  // hafifçe yana kayarak gelir, çerçeve yerinde kalır.
  bool switchingPage=!openingFile && !switchingSection && shownFileNode!=current?.id;
  shownFileNode=current?.id;
  if(openingFile)SceneVeil();else if(switchingSection || switchingPage)KarineMotion.Page(paper);
  EchoLeaving(current);
  FileFrame(paper,report,pages,current);
 }
 // Satırın altındaki tarih yalnız belgenin künyesinde tarih varsa yazılır.
 string PageDate(Node node) {
  var field=node.fileMeta?.FirstOrDefault(f=>f.labelKey.EndsWith(".date"));
  return field!=null?T(field.valueKey):null;
 }
 // 3 Ekim 2026 maketi: solda tür ve kişi süzgeçleri, sağda sonuç kâğıdı. Yazı alanı yok —
 // klavye telefonda ekranın yarısını kaplıyordu ve aranacak sözcüğü bilmek oyuncunun işi değil.
 // Kişi süzgeci vakaya özel liste tutmaz, "adı geçtiyse" kuralını kullanır.
 static readonly string[] SearchKinds={"conclude.filter.all","conclude.filter.documents","conclude.filter.interviews","conclude.filter.cctv"};
 void FileSearchPage() {
  showingInterviewList=false;
  Desk();
  KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("back.file"),T(game.Data.titleKey),T("file.unit"),FilePage,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(FileSearchPage));
  Back(FilePage);
  var lines=CaseSearch.Lines(game,locale);
  var people=game.Data.nodes.Where(n=>n.kind=="interview" && !string.IsNullOrEmpty(n.personId))
   .GroupBy(n=>n.personId).Select(g=>g.First()).ToArray();
  if(!people.Any(p=>p.personId==fileFilterPerson))fileFilterPerson="";
  var panel=KarineUI.SearchFilters(root);
  KarineUI.SearchGroup(panel,T("search.kind"),true);
  string[] kindIcons={"document","document","people","cctv"};
  for(int i=0;i<SearchKinds.Length;i++){var pick=i;KarineUI.SearchFilter(panel,kindIcons[i],null,T(SearchKinds[i]),fileFilterKind==i,()=>{fileFilterKind=pick;FileSearchPage();});}
  KarineUI.SearchGroup(panel,T("search.person"),false);
  KarineUI.SearchFilter(panel,"people",null,T("search.everyone"),fileFilterPerson=="",()=>{fileFilterPerson="";FileSearchPage();});
  foreach(var person in people){var pick=person.personId;
   KarineUI.SearchFilter(panel,"person",Portrait(pick),T(person.personNameKey),fileFilterPerson==pick,()=>{fileFilterPerson=pick;FileSearchPage();});}
  var hits=lines.Where(line=>{
   var source=game.Data.nodes.FirstOrDefault(n=>n.id==line.nodeId);
   if(source==null)return false;
   int kind=source.kind=="cctv"?3:source.kind=="interview"?2:1;
   if(fileFilterKind!=0 && fileFilterKind!=kind)return false;
   return string.IsNullOrEmpty(fileFilterPerson) || game.MentionsPerson(fileFilterPerson,line.text);
  }).ToArray();
  var paper=KarineUI.SearchPaper(root);
  KarineUI.SearchHead(paper,T("search.title"),T("search.count")+"  "+hits.Length);
  RecentSearches(paper,people);
  if(hits.Length==0){KarineUI.SearchEmpty(paper,T("search.empty"));return;}
  var results=new KarineScrollView(ScrollViewMode.Vertical);results.style.flexGrow=1;paper.Add(results);
  var chosen=people.FirstOrDefault(p=>p.personId==fileFilterPerson);
  var name=chosen==null?null:T(chosen.personNameKey);
  foreach(var hit in hits) {
   var source=game.Data.nodes.FirstOrDefault(n=>n.id==hit.nodeId);
   if(source==null)continue;
   var selected=hit;
   string icon=source.kind=="cctv"?"cctv":source.kind=="interview"?"people":"document";
   KarineUI.SearchResult(results,icon,CompareTitle(source),PageDate(source),"“"+Highlight(hit.excerpt,name)+"”",T("search.open"),()=>OpenSearchSource(selected));
  }
  KarineUI.Stream(results);
 }
 // Seçili kişinin adı (ya da yalnız ilk adı) satırda geçiyorsa amberle işaretlenir. Bu süzgecin
 // kendisidir, ipucu değil: satır zaten bu ad geçtiği için listede.
 static string Highlight(string text,string name) {
  text=(text ?? string.Empty).Replace("<","‹");
  if(string.IsNullOrEmpty(name))return text;
  foreach(var candidate in new[]{name,name.Split(' ')[0]}) {
   int at=KarineUI.TextCulture.CompareInfo.IndexOf(text,candidate,System.Globalization.CompareOptions.IgnoreCase);
   if(at<0)continue;
   return text.Substring(0,at)+"<mark=#E0A04070>"+text.Substring(at,candidate.Length)+"</mark>"+text.Substring(at+candidate.Length);
  }
  return text;
 }
 void OpenSearchSource(SearchHit hit) {
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==hit.nodeId);
  if(source==null)return;
  if(source.kind=="cctv"){cctvFromDesk=false;CctvScreen(source,hit.eventId);}
  else if(source.kind=="bps")ReadPage(source);
  else {
   selectedSearchTurn=hit.turnReference;
   selectedFileSection=source.kind=="interview"?"interview":source.id=="report"?"report":"evidence";
   selectedFileNode=source.id;FilePage();
  }
 }
 Node[] ComparisonSources() => game.Data.nodes.Where(n=>
  (n.kind=="document" || n.kind=="cctv" || n.kind=="bps") && game.State.read.Contains(n.id)
  || n.kind=="interview" && (game.State.read.Contains(n.id) || game.State.interviewTurns.Any(t=>t.nodeId==n.id))
 ).ToArray();
 void ReadPage(Node node) {
  showingInterviewList=false;
  game.Read(node.id);Save();
  // Tablet kaldırıldı (3 Ekim 2026): dijital kayıt da düz sayfada okunur.
  Frame(T("kind."+node.kind),T(node.titleKey),T("file.reference"));
  var scroll=Scroll(root);
  var paper=Panel(scroll);
  Text(paper,T(node.bodyKey),Ink,20);
  Button(root,T("back.file"),FilePage,true);
 }
}
}
