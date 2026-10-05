# İtalya bölümü — Dosya #046–#052 (bube Napoli — Quartieri Spagnoli)

Yeni mekanik: **tanıklar beklemez** (`closesAfterRead`). Her dosyada bir görüşme, belli bir belge okunduğunda kapanır; kişi şehri ya da ülkeyi terk eder ve `closedNoteKey` notu görünür. Kapanan görüşme hiçbir zaman doğru sonucun zorunlu zincirinde değildir (doğrulayıcı `ValidatePressure` + lint): oyuncu onu kaçırırsa dosya yine çözülür, ama bir tanığın gözü kaybolur. Kapanan görüşme düğüm sırasında kapatan belgeden **önce** gelir; aksi halde otomatik yürüyüş onu hiç açamaz.

Kişi sayısı dosyadan dosyaya değişir: 3, 5, 7, 4, 6, 3, 5.

| # | Ad | Kişi | Katil | Kapanan tanık → kapatan belge | Caruso / E. V. ipi |
|---|---|---|---|---|---|
| 046 | Basso | 3 | Gennaro Russo (torun) | Pieter de Vries → otopsi | Kocanın sakladığı Caruso sevk fişleri; “C.M.” |
| 047 | Forno | 5 | Enzo Ferraro (ortak) | Moussa Diop → soğuk oda incelemesi | Caruso unu; “Un pazartesi gelsin” |
| 048 | Funicolare | 7 | Fabio Marino (bakım şefi) | Kofi Mensah → otopsi | Bakım bütçesi Caruso Danışmanlık’a |
| 049 | Presepe | 4 | Vittoria Sala (antikacı) | Bruno Gallo → toksikoloji | Sahte figürler Caruso gemileriyle Zürih/Tokyo/New York’a |
| 050 | Sipario | 6 | Nicola Costa (sahne amiri) | Kenji Arai → askı incelemesi | Çift dipli Caruso turne sandıkları |
| 051 | Procida | 3 | Gianni De Luca (kaptan) | Birgit Holm → kıç kamerası | Sahte römorkör belgeleri; “Lorenzo C.” |
| 052 | Caruso (final) | 5 | Lorenzo Conte (damat) | Ottavio Mele → rıhtım kamerası | Ofisteki 1989 fotoğrafı, eski erkek saati, “E. V.”; İspanya numarası |

Diğer araçlar: #047, #048, #051, #052 sinyal CCTV dökümü kullanır (her biri 3 kare). #047–#052 gerekçeli arama izniyle açılan bir arama belgesi taşır. #052: `reconstruction` 7 adım; `chapterFinale` it → es / `finale.file.053`; anahtarlar `finale.it.call.3/5`, `finale.it.personnelBody`, `finale.progress.it`, `finale.country.es`.

Birbirine bağlantı: her dosyanın sonu bir sonrakine bir isim bırakır — #046 “C.M.”, #047 un sevkiyatı, #048 Caruso Danışmanlık, #049 Tokyo/Zürih/New York alıcıları (Japonya, ABD bölümlerine geri bağ), #050 Tokyo turnesi sandıkları, #051 “Lorenzo C.”, #052 Lorenzo Conte ve cevapsız İspanya numarası (Sevilla’ya köprü). E. V. ipi dosyaları çözmek için hiçbir zaman gerekmez.
