using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
public static partial class KarineUI {
 // A single reusable atlas; UVs select a postcard without creating ten textures.
 public static Image CountryPostcard(VisualElement parent,string id,Texture2D custom=null) {
  string[] ids={"tr","uk","de","jp","fr","us","it","es","ca","au"};
  int index=Array.IndexOf(ids,id);
  var art=custom??Resources.Load<Texture2D>("Bube/Art/CountryPostcards");
  var image=new Image {image=art,scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  if(custom==null && index>=0) image.uv=new Rect((index%5)/5f,index<5?.5f:0f,.2f,.5f);
  image.style.position=Position.Absolute;
  image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  parent.Add(image);
  return image;
 }

 public static Button CountryTile(VisualElement parent,string id,string title,string count,
                                  Texture2D art,bool selected,bool unlocked,Action click,float progress=0) {
  var card=new Button(Sounded(click)) {name="CountryTile-"+id};
  card.style.width=KarineTheme.CaseBrowser.CountryWidth;
  card.style.height=KarineTheme.CaseBrowser.CountryHeight;
  card.style.flexShrink=0;
  card.style.marginLeft=0;card.style.marginTop=0;
  card.style.marginRight=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceSm;card.style.paddingRight=KarineTheme.SpaceSm;
  card.style.justifyContent=Justify.FlexEnd;card.style.backgroundColor=KarineTheme.Background;
  card.style.overflow=Overflow.Hidden;
  Border(card,selected?2:KarineTheme.BorderWidth,selected?KarineTheme.Primary:KarineTheme.Accent);
  CountryPostcard(card,id,art).style.opacity=unlocked?1f:.45f;
  if(id=="tr") {
   var flag=new Image {image=Resources.Load<Texture2D>("Bube/TurkiyeFlag"),scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   flag.style.position=Position.Absolute;flag.style.left=KarineTheme.SpaceSm;flag.style.top=KarineTheme.SpaceSm;
   flag.style.width=KarineTheme.IconSize+KarineTheme.SpaceMd;flag.style.height=KarineTheme.IconSize;card.Add(flag);
  }
  var plate=new VisualElement();
  plate.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.95f);
  plate.style.paddingLeft=KarineTheme.SpaceXs;plate.style.paddingRight=KarineTheme.SpaceXs;
  card.Add(plate);
  var name=Body_(plate,title,KarineTheme.CaseBrowser.TextSize);
  name.style.color=KarineTheme.Primary;
  name.style.marginBottom=0;
  var tally=Body_(plate,count,KarineTheme.CaseBrowser.SmallSize);
  tally.style.color=KarineTheme.Secondary;
  tally.style.marginBottom=KarineTheme.SpaceXs;
  BrowserMeter(plate,progress);
  if(!unlocked) {var seal=new CaseSeal(false);seal.style.position=Position.Absolute;seal.style.top=Length.Percent(30);seal.style.left=Length.Percent(50);seal.style.translate=new Translate(Length.Percent(-50),0);card.Add(seal);}
  parent.Add(card);
  return card;
 }

 public static void BrowserMeter(VisualElement parent,float value) {
  var track=new VisualElement();track.style.height=KarineTheme.CaseBrowser.ProgressHeight;track.style.marginBottom=KarineTheme.SpaceXs;
  track.style.backgroundColor=KarineTheme.Panel2;parent.Add(track);
  var fill=new VisualElement();fill.style.width=Length.Percent(Mathf.Clamp01(value)*100);fill.style.height=Length.Percent(100);fill.style.backgroundColor=KarineTheme.Primary;track.Add(fill);
 }
 public static void BrowserProgress(VisualElement parent,string title,string summary,float progress) {
  var box=new VisualElement();box.style.width=KarineTheme.CaseBrowser.ProgressWidth;box.style.flexShrink=0;
  box.style.paddingLeft=KarineTheme.SpaceMd;box.style.paddingRight=KarineTheme.SpaceMd;box.style.paddingTop=KarineTheme.SpaceXs;
  box.style.backgroundColor=KarineTheme.GlassDeep;Border(box,KarineTheme.BorderWidth,KarineTheme.Panel2);parent.Add(box);
  box.style.paddingBottom=KarineTheme.SpaceXs;Round(box,KarineTheme.Radius);
  var top=new VisualElement();top.style.flexDirection=FlexDirection.Row;top.style.alignItems=Align.Center;box.Add(top);
  Icon(top,IconOr(KarineTheme.CaseBrowser.MapIcon,"pin"),KarineTheme.Primary,KarineTheme.IconSize+KarineTheme.SpaceSm).style.marginRight=KarineTheme.SpaceSm;
  var words=new VisualElement();words.style.flexGrow=1;top.Add(words);
  var heading=Technical(words,title,KarineTheme.CaseBrowser.SmallSize);heading.style.marginBottom=0;
  var detail=Body_(words,summary,KarineTheme.CaseBrowser.TextSize);detail.style.color=KarineTheme.Primary;detail.style.marginBottom=0;
  var meter=new VisualElement();meter.style.flexDirection=FlexDirection.Row;meter.style.alignItems=Align.Center;box.Add(meter);
  var track=new VisualElement();track.style.flexGrow=1;meter.Add(track);BrowserMeter(track,progress);
  var pct=Technical(meter,Mathf.RoundToInt(Mathf.Clamp01(progress)*100)+"%",KarineTheme.CaseBrowser.SmallSize);pct.style.marginLeft=KarineTheme.SpaceSm;pct.style.marginBottom=0;
 }
 public static VisualElement BrowserBanner(VisualElement parent,string country,Texture2D art,string title,string description,string stamp=null) {
  var banner=new VisualElement();banner.style.height=KarineTheme.CaseBrowser.BannerHeight;banner.style.flexShrink=0;banner.style.overflow=Overflow.Hidden;parent.Add(banner);
  CountryPostcard(banner,country,art);
  var shade=new VisualElement();OfficePlace(shade,new Rect(0,0,100,100));shade.style.backgroundColor=KarineTheme.Veil(.5f);banner.Add(shade);
  var copy=new VisualElement();copy.style.paddingLeft=KarineTheme.SpaceMd;copy.style.paddingTop=KarineTheme.SpaceSm;copy.style.paddingRight=KarineTheme.CaseBrowser.StampWidth+KarineTheme.SpaceLg;banner.Add(copy);
  var line=new VisualElement();line.style.flexDirection=FlexDirection.Row;line.style.alignItems=Align.Center;copy.Add(line);
  if(country=="tr") {var flag=new Image {image=Resources.Load<Texture2D>("Bube/TurkiyeFlag"),scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   flag.style.width=KarineTheme.IconSize*2;flag.style.height=KarineTheme.IconSize+KarineTheme.SpaceSm;flag.style.marginRight=KarineTheme.SpaceSm;line.Add(flag);}
  var heading=Subtitle(line,title,KarineTheme.CaseBrowser.TitleSize-KarineTheme.SpaceSm);heading.style.marginBottom=KarineTheme.SpaceXs;
  var desc=Body_(copy,description,KarineTheme.CaseBrowser.SmallSize);desc.style.marginBottom=0;
  return banner;
 }
 public static void BrowserSteps(VisualElement parent,bool[] completed,int active) {
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.marginTop=KarineTheme.SpaceSm;parent.Add(row);
  for(int i=0;i<completed.Length;i++) {
   var step=new VisualElement();step.style.flexGrow=1;step.style.alignItems=Align.Center;step.style.borderTopWidth=KarineTheme.BorderWidth;step.style.marginTop=KarineTheme.CaseBrowser.StepDot/2;step.style.borderTopColor=KarineTheme.Accent;row.Add(step);
   bool open=completed[i]||active==i;
   var dot=new VisualElement {pickingMode=PickingMode.Ignore};int d=KarineTheme.CaseBrowser.StepDot;
   dot.style.width=d;dot.style.height=d;dot.style.marginTop=-d/2;Round(dot,d/2);dot.style.alignItems=Align.Center;dot.style.justifyContent=Justify.Center;
   dot.style.backgroundColor=open?KarineTheme.Primary:KarineTheme.GlassDeep;Border(dot,KarineTheme.BorderWidth,open?KarineTheme.Primary:KarineTheme.Accent);step.Add(dot);
   if(!open&&IconOr("lock",null)!=null) Icon(dot,"lock",KarineTheme.Muted,d-8);
   var label=Technical(step,(i+1).ToString("00"),KarineTheme.CaseBrowser.SmallSize);label.style.marginBottom=0;
   label.style.color=open?KarineTheme.Primary:KarineTheme.Muted;
  }
 }

 // Polaroid, tape, pin and status are individual UI elements, not a screen image.
 public static Button CasePhotoCard(VisualElement parent,string id,string country,string number,
  string title,string state,Texture2D art,bool active,bool completed,Action click,string index="") {
  var card=new Button(Sounded(click)) {name="CaseCard-"+id};
  card.style.width=KarineTheme.CaseBrowser.CardWidth;card.style.height=KarineTheme.CaseBrowser.CardHeight;
  card.style.flexShrink=0;card.style.flexDirection=FlexDirection.Column;
  card.style.alignItems=Align.Stretch;
  card.style.marginLeft=0;card.style.marginRight=KarineTheme.SpaceMd;
  card.style.marginTop=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceXs;card.style.paddingRight=KarineTheme.SpaceXs;
  card.style.paddingTop=KarineTheme.SpaceSm;card.style.paddingBottom=KarineTheme.SpaceXs;
  bool locked=!active&&!completed;
  card.style.backgroundColor=active?KarineTheme.Paper.Light:locked?KarineTheme.Alpha(KarineTheme.Paper.Tint,.55f):KarineTheme.Paper.Tint;
  Border(card,active?2:KarineTheme.BorderWidth,active?KarineTheme.Primary:KarineTheme.Paper.Edge);
  var photo=new VisualElement();photo.style.height=KarineTheme.CaseBrowser.PhotoHeight;
  photo.style.flexShrink=0;photo.style.overflow=Overflow.Hidden;card.Add(photo);
  if(art!=null) {
   var image=new Image {image=art,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
   image.style.width=Length.Percent(100);image.style.height=Length.Percent(100);photo.Add(image);
  } else CountryPostcard(photo,country);
  if(!active&&!completed) {
   var shade=new VisualElement();shade.style.position=Position.Absolute;
   shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
   shade.style.backgroundColor=KarineTheme.Veil(.58f);
   shade.style.alignItems=Align.Center;shade.style.justifyContent=Justify.Center;
   shade.Add(new CaseSeal(false));photo.Add(shade);
  }
  if(completed) {
   var seal=new CaseSeal(true);seal.style.position=Position.Absolute;
   seal.style.right=KarineTheme.SpaceXs;seal.style.bottom=KarineTheme.SpaceXs;photo.Add(seal);
  }
  var badge=Technical(card,index,KarineTheme.CaseBrowser.SmallSize);badge.pickingMode=PickingMode.Ignore;
  badge.style.position=Position.Absolute;badge.style.left=-KarineTheme.SpaceXs;badge.style.top=-KarineTheme.SpaceXs;
  badge.style.width=KarineTheme.CaseBrowser.Badge;badge.style.height=KarineTheme.CaseBrowser.Badge;badge.style.marginBottom=0;
  badge.style.unityTextAlign=TextAnchor.MiddleCenter;badge.style.color=KarineTheme.Primary;badge.style.backgroundColor=KarineTheme.GlassDeep;
  Border(badge,KarineTheme.BorderWidth,active?KarineTheme.Primary:KarineTheme.Accent);
  var no=Technical(card,number,KarineTheme.CaseBrowser.SmallSize);
  no.style.color=KarineTheme.Paper.Faded;no.style.marginTop=KarineTheme.SpaceXs;no.style.marginBottom=0;
  var name=Body_(card,title,KarineTheme.CaseBrowser.TextSize);
  name.style.color=KarineTheme.Paper.Ink;name.style.flexGrow=1;name.style.marginBottom=0;
  var foot=Technical(card,state.ToUpper(new System.Globalization.CultureInfo("tr-TR")),KarineTheme.CaseBrowser.SmallSize);
  foot.style.flexShrink=0;foot.style.backgroundColor=active?KarineTheme.Paper.Stamp:KarineTheme.GlassDeep;
  foot.style.color=active||completed?KarineTheme.Primary:KarineTheme.Muted;foot.style.unityTextAlign=TextAnchor.MiddleCenter;
  Round(foot,KarineTheme.Radius);foot.style.paddingTop=KarineTheme.SpaceXs;
  foot.style.paddingBottom=KarineTheme.SpaceXs;foot.style.marginBottom=0;
  if(locked){no.style.color=KarineTheme.Paper.Ink;}
  var tape=new VisualElement();tape.pickingMode=PickingMode.Ignore;
  tape.style.position=Position.Absolute;tape.style.top=-KarineTheme.SpaceXs;
  tape.style.left=Length.Percent(38);tape.style.width=KarineTheme.CaseBrowser.TapeWidth;
  tape.style.height=KarineTheme.CaseBrowser.TapeHeight;tape.style.backgroundColor=KarineTheme.Secondary;
  card.Add(tape);
  if(active) {var pin=Icon(card,"pin",KarineTheme.Paper.Stamp,KarineTheme.IconSize);
   pin.style.position=Position.Absolute;pin.style.top=-KarineTheme.SpaceSm;pin.style.left=Length.Percent(45);}
  // Locked cards remain fully legible; no Unity disabled-style opacity wash.
  if(click==null) {card.focusable=false;card.pickingMode=PickingMode.Ignore;}
  parent.Add(card);return card;
 }

 sealed class CaseSeal:VisualElement {
  readonly bool completed;
  public CaseSeal(bool done) {
   completed=done;pickingMode=PickingMode.Ignore;
   style.width=KarineTheme.TouchTarget;style.height=KarineTheme.TouchTarget;
   generateVisualContent+=Draw;
  }
  void Draw(MeshGenerationContext context) {
   var p=context.painter2D;var r=contentRect;
   var c=new Vector2(r.width*.5f,r.height*.5f);
   p.lineWidth=3;p.strokeColor=KarineTheme.Primary;
   if(completed) {
    p.fillColor=KarineTheme.Paper.Approved;p.BeginPath();p.Arc(c,15,0,360);p.Fill();
    p.BeginPath();p.MoveTo(c+new Vector2(-8,0));p.LineTo(c+new Vector2(-2,6));p.LineTo(c+new Vector2(9,-7));p.Stroke();
   } else {
    p.BeginPath();p.Arc(c+new Vector2(0,-5),7,180,360);p.Stroke();
    p.fillColor=KarineTheme.Secondary;p.BeginPath();
    p.MoveTo(c+new Vector2(-10,-5));p.LineTo(c+new Vector2(10,-5));
    p.LineTo(c+new Vector2(10,12));p.LineTo(c+new Vector2(-10,12));p.ClosePath();p.Fill();
    p.strokeColor=KarineTheme.Paper.Ink;p.BeginPath();p.MoveTo(c);p.LineTo(c+new Vector2(0,6));p.Stroke();
   }
  }
 }
}
}
