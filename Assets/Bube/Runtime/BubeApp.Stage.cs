using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Sahne katmanının ikinci yarısı (P–X): vaka açılışı, masadaki kapanmış
// dosyalar rafı, görüşmeden çıkış, kariyer duvarı, kayıt satırı, erişilebilirlik
// seçenekleri ve geliştirici efekt laboratuvarı. `BubeApp`in parçasıdır.
public sealed partial class BubeApp {
 const string CaptionsKey="karine.captions",SpacingKey="karine.spacing",ContrastKey="karine.contrast",OneHandKey="karine.oneHand",PlayKey="karine.playSeconds";
 bool leavingRoom,heardHooked,draftCaptions,draftSpacing,draftContrast,draftOneHand;
 int draftStrength;
 float playSeconds=-1,playSaved;

 static bool Captions => PlayerPrefs.GetInt(CaptionsKey,1)==1;
 static bool Spacing => PlayerPrefs.GetInt(SpacingKey,0)==1;
 static bool HighContrast => PlayerPrefs.GetInt(ContrastKey,0)==1;
 static bool OneHand => PlayerPrefs.GetInt(OneHandKey,0)==1;

 void StageTick() {
  PolishTick();
  if(!heardHooked){heardHooked=true;AudioDirector.Heard+=OnHeard;}
  if(leavingRoom){leavingRoom=false;KarineUI.LeaveRoom(root);}
  if(Time.frameCount%30==0)KarineUI.AccessPass(root,Spacing,HighContrast);
  KarineUI.ThumbBack(root,OneHand && escapeBack!=null,escapeBack==null?null:(Action)HandleEscape,T("offer.back"));
  if(playSeconds<0)playSeconds=PlayerPrefs.GetFloat(PlayKey,0);
  playSeconds+=Time.unscaledDeltaTime;
  if(playSeconds-playSaved>30){playSaved=playSeconds;PlayerPrefs.SetFloat(PlayKey,playSeconds);}
 }
 void OnDestroy(){AudioDirector.Heard-=OnHeard;}

 // Ses betimlemesi: yalnız duyulan sesin adı; anlamı yok.
 void OnHeard(string id) {
  if(!Captions || root==null)return;
  string key="caption."+id,text=T(key);
  if(text!=key)KarineUI.Caption(root,text);
 }

 // Masa kurulunca: ışık akışı, raf ve (vaka başına bir kez) mekân kareleri ile açılış kartı.
 void StageDesk(VisualElement stage) {
  KarineUI.DayDrift(stage);
  // Eski masa plakasına göre yerleşmiş süsler (raf, takvim, lamba düğmesi, telefon kablosu, şehir ışıkları)
  // yeni sahnede yanlış yere düşüyordu; 3 Ekim 2026'da kaldırıldı. Lamba rengi ayarı sürer.
  LampTintOnly(stage);
  if(!game.State.caseAccepted || game.State.closed)return;
  string key="karine.opened."+game.Data.id;
  if(PlayerPrefs.GetInt(key,0)==1)return;
  PlayerPrefs.SetInt(key,1);PlayerPrefs.Save();
  // Vaka adı ayrıca büyük yazılmaz (3 Ekim 2026): üst şeritte ve teklifte zaten yazıyor.
  var frames=(game.Data.locationFrames ?? new string[0]).Select(p=>Resources.Load<Texture2D>(p)).Where(t=>t!=null).ToArray();
  root.schedule.Execute(()=>KarineUI.ChapterCard(root,ChapterTime(),ChapterPlace(),()=>
   KarineUI.LocationReel(root,frames,()=>audioDirector?.Sting()))).StartingIn(0);
 }

 // Kapanmış dosyalar rafı: masanın sol altında, her kapanan vaka için bir sırt.
 void ClosedShelf(VisualElement stage) {
  var front=stage.Q("OfficeFront") ?? stage;
  var history=game.Career.reviewHistory;
  if(history==null || history.Count==0)return;
  var shelf=new VisualElement {name="ClosedShelf"};
  KarineUI.OfficePlace(shelf,KarineTheme.Scene.Shelf);
  shelf.style.flexDirection=FlexDirection.Row;shelf.style.alignItems=Align.FlexEnd;
  shelf.style.borderBottomWidth=3;shelf.style.borderBottomColor=KarineTheme.Paper.FolderDeep;
  front.Add(shelf);
  foreach(var review in history.Take(8)) {
   var picked=review;
   var spine=new Button(KarineUI.Sounded(()=>CareerRecordPage(picked))) {tooltip=picked.caseId};
   spine.style.width=Length.Percent(11);spine.style.height=Length.Percent(80+picked.caseId.Length%3*7);
   spine.style.marginRight=2;spine.style.paddingLeft=0;spine.style.paddingRight=0;
   spine.style.backgroundColor=KarineTheme.Paper.Folder;KarineUI.Border(spine,1,KarineTheme.Paper.Edge);
   shelf.Add(spine);
  }
 }

 // Kayıt satırı: son vaka ve oynanan süre, devam satırının altında.
 void SlotLine(VisualElement menu) {
  float seconds=PlayerPrefs.GetFloat(PlayKey,0);
  int hours=Mathf.FloorToInt(seconds/3600),minutes=Mathf.FloorToInt(seconds%3600/60);
  var line=KarineUI.Technical(menu,T(game.Data.titleKey)+"  ·  "+hours+T("slot.hours")+" "+minutes.ToString("00")+T("slot.minutes"),13);
  line.style.color=KarineTheme.Secondary;line.style.marginTop=-4;line.style.marginBottom=KarineTheme.SpaceSm;line.style.marginLeft=KarineTheme.SpaceMd;
 }

 // Kariyer duvarı: kapanan her vaka için bir kupür.
 void CareerWall(VisualElement box,System.Collections.Generic.List<FaxReview> history) {
  if(history.Count==0)return;
  var wall=new KarineScrollView(ScrollViewMode.Horizontal);wall.style.flexShrink=0;wall.style.minHeight=118;
  wall.contentContainer.style.flexDirection=FlexDirection.Row;box.Add(wall);
  int index=0;
  foreach(var review in history.AsEnumerable().Reverse()) {
   var picked=review;
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   string title=data==null?review.caseId:T(data.titleKey).Split(new[]{'—'},2).Last().Trim();
   KarineUI.Clipping(wall.contentContainer,title,EvaluationTitle(review),index++,()=>CareerRecordPage(picked));
  }
 }

 // Ayarlar: okuma ve erişilebilirlik.
 void LoadAccessDraft(){draftCaptions=Captions;draftSpacing=Spacing;draftContrast=HighContrast;draftOneHand=OneHand;draftStrength=Fx.Strength;}
 void ResetAccessDraft(){draftCaptions=true;draftSpacing=false;draftContrast=false;draftOneHand=false;draftStrength=2;}
 void SaveAccessDraft() {
  PlayerPrefs.SetInt(CaptionsKey,draftCaptions?1:0);PlayerPrefs.SetInt(SpacingKey,draftSpacing?1:0);
  PlayerPrefs.SetInt(ContrastKey,draftContrast?1:0);PlayerPrefs.SetInt(OneHandKey,draftOneHand?1:0);Fx.SetStrength(draftStrength);
 }

 // Efekt laboratuvarı (yalnız geliştirme derlemesi): her efekt tek dokunuşla
 // denenir; "Konum" görüşme odasını açıp nesneleri sürüklenebilir yapar.
 void DevLab() {
  Back(()=>{Home();SettingsPage();});
  root.Clear();root.style.backgroundColor=KarineTheme.GlassDeep;
  var body=new VisualElement();body.style.flexGrow=1;body.style.paddingLeft=24;body.style.paddingRight=24;body.style.paddingTop=16;root.Add(body);
  var header=KarineUI.Row(body);header.style.justifyContent=Justify.SpaceBetween;
  KarineUI.Technical(header,T("settings.devLab"),KarineTheme.Office.TitleSize).style.color=KarineTheme.Primary;
  KarineUI.IconButton(header,"close",()=>{Home();SettingsPage();},T("offer.back"));
  var grid=new VisualElement();grid.style.flexDirection=FlexDirection.Row;grid.style.flexWrap=Wrap.Wrap;body.Add(grid);
  var stage=new VisualElement();stage.style.flexGrow=1;stage.style.marginTop=12;stage.style.backgroundColor=KarineTheme.Glass;stage.style.overflow=Overflow.Hidden;body.Add(stage);
  var art=new[]{Resources.Load<Texture2D>("Bube/Case001Building"),Resources.Load<Texture2D>("Bube/InterviewRoom")}.Where(t=>t!=null).ToArray();
  var parts=T(game.Data.titleKey).Split(new[]{'—'},2);
  var tests=new (string label,Action run)[]{
   ("Açılış",()=>KarineUI.CaseOpening(root,parts[0].Trim(),parts.Last().Trim(),null)),
   ("Mekân",()=>KarineUI.LocationReel(root,art,null)),
   ("Çıkış",()=>KarineUI.LeaveRoom(root)),
   ("Zarf",()=>KarineUI.EnvelopeSeal(root,v=>v?.RemoveFromHierarchy())),
   ("Mühür",()=>KarineUI.StampDown(root,T("report.stamp"),null)),
   ("Yanık",()=>KarineUI.FilmBurn(root,null)),
   ("Kapandı",()=>KarineUI.CaseClosed(root,parts[0].Trim(),parts.Last().Trim(),T("case.closed"),null)),
   ("Rütbe",()=>KarineUI.RankCeremony(root,T(game.TrustStatusKey),null)),
   ("Bağlantı",()=>KarineUI.Connecting(root,T("terminal.connecting"))),
   ("Geri sar",()=>KarineUI.Rewind(stage)),
   ("Kayma",()=>KarineUI.ChromaShake(root)),
   ("Altyazı",()=>KarineUI.Caption(root,T("caption.amb_phone"))),
   ("Motif",()=>audioDirector?.Sting()),
   ("Konum",PlaceRoom),
  }.Concat(PolishTests(stage)).ToArray();
  foreach(var test in tests){var b=KarineUI.Button_(grid,test.label,test.run,KarineButtonKind.Secondary);b.style.marginRight=6;b.style.marginBottom=6;}
 }
 void PlaceRoom() {
  var people=game.Data.nodes.FirstOrDefault(n=>n.kind=="interview" && game.Available(n));
  if(people==null)return;
  InterviewPage(people);
  root.schedule.Execute(()=>KarineUI.Placer(root,"RoomLamp","RoomGlass","RoomMirror","RoomRecorder","RoomClock")).StartingIn(300);
 }
}
}
