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
  var backdrop=new Image {image=Resources.Load<Texture2D>("Bube/DeskV2"),scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  backdrop.style.position=Position.Absolute;backdrop.style.left=0;backdrop.style.right=0;
  backdrop.style.top=0;backdrop.style.bottom=0;root.Add(backdrop);
  var veil=new VisualElement {pickingMode=PickingMode.Ignore};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.48f);root.Add(veil);WorldSidebar();
  var main=new VisualElement {name="CaseBrowser"};
  main.style.position=Position.Absolute;main.style.left=KarineTheme.CaseBrowser.MainLeft;
  main.style.right=KarineTheme.SpaceXl;main.style.top=KarineTheme.CaseBrowser.Top;
  main.style.bottom=KarineTheme.CaseBrowser.Bottom;root.Add(main);
  KarineUI.Title(main,T("world.page.title"),KarineTheme.CaseBrowser.TitleSize);
  KarineUI.Body_(main,T("world.browser.subtitle"),KarineTheme.CaseBrowser.TextSize);
  var countries=new ScrollView(ScrollViewMode.Horizontal) {name="CountryStrip"};
  countries.style.height=KarineTheme.CaseBrowser.CountryHeight+KarineTheme.SpaceXl;countries.style.flexShrink=0;
  countries.horizontalScrollerVisibility=ScrollerVisibility.Auto;
  countries.contentContainer.style.flexDirection=FlexDirection.Row;main.Add(countries);
  for(int i=0;i<atlas.countries.Count;i++) {
   int selected=i;var country=atlas.countries[i];
   KarineUI.CountryTile(countries.contentContainer,country.id,T(country.nameKey),country.slots.Count+" "+T("world.cases"),
    LoadWorldArt(country.image),i==worldPick,Worlds.CountryUnlocked(atlas,i,closed),()=> {
     countryStripOffset=countries.scrollOffset;worldPick=selected;WorldPage();
    });
  }
  countries.schedule.Execute(()=>countries.scrollOffset=countryStripOffset);
  var current=atlas.countries[worldPick];
  var board=KarineUI.Panel(main,true);board.name="CaseBoard";
  board.style.marginTop=KarineTheme.SpaceSm;board.style.marginLeft=0;board.style.marginRight=0;
  board.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Board,.88f);
  var head=KarineUI.Row(board);
  var title=KarineUI.Title(head,T(current.nameKey),KarineTheme.CaseBrowser.TitleSize-KarineTheme.SpaceMd);
  title.style.flexGrow=1;title.style.marginBottom=0;
  KarineUI.Technical(head,Worlds.CompletedIn(current,closed)+" / "+current.slots.Count,KarineTheme.CaseBrowser.TextSize)
   .style.marginRight=KarineTheme.SpaceMd;
  var cards=new ScrollView(ScrollViewMode.Horizontal) {name="CaseStrip"};
  cards.horizontalScrollerVisibility=ScrollerVisibility.Auto;cards.contentContainer.style.flexDirection=FlexDirection.Row;
  KarineUI.IconButton(head,"nav_prev",()=>cards.scrollOffset=new Vector2(Mathf.Max(0,cards.scrollOffset.x-KarineTheme.CaseBrowser.CardWidth-KarineTheme.SpaceMd),0),T("world.browser.previous"));
  KarineUI.IconButton(head,"nav_next",()=>cards.scrollOffset=new Vector2(cards.scrollOffset.x+KarineTheme.CaseBrowser.CardWidth+KarineTheme.SpaceMd,0),T("world.browser.next"));
  board.Add(cards);bool unlocked=Worlds.CountryUnlocked(atlas,worldPick,closed);
  for(int i=0;i<current.slots.Count;i++) {
   var slot=current.slots[i];var state=Worlds.SlotState(current,i,closed,unlocked);
   bool active=state==WorldSlotState.Active,done=state==WorldSlotState.Completed;
   Action press=active&&slot.caseId==game.Data.id?(Action)Desk:done?()=>WorldRecord(slot.caseId):null;
   var art=LoadWorldArt(slot.image);
   if(art==null&&slot.caseId=="case001") art=Resources.Load<Texture2D>("Bube/Case001Building");
   if(art==null&&slot.caseId=="case002") art=Resources.Load<Texture2D>("Bube/Art/Case002Cover");
   KarineUI.CasePhotoCard(cards.contentContainer,string.IsNullOrEmpty(slot.caseId)?current.id+"-"+i:slot.caseId,
    current.id,T("world.browser.case")+" "+(i+1).ToString("000"),T(slot.titleKey),WorldStateLabel(state),art,active,done,press);
  }
  if(!unlocked) KarineUI.Body_(board,T("world.locked.hint"),KarineTheme.CaseBrowser.SmallSize);
  FadeIn(main);
 }
 static Texture2D LoadWorldArt(string path)=>string.IsNullOrEmpty(path)?null:Resources.Load<Texture2D>(path);
 void WorldSidebar() {
  var side=new VisualElement {name="CaseBrowserSidebar"};
  side.style.position=Position.Absolute;side.style.left=KarineTheme.SpaceXl;side.style.top=KarineTheme.CaseBrowser.Top;
  side.style.bottom=KarineTheme.CaseBrowser.Bottom;side.style.width=KarineTheme.CaseBrowser.SidebarWidth;
  side.style.paddingLeft=KarineTheme.SpaceMd;side.style.paddingRight=KarineTheme.SpaceMd;
  side.style.paddingTop=KarineTheme.SpaceLg;side.style.paddingBottom=KarineTheme.SpaceMd;
  side.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.94f);
  KarineUI.Border(side,KarineTheme.BorderWidth,KarineTheme.Panel2);root.Add(side);
  KarineLogo.Hero(side,KarineTheme.CaseBrowser.LogoWidth);
  KarineUI.Technical(side,T("menu.tagline"),KarineTheme.CaseBrowser.SmallSize).style.marginBottom=KarineTheme.SpaceXl;
  MenuRow(side,"folder",T(game.State.caseAccepted?"menu.row.continue":"menu.row.start"),game.State.caseAccepted?(Action)Desk:()=>MaybeWorldIntro(Desk),false);
  MenuRow(side,"document",T("menu.row.chapters"),WorldPage,true);
  MenuRow(side,"chart",T("menu.row.career"),StatisticsPage,false);
  MenuRow(side,"gear",T("menu.row.settings"),SettingsPage,false);
  MenuRow(side,"info",T("menu.about"),AboutPage,false);
  var person=new VisualElement();person.style.flexGrow=1;person.style.justifyContent=Justify.FlexEnd;side.Add(person);
  var portrait=new Image {image=Resources.Load<Texture2D>("Bube/Characters/bora"),scaleMode=ScaleMode.ScaleAndCrop};
  portrait.style.height=KarineTheme.CaseBrowser.Portrait;portrait.style.width=KarineTheme.CaseBrowser.Portrait;person.Add(portrait);
  KarineUI.Subtitle(person,T("menu.identity.name"),KarineTheme.MainMenu.RowTextSize).style.marginBottom=0;
  KarineUI.Body_(person,T("menu.identity.role")+"\n"+T("menu.identity.unit")+"\n"+T("menu.identity.location"),KarineTheme.CaseBrowser.SmallSize).style.marginBottom=0;
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
