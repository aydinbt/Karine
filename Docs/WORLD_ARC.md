# Dünya ipi — Japonya'dan Avustralya'ya (5 Ekim 2026)

**Kullanıcı kararı (5 Ekim 2026):** "Japonya dahil bütün dünyaları ve vakaların senaryosunu yaz ve bitir; birbirine bağla; sabit fikre takılma, kişi sayısını dağıt, yeni özellik ekleyebilirsin." Bu belge yedi ülkenin (49 dosya, #025–#073) ortak ipini ve sırasını kanonlaştırır. Her ülkenin ayrıntısı kendi bölüm planındadır (`JP_CHAPTER_PLAN.md` … `AU_CHAPTER_PLAN.md`); her dosyanın tasarımı `CASEnnn_DESIGN.md`, görselleri `CASEnnn_PROMPTS.md`.

## Ana ip: Kâtip

Arendt defterinin son sayfası "Alıcı: Kisaragi Shōji, Tokyo" der. Oyuncu bunu bir şirket sanır. Yedi ülke boyunca ip el değiştirir: Tokyo'daki ticaret evi, Paris'teki müzayede evi, Chicago'daki nakliye firması, Napoli limanındaki aile şirketi, Sevilla'daki vakıf, Montreal'deki reasürans şirketi. Her ülkenin finali yerel halkayı kırar ve bir sonraki şehri gösterir; her finalde aynı iki harf bir yerde durur: **E. V.**

- **Japonya:** E.V. yalnız bir e-postanın imzası ("Paris bekliyor").
- **Fransa:** bir köken belgesinde "E. Varga" imzası; müzayedede gri şemsiyeli bir kadın.
- **ABD:** bir dinleme kaydında bir ses: "Edda'ya söyle, Napoli hazır."
- **İtalya:** liman ofisinde bir fotoğraf; yüz seçilmez, bilekte eski bir saat.
- **İspanya:** vakfın mütevelli listesinde "Dr. Edda Varga, danışman".
- **Kanada:** reasürans poliçesinin lehdarı bir program hesabı: **bube Uluslararası İrtibat Programı**. Program direktörü: Edda Varga. Bora'yı ülkeden ülkeye gönderen telefonun arkasındaki ad.
- **Avustralya:** Varga, Bora'yı her ülkede bir rakip halkayı yaktırmak için kullandı; kendi hattı her seferinde bir sonraki şehre taşındı. Melbourne'da programın bölge koordinatörü bunu bulur ve ölür. Son dosya, 72 dosyalık arşivin kendisiyle kapanır.

**Kural:** İp hiçbir dosyada çözüm şartı değildir. Her dosya kendi içinde kapanır; ip yalnız bir satır, bir belge kenarı, bir ad olarak geçer. Oyuncu takip etmezse finaller yine kendi başına çözülür. Oyun hiçbir yerde "bu bağlantıya dikkat et" demez.

## Bölüm finali telefonları

Her finalin telefonu aynı sesle başlar ("— Bora?"); üçüncü ve beşinci satırlar ülkeye özeldir. Kanada finalinde personel belgesinin imzası ilk kez okunur: *E. Varga, Program Direktörü*. Avustralya finalinde telefon çalmaz.

## Sıra ve kişi sayısı

Kişi sayısı = görüşülebilir kişi. 3–7 arası, ülke içinde dağınık, art arda iki dosya aynı sayıda değil (`DESIGN_AMENDMENTS.md` → "Kişi sayısı sabit değil").

| Ülke | Dosyalar | Şube | Kişi sayıları |
|---|---|---|---|
| Japonya | #025–#031 | bube Tokyo — Nihonbashi | 4 · 6 · 3 · 7 · 5 · 3 · 6 |
| Fransa | #032–#038 | bube Paris — Belleville | 5 · 3 · 6 · 4 · 7 · 4 · 5 |
| ABD | #039–#045 | bube Chicago — Pilsen | 6 · 4 · 7 · 3 · 5 · 6 · 4 |
| İtalya | #046–#052 | bube Napoli — Quartieri Spagnoli | 3 · 5 · 7 · 4 · 6 · 3 · 5 |
| İspanya | #053–#059 | bube Sevilla — Triana | 7 · 4 · 3 · 6 · 5 · 7 · 4 |
| Kanada | #060–#066 | bube Montreal — Mile End | 6 · 3 · 5 · 7 · 4 · 6 · 5 |
| Avustralya | #067–#073 | bube Melbourne — Footscray | 3 · 7 · 4 · 6 · 3 · 5 · 7 |

## Yeni mekanikler — ülke ülke

Hepsi motorda vardı ama #011–#024'te kullanılmadı ya da hiç kullanılmadı. Her ülke birini öne çıkarır, sonrakiler eskileri de kullanır. Böylece görüşmeler "hep aynı şeyi söyleyen" kişiler olmaktan çıkar.

| Ülke | Öne çıkan | Ne değişir |
|---|---|---|
| Japonya | **Bilgiye göre değişen cevap** (`answerVariants`) | Aynı soru, oyuncu bir belgeyi okuduktan sonra başka bir cevap alır. Tanık oyuncunun ne bildiğini sezer; oyun bunu söylemez. |
| Japonya | **Kanıta göre tepki** (`presentedAnswers`) | Aynı soruya iki farklı kayıt öne sürülünce iki farklı cevap gelir. |
| Fransa | **Gerekçeli izin talebi** (`warrant`) | Bir arama veya arşiv, oyuncu iki kaynağı gerekçe olarak seçmeden açılmaz; zayıf gerekçe reddedilir. |
| ABD | **İz seçimi** (`line`, `lineSlots`) | Üç iz var, kaynak yalnız ikisine yeter; seçilmeyen kapanır, geri açmak zaman alır. |
| İtalya | **Kaçan tanık** (`closesAfterRead`) | Bir tanık, oyuncu belli bir belgeyi okuyunca şehri terk eder; ondan önce sorulmayan soru kaybolur. |
| İspanya | **Eski dosyayı açma** (`requestKind: archive`, `reopenYear`) | Bugünkü ölüm, yıllar önce kapanmış bir dosyayı yeniden açmadan çözülmez. |
| Kanada | **Yanıltıcı kayıt tepkisi** (`decoyAnswers`) | İlgisiz bir kaydı öne süren oyuncuya kişi inandırıcı bir şey söyler; yanlış yolu oyun işaretlemez. |
| Avustralya | Hepsi birden + **dosyalar arası arşiv** | Final, önceki ülkelerin kapanmış dosyalarından belgeleri istenebilir kaynak olarak açar. |

Olay yeri planı (`scenePlan`) ve rekonstrüksiyon her ülkede yeniden kullanılır; her final yedi kartlık rekonstrüksiyon taşır.
