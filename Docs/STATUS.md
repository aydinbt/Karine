# Karine — durum özeti

**Son güncelleme:** 25 Eylül 2026 (UI Kit taşımasının kapanışı + vakadan bağımsız temeller: ses, geri tuşu, reklam dikişi)
**Bu dosya:** projeye bakan herkesin ilk okuyacağı tek sayfa. Ayrıntı için [ROADMAP.md](ROADMAP.md), kanıt için [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md), ileri plan için [PHASE_PLAN.md](PHASE_PLAN.md).

## Tek cümle

İçerik doğrulayıcısı vaka başına ayrıldı ve tüm bulguları tek koşuda raporluyor; Dosya #001'in soruşturma **mantığı** uçtan uca otomatik testlerle doğrulanıyor ve `BubeApp` Play Mode'da hatasız açılıyor. Açılış (masaya varış sinematiği, vaka teklifi, masa yerleşimi) 25 Eylül 2026'da Play Mode'da **gözlendi**. Kalan boşluk **oynanışın gerisi ve cihaz doğrulaması**: soruşturmanın tamamı hâlâ bir insan tarafından baştan sona oynanmadı ve hiçbir telefonda denenmedi ([PLAYTEST_001.md](PLAYTEST_001.md)).

## Dosya #001 hakkında (25 Eylül 2026 kullanıcı kararı)

**Vaka #001'in içeriği şimdilik tamam sayılıyor.** Yeni ifade, kaynak, kanıt veya tur eklenmeyecek; vakanın tasarımı üstünde yeni iş açılmaz. Bu **doğrulamanın yapıldığı anlamına gelmez**: soruşturma, sorgu, kanıt eşleme ve gerekçeli sonuç gönderme adımları hâlâ bir insan tarafından baştan sona oynanmadı ([PLAYTEST_001.md](PLAYTEST_001.md) §2-4), bu yüzden M1 ve M2 maddeleri `[~]` kalır. İçerik kapandığına göre sıradaki iş ya elle oynanış doğrulaması ya da başka bir eksen (cihaz derlemesi, case002) olur.

## Vakadan bağımsız temeller (25 Eylül 2026)

Hedef: bundan sonra yalnız vaka eklemek kalsın. Bugün atılanlar — hepsi `[~]`, hiçbiri Play Mode'da görülmedi:

- **Vaka eklemek koddan koptu:** yeni vaka = `caseXXX.json` + `tr.caseXXX.json` + varlıklar. Portre tonları veride, doğrulayıcı vakaya özel C# istemiyor.
- **Ses sistemi kurulu, ses dosyası yok.** Dosyalar geldiğinde tek iş onları `Resources/Bube/Audio/` içine koymak; ekranlara geri dönmek gerekmiyor.
- **Mobil davranış:** geri tuşu, çıkış onayı, arkaya atılınca kayıt, zincir sonu bildirimi.
- **Reklam dikişi kurulu, ağ yok.** `AdGateway` kuralları testli; LevelPlay/AdMob kurulumu senin hesap kimliklerini bekliyor. Ödüllü ipucu kanonu bozmuyor ve bunu doğrulayıcı kilitliyor.

**Ödüllü yeniden deneme bağlandı:** güven tam o faksın götürdüğü kadar iade ediliyor (gerekirse görevden ayrılma kalkıyor), faks geçmişi başarısızlığı "yeniden açıldı" işaretiyle saklıyor ve ikinci deneme kendi satırını yazıyor. Yeniden açmak ipucu vermiyor: bulunanlar duruyor, yalnız rapor alanları boşalıyor.

## Kimlik

- **Oyun adı:** Karine — 25 Eylül 2026'da karara bağlandı ([NAMING.md](NAMING.md)).
- **Stüdyo:** bubeGames · **Oyun içi kurum:** bube Police / BPS (değişmedi, kurmaca kurum adıdır).
- **Paket kimliği:** `com.bubedigital.karine` · **Depo:** `github.com/aydinbt/Karine`

## Şu an nerede

| Aşama | Durum |
| --- | --- |
| **Faz 0 — Zemin** | **Bitti ve doğrulandı** |
| **Faz 1 — Doğrulamayı otomatikleştir** | **Bitti** — doğrulayıcı vaka başına ayrıldı, ilk hatada durmuyor, 42 test yeşil |
| **Faz 2 — Gerçekten oyna** | **İlerliyor** — kit taşıması, ses, geri tuşu ve reklam dikişi kodlandı (105 test yeşil); elle oynanış ve cihaz adımı açık |
| Aşama 1 — Temel yapı | Kod tamam, cihaz doğrulaması açık |
| M1 — Dosya #001 döngüsü | Kod ~tamam, **Play Mode doğrulaması açık** |
| M2 — Soruşturmayı oyuna çevirme | Kod büyük ölçüde tamam, doğrulama açık |
| M3 — Vaka ekleme & kayıt | case002 taslak hâlde çalışıyor; kayıt göçü kodlandı `[~]` |
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

1. **Terminal ekranındaki arma yaması görünüyor** — armanın yeri tek düz renkle dolduruldu, ekranın gradyanından ayrılıyor ve CCTV kutusu sağa kaymış duruyor. Kullanıcı kararıyla sonraya bırakıldı. Kurum adı ve terminal arması hem masa görselinden hem videodan temizlendi; dosya kapağındaki arma kullanıcı kararıyla kalıyor. Oyun içi kurum kurgusaldır (bube Polis / BPS).
1. **Dosya #001 hiç baştan sona oynanmadı** (açılışı gözlendi, gerisi değil) — M1 ve M2'nin bitiş ölçütleri buna bağlı (Faz 2). **Asıl darboğaz budur.**
2. **Android keystore yok** — imzalı *mağaza* sürümü üretilemez. **Düzeltme:** cihaza geliştirme derlemesi kurmak için keystore gerekmiyor (Unity hata ayıklama anahtarıyla imzalar), bu yüzden madde Faz 2'den **Faz 5'e** taşındı; parola kullanıcıya aittir.
3. ~~**Kayıt şeması göçü yok** — `version != 1` olduğunda ilerleme sessizce siliniyor.~~ **Kodlandı, cihazda denenmedi `[~]`** — eski kayıt yükseltilir, gelecekten gelen kayıt silinmeyip yana kaldırılır ve oyuncuya söylenir.
4. ~~**Performans:** `Update()` her karede tam vaka JSON'u ayrıştırıyor; `Locale.Get` doğrusal arama yapıyor.~~ **Kodlandı (Faz 2), Play Mode'da gözlenmedi `[~]`** — beş düzeltme: görev önbelleği, güvenli alan yazımları, rozet yazımları, yüklem temsilcileri, sözlükle indeksli `Locale`.
5. ~~**Unity batchmode lisansı bu makinede çalışmıyor.**~~ **Yanlış teşhisti, düzeltildi.** Testler `Tools/run-tests.sh` ile komut satırından koşuyor (EditMode + PlayMode). Gereken tek şey Unity Hub'ın açık olması. Ayrıntı: [Architecture.md](Architecture.md) → "Testleri başsız koşmak".

## Son oturumda ne değişti (25 Eylül 2026)

Soruşturmanın **dokusu** ve telefonda **erişilebilirlik**. Hepsi `[~]`: kodlandı, 51 test geçiyor, ekranlara bakıldı — oynanarak doğrulanmadı.

- **Yeni kanon kural:** bir kaydı kişiye ancak **adı orada geçiyorsa** öne sürebilirsin. Metinden türer, elle etiketlenmez, yeni vakalarda kendiliğinden işler. `aboutPersonIds` artık yalnız "adını anmadan söz eden" kayıtlar için bir ek.
- **Yem kaynaklar (29) ve davranış satırı (38, deneme).** Yanlış kaynak artık gerçek ama yanıltıcı bir yanıt üretir; soru kapanmaz. Davranış satırı gözlem verir, yorum vermez.
- **Mobil sadeleştirme:** dosya ekranında iç içe kaydırma, "1/1" sayacı ve kayan sekme şeridi kalktı; "Dosyada ara" yazı alanı dokunulur tür/kişi süzgecine çevrildi.
- **KARINE UI/UX Kit bağlayıcı tasarım sistemi oldu** — `KarineTheme` + `KarineUI` tek kaynak; palet kit görselinden okundu, ikonlar kit'ten kesildi, ekranlar bileşenlere taşındı, ham renk borcu 156 → 65. Roboto Slab, Inter ve Alfa Slab One depoya kondu, gövde yazısı mono'dan çıktı; sinematiklerde tek denetim GEÇ kaldı (duraklatma ve kare ilerletme CCTV'de duruyor). Kanon: [UI_KIT.md](UI_KIT.md).
- **KARINE logosu oyuna bağlandı** (ana menü + kompakt başlık, tek oran/tek doku). Unity'nin varsayılan içe aktarımı logoyu 2048×512'ye eziyordu; kilitlendi.
- **Doğrulayıcıya sekiz yeni kural** — her biri bu oturumda yaşanan gerçek bir hatadan doğdu.

Ayrıntı ve gerekçeler: [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md), işaretler: [ROADMAP.md](ROADMAP.md).

## Sıradaki iş

[PLAYTEST_001.md](PLAYTEST_001.md) §2–4'ü elle koş: oyunu bir kez baştan sona oyna, sonra bir Android telefonda yinele. Betikteki otomatikleşmiş satırlar işaretli; kalanlar gözle doğrulanacak şeyler — video, çentik, dokunma hedefi, glif, klavye, kare hızı, okunabilirlik. Bunlar doldurulunca M1/M2 kapanabilir.
