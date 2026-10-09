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
  KarineUI.ConfirmPaper(root,T("restart.form"),T("restart.title"),T("restart.body"),T("restart.stamp"),
   T("restart.cancel"),()=>{confirmRestart=false;Home();},
   T("restart.confirm"),()=>{
   game=new Investigation(Load<CaseData>("Bube/Cases/"+config.initialCase),null,null,careerRules){Text=locale};
   game.Career.activeCaseId=game.Data.id; selectedSuspect=selectedMethod=selectedEvidence=selectedCustody=null;
   selectedSuspectSource=selectedMethodSource=selectedEvidenceSource=selectedCustodySource=null;
   foreach(var file in Resources.LoadAll<TextAsset>("Bube/Cases"))ForgetCaseMoments(file.name);
   Save(); confirmRestart=false; MaybeWorldIntro(Desk);
  });
 }
 // Vaka başına bir kez oynayan anlar (açılış kartı, kapanış, mürekkep) PlayerPrefs'te işaretlidir.
 // Yeni kariyerde ve yeni görevde vaka ilk kez başlıyormuş gibi davranmalı; işaretler silinir.
 void ForgetCaseMoments(string caseId) {
  foreach(var key in new[]{"karine.opened.","karine.caseClosed.","karine.closedCard.","karine.inkDry."})PlayerPrefs.DeleteKey(key+caseId);
  PlayerPrefs.Save();
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
 readonly HashSet<string> presentedArrivals=new HashSet<string>();
 void PresentInboxArrivals() {
  var stage=root.Q("OfficeStage");
  // Wait for the actual desk; dossier/tablet overlays also build a desk beneath them.
  if(stage==null || root.Children().Any(e=>e!=stage && e!=faxNotice && e!=documentNotice && e!=chainEndNotice))return;
  var keys=new List<string>();
  if(!game.State.caseAccepted)keys.Add("offer:"+game.Data.id);
  foreach(var node in game.Data.nodes.Where(incomingDocumentPredicate))keys.Add("document:"+game.Data.id+":"+node.id);
  if(HasIncomingFax)foreach(var review in game.Career.pendingReviews.Where(r=>r.readyAtUtcTicks>0 && r.readyAtUtcTicks<=DateTime.UtcNow.Ticks))
   keys.Add("fax:"+review.caseId+":"+review.readyAtUtcTicks);
  bool fresh=false;foreach(var key in keys)fresh|=presentedArrivals.Add(key);
  if(!fresh)return;
  KarineUI.IncomingPaper(stage);audioDirector?.Play(AudioDirector.Fax);
  if(inboxBadge!=null)inboxBadge.style.opacity=1;
 }
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
 System.Action inboxDeskBack;
 void InboxPage() { InboxPage(null,"all"); }
 void InboxPage(string selectedId,string filter) {
  var entries=new List<InboxEntry>();
  // Kabul edilmemis vaka artik ayri bir tam ekran yerine masadaki tepside durur.
  if(!game.State.caseAccepted)entries.Add(new InboxEntry {
   id="offer:"+game.Data.id,title=CaseText("offer.title","offer.title"),
   status=T("inbox.status.new"),unread=true,offer=game.Data
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
  // Masa altta kalır; evrak onun üstünde modal açılır (3 Ekim 2026 maketi).
  // Masa zaten ekrandaysa yeniden kurulmaz: yeniden kurmak bir kare boş ekran gösteriyordu.
  bool onDesk=SceneManager.GetActiveScene().name=="OfficeScene" && root.Q("OfficeHeader")!=null;
  if(onDesk)root.Q("InboxModal")?.RemoveFromHierarchy();else Desk();
  if(!onDesk || inboxDeskBack==null)inboxDeskBack=escapeBack;
  var deskBack=inboxDeskBack;
  System.Action close=()=>{root.Q("InboxModal")?.RemoveFromHierarchy();escapeBack=deskBack;inboxDeskBack=null;};
  int unreadCount=entries.Count(e=>e.unread);
  var panel=KarineUI.ModalFrame(root,"InboxModal","InboxPanel","document",T("inbox.title"),string.Format(T("inbox.unreadCount"),unreadCount),close);
  Back(close);
  var columns=new VisualElement();columns.style.flexDirection=FlexDirection.Row;columns.style.flexGrow=1;columns.style.minHeight=0;panel.Add(columns);
  var left=new VisualElement {name="InboxListPanel"};left.style.width=Length.Percent(KarineTheme.InboxModal.ListWidth);left.style.flexShrink=0;
  left.style.paddingRight=KarineTheme.SpaceLg;left.style.borderRightWidth=1;left.style.borderRightColor=KarineTheme.Border;columns.Add(left);
  KarineUI.InboxTabs(left,new[]{"all","unread","archive"}.Select(choice=>(T("inbox.filter."+choice),choice==filter,(System.Action)(()=>InboxPage(null,choice)))).ToArray());
  var list=Scroll(left);
  if(visible.Length==0) {
   Text(list,filter=="unread"?T("inbox.empty"):T("inbox.noItems"),Muted,18);
   Text(list,T("inbox.emptyHelp"),Muted,14);
  }
  foreach(var item in visible) {
   var current=item;
   string date=item.review!=null&&item.review.evaluatedAtUtcTicks>0?
    new DateTime(item.review.evaluatedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy"):null;
   KarineUI.InboxRow(list,item.title,item.unread?T("inbox.fresh"):item.status,item.unread,date,item.unread,item.id==selected?.id,()=>InboxPage(current.id,filter));
  }
  var right=new VisualElement();right.style.flexGrow=1;right.style.minWidth=0;right.style.paddingLeft=KarineTheme.SpaceLg;columns.Add(right);
  VisualElement paper;KarineUI.InboxDesk(right,out paper);
  var window=KarineUI.PaperWindow(paper);var paperBody=Scroll(window);KarineUI.PaperFades(window);
  var dark=KarineTheme.Paper.Ink;
  var footer=new VisualElement();footer.style.flexDirection=FlexDirection.Row;footer.style.justifyContent=Justify.SpaceBetween;footer.style.flexShrink=0;
  footer.style.marginTop=KarineTheme.SpaceMd;right.Add(footer);
  KarineUI.SettingsFooterButton(footer,"nav_prev",T("inbox.back"),null,false,close).name="InboxBack";
  // Evrakın eylemleri kâğıdın içinde değil, alttaki şeritte; birincil olan amber.
  // İki eylem (hatırlatma + yeniden açma) yan yana sağa sığmazsa ikinci satıra iner;
  // şerit Geri ile birlikte sağ sütunun içinde kalır, ekrandan taşmaz.
  footer.style.alignItems=Align.FlexEnd;
  var actions=new VisualElement();actions.style.flexDirection=FlexDirection.Row;actions.style.flexWrap=Wrap.Wrap;
  actions.style.justifyContent=Justify.FlexEnd;actions.style.flexShrink=1;actions.style.minWidth=0;actions.style.marginLeft=KarineTheme.SpaceSm;footer.Add(actions);
  if(selected==null) {
   Text(paperBody,T("inbox.noItems"),dark,21);
   Text(paperBody,T("inbox.emptyHelp"),dark,16);
   return;
  }
  // Evrak satır satır basılır (8 Ekim 2026); içerik kurulduktan sonra başlar.
  { var key="inbox:"+selected.id;paperBody.schedule.Execute(()=>PrintOut(paperBody,key)).StartingIn(0); }
  if(selected.offer!=null) {
   KarineUI.PaperLetterhead(paperBody,T("offer.unit"));
   KarineUI.PaperText(paperBody,selected.title,KarineTheme.InboxModal.PaperTitleSize).style.marginTop=KarineTheme.SpaceLg;
   KarineUI.PaperText(paperBody,CaseText("offer.subtitle","offer.subtitle"),KarineTheme.InboxModal.PaperSubSize);
   KarineUI.PaperFields(paperBody,(T("offer.place"),T(game.Data.summary.locationKey)),(T("offer.state"),T("offer.title")));
   KarineUI.PaperText(paperBody,CaseText("offer.summary","offer.summary"),KarineTheme.InboxModal.PaperBodySize).style.marginTop=KarineTheme.SpaceMd;
   KarineUI.PaperStamp(paper,T("offer.title"));
   KarineUI.SettingsFooterButton(actions,"check",T("offer.accept"),T("offer.accept.hint"),true,
    ()=>{ if(game.AcceptCase()){Save();LinkNudgeThen(()=>AdGateway.Request(AdPlacement.CaseStart,AdMoment.CaseAccepted,_=>LoadThen(Desk)));} }).name="InboxAccept";
   return;
  }
  // Diğer evraklar da aynı kâğıt dilinde: birim başlığı, daktilo başlık, künye.
  KarineUI.PaperLetterhead(paperBody,T("file.department"));
  KarineUI.PaperText(paperBody,selected.title,KarineTheme.InboxModal.PaperTitleSize-8).style.marginTop=KarineTheme.SpaceLg;
  KarineUI.PaperText(paperBody,selected.review!=null?selected.status:T(game.Data.titleKey)+"  ·  "+selected.status,KarineTheme.InboxModal.PaperLabelSize+2);
  KarineUI.PaperText(paperBody,T(game.Data.summary.locationKey),KarineTheme.InboxModal.PaperLabelSize).style.marginBottom=KarineTheme.SpaceMd;
  if(selected.document!=null) {
   var document=selected.document;
   if(selected.pending)Text(paperBody,T("inbox.pendingDocument"),dark,18);
   else {
    Text(paperBody,T(document.bodyKey),dark,18);
    if(selected.unread)InboxAction(actions,"check",T("inbox.receiveDocument"),true,()=>{
     if(game.ReceiveDocument(document.id)){Save();InboxPage("document:"+document.id,"all");}
    });
    else InboxAction(actions,"folder",T("inbox.openFile"),true,()=>{
     selectedFileSection="evidence";selectedFileNode=document.id;FilePage();
    });
   }
  } else if(selected.sealedFax) {
   Text(paperBody,T("inbox.faxSealed"),dark,18);
   InboxAction(actions,"document",T("inbox.faxOpen"),true,()=>{
    var review=game.DeliverNextFax();
    if(review==null)return;
    Save();faxNotice?.RemoveFromHierarchy();
    // Onaylanan dosya burada kapanır: önce "KAPANDI" kartı, sonra faksın kendisi.
    if(review.correct && !review.reopened)ClosedCard(review.caseId,()=>EnvelopeCard(review.caseId,()=>ChapterFinale(review.caseId,()=>InboxPage("fax:"+review.caseId,"all"))));
    else InboxPage("fax:"+review.caseId,"all");
   });
  } else if(selected.review!=null)DrawInboxFax(paperBody,selected.review,dark,actions);
  // Faks ve yeni gelen evrak basılarak çıkar; sonuç ne olursa olsun aynı biçimde.
  if(selected.review!=null) {
   string faxKey=selected.id+":"+selected.review.evaluatedAtUtcTicks;
   KarineUI.FaxWear(paperBody,faxKey);
   // İlk okumada kâğıt makineden çıkar ve oda kararır; sonuç ne olursa olsun aynı.
   // Basılarak çıkma, oda kararması ve satır satır yazı kaldırıldı (3 Ekim 2026); yerine yeni efekt gelecek.
   printedPapers.Add(faxKey);
  }
 }
 void DrawInboxFax(VisualElement body,FaxReview fax,Color dark,VisualElement actions) {
  var conclusion=Text(body,EvaluationTitle(fax),dark,21);
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
   if(person!=null)DrawFaxFinding(body,data,SuspectKey(data),person.labelKey,fax.suspectSourceId,fax.suspectSupported,"suspect",dark);
   if(method!=null)DrawFaxFinding(body,data,MethodKey(data),method.labelKey,fax.methodSourceId,fax.methodSupported,"method",dark);
   if(proof!=null)DrawFaxFinding(body,data,"conclude.evidence",proof.labelKey,fax.proofSourceId,fax.proofSupported,"evidence",dark);
   // Dördüncü sütun da raporun parçasıydı; faks onu da değerlendirir.
   var custody=(data.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==fax.custodyId);
   if(custody!=null)DrawFaxFinding(body,data,string.IsNullOrEmpty(data.custodyLabelKey)?"conclude.custody":data.custodyLabelKey,custody.labelKey,fax.custodySourceId,fax.custodySupported,"custody",dark);
   DrawEpilogue(body,data,fax,dark);
  }
  Text(body,T("fax.closing"),dark,15);
  Text(body,T("career.trust")+": "+T(TrustStatusKey(fax.trustAfter))+(fax.trustChange>0?" ↑":fax.trustChange<0?" ↓":""),dark,17);
  if(fax.startedProbation)Text(body,T("career.probation.start"),dark,16);
  else if(fax.endedProbation)Text(body,T("career.probation.end"),dark,16);
  // Rapor geri döndüyse ödüllü yöntem hatırlatması **teklif edilir**, dayatılmaz.
  // Teklif yalnız reklam gösterilebilecekse görünür; gösterilemiyorsa ekranda
  // çalışmayan bir düğme durmaz.
  if(!fax.correct && AdGateway.MayShow(AdPlacement.RewardedGuidance,AdMoment.ReportRejected))
   InboxAction(actions,"info",T("guidance.watch"),false,()=>OfferGuidance());
  if(RetryOffered(fax))InboxAction(actions,"nav_next",T("retry.watch"),true,OfferRetry);
 }

 // Ödüllü yeniden deneme teklifi. Yalnız geri dönen faksın vakası hâlâ elimizde
 // olan vakaysa görünür; reklam gösterilemiyorsa hiç çizilmez.
 void AddRetryOffer(VisualElement body,FaxReview fax) {
  if(RetryOffered(fax))KarineUI.PaperButton(body,T("retry.watch"),OfferRetry,KarinePaperKind.Quiet);
 }
 bool RetryOffered(FaxReview fax)=>fax!=null && !fax.correct && fax.caseId==game.Data.id && game.MayReopen &&
  AdGateway.MayShow(AdPlacement.RewardedRetry,AdMoment.ReportRejected);
 void InboxAction(VisualElement actions,string icon,string title,bool primary,Action click) {
  var b=KarineUI.SettingsFooterButton(actions,icon,title,null,primary,click);b.name="InboxAction";
  b.style.minWidth=StyleKeyword.Auto;b.style.marginLeft=KarineTheme.SpaceSm;b.style.marginTop=KarineTheme.SpaceSm;
  b.style.flexShrink=1;b.style.paddingLeft=KarineTheme.SpaceLg;b.style.paddingRight=KarineTheme.SpaceLg;
 }

 // Yeniden açma ve hatırlatma tam ekran (UI_RETRY / UI_GUIDANCE maketleri); her vakada aynı düzen.
 VisualElement GuidanceScreen(string screenKey,Action redraw) {
  Back(Desk);root.Clear();KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("back.desk"),T(screenKey),T(game.Data.titleKey),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(redraw));
  return root;
 }
 void OfferRetry() {
  AdGateway.Request(AdPlacement.RewardedRetry,AdMoment.ReportRejected,granted=>{
   bool reopened=granted && game.ReopenForRetry();
   if(reopened)Save();
   RetryPage(!granted?"guidance.unavailable":reopened?null:"retry.unavailable");
  });
 }
 // `message` boşsa dosya yeniden açıldı; değilse yalnız o tek cümle (reklam yok / açılamıyor).
 void RetryPage(string message) {
  GuidanceScreen("retry.screen",()=>RetryPage(message));
  var paper=KarineUI.GuidancePaper(root,KarineTheme.Guidance.Note,KarineTheme.Guidance.Tilt);
  if(message!=null) {
   var gap=new VisualElement();gap.style.flexGrow=1;gap.style.minHeight=KarineTheme.SpaceXl*4;paper.Add(gap);
   KarineUI.GuidanceText(paper,T(message),KarineTheme.Guidance.ItemSize+2).style.unityTextAlign=TextAnchor.MiddleCenter;
  } else {
   KarineUI.GuidanceStamp(paper,T("retry.stamp"));
   KarineUI.GuidanceText(paper,T("retry.done"),KarineTheme.Guidance.ItemSize+4).style.whiteSpace=WhiteSpace.Normal;
   KarineUI.GuidanceStatus(paper,"chart",T("career.trust"),T(game.TrustStatusKey),1);
   KarineUI.PaperRule(paper,false);
   KarineUI.GuidanceText(paper,T("retry.keptRecord"),KarineTheme.Guidance.IntroSize).style.whiteSpace=WhiteSpace.Normal;
  }
  var space=new VisualElement();space.style.flexGrow=1;paper.Add(space);
  KarineUI.InterviewAction(paper,null,T("back.desk"),Desk);
 }

 // Ödüllü ipucu ekranı. İçinde vakanın gerçeği **yok**: yöntem hatırlatması
 // (işin kuralları) ve oyuncunun kendi kapsamı (sayılar). Doğrulayıcı bu
 // metinlerde kişi adı, kaynak başlığı ve karar etiketi geçmesini yasaklar.
 void OfferGuidance() {
  AdGateway.Request(AdPlacement.RewardedGuidance,AdMoment.ReportRejected,granted=>{
   if(granted){GuidancePage();return;}
   GuidanceScreen("guidance.screen",OfferGuidance);
   var paper=KarineUI.GuidancePaper(root,KarineTheme.Guidance.Note,KarineTheme.Guidance.Tilt);
   KarineUI.GuidanceHeading(paper,T("guidance.title"),null);
   KarineUI.GuidanceText(paper,T("guidance.unavailable"),KarineTheme.Guidance.ItemSize);
   var space=new VisualElement();space.style.flexGrow=1;paper.Add(space);
   KarineUI.InterviewAction(paper,null,T("offer.back"),InboxPage);
  });
 }
 void GuidancePage() {
  GuidanceScreen("guidance.screen",GuidancePage);
  var paper=KarineUI.GuidancePaper(root,KarineTheme.Guidance.Paper,KarineTheme.Guidance.Tilt);
  KarineUI.GuidanceHeading(paper,T("guidance.title"),T("guidance.body"));
  for(int index=1;index<=4;index++) {
   var key="guidance.method."+index;
   if(locale.Has(key))KarineUI.GuidanceItem(paper,index,T(key));
  }
  var side=KarineUI.GuidanceSide(root,T("guidance.coverage.title"));
  var coverage=Coverage.Of(game);
  if(coverage.Complete)KarineUI.GuidanceText(side,T("guidance.coverage.complete"),KarineTheme.Guidance.MeterSize).style.color=KarineTheme.Primary;
  else {
   KarineUI.GuidanceMeter(side,"folder",T("guidance.coverage.sources"),coverage.SourcesOpen,coverage.SourcesAvailable);
   KarineUI.GuidanceMeter(side,"people",T("guidance.coverage.questions"),coverage.QuestionsAsked,coverage.QuestionsAvailable);
   KarineUI.GuidanceMeter(side,"pin",T("guidance.coverage.clues"),coverage.CluesPinned,coverage.CluesAvailable);
  }
  var spacer=new VisualElement();spacer.style.flexGrow=1;side.Add(spacer);
  KarineUI.InterviewAction(side,null,T("offer.back"),InboxPage);
 }

 // Dosyanın akıbeti: raporun gerçek dünyada neye yol açtığı. Oyuncunun yazdığı kişi ve
 // ikinci sorumluluk için vaka verisinden gelir; yanlış suçlamanın bedeli de burada görünür.
 // Faks gelmeden hiçbir yerde gösterilmez.
 void DrawEpilogue(VisualElement body,CaseData data,FaxReview fax,Color dark) {
  var keys=new List<string>();
  var person=data.verdicts.FirstOrDefault(v=>v.id==fax.suspectId);
  if(person!=null && locale.Has(person.epilogueKey))keys.Add(person.epilogueKey);
  var custody=(data.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==fax.custodyId);
  if(custody!=null && locale.Has(custody.epilogueKey))keys.Add(custody.epilogueKey);
  if(keys.Count==0)return;
  var heading=Text(body,T("fax.epilogue"),dark,18);heading.style.marginTop=10;
  if(dossierBoldFont!=null)heading.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  foreach(var key in keys)Text(body,T(key),dark,16);
 }
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
  bool arriving=SceneManager.GetActiveScene().name!="OfficeScene";
  inboxDeskBack=null;
  Back(Home);StopCctvVideo();StopMenuVideo();EnsureScene("OfficeScene");
  showingInterviewList=false;showingInvestigationRequests=false;root.Clear();
  root.style.backgroundColor=KarineTheme.Background;
  // Active case membership selects the view; menu selection never changes it.
  var countries=Worlds.Load();
  var country=countries.countries.FirstOrDefault(c=>c.slots.Any(s=>s.caseId==game.Data.id));
  var stage=KarineUI.OfficeStage(root,country==null?"tr":country.id);
  var header=new VisualElement {name="OfficeHeader"};
  KarineUI.OfficePlace(header,new Rect(0,0,100,KarineTheme.Office.HeaderHeight));
  header.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);header.style.borderBottomWidth=1;header.style.borderBottomColor=KarineTheme.Border;
  header.style.flexDirection=FlexDirection.Row;header.style.alignItems=Align.Center;
  header.style.paddingLeft=KarineTheme.SpaceLg;header.style.paddingRight=KarineTheme.SpaceMd;stage.Add(header);
  KarineUI.OfficeBrand(header,T("desk.menu"),Home);
  // Dosya kabul edilmeden üst şerit dosya numarasını ve yerini söylemez (8 Ekim 2026).
  if(game.State.caseAccepted)KarineUI.OfficeTitle(header,T(game.Data.titleKey),string.IsNullOrEmpty(game.Data.summary?.locationKey)?null:T(game.Data.summary.locationKey));
  else KarineUI.OfficeTitle(header,T("file.unit"),null);
  bool usable=game.State.caseAccepted&&!game.State.closed&&!game.Career.retired;
  if(usable) {
   KarineUI.OfficeHeaderAction(header,"folder",T("desk.view.file"),FilePage,true);
   KarineUI.OfficeHeaderAction(header,"people",T("desk.view.people"),()=>InterviewRequests());
   KarineUI.OfficeHeaderAction(header,"fingerprint",T("desk.view.clues"),()=>{selectedFileSection="evidence";FilePage();});
   KarineUI.OfficeHeaderAction(header,"document",T("desk.view.documents"),InboxPage);
   KarineUI.OfficeHeaderAction(header,"chart",T("menu.row.career"),StatisticsPage);
   KarineUI.OfficeHeaderAction(header,"gear",T("menu.row.settings"),()=>SettingsFrom(Desk));
  }


  Button inbox=null;
  if(!game.Career.retired && (!game.State.closed||HasIncomingFax||HasIncomingDocument||AvailableAssignment()!=null))
   inbox=KarineUI.OfficeAction(stage,"DeskInbox","document",T("desk.inbox"),KarineTheme.Office.Inbox,InboxPage);
  if(inbox!=null) {
   var parts=T(game.Data.titleKey).Split(new[]{'—'},2);
   // Tepsinin halkası yalnız okunmamış evrak varken atar.
   KarineUI.OfficePulse(inbox,!game.State.caseAccepted||HasIncomingFax||HasIncomingDocument||AvailableAssignment()!=null);
   inboxBadge=KarineUI.OfficeNotice(stage,T("desk.inbox.new"),parts[0].Trim(),out inboxBadgeLabel);RefreshInboxBadge();
  } else {inboxBadge=null;inboxBadgeLabel=null;}
  if(usable) {
   // Halka yalnız bakılmamış bir şey olan eşyada atar; boş ya da görülmüş eşya sessiz durur.
   var file=KarineUI.OfficeAction(stage,"DeskFile","folder",T("desk.view.folder"),KarineTheme.Office.Folder,FilePage);
   KarineUI.OfficePulse(file,game.State.interviewTurns.Count>game.State.seenInterviewTurns || !game.State.read.Contains("report"));
   var phone=KarineUI.OfficeAction(stage,"DeskInterviews","people",T("desk.view.phone"),KarineTheme.Office.Phone,()=>InterviewRequests());
   int fresh=InterviewBadgeCount()+InvestigationBadgeCount();
   KarineUI.OfficeCount(phone,fresh);KarineUI.OfficePulse(phone,fresh>0);
   var terminal=KarineUI.OfficeAction(stage,"DeskTerminal","cctv",T("desk.view.cctv"),KarineTheme.Office.Monitor,OpenTerminal);
   KarineUI.OfficeTabletScreen(stage,terminal);
   KarineUI.OfficePulse(terminal,game.Data.nodes.Any(n=>(n.kind=="cctv"||n.kind=="bps")&&game.Available(n)&&!game.State.read.Contains(n.id)&&!Seen("desk:"+n.id)));
   var evidence=KarineUI.OfficeAction(stage,"DeskEvidence","document",T("desk.view.evidence"),KarineTheme.Office.Evidence,
    ()=>{selectedFileSection="evidence";FilePage();});
   KarineUI.OfficePulse(evidence,game.Data.nodes.Any(n=>n.kind=="document"&&n.id!="report"&&game.Available(n)&&!game.State.read.Contains(n.id)));
  } else if(game.State.closed) {
   var closed=Panel(stage);KarineUI.OfficePlace(closed,new Rect(36,65,30,30));
   Text(closed,T(game.Career.pendingReviews.Any(r=>r.caseId==game.Data.id)?"desk.closed":"desk.reviewed"),Ink,20);
   NextStep(closed,Desk);
  }
  KarineUI.OfficeAtmosphere(stage);DeskFx(stage,arriving);KarineUI.OfficeNight(stage,game.Data.deskHour,DeskWeather);deskStage=stage;StageDesk(stage);
  if(HasIncomingFax)AddFaxNotice();if(HasIncomingDocument)AddDocumentNotice();
  if(game.State.interviewTurns.Count>game.State.seenInterviewTurns&&!game.State.closed) {
   // Dokununca dosyanın tutanak sekmesi açılır; orada görülünce etiket düşer.
   var unread=KarineUI.Button_(stage,T("file.newTranscript"),()=>{selectedFileSection="interview";selectedFileNode=null;FilePage();});
   KarineUI.OfficePlace(unread,new Rect(5,46.5f,18,5));unread.name="DeskNewTranscript";
   KarineUI.Unskin(unread,KarineTheme.GlassDeep);unread.style.fontSize=Typography.Snap(KarineTheme.Office.SmallSize);unread.style.minHeight=0;
  }
 }
 bool Seen(string key) => (game.State.seenRequests ?? new List<string>()).Contains(key);
 void OpenTerminal() {
  cctvFromDesk=true;
  var sources=game.Data.nodes.Where(n=>(n.kind=="cctv" || n.kind=="bps") && game.Available(n)).ToArray();
  // Açılan kayıtlar görülmüş sayılır; tabletin halkası yeni kayıt gelene kadar söner.
  game.State.seenRequests??=new List<string>();
  foreach(var n in sources)if(!Seen("desk:"+n.id))game.State.seenRequests.Add("desk:"+n.id);
  if(sources.Length>0)Save();
  Dial();
  if(sources.Length==0){CctvEmpty();return;}
  if(sources[0].kind=="cctv")CctvScreen(sources[0]);else ReadPage(sources[0]);
 }
 void RequestTabs(VisualElement content,bool interviews) {
  KarineUI.Tabs(content,new[]{T("tablet.tab.interviews"),T("tablet.tab.investigations")},
   interviews?0:1,picked=>{if(picked==0)InterviewRequests(false);else InvestigationRequests(false);},true);
 }
}
}
