# Karine — oyun adı kararı

**Durum: KARAR VERİLDİ — Karine (25 Eylül 2026).** Aşağıdaki ölçütler ve aday havuzu karar kaydı olarak saklanır.
`bube` **çalışma adıdır**; stüdyo adı `bubeGames`, oyun içi kurum `bube Police / BPS` olarak kalır. Bu dosya yalnız **mağazada görünecek oyun adını** takip eder.

> `DESIGN_DECISIONS.md`: "Nihai oyun adı daha sonra seçilecek; adlar yapılandırmadan değiştirilebilir olmalı." Bu karar bugüne kadar hiçbir dosyada takip edilmiyordu.

## Ölçütler

1. **Dedektiflik çağrıştırmalı** ama "Detective / Case / Files / Crime" gibi App Store ve Google Play'de yüzlerce sonucu olan jenerik kelimelerden kaçınmalı.
2. **Aranabilir olmalı.** Mağaza aramasında ilk sonuç oyun olmalı; ortak isimli bir kelime olmamalı.
3. **Oyunun tezini taşımalı.** Bu oyunun konusu kovalamaca değil; **ifadeleri ve kayıtları karşılaştırıp gerekçeli bir kanaate varmak**. İyi ad bunu ima eder.
4. **Yedi dünyaya ölçeklenmeli.** İsim Türkiye'ye çivilenmemeli; Dünya 2–7 sonradan gelecek.
5. **Telaffuz edilebilir olmalı.** Türkçe kökenli olabilir, ama yabancı bir oyuncu okuyabilmeli.
6. **Kısa.** Mağaza ikonunun altında kesilmemeli: 1–2 kelime, tercihen ≤12 karakter.

## Aday havuzu — **hiçbiri doğrulanmadı**

Aşağıdaki adların mağaza/alan adı/marka müsaitliği **kontrol edilmedi**. Karardan önce her aday için "Müsaitlik kontrol listesi" işletilmeli.

### A. Hukuk/kayıt dili — oyunun tezine en yakın
| Aday | Anlam / neden |
| --- | --- |
| **Karine** | Hukukta "dolaylı kanıttan çıkarılan sonuç". Oyunun tam olarak yaptığı şey. Kısa, yumuşak, uluslararası okunabilir. En güçlü aday. |
| **Emare** | "İz, belirti". Kanıt değil *işaret* olması oyunun "yalan ≠ suçluluk" ilkesiyle örtüşüyor. |
| **Kanaat** | "Gerekçeli vicdani kanı". Oyuncunun rapor gönderirken yaptığı şey. |
| **Zabıt** | "Resmî tutanak". Sert, kurumsal, dosya hissi güçlü. |

### B. Kurum/polis dili — daha düz, daha tanıdık
| Aday | Not |
| --- | --- |
| **Vukuat** | "Olay/hadise". Çok Türk polis dili; uluslararası telaffuz zor. |
| **Tutanak** | Güçlü dosya çağrışımı; jenerikleşme riski yüksek. |
| **Müzekkere** | Özgün ama uzun ve okunması zor. |

### C. Türkçe olmayan, atmosfer odaklı
| Aday | Not |
| --- | --- |
| **Nightdesk** | Bora'nın gece masası. Ölçüt 3'ü değil, ölçüt 4'ü iyi karşılıyor. |
| **Hearsay** | "Kulaktan dolma delil" — tema mükemmel, ama yaygın kelime (ölçüt 2 riskli). |
| **The Quiet File** | Atmosferik; üç kelime (ölçüt 6'ya aykırı). |

### D. Alt başlıklı yapı
Ana ad + vaka numarası, seriye açık bir yapı kurar: **`Karine — Dosya #001`**. Mağaza adı kısa kalır, bölümler alt başlıkla ilerler.

## Müsaitlik kontrol listesi (aday başına)

- [ ] App Store araması — aynı/benzer adda oyun var mı?
- [ ] Google Play araması — aynısı.
- [ ] Steam araması (ileride PC düşünülürse).
- [ ] Türkiye ve AB marka (TÜRKPATENT / EUIPO) kaba araması — 9. ve 41. sınıf.
- [ ] `.com` ve `.games` alan adı durumu.
- [ ] Sosyal medya kullanıcı adı (X, Instagram, TikTok, YouTube).
- [ ] Türkçe ve İngilizce arama motorunda ad + "game" — ilk sayfa temiz mi?
- [ ] İstenmeyen anlam kontrolü: adın diğer dillerde kötü/komik karşılığı var mı?

## Karar verilince yapılacaklar

Ad tek bir yerde değil, **dört yerde birden** değişir:
1. `Assets/Bube/Resources/Bube/config.json` → `title`
2. `ProjectSettings/ProjectSettings.asset` → `productName`
3. `ProjectSettings` → `applicationIdentifier` (ör. `com.bubedigital.<ad>`)
4. `tr.json` içindeki oyuncuya görünen başlık anahtarları

Stüdyo adı (`bubeGames`) ve oyun içi kurmaca kurum adı (`bube Polis` / `BPS`) **değişmez** — 25 Eylül 2026 kararı, bkz. [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md). Çalışma klasörü `bubeGame/karine-mobile`, depo `github.com/aydinbt/Karine`.
