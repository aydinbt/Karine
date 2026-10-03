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
 void RequestScreenHeader(VisualElement content) {
  var screen=content.parent;
  foreach(var child in screen.Children().Where(c=>c!=content).ToArray())child.RemoveFromHierarchy();
  KarineUI.DossierHeader(root,T("back.desk"),T(game.Data.titleKey),Desk);
  var tablet=screen.parent;float bar=KarineTheme.Requests.HeaderClearance;
  tablet.style.top=Length.Percent(bar);tablet.style.left=Length.Percent(bar/2);
  tablet.style.width=Length.Percent(100-bar);tablet.style.height=Length.Percent(100-bar);
 }
 string selectedRequestPerson,selectedRequestDocument;
 void InterviewRequests(bool lift=true) {
  lastPendingCount=game.Data.nodes.Count(pendingPredicate);
  VisualElement content;BpsTablet("tablet.interviews",out content,lift);RequestScreenHeader(content);showingInterviewList=true;
  VisualElement list,detail;
  KarineUI.RequestLayout(content,T("tablet.tab.interviews"),T("tablet.tab.investigations"),true,
   ()=>InterviewRequests(false),()=>InvestigationRequests(false),out list,out detail,
   T("tablet.interviews"),T("tablet.interviewHint"),InterviewBadgeCount(),InvestigationBadgeCount());
  var groups=game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId).ToArray();
  var nodes=groups.Select(g=>g.FirstOrDefault(n=>!game.State.read.Contains(n.id))??g.Last()).ToArray();
  var selected=nodes.FirstOrDefault(n=>n.personId==selectedRequestPerson)??nodes.FirstOrDefault();
  if(selected==null){detail.style.display=DisplayStyle.None;Text(list,T("desk.none"),Muted,KarineTheme.Requests.BodySize);return;}
  foreach(var node in nodes) {
   var target=node;
   var rowStatus=InterviewStatusKey(node);
   var said=groups.First(g=>g.Key==node.personId).LastOrDefault(n=>game.State.read.Contains(n.id)&&!string.IsNullOrEmpty(n.personQuoteKey));
   KarineUI.RequestItem(list,Resources.Load<Texture2D>("Bube/Characters/"+node.personId),T(node.personNameKey),T(node.personInfoKey),node==selected,
    ()=>{selectedRequestPerson=target.personId;InterviewRequests(false);},
    T(rowStatus),rowStatus=="interview.status.ready",said==null?null:"“"+T(said.personQuoteKey)+"”");
  }
  string statusKey=InterviewStatusKey(selected);
  var body=KarineUI.RequestDetail(detail,Resources.Load<Texture2D>("Bube/Characters/"+selected.personId),T(selected.personNameKey),T(selected.personInfoKey),T(statusKey));
  var quoted=groups.First(g=>g.Key==selected.personId).LastOrDefault(n=>game.State.read.Contains(n.id)&&!string.IsNullOrEmpty(n.personQuoteKey));
  KarineUI.DossierText(body,quoted!=null?"“"+T(quoted.personQuoteKey)+"”":T("interview.noStatement"),KarineTheme.Requests.BodySize);
  if(game.Closed(selected))DoorClosed(selected);
  if(game.Closed(selected))KarineUI.DossierText(body,locale.Has(selected.closedNoteKey)?T(selected.closedNoteKey):T("interview.goneNote"),KarineTheme.Requests.BodySize);
  if(game.CanRequest(selected))KarineUI.PaperButton(detail,T("interview.request"),()=>{
   if(game.RequestInterview(selected.id)){audioDirector?.Play("ui_dial");Save();InterviewRequests(false);}
  });
  else if(game.Available(selected))KarineUI.PaperButton(detail,T(game.State.read.Contains(selected.id)?"interview.resume":"interview.begin"),()=>InterviewPage(selected));
  else if(game.Pending(selected))SkipWait(detail,game.State.interviewRequests.First(r=>r.nodeId==selected.id),()=>InterviewRequests(false));
 }
 string InterviewStatusKey(Node node) =>
  game.Closed(node)?"interview.status.gone":game.CanRequest(node)?"interview.status.unrequested":game.Pending(node)?"interview.pending":game.State.read.Contains(node.id)?
   ((node.questions??new Question[0]).Any(q=>game.CanAskQuestion(node,q))?"interview.status.followup":"interview.status.complete"):"interview.status.ready";
 // Sekme sayacı yalnız oyuncunun zaten gördüğü durumları sayar: görüşmeye hazır kişi, dosyaya alınmamış gelen rapor.
 int InterviewBadgeCount() => game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId)
  .Count(g=>g.Any(n=>game.Available(n)&&!game.State.read.Contains(n.id)));
 int InvestigationBadgeCount() => game.Data.nodes.Count(n=>n.kind=="document"&&n.requestable&&game.IncomingDocument(n)&&!game.State.read.Contains(n.id));
 void InvestigationRequests(bool lift=true) {
  lastIncomingDocumentCount=game.Data.nodes.Count(incomingDocumentPredicate);
  VisualElement content;BpsTablet("tablet.investigations",out content,lift);RequestScreenHeader(content);showingInvestigationRequests=true;
  VisualElement list,detail;
  KarineUI.RequestLayout(content,T("tablet.tab.interviews"),T("tablet.tab.investigations"),false,
   ()=>InterviewRequests(false),()=>InvestigationRequests(false),out list,out detail,
   T("tablet.investigations"),T("tablet.investigationHint"),InterviewBadgeCount(),InvestigationBadgeCount());
  var documents=game.Data.nodes.Where(n=>n.kind=="document"&&n.requestable&&(game.Discovered(n)||game.State.documentRequests.Any(r=>r.nodeId==n.id))).ToArray();
  var selected=documents.FirstOrDefault(n=>n.id==selectedRequestDocument)??documents.FirstOrDefault();
  if(selected==null){detail.style.display=DisplayStyle.None;Text(list,T("tablet.noInvestigations"),Muted,KarineTheme.Requests.BodySize);return;}
  foreach(var node in documents) {
   var target=node;
   bool filed=game.State.read.Contains(node.id),arrived=game.IncomingDocument(node),requested=game.State.documentRequests.Any(r=>r.nodeId==node.id);
   string status=filed?"tablet.investigationFiled":arrived?"tablet.investigationArrived":requested?"tablet.investigationPending":"tablet.investigationAvailable";
   KarineUI.RequestItem(list,null,T(node.titleKey),T(status),node==selected,()=>{selectedRequestDocument=target.id;InvestigationRequests(false);},
    null,arrived&&!filed);
   if(node==selected)KarineUI.RequestDetail(detail,null,T(node.titleKey),T("tablet.investigationHelp"),T(status));
  }
  if(game.CanRequestDocument(selected))KarineUI.PaperButton(detail,T(selected.requestLabelKey),()=>{
   if(game.RequestDocument(selected.id)){Save();InvestigationRequests(false);}
  });
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
 // Künye alanının simgesi etiketin anahtarından gelir; bilinmeyen alan simgesiz kalır.
 static string MetaIcon(string labelKey) {
  if(labelKey.EndsWith(".location"))return "pin";
  if(labelKey.EndsWith(".number"))return "folder";
  if(labelKey.EndsWith(".date"))return "calendar";
  if(labelKey.EndsWith(".reporter"))return "person";
  return "info";
 }
 void FileSearchPage() {
  showingInterviewList=false;
  Desk();
  var dark=KarineTheme.Paper.Ink;
  var muted=KarineTheme.Paper.Faded;
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.82f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(11);folder.style.right=Length.Percent(11);
  folder.style.top=Length.Percent(5);folder.style.bottom=Length.Percent(5);
  folder.style.backgroundColor=KarineTheme.Paper.Folder;root.Add(folder);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(12);paper.style.right=Length.Percent(12);
  paper.style.top=Length.Percent(6);paper.style.bottom=Length.Percent(7);
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=21;paper.style.paddingBottom=18;root.Add(paper);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;paper.Add(header);
  var title=Text(header,T("search.title"),dark,24);title.style.flexGrow=1;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var back=KarineUI.PaperButton(header,T("compare.back"),FilePage,KarinePaperKind.Action);
  back.style.minHeight=43;back.style.paddingLeft=12;back.style.paddingRight=12;
  Text(paper,T("search.help"),muted,15);
  // Yazı alanı kaldırıldı: telefonda klavye ekranın yarısını kaplıyordu ve
  // aranacak sözcüğü bilmek oyuncunun işi değil. Yerine iki dokunulur eksen —
  // kayıt türü ve kişi. Kişi süzgeci vakaya özel liste tutmaz, "adı geçtiyse"
  // kuralının aynısını kullanır, yani yeni vakalarda kendiliğinden işler.
  var lines=CaseSearch.Lines(game,locale);
  var people=game.Data.nodes.Where(n=>n.kind=="interview" && !string.IsNullOrEmpty(n.personId))
   .GroupBy(n=>n.personId).Select(g=>g.First()).ToArray();
  var filters=new VisualElement();filters.style.marginTop=6;paper.Add(filters);
  var recent=new VisualElement();paper.Add(recent);
  var count=Text(paper,"",muted,14);count.style.marginTop=4;count.style.marginBottom=6;
  var results=Scroll(paper);
  var kindButtons=new List<Button>();var personButtons=new List<Button>();
  Action render=null;
  Func<VisualElement> shelf=()=>{
   var row=new KarineScrollView(ScrollViewMode.Horizontal);row.style.flexShrink=0;
   row.contentContainer.style.flexDirection=FlexDirection.Row;filters.Add(row);return row;
  };
  Action<VisualElement,List<Button>,string,Action> chip=(row,group,label,pick)=>{
   var button=KarineUI.PaperButton(row,label,()=>{pick();render();});
   button.style.marginRight=6;button.style.marginBottom=6;
   button.style.paddingLeft=16;button.style.paddingRight=16;
   button.style.fontSize=Typography.Snap(15);
   group.Add(button);
  };
  var kindRow=shelf();
  string[] kindLabels={"conclude.filter.all","conclude.filter.documents","conclude.filter.interviews","conclude.filter.cctv"};
  for(int i=0;i<kindLabels.Length;i++){var pick=i;chip(kindRow,kindButtons,T(kindLabels[i]),()=>fileFilterKind=pick);}
  var personRow=shelf();
  chip(personRow,personButtons,T("search.everyone"),()=>fileFilterPerson="");
  foreach(var person in people){var pick=person.personId;chip(personRow,personButtons,T(person.personNameKey),()=>fileFilterPerson=pick);}
  render=()=>{
   results.Clear();RecentSearches(recent,people,render);
   for(int i=0;i<kindButtons.Count;i++) {
    kindButtons[i].style.backgroundColor=i==fileFilterKind?KarineTheme.Paper.Stamp:KarineTheme.Paper.Sheet;
    kindButtons[i].style.color=i==fileFilterKind?KarineTheme.Paper.Sheet:KarineTheme.Paper.Ink;
   }
   for(int i=0;i<personButtons.Count;i++) {
    var chosen=i==0?fileFilterPerson=="":people[i-1].personId==fileFilterPerson;
    personButtons[i].style.backgroundColor=chosen?KarineTheme.Paper.Stamp:KarineTheme.Paper.Sheet;
    personButtons[i].style.color=chosen?KarineTheme.Paper.Sheet:KarineTheme.Paper.Ink;
   }
   var hits=lines.Where(line=>{
    var source=game.Data.nodes.FirstOrDefault(n=>n.id==line.nodeId);
    if(source==null)return false;
    int kind=source.kind=="cctv"?3:source.kind=="interview"?2:1;
    if(fileFilterKind!=0 && fileFilterKind!=kind)return false;
    return string.IsNullOrEmpty(fileFilterPerson) || game.MentionsPerson(fileFilterPerson,line.text);
   }).ToArray();
   count.text=T("search.count")+"  "+hits.Length;
   if(hits.Length==0){Text(results,T("search.empty"),muted,16);return;}
   foreach(var hit in hits) {
    var source=game.Data.nodes.FirstOrDefault(n=>n.id==hit.nodeId);
    if(source==null)continue;
    var selected=hit;
    var row=new VisualElement();row.style.marginBottom=8;row.style.paddingLeft=14;
    row.style.paddingRight=14;row.style.paddingTop=10;row.style.paddingBottom=8;
    row.style.backgroundColor=KarineTheme.Paper.Tint;results.Add(row);
    var name=Text(row,T(source.titleKey),dark,16);name.style.marginBottom=3;
    if(dossierBoldFont!=null)name.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
    var excerpt=Text(row,hit.excerpt,dark,15);excerpt.style.marginBottom=4;
    var open=KarineUI.PaperButton(row,T("search.open")+"  ›",()=>OpenSearchSource(selected),KarinePaperKind.Quiet);
    open.style.minHeight=37;open.style.fontSize=Typography.Snap(15);
    open.style.unityTextAlign=TextAnchor.MiddleRight;
   }
   KarineUI.Stream(results);
  };
  render();
 }
 void OpenSearchSource(SearchHit hit) {
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==hit.nodeId);
  if(source==null)return;
  if(source.kind=="cctv")CctvScreen(source,hit.eventId);
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
 void ComparePage() {
  showingInterviewList=false;
  var sources=ComparisonSources();
  if(!sources.Any(n=>n.id==compareLeftId))compareLeftId=null;
  if(!sources.Any(n=>n.id==compareRightId))compareRightId=null;
  Desk();
  var ink=KarineTheme.Paper.Ink;
  var muted=KarineTheme.Paper.Faded;
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.84f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(4);folder.style.right=Length.Percent(4);
  folder.style.top=Length.Percent(5);folder.style.bottom=Length.Percent(5);
  folder.style.backgroundColor=KarineTheme.Paper.Folder;root.Add(folder);
  var heading=new VisualElement();heading.style.flexDirection=FlexDirection.Row;
  heading.style.alignItems=Align.Center;heading.style.paddingLeft=20;heading.style.paddingRight=20;
  heading.style.height=62;folder.Add(heading);
  var title=Text(heading,T("compare.title"),Ink,21);title.style.flexGrow=1;title.style.marginBottom=0;
  var back=KarineUI.Button_(heading,T("compare.back"),()=>{comparePicker=-1;FilePage();});
  back.style.height=42;back.style.minHeight=42;
  back.style.paddingLeft=15;back.style.paddingRight=15;
  var spread=new VisualElement();spread.style.flexDirection=FlexDirection.Row;spread.style.flexGrow=1;
  spread.style.paddingLeft=12;spread.style.paddingRight=12;spread.style.paddingBottom=12;folder.Add(spread);
  for(int side=0;side<2;side++) {
   int selectedSide=side;
   string selectedId=side==0?compareLeftId:compareRightId;
   var current=sources.FirstOrDefault(n=>n.id==selectedId);
   var sheet=new VisualElement();sheet.style.flexGrow=1;sheet.style.flexBasis=0;
   sheet.style.marginLeft=4;sheet.style.marginRight=4;sheet.style.paddingLeft=22;
   sheet.style.paddingRight=22;sheet.style.paddingTop=18;sheet.style.paddingBottom=15;
   sheet.style.backgroundColor=KarineTheme.Paper.Sheet;spread.Add(sheet);
   var picker=KarineUI.PaperButton(sheet,(current==null?T("compare.choose"):T(current.titleKey))+"  ▾",
    ()=>{comparePicker=comparePicker==selectedSide?-1:selectedSide;ComparePage();},KarinePaperKind.Choice,true);
   picker.style.minHeight=50;
   if(comparePicker==side) {
    var choices=Scroll(sheet);choices.style.maxHeight=240;
    foreach(var source in sources) {
     var choice=source;
     var option=KarineUI.PaperButton(choices,T(source.titleKey),()=>{
      if(selectedSide==0)compareLeftId=choice.id;else compareRightId=choice.id;
      comparePicker=-1;ComparePage();
     },KarinePaperKind.Choice,true);
     option.style.minHeight=44;option.style.marginTop=4;option.style.fontSize=Typography.Snap(15);
    }
    if(sources.Length==0)Text(choices,T("compare.noSources"),muted,16);
   } else if(current==null) {
    Text(sheet,sources.Length==0?T("compare.noSources"):T("compare.empty"),muted,17);
   } else {
    var body=Scroll(sheet);
    if(current.fileMeta!=null)foreach(var field in current.fileMeta) {
     var meta=Text(body,T(field.labelKey)+"  :  "+T(field.valueKey),muted,13);
     meta.style.marginBottom=4;
    }
    if(current.kind=="interview") {
     var turns=game.State.interviewTurns.Where(t=>t.nodeId==current.id).ToArray();
     if(turns.Length==0)Text(body,T("file.noTranscript"),muted,16);
     foreach(var turn in turns) {
     Text(body,T("interview.bora"),muted,13);
     Text(body,T(turn.promptKey),ink,17);
     if(!string.IsNullOrEmpty(turn.sourceId)) {
      Text(body,T("interview.presented")+"  "+ReviewSourceTitle(game.Data,turn.sourceId),muted,13);
     }
      Text(body,T(current.personNameKey),muted,13);
      var answer=Text(body,T(turn.answerKey),ink,17);answer.style.marginBottom=16;
     }
    } else if(current.kind=="cctv") {
     if(current.cctvEvents!=null)foreach(var cctvEvent in current.cctvEvents) {
      var entry=Text(body,T(cctvEvent.textKey),ink,17);entry.style.marginBottom=10;
     }
    } else Text(body,T(current.bodyKey),ink,17);
   }
  }
  NotebookBar(folder,ink);
 }
 void ReadPage(Node node) {
  showingInterviewList=false;
  game.Read(node.id);Save();
  if(node.kind=="bps") {
   VisualElement content;BpsTablet(node.titleKey,out content);
   TerminalSourceTabs(content,node);
   var tabletScroll=Scroll(content);Text(tabletScroll,T(node.bodyKey),Ink,19);KarineUI.Corrupt(tabletScroll);PrintOnce(node);
  } else {
   Frame(T("kind."+node.kind),T(node.titleKey),T("file.reference"));
   var scroll=Scroll(root);
   var paper=Panel(scroll);
   Text(paper,T(node.bodyKey),Ink,20);
   Button(root,T("back.file"),FilePage,true);
  }
 }
}
}
