using UnityEngine;

namespace Bube {

// All text roles use Chakra Petch. Mono names remain for API compatibility;
// these roles now use proportional Chakra Petch too. The logo is a bitmap.
public sealed class FontSet {

 public Font Display { get; private set; }
 public Font Heading { get; private set; }
 public Font Mono { get; private set; }
 public Font MonoBold { get; private set; }
 public Font Body { get; private set; }
 public Font BodyBold { get; private set; }

 // Hangi rollerin hâlâ mono'ya düştüğü — kurulum eksiğini sessizce saklamamak için.
 public string[] Missing { get; private set; }

 public static FontSet Load() {
  var set=new FontSet();
  set.Mono=First("ChakraPetch-Regular");
  set.MonoBold=First("ChakraPetch-SemiBold") ?? set.Mono;
  // 3 Ekim 2026 panosu: başlıklar dar ve kalın (Bebas Neue); bulunamazsa eski Chakra Petch.
  var display=First("BebasNeue-Regular","ChakraPetch-Bold");
  var heading=First("BebasNeue-Regular","ChakraPetch-Bold");
  var body=First("ChakraPetch-Regular");
  var bodyBold=First("ChakraPetch-SemiBold");
  set.Heading=heading ?? set.MonoBold;
  set.Display=display ?? set.Heading;
  set.Body=body ?? set.Mono;
  set.BodyBold=bodyBold ?? set.MonoBold;
  var missing=new System.Collections.Generic.List<string>();
  if(display==null)missing.Add("Display (BebasNeue-Regular)");
  if(heading==null)missing.Add("Heading (BebasNeue-Regular)");
  if(body==null)missing.Add("Body (ChakraPetch-Regular)");
  if(bodyBold==null)missing.Add("BodyBold (ChakraPetch-SemiBold)");
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
