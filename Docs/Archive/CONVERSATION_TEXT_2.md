# Mobil Dedektif Oyunu Fikri — metin arşivi 2/4

Bu dosya önceki ChatGPT konuşmasının erişilebilen metin dökümüdür. Erken öneriler daha sonraki kararlarla değişmiş olabilir. Oyun kararları için BUBE_GAME_MASTER_CONTEXT.md geçerlidir. Görsel çıktılar okuma arayüzünde dosya olarak sunulmadığı için burada yer almaz.

## Tur 18

### Kullanıcı

# Selected text:

## Selection 1
Bu noktadan sonra ben yeni özellik eklemeyi bir süre durdurup oyunun ilk 10-15 dakikasını tasarlardım.

## My request:
şimdi bir hikaye çıkaralım

### Asistan

Evet. Artık sistemi değil, **oyuncunun yaşayacağı ilk vakayı** tasarlamak daha doğru. İlk hikâye aynı anda oyunu öğretmeli, merak uyandırmalı ve ileride işlerin çok daha karmaşıklaşacağını hissettirmeli.

Ben ilk vakada cinayetle başlamazdım. Oyuncu henüz **Junior Criminal Investigator** ve ilk dosyası nispeten sıradan görünen ama sonunda küçük bir ters köşe barındıran bir olay olsun.

## CASE #001 — The Missing Watch

**Yer:** İstanbul  
**Vaka türü:** Evden değerli eşya hırsızlığı  
**Tahmini süre:** 10–15 dakika  
**Zorluk:** Başlangıç  
**Amaç:** Oyuncuya rapor → kişi → ifade → CCTV → çelişki → sonuç döngüsünü öğretmek.

### Açılış

Oyuncunun masasına ilk dosya geliyor.

> **ISTANBUL INVESTIGATIONS DEPARTMENT**  
> CASE ASSIGNMENT — #001
>
> Incident: Residential Theft  
> Location: Kadıköy, Istanbul  
> Reported: 19:18
>
> **Stolen Property:**  
> Vintage Omega wristwatch  
> Estimated value: ₺180,000
>
> Reporting Person: **Emre Karaca**
>
> No signs of forced entry were observed.

Buradaki ilk önemli bilgi:

**Kapıda zorlama yok.**

Fakat oyun bunu oyuncuya “önemli kanıt!” diye işaretlemiyor.

**OPEN CASE FILE**

---

# İlk ifade — Emre Karaca

Emre evin sahibi.

Fotoğraf/3D karakter ekranda.

**Investigator:**  
“When did you last see the watch?”

**Emre:**  
“This morning. Around eight. It was on my bedroom dresser.”

**Investigator:**  
“Who had access to the apartment?”

**Emre:**  
“Only me. My sister has a spare key, but she hasn't been here for weeks.”

Sonra:

> “The cleaner was here today, though.”

İlk şüpheli doğal olarak oluşuyor.

### Selin — Temizlik görevlisi

Emre:

> “She arrived around 10:00. I left for work shortly after.”
>
> “She usually leaves around noon.”

Dosyamıza ilk timeline oluşuyor:

**08:00** — Emre saati gördüğünü söylüyor.  
**10:00** — Selin geliyor.  
**10:15** — Emre işe gidiyor.  
**~12:00** — Selin'in ayrıldığı düşünülüyor.  
**18:40** — Emre eve dönüyor.  
**19:18** — Hırsızlık ihbarı.

Oyuncu artık:

> “Temizlikçi almış olabilir.”

diye düşünmeye başlıyor.

Tam istediğimiz şey.

---

# Selin'in ifadesi

Selin'i çağırıyoruz.

**Investigator:**  
“What time did you leave the apartment?”

**Selin:**

> “A little before twelve. Maybe 11:50.”

**Investigator:**  
“Did you enter the bedroom?”

> “Yes. I cleaned it.”

**Investigator:**  
“Did you see the watch?”

> “No.”

Bu çok önemli.

Yalan söylüyor mu bilmiyoruz.

Oyuncunun önünde artık açık bir şüpheli var.

---

# İlk CCTV kullanımı

Burada oyuncuya yeni sistemimizi tanıtıyoruz.

**NEW RECORD AVAILABLE**

> **BUILDING CCTV ARCHIVE**

Oyuncu açıyor.

Tam konuştuğumuz CCTV UI geliyor:

```text id="f8u2ld"
CAM-01 / BUILDING ENTRANCE                    ● REC
14 OCT 2026                              19:46:12

SIGNAL  ███████░
QUALITY GOOD

────────────────────────────────────────────────

REVIEWED PERIOD
09:30 — 13:00

> 09:57 — Female individual enters building.

> 11:52 — Same individual exits building.

> 12:14 — Male individual enters building.

> 12:22 — Same individual exits building.

────────────────────────────────────────────────

REVIEW COMPLETE
```

İşte ilk **“bir dakika…”** anımız.

Selin doğru söylüyor.

11:52'de çıkmış.

Ama **12:14'te başka bir adam eve geliyor.**

---

# Emre'ye geri dönüyoruz

Yeni soru açılıyor:

**“Who entered the building at 12:14?”**

Emre önce:

> “I don't know.”

Sonra oyuncu biraz bastırıyor.

> “Could've been my sister's boyfriend. Kerem.”

**Investigator:**  
“Does Kerem have a key?”

> “No.”

Burada oyuncunun kafasında:

**Kapı zorlanmamıştı.**

Kerem'in anahtarı yok.

Nasıl girdi?

---

# Yeni kişi — Derya Karaca

Emre'nin kız kardeşi.

Derya'nın ifadesi:

> “Yes, Kerem went there.”

Oyuncu:

“Neden?”

> “I asked him to pick up a folder I'd left there.”

**Investigator:**  
“How did he enter?”

Derya:

> “I gave him my spare key.”

Boom.

Başta Emre:

> “My sister has a spare key, but she hasn't been here for weeks.”

demişti.

Teknik olarak doğru.

**Derya gelmemiş.**

Ama anahtarını Kerem'e vermiş.

Bu da oyuncuya oyunun önemli mantığını ilk vakada öğretir:

### Bir ifade doğru olabilir ama seni yanlış sonuca götürebilir.

---

# Kerem'i sorguluyoruz

Kerem:

> “I went in, grabbed the folder and left.”

CCTV zaten:

**12:14 → 12:22**

8 dakika.

Soruyoruz:

**“Did you enter the bedroom?”**

> “No.”

Derya'nın dosyası çalışma odasındaymış.

Şimdi elimizde iki ihtimal var:

**Selin:** yatak odasına girdi ama CCTV timeline'ı tutuyor.

**Kerem:** eve sonradan girdi ve anahtarı vardı ama yatak odasına girmediğini söylüyor.

Oyuncu doğal olarak Kerem'i suçlamaya yaklaşır.

---

# Küçük bir kanıt

Crime Scene Report'a geri dönüyoruz.

Başlangıçta önemsiz görünen satır:

> **Bedroom dresser:** No disturbance observed.
>
> **Watch box:** Empty.
>
> **Dust pattern suggests the box had not been moved recently.**

Ve burada oyuncuya başlangıç vakası olduğu için ücretsiz küçük bir sistem yönlendirmesi verebiliriz:

**NEW INFORMATION AVAILABLE**

Emre'nin ilk ifadesi:

> “This morning. Around eight. **It was on my bedroom dresser.**”

Ama rapor:

> **Watch box:** Empty.

Burada bir uyumsuzluk var ama hâlâ yeterli değil.

---

# Asıl kırılma

Oyuncu Emre'ye tekrar soruyor:

**“You said the watch was on the dresser. Why is there an empty watch box?”**

Emre:

> “That's where I keep it.”

Investigator:

> “That wasn't the question.”

Kısa sessizlik.

Emre:

> “...I might have put it away. I don't remember.”

İlk kez ifadesi değişiyor.

---

## Son belge

Oyuncuya basit bir **Property / Insurance Record** geliyor.

> **Insurance Policy Update**
>
> Omega Seamaster  
> Declared Value: ₺180,000
>
> Policy amended: **3 days ago**

Şüpheli.

Ama yine:

**sigorta yaptırmak = suçlu**

değil.

Oyuncu biraz daha araştırıyor.

Son olarak:

**Financial Record**

> Emre Karaca  
> Outstanding personal debt: ₺164,000

Artık motive oluşuyor.

---

# Final

Oyuncuya:

> **SUBMIT INVESTIGATION REPORT**

soruları geliyor.

**Was the watch stolen?**

`YES / NO / INSUFFICIENT EVIDENCE`

Oyuncu:

**NO**

Sonra:

**Who filed the false report?**

Emre Karaca.

**Probable motive?**

Financial gain / Insurance claim.

**Which evidence most strongly contradicts the original account?**

Burada birkaç kanıt arasından seçim yaptırabiliriz.

Submit.

Kısa bekleme.

> **CASE #001**
>
> **CLOSED**
>
> False theft report identified.
>
> Insurance investigation initiated.
>
> Department Confidence  
> **+4**

Ve Emre'nin saati daha sonra başka bir yerde sakladığı/elden çıkarmaya hazırlandığı doğrulanır; final raporunda bunu netleştiririz ki oyuncunun sonucu tahmine dayanmış olmasın.

---

## Ama ilk vakada asıl öğrettiğimiz şey suçlu değil

Oyuncu farkında olmadan şunların hepsini öğrenmiş oluyor:

**Case Assignment → Case File → Statement → CCTV → Timeline → yeni kişi → tekrar ifade → rapor karşılaştırma → contradiction → motive → conclusion.**

Tutorial'da:

> “Statements may contain contradictions. Tap here!”

diye oyuncuyu boğmadık.

**Hikâyenin kendisi tutorial oldu.**

Ve özellikle ilk vakada cinayet kullanmamamız hoşuma gidiyor. Oyuncu “demek ki oyun hırsızlık vakaları” diye düşünürken kariyer ilerledikçe:

Burglary → Missing Person → Fraud → Assault → Suspicious Death → Homicide...

gibi kapsam genişleyebilir.

Hatta **CASE #001'de hiçbir rewarded reklam önermem.** İlk vaka oyuncuya sistemi öğretir ve akıcı şekilde çözülür. CASE #002/#003 itibarıyla çelişkiler zorlaşmaya başlayınca ilk kez:

> **REQUEST INVESTIGATIVE ASSISTANCE**

ortaya çıkar.

Böylece reklam sistemi bile oyuncuya önce oyunu sevdirdikten sonra devreye girmiş olur.

## Tur 19

### Kullanıcı

Şunu yapmalıyız ama biz yönlendirmeden oyuncuya bunu yaptırmalıyız tabii ki bir işleyişimiz olacak sıralamamız olmalı ki hikaye devam etmeli yani mesela bana geldi sordu ben cevap verdim hikaye devam etmeli ki diğerine geçmeli eğer ki tekrar aynı kişiye gelirse ben her şeyi söyledim gibilerinden bir şey söylemeli yani dedektif olduğumuz için işleyiş hep doğru gitmeli ama yanlış sonuçta çıkartabilmeli kullanıcı ama bizim işleyişimiz hep net şekilde ilerlemeli anlatabildim mi?

### Asistan

Evet, tam olarak. Burada aslında **iki ayrı şeyi birbirinden ayırmamız gerekiyor: hikâyenin ilerleme sistemi ve oyuncunun vardığı sonuç.**

**Hikâye kontrollü ve mantıksal olarak hep ilerlemeli. Oyuncunun yorumu ise serbest olmalı ve yanlış olabilmeli.**

Mesela oyuncunun “şimdi Selin'e git, sonra CCTV'yi aç” diye yönlendirilmesini istemiyoruz. Bunun yerine vaka kendi doğal akışıyla yeni kapılar açmalı.

### Sistem şöyle çalışabilir

CASE #001 geldi. Başlangıçta erişilebilir tek kişi ihbarı yapan **Emre**, çünkü dosyada henüz başka kimseyi tanımıyoruz.

Oyuncu Emre'yle konuşuyor:

> “The cleaner was here today. Her name is Selin.”

Konuşma tamamlandığı anda sistem arka planda şunu yapıyor:

**PERSON DISCOVERED → Selin**

Artık `PEOPLE` kısmında Selin beliriyor.

Ama ekrana:

> **NEXT: INTERVIEW SELIN**

yazmıyoruz.

Oyuncu Case File'a dönüyor ve kendisi Selin'i görüyor.

---

Selin'le konuşuyor.

> “I left around 11:50.”

Konuşma tamamlandığında binanın kamera sisteminin bulunduğuna dair bilgi ortaya çıkıyor diyelim. Böylece:

**CCTV RECORDS → AVAILABLE**

Oyuncu dosyaya dönünce CCTV sekmesinde yeni kayıt olduğunu görüyor.

Yine:

> “CCTV'yi kontrol et!”

demiyoruz.

Sadece artık erişilebilir.

Bu çok önemli bir ayrım.

---

## Aynı kişiye tekrar giderse ne olacak?

Burada söylediğin şey kesinlikle olmalı.

Emre ile ilk görüşmeyi yaptık.

Oyuncu hiçbir yeni bilgi edinmeden tekrar Emre'ye tıklarsa:

> **Emre:**  
> “I've already told you everything I know.”

veya karakterine göre:

> “Is there something else, Detective?”

Ama yeni kanıt yoksa **yeni soru çıkmaz.**

Dolayısıyla oyuncu NPC'leri spamleyerek hikâyeyi ilerletemez.

Case File'da da:

**EMRE KARACA**  
`INTERVIEWED`

görünür.

---

### Ama sonra yeni bilgi bulursak durum değişir

Örneğin CCTV:

> **12:14 — Male individual enters building.**

Sonra Derya'dan bunun Kerem olduğunu öğreniyoruz.

Artık Emre'nin kartında:

**NEW QUESTIONS AVAILABLE**

belirebilir.

Emre'ye dönüyoruz.

Eski konuşmayı baştan yapmıyoruz.

Yeni soru açılmış:

> **Ask about Kerem**
>
> “Why didn't you mention that Kerem had access to the apartment?”

Böylece NPC konuşmaları **state-based** oluyor.

Bu sistem bütün oyunun temellerinden biri olabilir.

---

# Asıl kritik yapı: Investigation State

Her vaka arka planda görünmeyen bir state graph taşımalı.

Kabaca:

```text
CASE START
   ↓
EMRE INITIAL INTERVIEW
   ↓
┌───────────────┬────────────────┐
│               │                │
SELIN FOUND     DERYA FOUND      CCTV SOURCE FOUND
│               │                │
└──────────┬────┴───────────┬────┘
           ↓                ↓
      NEW INFORMATION   CCTV REVIEW
           │                │
           └───────┬────────┘
                   ↓
             KEREM IDENTIFIED
                   ↓
        NEW QUESTIONS UNLOCKED
                   ↓
             FOLLOW-UP INTERVIEW
                   ↓
             CASE CONCLUSION
```

Fakat oyuncu **bu şemayı görmez.**

Oyuncunun gördüğü yalnızca dosyanın giderek genişlemesi.

İşte bence aradığın sistem bu.

---

## Üstelik sıra her zaman tamamen lineer olmak zorunda değil

Bu çok önemli.

Hikâyenin **checkpoint'leri** lineer olabilir ama checkpoint'lerin arasındaki araştırma serbest olabilir.

Örneğin:

**PHASE 1**

Emre ile konuşmak zorunlu çünkü vakayı ondan öğreniyoruz.

↓

**PHASE 2**

Artık aynı anda:

Selin  
Crime Scene Report  
CCTV

açılabilir.

Oyuncu istediği sırada bakar.

↓

Ama üçünü tamamlayınca yeni bir şey ortaya çıkar:

**Kerem**

↓

**PHASE 3**

Kerem  
Derya  
Financial Report

gibi yeni dallar.

↓

**PHASE 4**

Follow-up Interviews

↓

**Conclusion**

Böylece oyun lineer hissettirmez ama biz hikâyenin kontrolden çıkmasına da izin vermeyiz.

Bence bunun adı bizim tasarımımızda **Investigation Phases** olabilir.

---

# Yanlış sonuç nasıl mümkün olacak?

En güzel tarafı burada.

Biz oyuncuya **bütün gerekli bilgiyi ulaştırıyoruz.**

Ama:

> “Bu kanıt şunu ispatlıyor.”

demiyoruz.

Örneğin oyuncunun dosyasında şunların hepsi var:

**Emre**

> “I last saw the watch at 08:00.”

**Selin**

> “I left at approximately 11:50.”

**CCTV**

> 11:52 — Female exits.  
> 12:14 — Male enters.  
> 12:22 — Male exits.

**Derya**

> “I gave Kerem my spare key.”

**Insurance**

> Policy amended three days ago.

**Financial**

> Outstanding debt: ₺164,000.

Oyuncu bütün bunları görmüş olabilir.

Ama hâlâ:

**ACCUSE**

→ Selin  
→ Kerem  
→ Emre

arasından seçim yapabilir.

Biz Selin seçmesini engellemiyoruz.

İşte dedektiflik burada.

---

## Hatta yanlış sonuç için özel diyalog olmalı

Oyuncu:

**ACCUSE KEREM**

dedi.

Kerem:

> “What? I told you why I was there.”

Oyuncu elindeki kanıtı seçiyor.

Sonuç yetersizse vaka raporu:

> **CONCLUSION NOT SUPPORTED**
>
> Insufficient evidence was provided to substantiate the accusation.

Department Confidence düşebilir.

Ama burada hemen:

> **DOĞRU CEVAP EMRE'YDİ**

dememeliyiz.

Oyuncu isterse dosyayı tekrar değerlendirebilmeli veya bazı vakalarda yanlış kararı gerçekten submit edip **vakayı yanlış kapatabilmeli.**

Bu ikinci seçenek özellikle ileride çok güzel olabilir.

---

# Bir de NPC hafızası şart

Her karakterin basit state'leri olur:

**NOT_CONTACTED**  
Henüz konuşulmadı.

**INTERVIEWED**  
Mevcut sorular soruldu.

**NO_NEW_INFORMATION**  
Tekrar gelirsen yeni bir şey yok.

**NEW_INFORMATION_AVAILABLE**  
Yeni kanıt nedeniyle yeni soru açıldı.

**FOLLOW_UP_COMPLETED**

Örneğin:

**Selin — INTERVIEWED**

Tekrar tıklarsın:

> “I've already told you what happened.”

Sonra CCTV'de bir şey çıkar.

State:

**Selin — NEW QUESTIONS**

Gidersin:

> “You said you left at 11:50. Did you see anyone outside?”

Yeni diyalog.

> “Actually... yes. There was a man waiting across the street.”

**PERSON / LEAD DISCOVERED**

Ve hikâye tekrar ilerler.

---

Bu sistem sayesinde çok güzel bir denge yakalıyoruz:

**Biz kontrol ediyoruz:** Oyuncunun hangi bilgiyi hangi aşamada öğrenebileceğini, hangi karakterlerin ne zaman açılacağını, hikâyenin mantıksal olarak hangi noktalardan geçeceğini.

**Oyuncu kontrol ediyor:** Nereye önce bakacağını, hangi ifadeye inanacağını, hangi bilgiyi önemli göreceğini ve sonunda ne sonuç çıkaracağını.

Yani aslında **oyuncuya hikâyeyi yazdırmıyoruz; soruşturmayı yorumlatıyoruz.**

Bence bundan sonraki hikâyeyi de klasik senaryo şeklinde yazmaktan ziyade tam bu yapıyla tasarlamalıyız: **CASE #001 → Phase 1 → hangi bilgi neyi unlock ediyor → NPC state'leri → follow-up'lar → finalde oyuncunun yapabileceği doğru ve yanlış çıkarımlar.** Böyle yaptığımızda hem hikâye hiçbir zaman kilitlenmez hem de oyun oyuncunun elinden tutuluyormuş gibi hissettirmez.

## Tur 20

### Kullanıcı

bu da olması gereken bir şey not al

### Asistan

Not aldım. Bu, oyunun **temel vaka mimarisi** olarak kabul edelim.

Özellikle şu ayrımı koruyacağız: **hikâye akışı hiçbir zaman bozulmayacak veya kilitlenmeyecek; fakat oyuncunun bu akıştan çıkardığı sonuç doğru olmak zorunda olmayacak.** NPC state'leri, yeni bilgiyle açılan follow-up soruları ve görünmeyen Investigation Phases sistemi de bunun temel parçaları olacak.

## Tur 21

### Kullanıcı

Bu doğrultuda 1. bölümü yapsan nasıl yaparsın unutma ama her bölümde bütün özellikleri göstermiyorduk.

### Asistan

Kesinlikle. Hatta 1. bölümde en büyük hata CCTV, telefon kayıtları, adli tıp, finansal kayıtlar, olay yeri inceleme, sorgu gibi her şeyi göstermeye çalışmak olur.

**Bölüm 1'in görevi oyunu öğretmek değil, oyuncuya “ben bunu çözdüm” hissini yaşatmak.** Sistemlerin geri kalanını sonraki vakalara saklarız.

Ben ilk bölümü yaklaşık **10–15 dakikalık**, 3 kişi ve yalnızca birkaç temel sistemle yapardım.

# CASE 001 — Açık Kapı

**İstanbul / Kadıköy**  
**Tür:** Konut hırsızlığı  
**Oyuncu:** Junior Criminal Investigator  
**Bu bölümde kullanılan sistemler:** Incident Report, People/Interview, basit Crime Scene Report, CCTV Archive, Case Conclusion.

**Henüz yok:** telefon inceleme, banka kayıtları, DNA/parmak izi, otopsi, laboratuvar, araç sorgusu, gelişmiş interrogation, olay yerine 3D ziyaret, reopened case vb.

Bu özellikle bilinçli.

---

## Açılış

Oyuncu doğrudan masasında.

Herhangi bir uzun sinematik yok.

Yeni dosya gelir:

> **CASE #001 — RESIDENTIAL BURGLARY**
>
> Reporting Person: **Mert Aydın**  
> Location: Kadıköy / İstanbul  
> Reported: 18:46
>
> Missing property:  
> • Laptop computer  
> • Wristwatch  
> • ₺24,000 cash
>
> Estimated loss: ₺78,000
>
> **Initial patrol note:**  
> No visible signs of forced entry.

`OPEN CASE`

İlk olarak **Incident Report** açılır.

Raporda önemli ama oyuncuya işaretlenmeyen bir bilgi daha vardır:

> Apartment door was found unlocked by reporting person.

Oyuncunun bildiği şimdilik bu kadar.

---

# PHASE 1 — Mert

Başlangıçta `PEOPLE` altında yalnızca:

**MERT AYDIN — Reporting Person**

var.

Mert'le görüşürüz.

Oyuncu birkaç soru arasından istediğini sorabilir ama **ilk görüşmedeki bütün gerekli sorular sonunda sorulur.**

> **Investigator:** When did you leave the apartment?
>
> **Mert:** Around 08:30. I went to work.

—

> **Investigator:** When did you return?
>
> **Mert:** Around six.

—

> **Investigator:** Was the door locked when you left?
>
> **Mert:** Yes. I'm sure.

—

> **Investigator:** Who else has access?
>
> **Mert:** My girlfriend, Elif. She has a key.

Burada:

**ELİF DEMİR — PERSON DISCOVERED**

Ama oyun:

> ❌ GO INTERVIEW ELİF

demez.

People ekranına dönünce yeni kişi vardır.

Mert bir şey daha söyler:

> “The neighbor across the hall might've seen something. He's home most days.”

**HASAN KAYA — PERSON DISCOVERED**

Artık oyuncunun önünde iki yol var:

**Elif** veya **Hasan**.

İstediğinden başlayabilir.

İşte kontrollü özgürlüğümüz burada başlıyor.

---

# PHASE 2 — Elif / Hasan

### Elif

> **Investigator:** When were you last at the apartment?
>
> **Elif:** Yesterday evening.

Anahtarının olduğunu doğrular.

> “Yes. Mert gave me one months ago.”

Bugün eve gitmiş mi?

> “No.”

Mert'le ilişkileri nasıl?

> “Fine... I guess.”

Biraz garip cevap.

Oyuncunun şüphelenmesine izin veriyoruz.

Ama **oyun `Elif seems nervous` falan yazmıyor.**

Oyuncu yorumlasın.

---

### Hasan

Karşı komşu.

> **Hasan:** I heard the apartment door sometime around noon.

Oyuncu soruyor:

> Did you see anyone?

> “I looked through the peephole. I saw a young woman leaving.”

İlk ciddi gelişme.

**Kadın.**

Ne zaman?

> “Around twelve. Maybe a little after.”

Tanıyor muydu?

> “I think I've seen her with Mert before.”

Oyuncunun kafasında doğal olarak:

**Elif.**

Ama oyun yine hiçbir şey söylemiyor.

---

# Burada CCTV açılıyor

Hasan'ın görüşmesi tamamlandığında bina girişinde kamera olduğu bilgisi onun ifadesinden gelir:

> “There should be a camera downstairs, by the way.”

Böylece:

**NEW SOURCE AVAILABLE — BUILDING CCTV**

Artık konuştuğumuz CCTV sistemini ilk defa kullanıyoruz.

Görüntü YOK.

Sadece gerçek güvenlik sistemi hissi veren ekran.

```text
CAM-01 / MAIN ENTRANCE                  ● REC

16 OCT 2026                           18:58:41
SIGNAL ███████░
CH 01
QUALITY: GOOD

────────────────────────────────────────

REVIEWED PERIOD
08:00 — 18:00

> 08:27 — Male resident exits building.

tak tak tak...

> 11:48 — Female individual enters building.

tak tak tak...

> 12:16 — Same individual exits building.

tak tak tak...

> 17:54 — Male resident enters building.

────────────────────────────────────────

REVIEW COMPLETE
```

İşte oyunun ilk güzel anı.

Mert:

> 08:30'da çıktım.

CCTV:

> 08:27.

Tutuyor.

Hasan:

> Öğlen bir kadın gördüm.

CCTV:

> 11:48–12:16 kadın binada.

Tutuyor.

Elif:

> **Bugün eve gitmedim.**

Problem burada.

---

# PHASE 3 — Follow-up

Artık Elif'in state'i değişir:

**INTERVIEWED → NEW QUESTIONS AVAILABLE**

Oyuncu Mert'e tekrar giderse:

> “I've told you everything I know, Detective.”

Hasan'a giderse:

> “That's everything I saw.”

Yeni bir şey yok.

Ama Elif'in kartında küçük bir işaret vardır.

Oyuncuya:

> **GO TALK TO ELİF**

demiyoruz.

Sadece yeni soru olduğunu sistem gösteriyor.

Elif'e girer.

Yeni soru:

> **You said you weren't at the apartment today.**

Elif:

> “I wasn't.”

Oyuncu artık CCTV bilgisini sunabilir.

> **Present CCTV Record — 11:48**

Elif duraksar.

> “Okay... I was there.”

İşte oyunun ilk **yalan yakalama anı.**

Ama çok önemli:

### Elif'in yalan söylemesi onu suçlu yapmıyor.

---

# Neden yalan söyledi?

Elif:

> “Mert and I broke up three days ago.”

Mert bize:

> “İlişkimiz normal.”

gibi bir izlenim vermişti.

Elif:

> “I went there to get my things.”

Anahtarı hâlâ kendisindeymiş.

11:48'de girmiş.

12:16'da çıkmış.

> **Investigator:** Did you take any of the missing property?
>
> **Elif:** No.

> **Investigator:** Was anyone else there?
>
> **Elif:** No.

Sonra önemli bilgi:

> “When I left, I put the spare key back where Mert keeps it.”

Nereye?

> “In the small planter next to the door.”

...

Oyuncu burada muhtemelen:

**Bir dakika.**

diyecek.

---

# Crime Scene Report'a geri dönüyoruz

Rapor başlangıçtan beri dosyada.

Ama artık oyuncu ne aradığını biliyor.

Raporda:

> Entrance door: No signs of forced entry.
>
> Lock: Functional.
>
> Hallway: No visible disturbance.
>
> **Small planter positioned beside entrance door.**
>
> No key recovered from planter.

İlk okuduğunda önemsizdi.

Şimdi değil.

---

# Mert'e yeni soru açılıyor

**MERT — NEW QUESTIONS AVAILABLE**

> **Investigator:** Why was a spare key kept outside the apartment?

Mert:

> “For emergencies.”

Kim biliyordu?

> “Me and Elif.”

Başka kimse?

Mert:

> “Not that I know of.”

Ve burada yeni bir detay ortaya çıkıyor.

Mert birkaç saniye düşünüyor.

> “Actually... Hasan once helped me when I locked myself out.”

Oyuncu:

**Hasan.**

---

# PHASE 4 — Hasan

Hasan'da artık yeni soru var.

> **Investigator:** You knew about the spare key.

Hasan:

> “I'd forgotten about that.”

Oyuncu:

> **The key disappeared after Elif left.**

Hasan:

> “What are you implying?”

Burada ilk bölüm olduğu için çok dallandırmıyoruz.

Oyuncu tekrar CCTV kaydına bakabilir.

Ama CCTV sadece **bina girişini** görüyor.

Hasan zaten aynı binada yaşıyor.

Dolayısıyla kamerada görünmesine gerek yok.

İşte CCTV'nin de kusursuz bir çözüm makinesi olmadığını daha ilk bölümde öğretmiş oluyoruz.

---

# Final

Artık oyuncu yeterli bilgiyi aldı.

**SUBMIT CASE CONCLUSION**

Ve oyun ilk kez oyuncuya gerçekten bırakılıyor.

### Who most likely removed the property?

**Mert**  
**Elif**  
**Hasan**

Oyuncu seçer.

Sonra:

### Select evidence supporting your conclusion.

Burada örneğin **2 kanıt** seçmesini isteriz.

Hasan için doğru mantık:

**Knowledge of spare key location**

+

**No forced entry**

Ama hâlâ bir problemimiz var.

Bunlar Hasan'ın hırsız olduğunu **kesin kanıtlamıyor.**

Bu nedenle finali burada bitirmem.

Oyuncu Hasan'ı seçip bu kanıtları sunduğunda yeni bir final confrontation açılır.

Hasan baskı altında çelişkili bir ifade verir ve sonunda çalınan laptopun seri numarası üzerinden yapılan basit bir property check ile eşyanın aynı gün yerel bir ikinci el dükkânına bırakıldığı doğrulanır.

Seller:

**Hasan Kaya.**

Artık elimizde gerçekten desteklenen sonuç var.

> **CASE #001 — CLOSED**
>
> Property recovered.  
> Suspect referred for prosecution.
>
> **Department Confidence +3**

---

# Ama oyuncu Elif'i seçerse?

İşte oyunumuz burada kendini gösteriyor.

Mantığı anlaşılır:

**Yalan söyledi + eve girdi + anahtarı vardı.**

Fakat oyuncunun kanıtı aslında yetersiz.

> **CONCLUSION: ELİF DEMİR**

Oyun:

> **Select supporting evidence.**

Oyuncu:

**CCTV 11:48**

ve

**False statement**

seçebilir.

Conclusion gönderilebilir.

**Biz engellemiyoruz.**

Fakat bunlar onun hırsızlığı yaptığını kanıtlamıyor.

Dolayısıyla vaka değerlendirmesinde bunun sonucu olur.

İlk bölüm olduğu için oyuncuyu çok sert cezalandırmayız ama:

> **Conclusion unsupported by available evidence.**

gibi bir sonuç alabilir ve tekrar inceleme şansı verilebilir.

Sonraki vakalarda ise yanlış conclusion gerçekten Department Confidence'a zarar verebilir.

---

## İlk bölümün öğrettikleri aslında sadece 5 şey

**1. Case File**  
Her şey burada.

**2. People / Interviews**  
İnsanlarla konuş.

**3. Yeni bilgi → yeni soru**  
Daha önce konuştuğun birine dönmen gerekebilir.

**4. CCTV Archive**  
Görüntü göstermeyen ama timeline bilgisi sağlayan ilk teknik aracımız.

**5. Conclusion**  
Oyunun sana verdiği bilgiyi yorumla.

Ve çok önemli birkaç şeyi **öğretmedik**:

Telefon yok.  
DNA yok.  
Fingerprint yok.  
Banka yok.  
Otopsi yok.  
Laboratuvar yok.  
Olay yeri gezme yok.  
Şüpheli sorgu odası yok.  
Araç/plaka sistemi yok.  
Cold Case yok.

Mesela **CASE #002'de Fingerprint Analysis**, #003'te Phone Records gelebilir. CCTV ise artık bildiğimiz araç olarak gerektiğinde yeniden kullanılabilir.

Böylece oyuncu her bölüm sonunda yalnızca *“yeni hikâye ne?”* diye değil, aynı zamanda **“bir sonraki vakada departman bana neyin erişimini verecek?”** diye de merak eder.

Ve ilk bölüm için en sevdiğim nokta şu olurdu: **oyun Elif'i kasıtlı olarak çok şüpheli gösteriyor ama hiçbir zaman “Elif şüpheli” demiyor.** Oyuncu kendi kendine o sonuca varıyor. Sonra da ilk kez oyunun ana dersini öğreniyor:

> **Birinin yalan söylemesi, aradığın suçu onun işlediği anlamına gelmez.**

Bence bu

## Tur 22

### Kullanıcı

Konuşulanların da tabii ki dedektif tekrar masasından okuyabilmeli aslında Papers,Please mantığı gibi masasında tıklayarak görebileceği bir çok detay yer almalı dosya vb diye düşünüyorum papers please'de 2d gibi ama değildi assetsler buna yönelikti bizimde buna yönelik olmalı aslında daha basit bir kullanımı olur hemde yapay zeka olmaz

### Asistan

Evet, bence burada oyunun **asıl görsel ve mekanik kimliğini** buluyoruz. Önceden “Case File bir menü olsun” diye düşünüyorduk; senin söylediğinle bunu bir adım ileri taşıyabiliriz:

**Case File bir UI menüsü değil, oyuncunun fiziksel masası olabilir.**

image_group{"query":["Papers Please desk gameplay documents","detective investigation desk case files evidence board game","police case file desk documents evidence"],"num_per_query":2,"aspect_ratio":"16:9"}

Papers, Please'deki temel fikir gibi: oyuncu aslında büyük ölçüde sabit bir çalışma alanında ama önündeki **nesneler ve belgeler oynanışın kendisi**. Biz pixel-art yapmak zorunda değiliz. 2D/2.5D, hazırlanmış asset'ler, dokular, belgeler, fotoğraflar ve küçük animasyonlarla çok daha modern bir versiyonunu yapabiliriz.

### Masayı oyunun merkezi yapardım

Oyuncu ofise döndüğünde karşısında masa:

```text
                    INVESTIGATION BOARD
                ┌─────────────────────────┐
                │ photo     note    photo │
                │      timeline            │
                └─────────────────────────┘


     INBOX                              PEOPLE
   ┌────────┐                         ┌────────┐
   │ NEW    │                         │ MERT   │
   │ REPORT │                         │ ELİF   │
   └────────┘                         └────────┘


             ┌───────────────────────┐
             │                       │
             │      CASE FILE        │
             │       #001            │
             │                       │
             └───────────────────────┘

     CCTV                              NOTES
   [terminal]                         [notepad]
```

Bunların her biri gerçekten tıklanabilir.

`CASE FILE` tıklarsın → dosya açılır.

`PEOPLE` klasörüne tıklarsın → kişi dosyaları çıkar.

`CCTV terminal` → konuştuğumuz CCTV ekranına geçer.

Telefon → gerektiğinde birini görüşmeye çağırma.

Inbox → yeni raporlar.

Böylece klasik:

**EVIDENCE | PEOPLE | REPORTS | CCTV | TIMELINE**

mobil menüsü yapmamıza bile gerek kalmayabilir.

**Masanın kendisi navigasyon.**

---

## Konuşmalar da fiziksel dosyada tutulmalı

Bu söylediğin kesinlikle olmalı.

Örneğin Elif'le görüştük.

Masaya dönüyoruz.

`CASE FILE #001`

açıyoruz.

İçinde:

**INTERVIEW TRANSCRIPTS**

Elif'in sayfasını çekiyoruz.

> **INTERVIEW RECORD — ELİF DEMİR**
>
> Date: 16 OCT 2026  
> Interviewed by: Investigator [Player]
>
> **Q:** When were you last at the apartment?
>
> **A:** Yesterday evening.
>
> **Q:** Did you visit the apartment today?
>
> **A:** No.
>
> **Q:** Do you still have a key?
>
> **A:** Yes.

Oyuncu istediği zaman geri dönüp okuyabilir.

Ezberlemek zorunda değil.

Sonra CCTV dosyasını açıyor:

> **11:48 — Female individual enters building.**

Ve oyuncunun kafasında:

**“Elif bugün gitmediğini söylemişti.”**

bağlantısı kuruluyor.

Oyun:

> 🚨 CONTRADICTION!

diye bağırmıyor.

**Oyuncu fark ediyor.**

Dedektiflik hissini yaratan tam olarak bu.

---

# Belgeleri gerçekten masaya çıkarabiliriz

Bence Papers, Please'den alınabilecek en değerli mekanik bu.

Oyuncu Elif'in ifadesini açar.

Kağıdı masanın soluna sürükler.

CCTV raporunu açar.

Sağına koyar.

Yan yana:

```text
┌────────────────────────┐     ┌────────────────────────┐
│ INTERVIEW: ELİF        │     │ CCTV REVIEW CAM-01     │
│                        │     │                        │
│ Q: Were you there?     │     │ 11:48                 │
│                        │     │ Female enters building │
│ A: No.                 │     │                        │
└────────────────────────┘     └────────────────────────┘
```

Oyuncu gerçekten **iki belgeyi karşılaştırıyor.**

Bu mekanik bizim oyuna cuk oturuyor.

---

## Hatta “Pin Evidence” olabilir

Bir satır önemli geldiyse:

> **11:48 — Female individual enters building.**

üzerine tıklarsın.

**PIN TO INVESTIGATION**

Masadaki küçük not alanına gider.

Başka belgede:

> “I wasn't at the apartment today.”

onu da pinlersin.

Artık Investigation Notes:

```text
INVESTIGATION NOTES

• ELİF:
  "I wasn't at the apartment today."

• CCTV CAM-01:
  11:48 — Female enters building.
```

Ama yine oyun:

**CONTRADICTION**

demiyor.

Sadece oyuncunun kendi topladığı bilgiler.

---

# Ve bu yaklaşım production açısından da bizim için çok daha mantıklı

Çünkü artık her vaka için:

❌ 20 tane AI görseli üretmek  
❌ sinematik yapmak  
❌ sürekli yeni 3D ortam yapmak  
❌ her olay için animasyon yapmak

gerekmiyor.

Asset pipeline oluşturuyoruz.

Örneğin bir kere:

**Belgeler**

Police Report  
Witness Statement  
Interview Transcript  
Forensic Report  
CCTV Report  
Phone Record  
Financial Statement  
Autopsy Report  
Evidence Sheet  
Photograph  
ID Record

hazırlıyoruz.

Sonra her vaka bu asset'leri **farklı içerikle dolduruyor.**

---

### Kişiler için bile AI gerekmiyor

Burada da sana katılıyorum.

Oyunun görsel stilini baştan belirlersek 30–50 adet kaliteli character asset / portrait sistemi hazırlanabilir.

Örneğin hafif stilize edilmiş gerçekçi karakterler.

Aynı karakter sisteminde:

front portrait  
neutral  
nervous  
angry  
upset

gibi birkaç varyasyon.

Hatta görüşmelerde çok fazla animasyon kullanmak zorunda değiliz.

Karakter + çok hafif idle hareket + yazılı diyalog.

Asıl oyun **konuşmanın içeriğinde.**

---

# 2D olması ucuz görünmesi demek değil

Burası özellikle önemli.

Bunu:

> “mobil 2D dedektif oyunu”

gibi düşünmezdim.

Daha çok **tactile investigation desk**.

Kağıdın dokusu var.

Dosya açılırken ses geliyor.

Sayfa çevriliyor.

Kalem masaya bırakılıyor.

CCTV ekranı hafif titreşiyor.

Yeni rapor geldiğinde:

*thump*

Inbox'a fiziksel bir dosya düşüyor.

Telefon çalıyor.

Masa lambasının ışığı kağıtların üzerinde.

Arka planda ofis ambiyansı.

Yağmur varsa cama yağmur sesi.

Saat çalışıyor.

Bunlar teknik olarak çok ucuz şeyler ama birlikte **çok pahalı bir atmosfer hissi** yaratabilir.

---

## Hatta kariyer ilerledikçe aynı masa gelişebilir

Bu da retention'a bağlanabilir.

Junior Investigator:

küçük masa, birkaç dosya, basit terminal.

Criminal Investigator:

daha fazla dosya alanı, CCTV terminali, department phone.

Senior Investigator:

ikinci monitör, gelişmiş database access, investigation board.

Oyuncunun:

> **“kariyerimde ilerledim”**

hissini yalnızca yazıyla değil **çalışma alanının değişmesiyle** gösteririz.

Ve yeni mekanik açıldığında tutorial popup yerine masaya **yeni bir fiziksel araç gelir.**

Örneğin CASE #003 sonrası masaya yeni terminal gelir:

> **AUTHORIZED ACCESS**  
> TELECOMMUNICATION RECORDS

Oyuncu artık o terminale tıklayabilir.

Bu çok daha şık.

---

Bence burada önceki tasarımımızdaki önemli bir şeyi de değiştirmeliyiz: **ana ekranımız “Case Management UI” olmamalı. Ana ekran dedektifin masası olmalı.**

Case File, People, Evidence, Reports, CCTV, Interview Records, Timeline gibi sistemlerin tamamı o masadaki **fiziksel/dijital araçlardan açılmalı.**

Bu hem *Papers, Please* benzeri dokunsal bir oynanış verir, hem asset tabanlı üretilebilir, hem AI görsel ihtiyacını neredeyse ortadan kaldırır, hem de bizim “oyuncuya cevabı söyleme, önüne belgeleri koy ve kendisi fark etsin” felsefemizle çok daha iyi örtüşür.

## Tur 23

### Kullanıcı

Bu doğrultuda bir masa yapsan nasıl yaparsın resim eder msiin

## Tur 24

### Kullanıcı

Mobilde yapıyoruz tepeden kamera açısı ile olmalı bu direkt yapay zeka... papers please gibi bir arayüzü kast ediyrum aslında daha oyunsu hissi yaratır.

## Tur 25

### Kullanıcı

Bu görsel direkt yapay zeka gibi 2D-2.5D yaptığımızı unutma söyledim ya papers,please tasarımsal olarak benzediğinde daha çok gerçekçi duruyor.

## Tur 26

### Kullanıcı

2D bir görüntü istiyorum açıkası mobilde oynanıyor

## Tur 27

### Kullanıcı

biraz daha minimaslist yapılacakları görmeye gerek yok bence oyunda

### Asistan

Kesinlikle. Hatta **“Yapılacaklar” oyunun felsefesine ters** kalıyor. Oyuncuya “Mert'le görüş → CCTV'ye bak → kişileri araştır” dersek dedektiflik yapmıyor, checklist tamamlıyor.

Ben masayı daha minimalist tutardım. Ekranda sadece gerçekten kullanılan fiziksel araçlar olsun: **aktif vaka dosyası**, gelen evrak kutusu, bilgisayar/terminal, telefon ve not defteri gibi. `People / Evidence / Reports / CCTV` şeklinde sürekli duran menüler bile şart değil; bunlar vaka dosyasının içindeki sekmeler veya masadaki araçlar üzerinden açılabilir.

Mesela ana masa yaklaşık şöyle bir kompozisyon olabilir:

```text
┌──────────────────────────────────────────────┐
│  16 OCT 2026                         18:42   │
├──────────────────────────────────────────────┤
│                                              │
│   [INBOX]                     [TERMINAL]      │
│                                              │
│              ┌────────────────┐              │
│              │                │              │
│              │   CASE #001    │              │
│              │                │              │
│              └────────────────┘              │
│                                              │
│   [PHONE]                     [NOTES]         │
│                                              │
└──────────────────────────────────────────────┘
```

**CASE #001** ana obje. Tıklayınca dosya masanın ortasında açılıyor; içinden raporlar, kişiler, ifadeler, kanıtlar ve CCTV çıktıları geliyor.

**INBOX** yeni rapor geldiğinde küçük bir `1` gösteriyor. Oyuncuya ne yapacağını söylemiyor, sadece *yeni bir şey geldiğini* bildiriyor.

**TERMINAL** kariyer ilerledikçe CCTV, kişi sorgulama, telefon kayıtları gibi yetkiler kazandığımız yer olabilir.

**PHONE** görüşme yapacağımız kişilere ulaşmak için.

**NOTES** ise oyuncunun önemli gördüğü bilgileri sakladığı alan.

Gerisi dekor: masa lambası, kalem, kahve, belki küçük saat. Ama bunların üzerine UI butonu koymaya gerek yok.

Ve özellikle **sol tarafta alt alta 7 menü istemiyorum**. Papers, Please'in güçlü taraflarından biri zaten ekranın “oyun HUD'ı” gibi değil, **çalışma alanı** gibi hissettirmesi.

Bu sefer görseli de tam bu mantıkla yapmak gerekir: **mobil yatay, saf 2D/2.5D asset tabanlı, minimalist, tepeden masa; ekranda 4-5 etkileşim noktası ve hiçbir checklist/görev yönlendirmesi yok.**

## Tur 28

### Kullanıcı

yap yapalım

## Tur 29

### Kullanıcı

bu çok sade ve güzel biraz daha minimalist olabilir sonuçta aktif olarak kullanamadoğımız bir şeyi göstermekte olmaz bence

### Asistan

Kesinlikle. Burada **“masada görünen her şeyin bir işlevi olmalı”** kuralını koyabiliriz. Son görsel güzel ama telefon, kahve, kalemlik, not kâğıdı, lamba gibi şeyler sadece dekor olacaksa ekran alanını yememeli.

Özellikle mobilde ben masayı neredeyse **4 ana obje**ye indirirdim:

- **CASE FILE** — oyunun merkezi. Raporlar, ifadeler, kişiler, kanıtlar bunun içinde.
- **INBOX** — yeni rapor/belge geldiğinde fiziksel olarak buraya düşer.
- **TERMINAL** — o an oyuncunun yetkisi olan dijital sistemler. İlk bölümde örneğin sadece CCTV varsa terminalde sadece CCTV erişimi görünür. Telefon kayıtları henüz açılmadıysa adı bile görünmez.
- **PHONE** — yalnızca gerçekten görüşme/çağırma mekaniğini buradan yapacaksak masada bulunur.

Hatta **Notes** mekanik olarak gerekli değilse onu da kaldırırım. Oyuncu zaten dosyaları istediği zaman tekrar okuyabiliyorsa ayrıca not defteri şart değil.

Masa böyle nefes almalı:

```text
┌──────────────────────────────────────────────┐
│ ISTANBUL INVESTIGATION UNIT      18:42       │
│                                              │
│       ┌────────┐              ┌─────────┐     │
│       │ INBOX  │              │TERMINAL │     │
│       │   1    │              │  CCTV   │     │
│       └────────┘              └─────────┘     │
│                                              │
│                ┌────────────┐                │
│                │            │                │
│                │ CASE #001  │                │
│                │            │                │
│                └────────────┘                │
│                                              │
│       ┌────────┐                             │
│       │ PHONE  │                             │
│       └────────┘                             │
└──────────────────────────────────────────────┘
```

En önemlisi de **kariyer ilerledikçe masa kalabalıklaşmaz; işlevleri gelişir.** Terminalin içinde yeni yetkiler açılır. Case File'ın içine yeni belge türleri gelir. Inbox yeni tip raporlar almaya başlar.

Yani CASE #001'de henüz `Phone Records` yoksa ekranda gri `Phone Records 🔒` bile göstermeyelim. **Oyuncu varlığından haberdar olmasın.** İleride yetki geldiğinde terminalde yeni seçenek belirir ve gerçekten “departmanda yeni erişim kazandım” hissi oluşur.

Bir sonraki görseli de bu kuralla yapmak lazım: **dekor minimum, boş alan daha fazla, yalnızca gerçekten etkileşime girilebilen 3–4 obje ve merkezde Case File.** Bu bence mobilde çok daha güçlü durur.

## Tur 30

### Kullanıcı

yap bir resim bakalım

## Tur 31

### Kullanıcı

bu çok iyi

### Asistan

Evet, bence de **aradığımız ana oyun ekranına çok yaklaştık.** Özellikle boş alan bırakması çok iyi; mobilde her yeri bilgiyle doldurmuyor ve göz doğrudan aktif dosyaya gidiyor.

Buradaki tasarım kuralını koruyalım: **Masada görünen şey = kullanılabilen şey.** Yeni yetki açıldığında masaya yeni eşya yığmak yerine mümkün olduğunca mevcut araçların işlevi genişlesin. Örneğin terminal ilk vakada yalnızca CCTV iken ileride telefon kayıtları, kişi/veritabanı sorgusu gibi yetkiler terminalin içine eklenebilir.

Ayrıca dosyanın ortada fiziksel bir obje olması çok iyi. Bence bir sonraki kritik ekran artık **DOSYA #001'e dokunduğumuzda dosyanın açılmış hali** olmalı. Orada ifade tutanaklarının, olay raporunun, kanıtların ve sonradan gelen CCTV çıktısının oyuncunun önünde nasıl organize edildiğini çözebiliriz. Bu ekran doğrudan oyunun dedektiflik mekaniklerini belirleyecek.

## Tur 32

### Kullanıcı

onun içinde bir resimle bakalım nasıl gözükmeli

## Tur 33

### Kullanıcı

Bence böyle görmek yerine ekranın tam ortasında arkası blurlu bir şekilde odaklı görmek daha iyi hem daha kolay okunur hem de kontrolü daha kolay olur

### Asistan

Evet, bence **çok daha iyi**. Masa ana navigasyon alanı olarak kalır; bir objeye dokununca obje **masadan öne gelir**, arka masa blur + hafif karartma alır. Böylece mobilde belgeyi gerçekten okuyabileceğimiz kadar büyütebiliriz.

Örneğin `DOSYA #001`e dokununca dosya ekranın yaklaşık **%80–90'ını** kaplar. Arkada telefon, gelen evrak ve terminal hâlâ seçilir ama odak dışındadır. Dosyanın kendisinde de ilk anda onlarca sekme göstermek yerine yalnızca o anda var olan belgeler görünür:

```text
        ×

 ┌─────────────────────────────┐
 │       DOSYA #001            │
 │       KONUT HIRSIZLIĞI      │
 │                             │
 │  [ OLAY RAPORU ]            │
 │                             │
 │  İhbar Eden: Mert Aydın     │
 │  Tarih: 16.10.2026          │
 │  Yer: Kadıköy / İstanbul    │
 │                             │
 │  ───────────────────────    │
 │                             │
 │  OLAY ÖZETİ                 │
 │  ...                        │
 │                             │
 │                             │
 │  ‹  OLAY RAPORU  1/2  ›     │
 └─────────────────────────────┘

       blurred detective desk
```

Ve bence kritik nokta: **dosyanın içinde klasik mobil uygulama menüsü de yapmayalım.**

Yeni bir ifade geldiyse dosyaya fiziksel olarak yeni bir sayfa eklenir. Oyuncu sağ/sol okla veya swipe ile:

`Olay Raporu → Mert İfadesi → Elif İfadesi → CCTV Raporu`

gezer.

Yeni belge geldiğinde örneğin:

**4 / 4 — CCTV REVIEW**

olarak son sayfaya eklenir.

Böylece hem *Papers, Please* hissini koruyoruz hem de mobil kullanılabilirliği çok daha yüksek oluyor. Masa **oyunun dünyası**, açtığımız belge ise **odak modu**.

Aynı şeyi terminale dokunduğumuzda da yaparız: terminal ekranı ortaya büyür, masa blur olur ve konuştuğumuz CCTV arayüzünü tam okunabilir şekilde görürüz. Telefona dokununca telefon/görüşme arayüzü ortaya gelir.

Yani oyunun bütün UX kuralını tek cümleye indirebiliriz:

**Masa → objeye dokun → odak modu → işi yap → kapat → masaya dön.**

Bu, bence şu ana kadar konuştuğumuz tasarımın mobil için en temiz hali.

## Tur 34

### Kullanıcı

evet o yüzden direkt resim olarak cevap ver

