# Dosya #051 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Gianni De Luca** → `Bube/Characters/gianni`

```
A 56-year-old Italian ferry captain, short grey hair, deeply tanned leathery face, heavy grey moustache, cold narrow eyes. White short-sleeved captain's shirt with plain epaulettes and no insignia. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Tommaso Scotto** → `Bube/Characters/tommaso`

```
A 31-year-old Italian deckhand, curly black hair, sun-darkened skin, a small scar on his chin, uneasy downcast eyes. Navy blue crew sweater with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Birgit Holm** → `Bube/Characters/holm`

```
A 64-year-old Swedish woman, short white hair, pale skin with sunburnt cheeks, bright alert blue eyes, reading glasses on a cord. Red windbreaker over a striped top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case051_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Su geçirmez tablet** → `Bube/Items/case051_tablet` — Kurbanın denetim tableti; kaptan kabinindeki can yeleği dolabından çıktı.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A rugged tablet in a thick orange waterproof case with a lanyard, its screen dark, lying on an orange life jacket folded in a locker, a few drops of seawater on the case. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Küpeşte** → `Bube/Items/case051_rail` — Kıç güvertedeki küpeştenin üstünde yeni bir sürtünme izi ve kopmuş bir yaka düğmesi.

```
Forensic evidence illustration, close-up of a white painted steel ferry deck railing at night under a forensic lamp. A fresh scuff mark on the paint and a small dark blue coat button caught in the joint of the railing. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Seyir defteri** → `Bube/Items/case051_log` — Feribotun seyir defterinin son sayfası; bir saat yanlış yazılmış.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A ship's logbook open on its last page, columns of handwritten entries in blue ink with one entry overwritten in darker ink, a brass pen beside it, all text blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case051.jpg`, yatay 16:9)

```
Moody cinematic illustration of a small night ferry crossing the Bay of Naples towards the island of Procida: the ferry's stern deck empty under a single yellow lamp, the white wake churning behind, the pastel houses of Marina Corricella glowing faintly on the dark shore ahead, a lost blue scarf caught on the railing. Deep navy, lamplight yellow and pastel palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still of a small ferry's stern deck at night, a white railing, a life-ring on the bulkhead, the wake dark behind, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case051_stern/01–03`**

01.
```
Grainy colour security camera still of a small ferry's stern deck at night, a white railing, a life-ring on the bulkhead, the wake dark behind, no timestamp, no text of any kind, faces not recognisable. A woman in a dark blue coat with an orange tablet case standing alone at the stern railing.
```
02.
```
Grainy colour security camera still of a small ferry's stern deck at night, a white railing, a life-ring on the bulkhead, the wake dark behind, no timestamp, no text of any kind, faces not recognisable. A man in a white short-sleeved shirt coming out of a deck door and walking towards her.
```
03.
```
Grainy colour security camera still of a small ferry's stern deck at night, a white railing, a life-ring on the bulkhead, the wake dark behind, no timestamp, no text of any kind, faces not recognisable. The man alone at the railing, then walking back to the deck door holding something orange.
```
