using System;
using System.Collections.Generic;
using System.Linq;
namespace Bube {
public sealed partial class Investigation {
 // Soruşturma hattı (#009): izin motorunu kullanan, kurumsal kaynak ayıran talep. Aynı anda
 // en çok `lineSlots` hat açık olabilir; açık hattın alt kayıtları ayrı ayrı istenir. Hat
 // kapatılınca gelmiş kayıtlar kalır, yeni alt talep açılmaz; sonraki hat daha geç gelir.
 public static bool IsLine(Node n) => IsWarrant(n) && n.requestKind=="line";
 public int LineSlots => Data.lineSlots>0?Data.lineSlots:2;
 List<string> ClosedLines => State.closedLines ??= new List<string>();
 public bool LineClosed(string id) => ClosedLines.Contains(id);
 List<DocumentRequest> Reopens => State.lineReopens ??= new List<DocumentRequest>();
 public DocumentRequest LineReopening(string id) => Reopens.FirstOrDefault(r=>r.nodeId==id && r.readyAtUtcTicks>DateTime.UtcNow.Ticks);
 public bool LineActive(string id) => !LineClosed(id) && State.read.Contains(id) && LineReopening(id)==null;
 bool LineHeld(Node n) => !LineClosed(n.id) && (State.read.Contains(n.id) || State.documentRequests.Any(r=>r.nodeId==n.id));
 public int ActiveLines => Data.nodes.Count(n=>IsLine(n) && LineHeld(n));
 public bool LineSlotFree => ActiveLines<LineSlots;
 public bool CanCloseLine(Node n) => IsLine(n) && LineActive(n.id) && !State.closed;
 public bool CloseLine(string id) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  if(n==null || !CanCloseLine(n))return false;
  ClosedLines.Add(id);
  return true;
 }
 // Kapatılan hat yeniden açılabilir (kilitlenme olmasın); bedeli bekleme süresidir.
 public bool CanReopenLine(Node n) => IsLine(n) && LineClosed(n.id) && LineSlotFree && !State.closed;
 public bool ReopenLine(string id,double? waitSeconds=null) {
  var n=Data.nodes.FirstOrDefault(x=>x.id==id);
  if(n==null || !CanReopenLine(n))return false;
  ClosedLines.Remove(id);Reopens.RemoveAll(r=>r.nodeId==id);
  double wait=Math.Max(0,waitSeconds ?? n.requestDelaySeconds+Math.Max(0,Data.lineReopenPenaltySeconds));
  Reopens.Add(new DocumentRequest { nodeId=id,readyAtUtcTicks=DateTime.UtcNow.AddSeconds(wait).Ticks });
  return true;
 }
}
}
