using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Kare dizisinden CCTV görüntüsü. Video yerine kayda birkaç durağan kare
// verilir; oyun onları güvenlik kamerası hızında (saniyede bir-iki kare)
// oynatır. Gerçek CCTV de böyle kayıt tutar, yani kesik akış bir eksik değil
// görüntünün dilidir. Kamera katmanı (köşeler, REC, tarama, parazit) video
// izleyicisiyle ortaktır; burada yalnız karenin kendisi ve damgası değişir.
public sealed partial class BubeApp {
 Texture2D[] cctvFrames;
 string[] cctvFrameTimes;
 int cctvFrameIndex, cctvFrameMs;
 bool cctvFramesPlaying;
 Image cctvFrameImage;
 Label cctvFrameRec;
 IVisualElementScheduledItem cctvFrameTask;

 void StartCctvFrames(CctvEvent record,Image image,Label rec) {
  var loaded=record.framePaths.Select(p=>Resources.Load<Texture2D>(p)).ToArray();
  if(loaded.Any(t=>t==null)) {
   Debug.LogWarning("CCTV frame missing: "+string.Join(", ",record.framePaths.Where((p,i)=>loaded[i]==null)));
   if(cctvVideoStatus!=null){cctvVideoStatus.text=T("cctv.videoUnavailable");cctvVideoStatus.style.display=DisplayStyle.Flex;}
   cctvPlaybackButton?.SetEnabled(false);cctvStepButton?.SetEnabled(false);
   return;
  }
  cctvFrames=loaded;cctvFrameImage=image;cctvFrameRec=rec;
  cctvFrameTimes=record.frameTimes;
  cctvFrameMs=record.frameMs>0?record.frameMs:KarineTheme.Effects.CctvFrameMs;
  cctvFrameIndex=-1;cctvReachedEnd=false;
  if(cctvVideoStatus!=null)cctvVideoStatus.style.display=DisplayStyle.None;
  cctvPlaybackButton?.SetEnabled(true);cctvStepButton?.SetEnabled(true);
  ShowCctvFrame(0);
  PlayCctvFrames(true);
 }

 void ShowCctvFrame(int index) {
  if(cctvFrames==null || cctvFrameImage==null)return;
  index=Mathf.Clamp(index,0,cctvFrames.Length-1);
  bool changed=index!=cctvFrameIndex;
  cctvFrameIndex=index;
  cctvFrameImage.image=cctvFrames[index];
  if(cctvFrameRec!=null && cctvFrameTimes!=null && index<cctvFrameTimes.Length && !string.IsNullOrEmpty(cctvFrameTimes[index]))
   cctvFrameRec.text=T("cctv.overlay.rec")+"  "+cctvFrameTimes[index];
  // Kare değişiminde kısa bir sinyal titremesi: görüntü bir an kararır ve
  // kayar. Her karede aynı; hiçbir kareyi öne çıkarmaz.
  if(changed && !KarineMotion.Reduced) {
   var picture=cctvFrameImage;
   KarineMotion.Run(picture,KarineTheme.Effects.CctvFlickerSeconds,t=>{
    picture.style.opacity=.72f+.28f*t;
    picture.style.translate=new Translate(KarineTheme.Effects.CctvJitter*(1-t)*(index%2==0?1:-1),0);
   });
  }
  bool last=index>=cctvFrames.Length-1;
  cctvStepButton?.SetEnabled(!last);
 }

 void PlayCctvFrames(bool play) {
  cctvFramesPlaying=play;
  cctvFrameTask?.Pause();cctvFrameTask=null;
  if(cctvPlaybackButton!=null)cctvPlaybackButton.text=T(play?"cctv.videoPause":"cctv.videoPlay");
  if(!play || cctvFrameImage==null)return;
  cctvFrameTask=cctvFrameImage.schedule.Execute(()=>{
   if(cctvFrames==null)return;
   if(cctvFrameIndex>=cctvFrames.Length-1){EndCctvFrames();return;}
   ShowCctvFrame(cctvFrameIndex+1);
  }).Every(FrameStep).StartingIn(FrameStep);
 }

 void EndCctvFrames() {
  cctvReachedEnd=true;
  PlayCctvFrames(false);
  cctvStepButton?.SetEnabled(false);
 }

 void ToggleCctvFrames() {
  if(cctvReachedEnd){ReplayCctvVideo();return;}
  PlayCctvFrames(!cctvFramesPlaying);
 }

 void StepCctvFrame() {
  if(cctvFrames==null || cctvReachedEnd)return;
  PlayCctvFrames(false);
  if(cctvFrameIndex<cctvFrames.Length-1)ShowCctvFrame(cctvFrameIndex+1);
  if(cctvFrameIndex>=cctvFrames.Length-1)cctvReachedEnd=true;
 }
}
}
