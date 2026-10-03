using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
// Kaynak karşılaştırma (3 Ekim 2026 maketi): iki kâğıt yan yana, her birinin üstünde kaynak
// seçici, ortada oyuncunun kendi hükmü (çelişiyor / örtüşüyor / sorulacak). Oyun hükmü onaylamaz.
public sealed partial class BubeApp {
 void ComparePage() {
  showingInterviewList=false;
  var sources=ComparisonSources();
  if(!sources.Any(n=>n.id==compareLeftId))compareLeftId=null;
  if(!sources.Any(n=>n.id==compareRightId))compareRightId=null;
  Desk();
  KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("compare.back"),T(game.Data.titleKey),T("file.unit"),()=>{comparePicker=-1;FilePage();},out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(ComparePage));
  var host=new VisualElement {name="CompareHost",pickingMode=PickingMode.Ignore};host.style.position=Position.Absolute;
  host.style.left=0;host.style.right=0;host.style.top=0;host.style.bottom=0;root.Add(host);
  var sheets=new VisualElement[2];
  for(int side=0;side<2;side++) {
   int selectedSide=side;
   var current=sources.FirstOrDefault(n=>n.id==(side==0?compareLeftId:compareRightId));
   var sheet=sheets[side]=KarineUI.CompareSheet(host,side==0);
   KarineUI.ComparePicker(sheet,current==null?T("compare.choose"):CompareTitle(current),comparePicker==side,
    ()=>{comparePicker=comparePicker==selectedSide?-1:selectedSide;ComparePage();});
   if(current==null)KarineUI.CompareEmpty(sheet,sources.Length==0?T("compare.noSources"):T("compare.empty"));
   else CompareBody(sheet,current);
  }
  var strip=KarineUI.CompareMarks(host);
  bool ready=!game.State.closed && compareLeftId!=null && compareRightId!=null && compareLeftId!=compareRightId;
  var existing=ready?game.FindNote(compareLeftId,compareRightId):null;
  foreach(var mark in Investigation.NotebookMarks) {
   var chosen=mark;
   string glyph=mark=="conflict"?"✕":mark=="agree"?"✓":"?";
   var tone=mark=="conflict"?KarineTheme.Danger:mark=="agree"?KarineTheme.Active:KarineTheme.Accent;
   KarineUI.CompareMark(strip,glyph,tone,T("notebook.mark."+mark),existing!=null && existing.mark==mark,ready,()=>{
    // İki kaynak ataçla tutturulur: her hükümde aynı ses.
    if(game.MarkNote(compareLeftId,compareRightId,chosen)){KarineUI.Cue("clip");Save();KarineUI.RedString(host,ComparePage);}
   });
  }
  // Açık liste kâğıtların ve hüküm şeridinin üstünde durur.
  if(comparePicker>=0 && comparePicker<2) {
   var sheet=sheets[comparePicker];sheet.BringToFront();
   var menu=KarineUI.CompareMenu(sheet);
   string currentId=comparePicker==0?compareLeftId:compareRightId;
   foreach(var group in new[]{("file.folder.report",(Func<Node,bool>)(n=>n.id=="report")),
     ("file.folder.evidence",(Func<Node,bool>)(n=>n.id!="report" && n.kind!="interview")),
     ("file.folder.interview",(Func<Node,bool>)(n=>n.kind=="interview"))}) {
    var items=sources.Where(group.Item2).ToArray();
    if(items.Length==0)continue;
    KarineUI.CompareGroup(menu,T(group.Item1));
    foreach(var source in items) {
     var choice=source;
     KarineUI.CompareOption(menu,CompareTitle(source),source.id==currentId,()=>{
      if(comparePicker==0)compareLeftId=choice.id;else compareRightId=choice.id;
      comparePicker=-1;ComparePage();
     });
    }
   }
   if(sources.Length==0)Text(menu,T("compare.noSources"),KarineTheme.Secondary,15);
  }
 }
 string CompareTitle(Node node) => node.kind=="interview"?T(node.personNameKey)+" — "+T("file.row.interview"):T(node.titleKey);
 void CompareBody(VisualElement sheet,Node current) {
  var meta=KarineUI.DossierPageHead(sheet,null,current.kind=="interview"?T("file.interviewTitle"):T(current.titleKey),null);
  if(current.kind=="interview")KarineUI.DossierKeyValue(meta,T("file.meta.speaker"),T(current.personNameKey));
  if(current.fileMeta!=null)foreach(var field in current.fileMeta)KarineUI.DossierKeyValue(meta,T(field.labelKey),T(field.valueKey));
  var body=Scroll(sheet);body.style.flexGrow=1;body.style.minHeight=0;
  if(current.kind=="interview") {
   string person=T(current.personNameKey);
   var turns=game.State.interviewTurns.Where(t=>t.nodeId==current.id).ToArray();
   if(turns.Length==0)KarineUI.DossierParagraph(body,T("file.noTranscript"),KarineTheme.Paper.Faded);
   foreach(var turn in turns) {
    bool shown=!string.IsNullOrEmpty(turn.sourceId);
    KarineUI.DossierLine(body,T("interview.bora"),T(turn.promptKey)+(shown?"\n"+T("interview.presented")+" "+ReviewSourceTitle(game.Data,turn.sourceId):""));
    KarineUI.DossierLine(body,person,T(turn.answerKey));
   }
  } else if(current.kind=="cctv") {
   if(current.cctvEvents!=null)foreach(var cctvEvent in current.cctvEvents)KarineUI.DossierParagraph(body,T(cctvEvent.textKey));
  } else MarkableBody(body,current,KarineTheme.Paper.Ink,KarineTheme.Paper.Faded);
 }
}
}
