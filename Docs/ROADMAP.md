# Karine — geliştirme yol haritası

**Son durum:** 7 Ekim 2026 (sorgu akışı ayıklandı, CCTV videosu kalmadı — #011–#073 oynanmadı)
**Tek sayfalık durum:** `Docs/STATUS.md`  
**Sıra ve gerekçe:** `Docs/PHASE_PLAN.md`  
**Denetim ve kanıt:** `Docs/AUDIT_2026-09-25.md`  
**Dosya #001 oynanış betiği:** `Docs/PLAYTEST_001.md`  
**Testleri koşmak:** `Tools/run-tests.sh` (EditMode + PlayMode, 151 test: 126 EditMode + 25 PlayMode; ekran görüntüleri için `Tools/capture-screens.sh`)
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
- [~] **Hesap sistemi ve mağaza şartları (8 Ekim 2026).** Misafir / Google Play Games / Sign in with Apple girişi (UGS Authentication), hesap başına kayıt klasörü ve Cloud Save eşitlemesi, misafiri hesaba bağlama, çıkış, uygulama içinden hesap ve veri silme, gizlilik/silme sayfası bağlantıları, ülkeye göre dil. Derleniyor, 143 EditMode + 27 PlayMode geçiyor; **cihazda giriş denenmedi** (konsol kurulumu kullanıcıda: `Docs/STORE_ACCOUNTS_CHECKLIST.md`).
- [~] **Misafir bulut kaydı (Firebase, 9 Ekim 2026).** Anonim Firebase kimliği + Firestore (REST, SDK yok); misafir kaydı buluta yazılır, uygulama içinden silme bulut belgelerini, kimliği ve cihaz klasörünü siler; oyuncu kimliği ayarlarda görünür; erişim kuralları `Firebase/firestore.rules`. 143 EditMode geçiyor; **gerçek Firebase projesiyle denenmedi** (konsol adımları ve KVKK listesi: `Docs/FIREBASE_AND_KVKK.md`).
- [~] **İlerleme kariyer kaydında + hesap bağlama önerisi (9 Ekim 2026).** Lamba kilitleri, jenerik, görülen rütbe, oynama süresi `CareerProgress`te (buluta gider); Dosya #002 kabulünde misafire bir kez bağlama önerisi; ayarlarda bağlama satırları hep görünür. 143 EditMode + 28 PlayMode; öneri kâğıdı oyunda görülmedi.
- [~] **Misafir yeniden kurulumda aynı profile döner (9 Ekim 2026).** Yenileme anahtarı ayrıca iOS Keychain / Android Block Store'da (`DurableStore`); PlayerPrefs boşsa oradan okunur, silmede oradan da kalkar. Android ve iOS dalları başsız derlendi; **cihazda denenmedi**, Android Gradle derlemesi de alınmadı.
- [~] **Giriş zorunlu + bulut yalnız dönüm noktasında (9 Ekim 2026).** Giriş kâğıdı seçim yapılmadan geçilmez (geri tuşu dahil); buluta yalnız kariyer belgesi, rapor/değerlendirme/rütbe/ödül anında; açılışta yalnız yeni olan yazılır. Oyunda ve cihazda görülmedi.
- [ ] Apple/Google girişini Firebase'e bağla (`signInWithIdp` + misafiri bağlama, çakışmada hesabın kaydı açılır) — geliştirici hesapları bekleniyor.
- [ ] Firebase konsolu: proje, anonim giriş, Firestore (eur3), kurallar, API anahtarı → `config.json` — kullanıcıda.
- [~] **Mağaza hazırlığı (9 Ekim 2026).** Reklamları kaldır + geri yükle (Unity IAP 4.12.2), iOS ATT (önce ATT sonra UMP), oyun içi puan (3. dosyadan sonra bir kez), Crashlytics (paket gelince derlenir), bozuk kayıtta `.bak`tan dönüş, gizlilik/silme sayfası taslakları (`Docs/Legal`). EditMode 143 geçti; Android/iOS dalları başsız derlendi; Gradle/Xcode derlemesi ve cihaz denemesi yok. Kullanıcı adımları: `STORE_ACCOUNTS_CHECKLIST.md` → 9 Ekim eki.
- [~] **Geri dönüş ve çalışma görünürlüğü (9 Ekim 2026).** Masada "Çalışmam" düğmesi (reklamsız yöntem hatırlatması ve kapsama sayıları), vaka özetinde "Çalışman" bölümü, yerel hatırlatma bildirimleri (com.unity.mobile.notifications 2.4.0; ertesi gün ve 3 gün sonra, yalnız masada bekleyen bir şey varsa; izin ilk özet ekranında bir kez; Ayarlar › Oyun anahtarı). EditMode 143; Android/iOS dalları başsız derlendi; cihazda denenmedi.
- [x] Oyun adı **Karine** olarak belirlendi ve `config.json`, `productName`, paket kimliği ile `about.body`'ye uygulandı. (Faz 0)

- [x] **Vaka teklifi tam ekran olmaktan çıkıp masadaki gelen evrak tepsisine taşındı.** Rozet yanıp söner, oyuncu tepsiyi kendisi açar, önizlemeyi okur ve kabul eder; kabul edilmeden masada başka hiçbir şey açılmaz. Başsız **Play Mode testiyle akış uçtan uca koşturuldu ve gözlendi**.
- [x] **`DeskReference.png` görselinin içinde gerçek kurum adı ve arma var.** Üst şerit artık her ekranda `Desk()` tarafından örtülüyor, ama terminaldeki `EMNİYET SİSTEMİ` yazısı ve armalar duruyor. **Görselin yenilenmesi gerekiyor**; metin denetimi bunu yakalayamaz. Ayrıntı: `Docs/DESIGN_AMENDMENTS.md`. **Kapandı (4 Ekim 2026):** görsel depodan silindi; masa `Bube/Art/OfficeDesk`.

- [x] **Dosyanın masaya bırakılışı sinematik video oldu** (`case001_arrival.mp4`); filigran "Geç" düğmesiyle örtülüyor, filigran yeri veriden geliyor. Sinematik siyaha kapanıyor, masa siyahtan açılıyor; **Play Mode'da gözlendi**.
- [x] **Terminal ekranındaki arma yaması görünüyor.** Armanın yeri tek düz renkle dolduruldu; ekranın kendi gradyanından ayrıldığı için soluk bir dikdörtgen leke kalıyor ve CCTV kutusu sağa kaymış duruyor. Kullanıcının gönderdiği düzende kutu ekranda **ortalanmış**. Ya kaynak PNG alınacak ya da depodaki görselde kutu ortalanıp boşluk gradyanla doldurulacak. Kullanıcı kararı: **sonraya bırakıldı**. **Kapandı (4 Ekim 2026):** `DeskReference.png` ile birlikte gitti.
- [x] **Sinematik video yenilendi** (2,83 sn): terminalde yalnız "CCTV ARŞİVİ", arma ve kurum adı yok, karartma videonun içinde. Filigran "Geç" düğmesiyle örtülüyor. **Play Mode'da gözlendi.**
- [x] **CCTV kare dizisiyle oynatılır** (2 Ekim 2026): kayıt `framePaths`/`frameTimes`/`frameMs` alır, kareler kamera hızında oynar, kare değişiminde tek tip titreme; video yedek olarak kalır. Testler geçti (111+23); kare görseli henüz yok, Play Mode’da görülmedi.

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

Oyuncu Bora'nın masasında Dosya #001'i alır; belgeleri ve ifadeleri inceler, metin tabanlı CCTV/BDS kayıtlarını kontrol eder, yeni kanıtlarla kişilere tekrar döner, gerekçeli raporunu göndererek dosyayı kesin kapatır. Bölüm özeti ardından kurumsal değerlendirme Gelen Evraklar faksıyla gelir. Sonraki vakalar temel kodu değiştirmeden veri ve Türkçe metin eklenerek hazırlanabilmelidir.

## Şu an çalışan temel

- [x] Unity 6000.3.17f1 projesi ve `Bootstrap` sahnesi açılıyor.
- [x] Bora portreli “Oyunu Başlat” ana ekranı, dosya kabul sahnesi, yatay masa görünümü ve ilk dosyaya giriş var. Kabul durumu yerel kayda yazılır.
- [x] Türkçe metin kataloğu, vaka JSON'u, önkoşullu belge/görüşme sırası ve yerel ilerleme kaydı var.
- [x] Metin tabanlı kamera/BDS kayıtları, sonuç seçimi ve gönderilen raporun dosyayı kesin kapatması kodda var.
- [x] Kamera eksikliği giderildi; masa görseli Unity'de gösteriliyor.
- [x] Dosya #001 **baştan kapanışa kadar** elle oynandı (kullanıcı, 25 Eylül 2026): soruşturma, sorgu, kanıt eşleme ve gerekçeli sonuç gönderme adımları sorunsuz. Cihazda yinelenmesi ayrı madde.
- [ ] Android/iOS cihaz testi ve yeniden açınca kayıt doğrulaması yapılmadı.

Bu bölümdeki işaretler teslim edilmiş davranışları gösterir; otomatik doğrulama, cihaz deneyiminin yerine geçmez.

## M1 — Dosya #001'in sağlam oynanabilir döngüsü

**Durum: kod büyük ölçüde bitti, doğrulama açık.** Aşağıdaki maddelerin çoğu `[~]`: yazıldı ve içerik doğrulaması geçti, ama Dosya #001 bugüne kadar bir kez bile Play Mode'da baştan sona oynanmadı. Bu aşamayı kapatan tek iş `PHASE_PLAN.md` Faz 2'dir.

**Özgün not:** Önceki sohbetle çelişen Dosya #001 içeriğini düzeltip akışı oyuncu açısından tamamlayacağız.

- [x] CCTV kayıt boşluğu, Elif/Mert/Hasan takip içerikleri ve Eşya Tespit Raporu eklendi; kameradan doğrudan Hasan teşhisi ve sonuç öncesi itiraf çıkarıldı.
- [x] Yeni soru yolları duyulan iddiaya göre dallanır, iddianın gizli doğruluğuna göre değil: Elif’in saksı açıklaması Mert’e takip sorusu açar; Hasan’a Elif’in veya Mert’in anlattıklarından gidilebilir. Bağımsız Eşya Tespit Raporu bu cevapların doğruluğuna bağlanmaz; Gelen Evraklar’da oyuncunun fark edeceği fiziksel geliş akışı ayrıca tamamlanacak. İki alternatif yol yalıtılmış Unity içerik testinden geçti; Play Mode ve cihaz testi açık.
- [x] Görüşmelerde sorulan soru ve o anda duyulan yanıt varyantı sıralı tutanak olarak kalıcı kayda alınıyor; fiziksel dosyanın Kişi İfadeleri sekmesinde okunuyor. Yeni tutanak için masa/dosya işareti var; sekme açılınca okunmuş sayılıyor. Yalıtılmış Unity kopyasında derleme ve kayıt-yükleme içerik testi geçti; açık Editor görsel/dokunma testi bekliyor.
- [ ] Dosya #001'in uzunluğu metin doldurarak değil, karakter motivasyonu, koşullu yanıt, yeni soru ve karşı kanıt katmanlarıyla artırılsın; önceki 10–15 dakika tahmini yeniden değerlendirilsin.
- [x] Dosya #001'in 46 Türkçe metni (ilk rapor, ifade özetleri, isteğe bağlı sorular, kaynakla yüzleştirme yanıtları, CCTV açıklaması, eşya raporu ve kapanış özeti) master truth ile karşılaştırılarak yeniden yazıldı. İlk dosya görünümünde soru sorulmadan kesin saat/anahtar bilgisi açığa çıkmıyor; Elif'in yalanı ile Hasan'ın fiili ayrı tutuluyor. Unity içerik doğrulaması tamamlandı; Play Mode'da anlatı temposu ve okunabilirlik sınanacak.
- [x] Dosya #001 bilgi temposu: Elif Mert'in anahtar cevabıyla, Hasan komşu cevabıyla, CCTV ise Hasan'ın kamera cevabıyla erişilebilir olur. Mert'in anahtar takip görüşmesi ilk ifadesi ve kamera kaydı incelendikten sonra, Elif'in saksı açıklaması **veya** Mert'in kapıyı kilitlediği cevabı duyulunca açılır; Elif'in itirafı tek zorunlu rota değildir. Eşya incelemesi CCTV ve en az bir takip kaynağından sonra istenebilir. Unity içerik doğrulaması tamamlandı; Play Mode'da tempo sınanacak.
- [x] Dosya #001 metin mantığı yeniden denetlendi: Hasan'ın Bora'nın Elif'le konuştuğunu açıklamasız bildiği yanıt varyantları kaldırıldı; Elif'in ilk inkârıyla çelişen soru, Mert'in ayrılık zamanı ifadesi ve iki ayrı anahtarın geçmişi netleştirildi. Eşya teslim fişinde Hasan'ın daireye girişini istemeden kabul ediyormuş gibi duyulan söz düzeltildi. Unity içerik doğrulaması tamamlandı; oynanışta duygu ve okunabilirlik kontrolü açık.
- [x] Görüşmeler sıralı konuşma turlarıyla ilerler: soru seç → Bora sorar → yanıtı dinle → devam et. İlgili delil ve önceki sorular yeni seçenekleri açar; yanıt varyantları vaka verisinde tanımlıdır. Elif ve Hasan ilk görüşmelerinin sırası serbest. Görüşmenin tamamlanması gerekli soruların sorulmasına bağlıdır.
- [x] Görüşme odasındaki sağ üst konuşma kutusu uzun soru/cevapları kendi içinde kaydırır ve soru düğmelerine taşmaz. Kod ve Unity derleme doğrulaması tamamlandı; farklı ekran oranlarında Play Mode okunabilirlik/dokunma testi açık.
- [ ] Yeni bilgi yokken tekrar yanıtı ve daha zengin mimik çeşitleri ekle; görsel düzeni gerçek cihazda doğrula.
- [x] Elif, Mert ve Hasan için ayrı pixel-art portreler görüşme ekranına bağlandı.
- [x] Adı öğrenilen kişiler ve açılan takip görüşmeleri kendiliğinden ifade olarak gelmez: oyuncu masadaki görüşme listesinden ifade alınmasını ister; kısa bir “İfadesi alınıyor…” durumu sonrasında metin erişilebilir olur. Talep ve hazır olma zamanı yerel kayda yazılır. Süre/tempo cihaz üzerinde henüz sınanmadı.
- [x] Görüşme Talepleri ekranı CCTV ile aynı elde tutulan pixel-art tablet ve açılış animasyonunu kullanır. Mert/Elif/Hasan kartlarında mevcut portreler, kimlik, talep durumu ve hazır olduğunda görüşmeye başlama eylemi bulunur; takip görüşmeleri aynı kişinin kartında ilerler. Henüz duyulmamış ifade alıntısı önceden gösterilmez. Unity görsel/dokunma doğrulaması açık.
- [x] Sonuç raporunda şüpheli, giriş yöntemi ve belirleyici kanıt ayrı seçiliyor. Sonraki karar doğrultusunda gönderim kesin kapanışa dönüştürüldü; yanlış raporun değerlendirmesi faksla geliyor.
- [x] Sonuç Raporu masa üstündeki doğrudan düğmeden kaldırıldı; yalnız fiziksel dosyanın içindeki sekmeden açılır. Rapor kâğıt görünümünde seçilip gönderiliyor ve dosya kapanıyor. Yeni giriş yolunun Unity Play Mode ve cihaz dokunma testi açık.
- [x] IBM Plex Mono Regular/SemiBold rapor seçimleri ve butonlarına uygulandı; ana menü ve tablet başlıklarına kısa glitch açılışı eklendi. Font tutarlılığı ve animasyonun okunurluğu Unity’de görsel olarak doğrulanmalı.

- [x] Masa nesnelerinin görevleri net: gelen evrak/dosya dosyayı, telefon görüşmeleri, **yalnız masadaki terminal** CCTV/BDS kayıtlarını açar. Hasan’ın kamera sözü yalnız terminal kaynağını erişilebilir yapar; otomatik geçiş veya görev oku yoktur. Terminale masadan erişim ve dosyadan doğrudan inceleme yolunu kaldırma kodda yapıldı; tam Play Mode tıklama testi açık.
- [x] Yeni tutanak masa/dosya işaretiyle, talep edilmiş Eşya Tespit Raporu hazır olduğunda Gelen Evraklar bildirimiyle fark edilir; görev oku veya “şimdi X ile konuş” emri verilmez. Belge akışı yalıtılmış Unity içerik testinden geçti, açık Editor ve cihaz görsel/dokunma testi bekliyor.
- [x] Gelen Evraklar fiziksel belge odak görünümüyle açılır: bekleyen inceleme talebi, teslim edilen rapor ve kurumsal faks burada görülür. Oyuncu raporu kendisi dosyaya alır; görünümün Unity ve cihaz etkileşimi doğrulanmalı.
- [ ] Bora’nın kısa Personel Profili yeni oyun girişinde UI odak görünümü olarak eklenir.
- [ ] Her adımda masaya ve dosyaya geri dönüş çalışır; oyuncu hiçbir durumda çıkmazda kalmaz.
- [x] Sonuç raporunda seçimler aynı kaydırma konumunda güncellenir; Mert/Elif/Hasan değerlendirilebilir. Kişi/yöntem/kanıt iddiaları ve her birinin hangi kaynağa dayandırıldığı açıklanır; kaynak seçicisinde erişilmiş metin önizlemesi ve gönderim öncesi üç satırlık rapor özeti vardır. Gönderilen rapor dosyayı kesin kapatır; bölüm özeti sonucu hemen söylemez. Gelen Evraklar faksı artık her iddianın seçilen kaynağını ve desteklenme gerekçesini okunabilir ayrı bölümlerde açıklar; yanlış raporda doğru faili söylemez. Unity yalıtılmış kopya içerik testi geçti; açık Editor Play Mode, kayıt/yeniden açma ve cihaz dokunma doğrulaması açık. Dosya #002 geçiş testi ayrıca yapılacak.
- [ ] Kaynakla yüzleştirmede ilgisiz belge/kayıt seçimi aynı sorunun kaynak seçimine geri döner; soru tüketilmez ve oyuncu konu listesinin başına atılmaz. Sonuç Raporu mobil için kişi → yöntem → kanıt → son kontrol adımlarına ayrıldı; her adım seçim ve dayanak gerektirir, geri dönüşte seçimler korunur. Unity derleme, Play Mode ve cihaz dokunma testi açık.
- [ ] Son kontrol ekranındaki kişi/yöntem/kanıt dayanakları ayrı dokunulabilir kartlardan tam metin olarak açılır; rapor seçicisinde tekil ifade turları bulunur, seçilen ifade turu ve CCTV satırı gösterilir, kapatınca rapor aynı adımda kalır. Unity derleme, Play Mode ve cihaz dokunma testi açık.
- [x] Sonuç Raporu dayanak seçicisinde Tümü / Belgeler / İfadeler / CCTV filtreleri eklendi. Boş sekmeler pasif, kaynak seçimi filtre değişince korunuyor; kategori doğruluk işareti taşımıyor. Unity derleme, Play Mode ve mobil dokunma testi açık.
- [x] Rapor dayanak seçicisine kelime/saat araması, temizleme, sonuç sayısı ve boş durum eklendi. Arama erişilmiş tam belge metnini, sorulmuş soru-cevapları ve CCTV satırlarını tarar; kategori filtresiyle birlikte çalışır, `11:48`/`11.48` eşdeğerdir. Arama alanı, temizleme düğmesi, sekmeler ve sonuç satırları en az 48 birim dokunma hedefiyle düzenlendi. Türkçe JSON, vaka saat metni ve hedef boyutu statik kontrolleri geçti; Unity lisans bağlantısı ve bilgisayar arayüzü zaman aşımı yüzünden Play Mode, farklı yatay ekran oranları ve gerçek cihaz klavye/dokunma kontrolü açık.
- [x] Görüşmede kaynak öne sürme seçicisine de tam kaynak metninde kelime/saat araması, iki sıralı kategori filtreleri ve sonuç sayısı eklendi. Kısa liste adlarının tam metni seçilince referans kartından okunur; seçilen kaynak ve öne sürme eylemi uzun listenin üstündedir. Aynı soruda arama/filtre korunur. Türkçe metin, dokunma hedefleri ve erişim koşulları statik olarak denetlendi; Unity Play Mode, yatay telefon oranları ve gerçek klavye/dokunma testi açık.
- [x] Görüşmede seçilen soru konusu ve açık soru sayısı liste kayarken sağ sütunun tepesinde sabit tutuldu. Konu düğmeleri 48 birim dokunma yüksekliğine çıkarıldı; soru ve konu sırası değişmedi. Kod düzeni statik olarak denetlendi; Unity Play Mode, küçük yatay ekran oranı ve cihaz kaydırma/dokunma kontrolü açık.
- [x] Görüşmeye kaydedilmiş soru-cevaplar için sağ sütunda Sorular / Geçmiş geçişi eklendi. Geçmiş tam metni kronolojik ve kaydırılabilir gösterir; yeni bilgi/yorum üretmez, soru listesi ve konu seçimi geçişte korunur. İlk kayıt öncesi görünmez, yeniden girişte Sorular açılır. Türkçe içerik ve kayıt erişimi statik olarak denetlendi; Unity Play Mode, küçük yatay ekran ve gerçek dokunma/kaydırma testi açık.
- [ ] Yeni oyun ilerlemeyi sıfırlar; Devam Et, uygulama kapatılıp açıldıktan sonra aynı dosya durumunu yükler.
- [ ] Play Mode'da tam yol ve ters sıra/geri dönüş senaryoları tamamlanır; Console'da hata kalmaz. İçerik doğrulaması geçti, fakat tam tıklama akışı henüz sınanmadı.
- [x] CCTV/BDS için eski yan menülü tablet ve ayrı arşiv listesi kaldırıldı. Masadaki terminal erişilebilir tek kaydı doğrudan elde tutulan tablete açar; kayıt yoksa aynı tablette kısa boş durum gösterir, birden çok kayıt olduğunda tablet içi sekmeler kullanır. Masa görseline gömülü sahte Gelen Evrak “1” rozeti kaynak PNG'den kaldırıldı; yeni belge/faks sayacı yalnız gerçek gelen öğelerde çizilir ve beklerken güncellenir. Kullanıcı Play Mode'da doğrulayacak.
- [x] Gelen Evraklar fiziksel tepsi/klasör odak görünümüne dönüştürüldü: solda tüm/yeni/okunan listesi, sağda seçilen belgenin okunabilir kâğıdı; bekleyen rapor, gelen rapor, dosyaya alınmış rapor, okunmamış faks ve eski fakslar erişilebilir. Dosya #001 ilk rapor ve eşya tutanağı ayrıntılandırıldı. Kurumsal faks üç iddiayı ayrı gerekçeli bölümlerde açıklar; yeni faksın ilişkili dosya adı listede görünür. Unity derleme ve içerik doğrulaması geçti; görsel/dokunma testi açık.

**Bitti ölçütü:** Yeni kayıttan başlayan bir oyuncu Dosya #001'i tek oturumda kapatır, oyunu yeniden açınca kapanmış hâlini görür ve yeni oyunla temiz başlangıç yapar.

- [x] Önceki soruşturma masası düzeni korundu. Görüşme talepleri ve CCTV elde tutulan tablet görünümünde; yalnız dosya fiziksel klasör/kâğıt olarak açılır. Ana menü Bora’nın gece ofisi görselinde Devam Et (kayıt varsa), Yeni Oyun, Ayarlar ve Hakkında eylemleriyle sınırlandı. Tabletin uçtan uca görsel/dokunma testi açık.

## Soruşturma dokusu ve mobil erişilebilirlik (25 Eylül 2026 oturumu)

**Durum: hepsi `[~]`.** Kodlandı, 51 test geçiyor, kullanıcı ekranlara baktı ve "normal görünüyor" dedi — ama hiçbiri **oynanarak** doğrulanmadı. Tasarım gerekçeleri: `DESIGN_AMENDMENTS.md`.

- [x] **"Adı geçtiyse cevap verme hakkı doğar"** — bir kaydı kişiye ancak adı orada geçiyorsa öne sürebilirsin. Kural metinden türer (`Investigation.MentionsPerson`), elle etiketlemeye bağlı değil, yeni vakalarda kendiliğinden işler. `aboutPersonIds` artık **ek**: yalnız kaydın kişiden adını anmadan söz ettiği yerler için (kamera satırları, kayıt boşluğu, Hasan'ın "kadın" dediği üç ifade). Otuz soru etiketi kaldırıldı. Kural belgelere de işliyor; belgeler eskiden hiç süzülmüyordu. Kişi başı kaynak: Elif 10→10, Hasan 9→8, Mert 11→20.
- [x] **Yem kaynaklar.** Yanlış kaynağı öne sürmek artık "ne diyeyim" değil, gerçek ama yanıltıcı bir yanıt üretir; soru kapanmaz. *(7 Ekim 2026: değişti — yem artık soruyu yeni ilerlemeye kadar kapatır.)* 29 yem. Metinler yeni olgu uydurmaz, yorumu ağırlaştırır: masum kişi kendi aleyhine konuşur, fail düz kalır.
- [x] **Davranış satırı** (Dosya #001'de dört kaynak sorusu, Dosya #002'de bütün görüşmeler). Yanıtın altında dedektifin *gördüğü* davranış; gözlem, yorum değil. `answerKey + ".demeanor"` sözleşmesi, `Locale.Has` ile isteğe bağlı. Teşhis edilebilir olmaması bilinçli: sakinlik ve gerginlik dört kişiye de dağıtıldı, satır failden söz etmez. Toplam 103 satır (38 + 65).
- [x] **Yanıtlanan soru kapanır.** Birden çok belirleyici kaynağı olan soru "YENİ KAYITLA" önekiyle listede kalıyordu; oyuncu bunu "eksik kaldı" diye okuyordu. Yem denemeleri etkilenmez.
- [x] **Görüşmede vazgeçme.** Soru seçtikten sonra tek çıkış görüşmeyi bitirmekti; hem kaynak seçicisine hem "dinle" adımına geri dönüş kondu.
- [x] **Kaynak satırı yanıtı gösterir**, sorulan soruyu değil; liste "soracağım sorular" gibi okunuyordu.
- [x] **Dosya ekranı telefon için sadeleşti:** iç içe kaydırma kalktı (metin tam genişlik, görsel akışın içinde), "1 / 1" sayacı ve Önceki/Sonraki yerine dokunulur sayfa şeridi, sekme şeridinin kendi kaydırması kalktı, sekme biçimi `FileTab` yardımcısına toplandı.
- [x] **"Dosyada ara" dokunulur süzgece çevrildi.** Yazı alanı kalktı (klavye ekranın yarısını kaplıyordu); yerine tür ve kişi ekseni. Kişi eşleşmesi "adı geçtiyse" kuralının aynısını kullanır. `CaseSearch` ikiye ayrıldı; okunmamış kaynağın sızmadığını denetleyen kurallar aynı kapıyı koruyor.
- [x] **Doğrulayıcıya sekiz yeni kural.** Belirleyici/yem kaynağın kişiye kapalı olması, öne sürülemez kaynak, yinelenen yem yanıtı, yem = çözücü kaynak, davranış satırında yorum sözcüğü, satır uzunluğu, soru metninin kaynağı tekrar etmesi (not). Her kural bu oturumda gerçekten yaşanan bir hatadan doğdu.

## M2 — Soruşturmayı okuma listesinden oyuna çevirme

**Durum: kodun çoğu yazıldı, hiçbiri oynanarak doğrulanmadı.** Aşağıdaki 20'ye yakın madde `[~]`. Bu aşama "planlandı" değil, "doğrulanmayı bekliyor" durumundadır.

**Özgün not:** Hikâyeyi ilerletmek için yalnızca bir metni açmak yeterli olmamalı.

- [ ] “Görüldü”, “iddia edildi” ve “bağımsız kanıtla doğrulandı” ayrımı vaka verisinde tutulur; bu teknik etiketler oyuncu adına yorum olarak ekrana basılmaz.
- [x] Dosya #001 CCTV terminali masadaki terminalden açılır; referansa yakın pixel-art eller ve tablet kısa bir yükselme animasyonuyla öne gelir. CCTV ekranında sol uygulama menüsü ve ayrı sinyal paneli yoktur; kısa kamera başlığının altında geniş, büyük puntolu kayıt dökümü ana odaktır. Sinyal durumu tek satırda görünür. Kayıtlar vaka verisindeki farklı gecikmelerle sırayla açılır; bozuk satırlar aynı satırdaki küçük çözümleme düğmesiyle netleştirilir. 12.37–13.08 arasındaki kayıt boşluğu doldurulmaz. Unity derleme ve görsel/dokunma doğrulaması açık.
- [x] Dosya referansa yaklaşan katmanlı kâğıt, olay bilgileri/görsel iki sütunu ve Olay Raporu / Kişi İfadeleri / Kanıtlar / Görseller sekmeleriyle kuruldu. Unity içerik doğrulaması geçti; tüm sekmelerin ve mobil dokunmanın tam etkileşim testi açık.
- [x] Eşya Tespit Raporu isteği masa üstündeki ayrı düğmeden kaldırılıp elde tutulan tabletin İnceleme Talepleri bölümüne taşındı. Tablet talep gönderme, hazırlanıyor, Gelen Evraklar'a ulaştı ve dosyaya alındı durumlarını gösterir. Rapor gecikmeyle Gelen Evraklar’a gelir ve oyuncu alınca dosyaya girer; tam Play Mode ve cihaz doğrulaması açık.
- [x] Eşya Tespit Raporu'nun seri numarası LQ7B-024861 ve olay günü işletmeye teslim saati 14.10 olarak sabitlendi; ilk rapor, inceleme talebi, teslim tutanağı ve kapanış özeti aynı bilgileri kullanır. Teslim kaydı satış veya evden çıkış görüntüsü olarak sunulmaz. Unity içerik doğrulaması geçti.
- [x] Fiziksel dosyada oyuncunun doldurduğu Zaman Çizelgesi eklendi: duyulan ifade saatleri ve tamamlanmış CCTV kayıtları aday olarak açılır; oyuncu kaynaklı kartları kendisi ekleyip çıkarır, kayıtlar sıralanır ve yerel kayıtta kalır. Oyun çelişki veya fail yorumu yapmaz. Unity derleme/içerik ve kayıt dönüşü doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] Fiziksel dosyadan açılan iki sayfalı karşılaştırma görünümü eklendi: oyuncu eriştiği belgeleri, sorduğu soruların ifade dökümünü ve incelediği CCTV kayıtlarını iki bağımsız sayfada seçip kaydırır. Ekran çelişki ya da suçlu yorumu üretmez. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] Görüşmede bazı takip soruları için Bora'nın öne süreceği belge/kayıt oyuncu tarafından seçilir. Dosya #001'de Elif'in kamera kaydıyla, Hasan'ın kayıt boşluğuyla ve eşya raporuyla yüzleşmesi bu mekanizmayı kullanır. Yanlış kaynak soruyu tüketmez; doğru sunulan kaynak ifade tutanağına kaydedilir. Unity derleme/içerik doğrulaması geçti; Play Mode testi açık.
- [x] Sonuç raporunda kişi, yöntem ve temel kanıt iddialarının her biri oyuncunun incelediği ayrı bir kaynağa bağlanır. Bağlantılar dosya kaydında, bölüm özetinde ve kurumsal faksta korunur; destek eşleşmesi yalnız faks değerlendirmesinde görünür. Dosya #001 destek kaynakları vaka verisinde tanımlıdır. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] CCTV kaynak olarak seçildiğinde raporda kameranın tamamı yerine incelenmiş dökümün belirli saatli satırı seçilir. Satır kimliği vaka verisindedir; seçilen metin rapor özeti ve faksta görünür, destek eşleşmesi veriyle yönetilir. Dosya #001 metin tabanlı CCTV dilini korur. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] Görüşmede CCTV öne sürülürken oyuncu incelenmiş kaydın belirli satırını seçer. Dosya #001'de Elif'in giriş/çıkış satırı iki farklı yanıt, Hasan'ın kayıt boşluğu kendi takip yanıtını açar; ilgisiz satır soruyu tüketmez. Kullanılan satır ve yanıt ifade tutanağında saklanır. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] Çok kaynaklı bir soruda oyuncu aynı kişiye dönüp henüz sunmadığı kayıt satırıyla yeniden sorabilir. İlk cevap silinmez; aynı satır ikinci kez yanıt üretmez. Görüşme talep ekranı yeni kaynakla konuşma imkânını gösterir. Unity derleme/içerik ve kayıt doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] Görüşmede kaynak seçilince sol alt köşede fiziksel referans kartı açılır. Seçilmiş CCTV satırının tam metni veya belgenin kaydırılabilir metni, soru ve cevap süresince odadan çıkmadan okunur; kart kapatılıp tekrar açılabilir. Unity derleme/içerik doğrulaması geçti; Play Mode görsel/dokunma testi açık.
- [x] Alınmış bir ifadenin belirli soru-cevap çifti başka kişiye kaynak olarak gösterilebilir. Dosya #001'de Mert'in yedek anahtarı Hasan'a gösterdiğini anlattığı cevap, Hasan'a yeni bir takip sorusu açar. İfade kartında tam soru-cevap görünür; kullanılmayan veya hiç alınmamış cevap öne sürülemez. Unity derleme/içerik doğrulaması geçti; Play Mode görünüm/dokunma testi açık.
- [x] Dosya #001'e Mert, Elif ve Hasan için on isteğe bağlı görüşme sorusu eklendi. Ayrılık, kişisel eşyalar, komşuluk, anahtar ve hafıza ayrıntıları farklı sırayla açılır. Bu dallar dosyayı kapatmanın zorunlu koşulu değildir ve otomatik suçluluk yorumu üretmez. Unity içerik doğrulaması geçti; Play Mode anlatı/okunabilirlik testi açık.
- [x] Görüşmedeki açık sorular vaka verisindeki nötr konu başlıkları altında toplanır. Yalnız açılmış sorular gösterilir, başlıklar oyuncuya çözüm sırası önermez; birden çok konu varsa dokunarak açılıp kapanır. Oyuncunun son açtığı konu aynı kişiyle görüşmeye devam ederken korunur. Dosya #001'in otuz sorusu Türkçe başlıklara ayrıldı. Unity derleme/içerik doğrulaması geçti; Play Mode okunabilirlik/dokunma testi açık.
- [x] Fiziksel dosyada arama sayfası: oyuncu açtığı belge/BDS metninde, aldığı ifade cevaplarında ve tamamladığı CCTV dökümünde Türkçe duyarlı kelime/saat arar. Eşleşen alıntı ve kaynak başlığı gösterilir; ifade ve CCTV sonucundan ilgili satıra kaydırılır. Erişilmemiş bilgi ve çelişki/fail yorumu gösterilmez. Unity derleme/içerik ve erişim doğrulaması geçti; Play Mode klavye/okunabilirlik testi açık.

**Bitti ölçütü:** Oyuncu Elif'in yalanını keşfeder ama Hasan'ı ancak bağımsız kanıtları birleştirerek doğru gerekçeyle seçer. Birinci vakanın metinleri ve zaman çizelgesi çelişkisizdir.

## M3 — Vaka ekleme ve kayıt güvenilirliği

**Durum: başladı.** Dosya #002 "Son Sefer" artık taslak değil: senaryo kullanıcıdan geldi, veri yazıldı ve zincire bağlandı. Oyun içinde baştan sona oynanması hâlâ açık. Dosya #001 kapanışı, kurumsal faks ve sonraki görev bildirimleri Play Mode ile cihazda sınanacak.

Yeni görevlendirme ritüeli: kapalı dosya için yalnız yayımlanmış bir sonraki vaka Gelen Evraklar'da okunmamış görevlendirme olarak görünür. Bölüm özeti ve masadaki “Yeni görevlendirme” eylemi bu evraka götürür; vaka ancak oyuncu evraktaki “Dosyayı aç” eylemini seçince yüklenir ve ardından ayrı dosya kabul ekranı açılır. Değerlendirme faksı bağımsızdır; yeni görev için onu okumak gerekmez. Taslak #002 bildirim veya rozet üretmez. Unity yalıtılmış kopya derleme ve içerik doğrulaması geçti; gerçek Play Mode ve cihaz testi açık.

- [ ] Vaka içeriği için şema doğrulaması: kimlikler, önkoşullar, metin anahtarları, erişilemeyen düğümler, döngüler.
- [x] Vaka kataloğu ve tamamlanan/yeni vaka durumu eklenir. Ana menüdeki salt okunur Dosya Arşivi, bu kariyerde kapanmış ve kaydı bulunan dosyaları gösterir; gönderilen rapor, erişilmiş belgeler/BDS-CCTV kayıtları, gerçekten sorulmuş ifade cevapları ve oyuncunun sabitlediği zaman çizelgesi yeniden okunur. Çizelge yalnız erişilmiş bilgiyle kaydedilmiş kartları gösterir; arşivde değiştirilemez. Raporla açılmış kurumsal faks aynı sayfada yan yana karşılaştırılır; faks Gelen Evraklar'dan açılmadan değerlendirme gösterilmez. Rapordaki erişilebilir dayanak bağlantısı belgeye, belirli CCTV satırına veya belirli ifade cevabına gider; satır/cevap vurgulanır. Arşiv aktif vakayı veya kayıtları değiştirmez. Unity derleme doğrulaması geçti; Play Mode mobil gezinme, kaynak bağlantıları, faks öncesi/sonrası görünüm, zaman çizelgesi ve eski kayıt kontrolü açık. Açık vakalar için ayrı katalog düzeni daha sonra.
- [x] Rapor gönderme sinematiği (`report_send.mp4`) rapor ile vaka özeti arasına girdi; GEÇ düğmesi varış filmiyle aynı yerde. **Play Mode'da gözle görülmedi.**
- [~] Bölüm geçişinde dosya bırakılış animasyonu her yeni vakada oynuyor; daha önce yalnız dünyanın ilk dosyasında vardı. İki PlayMode testi var, ama **cihazda/Play Mode'da gözle görülmedi**.
- [x] Dosya #002 — Son Sefer: 26 Eylül 2026'da kullanıcının verdiği senaryo vakanın yerine geçti ve eski "Kayıp Yedek" taslağı tamamen kaldırıldı. Veri yazıldı (13 düğüm, 31 soru, 170 metin anahtarı), `draft` bayrağı kalktı, doğrulayıcı vakayı baştan sona oynayıp dört sütunlu raporu gönderiyor. Geçiş, kayıt/yeniden açma, faks zamanlaması ve dokunma akışı **Play Mode'da sınanmadı**. [CASE002_DESIGN.md](CASE002_DESIGN.md) artık onaylanmış içeriğin kaydıdır.
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

- [x] Masa görselindeki Kadıköy/gerçek kurum çağrışımları kaynak varlıkta bube/Beşiktaş ile tutarlı hâle getirilir; mevcut örtüler geçici çözümdür. **Kapandı (4 Ekim 2026):** eski masa görseli silindi, yeni masada kurum çağrışımı yok.
- [ ] Görüşme ekranı paylaşılan pixel-art sprite yönünde kurulur: karakter, temel kimlik, söylenen cümle ve sorular; durum şeridi ve otomatik analiz notları görünmez.
- [x] Fiziksel dosya referansındaki katman/sekme hissi Unity’de kuruldu; Beşiktaş için ayrı temsili pixel-art olay yeri çizimi eklendi, gerçek kurum arması/Kadıköy dosya sayfasına alınmadı. Fontlar IBM Plex Mono olarak paketlendi; tüm ekran ve cihaz görsel testi açık.
- [ ] Dosya, telefon ve terminal açılışları için nesne odaklı geçişler; karakter sunumu, tipografi ve ses eklenir.
- [x] Kullanıcının paylaştığı görüşme ve dosya konseptleri `Docs/References/` klasörüne alındı; oyun içi kullanılacak/çıkarılacak öğeler kaydedildi.

**Bitti ölçütü:** Görsel dil tutarlı olur; süsleme, çalışan etkileşimleri gizlemez.

## M6 — Sonraki oyun sistemleri

**Durum: beklemede.** İlk vaka ve mobil oynanış doğrulandıktan sonra.

- [ ] Bora için kısa personel kartı/biyografi gösterilir; soyadı ve gereksiz arka plan ayrıntıları zorunlu kılınmaz.
- [ ] Vaka sonrası kalıcı kariyer kaydı, unvan ile departman güveni ayrımı ve performansa bağlı sorumluluk modeli tasarlanır. Ünvan listesi/eşikleri henüz kesin değildir.
- [x] Bora filmi Unity'de izlendi; GEÇ düğmesi onaylandı (kullanıcı, 8 Ekim 2026). Türkiye açılış videosu akıştan çıkarıldı; film sonrası doğrudan masa `[~]`.
- [~] Bora'nın göreve hazırlanış filmi (10 sn, Gemini, 2.5D) yeni kariyerde Türkiye açılışından önce bir kez oynar. Kullanıcı Unity'de izledi (8 Ekim 2026). Filigran yok; GEÇ kitin standart düğmesine çevrildi, bu hâli henüz görülmedi.
- [~] Zorluk ağırlığı (1–3) ve Kariyer → Bora sekmesi (profil + güven kuralları); Bora uluslararası birimde. 139 EditMode geçti; ekran görülmedi.
- [~] Kariyer bitmez (8 Ekim 2026): güven 0'da aynı birimde gözetimli masa görevi, sıradaki gerekçeli raporla dönüş; 3'lü seri +3 ek güven; kayıp türe göre (−15 asılsız suçlama, −5 eksik). 138 EditMode geçti; oyunda görülmedi.
- [x] Faks sonrası kariyer ve İstatistikler [tasarımı](CAREER_AND_STATISTICS_DESIGN.md) kodlandı: kalıcı sicil, üç değerlendirme ağırlığı, nitel kurum güveni, erişilebilir İstatistikler ve eski son-faks kaydı göçü. Yalıtılmış Unity kopyasında derleme/içerik testi geçti; açık Editor'da görsel-dokunma, gerçek cihaz kayıt testi ve ikinci vaka geçişi doğrulanmalı. Terfi eşikleri ve yeni araç yetkileri henüz tasarlanmadı.
- [ ] Yetki ile vakada mevcut veri kaynağı ayrı tutulur; yeni araçlar eskileri kaldırmaz, her vakada zorunlu kullanılmaz. CCTV ilk İstanbul vakasında kullanılmaya devam eder.
- [ ] Eski dosyaya dönüş ve vaka geçmişi geliştirilir; vakalar arası izler/yeniden açılan dosyalar ancak çekirdek döngü doğrulandıktan sonra kapsamlandırılır.
- [~] Dosya #001 için aynı sabit kamera açısından dört kısa görüntü (08.27, 11.48, 12.16, 17.54) üretilecek ve mevcut saatli metin dökümüne veriyle bağlanacak. Boş giriş referansı `Docs/Media/Case001_CCTV_Empty_Reference.png` ve dört klip hazır; 17.54 için kullanıcıdan gelen özgün CCTV 04 klibi mobil MP4 biçiminde eskisinin yerine kondu; sağ alt filigranın üzerine Geç düğmesi hizalandı. Tabletin ilgili saat satırlarında `İzle` düğmesi var; metin ve sinyal dökümü korunur. 12.37–13.08 arası görüntüsüz kalır; yüz teşhisi, anahtar ve fail gösterilmez. Unity derleme/içerik ve dosya yolu doğrulaması geçti; Play Mode ve mobil cihazda oynatma/dokunma testi açık.
- [x] Tablet içindeki videoya veriyle gelen kamera/konum ve saat etiketi, yanıp sönen REC, köşe işaretleri, hafif tarama/parazit katmanı eklendi. Dosya #001 “BİNA ÖNÜ” kullanır; sonraki vakalarda etiket vaka verisinden değişir. Yazılı döküm ve görüntüsüz sinyal boşluğu korunur. JSON ve 16:9 yerleşim hesapları statik doğrulandı; Unity Play Mode'da farklı telefon oranları, okunurluk, efekt şiddeti ve filigran örtme kontrolü açık.
- [~] CCTV kliplerine Oynat/Duraklat, tek kare ileri ve Başa al kontrolleri eklendi. Kare adımı için VideoPlayer desteği, yoksa kare konumlandırması kullanılır; destek yoksa düğme pasif ve durum açık. Üç kontrol en az 48 birim dokunma yüksekliğinde ve görüntünün altında; metin dökümü ile Geç düğmesi korunur. Unity API varlığı ve Türkçe metinler statik doğrulandı; klip sonu, kare adımı ve küçük telefon dokunma testi Play Mode/cihazda açık.
- [~] CCTV klibi tabletin tüm iç ekranına genişletildi; yinelenen üst başlık kaldırıldı, açıklama ve oynatma kontrolleri tek alt şeride alındı, video 1280×720 hedef dokuda kırpılmadan gösteriliyor. Kamera/REC işaretleri, Döküme dön ve filigranı örten Geç korunuyor. Farklı yatay telefon oranlarında video büyüklüğü, metin taşması, filigran örtme ve dokunma Play Mode/cihazda doğrulanacak.
- [ ] Sonraki vakalarda da görsel CCTV, vaka tasarımı gerektirirse kullanılabilir; zorunlu bölüm sırası kuralı değildir. Metin/sinyal dökümü temel inceleme biçimi olarak kalır.
- [ ] Ödüllü reklam ipuçları yalnızca karşılaştırmaya yönlendirir, faili vermez; Dosya #001 reklam ipucu içermez.
- [ ] Vaka kataloğunun **ölçeği** 26 Eylül 2026'da karara bağlandı: on ülke × yedi dosya = 70 ([WORLD_OPENINGS.md](WORLD_OPENINGS.md)). Ülke listesi ve sırası kanon; her ülkenin mekânı, atmosferi ve yedi vakasının içeriği hâlâ yazılmamış iştir ve oynanış verisine göre şekillenir.
- [x] **Bölüm seçici ekranı** maket yerleşimiyle kodlandı ve **Play Mode'da görüldü** (kullanıcı, 26 Eylül 2026): kimlik şeridi, ülke listesi, iğneli pano, ülke kartı ve yedi dosyalık şerit; veri `Bube/Worlds.json`dan gelir, kilit ilerlemeden türer (ülke sırayla, dosya sırayla), kapanmış dosya kariyer kaydını açar. 113 test yeşil (7 yeni kilit testi). Açık kalan iki iş ekranın kendisi değil **varlıklar**: dünya haritası ile ülke/dosya görselleri ve kit'te olmayan kilit/onay/oynat ikonları. Cihazda görülmedi.

### 3 Ekim 2026 — Arayüz yenilemesi ve vaka sonu akışı
- [x] Soruşturma talepleri tam ekran (görüşmeler/incelemeler, canlı geri sayım, beklemeyi atla, raporu aç) — kullanıcı gördü.
- [x] Kariyer kaydı tam ekran sicil kâğıdı; tablet çerçevesi kaldırıldı.
- [x] Gerekçeli rapor iki iddia: şüpheli + ne ile/nasıl (+ varsa gözaltı); kanıt adımı ve dayanak seçici kalktı. Genel ikinci soru etiketi "Ne ile / nasıl yaptı?".
- [x] Raporda yalnız dinlenen kişiler seçilebilir (şüpheli ve gözaltı); kişi olmayan seçenekler ve yöntemler açık.
- [x] Rapor özeti maket (`UI_CASE_SUMMARY`) ile: polaroid (yoksa vaka kapağı), "Rapor gönderildi" damgası, rapor tablosu, kaynak notu, faks notu; masaya dön sabit altta. Rapor sonrası video kalktı.
- [x] Dosya rapor gönderilince değil, onay faksıyla kapanır: onay faksı ilk açılınca "KAPANDI" kartı (`UI_CASE_CLOSED`, raf evresi yok). Yanlış raporda dosya kapanmaz, ilerlemek yine mümkün.
- [x] Vaka sonrası akış düğmeyle: "Değerlendirme bekleniyor…" → "Değerlendirme faksını aç" → "Yeni görevi aç"; durum değişince düğme kendini yeniler. Bölüm özeti düğmesi ve boş görevlendirme kartı kalktı; yeni dosya tek kabulle gelir.
- [x] Masa bildirimleri yalnız masa ekrandayken iner; okunan faks/belgenin bildirimi kalkar.
- [x] Açılış dizisi oyun geneli: ilk dosya da bırakılış + müdür karşılama notu; büyük vaka başlığı kalktı; saat kartı tarih de yazar (`deskDate`; #001 için 7 Kasım 2026 seçildi).
- [x] Yeni görev her zaman sıfırdan başlar; eski kayıt ve vaka başı işaretleri yok sayılır (Dosya #002'nin atlanması düzeldi).
- [x] Sorgu odası maket (`UI_INTERVIEW`) ile: dosya şeridi, kimlik kartı, konuşma balonu, Sorular/Geçmiş sekmeleri, açılır konu başlıkları; "Kaydı öne sür" paneli (süzgeç, kaynak satırları, kâğıt önizleme, "Öne sür"). Kullanıcı ekran görüntüleri gönderdi, düzeltmeler sonrası hali görülmedi.
- [x] Bir soruda öne sürülen kayıt (tutsa da tutmasa da) o soruda bir daha listelenmez; kişi bazında değil soru bazında.
- [x] Masada halka yalnız bakılmamış şeyi olan eşyada atar (tepsi, dosya, telefon, CCTV, kanıtlar); telefonda yeni talep sayısı; yeni açılan kişi ve inceleme sekmelerde sayılır; "Yeni tutanak" dokununca tutanakları açar.

### 4 Ekim 2026 — Kapanış turu

- [x] Kullanıcı Dosya #001–#003'ü ve yeni arayüzü Unity'de oynadı; yukarıdaki oynanış/arayüz maddeleri bu gözleme göre `[x]` oldu. Cihaz, reklam ve kayıt göçü maddeleri açık kalır.
- [x] Sorgu odası: kişi oda görselinin koordinatlarına göre koltuğa oturur, masanın ön kenarı portrenin önüne çizilir; su bardağı ve kayıt cihazı kaldırıldı.
- [x] Pencere kapanış devinimi (`KarineMotion.Leave`): modal, onay formu, ayarlar.
- [x] `DeskReference.png` silindi; doğrulayıcı masa olarak `Bube/Art/OfficeDesk`'i arar.
- [x] Ekran görüntüsü testleri batchmode'da koşuyor (UIDocument'lar bir RenderTexture'a çizer); PlayMode 25/25.
- [x] `Tools/capture-screens.sh`: her vakanın sorgu ekranlarını `Docs/Screenshots/<tarih>/` altına yazar (klasör git dışı).
- [x] `run-tests.sh` Unity Hub kapalıyken sessizce çıkmıyor, uyarı yazıyor.

### 4 Ekim 2026 — Dosya #004 "Sessiz Kat"

- [~] Tasarım: `Docs/CASE004_DESIGN.md` — şüpheli ölüm; ölüm anında dairede olan yönetici, düşüş itmeden değil geri çekilmeden; saklanan suç zimmet.
- [~] Veri ve metin: `case004.json` (18 düğüm), `tr.case004.json` (247 metin). Doğrulayıcının otomatik gezintisi doğru sonuca ulaşıyor.
- [~] Zincir: Dosya #003 → #004, Türkiye haritasında dördüncü yuva bağlandı.
- [~] Portreler (Cem, Nihat, Derya, Gülay) ve vaka kapağı girdi; sorgu ekranları galeri çekiminde görüldü.
- [~] CCTV kareleri: 01.31 Tolga girişi (5), 01.46 sinyal kaybı (4, efekt), 02.24 sinyal dönüşü (4, efekt). Kare oynatımı gözlenmedi.
- [~] Galeri: `KARINE_TOUR=case004 Tools/capture-screens.sh` masa, dosya, belge, CCTV, özet ve karşılaştırma sayfalarını da çeker.
- [ ] Dosya #004 baştan sona Play Mode oynanışı (kullanıcı).

### 4 Ekim 2026 — Dosya #005 "Son Görüldüğü Yer"

- [~] Tasarım: `Docs/CASE005_DESIGN.md` — kayıp şahıs; kaybolma kendi isteğiyle, sebebi iş ortağının mali usulsüzlüğü; saldırı yok.
- [~] Veri ve metin: `case005.json` (21 düğüm), `tr.case005.json` (280 metin). Doğrulayıcı vakayı otomatik çözüyor; 126/126 EditMode.
- [~] Zincir: Dosya #004 → #005, haritada beşinci yuva.
- [~] Zaman: Kasım 2027 (#004'ün Ocak 2027'sinden sonra); kaybolma gecesi 14 Kasım 2027 Pazar, bildirim 15 Kasım Pazartesi; `Case005Rules` doğrulayıcıda.
- [~] Portreler (Aslı, Kaan, Murat, Nermin, Levent) girdi; sorgu ekranlarında galeri çekimiyle görüldü.
- [~] Vaka kapağı. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [~] CCTV kareleri (20.11, 22.41, 23.19, 00.08) — önce iki boş zemin: Moda apartman ön kapısı ve Rıhtım otoparkı zemin katı (prompt'lar verildi); kişili kareler sonra. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [ ] Dosya #005 baştan sona Play Mode oynanışı (kullanıcı).

### 4 Ekim 2026 — Dosya #006 "Son Mesaj"

- [~] Tasarım: `Docs/CASE006_DESIGN.md` — kayıp kişi; ilk kayboluş Defne'nin planı, 23.41 mesajı Cem'in, takip Halil'in. #005'ten ayrışsın diye kişi, meslek, usulsüzlük ve semt değişti.
- [~] Veri ve metin: `case006.json` (24 düğüm), `tr.case006.json` (323 metin). Doğrulayıcı vakayı otomatik çözüyor; `Case006Rules` kırılma kayıtlarının soruyla açıldığını ve "eksik doğru" raporların desteklenmediğini sabitliyor. 126/126 EditMode, 25/25 PlayMode.
- [~] Zincir: Dosya #005 → #006, haritada altıncı yuva; galeri listesinde.
- [~] Portreler (Buse, Halil, Onur, Cem, Defne, Sevim) — şimdilik yordamsal yer tutucu. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [~] Vaka kapağı. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [~] CCTV kareleri (apartman 22.38, kafe 20.49, sahil 21.51 / 22.02 / 00.06). — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [ ] Dosya #006 baştan sona Play Mode oynanışı (kullanıcı).

### 4 Ekim 2026 — Dosya #007 "Kül Payı" ve inceleme izni

- [~] İnceleme izni motoru: `Node.warrant` (dayanak yolları) + `warrantSlots`; `Investigation.SubmitWarrant` tam sayıda erişilebilir kaynak ister, yol tutarsa evrak talebi açar, tutmazsa aynı gecikmeyle "Ek dayanak gerekiyor" reddi yazar (`Progress.warrantDenials`). Ret yeniden denenebilir; ipucu verilmez.
- [~] İzin arayüzü: talep kâğıdında dayanak seçme satırları (`KarineUI.RequestBasis`), ret notu, rozet sayımı.
- [~] Doğrulayıcı: yürüyüş her izinli düğümde önce zayıf dayanağın reddini, sonra doğru yolun onayını sınar.
- [~] Tasarım: `Docs/CASE007_DESIGN.md` — şüpheli yangın; darbe Serdar'ın, yangın kasıtlı, ilk amaç sigorta.
- [~] Veri ve metin: `case007.json` (22 düğüm, 3 izinli), `tr.case007.json` (240 metin). `Case007Rules`: zayıf dayanak reddi, desteklenen rapor, "düştü"/"delilleri örtmek" desteklenmez. 126/126 EditMode, 25/25 PlayMode.
- [~] Zincir: Dosya #006 → #007, haritada yedinci yuva; galeri listesinde.
- [~] Portreler: Serdar, Melis, Erdal, Oğuz. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [~] Kapak ve sokak kamerası CCTV kareleri. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [ ] İzin akışı ve Dosya #007 baştan sona Play Mode oynanışı (kullanıcı).

### 4 Ekim 2026 — Dosya #008 "Kopya": olay bağlantısı, arşiv taraması, ayrılan dosya

- [~] Olay bağlantısı ve arşiv taraması: inceleme izni motorunun üstünde `Node.requestKind` ("link" / "archive"); yalnız metinler ayrılır (`requests.link*`, `requests.archive*`), kurallar aynı.
- [~] "Yeniden açıldı" kartı: `Node.reopenYear` taşıyan kayıt ilk açılışta `KarineUI.FileReopened` ile masaya düşer (sararmış klasör, yıl, damga, devir notu).
- [~] Ayrılan dosya: `CaseData.closedStampKey/closedNoteKey` → kapanış kartında "AYRILDI" ve not; `coldCaseTitleKey/StatusKey` → arşivde "SOĞUK DOSYA — UMUT YÜCEL / 2006" kartı (oynanmaz).
- [~] Tasarım: `Docs/CASE008_DESIGN.md` — üç olay, üç ayrı el; 2006'da itme düşme değil.
- [~] Veri ve metin: `case008.json` (32 düğüm; 2 bağlantı, 1 arşiv, 1 izin), `tr.case008.json` (309 metin). `Case008Rules`. 126/126 EditMode, 25/25 PlayMode.
- [~] Zincir: #007 → #008; bölüm seçicide her ülkeye sekizinci yuva (TR'de #008, diğerlerinde mühürlü).
- [ ] Olay panosu (kart dizme, bağlantı türü, puansız teori) — tasarım kullanıcıdan gelince.
- [~] Portreler: Selim, Tolga, Aylin, Zeynep, Ferhat, Burak. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [ ] Kapak, birleştirilmiş fotoğraf görseli ve CCTV kareleri (site otoparkı, apartman girişi).
- [ ] "Yeniden açıldı" kartı, "AYRILDI" damgası ve Dosya #008 baştan sona Play Mode oynanışı (kullanıcı).

### 4 Ekim 2026 — Dosya #009 "Emanet": soruşturma hattı, iç denetim zarfı, yetki

- [~] Soruşturma hattı: `requestKind: "line"`, `CaseData.lineSlots` (2), `Node.line` alt kayıt kapısı; hat kapatma, gecikmeli yeniden açma (`lineReopenPenaltySeconds`). Motor `Investigation.Lines.cs`.
- [~] İç denetim zarfı: `envelope*Key` → kapanış kartından sonra bir kez `FileReopened` ile "GİZLİ" zarf.
- [~] Yetki genişletildi: `grantsAuthority` + doğru rapor → kariyer kartında yetki satırı.
- [~] Tasarım: `Docs/CASE009_DESIGN.md`. Veri: `case009.json` (30 düğüm; 3 hat, 1 izin, 1 bağlantı), `tr.case009.json` (260 metin). `Case009Rules`; doğrulayıcı yürüyüşü biten hattı kapatır. 126/126 EditMode, 25/25 PlayMode.
- [~] Zincir: #008 → #009; her ülkeye dokuzuncu yuva.
- [~] Portreler: Arda, Hakan, Nesrin, Volkan, Ceren, Ali Rıza. Kapak, koridor CCTV kareleri, delil fotoğrafları. — dosyası yerinde (9 Ekim 2026 sayımı: 73 kapak, 355 portre, 97 CCTV karesi, #011–#073 bulguları); oyunda gözle bakılmadı.
- [ ] Hat yuvaları, zarf, yetki satırı ve Dosya #009 baştan sona Play Mode oynanışı (kullanıcı).

## Güncelleme kuralı

Her geliştirme oturumunun sonunda:

1. **Bu dosyada** yalnızca Unity'de veya cihazda **gözlenen** davranış `[x]` yapılır. Kodlanan ama oynanmayan iş `[~]`'dir; `[~]` hiçbir "Bitti ölçütü"nü karşılamaz.
2. Tarih yenilenir.
3. `Docs/STATUS.md` tek sayfalık özeti güncellenir.
4. Yeni tasarım kararı `Docs/DESIGN_AMENDMENTS.md`'ye, yeni teknik gerçek `Docs/Architecture.md`'ye yazılır.
5. Yeni engel ilgili aşamaya veya baştaki **Yayın engelleri** listesine eklenir.

Bir aşamanın bittiği, “Bitti ölçütü” gerçekleşmeden ilan edilmez. Sıradaki iş sırası için `Docs/PHASE_PLAN.md` esastır: bugün darboğaz yeni özellik değil, **Dosya #001'in ilk kez baştan sona oynanmasıdır** (Faz 2).

## Bölüm sonu görseli ve faks zamanlaması

- [x] Bölüm tamamlandı ekranı geniş fiziksel dosya, olay görseli, gönderilen rapor alanları, incelenen kaynaklar ve iki eylemle referansa yaklaştırıldı. Doğruluk/kariyer ödülü faks öncesi gizli. Unity ekran görüntüsü ve mobil ölçek testi açık.
- [~] Faks, rapordan sonra yedi saniyelik prototip gecikmesiyle Gelen Evraklar'a düşer; gerçek ikinci vaka zorunlu değildir. Bildirim ve bekleyen durum kayıtta korunur. Zamanlama akıcılığı, açık Unity Editor ve gerçek cihazda doğrulanmalı; yedi saniye nihai ritim kuralı değildir.

- [x] Bölüm özetinde kurumsal durum tek üst şeride taşındı; uzun ve tekrarlanan açıklama kaldırıldı. Unity görsel/ölçek doğrulaması açık.

## Marka kimliği (25 Eylül 2026 oturumu)

- [x] **KARINE logosu oyuna bağlandı.** Kullanıcının verdiği şeffaf PNG; `KarineLogo` tek kaynak/tek oran/tek doku yoğunluğu ile çiziyor, katmanlar `LogoBase` + `DistressOverlay`. Ana menüde büyük, arşiv/vaka seçicide ve ekran başlıklarında kompakt sürüm. Oran hem varlıkta hem **ekranda ölçülerek** Play Mode testiyle doğrulandı; gözle bakılmadı.
- [x] **İçe aktarım ezmesi düzeltildi.** Unity varsayılanı (`nPOTScale: 1`) logoyu 2000×667 → 2048×512 eziyordu; meta kilitlendi ve düşen test yeşile döndü.
- [x] **Yazı tipi rol tablosu** (`FontSet`): Heading / Mono / Body ayrıldı, eksik dosya mono'ya düşüyor ve doğrulayıcı not yazıyor. `RobotoSlab-ExtraBold.ttf` ve `Inter-Regular.ttf` **projede yok** — bu yüzden bugün ekranda görünür bir değişiklik yok.
- [x] **Bütün görsellerde içe aktarım oranı kilitlendi** (11 meta). Arka planlar yatayda ~%12, bayrak ~%33 esniyordu; artık esnemiyor. Doğrulayıcı kuralı eklendi ve düştüğü görülerek sınandı. Ekranlara gözle bakılmadı → `[~]`.

- [~] **Ana menü 27 Eylül görseline göre yeniden düzenlendi.** Var olan döngü videosu ve hata durumundaki durağan görsel korunuyor. Logo ve Türkçe slogan solda; kayda göre Devam Et/Oyuna Başla, Vakalar, Kariyer, Ayarlar, Hakkında beş eşit satırda; Bora portresi ve kimliği sol altta ayrı kartta. Mevcut ikon/portre sprite'ları ve `KarineUI` bileşenleri kullanıldı, menü ekranı tek bir arka plan PNG'sine çevrilmedi. Yeni kariyer ayarlarda, çıkış geri tuşu onayında erişilebilir. Kod ve test doğrulaması yapıldı; referansa göre yerleşim ve dar yatay ekran cihazda gözle doğrulanacak.
- [x] **KARINE UI/UX Kit bağlayıcı tasarım sistemi olarak uygulandı.** `KarineTheme` (palet, boşluk, köşe, dokunma, devinim, diegetic kâğıt katmanı) + `KarineUI` (dört düğme biçimi, ikon düğme, panel, sekme, rozet, modal, bildirim, evrak gezintisi, ilerleme, tooltip). Eski palet sabitleri temaya bağlandı, kâğıt renkleri adlandırıldı (38 tekrar kalktı), ham renk borcu 156'da kilitlendi. 19 yeni EditMode testi kit kurallarını (palet hex'leri, düğme hiyerarşisi, 48 px dokunma hedefi, modal sırası, tipografi rolleri, köşe dili, devinim süreleri) sınıyor. **Ekranlara gözle bakılmadı** → `[~]`.
- [x] **`BubeApp.cs` bölündü** — 3.125 satırlık tek dosya konu başına sekiz `partial` parçaya ayrıldı; en uzun dosya 529 satır. Doğrulayıcıya 560 satır kilidi kondu. Testler bölünmeden sonra da geçiyor, **ekranda ayrıca bakılmadı** → `[~]`.
- [x] **Gövde yazısı mono'dan çıktı, büyük başlıklar logo diline yaklaştı** — kök öğe IBM Plex Mono'ya bağlıydı ve bütün oyun monospace okunuyordu; kök Inter oldu, mono yalnız teknik metinde kaldı. Büyük başlıklar (≥28) Alfa Slab One. **Ekranda görülmedi** → `[~]`.
- [x] **Yazı tipi rolleri gerçek oldu** — Roboto Slab ExtraBold/Bold ve Inter Regular/SemiBold depoya kondu (LFS, lisanslarıyla), Türkçe glif kapsamı denetlendi, doğrulayıcı dört dosyayı zorunlu tutuyor. **Ekranda görülmedi** → `[~]`.
- [x] **Sinematikte tek denetim GEÇ** — hızlandırma, duraklatma ve ilerleme çubuğu kaldırıldı; bunlar CCTV'ye ait. GEÇ kit'in birincil düğmesi oldu (`KarineUI.SkipButton`, dolu krem zemin — saydam değil); CCTV'nin görüntü üstü düğmeleri de aynı dile bağlandı. **Ekranda görülmedi** → `[~]`.
- [x] **Ekranlar kit bileşenlerine taşındı** — dört sekme şeridi, iki masa uyarısı, iki onay modalı, ayarlar radyoları, kariyer durum göstergeleri, sinematik kontroller (duraklat/ilerleme/süre/İLERİ SAR/GEÇ) ve teknik metinler `KarineUI`ye geçti. Kâğıt tonları beşe indi, ham renk borcu 156 → 65. **Ekranlara gözle bakılmadı** → `[~]`.
- [x] **Kit ikon dili kesildi** — 13 ortak ikon doğrudan kit görselinden; menüye özel dört ikon silindi (aynı işlev = tek ikon). Varlıkları doğrulayıcıda aranıyor.
- [x] **Menü simgeleri maketten kesildi** — beş PNG (`Bube/Art/Icons/menu_*`), satır tonuyla boyanıyor; font glifleri kalktı. Simgelerin ekrana geldiği Play Mode testiyle doğrulandı, **gözle bakılmadı** → `[~]`.
- [x] **Kit taşıması kapandı** — elle kurulan 41 düğmeden 5'i kaldı (CCTV'nin metni çalışma anında değişen üç düğmesi kit boyasını `KarineUI.Paint`ten alıyor, masadaki görünmez `Hotspot`, menünün kendine özgü satırı). Kâğıt katmanı için `PaperButton`, iki katman için `CloseButton` eklendi. Ham renk borcu 65 → 16; kalan 16 bilerek kalıyor (piksel portre tonları ve CCTV efektleri = oyun sanatı). Perde/cam/dosya kabı için `Veil`, `GlassDeep`/`Glass`/`GlassLift`, `Alpha`, `HotspotHover`, `Paper.Folder`/`FolderEdge`/`FolderDeep`/`Board`/`Approved` token'ları eklendi. Ekranlardaki her punto `Typography.Snap`ten geçiyor; iki yeni kilit de kaldırılıp denendi, beklenen hatayı verdi. 77 EditMode + 7 PlayMode yeşil. **Ekranlara Play Mode'da bakılmadı** → `[~]`.
- [x] **Vaka eklemek koddan koptu** — vaka metni vaka başına dil dosyasına taşındı (`LocaleLoader`, çakışmada ortak dosya kazanır), yedek portrenin tonları vaka verisine çıktı (`Node.portrait`). Doğrulayıcı vakaya özel C# kuralı istemiyor. Yeni vaka = JSON + dil dosyası + varlıklar.
- [x] **Ses temeli kuruldu** — `AudioDirector` (müzik/oda ortamı/efekt), `SoundSettings` (üç kademe, ayarlarda radyo grubu), düğme sesi kit kurucusundan, oda sesi sahneden ve vakadan. Sistem dosyasız da sessiz çalışıyor. Duyulmadı → `[~]`.
- [x] **Ses varlıkları üretildi, sadeleştirildi ve duyuldu** — dokuz klip `Resources/Bube/Audio/` altında (LFS): `ui_press`, `ui_typewriter`, `ui_stamp`, `ui_notification`, `ui_chat`/`ui_chat_low`, `menu_theme` (32 s neo-noir döngü, Am–F–Dm–E), `desk_theme` (40 s masa müziği, Dm–Gm–B♭–A), `room_interview` (24 s döngü). Üç tur kullanıcı geri bildirimiyle sadeleşti: duvar saati, floresan uğultusu ve tavan çınlaması kaldırıldı, düğme yuvarlandı, odalar −26 dBFS; kâğıt sesi tamamen kalktı (kâğıt düğmesi ve evrak okları artık düğme sesi çalıyor); konuşma taklidi (sesli harf, formant, klavye tuşu) tamamen bırakıldı — görüşmede yumuşak bir **sohbet blibi** çalıyor, hiçbir şey taklit edilmiyor. Kayıt değil sentez: `Tools/make-audio.py` üretir, `Tools/check-audio.py` kırpma/DC/seviye/döngü dikişini ve müzikte akorları ölçer (0 bulgu). `ProjectRules` dokuzunun, `CaseRules` vakanın `ambienceId`sinin varlığını kilitliyor. Kullanıcı 25 Eylül 2026'da sesleri oyunda **duydu** → `[x]`.
- [~] **Mobil davranış** — Android geri tuşu her katmanda ekranın kendi geri eylemine gidiyor, ana menüde çıkış onayı açılıyor, `OnApplicationPause` kayıt yazıyor, vaka zinciri bittiğinde masa kapanış bildirimi gösteriyor. **Cihazda denenmedi** → `[~]`.
- [x] **Reklam dikişi** — `AdGateway` tek karar yeri (onay, "reklam kaldırıldı", an kuralları), `IAdProvider` + `NoAdProvider`. Araya giren reklam yalnız vaka arası, ödüllü yalnız rapor geri döndükten sonra; soruşturma/sorgu/CCTV/sinematik kapalı. Ödüllü ipucu vakanın gerçeğini görmüyor (yöntem + kendi kapsamı) ve doğrulayıcı sızıntıyı yasaklıyor. **Ağ eklentisi yok** (kimlikler kullanıcıda), ödüllü yeniden deneme güveni iade ediyor ve kaydı saklıyor → `[~]`.
- [x] **Ödüllü yeniden deneme** — `Investigation.MayReopen`/`ReopenForRetry`: iade tam o faksın götürdüğü kadar, gerekirse görevden ayrılma kalkıyor; `reviewHistory` satırı "yeniden açıldı" işaretiyle kalıyor ve ikinci deneme kendi satırını yazıyor; soruşturma korunuyor, yalnız rapor alanları boşalıyor. Yedi EditMode testi. Play Mode'da gözlenmedi → `[~]`.
- [x] **Sessiz düğmeler, duyulmayan müzik ve masanın müziği** — oyun baştan sona oynandıktan sonra gelen dört bulgu kapatıldı. (1) Masadaki nesneler (`Hotspot`: gelen evrak, dosya, terminal, görüşme), menü satırı ve CCTV oynatma düğmeleri **sessizdi**: ses kilidi yalnız `Runtime/UI` içine bakıyordu, artık bütün `Runtime`e bakıyor. (2) Ana menü müziği hiç duyulmuyordu: akış klibi `Play()` anında yüklü değildi ve Unity bunu sessizce geçiyor — içe aktarım artık önceden yüklüyor, `AudioDirector` de yüklenmemiş klibi yüklüyor; ayrıca "kısık" kademesi 0,35'ten 0,55'e çıktı. (3) Masada oda gürültüsü yerine **müzik** çalıyor (`desk_theme`, 40 s, menüden yavaş ve alçak); `room_office` silindi. (4) Ayarlar sayfası yenilendi: kart telefonda kenardan %5 (eskiden %27, dikey ekranda daracık bir şerit oluyordu), içerik kaydırılabilir, bölümler alt başlık ve çizgiyle ayrılmış. Ses beş kademeye çıktı (kapalı, %25, %50, %75, tam) ve radyo listesiyle seçiliyor; eski üç kademeli kayıt en yakın kademeye çevriliyor. Dördü de kullanıcı tarafından oyunda **görüldü ve duyuldu** (25–26 Eylül 2026) → `[x]`.
- [x] Arşiv kariyer ekranına, Hakkında ayarlara taşındı — menü maketteki beş satıra indi, iki işlev kaybolmadı.

## 27 Eylül 2026 — Vakalar referans tasarımı

- [x] Sol KARINE/Bora menüsü, yatay ülke kartları, fotoğraflı dosya kartları ve önceki/sonraki gezinme ortak UI bileşenleriyle kuruldu.
- [x] Ülke atlası eklendi; Dosya #001 mevcut bina görselini, #002 mevcut CCTV videosundan alınan kapak karesini kullanır. Yazılmamış vakalar ülke kartpostalını kullanır, hikâye görseli diye sunulmaz.
- [ ] Unity Game görünümünde referansla görsel karşılaştırma ve telefon üzerinde dokunma kontrolü.

## 27 Eylül 2026 — Masa referansı
- [x] Oda, ülke penceresi, beyaz dosya, telefon, tepsi, monitör ve delil öğeleri ayrı yeniden kullanılabilir katmanlara taşındı. Görev ve Notlar yok.
- [x] Mevcut kabul/açık/kapalı/emekli dalları ve evrak rozeti korunuyor; dokunma alanları en az 48 birim.
- [ ] Gerçek telefonda son görsel kabul ve dokunma denemesi.

Masa doğrulaması: 111 EditMode + 13 PlayMode geçti. Grafik etkin Unity testinde 1280×720 masa görüntüsü alındı; son yerleşim görsel olarak incelendi. Bu, fiziksel telefonda dokunma kabulünün yerine geçmez.

### 1 Ekim 2026 — yeni masa referansı

- [x] Masa sunumu: geniş ülke penceresi, bağımsız beyaz dosya/telefon/evrak/monitör/delil katmanları, konumlu başlık ve altı üst menü eylemi. Grafik etkin Unity çıktısında gözlendi: `Reference/DESK_2026-10-01.png`. 111 EditMode + 13 PlayMode geçti.
- [ ] Yeni masa düzeninin fiziksel telefonda dokunma ve okunabilirlik kabulü.

## 1 Ekim 2026 — Dosya Vaka Detay

- [x] **1 Ekim — Dosya Vaka Detay sunumu:** yeniden kullanılabilir Dossier bileşenleri, kâğıt dokusu, mevcut fotoğraf/portreler, sağ sekmeler. Unity render incelemesi yapıldı; mobil kabul açık. Arama ve mevcut bütün dosya araçları korundu.

## 1 Ekim 2026 — Gelen Evraklar sunumu

- [x] Gelen Evraklar yeni sunumu: marka/geri/üst araçlar, ikonlu evrak listesi, Tümü/Yeni/Okunan filtreleri, okunmamış işareti ve geniş kâğıt önizleme. 124 test geçti; fiziksel telefon kabulü açık.

Son doğrulama notu: Unity önizlemesi incelendi; ardından yalnız liste hizası ve yinelenen kurum başlığı düzeltildi. Bu iki sunum düzeltmesi sonrası test betiği lisans oturumu bulunamadığından yeniden çalışmadı. Önceki sürümde 111 EditMode + 13 PlayMode başarılıydı.

## 1 Ekim 2026 — Görüşmeler ve İncelemeler tableti

- [x] Talep tabletinde seçilebilir liste + ayrı detay ve eylem kartı, ortak üst gezinme. 111 EditMode + 13 PlayMode geçti. Telefon okunabilirliği/dokunma kabulü açık.

## 1 Ekim 2026 — CCTV tablet sunumu

- [x] CCTV tablet referansı: sol gerçek kamera kaynakları, sağ kayıt paneli, ortak üst gezinme. Metin sırası/netleştirme/video eylemleri korundu. 124 test başarılı; telefon kabulü açık.

## 1 Ekim 2026 — Vakalar yeni referansı

- [x] Vakalar yeni referansı: ülke kartlarında ilerleme ve kilit; seçili ülke panoraması; yedi kompakt dosya kartı ve sıra göstergesi; gerçek tamamlanan dosyalardan genel ilerleme. 124 test başarılı; telefon kabulü açık.


## 1 Ekim 2026 — Çubuksuz kaydırma

- [x] Oyun genelinde çubuksuz kaydırma: ortak bileşen ve tüm mevcut kaydırılabilir ekranlar güncellendi; fiziksel telefon kontrolü açık.


## 1 Ekim 2026 — Ayarlar referansı

- [x] Referansa yakın Ayarlar paneli: sol sekmeler, krem seçim kartları, taslak tercih/kaydet, varsayılanlar ve çubuksuz kaydırma. 111 EditMode + 13 PlayMode geçti; fiziksel telefon kabulü açık.


## 1 Ekim 2026 — Chakra Petch

[x] Bütün oyun metinleri Chakra Petch Regular/SemiBold/Bold ailesine geçirildi. FontSet ve içerik doğrulayıcı güncellendi; logo bitmap olarak korundu. OFL lisansı fontlarla birlikte eklendi. Fiziksel telefon okunabilirliği kontrolü açık.


## 1 Ekim 2026 — Ortak geçişler

[~] İlk animasyon paketi: dosya yerleşmesi, tablet kaldırma/indirme, düğme geri bildirimi ve hareket azaltma. Cihazda hareket/etkileşim kabulü açık.


## 1 Ekim 2026 — Evrak varışı

[x] Evrak varış ritüeli: kâğıt kayması + ui_fax + rozet ışığı; masaya dönüşlerde tekrar önleme. Gerçek cihaz kabulü açık.


## 1 Ekim 2026 — Dosya sekmesi geçişi

[x] Dosya sekmesi geçişleri: 200 ms sayfa yerleşimi, aktif sekme çıkıntısı, aynı sekmeye yeniden dokunmada animasyonsuz kalma; cihaz kabulü açık.


## 1 Ekim 2026 — Masanın havası (1. kademe)

[~] Lamba ışığı (nefes alan sıcak havuz) + köşe kararması + ışıkta süzülen 18 toz zerresi; eğince parallax (cihazda ivmeölçer, Editör'de fare; arka plaka %50 derinlik, taban kendiliğinden ortalanır); dokunulan masa eşyası kalkar, gölgesi yayılır, bırakınca oturur; tepsiye düşen kâğıt yan düşüp düzelir. Hareketi azalt açıkken toz, parallax ve kalkma kapalı. PlayMode'da katman yapısı ve kalkma/oturma ölçüldü; **görüntü ve eğme hissi gözlenmedi** (testler `-nographics`), cihaz kabulü açık.


## 1 Ekim 2026 — Efektler (2. kademe)

[x] Kayıt öne sürme: düğme parmakla karşıdakine doğru sürülebilir, dokunarak da seçilebilir; ikisinde de kayıt kâğıt olarak portreye kayar (doğru/yem/ilgisiz için aynı hareket), kayma sürerken ekran dokunmaya kapalı. Faks ve yeni gelen evrak satır satır basılır (düzen sabit, basılmamış düğme dokunulmaz, dokununca biter, aynı kâğıt bir kez basılır). Kamera değişiminde ve görüntü açılışında sinyal bozulması. Dosya sekmesi geçişine sayfa çevirme kenarı. Hareketi azalt / anında metin açıkken hepsi atlanır. `EffectsTests` (5) PlayMode'da geçti; **görüntü ve parmak hissi gözlenmedi**, cihaz kabulü açık.


## 1 Ekim 2026 — Efektler (3. kademe, göz kırpmasız)

[x] Sorguda portre nefes alır (4,2 sn döngü, binde 4–8 genişleme, göğüs hizasından); yanıttan önce 380 ms duraksama. İkisi de **herkeste, her yanıtta aynı**. Göz kırpma kullanıcı kararıyla yapılmadı. Hareketi azalt / anında metin açıkken ikisi de kapalı. PlayMode'da nefesin sınırı ölçüldü; **görüntü gözlenmedi**.

## 2 Ekim 2026 — Vakalar ve Kariyer yeniden tasarımı

- [x] Vakalar: ilerleme kutusunda harita simgesi, ülke kartında kilit, damgalı ülke şeridi, numaralı vaka kartları, adım noktaları. Unity'de görülmedi.
- [x] Kariyer / İstatistikler: tam ekran panel; profil kartı, Genel İstatistikler / Vaka Geçmişi / Dosya Arşivi sekmeleri, ilerleme, dört sayaç, halka grafik, ilk üç dünya. Uydurma veri yok. Unity'de görülmedi.

## 2 Ekim 2026 — Dosya #002 CCTV kareleri

- [x] Kamera 04 kare dizileri: Deniz (`park` 6, `passenger_out` 6), Emre (`second_in` 5, `contact` 4, `second_out` 5), Kerem (`third_in` 5, `third_out` 3), devriye (`patrol` 3). `dispute` bilerek yalnız metin. "POLİS" yazılı kare kural gereği alınmadı. Play Mode'da kare akışı görülmedi.

## 2 Ekim 2026 — Dosya #003 "Son Teslimat"

- [x] Tasarım: `Docs/CASE003_DESIGN.md` — şüpheli ölüm, ölüm kaza; olay yerini değiştiren kişi ölüme sebep olmadı. Yanlış yollar ve çelişki anları planlandı.
- [x] Veri ve metin: `case003.json` (12 düğüm), `tr.case003.json` (190 metin). Doğrulayıcının otomatik gezintisi vakayı baştan sona açıyor ve raporu gönderiyor; insan eliyle oynanmadı.
- [x] Rapor başlıkları vakadan: `suspectLabelKey`, `methodLabelKey` (sihirbaz, özet, faks, arşiv). Doğrulayıcı başlık metnini arıyor.
- [x] Zincir: Dosya #002 → #003, Türkiye haritasında üçüncü yuva bağlandı.
- [x] Portreler: Ozan, Seda, Barış (`Characters/`). Oyunda boyut/kırpma görülmedi.
- [x] Dosya #003 CCTV kareleri: beş kayıt kare dizisine bağlı (Ozan, Barış giriş/çıkış…); kullanıcı Unity'de oynadı.
- [x] Dosya #001 → #002 → #003 baştan sona Play Mode oynanışı (kullanıcı). **Kullanıcı Unity'de oynadı (4 Ekim 2026).**

## 2 Ekim 2026 — Oyunu derinleştiren yedi iş

- [x] Zaman çizelgesi: zaten vardı, yeni iş gerekmedi.
- [x] Defter: karşılaştırmadan üç hükümle (çelişiyor/örtüşüyor/sorulacak) kaynak çifti işleme, dosyada "Defter" sekmesi.
- [x] Belgelerde cümle altı çizme, deftere toplanma.
- [x] Faksta dosyanın akıbeti (üç vakanın bütün şüpheli ve ikinci sorumluluk seçenekleri); faks dördüncü sütunu da değerlendiriyor.
- [x] Vaka ortam sesleri: `room_night` (#002), `room_rain` (#003). Ölçüm aracı geçti; kulakla dinlenmedi.
- [x] Sicil kaydında akıbet ve dördüncü sütun.
- [x] Kapanan görüşme (`closesAfterRead`), doğrulayıcı güvencesi; Dosya #003'te Barış'ın ikinci görüşmesi.
- ~~Vakalar arası geri dönen isim~~ — kaldırıldı (kullanıcı kararı, 2 Ekim 2026): eski dosyanın faksı yeni dosyada gelir ve oyuncu eski dosyaya dönebilir; sonucu henüz bilinmeyen bir dosyanın kişisini yeni dosyada geri getirmek mümkün değil.

## 2 Ekim 2026 — Kare hızı ayarı

- [~] Ayarlar → Oynanış: 30 / 60 / 120 fps seçimi (varsayılan 60), açılışta uygulanır. Cihazda ölçülmedi.

## 2 Ekim 2026 — Efekt, ses ve sahne katmanı (A–G)
Hepsi `[~]`: derlendi, 117 EditMode + 23 PlayMode geçti; Unity'de/cihazda gözle görülmedi.
- [x] A Film katmanı: ince gren, tarama çizgisi ve vinyet her ekranın üstünde (`KarineUI.FilmLayer`). Kamera shader'ı UI Toolkit paneline ulaşmadığı için doku katmanı olarak yapıldı.
- [x] B CCTV: CRT açılış/kapanış, VHS izi (renk hayaleti, takip şeridi, seyrek sarsıntı), sinyal boşluğunda kar + zaman kodu atlaması, kıstırarak/çift dokunarak yakınlaştırma (2.5×, yakında görüntü durur).
- [x] C Masa: vaka saatine göre ışık tonu (#001 20:00, #002 23:00, #003 02:00), #003'te camda yağmur, gece far geçişi, fincandan buhar, lambanın yanması ve seyrek titremesi, eşya sesleri (sol-sağ konumlu).
- [x] D Evrak: belgeye özgü sabit yıpranma, faks mürekkep taşması, faksın ilk okunuşta makineden çıkması, altı çizmede kalem sesi, deftere ataç, zaman çizelgesine raptiye.
- [x] E Görüşme: yanıt beklerken üç nokta, portrede duruş kayması, piksel portrede göz kırpma, kaydı sürüklerken kâğıt kalkması, kapanan görüşmede kapı sesi, görüşme odasına kendi müziği (`interview_theme`, 36 s).
- [x] F Rapor ve geçişler: gönderimde "GÖNDERİLDİ" damgası + sarsıntı, ilk varışta dosya başlığının yazılması, menüde neon yanışı ve Ken Burns kayması, sayfa çevirmede titreşim.
- [x] G Ayarlar ve erişilebilirlik: Efektler Kapalı/Hafif/Tam, Titreşim aç/kapa, saniyede 3'ten az parlama sınırı, düşük pil/bellek/yavaş karede kendiliğinden hafifleme; "hareketi azalt" hepsini kapatır. Uzak ortam sesleri (araba, siren, köpek, telsiz) ofiste 25–70 s arayla.

## 2 Ekim 2026 — Sahne katmanı (H–O)

Hepsi statik doğrulamadan geçti (117 EditMode + 23 PlayMode); hiçbiri gözle görülmedi.

- [x] H — Geçişler: dosyalar arası sayfa (zaten vardı) + açılışta kâğıt esnemesi, masadaki eşyadan büyüyerek açılan ekran (dosya, gelen evrak, monitör), çekmeceden yükselen delil, görüşmeye girerken hat bağlanması, defter kapağı.
- [x] I — Görüşme odası: sallanan lamba ve gölgesi, kayıt öne sürülünce bardakta halka (her kayıtta aynı), ayna silueti, LED'li kayıt cihazı, vaka saatinden başlayan duvar saati; nefes zaten vardı.
- [x] J — Delil/defter: işarette kırmızı ip, zaman çizelgesinde iğne sarsıntısı, Polaroid banyo, fotoğrafta büyüteç ve çizim kipi (oturumluk), kâğıt esnemesi.
- [x] K — CCTV/terminal: geri sarma, kare geri düğmesi, terminal başlığı tuş tuş + imleç, arama sonuçları akarak, oturumda ilk açılışta "bağlanıyor…", ekran yansıması.
- [x] L — Atmosfer: ay ışığı (gece), yağmurda gök gürültüsü + pencere parlaması, perde gölgesi, isteğe bağlı kül dumanı (varsayılan kapalı), uzak ofiste çalan telefon ve daktilo.
- [x] M — Kapanış: damgadan önce zarf, faks ısınması, "Dosya kapandı" kartı ve rafa kalkan klasör (vaka başına bir kez), sicilde mürekkep kuruması.
- [x] N — Menü/kariyer: neon titremesi, güven rozeti parlaması + durum değişince mühür, vaka kartı çekilmesi (yerleşim değişmedi), Ayarlar'da efekt önizlemesi.
- [x] O — Teknik: gerçek CRT gölgelendiricisi (deneysel, varsayılan kapalı), ses+titreşim işaret tablosu (`KarineUI.Cue`), ekran okuyucu köprüsü, yazı boyu ayarı, şekil işaretleri, geliştirici kare sayacı.
- [ ] Odadaki nesnelerin ve kül alanının yüzde konumları görsellere göre gözle ayarlanmalı.
- [ ] CRT geçişi cihazda denenmeli (Y ekseni ters dönebilir; yalnız UI katmanını işler).

## 2 Ekim 2026 — Sahne katmanı (P–X)

117 EditMode + 23 PlayMode geçti; 3 yeni ekran görüntüsü testi başsız koşuda atlanır. Hiçbiri gözle görülmedi.

- [x] P — Ayar ve doğrulama: Efekt laboratuvarı (Ayarlar > Oynanış, yalnız geliştirme derlemesi), görüşme odası konum ayarlayıcısı (`Konum` → sürükle → `Kopyala`), ekran görüntüsü testleri (`ScreenshotTests`, referans `Tools/Baselines/`), kare bütçesi (hafifledikten sonra da yavaşsa oturumda "Hafif"e iner; sayaçta görünür).
- [x] Q — Anlatı: vaka açılış kartı + vaka motifi (vaka başına bir kez), mekân kareleri (`CaseData.locationFrames`; **henüz hiçbir vakada kare yok**), görüşmeden çıkışta sandalye sesi ve karanlıktan açılan ofis, masada 30 dakikada akşama dönen ışık, masada kapanmış dosyalar rafı.
- [x] R — Görüşme odası: kamera nefesi, kişiye özel bekleme döngüsü (kimlikten türer, yanıta göre değişmez), koridor adımları ve havalandırma, kayıt cihazında süre sayacı.
- [x] S — Masa: kâğıt parmağı izler ve yaylanarak yerine döner, fotoğrafta ivmeölçerle kayan parlama, defter işaretinde kalem çizgisi (el yazısı yazı tipi yok, eğik yazı), büyüteç çizimleri kalıcı, kahve 12 dakikada soğur.
- [x] T — CCTV/terminal: ½x/1x/2x hız, kareyi köşeye iğneleyip karşılaştırma, son aramalar (süzgeç birleşimleri), izlenme sayısıyla artan bant aşınması.
- [x] U — Menü/kariyer: ana menüde yağmur ve geçen far, kariyer geçmişinde gazete kupürleri, rütbe töreni, devam satırının altında son vaka ve oynanan süre.
- [x] V — Ses: oda yankısı (ofis/görüşme), uzak seslerde boğukluk ve geniş yön, masada süreyle açılan müzik katmanı, vaka motifi. 8 yeni ses.
- [x] W — Glitch: terminal satırının bir an bozulması, kanal değiştirme sesi, dosya kapanışında film yanığı, mühürde renk kayması.
- [x] X — Erişilebilirlik: ses betimlemesi, geniş aralık, yüksek karşıtlık (7:1'e itme), tek elle geri düğmesi, üç kademeli titreşim gücü.
- [x] Ekran görüntüsü referanslarını Editor Test Runner'da ilk kez üret (`Tools/Baselines/`). **4 Ekim 2026:** `Tools/run-tests.sh` ilk koşuda üretti (2400×1080).
- [ ] Disleksi dostu yazı tipi dosyası yok; şimdilik yalnız aralık açılıyor.

## 2 Ekim 2026 — Cila katmanı (Y–AF)

117 EditMode + 23 PlayMode geçti (3 ekran görüntüsü testi başsız koşuda atlanır). Hiçbiri gözle görülmedi.

- [x] Y — Dokunuş: parmak altında halka, düğmenin basılınca 1 px inmesi, basılı tutma çemberi (`KarineUI.Hold`, şimdilik yalnız kütüphanede), listelerde lastik sonu, dosya açılırken basılan düğmeden büyüyen kâğıt, bekleme için kâğıt karışması (`Shuffle`, yalnız laboratuvarda).
- [x] Z — Masa: lamba düğmesi (tercih saklanır), telefon kablosu, pencerede uzak şehir ışıkları, takvim yaprağı (vaka başına bir kez koparılır), takvimin yeri hatırlanır. Masa kül tablası dumanı zaten vardı.
- [x] AA — Görüşme odası: göz kırpma (`Characters/<kişi>_blink` görseli gerekir; **hiçbirinde yok**), floresan titremesi, aynada kayan parlama, sigara dumanı (`Node.smokes`; **hiçbir vakada işaretli değil**), kayıt türüne göre bırakma sesi. Nefes ve kayıt cihazı makarası zaten vardı.
- [x] AB — Bant/terminal: zaman damgası titremesi, ara sıra tekrarlanan kare (her kayıtta aynı olasılık), kayıt ilk açılışta yazıcı çıktısı, terminal başlığında fosfor kalıntısı, görüntü açılırken bant takılması. CRT ısınması zaten vardı.
- [x] AC — Anlatı: bölüm başı saat/yer kartı (`openingPlaceKey`; yoksa yalnız saat), dosyada sayfa değişince önceki sayfanın kenarda kalan yankısı, epilog (`epilogueImage`/`epilogueKey`; **görsel yok**), ilk kapanan vakadan sonra bir kez jenerik.
- [x] AD — Ses: `Resources/Bube/Music/<ad>` varsa sentez yerine o çalar (**bestelenmiş parça yok**), görüşme odasında fayans adımı, belge açıkken ortam kısılır, Android'de kulaklık algılanınca geniş stereo, seslendirme kancası (`Resources/Bube/Voice/<cevap anahtarı>`; **kayıt yok**). 5 yeni ses.
- [x] AE — Erişilebilirlik: renk körlüğü düzeltmesi (protan/deutan/tritan, tam ekran gölgelendirici geçişi, görseller dahil), sürekli yazı boyu kaydırıcısı (%90–150), Android "animasyonları kaldır" ayarına uyum, tek elle erişim (ekranı aşağı indiren düğme).
- [x] AF — Teknik: laboratuvarda efekt kaydı (PNG dizisi, GIF değil), 60 saniyelik kare süresi profili (CSV + özet), CRT gölgelendiricisinde film tanesi ve renk matrisi.
- [ ] Disleksi dostu yazı tipi dosyası (lisans kararı kullanıcıda).
- [ ] Gerçek cihazda ekran görüntüsü testleri.
- [ ] Büyük görsellerin sonradan indirilmesi (Play Asset Delivery / Addressables kararı).
- [ ] iOS için sistem "hareketi azalt" okuması (yerel eklenti gerekir).

## 2 Ekim 2026 — Reklam yerleri genişletildi

121 EditMode testi geçti (4 yeni reklam testi). Ağ hâlâ bağlı değil; hiçbir reklam gerçekten gösterilmiyor.

- [~] Vaka başı araya giren reklam (teklif kabul edildikten sonra, soruşturmadan önce).
- [~] Ana menüye dönüşte araya giren reklam (ilk açılışta değil).
- [~] Ortak sıklık sınırı: araya giren tüm reklamlar arasında en az 4 dakika; vaka başı ve menü reklamı yalnız ilk vakasını kapatmış oyuncuya.
- [~] Ödüllü görünüm: masa lambası rengi (4 renk, 3'ü kilitli), Ayarlar > Oynanış.
- [~] Ödüllü bekleme atlama: yalnız kalan süre 60 saniye ve üstüyse. **Bugünkü bekleyişler 4–10 saniye; düğme görünmez.**
- [ ] LevelPlay kurulumu (Unity Gaming Services oyun kimliği kullanıcıda).
- [ ] Gizlilik politikası, Play "Data safety", yaş derecesi.

## 2 Ekim 2026 — AdMob bağlandı (test kimlikleri)

- [~] Google Mobile Ads 11.5.0 (OpenUPM) kuruldu; `Bube.Ads` derlemesi temiz, 121 EditMode + 23 PlayMode geçti.
- [~] `AdMobProvider`: geçiş ve ödüllü reklam, Google UMP izin formu; yalnız mobil platformda devreye girer.
- [ ] Cihazda test reklamının görünmesi (Android derlemesi).
- [ ] Kullanıcının AdMob hesabı: gerçek uygulama ve reklam birimi kimlikleri.
- [ ] Gizlilik politikası, Play "Data safety", AdMob'da UMP mesajının oluşturulması.

### 4 Ekim 2026 — Dosya #010 "Kırık Zincir": kapsam genişletme, olay rekonstrüksiyonu, Türkiye bölüm finali

- [~] Tasarım: `Docs/CASE010_DESIGN.md`. Veri: `case010.json` (39 düğüm; 3 hat, kapsam bağlantısı, 2 CCTV), `tr.case010.json` (323 metin). `Case010Rules`. 126/126 EditMode, 25/25 PlayMode.
- [~] Olay rekonstrüksiyonu: `Investigation.Recon.cs`, `BubeApp.Recon.cs`, `KarineUI.ReconRow`; rapor adımı, faks satırı.
- [~] Bölüm finali sinematiği: `BubeApp.Finale.cs`, `KarineUI.Finale.cs` (telefon → personel değerlendirmesi → TAMAMLANDI → DOSYA 011).
- [~] Zincir: #009 → #010; her ülkeye onuncu yuva.
- [~] Görseller: Dosya #009–#010 portreleri (13 kişi, Taylan kurban — portre yok), iki kapak ve #010'un on CCTV olayı kare dizisi (taylan_in 5, diğerleri 3 kare). 126/126 EditMode, 25/25 PlayMode (1 atlandı). Unity'de görülmedi.
- [~] CCTV kareleri, yalnız seyri değiştiren olaylara (olay başına çoğunlukla 3 kare): #005 on olay; #006 defne_home, defne_out, defne_leave, woman_bench, man_bench; #007 kerem_in, car_stop, light (2 kare); #008 glass. Gömülü kamera yazısı yok, etiketi oyun çiziyor. **Rafa kalktı (5 Ekim 2026):** #008 drop, garden ve #009 arda_in, empty. Unity'de görülmedi.
- [~] #001–#003 portreleri (elif, hasan, mert, selcuk, deniz, emre, kerem, ozan, seda, baris) Gemini ile #004 sonrası çizim tarzında yeniden yapıldı; önden bakış, şeffaf zemin. Kerem Şahin 29 yaşına, Emre Selçuk'tan ayırt edilecek biçimde çizildi. Unity'de görülmedi.
- [~] #004–#006 portreleri (cem, derya, gulay, nihat, asli, kaan, levent, murat, nermin, buse, case006/cem, defne, halil, onur, sevim) Gemini ile aynı tarzda yeniden yapıldı; artık #001–#010 tek tarz. Sorgu odası ekran görüntülerinde görüldü, Unity'de oynanmadı.
- [~] Masa: eşyalar masa katmanıyla aynı ölçek ve kaymada (paralaks farkı kaldırıldı); CRT açıkken eşyaların havada durması giderildi. Unity'de gözlenmedi.
- [~] Sorgu odası: masa ön kenarı ahşap çizgisine indi (`TableEdge` .734, `Sink` .06); kişiyle masa arasındaki koyu şerit kalktı. Ekran görüntüsünde görüldü (2400×1080), cihazda bakılmadı.
- [~] Galeri turu: açılış kartı tur boyunca atlanıyor (oyuncu kaydı geri yükleniyor), süre sınırı 30 dk; #005–#010 turları çekildi.
- [ ] Rekonstrüksiyon ekranı, finale sinematiği ve Dosya #010 baştan sona Play Mode oynanışı (kullanıcı).

## 5 Ekim 2026 — Birleşik Krallık: Dosya #011 "Sis Altında"
- [~] Bölüm planı `Docs/UK_CHAPTER_PLAN.md`; ülke başına on dosya kanonu (kullanıcı kararı).
- [~] Veri: `case011.json` (21 düğüm), `tr.case011.json` (217 metin), `Case011Rules`; #010 → #011 zinciri, `Worlds.json` UK ilk yuvası. 126/126 EditMode, 25 PlayMode + 1 atlandı. Play Mode'da oynanmadı.
- [ ] Görseller: portreler (Daniel, Sam, Margaret, Graham, Lucy — Gemini), adli bulgu görselleri (Gemini), kapak ve CCTV 23.12 koşan figür 3 kare (ChatGPT). Kareler gelince `road#runner` olayına `framePaths` eklenecek.
- [~] Dünya geçişi: kod incelemesi önceki notu düzeltti — #010 faksı #010'un içinde okunmadan "sıradaki görev" düğmesi açılmıyor, yani sıra zaten Türkiye finali → faks → #011. Eksik olan UK açılışıydı: `worldIntros` içine `world02` eklendi; videosu olmayan dünya siyah ekran yerine kartpostal (`Bube/Worlds/uk`) üstünde ülke/şube ve stüdyo kartı gösterir, 5,6 sn sonra dosya bırakılır. Video gelince yalnız `videoPath` doldurulur. Başsız test `WorldWithoutVideo_ShowsCardThenDropsFile` geçiyor (126 EditMode, 26 PlayMode + 1 atlandı); kart gözle görülmedi.

## 5 Ekim 2026 — Birleşik Krallık: Dosya #012 "Son Sefer Değil"
- [x] Tasarım onaylandı: `Docs/CASE012_DESIGN.md` (biniş dökümünden zaman çizelgesi; dört katmanlı sürpriz: Kieran → ·4417 → Owen ölür → Helen).
- [~] Veri: `case012.json` (24 düğüm), `tr.case012.json` (250 metin), `Case012Rules`; #011 → #012 zinciri, `Worlds.json` UK ikinci yuvası. 126/126 EditMode, 26 PlayMode + 1 atlandı. Play Mode'da oynanmadı.
- [ ] Görseller: `Docs/CASE012_PROMPTS.md` — 5 portre ve 4 adli bulgu (Gemini), kapak ve 3 CCTV karesi (ChatGPT). CCTV gelince `street#runner` olayına `framePaths`.

## 5 Ekim 2026 — Birleşik Krallık: Dosya #013 "Kiracı"
- [x] Tasarım onaylandı: `Docs/CASE013_DESIGN.md` (bakım kayıtları; katmanlar: ihmal → kasıt → yanlış kurban → gizli kiralama).
- [~] Veri: `case013.json` (19 düğüm), `tr.case013.json` (204 metin), `Case013Rules`; #012 → #013 zinciri, UK üçüncü yuva. Çok günlü çizelge sırası korunarak 0–1439 aralığına sıkıştırıldı (doğrulayıcı tek gün ister). 126/126 EditMode, 26 PlayMode + 1 atlandı. Play Mode'da oynanmadı.
- [ ] Görseller: `Docs/CASE013_PROMPTS.md`. CCTV gelince `alley#ladder` olayına `framePaths`.

## 5 Ekim 2026 — Birleşik Krallık: Dosya #014 "İkinci Görüş"
- [x] Tasarım onaylandı: `Docs/CASE014_DESIGN.md` (çelişen iki dürüst rapor; ikinci görüş iki ayrı tablo olduğunu gösterir; intihar notu koparılmış bir cümle).
- [~] Veri: `case014.json` (23 düğüm), `tr.case014.json` (216 metin), `Case014Rules`; #013 → #014, UK dördüncü yuva. Asistanın adı "Ben" değil "Oliver": ad eşleşmesi Türkçe "ben" sözcüğünü yakalardı. 126/126 EditMode, 26 PlayMode + 1 atlandı. Oynanmadı.
- [ ] Görseller: `Docs/CASE014_PROMPTS.md`. CCTV gelince `yard#leave` olayına `framePaths`.

## 5 Ekim 2026 — Birleşik Krallık: Dosya #015 "Kapanış Saati" ve Olay Yeri Planı
- [x] Tasarım onaylandı: `Docs/CASE015_DESIGN.md` (dört izinli polis aynı cümleyi kurar; görüş hattı onları yalanlar; fail komiser yardımcısı; devir yazısı #020'ye tohum).
- [~] Yeni özellik — Olay Yeri Planı: `Node.scenePlan` (`ScenePlan`, `PlanPoint`, `PlanOccluder`, `PlanMarker`), `Investigation.MarkerAvailable`, saf geometri `SightLine` (kesişim, görüş, koni) + 6 EditMode testi; doğrulayıcı kuralları (`CaseRules`); `KarineUI.ScenePlanView` belgede çizilir: engeller, olay çarpısı, kilidi açık işaretler, dokununca koni. Başsız PlayMode testi (`ScenePlanTests`) kilidi ve dokunuşu gözledi. Elle oynanmadı; tasarım panosu bekleniyor.
- [~] Veri: `case015.json` (24 düğüm), `tr.case015.json` (252 metin), `Case015Rules` (plan geometrisini de sınar: dört beyan ve bar terminali olay noktasını göremez, barmenin penceresi görür); #014 → #015, UK beşinci yuva. "karakol" yasak kelime listesinde → "Asayiş Birimi". 132/132 EditMode, 27 PlayMode + 1 atlandı. Oynanmadı.
- [ ] Görseller: `Docs/CASE015_PROMPTS.md` — plan görseli gelince `scenePlan.imagePath` ve koordinat kontrolü; CCTV gelince `taxi#follow` olayına `framePaths`.

## 5 Ekim 2026 — Dosya sayısı: Türkiye 10, diğerleri 7
- [~] `Worlds.json`: Türkiye 10 yuva, diğer dokuz ülke 7 (boş yuvalar kırpıldı). `WorldRules` eşit yuva kuralı Türkiye'yi dışarıda tutar; `WorldTests.ShippedAtlasLoads` 10/7 sınar. Seçici Unity'de görülmedi.
- [x] Birleşik Krallık planı 7 dosyaya daraltıldı (`UK_CHAPTER_PLAN.md`); #017 final.
- [x] #016 konusu seçildi: "Başkasının Adı".


## 5 Ekim 2026 — Birleşik Krallık finali ve Almanya bölümü (kullanıcı ön onayı)
Kullanıcı yokken verdiği ön onayla yazıldı ("Birleşik Krallığı bitir, Almanya'ya geç, güçlü bir senaryo yaz, onaylıyorum"). Hepsi doğrulayıcıda çözülüyor; **hiçbiri Unity'de oynanmadı.** 132/132 EditMode, 27 PlayMode + 1 atlandı.
- [~] #016 "Başkasının Adı" — para izi + kimlik kayıtları (`CASE016_DESIGN.md`).
- [~] #017 "Köprü" — UK finali: santral izi, olay yeri planı, 7 kartlık rekonstrüksiyon, Almanya'yı açan final.
- [~] `ChapterFinale.callKeys` / `personnelBodyKey` — finale özel telefon ve personel metni; boşsa genel `finale.call.*`. `CaseRules` final metin anahtarlarını sınar.
- [~] `world03` açılışı: "Berlin — Kreuzberg şubesi", `Bube/Worlds/de` (görsel yok, kartpostal bekliyor).
- [~] Almanya planı: `DE_CHAPTER_PLAN.md` (Arendt ipi).
- [~] #018 "Birinci Kat" — çeviri tutanağı ile özgün ifade.
- [~] #019 "Canlı Yayın" — medya üstverisi.
- [~] #020 "Karartılmış Satır" — iki arşiv sürümü.
- [~] #021 "Kürek" — olay yeri planı, ikizler; ölen Lukas.
- [~] #022 "Gece Treni" — vagon planı, terminal kayıtları; üç tanık üç ceza.
- [~] #023 "Ev Hırsızı" — gerçek hırsızlık saatler sonra; katil Arendt'ten para alan analist.
- [~] #024 "Defter" — Almanya finali: kurulmuş ölüm, 7 kartlık rekonstrüksiyon, JAPONYA / DOSYA 025.
- [ ] Görseller: `CASE016_PROMPTS.md` … `CASE024_PROMPTS.md` (portre, kapak, CCTV, planlar; plan görselleri gelince koordinat kontrolü).
- [ ] Kullanıcı Almanya senaryosunu gözden geçirecek.
- [~] Japonya bölüm planı (#025–#031) — aşağıda.


## 5 Ekim 2026 — Japonya'dan Avustralya'ya: bütün dünyalar yazıldı (kullanıcı ön onayı)
Kullanıcı kararı: "Japonya dahil bütün dünyaları ve vakaların senaryosunu yaz ve bitir; birbirine bağla; kişi sayısını dağıt; yeni özellik ekleyebilirsin." Her dosya doğrulayıcıda çözülüyor, kendi `CaseNNNRules.cs`'i var; **hiçbiri Unity'de oynanmadı.** 132/132 EditMode, 27 PlayMode + 1 atlandı.
- [~] Japonya #025–#031 (`JP_CHAPTER_PLAN.md`, `world04` Tokyo — Nihonbashi). İp: Kisaragi → Paris.
- [~] Fransa #032–#038 (`FR_CHAPTER_PLAN.md`, `world05`). Mekanik: gerekçeli arama izni. Final Delorme → Chicago.
- [~] ABD #039–#045 (`US_CHAPTER_PLAN.md`, `world06`). Mekanik: soruşturma hatları (aynı anda iki). Final Lakeshore → Napoli.
- [~] İtalya #046–#052 (`IT_CHAPTER_PLAN.md`, `world07`). Mekanik: tanıklar beklemez (`closesAfterRead`). Final Caruso → Sevilla.
- [~] İspanya #053–#059 (`ES_CHAPTER_PLAN.md`, `world08`). Mekanik: arşivden yeniden açma. Final Santa Ana → Montreal.
- [~] Kanada #060–#066 (`CA_CHAPTER_PLAN.md`, `world09`). Mekanik: yem kayıtlar. Final Laurentide → Melbourne.
- [~] Avustralya #067–#073 (`AU_CHAPTER_PLAN.md`, `world10`). Bütün aletler karışık; #073 "İrtibat" son dosya, `finale.country.archive` / `finale.file.end`, telefon çalmaz, E. V. bulunmaz.
- [~] Vaka zinciri onarıldı: #048 → #049 ve #055 → #056 `nextCaseId` boştu (doğrulayıcı notu "zincire bağlı değil: case049"); şimdi #001–#073 kesintisiz.
- [~] Kural üreticisi `decoys`, `archive`, `lines`, `closes`, `recon`, `finale` anahtarları (bkz. `Architecture.md` → "Vaka üretim hattı").
- [ ] Görseller: `CASE025_PROMPTS.md` … `CASE073_PROMPTS.md` (portre, bulgu, kapak, CCTV), `EVIDENCE_PROMPTS.md` (#013–#024 ayrıntılı bulgular), `WORLD_BACKDROP_PROMPTS.md` (jp … au kartpostalları).
- [~] Bulgu bağlama aracı: `Tools/bind-items.py` + `Tools/item_bindings.json` (186 bulgu, #013–#073; her biri bir belgeye eşli). Görseli olan bulguyu `relatedItems`'a ve yerele yazar, `.meta`'yı `nPOTScale: 0` ile kurar; görseli olmayanı atlar. Geçici görselle denendi, 132/132 geçti.
- [ ] Görseller geldikçe `python3 Tools/bind-items.py` çalıştırılacak (şu an 186 bulgu görsel bekliyor).
- [ ] Kullanıcı bütün bölümleri gözden geçirecek; Unity'de her bölümden en az bir dosya oynanacak.

## 7 Ekim 2026 — Sorgu akışı ve CCTV videosunun kalkması
- [~] Oyunda CCTV videosu kalmadı; #001'de yalnız 12.16 çıkışı iki kare, diğer olaylar metin. Bütün CCTV olaylarının kareleri girdi. Play Mode'da görülmedi.
- [~] Sinyal boşluğu / "kayıt yok" satırı öne sürülemez; doğrulayıcı kuralı. #001 `gap`, #004 `cam2_lost`.
- [~] Yem soruyu yeni ilerlemeye kadar kapatır (`Investigation.Decoy.cs`, `DecoyHoldTests`). Play Mode'da hissi gözlenmedi.
- [~] 73 dosyanın sorguları okundu: aynı kişiye aynı kaydın iki kez sunulması (#013, #027), boş tekrar soru (#009 `hakan_2.after`, #001 `hasan_follow.memory`), yinelenen cümleler (#001, #011), saat çelişkisi (#003), yazım (#033). 133/133 EditMode.
- [ ] Kullanıcı #001 ve #002'yi yeniden oynayıp sorgunun artık tekrar etmediğini doğrulayacak.

## 8 Ekim 2026 — Çok dil ve kariyer sicili

- [~] Dil altyapısı: tr/en/de/fr/it/es/pt-BR; cihaz dili → İngilizce → Türkçe düşme zinciri; Ayarlar → Genel'de dil seçimi (yalnız kurulu diller).
- [~] Ad geçme kuralı her dilde Türkçe kanondan hesaplanıyor (`RuleText`); test: `TranslationTests`.
- [~] Büyük harf ve yüzde oynanan dilin kurallarıyla (`TextCulture`, `Percent`).
- [~] Çeviri denetimi: Unity `TranslationRules` + `Tools/check-translation.py`; kılavuz `TRANSLATION_GUIDE.md`.
- [~] Kariyer ekranı **Sicil** sekmesi: güven seyri, kademe değişimleri, ülke karnesi.
- [~] İngilizce çeviri: 12.755 satır (ortak + 73 vaka), denetim 0 sorun; sözlük `Translation/GLOSSARY_en.md`. Oyunda okunmadı.
- [~] Almanca çeviri (9 Ekim 2026): 12.833 satır (ortak + 73 vaka), denetim 0 sorun; sözlük `Translation/GLOSSARY_de.md`. Oyunda okunmadı.
- [ ] Giriş ekranı Apple/Google logoları **temsilî** (9 Ekim 2026, kodla çizildi): mağazaya göndermeden önce resmî dosyalarla değiştir — `Bube/Art/Icons/apple.png` (beyaz), `google.png` (dört renkli G). Giriş ekranının yatay maketi `[~]`, Unity'de görülmedi.
- [~] Açılış videosu (`splash_loop.mp4`) yükleme ekranının arka planında; başsız PlayMode ekran görüntüsünde oynadığı görüldü, cihazda denenmedi (9 Ekim 2026).
- [ ] Çeviriler: fr, it, es, pt-BR — her biri tüm satırlar, denetim 0 sorun. (Kullanıcı onayı bekleniyor.)
- [ ] Her dilde #001 Play Mode'da oynanır; Almanca ve Fransızcada uzun metnin düğme/sekme taşması gözle kontrol edilir.
- [ ] Mağaza sayfası metinleri altı dilde.
- [ ] Sicil sekmesi Play Mode'da görülür (boş kariyer, birkaç faks, kademe düşüşü).

- [~] 8 Ekim 2026 geri bildirim turu: dil açılır listesi, 120 Hz isteği, oyun içi yükleme ipuçları, video/CCTV'de müzik susar, tam sayfa kaydırma, müzik yumuşatma, daktilo basımı, telefon sesi, CCTV siyahtan açılış + 3 tur.
