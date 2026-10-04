# Dosya #009 — Emanet

**Tür:** iç soruşturma / emanet eksikliği · **Yer ve tarih:** Fatih, 11–12 Aralık 2028 · **Zincir:** #008 → #009

## Gerçek
Altı aylık takibin sonunda Kuzey Depo boş çıkacaktı. Komiser Hakan Özer, 2023'teki 27-118 el koymasından kayıtsız sakladığı 420 gramı bagajında getirip kuzey rafın arkasına koydu; telsizde "tekrar kontrol edin" deyip paketi kendisi "buldu" (E-17). Arda dört paketi emanet tezgâhına bırakıp formu "Adet: 4" diye doldurdu ve 01.19'da çıktı. Laboratuvar profilinin 27-118'e çıkacağını bilen Hakan 01.21'de kartıyla amirlik koridorundaki iç kapıdan (kamera görmez) girip büyük paketi geri aldı. Nesrin 01.31'de üç paket taradı. 03.42'de Ceren, Hakan'ın isteğiyle amirlik terminalinde Nesrin'in açık kalan hesabından E-17 ön kaydını sildi ve fotoğraf etiketini "16 → 17" düzeltti.

## Kişiler
Arda Yılmaz (34), Komiser Hakan Özer (47), Nesrin Eker (43, emanet), Volkan Ateş (39), Ceren Başar (31, kayıt), Ali Rıza Toprak (52, laboratuvar).

## Mekanik: soruşturma hattı
- Üç hat: **A — Arda** (banka, dolap, araç), **B — Emanet** (erişim günlüğü, kartlı geçiş, sayım), **C — Operasyon** (telsiz, ham liste, fotoğraflar, ekip hareketleri).
- Hat, inceleme izni motoruyla iki dayanakla açılır (`requestKind: "line"`). Aynı anda en çok 2 hat açık (`lineSlots`). Alt kayıtlar yalnız açık hattan istenir.
- Hat kapatılabilir; gelmiş kayıtlar kalır. Kapalı hat yeniden açılabilir ama `lineReopenPenaltySeconds` (20 sn) ek bekleme ile; kilitlenme olmaz.
- A hattı tuzaktır: borç var ama nakit yok, dolap boş, araçtaki iz delil nakliyle açıklanır.

## Bağlantılar
- Amirlik araması: erişim günlüğü + (kart kaydı ya da telsiz) → 27-118 dosyası; madde yok.
- 27-118 ↔ E-17 bağlantısı → laboratuvar karşılaştırması.
- Hakan son görüşme: "Bu paketin nereden geldiğiyle ilgileniyorum." (laboratuvar raporu öne sürülür).

## Rapor
1. E-17'yi operasyona kim soktu? **Hakan** (yanlış: meşru el koyma / Arda / Ceren / Nesrin / belirlenemedi)
2. Kayıt değişikliğine kim katıldı? **Ceren** (Nesrin / Arda / belirlenemedi)
3. E-17 nasıl kayboldu? **Arda teslim etti; Nesrin'e ulaşmadan Hakan geri aldı** (Arda zimmet / Nesrin aldı / belirlenemedi)
4. Delil: **laboratuvar karşılaştırması**.

## Son
Kapanış kartından sonra bir kez "BDS İÇ DENETİM — GİZLİ" zarfı düşer. Doğru raporda kariyer kartına "Yetki: GENİŞLETİLDİ" satırı eklenir (`grantsAuthority`).
