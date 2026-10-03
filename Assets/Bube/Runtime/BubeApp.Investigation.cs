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
  Action write=()=>text.text=(label+" · "+KarineUI.Countdown(ready)).ToUpper(KarineUI.Tr);
  write();text.schedule.Execute(write).Every(1000);
 }
 string selectedRequestPerson,selectedRequestDocument;
 void InterviewRequests(bool lift=true) {
  var list=RequestFrame(true,()=>InterviewRequests(false));
  lastPendingCount=game.Data.nodes.Count(pendingPredicate);showingInterviewList=true;
  var groups=game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId).ToArray();
  var nodes=groups.Select(g=>g.FirstOrDefault(n=>!game.State.read.Contains(n.id))??g.Last()).ToArray();
  var selected=nodes.FirstOrDefault(n=>n.personId==selectedRequestPerson)??nodes.FirstOrDefault();
  if(selected==null){Text(list,T("desk.none"),Muted,KarineTheme.Requests.BodySize);return;}
  Node Said(Node node)=>groups.First(g=>g.Key==node.personId).LastOrDefault(n=>game.State.read.Contains(n.id)&&!string.IsNullOrEmpty(n.personQuoteKey));
  foreach(var node in nodes) {
   var target=node;var key=InterviewStatusKey(node);var said=Said(node);
   var row=KarineUI.RequestRow(list,Resources.Load<Texture2D>("Bube/Characters/"+node.personId),null,T(node.personNameKey),T(node.personInfoKey),
    said==null?null:"“"+T(said.personQuoteKey)+"”",node==selected,key=="interview.status.ready",T(key),InterviewTone(node),
    ()=>{selectedRequestPerson=target.personId;InterviewRequests(false);});
   if(game.Pending(node))LiveWait(row.Q("RequestPill"),T("requests.waiting"),WaitTicks(node));
  }
  var paper=KarineUI.RequestPaper(root);
  string statusKey=InterviewStatusKey(selected);
  KarineUI.RequestPersonHead(paper,Resources.Load<Texture2D>("Bube/Characters/"+selected.personId),T(selected.personNameKey),T(selected.personInfoKey),T(statusKey),InterviewTone(selected));
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
 // Sekme sayacı yalnız oyuncunun zaten gördüğü durumları sayar: görüşmeye hazır kişi, dosyaya alınmamış gelen rapor.
 int InterviewBadgeCount() => game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId)
  .Count(g=>g.Any(n=>game.Available(n)&&!game.State.read.Contains(n.id)));
 int InvestigationBadgeCount() => game.Data.nodes.Count(n=>n.kind=="document"&&n.requestable&&game.IncomingDocument(n)&&!game.State.read.Contains(n.id));
 void InvestigationRequests(bool lift=true) {
  var list=RequestFrame(false,()=>InvestigationRequests(false));
  lastIncomingDocumentCount=game.Data.nodes.Count(incomingDocumentPredicate);showingInvestigationRequests=true;
  var documents=game.Data.nodes.Where(n=>n.kind=="document"&&n.requestable&&(game.Discovered(n)||game.State.documentRequests.Any(r=>r.nodeId==n.id))).ToArray();
  var selected=documents.FirstOrDefault(n=>n.id==selectedRequestDocument)??documents.FirstOrDefault();
  if(selected==null){Text(list,T("tablet.noInvestigations"),Muted,KarineTheme.Requests.BodySize);return;}
  string Status(Node node)=>game.State.read.Contains(node.id)?"tablet.investigationFiled":game.IncomingDocument(node)?"tablet.investigationArrived":
   game.State.documentRequests.Any(r=>r.nodeId==node.id)?"tablet.investigationPending":"tablet.investigationAvailable";
  RequestTone Tone(string key)=>key=="tablet.investigationFiled"?RequestTone.Done:key=="tablet.investigationArrived"?RequestTone.Open:
   key=="tablet.investigationPending"?RequestTone.Waiting:RequestTone.Ready;
  foreach(var node in documents) {
   var target=node;var key=Status(node);
   var row=KarineUI.RequestRow(list,null,"document",T(node.titleKey),null,null,node==selected,key=="tablet.investigationArrived",T(key),Tone(key),
    ()=>{selectedRequestDocument=target.id;InvestigationRequests(false);});
   if(key=="tablet.investigationPending")LiveWait(row.Q("RequestPill"),T(key),game.State.documentRequests.First(r=>r.nodeId==node.id).readyAtUtcTicks);
  }
  var paper=KarineUI.RequestPaper(root);var status=Status(selected);
  KarineUI.RequestDocumentHead(paper,"document",T("requests.docHeading"),T(selected.titleKey));
  KarineUI.RequestSection(paper,T("requests.description"),T("tablet.investigationHelp"));
  KarineUI.RequestFact(paper,T("requests.status"),T(status));
  KarineUI.RequestFact(paper,T("requests.result"),T("requests.resultText"));
  if(status=="tablet.investigationPending")KarineUI.RequestWait(paper,T("requests.docWaitTitle"),T("requests.docWaitText"),
   new DateTime(game.State.documentRequests.First(r=>r.nodeId==selected.id).readyAtUtcTicks,DateTimeKind.Utc));
  if(game.CanRequestDocument(selected))KarineUI.RequestAction(paper,"document",T(selected.requestLabelKey),true,()=>{
   if(game.RequestDocument(selected.id)){Save();InvestigationRequests(false);}
  });
  else if(status=="tablet.investigationArrived")KarineUI.RequestAction(paper,"document",T("requests.openReport"),true,InboxPage);
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
   if(!game.State.read.Contains(current.id) && game.Read(current.id))Save();
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
   KarineUI.SearchFilter(panel,"person",Resources.Load<Texture2D>("Bube/Characters/"+pick),T(person.personNameKey),fileFilterPerson==pick,()=>{fileFilterPerson=pick;FileSearchPage();});}
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
   int at=KarineUI.Tr.CompareInfo.IndexOf(text,candidate,System.Globalization.CompareOptions.IgnoreCase);
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
