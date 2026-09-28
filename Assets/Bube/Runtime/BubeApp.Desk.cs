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
// Masa, gelen evrak tepsisi, tablet kabuğu ve terminal sekmeleri.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 // Kariyeri sıfırlamak geri alınamaz: kit'in yıkıcı onay modalı. Ekranın
 // arkasında ana menü durur, karar tek bir kartta sorulur.
 void RestartPage() {
  if(!confirmRestart){Home();return;}
  Home();
  KarineUI.Modal(root,T("restart.title"),T("restart.body"),
   T("restart.cancel"),()=>{confirmRestart=false;Home();},
   T("restart.confirm"),()=>{
   game=new Investigation(Load<CaseData>("Bube/Cases/"+config.initialCase),null,null,careerRules){Text=locale};
   game.Career.activeCaseId=game.Data.id; selectedSuspect=selectedMethod=selectedEvidence=selectedCustody=null;
   selectedSuspectSource=selectedMethodSource=selectedEvidenceSource=selectedCustodySource=null;
   Save(); confirmRestart=false; MaybeWorldIntro(Desk);
  },true);
 }
 void Hotspot(string label,float x,float y,float w,float h,Action action) {
  // Masadaki nesne de bir düğmedir: basıldığında duyulmalı. Kit dışında
  // kurulduğu için ses kapısı elle geçirilir.
  var button=new Button(KarineUI.Sounded(action)){text=string.Empty,tooltip=label};
  button.style.position=Position.Absolute;
  button.style.left=Length.Percent(x);button.style.top=Length.Percent(y);
  button.style.width=Length.Percent(w);button.style.height=Length.Percent(h);
  button.style.backgroundColor=Color.clear;
  button.style.borderTopWidth=0;button.style.borderBottomWidth=0;
  button.style.borderLeftWidth=0;button.style.borderRightWidth=0;
  button.RegisterCallback<PointerEnterEvent>(_=>button.style.backgroundColor=KarineTheme.HotspotHover);
  button.RegisterCallback<PointerLeaveEvent>(_=>button.style.backgroundColor=Color.clear);
  root.Add(button);
 }
 // Masadaki iki bildirim artık kit'in bildirim bileşeni: başlık + tek satır
 // açıklama + tek eylem. Eskiden ekranın içine yazılmış kırmızı/turuncu düz
 // renklerdi; kırmızı kit'te yalnız kritik uyarıdır, bu yüzden faks kırmızı
 // noktayla, yeni evrak nötr rozetle işaretleniyor.
 void AddFaxNotice() {
  if(!HasIncomingFax || faxNotice!=null && faxNotice.panel!=null)return;
  faxNotice=DeskNotice(T("inbox.faxNotice"),T("inbox.faxNotice.detail"),13f,KarineTone.Danger);
 }
 void AddDocumentNotice() {
  if(!HasIncomingDocument || documentNotice!=null && documentNotice.panel!=null)return;
  documentNotice=DeskNotice(T("inbox.newDocument"),T("inbox.newDocument.detail"),30f,KarineTone.Neutral);
 }
 // Vaka zinciri bitebilir: `nextCaseId` boş olabilir ya da sıradaki vaka
 // taslak olabilir. O zaman masa sessizce boş kalıyordu — oyuncu bir şeyin
 // bozulduğunu sanır. Kapanış bildirimi bunu söyler ve arşive yönlendirir,
 // ama sıradaki adımı **söylemez**: gidecek yeri kalmadığını söylemek ipucu değil.
 void AddChainEndNotice() {
  if(!game.State.closed || game.Career.retired)return;
  if(AvailableAssignment()!=null || HasIncomingFax || HasIncomingDocument)return;
  if(chainEndNotice!=null && chainEndNotice.panel!=null)return;
  chainEndNotice=KarineUI.Notification(root,T("chain.end.title"),T("chain.end.detail"),
   T("chain.end.action"),StatisticsPage,null);
  chainEndNotice.style.position=Position.Absolute;
  chainEndNotice.style.left=Length.Percent(1);chainEndNotice.style.top=Length.Percent(13);
  chainEndNotice.style.width=Length.Percent(29);
  chainEndNotice.style.marginBottom=0;chainEndNotice.style.marginRight=0;
 }
 VisualElement DeskNotice(string title,string detail,float top,KarineTone tone) {
  var notice=KarineUI.Notification(root,title,detail,T("inbox.notice.open"),InboxPage,null);
  notice.style.position=Position.Absolute;
  notice.style.left=Length.Percent(1);notice.style.top=Length.Percent(top);
  notice.style.width=Length.Percent(29);
  notice.style.marginBottom=0;notice.style.marginRight=0;
  if(tone==KarineTone.Danger)KarineUI.Border(notice,KarineTheme.BorderWidth,KarineTheme.Danger);
  return notice;
 }
 void InboxPage() { InboxPage(null,"all"); }
 void InboxPage(string selectedId,string filter) {
  var entries=new List<InboxEntry>();
  // Kabul edilmemis vaka artik ayri bir tam ekran yerine masadaki tepside durur.
  if(!game.State.caseAccepted)entries.Add(new InboxEntry {
   id="offer:"+game.Data.id,title=CaseText("offer.title","offer.title"),
   status=T("inbox.status.new"),unread=true,offer=game.Data
  });
  var assignment=AvailableAssignment();
  if(assignment!=null)entries.Add(new InboxEntry {
   id="assignment:"+assignment.id,title=T("next.assignment"),
   status=T(assignment.titleKey)+" · "+T("inbox.status.new"),unread=true,assignment=assignment
  });
  if(HasIncomingFax) {
   var pending=game.Career.pendingReviews.FirstOrDefault(review=>review.readyAtUtcTicks>0 && review.readyAtUtcTicks<=DateTime.UtcNow.Ticks);
   var asset=pending==null?null:Resources.Load<TextAsset>("Bube/Cases/"+pending.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   entries.Add(new InboxEntry {
    id="fax:new",title=T("inbox.faxTitle"),
    status=(data==null?T("inbox.status.new"):T(data.titleKey)+" · "+T("inbox.status.new")),
    unread=true,sealedFax=true
   });
  }
  foreach(var node in game.Data.nodes.Where(n=>n.requestable && game.State.documentRequests.Any(r=>r.nodeId==n.id))) {
   bool read=game.State.read.Contains(node.id);
   bool arrived=game.IncomingDocument(node);
   entries.Add(new InboxEntry {
    id="document:"+node.id,title=T(node.titleKey),document=node,
    status=T(read?"inbox.status.filed":arrived?"inbox.status.new":"inbox.status.pending"),
    unread=arrived,pending=!read && !arrived
   });
  }
  foreach(var review in game.Career.reviewHistory.AsEnumerable().Reverse()) {
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   entries.Add(new InboxEntry {
    id="fax:"+review.caseId,title=T("inbox.faxTitle"),
    status=data==null?review.caseId:T(data.titleKey),review=review
   });
  }
  var visible=entries.Where(e=>filter=="unread"?e.unread:filter=="archive"?!e.unread && !e.pending:true).ToArray();
  var selected=visible.FirstOrDefault(e=>e.id==selectedId) ?? visible.FirstOrDefault();
  Desk();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.84f);root.Add(shade);
  var binder=new VisualElement();binder.style.position=Position.Absolute;
  binder.style.left=Length.Percent(5);binder.style.right=Length.Percent(5);
  binder.style.top=Length.Percent(6);binder.style.bottom=Length.Percent(6);
  binder.style.flexDirection=FlexDirection.Row;
  binder.style.backgroundColor=KarineTheme.Paper.FolderDeep;
  binder.style.borderTopWidth=5;binder.style.borderBottomWidth=7;
  binder.style.borderLeftWidth=5;binder.style.borderRightWidth=5;
  binder.style.borderTopColor=KarineTheme.Paper.Stamp;
  binder.style.borderBottomColor=KarineTheme.Background;
  binder.style.borderLeftColor=KarineTheme.Paper.Stamp;
  binder.style.borderRightColor=KarineTheme.Background;
  root.Add(binder);
  var left=new VisualElement();left.style.width=Length.Percent(44);
  left.style.backgroundColor=KarineTheme.Glass;
  left.style.paddingLeft=18;left.style.paddingRight=18;
  left.style.paddingTop=14;left.style.paddingBottom=14;
  binder.Add(left);
  var heading=new VisualElement();heading.style.flexDirection=FlexDirection.Row;
  heading.style.alignItems=Align.Center;left.Add(heading);
  var headingText=Text(heading,T("inbox.title"),Ink,23);
  headingText.style.flexGrow=1;headingText.style.marginBottom=3;
  if(dossierBoldFont!=null)headingText.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  KarineUI.CloseButton(heading,Desk,T("back.desk"));
  Text(left,T("desk.brandLocation"),Muted,13).style.marginBottom=12;
  var filters=new VisualElement();filters.style.flexDirection=FlexDirection.Row;
  filters.style.marginBottom=12;left.Add(filters);
  foreach(var choice in new[]{"all","unread","archive"}) {
   var selectedFilter=choice;
   var button=KarineUI.Button_(filters,T("inbox.filter."+choice),()=>InboxPage(null,selectedFilter),
    choice==filter?KarineButtonKind.Primary:KarineButtonKind.Secondary);
   button.style.flexGrow=1;button.style.marginRight=5;button.style.marginBottom=0;
   button.style.fontSize=Typography.Snap(14);
  }
  var list=Scroll(left);
  if(visible.Length==0) {
   Text(list,filter=="unread"?T("inbox.empty"):T("inbox.noItems"),Muted,18);
   Text(list,T("inbox.emptyHelp"),Muted,14);
  }
  foreach(var item in visible) {
   var current=item;
   // Tepsideki satır bir liste seçimidir: seçili olan kit'in birincil dolgusunu
   // alır, okunmamış olan solunda vurgu taşır.
   bool chosen=item.id==selected?.id;
   var row=KarineUI.Button_(list,(item.unread?"●  ":"")+item.title+"\n"+item.status,
    ()=>InboxPage(current.id,filter),chosen?KarineButtonKind.Primary:KarineButtonKind.Secondary);
   row.style.minHeight=78;row.style.marginBottom=7;row.style.marginRight=0;
   row.style.paddingLeft=13;row.style.paddingRight=8;
   row.style.fontSize=Typography.Snap(16);row.style.whiteSpace=WhiteSpace.Normal;
   row.style.unityTextAlign=TextAnchor.MiddleLeft;
   row.style.borderLeftWidth=3;row.style.borderLeftColor=item.unread?KarineTheme.Secondary:KarineTheme.Muted;
  }
  var right=new VisualElement();right.style.flexGrow=1;
  right.style.paddingLeft=15;right.style.paddingRight=15;
  right.style.paddingTop=13;right.style.paddingBottom=13;
  right.style.backgroundColor=KarineTheme.Paper.Board;
  binder.Add(right);
  var paper=new VisualElement();paper.style.flexGrow=1;
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  paper.style.paddingLeft=22;paper.style.paddingRight=22;
  paper.style.paddingTop=17;paper.style.paddingBottom=14;
  right.Add(paper);
  var paperBody=Scroll(paper);
  var dark=KarineTheme.Paper.Ink;
  if(selected==null) {
   Text(paperBody,T("inbox.noItems"),dark,21);
   Text(paperBody,T("inbox.emptyHelp"),dark,16);
   return;
  }
  Text(paperBody,T("inbox.brand"),dark,14);
  var title=Text(paperBody,selected.title,dark,22);
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(paperBody,selected.review!=null || selected.assignment!=null || selected.offer!=null?selected.status:T(game.Data.titleKey)+"  ·  "+selected.status,dark,14);
  var line=new VisualElement();line.style.height=1;line.style.marginBottom=15;
  line.style.backgroundColor=KarineTheme.Paper.Edge;paperBody.Add(line);
  if(selected.offer!=null) {
   Text(paperBody,CaseText("offer.subtitle","offer.subtitle"),dark,18);
   Text(paperBody,CaseText("offer.summary","offer.summary"),dark,18);
   Button(paperBody,T("offer.accept"),()=>{ if(game.AcceptCase()){Save();Desk();} },true);
  } else if(selected.assignment!=null) {
   Text(paperBody,T("next.assignment.sender"),dark,16);
   Text(paperBody,T("next.assignment.body"),dark,18);
   Text(paperBody,T(selected.assignment.titleKey),dark,21);
   Button(paperBody,T("next.assignment.open"),()=>OpenAssignment(selected.assignment),true);
  } else if(selected.document!=null) {
   var document=selected.document;
   if(selected.pending)Text(paperBody,T("inbox.pendingDocument"),dark,18);
   else {
    Text(paperBody,T(document.bodyKey),dark,18);
    if(selected.unread)Button(paperBody,T("inbox.receiveDocument"),()=>{
     if(game.ReceiveDocument(document.id)){Save();InboxPage("document:"+document.id,"all");}
    },true);
    else Button(paperBody,T("inbox.openFile"),()=>{
     selectedFileSection="evidence";selectedFileNode=document.id;FilePage();
    });
   }
  } else if(selected.sealedFax) {
   Text(paperBody,T("inbox.faxSealed"),dark,18);
   Button(paperBody,T("inbox.faxOpen"),()=>{
    var review=game.DeliverNextFax();
    if(review!=null){Save();InboxPage("fax:"+review.caseId,"all");}
   },true);
  } else if(selected.review!=null)DrawInboxFax(paperBody,selected.review,dark);
 }
 void DrawInboxFax(VisualElement body,FaxReview fax,Color dark) {
  var conclusion=Text(body,T("career.evaluation."+fax.evaluationType),dark,21);
  if(dossierBoldFont!=null)conclusion.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(body,T("fax.explainIntro"),dark,16);
  if(fax.evaluatedAtUtcTicks>0)
   Text(body,new DateTime(fax.evaluatedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm"),dark,14);
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+fax.caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  if(data!=null) {
   var person=data.verdicts.FirstOrDefault(v=>v.id==fax.suspectId);
   var method=data.methods.FirstOrDefault(v=>v.id==fax.methodId);
   var proof=data.evidence.FirstOrDefault(v=>v.id==fax.proofId);
   if(person!=null)DrawFaxFinding(body,data,"conclude.suspect",person.labelKey,fax.suspectSourceId,fax.suspectSupported,"suspect",dark);
   if(method!=null)DrawFaxFinding(body,data,"conclude.method",method.labelKey,fax.methodSourceId,fax.methodSupported,"method",dark);
   if(proof!=null)DrawFaxFinding(body,data,"conclude.evidence",proof.labelKey,fax.proofSourceId,fax.proofSupported,"evidence",dark);
  }
  Text(body,T("fax.closing"),dark,15);
  Text(body,T("career.trust")+": "+T(TrustStatusKey(fax.trustAfter))+(fax.trustChange>0?" ↑":fax.trustChange<0?" ↓":""),dark,17);
  if(game.Career.retired)Text(body,T("career.ended"),dark,16);
  // Rapor geri döndüyse ödüllü yöntem hatırlatması **teklif edilir**, dayatılmaz.
  // Teklif yalnız reklam gösterilebilecekse görünür; gösterilemiyorsa ekranda
  // çalışmayan bir düğme durmaz.
  if(!fax.correct && AdGateway.MayShow(AdPlacement.RewardedGuidance,AdMoment.ReportRejected))
   KarineUI.PaperButton(body,T("guidance.watch"),()=>OfferGuidance(),KarinePaperKind.Quiet);
  AddRetryOffer(body,fax);
 }

 // Ödüllü yeniden deneme teklifi. Yalnız geri dönen faksın vakası hâlâ elimizde
 // olan vakaysa görünür; reklam gösterilemiyorsa hiç çizilmez.
 void AddRetryOffer(VisualElement body,FaxReview fax) {
  if(fax==null || fax.correct || fax.caseId!=game.Data.id || !game.MayReopen)return;
  if(!AdGateway.MayShow(AdPlacement.RewardedRetry,AdMoment.ReportRejected))return;
  KarineUI.PaperButton(body,T("retry.watch"),OfferRetry,KarinePaperKind.Quiet);
 }

 void OfferRetry() {
  AdGateway.Request(AdPlacement.RewardedRetry,AdMoment.ReportRejected,granted=>{
   VisualElement card;
   if(!granted) {
    MenuOverlay(T("retry.title"),out card);
    Text(card,T("guidance.unavailable"),Ink,17);
   } else if(!game.ReopenForRetry()) {
    MenuOverlay(T("retry.title"),out card);
    Text(card,T("retry.unavailable"),Ink,17);
   } else {
    Save();
    MenuOverlay(T("retry.title"),out card);
    Text(card,T("retry.done"),Ink,17);
    Text(card,T("career.trust")+": "+T(game.TrustStatusKey),Muted,16);
    Text(card,T("retry.keptRecord"),Muted,15);
   }
   var gap=new VisualElement();gap.style.flexGrow=1;card.Add(gap);
   Button(card,T("offer.back"),Desk);
  });
 }

 // Ödüllü ipucu ekranı. İçinde vakanın gerçeği **yok**: yöntem hatırlatması
 // (işin kuralları) ve oyuncunun kendi kapsamı (sayılar). Doğrulayıcı bu
 // metinlerde kişi adı, kaynak başlığı ve karar etiketi geçmesini yasaklar.
 void OfferGuidance() {
  AdGateway.Request(AdPlacement.RewardedGuidance,AdMoment.ReportRejected,granted=>{
   if(granted)GuidancePage();
   else { VisualElement card;MenuOverlay(T("guidance.title"),out card);
    Text(card,T("guidance.unavailable"),Ink,17);
    var gap=new VisualElement();gap.style.flexGrow=1;card.Add(gap);
    Button(card,T("offer.back"),InboxPage); }
  });
 }

 void GuidancePage() {
  VisualElement card;MenuOverlay(T("guidance.title"),out card);
  Text(card,T("guidance.body"),Muted,15);
  for(int index=1;index<=4;index++) {
   var key="guidance.method."+index;
   if(locale.Has(key))Text(card,"· "+T(key),Ink,16);
  }
  KarineUI.Rule(card);
  KarineUI.Subtitle(card,T("guidance.coverage.title"),17);
  var coverage=Coverage.Of(game);
  if(coverage.Complete)Text(card,T("guidance.coverage.complete"),Ink,16);
  else {
   CoverageMeter(card,"folder",T("guidance.coverage.sources"),coverage.SourcesOpen,coverage.SourcesAvailable);
   CoverageMeter(card,"people",T("guidance.coverage.questions"),coverage.QuestionsAsked,coverage.QuestionsAvailable);
   CoverageMeter(card,"pin",T("guidance.coverage.clues"),coverage.CluesPinned,coverage.CluesAvailable);
  }
  var spacer=new VisualElement();spacer.style.flexGrow=1;card.Add(spacer);
  Button(card,T("offer.back"),InboxPage);
 }

 // Sayı da oranla birlikte verilir: "4/9" oyuncunun kendi çalışmasıdır.
 void CoverageMeter(VisualElement card,string icon,string label,int done,int total) =>
  KarineUI.Meter(card,icon,label+"  "+done+"/"+Mathf.Max(total,done),
   total<=0?1f:Mathf.Clamp01((float)done/total));
 void DrawFaxFinding(VisualElement body,CaseData data,string headingKey,string choiceKey,string sourceId,bool supported,string claim,Color dark) {
  var block=new VisualElement();block.style.marginTop=7;block.style.marginBottom=8;
  block.style.paddingLeft=12;block.style.paddingRight=12;
  block.style.paddingTop=9;block.style.paddingBottom=6;
  block.style.backgroundColor=KarineTheme.Paper.Tint;
  block.style.borderLeftWidth=3;
  block.style.borderLeftColor=supported?KarineTheme.Active:KarineTheme.Paper.Stamp;
  body.Add(block);
  Text(block,T(headingKey)+"  ·  "+T(supported?"fax.supported":"fax.unsupported"),dark,16).style.marginBottom=3;
  Text(block,T(choiceKey),dark,16).style.marginBottom=3;
  Text(block,T("fax.submittedSource")+": "+ReviewSourceTitle(data,sourceId),dark,14).style.marginBottom=4;
  var sourceIdOnly=string.IsNullOrEmpty(sourceId)?string.Empty:sourceId.Split('#')[0];
  var reasonKey=supported?"fax.reason.supported":sourceIdOnly=="report"?"fax.reason."+claim+".report":"fax.reason."+claim+".other";
  Text(block,T(reasonKey),KarineTheme.Paper.Faded,14).style.marginBottom=0;
 }
 void Desk() {
  Back(Home);StopCctvVideo();StopMenuVideo();EnsureScene("OfficeScene");
  showingInterviewList=false;showingInvestigationRequests=false;root.Clear();
  root.style.backgroundColor=KarineTheme.Background;
  // Active case membership selects the view; menu selection never changes it.
  var countries=Worlds.Load();
  var country=countries.countries.FirstOrDefault(c=>c.slots.Any(s=>s.caseId==game.Data.id));
  var stage=KarineUI.OfficeStage(root,country==null?"tr":country.id);
  var header=new VisualElement {name="OfficeHeader"};
  KarineUI.OfficePlace(header,new Rect(0,0,100,12));
  header.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);
  header.style.flexDirection=FlexDirection.Row;header.style.alignItems=Align.Center;
  header.style.paddingLeft=KarineTheme.SpaceXl;header.style.paddingRight=KarineTheme.SpaceLg;stage.Add(header);
  KarineLogo.Hero(header,KarineTheme.Office.BrandWidth);
  var heading=KarineUI.Subtitle(header,T(game.Data.titleKey),KarineTheme.Office.TitleSize);
  heading.style.marginLeft=KarineTheme.SpaceXl;heading.style.flexGrow=1;heading.style.marginBottom=0;
  bool usable=game.State.caseAccepted&&!game.State.closed&&!game.Career.retired;
  if(usable) {
   KarineUI.OfficeHeaderAction(header,"folder",T("desk.view.file"),FilePage);
   KarineUI.OfficeHeaderAction(header,"people",T("desk.view.people"),()=>InterviewRequests());
   KarineUI.OfficeHeaderAction(header,"binoculars",T("desk.view.clues"),()=>{selectedFileSection="evidence";FilePage();});
   KarineUI.OfficeHeaderAction(header,"document",T("desk.view.documents"),InboxPage);
   KarineUI.OfficeHeaderAction(header,"gear",T("menu.row.settings"),SettingsPage);
  }
  KarineUI.IconButton(header,"menu_quit",Home,T("back.home"));

  var board=new VisualElement {name="OfficePeopleBoard"};
  KarineUI.OfficePlace(board,KarineTheme.Office.Board);stage.Add(board);
  var boardTitle=KarineUI.Technical(board,T("desk.view.activeCase"),KarineTheme.Office.LabelSize);
  boardTitle.style.backgroundColor=KarineTheme.Paper.Tint;boardTitle.style.color=KarineTheme.Paper.Ink;
  boardTitle.style.paddingLeft=KarineTheme.SpaceSm;boardTitle.style.marginBottom=KarineTheme.SpaceMd;
  var portraits=new VisualElement();portraits.style.flexDirection=FlexDirection.Row;
  portraits.style.flexWrap=Wrap.Wrap;board.Add(portraits);
  if(game.State.caseAccepted) foreach(var person in game.Data.nodes.Where(n=>n.kind=="interview"&&game.Discovered(n))
   .GroupBy(n=>n.personId).Select(g=>g.First()).Take(4)) {
   KarineUI.OfficePortrait(portraits,Resources.Load<Texture2D>("Bube/Characters/"+person.personId),T(person.personNameKey));
  }
  // Blank monitor screen: this is an entry point, never a preview or clue.
  var monitorLabel=KarineUI.Body_(stage,T("desk.view.cctv"),KarineTheme.Office.TitleSize);
  monitorLabel.name="OfficeMonitorTitle";
  KarineUI.OfficePlace(monitorLabel,KarineTheme.Office.MonitorLabel);
  monitorLabel.style.unityTextAlign=TextAnchor.MiddleCenter;monitorLabel.pickingMode=PickingMode.Ignore;

  Button inbox=null;
  if(!game.Career.retired && (!game.State.closed||HasIncomingFax||HasIncomingDocument||AvailableAssignment()!=null))
   inbox=KarineUI.OfficeAction(stage,"DeskInbox","document",T("desk.inbox"),KarineTheme.Office.InboxLabel,InboxPage);
  if(inbox!=null) {
   var badge=new VisualElement {name="DeskInboxBadge",pickingMode=PickingMode.Ignore};
   badge.style.position=Position.Absolute;badge.style.right=-KarineTheme.SpaceSm;badge.style.top=-KarineTheme.SpaceSm;
   badge.style.width=KarineTheme.Office.BadgeSize;badge.style.height=KarineTheme.Office.BadgeSize;
   badge.style.backgroundColor=KarineTheme.Danger;KarineUI.Round(badge,KarineTheme.Office.BadgeSize);
   inbox.Add(badge);inboxBadge=badge;
   inboxBadgeLabel=KarineUI.Technical(badge,string.Empty,KarineTheme.Office.LabelSize);
   inboxBadgeLabel.style.color=KarineTheme.Primary;inboxBadgeLabel.style.unityTextAlign=TextAnchor.MiddleCenter;
   inboxBadgeLabel.style.marginBottom=0;inboxBadgeLabel.style.flexGrow=1;
   inboxBadgeLabel.pickingMode=PickingMode.Ignore;RefreshInboxBadge();
   badge.schedule.Execute(()=>badge.style.opacity=badge.style.opacity.value>.6f?.3f:1f).Every(KarineTheme.Office.BlinkMs);
  } else {inboxBadge=null;inboxBadgeLabel=null;}
  if(usable) {
   KarineUI.OfficeAction(stage,"DeskFile","folder",T("desk.open"),KarineTheme.Office.FolderLabel,FilePage);
   KarineUI.OfficeAction(stage,"DeskInterviews","people",T("desk.view.phone"),KarineTheme.Office.PhoneLabel,()=>InterviewRequests());
   monitorLabel.style.display=DisplayStyle.None;
   KarineUI.OfficeAction(stage,"DeskTerminal","binoculars",T("desk.view.cctv"),KarineTheme.Office.MonitorLabel,OpenTerminal);
   KarineUI.OfficeAction(stage,"DeskEvidence","document",T("desk.view.evidence"),KarineTheme.Office.EvidenceLabel,
    ()=>{selectedFileSection="evidence";FilePage();});
  } else if(game.Career.retired) {
   var end=Panel(stage);KarineUI.OfficePlace(end,new Rect(30,46,40,28));
   Text(end,T("career.endedTitle"),Gold,24);Text(end,T("career.ended"),Ink,17);
   if(game.State.closed)Button(end,T("result.summaryOpen"),CaseSummary);
  } else if(game.State.closed) {
   var closed=Panel(stage);KarineUI.OfficePlace(closed,new Rect(36,65,30,30));
   Text(closed,T("desk.closed"),Ink,20);
   Button(closed,T("result.summaryOpen"),CaseSummary,true);Button(closed,T("result.continue"),ContinueToNextCase);
  }
  if(HasIncomingFax)AddFaxNotice();if(HasIncomingDocument)AddDocumentNotice();
  if(game.State.interviewTurns.Count>game.State.seenInterviewTurns&&!game.State.closed) {
   var unread=KarineUI.Technical(stage,T("file.newTranscript"),KarineTheme.Office.SmallSize);
   KarineUI.OfficePlace(unread,new Rect(43,75,18,5));
   unread.style.backgroundColor=KarineTheme.GlassDeep;unread.pickingMode=PickingMode.Ignore;
  }
 }
 void BpsTablet(string titleKey,out VisualElement content,bool lift=true) {
  Desk();
  var shade=new VisualElement();
  shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=Color.black;
  shade.style.opacity=0;
  root.Add(shade);
  var tablet=new VisualElement();
  tablet.style.position=Position.Absolute;
  tablet.style.left=Length.Percent(67);tablet.style.top=Length.Percent(17);
  tablet.style.width=Length.Percent(27);tablet.style.height=Length.Percent(27);
  root.Add(tablet);
  var art=Resources.Load<Texture2D>("Bube/CctvTabletHands");
  if(art!=null) {
   art.filterMode=FilterMode.Point;
   var image=new Image { image=art, scaleMode=ScaleMode.ScaleAndCrop, pickingMode=PickingMode.Ignore };
   image.style.position=Position.Absolute;
   image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
   tablet.Add(image);
  } else {
   tablet.style.backgroundColor=KarineTheme.GlassDeep;
  }
  var screen=new VisualElement();
  screen.style.position=Position.Absolute;
  screen.style.left=Length.Percent(14.7f);screen.style.right=Length.Percent(14.7f);
  screen.style.top=Length.Percent(14);screen.style.bottom=Length.Percent(15);
  screen.style.paddingLeft=18;screen.style.paddingRight=18;
  screen.style.paddingTop=12;screen.style.paddingBottom=8;
  screen.style.opacity=0;
  tablet.Add(screen);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;screen.Add(header);
  var brand=Text(header,"BDS",Ink,29);brand.style.marginRight=16;brand.style.marginBottom=0;
  var title=Text(header,T(game.Data.titleKey)+" / "+T(titleKey),Ink,17);title.style.flexGrow=1;title.style.marginBottom=0;
  GlitchHeading(title,T(titleKey));
  KarineUI.CloseButton(header,Desk,T("cctv.back"));
  var rule=new VisualElement();rule.style.height=2;rule.style.backgroundColor=KarineTheme.Panel2;
  rule.style.marginTop=7;rule.style.marginBottom=8;screen.Add(rule);
  content=new VisualElement();content.style.flexGrow=1;screen.Add(content);
  if(lift) {
   var shownScreen=screen;
   int frame=0;
   IVisualElementScheduledItem motion=null;
   motion=tablet.schedule.Execute(()=>{
    frame++;
    float t=Mathf.Clamp01(frame/16f);t=t*t*(3f-2f*t);
    tablet.style.left=Length.Percent(Mathf.Lerp(67f,0f,t));
    tablet.style.top=Length.Percent(Mathf.Lerp(17f,0f,t));
    tablet.style.width=Length.Percent(Mathf.Lerp(27f,100f,t));
    tablet.style.height=Length.Percent(Mathf.Lerp(27f,100f,t));
    shade.style.opacity=t;
    if(frame>=16){motion.Pause();shownScreen.style.opacity=1;}
   }).Every(22);
  } else {
   tablet.style.left=0;tablet.style.top=0;
   tablet.style.width=Length.Percent(100);tablet.style.height=Length.Percent(100);
   shade.style.opacity=1;screen.style.opacity=1;
  }
 }
 void OpenTerminal() {
  var sources=game.Data.nodes.Where(n=>(n.kind=="cctv" || n.kind=="bps") && game.Available(n)).ToArray();
  if(sources.Length==0) {
   VisualElement content;BpsTablet("terminal.title",out content);
   Text(content,T("terminal.noRecords"),Muted,19);
   return;
  }
  if(sources[0].kind=="cctv")CctvScreen(sources[0]);else ReadPage(sources[0]);
 }
 void TerminalSourceTabs(VisualElement content,Node selected) {
  var sources=game.Data.nodes.Where(n=>(n.kind=="cctv" || n.kind=="bps") && game.Available(n)).ToArray();
  if(sources.Length<2)return;
  var index=Array.FindIndex(sources,n=>n.id==selected.id);
  KarineUI.Tabs(content,sources.Select(n=>T(n.titleKey)).ToArray(),index,picked=>{
   var node=sources[picked];
   if(node.kind=="cctv")CctvScreen(node);else ReadPage(node);
  },true);
 }
 void RequestTabs(VisualElement content,bool interviews) {
  KarineUI.Tabs(content,new[]{T("tablet.tab.interviews"),T("tablet.tab.investigations")},
   interviews?0:1,picked=>{if(picked==0)InterviewRequests(false);else InvestigationRequests(false);},true);
 }
}
}
