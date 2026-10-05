using System;
using System.Collections.Generic;
using System.Linq;
namespace Bube {
// Olay rekonstrüksiyonu (#010): oyuncu olay kartlarını kendi sırasına dizer ve her
// karta dayandığı kaydı bağlar. Saat verilmez, anında doğru/yanlış söylenmez;
// sıra ve kaynaklar rapor değerlendirmesinde ayrı satır olarak okunur.
// Kartların verideki sırası doğru sıradır; ekranda karışık gelir.
[Serializable] public class ReconCard { public string id; public string labelKey; public string[] supportingSourceIds; }
[Serializable] public class ReconPlacement { public string cardId; public string sourceId; }
// Bölüm finali (#010): onaylı değerlendirme yerine telefon, personel belgesi ve
// bölüm kapanışı oynar; yeni ülkenin ilk dosyası gelen evraka düşer.
[Serializable] public class ChapterFinale { public string countryKey; public string progressKey; public string nextCountryKey; public string nextFileKey;
 // İsteğe bağlı: bu finale özel telefon satırları ve personel metni; boşsa ortak finale.call.* ve finale.personnelBody.
 public string[] callKeys; public string personnelBodyKey; }

public sealed partial class Investigation {
 public bool HasReconstruction => (Data.reconstruction ?? new ReconCard[0]).Length>0;
 public List<ReconPlacement> Recon => State.recon ??= new List<ReconPlacement>();
 ReconCard Card(string id) => (Data.reconstruction ?? new ReconCard[0]).FirstOrDefault(c=>c.id==id);
 public bool ReconPlaced(string id) => Recon.Any(p=>p.cardId==id);
 public bool ReconPlace(string id) {
  if(State.closed || Card(id)==null || ReconPlaced(id))return false;
  Recon.Add(new ReconPlacement{cardId=id});return true;
 }
 public bool ReconRemove(string id) => !State.closed && Recon.RemoveAll(p=>p.cardId==id)>0;
 public bool ReconMove(string id,int delta) {
  int i=Recon.FindIndex(p=>p.cardId==id),j=i+delta;
  if(State.closed || i<0 || j<0 || j>=Recon.Count)return false;
  var p=Recon[i];Recon.RemoveAt(i);Recon.Insert(j,p);return true;
 }
 public bool ReconSource(string id,string sourceId) {
  var p=Recon.FirstOrDefault(x=>x.cardId==id);
  if(State.closed || p==null || !WarrantBasisAvailable(sourceId))return false;
  p.sourceId=sourceId;return true;
 }
 public bool ReconComplete => !HasReconstruction ||
  Recon.Count==Data.reconstruction.Length && Recon.All(p=>!string.IsNullOrEmpty(p.sourceId));
 // Kart kaynağı düğümün yalın kimliği de olabilir: CCTV olayı seçildiyse düğümle eşleşir.
 bool ReconBacks(ReconCard card,string sourceId) => (card.supportingSourceIds ?? new string[0]).Any(id=>
  id==sourceId || sourceId.StartsWith(id+"#",StringComparison.Ordinal) || id.StartsWith(sourceId+"#",StringComparison.Ordinal));
 public bool ReconSupported => !HasReconstruction || ReconComplete &&
  Recon.Select(p=>p.cardId).SequenceEqual(Data.reconstruction.Select(c=>c.id)) &&
  Recon.All(p=>ReconBacks(Card(p.cardId),p.sourceId));
}
}
