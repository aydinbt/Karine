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
- `Resources/Bube/Cases/case002.json` — "Son Sefer", 13 düğüm, 31 soru. Taslak değil: `draft` bayrağı 26 Eylül 2026'da kaldırıldı, vaka zincirde case001'in ardından geliyor. Dört sütunlu rapor kullanan ilk vaka.
- `Resources/Bube/Locales/tr.json` — ortak metin. **Tek dil.** Vaka metni artık burada değil: `tr.case001.json` (9 anahtar) ve `tr.case002.json` (170 anahtar) dosyalarında durur ve `LocaleLoader` yüklemede birleştirir. Çakışan anahtarda ortak dosya kazanır ve doğrulama bunu iki dosya adıyla bildirir. Yinelenen anahtar yok, eksik anahtar yok; ~10 ölü anahtar var (kaldırılmış CCTV yan menüsünden kalma).
- `Resources/Bube/Audio/` — dokuz klip. `AudioDirector` klip adlarını buradan arar (`ui_press`, `ui_typewriter`, `ui_stamp`, `ui_notification`, `ui_chat`/`ui_chat_low`, `menu_theme`, `desk_theme`, `room_interview` + vakanın `ambienceId`si). Eksik klip oyunu durdurmaz, sessiz geçer ve bir kez not düşer.
- `StreamingAssets/Bube/` — `career_start.mp4` (3.2 MB, Bora filmi), `report_send.mp4`. Ana menü durağan görsel kullanır; `main_menu_loop.mp4` silindi (8 Ekim 2026).

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
| `Case001Rules.cs` / `Case002Rules.cs` / `Case005Rules.cs` / `Case006Rules.cs` / `Case007Rules.cs` / `Case008Rules.cs` / `Case009Rules.cs` | Yalnız o vakanın tasarımını sabitleyen iddialar (#005: 00.08 ve emanet kaydı soru sorulmadan açılmaz; "saldırı yok" otopark kaydına dayanır; Murat + yanlış nitelik doğru sayılmaz; #006: garson ifadesi, zarf ve arama dökümü soruyla açılır; mesajı Defne'ye ya da takibi Cem'e yükleyen rapor desteklenmez; #007: araç izni plaka + konum dayanağıyla reddedilir, "düştü" ve "delilleri örtmek" desteklenmez; #008: Olay C bağlantısız açılmaz, bağlantı/arşiv/arama zayıf dayanakla reddedilir, "eve Burak girdi" ve "Tolga aşağı itti" desteklenmez; #009: üçüncü hat yuvalar doluyken açılmaz, kapalı hattın kaydı istenmez, kapanan hat gecikmeyle döner, "Arda zimmet" ve "meşru el koyma" desteklenmez). |
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

Masa arka planı `Bube/Art/OfficeDesk`'tir (`KarineUI.Office`). Eski `Bube/DeskReference.png` 4 Ekim 2026'da silindi; içerik doğrulayıcı (`ProjectRules`) masa görseli olarak `OfficeDesk`'i arar.

## Yazı ölçeği

Yazı tipi tektir (IBM Plex Mono, kök öğeden miras). Punto da tektir: `Typography.Steps` dokuz basamaktır ve `Typography.Snap` ölçek dışı her değeri en yakın basamağa oturtur. `Text(...)` ve `Button(...)` boyutu **her zaman** `Snap`'ten geçirir, yani ölçek dışı bir punto ekrana ulaşamaz — yeni bir çağrı yeri eklerken ölçeği hatırlamak gerekmez. `TypographyTests` çıplak `style.fontSize=<sayı>` kalmadığını kaynak taramasıyla sabitler.

## Sinematikler

8 Ekim 2026'dan beri açılış filmi yalnız `career_start.mp4`dir (Bora'nın göreve hazırlanışı, `config.json` → `worldIntros` → `career_start`). Eski `world01_intro.mp4` ve `case001_arrival.mp4` silindi; dosya çizilmiş bırakılışla gelir. Videolar `Assets/StreamingAssets/Bube/` altında, Git LFS ile saklanır ve `VideoPlayer` ile `RenderTexture`'a çizilip tam ekran `ScaleAndCrop` gösterilir.

Üretici filigranı **"Geç" düğmesiyle örtülür**. Filigran yeri veriden gelir — `CornerMark { x, y, w, h }`, filmin kendi karesine oranlı (0..1). `PositionIntroSkip` filmin ekrandaki gerçek dikdörtgenini 16:9'dan hesaplar, yani telefonun eni ne olursa olsun düğme doğru yere oturur. Sabit 1280×720 varsayımı kaldırıldı; `world01` değerleri piksel eşdeğer kaldı.

Masaya bırakılış videosu kendi içinde siyaha kapanır; masa da siyahtan 1,25 saniyede açılır (`OpenEyes`) ve açılma boyunca kaplayan gölge dokunmaları tutar. Masaya bırakılış videosu oynatılamazsa elle çizilmiş `FirstDeskArrival` animasyonu devreye girer. "Geç" ile videonun bitişi aynı yere gelir; `deskArrivalDone` bayrağı ikinci çağrıyı yutar.

## Dosyanın masaya bırakılışı

Yeni dosya masaya **bırakılır**, tepside hazır bulunmaz. İki yol var ve ikisi de `MaybeWorldIntro` üzerinden geçer:

- Bir dünyanın **ilk** dosyasında, o dünyanın açılış filmi ve ardından `deskArrivalVideo` bir kez oynar (`config.json` → `worldIntros`). Video yoksa ya da oynatılamazsa elle çizilmiş bırakılış devreye girer.
- Aynı dünyadaki **sonraki her dosya** doğrudan çizilmiş bırakılışla gelir (`NewCaseArrival` → `FirstDeskArrival`): masa siyahtan açılır, klasör yukarıdan kayarak masaya iner ve üstünde vakanın adı yazar. Şerit kariyerin ilk dosyasında `intro.firstFile`, sonrakilerde `intro.newFile`.

Bırakılış yalnız **kabul edilmemiş** vakanın anıdır: kabul edilmiş bir vakaya dönerken animasyon oynamaz, görevden ayrılmış kariyerde hiç oynamaz. İki PlayMode testi bunu koruyor (`CaseArrivalTests`).

## Sahte `CS0246: NUnit could not be found` — bozuk paket önbelleği

Testler birden bire "NUnit bulunamadı" diye onlarca derleme hatası verirse kodda hata yoktur: geçici çalışma kopyasındaki `Library/PackageCache` yarım kalmıştır. Günlükte imzası şudur — `Asset Packages/com.unity.test-framework/... has no meta file, but it's in an immutable folder. The asset will be ignored.` Paket klasörü yerindedir ama `.meta` dosyaları yoktur, bu yüzden test derlemesi NUnit'i göremez.

Çözüm çalışma klasörünü silmektir; Unity paketleri baştan açar:

```bash
rm -rf "$TMPDIR/karine-tests" && Tools/run-tests.sh
```

Aynı günlükte görünen `[Licensing::Module] Error: Access token is unavailable` satırı **yanıltıcıdır**: hemen ardından `Successfully updated license` gelir ve lisans çalışır. Oturumla ilgili gerçek bir sorun varsa sonuç dosyası yine yazılmaz ama günlükte lisans hiç kurulmaz. Günlük: `$TMPDIR/karine-tests/<platform>.log`.

## Testler ve gerçek kayıt klasörü

`Application.persistentDataPath` yolu şirket ve ürün adından türer, yani testlerin koştuğu geçici proje kopyası da geliştiricinin **kendi** kayıtlarını yazar. Bir PlayMode koşusu böylece oynanışı bozabiliyordu: vaka kabul edilmiş, sıradaki dosya açılmış olarak kaydediliyor ve oyun o anı bir daha oynamıyordu. `Assets/Bube/Tests/PlayMode/SaveSandbox.cs` (`[SetUpFixture]`) tüm PlayMode testlerinden önce `bube-*` kayıtlarını belleğe alıp klasörü boşaltır, testler bitince geri koyar.

## Rapora kaynak olamayan kayıtlar

Vakayı açan tutanak `notReportSource: true` taşır: listede görünmez ve `ReportSourceAvailable` onu reddeder, yani eski bir kayıttan da gönderilemez. Gerekçe ifadelerden, kameradan ve soruşturma sırasında açılan belgelerden kurulur.

## Rapor sihirbazında kaynak seçimi

Kaynak listesi adımın içinde açılan bir panelken telefonda yönetilemiyordu: sayfanın kendi kaydırmasının içinde ikinci bir kaydırma, üstünde arama alanı ve dört filtre vardı. Artık adımda yalnız seçimi gösteren tek bir satır durur; dokununca kaynak listesi tam ekran açılır (tek kaydırma, dört filtre, arama yok). Seçim yapılınca sayfa aynı adımla yeniden çizilir.

## Raporun gönderilişi

Bırakılışın tersi: oyuncu son raporu gönderdiğinde `Result()` önce `PlayReportSend` çağırır — Bora formu doldurup kaşeler, evrak ekran dışına çıkar — ve vaka özeti ancak film bittikten (ya da GEÇ'e basıldıktan) sonra açılır. Video `config.json` → `reportSendVideo`, GEÇ düğmesinin yeri `reportSendMark` ile verilir; ikisi de masaya varış filmiyle aynı köşeyi kullanır. Alan boşsa ya da video oynatılamazsa özet doğrudan açılır; animasyonun yokluğu akışı kilitlemez.

## Vaka teklifi akışı

Ayrı bir tam ekran teklif ekranı **yoktur** (`CaseOffer()` kaldırıldı). Kabul edilmemiş vaka, `InboxPage()` içinde `InboxEntry.offer` alanı dolu olan en üstteki okunmamış evrak olarak listelenir; sağ sütun `offer.subtitle` + `offer.summary` önizlemesini ve `offer.accept` düğmesini çizer, düğme `AcceptCase()` → `Save()` → `Desk()` yapar. `Desk()` kabul edilmeden yalnız tepsi ve ana ekran kısayolunu açar. Rozet açık teklifi de sayar ve `badge.schedule.Execute(...).Every(520)` ile yanıp söner — zamanlayıcı rozetin paneline bağlı olduğu için ekran değişince kendiliğinden durur. `FirstDeskArrival()` kendi masa görselini çizmez, `Desk()`'i arka plan alır.

(Eski `DeskReference.png` ve içine gömülü kurum yazısı 4 Ekim 2026'da depodan silindi.)

## Rapor sütunları

Sonuç raporu üç sütunla açılır: fail (`verdicts`), yöntem (`methods`), kanıt (`evidence`). Bir vaka `custody` dizisini doldurursa **dördüncü sütun** araya girer (yöntemden sonra, kanıttan önce) ve başlığını `custodyLabelKey` belirler; boş bırakılırsa `conclude.custody` kullanılır. Aynı biçimde `suspectLabelKey` ve `methodLabelKey` fail ve yöntem sütunlarının başlığını vakaya göre değiştirir (Dosya #003: "Ölümden kim sorumlu?", "Ölüm nedeni"); boşsa `conclude.suspect` / `conclude.method`. Başlık rapor sihirbazında, özette, faksta ve arşivde aynı yerden gelir. Sütun, tek olayda ikinci bir sorumluluğu — örneğin yaralayan ile parayı alanın farklı kişiler olmasını — raporda ayrı bir soru olarak sorar.

Sihirbaz adım sayısını `BubeApp.ReportColumns()` üretir, bu yüzden hiçbir yerde sabit "04" yoktur; sayaç `01 / 04` ya da `01 / 05` olarak kendiliğinden yazılır. Değerlendirme tarafında yanlış kişi yazmak failde olduğu gibi `falseAccusation`, doğru kişiyi kaynaksız yazmak `incomplete` sayılır. `custody` tanımlamayan vaka hiç etkilenmez: eski üç argümanlı `SubmitFinalReport` çağrısı duruyor ve eski kayıtlar olduğu gibi okunuyor. Doğrulayıcı sütun varsa tam olarak bir doğru seçenek, etiket anahtarları ve tanımlı dayanak kaynakları arar.

## Yeni vaka ekleme

Çekirdek kod değişmez. Gereken dosyalar:

1. `Resources/Bube/Cases/caseXXX.json` — düğümler, sorular, zaman çizelgesi, kararlar, özet.
2. `Resources/Bube/Locales/tr.caseXXX.json` — o vakanın bütün metni. Ortak `tr.json`a dokunulmaz; aynı anahtarı yeniden tanımlamak doğrulamada hata verir.
3. Önceki vakanın `nextCaseId` alanı.
4. İsteğe bağlı: kişi portreleri (`Resources/Bube/Characters/<personId>.png`), vaka görselleri, `ambienceId` ile oda sesi.

Portre PNG'si yoksa piksel portre çizilir ve tonları vaka verisinden gelir (`Node.portrait`: `hairHex`, `skinHex`, `shirtHex`, `longHair`, `moustache`); alan boşsa varsayılan kullanılır. Doğrulayıcı vakaya özel C# kuralı **istemez** — `ContentValidator`ın kancaları isteğe bağlıdır, yeni vaka genel yoldan geçer.

Bu hedef case002 ile **veri düzeyinde** doğrulandı: doğrulayıcı vakayı baştan sona otomatik oynuyor, on üç düğümün hepsini açıyor ve desteklenen dört sütunlu raporu gönderiyor. Oyuncu akışı (geçiş, kayıt, faks zamanlaması) henüz Play Mode'da kanıtlanmadı.

## Ses

`SoundSettings` sesin kararlarını (beş kademe: kapalı, %25, %50, %75, tam — sayı doğrudan yüzdedir, kazanç ondan türer; `PlayerPrefs` anahtarları) tutar; `AudioDirector` çalar. Ayrım kasıtlı: düzey mantığı Editor testinde `AudioSource` olmadan sınanıyor. Üç kanal karışmaz — müzik ve oda ortamı döngülü, efekt üst üste binebilir. Oda sesi `EnsureScene`ten gelir; vaka kendi ortamını söyleyebilir (`CaseData.ambienceId`).

**Menü üstü kart telefon biçimine göre ölçülür.** `MenuOverlay` (ayarlar, hakkında, yeniden deneme, yönlendirme) kartı kenarlardan sabit %27 içeride kuruyordu: geniş ekranda makul, telefon dikey tutulduğunda daracık bir şerit. Artık pay ekranın biçiminden geliyor (dikeyde %5, yatayda %24) ve kartın içi `ScrollView` — başlık sabit kalır, içerik akar, uzun ayar listesi kesilmez. Ayarlarda ses kademeleri **radyo listesiyle** seçiliyor: beş satır, yukarıdan aşağı azalan (tam → kapalı). Arada bir sekme şeridi denendi ve bırakıldı — beş hücre tek satıra sığmıyor ve ayarların geri kalanı radyo, aynı soru ekranda iki biçimde sorulmamalı. Kademe sayısı üçten beşe çıktı (kullanıcı kararı); kayıtta duran eski üç kademeli değer (0/1/2) okunurken en yakın kademeye çevrilir, yani ayar göç yüzünden sıfırlanmaz.

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
- 27 Eylül düzeni: video ve hata durumundaki durağan görsel aynen kalır. Üstüne `KarineLogo.Hero`, Türkçe slogan, beş `KarineUI.MenuAction` satırı ve `KarineUI.MenuIdentity` personel kartı ayrı UI öğeleri olarak çizilir; tek bir menü ekranı PNG'si yoktur. Beş eylem: kayda göre Devam Et/Oyuna Başla, Vakalar, Kariyer, Ayarlar, Hakkında. Yeniden kariyer başlatma ayarlardadır; mobil geri tuşunun çıkış onayı korunur.
- Menünün ölçüleri `KarineTheme.MainMenu` altında, ikonları mevcut `Resources/Bube/Art/Icons/` kümesinde ve Bora portresi mevcut `Resources/Bube/Characters/bora.png` varlığındadır. Düğmelerin tamamı en az 48 birimdir. Sağ alt stüdyo bloğu referans görselde olmadığı için menü katmanından kaldırıldı; videonun kendisine dokunulmadı.

## Bölüm seçici verisi

Ekran `Assets/Bube/Runtime/BubeApp.World.cs`, veri modeli ve kilit kuralı `Assets/Bube/Runtime/Worlds.cs`, veri `Assets/Bube/Resources/Bube/Worlds.json`, doğrulama `Assets/Bube/Editor/Validation/WorldRules.cs`.

Veri: `countries[]` → `id`, `nameKey`, `cityKey`, `descriptionKey`, `image`, `slots[]`; her yuva `caseId` + `titleKey` + `image`. Boş `caseId` "bu dosya henüz yazılmadı" demektir (`WorldSlotState.Unwritten`) — kilitli değil, **yok**.

Durum tek yerde türetilir (`Worlds.SlotState`) ve ekran onu yalnız boyar. Kapanmış dosyaların kümesi `Career.reviewHistory`den gelir; ülke ilerlemesi, üstteki `n / 70` sayacı ve ülke çubuğu aynı kümeden hesaplanır, ayrı bir kayıt alanı **eklenmedi**. Ekran açılırken `Worlds.Resume` bitmemiş ilk açık ülkeyi seçer.

`WorldRules` şunları kilitler: ülke kimliği tekil, her ülkenin yuva sayısı eşit (çubuk ve sayaç aynı ölçeği göstersin), her metin anahtarı dil dosyasında var, dolu bir `caseId` gerçekten `Bube/Cases/` altında var ve iki yuvada geçmiyor, ilk yuva `config.initialCase` ile aynı, ve **yazılmış her vaka seçicide bir yuvada** — listede olmayan vaka oyuncunun asla göremeyeceği vakadır.

Eksik görsel hata değil: harita (`Bube/WorldMap`) ve ülke görseli (`Bube/Worlds/<id>`) yoksa ekran mukavva pano, kâğıt iğne ve ikonla kurulur. Bu yüzden `RequiredTextures`a eklenmediler.

## 27 Eylül 2026 — Vakalar sunumu
`BubeApp.World.cs` artık sidebar + yatay CountryStrip + CaseStrip kurar. `KarineUI.CaseBrowser.cs` içindeki `CountryTile`, `CountryPostcard`, `CasePhotoCard` ve vektör `CaseSeal` tekrar kullanılabilir. Ölçüler `KarineTheme.CaseBrowser` içindedir. `CountryPostcards.png` 5×2 atlası UV ile seçilir (tr, uk, de, jp, fr / us, it, es, ca, au). Tek görsel içine UI gömülmez. #002 kapağı mevcut `case002_2258.mp4` videosunun 1. saniyesinden alınmıştır. Ülke seçimi kaydırma konumunu korur; vaka şeridi dokunma kaydırması ve iki 48 birimlik yön düğmesiyle gezinir. `Worlds` ilerleme kuralları ve dosya açma/kurumsal kayıt işleyişi korunur. Başsız test görsel benzerlik kanıtı değildir.

## 27 Eylül 2026 — Masa sunum katmanı
`Desk()` mevcut kabul/açık/kapalı/emekli dallarını koruyarak `KarineUI.OfficeStage` kurar. OfficeRoom boş oda dekorudur; OfficeProps saydam atlasındaki monitör, dosya, telefon, tepsi, lamba ve delil yığını UV ile ayrı Image öğelerine bağlanır. Pencere CountryPostcards atlasından aktif dosyanın ülkesini kullanır; `worldPick` kullanılmaz. Ortak sahne oranı korunur, nesne ve düğmeler aynı referans koordinatları paylaşır. `OfficeAction` en az 48 birim dokunma alanı ve mevcut ses kapısını kullanır. Evrak sayacı mevcut RefreshInboxBadge işlevinden gelir. Delil girişi mevcut dosyanın evidence sekmesini açar; yeni oyun sistemi yoktur. Eski DeskReference varlığı silinmedi, yeni Desk() içinde kullanılmıyor.

## 1 Ekim 2026 — masa sunumu revizyonu

`KarineUI.OfficeStage` boş `Art/OfficeRoomV2` dekoruna ülke atlası, kodla çizilen panjur ve ayrı `OfficeProps` nesnelerini ekler. Eski oda varlığı korunur. Geometri `KarineTheme.Office` içindedir. `BubeApp.Desk` yalnız sunum katmanında güncellendi: mevcut eylemler yeni yerleşime bağlandı; `OfficeBrand` ana menüye döner, seçili Dosya üst menüsü kenarlıkla ayrılır. Ülke halen aktif dosyanın Worlds üyeliğinden alınır. Kayıt, vaka koşulları, görüşme veya CCTV oynatma mantığı değişmedi. 124 test geçti; grafik etkin ayrı PlayMode çalışmasında `Desk_UsesSeparatePropsAndCountryViewWithoutGuidance` çıktısı incelendi.

## 1 Ekim 2026 — Dosya Vaka Detay

KarineUI.Dossier.cs ortak DossierSheet/Header/Tab/Photo/Person/Meta/Title bileşenlerini içerir; ölçüler KarineTheme.Dossier içindedir. BubeApp.Investigation.FilePage sunumu bu bileşenleri kullanır; mevcut okuma, keşif, sonuç ve sekme geçiş koşulları korunur. DossierOverview yalnız aktif raporun fileMeta, imageResource, bodyKey alanlarını ve keşfedilmiş kişileri gösterir. Art/DossierPaper.png yeni tek reusable kâğıt dokusudur. KARINE_DOSSIER_CAPTURE test ortam değişkeni izole grafik etkin Unity koşusunda önizleme üretir.

## 1 Ekim 2026 — Gelen Evraklar sunumu

KarineUI.Inbox.cs InboxHeader/InboxList/InboxPaper/InboxItem ortak bileşenlerini sağlar; yerleşim ve ölçüler KarineTheme.Inbox içindedir. InboxPage veri üretimi ve eylemleri korunup sunumu değiştirildi. DossierPaper yeniden kullanılır; yeni bitmap yoktur. KARINE_INBOX_CAPTURE ortam değişkeni grafik etkin izole PlayMode testinde kabul bekleyen dosyanın evrak önizlemesini alır.

Son doğrulama notu: Unity önizlemesi incelendi; ardından yalnız liste hizası ve yinelenen kurum başlığı düzeltildi. Bu iki sunum düzeltmesi sonrası test betiği lisans oturumu bulunamadığından yeniden çalışmadı. Önceki sürümde 111 EditMode + 13 PlayMode başarılıydı.

## 1 Ekim 2026 — Görüşmeler ve İncelemeler tableti

KarineUI.Requests.cs, RequestLayout/RequestItem/RequestDetail bileşenlerini sağlar. KarineTheme.Requests ölçüleri merkezileştirir. BubeApp.Investigation içindeki iki talep ekranı mevcut keşif, pending, available ve request çağrılarını koruyarak sunumu ayırır. Seçili kişi/belge yalnız geçici UI alanlarıdır; kayıt şeması değişmez. RequestScreenHeader sadece talep ekranlarında ortak BpsTablet başlığının yerini alır; CCTV sunumu değişmez. KARINE_REQUEST_CAPTURE izole testte görüntü alınmasını sağlar.

## 1 Ekim 2026 — CCTV tablet sunumu

KarineUI.CctvArchive.cs sol kaynak listesi/sağ kayıt paneli/satır bileşenlerini sağlar; ölçüler KarineTheme.CctvArchive içindedir. CctvScreen yalnız sunumunu bu bileşenlere taşır. Zamanlanmış tarama, Read/Save, netleştirme ve video oynatma kodu korunur. Video overlay yeni iki sütunlu alanın tamamını kapsar. KARINE_CCTV_CAPTURE izole grafik testinde kayıt ekranını görüntüler.

## 1 Ekim 2026 — Vakalar yeni referansı

KarineUI.CaseBrowser içinde BrowserMeter/BrowserProgress/BrowserBanner/BrowserSteps eklendi. CountryTile opsiyonel ilerleme parametresi alır; kilit görseli mevcut CaseSeal ile çizilir. WorldPage sadece sunumda Worlds.CompletedIn ve SlotState sonuçlarını kullanır; Worlds/kayıt/açılma mantığı değişmedi. KarineTheme.CaseBrowser kompakt kart ölçülerini merkezileştirir. Mevcut OfficeRoomV2 dekoru, CountryPostcards atlası ve vaka kapakları yeniden kullanılır. KARINE_WORLD_CAPTURE grafik etkin testte önizleme sağlar.


## 1 Ekim 2026 — Çubuksuz kaydırma

`KarineScrollView : ScrollView` ortak sunum bileşeni iki eksenin scroller görünürlüğünü Hidden başlatır. Menü, Vakalar, sorgu, kaynak/inceleme ekranları ve ortak Scroll yardımcısı bu bileşeni kullanır. Unity yerel dokunma ve scrollOffset davranışı korunur.


## 1 Ekim 2026 — Ayarlar referansı

SettingsPage mevcut metin/ses değerlerini taslağa alır. RenderSettings sekme ve seçimlerde taslağı korur; Kaydet mevcut PlayerPrefs/SoundSettings yollarıyla uygular. Reklam izni mevcut onay akışında bağımsız kaydedilir. SettingsShell/SettingsChoice ve KarineTheme.Settings yerleşimi yönetir. Mevcut video ve ikonlar tekrar kullanıldı; bitmap eklenmedi.


## 1 Ekim 2026 — Chakra Petch

Bütün oyun metinleri Chakra Petch Regular/SemiBold/Bold ailesine geçirildi. FontSet ve içerik doğrulayıcı güncellendi; logo bitmap olarak korundu. OFL lisansı fontlarla birlikte eklendi. Fiziksel telefon okunabilirliği kontrolü açık.


## 1 Ekim 2026 — Ortak geçişler

KarineMotion unscaled zamanla çalışır ve DetachFromPanelEvent sırasında zamanlayıcıyı iptal eder. Tablet kapanışı tek seferlik korunur, animasyon sırasında içerik devre dışıdır. Dosya sekmesi yenilenirken giriş animasyonu tekrarlanmaz. Ortak kök pointer geri bildirimi mevcut düğmelerin eylemine müdahale etmez.


## 1 Ekim 2026 — Evrak varışı

PresentInboxArrivals yalnız görünür masada çalışır; overlay altındaki masa teslim efektini tüketmez. Oturum içi HashSet evrak kimliği, vaka kimliği ve faks hazır zamanı üzerinden tekrarları önler. Aynı anda gelenler tek efektte birleşir. Kayıt şeması değişmedi; yeni uygulama oturumunda okunmamış evrak yeniden bildirilebilir. ui_fax.wav yerel sentezlenmiş 0,85 saniyelik kâğıt sürme ve kısa ton sesidir.


## 1 Ekim 2026 — Dosya sekmesi geçişi

DossierPaper.userData görüntülenen bölüm kimliğini tutar. FilePage ilk açılışta Paper, bölüm değişiminde Page geçişini kullanır. Aktif DossierTab statik/animasyonlu öne çıkışı ortak KarineMotion üzerinden alır. Ekran yeniden kurulurken detach önceki zamanlayıcıyı iptal eder; veri akışı değişmez.

## Masanın havası (`KarineUI.OfficeAtmosphere`)

`Desk()` masayı kurduktan sonra `KarineUI.OfficeAtmosphere(stage)` çağrılır (`Assets/Bube/Runtime/UI/KarineUI.Atmosphere.cs`). Bu çağrı:

- `OfficeRoom` ve `OfficeWindow-*` öğelerini `OfficeBack` kabına, `DeskFront` kümesindeki eşya görsellerini ve masa düğmelerini `OfficeFront` kabına taşır (sıra korunur). Parallax kapların `translate`'iyle yapılır; 5 Ekim 2026'dan beri `OfficeFront` da `OfficeBack` ile aynı ölçek (`BackScale`) ve aynı kaymayı alır, çünkü masa yüzeyi arka plakada olduğundan ayrı kayma eşyaları masadan koparıyordu; derinlik hissi yalnız ışık ve tozda kalır. böylece çocukların kendi `translate`'i (kalkma, kâğıt varışı) serbest kalır. Düğme ile eşyası aynı kapta olduğu için dokunma alanı görselden kaymaz.
- Işık havuzu, eşya gölgeleri ve kararma çalışma anında üretilen iki 128×128 dokudan çizilir (`Glow`, `Vignette`; statik, bir kez). Kararma `OfficeFront` içinde ilk düğmenin hemen önüne konur.
- Eğme: `SystemInfo.supportsAccelerometer` ise `Input.acceleration` (proje eski Input Manager'da, `activeInputHandler: 0`), değilse fare. Taban `TiltRecenter` hızıyla yeni duruşa yaklaşır.
- Bütün sayılar `KarineTheme.Office.Atmosphere` içinde. `KarineMotion.Reduced` iken toz, parallax ve kalkma kurulmaz.
- Masa düğmesi → eşya eşlemesi `PropOf`; yeni bir masa eşyası eklenirse buraya ve `DeskFront`'a yazılır.

## İkinci kademe efektleri

- `BubeApp.Effects.cs`: `SlideToPerson` (öne sürülen kâğıt, ekranı `PresentBlocker` ile kayma boyunca kilitler), `DragToPresent` (sola sürme eşiği `Effects.SwipeDistance` ya da düğme genişliğinin %40'ı), `PrintOut` (gelen evrak basımı; basılmamış metin `<alpha=#00>` zengin metin etiketiyle yerinde tutulur, basılmamış öğe `visibility: hidden`; anahtar başına bir kez — `printedPapers`, oturum içi).
- `KarineUI.Effects.cs`: `SignalSwitch(frame)` ve `PageTurn(paper)`; ikisi de `pickingMode: Ignore`, bitince kendini kaldırır.
- Sayılar `KarineTheme.Effects`. `KarineMotion.Reduced` hepsini, `instantText` basımı atlar.
- Test notu: grafiksiz PlayMode'da açılış akışı video hazırlanamayınca **gecikmeli** `Desk()` çağırır ve kökü yeniden kurar. Kök üstüne öğe koyan testler kurulumda ~2 sn bekler (`EffectsTests.Setup`).


## CCTV kare dizisi (2 Ekim 2026)

`CctvEvent.framePaths/frameTimes/frameMs`, `HasFootage`. Oynatıcı `BubeApp.CctvFrames.cs`: `StartCctvFrames`, `ShowCctvFrame`, `PlayCctvFrames` (UI Toolkit zamanlayıcısı), `StepCctvFrame`; kamera katmanı `OpenCctvVideo` ile ortak, `StopCctvVideo` kare durumunu da temizler. Doğrulayıcı her karenin `Resources.Load<Texture2D>` ile bulunduğunu ve damga sayısının kare sayısını tuttuğunu denetler. Ayar: `KarineTheme.Effects.CctvFrameMs/CctvFlickerSeconds/CctvJitter`.

## Defter, altı çizili satırlar, akıbet ve kapanan görüşme (2 Ekim 2026)

- **Kayıt:** `Progress.notebook` (`NotebookEntry { leftId, rightId, mark }`, `mark` ∈ `Investigation.NotebookMarks` = conflict/agree/question) ve `Progress.highlights` (`"<nodeId>:<cümle sırası>"`). Yeni alanlar boş liste varsayılanıyla gelir, kayıt sürümü değişmedi; yükleme bilinmeyen kaynaklı notu ve satırı atar.
- **Motor:** `MarkNote` yalnız açılmış iki farklı kaynağı kabul eder (okunan belge/kayıt ya da konuşulan kişi); çift sırasızdır, aynı hükme yeniden dokunmak notu siler. `ToggleHighlight` yalnız okunmuş ve görüşme olmayan kaynakta çalışır. Kapanan dosyada ikisi de kilitlenir. Cümle bölme `Investigation.Sentences`: satır sonu ve ardından boşluk gelen `. ! ? …`; "23.35" gibi saatler bölünmez.
- **Arayüz:** `BubeApp.Notebook.cs` — karşılaştırma ekranının altındaki defter şeridi, belge metninin dokunulur cümleleri (`MarkableBody`) ve dosyadaki "Defter" sekmesi.
- **Akıbet:** `Verdict.epilogueKey` ve `Choice.epilogueKey` (ikinci sorumluluk için). Faks (`DrawEpilogue`) ve sicil kaydı gösterir. Doğrulayıcı: bir sütunda biri yazıldıysa hepsi yazılmalı. Faks artık dördüncü sütunu da değerlendirir (`fax.reason.custody.*`).
- **Kapanan görüşme:** `Node.closesAfterRead` + `Node.closedNoteKey`. `Investigation.Closed(n)`: listedeki kaynakların hepsi okundu ve görüşme ne istendi ne yapıldı. `CanRequest` kapalı görüşmeyi reddeder; tabletteki durum "Artık görüşülemiyor". Doğrulayıcı `CaseRules.EssentialNodes` ile doğru seçeneklerin dayanaklarını, kapanış önkoşullarını ve bunların önkoşul/soru zincirini çıkarır; kapanan görüşme bu kümede olamaz.
- **Ortam sesi:** `room_rain` ve `room_night`, `Tools/make-audio.py` üretir (`python3 Tools/make-audio.py room_rain room_night` yalnız ikisini yeniden yazar), `Tools/check-audio.py` döngü olarak ölçer. Vaka `ambienceId` ile seçer; masa sahnesinde müziğin altında çalar.
- **Testler:** `NotebookAndPressureTests` (6 test). Toplam 117 EditMode + 23 PlayMode.

## Kare hızı

`FrameRate` (Runtime/FrameRate.cs) `karine.fps` PlayerPrefs anahtarını okur (30/60/120, varsayılan 60), `QualitySettings.vSyncCount=0` ve `Application.targetFrameRate` ayarlar. Açılışta `BubeApp` içinde `SoundSettings.Load()` sonrası yüklenir; Ayarlar'da "Kaydet" ile uygulanır.

## Efekt katmanı (2 Ekim 2026)
- `Fx` (statik): seviye Kapalı/Hafif/Tam (`karine.fx`), titreşim (`karine.haptics`), `Fx.On`/`Amount`/`Count`, `MayFlash()` (0.34 s aralık), `Watch(dt)` ile uyarlamalı `Degraded`, `Buzz(Haptic)` (Android Vibrator JNI, iOS `Handheld.Vibrate`).
- `KarineUI.Film` (gren, CRT, VHS, kar, yakınlaştırma), `KarineUI.Weather` (saat tonu, yağmur, far, buhar, lamba), `KarineUI.Stagecraft` (yıpranma, faks, damga, ışık halkası, neon, duruş, göz kırpma, üç nokta, kaldırma). Sabitler `KarineTheme.Film`, `KarineTheme.Stagecraft`, `KarineTheme.Office.Weather`.
- `BubeApp.Fx.cs`: `InstallFx`, film katmanını her karede kökün en üstünde tutan `KeepFilmOnTop`, masa eşyası sesleri, `DoorClosed`.
- `BubeApp.Settings.cs`: ayarlar sayfası `Menu.cs`ten taşındı (560 satır sınırı).
- `AudioDirector.PlayAt(id,pan,gain,ambient)` 4 kaynaklı konumlu havuz; `Scatter(bool)` ofiste uzak sesler. Görüşme odası müziği `interview_theme`.
- `CaseData.deskHour` (-1 = yok) ve `CaseData.weather` ("rain" ya da boş); doğrulayıcı ikisini denetler.
- Ses: 17 yeni klip `Tools/make-audio.py`de; `check-audio.py` 29 dosya, 0 bulgu.

## Sahne katmanı (H–O)

- Kütüphane: `UI/KarineUI.Transitions.cs` (geçişler + `Cue` ses/titreşim tablosu), `Room.cs`, `Evidence.cs`, `Terminal.cs`, `Night.cs`, `Closing.cs`. Sabitler `KarineTheme.Scene`.
- Bağlantı: `BubeApp.Scene.cs` — `SceneTick` (`KeepFilmOnTop` içinden), eşya geçişi (`pendingProp`), `Dial`, `ShowCaseClosed`, `TrustBadge`, `Shape`/`Verdict`, `StepCctvBack`, ayarlar ve geliştirici kare sayacı.
- `CrtPass` (MonoBehaviour): açıkken panel `RenderTexture`'a çizilir, `Resources/Bube/Shaders/KarineCrt.shader` ile `OnGUI`de basılır; dokunuş aynı bombe formülüyle bükülür. PlayerPrefs `karine.crt`.
- `ScreenReader.Tick(root)`: Unity 6.3 `UnityEngine.Accessibility`; görünen düğme/yazılardan ağaç kurar. `com.unity.modules.accessibility` manifest'e eklendi.
- `Typography.Scale` (1 / 1.15 / 1.3, `karine.textScale`) yalnız `Snap` üzerinden geçen boyları büyütür.
- PlayerPrefs: `karine.shapes`, `karine.ashSmoke`, `karine.devMeter`, `karine.seenRank`, `karine.caseClosed.<id>`, `karine.inkDry.<id>`.

## Sahne katmanı (P–X)

- Kütüphane: `UI/KarineUI.Stage.cs` (açılış, mekân kareleri, çıkış, ışık akışı, kamera nefesi, bekleme, süre sayacı), `Touch.cs`, `Glitch.cs`, `Lobby.cs`, `Access.cs`, `Placer.cs`.
- Bağlantı: `BubeApp.Stage.cs` (`StageTick`, `StageDesk`, raf, kayıt satırı, kariyer duvarı, erişilebilirlik ayarları, `DevLab`), `BubeApp.Tape.cs` (CCTV hızı, kare iğneleme, aşınma, son aramalar).
- Ses: `AudioDirector` artık `partial`; `AudioDirector.Space.cs` — `Room(kind)` yankı ve uzak ses listesi, `Layer`/`Bed` kanalları, `Distance` alçak geçiren, `Heard` olayı (altyazı), `Sting()`.
- `Fx.Strength` (titreşim gücü), `Fx.FrameMs`, `Fx.Budgeted` (oturumluk "Hafif" iniş).
- `CaseData.locationFrames` (isteğe bağlı Resources yolları).
- Paketler: `com.unity.modules.screencapture`, `com.unity.modules.imageconversion` (ekran görüntüsü testleri). `Tools/run-tests.sh` `KARINE_BASELINES` dışa aktarır.
- Unity bileşeninde `GetComponent<T>() ?? Add...` kullanılmaz: Editor'de eksik bileşen sahte-null döner ve `??` onu null saymaz.
- PlayerPrefs: `karine.ink.<id>`, `karine.tapeWear.<node>.<record>`, `karine.opened.<case>`, `karine.playSeconds`, `karine.captions`, `karine.spacing`, `karine.contrast`, `karine.oneHand`, `karine.hapticStrength`.

## Cila katmanı (Y–AF)

- Kütüphane: `UI/KarineUI.Feel.cs` (dokunuş halkası, basılma, `Hold`, `MorphFrom`, `Shuffle`, `LastPressed`), `DeskLife.cs` (lamba, kablo, şehir, takvim, `Keepable`), `Presence.cs` (kırpma, floresan, ayna, duman, bırakma sesi), `Vhs.cs` (damga titremesi, `FrameRepeats`, yazıcı, `Persist`, bant takma), `Chapter.cs` (bölüm kartı, yankı, epilog, jenerik).
- Bağlantı: `BubeApp.Polish.cs` (`PolishTick`, erişim düğmesi, renk ve yazı boyu ayarları, `DeskPolish`, `DropFor`, `EchoLeaving`, `PrintOnce`, `AfterClosing`, laboratuvar ekleri, efekt kaydı, kare profili).
- Ses: `AudioDirector.Mix.cs` — `Bube/Music/` önceliği (`Clip` önce oraya bakar), `Duck`, `Headphones` (Android AudioManager), `Voice`/`StopVoice`, `StepsFor`.
- `CrtPass` artık renk körlüğü filtresi açıkken de etkin (bükme/tarama sıfır); `Correction(kind)` Machado 2009 benzetiminden daltonlaştırma matrisi kurar. Gölgelendiricide `_Grain`, `_CR/_CG/_CB`.
- `KarineMotion.SystemReduced`: Android `animator_duration_scale`=0.
- `Typography`: `Scales` dizisi kalktı; `MinScale`/`MaxScale`, 0.05 adım.
- `KarineScrollView` elastik.
- Veri alanları: `Node.smokes`, `CaseData.openingPlaceKey`, `epilogueImage`, `epilogueKey`.
- PlayerPrefs: `karine.lampOff`, `karine.calendar.<case>`, `karine.deskPos.<ad>`, `karine.colorFilter`, `karine.credits`.
- Laboratuvar çıktıları: `persistentDataPath/fxrec/<zaman>/NNN.png`, `persistentDataPath/perf_<zaman>.csv`.

## Reklam yerleri (2 Ekim 2026)

- `AdPlacement`: `CaseStart`, `MenuReturn`, `RewardedCosmetic`, `RewardedSkipWait` eklendi; `AdMoment`: `CaseAccepted`, `Waiting`.
- `AdGateway`: `InterstitialGapSeconds` (240), `MinSkipSeconds` (60), `Now` (testte değiştirilebilir saat), `Seasoned` (her karede `reviewHistory.Count>0`), `MaySkipWait`, `ResetInterstitialClock`. Araya giren reklam `Request` içinde gösterilince saat kaydedilir.
- Bağlantı: `BubeApp.Ads.cs` (`MenuReturnAd`, `LampOptions`, `SkipWait`). Teklif kabulü `Desk.cs`, menü `Menu.cs`, bekleme atlama görüşme talepleri ekranında.
- PlayerPrefs: `karine.lamp.tint`, `karine.lamp.unlocked.<n>`. Renkler `KarineTheme.Scene.LampTints`.

## AdMob (2 Ekim 2026)

- Paket: `com.google.ads.mobile` 11.5.0, OpenUPM kapsamlı deposu (`Packages/manifest.json`).
- `Assets/Bube/Ads/` ayrı derleme (`Bube.Ads` → `Bube.Runtime`). `AdMobProvider` mobilde `RuntimeInitializeOnLoad` ile `AdGateway.Provider`a takılır; Editor ve testlerde `NoAdProvider` kalır.
- Akış: UMP `ConsentInformation.Update` → gerekirse form → `CanRequestAds` → `MobileAds.Initialize` → geçiş ve ödüllü reklam önceden yüklenir, gösterimden sonra yenisi yüklenir. Geri çağrılar `MainThread` kuyruğuyla ana döngüye taşınır. Oyunun kendi izin kuralı (`AdGateway.Consent`) ayrıca geçerlidir.
- Kimlikler test: uygulama `Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset`, reklam birimleri `AdMobProvider.cs` başındaki iki sabit.

## Vaka sonu akışı (3 Ekim 2026)

- `Investigation.ChoiceKnown(id)`: rapor seçeneğinin sunulup sunulmayacağı; görüşme düğümüyle eşleşen kişi okunmuş olmalı. `BubeApp.Report.ReportColumns` şüpheli, yöntem ve gözaltı listelerini bununla süzer. Test: `KnownChoicesTests`.
- `BubeApp.NextStep(parent, refresh)` + `StepState()`: "fax" / "waiting" / "assignment" / "none". Saniyede bir durum karşılaştırılır, değişince `refresh` çağrılır; `InboxModal` açıkken yenilenmez. Rapor özeti ve masadaki kapanış paneli kullanır.
- `ClosedCard(caseId, then)`: onay faksı (`correct && !reopened`) ilk açılınca `KarineUI.CaseClosed`; işaret `karine.closedCard.<id>` (`ForgetCaseMoments` siler). Hem `FaxPage` hem tepsideki "faksı aç" yolu kullanır.
- Rapor özeti `KarineUI.SummaryPaper/SummaryStamp/SummarySources` (`KarineUI.CaseSummary.cs`, ölçüler `KarineTheme.CaseSummary`); resim yoksa `Bube/Art/Covers/<id>`.
- Masa bildirimleri `Update` içinde yalnız `deskStage` panelde iken eklenir; `HasIncomingFax/Document` düşünce kart kaldırılır.
- `ContinueToNextCase` tepsiye gitmez, `OpenAssignment` çağırır; tepside "assignment" girdisi yok.
- `CaseData.deskDate`: açılış kartında saatin önüne yazılan tarih (`ChapterTime`).

## Sorgu odası ve masa işaretleri (3 Ekim 2026)

- Sorgu ekranı bileşenleri `KarineUI.Interview.cs` (`InterviewIdentity/Bubble/Panel/Tab/Topic/Question/Present/Filter/Source/Paper/Action`), ölçüler `KarineTheme.Interview`. `BubeApp.InterviewSourcePicker` paneli kurar, `InterviewPreview` kâğıdı ve "Öne sür"ü, `InterviewSourceBody` kayıt içeriğini (öne sürülen kayıt kartı da kullanır).
- `Progress.triedSources` ("düğüm/soru/kaynak"): `Investigation.SourceTried` / `MarkSourceTried`. Test: `TriedSourceTests`.
- `Progress.seenRequests`: listede görülen inceleme kimlikleri, `person:<düğüm>` (talep edilmemiş kişi) ve `desk:<düğüm>` (açılmış CCTV). `InterviewBadgeCount` / `InvestigationBadgeCount` bunlarla sayar.
- `KarineUI.OfficePulse(action, on)`: `on` değilse eşyanın `OfficeHotspot` halkası kaldırılır. `KarineUI.OfficeCount(action, n)`: köşede yanıp sönen sayı. Koşullar `BubeApp.Desk` içinde.


## Sorgu odası yerleşimi ve kapanış devinimi (4 Ekim 2026)

`BubeApp.Interview.SeatInRoom` kişiyi ekran yüzdesine göre değil, **oda görselinin koordinatlarına** göre oturtur: koltuk ortası `SeatX=.4975`, masa kenarı `TableEdge=.734` (ahşabın başladığı çizgi; .726 koyu arka kenar bandını kişinin önüne çiziyordu), figür genişliği `.25`, batma `.06`. Arka plan ScaleAndCrop olduğundan ölçek = max(en oranı, boy oranı), kayma ortalanır; oran değişse de kişi koltukta kalır. Portrenin önüne `InterviewTableFront` çizilir: oda görselinin masa kenarından aşağısı, kamera nefesi dönüşümlerini kopyalar ve portre altındaki saydam boşluğu örter. `InterviewRoomFx` masanın önüne taşınır. Bardak ve kayıt cihazı yoktur.

`KarineMotion.Leave(veil, paper, then)` kapanış eylemini sarar: karartma söner, kâğıt `PaperOffset` kadar iner, sonra `then` çalışır; çift dokunuşu yok sayar. Modal (`ModalFrame`), onay formu ve rehber kâğıdı bunu kullanır.

## Ekran görüntüsü testleri ve galeri

Batchmode'da `WaitForEndOfFrame` hiç tetiklenmez, ekran yakalama asılı kalır. Bunun yerine testler her `UIDocument`'a kopyalanmış bir `PanelSettings` verip `targetTexture`'ını 2400×1080 bir RenderTexture yapar ve onu `ReadPixels` ile okur; teardown eski ayarları geri koyar. Bu yüzden `run-tests.sh` PlayMode'u `-nographics` olmadan koşar (EditMode hâlâ grafiksiz). Referanslar `Tools/Baselines/`.

`Tools/capture-screens.sh [GxY]` aynı yolla `GalleryTests`'i koşar ve her vakanın sorgu ekranlarını `Docs/Screenshots/<tarih>/` altına yazar (klasör git dışı). `KARINE_GALLERY` tanımlı değilse test atlanır; `run-tests.sh` atlanan testi düşmüş saymaz. `KARINE_TEST_FILTER` ile tek test sınıfı koşulabilir. `KARINE_TOUR=case005,...` turu masa, dosya, belge, CCTV, özet ve karşılaştırma sayfalarını da çeker; tur sırasında `karine.opened.<vaka>` geçici olarak 1 yapılır ki açılış kartı sayfaları örtmesin, sonra eski değer geri yazılır. Test süre sınırı 30 dk. `capture-screens.sh` ve `run-tests.sh` aynı geçici proje klasörünü kullanır; ikisi aynı anda koşturulmamalı.


## İnceleme izni (4 Ekim 2026)

- Veri: `Node.warrant` = `WarrantPath[]` (her yol bir `sources` listesi), `Node.warrantSlots` (varsayılan 2). İzinli düğüm `requestable` bir belgedir ama `CanRequestDocument` onu dışlar; yerine `CanRequestWarrant`.
- `SubmitWarrant(id, basis)`: tam `warrantSlots` kadar farklı, `WarrantBasisAvailable` kaynak ister (rapor kaynağı ya da okunmuş CCTV düğümünün yalın kimliği). Bir yolun tüm kaynakları seçimde varsa (`node#olay` için düğüm öneki de eşleşir) `DocumentRequest` açılır; yoksa aynı gecikmeyle `Progress.warrantDenials`a ret yazılır. Yeni talep eski reddi siler.
- Arayüz: `BubeApp.Investigation` → `WarrantForm`; aday listesi okunmuş belgeler, görüşmeler ve okunmuş CCTV olaylarıdır (`notReportSource` hariç). Ret `UnseenWarrantDenial` ile rozete sayılır, açılınca görülmüş işaretlenir.
- Doğrulama: `WalkRules.Warrant` her izinli düğümde hiçbir yolda geçmeyen okunmuş belgelerle reddi, ilk erişilebilir yolla onayı sınar.

- Talep türleri (#008): `Node.requestKind` "link" ya da "archive" aynı izin motorunu kullanır; `BubeApp.WT` metni `requests.<tür><Ek>` anahtarından alır, yoksa `requests.warrant<Ek>`a düşer.
- `Node.reopenYear` (+ `reopenNoteKey`): kayıt ilk okunduğunda `KarineUI.FileReopened` kartı oynar.
- `CaseData.closedStampKey/closedNoteKey`: `ClosedCard` damgayı ve notu değiştirir. `coldCaseTitleKey/coldCaseStatusKey`: onaylanmış vakanın arkasından arşiv çekmecesine salt kart.
- Soruşturma hattı (#009, `Investigation.Lines.cs`): `requestKind: "line"` izinli düğüm; `LineSlots` (`CaseData.lineSlots`, varsayılan 2) kadar hat tutulur (okunmuş ya da beklemede, kapatılmamış). `Node.line` taşıyan belge yalnız hat `LineActive` iken istenir. `CloseLine` → `Progress.closedLines`; `ReopenLine` kapalıyı çıkarır ve `Progress.lineReopens`a `requestDelaySeconds + lineReopenPenaltySeconds` gecikme yazar. Yeni hat talebi kapanan hat başına ceza kadar geç gelir. `WalkRules` alt kayıtları biten hattı kapatır.
- `CaseData.envelopeTitleKey/StampKey/BodyKey`: `BubeApp.EnvelopeCard` kapanış kartından sonra vaka başına bir kez (`karine.envelope.<id>`). `grantsAuthority`: doğru raporda `RecordTrust` ile "Yetki: GENİŞLETİLDİ".
- Bölüm seçici her ülkede 9 yuva gösterir (doğrulayıcı eşit sayı ister).
- Zaman çizelgesi `sortMinute` 0–1439 aralığında; çok günlü vakalarda yalnız sıra taşır, gerçek tarih etikettedir.

## Olay rekonstrüksiyonu ve bölüm finali (4 Ekim 2026)

- `CaseData.reconstruction` (`ReconCard[]`): doluysa rapor sihirbazına sütunlardan sonra bir adım eklenir (`BubeApp.Recon.cs`). `Progress.recon` yerleşimleri tutar. `ReconComplete` değilse `SubmitReport`/`SubmitFinalReport` false döner; `ReconSupported` (sıra = veri sırası, kaynak önek eşleşmesi) değilse rapor doğru sayılmaz. `FaxReview.hasRecon/reconSupported` faks satırını besler.
- `CaseData.chapterFinale`: faks sonrası (`FaxPage` ve masa gelen kutusu) `ChapterFinale` bir kez oynar (PlayerPrefs `karine.finale.<id>`); personel satırları Worlds yuvaları ve inceleme geçmişinden. İsteğe bağlı `callKeys` (telefon satırları) ve `personnelBodyKey` finale özel metin verir; boşsa genel `finale.call.1..5` ve `finale.personnelBody`. `CaseRules` final metin anahtarlarının yerelde var olduğunu sınar. `world03` (Almanya, `case018`) `world02` ile aynı kartpostal yolundan açılır.
- `requestKind: "scope"`: başka bir olay kaydını dosyaya alan izin; metinler `requests.scope*`.
- `Investigation` artık `sealed partial` (Lines, Recon ayrı dosyalarda).

## Videosuz dünya açılışı (5 Ekim 2026)
`WorldIntro.videoPath` boşsa `PlayWorldIntro` video oynatıcı kurmaz: `backdropResource` (ülke kartpostalı) `KarineTheme.Veil` ile karartılarak tam ekran çizilir, `graphicsEmbedded=false` yer/stüdyo katmanları aynı zamanlamayla (`introCardStart`, `Update` içinde video saati yerine) belirip söner, 5,6 sn sonra `FinishWorldIntro` normal yoldan dosyayı bırakır. GEÇ aynı. `world02` (Birleşik Krallık, `case011`) bu yolla açılır. Bölüm finali (`chapterFinale`) önceki dosyanın onay faksında, o dosyanın içinde oynar; "sıradaki görev" faks okunmadan açılmaz, yani sıra finale → faks → yeni dünya açılışı → dosya.

## Olay yeri planı (5 Ekim 2026)
- Veri: bir `document` düğümüne `scenePlan` eklenir (`imagePath`, `incident`, `incidentLabelKey`, `occluders[]`, `markers[]`). Koordinatlar plan görselinin içinde 0–1, sol üst (0,0). JsonUtility iç içe dizi okumadığından noktalar `PlanPoint{x,y}` nesnesidir. JsonUtility her düğüme boş bir plan nesnesi kurduğu için varlık `Node.HasScenePlan` (en az bir işaret) ile sınanır.
- İşaret: `kind` = `claimed` (tanığın anlattığı yer) ya da `recorded` (kaydın gösterdiği yer); kilit `requiresAsked` / `requiresRead`, `Investigation.MarkerAvailable`. `facing` derece (0 = sağ, saat yönü), `fov` 1–360.
- Geometri `Runtime/SightLine.cs`: `Hit` (parça kesişimi), `Blocked`, `Sees` (koni + engel), `Cone` (64 ışınlık yelpaze, ilk engelde ya da plan kenarında durur). Arayüzden bağımsız; EditMode testli. Vaka kuralları da kullanır (`Case015Rules`).
- Çizim `KarineUI.ScenePlanView` (`BubeApp.EvidencePaper` içinden): kare çerçeve, görsel yoksa engeller çizilir, olay çarpısı, işaret düğmeleri (`PlanMarker_<id>`), seçilene koni; alttaki `ScenePlanCaption` işaretin etiketini yazar. Renk/punto tokenlardan.
- Doğrulayıcı: plan yalnız belgede; olay noktası ve etiketi var; engeller ≥3 noktalı ve içeride; işaret kimlikleri tekil, içeride, türü geçerli, kilitleri var olan soru/düğüme bağlı; `imagePath` verildiyse dosya var.


## Vaka üretim hattı (5 Ekim 2026)
#025–#073 elle değil, oturum karalama klasöründeki bir Python hattıyla üretildi; hat depoya girmedi, çıktıları girdi. Her dosya için:
- `genNNN.py` (`lib.py` üstünde): `init` kişileri ve görünüşlerini, `setup` kanca/ip/görsel briefini, `doc`/`interview`/`clue`/`V`/`Mth`/`K`/`E`/`Rc` düğümleri ve rapor seçeneklerini kurar; `write` `Cases/caseNNN.json` ile `Locales/tr.caseNNN.json`'u yazar. Görüşme sorusunda `present` (çözücü kaynaklar), `presented`, `decoy {kaynak: yanıt}`, `variants`, `after`; düğümde `warrant`/`slots`, `requestKind` (`line`, `archive`), `line`, `reopenYear`/`reopenNoteKey`, `closes`/`closedNote`.
- `wire.py` `Worlds.json` yuvasını ve önceki dosyanın `nextCaseId`'sini bağlar. (İki kopukluk bu adımın yeniden çalıştırılmamasından doğdu; `CaseChainRules` notu yakaladı.)
- `lint.py` ek metin kuralları: öne sürülen ve yem kaynak kişinin ilk adını metinde taşımalı; dosyalar arası ad çakışması (`ids.py`); yasak kelimeler.
- `rules.py rNNN.json` → `Editor/Validation/CaseNNNRules.cs`: `forbid` (adsız kaynaklar), `require` (kişiye görünmesi gerekenler), `warrants {node, weak, strong}`, `lines`, `closes`, `archive {node: yıl}`, `decoys [{node, q, src}]` (yem bir yanıt döndürür ve soruyu çözmez: `DecoyAnswerKey != null && !SourceMatchesQuestion`), `recon`, `finale` (sonraki dosya anahtarı), `correct`/`wrong`.
- `design.py` `Docs/CASENNN_DESIGN.md` ve `Docs/CASENNN_PROMPTS.md`'yi yazar; `trkeys.py` final metin anahtarlarını `tr.json`'a ekler; `intro.py` `worldIntros` girişini kurar.
- Not: zayıf izin denemesi `report` gibi `notReportSource` bir düğüm içeremez; `SubmitWarrant` onu kaynak saymaz ve kural yanlış nedenle düşer.
- Bulgu görselleri: `Tools/item_bindings.json` (vaka → `{id, node, name, detail}`) ve `Tools/bind-items.py`. Araç `Items/<vaka>_<id>.png|jpg` varsa bulguyu o düğümün `relatedItems`'ına (`<vaka>.item.<id>.name/detail`) ekler ve `.meta`'yı `nPOTScale: 0` ile kurar (Unity varsayılanı 1, `CaseRules` reddeder); yoksa atlar, çünkü `CaseRules` olmayan görseli hata sayar. Tekrar çalıştırmak güvenli. #025–#073 düğüm eşlemesi metin benzerliğiyle çıkarıldı, #013–#024 elle; yanlış düşen olursa JSON'da düzeltilir. `--check` yalnız raporlar.
- Son dosya (`case073`) `nextCaseId` boştur; `chapterFinale.nextCountryKey = finale.country.archive`, `nextFileKey = finale.file.end`. `NewFileDrop` bu metinlerle bir kez oynar; sonra açılacak dosya yoktur.

## Yem bekletmesi ve öne sürülemez sinyal satırları (7 Ekim 2026)
- `Investigation.Decoy.cs` (kısmi sınıf): `HoldAfterDecoy(node, q)` `Progress.decoyHolds`'a `düğüm/soru/işaret` yazar; işaret `read.Count + asked.Count`. `DecoyHeld` işaret değişmediyse ve `OtherWorkLeft` (keşfedilmiş okunmamış görüşme dışı düğüm ya da sorulabilir başka soru) doğruysa soruyu tutar. `CanAskQuestion` artık `!DecoyHeld` da ister. `BubeApp.Interview.cs` yem yanıtında `HoldAfterDecoy` + `Save()` çağırır. Eski kayıtlarda `decoyHolds` boş gelir (`??=`).
- `CaseRules.ValidateCctv`: `signalKey`/`glitchKey` taşıyan ya da metni "KAYIT BULUNAMADI"/"sinyal kesil" içeren olay `notPresentable` değilse sorun.
- Test: `DecoyHoldTests.DecoyClosesQuestionUntilNewProgress`.

## Çok dil ve kanon metin (8 Ekim 2026)

- **Diller:** `Languages.All` = tr, en, de, fr, it, es, pt-BR (`LocaleLoader.cs`). Ayarlarda yalnız ortak dosyası (`Bube/Locales/<dil>.json`) bulunan diller listelenir; dil adı kendi dilinde yazılır.
- **Seçim sırası:** kayıtlı tercih (`karine.language`) → cihaz dili (`Application.systemLanguage`) → İngilizce → Türkçe.
- **Düşme zinciri:** `LocaleLoader.LoadPlayable(dil)` seçilen dili yükler, eksik anahtarları İngilizceden, sonra Türkçeden tamamlar. Ekranda hiçbir zaman `[anahtar]` görünmez.
- **Kanon metin:** `Investigation.RuleText` her zaman Türkçedir. "Kayıt ancak adı geçiyorsa öne sürülür" kuralı (`SourceConcernsPerson`, `MentionsPerson`) `game.RuleString(anahtarlar)` ile kanondan okunur; oyuncunun dili kuralın sonucunu değiştirmez. Dosya araması süzgeci ekrandaki metne bakar (adlar her dilde aynı).
- **Büyük harf ve yüzde:** `KarineUI.TextCulture` oynanan dilin kültürüdür (önceden sabit `tr-TR` idi; İngilizcede "HİSTORY" üretirdi). `KarineUI.Percent` Türkçede `%60`, Fransızcada `60 %`, diğerlerinde `60%` yazar.
- **Kurulu çeviriler (8 Ekim 2026):** yalnız `en` (ortak + 73 vaka, 12.755 satır, 0 sorun); diğer diller eksik satırda İngilizceye düşer.
- **Çeviri denetimi:** `Editor/Validation/TranslationRules.cs` her kurulu dil için Türkçe kuralların karşılığını uygular: kanonda olmayan anahtar, yer tutucu, kişi adı (unvan hariç) değişmezliği, ad geçme eşitliği, gerçek kurum adı, hüküm veren davranış satırı, ödüllü ipucunun vaka gerçeğine değmemesi. Unity'siz eşi: `Tools/check-translation.py <dil> [caseNNN]`. Kural metni: [TRANSLATION_GUIDE.md](TRANSLATION_GUIDE.md).
- **Fontlar:** mevcut 11 font altı dilin bütün harflerini kapsıyor (fontTools ile denetlendi). Japonca kapsam dışı olduğu için CJK fontu gerekmedi.

## Kariyer sicili (8 Ekim 2026)

Kariyer ekranına üçüncü sekme **Sicil** (`BubeApp.CareerRecord.cs`): faks geçmişinden güven seyri (`KarineUI.TrustCurve`, kesik çizgi başlangıç güveni), kademe değişimleri ve ülke karnesi (uygun / eksik / hatalı ve net güven). Kademe `Investigation.StatusKeyFor(değer, kurallar)` ile hesaplanır; masadaki rozet ve sicil aynı kuralı kullanır. `career-rules.json`'daki `unsolvedLoss` hiçbir değerlendirme türüne bağlı değil (değerlendirme türleri: supported, incomplete, falseAccusation).


## Kariyer: gözetim ve seri (8 Ekim 2026)

`Investigation.Career.cs` faks teslimi, seri, gözetim ve ödüllü yeniden açmayı taşır. `CareerProgress.retired` artık hep `false`. Kayıtla uyum için alan durur, yüklemede `probation`a çevrilir. `FaxReview.startedProbation/endedProbation/streakBefore`, iadenin neyi geri alacağını saklar. `TrustStatusKey` gözetimde `career.status.probation` döndürür.


## Ses susturma ve geçiş yüklemesi (8 Ekim 2026)

`AudioDirector.Hush(bool)` müzik, ortam ve katmanı `DuckSeconds` içinde sıfıra indirir (`Bed = duck*hush`). Dünya filmi ve CCTV izleyicisi açar, kapanışları geri alır. `BubeApp.LoadThen(next)` hedef ekranı kurar, üstüne `KarineUI.LoadingScreen` katmanını `Loading.TransitSeconds` boyunca koyar. `PrintOut` kâğıdın tüm alt `Label`larını sırayla basar, düğmeleri basım bitene dek gizler. `FrameRate` Android'de `Screen.SetResolution(..., RefreshRate)` ile yenileme hızı ister.

## Hesaplar ve bulut kaydı (8 Ekim 2026)

- **`Accounts` (Runtime):**
  - `IAccountProvider` dikişini ve misafir/yerel gerçeklemeyi (`LocalAccountProvider`) içerir.
  - Etkin hesabı PlayerPrefs'te tutar: `karine.account.kind`, `karine.account.id`, `karine.account.chosen`.
  - Kayıt yolu `persistentDataPath/accounts/<misafir|oyuncuKimliği>/bube-*.json`. `BubeApp.CaseSavePath` ve `CareerSavePath` buradan okur.
  - `MigrateLegacy` kökteki eski kayıtları misafire taşır.
  - `Adopt` misafiri hesaba devreder ve hedefteki kaydı ezmez.
  - `Merge` bulut zarfını (`CloudEntry{ticks,json}`) dosya yazım saatiyle karşılaştırır; yeni olan kazanır.
- **`Bube.Accounts` derlemesi (`Assets/Bube/Accounts/`):**
  - `defineConstraints: KARINE_UGS` ile yalnız UGS Authentication kuruluysa derlenir.
  - `versionDefines`: `KARINE_CLOUD`, `KARINE_GPGS`, `KARINE_APPLE`.
  - `UgsAccountProvider` mobilde `BeforeSceneLoad` ile kendini takar.
  - Google kolu: GPGS `ManuallyAuthenticate` → `RequestServerSideAccess` → `SignInWithGooglePlayGamesAsync` / `Link…`.
  - Apple kolu: `AppleAuthManager.LoginWithAppleId` → `SignInWithAppleAsync` / `Link…`. `AccountAlreadyLinked` gelirse o hesaba girilir.
  - Silme: Cloud Save `DeleteAllAsync` + `DeleteAccountAsync`.
- **`BubeApp.Account.cs`:**
  - `AccountBoot` sessiz geri yüklemeyi yapar, giriş kağıdını gösterir ve `PullCloud` çağırır.
  - `Save()` → `CloudDirty()`: 4 sn sonra itme. Arka plana atılınca `FlushCloud`.
  - Hesap değişince `LoadSaves` + `Home`.
- **Paketler:**
  - `com.unity.services.authentication` 3.8.0
  - `com.unity.services.cloudsave` 3.4.1
  - `com.google.play.games` 2.2.1 (OpenUPM)
  - `com.lupidan.apple-signin-unity` 1.5.0 (OpenUPM)
- **Doğrulama sınırı:** Editor'de (Android hedefi) GPGS kolu derlendi. iOS kolu (`UNITY_IOS`) derlenmedi; ilk iOS derlemesinde görülecek.

## 9 Ekim 2026 — Misafir bulut kaydı (Firebase REST)

`FirebaseAccountProvider` (`Assets/Bube/Runtime`) `IAccountProvider`ı paketsiz gerçekler: Identity Toolkit `accounts:signUp` ile anonim kimlik, `securetoken` ile yenileme (anahtar `PlayerPrefs` `karine.firebase.refresh`), Firestore REST ile `users/{uid}/saves/{anahtar}` belgeleri (`entry` = `CloudEntry` JSON). `config.json` → `firebaseApiKey` + `firebaseProjectId` doluysa ve başka sağlayıcı takılı değilse `BubeApp` açılışta takar. Arayüze `CloudReady`/`CloudId` eklendi: bulut eşitlemesi artık `Accounts.SignedIn` yerine `CloudReady`ye bakar, misafir de buluta yazar. Misafir klasörü (`accounts/guest`) değişmedi; anonim kimlik `Accounts.Id`ye yazılmaz. Silme: belgeler → kimlik (`accounts:delete`) → yerel klasör; bir adım düşerse yerel silinmez. Kurallar: `Firebase/firestore.rules`.
