using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Bant ve terminal: oynatma hızı, iki kareyi yan yana tutmak, sık izlenen
// kaydın aşınması ve son aramalar. Bu dosya `BubeApp`in parçasıdır.
//
// Aşınma her kayıtta aynı kuralla işler (izlenme sayısı); hangi kaydın önemli
// olduğunu söylemez. Kare karşılaştırmasını oyuncu kendisi kurar.
public sealed partial class BubeApp {
 VisualElement cctvVideoFrame;
 float cctvSpeed=1f;
 static readonly float[] CctvSpeeds={.5f,1f,2f};
 readonly List<(int kind,string person)> recentSearches=new List<(int,string)>();

 int FrameStep => Mathf.Max(60,Mathf.RoundToInt(cctvFrameMs/cctvSpeed));

 string CycleCctvSpeed() {
  int index=(Array.IndexOf(CctvSpeeds,cctvSpeed)+1)%CctvSpeeds.Length;
  cctvSpeed=CctvSpeeds[index];
  KarineUI.Cue("tape");
  if(cctvFrames!=null){if(cctvFramesPlaying)PlayCctvFrames(true);}
  else if(cctvPlayer!=null)cctvPlayer.playbackSpeed=cctvSpeed;
  return cctvSpeed==.5f?"½x":cctvSpeed.ToString("0")+"x";
 }

 // Şu anki kare köşeye iğnelenir; oynatma sürerken onunla karşılaştırılır.
 // Küçük resme dokunmak onu kaldırır.
 void PinCctvFrame() {
  if(cctvFrames==null || cctvVideoFrame==null || cctvFrameIndex<0)return;
  cctvVideoFrame.Q("CctvPinned")?.RemoveFromHierarchy();
  var pin=new Image {name="CctvPinned",image=cctvFrames[cctvFrameIndex],scaleMode=ScaleMode.ScaleToFit};
  pin.style.position=Position.Absolute;pin.style.left=10;pin.style.bottom=10;
  pin.style.width=Length.Percent(28);pin.style.height=Length.Percent(28);
  pin.style.backgroundColor=KarineTheme.GlassDeep;KarineUI.Border(pin,2,KarineTheme.Accent);
  string time=cctvFrameTimes!=null && cctvFrameIndex<cctvFrameTimes.Length?cctvFrameTimes[cctvFrameIndex]:string.Empty;
  if(!string.IsNullOrEmpty(time)){var stamp=KarineUI.Technical(pin,time,KarineTheme.Office.SmallSize);stamp.style.color=KarineTheme.Film.Phosphor;stamp.pickingMode=PickingMode.Ignore;}
  pin.RegisterCallback<PointerDownEvent>(e=>{pin.RemoveFromHierarchy();e.StopPropagation();});
  cctvVideoFrame.Add(pin);KarineUI.Cue("pin");
 }

 // Bant aşınması: her izlenişte biraz daha gren. Sınırlı, okunurluğu bozmaz.
 void TapeWear(VisualElement frame,Node node,CctvEvent record) {
  string key="karine.tapeWear."+node.id+"."+record.id;
  int views=PlayerPrefs.GetInt(key,0)+1;PlayerPrefs.SetInt(key,views);
  KarineUI.TapeWear(frame,views);
 }

 // Son aramalar: oturumda kullanılan süzgeç birleşimleri. Çipe dokununca geri gelir, ✕ siler.
 void RecentSearches(VisualElement paper,Node[] people) {
  var now=(fileFilterKind,fileFilterPerson??"");
  recentSearches.Remove(now);recentSearches.Insert(0,now);
  if(recentSearches.Count>5)recentSearches.RemoveAt(5);
  var others=recentSearches.Skip(1).ToArray();
  if(others.Length==0)return;
  var row=KarineUI.SearchRecentRow(paper,T("search.recentTitle"));
  foreach(var entry in others) {
   var pick=entry;
   var person=people.FirstOrDefault(p=>p.personId==pick.person);
   string text=T(SearchKinds[Mathf.Clamp(pick.kind,0,3)])+" · "+(person==null?T("search.everyone"):T(person.personNameKey));
   KarineUI.SearchRecentChip(row,text,()=>{fileFilterKind=pick.kind;fileFilterPerson=pick.person;FileSearchPage();},
    ()=>{recentSearches.Remove(pick);FileSearchPage();},T("search.remove"));
  }
 }
}
}
