using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UIElements;

namespace Bube {
// CCTV izleme: oynatma denetimleri ve kayıt ekranı.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 void StopCctvVideo() {
  if(cctvPlayer!=null) {
   cctvPlayer.prepareCompleted-=OnCctvVideoPrepared;
   cctvPlayer.errorReceived-=OnCctvVideoError;
   cctvPlayer.loopPointReached-=OnCctvVideoEnded;
   cctvPlayer.Stop();Destroy(cctvPlayer);cctvPlayer=null;
  }
  if(cctvTexture!=null){cctvTexture.Release();Destroy(cctvTexture);cctvTexture=null;}
  if(cctvViewer!=null){cctvViewer.RemoveFromHierarchy();cctvViewer=null;}
  cctvVideoStatus=null;
  cctvPlaybackButton=cctvStepButton=null;
  cctvReachedEnd=false;
 }
 void OnCctvVideoPrepared(VideoPlayer player) {
  if(player!=cctvPlayer)return;
  cctvReachedEnd=false;
  player.Play();
  bool canAdvance=player.canStep || player.canSetTime;
  if(cctvVideoStatus!=null){
   cctvVideoStatus.text=canAdvance?string.Empty:T("cctv.videoStepUnavailable");
   cctvVideoStatus.style.display=canAdvance?DisplayStyle.None:DisplayStyle.Flex;
  }
  if(cctvPlaybackButton!=null){cctvPlaybackButton.SetEnabled(true);cctvPlaybackButton.text=T("cctv.videoPause");}
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(canAdvance);
 }
 void OnCctvVideoEnded(VideoPlayer player) {
  if(player!=cctvPlayer)return;
  cctvReachedEnd=true;
  if(cctvPlaybackButton!=null)cctvPlaybackButton.text=T("cctv.videoPlay");
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
 }
 void OnCctvVideoError(VideoPlayer player,string message) {
  if(player!=cctvPlayer)return;
  Debug.LogWarning("CCTV footage unavailable: "+message);
  if(cctvVideoStatus!=null){cctvVideoStatus.text=T("cctv.videoUnavailable");cctvVideoStatus.style.display=DisplayStyle.Flex;}
  if(cctvPlaybackButton!=null)cctvPlaybackButton.SetEnabled(false);
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
 }
 void ReplayCctvVideo() {
  if(cctvPlayer==null)return;
  cctvReachedEnd=false;
  if(cctvVideoStatus!=null){cctvVideoStatus.text=T("cctv.videoLoading");cctvVideoStatus.style.display=DisplayStyle.Flex;}
  if(cctvPlaybackButton!=null)cctvPlaybackButton.SetEnabled(false);
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
  cctvPlayer.Stop();cctvPlayer.Prepare();
 }
 void OpenCctvVideo(Node node,CctvEvent record,VisualElement content) {
  if(string.IsNullOrEmpty(record.videoPath))return;
  StopCctvVideo();
  var viewer=new VisualElement();cctvViewer=viewer;
  viewer.style.position=Position.Absolute;
  viewer.style.left=0;viewer.style.right=0;viewer.style.top=0;viewer.style.bottom=0;
  viewer.style.flexDirection=FlexDirection.Column;
  viewer.style.paddingLeft=8;viewer.style.paddingRight=8;
  viewer.style.paddingTop=5;viewer.style.paddingBottom=5;
  viewer.style.backgroundColor=new Color(.035f,.065f,.085f);
  content.parent.Add(viewer);
  cctvTexture=new RenderTexture(1280,720,0,RenderTextureFormat.ARGB32);
  cctvTexture.Create();
  var videoFrame=new VisualElement();
  videoFrame.style.width=Length.Percent(100);videoFrame.style.flexGrow=1;
  videoFrame.style.minHeight=0;videoFrame.style.marginTop=0;videoFrame.style.marginBottom=5;
  viewer.Add(videoFrame);
  var image=new Image {image=cctvTexture,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
  image.style.position=Position.Absolute;
  image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  videoFrame.Add(image);
  var overlay=new VisualElement(){pickingMode=PickingMode.Ignore};
  overlay.style.position=Position.Absolute;
  overlay.style.backgroundColor=new Color(.025f,.055f,.06f,.12f);
  videoFrame.Add(overlay);
  var cameraMark=new Color(.88f,.91f,.87f,.84f);
  for(int corner=0;corner<4;corner++) {
   bool left=corner%2==0,upper=corner<2;
   var horizontal=new VisualElement(){pickingMode=PickingMode.Ignore};
   horizontal.style.position=Position.Absolute;horizontal.style.width=22;horizontal.style.height=2;
   horizontal.style.backgroundColor=cameraMark;
   if(left)horizontal.style.left=10;else horizontal.style.right=10;
   if(upper)horizontal.style.top=10;else horizontal.style.bottom=10;
   overlay.Add(horizontal);
   var vertical=new VisualElement(){pickingMode=PickingMode.Ignore};
   vertical.style.position=Position.Absolute;vertical.style.width=2;vertical.style.height=22;
   vertical.style.backgroundColor=cameraMark;
   if(left)vertical.style.left=10;else vertical.style.right=10;
   if(upper)vertical.style.top=10;else vertical.style.bottom=10;
   overlay.Add(vertical);
  }
  for(int i=0;i<8;i++) {
   var scanline=new VisualElement(){pickingMode=PickingMode.Ignore};
   scanline.style.position=Position.Absolute;scanline.style.left=0;scanline.style.right=0;
   scanline.style.top=Length.Percent(8+i*12);scanline.style.height=1;
   scanline.style.backgroundColor=new Color(.85f,.94f,.91f,.035f);
   overlay.Add(scanline);
  }
  var cameraLabel=new VisualElement(){pickingMode=PickingMode.Ignore};
  cameraLabel.style.position=Position.Absolute;cameraLabel.style.left=19;cameraLabel.style.top=17;
  cameraLabel.style.paddingLeft=7;cameraLabel.style.paddingRight=7;
  cameraLabel.style.paddingTop=5;cameraLabel.style.paddingBottom=5;
  cameraLabel.style.backgroundColor=new Color(.02f,.04f,.05f,.55f);overlay.Add(cameraLabel);
  var overlayTitle=string.IsNullOrEmpty(node.cctvOverlayKey)?T(node.cctvSourceKey):T(node.cctvOverlayKey);
  var cameraName=Text(cameraLabel,overlayTitle,Ink,14);cameraName.style.marginBottom=3;
  var recRow=new VisualElement(){pickingMode=PickingMode.Ignore};
  recRow.style.flexDirection=FlexDirection.Row;recRow.style.alignItems=Align.Center;cameraLabel.Add(recRow);
  var recDot=new VisualElement(){pickingMode=PickingMode.Ignore};
  recDot.style.width=8;recDot.style.height=8;recDot.style.marginRight=6;
  recDot.style.backgroundColor=KarineTheme.Danger;recRow.Add(recDot);
  var timeText=string.IsNullOrEmpty(record.overlayTimeKey)?string.Empty:T(record.overlayTimeKey);
  var recText=KarineUI.Technical(recRow,T("cctv.overlay.rec")+(timeText.Length==0?"":"  "+timeText),13);
  recText.style.color=Ink;
  recText.style.marginBottom=0;
  bool recVisible=true;
  recDot.schedule.Execute(()=>{recVisible=!recVisible;recDot.style.opacity=recVisible?1f:.15f;}).Every(480);
  var glitchBand=new VisualElement(){pickingMode=PickingMode.Ignore};
  glitchBand.style.position=Position.Absolute;glitchBand.style.left=0;glitchBand.style.right=0;
  glitchBand.style.top=Length.Percent(52);glitchBand.style.height=2;
  glitchBand.style.backgroundColor=new Color(.85f,.96f,.93f,.20f);
  glitchBand.style.opacity=0;overlay.Add(glitchBand);
  glitchBand.schedule.Execute(()=>{
   glitchBand.style.top=Length.Percent(UnityEngine.Random.Range(18f,82f));
   glitchBand.style.opacity=UnityEngine.Random.value<.13f?.38f:0f;
  }).Every(180);
  // Keep the skip control over the supplied footage's lower-right mark,
  // including when ScaleToFit adds letterboxing around the 16:9 frame.
  // Görüntünün üstündeki düğmeler de kit bileşenidir; saydam zemin yok.
  var skipVideo=KarineUI.SkipButton(videoFrame,T("intro.skip"),StopCctvVideo);
  skipVideo.style.position=Position.Absolute;
  var close=KarineUI.Button_(videoFrame,T("cctv.videoClose"),StopCctvVideo,KarineButtonKind.Secondary);
  close.style.position=Position.Absolute;close.style.right=10;close.style.top=10;
  close.style.marginRight=0;close.style.marginBottom=0;
  videoFrame.RegisterCallback<GeometryChangedEvent>(evt=>{
   float width=videoFrame.resolvedStyle.width,height=videoFrame.resolvedStyle.height;
   if(float.IsNaN(width)||float.IsNaN(height)||width<=0||height<=0)return;
   float scale=Mathf.Min(width/1280f,height/720f);
   float offsetX=(width-1280f*scale)*.5f;
   float offsetY=(height-720f*scale)*.5f;
   overlay.style.left=offsetX;overlay.style.top=offsetY;
   overlay.style.width=1280f*scale;overlay.style.height=720f*scale;
   cameraName.style.fontSize=Mathf.Clamp(14f*scale,11f,14f);
   cameraName.style.maxWidth=Mathf.Max(100f,1280f*scale-60f);
   float buttonWidth=Mathf.Max(110f,150f*scale);
   float buttonHeight=Mathf.Max(MinimumTouchTarget,84f*scale);
   skipVideo.style.width=buttonWidth;skipVideo.style.height=buttonHeight;
   skipVideo.style.left=Mathf.Clamp(offsetX+1280f*scale-buttonWidth,0f,Mathf.Max(0f,width-buttonWidth));
   skipVideo.style.top=Mathf.Clamp(offsetY+575f*scale,0f,Mathf.Max(0f,height-buttonHeight));
  });
  var controls=new VisualElement();controls.style.flexDirection=FlexDirection.Row;
  controls.style.alignItems=Align.Center;controls.style.flexShrink=0;viewer.Add(controls);
  var caption=Text(controls,T(record.textKey),Ink,15);
  caption.style.flexGrow=1;caption.style.minWidth=0;
  caption.style.marginBottom=0;caption.style.marginRight=8;
  cctvPlaybackButton=new Button(()=>{
   if(cctvPlayer==null || !cctvPlayer.isPrepared)return;
   if(cctvReachedEnd){ReplayCctvVideo();return;}
   if(cctvPlayer.isPlaying){cctvPlayer.Pause();cctvPlaybackButton.text=T("cctv.videoPlay");}
   else {cctvPlayer.Play();cctvPlaybackButton.text=T("cctv.videoPause");}
  }){text=T("cctv.videoPlay")};
  cctvStepButton=new Button(()=>{
   if(cctvPlayer==null || !cctvPlayer.isPrepared || cctvReachedEnd)return;
   if(cctvPlayer.isPlaying)cctvPlayer.Pause();
   cctvPlaybackButton.text=T("cctv.videoPlay");
   if(cctvPlayer.canStep)cctvPlayer.StepForward();
   else if(cctvPlayer.canSetTime)cctvPlayer.frame=Math.Max(0L,cctvPlayer.frame)+1L;
  }){text=T("cctv.videoStep")};
  var replay=new Button(ReplayCctvVideo){text=T("cctv.videoReplay")};
  foreach(var button in new[]{cctvPlaybackButton,cctvStepButton,replay}) {
   KarineUI.Paint(button,KarineButtonKind.Secondary,true);
   button.style.width=118;button.style.flexShrink=0;
   button.style.minHeight=MinimumTouchTarget;button.style.fontSize=Typography.Snap(14);
   button.style.marginRight=4;button.style.marginBottom=0;controls.Add(button);
  }
  cctvPlaybackButton.SetEnabled(false);cctvStepButton.SetEnabled(false);
  cctvVideoStatus=Text(videoFrame,T("cctv.videoLoading"),Gold,13);
  cctvVideoStatus.style.position=Position.Absolute;
  cctvVideoStatus.style.left=Length.Percent(30);
  cctvVideoStatus.style.right=Length.Percent(30);
  cctvVideoStatus.style.top=12;
  cctvVideoStatus.style.unityTextAlign=TextAnchor.MiddleCenter;
  cctvVideoStatus.style.backgroundColor=new Color(.02f,.04f,.05f,.78f);
  cctvVideoStatus.style.marginBottom=0;
  cctvPlayer=gameObject.AddComponent<VideoPlayer>();
  cctvPlayer.playOnAwake=false;cctvPlayer.isLooping=false;
  cctvPlayer.renderMode=VideoRenderMode.RenderTexture;
  cctvPlayer.targetTexture=cctvTexture;
  cctvPlayer.audioOutputMode=VideoAudioOutputMode.None;
  cctvPlayer.source=VideoSource.Url;
  cctvPlayer.url=Application.streamingAssetsPath+"/"+record.videoPath;
  cctvPlayer.prepareCompleted+=OnCctvVideoPrepared;
  cctvPlayer.errorReceived+=OnCctvVideoError;
  cctvPlayer.loopPointReached+=OnCctvVideoEnded;
  cctvPlayer.Prepare();
 }
 void CctvScreen(Node node,string focusEventId=null) {
  showingInterviewList=false;
  VisualElement content;
  BpsTablet("cctv.archive",out content);
  TerminalSourceTabs(content,node);
  var meta=new VisualElement();meta.style.flexDirection=FlexDirection.Row;
  meta.style.alignItems=Align.Center;content.Add(meta);
  var camera=Text(meta,T(node.cctvSourceKey),Ink,22);
  camera.style.flexGrow=1;camera.style.marginBottom=0;
  var status=Text(meta,T("cctv.signal"),Gold,14);status.style.marginBottom=0;
  var period=Text(content,T(node.cctvPeriodKey),Muted,14);period.style.marginBottom=7;
  var recordPanel=new VisualElement();recordPanel.style.flexGrow=1;
  recordPanel.style.backgroundColor=new Color(.045f,.08f,.105f);
  recordPanel.style.borderTopWidth=1;recordPanel.style.borderBottomWidth=1;
  recordPanel.style.borderLeftWidth=1;recordPanel.style.borderRightWidth=1;
  recordPanel.style.borderTopColor=Muted;recordPanel.style.borderBottomColor=Muted;
  recordPanel.style.borderLeftColor=Muted;recordPanel.style.borderRightColor=Muted;
  recordPanel.style.paddingLeft=11;recordPanel.style.paddingRight=11;
  recordPanel.style.paddingTop=6;content.Add(recordPanel);
  var stream=Scroll(recordPanel);
  stream.style.paddingTop=2;
  var records=node.cctvEvents ?? new CctvEvent[0];
  var rows=new List<VisualElement>();
  var lines=new List<Label>();
  var actions=new List<VisualElement>();
  foreach(var record in records) {
   var row=new VisualElement();row.style.display=DisplayStyle.None;
   row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
   row.style.minHeight=44;row.style.paddingLeft=10;row.style.paddingRight=8;
   row.style.marginBottom=3;row.style.borderBottomWidth=1;
   row.style.borderBottomColor=new Color(.19f,.26f,.28f);
   row.style.backgroundColor=new Color(.055f,.10f,.12f);
   stream.Add(row);rows.Add(row);
   var label=Text(row,string.Empty,Ink,22);label.style.flexGrow=1;
   label.style.marginBottom=0;lines.Add(label);
   var action=new VisualElement();row.Add(action);actions.Add(action);
  }
  Action<int> addFootageButton=index=>{
   var record=records[index];
   if(string.IsNullOrEmpty(record.videoPath))return;
   var watch=new Button(()=>OpenCctvVideo(node,record,content)){text="▶ "+T("cctv.watch")};
   watch.tooltip=T("cctv.watch");
   watch.style.minWidth=88;watch.style.height=MinimumTouchTarget;
   watch.style.backgroundColor=Paper;watch.style.color=Gold;
   watch.style.fontSize=Typography.Snap(15);actions[index].Add(watch);
  };
  var controls=new VisualElement();content.Add(controls);
  if(!string.IsNullOrEmpty(focusEventId) && game.State.read.Contains(node.id)) {
   status.text=T("cctv.complete");
   for(int i=0;i<records.Length;i++) {
    rows[i].style.display=DisplayStyle.Flex;
    lines[i].text=T(records[i].textKey);
    addFootageButton(i);
   }
   int focus=Array.FindIndex(records,e=>e.id==focusEventId);
   if(focus>=0)content.schedule.Execute(()=>stream.ScrollTo(rows[focus]));
   return;
  }
  bool reviewing=false;
  Button(controls,T("cctv.review"),()=>{
   if(reviewing)return;
   reviewing=true;
   controls.Clear();
   Text(controls,T("cctv.scanning"),Gold,15);
   int index=0;
   Action next=null;
   next=()=>{
    if(index>=records.Length) {
     controls.Clear();
     status.text=T("cctv.complete");
     game.Read(node.id);Save();
     return;
    }
    int current=index++;
    var record=records[current];
    int delay=record.delayMs>0?record.delayMs:650;
    content.schedule.Execute(()=>{
     if(!string.IsNullOrEmpty(record.signalKey))status.text=T(record.signalKey);
     rows[current].style.display=DisplayStyle.Flex;
     var line=lines[current];
     string finalText=T(string.IsNullOrEmpty(record.glitchKey)?record.textKey:record.glitchKey);
     line.text="▒▒▒  " + T("cctv.syncing");
     content.schedule.Execute(()=>{
      line.text=finalText;
      stream.ScrollTo(rows[current]);
      addFootageButton(current);
      if(!string.IsNullOrEmpty(record.glitchKey)) {
       Button clarify=null;
       clarify=new Button(()=>{
        clarify.RemoveFromHierarchy();
        line.text=T("cctv.syncing");
        content.schedule.Execute(()=>{line.text=T(record.textKey);}).ExecuteLater(360);
       }){text="↻"};
       clarify.tooltip=T("cctv.clarify");
       clarify.style.width=MinimumTouchTarget;clarify.style.height=MinimumTouchTarget;clarify.style.fontSize=Typography.Snap(25);
       clarify.style.backgroundColor=Paper;clarify.style.color=Gold;
       actions[current].Add(clarify);
      }
      next();
     }).ExecuteLater(110+current%3*70);
    }).ExecuteLater(delay);
   };
   next();
  },true);
 }
}
}
