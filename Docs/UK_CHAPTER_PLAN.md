# Birleşik Krallık bölümü — plan (taslak, 5 Ekim 2026)

**Kullanıcı kararları (5 Ekim 2026):** Bora bube **Londra şubesine tayin** edilir — aynı kurum, yerel şube; kariyer, rütbe ve kurum güveni Türkiye'den devam eder. Oyun metni Türkçe kalır; kişi, sokak ve mekân adları İngiliz, kurumlar **kurmacadır** (gerçek polis birimi, mahkeme, şirket, marka yok). Vakalar **bağımsızdır**; #020 bölüm finalidir ve Almanya'ya geçişi açar. #011–#015 onaylandı ve veriye girdi (5 Ekim 2026). **Ton kararı:** her dosya üst üste binen sürprizlerle büyür; bir kayıt yeni bir katman açar (bkz. #012).

**Sayı:** ülke başına on dosya (5 Ekim 2026 kullanıcı kararı).

## Yer ve ton
Şube: **bube Londra — Southwark**, Thames'in güney kıyısında eski bir depo binası. Zaman Mart 2029'dan başlar (Türkiye finali Ocak 2029). Ton: sis, yağmur, gece otobüsleri, rıhtımlar, pub kapanış saati. Masa aynı; pencere Londra kartpostalını gösterir.

## Açılış
Dünya 2 açılış sinematiği (`worldIntros`): Bora'nın Londra'ya varışı, şubeye ilk giriş, masaya ilk dosyanın bırakılması. Kare/video prompt'ları ayrı iş; video yoksa oyuncu takılmaz (mevcut kural).

## On dosya (tek satırlık konular — değişebilir)
| # | Ad | Tür | Öne çıkan yetenek |
|---|---|---|---|
| 011 | Sis Altında | rıhtımda ölüm | **İlk memur notu ile kayıt dökümü karşılaştırması** — özet bir kelimeyi çarpıtır |
| 012 | Son Sefer Değil | gece otobüsünde saldırı | Toplu taşıma kartı giriş/çıkışlarından zaman çizelgesi |
| 013 | Kiracı | paylaşımlı evde karbonmonoksit | Bakım/servis kayıtları; kaza ile ihmal ayrımı |
| 014 | İkinci Görüş | müzayedede sahte tablo | Çelişen uzman raporları; ikinci uzman talebi |
| 015 | Kapanış Saati | pub önünde kavga, ölüm | Çok tanık; kim nereden ne görebilirdi (görüş hattı) |
| 016 | Daktilo | Hampstead'de kayıp yazar | El yazısı / daktilo karşılaştırması |
| 017 | Başkasının Adı | kimlik dolandırıcılığı | Para izi + kimlik belgeleri |
| 018 | Gece Vardiyası | hastanede ilaç eksikliği ve ölüm | Sayım çizelgeleri, vardiya kayıtları |
| 019 | Manşet | danışmanın ölümü, basın sızıntısı | Zamanla değişen haber baskıları |
| 020 | Köprü | **bölüm finali** | Önceki yeteneklerin birleşimi; final sinematiği, DOSYA 021 Almanya |

Her dosya kendi içinde kapanır; ortak örgüt yok. Adlar Türkiye bölümündekilere benzemez. CCTV yalnız seyri değiştiren olaylara, olay başına üç kare.

## Teknik işler (vakadan bağımsız)
- Dünya geçişi: #010 faksı → final sinematiği → UK açılışı → #011 teklifi; kilit `Worlds.json`'dan.
- `Worlds.json` UK yuvalarına `case011…` bağlanması; UK kartpostalı, bayrak, pencere görseli.
- Doğrulayıcı: `Case011Rules` (vaka başına kural dosyası, mevcut düzen).

## Görsel üretimi (5 Ekim 2026 kullanıcı kararı)
- **ChatGPT:** CCTV kareleri ve bölüm/dosya kapakları.
- **Gemini:** portreler ve oyun içi adli bulgu görselleri (delil fotoğrafları vb.).
