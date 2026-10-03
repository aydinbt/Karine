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
  KarineUI.InterviewRoom(root,root.childCount,game.Data.deskHour);KarineUI.Fluorescent(root);KarineUI.MirrorSheen(root);
  PixelPortrait(root,node.personId);
  // Üst şerit diğer tam ekranlarla aynı: masaya dön, "Görüşme", kişi ve dosya, ayarlar.
  KarineUI.DossierBar(root,T("back.desk"),T("kind.interview"),T(node.personNameKey)+"  ·  "+T(game.Data.titleKey),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(()=>InterviewPage(node)));
  KarineUI.InterviewIdentity(root,Resources.Load<Texture2D>("Bube/Characters/"+node.personId),T("interview.identity"),T(node.personNameKey),T(node.personInfoKey));
  // Kayıt öne sürülürken balon ortaya kayar, sağı kayıt paneli alır.
  bool presenting=phase==1 && game.QuestionNeedsSource(active);
  var dialogueScroll=KarineUI.InterviewBubble(root,presenting,phase==1?T("interview.bora"):T(node.personNameKey),out var speech);
  var spoken=phase==1?T(active.promptKey):phase==2?T(answerKey):T(availableOptions.Length==0?"interview.noNewInfo":"interview.opening");
  speech.text=spoken;
  if(phase==2)audioDirector?.Voice(answerKey);else audioDirector?.StopVoice();
  // Karşındaki konuşuyor: daktilo değil **ses**. Kelime yok (kelime olursa
  // Türkçe metnin üstüne yabancı bir dil biner), yalnız sesin gövdesi; perde
  // kişiden gelir, yani üç kişi üç ses olur.
  // Yanıttan önce kısa bir duraksama — her yanıtta, her kişide aynı süre;
  // süresi içerikten bilgi taşımaz.
  if(phase==2) {
   if(instantText || KarineMotion.Reduced)Typewriter(speech,spoken,AudioDirector.Chat,1f,0.55f,5);
   else {
    speech.text=string.Empty;KarineUI.Ellipsis(speech,KarineTheme.Effects.AnswerPauseMs);
    speech.schedule.Execute(()=>Typewriter(speech,spoken,AudioDirector.Chat,1f,0.55f,5))
     .StartingIn(KarineTheme.Effects.AnswerPauseMs);
   }
  }
  // Dedektifin gördüğü davranış — yorum değil, gözlem. Yalan ya da çelişki
  // etiketi değildir; anlamını oyuncu kurar. Metni olmayan yanıtta satır yoktur.
  if(phase==2 && locale.Has(answerKey+".demeanor"))KarineUI.InterviewDemeanor(dialogueScroll,T(answerKey+".demeanor"));
  if(presenting){InterviewSourcePicker(node,active,sourceId);Arrive(enteringRoom);return;}
  var referenceCard=phase>0 && game.ReportSourceAvailable(sourceId)?InterviewReferenceCard(sourceId):null;
  var topics=availableOptions.GroupBy(q=>string.IsNullOrEmpty(q.topicKey)?"interview.topic.other":q.topicKey).ToArray();
  var initiallyOpen=topics.Any(g=>g.Key==selectedInterviewTopic)?selectedInterviewTopic:topics.FirstOrDefault()?.Key;
  var panel=KarineUI.InterviewPanel(root,out var historyTabs);
  var turns=game.State.interviewTurns.Where(turn=>turn.nodeId==node.id).ToArray();
  if(turns.Length==0)showingInterviewHistory=false;
  var questions=new KarineScrollView();questions.style.flexGrow=1;questions.style.minHeight=0;
  panel.Add(questions);
  if(phase==0) {
   for(int groupIndex=0;groupIndex<topics.Length;groupIndex++) {
    var topic=topics[groupIndex];
    var section=new VisualElement();questions.Add(section);
    var choices=new VisualElement();
    if(topics.Length>1) {
     var topicKey=topic.Key;bool open=topic.Key==initiallyOpen;
     choices.style.display=open?DisplayStyle.Flex:DisplayStyle.None;
     Button header=null;
     Action toggle=()=>{
      bool show=choices.style.display==DisplayStyle.None;
      choices.style.display=show?DisplayStyle.Flex:DisplayStyle.None;
      selectedInterviewTopic=topicKey;
      var fresh=KarineUI.InterviewTopic(null,T(topicKey)+"  ·  "+topic.Count(),show,null);
      header.Clear();foreach(var child in fresh.Children().ToArray())header.Add(child);
      header.style.borderLeftColor=fresh.style.borderLeftColor;
     };
     header=KarineUI.InterviewTopic(section,T(topic.Key)+"  ·  "+topic.Count(),open,()=>toggle());
    }
    section.Add(choices);
    foreach(var q in topic) {
     var question=q;
     KarineUI.InterviewQuestion(choices,T(q.promptKey),()=>InterviewPage(node,question,1));
    }
   }
   if(availableOptions.Length==0)Text(questions,T("interview.noNewInfo"),Muted,16);
  } else if(phase==1) {
   KarineUI.InterviewAction(questions,null,T("interview.listen"),()=>{
    var reply=game.AnswerKey(active);
    if(game.Ask(node.id,active.id)){Save();InterviewPage(node,active,2,reply);}
   });
   // Soruyu seçtikten sonra da vazgeçebilmeli; tek çıkış görüşmeyi bitirmek olmamalı.
   KarineUI.InterviewQuestion(questions,T("interview.cancelSource"),()=>InterviewPage(node)).style.marginTop=KarineTheme.SpaceMd;
  } else {
   if(referenceCard!=null)KarineUI.InterviewQuestion(questions,T("interview.openPresented"),()=>referenceCard.style.display=DisplayStyle.Flex);
   KarineUI.InterviewAction(questions,null,T(sourceAccepted?"interview.next":"interview.tryAnotherSource"),
    ()=>InterviewPage(node,sourceAccepted?null:active,sourceAccepted?0:1));
  }
  // Sorular/geçmiş sekmeleri her zaman görünür; geçmiş boşken kapalıdır.
  var history=new KarineScrollView();history.style.flexGrow=1;history.style.minHeight=0;panel.Add(history);
  for(int i=0;i<turns.Length;i++) {
   var turn=turns[i];
   var card=new VisualElement();card.style.paddingLeft=12;card.style.paddingRight=12;card.style.paddingTop=9;
   card.style.marginBottom=7;card.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Panel,.6f);
   card.style.borderLeftWidth=KarineTheme.Interview.Edge-1;card.style.borderLeftColor=KarineTheme.Border;
   history.Add(card);
   var number=KarineUI.Technical(card,(i+1).ToString("00")+"  ·  "+T("interview.bora"),13);number.style.marginBottom=3;
   var prompt=Text(card,T(turn.promptKey),Ink,15);prompt.style.marginBottom=8;
   var speaker=Text(card,T(node.personNameKey),Gold,13);speaker.style.marginBottom=3;
   var reply=Text(card,T(turn.answerKey),Ink,16);reply.style.marginBottom=11;
  }
  Action<bool> switchView=null;
  switchView=showHistory=>{
   showingInterviewHistory=showHistory;
   questions.style.display=showHistory?DisplayStyle.None:DisplayStyle.Flex;
   history.style.display=showHistory?DisplayStyle.Flex:DisplayStyle.None;
   historyTabs.Clear();
   KarineUI.InterviewTab(historyTabs,T("interview.questions"),!showHistory,()=>switchView(false));
   var past=KarineUI.InterviewTab(historyTabs,T("interview.history")+(turns.Length>0?"  ·  "+turns.Length:""),showHistory,()=>switchView(true));
   past.SetEnabled(turns.Length>0);
  };
  switchView(showingInterviewHistory);
  Arrive(enteringRoom);
 }
 void Arrive(bool enteringRoom) {
  if(enteringRoom){KarineUI.InterviewStarted();FadeIn(root);KarineUI.CutIn(root,KarineTheme.Scene.RingHold);}
 }
 string ShortInterviewSourceLabel(string value) {
  value=(value ?? "").Replace('\n',' ').Trim();
  // Satırlar iki satıra sarıyor; 66 karakter yanıtın anlamlı yerini kesiyordu.
  return value.Length<=110?value:value.Substring(0,109).TrimEnd()+"…";
 }
 // Kaydı öne sür: solda süzgeç ve kaynaklar, sağda seçilen kaydın kâğıdı ve "Öne sür".
 void InterviewSourcePicker(Node node,Question active,string sourceId) {
  if(interviewSourceQuestionId!=node.id+"/"+active.id) {
   interviewSourceQuestionId=node.id+"/"+active.id;interviewSourceFilter=0;
  }
  KarineUI.InterviewPresent(root,T("interview.presentSource"),T(active.promptKey),()=>InterviewPage(node),T("interview.cancelSource"),out var column,out var preview);
  var controls=new VisualElement();controls.style.flexShrink=0;column.Add(controls);
  var questions=new KarineScrollView();questions.style.flexGrow=1;questions.style.minHeight=0;column.Add(questions);
  // Sonuç ekranında her kaynak gösterilebilir, ama görüşmede öne sürülmesi
  // anlamsız olanlar (vakanın kendi raporu, sinyal telemetrisi) listeyi
  // kalabalıklaştırmaktan başka bir iş görmüyordu.
  var sources=ComparisonSources().Where(n=>(n.kind!="interview" || n.personId!=node.personId) && !n.notPresentable).ToArray();
  var rows=new List<VisualElement>();var categories=new List<int>();
  int[] categoryCounts=new int[4];
  Action<string,string,string,string,int> add=(icon,title,sub,reference,category)=>{
   rows.Add(KarineUI.InterviewSource(questions,icon,ShortInterviewSourceLabel(title),sub==null?null:ShortInterviewSourceLabel(sub),
    reference==sourceId,()=>InterviewPage(node,active,1,null,reference)));
   categories.Add(category);categoryCounts[category]++;
  };
  foreach(var source in sources) {
   var item=source;
   int category=item.kind=="cctv"?3:item.kind=="interview"?2:1;
   if(category==3) {
    foreach(var record in item.cctvEvents ?? new CctvEvent[0]) {
     var reference=item.id+"#"+record.id;
     if(record.notPresentable)continue;
     if(!game.SourceConcernsPerson(node,record.aboutPersonIds,T(record.textKey)))continue;
     if(game.SourceTried(node,active,reference))continue;
     // Aynı kameranın satırları tek tek ayırt edilsin: başlık kaydın kendisi, alt satır kamera.
     add("cctv",T(record.textKey),T(item.titleKey),reference,category);
    }
   } else if(category==2) {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==item.id)) {
     var reference=game.InterviewTurnReference(turn);
     if(!game.SourceConcernsPerson(node,game.FindQuestion(item,turn.questionId)?.aboutPersonIds,
      T(turn.promptKey)+" "+T(turn.answerKey)))continue;
     if(game.SourceTried(node,active,reference))continue;
     // Öne sürülen şey kişinin **verdiği yanıttır**, tırnak içinde gösterilir.
     add("chat",T(item.personNameKey),"“"+T(turn.answerKey)+"”",reference,category);
    }
   } else if(!game.SourceTried(node,active,item.id) &&
    game.SourceConcernsPerson(node,item.aboutPersonIds,T(item.titleKey)+" "+T(item.bodyKey))) {
    add("document",T(item.titleKey),locale.Has("kind."+item.kind)?T("kind."+item.kind):null,item.id,category);
   }
  }
  if(rows.Count==0) {Text(questions,T("interview.noSource"),Muted,15);return;}
  InterviewPreview(preview,node,active,sourceId);
  // Telefonda arama alanı yok sayılır; liste zaten kişiye göre süzülüyor. Yerine tür sekmeleri.
  var tabs=new VisualElement();controls.Add(tabs);
  var empty=Text(questions,T("conclude.noMatches"),Muted,15);empty.style.display=DisplayStyle.None;
  string[] labels={"conclude.filter.all","conclude.filter.documents","conclude.filter.interviews","conclude.filter.cctv"};
  Action update=null;
  update=()=>{
   int visible=0;
   for(int i=0;i<rows.Count;i++) {
    bool show=interviewSourceFilter==0 || interviewSourceFilter==categories[i];
    rows[i].style.display=show?DisplayStyle.Flex:DisplayStyle.None;if(show)visible++;
   }
   empty.style.display=visible==0?DisplayStyle.Flex:DisplayStyle.None;
   tabs.Clear();
   var strip=new VisualElement();strip.style.flexDirection=FlexDirection.Row;strip.style.marginBottom=KarineTheme.SpaceSm;tabs.Add(strip);
   for(int i=0;i<labels.Length;i++) {
    int picked=i;
    KarineUI.InterviewFilter(strip,T(labels[i]),interviewSourceFilter==i,i==0 || categoryCounts[i]>0,()=>{interviewSourceFilter=picked;update();});
   }
  };
  update();
 }
 void InterviewPreview(VisualElement preview,Node node,Question active,string sourceId) {
  if(string.IsNullOrEmpty(sourceId)) {
   var hint=Text(preview,T("interview.chooseSource"),Muted,15);hint.style.whiteSpace=WhiteSpace.Normal;
   return;
  }
  var source=SourceNode(sourceId);
  KarineUI.InterviewPaper(preview,source!=null?T(source.titleKey):CompactReportSourceLabel(sourceId),null,out var body);
  InterviewSourceBody(body,sourceId);
  Action send=()=>{
   game.MarkSourceTried(node,active,sourceId);Save();
   var decoy=game.DecoyAnswerKey(active,sourceId);
   if(decoy!=null){InterviewPage(node,active,2,decoy,sourceId,false);return;}
   var reply=game.AnswerKey(active,sourceId);
   if(game.Ask(node.id,active.id,sourceId)){Save();InterviewPage(node,active,2,reply,sourceId);}
   // `answerKey` bir anahtardır; çevrilmiş metin geçilirse ekrana "[...]" düşer.
   // Yemi yazılmamış kaynak: genel "ne diyeyim" yerine kişinin kendi savuşturması.
   else InterviewPage(node,active,2,node.deflectAnswerKey ?? "interview.unrelatedSource",sourceId,false);
  };
  // Dokunarak da, parmakla karşıdakine sürerek de öne sürülür; ikisi aynı kâğıt hareketiyle sonuçlanır.
  Button present=null;bool sent=false;
  var paperLabel=ShortInterviewSourceLabel(CompactReportSourceLabel(sourceId));
  Action slide=()=>{if(sent)return;sent=true;DropFor(sourceId);SlideToPerson(present,paperLabel,send);};
  present=KarineUI.InterviewAction(preview,"document",T("interview.present"),slide);
  DragToPresent(present,slide);
 }
 Node SourceNode(string sourceId) {
  int separator=sourceId.IndexOf('#');
  return game.Data.nodes.FirstOrDefault(n=>n.id==(separator<0?sourceId:sourceId.Substring(0,separator)));
 }
 // Kaydın kâğıda yazılan içeriği: CCTV satırı, görüşme yanıtı ya da belge gövdesi.
 void InterviewSourceBody(VisualElement body,string sourceId) {
  int separator=sourceId.IndexOf('#');
  var source=SourceNode(sourceId);if(source==null)return;
  var ink=KarineTheme.Paper.Ink;var muted=KarineTheme.Paper.Faded;
  if(source.kind=="cctv" && separator>=0) {
   if(!string.IsNullOrEmpty(source.cctvPeriodKey))Text(body,T(source.cctvPeriodKey),muted,12);
   var record=(source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e=>e.id==sourceId.Substring(separator+1));
   if(record!=null)Text(body,T(record.textKey),ink,16);
  } else if(source.kind=="interview" && separator>=0) {
   var turn=game.InterviewSourceTurn(sourceId);
   if(turn!=null) {
    Text(body,T(turn.promptKey),muted,13);
    Text(body,"“"+T(turn.answerKey)+"”",ink,16);
   }
  } else {
   if(source.fileMeta!=null)foreach(var field in source.fileMeta)
    Text(body,T(field.labelKey)+" : "+T(field.valueKey),muted,12);
   Text(body,T(source.bodyKey),ink,15);
  }
 }
 // Öne sürülen kaydın kâğıdı: kimlik kartının altında, kapatılabilir.
 VisualElement InterviewReferenceCard(string sourceId) {
  var source=SourceNode(sourceId);
  if(source==null)return null;
  var holder=new VisualElement {name="InterviewReference"};KarineUI.OfficePlace(holder,KarineTheme.Interview.Reference);root.Add(holder);
  var paper=KarineUI.InterviewPaper(holder,T(source.titleKey),null,out var body);
  KarineUI.CloseButton(paper,()=>holder.style.display=DisplayStyle.None,null,true).style.position=Position.Absolute;
  paper.Children().Last().style.right=KarineTheme.SpaceSm;paper.Children().Last().style.top=KarineTheme.SpaceSm;
  paper.style.paddingRight=KarineTheme.SpaceXl*2;
  InterviewSourceBody(body,sourceId);
  return holder;
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
   KarineUI.Breathe(holder);KarineUI.Posture(art);KarineUI.Idle(holder,personId);KarineUI.Blink(art,personId);
   if(game.Data.nodes.Any(n=>n.personId==personId && n.smokes))KarineUI.Smoke(holder);
   return;
  }
  string[] pixels={
   "....................","......hhhhhhhh......",".....hhhhhhhhhh.....","....hhhhhhhhhhhh....",
   "....hhhsssssshhh....","....hhsssssssshh....","....hssssssssssh....","....hssessssessh....",
   "....hssssnsssssh....","....hssssssssssh....","....hssssmsssssh....","....hhssssssssh.....",
   ".....hhssssssh......","......hsssssh.......",".......sssss........","......ttssstt.......",
   "....tttttttttttt....","...tttttttttttttt...","..tttttttttttttttt..",".tttttttttttttttttt."
  };
  // Buradan aşağısı **arayüz değil, oyun resmidir**: piksel portrenin ten, saç
  // ve giysi tonları. Kit paletinden gelmezler, gelmemeleri gerekir — bir yüzü
  // arayüz kremine boyamak portreyi bozar. `KarineTheme` bu yüzden aranmaz.
  //
  // Tonlar artık vaka verisinden gelir (`Node.portrait`); eskiden kişi kimliği
  // koda yazılıydı, yani her yeni kişi C# düzenlemesi demekti.
  var style=PortraitStyleFor(personId);
  var hair=Swatch(style.hairHex,PortraitStyle.Default.hairHex);
  var skin=Swatch(style.skinHex,PortraitStyle.Default.skinHex);
  var shirt=Swatch(style.shirtHex,PortraitStyle.Default.shirtHex);
  var eye=new Color(.12f,.12f,.12f);
  var eyes=new List<VisualElement>();
  for(int y=0;y<pixels.Length;y++)for(int x=0;x<pixels[y].Length;x++) {
   char p=pixels[y][x];
   if(!style.longHair && y>4 && p=='h')p='.';
   if(style.moustache && y==10 && x>=8 && x<=11)p='h';
   if(p=='.')continue;
   var cell=new VisualElement();cell.style.position=Position.Absolute;
   cell.style.left=Length.Percent(x*5);cell.style.top=Length.Percent(y*5);
   cell.style.width=Length.Percent(5);cell.style.height=Length.Percent(5);
   cell.style.backgroundColor=p=='h'?hair:p=='t'?shirt:p=='e'||p=='m'||p=='n'?eye:skin;
   holder.Add(cell);if(p=='e')eyes.Add(cell);
  }
  KarineUI.Breathe(holder);KarineUI.Blink(eyes,skin);
 }
 // Kişinin portre tanımı, o kişiyi taşıyan görüşme düğümünden okunur.
 PortraitStyle PortraitStyleFor(string personId) {
  foreach(var node in game.Data.nodes)
   if(node.personId==personId && node.portrait!=null)return node.portrait;
  return PortraitStyle.Default;
 }
 static Color Swatch(string hex,string fallback) =>
  KarineTheme.Hex(string.IsNullOrEmpty(hex)?fallback:hex);
}
}
