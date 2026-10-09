# Hesap sistemi ve mağaza şartları — yapılanlar / sana kalanlar

Tarih: 8 Ekim 2026. Kod tarafı `[~]`: derleniyor ve testler geçiyor, ama giriş akışı **cihazda denenmedi**. Cihazda gerçek giriş için aşağıdaki hesap/konsol adımları gerekir; bunları ben yapmadım (hesap açmak ve sözleşme kabul etmek senin işin).

## Kodda hazır olanlar

| Şart | Nerede | Not |
|---|---|---|
| Misafir oynama, giriş zorunlu değil (Apple 5.1.1, Google Play) | `BubeApp.Account.cs` → `AccountPaper` | İlk açılışta bir kez sorulur; "Misafir olarak devam et" her zaman var. |
| Google ile giriş (Android) | `Assets/Bube/Accounts/UgsAccountProvider.cs` | Google Play Games v2 → sunucu yetki kodu → UGS Authentication. |
| Apple ile giriş (iOS) — Apple 4.8 | aynı dosya | iOS'ta Google girişi sunulmuyor, Apple her zaman var. |
| Misafiri sonradan hesaba bağlama | Ayarlar › Hesap | Misafir ilerlemesi hesap klasörüne taşınır; hesapta zaten kayıt varsa o açılır, misafir kaydı silinmez. |
| Hesaba göre kayıt / cihazlar arası devam | `Accounts.cs` + UGS Cloud Save | Hesap başına klasör (`accounts/<oyuncu>/`). Değişiklik 4 sn sonra ya da uygulama arka plana atılınca buluta yazılır. Girişte yeni olan kopya kazanır. |
| Eski kayıtlar kaybolmaz | `Accounts.MigrateLegacy` | Kökteki eski `bube-*.json` dosyaları misafir klasörüne taşınır. |
| Çıkış yapma | Ayarlar › Hesap | |
| Uygulama içinden hesap silme (Apple 5.1.1(v), Google Play hesap silme politikası) | Ayarlar › Hesap › "Hesabı ve verileri sil" | Onay formu → bulut kaydı (`DeleteAllAsync`) + UGS oyuncusu (`DeleteAccountAsync`) + cihazdaki klasör silinir. Misafirde yalnız cihazdaki ilerleme silinir. |
| Gizlilik politikası bağlantısı uygulama içinde | Ayarlar › Hesap, Hakkında, giriş kağıdı | `config.json` → `privacyUrl` boşsa düğme basılmaz. |
| Web'den hesap silme bağlantısı (Google Play şartı) | Ayarlar › Hesap | `config.json` → `accountDeletionUrl`. |
| Destek e-postası | Hakkında › Geri bildirim | `config.json` → `supportEmail`. |
| Ülkeye göre dil | `Languages.Current` | Kayıtlı seçim → cihaz dili → **ülke** → İngilizce. Ülke yalnız cihaz dili desteklenmiyorsa kullanılır. |

Servis kurulamazsa (proje bağlı değil, internet yok) oyun **yerel misafir** olarak çalışır; hiçbir şey kırılmaz.

## Sana kalanlar (sırayla)

### 1. Unity Gaming Services
1. Unity Cloud'da proje oluştur ya da var olanı seç. Editor'de *Edit › Project Settings › Services* ile projeyi bağla. Bu adımla `ProjectSettings` içine proje kimliği yazılır, onu commit'le.
2. Dashboard'da **Authentication › Identity providers** bölümünden şunları ekle:
   - **Google Play Games**: Web client ID ve client secret, 2. adımdaki OAuth web istemcisinden gelir.
   - **Apple**: iOS bundle id `com.bubedigital.karine`.
3. **Cloud Save** servisini etkinleştir. Ücretsiz katman bu oyun için yeterli.
4. Ortam: `production`.

### 2. Google Play Console + Play Games Services
1. Uygulamayı oluştur (`com.bubedigital.karine`).
2. *Play Games Services › Setup*: yeni oyun projesi oluştur. Credentials bölümünde şunlar gerekir:
   - **Android** kimlik bilgisi: paket adı ve SHA-1. SHA-1'i hem yükleme anahtarından hem *App signing* sayfasındaki Google imza anahtarından ekle.
   - **Game server / Web application** OAuth istemcisi. Bunun client ID ve secret değerleri UGS'ye girilir.
3. Unity'de *Window › Google Play Games › Setup › Android setup* adımında kaynak XML'i ve Web client ID'yi yapıştır. Bu adım `GameInfo.cs` dosyasını üretir; onu commit'le.
4. Test kullanıcılarını ekle. Yayından önce Play Games projesi **Publish** edilmelidir.

### 3. Apple Developer / App Store Connect
1. App ID `com.bubedigital.karine` için **Sign in with Apple** yeteneğini aç.
2. Xcode projesinde aynı yetenek gerekir. AppleAuth paketi derleme sonrası bunu eklemeye yardımcı olur; ilk iOS derlemesinde *Signing & Capabilities* bölümünden kontrol et.
3. **Apple token iptali:** Apple, hesap silinince Sign in with Apple belirtecinin `revoke` uç noktasıyla iptal edilmesini bekler. UGS bunu kendisi yapmıyorsa, bunun için küçük bir sunucu işlevi gerekir (UGS Cloud Code ya da başka bir sunucu). İnceleme reddi riski var; ilk gönderimden önce karar verilmeli.

### 4. Web sayfaları (zorunlu)
- **Gizlilik politikası:** herkese açık bir URL. İçinde şunlar yer almalı:
  - toplanan veri: oyuncu kimliği, oyun ilerlemesi, reklam kimliği (AdMob)
  - amaç
  - saklama süresi
  - silme yolu
  - iletişim adresi

  Adresi `config.json` → `privacyUrl` alanına yaz. Aynı adresi her iki mağaza formuna da gir.
- **Hesap silme sayfası** (Google Play şartı): uygulamayı açamayan kullanıcı buradan silme ister. Sayfada uygulama/geliştirici adı, adımlar ve hangi verinin silindiği yazmalı. Adresi `config.json` → `accountDeletionUrl` alanına yaz.
- **Destek e-postası:** `config.json` → `supportEmail`.

### 5. Mağaza formları
- **Google Play › Data safety:**
  - Uygulama etkinliği / oyun ilerlemesi: toplanıyor, hesaba bağlı, silinebilir.
  - Cihaz/diğer kimlikler: AdMob.
  - Veri aktarımda şifreli: evet.
  - Silme talebi: var (URL).
- **App Store › App Privacy:**
  - Identifiers (User ID): App Functionality.
  - Gameplay Content: App Functionality.
  - AdMob için Identifiers/Usage Data: Third-Party Advertising.
  - Reklam kişiselleştirme açıksa iOS'ta ATT izni de gerekir.
- **Yaş derecelendirmesi / içerik anketi:** iki mağazada da doldur. Oyun suç soruşturması içeriyor ama grafik şiddet içermiyor.
- **Reklam:** AdMob kimlikleri hâlâ test kimlikleri (`Assets/Bube/Ads/AdMobProvider.cs`). Gerçek kimliklerle değiştir.

### 6. Cihazda doğrulama (ROADMAP `[x]` için)
- Android'de sırayla:
  1. ilk açılış giriş kağıdı
  2. Google girişi
  3. ilerleme
  4. uygulamayı silip yeniden yükleme
  5. girişten sonra ilerlemenin geri gelmesi
- Misafir → bağla → ilerleme korunuyor mu?
- Hesap silme → yeniden giriş → kayıt boş mu?
- iOS'ta aynı akış Apple ile.

## Görünüş
Giriş kağıdı şimdilik kit bileşenleriyle (onay formu dili) çizildi. Görünüşü ChatGPT tasarımı gelince yalnız `KarineUI.Account.cs` değişir. Marka kuralı: Apple ve Google düğmeleri kendi resmi logo/renk kurallarına uymalıdır. Bugün logo yok, yerine kilit simgesi var. Tasarımda resmi düğme görünüşü kullanılmalı.


## 9 Ekim 2026 eki — mağazaya hazırlık

**Önemli:** Seçilen servis artık Firebase ([FIREBASE_AND_KVKK.md](FIREBASE_AND_KVKK.md)). Yukarıdaki UGS adımları geçersiz. `config.json` içinde Firebase anahtarı varsa oyun UGS'yi kullanmaz.

### Kodda hazır (cihazda denenmedi)

| Konu | Nerede | Not |
|---|---|---|
| Reklamları kaldır (tek seferlik) + Satın alımları geri yükle (Apple 3.1.1) | `Assets/Bube/Purchasing/UnityStore.cs`, Ayarlar › Oyun | Unity IAP 4.12.2. Ürün kimliği `com.bubedigital.karine.noads`, tür *non-consumable*. Google'da makbuz açılışta kendiliğinden geri gelir. |
| iOS takip izni (ATT, Apple 5.1.2) | `Assets/Plugins/iOS/KarinePlatform.mm`, `AdMobProvider` | Önce ATT, sonra Google UMP formu. Info.plist metni `Editor/IosBuildSettings.cs` içinde (İngilizce). |
| Oyun içi puan isteme | `Platform.RequestReview`; Android `KarineReview.androidlib` | 3. dosya kapanınca bir kez. Pencereyi işletim sistemi gösterir ya da göstermez. |
| Çökme raporu | `Assets/Bube/Crash/CrashReporting.cs` | Firebase Crashlytics paketi kurulunca kendiliğinden derlenir. |
| Bozuk kayıttan dönüş | `BubeApp.ReadSave` | Her kayıtta `.bak` tutulur. Bozuk dosya `.corrupt` olarak kenara alınır ve yedek okunur. |
| Gizlilik politikası ve silme sayfası **taslakları** | `Docs/Legal/privacy.html`, `Docs/Legal/delete-account.html` | TR + EN. Köşeli parantezli yerleri doldur, sonra hukukçuya okut. |

### Sana kalanlar
1. **Mağaza ürünleri:** App Store Connect ve Play Console'da iki tek seferlik (non-consumable) ürün oluştur ve fiyatlarını belirle: `com.bubedigital.karine.noads` (Reklamları kaldır) ve `com.bubedigital.karine.priority` (Kalıcı öncelik).
2. **AdMob:** gerçek reklam birimi kimliklerini ver. *Privacy & messaging* bölümünde GDPR mesajını yayınla. Bu bölümdeki "IDFA explainer"ı açma; ATT'yi zaten oyun soruyor, çift pencere çıkar.
3. **Crashlytics:** Firebase projesinde Crashlytics'i aç. Firebase Unity SDK'dan `FirebaseCrashlytics` paketini kur. `google-services.json` ve `GoogleService-Info.plist` dosyalarını ver; bunlar depoya girebilir.
4. **Gizlilik ve silme sayfaları:** `Docs/Legal` altındaki sayfalarda adres, e-posta, tarih ve saklama süresini doldur. Hukukçuya okut, yayınla (GitHub Pages yeter). Adresleri bana ver; `config.json` içindeki `privacyUrl`, `accountDeletionUrl` ve `supportEmail` alanlarına yazarım.
5. **Mağaza formları:**
   - Data safety ve App Privacy: [FIREBASE_AND_KVKK.md](FIREBASE_AND_KVKK.md) dosyasındaki listeye reklam kimliği ve satın alma eklenir.
   - IARC yaş derecelendirmesi.
   - Mağaza görselleri ve tanıtım metinleri.
6. **Yayın imza anahtarı (keystore):** sen üretip saklarsın. Depoya asla girmez.
7. **Cihaz testi turu:** `Docs/PLAYTEST_001.md` dosyasına ek olarak şunlar denenecek:
   - satın al, uygulamayı sil ve yeniden kur, geri yükle;
   - ATT penceresi;
   - Dosya #002'deki hesap bağlama önerisi;
   - 3. dosyadan sonra puan penceresi.
