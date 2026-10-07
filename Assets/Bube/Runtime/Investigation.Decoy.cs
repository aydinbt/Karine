using System.Collections.Generic;
using System.Linq;
namespace Bube {
public sealed partial class Investigation {
 // Yem kayda gerçek bir yanıt alındıysa o soru o an kapanır: aynı soruyu hemen
 // ikinci bir kayıtla yeniden sormak gerçek bir sorguda olmaz. Soru ancak
 // dosyaya yeni bir şey girince (yeni okunan kaynak, yeni alınan yanıt) geri
 // gelir — yeni atıf, soruyu yeniden açmanın gerekçesidir. Açılacak başka iş
 // kalmadıysa beklemez; yoksa vaka kilitlenirdi.
 int ProgressMark => State.read.Count+State.asked.Count;
 public void HoldAfterDecoy(Node n,Question q) {
  State.decoyHolds ??= new List<string>();
  var prefix=n.id+"/"+q.id+"/";
  State.decoyHolds.RemoveAll(h=>h.StartsWith(prefix));
  State.decoyHolds.Add(prefix+ProgressMark);
 }
 public bool DecoyHeld(Node n,Question q) {
  if(State.decoyHolds==null || State.decoyHolds.Count==0)return false;
  var prefix=n.id+"/"+q.id+"/";
  var hold=State.decoyHolds.FirstOrDefault(h=>h.StartsWith(prefix));
  if(hold==null || !int.TryParse(hold.Substring(prefix.Length),out var mark) || mark!=ProgressMark)return false;
  return OtherWorkLeft(n,q);
 }
 bool OtherWorkLeft(Node held,Question heldQ) {
  foreach(var n in Data.nodes) {
   if(n.kind!="interview" && Discovered(n) && !State.read.Contains(n.id))return true;
   if(n.kind=="interview" && Available(n))
    foreach(var q in n.questions ?? new Question[0])
     if(q!=heldQ && QuestionAvailable(n,q) && !State.asked.Contains(q.id))return true;
  }
  return false;
 }
}
}
