using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using S = Bube.KarineTheme.Scene;

namespace Bube {
// Ekranlar arası geçişler: masadaki eşyadan büyüyerek açılan ekran, çekmeceden
// yükselen delil, telefon hattının bağlanması, defter kapağının açılması.
//
// Geçiş yalnız **nereden** gelindiğini söyler (dokunulan eşya); gidilen yerde
// ne olduğunu söylemez. Her eşyanın geçişi her seferinde aynıdır.
public static partial class KarineUI {
 // Ses ve titreşim birlikte: olay adı → (ses, titreşim). Bir sesin ağırlığı ya
 // da titreşimi tek yerden değişir; ekranlar yalnız olay adını bilir.
 static readonly Dictionary<string,(string sound,Haptic? haptic)> cues=new Dictionary<string,(string,Haptic?)> {
  {"pin",("ui_pin",Haptic.Tick)},{"clip",("ui_clip",Haptic.Tick)},{"pen",("ui_pen",null)},
  {"door",("ui_door",Haptic.Press)},{"drawer",("ui_drawer",Haptic.Tick)},{"folder",("ui_folder",Haptic.Thud)},
  {"ring",("ui_ring",Haptic.Press)},{"rewind",("ui_rewind",Haptic.Tick)},{"key",("ui_key",null)},
  {"modem",("ui_modem",null)},{"envelope",("ui_envelope",Haptic.Press)},{"shelf",("ui_shelf",Haptic.Press)},
  {"polaroid",("ui_polaroid",Haptic.Tick)},{"rank",("ui_rank",Haptic.Thud)},{"warm",("ui_fax_warm",null)},
  {"paper",("ui_paper",null)},{"channel",("ui_channel",null)},{"tape",("ui_tape",null)},{"burn",("ui_burn",Haptic.Press)},{"step",("ui_press",Haptic.Tick)},
  {"drop_photo",("ui_drop_photo",Haptic.Tick)},{"drop_file",("ui_drop_file",Haptic.Tick)},{"drop_bag",("ui_drop_bag",Haptic.Thud)},
 };
 public static void Cue(string id) {
  if(!cues.TryGetValue(id,out var cue)){Sound?.Invoke(id);return;}
  Sound?.Invoke(cue.sound);
  if(cue.haptic.HasValue)Fx.Buzz(cue.haptic.Value);
 }

 // Dokunulan eşyadan büyüme: yeni ekran eşyanın masadaki yerinden açılır.
 // `box` masa sahnesinin yüzdeleriyle verilir.
 public static void ZoomFrom(VisualElement layer,Rect box) {
  if(layer==null || !Fx.On)return;
  layer.style.transformOrigin=new TransformOrigin(Length.Percent(box.x+box.width*.5f),Length.Percent(box.y+box.height*.5f));
  KarineMotion.Run(layer,S.ZoomSeconds,t=> {
   float e=1-(1-t)*(1-t)*(1-t);
   layer.style.scale=new Scale(Vector3.one*Mathf.Lerp(S.ZoomFrom,1,e));
   layer.style.opacity=Mathf.Clamp01(t*2.2f);
  },()=>{layer.style.scale=StyleKeyword.Null;layer.style.opacity=StyleKeyword.Null;});
 }

 // Çekmece: içerik aşağıdan kayarak yükselir, sonda tok bir duruş.
 public static void DrawerRise(VisualElement layer) {
  if(layer==null || !Fx.On)return;
  KarineMotion.Run(layer,S.DrawerSeconds,t=> {
   float e=1-(1-t)*(1-t);
   float settle=t>.85f?Mathf.Sin((t-.85f)/.15f*Mathf.PI)*1.2f:0;
   layer.style.translate=new Translate(0,Length.Percent((1-e)*40+settle));
   layer.style.opacity=Mathf.Clamp01(t*3);
  },()=>{layer.style.translate=StyleKeyword.Null;layer.style.opacity=StyleKeyword.Null;});
 }

 // Kısa siyah kesme: hattın bağlanması gibi bir sesin altında ekran bir an
 // karanlık kalır, sonra açılır. Dokunmak kesmeyi bitirir.
 public static void CutIn(VisualElement root,float hold) {
  if(root==null || !Fx.On)return;
  var black=new VisualElement {name="CutIn"};
  black.style.position=Position.Absolute;black.style.left=0;black.style.right=0;black.style.top=0;black.style.bottom=0;
  black.style.backgroundColor=Color.black;root.Add(black);
  bool done=false;
  Action finish=()=>{if(done)return;done=true;
   KarineMotion.Run(black,S.CutSeconds,t=>black.style.opacity=1-t,()=>black.RemoveFromHierarchy());};
  black.RegisterCallback<PointerDownEvent>(_=>finish());
  black.schedule.Execute(finish).StartingIn((long)(hold*1000));
 }

 // Defter kapağı: kapak soldan katlanarak açılır, altından sayfa görünür.
 public static void CoverOpen(VisualElement paper) {
  if(paper==null || !Fx.On)return;
  Cue("paper");
  var cover=new VisualElement {name="NotebookCover",pickingMode=PickingMode.Ignore};
  cover.style.position=Position.Absolute;cover.style.left=0;cover.style.top=0;cover.style.bottom=0;cover.style.width=Length.Percent(100);
  cover.style.backgroundColor=KarineTheme.Paper.FolderDeep;cover.style.borderRightWidth=3;cover.style.borderRightColor=KarineTheme.Paper.Edge;
  cover.style.transformOrigin=new TransformOrigin(0,Length.Percent(50));paper.Add(cover);
  KarineMotion.Run(cover,S.CoverSeconds,t=> {
   float e=t*t;
   cover.style.scale=new Scale(new Vector3(Mathf.Max(.001f,1-e),1,1));
   cover.style.opacity=1-e*.6f;
  },()=>cover.RemoveFromHierarchy());
 }
}
}
