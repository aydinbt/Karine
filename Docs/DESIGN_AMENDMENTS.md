# Karine — sonraki tasarım düzeltmeleri

## Oyun adı: Karine (25 Eylül 2026)

Oyunun mağazada görünecek adı **Karine** olarak karara bağlandı. Karine, hukukta "dolaylı kanıttan çıkarılan sonuç" demektir; oyuncunun ifadeleri ve kayıtları karşılaştırıp gerekçeli bir sonuca varması olan oyun tezinin birebir karşılığıdır.

`bube` çalışma adı olarak sona erdi. Stüdyo adı **bubeGames**, oyun içi kurmaca kurum adı **bube Police / bube Polis / BPS** olduğu gibi kalır — bunlar oyun adı değil, dünya kurgusunun parçasıdır. Arayüzdeki "bube Polis" metinleri değişmez.

Uygulanan yerler: `config.json` `title`, `ProjectSettings` `productName`, `applicationIdentifier` (`com.bubedigital.karine`), `tr.json` `about.body`. Depo: `github.com/aydinbt/Karine`; çalışma klasörü `bubeGame/karine-mobile`. Aday havuzu, eleme ölçütleri ve müsaitlik kontrol listesi [NAMING.md](NAMING.md) içinde saklanır.

### Oyun içi kurum adı değişmiyor (25 Eylül 2026)

Oyun adının Karine olması üzerine, oyun içindeki 16 "bube Polis / bube Police" metninin de Karine'ye çevrilmesi değerlendirildi ve **çevrilmemesine karar verildi**. Kurmaca kurum **bube Polis / bube Police**, terminal **BPS — bube Police System** olduğu gibi kalır.

Gerekçe: Karine soyut bir hukuk terimidir ("dolaylı kanıttan çıkarılan sonuç"); oyun başlığı olarak tezi taşır ama kurum adı olarak kuruluş hissi vermez. Oyun başlığının kurumu adlandırmak zorunda olmadığı kabul edildi. Kurum adı sorusu kapanmış değildir; ileride yeniden açılırsa bu madde güncellenir.

## Dünya açılışları ve yedi ülke (24 Eylül 2026)

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

Dört gerçek Unity sahnesi vardır: `BootScene` teknik yükleme, `MainMenuScene` atmosferik ana menü, `OfficeScene` soruşturma masasının kalıcı merkezi ve `InterviewScene` fiziksel görüşme odası. Dosya, gelen evrak, tablet/BPS/CCTV ve sonuç raporu yeni sahne yüklemez; `OfficeScene` içinde nesneye dokunma → arka planı karartma → nesneyi öne alma → odak arayüzü açma diliyle çalışır. Görüşme bitince tekrar ofise dönülür. İlk vakada kullanılmayan BPS yetkileri sahte kilitli menü olarak gösterilmez; yetki gerçekten açıldığında araç eklenir. Özel olay yeri sahneleri ileride yalnız içerik gerektirirse açılır.

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

Son kontrol adımındaki üç iddianın seçilmiş dayanakları dokunularak aynı ekran üzerinde açılır. Açılan kâğıt kart, belge/BPS için tam metni, görüşme için oyuncunun seçtiği **tek soru-cevabı**, CCTV için seçilmiş saatli satırı gösterir. Rapor kaynak seçicisinde görüşmeler artık kişi başlığı yerine ayrı cevap turları olarak seçilebilir; vaka verisindeki kişi düzeyindeki destek tanımı bu turları da kapsar, belirli satır/cevap tanımları ise yalnız birebir eşleşir. Kart kapandığında rapor ve seçimler yerinde kalır; bu okuma yeni kaynak keşfetmez veya doğruluk değerlendirmesi vermez.

Raporun dayanak açılır listesinde Tümü / Belgeler / İfadeler / CCTV filtreleri bulunur. Belgeler sekmesi erişilmiş fiziksel belgeleri ve BPS metin kayıtlarını kapsar; İfadeler oyuncunun gerçekten aldığı tekil cevapları, CCTV incelenmiş saatli satırları gösterir. Boş kategori pasiftir. Filtre yalnız görünür listeyi değiştirir; seçilmiş kaynak ve rapor kararı korunur, kaynağın doğruluğuna ilişkin ipucu verilmez.

Uzun vakalar için aynı seçicide kelime veya saat araması bulunur. Arama yalnız erişilmiş belgenin tam metninde, alınmış ifade soru-cevabında ve incelenmiş CCTV satırında çalışır; `11:48` ile `11.48` eşdeğer kabul edilir. Kategori filtresiyle birlikte uygulanır; sonuç sayısı ve boş durum açıkça gösterilir. Yatay telefon düzeninde arama alanı, temizleme düğmesi, kategori sekmeleri ve kaynak satırları en az 48 birim dokunma yüksekliğiyle tasarlanır. Bu, oyuncunun yerine kaynak ilişkilendirmesi yapmaz.

Görüşmede kaynak öne sürme listesi de erişilmiş kaynakların tam metninde kelime/saat arar ve belge, ifade, CCTV filtreleri sunar. Dar telefon sütununda filtreler iki satıra ayrılır; listedeki adlar kısalır, seçilen kaynağın tam metni mevcut referans kartından açılır. Seçilen kaynak ve “Kaynağı Öne Sür” eylemi uzun listenin üstünde kalır. Aynı soruya dönüldüğünde arama/filtre korunur; yeni soruda sıfırlanır. Bu düzenin gerçek cihaz klavyesi, kaydırma ve dokunma davranışı Play Mode/cihaz testinde ayrıca doğrulanmalıdır.

Bir görüşmede birden çok açık soru konusu varsa son seçilen konunun adı ve soru sayısı, sağdaki soru listesinin üzerinde sabit kalır. Konu düğmeleri ve sorular aynı kaydırılabilir alanda kalır; başlığa dokunulduğunda sabit etiket güncellenir. Sabit başlık soru veya vaka bilgisi üretmez. Konu düğmeleri en az 48 birim dokunma yüksekliğindedir; dar yatay telefonda gerçek okunurluk ve kaydırma Play Mode/cihaz kontrolü gerektirir.

Görüşme sırasında o kişiyle gerçekten kaydedilmiş soru-cevaplar, sağ sütundaki Sorular / Geçmiş geçişiyle okunabilir. Geçmiş, konuşmanın sırasını ve tam metinleri gösteren kaydırılabilir bir paneldir; yeni bilgi, yorum veya doğru/yanlış işareti üretmez. İlk cevap kaydedilene kadar geçiş görünmez. Geçmiş açılınca soru listesi ve seçili konu durumu korunur; görüşme odasına yeniden girildiğinde Sorular açılır. Düğmeler en az 48 birimdir; küçük yatay telefon ve gerçek dokunma davranışı ayrıca sınanır.

## CCTV video oynatıcısının yerleşimi (24 Eylül 2026)

Klip açıldığında oynatıcı tabletin iç ekranını kaplar; arşiv başlığı ve döküm görünümü arka planda kalır. Ayrı kamera başlığı kaldırılır: kamera adı ve REC görüntü üstünde, Döküme dön sağ üstte, filigranı örten Geç sağ altta durur. Alt şeritte yalnız saatli kayıt açıklaması ve Oynat/Duraklat, Kare ileri, Başa al kontrolleri bulunur. Video kırpılmadan 16:9 oranında mümkün olan en geniş alana oturur; sahnedeki kişiler ve kanıt olabilecek ayrıntılar kesilmez. Bu yerleşim farklı yatay telefon oranlarında ve gerçek dokunmayla ayrıca doğrulanacaktır.

## 25 Eylül 2026 — Gerçek resmî kurum adı kullanılmaz

**Karar (kullanıcı):** Oyun içinde **hiçbir yerde** gerçek resmî kurum, kuruluş veya mevzuat adı kullanılmaz. Kurum kurgusaldır: **bube Polis / BPS**. Kurumsal gönderici "ilgili birim", "kurumsal değerlendirme" gibi genel ifadelerle anılır.

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

**Önerilen kurgusal ad:** Kurum **bube POLİS (BPS)**, departman **3. SORUŞTURMA MASASI**, terminal yazısı **"BPS KAYIT SİSTEMİ"**. Üst şeritteki "bube POLİS / İSTANBUL · BEŞİKTAŞ" olduğu gibi kalır.

**Durum:** Bu yazı `DeskReference.png`'nin **içine gömülü**, kodda değil; ancak yeni görselle değişir.

## 25 Eylül 2026 — Masaya dosya bırakılışı sinematik video oldu

**Kullanıcı Gemini ile `case_animation.mov` üretti (1920×1080, 10 sn).** Dosyanın masaya bırakılışı artık elle çizilmiş kayan klasör animasyonu değil, bu video.

**Uygulama:**
- Video H.264 mp4'e çevrilip `Assets/StreamingAssets/Bube/case001_arrival.mp4` olarak kondu (Android `.mov`'u güvenilir oynatmaz). `config.json` → `deskArrivalVideo`.
- **Üretici filigranı "Geç" düğmesiyle örtülür**, dünya sinematiğindeki gibi. Filigran yeri artık veriden gelir: `CornerMark { x, y, w, h }`, filmin kendi karesine **oranlı** (0..1). `PositionIntroSkip` 1280×720'ye sabitlenmek yerine filmin ekrandaki gerçek dikdörtgenini hesaplar, böylece her video kendi filigran yerini söyleyebilir ve telefonun eni ne olursa olsun düğme doğru yere oturur. `world01` değerleri eskisiyle piksel eşdeğer.
- Video oynatılamazsa elle çizilmiş animasyon devreye girer; oyun bu andan hiçbir koşulda yoksun kalmaz. "Geç" ile videonun bitişi aynı yere gelir, bayrak ikinci çağrıyı yutar.

**Geçiş: karart, sonra göz aç.** Karartma **videonun kendi içinde** (kullanıcı ekledi); masa da siyahtan 1,25 saniyede açılır. İki görüntü birbirine çarpmaz. Açılma boyunca kaplayan gölge dokunmaları tutar, oyuncu göremediği bir şeye basamaz.

**(Geri alındı — 25 Eylül 2026)** Bir ara masa arka planı videonun son karesi yapılmıştı. Kullanıcı **eski masa görselini geri istedi**; `Desk()` yine `DeskReference.png` yüklüyor ve künye şeridi üstte. Videonun son karesiyle masanın birebir aynı olmaması artık sorun değil, çünkü aradaki geçiş karartmayla yapılıyor.

**Kurum adı bitti — yerine hiçbir şey konmadı.** Terminaldeki "EMNİYET SİSTEMİ" `DeskReference.png`'den silindi (ekranın kendi arka planıyla kapatıldı). Kısa süre yerine kurgusal bir ad ("BPS KAYIT SİSTEMİ") koddan yazıldı, ama **kullanıcı kararıyla o da kaldırıldı**: ekranda yalnız "CCTV ARŞİVİ" kalıyor ve terminalin ne olduğu zaten anlaşılıyor. Kurum adı yazmamak, kurgusal kurum adı yazmaktan daha temiz — yazılmayan ad ihlal edemez.

Üst künye şeridi %97 saydamdı ve altındaki gömülü kurum yazısı hayalet gibi sızıyordu; şerit tamamen opak yapıldı.

**Terminal ekranındaki arma da silindi** (kullanıcı kararı): ekranda yalnız "CCTV ARŞİVİ" kutusu kalıyor. Kenarlardan interpolasyon denendi ama armanın ucu seçilen dikdörtgenin dışına taştığı için dikey izler bıraktı; ekranın o bölgesi zaten neredeyse düz olduğundan çevresinden örneklenen tek renkle doldurmak temiz sonuç verdi. **Dosya kapağındaki arma kalıyor** — kullanıcı kararı, uydurma görünüyor.

**(Çözüldü — 25 Eylül 2026)** Kullanıcı videoyu yeniden üretti (`case_fix_animation.mov`, 2,83 sn): terminalde yalnız "CCTV ARŞİVİ" var, arma ve kurum adı yok, karartma da videonun içinde. Koddaki karartma kaldırıldı — aynı işi iki kez yapmanın anlamı yok. Filigran yine sağ altta (0,906 / 0,833) ve "Geç" düğmesiyle örtülüyor; `CornerMark` veriden geldiği için tek satır değişti.

**Durum `[~]`:** Testler geçiyor ama **Play Mode'da gözle doğrulanmadı** — karartma/açılma geçişinin akıcılığı görülmeli.

## Görüşme ekranı düzeltmeleri (25 Eylül 2026)

**Yinelenen konu başlığı kaldırıldı.** Soru listesinin üstünde sabit bir konu şeridi vardı; hemen altındaki açık grubun başlığı aynı metni yazıyordu, yani "OLAY GÜNÜ · 1" iki kez görünüyordu. Şerit kaldırıldı; grup başlıkları zaten hem etiket hem de katlama düğmesi.

**Görüşülmüş kişide düğme "GÖRÜŞMEYE DÖN" diyor.** Kart "● Görüşüldü" derken düğmenin "GÖRÜŞMEYE BAŞLA" demesi, yeni bir görüşme açılacağı izlenimi veriyordu. Yeni anahtar `interview.resume`. Düğme **devre dışı bırakılmadı**: oyuncu geçmişi okumak ya da sonradan açılan soruları sormak için dönebilmeli. Yeni soru kalmadığında görüşme ekranı zaten `interview.noNewInfo` gösteriyor — bu, oyuncuya sıradaki adımı söylemeden durumu bildiren doğru yer.

**Dar eylem sütununda düğme metni sarıyor.** Kartın eylem sütunu %30 genişlikte; "İfade alınmasını iste" tek satıra sığmayıp kırpılıyordu. `FitActionButton` son düğmeyi sarmalı yapıyor ve yazı basamağını düşürüyor.
