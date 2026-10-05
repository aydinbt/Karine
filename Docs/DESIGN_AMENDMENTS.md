# Karine — sonraki tasarım düzeltmeleri

## Ana menü görsel hiyerarşisi (27 Eylül 2026)

Kullanıcının sağladığı yeni referansta büyük KARINE logosu sol üstte, kısa Türkçe slogan hemen altında, beş eşit dokunulabilir satır sol ortada ve Bora'nın kimlik kartı sol alttadır. Ana menüde zaten oynayan sinematik video **korunur**; logo, ikonlar, yazılar, düğmeler ve kimlik kartı videonun üstünde ayrı UI öğeleridir. Mevcut `KarineLogo`, ikon sprite'ları ve Bora portresi kullanılır; referans görsel bütün ekranı kaplayan tek bir PNG olarak uygulanmaz. Etkin satır krem, öbürleri koyu yüzeydir; marka ve karakter sağdaki videoyla yarışmaz. Referanstaki Kadıköy ibaresi oyun kanonu değildir: Bora İstanbul/Beşiktaş şubesindedir.

Menü eylemleri Devam Et (kayıt yoksa Oyuna Başla), Vakalar, Kariyer, Ayarlar ve Hakkında'dır. Yeni kariyer akışı ayarlarda, çıkış onayı mobil geri tuşunda kalır. Bu karar yalnız ana menünün sunumunu değiştirir; soruşturma akışı ve vakalar değişmez. Farklı yatay telefon oranlarında hiyerarşi, güvenli alan ve dokunma Play Mode/cihazda gözle kontrol edilecektir.

## Oyun adı: Karine (25 Eylül 2026)

Oyunun mağazada görünecek adı **Karine** olarak karara bağlandı. Karine, hukukta "dolaylı kanıttan çıkarılan sonuç" demektir; oyuncunun ifadeleri ve kayıtları karşılaştırıp gerekçeli bir sonuca varması olan oyun tezinin birebir karşılığıdır.

`bube` çalışma adı olarak sona erdi. Stüdyo adı **bubeGames**, oyun içi kurmaca kurum adı **bube Departman / bube Departman / BDS** olduğu gibi kalır — bunlar oyun adı değil, dünya kurgusunun parçasıdır. Arayüzdeki "bube Departman" metinleri değişmez.

Uygulanan yerler: `config.json` `title`, `ProjectSettings` `productName`, `applicationIdentifier` (`com.bubedigital.karine`), `tr.json` `about.body`. Depo: `github.com/aydinbt/Karine`; çalışma klasörü `bubeGame/karine-mobile`. Aday havuzu, eleme ölçütleri ve müsaitlik kontrol listesi [NAMING.md](NAMING.md) içinde saklanır.

### Oyun içi kurum adı değişmiyor (25 Eylül 2026)

Oyun adının Karine olması üzerine, oyun içindeki 16 "bube Departman / bube Departman" metninin de Karine'ye çevrilmesi değerlendirildi ve **çevrilmemesine karar verildi**. Kurmaca kurum **bube Departman / bube Departman**, terminal **BDS — bube Departman System** olduğu gibi kalır.

Gerekçe: Karine soyut bir hukuk terimidir ("dolaylı kanıttan çıkarılan sonuç"); oyun başlığı olarak tezi taşır ama kurum adı olarak kuruluş hissi vermez. Oyun başlığının kurumu adlandırmak zorunda olmadığı kabul edildi. Kurum adı sorusu kapanmış değildir; ileride yeniden açılırsa bu madde güncellenir.

## Dünya açılışları ve yedi ülke (24 Eylül 2026) — ülke sayısı 26 Eylül 2026'da ona çıktı

Yeni kullanıcı kararı: yedi ülke/dünya hedefleniyor. Dünya 1 Türkiye'de Bora'nın Beşiktaş şubesine ilk gelişini POV sinematik gösterir: gözler kapalı/yere dönük başlangıç, başın kalkışı ve gözlerin açılması, karakola üç dört adım yaklaşma, kararma, masaya oturma ve ilk dosyanın önüne bırakılması. Sinematik her yeni kariyerde yalnız bir kez oynar. Sağ orta bölgede `bubeGames` ve altında `powered by bubeDigital` görünür. Dünya 2'nin ülkesi henüz belirlenmedi; sonraki her dünya için ayrı açılış gerekir. [WORLD_OPENINGS.md](WORLD_OPENINGS.md) bu kararın üretim kaydıdır.

Kullanıcı daha sonra yaklaşık 10 saniyelik Gemini üretimi `Create_a_complete_second_op.mp4` klibini Dünya 1 açılışı olarak seçti. Videodaki Türk bayrağı ve `bubeGames / powered by bubeDigital` yazısı korunur; oyunun önceki yazı/bayrak katmanı bu klipte gizlenir. Sağ alttaki Gemini simgesi kaynak videoda korunur; mobil yatay oranlarda videonun kadrajını izleyen `Geç` düğmesi bu bölgeyi örter. Önceki Higgsfield klibi artık aktif değildir.

## Dosya #001 anlatı tutarlılığı (24 Eylül 2026)

Eşya kaydı için kesinleştirilen ayrıntı: Mert'in ihbarla sunduğu faturada dizüstü bilgisayarın seri numarası **LQ7B-024861**'dir. Aynı numarayı taşıyan cihaz, olay günü **14.10'da** Beşiktaş'taki ikinci el elektronik işletmesine Hasan Kaya adına düzenlenmiş fişle teslim edilmiştir. Bu saat Hasan'ın yaklaşık 13.00'te binadan ayrılmasından sonradır. Belge teslim ve seri numarası eşleşmesini doğrular; tamamlanmış satış, Mert'in evine giriş anı veya kayıp saat ve para hakkında doğrudan kanıt değildir.

Elif'in olay günü kullandığı ve saksıya bıraktığı anahtar, kendisinde kalan ayrı kopyadır. Mert'in daha önce saksıda sakladığı kendi yedeği kapıda kaldığı eski olaydan sonra geri alınmıştır. Hasan eski olayda saksının anahtar saklama yeri olduğunu görmüştür; Elif'in bıraktığı kopyayı bu bilgiyle bulur. İlk rapordaki boş saksı, Hasan'ın olay sonunda anahtarı yanında götürmesiyle uyumludur.

Hasan, Bora'nın Elif'le görüşüp görüşmediğini kendiliğinden bilemez. Oyuncunun okuduğu başka bir görüşme, Hasan'ın yanıtını tek başına değiştirmez; bilgi kendisine açıkça sunulduğunda ayrı takip sorusu kullanılmalıdır. Mert'in anahtar takip görüşmesi, ilk görüşme tamamlanıp bina girişi kaydı incelendikten sonra, **Elif'in saksıya bıraktığı anahtarı anlattığı görüşme** veya **Mert'in kapıyı kilitlediğine dair ek cevabı** üzerine açılır. Bu iki somut bilgi yolu, Elif'in itirafını tek zorunlu rota yapmadan takip sorusuna hikâye gerekçesi verir. İlk görüşmelerdeki inkârlar daha sonraki sorularda gerçekleşmiş ziyareti peşinen varsayan bir dille sorgulanmaz.

`MASTER_GAME_CONTEXT.md` kullanıcının sağladığı ana tasarım bağlamıdır ve değiştirilmeden korunur. Daha sonra doğrudan verilen kararlar bu dosyada tutulur.

- Dosya #001 CCTV yalnızca metin tabanlıdır. Oyuncu görüşmeden sonra dosyayı kapatıp masadaki terminale kendisi dokunur; kayıtlar arkası karartılmış/bulanık odak ekranda bozuk satırlar olarak görünür ve oyuncu incelenebilir satırları netleştirir. Gerçek sinyal boşluğu doldurulmaz.
- Sonraki bazı zor vakalarda, vaka tasarımı gerektirirse animasyonlu/canlı CCTV görüntüsü olabilir. Bu bir zorunlu bölüm sırası kuralı değildir; metin terminali temel biçim olarak kalır. Ana belgedeki genel “CCTV’de video yok” ifadesine bu dar istisna uygulanır.
- Oyunun tüm vakalarındaki değişmeyen omurga oyuncunun kendi aklıyla kaynakları açması, karşılaştırması, yeniden soru sorması ve sonucu gerekçelendirmesidir. Yeni bölüm özellikleri bunu genişletir.
- **Görüşme ekranı değişmez kuralı (kullanıcı teyidi):** Oyuncu yalnız karakterin göğüs üstü pixel-art sprite'ını, adını/temel kimlik bilgilerini, söylediği cümleyi ve o anda sorulabilen soruları görür. “İfade Durumları” şeridi, `Normal / Düşünüyor / Tedirgin / Savunmada` gibi iç durum adları ve `Notlar`/analiz paneli oyun ekranına konmaz. İç durumlar yalnız arka planda sprite ve yanıt seçimini etkileyebilir. Oyuncu, sınırlı paletli ve sade yüz geometrili sprite'taki az sayıdaki mimik farkını ve ifadeyi kendi yorumlar; tedirginlik veya yalan suçluluğa eşitlenmez. Yeni karakter varlıkları düşük/orta çözünürlüklü, silüetle ayırt edilebilir ve yaş/cinsiyet algısı net olacak biçimde gözden geçirilir.

## Sahne ve odak mimarisi (23 Eylül 2026)

Dört gerçek Unity sahnesi vardır: `BootScene` teknik yükleme, `MainMenuScene` atmosferik ana menü, `OfficeScene` soruşturma masasının kalıcı merkezi ve `InterviewScene` fiziksel görüşme odası. Dosya, gelen evrak, tablet/BDS/CCTV ve sonuç raporu yeni sahne yüklemez; `OfficeScene` içinde nesneye dokunma → arka planı karartma → nesneyi öne alma → odak arayüzü açma diliyle çalışır. Görüşme bitince tekrar ofise dönülür. İlk vakada kullanılmayan BDS yetkileri sahte kilitli menü olarak gösterilmez; yetki gerçekten açıldığında araç eklenir. Özel olay yeri sahneleri ileride yalnız içerik gerektirirse açılır.

Ana menü ilk sürümde `Devam Et` (kayıt varsa), `Yeni Oyun`, `Ayarlar`, `Hakkında` ile sınırlıdır. İstatistik/Kariyer/Arşiv sistemleri tamamlanmadan ana menüde yer kaplamaz. `Personel Profili` ve fiziksel `Gelen Evraklar` odak görünümü daha sonra ayrı UI durumları olarak tamamlanacaktır; bunlar için yeni Unity sahnesi açılmayacaktır.

## Dosya #001 kamera görselleri (24 Eylül 2026)

Kullanıcı, görsel üretim imkânı nedeniyle Dosya #001 için kamera görüntüsü kullanımına izin verdi; önceki mutlak "CCTV videosu yok" kuralı bu vaka için değişir. Mevcut text-only döküm ve tablet incelemesi temel kalır; açılırsa dört kısa, aynı sabit bina girişi kamerasından çekilmiş görüntü onunla eşlenir: 08.27 erkek çıkış, 11.48 kadın giriş, 12.16 aynı kadın çıkış, 17.54 erkek dönüş. İsim/yüz kesin teşhisi, saksı/anahtar, hırsızlık veya Hasan'ın kör aralıktaki hareketi videoda gösterilmez. 12.37–13.08 sinyal boşluğu görüntüsüz kalır. Saat ve kayıt metni videoya gömülmez; tablet veriyle çizer. Üretim referansı `Docs/Media/Case001_CCTV_Empty_Reference.png` oluşturuldu. Dört klip `Assets/StreamingAssets/Bube/CCTV/` içine eklendi ve olay satırlarına veriyle bağlandı. 17.54 klibi kullanıcının sonradan sağladığı özgün giriş görüntüsüdür. Oynatıcı sesi kapalı tutar; oyuncu dökümdeki ilgili `İzle` düğmesine dokunarak açar. Unity derleme ve içerik doğrulaması geçti; gerçek Play Mode ve cihazda hareket, okunabilirlik ve dokunma testi açıktır.

24 Eylül güncellemesi: 17.54 için kullanılan ters klip, kullanıcının sağladığı `cctv004_case001.mov` görüntüsünün 1920×1080, 6,53 saniyelik MP4 sürümüyle değiştirildi; vaka verisindeki `mert_in` satırı aynı dosya yolunu kullanır. Kaynak videonun sağ altındaki Gemini işareti için video üstündeki “Geç” düğmesi 16:9 görüntünün sağ alt köşesine hizalandı; farklı yatay ekran oranlarında üstünü kapatması ve oynatma gerçek cihazda kontrol edilecek. Oynatıcı sesi kapalı tutar; metin dökümü, sinyal boşluğu ve karakter teşhisi sınırları değişmez.

Video oynatıcısının görüntü alanına kamera köşe işaretleri, hafif tarama çizgileri, seyrek sinyal paraziti ve yanıp sönen REC noktası eklendi. Kamera/konum etiketi arayüze sabit yazılmaz: CCTV düğümünün `cctvOverlayKey` alanından, saat ise videolu olayın `overlayTimeKey` alanından gelir. Dosya #001 için etiket “KAMERA 01 · BİNA ÖNÜ”dür; sonraki vakalar kendi konum ve saat metnini verir. Katman görüntünün 16:9 sınırını izler, alttaki yazılı kayıt okunur kalır ve Geç düğmesi sağ altta üstte tutulur. Mobil Play Mode'da efekt şiddeti, yazı ölçeği ve filigran örtmesi doğrulanacak.

Kısa CCTV kliplerinde oyuncu videoyu duraklatıp yeniden oynatabilir, duraklatılmış kayıtta tek kare ileri gidebilir ve başa dönebilir. Üç kısa kontrol tablet videosunun altında en az 48 birim dokunma hedefiyle durur; olayın yazılı satırı ayrı okunur. Unity VideoPlayer'ın doğrudan kare adımı varsa o kullanılır, yoksa desteklenen kare/zaman konumlandırmasına düşülür; ikisi de yoksa Kare düğmesi pasif kalır ve oyuncuya kısa durum metni verilir. Klip sonu Oynat/başa alma durumunu günceller. Bu araç videodaki bir kişiyi teşhis etmez veya kayıt boşluğunu doldurmaz; cihaz desteği ve dokunma ergonomisi gerçek mobil Play Mode'da kontrol edilecek.

## Hikâye yoğunluğu ve tekrar okuma (23 Eylül 2026)

Kullanıcının son yönü: bube bir metin senaryosu ve soruşturma oyunu olarak uzun vadede katmanlı, hatırlamayı/geri dönmeyi gerektiren hikâyeler anlatmalı. Dosya #001 öğrenme vakası olarak kalır, fakat doğrusal kısa metin geçidi olmamalıdır. Yeni katmanlar yeni sırlar, yanıt varyantları, delil ilişkileri ve takip sorularıyla kurulmalı; sırf süreyi uzatan yazı eklenmemelidir. Oyuncu önceki ifadeyi unutabilir; tam konuşma dökümü ve kanıtlar dosyadan tekrar okunabilmelidir. Oyun bunu otomatik “çelişki” işareti veya fail yorumu olarak sunmaz. İlk vakanın önceki 10–15 dakikalık süre tahmini yeni hikâye çalışmasında yeniden değerlendirilecek; yeni hedef süre henüz kararlaştırılmamıştır.

Yanlış sonuç/masum kişiyi suçlama profesyonellik ve departman güveni yönünde değerlendirilir. “Yanlış kişiyi tutukla” eylemi ve sonuçları ayrı bir mekanik olarak kararlaştırılmadı. Eski “yanlış rapor iade edilir” kararı aşağıdaki kesin kapanış ve faks akışıyla değişti.

## İhbar sahibi ve kesin rapor kararı (23 Eylül 2026)

İhbar eden veya mağdur olduğunu söyleyen kişi, soruşturmadan otomatik çıkarılmaz. Dosya #001 sonuç raporunda Mert de seçilebilir; bu onun gerçek fail olduğu anlamına gelmez. Kişi rolü hiçbir vakada peşin suçluluk/masumiyet etiketi değildir. Rapor seçenekleri aynı kâğıt üstünde güncellenir; seçim kaydırma konumunu sıfırlamaz.

Yeni karar: Bora sonuç raporunu emin olarak gönderir ve dosya **doğru veya yanlış fark etmeksizin kapanır**. Bölüm tamamlandı ekranı Bora'nın gönderdiği şüpheli, yöntem ve kanıtı özetler ama doğruluğu açıklamaz. Oyuncu Devam Et ile sonraki görevlendirmeye geçer. Adliye/birim değerlendirmesi daha sonra **Gelen Evraklar**a faks olarak ulaşır; sol üstte okunmamış evrak bildirimi çıkar. Oyuncu faksı açınca doğru/yanlış sonucu, gerçek vaka özetini ve birim güveni değişimini görür. Başarı güveni artırır, başarısızlık düşürür; güven sıfıra inerse Bora soruşturmacı kariyerini tamamlar. Başlangıç güveni 60, +5/−15 ve sıfır eşiği ilk dengeleme değerleridir, nihai kariyer dengesi değildir. Dosya #002 henüz yazılmadığı için yeni görevlendirme geçişi hazır veri yokken bekleme ekranına düşer; gerçek ikinci vaka ve geçiş testi açık iştir.

## Bölüm özeti ve gecikmeli faks (23 Eylül 2026)

Bölüm tamamlandı ekranı referanstaki geniş, fiziksel kâğıt/dosya düzenini izler: dosya görseli, kurmaca bube kimliği, Bora'nın sunduğu kişi/yöntem/kanıt, gönderim zamanı ve gerçekten incelenen kaynaklar. “Çözüldü”, “başarılı soruşturma”, gerçek fail ve güven artışı bu ekranda gösterilmez; departman incelemesi sürmektedir. Alt bölümde Masaya Dön ve Devam Et bulunur.

Kurumsal inceleme faksı rapor anında veya özet ekranında başlamaz. Ancak gerçek bir sonraki vakaya geçildikten sonra yedi saniyelik kayıtlı bekleme başlar (hedef aralık 5–10 saniye). Süre dolunca Gelen Evraklar için sol üstte bildirim belirir; oyuncu faksı kendisi açar. Bekleme ve faks durumu uygulama kapatılıp açıldığında korunur. Dosya #002 henüz içerik olarak olmadığı için mevcut yapıda zamanlayıcı tetiklenmez; bu bağımlılık yol haritasında açıktır.

Faks, yalnız başarı/başarısızlık etiketi vermez: raporda seçilen kişi, giriş yöntemi ve belirleyici kanıtın her biri “desteklendi/desteklenmedi” olarak gösterilir; ardından vakadaki gerçek ve bağımsız kanıt ilişkisi açıklanır. Bu değerlendirme yalnız faks açıldıktan sonra görünür.

## Kariyer ve İstatistikler tasarım sırası (23 Eylül 2026)

Kullanıcı kararı: Önce faks sonrası kariyer/İstatistikler döngüsü tasarım olarak netleşecek; kod daha sonra yazılacak. Tasarım kaydı: [CAREER_AND_STATISTICS_DESIGN.md](CAREER_AND_STATISTICS_DESIGN.md). İstatistikler yalnız açılmış kurumsal faksların güven değişimini ve geçmişini gösterir; bekleyen değerlendirmenin doğruluğunu sızdırmaz. Başlangıç güveni 60, başarı +5, hata −15 ve sıfır eşiği mevcut prototip değerleridir, kesin denge kararı değildir. Kalıcı kayıt, erişilebilir tablet ekranı ve gerçek Dosya #002 geçişi henüz tamamlanmadı.

Sonraki kurumsal değerlendirme taslağı faksı bir oyun ödül/ceza ekranı değil, oyuncunun kendisinin açtığı fiziksel Gelen Evrak belgesi olarak tarif eder. Oyuncu ham güven puanını değil nitel sicil durumunu görür; yanlış raporda doğru fail cevap anahtarı gibi gösterilmez. Bora'nın düşük performansla görevden ayrılması “emeklilik” değil, “aktif soruşturma görevinin sonlandırılması”dır. Taslağın “faks önce, sonraki dosya sonra” sırası, daha önce kararlaştırılan “sonraki vakadayken faks” sırasıyla çelişiyor; tasarım kaydında açık karar olarak bırakıldı. Kod değiştirilmedi.

Son kullanıcı kararı bu sıra çelişkisini çözer: Faksın kesin geliş zamanı önemli değildir; akıcı ve doğal bir noktada, oyuncunun mevcut etkileşimini kesmeden gelmesi yeterlidir. Sonraki dosya faks okunana kadar kilitlenmez. Yedi saniye yalnız mevcut prototip zamanlamasıdır, tasarım zorunluluğu değildir. Önceki paragraftaki sıra karşılaştırması tarihsel bağlamdır.

Bölüm özetindeki değerlendirme durumu tek, üstte konumlu ince bir şeritte gösterilir: “Kurumsal incelemede · Sonuç Gelen Evraklar’a faksla bildirilecek.” Önceki uzun “Bu aşamada raporun doğru olup olmadığı anlaşılmaz” açıklaması ve alttaki tekrar eden durum paneli kaldırıldı. Bu durum başarı/başarısızlık işareti değildir.

## Kariyer altyapısının ilk uygulaması (23 Eylül 2026)

Kariyer ve İstatistikler tasarımının ilk kod karşılığı eklendi. Rapor sonrası faks gerçek ikinci vaka olmadan da doğal gecikmeyle ulaşabilir; oyuncunun işi bölünmez. Faks kişi, yöntem ve kanıtı ayrı değerlendirir, fakat yanlış iddiada doğru faili cevap anahtarı gibi göstermez. Güvenin sayısal hesabı kayıtta saklanır; faks ve tablet personel sicili nitel durum gösterir. Değerlendirme geçmişi kalıcıdır ve eski son-faks kaydı ilk açılışta geçmişe alınır. Açık Unity Editor ve cihaz görsel/dokunma testi hâlâ gereklidir.

## Açık soruşturma alanı ve iddia temelli akış (23 Eylül 2026)

Kullanıcı, vakaların net bir gerçek olay örgüsü olsa da gerçek hayattaki gibi araştırılmasını istedi. Birinin ifadesi yanlış olsa bile yeni kişi, soru veya kaynak açabilir; gizli doğru/yanlış etiketi ilerleme anahtarı değildir. Oyuncu masadan kaynakları kendi sırasıyla açar ve yüz ifadesi, CCTV, belgeler ve maddi delille iddiaları kendisi sınar. Dosya #001’de Elif’in saksı iddiası Mert’e takip sorusu açar; Hasan’a Elif’in veya Mert’in açıklamasından gidilebilir. Eşya Tespit Raporu tanığın doğru cevap vermesine bağlı değildir. Bu, serbest şehir dolaşımı vaadi değil, masa çevresinde doğrusal olmayan soruşturma tasarımıdır.

## Eşya inceleme talebi ve Gelen Evraklar (23 Eylül 2026)

Bora, gerekli kayıt ve ifadelerden sonra masadan Eşya Tespit Raporu için inceleme talebi gönderebilir. Talep doğru/yanlış iddia bilgisine değil, oyuncunun eriştiği kaynaklara dayanır. Oyuncu masada kalır ve başka işlere devam eder. Rapor kısa hazırlık süresinin ardından Gelen Evraklar’a okunmamış evrak olarak gelir; oyuncu kendisi alıp fiziksel dosyaya ekler. Aynı talep yinelenmez; bekleme zamanı ve alındı durumu yerel kayıtta korunur. Bu davranış başka vakalara yeni belge verisi eklenerek uygulanabilir. Yalıtılmış Unity derleme/içerik testi geçti; açık Editor ve cihaz etkileşimi bekliyor.

## Sonuç raporunun yeri (24 Eylül 2026)

Sonuç Raporu masada bağımsız bir eylem düğmesi olarak görünmez; oyuncu fiziksel dosyayı açıp içindeki Sonuç Raporu sekmesinden raporu hazırlar. Bu, raporu soruşturmanın başlangıcında öne çıkaran yanlış yönlendirmeyi kaldırır. Mevcut erken karar verebilme ve yanlış raporu kesin gönderme kuralları değişmez.

## İnceleme taleplerinin yeri (24 Eylül 2026)

Eşya Tespit Raporu gibi uzman incelemeleri masada yüzen yazılı eylem düğmesiyle istenmez. Oyuncu elde tutulan tablette Görüşmeler / İncelemeler arasında geçer ve erişilebilir incelemeyi oradan talep eder. Tablet talebin hazırlanma ve teslim durumunu gösterir; teslim edilen rapor yalnız fiziksel Gelen Evraklar'dan alınarak dosyaya eklenir. Bilgi açılma koşulları ve oyuncunun kaynak sırasını seçme özgürlüğü değişmez.

## Oyuncunun zaman çizelgesi (24 Eylül 2026)

Fiziksel dosyada boş Zaman Çizelgesi sekmesi bulunur. Oyuncu yalnızca duyduğu ifade ve incelediği kayıtlarda geçen saatleri kaynak adıyla birlikte kendisi ekler veya çıkarır. Çizelge saat sırasını gösterir ve yerel kayıtta kalır; oyun iddiaları doğrulamaz, kişi eşleştirmez ve çelişki/fail işareti koymaz. Vaka verisi zaman kartlarını ve bunların görünme koşullarını taşır; sonraki vakalarda yeni kartlar için temel kod değişikliği gerekmez.

## Kaynakla yüzleştirme ve sonuç raporunun mobil akışı (24 Eylül 2026)

Görüşmede seçilen belge veya kayıt o soruya uygun değilse kişi bunu söyleyebilir; yanlış kaynak soruyu tüketmez. Oyuncu “Başka kayıt seç” ile **aynı sorunun** kaynak seçimine döner, konu listesinin başına atılmaz. Uygun kaynak öne sürüldüğünde cevap tutulur ve sonraki sorulara geçilir. Oyun hangi kaynağın doğru olduğunu önceden işaretlemez.

Sonuç Raporu fiziksel dosyada tek, geniş bir odak yüzeyi olarak kalır; telefonda kişi → giriş yöntemi → belirleyici kanıt → son kontrol adımlarıyla ilerler. Her iddiada önce seçim, sonra oyuncunun incelediği bir dayanak kaynağı gerekir. İleri düğmesi bu iki seçim olmadan açılmaz. Geri gidildiğinde önceki seçimler korunur. Gönder düğmesi yalnız son kontrol adımındadır ve dosyayı kesin kapatır; doğruluk faks öncesi açıklanmaz.

Son kontrol adımındaki üç iddianın seçilmiş dayanakları dokunularak aynı ekran üzerinde açılır. Açılan kâğıt kart, belge/BDS için tam metni, görüşme için oyuncunun seçtiği **tek soru-cevabı**, CCTV için seçilmiş saatli satırı gösterir. Rapor kaynak seçicisinde görüşmeler artık kişi başlığı yerine ayrı cevap turları olarak seçilebilir; vaka verisindeki kişi düzeyindeki destek tanımı bu turları da kapsar, belirli satır/cevap tanımları ise yalnız birebir eşleşir. Kart kapandığında rapor ve seçimler yerinde kalır; bu okuma yeni kaynak keşfetmez veya doğruluk değerlendirmesi vermez.

Raporun dayanak açılır listesinde Tümü / Belgeler / İfadeler / CCTV filtreleri bulunur. Belgeler sekmesi erişilmiş fiziksel belgeleri ve BDS metin kayıtlarını kapsar; İfadeler oyuncunun gerçekten aldığı tekil cevapları, CCTV incelenmiş saatli satırları gösterir. Boş kategori pasiftir. Filtre yalnız görünür listeyi değiştirir; seçilmiş kaynak ve rapor kararı korunur, kaynağın doğruluğuna ilişkin ipucu verilmez.

Uzun vakalar için aynı seçicide kelime veya saat araması bulunur. Arama yalnız erişilmiş belgenin tam metninde, alınmış ifade soru-cevabında ve incelenmiş CCTV satırında çalışır; `11:48` ile `11.48` eşdeğer kabul edilir. Kategori filtresiyle birlikte uygulanır; sonuç sayısı ve boş durum açıkça gösterilir. Yatay telefon düzeninde arama alanı, temizleme düğmesi, kategori sekmeleri ve kaynak satırları en az 48 birim dokunma yüksekliğiyle tasarlanır. Bu, oyuncunun yerine kaynak ilişkilendirmesi yapmaz.

Görüşmede kaynak öne sürme listesi de erişilmiş kaynakların tam metninde kelime/saat arar ve belge, ifade, CCTV filtreleri sunar. Dar telefon sütununda filtreler iki satıra ayrılır; listedeki adlar kısalır, seçilen kaynağın tam metni mevcut referans kartından açılır. Seçilen kaynak ve “Kaynağı Öne Sür” eylemi uzun listenin üstünde kalır. Aynı soruya dönüldüğünde arama/filtre korunur; yeni soruda sıfırlanır. Bu düzenin gerçek cihaz klavyesi, kaydırma ve dokunma davranışı Play Mode/cihaz testinde ayrıca doğrulanmalıdır.

Bir görüşmede birden çok açık soru konusu varsa son seçilen konunun adı ve soru sayısı, sağdaki soru listesinin üzerinde sabit kalır. Konu düğmeleri ve sorular aynı kaydırılabilir alanda kalır; başlığa dokunulduğunda sabit etiket güncellenir. Sabit başlık soru veya vaka bilgisi üretmez. Konu düğmeleri en az 48 birim dokunma yüksekliğindedir; dar yatay telefonda gerçek okunurluk ve kaydırma Play Mode/cihaz kontrolü gerektirir.

Görüşme sırasında o kişiyle gerçekten kaydedilmiş soru-cevaplar, sağ sütundaki Sorular / Geçmiş geçişiyle okunabilir. Geçmiş, konuşmanın sırasını ve tam metinleri gösteren kaydırılabilir bir paneldir; yeni bilgi, yorum veya doğru/yanlış işareti üretmez. İlk cevap kaydedilene kadar geçiş görünmez. Geçmiş açılınca soru listesi ve seçili konu durumu korunur; görüşme odasına yeniden girildiğinde Sorular açılır. Düğmeler en az 48 birimdir; küçük yatay telefon ve gerçek dokunma davranışı ayrıca sınanır.

## CCTV video oynatıcısının yerleşimi (24 Eylül 2026)

Klip açıldığında oynatıcı tabletin iç ekranını kaplar; arşiv başlığı ve döküm görünümü arka planda kalır. Ayrı kamera başlığı kaldırılır: kamera adı ve REC görüntü üstünde, Döküme dön sağ üstte, filigranı örten Geç sağ altta durur. Alt şeritte yalnız saatli kayıt açıklaması ve Oynat/Duraklat, Kare ileri, Başa al kontrolleri bulunur. Video kırpılmadan 16:9 oranında mümkün olan en geniş alana oturur; sahnedeki kişiler ve kanıt olabilecek ayrıntılar kesilmez. Bu yerleşim farklı yatay telefon oranlarında ve gerçek dokunmayla ayrıca doğrulanacaktır.

## 25 Eylül 2026 — Gerçek resmî kurum adı kullanılmaz

**Karar (kullanıcı):** Oyun içinde **hiçbir yerde** gerçek resmî kurum, kuruluş veya mevzuat adı kullanılmaz. Kurum kurgusaldır: **bube Departman / BDS**. Kurumsal gönderici "ilgili birim", "kurumsal değerlendirme" gibi genel ifadelerle anılır.

**Metin durumu:** `tr.json`'daki 577 anahtarın hiçbirinde gerçek kurum adı yok; kural zaten uygulanıyordu. `LocaleRules` artık yasaklı ad listesini her koşumda denetliyor (`ValidationReportTests.RealInstitutionNames_AreRejected`). Liste tam sözcük eşleşmesine bakar, böylece "birim", "müdür", "kurumsal" gibi genel sözcükler yanlış alarm üretmez.

**Açık ihlal — görsel:** `Assets/Bube/Resources/Bube/DeskReference.png` görselinin **içine çizilmiş** hâlde:

1. Üst şerit: `İSTANBUL EMNİYET MÜDÜRLÜĞÜ` / `CİNAYET BÜRO AMİRLİĞİ` / `GERÇEKLER ŞEHRİ KORUR` + "POLİS" yazılı gerçek polis armasına benzer rozet.
2. Masadaki terminal ekranı: `EMNİYET SİSTEMİ` + aynı arma.
3. Dosya kapağı: aynı arma.

`Desk()` üst %11'e opak bir şerit çizip `desk.brandLocation` yazdığı için **1. madde yalnız masa ekranında gizleniyor**; giriş klasörünün kaydığı sahnede ve `CaseOffer()` ekranında ham görsel açıkta. 2. ve 3. maddeler her ekranda görünür.

**Çözüm görsel işidir** — metin denetimi bunu yakalayamaz. `DeskV2.png` depoda mevcut ve tertemiz (metin ve arma yok), ama kompozisyonu farklı: masa nesnelerinin yerleri `Desk()` içindeki yüzdelik `Hotspot` koordinatlarıyla eşleşmez.

## 25 Eylül 2026 — Vaka teklifi tam ekran değil, masadaki evrak

**Karar (kullanıcı):** Dosyayı sunan ayrı tam ekran (`CaseOffer()`) kaldırıldı. Masa zaten arkada duruyordu; dosya artık **gelen evrak tepsisinde** bulunur. Rozet kırmızı yanıp söner, oyuncu tepsiye kendisi dokunur, dosyanın önizlemesini okur ve kabul eder.

**Neden doğru:** Değişmeyen oynanış kuralıyla birebir örtüşüyor — oyun oyuncunun önüne ekran koymuyor, kaynağı **erişilebilir yapıyor**; fark etmek ve açmak oyuncunun işi. Ayrıca tam ekran teklif, `DeskReference.png`'nin ham hâlini üst şeridiyle birlikte gösteren tek yerdi.

**Uygulama:**
- `CaseOffer()` tamamen kaldırıldı. `Home()` "Yeni Oyun", `RestartPage()` onayı ve `OpenAssignment()` artık doğrudan `Desk()`'e gider.
- `Desk()` kabul edilmemiş vakada **yalnız tepsi ve ana ekran** kısayolunu açar; dosya, görüşme ve terminal kapalıdır.
- `InboxPage()` kabul edilmemiş vakayı en üstteki okunmamış evrak olarak listeler; sağ sütunda `offer.subtitle` + `offer.summary` önizlemesi ve `offer.accept` düğmesi vardır. **Yeni metin anahtarı gerekmedi.**
- Gelen evrak rozeti kabul edilmemiş dosyayı da sayar ve `schedule.Execute(...).Every(520)` ile yanıp söner. Zamanlayıcı rozetin paneline bağlı olduğu için ekran değişince kendiliğinden durur.
- `FirstDeskArrival()` (klasörün kayarak geldiği açılış) artık ayrı bir masa görseli çizmiyor; `Desk()`'i arka plan olarak kullanıyor. Kaplayan gölge animasyon boyunca dokunmaları tutuyor.

**Kurum adı üzerindeki etkisi:** `DeskReference.png` artık yalnız `Desk()` içinden yükleniyor ve orası üst %11'e opak şerit çizdiği için **"İSTANBUL EMNİYET MÜDÜRLÜĞÜ" şeridi hiçbir ekranda görünmüyor.** Ama görselin içindeki **terminal ekranındaki `EMNİYET SİSTEMİ` yazısı ve armalar hâlâ duruyor** — onlar örtülü değil. Görsel yine de yenilenmeli; bu değişiklik ihlali küçülttü, bitirmedi.

**Doğrulama:** `Assets/Bube/Tests/PlayMode/CaseOfferFlowTests.cs` — başsız Play Mode'da akış gerçekten koşturuldu: kabul edilmeden masada yalnız tepsi açık, tepside önizleme okunuyor, kabul düğmesine basılınca dosya kabul ediliyor ve masa tamamen açılıyor. `CaseOffer()`'ın geri gelmediği de sabitlendi.

## 25 Eylül 2026 — Tek yazı ölçeği

**Sorun (kullanıcı):** "Oyun içerisindeki yazıların font eşitsizliği var, hiçbiri aynı değil."

**Ölçüm:** Yazı **tipi** zaten tekti — her şey kök öğeden IBM Plex Mono'yu miras alıyor. Tutarsızlık **puntodaydı**: 12'den 76'ya **yirmi iki ayrı değer**, çoğu birbirinden bir punto farkla (14/15/16/17, 20/21/22/23/24, 26/27/28/29/31), ekrandan ekrana rastgele seçilmişti.

**Karar:** Dokuz basamaklı tek ölçek — `13, 15, 17, 19, 21, 24, 28, 40, 76`. Eşit uzaklıkta iki basamak varsa **büyüğü** seçilir; telefonda okunaklılık sıkışıklıktan değerlidir.

**Uygulama — çağrı yerlerini elle düzeltmek yerine tek kapı:** `Typography.Snap` ölçek dışı her puntoyu en yakın basamağa oturtur ve `Text(...)`/`Button(...)` yardımcıları boyutu ondan geçirir. Böylece yarın yazılan `Text(parent,"...",Ink,18)` de kendiliğinden uyar; ölçek dışı bir punto ekrana **ulaşamaz**. Doğrudan `style.fontSize=` atayan 51 yer de `Snap`'ten geçirildi.

**Doğrulama:** `TypographyTests` — `Snap` eşgüçlü ve hep ölçeğe oturuyor, eşitlikte büyüğü seçiyor, ve `BubeApp.cs` içinde ölçeği atlayan çıplak punto kalmadığı kaynak taramasıyla sabitlendi.

## 25 Eylül 2026 — Kurgusal departman adı; arma sorun değil

**Kullanıcı kararları:**
1. Terminaldeki "EMNİYET SİSTEMİ" **değişmeli** — dedektifin bağlı olduğu departmanın **uydurma** adı yazılmalı.
2. **Arma sorun değil:** gerçek bir rozet değil, uydurma görünüyor. Kurum adı yasağı armayı kapsamaz.

**Önerilen kurgusal ad:** Kurum **bube POLİS (BDS)**, departman **3. SORUŞTURMA MASASI**, terminal yazısı **"BDS KAYIT SİSTEMİ"**. Üst şeritteki "bube POLİS / İSTANBUL · BEŞİKTAŞ" olduğu gibi kalır.

**Durum:** Bu yazı `DeskReference.png`'nin **içine gömülü**, kodda değil; ancak yeni görselle değişir.

## 25 Eylül 2026 — Masaya dosya bırakılışı sinematik video oldu

**Kullanıcı Gemini ile `case_animation.mov` üretti (1920×1080, 10 sn).** Dosyanın masaya bırakılışı artık elle çizilmiş kayan klasör animasyonu değil, bu video.

**Uygulama:**
- Video H.264 mp4'e çevrilip `Assets/StreamingAssets/Bube/case001_arrival.mp4` olarak kondu (Android `.mov`'u güvenilir oynatmaz). `config.json` → `deskArrivalVideo`.
- **Üretici filigranı "Geç" düğmesiyle örtülür**, dünya sinematiğindeki gibi. Filigran yeri artık veriden gelir: `CornerMark { x, y, w, h }`, filmin kendi karesine **oranlı** (0..1). `PositionIntroSkip` 1280×720'ye sabitlenmek yerine filmin ekrandaki gerçek dikdörtgenini hesaplar, böylece her video kendi filigran yerini söyleyebilir ve telefonun eni ne olursa olsun düğme doğru yere oturur. `world01` değerleri eskisiyle piksel eşdeğer.
- Video oynatılamazsa elle çizilmiş animasyon devreye girer; oyun bu andan hiçbir koşulda yoksun kalmaz. "Geç" ile videonun bitişi aynı yere gelir, bayrak ikinci çağrıyı yutar.

**Geçiş: karart, sonra göz aç.** Karartma **videonun kendi içinde** (kullanıcı ekledi); masa da siyahtan 1,25 saniyede açılır. İki görüntü birbirine çarpmaz. Açılma boyunca kaplayan gölge dokunmaları tutar, oyuncu göremediği bir şeye basamaz.

**(Geri alındı — 25 Eylül 2026)** Bir ara masa arka planı videonun son karesi yapılmıştı. Kullanıcı **eski masa görselini geri istedi**; `Desk()` yine `DeskReference.png` yüklüyor ve künye şeridi üstte. Videonun son karesiyle masanın birebir aynı olmaması artık sorun değil, çünkü aradaki geçiş karartmayla yapılıyor.

**Kurum adı bitti — yerine hiçbir şey konmadı.** Terminaldeki "EMNİYET SİSTEMİ" `DeskReference.png`'den silindi (ekranın kendi arka planıyla kapatıldı). Kısa süre yerine kurgusal bir ad ("BDS KAYIT SİSTEMİ") koddan yazıldı, ama **kullanıcı kararıyla o da kaldırıldı**: ekranda yalnız "CCTV ARŞİVİ" kalıyor ve terminalin ne olduğu zaten anlaşılıyor. Kurum adı yazmamak, kurgusal kurum adı yazmaktan daha temiz — yazılmayan ad ihlal edemez.

Üst künye şeridi %97 saydamdı ve altındaki gömülü kurum yazısı hayalet gibi sızıyordu; şerit tamamen opak yapıldı.

**Terminal ekranındaki arma da silindi** (kullanıcı kararı): ekranda yalnız "CCTV ARŞİVİ" kutusu kalıyor. Kenarlardan interpolasyon denendi ama armanın ucu seçilen dikdörtgenin dışına taştığı için dikey izler bıraktı; ekranın o bölgesi zaten neredeyse düz olduğundan çevresinden örneklenen tek renkle doldurmak temiz sonuç verdi. **Dosya kapağındaki arma kalıyor** — kullanıcı kararı, uydurma görünüyor.

**(Çözüldü — 25 Eylül 2026)** Kullanıcı videoyu yeniden üretti (`case_fix_animation.mov`, 2,83 sn): terminalde yalnız "CCTV ARŞİVİ" var, arma ve kurum adı yok, karartma da videonun içinde. Koddaki karartma kaldırıldı — aynı işi iki kez yapmanın anlamı yok. Filigran yine sağ altta (0,906 / 0,833) ve "Geç" düğmesiyle örtülüyor; `CornerMark` veriden geldiği için tek satır değişti.

**Durum `[~]`:** Testler geçiyor ama **Play Mode'da gözle doğrulanmadı** — karartma/açılma geçişinin akıcılığı görülmeli.

## Görüşme ekranı düzeltmeleri (25 Eylül 2026)

**Yinelenen konu başlığı kaldırıldı.** Soru listesinin üstünde sabit bir konu şeridi vardı; hemen altındaki açık grubun başlığı aynı metni yazıyordu, yani "OLAY GÜNÜ · 1" iki kez görünüyordu. Şerit kaldırıldı; grup başlıkları zaten hem etiket hem de katlama düğmesi.

**Görüşülmüş kişide düğme "GÖRÜŞMEYE DÖN" diyor.** Kart "● Görüşüldü" derken düğmenin "GÖRÜŞMEYE BAŞLA" demesi, yeni bir görüşme açılacağı izlenimi veriyordu. Yeni anahtar `interview.resume`. Düğme **devre dışı bırakılmadı**: oyuncu geçmişi okumak ya da sonradan açılan soruları sormak için dönebilmeli. Yeni soru kalmadığında görüşme ekranı zaten `interview.noNewInfo` gösteriyor — bu, oyuncuya sıradaki adımı söylemeden durumu bildiren doğru yer.

**Dar eylem sütununda düğme metni sarıyor.** Kartın eylem sütunu %30 genişlikte; "İfade alınmasını iste" tek satıra sığmayıp kırpılıyordu. `FitActionButton` son düğmeyi sarmalı yapıyor ve yazı basamağını düşürüyor.

## Kaynak seçici kişiye özgü oldu (25 Eylül 2026)

**Sorun.** Bir kaynağı karşımızdaki kişiye gösterirken seçici, o kişi dışındaki *herkesin bütün görüşme dökümünü* ve bütün kamera olaylarını listeliyordu. Elif'in karşısında Mert'e sorduğumuz her soru görünüyordu; oysa Hasan'ın Mert hakkındaki bir ifadesinin Elif'e sorulmasının anlamı yok.

**Çözüm.** `Question` ve `CctvEvent` artık `aboutPersonIds` taşıyor: o ifade/kayıt kimden söz ediyor. Seçici yalnız karşısındaki kişiyle ilgili kaynakları listeliyor. Dosya #001'de liste kişi başına 27–29 satırdan 9–13 satıra indi.

**Bu bir doğruluk süzgeci değildir.** Her kişide hâlâ birden çok ilgili kaynak kalıyor; hangisinin belirleyici olduğunu oyuncu buluyor. Değişmeyen oynanış kuralı korunuyor.

**Etiketsiz kaynak herkese açıktır.** Sinyal zayıflaması, kesinti ve kayıt boşluğu kimseden söz etmeyen olgulardır; etiketsiz bırakıldı ve herkese gösteriliyor. Eski/yeni vaka verisi etiketsizken de çalışır.

**Yeni doğrulama kuralı.** Bir sorunun `presentedSourceIds` hedefi, soruyu soracağımız kişiyle etiketlenmemişse o kaynak listede hiç görünmez ve soru yanıtlanamaz olur — vaka çözülemez hale gelir. `CaseRules` bunu artık yakalıyor.

## Soru metinleri nötrleştirildi (25 Eylül 2026)

**Kural.** Kaynak sunulan bir sorunun metni, çelişkiyi oyuncu yerine kurmamalı. Soru yalnız *sorar*; çelişki, oyuncunun seçtiği kaynak öne sürülünce yanıtta ortaya çıkar.

Dört soru bu kuralı çiğniyordu:

| Soru | Eski (oyunun kurduğu) | Yeni (oyuncunun kuracağı) |
|---|---|---|
| `elif_follow.footage` | "Binaya hiç gitmediğinizi söylediniz. Kayıttaki kadın siz misiniz?" | "O gün öğle saatlerinde neredeydiniz?" |
| `hasan_follow.gap` | "Kamera kaydının olmadığı saatlerde neredeydiniz?" | "Öğleden sonra bir sularında neredeydiniz?" |
| `hasan_follow.mertStatement` | "Mert, kapıda kaldığında ona yardım ettiğinizi ve anahtarı nereden aldığını gördüğünüzü söyledi. Ne hatırlıyorsunuz?" | "Mert'le kapıda karşılaştığınız günü anlatır mısınız?" |
| `hasan_follow.sale` | "Bilgisayarın teslim fişinde adınız neden var?" | "Kaybolan eşyalardan herhangi biri sizin elinize geçti mi?" |

Yanıtlar değişmedi: doğru kaynak öne sürülünce aynı itiraf/savunma geliyor. Değişen tek şey, oyuncunun o kaynağı kendisinin seçmek zorunda olması.

**Doğrulama.** `CaseRules` artık sorunun metniyle kaynağın metni arasında ortak üç sözcüklük dizi arıyor ve bulursa not düşüyor. Eşik üçtür: dört sözcük, yakalamak istediğimiz `hasan_follow.mertStatement` ihlalini kaçırıyordu. Sezgisel bir kontroldür, o yüzden sorun değil **not** olarak raporlanır. Mevcut metinlerde yanlış alarm yok.

## Görüşmelerin ilk sorusu artık zorunlu değil (25 Eylül 2026)

**Ölçüm.** Dosya #001 rastgele 1500 oynanışta benzetildi. Sonuç: 33 içerik adımı, **çıkmaz yok**, ama her görüşmenin ilk hamlesi tek kapıydı — Mert'te yalnız `mert.day`, Elif'te `elif.relationship`, Hasan'da `hasan.sighting`. "Devam et, devam et" hissinin ölçülebilir kaynağı buydu.

**Değişiklik.** Metni önceki cevabı gerçekten varsaymayan dokuz sorunun `requiresAsked` kapısı kaldırıldı (`mert.key`, `mert.neighbor`, `elif.visit`, `elif.remaining`, `elif.contact`, `hasan.where`, `hasan.help`, `mert_follow.neighbor`, `hasan_follow.sale`). Varsayanlarda kapı **duruyor**: "Kapıyı çıkarken kilitlediğinizden emin misiniz?" çıktığını öğrenmeden sorulamaz.

İki kapı da doğru hedefe bağlandı: `elif.avoid` artık `elif.contact`'a bağlı (konuşmadığını orada söylüyor, `elif.visit`'te değil), `elif_follow.call` üç halkalı zincir yerine doğrudan `elif_follow.footage`'a.

**Sonuç.** Ortalama seçenek 4,4 → 5,0. Her oynanışta zorunlu kalan tek hamle `report`'u okumak — dosyayı açmadan soruşturma başlamaz, bu doğru.

**Açık kalan koridor.** `camera` düğümü yalnız `hasan.camera` sorulunca açılıyor; ikinci perdeye tek giriş var. Genişletmek yeni diyalog yazmayı gerektirir, tasarım kararıdır.

## Yem kaynaklar: oyun oyuncuyla oynar (25 Eylül 2026)

**Neden.** Sonuçta tek bir doğru var. O yüzden yanlış okumalar *inandırıcı* olmalı; oyuncu "acaba öyle miydi" diyebilmeli. Eskiden ilgisiz bir kaynağı öne sürmek boş bir hamleydi: "Bu kayıtla ilgili ne söylememi istiyorsunuz?" Ne bilgi, ne şüphe.

**Mekanizma.** `Question.decoyAnswers` — kaynak kimliği → yanıt anahtarı. Yem kaynak o kişiyle gerçekten ilgilidir ve öne sürmesi mantıklıdır, ama soruyu **kapatmaz**: `Ask` çağrılmaz, soru açık kalır, döküme girmez. Karşılığında baştan savma bir cümle değil, gerçek bir yanıt gelir — doğru ama yanıltıcı.

`presentedSourceIds` "bu mesele biter" demektir; yem oraya konmaz. Yem oraya konsaydı yanlış yola sapan oyuncu vakayı çözmüş sayılırdı. `Case001Rules` bunu zaten iddia ediyordu ve ilk denememi haklı olarak düşürdü.

**Yazılan yedi yem.** Elif'e kayıt boşluğunu, Mert'in anahtar ifadesini ya da Hasan'ın görgüsünü sunmak; Hasan'a Elif'in kamera geçişini, Mert'in komşuluk ifadesini, kayıt boşluğunu ya da Elif'in saksı ifadesini sunmak. Hepsi doğru söyler, hiçbiri itiraf etmez, her biri başka bir yöne bakar — Hasan'ınkiler oyuncuyu Elif'e geri iter.

**Üç doğrulama kuralı.** Yem aynı anda çözücü kaynak olamaz; kişinin kendi ifadesi olamaz; o kişiye görünür olmalı (`aboutPersonIds`). Üçüncüsü hemen iş gördü: `camera#elif_in` Hasan'a kapalıydı, oysa o geçişi gördüğünü iddia eden Hasan'dır — etiket düzeltildi.

**Yan bulgu (hata).** `interview.unrelatedSource` ekrana `[Bu kayıtla ilgili ne söylememi istiyorsunuz?]` diye köşeli parantezle düşüyordu: `InterviewPage` yanıt **anahtarı** bekler, oysa çevrilmiş metin geçiliyordu. Düzeltildi.

## Yemler bütün kaynak-sunulan sorulara yayıldı (25 Eylül 2026)

Dört soru kaynak sunduruyor: `elif_follow.footage`, `hasan_follow.gap`, `hasan_follow.sale`, `hasan_follow.mertStatement`. Yem sayısı 7'den **33'e** çıktı.

**Kapsama.** Seçicide o kişiye görünen kaynakların 9–10'u artık gerçek yanıt veriyor (toplam 13–14). Boş kalanlar yalnız `report` ve sinyal satırları (`weak`, `lost`, `restored`) — bunlara ayrı cümle yazmak dört sorunun her birinde neredeyse aynı metni tekrarlamak olurdu.

**Kişiye ait savuşturma.** Onun yerine `Node.deflectAnswerKey` eklendi: yemi yazılmamış bir kaynak sunulduğunda genel "Bu kayıtla ilgili ne söylememi istiyorsunuz?" yerine kişinin kendi sesi çıkıyor. Hasan: "Bunun benimle ilgisini kurmuyorum. Başka bir şey soracaksanız sorun." Elif: "Bununla benim aramda bir bağ kuruyorsanız o bağı siz söyleyin. Ben göremiyorum."

**Yeni kural.** İki yem aynı yanıt anahtarını paylaşamaz; paylaşırsa cümle ikisinden biri için kaçınılmaz olarak yersiz düşer. Bu kural gerçek bir kopyala-yapıştır hatasından doğdu: `hasan_follow.gap`'in iki ayrı yemi aynı anahtara bakıyordu.

## Kaynak seçici telefona göre sadeleşti (25 Eylül 2026)

**Üst üste binen sekmeler (hata).** "Sorular / Geçmiş" şeridi esnek kutuda küçülüp yüksekliğini yitiriyordu; düğmeler kabın dışına taşıp altındaki listenin üstüne biniyordu. `flexShrink=0` ve en az dokunma hedefi kadar yükseklik verildi.

**Öne sürülmesi anlamsız kaynaklar (`notPresentable`).** Listede vakanın kendi olay raporu ve sinyal telemetrisi (`SİNYAL ZAYIFLADI`, `SİNYAL KESİLDİ`, `SİNYAL GERİ GELDİ`) duruyordu. Bunları bir tanığa uzatmanın anlamı yok. Artık görüşme seçicisinde gizleniyorlar; **sonuç ekranında gösterilmeye devam ediyorlar**, orada gerekçe olarak gösterilebilirler.

Kayıt boşluğu (`gap`) listede **kaldı**: o bir telemetri satırı değil, vakanın olgusu.

Sonuç: dört kaynak-sunulan sorunun hepsinde kapsama **%100** — seçicide görünen her kaynağın artık gerçek bir yanıtı var, kişiye ait savuşturma cümlesi de ender bir güvenlik ağı olarak duruyor.

**Arama alanı kaldırıldı.** Telefonda klavye ekranın yarısını kaplıyordu ve liste zaten kişiye göre süzülüp 9-10 satıra indi. Tür sekmeleri (Tümü / Belgeler / İfadeler / CCTV) kaldı.

**İki yeni kural.** Ne belirleyici kaynak ne de yem, `notPresentable` olabilir — olursa görüşmede hiç öne sürülemez, yani yanıt metni oyunda hiç çıkmaz. Kural hemen iş gördü: `hasan_follow.gap`'in `camera#lost` yemi bu yüzden silindi.

## Görüşmede geri dönüş (25 Eylül 2026)

Bir soruyu seçtikten sonra vazgeçmenin tek yolu **görüşmeyi tümden bitirmekti**. Hem kaynak seçicisine hem de kaynak gerektirmeyen sorunun "dinle" adımına "‹ Vazgeç · Sorulara dön" eklendi; ikisi de soru listesine döner, hiçbir şey sorulmuş sayılmaz.

## Yem metinleri: şüpheyi oyuncuya bırak (25 Eylül 2026)

İlk yem metinleri "bunu bana neden gösteriyorsunuz", "ne alakası var" kalıbına sıkışmıştı; oyuncu bunu yanlış bir yol denediğinin işareti diye okuyordu — yani sistem oyuncuya cevabı söylüyordu. Otuz iki metin yeniden yazıldı. Yeni ilke: **yem, dünyaya yeni sert olgu eklemez, yorumu ağırlaştırır.** Masum kişi kendini beceriksizce savunur, bilgi sahibi olduğunu itiraf eder, kendi aleyhine konuşur ("anahtarı geri vermedim, vermek de istemedim"); asıl fail sakin ve düz kalır. Böylece yanlış kaynağı sunmak bir uyarı değil, yeni bir şüphe doğurur.

İki tuzak kapatıldı: yem metinleri bir kaynağın içeriğine aykırı olgu uyduramaz (üç metin `recovery` raporunu "eşya listesi" sanıyordu; rapor tek bir dizüstü ve Hasan adına bir teslim fişidir) ve iki yem aynı metni paylaşamaz — ikincisini doğrulayıcı zaten anahtar düzeyinde yakalıyor.

## Yanıtlanan soru kapanır; kaynak kartı adlandırması (25 Eylül 2026)

Bir soruyu birden çok belirleyici kaynak kapatabiliyordu (`presentedSourceIds`), bu yüzden soru listede "YENİ KAYITLA …" önekiyle tekrar duruyordu. Oyuncu bunu "bir şey eksik kaldı" diye okuyordu, oysa kişi cevabını çoktan vermişti. **Karar:** yanıtlanan soru listeden çıkar — `CanAskQuestion` artık `asked` içindeki soruyu yeniden açmıyor, ikinci belirleyici kaynak da geri çevriliyor. `interview.repeatPrefix` anahtarı kaldırıldı. Yem denemeleri bundan etkilenmez: yem `Ask` çağırmadığı için soru kapanmaz, oyuncu yanlış kaynakları istediği kadar deneyebilir.

`Case001Rules` eski davranışı şart koşuyordu (dört iddia); kurallar yeni karara çevrildi — bir kaynakla kapanma, ikinci kaynağın reddi, tek döküm satırı.

Etiketler anlaşılmıyordu: "KAYNAK KARTI" → **"ELİNDEKİ KAYIT"**, "KAYNAK KARTINI AÇ" → kaynak seçerken **"SEÇTİĞİN KAYDI OKU"**, yanıt ekranında **"ÖNE SÜRDÜĞÜN KAYDI OKU"** (yeni `interview.openPresented`). İkisi de aynı şeyi yapıyor: öne sürülecek/sürülmüş kaydın metnini görüşmeden çıkmadan okutur.

## Davranış satırı — dört kaynak sorusunda deneme (25 Eylül 2026)

Yem metinleri "fail sakin kalır, masum beceriksizce savunur" üzerine kuruluydu, ama ekranda yüz, ses ya da duraksama yok; ton tek başına taşımıyordu. **Deneme:** yanıtın altına dedektifin *gördüğü* davranış satırı — "Cümlenin ortasında durdu, baştan başladı." Kapsam dört kaynak sorusu (`elif_follow.footage`, `hasan_follow.gap`, `hasan_follow.sale`, `hasan_follow.mertStatement`): 38 satır, kapatan yanıtlar ve yemler dâhil.

Kural: **gözlem, yorum değil.** Satır kişinin gizli durumunu söylemez, yalan/çelişki etiketi koymaz; anlamı oyuncu kurar. Ayrıca satırlar **teşhis edilebilir olmamalı** — Hasan kendisini en çok bağlayan raporda sakin, zararsız bir kayıtta huzursuz; Elif masum olduğu hâlde titriyor. Davranışın "yalan söylüyor" sinyaline dönüşmemesi bu dengeye bağlı.

Şema değişmedi: metin `answerKey + ".demeanor"` sözleşmesiyle bulunur, `Locale.Has` ile yoksa satır hiç çizilmez. `LocaleRules` iki koşulu zorluyor — yorum sözcükleri (yalan, gizliyor, tedirginliği, masum…) yasak, ve satır 16 sözcüğü geçemez, yoksa yanıtı gölgede bırakır. Kuralın gerçekten ateşlendiği, metne bilerek "gizliyor" konarak görüldü.

Yayma kararı oyuncuya ait: beğenilirse öteki sorulara ve öteki vakalara aynı sözleşmeyle eklenir.

## Kaynak satırı yanıtı gösterir, soruyu değil (25 Eylül 2026)

Kaynak seçicideki görüşme satırları `personNameKey + " · " + promptKey` ile etiketleniyordu, yani kişiye **sorulan soru** yazıyordu. Liste bu yüzden "şimdi soracağım sorular" gibi okunuyor ve karşındaki kişiyle ilgisi görünmüyordu. Oysa öne sürülen şey kişinin **verdiği yanıttır**; satır artık `answerKey` metnini tırnak içinde gösteriyor.

Süzgecin kendisi doğruydu: ekrandaki üç satır da (`elif_follow.keyPlace`, `mert_follow.spare`, `mert_follow.known`) `aboutPersonIds` içinde `hasan` taşıyor — üçü de saksıdaki anahtarı ve Hasan'ın onu görmesini konuşuyor. Görünmeyen şey ilgi değil, ilginin *sebebiydi*; yanıt metni bunu kendiliğinden söylüyor.

Kırpma sınırı 66'dan 110 karaktere çıkarıldı: satırlar zaten iki satıra sarıyor, 66 karakter yanıtın anlamlı yerini (ör. "Hasan da kapıda kaldığım gün…") kesiyordu.

## Adı geçtiyse cevap verme hakkı doğar (25 Eylül 2026)

**Yeni kanon kural, bütün karakterler ve bütün vakalar için:** bir kaydı karşındaki kişiye ancak **adı orada geçiyorsa** öne sürebilirsin. Geçmiyorsa o kayıt onu ilgilendirmez. Kural metinden türetilir (`Investigation.MentionsPerson`), elle etiketlemeye bağlı değildir; yeni vakalarda hiçbir şey yapmadan işler.

`aboutPersonIds` artık **ek**tir, üst geçersiz değil: metin kuralını silmez, üstüne ekler. Tek işi, kaydın kişiden *adını anmadan* söz ettiği yerleri işaretlemektir — "11.48 — Kadın şahıs binaya girdi." Elif'i anlatır ama adını anmaz; Hasan'ın gördüğü "kadın" da öyle. Kayıt boşluğu (`camera#gap`) kimseyi anmaz ama herkesin o saatini ilgilendirir, bu yüzden üçü de etiketlidir.

Kural artık **belgelere de** işliyor. Belgeler eskiden hiç süzülmüyordu; doğrulayıcı da belge yemlerini atlıyordu (`mark < 0` olunca `continue`). İkisi de kapatıldı.

Eski elle etiketleme çoğu yerde yanlıştı: kişinin **kendi** ifadesini de işaretliyordu, oysa kendi ifadesi zaten listede görünmez. Otuz soru etiketi kaldırıldı, üçü kaldı (`hasan.sighting`, `hasan.time`, `hasan.recognition` — Hasan hep "kadın" der, "Elif" demez).

**Ölçülen etki** (kişi başı öne sürülebilir kaynak): Elif 10 → 10, Hasan 9 → 8, Mert 11 → 20. Mert'inki büyüdü çünkü herkes ondan söz ediyor; kuralın doğrudan sonucu.

**Kuralın bedeli, ödendi:** dört yem kaldırıldı. `elif_follow.footage → recovery` (eşya raporu Elif'in adını anmaz) ve `elif_follow.keyPlace`'i Hasan'a sunan üç yem (Elif hiçbir yerde Hasan'ın adını anmaz, "benden sonra oraya kimin baktığını bilmiyorum" der). Bunları etiketle geri açmak kuralı sessizce delmek olurdu; istenirse tek tek etiketlenerek geri gelebilirler.

## Dosya ekranı: tek kaydırma, dokunulur sayfalar (25 Eylül 2026)

Dosya telefonda karmaşıktı. Dört ayrı sebep vardı, dördü de düzeltildi:

1. **İç içe kaydırma.** Metin ile görsel yan yana iki sütundu (`flexBasis=0`), yani rapor metni sayfanın yarı genişliğine düşüyor ve kendi kaydırma çubuğunu kazanıyordu — sayfanın içinde ikinci bir kaydırma alanı. Artık tek sütun, tek kaydırma; görsel metnin akışında, tam genişlikte.
2. **"1 / 1" sayacı ve Önceki/Sonraki.** Tek sayfalık bölümlerde bile duruyordu, ve istenen sayfaya varmak için art arda dokunmak gerekiyordu. Sayfa birden çoksa adları doğrudan dokunulur (yatay şerit); tekse alt şerit hiç çizilmez.
3. **Kendi içinde kayan sekme şeridi.** Dar sütuna sekiz sekme sığmadığı için bir kısmı ekran dışındaydı; oraya varmak için önce şeridi kaydırmak gerekiyordu. Şerit genişledi, kaydırma kalktı, sekmeler yüksekliği paylaşıyor — hepsi görünür ve hepsi en az 48 birim.
4. **Sürdürülebilirlik.** Sekme biçimi dört ayrı yerde kopyalanmıştı (her biri yedi satır). `FileTab` yardımcısına toplandı; yeni sekme eklemek artık tek satır.

**Açık kalan:** "DOSYADA ARA" hâlâ yazmayı gerektiriyor. Görüşmedeki arama alanı telefonda klavye ekranın yarısını kapattığı için kaldırılmıştı; dosyadaki arama da aynı gerekçeyle gözden geçirilmeli, ama yerine ne konacağı (kişiye/türe göre dokunulur süzgeç) ayrı bir karar.

## "Dosyada ara" yerine dokunulur süzgeç (25 Eylül 2026)

Arama yazı alanıydı: telefonda klavye ekranın yarısını kaplıyor, üstelik hangi sözcüğü arayacağını bilmek oyuncunun işi değil — bilmediğini arayamaz. Yerine iki dokunulur eksen kondu, sekme adı da **"DOSYADA GEZİN"** oldu:

- **Tür:** Tümü · Belgeler · İfadeler · Kamera kaydı (görüşme kaynak seçicisindeki sekmelerin aynısı).
- **Kişi:** Herkes · Mert · Elif · Hasan. Bu liste vakaya elle yazılmaz; görüşme düğümlerinden türer. Eşleşme de elle etiketlenmez: **"adı geçtiyse" kuralının aynısını** (`Investigation.MentionsPerson`) kullanır, yani yeni vakalarda kendiliğinden işler ve oyunun geri kalanıyla aynı mantığı konuşur.

Süzgeç kalkınca ekran, kayda geçmiş bütün satırların gezilebilir listesi hâline geliyor — oyuncu ne aradığını bilmeden de dosyayı tarayabiliyor.

**Sızıntı kapısı korundu.** `CaseSearch` ikiye ayrıldı: `Lines` okunmuş kaynakların satırlarını toplar (tek kapı), `Find` onun üstünde metin süzer. Okunmamış kaynağın aramaya sızmadığını doğrulayan `Case001Rules` iddiaları `Find` üzerinden aynı kapıyı denetlediği için gezinme listesi de kendiliğinden kapsanıyor.

Ölü anahtar `search.enter` ("Aramak için en az iki karakter yaz") kaldırıldı.


## Kayıt göçü: eski kayıt yükseltilir, yeni kayıt silinmez (25 Eylül 2026)

Kaydın sürümü uymazsa dosya **sessizce atılıyordu**: oyuncu "Devam Et"e basıyor, vaka boş açılıyor, hiçbir şey söylenmiyor. Şema #002 için bir alan kazandığı anda bu, güncelleme yiyen herkesin ilerlemesini silmek anlamına gelirdi.

Kural artık şu: **kaydın sürümü bir sonuca bağlanır, sessizlik yok.** `SaveMigration.Migrate` beş sonuçtan birini verir — `Loaded`, `Migrated`, `FromFuture`, `OtherCase`, `Fresh` — ve `Investigation` bunu `StateOutcome` / `CareerOutcome` olarak dışarı verir.

- **Daha eski kayıt atılmaz, yükseltilir.** Basamaklar sırayla koşar, böylece çok eski bir kayıt da bugüne tırmanır. Yeni alan eklendiğinde `Migrate`'e tek bir `if(save.version<2)` basamağı yazılır.
- **Daha yeni kayıt çevrilemez ama silinmez.** Oyuncu eski sürüme düşmüş olabilir; dosya `<yol>.newer` olarak yana kaldırılır (bu aynı zamanda `Save()`'in üzerine yazmasını engeller) ve ekranda `save.fromFuture` satırı görünür. Kayıp varsa oyuncu bunu görerek öğrenir.
- **Güven sıfırlanmaz.** Göç edilen kariyer kaydı `departmentTrust` ve rütbesini korur; sıfırlama yalnız gerçekten yeni bir kariyerde olur.

`UnknownVersionSave_IsSilentlyDiscarded_KnownDebt` kaldırıldı — sabitlediği davranışın yanlış olduğunu biliyorduk. Yerine dört test: eski ilerleme korunuyor mu, eski kariyer güvenini koruyor mu, gelecekten gelen kayıt `FromFuture` deniyor mu, kaydın devralınmama sebebi adlandırılıyor mu.


## KARINE marka kimliği: logo görsel, yazı tipi rol tablosu (25 Eylül 2026)

Ana menüdeki marka bugüne kadar **yazıyla** çiziliyordu (`bube`, 76 punto, glitch efekti). Artık kullanıcının verdiği KARINE logosu kullanılıyor: ağır slab-serif, harflerin içinde kontrollü aşınma, şeffaf arka plan.

**Aşınma yazı tipine uygulanmaz.** Glif başına kırılma ve puntoya göre değişen mürekkep kaybı font dosyasından çıkmaz; bu yüzden logo tek bir görsel varlıktır, oyunun geri kalanı aşınmasız yazı tipleriyle yazılır. Logo yazı tipi **hiçbir yerde** gövde metnine uygulanmaz.

**Tek kaynak, tek oran, tek yoğunluk.** `KarineLogo` ekranlara yalnız **genişlik** seçtirir; yükseklik daima `AspectRatio`'dan türer, yani esnetme mümkün değil. Doku yoğunluğu sabittir, ekrana göre değişmez. Katman yapısı istenen şekildedir:

```
KarineLogo
 ├── LogoBase         — marka harfleri
 └── DistressOverlay  — üstteki doku katmanı
```

Bugün aşınma **temel görselin alfa kanalındadır**; `DistressOverlay` yerinde durur ama `Bube/Art/KarineDistress` konulmadıkça çizmez. İkinci bir doku üst üste binerse okunabilirlik bozulur — kural buydu, o yüzden varsayılanı yok.

**Kullanım:** ana menüde büyük (`Hero`, 380), diğer ekranlarda kompakt başlık (`Header`) — altında çizgiyle, "KARINE ─────". Açık kâğıt üzerindeki ekranlarda (arşiv/vaka seçici) ton koyulaşır; oran ve doku değişmez.

**Yazı tipi rolleri** (`FontSet`): `Heading` Roboto Slab, `Mono` IBM Plex Mono (dosya, terminal, tarih, vaka numarası), `Body` Inter / IBM Plex Sans (açıklama ve düğmeler). Dosya adları tek yerde tanımlı; bir rolün dosyası yoksa mono'ya düşer ve doğrulayıcı bunu **not** olarak yazar. `RobotoSlab-ExtraBold.ttf` ve `Inter-Regular.ttf` klasöre bırakıldığı an oyunun tamamı tek yerden geçiş yapar.

**Bulunan gerçek hata:** Unity varsayılan içe aktarımı (`nPOTScale: 1`) logoyu 2000×667'den **2048×512'ye eziyordu** — yani tam da yasaklanan esnetme, kimse dokunmadan, içe aktarımda oluyordu. Logo `nPOTScale: 0` ile kilitlendi; testler hem varlığın oranını hem de ekranda ölçülen oranı doğruluyor. Aynı ayar projedeki diğer görsellerde hâlâ açık (bkz. Architecture.md).


## Ana menü maketi ve dönen arka plan (25 Eylül 2026)

Kullanıcının verdiği maket ana menünün **tasarım kanonu** oldu; yanındaki 10 saniyelik animasyon (1920×1080, sessiz döngü) arka plan. Animasyonun sol tarafı zaten karartılmış, menü oraya oturuyor.

Soldaki sütun maketin sırası: KARINE logosu → `A DETECTIVE INVESTIGATION GAME` → çizgi → menü. Sözlük tanımı (`karine (n.)`) kullanıcı kararıyla kaldırıldı: ilk ekran kalabalık görünüyordu. Stüdyo imzası (`bubeGames` / `powered by bubeDigital`) sol sütundan çıkıp **sağ alt köşeye** taşındı; videonun o köşesi siyah olduğu için yazı orada okunuyor ve ÇIKIŞ satırıyla çakışmıyor.

**Menü beş satır** — maketteki gibi: DEVAM ET (kayıt varsa, öne çıkan), YENİ KARİYER, AYARLAR, KARİYER, ÇIKIŞ. Satırlar simge sütunu + etiket + (öne çıkanda) ok biçiminde; dokunma hedefi 52 piksel.

**Kaybolan iki giriş taşındı, silinmedi.** Maket beş satır gösterdiği için Arşiv ve Hakkında menüden çıktı: **Arşiv kariyer ekranının içinde** (kapanmış dosyalar zaten kariyer geçmişidir), **Hakkında ayarların içinde**. İkisi de erişilebilir; menü maketle birebir.

**Video açılmazsa menü boş kalmaz:** `errorReceived` gelirse durağan `MainMenuNight` görseline düşülür. Döngü sessizdir — müzik ayrı bir karardır. Masaya geçerken döngü durdurulur, menüye dönünce aynı doku yeniden kullanılır (her `Home()` çağrısı videoyu baştan başlatmaz; `MenuOverlay` ve arşiv ekranları `Home()`'u yeniden çiziyor).

**Simgeler maketten kesildi.** Satır simgeleri font glifiyle (`▣ ▤ ⚙ ▥ ◀`) çizilirken küçük ve cılız duruyordu; artık maketin kendi ikonları (klasör, belge, dişli, grafik, çıkış oku) beş küçük PNG olarak `Bube/Art/Icons/menu_*` altında duruyor. Krem renkle yazılıp satırın tonuyla boyanıyorlar, böylece öne çıkan satırda kendiliğinden koyuya dönüyorlar. Eksik bir simge dosyası doğrulayıcıda hata verir.

Eski `bube` yazı logosu ve `P O L I C E` satırı kalktı; marka artık KARINE.

## KARINE UI/UX Kit bağlayıcı tasarım sistemi oldu (25 Eylül 2026)

Kullanıcının verdiği UI/UX Kit görseli artık **spesifikasyondur**, ilham değil. Görsel depoya `Docs/Reference/UI_KIT.png` olarak alındı; yazılı karşılığı `Docs/UI_KIT.md`.

**Tek kaynak kuruldu.** `KarineTheme` paleti, boşluk ölçeğini, köşe dilini, dokunma hedefini ve devinim sürelerini tutuyor; `KarineUI` kit'in bileşenlerini üretiyor (dört düğme biçimi, ikon düğme, panel, sekme, rozet, modal, bildirim, evrak gezintisi, ilerleme, tooltip, ikon). Ekranların içine renk veya punto yazılmaz.

**Palet kit görselinden okundu.** Yazılı brief ile görselin etiketleri iki yerde çakışıyordu — brief `#EBDCC4` / `#C9836C` / `#C94F4F` diyor, görselin kendi etiketleri `#E8DCC4` / `#C9B38C` / `#E94F4F`. Kit'in kendi §23 maddesi "referans görsel önceliklidir" dediği için **görseldeki değerler** alındı; ayrıca `#E8DCC4` zaten KARINE logosunun rengi, yani görsel kendi içinde tutarlı.

**Diegetic katman bilinçli olarak paletin dışındadır.** Kit §13 dosya/evrak/faks/terminali oyun dünyasının parçası sayar. Ekranlarda 38 kez tekrarlanan kâğıt kremi ve kâğıt üstü mürekkep `KarineTheme.Paper` altında adlandırıldı. Bu bir palet ihlali değil, kit'in istediği ayrımın koda geçmiş hâlidir.

**İkon dili kit'ten kesildi.** On üç ikon (`folder`, `document`, `gear`, `binoculars`, `pin`, `people`, `chart`, `more`, `close`, `alert`, `info`, `nav_prev`, `nav_next`) doğrudan kit görselinden çıkarıldı. Ana menünün makete özel dört ikonu **silindi**: aynı işlev için iki ayrı çizim tutmak kit'in "aynı fonksiyon = aynı ikon" kuralını bozuyordu; menü artık ortak kümeyi kullanıyor.

**Ham renk borcu kilitlendi.** Kit'ten önce yazılmış ekranlarda 156 doğrudan `new Color(...)` kaldı. Hepsini bir oturumda çevirmek soruşturma ekranlarını gözle doğrulanamayacak kadar çok değiştirirdi; bunun yerine doğrulayıcıya sayının **büyümesini** engelleyen bir kilit kondu. Borç ancak aşağı iner.

## Ekranlar kit bileşenlerine taşındı (25 Eylül 2026)

Tema kurulduktan sonra ekranlar tek tek `KarineUI` bileşenlerine bağlandı: dört ayrı yerde elle kurulmuş sekme şeridi `Tabs`e, masadaki iki uyarı `Notification`a, kariyeri sıfırlama ve rapor gönderme onayları `Modal`a, ayarların metin hızı `Radio`ya, kariyer ekranının güven ve vaka sayıları `Meter`/`Counter`a, saat/tarih/damga/sayfa sayacı `Technical`a geçti.

**Rapor gönderme artık onay soruyor.** Kit'in modal örneği birebir bu an için yazılmış ("Gönderdiğin karar geri alınamaz."). Onay yalnız kararı sorar; hangi şüphelinin doğru olduğuna dair hiçbir şey söylemez, yani oyuncu güdümlü soruşturma kuralı bozulmaz.

**Sinematik kontroller kit §7'ye göre kuruldu.** Eskiden filmlerde tek bir "GEÇ" düğmesi vardı. Artık duraklat, ilerleme çubuğu, süre, İLERİ SAR (2× hız) ve GEÇ var; kit'in "bunlar aynı etkileşim değildir" kuralı testle kilitlendi. Bazı filmlerde GEÇ üretici filigranının üstüne oturmak zorunda olduğu için çubuğun dışında kalıyor — işlevsel bir zorunluluk, kit'ten sapma değil.

**Ayarlardaki iki seçenek birden birincil düğmeydi**, yani ekranda iki dominant eylem görünüyordu. Kit'in radyo grubuna çevrildi.

**Kâğıt tonları beşe indi.** Diegetic katmanda birbirinden bir iki basamak farklı otuzdan fazla bej vardı; `KarineTheme.Paper` altında `Sheet`/`Light`/`Tint`/`Edge`/`Stamp`/`Ink`/`Faded` olarak toplandı. Bu **görünümü bir miktar değiştirir** ve gözle bakılması gereken tek yer burasıdır.

Ham renk borcu 156'dan **65'e** indi; kalanlar CCTV taraması gibi saydamlıklı efektler ve piksel portre ten tonları, yani oyun sanatı.

## Sinematikte hızlandırma yok, fontlar geldi (25 Eylül 2026)

**Kullanıcı kararı:** filmlerde İLERİ SAR olmayacak. Bir sinematik ya izlenir ya geçilir; arada bir hız kademesi oyuncuya karar verdirecek bir şey katmıyor, üstelik kontrol çubuğunu kalabalıklaştırıyordu. Hızlandırma **CCTV izlemede** anlamlıdır ve orada zaten var (oynat/duraklat, kare ilerlet, baştan al). `CinematicControls` bileşeni isteğe bağlı hızlandırmayı desteklemeye devam ediyor; filmler artık bağlamıyor.

**GEÇ düğmesi kit düğmesi oldu.** Kutu filigranı örtmek için film karesine oranlı büyüyordu ve ekranda kit dışı, kocaman bir kutu gibi duruyordu. Artık yüksekliği rahat dokunma hedefinde, eni 260 px'te kapanıyor. Etiket kit'in büyük harf düğme dilinde: "Geç" → "GEÇ".

**Roboto Slab ve Inter depoya girdi** (LFS). Başlıklar artık slab-serif, gövde ve düğmeler Inter, teknik metin IBM Plex Mono — kit §2'nin istediği dört rol de gerçek. Türkçe kapsamı glif glif denetlendi. Roboto Slab'da ₺ yok; metası mono'ya yedekleniyor. Doğrulayıcı dört dosyayı da zorunlu tutuyor.

## Sinematikte tek denetim: GEÇ (25 Eylül 2026)

**Kullanıcı kararı:** duraklatma ve ilerleme çubuğu da kalktı. Sinematik bir karar anı değil; film ya izlenir ya geçilir. Kontrol çubuğu bütünüyle kaldırıldı, `CinematicControls` ve `Clock` bileşenleri silindi, `cine_pause` ve `cine_forward` simgeleri kaldırıldı.

**GEÇ artık tasarlanmış bir düğme.** Eskiden kit'in ikincil düğmesiydi ve filmin üstünde düz bir kare gibi duruyordu. `KarineUI.SkipButton` kendi zeminini taşıyor: yarı saydam koyu dolgu, krem çerçeve, birincil eylem kenarı ve `cine_skip` simgesi. Metin ayrı bir etiket — UI Toolkit'te `Button.text` ile çocuklar üst üste biniyor; önceki çubukta İLERİ SAR'ın simgesiyle yazısının çakışmasının sebebi buydu. Test bu kenarın varlığını kilitler.

## Film üstündeki düğmeler saydam değildir (25 Eylül 2026)

**Kullanıcı kararı:** GEÇ ve benzerleri yarı saydam olmayacak, kit'te nasılsa öyle çizilecek. `SkipButton` artık kit'in birincil düğmesi: dolu krem zemin, koyu yazı, sol eylem kenarı, `cine_skip` simgesi. Sinematikte tek eylem olduğu için birincil olması kit'in "tek birincil eylem" kuralına da uyar. Test zeminin tam donuk ve kit kremi olduğunu kilitler.

Aynı karar CCTV görüntüsündeki düğmelere de uygulandı: oradaki "GEÇ" aynı bileşene, kapat/oynat/kare ilerlet/baştan al kit'in ikincil düğmesine bağlandı — elle kurulmuş renkler kalktı.

## Yazı tipi: mono'dan çıkış, logo diline yaklaşma (25 Eylül 2026)

**Kullanıcı:** "oyunun içindeki fontlar aşırı kötü, KARINE logosunun fontuna benzer bir font uygulanmalı."

Kötü görünmesinin asıl sebebi bulundu: kök öğenin yazı tipi **IBM Plex Mono**'ydu, yani menüsünden dosyasına kadar bütün oyun monospace okunuyordu. Roboto Slab ve Inter dün depoya girdiği hâlde gövde yazısı hâlâ mono'ya bağlıydı. Kök artık Inter; mono **yalnız** teknik metinde (`Technical`) kaldı: dosya numarası, tarih, saat, güven yüzdesi.

**Logo diline yaklaşma.** Logo bir görseldir, font değil; ağır, sıkışık ahşap dizgi görünümündedir. Okunur akrabası olarak **Alfa Slab One** (OFL) eklendi ve yeni bir `Display` rolü oldu. Yalnız **28 punto ve üstünde** kullanılır; küçük başlıklar Roboto Slab'da kalır.

**Kit §2 ile ilişkisi:** kit "logo fontunu normal UI metinlerinde kullanma" der, gerekçesi okunabilirliktir. Burada logo fontunun kendisi değil ağır slab akrabası kullanılıyor ve yalnız büyük puntoda — kuralın gerekçesi korunuyor. Küçük punto eşiği bu yüzden var ve testle kilitli.

## `BubeApp` konu başına bölündü (25 Eylül 2026)

3.125 satırlık tek dosya sekiz `partial` parçaya ayrıldı: çekirdek, menü, sinematik, masa, soruşturma, görüşme, CCTV, rapor. Bölünme **mekaniktir** — tek satır davranış değişmedi, 84 test bölünmeden sonra da geçiyor. Amaç yeni ekranların tek dosyaya yığılmasını durdurmak; doğrulayıcıda 560 satır kilidi var ve kaldırılıp denendi, beklenen hatayı verdi.


## Renk ve punto tek kapıya alındı (25 Eylül 2026)

**Kullanıcı:** "UI/UX ile yapman gereken bütün her şeyi bitir."

Kit taşımasının kalan iki borcu kapandı.

**Düğmeler.** Elle kurulan 41 düğmeden 5 kaldı ve beşi de gerekçeli: CCTV'nin metni çalışma anında değişen üç düğmesi (elle kurulur ama `KarineUI.Paint` ile kit boyasını alır), masadaki görünmez `Hotspot` ve menünün kendine özgü satırı. Kâğıt katmanı için `KarineUI.PaperButton` (`Action`/`Choice`/`Quiet`) ve her iki katman için `KarineUI.CloseButton` eklendi. Kâğıt düğmesi HUD renklerini kâğıda taşımamak için ayrı bir bileşen — diegetik/HUD ayrımı bilerek korundu (kit §13).

**Renk.** Ekranlardaki 65 ham renk 16'ya indi. Yeni durak yaratmak yerine anlamı olan token'lar eklendi: `Veil(alpha)` (katman perdesi), `GlassDeep`/`Glass`/`GlassLift` (terminal camı, paletin koyu ucundan türer), `HotspotHover` (vurgu kahvesinden), `Alpha(token, a)` (saydamlık için ekran kendi RGB'sini yazmasın), `Paper.Folder`/`FolderEdge`/`FolderDeep`/`Board`/`Approved` (kâğıdın altındaki fiziksel malzeme). Birbirinden bir iki basamak farklı onlarca kahve ve koyu bu duraklara toplandı; bu bir **birleştirmedir**, tek tek piksellerin eski değeri korunmadı.

**Kalan 16 renk bilerek kalıyor** ve kodda öyle yazılı: piksel portrenin ten/saç/giysi tonları ile CCTV'nin cam, tarama, parazit ve köşe işareti efektleri oyun sanatıdır, arayüz paleti değil. Bir yüzü kit kremine boyamak portreyi bozar.

**Punto.** Ekranlardaki her `style.fontSize` artık `Typography.Snap`ten geçiyor; doğrulayıcı geçmeyeni dosya:satır olarak bildiriyor. Tek istisna CCTV'nin görüntüyle ölçeklenen kamera yazısı.

Her iki kilit de kaldırılıp denendi ve beklenen hatayı verdi. 77 EditMode + 7 PlayMode testi geçiyor. **Ama bu statik doğrulamadır:** ekranların yeni renklerle nasıl göründüğü Play Mode'da görülmedi, madde `[~]`.

## Dosya #001'in içeriği kapandı (25 Eylül 2026)

**Kullanıcı:** "şu an için tamam diyebiliriz vaka 001 için."

Vaka #001'in tasarımı ve içeriği şimdilik yeterli sayılıyor: yeni ifade, kaynak, kanıt ya da soruşturma turu eklenmeyecek. Karar **içerik** hakkındadır, doğrulama hakkında değil — oynanışın baştan sona gözlenmesi hâlâ açık ve M1/M2 bu yüzden `[~]`. Sonraki oturumlar #001'i büyütmek yerine ya doğrulamaya ya başka bir eksene gider.

## Vakadan bağımsız temeller: ses, geri tuşu, reklam (25 Eylül 2026)

**Kullanıcı:** "temeli şimdiden atabileceğimiz her şeyi yapalım ki bundan sonraki süreçte sadece vakaları eklemek kalsın." Reklam politikası olarak vaka arası + ödüllü yeniden deneme seçildi, buna ek olarak yanlış karar verildiğinde ödüllü bir **yönlendirme** istendi: "yani aslında yine karar kullanıcıya kalıyor."

**Ses.** Projede tek `AudioSource` yoktu; sonra eklemek her ekrana geri dönmek olurdu. Üç kanal (müzik, oda ortamı, efekt) kuruldu. Ses dosyası **yok** ve sistem dosyasız doğru çalışıyor: eksik klip sessiz geçer. Kit'te kaydırıcı olmadığı için ses üç kademedir ve radyo grubuyla seçilir. Varsayılan müzik kısık — dedektiflik oyunu sessiz odada oynanır. Düğme sesi kit düğmesinin kurucusundan çıkar; hiçbir ekran ses eklemeyi unutamaz, doğrulayıcı bunu kaynak üstünde kilitler.

**Geri tuşu.** Android'de karşılığı yoktu, yani oyuncu soruşturmanın ortasında kazara oyundan çıkabiliyordu. Geri tuşunun hedefi ekranın kendi "geri" eyleminin aynısı; ayrı bir gezinti ağacı tutulmuyor, yoksa ikisi zamanla birbirinden ayrı düşer. Ana menüde onay modalı açılıyor.

**Vaka zinciri sonu.** `nextCaseId` boşken ya da sıradaki vaka taslakken masa sessizce boş kalıyordu. Artık kapanış bildirimi var; sıradaki adımı söylemiyor, yalnız yeni dosya olmadığını söylüyor.

**Reklam.** Ağ değil **dikiş** girdi. Tek karar yeri `AdGateway`. Kurallar: onay alınmadan reklam yok (sorulmamış olmak izin değildir), araya giren reklam yalnız vaka kapandıktan sonra, ödüllü reklam yalnız rapor geri döndükten sonra. Soruşturmanın, sorgunun, CCTV'nin ve sinematiğin içi kapalı — oyuncunun düşündüğü an kesilmez. Reklam kaldırıldıysa ağ hiç çağrılmaz ama ödül alınmaz; ağ yokken de ödül verilmez, yoksa ağ takıldığı gün denge sessizce değişir.

**Ödüllü ipucu ve kanon.** Ödüllü reklamın klasik kullanımı "ipucu izle"dir ve bu oyunda doğrudan çekirdeği satardı: değişmeyen kural fail ipucu, sonraki adım ve gizli durum göstermemektir. Çözüm, ipucunun **vakanın gerçeğinden değil oyuncunun kendi çalışmasından** türemesi: yöntem hatırlatması (işin kuralları) + kapsama sayıları (açılabilir kaynaklardan kaçı açıldı, sorulabilir sorulardan kaçı soruldu). Kişi adı, kaynak başlığı ve karar etiketi geçmesi doğrulayıcıda yasak ve yasak sözcükler vaka metninden türetiliyor — böylece ileride iyi niyetle "Hasan'ın ifadesine bak" yazan biri testi düşürür. Karar oyuncuda kalır.

**Ödüllü yeniden deneme ilk girdiğinde yalnız kapıydı**; etkisi aynı gün verilen kararla bağlandı (aşağıya bakın).

**Kurulum senin elinde olan iki şeye bağlı:** LevelPlay bir Unity Gaming Services oyun kimliği, AdMob bir uygulama kimliği ister; ikisi de hesap açmayı gerektirir. Ayrıca reklam mağaza tarafında gizlilik metni, yaş derecesi ve Play "Data safety" formu gerektirir.

## Ödüllü yeniden deneme: güven iade edilir, kayıt kalır (25 Eylül 2026)

**Kullanıcı:** "Ödüllü yeniden denemede güven geri verilsin, faks geçmişi başarısızlığı saklasın."

İki cümle birbirini dengeliyor: ödül gerçek (puanın geri gelir, gerekiyorsa görev de geri gelir), ama **iz kalır**. Faks geçmişindeki satır silinmiyor, "yeniden açıldı" diye işaretleniyor ve sayımlar onu görmeye devam ediyor; ikinci deneme kendi satırını yazıyor, yani bir vaka geçmişte iki satır tutabiliyor. Böylece kariyer dürüst kalıyor: reklam bir başarısızlığı geri alabilir ama tarihten silemez.

İade tam o faksın götürdüğü kadardır, sabit bir sayı değil — yanlış suçlamanın iadesi de eksik raporun iadesi de kendi ağırlığında. Aynı faks bir kez iade eder.

**Yeniden açmak ipucu değildir.** Soruşturmada bulunanlar silinmiyor (aksi hâlde ceza olurdu) ve hiçbir yeni bilgi verilmiyor; yalnız rapor alanları boşalıyor, yani vaka ikinci kez gerekçeli sonuç göndermeye açılıyor. Oyuncu neyi değiştireceğine kendisi karar verir.

Yedi yeni EditMode testi bu kararların her cümlesini tutuyor (iade tam mı, kayıt duruyor mu, ikinci satır yazılıyor mu, soruşturma korunuyor mu, başarılı vakaya teklif çıkmıyor mu, görevden ayrılma kalkıyor mu). 98 EditMode + 7 PlayMode geçiyor. **Play Mode'da gözlenmedi, madde `[~]`.**

## Sesin atmosferi: soğuk oda, faks zili, seyrek noir (25 Eylül 2026)

**Kullanıcı:** "sesleri geçici değil ciddi anlamda atmosfere göre oluştur, müzik artık ne eklenmesi gerekiyorsa."

Sekiz klip yer tutucu değil; her biri bir karar. Referans dünya 90'lar sonu bir emniyet birimi: floresan, kâğıt, mekanik düğme, faks.

**Arayüz sesleri alçak ve kısa.** Dokunduğunu onaylarlar, dikkat istemezler. `ui_press` mekanik bir düğmedir (alçak gövde vuruşu + kuru tık), `ui_typewriter` ondan ayrılır çünkü daktilo tuşu çelik bir koldur — ince bir çınlaması vardır. `ui_page` tek gürültü patlaması değil üç düzensiz sürtünmedir; tek patlama kâğıt değil "ıss" olur. `ui_stamp` en tok ses: mühür kesindir, geri alınmaz.

**Bildirim faksın zili.** Alarm değil haber: küçük bir zil, anharmonik kısmilerle (gerçek zil gibi) ve önünde mekanizmanın tıkı. Beş arayüz sesinin en gürültülüsü, çünkü masaya evrak düştüğünü fark etmen gerekiyor.

**Odalar ayrı yerler.** Ofiste havalandırma, uzakta trafik ve **duvar saati** var — tam bir saniyede bir tik/tak, kuru ve alçak; zamanın geçtiğini hatırlatır. Görüşme odasında trafik yok, saat yok: daha derin uğultu, daha yakın floresan (balastın kararsız titremesiyle), tavanda ince bir çınlama. Sorgu odası kapalı ve baskılıdır; dışarıdan ses gelmemesi bunun yarısıdır.

**Müzik seyrek, çünkü oyuncu düşünüyor.** Am–F–Dm–E, akor başına sekiz saniye, 32 saniyelik döngü. Dördüncü akorda melodi hiç girmiyor — nefes. Menüde durup bekleyen bir oyuncuyu sıkmaması gerekiyor, o yüzden tema akılda kalıcı bir ezgi değil bir zemin. İnce bant hışırtısı var: dijital sessizlik soğuk durur.

**Kayıt değil sentez.** Klipler `Tools/make-audio.py` içinde sıfırdan üretiliyor. Sebebi zorunluluk değil tercih: bir tonu değiştirmek yeni bir kayıt aramak yerine bir satır değiştirmek oluyor, ve `ui_stamp`ın neden tok olduğu kodda **yazılı**. Dinleyemediğim için `Tools/check-audio.py` yazdım: kırpma, DC kayması, seviye, ölü sessizlik ve döngü dikişi ölçülüyor, müzikte dört akorun kökü Goertzel ile aranıyor. İlk ölçüm iki gerçek kusur buldu — yastık zarfının atağı saniye yerine oranla çalıştığı için müzik yalnız tellerden duyuluyordu, ve sürekli katmanların kuyruğu başa eklendiği için döngü başında seviye kamburu vardı. İkisi de düzeltildi, ölçüm 0 bulgu.

**Ama bu ölçümdür, kulak değil.** Sesin oyunda nasıl durduğu, seviyelerin birbirine göre dengesi ve döngünün gerçekten dikişsiz duyulup duyulmadığı Play Mode'da senin kararın.

## Ses yumuşatıldı; karakterler konuşuyor (25 Eylül 2026, aynı gün)

**Kullanıcı:** "room_office çok kötü, neden öyle bir ses var ki? … görüşme alanındaki ses de çok kötü… oyun dedektif bir oyun ama oyun sesini açanları da ürkütmemek gerekiyor, daha soft bir buton tıklama, interview, room office olmalı."

İlk sürüm tür doğruydu ama **dozu yanlıştı**: duvar saatinin tik takı, floresan uğultusu ve görüşme odasındaki tavan çınlaması gerilim kuruyordu. Bir oyunda gerilim sahneden gelir, ortam sesinden gelirse yalnızca yorar. Karar tersine çevrildi:

- **Odalar neredeyse sessiz.** Saat kaldırıldı, floresan uğultusu kaldırıldı, tavan çınlaması kaldırıldı. Kalan, kapalı bir odanın kendi havası; seviye −26 dBFS. İki oda arasındaki fark artık ses değil **renk**: görüşme odasında üst frekans yok, yani duvarlar yakın.
- **Düğme yuvarlandı.** Eski `ui_press` mekanik bir klavye tıkıydı. Kullanıcı bunu "daktilo sesi menülere konulmuş" diye duydu — haklı olarak, çünkü ses o karakterdeydi; oysa daktilo o düğmelerde hiç çalmıyordu. İkisi de düzeltildi: düğme artık üstü kapalı alçak bir "tup", daktilo ise **yalnız faks basılırken** çalıyor. Yani yanlış olan yerleştirme değil sesin kendisiydi, ama sonuç aynıydı.

**Karakterler konuşuyor.** Görüşmede cümle yazılırken artık daktilo değil sesin **gövdesi** duyuluyor: iki formant, yuvarlak açılış, çok alçak, altı karakterde bir. Kelime yok — kelime olsa Türkçe metnin üstüne yabancı bir dil binerdi ve her cümle için ayrı kayıt gerekirdi. Perde kişinin kimliğinden türüyor, yani üç kişi üç ses oluyor ve aynı kişi her zaman aynı perdeyle konuşuyor; her vuruşta perde biraz oynuyor, yoksa insan değil makine duyulur. Vaka verisine yeni alan eklemek gerekmedi.

**`ui_page` kullanılıyor, ama az görünüyordu:** kâğıt düğmesindeydi. Artık evrak gezintisinin okları da onu çalıyor — sayfa çevirmenin sesi kâğıttır, düğme değil.

## Kâğıt koyulaştı, ses formanta geçti (25 Eylül 2026, aynı gün)

**Kullanıcı:** "Dosyayı açtığım zaman kişi ifadeleri, kanıtlar, zaman çizelgesine tıkladığımda daktilo galiba aktif oluyor, onun değişmesi gerekiyor. Görüşmede kişilerin voice_mumble çok kötü, daha gerçekçi bir insan ses tonu yapılabilir."

**Dosya sekmelerindeki ses daktilo değildi, `ui_page`'di** — ama teşhis yine sonucu buldu: sesin yüksek bandı fazla, açılışı fazla keskindi, yani kâğıdın sürtünmesi değil tıkı duyuluyordu. Kâğıt koyulaştı ve yayvanlaştı: bant merkezleri aşağı indi, her sürtünmenin açılışı yumuşadı, tepe 0,30'a düştü. Kavramsal eşleme doğruydu (dosya sekmesi kâğıttır, düğme değil), düzeltilen ses.

**Konuşma sinüs yığını olmaktan çıkıp formant sentezine geçti.** Bir sesin insan gibi duyulmasını üç şey sağlıyor: harmonik açısından zengin bir kaynak (testere dalgası gırtlak darbesine yakındır), yüksek Q'lu üç formant yankılayıcı (sesli harfin kimliği formant tepeleridir) ve hecenin sonuna doğru düşen perde ile küçük bir titreme — sabit perde insan değil zil olur. Az nefes gürültüsü var, çünkü kuru kaynak plastik durur.

**Tek hece de yetmiyordu:** aynı klip tekrar ederse konuşma değil sinyal olur. Artık üç sesli harf var (`voice_a`, `voice_e`, `voice_o`) ve karışık sırayla çalıyor; her vuruşta hece, perde ve ağırlık biraz oynuyor. Kelime hâlâ yok ve olmayacak: kelime, Türkçe metnin üstüne yabancı bir dil bindirir ve her cümle için ayrı kayıt ister.

Ölçüm de büyüdü: `check-audio.py` artık formant tepesi arıyor — F1/F2 çevresindeki güç, formant aralarındaki frekanstan en az 6 dB yüksek olmalı. Üç sesli harf +11,9 / +18,5 / +29,7 dB ile geçiyor. **Yine kulakla dinlenmedi.**

## Kâğıt sesi kalktı, konuşma taklidi yerine klavye (25 Eylül 2026, aynı gün)

**Kullanıcı:** "ui_page çok kötü, buradaki amaç nedir? Kâğıt sesi olmasına gerek yok, buton sesi koyabiliriz." — ve: "a e o voice sesi de kötü, onun yerine klavye sesi olabilir, chat sesi olabilir."

**`ui_page` tamamen kalktı.** Diegetic gerekçe ("dosya sekmesi kâğıttır, sayfa çevirmenin sesi kâğıt olur") kulakta karşılığını bulmadı: sekme bir düğmedir, öyle de duyulmalı. Kâğıt düğmesi (`PaperButton`) ve evrak gezintisinin okları artık `ui_press` çalıyor; klip, generatör ve doğrulayıcı kaydı silindi. Bir sesin gerekçesi güzel olabilir ve ses yine kötü olabilir; ölçüt kulak.

**Konuşma taklidi bırakıldı.** İki sürüm denendi — sinüs yığını sentezleyici, formant sentezi ise insan sesi taklidi gibi duyuldu. Taklit, kaydın kendisi olmadıkça tekinsiz kalıyor; oyun da yazıyla konuşuyor, sesle değil. **Artık görüşmede duyulan şey konuşma değil, ifadenin kayda geçirilmesi:** klavye tuşu (`ui_key` / `ui_key_low`, karışık ve alçak, 0,55 kazanç). Daktilo değil klavye — çelik kol ve çınlama yok, çünkü daktilo faksın sesidir ve iki yüzey karışmamalı.

Bunun bir yan sonucu var ve iyi bir sonuç: `VoicePitch` de kalktı. Tuş karşıdakinin sesi olmadığı için kişi başına perde anlamsızdı; yeni vakaların yeni kişileri artık ses tarafında hiçbir veri istemiyor. Dokuz klip, 0 bulgu, 98 EditMode + 7 PlayMode yeşil. **Yine kulakla dinlenmedi.**

## Dördüncü sürüm: taklit yok, blip (25 Eylül 2026, aynı gün)

**Kullanıcı:** "chat sesi yap, tuş sesi de iyi değil."

Görüşmedeki satır sesinin dördüncü sürümü. Üçü de düştü ve düşme sebepleri aynı yere çıkıyor: **sinüs yığını** sentezleyici gibiydi, **formant sentezi** insan sesi taklidi gibiydi, **klavye tuşu** gürültülü ve yorucuydu. Ortak hata taklit etmeye çalışmaktı — taklit, kaydın kendisi olmadıkça tekinsiz kalıyor, üstelik Karine yazıyla konuşuyor, sesle değil.

Bu yüzden yeni ses hiçbir şeyi taklit etmiyor: `ui_chat` / `ui_chat_low`, yumuşak bir sinüs ve üstünde küçük bir çıngırak kısmisi (1:2,01 — tam oktav değil, yoksa organ gibi durur), rampalı açılış (tık yok), kısa sönme, çok az hava. İki varyant karışık çalıyor; alçak olan biraz daha uzun sönüyor, böylece ikisi aynı sesin iki vuruşu gibi durmuyor. Tepe 0,22: bir cümle boyunca düzinelerce kez duyulacak bir ses dikkat isterse yorgunluk yapar.

Kural olarak not: **bir sesin gerekçesi ne kadar sağlam olursa olsun, ölçüt kulaktır.** Kâğıt sesi de böyle gitti. Dokuz klip, 0 bulgu, 98 EditMode + 7 PlayMode yeşil. **Yine kulakla dinlenmedi.**

## Oynanıştan gelen dört bulgu (25 Eylül 2026, oyun baştan sona oynandıktan sonra)

Kullanıcı Dosya #001'i baştan kapanışa kadar oynadı, içerikte sorun çıkmadı ve sesler duyuldu. Dört bulgu oynanışın kendisinden geldi — hiçbiri masa başında görülebilecek şeyler değildi.

**1. Masadaki nesneler sessizdi.** Gelen evrak, dosya, terminal ve görüşme masada görünmez birer düğme (`Hotspot`); basınca ses gelmiyordu. Sebep kuralın kapsamıydı: "her `new Button(` `Sounded(` ile sarılır" kilidi yalnız `Runtime/UI` klasörüne bakıyordu, ekranların içinde elle kurulan beş düğme kapsam dışındaydı. Kural artık bütün `Runtime`e bakıyor; masadaki nesneler, menü satırı ve CCTV'nin üç oynatma düğmesi sesli. **Ders:** bir kilidin değeri kapsamı kadardır.

**2. Ana menü müziği hiç duyulmuyordu.** Akış klibi (`loadType: 2`, arkaplan yüklemesi) `Play()` anında hazır değildi ve Unity bunu **sessizce** geçiyor — ne hata, ne uyarı. İçe aktarım artık önceden yüklüyor (`preloadAudioData: 1`, `loadInBackground: 0`) ve `AudioDirector` yüklenmemiş klibi kendisi yüklüyor. Ayrıca "kısık" kademesi 0,35'ten 0,55'e çıktı: 0,35'te müzik varsayılan ayarda duyulmuyordu. Ölçüm dosyayı ölçer, çalmayı ölçmez — bu yüzden dokuz klip "0 bulgu" verirken müzik sessizdi.

**3. Masada gürültü yerine müzik.** Oda havası (`room_office`) bir süre sonra yoruyor, üstelik masada oyuncu **okuyor** ve okumaya eşlik eden şey müziktir. Masa artık `desk_theme` çalıyor: 40 saniye, Dm–Gm–B♭–A, menü parçasından yavaş (akor başına 10 s), kırk saniyede yalnız iki nota ve belirgin biçimde alçak — metnin üstünde durmaması gerekiyor. `room_office` silindi. Görüşme odası ortam sesiyle kalıyor: orada oyuncu okumuyor, konuşuyor.

**4. Ayarlar sayfası telefonda sıkışıktı.** `MenuOverlay` kartı kenarlardan sabit %27 içeriydi — geniş ekranda makul, dikey telefonda daracık bir şerit. Pay artık ekranın biçiminden geliyor (dikeyde %5, yatayda %24), kartın içi kaydırılabilir (başlık sabit, içerik akar), ses kademeleri üç sıkışık radyo yerine kit'in sekme şeridiyle seçiliyor ve bölümler alt başlık + çizgiyle ayrılıyor. Kit'e yeni bileşen **eklenmedi**; var olanlar doğru yerde kullanıldı.

## Ses beş kademe ve radyo listesi (26 Eylül 2026)

**Kullanıcı:** "Şimdi bu seslerin ayarlar kısmında ayarlanması lazım; kısık, kapalı, yüksek falan diye değil, radio button şeklinde olmalı." Sorulan seçenekten **beş kademeli radyo listesi** seçildi.

Üç kademe (kapalı/kısık/açık) ince ayar için yetmiyordu: "kısık" kelimesi oyuncuya ne kadar olduğunu söylemiyor ve iki uç arasında tek durak var. Artık beş kademe var — **kapalı, %25, %50, %75, tam** — ve ayarlarda alt alta radyo satırı olarak duruyor, yukarıdan aşağı azalarak (önce istenen genelde en yüksek olan). Kit'te kaydırıcı yok ve icat edilmiyor; yüzde de çevrilecek bir metin değil, o yüzden yalnız uçların sözü dil dosyasında ("Kapalı", "Tam").

**Bir gün önce koyduğum sekme şeridi geri alındı** ve gerekçesi kayda değer: beş hücre tek satıra sığmıyor, üstelik ayarların geri kalanı (metin hızı) radyo — aynı soru ekranda iki farklı biçimde sorulmamalı.

Kademe değeri artık **doğrudan yüzde** (`Off = 0 … Full = 100`), kazanç ondan türüyor. Böylece kayıtta duran sayı okunduğunda ne olduğu belli ve yeni bir kademe eklemek eski kaydı bozmuyor. Eski üç kademeli kayıt okunurken çevriliyor (1 → %50, 2 → tam), yani hiç kimsenin ses ayarı göç yüzünden sıfırlanmıyor — bunun kendi testi var. Varsayılan: müzik %50, efektler tam.

## Cihaz adımı ertelendi (26 Eylül 2026)

**Kullanıcı:** telefonda oynayamıyor — elde Android cihaz yok.

Bu makinede iki ikame sunuldu (Unity'nin kendi Android SDK'sındaki `adb` üstüne emülatör + ARM64 sistem imajı; ya da Unity Device Simulator paketi) ve bir de ölçülebilir kısmı kapatan otomatik çözünürlük/dokunma hedefi testleri. Kullanıcı **ertelemeyi** seçti.

**Karar:** Faz 2 kapanmıyor ve masaüstü oynanışı cihaz ölçütünün yerine geçmiyor. `[x]` işaretleri yalnız gerçekten gözlenen davranış için; "emülatörde çalıştı" ile "telefonda çalıştı" aynı cümlede yazılmaz. İş Faz 3'ten (Dosya #002) devam ediyor, cihaz adımı telefon bulunduğunda koşulacak. Bu, bilinen ve kabul edilmiş bir açık maddedir — unutulmuş bir eksik değil.

## Bölüm seçici ekranı (26 Eylül 2026)

**Kullanıcı:** "Yeni bir sayfa yapalım, Bölüm seçici ekranı eklememiz gerekiyor; ekran görüntüsündeki gibi birebir yapman gerekiyor." Maket: üstte kimlik şeridi (logo, Bora, rütbe, kurum güveni, tamamlanan vaka sayacı, ayarlar), solda kaydırılabilir ülke listesi, ortada iğneli dünya panosu, sağda seçili ülkenin kartı (görsel, şehir, açıklama, ülke ilerlemesi, "Vakaları Görüntüle"), altta yedi dosyanın şeridi.

**Yerleşim maketten alındı, dil kit'ten.** Ekranın içine renk, punto ya da yeni bileşen yazılmadı: ülke satırı, pano iğnesi ve dosya kartı `KarineTheme` + `KarineUI` ile kuruldu (`Panel`, `Row`, `Rule`, `Meter`, `Counter`, `Progress`, `Dot`, `Icon`, `IconButton`, `Technical`).

**Maketten üç bilinçli sapma:**

1. **Kilit/onay/oynat simgesi yok.** Kit'in ikon kümesinde kilit, tik ve oynat yok, emoji de yasak (Kit §15). Durum bunun yerine bir renk noktası + tek kelimelik teknik etiket: `TAMAM` / `AÇIK` / `KİLİTLİ` / `YAZILMADI`. İkonlar `UI_KIT.png`'den kesilince buraya girer.
2. **Görseller henüz yok.** Dünya haritası (`Bube/WorldMap`), ülke kartı görselleri (`Bube/Worlds/<id>`) ve maketteki polaroidler depoda değil. Ekran onlar olmadan da tamdır: harita yerine mukavva pano + kâğıt iğneler, kart görseli yerine iğne simgesi durur. Görsel gelince tek satır veri değişikliğiyle yerine oturur.
3. **Ülke sayısı maketten alındı: on.** `WORLD_OPENINGS.md` yedi dünya yazıyordu; çelişki aynı gün kullanıcı kararıyla kapandı (aşağıdaki madde).

**Kilit ilerlemedir, ipucu değil.** Ülke sırası: ilki her zaman açık, sonraki ancak önceki ülkenin bütün dosyaları kapanınca açılır. Dosyalar ülke içinde sırayla açılır. Hiçbir kilit "şunu yap" demiyor ve hiçbir yerde sıradaki adım yazılı değil — ekran yalnız neyin kapandığını gösterir. Kapanmış dosyaya basmak kariyer kaydını açar (oyuncu ne gönderdiğini ve kurumun ne dediğini yeniden okur), açık dosya masaya götürür, yazılmamış dosya pasiftir. "Vakaları Görüntüle" ekran değiştirmez, alt şeridi öne çıkarır: dosyayı yine oyuncu açar.

**Dosya adı ve başlığı veriden gelir.** Maketin "Beyoğlu'nda Kayıp / İsimsiz Zarf" gibi adları yer tutucuydu; şeritte vakanın **kendi** başlığı yazıyor (Dosya #001 için `case.title`). Türkiye'nin 3–7. yuvaları başlıklı ama vaka kimliği **boş**: dosya henüz yazılmadı, kilitli değil. Oyuncuya açılmayacak bir kilit gösterilmez.

## Dünya sayısı yediden ona çıktı (26 Eylül 2026)

**Kullanıcı:** "10 ülke kalsın, `WORLD_OPENINGS.md`'yi ona göre güncelle."

Bölüm seçici maketi on ülke × yedi dosya gösteriyordu, kanon ise yedi dünya diyordu. Seçiciyi yediye indirmek yerine **kanon güncellendi**: oyun on dünya, her dünya yedi dosya, toplam **70 vaka**. 24 Eylül 2026'daki yedi dünya kararı bu kararla geçersizdir.

Ülkeler adlandırıldı ve sıraları `Worlds.json` ile aynıdır: Türkiye (İstanbul), Birleşik Krallık (Londra), Almanya (Berlin), Japonya (Tokyo), Fransa (Paris), ABD (Chicago), İtalya (Napoli), İspanya (Sevilla), Kanada (Montreal), Avustralya (Melbourne). Şehirler Türkiye dışında ilk taslaktır; o ülkenin vakaları yazılırken değişebilir. Adlandırma **içerik taahhüdü değildir**: mekân, atmosfer, vakalar ve açılış sinematiği hâlâ yazılmamış iştir.

**Yan etki, açıkça yazılıyor:** ülke sırası artık bağlayıcı bir ilerleme kuralıdır. Eski not ("ülkelerin sırası kariyerin sabit ilerleme kuralı olarak buradan çıkarılmaz") bölüm seçicinin kilidiyle çelişiyordu; belge kilide göre düzeltildi. Kilit yine yalnız ilerlemeyi gösterir, sıradaki adımı söylemez.


## Dosya #002 "Son Sefer" ve dört sütunlu rapor (26 Eylül 2026)

Vaka 2'nin senaryosu kullanıcı tarafından yazıldı ve eski "Kayıp Yedek" taslağının yerine geçti; taslak veriyle birlikte tamamen kaldırıldı. Yeni vaka kişilere karşı işlenen suçlar alanındadır (yağma/gasp — yaralama) ve **BDS Asayiş Masası**na bağlıdır. Gerçek birim adları yasak listesine eklendi (KOM, narkotik, TEM, adli tıp kurumu ve açık yazımları); büro adları kurmaca BDS adlarından seçilir.

Tasarım kararı: bir olayda birden fazla sorumluluk olabilir. Selçuk Yalın'ı yaralayan Emre Koç'tur, ama parayı alan Kerem Şahin'dir; Deniz Arslan olaydan önce ayrılmıştır ve suçsuzdur. Mağdur da yalan söyler (kayıt dışı borcu saklar) ama mağdurdur — **yalan söylemek fail olmak değildir**. Oyun hiçbir yerde "şu kişi yalan söylüyor" demez; kimseyi bir sonraki kaynağa yönlendirmez.

Motor karşılığı: sonuç raporu artık vakadan gelen isteğe bağlı bir dördüncü sütun taşıyabilir (`CaseData.custody`). Sütun yalnız vaka tanımlarsa sorulur, sihirbaz adım sayısı veriden gelir ve yanlış kişi yazmak fail sütunundaki gibi asılsız suçlama sayılır. Üç sütunlu vakalarda hiçbir şey değişmez.

## 26 Eylül 2026 — Raporun gidişi de bir andır

Dosyanın masaya bırakılışının bir karşılığı yoktu: rapor gönderilince ekran
anında özete atlıyordu. Artık gönderme, bırakılışın tersini oynuyor — Bora formu
doldurup kaşeler, evrak ekran dışına çıkar — ve özet ancak ondan sonra açılır.
GEÇ düğmesi varış filmiyle aynı köşede durur. Video yoksa özet doğrudan açılır.

## 26 Eylül 2026 — Testler oyuncunun kaydına dokunmaz

PlayMode testleri gerçek kayıt klasörüne yazıyordu; bir koşu vakayı kabul
edilmiş duruma getirip dosya bırakılış anını yutabiliyordu. Kayıtlar artık
`SaveSandbox` ile testten önce kenara alınıp sonra geri konuyor.

## 26 Eylül 2026 — Her vaka kendi teklif metnini yazar

Gelen evrak tepsisindeki teklif, anahtar yoksa genel yedeğe düşüyordu ve o
yedek Dosya #001'i anlatıyordu: ikinci vakayı kabul eden oyuncu birincinin
adını ve özetini okuyordu. `case002.offer.*` yazıldı, genel yedek nötrleşti ve
doğrulayıcı artık her vakadan kendi teklif metnini istiyor.

## 26 Eylül 2026 — Sonuç sekmesi her vakada aynı anda açılır

Dosya #002 "Sonucu gönder" sekmesini dört belgenin okunmasına bağlıyordu;
Dosya #001 yalnız ilk rapora bağlar. Oyuncu iki vakada farklı bir oyunla
karşılaşıyordu. #002 de ilk rapora bağlandı: rapor her an gönderilebilir,
desteksiz gönderim zaten değerlendirmede karşılığını buluyor.

Kapak metinleri de (`file.caseType`, `tablet.caseLine`) vakaya özel anahtar
yoksa genel yedeğe düşüyor ve Dosya #001'in "KONUT HIRSIZLIĞI" başlığını
taşıyordu. Yedekler nötrleşti, doğrulayıcı her vakadan kendi kapağını istiyor.

## 26 Eylül 2026 — Kaynak seçimi tam ekrana taşındı, arama kaldırıldı

Rapor adımının içindeki kaynak paneli iç içe iki kaydırma, bir arama alanı ve
dört filtre taşıyordu; telefonda okunmuyor ve yönetilemiyordu. Liste tam ekrana
çıktı, arama alanı kaldırıldı — liste zaten bu vakada okunmuş kayıtlardan
oluşuyor ve dört filtre onu bölmeye yetiyor. Ekranda her an tek bir iş var.

## 26 Eylül 2026 — Açılış tutanağı rapora gerekçe olamaz

Olay tespit tutanağı soruşturmanın başlangıcıdır: failin adını, yöntemi ya da
parayı kimin aldığını göstermez. Oyuncu gerekçesini ifadelerden, kameradan ve
belgelerden kurmalıdır. Düğüm artık `notReportSource` taşıyor; işaretli kayıt
kaynak listesinde görünmüyor ve gönderimde de reddediliyor. Doğrulayıcı her
vakanın açılış tutanağından bu işareti istiyor.

## Davranış satırı Dosya #002'nin tamamına yayıldı (26 Eylül 2026)

Deneme kapsamı dört kaynak sorusuydu (bkz. 25 Eylül girdisi). Dosya #002'de
**bütün görüşme yanıtları ve 34 yemin tamamı** davranış satırı taşıyor: 65 yeni
satır, `tr.case002.json` içinde `<answerKey>.demeanor` olarak.

Yayarken tek kural korundu: satır **gözlem**dir, teşhis değil. Bu yüzden
gerginlik ve sakinlik dört kişiye de dağıtıldı — Emre kayda bakmadan cevap
verir ve sırtını yaslar, Deniz ellerini kucağına çeker, Kerem duraksar, Selçuk
sustuktan sonra gözünü kaçırmaz. Kimin ne sakladığı satırdan okunmaz; satır
yalnız oyuncuya *neyi tekrar sormak isteyeceğini* düşündürür. Doğrulayıcının
yorum sözcüğü yasağı ve 16 sözcük sınırı 65 satırın hepsinde geçiyor.

## 27 Eylül 2026 — Vakalar sayfası
Kullanıcının referansı sunum hiyerarşisidir: solda logo, menü ve Bora; sağda başlık, yatay ülke kartları ve polaroid dosyalar. Tam ekran referans görseli UI olarak kullanılmaz. Menü videosu ana menüde korunur. On ülke × yedi slot, Beşiktaş bilgisi ve gerçek dosya adları mevcut kanondan gelir; referanstaki hayali sonraki vaka adları aktarılmaz. Ülke atlası yalnız dekoratif kartpostal, delil değildir. Henüz yazılmamış vakalara yeni hikâye içeriği eklenmez.

## 27 Eylül 2026 — Parçalı soruşturma masası
Kullanıcının masa referansı uygulanırken Görev ve Notlar kaldırılır. Dosya fotoğrafsız beyaz kapak; monitör yalnız CCTV Arşivi girişidir, görüntü önizlemesi vermez. Pencere, menüde gezilen ülkeye değil aktif dosyanın Worlds.json üyeliğine göre ülke kartpostalını gösterir. Oda dekoru, saydam nesne atlası, ülke manzarası, portreler ve metin/düğmeler ayrı katmanlardır. Panoda yalnız erişilmiş kişiler bulunur; otomatik ilişki/şüpheli çıkarımı yapılmaz. Oynanış kuralları değişmez.

## Kurum adı: "bube Polis" değil "bube Departman" (28 Eylül 2026)

Kurum zaten kurgusaldı, ama adı **polis** sözcüğünü taşıdığı sürece oyuncunun
kafasında gerçek teşkilata oturuyordu. Artık hiçbir metinde geçmiyor:

- `bube Polis` / `bube Police` → **bube Departman**
- `bube POLİS`, `BUBE POLİS` (şeritler, terminal başlıkları) → **bube DEPARTMAN**
- Terminal kısaltması `BPS` (*bube Police System*) → **BDS** (*bube Departman
  Sistemi*); masadaki marka yazısı, kamera arşivi başlığı, kayıt etiketleri ve
  Dosya #002'nin dosya numarası (`BDS-2026-0214`) dâhil.

Rütbe ve birim adları (Soruşturmacı, Soruşturma Birimi, Asayiş Masası) olduğu
gibi kaldı; onlar bir kuruma işaret etmiyor. `LocaleRules.ForbiddenInstitutions`
listesi değişmedi — "polis merkezi", "karakol" gibi girdiler oradaki yasak
listesidir, kurumun kendi adı değil.


## 1 Ekim 2026 — geniş pencereli masa referansı

Son kullanıcı görseli `codex-clipboard-5722f368-91b8-4f69-9bf2-c84aa03efba3.png` masa kompozisyonu için önceki referansın yerini alır. Geniş pencere ortada, lamba ve evrak solda, telefon orta solda, beyaz ve fotoğrafsız dosya önde, sade CCTV monitörü sağdadır. Pencere aktif dosyanın ülkesini gösterir. Notlar, görev listesi ve karakter panosu yoktur. Üst şeritte Dosya, Kişiler, Deliller, Evraklar, Kariyer, Ayarlar; logo ana menüye dönüş girişidir. Ana menü videosu ve soruşturma kuralları korunur. Referans tek ekran resmi olarak kullanılmaz: boş oda dekoru + mevcut nesne atlası + ülke atlası + kodla oluşturulan UI birleşir.

## 1 Ekim 2026 — Dosya Vaka Detay

Dosya detay ekranının yeni referansı codex-clipboard-cb153b0f-7359-4385-8f2b-e785361269ea.png. Açık kâğıt dosya, sağ sekmeler, üst marka/geri ve araçlar, solda başlık/metadata, sağ üstte mevcut olay fotoğrafı, altta olay metni ve keşfedilmiş kişiler kullanılır. Referanstaki örnek tarih, eşya görselleri veya kişi bilgileri kanonik vaka verisine eklenmez. Uzun metin tek kaydırma alanında korunur. Arama mevcut özellik olduğu için sağ sekmelerde kalır.

## 1 Ekim 2026 — Gelen Evraklar sunumu

Gelen Evraklar referansı codex-clipboard-6c8b41e1-608b-458e-8d84-c0067d966f9c.png. Sol liste koyu, seçili satır krem, okunmamış işareti kırmızı; sağda katmanlı kâğıt belge. Sahte evrak ve örnek tarih eklenmez; tarih yalnız kayıtlı değerlendirme tarihi varsa görünür. Evrak filtreleme, geliş, teslim, okunma ve kariyer kuralları korunur.

## 1 Ekim 2026 — Görüşmeler ve İncelemeler tableti

Referans: codex-clipboard-e8fc4ed2-7b18-462b-a32b-fe01a883187d.png. Görüşmeler ve İncelemeler aynı sol gezinme/orta liste/sağ kâğıt kart düzenini paylaşır. Kişi seçimi yalnız detay açar; talep ve görüşme eylemleri sağ karttadır. Sadece keşfedilmiş kişiler gösterilir; referanstaki başka vaka kişileri ve doğrulanmamış demografik bilgiler eklenmez. İnceleme sonuç metni talep kartında erken gösterilmez. Örnek rozet sayıları kopyalanmaz.

## 1 Ekim 2026 — CCTV tablet sunumu

Yeni CCTV referansı codex-clipboard-19b4c04d-11ea-4f7b-a1e4-acbd5ab00d12.png. Sol liste yalnız aktif vakada erişilebilir cctv/bps kaynaklarını içerir. Referanstaki beş kamera, saatler ve kişi iddiaları örnektir; vaka verisine aktarılmaz. Metin geniş sağ panelde monospace satırlarla gösterilir. Var olan glitch/netleştirme ve isteğe bağlı klipler korunur.

## 1 Ekim 2026 — Vakalar yeni referansı

Vakalar referansı codex-clipboard-dec8099d-ec9c-4e45-a3d3-84459dcf9b1c.png. Sol marka/menü/Bora korunur; ana panelde ülke şeridi, genel ilerleme, ülke panoraması ve dosya kartları bulunur. İlerleme tamamlanmış dünyalar/dosyalar üzerinden hesaplanır. Referanstaki örnek sayılar, başlıklar veya ödül vaadi oyun verisine eklenmez. Yazılmamış vakalar mevcut durumunu korur; gelecekteki vakalara uydurma kapak üretilmez.


## 1 Ekim 2026 — Çubuksuz kaydırma

Kullanıcı kararı: oyun genelinde kaydırma çubuğu gösterilmez. Telefonda listeler ve uzun metinler parmakla kaydırılır; içerik kesilmez veya kaydırma kapatılmaz.


## 1 Ekim 2026 — Ayarlar referansı

Ayarlar referansı mevcut menü videosu üstünde ayrı kod tabanlı panel olarak uygulandı. Tercihler Kaydet ile uygulanır; kapatma taslağı bırakır. Varsayılanlara Dön yalnız metin ve ses tercihlerinin taslağını sıfırlar; kariyer ve reklam izni değişmez. Kırmızı yalnız yıkıcı eylemlere ayrıldığı için Kaydet krem birincil eylemdir.


## 1 Ekim 2026 — Chakra Petch

Bütün oyun metinleri Chakra Petch Regular/SemiBold/Bold ailesine geçirildi. FontSet ve içerik doğrulayıcı güncellendi; logo bitmap olarak korundu. OFL lisansı fontlarla birlikte eklendi. Fiziksel telefon okunabilirliği kontrolü açık.


## 1 Ekim 2026 — Ortak geçişler

Onaylanan ilk geçiş paketi: dosya sayfasının kısa yerleşmesi, tabletin alttan ele alınması ve aşağı bırakılması, hafif düğme basılması. Kare video veya yeni bitmap kullanılmaz. Hareket azaltma ayarı yalnız bu paketi kapsar; mevcut sinematik/glitch sistemlerini değiştirmez.


## 1 Ekim 2026 — Evrak varışı

Onaylanan evrak ritüeli: kâğıt tepsiye kayar, kısa faks sesi çalar, mevcut okunmamış rozet yanıp söner. Belge kendiliğinden açılmaz ve değerlendirme sonucu efektle açıklanmaz.


## 1 Ekim 2026 — Dosya sekmesi geçişi

Dosya sekmeleri için onaylanan sayfa geçişi eklendi: kısa yatay yerleşme ve aktif sekmenin öne çıkması. Okunabilirliği bozan tam sayfa çevirme veya bekleme kullanılmaz.

## Masanın havası — ışık, toz, parallax, kalkan eşya (1 Ekim 2026)

Kullanıcı ortam sesini istemedi; oyunu yukarı taşıyacak olan animasyon ve efekt. İlk kademe masaya uygulandı, çünkü oyuncunun en çok baktığı yer orası.

- **Işık ve toz** sahneyi durgun resim olmaktan çıkarır. Renkler kit paletinden (`Paper.Light`, `Background`, `Paper.FolderDeep`); yeni renk yok.
- **Parallax** derinlik verir, ama küçük tutuldu (genişliğin %1,1'i): eşyalar çizili masadan kopmamalı.
- **Kalkma yalnız dokunulan eşyada.** Hiçbir eşya kendiliğinden parlamaz, zıplamaz ya da dikkat çekmez — efekt oyuncuyu bir kaynağa götürmez. Bu, oyuncu güdümlü soruşturma kuralının görsel karşılığıdır ve sonraki kademelerde de bağlayıcıdır: doğru ve yanlış seçim için ayrı efekt yok.
- Hareketi azalt tercihi toz, parallax ve kalkmayı kapatır; ışık ve kararma durağan kalır.

## Efektler 2. kademe — öne sürme, basım, sinyal, sayfa (1 Ekim 2026)

- **Kayıt öne sürme** artık fiziksel bir an: kayıt kâğıt olarak karşıdaki kişiye kayar. Hareket doğru kaynakta, yemde ve ilgisiz kayıtta **aynıdır** — sonucu kişinin yanıtı söyler, efekt değil. Sürme isteğe bağlı bir jesttir; dokunmak aynı işi görür (erişilebilirlik). Sürmenin varlığı küçük bir satırla söylenir (`interview.swipeHint`), sıradaki adım söylenmez.
- **Faks basımı** başarı/başarısızlık efekti taşımaz; olumlu da olumsuz değerlendirme aynı biçimde basılır. Basım oyuncuyu bekletmez: dokununca biter.
- **Sinyal bozulması** kameranın resmine aittir; renkleri kit paletinden değil kamera camından gelir (CCTV görüntüsünün mevcut kuralı).
- **Sayfa çevirme** yalnız gölgeli bir kenardır; renk ve çerçeve dili değişmedi (Kit §24).

## Efektler 3. kademe — nefes ve duraksama, göz kırpma yok (1 Ekim 2026)

Kullanıcı göz kırpmayı istemedi; karakter başına ek görsel de gerekmiyor. Kalan iki canlılık işareti:

- **Nefes:** portre göğüs hizasından binde birkaç genişleyip daralır. Genlik ve hız sabittir; yalnız başlangıç anı rastgeledir.
- **Duraksama:** her yanıt 380 ms sonra yazılmaya başlar.

İkisi de kişiye, yanıta, öne sürülen kayda ya da kişinin vakadaki rolüne göre **değişmez**. Değişseydi oyuncu onu gizli bir durumun belirtisi diye okurdu; davranış bilgisini yalnız yazılı davranış satırları taşır.

## 2 Ekim 2026 — CCTV kare dizisiyle oynatılır

Video üretilemediği için CCTV kaydı durağan kareler dizisiyle gösterilir. Kayıt `framePaths` (Resources yolları), isteğe bağlı `frameTimes` (her karenin damgası) ve `frameMs` (varsayılan 650 ms) alır. Oyun kareleri güvenlik kamerası hızında oynatır; her kare değişiminde aynı kısa titreme olur, hiçbir kare öne çıkarılmaz. Oynat/duraklat/kare ilerlet/yeniden oynat aynıdır. Video desteği silinmedi: kare yoksa video oynar. Dosya #001'in sinyal boşluğu için kare de verilemez.

## 2 Ekim 2026 — Yeni görünüm başladı: Gelen Evraklar

Kullanıcı arayüzü baştan yenilemeye karar verdi; referans görseller ChatGPT ile üretilip ekran ekran veriliyor. İlk ekran Gelen Evraklar: gece ofisi sahnesi (`Bube/UI/bg_office`), karton dosya ve eskimiş kâğıt görselleri, ataş, bube damla logosu (liste paneli ve kâğıt başlığı), yeni krem düz simge seti (`Art/Icons` içindeki nav_prev, document, folder, people, binoculars, gear yerine geçti; bu simgeler her ekranda değişti). Yerleşim önceki Gelen Evraklar düzenini korur. Eski kayıtlarda boş kalan değerlendirme türü artık ham anahtar (`[career.evaluation.]`) göstermez, sonuçtan türetilir. `UI_KIT.md` yeni görünüm tamamlanınca yeniden yazılacak; o zamana kadar bu girdi geçerlidir.

## 2 Ekim 2026 — Masa kalır, üst çubukta simgeyle gezinme yok

Masa sahnesi merkez olarak kalır. Gelen Evraklar, dosya ve talep ekranlarının üst çubuğundaki dosya/görüşme/CCTV/evrak/ayarlar simgeleri kaldırıldı. Her ekran yalnız geri düğmesiyle masaya döner; oyuncu bir sonrakini masadan kendisi açar.

## 2 Ekim 2026 — Dosya / Vaka Detayı yeni görünümde

Gece ofisi sahnesi, karton dosya üstünde eskimiş kâğıt, sekmeler kâğıt dokulu kart. Künye iki sütunlu kart ızgarası (simge etiket anahtarından: konum→pin, numara→folder, tarih→calendar, ihbar eden→person). Olay fotoğrafı eğik polaroid ve ataşla. Bölüm başlıkları simge + çizgi. İlgili kişi kartı tıklanamaz kaldı (ok işareti konmadı: oyuncuyu bir yere yönlendirmez). Referanstaki "İlgili Eşyalar" galerisi için veri alanı ve eşya fotoğrafları bekleniyor. Yeni sekme simgeleri (person, fingerprint, clock, image, compare, search, calendar) gelene kadar boş görünür.

## 2 Ekim 2026 — Dosya #001 "İlgili Eşyalar"

Rapor düğümü `relatedItems` (ad, ayrıntı, fotoğraf) alır; dosya ekranında ilgili kişilerin altında fotoğraflı küçük kartlar olarak görünür, tıklanamaz. Yalnız raporun metninde geçen eşyalar ve bilgiler konur (dizüstü S/N LQ7B-024861, kol saati, nakit para "miktar belirsiz"); rapor metninde olmayan ayrıntı (renk, marka) eklenmez. Para fotoğrafında gerçek banknot okunmaz. Doğrulayıcı eşya metinlerini ve görsellerini denetler.

## 2 Ekim 2026 — Tablet talep ekranı yeni görünümde

BDS rayı altında "GÖRÜŞME/İNCELEME TALEPLERİ" alt başlığı ve simgeli, sayaçlı iki sekme. Sayaç yalnız oyuncunun zaten gördüğü durumları sayar (görüşmeye hazır kişi, dosyaya alınmamış gelen rapor). Liste satırı: portre, ad, durum (görüşmeye hazırsa kırmızı nokta), bilgi, dosyada zaten olan alıntı ve seçim oku. Ayrıntı kartı kâğıt dokulu; portre solda, ad/bilgi/durum sağda. Referanstaki yaş/meslek/ikamet alanları ve durum süzgeci veride olmadığı için eklenmedi; satırdaki alıntı yalnız okunmuş ifadeden gelir.

## 2 Ekim 2026 — Dokulu düğmeler (kit §24 kullanıcı kararıyla kaldırıldı)

Düğmeler artık düz renk değil, dokuz parçalı gerdirilen görseller: ikincil = koyu deri + pirinç kenar (`btn_dark`), birincil/seçili = krem kâğıt + pirinç kenar (`btn_primary`), kâğıt üstü işlem/seçim = mürekkep damgası çerçevesi (`btn_paper`). Basınca görsel kararır ve bir piksel iner; her düğmede aynı. Devre dışı düğme koyu görseli gri tonla taşır. Ghost ve Quiet düz kalır. Ayar: `KarineTheme.Button`.

## 2 Ekim 2026 — Dosya #002 kamera kareleri (ilk senaryo)

Deniz Arslan kamerasının ilk senaryosu (taksi gelir, park eder, arka kapıdan yolcu iner, sokağın üst ucuna yürür) 12 kare olarak oynatılır: `park` olayına 01–06 (22.57.46–22.58.16), `passenger_out` olayına 07–12 (23.03.02–23.03.27), 700 ms. Bu iki olayın mp4 yolu kaldırıldı; metin olayları aynen duruyor, yeni ipucu eklenmedi. Kaynak: `Docs/case2/2–13.png`, 1280×720 JPG.

## 2 Ekim 2026 — Video yalnız Dosya #001de

Kullanıcı kararı: `videoPath` (mp4) yalnız Dosya #001 için kalır. Dosya #002 ve sonraki vakalar kamera görüntüsünü yalnız kare dizisiyle (`framePaths`/`frameTimes`/`frameMs`) canlandırır. `case002_2258.mp4` silindi.

## 2 Ekim 2026 — CCTV arşivi yeni görünüm

Referansa göre: sol rayda dürbün + "CCTV KAYITLARI" başlığı ve yönerge, kamera kartları (simge karosu, kamera adı, yer, aralık; kırmızı nokta yalnız "henüz incelenmedi" — her kamerada aynı). Sağda kamera başlığı + aralık, saat sütunlu döküm (saat ayrı anahtardan ya da "08.27 — metin" kalıbından ayrılır), sinyal/bozulma satırları her vakada aynı kırmızıyla. Tabletin sağ üstünde kapat (X) dosyaya döner. Referanstaki "Konum" alanı ve ayrı tarih veride olmadığı için eklenmedi; üst çubuk simgeleri önceki karar gereği yok. Kamera simgesi (`Icons/cctv`) gelene kadar dürbün kullanılır.

## 2 Ekim 2026 — Masa referansına hizalama

Masa etiketlerinin konumu/boyutu referans görselden ölçülerek birebir alındı, üst çubuk eylemleri 96 px genişledi, konum satırına iğne simgesi eklendi. Arka plan hâlâ parça parça (oda plakası + nesne atlası + ülke penceresi, piksel stili); referansın resim stiline birebir geçiş için kullanıcıdan yazısız temiz masa görseli bekleniyor.

## 2 Ekim 2026 — Tek parça masa plakası

Masa artık kullanıcının verdiği tek görsel (`Art/OfficeDesk.png`, 1672×941, yazısız). Oda plakası + nesne atlası + ülke penceresi katmanları kaldırıldı; eşya adları boş işaretçi olarak kalıyor (katman ve test düzeni bozulmasın diye). Bedeli: (1) basılan eşyanın masadan kalkma efekti artık görünmez, (2) pencere her ülkede aynı genel şehir manzarası. İkisi de geri istenirse eşya kesitleri (saydam PNG) ve ülke başına pencere görseli gerekir. Eski `OfficeRoomV2`/`OfficeProps` dünya seçici arka planında hâlâ kullanıldığı için silinmedi.

## 2 Ekim 2026 — Boş masa + eşya kesitleri

Masa artık boş plaka (`Art/OfficeDesk.png`) + altı ayrı eşya kesiti (`Art/Desk/`: lamba, tepsi, telefon, dosya, tablet, evrak yığını). Kalkma efekti yerine tıklama hissi: basınca eşya 0,07 sn içinde hafifçe gömülür ve kararır, bırakınca küçük bir sekmeyle oturur. Lamba da ayrı katman ama düğmesi yok, tepki vermez. Konumlar görselde birleştirilerek seçildi (`Docs/desk_props/preview.jpg`).

## 2 Ekim 2026 — Tam masa görseline dönüş, ışıkla tıklama hissi

Kullanıcı kararı: ayrı kesitler gerçekçi durmadı (lamba bardağın üstünde vb.). Masa yine ilk tam görsel. Tıklama hissi geometriyle değil ışıkla: basınca eşyanın üstüne yumuşak sıcak bir ışık lekesi 0,12 sn içinde düşer, bırakınca 0,45 sn içinde söner. Her eşyada aynı. Kesitler `Docs/desk_props/` altında saklı, oyunda kullanılmıyor.

## 2 Ekim 2026 — Ayarlar yeni görünüm

Referansa göre: simgeli bölüm başlığı + açıklama, radyo daireli seçenek kartları (sağda simge ya da ses çubukları, 4 çubuk = seviye), alt çubukta simgeli "Varsayılanlara Dön" ve birincil "Kaydet". Referanstaki "Görüntü" sekmesi eklenmedi: oyunda ekran ayarı yok, boş sekme olurdu. Eksik simgeler (chat, music, gamepad, refresh, check) gelene kadar mevcut simgelere düşer (`KarineUI.IconOr`).

## 2 Ekim 2026 — Vakalar sayfası yeni görünüm

Referans görsele göre: büyük başlık; harita simgeli, yüzdeli "Genel ilerleme" kutusu; daha uzun ülke kartları, kilit ortada; şeridin sağında ileri oku; ülke bandında bayrak ve "N VAKA" damgası; polaroid vaka kartlarında sol üstte sıra rozeti, kilitli kartlar koyu, altta düğme gibi durum şeridi (etkin olan pas kırmızısı); alttaki adım çizgisinde kilitli adımlar kilit noktası taşır. Referanstaki "Türkiye'yi tamamla / Özel içerik açılır" kupası **eklenmedi**: böyle bir içerik yok, oyuncuya olmayan bir şey vaat edilmez. Durum: `[~]`, Unity'de gözle bakılmadı.

## 2 Ekim 2026 — Kariyer / İstatistikler yeni görünüm

Masa üstü tablet yerine tam ekran panel: üstte logo + "Kariyer / İstatistikler" + kapat; solda Bora profil kartı ve üç sekme (Genel İstatistikler, Vaka Geçmişi, Dosya Arşivi); sağda Genel İlerleme (dünya, vaka, yüzde), dört sayaç (değerlendirilen dosya, uygun bulunan, kurum güveni, kurumsal incelemede), halka grafikli Vaka İstatistikleri ve En Aktif Dünyalar. Referanstaki "İncelenen delil", "Görüşme yapılan kişi", "Yazılan rapor", "Sicil no", "Göreve başlama", "Görüşmeler/Raporlar/Başarımlar" sekmeleri **eklenmedi** — kariyer verisi bunları tutmuyor, uydurma sayı gösterilmez. Durum: `[~]`.

## 2 Ekim 2026 — Dosya #002 CCTV: Deniz ve Emre sekansları kapandı

Kamera 04'ün kare dizileri kişi sekansı olarak üretiliyor. **Deniz** (`park`, `passenger_out`, altışar kare) ve **Emre** (`second_in` 5, `contact` 4, `second_out` 5 kare; 500 ms) kullanıcı tarafından tamamlandı sayıldı. Emre kareleri yeni sahne çiziminde; Deniz kareleriyle sahne farkı ve son iki anda görüntü bozulması bilinçli olarak kabul edildi. Kalan: Kerem (`third_in`, `third_out`), `dispute`, `patrol` — şimdilik yalnız metin. Durum `[~]`, Play Mode'da izlenmedi.

## Dosya #002 CCTV akışı kapandı (2 Ekim 2026)

Kerem (`third_in` 5 kare, `third_out` 3 kare) ve devriye (`patrol` 3 kare) eklendi. Ambulans/görevli karesi "POLİS" yazısı taşıdığı için alınmadı; yerine yazısız, armasız devriye aracı karesi kondu. `dispute` bilerek yalnız metin kalır. Kareler Play Mode'da henüz görülmedi.

## Dosya #003 "Son Teslimat" yazıldı (2 Ekim 2026)

Kullanıcı senaryosu: şüpheli ölüm, fail yok — ölüm kaza (karbonmonoksit), olay yerini değiştiren ve delil alan kişi (Ozan) ölüme sebep olmadı. Eğitim eğrisi: 001 yalan ≠ fail, 002 yaralayan ≠ parayı alan, 003 olay yerini değiştiren ≠ ölüme sebep olan. Kullanıcı ilkesi: oyuncu sonucu faksa kadar bilmemeli; her kişi kendini korur, yanlış yollar (iş kazası, Seda, Barış, Ozan) savunulabilir olup kanıtla kapanır. Rapor dört sütunlu kaldı; fail ve yöntem sütunu başlıkları vakadan gelir. Zimmet raporda ayrı soru değil, güdüdür. Dosya #002 → #003 zinciri bağlandı. Ayrıntı: `CASE003_DESIGN.md`.

## Oyunu derinleştiren yedi iş (2 Ekim 2026)

Kullanıcı isteği: CCTV dışında oyunu ileri taşıyacak yedi öneri, önerilen sırayla. Hepsi oyuncu güdümlü soruşturma kuralına bağlı kaldı: hiçbiri sıradaki adımı söylemez, hükmü onaylamaz, faksa kadar sonucu sızdırmaz.

1. **Zaman çizelgesi** — zaten vardı (açılan ipuçlarını oyuncu kendisi iğneler, saat sırasıyla dizilir). Yeni iş açılmadı; önerideki "yok" ifadesi yanlıştı.
2. **Defter.** Dosyada yeni "Defter" sekmesi. Oyuncu karşılaştırma ekranında iki kaynağı yan yana koyup üç hükümden birini işler: *Çelişiyor / Örtüşüyor / Sorulacak*. Serbest yazı yok — telefonda klavye ekranı kapatıyordu, arama ekranında da aynı gerekçeyle kaldırılmıştı. Aynı çift için tek not tutulur; aynı hükme yeniden dokunmak notu siler. Oyun hükmün doğru olup olmadığını hiçbir zaman söylemez.
3. **Satır altı çizme.** Belge metinleri cümle cümle dokunulur; dokunulan cümlenin altı çizilir ve defterde kaynağıyla toplanır. Görüşme dökümleri bunun dışında (zaten soru-cevap olarak ayrık). Önerideki "sorguda alıntı olarak öne sürme" **yapılmadı**: öne sürme kuralı "adı geçiyorsa" kaynak düzeyinde işliyor, cümle düzeyine inmek her vakanın verisini yeniden yazmak demekti.
4. **Dosyanın akıbeti.** Faks artık rapordaki kişinin ve ikinci sorumluluğun gerçek dünyadaki sonucunu anlatır (`epilogueKey`). Yanlış suçlamanın bedeli somut: gözaltı, donan pay, kaybolan iz. Üç vakanın bütün seçenekleri yazıldı. Faks gelmeden hiçbir yerde görünmez. Aynı işte bir eksik kapandı: dört sütunlu vakalarda faks dördüncü sütunu hiç değerlendirmiyordu, artık değerlendiriyor.
5. **Ses.** Vaka ortamı altyapısı vardı ama hiçbir vaka kullanmıyordu. İki yeni sentez döngü: `room_night` (Dosya #002, gece sokağı) ve `room_rain` (Dosya #003, yağmurlu depo sokağı). Dosya #001 gündüz geçtiği için ortam almadı. Geçen araç ya da gök gürültüsü gibi tek seferlik olay konmadı: döngüde aynı yerde tekrar eden olay saat gibi duyulur.
6. **Kariyer bağı.** Sicil kaydı artık her raporun akıbetini ve dördüncü sütunu da gösterir: kariyer yalnız puan değil, raporların insanlara ne yaptığıdır. **Vakalar arası geri dönen isim kalıcı olarak kaldırıldı** (kullanıcı kararı): eski dosyanın faksı oyuncu yeni dosyaya geçtikten sonra gelir ve oyuncu eski dosyaya geri dönebilir. Yeni dosya yazılırken eskisinin sonucu belli değildir; eski bir kişiyi yeni dosyada geri getirmek o sonucu ya varsaymak ya da sızdırmak olur.
7. **Baskı.** Görüşme belli kaynaklar dosyaya girdiğinde, henüz istenmemişse kapanabilir (`closesAfterRead`, `closedNoteKey`). Süre gerçek saatle değil soruşturmanın ilerleyişiyle işler. Doğrulayıcı, doğru sonucun dayandığı hiçbir kaynağın ve önkoşul zincirinin kapanmasına izin vermez. Uygulama: Dosya #003'te Barış Tümer'in ikinci görüşmesi, oyuncu Ozan'la üçüncü görüşmeyi bitirdiğinde hâlâ istenmemişse kapanır ("şehir dışına çıktı"). Barış'ın jeneratör tanıklığı kaybolabilir, ama aynı gerçeği jeneratör incelemesi taşır.

Durum: hepsi `[~]`. Testlerle sınandı, Unity'de gözle görülmedi.

## Kare hızı ayarı (2 Ekim 2026)

Ayarlar → Oynanış sekmesine sabit kare hızı seçimi eklendi: 30 / 60 / 120 fps, varsayılan 60. Önceden hiçbir değer verilmediği için mobilde Unity varsayılanı olan 30 fps'te kalıyordu. Ekranın desteklemediği hız seçilirse cihaz kendi tavanında kalır.

## Efekt katmanı kuralları (2 Ekim 2026)
- Hiçbir efekt oyun durumunu kodlamaz: far, lamba titremesi, buhar, uzak sesler kendi rastgele saatinde olur; okuma, soru, kayıt onları tetiklemez.
- Kâğıt yıpranması belge kimliğinden türetilir; herkes aynı belgeyi aynı görür, yıpranma önem belirtmez.
- Görüşme müziği yanıtlara, yalana veya baskıya göre değişmez; duruş ve göz kırpma herkes için aynı rastgele zamanlamadadır.
- Faksın ilk okunuştaki vurgusu (ışık halkası) yalnız evrağın kendisine düşer, bir kaynağa yönlendirmez.
- Parlamalar saniyede 3'ün altında; "hareketi azalt" ve Efektler: Kapalı tüm hareketli katmanı kapatır.
- Vaka verisi `deskHour` ve `weather` alanlarıyla masanın saatini ve havasını belirler.

## 2 Ekim 2026 — Sahne katmanı (H–O) ve değişmez kural

- Defterde işaret konunca çekilen kırmızı ip **doğrulama değildir**: her işarette (çelişki, uyum, soru) aynıdır, doğru/yanlış söylemez.
- Kayıt öne sürülünce bardaktaki halka her kayıtta aynıdır; tepki ipucu taşımaz.
- "Masa telefonu boşuna çalar" önerisi, oyuncuyu telefona yönlendireceği için **uzak ofiste çalan telefon** sesine çevrildi.
- Geçiş yalnız nereden gelindiğini söyler (dokunulan eşya), gidilen yerde ne olduğunu söylemez.
- Güven rozeti mührü yalnız oyuncunun zaten gördüğü durum değiştiğinde iner; gizli değer göstermez.
- Şekil işaretleri yalnız oyuncunun kendi seçtiği işarete ve faksın açık sonucuna eklenir.
- Bölüm/vaka seçicide yalnız kart çekilme hareketi eklendi; yerleşim kullanıcıya ait olduğu için değişmedi.

## 2 Ekim 2026 — Sahne katmanı (P–X) ve değişmez kural

- Masa müziğinin üst katmanı yalnız masada geçen süreyle açılır; soruşturmanın ilerleyişine bağlanmadı, çünkü o bağ ilerlemeyi ele verirdi.
- Kişiye özel bekleme döngüsü kişinin kimliğinden türer; soru, yanıt ya da kayıtla değişmez.
- Bant aşınması her kayıtta aynı kuralla (izlenme sayısı) artar; önemli kaydı işaret etmez.
- Terminal bozulması rastgele satırda ve rastgele zamanda olur, metni hiçbir zaman kalıcı değiştirmez.
- Ses betimlemesi yalnız duyulan sesi adlandırır.
- Masaya sol altta kapanmış dosyalar rafı eklendi (yalnız kapanmış vaka varsa).

## 2 Ekim 2026 — Cila katmanı (Y–AF)

- Kayıt bırakma sesi kaydın **türünü** söyler (fotoğraf, dosya, torba), önemini değil; doğru kaynakta da yemde de aynıdır.
- Kare tekrarı ve zaman damgası titremesi her kayıtta aynı olasılıkla olur.
- Sigara dumanı kişinin verisindeki sabit bir işarettir; soru, yanıt ya da öne sürülen kayıtla değişmez.
- Yankı yalnız oyuncunun kendi açtığı önceki sayfanın ilk satırını tekrar eder; seçme yapmaz.
- Okurken sesin kısılması belgenin açık olmasına bağlıdır, içeriğine değil.
- Masaya lamba düğmesi, telefon kablosu, şehir ışıkları ve takvim eklendi; takvim sürüklenebilir. Masa yerleşimi kullanıcınındır; istenirse kaldırılır.
- Yazı boyu üç kademe yerine %90–150 arası kaydırıcı oldu.

## 2 Ekim 2026 — Reklam yerleri

**Kullanıcı:** "reklamlardan gelir ettiğimizi varsayarsak reklamları çoğaltmak gerekiyor" → "önerilerini yap".

- Yeni araya giren reklam yerleri: **vaka başı** (teklif kabulünden sonra) ve **menüye dönüş**. Soruşturma, sorgu, CCTV ve sinematik yine kapalı.
- Tüm araya giren reklamlar tek sıklık sınırını paylaşır (240 s). Vaka başı ve menü reklamı ilk vakasını kapatmamış oyuncuya gösterilmez: ilk izlenim reklamla bozulmaz.
- Ödüllü görünüm yalnız masaya dokunur (lamba rengi); oynanışa, ipucuna, kaynağa hiçbir etkisi yoktur.
- Ödüllü bekleme atlama yalnız 60 saniye ve üstü bekleyişte teklif edilir. Bugünkü bekleyişler kısa olduğundan görünmez. Bekleyişleri uzatmak bir tasarım kararıdır (zamanla kısıtlanan ilerleme) ve kullanıcıya sorulmadan yapılmaz.
- Başta önerilen "faksı hemen al" düşürüldü: faks zaten 5–10 saniyede geliyor; atlatılacak bir bekleyiş yok.

## 2 Ekim 2026 — Reklam ağı AdMob, banner yok

**Kullanıcı:** "Banner yok reklam ağı ne varsa olur." Ağ olarak AdMob seçildi: herkese açık test kimlikleriyle hesap açılmadan denenebiliyor, AB izin formu (UMP) hazır. Banner hiçbir ekranda yok.

## 3 Ekim 2026 — Arayüz baştan: tasarım sistemi panosu

Kullanıcı ChatGPT ile ürettiği panoyu (`Docs/Reference/UI_DESIGN_SYSTEM_2026-10.png`) yeni kanon olarak verdi; ekranlar sırayla bu dile taşınacak.
- Palet: zemin `#0E0F11`, cam `#1A1D22`, yükseltilmiş panel `#252A32`, kenar `#3A4048` (yeni `Border` tokenı), birincil metin `#E6E1D3`, ikincil metin `#9AA0A6`, vurgu kehribar `#D99A2B`, tehlike `#C64040`; kâğıt `#E8D9B7`, manila `#C9A06B`.
- Turkuaz kalktı; etkin/dijital vurgu da kehribar.
- Düğmeler dokulu görsel değil, düz yüzey: ana = dolu kehribar + koyu yazı; ikincil = koyu cam + gri kenar; ghost = zeminsiz + açık kenar; kilitli = sönük gri.
- Tipografi (dar, kalın başlık yazısı) ve simge seti henüz koda girmedi; ekranlarla birlikte gelecek.

## 3 Ekim 2026 — Tablet çerçevesi kaldırıldı

Masadaki CCTV tableti artık bir çerçeve değil, yalnız bir giriş noktasıdır: dokununca CCTV arşivi dosya ekranlarıyla aynı üst şeritli tam ekran açılır ve "Masaya dön" ile masaya döner. Dosyada Gezin'den açılan CCTV "Dosyaya dön" der. Tablet içinde açılan ekran kalmadı; soruşturma talepleri ve kariyer kaydı kendi yenilemelerinde tam ekrana geçecek.

## 3 Ekim 2026 — Rapor iki iddiaya indi: kim ve ne ile

Kullanıcı kararı: gerekçeli rapor artık **şüpheli** ve **ne ile / nasıl** (vaka verisindeki `methods`) adımlarından oluşur. Gözaltı sütunu olan vakada (`custody`) üçüncü adım kalır.
- "Belirleyici kanıt" adımı kalktı. Seçenekleri dosyanın kaynaklarıydı; kaynağın dayanağını yine kaynak olarak istemek döngüseldi.
- Her adımın altındaki dayanak kaynak seçicisi kalktı. Bütün ifade satırlarını ve CCTV sinyal boşluklarını listeliyordu, seçilen kişiyle ilgisiz kayıtlar da geliyordu ve oyuncuyu şaşırtıyordu.
- Gerekçe gizli kalır: doğru seçenek, onu gösteren kaynak (`supportingSourceIds`) soruşturmada açılmışsa "uygun" sayılır. Açılmadan yapılan doğru tahmin "eksik", yanlış kişi "asılsız suçlama"dır. Oyuncuya hangi kaynağın sayıldığı söylenmez.
- Motor: `Investigation.SubmitReport(suspect, method, custody)`. Eski `SubmitFinalReport` ve kayıtlardaki `proofId` / kaynak alanları eski kayıtlar ve doğrulayıcı için okunmaya devam eder.

## 3 Ekim 2026 — Açılış dizisi oyun geneli

Dünyanın ilk dosyası da sonraki dosyalar gibi gelir: dosya masaya bırakılır, bildirimler iner, kabulden sonra saat kartı ve mekân kareleri oynar. İlk dosyada departman müdürünün kısa karşılama notu bırakılışa iliştirilir; ipucu vermez. Bırakılışta ve açılışta büyük vaka başlığı yazılmaz (üst şeritte ve teklifte zaten var). Saat kartı vakanın tarihini de yazar (vaka verisinde `deskDate`).

## 3 Ekim 2026 — Dosya onayla kapanır, akış düğmeyle ilerler

Kullanıcı: "Dosyanın kapanması için gerçekten doğruyu bulmak gerekiyor." Rapor gönderilince dosya değerlendirmeye gider. Yalnız onay faksı geldiğinde, faks ilk açılırken o dosyanın "KAPANDI" kartı oynar. Yanlış ya da yeniden açılan raporda kart yoktur; oyuncu yine sonraki vakaya geçebilir.
- Vaka sonrası hiçbir adım kendiliğinden ilerlemez: rapor özetinde ve masadaki panelde tek bir sıradaki adım düğmesi vardır ("Değerlendirme bekleniyor…", "Değerlendirme faksını aç", "Yeni görevi aç"). Bildirimler ek olarak masada kalır.
- Yeni görev tepside ayrı bir kart olarak gelmez; düğme yeni dosyayı doğrudan bırakır ve dosya bir kez, teklif evrakında kabul edilir.
- Rapor sonrası video kaldırıldı; zarf ve mühürden sonra doğrudan rapor özeti gelir.

## 3 Ekim 2026 — Raporda yalnız dinlenen kişiler

Kullanıcı: rapor için seçenekler oyunun takibine göre gelmeli. Şüpheli ve gözaltı seçeneklerinden yalnız görüşmesi okunmuş kişiler sunulur. Kural veriden türetilir: seçeneğin kimliği (ya da `_` önü) bir görüşme düğümüne denk geliyorsa o düğüm okunmuş olmalı. Kişi olmayan seçenekler ("Kimse — kaza", "Olay yeri değiştirilmedi") ve yöntemler hep açıktır: yöntemleri metinde geçince açmak, yalnız doğru seçeneği geç açacağı için ipucu olurdu. Genel ikinci soru etiketi "Ne ile / nasıl yaptı?" oldu.

## 3 Ekim 2026 — Sorgu odası ve masadaki "yeni" işareti

- Sorgu odası maketle (`UI_INTERVIEW`) yeniden kuruldu. Öne sürme halinde balon kimlik kartının altına kayar, sağı kayıt paneli alır; seçilen kayıt kâğıt olarak okunur, "Öne sür" ile ya da sürükleyerek verilir.
- Kullanıcı: "Sunduğumuz kaydı tekrar seçenek olarak gösterme." Bir soruda öne sürülen kayıt, cevap işe yarasa da yaramasa da o soruda bir daha listelenmez. Kişi bazında gizlenmez: aynı kayıt aynı kişinin başka sorusunda gerekebilir (örn. Dosya #002, `camera#second_out`).
- Kullanıcı: "Baktığımızda yanmasın, bakmadıklarımız için yansın." Masadaki halka yalnız bakılmamış bir şeyi olan eşyada atar; erişilecek bir şey yoksa (henüz CCTV yok) sessizdir. Telefon yeni talep sayısını gösterir. Bu işaret yalnız "burada yeni bir şey var" der; sırayı ya da önemi söylemez.


## 3 Ekim 2026 — Oyunun kendi reklam izni penceresi kaldırıldı

Oyun artık reklam için kendi onay penceresini göstermiyor ve ayarlarda reklam izni satırı yok. Reklamlar kişiselleştirilmemiş olarak gösterilir; bölgeye göre gereken onayı (GDPR, iOS ATT) reklam ağının ve sistemin kendi formları sorar, bu da AdMob/LevelPlay entegrasyonunda (Faz 5) bağlanır. Reklam yerleri ve sıklık kuralları değişmedi: soruşturmanın içi kapalı, ödüllü reklam yalnız oyuncunun isteğiyle.

## 4 Ekim 2026 — Sorgu odası
- Kişi koltuğa oturur: yeri oda görselinin koordinatlarından (koltuk ortası, masa kenarı) hesaplanır; masanın ön kenarı kişinin önüne çizilir.
- Masadaki su bardağı (ve kayıt sunulunca dalgalanması) ile kayıt cihazı (makara, ışık, süre sayacı) kaldırıldı — kullanıcı kararı.
- Dosya #002/#003 portreleri olduğu gibi kalır; #001'in piksel tarzına çevrilmez (kullanıcı kararı).

## 4 Ekim 2026 — Dosya #005: fail seçmeyen rapor

Kullanıcı kararı: #005'te rapor "kim yaptı" sormaz, geceyi kurdurur. Motor değişmedi; üç sütunun etiketleri vakaya özel: `verdicts` = kaybolmanın niteliği, `methods` = bağlantılı kişi ve bağlantının niteliği (Murat'ı doğru seçip yanlış niteliği yükleyen yanılır), `custody` = fiziksel saldırı. Senaryodaki Barış/Seda/Tolga, önceki vakalardaki adlarla çakıştığı için Kaan/Nermin/Levent oldu. 00.08 kaydı ve emanet kaydı kendiliğinden düşmez; sırasıyla Murat'ın üçüncü görüşmesindeki ve Aslı'nın ikinci görüşmesindeki bir soruyla erişilebilir olur. Vaka Kasım 2027'de geçer (#004'ün Ocak 2027'sinden sonra; #004 öne alınmadı); hafta günleri tutsun diye kaybolma gecesi 14 Kasım 2027 Pazar.

## 4 Ekim 2026 — Dosya #006: iki katmanlı kayboluş

Kullanıcı kararı: #006 aynı vakayı tekrar oynatmasın. Senaryo #005'e çok benzediği için kişi (Defne Yalın), meslek (hakediş uzmanı), usulsüzlük (sahte taşeron hakedişi) ve semt (Üsküdar) değişti; çakışan adlar yenilendi. Rapor dört soru yerine motorun üç sütununa oturdu: ilk kayboluş, 23.41 mesajını gönderen, takibi organize eden ("üçüncü kişi müdahalesi" sorusu ilk ikisinden çıktığı için düştü). "Kendi isteğiyle" tek başına doğru ama eksiktir. Mantık açığı kapandı: gizli yeni hattın numarası şirket kayıtlarında olamazdı; Defne'nin 21.40 "deneme" mesajı ve kilidi kendisinin kapatması numarayı Cem'e verir. Kayıp kişinin kendisi sonda görüşme listesine girer.



## 4 Ekim 2026 — İnceleme izni ve Dosya #007 "Kül Payı"

Yeni mekanik: bazı kayıtlar (araç, şirket kartı, servis koridoru) doğrudan istenemez; oyuncu talebe elindeki kaynaklardan iki dayanak ekler. Dayanaklar tanımlı yollardan birini karşılıyorsa kayıt normal gecikmeyle gelir; karşılamıyorsa aynı gecikmeden sonra yalnız "Ek dayanak gerekiyor" döner. Hangi kaynağın eksik olduğu söylenmez, deneme cezası yoktur. Bu, değişmeyen kuralı korur: sistem oyuncuyu yönlendirmez, yalnız gerekçeyi sınar.

#007 kararları: önceki vakalarla çakışan adlar değişti (ölen ortak Kerem Sarıgül, eksper Oğuz Tezcan). Rapor motorun üç sütununa oturdu: yaralanmadan kim sorumlu ve nasıl / yangının niteliği / yangının ilk amacı. "Düştü" ve "delilleri örtmek" kolay ama yanlış: yara 6 cm yuvarlak cisimle uyumlu, priz ve teminat artışı kavgadan önce. Melis motifli, yalan söylemiş ve olay yerinde bulunmuş ama fail değil. Gerçek kurum adları kullanılmadı (Adli Muayene Raporu, soruşturma birimi). Sokak kamerası arka sokağı görmez; olay saati (02.14) suç saati (≈00.05) değildir.

## 4 Ekim 2026 — Dosya #008 "Kopya": olay bağlantısı ve ayrılan dosya

Kullanıcı kararı: #008'de dosyanın kapsamını oyuncu belirler. Olay B, Olay A'nın ilk görüşmesinden sonra gelen evraklara düşer; Olay C ancak A ↔ B bağlantısı onaylanınca gelir. Bağlantı ve arşiv taraması inceleme izni motorunu kullanır: iki dayanak, yetersizse nötr ret, ipucu yok. Arşivden gelen 2006 kaydı ilk açılışta "YENİDEN AÇILDI" kartıyla masaya düşer (kullanıcı: "eski dosya tekrar açıldı, sana verildi" hissi). Rapor sonunda damga "AYRILDI"dır; arşive oynanmayan "SOĞUK DOSYA — UMUT YÜCEL / 2006" kartı eklenir, ileride bir vakada dönebilir.

Düzeltmeler: oyun takvimi #007'den sonra Kasım 2028; eski olay 2006 (22 yıl korunur), yaşlar buna göre (öğrenciler 2006'da 18–20). Çakışan adlar değişti: Kaan Erdem → Tolga Saran, Aylin Keskin → Aylin Karataş, Mert → Umut Yücel. Fotoğrafı çeken 15 yaşındaki Burak'tır; negatif bu yüzden onda. Zeynep'in motivi: kasette "kimse aramasın" diyen sesi; ortaklık süreci. Zeynep kutuyu Aylin'in telefonda söylemesinden bilir. 2006 gerçeğini kaset ve Ferhat taşır; dosyadaki eksik belgeler kimin aldırdığını söylemeden durur. Rapor üç sütuna oturdu: zarfları gönderen / eve giren / 2006'da ne oldu; "amaç" ve "yeniden inceleme" epilog ve ayrılma sonuna geçti. Olay panosu ayrı bir ekran olarak tasarım akışına bırakıldı; pano asla puanlanmayacak.

## 4 Ekim 2026 — Dosya #009 "Emanet": soruşturma hattı

Kullanıcı kararı: oyuncu kurumsal kaynağı sınırlı kullanır. Hatlar önceden tanımlı (Arda / Emanet / Operasyon); aynı anda iki hat açık kalır. Kilitlenmeyi önlemek için kapatılan hat yeniden açılabilir; bedeli bekleme süresidir, ceza puanı yoktur. Hatlar açılırken iki dayanak ister, ret nötrdür. Vaka sonunda bir kez iç denetim zarfı düşer; doğru rapor "yetki genişletildi" satırı kazandırır (henüz oynanışa etkisi yok, ileriki vakalara bağlanabilir).

Düzeltmeler: çakışan adlar değişti — Nermin Kaya → Nesrin Eker, Ali Rıza Demir → Ali Rıza Toprak. Takvim Aralık 2028. Hakan paketi iç kapıdan (kamera dışı) alır; silme Nesrin'in amirlik terminalinde açık kalan hesabıyla Ceren tarafından yapılır, Nesrin o saatte binada yoktur. Araçtaki iz delil nakliyle açıklanır (tuzak). Rapor üç sütun + laboratuvar delili.

## 4 Ekim 2026 — Dosya #010 "Kırık Zincir": Türkiye finali

Kullanıcı kararları: adlar önceki vakalara benzemez (Onur Keskin → Serkan Ural, Seda → Pelin Kor); vakalar bağımsız kalır, "hepsinin arkasında aynı örgüt" yok; sonraki ülke Birleşik Krallık; bölüm sonu sinematiği eklenir. Mantık düzeltmeleri: Nehir'in gelişi 02.04 mesajı + misafir kartıyla açıklanır; Baran DNA ile değil özel güvenlik kimlik kartı iz kaydıyla eşleşir; Pelin'in telefon kaydı yalnız ölüm aralığını kapsar. Rekonstrüksiyon ipucu vermez: kartlar karışık havuzda, doğruluk yalnız faksta görünür.

## CCTV kare sayısı olaya göre (4 Ekim 2026)

Bir CCTV olayı beş kare olmak zorunda değil. Tek hareketli olaylar (giriş, çıkış, el uzatma) 3 kareyle anlatılır; beş kare yalnız olayın kendisi uzun ya da çok adımlıysa kullanılır. Kareler 4 saniye arayla zaman damgası alır ve olay metninden fazlasını göstermez (ör. #010 `pour`'da nesne seçilemez).

## CCTV karesi yalnız kritik olaylara, görüntüde yazı yok (5 Ekim 2026)

Kare dizisi her CCTV olayına konmaz; yalnız soruşturmanın seyrini değiştiren anlara (bırakılan/alınan nesne, masa altında kalan eller, cam kırma, şüpheli ışık). Rutin giriş/çıkış ve ara adımlar (zile basma) metin kalır. Görüntüye kamera adı, saat veya etiket gömülmez: oyun dili değişince sabit kalırdı; kamera etiketini oyun yerelleştirip kendisi çizer. Gelen görselde yazı varsa kırpılır. Kayıtta adı geçmeyen kişinin yüzü karede seçilmez.

## Tek portre tarzı: #001–#010 Gemini (5 Ekim 2026)

ChatGPT ile yapılmış portreler (#001–#006'nın büyük kısmı) Gemini ile, #007 sonrası tarzda yeniden çizildi: önden bakış, şeffaf zemin, dokulu mürekkep. Bundan sonra tüm portreler bu tarzda. Masa eşyaları da masa yüzeyiyle tek parça hareket eder; eşyalar ile masa arasında paralaks yoktur.

## Birleşik Krallık bölümü başladı (5 Ekim 2026)

Kullanıcı kararları: Bora bube Londra şubesine (Southwark) **tayin** edilir; kariyer ve güven devam eder. Metin Türkçe, adlar İngiliz, kurumlar kurmaca. Vakalar bağımsız, #020 bölüm finali. Plan: [UK_CHAPTER_PLAN.md](UK_CHAPTER_PLAN.md); ilk dosya: [CASE011_DESIGN.md](CASE011_DESIGN.md) — onay bekliyor, veri yazılmadı. Açık çelişki: Kullanıcı on dosyada karar kıldı; `WORLD_OPENINGS.md` düzeltildi.
