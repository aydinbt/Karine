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
  cctvFrameTask?.Pause();cctvFrameTask=null;cctvFrames=null;cctvFrameImage=null;cctvFrameRec=null;
  if(cctvViewer!=null) {
   // Ekran kapanırken görüntü bir çizgiye, çizgi bir noktaya söner.
   if(cctvViewer.parent!=null && cctvViewer.panel!=null){audioDirector?.Play("ui_crt_off",1f,.6f);KarineUI.CrtOff(cctvViewer.parent);}
   cctvViewer.RemoveFromHierarchy();cctvViewer=null;
  }
  cctvVideoStatus=null;
  cctvPlaybackButton=cctvStepButton=null;
  cctvReachedEnd=false;
  audioDirector?.Hush(false);
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
  if(cctvFrames!=null){cctvFrameIndex=-1;cctvFrameLoop=0;cctvReachedEnd=false;ShowCctvFrame(0);PlayCctvFrames(true);return;}
  if(cctvPlayer==null)return;
  cctvReachedEnd=false;
  if(cctvVideoStatus!=null){cctvVideoStatus.text=T("cctv.videoLoading");cctvVideoStatus.style.display=DisplayStyle.Flex;}
  if(cctvPlaybackButton!=null)cctvPlaybackButton.SetEnabled(false);
  if(cctvStepButton!=null)cctvStepButton.SetEnabled(false);
  cctvPlayer.Stop();cctvPlayer.Prepare();
 }
 void OpenCctvVideo(Node node,CctvEvent record,VisualElement content) {
  if(!record.HasFootage)return;
  StopCctvVideo();
  audioDirector?.Hush(true);
  bool frames=record.framePaths!=null && record.framePaths.Length>0;
  var viewer=new VisualElement();cctvViewer=viewer;
  viewer.style.position=Position.Absolute;
  viewer.style.left=0;viewer.style.right=0;viewer.style.top=0;viewer.style.bottom=0;
  viewer.style.flexDirection=FlexDirection.Column;
  viewer.style.paddingLeft=KarineTheme.SpaceLg;viewer.style.paddingRight=KarineTheme.SpaceLg;
  viewer.style.paddingTop=KarineTheme.SpaceMd;viewer.style.paddingBottom=KarineTheme.SpaceMd;
  // CCTV görüntüsünün kendisi arayüz yüzeyi değil, **kameranın resmidir**:
  // yeşile çalan cam, tarama çizgisi, parazit bandı ve köşe işaretleri kit
  // paletinden gelmez; kamera görüntüsü gibi görünmeleri gerekir.
  viewer.style.backgroundColor=new Color(.035f,.065f,.085f);
  content.parent.Add(viewer);
  if(!frames){cctvTexture=new RenderTexture(1280,720,0,RenderTextureFormat.ARGB32);cctvTexture.Create();}
  var videoFrame=new VisualElement();
  videoFrame.style.width=Length.Percent(100);videoFrame.style.flexGrow=1;
  videoFrame.style.minHeight=0;videoFrame.style.marginTop=0;videoFrame.style.marginBottom=5;
  viewer.Add(videoFrame);
  var image=new Image {image=frames?null:cctvTexture,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
  image.style.position=Position.Absolute;
  image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  videoFrame.Add(image);
  videoFrame.schedule.Execute(()=>{KarineUI.Glare(videoFrame);TapeWear(videoFrame,node,record);}).StartingIn(0);
  cctvVideoFrame=videoFrame;cctvSpeed=1f;
  audioDirector?.Play("ui_crt_on",1f,.6f);
  KarineUI.VhsTrace(videoFrame,image);
  // Yaklaşmak oynatmayı durdurur; kare olduğu gibi kalır.
  KarineUI.Zoomable(videoFrame,image,()=>{
   if(cctvFrames!=null){if(cctvFramesPlaying)PlayCctvFrames(false);return;}
   if(cctvPlayer!=null && cctvPlayer.isPlaying){cctvPlayer.Pause();if(cctvPlaybackButton!=null)cctvPlaybackButton.text=T("cctv.videoPlay");}
  });
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
  cameraLabel.style.position=Position.Absolute;cameraLabel.style.right=19;cameraLabel.style.top=17;
  cameraLabel.style.flexDirection=FlexDirection.Row;cameraLabel.style.alignItems=Align.Center;
  cameraLabel.style.paddingLeft=7;cameraLabel.style.paddingRight=7;
  cameraLabel.style.paddingTop=5;cameraLabel.style.paddingBottom=5;
  cameraLabel.style.backgroundColor=new Color(.02f,.04f,.05f,.55f);overlay.Add(cameraLabel);
  var overlayTitle=string.IsNullOrEmpty(node.cctvOverlayKey)?T(node.cctvSourceKey):T(node.cctvOverlayKey);
  var cameraName=Text(cameraLabel,overlayTitle,Ink,14);cameraName.style.marginBottom=0;cameraName.style.marginRight=12;
  var recRow=new VisualElement(){pickingMode=PickingMode.Ignore};
  recRow.style.flexDirection=FlexDirection.Row;recRow.style.alignItems=Align.Center;cameraLabel.Add(recRow);
  var recDot=new VisualElement(){pickingMode=PickingMode.Ignore};
  recDot.style.width=8;recDot.style.height=8;recDot.style.marginRight=6;
  recDot.style.backgroundColor=KarineTheme.Danger;recRow.Add(recDot);
  var timeText=string.IsNullOrEmpty(record.overlayTimeKey)?string.Empty:T(record.overlayTimeKey);
  var recText=KarineUI.Technical(recRow,T("cctv.overlay.rec")+(timeText.Length==0?"":"  "+timeText),13);
  recText.style.color=Ink;KarineUI.StampJitter(recText);
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
  // Sol üstte döküme dönüş; maketteki yeri.
  var toLog=KarineUI.CctvControl(videoFrame,"nav_prev",T("cctv.videoClose"),false,StopCctvVideo);
  toLog.style.position=Position.Absolute;toLog.style.left=10;toLog.style.top=10;
  videoFrame.RegisterCallback<GeometryChangedEvent>(evt=>{
   float width=videoFrame.resolvedStyle.width,height=videoFrame.resolvedStyle.height;
   if(float.IsNaN(width)||float.IsNaN(height)||width<=0||height<=0)return;
   float scale=Mathf.Min(width/1280f,height/720f);
   float offsetX=(width-1280f*scale)*.5f;
   float offsetY=(height-720f*scale)*.5f;
   overlay.style.left=offsetX;overlay.style.top=offsetY;
   overlay.style.width=1280f*scale;overlay.style.height=720f*scale;
   cameraName.style.fontSize=Mathf.Clamp(14f*scale,11f,14f);
  });
  // Denetim çubuğu: BAŞA AL, OYNAT/DURAKLAT (amber), KARE ›, ilerleme ve saat, kare geri, iğnele, hız.
  var controls=new VisualElement {name="CctvControls"};controls.style.flexDirection=FlexDirection.Row;
  controls.style.alignItems=Align.Center;controls.style.flexShrink=0;viewer.Add(controls);
  var replay=KarineUI.CctvControl(controls,"nav_prev",T("cctv.videoReplay"),false,()=>{KarineUI.Rewind(videoFrame);ReplayCctvVideo();});
  cctvPlaybackButton=KarineUI.CctvControl(controls,null,T("cctv.videoPlay"),true,()=>{
   if(cctvFrames!=null){ToggleCctvFrames();return;}
   if(cctvPlayer==null || !cctvPlayer.isPrepared)return;
   if(cctvReachedEnd){ReplayCctvVideo();return;}
   if(cctvPlayer.isPlaying){cctvPlayer.Pause();cctvPlaybackButton.text=T("cctv.videoPlay");}
   else {cctvPlayer.Play();cctvPlaybackButton.text=T("cctv.videoPause");}
  });
  cctvPlaybackButton.style.minWidth=150;
  cctvStepButton=KarineUI.CctvControl(controls,null,T("cctv.videoStep"),false,()=>{
   if(cctvFrames!=null){StepCctvFrame();return;}
   if(cctvPlayer==null || !cctvPlayer.isPrepared || cctvReachedEnd)return;
   if(cctvPlayer.isPlaying)cctvPlayer.Pause();
   cctvPlaybackButton.text=T("cctv.videoPlay");
   if(cctvPlayer.canStep)cctvPlayer.StepForward();
   else if(cctvPlayer.canSetTime)cctvPlayer.frame=Math.Max(0L,cctvPlayer.frame)+1L;
  });
  KarineUI.CctvProgress(controls,out var progressFill);
  var clock=KarineUI.Technical(controls,timeText,KarineTheme.CctvArchive.TimeSize);clock.style.marginBottom=0;clock.style.marginRight=KarineTheme.SpaceMd;clock.style.color=Ink;
  var back=KarineUI.CctvControl(controls,null,T("cctv.frameBack"),false,StepCctvBack);
  var compare=KarineUI.CctvControl(controls,"pin",null,false,PinCctvFrame);compare.tooltip=T("cctv.compare");compare.style.display=frames?DisplayStyle.Flex:DisplayStyle.None;
  Button speed=null;speed=KarineUI.CctvControl(controls,null,"1.0x",false,()=>KarineUI.CctvControlText(speed,CycleCctvSpeed()));
  speed.style.marginRight=0;
  // Düğme metni oynatma durumuyla değişir; `text` yerine iç etiket güncellenir.
  controls.schedule.Execute(()=>{
   KarineUI.CctvControlText(cctvPlaybackButton,cctvPlaybackButton.text);
   float ratio=0f;string now=timeText;
   if(cctvFrames!=null && cctvFrames.Length>1){ratio=Mathf.Max(0,cctvFrameIndex)/(float)(cctvFrames.Length-1);
    if(cctvFrameTimes!=null && cctvFrameIndex>=0 && cctvFrameIndex<cctvFrameTimes.Length && !string.IsNullOrEmpty(cctvFrameTimes[cctvFrameIndex]))now=cctvFrameTimes[cctvFrameIndex];}
   else if(cctvPlayer!=null && cctvPlayer.isPrepared && cctvPlayer.length>0){ratio=(float)(cctvPlayer.time/cctvPlayer.length);}
   if(cctvReachedEnd)ratio=1f;
   progressFill.style.width=Length.Percent(Mathf.Clamp01(ratio)*100f);clock.text=now;
  }).Every(150);
  string captionTime,captionBody;
  KarineUI.SplitCctvLine(T(record.textKey),string.IsNullOrEmpty(record.overlayTimeKey)?null:T(record.overlayTimeKey),out captionTime,out captionBody);
  KarineUI.CctvCaption(viewer,captionTime,captionBody);
  cctvPlaybackButton.SetEnabled(false);cctvStepButton.SetEnabled(false);
  cctvVideoStatus=Text(videoFrame,T("cctv.videoLoading"),Gold,13);
  cctvVideoStatus.style.position=Position.Absolute;
  cctvVideoStatus.style.left=Length.Percent(30);
  cctvVideoStatus.style.right=Length.Percent(30);
  cctvVideoStatus.style.top=12;
  cctvVideoStatus.style.unityTextAlign=TextAnchor.MiddleCenter;
  cctvVideoStatus.style.backgroundColor=new Color(.02f,.04f,.05f,.78f);
  cctvVideoStatus.style.marginBottom=0;
  KarineUI.FromBlack(viewer,frames?(Action)(()=>{if(cctvViewer==viewer)StartCctvFrames(record,image,recText);}):null);
  if(frames)return;
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
 // 3 Ekim 2026 maketi: dosya ekranlarının üst şeridi, solda kamera listesi, sağda monitör dökümü.
 // Henüz erişilebilir kamera kaydı yoksa aynı tam ekran çerçeve, boş monitörle açılır.
 void CctvEmpty() {
  Back(Desk);
  Desk();
  KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T("back.desk"),T(game.Data.titleKey),T("file.unit"),Desk,out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>SettingsFrom(CctvEmpty));
  KarineUI.CctvCameras(root,T("cctv.cameras"));
  var content=KarineUI.CctvMonitor(root);
  KarineUI.CompareEmpty(content,T("terminal.noRecords"));
 }
 // Masadaki tabletten açılırsa masaya, dosyadan (Dosyada Gezin) açılırsa dosyaya dönülür.
 bool cctvFromDesk;
 void CctvScreen(Node node,string focusEventId=null) {
  StopCctvVideo();
  Action leave=cctvFromDesk?(Action)Desk:FilePage;
  Back(()=>{StopCctvVideo();leave();});
  showingInterviewList=false;
  Desk();
  KarineUI.InboxScene(root);
  KarineUI.DossierBar(root,T(cctvFromDesk?"back.desk":"back.file"),T(game.Data.titleKey),T("file.unit"),()=>{StopCctvVideo();leave();},out var tools);
  KarineUI.DossierTool(tools,"gear",T("menu.row.settings"),()=>{StopCctvVideo();SettingsFrom(()=>CctvScreen(node,focusEventId));});
  var cameraList=KarineUI.CctvCameras(root,T("cctv.cameras"));
  foreach(var source in game.Data.nodes.Where(n=>(n.kind=="cctv"||n.kind=="bps")&&(n==node||game.Available(n)))) {
   var target=source;
   string sourceTitle=string.IsNullOrEmpty(source.cctvSourceKey)?T(source.titleKey):T(source.cctvSourceKey),place=null;
   int split=sourceTitle.IndexOf(" — ",StringComparison.Ordinal);if(split<0)split=sourceTitle.IndexOf(" · ",StringComparison.Ordinal);
   if(split>0){
    string a=sourceTitle.Substring(0,split),b=sourceTitle.Substring(split+3);
    bool aIsCamera=a.StartsWith("KAMERA",StringComparison.OrdinalIgnoreCase); // kamera adı başlık, yer alt satır
    sourceTitle=aIsCamera?a:b;place=aIsCamera?b:a;
   }
   var sourcePeriod=string.IsNullOrEmpty(source.cctvPeriodKey)?string.Empty:T(source.cctvPeriodKey);
   KarineUI.CctvCameraItem(cameraList,sourceTitle,place,sourcePeriod,source==node,!game.State.read.Contains(source.id),()=>{
    if(target.kind=="cctv")CctvScreen(target);else ReadPage(target);
   });
  }
  var content=KarineUI.CctvMonitor(root);
  var status=KarineUI.CctvMonitorHead(content,T(node.cctvSourceKey),string.IsNullOrEmpty(node.cctvPeriodKey)?null:T(node.cctvPeriodKey),T("cctv.signal"),T("cctv.overlay.rec"));
  var recordPanel=KarineUI.CctvRecordPanel(content);
  KarineUI.SignalSwitch(recordPanel);
  var stream=Scroll(recordPanel);
  var records=node.cctvEvents ?? new CctvEvent[0];
  var rows=new List<VisualElement>();
  var lines=new List<Label>();
  var actions=new List<VisualElement>();
  var stamps=new List<Label>();
  foreach(var record in records) {
   var row=KarineUI.CctvRecordRow(stream);rows.Add(row);
   var stamp=KarineUI.Technical(row,string.Empty,KarineTheme.CctvArchive.LineSize);stamp.style.width=KarineTheme.CctvArchive.TimeWidth-30;
   stamp.style.flexShrink=0;stamp.style.marginBottom=0;stamp.style.color=KarineTheme.Primary;stamps.Add(stamp);
   var dash=KarineUI.Technical(row,"--",KarineTheme.CctvArchive.LineSize);dash.style.width=40;dash.style.marginBottom=0;dash.style.color=KarineTheme.Primary;
   var label=KarineUI.Technical(row,string.Empty,KarineTheme.CctvArchive.LineSize);label.style.flexGrow=1;label.style.flexShrink=1;
   label.style.marginBottom=0;label.style.color=KarineTheme.Primary;lines.Add(label);
   // Sinyal satırları her vakada aynı kırmızıyla yazılır; içerik değil cihaz durumudur.
   bool alert=!string.IsNullOrEmpty(record.glitchKey);
   if(alert){KarineUI.CctvRowAlert(row);label.style.color=dash.style.color=stamp.style.color=KarineTheme.Danger;}
   else if(!string.IsNullOrEmpty(record.signalKey))label.style.color=KarineTheme.CctvArchive.Signal;
   var still=record.framePaths!=null && record.framePaths.Length>0?Resources.Load<Texture2D>(record.framePaths[0]):null;
   KarineUI.CctvThumb(row,still,record.HasFootage || alert);
   actions.Add(KarineUI.CctvActionSlot(row));
  }
  Action<int,string> write=(i,text)=>{
   string time=string.IsNullOrEmpty(records[i].overlayTimeKey)?null:T(records[i].overlayTimeKey),stamp,body;
   KarineUI.SplitCctvLine(text,time,out stamp,out body);stamps[i].text=stamp;lines[i].text=body;
  };
  Action<int> addFootageButton=index=>{
   var record=records[index];
   if(!record.HasFootage){if(actions[index].childCount==0 && string.IsNullOrEmpty(record.glitchKey) && string.IsNullOrEmpty(record.signalKey))KarineUI.Technical(actions[index],"---",KarineTheme.CctvArchive.LineSize).style.color=KarineTheme.Secondary;return;}
   KarineUI.CctvRowButton(actions[index],"nav_next",T("cctv.watch"),false,()=>OpenCctvVideo(node,record,content));
  };
  var footer=KarineUI.CctvFooter(content);
  Action finish=()=>KarineUI.CctvFooterDone(footer,T("cctv.complete"),T("cctv.total")+" "+records.Length+" "+T("cctv.records"));
  if(!string.IsNullOrEmpty(focusEventId) && game.State.read.Contains(node.id)) {
   finish();
   for(int i=0;i<records.Length;i++) {
    rows[i].style.display=DisplayStyle.Flex;
    write(i,T(records[i].textKey));
    addFootageButton(i);
   }
   int focus=Array.FindIndex(records,e=>e.id==focusEventId);
   if(focus>=0)content.schedule.Execute(()=>stream.ScrollTo(rows[focus]));
   return;
  }
  bool reviewing=false;
  var review=KarineUI.CctvRowButton(footer,"search",T("cctv.review"),true,()=>{
   if(reviewing)return;
   reviewing=true;
   KarineUI.CctvFooterText(footer,T("cctv.scanning"));
   int index=0;
   Action next=null;
   next=()=>{
    if(index>=records.Length) {
     finish();
     game.Read(node.id);Save();
     return;
    }
    int current=index++;
    var record=records[current];
    int delay=record.delayMs>0?record.delayMs:650;
    content.schedule.Execute(()=>{
     if(!string.IsNullOrEmpty(record.signalKey))status.text=T(record.signalKey).TrimStart('●',' ').ToUpper(KarineUI.TextCulture);
     rows[current].style.display=DisplayStyle.Flex;
     var line=lines[current];
     string finalText=T(string.IsNullOrEmpty(record.glitchKey)?record.textKey:record.glitchKey);
     stamps[current].text="▒▒▒";line.text=T("cctv.syncing");
     // Sinyal satırı: görüntü karlanır, cızırtı duyulur, saat bir an karışır.
     // Bütün sinyal satırlarında aynı; satırın kendisi zaten cihaz durumudur.
     bool signal=!string.IsNullOrEmpty(record.signalKey);
     if(signal){KarineUI.Snow(recordPanel,KarineTheme.Film.SnowSeconds);audioDirector?.Play("ui_static",1f,.5f);}
     content.schedule.Execute(()=>{
      write(current,finalText);
      if(signal)KarineUI.TimecodeSkip(stamps[current],stamps[current].text);
      stream.ScrollTo(rows[current]);
      addFootageButton(current);
      if(!string.IsNullOrEmpty(record.glitchKey)) {
       Button clarify=null;
       clarify=KarineUI.CctvRowButton(actions[current],"gear",T("cctv.clarifyShort"),true,()=>{
        clarify.RemoveFromHierarchy();
        line.text=T("cctv.syncing");
        content.schedule.Execute(()=>write(current,T(record.textKey))).ExecuteLater(360);
       });
       clarify.tooltip=T("cctv.clarify");
      }
      next();
     }).ExecuteLater(110+current%3*70);
    }).ExecuteLater(delay);
   };
   next();
  });
  review.style.alignSelf=Align.Center;review.style.flexGrow=1;
 }
}
}
