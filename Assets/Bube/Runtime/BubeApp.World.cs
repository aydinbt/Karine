using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Presentation only: Worlds remains the authority for availability.
public sealed partial class BubeApp {
 WorldAtlas atlas;
 int worldPick=-1;
 Vector2 countryStripOffset;
 void WorldPage() {
  Back(Home);StopCctvVideo();EnsureScene("MainMenuScene");
  showingInterviewList=false;root.Clear();root.style.backgroundColor=KarineTheme.Background;
  atlas=atlas??Worlds.Load();var closed=Worlds.Closed(game.Career);
  if(atlas.countries.Count==0) {WorldEmpty();return;}
  if(worldPick<0||worldPick>=atlas.countries.Count) worldPick=Worlds.Resume(atlas,closed);
  var main=CasePageShell("CaseBrowser",true);

  // Ülke sekmeleri: koyu bir rafın içinde yatay kayar.
  var shelf=new VisualElement();shelf.style.flexShrink=0;
  shelf.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.7f);KarineUI.Round(shelf,KarineTheme.Radius);
  shelf.style.paddingLeft=KarineTheme.SpaceMd;shelf.style.paddingTop=KarineTheme.SpaceMd;shelf.style.paddingBottom=KarineTheme.SpaceMd;
  main.Add(shelf);
  var countries=new KarineScrollView(ScrollViewMode.Horizontal) {name="CountryStrip"};
  countries.style.height=KarineTheme.CaseFiles.CountryHeight;countries.style.flexShrink=0;
  countries.horizontalScrollerVisibility=ScrollerVisibility.Hidden;countries.verticalScrollerVisibility=ScrollerVisibility.Hidden;
  countries.contentContainer.style.flexDirection=FlexDirection.Row;shelf.Add(countries);
  for(int i=0;i<atlas.countries.Count;i++) {
   int selected=i;var country=atlas.countries[i];
   KarineUI.CountryTab(countries.contentContainer,country.id,T(country.nameKey),
    T("world.country.cases")+"  "+Worlds.CompletedIn(country,closed)+"/"+country.slots.Count,
    LoadWorldArt(country.image),i==worldPick,Worlds.CountryUnlocked(atlas,i,closed),()=> {
     countryStripOffset=countries.scrollOffset;worldPick=selected;WorldPage();
    });
  }
  countries.schedule.Execute(()=>countries.scrollOffset=countryStripOffset);

  // Dosya kartları: büzülmez, kayar.
  var current=atlas.countries[worldPick];
  bool unlocked=Worlds.CountryUnlocked(atlas,worldPick,closed);
  var cards=new KarineScrollView(ScrollViewMode.Horizontal) {name="CaseStrip"};
  cards.style.marginTop=KarineTheme.SpaceXl;
  cards.style.height=KarineTheme.CaseFiles.CardHeight;cards.style.flexShrink=0;cards.verticalScrollerVisibility=ScrollerVisibility.Hidden;
  cards.horizontalScrollerVisibility=ScrollerVisibility.Hidden;cards.contentContainer.style.flexDirection=FlexDirection.Row;
  main.Add(cards);
  for(int i=0;i<current.slots.Count;i++) {
   var slot=current.slots[i];var state=Worlds.SlotState(current,i,closed,unlocked);
   bool active=state==WorldSlotState.Active,done=state==WorldSlotState.Completed;
   Action press=active&&slot.caseId==game.Data.id?(Action)Desk:done?()=>WorldRecord(slot.caseId):null;
   var cover=LoadWorldArt(slot.image);
   if(cover==null&&!string.IsNullOrEmpty(slot.caseId)) cover=Resources.Load<Texture2D>("Bube/Art/Covers/"+slot.caseId);
   var file=done?KarineUI.CaseFileState.Closed:!active?KarineUI.CaseFileState.Locked
    :game.State.caseAccepted&&slot.caseId==game.Data.id?KarineUI.CaseFileState.Ongoing:KarineUI.CaseFileState.Open;
   string status=file==KarineUI.CaseFileState.Closed?T("world.state.closed"):file==KarineUI.CaseFileState.Ongoing?T("world.state.ongoing"):T("world.state.active");
   string summary=file==KarineUI.CaseFileState.Locked||string.IsNullOrEmpty(slot.caseId)?T("world.slot.lockedBody"):T(slot.caseId+".offer.summary");
   string title=string.IsNullOrEmpty(slot.titleKey)?"":T(slot.titleKey).Split(new[]{'—'},2).Last().Trim();
   KarineUI.PullOut(KarineUI.CaseFileCard(cards.contentContainer,string.IsNullOrEmpty(slot.caseId)?current.id+"-"+i:slot.caseId,
    T("world.browser.file")+" #"+(i+1).ToString("000"),title,summary,status,file,cover,press));
  }
  if(!unlocked) {var hint=KarineUI.Body_(main,T("world.locked.hint"),KarineTheme.CaseFiles.SmallSize);hint.style.marginTop=KarineTheme.SpaceMd;hint.style.color=KarineTheme.Secondary;}
  KarineUI.CardsDrop(cards.contentContainer);
 }
 // Vakalar ve Kariyer'in ortak iskeleti: canlı büro arka planı, örtü, sol sütun
 // ve sağdaki ana alan. `cases` hangi menü satırının amber olacağını seçer.
 VisualElement CasePageShell(string name,bool cases) {
  var art=Resources.Load<Texture2D>("Bube/Art/CaseBrowserBackdrop") ?? Resources.Load<Texture2D>("Bube/Art/OfficeRoomV2");
  var backdrop=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  backdrop.style.position=Position.Absolute;backdrop.style.left=0;backdrop.style.right=0;
  backdrop.style.top=0;backdrop.style.bottom=0;root.Add(backdrop);KarineUI.CaseBrowserStage(backdrop,root);
  var veil=new VisualElement {pickingMode=PickingMode.Ignore};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.55f);root.Add(veil);WorldSidebar(cases);
  var main=new VisualElement {name=name};
  main.style.position=Position.Absolute;main.style.left=KarineTheme.CaseFiles.MainLeft;
  main.style.right=KarineTheme.SpaceXl;main.style.top=KarineTheme.CaseFiles.Top;
  main.style.bottom=KarineTheme.CaseFiles.Bottom;root.Add(main);
  return main;
 }
 static Texture2D LoadWorldArt(string path)=>string.IsNullOrEmpty(path)?null:Resources.Load<Texture2D>(path);
 void WorldSidebar(bool cases) {
  var side=new VisualElement {name="CaseBrowserSidebar"};
  side.style.position=Position.Absolute;side.style.left=KarineTheme.CaseFiles.SidebarLeft;side.style.top=KarineTheme.CaseFiles.Top-KarineTheme.SpaceMd;
  side.style.bottom=KarineTheme.CaseFiles.Bottom;side.style.width=KarineTheme.CaseFiles.SidebarWidth;root.Add(side);
  KarineLogo.Aligned(side,KarineTheme.CaseFiles.LogoWidth);
  KarineLogo.Tagline(side,KarineTheme.CaseFiles.LogoWidth,T("menu.tagline")).style.marginBottom=KarineTheme.SpaceXl;
  MenuRow(side,"cine_skip",T(game.State.caseAccepted?"menu.row.continue":"menu.row.start"),game.State.caseAccepted?(Action)Desk:()=>MaybeWorldIntro(Desk),false);
  MenuRow(side,"folder",T("menu.row.chapters"),WorldPage,cases);
  MenuRow(side,"chart",T("menu.row.career"),StatisticsPage,!cases);
  MenuRow(side,"gear",T("menu.row.settings"),SettingsPage,false);
  MenuRow(side,"document",T("menu.about"),AboutPage,false);
  // Kimlik kartı menünün hemen altında; sütunun dibine yapışmaz.
  KarineUI.AgentCard(side,Resources.Load<Texture2D>("Bube/Characters/bora_portrait") ?? Resources.Load<Texture2D>("Bube/Characters/bora"),T("menu.identity.name"),T("menu.identity.role"),
   new[]{(T("menu.identity.unitLabel"),T("menu.identity.unit")),(T("menu.identity.locationLabel"),T("menu.identity.location"))},StatisticsPage)
   .style.marginTop=KarineTheme.SpaceMd;
 }
 void WorldEmpty() {
  var panel=KarineUI.Panel(root,true);KarineUI.Title(panel,T("world.page.title"));KarineUI.Body_(panel,T("world.slot.unwritten"));Button(panel,T("world.back"),Home);
 }
 string WorldStateLabel(WorldSlotState state)=>state==WorldSlotState.Completed?T("world.state.completed"):state==WorldSlotState.Active?T("world.state.active"):state==WorldSlotState.Unwritten?T("world.state.unwritten"):T("world.state.locked");
 void WorldRecord(string caseId) {
  var review=game.Career.reviewHistory.LastOrDefault(item=>item.caseId==caseId);
  if(review==null) WorldPage();else CareerRecordPage(review);
 }
}
}
