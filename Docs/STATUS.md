# Karine — durum özeti

**Son güncelleme:** 9 Ekim 2026 (mağaza hazırlığı)
**Bu dosya:** projeye bakan herkesin ilk okuyacağı tek sayfa. Ayrıntı için [ROADMAP.md](ROADMAP.md), kanıt için [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md), ileri plan için [PHASE_PLAN.md](PHASE_PLAN.md).

## Tek cümle

**9 Ekim (gece, 4):** Gelir modeli kodlandı.
- İzin gerektiren incelemeler 20–30 dakika sürüyor. Her ödüllü reklam süreyi 10 dakika kısaltıyor, yani 2–3 reklamla hemen alınıyor.
- "Kalıcı öncelik" satın alması eklendi.
- Lamba için iki yeni renk eklendi.
- Bütün ülkeler ücretsiz kalıyor.
- Durum `[~]`.

**9 Ekim (gece, 3):** Masada **Çalışmam** düğmesi eklendi: yöntem hatırlatması ve kapsama sayıları, artık reklamsız. Vaka özetine **Çalışman** bölümü eklendi. **Yerel hatırlatma bildirimleri** kuruldu: dosya beklerken ertesi gün ve 3 gün sonra birer kez. Metinler yalnız tr/en/de. Hepsi `[~]`. Ekran görüntüsü testi `Account_MatchesBaseline` düşüyor; sebep ışık animasyonunun anı ve referanstaki eski fps sayacı. Referans yenilenmeli.

**9 Ekim (gece, 2):** **Mağazaya hazırlık (kod).**
- **Satın alma:** Reklamları kaldır + Satın alımları geri yükle (Unity IAP).
- **iOS:** takip izni (ATT).
- **Puan:** 3. dosyadan sonra oyun içi puan isteme.
- **Çökme raporu:** Crashlytics, paket gelince derlenir.
- **Kayıt:** bozuk kayıtta yedekten dönüş.
- **Yasal metinler:** gizlilik ve silme sayfası taslakları `Docs/Legal`.
- **Düzeltme:** görseller sanıldığı gibi eksik değil (73 kapak, 355 portre, 97 CCTV karesi yerinde); ROADMAP'teki eski satırlar düzeltildi.
- **Sağlayıcı:** Firebase ayarlıysa eski UGS sağlayıcısının yerini alıyor.
- **Sana kalanlar:** [STORE_ACCOUNTS_CHECKLIST.md](STORE_ACCOUNTS_CHECKLIST.md) → 9 Ekim eki.
- **Doğrulama:** 143 EditMode + 28 PlayMode → `[~]`.

**9 Ekim (gece):** **Hesap ve bulut kaydı tamamlandı (kod).**
- **Giriş ekranı zorunlu:** Apple, Google ya da misafirden biri seçilmeden oyuna girilmez. Geri tuşu ekranı geçmez. Seçim bir kez yapılır.
- **Misafir bulutta:** Firebase anonim kimlik + Firestore (REST).
- **Yeniden kurulum:** misafir aynı profile döner (iOS Keychain / Android Block Store).
- **Ne buluta gider:** yalnız kariyer belgesi, yalnız dönüm noktasında (rapor gönderildi, dosya değerlendirildi, rütbe ya da ödül değişti). Vaka içi iz cihazda kalır.
- **Kariyer kaydına taşınanlar:** lamba ödülleri, jenerik, görülen rütbe, oynama süresi.
- **Hesap bağlama önerisi:** misafire Dosya #002 kabulünde bir kez çıkar. Ayarlar'da Apple/Google bağlama hep görünür.
- **Silme:** uygulama içinden; bulutu, kimliği, kalıcı anahtarı ve cihazı siler.
- **Kota:** 10 bin oyuncu hesabı ve KVKK listesi [FIREBASE_AND_KVKK.md](FIREBASE_AND_KVKK.md) içinde.
- **Çeviri:** giriş metinleri fr/it/es/pt-BR için hazır; bu diller tam çeviriye kadar gizli.
- **Sırada (kullanıcıda):** Firebase konsolu → Web API Key + Project ID. Ardından Apple/Google girişi Firebase'e bağlanacak.
- **Doğrulama:** 143 EditMode + 28 PlayMode geçiyor. Android ve iOS dalları başsız derlendi. **Cihazda ve gerçek Firebase'le denenmedi** → `[~]`.

**9 Ekim (akşam):** **Giriş ve açılış ekranları yeni.** Giriş: yatay maket, yazısız masa arka planı, solda logo ve dönen ifadeler, sağda düz dosya kâğıdı; Apple/Google logoları temsilî. Açılış: Gemini videosu (yağmurlu adsız şehir, Bora) üstünde sırayla KARINE ve Bube Games logoları, yükleme sağ altta; "Made with Unity" kapalı. Başsız PlayMode ekran görüntüleriyle görüldü, cihazda değil. 143 EditMode + 28 PlayMode.

**9 Ekim:** **Almanca çeviri tamamlandı.** 73 dosya ve ortak arayüz, 12.833 satır; çeviri denetimi 0 sorun. Sözlük: [Translation/GLOSSARY_de.md](Translation/GLOSSARY_de.md). 143 EditMode geçiyor. Kalan diller (fr, it, es, pt-BR) kullanıcı onayı bekliyor. **Almanca metin oyunda okunmadı** → `[~]`.

**8 Ekim (gece, 4):** **Hesap sistemi kuruldu.** Misafir, Google (Android) ve Apple (iOS) girişi; hesaba göre kayıt ve buluttan devam; uygulama içinden hesap silme, çıkış, gizlilik bağlantısı; cihaz dili desteklenmiyorsa ülkeye göre dil. Konsol/mağaza adımları kullanıcıda: [STORE_ACCOUNTS_CHECKLIST.md](STORE_ACCOUNTS_CHECKLIST.md). 143 EditMode + 27 PlayMode → `[~]`.

**8 Ekim (gece, 3):** Bora'nın göreve hazırlanış filmi eklendi; yeni kariyerde Türkiye açılışından önce oynar. 139 EditMode + 27 PlayMode → `[~]`.

**8 Ekim (gece, 2):** Zor dosyalar güveni daha cömert etkiliyor; Kariyer sayfasına Bora sekmesi (profil + güven kuralları) eklendi; Bora artık uluslararası birimde. 139 EditMode → `[~]`.

**8 Ekim (gece):** **Kariyer artık bitmiyor.** Güven sıfırlanınca Bora aynı ülkede gözetimli masa görevine alınıyor, sıradaki dosyayı gerekçeli kapatınca dönüyor; 3 doğru raporluk seri ek güven veriyor. 138 EditMode geçiyor → `[~]`.

**8 Ekim (akşam):** **İngilizce çeviri tamamlandı.** 73 dosya ve ortak arayüz, 12.755 satır; çeviri denetimi 0 sorun. Sözlük: [Translation/GLOSSARY_en.md](Translation/GLOSSARY_en.md). Diğer diller (de, fr, it, es, pt-BR) kullanıcı onayı bekliyor. **İngilizce metin oyunda okunmadı** → `[~]`.

**8 Ekim:** **Oyun altı dile hazırlanıyor; kariyer ekranına Sicil sekmesi eklendi.** Diller: İngilizce, Almanca, Fransızca, İtalyanca, İspanyolca, Portekizce (BR); Türkçe kanon. Dil seçimi, cihaz diline göre açılış ve eksik satırda İngilizce → Türkçe düşme zinciri kuruldu. Ad geçme kuralı her dilde Türkçe metinden hesaplanıyor; çeviri denetimi Türkçedeki kuralların karşılığını her dile uyguluyor ([TRANSLATION_GUIDE.md](TRANSLATION_GUIDE.md)). Büyük harf ve yüzde artık dile göre. **Çeviri metinleri henüz yazılmadı.** 136 EditMode + 27 PlayMode geçiyor; hiçbiri Play Mode'da görülmedi.

**7 Ekim:** **Sorgu akışı ayıklandı ve oyunda CCTV videosu kalmadı.** Sinyal boşluğu ya da "kayıt yok" satırı artık görüşmede öne sürülemiyor (doğrulayıcı kuralı). Yanlış ama ilgili kayıt sunulup yanıt alınınca soru, oyuncu yeni bir şey okuyana ya da başka soru sorana kadar kapanıyor. 73 dosyanın bütün sorguları okundu; aynı kişiye aynı kaydın iki kez sunulması, boş tekrar sorular, yinelenen cümleler ve bir saat çelişkisi düzeltildi. Bütün CCTV olaylarının kareleri girdi; #001'de yalnız 12.16 çıkışı karelendi. 133/133 EditMode; **hiçbiri Play Mode'da görülmedi.**

**5 Ekim (gece, 7):** **Bütün dünyalar yazıldı: on ülke, 73 dosya veride.** Japonya (#025–#031), Fransa (#032–#038), ABD (#039–#045), İtalya (#046–#052), İspanya (#053–#059), Kanada (#060–#066) ve Avustralya (#067–#073). Her bölüm bir alet ekler ve öncekileri korur: gerekçeli izin, soruşturma hatları, beklemeyen tanıklar, arşivden yeniden açma, yem kayıtlar; Avustralya hepsini karıştırır. Kişi sayısı dosya başına 3–7, dağınık. Bölümleri bağlayan ip Arendt → Kisaragi → Delorme → Lakeshore → Caruso → Santa Ana → Laurentide → Pacific Liaison Trust; hepsi "Uluslararası İrtibat Programı"na ve direktör Edda Varga'ya çıkar. #073 son dosyadır: telefon çalmaz, defter Budapeşte'ye gitmiştir, dosya açık kalır. Vaka zincirindeki iki kopukluk (#048, #055) onarıldı. Testler yeşil (132 EditMode, 27 PlayMode); **#011–#073 hiçbiri Unity'de oynanmadı.** Görsel prompt'ları hazır: vaka başına `CASE0NN_PROMPTS.md`, `EVIDENCE_PROMPTS.md`, `WORLD_BACKDROP_PROMPTS.md`.

**5 Ekim (gece, 6):** Kullanıcının ön onayıyla **Birleşik Krallık tamamlandı (#016–#017)** ve **Almanya bölümü yazıldı (#018–#024)**. Bölümün ipi Arendt Handelsgesellschaft: Londra'daki rüşvetin parası Berlin'den geliyor, #019/#021/#023'te birer satırla geçiyor, #024 "Defter"de ölümcül hasta kurucu kendi ölümünü cinayet gibi kurup defteri bube'ye ulaştırıyor; son sayfa Tokyo'yu gösteriyor. Finaller artık ülkeye özel telefon/personel metni taşıyabiliyor; `world03` Berlin açılışı eklendi. Testler yeşil (132 EditMode, 27 PlayMode); **hiçbiri Unity'de oynanmadı**, görseller bekliyor (prompt dosyaları hazır).

**5 Ekim (gece, 5):** Dosya sayısı yeniden kararlaştırıldı: **Türkiye 10, sonraki dokuz ülke 7'şer** (toplam 73). `Worlds.json` yuvaları kırpıldı, doğrulayıcı ve test buna göre; Birleşik Krallık #011–#017 (#016 seçilecek, #017 final).

**5 Ekim (gece, 4):** **Dosya #015 "Kapanış Saati"** veride ve yeni özellik **Olay Yeri Planı** kodda: kuşbakışı krokide engeller, olay çarpısı ve tanık işaretleri; işarete dokununca görüş konisi, engeller gölge bırakır. Dört izinli polis "sigara alanından gördük" der; plan duvarın ve kamyonetin görüşü kestiğini gösterir, oyun bunu söylemez. 24 düğüm, 252 metin. Testler yeşil (132 EditMode, 27 PlayMode); elle oynanmadı.

**5 Ekim (gece, 3):** **Dosya #014 "İkinci Görüş"** veride: rezil edilmiş müzayede uzmanının "intiharı". Ters köşe: çelişen iki uzman raporu da doğru — ikinci görüş iki ayrı tabloya bakıldığını gösterir; alıcı Ashby tabloyu satıştan sonra kopyayla değiştirmişti. "Yanıldım" notu, defterden koparılmış "Yanıldım sandılar…" cümlesinin başı. 23 düğüm, 216 metin. Testler yeşil; oynanmadı.

**5 Ekim (gece, 2):** **Dosya #013 "Kiracı"** veride: paylaşımlı evde karbonmonoksit. Bakım kayıtları (sertifika, servis raporu, anahtar kaydı) kaza/ihmal/kasıt ayrımını oyuncuya bırakır. Katmanlar: ihmal → bacada bez → ölen kişi kiracı değil, hedef Ellie → emlakçı Simon odayı ev sahibinden gizleyip kiralıyordu. 19 düğüm, 204 metin; UK üçüncü yuva. Testler yeşil; oynanmadı.

**5 Ekim (gece):** **Dosya #012 "Son Sefer Değil"** veride: gece otobüsünde saldırı, biniş dökümünden zaman çizelgesi (yalnız biniş okutulur; adsız ·4417 kartını banka hareketleri Liam'a bağlar). Kullanıcı isteğiyle dört katmanlı sürpriz: kavgacı genç → adsız kart → kurban ölür, dosya cinayete döner → ihbarı yaptıran amir Helen. Rapor: Liam + kışkırtma + Helen + banka hareketleri. 24 düğüm, 250 metin; UK ikinci yuvası. Testler yeşil; Unity'de oynanmadı. Bekleyen: #011 ve #012 görselleri.

**5 Ekim (akşam):** **Birleşik Krallık bölümü başladı.** Ülke başına on dosya kanonlaştı. **Dosya #011 "Sis Altında"** (Rotherhithe, rıhtımda ölüm; özet ile tam döküm) veriye girdi: 21 düğüm, 217 metin, #010'un ardından zincirde, UK'nin ilk yuvasında. Testler yeşil; Unity'de oynanmadı. Bekleyen: portreler ve adli görseller (Gemini), kapak ve CCTV kareleri (ChatGPT), dünya geçişi sırası.

**5 Ekim:** CCTV kareleri yalnız soruşturmanın seyrini değiştiren olaylara konuyor; geri kalanı metin. #005 tamam; #006 beş, #007 üç, #008 bir olayın kareleri girdi. Görüntüye gömülü kamera yazısı yasak (dil değişince kalıyor). Kalan dört olay (#008 drop/garden, #009 arda_in/empty) rafa kaldırıldı. #001–#003'ün on portresi Gemini ile yeni tarzda yeniden çizildi; tüm vakalar artık tek portre tarzında. Testler yeşil (126/126 EditMode, 25 PlayMode + 1 atlandı). Unity'de görülmedi. Ardından #004–#006 portreleri de Gemini ile yenilendi (tüm vakalar tek tarz). Masada eşyaların havada durması (paralaks farkı) ve sorgu odasında kişiyle masa arasındaki koyu şerit düzeltildi. Galeri turu açılış kartında takılmıyor; #005–#010 tur görüntüleri alındı. Testler yeşil.

**4 Ekim (gece, 7):** #009–#010 portreleri, kapakları ve #010'un on CCTV olayı girdi. **#005–#009'un CCTV kareleri (40 olay) henüz yok.** Kare sayısı artık olaya göre — çoğu olay 3 kare yetiyor. Testler yeşil; Unity'de görülmedi.

**4 Ekim (gece, 6):** **Dosya #010 "Kırık Zincir"** — Türkiye finali. Yeni: kapsam genişletme (yangın kaydını dosyaya almak), rapordan önce yedi kartlık olay rekonstrüksiyonu (sıra + kaynak) ve bir kez oynayan bölüm finali sinematiği (telefon, personel değerlendirmesi, TÜRKİYE 10/10, DOSYA 011 Birleşik Krallık). 39 düğüm, 323 metin; onuncu yuva. Testler yeşil; Play Mode'da görülmedi.

**4 Ekim (gece, 5):** **Dosya #009 "Emanet"** ve **soruşturma hattı** girdi: oyuncu üç hattan (Arda / Emanet / Operasyon) aynı anda yalnız ikisini açık tutabiliyor; biri kapatılınca yer açılıyor, kapanan hat gecikmeyle yeniden açılıyor. Vaka sonunda "BDS İÇ DENETİM — GİZLİ" zarfı ve doğru raporda "Yetki: GENİŞLETİLDİ". 30 düğüm, 260 metin; dokuzuncu yuva. Testler yeşil; Play Mode'da görülmedi.

**4 Ekim (gece, 4):** **Dosya #008 "Kopya"** girdi: oyuncu üç olayı (zarf, araç camı, ev girişi) kendisi ilişkilendiriyor, birleşik fotoğrafla 2006 arşivini taratıyor. Eski dosya ilk açılışta "YENİDEN AÇILDI" kartıyla masaya düşüyor; rapor sonunda dosya "AYRILDI" damgası alıyor ve arşive soğuk dosya kartı ekleniyor. 32 düğüm, 309 metin; bölüm seçicide sekizinci yuva. Testler yeşil; Play Mode'da görülmedi. Olay panosu tasarım bekliyor.

**4 Ekim (gece, 3):** **İnceleme izni** mekaniği ve **Dosya #007 "Kül Payı"** (şüpheli yangın) girdi. Bazı kayıtlar (araç, şirket kartı, servis koridoru) ancak oyuncunun seçtiği iki dayanakla talep edilebilir; dayanak yetersizse talep "Ek dayanak gerekiyor" diye döner. 22 düğüm, 240 metin, #006'nın ardından zincirde. Testler yeşil; Play Mode'da henüz görülmedi. Portre, kapak, CCTV sonra.

**4 Ekim (gece, 2):** **Dosya #006 "Son Mesaj"** (kayıp kişi, iki katmanlı) veriye girdi: 24 düğüm, 323 metin, #005'in ardından zincirde. Rapor: ilk kayboluş / 23.41 mesajını kim gönderdi / takibi kim organize etti. Doğrulayıcı vakayı çözüyor (`Case006Rules`). Portre, kapak ve CCTV kareleri sonra.

**4 Ekim (gece):** **Dosya #005 "Son Görüldüğü Yer"** (kayıp şahıs) veriye girdi: 21 düğüm, 280 metin, #004’ün ardından zincirde. Rapor üç sütun: kaybolmanın niteliği, bağlantılı kişi+nitelik, fiziksel saldırı. Vaka Kasım 2027'de (14–15 Kasım). Doğrulayıcı vakayı çözüyor (`Case005Rules`); portreler girdi. Bekleyen: kapak ve CCTV için iki boş zemin (bina girişi, otopark), ardından kişili kareler.

**4 Ekim (akşam):** **Dosya #004 "Sessiz Kat"** veriye girdi (18 düğüm, 247 metin) ve #003'ün ardından zincire bağlandı. Doğrulayıcı vakayı baştan sona açıyor; insan eliyle oynanmadı. Portreler, kapak ve üç CCTV anının kareleri girdi; 22 ekranlık galeri çekimi alındı. Sırada kullanıcının Unity'de baştan sona oynaması var.

**4 Ekim:** Kullanıcı Dosya #001 → #002 → #003'ü ve yeni arayüzü **Unity'de oynadı**; ROADMAP'teki oynanış/arayüz maddeleri `[x]`. Sorgu odasında kişi koltuğa oturuyor, masanın ön kenarı portrenin önünde; bardak ve kayıt cihazı kalktı. Pencereler sönerek kapanıyor. Gerçek kurum adı gömülü `DeskReference.png` silindi — açık içerik ihlali kalmadı. Ekran görüntüsü testleri artık batchmode'da koşuyor: **126 EditMode + 25/25 PlayMode**. `Tools/capture-screens.sh` sorgu ekranlarını toplar. Açık kalanlar: cihaz testi, reklam hesabı (kullanıcıda), yeni vakalar.

**3 Ekim (akşam):** Sorgu odası ve "Kaydı öne sür" maket (`UI_INTERVIEW`) ile kuruldu; denenen kayıt o soruda tekrar listelenmez. Masada yalnız bakılmamış şeyi olan eşya yanıp söner, telefon yeni talep sayısını gösterir, "Yeni tutanak" dokunulur oldu. 128 EditMode + 22/25 PlayMode. Son hali **gözle görülmedi**.

**3 Ekim:** Arayüz ekran ekran yeni tasarım diline taşınıyor ([UI_REDESIGN_PLAN.md](UI_REDESIGN_PLAN.md)). Talepler ekranı kullanıcı tarafından görüldü. Rapor iki iddiaya indi ve yalnız dinlenen kişileri sunuyor; rapor özeti ve "dosya kapandı" kartı maketlere göre kuruldu; dosya artık yalnız onay faksıyla kapanıyor; vaka sonrası her adım bir düğmeyle açılıyor, yeni dosya tek kabulle geliyor; açılış kartı tarih yazıyor. 127 EditMode + 22/25 PlayMode (3 ekran görüntüsü testi grafik aygıtı olmadığı için koşmuyor). Son değişiklikler **gözle görülmedi**.

**2 Ekim (gece, 7):** AdMob bağlandı (Google test kimlikleri, UMP izin formu, banner yok). Cihazda görülmedi; gerçek kimlikler kullanıcının hesabından gelecek.

**2 Ekim (gece, 6):** Reklam yerleri genişletildi: vaka başı ve menüye dönüş araya giren reklamları (4 dakikalık ortak sınır, yalnız ilk vakasını kapatmış oyuncuya), ödüllü lamba rengi, uzun bekleyişte ödüllü atlama (bugün hiç görünmez). Ağ hâlâ yok; gelir için LevelPlay kimliği gerekiyor.

**2 Ekim (gece, 5):** Cila katmanı Y–AF girdi: dokunuş halkası ve basılma derinliği, dosyaya büyüyerek geçiş; lamba, telefon kablosu, şehir ışıkları, takvim; floresan, ayna parlaması, bırakma sesleri; bant takılması, yazıcı, kare tekrarı; bölüm kartı, yankı, jenerik; okurken ses kısılması, kulaklık; renk körlüğü geçişi, sürekli yazı boyu, sistem hareket ayarı, tek elle erişim; efekt kaydı ve kare profili. Görsel/müzik/ses kaydı isteyen kancalar boş. 117 + 23 test geçti; **gözle görülmedi**.

**2 Ekim (gece, 4):** Sahne katmanı P–X girdi (39 madde): efekt laboratuvarı ve konum ayarlayıcı, ekran görüntüsü testleri, kare bütçesi; vaka açılış kartı, görüşmeden çıkış, ışık akışı, dosya rafı; kamera nefesi, kişiye özel bekleme; CCTV hızı ve kare iğneleme; canlı ana menü, kariyer duvarı, rütbe töreni; oda yankısı, müzik katmanı; film yanığı; ses betimlemesi, karşıtlık, tek elle. 117 EditMode + 23 PlayMode geçti; **gözle görülmedi**.

**2 Ekim (gece, 3):** Sahne katmanı H–O girdi (43 madde): eşyadan açılan geçişler, görüşme odası nesneleri, kırmızı ip, Polaroid, büyüteç/çizim, geri sarma, terminal yazımı, ay ışığı, gök gürültüsü, zarf, dosya kapandı kartı, rozet mührü; deneysel CRT, ekran okuyucu köprüsü, yazı boyu, şekil işaretleri. 117 EditMode + 23 PlayMode geçti; **gözle görülmedi**.

**2 Ekim (gece, 2):** Efekt katmanı A–G girdi: film greni, CCTV'de CRT/VHS/kar ve yakınlaştırma, masada vaka saatine göre ışık, yağmur, far, buhar, lamba; evrak yıpranması, faks çıkışı, damga; görüşmede üç nokta, duruş, göz kırpma ve kendi müziği; 17 yeni ses; Ayarlar'da Efektler ve Titreşim. Hiçbir efekt oyun durumunu kodlamaz. 117 EditMode + 23 PlayMode geçti; **gözle görülmedi**.

**2 Ekim (gece):** Ayarlar → Oynanış'a 30 / 60 / 120 fps seçimi eklendi (varsayılan 60; önceden mobilde Unity varsayılanı 30'du). Testler geçti; cihazda ölçülmedi.

**2 Ekim (akşam):** Oyunu derinleştiren yedi iş girdi. Oyuncunun kendi **defteri** (iki kaynağı çelişiyor/örtüşüyor/sorulacak diye işler, oyun hükmü onaylamaz), belgelerde **cümle altı çizme**, faksta **dosyanın akıbeti** (yanlış suçlamanın somut bedeli; üç vakanın bütün seçenekleri), Dosya #002 ve #003 için **ortam sesi**, sicilde akıbet ve **kapanan görüşme** (Dosya #003'te Barış'ın ikinci görüşmesi; doğrulayıcı doğru sonucun yolunu kesmediğini kanıtlıyor). Zaman çizelgesi zaten vardı. Faksın dördüncü sütunu hiç değerlendirmemesi de düzeldi. 117 EditMode + 23 PlayMode geçti; **Unity'de gözle görülmedi**. Vakalar arası geri dönen isim yapısal olarak mümkün olmadığı için kaldırıldı.

**2 Ekim:** Üç vaka artık zincirde: #001 → #002 → #003. **Dosya #003 "Son Teslimat"** (şüpheli ölüm; ölüm kaza, olay yerini değiştiren çalışan ölüme sebep olmadı) veri, metin, portreler ve vakaya özel rapor başlıklarıyla yazıldı; doğrulayıcı vakayı otomatik oynayıp çözüyor. Dosya #002'nin kamera anları (dispute hariç) kare dizisine kavuştu. Vakalar ve Kariyer ekranları referansa göre yeniden kuruldu. 111 EditMode + 23 PlayMode geçti. **Hiçbiri Unity'de gözle görülmedi**; sıradaki darboğaz üç vakanın kullanıcı tarafından baştan sona oynanması. Açık içerik ihlali sürüyor: `DeskReference.png` içinde kurum adı ve arma.

**1 Ekim masa sunumu:** Yeni referansa göre geniş orta pencere, solda lamba ve evrak tepsisi, ortada telefon/beyaz dosya, sağda sade CCTV monitörü ve delil yığını yerleştirildi. Üst şeritte dosya/konum ve altı menü eylemi bulunur; logo ana menüye döndürür. 111 EditMode + 13 PlayMode geçti; grafik etkin Unity çıktısı incelendi. Fiziksel telefon kabulü açık.

**27 Eylül menü işi:** Kullanıcının referansına göre logo, Türkçe slogan, beş menü eylemi ve Bora kartı mevcut video üzerine ayrı, yeniden kullanılabilir UI katmanlarıyla kuruldu. Yeni kariyer ayarlardan erişilir; oyun/vaka akışına dokunulmadı. 111 EditMode ve 11 PlayMode testi geçti; referansa göre farklı yatay ekran oranlarında gözle inceleme açık olduğundan yol haritası işareti `[~]`.

**Dosya #001 baştan sona oynandı** (kullanıcı, 25 Eylül 2026): soruşturma, sorgu, kanıt eşleme ve gerekçeli sonuç gönderme adımları sorunsuz çalıştı ve sesler duyuldu. Oynanıştan dört bulgu çıktı, kapatıldı ve kullanıcı tarafından doğrulandı: masadaki nesneler sessizdi, ana menü müziği hiç duyulmuyordu, masada oda gürültüsü yerine müzik istendi, ayarlar sayfası telefonda sıkışıktı. Kalan boşluk **cihaz doğrulaması**: hiçbir telefonda denenmedi ([PLAYTEST_001.md](PLAYTEST_001.md) §3) ve kullanıcının elinde telefon olmadığı için bu adım **ertelendi** (26 Eylül 2026). Faz 2 açık kalıyor; iş Faz 3'ten devam ediyor: **Dosya #002 "Son Sefer" yazıldı** — veri, metin ve doğrulama tamam, Play Mode'da oynanması açık.

## Dosya #001 hakkında (25 Eylül 2026 kullanıcı kararı)

**Vaka #001'in içeriği şimdilik tamam sayılıyor.** Yeni ifade, kaynak, kanıt veya tur eklenmeyecek; vakanın tasarımı üstünde yeni iş açılmaz. **Oynanış doğrulaması yapıldı:** kullanıcı vakayı 25 Eylül 2026'da baştan kapanışa kadar oynadı ve içerikte sorun bulunmadı ([PLAYTEST_001.md](PLAYTEST_001.md) §2, §4). Geriye cihaz adımı (§3) kaldı; M1 ve M2'nin bitiş ölçütleri buna bağlı. Sıradaki eksen cihaz derlemesi ya da Dosya #002'nin oynanış doğrulaması.

## Vakadan bağımsız temeller (25 Eylül 2026)

Hedef: bundan sonra yalnız vaka eklemek kalsın. Bugün atılanlar — işareti ayrıca yazılmayanlar `[~]`, yani kodlandı ama Play Mode'da görülmedi:

- **Vaka eklemek koddan koptu:** yeni vaka = `caseXXX.json` + `tr.caseXXX.json` + varlıklar. Portre tonları veride, doğrulayıcı vakaya özel C# istemiyor.
- **Ses sistemi kurulu, dokuz klip depoda ve sesler duyuldu.** Klipler sentezlenmiş: `Tools/make-audio.py` üretir, `Tools/check-audio.py` ölçer. Dört arayüz sesi, görüşmede yumuşak bir sohbet blibi (konuşma taklidi yok), ana menü müziği (32 s döngü), masa müziği (`desk_theme`, 40 s — oda gürültüsünün yerini aldı) ve görüşme odası ortamı. Doğrulayıcı dokuzunun varlığını kilitliyor.
- **Ses ayarı beş kademe:** kapalı, %25, %50, %75, tam — ayarlarda radyo listesi (kit'te kaydırıcı yok). Varsayılan: müzik %50, efektler tam.
- **Bölüm seçici ekranı çalışıyor (`[x]`, Play Mode'da görüldü — kullanıcı, 26 Eylül 2026):** Önceki yerleşim 26 Eylül’de gözlendi. 27 Eylül’de sol kimlik/menü, yatay ülke kartları ve polaroid vaka şeridine dönüştürüldü; yeni görsel yerleşim `[~]`. Veri `Bube/Worlds.json`da; kilit ilerlemeden türer, ipucu vermez. Ölçek kanon oldu: **on ülke × yedi dosya = 70** ([WORLD_OPENINGS.md](WORLD_OPENINGS.md), 26 Eylül 2026 kararı). Ülke görselleri tek atlasla, ilk iki vaka kapakları mevcut içerikle karşılanır. Dünya haritası artık bu sayfanın parçası değildir. Kilit/onay işaretleri ortak vektör UI öğesidir.
- **Mobil davranış:** geri tuşu, çıkış onayı, arkaya atılınca kayıt, zincir sonu bildirimi.
- **Reklam dikişi kurulu, ağ yok.** `AdGateway` kuralları testli; LevelPlay/AdMob kurulumu senin hesap kimliklerini bekliyor. Ödüllü ipucu kanonu bozmuyor ve bunu doğrulayıcı kilitliyor.

**Ödüllü yeniden deneme bağlandı:** güven tam o faksın götürdüğü kadar iade ediliyor (gerekirse görevden ayrılma kalkıyor), faks geçmişi başarısızlığı "yeniden açıldı" işaretiyle saklıyor ve ikinci deneme kendi satırını yazıyor. Yeniden açmak ipucu vermiyor: bulunanlar duruyor, yalnız rapor alanları boşalıyor.

## Dosya #002 — Son Sefer (26 Eylül 2026)

Senaryo kullanıcıdan geldi ve eski "Kayıp Yedek" taslağının yerine geçti; taslak tamamen kaldırıldı. Büro **BDS Asayiş Masası**, nitelik yağma/gasp ve yaralama. Vakanın yeni olan yanı mekanik değil **yapı**: tek olay → iki ayrı eylem → iki ayrı sorumluluk. Yaralayan ile parayı alan farklı kişiler, bu yüzden sonuç raporu bu vakada dört sütunlu ([CASE002_DESIGN.md](CASE002_DESIGN.md)). Sütun motorda isteğe bağlıdır; Dosya #001 üç sütunlu kalır.

Durum `[~]`: 13 düğüm, 31 soru, 170 metin anahtarı; doğrulayıcı vakayı baştan sona otomatik oynuyor ve desteklenen raporu gönderiyor. **Play Mode'da oynanmadı.** Dört kişinin portreleri kullanıcıdan geldi ve depoda.

Bölüm geçişi artık sessiz değil: kabul edilmemiş her yeni dosya masaya bırakılıyor (`NewCaseArrival`). Dünyanın kendi varış filmi yalnız o dünyanın ilk dosyasında bir kez oynuyor. İki PlayMode testi var, **Play Mode'da gözle görülmedi** `[~]`.

Rapor gönderildiğinde bırakılışın tersi oynuyor: Bora evrakı kaşeler, dosya ekran dışına gider, sonra vaka özeti açılır (`reportSendVideo`) `[~]`.

## Kimlik

- **Oyun adı:** Karine — 25 Eylül 2026'da karara bağlandı ([NAMING.md](NAMING.md)).
- **Stüdyo:** bubeGames · **Oyun içi kurum:** bube Departman / BDS (değişmedi, kurmaca kurum adıdır).
- **Paket kimliği:** `com.bubedigital.karine` · **Depo:** `github.com/aydinbt/Karine`

## Şu an nerede

| Aşama | Durum |
| --- | --- |
| **Faz 0 — Zemin** | **Bitti ve doğrulandı** |
| **Faz 1 — Doğrulamayı otomatikleştir** | **Bitti** — doğrulayıcı vaka başına ayrıldı, ilk hatada durmuyor, 42 test yeşil |
| **Faz 2 — Gerçekten oyna** | **İlerliyor** — Dosya #001 elle oynandı ve oynanıştan gelen dört düzeltme girildi (113 test yeşil); **cihaz adımı açık** |
| Aşama 1 — Temel yapı | Kod tamam, cihaz doğrulaması açık |
| M1 — Dosya #001 döngüsü | **Baştan sona oynandı**; cihaz adımı açık |
| M2 — Soruşturmayı oyuna çevirme | Oynanarak doğrulandı; cihaz adımı açık |
| M3 — Vaka ekleme & kayıt | case002 "Son Sefer" yazıldı ve taslaklıktan çıktı `[~]`; kayıt göçü kodlandı `[~]` |
| M4 — Mobil kalite kapısı | Derleme ayarları hazır; cihaz testi başlamadı |
| M5 — Görsel ve ses | Ertelendi |
| M6 — Sonraki sistemler | Beklemede |

## Sayılarla

- Kod: ~4.400 satır C#; `BubeApp` konu başına sekiz `partial` dosya, en uzunu 529 satır
- Vaka #001: 9 düğüm, 30 soru — bütünlük denetiminden temiz geçti
- Vaka #002: 7 düğüm, 12 soru — `draft: true`, oyuncuya kapalı
- Türkçe metin: 577 anahtar, eksik 0, yinelenen 0, ölü ~10
- Diller: 1 (tr)
- Assembly: 3 (`Bube.Runtime`, `Bube.Editor`, `Bube.Tests.EditMode`) — hepsi 0 hatayla derleniyor
- Test: **84, hepsi geçiyor** (77 EditMode + 7 PlayMode) — 40 EditMode (8 içerik/metin, 12 Dosya #001 akış, 8 kapı/zaman çizelgesi, 7 doğrulama raporu, 6 kayıt şeması, 1 config) + 5 PlayMode (2 duman + 3 vaka kabul akışı). Tek komut: `Tools/run-tests.sh`
- Doğrulayıcı: 9 dosya `Assets/Bube/Editor/Validation/` altında; `ProjectSetup.cs` 285 → 37 satır

## Faz 0'da yapılanlar (25 Eylül 2026)

- [x] Oyun adı **Karine**; `config.json` `title`, `productName`, `about.body` güncellendi.
- [x] `applicationIdentifier` = `com.bubedigital.karine` (Android / iPhone / Standalone).
- [x] `scriptingBackend` = **IL2CPP**; API seviyesi .NET Standard 2.1. *(ARM64-only Android ile Mono desteklenmiyordu.)*
- [x] `AndroidTargetSdkVersion` = **35** olarak sabitlendi (önceden 0 = Automatic).
- [x] `com.unity.test-framework` 1.4.6 eklendi.
- [x] `Bube.Runtime` / `Bube.Editor` / `Bube.Tests.EditMode` asmdef'leri kuruldu — üçü de sıfır hatayla derlendi.
- [x] Var olan `Bube/Validate Content` doğrulayıcısı EditMode testinden çağrılır hâle geldi.
- [x] Depo git'e alındı, `github.com/aydinbt/Karine`'e bağlandı; `.gitignore` / `.gitattributes` yazıldı, `.mp4` ve `.ttf` **Git LFS**'e alındı.
- [x] Çalışma klasörü `dedektif` → **`karine-mobile`** olarak yeniden adlandırıldı.
- [x] **EditMode testleri Unity Editor'da koşturuldu ve geçti** — `BUBE VALIDATION PASSED`. Tek uyarı: Dosya #002'nin (taslak) `arda` ve `deniz` karakterleri için üretilmiş yedek portre kullanılıyor; bu beklenen davranış.

## Açık kritik maddeler

1. **Terminal ekranındaki arma yaması görünüyor** — armanın yeri tek düz renkle dolduruldu, ekranın gradyanından ayrılıyor ve CCTV kutusu sağa kaymış duruyor. Kullanıcı kararıyla sonraya bırakıldı. Kurum adı ve terminal arması hem masa görselinden hem videodan temizlendi; dosya kapağındaki arma kullanıcı kararıyla kalıyor. Oyun içi kurum kurgusaldır (bube Departman / BDS).
1. ~~**Dosya #001 hiç baştan sona oynanmadı.**~~ **Oynandı (kullanıcı, 25 Eylül 2026), sorun çıkmadı.** **Cihaz doğrulaması ertelendi (26 Eylül 2026):** elde Android telefon yok. M1/M2 bu yüzden kapanmıyor ve masaüstü oynanışı cihaz ölçütünün yerine **geçmez**. Emülatör/Device Simulator seçenekleri açık, kullanıcı şimdilik ertelemeyi seçti.
2. **Android keystore yok** — imzalı *mağaza* sürümü üretilemez. **Düzeltme:** cihaza geliştirme derlemesi kurmak için keystore gerekmiyor (Unity hata ayıklama anahtarıyla imzalar), bu yüzden madde Faz 2'den **Faz 5'e** taşındı; parola kullanıcıya aittir.
3. ~~**Kayıt şeması göçü yok** — `version != 1` olduğunda ilerleme sessizce siliniyor.~~ **Kodlandı, cihazda denenmedi `[~]`** — eski kayıt yükseltilir, gelecekten gelen kayıt silinmeyip yana kaldırılır ve oyuncuya söylenir.
4. ~~**Performans:** `Update()` her karede tam vaka JSON'u ayrıştırıyor; `Locale.Get` doğrusal arama yapıyor.~~ **Kodlandı (Faz 2), Play Mode'da gözlenmedi `[~]`** — beş düzeltme: görev önbelleği, güvenli alan yazımları, rozet yazımları, yüklem temsilcileri, sözlükle indeksli `Locale`.
5. ~~**Unity batchmode lisansı bu makinede çalışmıyor.**~~ **Yanlış teşhisti, düzeltildi.** Testler `Tools/run-tests.sh` ile komut satırından koşuyor (EditMode + PlayMode). Gereken tek şey Unity Hub'ın açık olması. Ayrıntı: [Architecture.md](Architecture.md) → "Testleri başsız koşmak".

## Son oturumda ne değişti (25 Eylül 2026)

Soruşturmanın **dokusu** ve telefonda **erişilebilirlik**. Hepsi `[~]`: kodlandı, 51 test geçiyor, ekranlara bakıldı — oynanarak doğrulanmadı.

- **Yeni kanon kural:** bir kaydı kişiye ancak **adı orada geçiyorsa** öne sürebilirsin. Metinden türer, elle etiketlenmez, yeni vakalarda kendiliğinden işler. `aboutPersonIds` artık yalnız "adını anmadan söz eden" kayıtlar için bir ek.
- **Yem kaynaklar (63) ve davranış satırı (103).** Yanlış kaynak artık gerçek ama yanıltıcı bir yanıt üretir; soru kapanmaz. Davranış satırı gözlem verir, yorum vermez.
- **Mobil sadeleştirme:** dosya ekranında iç içe kaydırma, "1/1" sayacı ve kayan sekme şeridi kalktı; "Dosyada ara" yazı alanı dokunulur tür/kişi süzgecine çevrildi.
- **KARINE UI/UX Kit bağlayıcı tasarım sistemi oldu** — `KarineTheme` + `KarineUI` tek kaynak; palet kit görselinden okundu, ikonlar kit'ten kesildi, ekranlar bileşenlere taşındı, ham renk borcu 156 → 65. Roboto Slab, Inter ve Alfa Slab One depoya kondu, gövde yazısı mono'dan çıktı; sinematiklerde tek denetim GEÇ kaldı (duraklatma ve kare ilerletme CCTV'de duruyor). Kanon: [UI_KIT.md](UI_KIT.md).
- **KARINE logosu oyuna bağlandı** (ana menü + kompakt başlık, tek oran/tek doku). Unity'nin varsayılan içe aktarımı logoyu 2048×512'ye eziyordu; kilitlendi.
- **Doğrulayıcıya sekiz yeni kural** — her biri bu oturumda yaşanan gerçek bir hatadan doğdu.

Ayrıntı ve gerekçeler: [DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md), işaretler: [ROADMAP.md](ROADMAP.md).

## Sıradaki iş

Cihaz adımı ertelendiği için sıra **Faz 3 — Dosya #002**: vaka bugün `draft`, 7 kaynak ve 12 soru ile duruyor (Dosya #001'de 9 kaynak, 30 soru). Yapılacaklar: soruşturma dokusunu #001 seviyesine çıkarmak, `draft` kalkınca #001 → #002 geçişini ve faks zamanlamasını doğrulamak, `CASE_AUTHORING.md` yazmak. Cihaz adımı telefon bulunduğunda [PLAYTEST_001.md](PLAYTEST_001.md) §3 ile koşulur. Betikteki otomatikleşmiş satırlar işaretli; kalanlar gözle doğrulanacak şeyler — video, çentik, dokunma hedefi, glif, klavye, kare hızı, okunabilirlik. Bunlar doldurulunca M1/M2 kapanabilir.

**Vakalar — 27 Eylül:** Referansın sol menü ve fotoğraflı kart hiyerarşisi uygulandı; mevcut masa görseli arka dekor olarak yeniden kullanıldı. Başlıklar, durumlar, kartlar ve gezinme ayrı UI öğeleridir. Vaka verisi, kariyer kaydı ve oyun akışı değiştirilmedi. Görsel kabul ve cihaz kontrolü açık.

**Son test:** 111 EditMode + 12 PlayMode geçti. Yeni test, ülke değişiminde kartların kaydırılabilir kalmasını ve kapalı ülkedeki dosyaların etkileşime açılmamasını denetler; görsel karşılaştırma yapmaz.

**Masa — 27 Eylül:** Kullanıcı referansı doğrultusunda boş oda dekoru + saydam nesne atlası + ülke manzarası + UI bileşenleri kuruldu. Görev/Notlar yok; dosya beyaz ve fotoğrafsız, monitör yalnız CCTV Arşivi yazısı taşır. Mevcut oynanış dalları korunur. Gerçek telefon kabulü açık.

**Masa son doğrulama:** 111 EditMode + 13 PlayMode testi başarılı. Unity render görüntüsünde ayrı oda/nesne/pencere katmanları ve etiket yerleşimi incelendi. Gerçek cihaz kontrolü açık; oyuncunun kaydı test için değiştirilmedi.

## 1 Ekim 2026 — Dosya Vaka Detay

Dosya detay sunumu 1 Ekim 2026 referansına uyarlandı: katmanlı kâğıt, olay fotoğrafı, kompakt metadata, keşfedilmiş kişi kartları, sağ sekmeler ve üst gezinme. Vaka metinleri ve soruşturma koşulları değişmedi. 111 EditMode + 13 PlayMode geçti; fiziksel telefon kontrolü açık.

## 1 Ekim 2026 — Gelen Evraklar sunumu

Gelen Evraklar ekranı son referansa göre sol liste + geniş kâğıt önizleme düzenine taşındı. Mevcut logo, ikonlar, oda ve DossierPaper dokusu yeniden kullanıldı. 111 EditMode + 13 PlayMode geçti. Fiziksel telefon kontrolü açık.

Son doğrulama notu: Unity önizlemesi incelendi; ardından yalnız liste hizası ve yinelenen kurum başlığı düzeltildi. Bu iki sunum düzeltmesi sonrası test betiği lisans oturumu bulunamadığından yeniden çalışmadı. Önceki sürümde 111 EditMode + 13 PlayMode başarılıydı.

## 1 Ekim 2026 — Görüşmeler ve İncelemeler tableti

Görüşmeler/İncelemeler tablet sunumu sol gezinme, orta liste ve sağ detay kartına taşındı. Mevcut tablet ve portreler kullanıldı; yeni bitmap yok. 111 EditMode ve 13 PlayMode geçti; telefon kabulü açık.

## 1 Ekim 2026 — CCTV tablet sunumu

CCTV tablet sunumu erişilebilir kamera listesi + geniş monospace kayıt dökümü düzenine taşındı. Mevcut tablet/ikonlar kullanıldı; yeni asset üretilmedi. 111 EditMode + 13 PlayMode başarılı; fiziksel telefon kontrolü açık.

## 1 Ekim 2026 — Vakalar yeni referansı

Vakalar sayfası son referansa göre koyu ana panel, panoramik ülke başlığı, kompakt dosya kartları, ülke ilerleme çubukları ve genel ilerleme göstergesiyle güncellendi. Mevcut ülke atlası/Bora/kapaklar kullanıldı. 111 EditMode + 13 PlayMode geçti; fiziksel telefon kabulü açık.

Vakalar yerleşimi Unity’de 1280×720 görüntüyle kontrol edildi; iki satırlı vaka başlıkları ve durum etiketleri kart içinde kalıyor. Görsel kayıt: `Docs/Reference/WORLDS_2026-10-01.png`. Son yerleşim sonrası 13 PlayMode testi yeniden geçti.


## 1 Ekim 2026 — Çubuksuz kaydırma

Oyun genelindeki kaydırma çubukları ortak bileşen üzerinden gizlendi; içerik kaydırma korunuyor. Gerçek telefonda parmakla kullanım doğrulaması açık.


## 1 Ekim 2026 — Ayarlar referansı

Ayarlar sol sekmeli koyu panel ve krem tercih kartlarıyla yenilendi. Kaydet/varsayılan taslağı eklendi; gameplay akışı değiştirilmedi. 111 EditMode + 13 PlayMode geçti; fiziksel telefon doğrulaması açık.


## 1 Ekim 2026 — Chakra Petch

Bütün oyun metinleri Chakra Petch Regular/SemiBold/Bold ailesine geçirildi. FontSet ve içerik doğrulayıcı güncellendi; logo bitmap olarak korundu. OFL lisansı fontlarla birlikte eklendi. Fiziksel telefon okunabilirliği kontrolü açık.

Chakra Petch doğrulaması: Regular/SemiBold/Bold dosyalarında Türkçe karakterler ve ₺ mevcut. 111 EditMode + 13 PlayMode başarılı; Unity Ayarlar görünümü incelendi.


## 1 Ekim 2026 — Ortak geçişler

Dosya/tablet/düğme geçişleri ortak sisteme alındı. Hareketi azalt tercihi eklendi. Fiziksel telefon doğrulaması açık.

Geçiş paketi sonrası 14 PlayMode testi geçti. Yeni test: oyun zamanı dururken tamamlanma, ekrandan ayrılınca eski callback iptali ve azaltılmış harekette anında tamamlanma.


## 1 Ekim 2026 — Evrak varışı

Gelen evrak ritüeli eklendi: tepsiye kâğıt hareketi, kısa faks sesi, bildirim ışığı. Tekrar kontrolü oturum içidir; fiziksel cihaz ve duyumsal ses kontrolü açık.

Evrak varışı sonrası 15 PlayMode testi geçti; yeni test kâğıdın gelmesini, tamamlanınca kaldırılmasını ve masaya dönünce tekrarlanmamasını doğruluyor.


## 1 Ekim 2026 — Dosya sekmesi geçişi

Dosya sekmelerinde kısa sayfa geçişi ve aktif sekmenin öne çıkması eklendi; hareket azaltma tercihine uyar. Fiziksel cihaz kontrolü açık.


## 1 Ekim 2026 — Masanın havası (1. kademe)

Masa artık iki katman: oda + pencere arkada, eşyalar ve masa düğmeleri önde. Telefon eğildikçe katmanlar farklı hızda kayıyor; lamba ışığı ve toz ön katmanda, kararma eşyaların üstünde ama düğmelerin altında. Dokunulan eşya kalkıyor. Yeni test `DeskAtmosphereTests` (2): katman sırası ve kalkma/oturma. EditMode 111/111, PlayMode 17/17. Görsel kabul ve cihazda eğme hissi açık.


## 1 Ekim 2026 — Efektler (2. kademe)

Sorguda kayıt kâğıt olarak karşıdakine kayıyor (sürme ya da dokunma), faks ve yeni evrak basılarak çıkıyor, CCTV'de kamera değişince sinyal bozuluyor, dosya sekmesinde sayfa çevriliyor. Yeni test `EffectsTests` (5). EditMode 111/111, PlayMode 22/22. Görsel kabul ve dokunmatik his açık.


## 1 Ekim 2026 — Efektler (3. kademe)

Sorgudaki portre nefes alıyor, yanıttan önce kısa ve sabit bir duraksama var. Göz kırpma yok (kullanıcı kararı). EditMode 111/111, PlayMode 23/23. Görsel kabul açık.


## 8 Ekim 2026 — geri bildirim turu `[~]`

Bayraklı dil listesi, Android yenileme hızı, oyun içi yükleme + terim ipuçları, video/CCTV'de müzik susması, tam sayfa kaydırma, yumuşak masa müziği, daktiloyla basılan sayfalar, telefon sesi, siyahtan açılan ve 3 tur dönen CCTV. Testler geçti (139 EditMode, 27 PlayMode); Unity'de gözlenmedi.
