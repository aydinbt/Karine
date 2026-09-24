# bube — hikâye, kariyer ve sürdürülebilirlik denetimi

**23 Eylül 2026.** Kaynaklar: `MASTER_GAME_CONTEXT.md`, `DESIGN_AMENDMENTS.md`, kullanıcının “Mobil Dedektif Oyunu Fikri” konuşması ve mevcut Unity vaka verisi/kodu. Bu rapor bir tasarım denetimidir; yeni mekaniklerin tamamlandığı iddiası değildir.

## Kesin oyun omurgası

Oyuncu sabit karakter Bora olarak, geliştiricinin önceden belirlediği gerçeği araştırır. Olayı veya Bora'nın kişiliğini seçimlerle yeniden yazmaz. Masadaki fiziksel dosya, gelen evrak, tablet ve görüşme taleplerini kendisi kullanır; ifadeleri, belgeleri ve zamanları karşılaştırıp gerekçeli sonuç verir. Sistem “yalan söylüyor”, “çelişki bulundu”, “suçlu bu” veya “şimdi CCTV'ye git” demez. Tanık yalanı suçlulukla eşitlenmez. Soruşturma görünmeyen bir içerik/state graph ile tıkanmadan ilerler; bir kontrol noktası içindeki bazı kaynakların sırası serbesttir. Bilgiler tekrar okunabilir olmalıdır; oyuncudan ezber beklenmez.

Vaka formülü sabit üç kişi/sabit araç dizisi değildir. Her dosyanın kesin gerçeği, karakterlerin bildikleri/sakladıkları, kanıt zinciri ve erişim koşulları ayrı yazılır. Sonraki vakalarda yeni yetki ve kaynak türleri gelebilir; eski yetenekler kaybolmaz. Bir yetkinin kalıcı olması, her vakada o kaynağın bulunacağı anlamına gelmez. Zorluk suçun büyüklüğüyle ölçülmez. Ülke/şehir otomatik rütbe veya seviye basamağı değildir. Sohbette geçen 70/100 vaka ve 10 ülke örnektir, kilitlenmiş üretim hedefi değildir.

## Bora ve kariyer

Bora 27 yaşında, erkek, Türk; İstanbul/Beşiktaş'ta büyümüş ve birkaç yıllık polislikten sonra bube Police soruşturma birimine geçmiştir. Sakin, gözlemci, kanıta dayalı ve gereksiz agresif olmayan sabit bir karakterdir. Soyadı ve ayrıntılı geçmişi belirlenmemiştir. Kısa personel profili planlanmıştır. Oyuncu onun kimliğini değil, araştırma yöntemini ve kanıt yorumunu seçer.

Kariyer tasarımında kalıcı ünvan/yetki ile değişebilir departman güveni ayrı tutulur. Yeni ünvanlar daha karmaşık sorumluluklara ve sistem erişimine kapı açabilir; kesin ünvan adları ve eşikler kararlaştırılmamıştır. Hatalar kalıcı oynanış kilidi yaratmamalıdır. Vaka sonu profesyonel değerlendirme, desteklenmeyen suçlamalar ve departman güveni sohbetin güçlü tasarım yönüdür; puanlama formülü henüz kararlaştırılmamış ve kodlanmamıştır.

## Yanlış suçlama, tutuklama ve çözülemeyen dosya

Kesinleşen kural: oyuncu yanlış veya delilsiz sonuç sunabilir; ilk vakada ağır ceza yerine sonuç iade edilir ve araştırmaya dönülür. Gelecekte bazı dosyalar çözülemeden kalabilir, yeni delille yeniden açılabilir. Masum kişiyi hedef alan desteklenmeyen suçlama, uzun vadeli profesyonellik/departman güveni sisteminin değerlendirebileceği davranış olarak konuşuldu.

**Kesinleşmeyen nokta:** Oyuncunun “tutukla” eylemiyle yanlış kişiyi gerçekten tutuklayabildiği bir mekanik, bunun hikâye sonucu, geri alma sınırı veya puan etkisi belirlenmedi. Mevcut kodda tutuklama yok; yanlış sonuç yalnızca iade ediliyor. Bunu olmuş gibi anlatmamak ve ayrı tasarım kararı olarak ele almak gerekir.

## Dosya #001'in yazı omurgası

“Açık Kapı” öğretici ama eğitim ekranı gibi hissettirmeyen Beşiktaş konut hırsızlığıdır. Gizli gerçek: Mert evden çıkar; Elif kalan eşyalarını alıp anahtarı saksıya bırakır ve bunu saklar; Hasan anahtarın yerini bildiği için fırsatçı hırsızlık yapar; kamera kayıt boşluğu failin girişini göstermez; eşya tespit raporu Hasan'ı bağımsız kanıtla ilişkilendirir. Öğrenilen ilke “yalan ≠ suçluluk”tur. Önceki kayıtta hedef süre 10–15 dakika olarak yazılmıştır; kullanıcının son isteği daha uzun ve katmanlı anlatımdır. Yeni süre hedefi kararlaştırılmamıştır. Uzatma, boş metin eklemek yerine geri dönülen ifade, yeni sorular, koşullu yanıt ve hatırlanabilir kanıt ilişkileriyle yapılmalıdır.

## Bugünkü Unity uygulamasıyla farklar

- Vaka JSON'u dokuz düğüm içeriyor: rapor, Mert/Elif/Hasan ilk görüşmeleri, CCTV, üç takip görüşmesi, eşya tespit raporu. Soru seçenekleri ve bazı yanıt varyantları veriden geliyor.
- Görüşmede soru → Bora'nın sorusu → yanıt → yeni soru akışı var. Kişi talebi kısa bir zaman beklemesiyle hazır oluyor. Bu süre oyun içi başka eyleme bağlı değil; gerçek zaman damgasına bağlı dört saniyelik bekleme. Sohbette önerilen “başka bir soruşturma eyleminden sonra bildirim” deneyimi henüz yok.
- Oyuncunun sorduğu soruların kimlikleri kaydediliyor; **o anda gördüğü yanıtın tam metni ve varyantı kaydedilmiyor**. Dosya ekranı da bu soru-cevapların tam dökümünü göstermiyor; yalnız statik `bodyKey` özetini gösteriyor. Bu, “unutursam dosyadan tekrar bakayım” hedefiyle çelişen ana eksik.
- Kilitler çoğunlukla tamamlanmış düğüme bağlı. Elif'in belirli anahtar açıklaması ile Mert'in ilgili takip sorusu arasındaki ince eşik henüz temsil edilmiyor. Eşya tespit raporu CCTV ve takip görüşmelerinden herhangi biri tamamlanınca açılabiliyor; hedeflenen kanıt zinciri daha sıkı yazılmalı.
- Sonuç ekranı olay raporundan sonra açılabiliyor; doğru kapanış için Hasan + yedek anahtar + okunmuş eşya tespit raporu gerekiyor. Yanlış sonuç iade ediliyor. Yanlış deneme, desteklenmeyen suçlama, güven ve kariyer kaydı tutulmuyor.
- Tek vaka içeriğinin data-driven temeli var; “yeni vaka temel kod değişmeden eklenir” hedefi ikinci bir gerçek vaka ile henüz kanıtlanmadı. Uçtan uca Play Mode ve telefon testi de açık.

## Sürdürülebilir üretim kuralı

Her yeni vaka için aynı çekirdek arayüz ve sistemler kullanılmalı; üretilecek asıl içerik gizli gerçek, karakter motivasyonları, olay zaman çizelgesi, kaynaklar, koşullu diyaloglar, kanıtın gücü ve olası sonuçlardır. Görüşme, belge, metin CCTV, teknik rapor ve aynı tablet/dosya arayüzü maliyeti kontrol eder. Özel animasyonlu CCTV, sinematik veya olay yeri sahnesi nadir, vakaya gerçekten değer kattığında kullanılır. Vaka çeşitliliği araçların sayısından değil, bilgi ilişkilerinden ve oyuncunun verdiği karardan gelir.

Bir sonraki hikâye çalışmasında önce Dosya #001 için **master truth → her kişinin bildiği/sakladığı → oyuncuya açılma koşulları → soru/yanıt varyantları → yanlış çıkarımlar ve karşı kanıtlar → sonuç gerekçesi** tablosu tamamlanmalı. Ardından konuşma dökümü ve tekrar okuma uygulanmalı; yeni metin yazımı bu altyapı üzerinden genişletilmeli. Böylece uzun hikâye oyuncuyu ezber sınavına sokmadan karmaşıklaşır.

## Sonraki karar: kesin dosya kapanışı ve faks (23 Eylül 2026)

Bu denetimde anlatılan “yanlış rapor iade edilir” kuralı, kullanıcının sonraki kararıyla değişti. Gönderilen sonuç dosyayı doğru/yanlış fark etmeksizin kapatır. Bölüm özeti yalnız oyuncunun raporunu gösterir. Başarı/başarısızlık ve birim güveni etkisi yeni görevlendirme sürecinde Gelen Evraklar'a düşen kurumsal faksla bildirilir. Tekrarlanan başarısızlık kariyer sonuna götürebilir. Dosya #002 içeriği yazılmadan gerçek ikinci vaka geçişi tamamlanmış sayılmaz. Güncel ayrıntı `DESIGN_AMENDMENTS.md` ve `ROADMAP.md` içindedir.
