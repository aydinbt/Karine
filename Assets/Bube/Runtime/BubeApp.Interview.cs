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
// Görüşme ekranı: sorular, kaynak öne sürme, portre.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 void InterviewPage(Node node,Question active=null,int phase=0,string answerKey=null,string sourceId=null,bool sourceAccepted=true) {
  if(selectedInterviewNodeId!=node.id){selectedInterviewNodeId=node.id;selectedInterviewTopic=null;showingInterviewHistory=false;}
  if(active!=null && !string.IsNullOrEmpty(active.topicKey))selectedInterviewTopic=active.topicKey;
  bool enteringRoom=SceneManager.GetActiveScene().name!="InterviewScene";
  if(enteringRoom)showingInterviewHistory=false;
  EnsureScene("InterviewScene");
  showingInterviewList=false;
  var availableOptions=(node.questions ?? new Question[0]).Where(q=>game.CanAskQuestion(node,q)).ToArray();
  root.Clear();
  var room=Resources.Load<Texture2D>("Bube/InterviewRoom");
  if(room!=null) {
   room.filterMode=FilterMode.Point;
   var background=new Image {image=room,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   background.style.position=Position.Absolute;
   background.style.left=0;background.style.right=0;background.style.top=0;background.style.bottom=0;
   root.Add(background);
  }
  var top=new VisualElement();top.style.position=Position.Absolute;
  top.style.left=0;top.style.right=0;top.style.top=0;top.style.height=64;
  top.style.backgroundColor=new Color(.055f,.075f,.09f,.98f);
  top.style.paddingLeft=26;top.style.paddingTop=12;root.Add(top);
  Text(top,"bube POLİS  /  "+T("kind.interview")+"  /  "+T(game.Data.titleKey),Ink,17);
  var identity=new VisualElement();identity.style.position=Position.Absolute;
  identity.style.left=Length.Percent(2);identity.style.top=Length.Percent(17);
  identity.style.width=Length.Percent(23);identity.style.backgroundColor=new Color(.055f,.075f,.09f,.94f);
  identity.style.paddingLeft=16;identity.style.paddingRight=12;identity.style.paddingTop=14;
  root.Add(identity);
  Text(identity,T("interview.identity"),Gold,14);
  Text(identity,T(node.personNameKey).ToUpperInvariant(),Ink,20);
  Text(identity,T(node.personInfoKey),Muted,15);
  PixelPortrait(root,node.personId);
  var dialogue=new VisualElement();dialogue.style.position=Position.Absolute;
  dialogue.style.left=Length.Percent(62);dialogue.style.right=Length.Percent(2);
  dialogue.style.top=Length.Percent(16);dialogue.style.height=Length.Percent(19);
  dialogue.style.backgroundColor=new Color(.055f,.075f,.09f,.95f);
  dialogue.style.paddingLeft=18;dialogue.style.paddingRight=16;dialogue.style.paddingTop=13;
  dialogue.style.overflow=Overflow.Hidden;
  root.Add(dialogue);
  Text(dialogue,phase==1?T("interview.bora"):T(node.personNameKey).ToUpperInvariant(),Gold,15);
  var spoken=phase==1?T(active.promptKey):phase==2?T(answerKey):T(availableOptions.Length==0?"interview.noNewInfo":"interview.opening");
  var dialogueScroll=new ScrollView(ScrollViewMode.Vertical);
  dialogueScroll.style.position=Position.Absolute;
  dialogueScroll.style.left=18;dialogueScroll.style.right=12;
  dialogueScroll.style.top=38;dialogueScroll.style.bottom=8;
  dialogueScroll.verticalScrollerVisibility=ScrollerVisibility.Auto;
  dialogue.Add(dialogueScroll);
  var speech=Text(dialogueScroll,spoken,Ink,17);
  speech.style.whiteSpace=WhiteSpace.Normal;
  if(phase==2)Typewriter(speech,spoken);
  // Dedektifin gördüğü davranış — yorum değil, gözlem. Yalan ya da çelişki
  // etiketi değildir; anlamını oyuncu kurar. Metni olmayan yanıtta satır yoktur.
  if(phase==2 && locale.Has(answerKey+".demeanor")) {
   var demeanor=Text(dialogueScroll,T(answerKey+".demeanor"),Muted,15);
   demeanor.style.whiteSpace=WhiteSpace.Normal;demeanor.style.marginTop=10;
   demeanor.style.unityFontStyleAndWeight=FontStyle.Italic;
  }
  var referenceCard=phase>0 && game.ReportSourceAvailable(sourceId)?InterviewReferenceCard(sourceId):null;
  var topics=availableOptions.GroupBy(q=>string.IsNullOrEmpty(q.topicKey)?"interview.topic.other":q.topicKey).ToArray();
  var initiallyOpen=topics.Any(g=>g.Key==selectedInterviewTopic)?selectedInterviewTopic:topics.FirstOrDefault()?.Key;
  var questionArea=new VisualElement();questionArea.style.position=Position.Absolute;
  questionArea.style.left=Length.Percent(62);questionArea.style.right=Length.Percent(2);
  questionArea.style.top=Length.Percent(38);questionArea.style.bottom=Length.Percent(19);
  questionArea.style.flexDirection=FlexDirection.Column;
  root.Add(questionArea);
  var turns=game.State.interviewTurns.Where(turn=>turn.nodeId==node.id).ToArray();
  if(turns.Length==0)showingInterviewHistory=false;
  VisualElement historyTabs=null;
  if(turns.Length>0) {
   historyTabs=new VisualElement();historyTabs.style.flexDirection=FlexDirection.Row;
   historyTabs.style.flexShrink=0;historyTabs.style.minHeight=MinimumTouchTarget+4;
   questionArea.Add(historyTabs);
  }
  var questions=new ScrollView();questions.style.flexGrow=1;questions.style.minHeight=0;
  questionArea.Add(questions);
  if(phase==0) {
   for(int groupIndex=0;groupIndex<topics.Length;groupIndex++) {
    var topic=topics[groupIndex];
    var section=new VisualElement();questions.Add(section);
    var choices=new VisualElement();section.Add(choices);
    if(topics.Length>1) {
     choices.style.display=topic.Key==initiallyOpen?DisplayStyle.Flex:DisplayStyle.None;
     var topicKey=topic.Key;
     var header=new Button(()=>{
      choices.style.display=choices.style.display==DisplayStyle.None?DisplayStyle.Flex:DisplayStyle.None;
      selectedInterviewTopic=topicKey;
     })
      {text=T(topic.Key)+"  ·  "+topic.Count()};
     header.style.minHeight=MinimumTouchTarget;header.style.marginBottom=6;header.style.paddingLeft=12;
     header.style.unityTextAlign=TextAnchor.MiddleLeft;header.style.fontSize=Typography.Snap(16);
     header.style.color=Ink;header.style.backgroundColor=KarineTheme.Panel2;
     section.Insert(0,header);
    }
    foreach(var q in topic) {
     var question=q;
     Button(choices,"›  "+T(q.promptKey),()=>InterviewPage(node,question,1));
     var choiceButton=choices.Children().Last() as Button;
     choiceButton.style.whiteSpace=WhiteSpace.Normal;
     choiceButton.style.fontSize=Typography.Snap(16);
     choiceButton.style.minHeight=66;
     choiceButton.style.backgroundColor=new Color(.09f,.13f,.14f);
     choiceButton.style.borderLeftWidth=3;
     choiceButton.style.borderLeftColor=KarineTheme.Active;
    }
   }
   if(availableOptions.Length==0)Text(questions,T("interview.noNewInfo"),Muted,16);
  } else if(phase==1) {
   if(game.QuestionNeedsSource(active)) {
    InterviewSourcePicker(questions,node,active,sourceId,referenceCard);
   } else {
    Button(questions,T("interview.listen"),()=>{
     var reply=game.AnswerKey(active);
     if(game.Ask(node.id,active.id)){Save();InterviewPage(node,active,2,reply);}
    },true);
    // Soruyu seçtikten sonra da vazgeçebilmeli; tek çıkış görüşmeyi bitirmek olmamalı.
    Button(questions,T("interview.cancelSource"),()=>InterviewPage(node));
    var giveUp=questions.Children().Last() as Button;
    giveUp.style.minHeight=MinimumTouchTarget;giveUp.style.fontSize=Typography.Snap(15);
    giveUp.style.backgroundColor=KarineTheme.Panel2;giveUp.style.color=Ink;
   }
  } else {
   if(referenceCard!=null)Button(questions,T("interview.openPresented"),()=>referenceCard.style.display=DisplayStyle.Flex);
   Button(questions,T(sourceAccepted?"interview.next":"interview.tryAnotherSource"),
    ()=>InterviewPage(node,sourceAccepted?null:active,sourceAccepted?0:1),true);
  }
  if(turns.Length>0) {
   var history=new ScrollView();history.style.flexGrow=1;history.style.minHeight=0;questionArea.Add(history);
   for(int i=0;i<turns.Length;i++) {
    var turn=turns[i];
    var card=new VisualElement();card.style.paddingLeft=12;card.style.paddingRight=12;card.style.paddingTop=9;
    card.style.marginBottom=7;card.style.backgroundColor=KarineTheme.Panel;
    history.Add(card);
    var number=KarineUI.Technical(card,(i+1).ToString("00")+"  ·  "+T("interview.bora"),13);number.style.marginBottom=3;
    var prompt=Text(card,T(turn.promptKey),Ink,15);prompt.style.marginBottom=8;
    var speaker=Text(card,T(node.personNameKey),Muted,13);speaker.style.marginBottom=3;
    var reply=Text(card,T(turn.answerKey),Ink,16);reply.style.marginBottom=11;
   }
   // Soru/geçmiş ikilisi kit'in sekme şeridi. Sekme değişimi sayfayı yeniden
   // kurmaz, yalnız görünürlüğü değiştirir; şerit seçili sekmeyi göstermek için
   // yeniden çizilir.
   var labels=new[]{T("interview.questions"),T("interview.history")+"  ·  "+turns.Length};
   Action<bool> switchView=null;
   switchView=showHistory=>{
    showingInterviewHistory=showHistory;
    questions.style.display=showHistory?DisplayStyle.None:DisplayStyle.Flex;
    history.style.display=showHistory?DisplayStyle.Flex:DisplayStyle.None;
    historyTabs.Clear();
    KarineUI.Tabs(historyTabs,labels,showHistory?1:0,picked=>switchView(picked==1),true);
   };
   switchView(showingInterviewHistory);
  }
  var back=new VisualElement();back.style.position=Position.Absolute;
  back.style.left=Length.Percent(62);back.style.right=Length.Percent(2);
  back.style.bottom=Length.Percent(5);root.Add(back);
  Button(back,T("interview.back"),Desk);
  if(enteringRoom)FadeIn(root);
 }
 string ShortInterviewSourceLabel(string value) {
  value=(value ?? "").Replace('\n',' ').Trim();
  // Satırlar iki satıra sarıyor; 66 karakter yanıtın anlamlı yerini kesiyordu.
  return value.Length<=110?value:value.Substring(0,109).TrimEnd()+"…";
 }
 void InterviewSourcePicker(ScrollView questions,Node node,Question active,string sourceId,VisualElement referenceCard) {
  if(interviewSourceQuestionId!=node.id+"/"+active.id) {
   interviewSourceQuestionId=node.id+"/"+active.id;interviewSourceFilter=0;
  }
  Text(questions,T("interview.chooseSource"),Gold,16);
  // Kaynak sunmaktan vazgeçmenin tek yolu görüşmeyi tümden bitirmekti.
  Button(questions,T("interview.cancelSource"),()=>InterviewPage(node));
  var cancel=questions.Children().Last() as Button;
  cancel.style.minHeight=MinimumTouchTarget;cancel.style.fontSize=Typography.Snap(15);
  cancel.style.backgroundColor=KarineTheme.Panel2;cancel.style.color=Ink;
  if(!string.IsNullOrEmpty(sourceId)) {
   var chosen=Text(questions,T("interview.selectedSource")+"  ·  "+ShortInterviewSourceLabel(CompactReportSourceLabel(sourceId)),Ink,15);
   chosen.style.whiteSpace=WhiteSpace.Normal;
   if(referenceCard!=null) {
    Button(questions,T("interview.openReference"),()=>referenceCard.style.display=DisplayStyle.Flex);
    var open=questions.Children().Last() as Button;
    open.style.minHeight=MinimumTouchTarget;
   }
   Button(questions,T("interview.presentSource"),()=>{
    var decoy=game.DecoyAnswerKey(active,sourceId);
    if(decoy!=null){InterviewPage(node,active,2,decoy,sourceId,false);return;}
    var reply=game.AnswerKey(active,sourceId);
    if(game.Ask(node.id,active.id,sourceId)){Save();InterviewPage(node,active,2,reply,sourceId);}
    // `answerKey` bir anahtardır; çevrilmiş metin geçilirse ekrana "[...]" düşer.
    // Yemi yazılmamış kaynak: genel "ne diyeyim" yerine kişinin kendi savuşturması.
    else InterviewPage(node,active,2,node.deflectAnswerKey ?? "interview.unrelatedSource",sourceId,false);
   },true);
   var present=questions.Children().Last() as Button;
   present.style.minHeight=MinimumTouchTarget;
  }
  var controls=new VisualElement();questions.Add(controls);
  // Sonuç ekranında her kaynak gösterilebilir, ama görüşmede öne sürülmesi
  // anlamsız olanlar (vakanın kendi raporu, sinyal telemetrisi) listeyi
  // kalabalıklaştırmaktan başka bir iş görmüyordu.
  var sources=ComparisonSources().Where(n=>(n.kind!="interview" || n.personId!=node.personId) && !n.notPresentable).ToArray();
  var rows=new List<VisualElement>();var categories=new List<int>();
  int[] categoryCounts=new int[4];
  foreach(var source in sources) {
   var item=source;
   int category=item.kind=="cctv"?3:item.kind=="interview"?2:1;
   if(category==3) {
    foreach(var record in item.cctvEvents ?? new CctvEvent[0]) {
     var eventItem=record;var reference=item.id+"#"+eventItem.id;
     if(eventItem.notPresentable)continue;
     if(!game.SourceConcernsPerson(node,eventItem.aboutPersonIds,T(eventItem.textKey)))continue;
     if(game.SourceAlreadyPresented(node,active,reference))continue;
     Button(questions,ShortInterviewSourceLabel(T(item.titleKey)+" · "+T(eventItem.textKey)),()=>InterviewPage(node,active,1,null,reference),reference==sourceId);
     var row=questions.Children().Last();
     rows.Add(row);categories.Add(category);categoryCounts[category]++;
    }
   } else if(category==2) {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==item.id)) {
     var answer=turn;var reference=game.InterviewTurnReference(answer);
     if(!game.SourceConcernsPerson(node,game.FindQuestion(item,answer.questionId)?.aboutPersonIds,
      T(answer.promptKey)+" "+T(answer.answerKey)))continue;
     if(game.SourceAlreadyPresented(node,active,reference))continue;
     // Satırda sorunun metni yazıyordu; liste "soracağım sorular" gibi okunuyordu.
     // Oysa öne sürülen şey kişinin **verdiği yanıttır**, tırnak içinde gösterilir.
     Button(questions,ShortInterviewSourceLabel(T(item.personNameKey)+" · \u201c"+T(answer.answerKey)+"\u201d"),()=>InterviewPage(node,active,1,null,reference),reference==sourceId);
     var row=questions.Children().Last();
     rows.Add(row);categories.Add(category);categoryCounts[category]++;
    }
   } else if(!game.SourceAlreadyPresented(node,active,item.id) &&
    game.SourceConcernsPerson(node,item.aboutPersonIds,T(item.titleKey)+" "+T(item.bodyKey))) {
    Button(questions,ShortInterviewSourceLabel(T(item.titleKey)),()=>InterviewPage(node,active,1,null,item.id),item.id==sourceId);
    var row=questions.Children().Last();
    rows.Add(row);categories.Add(category);categoryCounts[category]++;
   }
  }
  if(rows.Count==0) {Text(questions,T("interview.noSource"),Muted,15);return;}
  // Telefonda arama alanı yoktu sayılır: klavye ekranın yarısını kaplıyor, liste
  // zaten kişiye göre süzülüp 9-10 satıra indi. Yerine tür sekmeleri kalıyor.
  var tabs=new VisualElement();controls.Add(tabs);
  var count=Text(controls,"",Muted,13);
  var empty=Text(questions,T("conclude.noMatches"),Muted,15);empty.style.display=DisplayStyle.None;
  string[] labels={"conclude.filter.all","conclude.filter.documents","conclude.filter.interviews","conclude.filter.cctv"};
  // Tür süzgeci kit'in sekme şeridi. Dar tablet için iki satıra bölünür;
  // kayıt türü olmayan sekme kapalı kalır (kit'in DISABLED durumu).
  Action update=null;
  update=()=>{
   int visible=0;
   for(int i=0;i<rows.Count;i++) {
    bool show=interviewSourceFilter==0 || interviewSourceFilter==categories[i];
    rows[i].style.display=show?DisplayStyle.Flex:DisplayStyle.None;if(show)visible++;
   }
   count.text=visible+" "+T("conclude.sourceCount");
   empty.style.display=visible==0?DisplayStyle.Flex:DisplayStyle.None;
   tabs.Clear();
   for(int half=0;half<2;half++) {
    int offset=half*2;
    var strip=KarineUI.Tabs(tabs,new[]{T(labels[offset]),T(labels[offset+1])},
     interviewSourceFilter-offset,picked=>{interviewSourceFilter=offset+picked;update();},true);
    strip.style.marginBottom=3;
    for(int i=0;i<2;i++)strip[i].SetEnabled(offset+i==0 || categoryCounts[offset+i]>0);
   }
  };
  foreach(var row in rows) {row.style.minHeight=MinimumTouchTarget;row.style.whiteSpace=WhiteSpace.Normal;row.style.fontSize=Typography.Snap(15);}
  update();
 }
 VisualElement InterviewReferenceCard(string sourceId) {
  int separator=sourceId.IndexOf('#');
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==(separator<0?sourceId:sourceId.Substring(0,separator)));
  if(source==null)return null;
  var ink=KarineTheme.Paper.Ink;
  var muted=new Color(.37f,.36f,.33f);
  var card=new VisualElement();card.style.position=Position.Absolute;
  card.style.left=Length.Percent(2);card.style.width=Length.Percent(27);
  card.style.top=Length.Percent(45);card.style.bottom=Length.Percent(15);
  card.style.paddingLeft=16;card.style.paddingRight=15;card.style.paddingTop=10;card.style.paddingBottom=11;
  card.style.backgroundColor=KarineTheme.Paper.Sheet;
  card.style.borderLeftWidth=3;card.style.borderTopWidth=2;
  card.style.borderLeftColor=KarineTheme.Paper.Stamp;card.style.borderTopColor=KarineTheme.Paper.Light;
  root.Add(card);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;card.Add(header);
  var title=Text(header,T("interview.referenceCard"),muted,13);
  title.style.flexGrow=1;title.style.marginBottom=0;
  var close=new Button(()=>card.style.display=DisplayStyle.None){text="×"};
  close.style.width=MinimumTouchTarget;close.style.height=MinimumTouchTarget;close.style.fontSize=Typography.Snap(22);
  close.style.color=ink;close.style.backgroundColor=KarineTheme.Paper.Tint;header.Add(close);
  var name=Text(card,T(source.titleKey),ink,17);name.style.marginBottom=8;
  if(dossierBoldFont!=null)name.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var body=Scroll(card);
  if(source.kind=="cctv" && separator>=0) {
   if(!string.IsNullOrEmpty(source.cctvPeriodKey))Text(body,T(source.cctvPeriodKey),muted,12);
   var record=(source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e=>e.id==sourceId.Substring(separator+1));
   if(record!=null)Text(body,T(record.textKey),ink,16);
  } else if(source.kind=="interview" && separator>=0) {
   var turn=game.InterviewSourceTurn(sourceId);
   if(turn!=null) {
    Text(body,T(turn.promptKey),muted,13);
    Text(body,T(turn.answerKey),ink,16);
   }
  } else {
   if(source.fileMeta!=null)foreach(var field in source.fileMeta)
    Text(body,T(field.labelKey)+" : "+T(field.valueKey),muted,12);
   Text(body,T(source.bodyKey),ink,15);
  }
  return card;
 }
 void PixelPortrait(VisualElement parent,string personId) {
  var holder=new VisualElement();holder.style.position=Position.Absolute;
  holder.style.left=Length.Percent(31);holder.style.top=Length.Percent(23);
  holder.style.width=Length.Percent(27);holder.style.height=Length.Percent(51);
  parent.Add(holder);
  var portrait=Resources.Load<Texture2D>("Bube/Characters/"+personId);
  if(portrait!=null) {
   portrait.filterMode=FilterMode.Point;
   var art=new Image {image=portrait,scaleMode=ScaleMode.ScaleToFit};
   art.style.width=Length.Percent(100);art.style.height=Length.Percent(100);
   holder.Add(art);
   return;
  }
  string[] pixels={
   "....................","......hhhhhhhh......",".....hhhhhhhhhh.....","....hhhhhhhhhhhh....",
   "....hhhsssssshhh....","....hhsssssssshh....","....hssssssssssh....","....hssessssessh....",
   "....hssssnsssssh....","....hssssssssssh....","....hssssmsssssh....","....hhssssssssh.....",
   ".....hhssssssh......","......hsssssh.......",".......sssss........","......ttssstt.......",
   "....tttttttttttt....","...tttttttttttttt...","..tttttttttttttttt..",".tttttttttttttttttt."
  };
  var hair=personId=="hasan"?new Color(.38f,.36f,.33f):personId=="mert"?new Color(.16f,.13f,.12f):new Color(.18f,.13f,.12f);
  var skin=personId=="hasan"?new Color(.64f,.46f,.34f):new Color(.68f,.47f,.35f);
  var shirt=personId=="mert"?new Color(.30f,.37f,.39f):personId=="hasan"?new Color(.31f,.29f,.25f):new Color(.12f,.14f,.15f);
  for(int y=0;y<pixels.Length;y++)for(int x=0;x<pixels[y].Length;x++) {
   char p=pixels[y][x];
   if(personId!="elif" && y>4 && p=='h')p='.';
   if(personId=="hasan" && y==10 && x>=8 && x<=11)p='h';
   if(p=='.')continue;
   var cell=new VisualElement();cell.style.position=Position.Absolute;
   cell.style.left=Length.Percent(x*5);cell.style.top=Length.Percent(y*5);
   cell.style.width=Length.Percent(5);cell.style.height=Length.Percent(5);
   cell.style.backgroundColor=p=='h'?hair:p=='t'?shirt:p=='e'||p=='m'||p=='n'?new Color(.12f,.12f,.12f):skin;
   holder.Add(cell);
  }
 }
}
}
