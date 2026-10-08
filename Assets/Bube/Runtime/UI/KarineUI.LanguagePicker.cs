using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using M = Bube.KarineTheme.SettingsModal;

namespace Bube {
// Dil seçici: bayrak + dilin kendi adı; dokununca altında liste açılır.
// Dil adı o dilin kendi kültürüyle büyütülür (Türkçe kültürde "English"
// "ENGLİSH" olurdu). Bayraklar sade şeritlerle çizilir; görsel dosyası yok.
public static partial class KarineUI {
 const int FlagW=36,FlagH=24;

 public static VisualElement LanguageDropdown(VisualElement parent,string[] codes,string current,Func<string,string> name,Action<string> pick) {
  var box=new VisualElement {name="LanguageDropdown"};box.style.flexDirection=FlexDirection.Column;box.style.width=Length.Percent(100);parent.Add(box);
  var list=new VisualElement {name="LanguageList"};
  Button head=LanguageOption(box,current,name(current),true,null);
  var arrow=Icon(head,"nav_next",KarineTheme.Primary,KarineTheme.IconSize);arrow.style.rotate=new Rotate(90);arrow.pickingMode=PickingMode.Ignore;
  head.clicked+=()=>{bool open=list.style.display!=DisplayStyle.Flex;list.style.display=open?DisplayStyle.Flex:DisplayStyle.None;arrow.style.rotate=new Rotate(open?-90:90);};
  list.style.display=DisplayStyle.None;Border(list,KarineTheme.BorderWidth,KarineTheme.Border);Round(list,KarineTheme.Radius);
  list.style.backgroundColor=KarineTheme.Alpha(KarineTheme.Background,.9f);list.style.marginTop=KarineTheme.SpaceXs;box.Add(list);
  foreach(var code in codes){var v=code;if(v==current)continue;LanguageOption(list,v,name(v),false,()=>pick(v));}
  return box;
 }

 static Button LanguageOption(VisualElement parent,string code,string label,bool head,Action click) {
  var b=new Button(Sounded(click??(()=>{})));b.style.flexDirection=FlexDirection.Row;b.style.alignItems=Align.Center;
  b.style.minHeight=M.CardHeight;b.style.marginLeft=0;b.style.marginRight=0;b.style.marginTop=0;b.style.marginBottom=0;
  b.style.paddingLeft=KarineTheme.SpaceMd;b.style.paddingRight=KarineTheme.SpaceMd;
  Unskin(b,head?KarineTheme.Alpha(KarineTheme.Background,.6f):Color.clear);
  if(head){Border(b,2,KarineTheme.Accent);Round(b,KarineTheme.Radius);}
  Flag(b,code).style.marginRight=KarineTheme.SpaceMd;
  CultureInfo culture;try{culture=new CultureInfo(code);}catch{culture=CultureInfo.InvariantCulture;}
  var t=Write(b,label.ToUpper(culture),KarineTheme.Primary,M.CardTitleSize,Heading);t.style.marginBottom=0;t.style.letterSpacing=1;t.style.flexGrow=1;Left(t);
  t.pickingMode=PickingMode.Ignore;parent.Add(b);return b;
 }

 // Sade bayrak: şeritler; Türkiye'de ay-yıldız yerine ay; İngiltere'de haç.
 public static VisualElement Flag(VisualElement parent,string code) {
  var f=new VisualElement {pickingMode=PickingMode.Ignore};f.style.width=FlagW;f.style.height=FlagH;f.style.flexShrink=0;f.style.overflow=Overflow.Hidden;
  Round(f,3);parent.Add(f);
  Color red=new Color(.82f,.1f,.15f),white=Color.white,blue=new Color(.0f,.2f,.55f),green=new Color(0f,.55f,.27f),yellow=new Color(1f,.8f,0f),black=new Color(.08f,.08f,.08f);
  switch(code) {
   case "tr": f.style.backgroundColor=red;Disc(f,12,7,10,white);Disc(f,14.5f,7.8f,8.4f,red);Disc(f,22,10,4,white);break;
   case "en": {f.style.backgroundColor=blue;
    // Union Jack: önce çaprazlar (beyaz geniş, kırmızı ince), sonra ortadaki haç.
    foreach(float a in new[]{33.7f,-33.7f}){var w=Bar(f,-4,10,44,4.5f,white);w.style.rotate=new Rotate(a);var r=Bar(f,-4,11.25f,44,1.5f,red);r.style.rotate=new Rotate(a);}
    Bar(f,0,8,36,8,white);Bar(f,14,0,8,24,white);Bar(f,0,9.75f,36,4.5f,red);Bar(f,15.75f,0,4.5f,24,red);break;}
   case "de": Stripes(f,false,black,red,yellow);break;
   case "fr": Stripes(f,true,blue,white,red);break;
   case "it": Stripes(f,true,green,white,red);break;
   case "es": f.style.backgroundColor=red;Bar(f,0,6,36,12,yellow);break;
   case "pt-BR": {f.style.backgroundColor=green;var d=Bar(f,9,3,18,18,yellow);d.style.rotate=new Rotate(45);d.style.scale=new Scale(new Vector3(1.1f,.75f,1));Disc(f,12,6,12,blue);break;}
   default: f.style.backgroundColor=KarineTheme.Panel2;break;
  }
  return f;
 }
 static void Stripes(VisualElement f,bool vertical,params Color[] colors) {
  f.style.flexDirection=vertical?FlexDirection.Row:FlexDirection.Column;
  foreach(var c in colors){var s=new VisualElement {pickingMode=PickingMode.Ignore};s.style.flexGrow=1;s.style.backgroundColor=c;f.Add(s);}
 }
 static VisualElement Bar(VisualElement f,float x,float y,float w,float h,Color c) {
  var b=new VisualElement {pickingMode=PickingMode.Ignore};b.style.position=Position.Absolute;b.style.left=x;b.style.top=y;b.style.width=w;b.style.height=h;
  b.style.backgroundColor=c;f.Add(b);return b;
 }
 static void Disc(VisualElement f,float x,float y,float size,Color c){Round(Bar(f,x,y,size,size,c),Mathf.CeilToInt(size/2));}
}
}
