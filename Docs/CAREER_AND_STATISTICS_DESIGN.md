# bube — kariyer değerlendirmesi ve İstatistikler tasarımı

**23 Eylül 2026 · tasarım kaydı, uygulama işi değil.** Bu belge kullanıcının “önce yapıyı oluşturalım, sonra koda çevirelim” kararını uygular. Kesinleşmiş oyun ilkeleri ile dengeleme önerilerini ayırır.

## Kesin oyun akışı

1. Bora şüpheliyi, yöntemi ve dayanak gösterdiği kanıtı seçip kesin raporunu ilgili soruşturma birimine gönderir. Dosya kapanır; oyuncu aynı koşuda bu raporu geri alamaz.
2. “Bölüm tamamlandı” sayfası yalnız gönderilen iddiayı ve incelenen kaynakları gösterir. Raporun doğruluğunu veya kariyer etkisini söylemez.
3. Rapor gönderildikten sonra oyun akışındaki doğal bir masa anında sonuç faksı Gelen Evraklar'a düşer; sol üstte okunmamış evrak bildirimi görünür. Geliş zamanı katı bir saniyeye veya belirli bir vaka geçişine bağlı tasarım şartı değildir. Oyuncunun yaptığı işi kesmez.
4. Oyuncu faksı açınca birim kişi, yöntem ve kanıt gerekçesini ayrı değerlendirir. Belge oyuncunun iddiasının hangi yönünün desteklenip desteklenmediğini kurumsal dille açıklar; yanlış seçimde doğru faili bir cevap anahtarı gibi doğrudan söylemez. Kurumsal sonuç kariyer geçmişine bir kez işlenir.
5. İstatistikler, bu işlenmiş faksı ve birim güvenindeki değişimi gösterir. Faks beklerken veya okunmamışken gizli başarı/başarısızlık istatistiklerden sızmaz; yalnız “inceleme bekliyor / yeni faks var” durumu gösterilir.

İhbar eden kişi otomatik aklanmaz. Bir yalan otomatik suçluluk sayılmaz. Faksın değerlendirmesi vakada **gerçek fail**, **olay yöntemi** ve **bağımsız kanıt** için yazılmış gizli gerçeğe dayanır; kişinin ihbar sahibi/tanık/komşu rolü sonuç kuralı değildir. Bu yapı her vakada aynı arayüzle çalışmalı, fakat vaka gerçeği ve değerlendirme koşulları vaka verisiyle belirlenmelidir.

## Kariyer kaydında ayrı kavramlar

- **Birim güveni:** Son kurumsal değerlendirmelerin yarattığı değişken itimat. Arka planda sayısal saklanabilir; oyuncu öncelikle nitel durumunu ve yükseliş/düşüş yönünü görür.
- **Unvan ve yetki:** Bora'nın kalıcı mesleki konumu ve açılmış araçları. Birkaç olumsuz faks otomatik olarak unvanı silmez veya daha önce edinilen araçları kaldırmaz. Kesin unvan adları ve terfi eşikleri henüz seçilmedi.
- **Kariyer geçmişi:** Her değerlendirilen vaka için Bora'nın gönderdiği iddia, birimin değerlendirmesi, güven değişimi ve tarih. Eski kayıtlar silinmeden okunabilir.
- **Aktif dosya durumu:** Açık, rapor gönderilmiş/kurumda incelemede, değerlendirilmiş. Bu, güven veya unvan değildir.

Mevcut prototipte güven 60'tan başlar; tam doğru rapor +5, herhangi bir yanlış rapor −15 ve sıfır güven kariyer sonudur. **Bu sayılar dengeleme taslağıdır.** Uzun vadede “doğru kişi ama zayıf gerekçe” ile “masum kişiyi fail ilan etme” aynı ağırlıkta tutulmamalıdır. Önerilen değerlendirme katmanları:

| Birim sonucu | Örnek | Önerilen yön |
| --- | --- | --- |
| Desteklendi | Kişi, yöntem ve belirleyici kanıt uyumlu | Güven artar |
| Gerekçe yetersiz | Kişi doğru; yöntem veya kanıt iddiayı taşımıyor | Küçük güven kaybı |
| Hatalı suçlama | Yanlış kişi fail ilan edildi | Daha büyük güven kaybı |

Bu katmanların kesin puanları, bazı vakalarda “fail belirlenemedi” gibi ihtiyatlı bir raporun geçerli olup olmayacağı ve terfi eşikleri henüz **karar değildir**. Dosya #001 için ilk oynanışta kullanılan +5/−15 kuralı, bu tasarımın nihai dengesi sayılmaz. Faks bir sonuç ve gerekçe verir; ekranda soru sorma hızı, tıklama sayısı veya yanlış sorular için XP yağmuru yoktur.

## İstatistikler ekranı

İstatistikler, bube'nin aynı IBM Plex Mono fontu, koyu BPS tableti ve sınırlı paletiyle sunulur. Genel yönetim paneli gibi çok sayıda metrik dökülmez. İlk görünümde yalnız şu bilgiler yeterlidir:

1. **Bora / görev durumu / mevcut unvan** — unvan sistemi tamamlanana kadar sahte terfi göstergesi konmaz.
2. **Birim güveni ve yönü** — ör. `GÜVENİLİR ↑` veya `GÖZETİM ALTINDA ↓`, bunu değiştiren son faksla birlikte. Ham `65/100`, `+5 XP` veya eksi puan oyuncuya gösterilmez.
3. **Değerlendirilen dosyalar** — desteklenen, gerekçesi yetersiz/hatalı ve inceleme bekleyen adetleri birbirinden ayrılır. Bekleyen dosya gizlice başarılı/başarısız sayılmaz.
4. **Personel sicili** — dosya adı, kurumsal değerlendirme, güvenin yönü ve belge tarihi. Satır açılınca faks ve Bora'nın gönderdiği rapor yeniden okunabilir; sayısal hesap arka planda kalır.

Faks açılınca “İstatistiklere bak” eylemi bulunabilir. Ana menüde İstatistikler girişi ancak bu ekran ve kalıcı geçmiş gerçekten çalıştığında açılır. Masadan açılıyorsa yine tablet metaforuyla açılır; fiziksel dosya yalnız vaka belgeleri içindir. Güven sıfıra inerse kariyer sonu görünümü ve geçmiş okunabilir kalır; yeni bir koşu başlatma ayrı bir oyuncu kararıdır.

## Kurumsal değerlendirme ritüeli

Dosya kapağının kapanması, masaya dönüş, kısa sessizlik, faks/printer sesi, Gelen Evraklar üzerindeki okunmamış işareti ve oyuncunun belgeyi **kendisi** açması aynı tekrar eden ritüelin parçalarıdır. Bildirim belgeyi zorla açmaz. Ses varlığı henüz yoksa olay kancası yeterlidir. Faks fiziksel evraktır; İstatistikler ve görüşme talepleri tablette yer alır.

Faks “DOĞRU/YANLIŞ” oyun kartı değildir. Üstte gönderilen raporun incelemeye alındığı, altta kişi, yöntem ve kanıtın destek düzeyi ile birimin kanaati yer alır. Yanlış fail seçildiyse belge, o kişiye yöneltilen suçlamanın neden doğrulanmadığını açıklar; başka bir kişinin adını otomatik cevap olarak vermez. Oyuncu olayın tamamını sonraki kayıtlar, yeni deliller veya ileride yeniden açılan vaka üzerinden anlayabilir. Kurum güveninin yeni nitel durumu ve personel sicili bağlantısı belgenin sonundadır.

Faksın tam saniyesi veya ikinci vaka öncesi/sonrası gelişi oyun kuralı değildir. Bölüm özetinin ardından oyuncuya nefes alanı bırakılır; masa, yeni dosya veya başka bir doğal etkileşim sırasında faks gelir. Bildirim dikkat çeker ama akışı bölmez. Sonraki dosyaya erişim faksı zorla okutmak için kilitlenmez. Sabit yedi saniyelik mevcut prototip davranışı, bu ritmin denenebilecek bir uygulamasıdır; nihai tasarım şartı değildir.

Gelen Evraklar, değerlendirme dışında adli rapor, yeni delil, personel yazısı, terfi ve yeni görevlendirmeyi de taşıyabilecek bir sistem olarak tasarlanır. Her belgenin tekil kimliği, kaynağı, ilişkili dosyası, okunma durumu ve isteğe bağlı eylemi vardır. Aynı faks kayıttan yüklemede tekrar üretilmez; güven değişikliği yalnız bir kez uygulanır.

## Veri ve güvenilirlik kuralları — kod öncesi sözleşme

- Her gönderim kalıcı `caseId` ve raporlanan kişi/yöntem/kanıt kimlikleriyle saklanır.
- Faks olayı tekil kimlik taşır; uygulama kapanıp açılsa veya ekran yeniden açılsa güven değişimi ve istatistik kaydı **bir kez** uygulanır.
- Faksın planlanan geliş zamanı kayıtlıdır. Uygulama kapalıyken süre dolarsa bir sonraki açılışta okunmamış evrak görünür.
- İstatistikler yalnız kurumsal olarak açıklanmış sonucu okur. Gizli `correct` bilgisi rapor gönderiminde UI'a veya sayaçlara çıkmaz.
- Vaka kataloğu ve kariyer kaydı ayrı tutulur; yeni vaka eklemek temel soruşturma/istatistik kodunu değiştirmez.
- Yeniden başlatılan kariyer ile mevcut koşunun kaydı birbirine karışmaz. Eski kariyeri arşivleme/silme davranışı ayrıca tasarlanacaktır.

## Uygulamadan önce açık kararlar

- Üç sonuç katmanı ve güven değişimlerinin kesin puanları.
- Sıfır güvenin hikâyedeki kesin adı: **Aktif soruşturma görevi sonlandırıldı.** Bora 27 yaşında olduğundan düşük performansla kariyer bitişi “emeklilik” diye sunulmaz. Başarılı uzun kariyer sonunda emeklilik ayrı bir final olabilir.
- Başarılı dosyaların unvana/yetkiye ne zaman yansıyacağı ve unvan listesi.
- Birden çok rapor değerlendirmesi aynı zamanda beklerse faksların sıra ve gösterim kuralı.
- Dosya #002'nin gerçek içeriği; vakalar arası ritmi uçtan uca değerlendirmek için gereklidir.

## Uygulama durumu

Kalıcı faks geçmişi, üç kademeli değerlendirme, ayrı unvan alanı, nitel güven durumu ve ana menüden erişilen personel sicili kodlandı. Faks artık gerçek ikinci vaka olmadan da doğal gecikmeyle Gelen Evraklar'a ulaşabiliyor; yanlış suçlama belgesi doğru faili doğrudan açıklamıyor. Mevcut eski kayıttaki son faks geçmişe taşınır. Bu ilk altyapı Unity 6000.3.17f1 ile yalıtılmış proje kopyasında derleme ve içerik doğrulamasından geçti. Açık Editor'da görsel/dokunma ve gerçek cihaz kayıt testleri henüz yapılmadı. Kariyer unvanlarının terfi koşulları, daha fazla vaka verisi ve yeniden açılan vaka akışı sonraki tasarım/uygulama işleridir.
