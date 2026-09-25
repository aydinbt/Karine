# KARINE — UI/UX Kit (bağlayıcı tasarım sistemi)

**Referans görsel:** [Reference/UI_KIT.png](Reference/UI_KIT.png) — kullanıcı tarafından 25 Eylül 2026'da verildi.
**Statü:** ilham değil, **spesifikasyon**. Bir bileşenin nasıl görüneceği tartışmalıysa önce görsele bakılır, oradaki bileşen yeniden kullanılır.
**Kod karşılığı:** `Assets/Bube/Runtime/UI/KarineTheme.cs` (değerler) + `Assets/Bube/Runtime/UI/KarineUI.cs` (bileşenler).

## Değişmez kural

Yeni ekran yazarken **yeni tasarım dili üretilmez**. Sıra şudur:

1. Kit görseline bak.
2. `KarineUI`de o bileşen var mı?
3. Yoksa en yakınından türet.
4. Yeni bileşen en son seçenektir ve `KarineUI`ye girer, ekranın içine değil.

Renk, punto, boşluk, köşe yarıçapı ve süre ekranın içine **yazılmaz**; `KarineTheme`den alınır.

## Palet

Değerler kit görselindeki etiketli sekiz kutudan okunmuştur. `UiKitTests.Palette_MatchesTheKitSwatches` bunları kilitler.

| Rol | Hex | Kullanım |
| --- | --- | --- |
| Arka plan | `#0B0F14` | ekran zemini |
| Panel | `#1B2228` | kart, panel |
| Panel 2 | `#2F3A3F` | yükseltilmiş yüzey, kenar |
| Birincil (krem) | `#E8DCC4` | birincil eylem, seçili durum, önemli vurgu |
| İkincil | `#C9B38C` | ikincil metin, üst başlık (kicker) |
| Vurgu | `#8F7A5A` | çizgi, kenar, sıcak vurgu |
| Aktif (teal) | `#29D3C3` | **yalnız** terminal/dijital sistem geri bildirimi |
| Uyarı | `#E94F4F` | **yalnız** yıkıcı eylem ve kritik uyarı |

`Muted` ve `Disabled` paletin dışında değildir, paletten türetilir.

**Diegetic katman ayrıdır.** Kit §13: dosya, evrak, faks, terminal oyun dünyasının parçasıdır ve HUD paletiyle boyanmaz. Bu katman `KarineTheme.Paper` altında tek yerde durur (`Sheet`, `Ink`, `Faded`, `Stamp`). Masadaki kâğıt kâğıt gibi görünür; bu bir palet ihlali değil, kit'in istediği ayrımdır.

## Tipografi

| Rol | Yazı tipi |
| --- | --- |
| Logo | distressed slab — **yalnız marka görseli**, UI metninde kullanılmaz |
| Büyük başlık (≥28) | Alfa Slab One — logonun ahşap dizgi dilinin okunur akrabası |
| Başlık | Roboto Slab Bold |
| Alt başlık | Roboto Slab Medium |
| Gövde / düğme | Inter |
| Teknik metin | IBM Plex Mono |

Teknik metin = oyuncunun "kayıt" olarak okuduğu her şey: `DOSYA #001`, tarih/saat, `KURUM GÜVENİ`, sayfa sayacı, CCTV zaman damgası. Kod karşılığı `KarineUI.Technical(...)`.

Alfa Slab One, Roboto Slab (ExtraBold, Bold) ve Inter (Regular, SemiBold) **depoya kondu** — `Assets/Bube/Resources/Bube/Fonts/`, Git LFS. Roboto Slab Apache 2.0, Inter OFL; lisans metinleri aynı klasörde. Türkçe kapsamı denetlendi: ı İ ğ Ğ ş Ş ç ö ü â î û hepsi var. Roboto Slab'da ₺ yoktur, o yüzden metası IBM Plex Mono'ya yedeklenir. Doğrulayıcı beş dosyanın da varlığını **zorunlu** tutar.

**Gövde yazısı mono değildir.** Kök öğe uzun süre IBM Plex Mono'ya bağlıydı, yani bütün ekranlar monospace okunuyordu ve yazı kötü görünüyordu. Kök artık Inter; mono **yalnız** `Technical` ile gelir: dosya numarası, tarih, saat, güven yüzdesi, kayıt numarası.

**Logo dili ve kit §2.** Kit "logo fontunu normal UI metinlerinde kullanma" der ve bunun gerekçesi okunabilirliktir. KARINE logosu zaten bir görseldir, font değil. Kullanıcı isteğiyle büyük başlıklar logonun ağır ahşap dizgi diline **yakın** bir yüz kullanıyor: Alfa Slab One, yalnız 28 punto ve üstünde (`KarineUI.DisplayFrom`). Küçük başlıklar Roboto Slab'da kalır, çünkü ahşap dizgi küçük puntoda okunmaz — kit'in okunabilirlik kuralı orada ağır basar. Test iki eşiği de kilitler.

## Düğme hiyerarşisi

`KarineButtonKind` dışında düğme biçimi yoktur.

- **Primary** — krem zemin, koyu yazı, sol kenar şeridi. Ekranda **tek** dominant eylem.
- **Secondary** — koyu zemin, krem kenar, krem yazı. (GERİ, KAPAT, İPTAL)
- **Ghost** — kenarsız, sönük. (GEÇ, DETAYLAR)
- **Danger** — koyu zemin, kırmızı kenar ve yazı. Kırmızı dekoratif zemin değildir.
- **IconOnly** — 48 px kare koyu düğme + krem çizgi ikon.

Durumlar tek yerde: NORMAL / PRESSED / DISABLED / SELECTED. Basma geri bildirimi renk kaymasıdır; ölçek veya zıplama yoktur.

## Dokunma

Hedef en az **48 px**, rahat ölçü 56. Görsel ikon 22 px'e inebilir, **hedef inemez**. `UiKitTests.EveryInteractiveComponent_MeetsTheTouchTarget` bunu düğme, ikon düğme ve sekmelerde kontrol eder. Hover hiçbir zaman zorunlu etkileşim değildir.

## İkon dili

Ortak küme `Assets/Bube/Resources/Bube/Art/Icons/` altındadır ve **kit görselinden kesilmiştir**: `folder`, `document`, `gear`, `binoculars`, `pin`, `people`, `chart`, `more`, `close`, `alert`, `info`, `nav_prev`, `nav_next`, `menu_quit`.

Aynı işlev her ekranda aynı ikon: Dosyalar = `folder`, Evrak = `document`, Ayarlar = `gear`, Görüşmeler = `people`, Kariyer = `chart`, Kapat = `close`. Emoji kullanılmaz. Eksik bir ikon doğrulayıcıda hatadır.

## Devinim

Basma ~100 ms, panel ~200 ms, modal ~220 ms, bildirim ~250 ms. Yalnız fade, küçük kayma ve hafif ölçek. Bounce, elastic ve mobil oyun "pop" animasyonu yasaktır.

## Ekranların bileşenlere taşınması

Ekranlar kit bileşenlerine taşındı:

| Ekran / parça | Artık ne kullanıyor |
| --- | --- |
| Terminal kaynak sekmeleri, tablet talep sekmeleri, görüşme soru/geçmiş, kaynak tür süzgeci | `KarineUI.Tabs` |
| Masadaki faks ve yeni evrak uyarısı | `KarineUI.Notification` (başlık + açıklama + tek eylem) |
| Gelen evrak rozeti | kit kırmızısı + `Technical` sayı |
| Kariyeri sıfırlama onayı | `KarineUI.Modal` (yıkıcı) |
| Rapor gönderme onayı | `KarineUI.Modal` — "Gönderdiğin karar geri alınamaz." |
| Ayarlar metin hızı | `KarineUI.Radio` (eskiden iki birincil düğme) |
| Kariyer ekranı güven ve vaka sayısı | `KarineUI.Meter` / `KarineUI.Counter` |
| Dünya girişi ve masaya varış filmleri | `KarineUI.SkipButton` — tek denetim: GEÇ |
| Saat, tarih, damga, sayfa sayacı, kayıt numarası | `KarineUI.Technical` (monospace) |
| Menü örtüsü, ayarlar/hakkında kartı | `KarineUI.Panel` + `Title` + `Rule` |
| Kâğıt/dosya katmanındaki seçim, eylem ve sessiz düğmeler | `KarineUI.PaperButton` (`Action`/`Choice`/`Quiet`) |
| Katman kapatma ve alan temizleme `×` | `KarineUI.CloseButton` (`paper: true` kâğıt üstünde) |

**Sinematikte tek denetim GEÇ'tir.** Duraklatma, ilerleme çubuğu, süre ve hızlandırma **yoktur**: film ya izlenir ya geçilir. Ara kademeler oyuncuya karar verdirmez, yalnız kareyi kalabalıklaştırır. Duraklatma ve kare ilerletme **CCTV izlemede** anlamlıdır ve orada kendi denetimleri vardır (oynat/duraklat, kare ilerlet, baştan al).

`KarineUI.SkipButton` kit'in **birincil** düğmesidir — sinematikte tek eylem odur: dolu krem zemin, koyu yazı, sol eylem kenarı ve `cine_skip` simgesi. Zemin **saydam değildir**; filmin üstünde bile düğme düğme gibi durur, test bunu kilitler. Aynı bileşen CCTV görüntüsündeki "GEÇ" için de kullanılır; CCTV'nin kapat/oynat/kare ilerlet/baştan al düğmeleri kit'in ikincil düğmesidir. Metin düğmenin kendi `text`i değil ayrı bir etikettir — UI Toolkit'te bir `Button`un metni ile çocukları üst üste biner, simge ancak böyle yanına oturur. Bazı filmlerde düğme üretici filigranının **üstüne oturmak zorunda**; orada yeri film karesine göre hesaplanır, yüksekliği rahat dokunma hedefine ve eni 260 px'e kapanır. Öteki filmlerde sağ altta durur.

**Kâğıt tonları birleştirildi.** Kâğıt katmanında birbirinden bir iki basamak farklı otuzdan fazla bej vardı (`.82/.76/.65`, `.79/.72/.61`, `.78/.71/.61` …). Hepsi `KarineTheme.Paper` altındaki beş tona indi: `Sheet`, `Light`, `Tint`, `Edge`, `Stamp`, `Ink`, `Faded`. Kit "kendi başına yeni bir stil icat etme" dediği için bu tonlar tek yerde durur.

## Örtü, cam ve dosya kabı

Ekranlar kendi koyusunu da seçmez. Bir katman açıldığında altındaki sahneyi örten perde `KarineTheme.Veil(alpha)`dan gelir — yoğunluk sahneye göre değişir, renk değişmez. Terminalin ve tabletin cam yüzeyi paletin koyu ucundan türeyen üç duraktır: `GlassDeep` (tablet, kayıt listesi), `Glass` (sorgu şeridi, başlık), `GlassLift` (üstteki kart, seçim satırı). Masadaki dokunulabilir noktanın üstüne gelince görünen iz `HotspotHover`dır ve kit'in vurgu kahvesinden üretilir. Saydamlık gerekiyorsa ekran kendi RGB'sini yazmaz, `KarineTheme.Alpha(token, a)` kullanır.

Kâğıdın **altındaki** fiziksel malzeme de tek yerden gelir: `Paper.Folder` (dosya kabının yüzü), `Paper.FolderEdge` (üst kenar, logo gölgesi), `Paper.FolderDeep` (sırt gölgesi), `Paper.Board` (mukavva/pano), `Paper.Approved` (kabul mührünün yeşili). Bunlar HUD paletinin dışındadır ama uydurma da değildir — diegetik katmanın kendi beş durağıdır (kit §13).

## Ham renk borcu

Kit'ten **önce** yazılmış ekranlarda 156 doğrudan `new Color(...)` çağrısı vardı; taşıma sonrası **16** kaldı. Kalanların hepsi oyun **sanatıdır**, arayüz paleti değil: piksel portrenin ten/saç/giysi tonları (`BubeApp.Interview.cs`) ve CCTV'nin cam, tarama çizgisi, parazit bandı ve köşe işareti efektleri (`BubeApp.Cctv.cs`). Bir yüzü arayüz kremine boyamak portreyi bozar; ikisi de kit paletinden gelmemeli ve kodda böyle yazılıdır. Doğrulayıcıda bir **kilit** var: sayı 16'yı aşarsa doğrulama düşer. Yeni kod rengi `KarineTheme`den alır; bu sayı ancak aşağı çekilir.

## Punto borcu

Punto da ekranın içine elle yazılmaz: ekranlardaki her `style.fontSize` ataması `Typography.Snap`ten geçer (tek istisna CCTV'nin görüntüyle ölçeklenen kamera yazısıdır, o `Mathf.Clamp` ile kendi ölçeğini kullanır). Doğrulayıcı satır satır bakar ve kapıdan geçmeyen ilk atamayı dosya:satır olarak bildirir.
