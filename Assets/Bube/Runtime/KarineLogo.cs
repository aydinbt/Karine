using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {

// KARINE markası. Tek kaynak, tek oran, tek doku yoğunluğu — ekranlar yalnız
// **genişlik** seçer.
//
// Neden yazı tipi değil de görsel: aşınma efekti font dosyasının içine
// uygulanamaz (glif başına kırılma, ölçeğe göre değişen mürekkep kaybı). Bu
// yüzden logo, şeffaf arka planlı tek bir PNG'dir; oyunun geri kalanı
// aşınmasız yazı tipleriyle yazılır (`FontSet`).
//
// Katmanlar, istenen yapının aynısı:
//   KarineLogo
//    ├── LogoBase          — marka rengine boyanmış harfler
//    └── DistressOverlay   — üstüne gelen doku katmanı
//
// Bugün aşınma **temel görselin alfa kanalındadır**: harflerin içindeki yatay
// kopmalar ve kırık kenarlar oradan gelir. `DistressOverlay` boştur ve ancak
// `Bube/Art/KarineDistress` dosyası konulursa çizer. İkinci bir doku üst üste
// binerse okunabilirlik bozulur; katman yapısı duruyor ama varsayılanı yoktur.
public static class KarineLogo {

 // 2000 × 667. Hiçbir ekranda esnetilmez: yükseklik daima genişlikten türer.
 public const float AspectRatio = 2000f/667f;

 // Doku yoğunluğu ekrandan ekrana değişmez; sabit olması markanın şartıdır.
 const float DistressOpacity = 1f;

 public const string BaseResource = "Bube/Art/KarineLogo";
 public const string DistressResource = "Bube/Art/KarineDistress";

 public static VisualElement Hero(VisualElement parent,float width) => Build(parent,width,Color.white);

 // Ülke/vaka seçici, kariyer, dosya başlığı: küçük ama aynı marka.
 // Altındaki çizgi "KARINE ─────" görüntüsünü verir.
 public static VisualElement Header(VisualElement parent,float width,Color rule) =>
  Header(parent,width,rule,Color.white);

 // Acik kagit uzerinde krem harfler kaybolur: oralarda koyu bir ton verilir.
 // Ton degisir, oran ve doku yogunlugu degismez.
 public static VisualElement Header(VisualElement parent,float width,Color rule,Color tint) {
  var holder=new VisualElement();
  holder.style.alignItems=Align.FlexStart;
  parent.Add(holder);
  Build(holder,width,tint);
  var line=new VisualElement();
  line.style.height=2;line.style.width=width;
  line.style.backgroundColor=rule;
  line.style.marginTop=4;line.style.marginBottom=10;
  holder.Add(line);
  return holder;
 }

 public static VisualElement Build(VisualElement parent,float width,Color tint) {
  var logo=new VisualElement {name="KarineLogo"};
  logo.style.width=width;
  logo.style.height=width/AspectRatio;
  logo.style.flexShrink=0;
  logo.pickingMode=PickingMode.Ignore;
  parent.Add(logo);

  var art=Resources.Load<Texture2D>(BaseResource);
  if(art==null) {
   // Görsel bulunamazsa marka kaybolmasın: harfler yazıyla çizilir.
   var fallback=new Label("KARINE") {name="LogoBase"};
   fallback.style.color=tint;
   fallback.style.fontSize=Typography.Snap((int)(width*0.20f));
   fallback.style.letterSpacing=width*0.02f;
   fallback.style.unityFontStyleAndWeight=FontStyle.Bold;
   logo.Add(fallback);
   return logo;
  }

  logo.Add(Layer("LogoBase",art,tint,1f));
  var distress=Resources.Load<Texture2D>(DistressResource);
  logo.Add(Layer("DistressOverlay",distress,tint,distress==null?0f:DistressOpacity));
  return logo;
 }

 // Katmanlar üst üste oturur ve ikisi de `ScaleToFit` ile çizilir: kutu ne
 // kadar dar olursa olsun görsel kırpılmaz ve **esnemez**.
 static VisualElement Layer(string name,Texture2D texture,Color tint,float opacity) {
  var layer=new VisualElement {name=name};
  layer.style.position=Position.Absolute;
  layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  layer.pickingMode=PickingMode.Ignore;
  layer.style.opacity=opacity;
  if(texture!=null) {
   layer.style.backgroundImage=new StyleBackground(texture);
   layer.style.backgroundRepeat=new BackgroundRepeat(Repeat.NoRepeat,Repeat.NoRepeat);
   layer.style.backgroundSize=new BackgroundSize(BackgroundSizeType.Contain);
   layer.style.backgroundPositionX=new BackgroundPosition(BackgroundPositionKeyword.Center);
   layer.style.backgroundPositionY=new BackgroundPosition(BackgroundPositionKeyword.Center);
   layer.style.unityBackgroundImageTintColor=tint;
  }
  return layer;
 }
}
}
