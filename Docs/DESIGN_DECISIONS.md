# bube — önceki sohbetten doğrulanan tasarım kararları

**Kaynak:** “Mobil Dedektif Oyunu Fikri” sohbeti (`6ab2f01e-3420-83eb-a7d9-02f0adbb0d91`).
**Ana kayıt:** `BUBE_GAME_MASTER_CONTEXT.md`. Bu kısa dosyada eksik kalan veya eski prototipe ait ifadelerde ana kayıt geçerlidir.
**Durum:** 23 Eylül 2026. Bu belge, konuşmadaki son kararları önceki önerilerden ayırır. Oynanabilir prototipin mevcut davranışı ayrı belirtilir.

## Oyun ve sunum

- Çalışma adı **bube**; geliştirici adı bubeGames. Oyun içi kurum **bube Police / bube Polis**, terminal **BPS — bube Police System**. Nihai oyun adı daha sonra seçilecek; adlar yapılandırmadan değiştirilebilir olmalı.
- İlk oyuncu dili Türkçe. Kod anahtarları İngilizce olabilir; oyuncuya görünen metinler yerelleştirme verisinde tutulmalı.
- Oynanabilir karakter Bora: 27 yaşında, erkek, Türk, İstanbul/Beşiktaşlı; bube Police soruşturma biriminde yeni. Oyuncunun değiştirdiği bir avatar değil.
- Mobil yatay ekran; sabit masa ve belge/görüşme/terminal görünümleri. Serbest yürüyüş veya joystick yok. Görsel yön 2D/pixel art; masa nesneleri ancak işlevleri varsa görünür. Karakter duygu veya iç durum etiketleri oyuncuya açıklanmaz. Görüşme ve dosya görsel kararları için `Docs/VISUAL_DIRECTION.md` ana kayıttır.
- Ana döngü: masa → dosya → görüşmeler → metin tabanlı CCTV/BPS → yeni belge ve takip görüşmeleri → gerekçeli sonuç → dosyayı kapat → masaya dön.

## Dosya #001 — Açık Kapı

**Tür:** Konut hırsızlığı. **Yer:** Beşiktaş, İstanbul. **Hedef süre:** 10–15 dakika. **Mağdur:** Mert Aydın. **Gerçek fail:** komşu Hasan Kaya. **Yanlış şüphe:** Elif Demir. Temel düşünce: Birinin yalan söylemesi suçlu olduğu anlamına gelmez.

### Yalnızca tasarım/uygulama ekibinin bildiği olaylar

| Saat | Gerçekte olan |
| --- | --- |
| 08.27 | Mert evden çıkar. |
| 11.48 | Elif kalan eşyalarını almak için eve gelir. |
| 12.16 | Elif ayrılır, anahtarı kapı yanındaki saksıya bırakır. |
| ~12.40 | Anahtarın yerini önceden bilen Hasan anahtarla girer. |
| ~12.50 | Hasan dizüstü bilgisayarı, saati ve nakit parayı alır. |
| ~13.00 | Hasan anahtarı da götürerek çıkar. |
| 17.54 | Mert eve döner, hırsızlığı fark eder. |

Bu çizelge oyuncuya doğrudan gösterilmez. Başlangıç olay raporunda zorlama izi olmadığı, kayıp eşyalar, kapı yanındaki saksı ve kayıp anahtar yer alır.

### Oyuncunun izleyeceği kanıt akışı

1. Mert'in ilk görüşmesi Elif ile Hasan'ı tanıtır. Oyuncu ikisiyle hangi sırada görüşeceğini seçebilir.
2. Elif o gün eve gitmediğini söyler. Hasan bir kadın gördüğünü ve bina kamerasını belirtir; kamera kaydı açılır.
3. CCTV **yalnızca metinsel kayıttır**: 08.27 erkek çıkar; 11.48 kadın girer; 12.16 kadın çıkar; 12.31 sinyal zayıflar; 12.37 sinyal gider; kayıt yoktur; 13.08 sinyal döner; 17.54 erkek girer. Kayıt Hasan'ı ya da hırsızlığı doğrudan göstermez ve oyuncu adına yalan/sonuç yorumu yapmaz.
4. Elif takip görüşmesinde eve gittiğini ve anahtarı saksıya bıraktığını, kuşkudan korktuğu için yalan söylediğini açıklar. Mert takip görüşmesinde Hasan'ın anahtar yerini bildiğini söyler. Hasan takip görüşmesinde anahtar konusunda kaçamak konuşur. Yeni bilgi yoksa tekrarlanan görüşme kısa bir “anlattım” yanıtı verir.
5. Ardından gelen **Eşya Tespit Raporu**, çalınan dizüstünün seri numarası eşleşmesini ve cihazı ikinci el elektronikçiye teslim eden kişinin Hasan olduğunu bildirir. Bu, yeni bir dükkân mekaniği değil, gelen polis belgesidir. Seri numarasının kendisi ve teslim saati konuşmada kesinleştirilmedi.
6. Sonuç raporunda oyuncu **şüpheliyi, giriş yöntemini ve belirleyici kanıtı** seçer. Doğru birleşim Hasan / yedek anahtar / Eşya Tespit Raporu. İlk dosyada yanlış veya yetersiz rapor “SONUÇ İADE EDİLDİ” geri bildirimiyle araştırmaya döner; fail otomatik açıklanmaz. Doğru rapordan sonra oyuncu dosyayı açık eylemle kapatıp masaya döner.

İlk dosyanın kapsamı dışında: telefon kayıtları, banka kayıtları, DNA/parmak izi laboratuvarı, otopsi, araç ve sosyal medya araştırması, 3D olay yeri, video/foto CCTV, gelişmiş sorgu, yeniden açılan dosya ve reklamla ipucu. Masadaki telefon varsa görüşme işlevini görür; telefon inceleme sistemi anlamına gelmez.

## Sonraki aşama kararları

- Vaka ilerleyişi çıkmaz yaratmamalı; yeni kanıtlar yeni soruları ve evrakları açmalı. Oyuncu her an önceki belge ve ifadelere dönebilir. Alt durumlar ve “doğru adım” okları doğrudan gösterilmez.
- Vaka #002, veri odaklı yapıyı doğrulamak için kod değiştirmeden eklenmeli. Dosya sayısı veya ülke sayısı için konuşmadaki örnek rakamlar kesin hedef değildir. CCTV ilk İstanbul vakasında vardır; ülke/kariyer kilidine bağlı değildir.
- Kariyer ve yetki genişlemesi gelecekteki sistemdir. Ödüllü reklamlar ileride yorumlamayı destekleyen ipuçları verebilir, faili söylemez; geçiş reklamları yalnızca doğal bölüm aralarında düşünülür. Dosya #001'de reklam ipucu yok.
- Gerçek polis logosu veya kurum kimliği nihai görselde kullanılmaz. Eski masa görseli yalnızca kavram referansıdır; üzerindeki eski semt/kurum işaretleri final varlık olarak kabul edilmez.

## Mevcut prototiple ayrışan noktalar

- CCTV metninin sinyal kesintisi ve Hasan'ı doğrudan göstermeme kararı vaka verisine işlendi. Ancak terminal hâlâ olayları satır satır tarama ve kaliteye bağlı belirsizlik olarak oynatmıyor.
- Elif/Mert/Hasan takip görüşmeleri, gelen Eşya Tespit Raporu ve üç alanlı sonuç prototipte var; görüşmeler hâlâ soru seçimi yerine hazır metin açıyor. Tekrar görüşme yanıtı da ayrı değil.
- Dosya hâlâ fiziksel, bağımsız sayfalar ve okunmadı bildirimleriyle tamamlanmadı.
- Arayüzde Türkçe kataloğun dışında sabit yazılmış oyuncu metinleri bulunuyor; yerelleştirme tamamlanmalı.

Bu ayrışmalar yol haritasında izlenir. Gizli olay çizelgesi oyuncu kaydıymış gibi gösterilmez.

## Görsel referansların erişimi

Masa görseli `Assets/Bube/Resources/Bube/DeskReference.png` içindedir. Kullanıcının bu oturumda paylaştığı görüşme ve dosya konseptleri `Docs/References/` klasörüne, kullanım kararları `Docs/VISUAL_DIRECTION.md` dosyasına eklendi. Daha eski sohbetin başka görselleri otomatik olarak dosya halinde aktarılmadı.
