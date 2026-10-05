# Dosya #035 — Metro (Fransa, 4. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Belleville — Mayıs 2030. **Masa tarihi:** 28 Mayıs 2030.

**Teklif metni:** Belleville metrosunda bir adam sabah treninin altında kalıyor. Gazeteler, yardıma ilk koşan yolcuyu kahraman ilan ediyor.

## Kanca
Sabah metrosunda bir adam raylara düşüyor. Yardıma ilk koşan, acil çağrıyı yapan yolcu, kahraman gibi anılıyor. Ama çağrı kayıt saati, trenin çarpmasından yedi saniye önce.

## Gerçek
2019’da kızı Camille’i bir yaya geçidinde öldüren ve beraat eden Rémi Vidal’in sabah saatlerini Lucien Mallet beş hafta boyunca not etti, peronun kör noktasını ölçtü. 28 Mayıs’ta 07.36’dan beri peronda bekledi; telefonu kulağına götürüp acil çağrıyı başlattı ve yedi saniye sonra boştaki eliyle Vidal’i trenin önüne itti, ardından ‘yardım edin’ diye bağırdı.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Odile Vasseur | 70 yaşında, emekli öğretmen; peronda bankta oturuyordu. | bankta oturan tanık; ‘itildi mi düştü mü’ emin değil |
| Karim Benhamou | 44 yaşında, metro sürücüsü; treni o kullanıyordu. | sürücü; tren kamerası |
| Lucien Mallet | 58 yaşında, acil çağrıyı yapan ve kurbanın yanına ilk koşan yolcu. | kahraman yolcu; katil, eski bir intikam |
| Pascal Royer | 39 yaşında, kurbanın gümrük müşavirliği ofisindeki ortağı. | ortak; kırmızı ringa, ortaklık sigortası |

Görüşülebilir kişi sayısı: **4**.

## Merkez yetenek
Saniyeleri sıralamak: acil çağrı, tren kamerası, kart okutma. Bir tanığın ‘ilk ben koştum’ demesi, olayı önceden bildiğini de söyleyebilir.

## Dünya ipi
Kurbanın çantasında Maison Delorme’un Chicago’ya giden sandıkları için gümrük beyannameleri: taşıyıcı Lakeshore Freight; her birine ekli köken mektubu ‘Dr. E. Varga’.
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca
- **Odile Vasseur — görüşme** (`odile`, interview) — açılır: report okununca
  - Ne gördünüz?
- **Karim Benhamou — görüşme** (`benhamou`, interview) — açılır: report okununca
  - Kabinden ne gördünüz?
- **TREN KABİN KAMERASI — kayıt dökümü** (`cab`, cctv) — açılır: benhamou okununca
  - 08.14.03 · Kamera: Çantalı bir adam peron kenarında telefonuna bakıyor; hemen arkasında gri paltolu bir adam, bir eli kulağında, telefon tutuyor.
  - 08.14.11 · Kamera: Gri paltolu adamın boştaki eli çantalı adamın sırtında; çantalı adam sarı çizginin üstünden öne düşüyor.
  - 08.14.13 · Kamera: Gri paltolu adam kalabalığın içine geri çekiliyor, telefon hâlâ kulağında, öbür eli yardım ister gibi havada.
- **Lucien Mallet — görüşme** (`mallet`, interview) — açılır: report okununca
  - Ne oldu?
  - Rémi Vidal’i tanıyor muydunuz? _(bilgiye göre değişir: ruling)_
- **ACİL ÇAĞRI KAYDI** (`calllog`, document) — açılır: cab, mallet okununca; soru: mallet.help; istenir (Acil çağrının saat kaydını iste, 4 sn)
- **KART OKUTMA KAYDI** (`navigo`, document) — açılır: mallet okununca; soru: mallet.help; istenir (Metro kartlarının okutma kaydını iste, 4 sn)
- **Pascal Royer — görüşme** (`royer`, interview) — açılır: report okununca
  - Ortaklıkta sorun var mıydı?
  - Çantasında ne vardı?
- **2019 KAZA DOSYASI** (`ruling`, document) — açılır: royer okununca; soru: royer.partner; istenir (2019 kaza dosyasını arşivden iste, 5 sn)
- **LUCİEN MALLET’NİN TELEFONU** (`phone`, document) — açılır: calllog okununca; istenir (Lucien Mallet’nin telefonunu incelemek için izin iste, 6 sn); gerekçeli talep: calllog + cab | navigo + ruling | calllog + ruling
- **Lucien Mallet — ikinci görüşme** (`mallet_2`, interview) — açılır: mallet, calllog okununca
  - Aramanız düşmeden yedi saniye önce başladı. _(öne sür: calllog, navigo, phone)_

## Rapor
- **Rémi Vidal’in ölümünden kim sorumlu?** **Lucien Mallet** ✔ · Pascal Royer · Kimse — kalabalıkta düştü
- **Nasıl?** **Bir elinde telefon tutan adam öbür eliyle raylara itti** ✔ · Kalabalık itti · Baygınlık geçirip düştü
- **Acil çağrı ne zaman başladı?** **Düşmeden yedi saniye önce** ✔ · Düştükten hemen sonra · Tren durduktan sonra
- **Belirleyici kanıt:** **Acil çağrı kaydı** ✔ · Tren kabin kamerası · Lucien Mallet’nin telefonu · Kart okutma kaydı

## Kanıt zinciri
Çağrı kaydı itmeden önce başladı; kabin kamerası eli, otopsi sırttaki morluğu, kart kaydı uzun bekleyişi, eski kaza dosyası bağı gösterdi. İki gerekçeyle açılan telefon, beş haftalık saat listesini verdi.

## Akıbet
- Lucien Mallet: Lucien Mallet tutuklandı; mahkemede kızının adını söyledi. Vidal’in çantasındaki Lakeshore beyannameleri ayrı dosyaya alındı.
- Pascal Royer: Pascal Royer suçlandı; ‘kahraman’ yolcuya kimse soru sormadı.
- Kimse — kalabalıkta düştü: Ölüm kaza olarak kapandı; Lucien Mallet’ye belediye bir madalya verdi.

## Ders
İlk koşan, her zaman ilk gören değildir; bazen önceden bilendir. Saniyeleri sırala.
