# bube — oyun ana bağlamı ve tasarım sözleşmesi

**Kaynak:** “Mobil Dedektif Oyunu Fikri” konuşmasının erişilebilen 68 metin turu. Bu kayıt, konuşmanın son ve kullanıcı tarafından onaylanmış kararlarını önceki fikirlerden ayırır. Tam metin `CONVERSATION_TEXT_1.md`–`CONVERSATION_TEXT_4.md` dosyalarında korunur. Görsel çıktılar metin dökümünden alınamadı; ayrıca paylaşılan görseller `Docs/References/` ve masa referansı `Assets/Bube/Resources/Bube/DeskReference.png` içindedir.

**Karar sırası:** Kullanıcının son düzeltmesi > o düzeltmeyi kabul eden yanıt > eski öneri. Aşağıdaki “sonraya bırakıldı” fikirler otomatik üretim talimatı veya kesin özellik değildir.

## Oyuncu ne yapıyor?

Oyuncu, bube Polis soruşturma birimindeki Bora olarak bir suç olayını **kendisine gelen bilgilerden çözmeye çalışır**. Sahadaki ve uzman ekipler gerekli teknik çalışmayı yapabilir; oyuncunun asıl işi dosyayı okumak, insanlarla konuşmak, kayıtlara bakmak, ifadeler arasındaki farkları fark etmek, yeni bilgiyle tekrar soru sormak ve gerekçeli bir sonuç raporu yazmaktır. Oyun oyuncunun belleğini sınamaz: bütün konuşma ve belgeler masadaki dosyadan tekrar okunur. Oyun oyuncu adına “şu kişi yalan söylüyor”, “bu çelişkiyi buldun” veya “fail belli” demez.

Oyuncunun beklenen zihinsel döngüsü **söyleneni kaydet → başka kaynakla karşılaştır → hangi bilginin iddia, gözlem ya da bağımsız doğrulama olduğunu düşün → uygun soruyu sor → sonucunu kanıtla savun** şeklindedir. Beden dili ve mimik ipucu olabilir; otomatik yalan makinesi değildir. Masum kişi de korktuğu için yalan söyleyebilir veya tedirgin görünebilir.

Oyuncu her adımda bir görev oku izlemez. Bilgi yeni kişileri, belgeleri ve soruları doğal olarak açar. Dosyanın genişlemesini kendisi görür. Görüşme, belge ve terminal arasında kendi sırasını seçebilir; bazı hikâye eşikleri mantıksal olarak zorunludur. Yanlış yorum yapabilir, yanlış/yetersiz sonuç gönderebilir. İlk vakada bu rapor iade edilir ve oyuncu masaya dönerek tekrar düşünür. Gizli state grafiği akışı çıkmaza sokmaz.

## Her vakada değişmeyen oyun omurgası

Bu kurallar Dosya #001'e özel değildir; sonraki bütün vakaların tasarım sözleşmesidir. Oyuncu dedektif gibi **aktif bilgi toplar ve muhakeme eder**. Karakterin söylediği bir kaynak, yalnızca dünyada o kaynağı kullanılabilir kılar; oyun otomatik olarak sonraki ekrana geçmez ve görev oku vermez. Oyuncu masaya/dosyaya döner, kaynağı kendisi açar, güvenilirliğini tartar, başka ifadelerle karşılaştırır ve takip sorusunu seçer. Bir kaynağın açılması suçluyu veya doğru sırayı ilan etmez.

Her vaka aynı temel eylem dilini kullanır: **dosya/ihbarı al → ulaşılabilir kişilere ve kaynaklara kendi seçtiği sırayla bak → yeni bilgiyle açılan soruları ve raporları araştır → eski kayıtlara dönüp karşılaştır → gerekçeli sonuç yaz → dosyayı kapat/sonucunu gör**. Hikâyenin gizli bilgi grafiği mantıklı eşikleri yönetir; oyuncu grafiği görmez. Gerekli bilgiye erişim için çıkmaz oluşmaz, fakat oyuncunun yorumu yanlış olabilir. Oyun şüpheliyi, çelişkiyi, yalanı veya mimikten suçluluğu otomatik etiketlemez.

**Her vakada sabit olan mekanik ilişki**, ekranlarda her zaman aynı belge sayısı, aynı üç kişi veya CCTV bulunması değildir. Vaka verisi hangi kişilerin, kaynakların, soru seçeneklerinin, belirsizliklerin ve rapor alanlarının mevcut olduğunu belirler. Sonraki vakalar bu temele yeni soruşturma yetenekleri ekler; önceki yetenekler kaybolmaz. Yetki ile belirli bir vakadaki kaynak varlığı ayrı tutulur. Zorluk daha çok düğmeye basmaktan değil, bilgi bağlantılarını ve eksik/güvensiz kayıtları çözmekten gelir. Metin CCTV bu omurganın ilk örneğidir; ileride bazı zor vakalardaki animasyonlu CCTV de aynı oyuncu inisiyatifi ve kanıt değerlendirme kuralını izler.

**İçerik üretim kabulü:** Bir vaka ancak oyuncunun kendi seçimiyle en az bir anlamlı karşılaştırma ve yeni bilgiye dayalı takip kararı gerektiriyorsa bu oyunun vakasıdır. Sadece sıradaki belgeyi açıp okutarak final düğmesini açan lineer liste, hedeflenen dedektiflik oynanışı değildir. Yeni vaka temel motor kodunu değiştirmeden vaka verisiyle kurulmalıdır; Dosya #002 bu davranışı kanıtlamak için kullanılacaktır.

## Çekirdek oyun yapısı

- Platform mobil, yön yatay. Serbest yürüme, joystick veya her vaka için 3D mekân üretme zorunluluğu yok. Oyunun merkezi, *Papers, Please* benzeri fiziksel ve işlevsel **2D/pixel-art masa**dır.
- Masa klasik seviye seçme menüsü yerine Bora'nın iş yeridir. İlk vakada yalnızca işlevsel nesneler görünür: gelen evraklar, merkezde dosya, CCTV/BPS terminali; telefon ancak görüşme işlevi varsa. Dekor ve kilitli gelecek özellikler masa kalabalığı yaratmaz. “Yapılacaklar” listesi, numaralı görev yolu veya görünür ilerleme grafiği yoktur.
- Temel etkileşim: **masadaki nesneye dokun → nesne öne gelir, arka masa kararır/bulanıklaşır → okunabilir odak görünümünde işlem yap → kapat → masaya dön**. Açılmış dosyada gerçekten mevcut sayfalar/sekme içerikleri görünür. Görüşme dökümleri ve belgeler sonra tekrar okunabilir. Yan yana belge karşılaştırma ve oyuncunun satır işaretlemesi gelecekte değerlendirilecek önerilerdir; temel teslimin şartı değildir.
- Bir vakada ilk olay raporu gelir; görüşmeler yeni kişileri ve soruları, CCTV/terminal yeni kayıtları, uzman birimlerin çalışması yeni belgeleri açar. Teknik analizi mini oyun haline getirmek yerine gelen raporun anlamını oyuncu yorumlar. Her vakada aynı araçların tümü zorunlu değildir.
- Yeni mekanikler ileride oyuncunun araç kutusuna eklenebilir, eskiler kaybolmaz. **Yetki sahibi olmak** ile belirli vakada o veri kaynağının **mevcut olması** ayrıdır. Ülkeler zorluk/özellik kapısı değil, vaka ortamıdır. İstanbul'daki ilk vakada CCTV vardır. Zorluk suçun büyüklüğünden değil bilgi, belirsizlik ve bağlantıların yapısından doğar.
- Her vakayı yeni temel kod yazmadan vaka verisi, Türkçe metin ve uygun yeniden kullanılabilir varlıklarla hazırlamak hedeflenir. İkinci vaka bu mimariyi sınamak içindir. “100 vaka / 10 ülke” yalnızca varsayımsal örnekti, taahhüt edilmiş kapsam değildir.

## Bilgi ve görüşme kuralları

- Vaka için gizli bir **master truth** ve oyuncunun adım adım ulaşabileceği ayrı bilgi katmanı vardır. Gizli gerçek zaman çizelgesi başlangıçta gösterilmez.
- Bir kişinin ilk görüşmesinde oyuncunun seçebileceği sorular vardır; yanıtlar kayıt olur. Yeni bilgi yoksa aynı kişiye dönmek ilk konuşmayı yeniden oynatmaz, kısa bir tekrar yanıtı verir. Yeni kanıt gelince yalnızca ilgili takip soruları açılır. Soruların varlığı, kişinin suçlu olduğunu ima etmez.
- Soru seçeneklerinin sırası serbest olabilir. Bazı bilgiler, oyuncunun gerekli soruyu sormasıyla açılır; karaktere tıklamak tek başına bütün bilgiyi vermemelidir. Eski konuşmada “ilk görüşmedeki gerekli sorular sonunda sorulur” ilk vaka için önerildi; uygulamada çıkmaz yaratmadan sağlanmalı.
- İfadeler, kamera dökümü ve raporlar ayrı kaynaklar olarak durur. Kaynakta olmayan yorum, otomatik çelişki etiketi ve suçlu işareti eklenmez. Oyuncu belgeye dönebilir, ihtiyaç varsa satırı işaretleyebilir; hafızaya zorlanmaz.
- Görünmeyen soruşturma durumları (bilinen kişiler, açılan kayıtlar, tamamlanan sorular, yeni takip soruları, edinilmiş kanıtlar) veriyle ifade edilir. UI bu durumların teknik adlarını oyuncuya göstermez. Yeni kayıt, doğal bildirim veya okunmadı işaretiyle fark edilir; “şimdi X'i yap” emri verilmez.
- Oyuncu raporunu yeterli kanıtla savunur. İlk vakada sonuç üç alanlıdır: şüpheli, giriş yöntemi, belirleyici kanıt. Daha sonraki vakalar başka alanlar gerektirebilir; rapor şeması vakaya göre veriden gelebilmelidir.

## Metin tabanlı CCTV ve BPS

Dosya #001 CCTV **video, görüntü veya insan/ortam animasyonu oynatmaz**. Görünen şey güvenlik sistemi terminalidir: kamera kimliği/konumu, tarih-saat, incelenen aralık, REC, kanal, sinyal ve kalite bilgisi, inceleme durumu. Oyuncu incelemeyi başlatınca olay satırları bir anda topluca değil, tarama/yazma ritmiyle sırayla gelir; terminal/klavye sesleri ve hafif mikro animasyonlar atmosfer sağlar. Yeni vaka için esasen kamera meta verisi, olay satırları, kalite ve sinyal kesintileri girilir.

Görüşmede kameradan söz edilmesi yalnızca kayıt kaynağını **erişilebilir** yapar. Oyun görüşmeden otomatik CCTV ekranına geçmez, dosyaya “sıradaki adım kamera” komutu koymaz. Oyuncu görüşmeyi/dosyayı kapatır, masaya döner, terminale kendisi dokunur. Terminal masa önünde büyük, okunabilir bir odak görünümü olarak açılır; arka masa karartılıp/bulanıklaştırılır. Kayıtlar bozuk geldikçe oyuncu incelenebilir satırları netleştirir veya gerçek sinyal boşluğunu belirsiz bırakır; hangi ifadeyle karşılaştıracağını kendisi seçer.

Sinyal kalitesi yalnızca süs değildir. Düşük kalitede saat veya kimlik bilgisi güvenilir biçimde okunamayabilir; sinyal kesildiyse o aralık için kayıt **yoktur**. Glitch oyuncuyu yanlış kesin bilgi okumaya sevk etmez: belirsizlik vaka verisinde tanımlanır, arayüz onu görünür kılar. CCTV eksiksiz hakikat kaynağı değildir ve oyuncu adına ifade yorumlamaz. El kamerası bataryası Dosya #001 tasarımında yoktur. **Sonraki ve özellikle zor vakalarda** vaka tasarımı gerektirirse sınırlı, animasyonlu/canlı CCTV görüntüsü kullanılabilir. Bu bir ilerleme ödülü veya son iki vakaya otomatik bağlanan zorunlu kural değildir; vaka bazlı içerik seçimidir. Böyle bir vakada da oyuncu terminali kendisi açar, sinyal/kanıt güvenilirliğini değerlendirir ve oyun sonucu onun yerine çıkarmaz. Metin terminali yeniden kullanılabilir temel biçim olarak kalır.

## Görsel ve metin dili

- Nihai yön düşük/orta çözünürlüklü pixel-art: sınırlı palet, sade yüz, göğüs üstü sprite, silüetle ayrışan kişiler, küçük ama okunabilir mimikler. AI illüstrasyonu gibi duran portre ve arka planlar tercih edilmedi.
- Gerçek görüşme ekranında **karakter, adı/temel kimliği, söylediği cümle ve sorulabilir sorular** vardır. “Normal/Tedirgin/Savunmada” gibi geliştirici state adları, “sakin görünüyor”, “yalan söylüyor olabilir” gibi sistem yorumları veya oyuncunun yerine analiz yapan notlar görünmez. Karakterin tedirginliği suçluluk göstergesi değildir.
- Dosyalar fiziksel kâğıt ve sayfa hissi taşır, odak modunda mobilde rahat okunur. Masa, dosya, terminal ve görüşme aynı oyunun görsel diliyle tutarlı olur.
- Oyuncuya görünen ilk dil Türkçe. Kod/veri anahtarları İngilizce olabilir; oyuncu metni yerelleştirme kataloğundan gelir. `Case`, `Statement`, `Property Recovery Report` gibi oyuncu başlıklarının Türkçe karşılıkları kullanılır.
- Oyun çalışma adı **bube**, yapımcı bubeGames. Nihai oyun adı ertelendi ve değiştirilebilir olmalı. Kurum kurgusal **bube Police / bube Polis**; şehir/ülkeye göre yerel varyantlar olabilir. BPS “bube Police System” ortak sistem adıdır. Gerçek polis adı/arması veya başka gerçek kurum markası nihai varlıklarda yer almaz. Eski görsellerdeki kurum, Kadıköy ve tarih bilgileri görsel referansın içeriğidir, kanon değildir.
- Protagonist sabit **Bora**, 27, erkek, Türk, İstanbul/Beşiktaşlı ve soruşturma birimine yeni atanmış deneyimli polis. Sessiz/gözlemci, kanıt temelli ve agresif olmayan bir konuşma tonu hedeflendi. Kısa biyografisi personel kartında olabilir; uzun zorunlu giriş yoktur. İsim seçimi, Alex/Emir ve self-insert fikirleri sonradan elendi.

## Bora ve kariyer sürekliliği

Bora'nın kilitlenen biyografisi, oyuncunun kontrol ettiği alan, kariyerin ülke/XP sistemiyle ilişkisi, unvan ile departman güveni ayrımı ve gelecek önerilerinin statüsü [CAREER_AND_CHARACTER.md](CAREER_AND_CHARACTER.md) içinde ayrıntılıdır. Bu belge ana kayıtla birlikte okunur.

## DOSYA #001 — Açık Kapı: kesin vaka sözleşmesi

**Tür:** konut hırsızlığı. **Yer:** Beşiktaş, İstanbul. **Süre hedefi:** 10–15 dakika. **Mağdur:** Mert Aydın. **Yanlış şüphe:** Elif Demir. **Gerçek fail:** komşu Hasan Kaya. Temel ders: **Bir ifade kanıt değildir; bir yalan da suçluluk kanıtı değildir.** İlk dosya öğretici olur ama eğitim görevi gibi konuşmaz.

**Ekip için gizli olay çizelgesi:** Mert 08.27'de çıkar. Elif, kalan eşyaları için 11.48'de gelir ve 12.16'da ayrılırken anahtarı kapı yanındaki saksıya bırakır. Anahtarın yerini önceden bilen Hasan, Elif'i çıkarken görür, yaklaşık 12.40'ta anahtarla eve girer; dizüstü bilgisayarı, kol saatini ve nakdi alır, anahtarı da götürür. Mert 17.54'te döner ve hırsızlığı fark eder. Elif ziyaretini gizler çünkü ayrılık sonrası habersiz girişin kendisini şüpheli göstereceğinden korkar; hırsız değildir. Hasan fırsatçı hareket eder, maddi sıkıntısı vardır. Gizli saatler otomatik oyuncu çizelgesine aktarılmaz.

**Oyuncu akışı:**

1. Gelen evrakta olay raporu: kayıp bilgisayar/saat/nakit, zorlama izi yok, kapı yanında saksı var, anahtar yok. Mert erişilebilir.
2. Mert ilk görüşmede yaklaşık çıkış/dönüş saatlerini, kapıyı kilitlediğini, Elif'te anahtar olduğunu ve komşu Hasan'ı söyler. Elif ve Hasan görüşmeleri açılır; sıra oyuncuya bırakılır.
3. Elif “o gün gitmedim” der. Hasan öğlen bir kadın gördüğünü ve giriş kamerası olduğunu söyler. Hasan'ın kamera bilgisi BPS kamera arşivini masadaki terminalde erişilebilir yapar; oyuncu dosyayı/görüşmeyi kapatıp masaya döner ve terminale kendisi dokunur. Oyun onu otomatik kameraya götürmez. Elif'in görüşmesini tamamlamak kamera için ayrıca zorunlu değildir.
4. Kamera dökümü: 08.27 erkek çıkar; 11.48 kadın girer; 12.16 kadın çıkar; 12.31 sinyal zayıflar; 12.37 kesilir; 13.08 geri gelir; 17.54 erkek girer. **12.37–13.08 kaydı yoktur.** Hasan'ın girişi, hırsızlık, çanta veya eşyalar kamerada açıkça görünmez. Sistem “Elif yalan söyledi” diye yorum yapmaz.
5. Kamera/Elif bilgisiyle Elif'in takip sorusu açılır. Elif ziyareti ve anahtarı saksıya bıraktığını kabul eder. Bu bilgi Mert'in anahtar takip sorusuna yol açar; Mert Hasan'ın anahtar yerini önceden gördüğünü söyler. Hasan'ın takip görüşmesinde anahtarla ilgili kaçamak cevabı şüphe doğurabilir ama suçluluk kanıtı değildir.
6. Masaya gelen **Eşya Tespit Raporu** çalınan dizüstünün seri numarası eşleşmesini ve cihazı ikinci el elektronik işletmesine getirenin Hasan olduğunu bildirir. Oyuncu yeni mağaza sistemi kullanmaz. Kesin seri numarası ve satış saati önceki konuşmada kararlaştırılmadı; içerik tutarlı olacak şekilde seçilebilir.
7. Sonuç raporunda mevcut seçenekler arasından şüpheli, giriş yöntemi ve temel kanıt seçilir. Doğru birleşim **Hasan / yedek anahtar / Eşya Tespit Raporu**. Yanlış veya yetersiz rapor ilk vakada “SONUÇ İADE EDİLDİ” ile dosyaya döner, cevap açıklanmaz. Doğru rapordan sonra dosyayı kapatma eylemi ve masaya dönüş vardır.

**İlk vakada yok:** telefon kayıtları, banka kayıtları, DNA/parmak izi laboratuvarı, otopsi, araç/sosyal medya araştırması, 3D olay yeri, video/foto CCTV, gelişmiş sorgu, yeniden açılan dosya, başka ülke, günlük vaka ve reklamla ipucu. Masadaki telefon bulunursa yalnızca görüşme işlevine hizmet eder. İlk dosyadaki mekanikler ileride büyüyebilecek sistemlerin tamamını göstermek zorunda değildir.

## Daha sonraki oyun için yön, kesin ilk teslim değil

- Mesleki ilerleme yalnız XP/sayı değildir; performans ve departman güveniyle artan sorumluluk/yetki hissi hedeflenir. Yanlış suçlama ile sıradan soru denemesi aynı ağırlıkta cezalandırılmaz. Düşük performans oyuncuyu kalıcı çıkmaza sokmamalıdır. Ünvan adları ve hangi araçların ne zaman açılacağı kesinleştirilmedi; ilk vakada CCTV bulunduğu için buna aykırı kilit uygulanmaz.
- Vaka çeşitliliği önemlidir: her olayda aynı üç şüpheli, aynı CCTV ve aynı çözüm yolu olmamalı. Yeni araçlar eski bilgi okuma/karşılaştırma becerisinin üzerine eklenir; her vakada hepsi kullanılmaz. Bağımsız vakalarla uzaktan ilişkili tekrar eden izlerin birleşimi, arşiv ve yeniden açılan dosyalar uzun vadeli fikirlerdir, ilk milestone kapsamı değildir.
- Kullanıcı reklam geliri istedi. Ödüllü ipucu dikkat edilecek kayıt/saat/ifade karşılaştırmasına yönlendirir, faili söylemez. Geçiş reklamı ancak dosya kapanışı gibi doğal aralarda, kontrollü sıklıkla düşünülür. Ek reklam fikirleri (günlük vaka, detaylı performans raporu vb.) konuşmada öneri düzeyindedir; ilk vakaya aktarılmaz.

## Mevcut Unity prototipine karşı kabul ölçütleri

- Vaka metinlerini yalnız açıp okumak, seçilmiş görüşme soruları ve oyuncunun bilgi edinme eylemi yerine geçmez. Soru bazlı, koşullu ve tekrarda farklı yanıt veren veri şeması gereklidir.
- Oyuncu Elif/Hasan arasında ve kamera sonrası uygun takip kollarında seçim yapabilmeli; bilgiye dayanmadıkça açılmayan içerik açık bir görev zinciri olarak sunulmamalı.
- Kamera yalnız masadaki terminalden açılmalı; tanık sözü ekran değiştirmemeli. Kayıt tek seferde düz paragraf olarak değil odak terminalinde satır satır ve veriyle tanımlı sinyal belirsizliğiyle incelenmeli. İlk dosya metin; sonraki zor vakada isteğe bağlı animasyonlu kayıt biçimi ancak vaka gerektirirse eklenebilir.
- Dosya, bağımsız ve tekrar okunabilir fiziksel sayfalara ayrılmalı; yeni belgeler gelen evrakta fark edilmeli. Oyuncu önceki ifadeyi bulabilmeli.
- Görüşme ekranı referans sprite diline geçmeli; geliştirici state etiketleri ve otomatik analiz notları olmamalı.
- Üç alanlı sonuç ve yanlış rapor iadesi hem UI hem veri hem tam baştan sona Play Mode ile doğrulanmalı.
- Save/yeni oyun/geri dönüş/Android veya iOS yatay kullanım doğrulanmalı. Dosya #002 yalnız veri ve yerelleştirme ile eklenerek mimari sınanmalı.

Mevcut işlerin sırası ve doğrulanan tamamlanma durumu `Docs/ROADMAP.md` dosyasında tutulur. Bu belge kararların kanonik kaydıdır; tam metin arşivi geçmiş fikirleri kontrol etmek içindir.
