# bube — dünya açılış sinematikleri

**24 Eylül 2026 kullanıcı kararı.** Oyun yedi ülke/dünya için planlanır. Bu sayı önceki belirsiz ülke sayısı notlarının yerine geçer. Dünya 1 Türkiye'dir; Dünya 2 ve sonraki ülkeler henüz seçilmedi. Ülkelerin sırası veya vaka sayısı kariyerin sabit ilerleme kuralı olarak buradan çıkarılmaz.

## Dünya 1 / Türkiye

Yeni kariyer ilk başladığında ve yalnız bu kariyerde bir kez yaklaşık 10 saniyelik yatay POV açılışı oynar. Kamera Bora'nın gözüdür; dışarıdan Bora'nın yüzü gösterilmez. Karakol, İstanbul manzarası ve belirgin Türk bayrağı görünür. `bubeGames` / `powered by bubeDigital` yazısı videonun içindedir. Bu klipte oyun katmanı ikinci kez stüdyo yazısı veya yer etiketi çizmez. Kararmanın ardından masaya geçilir, ilk dosya Bora'nın önüne bırakılır ve Dosya #001 kabul ekranı gelir. Oyuncu sinematiği geçebilir.

Çıkış dosyası `Assets/StreamingAssets/Bube/world01_intro.mp4`, kullanıcının seçtiği `Create_a_complete_second_op.mp4` adlı Gemini üretiminin ses ve görüntü akışı korunmuş proje kopyasıdır. Sağ alttaki Gemini simgesi video piksellerinden silinmez; `Geç` düğmesi videonun ölçeklenmesine göre bu bölgenin üstüne yerleştirilir. Kopya 1280×720 H.264/AAC ve AVFoundation tarafından tanınan BT.709 renk bilgisi taşır. Oyun oynatıcısının ses kanalı etkindir. Unity derleme/içerik doğrulaması geçti; Play Mode ve mobil cihazda ses, görüntü ve `Geç` düğmesinin filigranı örtmesi kontrolü açık iştir. Önceki Higgsfield klibi ve yerel prototip proje açılışında artık kullanılmaz.

## Tekrarlanabilir yapı

`config.json` içindeki `worldIntros` her dünyanın ilk vaka kimliğini, ülke metin anahtarını, video dosyasını ve görsel yazıların videonun içine gömülü olup olmadığını taşır. Kariyer kaydı gösterilen dünya açılışlarını saklar. Yeni kariyer bu kaydı sıfırlar; menüye, eski dosyaya veya aynı dünyadaki başka vakaya her dönüşte sinematik tekrarlanmaz. Yeni bir dünyanın ilk dosyası açılırken kendi sinematiği bir kez oynar; ardından o dünyanın dosya kabul ekranına geçilir. Video erişilemezse oyuncu takılmaz, görev açılır.

| Dünya | Ülke | Açılış |
| --- | --- | --- |
| 1 | Türkiye | Bora'nın Beşiktaş şubesine ilk gelişi — ilk sürüm mevcut |
| 2 | Belirlenmedi | Ülke ve mekân kararı sonrası ayrı sinematik/animasyon gerekir |
| 3 | Belirlenmedi | Ayrı açılış gerekir |
| 4 | Belirlenmedi | Ayrı açılış gerekir |
| 5 | Belirlenmedi | Ayrı açılış gerekir |
| 6 | Belirlenmedi | Ayrı açılış gerekir |
| 7 | Belirlenmedi | Ayrı açılış gerekir |

Her yeni açılış, o ülkenin yerel atmosferini taşımalı; Bora'yı ve bube kimliğini tutarlı korumalı. Bu açılışlar soruşturma kaynağı veya oyuncuya ipucu veren sahneler değildir.
