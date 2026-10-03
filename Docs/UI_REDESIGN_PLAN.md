# Arayüz yenileme planı (3 Ekim 2026)

Karar: oyundaki **bütün** görsel öğeler yeni tasarımla değişir. Masa üstü düğmeler, bildirimler, gezinme çubuğu, çıkış düğmesi ve efektler de buna dahildir; eski görünümde hiçbir parça kalmaz.

Akış her öğe için aynıdır:

1. Ben öğenin neye ihtiyacı olduğunu listeler ve ChatGPT prompt'unu veririm.
2. Kullanıcı maketi gönderir. Maket `Docs/Reference/UI_<AD>_2026-10.png` olarak saklanır.
3. Ben maketi `KarineTheme` ve `KarineUI` ile birebir kurarım. Testler koşulur.
4. Kullanıcı Unity'de görür. Ancak ondan sonra öğe `[x]` olur.

İşaretler:
- `[ ]` maket yok
- `[m]` maket geldi
- `[~]` kuruldu, gözlenmedi
- `[x]` Unity'de gözlendi

## 0. Temel
- [x] Tasarım sistemi panosu (`UI_DESIGN_SYSTEM`)
- [x] Ana menü (`UI_MAIN_MENU`)
- [x] Açılış yükleme ekranı ve stüdyo imzası

## 1. Menü katmanı
- [x] Ayarlar modalı
- [x] Hakkında modalı
- [~] Vakalar / dünya seçici (`UI_CASES`)
- [~] Kariyer panosu (`UI_CAREER`)
- [ ] Kariyer geçmişi ve kayıt sayfası (CareerRecordPage)
- [ ] Arşiv ve arşivdeki vaka (ArchivePage, ArchiveCasePage)
- [ ] Yeniden başlatma onayı (RestartPage)
- [ ] Reklam izni penceresi

## 2. Masa (ana oyun ekranı)
- [x] Üst gezinme çubuğu: dosya başlığı, sekmeler, ayarlar
- [x] Masa sahnesi ve nesne düğmeleri: telefon, dosya, tepsi, lamba, CCTV tableti
- [x] Gelen evrak bildirimi: yanıp sönen rozet ve önizleme
- [x] Çıkış / ana menüye dönüş düğmesi: masadaki Menü düğmesi karşılıyor
- [x] Gelen evrak modalı, teklif hali (`UI_INBOX`); diğer evrak türleri `[~]`
- [ ] Tablet çerçevesi (ertelendi; karar bekliyor: dijital kaynaklar tablette)

## 3. Soruşturma
- [x] Dosya ve kanıt sayfası (FilePage, `UI_FILE*`): sekmeler, liste, rapor kâğıdı gözlendi; sorgu/zaman çizelgesi kâğıtları `[~]`; not defteri sekmesi kaldırıldı
- [ ] Dosyada arama (FileSearchPage)
- [ ] Belge okuma (ReadPage)
- [~] Karşılaştırma (ComparePage, `UI_COMPARE`)
- [ ] CCTV dökümü ve görüntüler
- [ ] Not defteri
- [ ] Soruşturma talepleri

## 4. Sorgu
- [ ] Sorgu odası ve kişi listesi (InterviewPage)
- [ ] Kaydı öne sürme

## 5. Sonuç
- [~] Gerekçeli rapor (`UI_REPORT`): adım sütunu, polaroid şüpheliler, dayanak kaynak, özet, onay kartı; kaynakta oyuncunun kendi karşılaştırma notu görünür
- [ ] Faks / değerlendirme (FaxPage)
- [ ] Yönlendirme ve yeniden deneme teklifleri (GuidancePage)

## 6. Efekt ve geçişler (eskiler kaldırıldı, yenisi tasarlanacak)
- [x] Ekran ve sahne geçişi: düz siyah perde (3 Ekim, gözlendi)
- [ ] Masaya varış: 3 Ekim'de sinematik kaldırıldı
- [ ] Evrak açılışı: basılarak çıkma, oda kararması ve satır satır yazı kaldırıldı
- [ ] Modal açılış / kapanış geçişi
- [ ] Vaka açılış dizisi (bölüm kartı, mekân karesi, başlık)
- [ ] Ortak küçük öğeler: rozet, toast, onay penceresi, boş durum

Sıra yukarıdan aşağıya. Masa en çok görülen ekran olduğu için 2. bölüm önceliklidir.
