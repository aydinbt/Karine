# Dosya #004 — Sessiz Kat

Bu sayfa içerik ekibi içindir; oyuncuya gösterilmez. **Durum: taslak (4 Ekim 2026).** Senaryo kullanıcıdan geldi. Aşağıdaki düzeltmeler önerimdir; kullanıcı onaylayınca veri (`case004.json`, `tr.case004.json`) yazılır.

**Büro:** BDS — şüpheli ölüm / olası cinayet. **Yer/zaman:** Şişli, altı katlı bir apartman, 9 Ocak 2027 gecesi (#003'ün 12 Aralık'ından sonra). **Zorluk:** orta–yüksek. **Hedef süre:** 25–30 dk.
**Yeni yetenek:** CCTV cevabı vermez. Oyuncu zaman çizelgesini **üç bağımsız kaynaktan** (otopark kaydı, telefon hareketi, servis kapısı) kendisi yeniden kurar.

## Eğitim eğrisi

- 001: Yalan söyleyen kişi ≠ fail.
- 002: Yaralayan ≠ parayı alan.
- 003: Olay yerini değiştiren ≠ ölüme sebep olan.
- 004: **O gece orada olan ≠ ölüm anında orada olan.** Kanıt yalan söylemez; yanlış zamana oturtulan yorum yanıltır.

Bundan sonraki vakaların standardı: görünen olay + saklanan ikinci suç + farklı sebeplerle yalan söyleyen birkaç kişi + zaman çizelgesini yeniden kurma.

## Kişiler

| Kişi | Yaş | Kim | Ne saklıyor |
|---|---|---|---|
| Tolga Ergin | 38 | Ölen. Lojistik şirketinde muhasebe sorumlusu. | — |
| Cem Gür | 41 | Eski iş arkadaşı, şirketten bir yıl önce ayrıldı. | O gece dairedeydi. Zimmeti ilk o fark edip Tolga'ya belge vermişti; adı karışsın istemiyor. |
| Nihat Sönmez | 52 | Şirketin operasyon müdürü. | Zimmet onun. Ölüm anında dairedeydi, telefonu aldı. |
| Derya Aksoy | 35 | Tolga'nın eski sevgilisi. | Akşamki sert mesajlar; kıskançlık değil, Tolga'ya verdiği borç. Utandığı için küçümsüyor. |
| Gülay Tan | 67 | Alt kat (5. kat) komşusu, acil çağrıyı yapan. | Hiçbir şey; ama saatleri kesin bilmiyor. |

## Kesin olay

1. 01.31: Tolga apartmana ön kapıdan yalnız girer (Kamera 01).
2. 01.38: Cem arka otoparktan, yan binayla ortak **servis geçidinden** girer. Kamera 02 (arka) haftalardır arızalıdır; o gece 01.46'da yine düşer.
3. Cem ile Tolga kahve içer, tartışır: Cem belgeleri geri ister, "adımı verme" der. Tolga reddeder.
4. ~01.55: Cem servis geçidinden çıkar. Fincanı masada kalır.
5. 01.57: Nihat'ın aracı arka otoparka girer (otopark plaka kaydı).
6. 02.03: Servis kapısı hareket algılar (kapı sensörü; kamera değil, kim olduğu yok).
7. Nihat daireye çıkar. Tolga'yı tehdit eder; Tolga geri çekilirken açık pencerenin önündeki alçak sehpaya takılır ve düşer. **İtilmez.**
8. 02.17: Gülay Tan acil çağrı yapar.
9. Nihat Tolga'nın telefonunu (zimmet yazışmaları içinde) alır, kapıyı çekip çıkar. Kapı **çekince kendiliğinden kilitlenen** gömme kilitlidir.
10. 02.21: Tolga'nın telefonu apartmandan uzaklaşmaya başlar (cihaz hareket raporu).
11. 02.23: Nihat'ın aracı otoparktan çıkar.
12. 02.24: Kamera 02 sinyali geri gelir.

## Senaryoya önerdiğim düzeltmeler

- **"Kapı içeriden kilitliydi" çözülmeli.** Yoksa Nihat'ın çıkışı imkânsız olur ve vaka intihara kilitlenir. Öneri: tutanak "kilitli, zorlama yok" der (dürüst gözlem); **kilit inceleme notu** kapının dışarıdan çekilince kendiliğinden kilitlendiğini gösterir. Bu, "intihar" teorisini yıkan ilk fiziksel kaynaktır.
- **Kamera 02'nin tam o saatte düşmesi tesadüf gibi durmamalı.** Öneri: kamera haftalardır aralıklı düşüyor; **yönetim bakım kaydı** bunu belgeler. Böylece "biri kamerayı kapattı" yanlış yolu açılır ve kanıtla kapanır; servis geçidinin kör nokta olduğu da buradan öğrenilir.
- **Fincanların zamanı netleşmeli.** "Önceki ziyaret" günler önce değil, **aynı gece Nihat'tan önce**: Cem'in fincanıdır. Fincan Cem'in orada olduğunu kanıtlar, ölüm anını değil. Nihat hiçbir şey içmez — bu yüzden ikinci kişinin izi fincanda değil, otopark ve telefonda.
- **Komşu tanık eklensin (Gülay Tan).** Acil çağrıyı biri yapıyor; görüşülebilir olması iyi. 02.10 civarı "iki erkek sesi, biri bağırıyordu" der. Cem 01.55'te çıktığı için bu ses Nihat'ındır — ama Gülay saati "gece ikiyi geçiyordu" diye bilir, kesin değildir; tek başına kimseyi göstermez.
- **Derya'nın açığı kanıtla kapanmalı.** Mesajlar kişisel cinayet yolunu açar; **taksi/uygulama yolculuk kaydı** onu 01.40–02.40 arası Kadıköy'de gösterir. Mesajların konusu borçtur (Derya ifadesinin ikinci aşamasında kabul eder).
- **Rapor dört sütuna sığmalı** (motor değişmez, #003 kararıyla aynı ilke):

| Sütun | Soru (`*LabelKey`) | Seçenekler | Doğru |
|---|---|---|---|
| `verdicts` | Düştüğü anda dairede kim vardı? | Nihat / Cem / Derya / Kimse (yalnızdı) | Nihat |
| `methods` | Tolga nasıl düştü? | Tartışmada geri çekilirken dengesini kaybetti / İtildi / Kendisi atladı / Yalnızken kaza | Geri çekilirken dengesini kaybetti |
| `custody` | Telefonu kim aldı? | Nihat / Cem / Derya / Kimse, düşerken kayboldu | Nihat |
| `evidence` | Dayanak | otopark + telefon hareketi + servis kapısı + kilit notu | — |

  Zimmet ve Cem'in rolü ayrı soru değildir: güdü olarak hesap incelemesinden, sonuç olarak faksta anlatılır (#003'te Ozan'ın zimmeti gibi).
- **Nihat "itti" seçeneğinde yanlış ama yakın sayılmalı mı?** Öneri: yanlış. Faks "itme bulgusu yok: kollarda savunma izi yok, düşüş açısı geri çekilmeyle uyumlu" der. Ölüm yine Nihat'ın tehdidinin sonucudur; faks bunu "tehdit ve delil karartma" olarak işler.
- **Gerçek kurum adı yok:** "polis/savcılık" yerine BDS; baz istasyonu raporu "operatör" adı vermez; şirket kurgusal (`Kuzey Hat Lojistik`).
- **İntihar çerçevesi duyarlı yazılır:** yöntem ayrıntısı yok, yalnız "atladı mı, düştü mü" sorusu; ilk ekip notu kısa ve klinik.

## Kaynaklar ve açılma sırası (öneri)

Hiçbir yerde "şimdi şuna bak" denmez. Bir ifade yeni kaynağı erişilebilir yapar.

1. **Olay tespit tutanağı:** 6. kat, pencere açık, kapı kilitli/zorlama yok, iki fincan, telefon yok, devrilmiş alçak sehpa. İlk ekip notu: intihar değerlendiriliyor.
2. **Gülay Tan 1:** çağrıyı yaptı; düşmeden önce yukarıda "iki erkek sesi" duydu. Saati kesin değil. → Kamera kayıtlarını açar.
3. **Apartman kamera dökümü (Kamera 01 ön, Kamera 02 arka):** 01.31 Tolga giriş (yalnız), 01.46 K02 sinyal kaybı, 02.03 servis kapısı hareket sensörü, 02.17 çağrı (zaman damgası), 02.24 K02 geri. Kimse "girdi" diye görünmez.
4. **Telefon kaydı (Tolga'nın hattı, son 24 saat):** akşam Derya ile sert mesajlar; 01.20 Cem'le kısa arama; 23.10'da "N. Sönmez" cevapsız arama. → Derya 1, Cem 1, Nihat 1 açılır.
5. **Derya 1:** mesajları küçümser, "kişisel" der; gece evdeydi.
6. **Cem 1:** "Tolga'yı aylardır görmedim." 01.20 aramasını "iş teklifi" diye açıklar. (Yalan.)
7. **Nihat 1:** "O gece eve gittim, Tolga'yla konuşmadım." Tolga'yı "titiz ama son zamanlarda gergin" anlatır — intihar yorumunu besler. (Yalan; arka planda kalır.)
8. **Fincan ve olay yeri inceleme raporu:** bir fincanda Tolga'nın, diğerinde başka birinin izi; ikincisi bir saatten uzun bekletilmiş (soğuk, kahve dibi kurumuş). Sehpa devrik; pencere pervazında itme/boğuşma izi yok.
9. **Kilit inceleme notu:** kapı dışarıdan çekilince kilitlenir; içeriden kilitlenmiş olması gerekmez.
10. **Yönetim bakım kaydı:** K02 üç haftadır aralıklı düşüyor; servis geçidi kör nokta. (Gülay 1'deki "arka kapıdan da girilir" sözü veya kamera dökümüyle açılır.)
11. **Derya yolculuk kaydı:** 01.40–02.40 Kadıköy. → Derya 2: mesajlar borç içindi.
12. **Cem 2:** fincan raporu + telefon kaydı öne sürülünce o gece orada olduğunu kabul eder; servis geçidini kullandığını, 01.55 civarı ayrıldığını, Tolga'nın şirkette zimmet araştırdığını söyler. **Nihat'ın adını ilk kez anar.** → Hesap incelemesi ve otopark kaydı istenebilir.
13. **Otopark plaka kaydı:** Nihat'ın aracı 01.57 giriş, 02.23 çıkış. Cem'in aracı 01.36 giriş, 01.58 çıkış.
14. **Cihaz hareket raporu (Tolga'nın telefonu):** 02.21'den sonra apartmandan uzaklaşır, Nihat'ın evinin bulunduğu semte doğru gider, 02.48'de kapanır.
15. **Şirket hesap incelemesi:** iki yıldır küçük tutarlı ödemeler, onay zinciri Nihat'ta. Tolga'nın son haftalarda bu kayıtları indirdiği görülür.
16. **Nihat 2:** otopark kaydı öne sürülünce bölgede olduğunu kabul eder ("Tolga'yı görmeye gittim ama çıkmadım, vazgeçtim"). (Yalan.)
17. **Nihat 3:** cihaz hareket raporu öne sürülünce dairede olduğunu, tartıştıklarını, Tolga'nın geri çekilirken sehpaya takılıp düştüğünü, telefonu panikle aldığını kabul eder. İtmeyi reddeder.

## Yanlış yollar (her biri savunulabilir, sonra kanıtla kapanır)

- **İntihar:** kilitli kapı, kimse yok, ilk ekip notu, Nihat'ın "gergindi" sözü. Kilit notu, iki fincan, kayıp telefon ve Gülay'ın iki sesi yıkar.
- **Cem (en güçlü tuzak):** yalan söyledi, o gece oradaydı, fincanı masada, tartıştılar, gizli girdi. Otopark (01.58 çıkış) + soğumuş fincan + 02.03 servis kapısı + telefonun 02.21 hareketi onu ölüm anından ayırır.
- **Derya:** sert mesajlar, eski ilişki. Yolculuk kaydı kapatır.
- **Kamerayı biri kapattı:** bakım kaydı kapatır.
- **Nihat itti:** faks; pervazda boğuşma izi yok, sehpa devrik.

## Çelişki anları (oyuncu kendi bulur, oyun işaretlemez)

- Tutanak "kapı içeriden kilitli" ↔ kilit notu: çekince kilitlenir.
- Kamera "Tolga yalnız girdi" ↔ iki fincan ↔ Gülay'ın iki erkek sesi.
- Cem "aylardır görmedim" ↔ 01.20 arama ↔ fincan ↔ otopark 01.36.
- Nihat "eve gittim" ↔ otopark 01.57–02.23.
- Nihat "çıkmadım, vazgeçtim" ↔ telefon 02.21'de apartmandan onun semtine gider.
- Fincan "ikinci kişi" ↔ fincan bir saatten uzun beklemiş ↔ Cem 01.58'de çıkmış.

Her doğru cevabın en az iki bağımsız dayanağı vardır: Nihat → otopark + telefon hareketi (+ servis kapısı saati). Düşüş → pervaz/sehpa bulgusu + Nihat 3. Telefon → cihaz hareketi + otopark çıkış saati.

## Baskı ve akıbet

- **Kapanan görüşme:** Derya'nın ikinci görüşmesi, Nihat 2 dosyaya girdiğinde istenmemişse kapanır (Derya yurt dışına iş gezisine çıkar). Kaybolan yalnız mesajların açıklaması; yolculuk kaydı açığı yine kapatır. Doğrulayıcı bu görüşmenin doğru sonucun yolunda olmadığını denetler.
- **Akıbet (faks):** yanlış suçlamanın bedeli — Cem tutuklanır, Nihat yurt dışına çıkar ve zimmet dosyası kapanır; "intihar" yazılırsa dosya kapanır, şirket Tolga'nın adını zimmetle lekeler. Doğru sonuçta Nihat tehdit, delil karartma ve zimmetle yargılanır; Cem'in belgeleri zimmet davasının dayanağı olur.

## CCTV (kare dizisi; prompt'ları vaka metnine birebir ben veririm)

Kamera 01 (ön kapı) ve Kamera 02 (arka, servis geçidini kısmen gören):
- K01 01.31 Tolga giriş (yalnız)
- K02 01.46 sinyal kaybı (parazit → siyah)
- K02 02.24 sinyal geri (boş geçit)

Servis kapısı 02.03 bir **sensör satırıdır**, görüntüsü yoktur. Hiçbir kare yüz göstermez.

## Gereken görseller (kullanıcıda)

- Portreler: Cem, Nihat, Derya, Gülay (`Characters/`, #002/#003 illüstrasyon üslubu).
- Vaka kapağı / olay yeri: apartman cephesi, gece, 6. kat penceresi açık.
- CCTV kareleri: yukarıdaki üç an.

## Açık kararlar

1. Yukarıdaki düzeltmeler (kilit notu, bakım kaydı, Gülay, Derya'nın yolculuk kaydı, dört sütun eşlemesi) onaylanıyor mu?
2. "İtildi" seçeneği yanlış mı sayılsın (öneri: evet)?
3. Tarih 9 Ocak 2027 uygun mu?
