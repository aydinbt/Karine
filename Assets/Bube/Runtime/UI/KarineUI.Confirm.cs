using System;
using UnityEngine;
using UnityEngine.UIElements;
using C = Bube.KarineTheme.Confirm;
namespace Bube {
// Onay formu (3 Ekim 2026 maketi, Docs/Reference/UI_CONFIRM_2026-10.png): karartılmış ekranın
// ortasında ataçlı krem form; üstte form satırı, büyük başlık, uyarı ikonu ve metin, eğik
// mürekkep damgası; altta koyu "Vazgeç" ve mürekkep kırmızısı onay. Yeniden başlatma ve çıkış.
public static partial class KarineUI {
 public static VisualElement ConfirmPaper(VisualElement parent,string form,string title,string body,string stamp,
                                          string cancelLabel,Action onCancel,string confirmLabel,Action onConfirm) {
  var veil=new VisualElement {name="ConfirmVeil"};veil.style.position=Position.Absolute;veil.style.left=veil.style.top=veil.style.right=veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.8f);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;parent?.Add(veil);
  var paper=new VisualElement {name="ConfirmPaper"};onCancel=KarineMotion.Leave(veil,paper,onCancel);paper.style.width=Length.Percent(C.Width);paper.style.maxWidth=C.MaxWidth;paper.style.rotate=new Rotate(C.Tilt);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceXl*2;paper.style.paddingTop=KarineTheme.SpaceXl*2;paper.style.paddingBottom=KarineTheme.SpaceXl;veil.Add(paper);
  var clip=new VisualElement {pickingMode=PickingMode.Ignore};clip.style.position=Position.Absolute;clip.style.left=Length.Percent(6);clip.style.top=-C.Clip/3;
  clip.style.width=C.Clip/3;clip.style.height=C.Clip;Border(clip,3,KarineTheme.Alpha(KarineTheme.Secondary,.8f));Round(clip,C.Clip/6);paper.Add(clip);
  if(!string.IsNullOrEmpty(form)){var f=Typed(paper,form.ToUpper(Tr),C.FormSize);f.style.unityTextAlign=TextAnchor.MiddleCenter;f.style.letterSpacing=1;}
  var h=Write(paper,title.ToUpper(Tr),KarineTheme.Paper.Ink,C.TitleSize,Heading);h.style.unityTextAlign=TextAnchor.MiddleCenter;h.style.marginBottom=KarineTheme.SpaceXs;
  var rule=new VisualElement();rule.style.height=2;rule.style.backgroundColor=KarineTheme.Paper.Ink;rule.style.marginBottom=KarineTheme.SpaceLg;paper.Add(rule);
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.FlexStart;paper.Add(row);
  Icon(row,"alert",KarineTheme.Paper.Stamp,C.Alert).style.marginRight=KarineTheme.SpaceXl;
  var b=Typed(row,body,C.BodySize);b.style.whiteSpace=WhiteSpace.Normal;b.style.flexShrink=1;
  var stampRow=new VisualElement {pickingMode=PickingMode.Ignore};stampRow.style.alignItems=Align.FlexEnd;stampRow.style.minHeight=C.StampSize*2;paper.Add(stampRow);
  if(!string.IsNullOrEmpty(stamp))InkStamp(stampRow,stamp,KarineTheme.Paper.Stamp,C.StampSize).style.alignSelf=Align.FlexEnd;
  var actions=new VisualElement();actions.style.flexDirection=FlexDirection.Row;paper.Add(actions);
  ConfirmButton(actions,cancelLabel,onCancel,KarineTheme.GlassDeep,KarineTheme.Primary).style.marginRight=KarineTheme.SpaceLg;
  ConfirmButton(actions,confirmLabel,onConfirm,KarineTheme.Paper.Stamp,KarineTheme.Paper.Light).style.flexGrow=2;
  Enter(veil,KarineTheme.ModalMs);KarineMotion.Paper(paper);
  return veil;
 }
 static Button ConfirmButton(VisualElement parent,string label,Action action,Color fill,Color ink) {
  var b=new Button(Sounded(action)) {name="ConfirmButton",text=label.ToUpper(Tr),tooltip=label};b.style.flexGrow=1;b.style.flexBasis=0;b.style.height=C.ButtonHeight;
  b.style.marginLeft=b.style.marginRight=b.style.marginTop=b.style.marginBottom=0;
  Unskin(b,fill);Border(b,KarineTheme.BorderWidth,KarineTheme.Paper.Ink);Round(b,KarineTheme.Radius);
  b.style.color=ink;b.style.fontSize=Typography.Snap(C.ButtonSize);if(Heading!=null)b.style.unityFontDefinition=FontDefinition.FromFont(Heading);
  parent.Add(b);return b;
 }
}
}
