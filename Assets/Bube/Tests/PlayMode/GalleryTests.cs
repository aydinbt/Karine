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
 [UnityTest,Timeout(1800000)] public IEnumerator Capture(){
  if(string.IsNullOrEmpty(Folder))Assert.Ignore("KARINE_GALLERY tanımlı değil");
  Directory.CreateDirectory(Folder);
  SceneManager.LoadScene("BootScene",LoadSceneMode.Single);
  float until=Time.realtimeSinceStartup+3f;while(Time.realtimeSinceStartup<until)yield return null;
  app=UnityEngine.Object.FindFirstObjectByType<BubeApp>();
  int w=int.Parse(Environment.GetEnvironmentVariable("KARINE_W")??"2400"),h=int.Parse(Environment.GetEnvironmentVariable("KARINE_H")??"1080");
  rt=new RenderTexture(w,h,24,RenderTextureFormat.ARGB32);rt.Create();
  foreach(var d in UnityEngine.Object.FindObjectsByType<UnityEngine.UIElements.UIDocument>(FindObjectsSortMode.None)){var ps=UnityEngine.Object.Instantiate(d.panelSettings);ps.targetTexture=rt;ps.clearColor=true;d.panelSettings=ps;}
  var cfg=(GameConfig)F("config");var loc=(Locale)F("locale");var rules=(CareerRules)F("careerRules");

  foreach(var c in new[]{"case001","case002","case003","case004","case005","case006","case007","case008","case009","case010","case011","case012","case013","case014","case015","case016","case017","case018","case019","case020","case021","case022","case023","case024","case025","case026","case027","case028","case029","case030","case031","case032","case033","case034","case035","case036","case037","case038","case039","case040","case041","case042","case043","case044","case045"}){
   var data=JsonUtility.FromJson<CaseData>(Resources.Load<TextAsset>("Bube/Cases/"+c).text);
   var game=new Investigation(data,null,null,rules){Text=loc};game.Career.activeCaseId=c;game.AcceptCase();
   typeof(BubeApp).GetField("game",Any).SetValue(app,game);
   foreach(var n in data.nodes.Where(n=>n.kind=="interview"&&!string.IsNullOrEmpty(n.personId)).GroupBy(n=>n.personId).Select(g=>g.First())){
    var node=n;game.State.read.Add(node.id);game.RequestInterview(node.id,0);
    yield return Shot(c+"_"+node.personId+"_"+w+"x"+h,()=>Call("InterviewPage",node));
   }
   // KARINE_TOUR=case004 gibi: o vakanın masa, dosya, belge, CCTV ve özet sayfaları da çekilir.
   if(!(Environment.GetEnvironmentVariable("KARINE_TOUR")??"").Split(',').Contains(c))continue;
   foreach(var n in data.nodes)game.State.read.Add(n.id);
   // Açılış kartı turu örtmesin; oyuncunun kendi kaydı bozulmasın diye sonra geri alınır.
   string openedKey="karine.opened."+c;int opened=PlayerPrefs.GetInt(openedKey,0);PlayerPrefs.SetInt(openedKey,1);
   yield return Shot(c+"_00_desk",()=>Call("Desk"));
   yield return Shot(c+"_01_file",()=>Call("FilePage"));
   int k=0;
   foreach(var n in data.nodes){
    var node=n;
    if(n.kind=="cctv"){foreach(var e in n.cctvEvents){var id=e.id;yield return Shot(c+"_cctv_"+id,()=>Call("CctvScreen",node,id));}}
    else if(n.kind!="interview")yield return Shot(c+"_doc_"+(++k).ToString("00")+"_"+n.id,()=>Call("ReadPage",node));
   }
   yield return Shot(c+"_summary",()=>Call("CaseSummary"));
   yield return Shot(c+"_compare",()=>Call("ComparePage"));
   PlayerPrefs.SetInt(openedKey,opened);
  }
 }
}
}
