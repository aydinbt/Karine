# Yeniden üretilecek görseller

Portreler ve adli bulgular bitince topluca yeniden üretilecek. Şu anki görseller oyunda yer tutuyor; yenisi aynı yola yazılır.

| Yol | Sorun | Gemini'ye ek |
|---|---|---|

Üslup kararı (5 Ekim 2026): bütün bulgular portrelerle aynı 2D mürekkep çizim tarzında; prompt'lardaki "photorealistic" bu tarzla değiştirildi. #011–#012 de bu tarzda yenilenir.

## Bulgular ayrı turda (5 Ekim 2026, kullanıcı kararı)
#013'ten itibaren yalnız portreler geliyor. Adli bulgular, portreler bitince her biri için ayrıntılı (nesne, konum, durum, yasaklar açık) yeni prompt'larla topluca üretilecek; #011–#012'nin bulguları da o turda yenilenir. Henüz bulgusu olmayan dosyalar: #013–#024.

## Bulgu turu hazır (5 Ekim 2026)
- #013–#024: ayrıntılı Gemini prompt'ları `EVIDENCE_PROMPTS.md`'de (nesne → durum → ortam → kamera → ışık → yasaklar).
- #025–#073: ayrıntılı bulgu prompt'ları her dosyanın `CASE0NN_PROMPTS.md` → "Adli bulgular" bölümünde.
- Dünya kartpostalları (jp, fr, us, it, es, ca, au): `WORLD_BACKDROP_PROMPTS.md`.
- Görsel geldikçe bu tabloya sorunlu olanlar eklenir. Bulgular `Bube/Items/caseNNN_<ad>.png` yoluna yazılır; sonra `python3 Tools/bind-items.py` çalıştırılır; oyuna bağlar.
| `Items/case028–033_*` | Pikselli kenar — 1024×765 olanlar kırpılarak temizlendi; 1024×1024 kalan #028–#029 kontrol edilecek | gerekirse yeniden üret |
