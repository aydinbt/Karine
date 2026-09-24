# Karine — faz planı

**Tarih:** 25 Eylül 2026
**Dayanak:** [AUDIT_2026-09-25.md](AUDIT_2026-09-25.md) · **Durum:** [STATUS.md](STATUS.md) · **Ayrıntılı iş listesi:** [ROADMAP.md](ROADMAP.md)

Bu plan `ROADMAP.md`'nin yerine geçmez. ROADMAP **ne yapılacağını** sayar; bu dosya **hangi sırayla ve neden** sorusunu cevaplar. Denetimin ortaya koyduğu temel gerçek şu: proje yazılım olarak yol haritasının gösterdiğinden ileride, altyapı olarak geride. Bu yüzden sıradaki üç faz yeni özellik değil, **var olanı sağlamlaştırma** işidir.

## Durum işareti sözleşmesi

Bundan sonra `ROADMAP.md`'de ve burada:

| İşaret | Anlam |
| --- | --- |
| `[ ]` | Yapılmadı. Kod yok. |
| `[~]` | Kodlandı ve statik/içerik doğrulaması geçti. **Play Mode veya cihazda kanıtlanmadı.** |
| `[x]` | Unity'de veya cihazda davranışı gözlenerek doğrulandı. |

`[~]` hiçbir bitiş ölçütünü karşılamaz. Bir aşama ancak içindeki maddeler `[x]` olduğunda kapanır.

---

## Faz 0 — Zemini sabitle — **BİTTİ (25 Eylül 2026)**

**Neden önce bu:** Bundan sonraki her iş, geri alınabilir ve tekrar çalıştırılabilir bir zemin olmadan risk üretir.

- [x] **İsim kararı: Karine.** `config.json` `title`, `productName`, `about.body` güncellendi. Oyun içi kurmaca kurum adı `bube Polis` / `BPS` bilinçli olarak korundu ([DESIGN_AMENDMENTS.md](DESIGN_AMENDMENTS.md)).
- [x] **Depo izlemeye alındı.** `github.com/aydinbt/Karine`, `main` dalı; `.mp4` ve `.ttf` **Git LFS**'te (7 nesne, 17 MB). Çalışma klasörü `bubeGame/karine-mobile`.
- [x] **Mobil derleme engelleri kapandı.** `applicationIdentifier` = `com.bubedigital.karine`; `scriptingBackend` = IL2CPP; API seviyesi .NET Standard 2.1; `AndroidTargetSdkVersion` = 35.
- [x] **Unity Test Framework** 1.4.6 eklendi.
- [x] **Assembly tanımları:** `Bube.Runtime`, `Bube.Editor`, `Bube.Tests.EditMode` — üçü de sıfır CS hatasıyla derleniyor.
- [x] **EditMode testleri Editor'da koşturuldu ve geçti** (`BUBE VALIDATION PASSED`).

**Bitiş ölçütü karşılandı.** Kalan tek Faz 0 kalıntısı: Android keystore (Faz 2'ye taşındı; keystore desenleri `.gitignore`'da).

---

## Faz 1 — Doğrulamayı otomatikleştir (2–3 oturum)

**Neden:** `ProjectSetup.Validate()` (menü: `Bube/Validate Content`) zaten güçlü bir doğrulayıcı — sarkan referans, eksik metin/görsel, erişilebilirlik ve Dosya #001 rota simülasyonu dahil. Faz 0'da test koşucusuna bağlandı. Bu fazın işi onu **yeniden yazmak değil**, dört boşluğunu kapatmak ve ölçeklenebilir hâle getirmek.

- [ ] **Doğrulayıcıyı vaka başına ayır.** Bugün `data.id=="case001"` blokları genel doğrulayıcının içine gömülü; üçüncü vakada bu dosya okunamaz olur. Genel kurallar ayrı, vakaya özel iddialar ayrı test sınıflarına.
- [ ] **İlk hatada durmayı bırak.** Bugün `throw` ile ilk sorunda kesiliyor; tüm bulgular tek koşuda toplanıp raporlanmalı.
- [ ] **Kapsanmayan dört kontrol:** `tr.json` yinelenen/ölü anahtar raporu; `nextCaseId` zincirinin geçerliliği; her vakada **tam olarak bir** `correct` şüpheli/yöntem/kanıt; kayıt şeması yuvarlak yolculuğu. *(Bu dördü 25 Eylül denetiminde elle kontrol edildi ve temiz çıktı — kalıcılaştırılacak olan o kontrollerdir.)*
- [ ] **Kayıt yuvarlak yolculuğu testi.** `Progress`/`CareerProgress` → JSON → geri; bozuk/eksik/eski kayıtla yükleme.
- [ ] **Soruşturma mantığı testleri.** `Investigation` üzerinde: önkoşul kapıları, `PinTimeline`, soru açılma koşulları, kapanış gereksinimleri. Dosya #001'in bilinen iki alternatif rotası test olarak sabitlenir.
- [ ] Bu maddelerden geçen her ROADMAP satırı `[~]` işaretine çekilir.

**Bitiş ölçütü:** Tek komutla çalışan test paketi yeşil; yeni vaka eklerken bozulan bir şey varsa test söylüyor.

---

## Faz 2 — Gerçekten oyna (2–4 oturum) ← **asıl darboğaz**

**Neden:** Dosya #001 bugüne kadar **bir kez bile** baştan sona oynanmadı. M1 ve M2'nin bitiş ölçütleri tek tek bu adıma bağlı. Bu faz bitene kadar hiçbir yeni özellik başlamamalı.

- [x] **Mobil derleme ayarları** Faz 0'da bitti: `com.bubedigital.karine`, IL2CPP, `AndroidTargetSdkVersion` 35.
  - **Keystore maddesi Faz 5'e taşındı.** Cihaza geliştirme derlemesi kurmak için özel keystore gerekmez — Unity hata ayıklama anahtarıyla imzalar. Kendi keystore'u yalnız mağaza sürümü için gerekir ve parolası kullanıcıya aittir.
- [~] **Perf düzeltmeleri (Play Mode'a girmeden önce)** — kodlandı, üç assembly sıfır hata/uyarı ile derlendi, **Play Mode'da gözlenmedi:**
  - [~] `AvailableAssignment()` sonucu vaka kimliği başına önbelleğe alındı — kare başına tam vaka JSON'u ayrıştırması kalktı.
  - [~] Güvenli alan `style.*` yazımları ekran ölçüsü/güvenli alan/kök öge değişimine bağlandı.
  - [~] Gelen kutusu rozeti yalnız sayı ya da öge değiştiğinde yazılıyor.
  - [~] LINQ yüklemleri (`Pending`, `IncomingDocument`) bir kez kurulan temsilcilere alındı — kare başına `Func` ayırması kalktı.
  - [~] `Locale.Get` `Dictionary` ile indeksli; ilk-kazanır ve `[anahtar]` davranışı beş EditMode testiyle korunuyor.
  - [x] Güvenli alan kenar boşlukları ve `BubeApp` açılışı **PlayMode duman testiyle Unity'de gözlendi**.
- [x] **Otomatik test altyapısı:** `Tools/run-tests.sh` EditMode + PlayMode testlerini komut satırından koşuyor (23 test, hepsi geçiyor). Dosya #001'in kilit zinciri, iki rotası, yanlış kaynak sunma, talep gecikmesi, kayıt gidiş-dönüşü ve rapor değerlendirmesinin üç sonucu otomatik.
- [ ] **Elle uçtan uca oynanış:** betik hazır → [PLAYTEST_001.md](PLAYTEST_001.md). yeni kariyer → dünya açılışı → dosya kabul → üç görüşme → CCTV (metin + 4 klip) → eşya raporu → rapor sihirbazı → kapanış → faks → arşiv. Console'da hata bırakılmaz.
- [ ] **Ters sıra ve çıkmaz avı:** her ekrandan masaya dönüş, yanlış kaynak sunma, yarıda bırakıp çıkma, uygulamayı kapatıp açma.
- [ ] **Gerçek cihaz:** bir Android telefon yeter. 16:9 / 19.5:9 / 20:9, güvenli alan, 48 birim dokunma hedefleri, Türkçe karakterler, klavye, arka plana alma.
- [ ] Doğrulanan her ROADMAP satırı `[~]` → `[x]`.

**Bitiş ölçütü:** Bir telefonda, baştan sona, kayıt kaybetmeden, okunabilir şekilde oynanan bir Dosya #001 kaydı (video veya ekran görüntüsü serisi).

**Kim ne yapıyor:** mantık otomatik testlerle kapatıldı (`Tools/run-tests.sh`, PlayMode dahil). **Kalan iş gözle doğrulama:** video, düzen, çentik, dokunma hedefi, glif, klavye, kare hızı, okunabilirlik ve oyunun bir insan tarafından bir kez baştan sona oynanması. Otomatik test mantığın tutarlı olduğunu söyler, oyunun iyi olduğunu söylemez.

---

## Faz 3 — İkinci vakayı gerçek yap (3–5 oturum)

**Neden:** "Yeni vaka kod değişmeden eklenir" iddiası projenin üretim ekonomisinin tamamı. Bugün **veri düzeyinde** doğru, oyuncu akışında kanıtsız.

- [ ] Dosya #002 "Kayıp Yedek" metni ve soruşturma akışı kullanıcıyla netleştirilir ([CASE002_DESIGN.md](CASE002_DESIGN.md) bugün taslak).
- [ ] Arda / Ece / Deniz için pixel-art portreler.
- [ ] **Kayıt şeması göçü.** Bugün `version != 1` olduğunda kayıt sessizce siliniyor. Sürüm alanı + göç fonksiyonu + bozuk kayıtta oyuncuya görünür bilgi.
- [ ] `draft` kaldırılır, Dosya #001 → #002 geçişi, faks zamanlaması ve arşiv Play Mode + cihazda doğrulanır.
- [ ] **Vaka yazım kılavuzu** (`CASE_AUTHORING.md`): master truth tablosu → kişilerin bildiği/sakladığı → açılma koşulları → yanıt varyantları → kanıt zinciri. Üçüncü vakanın dokümana bakarak yazılabilmesi hedef.

**Bitiş ölçütü:** İkinci vaka tek satır C# değişmeden oynanıyor; eski Dosya #001 kaydı bozulmuyor.

---

## Faz 4 — Mimariyi ölçeklenebilir kıl (sürekli, Faz 2 sonrası)

**Neden:** `BubeApp.cs` 2788 satır. Üçüncü vakadan sonra her yeni ekran bu dosyaya dokunmayı gerektirecek. Büyük patlama refactor'ü değil, **Faz 2'de dokunulan her ekranı çıkarken ayrıştırma** kuralı.

- [ ] `BubeApp` yalnız uygulama kökü + yönlendirici olur.
- [ ] Ekranlar ayrı sınıflara böl: Masa, Dosya, Görüşme, Tablet/CCTV, Gelen Evraklar, Rapor, Kariyer/Arşiv.
- [ ] Kayıt/yükleme ayrı bir `SaveStore` sınıfına; `v1` sabit kodu kaldırılır.
- [ ] Sahne bölünmesi kararı: ya sahneler gerçek içerik taşısın ya da tek sahneye dönülüp durum makinesi olarak adlandırılsın. Bugün üç sahne birebir kopya.

**Bitiş ölçütü:** Yeni bir ekran eklemek `BubeApp.cs`'i büyütmüyor.

---

## Faz 5 — Görsel, ses ve ürün kimliği

Faz 2 ve 3 geçilmeden başlanmaz. [ROADMAP.md](ROADMAP.md) M5 ve M6 içerikleri buraya bağlanır. Ek olarak, bugün hiçbir dosyada takip edilmeyen **yayın işleri**:

- [ ] Mağaza kimliği: ad, ikon, ekran görüntüleri, açıklama metni, yaş sınırı.
- [ ] Gizlilik politikası ve reklam kimliği beyanı (ödüllü reklam ipuçları planlanıyorsa zorunlu).
- [ ] Ödüllü reklam ipucu tasarımı — bugün tek satırlık bir yol haritası maddesi. Kural yazılı: ipucu karşılaştırmaya yönlendirir, faili vermez.
- [ ] İkinci dil kararı ve `Locale` geri düşüş stratejisi. Yedi dünya planı dil planı değildir; karar ayrı verilmeli.

---

## Önerilen çalışma kuralı

Her oturum sonunda: `ROADMAP.md`'de yalnız **gözlenen** davranış `[x]` olur, kodlanan `[~]` olur, `STATUS.md` yenilenir, yeni karar `DESIGN_AMENDMENTS.md`'ye yazılır. Bir aşama "Bitti ölçütü" gerçekleşmeden kapalı ilan edilmez.
