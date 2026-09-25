using UnityEngine;

namespace Bube {

// Yazı tipi **rolleri**. Oyunun dört ayrı işi vardır ve her biri farklı bir
// yazı tipi ister:
//
//   Logo     — yalnız KARINE markası. Yazı tipi değil, görseldir (`KarineLogo`).
//   Heading  — ekran başlıkları. Ağır slab-serif (Roboto Slab benzeri).
//   Mono     — dosya, terminal, tarih, vaka numarası. IBM Plex Mono.
//   Body     — açıklamalar ve düğmeler. Inter / IBM Plex Sans.
//
// Dosya adları burada sabittir, çağrı yerlerinde değil. Bir rolün dosyası
// projede yoksa **bir sonraki adaya**, en sonunda mono'ya düşer; ekran hiçbir
// zaman yazısız kalmaz. Böylece `RobotoSlab-Bold.ttf` ve `Inter-Regular.ttf`
// klasöre bırakıldığı an oyunun tamamı tek yerden geçiş yapar.
public sealed class FontSet {

 public Font Heading { get; private set; }
 public Font Mono { get; private set; }
 public Font MonoBold { get; private set; }
 public Font Body { get; private set; }
 public Font BodyBold { get; private set; }

 // Hangi rollerin hâlâ mono'ya düştüğü — kurulum eksiğini sessizce saklamamak için.
 public string[] Missing { get; private set; }

 public static FontSet Load() {
  var set=new FontSet();
  set.Mono=First("IBMPlexMono-Regular");
  set.MonoBold=First("IBMPlexMono-SemiBold") ?? set.Mono;
  var heading=First("RobotoSlab-ExtraBold","RobotoSlab-Bold","Arvo-Bold");
  var body=First("Inter-Regular","IBMPlexSans-Regular");
  var bodyBold=First("Inter-SemiBold","Inter-Bold","IBMPlexSans-SemiBold");
  set.Heading=heading ?? set.MonoBold;
  set.Body=body ?? set.Mono;
  set.BodyBold=bodyBold ?? set.MonoBold;
  var missing=new System.Collections.Generic.List<string>();
  if(heading==null)missing.Add("Heading (RobotoSlab-ExtraBold)");
  if(body==null)missing.Add("Body (Inter-Regular)");
  if(bodyBold==null)missing.Add("BodyBold (Inter-SemiBold)");
  set.Missing=missing.ToArray();
  return set;
 }

 static Font First(params string[] names) {
  foreach(var name in names) {
   var font=Resources.Load<Font>("Bube/Fonts/"+name);
   if(font!=null)return font;
  }
  return null;
 }
}
}
