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
    // Lambanın ışık havuzu, masadakinden biraz güçlü: renk farkı küçük görüntüde de seçilsin.
    var pool=new Image {image=Glow(),scaleMode=ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore,tintColor=KarineTheme.Alpha(color,.55f)};
    OfficePlace(pool,new Rect(52,0,56,90));view.Add(pool);
    break;
   }
   case "film": {
    var tone=new VisualElement {pickingMode=PickingMode.Ignore};Fill(tone);tone.style.backgroundColor=KarineTheme.Alpha(color,.45f);view.Add(tone);
    break;
   }
   case "city": {
    // Gece şehri: koyu pencere ve içinde yanan ışıklar.
    var night=new VisualElement {pickingMode=PickingMode.Ignore};OfficePlace(night,new Rect(10,10,80,80));
    night.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.92f);Border(night,6,KarineTheme.Panel2);view.Add(night);
    var rng=new System.Random(7);
    for(int i=0;i<60;i++) {
     var w=new VisualElement {pickingMode=PickingMode.Ignore};w.style.position=Position.Absolute;
     w.style.left=Length.Percent(rng.Next(3,95));w.style.top=Length.Percent(rng.Next(30,95));w.style.width=6;w.style.height=8;
     w.style.backgroundColor=KarineTheme.Alpha(color,.5f+(float)rng.NextDouble()*.5f);night.Add(w);
    }
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
