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

