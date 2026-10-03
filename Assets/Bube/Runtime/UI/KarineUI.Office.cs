using System;
using UnityEngine;
using UnityEngine.UIElements;
using O = Bube.KarineTheme.Office;

namespace Bube {
public static partial class KarineUI {
 public static void IncomingPaper(VisualElement stage) {
  var paper=new VisualElement {name="IncomingPaper",pickingMode=PickingMode.Ignore};
  var box=KarineTheme.Office.Inbox;
  OfficePlace(paper,new Rect(box.x+2,box.y+1,box.width-4,box.height/3));
  paper.style.backgroundColor=KarineTheme.Paper.Sheet;
  Border(paper,KarineTheme.BorderWidth,KarineTheme.Paper.Edge);stage.Add(paper);
  KarineMotion.Run(paper,KarineTheme.Motion.PaperArrivalSeconds,t=>{
   paper.style.translate=new Translate(0,-KarineTheme.Motion.PaperOffset*2*(1-t));
   // Kâğıt hafif yan düşer ve tepsiye oturdukça düzelir.
   paper.style.rotate=new Rotate(Angle.Degrees(-6f*(1-t)-1f));
   paper.style.opacity=t<.8f?1:Mathf.Clamp01((1-t)/.2f);
  },()=>paper.RemoveFromHierarchy());
 }

 public static void OfficePlace(VisualElement element,Rect box) {
  element.style.position=Position.Absolute;
  element.style.left=Length.Percent(box.x);element.style.top=Length.Percent(box.y);
  element.style.width=Length.Percent(box.width);element.style.height=Length.Percent(box.height);
 }
 public static VisualElement OfficeStage(VisualElement parent,string country) {
  var stage=new VisualElement {name="OfficeStage"};parent.Add(stage);
  stage.style.position=Position.Absolute;stage.style.overflow=Overflow.Hidden;
  Action fit=()=> {
   // Güvenli alan dolgusu contentRect'i küçültür; mutlak konum dolgu kutusuna göre olduğundan tam boy kullanılır.
   var size=parent.layout.size;
   // Ekranı kaplar (kenar kırpılır), siyah şerit kalmaz.
   float width=Mathf.Max(size.x,size.y*KarineTheme.Office.Aspect),height=width/KarineTheme.Office.Aspect;
   stage.style.width=width;stage.style.height=height;
   stage.style.left=(size.x-width)*.5f;stage.style.top=(size.y-height)*.5f;
  };
  EventCallback<GeometryChangedEvent> resize=_=>fit();
  parent.RegisterCallback(resize);
  stage.RegisterCallback<DetachFromPanelEvent>(_=>parent.UnregisterCallback(resize));
  fit();
  // Tek parça masa plakası (2 Ekim 2026): eşyalar ve pencere resmin içinde; eşya adları ışık lekeleridir.
  var room=new Image {name="OfficeRoom",image=Resources.Load<Texture2D>("Bube/Art/OfficeDesk"),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  OfficePlace(room,new Rect(0,0,100,100));stage.Add(room);
  var window=new VisualElement {name="OfficeWindow-"+country,pickingMode=PickingMode.Ignore};
  OfficePlace(window,KarineTheme.Office.Window);stage.Add(window);
  OfficeProp(stage,KarineTheme.Office.Lamp,"DeskLamp");
  OfficeProp(stage,KarineTheme.Office.Inbox,"InboxTray");
  OfficeProp(stage,KarineTheme.Office.Phone,"DeskPhone");
  OfficeProp(stage,KarineTheme.Office.Folder,"BlankCaseFolder");
  OfficeProp(stage,KarineTheme.Office.Monitor,"CctvMonitor");
  OfficeProp(stage,KarineTheme.Office.Evidence,"EvidencePile");
  return stage;
 }
 // Eşya resmin içinde; buradaki yalnız onun üstüne düşen yumuşak ışık lekesi. Dokununca
 // aydınlanır, bırakınca söner — geometri oynamaz, masa gerçek kalır.
 static void OfficeProp(VisualElement stage,Rect box,string name) {
  var glow=new Image {name=name,image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Paper.Light};
  OfficePlace(glow,box);glow.style.opacity=0;stage.Add(glow);
 }
 // Eşyanın kendisi düğmedir: yazı ya da kutu yok, yalnız ince amber kontur (3 Ekim maketi).
 // Masadaki nesnenin köşesinde yanıp sönen amber sayı; sıfırsa hiçbir şey eklenmez.
 public static void OfficeCount(VisualElement action,int count) {
  if(action==null || count<=0)return;
  int b=KarineTheme.Office.BadgeSize;
  var badge=new VisualElement {name="DeskCount",pickingMode=PickingMode.Ignore};badge.style.position=Position.Absolute;badge.style.top=-b/3;badge.style.right=-b/3;
  badge.style.width=b;badge.style.height=b;Round(badge,b/2);badge.style.backgroundColor=KarineTheme.Accent;
  badge.style.alignItems=Align.Center;badge.style.justifyContent=Justify.Center;action.Add(badge);
  var l=Write(badge,count.ToString(),KarineTheme.OnPrimary,KarineTheme.Office.BadgeTextSize,Heading);l.style.marginBottom=0;l.style.unityTextAlign=TextAnchor.MiddleCenter;
  badge.schedule.Execute(()=>badge.style.opacity=KarineMotion.Reduced?1f:(badge.style.opacity.value>.6f?.45f:1f)).Every(KarineTheme.Office.BlinkMs);
 }
 public static Button OfficeAction(VisualElement stage,string name,string icon,string label,Rect box,Action action) {
  var button=new Button(Sounded(action)) {name=name,tooltip=label};
  OfficePlace(button,box);button.style.minHeight=KarineTheme.TouchTarget;
  button.style.marginLeft=0;button.style.marginRight=0;button.style.marginTop=0;button.style.marginBottom=0;
  Unskin(button,Color.clear);Round(button,KarineTheme.Radius*3);
  // Kontur yalnız dokunulurken görünür; dururken masa yalnız resimdir.
  Border(button,1,Color.clear);
  var on=KarineTheme.Alpha(KarineTheme.Accent,KarineTheme.Office.OutlineAlpha);
  button.RegisterCallback<PointerEnterEvent>(_=>Border(button,1,on));
  button.RegisterCallback<PointerDownEvent>(_=>Border(button,2,KarineTheme.Accent),TrickleDown.TrickleDown);
  button.RegisterCallback<PointerUpEvent>(_=>Border(button,1,Color.clear));
  button.RegisterCallback<PointerLeaveEvent>(_=>Border(button,1,Color.clear));
  OfficeHotspot(button);
  stage.Add(button);return button;
 }

 // Dokunulabilir eşyanın işareti: ortada küçük amber nokta, çevresinden yayılan halka.
 // Bütün eşyalarda aynı ritimde, sırayla atar; hiçbirini öne çıkarmaz (ipucu değil, yalnız "dokunulur").
 static int hotspotCount;
 static void OfficeHotspot(Button button) {
  float phase=(hotspotCount++%5)*O.HotspotStagger;
  var spot=new VisualElement {name="OfficeHotspot",pickingMode=PickingMode.Ignore};spot.style.position=Position.Absolute;
  spot.style.left=Length.Percent(50);spot.style.top=Length.Percent(50);spot.style.width=0;spot.style.height=0;
  spot.style.alignItems=Align.Center;spot.style.justifyContent=Justify.Center;spot.style.overflow=Overflow.Visible;button.Add(spot);
  var ring=new VisualElement {pickingMode=PickingMode.Ignore};ring.style.position=Position.Absolute;
  Border(ring,2,KarineTheme.Accent);spot.Add(ring);
  var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.position=Position.Absolute;
  dot.style.width=O.HotspotDot;dot.style.height=O.HotspotDot;dot.style.left=-O.HotspotDot/2f;dot.style.top=-O.HotspotDot/2f;Round(dot,(int)O.HotspotDot);
  dot.style.backgroundColor=KarineTheme.Accent;Border(dot,2,KarineTheme.Alpha(KarineTheme.Background,.6f));spot.Add(dot);
  Action<float> ringAt=t=>{float r=Mathf.Lerp(O.HotspotDot*.5f,O.HotspotRing,t);
   ring.style.width=r*2;ring.style.height=r*2;ring.style.left=-r;ring.style.top=-r;Round(ring,(int)(r+1));ring.style.opacity=(1-t)*.9f;};
  ringAt(1);
  spot.schedule.Execute(()=>{
   if(KarineMotion.Reduced){ringAt(1);dot.style.opacity=.9f;return;}
   float t=Mathf.Repeat(Time.unscaledTime+phase,O.HotspotPeriod)/O.HotspotPeriod;
   ringAt(Mathf.Clamp01(t*2.2f));
   dot.style.opacity=.75f+.25f*Mathf.Cos(t*Mathf.PI*2);
  }).Every(40);
  // Basılırken işaret kaybolur; eşyanın kendisi görünsün.
  button.RegisterCallback<PointerDownEvent>(_=>spot.style.display=DisplayStyle.None,TrickleDown.TrickleDown);
  button.RegisterCallback<PointerUpEvent>(_=>spot.style.display=DisplayStyle.Flex);
  button.RegisterCallback<PointerLeaveEvent>(_=>spot.style.display=DisplayStyle.Flex);
 }
 // Klasör etiketi: resimde boş kâğıt şerit, dosya adı daktiloyla üstüne yazılır.
 public static void OfficeFolderLabel(VisualElement stage,string number,string title) {
  var label=new VisualElement {name="OfficeFolderLabel",pickingMode=PickingMode.Ignore};OfficePlace(label,KarineTheme.Office.FolderLabel);
  label.style.alignItems=Align.Center;label.style.justifyContent=Justify.Center;label.style.rotate=new Rotate(KarineTheme.Office.FolderLabelTilt);
  foreach(var line in new[]{number,title}) {
   if(string.IsNullOrEmpty(line))continue;
   var t=Write(label,line.ToUpper(Tr),KarineTheme.Paper.Ink,KarineTheme.Office.FolderLabelSize,null);t.style.marginBottom=0;t.style.letterSpacing=1;
   ApplyFont(t,Typewriter);
  }
  stage.Add(label);
 }
 // Tabletin ekranı: yazı yok. Hafif fosfor parıltısı, tarama çizgileri ve küçük kayıt ışığı;
 // basılınca ekran açılır gibi aydınlanır. Kamera ya da kare göstermez (önizleme ipucu olurdu).
 static Texture2D scanlines;
 public static VisualElement OfficeTabletScreen(VisualElement stage,Button press) {
  var screen=new VisualElement {name="OfficeTabletScreen",pickingMode=PickingMode.Ignore};OfficePlace(screen,KarineTheme.Office.Screen);
  screen.style.rotate=new Rotate(KarineTheme.Office.ScreenTilt);screen.style.overflow=Overflow.Hidden;Round(screen,KarineTheme.Radius*2);
  var glow=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Film.Phosphor};
  glow.style.position=Position.Absolute;glow.style.left=Length.Percent(-20);glow.style.right=Length.Percent(-20);glow.style.top=Length.Percent(-30);glow.style.bottom=Length.Percent(-30);
  screen.Add(glow);
  if(scanlines==null) {
   scanlines=new Texture2D(1,4,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Repeat,filterMode=FilterMode.Point,hideFlags=HideFlags.DontSave};
   scanlines.SetPixels(new[]{new Color(0,0,0,.55f),new Color(0,0,0,0),new Color(0,0,0,0),new Color(0,0,0,0)});scanlines.Apply();
  }
  var lines=new VisualElement {pickingMode=PickingMode.Ignore};lines.style.position=Position.Absolute;lines.style.left=0;lines.style.right=0;lines.style.top=0;lines.style.bottom=0;
  lines.style.backgroundImage=new StyleBackground(scanlines);lines.style.backgroundRepeat=new BackgroundRepeat(Repeat.Repeat,Repeat.Repeat);
  lines.style.backgroundSize=new BackgroundSize(new Length(100,LengthUnit.Percent),new Length(4,LengthUnit.Pixel));screen.Add(lines);
  var dot=new VisualElement {pickingMode=PickingMode.Ignore};dot.style.position=Position.Absolute;dot.style.right=KarineTheme.SpaceMd;dot.style.top=KarineTheme.SpaceMd;
  dot.style.width=7;dot.style.height=7;Round(dot,4);dot.style.backgroundColor=KarineTheme.Danger;screen.Add(dot);
  float rest=KarineTheme.Office.ScreenGlow,lit=KarineTheme.Office.ScreenGlowLit;bool pressed=false;
  glow.style.opacity=rest;
  screen.schedule.Execute(()=>{
   bool still=KarineMotion.Reduced;
   float breath=still?0:Mathf.Sin(Time.unscaledTime*1.3f)*.03f;
   glow.style.opacity=(pressed?lit:rest)+breath;
   dot.style.opacity=still||Mathf.Repeat(Time.unscaledTime,1.6f)<.9f?1f:.15f;
  }).Every(50);
  if(press!=null) {
   press.RegisterCallback<PointerDownEvent>(_=>pressed=true,TrickleDown.TrickleDown);
   press.RegisterCallback<PointerUpEvent>(_=>pressed=false);press.RegisterCallback<PointerLeaveEvent>(_=>pressed=false);
  }
  stage.Add(screen);return screen;
 }
 // Tepsideki bildirim: amber yuvarlak sayı ve yanında koyu kart. Sayıyı uygulama yazar.
 public static VisualElement OfficeNotice(VisualElement stage,string title,string detail,out Label count) {
  var notice=new VisualElement {name="DeskInboxBadge",pickingMode=PickingMode.Ignore};OfficePlace(notice,KarineTheme.Office.Notice);
  notice.style.height=StyleKeyword.Auto;notice.style.flexDirection=FlexDirection.Row;notice.style.alignItems=Align.Center;
  var card=new VisualElement {pickingMode=PickingMode.Ignore};card.style.position=Position.Absolute;card.style.left=KarineTheme.Office.BadgeSize/2;card.style.right=0;
  card.style.paddingLeft=KarineTheme.Office.BadgeSize/2+KarineTheme.SpaceSm;card.style.paddingRight=KarineTheme.SpaceSm;
  card.style.paddingTop=KarineTheme.SpaceXs;card.style.paddingBottom=KarineTheme.SpaceXs;
  card.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.94f);Border(card,KarineTheme.BorderWidth,KarineTheme.Border);Round(card,KarineTheme.Radius);
  var t=Write(card,title.ToUpper(Tr),KarineTheme.Accent,KarineTheme.Office.NoticeTitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceXs;t.style.letterSpacing=1;
  var d=Body_(card,detail,KarineTheme.Office.NoticeSubSize);d.style.color=KarineTheme.Primary;d.style.marginBottom=0;
  d.style.whiteSpace=WhiteSpace.NoWrap;d.style.overflow=Overflow.Hidden;d.style.textOverflow=TextOverflow.Ellipsis;
  notice.Add(card);
  var badge=new VisualElement {pickingMode=PickingMode.Ignore};int b=KarineTheme.Office.BadgeSize;
  badge.style.width=b;badge.style.height=b;Round(badge,b/2);badge.style.backgroundColor=KarineTheme.Accent;
  badge.style.alignItems=Align.Center;badge.style.justifyContent=Justify.Center;notice.Add(badge);
  count=Write(badge,string.Empty,KarineTheme.OnPrimary,KarineTheme.Office.BadgeTextSize,Heading);count.style.marginBottom=0;count.style.unityTextAlign=TextAnchor.MiddleCenter;
  badge.schedule.Execute(()=>badge.style.opacity=KarineMotion.Reduced?1f:(badge.style.opacity.value>.6f?.45f:1f)).Every(KarineTheme.Office.BlinkMs);
  stage.Add(notice);return notice;
 }
 public static void OfficePortrait(VisualElement board,Texture2D art,string person) {
  var card=new VisualElement();card.style.width=KarineTheme.Office.PortraitWidth;
  card.style.marginRight=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceXs;card.style.paddingRight=KarineTheme.SpaceXs;
  card.style.paddingTop=KarineTheme.SpaceXs;card.style.backgroundColor=KarineTheme.Paper.Tint;
  var image=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  image.style.height=KarineTheme.Office.PortraitHeight;card.Add(image);
  var label=Body_(card,person,KarineTheme.Office.SmallSize);
  label.style.color=KarineTheme.Paper.Ink;label.style.marginBottom=KarineTheme.SpaceXs;
  board.Add(card);
 }
 public static void OfficeBrand(VisualElement parent,string title,Action action) {
  var holder=new VisualElement {pickingMode=PickingMode.Ignore};holder.style.width=KarineTheme.Office.BrandWidth;holder.style.flexShrink=0;
  KarineLogo.Hero(holder,KarineTheme.Office.BrandWidth);parent.Add(holder);
  var menu=new Button(Sounded(action)) {name="OfficeMenu",tooltip=title};
  menu.style.width=KarineTheme.Office.MenuWidth;menu.style.paddingLeft=0;menu.style.paddingRight=0;menu.style.height=KarineTheme.Office.TabHeight;menu.style.flexShrink=0;
  menu.style.flexDirection=FlexDirection.Row;menu.style.alignItems=Align.Center;menu.style.justifyContent=Justify.Center;
  menu.style.marginLeft=KarineTheme.SpaceLg;menu.style.marginRight=0;
  Unskin(menu,Color.clear);Border(menu,KarineTheme.BorderWidth,KarineTheme.Border);Round(menu,KarineTheme.Radius);
  Icon(menu,"home",KarineTheme.Primary,KarineTheme.Office.TabIcon+2).style.marginRight=KarineTheme.SpaceMd;
  var l=Write(menu,title.ToUpper(Tr),KarineTheme.Primary,KarineTheme.Office.TabLabelSize+2,Heading);l.style.marginBottom=0;l.style.letterSpacing=1;
  parent.Add(menu);
  var rule=new VisualElement {pickingMode=PickingMode.Ignore};rule.style.width=1;rule.style.alignSelf=Align.Stretch;
  rule.style.marginTop=KarineTheme.SpaceSm;rule.style.marginBottom=KarineTheme.SpaceSm;rule.style.marginLeft=KarineTheme.SpaceLg;
  rule.style.backgroundColor=KarineTheme.Border;parent.Add(rule);
 }
 public static void OfficeTitle(VisualElement parent,string title,string sub) {
  var words=new VisualElement {pickingMode=PickingMode.Ignore};words.style.flexGrow=1;words.style.flexShrink=1;words.style.minWidth=0;
  words.style.marginLeft=KarineTheme.SpaceLg;words.style.justifyContent=Justify.Center;parent.Add(words);
  var t=Write(words,title.ToUpper(Tr),KarineTheme.Primary,KarineTheme.Office.DeskTitleSize,Heading);t.style.marginBottom=-KarineTheme.SpaceSm;t.style.letterSpacing=1;
  t.style.whiteSpace=WhiteSpace.NoWrap;
  if(!string.IsNullOrEmpty(sub)){var s=Body_(words,sub,KarineTheme.Office.SubSize);s.style.color=KarineTheme.Secondary;s.style.marginBottom=0;
   s.style.whiteSpace=WhiteSpace.NoWrap;s.style.overflow=Overflow.Hidden;s.style.textOverflow=TextOverflow.Ellipsis;}
 }
 // Üst çubuk sekmesi: ikon ve etiket yan yana; seçili olan amber çerçeve, amber yazı ve alt çizgi.
 public static Button OfficeHeaderAction(VisualElement parent,string icon,string title,Action action,bool selected=false) {
  var button=new Button(Sounded(action)) {tooltip=title};
  button.style.height=KarineTheme.Office.TabHeight;button.style.flexShrink=0;
  button.style.flexDirection=FlexDirection.Row;button.style.alignItems=Align.Center;button.style.justifyContent=Justify.Center;
  button.style.paddingLeft=KarineTheme.SpaceSm+2;button.style.paddingRight=KarineTheme.SpaceSm+2;
  button.style.marginLeft=KarineTheme.SpaceXs;button.style.marginRight=0;
  var ink=selected?KarineTheme.Accent:KarineTheme.Secondary;
  Unskin(button,KarineTheme.Alpha(selected?KarineTheme.Background:KarineTheme.Panel,.9f));
  Border(button,KarineTheme.BorderWidth,selected?KarineTheme.Accent:KarineTheme.Border);Round(button,KarineTheme.Radius);
  if(selected){button.style.borderBottomWidth=3;}
  Icon(button,icon,selected?KarineTheme.Accent:KarineTheme.Primary,KarineTheme.Office.TabIcon).style.marginRight=KarineTheme.SpaceSm;
  var l=Write(button,title.ToUpper(Tr),ink,KarineTheme.Office.TabLabelSize,Heading);l.style.marginBottom=0;l.style.letterSpacing=1;l.style.whiteSpace=WhiteSpace.NoWrap;
  parent.Add(button);return button;
 }
}
}
