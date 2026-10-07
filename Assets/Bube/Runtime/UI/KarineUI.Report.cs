using System;
using UnityEngine;
using UnityEngine.UIElements;
using R = Bube.KarineTheme.Report;
namespace Bube {
// Sonuç raporunun parçaları (3 Ekim 2026 maketi, Docs/Reference/UI_REPORT_2026-10.png):
// koyu adım sütunu, form kâğıdı, polaroid şüpheli kartları, koyu kaynak seçici, özet tablo ve onay kartı.
public static partial class KarineUI {
 public static VisualElement ReportRail(VisualElement parent) {
  var rail=new VisualElement {name="ReportRail"};OfficePlace(rail,R.Rail);
  rail.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.96f);Border(rail,KarineTheme.BorderWidth,KarineTheme.Border);Round(rail,KarineTheme.Radius);
  rail.style.paddingLeft=KarineTheme.SpaceSm;rail.style.paddingRight=KarineTheme.SpaceSm;rail.style.paddingTop=KarineTheme.SpaceMd;rail.style.paddingBottom=KarineTheme.SpaceMd;
  parent.Add(rail);return rail;
 }
 // Adım: ikon (bitti ise amber tik), iki haneli numara ve başlık. Etkin adım amber çerçeveli.
 public static Button ReportStep(VisualElement rail,string number,string label,string icon,bool done,bool active,bool enabled,Action action) {
  var step=new Button(Sounded(action)) {name="ReportStep",tooltip=label};step.SetEnabled(enabled);
  step.style.flexDirection=FlexDirection.Row;step.style.alignItems=Align.Center;step.style.flexGrow=1;step.style.flexBasis=0;step.style.maxHeight=96;
  step.style.marginLeft=0;step.style.marginRight=0;step.style.marginBottom=KarineTheme.SpaceSm;
  step.style.paddingLeft=KarineTheme.SpaceSm;step.style.paddingRight=KarineTheme.SpaceXs;
  Unskin(step,active?KarineTheme.Alpha(KarineTheme.Accent,.12f):KarineTheme.Alpha(KarineTheme.Background,.7f));
  Border(step,active?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,active?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(step,KarineTheme.Radius);
  var tone=active||done?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Secondary,.7f);
  Icon(step,done&&!active?"check":icon,tone,R.StepIcon).style.marginRight=KarineTheme.SpaceSm;
  var text=new VisualElement {pickingMode=PickingMode.Ignore};text.style.flexShrink=1;step.Add(text);
  var n=Write(text,number,tone,R.StepNumberSize,Heading);n.style.marginBottom=0;
  var l=Write(text,label.ToUpper(TextCulture),active?KarineTheme.Primary:KarineTheme.Secondary,R.StepLabelSize,Heading);l.style.marginBottom=0;
  rail.Add(step);return step;
 }
 public static VisualElement ReportPaper(VisualElement parent) {
  var paper=new VisualElement {name="ReportPaper"};OfficePlace(paper,R.Paper);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(paper,KarineTheme.Radius);
  paper.style.paddingLeft=KarineTheme.SpaceXl+KarineTheme.SpaceMd;paper.style.paddingRight=KarineTheme.SpaceXl+KarineTheme.SpaceMd;
  paper.style.paddingTop=KarineTheme.SpaceLg;paper.style.paddingBottom=KarineTheme.SpaceLg;
  parent.Add(paper);return paper;
 }
 // "SONUÇ RAPORU        DOSYA #001   01 / 04", çizgi, daktilo yönerge.
 public static void ReportHead(VisualElement paper,string title,string file,string counter,string help) {
  var line=new VisualElement();line.style.flexDirection=FlexDirection.Row;line.style.alignItems=Align.FlexEnd;line.style.flexShrink=0;paper.Add(line);
  var t=Typed(line,title.ToUpper(TextCulture),R.TitleSize,true);t.style.flexGrow=1;
  Typed(line,file.ToUpper(TextCulture),R.MetaSize).style.marginRight=KarineTheme.SpaceXl;
  Typed(line,counter,R.MetaSize,true);
  PaperRule(paper,false);
  Typed(paper,help,R.HelpSize).style.marginBottom=KarineTheme.SpaceMd;
 }
 public static void ReportHeading(VisualElement parent,string text) {
  Typed(parent,text.ToUpper(TextCulture),R.HeadingSize,true);PaperRule(parent,false);
 }
 // Şüpheli kartı: polaroid, altında daktilo ad; seçili olan amber çerçeve ve köşede tik.
 public static Button ReportPortrait(VisualElement row,Texture2D portrait,string label,bool selected,int index,Action action) {
  var card=new Button(Sounded(action)) {name="ReportPortrait",tooltip=label};card.style.width=R.CardWidth;card.style.flexShrink=0;
  card.style.marginLeft=KarineTheme.SpaceLg;card.style.marginRight=KarineTheme.SpaceLg;card.style.marginBottom=KarineTheme.SpaceMd;
  card.style.paddingLeft=KarineTheme.SpaceSm;card.style.paddingRight=KarineTheme.SpaceSm;card.style.paddingTop=KarineTheme.SpaceSm;card.style.paddingBottom=KarineTheme.SpaceSm;
  Unskin(card,KarineTheme.Paper.Light);Border(card,selected?KarineTheme.BorderWidth+2:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Paper.Edge);
  card.style.rotate=new Rotate(selected?0:(index%2==0?-R.CardTilt:R.CardTilt));
  if(portrait!=null){var photo=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};photo.style.height=R.PhotoHeight;card.Add(photo);}
  else {var blank=new VisualElement {pickingMode=PickingMode.Ignore};blank.style.height=R.PhotoHeight;blank.style.alignItems=Align.Center;blank.style.justifyContent=Justify.Center;
   blank.style.backgroundColor=KarineTheme.Paper.Tint;card.Add(blank);Icon(blank,"person",KarineTheme.Paper.Faded,R.PhotoHeight/2);}
  var l=Typed(card,label.ToUpper(TextCulture),R.CardLabelSize,true);l.style.unityTextAlign=TextAnchor.MiddleCenter;l.style.marginTop=KarineTheme.SpaceSm;l.style.whiteSpace=WhiteSpace.Normal;
  if(selected)Tick(card);
  row.Add(card);return card;
 }
 static void Tick(VisualElement card) {
  var badge=new VisualElement {name="ReportTick",pickingMode=PickingMode.Ignore};badge.style.position=Position.Absolute;badge.style.right=KarineTheme.SpaceXs;badge.style.top=KarineTheme.SpaceXs;
  badge.style.width=R.StepIcon;badge.style.height=R.StepIcon;badge.style.alignItems=Align.Center;badge.style.justifyContent=Justify.Center;
  badge.style.backgroundColor=KarineTheme.Accent;Round(badge,KarineTheme.Radius);card.Add(badge);
  Icon(badge,"check",KarineTheme.OnPrimary,R.StepIcon-8);
 }
 // Yöntem, kanıt ve benzeri sütunlar: açık kâğıt kart, seçili olan amber çerçeve ve tikli.
 public static Button ReportChoiceCard(VisualElement wrap,string label,bool selected,Action action) {
  var card=new Button(Sounded(action)) {name="ReportChoice",tooltip=label};card.style.minWidth=R.ChoiceMinWidth;card.style.flexGrow=1;card.style.flexBasis=R.ChoiceMinWidth;
  card.style.minHeight=KarineTheme.TouchTarget+KarineTheme.SpaceMd;card.style.flexDirection=FlexDirection.Row;card.style.alignItems=Align.Center;
  card.style.marginLeft=0;card.style.marginRight=KarineTheme.SpaceMd;card.style.marginBottom=KarineTheme.SpaceMd;
  card.style.paddingLeft=KarineTheme.SpaceMd;card.style.paddingRight=R.StepIcon+KarineTheme.SpaceMd;
  Unskin(card,KarineTheme.Paper.Light);Border(card,selected?KarineTheme.BorderWidth+2:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Paper.Edge);Round(card,KarineTheme.Radius);
  var l=Typed(card,label,R.ChoiceSize,selected);l.style.whiteSpace=WhiteSpace.Normal;l.style.flexShrink=1;l.style.unityTextAlign=TextAnchor.MiddleLeft;
  if(selected)Tick(card);
  wrap.Add(card);return card;
 }
 // "DAYANAK KAYNAK" başlığı altındaki koyu seçici: belge ikonu, seçilen kaynak ya da yönerge, aşağı ok.
 public static Button ReportSourceBar(VisualElement parent,string heading,string label,Action open) {
  Typed(parent,heading.ToUpper(TextCulture),R.HeadingSize-4,true).style.marginBottom=KarineTheme.SpaceXs;
  var bar=new Button(Sounded(open)) {name="ReportSourcePicker",tooltip=label};bar.style.height=R.PickerHeight;bar.style.flexShrink=0;
  bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;bar.style.marginLeft=0;bar.style.marginRight=0;bar.style.marginBottom=KarineTheme.SpaceSm;
  bar.style.paddingLeft=KarineTheme.SpaceMd;bar.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(bar,KarineTheme.Alpha(KarineTheme.GlassDeep,.97f));Border(bar,KarineTheme.BorderWidth,KarineTheme.Border);Round(bar,KarineTheme.Radius);
  Icon(bar,"document",KarineTheme.Primary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(bar,label,KarineTheme.Primary,R.QuoteSize,Typewriter);l.style.marginBottom=0;l.style.flexGrow=1;l.style.flexShrink=1;
  l.style.unityTextAlign=TextAnchor.MiddleLeft;l.style.whiteSpace=WhiteSpace.NoWrap;l.style.overflow=Overflow.Hidden;l.style.textOverflow=TextOverflow.Ellipsis;
  Icon(bar,"nav_next",KarineTheme.Primary,KarineTheme.IconSize).style.rotate=new Rotate(90);
  parent.Add(bar);return bar;
 }
 // Seçilen kaynağın alıntısı ve "KAYNAĞI AÇ"; oyuncunun karşılaştırmada kendi yazdığı notlar altta.
 public static VisualElement ReportQuote(VisualElement parent,string quote,string[] notes,string openLabel,Action open) {
  var box=new VisualElement {name="ReportQuote"};box.style.flexShrink=0;box.style.marginBottom=KarineTheme.SpaceMd;
  box.style.paddingLeft=KarineTheme.SpaceMd;box.style.paddingRight=KarineTheme.SpaceMd;box.style.paddingTop=KarineTheme.SpaceSm;box.style.paddingBottom=KarineTheme.SpaceSm;
  box.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Paper.Light,.7f);Border(box,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);Round(box,KarineTheme.Radius);parent.Add(box);
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;box.Add(row);
  var q=Typed(row,quote,R.QuoteSize);q.style.flexGrow=1;q.style.flexShrink=1;q.style.whiteSpace=WhiteSpace.Normal;
  var link=new Button(Sounded(open)) {name="ReportOpenSource",tooltip=openLabel};link.style.flexDirection=FlexDirection.Row;link.style.alignItems=Align.Center;link.style.flexShrink=0;
  link.style.minHeight=KarineTheme.TouchTarget;link.style.marginLeft=KarineTheme.SpaceMd;link.style.marginRight=0;Unskin(link,Color.clear);
  Typed(link,openLabel.ToUpper(TextCulture),R.NoteSize,true);Icon(link,"nav_next",KarineTheme.Paper.Ink,R.NoteSize+4).style.marginLeft=KarineTheme.SpaceXs;row.Add(link);
  foreach(var note in notes ?? new string[0])Typed(box,note,R.NoteSize,true,KarineTheme.Paper.Stamp).style.whiteSpace=WhiteSpace.Normal;
  return box;
 }
 public static VisualElement ReportNav(VisualElement paper) {
  var nav=new VisualElement {name="ReportNav"};nav.style.flexDirection=FlexDirection.Row;nav.style.alignItems=Align.Center;nav.style.flexShrink=0;
  nav.style.marginTop=KarineTheme.SpaceMd;paper.Add(nav);return nav;
 }
 // ÖNCEKİ koyu; DEVAM ET / RAPORU GÖNDER amber, hazır değilse sönük.
 public static Button ReportNavButton(VisualElement nav,string label,bool primary,bool enabled,Action action,string icon=null,bool iconAfter=false) {
  var button=new Button(Sounded(action)) {name=primary?"ReportNext":"ReportPrevious",tooltip=label};button.SetEnabled(enabled);
  button.style.height=R.NavHeight;button.style.minWidth=R.NavWidth;button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  button.style.marginLeft=0;button.style.marginRight=KarineTheme.SpaceMd;button.style.paddingLeft=KarineTheme.SpaceLg;button.style.paddingRight=KarineTheme.SpaceLg;
  var fill=primary?(enabled?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Secondary,.45f)):KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);
  var ink=primary?KarineTheme.OnPrimary:KarineTheme.Primary;
  Unskin(button,fill);Border(button,KarineTheme.BorderWidth,primary?fill:KarineTheme.Border);Round(button,KarineTheme.Radius);
  if(icon!=null && !iconAfter)Icon(button,icon,ink,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(button,label.ToUpper(TextCulture),ink,R.HeadingSize-2,Heading);l.style.marginBottom=0;
  if(icon!=null && iconAfter)Icon(button,icon,ink,KarineTheme.IconSize).style.marginLeft=KarineTheme.SpaceMd;
  nav.Add(button);return button;
 }
 // Özet satırı: ikon, "ŞÜPHELİ : seçim" ve dikey çizginin sağında "DAYANAK : kaynak". Dokununca kaynak açılır.
 public static Button ReportSummaryRow(VisualElement parent,string icon,string key,string value,string sourceKey,string source,Action open) {
  var row=new Button(Sounded(open)) {name="ReportSummaryRow",tooltip=source??value};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=KarineTheme.TouchTarget+KarineTheme.SpaceSm;row.style.marginLeft=0;row.style.marginRight=0;row.style.paddingLeft=0;row.style.paddingRight=0;
  Unskin(row,Color.clear);row.style.borderBottomWidth=1;row.style.borderBottomColor=KarineTheme.Paper.Edge;parent.Add(row);
  Icon(row,icon,KarineTheme.Paper.Ink,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  var left=new VisualElement {pickingMode=PickingMode.Ignore};left.style.flexDirection=FlexDirection.Row;left.style.flexGrow=1;left.style.flexBasis=0;left.style.paddingRight=KarineTheme.SpaceMd;row.Add(left);
  var k=Typed(left,key.ToUpper(TextCulture),R.RowSize,true);k.style.width=R.RowKeyWidth;k.style.flexShrink=0;
  Typed(left,":  ",R.RowSize);Typed(left,value,R.RowSize).style.flexShrink=1;left.ElementAt(2).style.whiteSpace=WhiteSpace.Normal;
  if(sourceKey==null)return row;
  var right=new VisualElement {pickingMode=PickingMode.Ignore};right.style.flexDirection=FlexDirection.Row;right.style.flexGrow=1;right.style.flexBasis=0;
  right.style.borderLeftWidth=1;right.style.borderLeftColor=KarineTheme.Paper.Edge;right.style.paddingLeft=KarineTheme.SpaceMd;row.Add(right);
  var s=Typed(right,sourceKey.ToUpper(TextCulture),R.RowSize,true);s.style.width=R.RowSourceKey;s.style.flexShrink=0;
  Typed(right,":  ",R.RowSize);var v=Typed(right,source,R.RowSize);v.style.flexShrink=1;v.style.whiteSpace=WhiteSpace.Normal;
  return row;
 }
 // Gönderme onayı: eğik kâğıt kart, ataş, kırmızı ünlem, VAZGEÇ koyu / GÖNDER amber.
 public static VisualElement ReportConfirm(VisualElement parent,string title,string body,string cancel,Action onCancel,string send,Action onSend) {
  var veil=new VisualElement {name="ReportConfirm"};veil.style.position=Position.Absolute;veil.style.left=0;veil.style.right=0;veil.style.top=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.72f);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;parent.Add(veil);
  var card=new VisualElement();card.style.width=R.ConfirmWidth;card.style.alignItems=Align.Center;card.style.rotate=new Rotate(R.ConfirmTilt);
  Stretched(card,"Bube/UI/paper_sheet");card.style.backgroundColor=KarineTheme.Paper.Sheet;Border(card,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  card.style.paddingLeft=KarineTheme.SpaceXl;card.style.paddingRight=KarineTheme.SpaceXl;card.style.paddingTop=KarineTheme.SpaceXl;card.style.paddingBottom=KarineTheme.SpaceXl;veil.Add(card);
  var mark=new VisualElement {pickingMode=PickingMode.Ignore};mark.style.width=R.ConfirmGlyph;mark.style.height=R.ConfirmGlyph;mark.style.alignItems=Align.Center;mark.style.justifyContent=Justify.Center;
  Border(mark,3,KarineTheme.Danger);Round(mark,R.ConfirmGlyph/2);mark.style.marginBottom=KarineTheme.SpaceMd;card.Add(mark);
  Write(mark,"!",KarineTheme.Danger,R.ConfirmGlyphSize,Heading).style.marginBottom=0;
  var t=Write(card,title.ToUpper(TextCulture),KarineTheme.Paper.Ink,R.ConfirmTitleSize,Heading);t.style.unityTextAlign=TextAnchor.MiddleCenter;t.style.marginBottom=KarineTheme.SpaceSm;
  var b=Typed(card,body,R.HelpSize);b.style.unityTextAlign=TextAnchor.MiddleCenter;b.style.whiteSpace=WhiteSpace.Normal;b.style.marginBottom=KarineTheme.SpaceXl;
  var actions=new VisualElement();actions.style.flexDirection=FlexDirection.Row;card.Add(actions);
  ReportNavButton(actions,cancel,false,true,onCancel);
  ReportNavButton(actions,send,true,true,onSend).style.marginRight=0;
  Enter(veil,KarineTheme.ModalMs);
  return veil;
 }
}
}
