using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace Bube.Editor {

// Her vakaya uygulanan kurallar. Vakaya özel iddialar burada DEĞİL, Case001Rules /
// Case002Rules içinde durur; eskiden `data.id=="case001"` blokları bu mantığın
// içine gömülüydü ve üçüncü vakada dosya okunamaz hâle gelecekti.
public static class CaseRules {

 public static bool MissingText(Locale locale, string key) =>
  string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("[");
 static string Text(Locale locale, string key) => MissingText(locale, key) ? null : locale.Get(key);

 public static void ValidateStructure(CaseData data, Locale locale, ValidationReport report) {
  var nodes = data.nodes ?? new Node[0];
  if (!report.Step(nodes.Length > 0, "Vakanın hiç düğümü yok.")) return;

  report.Require(nodes.Select(n => n.id).Distinct().Count() == nodes.Length, "Yinelenen düğüm kimliği.");
  report.Require(nodes.SelectMany(n => n.questions ?? new Question[0]).Select(q => q.id).Distinct().Count()
   == nodes.Sum(n => (n.questions ?? new Question[0]).Length), "Yinelenen soru kimliği.");
  report.Require(data.failedReportTrustLoss >= 0 && data.successfulReportTrustGain >= 0,
   "Kariyer etkisi negatif olamaz.");

  // Vakayi acan tutanak soruşturmanin baslangicidir: rapora gerekce olarak
  // yazilamaz. Isaret veride durur, kod vaka kimligi bilmez.
  foreach (var opener in nodes.Where(n => n.kind == "document" && (n.requires == null || n.requires.Length == 0)))
   report.Require(opener.notReportSource,
    "Vakayı açan tutanak rapora kaynak olamaz, `notReportSource` eksik: " + opener.id);

  // Gelen evrak tepsisindeki teklif metni vakaya özeldir. Anahtar yoksa oyun
  // genel yedeğe düşer; o yedek bir zamanlar Dosya #001'i anlatıyordu, yani
  // ikinci vakayı kabul eden oyuncu birincinin özetini okuyordu. Artık her
  // vaka kendi teklif metnini yazmak zorunda.
  foreach (var suffix in new[] { "offer.title", "offer.subtitle", "offer.summary",
                                "file.caseType", "tablet.caseLine" })
   report.Require(!MissingText(locale, data.id + "." + suffix),
    "Vakanın kendi kapak metni eksik: " + data.id + "." + suffix);

  // Vaka kendi ortam sesini söyleyebilir; söylediyse dosyası olmalı. Eksik
  // klip sessiz geçtiği için yazım hatası başka hiçbir yerde duyulmaz.
  report.Require(data.deskHour >= -1 && data.deskHour <= 23, "Vakanın masa saati 0-23 arasında olmalı: " + data.deskHour);
  report.Require(string.IsNullOrEmpty(data.weather) || data.weather == "rain", "Bilinmeyen hava: " + data.weather);
  if (!string.IsNullOrEmpty(data.ambienceId))
   report.Require(Resources.Load<AudioClip>(AudioDirector.Folder + data.ambienceId) != null,
    "Vakanın ortam sesi yok: " + AudioDirector.Folder + data.ambienceId);

  // Olay yeri planı: engeller kapalı çokgen, her şey planın içinde, kilitler var olan
  // kaynak ve sorulara bağlı. Plan bir belgedir; oyuncu onu masadan açar.
  var askable = nodes.SelectMany(n => n.questions ?? new Question[0]).Select(q => q.id).ToHashSet();
  bool Inside(float x, float y) => x >= 0 && x <= 1 && y >= 0 && y <= 1;
  foreach (var host in nodes.Where(n => n.HasScenePlan)) {
   var plan = host.scenePlan;
   report.Require(host.kind == "document", "Olay yeri planı yalnız belgede durur: " + host.id);
   report.Require(plan.incident != null && Inside(plan.incident.x, plan.incident.y), "Planın olay noktası eksik ya da dışarıda: " + host.id);
   report.Require(!MissingText(locale, plan.incidentLabelKey), "Planın olay noktası etiketi eksik: " + host.id);
   if (!string.IsNullOrEmpty(plan.imagePath))
    report.Require(Resources.Load<Texture2D>(plan.imagePath) != null, "Plan görseli yok: " + plan.imagePath);
   foreach (var o in plan.occluders ?? new PlanOccluder[0])
    report.Require(o.points != null && o.points.Length >= 3 && o.points.All(p => Inside(p.x, p.y)),
     "Plan engeli kapalı çokgen değil ya da dışarıda: " + host.id + "/" + o.id);
   var markers = plan.markers ?? new PlanMarker[0];
   report.Require(markers.Length > 0 && markers.Select(m => m.id).Distinct().Count() == markers.Length, "Planın işaretleri eksik ya da yinelenmiş: " + host.id);
   foreach (var m in markers) {
    report.Require(Inside(m.x, m.y) && m.fov > 0 && m.fov <= 360, "Plan işareti geçersiz: " + m.id);
    report.Require(!MissingText(locale, m.labelKey), "Plan işaretinin etiketi eksik: " + m.id);
    report.Require(m.kind == "claimed" || m.kind == "recorded", "Plan işaretinin türü claimed/recorded olmalı: " + m.id);
    report.Require((m.requiresAsked ?? new string[0]).All(askable.Contains), "Plan işareti olmayan soruya bağlı: " + m.id);
    report.Require((m.requiresRead ?? new string[0]).All(id => nodes.Any(n => n.id == id)), "Plan işareti olmayan kaynağa bağlı: " + m.id);
   }
  }

  // Rapor sihirbazının üç sütunu da tam olarak bir doğru seçenek içermeli.
  // Sıfır olursa vaka çözülemez, birden fazla olursa değerlendirme keyfîleşir.
  // Bu kontrol 25 Eylül 2026 denetiminde elle yapıldı; burada kalıcılaşıyor.
  RequireExactlyOneCorrect(data.verdicts?.Select(v => v.correct), "şüpheli", report);
  RequireExactlyOneCorrect(data.methods?.Select(m => m.correct), "yöntem", report);
  RequireExactlyOneCorrect(data.evidence?.Select(e => e.correct), "kanıt", report);
  // Dördüncü sütun isteğe bağlıdır: tanımlanmadıysa hiç sorulmaz. Tanımlandıysa
  // diğer sütunlarla aynı kurala uyar ve kendi etiketini taşımak zorundadır.
  if ((data.custody ?? new Choice[0]).Length > 0) {
   RequireExactlyOneCorrect(data.custody.Select(c => c.correct), "ikinci sorumluluk", report);
   foreach (var choice in data.custody)
    report.Forbid(MissingText(locale, choice.labelKey), "İkinci sorumluluk etiketi eksik: " + choice.id);
   report.Forbid(!string.IsNullOrEmpty(data.custodyLabelKey) && MissingText(locale, data.custodyLabelKey),
    "İkinci sorumluluk sütununun başlığı eksik: " + data.custodyLabelKey);
   foreach (var key in new[] { data.suspectLabelKey, data.methodLabelKey }.Where(k => !string.IsNullOrEmpty(k)))
    report.Forbid(MissingText(locale, key), "Rapor sütunu başlığı eksik: " + key);
  }

  foreach (var id in (data.verdicts ?? new Verdict[0]).SelectMany(v => v.supportingSourceIds ?? new string[0])
    .Concat((data.methods ?? new Choice[0]).SelectMany(v => v.supportingSourceIds ?? new string[0]))
    .Concat((data.evidence ?? new Choice[0]).SelectMany(v => v.supportingSourceIds ?? new string[0]))
    .Concat((data.custody ?? new Choice[0]).SelectMany(v => v.supportingSourceIds ?? new string[0]))) {
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

  ValidateEpilogues(data, locale, report);
  ValidatePressure(data, locale, report);
  ValidateTimeline(data, locale, report);
  foreach (var node in nodes) ValidateNode(data, node, locale, report);
 }

 // Akıbet isteğe bağlıdır, ama bir sütunda bir seçenek yazdıysa hepsi yazmalı:
 // yoksa faks bazı yanlış suçlamaların bedelini anlatır, bazılarını anlatmaz ve
 // eksiklik kendisi bir işarete dönüşür.
 static void ValidateEpilogues(CaseData data, Locale locale, ValidationReport report) {
  var verdicts = data.verdicts ?? new Verdict[0];
  if (verdicts.Any(v => !string.IsNullOrEmpty(v.epilogueKey)))
   foreach (var v in verdicts)
    report.Forbid(MissingText(locale, v.epilogueKey), "Akıbet metni eksik: şüpheli " + v.id);
  var custody = data.custody ?? new Choice[0];
  if (custody.Any(c => !string.IsNullOrEmpty(c.epilogueKey)))
   foreach (var c in custody)
    report.Forbid(MissingText(locale, c.epilogueKey), "Akıbet metni eksik: ikinci sorumluluk " + c.id);
 }

 // Baskı unsuru: bir görüşme belli kaynaklar okununca kapanabilir. Kapanan
 // kaynak doğru sonucun dayandığı hiçbir yolda olamaz; yoksa oyuncu sırayla
 // oynadı diye vakayı çözemez hâle gelir. Doğru seçeneklerin dayanakları,
 // kapanış önkoşulları ve bunların bütün önkoşul zinciri korunur.
 public static System.Collections.Generic.HashSet<string> EssentialNodes(CaseData data) {
  var nodes = data.nodes ?? new Node[0];
  var seeds = (data.verdicts ?? new Verdict[0]).Where(v => v.correct).SelectMany(v => v.supportingSourceIds ?? new string[0])
   .Concat(new[] { data.methods, data.evidence, data.custody }.SelectMany(list => (list ?? new Choice[0]).Where(c => c.correct))
    .SelectMany(c => c.supportingSourceIds ?? new string[0]))
   .Select(id => id.Split('#')[0]).Concat(data.conclusionRequires ?? new string[0]);
  var essential = new System.Collections.Generic.HashSet<string>();
  var queue = new System.Collections.Generic.Queue<string>(seeds);
  while (queue.Count > 0) {
   var id = queue.Dequeue();
   if (!essential.Add(id)) continue;
   var node = nodes.FirstOrDefault(n => n.id == id);
   if (node == null) continue;
   var asked = (node.requiresAsked ?? new string[0]).Concat(node.requiresAnyAsked ?? new string[0])
    .Concat((node.questions ?? new Question[0]).SelectMany(q => (q.requiresAsked ?? new string[0]).Concat(q.requiresAnyAsked ?? new string[0])));
   var askedOwners = asked.SelectMany(q => nodes.Where(n => (n.questions ?? new Question[0]).Any(x => x.id == q)).Select(n => n.id));
   foreach (var next in (node.requires ?? new string[0]).Concat(node.requiresAny ?? new string[0])
    .Concat((node.questions ?? new Question[0]).SelectMany(q => q.requiresRead ?? new string[0])).Concat(askedOwners))
    queue.Enqueue(next);
  }
  return essential;
 }

 static void ValidatePressure(CaseData data, Locale locale, ValidationReport report) {
  var closing = (data.nodes ?? new Node[0]).Where(n => n.closesAfterRead != null && n.closesAfterRead.Length > 0).ToArray();
  if (closing.Length == 0) return;
  var essential = EssentialNodes(data);
  foreach (var node in closing) {
   report.Require(node.kind == "interview", "Yalnız görüşme kapanabilir: " + node.id);
   report.Forbid(essential.Contains(node.id), "Doğru sonucun dayandığı görüşme kapanamaz: " + node.id);
   report.Forbid(node.closesAfterRead.Any(id => !data.nodes.Any(n => n.id == id) || id == node.id),
    "Bilinmeyen kapanış kaynağı: " + node.id);
   report.Forbid(!string.IsNullOrEmpty(node.closedNoteKey) && MissingText(locale, node.closedNoteKey),
    "Kapanış notu metni eksik: " + node.id);
  }
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
  foreach (var item in node.relatedItems ?? new RelatedItem[0]) {
   foreach (var key in new[] { item.nameKey, item.detailKey })
    report.Forbid(MissingText(locale, key), "İlgili eşya metni eksik: " + key);
   report.Require(!string.IsNullOrEmpty(item.imageResource) && Resources.Load<Texture2D>(item.imageResource) != null,
    "İlgili eşya görseli yok: " + item.imageResource);
  }

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
   report.Note("Üretilmiş yedek portre kullanılıyor: " + node.personId +
    (node.portrait == null ? " (vaka verisinde `portrait` yok, varsayılan tonlar)" : string.Empty));

  // Yedek portrenin tonları vaka verisinden gelir. Yanlış yazılmış bir renk
  // ekranda magenta bir yüz olarak görünür; burada yakalanması daha iyi.
  if (node.portrait != null)
   foreach (var pair in new[] {
    new[] { "hairHex", node.portrait.hairHex },
    new[] { "skinHex", node.portrait.skinHex },
    new[] { "shirtHex", node.portrait.shirtHex },
   })
    report.Forbid(!string.IsNullOrEmpty(pair[1]) && !ColorUtility.TryParseHtmlString(pair[1], out _),
     "Portre rengi okunamıyor (" + node.personId + "." + pair[0] + "): " + pair[1]);

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
   report.Forbid(!string.IsNullOrEmpty(question.presentedSourceId) &&
    (data.nodes.FirstOrDefault(x => x.id == question.presentedSourceId)?.notPresentable ?? false),
    "Belirleyici kaynak görüşmede öne sürülemiyor: " + question.id);

   // Tek belirleyici kaynak da (`presentedSourceId`) aynı süzgeçten geçer; eskiden
   // yalnız `presentedSourceIds` içindeki `#` ayrıntılı kayıtlar denetleniyordu ve
   // adı geçmeyen bir belge (Dosya #002 muayene raporu) soruyu kilitliyordu.
   var singles = string.IsNullOrEmpty(question.presentedSourceId) ? new string[0] : new[] { question.presentedSourceId };
   foreach (var sourceRef in singles.Concat(question.presentedSourceIds ?? new string[0]).Distinct()) {
    var separator = sourceRef.IndexOf('#');
    var source = data.nodes.FirstOrDefault(x => x.id == (separator < 0 ? sourceRef : sourceRef.Substring(0, separator)));
    if (!report.Step(source != null, "Bilinmeyen görüşme kaynağı: " + question.id + " → " + sourceRef)) continue;
    if (separator < 0) {
     report.Forbid(!Concerns(data, node, source, null, locale),
      "Belirleyici kaynak bu kişiye kapalı (adı geçmiyor, aboutPersonIds de yok): " + question.id + " → " + sourceRef);
     continue;
    }
    var detail = sourceRef.Substring(separator + 1);
    report.Forbid(
     source.kind == "cctv" && !(source.cctvEvents ?? new CctvEvent[0]).Any(e => e.id == detail) ||
     source.kind == "interview" && !(source.questions ?? new Question[0]).Any(other => other.id == detail.Split('|')[0]) ||
     source.kind != "cctv" && source.kind != "interview",
     "Bilinmeyen görüşme kaynağı ayrıntısı: " + question.id + " → " + sourceRef);

    // Kaynak seçici artık kaynakları `aboutPersonIds` ile süzüyor. Belirleyici bir
    // kaynak karşımızdaki kişiyle etiketlenmemişse listede hiç görünmez ve soru
    // yanıtlanamaz hale gelir — vaka çözülemez olur. Bu, göz kaçırmayı imkânsız
    // kılan türden bir kontrol: etiketi eklemeyi unutan kişiyi burada yakalar.
    report.Forbid(!Concerns(data, node, source, detail, locale),
     "Belirleyici kaynak bu kişiye kapalı (adı geçmiyor, aboutPersonIds de yok): "
      + question.id + " → " + sourceRef);
    report.Forbid(source.notPresentable || source.kind == "cctv" &&
      ((source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e => e.id == detail)?.notPresentable ?? false),
     "Belirleyici kaynak görüşmede öne sürülemiyor: " + question.id + " → " + sourceRef);

    // Kaynak sunulan bir soru, kaynağın içeriğini kendi metninde tekrar etmemeli:
    // ettiği anda çelişkiyi oyuncu yerine oyun kurmuş olur. Bu sezgisel bir
    // kontroldür (ortak dört sözcüklük dizi arar), o yüzden not olarak raporlanır.
    var sourceText = source.kind == "cctv"
     ? Text(locale, (source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e => e.id == detail)?.textKey)
     : Text(locale, (source.questions ?? new Question[0]).FirstOrDefault(o => o.id == detail.Split('|')[0])?.answerKey);
    var echo = SharedPhrase(Text(locale, question.promptKey), sourceText);
    if (echo != null)
     report.Note("Soru metni kaynağı tekrar ediyor, çelişkiyi oyuncu kurmalı (\"" + echo + "\"): " + question.id);
   }

   // Yem kaynaklar. Bir yem, soruyu kapatan kaynaklardan biri OLMAMALI (olursa
   // yanlış yola sapan oyuncu vakayı çözmüş sayılır) ve o kişiye görünür olmalı
   // (görünmezse yazılan yanıt oyunda hiç çıkmaz).
   // İki yem aynı yanıt metnini paylaşırsa, ikisinden biri için o cümle
   // kaçınılmaz olarak yersiz düşer. (Bu kural bir kopyala-yapıştır hatasıyla
   // doğdu: `hasan_follow.gap`'in iki ayrı yemi aynı anahtara bakıyordu.)
   foreach (var pair in (question.decoyAnswers ?? new PresentedAnswer[0])
     .GroupBy(x => x.answerKey).Where(g => g.Count() > 1))
    report.Problem("İki yem aynı yanıtı paylaşıyor: " + question.id + " → " + pair.Key);

   foreach (var decoy in question.decoyAnswers ?? new PresentedAnswer[0]) {
    report.Forbid(MissingText(locale, decoy.answerKey), "Yem yanıt metni eksik: " + decoy.answerKey);
    report.Forbid(decoy.sourceId == question.presentedSourceId ||
     (question.presentedSourceIds ?? new string[0]).Contains(decoy.sourceId),
     "Yem kaynak aynı zamanda çözücü kaynak: " + question.id + " → " + decoy.sourceId);
    var mark = decoy.sourceId.IndexOf('#');
    var host = data.nodes.FirstOrDefault(x => x.id == (mark < 0 ? decoy.sourceId : decoy.sourceId.Substring(0, mark)));
    if (!report.Step(host != null, "Bilinmeyen yem kaynağı: " + question.id + " → " + decoy.sourceId)) continue;
    report.Forbid(host.kind == "interview" && host.personId == node.personId,
     "Yem kaynak kişinin kendi ifadesi: " + question.id + " → " + decoy.sourceId);
    var tail = mark < 0 ? null : decoy.sourceId.Substring(mark + 1);
    // Belge yemleri de denetlenir: eskiden `mark < 0` olunca bu kontrol atlanıyordu,
    // çünkü belgeler hiç süzülmüyordu. Artık ad geçme kuralı onlara da işliyor.
    report.Forbid(!Concerns(data, node, host, tail, locale),
     "Yem kaynak bu kişiye görünmüyor: " + question.id + " → " + decoy.sourceId);
    if (mark < 0) continue;
    report.Forbid(host.notPresentable || host.kind == "cctv" &&
      ((host.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e => e.id == tail)?.notPresentable ?? false),
     "Yem kaynak görüşmede öne sürülemiyor: " + question.id + " → " + decoy.sourceId);
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
  {
   if (!string.IsNullOrEmpty(record.videoPath))
    report.Require(File.Exists(Path.Combine(Application.streamingAssetsPath, record.videoPath)),
     "CCTV görüntüsü yok: " + record.videoPath);
   // Kare dizisi: her kare Resources altında bir görsel olmalı; damga verildiyse kare sayısıyla eşleşmeli.
   foreach (var frame in record.framePaths ?? new string[0])
    report.Require(!string.IsNullOrEmpty(frame) && Resources.Load<Texture2D>(frame) != null,
     "CCTV karesi yok: " + frame + " (" + node.id + "#" + record.id + ")");
   if (record.frameTimes != null && record.frameTimes.Length > 0)
    report.Require(record.framePaths != null && record.frameTimes.Length == record.framePaths.Length,
     "CCTV kare damgası sayısı kare sayısını tutmuyor: " + node.id + "#" + record.id);
   report.Forbid(record.frameMs < 0, "Negatif CCTV kare süresi: " + node.id + "#" + record.id);
  }
  foreach (var key in new[] { node.cctvSourceKey, node.cctvPeriodKey }
    .Concat(new[] { node.cctvOverlayKey }.Where(k => !string.IsNullOrEmpty(k)))
    .Concat(events.SelectMany(e => new[] { e.textKey, e.overlayTimeKey, e.glitchKey, e.signalKey })
     .Where(k => !string.IsNullOrEmpty(k))))
   report.Forbid(MissingText(locale, key), "CCTV metni eksik: " + key);
 }
 // İki metinde ortak geçen üç sözcüklük ilk dizi; yoksa null. Eşik üçtür çünkü
 // dördü, yakalamak istediğimiz gerçek ihlali ("Mert, kapıda kaldığında ona
 // yardım ettiğinizi… söyledi") kaçırıyordu. Dosya #001'in mevcut metinlerinde
 // üç sözcükle yanlış alarm yok.
 static string SharedPhrase(string left, string right) {
  if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right)) return null;
  var a = Words(left); var b = Words(right);
  for (int i = 0; i + 3 <= a.Length; i++) {
   var phrase = string.Join(" ", a.Skip(i).Take(3));
   if ((" " + string.Join(" ", b) + " ").Contains(" " + phrase + " ")) return phrase;
  }
  return null;
 }
 // Kaynak seçicinin süzgecinin aynısı: satır listede görünüyor mu?
 static bool Concerns(CaseData data, Node subject, Node source, string detail, Locale locale) {
  var game = new Investigation(data) { Text = locale };
  if (source.kind == "cctv" && detail != null) {
   var record = (source.cctvEvents ?? new CctvEvent[0]).FirstOrDefault(e => e.id == detail);
   return game.SourceConcernsPerson(subject, record?.aboutPersonIds, Text(locale, record?.textKey));
  }
  if (source.kind == "interview" && detail != null) {
   var asked = (source.questions ?? new Question[0]).FirstOrDefault(o => o.id == detail.Split('|')[0]);
   return game.SourceConcernsPerson(subject, asked?.aboutPersonIds,
    Text(locale, asked?.promptKey) + " " + Text(locale, asked?.answerKey));
  }
  return game.SourceConcernsPerson(subject, source.aboutPersonIds,
   Text(locale, source.titleKey) + " " + Text(locale, source.bodyKey));
 }

 static string[] Words(string text) =>
  Regex.Split(text.ToLowerInvariant(), @"[^\p{L}\p{N}]+").Where(w => w.Length > 0).ToArray();
}

}
