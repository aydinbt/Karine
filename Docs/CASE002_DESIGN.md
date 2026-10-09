# Dosya #002 — Son Sefer

Bu sayfa içerik ekibi içindir; oyuncuya gösterilmez. Oynanabilir vaka verisi `Assets/Bube/Resources/Bube/Cases/case002.json`, Türkçe metinler `Assets/Bube/Resources/Bube/Locales/tr.case002.json` içindedir. Bu sürüm, 26 Eylül 2026'da kullanıcının verdiği senaryonun yerine geçtiği eski "Kayıp Yedek" taslağını **tamamen** kaldırır.

**Büro:** BDS Asayiş Masası — kişilere karşı işlenen suçlar. **Nitelik:** yağma/gasp iddiası ve yaralama. **Yer/zaman:** Beşiktaş, sahil yolu ara sokağı, 21 Kasım 2026 gecesi.

## Kesin olay

Taksi şoförü Selçuk Yalın (42) aracını 22.58'de sokağa park eder. Yolcusu Deniz Arslan (24) ile ücret yüzünden tartışır; Deniz 23.03'te iner ve olaydan önce uzaklaşır — suçla ilgisi yoktur.

Emre Koç (34) 23.18'de gelir. Selçuk'a kayıt dışı bir borcu vardır ve 8.000 TL'yi ödemeye gelmiştir. Tartışma büyür, 23.20'de Emre Selçuk'u kolundan tutup iter; Selçuk geri giderek bordüre çarpar ve başından yaralanır. Emre 23.31'de **parayı bırakarak** ayrılır.

Kerem Şahin (29, köşedeki büfe) 23.34'te araca yaklaşır, açık çantadaki zarfı alır ve 23.39'da uzaklaşır. Parayı ertesi sabah 07.14'te bankamatikten kendi hesabına yatırır (7.800 TL).

Selçuk ilk iki ifadesinde borcu ve parayı saklar; parayı "taksi kazancı" diye anlatır ve saldıranı tanımadığını söyler. **Yalan söylüyor ≠ suçlu:** o mağdurdur, sakladığı şey kayıt dışı borçtur.

## İki eylem, iki sorumluluk

Vaka 001 tek suç → tek failken, Vaka 002 **tek olay → iki ayrı eylem → iki ayrı sorumluluk** kurar: yaralayan Emre Koç, parayı alan Kerem Şahin. Sonuç raporu bu yüzden dört sütunludur; dördüncü sütun (`custody`, "8.000 TL'yi kim aldı?") motorun isteğe bağlı özelliğidir ve `Docs/Architecture.md` → "Rapor sütunları" bölümünde anlatılır.

## Kaynakların açılma sırası

Hiçbir yerde "şimdi şununla konuş" ya da "kaydı kontrol et" denmez; her kaynağı bir ifade **erişilebilir** kılar, oyuncu masadan kendisi açar.

1. Olay tespit tutanağı.
2. Selçuk 1 — son yolcu sorusu sefer kaydından adı bulunabileceğini söyler.
3. Deniz Arslan — ayrılış saatini verir; sokakta kayıt tutan yer sorusunda büfe kamerasından söz eder.
4. KAMERA 04 kayıt dökümü — dokuz an: 22.58, 23.02, 23.03, 23.18, 23.20, 23.31, 23.34, 23.39, 23.46. Metin dökümüdür; bu vakada görüntü yoktur.
5. Selçuk 2 — araca yaklaşan kişiyi hatırlamadığını söyler, araçta ek inceleme yapılıp yapılmadığını sorar.
6. Olay yeri ek inceleme raporu — araçta buruşuk not: `EMRE K. / 8.000 / 21/11`.
7. Emre 1 — borcu kabul eder, tartışmayı kabul eder, temas etmediğini söyler.
8. Adli muayene ön raporu — yaralanma sert yüzeye çarpma ile uyumlu, yumruk için beklenen bulgu yok; sol ön kolda yüzeysel kavrama izleri. **Bulgular tek başına mekanizmayı kanıtlamaz.**
9. Emre 2 — ittiğini kabul eder, parayı almadığını söyler; çıkarken büfe önünde bir genç gördüğünü söyler.
10. Kerem 1 — uzaktan baktığını, yaklaşmadığını söyler.
11. Selçuk 3 — Emre'yi tanıdığını, parayı onun getirdiğini kabul eder; aracın kapısına eğilen büfedeki çocuğu hatırlar.
12. Kerem 2 — kayıt ve ifade karşısında çantadan zarfı aldığını söyler.
13. Mali tespit ve temas bulgusu — 7.800 TL nakit yatırma ve çanta üzerindeki temas örneği.

## Rapor

| Sütun | Doğru | Dayanak |
| --- | --- | --- |
| Yaralanmaya kim neden oldu | Emre Koç | Emre 2, adli muayene |
| Nasıl | Tartışma sırasında itilme ve sert zemine düşme | Adli muayene, Emre 2 |
| 8.000 TL'yi kim aldı | Kerem Şahin | Mali tespit, Kerem 2 |
| Kritik maddi tespit | KAMERA 04 kayıt dökümü | 23.20 / 23.31 / 23.34 / 23.39 anları |

Yanlış kişi yazmak — fail sütununda da para sütununda da — asılsız suçlamadır; doğru kişiyi kaynaksız yazmak eksik rapordur. Doğrulayıcı iki sorumluluğun aynı kişiye çıkmamasını da şart koşar (`Case002Rules`).

## Üretim notu

Yeni mekanik yok: Dosya #001'in sistemleri (dosya → görüşme → CCTV metin dökümü → gelen evrak → yeniden görüşme → yeni evrak → sonuç raporu) yeniden kullanılır. Yeni olan tek şey soruşturmanın yapısıdır. Selçuk, Deniz, Emre ve Kerem'in portreleri 26 Eylül 2026'da kullanıcıdan geldi ve `Resources/Bube/Characters/` altına girdi; veri içindeki portre renkleri yalnız yedek olarak duruyor.

## 9 Ekim 2026 değişikliği

- Büfe kamerasında 23.20–23.30 sinyal boşluğu; temas olayı yalnız ilk iki kare (iki kişi karşı karşıya). Düşüş anı kayıtta yok; yöntem adli muayene + Emre'nin ikinci ifadesiyle çözülür.
- 23.19: sokağın üst ucunda biri yeniden görünür (`passenger_back`, yalnız Deniz'e sunulabilir). Deniz'in yeni sorusu `case002.deniz.return`: kulaklığını unuttuğunu, araca yaklaşmadığını söyler.
- Çözüm değişmedi: fail Emre (itme), parayı alan Kerem.
