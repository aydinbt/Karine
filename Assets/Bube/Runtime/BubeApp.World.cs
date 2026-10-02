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
  var backdrop=new Image {image=Resources.Load<Texture2D>("Bube/Art/OfficeRoomV2"),scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  backdrop.style.position=Position.Absolute;backdrop.style.left=0;backdrop.style.right=0;
  backdrop.style.top=0;backdrop.style.bottom=0;root.Add(backdrop);
  var veil=new VisualElement {pickingMode=PickingMode.Ignore};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.48f);root.Add(veil);WorldSidebar();
  var main=new VisualElement {name="CaseBrowser"};
  main.style.position=Position.Absolute;main.style.left=KarineTheme.CaseBrowser.MainLeft;
  main.style.right=KarineTheme.SpaceXl;main.style.top=KarineTheme.CaseBrowser.Top;
  main.style.bottom=KarineTheme.CaseBrowser.Bottom;root.Add(main);
  main.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);
  main.style.paddingLeft=KarineTheme.SpaceMd;main.style.paddingRight=KarineTheme.SpaceMd;main.style.paddingTop=KarineTheme.SpaceMd;
  KarineUI.Border(main,KarineTheme.BorderWidth,KarineTheme.Panel2);
  var heading=KarineUI.Row(main);heading.style.flexShrink=0;heading.style.height=KarineTheme.CaseBrowser.HeadingHeight;var words=new VisualElement();words.style.flexGrow=1;heading.Add(words);
  KarineUI.Title(words,T("world.page.title"),KarineTheme.CaseBrowser.TitleSize).style.marginBottom=0;
  KarineUI.Body_(words,T("world.browser.subtitle"),KarineTheme.CaseBrowser.SmallSize);
  int total=atlas.countries.Sum(c=>c.slots.Count),completed=atlas.countries.Sum(c=>Worlds.CompletedIn(c,closed));
  int finishedWorlds=atlas.countries.Count(c=>c.slots.Count>0&&Worlds.CompletedIn(c,closed)==c.slots.Count);
  KarineUI.BrowserProgress(heading,T("world.browser.progress"),finishedWorlds+" / "+atlas.countries.Count+" "+T("world.browser.worlds")+" · "+completed+" / "+total+" "+T("world.cases"),total==0?0:(float)completed/total);
  var countries=new KarineScrollView(ScrollViewMode.Horizontal) {name="CountryStrip"};
  countries.style.height=KarineTheme.CaseBrowser.CountryHeight+KarineTheme.SpaceXl;countries.style.flexShrink=0;
  countries.horizontalScrollerVisibility=ScrollerVisibility.Hidden;
  countries.contentContainer.style.flexDirection=FlexDirection.Row;main.Add(countries);
  for(int i=0;i<atlas.countries.Count;i++) {
   int selected=i;var country=atlas.countries[i];
   KarineUI.CountryTile(countries.contentContainer,country.id,T(country.nameKey),Worlds.CompletedIn(country,closed)+" / "+country.slots.Count,
    LoadWorldArt(country.image),i==worldPick,Worlds.CountryUnlocked(atlas,i,closed),()=> {
     countryStripOffset=countries.scrollOffset;worldPick=selected;WorldPage();
    },country.slots.Count==0?0:(float)Worlds.CompletedIn(country,closed)/country.slots.Count);
  }
  countries.schedule.Execute(()=>countries.scrollOffset=countryStripOffset);
  var stripRow=KarineUI.Row(main);stripRow.style.flexShrink=0;stripRow.style.alignItems=Align.Center;main.Insert(main.IndexOf(countries),stripRow);
  countries.style.flexGrow=1;stripRow.Add(countries);
  KarineUI.IconButton(stripRow,"nav_next",()=>countries.scrollOffset=new Vector2(countries.scrollOffset.x+KarineTheme.CaseBrowser.CountryWidth+KarineTheme.SpaceSm,0),T("world.browser.next"));
  var current=atlas.countries[worldPick];
  var board=KarineUI.Panel(main,true);board.name="CaseBoard";
  board.style.marginTop=KarineTheme.SpaceSm;board.style.marginLeft=0;board.style.marginRight=0;
  board.style.backgroundColor=KarineTheme.GlassDeep;board.style.flexShrink=0;
  board.style.paddingLeft=KarineTheme.SpaceSm;board.style.paddingRight=KarineTheme.SpaceSm;board.style.paddingTop=KarineTheme.SpaceSm;board.style.paddingBottom=KarineTheme.SpaceSm;
  KarineUI.BrowserBanner(board,current.id,LoadWorldArt(current.image),T(current.nameKey),T(current.descriptionKey),current.slots.Count+" "+T("world.cases").ToUpper(new System.Globalization.CultureInfo("tr-TR")));
  var head=KarineUI.Row(board);
  var title=KarineUI.Body_(head,T("world.cases"),KarineTheme.CaseBrowser.SmallSize);
  title.style.flexGrow=1;title.style.marginBottom=0;
  KarineUI.Technical(head,Worlds.CompletedIn(current,closed)+" / "+current.slots.Count,KarineTheme.CaseBrowser.TextSize)
   .style.marginRight=KarineTheme.SpaceMd;
  var cards=new KarineScrollView(ScrollViewMode.Horizontal) {name="CaseStrip"};
  cards.style.height=KarineTheme.CaseBrowser.CardHeight+KarineTheme.SpaceMd;cards.style.flexShrink=0;cards.verticalScrollerVisibility=ScrollerVisibility.Hidden;
  cards.horizontalScrollerVisibility=ScrollerVisibility.Hidden;cards.contentContainer.style.flexDirection=FlexDirection.Row;
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
    current.id,T("world.browser.case")+" "+(i+1).ToString("000"),T(slot.titleKey).Split(new[]{'—'},2).Last().Trim(),WorldStateLabel(state),art,active,done,press,(i+1).ToString("00"));
  }
  KarineUI.BrowserSteps(board,current.slots.Select((s,i)=>Worlds.SlotState(current,i,closed,unlocked)==WorldSlotState.Completed).ToArray(),
   Array.FindIndex(current.slots.ToArray(),s=>Worlds.SlotState(current,current.slots.IndexOf(s),closed,unlocked)==WorldSlotState.Active));
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
