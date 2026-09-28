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
                                  Texture2D art,bool selected,bool unlocked,Action click) {
  var card=new Button(Sounded(click)) {name="CountryTile-"+id};
  card.style.width=KarineTheme.CaseBrowser.CountryWidth;
  card.style.height=KarineTheme.CaseBrowser.CountryHeight;
  card.style.flexShrink=0;
  card.style.marginLeft=0;card.style.marginTop=0;
  card.style.marginRight=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceSm;card.style.paddingRight=KarineTheme.SpaceSm;
  card.style.justifyContent=Justify.FlexEnd;
  card.style.overflow=Overflow.Hidden;
  Border(card,selected?2:KarineTheme.BorderWidth,selected?KarineTheme.Primary:KarineTheme.Accent);
  CountryPostcard(card,id,art).style.opacity=unlocked?1f:.45f;
  if(id=="tr") {
   var flag=new Image {image=Resources.Load<Texture2D>("Bube/TurkiyeFlag"),scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   flag.style.position=Position.Absolute;flag.style.left=KarineTheme.SpaceSm;flag.style.top=KarineTheme.SpaceSm;
   flag.style.width=KarineTheme.IconSize+KarineTheme.SpaceMd;flag.style.height=KarineTheme.IconSize;card.Add(flag);
  }
  var plate=new VisualElement();
  plate.style.backgroundColor=selected?KarineTheme.Paper.Sheet:KarineTheme.Alpha(KarineTheme.Background,.9f);
  plate.style.paddingLeft=KarineTheme.SpaceXs;plate.style.paddingRight=KarineTheme.SpaceXs;
  card.Add(plate);
  var name=Body_(plate,title,KarineTheme.CaseBrowser.TextSize);
  name.style.color=selected?KarineTheme.Paper.Ink:KarineTheme.Primary;
  name.style.marginBottom=0;
  var tally=Body_(plate,count,KarineTheme.CaseBrowser.SmallSize);
  tally.style.color=selected?KarineTheme.Paper.Faded:KarineTheme.Secondary;
  tally.style.marginBottom=KarineTheme.SpaceXs;
  parent.Add(card);
  return card;
 }

 // Polaroid, tape, pin and status are individual UI elements, not a screen image.
 public static Button CasePhotoCard(VisualElement parent,string id,string country,string number,
  string title,string state,Texture2D art,bool active,bool completed,Action click) {
  var card=new Button(Sounded(click)) {name="CaseCard-"+id};
  card.style.width=KarineTheme.CaseBrowser.CardWidth;card.style.height=KarineTheme.CaseBrowser.CardHeight;
  card.style.flexShrink=0;card.style.flexDirection=FlexDirection.Column;
  card.style.alignItems=Align.Stretch;
  card.style.marginLeft=0;card.style.marginRight=KarineTheme.SpaceMd;
  card.style.marginTop=KarineTheme.SpaceSm;card.style.marginBottom=KarineTheme.SpaceSm;
  card.style.paddingLeft=KarineTheme.SpaceXs;card.style.paddingRight=KarineTheme.SpaceXs;
  card.style.paddingTop=KarineTheme.SpaceSm;card.style.paddingBottom=KarineTheme.SpaceXs;
  card.style.backgroundColor=active?KarineTheme.Paper.Light:KarineTheme.Paper.Tint;
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
  var no=Technical(card,number,KarineTheme.CaseBrowser.SmallSize);
  no.style.color=KarineTheme.Paper.Faded;no.style.marginTop=KarineTheme.SpaceXs;no.style.marginBottom=0;
  var name=Body_(card,title,KarineTheme.CaseBrowser.TextSize);
  name.style.color=KarineTheme.Paper.Ink;name.style.flexGrow=1;name.style.marginBottom=0;
  var foot=Body_(card,state,KarineTheme.CaseBrowser.SmallSize);
  foot.style.backgroundColor=KarineTheme.GlassDeep;
  foot.style.color=completed?KarineTheme.Primary:KarineTheme.Secondary;
  foot.style.paddingLeft=KarineTheme.SpaceSm;foot.style.paddingTop=KarineTheme.SpaceXs;
  foot.style.paddingBottom=KarineTheme.SpaceXs;foot.style.marginBottom=0;
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
