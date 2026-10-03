using System.Collections;
using System.Linq;
using UnityEngine;

namespace Bube {
// Açılış yüklemesi: ana menünün müziği, ortam sesleri ve dünya görselleri
// arka planda yüklenir; çubuk bu gerçek adımları sayar. İpucu okunabilsin diye
// katman en az `Loading.MinSeconds` kalır. İpuçları yalnız oynanış kuralını
// ve ayarları anlatır, hiçbir vakanın çözümüne değinmez.
public sealed partial class BubeApp {
 const int LoadingTips=6;

 void ShowLoading() {
  var tips=Enumerable.Range(0,LoadingTips).Select(i=>T("loading.tip."+i)).ToArray();
  System.Action<float> progress;System.Action finish;
  KarineUI.LoadingScreen(root,T("loading.label"),tips,out progress,out finish);
  StartCoroutine(Preload(progress,finish));
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
