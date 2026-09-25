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
| Başlık | Roboto Slab Bold |
| Alt başlık | Roboto Slab Medium |
| Gövde / düğme | Inter |
| Teknik metin | IBM Plex Mono |

Teknik metin = oyuncunun "kayıt" olarak okuduğu her şey: `DOSYA #001`, tarih/saat, `KURUM GÜVENİ`, sayfa sayacı, CCTV zaman damgası. Kod karşılığı `KarineUI.Technical(...)`.

Roboto Slab (ExtraBold, Bold) ve Inter (Regular, SemiBold) **depoya kondu** — `Assets/Bube/Resources/Bube/Fonts/`, Git LFS. Roboto Slab Apache 2.0, Inter OFL; lisans metinleri aynı klasörde. Türkçe kapsamı denetlendi: ı İ ğ Ğ ş Ş ç ö ü â î û hepsi var. Roboto Slab'da ₺ yoktur, o yüzden metası IBM Plex Mono'ya yedeklenir. Doğrulayıcı artık dört dosyanın da varlığını **zorunlu** tutar.

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
| Dünya girişi ve masaya varış filmleri | `KarineUI.CinematicControls` (duraklat, ilerleme, süre) + GEÇ |
| Saat, tarih, damga, sayfa sayacı, kayıt numarası | `KarineUI.Technical` (monospace) |
| Menü örtüsü, ayarlar/hakkında kartı | `KarineUI.Panel` + `Title` + `Rule` |

**Sinematik kontroller**, kit §7'nin istediği gibi bütün oyunda tek biçimdir: duraklat, ilerleme çubuğu, süre. **Sinematiklerde hızlandırma yoktur** — filmden tek çıkış GEÇ'tir. Hızlandırma/kare ilerletme yalnız CCTV izlemede anlamlıdır ve orada zaten vardır (oynat/duraklat, kare ilerlet, baştan al). Bileşen hâlâ isteğe bağlı bir hızlandırma düğmesi alabilir; iki eylemin ayrı kalmasını test kilitler, ama filmlerde bağlanmaz. "GEÇ" üretici filigranının **üstüne oturmak zorunda** olduğu için çubuğun içinde değil kendi yerinde durur; çubuk o ekranlarda kısalır. Kutu filigranı örter ama yüksekliği rahat dokunma hedefine, eni 260 px'e kapatılır, yani kit düğmesi gibi görünür.

**Kâğıt tonları birleştirildi.** Kâğıt katmanında birbirinden bir iki basamak farklı otuzdan fazla bej vardı (`.82/.76/.65`, `.79/.72/.61`, `.78/.71/.61` …). Hepsi `KarineTheme.Paper` altındaki beş tona indi: `Sheet`, `Light`, `Tint`, `Edge`, `Stamp`, `Ink`, `Faded`. Kit "kendi başına yeni bir stil icat etme" dediği için bu tonlar tek yerde durur.

## Ham renk borcu

Kit'ten **önce** yazılmış ekranlarda 156 doğrudan `new Color(...)` çağrısı vardı; taşıma sonrası **65** kaldı. Kalanlar çoğunlukla CCTV taraması gibi saydamlıklı efektler ve piksel portrelerin ten tonlarıdır — bunlar oyun sanatıdır, arayüz paleti değil. Doğrulayıcıda bir **kilit** var: sayı 65'i aşarsa doğrulama düşer. Yeni kod rengi `KarineTheme`den alır; bu sayı ancak aşağı çekilir.
