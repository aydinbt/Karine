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
// Ana menü, ayarlar, hakkında, arşiv ve kariyer ekranları.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 // Video arka planda kalır; marka, eylemler ve personel kartı ayrı UI
 // katmanlarıdır. Böylece menü görseli tek bir tıklanabilir resim değildir.
 void Home() {
  // Ana menü kökün kendisi: geri tuşunun gidecek yeri yok, çıkış onayı açılır.
  Back(null);
  StopCctvVideo();
  EnsureScene("MainMenuScene");
  showingInterviewList=false;
  root.Clear();
  root.style.backgroundColor=Color.black;
  MenuBackdrop();MenuVeil();MenuReturnAd();
  var left=new VisualElement();left.style.position=Position.Absolute;
  left.style.left=Length.Percent(6);left.style.top=Length.Percent(7);
  left.style.width=KarineTheme.MainMenu.LogoWidth;
  root.Add(left);

  var logo=KarineLogo.Aligned(left,KarineTheme.MainMenu.LogoWidth);KarineUI.NeonIgnite(logo);KarineUI.NeonStutter(logo);
  KarineLogo.Tagline(left,KarineTheme.MainMenu.LogoWidth,T("menu.tagline")).style.marginBottom=KarineTheme.SpaceXl;

  var menu=new VisualElement();
  menu.style.width=KarineTheme.MainMenu.ColumnWidth;
  left.Add(menu);
  if(game.State.caseAccepted) {
   MenuRow(menu,"folder",T("menu.row.continue"),Desk,true);SlotLine(menu);
  } else {
   MenuRow(menu,"folder",T("menu.row.start"),()=>MaybeWorldIntro(Desk),true);
  }
  MenuRow(menu,"document",T("menu.row.chapters"),WorldPage,false);
  MenuRow(menu,"chart",T("menu.row.career"),StatisticsPage,false);
  MenuRow(menu,"gear",T("menu.row.settings"),SettingsPage,false);
  MenuRow(menu,"info",T("menu.about"),AboutPage,false);
  // Çıkış ayrı bir çizginin altında; yanlışlıkla dokunulmasın diye onay modalını açar.
  var cut=new VisualElement();cut.style.height=1;cut.style.backgroundColor=KarineTheme.Border;
  cut.style.marginTop=KarineTheme.SpaceMd;cut.style.marginBottom=KarineTheme.SpaceMd+KarineTheme.MainMenu.RowGap;
  menu.Add(cut);
  KarineUI.MenuAction(menu,"menu_quit",T("menu.row.quit"),AskToQuit,false).name="MenuExit";

  var studio=KarineUI.StudioMark(root,KarineTheme.Loading.StudioHeight);studio.style.position=Position.Absolute;
  studio.style.right=Length.Percent(3);studio.style.bottom=Length.Percent(4);studio.style.opacity=.85f;
  var version=KarineUI.Technical(root,"v"+Application.version,KarineTheme.MainMenu.TaglineSize);
  version.style.position=Position.Absolute;version.style.right=Length.Percent(3);version.style.bottom=Length.Percent(4);version.style.marginBottom=KarineTheme.Loading.StudioHeight+KarineTheme.SpaceSm;
  version.style.color=KarineTheme.Alpha(KarineTheme.Secondary,.7f);
  KarineUI.MenuStorm(root);KarineUI.MenuStreet(root);KarineUI.MenuParallax(root);KarineUI.MenuIntro(root,logo,menu);
 }

 // Sol sütunun arkası koyulaşır, sağdaki sahne açık kalır: kademeli üç bant.
 void MenuVeil() {
  float[] widths={52,40,30};float[] alphas={.28f,.28f,.3f};
  for(int i=0;i<widths.Length;i++) {
   var band=new VisualElement {pickingMode=PickingMode.Ignore};
   band.style.position=Position.Absolute;band.style.left=0;band.style.top=0;band.style.bottom=0;
   band.style.width=Length.Percent(widths[i]);band.style.backgroundColor=KarineTheme.Veil(alphas[i]);
   root.Add(band);
  }
 }

 // Satırın görseli ortak UI Kit bileşenindedir; burada yalnız eylem bağlanır.
 void MenuRow(VisualElement parent,string icon,string label,Action open,bool primary) {
  KarineUI.MenuAction(parent,icon,label,open,primary);
 }

 void QuitGame() {
  Save();
  Application.Quit();
 }

 // Arka plan: durağan görsel (3 Ekim 2026: video kaldırıldı). Canlılık
 // `MenuDrift` katmanından gelir: pencerede yağmur, lambada titrek hale.
 void MenuBackdrop() {
  var art=Resources.Load<Texture2D>("Bube/MainMenuBackdrop") ?? Resources.Load<Texture2D>("Bube/MainMenuNight");
  if(art==null)return;
  art.filterMode=FilterMode.Bilinear;
  var backdrop=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  backdrop.style.position=Position.Absolute;
  backdrop.style.left=0;backdrop.style.right=0;backdrop.style.top=0;backdrop.style.bottom=0;
  root.Add(backdrop);KarineUI.MenuDrift(backdrop,root);
 }

 void OnMenuVideoPrepared(VideoPlayer player) => player.Play();

 void OnMenuVideoError(VideoPlayer player,string message) {
  Debug.LogWarning("Main menu loop unavailable: "+message);
  menuVideoFailed=true;
  StopMenuVideo();
  if(root!=null && root.childCount>0)Home();
 }

 void StopMenuVideo() {
  if(menuPlayer!=null) {
   menuPlayer.errorReceived-=OnMenuVideoError;
   menuPlayer.prepareCompleted-=OnMenuVideoPrepared;
   menuPlayer.Stop();Destroy(menuPlayer);menuPlayer=null;
  }
  if(menuTexture!=null){menuTexture.Release();Destroy(menuTexture);menuTexture=null;}
 }

 // Menü üstü kart. Eskiden kenarlardan %27 içeriydi: geniş bir ekranda
 // makuldü, telefonda dikey tutulduğunda kart daracık bir şerit oluyor ve
 // ayarlar sıkışıyordu. Artık kenar payı ekranın biçimine göre; içerik de
 // kaydırılabilir, çünkü ayarlar kartın boyundan uzun olabilir.
 void MenuOverlay(string title,out VisualElement card) {
  Back(Home);
  Home();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.72f);root.Add(shade);
  bool tall=Screen.height>=Screen.width;
  var frame=KarineUI.Panel(root,true);
  frame.style.position=Position.Absolute;
  frame.style.left=Length.Percent(tall?5:24);frame.style.right=Length.Percent(tall?5:24);
  frame.style.top=Length.Percent(tall?7:12);frame.style.bottom=Length.Percent(tall?6:12);
  frame.style.marginBottom=0;frame.style.marginRight=0;
  KarineUI.Title(frame,title,24);
  KarineUI.Rule(frame);
  // Kaydırma kartın **içinde**: başlık sabit kalır, içerik akar.
  var scroll=new KarineScrollView(ScrollViewMode.Vertical);
  scroll.style.flexGrow=1;
  scroll.verticalScrollerVisibility=ScrollerVisibility.Hidden;
  frame.Add(scroll);
  card=scroll.contentContainer;
  card.style.flexGrow=1;
 }
 void ApplySound() { if(audioDirector!=null)audioDirector.ApplyLevels(); }

 // Onay ekranı reklamdan **önce** gelir; onay yoksa hiçbir reklam gösterilmez
 // ve oyunun hiçbir bölümü kapanmaz. Kit'in modalı yıkıcı değil, bu bir tercih.
 void AskForAdConsent(Action after) {
  KarineUI.Modal(root,T("ads.consent.title"),T("ads.consent.body"),
   T("ads.consent.deny"),()=>{AdGateway.SetConsent(AdConsent.Denied);after?.Invoke();},
   T("ads.consent.allow"),()=>{AdGateway.SetConsent(AdConsent.Granted);after?.Invoke();});
 }

 // Hakkında: açıldığı ekranın üstünde modal. Emeği geçenler yalnız gerçek
 // kaynakları sayar; gizlilik ve geri bildirim düğmeleri adresler gelince açılır.
 void AboutPage() {
  var escape=escapeBack;
  System.Action close=()=>{root.Q("AboutModal")?.RemoveFromHierarchy();escapeBack=escape;};
  var panel=KarineUI.ModalFrame(root,"AboutModal","AboutPanel","info",T("menu.about"),T("about.subtitle"),close);
  Back(close);
  var columns=new VisualElement();columns.style.flexDirection=FlexDirection.Row;columns.style.flexGrow=1;columns.style.minHeight=0;panel.Add(columns);
  var left=new VisualElement();left.style.width=Length.Percent(42);left.style.alignItems=Align.Center;left.style.paddingRight=KarineTheme.SpaceLg;
  left.style.borderRightWidth=1;left.style.borderRightColor=KarineTheme.Border;columns.Add(left);
  float width=KarineTheme.Loading.LogoWidth*.62f;
  var identity=new VisualElement();identity.style.alignItems=Align.FlexStart;left.Add(identity);
  KarineLogo.Hero(identity,width);KarineLogo.Tagline(identity,width,T("menu.tagline"));
  var version=KarineUI.Technical(left,T("about.version")+" "+Application.version,KarineTheme.Loading.LabelSize);version.style.color=KarineTheme.Secondary;version.style.letterSpacing=3;
  version.style.marginTop=KarineTheme.SpaceSm;
  var body=KarineUI.Body_(left,T("about.body"),KarineTheme.SettingsModal.RowTitleSize-2);body.style.whiteSpace=WhiteSpace.Normal;body.style.unityTextAlign=TextAnchor.MiddleCenter;
  body.style.marginTop=KarineTheme.SpaceMd;
  var spacer=new VisualElement();spacer.style.flexGrow=1;left.Add(spacer);
  var made=KarineUI.Technical(left,T("about.madeBy"),KarineTheme.Loading.LabelSize);made.style.color=KarineTheme.Secondary;made.style.letterSpacing=4;made.style.marginBottom=KarineTheme.SpaceXs;
  KarineUI.StudioMark(left,KarineTheme.Loading.StudioHeight+12);
  var right=new KarineScrollView();right.style.flexGrow=1;right.style.minHeight=0;right.style.paddingLeft=KarineTheme.SpaceXl;columns.Add(right);
  var list=right.contentContainer;
  KarineUI.ModalSection(list,T("about.credits"));
  KarineUI.AboutCredit(list,T("about.credit.studio"),"bubeGames");
  KarineUI.AboutCredit(list,T("about.credit.fonts"),T("about.credit.fonts.detail"));
  KarineUI.ModalSection(list,T("about.links")).style.marginTop=KarineTheme.SpaceLg;
  // Adresler henüz yok: düğmeler yerinde durur ama basılmaz.
  KarineUI.AboutLink(list,"lock",T("about.link.privacy"),T("about.link.privacy.hint"),null).SetEnabled(false);
  KarineUI.AboutLink(list,"chat",T("about.link.feedback"),T("about.link.feedback.hint"),null).SetEnabled(false);
  KarineUI.ModalRule(panel);
  var footer=new VisualElement();footer.style.flexDirection=FlexDirection.Row;footer.style.justifyContent=Justify.SpaceBetween;footer.style.flexShrink=0;panel.Add(footer);
  var copy=KarineUI.Body_(footer,"© "+System.DateTime.Now.Year+" bubeGames. "+T("about.rights"),KarineTheme.SettingsModal.RowHintSize);copy.style.color=KarineTheme.Secondary;copy.style.marginBottom=0;
  var thanks=KarineUI.Body_(footer,T("credits.2"),KarineTheme.SettingsModal.RowHintSize+1);thanks.style.unityFontStyleAndWeight=FontStyle.Italic;thanks.style.marginBottom=0;
 }
 ArchivedCase[] ClosedCases() {
  var result=new List<ArchivedCase>();
  var careerCaseIds=new HashSet<string>(game.Career.pendingReviews.Select(review=>review.caseId)
   .Concat(game.Career.reviewHistory.Select(review=>review.caseId)));
  if(game.State.closed)careerCaseIds.Add(game.Data.id);
  foreach(var asset in Resources.LoadAll<TextAsset>("Bube/Cases")) {
   try {
    var data=JsonUtility.FromJson<CaseData>(asset.text);
    if(data==null || string.IsNullOrEmpty(data.id) || data.nodes==null || !careerCaseIds.Contains(data.id))continue;
    var path=CaseSavePath(data.id);
    if(!File.Exists(path))continue;
    var progress=JsonUtility.FromJson<Progress>(File.ReadAllText(path));
    var outcome=SaveMigration.Migrate(progress);
    if(outcome!=SaveOutcome.Loaded && outcome!=SaveOutcome.Migrated)continue;
    if(progress.caseId!=data.id || !progress.closed)continue;
    if(progress.read==null)progress.read=new List<string>();
    if(progress.asked==null)progress.asked=new List<string>();
    if(progress.interviewTurns==null)progress.interviewTurns=new List<InterviewTurn>();
    if(progress.timelinePinned==null)progress.timelinePinned=new List<string>();
    result.Add(new ArchivedCase {data=data,progress=progress});
   } catch(Exception e) { Debug.LogWarning("Archive entry could not be loaded: "+e.Message); }
  }
  return result.OrderByDescending(item=>item.progress.submittedAtUtcTicks).ToArray();
 }
 VisualElement ArchivePaper(string heading,out VisualElement content) {
  Home();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=KarineTheme.Veil(.82f);root.Add(shade);
  var backing=new VisualElement();backing.style.position=Position.Absolute;
  backing.style.left=Length.Percent(8);backing.style.right=Length.Percent(8);
  backing.style.top=Length.Percent(6);backing.style.bottom=Length.Percent(5);
  backing.style.backgroundColor=KarineTheme.Paper.Folder;root.Add(backing);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(9);paper.style.right=Length.Percent(9);
  paper.style.top=Length.Percent(5);paper.style.bottom=Length.Percent(6);
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=18;paper.style.paddingBottom=16;
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;root.Add(paper);
  var dark=KarineTheme.Paper.Ink;
  // Vaka secici basligi: sol ustte kompakt marka, altinda cizgi.
  KarineLogo.Header(paper,150,KarineTheme.Paper.Stamp,KarineTheme.Paper.FolderEdge);
  Text(paper,T("archive.kicker"),KarineTheme.Paper.Stamp,14).style.marginBottom=2;
  var archiveHeading=Text(paper,heading,dark,27);archiveHeading.style.marginBottom=8;
  if(fonts!=null && fonts.Heading!=null)archiveHeading.style.unityFontDefinition=FontDefinition.FromFont(fonts.Heading);
  var line=new VisualElement();line.style.height=1;line.style.backgroundColor=KarineTheme.Paper.Edge;
  line.style.marginBottom=12;paper.Add(line);
  content=new VisualElement();content.style.flexGrow=1;paper.Add(content);
  return paper;
 }
 void ArchivePage() {
  VisualElement content;
  var paper=ArchivePaper(T("archive.title"),out content);
  var dark=KarineTheme.Paper.Ink;
  var cases=ClosedCases();
  KarineUI.Technical(content,T("archive.count")+"  "+cases.Length,15).style.color=dark;
  var list=Scroll(content);
  if(cases.Length==0)Text(list,T("archive.empty"),dark,18);
  foreach(var item in cases) {
   var stamp=item.progress.submittedAtUtcTicks>0
    ?new DateTime(item.progress.submittedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm")
    :T("summary.unknownDate");
   Button(list,T(item.data.titleKey)+"  ·  "+stamp+"   ›",()=>ArchiveCasePage(item.data.id,null));
  }
  KarineUI.PaperButton(paper,T("offer.back"),Home).style.minHeight=45;
 }
 bool ArchiveReferenceAvailable(ArchivedCase item,string reference,out Node node) {
  node=null;
  if(string.IsNullOrEmpty(reference))return false;
  int separator=reference.IndexOf('#');
  var nodeId=separator<0?reference:reference.Substring(0,separator);
  node=item.data.nodes.FirstOrDefault(candidate=>candidate.id==nodeId);
  if(node==null)return false;
  if(node.kind=="interview") {
   var turns=item.progress.interviewTurns.Where(turn=>turn.nodeId==nodeId).ToArray();
   if(separator<0)return turns.Length>0;
   return turns.Any(turn=>nodeId+"#"+turn.questionId+
    (string.IsNullOrEmpty(turn.sourceId)?"":"|"+turn.sourceId)==reference);
  }
  if(!item.progress.read.Contains(nodeId))return false;
  if(node.kind=="cctv")return separator>=0 && (node.cctvEvents ?? new CctvEvent[0])
   .Any(record=>record.id==reference.Substring(separator+1));
  return separator<0;
 }
 void ArchiveSourceLink(VisualElement parent,ArchivedCase item,string reference) {
  Node source;
  bool available=ArchiveReferenceAvailable(item,reference,out source);
  var turn=source!=null && source.kind=="interview"
   ?item.progress.interviewTurns.FirstOrDefault(t=>t.nodeId==source.id && game.InterviewTurnReference(t)==reference):null;
  var sourceLabel=turn==null?ReviewSourceTitle(item.data,reference)
   :T(source.personNameKey)+" · "+T(turn.promptKey)+" · "+T(turn.answerKey);
  var label=T("conclude.source")+": "+sourceLabel;
  if(!available) {
   Text(parent,label,KarineTheme.Paper.Faded,13);
   return;
  }
  var link=KarineUI.PaperButton(null,label+"  →",()=>ArchiveCasePage(item.data.id,source.id,reference),
   KarinePaperKind.Choice,true);
  link.style.minHeight=42;link.style.fontSize=Typography.Snap(13);
  link.style.paddingLeft=9;link.style.paddingRight=8;link.style.marginBottom=10;
  parent.Add(link);
 }
 void ArchiveCasePage(string caseId,string sourceId,string focusReference=null) {
  var item=ClosedCases().FirstOrDefault(entry=>entry.data.id==caseId);
  if(item==null){ArchivePage();return;}
  var data=item.data;var progress=item.progress;
  VisualElement content;
  var paper=ArchivePaper(T(data.titleKey),out content);
  var dark=KarineTheme.Paper.Ink;var muted=KarineTheme.Paper.Faded;
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.flexGrow=1;content.Add(row);
  var sourceColumn=new VisualElement();sourceColumn.style.width=Length.Percent(28);
  sourceColumn.style.paddingRight=14;row.Add(sourceColumn);
  Text(sourceColumn,T("archive.sources"),dark,17);
  var sourceList=Scroll(sourceColumn);
  Button(sourceList,T("archive.report"),()=>ArchiveCasePage(caseId,null),sourceId==null);
  if(data.timelineClues!=null && data.timelineClues.Length>0)
   Button(sourceList,T("file.tab.timeline"),()=>ArchiveCasePage(caseId,ArchiveTimelineId),sourceId==ArchiveTimelineId);
  var available=data.nodes.Where(node=>progress.read.Contains(node.id) || node.kind=="interview" && progress.interviewTurns.Any(turn=>turn.nodeId==node.id)).ToArray();
  foreach(var node in available) {
   if(node.id=="report")continue;
   Button(sourceList,T(node.titleKey),()=>ArchiveCasePage(caseId,node.id),sourceId==node.id);
  }
  var detail=Scroll(row);detail.style.flexGrow=1;
  detail.style.borderLeftWidth=1;detail.style.borderLeftColor=KarineTheme.Paper.Edge;
  detail.style.paddingLeft=20;
  var selected=available.FirstOrDefault(node=>node.id==sourceId);
  if(sourceId==ArchiveTimelineId) {
   Text(detail,T("timeline.title"),dark,21);
   var pinned=(data.timelineClues ?? new TimelineClue[0])
    .Where(clue=>progress.timelinePinned.Contains(clue.id) &&
     (clue.requiresRead==null || clue.requiresRead.All(progress.read.Contains)) &&
     (clue.requiresAsked==null || clue.requiresAsked.All(progress.asked.Contains)))
    .OrderBy(clue=>clue.sortMinute).ThenBy(clue=>clue.id).ToArray();
   if(pinned.Length==0)Text(detail,T("timeline.empty"),muted,16);
   foreach(var clue in pinned)TimelineRow(detail,clue,dark,muted,null,null);
  } else if(selected==null) {
   var comparison=new VisualElement();comparison.style.flexDirection=FlexDirection.Row;
   comparison.style.width=Length.Percent(100);detail.Add(comparison);
   var reportColumn=new VisualElement();reportColumn.style.flexGrow=1;reportColumn.style.flexBasis=0;
   reportColumn.style.paddingRight=14;comparison.Add(reportColumn);
   Text(reportColumn,T("archive.report"),dark,19);
   var suspect=data.verdicts.FirstOrDefault(v=>v.id==progress.reportSuspect);
   var method=data.methods.FirstOrDefault(v=>v.id==progress.reportMethod);
   var proof=data.evidence.FirstOrDefault(v=>v.id==progress.reportProof);
   if(suspect!=null)Text(reportColumn,T(SuspectKey(data))+": "+T(suspect.labelKey),dark,15);
   if(!string.IsNullOrEmpty(progress.reportSuspectSource))ArchiveSourceLink(reportColumn,item,progress.reportSuspectSource);
   if(method!=null)Text(reportColumn,T(MethodKey(data))+": "+T(method.labelKey),dark,15);
   if(!string.IsNullOrEmpty(progress.reportMethodSource))ArchiveSourceLink(reportColumn,item,progress.reportMethodSource);
   if(proof!=null)Text(reportColumn,T("conclude.evidence")+": "+T(proof.labelKey),dark,15);
   if(!string.IsNullOrEmpty(progress.reportProofSource))ArchiveSourceLink(reportColumn,item,progress.reportProofSource);
   var faxColumn=new VisualElement();faxColumn.style.flexGrow=1;faxColumn.style.flexBasis=0;
   faxColumn.style.paddingLeft=14;faxColumn.style.borderLeftWidth=1;
   faxColumn.style.borderLeftColor=KarineTheme.Paper.Edge;comparison.Add(faxColumn);
   Text(faxColumn,T("archive.fax"),dark,19);
   // Ödüllü yeniden deneme aynı vaka için ikinci satır yazabilir; arşiv masadaki
   // raporu en son değerlendirmeyle karşılaştırır.
   var fax=game.Career.reviewHistory.LastOrDefault(review=>review.caseId==caseId);
   if(fax==null)Text(faxColumn,T("archive.pendingReview"),muted,15);
   else {
    Text(faxColumn,EvaluationTitle(fax),dark,15);
    if(suspect!=null)Text(faxColumn,T(SuspectKey(data))+": "+T(fax.suspectSupported?"fax.supported":"fax.unsupported"),dark,15);
    if(method!=null)Text(faxColumn,T(MethodKey(data))+": "+T(fax.methodSupported?"fax.supported":"fax.unsupported"),dark,15);
    if(proof!=null)Text(faxColumn,T("conclude.evidence")+": "+T(fax.proofSupported?"fax.supported":"fax.unsupported"),dark,15);
    Text(faxColumn,T("career.trust")+": "+T(TrustStatusKey(fax.trustAfter))+
     (fax.trustChange>0?" ↑":fax.trustChange<0?" ↓":""),muted,14);
    if(fax.reopened)Text(faxColumn,T("retry.recordNote"),muted,14);
   }
  } else {
   Text(detail,T(selected.titleKey),dark,21);
   if(selected.kind=="interview") {
    var turns=progress.interviewTurns.Where(turn=>turn.nodeId==selected.id).ToArray();
    foreach(var turn in turns) {
     var turnReference=selected.id+"#"+turn.questionId+
      (string.IsNullOrEmpty(turn.sourceId)?"":"|"+turn.sourceId);
     var target=new VisualElement();target.style.marginBottom=10;detail.Add(target);
     Text(target,T("interview.bora")+": "+T(turn.promptKey),muted,15);
     if(!string.IsNullOrEmpty(turn.sourceId))Text(target,T("interview.presented")+"  "+ReviewSourceTitle(data,turn.sourceId),muted,13);
     Text(target,T(selected.personNameKey)+": "+T(turn.answerKey),dark,16);
     if(focusReference==turnReference)ArchiveFocus(detail,target);
    }
   } else if(selected.kind=="cctv") {
    Text(detail,T(selected.cctvSourceKey),muted,15);
    Text(detail,T(selected.cctvPeriodKey),muted,14);
    foreach(var record in selected.cctvEvents ?? new CctvEvent[0]) {
     var target=Text(detail,T(record.textKey),dark,16);
     if(focusReference==selected.id+"#"+record.id)ArchiveFocus(detail,target);
    }
   } else Text(detail,T(selected.bodyKey),dark,16);
  }
  KarineUI.PaperButton(paper,T("archive.back"),ArchivePage).style.minHeight=45;
 }
 void ArchiveFocus(ScrollView scroll,VisualElement target) {
  target.style.backgroundColor=KarineTheme.Paper.Tint;
  target.style.paddingLeft=7;target.style.paddingRight=7;
  scroll.schedule.Execute(()=>scroll.ScrollTo(target)).ExecuteLater(1);
 }
 int careerTab;
 // Kariyer ekranı (yeni görünüm, 2 Ekim 2026). Yalnız gerçekten tutulan sayılar gösterilir:
 // referanstaki "incelenen delil / görüşme / sicil no" gibi alanlar kariyer verisinde yok.
 // Tek satır sayım: nokta + etiket + sayı. Renk kit'in tonlarıdır; kırmızı
 // yalnız yanlış suçlama gibi ağır sonuç içindir.
 void Tally(VisualElement parent,string label,int count,KarineTone tone) {
  var item=KarineUI.Row(parent);
  item.style.marginRight=KarineTheme.SpaceXl;
  KarineUI.Dot(item,tone).style.marginRight=KarineTheme.SpaceSm;
  var text=KarineUI.Technical(item,label+"  "+count,13);
  text.style.marginBottom=0;
 }

 void CareerRecordPage(FaxReview review) {
  Back(StatisticsPage);root.Clear();KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("career.backToCareer"),T("career.recordTitle"),T("file.unit"),StatisticsPage,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(()=>CareerRecordPage(review)));
  var card=KarineUI.RecordPaper(root);
  if(PlayerPrefs.GetInt("karine.inkDry."+review.caseId,0)==0){PlayerPrefs.SetInt("karine.inkDry."+review.caseId,1);KarineUI.InkDry(card);}
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  string date=review.evaluatedAtUtcTicks>0?T("career.evaluatedAt")+": "+new DateTime(review.evaluatedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm"):null;
  KarineUI.RecordHead(card,data==null?review.caseId:T(data.titleKey),date,T(review.correct?"career.stamp.verified":"career.stamp.rejected"),!review.correct);
  KarineUI.RecordAfter(card,EvaluationTitle(review));
  if(data!=null) {
   var person=data.verdicts.FirstOrDefault(v=>v.id==review.suspectId);
   var method=data.methods.FirstOrDefault(v=>v.id==review.methodId);
   var proof=data.evidence.FirstOrDefault(v=>v.id==review.proofId);
   var custody=(data.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==review.custodyId);
   KarineUI.RecordHeading(card,T("career.sentReport"));
   string Basis(string id)=>T("career.basis")+": "+ReviewSourceTitle(data,id);
   if(person!=null)KarineUI.RecordRow(card,T(SuspectKey(data)),T(person.labelKey),Resources.Load<Texture2D>("Bube/Characters/"+person.id),Basis(review.suspectSourceId));
   if(method!=null)KarineUI.RecordRow(card,T(MethodKey(data)),T(method.labelKey),null,Basis(review.methodSourceId));
   if(proof!=null)KarineUI.RecordRow(card,T("conclude.evidence"),T(proof.labelKey),null,Basis(review.proofSourceId));
   if(custody!=null)KarineUI.RecordRow(card,T(string.IsNullOrEmpty(data.custodyLabelKey)?"conclude.custody":data.custodyLabelKey),T(custody.labelKey),null,Basis(review.custodySourceId));
   // Sicil yalnız puan değildir: her raporun bir insana ne yaptığı da kayıtta kalır.
   var epilogue=new[]{person?.epilogueKey,custody?.epilogueKey}.Where(locale.Has).ToArray();
   if(epilogue.Length>0){KarineUI.RecordHeading(card,T("career.epilogue"));foreach(var key in epilogue)KarineUI.RecordAfter(card,T(key));}
  }
  var status=TrustStatusKey(review.trustAfter);
  KarineUI.RecordTrust(card,T("career.trustState"),T(status),status=="career.status.ended"||status=="career.status.risk"||status=="career.status.review"||status=="career.status.monitored",
   review.reopened?T("retry.recordNote"):null);
 }
 string TrustStatusKey(int value) {
  var t=careerRules.statusThresholds;
  if(value<=careerRules.endThreshold)return "career.status.ended";
  return value>=t[0]?"career.status.high":value>=t[1]?"career.status.reliable":value>=t[2]?"career.status.monitored":value>=t[3]?"career.status.review":"career.status.risk";
 }
 void ExitGame() {
#if UNITY_EDITOR
  UnityEditor.EditorApplication.isPlaying=false;
#else
  Application.Quit();
#endif
 }
}
}
