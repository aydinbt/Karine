using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Bölüm seçici: üstte kimlik şeridi, solda ülke listesi, ortada dünya panosu,
// sağda seçili ülkenin kartı, altta o ülkenin yedi dosyası.
// Ekran hiçbir yere **götürmez**: kilit yalnız ilerlemeyi gösterir, sıradaki
// adımı söylemez.
public sealed partial class BubeApp {
 WorldAtlas atlas;
 int worldPick = -1;

 void WorldPage() {
  Back(Home);
  StopCctvVideo();
  EnsureScene("MainMenuScene");
  showingInterviewList = false;
  root.Clear();
  root.style.backgroundColor = KarineTheme.Background;

  atlas = atlas ?? Worlds.Load();
  var closed = Worlds.Closed(game.Career);
  if (atlas.countries.Count == 0) { WorldEmpty(); return; }
  if (worldPick < 0 || worldPick >= atlas.countries.Count) worldPick = Worlds.Resume(atlas, closed);

  WorldTopBar(closed);

  var middle = new VisualElement();
  middle.style.flexDirection = FlexDirection.Row;
  middle.style.flexGrow = 1;
  middle.style.paddingLeft = KarineTheme.SpaceLg;
  middle.style.paddingRight = KarineTheme.SpaceLg;
  root.Add(middle);

  WorldCountryList(middle, closed);
  WorldMap(middle, closed);
  WorldDetail(middle, closed);

  WorldCaseStrip(closed);
 }

 void WorldEmpty() {
  var panel = KarineUI.Panel(root, true);
  panel.style.marginTop = KarineTheme.SpaceXl;
  panel.style.marginLeft = KarineTheme.SpaceXl;
  panel.style.marginRight = KarineTheme.SpaceXl;
  KarineUI.Title(panel, T("world.page.title"), 24);
  KarineUI.Body_(panel, T("world.slot.unwritten"));
  Button(panel, T("world.back"), Home);
 }

 // --- Üst şerit -------------------------------------------------------------

 void WorldTopBar(ICollection<string> closed) {
  var bar = new VisualElement();
  bar.style.flexDirection = FlexDirection.Row;
  bar.style.alignItems = Align.Center;
  bar.style.backgroundColor = KarineTheme.Glass;
  bar.style.paddingLeft = KarineTheme.SpaceLg;
  bar.style.paddingRight = KarineTheme.SpaceLg;
  bar.style.paddingTop = KarineTheme.SpaceSm;
  bar.style.paddingBottom = KarineTheme.SpaceSm;
  bar.style.borderBottomWidth = KarineTheme.BorderWidth;
  bar.style.borderBottomColor = KarineTheme.Accent;
  root.Add(bar);

  KarineLogo.Hero(bar, 150);

  var portrait = WorldPortrait(52);
  portrait.style.marginLeft = KarineTheme.SpaceLg;
  portrait.style.marginRight = KarineTheme.SpaceMd;
  bar.Add(portrait);

  var who = new VisualElement();
  who.style.marginRight = KarineTheme.SpaceXl;
  bar.Add(who);
  var name = KarineUI.Subtitle(who, T("career.bora"), 17);
  name.style.marginBottom = 0;
  KarineUI.Technical(who, T("career.rank." + (game.Career.careerRankId ?? "investigator")), 13)
   .style.marginBottom = 0;

  var trust = KarineUI.Meter(bar, "gear", T(game.TrustStatusKey), game.Career.departmentTrust / 100f);
  trust.style.width = 220;
  trust.style.marginBottom = 0;
  trust.style.marginRight = KarineTheme.SpaceMd;

  var counter = KarineUI.Counter(bar, "folder", T("world.completedCases"),
   Worlds.TotalCompleted(atlas, closed) + " / " + Worlds.TotalCases(atlas));
  counter.style.width = 180;
  counter.style.marginBottom = 0;
  counter.style.marginRight = KarineTheme.SpaceMd;

  var gear = KarineUI.IconButton(bar, "gear", SettingsPage, T("menu.settings"));
  gear.style.marginRight = 0;
  var close = KarineUI.IconButton(bar, "close", Home, T("world.back"));
  close.style.marginRight = 0;
 }

 // Kimlik şeridinin küçük portresi. PNG yoksa piksel portre çizilir; ikisi de
 // yoksa baş harf durur — ekran portre yüzünden boş kalmaz.
 VisualElement WorldPortrait(int size) {
  var frame = new VisualElement();
  frame.style.width = size; frame.style.height = size; frame.style.flexShrink = 0;
  frame.style.alignItems = Align.Center; frame.style.justifyContent = Justify.Center;
  frame.style.backgroundColor = KarineTheme.GlassDeep;
  KarineUI.Border(frame, KarineTheme.BorderWidth, KarineTheme.Accent);
  var art = Resources.Load<Texture2D>("Bube/Characters/" + (config.investigatorKey ?? "bora"));
  if (art != null) {
   art.filterMode = FilterMode.Point;
   var image = new Image { image = art, scaleMode = ScaleMode.ScaleAndCrop, pickingMode = PickingMode.Ignore };
   image.style.position = Position.Absolute;
   image.style.left = 0; image.style.right = 0; image.style.top = 0; image.style.bottom = 0;
   frame.Add(image);
  } else {
   var initial = KarineUI.Title(frame, T("career.bora").Substring(0, 1), 21);
   initial.style.marginBottom = 0;
  }
  return frame;
 }

 // --- Sol: ülke listesi -----------------------------------------------------

 void WorldCountryList(VisualElement parent, ICollection<string> closed) {
  var column = new VisualElement();
  column.style.width = Length.Percent(24);
  column.style.marginRight = KarineTheme.SpaceLg;
  column.style.marginTop = KarineTheme.SpaceMd;
  parent.Add(column);

  var scroll = new ScrollView(ScrollViewMode.Vertical);
  scroll.style.flexGrow = 1;
  scroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
  column.Add(scroll);

  for (int index = 0; index < atlas.countries.Count; index++) {
   var country = atlas.countries[index];
   int picked = index;
   bool unlocked = Worlds.CountryUnlocked(atlas, index, closed);
   bool selected = index == worldPick;
   var row = new Button(KarineUI.Sounded(() => { worldPick = picked; worldStripFocus = false; WorldPage(); }));
   row.style.flexDirection = FlexDirection.Row;
   row.style.alignItems = Align.Center;
   row.style.minHeight = MinimumTouchTarget;
   row.style.marginLeft = 0; row.style.marginRight = 0;
   row.style.marginTop = 0; row.style.marginBottom = KarineTheme.SpaceXs;
   row.style.paddingLeft = KarineTheme.SpaceMd;
   row.style.paddingRight = KarineTheme.SpaceMd;
   row.style.backgroundColor = selected ? KarineTheme.GlassLift : KarineTheme.Glass;
   KarineUI.Border(row, KarineTheme.BorderWidth, selected ? KarineTheme.Primary : KarineTheme.Panel2);
   if (selected) {
    row.style.borderLeftWidth = KarineTheme.PrimaryEdgeWidth;
    row.style.borderLeftColor = KarineTheme.Primary;
   }
   scroll.Add(row);

   var column2 = new VisualElement();
   column2.style.flexGrow = 1;
   row.Add(column2);
   var label = KarineUI.Subtitle(column2, T(country.nameKey), 17);
   label.style.marginBottom = 0;
   label.style.color = unlocked ? KarineTheme.Primary : KarineTheme.Disabled;
   var tally = KarineUI.Technical(column2,
    Worlds.CompletedIn(country, closed) + " / " + country.slots.Count + " " + T("world.cases"), 13);
   tally.style.marginBottom = 0;

   // Kit'te kilit/onay ikonu yok ve emoji kullanılmaz: durum bir nokta ve tek
   // kelimelik teknik etikettir.
   KarineUI.Dot(row, unlocked ? (Worlds.Finished(country, closed) ? KarineTone.Active : KarineTone.Neutral)
                              : KarineTone.Danger);
   if (!unlocked) {
    var state = KarineUI.Technical(row, T("world.state.locked"), 12);
    state.style.marginBottom = 0;
    state.style.color = KarineTheme.Disabled;
   } else if (selected) {
    var chevron = KarineUI.Subtitle(row, "›", 19);
    chevron.style.marginBottom = 0;
   }
  }
 }

 // --- Orta: dünya panosu ----------------------------------------------------

 // Maketteki harita görseli henüz çizilmedi. Görsel gelince `Bube/WorldMap`
 // olarak konur; yokken pano boş kalmaz, ülkeler pano üstünde iğnelerle durur.
 void WorldMap(VisualElement parent, ICollection<string> closed) {
  var board = new VisualElement();
  board.style.flexGrow = 1;
  board.style.marginTop = KarineTheme.SpaceMd;
  board.style.marginBottom = KarineTheme.SpaceMd;
  board.style.backgroundColor = KarineTheme.Paper.Board;
  KarineUI.Border(board, KarineTheme.BorderWidth, KarineTheme.Accent);
  parent.Add(board);

  var map = Resources.Load<Texture2D>("Bube/WorldMap");
  if (map != null) {
   var image = new Image { image = map, scaleMode = ScaleMode.ScaleToFit, pickingMode = PickingMode.Ignore };
   image.style.position = Position.Absolute;
   image.style.left = 0; image.style.right = 0; image.style.top = 0; image.style.bottom = 0;
   board.Add(image);
  }

  var pins = new VisualElement();
  pins.style.flexDirection = FlexDirection.Row;
  pins.style.flexWrap = Wrap.Wrap;
  pins.style.justifyContent = Justify.Center;
  pins.style.alignItems = Align.Center;
  pins.style.flexGrow = 1;
  pins.style.paddingLeft = KarineTheme.SpaceLg;
  pins.style.paddingRight = KarineTheme.SpaceLg;
  board.Add(pins);

  for (int index = 0; index < atlas.countries.Count; index++) {
   var country = atlas.countries[index];
   int picked = index;
   bool unlocked = Worlds.CountryUnlocked(atlas, index, closed);
   var pin = new Button(KarineUI.Sounded(() => { worldPick = picked; worldStripFocus = false; WorldPage(); }));
   pin.style.flexDirection = FlexDirection.Row;
   pin.style.alignItems = Align.Center;
   pin.style.minHeight = MinimumTouchTarget;
   pin.style.marginLeft = KarineTheme.SpaceXs; pin.style.marginRight = KarineTheme.SpaceXs;
   pin.style.marginTop = KarineTheme.SpaceXs; pin.style.marginBottom = KarineTheme.SpaceXs;
   pin.style.paddingLeft = KarineTheme.SpaceSm; pin.style.paddingRight = KarineTheme.SpaceMd;
   pin.style.backgroundColor = index == worldPick ? KarineTheme.Paper.Tint : KarineTheme.Paper.Sheet;
   KarineUI.Border(pin, KarineTheme.BorderWidth, KarineTheme.Paper.Edge);
   pins.Add(pin);
   KarineUI.Icon(pin, "pin", unlocked ? KarineTheme.Paper.Stamp : KarineTheme.Paper.Faded, 18)
    .style.marginRight = KarineTheme.SpaceSm;
   var label = Text(pin, T(country.nameKey), unlocked ? KarineTheme.Paper.Ink : KarineTheme.Paper.Faded, 14);
   label.style.marginBottom = 0;
  }
 }

 // --- Sağ: seçili ülke kartı ------------------------------------------------

 void WorldDetail(VisualElement parent, ICollection<string> closed) {
  var country = atlas.countries[worldPick];
  bool unlocked = Worlds.CountryUnlocked(atlas, worldPick, closed);
  int done = Worlds.CompletedIn(country, closed);

  var card = KarineUI.Panel(parent, true);
  card.style.width = Length.Percent(26);
  card.style.marginLeft = KarineTheme.SpaceLg;
  card.style.marginTop = KarineTheme.SpaceMd;
  card.style.marginBottom = KarineTheme.SpaceMd;
  card.style.marginRight = 0;

  var art = string.IsNullOrEmpty(country.image) ? null : Resources.Load<Texture2D>(country.image);
  var plate = new VisualElement();
  plate.style.height = 120;
  plate.style.marginBottom = KarineTheme.SpaceMd;
  plate.style.backgroundColor = KarineTheme.GlassDeep;
  KarineUI.Border(plate, KarineTheme.BorderWidth, KarineTheme.Panel2);
  card.Add(plate);
  if (art != null) {
   art.filterMode = FilterMode.Point;
   var image = new Image { image = art, scaleMode = ScaleMode.ScaleAndCrop, pickingMode = PickingMode.Ignore };
   image.style.position = Position.Absolute;
   image.style.left = 0; image.style.right = 0; image.style.top = 0; image.style.bottom = 0;
   plate.Add(image);
  } else {
   plate.style.alignItems = Align.Center;
   plate.style.justifyContent = Justify.Center;
   KarineUI.Icon(plate, "pin", KarineTheme.Muted, 32);
  }

  var head = KarineUI.Row(card);
  var title = KarineUI.Title(head, T(country.nameKey), 24);
  title.style.flexGrow = 1;
  title.style.marginBottom = 0;
  var tally = KarineUI.Technical(head, done + " / " + country.slots.Count + " " + T("world.cases"), 14);
  tally.style.marginBottom = 0;

  KarineUI.Rule(card);
  KarineUI.Technical(card, T(country.cityKey), 13);
  KarineUI.Body_(card, T(country.descriptionKey));

  KarineUI.Technical(card, T("world.progress"), 12);
  var ratio = country.slots.Count == 0 ? 0f : (float)done / country.slots.Count;
  KarineUI.Progress(card, ratio, unlocked ? KarineTone.Active : KarineTone.Neutral);
  KarineUI.Technical(card, "%" + Mathf.RoundToInt(ratio * 100f), 13);

  if (unlocked) {
   Button(card, T("world.open"), () => { worldStripFocus = true; WorldPage(); }, true);
  } else {
   KarineUI.Body_(card, T("world.locked.hint"));
  }
 }

 // --- Alt: dosya şeridi -----------------------------------------------------

 // "Vakaları Görüntüle" ekranı değiştirmez, alt şeridi öne çıkarır: oyuncu
 // dosyaları yine kendisi açar.
 bool worldStripFocus;

 void WorldCaseStrip(ICollection<string> closed) {
  var country = atlas.countries[worldPick];
  bool unlocked = Worlds.CountryUnlocked(atlas, worldPick, closed);

  var strip = new VisualElement();
  strip.style.backgroundColor = KarineTheme.Glass;
  strip.style.borderTopWidth = worldStripFocus ? KarineTheme.PrimaryEdgeWidth : KarineTheme.BorderWidth;
  strip.style.borderTopColor = worldStripFocus ? KarineTheme.Primary : KarineTheme.Accent;
  strip.style.paddingLeft = KarineTheme.SpaceLg;
  strip.style.paddingRight = KarineTheme.SpaceLg;
  strip.style.paddingTop = KarineTheme.SpaceSm;
  strip.style.paddingBottom = KarineTheme.SpaceSm;
  root.Add(strip);

  var scroll = new ScrollView(ScrollViewMode.Horizontal);
  scroll.horizontalScrollerVisibility = ScrollerVisibility.Auto;
  scroll.contentContainer.style.flexDirection = FlexDirection.Row;
  strip.Add(scroll);

  for (int index = 0; index < country.slots.Count; index++) {
   var slot = country.slots[index];
   var state = Worlds.SlotState(country, index, closed, unlocked);
   WorldCaseCard(scroll.contentContainer, slot, index + 1, state);
  }
 }

 void WorldCaseCard(VisualElement parent, WorldSlot slot, int number, WorldSlotState state) {
  bool open = state == WorldSlotState.Active && slot.caseId == game.Data.id;
  bool record = state == WorldSlotState.Completed;
  Action press = open ? (Action)Desk : record ? () => WorldRecord(slot.caseId) : null;

  var card = new Button(KarineUI.Sounded(press));
  card.style.width = 170;
  card.style.minHeight = 96;
  card.style.flexShrink = 0;
  card.style.flexDirection = FlexDirection.Column;
  card.style.alignItems = Align.FlexStart;
  card.style.marginLeft = 0; card.style.marginTop = 0; card.style.marginBottom = 0;
  card.style.marginRight = KarineTheme.SpaceSm;
  card.style.paddingLeft = KarineTheme.SpaceMd; card.style.paddingRight = KarineTheme.SpaceMd;
  card.style.paddingTop = KarineTheme.SpaceSm; card.style.paddingBottom = KarineTheme.SpaceSm;
  card.style.backgroundColor = state == WorldSlotState.Active ? KarineTheme.GlassLift : KarineTheme.GlassDeep;
  KarineUI.Border(card, KarineTheme.BorderWidth,
   state == WorldSlotState.Active ? KarineTheme.Primary : KarineTheme.Panel2);
  card.SetEnabled(press != null);
  parent.Add(card);

  var head = KarineUI.Row(card);
  head.style.marginBottom = KarineTheme.SpaceXs;
  var no = KarineUI.Technical(head, "DOSYA " + number.ToString("000"), 12);
  no.style.flexGrow = 1;
  no.style.marginBottom = 0;
  KarineUI.Dot(head, state == WorldSlotState.Completed ? KarineTone.Active :
                     state == WorldSlotState.Active ? KarineTone.Neutral : KarineTone.Danger);

  var name = KarineUI.Subtitle(card, T(slot.titleKey), 15);
  name.style.marginBottom = KarineTheme.SpaceXs;
  name.style.color = state == WorldSlotState.Locked || state == WorldSlotState.Unwritten
   ? KarineTheme.Disabled : KarineTheme.Primary;

  var foot = KarineUI.Technical(card, WorldStateLabel(state), 12);
  foot.style.marginBottom = 0;
  foot.style.color = state == WorldSlotState.Completed ? KarineUI.Tone(KarineTone.Active) : KarineTheme.Muted;
 }

 string WorldStateLabel(WorldSlotState state) =>
  state == WorldSlotState.Completed ? T("world.state.completed") :
  state == WorldSlotState.Active ? T("world.state.active") :
  state == WorldSlotState.Unwritten ? T("world.state.unwritten") : T("world.state.locked");

 // Kapanmış dosyaya basmak kariyer kaydını açar: oyuncu ne gönderdiğini ve
 // kurumun ne dediğini yeniden okur.
 void WorldRecord(string caseId) {
  var review = game.Career.reviewHistory.LastOrDefault(item => item.caseId == caseId);
  if (review == null) WorldPage(); else CareerRecordPage(review);
 }
}
}
