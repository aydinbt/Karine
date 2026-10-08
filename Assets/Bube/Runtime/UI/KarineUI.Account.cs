using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using C = Bube.KarineTheme.Confirm;

namespace Bube {
// Giriş kağıdı: karartılmış ekranın ortasında krem form; başlık, kısa açıklama,
// her giriş yolu için tam genişlikte düğme (Apple / Google / misafir), altta
// gizlilik bağlantısı. Kapatma düğmesi yok: misafir seçeneği zaten "geç" demektir.
// Geçici görünüş — tasarım maketi gelince yalnız bu dosya değişir.
public static partial class KarineUI {
 public readonly struct AccountOption {
  public readonly string Icon, Label; public readonly Action Pick;
  public AccountOption(string icon, string label, Action pick) { Icon = icon; Label = label; Pick = pick; }
 }

 public static VisualElement AccountPaper(VisualElement parent,string title,string body,IList<AccountOption> options,string notice,string privacyLabel,Action privacy) {
  var veil=new VisualElement {name="AccountPaper"};veil.style.position=Position.Absolute;veil.style.left=veil.style.top=veil.style.right=veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.85f);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;parent?.Add(veil);
  var paper=new VisualElement();paper.style.width=Length.Percent(C.Width);paper.style.maxWidth=C.MaxWidth;
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceXl*2;paper.style.paddingTop=KarineTheme.SpaceXl*2;paper.style.paddingBottom=KarineTheme.SpaceXl;veil.Add(paper);
  var h=Write(paper,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,C.TitleSize,Heading);h.style.unityTextAlign=TextAnchor.MiddleCenter;h.style.marginBottom=KarineTheme.SpaceXs;
  var rule=new VisualElement();rule.style.height=2;rule.style.backgroundColor=KarineTheme.Paper.Ink;rule.style.marginBottom=KarineTheme.SpaceLg;paper.Add(rule);
  var b=Typed(paper,body,C.BodySize);b.style.whiteSpace=WhiteSpace.Normal;b.style.marginBottom=KarineTheme.SpaceLg;
  if(!string.IsNullOrEmpty(notice)){var n=Typed(paper,notice,C.BodySize);n.style.color=KarineTheme.Paper.Stamp;n.style.whiteSpace=WhiteSpace.Normal;n.style.marginBottom=KarineTheme.SpaceMd;}
  for(int i=0;i<options.Count;i++) {
   var o=options[i];bool last=i==options.Count-1;
   var btn=ConfirmButton(paper,o.Label,o.Pick,last?KarineTheme.GlassDeep:KarineTheme.Paper.Stamp,last?KarineTheme.Primary:KarineTheme.Paper.Light);
   btn.name="AccountOption";btn.style.flexBasis=StyleKeyword.Auto;btn.style.flexGrow=0;btn.style.marginBottom=KarineTheme.SpaceMd;
   btn.style.flexDirection=FlexDirection.Row;btn.style.justifyContent=Justify.Center;
   var icon=Icon(btn,o.Icon,last?KarineTheme.Primary:KarineTheme.Paper.Light,KarineTheme.IconSize);icon.style.marginRight=KarineTheme.SpaceMd;icon.pickingMode=PickingMode.Ignore;
  }
  if(privacy!=null){var link=new Button(Sounded(privacy)) {name="PrivacyLink",text=privacyLabel};Unskin(link,Color.clear);link.style.color=KarineTheme.Paper.Ink;
   link.style.unityFontStyleAndWeight=FontStyle.Italic;link.style.alignSelf=Align.Center;link.style.minHeight=KarineTheme.TouchTarget;paper.Add(link);}
  Enter(veil,KarineTheme.ModalMs);KarineMotion.Paper(paper);
  return veil;
 }
}
}
