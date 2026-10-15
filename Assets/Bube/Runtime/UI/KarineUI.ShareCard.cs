using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Paylaşılabilir kapanış kartı (15 Ekim 2026). Spoiler yok: kapak, dosya numarası ve adı,
// "KAPANDI" damgası, değerlendirme ve güven durumu. Fail, yöntem ya da kanıt **yazılmaz**.
// `face` ekran görüntüsü alınan kısımdır; düğmeler onun dışında kalır.
public static partial class KarineUI {
 public static VisualElement ShareCard(VisualElement parent,Texture2D cover,string number,string title,string stamp,
                                       string evaluation,string trust,string footer,string closeLabel,string shareLabel,
                                       Action<VisualElement> share) {
  var veil=new VisualElement {name="ShareCard"};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.top=0;veil.style.right=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.82f);veil.style.flexDirection=FlexDirection.Row;
  veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;
  parent.Add(veil);
  Action close=()=>veil.RemoveFromHierarchy();
  veil.RegisterCallback<PointerDownEvent>(e=>{if(e.target==veil)close();});

  // 4:5 dikey kart — sosyal ağların ortak oranı. Yatay ekranda yüksekliğe göre boyutlanır.
  var face=new VisualElement {name="ShareCardFace",pickingMode=PickingMode.Ignore};
  face.style.height=Length.Percent(86);face.style.aspectRatio=.8f;
  face.style.backgroundColor=KarineTheme.Background;face.style.overflow=Overflow.Hidden;
  Border(face,KarineTheme.BorderWidth,KarineTheme.Accent);veil.Add(face);

  var art=new Image {image=cover,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  art.style.width=Length.Percent(100);art.style.flexGrow=1;face.Add(art);
  var shade=new VisualElement {pickingMode=PickingMode.Ignore};Fill(shade);
  shade.style.backgroundColor=KarineTheme.Veil(.35f);art.Add(shade);
  // Damga kapağın üstünde, hafif yatık.
  var seal=Technical(art,stamp,34);seal.style.position=Position.Absolute;seal.style.right=Length.Percent(8);seal.style.top=Length.Percent(10);
  seal.style.color=KarineTheme.Danger;Border(seal,4,KarineTheme.Danger);Round(seal,6);seal.style.letterSpacing=4;
  seal.style.paddingLeft=14;seal.style.paddingRight=14;seal.style.rotate=new Rotate(-9);

  var info=new VisualElement {pickingMode=PickingMode.Ignore};
  info.style.paddingLeft=KarineTheme.SpaceLg;info.style.paddingRight=KarineTheme.SpaceLg;
  info.style.paddingTop=KarineTheme.SpaceMd;info.style.paddingBottom=KarineTheme.SpaceMd;
  info.style.borderTopWidth=2;info.style.borderTopColor=KarineTheme.Accent;face.Add(info);
  Technical(info,number,15).style.color=KarineTheme.Accent;
  var name=Title(info,title,26);name.style.whiteSpace=WhiteSpace.Normal;name.style.marginBottom=KarineTheme.SpaceSm;
  Body_(info,evaluation,16);
  Body_(info,trust,14).style.color=KarineTheme.Muted;
  var foot=Row(info);foot.style.justifyContent=Justify.SpaceBetween;foot.style.alignItems=Align.Center;foot.style.marginTop=KarineTheme.SpaceSm;
  KarineLogo.Aligned(foot,120);
  Technical(foot,footer,12).style.color=KarineTheme.Muted;

  var actions=new VisualElement();actions.style.marginLeft=KarineTheme.SpaceLg;veil.Add(actions);
  Button_(actions,shareLabel,()=>share(face),KarineButtonKind.Primary).style.marginBottom=KarineTheme.SpaceSm;
  Button_(actions,closeLabel,close,KarineButtonKind.Secondary);
  Enter(veil,KarineTheme.ModalMs);
  return veil;
 }
}
}
