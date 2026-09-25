# Karine — geliştirme yol haritası

**Son durum:** 25 Eylül 2026 (vakadan bağımsız temeller: dil ayrımı, portre verisi, ses, geri tuşu, reklam dikişi)  
**Tek sayfalık durum:** `Docs/STATUS.md`  
**Sıra ve gerekçe:** `Docs/PHASE_PLAN.md`  
**Denetim ve kanıt:** `Docs/AUDIT_2026-09-25.md`  
**Dosya #001 oynanış betiği:** `Docs/PLAYTEST_001.md`  
**Testleri koşmak:** `Tools/run-tests.sh` (EditMode + PlayMode, 105 test: 98 EditMode + 7 PlayMode)  
**Kanonik oyun bağlamı:** `Docs/MASTER_GAME_CONTEXT.md` ve `Docs/DESIGN_AMENDMENTS.md`  
**Mevcut teknik gerçek:** `Docs/Architecture.md`  
**Görsel kararlar:** `Docs/VISUAL_DIRECTION.md`  
**Öncelik:** Çalışan soruşturma döngüsü. Görsel üretim ve ses, mekanikler doğrulandıktan sonra.

## Durum işaretleri

| İşaret | Anlam |
| --- | --- |
| `[ ]` | Yapılmadı. Kod yok. |
| `[~]` | Kodlandı, statik/içerik doğrulaması geçti. **Play Mode veya cihazda kanıtlanmadı.** |
| `[x]` | Unity'de veya gerçek cihazda davranışı gözlenerek doğrulandı. |

`[~]` hiçbir "Bitti ölçütü"nü karşılamaz. 25 Eylül 2026 denetimine kadar bu ayrım yoktu: "hiç yapılmadı" ile "kodlandı ama test edilmedi" maddeleri aynı `[ ]` işaretini paylaşıyordu ve bu, yol haritasını okunamaz hâle getirmişti. Aşağıdaki maddelerin metninde "… doğrulaması geçti; Play Mode/cihaz testi açık" yazan her satır artık `[~]`'dir.

## Yayın engelleri — aşama dışı, her şeyden önce

Bu maddeler hiçbir aşamanın içinde değildi ama **hepsini bloke ediyor**. Ayrıntı: `AUDIT_2026-09-25.md` A1–A3.

- [x] Depo git'e alındı ve `github.com/aydinbt/Karine`'e bağlandı; video/font dosyaları Git LFS'te. (Faz 0)
- [x] `applicationIdentifier` = `com.bubedigital.karine` (Android/iPhone/Standalone). (Faz 0)
- [x] `scriptingBackend` = IL2CPP (Android/iPhone/Standalone); API seviyesi .NET Standard 2.1. (Faz 0)
- [x] `AndroidTargetSdkVersion` = 35 olarak sabitlendi. (Faz 0)
- [ ] Android keystore yok → imzalı **mağaza** sürümü üretilemez. *(Faz 5'e taşındı: cihaza geliştirme derlemesi kurmak için gerekmiyor, Unity hata ayıklama anahtarıyla imzalar. Keystore dosyaları `.gitignore`'da; parola kullanıcıya ait.)*
- [x] `com.unity.test-framework` eklendi; `Bube.Runtime` / `Bube.Editor` / `Bube.Tests.EditMode` asmdef'leri kuruldu; var olan `Bube/Validate Content` doğrulayıcısı artık EditMode testinden çağrılıyor. **Testler Unity Editor'da koşturuldu ve geçti** (`BUBE VALIDATION PASSED`). (Faz 0)
- [x] **İçerik doğrulayıcı vaka başına ayrıldı ve ilk hatada durmayı bıraktı.** `Assets/Bube/Editor/Validation/` altında dokuz dosya; `ProjectSetup.cs` 285 → 37 satır. Yinelenen/ölü dil anahtarı, `nextCaseId` zinciri ve sütun başına tam bir `correct` kontrolleri eklendi. Tek komutla 42 test (40 EditMode + 2 PlayMode) **koşturuldu ve geçti**. (Faz 1)
- [~] **Kayıt şeması göçü yazıldı.** `SaveMigration` eski kaydı atmak yerine yükseltiyor, daha yeni sürümlü kaydı `FromFuture` olarak adlandırıp `<yol>.newer` diye yana kaldırıyor (silmiyor, üzerine yazmıyor) ve oyuncuya söylüyor. `UnknownVersionSave_IsSilentlyDiscarded_KnownDebt` kaldırıldı, yerine dört test geldi; 54 test (49 EditMode + 5 PlayMode) **koşturuldu ve geçti**. Cihazda gerçek bir eski kayıtla denenmedi → `[~]`. (Faz 3'ten alındı)
- [~] `Update()` her karede tam vaka JSON'u ayrıştırıyordu ve `Locale.Get` 577 girişte doğrusal arama yapıyordu. **Faz 2'de beş düzeltme kodlandı** (görev önbelleği, güvenli alan yazımları, rozet yazımları, yüklem temsilcileri, sözlükle indeksli `Locale`); üç assembly sıfır hata/uyarı ile derlendi ve `Locale.Get` davranışı beş EditMode testiyle sabitlendi. Güvenli alan yazımları ve açılış **PlayMode duman testiyle gözlendi** `[x]`; görev önbelleği, rozet yazımları ve temsilciler yalnız statik olarak doğrulandı `[~]` → `Docs/PLAYTEST_001.md` §5.
- [x] Oyun adı **Karine** olarak belirlendi ve `config.json`, `productName`, paket kimliği ile `about.body`'ye uygulandı. (Faz 0)

- [x] **Vaka teklifi tam ekran olmaktan çıkıp masadaki gelen evrak tepsisine taşındı.** Rozet yanıp söner, oyuncu tepsiyi kendisi açar, önizlemeyi okur ve kabul eder; kabul edilmeden masada başka hiçbir şey açılmaz. Başsız **Play Mode testiyle akış uçtan uca koşturuldu ve gözlendi**.
- [ ] **`DeskReference.png` görselinin içinde gerçek kurum adı ve arma var.** Üst şerit artık her ekranda `Desk()` tarafından örtülüyor, ama terminaldeki `EMNİYET SİSTEMİ` yazısı ve armalar duruyor. **Görselin yenilenmesi gerekiyor**; metin denetimi bunu yakalayamaz. Ayrıntı: `Docs/DESIGN_AMENDMENTS.md`.

- [x] **Dosyanın masaya bırakılışı sinematik video oldu** (`case001_arrival.mp4`); filigran "Geç" düğmesiyle örtülüyor, filigran yeri veriden geliyor. Sinematik siyaha kapanıyor, masa siyahtan açılıyor; **Play Mode'da gözlendi**.
- [ ] **Terminal ekranındaki arma yaması görünüyor.** Armanın yeri tek düz renkle dolduruldu; ekranın kendi gradyanından ayrıldığı için soluk bir dikdörtgen leke kalıyor ve CCTV kutusu sağa kaymış duruyor. Kullanıcının gönderdiği düzende kutu ekranda **ortalanmış**. Ya kaynak PNG alınacak ya da depodaki görselde kutu ortalanıp boşluk gradyanla doldurulacak. Kullanıcı kararı: **sonraya bırakıldı**.
- [x] **Sinematik video yenilendi** (2,83 sn): terminalde yalnız "CCTV ARŞİVİ", arma ve kurum adı yok, karartma videonun içinde. Filigran "Geç" düğmesiyle örtülüyor. **Play Mode'da gözlendi.**

## Her vaka için değişmeyen kabul kuralı

Oyuncu kaynakları masadan kendisi açar, ifadeleri/kayıtları kendi karşılaştırır, yeni bilgiyle takip sorusunu seçer ve raporunu kanıtla savunur. Oyun otomatik ekran geçişi, görev oku, “çelişki bulundu” veya fail işaretiyle onun yerine dedektiflik yapmaz. Sonraki vakalar aynı omurgaya yeni kaynak/yetki ekler; aynı sırayı ve aynı araçları tekrarlamak zorunda değildir. `MASTER_GAME_CONTEXT.md` bu kuralın ana kaydıdır.

## Aşama 1 — Temel yapı

**Durum: sürüyor.** Kaynak: `MASTER_GAME_CONTEXT.md` içindeki AŞAMA 1. Unity 6000.3.17f1 projesi incelendi; var olan sahne ve sistemler korunuyor.

- [x] Unity sürümü ve proje yapısı incelendi; teknik başlangıç `BootScene`, ardından `MainMenuScene`, `OfficeScene` ve `InterviewScene` akışı kuruldu. Eski `Bootstrap` sahnesi geçiş için korunur; oyun derlemesine alınmaz. Dosya/tablet/sonuç ofis içinde UI odak durumlarıdır.
- [ ] Yeni dört sahneli akış Unity Play Mode ve mobil cihazda baştan sona doğrulansın.
- [x] Mobil yatay yön, iki landscape dönüşü ve 1280×720 referans çözünürlüğü ayarlı; portre yönleri kapalı.
- [x] Merkezi `config.json` ve Türkçe metin kataloğu var; masa/ana ekranındaki kalan sabit metinler kataloğa taşındı.
- [x] Yerel ilerleme kaydı ve yeniden yükleme temeli var; içerik doğrulaması kayıt serileştirmesini denetliyor.
- [x] UI Toolkit düğmeleri, masa dokunma bölgeleri ve güvenli alan uyarlaması kodda kurulu.
- [ ] Gerçek Android/iOS cihazda dokunma hedefleri, yatay güvenli alan ve uygulama yeniden açıldığında kayıt doğrulansın.

Aşama 1 cihaz sınaması tamamlanana kadar bitmiş sayılmaz. Sonraki geliştirmede de `MASTER_GAME_CONTEXT.md` esas alınır; geç tarihli kullanıcı düzeltmeleri `DESIGN_AMENDMENTS.md` içinde saklanır.

## Ürün hedefi

Oyuncu Bora'nın masasında Dosya #001'i alır; belgeleri ve ifadeleri inceler, metin tabanlı CCTV/BPS kayıtlarını kontrol eder, yeni kanıtlarla kişilere tekrar döner, gerekçeli raporunu göndererek dosyayı kesin kapatır. Bölüm özeti ardından kurumsal değerlendirme Gelen Evraklar faksıyla gelir. Sonraki vakalar temel kodu değiştirmeden veri ve Türkçe metin eklenerek hazırlanabilmelidir.

## Şu an çalışan temel

- [x] Unity 6000.3.17f1 projesi ve `Bootstrap` sahnesi açılıyor.
- [x] Bora portreli “Oyunu Başlat” ana ekranı, dosya kabul sahnesi, yatay masa görünümü ve ilk dosyaya giriş var. Kabul durumu yerel kayda yazılır.
- [x] Türkçe metin kataloğu, vaka JSON'u, önkoşullu belge/görüşme sırası ve yerel ilerleme kaydı var.
- [x] Metin tabanlı kamera/BPS kayıtları, sonuç seçimi ve gönderilen raporun dosyayı kesin kapatması kodda var.
- [x] Kamera eksikliği giderildi; masa görseli Unity'de gösteriliyor.
- [x] Dosya #001 **baştan kapanışa kadar** elle oynandı (kullanıcı, 25 Eylül 2026): soruşturma, sorgu, kanıt eşleme ve gerekçeli sonuç gönderme adımları sorunsuz. Cihazda yinelenmesi ayrı madde.
- [ ] Android/iOS cihaz testi ve yeniden açınca kayıt doğrulaması yapılmadı.

Bu bölümdeki işaretler teslim edilmiş davranışları gösterir; otomatik doğrulama, cihaz deneyiminin yerine geçmez.

## M1 — Dosya #001'in sağlam oynanabilir döngüsü

**Durum: kod büyük ölçüde bitti, doğrulama açık.** Aşağıdaki maddelerin çoğu `[~]`: yazıldı ve içerik doğrulaması geçti, ama Dosya #001 bugüne kadar bir kez bile Play Mode'da baştan sona oynanmadı. Bu aşamayı kapatan tek iş `PHASE_PLAN.md` Faz 2'dir.

**Özgün not:** Önceki sohbetle çelişen Dosya #001 içeriğini düzeltip akışı oyuncu açısından tamamlayacağız.

- [x] CCTV kayıt boşluğu, Elif/Mert/Hasan takip içerikleri ve Eşya Tespit Raporu eklendi; kameradan doğrudan Hasan teşhisi ve sonuç öncesi itiraf çıkarıldı.
- [~] Yeni soru yolları duyulan iddiaya göre dallanır, iddianın gizli doğruluğuna göre değil: Elif’in saksı açıklaması Mert’e takip sorusu açar; Hasan’a Elif’in veya Mert’in anlattıklarından gidilebilir. Bağımsız Eşya Tespit Raporu bu cevapların doğruluğuna bağlanmaz; Gelen Evraklar’da oyuncunun fark edeceği fiziksel geliş akışı ayrıca tamamlanacak. İki alternatif yol yalıtılmış Unity içerik testinden geçti; Play Mode ve cihaz testi açık.
- [~] Görüşmelerde sorulan soru ve o anda duyulan yanıt varyantı sıralı tutanak olarak kalıcı kayda alınıyor; fiziksel dosyanın Kişi İfadeleri sekmesinde okunuyor. Yeni tutanak için masa/dosya işareti var; sekme açılınca okunmuş sayılıyor. Yalıtılmış Unity kopyasında derleme ve kayıt-yükleme içerik testi geçti; açık Editor görsel/dokunma testi bekliyor.
- [ ] Dosya #001'in uzunluğu metin doldurarak değil, karakter motivasyonu, koşullu yanıt, yeni soru ve karşı kanıt katmanlarıyla artırılsın; önceki 10–15 dakika tahmini yeniden değerlendirilsin.
- [~] Dosya #001'in 46 Türkçe metni (ilk rapor, ifade özetleri, isteğe bağlı sorular, kaynakla yüzleştirme yanıtları, CCTV açıklaması, eşya raporu ve kapanış özeti) master truth ile karşılaştırılarak yeniden yazıldı. İlk dosya görünümünde soru sorulmadan kesin saat/anahtar bilgisi açığa çıkmıyor; Elif'in yalanı ile Hasan'ın fiili ayrı tutuluyor. Unity içerik doğrulaması tamamlandı; Play Mode'da anlatı temposu ve okunabilirlik sınanacak.
- [~] Dosya #001 bilgi temposu: Elif Mert'in anahtar cevabıyla, Hasan komşu cevabıyla, CCTV ise Hasan'ın kamera cevabıyla erişilebilir olur. Mert'in anahtar takip görüşmesi ilk ifadesi ve kamera kaydı incelendikten sonra, Elif'in saksı açıklaması **veya** Mert'in kapıyı kilitlediği cevabı duyulunca açılır; Elif'in itirafı tek zorunlu rota değildir. Eşya incelemesi CCTV ve en az bir takip kaynağından sonra istenebilir. Unity içerik doğrulaması tamamlandı; Play Mode'da tempo sınanacak.
- [~] Dosya #001 metin mantığı yeniden denetlendi: Hasan'ın Bora'nın Elif'le konuştuğunu açıklamasız bildiği yanıt varyantları kaldırıldı; Elif'in ilk inkârıyla çelişen soru, Mert'in ayrılık zamanı ifadesi ve iki ayrı anahtarın geçmişi netleştirildi. Eşya teslim fişinde Hasan'ın daireye girişini istemeden kabul ediyormuş gibi duyulan söz düzeltildi. Unity içerik doğrulaması tamamlandı; oynanışta duygu ve okunabilirlik kontrolü açık.
- [x] Görüşmeler sıralı konuşma turlarıyla ilerler: soru seç → Bora sorar → yanıtı dinle → devam et. İlgili delil ve önceki sorular yeni seçenekleri açar; yanıt varyantları vaka verisinde tanımlıdır. Elif ve Hasan ilk görüşmelerinin sırası serbest. Görüşmenin tamamlanması gerekli soruların sorulmasına bağlıdır.
- [~] Görüşme odasındaki sağ üst konuşma kutusu uzun soru/cevapları kendi içinde kaydırır ve soru düğmelerine taşmaz. Kod ve Unity derleme doğrulaması tamamlandı; farklı ekran oranlarında Play Mode okunabilirlik/dokunma testi açık.
- [ ] Yeni bilgi yokken tekrar yanıtı ve daha zengin mimik çeşitleri ekle; görsel düzeni gerçek cihazda doğrula.
- [x] Elif, Mert ve Hasan için ayrı pixel-art portreler görüşme ekranına bağlandı.
- [x] Adı öğrenilen kişiler ve açılan takip görüşmeleri kendiliğinden ifade olarak gelmez: oyuncu masadaki görüşme listesinden ifade alınmasını ister; kısa bir “İfadesi alınıyor…” durumu sonrasında metin erişilebilir olur. Talep ve hazır olma zamanı yerel kayda yazılır. Süre/tempo cihaz üzerinde henüz sınanmadı.
- [x] Görüşme Talepleri ekranı CCTV ile aynı elde tutulan pixel-art tablet ve açılış animasyonunu kullanır. Mert/Elif/Hasan kartlarında mevcut portreler, kimlik, talep durumu ve hazır olduğunda görüşmeye başlama eylemi bulunur; takip görüşmeleri aynı kişinin kartında ilerler. Henüz duyulmamış ifade alıntısı önceden gösterilmez. Unity görsel/dokunma doğrulaması açık.
- [x] Sonuç raporunda şüpheli, giriş yöntemi ve belirleyici kanıt ayrı seçiliyor. Sonraki karar doğrultusunda gönderim kesin kapanışa dönüştürüldü; yanlış raporun değerlendirmesi faksla geliyor.
- [~] Sonuç Raporu masa üstündeki doğrudan düğmeden kaldırıldı; yalnız fiziksel dosyanın içindeki sekmeden açılır. Rapor kâğıt görünümünde seçilip gönderiliyor ve dosya kapanıyor. Yeni giriş yolunun Unity Play Mode ve cihaz dokunma testi açık.
- [~] IBM Plex Mono Regular/SemiBold rapor seçimleri ve butonlarına uygulandı; ana menü ve tablet başlıklarına kısa glitch açılışı eklendi. Font tutarlılığı ve animasyonun okunurluğu Unity’de görsel olarak doğrulanmalı.

- [x] Masa nesnelerinin görevleri net: gelen evrak/dosya dosyayı, telefon görüşmeleri, **yalnız masadaki terminal** CCTV/BPS kayıtlarını açar. Hasan’ın kamera sözü yalnız terminal kaynağını erişilebilir yapar; otomatik geçiş veya görev oku yoktur. Terminale masadan erişim ve dosyadan doğrudan inceleme yolunu kaldırma kodda yapıldı; tam Play Mode tıklama testi açık.
- [~] Yeni tutanak masa/dosya işaretiyle, talep edilmiş Eşya Tespit Raporu hazır olduğunda Gelen Evraklar bildirimiyle fark edilir; görev oku veya “şimdi X ile konuş” emri verilmez. Belge akışı yalıtılmış Unity içerik testinden geçti, açık Editor ve cihaz görsel/dokunma testi bekliyor.
- [~] Gelen Evraklar fiziksel belge odak görünümüyle açılır: bekleyen inceleme talebi, teslim edilen rapor ve kurumsal faks burada görülür. Oyuncu raporu kendisi dosyaya alır; görünümün Unity ve cihaz etkileşimi doğrulanmalı.
- [ ] Bora’nın kısa Personel Profili yeni oyun girişinde UI odak görünümü olarak eklenir.
- [ ] Her adımda masaya ve dosyaya geri dönüş çalışır; oyuncu hiçbir durumda çıkmazda kalmaz.
- [~] Sonuç raporunda seçimler aynı kaydırma konumunda güncellenir; Mert/Elif/Hasan değerlendirilebilir. Kişi/yöntem/kanıt iddiaları ve her birinin hangi kaynağa dayandırıldığı açıklanır; kaynak seçicisinde erişilmiş metin önizlemesi ve gönderim öncesi üç satırlık rapor özeti vardır. Gönderilen rapor dosyayı kesin kapatır; bölüm özeti sonucu hemen söylemez. Gelen Evraklar faksı artık her iddianın seçilen kaynağını ve desteklenme gerekçesini okunabilir ayrı bölümlerde açıklar; yanlış raporda doğru faili söylemez. Unity yalıtılmış kopya içerik testi geçti; açık Editor Play Mode, kayıt/yeniden açma ve cihaz dokunma doğrulaması açık. Dosya #002 geçiş testi ayrıca yapılacak.
- [ ] Kaynakla yüzleştirmede ilgisiz belge/kayıt seçimi aynı sorunun kaynak seçimine geri döner; soru tüketilmez ve oyuncu konu listesinin başına atılmaz. Sonuç Raporu mobil için kişi → yöntem → kanıt → son kontrol adımlarına ayrıldı; her adım seçim ve dayanak gerektirir, geri dönüşte seçimler korunur. Unity derleme, Play Mode ve cihaz dokunma testi açık.
- [ ] Son kontrol ekranındaki kişi/yöntem/kanıt dayanakları ayrı dokunulabilir kartlardan tam metin olarak açılır; rapor seçicisinde tekil ifade turları bulunur, seçilen ifade turu ve CCTV satırı gösterilir, kapatınca rapor aynı adımda kalır. Unity derleme, Play Mode ve cihaz dokunma testi açık.
- [~] Sonuç Raporu dayanak seçicisinde Tümü / Belgeler / İfadeler / CCTV filtreleri eklendi. Boş sekmeler pasif, kaynak seçimi filtre değişince korunuyor; kategori doğruluk işareti taşımıyor. Unity derleme, Play Mode ve mobil dokunma testi açık.
- [~] Rapor dayanak seçicisine kelime/saat araması, temizleme, sonuç sayısı ve boş durum eklendi. Arama erişilmiş tam belge metnini, sorulmuş soru-cevapları ve CCTV satırlarını tarar; kategori filtresiyle birlikte çalışır, `11:48`/`11.48` eşdeğerdir. Arama alanı, temizleme düğmesi, sekmeler ve sonuç satırları en az 48 birim dokunma hedefiyle düzenlendi. Türkçe JSON, vaka saat metni ve hedef boyutu statik kontrolleri geçti; Unity lisans bağlantısı ve bilgisayar arayüzü zaman aşımı yüzünden Play Mode, farklı yatay ekran oranları ve gerçek cihaz klavye/dokunma kontrolü açık.
- [~] Görüşmede kaynak öne sürme seçicisine de tam kaynak metninde kelime/saat araması, iki sıralı kategori filtreleri ve sonuç sayısı eklendi. Kısa liste adlarının tam metni seçilince referans kartından okunur; seçilen kaynak ve öne sürme eylemi uzun listenin üstündedir. Aynı soruda arama/filtre korunur. Türkçe metin, dokunma hedefleri ve erişim koşulları statik olarak denetlendi; Unity Play Mode, yatay telefon oranları ve gerçek klavye/dokunma testi açık.
- [~] Görüşmede seçilen soru konusu ve açık soru sayısı liste kayarken sağ sütunun tepesinde sabit tutuldu. Konu düğmeleri 48 birim dokunma yüksekliğine çıkarıldı; soru ve konu sırası değişmedi. Kod düzeni statik olarak denetlendi; Unity Play Mode, küçük yatay ekran oranı ve cihaz kaydırma/dokunma kontrolü açık.
- [~] Görüşmeye kaydedilmiş soru-cevaplar için sağ sütunda Sorular / Geçmiş geçişi eklendi. Geçmiş tam metni kronolojik ve kaydırılabilir gösterir; yeni bilgi/yorum üretmez, soru listesi ve konu seçimi geçişte korunur. İlk kayıt öncesi görünmez, yeniden girişte Sorular açılır. Türkçe içerik ve kayıt erişimi statik olarak denetlendi; Unity Play Mode, küçük yatay ekran ve gerçek dokunma/kaydırma testi açık.
- [ ] Yeni oyun ilerlemeyi sıfırlar; Devam Et, uygulama kapatılıp açıldıktan sonra aynı dosya durumunu yükler.
- [ ] Play Mode'da tam yol ve ters sıra/geri dönüş senaryoları tamamlanır; Console'da hata kalmaz. İçerik doğrulaması geçti, fakat tam tıklama akışı henüz sınanmadı.
- [~] CCTV/BPS için eski yan menülü tablet ve ayrı arşiv listesi kaldırıldı. Masadaki terminal erişilebilir tek kaydı doğrudan elde tutulan tablete açar; kayıt yoksa aynı tablette kısa boş durum gösterir, birden çok kayıt olduğunda tablet içi sekmeler kullanır. Masa görseline gömülü sahte Gelen Evrak “1” rozeti kaynak PNG'den kaldırıldı; yeni belge/faks sayacı yalnız gerçek gelen öğelerde çizilir ve beklerken güncellenir. Kullanıcı Play Mode'da doğrulayacak.
- [~] Gelen Evraklar fiziksel tepsi/klasör odak görünümüne dönüştürüldü: solda tüm/yeni/okunan listesi, sağda seçilen belgenin okunabilir kâğıdı; bekleyen rapor, gelen rapor, dosyaya alınmış rapor, okunmamış faks ve eski fakslar erişilebilir. Dosya #001 ilk rapor ve eşya tutanağı ayrıntılandırıldı. Kurumsal faks üç iddiayı ayrı gerekçeli bölümlerde açıklar; yeni faksın ilişkili dosya adı listede görünür. Unity derleme ve içerik doğrulaması geçti; görsel/dokunma testi açık.

**Bitti ölçütü:** Yeni kayıttan başlayan bir oyuncu Dosya #001'i tek oturumda kapatır, oyunu yeniden açınca kapanmış hâlini görür ve yeni oyunla temiz başlangıç yapar.

- [x] Önceki soruşturma masası düzeni korundu. Görüşme talepleri ve CCTV elde tutulan tablet görünümünde; yalnız dosya fiziksel klasör/kâğıt olarak açılır. Ana menü Bora’nın gece ofisi görselinde Devam Et (kayıt varsa), Yeni Oyun, Ayarlar ve Hakkında eylemleriyle sınırlandı. Tabletin uçtan uca görsel/dokunma testi açık.

## Soruşturma dokusu ve mobil erişilebilirlik (25 Eylül 2026 oturumu)

**Durum: hepsi `[~]`.** Kodlandı, 51 test geçiyor, kullanıcı ekranlara baktı ve "normal görünüyor" dedi — ama hiçbiri **oynanarak** doğrulanmadı. Tasarım gerekçeleri: `DESIGN_AMENDMENTS.md`.

- [~] **"Adı geçtiyse cevap verme hakkı doğar"** — bir kaydı kişiye ancak adı orada geçiyorsa öne sürebilirsin. Kural metinden türer (`Investigation.MentionsPerson`), elle etiketlemeye bağlı değil, yeni vakalarda kendiliğinden işler. `aboutPersonIds` artık **ek**: yalnız kaydın kişiden adını anmadan söz ettiği yerler için (kamera satırları, kayıt boşluğu, Hasan'ın "kadın" dediği üç ifade). Otuz soru etiketi kaldırıldı. Kural belgelere de işliyor; belgeler eskiden hiç süzülmüyordu. Kişi başı kaynak: Elif 10→10, Hasan 9→8, Mert 11→20.
- [~] **Yem kaynaklar.** Yanlış kaynağı öne sürmek artık "ne diyeyim" değil, gerçek ama yanıltıcı bir yanıt üretir; soru kapanmaz. 29 yem. Metinler yeni olgu uydurmaz, yorumu ağırlaştırır: masum kişi kendi aleyhine konuşur, fail düz kalır.
- [~] **Davranış satırı** (deneme, dört kaynak sorusunda). Yanıtın altında dedektifin *gördüğü* davranış; gözlem, yorum değil. `answerKey + ".demeanor"` sözleşmesi, `Locale.Has` ile isteğe bağlı. Teşhis edilebilir olmaması bilinçli. Yayma kararı kullanıcıya ait.
- [~] **Yanıtlanan soru kapanır.** Birden çok belirleyici kaynağı olan soru "YENİ KAYITLA" önekiyle listede kalıyordu; oyuncu bunu "eksik kaldı" diye okuyordu. Yem denemeleri etkilenmez.
- [~] **Görüşmede vazgeçme.** Soru seçtikten sonra tek çıkış görüşmeyi bitirmekti; hem kaynak seçicisine hem "dinle" adımına geri dönüş kondu.
- [~] **Kaynak satırı yanıtı gösterir**, sorulan soruyu değil; liste "soracağım sorular" gibi okunuyordu.
- [~] **Dosya ekranı telefon için sadeleşti:** iç içe kaydırma kalktı (metin tam genişlik, görsel akışın içinde), "1 / 1" sayacı ve Önceki/Sonraki yerine dokunulur sayfa şeridi, sekme şeridinin kendi kaydırması kalktı, sekme biçimi `FileTab` yardımcısına toplandı.
- [~] **"Dosyada ara" dokunulur süzgece çevrildi.** Yazı alanı kalktı (klavye ekranın yarısını kaplıyordu); yerine tür ve kişi ekseni. Kişi eşleşmesi "adı geçtiyse" kuralının aynısını kullanır. `CaseSearch` ikiye ayrıldı; okunmamış kaynağın sızmadığını denetleyen kurallar aynı kapıyı koruyor.
- [~] **Doğrulayıcıya sekiz yeni kural.** Belirleyici/yem kaynağın kişiye kapalı olması, öne sürülemez kaynak, yinelenen yem yanıtı, yem = çözücü kaynak, davranış satırında yorum sözcüğü, satır uzunluğu, soru metninin kaynağı tekrar etmesi (not). Her kural bu oturumda gerçekten yaşanan bir hatadan doğdu.

## M2 — Soruşturmayı okuma listesinden oyuna çevirme

**Durum: kodun çoğu yazıldı, hiçbiri oynanarak doğrulanmadı.** Aşağıdaki 20'ye yakın madde `[~]`. Bu aşama "planlandı" değil, "doğrulanmayı bekliyor" durumundadır.

**Özgün not:** Hikâyeyi ilerletmek için yalnızca bir metni açmak yeterli olmamalı.

- [ ] “Görüldü”, “iddia edildi” ve “bağımsız kanıtla doğrulandı” ayrımı vaka verisinde tutulur; bu teknik etiketler oyuncu adına yorum olarak ekrana basılmaz.
- [x] Dosya #001 CCTV terminali masadaki terminalden açılır; referansa yakın pixel-art eller ve tablet kısa bir yükselme animasyonuyla öne gelir. CCTV ekranında sol uygulama menüsü ve ayrı sinyal paneli yoktur; kısa kamera başlığının altında geniş, büyük puntolu kayıt dökümü ana odaktır. Sinyal durumu tek satırda görünür. Kayıtlar vaka verisindeki farklı gecikmelerle sırayla açılır; bozuk satırlar aynı satırdaki küçük çözümleme düğmesiyle netleştirilir. 12.37–13.08 arasındaki kayıt boşluğu doldurulmaz. Unity derleme ve görsel/dokunma doğrulaması açık.
- [~] Dosya referansa yaklaşan katmanlı kâğıt, olay bilgileri/görsel iki sütunu ve Olay Raporu / Kişi İfadeleri / Kanıtlar / Görseller sekmeleriyle kuruldu. Unity içerik doğrulaması geçti; tüm sekmelerin ve mobil dokunmanın tam etkileşim testi açık.
- [~] Eşya Tespit Raporu isteği masa üstündeki ayrı düğmeden kaldırılıp elde tutulan tabletin İnceleme Talepleri bölümüne taşındı. Tablet talep gönderme, hazırlanıyor, Gelen Evraklar'a ulaştı ve dosyaya alındı durumlarını gösterir. Rapor gecikmeyle Gelen Evraklar’a gelir ve oyuncu alınca dosyaya girer; tam Play Mode ve cihaz doğrulaması açık.
- [x] Eşya Tespit Raporu'nun seri numarası LQ7B-024861 ve olay günü işletmeye teslim saati 14.10 olarak sabitlendi; ilk rapor, inceleme talebi, teslim tutanağı ve kapanış özeti aynı bilgileri kullanır. Teslim kaydı satış veya evden çıkış görüntüsü olarak sunulmaz. Unity içerik doğrulaması geçti.
- [~] Fiziksel dosyada oyuncunun doldurduğu Zaman Çizelgesi eklendi: duyulan ifade saatleri ve tamamlanmış CCTV kayıtları aday olarak açılır; oyuncu kaynaklı kartları kendisi ekleyip çıkarır, kayıtlar sıralanır ve yerel kayıtta kalır. Oyun çelişki veya fail yorumu yapmaz. Unity derleme/içerik ve kayıt dönüşü doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] Fiziksel dosyadan açılan iki sayfalı karşılaştırma görünümü eklendi: oyuncu eriştiği belgeleri, sorduğu soruların ifade dökümünü ve incelediği CCTV kayıtlarını iki bağımsız sayfada seçip kaydırır. Ekran çelişki ya da suçlu yorumu üretmez. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] Görüşmede bazı takip soruları için Bora'nın öne süreceği belge/kayıt oyuncu tarafından seçilir. Dosya #001'de Elif'in kamera kaydıyla, Hasan'ın kayıt boşluğuyla ve eşya raporuyla yüzleşmesi bu mekanizmayı kullanır. Yanlış kaynak soruyu tüketmez; doğru sunulan kaynak ifade tutanağına kaydedilir. Unity derleme/içerik doğrulaması geçti; Play Mode testi açık.
- [~] Sonuç raporunda kişi, yöntem ve temel kanıt iddialarının her biri oyuncunun incelediği ayrı bir kaynağa bağlanır. Bağlantılar dosya kaydında, bölüm özetinde ve kurumsal faksta korunur; destek eşleşmesi yalnız faks değerlendirmesinde görünür. Dosya #001 destek kaynakları vaka verisinde tanımlıdır. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] CCTV kaynak olarak seçildiğinde raporda kameranın tamamı yerine incelenmiş dökümün belirli saatli satırı seçilir. Satır kimliği vaka verisindedir; seçilen metin rapor özeti ve faksta görünür, destek eşleşmesi veriyle yönetilir. Dosya #001 metin tabanlı CCTV dilini korur. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] Görüşmede CCTV öne sürülürken oyuncu incelenmiş kaydın belirli satırını seçer. Dosya #001'de Elif'in giriş/çıkış satırı iki farklı yanıt, Hasan'ın kayıt boşluğu kendi takip yanıtını açar; ilgisiz satır soruyu tüketmez. Kullanılan satır ve yanıt ifade tutanağında saklanır. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] Çok kaynaklı bir soruda oyuncu aynı kişiye dönüp henüz sunmadığı kayıt satırıyla yeniden sorabilir. İlk cevap silinmez; aynı satır ikinci kez yanıt üretmez. Görüşme talep ekranı yeni kaynakla konuşma imkânını gösterir. Unity derleme/içerik ve kayıt doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] Görüşmede kaynak seçilince sol alt köşede fiziksel referans kartı açılır. Seçilmiş CCTV satırının tam metni veya belgenin kaydırılabilir metni, soru ve cevap süresince odadan çıkmadan okunur; kart kapatılıp tekrar açılabilir. Unity derleme/içerik doğrulaması geçti; Play Mode görsel/dokunma testi açık.
- [~] Alınmış bir ifadenin belirli soru-cevap çifti başka kişiye kaynak olarak gösterilebilir. Dosya #001'de Mert'in yedek anahtarı Hasan'a gösterdiğini anlattığı cevap, Hasan'a yeni bir takip sorusu açar. İfade kartında tam soru-cevap görünür; kullanılmayan veya hiç alınmamış cevap öne sürülemez. Unity derleme/içerik doğrulaması geçti; Play Mode görünüm/dokunma testi açık.
- [~] Dosya #001'e Mert, Elif ve Hasan için on isteğe bağlı görüşme sorusu eklendi. Ayrılık, kişisel eşyalar, komşuluk, anahtar ve hafıza ayrıntıları farklı sırayla açılır. Bu dallar dosyayı kapatmanın zorunlu koşulu değildir ve otomatik suçluluk yorumu üretmez. Unity içerik doğrulaması geçti; Play Mode anlatı/okunabilirlik testi açık.
- [~] Görüşmedeki açık sorular vaka verisindeki nötr konu başlıkları altında toplanır. Yalnız açılmış sorular gösterilir, başlıklar oyuncuya çözüm sırası önermez; birden çok konu varsa dokunarak açılıp kapanır. Oyuncunun son açtığı konu aynı kişiyle görüşmeye devam ederken korunur. Dosya #001'in otuz sorusu Türkçe başlıklara ayrıldı. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [~] Fiziksel dosyada arama sayfası: oyuncu açtığı belge/BPS metninde, aldığı ifade cevaplarında ve tamamladığı CCTV dökümünde Türkçe duyarlı kelime/saat arar. Eşleşen alıntı ve kaynak başlığı gösterilir; ifade ve CCTV sonucundan ilgili satıra kaydırılır. Erişilmemiş bilgi ve çelişki/fail yorumu gösterilmez. Unity derleme/içerik ve erişim doğrulaması geçti; Play Mode klavye/okunabilirlik testi açık.

**Bitti ölçütü:** Oyuncu Elif'in yalanını keşfeder ama Hasan'ı ancak bağımsız kanıtları birleştirerek doğru gerekçeyle seçer. Birinci vakanın metinleri ve zaman çizelgesi çelişkisizdir.

## M3 — Vaka ekleme ve kayıt güvenilirliği

**Durum: başladı.** Dosya #002 çalışma taslağıdır; oyuncuya yeni görevlendirme olarak açılmaz. Hikâye ve metin akışı kullanıcıyla netleştirilmeden yayımlanmış bölüm sayılmaz. Dosya #001 kapanışı, kurumsal faks ve sonraki görev bildirimleri Play Mode ile cihazda sınanacak.

Yeni görevlendirme ritüeli: kapalı dosya için yalnız yayımlanmış bir sonraki vaka Gelen Evraklar'da okunmamış görevlendirme olarak görünür. Bölüm özeti ve masadaki “Yeni görevlendirme” eylemi bu evraka götürür; vaka ancak oyuncu evraktaki “Dosyayı aç” eylemini seçince yüklenir ve ardından ayrı dosya kabul ekranı açılır. Değerlendirme faksı bağımsızdır; yeni görev için onu okumak gerekmez. Taslak #002 bildirim veya rozet üretmez. Unity yalıtılmış kopya derleme ve içerik doğrulaması geçti; gerçek Play Mode ve cihaz testi açık.

- [ ] Vaka içeriği için şema doğrulaması: kimlikler, önkoşullar, metin anahtarları, erişilemeyen düğümler, döngüler.
- [~] Vaka kataloğu ve tamamlanan/yeni vaka durumu eklenir. Ana menüdeki salt okunur Dosya Arşivi, bu kariyerde kapanmış ve kaydı bulunan dosyaları gösterir; gönderilen rapor, erişilmiş belgeler/BPS-CCTV kayıtları, gerçekten sorulmuş ifade cevapları ve oyuncunun sabitlediği zaman çizelgesi yeniden okunur. Çizelge yalnız erişilmiş bilgiyle kaydedilmiş kartları gösterir; arşivde değiştirilemez. Raporla açılmış kurumsal faks aynı sayfada yan yana karşılaştırılır; faks Gelen Evraklar'dan açılmadan değerlendirme gösterilmez. Rapordaki erişilebilir dayanak bağlantısı belgeye, belirli CCTV satırına veya belirli ifade cevabına gider; satır/cevap vurgulanır. Arşiv aktif vakayı veya kayıtları değiştirmez. Unity derleme doğrulaması geçti; Play Mode mobil gezinme, kaynak bağlantıları, faks öncesi/sonrası görünüm, zaman çizelgesi ve eski kayıt kontrolü açık. Açık vakalar için ayrı katalog düzeni daha sonra.
- [ ] Dosya #002 — Kayıp Yedek: eldeki veri yalnız geliştirme taslağıdır; Ece Tan ve Arda Yalın karakterleri Dosya #001'in parçası değildir. Yeni vaka metni ve soruşturma akışı netleştirildikten sonra taslak işareti kaldırılabilir. Geçiş, kayıt/yeniden açma, faks zamanlaması ve dokunma akışı o aşamada Play Mode/cihazda sınanır. [CASE002_DESIGN.md](CASE002_DESIGN.md) şu an onaylanmış içerik değil, taslak kaydıdır.
- [ ] Kayıt dosyası sürümleme/göç ve bozuk kayıt için güvenli geri dönüş sağlanır.

**Bitti ölçütü:** İkinci vaka kod değişikliği olmadan çalışır; eski Dosya #001 kaydı bozulmaz.

## M4 — Mobil kalite kapısı

> **Eksik bulundu (25 Eylül 2026):** Bu aşama derleme ayarlarını hiç içermiyordu. `applicationIdentifier`, IL2CPP, `AndroidTargetSdkVersion` ve keystore olmadan aşağıdaki hiçbir madde test edilemez. Bunlar baştaki **Yayın engelleri** listesine alındı.

Yeni kariyer açılışı: Dünya 1/Türkiye için kullanıcının seçtiği yaklaşık 10 saniyelik Gemini POV videosu yerleştirildi. Videodaki Türk bayrağı ve `bubeGames / powered by bubeDigital` yazısı korunur; eski oyun katmanı yazıları bu klipte yinelenmez. Sağ alttaki Gemini simgesi `Geç` düğmesiyle örtülür; ses kanalı açık, renk bilgisi BT.709. Kararma → masa → ilk dosya bırakma → Dosya #001 kabul geçişi ve kariyer başına tek gösterim sürer. Yalıtılmış Unity derleme/içerik doğrulaması geçti; Play Mode ve Android/iOS cihazda görüntü, ses ve düğmenin filigranı örtmesi doğrulanacak. Yedi ülkelik dünya açılışları ve Dünya 2 ülkesinin belirsizliği [WORLD_OPENINGS.md](WORLD_OPENINGS.md) içinde kayıtlıdır.

**Durum: planlandı.** Masa görseli ve içerik mobilde gerçekten okunabilir olmalı.

- [ ] Yaygın 16:9 ve daha uzun yatay telefon oranlarında güvenli alan, ölçek ve tıklanabilir bölgeler test edilir.
- [ ] Dokunma hedefleri, kaydırma, metin boyutu ve Türkçe karakterler gerçek Android/iOS cihazda kontrol edilir.
- [~] Sık kullanılan dar kontroller (Gelen Evrak filtreleri, tablet sekmeleri, dosya sekmeleri, CCTV çözümleme ve referans kartı kapatma) en az 48 arayüz birimlik dokunma alanına çıkarıldı; dosya sekmeleri düşük yükseklikte kaydırılabilir. Unity derleme doğrulaması tamamlandı. 16:9, 19.5:9 ve 20:9 yatay ekranlarda sekmelerin tamamına erişim, metin taşması ve başparmakla kullanım Play Mode/cihazda sınanacak.
- [ ] Arka plana alma, uygulamayı kapatma/açma ve kayıt devamlılığı denenir.
- [ ] Bellek, ilk açılış süresi ve paket boyutu ölçülür.

**Bitti ölçütü:** Dosya #001 en az bir gerçek telefonda baştan sona, okunabilir ve kayıt kaybetmeden oynanır.

## M5 — Görsel ve ses

**Durum: ertelendi.** İşleyiş ve mobil kapılar geçildikten sonra.

- [ ] Masa görselindeki Kadıköy/gerçek kurum çağrışımları kaynak varlıkta bube/Beşiktaş ile tutarlı hâle getirilir; mevcut örtüler geçici çözümdür.
- [ ] Görüşme ekranı paylaşılan pixel-art sprite yönünde kurulur: karakter, temel kimlik, söylenen cümle ve sorular; durum şeridi ve otomatik analiz notları görünmez.
- [~] Fiziksel dosya referansındaki katman/sekme hissi Unity’de kuruldu; Beşiktaş için ayrı temsili pixel-art olay yeri çizimi eklendi, gerçek kurum arması/Kadıköy dosya sayfasına alınmadı. Fontlar IBM Plex Mono olarak paketlendi; tüm ekran ve cihaz görsel testi açık.
- [ ] Dosya, telefon ve terminal açılışları için nesne odaklı geçişler; karakter sunumu, tipografi ve ses eklenir.
- [x] Kullanıcının paylaştığı görüşme ve dosya konseptleri `Docs/References/` klasörüne alındı; oyun içi kullanılacak/çıkarılacak öğeler kaydedildi.

**Bitti ölçütü:** Görsel dil tutarlı olur; süsleme, çalışan etkileşimleri gizlemez.

## M6 — Sonraki oyun sistemleri

**Durum: beklemede.** İlk vaka ve mobil oynanış doğrulandıktan sonra.

- [ ] Bora için kısa personel kartı/biyografi gösterilir; soyadı ve gereksiz arka plan ayrıntıları zorunlu kılınmaz.
- [ ] Vaka sonrası kalıcı kariyer kaydı, unvan ile departman güveni ayrımı ve performansa bağlı sorumluluk modeli tasarlanır. Ünvan listesi/eşikleri henüz kesin değildir.
- [~] Faks sonrası kariyer ve İstatistikler [tasarımı](CAREER_AND_STATISTICS_DESIGN.md) kodlandı: kalıcı sicil, üç değerlendirme ağırlığı, nitel kurum güveni, erişilebilir İstatistikler ve eski son-faks kaydı göçü. Yalıtılmış Unity kopyasında derleme/içerik testi geçti; açık Editor'da görsel-dokunma, gerçek cihaz kayıt testi ve ikinci vaka geçişi doğrulanmalı. Terfi eşikleri ve yeni araç yetkileri henüz tasarlanmadı.
- [ ] Yetki ile vakada mevcut veri kaynağı ayrı tutulur; yeni araçlar eskileri kaldırmaz, her vakada zorunlu kullanılmaz. CCTV ilk İstanbul vakasında kullanılmaya devam eder.
- [ ] Eski dosyaya dönüş ve vaka geçmişi geliştirilir; vakalar arası izler/yeniden açılan dosyalar ancak çekirdek döngü doğrulandıktan sonra kapsamlandırılır.
- [~] Dosya #001 için aynı sabit kamera açısından dört kısa görüntü (08.27, 11.48, 12.16, 17.54) üretilecek ve mevcut saatli metin dökümüne veriyle bağlanacak. Boş giriş referansı `Docs/Media/Case001_CCTV_Empty_Reference.png` ve dört klip hazır; 17.54 için kullanıcıdan gelen özgün CCTV 04 klibi mobil MP4 biçiminde eskisinin yerine kondu; sağ alt filigranın üzerine Geç düğmesi hizalandı. Tabletin ilgili saat satırlarında `İzle` düğmesi var; metin ve sinyal dökümü korunur. 12.37–13.08 arası görüntüsüz kalır; yüz teşhisi, anahtar ve fail gösterilmez. Unity derleme/içerik ve dosya yolu doğrulaması geçti; Play Mode ve mobil cihazda oynatma/dokunma testi açık.
- [~] Tablet içindeki videoya veriyle gelen kamera/konum ve saat etiketi, yanıp sönen REC, köşe işaretleri, hafif tarama/parazit katmanı eklendi. Dosya #001 “BİNA ÖNÜ” kullanır; sonraki vakalarda etiket vaka verisinden değişir. Yazılı döküm ve görüntüsüz sinyal boşluğu korunur. JSON ve 16:9 yerleşim hesapları statik doğrulandı; Unity Play Mode'da farklı telefon oranları, okunurluk, efekt şiddeti ve filigran örtme kontrolü açık.
- [~] CCTV kliplerine Oynat/Duraklat, tek kare ileri ve Başa al kontrolleri eklendi. Kare adımı için VideoPlayer desteği, yoksa kare konumlandırması kullanılır; destek yoksa düğme pasif ve durum açık. Üç kontrol en az 48 birim dokunma yüksekliğinde ve görüntünün altında; metin dökümü ile Geç düğmesi korunur. Unity API varlığı ve Türkçe metinler statik doğrulandı; klip sonu, kare adımı ve küçük telefon dokunma testi Play Mode/cihazda açık.
- [~] CCTV klibi tabletin tüm iç ekranına genişletildi; yinelenen üst başlık kaldırıldı, açıklama ve oynatma kontrolleri tek alt şeride alındı, video 1280×720 hedef dokuda kırpılmadan gösteriliyor. Kamera/REC işaretleri, Döküme dön ve filigranı örten Geç korunuyor. Farklı yatay telefon oranlarında video büyüklüğü, metin taşması, filigran örtme ve dokunma Play Mode/cihazda doğrulanacak.
- [ ] Sonraki vakalarda da görsel CCTV, vaka tasarımı gerektirirse kullanılabilir; zorunlu bölüm sırası kuralı değildir. Metin/sinyal dökümü temel inceleme biçimi olarak kalır.
- [ ] Ödüllü reklam ipuçları yalnızca karşılaştırmaya yönlendirir, faili vermez; Dosya #001 reklam ipucu içermez.
- [ ] Vaka kataloğu ve sonraki içerik ölçeği oynanış verisine göre belirlenir; ülke/dosya sayısı için erken örnekler hedef sayılmaz.

## Güncelleme kuralı

Her geliştirme oturumunun sonunda:

1. **Bu dosyada** yalnızca Unity'de veya cihazda **gözlenen** davranış `[x]` yapılır. Kodlanan ama oynanmayan iş `[~]`'dir; `[~]` hiçbir "Bitti ölçütü"nü karşılamaz.
2. Tarih yenilenir.
3. `Docs/STATUS.md` tek sayfalık özeti güncellenir.
4. Yeni tasarım kararı `Docs/DESIGN_AMENDMENTS.md`'ye, yeni teknik gerçek `Docs/Architecture.md`'ye yazılır.
5. Yeni engel ilgili aşamaya veya baştaki **Yayın engelleri** listesine eklenir.

Bir aşamanın bittiği, “Bitti ölçütü” gerçekleşmeden ilan edilmez. Sıradaki iş sırası için `Docs/PHASE_PLAN.md` esastır: bugün darboğaz yeni özellik değil, **Dosya #001'in ilk kez baştan sona oynanmasıdır** (Faz 2).

## Bölüm sonu görseli ve faks zamanlaması

- [~] Bölüm tamamlandı ekranı geniş fiziksel dosya, olay görseli, gönderilen rapor alanları, incelenen kaynaklar ve iki eylemle referansa yaklaştırıldı. Doğruluk/kariyer ödülü faks öncesi gizli. Unity ekran görüntüsü ve mobil ölçek testi açık.
- [~] Faks, rapordan sonra yedi saniyelik prototip gecikmesiyle Gelen Evraklar'a düşer; gerçek ikinci vaka zorunlu değildir. Bildirim ve bekleyen durum kayıtta korunur. Zamanlama akıcılığı, açık Unity Editor ve gerçek cihazda doğrulanmalı; yedi saniye nihai ritim kuralı değildir.

- [~] Bölüm özetinde kurumsal durum tek üst şeride taşındı; uzun ve tekrarlanan açıklama kaldırıldı. Unity görsel/ölçek doğrulaması açık.

## Marka kimliği (25 Eylül 2026 oturumu)

- [~] **KARINE logosu oyuna bağlandı.** Kullanıcının verdiği şeffaf PNG; `KarineLogo` tek kaynak/tek oran/tek doku yoğunluğu ile çiziyor, katmanlar `LogoBase` + `DistressOverlay`. Ana menüde büyük, arşiv/vaka seçicide ve ekran başlıklarında kompakt sürüm. Oran hem varlıkta hem **ekranda ölçülerek** Play Mode testiyle doğrulandı; gözle bakılmadı.
- [x] **İçe aktarım ezmesi düzeltildi.** Unity varsayılanı (`nPOTScale: 1`) logoyu 2000×667 → 2048×512 eziyordu; meta kilitlendi ve düşen test yeşile döndü.
- [~] **Yazı tipi rol tablosu** (`FontSet`): Heading / Mono / Body ayrıldı, eksik dosya mono'ya düşüyor ve doğrulayıcı not yazıyor. `RobotoSlab-ExtraBold.ttf` ve `Inter-Regular.ttf` **projede yok** — bu yüzden bugün ekranda görünür bir değişiklik yok.
- [~] **Bütün görsellerde içe aktarım oranı kilitlendi** (11 meta). Arka planlar yatayda ~%12, bayrak ~%33 esniyordu; artık esnemiyor. Doğrulayıcı kuralı eklendi ve düştüğü görülerek sınandı. Ekranlara gözle bakılmadı → `[~]`.

- [~] **Ana menü maketle eşlendi ve arka plana dönen animasyon bağlandı.** Logo, alt başlık, beş satırlık menü, sağ alt köşede stüdyo bloğu; video açılmazsa durağan görsele düşüyor. Satırların gerçekten çizildiği **Play Mode testiyle** doğrulandı (7 PlayMode testi); yerleşim oranlarına **gözle bakılmadı** → `[~]`.
- [~] **KARINE UI/UX Kit bağlayıcı tasarım sistemi olarak uygulandı.** `KarineTheme` (palet, boşluk, köşe, dokunma, devinim, diegetic kâğıt katmanı) + `KarineUI` (dört düğme biçimi, ikon düğme, panel, sekme, rozet, modal, bildirim, evrak gezintisi, ilerleme, tooltip). Eski palet sabitleri temaya bağlandı, kâğıt renkleri adlandırıldı (38 tekrar kalktı), ham renk borcu 156'da kilitlendi. 19 yeni EditMode testi kit kurallarını (palet hex'leri, düğme hiyerarşisi, 48 px dokunma hedefi, modal sırası, tipografi rolleri, köşe dili, devinim süreleri) sınıyor. **Ekranlara gözle bakılmadı** → `[~]`.
- [~] **`BubeApp.cs` bölündü** — 3.125 satırlık tek dosya konu başına sekiz `partial` parçaya ayrıldı; en uzun dosya 529 satır. Doğrulayıcıya 560 satır kilidi kondu. Testler bölünmeden sonra da geçiyor, **ekranda ayrıca bakılmadı** → `[~]`.
- [~] **Gövde yazısı mono'dan çıktı, büyük başlıklar logo diline yaklaştı** — kök öğe IBM Plex Mono'ya bağlıydı ve bütün oyun monospace okunuyordu; kök Inter oldu, mono yalnız teknik metinde kaldı. Büyük başlıklar (≥28) Alfa Slab One. **Ekranda görülmedi** → `[~]`.
- [~] **Yazı tipi rolleri gerçek oldu** — Roboto Slab ExtraBold/Bold ve Inter Regular/SemiBold depoya kondu (LFS, lisanslarıyla), Türkçe glif kapsamı denetlendi, doğrulayıcı dört dosyayı zorunlu tutuyor. **Ekranda görülmedi** → `[~]`.
- [~] **Sinematikte tek denetim GEÇ** — hızlandırma, duraklatma ve ilerleme çubuğu kaldırıldı; bunlar CCTV'ye ait. GEÇ kit'in birincil düğmesi oldu (`KarineUI.SkipButton`, dolu krem zemin — saydam değil); CCTV'nin görüntü üstü düğmeleri de aynı dile bağlandı. **Ekranda görülmedi** → `[~]`.
- [~] **Ekranlar kit bileşenlerine taşındı** — dört sekme şeridi, iki masa uyarısı, iki onay modalı, ayarlar radyoları, kariyer durum göstergeleri, sinematik kontroller (duraklat/ilerleme/süre/İLERİ SAR/GEÇ) ve teknik metinler `KarineUI`ye geçti. Kâğıt tonları beşe indi, ham renk borcu 156 → 65. **Ekranlara gözle bakılmadı** → `[~]`.
- [~] **Kit ikon dili kesildi** — 13 ortak ikon doğrudan kit görselinden; menüye özel dört ikon silindi (aynı işlev = tek ikon). Varlıkları doğrulayıcıda aranıyor.
- [~] **Menü simgeleri maketten kesildi** — beş PNG (`Bube/Art/Icons/menu_*`), satır tonuyla boyanıyor; font glifleri kalktı. Simgelerin ekrana geldiği Play Mode testiyle doğrulandı, **gözle bakılmadı** → `[~]`.
- [~] **Kit taşıması kapandı** — elle kurulan 41 düğmeden 5'i kaldı (CCTV'nin metni çalışma anında değişen üç düğmesi kit boyasını `KarineUI.Paint`ten alıyor, masadaki görünmez `Hotspot`, menünün kendine özgü satırı). Kâğıt katmanı için `PaperButton`, iki katman için `CloseButton` eklendi. Ham renk borcu 65 → 16; kalan 16 bilerek kalıyor (piksel portre tonları ve CCTV efektleri = oyun sanatı). Perde/cam/dosya kabı için `Veil`, `GlassDeep`/`Glass`/`GlassLift`, `Alpha`, `HotspotHover`, `Paper.Folder`/`FolderEdge`/`FolderDeep`/`Board`/`Approved` token'ları eklendi. Ekranlardaki her punto `Typography.Snap`ten geçiyor; iki yeni kilit de kaldırılıp denendi, beklenen hatayı verdi. 77 EditMode + 7 PlayMode yeşil. **Ekranlara Play Mode'da bakılmadı** → `[~]`.
- [~] **Vaka eklemek koddan koptu** — vaka metni vaka başına dil dosyasına taşındı (`LocaleLoader`, çakışmada ortak dosya kazanır), yedek portrenin tonları vaka verisine çıktı (`Node.portrait`). Doğrulayıcı vakaya özel C# kuralı istemiyor. Yeni vaka = JSON + dil dosyası + varlıklar.
- [~] **Ses temeli kuruldu** — `AudioDirector` (müzik/oda ortamı/efekt), `SoundSettings` (üç kademe, ayarlarda radyo grubu), düğme sesi kit kurucusundan, oda sesi sahneden ve vakadan. Sistem dosyasız da sessiz çalışıyor. Duyulmadı → `[~]`.
- [x] **Ses varlıkları üretildi, sadeleştirildi ve duyuldu** — dokuz klip `Resources/Bube/Audio/` altında (LFS): `ui_press`, `ui_typewriter`, `ui_stamp`, `ui_notification`, `ui_chat`/`ui_chat_low`, `menu_theme` (32 s neo-noir döngü, Am–F–Dm–E), `desk_theme` (40 s masa müziği, Dm–Gm–B♭–A), `room_interview` (24 s döngü). Üç tur kullanıcı geri bildirimiyle sadeleşti: duvar saati, floresan uğultusu ve tavan çınlaması kaldırıldı, düğme yuvarlandı, odalar −26 dBFS; kâğıt sesi tamamen kalktı (kâğıt düğmesi ve evrak okları artık düğme sesi çalıyor); konuşma taklidi (sesli harf, formant, klavye tuşu) tamamen bırakıldı — görüşmede yumuşak bir **sohbet blibi** çalıyor, hiçbir şey taklit edilmiyor. Kayıt değil sentez: `Tools/make-audio.py` üretir, `Tools/check-audio.py` kırpma/DC/seviye/döngü dikişini ve müzikte akorları ölçer (0 bulgu). `ProjectRules` dokuzunun, `CaseRules` vakanın `ambienceId`sinin varlığını kilitliyor. Kullanıcı 25 Eylül 2026'da sesleri oyunda **duydu** → `[x]`.
- [~] **Mobil davranış** — Android geri tuşu her katmanda ekranın kendi geri eylemine gidiyor, ana menüde çıkış onayı açılıyor, `OnApplicationPause` kayıt yazıyor, vaka zinciri bittiğinde masa kapanış bildirimi gösteriyor. **Cihazda denenmedi** → `[~]`.
- [~] **Reklam dikişi** — `AdGateway` tek karar yeri (onay, "reklam kaldırıldı", an kuralları), `IAdProvider` + `NoAdProvider`. Araya giren reklam yalnız vaka arası, ödüllü yalnız rapor geri döndükten sonra; soruşturma/sorgu/CCTV/sinematik kapalı. Ödüllü ipucu vakanın gerçeğini görmüyor (yöntem + kendi kapsamı) ve doğrulayıcı sızıntıyı yasaklıyor. **Ağ eklentisi yok** (kimlikler kullanıcıda), ödüllü yeniden deneme güveni iade ediyor ve kaydı saklıyor → `[~]`.
- [~] **Ödüllü yeniden deneme** — `Investigation.MayReopen`/`ReopenForRetry`: iade tam o faksın götürdüğü kadar, gerekirse görevden ayrılma kalkıyor; `reviewHistory` satırı "yeniden açıldı" işaretiyle kalıyor ve ikinci deneme kendi satırını yazıyor; soruşturma korunuyor, yalnız rapor alanları boşalıyor. Yedi EditMode testi. Play Mode'da gözlenmedi → `[~]`.
- [~] **Sessiz düğmeler, duyulmayan müzik ve masanın müziği** — oyun baştan sona oynandıktan sonra gelen dört bulgu kapatıldı. (1) Masadaki nesneler (`Hotspot`: gelen evrak, dosya, terminal, görüşme), menü satırı ve CCTV oynatma düğmeleri **sessizdi**: ses kilidi yalnız `Runtime/UI` içine bakıyordu, artık bütün `Runtime`e bakıyor. (2) Ana menü müziği hiç duyulmuyordu: akış klibi `Play()` anında yüklü değildi ve Unity bunu sessizce geçiyor — içe aktarım artık önceden yüklüyor, `AudioDirector` de yüklenmemiş klibi yüklüyor; ayrıca "kısık" kademesi 0,35'ten 0,55'e çıktı. (3) Masada oda gürültüsü yerine **müzik** çalıyor (`desk_theme`, 40 s, menüden yavaş ve alçak); `room_office` silindi. (4) Ayarlar sayfası yenilendi: kart telefonda kenardan %5 (eskiden %27, dikey ekranda daracık bir şerit oluyordu), içerik kaydırılabilir, ses kademeleri üç sıkışık radyo yerine kit'in sekme şeridi, bölümler alt başlık ve çizgiyle ayrılmış. **Gözle/kulakla bakılmadı** → `[~]`.
- [x] Arşiv kariyer ekranına, Hakkında ayarlara taşındı — menü maketteki beş satıra indi, iki işlev kaybolmadı.
