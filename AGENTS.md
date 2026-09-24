# Karine (bubeGame/dedektif) — project context

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

Depo git'te izlenmiyor (0 takipli dosya) ve mobil derleme ayarları eksik. Oyun adı belirlenmedi (`Docs/NAMING.md`). Ayrıntı: `Docs/AUDIT_2026-09-25.md`.
