# Karine — durum özeti

**Son güncelleme:** 25 Eylül 2026
**Bu dosya:** projeye bakan herkesin ilk okuyacağı tek sayfa. Ayrıntı için [ROADMAP.md](ROADMAP.md), kanıt için [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md), ileri plan için [PHASE_PLAN.md](PHASE_PLAN.md).

## Tek cümle

Dosya #001'in tüm soruşturma döngüsü **kodlanmış ve içerik doğrulamasından geçmiş** durumda; henüz **bir kez bile Play Mode'da baştan sona oynanmadı**. Faz 0 tamamlandı; **Faz 2 başladı** — performans düzeltmeleri girildi, oynanış betiği hazır ([PLAYTEST_001.md](PLAYTEST_001.md)).

## Kimlik

- **Oyun adı:** Karine — 25 Eylül 2026'da karara bağlandı ([NAMING.md](NAMING.md)).
- **Stüdyo:** bubeGames · **Oyun içi kurum:** bube Police / BPS (değişmedi, kurmaca kurum adıdır).
- **Paket kimliği:** `com.bubedigital.karine` · **Depo:** `github.com/aydinbt/Karine`

## Şu an nerede

| Aşama | Durum |
| --- | --- |
| **Faz 0 — Zemin** | **Bitti ve doğrulandı** |
| **Faz 2 — Gerçekten oyna** | **Başladı** — perf düzeltmeleri kodlandı `[~]`, oynanış bekliyor |
| Aşama 1 — Temel yapı | Kod tamam, cihaz doğrulaması açık |
| M1 — Dosya #001 döngüsü | Kod ~tamam, **Play Mode doğrulaması açık** |
| M2 — Soruşturmayı oyuna çevirme | Kod büyük ölçüde tamam, doğrulama açık |
| M3 — Vaka ekleme & kayıt | case002 taslak hâlde çalışıyor; kayıt göçü yok |
| M4 — Mobil kalite kapısı | Derleme ayarları hazır; cihaz testi başlamadı |
| M5 — Görsel ve ses | Ertelendi |
| M6 — Sonraki sistemler | Beklemede |

## Sayılarla

- Kod: 3.353 satır C# (`BubeApp.cs` tek başına 2.788)
- Vaka #001: 9 düğüm, 30 soru — bütünlük denetiminden temiz geçti
- Vaka #002: 7 düğüm, 12 soru — `draft: true`, oyuncuya kapalı
- Türkçe metin: 577 anahtar, eksik 0, yinelenen 0, ölü ~10
- Diller: 1 (tr)
- Assembly: 3 (`Bube.Runtime`, `Bube.Editor`, `Bube.Tests.EditMode`) — hepsi 0 hatayla derleniyor
- EditMode testi: 8 (3 duman + 5 `Locale.Get` testi; asıl kapsam Faz 1)

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

1. **Dosya #001 hiç baştan sona oynanmadı** — M1 ve M2'nin bitiş ölçütleri buna bağlı (Faz 2). **Asıl darboğaz budur.**
2. **Android keystore yok** — imzalı *mağaza* sürümü üretilemez. **Düzeltme:** cihaza geliştirme derlemesi kurmak için keystore gerekmiyor (Unity hata ayıklama anahtarıyla imzalar), bu yüzden madde Faz 2'den **Faz 5'e** taşındı; parola kullanıcıya aittir.
3. **Kayıt şeması göçü yok** — `version != 1` olduğunda ilerleme sessizce siliniyor (Faz 3).
4. ~~**Performans:** `Update()` her karede tam vaka JSON'u ayrıştırıyor; `Locale.Get` doğrusal arama yapıyor.~~ **Kodlandı (Faz 2), Play Mode'da gözlenmedi `[~]`** — beş düzeltme: görev önbelleği, güvenli alan yazımları, rozet yazımları, yüklem temsilcileri, sözlükle indeksli `Locale`.
5. **Unity batchmode lisansı bu makinede çalışmıyor** — testler yalnız Editor'dan koşturulabiliyor; ileride CI kurulacaksa bu çözülmeli.

## Sıradaki iş

[PLAYTEST_001.md](PLAYTEST_001.md) betiğini Unity Play Mode'da baştan sona koş, sonra bir Android telefonda yinele. Bu benim yapamadığım tek adım: Play Mode Editor gerektiriyor ve bu makinede batchmode lisansı bağlanmıyor. Betikteki sonuç sütunları doldurulunca `[~]` satırları `[x]` olur ve M1/M2 kapanabilir.
