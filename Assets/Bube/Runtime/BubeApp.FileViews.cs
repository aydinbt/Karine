using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
// Dosya Detay ekranının çerçevesi ve kâğıt içerikleri (3 Ekim 2026 maketleri,
// Docs/Reference/UI_FILE_*_2026-10.png). `FilePage` hangi bölümün ve sayfanın
// açık olduğunu belirler; burası çizer.
public sealed partial class BubeApp {
 string selectedTimelineClue,shownFileNode;

 void FileFrame(VisualElement paper,Node report,Node[] pages,Node current) {
  KarineUI.DossierBar(root,T("back.desk"),T(game.Data.titleKey),T("file.unit"),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(FilePage));
  if(selectedFileSection=="timeline")TimelineView(paper);
  else if(selectedFileSection=="visual")VisualPaper(paper,report);
  else {
   var list=KarineUI.DossierList(root);
   foreach(var page in pages) {
    var target=page;
    bool person=target.kind=="interview";
    KarineUI.DossierListRow(list,"document",person?T(target.personNameKey).ToUpper(KarineUI.TextCulture):T(target.titleKey).ToUpper(KarineUI.TextCulture),
     person?T("file.row.interview"):null,PageDate(target),target==current,!game.State.read.Contains(target.id),
     ()=>{if(selectedFileNode==target.id)return;selectedFileNode=target.id;FilePage();});
   }
   if(selectedFileSection=="report")ReportPaper(paper,report);
   else if(current==null)KarineUI.DossierParagraph(paper,T(selectedFileSection=="interview"?"file.emptyInterview":"file.emptyEvidence"),KarineTheme.Paper.Faded);
   else if(current.kind=="interview")InterviewPaper(paper,current);
   else EvidencePaper(paper,current);
  }
  var tabs=KarineUI.DossierTabColumn(root);
  // Not defteri sekmesi kaldırıldı (3 Ekim 2026); yerine eski dosya sayfasının sekmeleri:
  // görseller, karşılaştır, dosyada gezin ve hazırsa sonuç.
  var icons=new[]{"document","people","folder","clock","image"};int tabIndex=0;
  foreach(var section in new[]{"report","interview","evidence","timeline","visual"}) {
   var choice=section;
   var unread=choice=="interview" && game.State.interviewTurns.Count>game.State.seenInterviewTurns;
   KarineUI.DossierFolderTab(tabs,icons[tabIndex++],T("file.folder."+choice),choice==selectedFileSection,unread,
    ()=>{if(selectedFileSection==choice)return;selectedFileSection=choice;FilePage();});
  }
  KarineUI.DossierFolderTab(tabs,"compare",T("file.folder.compare"),false,false,()=>{comparePicker=-1;ComparePage();});
  KarineUI.DossierFolderTab(tabs,"search",T("file.folder.search"),false,false,FileSearchPage);
  if(game.CanConclude)KarineUI.DossierFolderTab(tabs,"chart",T("file.folder.conclude"),false,false,Conclusion);
 }
 void VisualPaper(VisualElement paper,Node report) {
  KarineUI.DossierPageHead(paper,CaseNumber(),T("file.folder.visual"),null,"image");
  var texture=Photo(report.imageResource);
  if(texture==null){KarineUI.DossierParagraph(paper,T("file.emptyVisual"),KarineTheme.Paper.Faded);return;}
  var img=new Image{image=texture,scaleMode=ScaleMode.ScaleToFit};img.style.flexGrow=1;paper.Add(img);
  KarineUI.DossierParagraph(paper,T(report.imageCaptionKey),KarineTheme.Paper.Faded);
 }
 string CaseNumber() => T(game.Data.titleKey).Split(new[]{'—'},2)[0].Trim();
 Texture2D Photo(string resource) => string.IsNullOrEmpty(resource)?null:Resources.Load<Texture2D>(resource);

 void ReportPaper(VisualElement paper,Node report) {
  // Sayfanın tamamı kayar: künye de metinle birlikte (8 Ekim 2026).
  var scroll=Scroll(paper);scroll.name="DossierOverview";scroll.style.flexGrow=1;scroll.style.minHeight=0;
  scroll.contentContainer.style.paddingBottom=KarineTheme.SpaceXl;
  var meta=KarineUI.DossierPageHead(scroll,CaseNumber(),T(report.titleKey),Photo(report.imageResource));
  if(report.fileMeta!=null)foreach(var field in report.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  KarineUI.DossierParagraph(scroll,T(report.bodyKey));
  var people=game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n)).GroupBy(n=>n.personId).Select(g=>g.First()).ToArray();
  if(people.Length>0) {
   KarineUI.PaperRule(scroll,true);
   KarineUI.DossierSection(scroll,"people",T("file.relatedPeople"));
   foreach(var person in people)
    KarineUI.DossierPerson(scroll,Portrait(person.personId),T(person.personNameKey),T(person.personInfoKey));
  }
  if(report.relatedItems!=null && report.relatedItems.Length>0) {
   KarineUI.DossierSection(scroll,"image",T("file.relatedItems"));
   var shelf=new VisualElement();shelf.style.flexDirection=FlexDirection.Row;scroll.Add(shelf);
   foreach(var item in report.relatedItems)
    KarineUI.DossierItem(shelf,Resources.Load<Texture2D>(item.imageResource),T(item.nameKey),T(item.detailKey));
  }
  PrintOut(scroll.contentContainer,game.Data.id+":"+report.id);
 }

 void EvidencePaper(VisualElement paper,Node current) {
  var body=Scroll(paper);body.style.flexGrow=1;body.style.minHeight=0;
  var meta=KarineUI.DossierPageHead(body,CaseNumber(),T(current.titleKey),null);
  if(current.fileMeta!=null)foreach(var field in current.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  MarkableBody(body,current,KarineTheme.Paper.Ink,KarineTheme.Paper.Faded);
  var texture=Photo(current.imageResource);
  if(texture!=null) {
   var photo=new Image{image=texture,scaleMode=ScaleMode.ScaleAndCrop};
   photo.style.height=240;photo.style.marginTop=16;
   KarineUI.Develop(photo,current.imageResource);
   KarineUI.Tilt(KarineUI.Inspectable(body,photo,current.imageResource,T("ink.draw"),T("ink.clear")));
   Text(body,T(current.imageCaptionKey),KarineTheme.Paper.Faded,13);
  }
  // Olay yeri planı: yalnız kilidi açılmış işaretler görünür.
  if(current.HasScenePlan) {
   var plan=current.scenePlan;
   KarineUI.ScenePlanView(body,plan,Photo(plan.imagePath),
    (plan.markers ?? new PlanMarker[0]).Where(game.MarkerAvailable).ToList(),T,T(plan.incidentLabelKey));
  }
  PrintOut(body.contentContainer,game.Data.id+":"+current.id);
 }

 // Sorgu dökümü: künye, ifade, sonra sorulan her soru ve cevabı konuşan/metin sütunlarında.
 // Öne sürülen kaydın yanında kırmızı mühür; hükmü oyuncu verir, mühür yalnız "gösterildi" der.
 void InterviewPaper(VisualElement paper,Node current) {
  var body=Scroll(paper);body.style.flexGrow=1;body.style.minHeight=0;
  var meta=KarineUI.DossierPageHead(body,null,T("file.interviewTitle"),Photo(current.imageResource)??Portrait(current.personId));
  KarineUI.DossierKeyValue(meta,T("file.meta.speaker"),T(current.personNameKey));
  if(current.fileMeta!=null)foreach(var field in current.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  KarineUI.PaperRule(body,true);
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
