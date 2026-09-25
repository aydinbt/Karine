> **Kanonik oyun tasarımı:** [MASTER_GAME_CONTEXT.md](MASTER_GAME_CONTEXT.md) + [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md).
> `BUBE_GAME_MASTER_CONTEXT.md` destekleyici sohbet kaydıdır, kanon değildir.
> Bu dosya **mevcut kodun gerçek durumunu** anlatır; tasarım hedefi değildir.
> **Son doğrulama:** 25 Eylül 2026 — kaynak kod okunarak. Denetim: [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md).

# Karine — teknik mimari (mevcut hâl)

Unity **6000.3.17f1**. Paketler yalnızca: `uielements`, `jsonserialize`, `video`, `imgui`, `ugui`.
`com.unity.test-framework` kuruludur; depoda tek komutla koşan 51 test vardır (`Tools/run-tests.sh`).

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
- `Resources/Bube/Locales/tr.json` — ortak metin. **Tek dil.** Vaka metni artık burada değil: `tr.case001.json` (9 anahtar) ve `tr.case002.json` (86 anahtar) dosyalarında durur ve `LocaleLoader` yüklemede birleştirir. Çakışan anahtarda ortak dosya kazanır ve doğrulama bunu iki dosya adıyla bildirir. Yinelenen anahtar yok, eksik anahtar yok; ~10 ölü anahtar var (kaldırılmış CCTV yan menüsünden kalma).
- `Resources/Bube/Audio/` — dokuz klip. `AudioDirector` klip adlarını buradan arar (`ui_press`, `ui_typewriter`, `ui_stamp`, `ui_notification`, `ui_chat`/`ui_chat_low`, `menu_theme`, `desk_theme`, `room_interview` + vakanın `ambienceId`si). Eksik klip oyunu durdurmaz, sessiz geçer ve bir kez not düşer.
- `StreamingAssets/Bube/` — `world01_intro.mp4` (4.1 MB) + 4 CCTV klibi (12 MB).

Vaka verisi bütünlüğü her test koşumunda otomatik doğrulanır (aşağıdaki "İçerik doğrulama"). case001 ve case002'de sarkan referans, erişilemeyen düğüm veya erişilemeyen soru yoktur.

## Kayıt

- `Application.persistentDataPath/bube-<caseId>-v1.json` — vaka ilerlemesi.
- `Application.persistentDataPath/bube-career-v1.json` — kariyer/güven/faks geçmişi.
- Yazım atomiktir (`.tmp` + `File.Replace`).
- **Göç vardır** (`SaveMigration`, `Investigation.cs`). `ProgressVersion` / `CareerVersion` bugünkü şemayı söyler; `Migrate` her kaydı bir sonuca bağlar: `Loaded` (aynı sürüm), `Migrated` (daha eski — basamak basamak yükseltilir, ilerleme korunur), `FromFuture` (daha yeni — çevrilemez), `OtherCase` (başka vakanın kaydı), `Fresh` (kayıt yok). Sonuç `Investigation.StateOutcome` / `CareerOutcome` ile dışarı verilir.
- Şema büyüdüğünde `Migrate` içine bir basamak eklenir (`if(save.version<2){…;save.version=2;}`); basamaklar sırayla koştuğu için çok eski bir kayıt da bugüne tırmanır. Sürüm `0`, sürüm alanı hiç yazılmamış ilk kayıtlardır; şema aynı olduğu için damgalanmakla yükselirler.
- **`FromFuture` kayıt silinmez.** `BubeApp.SetAside` dosyayı `<yol>.newer` olarak yana kaldırır (böylece `Save()` üzerine yazmaz) ve oyuncuya `save.fromFuture` satırı gösterilir. Dosya adındaki `v1` hâlâ sabit kodludur; sürüm bilgisi dosyanın içinden okunur.
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
- `Assets/Bube/Tests/EditMode/TypographyTests.cs` — yazı ölçeği: `Snap` eşgüçlülüğü, eşitlikte büyük basamak, ve arayüzde ölçeği atlayan çıplak punto kalmaması.
- `Assets/Bube/Tests/EditMode/SaveSchemaTests.cs` — `Progress`/`CareerProgress` JSON gidiş-dönüşü; eksik, boş, başka vakaya ait ve bilinmeyen sürümlü kayıtla yükleme.
- `Assets/Bube/Tests/EditMode/TimelineAndGatingTests.cs` — zaman çizelgesi kilitleri, `PinTimeline`/`UnpinTimeline` eşgüçlülüğü ve kayıttan sağ çıkması, soru ve kaynak açılma koşulları.
- `Assets/Bube/Tests/EditMode/CaseFlowTests.cs` — Dosya #001'in soruşturma mantığı: kilit zinciri, takip görüşmelerine giden iki rota, yarım görüşme, yanlış kaynak sunma, talep gecikmesi, kayıt gidiş-dönüşü, rapor değerlendirmesinin üç sonucu, kapanmış vaka. `Investigation.cs` Unity'den bağımsız olduğu için bunların hiçbiri Play Mode gerektirmiyor.
- `Assets/Bube/Tests/PlayMode/CaseOfferFlowTests.cs` — vaka teklifinin masadaki akışı: kabul edilmemiş vakada masada yalnız gelen evrak tepsisi açık, tepside önizleme okunuyor, kabul düğmesine gerçek bir tıklamayla basılınca vaka kabul edilip masa tamamen açılıyor. Eski tam ekran `CaseOffer()`'ın geri gelmediği de sabitlendi. Testler `BubeApp`'in özel üyelerine yansımayla erişiyor; `InboxPage`'in iki aşırı yüklemesi olduğu için parametre sayısına göre seçiliyor.
- `Assets/Bube/Tests/PlayMode/BootSmokeTests.cs` — `BootScene` açılıyor, `BubeApp` arayüzü kuruyor, sahne geçişinde tek örnek olarak hayatta kalıyor, güvenli alan kenar boşlukları yazılıyor, konsolda hata yok.

Toplam 51 test: 46 EditMode + 5 PlayMode.

Kapsam dışı ve gözle doğrulanması gerekenler: video oynatma, çentik/güvenli alan görünümü, dokunma hedefi boyutları, Türkçe glifler, klavye davranışı, kare hızı ve okunabilirlik. Bunlar `Docs/PLAYTEST_001.md`'de.

`Desk()` arka planı `Bube/DeskReference`'tır. Terminaldeki kurum adı görselden silinmiştir ve yerine bir şey yazılmaz; ekranda yalnız "CCTV ARŞİVİ" kutusu kalır, arma da silinmiştir. Künye şeridi tamamen opaktır — saydam bırakılırsa görsele gömülü kurum yazısı altından sızıyor.

## Yazı ölçeği

Yazı tipi tektir (IBM Plex Mono, kök öğeden miras). Punto da tektir: `Typography.Steps` dokuz basamaktır ve `Typography.Snap` ölçek dışı her değeri en yakın basamağa oturtur. `Text(...)` ve `Button(...)` boyutu **her zaman** `Snap`'ten geçirir, yani ölçek dışı bir punto ekrana ulaşamaz — yeni bir çağrı yeri eklerken ölçeği hatırlamak gerekmez. `TypographyTests` çıplak `style.fontSize=<sayı>` kalmadığını kaynak taramasıyla sabitler.

## Sinematikler

İki video var, ikisi de `Assets/StreamingAssets/Bube/` altında ve Git LFS ile saklanıyor: `world01_intro.mp4` (dünya açılışı) ve `case001_arrival.mp4` (dosyanın masaya bırakılışı). İkisi de `VideoPlayer` ile `RenderTexture`'a çizilip tam ekran `ScaleAndCrop` gösterilir.

Üretici filigranı **"Geç" düğmesiyle örtülür**. Filigran yeri veriden gelir — `CornerMark { x, y, w, h }`, filmin kendi karesine oranlı (0..1). `PositionIntroSkip` filmin ekrandaki gerçek dikdörtgenini 16:9'dan hesaplar, yani telefonun eni ne olursa olsun düğme doğru yere oturur. Sabit 1280×720 varsayımı kaldırıldı; `world01` değerleri piksel eşdeğer kaldı.

Masaya bırakılış videosu kendi içinde siyaha kapanır; masa da siyahtan 1,25 saniyede açılır (`OpenEyes`) ve açılma boyunca kaplayan gölge dokunmaları tutar. Masaya bırakılış videosu oynatılamazsa elle çizilmiş `FirstDeskArrival` animasyonu devreye girer. "Geç" ile videonun bitişi aynı yere gelir; `deskArrivalDone` bayrağı ikinci çağrıyı yutar.

## Vaka teklifi akışı

Ayrı bir tam ekran teklif ekranı **yoktur** (`CaseOffer()` kaldırıldı). Kabul edilmemiş vaka, `InboxPage()` içinde `InboxEntry.offer` alanı dolu olan en üstteki okunmamış evrak olarak listelenir; sağ sütun `offer.subtitle` + `offer.summary` önizlemesini ve `offer.accept` düğmesini çizer, düğme `AcceptCase()` → `Save()` → `Desk()` yapar. `Desk()` kabul edilmeden yalnız tepsi ve ana ekran kısayolunu açar. Rozet açık teklifi de sayar ve `badge.schedule.Execute(...).Every(520)` ile yanıp söner — zamanlayıcı rozetin paneline bağlı olduğu için ekran değişince kendiliğinden durur. `FirstDeskArrival()` kendi masa görselini çizmez, `Desk()`'i arka plan alır.

Bunun bir yan etkisi var: `Assets/Bube/Resources/Bube/DeskReference.png` artık yalnız `Desk()` içinden yükleniyor ve orası üst şeride opak bir başlık çizdiği için görselin içine gömülü kurum şeridi hiçbir ekranda görünmüyor. Terminaldeki yazı ve armalar hâlâ görselin içinde; ayrıntı `Docs/DESIGN_AMENDMENTS.md`.

## Yeni vaka ekleme

Çekirdek kod değişmez. Gereken dosyalar:

1. `Resources/Bube/Cases/caseXXX.json` — düğümler, sorular, zaman çizelgesi, kararlar, özet.
2. `Resources/Bube/Locales/tr.caseXXX.json` — o vakanın bütün metni. Ortak `tr.json`a dokunulmaz; aynı anahtarı yeniden tanımlamak doğrulamada hata verir.
3. Önceki vakanın `nextCaseId` alanı.
4. İsteğe bağlı: kişi portreleri (`Resources/Bube/Characters/<personId>.png`), vaka görselleri, `ambienceId` ile oda sesi.

Portre PNG'si yoksa piksel portre çizilir ve tonları vaka verisinden gelir (`Node.portrait`: `hairHex`, `skinHex`, `shirtHex`, `longHair`, `moustache`); alan boşsa varsayılan kullanılır. Doğrulayıcı vakaya özel C# kuralı **istemez** — `ContentValidator`ın kancaları isteğe bağlıdır, yeni vaka genel yoldan geçer.

Bu hedef case002 taslağıyla **veri düzeyinde** doğrulandı; oyuncu akışında (geçiş, kayıt, faks zamanlaması) henüz Play Mode'da kanıtlanmadı.

## Ses

`SoundSettings` sesin kararlarını (üç kademe: kapalı/kısık/açık, kazançları ve `PlayerPrefs` anahtarları) tutar; `AudioDirector` çalar. Ayrım kasıtlı: düzey mantığı Editor testinde `AudioSource` olmadan sınanıyor. Üç kanal karışmaz — müzik ve oda ortamı döngülü, efekt üst üste binebilir. Oda sesi `EnsureScene`ten gelir; vaka kendi ortamını söyleyebilir (`CaseData.ambienceId`).

**Menü üstü kart telefon biçimine göre ölçülür.** `MenuOverlay` (ayarlar, hakkında, yeniden deneme, yönlendirme) kartı kenarlardan sabit %27 içeride kuruyordu: geniş ekranda makul, telefon dikey tutulduğunda daracık bir şerit. Artık pay ekranın biçiminden geliyor (dikeyde %5, yatayda %24) ve kartın içi `ScrollView` — başlık sabit kalır, içerik akar, uzun ayar listesi kesilmez. Ayarlarda ses kademeleri üç sıkışık radyo yerine kit'in **sekme şeridi** (`KarineUI.Tabs`, eşit genişlik) ile seçiliyor; bölümler alt başlık ve çizgiyle ayrılıyor.

**Masada müzik çalar, oda gürültüsü değil.** İlk kurulum masaya bir hava hışırtısı koyuyordu (`room_office`); oyun baştan sona oynandığında bunun yorucu olduğu görüldü, üstelik masada oyuncu **okuyor** ve okumaya eşlik eden şey gürültü değil müziktir. Masa artık `desk_theme` çalıyor, `room_office` silindi. Görüşme odası ortam sesiyle kalıyor: orada oyuncu okumuyor, konuşuyor.

**Akış klibi yüklenmeden çalmaz ve hata da vermez.** Ana menü müziğinin hiç duyulmamasının sebebi buydu: `menu_theme` akış (`loadType: 2`) + arkaplan yüklemesiyle içe aktarılıyordu, `Play()` klip hazır olmadan çağrılıyordu ve Unity bunu **sessizce** geçiyordu. İki kapı kondu: döngü kliplerinin metası artık `preloadAudioData: 1` / `loadInBackground: 0`, `AudioDirector.Loop` da yüklenmemiş klibi `LoadAudioData()` ile yüklüyor. Ayrıca "kısık" kademesi 0,35'ten 0,55'e çıktı — 0,35'te müzik varsayılan ayarda duyulmuyordu.

Düğme sesi ekranların içine yazılmaz: `KarineUI.Sounded` kit düğmesinin kurucusunda durur, `KarineUI.Sound` temsilcisini `BubeApp` `AudioDirector`a bağlar. `ProjectRules` `Runtime/UI` içindeki her `new Button(` çağrısının `Sounded(` ile sarılı olmasını kilitler, yoksa yeni bir kit bileşeni sessiz kalır.

### Ses varlıkları sentezlenir

On bir klip `Assets/Bube/Resources/Bube/Audio/` altında (mono, 44.1 kHz, 16 bit WAV, Git LFS). **Kayıt değil, üretim:** `Tools/make-audio.py` hepsini sıfırdan sentezler (saf Python, harici bağımlılık yok — numpy ve ffmpeg gerekmiyor) ve `Tools/check-audio.py` ölçer.

```bash
python3 Tools/make-audio.py     # üretir
python3 Tools/check-audio.py    # ölçer, kusurda 1 döner
```

Bir tonu değiştirmek için yeni kayıt aranmaz; betikteki değer değişir ve yeniden koşulur. Sesler böylece **okunabilir**: `ui_stamp`ın neden tok olduğu kodda yazılı.

| dosya | ne | içe aktarım |
|---|---|---|
| `ui_press` | yumuşak düğme: üstü kapalı, yuvarlak, alçak "tup" | ADPCM, belleğe açılır |
| `ui_typewriter` | daktilo tuşu — **yalnız faks basılırken** | ADPCM |
| `ui_stamp` | mühürün lastiği: tok, tek, kesin | ADPCM |
| `ui_notification` | faksın küçük zili: anharmonik kısmiler + mekanizma tıkı | ADPCM |
| `menu_theme` | 32 s neo-noir döngü, Am–F–Dm–E | Vorbis, akış |
| `ui_chat` `ui_chat_low` | sohbet blibi: yumuşak sinüs + küçük çıngırak, iki varyant | ADPCM |
| `desk_theme` | 40 s masa müziği, Dm–Gm–B♭–A: menüden yavaş ve alçak | Vorbis, akış |
| `room_interview` | 24 s döngü: kapalı bir odanın havası, üst frekans yok | Vorbis, akış |

**Döngü dikişi** iki ayrı teknikle kapatılır, çünkü iki ayrı sorun var. Sürekli katmanlar (gürültü yatağı) `loop_noise` ile **çapraz geçirilir**; kuyruğu başa eklemek o bölgede seviyeyi 1,4 katına çıkarır. Çınlayan katmanlar (yankı, nota kuyruğu) `wrap_tail` ile başa **eklenir**, böylece döngü kendi kuyruğunun üstüne biner. Yankı ise iki kopya sürülüp ikincisi alınarak kararlı hâle getirilir (`steady_reverb`), yoksa oda döngü başında boş, sonunda dolu olur. Periyodik bileşenlerin frekansı döngü boyunda tam çevrim yapar (100 Hz × 24 s = 2400 çevrim), yoksa dikişte faz atlar.

`check-audio.py` kırpma, DC kayması, ölü sessizlik, seviye aralığı, dikişteki örnek atlaması ve döngü başındaki seviye kamburunu ölçer; menü ve masa müziğinde ayrıca dört akorun kökünü Goertzel ile ölçüp akora yabancı bir aralıktan yüksek olduğunu doğrular. Darbeli arayüz seslerinde seviye dosya boyu değil **en gürültülü 100 ms penceresi** üzerinden ölçülür, yoksa kuyruk sessizliği ölçümü yanıltır.

**Hangi ses nerede çalar** — bu eşleme sesin kendisi kadar önemli, çünkü doğru ses yanlış yerde yanlış sestir:

- `ui_press` her düğme (`KarineUI.Sounded`ın varsayılanı) — masadaki görünmez `Hotspot`lar, menü satırı ve CCTV'nin oynatma düğmeleri dâhil. Bu üçü bir süre **sessizdi**, çünkü ses kilidi yalnız `Runtime/UI` içine bakıyordu; kural artık bütün `Runtime`e bakıyor.
- `ui_typewriter` yalnız faks basılırken (`FaxPage`'in değerlendirme satırı). Arayüz düğmelerinde hiç yoktu.
- `AudioDirector.Chat` (iki blip varyantı) görüşmede karşıdakinin cümlesi belirirken, beş karakterde bir, karışık sırayla ve alçak (0,55 kazanç).

**Konuşma sesi neden bir blip.** Üç sürüm denendi ve üçü de kullanıcı kulağında düştü: sinüs yığını sentezleyici, formant sentezi insan sesi **taklidi**, klavye tuşu ise gürültülü ve yorucu duyuldu. Ortak hata taklitti — taklit, kaydın kendisi olmadıkça tekinsiz kalıyor, üstelik oyun yazıyla konuşuyor. Dördüncü sürüm hiçbir şeyi taklit etmiyor: yumuşak bir blip yalnızca "yeni bir satır geldi" diyor. Bu hem dürüst hem dayanıklı — yeni vakaların yeni kişileri için ses verisi gerekmiyor. Kişi başına perde türetmek de kalktı (`VoicePitch`); blip, karşıdakinin sesi değil.

- `ui_stamp` mühür, `ui_notification` gelen evrak.

`Typewriter(label, line, soundId, pitch, gain, every)` — metin harf harf yazılırken ne duyulacağı **satırın kime ait olduğuna** bağlıdır, o yüzden çağrı yerinden gelir.

**Yumuşaklık bir karardır.** İlk sürüm mekanik ve gergindi (klavye tıkı, duvar saati, floresan uğultusu, tavan çınlaması) ve sesi açan oyuncuyu ürkütüyordu. İkinci sürümde tık yuvarlandı, saat ve uğultular tamamen kaldırıldı, odalar –26 dBFS'e indi. Odalar arasındaki fark artık ses değil **renk**: görüşme odasında üst frekans yok, yani duvarlar yakın.

`ProjectRules` on bir klibin varlığını kilitler ve `CaseRules` vakanın `ambienceId`si için dosya arar: `AudioDirector` eksik klibi sessiz geçtiği için bir sesin silinmesi ya da adının yanlış yazılması başka hiçbir yerde duyulmazdı. Kural, `ui_press.wav` geçici olarak kaldırılıp düşmesi görülerek doğrulandı.

## Geri tuşu ve duraklatma

Android geri tuşu (ve masaüstünde Esc) `BubeApp.Update` içinde okunur ve `escapeBack`e gider. Her katman açıldığında hedefini `Back(...)` ile söyler ve hedef **ekranın kendi "geri" eyleminin aynısıdır**; ayrı bir gezinti ağacı tutulmaz. Ana menüde karşılığı yoktur, orada kit'in onay modalı açılır. `OnApplicationPause(true)` kayıt yazar.

## Reklam ve para kazanma

Ağ eklentisi projede **yok**; dikişi var. `IAdProvider` tek arayüz, `NoAdProvider` bugünün gerçeklemesi (hiçbir şey göstermez ve ödül vermez). `AdGateway` tek karar yeri: onay durumu (`AdConsent`), "reklam kaldırıldı" bayrağı ve `MayShow(placement, moment)` kuralı. Anlar `AdMoment`tan gelir, ekran adından değil.

- `CaseInterval` yalnız `CaseClosed` anında — çağrı yeri `BubeApp.Report.cs` → `OpenAssignment`.
- `RewardedGuidance` ve `RewardedRetry` yalnız `ReportRejected` anında.
- Soruşturma, sorgu, CCTV ve sinematik anları kapalıdır.
- Reklam kaldırıldıysa ağ hiç çağrılmaz, ama ödül reklamsız verilir.
- Ağ yokken ödül **verilmez**: "reklam yok" bedava ödül anlamına gelmemeli.

Ödüllü ipucu (`GuidancePage`) vakanın gerçeğini hiç görmez: yöntem hatırlatması (`guidance.method.*`, işin kuralları) ve oyuncunun kendi kapsamı (`Coverage`: açılabilir kaynaklardan kaçı açıldı, sorulabilir sorulardan kaçı soruldu, çizelgeye kaç satır alındı). `LocaleRules` `guidance.` ile başlayan her metinde kişi adı, kaynak başlığı ve karar etiketi geçmesini yasaklar; yasak sözcükler vaka metninden türetilir, elle listelenmez.

Ödüllü yeniden deneme (`Investigation.MayReopen` / `ReopenForRetry`, düğmesi `BubeApp.Desk.cs` → `AddRetryOffer`, hem gelen evrak tepsisindeki faksta hem `FaxPage`'de) kullanıcı kararıyla şöyle: **güven geri verilir, faks geçmişi başarısızlığı saklar.**

- İade tam o faksın götürdüğü kadardır (`departmentTrust -= fax.trustChange`) ve gerekiyorsa görevden ayrılmayı kaldırır; yoksa ödül hiçbir işe yaramazdı.
- Kayıt silinmez: `reviewHistory` satırı yerinde kalır, `reopened`/`trustRefunded` işaretlenir ve kariyer ekranında "yeniden açıldı" diye görünür. Sayımlar (desteklendi / eksik / yanlış suçlama) o satırı görmeye devam eder.
- İkinci deneme kendi satırını yazar; aynı vaka geçmişte iki satır tutabilir. `DeliverNextFax` yinelenmeyi artık "yeniden açılmamış satır var mı" diye sorar, arşiv de en son değerlendirmeyi okur.
- Yeniden açmak soruşturmayı sıfırlamaz ve **ipucu vermez**: okunanlar, sorulanlar ve çizelge durur; yalnız rapor alanları ve `closed` boşalır, yani vaka ikinci kez gerekçeli sonuç göndermeye açılır.
- Aynı faks bir kez iade eder (`MayReopen` `reopened`e bakar).

LevelPlay/AdMob kurulumu bir Unity Gaming Services oyun kimliği ve bir AdMob uygulama kimliği ister; ikisi de hesap açmayı gerektirdiği için kimlikler geldiğinde takılacak.


## Marka ve yazı tipleri

- **Logo:** `Assets/Bube/Resources/Bube/Art/KarineLogo.png` (2000×667, şeffaf). Kullanıcının verdiği dosya; pikselleri değiştirilmedi, yalnız webp→png dönüştürüldü. `KarineLogo.Hero` / `KarineLogo.Header` ile çizilir; ekranlar sadece genişlik verir, yükseklik `KarineLogo.AspectRatio`'dan türer.
- **İçe aktarım kilidi:** meta dosyasında `nPOTScale: 0`, `enableMipMap: 0`, `alphaIsTransparency: 1`, sıkıştırma kapalı. **Varsayılan `nPOTScale: 1` bu logoyu 2048×512'ye eziyordu.**
- **Projedeki bütün görsellerde kilitlendi.** On bir `.png.meta` dosyasının hepsi `nPOTScale: 0` oldu. Önceki hâlde Unity her görseli en yakın ikinin kuvvetine çekiyor ve iki ekseni ayrı ölçeklediği için oranı bozuyordu: 1672×941 arka planlar 2048×1024 (yatayda ~%12 esneme), 1199×1312 karakterler 1024×1024, 96×64 bayrak 128×64 (~%33 esneme). Artık görseller kendi boyutlarında gidiyor.
- **Kural doğrulayıcıda:** `ProjectRules` `Assets/Bube/Resources` altındaki her `.png.meta` dosyasını tarar ve `nPOTScale: 1` bulursa sorun yazar. Yarın eklenen bir görsel sessizce ezilemez. Kuralın gerçekten çalıştığı, bir metayı geçici olarak bozup düşmesi görülerek doğrulandı.
- **`BubeApp` parçalara ayrıldı.** 3.125 satırlık tek dosyaydı; artık konu başına sekiz `partial` dosya: `BubeApp.cs` (alanlar, yaşam döngüsü, kayıt, ekran ilkelleri), `.Menu`, `.Cinematics`, `.Desk`, `.Investigation`, `.Interview`, `.Cctv`, `.Report`. Tek `MonoBehaviour`, tek örnek, davranış aynı — bölünme mekaniktir. `ProjectRules.LongestRuntimeFile = 560` en uzun çalışma zamanı dosyasını sınırlar ve yalnız aşağı çekilir.
- **Yazı tipi rolleri** `FontSet.Load()` ile tek yerden çözülür: `Display` (AlfaSlabOne-Regular → Heading), `Heading` (RobotoSlab-ExtraBold → Arvo-Bold), `Body` (Inter-Regular → IBMPlexSans-Regular), `Mono` (IBMPlexMono). Kök öğenin ve `Text()`/`Button()`un yazısı **Body**'dir; mono yalnız `KarineUI.Technical` ile gelir. `KarineUI.Title` 28 punto ve üstünde `Display`e, altında `Heading`e gider (`KarineUI.DisplayFrom`). Dört rol dosyası da depodadır (LFS); `ProjectRules` varlıklarını **zorunlu** tutar, `FontSet.Missing` yalnız bir dosya silinirse konuşur. Roboto Slab'da ₺ glifi yok, metası IBM Plex Mono'ya yedeklenir. `Text()` ve `Button()` gövde rolünü, ekran başlıkları `Heading` rolünü kullanır.


## Arayüz tasarım sistemi

- **Kanon:** `Docs/Reference/UI_KIT.png` (görsel spesifikasyon) + `Docs/UI_KIT.md` (yazılı özet).
- `Assets/Bube/Runtime/UI/KarineTheme.cs` — palet (sekiz kit rengi + türetilmiş `Muted`/`Disabled`), boşluk ölçeği (4/8/12/18/24), köşe (`Radius = 2`), kenar kalınlığı, dokunma hedefi (48/56), devinim süreleri ve diegetic `Paper` katmanı. Renkler hex sabitten okunur; ayrıştırılamayan değer magenta olur, sessizce siyaha düşmez.
- `Assets/Bube/Runtime/UI/KarineUI.cs` — kit bileşenleri: `Button_` (Primary/Secondary/Ghost/Danger + basılı/devre dışı durumları), `IconButton`, `Icon`, `Panel`, `Row`, `Rule`, `Title`/`Subtitle`/`Body_`/`Technical`, `Badge`, `Dot`, `Tabs`, `DocumentNav`, `Modal`, `Notification`, `Progress`, `Tooltip`, `Radio`, `Meter`, `Counter`, `SkipButton`, `PaperButton`, `CloseButton`, `Border`/`Round`/`Enter`. Dosya üçe ayrıldı: `KarineUI.cs` (yazı tipi, tipografi, yüzey, düğme, simge, rozet), `KarineUI.Controls.cs` (Tabs/Radio/Meter/Counter/DocumentNav), `KarineUI.Layers.cs` (kâğıt katmanı, SkipButton, Modal, Notification, Progress, Tooltip). Prefab yoktur; "bileşen" burada `VisualElement` döndüren üretici demektir.
- `KarineUI.Fonts` `Awake`'te bir kez kurulur (`FontSet.Load()`); rol eksikse bileşenler yine çizilir.
- `BubeApp`in eski palet sabitleri (`Ink`, `Muted`, `Gold`, `Base`, `Card`, `Paper`) artık `KarineTheme`ye bağlı takma adlardır — tek dosya değişince yüzden fazla ekran birlikte kayar. `Button(...)` ve `Panel(...)` yardımcıları `KarineUI`ye devrediyor.
- **Ham renk kilidi:** `ProjectRules.RawColorBudget = 16` (156'dan indi; kalanlar piksel portre ve CCTV efektleri, yani oyun sanatı). `Assets/Bube/Runtime` altındaki (UI klasörü hariç) doğrudan `new Color(` sayısı bunu aşarsa doğrulama düşer; azalırsa doğrulayıcı bütçenin düşürülebileceğini not eder.
- **Punto kilidi:** `Assets/Bube/Runtime` altında (UI klasörü hariç) `Typography.Snap` veya `Mathf.Clamp` içermeyen bir `style.fontSize` ataması doğrulamayı düşürür ve dosya:satır olarak bildirilir.
- Kit ikonları `Resources/Bube/Art/Icons/` altındadır ve `ProjectRules.KitIcons` listesiyle varlıkları aranır.

## Ana menü

- Arka plan `Assets/StreamingAssets/Bube/main_menu_loop.mp4` (1920×1080, 10 sn, H.264/AAC, LFS). `VideoPlayer` → `RenderTexture(1920×1080)` → `Image(ScaleAndCrop)`; `isLooping = true`, ses kapalı.
- `menuPlayer` / `menuTexture` `BubeApp` alanlarıdır; `Home()` her çağrıldığında yeniden kurulmaz, yalnız görüntü ögesi eklenir. `Desk()` `StopMenuVideo()` çağırır.
- Video hata verirse `menuVideoFailed` işaretlenir ve durağan `Bube/MainMenuNight` görseline düşülür.
- Menü satırları `MenuRow(...)` ile çizilir (simge sütunu + etiket + öne çıkan satırda ok, 52 px dokunma hedefi). Etiketler `menu.row.*` anahtarlarında **büyük harfle saklanır** — Türkçe `ToUpper` tuzağına (I/İ) hiç girilmez.
- Simgeler `Resources/Bube/Art/Icons/menu_{continue,newCareer,settings,career,quit}.png`. `MenuIcon(...)` bunları arka plan görseli olarak (`BackgroundSizeType.Contain`, sola yaslı, 34×22 sütun) koyar ve `unityBackgroundImageTintColor` ile satırın tonuna boyar. Dosya yoksa sütun genişliği korunur, etiketler kaymaz; `ProjectRules` beşinin de varlığını arar.
- Stüdyo imzası sol sütunun değil, kökün çocuğudur: sağ alt köşede mutlak yerleşim (`right 4% / bottom 5%`).
