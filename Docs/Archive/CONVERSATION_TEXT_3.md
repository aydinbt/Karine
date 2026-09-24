# Mobil Dedektif Oyunu Fikri — metin arşivi 3/4

Bu dosya önceki ChatGPT konuşmasının erişilebilen metin dökümüdür. Erken öneriler daha sonraki kararlarla değişmiş olabilir. Oyun kararları için BUBE_GAME_MASTER_CONTEXT.md geçerlidir. Görsel çıktılar okuma arayüzünde dosya olarak sunulmadığı için burada yer almaz.

## Tur 35

### Kullanıcı

bu çok iyi sonra cctv görüntüsünü yapar mısın

## Tur 36

### Kullanıcı

ama biz cctvyi böyle vermeyecektik hani?

## Tur 37

### Kullanıcı

hayır bizim konuştuğumuz kaydettiğin not aldığın şekilde değil ki

## Tur 38

### Kullanıcı

hani sadece yazıydı? hatırlamıyor musun yazarak cevap ver

### Asistan

Evet, haklısın. **CCTV'de hiçbir görüntü/video olmayacaktı.** Son görsellerde yanlış yöne gittim.

Konuştuğumuz ve not aldığımız sistem şuydu:

CCTV'ye tıklayınca masa arkada **blur** olacak ve ortada güvenlik sistemi arayüzü açılacak. Ama kamera görüntüsü yerine CCTV'nin teknik arayüzü olacak: `REC`, kamera numarası, tarih/saat, sinyal seviyesi, kanal, kayıt kalitesi, incelenen zaman aralığı, scanline/parazit gibi detaylar.

Ortadaki esas içerik ise **yalnızca metin** olacak ve kayıt inceleniyormuş gibi satırlar sırayla belirecek:

```text
● REC                                      CAM 01
BUILDING ENTRANCE                         CH 01

16.10.2026                             18:42:17

SIGNAL  ███████░
QUALITY  GOOD

──────────────────────────────────────────────

             REVIEWING ARCHIVE...

        REVIEWED PERIOD: 08:00 — 18:00

──────────────────────────────────────────────

08:27 — Male resident exits building.

11:48 — Female individual enters building.

12:16 — Same individual exits building.

17:54 — Male resident enters building.

──────────────────────────────────────────────

REVIEW COMPLETE

SIGNIFICANT EVENTS: 4
```

Ve bunlar baştan topluca görünmeyecek.

`REVIEW RECORDS` basılacak → tarama başlayacak → hafif elektronik/klavye sesiyle:

**tak tak tak...**

`08:27 — Male resident exits building.`

biraz bekleme/tarama...

**tak tak...**

`11:48 — Female individual enters building.`

şeklinde sırayla gelecek.

Sinyal grafiği hareket edebilir, saat ilerleyebilir, `REC` yanıp sönebilir, hafif CRT/scanline efekti olabilir. Böylece **ortada tek kare CCTV görüntüsü bile olmadan CCTV inceliyormuş hissi** vereceğiz.

En önemli sebebi de buydu: her vaka için CCTV sahnesi, karakter animasyonu ve video üretmeyeceğiz. Yeni bir CCTV kaydı bizim açımızdan esasen **birkaç satır veri** olacak; kaliteli UI + ses + mikro animasyon bunu pahalı bir sistemmiş gibi hissettirecek.

Yani doğru tasarımımız:

**CCTV görüntüsü ❌  
CCTV videosu ❌  
İnsan/ortam görseli ❌  
CCTV terminal arayüzü + sinyal/saat/REC + text event'ler ✅**

Bir sonraki görseli tam olarak **bu kurala göre**, önceki dosya ekranındaki gibi arkası blur masa ve ortada text-only CCTV terminali şeklinde yapmamız gerekiyor.

## Tur 39

### Kullanıcı

hatırlarsan da signal'a göre yazının glitch durum da değişecekti

### Asistan

Evet, o da sistemin önemli parçasıydı. **Signal/Quality sadece dekoratif bilgi olmayacaktı; CCTV'den elde edilen metnin güvenilirliğini ve okunabilirliğini de etkileyecekti.**

Örneğin:

**GOOD — %90+**

```text id="bxj6f8"
SIGNAL  █████████░  94%
QUALITY GOOD

21:42 — Victim enters building.
22:07 — Unidentified male enters building.
22:51 — Same individual exits building.
```

Metin temiz, terminal stabil.

**MODERATE — %50–80**

```text id="ek9pjf"
SIGNAL  ██████░░░░  63%
QUALITY MODERATE

21:42 — Victim enters building.
22:07 — Unidentified m_le enters building.
22:5_ — Same individual exits building.
```

Satır yazılırken karakterler kısa süre glitch yapabilir:

`22:07 — Unid#ntif!ed m_le...`

sonra bir kısmı stabilize olur:

`22:07 — Unidentified m_le enters building.`

Yani oyuncu **hangi bilginin kesin olmadığını görür.**

**POOR — %20–50**

```text id="6ehkvg"
SIGNAL  ███░░░░░░░  31%
QUALITY POOR

21:4_ — Individual enters building.
22:0_ — ███████████ enters building.
22:5_ — Individual exits.
```

Burada sistem artık cinsiyet, tam saat veya kişinin aynı kişi olup olmadığını güvenilir şekilde çıkaramayabilir.

Daha da önemlisi bazı bölümlerde:

```text id="rfqbsf"
22:13:08 — SIGNAL LOST

       [ NO RECORD AVAILABLE ]

22:37:51 — SIGNAL RESTORED
```

olabilir.

Dolayısıyla **CCTV bize her zaman gerçeğin tamamını vermiyor.**

Mesela bir tanık:

> “22:20'de binadan kimse çıkmadı.”

diyor.

Ama CCTV:

`SIGNAL LOST 22:13–22:37`

Oyuncu tanığın doğru söylediği sonucuna varamaz. Kamera o sırada zaten kayıt sağlayamamış.

Ve bunun görsel sunumu da signal'a bağlı olacak:

**Good:** çok hafif scanline, stabil yazı.  
**Moderate:** ara sıra horizontal distortion + yazılırken karakter bozulması.  
**Poor:** daha yoğun parazit + eksik karakterler/timestamp + bazı bilgilerin `UNKNOWN` olması.  
**Signal Lost:** terminal tamamen kesiliyor.

Burada özellikle bir kural koymuştuk: **glitch oyuncuyu kandırmak için rastgele olmamalı.** Eğer `22:07` gerçekten güvenilir biçimde tespit edilmişse görsel efekt yüzünden oyuncu `22:01` okumasın. Belirsiz olan veri, vaka datasında zaten belirsiz olarak tanımlanmalı ve UI bunu görselleştirmeli.

Böylece `SIGNAL` yalnızca güzel görünen bir bar değil, doğrudan **kanıtın güvenilirlik seviyesi** haline geliyor. Bu da ileride vaka yazarken bize çok fazla oyun alanı açar.

## Tur 40

### Kullanıcı

peki karakter modellemsi nasıl olmalı

### Asistan

Bence karakter tarafında da masa tasarımında bulduğumuz aynı kuralı uygulamalıyız: **“pahalı görün, ucuz üretilebilir ol.”** Oyunun geri kalanı 2D/2.5D asset tabanlıyken karakterleri gerçekçi 3D yaparsak iki farklı oyun birleşmiş gibi olur.

image_group{"query":["Papers Please character portraits gameplay","Not Tonight game character dialogue portraits","pixel art detective game character portrait dialogue","2D noir detective game character portraits"]}

Ben karakterleri **2D, hafif stilize, ciddi ve modüler** yapardım. *Papers, Please* kadar düşük çözünürlüklü olmak zorunda değil; bizim masa/dosya tasarımımızdaki daha detaylı pixel/illustrated dili devam ettiririz.

### Görüşme ekranı

Karakteri tam vücut göstermeye gerek yok. Masanın karşısında oturuyormuş gibi **göğüs/omuz üstü portre** yeterli.

Örneğin ekran:

```text id="6bd99n"
┌──────────────────────────────────────────────┐
│ ELİF DEMİR                         WITNESS    │
│                                              │
│                  ┌───────────┐               │
│                  │           │               │
│                  │   ELİF    │               │
│                  │ portrait  │               │
│                  │           │               │
│                  └───────────┘               │
│                                              │
│ “I wasn't at the apartment today.”           │
│                                              │
│ ──────────────────────────────────────────── │
│                                              │
│ > When were you last there?                  │
│ > Who else has a key?                        │
│ > That's all for now.                        │
│                                              │
└──────────────────────────────────────────────┘
```

Ama karakter tamamen hareketsiz karton gibi de durmasın.

### Her karakter için 20 animasyon üretmeyelim

Ben başlangıçta yaklaşık **4–5 expression/state** yeter derdim:

**Neutral** — normal konuşma  
**Thinking** — soru sonrası kısa bekleme  
**Uneasy** — rahatsızlık  
**Defensive** — baskı altında  
**Upset/Angry** — uygun karakterlerde

Fakat bunları bile her NPC için beş ayrı çizim yapmak zorunda değiliz.

Modüler karakter sistemi yapılabilir:

**Head + hair + face + torso + clothing + accessories**

gibi.

Örneğin elimizde:

12 yüz tipi × 20 saç × 15 kıyafet × ten varyasyonları × gözlük/sakal/aksesuar

olursa yüzlerce farklı NPC üretebiliriz. Önemli karakterlere ise elle hazırlanmış küçük detaylar eklenir.

---

### “AI görseli” hissinden kaçınmanın asıl yolu stil tutarlılığı

Senin önceki görsellerde rahatsız olduğun nokta bence tam buydu. Her karakter hiper-detaylı illustration olursa bir süre sonra:

> AI ile portre basılmış.

hissi verir.

Bizim karakterlerimiz masa assetleriyle **aynı üretim dilinde** olmalı.

Keskin şekiller, kontrollü gölgeler, sınırlı detay, hafif texture/pixel dokusu.

Saçın her telini görmemize gerek yok. Cilt gözenekleri hiç gerekmiyor.

Hatta karakterlerin yüzlerinde biraz **grafik sadeleştirme** oyuna özgü kimlik kazandırır.

---

## Konuşurken hareket nasıl olacak?

Tam animasyon yerine **mikro animasyon**.

Karakter ekrana geldiğinde çok hafif yerleşme hareketi.

Konuşurken belki 2–3 frame ağız hareketi bile gerekmeyebilir. Ben şahsen ağız oynatmayı bile düşünmezdim.

Metin gelirken:

> “I wasn't at the apartment—”

karakter neutral.

Sonra:

> “...today.”

küçük expression değişimi.

Oyuncu CCTV'yi sunduğunda:

> **Present CCTV Record**

1 saniyelik sessizlik.

Portre:

**Neutral → Uneasy**

> “Okay... I was there.”

Bu **tek expression değişimi**, 10 saniyelik animasyondan daha etkili olabilir.

---

# En güzel tarafı: ifadelerdeki karakter görseli de aynı asset

Elif'le görüştük.

Sonra Case File'a döndük.

**INTERVIEW — ELİF DEMİR**

sayfasında küçük vesikalık:

`[ELİF PORTRAIT]`

Aynı karakter asset'inin neutral hali.

People kaydında yine aynı.

Böylece oyuncu 20 kişi olduğunda bile isim okumadan:

> “Ha, bu kadın.”

diye tanıyor.

---

## Şüpheli sorgusunda sistemi değiştirmeyiz

Bu önemli.

Witness Interview için başka sistem, Suspect Interrogation için başka pahalı sistem yapmayalım.

Aynı character renderer.

Sadece çevresi değişebilir.

Normal görüşme:

daha açık arka plan / ofis.

Interrogation:

daha karanlık ton, masa kenarı, sert ışık.

Karakter aynı.

UI aynı temel sistem.

Ama atmosfer:

**“Bu artık ciddi.”**

diyor.

---

### Karakterlerin tamamı aynı ölçüde detaylı da olmak zorunda değil

Bence üç kategori güzel çalışır:

**Background NPC**  
Tek portrait/state. Örneğin resepsiyon görevlisi.

**Witness / Person of Interest**  
Neutral + 2/3 expression.

**Major Character / Suspect**  
4–5 expression + birkaç özel reaction.

Böylece 70 vaka üretirken her konuştuğumuz insana aynı production bütçesini harcamıyoruz.

---

Ve ben özellikle **dedektifimizin yüzünü göstermemeyi** ciddi düşünürdüm.

Oyuncu masaya yukarıdan bakıyor. Görüşmelerde karşısındaki insanı görüyor. Bizim sorularımız metin olarak geliyor.

Dolayısıyla kamera hiçbir zaman bize dönmüyor.

Bu hem karakter üretim maliyetini azaltır hem de:

**“Bu dedektif benim.”**

hissini güçlendirir.

Bence bizim mevcut görsel dilimize en uygun kombinasyon: **2D/2.5D masa + fiziksel belgeler + text-only teknik sistemler + stilize 2D portre karakterler + minimum expression animasyonu.** Bu artık oldukça tutarlı tek bir sanat yönü oluyor.

## Tur 41

### Kullanıcı

resimlesen nasıl olur sence

## Tur 42

### Kullanıcı

olmaz ama ben böyle bir şey istemiyorum ki yine geldik yapay zekaya bence papers please mantığı daha iyi pixel olması yani söylediğinde yaptığın resim farklı

### Asistan

Evet, burada yine yanlış tarafa kaydım. Senin istediğin **“2D çizilmiş güzel karakter portresi” değil**; çünkü o yine AI illustration hissine dönüyor.

Doğru yön, masada yakaladığımız görsel dilin aynısı: **Papers, Please mantığında gerçek pixel-art karakter assetleri.** Düşük/orta çözünürlük, sınırlı renk paleti, net pixel kümeleri, detay yerine silüet ve ayırt edici özellikler. Karakterin yüzü “güzel çizilmiş portre” değil, gerçekten oyun sprite'ı gibi görünmeli.

Mesela görüşmede karakter yaklaşık göğüs hizasına kadar görünür; 2–4 frame idle yeter. Duygu değiştiğinde AI tarzı yeniden çizilmiş yüz yerine kaş, ağız, göz ve baş pozisyonunda birkaç piksellik değişiklik olur. Böylece `neutral / uneasy / defensive / angry` durumları gerçek sprite sheet mantığıyla yapılabilir.

Daha da önemlisi, **modüler karakter sistemi pixel-art'ta çok daha mantıklı**:

`Body + Head + Hair + Eyes + Nose + Mouth + Clothing + Facial Hair + Accessory`

Bunların hepsi aynı pixel grid'e oturur. Böylece 15 kafa, 20 saç, 10 göz, 15 kıyafet vb. kombinasyonlarla çok sayıda NPC çıkarabiliriz ve hepsi **aynı sanatçı tarafından yapılmış gibi** görünür.

*Papers, Please*'den birebir kopyalamayız; bizim çözünürlüğümüz biraz daha yüksek ve İstanbul/polis soruşturması atmosferimize uygun olur. Ama felsefe aynı:

**Illustration değil → sprite.  
Portre değil → oyun asset'i.  
Fotogerçekçilik değil → okunabilir silüet.  
AI görüntüsü değil → kontrollü pixel grid.**

Hatta mevcut dosya/masa ekranımız biraz fazla “illustrated” kalıyorsa karakterler geldiğinde onu da aynı pixel-art yönüne biraz daha çekmek iyi olabilir. Böylece masa, dosya, CCTV terminali ve karakterlerin tamamı **tek oyunun parçaları** gibi görünür.

Bir sonraki karakter görselini yapacaksak bu sefer özellikle **pixel-art sprite sheet + oyundaki gerçek görüşme ekranı** olarak çizmek lazım; konsept illustration değil.

## Tur 43

### Kullanıcı

yap o zaman bir reis

## Tur 44

### Kullanıcı

biraz daha miniamalist kadın dedik sakalı var

## Tur 45

### Kullanıcı

kesinlikle bu

### Asistan

Evet, **karakter tarafında referansımız bu olsun.** Öncekiler fazla illüstrasyona kaçıyordu; bunda ise gerçekten oyunun parçası olan bir sprite hissi var.

Özellikle koruyacağımız dil: düşük/orta çözünürlüklü **pixel-art**, sade yüz geometrisi, az ama okunabilir mimik, sınırlı renk paleti, göğüs üstü karakter görünümü ve detaydan çok silüetle karakter ayrıştırma. Kadın/erkek/yaş gibi özellikler de birkaç piksel farkla bile net okunmalı.

Bence yalnız bir sonraki iterasyonda alttaki **“İfade Durumları” geliştirici gösterimini gerçek oyun ekranından kaldırırız**. Oyuncu `Normal / Düşünüyor / Tedirgin / Savunmada` diye state görmemeli; bunlar bizim arka plandaki character state'lerimiz olur. Oyuncu sadece karakterin yüzündeki değişimi fark eder.

Aynı şekilde soldaki `Notlar: sakin görünüyor, zaman çizelgesinde tutarsızlıklar var` kısmını da kaldırmayı tercih ederim. **Oyun oyuncunun yerine insanı analiz etmemeli.** Ekranda karakter, temel kimlik bilgileri, söylediği cümle ve sorabileceğimiz sorular yeterli.

Yani bu görselin **sanat yönü kesinlikle evet**, fakat gerçek gameplay versiyonu bundan da minimalist olacak.

## Tur 46

### Kullanıcı

Evet dediğin gibi yani görmemeli kaldırırız da

### Asistan

Aynen. Onlar sadece karakter sisteminin nasıl çalışacağını göstermek için konseptte kalabilir; **final oyunda oyuncu state isimlerini veya sistemin yaptığı yorumları görmemeli.**

Gerçek görüşme ekranında yalnızca karakter, adı/temel bilgileri, söylediği cümle ve o anda sorulabilen sorular olur. `Tedirgin`, `Savunmada`, `Yalan söylüyor olabilir`, `Zaman çizelgesinde tutarsızlık var` gibi yorumları oyun yazmaz. Oyuncu **sprite'taki birkaç piksellik mimik değişiminden, cevabın içeriğinden ve dosyadaki bilgilerden** kendi çıkarımını yapar.

Hatta karakterin tedirgin görünmesi bile suçluluk anlamına gelmemeli. Masum biri polisle konuşurken de tedirgin olabilir. Bu da daha önce koyduğumuz **“yalan söylemek ≠ suçlu olmak”** prensibiyle aynı çizgide.

Dolayısıyla artık görsel dilimiz de netleşiyor: **minimalist pixel-art masa → odaklanmış fiziksel dosyalar → text-only CCTV terminali → minimalist pixel-art görüşmeler.** Bence bunların hepsi artık aynı oyuna aitmiş gibi duruyor.

## Tur 47

### Kullanıcı

Karakterimizin ismi Global olmalı ayrıca oyun içerisinde gerçek logo ve isimleri kullanamayız

### Asistan

Kesinlikle. Hatta burada iki önemli tasarım kararını netleştirelim.

**Oyuncunun kurumu tamamen kurgusal olmalı.** Önceki görsellerde kullandığımız “İstanbul Emniyet Müdürlüğü”, gerçek polis armasını andıran logo vb. final oyunda olmamalı. Zaten farklı ülkelerde vakalar işleyeceğimiz için global bir kurum çok daha iyi oturuyor.

Ben kurum için örneğin **GIB — Global Investigation Bureau** gibi tamamen kurgusal bir yapı düşünürdüm. Altında oyuncunun çalıştığı birim olabilir:

> **GLOBAL INVESTIGATION BUREAU**  
> Criminal Investigation Division  
> Case Management System

Ama `GIB` gibi isimleri de seçmeden önce marka/kurum çakışması açısından ayrıca kontrol etmek gerekir. Final isim aşamasında birkaç aday çıkarıp araştırırız.

Karakterimiz için de Türkçe bir isim yerine **uluslararası ortamda doğal duran** bir isim seçelim. Hatta oyuncunun dedektifinin adını kendisinin belirlemesi bence daha güçlü olabilir:

> **First Name**  
> **Last Name**
>
> `INVESTIGATOR [PLAYER NAME]`

Böylece masadaki kimlik kartında, raporlarda ve kariyer ekranında oyuncunun adı görünür. NPC'ler ise vaka ülkesine göre doğal isimler taşır; İstanbul'daki Mert/Elif gibi isimler İstanbul vakasında gayet mantıklı, Londra vakasında İngiliz, Tokyo vakasında Japon isimleri kullanılır.

Aynı kural marka ve nesnelerde de geçerli olabilir. Gerçek dünyadaki:

**WhatsApp →** `Messages` / kurgusal mesajlaşma uygulaması  
**Instagram →** kurgusal sosyal ağ  
**Google →** kurumun kendi database/search sistemi  
**iPhone →** generic smartphone  
**BMW/Mercedes →** kurgusal/generic araç markası

şeklinde gideriz.

Böylece oyunun kendi küçük evreni oluşur. Hatta bence bu daha kaliteli: CCTV'nin üstünde bile gerçek polis logosu yerine **bizim kurgusal kurumumuzun küçük pixel-art amblemi** olur ve bütün dosyalarda aynı görsel kimliği taşır.

Bir de “global” derken ana karakterin adı için **Alex Morgan, Jordan Reed, Sam Carter** gibi farklı ülkelerde kolay okunabilen, cinsiyet konusunda da nispeten esnek isimler düşünülebilir. Fakat oyuncuya isim seçtirmek bana daha doğru geliyor; çünkü zaten dedektifin yüzünü göstermiyoruz. **Oyuncu doğrudan investigator oluyor.**

## Tur 48

### Kullanıcı

bube Polis Karakolu diyebiliriz istanbula diğerleri içinde aynı varyasoylnıar kullanaabiliriz.

### Asistan

Evet, hatta bube markasını oyun evreninin içine sokmak güzel olabilir. Gerçek kurumları taklit etmek yerine tamamen **bubeGames evrenine ait kurgusal kolluk yapısı** kurmuş oluruz.

İstanbul için örneğin:

> **bube Polis Karakolu**  
> İstanbul Soruşturma Birimi

veya daha kurumsal görünmesini istersek:

> **bube Police Department**  
> Istanbul Investigation Division

kullanabiliriz.

Diğer ülkelerde de aynı kurumun yerel varyasyonları olur:

**İstanbul:** bube Polis Karakolu  
**London:** bube Police Department  
**Berlin:** bube Polizeidienststelle  
**Tokyo:** bube Police Department

Ama ben kurum adını her ülkede tamamen çevirmek yerine **“bube”yi sabit marka** olarak tutup alt birimi yerelleştirirdim. Böylece oyuncu ülke değiştirdiğinde bile aynı organizasyonun farklı yerel birimlerinde çalıştığını hisseder.

Logoda da gerçek polis yıldızı/arma/kartal vb. taklit etmek yerine çok basit, özgün bir **bube Investigation** sembolü tasarlarız. Aynı sembol dosya kapağında, CCTV terminalinde, görüşme odasında, rapor başlığında ve oyuncunun kimlik kartında tekrar eder.

Bu ayrıca oyunun ileride kendi görsel kimliğini oluşturması açısından da daha güçlü olur: oyuncu ekran görüntüsünü gördüğünde gerçek bir kurumun arayüzü değil, **“bu bubeGames'in dedektif oyunu”** diyebileceği bir dil oluşur.

## Tur 49

### Kullanıcı

Örnek olarak nasıl olur peki

### Asistan

Mesela ben **bube'yi uluslararası bir teşkilat gibi değil, her şehirde aynı oyun evrenine ait yerel polis yapılanmasının markası** gibi kullanırdım. Böylece İstanbul vakasında “neden yabancı bir global kurum Türk polisine ait olaya bakıyor?” sorunu da çıkmaz.

Örneğin İstanbul'da:

```text
bube POLİS KARAKOLU
İstanbul · Kadıköy Şubesi

SORUŞTURMA BİRİMİ
────────────────────
DOSYA #001
KONUT HIRSIZLIĞI

Sorumlu Soruşturmacı
ALEX REED
```

Londra'ya geçtiğimizde aynı tasarım dili:

```text
bube POLICE
London · Camden Division

CRIMINAL INVESTIGATION UNIT
────────────────────────────
CASE #018
MISSING PERSON

Investigator
ALEX REED
```

Tokyo'da:

```text
bube POLICE
Tokyo · Shinjuku Division

INVESTIGATION UNIT
──────────────────
CASE #031
...
```

Yani **bube logosu ve kurumsal kimliği değişmiyor**, şehir/şube değişiyor.

Oyuncunun karakterine de örneğin **Alex Reed** diyebiliriz. Global okunuyor, kısa ve UI'da güzel duruyor. Ama isim seçilebilir olacaksa varsayılan isim olarak kullanılabilir.

### Logoyu da çok basit düşünürdüm

Polis yıldızı, gerçek devlet arması vb. kullanmayalım. Mesela pixel-art'ta kolay okunabilecek:

```text
 ┌───┐
 │ B │
 └─┬─┘
   │

 bube
 POLICE
```

gibi geometrik bir `B` amblemi.

Dosyanın köşesinde sadece küçük:

**[B] bube POLICE**

CCTV ekranında:

**BPS // CCTV ARCHIVE**

gibi.

Burada `BPS` de kurgusal sistem kodu olabilir:

**bube Police System**

CCTV ekranımız mesela:

```text
BPS // CCTV ARCHIVE
ISTANBUL · KADIKÖY

CAM 01                              ● REC
16 OCT 2026                      11:48:32

SIGNAL  ████████░░  82%
QUALITY GOOD

──────────────────────────────────────

REVIEWED PERIOD  08:00 — 18:00

08:27 — Male resident exits building.

11:48 — Female individual enters building.

12:16 — Same individual exits building.

17:54 — Male resident enters building.

──────────────────────────────────────
REVIEW COMPLETE
```

Görüşme odasında da duvarda kocaman polis logosu yerine küçük:

**bube POLICE · ISTANBUL**

yeter.

Bence burada özellikle **“bube Polis Karakolu” biraz sevimli/indie**, **“bube Police” ise daha ciddi ve global oyun markası** hissi veriyor. İkisini birleştirmek de mümkün: oyuncunun gördüğü kurumsal branding her yerde `bube POLICE`, yerel belgelerde ise `bube Polis Karakolu — İstanbul/Kadıköy Şubesi`.

Böylece hem bubeGames'e bağlanıyor hem de oyun kendi evrenine sahip oluyor.

## Tur 50

### Kullanıcı

Olur mantıklı bunu da not alalım

### Asistan

Not aldım. **bube Police** ana kurumsal kimlik olacak; ülke ve şehre göre yerel şube isimleri değişecek ama logo, temel tasarım dili ve BPS gibi sistemler aynı evrene ait kalacak. Gerçek kurum/arma ve marka kullanımından kaçınacağız.

Böylece ileride dosya, CCTV, sorgu ekranı veya yeni ülke tasarlarken hepsini bu kurala göre devam ettirebiliriz.

## Tur 51

### Kullanıcı

Oyun ismi ne olmalı sence aktif olarak pazarda olmayan google play store ve apple store'da yaratıcı olarak, Dont Talk the Lier mı desek falan

### Asistan

Bence burada isim çok önemli çünkü oyun mekaniğimiz aslında tek bir cümlede özetlenebiliyor: **insanların söylediklerine değil, dosyadaki gerçeklere güven.** İsim de bunu taşımalı.

`Don't Talk the Lier` fikrindeki hissi seviyorum ama İngilizce olarak doğal değil. `lier` değil **liar** olur; ayrıca “Don't Talk to the Liar” dersen anlamı “yalancıyla konuşma” oluyor. Bizim oyunda tam tersine herkesle konuşuyoruz. 😄

Ben isimde **lie / statement / evidence / doubt / case** eksenini kullanırdım. Fakat hızlı pazar taramasında dikkat çeken bir şey var: bu alan şimdiden kalabalık. `Case Closed` hem App Store/Google Play'de hem Steam'de kullanılıyor; `Liar Logic` ve `True Doubt` gibi doğrudan yalan/çelişki üzerine kurulmuş dedektif oyunları da mevcut. :chatgpt-content-reference{index="0"} Dolayısıyla **Case Closed, Liar Logic, True Doubt** gibi yönlerden uzaklaşırdım.

Bizim oyuna daha özgü olabilecek isimleri şöyle daraltırdım:

- **Don't Trust the Statement** — konsepti çok iyi anlatıyor ama biraz uzun.
- **Trust No Statement** — daha sert, akılda kalıcı ve dosya/sorgu sistemine cuk oturuyor.
- **Read Between Lies** — güzel kelime oyunu; “satır aralarını oku” + yalanları çöz.
- **The Last Statement** — ciddi, polisiye ve premium hissi var.
- **False Statement** — polis/dosya atmosferine çok uygun ama biraz jenerik.
- **Unsaid** — tek kelime, çok güçlü marka olabilir; oyunun “söylenmeyeni fark et” tarafına uyuyor.
- **Unaccounted** — özellikle zaman çizelgesi, kayıp dakikalar, alibi ve CCTV mantığımıza güzel oturuyor.
- **No Statement Is Innocent** — çok karakterli ama oyun isminden çok tagline gibi.
- **Read the Record** — bizim dosya/CCTV/rapor oynanışımıza doğrudan bağlanıyor.
- **Off the Record** — müthiş polisiye hissi veriyor ama çok kullanılan bir ifade olduğu için marka açısından araştırmak gerekir.

Ben özellikle **TRUST NO STATEMENT** üzerinde dururdum.

Pixel-art açılış ekranını düşün:

```text
        bubeGames

   TRUST NO STATEMENT

      [ B ] bube POLICE

       NEW CASE
       ARCHIVE
       CAREER
```

Ve altında küçücük:

**Everyone has a story. Evidence doesn't.**

Bu isim oyunun temel felsefesini de anlatıyor. Çünkü bizim oyunda **herkes yalancı değil**, hatta yalan söyleyen kişi suçlu bile olmayabilir. Dolayısıyla `Find the Liar`, `Catch the Liar` gibi isimler oyunun mekaniğini yanlış anlatır. `Trust No Statement` ise “ifadeyi otomatik doğru kabul etme, doğrula” diyor.

Bir başka güçlü seçenek de **UNSAID**. App icon'da yalnızca:

```text
UN
SAID
```

ve pixel `B` amblemi oldukça karakteristik olabilir.

Yalnız “aktif olarak Google Play/App Store'da yok” konusunda sadece genel web aramasına güvenip kesin konuşmam. **Final 5–10 ismi çıkardıktan sonra her birini App Store + Google Play + Steam + alan adı + temel marka/trademark çakışması açısından tek tek tarayalım.** Şu anda yaptığım arama aday elemek için ilk tarama seviyesinde.

