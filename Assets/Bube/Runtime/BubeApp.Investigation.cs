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
 void InterviewRequests(bool lift=true) {
  lastPendingCount=game.Data.nodes.Count(pendingPredicate);
  VisualElement content;
  BpsTablet("tablet.interviews",out content,lift);
  showingInterviewList=true;
  RequestTabs(content,true);
  var intro=new VisualElement();intro.style.flexDirection=FlexDirection.Row;
  intro.style.alignItems=Align.Center;intro.style.marginBottom=8;content.Add(intro);
  var caseLine=Text(intro,CaseText("tablet.caseLine","tablet.caseLine"),Gold,14);
  caseLine.style.marginBottom=0;caseLine.style.marginRight=20;
  var help=Text(intro,T("tablet.interviewHelp"),Muted,14);
  help.style.marginBottom=0;
  var list=Scroll(content);
  var groups=game.Data.nodes.Where(n=>n.kind=="interview" && game.Discovered(n))
   .GroupBy(n=>n.personId).ToArray();
  if(groups.Length==0)Text(list,T("desk.none"),Muted,19);
  foreach(var group in groups) {
   var node=group.FirstOrDefault(n=>!game.State.read.Contains(n.id)) ?? group.Last();
   var quoted=group.LastOrDefault(n=>game.State.read.Contains(n.id) && !string.IsNullOrEmpty(n.personQuoteKey));
   var card=new VisualElement();card.style.flexDirection=FlexDirection.Row;
   card.style.alignItems=Align.Center;
   card.style.minHeight=116;card.style.marginBottom=10;
   card.style.paddingLeft=12;card.style.paddingRight=12;
   card.style.paddingTop=8;card.style.paddingBottom=8;
   card.style.backgroundColor=KarineTheme.Background;
   card.style.borderTopWidth=1;card.style.borderBottomWidth=1;
   card.style.borderLeftWidth=1;card.style.borderRightWidth=1;
   card.style.borderTopColor=Muted;card.style.borderBottomColor=Muted;
   card.style.borderLeftColor=Muted;card.style.borderRightColor=Muted;
   list.Add(card);
   var portrait=Resources.Load<Texture2D>("Bube/Characters/"+node.personId);
   if(portrait!=null) {
    portrait.filterMode=FilterMode.Point;
    var thumb=new Image {image=portrait,scaleMode=ScaleMode.ScaleToFit};
    thumb.style.width=100;thumb.style.height=100;thumb.style.marginRight=16;
    thumb.style.backgroundColor=Base;card.Add(thumb);
   }
   var details=new VisualElement();details.style.flexGrow=1;card.Add(details);
   var name=Text(details,T(node.personNameKey),Ink,22);name.style.marginBottom=2;
   var info=Text(details,T(node.personInfoKey),Muted,14);info.style.marginBottom=6;
   var quote=Text(details,quoted!=null?"“"+T(quoted.personQuoteKey)+"”":T("interview.noStatement"),Muted,15);
   quote.style.marginBottom=0;
   var action=new VisualElement();action.style.width=Length.Percent(30);
   action.style.paddingLeft=12;card.Add(action);
   string statusKey=game.CanRequest(node)?"interview.status.unrequested":game.Pending(node)?"interview.pending":game.State.read.Contains(node.id)?
    ((node.questions ?? new Question[0]).Any(q=>game.CanAskQuestion(node,q))?"interview.status.followup":"interview.status.complete"):"interview.status.ready";
   var status=Text(action,T(statusKey),game.Pending(node)?Gold:Muted,14);
   status.style.marginBottom=8;
   if(game.CanRequest(node))Button(action,T("interview.request"),()=>{
    if(game.RequestInterview(node.id)){Save();InterviewRequests(false);}
   });
   else if(game.Available(node))
    Button(action,T(game.State.read.Contains(node.id)?"interview.resume":"interview.begin"),()=>InterviewPage(node),true);
   FitActionButton(action);
  }
 }
 void InvestigationRequests(bool lift=true) {
  lastIncomingDocumentCount=game.Data.nodes.Count(incomingDocumentPredicate);
  VisualElement content;
  BpsTablet("tablet.investigations",out content,lift);
  showingInvestigationRequests=true;
  RequestTabs(content,false);
  Text(content,CaseText("tablet.caseLine","tablet.caseLine"),Gold,14).style.marginBottom=5;
  Text(content,T("tablet.investigationHelp"),Muted,15).style.marginBottom=9;
  var list=Scroll(content);
  var documents=game.Data.nodes.Where(n=>n.kind=="document" && n.requestable &&
   (game.Discovered(n) || game.State.documentRequests.Any(r=>r.nodeId==n.id))).ToArray();
  if(documents.Length==0)Text(list,T("tablet.noInvestigations"),Muted,18);
  foreach(var node in documents) {
   var current=node;
   var card=new VisualElement();card.style.marginBottom=10;
   card.style.paddingLeft=15;card.style.paddingRight=15;
   card.style.paddingTop=11;card.style.paddingBottom=10;
   card.style.backgroundColor=KarineTheme.Background;
   card.style.borderLeftWidth=3;card.style.borderLeftColor=Gold;
   list.Add(card);
   Text(card,T(node.titleKey),Ink,21).style.marginBottom=4;
   bool filed=game.State.read.Contains(node.id);
   bool arrived=game.IncomingDocument(node);
   bool requested=game.State.documentRequests.Any(r=>r.nodeId==node.id);
   string statusKey=filed?"tablet.investigationFiled":arrived?"tablet.investigationArrived":
    requested?"tablet.investigationPending":"tablet.investigationAvailable";
   Text(card,T(statusKey),filed?Muted:arrived?Gold:Muted,15).style.marginBottom=8;
   if(game.CanRequestDocument(node)) {
    KarineUI.Button_(card,T(node.requestLabelKey),()=>{
     if(game.RequestDocument(current.id)){Save();InvestigationRequests(false);}
    },KarineButtonKind.Primary);
   }
  }
 }
 void TimelineRow(VisualElement parent,TimelineClue clue,Color ink,Color muted,string actionKey,Action action) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;row.style.minHeight=70;
  row.style.marginBottom=6;row.style.paddingLeft=9;row.style.paddingRight=8;
  row.style.backgroundColor=KarineTheme.Paper.Tint;
  row.style.borderLeftWidth=3;row.style.borderLeftColor=KarineTheme.Paper.Stamp;
  parent.Add(row);
  // Saat teknik metindir (kit §4): monospace, sütun hizası bozulmaz.
  var time=KarineUI.Technical(row,T(clue.timeKey),17);
  time.style.color=ink;time.style.width=112;time.style.marginBottom=0;
  var details=new VisualElement();details.style.flexGrow=1;row.Add(details);
  Text(details,T(clue.noteKey),ink,15).style.marginBottom=2;
  Text(details,T(clue.sourceKey),muted,12).style.marginBottom=0;
  if(action==null)return;
  var button=KarineUI.PaperButton(row,T(actionKey),action,KarinePaperKind.Action);
  button.style.width=80;button.style.minHeight=40;
  button.style.marginLeft=7;button.style.fontSize=Typography.Snap(13);
 }
 void TimelineContents(VisualElement paper,Color ink,Color muted) {
  var title=Text(paper,T("timeline.title"),ink,21);
  title.style.marginBottom=4;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(paper,T("timeline.help"),muted,14).style.marginBottom=10;
  var scroll=Scroll(paper);
  Text(scroll,T("timeline.pinned"),ink,17).style.marginBottom=5;
  var pinnedList=new VisualElement();scroll.Add(pinnedList);
  var divider=new VisualElement();divider.style.height=1;divider.style.marginTop=14;
  divider.style.marginBottom=12;divider.style.backgroundColor=KarineTheme.Paper.Edge;scroll.Add(divider);
  Text(scroll,T("timeline.available"),ink,17).style.marginBottom=5;
  var availableList=new VisualElement();scroll.Add(availableList);
  Action refresh=null;
  refresh=()=>{
   pinnedList.Clear();availableList.Clear();
   var clues=game.Data.timelineClues ?? new TimelineClue[0];
   var pinned=clues.Where(c=>game.State.timelinePinned.Contains(c.id)).OrderBy(c=>c.sortMinute).ThenBy(c=>c.id).ToArray();
   if(pinned.Length==0)Text(pinnedList,T("timeline.empty"),muted,15);
   foreach(var clue in pinned) {
    var id=clue.id;
    TimelineRow(pinnedList,clue,ink,muted,game.State.closed?null:"timeline.remove",game.State.closed?null:(Action)(()=>{
     if(game.UnpinTimeline(id)){Save();refresh();}
    }));
   }
   var available=game.State.closed?new TimelineClue[0]:clues.Where(c=>game.TimelineAvailable(c) && !game.State.timelinePinned.Contains(c.id))
    .OrderBy(c=>c.sortMinute).ThenBy(c=>c.id).ToArray();
   if(available.Length==0)Text(availableList,T("timeline.noCandidates"),muted,15);
   foreach(var clue in available) {
    var id=clue.id;
    TimelineRow(availableList,clue,ink,muted,"timeline.add",()=>{
     if(game.PinTimeline(id)){Save();refresh();}
    });
   }
  };
  refresh();
 }
 void FileTab(VisualElement column,string label,Action open,Color background) {
  var tab=KarineUI.PaperButton(column,label,open,KarinePaperKind.Choice,true);
  tab.style.flexGrow=1;tab.style.marginBottom=5;tab.style.paddingRight=8;
  tab.style.backgroundColor=background;tab.style.fontSize=Typography.Snap(15);
 }
 void FilePage() {
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
  var shade=new VisualElement();
  shade.style.position=Position.Absolute;shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.78f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(14);folder.style.right=Length.Percent(21);
  folder.style.top=Length.Percent(9);folder.style.bottom=Length.Percent(8);
  folder.style.backgroundColor=KarineTheme.Paper.Folder;
  folder.style.borderBottomWidth=8;folder.style.borderBottomColor=KarineTheme.Paper.FolderDeep;
  root.Add(folder);
  for(int i=0;i<3;i++) {
   var sheet=new VisualElement();sheet.style.position=Position.Absolute;
   sheet.style.left=Length.Percent(15+i*.35f);sheet.style.right=Length.Percent(22-i*.35f);
   sheet.style.top=Length.Percent(8+i*.55f);sheet.style.bottom=Length.Percent(8-i*.55f);
   sheet.style.backgroundColor=KarineTheme.Paper.Tint;root.Add(sheet);
  }
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(16);paper.style.right=Length.Percent(23);
  paper.style.top=Length.Percent(7);paper.style.bottom=Length.Percent(9);
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  paper.style.paddingLeft=32;paper.style.paddingRight=30;paper.style.paddingTop=22;paper.style.paddingBottom=15;
  paper.style.borderLeftWidth=2;paper.style.borderTopWidth=2;
  paper.style.borderLeftColor=KarineTheme.Paper.Light;paper.style.borderTopColor=KarineTheme.Paper.Light;
  root.Add(paper);
  var fileInk=KarineTheme.Paper.Ink;var fileMuted=KarineTheme.Paper.Faded;
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;header.style.marginBottom=12;paper.Add(header);
  var titles=new VisualElement();titles.style.flexGrow=1;header.Add(titles);
  var title=Text(titles,T(game.Data.titleKey),fileInk,26);title.style.marginBottom=2;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(titles,CaseText("file.caseType","file.caseType"),fileInk,17);
  var stamp=KarineUI.Technical(header,T("file.stamp"),13);
  stamp.style.color=fileMuted;stamp.style.unityTextAlign=TextAnchor.UpperRight;
  var line=new VisualElement();line.style.height=1;line.style.backgroundColor=KarineTheme.Paper.Edge;line.style.marginBottom=15;paper.Add(line);
  if(selectedFileSection=="timeline") {
   TimelineContents(paper,fileInk,fileMuted);
  } else if(selectedFileSection=="visual") {
   Text(paper,T("file.visuals"),fileInk,22);
   var visual=Resources.Load<Texture2D>(report.imageResource);
   if(visual!=null) {
    var img=new Image{image=visual,scaleMode=ScaleMode.ScaleToFit};img.style.flexGrow=1;paper.Add(img);
    Text(paper,T(report.imageCaptionKey),fileMuted,15);
   } else Text(paper,T("file.emptyVisual"),fileMuted,18);
  } else if(current==null) {
   Text(paper,T(selectedFileSection=="interview"?"file.emptyInterview":"file.emptyEvidence"),fileMuted,19);
   var filler=new VisualElement();filler.style.flexGrow=1;paper.Add(filler);
  } else {
   var heading=Text(paper,T(current.titleKey).ToUpperInvariant(),fileInk,19);
   if(dossierBoldFont!=null)heading.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
   // Metin ile görsel yan yana iki sütundaydı: telefonda metin yarı genişliğe
   // düşüyor, kendi kaydırma çubuğunu kazanıyor ve sayfa içinde ikinci bir
   // kaydırma alanı doğuyordu. Tek sütun, tek kaydırma; görsel akışın içinde.
   var textColumn=new VisualElement();textColumn.style.flexGrow=1;paper.Add(textColumn);
   if(current.fileMeta!=null)foreach(var field in current.fileMeta) {
    var meta=Text(textColumn,T(field.labelKey)+"  :  "+T(field.valueKey),fileInk,14);meta.style.marginBottom=5;
   }
   if(current.fileMeta!=null && current.fileMeta.Length>0) {
    var divider=new VisualElement();divider.style.height=1;divider.style.marginTop=9;divider.style.marginBottom=15;
    divider.style.backgroundColor=KarineTheme.Paper.Edge;textColumn.Add(divider);
   }
   var body=Scroll(textColumn);
   Text(body,T(current.bodyKey),fileInk,17);
   if(current.kind=="interview") {
    var turns=game.State.interviewTurns.Where(turn=>turn.nodeId==current.id).ToArray();
    if(turns.Length>0) {
     var transcriptTitle=Text(body,T("file.transcript"),fileInk,18);
     transcriptTitle.style.marginTop=16;
     for(int turnIndex=0;turnIndex<turns.Length;turnIndex++) {
      var turn=turns[turnIndex];
      Text(body,(turnIndex+1)+"  "+T("interview.bora"),fileMuted,14);
      Text(body,T(turn.promptKey),fileInk,16);
      if(!string.IsNullOrEmpty(turn.sourceId)) {
       Text(body,T("interview.presented")+"  "+ReviewSourceTitle(game.Data,turn.sourceId),fileMuted,14);
      }
      Text(body,T(current.personNameKey),fileMuted,14);
      var answer=Text(body,T(turn.answerKey),fileInk,16);
      answer.style.marginBottom=13;
      if(game.InterviewTurnReference(turn)==selectedSearchTurn) {
       selectedSearchTurn=null;
       body.schedule.Execute(()=>body.ScrollTo(answer));
      }
     }
    } else Text(body,T("file.noTranscript"),fileMuted,15);
   }
   if(!string.IsNullOrEmpty(current.imageResource)) {
    var texture=Resources.Load<Texture2D>(current.imageResource);
    if(texture!=null) {
     var photo=new Image{image=texture,scaleMode=ScaleMode.ScaleAndCrop};
     photo.style.height=240;photo.style.marginTop=16;body.Add(photo);
     Text(body,T(current.imageCaptionKey),fileMuted,13);
    }
   }
  }
  // "1 / 1" sayacı ve Önceki/Sonraki, tek sayfalık bölümlerde bile duruyordu ve
  // istenen sayfaya varmak için art arda dokunmak gerekiyordu. Sayfa birden
  // çoksa adları doğrudan dokunulur; tekse alt şerit hiç çizilmez.
  if(selectedFileSection!="timeline" && pages.Length>1) {
   var footerLine=new VisualElement();footerLine.style.height=1;footerLine.style.flexShrink=0;
   footerLine.style.backgroundColor=KarineTheme.Paper.Edge;footerLine.style.marginTop=10;paper.Add(footerLine);
   var footer=new ScrollView(ScrollViewMode.Horizontal);
   footer.style.flexShrink=0;footer.style.marginTop=8;
   footer.contentContainer.style.flexDirection=FlexDirection.Row;paper.Add(footer);
   foreach(var page in pages) {
    var target=page;
    var chip=KarineUI.PaperButton(footer,T(target.titleKey),()=>{selectedFileNode=target.id;FilePage();},
     target==current?KarinePaperKind.Action:KarinePaperKind.Choice);
    chip.style.marginRight=6;chip.style.paddingLeft=14;chip.style.paddingRight=14;
    chip.style.fontSize=Typography.Snap(15);
   }
  }
  // Sekme şeridi kendi içinde kayıyordu: dar bir sütuna sekiz sekme sığmadığı
  // için bir kısmı ekran dışında kalıyor, oraya varmak için önce şeridi
  // kaydırmak gerekiyordu. Şerit genişledi, kaydırma kalktı — hepsi görünür.
  // Ayrıca dört ayrı yerde kopyalanan sekme biçimi tek yere toplandı; yeni bir
  // sekme eklemek artık tek satır.
  var tabs=new VisualElement();tabs.style.position=Position.Absolute;
  tabs.style.left=Length.Percent(78);tabs.style.top=Length.Percent(18);
  tabs.style.right=Length.Percent(3);tabs.style.bottom=Length.Percent(7);
  root.Add(tabs);
  foreach(var section in new[]{"report","interview","evidence","timeline","visual"}) {
   var choice=section;
   var unread=choice=="interview" && game.State.interviewTurns.Count>game.State.seenInterviewTurns;
   FileTab(tabs,T("file.tab."+choice)+(unread?"  •":""),()=>{selectedFileSection=choice;FilePage();},
    choice==selectedFileSection?KarineTheme.Paper.Sheet:KarineTheme.Paper.Edge);
  }
  FileTab(tabs,T("file.tab.compare"),()=>{comparePicker=-1;ComparePage();},KarineTheme.Paper.Edge);
  FileTab(tabs,T("file.tab.search"),FileSearchPage,KarineTheme.Paper.Edge);
  if(game.CanConclude)FileTab(tabs,T("conclude.tab"),Conclusion,KarineTheme.Paper.Stamp);
  var close=KarineUI.CloseButton(root,Desk,T("back.desk"));
  close.style.position=Position.Absolute;close.style.right=Length.Percent(8);close.style.top=Length.Percent(7);
  close.style.width=58;close.style.height=58;close.style.fontSize=Typography.Snap(36);
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
  var count=Text(paper,"",muted,14);count.style.marginTop=4;count.style.marginBottom=6;
  var results=Scroll(paper);
  var kindButtons=new List<Button>();var personButtons=new List<Button>();
  Action render=null;
  Func<VisualElement> shelf=()=>{
   var row=new ScrollView(ScrollViewMode.Horizontal);row.style.flexShrink=0;
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
   results.Clear();
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
 }
 void ReadPage(Node node) {
  showingInterviewList=false;
  game.Read(node.id);Save();
  if(node.kind=="bps") {
   VisualElement content;BpsTablet(node.titleKey,out content);
   TerminalSourceTabs(content,node);
   var tabletScroll=Scroll(content);Text(tabletScroll,T(node.bodyKey),Ink,19);
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
