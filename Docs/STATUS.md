# Karine — durum özeti

**Son güncelleme:** 1 Ekim 2026 (yeni masa referansı: geniş ülke penceresi, ayrı nesneler, sade üst şerit)
**Bu dosya:** projeye bakan herkesin ilk okuyacağı tek sayfa. Ayrıntı için [ROADMAP.md](ROADMAP.md), kanıt için [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md), ileri plan için [PHASE_PLAN.md](PHASE_PLAN.md).

## Tek cümle

**1 Ekim masa sunumu:** Yeni referansa göre geniş orta pencere, solda lamba ve evrak tepsisi, ortada telefon/beyaz dosya, sağda sade CCTV monitörü ve delil yığını yerleştirildi. Üst şeritte dosya/konum ve altı menü eylemi bulunur; logo ana menüye döndürür. 111 EditMode + 13 PlayMode geçti; grafik etkin Unity çıktısı incelendi. Fiziksel telefon kabulü açık.

**27 Eylül menü işi:** Kullanıcının referansına göre logo, Türkçe slogan, beş menü eylemi ve Bora kartı mevcut video üzerine ayrı, yeniden kullanılabilir UI katmanlarıyla kuruldu. Yeni kariyer ayarlardan erişilir; oyun/vaka akışına dokunulmadı. 111 EditMode ve 11 PlayMode testi geçti; referansa göre farklı yatay ekran oranlarında gözle inceleme açık olduğundan yol haritası işareti `[~]`.

**Dosya #001 baştan sona oynandı** (kullanıcı, 25 Eylül 2026): soruşturma, sorgu, kanıt eşleme ve gerekçeli sonuç gönderme adımları sorunsuz çalıştı ve sesler duyuldu. Oynanıştan dört bulgu çıktı, kapatıldı ve kullanıcı tarafından doğrulandı: masadaki nesneler sessizdi, ana menü müziği hiç duyulmuyordu, masada oda gürültüsü yerine müzik istendi, ayarlar sayfası telefonda sıkışıktı. Kalan boşluk **cihaz doğrulaması**: hiçbir telefonda denenmedi ([PLAYTEST_001.md](PLAYTEST_001.md) §3) ve kullanıcının elinde telefon olmadığı için bu adım **ertelendi** (26 Eylül 2026). Faz 2 açık kalıyor; iş Faz 3'ten devam ediyor: **Dosya #002 "Son Sefer" yazıldı** — veri, metin ve doğrulama tamam, Play Mode'da oynanması açık.

## Dosya #001 hakkında (25 Eylül 2026 kullanıcı kararı)

**Vaka #001'in içeriği şimdilik tamam sayılıyor.** Yeni ifade, kaynak, kanıt veya tur eklenmeyecek; vakanın tasarımı üstünde yeni iş açılmaz. **Oynanış doğrulaması yapıldı:** kullanıcı vakayı 25 Eylül 2026'da baştan kapanışa kadar oynadı ve içerikte sorun bulunmadı ([PLAYTEST_001.md](PLAYTEST_001.md) §2, §4). Geriye cihaz adımı (§3) kaldı; M1 ve M2'nin bitiş ölçütleri buna bağlı. Sıradaki eksen cihaz derlemesi ya da Dosya #002'nin oynanış doğrulaması.

## Vakadan bağımsız temeller (25 Eylül 2026)

Hedef: bundan sonra yalnız vaka eklemek kalsın. Bugün atılanlar — işareti ayrıca yazılmayanlar `[~]`, yani kodlandı ama Play Mode'da görülmedi:

- **Vaka eklemek koddan koptu:** yeni vaka = `caseXXX.json` + `tr.caseXXX.json` + varlıklar. Portre tonları veride, doğrulayıcı vakaya özel C# istemiyor.
- **Ses sistemi kurulu, dokuz klip depoda ve sesler duyuldu.** Klipler sentezlenmiş: `Tools/make-audio.py` üretir, `Tools/check-audio.py` ölçer. Dört arayüz sesi, görüşmede yumuşak bir sohbet blibi (konuşma taklidi yok), ana menü müziği (32 s döngü), masa müziği (`desk_theme`, 40 s — oda gürültüsünün yerini aldı) ve görüşme odası ortamı. Doğrulayıcı dokuzunun varlığını kilitliyor.
- **Ses ayarı beş kademe:** kapalı, %25, %50, %75, tam — ayarlarda radyo listesi (kit'te kaydırıcı yok). Varsayılan: müzik %50, efektler tam.
- **Bölüm seçici ekranı çalışıyor (`[x]`, Play Mode'da görüldü — kullanıcı, 26 Eylül 2026):** Önceki yerleşim 26 Eylül’de gözlendi. 27 Eylül’de sol kimlik/menü, yatay ülke kartları ve polaroid vaka şeridine dönüştürüldü; yeni görsel yerleşim `[~]`. Veri `Bube/Worlds.json`da; kilit ilerlemeden türer, ipucu vermez. Ölçek kanon oldu: **on ülke × yedi dosya = 70** ([WORLD_OPENINGS.md](WORLD_OPENINGS.md), 26 Eylül 2026 kararı). Ülke görselleri tek atlasla, ilk iki vaka kapakları mevcut içerikle karşılanır. Dünya haritası artık bu sayfanın parçası değildir. Kilit/onay işaretleri ortak vektör UI öğesidir.
- **Mobil davranış:** geri tuşu, çıkış onayı, arkaya atılınca kayıt, zincir sonu bildirimi.
- **Reklam dikişi kurulu, ağ yok.** `AdGateway` kuralları testli; LevelPlay/AdMob kurulumu senin hesap kimliklerini bekliyor. Ödüllü ipucu kanonu bozmuyor ve bunu doğrulayıcı kilitliyor.

**Ödüllü yeniden deneme bağlandı:** güven tam o faksın götürdüğü kadar iade ediliyor (gerekirse görevden ayrılma kalkıyor), faks geçmişi başarısızlığı "yeniden açıldı" işaretiyle saklıyor ve ikinci deneme kendi satırını yazıyor. Yeniden açmak ipucu vermiyor: bulunanlar duruyor, yalnız rapor alanları boşalıyor.

## Dosya #002 — Son Sefer (26 Eylül 2026)

Senaryo kullanıcıdan geldi ve eski "Kayıp Yedek" taslağının yerine geçti; taslak tamamen kaldırıldı. Büro **BDS Asayiş Masası**, nitelik yağma/gasp ve yaralama. Vakanın yeni olan yanı mekanik değil **yapı**: tek olay → iki ayrı eylem → iki ayrı sorumluluk. Yaralayan ile parayı alan farklı kişiler, bu yüzden sonuç raporu bu vakada dört sütunlu ([CASE002_DESIGN.md](CASE002_DESIGN.md)). Sütun motorda isteğe bağlıdır; Dosya #001 üç sütunlu kalır.

Durum `[~]`: 13 düğüm, 31 soru, 170 metin anahtarı; doğrulayıcı vakayı baştan sona otomatik oynuyor ve desteklenen raporu gönderiyor. **Play Mode'da oynanmadı.** Dört kişinin portreleri kullanıcıdan geldi ve depoda.

Bölüm geçişi artık sessiz değil: kabul edilmemiş her yeni dosya masaya bırakılıyor (`NewCaseArrival`). Dünyanın kendi varış filmi yalnız o dünyanın ilk dosyasında bir kez oynuyor. İki PlayMode testi var, **Play Mode'da gözle görülmedi** `[~]`.

Rapor gönderildiğinde bırakılışın tersi oynuyor: Bora evrakı kaşeler, dosya ekran dışına gider, sonra vaka özeti açılır (`reportSendVideo`) `[~]`.

## Kimlik

- **Oyun adı:** Karine — 25 Eylül 2026'da karara bağlandı ([NAMING.md](NAMING.md)).
- **Stüdyo:** bubeGames · **Oyun içi kurum:** bube Departman / BDS (değişmedi, kurmaca kurum adıdır).
- **Paket kimliği:** `com.bubedigital.karine` · **Depo:** `github.com/aydinbt/Karine`

## Şu an nerede

| Aşama | Durum |
| --- | --- |
| **Faz 0 — Zemin** | **Bitti ve doğrulandı** |
| **Faz 1 — Doğrulamayı otomatikleştir** | **Bitti** — doğrulayıcı vaka başına ayrıldı, ilk hatada durmuyor, 42 test yeşil |
| **Faz 2 — Gerçekten oyna** | **İlerliyor** — Dosya #001 elle oynandı ve oynanıştan gelen dört düzeltme girildi (113 test yeşil); **cihaz adımı açık** |
| Aşama 1 — Temel yapı | Kod tamam, cihaz doğrulaması açık |
| M1 — Dosya #001 döngüsü | **Baştan sona oynandı**; cihaz adımı açık |
| M2 — Soruşturmayı oyuna çevirme | Oynanarak doğrulandı; cihaz adımı açık |
| M3 — Vaka ekleme & kayıt | case002 "Son Sefer" yazıldı ve taslaklıktan çıktı `[~]`; kayıt göçü kodlandı `[~]` |
| M4 — Mobil kalite kapısı | Derleme ayarları hazır; cihaz testi başlamadı |
| M5 — Görsel ve ses | Ertelendi |
| M6 — Sonraki sistemler | Beklemede |

## Sayılarla

- Kod: ~4.400 satır C#; `BubeApp` konu başına sekiz `partial` dosya, en uzunu 529 satır
- Vaka #001: 9 düğüm, 30 soru — bütünlük denetiminden temiz geçti
- Vaka #002: 7 düğüm, 12 soru — `draft: true`, oyuncuya kapalı
- Türkçe metin: 577 anahtar, eksik 0, yinelenen 0, ölü ~10
- Diller: 1 (tr)
- Assembly: 3 (`Bube.Runtime`, `Bube.Editor`, `Bube.Tests.EditMode`) — hepsi 0 hatayla derleniyor
- Test: **84, hepsi geçiyor** (77 EditMode + 7 PlayMode) — 40 EditMode (8 içerik/metin, 12 Dosya #001 akış, 8 kapı/zaman çizelgesi, 7 doğrulama raporu, 6 kayıt şeması, 1 config) + 5 PlayMode (2 duman + 3 vaka kabul akışı). Tek komut: `Tools/run-tests.sh`
- Doğrulayıcı: 9 dosya `Assets/Bube/Editor/Validation/` altında; `ProjectSetup.cs` 285 → 37 satır

## Faz 0'da yapılanlar (25 Eylül 2026)

- [x] Oyun adı **Karine**; `config.json` `title`, `productName`, `about.body` güncellendi.
- [x] `applicationIdentifier` = `com.bubedigital.karine` (Android / iPhone / Standalone).
- [x] `scriptingBackend` = **IL2CPP**; API seviyesi .NET Standard 2.1. *(ARM64-only Android ile Mono desteklenmiyordu.)*
- [x] `AndroidTargetSdkVersion` = **35** olarak sabitlendi (önceden 0 = Automatic).
- [x] `com.unity.test-framework` 1.4.6 eklendi.
- [x] `Bube.Runtime` / `Bube.Editor` / `Bube.Tests.EditMode` asmdef'leri kuruldu — üçü de sıfır hatayla derlendi.
- [x] Var olan `Bube/Validate Content` doğrulayıcısı EditMode testinden çağrılır hâle geldi.
- [x] Depo git'e alındı, `github.com/aydinbt/Karine`'e bağlandı; `.gitignore` / `.gitattributes` yazıldı, `.mp4` ve `.ttf` **Git LFS**'e alındı.
- [x] Çalışma klasörü `dedektif` → **`karine-mobile`** olarak yeniden adlandırıldı.
- [x] **EditMode testleri Unity Editor'da koşturuldu ve geçti** — `BUBE VALIDATION PASSED`. Tek uyarı: Dosya #002'nin (taslak) `arda` ve `deniz` karakterleri için üretilmiş yedek portre kullanılıyor; bu beklenen davranış.

## Açık kritik maddeler

1. **Terminal ekranındaki arma yaması görünüyor** — armanın yeri tek düz renkle dolduruldu, ekranın gradyanından ayrılıyor ve CCTV kutusu sağa kaymış duruyor. Kullanıcı kararıyla sonraya bırakıldı. Kurum adı ve terminal arması hem masa görselinden hem videodan temizlendi; dosya kapağındaki arma kullanıcı kararıyla kalıyor. Oyun içi kurum kurgusaldır (bube Departman / BDS).
1. ~~**Dosya #001 hiç baştan sona oynanmadı.**~~ **Oynandı (kullanıcı, 25 Eylül 2026), sorun çıkmadı.** **Cihaz doğrulaması ertelendi (26 Eylül 2026):** elde Android telefon yok. M1/M2 bu yüzden kapanmıyor ve masaüstü oynanışı cihaz ölçütünün yerine **geçmez**. Emülatör/Device Simulator seçenekleri açık, kullanıcı şimdilik ertelemeyi seçti.
2. **Android keystore yok** — imzalı *mağaza* sürümü üretilemez. **Düzeltme:** cihaza geliştirme derlemesi kurmak için keystore gerekmiyor (Unity hata ayıklama anahtarıyla imzalar), bu yüzden madde Faz 2'den **Faz 5'e** taşındı; parola kullanıcıya aittir.
3. ~~**Kayıt şeması göçü yok** — `version != 1` olduğunda ilerleme sessizce siliniyor.~~ **Kodlandı, cihazda denenmedi `[~]`** — eski kayıt yükseltilir, gelecekten gelen kayıt silinmeyip yana kaldırılır ve oyuncuya söylenir.
4. ~~**Performans:** `Update()` her karede tam vaka JSON'u ayrıştırıyor; `Locale.Get` doğrusal arama yapıyor.~~ **Kodlandı (Faz 2), Play Mode'da gözlenmedi `[~]`** — beş düzeltme: görev önbelleği, güvenli alan yazımları, rozet yazımları, yüklem temsilcileri, sözlükle indeksli `Locale`.
5. ~~**Unity batchmode lisansı bu makinede çalışmıyor.**~~ **Yanlış teşhisti, düzeltildi.** Testler `Tools/run-tests.sh` ile komut satırından koşuyor (EditMode + PlayMode). Gereken tek şey Unity Hub'ın açık olması. Ayrıntı: [Architecture.md](Architecture.md) → "Testleri başsız koşmak".

## Son oturumda ne değişti (25 Eylül 2026)

Soruşturmanın **dokusu** ve telefonda **erişilebilirlik**. Hepsi `[~]`: kodlandı, 51 test geçiyor, ekranlara bakıldı — oynanarak doğrulanmadı.

- **Yeni kanon kural:** bir kaydı kişiye ancak **adı orada geçiyorsa** öne sürebilirsin. Metinden türer, elle etiketlenmez, yeni vakalarda kendiliğinden işler. `aboutPersonIds` artık yalnız "adını anmadan söz eden" kayıtlar için bir ek.
- **Yem kaynaklar (63) ve davranış satırı (103).** Yanlış kaynak artık gerçek ama yanıltıcı bir yanıt üretir; soru kapanmaz. Davranış satırı gözlem verir, yorum vermez.
- **Mobil sadeleştirme:** dosya ekranında iç içe kaydırma, "1/1" sayacı ve kayan sekme şeridi kalktı; "Dosyada ara" yazı alanı dokunulur tür/kişi süzgecine çevrildi.
- **KARINE UI/UX Kit bağlayıcı tasarım sistemi oldu** — `KarineTheme` + `KarineUI` tek kaynak; palet kit görselinden okundu, ikonlar kit'ten kesildi, ekranlar bileşenlere taşındı, ham renk borcu 156 → 65. Roboto Slab, Inter ve Alfa Slab One depoya kondu, gövde yazısı mono'dan çıktı; sinematiklerde tek denetim GEÇ kaldı (duraklatma ve kare ilerletme CCTV'de duruyor). Kanon: [UI_KIT.md](UI_KIT.md).
- **KARINE logosu oyuna bağlandı** (ana menü + kompakt başlık, tek oran/tek doku). Unity'nin varsayılan içe aktarımı logoyu 2048×512'ye eziyordu; kilitlendi.
- **Doğrulayıcıya sekiz yeni kural** — her biri bu oturumda yaşanan gerçek bir hatadan doğdu.

Ayrıntı ve gerekçeler: [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md), işaretler: [ROADMAP.md](ROADMAP.md).

## Sıradaki iş

Cihaz adımı ertelendiği için sıra **Faz 3 — Dosya #002**: vaka bugün `draft`, 7 kaynak ve 12 soru ile duruyor (Dosya #001'de 9 kaynak, 30 soru). Yapılacaklar: soruşturma dokusunu #001 seviyesine çıkarmak, `draft` kalkınca #001 → #002 geçişini ve faks zamanlamasını doğrulamak, `CASE_AUTHORING.md` yazmak. Cihaz adımı telefon bulunduğunda [PLAYTEST_001.md](PLAYTEST_001.md) §3 ile koşulur. Betikteki otomatikleşmiş satırlar işaretli; kalanlar gözle doğrulanacak şeyler — video, çentik, dokunma hedefi, glif, klavye, kare hızı, okunabilirlik. Bunlar doldurulunca M1/M2 kapanabilir.

**Vakalar — 27 Eylül:** Referansın sol menü ve fotoğraflı kart hiyerarşisi uygulandı; mevcut masa görseli arka dekor olarak yeniden kullanıldı. Başlıklar, durumlar, kartlar ve gezinme ayrı UI öğeleridir. Vaka verisi, kariyer kaydı ve oyun akışı değiştirilmedi. Görsel kabul ve cihaz kontrolü açık.

**Son test:** 111 EditMode + 12 PlayMode geçti. Yeni test, ülke değişiminde kartların kaydırılabilir kalmasını ve kapalı ülkedeki dosyaların etkileşime açılmamasını denetler; görsel karşılaştırma yapmaz.

**Masa — 27 Eylül:** Kullanıcı referansı doğrultusunda boş oda dekoru + saydam nesne atlası + ülke manzarası + UI bileşenleri kuruldu. Görev/Notlar yok; dosya beyaz ve fotoğrafsız, monitör yalnız CCTV Arşivi yazısı taşır. Mevcut oynanış dalları korunur. Gerçek telefon kabulü açık.

**Masa son doğrulama:** 111 EditMode + 13 PlayMode testi başarılı. Unity render görüntüsünde ayrı oda/nesne/pencere katmanları ve etiket yerleşimi incelendi. Gerçek cihaz kontrolü açık; oyuncunun kaydı test için değiştirilmedi.

## 1 Ekim 2026 — Dosya Vaka Detay

Dosya detay sunumu 1 Ekim 2026 referansına uyarlandı: katmanlı kâğıt, olay fotoğrafı, kompakt metadata, keşfedilmiş kişi kartları, sağ sekmeler ve üst gezinme. Vaka metinleri ve soruşturma koşulları değişmedi. 111 EditMode + 13 PlayMode geçti; fiziksel telefon kontrolü açık.

## 1 Ekim 2026 — Gelen Evraklar sunumu

Gelen Evraklar ekranı son referansa göre sol liste + geniş kâğıt önizleme düzenine taşındı. Mevcut logo, ikonlar, oda ve DossierPaper dokusu yeniden kullanıldı. 111 EditMode + 13 PlayMode geçti. Fiziksel telefon kontrolü açık.

Son doğrulama notu: Unity önizlemesi incelendi; ardından yalnız liste hizası ve yinelenen kurum başlığı düzeltildi. Bu iki sunum düzeltmesi sonrası test betiği lisans oturumu bulunamadığından yeniden çalışmadı. Önceki sürümde 111 EditMode + 13 PlayMode başarılıydı.

## 1 Ekim 2026 — Görüşmeler ve İncelemeler tableti

Görüşmeler/İncelemeler tablet sunumu sol gezinme, orta liste ve sağ detay kartına taşındı. Mevcut tablet ve portreler kullanıldı; yeni bitmap yok. 111 EditMode ve 13 PlayMode geçti; telefon kabulü açık.

## 1 Ekim 2026 — CCTV tablet sunumu

CCTV tablet sunumu erişilebilir kamera listesi + geniş monospace kayıt dökümü düzenine taşındı. Mevcut tablet/ikonlar kullanıldı; yeni asset üretilmedi. 111 EditMode + 13 PlayMode başarılı; fiziksel telefon kontrolü açık.

## 1 Ekim 2026 — Vakalar yeni referansı

Vakalar sayfası son referansa göre koyu ana panel, panoramik ülke başlığı, kompakt dosya kartları, ülke ilerleme çubukları ve genel ilerleme göstergesiyle güncellendi. Mevcut ülke atlası/Bora/kapaklar kullanıldı. 111 EditMode + 13 PlayMode geçti; fiziksel telefon kabulü açık.

Vakalar yerleşimi Unity’de 1280×720 görüntüyle kontrol edildi; iki satırlı vaka başlıkları ve durum etiketleri kart içinde kalıyor. Görsel kayıt: `Docs/Reference/WORLDS_2026-10-01.png`. Son yerleşim sonrası 13 PlayMode testi yeniden geçti.


## 1 Ekim 2026 — Çubuksuz kaydırma

Oyun genelindeki kaydırma çubukları ortak bileşen üzerinden gizlendi; içerik kaydırma korunuyor. Gerçek telefonda parmakla kullanım doğrulaması açık.


## 1 Ekim 2026 — Ayarlar referansı

Ayarlar sol sekmeli koyu panel ve krem tercih kartlarıyla yenilendi. Kaydet/varsayılan taslağı eklendi; gameplay akışı değiştirilmedi. 111 EditMode + 13 PlayMode geçti; fiziksel telefon doğrulaması açık.


## 1 Ekim 2026 — Chakra Petch

Bütün oyun metinleri Chakra Petch Regular/SemiBold/Bold ailesine geçirildi. FontSet ve içerik doğrulayıcı güncellendi; logo bitmap olarak korundu. OFL lisansı fontlarla birlikte eklendi. Fiziksel telefon okunabilirliği kontrolü açık.

Chakra Petch doğrulaması: Regular/SemiBold/Bold dosyalarında Türkçe karakterler ve ₺ mevcut. 111 EditMode + 13 PlayMode başarılı; Unity Ayarlar görünümü incelendi.


## 1 Ekim 2026 — Ortak geçişler

Dosya/tablet/düğme geçişleri ortak sisteme alındı. Hareketi azalt tercihi eklendi. Fiziksel telefon doğrulaması açık.

Geçiş paketi sonrası 14 PlayMode testi geçti. Yeni test: oyun zamanı dururken tamamlanma, ekrandan ayrılınca eski callback iptali ve azaltılmış harekette anında tamamlanma.


## 1 Ekim 2026 — Evrak varışı

Gelen evrak ritüeli eklendi: tepsiye kâğıt hareketi, kısa faks sesi, bildirim ışığı. Tekrar kontrolü oturum içidir; fiziksel cihaz ve duyumsal ses kontrolü açık.

Evrak varışı sonrası 15 PlayMode testi geçti; yeni test kâğıdın gelmesini, tamamlanınca kaldırılmasını ve masaya dönünce tekrarlanmamasını doğruluyor.


## 1 Ekim 2026 — Dosya sekmesi geçişi

Dosya sekmelerinde kısa sayfa geçişi ve aktif sekmenin öne çıkması eklendi; hareket azaltma tercihine uyar. Fiziksel cihaz kontrolü açık.


## 1 Ekim 2026 — Masanın havası (1. kademe)

Masa artık iki katman: oda + pencere arkada, eşyalar ve masa düğmeleri önde. Telefon eğildikçe katmanlar farklı hızda kayıyor; lamba ışığı ve toz ön katmanda, kararma eşyaların üstünde ama düğmelerin altında. Dokunulan eşya kalkıyor. Yeni test `DeskAtmosphereTests` (2): katman sırası ve kalkma/oturma. EditMode 111/111, PlayMode 17/17. Görsel kabul ve cihazda eğme hissi açık.


## 1 Ekim 2026 — Efektler (2. kademe)

Sorguda kayıt kâğıt olarak karşıdakine kayıyor (sürme ya da dokunma), faks ve yeni evrak basılarak çıkıyor, CCTV'de kamera değişince sinyal bozuluyor, dosya sekmesinde sayfa çevriliyor. Yeni test `EffectsTests` (5). EditMode 111/111, PlayMode 22/22. Görsel kabul ve dokunmatik his açık.
