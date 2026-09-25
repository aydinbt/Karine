# Karine — project context

## Önce oku

**Durum ve sıra:** `Docs/STATUS.md` (tek sayfa) → `Docs/PHASE_PLAN.md` (hangi iş, hangi sırayla) → `Docs/ROADMAP.md` (ayrıntılı liste).
**Tasarım kanonu:** `Docs/MASTER_GAME_CONTEXT.md` + `Docs/DESIGN_AMENDMENTS.md` (sonraki düzeltmeler günlüğü — yeni karar buraya yazılır).
**Destekleyici:** `Docs/CAREER_AND_CHARACTER.md`, `Docs/VISUAL_DIRECTION.md`, `Docs/WORLD_OPENINGS.md`.
**Mevcut teknik gerçek:** `Docs/Architecture.md`. **Teknik borç ve kanıt:** `Docs/AUDIT_2026-09-25.md`.
`Docs/BUBE_GAME_MASTER_CONTEXT.md` ve `Docs/DESIGN_DECISIONS.md` destekleyicidir, kanon değildir. `Docs/NARRATIVE_SYSTEM_AUDIT.md` eski bir denetimdir; kendi eskimiş bölümlerini işaretler. `Docs/Archive/CONVERSATION_TEXT_*.md` erken tartışmayı saklar; oradaki erken öneriler sonraki kullanıcı kararlarıyla geçersiz olmuş olabilir.

## Değişmeyen oynanış kuralı

Oyuncu güdümlü soruşturma: bir ifade yeni bir kaynağı **erişilebilir yapar**, ama oyuncuyu oraya götürmez ve sıradaki adımı söylemez. Oyuncu kaynakları masadan kendisi açar, iddiaları kanıtla kendisi karşılaştırır, yeni açılan soruları sorar ve gerekçeli bir sonuç gönderir. Gizli karakter durumları, otomatik yalan/çelişki etiketi veya fail ipucu **gösterilmez**. Dosya #001 metin/sinyal CCTV dökümünü korur ve dört kayıtlı giriş anı için isteğe bağlı görüntü ekler; sinyal boşluğunun görüntüsü yoktur. Sonraki vakalar animasyonlu CCTV'yi yalnız vaka tasarımı gerektirirse kullanır. Yeni vakalar veri güdümlü olmalı ve önceki soruşturma yeteneklerini korumalıdır.

## Durum işaretleri — zorunlu

`ROADMAP.md`'de:
- `[ ]` yapılmadı
- `[~]` kodlandı, statik doğrulama geçti, **Play Mode/cihazda kanıtlanmadı**
- `[x]` Unity'de veya cihazda **gözlenerek** doğrulandı

Bir maddeyi `[x]` yapmak için "derleniyor" veya "içerik doğrulaması geçti" yetmez. Davranış görülmüş olmalı. Statik doğrulama en fazla `[~]` hak eder.

## Oturum sonu

`ROADMAP.md` işaretlerini ve tarihini güncelle, `STATUS.md`'yi yenile, yeni tasarım kararını `DESIGN_AMENDMENTS.md`'ye, yeni teknik gerçeği `Architecture.md`'ye yaz. Bir aşama "Bitti ölçütü" gerçekleşmeden kapalı ilan edilmez.

## Bilinen durum (25 Eylül 2026)

**Faz 0 bitti.** Oyun adı **Karine** (`Docs/NAMING.md`); depo `github.com/aydinbt/Karine`, çalışma klasörü `bubeGame/karine-mobile`; paket kimliği `com.bubedigital.karine`; IL2CPP + Android target SDK 35; `com.unity.test-framework` ve üç asmdef kurulu; EditMode testleri Editor'da koşturuldu ve geçti.

**Faz 1 bitti.** İçerik doğrulayıcı `Assets/Bube/Editor/Validation/` altında dokuz dosyaya ayrıldı (`ContentValidator` orkestratör, vakaya özel iddialar `Case001Rules`/`Case002Rules` içinde, kanca sözlüğüyle bağlanıyor); ilk hatada durmak yerine `ValidationReport` tüm bulguları topluyor. 51 test (46 EditMode + 5 PlayMode) tek komutla koşuyor ve geçiyor. Ayrıntı: `Docs/Architecture.md` → "İçerik doğrulama".

**Faz 2 başladı.** Beş performans düzeltmesi girildi (görev önbelleği, güvenli alan ve rozet yazımları, yüklem temsilcileri, sözlükle indeksli `Locale`) — hepsi `[~]`. Oynanış betiği: `Docs/PLAYTEST_001.md`. Vaka teklifi tam ekran olmaktan çıkıp masadaki gelen evrak tepsisine taşındı (yanıp sönen rozet, önizleme, kabul) ve akış başsız Play Mode testiyle **gözlendi**.

**Açık içerik ihlali:** `DeskReference.png` görselinin içinde gerçek kurum adı ve polis armasına benzer rozet gömülüdür. Üst şerit artık her ekranda örtülüyor ama terminaldeki yazı ve armalar duruyor; **görselin yenilenmesi gerekiyor.** Metin denetimi görsellerin içini göremez.

**Sıradaki darboğaz:** Masaya varış sinematiği, vaka teklifi ve masa yerleşimi 25 Eylül 2026'da Play Mode'da gözlendi. Ama Dosya #001 hâlâ **baştan sona** oynanmadı — soruşturma, sorgu, kanıt eşleme ve gerekçeli sonuç gönderme adımları kanıtlanmadı; **bu adım kullanıcıya ait**. M1 ve M2'nin bitiş ölçütleri buna bağlıdır. Keystore maddesi Faz 2'den Faz 5'e taşındı: geliştirme derlemesini Unity kendi hata ayıklama anahtarıyla imzalar.

## Testleri koşmak

```bash
Tools/run-tests.sh            # EditMode + PlayMode
Tools/run-tests.sh EditMode   # yalnız biri
```

Unity Hub'ın (ya da Editor'ün) açık olması gerekir: batchmode kendi lisans istemcisini kuramıyor, betik çalışan Unity süreçlerinden oturum kanalını okuyup ona bağlanıyor. Proje geçici bir kopyaya alınır, böylece Editor açıkken de koşar.

**Önceki bir notu düzeltmek gerekiyor:** "bu makinede batchmode lisansı çalışmıyor" iddiası yanlıştı. İki ayrı hata vardı — varsayılan lisans kanalı adı aranıyordu (Hub oturum kanalı geçirilmeli) ve `-runTests` ile `-quit` birlikte kullanılıyordu (`-quit`, testler başlamadan Unity'yi kapatır ve sonuç dosyası hiç yazılmaz).

Salt derleme kontrolü lisans bile gerektirmez: `Docs/Architecture.md` → "Başsız derleme doğrulaması".

Ayrıntı ve teknik borç: `Docs/AUDIT_2026-09-25.md`.
