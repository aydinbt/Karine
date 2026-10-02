using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Bube.Tests {

// Vaka teklifi artık ayrı bir tam ekran değil: dosya masadaki gelen evrak
// tepsisinde durur, rozet yanıp söner, oyuncu tepsiyi kendisi açar ve önizlemeyi
// okuyup kabul eder. Bu test o akışı gerçek arayüz üzerinde koşturur.
//
// Yansıma kullanılıyor çünkü `BubeApp` tek parça ve ekran yöntemleri özel.
// Faz 4'te sınıf bölündüğünde bu testin de sadeleşmesi beklenir.
public sealed class CaseOfferFlowTests {

 BubeApp app;
 object game;

 static object Field(object target, string name) =>
  target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
   .GetValue(target);

 // `InboxPage`'in iki aşırı yüklemesi var; parametresiz olanı seçiyoruz.
 static void Call(object target, string name) =>
  target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
   .First(m => m.Name == name && m.GetParameters().Length == 0)
   .Invoke(target, null);

 VisualElement Root => (VisualElement)Field(app, "root");

 static string[] Tooltips(VisualElement root) =>
  root.Query<Button>().ToList().Select(b => b.tooltip).Where(t => !string.IsNullOrEmpty(t)).ToArray();

 static Button ButtonWithText(VisualElement root, string text) =>
  root.Query<Button>().ToList().FirstOrDefault(b => b.text != null && b.text.Contains(text));

 [UnitySetUp] public IEnumerator Setup() {
  SceneManager.LoadScene("BootScene", LoadSceneMode.Single);
  yield return null;
  for (int frame = 0; frame < 20; frame++) yield return null;
  app = Object.FindFirstObjectByType<BubeApp>();
  Assert.IsNotNull(app, "BubeApp açılmadı.");
  SaveSandbox.ResetToInitialCase(app);
  game = Field(app, "game");
  Assert.IsNotNull(game, "Investigation kurulmadı.");
 }

 Progress State => (Progress)game.GetType().GetProperty("State").GetValue(game);

 [UnityTest] public IEnumerator UnacceptedCase_LeavesOnlyTheInboxOpenOnTheDesk() {
  State.caseAccepted = false;
  Call(app, "Desk");
  yield return null;

  var tooltips = Tooltips(Root);
  var inbox = (string)app.GetType()
   .GetMethod("T", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(app, new object[] { "desk.inbox" });
  Assert.Contains(inbox, tooltips, "Gelen evrak tepsisi açık olmalı.");
  // Dosya, görüşme ve terminal kabul edilmeden erişilemez: oyuncu önce evrağı fark eder.
  Assert.AreEqual(2, tooltips.Length,
   "Kabul edilmeden yalnız tepsi ve ana ekran erişilebilir olmalı, bulunan: " + string.Join(" | ", tooltips));
 }

 [UnityTest] public IEnumerator Inbox_ShowsThePreviewAndAcceptingOpensTheDesk() {
  State.caseAccepted = false;
  Call(app, "InboxPage");
  yield return null;

  var accept = ButtonWithText(Root, "Dosyayı kabul et");
  Assert.IsNotNull(accept, "Tepside kabul düğmesi yok.");
  Assert.IsTrue(Root.Query<Label>().ToList().Any(l => l.text != null && l.text.Contains("KONUT HIRSIZLIĞI")),
   "Dosya önizlemesi tepside okunmuyor.");

  using (var click = new NavigationSubmitEvent()) { click.target = accept; accept.SendEvent(click); }
  yield return null;
  yield return null;

  Assert.IsTrue(State.caseAccepted, "Kabul düğmesi dosyayı kabul etmedi.");
  var tooltips = Tooltips(Root);
  Assert.Greater(tooltips.Length, 2, "Kabulden sonra masa tamamen açılmalı: " + string.Join(" | ", tooltips));
 }

 [UnityTest] public IEnumerator Desk_UsesSeparatePropsAndCountryViewWithoutGuidance() {
  State.caseAccepted=true;Call(app,"Desk");
  for(int frame=0;frame<8;frame++)yield return null;
  Assert.IsNotNull(Root.Q("OfficeWindow-tr"));
  foreach(var name in new[]{"BlankCaseFolder","CctvMonitor","InboxTray","DeskPhone","EvidencePile"})
   Assert.IsNotNull(Root.Q(name),"Prop marker missing: "+name);
  foreach(var name in new[]{"DeskInbox","DeskFile","DeskInterviews","DeskTerminal","DeskEvidence"}) {
   var button=Root.Q<Button>(name);Assert.IsNotNull(button);
   Assert.GreaterOrEqual(button.layout.height,KarineTheme.TouchTarget);
   Assert.IsTrue(Root.Q("OfficeStage").worldBound.Overlaps(button.worldBound));
  }
  var labels=Root.Query<Label>().ToList().Select(l=>l.text).ToArray();
  Assert.IsFalse(labels.Contains("GÖREV"));Assert.IsFalse(labels.Contains("NOTLAR"));
  // Optional rendered evidence from a graphics-enabled isolated test run.
  if(!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("KARINE_DOSSIER_CAPTURE"))) {
   Call(app,"FilePage");for(int frame=0;frame<8;frame++)yield return null;
   Assert.IsNotNull(Root.Q("DossierPaper"));Assert.IsNotNull(Root.Q("DossierOverview"));
  }
  if(!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("KARINE_INBOX_CAPTURE"))) {
   State.caseAccepted=false;Call(app,"InboxPage");for(int frame=0;frame<8;frame++)yield return null;
   Assert.IsNotNull(Root.Q("InboxPaper"));Assert.IsNotNull(Root.Q("InboxListPanel"));
  }
  if(!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("KARINE_REQUEST_CAPTURE"))) {
   State.caseAccepted=true;State.read.Add("report");
   app.GetType().GetMethod("InterviewRequests",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(app,new object[]{false});
   for(int frame=0;frame<8;frame++)yield return null;
   Assert.IsNotNull(Root.Q("RequestDetail"));
  }
  if(!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("KARINE_CCTV_CAPTURE"))) {
   var data=(CaseData)game.GetType().GetProperty("Data").GetValue(game);
   var camera=data.nodes.First(n=>n.kind=="cctv");
   State.caseAccepted=true;State.read.Add("report");State.read.Add(camera.id);
   app.GetType().GetMethod("CctvScreen",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(app,new object[]{camera,camera.cctvEvents[0].id});
   yield return new WaitForSecondsRealtime(1f);
   Assert.IsNotNull(Root.Q("CctvArchiveRecords"));
  }
  if(!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("KARINE_WORLD_CAPTURE"))) {
   Call(app,"WorldPage");yield return new WaitForSecondsRealtime(1f);
   Assert.IsNotNull(Root.Q("CaseBrowser"));
  }
  if(!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("KARINE_SETTINGS_CAPTURE"))) {
   Call(app,"SettingsPage");yield return new WaitForSecondsRealtime(1f);
   Assert.IsNotNull(Root.Q("SettingsPanel"));
  }
  var capture=System.Environment.GetEnvironmentVariable("KARINE_SETTINGS_CAPTURE")??System.Environment.GetEnvironmentVariable("KARINE_WORLD_CAPTURE")??System.Environment.GetEnvironmentVariable("KARINE_CCTV_CAPTURE")??System.Environment.GetEnvironmentVariable("KARINE_REQUEST_CAPTURE")??System.Environment.GetEnvironmentVariable("KARINE_INBOX_CAPTURE")??System.Environment.GetEnvironmentVariable("KARINE_DOSSIER_CAPTURE")??System.Environment.GetEnvironmentVariable("KARINE_DESK_CAPTURE");
  if(!string.IsNullOrEmpty(capture)) {
   var settings=app.GetComponent<UIDocument>().panelSettings;
   var previous=settings.targetTexture;
   var target=new RenderTexture(1280,720,0);target.Create();settings.targetTexture=target;
   for(int frame=0;frame<8;frame++)yield return null;
   var previousActive=RenderTexture.active;RenderTexture.active=target;
   var pixels=new Texture2D(1280,720,TextureFormat.RGB24,false);
   pixels.ReadPixels(new Rect(0,0,1280,720),0,0);pixels.Apply();
   var colors=pixels.GetPixels32();
   using(var stream=new System.IO.BinaryWriter(System.IO.File.Create(capture))) {
    stream.Write(System.Text.Encoding.ASCII.GetBytes("P6\n1280 720\n255\n"));
    for(int y=719;y>=0;y--)for(int x=0;x<1280;x++) {
     var c=colors[y*1280+x];stream.Write(c.r);stream.Write(c.g);stream.Write(c.b);
    }
   }
   RenderTexture.active=previousActive;settings.targetTexture=previous;
   target.Release();Object.Destroy(target);Object.Destroy(pixels);
  }
 }

 [UnityTest] public IEnumerator Motion_CompletesUnscaledAndCancelsDetachedCallbacks() {
  int previous=PlayerPrefs.GetInt("karine.reducedMotion",0);
  float oldScale=Time.timeScale;
  try {
   PlayerPrefs.SetInt("karine.reducedMotion",0);Time.timeScale=0;
   var element=new VisualElement();Root.Add(element);
   bool complete=false;float progress=0;
   KarineMotion.Run(element,.05f,t=>progress=t,()=>complete=true);
   yield return new WaitForSecondsRealtime(.2f);
   Assert.IsTrue(complete);Assert.AreEqual(1f,progress);
   bool stale=false;
   KarineMotion.Run(element,.1f,t=>{},()=>stale=true);
   element.RemoveFromHierarchy();yield return new WaitForSecondsRealtime(.2f);
   Assert.IsFalse(stale,"Kapatılan ekranın eski geçişi yeni ekran açmamalı.");
   PlayerPrefs.SetInt("karine.reducedMotion",1);
   bool immediate=false;KarineMotion.Run(Root,1,t=>progress=t,()=>immediate=true);
   Assert.IsTrue(immediate);Assert.AreEqual(1f,progress);
  } finally {Time.timeScale=oldScale;PlayerPrefs.SetInt("karine.reducedMotion",previous);}
 }

 [UnityTest] public IEnumerator InboxArrival_PlaysOnceWhenReturningToDesk() {
  int previous=PlayerPrefs.GetInt("karine.reducedMotion",0);
  try {
   PlayerPrefs.SetInt("karine.reducedMotion",0);State.caseAccepted=false;
   ((System.Collections.Generic.HashSet<string>)Field(app,"presentedArrivals")).Clear();
   Call(app,"Desk");Call(app,"PresentInboxArrivals");
   Assert.IsNotNull(Root.Q("IncomingPaper"));
   yield return new WaitForSecondsRealtime(1f);
   Assert.IsNull(Root.Q("IncomingPaper"));
   Call(app,"Desk");Call(app,"PresentInboxArrivals");
   Assert.IsNull(Root.Q("IncomingPaper"),"Aynı evrak masaya dönünce tekrar gelmemeli.");
  } finally {PlayerPrefs.SetInt("karine.reducedMotion",previous);}
 }

 // Kaldırılan tam ekran teklifin geri gelmediğini sabitler.
 [Test] public void CaseOfferScreen_NoLongerExists() {
  Assert.IsNull(typeof(BubeApp).GetMethod("CaseOffer", BindingFlags.Instance | BindingFlags.NonPublic),
   "CaseOffer() geri gelmiş; teklif masadaki tepside durmalı.");
 }
}
}
