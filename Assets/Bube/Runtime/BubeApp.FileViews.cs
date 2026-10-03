using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
// Dosya Detay ekranının çerçevesi ve kâğıt içerikleri (3 Ekim 2026 maketleri,
// Docs/Reference/UI_FILE_*_2026-10.png). `FilePage` hangi bölümün ve sayfanın
// açık olduğunu belirler; burası çizer.
public sealed partial class BubeApp {
 string selectedTimelineClue, selectedNote;

 void FileFrame(VisualElement paper,Node report,Node[] pages,Node current) {
  KarineUI.DossierBar(root,T("back.desk"),T(game.Data.titleKey),T("file.unit"),Desk,out var tools);
  KarineUI.DossierTool(tools,"search",T("file.tab.search"),FileSearchPage);
  KarineUI.DossierTool(tools,"compare",T("file.tab.compare"),()=>{comparePicker=-1;ComparePage();});
  if(game.CanConclude)KarineUI.DossierTool(tools,"chart",T("conclude.tab"),Conclusion);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(FilePage));
  if(selectedFileSection=="timeline")TimelineView(paper);
  else if(selectedFileSection=="notebook")NotebookView(paper);
  else {
   var list=KarineUI.DossierList(root);
   foreach(var page in pages) {
    var target=page;
    bool person=target.kind=="interview";
    KarineUI.DossierListRow(list,"document",person?T(target.personNameKey).ToUpper(KarineUI.Tr):T(target.titleKey).ToUpper(KarineUI.Tr),
     person?T("file.row.interview"):null,PageDate(target),target==current,!game.State.read.Contains(target.id),
     ()=>{if(selectedFileNode==target.id)return;selectedFileNode=target.id;FilePage();});
   }
   if(selectedFileSection=="report")ReportPaper(paper,report);
   else if(current==null)KarineUI.DossierParagraph(paper,T(selectedFileSection=="interview"?"file.emptyInterview":"file.emptyEvidence"),KarineTheme.Paper.Faded);
   else if(current.kind=="interview")InterviewPaper(paper,current);
   else EvidencePaper(paper,current);
  }
  var tabs=KarineUI.DossierTabColumn(root);
  var icons=new[]{"document","people","folder","clock","document"};int tabIndex=0;
  foreach(var section in new[]{"report","interview","evidence","timeline","notebook"}) {
   var choice=section;
   var unread=choice=="interview" && game.State.interviewTurns.Count>game.State.seenInterviewTurns;
   KarineUI.DossierFolderTab(tabs,icons[tabIndex++],T("file.folder."+choice),choice==selectedFileSection,unread,
    ()=>{if(selectedFileSection==choice)return;selectedFileSection=choice;FilePage();});
  }
 }
 string CaseNumber() => T(game.Data.titleKey).Split(new[]{'—'},2)[0].Trim();
 Texture2D Photo(string resource) => string.IsNullOrEmpty(resource)?null:Resources.Load<Texture2D>(resource);

 void ReportPaper(VisualElement paper,Node report) {
  var meta=KarineUI.DossierPageHead(paper,CaseNumber(),T(report.titleKey),Photo(report.imageResource));
  if(report.fileMeta!=null)foreach(var field in report.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  var scroll=Scroll(paper);scroll.name="DossierOverview";scroll.style.flexGrow=1;scroll.style.minHeight=0;
  scroll.contentContainer.style.paddingBottom=KarineTheme.SpaceXl;
  KarineUI.DossierParagraph(scroll,T(report.bodyKey));
  var people=game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId).Select(g=>g.First()).ToArray();
  if(people.Length>0) {
   KarineUI.PaperRule(scroll,true);
   KarineUI.DossierSection(scroll,"people",T("file.relatedPeople"));
   foreach(var person in people)
    KarineUI.DossierPerson(scroll,Resources.Load<Texture2D>("Bube/Characters/"+person.personId),T(person.personNameKey),T(person.personInfoKey));
  }
  if(report.relatedItems!=null && report.relatedItems.Length>0) {
   KarineUI.DossierSection(scroll,"image",T("file.relatedItems"));
   var shelf=new VisualElement();shelf.style.flexDirection=FlexDirection.Row;scroll.Add(shelf);
   foreach(var item in report.relatedItems)
    KarineUI.DossierItem(shelf,Resources.Load<Texture2D>(item.imageResource),T(item.nameKey),T(item.detailKey));
  }
 }

 void EvidencePaper(VisualElement paper,Node current) {
  var meta=KarineUI.DossierPageHead(paper,CaseNumber(),T(current.titleKey),null);
  if(current.fileMeta!=null)foreach(var field in current.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  var body=Scroll(paper);body.style.flexGrow=1;body.style.minHeight=0;
  MarkableBody(body,current,KarineTheme.Paper.Ink,KarineTheme.Paper.Faded);
  var texture=Photo(current.imageResource);
  if(texture!=null) {
   var photo=new Image{image=texture,scaleMode=ScaleMode.ScaleAndCrop};
   photo.style.height=240;photo.style.marginTop=16;
   KarineUI.Develop(photo,current.imageResource);
   KarineUI.Tilt(KarineUI.Inspectable(body,photo,current.imageResource,T("ink.draw"),T("ink.clear")));
   Text(body,T(current.imageCaptionKey),KarineTheme.Paper.Faded,13);
  }
 }

 // Sorgu dökümü: künye, ifade, sonra sorulan her soru ve cevabı konuşan/metin sütunlarında.
 // Öne sürülen kaydın yanında kırmızı mühür; hükmü oyuncu verir, mühür yalnız "gösterildi" der.
 void InterviewPaper(VisualElement paper,Node current) {
  var meta=KarineUI.DossierPageHead(paper,null,T("file.interviewTitle"),Photo(current.imageResource)??Resources.Load<Texture2D>("Bube/Characters/"+current.personId));
  KarineUI.DossierKeyValue(meta,T("file.meta.speaker"),T(current.personNameKey));
  if(current.fileMeta!=null)foreach(var field in current.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  KarineUI.PaperRule(paper,true);
  var body=Scroll(paper);body.style.flexGrow=1;body.style.minHeight=0;
  string person=T(current.personNameKey);
  KarineUI.DossierLine(body,person,T(current.bodyKey));
  var turns=game.State.interviewTurns.Where(turn=>turn.nodeId==current.id).ToArray();
  if(turns.Length==0){KarineUI.DossierParagraph(body,T("file.noTranscript"),KarineTheme.Paper.Faded);return;}
  foreach(var turn in turns) {
   bool shown=!string.IsNullOrEmpty(turn.sourceId);
   KarineUI.DossierLine(body,T("interview.bora"),T(turn.promptKey)+(shown?"\n"+T("interview.presented")+" "+ReviewSourceTitle(game.Data,turn.sourceId):""),
    shown?T("interview.presentedStamp"):null);
   var answer=KarineUI.DossierLine(body,person,T(turn.answerKey));
   if(game.InterviewTurnReference(turn)==selectedSearchTurn) {
    selectedSearchTurn=null;
    body.schedule.Execute(()=>body.ScrollTo(answer));
   }
  }
 }

 // Zaman çizelgesi: listede oyuncunun çizelgesi, kâğıtta seçili olayın ayrıntısı ve dikey çizgi,
 // altta kaynaklardan eklenebilir saatler. Sıralama saate göredir; yorum oyuncunundur.
 void TimelineView(VisualElement paper) {
  var clues=game.Data.timelineClues ?? new TimelineClue[0];
  var pinned=clues.Where(c=>game.State.timelinePinned.Contains(c.id)).OrderBy(c=>c.sortMinute).ThenBy(c=>c.id).ToArray();
  var available=game.State.closed?new TimelineClue[0]:clues.Where(c=>game.TimelineAvailable(c) && !game.State.timelinePinned.Contains(c.id))
   .OrderBy(c=>c.sortMinute).ThenBy(c=>c.id).ToArray();
  var selected=pinned.FirstOrDefault(c=>c.id==selectedTimelineClue)??pinned.FirstOrDefault();
  selectedTimelineClue=selected?.id;
  var list=KarineUI.DossierList(root,T("timeline.listTitle"));
  if(pinned.Length==0)Text(list,T("timeline.empty"),KarineTheme.Secondary,15);
  foreach(var clue in pinned) {
   var id=clue.id;
   KarineUI.DossierListRow(list,"clock",T(clue.timeKey),T(clue.noteKey),null,clue==selected,false,()=>{selectedTimelineClue=id;FilePage();});
  }
  if(selected!=null) {
   KarineUI.DossierPageHead(paper,T(selected.sourceKey),T(selected.timeKey),null,"clock");
   KarineUI.DossierParagraph(paper,T(selected.noteKey));
  } else {
   KarineUI.DossierPageHead(paper,null,T("timeline.title"),null,"clock");
   KarineUI.DossierParagraph(paper,T("timeline.help"),KarineTheme.Paper.Faded);
  }
  var scroll=Scroll(paper);scroll.style.flexGrow=1;scroll.style.minHeight=0;
  for(int i=0;i<pinned.Length;i++) {
   var clue=pinned[i];var id=clue.id;
   KarineUI.DossierTimeRow(scroll,T(clue.timeKey),T(clue.noteKey),i==0,i==pinned.Length-1,clue==selected,
    ()=>{selectedTimelineClue=id;FilePage();},
    game.State.closed?null:(Action)(()=>{if(game.UnpinTimeline(id)){Save();FilePage();}}),T("timeline.remove")).name="Clue-"+id;
  }
  if(game.State.closed)return;
  var strip=KarineUI.DossierAddStrip(paper,T("timeline.addTitle"),T("timeline.addHelp"));
  if(available.Length==0)Text(strip,T("timeline.noCandidates"),KarineTheme.Secondary,14);
  foreach(var clue in available) {
   var id=clue.id;
   KarineUI.DossierAddCard(strip,T(clue.timeKey),T(clue.noteKey),()=>{
    if(game.PinTimeline(id)){KarineUI.Cue("pin");selectedTimelineClue=id;Save();FilePage();KarineUI.PinShake(root.Q("Clue-"+id));}
   },T("timeline.add"));
  }
 }

 // Not defteri: listede oyuncunun işlediği kaynak çiftleri ve altını çizdiği belgeler,
 // kâğıtta kareli yaprakta seçili not. Notlar oyuncunundur; doğru/yanlış söylenmez.
 void NotebookView(VisualElement paper) {
  var pairs=game.State.notebook.ToArray();
  var lined=game.Data.nodes.Select(n=>(node:n,lines:Investigation.Sentences(T(n.bodyKey))
   .Where((_,i)=>game.State.highlights.Contains(Investigation.HighlightId(n.id,i))).ToArray())).Where(e=>e.lines.Length>0).ToArray();
  var keys=pairs.Select((p,i)=>"pair:"+i).Concat(lined.Select(e=>"lines:"+e.node.id)).ToArray();
  if(!keys.Contains(selectedNote))selectedNote=keys.FirstOrDefault();
  var list=KarineUI.DossierList(root,T("notebook.listTitle"));
  for(int i=0;i<pairs.Length;i++) {
   var key="pair:"+i;var note=pairs[i];
   KarineUI.DossierListRow(list,"compare",NotebookSourceTitle(note.leftId)+" ↔ "+NotebookSourceTitle(note.rightId),T("notebook.mark."+note.mark),null,
    key==selectedNote,false,()=>{selectedNote=key;FilePage();});
  }
  foreach(var entry in lined) {
   var key="lines:"+entry.node.id;
   KarineUI.DossierListRow(list,"document",T(entry.node.titleKey),T("notebook.lineTitle"),null,key==selectedNote,false,()=>{selectedNote=key;FilePage();});
  }
  KarineUI.DossierNotebookSheet(paper);
  var ink=KarineTheme.Paper.Ink;
  if(selectedNote==null) {
   var empty=Text(paper,T("notebook.title"),ink,28);KarineUI.Handwrite(empty,ink);
   KarineUI.DossierParagraph(paper,T("notebook.help"),KarineTheme.Paper.Faded);
   return;
  }
  var scroll=Scroll(paper);scroll.style.flexGrow=1;scroll.style.minHeight=0;
  if(selectedNote.StartsWith("pair:")) {
   var note=pairs[int.Parse(selectedNote.Substring(5))];
   var title=Text(scroll,T("notebook.pairTitle"),ink,28);KarineUI.Handwrite(title,ink);
   var markInk=note.mark=="conflict"?KarineTheme.Paper.Stamp:ink;
   var mark=Text(scroll,Shape(note.mark)+T("notebook.mark."+note.mark),markInk,22);KarineUI.Handwrite(mark,markInk);
   Text(scroll,"– "+NotebookSourceTitle(note.leftId),ink,19);
   Text(scroll,"– "+NotebookSourceTitle(note.rightId),ink,19);
   var strip=KarineUI.DossierToolStrip(paper);
   KarineUI.DossierStripButton(strip,"compare",T("notebook.open"),()=>{compareLeftId=note.leftId;compareRightId=note.rightId;comparePicker=-1;ComparePage();},true);
   if(!game.State.closed)KarineUI.DossierStripButton(strip,"close",T("timeline.remove"),()=>{if(game.RemoveNote(note)){selectedNote=null;Save();FilePage();}});
  } else {
   var entry=lined.First(e=>"lines:"+e.node.id==selectedNote);
   var title=Text(scroll,T(entry.node.titleKey),ink,28);KarineUI.Handwrite(title,ink);
   foreach(var line in entry.lines) {
    var l=Text(scroll,"“"+line+"”",ink,18);l.style.marginBottom=8;l.style.paddingLeft=10;
    l.style.borderLeftWidth=2;l.style.borderLeftColor=KarineTheme.Paper.Stamp;
   }
  }
 }
 // Arşivdeki vaka sayfası eski satırı kullanır.
 void TimelineRow(VisualElement parent,TimelineClue clue,Color ink,Color muted,string actionKey,Action action) {
  var row=new VisualElement{name="Clue-"+clue.id};row.style.flexDirection=FlexDirection.Row;
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
}
}
