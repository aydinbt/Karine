using System.Collections.Generic;
using UnityEngine;

namespace Bube {
// Görselsiz kozmetikler: ekran fosforu, damga mürekkebi, film tonu.
// Şehir ışıkları seçeneği 10 Ekim 2026'da kaldırıldı; pencere hep varsayılan renkte yanar.
// Seçim cihazda (PlayerPrefs), açılanlar kariyer belgesinde ("screen:2"). Önizleme,
// ayarlar açıkken seçimi geçici olarak ezer; ayarlar kapanınca kalkar.
public static class Cosmetics {
 public static readonly string[] Ids={"screen","ink","film"};
 static readonly Dictionary<string,int> preview=new Dictionary<string,int>();
 const string Key="karine.cosmetic.";

 public static Color[] Colors(string id)=>id switch{
  "screen"=>KarineTheme.Cosmetic.Screen,"ink"=>KarineTheme.Cosmetic.Ink,
  "city"=>KarineTheme.Cosmetic.City,_=>KarineTheme.Cosmetic.Film};
 public static int Saved(string id)=>Mathf.Clamp(PlayerPrefs.GetInt(Key+id,0),0,Colors(id).Length-1);
 public static int Chosen(string id)=>preview.TryGetValue(id,out var v)?v:Saved(id);
 public static void Save(string id,int i)=>PlayerPrefs.SetInt(Key+id,Mathf.Clamp(i,0,Colors(id).Length-1));
 public static void Preview(string id,int i)=>preview[id]=i;
 public static void EndPreview()=>preview.Clear();
}
}
