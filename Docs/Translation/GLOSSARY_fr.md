# Fransızca sözlük (fr)

Türkçe kanondur. Bu sözlük Fransızca metnin tutarlılığı içindir; yeni dosya çevrilirken buna uyulur. Denetim: `python3 Tools/check-translation.py fr` → 0 sorun.

## Yazım
- Hitap: anlatı, sistem ve sorgu metninde **vous**. Kanon bir gence ya da yakınına senli konuşuyorsa **tu**. `summary.lesson` oyuncuya **tu** ile konuşur.
- Kesme işareti `’`, alıntı `« … »` (kanonda `‘…’` varsa `« … »`). `:` `;` `?` `!` öncesi boşluk (ince boşluk yerine düz boşluk).
- Saat Türkçedeki gibi noktayla: `08.27`. Tarih: `13 novembre 2030`.
- Adlar değişmez. **Ad + iyelik yazılmaz**: `la voiture de Tamer`, `l’empreinte de X`. Denetleyici adların kanonla aynı satırda geçmesini zorlar.
- `Bey / Hanım` Türkiye dosyalarında korunur (`Hasan Bey`); diğer ülkelerde `M. / Mme`.
- Yabancı sözcükler ve kurum adları çevrilmez (Laurentide Réassurance, Pacific Liaison Trust).
- Gerçek kurum adı yok: savcılık → **le ministère public** / **l’autorité de poursuite** (asla « parquet de Paris », « police judiciaire », « brigade criminelle », « gendarmerie »).
- Kimliği belirsiz kişi: `la personne`, `quelqu’un`, `un individu` — cinsiyet verilmez.

## Kurum ve dosya
| Türkçe | Fransızca |
|---|---|
| bube Departman | Département bube (küçük harf korunur) |
| Uluslararası Soruşturma Birimi | Unité d’enquête internationale |
| şube | antenne |
| Dosya #0NN | Dossier #0NN |
| Bora’nın masası | Le bureau de Bora |
| Gelen evrak | Courrier entrant |
| Sonuç raporu | Rapport final |
| Değerlendirme | Évaluation |
| Kurum güveni | Confiance du département |
| BDS | BDS (değişmez) |

## Belge başlıkları
| Türkçe | Fransızca |
|---|---|
| OLAY RAPORU | RAPPORT D’INCIDENT |
| OTOPSİ RAPORU | RAPPORT D’AUTOPSIE |
| … — kayıt dökümü | … — relevé d’enregistrement |
| X — İlk ifade | X — Première déposition |
| X — görüşme / takip görüşmesi | X — audition / audition de suivi |
| … — ARAMA | … — PERQUISITION |
| … iste | demander … |

## Sorular ve sonuç
| Türkçe | Fransızca |
|---|---|
| Şüpheli | Suspect |
| Belirleyici kanıt | Preuve décisive |
| Belirlenemedi | Indéterminé |
| yem (kanıt) | leurre |
| SİNYAL KESİLDİ / KAYIT BULUNAMADI | SIGNAL PERDU / AUCUN ENREGISTREMENT |
| Bir ifade kanıt değildir. | Une déposition n’est pas une preuve. |

## Davranış satırları (`.demeanor`)
Gözlem yazılır, yorum yazılmaz; şimdiki zaman, öznesiz (« Tourne le casque entre ses mains. »). Hüküm sözcükleri (mensonge, vérité, coupable, innocent, cache, dissimule, sincère, peur, paniqué, soulagé, contradiction vb.) **yasak**; denetleyici yakalar.
