# Karine — çeviri kılavuzu

**Tarih:** 8 Ekim 2026 · **Diller:** İngilizce (`en`), Almanca (`de`), Fransızca (`fr`), İtalyanca (`it`), İspanyolca (`es`), Portekizce-Brezilya (`pt-BR`). Japonca, Lehçe ve Felemenkçe kullanıcı kararıyla kapsam dışı.

Türkçe **kanondur**. Çeviri oyunu değiştirmez: Türkçede hangi kural işliyorsa her dilde aynı şekilde işler. Oyuncu hangi dilde oynarsa oynasın aynı kayıtları aynı kişilere sunabilir, aynı soruları açar, aynı sonuca aynı kanıtla varır.

## Dosyalar

- Her `tr*.json` dosyasının karşılığı yazılır: `tr.json` → `en.json`, `tr.case014.json` → `en.case014.json`. Biçim aynı: `{"entries":[{"key":…,"value":…}]}`, anahtar sırası kanonla aynı, `ensure_ascii=False`, girinti 1.
- Önce ortak dosya (`<dil>.json`, arayüz) çevrilir; sözlük onunla kurulur ve `Docs/Translation/GLOSSARY_<dil>.md`'ye yazılır. Vaka dosyaları sırayla, #001'den #073'e.
- Her dosyadan sonra: `python3 Tools/check-translation.py <dil> caseNNN` → **0 sorun** olmadan sonraki dosyaya geçilmez. Unity doğrulayıcısı (`TranslationRules`) aynı kuralları uygular.
- Eksik satır oyunu bozmaz (İngilizceye, o da yoksa Türkçeye düşer) ama yayında eksik satır kalmaz.

## Değişmeyen kurallar (denetim zorlar)

1. **Kişi adları değişmez.** `Hasan Yıldız` her dilde `Hasan Yıldız`; Türkçe harfler (ı, ş, ğ, İ) korunur. Unvan çevrilir: `Komiser Hakan Özer` → `Inspector Hakan Özer`, `Doç.` → `Assoc. Prof.`.
2. **Ad geçme eşitliği.** Bir metin kanonda hangi kişilerin adını anıyorsa çeviride de **yalnız onları** anar. Ad eklenmez, düşürülmez. Kural kanondan hesaplanır ("kayıt ancak kişinin adı geçiyorsa öne sürülür"); ekranda oyuncu da aynısını görmeli. Türkçe zamir yerine ad geçiyorsa ad kalır; Türkçe adı anmıyorsa çeviri de anmaz ("the driver", "la vecina").
3. **Yer tutucular** (`{0}`, `{1}`) aynı kalır.
4. **Gerçek kurum adı yok.** Polis, savcılık ve bakanlıkların gerçek adları kullanılmaz (FBI, Scotland Yard, Carabinieri, Guardia Civil, Polícia Federal…). Kurgusal kurum: **bube** markası küçük harfle kalır, tanımı çevrilir: `bube Departman` → `bube Department` / `bube Abteilung` / `Département bube` / `Dipartimento bube` / `Departamento bube`. Kısaltma `BDS` her dilde aynı.
5. **Davranış satırı (`.demeanor`) hüküm vermez.** Yalnız gözlem: "gözünü masaya indirdi" → "looked down at the table". "Yalan", "suçlu", "korkuyor", "samimi" ve karşılıkları yasak.

## Denetimin göremediği ama aynı derecede bağlayıcı kurallar

6. **Cinsiyet sızdırılmaz.** Türkçede "o" cinsiyetsizdir. Kimliği belirsiz bir kişiden (görüntüdeki biri, "biri", "arayan", "şoför") söz eden satır çeviride de cinsiyetsiz kalır: İngilizcede *they / the person*, Almanca *die Person*, Fransızca *la personne / quelqu'un*, İtalyanca *la persona*, İspanyolca *la persona / alguien*, Portekizce *a pessoa / alguém*. Cinsiyet ancak kanon metin adı ya da cinsiyeti açıkça veriyorsa yazılır. Aksi hâlde çeviri oyuncunun çıkarması gereken şeyi hazır verir.
7. **Bilgi eklenmez, belirsizlik korunur.** Türkçe neyi söylemiyorsa çeviri de söylemez; açıklama, ipucu, "aslında" eklenmez. Karakter kaçamak konuşuyorsa çeviride de kaçamak konuşur.
8. **Saat, tarih ve sayı metinde kanondaki gibi kalır** (`21.04`, `00.50`, `14 Mart`). Oyuncu saatleri kayıtlar arasında karşılaştırır; biçim her yerde aynı olmalı. Ay adları çevrilir, rakamlar değişmez.
9. **Yer ve mekân adları** (Beşiktaş, Moda, Southwark, Shinjuku) çevrilmez; tür sözcüğü çevrilir ("Moda iskelesi" → "the Moda pier").
10. **Kayıt sözlüğü tutarlıdır.** "ifade", "tutanak", "kayıt", "CCTV dökümü", "sinyal boşluğu", "faks", "gelen evrak", "sonuç raporu", "yöntem", "dayanak", "yem kayıt" gibi terimler ilk çevrildiği gibi kalır (sözlükte). Aynı kaynak her yerde aynı adla anılır, çünkü oyuncu onu masada adıyla arar.
11. **Arayüz kısa kalır.** `tr.json`'daki düğme, sekme ve etiketler (menu., settings., desk., career.board., file. …) Türkçe uzunluğun ~%130'unu geçmez; geçiyorsa kısa eşanlamlı seçilir. Büyük harf kodda yapılır, metinde büyük yazılmaz (yalnız kanonda büyük olan kalır).
12. **Ses ve ton.** Noir, kuru, kısa cümle. Tanıklar kendi ülkelerinin doğal konuşma dilini kullanır (Londra'da İngiliz İngilizcesi, Sidney'de Avustralya, Montreal'de Québec Fransızcası Fransızca sürümde hafifçe). Küfür kanondaki şiddetten ağır olmaz.
13. **Bora Türk bir dedektiftir.** Bu her dilde böyle kalır; Türkçe hitaplar (Bey, Hanım, Abi) Türkiye vakalarında korunur, diğer ülkelerde kanon zaten yerel hitabı kullanır.

## Bitti ölçütü (dil başına)

- `python3 Tools/check-translation.py <dil>` → 0 sorun, 0 eksik.
- `Tools/run-tests.sh EditMode` yeşil (Unity doğrulayıcısı aynı denetimi koşar).
- Sözlük dosyası yazılmış.
- Oyunda o dil seçilip en az #001 Play Mode'da oynanmış — o zamana kadar ROADMAP'te `[~]`.
