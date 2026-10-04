// Galeri: her vakanın sorgu ekranlarını KARINE_GALLERY klasörüne yazar (Tools/capture-screens.sh).
// Değişken yoksa atlanır; normal test koşusunu etkilemez.
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
namespace Bube.Tests {
public sealed class GalleryTests {
 const BindingFlags Any=BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public;
 BubeApp app; RenderTexture rt;
 static string Folder=>Environment.GetEnvironmentVariable("KARINE_GALLERY");
 object F(string n)=>typeof(BubeApp).GetField(n,Any).GetValue(app);
 void Call(string n,params object[] a){var m=typeof(BubeApp).GetMethods(Any).First(x=>x.Name==n&&x.GetParameters().Length>=a.Length);
  var full=m.GetParameters().Select((p,i)=>i<a.Length?a[i]:(p.HasDefaultValue?p.DefaultValue:null)).ToArray();m.Invoke(app,full);}
 IEnumerator Shot(string name,Action act){
  try{act();}catch(Exception e){File.WriteAllText(Path.Combine(Folder,name+".err.txt"),e.ToString());yield break;}
  float until=Time.realtimeSinceStartup+1.2f;while(Time.realtimeSinceStartup<until)yield return null;
  var prev=RenderTexture.active;RenderTexture.active=rt;
  var t=new Texture2D(rt.width,rt.height,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);t.Apply();RenderTexture.active=prev;
  File.WriteAllBytes(Path.Combine(Folder,name+".png"),t.EncodeToPNG());UnityEngine.Object.Destroy(t);
 }
 [UnityTest] public IEnumerator Capture(){
  if(string.IsNullOrEmpty(Folder))Assert.Ignore("KARINE_GALLERY tanımlı değil");
  Directory.CreateDirectory(Folder);
  SceneManager.LoadScene("BootScene",LoadSceneMode.Single);
  float until=Time.realtimeSinceStartup+3f;while(Time.realtimeSinceStartup<until)yield return null;
  app=UnityEngine.Object.FindFirstObjectByType<BubeApp>();
  int w=int.Parse(Environment.GetEnvironmentVariable("KARINE_W")??"2400"),h=int.Parse(Environment.GetEnvironmentVariable("KARINE_H")??"1080");
  rt=new RenderTexture(w,h,24,RenderTextureFormat.ARGB32);rt.Create();
  foreach(var d in UnityEngine.Object.FindObjectsByType<UnityEngine.UIElements.UIDocument>(FindObjectsSortMode.None)){var ps=UnityEngine.Object.Instantiate(d.panelSettings);ps.targetTexture=rt;ps.clearColor=true;d.panelSettings=ps;}
  var cfg=(GameConfig)F("config");var loc=(Locale)F("locale");var rules=(CareerRules)F("careerRules");

  foreach(var c in new[]{"case001","case002","case003"}){
   var data=JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/"+c).text);
   var game=new Investigation(data,null,null,rules){Text=loc};game.Career.activeCaseId=c;game.AcceptCase();
   typeof(BubeApp).GetField("game",Any).SetValue(app,game);
   foreach(var n in data.nodes.Where(n=>n.kind=="interview"&&!string.IsNullOrEmpty(n.personId)).GroupBy(n=>n.personId).Select(g=>g.First())){
    var node=n;game.State.read.Add(node.id);game.RequestInterview(node.id,0);
    yield return Shot(c+"_"+node.personId+"_"+w+"x"+h,()=>Call("InterviewPage",node));
   }
  }
 }
}
}
