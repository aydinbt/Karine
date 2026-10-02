# Dosya #003 — Son Teslimat

Bu sayfa içerik ekibi içindir; oyuncuya gösterilmez. **Durum: kesinleşti (2 Ekim 2026).** Senaryo kullanıcıdan geldi; düzeltmeler ve kararlar kullanıcı onayıyla işlendi. Veri yazıldı (2 Ekim 2026): `Cases/case003.json` (12 düğüm), `Locales/tr.case003.json` (190 metin). Doğrulayıcının otomatik gezintisi vakayı baştan sona açıyor; Play Mode'da insan eliyle oynanmadı.

**Büro:** BDS — şüpheli ölüm. **Yer/zaman:** Karaköy, küçük bir elektronik deposu, 12 Aralık 2026 gecesi. **Hedef süre:** 20–25 dk.
**Yeni yetenek:** adli rapor + belge + zaman çizelgesi arasındaki uyumsuzluğu çözmek. CCTV yan kaynaktır.

## Eğitim eğrisi

- 001: Yalan söyleyen kişi ≠ fail.
- 002: Yaralayan ≠ parayı alan.
- 003: **Olay yerini değiştiren ≠ ölüme sebep olan.** Bu vakada fail yoktur; ölüm kazadır.

## Kesin olay

1. 22.30: Seda Sarı (46, kardeş/ortak) borç tartışmasından sonra ayrılır ve geri dönmez.
2. 23.20: Barış Tümer (39, kurye/tedarikçi) teslimat için gelir.
3. Kısa bir elektrik kesintisi olur. Levent Sarı (51) arka bölümdeki benzinli jeneratörü çalıştırır.
4. Jeneratörün egzoz borusu, birkaç gün önceki depo düzenlemesinde yerinden çıkmıştır. Gaz kapalı depoya dolar.
5. 23.30: Barış ayrılır.
6. 23.35–00.05: Levent karbonmonoksitten ölür.
7. Jeneratör yakıtı bitince durur. Bu, Ozan'ın içeri girebilmesini açıklar.
8. 00.42: Ozan Çelik (28, çalışan) gelir. Levent'in öldüğünü anlar.
9. Ozan, şirketin mali kayıtlarının olduğu USB belleği ve bazı belgeleri alır. Sebebi, zimmetinin ortaya çıkmasından korkmasıdır.
10. Ozan ortamı değiştirmeye çalışırken raftaki ağır kutuyu düşürür. Kutu Levent'in başına isabet eder.
11. 01.12: Ozan BDS'yi arar.

Ozan'ın ilk ifadesi: "00.50'de geldim, kapı açıktı, hemen aradım." Bu iki yerde çatlar:
- **Kamera:** Ozan'ın 00.42'de geldiğini gösterir.
- **Çağrı kaydı:** Arama 01.12'dedir.

Böylece arada açıklanmayan yaklaşık 30 dakika kalır.

## Kaynakların açılma sırası (öneri)

Hiçbir yerde "şimdi şuna bak" denmez.

1. Olay tespit tutanağı: düşmüş kutu, baş yarası, boğuşma izi yok, arka bölümde jeneratör.
2. Ozan 1: geliş saati 00.50, hemen aradığını söyler, "jeneratör haftalardır çalışmıyordu". Barış'ın o gece teslimatı olduğunu söyler.
3. Barış 1: 23.20–23.30 arasında oradaydı, elektriklerin gidip geldiğini söyler. Seda'nın akşam orada olduğunu duymuştur.
4. Seda 1: borç tartışmasını kabul eder, 22.30'da ayrıldığını söyler. Mali kayıtları Levent'in bir USB'de tuttuğunu söyler.
5. Depo dış kapı kamerası: tek kamera. Seda 22.30'da çıkıyor. Barış 23.20'de giriyor, 23.30'da çıkıyor. Ozan 00.42'de giriyor. Çağrı 01.12'de. Kamera ölümün nasıl olduğunu göstermez.
6. Adli tıp ön raporu: tahmini ölüm 23.35–00.05, asıl neden karbonmonoksit. Baş travması ölümcül değildir ve ölüm sonrası oluşmuştur (canlılık bulgusu yok).
7. Jeneratör teknik inceleme raporu: o gece çalıştırılmış, yakıt deposu boş, egzoz borusu yerinden çıkık. Bağlantıda zorlama izi yok, çıkma eskidir.
8. Olay yeri eşya dökümü: masada USB bölmesi boş, ilgili klasör eksik. Seda'nın "USB" sözü burada karşılık bulur.
9. Ozan 2: kamera ve çağrı saati karşısında erken geldiğini kabul eder, panikten bekledim der.
10. Şirket hesap incelemesi: Ozan'ın onayıyla yapılmış açıklanmayan ödemeler (zimmet güdüsü).
11. Ozan 3: USB'yi aldığını, kutuyu düşürdüğünü kabul eder. Levent'e dokunmadığını, onu ölü bulduğunu söyler.

## Rapor (çekirdek kod değişmeden, dört sütun)

Motor bugün dört sütun destekler: fail, yöntem, isteğe bağlı `custody`, kanıt. Önerilen eşleme:

| Sütun | Soru | Seçenekler | Doğru |
|---|---|---|---|
| `verdicts` | Ölümden kim sorumlu? | Kimse (kaza) / Ozan / Seda / Barış | Kimse (kaza) |
| `methods` | Ölüm nedeni | Karbonmonoksit / Baş travması / Kalp krizi / Belirsiz | Karbonmonoksit |
| `custody` | Olay yerini kim değiştirdi ve ne aldı? | Ozan, USB / Seda, USB / Barış, para / Kimse | Ozan, USB |
| `evidence` | Dayanak | adli rapor + jeneratör raporu + kamera/çağrı | — |

"Kimse (kaza)" fail sütununda kişi olmayan bir seçenektir. `verdicts` girdisi serbest kimlik ve etiket aldığı için bu, veriyle yapılabilir. Fail sütununun başlığı ise bugün sabittir; bu vakada "Ölümden kim sorumlu?" yazması küçük bir veri alanı gerektirebilir (bkz. açık kararlar).

Önerilen beş sütunlu biçimde (ölüm şekli ve eksik eşya ayrı sorular) motora beşinci sütun eklemek gerekir. Bu, "yeni vaka kod değişmeden eklenir" ilkesini bozar. Yukarıdaki dört sütunlu eşleme aynı bilgiyi sorar.

## Senaryoya önerdiğim düzeltmeler

- **Baş travması "ölüm sonrası" olsun.** Yalnız "ölümcül değil" demek yetmez. Olay yerinin sonradan değiştirildiğini kanıtlayan tek fiziksel bulgu budur.
- **Jeneratör kendiliğinden durmalı.** Yakıt bitmiş olmalı; yoksa 00.42'de içeri giren Ozan da zehirlenir.
- **Egzoz borusunun çıkması eski olmalı, zorlama izi olmamalı.** Böylece "biri borunun yerini oynattı, cinayet" yorumu kanıtla kapanır.
- **Çağrı saati 01.12 olmalı.** Ozan'ın "hemen aradım" sözünü çürüten ikinci zaman kanıtı budur.
- **"Polisi arıyor" yerine "BDS'yi arıyor" demeliyiz.** Resmi kurum adı kullanmama kuralı gereği.
- **"Zehirlenme" ile "karbonmonoksit" aynı seçenekte olmamalı.** Biri diğerini kapsıyor; yerine "Kalp krizi" önerdim.
- **Seda'nın bir şüphe ağırlığı olmalı.** Seda borç tartışması ve ortaklık payıyla güdüsü olan, yanlış yönlendiren kişi olmalı. Kamera 22.30'da çıktığını ve dönmediğini gösterdiğinde temize çıkar.

## CCTV

Tek kamera: depo dış kapısı. Kararımız gereği yalnız Dosya #001 video kullanır; burada anlar kare dizisidir. Önerilen anlar:
- Seda 22.30 çıkış
- Barış 23.20 giriş
- Barış 23.30 çıkış
- Ozan 00.42 giriş

Hiçbir kare yüz ya da ayırt edici ayrıntı göstermez. Kareleri görüntü üretimi tamamlanınca bağlarız; o zamana kadar anlar yalnız metin dökümü olarak durur.

## Kararlar (2 Ekim 2026)

1. **Rapor dört sütundur;** yukarıdaki eşleme kullanılır, motora beşinci sütun eklenmez.
2. **Fail sütununun başlığı vakadan gelir.** `custodyLabelKey` gibi isteğe bağlı genel bir `suspectLabelKey` alanı eklenir; boşsa `conclude.suspect` kullanılır. Bu vaka "Ölümden kim sorumlu?" yazar, 001 ve 002 etkilenmez.
3. **Tarih:** 12 Aralık 2026 gecesi; ölüm 13 Aralık'ın ilk saatlerine sarkar.
4. **Ozan'ın zimmeti raporda ayrı soru değildir.** Yalnız güdüdür; hesap incelemesi Ozan'ın USB'yi neden aldığını açıklar.

## Oyuncu deneyimi: yanıltma ve "aa" anları (kullanıcı ilkesi, 2 Ekim 2026)

Oyuncu sonucu faks gelene kadar bilmemeli; kaynaklar doğruyu **mümkün kılar**, söylemez. Her kişi gerçek insan gibi kendi çıkarını korur, algıyı yönlendirmeye çalışır. Çözüm tek bir kaynakta değil, kaynakların çarpışmasında durur.

**Yanlış yollar (her biri savunulabilir olmalı, sonra kanıtla kapanmalı):**
- **İş kazası:** tutanak ve Ozan'ın anlatısı bunu kurar ("kutu düşmüş"). Adli rapor yıkar.
- **Seda cinayeti:** borç tartışması, ortaklık payı, Barış'ın "akşam bağırışlar duydum" sözü, Seda'nın tartışmayı küçümsemesi. Kamera ve ölüm saati temize çıkarır.
- **Barış:** Levent'i son gören, elektrik kesintisinde oradaydı, teslimat irsaliyesinde saat oynaması var (gerçekte faturayı geç kesmiş, küçük bir vergi kaçamağı saklıyor). Saklaması suçluluk gibi okunur; kamera çıkış saatini, adli rapor ölüm penceresini verir.
- **Ozan cinayeti (en güçlü tuzak):** erken geldi, yalan söyledi, olay yerini değiştirdi, güdüsü var. Ama jeneratör raporu (eski kopma, zorlama yok, yakıt bitmiş) ve ölüm penceresi (Ozan gelmeden önce) onu ölümden ayırır. **Oyuncu Ozan'ı "katil" yazarsa yanlış olur.**

**Çelişki anları (oyuncu kendi bulmalı, oyun işaretlemez):**
- Ozan "00.50'de geldim, hemen aradım" ↔ kamera 00.42 ↔ çağrı 01.12.
- Ozan "jeneratör haftalardır çalışmıyordu" ↔ teknik rapor: o gece çalıştırılmış. (Ozan burada yalan söylemiyor; bilmiyor. Yalan ≠ suç, bilgisizlik ≠ yalan.)
- Tutanak "ağır kutu, baş travması" ↔ adli rapor: travma ölüm sonrası.
- Seda "Levent USB'yi hep yanında taşırdı" ↔ eşya dökümü: USB yok.
- Barış "23.30 gibi çıktım" ↔ irsaliye 23.50 (geç kesilmiş fatura) ↔ kamera 23.30.

**Destek:** her doğru cevabın en az iki bağımsız dayanağı olur; ifadeler yeni kaynağı erişilebilir kılar ama sırayı söylemez; yanlış cevap faksta gerekçesiyle döner.

## Veride uygulanan yapı (2 Ekim 2026)

Açılma zinciri: tutanak → Ozan 1 (`visitors` sorusu) → Barış 1 (`voices`) → Seda 1 (`camera`) → kapı kamerası. Ozan 1'deki `call` sorusu adli tıbbı istenebilir yapar; adli tıp jeneratör incelemesini, jeneratör + Seda'nın USB sözü eşya dökümünü açar. Eşya dökümü Barış 2'yi; eşya dökümü + Ozan 2 hesap incelemesini; hesap incelemesi Ozan 3'ü açar.

- **Ozan 2:** kamera `ozan_in` karesi öne sürülünce saati kabul eder; yarım saati "dondum" diye açıklar; koliye dokunmadığını söyler (yalan); jeneratörün çalıştığını bilmez (dürüst bilgisizlik, "ağır bir koku vardı, rutubet sandım").
- **Barış 2:** eşya dökümündeki irsaliye öne sürülünce saat oyununu vergi kaçamağı olarak kabul eder; elektrik kesilince Levent'in jeneratörü **kendisinin** çalıştırdığını anlatır (kazayı bağımsız kuran ifade); Seda'yı gördüğünden emin olmadığını söyler.
- **Ozan 3:** hesap incelemesi öne sürülünce zimmeti, belleği ve klasörü almayı, koliyi düşürmeyi kabul eder; ölümü reddeder.
- Kamerada `lights` (23.24) anı elektrik kesintisini Barış'ın ifadesinden bağımsız doğrular.
- Zaman çizelgesi gece yarısını geçtiği için `sortMinute` değerleri 720 dakika kaydırıldı (yalnız sıralama içindir).
- Rapor başlıkları vakadan gelir: `suspectLabelKey` "Ölümden kim sorumlu?", `methodLabelKey` "Ölüm nedeni", `custodyLabelKey` "Olay yerini kim değiştirdi, ne aldı?".
- CCTV karesi yok; altı an metin dökümü. Kareler sonra eklenebilir.

## Baskı ve akıbet (2 Ekim 2026)

- **Kapanan görüşme:** Barış Tümer'in ikinci görüşmesi (`baris_follow`), Ozan'la üçüncü görüşme (`ozan_third`) dosyaya girdiğinde hâlâ istenmemişse kapanır: Barış şehir dışına çıkar, avukatı dönmeyeceğini bildirir. Kaybolan şey Barış'ın "jeneratörü Levent kendisi çalıştırdı" tanıklığı ve irsaliye çelişkisidir; kazanın kanıtı jeneratör incelemesinde ve adli tıp raporunda kalır. Doğrulayıcı bu görüşmenin doğru sonucun yolunda olmadığını denetler.
- **Akıbet:** dört şüpheli ve dört ikinci sorumluluk seçeneğinin her biri faksta kendi sonucunu anlatır. Yanlış suçlamanın bedeli: Ozan dört ay tutuklu, Seda'nın payı donar, Barış'ın sözleşmeleri askıya alınır; ikinci sorumluluk yanlışsa zimmetin izi kaybolur.
