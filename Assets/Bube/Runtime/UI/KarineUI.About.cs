using System;
using UnityEngine;
using UnityEngine.UIElements;
using M = Bube.KarineTheme.SettingsModal;

namespace Bube {
// Hakkında modalı, 3 Ekim 2026 maketi (Docs/Reference/UI_ABOUT_2026-10.png):
// solda oyun kimliği ve yapımcı, sağda emeği geçenler ve bağlantılar.
public static partial class KarineUI {

 public static Label ModalSection(VisualElement parent,string title) {
  var head=Write(parent,title.ToUpper(TextCulture),KarineTheme.Primary,M.TitleSize-14,Heading);head.style.marginBottom=KarineTheme.SpaceXs;head.style.letterSpacing=1;
  var rule=new VisualElement();rule.style.height=1;rule.style.backgroundColor=KarineTheme.Border;rule.style.marginBottom=KarineTheme.SpaceXs;parent.Add(rule);
  return head;
 }


 // Bağlantı düğmesi: amber ikon, başlık + açıklama, sağda ok.
 public static Button AboutLink(VisualElement parent,string icon,string title,string detail,Action click) {
  var button=new Button(Sounded(click)) {name="AboutLink"};
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.minHeight=M.CardHeight;
  button.style.marginLeft=0;button.style.marginRight=0;button.style.marginTop=KarineTheme.SpaceSm;button.style.marginBottom=0;
  button.style.paddingLeft=KarineTheme.SpaceLg;button.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(button,KarineTheme.Alpha(KarineTheme.Background,.6f));Border(button,KarineTheme.BorderWidth,KarineTheme.Border);Round(button,KarineTheme.Radius);
  Icon(button,icon,KarineTheme.Accent,KarineTheme.IconSize+8).style.marginRight=KarineTheme.SpaceLg;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.justifyContent=Justify.Center;button.Add(words);
  var t=Write(words,title.ToUpper(TextCulture),KarineTheme.Primary,M.CardTitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceXs;t.style.letterSpacing=1;Left(t);
  var d=Body_(words,detail,M.RowHintSize);d.style.color=KarineTheme.Secondary;d.style.marginBottom=0;Left(d);
  Icon(button,"nav_next",KarineTheme.Primary,KarineTheme.IconSize);
  parent.Add(button);return button;
 }
}
}
