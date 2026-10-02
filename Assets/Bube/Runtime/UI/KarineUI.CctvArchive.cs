using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 // CCTV arşivi (yeni görünüm, 2 Ekim 2026): solda başlık + kamera kartları, sağda başlık, saat sütunlu döküm.
 public static VisualElement CctvArchiveLayout(VisualElement parent,string title,out VisualElement cameras,string hint=null) {
  parent.name="CctvArchiveLayout";parent.style.flexDirection=FlexDirection.Row;parent.style.minHeight=0;
  var side=new VisualElement();side.style.width=Length.Percent(KarineTheme.CctvArchive.SidebarWidth);
  side.style.paddingRight=KarineTheme.SpaceMd;side.style.marginRight=KarineTheme.SpaceMd;
  side.style.borderRightWidth=1;side.style.borderRightColor=KarineTheme.GlassLift;parent.Add(side);
  var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.Center;head.style.marginBottom=KarineTheme.SpaceMd;side.Add(head);
  Icon(head,"binoculars",KarineTheme.Primary,KarineTheme.IconSize+KarineTheme.SpaceSm).style.marginRight=KarineTheme.SpaceSm;
  var words=new VisualElement();words.style.flexShrink=1;head.Add(words);
  Subtitle(words,title.ToUpper(new System.Globalization.CultureInfo("tr-TR")),KarineTheme.CctvArchive.TitleSize).style.marginBottom=0;
  if(!string.IsNullOrEmpty(hint)){var h=Body_(words,hint,KarineTheme.CctvArchive.MetaSize-2);h.style.color=KarineTheme.Secondary;h.style.marginBottom=0;}
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;side.Add(scroll);cameras=scroll;
  var main=new VisualElement {name="CctvArchiveRecords"};main.style.flexGrow=1;main.style.minWidth=0;
  main.style.width=Length.Percent(100-KarineTheme.CctvArchive.SidebarWidth);parent.Add(main);return main;
 }
 // Kamera kartı: simge karosu, ad, konum ve aralık. Kırmızı nokta yalnız "henüz incelenmedi" demektir, her kamerada aynıdır.
 public static Button CctvCameraItem(VisualElement list,string title,string place,string period,bool selected,bool unread,Action click) {
  var button=Button_(list,"",click,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.minHeight=KarineTheme.CctvArchive.CameraRow;
  button.style.marginBottom=KarineTheme.SpaceSm;button.style.paddingLeft=KarineTheme.SpaceSm;button.style.paddingRight=KarineTheme.SpaceSm;
  // Kâğıt dokulu kart yerine düz cam: seçili kart biraz açık zemin ve turkuaz sol kenar.
  Unskin(button,selected?KarineTheme.GlassLift:KarineTheme.GlassDeep);Round(button,KarineTheme.Radius);
  Border(button,1,selected?KarineTheme.Active:KarineTheme.Alpha(KarineTheme.Secondary,.25f));
  button.style.borderLeftWidth=KarineTheme.PrimaryEdgeWidth;
  var ink=KarineTheme.Primary;var faded=KarineTheme.Secondary;
  var tile=new VisualElement {pickingMode=PickingMode.Ignore};tile.style.width=KarineTheme.CctvArchive.CameraTile;tile.style.height=KarineTheme.CctvArchive.CameraTile;
  tile.style.flexShrink=0;tile.style.alignItems=Align.Center;tile.style.justifyContent=Justify.Center;tile.style.marginRight=KarineTheme.SpaceMd;
  tile.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.8f);Border(tile,KarineTheme.BorderWidth,KarineTheme.Alpha(KarineTheme.Secondary,.25f));Round(tile,KarineTheme.Radius);
  button.Add(tile);Icon(tile,Resources.Load<Texture2D>("Bube/Art/Icons/"+KarineTheme.CctvArchive.CameraIcon)!=null?KarineTheme.CctvArchive.CameraIcon:"binoculars",ink,KarineTheme.IconSize+KarineTheme.SpaceSm);
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;button.Add(words);
  var name=Subtitle(words,title,KarineTheme.CctvArchive.TextSize);name.style.color=ink;name.style.marginBottom=0;
  foreach(var line in new[]{place,period}) {
   if(string.IsNullOrEmpty(line))continue;
   var l=Body_(words,line,KarineTheme.CctvArchive.MetaSize-1);l.style.color=faded;l.style.marginBottom=0;
  }
  if(unread){var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=KarineTheme.SpaceMd;dot.style.height=KarineTheme.SpaceMd;dot.style.flexShrink=0;dot.style.alignSelf=Align.FlexStart;dot.style.marginTop=KarineTheme.SpaceSm;dot.style.backgroundColor=KarineTheme.Danger;Round(dot,KarineTheme.SpaceMd);button.Add(dot);}
  return button;
 }
 public static VisualElement CctvRecordPanel(VisualElement parent) {
  var panel=new VisualElement();panel.style.flexGrow=1;panel.style.minHeight=0;
  panel.style.backgroundColor=KarineTheme.GlassDeep;Border(panel,KarineTheme.BorderWidth,KarineTheme.Panel2);Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=KarineTheme.SpaceMd;panel.style.paddingRight=KarineTheme.SpaceSm;panel.style.paddingTop=KarineTheme.SpaceSm;parent.Add(panel);return panel;
 }
 public static VisualElement CctvRecordRow(VisualElement parent) {
  var row=new VisualElement();row.style.display=DisplayStyle.None;row.style.flexDirection=FlexDirection.Row;
  row.style.alignItems=Align.Center;row.style.minHeight=KarineTheme.CctvArchive.RowHeight;
  row.style.paddingLeft=KarineTheme.SpaceXs;row.style.paddingRight=KarineTheme.SpaceSm;parent.Add(row);return row;
 }
 // Döküm satırındaki küçük düğme (izle, netleştir): dokulu düğme yerine düz cam, turkuaz kenar.
 public static void CctvChip(Button button) {
  Unskin(button,KarineTheme.GlassLift);Border(button,1,KarineTheme.Active);Round(button,KarineTheme.Radius);
  button.style.color=KarineTheme.Primary;button.style.marginLeft=KarineTheme.SpaceSm;
 }
 // Döküm satırını saat ve metin olarak ayırır: "08.27 — metin" ya da ayrı saat anahtarı.
 public static void SplitCctvLine(string text,string time,out string stamp,out string body) {
  stamp=time??string.Empty;body=text??string.Empty;
  if(stamp.Length>0){if(body.StartsWith(stamp+" — ",StringComparison.Ordinal))body=body.Substring(stamp.Length+3);return;}
  int dash=body.IndexOf(" — ",StringComparison.Ordinal);
  if(dash>0 && dash<=13){stamp=body.Substring(0,dash);body=body.Substring(dash+3);}
 }
}
}
