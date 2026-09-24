> **Kanonik oyun tasarımı:** [MASTER_GAME_CONTEXT.md](MASTER_GAME_CONTEXT.md) + [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md).
> `BUBE_GAME_MASTER_CONTEXT.md` destekleyici sohbet kaydıdır, kanon değildir.
> Bu dosya **mevcut kodun gerçek durumunu** anlatır; tasarım hedefi değildir.
> **Son doğrulama:** 25 Eylül 2026 — kaynak kod okunarak. Denetim: [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md).

# Karine — teknik mimari (mevcut hâl)

Unity **6000.3.17f1**. Paketler yalnızca: `uielements`, `jsonserialize`, `video`, `imgui`, `ugui`.
**Test framework paketi yoktur** — bu yüzden depoda çalıştırılabilir otomatik test yoktur (bkz. Faz 1).

## Çalıştırma

Build listesindeki sahneler sırasıyla: `BootScene` → `MainMenuScene` → `OfficeScene` → `InterviewScene`.
Eski `Bootstrap.unity` build listesinde **değildir**; `BootScene` ile byte düzeyinde aynıdır ve yalnız geriye dönük uyumluluk için durur.

**Önemli gerçek:** `MainMenuScene`, `OfficeScene` ve `InterviewScene` dosyaları birbirinin **birebir kopyasıdır** (boş sahne + kamera). Sahneler bugün ayrı içerik taşımaz; yalnızca `SceneManager.GetActiveScene().name` üzerinden okunan bir **durum etiketi** işlevi görür. Tüm arayüzü tek bir kalıcı `BubeApp` MonoBehaviour'ı (`DontDestroyOnLoad`) UI Toolkit ile kodda kurar. Bu çalışır, ama "dört sahneli mimari" ifadesi bugünkü kodu fazla anlatır; sahne bölünmesi şu an yalnız isim kazandırır, karşılığında her geçişte tam sahne yüklemesi + tam UI yeniden kurulumu maliyeti getirir.

## Kod haritası

| Dosya | Satır | Sorumluluk |
| --- | --- | --- |
| `Runtime/Investigation.cs` | 199 | Unity bağımsız veri modeli + koşul/ilerleme mantığı. Test edilebilir çekirdek. |
| `Runtime/CaseSearch.cs` | 44 | Türkçe duyarlı kaynak araması. Unity bağımsız. |
| `Runtime/BubeApp.cs` | **2788** | Tüm ekranlar, UI Toolkit kurulumu, Resources yükleme, kayıt adaptörü, video, kariyer akışı. **Tek monolit.** |
| `Editor/ProjectSetup.cs` | 285 | Sahne/ayar üretimi için editör yardımcıları. |

`Assets/Bube/` altında **asmdef yoktur**; her şey `Assembly-CSharp` içinde derlenir. Bu, hem test assembly'si eklemeyi hem de derleme süresini olumsuz etkiler.

## Veri

- `Resources/Bube/config.json` — başlangıç vakası, dil, `worldIntros` tablosu.
- `Resources/Bube/career-rules.json` — güven eşikleri ve puan değişimleri.
- `Resources/Bube/Cases/case001.json` — 9 düğüm, 30 soru. Yayımlanmış.
- `Resources/Bube/Cases/case002.json` — 7 düğüm, 12 soru, `draft: true`. Oyuncuya açılmaz (kod `draft` bayrağına uyuyor: `BubeApp.cs:91`, `:2748`).
- `Resources/Bube/Locales/tr.json` — 577 benzersiz anahtar. **Tek dil.** Yinelenen anahtar yok, eksik anahtar yok; ~10 ölü anahtar var (kaldırılmış CCTV yan menüsünden kalma).
- `StreamingAssets/Bube/` — `world01_intro.mp4` (4.1 MB) + 4 CCTV klibi (12 MB).

Vaka verisi bütünlüğü (25 Eylül 2026 denetimi): case001 ve case002'de **sarkan referans, erişilemeyen düğüm veya erişilemeyen soru yoktur**. Bu kontrol elle yapıldı; depoda kalıcı bir doğrulayıcı olarak yaşamıyor — Faz 1'in işi.

## Kayıt

- `Application.persistentDataPath/bube-<caseId>-v1.json` — vaka ilerlemesi.
- `Application.persistentDataPath/bube-career-v1.json` — kariyer/güven/faks geçmişi.
- Yazım atomiktir (`.tmp` + `File.Replace`).
- **Göç yoktur.** Dosya adındaki `v1` sabit kodludur ve `Progress.version != 1` olduğunda kayıt sessizce atılıp sıfırdan başlanır (`Investigation.cs:44`). Şema değişirse oyuncu ilerlemesini kaybeder ve bunu fark etmez.
- `PlayerPrefs` yalnız `bube.instantText` için kullanılır.

## Bilinen teknik borç

Ayrıntı ve kanıt: [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md).

1. `Update()` her karede `RefreshInboxBadge()` → `AvailableAssignment()` → `Resources.Load` + `JsonUtility.FromJson<CaseData>` çalıştırır. Vaka kapandıktan sonra **her karede tam vaka JSON'u ayrıştırılır**.
2. `Locale.Get` 577 girişlik dizide `FirstOrDefault` ile doğrusal arama yapar; sayfa başına yüzlerce çağrı.
3. `BubeApp.cs` 2788 satırlık tek sınıf.
4. Kayıt şeması göçü yok.
5. Android/iOS için `applicationIdentifier` boş, `scriptingBackend` seçilmemiş, `AndroidTargetSdkVersion: 0` — mobil derleme bugün yapılamaz.

## Yeni vaka ekleme

Yeni `caseXXX.json` + `tr.json` anahtarları + önceki vakanın `nextCaseId` alanı. Çekirdek kod değişmez. Bu hedef case002 taslağıyla **veri düzeyinde** doğrulandı; oyuncu akışında (geçiş, kayıt, faks zamanlaması) henüz Play Mode'da kanıtlanmadı.
