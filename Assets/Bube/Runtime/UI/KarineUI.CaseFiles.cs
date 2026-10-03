using System;
using UnityEngine;
using UnityEngine.UIElements;
using F = Bube.KarineTheme.CaseFiles;

namespace Bube {
// Vakalar sayfası, 3 Ekim 2026 maketi (Docs/Reference/UI_CASES_2026-10.png):
// üstte ülke sekmeleri, altta dikey dosya kartları, solda kâğıt kimlik kartı.
// Kart yalnız durum gösterir; fail ipucu, zorluk veya gizli bilgi taşımaz.
public static partial class KarineUI {
 static readonly System.Globalization.CultureInfo Tr=new System.Globalization.CultureInfo("tr-TR");

 public static Button CountryTab(VisualElement parent,string id,string name,string tally,
                                 Texture2D art,bool selected,bool unlocked,Action click) {
  var tab=new Button(Sounded(click)) {name="CountryTile-"+id};
  tab.style.width=F.CountryWidth;tab.style.height=F.CountryHeight;tab.style.flexShrink=0;
  tab.style.flexDirection=FlexDirection.Row;tab.style.alignItems=Align.Stretch;
  tab.style.marginLeft=0;tab.style.marginTop=0;tab.style.marginBottom=0;tab.style.marginRight=KarineTheme.SpaceMd;
  tab.style.paddingLeft=0;tab.style.paddingRight=0;tab.style.paddingTop=0;tab.style.paddingBottom=0;
  tab.style.overflow=Overflow.Hidden;
  Unskin(tab,selected?KarineTheme.Alpha(KarineTheme.Accent,.16f):KarineTheme.Alpha(KarineTheme.Panel,.92f));
  Border(tab,selected?2:KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Border);
  Round(tab,KarineTheme.Radius);
  var photo=new VisualElement {pickingMode=PickingMode.Ignore};
  photo.style.width=Length.Percent(36);photo.style.overflow=Overflow.Hidden;tab.Add(photo);
  CountryPostcard(photo,id,art).style.opacity=unlocked?1f:.3f;
  // Kilit görselin üstünde durur; yazı sütunu ülke adına kalır.
  if(!unlocked) {
   photo.style.alignItems=Align.Center;photo.style.justifyContent=Justify.Center;
   Icon(photo,"lock",KarineTheme.Primary,KarineTheme.IconSize+6);
  }
  var copy=new VisualElement {pickingMode=PickingMode.Ignore};
  copy.style.flexGrow=1;copy.style.flexDirection=FlexDirection.Row;copy.style.alignItems=Align.Center;
  copy.style.paddingLeft=KarineTheme.SpaceLg;tab.Add(copy);
  var words=new VisualElement {pickingMode=PickingMode.Ignore};copy.Add(words);
  words.style.flexGrow=1;words.style.minWidth=0;words.style.paddingRight=KarineTheme.SpaceMd;
  var title=Write(words,name.ToUpper(Tr),unlocked?KarineTheme.Primary:KarineTheme.Secondary,name.Length>12?F.CountryNameSize-12:name.Length>8?F.CountryNameSize-6:F.CountryNameSize,Heading);
  title.style.marginBottom=0;title.style.whiteSpace=WhiteSpace.NoWrap;Left(title);
  title.style.overflow=Overflow.Hidden;title.style.textOverflow=TextOverflow.Ellipsis;
  var count=Technical(words,tally,F.SmallSize);count.style.letterSpacing=2;count.style.marginBottom=0;
  count.style.color=unlocked?KarineTheme.Primary:KarineTheme.Secondary;Left(count);count.style.whiteSpace=WhiteSpace.NoWrap;
  parent.Add(tab);return tab;
 }

 // Düğme içindeki yazılar ortalanır; dosya kâğıdı ise sola yaslı okunur.
 static void Left(Label label){label.style.unityTextAlign=TextAnchor.UpperLeft;label.style.alignSelf=Align.Stretch;}
 // Özet karta sığsın diye kelime sınırında kısaltılır; tam metin dosyanın içindedir.
 static string Clip(string text,int max) {
  if(string.IsNullOrEmpty(text)||text.Length<=max)return text;
  int cut=text.LastIndexOf(' ',max);if(cut<max/2)cut=max;
  return text.Substring(0,cut).TrimEnd(',',';','.',' ')+"…";
 }

 public enum CaseFileState{Open,Ongoing,Closed,Locked}

 public static Button CaseFileCard(VisualElement parent,string id,string number,string title,string summary,
                                   string status,CaseFileState state,Texture2D art,Action click) {
  bool locked=state==CaseFileState.Locked;bool open=state==CaseFileState.Open;
  // Kilitli kartın eylemi yoktur: dokunuş yalnız sarsıntı ve çekmece sesi verir.
  var card=locked?new Button {name="CaseCard-"+id}:new Button(Sounded(click)) {name="CaseCard-"+id};
  card.style.width=F.CardWidth;card.style.height=F.CardHeight;card.style.flexShrink=0;
  card.style.flexDirection=FlexDirection.Column;card.style.alignItems=Align.Stretch;
  card.style.marginLeft=0;card.style.marginTop=0;card.style.marginBottom=0;card.style.marginRight=KarineTheme.SpaceLg;
  int pad=KarineTheme.SpaceSm;
  card.style.paddingLeft=pad;card.style.paddingRight=pad;card.style.paddingTop=pad;card.style.paddingBottom=pad;
  Unskin(card,KarineTheme.Alpha(KarineTheme.Panel,.94f));
  Border(card,open?2:KarineTheme.BorderWidth,open?KarineTheme.Accent:KarineTheme.Border);
  Round(card,KarineTheme.Radius);

  var photo=new VisualElement {pickingMode=PickingMode.Ignore};
  photo.style.height=F.PhotoHeight;photo.style.flexShrink=0;photo.style.overflow=Overflow.Hidden;
  photo.style.backgroundColor=KarineTheme.Panel2;card.Add(photo);
  if(art!=null && !locked) {
   var image=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   image.style.width=Length.Percent(100);image.style.height=Length.Percent(100);photo.Add(image);
  }
  if(locked) {
   photo.style.alignItems=Align.Center;photo.style.justifyContent=Justify.Center;
   var plate=new VisualElement {pickingMode=PickingMode.Ignore};
   plate.style.width=F.LockPlate;plate.style.height=F.LockPlate;plate.style.alignItems=Align.Center;plate.style.justifyContent=Justify.Center;
   plate.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.6f);Round(plate,KarineTheme.Radius);photo.Add(plate);
   Icon(plate,"lock",KarineTheme.Primary,F.LockPlate-24);
  }

  // Kâğıt bölüm: dosya sekmesi fotoğrafın üstüne biner.
  var paper=new VisualElement {pickingMode=PickingMode.Ignore};
  paper.style.flexGrow=1;paper.style.paddingLeft=KarineTheme.SpaceMd;paper.style.paddingRight=KarineTheme.SpaceMd;
  paper.style.paddingBottom=KarineTheme.SpaceSm;paper.style.paddingTop=KarineTheme.SpaceXs;
  paper.style.backgroundColor=locked?KarineTheme.Alpha(KarineTheme.Panel2,.9f):KarineTheme.Paper.Sheet;card.Add(paper);
  var ink=locked?KarineTheme.Secondary:KarineTheme.Paper.Ink;
  var tag=Technical(card,number,F.SmallSize);tag.pickingMode=PickingMode.Ignore;
  tag.style.position=Position.Absolute;tag.style.left=pad;tag.style.top=pad+F.PhotoHeight-F.TagHeight;
  tag.style.height=F.TagHeight;tag.style.paddingLeft=KarineTheme.SpaceMd;tag.style.paddingRight=KarineTheme.SpaceMd;
  tag.style.unityTextAlign=TextAnchor.MiddleLeft;tag.style.marginBottom=0;tag.style.letterSpacing=1;
  tag.style.backgroundColor=locked?KarineTheme.Alpha(KarineTheme.Panel2,.9f):KarineTheme.Paper.Sheet;tag.style.color=ink;
  tag.style.borderTopRightRadius=KarineTheme.Radius;
  var name=Write(paper,locked?"?????":title.ToUpper(Tr),ink,F.CardTitleSize,Heading);
  name.style.marginBottom=KarineTheme.SpaceXs;name.style.whiteSpace=WhiteSpace.NoWrap;
  name.style.overflow=Overflow.Hidden;name.style.textOverflow=TextOverflow.Ellipsis;Left(name);
  var rule=new VisualElement {pickingMode=PickingMode.Ignore};rule.style.height=1;
  rule.style.backgroundColor=locked?KarineTheme.Border:KarineTheme.Paper.Edge;rule.style.marginBottom=KarineTheme.SpaceSm;paper.Add(rule);
  var body=Write(paper,Clip(summary,F.SummaryChars),ink,F.SummarySize,Body);Left(body);
  body.style.flexGrow=1;body.style.flexShrink=1;body.style.minHeight=0;body.style.overflow=Overflow.Hidden;body.style.marginBottom=KarineTheme.SpaceSm;

  // Durum çubuğu: etiket + ok bölmesi. Kilitli dosyada yalnız kilit.
  var bar=new VisualElement {pickingMode=PickingMode.Ignore};
  bar.style.height=F.StatusHeight;bar.style.flexShrink=0;bar.style.flexDirection=FlexDirection.Row;
  var fill=open?KarineTheme.Accent:locked?KarineTheme.Alpha(KarineTheme.Background,.5f):KarineTheme.Background;
  var tone=open?KarineTheme.OnPrimary:KarineTheme.Primary;
  bar.style.backgroundColor=fill;Round(bar,KarineTheme.Radius);
  if(locked)Border(bar,KarineTheme.BorderWidth,KarineTheme.Border);
  paper.Add(bar);
  if(locked) {
   bar.style.justifyContent=Justify.Center;bar.style.alignItems=Align.Center;
   Icon(bar,"lock",KarineTheme.Secondary,KarineTheme.IconSize-4);
  } else {
   var label=Write(bar,status.ToUpper(Tr),tone,F.StatusSize,Heading);
   label.style.flexGrow=1;label.style.unityTextAlign=TextAnchor.MiddleCenter;label.style.marginBottom=0;label.style.letterSpacing=1;
   var arrow=new VisualElement {pickingMode=PickingMode.Ignore};
   arrow.style.width=F.StatusHeight;arrow.style.alignItems=Align.Center;arrow.style.justifyContent=Justify.Center;
   arrow.style.borderLeftWidth=1;arrow.style.borderLeftColor=KarineTheme.Alpha(tone,.35f);bar.Add(arrow);
   Icon(arrow,"nav_next",tone,KarineTheme.IconSize-4);
  }
  // Kilitli kart dokunulunca sarsılır; açık olmayan diğer kartlar dokunuşu almaz.
  if(locked){card.focusable=false;card.AddToClassList("case-locked");LockedShake(card);}
  else if(click==null){card.focusable=false;card.pickingMode=PickingMode.Ignore;}
  else CardHover(card,photo.childCount>0?photo[0]:null);
  parent.Add(card);return card;
 }

 // Sol alttaki kâğıt kimlik kartı: polaroid portre, ad ve oyunda gerçekten
 // bulunan bilgiler. Uydurma yaş/şehir alanı yok.
 public static Button AgentCard(VisualElement parent,Texture2D portrait,string name,string role,
                                (string label,string value)[] rows,Action click) {
  var card=new Button(Sounded(click)) {name="MenuIdentity"};
  card.style.flexDirection=FlexDirection.Row;card.style.alignItems=Align.Center;card.style.flexShrink=0;
  card.style.marginLeft=0;card.style.marginRight=0;card.style.marginTop=0;card.style.marginBottom=0;
  int pad=KarineTheme.SpaceMd;
  card.style.paddingLeft=pad;card.style.paddingRight=pad;card.style.paddingTop=pad;card.style.paddingBottom=pad;
  Unskin(card,KarineTheme.Paper.Sheet);Border(card,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);
  var frame=new VisualElement {pickingMode=PickingMode.Ignore};
  frame.style.width=F.PortraitWidth;frame.style.height=F.PortraitHeight;frame.style.flexShrink=0;
  frame.style.backgroundColor=KarineTheme.Paper.Light;frame.style.paddingLeft=4;frame.style.paddingRight=4;
  frame.style.paddingTop=4;frame.style.paddingBottom=4;frame.style.rotate=new Rotate(Angle.Degrees(KarineTheme.Stagecraft.PolaroidTilt));
  frame.style.marginRight=KarineTheme.SpaceMd;card.Add(frame);Straighten(card,frame);
  if(portrait!=null) {
   var image=new Image {image=portrait,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   image.style.flexGrow=1;frame.Add(image);
  }
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.minWidth=0;card.Add(words);
  var title=Write(words,name,KarineTheme.Paper.Ink,F.AgentNameSize,Heading);title.style.marginBottom=0;Left(title);
  var sub=Technical(words,role.ToUpper(Tr),F.SmallSize);sub.style.color=KarineTheme.Paper.Faded;sub.style.letterSpacing=2;
  sub.style.marginBottom=KarineTheme.SpaceXs;Left(sub);
  var rule=new VisualElement {pickingMode=PickingMode.Ignore};rule.style.height=1;rule.style.backgroundColor=KarineTheme.Paper.Edge;
  rule.style.marginBottom=KarineTheme.SpaceXs;words.Add(rule);
  foreach(var row in rows) {
   var value=Write(words,row.value,KarineTheme.Paper.Ink,F.SmallSize,Body);value.style.marginBottom=0;Left(value);
   value.style.whiteSpace=WhiteSpace.NoWrap;value.style.overflow=Overflow.Hidden;value.style.textOverflow=TextOverflow.Ellipsis;
  }
  parent.Add(card);return card;
 }
}
}
