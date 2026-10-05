# Yeniden üretilecek görseller

Portreler ve adli bulgular bitince topluca yeniden üretilecek. Şu anki görseller oyunda yer tutuyor; yenisi aynı yola yazılır.

| Yol | Sorun | Gemini'ye ek |
|---|---|---|
| `Items/case011_bronze` | Bronz parçalar yerine eski bir anahtar çizilmiş | "small pile of bronze boat fittings — propeller nuts, cleats, hinges — no keys" |
| `Items/case011_bollard` | İskele yok, leke yok; zeminde sarı silindir | olay yeri fotoğrafı: "on old wet wooden pier planks, river fog, dark smear on the rim, no evidence mat" |
| `Characters/kieran` | Şapkanın tepesi kadrajda kesik, yüz fazla yakın | "full head with space above the cap, head-and-shoulders framing" |
| `Items/case012_lock` | U kilit yerine asma kilit | "heavy black steel U-shaped bicycle lock, not a padlock" |
| `Items/case012_camera` | Tavandaki bantlı kamera yerine fotoğraf makinesi | yerinde fotoğraf: "small dome security camera on a bus ceiling, lens covered with black electrical tape, no evidence mat" |
| `Items/case012_sheet` | Boş kâğıt; panoya takılı sayım çizelgesi değil | "clipboard with handwritten tally columns, unreadable, some numbers circled in red" |

Üslup kararı (5 Ekim 2026): bütün bulgular portrelerle aynı 2D mürekkep çizim tarzında; prompt'lardaki "photorealistic" bu tarzla değiştirildi. #011–#012 de bu tarzda yenilenir.

## Bulgular ayrı turda (5 Ekim 2026, kullanıcı kararı)
#013'ten itibaren yalnız portreler geliyor. Adli bulgular, portreler bitince her biri için ayrıntılı (nesne, konum, durum, yasaklar açık) yeni prompt'larla topluca üretilecek; #011–#012'nin bulguları da o turda yenilenir. Henüz bulgusu olmayan dosyalar: #013–#024.

## Bulgu turu hazır (5 Ekim 2026)
- #013–#024: ayrıntılı Gemini prompt'ları `EVIDENCE_PROMPTS.md`'de (nesne → durum → ortam → kamera → ışık → yasaklar).
- #025–#073: ayrıntılı bulgu prompt'ları her dosyanın `CASE0NN_PROMPTS.md` → "Adli bulgular" bölümünde.
- Dünya kartpostalları (jp, fr, us, it, es, ca, au): `WORLD_BACKDROP_PROMPTS.md`.
- Görsel geldikçe bu tabloya sorunlu olanlar eklenir. Bulgular `Bube/Items/caseNNN_<ad>.png` yoluna yazılır; sonra `python3 Tools/bind-items.py` çalıştırılır; oyuna bağlar.
| `Items/case013_ledger` | Okunur yazı var ("Monthly Rent Statement", "Police…") | "all text must be blurred squiggles, no readable words or numbers" |
| `Items/case014_note` | Gemini okunur bir ad yazdı ("Maria", vakada yok); silindi, sayfa artık boş, üstünde hafif bir yama var | "one illegible pencil scrawl, not a name, no readable letters" |
| `Items/case016_letters` | Prompt'taki "angry scrawl" ifadesi imza yerine okunur yazı olarak çizilmiş | "an illegible scribbled signature, no readable words" |
| `Items/case027_ledger` | Başlıkta okunur "Invoice/INVOICE" yazısı | "remove all text, keep everything else identical" |
