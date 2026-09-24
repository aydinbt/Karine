# bube — görsel yön ve arayüz referansları

**Karar tarihi:** 23 Eylül 2026. Kullanıcının paylaştığı iki görsel, uygulanacak arayüzün sanat ve yerleşim referansıdır; üzerlerindeki metin ve markalar oyun içeriği olarak kabul edilmez.

## Görüşme ekranı

**Referans:** [Elif görüşme konsepti](References/InterviewConcept-Elif.png)

- Düşük/orta çözünürlüklü pixel-art; sade yüz geometrisi, sınırlı palet ve silüetle kolay ayrışan karakterler. Göğüs üstü sprite görünümü; az ama okunabilir mimik. Yaş, cinsiyet ve kişi ayrımı küçük piksel farklarıyla da anlaşılmalı.
- Gerçek oyun ekranında yalnızca karakter sprite'ı, adı ve temel kimlik bilgileri, söylediği cümle ve o anda sorulabilen sorular görünür. Masa/oda arka planı odağı karakterden almaz.
- Alttaki **“İfade Durumları (Örnek)” şeridi**, `Normal`, `Düşünüyor`, `Tedirgin`, `Savunmada`, `Sinirli` etiketleri ve yan yana durum örnekleri çıkarılır. Bu durumlar varsa yalnızca içerik/sprite yönetiminde saklanır.
- Soldaki **“Notlar” paneli** ve `sakin görünüyor`, `zaman çizelgesinde tutarsızlıklar var`, `yalan söylüyor olabilir` gibi oyuncunun yerine çıkarım yapan yorumlar çıkarılır. Oyuncu mimik, söz ve dosya bilgisinden kendi yorumunu yapar. Tedirginlik suçluluk işareti olarak kodlanmaz.
- Görüşme soruları gerçek oyun durumuna ve edinilmiş bilgiye göre açılır. Sorunun seçilmesi yanıtı ve gerektiğinde yeni bilgiyi getirir; karakterin iç durum adı UI'da gösterilmez.

## Fiziksel dosya

**Referans:** [Dosya #001 konsepti](References/CaseFileConcept-001.png)

- Masadan açılan fiziksel dosya hissi, kâğıt katmanları, dosya sekmeleri ve okunabilir Türkçe tipografi korunacak yönü gösterir. İçerik bölümleri, yalnızca gerçekten mevcut belge ve bulguları açar.
- Görseldeki `İstanbul Emniyet Müdürlüğü`, gerçek polis arması, `Cinayet Büro Amirliği`, Kadıköy ve 16.10.2026 tarihli olay metni **bube / Beşiktaş / konut hırsızlığı** kanonuna ait değildir. Nihai oyunda kurmaca bube kimliği ve vaka verisindeki bilgiler kullanılır. Gerçek kuruma benzeyen arma veya logo taşınmaz.
- “Görseller” gibi boş/işlevsiz sekmeler nihai arayüzde yer almaz. CCTV saatli metin/sinyal dökümü olarak çalışır; 24 Eylül 2026 kararıyla Dosya #001 için aynı sabit kameradan kısa görüntüler döküme eşlik edebilir. Kayıt boşluğu görüntüsüz kalır. Ayrıntı DESIGN_AMENDMENTS.md içinde.

## Bütünlük

Masa → fiziksel dosya → minimalist pixel-art görüşme → metin tabanlı terminal, tek bir oyunun parçaları gibi görünmeli. Bu referanslar görsel üretim aşaması gelene kadar proje belgelerinde tutulur; prototip UI'ına doğrudan arka plan olarak yapıştırılmaz.

## Dosya ekranı ve tipografi (23 Eylül 2026)

Dosya odak görünümü katmanlı kâğıt, iki sütunlu olay raporu ve Olay Raporu / Kişi İfadeleri / Kanıtlar / Görseller sekmeleri kullanır. İlk vakadaki bina çizimi Beşiktaş için oluşturulmuş temsili pixel-art varlığıdır; fotoğraf veya bağımsız kanıt olarak sunulmaz. Başlık ve metinler Türkçe karakter destekli IBM Plex Mono Regular/SemiBold ile gösterilir; fontun OFL lisansı `Docs/Licenses/IBMPlexMono-OFL.txt` içinde korunur. Referans görseldeki Kadıköy ve gerçek kurum işaretleri oyuna taşınmaz.

## Oynanabilir görüşme prototipi (23 Eylül 2026)

Görüşme ekranı arka odası, masa önü, karakter büstü, temel kimlik kutusu, yanıt kutusu ve seçilebilir sorularla kuruldu. Elif, Mert ve Hasan için ayrı, saydam arka planlı piksel portreler `Resources/Bube/Characters` altında tutulur. Kodla çizilen portreler yalnızca eksik varlıklar için yedek görünümdür. Geliştiriciye özel mimik durumları ve analitik notlar oyuncuya gösterilmez. Soru/yanıt ve açılma koşulları vaka JSON'undadır.

## Görüşme odası yerleşimi

Görüşmede karanlık, düşük/orta çözünürlüklü piksel oda arka planı kullanılır: panjurlu pencere, dosya dolabı, kapı ve öndeki masa. Kimlik kutusu solda; göğüs üstü karakter ortada; söylediği cümle ve sorular sağdadır. Altta durum şeridi veya yorum paneli yoktur. Kurum başlığı kurmaca bube kimliğini kullanır.

## Açılış ve konuşma ritmi

Ana ekran, Bora portresi ve `Oyunu Başlat` ile açılır. İlk kez başlayan oyuncu gelen dosyayı ayrıca kabul eder; bu eylemden sonra masa etkileşimleri açılır. Görüşmelerde soru ve yanıt ayrı turlardır; yeni soru yanıt okunduktan ve oyuncu devam ettikten sonra seçilir. Bu ritim sonraki vakalar için de veri odaklıdır.

## Masa ve tablet (23 Eylül 2026)

Soruşturma masasında önceki `DeskReference.png` düzeni korunur. Gelen evrak ve fiziksel dosya kâğıt görünümünde açılır. Görüşme talepleri, CCTV ve BPS kayıtları masa üstündeki tabletin ortak kabuğunda gösterilir; tablet açıldığında masa kararır, yan gezinme ve kapatma düğmesi görünür. Oyuncu kaynaklar arasında kendisi geçer. Ana menüde Bora’nın gece ofisinde oturduğu `MainMenuNight.png` kullanılır; başlık ve çalışabilir menü düğmeleri Unity arayüzündedir. Masa görselindeki eski kurum yazıları için kaynak çizim düzeltmesi hâlâ planlıdır. Tablet ekranlarında karakterin iç durumu veya oyuncu adına analiz bulunmaz.

## Bölüm tamamlandı ekranı (23 Eylül 2026)

**Referans:** [Bölüm özeti konsepti](References/ChapterSummaryConcept.png). Geniş, fiziksel klasör/kâğıt; sol tarafta olay görseli, sağda rapor bilgileri, altta kurum değerlendirmesi ve iki büyük eylem. Oyun sürümünde referanstaki “Çözüldü”, “Başarılı Soruşturma” ve itibar ödülü hemen gösterilmez: rapor yalnız ilgili birime gönderilmiş ve dosya kapanmıştır. Başarı/başarısızlık ile güven değişimi, oyuncu ikinci vakaya geçtikten yedi saniye sonra Gelen Evraklar'a düşen faksta görünür. Referanstaki şüpheli ölüm olayı, tarih ve sonuç metinleri Dosya #001 kanonu değildir.
