# bube — Bora ve uzun vadeli kariyer yapısı

Faks sonrası kariyer ve İstatistikler ekranının ayrıntılı tasarım kaydı: [CAREER_AND_STATISTICS_DESIGN.md](CAREER_AND_STATISTICS_DESIGN.md). Bu kayıt uygulama tamamlandı anlamına gelmez.

**Kaynak ve statü:** “Mobil Dedektif Oyunu Fikri” konuşmasının kariyer, süreklilik ve ana karakter bölümleri. Tam metin `Archive/CONVERSATION_TEXT_1.md`–`4.md` içindedir. Bu dosya hangi unsurun kullanıcı tarafından benimsendiğini, hangisinin yalnızca örnek olduğunu ayırır. Ana ürün sözleşmesi `BUBE_GAME_MASTER_CONTEXT.md` içindedir.

## Bora: kilitlenen profil

- Ana karakteri oyun belirler; oyuncu ad, cinsiyet veya kişilik seçmez. Oyuncu **Bora** olarak soruşturmayı yürütür. Alex, Emir ve özelleştirilebilir self-insert önerileri daha sonra elendi.
- Bora 27 yaşında, erkek, Türk; İstanbul/Beşiktaş'ta doğup büyüdü. bube Polis İstanbul soruşturma birimine **yeni atanmış**, fakat polislikte birkaç yıllık deneyimi var. Bu, ilk dosyada temel meslek bilgisini bilmesini ve soruşturmacı olarak hâlâ gelişmesini açıklar.
- Bora'nın kişiliği sabit: sakin, gözlemci, ayrıntı ve kayıt odaklı. İnsanlara doğrudan “yalan söylüyorsun” diye saldırmak yerine söyledikleriyle kayıtları karşı karşıya koyar. Oyuncu Bora'nın kişiliğini değil, **hangi bilgiyi ne zaman incelediğini, hangi soruyu sorduğunu ve hangi sonuca ulaştığını** kontrol eder.
- Başlangıçta kısa bir personel kartı/biyografisi oyuncuya kim olduğunu ve neden masada olduğunu anlatır. Uzun zorunlu geçmiş anlatısı, soyadı, aile/eğitim geçmişi ve neden polis olduğu şimdilik belirlenmedi; ancak vaka anlatısına hizmet ederse ortaya çıkar. “Biyografiyi hiç koymayalım” erken önerisi kullanıcı düzeltmesiyle geçersiz oldu.
- Bora'nın sprite'ı gerektiğinde açılış, personel/kariyer ekranı veya özel sahnede gösterilebilir. Her dosya ekranına portre eklemek gerekli değil. Görsel dil minimalist pixel-art.
- Yanlış sonuca ulaşabilen kişi oyuncudur; Bora'nın kanonik hikâyesi ve gerçek fail değişmez. Gönderilen yanlış rapor da dosyayı kapatır; kurumsal sonuç daha sonra faksla bildirilir.

### Kısa biyografi için kabul edilen içerik yönü

> Bora — 27 · İstanbul/Beşiktaş. Birkaç yıllık polislik deneyiminin ardından bube Polis soruşturma birimine atandı. Sakin ve gözlemci; ifadeleri kayıtlara ve kanıtlara karşı kontrol eder. Yeni görevi, soruşturmacılık kariyerinin başlangıcıdır.

Bu örnek nihai UI metni değildir; oyuncuya gereken yoğunluğu gösterir. Soyadı ve daha ayrıntılı geçmiş açık kalır.

## Kariyer: kabul edilen omurga

Oyuncu yalnızca dosya numarası veya XP biriktirmez; **Bora'nın soruşturmacı olarak geliştiğini ve departmanın ona giderek daha karmaşık sorumluluklar emanet ettiğini** hisseder. Bir vaka bittiğinde yalnızca “sonraki bölüm” açılmamalı: dosya kariyer geçmişine yazılır, yaptığı muhakeme ve kanıt kullanımı profesyonel sonuç doğurur. Bu, kullanıcıyı oyunda tutacak sürekliliğin temelidir.

- Kariyer gelişimi **ülke sırası** değildir. Ülke/şehir vakaların coğrafyası ve atmosferidir. Eski 10 ülke × 10 vaka / 100 vaka örnekleri kabul edilmedi; 24 Eylül 2026 tarihli yeni kullanıcı kararıyla **toplam yedi ülke/dünya** hedefi belirlendi. Dünya 1 Türkiye; diğer ülkeler ve geçiş sırası henüz seçilmedi. Her dünyanın ilk kariyer girişinde ayrı bir sinematik gerekir. Ayrıntı: [WORLD_OPENINGS.md](WORLD_OPENINGS.md).
- Unvan/rol ve departman güveni farklı kavramlardır. Unvan, Bora'nın kalıcı mesleki basamağını/sorumluluğunu gösterir; güven, son kararlarının departmanda yarattığı güncel itimadı gösterebilir. Birkaç kötü kararda otomatik alt rütbeye düşmesi amaçlanmadı. Güvenin vakalara etkisi ve sayısal formülü kesinleştirilmedi.
- Başarı sadece “doğru faili buldu” değildir. Soruşturma kalitesi, kanıtları fark etme, ifadeleri karşılaştırma, yetersiz kanıtla suçlamadan kaçınma ve masum kişilere yaklaşım profesyonellik hissini oluşturur. Yanlış soru denemesi ile yanlış suçlama aynı ağırlıkta cezalandırılmaz. Vaka sonu değerlendirme bir amir/dosya incelemesi gibi olabilir; ekranda sürekli XP yağmuru olmamalı.
- Terfi, daha zor dosya ve uygun yeni soruşturma imkanlarına erişim sağlayabilir. Önceden edinilmiş imkanlar kaybolmaz. **Yetki mevcut** olmak, **her vakada o veri mevcut** demek değildir: kamera erişimi olan Bora, kamerasız binadan kayıt çıkaramaz. Sonradan edinilen araçlar her vakada kullanılmaya zorlanmaz.
- İlk İstanbul vakasında CCTV vardır; daha önceki “CCTV birkaçıncı bölümde veya terfide açılır” örneği sonraki kullanıcı düzeltmesi ve Dosya #001 kararıyla geçersizdir. Ülke değiştirince eski yeteneklerin sıfırlanması da istenmedi.
- Suçun ağırlığı doğrudan zorluk derecesi değildir. Küçük hırsızlık karmaşık olabilir; ciddi olay az ama net kanıt içerebilir. Zorluk, bilgi kaynaklarının güvenilirliği, çelişkiler ve bağlantıların yapısından doğar.
- Oyuncunun eski dosyalara ve ifadelerine dönebilmesi süreklilik için değerlidir. Kariyer dosyası/arsivi, çözülmüş vakaları ve oyuncunun kendi sonuçlarını kalıcı biçimde tutan bir yön olarak konuşuldu. Vakalar arası tekrarlayan izler, yeniden açılan ve çözülemeyen vakalar **gelecek fikirleri** olarak ele alındı; ilk teslim veya sabit kapsam değildir.
- Hatalar sonuç doğurabilir ama yeni oyuncuyu oyundan soğutan kalıcı çıkmaz yaratmamalıdır. İlk dosyada rapor kesindir; başarısızlık kurumsal faks sonrası birim güvenini düşürür. Sonraki vakalarda güven, ek sorumluluk ve yanlış karar etkisi ayrıca dengelenecek.

## Kesinleşmemiş, örnek olarak konuşulan yapı

Konuşmada `Probationary Investigator → Criminal Investigator → Senior Investigator → Lead Investigator` gibi ünvanlar ve ek `Investigator II`, `Supervisory Investigator` önerileri vardı. **Kesin rütbe listesi, eşikleri ve terfi sayıları kilitlenmedi.** Bora'nın “soruşturma birimine yeni geçişi” kilitlendi. Gerçek teşkilatların rütbeleri birebir kopyalanmayacak; bube evreni kurgusal ve tutarlı olacak.

`Investigation / Reasoning / Interviewing / Judgement / Integrity` gibi değerlendirme boyutları, sayısal departman güveni dengesinin nihai değerleri, yüksek profilli dosya eşiği, ayrıntılı vaka sonu performans raporu, birbiriyle bağlantılı uzun hikâye, günlük olay, yeniden açılan dosya ve çözülemeyen dosyalar **tasarım yönü/öneri** düzeyindedir. Bunlar potansiyel sistemlerdir; Dosya #001'i yapmak için zorunlu bağımlılık oluşturmaz. Özellikle bir zamanlar önerilen uluslararası merkez teşkilat ve ülke bazlı açılma, daha sonra kabul edilen **yerel bube Police şubeleri** ve ülkenin progression kapısı olmaması kararıyla değiştirilmiştir.

## Üretim ve veri mimarisi için sonuç

Kariyer kayıtları sonradan eklenecek olsa bile vaka ve kayıt kimlikleri kalıcı olmalı. Bir vaka tamamlanınca oyuncunun seçtiği rapor ile vaka durumu saklanabilmeli; dosyaya sonra bakılabilmeli. Olası kariyer sistemi vaka verisini, yetkiyi ve vakadaki mevcut kaynağı ayrı tanımlamalı. Yeni vaka hazırlama, yeni temel kod yazmayı gerektirmemeli. İlk öncelik yine **Dosya #001'in tam, serbest ve doğrulanmış oynanabilir döngüsü**dür; kariyer sisteminin sayısal kuralları bundan sonra kararlaştırılır.

## 23 Eylül 2026 kesin rapor akışı güncellemesi

Kullanıcının sonraki kararı, yukarıdaki ilk vaka için “yanlış rapor iade edilir” önerisini geçersiz kılar. Bora emin olduğu raporu gönderir ve dosya kapanır. Bölüm tamamlandı ekranı kendi iddiasını özetler, doğruluğu söylemez. Kurumsal faks Gelen Evraklar'a daha sonra gelir; doğru kararda birim güveni artar, yanlış kararda azalır. Sürekli başarısızlık güveni sıfıra indirebilir ve Bora'nın soruşturmacı kariyerini bitirir. İlk kod dengesi başlangıç güveni 60, +5/−15, emeklilik eşiği 0'dır; bunlar oynanış testinde ayarlanacak değerlerdir.
