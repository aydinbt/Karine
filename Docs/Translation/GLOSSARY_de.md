# Almanca sözlük (de)

Türkçe kanondur. Bu sözlük Almanca metnin tutarlılığı içindir; yeni dosya çevrilirken buna uyulur. Denetim: `python3 Tools/check-translation.py de` → 0 sorun (9 Ekim 2026: 12.833 satırın tamamı).

## Yazım
- Hitap: anlatı, sistem ve sorgu metninde **Sie**. Kanon bir gence ya da yakınına senli konuşuyorsa **du** (ör. #053 Kaya, #061 Anika, #072 oğula soru). `summary.lesson` oyuncuya **du** ile konuşur.
- Kesme işareti ve alıntı: `’` ve `‘…’` (kanondaki gibi).
- Saat Türkçedeki gibi noktayla: `08.27`. Tarih: `13. November 2030`.
- Adlar değişmez. **Ad + iyelik eki yazılmaz** (Tamers Auto değil): `das Auto von Tamer`, `der Fingerabdruck von X`. Denetleyici adların kanonla aynı satırda geçmesini zorlar.
- `Bey / Hanım` → `Herr / Frau`; aile içi hitap `amca/teyze` → `Onkel/Tante` yalnız hitapta.
- Yabancı sözcükler ve kurum adları çevrilmez (Laurentide Réassurance, Pacific Liaison Trust, Fundación Santa Ana).
- Gerçek kurum adı yok: savcılık → **Anklagebehörde** (asla „Staatsanwaltschaft“).

## Kurum ve dosya
| Türkçe | Almanca |
|---|---|
| bube (kurum) | bube (küçük harf korunur) |
| şube | Dienststelle |
| (şube şefinin onayıyla) | (mit Genehmigung der Dienststellenleitung) |
| Dosya #0NN | Akte #0NN |
| DOSYA #0NN  /  BUBE ŞEHİR — SEMT | AKTE #0NN  /  BUBE STADT — VIERTEL |
| BDS Sevilla — Triana Şubesi | BDS Sevilla — Dienststelle Triana |
| BDM Montreal — Mile End Şubesi | BDM Montreal — Dienststelle Mile End |
| BDA Melbourne — Footscray Şubesi | BDA Melbourne — Dienststelle Footscray |
| Uluslararası İrtibat Programı | Internationales Verbindungsprogramm |
| Bölge Koordinatörü | Regionalkoordinator(in) |

## Belge başlıkları
| Türkçe | Almanca |
|---|---|
| OLAY RAPORU | VORFALLSBERICHT |
| OTOPSİ RAPORU | OBDUKTIONSBERICHT |
| … — kayıt dökümü | … — Aufzeichnungsprotokoll |
| … — ARAMA | … — DURCHSUCHUNG |
| … İNCELEMESİ | UNTERSUCHUNG … |
| X — görüşme | X — Befragung |
| Kamera: … | Kamera: … |
| Ölüm nedeni / Ölüm zamanı | Todesursache / Todeszeit |
| HAT A — … | LINIE A — … |
| … hattını aç | …linie öffnen |
| … iste | … anfordern |
| … aramak için izin iste | Durchsuchungsbeschluss für … beantragen |
| arşivden açmak için izin iste | Genehmigung beantragen, … aus dem Archiv zu öffnen |

## Dosya türleri ve sorular
| Türkçe | Almanca |
|---|---|
| ŞÜPHELİ ÖLÜM | VERDÄCHTIGER TODESFALL |
| CİNAYET | MORD |
| ZEHİRLEME | VERGIFTUNG |
| Nasıl öldü? | Wie ist er gestorben? / Wie ist sie gestorben? |
| X’i kim öldürdü? | Wer hat X getötet? |
| … nereye gitti? | Wohin kam …? |
| Kimse — … | Niemand — … |
| … tutuklandı / sorgulandı | … wurde festgenommen / wurde vernommen |
| … bulunduğu yazıldı | Festgehalten wurde, dass … gefunden wurde. |
| yem (kanıt) | Köder |

## Davranış satırları (`.demeanor`)
Gözlem yazılır, yorum yazılmaz; şimdiki zaman, öznesiz („Dreht den Helm …“). Hüküm sözcükleri (Lüge, Wahrheit, schuldig, unschuldig, versteckt, Angst, erleichtert, Widerspruch vb.) **yasak**; denetleyici yakalar.
