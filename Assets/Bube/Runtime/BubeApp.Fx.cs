using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Efekt katmanının oyuna bağlandığı yer: film dokusu, masanın hâli, masa
// eşyalarının sesleri ve konumları. Bu dosya `BubeApp`in bir parçasıdır.
public sealed partial class BubeApp {
 // 8 Ekim 2026: masada varsayılan hava yağmurdur (pencerede yağmur, ara ara gök
 // gürültüsü). Vaka "weather": "clear" yazarsa açık gecede kalır.
 string DeskWeather => game==null?null:string.IsNullOrEmpty(game.Data.weather)?"rain":game.Data.weather=="clear"?null:game.Data.weather;
 VisualElement filmLayer;
 // Bu oturumda kapısı kapandığı duyulmuş görüşmeler: kapı sesi bir kez çalar.
 readonly HashSet<string> heardDoors=new HashSet<string>();

 void InstallFx() {
  Fx.Load();
  KarineUI.SoundAt=(id,pan,gain)=>audioDirector?.PlayAt(id,pan,gain,id.StartsWith("amb_"));
 }

 // Film katmanı her ekranın üstünde durur. Ekranlar kökü temizler; katman
 // her karede yeniden en üste alınır, ekranların onu bilmesi gerekmez.
 void KeepFilmOnTop() {
  Fx.Watch(Time.unscaledDeltaTime);
  if(root==null)return;
  if(filmLayer==null)filmLayer=KarineUI.FilmLayer();
  if(filmLayer.parent!=root || root.IndexOf(filmLayer)!=root.childCount-1)root.Add(filmLayer);
  SceneTick();
 }

 // Masa eşyası → sesi. Eşyanın masadaki yeri sesin sol-sağ yerini verir.
 static readonly (string button,string sound,string prop)[] PropSounds={
  ("DeskInbox","ui_paper","Inbox"),("DeskFile","ui_folder","Folder"),("DeskInterviews","ui_dial","Phone"),
  ("DeskTerminal","ui_crt_on","Monitor"),("DeskEvidence","ui_drawer","Evidence"),
 };
 static Rect PropBox(string prop) {
  switch(prop) {
   case "Inbox": return KarineTheme.Office.Inbox;
   case "Folder": return KarineTheme.Office.Folder;
   case "Phone": return KarineTheme.Office.Phone;
   case "Monitor": return KarineTheme.Office.Monitor;
   default: return KarineTheme.Office.Evidence;
  }
 }

 // Masa kurulduktan sonra: saat tonu, hava, buhar, far, lamba; eşya sesleri.
 // `arriving` masaya başka bir sahneden gelindiğini söyler: lamba o zaman yanar.
 void DeskFx(VisualElement stage,bool arriving) {
  KarineUI.OfficeWeather(stage,game.Data.deskHour,DeskWeather,arriving);
  foreach(var entry in PropSounds) {
   var button=stage.Q<Button>(entry.button);if(button==null)continue;
   string sound=entry.sound;float pan=KarineUI.PanOf(PropBox(entry.prop));
   string prop=entry.prop;
   button.RegisterCallback<PointerDownEvent>(_=>{audioDirector?.PlayAt(sound,pan,.7f);Fx.Buzz(Haptic.Tick);pendingProp=prop;pendingAt=Time.unscaledTime;pressedScreen=null;if(prop=="Phone")KarineUI.CordSwing();},TrickleDown.TrickleDown);
  }
 }

 // Kapanan görüşme: bu oturumda ilk görüldüğünde kapı kapanır, ekran bir an kararır.
 void DoorClosed(Node node) {
  if(node==null || !heardDoors.Add(node.id))return;
  KarineUI.SoundAt?.Invoke("ui_door",.4f,.8f);Fx.Buzz(Haptic.Press);
  if(!Fx.On)return;
  var shade=new VisualElement {pickingMode=PickingMode.Ignore};
  shade.style.position=Position.Absolute;shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=Color.black;root.Add(shade);
  KarineMotion.Run(shade,.9f,t=>shade.style.opacity=t<.3f?t/.3f*.6f:.6f*(1-(t-.3f)/.7f),()=>shade.RemoveFromHierarchy());
 }
}
}
