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

**Sıradaki darboğaz:** Dosya #001 bugüne kadar bir kez bile Play Mode'da baştan sona oynanmadı. `Docs/PHASE_PLAN.md` Faz 2 bu işi tanımlar; M1 ve M2'nin bitiş ölçütleri buna bağlıdır.

**Bu makinede Unity batchmode lisans istemcisi bağlanmıyor** (`LicenseClient-<kullanıcı>` kanalı açılmıyor, 60 sn timeout döngüsü). `-runTests` ile komut satırından test koşturulamaz; testler Unity Editor'da `Window > General > Test Runner` üzerinden çalıştırılır.

Ayrıntı ve teknik borç: `Docs/AUDIT_2026-09-25.md`.
