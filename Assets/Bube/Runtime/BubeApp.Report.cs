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
 void ConclusionStep(int step) {
  if(!game.CanConclude){FilePage();return;}
  step=Mathf.Clamp(step,0,3);
  showingInterviewList=false;
  VisualElement body;
  ReportSheet(T("conclude"),T("conclude.prompt"),out body,null,true);
  var scroll=body as ScrollView;
  var dark=KarineTheme.Paper.Ink;
  var muted=KarineTheme.Paper.Faded;
  // Sayfa sayacı kit'in `03 / 07` biçimi: monospace, iki hane.
  KarineUI.Technical(scroll,(step+1).ToString("00")+" / 04",15).style.color=muted;
  string[] headings={"conclude.suspect","conclude.method","conclude.evidence","conclude.previewTitle"};
  Text(scroll,T(headings[step]),dark,23).style.marginBottom=9;
  Button next=null;
  Action refresh=()=>{
   bool ready=step==0?selectedSuspect!=null && game.ReportSourceAvailable(selectedSuspectSource)
    :step==1?selectedMethod!=null && game.ReportSourceAvailable(selectedMethodSource)
    :step==2?selectedEvidence!=null && game.ReportSourceAvailable(selectedEvidenceSource)
    :selectedSuspect!=null && selectedMethod!=null && selectedEvidence!=null
     && game.ReportSourceAvailable(selectedSuspectSource)
     && game.ReportSourceAvailable(selectedMethodSource)
     && game.ReportSourceAvailable(selectedEvidenceSource);
   if(next!=null){next.SetEnabled(ready);next.style.opacity=ready?1f:.45f;}
  };
  if(step<3) {
   Text(scroll,T("conclude.stepHelp"),muted,15);
   var buttons=new List<Button>();var labels=new List<string>();var ids=new List<string>();
   if(step==0)foreach(var v in game.Data.verdicts) {
    var id=v.id;var label=T(v.labelKey);
    ids.Add(id);labels.Add(label);
    buttons.Add(ReportChoice(scroll,label,selectedSuspect==id,()=>{
     if(selectedSuspect!=id){selectedSuspect=id;selectedSuspectSource=null;ConclusionStep(0);}
    }));
   }
   if(step==1)foreach(var v in game.Data.methods) {
    var id=v.id;var label=T(v.labelKey);
    ids.Add(id);labels.Add(label);
    buttons.Add(ReportChoice(scroll,label,selectedMethod==id,()=>{
     if(selectedMethod!=id){selectedMethod=id;selectedMethodSource=null;ConclusionStep(1);}
    }));
   }
   if(step==2)foreach(var v in game.Data.evidence.Where(v=>game.State.read.Contains(v.id))) {
    var id=v.id;var label=T(v.labelKey);
    ids.Add(id);labels.Add(label);
    buttons.Add(ReportChoice(scroll,label,selectedEvidence==id,()=>{
     if(selectedEvidence!=id){selectedEvidence=id;selectedEvidenceSource=null;ConclusionStep(2);}
    }));
   }
   if(step==0)ReportSourcePicker(scroll,"conclude.suspectSource",()=>selectedSuspectSource,id=>selectedSuspectSource=id,refresh);
   if(step==1)ReportSourcePicker(scroll,"conclude.methodSource",()=>selectedMethodSource,id=>selectedMethodSource=id,refresh);
   if(step==2)ReportSourcePicker(scroll,"conclude.evidenceSource",()=>selectedEvidenceSource,id=>selectedEvidenceSource=id,refresh);
  } else {
   var suspect=game.Data.verdicts.FirstOrDefault(v=>v.id==selectedSuspect);
   var method=game.Data.methods.FirstOrDefault(v=>v.id==selectedMethod);
   var proof=game.Data.evidence.FirstOrDefault(v=>v.id==selectedEvidence);
   Text(scroll,T("conclude.reviewHelp"),muted,15);
   ReportReviewClaim(scroll,"conclude.suspect",suspect==null?"conclude.unselected":suspect.labelKey,selectedSuspectSource);
   ReportReviewClaim(scroll,"conclude.method",method==null?"conclude.unselected":method.labelKey,selectedMethodSource);
   ReportReviewClaim(scroll,"conclude.evidence",proof==null?"conclude.unselected":proof.labelKey,selectedEvidenceSource);
  }
  var nav=new VisualElement();nav.style.flexDirection=FlexDirection.Row;
  nav.style.marginTop=12;nav.style.marginBottom=12;scroll.Add(nav);
  if(step>0) {
   var previous=KarineUI.PaperButton(nav,"‹  "+T("conclude.previous"),()=>ConclusionStep(step-1));
   previous.style.flexGrow=1;previous.style.minHeight=50;previous.style.marginRight=7;
  }
  next=step==3
   ?KarineUI.PaperButton(nav,T("conclude.submit"),ConfirmSubmit,KarinePaperKind.Action)
   :KarineUI.PaperButton(nav,T("conclude.next")+"  ›",()=>ConclusionStep(step+1),KarinePaperKind.Action);
  next.style.flexGrow=1;next.style.minHeight=50;
  refresh();
 }
 // Gönderilen rapor geri alınamaz; kit'in onay modalının var olma sebebi tam
 // olarak budur. Modal yalnız kararı sorar, ne seçileceğini söylemez.
 void ConfirmSubmit() {
  KarineUI.Modal(root,T("conclude.confirm.title"),T("conclude.confirm.body"),
   T("conclude.confirm.cancel"),()=>ConclusionStep(3),
   T("conclude.confirm.send"),Result);
 }

 void ReportReviewClaim(VisualElement parent,string headingKey,string choiceKey,string sourceId) {
  var ink=KarineTheme.Paper.Ink;
  var card=new VisualElement();card.style.backgroundColor=KarineTheme.Paper.Tint;
  card.style.paddingLeft=12;card.style.paddingRight=12;
  card.style.paddingTop=9;card.style.paddingBottom=9;card.style.marginBottom=8;
  parent.Add(card);
  Text(card,T(headingKey)+"  ·  "+T(choiceKey),ink,17).style.marginBottom=5;
  var source=KarineUI.PaperButton(card,T("conclude.openSource")+"  ›  "+CompactReportSourceLabel(sourceId),
   ()=>ShowReportSourceCard(sourceId),KarinePaperKind.Quiet,true);
  source.style.minHeight=50;source.style.fontSize=Typography.Snap(15);
 }
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
 void ShowReportSourceCard(string sourceId) {
  if(!game.ReportSourceAvailable(sourceId))return;
  int separator=sourceId.IndexOf('#');
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==(separator<0?sourceId:sourceId.Substring(0,separator)));
  if(source==null)return;
  var ink=KarineTheme.Paper.Ink;
  var muted=KarineTheme.Paper.Faded;
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.80f);root.Add(shade);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(10);paper.style.right=Length.Percent(10);
  paper.style.top=Length.Percent(9);paper.style.bottom=Length.Percent(9);
  paper.style.paddingLeft=20;paper.style.paddingRight=20;
  paper.style.paddingTop=14;paper.style.paddingBottom=14;
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;shade.Add(paper);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;paper.Add(header);
  var title=Text(header,CompactReportSourceLabel(sourceId),ink,18);
  title.style.flexGrow=1;title.style.whiteSpace=WhiteSpace.Normal;
  KarineUI.CloseButton(header,()=>shade.RemoveFromHierarchy(),null,true);
  var content=Scroll(paper);
  if(source.kind=="cctv" && separator>=0) {
   if(!string.IsNullOrEmpty(source.cctvPeriodKey))Text(content,T(source.cctvPeriodKey),muted,14);
   var record=(source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e=>e.id==sourceId.Substring(separator+1));
   if(record!=null)Text(content,T(record.textKey),ink,18);
  } else if(source.kind=="interview" && separator>=0) {
   var turn=game.InterviewSourceTurn(sourceId);
   if(turn!=null) {
    Text(content,T("interview.bora")+"  ·  "+T(turn.promptKey),muted,15);
    Text(content,T(source.personNameKey)+"  ·  "+T(turn.answerKey),ink,18);
   }
  } else {
   if(source.fileMeta!=null)foreach(var field in source.fileMeta)
    Text(content,T(field.labelKey)+" : "+T(field.valueKey),muted,14);
   Text(content,T(source.bodyKey),ink,17);
  }
 }
 string ReportSourcePreview(Node source) {
  string value;
  if(source.kind=="interview") {
   var turn=game.State.interviewTurns.LastOrDefault(item=>item.nodeId==source.id);
   value=turn==null?string.Empty:T(turn.answerKey);
  } else value=T(source.bodyKey);
  value=value.Replace('\n',' ').Trim();
  return value.Length>120?value.Substring(0,120)+"…":value;
 }
 void ReportSourcePicker(VisualElement parent,string promptKey,Func<string> selected,Action<string> setSelected,Action refresh) {
  var dark=KarineTheme.Paper.Ink;
  var turkish=CultureInfo.GetCultureInfo("tr-TR");
  Func<string,string> normalize=value=>(value ?? "").ToLower(turkish).Replace(':','.');
  Text(parent,T(promptKey),KarineTheme.Paper.Faded,14).style.marginBottom=3;
  var opener=new Button{ text=T("conclude.source")+"  ·  "+CompactReportSourceLabel(selected())+"  ▾" };
  opener.style.minHeight=MinimumTouchTarget;opener.style.marginBottom=5;opener.style.paddingLeft=12;
  opener.style.unityTextAlign=TextAnchor.MiddleLeft;opener.style.fontSize=Typography.Snap(15);
  opener.style.whiteSpace=WhiteSpace.Normal;
  opener.style.color=dark;opener.style.backgroundColor=KarineTheme.Paper.Tint;parent.Add(opener);
  var panel=new VisualElement();panel.style.display=DisplayStyle.None;
  panel.style.marginBottom=10;parent.Add(panel);
  opener.clicked+=()=>panel.style.display=panel.style.display==DisplayStyle.None?DisplayStyle.Flex:DisplayStyle.None;
  var tabs=new VisualElement();tabs.style.flexDirection=FlexDirection.Row;
  tabs.style.marginBottom=5;panel.Add(tabs);
  var searchBar=new VisualElement();searchBar.style.flexDirection=FlexDirection.Row;
  searchBar.style.alignItems=Align.Center;searchBar.style.marginBottom=5;panel.Add(searchBar);
  var search=new TextField(){label=T("conclude.search")};search.style.flexGrow=1;search.style.minWidth=0;
  search.style.height=MinimumTouchTarget;search.style.fontSize=Typography.Snap(18);
  search.style.paddingLeft=8;search.style.color=dark;
  search.style.backgroundColor=KarineTheme.Paper.Light;
  searchBar.Add(search);
  var clear=KarineUI.CloseButton(searchBar,()=>search.value="",null,true);
  clear.style.marginLeft=5;
  var count=Text(panel,"",KarineTheme.Paper.Faded,13);
  count.style.marginBottom=4;
  var choices=new ScrollView();choices.style.maxHeight=210;panel.Add(choices);
  var rows=new List<VisualElement>();
  var categories=new List<int>();
  var searchTexts=new List<string>();
  int[] categoryCounts=new int[4];
  foreach(var source in ComparisonSources()) {
   var item=source;
   int category=item.kind=="cctv"?3:item.kind=="interview"?2:1;
   if(category==3) {
    foreach(var record in item.cctvEvents ?? new CctvEvent[0]) {
     var chosen=record;
     var reference=item.id+"#"+chosen.id;
     var label=T(item.titleKey)+"  ·  "+T(chosen.textKey);
     var option=KarineUI.PaperButton(null,label,()=>{
      setSelected(reference);opener.text=T("conclude.source")+"  ·  "+CompactReportSourceLabel(reference)+"  ▾";
      panel.style.display=DisplayStyle.None;refresh();
     },KarinePaperKind.Choice,true);
     option.style.minHeight=58;option.style.fontSize=Typography.Snap(15);
     option.style.marginBottom=5;choices.Add(option);
     rows.Add(option);categories.Add(category);searchTexts.Add(normalize(label));categoryCounts[category]++;
    }
   } else if(category==2) {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==item.id)) {
     var chosen=turn;
     var reference=game.InterviewTurnReference(chosen);
     var label=T(item.personNameKey)+"  ·  "+T(chosen.promptKey)+"\n"+T(chosen.answerKey);
     var option=KarineUI.PaperButton(null,label,()=>{
      setSelected(reference);opener.text=T("conclude.source")+"  ·  "+CompactReportSourceLabel(reference)+"  ▾";
      panel.style.display=DisplayStyle.None;refresh();
     },KarinePaperKind.Choice,true);
     option.style.minHeight=64;option.style.fontSize=Typography.Snap(15);
     option.style.marginBottom=5;choices.Add(option);
     rows.Add(option);categories.Add(category);searchTexts.Add(normalize(label));categoryCounts[category]++;
    }
   } else {
    var label=T(item.titleKey)+"\n"+ReportSourcePreview(item);
    var option=KarineUI.PaperButton(null,label,()=>{
     setSelected(item.id);opener.text=T("conclude.source")+"  ·  "+T(item.titleKey)+"  ▾";
     panel.style.display=DisplayStyle.None;refresh();
    },KarinePaperKind.Choice,true);
    option.style.minHeight=64;option.style.fontSize=Typography.Snap(15);
    option.style.marginBottom=5;choices.Add(option);
    var fullText=T(item.titleKey)+" "+T(item.bodyKey);
    if(item.fileMeta!=null)foreach(var field in item.fileMeta)
     fullText+=" "+T(field.labelKey)+" "+T(field.valueKey);
    rows.Add(option);categories.Add(category);searchTexts.Add(normalize(fullText));categoryCounts[category]++;
   }
  }
  var empty=Text(choices,T("conclude.noMatches"),dark,15);
  empty.style.display=DisplayStyle.None;
  string[] labels={"conclude.filter.all","conclude.filter.documents","conclude.filter.interviews","conclude.filter.cctv"};
  var tabButtons=new List<Button>();
  int activeFilter=0;
  Action updateFilter=()=>{
   string query=normalize(search.value).Trim();
   int visible=0;
   for(int i=0;i<rows.Count;i++) {
    bool show=(activeFilter==0 || activeFilter==categories[i]) &&
     (query.Length==0 || searchTexts[i].Contains(query));
    rows[i].style.display=show?DisplayStyle.Flex:DisplayStyle.None;
    if(show)visible++;
   }
   empty.style.display=visible==0?DisplayStyle.Flex:DisplayStyle.None;
   count.text=visible+" "+T("conclude.sourceCount");
   for(int i=0;i<tabButtons.Count;i++) {
    tabButtons[i].style.backgroundColor=i==activeFilter?KarineTheme.Paper.Stamp:KarineTheme.Paper.Tint;
    tabButtons[i].style.color=dark;
   }
  };
  for(int i=0;i<labels.Length;i++) {
   int category=i;
   var tab=KarineUI.PaperButton(tabs,T(labels[i]),()=>{activeFilter=category;updateFilter();});
   tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.minWidth=0;
   tab.style.fontSize=Typography.Snap(14);
   tab.style.marginLeft=2;tab.style.marginRight=2;
   tab.SetEnabled(i==0 || categoryCounts[i]>0);
   tabButtons.Add(tab);
  }
  search.RegisterValueChangedCallback(evt=>updateFilter());
  updateFilter();
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
 void CaseSummary() {
  if(!game.State.closed){Desk();return;}
  Desk();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.80f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(9);folder.style.right=Length.Percent(9);
  folder.style.top=Length.Percent(5);folder.style.bottom=Length.Percent(4);
  folder.style.backgroundColor=KarineTheme.Paper.Folder;
  folder.style.borderBottomWidth=8;folder.style.borderBottomColor=KarineTheme.Paper.FolderDeep;root.Add(folder);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(10);paper.style.right=Length.Percent(10);
  paper.style.top=Length.Percent(6);paper.style.bottom=Length.Percent(6);
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=18;paper.style.paddingBottom=16;root.Add(paper);
  var dark=KarineTheme.Paper.Ink;var muted=KarineTheme.Paper.Faded;
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;header.style.alignItems=Align.Center;paper.Add(header);
  var mark=Text(header,"✓",KarineTheme.Paper.Approved,42);mark.style.width=64;mark.style.marginBottom=0;
  var titles=new VisualElement();titles.style.flexGrow=1;header.Add(titles);
  var kicker=Text(titles,T(game.Data.titleKey),muted,15);kicker.style.marginBottom=1;
  var title=Text(titles,T("result.summary"),dark,28);title.style.marginBottom=2;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var subtitle=Text(titles,T("result.status"),dark,14);subtitle.style.marginBottom=0;
  var brand=Text(header,T("summary.brand"),muted,14);brand.style.width=190;brand.style.unityTextAlign=TextAnchor.MiddleRight;
  var line=new VisualElement();line.style.height=1;line.style.marginTop=13;line.style.marginBottom=13;
  line.style.backgroundColor=KarineTheme.Paper.Edge;paper.Add(line);
  bool reviewed=game.Career.reviewHistory.Any(r=>r.caseId==game.Data.id);
  var status=new VisualElement();status.style.flexDirection=FlexDirection.Row;status.style.alignItems=Align.Center;
  status.style.backgroundColor=KarineTheme.GlassLift;
  status.style.paddingLeft=14;status.style.paddingRight=14;status.style.paddingTop=7;status.style.paddingBottom=6;
  status.style.marginBottom=12;paper.Add(status);
  var statusTitle=Text(status,T(reviewed?"summary.reviewReceived":"summary.reviewPending"),Ink,16);
  statusTitle.style.marginBottom=0;statusTitle.style.flexGrow=1;
  if(dossierBoldFont!=null)statusTitle.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var statusHint=Text(status,T(reviewed?"summary.faxAvailable":"summary.faxLater"),Muted,13);
  statusHint.style.marginBottom=0;statusHint.style.unityTextAlign=TextAnchor.MiddleRight;
  var content=Scroll(paper);
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;content.Add(top);
  var photoCard=new VisualElement();photoCard.style.width=Length.Percent(29);
  photoCard.style.paddingRight=16;top.Add(photoCard);
  var report=game.Data.nodes.FirstOrDefault(n=>n.id=="report");
  var art=report==null || string.IsNullOrEmpty(report.imageResource)?null:Resources.Load<Texture2D>(report.imageResource);
  if(art!=null){art.filterMode=FilterMode.Point;var photo=new Image{image=art,scaleMode=ScaleMode.ScaleAndCrop};photo.style.height=170;photoCard.Add(photo);}
  Text(photoCard,T(game.Data.titleKey),dark,16);
  Text(photoCard,T(game.Data.summary.locationKey),muted,14);
  var details=new VisualElement();details.style.flexGrow=1;top.Add(details);
  var band=Text(details,T("summary.report"),dark,17);band.style.backgroundColor=KarineTheme.Paper.Tint;
  SummaryField(details,T("summary.subject"),T(game.Data.titleKey));
  SummaryField(details,T("summary.status"),T("summary.sent"));
  SummaryField(details,T("summary.investigator"),T("summary.bora"));
  var submitted=game.State.submittedAtUtcTicks>0
   ?new DateTime(game.State.submittedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm")
   :T("summary.unknownDate");
  SummaryField(details,T("summary.sentAt"),submitted);
  var suspect=game.Data.verdicts.FirstOrDefault(v=>v.id==game.State.reportSuspect);
  var method=game.Data.methods.FirstOrDefault(v=>v.id==game.State.reportMethod);
  var proof=game.Data.evidence.FirstOrDefault(v=>v.id==game.State.reportProof);
  if(suspect!=null)SummaryField(details,T("conclude.suspect"),T(suspect.labelKey)+" · "+ReviewSourceTitle(game.Data,game.State.reportSuspectSource));
  if(method!=null)SummaryField(details,T("conclude.method"),T(method.labelKey)+" · "+ReviewSourceTitle(game.Data,game.State.reportMethodSource));
  if(proof!=null)SummaryField(details,T("conclude.evidence"),T(proof.labelKey)+" · "+ReviewSourceTitle(game.Data,game.State.reportProofSource));
  var findings=Text(content,T("summary.sources"),dark,17);findings.style.marginTop=12;
  findings.style.backgroundColor=KarineTheme.Paper.Tint;
  var sourceNames=game.Data.nodes.Where(n=>game.State.read.Contains(n.id) && n.id!=game.State.reportProof)
   .Select(n=>T(n.titleKey)).Distinct().Take(4).ToArray();
  foreach(var name in sourceNames)Text(content,"•  "+name,dark,14);
  if(sourceNames.Length==0)Text(content,T("summary.noSources"),muted,14);
  var actions=new VisualElement();actions.style.flexDirection=FlexDirection.Row;actions.style.marginTop=10;paper.Add(actions);
  var back=KarineUI.PaperButton(actions,T("back.desk"),Desk);
  back.style.flexGrow=1;back.style.minHeight=48;
  var next=KarineUI.PaperButton(actions,T("result.continue")+"  →",ContinueToNextCase,KarinePaperKind.Action);
  next.style.flexGrow=1;next.style.minHeight=48;next.style.marginLeft=12;
  FadeIn(paper);
 }
 void Result() {
  if(game.SubmitFinalReport(selectedSuspect,selectedMethod,selectedEvidence,selectedSuspectSource,selectedMethodSource,selectedEvidenceSource)) {
   game.BeginNextCaseReview(7);
   Save();CaseSummary();
  }
 }
 void ContinueToNextCase() {
  if(game.Career.retired){Desk();return;}
  var nextData=AvailableAssignment();
  if(nextData!=null){InboxPage("assignment:"+nextData.id,"all");return;}
  Desk();
  var pending=Panel(root);pending.style.position=Position.Absolute;
  pending.style.left=Length.Percent(34);pending.style.top=Length.Percent(23);
  Text(pending,T("next.pending"),Ink,17);
  Button(pending,T("back.desk"),Desk);
 }
 void OpenAssignment(CaseData nextData) {
  if(nextData==null || nextData.draft || !game.State.closed || game.Data.nextCaseId!=nextData.id)return;
  var nextId=nextData.id;
   Progress progress=null;
   try { if(File.Exists(CaseSavePath(nextId))) progress=JsonUtility.FromJson<Progress>(File.ReadAllText(CaseSavePath(nextId))); }
   catch(Exception e) { Debug.LogWarning("Next case save could not be loaded: "+e.Message); }
   game=new Investigation(nextData,progress,game.Career,careerRules){Text=locale};
   game.Career.activeCaseId=nextId;
   game.BeginNextCaseReview(7);
   selectedSuspect=selectedMethod=selectedEvidence=null;
   selectedSuspectSource=selectedMethodSource=selectedEvidenceSource=null;
   Save();
   if(game.State.caseAccepted)Desk();else MaybeWorldIntro(Desk);
 }
 void FaxPage() {
  if(!HasIncomingFax){Desk();return;}
  var fax=game.DeliverNextFax();
  if(fax==null){Desk();return;}
  Save();
  VisualElement body;
  ReportSheet(T("inbox.faxTitle"),T("inbox.faxPending"),out body,Desk);
  var dark=KarineTheme.Paper.Ink;
  Text(body,T("career.evaluation."+fax.evaluationType),dark,20);
  var reviewedAsset=Resources.Load<TextAsset>("Bube/Cases/"+fax.caseId);
  var reviewed=reviewedAsset==null?null:JsonUtility.FromJson<CaseData>(reviewedAsset.text);
  if(reviewed!=null) {
   Text(body,T("fax.reviewHeading"),dark,18);
   var person=reviewed.verdicts.FirstOrDefault(v=>v.id==fax.suspectId);
   var method=reviewed.methods.FirstOrDefault(v=>v.id==fax.methodId);
   var proof=reviewed.evidence.FirstOrDefault(v=>v.id==fax.proofId);
   if(person!=null)SummaryField(body,T("conclude.suspect"),T(person.labelKey)+" · "+T(fax.suspectSupported?"fax.supported":"fax.unsupported"));
   if(method!=null)SummaryField(body,T("conclude.method"),T(method.labelKey)+" · "+T(fax.methodSupported?"fax.supported":"fax.unsupported"));
   if(proof!=null)SummaryField(body,T("conclude.evidence"),T(proof.labelKey)+" · "+T(fax.proofSupported?"fax.supported":"fax.unsupported"));
  }
  Text(body,T("career.trust")+"  "+T(game.TrustStatusKey)+(fax.trustChange>0?" ↑":fax.trustChange<0?" ↓":""),dark,17);
  Button(body,T("career.openRecord"),StatisticsPage);
  if(game.Career.retired)Text(body,T("career.ended"),KarineTheme.Danger,18);
  else if(game.State.closed)Button(body,T("result.continue"),ContinueToNextCase,true);
 }
}
}
