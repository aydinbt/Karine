using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace Bube {
// Açılış yüklemesi: ana menünün müziği, ortam sesleri ve dünya görselleri
// arka planda yüklenir; çubuk bu gerçek adımları sayar. İpucu okunabilsin diye
// katman en az `Loading.MinSeconds` kalır. İpuçları yalnız oynanış kuralını
// ve ayarları anlatır, hiçbir vakanın çözümüne değinmez.
public sealed partial class BubeApp {
 const int LoadingTips=6,LoadingTerms=14;

 void ShowLoading() {
  var tips=Enumerable.Range(0,LoadingTips).Select(i=>T("loading.tip."+i)).ToArray();
  System.Action<float> progress;System.Action finish;
  var layer=KarineUI.LoadingScreen(root,T("loading.label"),tips,out progress,out finish);
  SplashVideo(layer);var done=finish;
  StartCoroutine(Preload(progress,()=>{done();StartCoroutine(StopSplashAfter(KarineTheme.Loading.FadeSeconds));}));
 }

 // Açılış arka planı (9 Ekim 2026): yağmurlu, adsız bir şehirde lamba altında Bora;
 // Gemini, 10 sn döngü, sessiz. Hiçbir ülkeyi göstermez, on bölümün hepsine yakışır.
 // Video hazır olunca belirir; açılamazsa düz koyu zemin kalır. Yalnız açılışta oynar.
 const string SplashVideoPath="Bube/splash_loop.mp4";
 VideoPlayer splashPlayer;RenderTexture splashTexture;
 void SplashVideo(VisualElement layer) {
  if(layer==null)return;
  splashTexture=new RenderTexture(1280,720,0);splashTexture.Create();
  var image=new Image {image=splashTexture,scaleMode=ScaleMode.ScaleAndCrop,pickingMode=PickingMode.Ignore};
  image.style.position=Position.Absolute;image.style.left=image.style.top=image.style.right=image.style.bottom=0;image.style.opacity=0;
  layer.Insert(0,image);
  var veil=new VisualElement {pickingMode=PickingMode.Ignore};veil.style.position=Position.Absolute;veil.style.left=veil.style.top=veil.style.right=veil.style.bottom=0;
  veil.style.backgroundColor=KarineTheme.Veil(KarineTheme.Loading.VideoVeil);veil.style.opacity=0;layer.Insert(1,veil);
  splashPlayer=gameObject.AddComponent<VideoPlayer>();
  splashPlayer.playOnAwake=false;splashPlayer.isLooping=true;splashPlayer.renderMode=VideoRenderMode.RenderTexture;splashPlayer.targetTexture=splashTexture;
  splashPlayer.audioOutputMode=VideoAudioOutputMode.None;splashPlayer.source=VideoSource.Url;splashPlayer.url=Application.streamingAssetsPath+"/"+SplashVideoPath;
  splashPlayer.prepareCompleted+=p=>{p.Play();KarineMotion.Run(image,.8f,t=>{image.style.opacity=t;veil.style.opacity=t;});};
  splashPlayer.errorReceived+=(p,message)=>{Debug.LogWarning("Splash video unavailable: "+message);StopSplash();};
  splashPlayer.Prepare();
 }
 IEnumerator StopSplashAfter(float seconds){yield return new WaitForSecondsRealtime(seconds+.1f);StopSplash();}
 void StopSplash() {
  if(splashPlayer!=null){splashPlayer.Stop();Destroy(splashPlayer);splashPlayer=null;}
  if(splashTexture!=null){splashTexture.Release();Destroy(splashTexture);splashTexture=null;}
 }

 // Oyun içi geçiş yüklemesi: hedef ekran altta kurulur, üstünde katman
 // terim/hukuk ipuçlarıyla `Loading.TransitSeconds` kalır ve söner. Masaya
 // girişte, yeni vaka kabulünde ve görüşme odasına girerken çıkar.
 void LoadThen(System.Action next) {
  next();
  var tips=Enumerable.Range(0,LoadingTerms).Select(i=>T("loading.term."+i)).Concat(Enumerable.Range(0,LoadingTips).Select(i=>T("loading.tip."+i))).ToArray();
  System.Action<float> progress;System.Action finish;
  KarineUI.LoadingScreen(root,T("loading.label"),tips,out progress,out finish,KarineTheme.Loading.TransitTipSeconds);
  StartCoroutine(Transit(progress,finish));
 }
 IEnumerator Transit(System.Action<float> progress,System.Action finish) {
  float started=Time.realtimeSinceStartup,length=KarineTheme.Loading.TransitSeconds;
  while(Time.realtimeSinceStartup-started<length){progress((Time.realtimeSinceStartup-started)/length);yield return null;}
  progress(1);yield return new WaitForSecondsRealtime(.25f);finish();
 }

 IEnumerator Preload(System.Action<float> progress,System.Action finish) {
  float started=Time.realtimeSinceStartup;
  atlas=atlas??Worlds.Load();
  var paths=new[]{"Bube/Audio/menu_theme","Bube/Audio/desk_theme","Bube/Audio/room_rain","Bube/Audio/amb_thunder","Bube/Audio/amb_car","Bube/Audio/case_sting"}
   .Concat(atlas.countries.Where(c=>!string.IsNullOrEmpty(c.image)).Select(c=>c.image)).ToArray();
  for(int i=0;i<paths.Length;i++) {
   var request=Resources.LoadAsync(paths[i]);
   while(!request.isDone){progress((i+request.progress)/paths.Length);yield return null;}
   progress((i+1f)/paths.Length);
  }
  while(Time.realtimeSinceStartup-started<KarineTheme.Loading.MinSeconds)yield return null;
  finish();
 }
}
}
