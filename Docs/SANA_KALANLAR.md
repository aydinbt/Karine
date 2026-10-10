# Sana kalanlar — tek liste

Son güncelleme: **10 Ekim 2026**

Bu dosya, **yalnız senin yapabileceğin** işlerin tek listesidir: hesap açmak, sözleşme kabul etmek, para ödemek, cihazda oynamak, karar vermek. Benim yapabileceğim işler burada yer almaz; onlar `ROADMAP.md`'dedir. Her oturum sonunda bu dosya güncellenir. Bir iş bitince satırı silinmez, işaretlenir:

- `[ ]` bekliyor
- `[x]` bitti (tarihiyle)

Ayrıntılı tıklama adımları bağlantılı belgelerdedir.

---

## 1. Hesaplar ve konsollar (yayından önce)

### Firebase — [FIREBASE_AND_KVKK.md](FIREBASE_AND_KVKK.md)
- [ ] Proje oluştur (ör. `karine`).
- [ ] *Authentication › Anonymous* aç.
- [ ] Firestore'u production modunda, `eur3` ya da `europe-west3` bölgesinde kur.
- [ ] `Firebase/firestore.rules` içeriğini yapıştır ve **Publish** et (`config/live` okuma izni dahil).
- [ ] Web uygulaması ekle; **Web API Key** ve **Project ID**'yi bana ver. Ben `config.json`'a yazarım.
- [ ] (Önerilir) API anahtarını üç API ile kısıtla: Identity Toolkit, Token Service, Cloud Firestore.
- [ ] (Önerilir) Blaze planına geç ve 5 $/ay bütçe uyarısı koy.
- [ ] (Önerilir) Hareketsiz anonim hesapların otomatik silinmesini aç (Identity Platform).
- [ ] Firestore'da `config` koleksiyonu altında `live` belgesini oluştur. Şimdilik boş kalabilir.
- [ ] Crashlytics'i aç. `google-services.json` ve `GoogleService-Info.plist` dosyalarını bana ver.

### Google Play Console — [STORE_ACCOUNTS_CHECKLIST.md](STORE_ACCOUNTS_CHECKLIST.md)
- [ ] Geliştirici hesabı (tek seferlik ücret) ve `com.bubedigital.karine` uygulaması.
- [ ] Tek seferlik iki ürün oluştur ve fiyatlarını belirle:
  - `com.bubedigital.karine.noads`
  - `com.bubedigital.karine.priority`
- [ ] Test kullanıcıları ve *Internal testing* kanalı.
- [ ] Google girişi için Play Games Services ve OAuth istemcisi. Bunu Firebase'e bağlamayı ben yazacağım (`ROADMAP`: Apple/Google girişi).

### Apple Developer / App Store Connect
- [ ] Geliştirici hesabı (yıllık ücret) ve `com.bubedigital.karine` App ID'si.
- [ ] *Sign in with Apple* yeteneğini aç.
- [ ] Aynı iki tek seferlik ürünü oluştur.
- [ ] TestFlight.
- [ ] App Store bağlantısını bana ver. Ben `config/live` → `storeUrl_ios` alanının nasıl doldurulacağını gösteririm.
- [ ] Apple girişinde hesap silinince belirteç iptali (revoke) gerekiyor. Sunucu işlevinin nasıl yapılacağına birlikte karar verelim.

### AdMob
- [ ] Hesabı ve uygulamayı oluştur. Gerçek **interstitial** ve **rewarded** birim kimliklerini bana ver; şu an test kimlikleri var.
- [ ] *Privacy & messaging* bölümünde GDPR mesajını yayınla. "IDFA explainer"ı **açma**.

### Yayın imza anahtarı
- [ ] Android keystore'u sen üret ve parolasını sakla. Depoya asla girmez.

---

## 2. Hukuk ve web sayfaları — `bubegames.com/karine/...`

Metinler hazır: [Web/README.md](Web/README.md) (hangi dosya hangi adres). Site Squarespace'te kurulur, sonra taşınır; adresler değişmemeli.

- [ ] Köşeli parantezli yerleri doldur: `[E-POSTA]`, `[ADRES]`, `[TARİH]`, `[N]` (saklama süresi).
- [ ] Destek e-postasını aç (ör. `destek@bubegames.com`).
- [ ] Gizlilik ve koşulları hukukçuya okut: KVKK yurt dışı aktarımı (m.9), hukuki sebep (m.5/2-c), VERBİS, uygulanacak hukuk.
- [ ] Dört sayfayı yayınla: `/karine`, `/karine/gizlilik`, `/karine/kosullar`, `/karine/hesap-silme`.
- [ ] Yayınlanınca haber ver; `privacyUrl`, `accountDeletionUrl`, `supportEmail`'i `config.json`'a ben yazarım.

## 3. Mağaza formları ve görselleri

- [ ] Google Play *Data safety* ve App Store *App Privacy* formları. Liste [FIREBASE_AND_KVKK.md](FIREBASE_AND_KVKK.md) → "Mağaza formları" bölümünde; reklam kimliği ve satın almayı da ekle.
- [ ] IARC yaş derecelendirme anketini iki mağazada doldur.
- [ ] Mağaza ekran görüntüleri, tanıtım metni ve ikon. Metinleri ben yazabilirim; istersen söyle.

---

## 4. Oynayarak doğrulama (Unity Play Mode)

Kodda `[~]` duran ve ancak gözle `[x]` olabilecek maddeler:

- [ ] **Dosya #001'i Fransızca oyna** (Ayarlar → Français): düğme/sekme taşması ve okunuş.

- [ ] **Dosya #002** (9 Ekim değişikliği), şu üçü kontrol edilecek:
  - 23.20–23.30 sinyal boşluğu görünüyor mu,
  - Deniz'in 23.19 sorusu açılıyor mu,
  - Selçuk sorusu kilitlenmeden bitiyor mu.
- [ ] Rapor özetindeki **Çalışman** sayaçları dosyanın tamamını gösteriyor mu.
- [ ] Yem kayıttan sonra soru açık kalıyor ve başka kayıtla devam edilebiliyor mu.
- [ ] Doğru ama dayanaksız rapor "şans tuttu" sayılıyor mu.
- [ ] Jenerik yalnız son dosyadan sonra mı çıkıyor.
- [ ] Dosya **#004–#009** baştan sona.
- [ ] Hesap silinince giriş kâğıdına dönülüyor mu.
- [ ] Ayarlar › Görünüm (kozmetik) ekranı.

---

## 5. Cihaz testi (telefon gerekince)

[PLAYTEST_001.md](PLAYTEST_001.md) §3'e ek olarak şunlar denenecek:

- [ ] Android ve iOS'ta şunlar:
  - dokunma hedefleri, güvenli alan, Türkçe karakterler
  - uygulamayı arka plana atınca ve kapatıp açınca kaydın sürmesi
- [ ] Misafir bulut kaydı akışı (Firebase açıldıktan sonra):
  1. uygulamayı sil
  2. yeniden kur
  3. aynı profil geri geliyor mu (Keychain / Block Store)
- [ ] Hesap silme → yepyeni profil.
- [ ] Satın alma akışı:
  1. satın al
  2. uygulamayı sil, yeniden kur
  3. geri yükle
- [ ] iOS'ta ATT penceresi tek mi çıkıyor, ardından UMP formu.
- [ ] Dosya #002'de hesap bağlama önerisi.
- [ ] 3. dosyadan sonra puan penceresi.
- [ ] Uzaktan ayar denemesi (`config/live`):
  - `minVersion`'ı yüksek yap → güncelleme kâğıdı çıkıyor mu
  - `pausedCases=case002` → "dosya kapalı" kâğıdı çıkıyor mu
  - bir duyuru yaz → menüde görünüyor mu
- [ ] Bellek, ilk açılış süresi, paket boyutu (ölçümü ben yaparım; derlemeyi cihaza sen kurarsın).

---

## 6. Yayın günü — [STORE_ACCOUNTS_CHECKLIST.md](STORE_ACCOUNTS_CHECKLIST.md) → "Sürüm yayını"

- [ ] Sürümü önce Internal testing ve TestFlight'tan geçir.
- [ ] Google'da Staged rollout'u %5 → %20 → %100 aç; Apple'da Phased release'i aç.
- [ ] Çökme raporunu (Crashlytics) izle. Sorun çıkarsa yayını durdur ve `config/live` ile duyuru yap ya da dosyayı durdur.

---

## 7. Karar bekleyenler

- [x] **Fransızca** tamam (10 Ekim 2026).
- [ ] **Çeviri:** tr/en/de/fr tamam. İtalyanca, İspanyolca ve Portekizce (BR) bekliyor. "İtalyancaya başla" dersen başlarım.
- [ ] **Türkiye #001–#010 anlatı denetimi:** öneri kartı açık; tıklarsan ayrı oturumda başlar.
- [ ] **Olay panosu** tasarımı (ROADMAP, Dosya #008): tasarımı sen vereceksin.
- [ ] **Uzaktan indirilen içerik** (yeni vakaları mağaza onayı olmadan göndermek): düzenli vaka yayını planı olursa konuşalım.
