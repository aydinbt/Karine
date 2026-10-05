# bube — dünya açılış sinematikleri

**26 Eylül 2026 kullanıcı kararı.** Oyun **on ülke/dünya** için planlanır ve her dünya **on dosya** taşır — toplam 100 (5 Ekim 2026 kullanıcı kararı; önce yedi/70 idi). Bu sayı, 24 Eylül 2026'daki yedi dünya kararının yerine geçer; bölüm seçici ekranı birebir bu yerleşimle istendi ve kanon ona göre güncellendi. Dünya 1 Türkiye'dir; 2–10 arasındaki ülkeler artık **adlandırıldı** (aşağıdaki çizelge), ama mekânları, vakaları ve açılışları henüz yazılmadı.

**Sıra artık ilerleme kuralıdır.** Önceki not sırayı bağlayıcı saymıyordu; bölüm seçici geldikten sonra sayıyor: bir ülke, kendinden önceki ülkenin on dosyası kapanmadan açılmaz ve dosyalar ülke içinde sırayla açılır. Kilit yalnız ilerlemeyi gösterir — hiçbir yerde sıradaki adım söylenmez, fail veya ipucu verilmez. Teknik karşılığı `Docs/Architecture.md` → "Bölüm seçici verisi"; veri `Assets/Bube/Resources/Bube/Worlds.json`.

**Ülke listesi kanon, içeriği değil.** Aşağıdaki on ülke seçicide görünür ve kilit sırasını belirler; her birinin şehri, atmosferi ve on dosyası ayrı yazılacak iştir. Bir ülkenin yeri değiştirilecekse tek düzenleme `Worlds.json` sırasıdır, ama o zaman bu belge de aynı oturumda güncellenir.

## Dünya 1 / Türkiye

Yeni kariyer ilk başladığında ve yalnız bu kariyerde bir kez yaklaşık 10 saniyelik yatay POV açılışı oynar. Kamera Bora'nın gözüdür; dışarıdan Bora'nın yüzü gösterilmez. Karakol, İstanbul manzarası ve belirgin Türk bayrağı görünür. `bubeGames` / `powered by bubeDigital` yazısı videonun içindedir. Bu klipte oyun katmanı ikinci kez stüdyo yazısı veya yer etiketi çizmez. Kararmanın ardından masaya geçilir, ilk dosya Bora'nın önüne bırakılır ve Dosya #001 kabul ekranı gelir. Oyuncu sinematiği geçebilir.

Çıkış dosyası `Assets/StreamingAssets/Bube/world01_intro.mp4`, kullanıcının seçtiği `Create_a_complete_second_op.mp4` adlı Gemini üretiminin ses ve görüntü akışı korunmuş proje kopyasıdır. Sağ alttaki Gemini simgesi video piksellerinden silinmez; `Geç` düğmesi videonun ölçeklenmesine göre bu bölgenin üstüne yerleştirilir. Kopya 1280×720 H.264/AAC ve AVFoundation tarafından tanınan BT.709 renk bilgisi taşır. Oyun oynatıcısının ses kanalı etkindir. Unity derleme/içerik doğrulaması geçti; Play Mode ve mobil cihazda ses, görüntü ve `Geç` düğmesinin filigranı örtmesi kontrolü açık iştir. Önceki Higgsfield klibi ve yerel prototip proje açılışında artık kullanılmaz.

## Tekrarlanabilir yapı

`config.json` içindeki `worldIntros` her dünyanın ilk vaka kimliğini, ülke metin anahtarını, video dosyasını ve görsel yazıların videonun içine gömülü olup olmadığını taşır. Kariyer kaydı gösterilen dünya açılışlarını saklar. Yeni kariyer bu kaydı sıfırlar; menüye, eski dosyaya veya aynı dünyadaki başka vakaya her dönüşte sinematik tekrarlanmaz. Yeni bir dünyanın ilk dosyası açılırken kendi sinematiği bir kez oynar; ardından o dünyanın dosya kabul ekranına geçilir. Video erişilemezse oyuncu takılmaz, görev açılır.

| Dünya | Ülke | Şehir | Dosya | Açılış |
| --- | --- | --- | --- | --- |
| 1 | Türkiye | İstanbul | 10 | Bora'nın Beşiktaş şubesine ilk gelişi — ilk sürüm mevcut |
| 2 | Birleşik Krallık | Londra | 10 | Ayrı açılış gerekir; plan `UK_CHAPTER_PLAN.md` |
| 3 | Almanya | Berlin | 10 | Ayrı açılış gerekir |
| 4 | Japonya | Tokyo | 10 | Ayrı açılış gerekir |
| 5 | Fransa | Paris | 10 | Ayrı açılış gerekir |
| 6 | ABD | Chicago | 10 | Ayrı açılış gerekir |
| 7 | İtalya | Napoli | 10 | Ayrı açılış gerekir |
| 8 | İspanya | Sevilla | 10 | Ayrı açılış gerekir |
| 9 | Kanada | Montreal | 10 | Ayrı açılış gerekir |
| 10 | Avustralya | Melbourne | 10 | Ayrı açılış gerekir |

Şehirler bölüm seçicinin ülke kartında görünür ve `Worlds.json` ile aynıdır; Türkiye dışındakiler ilk taslaktır, o ülkenin vakaları yazılırken değişebilir. Dosya sayısı on ülkede de ondur: seçicinin ilerleme çubuğu ve `n / 100` sayacı aynı ölçeği gösterir, doğrulayıcı eşitliği kilitler.

Her yeni açılış, o ülkenin yerel atmosferini taşımalı; Bora'yı ve bube kimliğini tutarlı korumalı. Bu açılışlar soruşturma kaynağı veya oyuncuya ipucu veren sahneler değildir.
