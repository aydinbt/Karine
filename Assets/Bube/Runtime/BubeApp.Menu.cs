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
  MenuBackdrop();
  var left=new VisualElement();left.style.position=Position.Absolute;
  left.style.left=Length.Percent(6);left.style.top=Length.Percent(7);
  left.style.width=KarineTheme.MainMenu.LogoWidth;
  root.Add(left);

  KarineUI.NeonIgnite(KarineLogo.Hero(left,KarineTheme.MainMenu.LogoWidth));
  var tagline=KarineUI.Technical(left,T("menu.tagline"),KarineTheme.MainMenu.TaglineSize);
  tagline.style.color=KarineTheme.Secondary;
  tagline.style.letterSpacing=3;
  tagline.style.marginTop=0;
  tagline.style.marginBottom=KarineTheme.SpaceXl;

  var menu=new VisualElement();
  menu.style.width=KarineTheme.MainMenu.ColumnWidth;
  left.Add(menu);
  if(game.State.caseAccepted) {
   MenuRow(menu,"folder",T("menu.row.continue"),Desk,true);
  } else {
   MenuRow(menu,"folder",T("menu.row.start"),()=>MaybeWorldIntro(Desk),true);
  }
  MenuRow(menu,"document",T("menu.row.chapters"),WorldPage,false);
  MenuRow(menu,"chart",T("menu.row.career"),StatisticsPage,false);
  MenuRow(menu,"gear",T("menu.row.settings"),SettingsPage,false);
  MenuRow(menu,"info",T("menu.about"),AboutPage,false);

  var identity=KarineUI.MenuIdentity(root,
   Resources.Load<Texture2D>("Bube/Characters/bora"),
   T("menu.identity.name"),T("menu.identity.role"),
   T("menu.identity.unit"),T("menu.identity.location"),StatisticsPage);
  identity.style.position=Position.Absolute;
  identity.style.left=Length.Percent(6);
  identity.style.bottom=Length.Percent(7);
  identity.style.width=KarineTheme.MainMenu.ColumnWidth;
  FadeIn(left);
  FadeIn(identity);
 }

 // Satırın görseli ortak UI Kit bileşenindedir; burada yalnız eylem bağlanır.
 void MenuRow(VisualElement parent,string icon,string label,Action open,bool primary) {
  KarineUI.MenuAction(parent,icon,label,open,primary);
 }

 void QuitGame() {
  Save();
  Application.Quit();
 }

 // Arka plan: dönen animasyon. Video açılmazsa durağan görsele düşer — menü
 // hiçbir durumda boş siyah ekrana bakmaz.
 void MenuBackdrop() {
  if(!menuVideoFailed) {
   if(menuTexture==null) {
    menuTexture=new RenderTexture(1920,1080,0,RenderTextureFormat.ARGB32);
    menuTexture.Create();
   }
   if(menuPlayer==null) {
    menuPlayer=gameObject.AddComponent<VideoPlayer>();
    menuPlayer.playOnAwake=false;
    menuPlayer.isLooping=true;
    menuPlayer.renderMode=VideoRenderMode.RenderTexture;
    menuPlayer.targetTexture=menuTexture;
    // Menü döngüsü sessizdir: müzik/ses ayrı bir karardır.
    menuPlayer.audioOutputMode=VideoAudioOutputMode.None;
    menuPlayer.source=VideoSource.Url;
    menuPlayer.url=Application.streamingAssetsPath+"/Bube/main_menu_loop.mp4";
    menuPlayer.errorReceived+=OnMenuVideoError;
    menuPlayer.prepareCompleted+=OnMenuVideoPrepared;
    menuPlayer.Prepare();
   }
   var film=new Image {image=menuTexture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   film.style.position=Position.Absolute;
   film.style.left=0;film.style.right=0;film.style.top=0;film.style.bottom=0;
   root.Add(film);KarineUI.MenuDrift(film,root);
   return;
  }
  var art=Resources.Load<Texture2D>("Bube/MainMenuNight");
  if(art==null)return;
  art.filterMode=FilterMode.Point;
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
 void ApplySound() { if(audio!=null)audio.ApplyLevels(); }

 // Onay ekranı reklamdan **önce** gelir; onay yoksa hiçbir reklam gösterilmez
 // ve oyunun hiçbir bölümü kapanmaz. Kit'in modalı yıkıcı değil, bu bir tercih.
 void AskForAdConsent() {
  KarineUI.Modal(root,T("ads.consent.title"),T("ads.consent.body"),
   T("ads.consent.deny"),()=>{AdGateway.SetConsent(AdConsent.Denied);RenderSettings();},
   T("ads.consent.allow"),()=>{AdGateway.SetConsent(AdConsent.Granted);RenderSettings();});
 }

 void AboutPage() {
  VisualElement card;MenuOverlay(T("menu.about"),out card);
  Text(card,T("about.body"),Ink,19);
  var spacer=new VisualElement();spacer.style.flexGrow=1;card.Add(spacer);
  Button(card,T("offer.back"),Home);
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
 void StatisticsPage() {
  Home();Back(Home);
  var veil=new VisualElement();KarineUI.OfficePlace(veil,new Rect(0,0,100,100));veil.style.backgroundColor=KarineTheme.Veil(.7f);root.Add(veil);
  var frame=KarineUI.Panel(root,true);frame.name="CareerPanel";frame.style.position=Position.Absolute;
  frame.style.left=Length.Percent(KarineTheme.Settings.Inset-6);frame.style.right=frame.style.left;
  frame.style.top=Length.Percent(KarineTheme.Settings.Top-4);frame.style.bottom=Length.Percent(KarineTheme.Settings.Bottom);
  frame.style.backgroundColor=KarineTheme.Background;KarineUI.Border(frame,2,KarineTheme.Accent);
  var header=KarineUI.Row(frame);header.style.alignItems=Align.Center;header.style.marginBottom=KarineTheme.SpaceMd;
  KarineLogo.Hero(header,KarineTheme.CaseBrowser.LogoWidth*0.7f);
  KarineUI.Icon(header,"folder",KarineTheme.Primary,KarineTheme.TouchTarget-8).style.marginLeft=KarineTheme.SpaceLg;
  var hw=new VisualElement();hw.style.flexGrow=1;hw.style.marginLeft=KarineTheme.SpaceMd;header.Add(hw);
  KarineUI.Subtitle(hw,T("menu.row.career"),KarineTheme.Career.HeadingSize).style.marginBottom=0;
  var hs=KarineUI.Body_(hw,T("menu.stats"),KarineTheme.CaseBrowser.SmallSize+1);hs.style.color=KarineTheme.Secondary;hs.style.marginBottom=0;
  KarineUI.IconButton(header,"close",Home,T("offer.back"));
  var columns=KarineUI.Row(frame,Align.Stretch);columns.style.flexGrow=1;columns.style.minHeight=0;
  var left=new VisualElement();left.style.width=KarineTheme.Career.ProfileWidth;left.style.flexShrink=0;left.style.marginRight=KarineTheme.SpaceMd;columns.Add(left);
  var profile=KarineUI.CareerBox(left,null);profile.style.flexDirection=FlexDirection.Row;profile.style.marginBottom=KarineTheme.SpaceSm;
  var portrait=new Image{image=Resources.Load<Texture2D>("Bube/Characters/bora"),scaleMode=ScaleMode.ScaleAndCrop};
  portrait.style.width=KarineTheme.Career.Portrait*.8f;portrait.style.height=KarineTheme.Career.Portrait;portrait.style.marginRight=KarineTheme.SpaceMd;profile.Add(portrait);
  var who=new VisualElement();who.style.flexGrow=1;who.style.flexShrink=1;profile.Add(who);
  KarineUI.Title(who,T("menu.identity.name"),KarineTheme.Career.ValueSize).style.marginBottom=0;
  var role=KarineUI.Body_(who,T("menu.identity.role")+"\n"+T("menu.identity.unit"),KarineTheme.CaseBrowser.SmallSize+1);role.style.color=KarineTheme.Secondary;
  var place=KarineUI.Row(who);place.style.alignItems=Align.Center;KarineUI.Icon(place,"pin",KarineTheme.Secondary,KarineTheme.IconSize-4).style.marginRight=KarineTheme.SpaceXs;
  var pl=KarineUI.Body_(place,T("menu.identity.location"),KarineTheme.CaseBrowser.SmallSize);pl.style.marginBottom=0;pl.style.flexShrink=1;pl.style.whiteSpace=WhiteSpace.Normal;
  KarineUI.CareerNav(left,"chart",T("career.tab.general"),careerTab==0,()=>{careerTab=0;StatisticsPage();});
  KarineUI.CareerNav(left,"folder",T("career.tab.history"),careerTab==1,()=>{careerTab=1;StatisticsPage();});
  KarineUI.CareerNav(left,"document",T("archive.menu"),false,ArchivePage);
  var right=new VisualElement();right.style.flexGrow=1;right.style.minWidth=0;columns.Add(right);
  var history=game.Career.reviewHistory;int pending=game.Career.pendingReviews.Count;
  if(careerTab==1){CareerHistory(right,history);return;}
  atlas=atlas??Worlds.Load();var closed=Worlds.Closed(game.Career);
  int total=atlas.countries.Sum(c=>c.slots.Count),done=atlas.countries.Sum(c=>Worlds.CompletedIn(c,closed));
  int worlds=atlas.countries.Count(c=>c.slots.Count>0&&Worlds.CompletedIn(c,closed)==c.slots.Count);
  float ratio=total==0?0:(float)done/total;
  var overall=KarineUI.CareerBox(right,T("career.overall"));overall.style.marginBottom=KarineTheme.SpaceSm;
  var figures=KarineUI.Row(overall,Align.Stretch);
  KarineUI.CareerFigure(figures,worlds+" / "+atlas.countries.Count,T("career.worldsDone"),false);
  KarineUI.CareerFigure(figures,done+" / "+total,T("career.casesDone"),true);
  KarineUI.CareerFigure(figures,"%"+Mathf.RoundToInt(ratio*100),T("career.overallShort"),true);
  KarineUI.BrowserMeter(overall,ratio);
  var tiles=KarineUI.Row(right,Align.Stretch);tiles.style.marginBottom=KarineTheme.SpaceSm;
  int supported=history.Count(r=>r.evaluationType=="supported"),incomplete=history.Count(r=>r.evaluationType=="incomplete"),wrong=history.Count(r=>r.evaluationType=="falseAccusation");
  KarineUI.CareerStat(tiles,"folder",T("career.record"),history.Count.ToString());
  KarineUI.CareerStat(tiles,KarineUI.IconOr("check","document"),T("career.supportedCount"),supported.ToString());
  KarineUI.CareerStat(tiles,"chart",T(game.TrustStatusKey),"%"+game.Career.departmentTrust);
  KarineUI.CareerStat(tiles,"clock",T("career.pending"),pending.ToString()).style.marginRight=0;
  var lower=KarineUI.Row(right,Align.Stretch);lower.style.flexGrow=1;lower.style.minHeight=0;
  var stats=KarineUI.CareerBox(lower,T("career.caseStats"));stats.style.flexGrow=1;stats.style.flexBasis=0;stats.style.marginRight=KarineTheme.SpaceSm;
  var sr=KarineUI.Row(stats);sr.style.alignItems=Align.Center;
  var tones=new[]{KarineTheme.Active,KarineTheme.Primary,KarineTheme.Secondary,KarineTheme.Danger};
  var counts=new[]{supported,pending,incomplete,wrong};
  sr.Add(new KarineUI.CareerRing(counts,tones,(history.Count+pending).ToString(),T("career.totalCases")));
  var legend=new VisualElement();legend.style.flexGrow=1;legend.style.marginLeft=KarineTheme.SpaceLg;sr.Add(legend);
  var labels=new[]{T("career.supportedCount"),T("career.pending"),T("career.incompleteCount"),T("career.falseCount")};
  for(int i=0;i<4;i++) KarineUI.CareerLegend(legend,tones[i],labels[i],counts[i]);
  var active=KarineUI.CareerBox(lower,T("career.topWorlds"));active.style.flexGrow=1;active.style.flexBasis=0;
  var ranked=atlas.countries.Select((c,i)=>new{c,i}).OrderByDescending(x=>Worlds.CompletedIn(x.c,closed)).ThenBy(x=>x.i).Take(3).ToList();
  for(int k=0;k<ranked.Count;k++){var x=ranked[k];int pick=x.i;int n=Worlds.CompletedIn(x.c,closed);
   KarineUI.CareerWorldRow(active,k+1,x.c.id,LoadWorldArt(x.c.image),T(x.c.nameKey),n+" / "+x.c.slots.Count,x.c.slots.Count==0?0:(float)n/x.c.slots.Count,
    !Worlds.CountryUnlocked(atlas,x.i,closed),()=>{worldPick=pick;WorldPage();});}
 }
 void CareerHistory(VisualElement parent,System.Collections.Generic.List<FaxReview> history) {
  var box=KarineUI.CareerBox(parent,T("career.tab.history"));box.style.flexGrow=1;box.style.minHeight=0;
  var list=Scroll(box);
  if(history.Count==0) KarineUI.Body_(list,T("career.noHistory"),KarineTheme.CaseBrowser.TextSize).style.color=KarineTheme.Secondary;
  foreach(var review in history.AsEnumerable().Reverse()) {
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   var title=data==null?review.caseId:T(data.titleKey);
   var direction=review.trustChange>0?" ↑":review.trustChange<0?" ↓":" —";
   Button(list,title+"  ·  "+EvaluationTitle(review)+direction+
    (review.reopened?"  ·  "+T("retry.recordShort"):""),()=>CareerRecordPage(review));
  }
 }
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
  VisualElement card;BpsTablet("career.record",out card);
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  Text(card,data==null?review.caseId:T(data.titleKey),Ink,19);
  Text(card,EvaluationTitle(review),Gold,20);
  if(review.evaluatedAtUtcTicks>0)Text(card,new DateTime(review.evaluatedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm"),Muted,14);
  if(data!=null) {
   var person=data.verdicts.FirstOrDefault(v=>v.id==review.suspectId);
   var method=data.methods.FirstOrDefault(v=>v.id==review.methodId);
   var proof=data.evidence.FirstOrDefault(v=>v.id==review.proofId);
   if(person!=null)Text(card,T(SuspectKey(data))+": "+T(person.labelKey)+" · "+ReviewSourceTitle(data,review.suspectSourceId),Ink,16);
   if(method!=null)Text(card,T(MethodKey(data))+": "+T(method.labelKey)+" · "+ReviewSourceTitle(data,review.methodSourceId),Ink,16);
   if(proof!=null)Text(card,T("conclude.evidence")+": "+T(proof.labelKey)+" · "+ReviewSourceTitle(data,review.proofSourceId),Ink,16);
   var custody=(data.custody ?? new Choice[0]).FirstOrDefault(v=>v.id==review.custodyId);
   if(custody!=null)Text(card,T(string.IsNullOrEmpty(data.custodyLabelKey)?"conclude.custody":data.custodyLabelKey)+": "+T(custody.labelKey)+" · "+ReviewSourceTitle(data,review.custodySourceId),Ink,16);
   // Sicil yalnız puan değildir: her raporun bir insana ne yaptığı da kayıtta kalır.
   var epilogue=new[]{person?.epilogueKey,custody?.epilogueKey}.Where(locale.Has).ToArray();
   if(epilogue.Length>0) {
    Text(card,T("career.epilogue"),Gold,17);
    foreach(var key in epilogue)Text(card,T(key),Ink,15);
   }
  }
  Text(card,T("career.trust")+": "+T(TrustStatusKey(review.trustAfter)),Gold,18);
  if(review.reopened)Text(card,T("retry.recordNote"),Muted,16);
  Button(card,T("offer.back"),StatisticsPage);
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
