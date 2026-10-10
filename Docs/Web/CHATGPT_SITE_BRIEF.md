# ChatGPT'ye site tasarımı — nasıl kullanılır

ChatGPT'ye iki şey verirsin: aşağıdaki **brif metni** ve `Docs/Web/gorseller/` klasöründeki görseller.
Sayfa metinleri ayrı dosyalarda (`karine.md`, `karine-gizlilik.md`, `karine-kosullar.md`, `karine-hesap-silme.md`).

**Önerilen sıra (UI tasarımında yaptığımız gibi, parça parça):**
1. Brifi ve görselleri gönder, önce yalnız **tasarım panosu** iste: renkler, yazı tipleri, düğme, kart, üst menü, alt bilgi.
2. Panoyu beğenince **oyun sayfası** (`/karine`) — `karine.md`'nin metnini yapıştır.
3. Sonra **yasal sayfa şablonu** — tek şablon üç sayfaya (gizlilik, koşullar, hesap silme) yeter; `karine-gizlilik.md`'yi örnek ver.
4. En son **ana sayfa** (`bubegames.com`): şimdilik tek oyun, Karine kartı.

Görsel klasörü (`gorseller/`, depoya girmez; asılları oyunun `Resources` klasöründe):

| Dosya | Ne |
|---|---|
| `01-logo-karine.png` | Oyun logosu (kırmızı I'lı KARINE) |
| `02-logo-bubegames.png` | Stüdyo logosu |
| `03-ana-menu-gece.png` | Kahraman görsel: Bora masada, arkada İstanbul gecesi |
| `04-masa.png` | Oyunun masası (oyun ekranının zemini) |
| `05-bora.jpg` | Dedektif Bora portresi |
| `06-kapak-dosya001.jpg`, `07-kapak-dosya002.jpg` | Dosya kapakları |
| `08-cctv-kare.jpg` | Kamera karesi (oyundaki kanıt türlerinden) |
| `09-ulke-kartpostallari.png` | 10 ülke, gece silüetleri |
| `10-dunya-turkiye.jpg`, `11-dunya-japonya.jpg` | Ülke arka planları |
| `12-dosya-kagidi.png` | Dosya kâğıdı dokusu |

Ayrıca oyundan **3–4 ekran görüntüsü** ekle (Unity Game penceresinde 16:9): masa, dosya sayfası, sorgu, rapor/faks. Bunlar "oyun içi" bölümünde kullanılır.

---

## Brif metni (kopyala → ChatGPT'ye yapıştır)

> **Proje:** Bube Games stüdyosunun web sitesi, ilk oyunu **KARINE** için. Adres `bubegames.com`; oyun sayfası `bubegames.com/karine`; yasal sayfalar `bubegames.com/karine/gizlilik`, `/karine/kosullar`, `/karine/hesap-silme`. Site Squarespace'te kurulacak, ileride başka bir sunucuya taşınacak; o yüzden tasarım sade HTML/CSS ile yeniden yapılabilir olmalı, ağır animasyon ya da özel eklenti gerektirmemeli.
>
> **Oyun:** KARINE, iOS ve Android için oyuncu güdümlü bir dedektif oyunu. Oyuncu, gece bir masada dosyaları açar; tutanakları, mesaj dökümlerini, kamera karelerini ve ifadeleri karşılaştırır, tanıkları sorgular ve gerekçeli rapor yazar. Kimse sıradaki adımı söylemez, kimin yalan söylediği işaretlenmez. Kariyer İstanbul'da başlar, dünyanın 10 şehrine uzanır. Dedektifin adı Bora.
>
> **Ton:** noir, gece, sıcak lamba ışığı, yağmur, kâğıt ve dosya. Ciddi ama sinematik. Mobil oyun "pop" estetiği **yok**: parlak renkler, zıplayan düğmeler, çizgi film yok. Görseller piksel-sanat dokulu, karanlık ve sıcak ışıklı.
>
> **Renkler (oyunun kendi paleti):**
> - Zemin `#0E0F11`, panel `#1A1D22`, yükseltilmiş panel `#252A32`, kenar `#3A4048`
> - Ana metin krem `#E6E1D3`, ikincil metin gri `#9AA0A6`
> - Vurgu **kehribar** `#D99A2B` (seçili sekme, ana düğme, ince çizgiler)
> - Uyarı kırmızısı `#C64040` yalnız uyarı için; logodaki kırmızı I dışında dekor olarak kırmızı yok
> - Kâğıt katmanı (yasal metin kartı olabilir): kâğıt `#E8DCC4` üstünde koyu mürekkep
>
> **Yazı tipleri (Google Fonts):** başlıklar **Bebas Neue** (büyük harf, geniş harf aralığı); gövde **Chakra Petch** 400/600/700. Logo görsel olarak kullanılır, yazıyla taklit edilmez.
>
> **Bileşen dili:** köşeler az yuvarlak (4–6 px), ince 1 px kenarlar, düz koyu paneller, ana düğme kehribar zemin + koyu yazı, ikincil düğme koyu zemin + krem kenar. Hafif CRT/tarama çizgisi ya da film tanesi dokusu olabilir ama okunurluğu bozmasın. Geçişler yalnız yumuşak solma.
>
> **Oyun sayfası bölümleri:**
> 1. Kahraman alanı: `03-ana-menu-gece.png` arka planda, KARINE logosu, tek cümle tanıtım, App Store ve Google Play düğmeleri (şimdilik "Yakında").
> 2. "Nasıl oynanır": dört adım — Dosyayı aç · Karşılaştır · Sorgula · Raporla — her biri bir ikon ve kısa metin; masa ve dosya kâğıdı dokusuyla.
> 3. Oyun içinden: 3–4 ekran görüntüsü, telefon çerçevesinde ya da sade kartta.
> 4. Dünya: `09-ulke-kartpostallari.png` ile "İstanbul'dan dünyaya" şeridi.
> 5. Dedektif: Bora portresi ve iki cümle.
> 6. Özellikler listesi.
> 7. Alt bilgi: Bube Games logosu, Gizlilik · Kullanım Koşulları · Hesap silme · support@bubegames.com, © Bube Games, "Karine bir kurgudur" notu.
>
> **Yasal sayfalar:** uzun metin okumaya uygun tek sütun (en fazla ~720 px), üstte TR/EN dil düğmesi, başlıkta küçük KARINE logosu ve oyun sayfasına geri bağlantı. Tablolar okunur olsun.
>
> **Teknik:** önce mobil (telefon genişliğinde yatay kaydırma yok), açık/koyu tema gerekmez (site hep koyu), metinler Türkçe ve İngilizce.
>
> İlk istek: yalnız **tasarım panosu** üret — renk kutuları, yazı tipi örnekleri, ana/ikincil düğme, kart, üst menü, alt bilgi ve bir örnek bölüm başlığı. Sayfaları sonra tek tek isteyeceğim.

---

## Dikkat

- **Gerçek kurum adı/logosu yok:** ChatGPT bir polis, emniyet, mahkeme ya da gerçek şirket adı/amblemi eklerse çıkarılmalı. Oyun kurgudur.
- **Mağaza rozetleri:** gerçek App Store / Google Play rozetleri Apple ve Google'ın resmi dosyalarından alınır; ChatGPT'nin çizdiği rozet kullanılmaz.
- **Yazı tipi lisansı:** Bebas Neue ve Chakra Petch açık lisanslı (OFL), sitede kullanılabilir.
