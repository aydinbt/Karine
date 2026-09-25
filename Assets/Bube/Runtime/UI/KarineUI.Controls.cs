using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Seçim ve durum bileşenleri: sekme, radyo, gösterge, sayaç, evrak gezintisi.
// `KarineUI` tek bileşen kitaplığıdır; bu dosya onun bir parçasıdır.
public static partial class KarineUI {
 // --- Sekme ----------------------------------------------------------------

 public static VisualElement Tabs(VisualElement parent, string[] labels, int selected, Action<int> onSelect,
                                 bool stretch = false) {
  var strip = Row(parent);
  strip.style.backgroundColor = KarineTheme.Panel;
  strip.style.marginBottom = KarineTheme.SpaceMd;
  Border(strip, KarineTheme.BorderWidth, KarineTheme.Panel2);
  for (int index = 0; index < labels.Length; index++) {
   int captured = index;
   var tab = new Button(() => onSelect?.Invoke(captured)) { text = labels[index] };
   tab.style.minHeight = KarineTheme.TouchTarget;
   tab.style.fontSize = Typography.Snap(15);
   if (stretch) { tab.style.flexGrow = 1; tab.style.whiteSpace = WhiteSpace.Normal; }
   tab.style.marginLeft = 0; tab.style.marginRight = 0;
   tab.style.marginTop = 0; tab.style.marginBottom = 0;
   Border(tab, 0, Color.clear);
   Round(tab, KarineTheme.Radius);
   bool active = index == selected;
   tab.style.backgroundColor = active ? KarineTheme.Primary : Color.clear;
   tab.style.color = active ? KarineTheme.OnPrimary : KarineTheme.Secondary;
   ApplyFont(tab, Body);
   strip.Add(tab);
  }
  return strip;
 }

 // --- Seçim: radyo ve anahtar ----------------------------------------------

 // Kit'teki SEÇİM RADYO: dolu halka seçili, boş halka değil. Birbirini dışlayan
 // ayarlar için; iki düğmeyi birden birincil yapmak yerine bu kullanılır.
 public static Button Radio(VisualElement parent, string label, bool selected, Action onSelect) {
  var row = new Button(onSelect);
  row.style.flexDirection = FlexDirection.Row;
  row.style.alignItems = Align.Center;
  row.style.minHeight = KarineTheme.TouchTarget;
  row.style.backgroundColor = Color.clear;
  row.style.marginLeft = 0; row.style.marginRight = 0;
  row.style.marginTop = 0; row.style.marginBottom = KarineTheme.SpaceXs;
  row.style.paddingLeft = 0;
  row.style.unityTextAlign = TextAnchor.MiddleLeft;
  Border(row, 0, Color.clear);
  parent?.Add(row);

  var ring = new VisualElement();
  ring.style.width = 20; ring.style.height = 20; ring.style.flexShrink = 0;
  ring.style.alignItems = Align.Center; ring.style.justifyContent = Justify.Center;
  ring.style.marginRight = KarineTheme.SpaceMd;
  Border(ring, KarineTheme.BorderWidth, selected ? KarineTheme.Primary : KarineTheme.Muted);
  Round(ring, 10);
  row.Add(ring);
  if (selected) {
   var core = new VisualElement();
   core.style.width = 10; core.style.height = 10;
   core.style.backgroundColor = KarineTheme.Primary;
   Round(core, 5);
   ring.Add(core);
  }

  var text = new Label(label);
  text.style.color = selected ? KarineTheme.Primary : KarineTheme.Secondary;
  text.style.fontSize = Typography.Snap(17);
  ApplyFont(text, Body);
  row.Add(text);
  return row;
 }

 // --- Durum göstergesi -----------------------------------------------------

 // Kit'in "DURUM GÖSTERGELERİ" kutusu: ikon + etiket + çubuk (Kurum Güveni)
 // ya da ikon + etiket + sayı (Tamamlanan Vaka 3 / 70). Sayı monospace'tir.
 public static VisualElement Meter(VisualElement parent, string icon, string label, float ratio) {
  var card = Panel(parent, true);
  card.style.flexDirection = FlexDirection.Row;
  card.style.alignItems = Align.Center;
  Icon(card, icon, KarineTheme.Primary).style.marginRight = KarineTheme.SpaceMd;
  var column = new VisualElement();
  column.style.flexGrow = 1;
  card.Add(column);
  var name = Technical(column, label, 13);
  name.style.marginBottom = KarineTheme.SpaceXs;
  Progress(column, ratio);
  return card;
 }

 public static VisualElement Counter(VisualElement parent, string icon, string label, string value) {
  var card = Panel(parent, true);
  card.style.flexDirection = FlexDirection.Row;
  card.style.alignItems = Align.Center;
  Icon(card, icon, KarineTheme.Primary).style.marginRight = KarineTheme.SpaceMd;
  var column = new VisualElement();
  column.style.flexGrow = 1;
  card.Add(column);
  Technical(column, label, 13).style.marginBottom = KarineTheme.SpaceXs;
  var number = Technical(column, value, 19);
  number.style.color = KarineTheme.Primary;
  number.style.marginBottom = 0;
  return card;
 }

 // --- Evrak gezintisi ------------------------------------------------------

 // `<  03 / 07  >` — sayfa sayacı monospace, oklar ikon düğme.
 public static VisualElement DocumentNav(VisualElement parent, int index, int count,
                                         Action onPrev, Action onNext) {
  var nav = Row(parent);
  nav.style.justifyContent = Justify.Center;
  var back = IconButton(nav, "nav_prev", onPrev);
  back.SetEnabled(index > 1);
  var counter = Technical(nav, index.ToString("00") + " / " + count.ToString("00"), 21);
  counter.style.marginBottom = 0;
  counter.style.marginLeft = KarineTheme.SpaceLg;
  counter.style.marginRight = KarineTheme.SpaceLg;
  counter.style.color = KarineTheme.Primary;
  var next = IconButton(nav, "nav_next", onNext);
  next.SetEnabled(index < count);
  next.style.marginRight = 0;
  return nav;
 }
}
}
