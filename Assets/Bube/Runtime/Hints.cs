using System.Linq;

namespace Bube {

// Kademeli ipucu (ödüllü reklam, 15 Ekim 2026). Coverage gibi yalnız oyuncunun
// **kendi çalışmasına** bakar: açılmış ama okunmamış kaynak, sorulmamış soru,
// öne sürülecek kaydı elinde olan soru. Seçim veri sırasıyladır; doğru sonucun
// dayandığı kaynaklar öne alınmaz, fail, yalan ya da çelişki adlandırılmaz.
//  1 — ne tür bir şeyin beklediği ("açmadığın bir kamera kaydı var")
//  2 — yeri ("şu belgeyi oku", "şu kişiye sorulacak bir şey var")
//  3 — karşılaştırılacak ikili ("şu kişiyle görüşmede şu kayıt") — sonucu söylemez
public struct Hint {
 public int Tier;
 public string Kind;      // "document", "interview", "cctv"… ya da "question"
 public string NodeId;    // okunacak kaynak ya da soru sorulacak kişi
 public string SourceId;  // yalnız 3. kademede: öne sürülebilecek, okunmuş kayıt
 public bool Empty => string.IsNullOrEmpty(NodeId);
}

public static class Hints {
 public const int MaxTier = 3;

 // Dosya #001 ilk dosyadır: ipucu yok (MASTER_GAME_CONTEXT, ödüllü reklam kuralı).
 public static bool Allowed(Investigation game) =>
  game != null && game.Data != null && game.Data.id != "case001" && !game.State.closed;

 public static Hint For(Investigation game, int tier) {
  var nodes = game.Data.nodes ?? new Node[0];
  var state = game.State;
  if (tier >= 3) {
   foreach (var node in nodes)
    foreach (var q in node.questions ?? new Question[0]) {
     if (state.asked.Contains(q.id) || !game.QuestionAvailable(node, q) || !game.QuestionNeedsSource(q)) continue;
     var ids = (q.presentedSourceIds ?? new string[0]).Prepend(q.presentedSourceId).Where(s => !string.IsNullOrEmpty(s));
     var held = ids.FirstOrDefault(s => state.read.Contains(s.Split('#')[0]));
     if (held != null) return new Hint { Tier = 3, Kind = "question", NodeId = node.id, SourceId = held };
    }
  }
  // 3. kademede eşleşme yoksa (ya da istenen 1–2 ise) okunmamış kaynak, sonra sorulmamış soru.
  var unread = nodes.FirstOrDefault(n => !state.read.Contains(n.id) && game.Available(n));
  if (unread != null) return new Hint { Tier = tier >= 2 ? 2 : 1, Kind = unread.kind, NodeId = unread.id };
  var person = nodes.FirstOrDefault(n => (n.questions ?? new Question[0]).Any(q => !state.asked.Contains(q.id) && game.QuestionAvailable(n, q)));
  if (person != null) return new Hint { Tier = tier >= 2 ? 2 : 1, Kind = "question", NodeId = person.id };
  return new Hint();
 }
}
}
