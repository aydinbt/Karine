# bube — MASTER GAME CONTEXT

> Bu doküman projenin mevcut tasarım kararlarının ana kaynağıdır. Buradaki kararları proje boyunca koru. Kullanıcı açıkça değiştirmedikçe eski/generic önerilerle üzerine yazma.

## 1. PROJE

**Working Title:** bube  
**Developer / Studio:** bubeGames  
**Engine:** Unity  
**Platform önceliği:** Mobil  
**Ekran yönü:** Landscape  
**Tür:** Dedektiflik / soruşturma / belge inceleme  
**Görsel yön:** Minimalist pixel-art, 2D/2.5D  
**Temel ilham:** Papers, Please'in asset ve etkileşim felsefesi. Birebir kopyalanmayacak.

Final oyun adı henüz belirlenmedi.

Kod ve proje yapısında `bube` ismi hard-code edilmemeli. Oyun adı merkezi config/localization üzerinden değiştirilebilmeli.

Oyuncuya görünen ilk dil Türkçe olacak. Kod anahtarları İngilizce olabilir ancak kullanıcıya görünen metinler localization sistemi üzerinden gelmeli.

---

# 2. ANA OYUN FELSEFESİ

Oyuncu bir Criminal Investigator / kriminal soruşturmacıdır.

Temel fikir:

**Oyuncuya hikâyeyi yazdırmıyoruz; soruşturmayı yorumlatıyoruz.**

Hikâyenin gerçeği geliştirici tarafından belirlenir.

Oyuncu:
- delilleri inceler,
- raporları okur,
- insanlarla görüşür,
- ifadeleri karşılaştırır,
- CCTV kayıtlarını inceler,
- zaman çizelgelerini değerlendirir,
- çelişkileri kendisi fark eder,
- sonunda kendi sonucunu oluşturur.

Oyuncunun yaptığı role-play seçimleri protagonistin kişiliğini veya hikâyenin gerçeğini değiştirmez.

Gerçek fail geliştirici tarafından belirlenmiştir.

Oyuncunun görevi gerçeği ortaya çıkarmaktır.

---

# 3. EN ÖNEMLİ TASARIM KURALLARI

## Oyun oyuncunun yerine düşünmez

Asla doğrudan:

- “ÇELİŞKİ TESPİT EDİLDİ”
- “BU KİŞİ YALAN SÖYLÜYOR”
- “HASAN ŞÜPHELİ”
- “ŞİMDİ CCTV'YE BAK”
- “ELİF'LE TEKRAR KONUŞ”

gibi yönlendirmeler yapılmaz.

Oyuncu bilgileri kendisi bağlar.

## Yalan söylemek suçlu olmak değildir

Bir NPC başka bir nedenle yalan söyleyebilir.

Oyuncunun öğrenmesi gereken önemli prensip:

**Yalan ≠ suçluluk.**

## Uzmanlar sonucu verir, yorumu oyuncu yapar

Forensics, CCTV, teknik birimler vb. sonuçlarını raporlar.

Hiçbir sistem:

> “Bu nedenle suçlu X'tir.”

demez.

## Bilgi her zaman tekrar okunabilir

Oyuncudan konuşmaları ezberlemesi beklenmez.

İfadeler, raporlar, deliller, saatler ve diğer bilgiler dosyada tekrar okunabilir.

---

# 4. ANA KARAKTER — BORA

Ana oynanabilir karakter sabittir.

**Ad:** Bora  
**Soyad:** Henüz belirlenmedi  
**Yaş:** 27  
**Cinsiyet:** Erkek  
**Milliyet:** Türk  
**Memleket:** İstanbul  
**Büyüdüğü yer:** Beşiktaş

Bora birkaç yıllık polislik deneyiminin ardından bube Police Criminal Investigation birimine geçmiştir.

Oyun, soruşturmacılık kariyerinin yeni döneminin başlangıcında başlar.

## Kişilik

Bora:

- sakin,
- gözlemci,
- ayrıntılara dikkat eden,
- gereksiz agresif olmayan,
- insanların söylediklerini otomatik olarak doğru kabul etmeyen,
- kayıt ve kanıtlarla değerlendirme yapan

bir karakterdir.

Kişiliği oyuncunun seçimlerine göre dallanmaz.

Bora birine:

> “Yalan söylüyorsun!”

demek yerine daha kontrollü konuşur.

Örneğin:

> “Saat 11.00'de evde olduğunuzu söylediniz.”

ve ardından:

> “Binanın giriş kaydı sizi 11.48'de gösteriyor.”

## Bora'nın biyografisi

Oyunda kısa bir personel profili/biyografi bulunacak.

Oyuncu oynadığı kişinin kim olduğunu bilecek.

Ancak A'dan Z'ye gereksiz lore verilmeyecek.

Oyuncunun bilmesi gereken temel bilgiler:
- Bora'nın yaşı
- İstanbul/Beşiktaş geçmişi
- polislik deneyimi
- soruşturma birimine yeni geçmesi
- temel kişiliği

Hikâyeye hizmet eden başka kişisel bilgiler gerektiğinde oyun içerisinde doğal biçimde ortaya çıkabilir.

---

# 5. KURGUSAL POLİS KURUMU

Gerçek polis teşkilatları, resmi armalar veya gerçek kurum logoları kullanılmayacak.

Oyun evrenindeki kurum:

# bube Police

Türkiye'deki yerel kullanım örneği:

**bube Polis — İstanbul**  
**Beşiktaş Şubesi / Soruşturma Birimi**

Farklı ülkelerde aynı bube kurumsal kimliğinin yerel varyasyonları kullanılabilir.

Örneğin:

**bube Police — London / Camden Division**

Ana `bube` markası sabit kalır.

Şehir/şube/birim isimleri değişebilir.

## Logo

Gerçek polis yıldızı, devlet arması, kartal vb. taklit edilmeyecek.

Özgün, minimalist, pixel-art uyumlu bir **B amblemi** kullanılabilir.

---

# 6. BPS

Polis içerisindeki dijital sistemlerin ortak markası:

**BPS — bube Police System**

Örneğin CCTV:

**BPS // GÜVENLİK KAMERASI ARŞİVİ**

Bu tasarım dili farklı sistemlerde tekrar kullanılabilir.

---

# 7. GERÇEK MARKALAR

Oyunda mümkün olduğunca gerçek şirket/logo/ürün markaları kullanılmayacak.

Örneğin:

WhatsApp → generic/kurgusal mesajlaşma sistemi  
Instagram → kurgusal sosyal ağ  
Google → kurum içi arama/database  
iPhone → generic smartphone  
BMW/Mercedes → generic/kurgusal araç

Amaç oyunun kendi dünyasını oluşturmak.

---

# 8. ANA EKRAN — BORA'NIN MASASI

Ana ekran generic bir Case Management uygulaması DEĞİLDİR.

Oyuncu yukarıdan baktığı fiziksel bir soruşturmacı masası görür.

Minimalist pixel-art.

Temel kural:

# Masada görünen şey = kullanılabilen şey.

Dekoratif ama kullanılamayan gereksiz objeler konulmamalı.

İlk aşamada masada:

### DOSYA
Vakanın bütün belgeleri burada.

### GELEN EVRAKLAR
Yeni raporlar fiziksel olarak buraya gelir.

Küçük bildirim işareti olabilir.

### TERMİNAL
Bora'nın erişimi bulunan dijital sistemler.

İlk vakada temel olarak CCTV.

İleride yetkiler arttıkça yeni terminal fonksiyonları açılabilir.

Yeni mekanik için masaya sürekli yeni obje eklemek yerine terminal genişleyebilir.

### TELEFON
Yalnızca gerçekten kullanılan görüşme/arama mekaniği varsa masada bulunmalı.

---

# 9. MASA UX

Temel akış:

**Masa → Objeye dokun → Focus Mode → İşlemi yap → Kapat → Masaya dön**

Bir belge açıldığında ekranın yaklaşık %80–90'ını kaplar.

Arkadaki masa blur/dim olur.

Mobil okunabilirlik önceliklidir.

Dosya generic uygulama menüsü gibi değil, fiziksel belge/dosya hissinde olmalıdır.

---

# 10. DOSYA

Dosyanın içerisinde fiziksel sayfa mantığı kullanılabilir.

Örnek:

- Olay Raporu
- Mert Görüşmesi
- Elif Görüşmesi
- Hasan Görüşmesi
- CCTV inceleme çıktısı
- Eşya Tespit Raporu
- Sonuç Raporu

Yeni belgeler soruşturma ilerledikçe fiziksel olarak dosyaya eklenir.

Swipe veya oklarla sayfalar arasında geçilebilir.

---

# 11. GÖRÜŞME SİSTEMİ

Karakterler minimalist pixel-art sprite olacaktır.

AI illustration / photorealistic portrait görünümü kesinlikle kullanılmamalıdır.

Karakterler gerçek oyun asset'i gibi görünmelidir.

## Sprite yaklaşımı

Karakter başına gerektiğinde:

- neutral
- thinking
- uneasy
- defensive
- angry/upset

gibi az sayıda state olabilir.

Ancak oyuncu:

- TEDİRGİN
- SAVUNMACI
- YALAN SÖYLÜYOR

gibi state isimlerini ASLA görmez.

Oyuncu karakterin davranışından/mimiklerinden kendi yorumunu yapar.

Background NPC: 1 state olabilir.  
Tanık: 2–3 state.  
Önemli şüpheli: 4–5 state.

Lip-sync gerekli değildir.

---

# 12. NPC STATE SİSTEMİ

Arka planda NPC durumları tutulabilir:

- NOT_CONTACTED
- INTERVIEWED
- NO_NEW_INFORMATION
- NEW_INFORMATION_AVAILABLE
- FOLLOW_UP_COMPLETED

Oyuncu bunların teknik isimlerini görmez.

Bir NPC ile tekrar konuşulduğunda yeni bilgi yoksa doğal cevap:

> “Size bildiğim her şeyi anlattım.”

Yeni bir delil ortaya çıkarsa aynı NPC için yeni sorular açılabilir.

---

# 13. INVESTIGATION PHASE / STATE GRAPH

Soruşturma tamamen serbest sandbox olmayacak.

Hikâyenin devam edebilmesi için görünmeyen bir state graph bulunacak.

Belirli checkpoint'ler kontrollüdür.

Checkpoint içerisindeki bazı işlemlerin sırası serbest olabilir.

Ama oyuncu hikâyeyi bozabilecek bir noktaya gelemez.

Yeni bilgi:
→ yeni kişi
→ yeni rapor
→ yeni soru
→ yeni sistem erişimi

gibi doğal biçimde yeni içerik açar.

Checklist kullanılmaz.

---

# 14. CCTV — ÇOK ÖNEMLİ

CCTV sistemi hakkında kesin karar:

# CCTV'DE VİDEO YOK.

# CCTV'DE FOTOĞRAF / STILL IMAGE YOK.

# CCTV'DE KİŞİ GÖRSELİ YOK.

Tamamen text-based CCTV terminalidir.

Ama sistem gerçek bir güvenlik sistemi gibi hissettirmelidir.

Örnek görsel öğeler:

- REC
- Kamera numarası
- Konum
- Tarih
- Saat
- Saniye
- Signal graph
- Signal quality
- Scanline
- Hafif interference
- Terminal animasyonları
- Yazma sesi
- Elektronik sesler

Oyuncu **KAYITLARI İNCELE** komutunu çalıştırır.

Sistem tarar.

Olaylar tek tek terminale yazılır.

Örnek:

KAMERA 01 — BİNA GİRİŞİ

İNCELENEN ARALIK  
08.00 — 18.00

08.27 — Erkek şahıs binadan ayrıldı.

11.48 — Kadın şahıs binaya girdi.

12.16 — Aynı şahıs binadan ayrıldı.

İNCELEME TAMAMLANDI

---

# 15. CCTV SIGNAL QUALITY

Signal quality gameplay üzerinde etkilidir.

## GOOD
Bilgi büyük ölçüde temiz.

## MODERATE
Bazı karakterler/saatler belirsiz olabilir.

## POOR
UNKNOWN, eksik saat veya eksik tanımlama olabilir.

## SIGNAL LOST

Örnek:

12.37 — SİNYAL KESİLDİ  
[ KAYIT BULUNAMADI ]

13.08 — SİNYAL GERİ GELDİ

Glitch rastgele yanlış bilgi üretmez.

Gerçek bir bilgiyi başka bir bilgiye dönüştürmemeli.

Belirsizlik case data içerisinde geliştirici tarafından yazılır.

UI sadece bunu görselleştirir.

---

# 16. CCTV DATA-DRIVEN OLMALI

Her vaka için yeni CCTV sistemi yazılmamalı.

Örneğin veri:

cameraName  
location  
date  
reviewPeriod  
signalQuality  
events  
signalLostPeriods

üzerinden aynı component farklı vakalarda çalışabilmeli.

---

# 17. KARİYER

Ülkeler veya şehirler progression tier değildir.

Oyuncu ilk şehirdeyken bile kariyerinde yükselebilir.

Kariyer progression yeni yetkiler/sistemler açabilir.

Örnek:

Junior / Probationary Investigator  
Criminal Investigator  
Senior Investigator  
Lead Investigator

İsimler daha sonra değiştirilebilir.

Rank kalıcı olabilir.

Department Confidence / Reputation değişken olabilir.

Kötü kararlar oyuncuyu kalıcı olarak kilitlememelidir.

---

# 18. DOSYA SONUCU

Bir vakada yalnızca:

> Suçlu kim?

sorusu olmak zorunda değildir.

İleride sonuç formunda:

- KİM?
- MOTİF?
- YÖNTEM?
- TEMEL DELİL?

gibi seçimler olabilir.

Yanlış sonuç verilebilir.

Yanlış sonuç hikâyeyi sonsuza kadar bozmaz.

Dosya geri dönebilir.

Bazı vakalar UNSOLVED olarak kalabilir.

Daha sonra yeni delille REOPENED olabilir.

---

# 19. MONETIZATION

Ana gelir modeli:

**Rewarded Ads + kontrollü Interstitial**

Banner tercih edilmiyor.

App-open reklam başlangıçta düşünülmüyor.

Reklam:

- görüşmenin ortasında
- CCTV incelemesinin ortasında
- rapor okunurken
- kritik sahnede

çıkmamalı.

Interstitial için doğal yer:

**Dosya tamamlandı → ofise/masaya dönüş**

Rewarded Ads özellikle ipucu sistemi için kullanılabilir.

Ama ipucu suçluyu söylemez.

Örneğin:

> “21.00–23.00 arasında verilen ifadeleri karşılaştır.”

veya:

> “Hasan'ın ifadesini Kamera 01 kayıtlarıyla karşılaştır.”

Oyuncuya yön verir, cevabı vermez.

---

# 20. RETENTION

Uzun vadede düşünülen sistemler:

### Daily Incident
5–10 dakikalık düşük maliyetli mini vaka.

### Weekly Case
Toplulukla aynı vaka.

### Archive
Eski vakalar saklanır.

Bazıları yeniden açılabilir.

### Career Stats
- Atanan Dosyalar
- Çözülen Dosyalar
- Çözülemeyenler
- Yeniden Açılanlar
- Doğru Sonuçlar
- Desteksiz Suçlamalar
- Yardım Kullanımı
- Department Confidence
- Current Division

Bunlar ilk vertical slice'ın scope'unda değildir.

---

# 21. DOSYA #001 — AÇIK KAPI

İlk oynanabilir vaka.

**Yer:** Beşiktaş / İstanbul  
**Tür:** Konut Hırsızlığı  
**Soruşturmacı:** Bora  
**Tahmini süre:** 10–15 dakika

Karakterler:

### Mert Aydın
Ev sahibi / ihbar sahibi.

### Elif Demir
Mert'in kısa süre önce ayrıldığı eski partneri.

### Hasan Kaya
Mert'in komşusu.

Gerçek fail:

# Hasan Kaya

---

# 22. DOSYA #001 MASTER TRUTH

Oyuncuya gösterilmeyecek gerçek olay:

08.27 — Mert binadan ayrılır.

11.48 — Elif binaya girer.

12.16 — Elif çıkar.

Elif, Mert'in evindeki kalan eşyalarını almak için habersiz gelmiştir.

Çıkarken Mert'in yedek anahtarını kapının yanındaki saksıya bırakır.

Hasan daha önce Mert kapıda kaldığında ona yardım ettiği için anahtarın burada tutulabildiğini bilmektedir.

Elif'in çıktığını görür.

Yaklaşık 12.40 — Hasan anahtarı alır ve eve girer.

Yaklaşık 12.50 — Laptop, kol saati ve nakit para çalınır.

Yaklaşık 13.00 — Hasan çıkar ve anahtarı yanında götürür.

17.54 — Mert eve döner.

Yaklaşık 18.00 — Hırsızlık fark edilir.

---

# 23. DOSYA #001 BAŞLANGIÇ RAPORU

**OLAY RAPORU — DOSYA #001**

Olay: Konut Hırsızlığı  
Konum: Beşiktaş / İstanbul  
İhbar Sahibi: Mert Aydın

Konutta yapılan ilk incelemede zorla giriş izine rastlanmamıştır.

Kayıp olduğu bildirilen:

- 1 dizüstü bilgisayar
- 1 kol saati
- nakit para

Konut girişinin yanında küçük bir saksı tespit edilmiştir.

İçerisinde anahtar bulunmamıştır.

---

# 24. MERT — İLK GÖRÜŞME

Mert:

> “Sabah sekiz buçuk gibi çıktım.”

> “Akşam altıya doğru döndüm.”

> “Çıkarken kapıyı kilitlediğimden eminim.”

> “Elif'te bir anahtar vardı.”

> “Hasan karşı dairede oturuyor. Belki bir şey görmüştür.”

Bu görüşmeden sonra Elif ve Hasan doğal olarak erişilebilir olur.

Oyuncuya görev/checklist verilmez.

---

# 25. ELİF — İLK GÖRÜŞME

Bora:

> “Bugün Mert'in evine gittiniz mi?”

Elif:

> “Hayır.”

Bu ifade yanlıştır.

Ama Elif hırsız değildir.

---

# 26. HASAN — İLK GÖRÜŞME

Hasan:

> “Öğlen bir kadın gördüm. Mert'in yanında daha önce gördüğüm birine benziyordu.”

> “Binanın girişinde kamera var. Belki kayıtlarda vardır.”

Bu bilgi CCTV arşivini erişilebilir hâle getirir.

---

# 27. DOSYA #001 CCTV

BPS // GÜVENLİK KAMERASI ARŞİVİ

KAMERA 01 — BİNA GİRİŞİ

İNCELENEN ARALIK  
08.00 — 18.00

08.27 — Erkek şahıs binadan ayrıldı.

11.48 — Kadın şahıs binaya girdi.

12.16 — Aynı şahıs binadan ayrıldı.

12.31 — SİNYAL ZAYIFLADI

12.37 — SİNYAL KESİLDİ  
[ KAYIT BULUNAMADI ]

13.08 — SİNYAL GERİ GELDİ

17.54 — Erkek şahıs binaya girdi.

İNCELEME TAMAMLANDI

Hasan'ın hırsızlığı gerçekleştirdiği zaman kör noktadadır.

CCTV failin kim olduğunu doğrudan söylemez.

---

# 28. ELİF — İKİNCİ GÖRÜŞME

CCTV incelendikten sonra yeni soru açılır.

Bora:

> “Bugün binaya hiç gitmediğinizi söylediniz.”

Elif sonunda kabul eder:

> “Tamam. Gittim.”

> “Mert'le birkaç gün önce ayrıldık. Evde kalan birkaç eşyam vardı.”

> “Onunla tekrar konuşmak istemedim.”

> “Eşyalarımı aldım ve çıktım.”

> “Anahtarı da kapının yanındaki saksıya bıraktım.”

Bora:

> “Neden bunu ilk görüşmede söylemediniz?”

Elif:

> “Çünkü evine habersiz girdim. Sonra eşyalarının çalındığını öğrendim. Nasıl görüneceğini tahmin edebiliyorsunuz.”

Bu vaka oyuncuya:

**Yalan ≠ suçluluk**

prensibini öğretir.

---

# 29. MERT — İKİNCİ GÖRÜŞME

Yeni soru:

> “Yedek anahtarı daha önce saksıda tutuyor muydunuz?”

Mert:

> “Bazen. Geçen ay kapıda kalmıştım.”

> “Hasan yardım etmişti. Anahtarı oradan aldığımı gördü.”

Hasan'ın anahtarın yerini bildiği öğrenilir.

---

# 30. HASAN — İKİNCİ GÖRÜŞME

Bora:

> “Mert'in yedek anahtarını nerede tuttuğunu biliyor muydunuz?”

Hasan:

> “Bir kere görmüş olabilirim. Hatırlamıyorum.”

Bu şüphe yaratır ancak suçluluk kanıtı değildir.

---

# 31. SON DELİL

Masaya yeni rapor gelir:

# EŞYA TESPİT RAPORU

Kayıp olarak bildirilen dizüstü bilgisayarın seri numarasıyla eşleşen cihaz tespit edilmiştir.

Cihaz aynı gün Beşiktaş'taki bir ikinci el elektronik işletmesine bırakılmıştır.

İşletme kayıtlarında cihazı teslim eden kişi:

**HASAN KAYA**

Cihaz muhafaza altına alınmıştır.

Oyun:

> “HASAN SUÇLU!”

demez.

Oyuncu bağlantıyı kendisi kurar.

---

# 32. DOSYA #001 SONUÇ RAPORU

Oyuncu örneğin:

### Şüpheli
Mert Aydın  
Elif Demir  
Hasan Kaya

### Giriş yöntemi
Zorla giriş  
Yedek anahtar  
Açık kapı  
Bilinmiyor

### Temel delil
Mevcut belgeler arasından seçim

Doğru sonuç:

**Hasan Kaya**  
**Yedek anahtar**  
**Eşya Tespit Raporu**

Doğruysa:

DOSYA #001

SONUÇ ONAYLANDI

Şüpheli: Hasan Kaya  
Durum: ÇÖZÜLDÜ

DOSYA KAPATILDI

Yanlışsa ilk vakada ağır ceza verilmez.

Örneğin:

**SONUÇ İADE EDİLDİ**

> Sunulan deliller sonucu desteklemek için yeterli değil.

Dosya tekrar masaya gelir.

---

# 33. DOSYA #001'DE OLMAYACAK SİSTEMLER

İlk vakaya scope creep yapılmamalı.

YOK:

- telefon extraction
- banka kayıtları
- DNA
- fingerprint lab
- otopsi
- gelişmiş forensic
- araç sorgulama
- sosyal medya sistemi
- 3D crime scene
- CCTV videosu
- CCTV fotoğrafı
- gelişmiş interrogation mechanic
- reopened case
- ülke değiştirme
- daily case
- weekly case
- reklam/ipucu sistemi

DOSYA #001 temel loop:

# MASA → DOSYA → GÖRÜŞME → CCTV → YENİ BELGE → FOLLOW-UP → SONUÇ → DOSYA KAPAT

---

# 34. UNITY MİMARİ HEDEFİ

En önemli teknik kural:

# Yeni vaka eklemek için temel gameplay kodu yeniden yazılmamalı.

Case sistemi data-driven kurulmalı.

Case #002 mümkün olduğunca:

- yeni case data
- yeni localization
- yeni karakter assetleri
- yeni belgeler
- yeni investigation graph

eklenerek oluşturulabilmeli.

Core sistemlere minimum müdahale edilmeli.

---

# 35. UNITY İLK ROADMAP

## AŞAMA 1 — Foundation

- mevcut Unity projesini incele
- Unity sürümünü kontrol et
- mobil landscape yapı
- temel scene architecture
- localization
- merkezi game config
- save/load foundation
- input/touch foundation

Önce mevcut projeyi incele. Gereksiz dosya veya sistem oluşturma.

## AŞAMA 2 — Desk

Minimal masa.

İlk objeler:

- Dosya
- Gelen Evraklar
- Terminal

Interaction:

Desk → tap → focus → close → desk

## AŞAMA 3 — Case File

DOSYA #001:

- kapak
- olay raporu
- kişiler
- görüşmeler
- CCTV çıktısı
- gelen raporlar
- sonuç raporu

Yeni sayfalar runtime'da eklenebilmeli.

## AŞAMA 4 — Investigation Engine

Data-driven state sistemi.

NPC states.

Conditional questions.

Document unlock.

System unlock.

Follow-up interview unlock.

Case progression graph.

## AŞAMA 5 — CCTV + Interview

Reusable BPS CCTV component.

Text event playback.

Signal states.

Terminal audio/animation.

Pixel-art interview UI.

Character sprite states.

## AŞAMA 6 — DOSYA #001 VERTICAL SLICE

Gerçek assetler.

Bora.

Mert.

Elif.

Hasan.

Masa.

Dosyalar.

CCTV.

Sesler.

Micro animations.

Conclusion screen.

Android/mobile test.

---

# 36. İLK BÜYÜK MILESTONE

Oyuncu:

**Oyunu açar  
→ Bora'nın masasına gelir  
→ DOSYA #001'i alır  
→ soruşturmayı yürütür  
→ Hasan'a ulaşır  
→ doğru sonucu verir  
→ dosyayı kapatır  
→ masaya döner.**

Bu akış developer müdahalesi olmadan baştan sona çalıştığında ilk vertical slice tamamlanmış sayılır.

---

# 37. GÖRSEL REFERANS — ÇOK ÖNEMLİ

Önceki konuşmada oluşturulan ve kullanıcı tarafından kesinlikle beğenilen karakter yönü:

**minimalist pixel-art interrogation character/UI.**

Karakterler gerçek sprite/game asset hissi vermeli.

Kaçınılacak:

- AI illustration görünümü
- photorealism
- aşırı detaylı yüz
- skin texture
- concept-art portrait görünümü

İstenen:

- pixel clusters
- düşük/orta çözünürlük
- sınırlı palette
- sade yüz
- güçlü silhouette
- birkaç pikselle expression değişimi
- Papers, Please benzeri kontrollü game-asset hissi

Önceki referans görselde Elif için oluşturulan minimalist pixel-art kadın karakter yönü kullanıcı tarafından:

**“Kesinlikle bu.”**

şeklinde onaylandı.

Ancak final UI'da referans görselde bulunan:

- “İfade Durumları”
- “Normal”
- “Düşünüyor”
- “Tedirgin”
- “Savunmada”
- “Notlar: sakin görünüyor”
- “zaman çizelgesinde tutarsızlıklar var”

gibi geliştirici/debug yorumları oyuncuya GÖSTERİLMEYECEK.

Görselde yalnızca gerekli gameplay bilgisi kalmalı.

Bu referans görsel çalışma sohbetine ayrıca yüklenmelidir.

---

# 38. GENEL ÜRETİM PRENSİBİ

Ucuz ve tekrar kullanılabilir içerik:

- text
- reports
- documents
- statements
- messages
- evidence renders
- timeline
- reusable UI

Orta maliyet:

- pixel character interviews
- basit interactive scenes

Pahalı içerik:

- bespoke animation
- cinematic
- büyük interactive environments

Pahalı içerikler nadir kullanılmalı.

Oyunun “premium” hissi yüksek asset sayısından değil:

- güçlü UI
- pixel-art direction
- sound design
- micro-animation
- iyi yazılmış vakalar
- tutarlı interaction

üzerinden gelmeli.

---

# 39. PROJEDE KORUNACAK CÜMLELER

Tasarım kararlarında şu prensipleri referans al:

**“Masada görünen şey = kullanılabilen şey.”**

**“Oyuncuya hikâyeyi yazdırmıyoruz; soruşturmayı yorumlatıyoruz.”**

**“Uzmanlar sonucu verir; anlamını oyuncu çıkarır.”**

**“Yalan söylemek suçlu olmak değildir.”**

**“Bilgi yeni bilgiyi açar; checklist oyuncuyu yönlendirmez.”**

---

# 40. ŞU ANKİ DURUM

Tasarım konuşmasından aktif geliştirmeye geçildi.

Kullanıcı Unity'de:

**dedektif**

adında proje oluşturdu.

Öncelik artık yeni özellik brainstorm etmek değil.

Öncelik:

1. mevcut Unity projesini incelemek,
2. doğru foundation'ı kurmak,
3. DOSYA #001'i oynanabilir vertical slice hâline getirmek.

Kod yazmadan önce mevcut proje yapısını kontrol et.

Kullanıcının açıkça onaylamadığı yeni büyük gameplay sistemlerini kendiliğinden ekleme.

Scope'u DOSYA #001 vertical slice üzerinde tut.

Final oyun adı henüz belirlenmedi.

Working title:

# bube