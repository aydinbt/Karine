# Dosya #041 — Kongre (ABD, 3. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Loop — Temmuz 2030. **Masa tarihi:** 27 Temmuz 2030.

**Teklif metni:** Loop’ta bir otelin çatısında bağış yemeğinin ardından kampanyanın genç asistanı kapalı havuzda ölü bulundu.

## Kanca
Loop’ta bir otelin çatısında meclis üyesi Dolores Vega için bağış yemeği. Gece yarısı kampanyanın genç asistanı Tyler Brooks çatı havuzunda yüzüstü bulunuyor. Sarhoş bir genç, kapanmış bir havuz, diyorlar. Tyler içki içmezdi.

## Gerçek
Kampanya saymanı Gordon Pratt, Lakeshore Freight’ın parasını depo işçisi adlarıyla meclis üyesinin kampanyasına akıtıyordu. Asistan Tyler Brooks bağışçıların yirmisinin aynı adreste olduğunu bulup gazeteciyi çatıdan arayınca Pratt kendi süit kartıyla kapalı havuza çıktı, telefonu almaya çalıştı ve onu suyun altında tuttu.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Marco Bellini | 45 yaşında, çatı barının barmeni. | barmen; kurbanın içkisi |
| Jordan Lee | 27 yaşında, kurbanın ev arkadaşı; kampanyada gönüllü. | ev arkadaşı; kurbanın son sözleri |
| Nina Kessler | 34 yaşında, yerel bir haber sitesinde araştırmacı gazeteci. | gazeteci; kurbanın kaynağı olduğu kişi |
| Ahmed Saleh | 39 yaşında, otelin gece müdürü. | gece müdürü; kart sistemi |
| Dolores Vega | 52 yaşında, Pilsen meclis üyesi; bağış yemeğinin ev sahibi. | meclis üyesi; kırmızı ringa |
| Clint Harlan | 61 yaşında, Lakeshore Freight’ın kurumsal ilişkiler başkan yardımcısı; bağışçı. | Lakeshore; bağışçı, ip |
| Gordon Pratt | 58 yaşında, kampanyanın saymanı. | sayman; katil, paravan bağışlar |

Görüşülebilir kişi sayısı: **7**.

## Merkez yetenek
Yedi kişi ve üç hat: otel sistemleri, kampanya finansı, gazetecinin dosyası. Kalabalık bir masada iki hattı seçmek, yanlış seçince geri dönüp beklemek.

## Dünya ipi
Bağış listesinde Lakeshore Freight’ın paravan şirketleri. Gazetecinin dosyasında bir dinleme kaydından söz eden bir muhbir: ‘Bir kadın sesi, E. diye imzalıyor, Napoli diye bir yer geçiyor.’
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca
- **ÇATI SERVİS KORİDORU KAMERASI — kayıt dökümü** (`roof`, cctv) — açılır: report okununca
  - 23.41 · Kamera: Gömlekli genç bir adam elinde telefonla çatı kapısından geçiyor.
  - 23.47 · Kamera: Koyu takımlı, iri yapılı yaşlıca bir adam kart okutup aynı kapıdan geçiyor.
  - 23.58 · Kamera: Aynı adam yalnız dönüyor; gömlek kolları ıslak, ceketinin kollarını aşağı çekiyor.
- **Marco Bellini — görüşme** (`bellini`, interview) — açılır: report okununca
  - Tyler ne içti?
  - Biriyle tartıştı mı?
- **Jordan Lee — görüşme** (`jordan`, interview) — açılır: report okununca
  - Tyler size bir şey söyledi mi?
- **Nina Kessler — görüşme** (`kessler`, interview) — açılır: report okununca
  - Tyler sizi aradı mı?
- **Ahmed Saleh — görüşme** (`saleh`, interview) — açılır: report okununca
  - Havuz kapısı 23.00’te kilitlenmiyor mu?
- **Dolores Vega — görüşme** (`vega`, interview) — açılır: report okununca
  - 23.30’dan sonra neredeydiniz?
- **Clint Harlan — görüşme** (`harlan`, interview) — açılır: report okununca
  - Şirketinizin işçileri neden aynı adresten bağış yapıyor?
- **Gordon Pratt — görüşme** (`pratt`, interview) — açılır: report okununca
  - Gece yarısından önce neredeydiniz? _(bilgiye göre değişir: roof)_
  - 23.47’de çatıya sizin kartınız açtı. _(öne sür: keycard, donors)_
- **HAT A — OTEL SİSTEMLERİ** (`line_hotel`, document) — açılır: saleh okununca; istenir (Otel sistemleri hattını aç, 8 sn); gerekçeli talep: saleh + roof | saleh + autopsy
- **HAVUZ KAPISI KART KAYDI** (`keycard`, document) — açılır: line_hotel okununca; istenir (Havuz kapısı kart kaydını iste, 5 sn); iz: line_hotel
- **HAT B — KAMPANYA FİNANSI** (`line_finance`, document) — açılır: jordan okununca; istenir (Kampanya finansı hattını aç, 8 sn); gerekçeli talep: jordan + kessler | jordan + bellini
- **BAĞIŞ DÖKÜMÜ** (`donors`, document) — açılır: line_finance okununca; istenir (Bağış dökümünü iste, 5 sn); iz: line_finance
- **HAT C — GAZETECİNİN DOSYASI** (`line_press`, document) — açılır: kessler okununca; istenir (Gazetecinin dosya hattını aç, 8 sn); gerekçeli talep: kessler + report | kessler + autopsy
- **GAZETECİNİN NOTLARI** (`pressfile`, document) — açılır: line_press okununca; istenir (Gazetecinin notlarını iste, 5 sn); iz: line_press

## Rapor
- **Tyler Brooks’u kim öldürdü?** **Gordon Pratt** ✔ · Clint Harlan · Dolores Vega · Kimse — sarhoş düştü
- **Nasıl öldü?** **Havuzda suyun altında tutuldu** ✔ · Sarhoşken havuza düştü · Kenarda kayıp başını çarptı
- **Kapalı havuzun kapısı nasıl açıldı?** **Saymanın süit kartıyla** ✔ · Personel kapıyı açık unuttu · Kurban kendi kartıyla
- **Belirleyici kanıt:** **Havuz kapısı kart kaydı** ✔ · Bağış dökümü · Servis koridoru kamerası · Gazetecinin notları

## Kanıt zinciri
Barmen kurbanın ayık olduğunu, gazeteci son aramayı, kamera ıslak kollarla dönen adamı gösterdi. Otel hattı kart kaydını, finans hattı paravan bağışları verdi. Gazetecinin hattı bir sesten söz etti: E. ve Napoli.

## Akıbet
- Gordon Pratt: Gordon Pratt tutuklandı. Paravan bağışlar seçim kuruluna bildirildi; Clint Harlan’ın ifadesi Lakeshore avukatlarının huzurunda alındı.
- Clint Harlan: Clint Harlan gözaltına alındı ve ertesi sabah avukatlarıyla çıktı; sayman kampanyayı yürütmeye devam etti.
- Dolores Vega: Dolores Vega’ya soruşturma açıldı; seçimi kaybetti, dosya kapandı.
- Kimse — sarhoş düştü: Ölüm kaza sayıldı; otel havuz kapısına yeni bir kilit taktı.

## Ders
Kalabalık bir masada herkes bir şey bilir; yalnız biri kapıyı açar.
