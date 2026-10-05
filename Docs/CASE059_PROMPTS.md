# Dosya #059 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Mercedes Llorente** → `Bube/Characters/mercedes`

```
A 54-year-old Spanish woman, black hair in an elegant chignon, a fine-boned composed face, pearl earrings, cold assessing dark eyes. A tailored midnight navy dress suit. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Amaia Santa Ana** → `Bube/Characters/amaia`

```
A 31-year-old Spanish woman, long chestnut hair, an angry proud face, tear-swollen eyes, a single silver ring. A deep red coat over a black top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Isidro Navas** → `Bube/Characters/isidro`

```
A 67-year-old Spanish man, white hair combed back, a long solemn lined face, faithful sad eyes. A black butler's waistcoat over a white shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Héctor Bermejo** → `Bube/Characters/bermejo`

```
A 45-year-old Spanish man, short brown hair, rimless glasses, a careful nervous face. A blue-grey suit with a loosened tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case059_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Sarnıç anahtarı** → `Bube/Items/case059_key` — Avlunun sarnıç kapağının büyük demir anahtarı; müdür yardımcısının masasının gizli gözünden çıktı.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A large antique hand-forged iron key with an ornate oval bow and a long shaft, traces of wet limestone dust and green moss on the bit, lying next to an open hidden drawer compartment of dark walnut. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Mütevelli listesi** → `Bube/Items/case059_list` — Vakfın 1989’dan bu yana mütevelli ve danışman listesi; bir adın yanına kurşunkalemle bir yıldız konmuş.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. An old typed foundation board list on cream letterhead paper, the letterhead emblem blank, the names replaced by illegible grey type lines, one line marked with a small pencilled star in the margin, a paperclip at the corner. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Rodrigo’nun dosyası** → `Bube/Items/case059_folder` — Rodrigo’nun bube’ye götüreceği kalın dosya; şömine külünün içinden yarısı yanmış çıktı.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A thick brown cardboard document folder half burned, charred black edges curling, partially burnt typed pages and bank transfer slips inside with all text illegible, flakes of grey fireplace ash around it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case059.jpg`, yatay 16:9)

```
Moody cinematic illustration of a Seville palace courtyard at night: Moorish arches and azulejo-tiled walls around a central courtyard, an orange tree, an ancient stone cistern well-head in the middle with its heavy iron grille lid hinged open, lantern light reflecting on wet flagstones, a single upper window lit. Deep indigo, terracotta and lantern-gold palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still of a Seville palace courtyard at night seen from a high corner, Moorish arches, an orange tree, a round stone cistern well-head with an iron grille lid, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case059_patio/01–03`**

01.
```
Grainy black-and-white security camera still of a Seville palace courtyard at night seen from a high corner, Moorish arches, an orange tree, a round stone cistern well-head with an iron grille lid, no timestamp, no text of any kind, faces not recognisable. An older man in a dark suit standing at the cistern well-head with a woman in a dark dress suit, the iron lid open between them.
```
02.
```
Grainy black-and-white security camera still of a Seville palace courtyard at night seen from a high corner, Moorish arches, an orange tree, a round stone cistern well-head with an iron grille lid, no timestamp, no text of any kind, faces not recognisable. The man leaning over the open well-head, the woman close behind him with her hand raised.
```
03.
```
Grainy black-and-white security camera still of a Seville palace courtyard at night seen from a high corner, Moorish arches, an orange tree, a round stone cistern well-head with an iron grille lid, no timestamp, no text of any kind, faces not recognisable. The woman alone closing the iron lid and turning a key in it.
```
