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
public sealed class BubeApp : MonoBehaviour {
 sealed class InboxEntry {
  public string id;
  public string title;
  public string status;
  public Node document;
  public FaxReview review;
  public CaseData offer;
  public bool unread;
  public bool pending;
  public bool sealedFax;
  public CaseData assignment;
 }
 sealed class ArchivedCase {
  public CaseData data;
  public Progress progress;
 }
 static BubeApp instance;
 Locale locale;
 CareerRules careerRules;
 GameConfig config;
 Investigation game;
 VisualElement root;
 bool confirmRestart;
 bool instantText;
 string selectedFileNode="report";
 string selectedFileSection="report";
 string fileSearchQuery="";
 string selectedSearchTurn;
 string compareLeftId, compareRightId;
 int comparePicker=-1;
 Font dossierFont, dossierBoldFont;
 string selectedSuspect, selectedMethod, selectedEvidence;
 string selectedSuspectSource, selectedMethodSource, selectedEvidenceSource;
 bool showingInterviewList;
 string selectedInterviewTopic, selectedInterviewNodeId;
 bool showingInterviewHistory;
 string interviewSourceQuery="", interviewSourceQuestionId;
 int interviewSourceFilter;
 bool showingInvestigationRequests;
 int lastPendingCount=-1;
 int lastIncomingDocumentCount=-1;
 // Yüklem temsilcileri bir kez kurulur; her karede `game.IncomingDocument` geçmek kare başına Func ayırır.
 // Lambda'lar `game` alanını okur, örnek yeniden kurulduğunda da geçerli kalır.
 Func<Node,bool> pendingPredicate;
 Func<Node,bool> incomingDocumentPredicate;
 string assignmentCacheId;
 CaseData assignmentCache;
 int lastInboxBadgeCount=-1;
 VisualElement lastBadgedElement;
 VisualElement lastSafeAreaRoot;
 Rect lastSafeArea;
 int lastSafeAreaWidth,lastSafeAreaHeight;
 VisualElement faxNotice;
 VisualElement documentNotice;
 VisualElement inboxBadge;
 Label inboxBadgeLabel;
 VideoPlayer introPlayer;
 RenderTexture introTexture;
 VisualElement introBrand;
 VisualElement introPlace;
 Button introSkip;
 WorldIntro activeIntro;
 CornerMark activeMark;
 bool deskArrivalDone;
 VisualElement deskArrivalFade;
 const float DeskArrivalFadeSeconds=1.15f;
 Action introAfter;
 VideoPlayer cctvPlayer;
 RenderTexture cctvTexture;
 VisualElement cctvViewer;
 Label cctvVideoStatus;
 Button cctvPlaybackButton, cctvStepButton;
 bool cctvReachedEnd;

 static readonly Color Ink = new Color(.91f,.88f,.77f);
 static readonly Color Muted = new Color(.62f,.65f,.62f);
 static readonly Color Gold = new Color(.86f,.67f,.36f);
 static readonly Color Base = new Color(.07f,.10f,.12f);
 static readonly Color Card = new Color(.12f,.16f,.17f);
 static readonly Color Paper = new Color(.17f,.20f,.19f);
 const string ArchiveTimelineId="@timeline";
 const int MinimumTouchTarget=48;

 string CaseSavePath(string id) => Path.Combine(Application.persistentDataPath,"bube-"+id+"-v1.json");
 string SavePath => CaseSavePath(game.Data.id);
 string CareerSavePath => Path.Combine(Application.persistentDataPath,"bube-career-v1.json");
 bool HasIncomingFax => game.HasIncomingFax;
 bool HasIncomingDocument => game.HasIncomingDocument;
 // Resources içeriği çalışma anında değişmez; sonuç vaka kimliği başına bir kez ayrıştırılır.
 // Önbellek yoksa Update() her karede tam vaka JSON'unu ayrıştırır.
 CaseData AvailableAssignment() {
  if(!game.State.closed || game.Career.retired || string.IsNullOrEmpty(game.Data.nextCaseId))return null;
  var nextId=game.Data.nextCaseId;
  if(assignmentCacheId!=nextId) {
   assignmentCacheId=nextId;
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+nextId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   assignmentCache=data!=null && !data.draft?data:null;
  }
  return assignmentCache;
 }
 string T(string key) => locale.Get(key);
 string CaseText(string suffix,string fallbackKey) {
  var key=game.Data.id+"."+suffix;
  var value=T(key);
  return value=="["+key+"]"?T(fallbackKey):value;
 }
 TData Load<TData>(string path) => JsonUtility.FromJson<TData>(Resources.Load<TextAsset>(path).text);

 void Awake() {
  if(instance!=null && instance!=this){Destroy(gameObject);return;}
  instance=this;
  DontDestroyOnLoad(gameObject);
  config=Load<GameConfig>("Bube/config");
  locale=Load<Locale>("Bube/Locales/"+config.locale);
  careerRules=Load<CareerRules>("Bube/career-rules");
  CareerProgress career=null;
  try { if(File.Exists(CareerSavePath)) career=JsonUtility.FromJson<CareerProgress>(File.ReadAllText(CareerSavePath)); }
  catch(Exception e) { Debug.LogWarning("Career save could not be loaded: "+e.Message); }
  string caseId=career!=null && !string.IsNullOrEmpty(career.activeCaseId) && Resources.Load<TextAsset>("Bube/Cases/"+career.activeCaseId)!=null
   ?career.activeCaseId:config.initialCase;
  var caseData=Load<CaseData>("Bube/Cases/"+caseId);
  bool redirectedDraft=caseData.draft;
  if(redirectedDraft) {
   caseId=config.initialCase;
   caseData=Load<CaseData>("Bube/Cases/"+caseId);
  }
  Progress progress=null;
  try { if(File.Exists(CaseSavePath(caseId))) progress=JsonUtility.FromJson<Progress>(File.ReadAllText(CaseSavePath(caseId))); }
  catch(Exception e) { Debug.LogWarning("Save could not be loaded: "+e.Message); }
  pendingPredicate=n=>game.Pending(n);
  incomingDocumentPredicate=n=>game.IncomingDocument(n);
  game=new Investigation(caseData,progress,career,careerRules);
  game.Career.activeCaseId=caseId;
  instantText=PlayerPrefs.GetInt("bube.instantText",0)==1;
  var doc=GetComponent<UIDocument>() ?? gameObject.AddComponent<UIDocument>();
  var panel=ScriptableObject.CreateInstance<PanelSettings>();
  panel.scaleMode=PanelScaleMode.ScaleWithScreenSize;
  panel.referenceResolution=new Vector2Int(1280,720);
  panel.screenMatchMode=PanelScreenMatchMode.MatchWidthOrHeight;
  panel.match=1;
  panel.themeStyleSheet=Resources.Load<ThemeStyleSheet>("Bube/DefaultTheme");
  doc.panelSettings=panel;
  dossierFont=Resources.Load<Font>("Bube/Fonts/IBMPlexMono-Regular");
  dossierBoldFont=Resources.Load<Font>("Bube/Fonts/IBMPlexMono-SemiBold");
  root=doc.rootVisualElement;
  if(dossierFont!=null)root.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
  root.style.backgroundColor=Base;
  root.style.color=Ink;
  root.style.fontSize=Typography.Snap(20);
  root.style.paddingLeft=36;
  root.style.paddingRight=36;
  root.style.paddingTop=26;
  root.style.paddingBottom=24;
  if(redirectedDraft)Save();
  Home();
 }

 void EnsureScene(string sceneName) {
  if(SceneManager.GetActiveScene().name!=sceneName)
   SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
 }

 void Update() {
  if(root==null)return;
  if(introBrand!=null && introBrand.panel!=null && introPlayer!=null) {
   double time=introPlayer.time;
   float reveal=Mathf.Clamp01((float)(time-3.0)/0.3f);
   float hide=1-Mathf.Clamp01((float)(time-4.55)/0.3f);
   introBrand.style.opacity=reveal*hide;
   if(introPlace!=null && introPlace.panel!=null) {
    float placeReveal=Mathf.Clamp01((float)(time-1.8)/0.5f);
    float placeHide=1-Mathf.Clamp01((float)(time-4.4)/0.35f);
    introPlace.style.opacity=placeReveal*placeHide;
   }
  }
  if(showingInterviewList) {
   int pending=game.Data.nodes.Count(pendingPredicate);
   if(pending!=lastPendingCount) { lastPendingCount=pending; InterviewRequests(false); }
  }
  if(showingInvestigationRequests) {
   int incoming=game.Data.nodes.Count(incomingDocumentPredicate);
   if(incoming!=lastIncomingDocumentCount) { lastIncomingDocumentCount=incoming; InvestigationRequests(false); }
  }
  var safe=Screen.safeArea;
  if(Screen.width<=0 || Screen.height<=0)return;
  // Güvenli alan kenar boşlukları yalnız ekran ölçüsü, güvenli alan ya da kök öge değiştiğinde yazılır.
  if(!ReferenceEquals(root,lastSafeAreaRoot) || safe!=lastSafeArea || Screen.width!=lastSafeAreaWidth || Screen.height!=lastSafeAreaHeight) {
   lastSafeAreaRoot=root; lastSafeArea=safe; lastSafeAreaWidth=Screen.width; lastSafeAreaHeight=Screen.height;
   root.style.left=Length.Percent(safe.xMin/Screen.width*100);
   root.style.right=Length.Percent((Screen.width-safe.xMax)/Screen.width*100);
   root.style.top=Length.Percent((Screen.height-safe.yMax)/Screen.height*100);
   root.style.bottom=Length.Percent(safe.yMin/Screen.height*100);
  }
  if(HasIncomingFax || HasIncomingDocument) {
   bool atOffice=SceneManager.GetActiveScene().name=="OfficeScene";
   if(HasIncomingFax && atOffice)AddFaxNotice();
   if(HasIncomingDocument && atOffice)AddDocumentNotice();
  }
  RefreshInboxBadge();
 }

 void RefreshInboxBadge() {
  if(inboxBadge==null || inboxBadge.panel==null)return;
  int count=game.Data.nodes.Count(incomingDocumentPredicate)+(HasIncomingFax?1:0)+(AvailableAssignment()!=null?1:0)
   +(game.State.caseAccepted?0:1);
  // Rozet yeniden kurulduğunda öge kimliği değişir; o durumda sayı aynı olsa da yeniden yazılır.
  if(count==lastInboxBadgeCount && ReferenceEquals(inboxBadge,lastBadgedElement))return;
  lastInboxBadgeCount=count; lastBadgedElement=inboxBadge;
  inboxBadge.style.display=count>0?DisplayStyle.Flex:DisplayStyle.None;
  inboxBadgeLabel.text=count>0?count.ToString():string.Empty;
 }

 void Save() {
  try {
   var temp=SavePath+".tmp";
   File.WriteAllText(temp,JsonUtility.ToJson(game.State,true));
   if(File.Exists(SavePath)) File.Replace(temp,SavePath,null); else File.Move(temp,SavePath);
   var careerTemp=CareerSavePath+".tmp";
   File.WriteAllText(careerTemp,JsonUtility.ToJson(game.Career,true));
   if(File.Exists(CareerSavePath)) File.Replace(careerTemp,CareerSavePath,null); else File.Move(careerTemp,CareerSavePath);
  } catch(Exception e) { Debug.LogWarning("Save failed: "+e.Message); Text(root,T("save.failed"),Muted,16); }
 }

 Label Text(VisualElement parent,string value,Color color,int size) {
  var label=new Label(value);
  label.style.whiteSpace=WhiteSpace.Normal;
  label.style.color=color;
  label.style.fontSize=Typography.Snap(size);
  if(dossierFont!=null)label.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
  label.style.marginBottom=12;
  parent.Add(label);
  return label;
 }
 void Button(VisualElement parent,string value,Action onClick,bool primary=false) {
  var button=new Button(onClick){text=value};
  button.style.minHeight=54;
  button.style.fontSize=Typography.Snap(20);
  if(dossierFont!=null)button.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
  button.style.unityTextAlign=TextAnchor.MiddleLeft;
  button.style.paddingLeft=18;
  button.style.marginBottom=10;
  button.style.backgroundColor=primary?Gold:Paper;
  button.style.color=primary?Base:Ink;
  button.style.borderTopWidth=0;
  button.style.borderBottomWidth=0;
  button.style.borderLeftWidth=primary?5:1;
  button.style.borderRightWidth=0;
  button.style.borderLeftColor=primary?Ink:Muted;
  parent.Add(button);
 }
 VisualElement Panel(VisualElement parent,int grow=0) {
  var panel=new VisualElement();
  panel.style.backgroundColor=Card;
  panel.style.paddingLeft=24;
  panel.style.paddingRight=24;
  panel.style.paddingTop=22;
  panel.style.paddingBottom=16;
  panel.style.marginRight=16;
  panel.style.marginBottom=16;
  if(grow>0)panel.style.flexGrow=grow;
  parent.Add(panel);
  return panel;
 }
 void Frame(string kicker,string title,string subtitle) {
  root.Clear();
  Text(root,config.title+"  /  "+kicker,Gold,16);
  Text(root,title,Ink,38);
  if(!string.IsNullOrEmpty(subtitle))Text(root,subtitle,Muted,17);
 }
 ScrollView Scroll(VisualElement parent) {
  var scroll=new ScrollView();
  scroll.style.flexGrow=1;
  parent.Add(scroll);
  return scroll;
 }
 void FadeIn(VisualElement element) {
  element.style.opacity=0;
  int step=0;
  IVisualElementScheduledItem animation=null;
  animation=element.schedule.Execute(()=>{
   step++;
   element.style.opacity=Mathf.Min(1,step/12f);
   if(step>=12)animation.Pause();
  }).Every(25);
 }
 void Typewriter(Label label,string line) {
  if(instantText){label.text=line;return;}
  label.text=string.Empty;
  int length=0;
  IVisualElementScheduledItem animation=null;
  animation=label.schedule.Execute(()=>{
   length=Mathf.Min(line.Length,length+2);
   label.text=line.Substring(0,length);
   if(length>=line.Length)animation.Pause();
  }).Every(22);
 }

 void GlitchHeading(Label label,string value) {
  if(instantText || string.IsNullOrEmpty(value))return;
  string[] frames={"▌ "+value,"▌ "+value.Substring(0,Mathf.Max(1,value.Length/2))+" ░",value};
  int index=0;
  IVisualElementScheduledItem animation=null;
  animation=label.schedule.Execute(()=>{
   if(label.panel==null){animation.Pause();return;}
   label.text=frames[index++];
   if(index>=frames.Length)animation.Pause();
  }).Every(65);
 }
 void ReportSheet(string heading,string subtitle,out VisualElement body,Action backAction=null,bool wide=false) {
  Desk();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.02f,.025f,.03f,.84f);root.Add(shade);
  var backing=new VisualElement();backing.style.position=Position.Absolute;
  backing.style.left=Length.Percent(wide?6:20);backing.style.right=Length.Percent(wide?6:19);
  backing.style.top=Length.Percent(7);backing.style.bottom=Length.Percent(5);
  backing.style.backgroundColor=new Color(.29f,.15f,.13f);
  backing.style.borderBottomWidth=7;backing.style.borderBottomColor=new Color(.12f,.08f,.07f);root.Add(backing);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(wide?8:22);paper.style.right=Length.Percent(wide?8:21);
  paper.style.top=Length.Percent(5);paper.style.bottom=Length.Percent(7);
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=18;paper.style.paddingBottom=15;
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);root.Add(paper);
  var dark=new Color(.13f,.16f,.20f);
  var kicker=Text(paper,T(game.Data.titleKey),new Color(.45f,.29f,.23f),14);kicker.style.marginBottom=5;
  var title=Text(paper,heading,dark,26);title.style.marginBottom=5;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(paper,subtitle,dark,14);
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=new Color(.59f,.52f,.43f);
  rule.style.marginBottom=9;paper.Add(rule);
  body=Scroll(paper);
  var back=new Button(backAction ?? (Action)FilePage){text=T(backAction==null?"back.file":"back.desk")};
  back.style.minHeight=42;back.style.fontSize=Typography.Snap(16);back.style.backgroundColor=new Color(.76f,.69f,.58f);
  back.style.color=dark;
  if(dossierFont!=null)back.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
  paper.Add(back);
 }
 Button ReportChoice(VisualElement parent,string label,bool selected,Action choose) {
  var option=new Button(choose){text=(selected?"✓  ":"□  ")+label};
  option.style.minHeight=40;option.style.fontSize=Typography.Snap(17);
  option.style.unityTextAlign=TextAnchor.MiddleLeft;
  option.style.paddingLeft=12;option.style.marginBottom=4;
  option.style.backgroundColor=selected?new Color(.74f,.60f,.40f):new Color(.82f,.76f,.65f);
  option.style.color=new Color(.13f,.16f,.20f);
  if(dossierFont!=null)option.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
  parent.Add(option);
  return option;
 }
 void RefreshReportChoices(List<Button> buttons,List<string> labels,List<string> ids,string selected) {
  for(int i=0;i<buttons.Count;i++) {
   bool active=ids[i]==selected;
   buttons[i].text=(active?"✓  ":"□  ")+labels[i];
   buttons[i].style.backgroundColor=active?new Color(.74f,.60f,.40f):new Color(.82f,.76f,.65f);
  }
 }
 void Home() {
  StopCctvVideo();
  EnsureScene("MainMenuScene");
  showingInterviewList=false;
  root.Clear();
  var art=Resources.Load<Texture2D>("Bube/MainMenuNight");
  if(art!=null) {
   art.filterMode=FilterMode.Point;
   var backdrop=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   backdrop.style.position=Position.Absolute;
   backdrop.style.left=0;backdrop.style.right=0;backdrop.style.top=0;backdrop.style.bottom=0;
   root.Add(backdrop);
  }
  var left=new VisualElement();left.style.position=Position.Absolute;
  left.style.left=Length.Percent(4);left.style.top=Length.Percent(4);
  left.style.width=Length.Percent(32);left.style.bottom=Length.Percent(3);
  root.Add(left);
  Text(left,"bubeGames",Gold,18);
  var gap=new VisualElement();gap.style.height=25;left.Add(gap);
  var logo=Text(left,"bube",Ink,76);logo.style.marginBottom=0;
  if(dossierBoldFont!=null)logo.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  GlitchHeading(logo,"bube");
  var rule=new VisualElement();rule.style.height=3;rule.style.width=Length.Percent(78);
  rule.style.backgroundColor=Ink;rule.style.marginTop=3;rule.style.marginBottom=10;left.Add(rule);
  Text(left,"P O L I C E",Ink,22);
  var menuGap=new VisualElement();menuGap.style.height=18;left.Add(menuGap);
  if(game.State.caseAccepted)Button(left,"▣  "+T("home.continue")+"   ›",Desk,true);
  if(game.State.caseAccepted)Button(left,"↺  "+T("home.new"),()=>{confirmRestart=true;RestartPage();});
  else Button(left,"↺  "+T("home.new"),()=>MaybeWorldIntro(Desk),true);
  Button(left,"⚙  "+T("menu.settings"),SettingsPage);
  Button(left,"▥  "+T("menu.stats"),StatisticsPage);
  Button(left,"▤  "+T("archive.menu"),ArchivePage);
  Button(left,"ⓘ  "+T("menu.about"),AboutPage);
  FadeIn(left);
 }
 void MenuOverlay(string title,out VisualElement card) {
  Home();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.015f,.025f,.03f,.78f);root.Add(shade);
  card=new VisualElement();card.style.position=Position.Absolute;
  card.style.left=Length.Percent(27);card.style.right=Length.Percent(27);
  card.style.top=Length.Percent(20);card.style.bottom=Length.Percent(20);
  card.style.backgroundColor=Card;card.style.paddingLeft=30;card.style.paddingRight=30;
  card.style.paddingTop=28;card.style.paddingBottom=24;root.Add(card);
  Text(card,title,Gold,29);
 }
 void SettingsPage() {
  VisualElement card;MenuOverlay(T("menu.settings"),out card);
  Text(card,T("settings.textSpeed"),Ink,19);
  Button(card,(instantText?"✓  ":"")+T("settings.instant"),()=>{
   instantText=true;PlayerPrefs.SetInt("bube.instantText",1);PlayerPrefs.Save();SettingsPage();
  },instantText);
  Button(card,(!instantText?"✓  ":"")+T("settings.normal"),()=>{
   instantText=false;PlayerPrefs.SetInt("bube.instantText",0);PlayerPrefs.Save();SettingsPage();
  },!instantText);
  var spacer=new VisualElement();spacer.style.flexGrow=1;card.Add(spacer);
  Button(card,T("offer.back"),Home);
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
    if(progress==null || progress.version!=1 || progress.caseId!=data.id || !progress.closed)continue;
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
  shade.style.backgroundColor=new Color(.02f,.025f,.03f,.82f);root.Add(shade);
  var backing=new VisualElement();backing.style.position=Position.Absolute;
  backing.style.left=Length.Percent(8);backing.style.right=Length.Percent(8);
  backing.style.top=Length.Percent(6);backing.style.bottom=Length.Percent(5);
  backing.style.backgroundColor=new Color(.28f,.17f,.13f);root.Add(backing);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(9);paper.style.right=Length.Percent(9);
  paper.style.top=Length.Percent(5);paper.style.bottom=Length.Percent(6);
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=18;paper.style.paddingBottom=16;
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);root.Add(paper);
  var dark=new Color(.13f,.16f,.20f);
  Text(paper,T("archive.kicker"),new Color(.45f,.29f,.23f),14).style.marginBottom=2;
  Text(paper,heading,dark,27).style.marginBottom=8;
  var line=new VisualElement();line.style.height=1;line.style.backgroundColor=new Color(.58f,.51f,.42f);
  line.style.marginBottom=12;paper.Add(line);
  content=new VisualElement();content.style.flexGrow=1;paper.Add(content);
  return paper;
 }
 void ArchivePage() {
  VisualElement content;
  var paper=ArchivePaper(T("archive.title"),out content);
  var dark=new Color(.13f,.16f,.20f);
  var cases=ClosedCases();
  Text(content,T("archive.count")+"  "+cases.Length,dark,16);
  var list=Scroll(content);
  if(cases.Length==0)Text(list,T("archive.empty"),dark,18);
  foreach(var item in cases) {
   var stamp=item.progress.submittedAtUtcTicks>0
    ?new DateTime(item.progress.submittedAtUtcTicks,DateTimeKind.Utc).ToLocalTime().ToString("dd.MM.yyyy HH:mm")
    :T("summary.unknownDate");
   Button(list,T(item.data.titleKey)+"  ·  "+stamp+"   ›",()=>ArchiveCasePage(item.data.id,null));
  }
  var back=new Button(Home){text=T("offer.back")};back.style.minHeight=45;
  back.style.backgroundColor=new Color(.77f,.69f,.57f);back.style.color=dark;paper.Add(back);
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
   Text(parent,label,new Color(.39f,.36f,.31f),13);
   return;
  }
  var link=new Button(()=>ArchiveCasePage(item.data.id,source.id,reference)){text=label+"  →"};
  link.style.minHeight=42;link.style.whiteSpace=WhiteSpace.Normal;
  link.style.unityTextAlign=TextAnchor.MiddleLeft;
  link.style.fontSize=Typography.Snap(13);link.style.paddingLeft=9;link.style.paddingRight=8;
  link.style.marginBottom=10;
  link.style.backgroundColor=new Color(.82f,.76f,.65f);
  link.style.color=new Color(.13f,.16f,.20f);
  if(dossierFont!=null)link.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
  parent.Add(link);
 }
 void ArchiveCasePage(string caseId,string sourceId,string focusReference=null) {
  var item=ClosedCases().FirstOrDefault(entry=>entry.data.id==caseId);
  if(item==null){ArchivePage();return;}
  var data=item.data;var progress=item.progress;
  VisualElement content;
  var paper=ArchivePaper(T(data.titleKey),out content);
  var dark=new Color(.13f,.16f,.20f);var muted=new Color(.39f,.36f,.31f);
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
  detail.style.borderLeftWidth=1;detail.style.borderLeftColor=new Color(.58f,.51f,.42f);
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
   faxColumn.style.borderLeftColor=new Color(.58f,.51f,.42f);comparison.Add(faxColumn);
   Text(faxColumn,T("archive.fax"),dark,19);
   var fax=game.Career.reviewHistory.FirstOrDefault(review=>review.caseId==caseId);
   if(fax==null)Text(faxColumn,T("archive.pendingReview"),muted,15);
   else {
    Text(faxColumn,T("career.evaluation."+fax.evaluationType),dark,15);
    if(suspect!=null)Text(faxColumn,T("conclude.suspect")+": "+T(fax.suspectSupported?"fax.supported":"fax.unsupported"),dark,15);
    if(method!=null)Text(faxColumn,T("conclude.method")+": "+T(fax.methodSupported?"fax.supported":"fax.unsupported"),dark,15);
    if(proof!=null)Text(faxColumn,T("conclude.evidence")+": "+T(fax.proofSupported?"fax.supported":"fax.unsupported"),dark,15);
    Text(faxColumn,T("career.trust")+": "+T(TrustStatusKey(fax.trustAfter))+
     (fax.trustChange>0?" ↑":fax.trustChange<0?" ↓":""),muted,14);
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
  var back=new Button(ArchivePage){text=T("archive.back")};back.style.minHeight=45;
  back.style.backgroundColor=new Color(.77f,.69f,.57f);back.style.color=dark;paper.Add(back);
 }
 void ArchiveFocus(ScrollView scroll,VisualElement target) {
  target.style.backgroundColor=new Color(.83f,.73f,.52f);
  target.style.paddingLeft=7;target.style.paddingRight=7;
  scroll.schedule.Execute(()=>scroll.ScrollTo(target)).ExecuteLater(1);
 }
 void StatisticsPage() {
  VisualElement card;BpsTablet("menu.stats",out card);
  Text(card,T("career.bora"),Ink,20);
  Text(card,T("career.rank.investigator"),Muted,15);
  var status=Text(card,T(game.TrustStatusKey),Gold,23);
  if(dossierBoldFont!=null)status.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var history=game.Career.reviewHistory;
  Text(card,T("career.record")+"  "+history.Count,Ink,18);
  Text(card,T("career.supportedCount")+"  "+history.Count(r=>r.evaluationType=="supported"),Muted,15);
  Text(card,T("career.incompleteCount")+"  "+history.Count(r=>r.evaluationType=="incomplete"),Muted,15);
  Text(card,T("career.falseCount")+"  "+history.Count(r=>r.evaluationType=="falseAccusation"),Muted,15);
  var pending=game.Career.pendingReviews.Count;
  if(pending>0)Text(card,T("career.pending")+"  "+pending,Muted,15);
  var list=Scroll(card);
  foreach(var review in history.AsEnumerable().Reverse()) {
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   var title=data==null?review.caseId:T(data.titleKey);
   var direction=review.trustChange>0?" ↑":review.trustChange<0?" ↓":" —";
   Button(list,title+"  ·  "+T("career.evaluation."+review.evaluationType)+direction,()=>CareerRecordPage(review));
  }
  Button(card,T("offer.back"),Home);
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
 void MaybeWorldIntro(Action after) {
  var world=(config.worldIntros ?? new WorldIntro[0]).FirstOrDefault(w=>w.firstCaseId==game.Data.id);
  if(world==null || game.Career.seenWorldIntros.Contains(world.id)){after();return;}
  activeIntro=world;
  introAfter=after;
  PlayWorldIntro();
 }
 void PlayWorldIntro() {
  EnsureScene("OfficeScene");
  root.Clear();
  root.style.backgroundColor=Color.black;
  introTexture=new RenderTexture(1280,720,0,RenderTextureFormat.ARGB32);
  introTexture.Create();
  var film=new Image {image=introTexture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  film.style.position=Position.Absolute;
  film.style.left=0;film.style.right=0;film.style.top=0;film.style.bottom=0;
  root.Add(film);
  if(!activeIntro.graphicsEmbedded) {
  introPlace=new VisualElement();
  introPlace.style.position=Position.Absolute;
  introPlace.style.left=Length.Percent(5);introPlace.style.top=Length.Percent(13);
  introPlace.style.flexDirection=FlexDirection.Row;introPlace.style.alignItems=Align.Center;
  introPlace.style.paddingLeft=12;introPlace.style.paddingRight=16;
  introPlace.style.paddingTop=9;introPlace.style.paddingBottom=7;
  introPlace.style.backgroundColor=new Color(.04f,.06f,.08f,.70f);
  introPlace.style.opacity=0;root.Add(introPlace);
  var flag=string.IsNullOrEmpty(activeIntro.flagResource)?null:Resources.Load<Texture2D>(activeIntro.flagResource);
  if(flag!=null) {
   flag.filterMode=FilterMode.Point;
   var emblem=new Image {image=flag,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   emblem.style.width=54;emblem.style.height=36;emblem.style.marginRight=12;
   introPlace.Add(emblem);
  }
  var location=new VisualElement();introPlace.Add(location);
  var country=Text(location,T(activeIntro.countryKey),Ink,18);country.style.marginBottom=0;
  if(dossierBoldFont!=null)country.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  if(!string.IsNullOrEmpty(activeIntro.locationKey)) {
   var city=Text(location,T(activeIntro.locationKey),Muted,12);city.style.marginBottom=0;
  }
  introBrand=new VisualElement();
  introBrand.style.position=Position.Absolute;
  introBrand.style.right=Length.Percent(6);introBrand.style.top=Length.Percent(42);
  introBrand.style.paddingLeft=22;introBrand.style.paddingRight=22;
  introBrand.style.paddingTop=14;introBrand.style.paddingBottom=12;
  introBrand.style.backgroundColor=new Color(.04f,.06f,.08f,.64f);
  introBrand.style.opacity=0;root.Add(introBrand);
  var studio=Text(introBrand,"bubeGames",Ink,31);studio.style.marginBottom=1;
  if(dossierBoldFont!=null)studio.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var credit=Text(introBrand,"powered by bubeDigital",Gold,16);credit.style.marginBottom=0;
  }
  var skip=new Button(FinishWorldIntro){text=T("intro.skip")};
  skip.style.position=Position.Absolute;skip.style.right=Length.Percent(4);
  skip.style.bottom=Length.Percent(5);skip.style.minWidth=150;
  skip.style.minHeight=MinimumTouchTarget;skip.style.backgroundColor=Card;
  skip.style.color=Ink;skip.style.fontSize=Typography.Snap(16);root.Add(skip);
  if(activeIntro.skipCoversCornerMark) {
   introSkip=skip;
   activeMark=activeIntro.skipMark ?? new CornerMark();
   root.RegisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
   skip.schedule.Execute(PositionIntroSkip).StartingIn(0);
  }
  introPlayer=gameObject.AddComponent<VideoPlayer>();
  introPlayer.playOnAwake=false;
  introPlayer.isLooping=false;
  introPlayer.renderMode=VideoRenderMode.RenderTexture;
  introPlayer.targetTexture=introTexture;
  introPlayer.audioOutputMode=VideoAudioOutputMode.Direct;
  introPlayer.source=VideoSource.Url;
  introPlayer.url=Application.streamingAssetsPath+"/"+activeIntro.videoPath;
  introPlayer.prepareCompleted+=OnIntroPrepared;
  introPlayer.loopPointReached+=OnIntroEnded;
  introPlayer.errorReceived+=OnIntroError;
  introPlayer.Prepare();
 }
 void OnIntroGeometryChanged(GeometryChangedEvent evt) { PositionIntroSkip(); }
 // "Gec" dugmesi uretici filigraninin tam ustune oturur: filigran filmin kendi
 // karesine oranli oldugu icin once filmin ekrandaki gercek dikdortgeni bulunur.
 // Film 16:9 olarak taranip kirpildigindan telefonun eni ne olursa olsun dogru yere gelir.
 void PositionIntroSkip() {
  if(introSkip==null || root==null || activeMark==null)return;
  float width=root.resolvedStyle.width, height=root.resolvedStyle.height;
  if(float.IsNaN(width) || float.IsNaN(height) || width<=0 || height<=0)return;
  float scale=Mathf.Max(width/16f,height/9f);
  float filmWidth=16f*scale, filmHeight=9f*scale;
  float offsetX=(width-filmWidth)*.5f;
  float offsetY=(height-filmHeight)*.5f;
  float buttonWidth=Mathf.Max(150f,activeMark.w*filmWidth);
  float buttonHeight=Mathf.Max(MinimumTouchTarget,activeMark.h*filmHeight);
  float centerX=offsetX+activeMark.x*filmWidth;
  float centerY=offsetY+activeMark.y*filmHeight;
  introSkip.style.width=buttonWidth;
  introSkip.style.height=buttonHeight;
  introSkip.style.left=Mathf.Clamp(centerX-buttonWidth*.5f,0f,Mathf.Max(0f,width-buttonWidth));
  introSkip.style.top=Mathf.Clamp(centerY-buttonHeight*.5f,0f,Mathf.Max(0f,height-buttonHeight));
  introSkip.style.right=StyleKeyword.Auto;
  introSkip.style.bottom=StyleKeyword.Auto;
 }
 void OnIntroPrepared(VideoPlayer player) {
  if(player.audioTrackCount>0) {
   player.EnableAudioTrack(0,true);
   player.SetDirectAudioMute(0,false);
   player.SetDirectAudioVolume(0,1f);
  }
  player.Play();
 }
 void OnIntroEnded(VideoPlayer player) { FinishWorldIntro(); }
 void OnIntroError(VideoPlayer player,string message) {
  Debug.LogWarning("World intro video unavailable: "+message);
  FinishWorldIntro();
 }
 void FinishWorldIntro() {
  if(activeIntro==null)return;
  var world=activeIntro;
  var after=introAfter;
  if(introSkip!=null)root.UnregisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
  introSkip=null;
  activeIntro=null;introAfter=null;introBrand=null;introPlace=null;activeMark=null;
  if(introPlayer!=null) {
   introPlayer.prepareCompleted-=OnIntroPrepared;
   introPlayer.loopPointReached-=OnIntroEnded;
   introPlayer.errorReceived-=OnIntroError;
   introPlayer.Stop();Destroy(introPlayer);introPlayer=null;
  }
  if(introTexture!=null){introTexture.Release();Destroy(introTexture);introTexture=null;}
  if(!game.Career.seenWorldIntros.Contains(world.id))game.Career.seenWorldIntros.Add(world.id);
  Save();
  if(!world.deskArrival){after();return;}
  if(!string.IsNullOrEmpty(world.deskArrivalVideo))PlayDeskArrival(world,after);
  else StartCoroutine(FirstDeskArrival(after));
 }
 // Dosyanin masaya birakilisi artik cizilmis bir animasyon degil, sinematik bir
 // video. Video yoksa ya da oynatilamazsa asagidaki elle cizilmis animasyon
 // devreye girer; oyun hicbir kosulda bu andan yoksun kalmaz.
 void PlayDeskArrival(WorldIntro world,Action after) {
  EnsureScene("OfficeScene");
  root.Clear();
  root.style.backgroundColor=Color.black;
  deskArrivalDone=false;
  introTexture=new RenderTexture(1920,1080,0,RenderTextureFormat.ARGB32);
  introTexture.Create();
  var film=new Image { image=introTexture, scaleMode=ScaleMode.ScaleAndCrop, pickingMode=PickingMode.Ignore };
  film.style.position=Position.Absolute;
  film.style.left=0;film.style.right=0;film.style.top=0;film.style.bottom=0;
  root.Add(film);
  // Sinematik karanliga kapanir, masa da karanliktan acilir: iki goruntu
  // birbirine carpmadan, goz acar gibi gecis yapar.
  var fade=new VisualElement();
  fade.style.position=Position.Absolute;
  fade.style.left=0;fade.style.right=0;fade.style.top=0;fade.style.bottom=0;
  fade.style.backgroundColor=Color.black;fade.style.opacity=0;
  fade.pickingMode=PickingMode.Ignore;
  root.Add(fade);
  deskArrivalFade=fade;
  fade.schedule.Execute(()=>{
   if(introPlayer==null || deskArrivalFade==null)return;
   double length=introPlayer.length;
   if(length<=0)return;
   float remaining=(float)(length-introPlayer.time);
   deskArrivalFade.style.opacity=Mathf.Clamp01((DeskArrivalFadeSeconds-remaining)/DeskArrivalFadeSeconds);
  }).Every(16);
  var skip=new Button(()=>FinishDeskArrival(after,false)){text=T("intro.skip")};
  skip.style.position=Position.Absolute;
  skip.style.backgroundColor=Card;skip.style.color=Ink;
  skip.style.fontSize=Typography.Snap(16);
  root.Add(skip);
  introSkip=skip;
  activeMark=world.deskArrivalMark ?? new CornerMark();
  root.RegisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
  skip.schedule.Execute(PositionIntroSkip).StartingIn(0);
  introPlayer=gameObject.AddComponent<VideoPlayer>();
  introPlayer.playOnAwake=false;
  introPlayer.isLooping=false;
  introPlayer.renderMode=VideoRenderMode.RenderTexture;
  introPlayer.targetTexture=introTexture;
  introPlayer.audioOutputMode=VideoAudioOutputMode.Direct;
  introPlayer.source=VideoSource.Url;
  introPlayer.url=Application.streamingAssetsPath+"/"+world.deskArrivalVideo;
  introPlayer.prepareCompleted+=OnIntroPrepared;
  introPlayer.loopPointReached+=_=>FinishDeskArrival(after,false);
  introPlayer.errorReceived+=(_,message)=>{
   Debug.LogWarning("Desk arrival video unavailable: "+message);
   FinishDeskArrival(after,true);
  };
  introPlayer.Prepare();
 }
 // Hem "Gec" dugmesi hem videonun bitisi buraya gelir; bayrak ikinci cagriyi yutar.
 void FinishDeskArrival(Action after,bool fallback) {
  if(deskArrivalDone)return;
  deskArrivalDone=true;
  if(introSkip!=null)root.UnregisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
  introSkip=null;activeMark=null;deskArrivalFade=null;
  if(introPlayer!=null){introPlayer.Stop();Destroy(introPlayer);introPlayer=null;}
  if(introTexture!=null){introTexture.Release();Destroy(introTexture);introTexture=null;}
  if(fallback)StartCoroutine(FirstDeskArrival(after));
  else StartCoroutine(OpenEyes(after));
 }
 // Masa siyahtan yavasca acilir. Kaplayan golge acilma boyunca dokunmalari da
 // tutar, boylece oyuncu goremedigi bir seye basamaz.
 IEnumerator OpenEyes(Action after) {
  after();
  var shade=new VisualElement();
  shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=Color.black;shade.style.opacity=1;
  root.Add(shade);
  float elapsed=0;
  while(elapsed<1.25f) {
   elapsed+=Time.unscaledDeltaTime;
   shade.style.opacity=1-Mathf.SmoothStep(0,1,Mathf.Clamp01(elapsed/1.25f));
   yield return null;
  }
  shade.RemoveFromHierarchy();
 }
 IEnumerator FirstDeskArrival(Action after) {
  // Masanin kendisi arka plandir: ayri bir tam ekran gorsel cizilmez, boylece
  // `Desk()` ust seridi de dahil her sey yerli yerinde kalir. Kaplayan golge
  // animasyon boyunca tiklamalari da tutar.
  Desk();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=Color.black;shade.style.opacity=1;root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(32);folder.style.width=Length.Percent(36);
  folder.style.height=Length.Percent(29);folder.style.top=Length.Percent(-35);
  folder.style.backgroundColor=new Color(.73f,.59f,.41f);
  folder.style.borderBottomWidth=7;folder.style.borderBottomColor=new Color(.31f,.18f,.13f);
  folder.style.paddingLeft=24;folder.style.paddingTop=20;root.Add(folder);
  Text(folder,T("intro.firstFile"),Base,17);
  var label=Text(folder,T(game.Data.titleKey),Base,24);
  if(dossierBoldFont!=null)label.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  float elapsed=0;
  while(elapsed<2.1f) {
   elapsed+=Time.unscaledDeltaTime;
   float reveal=Mathf.SmoothStep(0,1,Mathf.Clamp01(elapsed/.65f));
   shade.style.opacity=1-reveal*.62f;
   float slide=Mathf.SmoothStep(0,1,Mathf.Clamp01((elapsed-.45f)/1.2f));
   folder.style.top=Length.Percent(Mathf.Lerp(-35,48,slide));
   yield return null;
  }
  yield return new WaitForSecondsRealtime(.65f);
  after();
 }
 void RestartPage() {
  if(!confirmRestart){Home();return;}
  Frame(T("home.new"),T("restart.title"),T("restart.body"));
  var card=Panel(root);
  Button(card,T("restart.confirm"),()=>{
   game=new Investigation(Load<CaseData>("Bube/Cases/"+config.initialCase),null,null,careerRules);
   game.Career.activeCaseId=game.Data.id; selectedSuspect=selectedMethod=selectedEvidence=null;
   selectedSuspectSource=selectedMethodSource=selectedEvidenceSource=null;
   Save(); confirmRestart=false; MaybeWorldIntro(Desk);
  },true);
  Button(card,T("restart.cancel"),()=>{confirmRestart=false;Home();});
 }
 void Hotspot(string label,float x,float y,float w,float h,Action action) {
  var button=new Button(action){text=string.Empty,tooltip=label};
  button.style.position=Position.Absolute;
  button.style.left=Length.Percent(x);button.style.top=Length.Percent(y);
  button.style.width=Length.Percent(w);button.style.height=Length.Percent(h);
  button.style.backgroundColor=Color.clear;
  button.style.borderTopWidth=0;button.style.borderBottomWidth=0;
  button.style.borderLeftWidth=0;button.style.borderRightWidth=0;
  button.RegisterCallback<PointerEnterEvent>(_=>button.style.backgroundColor=new Color(1f,.8f,.45f,.12f));
  button.RegisterCallback<PointerLeaveEvent>(_=>button.style.backgroundColor=Color.clear);
  root.Add(button);
 }
 void AddFaxNotice() {
  if(!HasIncomingFax || faxNotice!=null && faxNotice.panel!=null)return;
  var notice=new Button(InboxPage){text=T("inbox.faxNotice")};
  notice.style.position=Position.Absolute;notice.style.left=Length.Percent(1);notice.style.top=Length.Percent(13);
  notice.style.width=Length.Percent(29);notice.style.minHeight=44;
  notice.style.backgroundColor=new Color(.69f,.23f,.18f);notice.style.color=Ink;notice.style.fontSize=Typography.Snap(16);
  if(dossierBoldFont!=null)notice.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  root.Add(notice);faxNotice=notice;
 }
 void AddDocumentNotice() {
  if(!HasIncomingDocument || documentNotice!=null && documentNotice.panel!=null)return;
  var notice=new Button(InboxPage){text=T("inbox.newDocument")};
  notice.style.position=Position.Absolute;notice.style.left=Length.Percent(1);notice.style.top=Length.Percent(21);
  notice.style.width=Length.Percent(29);notice.style.minHeight=44;
  notice.style.backgroundColor=new Color(.69f,.48f,.20f);notice.style.color=Base;notice.style.fontSize=Typography.Snap(16);
  if(dossierBoldFont!=null)notice.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  root.Add(notice);documentNotice=notice;
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
  shade.style.backgroundColor=new Color(.015f,.02f,.025f,.84f);root.Add(shade);
  var binder=new VisualElement();binder.style.position=Position.Absolute;
  binder.style.left=Length.Percent(5);binder.style.right=Length.Percent(5);
  binder.style.top=Length.Percent(6);binder.style.bottom=Length.Percent(6);
  binder.style.flexDirection=FlexDirection.Row;
  binder.style.backgroundColor=new Color(.12f,.11f,.10f);
  binder.style.borderTopWidth=5;binder.style.borderBottomWidth=7;
  binder.style.borderLeftWidth=5;binder.style.borderRightWidth=5;
  binder.style.borderTopColor=new Color(.24f,.18f,.14f);
  binder.style.borderBottomColor=new Color(.08f,.06f,.05f);
  binder.style.borderLeftColor=new Color(.24f,.18f,.14f);
  binder.style.borderRightColor=new Color(.08f,.06f,.05f);
  root.Add(binder);
  var left=new VisualElement();left.style.width=Length.Percent(44);
  left.style.backgroundColor=new Color(.075f,.09f,.10f);
  left.style.paddingLeft=18;left.style.paddingRight=18;
  left.style.paddingTop=14;left.style.paddingBottom=14;
  binder.Add(left);
  var heading=new VisualElement();heading.style.flexDirection=FlexDirection.Row;
  heading.style.alignItems=Align.Center;left.Add(heading);
  var headingText=Text(heading,T("inbox.title"),Ink,23);
  headingText.style.flexGrow=1;headingText.style.marginBottom=3;
  if(dossierBoldFont!=null)headingText.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var close=new Button(Desk){text="×"};close.tooltip=T("back.desk");
  close.style.width=MinimumTouchTarget;close.style.height=MinimumTouchTarget;close.style.fontSize=Typography.Snap(28);
  close.style.backgroundColor=Paper;close.style.color=Ink;heading.Add(close);
  Text(left,T("desk.brandLocation"),Muted,13).style.marginBottom=12;
  var filters=new VisualElement();filters.style.flexDirection=FlexDirection.Row;
  filters.style.marginBottom=12;left.Add(filters);
  foreach(var choice in new[]{"all","unread","archive"}) {
   var selectedFilter=choice;
   var button=new Button(()=>InboxPage(null,selectedFilter)) {text=T("inbox.filter."+choice)};
   button.style.flexGrow=1;button.style.minHeight=MinimumTouchTarget;button.style.marginRight=5;
   button.style.fontSize=Typography.Snap(14);button.style.color=choice==filter?Base:Ink;
   button.style.backgroundColor=choice==filter?Gold:Paper;
   if(dossierFont!=null)button.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
   filters.Add(button);
  }
  var list=Scroll(left);
  if(visible.Length==0) {
   Text(list,filter=="unread"?T("inbox.empty"):T("inbox.noItems"),Muted,18);
   Text(list,T("inbox.emptyHelp"),Muted,14);
  }
  foreach(var item in visible) {
   var current=item;
   var row=new Button(()=>InboxPage(current.id,filter)) {
    text=(item.unread?"●  ":"")+item.title+"\n"+item.status
   };
   row.style.minHeight=78;row.style.marginBottom=7;
   row.style.paddingLeft=13;row.style.paddingRight=8;
   row.style.fontSize=Typography.Snap(16);row.style.whiteSpace=WhiteSpace.Normal;
   row.style.unityTextAlign=TextAnchor.MiddleLeft;
   row.style.color=item.id==selected?.id?Base:Ink;
   row.style.backgroundColor=item.id==selected?.id?new Color(.84f,.72f,.53f):new Color(.13f,.16f,.17f);
   row.style.borderLeftWidth=3;row.style.borderLeftColor=item.unread?Gold:Muted;
   if(dossierFont!=null)row.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
   list.Add(row);
  }
  var right=new VisualElement();right.style.flexGrow=1;
  right.style.paddingLeft=15;right.style.paddingRight=15;
  right.style.paddingTop=13;right.style.paddingBottom=13;
  right.style.backgroundColor=new Color(.16f,.12f,.10f);
  binder.Add(right);
  var paper=new VisualElement();paper.style.flexGrow=1;
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);
  paper.style.paddingLeft=22;paper.style.paddingRight=22;
  paper.style.paddingTop=17;paper.style.paddingBottom=14;
  right.Add(paper);
  var paperBody=Scroll(paper);
  var dark=new Color(.13f,.16f,.20f);
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
  line.style.backgroundColor=new Color(.58f,.51f,.43f);paperBody.Add(line);
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
 }
 void DrawFaxFinding(VisualElement body,CaseData data,string headingKey,string choiceKey,string sourceId,bool supported,string claim,Color dark) {
  var block=new VisualElement();block.style.marginTop=7;block.style.marginBottom=8;
  block.style.paddingLeft=12;block.style.paddingRight=12;
  block.style.paddingTop=9;block.style.paddingBottom=6;
  block.style.backgroundColor=new Color(.84f,.78f,.67f);
  block.style.borderLeftWidth=3;
  block.style.borderLeftColor=supported?new Color(.22f,.43f,.34f):new Color(.55f,.34f,.25f);
  body.Add(block);
  Text(block,T(headingKey)+"  ·  "+T(supported?"fax.supported":"fax.unsupported"),dark,16).style.marginBottom=3;
  Text(block,T(choiceKey),dark,16).style.marginBottom=3;
  Text(block,T("fax.submittedSource")+": "+ReviewSourceTitle(data,sourceId),dark,14).style.marginBottom=4;
  var sourceIdOnly=string.IsNullOrEmpty(sourceId)?string.Empty:sourceId.Split('#')[0];
  var reasonKey=supported?"fax.reason.supported":sourceIdOnly=="report"?"fax.reason."+claim+".report":"fax.reason."+claim+".other";
  Text(block,T(reasonKey),new Color(.35f,.31f,.27f),14).style.marginBottom=0;
 }
 void Desk() {
  StopCctvVideo();
  EnsureScene("OfficeScene");
  showingInterviewList=false;
  showingInvestigationRequests=false;
  root.Clear();
  var texture=Resources.Load<Texture2D>("Bube/DeskReference");
  if(texture==null) {
   Frame(T("desk"),T("desk.title"),T("desk.subtitle"));
   Button(root,T("desk.open"),FilePage,true);Button(root,T("back.home"),Home);return;
  }
  var image=new Image { image=texture, scaleMode=ScaleMode.ScaleAndCrop, pickingMode=PickingMode.Ignore };
  image.style.position=Position.Absolute;
  image.style.left=0;image.style.top=0;image.style.right=0;image.style.bottom=0;
  root.Add(image);
  var badge=new VisualElement();
  badge.style.position=Position.Absolute;
  badge.style.left=Length.Percent(32.5f);badge.style.top=Length.Percent(31.6f);
  badge.style.width=Length.Percent(2.6f);badge.style.height=Length.Percent(4.1f);
  badge.style.backgroundColor=new Color(.72f,.20f,.19f);
  badge.style.borderTopLeftRadius=4;badge.style.borderTopRightRadius=4;
  badge.style.borderBottomLeftRadius=4;badge.style.borderBottomRightRadius=4;
  badge.pickingMode=PickingMode.Ignore;
  root.Add(badge);
  inboxBadge=badge;
  inboxBadgeLabel=Text(badge,string.Empty,Color.white,15);
  inboxBadgeLabel.style.unityTextAlign=TextAnchor.MiddleCenter;
  inboxBadgeLabel.style.marginBottom=0;
  inboxBadgeLabel.style.flexGrow=1;
  inboxBadgeLabel.pickingMode=PickingMode.Ignore;
  RefreshInboxBadge();
  // Okunmamis evrak varken rozet yanip soner. Zamanlayici rozetin paneline bagli
  // oldugu icin ekran degisince kendiliginden durur.
  badge.schedule.Execute(()=>{
   if(inboxBadge==null)return;
   bool dim=inboxBadge.style.opacity.value>.6f;
   inboxBadge.style.opacity=dim?.3f:1f;
  }).Every(520);
  var header=new VisualElement();
  header.style.position=Position.Absolute;header.style.left=0;header.style.right=0;
  header.style.top=0;header.style.height=Length.Percent(11);
  header.style.backgroundColor=new Color(.055f,.075f,.09f,1f);
  header.style.paddingLeft=36;header.style.paddingTop=12;
  root.Add(header);
  var brand=Text(header,T("desk.brandLocation"),Ink,21);brand.style.marginBottom=2;
  var caseTitle=Text(header,T(game.Data.titleKey),Gold,14);caseTitle.style.marginBottom=0;
  var patch=new VisualElement();
  patch.style.position=Position.Absolute;patch.style.left=Length.Percent(44);
  patch.style.top=Length.Percent(79);patch.style.width=Length.Percent(17);
  patch.style.height=Length.Percent(7);
  patch.style.backgroundColor=new Color(.67f,.53f,.40f);
  patch.style.unityTextAlign=TextAnchor.MiddleCenter;
  root.Add(patch);
  Text(patch,T("desk.location"),Base,16);
  if(game.Career.retired) {
   var end=Panel(root);end.style.position=Position.Absolute;end.style.left=Length.Percent(30);end.style.top=Length.Percent(42);
   Text(end,T("career.endedTitle"),Gold,24);Text(end,T("career.ended"),Ink,17);
   if(game.State.closed)Button(end,T("result.summaryOpen"),CaseSummary);
  } else if(!game.State.caseAccepted) {
   // Dosya kabul edilene kadar masadaki tek etkilesim gelen evrak tepsisidir;
   // oyuncuya sirada ne yapacagi soylenmez, yalnizca evrak fark edilir.
   Hotspot(T("desk.inbox"),16,16,22,27,InboxPage);
  } else if(!game.State.closed) {
   Hotspot(T("desk.inbox"),16,16,22,27,InboxPage);
   Hotspot(T(game.Data.titleKey),36,42,30,51,FilePage);
   Hotspot(T("kind.interview"),6,52,20,38,()=>InterviewRequests());
   Hotspot(T(game.Data.nodes.Any(n=>n.kind=="cctv")?"kind.cctv":"kind.bps"),64,11,33,36,OpenTerminal);
  } else {
   var closed=Panel(root);closed.style.position=Position.Absolute;
   closed.style.left=Length.Percent(38);closed.style.top=Length.Percent(51);
   Text(closed,T("desk.closed"),Ink,20);
   Button(closed,T("result.summaryOpen"),CaseSummary,true);
   Button(closed,T("result.continue"),ContinueToNextCase);
   if(HasIncomingFax || HasIncomingDocument || AvailableAssignment()!=null)Hotspot(T("desk.inbox"),16,16,22,27,InboxPage);
  }
  if(HasIncomingFax)AddFaxNotice();
  if(HasIncomingDocument)AddDocumentNotice();
  if(game.State.interviewTurns.Count>game.State.seenInterviewTurns && !game.State.closed) {
   var unread=Text(root,T("file.newTranscript"),Gold,15);
   unread.style.position=Position.Absolute;
   unread.style.left=Length.Percent(46);unread.style.top=Length.Percent(44);
   unread.style.backgroundColor=Base;
   unread.style.paddingLeft=7;unread.style.paddingRight=7;
   unread.pickingMode=PickingMode.Ignore;
  }
  Hotspot(T("back.home"),90,0,10,12,Home);
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
   tablet.style.backgroundColor=new Color(.035f,.055f,.075f);
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
  var brand=Text(header,"BPS",Ink,29);brand.style.marginRight=16;brand.style.marginBottom=0;
  var title=Text(header,T(game.Data.titleKey)+" / "+T(titleKey),Ink,17);title.style.flexGrow=1;title.style.marginBottom=0;
  GlitchHeading(title,T(titleKey));
  var close=new Button(Desk){text="×"};close.tooltip=T("cctv.back");
  close.style.width=MinimumTouchTarget;close.style.height=MinimumTouchTarget;close.style.fontSize=Typography.Snap(26);
  close.style.backgroundColor=Paper;close.style.color=Ink;
  header.Add(close);
  var rule=new VisualElement();rule.style.height=2;rule.style.backgroundColor=new Color(.25f,.35f,.42f);
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
  var tabs=new VisualElement();tabs.style.flexDirection=FlexDirection.Row;
  tabs.style.marginBottom=8;content.Add(tabs);
  foreach(var source in sources) {
   var node=source;
   var button=new Button(()=>{if(node.kind=="cctv")CctvScreen(node);else ReadPage(node);}){text=T(node.titleKey)};
   button.style.flexGrow=1;button.style.minHeight=MinimumTouchTarget;button.style.fontSize=Typography.Snap(15);
   button.style.whiteSpace=WhiteSpace.Normal;
   button.style.backgroundColor=node.id==selected.id?Gold:Paper;
   button.style.color=node.id==selected.id?Base:Ink;
   if(dossierFont!=null)button.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);
   tabs.Add(button);
  }
 }
 void RequestTabs(VisualElement content,bool interviews) {
  var tabs=new VisualElement();tabs.style.flexDirection=FlexDirection.Row;
  tabs.style.marginBottom=10;content.Add(tabs);
  var interviewTab=new Button(()=>InterviewRequests(false)){text=T("tablet.tab.interviews")};
  var documentTab=new Button(()=>InvestigationRequests(false)){text=T("tablet.tab.investigations")};
  foreach(var tab in new[]{interviewTab,documentTab}) {
   bool active=tab==interviewTab?interviews:!interviews;
   tab.style.flexGrow=1;tab.style.minHeight=MinimumTouchTarget;
   tab.style.marginRight=7;tab.style.fontSize=Typography.Snap(16);
   tab.style.backgroundColor=active?Gold:Paper;
   tab.style.color=active?Base:Ink;
   if(dossierBoldFont!=null)tab.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
   tabs.Add(tab);
  }
 }
 void InterviewRequests(bool lift=true) {
  lastPendingCount=game.Data.nodes.Count(pendingPredicate);
  VisualElement content;
  BpsTablet("tablet.interviews",out content,lift);
  showingInterviewList=true;
  RequestTabs(content,true);
  var intro=new VisualElement();intro.style.flexDirection=FlexDirection.Row;
  intro.style.alignItems=Align.Center;intro.style.marginBottom=8;content.Add(intro);
  var caseLine=Text(intro,CaseText("tablet.caseLine","tablet.caseLine"),Gold,14);
  caseLine.style.marginBottom=0;caseLine.style.marginRight=20;
  var help=Text(intro,T("tablet.interviewHelp"),Muted,14);
  help.style.marginBottom=0;
  var list=Scroll(content);
  var groups=game.Data.nodes.Where(n=>n.kind=="interview" && game.Discovered(n))
   .GroupBy(n=>n.personId).ToArray();
  if(groups.Length==0)Text(list,T("desk.none"),Muted,19);
  foreach(var group in groups) {
   var node=group.FirstOrDefault(n=>!game.State.read.Contains(n.id)) ?? group.Last();
   var quoted=group.LastOrDefault(n=>game.State.read.Contains(n.id) && !string.IsNullOrEmpty(n.personQuoteKey));
   var card=new VisualElement();card.style.flexDirection=FlexDirection.Row;
   card.style.alignItems=Align.Center;
   card.style.minHeight=116;card.style.marginBottom=10;
   card.style.paddingLeft=12;card.style.paddingRight=12;
   card.style.paddingTop=8;card.style.paddingBottom=8;
   card.style.backgroundColor=new Color(.075f,.105f,.11f);
   card.style.borderTopWidth=1;card.style.borderBottomWidth=1;
   card.style.borderLeftWidth=1;card.style.borderRightWidth=1;
   card.style.borderTopColor=Muted;card.style.borderBottomColor=Muted;
   card.style.borderLeftColor=Muted;card.style.borderRightColor=Muted;
   list.Add(card);
   var portrait=Resources.Load<Texture2D>("Bube/Characters/"+node.personId);
   if(portrait!=null) {
    portrait.filterMode=FilterMode.Point;
    var thumb=new Image {image=portrait,scaleMode=ScaleMode.ScaleToFit};
    thumb.style.width=100;thumb.style.height=100;thumb.style.marginRight=16;
    thumb.style.backgroundColor=Base;card.Add(thumb);
   }
   var details=new VisualElement();details.style.flexGrow=1;card.Add(details);
   var name=Text(details,T(node.personNameKey),Ink,22);name.style.marginBottom=2;
   var info=Text(details,T(node.personInfoKey),Muted,14);info.style.marginBottom=6;
   var quote=Text(details,quoted!=null?"“"+T(quoted.personQuoteKey)+"”":T("interview.noStatement"),Muted,15);
   quote.style.marginBottom=0;
   var action=new VisualElement();action.style.width=Length.Percent(30);
   action.style.paddingLeft=12;card.Add(action);
   string statusKey=game.CanRequest(node)?"interview.status.unrequested":game.Pending(node)?"interview.pending":game.State.read.Contains(node.id)?
    ((node.questions ?? new Question[0]).Any(q=>game.CanAskQuestion(node,q))?"interview.status.followup":"interview.status.complete"):"interview.status.ready";
   var status=Text(action,T(statusKey),game.Pending(node)?Gold:Muted,14);
   status.style.marginBottom=8;
   if(game.CanRequest(node))Button(action,T("interview.request"),()=>{
    if(game.RequestInterview(node.id)){Save();InterviewRequests(false);}
   });
   else if(game.Available(node))Button(action,T("interview.begin"),()=>InterviewPage(node),true);
  }
 }
 void InvestigationRequests(bool lift=true) {
  lastIncomingDocumentCount=game.Data.nodes.Count(incomingDocumentPredicate);
  VisualElement content;
  BpsTablet("tablet.investigations",out content,lift);
  showingInvestigationRequests=true;
  RequestTabs(content,false);
  Text(content,CaseText("tablet.caseLine","tablet.caseLine"),Gold,14).style.marginBottom=5;
  Text(content,T("tablet.investigationHelp"),Muted,15).style.marginBottom=9;
  var list=Scroll(content);
  var documents=game.Data.nodes.Where(n=>n.kind=="document" && n.requestable &&
   (game.Discovered(n) || game.State.documentRequests.Any(r=>r.nodeId==n.id))).ToArray();
  if(documents.Length==0)Text(list,T("tablet.noInvestigations"),Muted,18);
  foreach(var node in documents) {
   var current=node;
   var card=new VisualElement();card.style.marginBottom=10;
   card.style.paddingLeft=15;card.style.paddingRight=15;
   card.style.paddingTop=11;card.style.paddingBottom=10;
   card.style.backgroundColor=new Color(.075f,.105f,.11f);
   card.style.borderLeftWidth=3;card.style.borderLeftColor=Gold;
   list.Add(card);
   Text(card,T(node.titleKey),Ink,21).style.marginBottom=4;
   bool filed=game.State.read.Contains(node.id);
   bool arrived=game.IncomingDocument(node);
   bool requested=game.State.documentRequests.Any(r=>r.nodeId==node.id);
   string statusKey=filed?"tablet.investigationFiled":arrived?"tablet.investigationArrived":
    requested?"tablet.investigationPending":"tablet.investigationAvailable";
   Text(card,T(statusKey),filed?Muted:arrived?Gold:Muted,15).style.marginBottom=8;
   if(game.CanRequestDocument(node)) {
    var request=new Button(()=>{
     if(game.RequestDocument(current.id)){Save();InvestigationRequests(false);}
    }){text=T(node.requestLabelKey)};
    request.style.minHeight=MinimumTouchTarget;request.style.fontSize=Typography.Snap(16);
    request.style.backgroundColor=Gold;request.style.color=Base;
    if(dossierBoldFont!=null)request.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
    card.Add(request);
   }
  }
 }
 void TimelineRow(VisualElement parent,TimelineClue clue,Color ink,Color muted,string actionKey,Action action) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;row.style.minHeight=70;
  row.style.marginBottom=6;row.style.paddingLeft=9;row.style.paddingRight=8;
  row.style.backgroundColor=new Color(.85f,.79f,.68f);
  row.style.borderLeftWidth=3;row.style.borderLeftColor=new Color(.47f,.35f,.25f);
  parent.Add(row);
  var time=Text(row,T(clue.timeKey),ink,17);time.style.width=112;time.style.marginBottom=0;
  var details=new VisualElement();details.style.flexGrow=1;row.Add(details);
  Text(details,T(clue.noteKey),ink,15).style.marginBottom=2;
  Text(details,T(clue.sourceKey),muted,12).style.marginBottom=0;
  if(action==null)return;
  var button=new Button(action){text=T(actionKey)};
  button.style.width=80;button.style.minHeight=40;
  button.style.marginLeft=7;button.style.fontSize=Typography.Snap(13);
  button.style.backgroundColor=new Color(.30f,.23f,.19f);button.style.color=Ink;
  if(dossierBoldFont!=null)button.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  row.Add(button);
 }
 void TimelineContents(VisualElement paper,Color ink,Color muted) {
  var title=Text(paper,T("timeline.title"),ink,21);
  title.style.marginBottom=4;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(paper,T("timeline.help"),muted,14).style.marginBottom=10;
  var scroll=Scroll(paper);
  Text(scroll,T("timeline.pinned"),ink,17).style.marginBottom=5;
  var pinnedList=new VisualElement();scroll.Add(pinnedList);
  var divider=new VisualElement();divider.style.height=1;divider.style.marginTop=14;
  divider.style.marginBottom=12;divider.style.backgroundColor=new Color(.62f,.57f,.48f);scroll.Add(divider);
  Text(scroll,T("timeline.available"),ink,17).style.marginBottom=5;
  var availableList=new VisualElement();scroll.Add(availableList);
  Action refresh=null;
  refresh=()=>{
   pinnedList.Clear();availableList.Clear();
   var clues=game.Data.timelineClues ?? new TimelineClue[0];
   var pinned=clues.Where(c=>game.State.timelinePinned.Contains(c.id)).OrderBy(c=>c.sortMinute).ThenBy(c=>c.id).ToArray();
   if(pinned.Length==0)Text(pinnedList,T("timeline.empty"),muted,15);
   foreach(var clue in pinned) {
    var id=clue.id;
    TimelineRow(pinnedList,clue,ink,muted,game.State.closed?null:"timeline.remove",game.State.closed?null:(Action)(()=>{
     if(game.UnpinTimeline(id)){Save();refresh();}
    }));
   }
   var available=game.State.closed?new TimelineClue[0]:clues.Where(c=>game.TimelineAvailable(c) && !game.State.timelinePinned.Contains(c.id))
    .OrderBy(c=>c.sortMinute).ThenBy(c=>c.id).ToArray();
   if(available.Length==0)Text(availableList,T("timeline.noCandidates"),muted,15);
   foreach(var clue in available) {
    var id=clue.id;
    TimelineRow(availableList,clue,ink,muted,"timeline.add",()=>{
     if(game.PinTimeline(id)){Save();refresh();}
    });
   }
  };
  refresh();
 }
 void FilePage() {
  showingInterviewList=false;
  var report=game.Data.nodes.First(n=>n.id=="report");
  var all=game.Data.nodes.Where(n=>game.Available(n) || game.State.closed && (game.State.read.Contains(n.id) || game.State.interviewTurns.Any(turn=>turn.nodeId==n.id))).ToArray();
  var interviewPages=all.Where(n=>n.kind=="interview" && (game.State.read.Contains(n.id) || game.State.interviewTurns.Any(turn=>turn.nodeId==n.id))).ToArray();
  var evidencePages=all.Where(n=>n.kind=="document" && n.id!=report.id).ToArray();
  Node[] pages=selectedFileSection=="interview"?interviewPages:selectedFileSection=="evidence"?evidencePages:selectedFileSection=="timeline"?new Node[0]:new[]{report};
  var current=pages.FirstOrDefault(n=>n.id==selectedFileNode) ?? pages.FirstOrDefault();
  if(current!=null) {
   selectedFileNode=current.id;
   if(!game.State.read.Contains(current.id) && game.Read(current.id))Save();
  }
  if(selectedFileSection=="interview" && game.State.seenInterviewTurns!=game.State.interviewTurns.Count) {
   game.State.seenInterviewTurns=game.State.interviewTurns.Count;
   Save();
  }
  Desk();
  var shade=new VisualElement();
  shade.style.position=Position.Absolute;shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.025f,.025f,.025f,.78f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(14);folder.style.right=Length.Percent(21);
  folder.style.top=Length.Percent(9);folder.style.bottom=Length.Percent(8);
  folder.style.backgroundColor=new Color(.30f,.16f,.14f);
  folder.style.borderBottomWidth=8;folder.style.borderBottomColor=new Color(.13f,.09f,.08f);
  root.Add(folder);
  for(int i=0;i<3;i++) {
   var sheet=new VisualElement();sheet.style.position=Position.Absolute;
   sheet.style.left=Length.Percent(15+i*.35f);sheet.style.right=Length.Percent(22-i*.35f);
   sheet.style.top=Length.Percent(8+i*.55f);sheet.style.bottom=Length.Percent(8-i*.55f);
   sheet.style.backgroundColor=new Color(.72f,.65f,.53f);root.Add(sheet);
  }
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(16);paper.style.right=Length.Percent(23);
  paper.style.top=Length.Percent(7);paper.style.bottom=Length.Percent(9);
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);
  paper.style.paddingLeft=32;paper.style.paddingRight=30;paper.style.paddingTop=22;paper.style.paddingBottom=15;
  paper.style.borderLeftWidth=2;paper.style.borderTopWidth=2;
  paper.style.borderLeftColor=new Color(.98f,.93f,.83f);paper.style.borderTopColor=new Color(.98f,.93f,.83f);
  root.Add(paper);
  var fileInk=new Color(.13f,.16f,.20f);var fileMuted=new Color(.33f,.33f,.32f);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;header.style.marginBottom=12;paper.Add(header);
  var titles=new VisualElement();titles.style.flexGrow=1;header.Add(titles);
  var title=Text(titles,T(game.Data.titleKey),fileInk,26);title.style.marginBottom=2;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(titles,CaseText("file.caseType","file.caseType"),fileInk,17);
  var stamp=Text(header,T("file.stamp"),fileMuted,14);stamp.style.unityTextAlign=TextAnchor.UpperRight;
  var line=new VisualElement();line.style.height=1;line.style.backgroundColor=new Color(.62f,.57f,.48f);line.style.marginBottom=15;paper.Add(line);
  if(selectedFileSection=="timeline") {
   TimelineContents(paper,fileInk,fileMuted);
  } else if(selectedFileSection=="visual") {
   Text(paper,T("file.visuals"),fileInk,22);
   var visual=Resources.Load<Texture2D>(report.imageResource);
   if(visual!=null) {
    var img=new Image{image=visual,scaleMode=ScaleMode.ScaleToFit};img.style.flexGrow=1;paper.Add(img);
    Text(paper,T(report.imageCaptionKey),fileMuted,15);
   } else Text(paper,T("file.emptyVisual"),fileMuted,18);
  } else if(current==null) {
   Text(paper,T(selectedFileSection=="interview"?"file.emptyInterview":"file.emptyEvidence"),fileMuted,19);
   var filler=new VisualElement();filler.style.flexGrow=1;paper.Add(filler);
  } else {
   var heading=Text(paper,T(current.titleKey).ToUpperInvariant(),fileInk,19);
   if(dossierBoldFont!=null)heading.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
   var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.flexGrow=1;paper.Add(row);
   var textColumn=new VisualElement();textColumn.style.flexGrow=1;textColumn.style.flexBasis=0;row.Add(textColumn);
   if(current.fileMeta!=null)foreach(var field in current.fileMeta) {
    var meta=Text(textColumn,T(field.labelKey)+"  :  "+T(field.valueKey),fileInk,14);meta.style.marginBottom=5;
   }
   if(current.fileMeta!=null && current.fileMeta.Length>0) {
    var divider=new VisualElement();divider.style.height=1;divider.style.marginTop=9;divider.style.marginBottom=15;
    divider.style.backgroundColor=new Color(.62f,.57f,.48f);textColumn.Add(divider);
   }
   var body=Scroll(textColumn);
   Text(body,T(current.bodyKey),fileInk,17);
   if(current.kind=="interview") {
    var turns=game.State.interviewTurns.Where(turn=>turn.nodeId==current.id).ToArray();
    if(turns.Length>0) {
     var transcriptTitle=Text(body,T("file.transcript"),fileInk,18);
     transcriptTitle.style.marginTop=16;
     for(int turnIndex=0;turnIndex<turns.Length;turnIndex++) {
      var turn=turns[turnIndex];
      Text(body,(turnIndex+1)+"  "+T("interview.bora"),fileMuted,14);
      Text(body,T(turn.promptKey),fileInk,16);
      if(!string.IsNullOrEmpty(turn.sourceId)) {
       Text(body,T("interview.presented")+"  "+ReviewSourceTitle(game.Data,turn.sourceId),fileMuted,14);
      }
      Text(body,T(current.personNameKey),fileMuted,14);
      var answer=Text(body,T(turn.answerKey),fileInk,16);
      answer.style.marginBottom=13;
      if(game.InterviewTurnReference(turn)==selectedSearchTurn) {
       selectedSearchTurn=null;
       body.schedule.Execute(()=>body.ScrollTo(answer));
      }
     }
    } else Text(body,T("file.noTranscript"),fileMuted,15);
   }
   if(!string.IsNullOrEmpty(current.imageResource)) {
    var imageColumn=new VisualElement();imageColumn.style.flexGrow=1;imageColumn.style.flexBasis=0;
    imageColumn.style.paddingLeft=20;row.Add(imageColumn);
    var texture=Resources.Load<Texture2D>(current.imageResource);
    if(texture!=null) {
     var photo=new Image{image=texture,scaleMode=ScaleMode.ScaleAndCrop};
     photo.style.height=Length.Percent(73);imageColumn.Add(photo);
     var caption=Text(imageColumn,T(current.imageCaptionKey),fileMuted,13);
     caption.style.unityTextAlign=TextAnchor.MiddleRight;
    }
   }
  }
  if(selectedFileSection!="timeline") {
   var footerLine=new VisualElement();footerLine.style.height=1;footerLine.style.backgroundColor=new Color(.62f,.57f,.48f);paper.Add(footerLine);
   var footer=new VisualElement();footer.style.flexDirection=FlexDirection.Row;footer.style.alignItems=Align.Center;paper.Add(footer);
   var index=current==null?-1:Array.IndexOf(pages,current);
   Text(footer,index<0?"—":(index+1)+" / "+pages.Length,fileMuted,14);
   var spacer=new VisualElement();spacer.style.flexGrow=1;footer.Add(spacer);
   if(index>0)Button(footer,T("file.previous"),()=>{selectedFileNode=pages[index-1].id;FilePage();});
   if(index>=0 && index<pages.Length-1)Button(footer,T("file.next"),()=>{selectedFileNode=pages[index+1].id;FilePage();},true);
  }
  var tabs=new ScrollView(ScrollViewMode.Vertical);tabs.style.position=Position.Absolute;
  tabs.style.left=Length.Percent(77);tabs.style.top=Length.Percent(18);
  tabs.style.width=Length.Percent(13);tabs.style.bottom=Length.Percent(7);
  tabs.verticalScrollerVisibility=ScrollerVisibility.Auto;root.Add(tabs);
  foreach(var section in new[]{"report","interview","evidence","timeline","visual"}) {
   var choice=section;
   var unread=choice=="interview" && game.State.interviewTurns.Count>game.State.seenInterviewTurns;
   var tab=new Button(()=>{selectedFileSection=choice;FilePage();}) {text=T("file.tab."+choice)+(unread?"  •":"")};
   tab.style.minHeight=54;tab.style.marginBottom=5;tab.style.paddingLeft=10;
   tab.style.whiteSpace=WhiteSpace.Normal;tab.style.unityTextAlign=TextAnchor.MiddleLeft;
   tab.style.backgroundColor=choice==selectedFileSection?new Color(.91f,.85f,.73f):new Color(.49f,.46f,.42f);
   tab.style.color=fileInk;tab.style.fontSize=Typography.Snap(15);
   if(dossierBoldFont!=null)tab.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
   tabs.Add(tab);
  }
  var compareTab=new Button(()=>{comparePicker=-1;ComparePage();}) {text=T("file.tab.compare")};
  compareTab.style.minHeight=54;compareTab.style.marginBottom=5;compareTab.style.paddingLeft=10;
  compareTab.style.whiteSpace=WhiteSpace.Normal;compareTab.style.unityTextAlign=TextAnchor.MiddleLeft;
  compareTab.style.backgroundColor=new Color(.49f,.46f,.42f);compareTab.style.color=fileInk;compareTab.style.fontSize=Typography.Snap(15);
  if(dossierBoldFont!=null)compareTab.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  tabs.Add(compareTab);
  var searchTab=new Button(FileSearchPage){text=T("file.tab.search")};
  searchTab.style.minHeight=54;searchTab.style.marginBottom=5;searchTab.style.paddingLeft=10;
  searchTab.style.whiteSpace=WhiteSpace.Normal;searchTab.style.unityTextAlign=TextAnchor.MiddleLeft;
  searchTab.style.backgroundColor=new Color(.49f,.46f,.42f);searchTab.style.color=fileInk;searchTab.style.fontSize=Typography.Snap(15);
  if(dossierBoldFont!=null)searchTab.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  tabs.Add(searchTab);
  if(game.CanConclude) {
   var reportTab=new Button(Conclusion){text=T("conclude.tab")};
   reportTab.style.minHeight=54;reportTab.style.marginBottom=5;reportTab.style.paddingLeft=10;
   reportTab.style.backgroundColor=new Color(.79f,.63f,.40f);reportTab.style.color=fileInk;
   reportTab.style.unityTextAlign=TextAnchor.MiddleLeft;reportTab.style.fontSize=Typography.Snap(15);
   if(dossierBoldFont!=null)reportTab.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
   tabs.Add(reportTab);
  }
  var close=new Button(Desk){text="×"};close.tooltip=T("back.desk");
  close.style.position=Position.Absolute;close.style.right=Length.Percent(8);close.style.top=Length.Percent(7);
  close.style.width=58;close.style.height=58;close.style.fontSize=Typography.Snap(36);
  close.style.color=Ink;close.style.backgroundColor=new Color(.08f,.09f,.10f);root.Add(close);
 }
 void FileSearchPage() {
  showingInterviewList=false;
  Desk();
  var dark=new Color(.13f,.16f,.20f);
  var muted=new Color(.36f,.35f,.33f);
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.025f,.025f,.025f,.82f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(11);folder.style.right=Length.Percent(11);
  folder.style.top=Length.Percent(5);folder.style.bottom=Length.Percent(5);
  folder.style.backgroundColor=new Color(.29f,.16f,.14f);root.Add(folder);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(12);paper.style.right=Length.Percent(12);
  paper.style.top=Length.Percent(6);paper.style.bottom=Length.Percent(7);
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=21;paper.style.paddingBottom=18;root.Add(paper);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;paper.Add(header);
  var title=Text(header,T("search.title"),dark,24);title.style.flexGrow=1;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var back=new Button(FilePage){text=T("compare.back")};back.style.minHeight=43;
  back.style.paddingLeft=12;back.style.paddingRight=12;back.style.color=Ink;
  back.style.backgroundColor=new Color(.30f,.19f,.17f);header.Add(back);
  Text(paper,T("search.help"),muted,15);
  var input=new TextField();input.value=fileSearchQuery;
  input.style.height=51;input.style.marginTop=5;input.style.marginBottom=14;
  input.style.paddingLeft=10;input.style.fontSize=Typography.Snap(20);input.style.color=dark;
  input.style.backgroundColor=new Color(.98f,.94f,.85f);paper.Add(input);
  var count=Text(paper,"",muted,14);count.style.marginBottom=6;
  var results=Scroll(paper);
  Action<string> render=value=>{
   fileSearchQuery=value ?? "";
   results.Clear();
   if(fileSearchQuery.Trim().Length<2) {
    count.text="";Text(results,T("search.enter"),muted,16);return;
   }
   var hits=CaseSearch.Find(game,locale,fileSearchQuery);
   count.text=T("search.count")+"  "+hits.Length;
   if(hits.Length==0){Text(results,T("search.empty"),muted,16);return;}
   foreach(var hit in hits) {
    var source=game.Data.nodes.FirstOrDefault(n=>n.id==hit.nodeId);
    if(source==null)continue;
    var selected=hit;
    var row=new VisualElement();row.style.marginBottom=8;row.style.paddingLeft=14;
    row.style.paddingRight=14;row.style.paddingTop=10;row.style.paddingBottom=8;
    row.style.backgroundColor=new Color(.83f,.77f,.67f);results.Add(row);
    var name=Text(row,T(source.titleKey),dark,16);name.style.marginBottom=3;
    if(dossierBoldFont!=null)name.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
    var excerpt=Text(row,hit.excerpt,dark,15);excerpt.style.marginBottom=4;
    var open=new Button(()=>OpenSearchSource(selected)){text=T("search.open")+"  ›"};
    open.style.minHeight=37;open.style.fontSize=Typography.Snap(15);open.style.unityTextAlign=TextAnchor.MiddleRight;
    open.style.color=dark;open.style.backgroundColor=new Color(.91f,.85f,.73f);row.Add(open);
   }
  };
  input.RegisterValueChangedCallback(evt=>render(evt.newValue));
  render(fileSearchQuery);
 }
 void OpenSearchSource(SearchHit hit) {
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==hit.nodeId);
  if(source==null)return;
  if(source.kind=="cctv")CctvScreen(source,hit.eventId);
  else if(source.kind=="bps")ReadPage(source);
  else {
   selectedSearchTurn=hit.turnReference;
   selectedFileSection=source.kind=="interview"?"interview":source.id=="report"?"report":"evidence";
   selectedFileNode=source.id;FilePage();
  }
 }
 Node[] ComparisonSources() => game.Data.nodes.Where(n=>
  (n.kind=="document" || n.kind=="cctv" || n.kind=="bps") && game.State.read.Contains(n.id)
  || n.kind=="interview" && (game.State.read.Contains(n.id) || game.State.interviewTurns.Any(t=>t.nodeId==n.id))
 ).ToArray();
 void ComparePage() {
  showingInterviewList=false;
  var sources=ComparisonSources();
  if(!sources.Any(n=>n.id==compareLeftId))compareLeftId=null;
  if(!sources.Any(n=>n.id==compareRightId))compareRightId=null;
  Desk();
  var ink=new Color(.13f,.16f,.20f);
  var muted=new Color(.34f,.34f,.32f);
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.025f,.025f,.025f,.84f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(4);folder.style.right=Length.Percent(4);
  folder.style.top=Length.Percent(5);folder.style.bottom=Length.Percent(5);
  folder.style.backgroundColor=new Color(.28f,.16f,.14f);root.Add(folder);
  var heading=new VisualElement();heading.style.flexDirection=FlexDirection.Row;
  heading.style.alignItems=Align.Center;heading.style.paddingLeft=20;heading.style.paddingRight=20;
  heading.style.height=62;folder.Add(heading);
  var title=Text(heading,T("compare.title"),Ink,21);title.style.flexGrow=1;title.style.marginBottom=0;
  var back=new Button(()=>{comparePicker=-1;FilePage();}){text=T("compare.back")};
  back.style.height=42;back.style.paddingLeft=15;back.style.paddingRight=15;
  back.style.color=Ink;back.style.backgroundColor=Card;heading.Add(back);
  var spread=new VisualElement();spread.style.flexDirection=FlexDirection.Row;spread.style.flexGrow=1;
  spread.style.paddingLeft=12;spread.style.paddingRight=12;spread.style.paddingBottom=12;folder.Add(spread);
  for(int side=0;side<2;side++) {
   int selectedSide=side;
   string selectedId=side==0?compareLeftId:compareRightId;
   var current=sources.FirstOrDefault(n=>n.id==selectedId);
   var sheet=new VisualElement();sheet.style.flexGrow=1;sheet.style.flexBasis=0;
   sheet.style.marginLeft=4;sheet.style.marginRight=4;sheet.style.paddingLeft=22;
   sheet.style.paddingRight=22;sheet.style.paddingTop=18;sheet.style.paddingBottom=15;
   sheet.style.backgroundColor=new Color(.91f,.85f,.73f);spread.Add(sheet);
   var picker=new Button(()=>{comparePicker=comparePicker==selectedSide?-1:selectedSide;ComparePage();})
    {text=(current==null?T("compare.choose"):T(current.titleKey))+"  ▾"};
   picker.style.minHeight=50;picker.style.paddingLeft=12;picker.style.unityTextAlign=TextAnchor.MiddleLeft;
   picker.style.fontSize=Typography.Snap(17);picker.style.color=ink;picker.style.backgroundColor=new Color(.79f,.72f,.60f);
   sheet.Add(picker);
   if(comparePicker==side) {
    var choices=Scroll(sheet);choices.style.maxHeight=240;
    foreach(var source in sources) {
     var choice=source;
     var option=new Button(()=>{
      if(selectedSide==0)compareLeftId=choice.id;else compareRightId=choice.id;
      comparePicker=-1;ComparePage();
     }){text=T(source.titleKey)};
     option.style.minHeight=44;option.style.marginTop=4;option.style.fontSize=Typography.Snap(15);
     option.style.unityTextAlign=TextAnchor.MiddleLeft;
     option.style.color=ink;option.style.backgroundColor=new Color(.84f,.78f,.67f);
     choices.Add(option);
    }
    if(sources.Length==0)Text(choices,T("compare.noSources"),muted,16);
   } else if(current==null) {
    Text(sheet,sources.Length==0?T("compare.noSources"):T("compare.empty"),muted,17);
   } else {
    var body=Scroll(sheet);
    if(current.fileMeta!=null)foreach(var field in current.fileMeta) {
     var meta=Text(body,T(field.labelKey)+"  :  "+T(field.valueKey),muted,13);
     meta.style.marginBottom=4;
    }
    if(current.kind=="interview") {
     var turns=game.State.interviewTurns.Where(t=>t.nodeId==current.id).ToArray();
     if(turns.Length==0)Text(body,T("file.noTranscript"),muted,16);
     foreach(var turn in turns) {
     Text(body,T("interview.bora"),muted,13);
     Text(body,T(turn.promptKey),ink,17);
     if(!string.IsNullOrEmpty(turn.sourceId)) {
      Text(body,T("interview.presented")+"  "+ReviewSourceTitle(game.Data,turn.sourceId),muted,13);
     }
      Text(body,T(current.personNameKey),muted,13);
      var answer=Text(body,T(turn.answerKey),ink,17);answer.style.marginBottom=16;
     }
    } else if(current.kind=="cctv") {
     if(current.cctvEvents!=null)foreach(var cctvEvent in current.cctvEvents) {
      var entry=Text(body,T(cctvEvent.textKey),ink,17);entry.style.marginBottom=10;
     }
    } else Text(body,T(current.bodyKey),ink,17);
   }
  }
 }
 void ReadPage(Node node) {
  showingInterviewList=false;
  game.Read(node.id);Save();
  if(node.kind=="bps") {
   VisualElement content;BpsTablet(node.titleKey,out content);
   TerminalSourceTabs(content,node);
   var tabletScroll=Scroll(content);Text(tabletScroll,T(node.bodyKey),Ink,19);
  } else {
   Frame(T("kind."+node.kind),T(node.titleKey),T("file.reference"));
   var scroll=Scroll(root);
   var paper=Panel(scroll);
   Text(paper,T(node.bodyKey),Ink,20);
   Button(root,T("back.file"),FilePage,true);
  }
 }
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
  if(turns.Length>0){historyTabs=new VisualElement();historyTabs.style.flexDirection=FlexDirection.Row;questionArea.Add(historyTabs);}
  Label topicStrip=null;
  if(phase==0 && topics.Length>1) {
   var current=topics.First(g=>g.Key==initiallyOpen);
   topicStrip=Text(questionArea,T(current.Key)+"  ·  "+current.Count(),Gold,16);
   topicStrip.style.height=MinimumTouchTarget;topicStrip.style.flexShrink=0;
   topicStrip.style.marginBottom=4;topicStrip.style.paddingLeft=12;
   topicStrip.style.unityTextAlign=TextAnchor.MiddleLeft;
   topicStrip.style.backgroundColor=new Color(.055f,.075f,.09f,.96f);
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
     var topicCount=topic.Count();
     var header=new Button(()=>{
      choices.style.display=choices.style.display==DisplayStyle.None?DisplayStyle.Flex:DisplayStyle.None;
      selectedInterviewTopic=topicKey;
      if(topicStrip!=null)topicStrip.text=T(topicKey)+"  ·  "+topicCount;
     })
      {text=T(topic.Key)+"  ·  "+topicCount};
     header.style.minHeight=MinimumTouchTarget;header.style.marginBottom=6;header.style.paddingLeft=12;
     header.style.unityTextAlign=TextAnchor.MiddleLeft;header.style.fontSize=Typography.Snap(16);
     header.style.color=Ink;header.style.backgroundColor=new Color(.14f,.20f,.20f);
     section.Insert(0,header);
    }
    foreach(var q in topic) {
     var question=q;
     Button(choices,"›  "+(game.State.asked.Contains(q.id)?T("interview.repeatPrefix")+"  ":"")+T(q.promptKey),()=>InterviewPage(node,question,1));
     var choiceButton=choices.Children().Last() as Button;
     choiceButton.style.whiteSpace=WhiteSpace.Normal;
     choiceButton.style.fontSize=Typography.Snap(16);
     choiceButton.style.minHeight=66;
     choiceButton.style.backgroundColor=new Color(.09f,.13f,.14f);
     choiceButton.style.borderLeftWidth=3;
     choiceButton.style.borderLeftColor=new Color(.38f,.70f,.63f);
    }
   }
   if(availableOptions.Length==0)Text(questions,T("interview.noNewInfo"),Muted,16);
  } else if(phase==1) {
   if(game.QuestionNeedsSource(active)) {
    InterviewSourcePicker(questions,node,active,sourceId,referenceCard);
   } else Button(questions,T("interview.listen"),()=>{
    var reply=game.AnswerKey(active);
    if(game.Ask(node.id,active.id)){Save();InterviewPage(node,active,2,reply);}
   },true);
  } else {
   if(referenceCard!=null)Button(questions,T("interview.openReference"),()=>referenceCard.style.display=DisplayStyle.Flex);
   Button(questions,T(sourceAccepted?"interview.next":"interview.tryAnotherSource"),
    ()=>InterviewPage(node,sourceAccepted?null:active,sourceAccepted?0:1),true);
  }
  if(turns.Length>0) {
   var history=new ScrollView();history.style.flexGrow=1;history.style.minHeight=0;questionArea.Add(history);
   for(int i=0;i<turns.Length;i++) {
    var turn=turns[i];
    var card=new VisualElement();card.style.paddingLeft=12;card.style.paddingRight=12;card.style.paddingTop=9;
    card.style.marginBottom=7;card.style.backgroundColor=new Color(.055f,.075f,.09f,.94f);
    history.Add(card);
    var number=Text(card,(i+1)+"  ·  "+T("interview.bora"),Gold,13);number.style.marginBottom=3;
    var prompt=Text(card,T(turn.promptKey),Ink,15);prompt.style.marginBottom=8;
    var speaker=Text(card,T(node.personNameKey),Muted,13);speaker.style.marginBottom=3;
    var reply=Text(card,T(turn.answerKey),Ink,16);reply.style.marginBottom=11;
   }
   var questionTab=new Button(){text=T("interview.questions")};
   var historyTab=new Button(){text=T("interview.history")+"  ·  "+turns.Length};
   foreach(var tab in new[]{questionTab,historyTab}) {
    tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.minWidth=0;
    tab.style.minHeight=MinimumTouchTarget;tab.style.fontSize=Typography.Snap(15);tab.style.color=Ink;
    tab.style.marginBottom=4;historyTabs.Add(tab);
   }
   questionTab.style.marginRight=4;
   Action<bool> switchView=showHistory=>{
    showingInterviewHistory=showHistory;
    questions.style.display=showHistory?DisplayStyle.None:DisplayStyle.Flex;
    history.style.display=showHistory?DisplayStyle.Flex:DisplayStyle.None;
    if(topicStrip!=null)topicStrip.style.display=showHistory?DisplayStyle.None:DisplayStyle.Flex;
    questionTab.style.backgroundColor=showHistory?new Color(.14f,.20f,.20f):new Color(.25f,.42f,.39f);
    historyTab.style.backgroundColor=showHistory?new Color(.25f,.42f,.39f):new Color(.14f,.20f,.20f);
   };
   questionTab.clicked+=()=>switchView(false);
   historyTab.clicked+=()=>switchView(true);
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
  return value.Length<=66?value:value.Substring(0,65).TrimEnd()+"…";
 }
 void InterviewSourcePicker(ScrollView questions,Node node,Question active,string sourceId,VisualElement referenceCard) {
  if(interviewSourceQuestionId!=node.id+"/"+active.id) {
   interviewSourceQuestionId=node.id+"/"+active.id;interviewSourceQuery="";interviewSourceFilter=0;
  }
  Text(questions,T("interview.chooseSource"),Gold,16);
  if(!string.IsNullOrEmpty(sourceId)) {
   var chosen=Text(questions,T("interview.selectedSource")+"  ·  "+ShortInterviewSourceLabel(CompactReportSourceLabel(sourceId)),Ink,15);
   chosen.style.whiteSpace=WhiteSpace.Normal;
   if(referenceCard!=null) {
    Button(questions,T("interview.openReference"),()=>referenceCard.style.display=DisplayStyle.Flex);
    var open=questions.Children().Last() as Button;
    open.style.minHeight=MinimumTouchTarget;
   }
   Button(questions,T("interview.presentSource"),()=>{
    var reply=game.AnswerKey(active,sourceId);
    if(game.Ask(node.id,active.id,sourceId)){Save();InterviewPage(node,active,2,reply,sourceId);}
    else InterviewPage(node,active,2,T("interview.unrelatedSource"),sourceId,false);
   },true);
   var present=questions.Children().Last() as Button;
   present.style.minHeight=MinimumTouchTarget;
  }
  var controls=new VisualElement();questions.Add(controls);
  var sources=ComparisonSources().Where(n=>n.kind!="interview" || n.personId!=node.personId).ToArray();
  var rows=new List<VisualElement>();var categories=new List<int>();var searchable=new List<string>();
  int[] categoryCounts=new int[4];
  foreach(var source in sources) {
   var item=source;
   int category=item.kind=="cctv"?3:item.kind=="interview"?2:1;
   if(category==3) {
    foreach(var record in item.cctvEvents ?? new CctvEvent[0]) {
     var eventItem=record;var reference=item.id+"#"+eventItem.id;
     if(game.SourceAlreadyPresented(node,active,reference))continue;
     var full=T(item.titleKey)+" · "+T(eventItem.textKey);
     Button(questions,ShortInterviewSourceLabel(full),()=>InterviewPage(node,active,1,null,reference),reference==sourceId);
     var row=questions.Children().Last();
     rows.Add(row);categories.Add(category);searchable.Add(full);categoryCounts[category]++;
    }
   } else if(category==2) {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==item.id)) {
     var answer=turn;var reference=game.InterviewTurnReference(answer);
     if(game.SourceAlreadyPresented(node,active,reference))continue;
     var full=T(item.personNameKey)+" · "+T(answer.promptKey)+" · "+T(answer.answerKey);
     Button(questions,ShortInterviewSourceLabel(T(item.personNameKey)+" · "+T(answer.promptKey)),()=>InterviewPage(node,active,1,null,reference),reference==sourceId);
     var row=questions.Children().Last();
     rows.Add(row);categories.Add(category);searchable.Add(full);categoryCounts[category]++;
    }
   } else if(!game.SourceAlreadyPresented(node,active,item.id)) {
    var full=T(item.titleKey)+" · "+T(item.bodyKey);
    if(item.fileMeta!=null)foreach(var field in item.fileMeta)full+=" · "+T(field.labelKey)+" "+T(field.valueKey);
    Button(questions,ShortInterviewSourceLabel(T(item.titleKey)),()=>InterviewPage(node,active,1,null,item.id),item.id==sourceId);
    var row=questions.Children().Last();
    rows.Add(row);categories.Add(category);searchable.Add(full);categoryCounts[category]++;
   }
  }
  if(rows.Count==0) {Text(questions,T("interview.noSource"),Muted,15);return;}
  var searchRow=new VisualElement();searchRow.style.flexDirection=FlexDirection.Row;searchRow.style.alignItems=Align.Center;controls.Add(searchRow);
  var search=new TextField(){label=T("conclude.search"),value=interviewSourceQuery};
  search.style.flexGrow=1;search.style.minWidth=0;search.style.height=MinimumTouchTarget;search.style.fontSize=Typography.Snap(18);
  search.style.backgroundColor=new Color(.09f,.13f,.14f);search.style.color=Ink;searchRow.Add(search);
  var clear=new Button(()=>search.value=""){text="×"};clear.style.width=MinimumTouchTarget;clear.style.height=MinimumTouchTarget;
  clear.style.marginLeft=5;clear.style.fontSize=Typography.Snap(24);clear.style.color=Ink;clear.style.backgroundColor=new Color(.14f,.20f,.20f);searchRow.Add(clear);
  var tabs=new VisualElement();tabs.style.marginTop=5;controls.Add(tabs);
  var count=Text(controls,"",Muted,13);
  var empty=Text(questions,T("conclude.noMatches"),Muted,15);empty.style.display=DisplayStyle.None;
  string[] labels={"conclude.filter.all","conclude.filter.documents","conclude.filter.interviews","conclude.filter.cctv"};
  var tabButtons=new List<Button>();var turkish=CultureInfo.GetCultureInfo("tr-TR");
  Func<string,string> normalize=value=>(value??"").ToLower(turkish).Replace(':','.');
  Action update=()=>{
   interviewSourceQuery=search.value;
   var query=normalize(interviewSourceQuery).Trim();int visible=0;
   for(int i=0;i<rows.Count;i++) {
    bool show=(interviewSourceFilter==0 || interviewSourceFilter==categories[i]) && (query.Length==0 || normalize(searchable[i]).Contains(query));
    rows[i].style.display=show?DisplayStyle.Flex:DisplayStyle.None;if(show)visible++;
   }
   count.text=visible+" "+T("conclude.sourceCount");
   empty.style.display=visible==0?DisplayStyle.Flex:DisplayStyle.None;
   for(int i=0;i<tabButtons.Count;i++)tabButtons[i].style.backgroundColor=i==interviewSourceFilter?new Color(.25f,.42f,.39f):new Color(.14f,.20f,.20f);
  };
  VisualElement tabRow=null;
  for(int i=0;i<labels.Length;i++) {
   if(i%2==0){tabRow=new VisualElement();tabRow.style.flexDirection=FlexDirection.Row;tabs.Add(tabRow);}
   int filter=i;var tab=new Button(()=>{interviewSourceFilter=filter;update();}){text=T(labels[i])};
   tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.minWidth=0;tab.style.minHeight=MinimumTouchTarget;
   tab.style.fontSize=Typography.Snap(14);tab.style.color=Ink;tab.style.marginRight=3;tab.style.marginBottom=3;
   tab.SetEnabled(i==0 || categoryCounts[i]>0);
   tabRow.Add(tab);tabButtons.Add(tab);
  }
  foreach(var row in rows) {row.style.minHeight=MinimumTouchTarget;row.style.whiteSpace=WhiteSpace.Normal;row.style.fontSize=Typography.Snap(15);}
  search.RegisterValueChangedCallback(evt=>update());update();
 }
 VisualElement InterviewReferenceCard(string sourceId) {
  int separator=sourceId.IndexOf('#');
  var source=game.Data.nodes.FirstOrDefault(n=>n.id==(separator<0?sourceId:sourceId.Substring(0,separator)));
  if(source==null)return null;
  var ink=new Color(.13f,.16f,.20f);
  var muted=new Color(.37f,.36f,.33f);
  var card=new VisualElement();card.style.position=Position.Absolute;
  card.style.left=Length.Percent(2);card.style.width=Length.Percent(27);
  card.style.top=Length.Percent(45);card.style.bottom=Length.Percent(15);
  card.style.paddingLeft=16;card.style.paddingRight=15;card.style.paddingTop=10;card.style.paddingBottom=11;
  card.style.backgroundColor=new Color(.91f,.85f,.73f);
  card.style.borderLeftWidth=3;card.style.borderTopWidth=2;
  card.style.borderLeftColor=new Color(.57f,.39f,.29f);card.style.borderTopColor=new Color(.98f,.93f,.83f);
  root.Add(card);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;card.Add(header);
  var title=Text(header,T("interview.referenceCard"),muted,13);
  title.style.flexGrow=1;title.style.marginBottom=0;
  var close=new Button(()=>card.style.display=DisplayStyle.None){text="×"};
  close.style.width=MinimumTouchTarget;close.style.height=MinimumTouchTarget;close.style.fontSize=Typography.Snap(22);
  close.style.color=ink;close.style.backgroundColor=new Color(.79f,.72f,.61f);header.Add(close);
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
 void StopCctvVideo() {
  if(cctvPlayer!=null) {
   cctvPlayer.prepareCompleted-=OnCctvVideoPrepared;
   cctvPlayer.errorReceived-=OnCctvVideoError;
   cctvPlayer.loopPointReached-=OnCctvVideoEnded;
   cctvPlayer.Stop();Destroy(cctvPlayer);cctvPlayer=null;
  }
  if(cctvTexture!=null){cctvTexture.Release();Destroy(cctvTexture);cctvTexture=null;}
  if(cctvViewer!=null){cctvViewer.RemoveFromHierarchy();cctvViewer=null;}
  cctvVideoStatus=null;
  cctvPlaybackButton=cctvStepButton=null;
  cctvReachedEnd=false;
 }
 void OnCctvVideoPrepared(VideoPlayer player) {
  if(player!=cctvPlayer)return;
  cctvReachedEnd=false;
  player.Play();
  bool canAdvance=player.canStep || player.canSetTime;
  if(cctvVideoStatus!=null){
   cctvVideoStatus.text=canAdvance?string.Empty:T("cctv.videoStepUnavailable");
   cctvVideoStatus.style.display=canAdvance?DisplayStyle.None:DisplayStyle.Flex;
  }
  if(cctvPlaybackButton!=null){cctvPlaybackButton.SetEnabled(true);cctvPlaybackButton.text=T("cctv.videoPause");}
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(canAdvance);
 }
 void OnCctvVideoEnded(VideoPlayer player) {
  if(player!=cctvPlayer)return;
  cctvReachedEnd=true;
  if(cctvPlaybackButton!=null)cctvPlaybackButton.text=T("cctv.videoPlay");
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
 }
 void OnCctvVideoError(VideoPlayer player,string message) {
  if(player!=cctvPlayer)return;
  Debug.LogWarning("CCTV footage unavailable: "+message);
  if(cctvVideoStatus!=null){cctvVideoStatus.text=T("cctv.videoUnavailable");cctvVideoStatus.style.display=DisplayStyle.Flex;}
  if(cctvPlaybackButton!=null)cctvPlaybackButton.SetEnabled(false);
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
 }
 void ReplayCctvVideo() {
  if(cctvPlayer==null)return;
  cctvReachedEnd=false;
  if(cctvVideoStatus!=null){cctvVideoStatus.text=T("cctv.videoLoading");cctvVideoStatus.style.display=DisplayStyle.Flex;}
  if(cctvPlaybackButton!=null)cctvPlaybackButton.SetEnabled(false);
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
  cctvPlayer.Stop();cctvPlayer.Prepare();
 }
 void OpenCctvVideo(Node node,CctvEvent record,VisualElement content) {
  if(string.IsNullOrEmpty(record.videoPath))return;
  StopCctvVideo();
  var viewer=new VisualElement();cctvViewer=viewer;
  viewer.style.position=Position.Absolute;
  viewer.style.left=0;viewer.style.right=0;viewer.style.top=0;viewer.style.bottom=0;
  viewer.style.flexDirection=FlexDirection.Column;
  viewer.style.paddingLeft=8;viewer.style.paddingRight=8;
  viewer.style.paddingTop=5;viewer.style.paddingBottom=5;
  viewer.style.backgroundColor=new Color(.035f,.065f,.085f);
  content.parent.Add(viewer);
  cctvTexture=new RenderTexture(1280,720,0,RenderTextureFormat.ARGB32);
  cctvTexture.Create();
  var videoFrame=new VisualElement();
  videoFrame.style.width=Length.Percent(100);videoFrame.style.flexGrow=1;
  videoFrame.style.minHeight=0;videoFrame.style.marginTop=0;videoFrame.style.marginBottom=5;
  viewer.Add(videoFrame);
  var image=new Image {image=cctvTexture,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
  image.style.position=Position.Absolute;
  image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  videoFrame.Add(image);
  var overlay=new VisualElement(){pickingMode=PickingMode.Ignore};
  overlay.style.position=Position.Absolute;
  overlay.style.backgroundColor=new Color(.025f,.055f,.06f,.12f);
  videoFrame.Add(overlay);
  var cameraMark=new Color(.88f,.91f,.87f,.84f);
  for(int corner=0;corner<4;corner++) {
   bool left=corner%2==0,upper=corner<2;
   var horizontal=new VisualElement(){pickingMode=PickingMode.Ignore};
   horizontal.style.position=Position.Absolute;horizontal.style.width=22;horizontal.style.height=2;
   horizontal.style.backgroundColor=cameraMark;
   if(left)horizontal.style.left=10;else horizontal.style.right=10;
   if(upper)horizontal.style.top=10;else horizontal.style.bottom=10;
   overlay.Add(horizontal);
   var vertical=new VisualElement(){pickingMode=PickingMode.Ignore};
   vertical.style.position=Position.Absolute;vertical.style.width=2;vertical.style.height=22;
   vertical.style.backgroundColor=cameraMark;
   if(left)vertical.style.left=10;else vertical.style.right=10;
   if(upper)vertical.style.top=10;else vertical.style.bottom=10;
   overlay.Add(vertical);
  }
  for(int i=0;i<8;i++) {
   var scanline=new VisualElement(){pickingMode=PickingMode.Ignore};
   scanline.style.position=Position.Absolute;scanline.style.left=0;scanline.style.right=0;
   scanline.style.top=Length.Percent(8+i*12);scanline.style.height=1;
   scanline.style.backgroundColor=new Color(.85f,.94f,.91f,.035f);
   overlay.Add(scanline);
  }
  var cameraLabel=new VisualElement(){pickingMode=PickingMode.Ignore};
  cameraLabel.style.position=Position.Absolute;cameraLabel.style.left=19;cameraLabel.style.top=17;
  cameraLabel.style.paddingLeft=7;cameraLabel.style.paddingRight=7;
  cameraLabel.style.paddingTop=5;cameraLabel.style.paddingBottom=5;
  cameraLabel.style.backgroundColor=new Color(.02f,.04f,.05f,.55f);overlay.Add(cameraLabel);
  var overlayTitle=string.IsNullOrEmpty(node.cctvOverlayKey)?T(node.cctvSourceKey):T(node.cctvOverlayKey);
  var cameraName=Text(cameraLabel,overlayTitle,Ink,14);cameraName.style.marginBottom=3;
  var recRow=new VisualElement(){pickingMode=PickingMode.Ignore};
  recRow.style.flexDirection=FlexDirection.Row;recRow.style.alignItems=Align.Center;cameraLabel.Add(recRow);
  var recDot=new VisualElement(){pickingMode=PickingMode.Ignore};
  recDot.style.width=8;recDot.style.height=8;recDot.style.marginRight=6;
  recDot.style.backgroundColor=new Color(.95f,.18f,.13f);recRow.Add(recDot);
  var timeText=string.IsNullOrEmpty(record.overlayTimeKey)?string.Empty:T(record.overlayTimeKey);
  var recText=Text(recRow,T("cctv.overlay.rec")+(timeText.Length==0?"":"  "+timeText),Ink,12);
  recText.style.marginBottom=0;
  bool recVisible=true;
  recDot.schedule.Execute(()=>{recVisible=!recVisible;recDot.style.opacity=recVisible?1f:.15f;}).Every(480);
  var glitchBand=new VisualElement(){pickingMode=PickingMode.Ignore};
  glitchBand.style.position=Position.Absolute;glitchBand.style.left=0;glitchBand.style.right=0;
  glitchBand.style.top=Length.Percent(52);glitchBand.style.height=2;
  glitchBand.style.backgroundColor=new Color(.85f,.96f,.93f,.20f);
  glitchBand.style.opacity=0;overlay.Add(glitchBand);
  glitchBand.schedule.Execute(()=>{
   glitchBand.style.top=Length.Percent(UnityEngine.Random.Range(18f,82f));
   glitchBand.style.opacity=UnityEngine.Random.value<.13f?.38f:0f;
  }).Every(180);
  // Keep the skip control over the supplied footage's lower-right mark,
  // including when ScaleToFit adds letterboxing around the 16:9 frame.
  var skipVideo=new Button(StopCctvVideo){text=T("intro.skip")};
  skipVideo.style.position=Position.Absolute;
  skipVideo.style.backgroundColor=Card;skipVideo.style.color=Ink;
  skipVideo.style.fontSize=Typography.Snap(15);skipVideo.style.minHeight=MinimumTouchTarget;
  videoFrame.Add(skipVideo);
  var close=new Button(StopCctvVideo){text=T("cctv.videoClose")};
  close.style.position=Position.Absolute;close.style.right=10;close.style.top=10;
  close.style.minHeight=MinimumTouchTarget;close.style.backgroundColor=Card;
  close.style.color=Ink;close.style.fontSize=Typography.Snap(14);videoFrame.Add(close);
  videoFrame.RegisterCallback<GeometryChangedEvent>(evt=>{
   float width=videoFrame.resolvedStyle.width,height=videoFrame.resolvedStyle.height;
   if(float.IsNaN(width)||float.IsNaN(height)||width<=0||height<=0)return;
   float scale=Mathf.Min(width/1280f,height/720f);
   float offsetX=(width-1280f*scale)*.5f;
   float offsetY=(height-720f*scale)*.5f;
   overlay.style.left=offsetX;overlay.style.top=offsetY;
   overlay.style.width=1280f*scale;overlay.style.height=720f*scale;
   cameraName.style.fontSize=Mathf.Clamp(14f*scale,11f,14f);
   cameraName.style.maxWidth=Mathf.Max(100f,1280f*scale-60f);
   float buttonWidth=Mathf.Max(110f,150f*scale);
   float buttonHeight=Mathf.Max(MinimumTouchTarget,84f*scale);
   skipVideo.style.width=buttonWidth;skipVideo.style.height=buttonHeight;
   skipVideo.style.left=Mathf.Clamp(offsetX+1280f*scale-buttonWidth,0f,Mathf.Max(0f,width-buttonWidth));
   skipVideo.style.top=Mathf.Clamp(offsetY+575f*scale,0f,Mathf.Max(0f,height-buttonHeight));
  });
  var controls=new VisualElement();controls.style.flexDirection=FlexDirection.Row;
  controls.style.alignItems=Align.Center;controls.style.flexShrink=0;viewer.Add(controls);
  var caption=Text(controls,T(record.textKey),Ink,15);
  caption.style.flexGrow=1;caption.style.minWidth=0;
  caption.style.marginBottom=0;caption.style.marginRight=8;
  cctvPlaybackButton=new Button(()=>{
   if(cctvPlayer==null || !cctvPlayer.isPrepared)return;
   if(cctvReachedEnd){ReplayCctvVideo();return;}
   if(cctvPlayer.isPlaying){cctvPlayer.Pause();cctvPlaybackButton.text=T("cctv.videoPlay");}
   else {cctvPlayer.Play();cctvPlaybackButton.text=T("cctv.videoPause");}
  }){text=T("cctv.videoPlay")};
  cctvStepButton=new Button(()=>{
   if(cctvPlayer==null || !cctvPlayer.isPrepared || cctvReachedEnd)return;
   if(cctvPlayer.isPlaying)cctvPlayer.Pause();
   cctvPlaybackButton.text=T("cctv.videoPlay");
   if(cctvPlayer.canStep)cctvPlayer.StepForward();
   else if(cctvPlayer.canSetTime)cctvPlayer.frame=Math.Max(0L,cctvPlayer.frame)+1L;
  }){text=T("cctv.videoStep")};
  var replay=new Button(ReplayCctvVideo){text=T("cctv.videoReplay")};
  foreach(var button in new[]{cctvPlaybackButton,cctvStepButton,replay}) {
   button.style.width=118;button.style.flexShrink=0;
   button.style.minHeight=MinimumTouchTarget;button.style.fontSize=Typography.Snap(14);
   button.style.backgroundColor=Paper;button.style.color=Ink;
   button.style.marginRight=4;controls.Add(button);
  }
  cctvPlaybackButton.SetEnabled(false);cctvStepButton.SetEnabled(false);
  cctvVideoStatus=Text(videoFrame,T("cctv.videoLoading"),Gold,13);
  cctvVideoStatus.style.position=Position.Absolute;
  cctvVideoStatus.style.left=Length.Percent(30);
  cctvVideoStatus.style.right=Length.Percent(30);
  cctvVideoStatus.style.top=12;
  cctvVideoStatus.style.unityTextAlign=TextAnchor.MiddleCenter;
  cctvVideoStatus.style.backgroundColor=new Color(.02f,.04f,.05f,.78f);
  cctvVideoStatus.style.marginBottom=0;
  cctvPlayer=gameObject.AddComponent<VideoPlayer>();
  cctvPlayer.playOnAwake=false;cctvPlayer.isLooping=false;
  cctvPlayer.renderMode=VideoRenderMode.RenderTexture;
  cctvPlayer.targetTexture=cctvTexture;
  cctvPlayer.audioOutputMode=VideoAudioOutputMode.None;
  cctvPlayer.source=VideoSource.Url;
  cctvPlayer.url=Application.streamingAssetsPath+"/"+record.videoPath;
  cctvPlayer.prepareCompleted+=OnCctvVideoPrepared;
  cctvPlayer.errorReceived+=OnCctvVideoError;
  cctvPlayer.loopPointReached+=OnCctvVideoEnded;
  cctvPlayer.Prepare();
 }
 void CctvScreen(Node node,string focusEventId=null) {
  showingInterviewList=false;
  VisualElement content;
  BpsTablet("cctv.archive",out content);
  TerminalSourceTabs(content,node);
  var meta=new VisualElement();meta.style.flexDirection=FlexDirection.Row;
  meta.style.alignItems=Align.Center;content.Add(meta);
  var camera=Text(meta,T(node.cctvSourceKey),Ink,22);
  camera.style.flexGrow=1;camera.style.marginBottom=0;
  var status=Text(meta,T("cctv.signal"),Gold,14);status.style.marginBottom=0;
  var period=Text(content,T(node.cctvPeriodKey),Muted,14);period.style.marginBottom=7;
  var recordPanel=new VisualElement();recordPanel.style.flexGrow=1;
  recordPanel.style.backgroundColor=new Color(.045f,.08f,.105f);
  recordPanel.style.borderTopWidth=1;recordPanel.style.borderBottomWidth=1;
  recordPanel.style.borderLeftWidth=1;recordPanel.style.borderRightWidth=1;
  recordPanel.style.borderTopColor=Muted;recordPanel.style.borderBottomColor=Muted;
  recordPanel.style.borderLeftColor=Muted;recordPanel.style.borderRightColor=Muted;
  recordPanel.style.paddingLeft=11;recordPanel.style.paddingRight=11;
  recordPanel.style.paddingTop=6;content.Add(recordPanel);
  var stream=Scroll(recordPanel);
  stream.style.paddingTop=2;
  var records=node.cctvEvents ?? new CctvEvent[0];
  var rows=new List<VisualElement>();
  var lines=new List<Label>();
  var actions=new List<VisualElement>();
  foreach(var record in records) {
   var row=new VisualElement();row.style.display=DisplayStyle.None;
   row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
   row.style.minHeight=44;row.style.paddingLeft=10;row.style.paddingRight=8;
   row.style.marginBottom=3;row.style.borderBottomWidth=1;
   row.style.borderBottomColor=new Color(.19f,.26f,.28f);
   row.style.backgroundColor=new Color(.055f,.10f,.12f);
   stream.Add(row);rows.Add(row);
   var label=Text(row,string.Empty,Ink,22);label.style.flexGrow=1;
   label.style.marginBottom=0;lines.Add(label);
   var action=new VisualElement();row.Add(action);actions.Add(action);
  }
  Action<int> addFootageButton=index=>{
   var record=records[index];
   if(string.IsNullOrEmpty(record.videoPath))return;
   var watch=new Button(()=>OpenCctvVideo(node,record,content)){text="▶ "+T("cctv.watch")};
   watch.tooltip=T("cctv.watch");
   watch.style.minWidth=88;watch.style.height=MinimumTouchTarget;
   watch.style.backgroundColor=Paper;watch.style.color=Gold;
   watch.style.fontSize=Typography.Snap(15);actions[index].Add(watch);
  };
  var controls=new VisualElement();content.Add(controls);
  if(!string.IsNullOrEmpty(focusEventId) && game.State.read.Contains(node.id)) {
   status.text=T("cctv.complete");
   for(int i=0;i<records.Length;i++) {
    rows[i].style.display=DisplayStyle.Flex;
    lines[i].text=T(records[i].textKey);
    addFootageButton(i);
   }
   int focus=Array.FindIndex(records,e=>e.id==focusEventId);
   if(focus>=0)content.schedule.Execute(()=>stream.ScrollTo(rows[focus]));
   return;
  }
  bool reviewing=false;
  Button(controls,T("cctv.review"),()=>{
   if(reviewing)return;
   reviewing=true;
   controls.Clear();
   Text(controls,T("cctv.scanning"),Gold,15);
   int index=0;
   Action next=null;
   next=()=>{
    if(index>=records.Length) {
     controls.Clear();
     status.text=T("cctv.complete");
     game.Read(node.id);Save();
     return;
    }
    int current=index++;
    var record=records[current];
    int delay=record.delayMs>0?record.delayMs:650;
    content.schedule.Execute(()=>{
     if(!string.IsNullOrEmpty(record.signalKey))status.text=T(record.signalKey);
     rows[current].style.display=DisplayStyle.Flex;
     var line=lines[current];
     string finalText=T(string.IsNullOrEmpty(record.glitchKey)?record.textKey:record.glitchKey);
     line.text="▒▒▒  " + T("cctv.syncing");
     content.schedule.Execute(()=>{
      line.text=finalText;
      stream.ScrollTo(rows[current]);
      addFootageButton(current);
      if(!string.IsNullOrEmpty(record.glitchKey)) {
       Button clarify=null;
       clarify=new Button(()=>{
        clarify.RemoveFromHierarchy();
        line.text=T("cctv.syncing");
        content.schedule.Execute(()=>{line.text=T(record.textKey);}).ExecuteLater(360);
       }){text="↻"};
       clarify.tooltip=T("cctv.clarify");
       clarify.style.width=MinimumTouchTarget;clarify.style.height=MinimumTouchTarget;clarify.style.fontSize=Typography.Snap(25);
       clarify.style.backgroundColor=Paper;clarify.style.color=Gold;
       actions[current].Add(clarify);
      }
      next();
     }).ExecuteLater(110+current%3*70);
    }).ExecuteLater(delay);
   };
   next();
  },true);
 }
 void Conclusion() { ConclusionStep(0); }
 void ConclusionStep(int step) {
  if(!game.CanConclude){FilePage();return;}
  step=Mathf.Clamp(step,0,3);
  showingInterviewList=false;
  VisualElement body;
  ReportSheet(T("conclude"),T("conclude.prompt"),out body,null,true);
  var scroll=body as ScrollView;
  var dark=new Color(.13f,.16f,.20f);
  var muted=new Color(.39f,.36f,.31f);
  Text(scroll,(step+1)+" / 4",muted,15).style.marginBottom=4;
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
   var previous=new Button(()=>ConclusionStep(step-1)){text="‹  "+T("conclude.previous")};
   previous.style.flexGrow=1;previous.style.minHeight=50;previous.style.fontSize=Typography.Snap(17);
   previous.style.backgroundColor=new Color(.76f,.69f,.58f);previous.style.color=dark;
   previous.style.marginRight=7;nav.Add(previous);
  }
  next=step==3?new Button(Result){text=T("conclude.submit")}
   :new Button(()=>ConclusionStep(step+1)){text=T("conclude.next")+"  ›"};
  next.style.flexGrow=1;next.style.minHeight=50;
  next.style.backgroundColor=new Color(.32f,.20f,.17f);next.style.color=Ink;
  next.style.fontSize=Typography.Snap(18);
  if(dossierBoldFont!=null)next.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  nav.Add(next);refresh();
 }
 void ReportReviewClaim(VisualElement parent,string headingKey,string choiceKey,string sourceId) {
  var ink=new Color(.13f,.16f,.20f);
  var card=new VisualElement();card.style.backgroundColor=new Color(.82f,.76f,.65f);
  card.style.paddingLeft=12;card.style.paddingRight=12;
  card.style.paddingTop=9;card.style.paddingBottom=9;card.style.marginBottom=8;
  parent.Add(card);
  Text(card,T(headingKey)+"  ·  "+T(choiceKey),ink,17).style.marginBottom=5;
  var source=new Button(()=>ShowReportSourceCard(sourceId))
   {text=T("conclude.openSource")+"  ›  "+CompactReportSourceLabel(sourceId)};
  source.style.minHeight=50;source.style.whiteSpace=WhiteSpace.Normal;
  source.style.unityTextAlign=TextAnchor.MiddleLeft;source.style.fontSize=Typography.Snap(15);
  source.style.backgroundColor=new Color(.92f,.86f,.75f);source.style.color=ink;
  card.Add(source);
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
  var ink=new Color(.13f,.16f,.20f);
  var muted=new Color(.39f,.36f,.31f);
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.02f,.025f,.03f,.80f);root.Add(shade);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(10);paper.style.right=Length.Percent(10);
  paper.style.top=Length.Percent(9);paper.style.bottom=Length.Percent(9);
  paper.style.paddingLeft=20;paper.style.paddingRight=20;
  paper.style.paddingTop=14;paper.style.paddingBottom=14;
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);shade.Add(paper);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;
  header.style.alignItems=Align.Center;paper.Add(header);
  var title=Text(header,CompactReportSourceLabel(sourceId),ink,18);
  title.style.flexGrow=1;title.style.whiteSpace=WhiteSpace.Normal;
  var close=new Button(()=>shade.RemoveFromHierarchy()){text="×"};
  close.style.width=MinimumTouchTarget;close.style.height=MinimumTouchTarget;
  close.style.fontSize=Typography.Snap(23);close.style.backgroundColor=new Color(.76f,.69f,.58f);
  close.style.color=ink;header.Add(close);
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
  var dark=new Color(.13f,.16f,.20f);
  var turkish=CultureInfo.GetCultureInfo("tr-TR");
  Func<string,string> normalize=value=>(value ?? "").ToLower(turkish).Replace(':','.');
  Text(parent,T(promptKey),new Color(.39f,.36f,.31f),14).style.marginBottom=3;
  var opener=new Button{ text=T("conclude.source")+"  ·  "+CompactReportSourceLabel(selected())+"  ▾" };
  opener.style.minHeight=MinimumTouchTarget;opener.style.marginBottom=5;opener.style.paddingLeft=12;
  opener.style.unityTextAlign=TextAnchor.MiddleLeft;opener.style.fontSize=Typography.Snap(15);
  opener.style.whiteSpace=WhiteSpace.Normal;
  opener.style.color=dark;opener.style.backgroundColor=new Color(.78f,.71f,.61f);parent.Add(opener);
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
  search.style.backgroundColor=new Color(.98f,.94f,.85f);
  searchBar.Add(search);
  var clear=new Button(()=>search.value=""){text="×"};
  clear.style.width=MinimumTouchTarget;clear.style.height=MinimumTouchTarget;
  clear.style.fontSize=Typography.Snap(24);clear.style.marginLeft=5;
  clear.style.color=dark;clear.style.backgroundColor=new Color(.78f,.71f,.61f);
  searchBar.Add(clear);
  var count=Text(panel,"",new Color(.39f,.36f,.31f),13);
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
     var option=new Button(()=>{
      setSelected(reference);opener.text=T("conclude.source")+"  ·  "+CompactReportSourceLabel(reference)+"  ▾";
      panel.style.display=DisplayStyle.None;refresh();
     }){text=label};
     option.style.minHeight=58;option.style.whiteSpace=WhiteSpace.Normal;
     option.style.fontSize=Typography.Snap(15);option.style.unityTextAlign=TextAnchor.MiddleLeft;
     option.style.color=dark;option.style.backgroundColor=new Color(.85f,.79f,.69f);
     option.style.marginBottom=5;choices.Add(option);
     rows.Add(option);categories.Add(category);searchTexts.Add(normalize(label));categoryCounts[category]++;
    }
   } else if(category==2) {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==item.id)) {
     var chosen=turn;
     var reference=game.InterviewTurnReference(chosen);
     var label=T(item.personNameKey)+"  ·  "+T(chosen.promptKey)+"\n"+T(chosen.answerKey);
     var option=new Button(()=>{
      setSelected(reference);opener.text=T("conclude.source")+"  ·  "+CompactReportSourceLabel(reference)+"  ▾";
      panel.style.display=DisplayStyle.None;refresh();
     }){text=label};
     option.style.minHeight=64;option.style.whiteSpace=WhiteSpace.Normal;
     option.style.fontSize=Typography.Snap(15);option.style.unityTextAlign=TextAnchor.MiddleLeft;
     option.style.color=dark;option.style.backgroundColor=new Color(.85f,.79f,.69f);
     option.style.marginBottom=5;choices.Add(option);
     rows.Add(option);categories.Add(category);searchTexts.Add(normalize(label));categoryCounts[category]++;
    }
   } else {
    var label=T(item.titleKey)+"\n"+ReportSourcePreview(item);
    var option=new Button(()=>{
     setSelected(item.id);opener.text=T("conclude.source")+"  ·  "+T(item.titleKey)+"  ▾";
     panel.style.display=DisplayStyle.None;refresh();
    }){text=label};
    option.style.minHeight=64;option.style.whiteSpace=WhiteSpace.Normal;
    option.style.fontSize=Typography.Snap(15);option.style.unityTextAlign=TextAnchor.MiddleLeft;
    option.style.color=dark;option.style.backgroundColor=new Color(.85f,.79f,.69f);
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
    tabButtons[i].style.backgroundColor=i==activeFilter?new Color(.65f,.50f,.32f):new Color(.78f,.71f,.61f);
    tabButtons[i].style.color=dark;
   }
  };
  for(int i=0;i<labels.Length;i++) {
   int category=i;
   var tab=new Button(()=>{activeFilter=category;updateFilter();}){text=T(labels[i])};
   tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.minWidth=0;
   tab.style.minHeight=MinimumTouchTarget;tab.style.fontSize=Typography.Snap(14);
   tab.style.marginLeft=2;tab.style.marginRight=2;
   tab.SetEnabled(i==0 || categoryCounts[i]>0);
   tabs.Add(tab);tabButtons.Add(tab);
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
  var dark=new Color(.13f,.16f,.20f);
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
  var dark=new Color(.13f,.16f,.20f);
  var name=Text(row,label, new Color(.36f,.34f,.31f),15);name.style.width=150;name.style.marginBottom=0;
  var detail=Text(row,":  "+value,dark,15);detail.style.flexGrow=1;detail.style.marginBottom=0;
 }
 void CaseSummary() {
  if(!game.State.closed){Desk();return;}
  Desk();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=new Color(.025f,.025f,.025f,.80f);root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(9);folder.style.right=Length.Percent(9);
  folder.style.top=Length.Percent(5);folder.style.bottom=Length.Percent(4);
  folder.style.backgroundColor=new Color(.27f,.16f,.13f);
  folder.style.borderBottomWidth=8;folder.style.borderBottomColor=new Color(.11f,.07f,.06f);root.Add(folder);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(10);paper.style.right=Length.Percent(10);
  paper.style.top=Length.Percent(6);paper.style.bottom=Length.Percent(6);
  paper.style.backgroundColor=new Color(.91f,.85f,.73f);
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=18;paper.style.paddingBottom=16;root.Add(paper);
  var dark=new Color(.13f,.16f,.20f);var muted=new Color(.39f,.36f,.31f);
  var header=new VisualElement();header.style.flexDirection=FlexDirection.Row;header.style.alignItems=Align.Center;paper.Add(header);
  var mark=Text(header,"✓",new Color(.16f,.39f,.31f),42);mark.style.width=64;mark.style.marginBottom=0;
  var titles=new VisualElement();titles.style.flexGrow=1;header.Add(titles);
  var kicker=Text(titles,T(game.Data.titleKey),muted,15);kicker.style.marginBottom=1;
  var title=Text(titles,T("result.summary"),dark,28);title.style.marginBottom=2;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var subtitle=Text(titles,T("result.status"),dark,14);subtitle.style.marginBottom=0;
  var brand=Text(header,T("summary.brand"),muted,14);brand.style.width=190;brand.style.unityTextAlign=TextAnchor.MiddleRight;
  var line=new VisualElement();line.style.height=1;line.style.marginTop=13;line.style.marginBottom=13;
  line.style.backgroundColor=new Color(.57f,.51f,.44f);paper.Add(line);
  bool reviewed=game.Career.reviewHistory.Any(r=>r.caseId==game.Data.id);
  var status=new VisualElement();status.style.flexDirection=FlexDirection.Row;status.style.alignItems=Align.Center;
  status.style.backgroundColor=new Color(.11f,.16f,.17f);
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
  var band=Text(details,T("summary.report"),dark,17);band.style.backgroundColor=new Color(.79f,.73f,.63f);
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
  findings.style.backgroundColor=new Color(.79f,.73f,.63f);
  var sourceNames=game.Data.nodes.Where(n=>game.State.read.Contains(n.id) && n.id!=game.State.reportProof)
   .Select(n=>T(n.titleKey)).Distinct().Take(4).ToArray();
  foreach(var name in sourceNames)Text(content,"•  "+name,dark,14);
  if(sourceNames.Length==0)Text(content,T("summary.noSources"),muted,14);
  var actions=new VisualElement();actions.style.flexDirection=FlexDirection.Row;actions.style.marginTop=10;paper.Add(actions);
  var back=new Button(Desk){text=T("back.desk")};back.style.flexGrow=1;back.style.minHeight=48;
  back.style.backgroundColor=new Color(.77f,.69f,.57f);back.style.color=dark;
  back.style.fontSize=Typography.Snap(17);if(dossierFont!=null)back.style.unityFontDefinition=FontDefinition.FromFont(dossierFont);actions.Add(back);
  var next=new Button(ContinueToNextCase){text=T("result.continue")+"  →"};next.style.flexGrow=1;next.style.minHeight=48;
  next.style.marginLeft=12;next.style.backgroundColor=new Color(.32f,.20f,.17f);next.style.color=Ink;
  next.style.fontSize=Typography.Snap(17);if(dossierBoldFont!=null)next.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);actions.Add(next);
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
   game=new Investigation(nextData,progress,game.Career,careerRules);
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
  var dark=new Color(.13f,.16f,.20f);
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
  if(game.Career.retired)Text(body,T("career.ended"),new Color(.45f,.24f,.19f),18);
  else if(game.State.closed)Button(body,T("result.continue"),ContinueToNextCase,true);
 }

}
}
