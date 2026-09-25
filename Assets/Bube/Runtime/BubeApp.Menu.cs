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
 // Ana menü maketin birebir karşılığı: solda marka ve menü, arkada dönen
 // animasyon. Animasyonun sol tarafı zaten karartılmış; menü oraya oturur.
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
  left.style.left=Length.Percent(4);left.style.top=Length.Percent(7);
  left.style.width=Length.Percent(46);left.style.bottom=Length.Percent(4);
  root.Add(left);

  KarineLogo.Hero(left,520);
  var tagline=Text(left,T("menu.tagline"),KarineTheme.Paper.Tint,15);
  tagline.style.letterSpacing=4;tagline.style.marginTop=0;tagline.style.marginBottom=14;

  MenuRule(left);
  var menu=new VisualElement();menu.style.marginTop=8;menu.style.marginBottom=0;left.Add(menu);
  if(game.State.caseAccepted) {
   MenuRow(menu,"folder",T("menu.row.continue"),Desk,true);
   MenuRow(menu,"document",T("menu.row.newCareer"),()=>{confirmRestart=true;RestartPage();},false);
  } else {
   MenuRow(menu,"document",T("menu.row.newCareer"),()=>MaybeWorldIntro(Desk),true);
  }
  MenuRow(menu,"gear",T("menu.row.settings"),SettingsPage,false);
  MenuRow(menu,"chart",T("menu.row.career"),StatisticsPage,false);
  MenuRow(menu,"menu_quit",T("menu.row.quit"),QuitGame,false);

  // Stüdyo bloğu sol sütundan çıktı: menünün altına sığmıyor ve son satırla
  // çakışıyordu. Videonun sağ alt köşesi zaten karanlık, oraya oturuyor.
  var studioBlock=new VisualElement();
  studioBlock.style.position=Position.Absolute;
  studioBlock.style.right=Length.Percent(4);studioBlock.style.bottom=Length.Percent(5);
  studioBlock.style.alignItems=Align.FlexEnd;
  root.Add(studioBlock);
  var studio=Text(studioBlock,"bubeGames",Ink,18);studio.style.marginBottom=1;
  if(fonts!=null && fonts.Heading!=null)studio.style.unityFontDefinition=FontDefinition.FromFont(fonts.Heading);
  Text(studioBlock,"powered by bubeDigital",KarineTheme.Muted,13).style.marginBottom=0;
  FadeIn(left);
  FadeIn(studioBlock);
 }

 // Simge sütunu: maketten kesilmiş PNG, satırın tonuyla boyanır (seçili satırda
 // koyu, ötekilerde krem). Dosya yoksa satır simgesiz kalır ama sütun genişliği
 // korunur, böylece etiketler kaymaz.
 VisualElement MenuIcon(string name,Color tone) {
  var mark=KarineUI.Icon(null,name,tone);
  mark.style.marginRight=KarineTheme.SpaceMd;
  return mark;
 }

 void MenuRule(VisualElement parent) {
  var rule=new VisualElement();
  rule.style.height=2;rule.style.width=70;
  rule.style.backgroundColor=KarineTheme.Accent;
  parent.Add(rule);
 }

 // Menü satırı: solda simge sütunu, ortada etiket, seçili satırda sağda ok.
 // Tek dokunuşluk hedef yüksekliği `MinimumTouchTarget`in üstünde tutulur.
 void MenuRow(VisualElement parent,string icon,string label,Action open,bool primary) {
  var row=new Button(KarineUI.Sounded(open));
  row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;
  row.style.minHeight=MinimumTouchTarget;
  row.style.marginTop=0;row.style.marginBottom=2;row.style.marginLeft=0;row.style.marginRight=0;
  row.style.paddingLeft=10;row.style.paddingRight=12;
  row.style.width=330;
  row.style.borderTopWidth=0;row.style.borderBottomWidth=0;
  row.style.borderLeftWidth=0;row.style.borderRightWidth=0;
  row.style.backgroundColor=primary?Ink:Color.clear;
  var tone=primary?KarineTheme.Glass:Ink;
  parent.Add(row);

  row.Add(MenuIcon(icon,tone));

  var text=new Label(label);
  text.style.color=tone;
  text.style.fontSize=Typography.Snap(17);
  text.style.letterSpacing=2;
  text.style.flexGrow=1;
  text.style.unityTextAlign=TextAnchor.MiddleLeft;
  if(fonts!=null && fonts.Body!=null)text.style.unityFontDefinition=FontDefinition.FromFont(fonts.Body);
  row.Add(text);

  if(primary) {
   var chevron=new Label("›");
   chevron.style.color=tone;chevron.style.fontSize=Typography.Snap(19);
   row.Add(chevron);
  }
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
   root.Add(film);
   return;
  }
  var art=Resources.Load<Texture2D>("Bube/MainMenuNight");
  if(art==null)return;
  art.filterMode=FilterMode.Point;
  var backdrop=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  backdrop.style.position=Position.Absolute;
  backdrop.style.left=0;backdrop.style.right=0;backdrop.style.top=0;backdrop.style.bottom=0;
  root.Add(backdrop);
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
  var scroll=new ScrollView(ScrollViewMode.Vertical);
  scroll.style.flexGrow=1;
  scroll.verticalScrollerVisibility=ScrollerVisibility.Auto;
  frame.Add(scroll);
  card=scroll.contentContainer;
  card.style.flexGrow=1;
 }
 void SettingsPage() {
  VisualElement card;MenuOverlay(T("menu.settings"),out card);
  // Ayarlar üç bölüm: metin, ses, reklam. Her bölüm kendi alt başlığıyla
  // açılıyor ve aralarına çizgi giriyor — eskiden hepsi tek sütunda üst üste
  // yığılıydı ve nerede bittiği belli olmuyordu.
  SettingsSection(card,T("settings.textSpeed"),false);
  KarineUI.Radio(card,T("settings.instant"),instantText,()=>{
   instantText=true;PlayerPrefs.SetInt("bube.instantText",1);PlayerPrefs.Save();SettingsPage();
  });
  KarineUI.Radio(card,T("settings.normal"),!instantText,()=>{
   instantText=false;PlayerPrefs.SetInt("bube.instantText",0);PlayerPrefs.Save();SettingsPage();
  });

  // Ses üç kademedir; kit'te kaydırıcı yok. Üç radyoyu tek satıra sıkıştırmak
  // yerine kit'in **sekme şeridi** kullanılıyor: üç kademe eşit genişlikte,
  // seçili olan dolu. Dokunma hedefi de böylece satır boyunca açılıyor.
  SettingsSection(card,T("settings.music"),true);
  SoundRow(card,SoundSettings.Music,level=>{SoundSettings.SetMusic(level);ApplySound();});
  SettingsSection(card,T("settings.sfx"),false);
  SoundRow(card,SoundSettings.Sfx,level=>{SoundSettings.SetSfx(level);ApplySound();});

  // Reklam onayı ayarlarda durur ve **her zaman geri alınabilir**; onay bir kez
  // alınıp kilitlenen bir şey değildir.
  SettingsSection(card,T("settings.ads"),true);
  Text(card,T("settings.ads.status."+
   (AdGateway.Consent==AdConsent.Granted?"granted":AdGateway.Consent==AdConsent.Denied?"denied":"unknown")),Muted,15);
  Button(card,T("settings.ads.change"),AskForAdConsent);

  // Hakkında da menüden çıktı; ayarların içinde duruyor.
  KarineUI.Rule(card);
  Button(card,T("menu.about"),AboutPage);
  Button(card,T("offer.back"),Home);
 }
 // Bölüm başlığı: ilkinin üstüne çizgi gerekmez, sonrakiler ayrılır.
 void SettingsSection(VisualElement card,string title,bool separated) {
  if(separated)KarineUI.Rule(card);
  var label=KarineUI.Subtitle(card,title,17);
  label.style.marginTop=separated?KarineTheme.SpaceSm:0;
  label.style.marginBottom=KarineTheme.SpaceSm;
 }
 // Üç kademe tek şerit: kapalı / kısık / açık.
 void SoundRow(VisualElement card,SoundLevel current,Action<SoundLevel> onPick) {
  var levels=new[]{SoundLevel.Off,SoundLevel.Low,SoundLevel.Full};
  KarineUI.Tabs(card,levels.Select(level=>T(SoundSettings.LabelKey(level))).ToArray(),
   Array.IndexOf(levels,current),index=>{onPick(levels[index]);SettingsPage();},true);
 }
 void ApplySound() { if(audio!=null)audio.ApplyLevels(); }

 // Onay ekranı reklamdan **önce** gelir; onay yoksa hiçbir reklam gösterilmez
 // ve oyunun hiçbir bölümü kapanmaz. Kit'in modalı yıkıcı değil, bu bir tercih.
 void AskForAdConsent() {
  KarineUI.Modal(root,T("ads.consent.title"),T("ads.consent.body"),
   T("ads.consent.deny"),()=>{AdGateway.SetConsent(AdConsent.Denied);SettingsPage();},
   T("ads.consent.allow"),()=>{AdGateway.SetConsent(AdConsent.Granted);SettingsPage();});
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
   if(suspect!=null)Text(reportColumn,T("conclude.suspect")+": "+T(suspect.labelKey),dark,15);
   if(!string.IsNullOrEmpty(progress.reportSuspectSource))ArchiveSourceLink(reportColumn,item,progress.reportSuspectSource);
   if(method!=null)Text(reportColumn,T("conclude.method")+": "+T(method.labelKey),dark,15);
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
    Text(faxColumn,T("career.evaluation."+fax.evaluationType),dark,15);
    if(suspect!=null)Text(faxColumn,T("conclude.suspect")+": "+T(fax.suspectSupported?"fax.supported":"fax.unsupported"),dark,15);
    if(method!=null)Text(faxColumn,T("conclude.method")+": "+T(fax.methodSupported?"fax.supported":"fax.unsupported"),dark,15);
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
 void StatisticsPage() {
  VisualElement card;BpsTablet("menu.stats",out card);
  KarineUI.Title(card,T("career.bora"),21);
  KarineUI.Technical(card,T("career.rank.investigator"),13);
  var history=game.Career.reviewHistory;
  // Kit'in "DURUM GÖSTERGELERİ" kutusu: güven bir çubuk, tamamlanan vaka bir
  // sayaç. İkisi de teknik metin, yani monospace.
  var meters=KarineUI.Row(card,Align.Stretch);
  var trust=KarineUI.Meter(meters,"gear",T(game.TrustStatusKey),game.Career.departmentTrust/100f);
  trust.style.flexGrow=1;trust.style.marginRight=KarineTheme.SpaceMd;
  var closed=KarineUI.Counter(meters,"folder",T("career.record"),history.Count.ToString());
  closed.style.flexGrow=1;closed.style.marginRight=0;
  var tally=KarineUI.Row(card);
  Tally(tally,T("career.supportedCount"),history.Count(r=>r.evaluationType=="supported"),KarineTone.Active);
  Tally(tally,T("career.incompleteCount"),history.Count(r=>r.evaluationType=="incomplete"),KarineTone.Neutral);
  Tally(tally,T("career.falseCount"),history.Count(r=>r.evaluationType=="falseAccusation"),KarineTone.Danger);
  var pending=game.Career.pendingReviews.Count;
  if(pending>0)Tally(tally,T("career.pending"),pending,KarineTone.Neutral);
  var list=Scroll(card);
  foreach(var review in history.AsEnumerable().Reverse()) {
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   var title=data==null?review.caseId:T(data.titleKey);
   var direction=review.trustChange>0?" ↑":review.trustChange<0?" ↓":" —";
   Button(list,title+"  ·  "+T("career.evaluation."+review.evaluationType)+direction+
    (review.reopened?"  ·  "+T("retry.recordShort"):""),()=>CareerRecordPage(review));
  }
  // Arşiv menü satırı olmaktan çıktı (maket beş satır gösteriyor); kariyer
  // ekranının içinde duruyor — kapanmış dosyalar zaten kariyer geçmişidir.
  Button(card,T("archive.menu"),ArchivePage);
  Button(card,T("offer.back"),Home);
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
  Text(card,T("career.evaluation."+review.evaluationType),Gold,20);
  if(review.evaluatedAtUtcTicks>0)Text(card,new DateTime(review.evaluatedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm"),Muted,14);
  if(data!=null) {
   var person=data.verdicts.FirstOrDefault(v=>v.id==review.suspectId);
   var method=data.methods.FirstOrDefault(v=>v.id==review.methodId);
   var proof=data.evidence.FirstOrDefault(v=>v.id==review.proofId);
   if(person!=null)Text(card,T("conclude.suspect")+": "+T(person.labelKey)+" · "+ReviewSourceTitle(data,review.suspectSourceId),Ink,16);
   if(method!=null)Text(card,T("conclude.method")+": "+T(method.labelKey)+" · "+ReviewSourceTitle(data,review.methodSourceId),Ink,16);
   if(proof!=null)Text(card,T("conclude.evidence")+": "+T(proof.labelKey)+" · "+ReviewSourceTitle(data,review.proofSourceId),Ink,16);
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
