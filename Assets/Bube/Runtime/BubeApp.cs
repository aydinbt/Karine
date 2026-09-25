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
// Oyunun tek kalıcı MonoBehaviour'u. Ekranlar kodla kurulur ve dosya başına
// bir konu olacak şekilde `BubeApp.*.cs` parçalarına ayrılmıştır. Burada
// çekirdek durur: alanlar, yaşam döngüsü, kayıt ve ekran ilkelleri.
public sealed partial class BubeApp : MonoBehaviour {
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
 AudioDirector audio;
 // Android'de geri tuşu bir "geri" eylemidir. Karşılığı yoksa işletim sistemi
 // uygulamayı kapatır ve oyuncu bunu kazara yapar — soruşturmanın ortasında.
 // Her katman açıldığında geri tuşunun nereye gideceğini söyler.
 Action escapeBack;
 VisualElement chainEndNotice;
 bool askingToQuit;
 GameConfig config;
 Investigation game;
 VisualElement root;
 bool confirmRestart;
 bool instantText;
 string selectedFileNode="report";
 string selectedFileSection="report";
 // Dosyada gezinme süzgeçleri: tür (0 tümü, 1 belge, 2 ifade, 3 kamera) ve kişi.
 int fileFilterKind;
 string fileFilterPerson="";
 string selectedSearchTurn;
 string compareLeftId, compareRightId;
 int comparePicker=-1;
 Font dossierFont, dossierBoldFont;
 FontSet fonts;
 string selectedSuspect, selectedMethod, selectedEvidence;
 string selectedSuspectSource, selectedMethodSource, selectedEvidenceSource;
 bool showingInterviewList;
 string selectedInterviewTopic, selectedInterviewNodeId;
 bool showingInterviewHistory;
 string interviewSourceQuestionId;
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
 VideoPlayer menuPlayer;
 RenderTexture menuTexture;
 bool menuVideoFailed;
 VideoPlayer introPlayer;
 RenderTexture introTexture;
 VisualElement introBrand;
 VisualElement introPlace;
 Button introSkip;
 WorldIntro activeIntro;
 CornerMark activeMark;
 bool deskArrivalDone;
 Action introAfter;
 VideoPlayer cctvPlayer;
 RenderTexture cctvTexture;
 VisualElement cctvViewer;
 Label cctvVideoStatus;
 Button cctvPlaybackButton, cctvStepButton;
 bool cctvReachedEnd;

 // Palet artık ekranın içinde değil `KarineTheme`de. Buradaki adlar eski
 // çağrı yerlerini kırmamak için duruyor; değerleri kit'ten gelir.
 static readonly Color Ink = KarineTheme.Primary;
 static readonly Color Muted = KarineTheme.Muted;
 static readonly Color Gold = KarineTheme.Secondary;
 static readonly Color Base = KarineTheme.Background;
 static readonly Color Card = KarineTheme.Panel;
 static readonly Color Paper = KarineTheme.Panel2;
 const string ArchiveTimelineId="@timeline";
 const int MinimumTouchTarget=KarineTheme.TouchTarget;

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
  locale=LocaleLoader.Load(config.locale);
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
  game=new Investigation(caseData,progress,career,careerRules){Text=locale};
  // Daha yeni bir surumden gelen kayit okunamaz. Silmek yerine yana kaldirilir:
  // oyuncu eski surume donerse kayit yerinde durur.
  string saveNotice=null;
  if(game.StateOutcome==SaveOutcome.FromFuture)saveNotice=SetAside(CaseSavePath(caseId));
  if(game.CareerOutcome==SaveOutcome.FromFuture)saveNotice=SetAside(CareerSavePath);
  if(game.StateOutcome==SaveOutcome.Migrated || game.CareerOutcome==SaveOutcome.Migrated)
   Debug.Log("Save migrated to current schema.");
  game.Career.activeCaseId=caseId;
  instantText=PlayerPrefs.GetInt("bube.instantText",0)==1;
  SoundSettings.Load();
  AdGateway.Load();
  audio=AudioDirector.Attach(gameObject);
  // Kit'in her düğmesi basıldığında ses ister; çalan tek yer burası.
  KarineUI.Sound=id=>{ if(audio!=null)audio.Play(id); };
  var doc=GetComponent<UIDocument>() ?? gameObject.AddComponent<UIDocument>();
  var panel=ScriptableObject.CreateInstance<PanelSettings>();
  panel.scaleMode=PanelScaleMode.ScaleWithScreenSize;
  panel.referenceResolution=new Vector2Int(1280,720);
  panel.screenMatchMode=PanelScreenMatchMode.MatchWidthOrHeight;
  panel.match=1;
  panel.themeStyleSheet=Resources.Load<ThemeStyleSheet>("Bube/DefaultTheme");
  doc.panelSettings=panel;
  fonts=FontSet.Load();KarineUI.Fonts=fonts;
  // Dosya/terminal dokusu mono kalir; govde ve baslik rolleri ayri dusunulur.
  // Arayüzün gövde yazısı **mono değildir**. Mono yalnız teknik metne aittir
  // (`Technical`): dosya numarası, tarih, saat, güven yüzdesi. Ekranların
  // tamamı monospace okunduğu için yazı kötü görünüyordu.
  dossierFont=fonts.Body;
  dossierBoldFont=fonts.BodyBold;
  if(fonts.Missing.Length>0)
   Debug.Log("Font rolleri mono'ya dusuyor (dosya eksik): "+string.Join(", ",fonts.Missing));
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
  if(saveNotice!=null)Text(root,T("save.fromFuture"),Muted,15);
 }

 // Geri tuşunun hedefi ekranın kendi "geri" eyleminin **aynısıdır**; ayrı bir
 // gezinti ağacı tutulmaz, yoksa ikisi birbirinden ayrı düşer.
 void Back(Action action) => escapeBack=action;

 void HandleEscape() {
  if(askingToQuit)return;
  var action=escapeBack;
  if(action!=null){action();return;}
  AskToQuit();
 }

 // Ana menüde geri tuşu oyunu doğrudan kapatmaz; kit'in onay modalını açar.
 void AskToQuit() {
  askingToQuit=true;
  KarineUI.Modal(root,T("quit.title"),T("quit.body"),
   T("quit.cancel"),()=>{askingToQuit=false;Home();},
   T("quit.confirm"),()=>{askingToQuit=false;QuitGame();},true);
 }

 void EnsureScene(string sceneName) {
  SetRoomSound(sceneName);
  if(SceneManager.GetActiveScene().name!=sceneName)
   SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
 }

 // Odanın sesi sahneden gelir; vaka kendi ortam sesini söyleyebilir
 // (`CaseData.ambienceId`), söylemezse odanın varsayılanı çalar. Ses dosyası
 // yoksa sessizdir — ekranlar bunu bilmek zorunda değil.
 void SetRoomSound(string sceneName) {
  if(audio==null)return;
  string caseAmbience=game!=null && !string.IsNullOrEmpty(game.Data.ambienceId)?game.Data.ambienceId:null;
  switch(sceneName) {
   case "MainMenuScene": audio.PlayMusic("menu_theme"); audio.PlayAmbience(null); break;
   case "InterviewScene": audio.StopMusic(); audio.PlayAmbience("room_interview"); break;
   default: audio.StopMusic(); audio.PlayAmbience(caseAmbience ?? "room_office"); break;
  }
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
  // Android geri tuşu ve masaüstünde Esc aynı olaydır.
  if(Input.GetKeyDown(KeyCode.Escape))HandleEscape();
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
  bool inOffice=SceneManager.GetActiveScene().name=="OfficeScene";
  if(HasIncomingFax || HasIncomingDocument) {
   if(HasIncomingFax && inOffice)AddFaxNotice();
   if(HasIncomingDocument && inOffice)AddDocumentNotice();
  } else if(inOffice)AddChainEndNotice();
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

 // Telefonda oyuncu uygulamadan çıkmaz, arkaya atar; işletim sistemi onu
 // haber vermeden kapatabilir. Bu yüzden arkaya atılma anı bir kayıt anıdır.
 void OnApplicationPause(bool paused) {
  if(!paused || game==null)return;
  Save();
 }

 // Okunamayan kaydi bozmadan yana kaldirir; donus degeri yeni yoldur, yoksa null.
 string SetAside(string path) {
  try {
   if(!File.Exists(path))return null;
   var aside=path+".newer";
   if(File.Exists(aside))File.Delete(aside);
   File.Move(path,aside);
   Debug.LogWarning("Save is from a newer schema; kept at "+aside);
   return aside;
  } catch(Exception e) { Debug.LogWarning("Save could not be set aside: "+e.Message); return null; }
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
  var bodyFont=fonts!=null?fonts.Body:dossierFont;
  if(bodyFont!=null)label.style.unityFontDefinition=FontDefinition.FromFont(bodyFont);
  label.style.marginBottom=12;
  parent.Add(label);
  return label;
 }
 // Görüşme talepleri kartında eylem sütunu dardır (%30). Oradaki düğme metni
 // ("İfade alınmasını iste") tek satıra sığmıyordu ve kırpılıyordu.
 static void FitActionButton(VisualElement column) {
  if(column.childCount==0)return;
  var button=column.Children().Last() as Button;
  if(button==null)return;
  button.style.whiteSpace=WhiteSpace.Normal;
  button.style.paddingLeft=12;button.style.paddingRight=12;
  button.style.paddingTop=8;button.style.paddingBottom=8;
  button.style.fontSize=Typography.Snap(16);
 }

 // Tek düğme kapısı. Biçim `KarineUI`den gelir (kit'in PRIMARY/SECONDARY
 // hiyerarşisi); burada yalnız listelerde okunan sola yaslı yerleşim kalır.
 void Button(VisualElement parent,string value,Action onClick,bool primary=false) {
  var button=KarineUI.Button_(parent,value,onClick,
   primary?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.unityTextAlign=TextAnchor.MiddleLeft;
  button.style.marginRight=0;
  button.style.marginBottom=KarineTheme.SpaceSm;
 }
 // Teknik metin: dosya numarası, tarih/saat, güven yüzdesi — kit bunları
 // monospace ister.
 Label Technical(VisualElement parent,string value,int size=15) => KarineUI.Technical(parent,value,size);
 VisualElement Panel(VisualElement parent,int grow=0) {
  var panel=new VisualElement();
  panel.style.backgroundColor=KarineTheme.Panel;
  KarineUI.Border(panel,KarineTheme.BorderWidth,KarineTheme.Panel2);
  KarineUI.Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=KarineTheme.SpaceXl;
  panel.style.paddingRight=KarineTheme.SpaceXl;
  panel.style.paddingTop=KarineTheme.SpaceLg;
  panel.style.paddingBottom=KarineTheme.SpaceLg;
  panel.style.marginRight=KarineTheme.SpaceMd;
  panel.style.marginBottom=KarineTheme.SpaceMd;
  if(grow>0)panel.style.flexGrow=grow;
  parent.Add(panel);
  return panel;
 }
 void Frame(string kicker,string title,string subtitle) {
  root.Clear();
  // Diger ekranlarda kompakt baslık surumu: ayni gorsel, kucuk genislik.
  KarineLogo.Header(root,190,Gold);
  Text(root,kicker,Gold,16);
  var heading=Text(root,title,Ink,38);
  if(fonts!=null && fonts.Heading!=null)heading.style.unityFontDefinition=FontDefinition.FromFont(fonts.Heading);
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
 // Metin harf harf yazılırken ne duyulacağı **satırın kime ait olduğuna**
 // bağlıdır: resmî bir kâğıt daktiloyla basılır, bir insan konuşur. Ses
 // kimliği, perdesi ve sıklığı bu yüzden çağrı yerinden gelir.
 void Typewriter(Label label,string line,string soundId=AudioDirector.Typewriter,
                 float pitch=1f,float gain=1f,int every=4) =>
  Typewriter(label,line,new[]{soundId},pitch,gain,every);

 void Typewriter(Label label,string line,string[] sounds,
                 float pitch=1f,float gain=1f,int every=4) {
  if(instantText){label.text=line;return;}
  label.text=string.Empty;
  int length=0;
  IVisualElementScheduledItem animation=null;
  int tick=0;
  animation=label.schedule.Execute(()=>{
   length=Mathf.Min(line.Length,length+2);
   label.text=line.Substring(0,length);
   // Her karede değil: harf harf çalarsa gürültü olur.
   if(audio!=null && ++tick%every==0)
    // Perdedeki ve hece seçimindeki küçük oynama konuşmayı makineden ayırır.
    audio.Play(sounds[UnityEngine.Random.Range(0,sounds.Length)],
     pitch*(0.97f+0.06f*UnityEngine.Random.value),gain*(0.85f+0.3f*UnityEngine.Random.value));
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
  shade.style.backgroundColor=KarineTheme.Veil(.84f);root.Add(shade);
  var backing=new VisualElement();backing.style.position=Position.Absolute;
  backing.style.left=Length.Percent(wide?6:20);backing.style.right=Length.Percent(wide?6:19);
  backing.style.top=Length.Percent(7);backing.style.bottom=Length.Percent(5);
  backing.style.backgroundColor=KarineTheme.Paper.Folder;
  backing.style.borderBottomWidth=7;backing.style.borderBottomColor=KarineTheme.Paper.FolderDeep;root.Add(backing);
  var paper=new VisualElement();paper.style.position=Position.Absolute;
  paper.style.left=Length.Percent(wide?8:22);paper.style.right=Length.Percent(wide?8:21);
  paper.style.top=Length.Percent(5);paper.style.bottom=Length.Percent(7);
  paper.style.paddingLeft=28;paper.style.paddingRight=28;
  paper.style.paddingTop=18;paper.style.paddingBottom=15;
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;root.Add(paper);
  var dark=KarineTheme.Paper.Ink;
  var kicker=Text(paper,T(game.Data.titleKey),KarineTheme.Paper.Stamp,14);kicker.style.marginBottom=5;
  var title=Text(paper,heading,dark,26);title.style.marginBottom=5;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(paper,subtitle,dark,14);
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Paper.Edge;
  rule.style.marginBottom=9;paper.Add(rule);
  body=Scroll(paper);
  var back=KarineUI.PaperButton(paper,T(backAction==null?"back.file":"back.desk"),backAction ?? (Action)FilePage);
  Back(backAction ?? (Action)FilePage);
  back.style.minHeight=42;back.style.fontSize=Typography.Snap(16);
 }
 Button ReportChoice(VisualElement parent,string label,bool selected,Action choose) {
  var option=KarineUI.PaperButton(parent,(selected?"✓  ":"□  ")+label,choose,
   selected?KarinePaperKind.Action:KarinePaperKind.Choice,true);
  option.style.minHeight=40;option.style.fontSize=Typography.Snap(17);
  option.style.marginBottom=4;
  return option;
 }
 void RefreshReportChoices(List<Button> buttons,List<string> labels,List<string> ids,string selected) {
  for(int i=0;i<buttons.Count;i++) {
   bool active=ids[i]==selected;
   buttons[i].text=(active?"✓  ":"□  ")+labels[i];
   buttons[i].style.backgroundColor=active?KarineTheme.Paper.Stamp:KarineTheme.Paper.Tint;
   buttons[i].style.color=active?KarineTheme.Primary:KarineTheme.Paper.Ink;
  }
 }

}
}
