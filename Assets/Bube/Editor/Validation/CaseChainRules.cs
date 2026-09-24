using System.Collections.Generic;
using System.Linq;

namespace Bube.Editor {

// Vaka zinciri kuralları. Bu kontrol 25 Eylül 2026 denetiminde elle yapıldı;
// burada kalıcılaşıyor. Zincir bozulursa oyuncu bir vakayı bitirdikten sonra
// sıradakine hiç geçemez — ve bu, tek tek vakalara bakarak görülemez.
public static class CaseChainRules {
 public static void Validate(IReadOnlyList<CaseData> cases, GameConfig config, ValidationReport report) {
  report.Scope("vaka zinciri");
  var byId = new Dictionary<string, CaseData>();
  foreach (var data in cases) {
   if (string.IsNullOrEmpty(data.id)) { report.Problem("Kimliği olmayan vaka dosyası var."); continue; }
   if (byId.ContainsKey(data.id)) report.Problem("Yinelenen vaka kimliği: " + data.id);
   else byId[data.id] = data;
  }

  if (!report.Step(!string.IsNullOrEmpty(config.initialCase), "config.json `initialCase` boş.")) return;
  if (!report.Step(byId.ContainsKey(config.initialCase), "İlk vaka bulunamadı: " + config.initialCase)) return;
  report.Forbid(byId[config.initialCase].draft, "İlk vaka taslak olamaz: " + config.initialCase);

  foreach (var data in byId.Values) {
   if (string.IsNullOrEmpty(data.nextCaseId)) continue;
   report.Forbid(data.nextCaseId == data.id, data.id + " kendini sıradaki vaka olarak gösteriyor.");
   if (!byId.ContainsKey(data.nextCaseId)) {
    report.Problem(data.id + " var olmayan bir vakaya işaret ediyor: " + data.nextCaseId);
    continue;
   }
   // Taslak hedef hata değil: oyun taslak vakayı görev olarak sunmuyor.
   // Ama zincirin orada bittiğini söylemek gerekir, sessizce durmasın.
   if (byId[data.nextCaseId].draft)
    report.Note(data.id + " sonrası " + data.nextCaseId + " taslak; zincir şimdilik burada bitiyor.");
  }

  // Döngü, oyuncuyu aynı iki vaka arasında sonsuza kadar döndürür.
  var seen = new List<string>();
  for (var id = config.initialCase; !string.IsNullOrEmpty(id) && byId.ContainsKey(id); id = byId[id].nextCaseId) {
   if (seen.Contains(id)) {
    report.Problem("Vaka zincirinde döngü: " + string.Join(" → ", seen) + " → " + id);
    break;
   }
   seen.Add(id);
  }

  foreach (var id in byId.Keys.Where(id => !seen.Contains(id)))
   report.Note("Vaka zincire bağlı değil, oyuncu ulaşamaz: " + id);
 }
}
}
