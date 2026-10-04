using System.Linq;
using UnityEngine.UIElements;

namespace Bube {
// Olay rekonstrüksiyonu adımı: dizilmemiş kartlar altta, dizilenler üstte sırayla.
// Bir karta dokununca okunmuş kayıtlar listelenir; seçilen kayıt karta bağlanır.
// Ekran sıranın doğru olup olmadığını söylemez.
public sealed partial class BubeApp {
 string reconFocus;
 void ReconStep(VisualElement scroll,int step) {
  KarineUI.ReportHeading(scroll,T("recon.heading"));
  Text(scroll,T("recon.help"),KarineTheme.Paper.Faded,15);
  var placed=game.Recon;
  for(int i=0;i<placed.Count;i++) {
   var p=placed[i];var card=game.Data.reconstruction.First(c=>c.id==p.cardId);var id=p.cardId;int index=i;
   KarineUI.ReconRow(scroll,(i+1).ToString("00"),T(card.labelKey),string.IsNullOrEmpty(p.sourceId)?T("recon.noSource"):CompactReportSourceLabel(p.sourceId),
    reconFocus==id,()=>{reconFocus=reconFocus==id?null:id;ConclusionStep(step);},
    index>0?()=>{game.ReconMove(id,-1);Save();ConclusionStep(step);}:null,
    index<placed.Count-1?()=>{game.ReconMove(id,1);Save();ConclusionStep(step);}:null,
    ()=>{game.ReconRemove(id);if(reconFocus==id)reconFocus=null;Save();ConclusionStep(step);});
   if(reconFocus==id) {
    KarineUI.RequestSection(scroll,T("recon.sourceHeading"),T("recon.sourceHelp"));
    foreach(var source in WarrantCandidates()) {
     var s=source;
     KarineUI.RequestBasis(scroll,CompactReportSourceLabel(s),p.sourceId==s,()=>{game.ReconSource(id,s);reconFocus=null;Save();ConclusionStep(step);});
    }
   }
  }
  var pool=game.Data.reconstruction.Where(c=>!game.ReconPlaced(c.id)).OrderBy(c=>c.id,System.StringComparer.Ordinal).ToArray();
  if(pool.Length==0)return;
  KarineUI.ReportHeading(scroll,T("recon.pool"));
  var wrap=new VisualElement();wrap.style.flexDirection=FlexDirection.Row;wrap.style.flexWrap=Wrap.Wrap;scroll.Add(wrap);
  foreach(var c in pool){var id=c.id;KarineUI.ReportChoiceCard(wrap,T(c.labelKey),false,()=>{game.ReconPlace(id);reconFocus=id;Save();ConclusionStep(step);});}
 }
}
}
