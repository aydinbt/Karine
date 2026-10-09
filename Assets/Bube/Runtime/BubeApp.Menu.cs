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
  // Giriş seçilmeden menü yalnız giriş kağıdının arkasında durur; o an ilerlemeyi ima etmez.
  if(Accounts.Chosen && game.State.caseAccepted) {
   MenuRow(menu,"folder",T("menu.row.continue"),()=>LoadThen(Desk),true);SlotLine(menu);
  } else {
   MenuRow(menu,"folder",T("menu.row.start"),()=>MaybeWorldIntro(()=>LoadThen(Desk)),true);
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
  KarineUI.ModalSection(list,T("about.links"));
  // Adres config'te yoksa düğme yerinde durur ama basılmaz.
  KarineUI.AboutLink(list,"lock",T("about.link.privacy"),T("about.link.privacy.hint"),OpenPrivacy).SetEnabled(!string.IsNullOrEmpty(config.privacyUrl));
  KarineUI.AboutLink(list,"chat",T("about.link.feedback"),T("about.link.feedback.hint"),OpenSupport).SetEnabled(!string.IsNullOrEmpty(config.supportEmail));
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
   string Basis(string id)=>string.IsNullOrEmpty(id)?"":T("career.basis")+": "+ReviewSourceTitle(data,id);
   if(person!=null)KarineUI.RecordRow(card,T(SuspectKey(data)),T(person.labelKey),Portrait(data.id,person.id),Basis(review.suspectSourceId));
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
  // Yetki genişlemesi (#009): onaylanan raporu yetki veren vakada kalıcı satır.
  if(data!=null && data.grantsAuthority && review.correct)KarineUI.RecordTrust(card,T("career.authority"),T("career.authorityExpanded"),false,null);
 }
 string TrustStatusKey(int value)=>Investigation.StatusKeyFor(value,careerRules);
 void ExitGame() {
#if UNITY_EDITOR
  UnityEditor.EditorApplication.isPlaying=false;
#else
  Application.Quit();
#endif
 }
}
}
