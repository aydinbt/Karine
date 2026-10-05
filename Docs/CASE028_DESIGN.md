# Dosya #028 — Karaoke (Japonya, 4. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Shinjuku — Mart 2030. **Masa tarihi:** 4 Mart 2030.

**Teklif metni:** Shinjuku’da bir idol, doğum günü partisinin ortasında yalnız kaldığı karaoke odasında ölü bulunuyor. İlk değerlendirme: astım.

## Kanca
Ünlü bir idol karaoke odasında ölü. Menajerin alibisi kusursuz görünüyor: cinayet saatinde odanın sistemine onun telefonundan şarkı eklenmiş ve o şarkıyı kendi sesiyle söylerken kaydedilmiş. Ama şarkı kuyruğu uzaktan, kayıt da bir yıl önceki doğum günü partisinden.

## Gerçek
Ajansın borcu Kisaragi Shōji’ye bağlı bir fona geçmişti ve fon, Aikawa Mio’nun Paris’teki bir moda markasıyla imzasını şart koşuyordu. Mio gruptan ayrılmak isteyince menajer Kanda Shūji 22.52’de onun yalnız kaldığı odaya girdi; kuru buzu açtı, havalandırmayı kapattı ve dolu inhaleri aldı, elinde ilacı olmayan hazne kaldı. Alibi için parti odasına telefonundan uzaktan şarkı istedi ve bir yıl önceki doğum günü kaydını çaldırdı.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Kobayashi Nana | 22 yaşında, grubun ikinci vokali; kurbanla aynı evde kalıyor. | ev arkadaşı; kurbanın ilacını biliyordu |
| Mizuno Reina | 20 yaşında, grubun en genç üyesi; o gece doğum günüydü. | doğum günü kızı; kırmızı ringa |
| Inoue Haru | 27 yaşında, grubun şarkı yazarı; kurbanın eski sevgilisi. | eski sevgili; tartışma, kırmızı ringa |
| Kanda Shūji | 46 yaşında, grubun menajeri; ajansın sahibi. | menajer; katil |
| Endō Yamato | 31 yaşında, karaoke salonunun gece sorumlusu. | salon sorumlusu; sistem kaydını veriyor |
| Tsuda Miki | 35 yaşında, hayran kulübü yöneticisi; kurbanın her konserinde ön sırada. | hayran kulübü; asansörde menajeri gördü |
| Gotō Ryū | 40 yaşında, magazin fotoğrafçısı; binanın karşısında bekliyordu. | fotoğrafçı; arka kapı fotoğrafı |

Görüşülebilir kişi sayısı: **7**.

## Merkez yetenek
Dijital alibiyi sökmek: bir kaydın ne zaman yapıldığı ile ne zaman çalındığı aynı şey değil. Yedi kişiden yalnız üçü önemli; gerisi gürültü.

## Dünya ipi
Ajansın borcunu geçen ay Kisaragi Shōji’nin bir eğlence fonu satın aldı; sözleşmede idolün Paris’te bir moda markasıyla imzası şart.
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca
- **Kobayashi Nana — görüşme** (`nana`, interview) — açılır: report okununca
  - Mio’nun astımı?
  - Son günlerde bir şey değişmiş miydi?
- **Mizuno Reina — görüşme** (`reina`, interview) — açılır: report okununca
  - Parti nasıldı?
- **Inoue Haru — görüşme** (`haru`, interview) — açılır: report okununca
  - Mio ile tartıştınız mı?
- **Kanda Shūji — görüşme** (`kanda`, interview) — açılır: report okununca
  - 22.30–23.30 arası neredeydiniz? _(bilgiye göre değişir: syslog)_
- **Endō Yamato — görüşme** (`yamato`, interview) — açılır: report okununca
  - Odaların sistemi neleri kaydeder?
  - O saatte bir şey dikkatinizi çekti mi?
- **Tsuda Miki — görüşme** (`miki`, interview) — açılır: report okununca
  - Binada mıydınız?
- **6. KAT KORİDOR KAMERASI — kayıt dökümü** (`corridor`, cctv) — açılır: yamato okununca
  - 22.52 · Kamera: Koyu takım elbiseli bir adam koridorun sonundaki odaya yürüyor, omzunun üstünden geriye bakıyor.
  - 22.53 · Kamera: Koridor boş; son odanın kapısı kapalı, altından renkli ışık yanıp sönüyor.
  - 23.06 · Kamera: Aynı adam asansöre değil, acil merdivene yürüyor; bir eli cebinde.
- **KARAOKE SİSTEM KAYDI — 6. KAT** (`syslog`, document) — açılır: yamato, kanda okununca; soru: yamato.system; istenir (Sistemin istek ve kayıt dökümünü iste, 5 sn)
- **İNHALER İNCELEMESİ** (`inhalerlab`, document) — açılır: nana, autopsy okununca; soru: nana.inhaler; istenir (İnhaleri incelemeye gönder, 6 sn)
- **FOTOĞRAFÇININ KARELERİ** (`backdoor`, document) — açılır: miki okununca; soru: miki.lift; istenir (Karşıdaki fotoğrafçıdan kareleri iste, 6 sn)
- **Gotō Ryū — görüşme** (`goto`, interview) — açılır: backdoor okununca
  - O gece neden bekliyordunuz?
- **ACİL MERDİVEN ARAMASI** (`cuff`, document) — açılır: corridor okununca; istenir (Acil merdiveni ara, 5 sn)
- **Kanda Shūji — ikinci görüşme** (`kanda_2`, interview) — açılır: kanda, syslog okununca
  - 22.52’de 606’ya girdiniz. _(öne sür: syslog, inhalerlab, cuff)_

## Rapor
- **Aikawa Mio’nun ölümünden kim sorumlu?** **Kanda Shūji** ✔ · Inoue Haru · Kobayashi Nana · Kimse — kendiliğinden astım krizi
- **Nasıl?** **Kuru buz ve kapatılan havalandırmayla tetiklenen kriz; inhaler boş hazneyle değiştirilmiş** ✔ · Boğuldu · İçkisine bir şey katıldı · Kendiliğinden kriz
- **Menajerin alibisi neydi?** **Uzaktan istek ve eski kayıt** ✔ · Gerçekten 601’de söylüyordu · Alibisi yoktu
- **Belirleyici kanıt:** **Karaoke sistem kaydı** ✔ · İnhaler incelemesi · Acil merdiven araması · Fotoğrafçının kareleri

## Kanıt zinciri
Koridor kamerası girişini, sistem kaydı ‘canlı’ şarkının uzaktan ve bir yıllık olduğunu, inhaler incelemesi değiştirilen hazneyi, acil merdiven kol düğmesini ve son hazneyi verdi.

## Akıbet
- Kanda Shūji: Kanda Shūji tutuklandı. Ajansın borcunu satın alan fonun Kisaragi Shōji’ye bağlı olduğu dosyaya not düşüldü; Paris sözleşmesi iptal edildi.
- Inoue Haru: Inoue Haru suçlandı; menajerin uzaktan alibisi hiç sorgulanmadı.
- Kobayashi Nana: Kobayashi Nana gözaltına alındı; grup dağıldı.
- Kimse — kendiliğinden astım krizi: Ölüm talihsiz bir kriz olarak kapandı; ajans Paris sözleşmesini başka bir üyeyle imzaladı.

## Ders
Bir kayıt, kaydedildiği anın kanıtıdır; çalındığı anın değil.
