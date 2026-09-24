# Dosya #001 — uçtan uca oynanış betiği (Faz 2)

**Tarih:** 25 Eylül 2026 · **Dayanak:** [PHASE_PLAN.md](PHASE_PLAN.md) Faz 2 · **Sonuç kaydı:** aşağıdaki tablolar doldurulur.

Bu betik Faz 2'nin "Bitiş ölçütü"nü karşılamak için yazıldı: Dosya #001 bir kez bile baştan sona oynanmadı. Betik **ne yapılacağını** söyler, oyunun oyuncuya ne söylediğini değiştirmez — oynanış kuralı gereği oyun sıradaki adımı asla göstermez, bu yüzden kilit zinciri burada geliştirici bilgisi olarak yazılıdır.

## 0. Hazırlık

Play Mode: `MainMenuScene` açılır, Play. Temiz başlangıç için kayıt dosyaları silinir:

```bash
rm -f ~/Library/Application\ Support/bubeGames/Karine/bube-case001-v1.json ~/Library/Application\ Support/bubeGames/Karine/bube-career-v1.json
```

Cihazda temiz başlangıç: uygulama verisi temizlenir veya uygulama kaldırılıp yeniden kurulur.

**Android geliştirme derlemesi imza notu:** cihaza kurmak için ayrı keystore **gerekmez**; Unity geliştirme derlemesini kendi hata ayıklama anahtarıyla imzalar. Kendi keystore'u yalnız mağazaya yüklenecek sürüm için gerekir ve parolası kullanıcıya aittir — bu iş Faz 5'e (yayın) ait, Faz 2'ye değil. Keystore desenleri `.gitignore`'da.

## 1. Kilit zinciri (geliştirici bilgisi)

| Kaynak | Tür | Açılma koşulu |
| --- | --- | --- |
| `report` | belge | başlangıçta açık |
| `mert` | görüşme | `report` okundu |
| `elif` | görüşme | `report` okundu **+** `mert.key` soruldu |
| `hasan` | görüşme | `report` okundu **+** `mert.neighbor` soruldu |
| `camera` | CCTV | `report` okundu **+** `hasan.camera` soruldu |
| `elif_follow` | görüşme | `camera` **+** `elif` okundu |
| `mert_follow` | görüşme | `mert` **+** `camera` okundu |
| `hasan_follow` | görüşme | `hasan` **+** `camera` okundu |
| `recovery` | belge (**talep edilir**, gecikmeli) | `camera` **+** üç takip görüşmesinden **en az biri** |

Rapor kapanışı `conclusionRequires: ["report"]`. Doğru üçlü: fail **hasan**, yöntem **spare**, kanıt **recovery**. Diğer her seçim yanlış değerlendirme üretmelidir (oyun bunu önceden ele vermemeli).

## 2. İleri rota — ana akış

| # | Adım | Beklenen | Sonuç |
| --- | --- | --- | --- |
| 1 | Yeni kariyer | Güven 60, rütbe "investigator" | |
| 2 | Dünya açılışı videosu | Oynar; marka ve yer yazısı 1.8–4.9 sn arasında belirip kaybolur; atlanabilir | |
| 3 | Masaya varış | Gelen kutusu rozeti dosya kabulünü gösterir | |
| 4 | Dosya kabulü → `report` okunur | `mert` erişilebilir olur; oyun yönlendirme yazmaz | |
| 5 | `mert` — 6 soru, `mert.key` ve `mert.neighbor` dahil | `elif` ve `hasan` erişilebilir olur | |
| 6 | `elif` — 5 soru | | |
| 7 | `hasan` — 6 soru, `hasan.camera` dahil | `camera` erişilebilir olur | |
| 8 | `camera` — metin/sinyal dökümü | Dört kayıtlı giriş anında görüntü açılır; **sinyal boşluğunda görüntü yok** | |
| 9 | Üç takip görüşmesi (`elif_follow`, `mert_follow`, `hasan_follow`) | | |
| 10 | `recovery` talep edilir | Gecikme sonunda gelen kutusuna düşer, rozet artar | |
| 11 | Rapor sihirbazı: hasan / spare / recovery | Gönderilir; gizli durum veya doğruluk ipucu **gösterilmez** | |
| 12 | Kapanış → faks | Değerlendirme gecikmeli gelir, güven artar | |
| 13 | Arşiv | Dosya #001 özeti ve zaman çizelgesi okunur | |
| 14 | Sıradaki görev | Dosya #002 **taslak** olduğu için görev **çıkmamalı**; gelen kutusu rozeti şişmemeli | |

Konsolda hata bırakılmaz. Play Mode çıkışında Console temiz olmalı (tek beklenen uyarı: Dosya #002 portre yedeği — o da #002 açılırsa).

## 3. Ters sıra ve çıkmaz avı

| # | Deneme | Beklenen | Sonuç |
| --- | --- | --- | --- |
| 1 | Her ekrandan masaya dönüş | Geri her yerde çalışır, ekran takılı kalmaz | |
| 2 | Görüşmede yanlış kaynak sunma | Reddedilir, ilerleme bozulmaz, ipucu sızmaz | |
| 3 | Görüşmeyi yarıda bırakıp çıkma | Sorulan sorular korunur | |
| 4 | Uygulamayı kapatıp açma (her aşamada) | Kayıt aynı yerden sürer; **sessiz kayıt silinmesi olmamalı** | |
| 5 | `recovery` gelmeden rapor gönderme | Engellenmez ama yanlış değerlendirme üretir | |
| 6 | Arama alanında var olmayan kaynak | Sızıntı yok | |
| 7 | Rapor sihirbazını yarıda bırakma | Seçimler kaybolsa bile ekran kilitlenmemeli | |

## 4. Gerçek cihaz (bir Android telefon yeter)

| Kontrol | Beklenen | Sonuç |
| --- | --- | --- |
| 16:9 / 19.5:9 / 20:9 | Düzen kırılmaz | |
| Güvenli alan (çentik, alt çubuk) | İçerik kesilmez | |
| Dokunma hedefleri | En az 48 birim | |
| Türkçe karakterler (ı, İ, ğ, ş, ç, ö, ü) | Fontta eksik glif yok | |
| Klavye (arama alanı) | Alanı kapatmaz | |
| Arka plana alıp dönme | Kayıt ve video durumu bozulmaz | |
| Kare hızı | Masa ve görüşme ekranlarında akıcı | |

## 5. Faz 2 performans düzeltmeleri — doğrulama

Aşağıdakiler kodlandı ve derleme doğrulandı; **Play Mode'da gözlenmesi** bekleniyor.

| Düzeltme | Nerede | Play Mode'da ne görülmeli | Sonuç |
| --- | --- | --- | --- |
| `AvailableAssignment()` önbelleği | [BubeApp.cs](../Assets/Bube/Runtime/BubeApp.cs) | Dosya kapandıktan sonra Profiler'da kare başına `JsonUtility.FromJson` yok | |
| Güvenli alan yazımları olaya bağlandı | aynı | Ekran döndürülmedikçe kare başına düzen yazımı yok | |
| Gelen kutusu rozeti yalnız değişimde yazılıyor | aynı | Rozet hâlâ doğru sayıyı gösteriyor (talep gelince artıyor) | |
| Yüklem temsilcileri önbelleklendi | aynı | Profiler'da kare başına GC ayırması düşük | |
| `Locale.Get` sözlükle indeksli | [Investigation.cs](../Assets/Bube/Runtime/Investigation.cs) | Tüm metinler doğru çıkıyor, `[anahtar]` görünmüyor | |

Bu satırlar gözlendikçe [ROADMAP.md](ROADMAP.md) karşılıkları `[~]` → `[x]` yapılır.
