using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Ayarlar > Görüntü: seçili rengin örnek görüntüsü. Masaya dönmeden lambanın, damganın,
// ekranın, şehir ışıklarının ya da film tonunun nasıl görüneceğini gösterir.
// Kilitli renkte pencerenin altına "Reklamla aç" gelir; açılınca pencere kapanır.
public static partial class KarineUI {
 public static VisualElement CosmeticPreview(VisualElement parent,string title,string id,Color color,
                                             string sample,string closeLabel,string unlockLabel,Action unlock,string note=null) {
  var veil=new VisualElement {name="CosmeticPreview"};
  veil.style.position=Position.Absolute;veil.style.left=0;veil.style.top=0;veil.style.right=0;veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(.78f);veil.style.alignItems=Align.Center;veil.style.justifyContent=Justify.Center;
  parent.Add(veil);
  Action close=()=>veil.RemoveFromHierarchy();
  veil.RegisterCallback<PointerDownEvent>(e=>{if(e.target==veil)close();});

  var card=Panel(veil,true);card.style.width=Length.Percent(62);card.style.maxWidth=900;
  Border(card,KarineTheme.BorderWidth,KarineTheme.Accent);
  Title(card,title,21);

  // Örnek görüntü masa resminin oranında; renk kendi katmanında boyanır.
  var view=new VisualElement {pickingMode=PickingMode.Ignore};
  view.style.width=Length.Percent(100);view.style.aspectRatio=KarineTheme.Office.Aspect;
  view.style.overflow=Overflow.Hidden;Round(view,KarineTheme.Radius);Border(view,KarineTheme.BorderWidth,KarineTheme.Border);
  view.style.marginBottom=KarineTheme.SpaceMd;card.Add(view);
  PaintPreview(view,id,color,sample);

  if(!string.IsNullOrEmpty(note))Body_(card,note,15).style.color=KarineTheme.Muted;
  var actions=Row(card);actions.style.justifyContent=Justify.FlexEnd;
  Button_(actions,closeLabel,close,KarineButtonKind.Secondary);
  if(unlock!=null)Button_(actions,unlockLabel,()=>{close();unlock();},KarineButtonKind.Primary).style.marginRight=0;
  Enter(veil,KarineTheme.ModalMs);
  return veil;
 }


 static void PaintPreview(VisualElement view,string id,Color color,string sample) {
  var desk=new Image {image=Resources.Load<Texture2D>("Bube/Art/OfficeDesk"),scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  Fill(desk);view.Add(desk);
  switch(id) {
   case "lamp": {
    // Masa geceye karartılır, lambanın ışık havuzu oyundakiyle aynı yerde ama güçlü çizilir:
    // açık tonlu renkler (yeşil banker, floresan, kehribar) aydınlık resmin üstünde seçilmiyordu.
    var dark=new VisualElement {pickingMode=PickingMode.Ignore};Fill(dark);dark.style.backgroundColor=KarineTheme.Veil(.62f);view.Add(dark);
    var pool=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(color,.85f)};
    OfficePlace(pool,KarineTheme.Office.Atmosphere.LightPool);view.Add(pool);
    var core=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=color};
    var r=KarineTheme.Office.Atmosphere.LightPool;OfficePlace(core,new Rect(r.x+r.width*.25f,r.y+r.height*.2f,r.width*.5f,r.height*.5f));view.Add(core);
    break;
   }
   case "film": {
    var tone=new VisualElement {pickingMode=PickingMode.Ignore};Fill(tone);tone.style.backgroundColor=KarineTheme.Alpha(color,.45f);view.Add(tone);
    break;
   }
   case "screen": {
    var screen=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(screen,new Rect(18,18,64,64));
    screen.style.backgroundColor=KarineTheme.Hex("#0B0F0D");Border(screen,6,KarineTheme.Panel2);Round(screen,10);
    screen.style.paddingLeft=24;screen.style.paddingTop=20;view.Add(screen);
    foreach(var line in new[]{"> "+sample,"> 12.16  ...","> _"})
     Technical(screen,line,22).style.color=color;
    break;
   }
   default: {
    // Damga mürekkebi: dosya kâğıdına basılmış damga.
    var paper=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(paper,new Rect(24,10,52,80));
    paper.style.backgroundColor=KarineTheme.Paper.Tint;paper.style.alignItems=Align.Center;paper.style.justifyContent=Justify.Center;
    paper.style.rotate=new Rotate(-2);view.Add(paper);
    var stamp=Technical(paper,sample,40);stamp.style.color=color;Border(stamp,5,color);Round(stamp,6);
    stamp.style.paddingLeft=18;stamp.style.paddingRight=18;stamp.style.rotate=new Rotate(-8);stamp.style.letterSpacing=4;
    break;
   }
  }
 }
}
}
