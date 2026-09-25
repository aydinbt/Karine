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
 // Süzgeçler satırın tam metnine bakar; `excerpt` kırpılmış olabilir.
 public string text;
}

public static class CaseSearch {
 static readonly CultureInfo Turkish=CultureInfo.GetCultureInfo("tr-TR");

 // Dosyada gezilebilir bütün satırlar. Kapı burasıdır: oyuncunun okumadığı
 // kaynak hiç toplanmaz, yani ne aramaya ne süzgece sızabilir.
 public static SearchHit[] Lines(Investigation game,Locale locale) {
  var lines=new List<SearchHit>();
  foreach(var node in game.Data.nodes) {
   if(node.kind=="interview") {
    foreach(var turn in game.State.interviewTurns.Where(t=>t.nodeId==node.id))
     lines.Add(new SearchHit {nodeId=node.id,
      text=locale.Get(turn.promptKey)+"\n"+locale.Get(turn.answerKey),
      turnReference=game.InterviewTurnReference(turn)});
   } else if(node.kind=="cctv" && game.State.read.Contains(node.id)) {
    foreach(var record in node.cctvEvents ?? new CctvEvent[0])
     lines.Add(new SearchHit {nodeId=node.id,text=locale.Get(record.textKey),eventId=record.id});
   } else if((node.kind=="document" || node.kind=="bps") && game.State.read.Contains(node.id)) {
    foreach(var line in locale.Get(node.bodyKey).Split('\n'))
     if(line.Trim().Length>0)lines.Add(new SearchHit {nodeId=node.id,text=line});
   }
  }
  foreach(var line in lines)line.excerpt=Shorten(line.text);
  return lines.ToArray();
 }

 static string Shorten(string text) {
  var flat=(text ?? "").Replace('\n',' ').Trim();
  return flat.Length<=180?flat:flat.Substring(0,179).TrimEnd()+"…";
 }

 public static SearchHit[] Find(Investigation game,Locale locale,string query) {
  query=(query ?? "").Trim();
  if(query.Length<2)return new SearchHit[0];
  var hits=new List<SearchHit>();
  foreach(var line in Lines(game,locale)) {
   var excerpt=Excerpt(line.text,query);
   if(excerpt==null)continue;
   line.excerpt=excerpt;hits.Add(line);
  }
  return hits.ToArray();
 }

 static string Excerpt(string text,string query) {
  int at=text.ToUpper(Turkish).IndexOf(query.ToUpper(Turkish),StringComparison.Ordinal);
  if(at<0)return null;
  int start=Math.Max(0,at-48);
  int end=Math.Min(text.Length,at+query.Length+72);
  return (start>0?"…":"")+text.Substring(start,end-start).Trim().Replace('\n',' ')+(end<text.Length?"…":"");
 }
}
}
