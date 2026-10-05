# Dosya #015 — Kapanış Saati (Birleşik Krallık, beşinci dosya)

**Tür:** şüpheli ölüm, sonra kurum içi örtbas. **Yer/zaman:** Bermondsey, kurmaca *The Lamplighter* pub'ı ve önündeki Tanner Lane; 13 Nisan 2029, kapanış saati (23.00–23.20). **Masa tarihi:** 14 Nisan 2029.

**Seviye:** Bu dosyada kötü adam tek bir kişi değil, bir kurumun içindeki bir grup. Dört tanık aynı şeyi söyler; dördü de yalan söyler; biri ilk polis tutanağını kendi eliyle yazmıştır. Bora'nın Londra'daki ilk "kendi tarafına" karşı dosyası.

## Yeni özellik: Olay Yeri Planı (görüş hattı)
Masada yeni bir kaynak: **kuşbakışı plan** — uygulamada ayrı bir tür değil, `scenePlan` alanı taşıyan bir belge düğümü (`plan`); böylece okuma, rapor ve kanıt altyapısı değişmeden çalışır. Pub'ın içi, sigara alanı ve sokak tek bir çizimde. Planda duvarlar, sütunlar ve olay gecesi park etmiş bir teslimat kamyoneti **engel** olarak durur.

- Bir tanık "şuradaydım" dediğinde (o soruyu sorduğunda), planda o tanığın **beyan ettiği konuma** bir işaret düşer.
- İşarete dokununca o noktadan bir **görüş konisi** çizilir; engellerin arkası gölgede kalır. Oyuncu olay noktasının (kaldırımda bir çarpı) ışıkta mı gölgede mi kaldığını kendisi görür.
- Nesnel kaynaklar (kart ödemesi yapılan terminal, araç kamerası) aynı kişi için ikinci bir işaret düşürebilir: **kaydın gösterdiği konum.** İki işaret yan yana durur.
- Oyun "bu tanık göremezdi" demez, işaret renklendirmez, çelişki etiketi koymaz. Yalnız geometri çizer. (Değişmeyen oynanış kuralına uygun.)
- Bir işaret, kaydı adıyla anan kişiye öne sürülebilir; plan kendisi kimseye sürülemez (adsız).

**Veri (vaka bağımsız, sonraki vakalar da kullanır):**
```json
"scenePlan": {
  "imagePath": "Bube/Plans/case015",
  "occluders": [ {"id":"van","points":[[x,y],...]}, ... ],
  "incident": {"x":0.62,"y":0.81,"labelKey":"..."},
  "markers": [
    {"id":"gareth_claim","personId":"gareth","x":..,"y":..,"facing":135,"fov":110,
     "kind":"claimed","requiresAsked":["case015.gareth.where"]},
    {"id":"gareth_till","personId":"gareth","x":..,"y":..,"kind":"recorded",
     "requiresRead":["till"]}
  ]
}
```
Koordinatlar plan görselinin 0–1 aralığında. Doğrulayıcı: engeller kapalı çokgen, işaretler plan içinde, her işaretin kilidi var olan bir soru/belgeye bağlı, olay noktası tanımlı. Görüş hesabı saf C# (ışın–çokgen kesişimi), EditMode testli.

**Arayüz:** `KarineUI` bileşeni olarak tablet içinde yeni bir sekme ya da kaynak kartı; renk ve punto `KarineTheme`den. Tasarım panosu kullanıcıdan (ChatGPT) istenecek; o gelene kadar mevcut tablet kabı içinde sade bir sürüm.

## Katmanlar
1. **Kapanışta bir kavga.** Callum Reid (33), pub'dan çıkınca kaldırımda düşüp başını bordüre çarparak ölür. İlk tutanak: içeride genç inşaat işçisi Ryan Webb'le itiştiği, Ryan'ın onu dışarıda da ittiği, düştüğü. Dört tanık — Gareth Hollis, Paul Ainsworth, Steve Barker, Neil Garrett — aynısını anlatır. Ryan içerideki itişmeyi kabul eder. Kapanmış bir dosya gibi durur.
2. **Görüş hattı.** Tanıklar "sigara alanındaydık, gördük" der. Planda sigara alanı ile olay noktası arasında yan duvar ve park etmiş kamyonet var: üçünün konisinde olay noktası gölgede. Gareth'in bar terminalindeki kart ödemesi 23.11'de, yani "gördüğü" dakikada **içeride.** Ayrıca üç ifadede aynı tuhaf cümle: *"kafasını bordürün köşesine çarptı."* Barmen Josie ise Ryan'ın 23.05'te arka kapıdan çıkıp gittiğini söyler.
3. **Kim bu dört kişi?** Olay tutanağını yazan memurun adı: Paul Ainsworth. Dört "tesadüfi tanık" aynı karakolda çalışan, o gece izinli polisler. Dördüncüsü Komiser Yardımcısı Neil Garrett. Bir taksinin araç kamerası (CCTV) 23.12'de Callum'un hemen arkasından çıkan birini gösterir: uzun boylu, şapkasız, hızlı. Sigara alanında "üç kişiydik, hep birlikte" diyen dörtlüden biri o sırada orada değildir.
4. **Neden?** Callum'un telefonu kayıp. Bulut yedeği (istenebilir) 12 Nisan'da çekilmiş bir video içerir: pub sahibi Frank Doyle arka bahçede Garrett'a kalın bir zarf veriyor. Callum videoyu bir gazeteciye göndermek üzereydi (taslak e-posta). Garrett bunu öğrendi (Frank söyledi), Callum'u dışarıda yakaladı, telefonu istedi, Callum direnince başına vurdu, düşen Callum'un telefonunu aldı. Üç arkadaşı hikâyeyi Ryan'a yıktı; Paul tutanağı yazdı.
5. **Son ters köşe.** Dosya bube'ye "rutin şüpheli ölüm" olarak devredilmişti. Devir yazısını imzalayan bölge irtibat amiri, Garrett'ın eski ortağıdır ve devir notuna "tanık ifadeleri tutarlı, kısa sürede kapatılması önerilir" yazmıştır. Bu kişi fail değildir ve dosyada suçlanamaz; faks yalnız bir satırla değinir. Bora'nın bir sonraki dosyalarda bu adla yeniden karşılaşma ihtimali açık kalır (bölüm finali #017 için tohum).

## Gerçek, kronolojik
- 12 Nisan 22.40 · Callum arka bahçede Frank'in Garrett'a zarf verdiğini telefonla çeker.
- 13 Nisan 21.30 · Callum barmene "yarın bir gazeteciyle buluşuyorum" der.
- 22.50 · içeride Callum ile Ryan arasında masa yüzünden itişme; Josie ayırır.
- 23.05 · Ryan arka kapıdan çıkar (Josie görür; Ryan'ın otobüs kartı 23.09'da iki sokak ötede).
- 23.10 · Callum ön kapıdan çıkar. 23.11 · Gareth barda kartla öder. 23.12 · Garrett ön kapıdan çıkar (araç kamerası).
- 23.13 · kaldırımda boğuşma, darbe, düşüş. Garrett telefonu alır.
- 23.16 · Steve 999'u arar: "Biri kavgada düştü, itenin adı Ryan."
- 23.40 · Paul Ainsworth, olay yerine gelen devriyeye "tanık olarak" ilk ifadeyi verir ve ertesi sabah tutanağı yazar.

## Kişiler
| Kişi | Yaş | Rol |
|---|---|---|
| Callum Reid | 33 | ölen; portre yok |
| Ryan Webb | 24 | inşaat işçisi; içeride itişti, sonra gitti — kırmızı ringa, aklanır |
| Josie Lang | 38 | barmen; tek dürüst göz tanığı (içeriden) |
| Frank Doyle | 57 | pub sahibi; Garrett'a para ödüyor |
| Neil Garrett | 45 | komiser yardımcısı, izinli; fail |
| Gareth Hollis | 39 | polis, izinli; tanık |
| Paul Ainsworth | 34 | polis, izinli; tanık ve tutanağı yazan |
| Steve Barker | 41 | polis, izinli; 999'u arayan |

## Akış (özet)
- **Açık:** olay raporu (Paul imzalı), otopsi (oksipital darbe + alında yumruk izi; düşüşle tek darbe değil), dört tanık görüşmesi, Ryan görüşmesi, **olay yeri planı**.
- Her tanığın "nerede duruyordunuz?" sorusu plana iddia işaretini düşürür.
- **İstenebilir:** bar terminali kayıtları (Gareth 23.11), 999 kaydı (Steve "Ryan" adını veriyor — Ryan'ı adıyla nereden biliyor?), Ryan'ın otobüs kartı, taksi araç kamerası, Callum'un bulut yedeği.
- **Josie** (planı okuduktan sonra ikinci görüşme): pencereden gördüğünü anlatır — "Garrett beyle Callum aynı anda çıkmadı mı?"
- **Personel bilgisi** (Paul'un tutanağı + 999 kaydı okununca istenebilir): dört tanığın aynı karakolda çalıştığı.
- **Bulut yedeği** → Frank görüşmesi; Frank 2 (video öne sürülünce — video Frank'i adıyla anmaz, ama yedek dökümü "Frank Doyle ile Neil Garrett" diye tanımlar) zarfı kabul eder, Garrett'a Callum'un çektiğini söylediğini itiraf eder.
- **Garrett 3:** dosya ağırlığı altında "kendini savunduğunu" söyler; telefonu Thames'e attığını söylemez — faks bulur.
- **Devir yazısı** (en sonda, dosya kaybı eşiğinde okunabilir): irtibat amirinin notu.

**CCTV:** Taksi araç kamerası, Tanner Lane, 23.12–23.14 — seyri değiştiren tek olay: pub kapısından çıkan uzun boylu adam, kaldırımda Callum'a yetişip kolundan çekiyor (3 kare). Yüz yarı seçilir; boy ve ceket Garrett'la uyumlu, kesin değil.

## Kurallar
- Plan ve araç kamerası kimsenin adını anmaz.
- Bar terminali kaydı "G. Hollis" değil "Gareth Hollis" diye tam ad yazar → Gareth'e öne sürülebilir.
- 999 kaydı Steve Barker'ı ve Ryan'ı adıyla anar.
- Bulut yedeği dökümü Frank'i ve Garrett'ı anar.

## Rapor
- **Callum'un ölümünden sorumlu:** **Neil Garrett** ✔ · Ryan Webb · Frank Doyle · kimse (sarhoş düştü).
- **Nasıl öldü?** kavgada itilip düştü · **telefonu almak için vurulup düşürüldü** ✔ · sarhoşken düştü · belirlenemedi.
- **Dört tanık neden aynı şeyi anlattı?** gerçekten gördüler · **meslektaşlarını korumak için ifadeleri birlikte kurdular** ✔ · alkolün etkisiyle yanıldılar · belirlenemedi.
- **Belirleyici kanıt:** **olay yeri planı** ✔ · bar terminali · 999 kaydı · araç kamerası · bulut yedeği. (Plan bir düğümdür; okunmuş olması gerekir.)

Faksta akıbet: Garrett adam öldürme ve rüşvetten, üç meslektaşı adaleti engellemekten yargılanır; Frank Doyle'un pub ruhsatı iptal edilir. Ek satır: "Devir yazısını imzalayan bölge irtibat amiri hakkında ayrı inceleme açıldı." Ryan seçilirse genç adam tutuklanır, dört polis terfi eder. "Gerçekten gördüler" seçilirse Garrett yargılanır ama arkadaşları tanıklık eder ve beraat eder.

## Ders
Tanığın ne gördüğünü sormadan önce nereden bakabildiğini sor. Birbirini tutan dört ifade dört kanıt değildir; aynı cümleyi kuran dört ağız tek bir kaynaktır.

## Teknik iş (veriyle birlikte)
1. `scenePlan` veri modeli (`Investigation.cs`), doğrulayıcı kuralları, ışın–çokgen görüş hesabı + EditMode testleri.
2. Tablet içinde plan görünümü (`BubeApp.ScenePlan.cs`, `KarineUI.ScenePlan.cs`): plan görseli, işaretler, dokununca koni ve gölge. Önce sade sürüm; görsel pano gelince ona uyarlanır.
3. Başsız PlayMode testi: işaret kilidi açılır, dokununca koni çizilir.
4. `Docs/Architecture.md` ve `UI_KIT.md`'ye yeni bileşen.

## Görseller (onaydan sonra prompt dosyası)
Plan görseli (ChatGPT): kuşbakışı, yazısız pub + sokak planı, mimari çizim tarzı. Portreler (Gemini): Ryan, Josie, Frank, Garrett, Gareth, Paul, Steve. Adli bulgular (Gemini): kaldırım bordürü, kalın zarf (video karesi tarzında değil, fotoğraf), Callum'un kırık telefon kılıfı. Kapak (ChatGPT): kapanış saatinde ışıkları sönen bir Londra pub'ı, yağmur. CCTV (ChatGPT): taksi araç kamerası 23.12, 3 kare.
