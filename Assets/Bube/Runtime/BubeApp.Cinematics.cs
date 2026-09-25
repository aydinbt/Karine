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
// Dünya girişi ve masaya varış filmleri; GEÇ düğmesinin yerleşimi.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 void MaybeWorldIntro(Action after) {
  var world=(config.worldIntros ?? new WorldIntro[0]).FirstOrDefault(w=>w.firstCaseId==game.Data.id);
  if(world==null || game.Career.seenWorldIntros.Contains(world.id)){after();return;}
  activeIntro=world;
  introAfter=after;
  PlayWorldIntro();
 }
 void PlayWorldIntro() {
  EnsureScene("OfficeScene");
  root.Clear();
  root.style.backgroundColor=Color.black;
  introTexture=new RenderTexture(1280,720,0,RenderTextureFormat.ARGB32);
  introTexture.Create();
  var film=new Image {image=introTexture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  film.style.position=Position.Absolute;
  film.style.left=0;film.style.right=0;film.style.top=0;film.style.bottom=0;
  root.Add(film);
  if(!activeIntro.graphicsEmbedded) {
  introPlace=new VisualElement();
  introPlace.style.position=Position.Absolute;
  introPlace.style.left=Length.Percent(5);introPlace.style.top=Length.Percent(13);
  introPlace.style.flexDirection=FlexDirection.Row;introPlace.style.alignItems=Align.Center;
  introPlace.style.paddingLeft=12;introPlace.style.paddingRight=16;
  introPlace.style.paddingTop=9;introPlace.style.paddingBottom=7;
  introPlace.style.backgroundColor=KarineTheme.Veil(.70f);
  introPlace.style.opacity=0;root.Add(introPlace);
  var flag=string.IsNullOrEmpty(activeIntro.flagResource)?null:Resources.Load<Texture2D>(activeIntro.flagResource);
  if(flag!=null) {
   flag.filterMode=FilterMode.Point;
   var emblem=new Image {image=flag,scaleMode=ScaleMode.ScaleToFit,pickingMode=PickingMode.Ignore};
   emblem.style.width=54;emblem.style.height=36;emblem.style.marginRight=12;
   introPlace.Add(emblem);
  }
  var location=new VisualElement();introPlace.Add(location);
  var country=Text(location,T(activeIntro.countryKey),Ink,18);country.style.marginBottom=0;
  if(dossierBoldFont!=null)country.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  if(!string.IsNullOrEmpty(activeIntro.locationKey)) {
   var city=Text(location,T(activeIntro.locationKey),Muted,12);city.style.marginBottom=0;
  }
  introBrand=new VisualElement();
  introBrand.style.position=Position.Absolute;
  introBrand.style.right=Length.Percent(6);introBrand.style.top=Length.Percent(42);
  introBrand.style.paddingLeft=22;introBrand.style.paddingRight=22;
  introBrand.style.paddingTop=14;introBrand.style.paddingBottom=12;
  introBrand.style.backgroundColor=KarineTheme.Veil(.64f);
  introBrand.style.opacity=0;root.Add(introBrand);
  var studio=Text(introBrand,"bubeGames",Ink,31);studio.style.marginBottom=1;
  if(dossierBoldFont!=null)studio.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  var credit=Text(introBrand,"powered by bubeDigital",Gold,16);credit.style.marginBottom=0;
  }
  // Sinematikte tek denetim GEÇ'tir: duraklatma, ilerleme çubuğu ve
  // hızlandırma yok. Bazı filmlerde düğme üretici filigranının üstüne
  // oturmak zorunda; o zaman yeri kareye göre hesaplanır.
  var skip=KarineUI.SkipButton(root,T("intro.skip"),FinishWorldIntro);
  skip.style.position=Position.Absolute;
  introSkip=skip;
  if(activeIntro.skipCoversCornerMark) {
   activeMark=activeIntro.skipMark ?? new CornerMark();
   root.RegisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
   skip.schedule.Execute(PositionIntroSkip).StartingIn(0);
  } else PlaceSkipInCorner(skip);
  introPlayer=gameObject.AddComponent<VideoPlayer>();
  introPlayer.playOnAwake=false;
  introPlayer.isLooping=false;
  introPlayer.renderMode=VideoRenderMode.RenderTexture;
  introPlayer.targetTexture=introTexture;
  introPlayer.audioOutputMode=VideoAudioOutputMode.Direct;
  introPlayer.source=VideoSource.Url;
  introPlayer.url=Application.streamingAssetsPath+"/"+activeIntro.videoPath;
  introPlayer.prepareCompleted+=OnIntroPrepared;
  introPlayer.loopPointReached+=OnIntroEnded;
  introPlayer.errorReceived+=OnIntroError;
  introPlayer.Prepare();
 }
 void OnIntroGeometryChanged(GeometryChangedEvent evt) { PositionIntroSkip(); }
 // Filigran örtme derdi olmayan filmlerde GEÇ sağ altta, güvenli alanın içinde.
 void PlaceSkipInCorner(Button skip) {
  skip.style.right=Length.Percent(4);
  skip.style.bottom=Length.Percent(6);
  skip.style.left=StyleKeyword.Auto;
  skip.style.top=StyleKeyword.Auto;
 }
 // "Gec" dugmesi uretici filigraninin tam ustune oturur: filigran filmin kendi
 // karesine oranli oldugu icin once filmin ekrandaki gercek dikdortgeni bulunur.
 // Film 16:9 olarak taranip kirpildigindan telefonun eni ne olursa olsun dogru yere gelir.
 void PositionIntroSkip() {
  if(introSkip==null || root==null || activeMark==null)return;
  float width=root.resolvedStyle.width, height=root.resolvedStyle.height;
  if(float.IsNaN(width) || float.IsNaN(height) || width<=0 || height<=0)return;
  float scale=Mathf.Max(width/16f,height/9f);
  float filmWidth=16f*scale, filmHeight=9f*scale;
  float offsetX=(width-filmWidth)*.5f;
  float offsetY=(height-filmHeight)*.5f;
  // Kutu filigranı örtecek kadar büyük olmalı ama kit düğmesi gibi durmalı:
  // yüksekliği rahat dokunma hedefinde tutulur, eni yazıya göre taşmaz.
  float buttonWidth=Mathf.Clamp(activeMark.w*filmWidth,150f,260f);
  float buttonHeight=Mathf.Clamp(activeMark.h*filmHeight,KarineTheme.TouchTargetComfortable,72f);
  float centerX=offsetX+activeMark.x*filmWidth;
  float centerY=offsetY+activeMark.y*filmHeight;
  introSkip.style.width=buttonWidth;
  introSkip.style.height=buttonHeight;
  introSkip.style.left=Mathf.Clamp(centerX-buttonWidth*.5f,0f,Mathf.Max(0f,width-buttonWidth));
  introSkip.style.top=Mathf.Clamp(centerY-buttonHeight*.5f,0f,Mathf.Max(0f,height-buttonHeight));
  introSkip.style.right=StyleKeyword.Auto;
  introSkip.style.bottom=StyleKeyword.Auto;
 }
 void OnIntroPrepared(VideoPlayer player) {
  if(player.audioTrackCount>0) {
   player.EnableAudioTrack(0,true);
   player.SetDirectAudioMute(0,false);
   player.SetDirectAudioVolume(0,1f);
  }
  player.Play();
 }
 void OnIntroEnded(VideoPlayer player) { FinishWorldIntro(); }
 void OnIntroError(VideoPlayer player,string message) {
  Debug.LogWarning("World intro video unavailable: "+message);
  FinishWorldIntro();
 }
 void FinishWorldIntro() {
  if(activeIntro==null)return;
  var world=activeIntro;
  var after=introAfter;
  if(introSkip!=null)root.UnregisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
  introSkip=null;
  activeIntro=null;introAfter=null;introBrand=null;introPlace=null;activeMark=null;
  if(introPlayer!=null) {
   introPlayer.prepareCompleted-=OnIntroPrepared;
   introPlayer.loopPointReached-=OnIntroEnded;
   introPlayer.errorReceived-=OnIntroError;
   introPlayer.Stop();Destroy(introPlayer);introPlayer=null;
  }
  if(introTexture!=null){introTexture.Release();Destroy(introTexture);introTexture=null;}
  if(!game.Career.seenWorldIntros.Contains(world.id))game.Career.seenWorldIntros.Add(world.id);
  Save();
  if(!world.deskArrival){after();return;}
  if(!string.IsNullOrEmpty(world.deskArrivalVideo))PlayDeskArrival(world,after);
  else StartCoroutine(FirstDeskArrival(after));
 }
 // Dosyanin masaya birakilisi artik cizilmis bir animasyon degil, sinematik bir
 // video. Video yoksa ya da oynatilamazsa asagidaki elle cizilmis animasyon
 // devreye girer; oyun hicbir kosulda bu andan yoksun kalmaz.
 void PlayDeskArrival(WorldIntro world,Action after) {
  EnsureScene("OfficeScene");
  root.Clear();
  root.style.backgroundColor=Color.black;
  deskArrivalDone=false;
  introTexture=new RenderTexture(1920,1080,0,RenderTextureFormat.ARGB32);
  introTexture.Create();
  var film=new Image { image=introTexture, scaleMode=ScaleMode.ScaleAndCrop, pickingMode=PickingMode.Ignore };
  film.style.position=Position.Absolute;
  film.style.left=0;film.style.right=0;film.style.top=0;film.style.bottom=0;
  root.Add(film);
  // Burada da tek denetim GEÇ; filigranın üstüne oturur.
  var skip=KarineUI.SkipButton(root,T("intro.skip"),()=>FinishDeskArrival(after,false));
  skip.style.position=Position.Absolute;
  introSkip=skip;
  activeMark=world.deskArrivalMark ?? new CornerMark();
  root.RegisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
  skip.schedule.Execute(PositionIntroSkip).StartingIn(0);
  introPlayer=gameObject.AddComponent<VideoPlayer>();
  introPlayer.playOnAwake=false;
  introPlayer.isLooping=false;
  introPlayer.renderMode=VideoRenderMode.RenderTexture;
  introPlayer.targetTexture=introTexture;
  introPlayer.audioOutputMode=VideoAudioOutputMode.Direct;
  introPlayer.source=VideoSource.Url;
  introPlayer.url=Application.streamingAssetsPath+"/"+world.deskArrivalVideo;
  introPlayer.prepareCompleted+=OnIntroPrepared;
  introPlayer.loopPointReached+=_=>FinishDeskArrival(after,false);
  introPlayer.errorReceived+=(_,message)=>{
   Debug.LogWarning("Desk arrival video unavailable: "+message);
   FinishDeskArrival(after,true);
  };
  introPlayer.Prepare();
 }
 // Hem "Gec" dugmesi hem videonun bitisi buraya gelir; bayrak ikinci cagriyi yutar.
 void FinishDeskArrival(Action after,bool fallback) {
  if(deskArrivalDone)return;
  deskArrivalDone=true;
  if(introSkip!=null)root.UnregisterCallback<GeometryChangedEvent>(OnIntroGeometryChanged);
  introSkip=null;activeMark=null;
  if(introPlayer!=null){introPlayer.Stop();Destroy(introPlayer);introPlayer=null;}
  if(introTexture!=null){introTexture.Release();Destroy(introTexture);introTexture=null;}
  if(fallback)StartCoroutine(FirstDeskArrival(after));
  else StartCoroutine(OpenEyes(after));
 }
 // Masa siyahtan yavasca acilir. Kaplayan golge acilma boyunca dokunmalari da
 // tutar, boylece oyuncu goremedigi bir seye basamaz.
 IEnumerator OpenEyes(Action after) {
  after();
  var shade=new VisualElement();
  shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=Color.black;shade.style.opacity=1;
  root.Add(shade);
  float elapsed=0;
  while(elapsed<1.25f) {
   elapsed+=Time.unscaledDeltaTime;
   shade.style.opacity=1-Mathf.SmoothStep(0,1,Mathf.Clamp01(elapsed/1.25f));
   yield return null;
  }
  shade.RemoveFromHierarchy();
 }
 IEnumerator FirstDeskArrival(Action after) {
  // Masanin kendisi arka plandir: ayri bir tam ekran gorsel cizilmez, boylece
  // `Desk()` ust seridi de dahil her sey yerli yerinde kalir. Kaplayan golge
  // animasyon boyunca tiklamalari da tutar.
  Desk();
  var shade=new VisualElement();shade.style.position=Position.Absolute;
  shade.style.left=0;shade.style.right=0;shade.style.top=0;shade.style.bottom=0;
  shade.style.backgroundColor=Color.black;shade.style.opacity=1;root.Add(shade);
  var folder=new VisualElement();folder.style.position=Position.Absolute;
  folder.style.left=Length.Percent(32);folder.style.width=Length.Percent(36);
  folder.style.height=Length.Percent(29);folder.style.top=Length.Percent(-35);
  folder.style.backgroundColor=KarineTheme.Paper.Tint;
  folder.style.borderBottomWidth=7;folder.style.borderBottomColor=KarineTheme.Paper.FolderEdge;
  folder.style.paddingLeft=24;folder.style.paddingTop=20;root.Add(folder);
  Text(folder,T("intro.firstFile"),Base,17);
  var label=Text(folder,T(game.Data.titleKey),Base,24);
  if(dossierBoldFont!=null)label.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  float elapsed=0;
  while(elapsed<2.1f) {
   elapsed+=Time.unscaledDeltaTime;
   float reveal=Mathf.SmoothStep(0,1,Mathf.Clamp01(elapsed/.65f));
   shade.style.opacity=1-reveal*.62f;
   float slide=Mathf.SmoothStep(0,1,Mathf.Clamp01((elapsed-.45f)/1.2f));
   folder.style.top=Length.Percent(Mathf.Lerp(-35,48,slide));
   yield return null;
  }
  yield return new WaitForSecondsRealtime(.65f);
  after();
 }
}
}
