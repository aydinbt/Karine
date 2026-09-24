> **Kanonik oyun tasarımı:** [MASTER_GAME_CONTEXT.md](MASTER_GAME_CONTEXT.md) + [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md).
> `BUBE_GAME_MASTER_CONTEXT.md` destekleyici sohbet kaydıdır, kanon değildir.
> Bu dosya **mevcut kodun gerçek durumunu** anlatır; tasarım hedefi değildir.
> **Son doğrulama:** 25 Eylül 2026 — kaynak kod okunarak. Denetim: [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md).

# Karine — teknik mimari (mevcut hâl)

Unity **6000.3.17f1**. Paketler yalnızca: `uielements`, `jsonserialize`, `video`, `imgui`, `ugui`.
`com.unity.test-framework` kuruludur; depoda tek komutla koşan 48 test vardır (`Tools/run-tests.sh`).

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
| `Editor/ProjectSetup.cs` | 37 | `Bube/Validate Content` menü kabuğu + sahne/ayar üretimi. |
| `Editor/Validation/*.cs` | 9 dosya | İçerik doğrulama kuralları. Aşağıya bakın. |

`Assets/Bube/` altında dört asmdef vardır: `Bube.Runtime`, `Bube.Editor`, `Bube.Tests.EditMode`, `Bube.Tests.PlayMode`. Test assembly'leri `UNITY_INCLUDE_TESTS` kısıtıyla oyundan dışlanır.

## Veri

- `Resources/Bube/config.json` — başlangıç vakası, dil, `worldIntros` tablosu.
- `Resources/Bube/career-rules.json` — güven eşikleri ve puan değişimleri.
- `Resources/Bube/Cases/case001.json` — 9 düğüm, 30 soru. Yayımlanmış.
- `Resources/Bube/Cases/case002.json` — 7 düğüm, 12 soru, `draft: true`. Oyuncuya açılmaz (kod `draft` bayrağına uyuyor: `BubeApp.cs:91`, `:2748`).
- `Resources/Bube/Locales/tr.json` — 577 benzersiz anahtar. **Tek dil.** Yinelenen anahtar yok, eksik anahtar yok; ~10 ölü anahtar var (kaldırılmış CCTV yan menüsünden kalma).
- `StreamingAssets/Bube/` — `world01_intro.mp4` (4.1 MB) + 4 CCTV klibi (12 MB).

Vaka verisi bütünlüğü her test koşumunda otomatik doğrulanır (aşağıdaki "İçerik doğrulama"). case001 ve case002'de sarkan referans, erişilemeyen düğüm veya erişilemeyen soru yoktur.

## Kayıt

- `Application.persistentDataPath/bube-<caseId>-v1.json` — vaka ilerlemesi.
- `Application.persistentDataPath/bube-career-v1.json` — kariyer/güven/faks geçmişi.
- Yazım atomiktir (`.tmp` + `File.Replace`).
- **Göç yoktur.** Dosya adındaki `v1` sabit kodludur ve `Progress.version != 1` olduğunda kayıt sessizce atılıp sıfırdan başlanır (`Investigation.cs:44`). Şema değişirse oyuncu ilerlemesini kaybeder ve bunu fark etmez.
- `PlayerPrefs` yalnız `bube.instantText` için kullanılır.

## Bilinen teknik borç

Ayrıntı ve kanıt: [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md).

1. ~~`Update()` her karede tam vaka JSON'u ayrıştırır.~~ **Düzeltildi (Faz 2, 25 Eylül 2026).** Aşağıya bakın.
2. ~~`Locale.Get` doğrusal arama yapar.~~ **Düzeltildi (Faz 2).** Aşağıya bakın.
3. `BubeApp.cs` ~2800 satırlık tek sınıf. (Faz 4)
4. Kayıt şeması göçü yok. (Faz 3)
5. ~~Android/iOS derleme ayarları eksik.~~ **Düzeltildi (Faz 0).**

## İçerik doğrulama (Faz 1, 25 Eylül 2026)

Tek giriş: `ContentValidator.Run()` → `ValidationReport`. Menü komutu `Bube/Validate Content` ve `ContentValidationTests` ikisi de buradan geçer. Eskiden her şey `ProjectSetup.Validate()` içinde tek ~250 satırlık yöntemdeydi, ilk sorunda `throw` ediyordu ve `data.id=="case001"` blokları genel mantığın arasına serpilmişti.

| Dosya | İş |
| --- | --- |
| `ValidationReport.cs` | Bulgu toplayıcı. **Sorun** koşumu düşürür, **not** düşürmez. `Require`/`Forbid`/`Step`/`Scope`. |
| `ProjectRules.cs` | Sahne dosyaları, build sırası, fontlar, zorunlu dokular. `SceneOrder` sahne sırasının tek kaynağıdır. |
| `CaseChainRules.cs` | `initialCase`, `nextCaseId` hedefi, kendine gönderme, döngü, erişilemeyen vaka. |
| `LocaleRules.cs` | Yinelenen anahtar (sorun), ölü anahtar (not). |
| `CaseRules.cs` | Her vakaya uygulanan yapı kuralları: sarkan referans, eksik metin, görüşme bütünlüğü, CCTV bütünlüğü, zaman çizelgesi, sütun başına tam bir `correct`. |
| `WalkRules.cs` | Vakayı otomatik oynar: her düğümü açar, açılan her soruyu sorar, belge taleplerini gelen kutusundan geçirir. Erişilemeyen içeriğin tek kontrolü budur. |
| `Case001Rules.cs` / `Case002Rules.cs` | Yalnız o vakanın tasarımını sabitleyen iddialar. |
| `ContentValidator.cs` | Sırayı kurar; vakaya özel kancaları kimlik → yöntem sözlüğünden çağırır. |

**Neden iki evreli kanca:** tempo ve kilit sırası kontrolleri gezintiden **önce** koşmak zorunda (gezinti her şeyi açar), rapor/arama/rota kontrolleri **sonra** (oynanmış `Investigation` ve kayıt anlık görüntüsü gerekir). Bu yüzden kanca sözlüğü iki tanedir: `EarlyHooks`, `AfterWalkHooks`.

**Üçüncü vaka eklerken:** genel yolda hiçbir değişiklik gerekmez. Vakaya özel iddia varsa yeni bir `CaseNNNRules` yazılıp sözlüğe bir satır eklenir.

Yapı bozuksa gezinti çalıştırılmaz — yanıltıcı ikincil hatalar üretirdi. Öteki vakalar yine doğrulanır; bir vakanın bozuk olması ötekileri gizlemez.

## Kare başına iş — Faz 2 düzeltmeleri (25 Eylül 2026)

`Update()` her karede koşar ve tek kalıcı `MonoBehaviour` olduğu için her ekranda etkindir. Beş düzeltme girildi; hiçbiri davranış değiştirmez, yalnız yazma/ayırma sıklığını düşürür.

- **Görev önbelleği.** `AvailableAssignment()` sonucu `nextCaseId` başına bir kez ayrıştırılır (`assignmentCacheId` / `assignmentCache`). `Resources` içeriği çalışma anında değişmediği için önbellek eskimez; `null` sonuç da önbelleklenir.
- **Güvenli alan yazımları.** Dört `root.style.*` yazımı yalnız `Screen.safeArea`, ekran ölçüsü **ya da kök öge** değiştiğinde yapılır. Kök öge sahne başına yeniden kurulduğu için referans karşılaştırması şart — yoksa sahne geçişinde kenar boşlukları kaybolurdu.
- **Rozet yazımları.** `RefreshInboxBadge()` sayı ve rozet ögesi aynıysa hiçbir şey yazmaz. Rozet yeniden kurulduğunda öge referansı değişir, o yüzden sayı aynı olsa da yeniden yazılır — çağıranların hiçbiri değişmedi.
- **Yüklem temsilcileri.** `nodes.Count(game.IncomingDocument)` gibi çağrılar kare başına yeni `Func<Node,bool>` ayırıyordu. Temsilciler `Awake()`'te bir kez kurulur ve `game` alanını okur, bu yüzden `game` yeniden kurulduğunda (yeni kariyer, sıradaki vaka) geçerli kalır.
- **Sahne adı.** `SceneManager.GetActiveScene().name` kare başına iki kez okunuyordu; artık en fazla bir kez, o da gelen faks/belge varken.

`Locale.Get` artık tembel kurulan `Dictionary` kullanır (`Investigation.cs`). `[NonSerialized]` alan `JsonUtility` ile çakışmaz. Eski `FirstOrDefault` davranışı bilinçli korundu: yinelenen anahtarda **ilk giriş kazanır**, eksik anahtar ve `null` değer `[anahtar]` döndürür. Beşi `Locale.Get` için olmak üzere sekiz EditMode testi bunu sabitler.

## Testleri başsız koşmak

`Tools/run-tests.sh` EditMode ve PlayMode testlerini komut satırından koşar. İki incelik, ikisi de bir oturum kaybettirdi:

1. **Lisans kanalı.** Unity batchmode kendi lisans istemcisini kuramıyor ve varsayılan `LicenseClient-<kullanıcı>` kanalını arayıp 60 sn'de zaman aşımına düşüyor. Çözüm: Unity Hub ya da Editor açıkken var olan oturum kanalına bağlanmak (`-licensingIpc LicenseClient-<oturum>`); betik kanal adını çalışan süreçlerden okur.
2. **`-quit` kullanılmaz.** `-runTests` ile birlikte verilirse Unity testler başlamadan çıkar, çıkış kodu 0 döner ve sonuç dosyası hiç yazılmaz — sessiz bir yanlış "başarı".

Editor aynı projeyi kilitlediği için betik projeyi geçici bir dizine kopyalayıp orada koşar. PlayMode testleri `-nographics` ile de koşuyor; grafik bağlamı gerektiren bir test eklenirse bayrak kaldırılmalı.

`Packages/manifest.json`'daki `testables` girişi kaldırıldı: `com.unity.test-framework`'ün kendi örnek testlerini (`NewPlayModeTest…`) her koşuma katıyordu. Kendi testlerimiz `Assets/` altında olduğu için bu girişe gerek yok.

## Başsız derleme doğrulaması

Yalnız derlemeyi kontrol etmek için lisans bile gerekmiyor. Unity'nin kendi Roslyn'i ve Bee'nin ürettiği yanıt dosyaları doğrudan kullanılabilir:

```bash
S=/tmp/karine-compile; mkdir -p $S
D="/Applications/Unity/Hub/Editor/6000.3.17f1/Unity.app/Contents/Resources/Scripting"
for a in Bube.Runtime Bube.Editor Bube.Tests.EditMode; do
  R=$(find Library/Bee/artifacts -name "$a.rsp" | head -1)
  sed -e "s#^-out:.*#-out:\"$S/$a.dll\"#" -e "s#^-refout:.*#-refout:\"$S/$a.ref.dll\"#" "$R" > $S/$a.rsp
  "$D/NetCoreRuntime/dotnet" "$D/DotNetSdkRoslyn/csc.dll" "@$S/$a.rsp"
done
```

`-out`/`-refout` yönlendirilmezse çıktı `Library/Bee/artifacts` içine yazılır ve Unity'nin önbelleğine dokunur — her zaman yönlendirin. Bu yol **yalnız derlemeyi** doğrular; davranış için `Tools/run-tests.sh` gerekir.

## Test kapsamı

- `Assets/Bube/Tests/EditMode/ContentValidationTests.cs` — `Bube/Validate Content` doğrulayıcısı, `config.json` kimliği, metin anahtarları ve `Locale.Get` davranışı.
- `Assets/Bube/Tests/EditMode/ValidationReportTests.cs` — doğrulayıcının kendisi: bilerek bozulmuş bellek içi vakada **tüm** bulguların toplandığı, sütun başına tam bir `correct`, yinelenen dil anahtarı, `nextCaseId` zinciri (eksik hedef, döngü, kendine gönderme) ve yayımlanan içeriğin temiz geçmesi.
- `Assets/Bube/Tests/EditMode/SaveSchemaTests.cs` — `Progress`/`CareerProgress` JSON gidiş-dönüşü; eksik, boş, başka vakaya ait ve bilinmeyen sürümlü kayıtla yükleme.
- `Assets/Bube/Tests/EditMode/TimelineAndGatingTests.cs` — zaman çizelgesi kilitleri, `PinTimeline`/`UnpinTimeline` eşgüçlülüğü ve kayıttan sağ çıkması, soru ve kaynak açılma koşulları.
- `Assets/Bube/Tests/EditMode/CaseFlowTests.cs` — Dosya #001'in soruşturma mantığı: kilit zinciri, takip görüşmelerine giden iki rota, yarım görüşme, yanlış kaynak sunma, talep gecikmesi, kayıt gidiş-dönüşü, rapor değerlendirmesinin üç sonucu, kapanmış vaka. `Investigation.cs` Unity'den bağımsız olduğu için bunların hiçbiri Play Mode gerektirmiyor.
- `Assets/Bube/Tests/PlayMode/CaseOfferFlowTests.cs` — vaka teklifinin masadaki akışı: kabul edilmemiş vakada masada yalnız gelen evrak tepsisi açık, tepside önizleme okunuyor, kabul düğmesine gerçek bir tıklamayla basılınca vaka kabul edilip masa tamamen açılıyor. Eski tam ekran `CaseOffer()`'ın geri gelmediği de sabitlendi. Testler `BubeApp`'in özel üyelerine yansımayla erişiyor; `InboxPage`'in iki aşırı yüklemesi olduğu için parametre sayısına göre seçiliyor.
- `Assets/Bube/Tests/PlayMode/BootSmokeTests.cs` — `BootScene` açılıyor, `BubeApp` arayüzü kuruyor, sahne geçişinde tek örnek olarak hayatta kalıyor, güvenli alan kenar boşlukları yazılıyor, konsolda hata yok.

Toplam 48 test: 43 EditMode + 5 PlayMode.

Kapsam dışı ve gözle doğrulanması gerekenler: video oynatma, çentik/güvenli alan görünümü, dokunma hedefi boyutları, Türkçe glifler, klavye davranışı, kare hızı ve okunabilirlik. Bunlar `Docs/PLAYTEST_001.md`'de.

## Vaka teklifi akışı

Ayrı bir tam ekran teklif ekranı **yoktur** (`CaseOffer()` kaldırıldı). Kabul edilmemiş vaka, `InboxPage()` içinde `InboxEntry.offer` alanı dolu olan en üstteki okunmamış evrak olarak listelenir; sağ sütun `offer.subtitle` + `offer.summary` önizlemesini ve `offer.accept` düğmesini çizer, düğme `AcceptCase()` → `Save()` → `Desk()` yapar. `Desk()` kabul edilmeden yalnız tepsi ve ana ekran kısayolunu açar. Rozet açık teklifi de sayar ve `badge.schedule.Execute(...).Every(520)` ile yanıp söner — zamanlayıcı rozetin paneline bağlı olduğu için ekran değişince kendiliğinden durur. `FirstDeskArrival()` kendi masa görselini çizmez, `Desk()`'i arka plan alır.

Bunun bir yan etkisi var: `Assets/Bube/Resources/Bube/DeskReference.png` artık yalnız `Desk()` içinden yükleniyor ve orası üst şeride opak bir başlık çizdiği için görselin içine gömülü kurum şeridi hiçbir ekranda görünmüyor. Terminaldeki yazı ve armalar hâlâ görselin içinde; ayrıntı `Docs/DESIGN_AMENDMENTS.md`.

## Yeni vaka ekleme

Yeni `caseXXX.json` + `tr.json` anahtarları + önceki vakanın `nextCaseId` alanı. Çekirdek kod değişmez. Bu hedef case002 taslağıyla **veri düzeyinde** doğrulandı; oyuncu akışında (geçiş, kayıt, faks zamanlaması) henüz Play Mode'da kanıtlanmadı.
