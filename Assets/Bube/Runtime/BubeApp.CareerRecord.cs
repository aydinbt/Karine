using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Sicil: kariyerin seyri. Güven her faksla nasıl değişti, Bora hangi kademeden
// hangisine geçti, hangi ülkede nasıl bir karne bıraktı. Hepsi kayıtlı faks
// geçmişinden türer; bekleyen değerlendirmenin sonucu burada da sızmaz.
public sealed partial class BubeApp {
 void CareerRecord(VisualElement parent,List<FaxReview> history) {
  var ordered=history.OrderBy(r=>r.evaluatedAtUtcTicks).ToList();
  int start=careerRules.initialTrust;
  var curve=KarineUI.BoardPanel(parent,T("career.record.curve"));curve.style.flexShrink=0;
  KarineUI.TrustCurve(curve,start,ordered.Select(r=>(r.trustAfter,r.trustChange)).ToList());
  int lowest=ordered.Count==0?start:Mathf.Min(start,ordered.Min(r=>r.trustAfter));
  int highest=ordered.Count==0?start:Mathf.Max(start,ordered.Max(r=>r.trustAfter));
  var summary=KarineUI.Body_(curve,T("career.record.start")+" "+KarineUI.Percent(start)+"   ·   "+T("career.record.now")+" "+KarineUI.Percent(game.Career.departmentTrust)+
   "   ·   "+T("career.record.lowest")+" "+KarineUI.Percent(lowest)+"   ·   "+T("career.record.highest")+" "+KarineUI.Percent(highest),KarineTheme.CareerBoard.RecordLineSize);
  summary.style.color=KarineTheme.Secondary;summary.style.marginTop=KarineTheme.SpaceSm;summary.style.marginBottom=0;

  var lower=new VisualElement();lower.style.flexDirection=FlexDirection.Row;lower.style.flexGrow=1;lower.style.minHeight=0;lower.style.marginTop=KarineTheme.SpaceSm;parent.Add(lower);
  var ranks=KarineUI.BoardPanel(lower,T("career.record.ranks"));ranks.style.flexGrow=1;ranks.style.flexBasis=0;ranks.style.marginRight=KarineTheme.SpaceMd;ranks.style.minHeight=0;
  var rankList=Scroll(ranks);
  string previous=Investigation.StatusKeyFor(start,careerRules);int changes=0;
  KarineUI.RecordLine(rankList,T("career.record.began"),T(previous),KarineTheme.Secondary);
  foreach(var review in ordered) {
   var status=Investigation.StatusKeyFor(review.trustAfter,careerRules);
   if(status==previous)continue;
   bool up=Rank(status)<Rank(previous);changes++;
   KarineUI.RecordLine(rankList,CaseTitle(review.caseId)+"  ·  "+T(previous)+" → ",T(status)+(up?" ↑":" ↓"),up?KarineTheme.CareerBoard.Supported:KarineTheme.Danger);
   previous=status;
  }
  if(changes==0)KarineUI.Body_(rankList,T("career.record.noChange"),KarineTheme.CareerBoard.RecordLineSize).style.color=KarineTheme.Secondary;

  var card=KarineUI.BoardPanel(lower,T("career.record.countries"));card.style.flexGrow=1;card.style.flexBasis=0;card.style.minHeight=0;
  var cardList=Scroll(card);
  atlas=atlas??Worlds.Load();int shown=0;
  foreach(var country in atlas.countries) {
   var ids=new HashSet<string>(country.slots.Where(s=>!string.IsNullOrEmpty(s.caseId)).Select(s=>s.caseId));
   var rows=ordered.Where(r=>ids.Contains(r.caseId)).ToList();
   if(rows.Count==0)continue;shown++;
   int ok=rows.Count(r=>r.evaluationType=="supported"||r.evaluationType=="lucky"),part=rows.Count(r=>r.evaluationType=="incomplete"),
    wrong=rows.Count(r=>r.evaluationType=="falseAccusation"),net=rows.Sum(r=>r.trustChange);
   KarineUI.RecordLine(cardList,T(country.nameKey)+"   ✓"+ok+"  ~"+part+"  ✗"+wrong,(net>0?"+":"")+net,
    net>0?KarineTheme.CareerBoard.Supported:net<0?KarineTheme.Danger:KarineTheme.Secondary);
  }
  if(shown==0)KarineUI.Body_(cardList,T("career.noHistory"),KarineTheme.CareerBoard.RecordLineSize).style.color=KarineTheme.Secondary;
 }

 static readonly string[] RankOrder={"career.status.high","career.status.reliable","career.status.monitored","career.status.review","career.status.risk","career.status.ended"};
 static int Rank(string key)=>System.Array.IndexOf(RankOrder,key);

 string CaseTitle(string caseId) {
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  return data==null?caseId:T(data.titleKey);
 }
}
}
