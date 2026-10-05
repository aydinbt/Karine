# Dosya #070 — Depo (Avustralya, 4. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Footscray tramvay deposu — Kasım 2030. **Masa tarihi:** 16 Kasım 2030.

**Teklif metni:** Bir sendika temsilcisi, bakım çukurunda üstüne kayan bir tramvayın altında bulundu.

## Kanca
Footscray’in tramvay deposunda sendikanın iş güvenliği temsilcisi Neil Ashworth, bakım çukurunda, üstüne kayan bir tramvayın altında ölü bulunuyor. Fren kaydı ‘frenler sağlam’ diyor.

## Gerçek
Vardiya şefi Paul Kerrigan yıllarca bakım defterine ‘sağlam’ yazdı. İhaleyi kazanacak konsorsiyum onu ‘sorunsuz devir’ primi listesine koymuştu. Sendikanın Neil Ashworth’u frenleri raporlayacaktı. Kerrigan gece panodan anahtarı aldı ve Neil çukurdayken freni boşalttı.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Wiremu Tane | 44 yaşında, bakım teknisyeni; cesedi bulan. | teknisyen; cesedi bulan, kırmızı ringa |
| Dimitri Kostas | 62 yaşında, emekli sürücü; deponun gece bekçisi. | gece bekçisi; sesi duyan |
| Cormac Byrne | 29 yaşında, tramvay sürücüsü. | sürücü; tramvayı park eden |
| Rania Haddad | 38 yaşında, sendikanın iş güvenliği temsilcisi. | sendika temsilcisi; raporu bilen |
| Maeve Sullivan | 55 yaşında, ulaşım idaresinde sözleşme müdürü. | sözleşme müdürü; ihaleyi yöneten |
| Paul Kerrigan | 50 yaşında, tramvay deposunun gece vardiya şefi. | vardiya şefi; katil |

Görüşülebilir kişi sayısı: **6**.

## Merkez yetenek
Altı kişi, üç hat: depo, sendika, sözleşme. Masada iki hat aynı anda açık durur; üçüncüyü açmak için birini kapatmak gerekir.

## Dünya ipi
Hattın özelleştirme ihalesini kazanması beklenen konsorsiyumun güvencesi Pacific Liaison Trust. Kazanırsa ilk iş: ‘eski filonun bakım raporlarını devralmak’.
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca; istenir (Otopsi raporunu iste, 7 sn)
- **BAKIM ÇUKURU İNCELEMESİ** (`pit`, document) — açılır: report okununca; istenir (Bakım çukurunun incelenmesini iste, 5 sn)
- **Wiremu Tane — görüşme** (`wiremu`, interview) — açılır: report okununca
  - Çukura neden indiniz?
- **Dimitri Kostas — görüşme** (`dimitri`, interview) — açılır: report okununca
  - Gece ne duydunuz?
- **Cormac Byrne — görüşme** (`cormac`, interview) — açılır: report okununca
  - Tramvayı siz mi park ettiniz?
- **Rania Haddad — görüşme** (`haddad`, interview) — açılır: report okununca
  - Neil neyi raporluyordu?
- **Maeve Sullivan — görüşme** (`maeve`, interview) — açılır: haddad okununca
  - Raporu neden ertelemek istediniz?
- **HAT A — DEPO** (`line_depot`, document) — açılır: wiremu okununca; istenir (Depo hattını aç, 8 sn); gerekçeli talep: wiremu + pit | wiremu + dimitri
- **VARDİYA ŞEFİNİN DOLABI — ARAMA** (`locker`, document) — açılır: line_depot okununca; istenir (Vardiya şefinin dolabını aramak için izin iste, 5 sn); iz: line_depot
- **HAT B — SENDİKA** (`line_union`, document) — açılır: haddad okununca; istenir (Sendika hattını aç, 8 sn); gerekçeli talep: haddad + cormac | haddad + wiremu
- **SENDİKA YAZIŞMALARI** (`mail`, document) — açılır: line_union okununca; istenir (Sendika yazışmalarını iste, 5 sn); iz: line_union
- **HAT C — İHALE** (`line_bid`, document) — açılır: maeve okununca; istenir (İhale hattını aç, 8 sn); gerekçeli talep: maeve + haddad | maeve + report
- **İHALE DOSYASI** (`bid`, document) — açılır: line_bid okununca; istenir (İhale dosyasını iste, 5 sn); iz: line_bid
- **Paul Kerrigan — görüşme** (`kerrigan`, interview) — açılır: report okununca
  - Gece yarısı neredeydiniz?
  - Freni siz boşalttınız. _(öne sür: locker, mail)_

## Rapor
- **Neil Ashworth’u kim öldürdü?** **Paul Kerrigan** ✔ · Wiremu Tane · Maeve Sullivan · Kimse — fren kendiliğinden boşaldı
- **Nasıl öldü?** **Freni bilerek boşaltılan tramvay çukurdaki kurbanın üstüne yürüdü** ✔ · Fren kendiliğinden boşaldı · Çukura düşüp ezildi
- **Fren kolunun anahtarı nereye gitti?** **Vardiya şefinin dolabına** ✔ · Anahtar panosuna geri · Çukurun dibine
- **Belirleyici kanıt:** **Dolap araması** ✔ · Sendika yazışmaları · İhale dosyası · Çukur incelemesi

## Kanıt zinciri
Üç hattan ikisi yetti: depo hattı anahtarı, sendika hattı ‘gelme’ mesajını verdi. İhale hattı nedeni gösterdi.

## Akıbet
- Paul Kerrigan: Paul Kerrigan tutuklandı; on dört tramvay seferden çekildi. İhale komisyonu Pacific Liaison güvencesini dosyadan çıkardı.
- Wiremu Tane: Wiremu Tane tutuklandı; anahtar dolapta kaldı.
- Maeve Sullivan: Maeve Sullivan sorgulandı; şef işine döndü.
- Kimse — fren kendiliğinden boşaldı: Ölüm kaza sayıldı; tramvaylar raylarda kaldı.

## Ders
Defter freni sağlam diyordu. Defteri o yazmıştı.
