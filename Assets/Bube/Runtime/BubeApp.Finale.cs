using System;
using System.Linq;
using UnityEngine;

namespace Bube {
// Bölüm finali: onaylanan son dosyanın değerlendirmesi faks yerine telefonla gelir.
// Telefon → personel belgesi → "ÜLKE — n / n" → yeni dosya; ardından faks tablosu açılır.
// Vaka başına bir kez oynar.
public sealed partial class BubeApp {
 const string FinalePrefix="karine.finale.";
 void ChapterFinale(string caseId,Action then) {
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  var finale=data?.chapterFinale;
  // Son dosya: final bitince bir kez jenerik ve "devamı gelecek".
  if(data!=null && string.IsNullOrEmpty(data.nextCaseId)){var after=then;then=()=>EndCredits(after);}
  if(finale==null || string.IsNullOrEmpty(finale.countryKey) || PlayerPrefs.GetInt(FinalePrefix+caseId,0)==1){then?.Invoke();return;}
  PlayerPrefs.SetInt(FinalePrefix+caseId,1);PlayerPrefs.Save();
  var keys=finale.callKeys!=null && finale.callKeys.Length>0?finale.callKeys:Enumerable.Range(1,5).Select(i=>"finale.call."+i).ToArray();
  var lines=keys.Where(locale.Has).Select(T).ToArray();
  var personnelBody=string.IsNullOrEmpty(finale.personnelBodyKey)?"finale.personnelBody":finale.personnelBodyKey;
  KarineUI.FinaleCall(root,T("finale.morning"),lines,()=>
   KarineUI.PersonnelReview(root,T("finale.personnel"),PersonnelRows(caseId),T(personnelBody),T("finale.program"),T(finale.nextCountryKey),T("finale.closeFile"),()=>
    KarineUI.ChapterComplete(root,T(finale.countryKey)+" — "+T(finale.progressKey),string.Format(T("finale.complete"),T(finale.countryKey)),()=>
     KarineUI.NewFileDrop(root,T(finale.nextFileKey),T(finale.nextCountryKey),T("finale.sealed"),then))));
 }
 void EndCredits(Action then) {
  if(game.Career.creditsSeen){then?.Invoke();return;}
  game.Career.creditsSeen=true;Save();
  KarineUI.Credits(root,new[]{T("credits.0"),T("credits.1"),T("credits.2"),T("credits.3")},then);
 }
 // Bölümdeki dosyalar bölüm seçici sırasıyla; durum yalnız değerlendirme geçmişinden.
 (string,string)[] PersonnelRows(string caseId) {
  var country=Worlds.Load().countries.FirstOrDefault(c=>c.slots.Any(s=>s.caseId==caseId));
  if(country==null)return new (string,string)[0];
  return country.slots.Where(s=>!string.IsNullOrEmpty(s.caseId)).Select(s=> {
   var reviews=game.Career.reviewHistory.Where(r=>r.caseId==s.caseId).ToList();
   bool approved=s.caseId==caseId || reviews.Any(r=>r.correct);
   return (T(s.titleKey),T(approved?"finale.state.approved":reviews.Count>0?"finale.state.reviewed":"finale.state.none"));
  }).ToArray();
 }
 // Portre: önce vakaya özel "Characters/<vaka>/<kişi>", yoksa ortak "Characters/<kişi>" (aynı adlı iki kişi için).
 static Texture2D Portrait(string caseId,string personId)=>Resources.Load<Texture2D>("Bube/Characters/"+caseId+"/"+personId)??Resources.Load<Texture2D>("Bube/Characters/"+personId);
 Texture2D Portrait(string personId)=>Portrait(game?.Data?.id,personId);
}
}
