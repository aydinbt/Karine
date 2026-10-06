# Yeniden üretilecek görseller

Portreler ve adli bulgular bitince topluca yeniden üretilecek. Şu anki görseller oyunda yer tutuyor; yenisi aynı yola yazılır.

| Yol | Sorun | Gemini'ye ek |
|---|---|---|
| `Items/case012_sheet` | Yenisi geldi ama başlık okunur: "Hospital Medicine Count Sheet" — yalnız başlıksız tekrar | "clipboard with handwritten tally columns, unreadable, some numbers circled in red" |

Üslup kararı (5 Ekim 2026): bütün bulgular portrelerle aynı 2D mürekkep çizim tarzında; prompt'lardaki "photorealistic" bu tarzla değiştirildi. #011–#012 de bu tarzda yenilenir.

## Bulgular ayrı turda (5 Ekim 2026, kullanıcı kararı)
#013'ten itibaren yalnız portreler geliyor. Adli bulgular, portreler bitince her biri için ayrıntılı (nesne, konum, durum, yasaklar açık) yeni prompt'larla topluca üretilecek; #011–#012'nin bulguları da o turda yenilenir. Henüz bulgusu olmayan dosyalar: #013–#024.

## Bulgu turu hazır (5 Ekim 2026)
- #013–#024: ayrıntılı Gemini prompt'ları `EVIDENCE_PROMPTS.md`'de (nesne → durum → ortam → kamera → ışık → yasaklar).
- #025–#073: ayrıntılı bulgu prompt'ları her dosyanın `CASE0NN_PROMPTS.md` → "Adli bulgular" bölümünde.
- Dünya kartpostalları (jp, fr, us, it, es, ca, au): `WORLD_BACKDROP_PROMPTS.md`.
- Görsel geldikçe bu tabloya sorunlu olanlar eklenir. Bulgular `Bube/Items/caseNNN_<ad>.png` yoluna yazılır; sonra `python3 Tools/bind-items.py` çalıştırılır; oyuna bağlar.
| `Items/case028–033_*` | Pikselli kenar — 1024×765 olanlar kırpılarak temizlendi; 1024×1024 kalan #028–#029 kontrol edilecek | gerekirse yeniden üret |
| `Items/case053_file1994` | Daktilo sayfası yarı okunur sahte İngilizce, tarih "1990" gibi okunuyor (dosya 1994); "REOPENED" bandı okunur | "typed lines as illegible grey marks, no readable text" |
| `Items/case057_ledger` | Okunur İngilizce "Accident Logbook" başlığı ve yarı okunur el yazısı | "handwriting as illegible wavy lines, no readable text" |
| `Items/case058_cradle` | Okunur İngilizce sütun başlıkları (Date/Item/Condition/Notes) ve tarihler | "illegible handwritten entries, no readable text" |
| `Items/case059_list`, `case059_folder`, `case060_policy`, `case060_clock` | Yarı okunur sahte İngilizce ("Bank Transfer Slip", "Amendment", "EMPLOYEEE", tarihler) | "illegible grey marks, no readable text" |
| `Items/case062_count` | Okunur İngilizce "Carbon-Copy Warehouse Inventory" başlığı ve sütun adları | "illegible grey marks, no readable text" |
| `Items/case064_loader` | **Gerçek marka:** koltuk sırtında kabartma "CATERPILLAR" yazısı | "plain seat back, no logo, no brand, no text" — öncelikli |
| `Items/case065_receipt` | Fişte okunur "RESTAURANT" ve yarı okunur satırlar | "illegible grey marks, no readable text" |
| `Items/case066_minutes` | Okunur "CORPORATE BOARD MEETING MINUTES" başlığı ve uydurma isim listesi | "illegible grey marks, no readable text" |
| `Items/case066_table` | Yarı okunur el yazısı tablo ve "Total" satırı | "illegible grey marks, no readable text" |
| `Items/case068_slip` | Okunur sahte-İngilizce tablo ("Ledger" satırları, rakamlar) | "illegible scribbles, no readable words or numbers" |
| `Items/case069_gate` | "ACCESS-CONTROL LOG" başlığı, İngilizce sütun başlıkları, okunur rakamlar | "illegible scribbles, no readable words or numbers" |
| `Items/case071_manifest` | "Carbon Shipping Manifest" başlığı, "RECEIVED/SHIPPED" damgaları, okunur satırlar | "illegible scribbles, no readable words or numbers, blank stamps" |
| `Items/case072_hygiene` | "Council Hygiene Inspection Form" başlığı ve okunur madde satırları | "illegible scribbles, no readable words" |
| `Items/case073_courier` | "CARBORLESS INTERNATIONAL RECEIPT" başlığı, okunur barkod numarası | "illegible scribbles, no readable words or numbers" |
