using System;
using UnityEngine;
using UnityEngine.UIElements;
using I = Bube.KarineTheme.Interview;
namespace Bube {
// Sorgu odası (3 Ekim 2026 maketi, Docs/Reference/UI_INTERVIEW_2026-10.png): kimlik kartı, konuşma
// balonu, sekmeli soru paneli ve "kaydı öne sür" paneli. Yalan, çelişki ya da doğru soru işareti yok.
public static partial class KarineUI {
 static VisualElement InterviewGlass(VisualElement parent,string name,Rect box,bool accent) {
  var panel=new VisualElement {name=name};OfficePlace(panel,box);
  panel.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);
  Border(panel,KarineTheme.BorderWidth,accent?KarineTheme.Alpha(KarineTheme.Accent,.7f):KarineTheme.Border);Round(panel,KarineTheme.Radius);
  panel.style.paddingLeft=panel.style.paddingRight=KarineTheme.SpaceLg;panel.style.paddingTop=panel.style.paddingBottom=KarineTheme.SpaceMd;
  parent.Add(panel);return panel;
 }
 public static VisualElement InterviewIdentity(VisualElement parent,Texture2D portrait,string label,string name,string info) {
  var card=InterviewGlass(parent,"InterviewIdentity",I.Identity,false);card.style.height=StyleKeyword.Auto;
  var top=new VisualElement {pickingMode=PickingMode.Ignore};top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.FlexStart;card.Add(top);
  if(portrait!=null) {
   var face=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   face.style.width=I.Thumb;face.style.height=I.Thumb*1.15f;face.style.flexShrink=0;face.style.marginRight=KarineTheme.SpaceMd;
   Border(face,KarineTheme.BorderWidth,KarineTheme.Border);top.Add(face);
  }
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexShrink=1;words.style.flexGrow=1;top.Add(words);
  Write(words,label.ToUpper(Tr),KarineTheme.Primary,I.LabelSize,Heading).style.marginBottom=KarineTheme.SpaceXs;
  Write(words,name.ToUpper(Tr),KarineTheme.Primary,I.NameSize,Heading).style.marginBottom=KarineTheme.SpaceXs;
  // Bilgi satırı kartın tam genişliğinde; fotoğrafın yanındaki dar sütunda kelime kelime kırılıyordu.
  var line=Write(card,info,KarineTheme.Secondary,I.InfoSize,Typewriter);line.style.marginTop=KarineTheme.SpaceMd;line.style.marginBottom=0;
  return card;
 }
 // Konuşma balonu: amber ad, söz, soluk italik gözlem. Sol kenarda kişiye bakan küçük kuyruk.
 public static VisualElement InterviewBubble(VisualElement parent,bool aside,string speaker,out Label speech) {
  var bubble=InterviewGlass(parent,"InterviewBubble",aside?I.BubbleAside:I.Bubble,false);
  bubble.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.97f);
  var tail=new VisualElement {pickingMode=PickingMode.Ignore};tail.style.position=Position.Absolute;
  tail.style.width=tail.style.height=I.Tail;if(aside)tail.style.right=-I.Tail/2-1;else tail.style.left=-I.Tail/2-1;tail.style.top=Length.Percent(40);tail.style.rotate=new Rotate(45);
  tail.style.backgroundColor=bubble.style.backgroundColor;if(aside){tail.style.borderRightWidth=tail.style.borderTopWidth=KarineTheme.BorderWidth;tail.style.borderRightColor=tail.style.borderTopColor=KarineTheme.Border;}
  else{tail.style.borderLeftWidth=tail.style.borderBottomWidth=KarineTheme.BorderWidth;tail.style.borderLeftColor=tail.style.borderBottomColor=KarineTheme.Border;}bubble.Add(tail);
  Write(bubble,speaker.ToUpper(Tr),KarineTheme.Accent,I.SpeakerSize,Heading).style.marginBottom=KarineTheme.SpaceXs;
  var scroll=new KarineScrollView(ScrollViewMode.Vertical);scroll.style.flexGrow=1;scroll.style.minHeight=0;
  scroll.verticalScrollerVisibility=ScrollerVisibility.Hidden;bubble.Add(scroll);
  speech=Write(scroll,"",KarineTheme.Primary,I.SpeechSize,Typewriter);speech.name="InterviewSpeech";
  return scroll.contentContainer;
 }
 public static void InterviewDemeanor(VisualElement bubble,string text) {
  var l=Write(bubble,text,KarineTheme.Alpha(KarineTheme.Secondary,.8f),I.DemeanorSize,Typewriter);
  l.style.unityFontStyleAndWeight=FontStyle.Italic;l.style.marginTop=KarineTheme.SpaceSm;l.style.marginBottom=0;
 }
 public static VisualElement InterviewPanel(VisualElement parent,out VisualElement tabs) {
  var area=new VisualElement {name="InterviewQuestions"};OfficePlace(area,I.Questions);parent.Add(area);
  tabs=new VisualElement();tabs.style.flexDirection=FlexDirection.Row;tabs.style.flexShrink=0;area.Add(tabs);
  var panel=InterviewGlass(area,"InterviewQuestionPanel",new Rect(0,0,100,100),false);
  panel.style.position=Position.Relative;panel.style.width=StyleKeyword.Auto;panel.style.height=StyleKeyword.Auto;
  panel.style.left=panel.style.top=StyleKeyword.Auto;panel.style.flexGrow=1;panel.style.minHeight=0;panel.style.paddingLeft=panel.style.paddingRight=KarineTheme.SpaceSm;
  return panel;
 }
 public static Button InterviewTab(VisualElement tabs,string label,bool active,Action action) {
  var tab=new Button(Sounded(action)) {name="InterviewTab",tooltip=label};tab.style.flexGrow=1;tab.style.flexBasis=0;tab.style.height=I.TabHeight;
  tab.style.marginLeft=tab.style.marginRight=tab.style.marginTop=0;tab.style.marginBottom=-KarineTheme.BorderWidth;
  Unskin(tab,active?KarineTheme.Alpha(KarineTheme.Accent,.14f):KarineTheme.Alpha(KarineTheme.GlassDeep,.9f));
  Border(tab,KarineTheme.BorderWidth,active?KarineTheme.Accent:KarineTheme.Border);Round(tab,KarineTheme.Radius);
  tab.style.borderBottomLeftRadius=tab.style.borderBottomRightRadius=0;
  var l=Write(tab,label.ToUpper(Tr),active?KarineTheme.Accent:KarineTheme.Secondary,I.TabSize,Heading);l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleCenter;
  tabs.Add(tab);return tab;
 }
 // Açılır konu başlığı: sol amber şerit, "KONU · n", sağda ok.
 public static Button InterviewTopic(VisualElement parent,string title,bool open,Action toggle) {
  var head=new Button(Sounded(toggle)) {name="InterviewTopic",tooltip=title};head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.Center;
  head.style.minHeight=I.TopicHeight;head.style.marginLeft=head.style.marginRight=head.style.marginTop=0;head.style.marginBottom=KarineTheme.SpaceSm;
  head.style.paddingLeft=KarineTheme.SpaceMd;head.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(head,KarineTheme.Alpha(KarineTheme.Background,.7f));Round(head,KarineTheme.Radius);
  head.style.borderLeftWidth=I.Edge;head.style.borderLeftColor=open?KarineTheme.Accent:KarineTheme.Border;
  var t=Write(head,title.ToUpper(Tr),open?KarineTheme.Accent:KarineTheme.Secondary,I.TopicSize,Heading);t.style.marginBottom=0;t.style.flexGrow=1;t.style.unityTextAlign=TextAnchor.MiddleLeft;
  var arrow=Icon(head,"nav_next",open?KarineTheme.Accent:KarineTheme.Secondary,KarineTheme.IconSize-6);arrow.style.rotate=new Rotate(open?90:0);
  parent?.Add(head);return head;
 }
 public static Button InterviewQuestion(VisualElement parent,string text,Action ask) {
  var row=new Button(Sounded(ask)) {name="InterviewQuestion",tooltip=text};row.style.minHeight=I.RowHeight;
  row.style.marginLeft=KarineTheme.SpaceSm;row.style.marginRight=row.style.marginTop=0;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceMd;row.style.paddingRight=KarineTheme.SpaceMd;row.style.justifyContent=Justify.Center;
  Unskin(row,KarineTheme.Alpha(KarineTheme.Panel,.55f));row.style.borderLeftWidth=I.Edge-1;row.style.borderLeftColor=KarineTheme.Accent;
  var l=Write(row,text,KarineTheme.Primary,I.RowSize,Typewriter);l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleLeft;
  parent.Add(row);return row;
 }
 // Kaynak süzgeci: tek satırda küçük çerçeveli düğmeler; seçili olan amber.
 public static Button InterviewFilter(VisualElement row,string label,bool active,bool enabled,Action pick) {
  var chip=new Button(Sounded(pick)) {name="InterviewFilter",tooltip=label};chip.style.flexGrow=1;chip.style.flexBasis=0;chip.style.height=I.FilterHeight;
  chip.style.marginLeft=chip.style.marginTop=chip.style.marginBottom=0;chip.style.marginRight=KarineTheme.SpaceXs;chip.style.paddingLeft=chip.style.paddingRight=0;
  Unskin(chip,active?KarineTheme.Alpha(KarineTheme.Accent,.14f):KarineTheme.Alpha(KarineTheme.Background,.6f));
  Border(chip,KarineTheme.BorderWidth,active?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.6f));Round(chip,KarineTheme.Radius);
  var l=Write(chip,label.ToUpper(Tr),active?KarineTheme.Accent:KarineTheme.Secondary,I.FilterSize,Heading);l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleCenter;
  chip.SetEnabled(enabled);row.Add(chip);return chip;
 }
 // Kaydı öne sür paneli: başlık ve soru, solda kaynak listesi, sağda kâğıt önizleme ve düğme.
 public static VisualElement InterviewPresent(VisualElement parent,string heading,string question,Action cancel,string cancelTitle,out VisualElement list,out VisualElement preview) {
  var panel=InterviewGlass(parent,"InterviewPresent",I.Present,true);
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.FlexStart;top.style.flexShrink=0;panel.Add(top);
  var words=new VisualElement();words.style.flexGrow=1;words.style.flexShrink=1;top.Add(words);
  Write(words,heading.ToUpper(Tr),KarineTheme.Accent,I.PresentHead,Heading).style.marginBottom=KarineTheme.SpaceXs;
  Write(words,"“"+question+"”",KarineTheme.Primary,I.PresentQuote,Typewriter).style.marginBottom=KarineTheme.SpaceSm;
  CloseButton(top,cancel,cancelTitle);
  var rule=new VisualElement {pickingMode=PickingMode.Ignore};rule.style.height=1;rule.style.flexShrink=0;rule.style.backgroundColor=KarineTheme.Border;
  rule.style.marginBottom=KarineTheme.SpaceMd;panel.Add(rule);
  var body=new VisualElement();body.style.flexDirection=FlexDirection.Row;body.style.flexGrow=1;body.style.minHeight=0;panel.Add(body);
  var column=new VisualElement();column.style.width=Length.Percent(44);column.style.flexShrink=0;column.style.minHeight=0;body.Add(column);
  list=column;
  preview=new VisualElement();preview.style.flexGrow=1;preview.style.flexShrink=1;preview.style.minHeight=0;preview.style.marginLeft=KarineTheme.SpaceMd;body.Add(preview);
  return panel;
 }
 public static Button InterviewSource(VisualElement list,string icon,string title,string sub,bool selected,Action pick) {
  var row=new Button(Sounded(pick)) {name="InterviewSource",tooltip=title};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=I.SourceRow;row.style.marginLeft=row.style.marginRight=row.style.marginTop=0;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceSm;
  Unskin(row,selected?KarineTheme.Alpha(KarineTheme.Accent,.10f):KarineTheme.Alpha(KarineTheme.Background,.6f));
  Border(row,selected?KarineTheme.BorderWidth+1:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Alpha(KarineTheme.Border,.5f));Round(row,KarineTheme.Radius);
  Icon(row,icon,selected?KarineTheme.Accent:KarineTheme.Secondary,I.SourceIcon).style.marginRight=KarineTheme.SpaceSm;
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;row.Add(words);
  var t=Write(words,title,selected?KarineTheme.Accent:KarineTheme.Primary,I.SourceTitle,Typewriter);t.style.marginBottom=0;t.style.whiteSpace=WhiteSpace.NoWrap;t.style.overflow=Overflow.Hidden;t.style.textOverflow=TextOverflow.Ellipsis;
  if(!string.IsNullOrEmpty(sub)){var s=Write(words,sub,KarineTheme.Secondary,I.SourceSub,Typewriter);s.style.marginBottom=0;s.style.whiteSpace=WhiteSpace.NoWrap;s.style.overflow=Overflow.Hidden;s.style.textOverflow=TextOverflow.Ellipsis;}
  words.Query<Label>().ForEach(l=>l.style.unityTextAlign=TextAnchor.MiddleLeft);
  list.Add(row);return row;
 }
 // Seçilen kaydın krem kâğıdı; içerik `body`ye yazılır.
 public static VisualElement InterviewPaper(VisualElement parent,string title,string stamp,out VisualElement body) {
  var paper=new VisualElement {name="InterviewPaper"};paper.style.flexGrow=1;paper.style.minHeight=0;paper.style.rotate=new Rotate(.6f);
  Stretched(paper,"Bube/UI/paper_sheet");paper.style.backgroundColor=KarineTheme.Paper.Sheet;Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  paper.style.paddingLeft=paper.style.paddingRight=KarineTheme.SpaceLg;paper.style.paddingTop=paper.style.paddingBottom=KarineTheme.SpaceMd;parent.Add(paper);
  var head=new VisualElement();head.style.flexDirection=FlexDirection.Row;head.style.alignItems=Align.FlexStart;head.style.flexShrink=0;paper.Add(head);
  var t=Typed(head,title.ToUpper(Tr),KarineTheme.Dossier.PageBodySize+1,true);t.style.flexGrow=1;t.style.flexShrink=1;t.style.whiteSpace=WhiteSpace.Normal;
  if(!string.IsNullOrEmpty(stamp))Typed(head,stamp,KarineTheme.Dossier.PageBodySize-2).style.marginLeft=KarineTheme.SpaceSm;
  PaperRule(paper,false);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;paper.Add(scroll);
  body=scroll.contentContainer;return paper;
 }
 public static Button InterviewAction(VisualElement parent,string icon,string label,Action action) {
  var button=new Button(Sounded(action)) {name="InterviewAction",tooltip=label};button.style.flexDirection=FlexDirection.Row;
  button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;button.style.height=I.ActionHeight;button.style.flexShrink=0;
  button.style.marginLeft=button.style.marginRight=0;button.style.marginTop=KarineTheme.SpaceMd;button.style.marginBottom=0;
  Unskin(button,KarineTheme.Accent);Round(button,KarineTheme.Radius);
  if(!string.IsNullOrEmpty(icon))Icon(button,icon,KarineTheme.OnPrimary,KarineTheme.IconSize).style.marginRight=KarineTheme.SpaceMd;
  Write(button,label.ToUpper(Tr),KarineTheme.OnPrimary,I.ActionSize,Heading).style.marginBottom=0;
  parent.Add(button);return button;
 }
}
}
