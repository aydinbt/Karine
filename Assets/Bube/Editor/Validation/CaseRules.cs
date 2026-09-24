using System.IO;
using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Her vakaya uygulanan kurallar. Vakaya özel iddialar burada DEĞİL, Case001Rules /
// Case002Rules içinde durur; eskiden `data.id=="case001"` blokları bu mantığın
// içine gömülüydü ve üçüncü vakada dosya okunamaz hâle gelecekti.
public static class CaseRules {

 public static bool MissingText(Locale locale, string key) =>
  string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("[");

 public static void ValidateStructure(CaseData data, Locale locale, ValidationReport report) {
  var nodes = data.nodes ?? new Node[0];
  if (!report.Step(nodes.Length > 0, "Vakanın hiç düğümü yok.")) return;

  report.Require(nodes.Select(n => n.id).Distinct().Count() == nodes.Length, "Yinelenen düğüm kimliği.");
  report.Require(nodes.SelectMany(n => n.questions ?? new Question[0]).Select(q => q.id).Distinct().Count()
   == nodes.Sum(n => (n.questions ?? new Question[0]).Length), "Yinelenen soru kimliği.");
  report.Require(data.failedReportTrustLoss >= 0 && data.successfulReportTrustGain >= 0,
   "Kariyer etkisi negatif olamaz.");

  // Rapor sihirbazının üç sütunu da tam olarak bir doğru seçenek içermeli.
  // Sıfır olursa vaka çözülemez, birden fazla olursa değerlendirme keyfîleşir.
  // Bu kontrol 25 Eylül 2026 denetiminde elle yapıldı; burada kalıcılaşıyor.
  RequireExactlyOneCorrect(data.verdicts?.Select(v => v.correct), "şüpheli", report);
  RequireExactlyOneCorrect(data.methods?.Select(m => m.correct), "yöntem", report);
  RequireExactlyOneCorrect(data.evidence?.Select(e => e.correct), "kanıt", report);

  foreach (var id in (data.verdicts ?? new Verdict[0]).SelectMany(v => v.supportingSourceIds ?? new string[0])
    .Concat((data.methods ?? new Choice[0]).SelectMany(v => v.supportingSourceIds ?? new string[0]))
    .Concat((data.evidence ?? new Choice[0]).SelectMany(v => v.supportingSourceIds ?? new string[0]))) {
   var parts = id.Split('#');
   var source = nodes.FirstOrDefault(n => n.id == parts[0]);
   report.Forbid(source == null || parts.Length > 2 || parts.Length == 2 &&
    (source.kind != "cctv" || !(source.cctvEvents ?? new CctvEvent[0]).Any(e => e.id == parts[1])),
    "Bilinmeyen rapor dayanağı: " + id);
  }

  foreach (var verdict in data.verdicts ?? new Verdict[0])
   report.Forbid(MissingText(locale, verdict.labelKey), "Şüpheli etiketi eksik: " + verdict.id);

  report.Require(data.summary != null && new[] { data.summary.locationKey, data.summary.truthKey,
    data.summary.evidenceKey, data.summary.lessonKey }.All(key => !MissingText(locale, key)),
   "Vaka özeti eksik.");

  report.Forbid((data.conclusionRequires ?? new string[0]).Any(id => !nodes.Any(x => x.id == id)),
   "Bilinmeyen kapanış önkoşulu.");

  ValidateTimeline(data, locale, report);
  foreach (var node in nodes) ValidateNode(data, node, locale, report);
 }

 static void RequireExactlyOneCorrect(System.Collections.Generic.IEnumerable<bool> flags, string label, ValidationReport report) {
  int count = flags?.Count(flag => flag) ?? -1;
  report.Require(count == 1, "Tam olarak bir doğru " + label + " olmalı, " +
   (count < 0 ? "liste yok" : count + " var") + ".");
 }

 static void ValidateTimeline(CaseData data, Locale locale, ValidationReport report) {
  var timeline = data.timelineClues ?? new TimelineClue[0];
  report.Require(timeline.Select(c => c.id).Distinct().Count() == timeline.Length,
   "Yinelenen zaman çizelgesi ipucu kimliği.");
  foreach (var clue in timeline) {
   report.Require(!string.IsNullOrEmpty(clue.id) && clue.sortMinute >= 0 && clue.sortMinute <= 1439,
    "Geçersiz zaman çizelgesi ipucu: " + clue.id);
   report.Forbid(new[] { clue.timeKey, clue.noteKey, clue.sourceKey }.Any(key => MissingText(locale, key)),
    "Zaman çizelgesi metni eksik: " + clue.id);
   report.Forbid((clue.requiresRead ?? new string[0]).Any(id => !data.nodes.Any(n => n.id == id)),
    "Bilinmeyen zaman çizelgesi kaynağı: " + clue.id);
   report.Forbid((clue.requiresAsked ?? new string[0]).Any(id =>
     !data.nodes.SelectMany(n => n.questions ?? new Question[0]).Any(q => q.id == id)),
    "Bilinmeyen zaman çizelgesi sorusu: " + clue.id);
  }
 }

 static void ValidateNode(CaseData data, Node node, Locale locale, ValidationReport report) {
  var allQuestions = data.nodes.SelectMany(x => x.questions ?? new Question[0]).ToArray();

  report.Forbid(node.requestable && (node.kind != "document" || node.requestDelaySeconds < 0 ||
   MissingText(locale, node.requestLabelKey)), "Geçersiz belge talebi: " + node.id);
  report.Forbid((node.requires ?? new string[0]).Concat(node.requiresAny ?? new string[0])
   .Any(id => !data.nodes.Any(x => x.id == id)), "Bilinmeyen önkoşul: " + node.id);
  report.Forbid((node.requiresAsked ?? new string[0]).Concat(node.requiresAnyAsked ?? new string[0])
   .Any(id => !allQuestions.Any(q => q.id == id)), "Bilinmeyen soru önkoşulu: " + node.id);
  foreach (var key in new[] { node.titleKey, node.bodyKey, "kind." + node.kind })
   report.Forbid(MissingText(locale, key), "Metin eksik: " + key);

  foreach (var field in node.fileMeta ?? new FileMeta[0])
   foreach (var key in new[] { field.labelKey, field.valueKey })
    report.Forbid(MissingText(locale, key), "Dosya künyesi metni eksik: " + key);

  if (!string.IsNullOrEmpty(node.imageResource)) {
   report.Require(Resources.Load<Texture2D>(node.imageResource) != null,
    "Dosya görseli yok: " + node.imageResource);
   report.Forbid(MissingText(locale, node.imageCaptionKey), "Görsel açıklaması eksik: " + node.id);
  }

  if (node.kind == "interview") ValidateInterview(data, node, locale, report, allQuestions);
  if (node.kind == "cctv") ValidateCctv(node, locale, report);
 }

 static void ValidateInterview(CaseData data, Node node, Locale locale, ValidationReport report, Question[] allQuestions) {
  if (!report.Step(!string.IsNullOrEmpty(node.personId) && node.questions != null && node.questions.Length > 0 &&
   node.completionQuestionIds != null && node.completionQuestionIds.Length > 0,
   "Eksik görüşme: " + node.id)) return;

  if (Resources.Load<Texture2D>("Bube/Characters/" + node.personId) == null)
   report.Note("Üretilmiş yedek portre kullanılıyor: " + node.personId);

  foreach (var key in new[] { node.personNameKey, node.personInfoKey }
    .Concat(node.questions.SelectMany(q => new[] { q.promptKey, q.answerKey })))
   report.Forbid(MissingText(locale, key), "Görüşme metni eksik: " + key);
  if (!string.IsNullOrEmpty(node.personQuoteKey))
   report.Forbid(MissingText(locale, node.personQuoteKey), "Görüşme alıntısı eksik: " + node.personQuoteKey);
  report.Forbid(node.completionQuestionIds.Any(id => !node.questions.Any(q => q.id == id)),
   "Bilinmeyen tamamlama sorusu: " + node.id);

  foreach (var question in node.questions) {
   if (!string.IsNullOrEmpty(question.topicKey))
    report.Forbid(MissingText(locale, question.topicKey), "Görüşme başlığı eksik: " + question.id);
   report.Forbid(!string.IsNullOrEmpty(question.presentedSourceId) && !data.nodes.Any(x =>
     x.id == question.presentedSourceId && (x.kind == "document" || x.kind == "cctv" || x.kind == "bps")),
    "Sunulabilir olmayan kaynak: " + question.id);

   foreach (var sourceRef in question.presentedSourceIds ?? new string[0]) {
    var separator = sourceRef.IndexOf('#');
    var source = data.nodes.FirstOrDefault(x => x.id == (separator < 0 ? sourceRef : sourceRef.Substring(0, separator)));
    if (!report.Step(source != null, "Bilinmeyen görüşme kaynağı: " + question.id + " → " + sourceRef)) continue;
    if (separator < 0) continue;
    var detail = sourceRef.Substring(separator + 1);
    report.Forbid(
     source.kind == "cctv" && !(source.cctvEvents ?? new CctvEvent[0]).Any(e => e.id == detail) ||
     source.kind == "interview" && !(source.questions ?? new Question[0]).Any(other => other.id == detail.Split('|')[0]) ||
     source.kind != "cctv" && source.kind != "interview",
     "Bilinmeyen görüşme kaynağı ayrıntısı: " + question.id + " → " + sourceRef);
   }

   foreach (var response in question.presentedAnswers ?? new PresentedAnswer[0])
    report.Forbid(!(question.presentedSourceIds ?? new string[0]).Contains(response.sourceId) ||
     MissingText(locale, response.answerKey), "Geçersiz kaynak yanıtı: " + question.id);
   foreach (var key in (question.answerVariants ?? new AnswerVariant[0]).Select(v => v.answerKey))
    report.Forbid(MissingText(locale, key), "Yanıt çeşidi eksik: " + key);

   foreach (var id in (question.requiresAsked ?? new string[0])
     .Concat(question.requiresAnyAsked ?? new string[0])
     .Concat(question.excludesAsked ?? new string[0])
     .Concat((question.answerVariants ?? new AnswerVariant[0])
      .SelectMany(v => (v.requiresAsked ?? new string[0]).Concat(v.excludesAsked ?? new string[0]))))
    report.Forbid(!allQuestions.Any(x => x.id == id), "Bilinmeyen soru önkoşulu: " + id);
   foreach (var id in (question.requiresRead ?? new string[0])
     .Concat((question.answerVariants ?? new AnswerVariant[0]).SelectMany(v => v.requiresRead ?? new string[0])))
    report.Forbid(!data.nodes.Any(x => x.id == id), "Bilinmeyen yanıt kanıtı önkoşulu: " + id);
  }
 }

 static void ValidateCctv(Node node, Locale locale, ValidationReport report) {
  var events = node.cctvEvents ?? new CctvEvent[0];
  if (!report.Step(events.Length > 0, "CCTV olayları yok: " + node.id)) return;
  report.Forbid(events.Any(e => string.IsNullOrEmpty(e.id)) ||
   events.Select(e => e.id).Distinct().Count() != events.Length,
   "CCTV olay kimlikleri eksik ya da yinelenmiş: " + node.id);
  report.Forbid(events.Any(e => e.delayMs < 0), "Negatif CCTV gösterim gecikmesi: " + node.id);
  foreach (var record in events)
   if (!string.IsNullOrEmpty(record.videoPath))
    report.Require(File.Exists(Path.Combine(Application.streamingAssetsPath, record.videoPath)),
     "CCTV görüntüsü yok: " + record.videoPath);
  foreach (var key in new[] { node.cctvSourceKey, node.cctvPeriodKey }
    .Concat(new[] { node.cctvOverlayKey }.Where(k => !string.IsNullOrEmpty(k)))
    .Concat(events.SelectMany(e => new[] { e.textKey, e.overlayTimeKey, e.glitchKey, e.signalKey })
     .Where(k => !string.IsNullOrEmpty(k))))
   report.Forbid(MissingText(locale, key), "CCTV metni eksik: " + key);
 }
}
}
