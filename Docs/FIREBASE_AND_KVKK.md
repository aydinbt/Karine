# Misafir bulut kaydı (Firebase) ve KVKK / mağaza uyumu

Tarih: 9 Ekim 2026. Kod `[~]`: derleniyor ve EditMode testleri geçiyor. Gerçek bir Firebase projesiyle **henüz denenmedi**. Bunu denemek için önce aşağıdaki konsol adımlarının yapılması gerekiyor.

> Bu belge teknik bir uyum listesidir, hukuki görüş değildir. Gizlilik metni yayına girmeden önce bir hukukçuya okutulmalıdır.

## Nasıl çalışıyor

- **İlk açılış:** oyun Firebase'den **anonim** bir kimlik alır. Oyuncuya hiçbir şey sorulmaz; ad, e-posta, telefon ya da reklam kimliği alınmaz.
- **Kimliği hatırlama:** cihazda yalnız yenileme anahtarı (`karine.firebase.refresh`) tutulur. Böylece sonraki açılışlarda aynı oyuncu kimliği döner.
- **Bulut kaydı:** kayıt dosyaları (`bube-*.json`) değişince 4 saniye sonra Firestore'a yazılır. Uygulama arka plana atılınca da hemen yazılır. Yol `users/{uid}/saves/{anahtar}`, belgede tek bir alan var: `entry`.
- **Açılışta senkron:** buluttaki daha yeni kopya cihazdakini ezer. Sonra cihazdaki daha yeni kopyalar buluta gider.
- **İnternet yoksa:** oyun cihazdaki kayıtla açılır, hiçbir şey kırılmaz. Bulut bir sonraki açılışta yeniden denenir.
- **Silme (Ayarlar › Hesap › Hesabı ve verileri sil):** onaydan sonra sırasıyla şunlar silinir:
  1. Buluttaki tüm kayıt belgeleri.
  2. Firebase'deki anonim kimlik.
  3. Cihazdaki klasör.

  Bir adım başarısız olursa hata gösterilir ve cihazdaki kayıt silinmez; oyuncu tekrar deneyebilir. Silmeden sonra oyun açık kalırsa, bir sonraki açılışta yepyeni ve ilişkisiz bir kimlik alınır.
- **Oyuncu kimliği:** Ayarlar › Hesap'ta görünür. Oyuncu web'den ya da e-postayla silme isterse bu kimliği bildirir.
- **Uygulama silinip yeniden kurulursa:** misafir anahtarı ayrıca kalıcı depoda tutulur, böylece aynı profil geri döner.
  - **iOS:** Keychain'de tutulur (`Assets/Plugins/iOS/KarineKeychain.mm`). Uygulama silinse de aynı iPhone'da kalır; iCloud'a gitmez.
  - **Android:** Google Block Store'da tutulur (`Assets/Plugins/Android/KarineBlockStore.androidlib`). Yeniden kurulumda ve aynı Google hesabıyla kurulan yeni telefonda geri gelir.
  - Silme işlemi anahtarı bu depodan da kaldırır.
  - Bu ikisi ancak cihazda denenebilir.
- **Firebase bağlı değilse:** `config.json` içindeki anahtarlar boşken oyun eskisi gibi yalnız cihazda kayıt tutar. Metinler de buna göre değişir ("yalnız bu cihazda").
- **Apple ve Google girişi:** ileride aynı Firebase kimliğine bağlanacak (`signInWithIdp` ile `linkWithIdp`). Böylece misafir ilerlemesi hesaba geçer.

Kod: `Assets/Bube/Runtime/FirebaseAccountProvider.cs` (SDK yok, REST + `UnityWebRequest`). Erişim kuralları: `Firebase/firestore.rules`.

## Sana kalanlar (sırayla)

1. **Firebase projesini oluştur:** [console.firebase.google.com](https://console.firebase.google.com) adresinde, örnek ad `karine`. Hesabı ve sözleşmeyi sen açıp kabul etmelisin.
2. **Anonim girişi aç:** *Authentication › Sign-in method › Anonymous* → Etkinleştir.
3. **Firestore'u kur:** *Firestore Database › Create database* → production modu. Bölge olarak `eur3` ya da `europe-west3` (Frankfurt) seç; Türkiye'ye en yakın seçenek bunlar ve yurt dışı aktarım değerlendirmesi kolaylaşır.
4. **Kuralları yükle:** *Firestore › Rules* sekmesine `Firebase/firestore.rules` dosyasının içeriğini yapıştırıp **Publish** de.
5. **Anahtarları ver:** *Project settings › General*. Bir Web uygulaması ekle; "Web API Key" ve "Project ID" değerlerini bana ver. Ben `Assets/Bube/Resources/Bube/config.json` içindeki `firebaseApiKey` ve `firebaseProjectId` alanlarına yazarım.
   - Firebase web anahtarı gizli bir parola değildir, depoda durabilir. Erişimi kurallar korur.
6. **(Önerilir) API anahtarını kısıtla:** Google Cloud Console › *APIs & Services › Credentials* altında anahtarı yalnız **Identity Toolkit API**, **Token Service API** ve **Cloud Firestore API** ile sınırla.
7. **(Önerilir) Kullanılmayan anonim hesapları temizle:** Identity Platform'a yükseltirsen, otomatik silme ayarı ile uzun süre girilmeyen anonim hesaplar silinir. Bu, KVKK'daki "gereğinden uzun saklamama" ilkesine uyar.
8. **Gizlilik ve silme sayfalarını yayınla:** gizlilik politikası ve web silme sayfası. Bağlantılar `config.json` içindeki `privacyUrl`, `accountDeletionUrl` ve `supportEmail` alanlarına gider. Google Play, web üzerinden silme talebi bağlantısını zorunlu tutuyor.

## KVKK listesi

| İlke / yükümlülük | Durum |
|---|---|
| Veri en aza indirme | Yalnız rastgele oyuncu kimliği ve oyun kaydı. Ad, e-posta ve konum yok. Firebase sunucu tarafında IP adresini işler; bu metinde yazılmalı. |
| Hukuki sebep | Oyun kaydını cihazlar ve kurulumlar arasında korumak, "sözleşmenin ifası" (KVKK m.5/2-c) kapsamında değerlendirilebilir. Bunu hukukçu teyit etmeli. |
| Aydınlatma yükümlülüğü (m.10) | Gizlilik metninde şunlar yazmalı: veri sorumlusu (Bube Games), işlenen veri, amaç, hukuki sebep, aktarılan taraf (Google/Firebase) ve başvuru yolu. **Metin henüz yazılmadı.** |
| Silme hakkı (m.7, m.11) | Uygulama içinden tek adımla siliniyor; web ve e-posta yolu `accountDeletionUrl` ve `supportEmail` ile sağlanacak. |
| Yurt dışına aktarım (m.9, 2024 değişikliği) | Firebase sunucuları yurt dışında. Standart sözleşme imzalanıp 5 iş günü içinde Kurul'a bildirilmesi ya da başka bir uygun güvence gerekebilir. **Hukukçuya sorulmalı.** |
| Saklama süresi | Oyuncu silene kadar. Ek olarak hareketsiz anonim hesapların otomatik silinmesi önerilir (7. adım). |
| VERBİS | Çalışan sayısı ve ciro eşiklerinin altındaysan kayıt yükümlülüğü genelde yok. Teyit et. |

## Mağaza formları

- **Google Play › Data safety:** "Uygulama etkinliği / oyun ilerlemesi" ve "Cihaz ya da diğer kimlikler (uygulamaya özel kimlik)" toplanıyor, aktarımda şifreli ve silinebilir. Paylaşım yok.
- **App Store › App Privacy:** "Identifiers – User ID" ve "Gameplay Content" toplanıyor, uygulama işlevi için kullanılıyor ve takipte kullanılmıyor.
- **Reklam SDK'sı girince** bu iki formun yeniden doldurulması gerekir.

## Ne yazılır, ne zaman — 10.000 oyuncu hesabı (9 Ekim 2026)

- **Buluta giden tek belge kariyer dosyasıdır** (`bube-career-v1`). İçinde şunlar var: kapanan dosyaların sonucu, güven, rütbe, açılan ödüller ve oynama süresi.
- **Buluta gitmeyenler:** vakanın içindeki anlık iz (ne okundu, ne soruldu, not defteri) cihazda kalır. Böylece hem veri en aza iner hem yazma sayısı düşük kalır.
- **Yazma anı:** yalnız dönüm noktalarında belge bir kez yazılır:
  - rapor gönderildi,
  - dosya değerlendirildi,
  - rütbe ya da emeklilik değişti,
  - ödül açıldı.

  Bu da dosya başına yaklaşık 2 yazma eder. Açılışta yalnız cihazdaki kopya buluttakinden yeniyse yazılır.
- **Okuma:** her açılışta 1 okuma.

| | Günlük aktif 10.000 oyuncu | Ücretsiz Spark kotası |
|---|---|---|
| Okuma (günde 3 açılış) | ~30.000 | 50.000 / gün |
| Yazma (günde ~2 dosya → ~4) | ~40.000 | 20.000 / gün |
| Depolama (oyuncu başı ~50 KB'tan az) | ~500 MB | 1 GiB |
| Anonim giriş | ücretsiz | sınırsız |

- **10.000 indirme, 10.000 günlük oyuncu değildir.** Bu tür oyunlarda günlük oyuncu genelde indirmelerin %20–30'u kadardır, yani 2–3 bin kişi. Bu durumda ücretsiz kota yeter.
- **Kota aşılırsa (Spark):** o gün bulut durur, oyun cihazdaki kayıtla devam eder. Hiçbir şey kırılmaz, ertesi gün eşitlenir.
- **Önerilen:** Blaze planına geçip bütçe uyarısı koymak (örneğin 5 $/ay). Kotayı aşan 100.000 yazmanın bedeli yaklaşık 0,18 $; günlük 10.000 aktif oyuncuda bu, ayda 1–2 $ eder.

## Uzaktan ayar — mağaza onayı beklemeden müdahale (9 Ekim 2026)

Kod `Assets/Bube/Runtime/RemoteSettings.cs`. Açılışta herkese açık tek bir belge okunur: **`config/live`**. Ağ yoksa son okunan değerler kullanılır; belge hiç yoksa hiçbir şey kapanmaz. Okunan veri yalnız ayardır, oyuncudan hiçbir şey gönderilmez.

Firestore konsolunda *Start collection* → `config`, belge kimliği `live`. Alanlar hep **string** ve hepsi isteğe bağlı:

| Alan | Örnek | Etkisi |
|---|---|---|
| `minVersion` | `1.0.3` | Daha eski sürüm "Güncelleme gerekli" kâğıdı görür, oyuna girilmez. |
| `pausedCases` | `case002,case014` | Bu vakalar teklif edilmez; açık olan masaya girmez, kayıt korunur. |
| `notice_tr`, `notice_en`, `notice_de`, `notice_fr` | `Dosya #002 düzeltmesi yolda.` | Ana menüde tek satır duyuru. |
| `storeUrl_android`, `storeUrl_ios` | mağaza sayfası | "Mağazaya git" düğmesi. Android boşsa paket kimliğinden üretilir; iOS'ta App Store bağlantısı yazılmalı. |

Kurallar dosyasında (`Firebase/firestore.rules`) `config/live` herkese okunur, kimse yazamaz; yalnız konsoldan değiştirilir. Kuralları yeniden **Publish** etmeyi unutma.

**Ne zaman ne kullanılır:**
- Bir vakada kilit bulundu → `pausedCases` ve duyuru; düzeltme yayına girince ikisini sil.
- Bir sürüm kayıtları bozuyor → düzeltilmiş sürümü yayınla, sonra `minVersion`'ı ona yükselt.
- Kod değişikliği her zaman mağazadan gider; bu belge kodu değiştirmez.

## API anahtarı depoda değil (10 Ekim 2026)

`config.json`'daki `firebaseApiKey` boş kalır. Anahtar `Assets/Bube/Resources/Bube/firebase.local.json` dosyasındadır; dosya `.gitignore`'dadır ve `BubeApp.Awake` onu `config`'in üstüne yazar. Yeni bir makinede bu dosya elle oluşturulur:

```json
{ "firebaseApiKey": "AIza...", "firebaseProjectId": "bubegames-karine" }
```

Dosya yoksa oyun bulutsuz (yalnız cihazda) çalışır. Derlenen oyunun içinde anahtar yine bulunur; bu Firebase'de olağandır, güvenlik `Firebase/firestore.rules` ve Cloud Console'daki API kısıtlamasıyla sağlanır.
