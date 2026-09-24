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

## Faz 0 — Zemini sabitle (1–2 oturum)

**Neden önce bu:** Bundan sonraki her iş, geri alınabilir ve tekrar çalıştırılabilir bir zemin olmadan risk üretir.

- [ ] **İsim kararı.** Mağazada çakışmayan, dedektif çağrışımlı ad. Aday listesi ve eleme ölçütleri: [NAMING.md](NAMING.md). Karar verilince `config.json` `title`, `ProjectSettings` `productName` ve paket kimliği aynı anda ayarlanır.
- [ ] **Depoyu izlemeye al.** `dedektif/` commit edilir, `origin/main`'e push edilir. Video/CCTV binary'leri için Git LFS kararı verilir (16 MB; LFS önerilir). Depo adı isimden bağımsızdır, sonradan değiştirilebilir — isim kararı bunu beklemek zorunda değil.
- [ ] **Unity Test Framework paketi** (`com.unity.test-framework`) eklenir.
- [ ] **Assembly tanımları:** `Bube.Runtime`, `Bube.Editor`, `Bube.Tests.EditMode`. `Investigation.cs` ve `CaseSearch.cs` zaten Unity'den bağımsız — test yüzeyi hazır.

**Bitiş ölçütü:** Temiz bir makinede depo klonlanır, Unity açılır, proje derlenir.

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

- [ ] **Faz 0 mobil derleme ayarları:** `applicationIdentifier` (ör. `com.bubeGames.<ad>`), Android/iOS için **IL2CPP**, `AndroidTargetSdkVersion` sabitlenir (ARM64-only ile Mono desteklenmez), geliştirme keystore'u üretilir.
- [ ] **Perf düzeltmeleri (Play Mode'a girmeden önce):**
  - `AvailableAssignment()` sonucunu önbelleğe al — bugün her karede tam vaka JSON'u ayrıştırılıyor.
  - `Update()` içindeki LINQ `Count` çağrılarını ve koşulsuz `style.*` yazımlarını olaya bağla.
  - `Locale`'i `Dictionary` ile indeksle.
- [ ] **Uçtan uca oynanış:** yeni kariyer → dünya açılışı → dosya kabul → üç görüşme → CCTV (metin + 4 klip) → eşya raporu → rapor sihirbazı → kapanış → faks → arşiv. Console'da hata bırakılmaz.
- [ ] **Ters sıra ve çıkmaz avı:** her ekrandan masaya dönüş, yanlış kaynak sunma, yarıda bırakıp çıkma, uygulamayı kapatıp açma.
- [ ] **Gerçek cihaz:** bir Android telefon yeter. 16:9 / 19.5:9 / 20:9, güvenli alan, 48 birim dokunma hedefleri, Türkçe karakterler, klavye, arka plana alma.
- [ ] Doğrulanan her ROADMAP satırı `[~]` → `[x]`.

**Bitiş ölçütü:** Bir telefonda, baştan sona, kayıt kaybetmeden, okunabilir şekilde oynanan bir Dosya #001 kaydı (video veya ekran görüntüsü serisi).

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
