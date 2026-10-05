# Dosya #068 — Ahır (Avustralya, 2. dosya)

**Tür:** şüpheli ölüm. **Yer/zaman:** Flemington ahırları — Kasım 2030. **Masa tarihi:** 1 Kasım 2030.

**Teklif metni:** Kupanın favorisinin bölmesinde bir yarış müfettişi ölü; ilk rapor ‘at tekmesi’ diyor.

## Kanca
Melbourne Kupası’na dört gün var. Flemington ahırlarında yarış komiserliğinin müfettişi Owen Pryce, favori ‘Southern Liaison’ın bölmesinde ölü bulunuyor. İlk bakışta at tekmelemiş.

## Gerçek
Pacific Liaison Trust’ın sahibi olduğu favori at, kupayı kazanmamalıydı. Antrenör Grant Ainsley atı ilaçladı; yüzlerce hesap atın aleyhine oynadı. Müfettiş Owen Pryce atın kanından numune aldı. Ainsley gece tüpü geri istedi, alamadı, nal perçinli çekiçle vurdu ve atı arkaya bağlayıp tekme süsü verdi.

## Kişiler
| Kişi | Bilgi | Rol (gizli) |
|---|---|---|
| Harriet Combe | 45 yaşında, ahırın veteriner hekimi. | veteriner; ayrılan tanık |
| Deng Akol | 24 yaşında, seyis; cesedi bulan. | seyis; cesedi bulan |
| Sione Taufa | 38 yaşında, nalbant. | nalbant; kırmızı ringa |
| Shane Keogh | 27 yaşında, jokey. | jokey; atı tutmaya razı |
| Mai Pham | 31 yaşında, bahis şirketinde risk analisti. | bahis analisti; parayı gören |
| Declan Rafferty | 61 yaşında, at sahibi; ortaklığın temsilcisi. | sahip; Trust’ın adamı |
| Grant Ainsley | 49 yaşında, Flemington’da antrenör; ‘Southern Liaison’ atının hocası. | antrenör; katil |

Görüşülebilir kişi sayısı: **7**.

## Merkez yetenek
Yedi kişi, üç hat: ahır, bahis, sahiplik. Masada aynı anda yalnız iki hat açık durabilir. Veteriner otopsi okunduktan sonra ülkeden ayrılıyor; onu dinlemek istiyorsan önce dinle.

## Dünya ipi
Atın sahibi ortaklığın parası Pacific Liaison Trust’tan geliyor; Footscray’deki balık pazarına kredi veren aynı vakıf. Atın adı tesadüf değil.
(Çözüm şartı değildir; yalnız bir satır olarak geçer.)

## Akış
- **OLAY RAPORU** (`report`, document) — açık
- **Harriet Combe — görüşme** (`harriet`, interview) — açılır: report okununca; kapanır: autopsy okununca
  - At tekmelemiş olabilir mi?
- **OTOPSİ RAPORU** (`autopsy`, document) — açılır: report okununca; istenir (Otopsi raporunu iste, 7 sn)
- **BÖLME İNCELEMESİ** (`stall`, document) — açılır: report okununca; istenir (Bölmenin incelenmesini iste, 5 sn)
- **Deng Akol — görüşme** (`deng`, interview) — açılır: report okununca
  - Gece ahırda ne oldu?
- **Sione Taufa — görüşme** (`sione`, interview) — açılır: report okununca
  - Yaradaki iz bir nala benziyormuş.
- **Shane Keogh — görüşme** (`keogh`, interview) — açılır: report okununca
  - Kupada atı nasıl sürecektiniz?
- **Mai Pham — görüşme** (`mai`, interview) — açılır: keogh okununca
  - Ne tür bir hareket gördünüz?
- **Declan Rafferty — görüşme** (`rafferty`, interview) — açılır: report okununca
  - Atın gerçek sahibi kim?
- **HAT A — AHIR** (`line_stable`, document) — açılır: deng okununca; istenir (Ahır hattını aç, 8 sn); gerekçeli talep: deng + stall | deng + autopsy
- **ANTRENÖRÜN RÖMORKU — ARAMA** (`float`, document) — açılır: line_stable okununca; istenir (Antrenörün römorkunu aramak için izin iste, 5 sn); iz: line_stable
- **HAT B — BAHİS** (`line_bets`, document) — açılır: mai okununca; istenir (Bahis hattını aç, 8 sn); gerekçeli talep: mai + keogh | mai + rafferty
- **BAHİS DÖKÜMÜ** (`bets`, document) — açılır: line_bets okununca; istenir (Bahis dökümünü iste, 5 sn); iz: line_bets
- **HAT C — SAHİPLİK** (`line_owner`, document) — açılır: rafferty okununca; istenir (Sahiplik hattını aç, 8 sn); gerekçeli talep: rafferty + report | rafferty + autopsy
- **ORTAKLIK SÖZLEŞMESİ** (`syndicate`, document) — açılır: line_owner okununca; istenir (Ortaklık sözleşmesini iste, 5 sn); iz: line_owner
- **Grant Ainsley — görüşme** (`ainsley`, interview) — açılır: report okununca
  - Dün gece neredeydiniz?
  - Owen Pryce’ı siz öldürdünüz. _(öne sür: float, bets)_

## Rapor
- **Owen Pryce’ı kim öldürdü?** **Grant Ainsley** ✔ · Sione Taufa · Declan Rafferty · Kimse — at tekmeledi
- **Nasıl öldü?** **Nal perçinli çekiçle vuruldu** ✔ · At tekmeledi · Düşüp başını çarptı
- **Müfettişin kayıp numune tüpü nereye gitti?** **Antrenörün römorkunda saman altına** ✔ · Laboratuvara · Nalbantın ocağına
- **Belirleyici kanıt:** **Römork araması** ✔ · Bahis dökümü · Ortaklık sözleşmesi · Otopsi raporu

## Kanıt zinciri
Veteriner otopsiden önce dinlenmeliydi; sonra gitti. Üç hattan ikisi aynı anda açıktı: ahır hattı çekici, bahis hattı kızın adını, sahiplik hattı primi verdi.

## Akıbet
- Grant Ainsley: Grant Ainsley tutuklandı; Southern Liaison kupadan çekildi. Pacific Liaison Trust’ın alt hesabı donduruldu.
- Sione Taufa: Sione Taufa tutuklandı; çekiç römorkta kaldı.
- Declan Rafferty: Declan Rafferty sorgulandı; asıl el serbest kaldı.
- Kimse — at tekmeledi: Ölüm kaza sayıldı; at kupada koştu ve kaybetti.

## Ders
At tekmelememişti. Yalnızca orada duruyordu.
