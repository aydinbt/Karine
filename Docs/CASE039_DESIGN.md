# Dosya #039 — Depo (ABD, 1. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Pilsen — Temmuz 2030. **Masa tarihi:** 12 Temmuz 2030.

**Teklif metni:** Pilsen’deki Lakeshore Freight deposunda bir forklift operatörü devrilen rafın altında ölü. İş kazası, diyor herkes.

## Kanca
Paris’ten gelen manifestolar Chicago’nun güneybatısında, Pilsen’deki bir depoyu gösteriyordu. Bora şubeye vardığı sabah o deponun gece vardiyasından bir forklift operatörü, devrilen bir palet rafının altında ölü bulunur. İş kazası, diyor herkes.

## Gerçek
Vardiya şefi Ray Kowalski, Paris’ten gelen müzayede sandıklarını ‘makine parçası’ diye yeniden etiketliyordu. Forklift operatörü Luis Ortega fotoğraf çekip sendikaya gideceğini yazınca Kowalski onu konteynırın yanına çağırdı, levyeyle öldürdü. Yirmi dakika sonra kendi kartıyla forklifte bindi, rafın cıvatalarını çatalla kopardı ve rafı cesedin üstüne devirdi.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Marisol Reyes | 31 yaşında, depoda envanter memuru; kurbanın sevgilisi. | sevgili; envanter, son mesaj |
| Teresa Novak | 50 yaşında, sendika temsilcisi; forklift operatörü. | sendika; kurbanın güvendiği kişi |
| Dwayne Carter | 47 yaşında, deponun gece güvenlik görevlisi. | güvenlik; kamerayı bilen |
| Pete Lindqvist | 62 yaşında, şehrin iş güvenliği müfettişi. | müfettiş; ankraj cıvataları |
| Hector Ortega | 35 yaşında, kurbanın ağabeyi; şartlı tahliyede. | ağabey; sabıkalı, kırmızı ringa |
| Ray Kowalski | 54 yaşında, Lakeshore Freight Pilsen deposunun gece vardiya şefi. | vardiya şefi; katil, etiket değişimini o yürütüyor |

Görüşülebilir kişi sayısı: **6**.

## Merkez yetenek
Yeni mekanik: soruşturma hatları. Şube aynı anda yalnız iki hat yürütebilir: depo operasyonu, telefon kayıtları, şartlı tahliye kayıtları. Seçmediğin hat kapanır; geri açmak zaman alır.

## Dünya ipi
Kurban, Paris müzayede lot etiketli sandıkların ‘makine parçası’ diye yeniden etiketlendiğini görmüştü. Sandıklar Maison Delorme’dan, gönderen Lakeshore Freight.
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca
- **İŞ GÜVENLİĞİ TEFTİŞİ** (`inspect`, document) — açılır: report okununca; istenir (Şehrin iş güvenliği teftiş raporunu iste, 6 sn)
- **KORİDOR 14 KAMERASI — kayıt dökümü** (`dock`, cctv) — açılır: report okununca
  - 02.02 · Kamera: Koridorun sonunda bir forklift; yanında yerde hareketsiz yatan reflektörlü yelekli biri.
  - 02.11 · Kamera: Reflektörlü yelekli biri forklifti koridora sürüyor; çatallar raf ayağının dibinde, alçakta.
  - 02.12 · Kamera: Uzun raf eğilip devriliyor, sandıklar koridora düşüyor; forklift geri çekiliyor.
- **Marisol Reyes — görüşme** (`reyes`, interview) — açılır: report okununca
  - Luis son günlerde ne konuşuyordu?
  - O gece onu en son ne zaman gördünüz?
- **Teresa Novak — görüşme** (`novak`, interview) — açılır: report okununca
  - Luis size bir şey söyledi mi?
- **Dwayne Carter — görüşme** (`carter`, interview) — açılır: report okununca
  - Kameralara bakıyor muydunuz?
- **Pete Lindqvist — görüşme** (`lindqvist`, interview) — açılır: report okununca
  - Raf kendiliğinden devrilebilir mi?
- **Hector Ortega — görüşme** (`hector`, interview) — açılır: report okununca
  - Kardeşinize borcunuz varmış.
- **Ray Kowalski — görüşme** (`kowalski`, interview) — açılır: report okununca
  - Olay sırasında neredeydiniz?
  - 02.11’de 3 numaralı forklifti siz kullandınız. _(öne sür: telematics, texts)_
- **HAT A — DEPO OPERASYONU** (`line_ops`, document) — açılır: inspect okununca; istenir (Depo operasyon hattını aç, 8 sn); gerekçeli talep: inspect + autopsy | inspect + dock
- **FORKLİFT TELEMATİĞİ — 3 NUMARA** (`telematics`, document) — açılır: line_ops okununca; istenir (Forklift telematiğini iste, 5 sn); iz: line_ops
- **VARDİYA ÇİZELGESİ** (`roster`, document) — açılır: line_ops okununca; istenir (Vardiya çizelgesini iste, 4 sn); iz: line_ops
- **HAT B — TELEFON KAYITLARI** (`line_phone`, document) — açılır: novak okununca; istenir (Telefon kayıtları hattını aç, 8 sn); gerekçeli talep: novak + reyes | novak + autopsy
- **LUIS ORTEGA’NIN MESAJLARI** (`texts`, document) — açılır: line_phone okununca; istenir (Kurbanın mesajlarını iste, 5 sn); iz: line_phone
- **BAZ İSTASYONU KAYDI** (`tower`, document) — açılır: line_phone okununca; istenir (Baz istasyonu kaydını iste, 5 sn); iz: line_phone
- **HAT C — ŞARTLI TAHLİYE KAYITLARI** (`line_parole`, document) — açılır: hector okununca; istenir (Şartlı tahliye hattını aç, 8 sn); gerekçeli talep: hector + report | hector + autopsy
- **ELEKTRONİK KELEPÇE KAYDI** (`monitor`, document) — açılır: line_parole okununca; istenir (Elektronik kelepçe kaydını iste, 4 sn); iz: line_parole

## Rapor
- **Luis Ortega’yı kim öldürdü?** **Ray Kowalski** ✔ · Hector Ortega · Teresa Novak · Kimse — iş kazası
- **Nasıl öldü?** **Levyeyle başına vuruldu, raf sonradan devrildi** ✔ · Raf üstüne devrildi · Forklift çarptı
- **Raf nasıl devrildi?** **Ölümden sonra forkliftle devrildi** ✔ · Cıvatalar yoruldu, kendiliğinden devrildi · Kurban forkliftle çarptı
- **Belirleyici kanıt:** **Forklift telematiği** ✔ · Kurbanın mesajları · İş güvenliği teftişi · Elektronik kelepçe kaydı

## Kanıt zinciri
Otopsi kafadaki darbenin canlıyken, ezilmenin ölümden sonra olduğunu; teftiş cıvataların itilerek koptuğunu gösterdi. Operasyon hattı forkliftteki kartı, telefon hattı son mesajları verdi. Şartlı tahliye hattı yalnız ağabeyi akladı.

## Akıbet
- Ray Kowalski: Ray Kowalski tutuklandı. Konteynır 22 mühürlendi: içinde Maison Delorme’dan gelmiş on dört sandık, ‘makine parçası’ beyanlı. Lakeshore Freight’ın avukatları ertesi sabah şubedeydi.
- Hector Ortega: Hector Ortega tutuklandı; konteynır 22 ertesi gün yola çıktı.
- Teresa Novak: Teresa Novak gözaltına alındı; sendika depoyu greve soktu, dosya kapandı.
- Kimse — iş kazası: Ölüm iş kazası sayıldı; Lakeshore bir para cezası ödedi.

## Ders
İki hat açabilirsin. Hangisinin cevabı değiştireceğini, açmadan önce düşün.
