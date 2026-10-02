using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace Bube {
public static partial class KarineUI {
 // Ayarlar (yeni görünüm, 2 Ekim 2026): simgeli bölüm başlığı, radyo düğmeli seçenek kartları.
 public static string IconOr(string name,string fallback)=>Resources.Load<Texture2D>("Bube/Art/Icons/"+name)!=null?name:fallback;
 public static void SettingsSection(VisualElement parent,string icon,string title,string hint,bool separated) {
  if(separated)Rule(parent);
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;row.style.marginBottom=KarineTheme.SpaceMd;parent.Add(row);
  Icon(row,icon,KarineTheme.Primary,KarineTheme.Settings.SectionIcon).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement();words.style.flexShrink=1;row.Add(words);
  var t=Subtitle(words,title,KarineTheme.Settings.TextSize+2);t.style.color=KarineTheme.Primary;t.style.marginBottom=KarineTheme.SpaceXs;
  if(!string.IsNullOrEmpty(hint)){var h=Body_(words,hint,KarineTheme.CaseBrowser.SmallSize+1);h.style.color=KarineTheme.Secondary;h.style.marginBottom=0;}
 }
 // Seçenek kartı: solda radyo dairesi, ortada başlık + açıklama, sağda isteğe bağlı simge ya da ses çubukları.
 public static Button SettingsOption(VisualElement parent,string title,string detail,bool selected,Action pick,string trailIcon=null,int bars=-1) {
  var button=Button_(parent,"",pick,selected?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.minHeight=KarineTheme.Settings.ChoiceHeight;button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;
  button.style.flexGrow=1;button.style.flexBasis=0;button.style.minWidth=0;button.style.paddingLeft=KarineTheme.SpaceMd;button.style.paddingRight=KarineTheme.SpaceMd;
  var ink=selected?KarineTheme.Paper.Ink:KarineTheme.Primary;var faded=selected?KarineTheme.Paper.Faded:KarineTheme.Secondary;
  int r=KarineTheme.Settings.Radio;
  var ring=new VisualElement {pickingMode=PickingMode.Ignore};ring.style.width=r;ring.style.height=r;ring.style.flexShrink=0;
  ring.style.alignItems=Align.Center;ring.style.justifyContent=Justify.Center;ring.style.marginRight=KarineTheme.SpaceMd;
  Border(ring,2,selected?ink:faded);Round(ring,r);button.Add(ring);
  if(selected){var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.width=r/2;dot.style.height=r/2;dot.style.backgroundColor=ink;Round(dot,r);ring.Add(dot);}
  var copy=new VisualElement();copy.style.flexGrow=1;copy.style.flexShrink=1;copy.style.minWidth=0;button.Add(copy);
  var heading=Body_(copy,title,KarineTheme.Settings.TextSize);heading.style.color=ink;heading.style.marginBottom=detail==null?0:KarineTheme.SpaceXs;
  heading.style.unityFontStyleAndWeight=FontStyle.Bold;heading.style.whiteSpace=WhiteSpace.NoWrap;
  if(!string.IsNullOrEmpty(detail)){var hint=Body_(copy,detail,KarineTheme.CaseBrowser.SmallSize);hint.style.color=faded;hint.style.marginBottom=0;}
  if(trailIcon!=null)Icon(button,trailIcon,faded,KarineTheme.IconSize+KarineTheme.SpaceSm).style.marginLeft=KarineTheme.SpaceSm;
  if(bars>=0) {
   var meter=new VisualElement {pickingMode=PickingMode.Ignore};meter.style.flexDirection=FlexDirection.Row;meter.style.alignItems=Align.FlexEnd;meter.style.marginLeft=KarineTheme.SpaceSm;button.Add(meter);
   for(int i=0;i<4;i++){var b=new VisualElement {pickingMode=PickingMode.Ignore};b.style.width=4;b.style.height=6+i*4;b.style.marginLeft=2;b.style.backgroundColor=i<bars?ink:KarineTheme.Alpha(faded,.35f);meter.Add(b);}
  }
  return button;
 }
 // Alt çubuk eylemi: simge + başlık + açıklama. Kaydet birincil, diğeri ikincil.
 public static Button SettingsAction(VisualElement parent,string icon,string title,string detail,bool primary,Action click) {
  var button=Button_(parent,"",click,primary?KarineButtonKind.Primary:KarineButtonKind.Secondary);
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.minHeight=KarineTheme.Settings.ChoiceHeight-KarineTheme.SpaceSm;
  button.style.minWidth=KarineTheme.Settings.ActionWidth;button.style.paddingLeft=KarineTheme.SpaceLg;button.style.paddingRight=KarineTheme.SpaceLg;
  var ink=primary?KarineTheme.OnPrimary:KarineTheme.Primary;
  Icon(button,icon,ink,KarineTheme.IconSize+KarineTheme.SpaceSm).style.marginRight=KarineTheme.SpaceMd;
  var copy=new VisualElement();copy.style.flexShrink=1;button.Add(copy);
  var t=Body_(copy,title,KarineTheme.Settings.TextSize);t.style.color=ink;t.style.marginBottom=0;t.style.unityFontStyleAndWeight=FontStyle.Bold;
  if(!string.IsNullOrEmpty(detail)){var d=Body_(copy,detail,KarineTheme.CaseBrowser.SmallSize);d.style.color=primary?KarineTheme.Paper.Ink:KarineTheme.Secondary;d.style.marginBottom=0;}
  return button;
 }
}
}
