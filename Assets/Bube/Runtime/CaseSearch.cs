using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bube {
public sealed class SearchHit {
 public string nodeId;
 public string excerpt;
 public string eventId;
 public string turnReference;
}

public static class CaseSearch {
 static readonly CultureInfo Turkish=CultureInfo.GetCultureInfo("tr-TR");

 public static SearchHit[] Find(Investigation game,Locale locale,string query) {
  query=(query ?? "").Trim();
  if(query.Length<2)return new SearchHit[0];
  var hits=new List<SearchHit>();
  foreach(var node in game.Data.nodes) {
   if(node.kind=="interview") {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==node.id))
     Add(hits,node.id,locale.Get(turn.promptKey)+"\n"+locale.Get(turn.answerKey),query,null,game.InterviewTurnReference(turn));
   } else if(node.kind=="cctv" && game.State.read.Contains(node.id)) {
    foreach(var record in node.cctvEvents ?? new CctvEvent[0])
     Add(hits,node.id,locale.Get(record.textKey),query,record.id);
   } else if((node.kind=="document" || node.kind=="bps") && game.State.read.Contains(node.id)) {
    foreach(var line in locale.Get(node.bodyKey).Split('\n'))Add(hits,node.id,line,query);
   }
  }
  return hits.ToArray();
 }

 static void Add(List<SearchHit> hits,string nodeId,string text,string query,string eventId=null,string turnReference=null) {
  int at=text.ToUpper(Turkish).IndexOf(query.ToUpper(Turkish),StringComparison.Ordinal);
  if(at<0)return;
  int start=Math.Max(0,at-48);
  int end=Math.Min(text.Length,at+query.Length+72);
  var excerpt=(start>0?"…":"")+text.Substring(start,end-start).Trim().Replace('\n',' ')+(end<text.Length?"…":"");
  hits.Add(new SearchHit {nodeId=nodeId,excerpt=excerpt,eventId=eventId,turnReference=turnReference});
 }
}
}
