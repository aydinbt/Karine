using System;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Erişilebilirlik: ses betimlemesi (altyazı), okuma aralığı, yüksek karşıtlık
// ve tek elle oynama için alttaki geri düğmesi.
//
// Altyazı yalnız duyulan sesi söyler ("uzakta telefon çalıyor"); sesin bir
// anlamı olduğunu ima etmez. Karşıtlık geçişi yazının rengini değiştirir,
// içeriğini değil.
public static partial class KarineUI {
 public static void Caption(VisualElement root,string text) {
  if(root==null || string.IsNullOrEmpty(text))return;
  root.Q("SoundCaption")?.RemoveFromHierarchy();
  var label=Technical(root,"["+text+"]",KarineTheme.Office.SmallSize+1);
  label.name="SoundCaption";label.pickingMode=PickingMode.Ignore;
  label.style.position=Position.Absolute;label.style.bottom=Length.Percent(4);label.style.left=Length.Percent(30);label.style.right=Length.Percent(30);
  label.style.unityTextAlign=TextAnchor.MiddleCenter;label.style.color=KarineTheme.Primary;
  label.style.backgroundColor=KarineTheme.Alpha(KarineTheme.GlassDeep,.85f);label.style.paddingTop=4;label.style.paddingBottom=4;
  label.schedule.Execute(()=>KarineMotion.Run(label,.4f,t=>label.style.opacity=1-t,()=>label.RemoveFromHierarchy())).StartingIn((long)(S.CaptionSeconds*1000));
 }

 // Okuma geçişi: harf ve sözcük aralığı açılır (disleksi dostu), istenirse
 // yazı rengi arka planına karşı en az 7:1 karşıtlığa itilir. Her yazıya bir kez.
 const string Passed="karine.access";
 public static void AccessPass(VisualElement root,bool spacing,bool contrast) {
  if(root==null || !spacing && !contrast)return;
  root.Query<Label>().ForEach(label=> {
   if(label.ClassListContains(Passed))return;
   label.AddToClassList(Passed);
   if(spacing){label.style.letterSpacing=1.2f;label.style.wordSpacing=4f;}
   if(contrast)Contrast(label);
  });
 }
 static void Contrast(Label label) {
  var color=label.resolvedStyle.color;
  var back=Background(label);
  if(Ratio(color,back)>=7f)return;
  var dark=new Color(.04f,.04f,.05f,color.a);var light=new Color(.98f,.97f,.94f,color.a);
  label.style.color=Ratio(dark,back)>Ratio(light,back)?dark:light;
 }
 static Color Background(VisualElement element) {
  for(var e=element;e!=null;e=e.parent){var c=e.resolvedStyle.backgroundColor;if(c.a>.5f)return c;}
  return Color.black;
 }
 static float Luma(Color c) {
  float L(float v)=>v<=.03928f?v/12.92f:Mathf.Pow((v+.055f)/1.055f,2.4f);
  return .2126f*L(c.r)+.7152f*L(c.g)+.0722f*L(c.b);
 }
 static float Ratio(Color a,Color b){float x=Luma(a),y=Luma(b);return (Mathf.Max(x,y)+.05f)/(Mathf.Min(x,y)+.05f);}

 // Tek elle: geri gidilebilen her ekranda sağ altta başparmağın erişeceği bir geri düğmesi.
 public static void ThumbBack(VisualElement root,bool visible,Action back,string label) {
  var existing=root?.Q<Button>("ThumbBack");
  if(!visible || back==null){existing?.RemoveFromHierarchy();return;}
  if(existing!=null){if(root.IndexOf(existing)!=root.childCount-1)root.Add(existing);return;}
  var button=new Button(Sounded(back)) {name="ThumbBack",text="‹  "+label};
  Paint(button,KarineButtonKind.Secondary,true);
  button.style.position=Position.Absolute;button.style.right=Length.Percent(2);button.style.bottom=Length.Percent(3);
  button.style.minHeight=KarineTheme.TouchTarget;button.style.paddingLeft=18;button.style.paddingRight=18;
  root.Add(button);
 }
}
}
