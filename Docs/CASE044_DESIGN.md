# Dosya #044 — Göl (ABD, 6. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Michigan Gölü — Ağustos 2030. **Masa tarihi:** 31 Ağustos 2030.

**Teklif metni:** Lakeshore’un başkan yardımcısı Michigan Gölü’nde kendi teknesinden düştü. Pazartesi bube’de randevusu vardı.

## Kanca
Bir hafta önce bağış yemeğinde, avukatlarının arkasına saklanan adam: Clint Harlan. Cuma gecesi kendi yelkenlisinde, Michigan Gölü’nde bir yarış dönüşü güverteden düşüyor. Can yeleği üstünde ama şişmemiş. Pazartesi sabahı bube Chicago’da bir randevusu vardı.

## Gerçek
Clint Harlan pazartesi bube’ye Lakeshore’un sahibini ve Napoli bağlantısını anlatacaktı. Şirketin hukuk müşaviri Brent Calloway bunu Napoli’deki bir numaraya bildirdi, ‘hafta sonu çözülsün’ yanıtını aldı. Kalkıştan önce Harlan’ın yeleğinin kartuşunu söktü; tekne rüzgâra dönerken baş güvertede onu vinç koluyla vurup göle itti ve ‘kamarada’ dedi.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Lauren Cho | 36 yaşında, kurtarma dalgıcı. | dalgıç; ceset ve yelek |
| Arne Jensen | 68 yaşında, marinanın liman müdürü. | liman müdürü; dönüş kaydı |
| Skip Morrow | 63 yaşında, teknenin kaptanı. | kaptan; dümende |
| Rico Mendes | 24 yaşında, teknenin tayfası. | tayfa; ön güvertede |
| Diane Harlan | 57 yaşında, kurbanın eşi. | eş; mirasçı, kırmızı ringa |
| Brent Calloway | 50 yaşında, Lakeshore Freight’ın baş hukuk müşaviri. | hukuk müşaviri; katil |

Görüşülebilir kişi sayısı: **6**.

## Merkez yetenek
Altı kişi bir teknede; denizde kimse kaçamaz ama herkes başka yöne bakar. Üç hat: tekne incelemesi, telefonlar, teknenin rotası. Hangisini kapatırsan o kapı kapanır.

## Dünya ipi
Harlan pazartesi bube’ye ‘Lakeshore’un sahibini ve Napoli’yi’ anlatacaktı. Şirketin hukuk müşaviri bunu bir gece önce birine yazmıştı: ‘C. konuşmak istiyor. Edda’ya söyle.’
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca
- **DALGIÇ RAPORU** (`divenote`, document) — açılır: cho okununca
- **MARİNA İSKELE KAMERASI — kayıt dökümü** (`pier`, cctv) — açılır: jensen okununca
  - 16.12 · Kamera: Kalkıştan önce lacivert yelekli bir adam deri bir sefer çantasıyla tekneye binip tek başına aşağı iniyor.
  - 00.05 · Kamera: Dönüşte tekne yanaşıyor; hırkalı bir kadın ve beyaz gömlekli genç tayfa güverte boyunca bağırıyor.
  - 00.11 · Kamera: Lacivert yelekli adam aynı çantayla tekneden inip iskelede hızla uzaklaşıyor.
- **Lauren Cho — görüşme** (`cho`, interview) — açılır: report okununca
  - Yelekte ne gördünüz?
- **Arne Jensen — görüşme** (`jensen`, interview) — açılır: report okununca
  - Dönüşte ne gördünüz?
- **Skip Morrow — görüşme** (`morrow`, interview) — açılır: report okununca
  - Düştüğü an neredeydiniz?
- **Rico Mendes — görüşme** (`mendes`, interview) — açılır: report okununca
  - Ön güvertede ne gördünüz?
- **Diane Harlan — görüşme** (`diane`, interview) — açılır: report okununca
  - Kocanız son günlerde nasıldı?
- **Brent Calloway — görüşme** (`calloway`, interview) — açılır: report okununca
  - Clint düştüğünde neredeydiniz? _(bilgiye göre değişir: pier)_
  - Kartuşu siz söktünüz. _(öne sür: boat, phone)_
- **HAT A — TEKNE İNCELEMESİ** (`line_boat`, document) — açılır: divenote okununca; istenir (Tekne incelemesi hattını aç, 8 sn); gerekçeli talep: divenote + autopsy | divenote + pier
- **TEKNE VE ÇANTA İNCELEMESİ** (`boat`, document) — açılır: line_boat okununca; istenir (Tekne ve çanta incelemesini iste, 6 sn); iz: line_boat
- **HAT B — TELEFON KAYITLARI** (`line_phone`, document) — açılır: diane okununca; istenir (Telefon kayıtları hattını aç, 8 sn); gerekçeli talep: diane + autopsy | diane + morrow
- **BRENT CALLOWAY’İN MESAJLARI** (`phone`, document) — açılır: line_phone okununca; istenir (Brent Calloway’in mesajlarını iste, 5 sn); iz: line_phone
- **HAT C — TEKNENİN ROTASI** (`line_ais`, document) — açılır: jensen okununca; istenir (Teknenin rota hattını aç, 8 sn); gerekçeli talep: jensen + report | jensen + autopsy
- **AIS ROTA KAYDI** (`ais`, document) — açılır: line_ais okununca; istenir (AIS rota kaydını iste, 4 sn); iz: line_ais

## Rapor
- **Clint Harlan’ı kim öldürdü?** **Brent Calloway** ✔ · Diane Harlan · Skip Morrow · Kimse — sarhoş düştü
- **Nasıl öldü?** **Vinç koluyla vuruldu, göle itildi** ✔ · Sarhoş güverteden düştü · Bumba çarptı
- **Can yeleği neden şişmedi?** **Kartuşu katil kalkıştan önce söktü** ✔ · Yelek arızalıydı · Kurban kartuşsuz yelek giymişti
- **Belirleyici kanıt:** **Tekne ve çanta incelemesi** ✔ · Avukatın mesajları · İskele kamerası · AIS rota kaydı

## Kanıt zinciri
Otopsi darbenin suya düşmeden önce olduğunu, dalgıç kartuşun söküldüğünü, tayfa vinç kolunu, iskele kamerası çantayı gösterdi. Tekne hattı kolu ve kartuşu, telefon hattı Napoli mesajlarını verdi.

## Akıbet
- Brent Calloway: Brent Calloway tutuklandı. Napoli numarasındaki mesajlar ‘Edda’ adıyla birlikte bube Chicago’nun Lakeshore dosyasına girdi.
- Diane Harlan: Diane Harlan tutuklandı; avukat dosyayı ‘müvekkilinin mirası’ adına yönetti.
- Skip Morrow: Skip Morrow gözaltına alındı; tekne sezon sonuna kadar mühürlendi.
- Kimse — sarhoş düştü: Ölüm kaza sayıldı; Lakeshore bir taziye bildirisi yayımladı.

## Ders
Herkesin başka yöne baktığı bir teknede, hiç bakmayan kişi her şeyi önceden hazırlamıştır.
