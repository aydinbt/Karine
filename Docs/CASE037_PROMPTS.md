# Dosya #037 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Clémence Roux** → `Bube/Characters/clemence`

```
A 41-year-old French woman, sleek black bob with a sharp fringe, pale skin, red lipstick, intense tired dark eyes. Oversized ivory linen shirt, a pincushion bracelet on her wrist. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Baptiste Lenoir** → `Bube/Characters/lenoir`

```
A 49-year-old French man, neatly combed brown hair greying at the sides, clean-shaven, smooth charming face, slightly too-white teeth. Midnight navy suit, light grey shirt, a thin silk pocket square. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Fatou Diallo** → `Bube/Characters/diallo`

```
A 36-year-old Senegalese-French woman, dark hair wrapped in a patterned orange and brown headscarf, warm brown skin, red-rimmed grieving eyes, a measuring tape around her neck. Burnt-orange work smock. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Serge Aubert** → `Bube/Characters/aubert`

```
A 61-year-old French man, short grey hair, heavy jowls, bushy eyebrows, patient tired eyes. Dark green security guard jumper with shoulder patches, no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case037_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Buhar presi** → `Bube/Items/case037_press` — Atölyedeki endüstriyel buhar presi; kablosunun yalıtımı bıçakla sıyrılmış.

```
Forensic evidence photograph, three-quarter view in a Paris fashion workshop. A large industrial garment steam press with a padded white board, its power cable lying across the wooden floor with a short section of the insulation neatly sliced open exposing copper wire, a yellow evidence marker beside the cut. Bolts of fabric blurred in the background. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Terzi metresi** → `Bube/Items/case037_tape` — Kurbanın masasının altından çıkan bez mezura; ortasında gerilmiş, buruşmuş bir bölüm.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A yellow fabric tailor's measuring tape laid out in a loose curve, one middle section stretched, twisted and creased with faint skin-coloured smudges, the printed numbers blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Lisans faturaları** → `Bube/Items/case037_invoices` — Kurbanın dolabında saklı fotokopiler: Tokyo lisans faturaları, köşelerinde el yazısıyla iki harf.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A small stack of photocopied invoices with blurred unreadable columns of figures, one corner showing a tiny handwritten two-letter initial in blue ink, illegible, a yellow sticky note with a question mark drawn on it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case037.jpg`, yatay 16:9)

```
Moody cinematic illustration of a Paris fashion atelier at night in Le Marais: a tall arched window on the second floor of an old stone building glowing white, dress forms and hanging fabric silhouettes visible through the glass, a cobbled courtyard below with a single dark car parked under a lamp, light rain. Black, ivory and deep crimson palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still overlooking a small cobbled courtyard of an old Paris building at night, a carriage gate, a single wall lamp, light rain on the stones, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case037_court/01–03`**

01.
```
Grainy colour security camera still overlooking a small cobbled courtyard of an old Paris building at night, a carriage gate, a single wall lamp, light rain on the stones, no timestamp, no text of any kind, faces not recognisable. A man in a dark suit walking out through the carriage gate on foot, holding a phone to his ear.
```
02.
```
Grainy colour security camera still overlooking a small cobbled courtyard of an old Paris building at night, a carriage gate, a single wall lamp, light rain on the stones, no timestamp, no text of any kind, faces not recognisable. The courtyard empty, a dark saloon car parked under the lamp, rain falling.
```
03.
```
Grainy colour security camera still overlooking a small cobbled courtyard of an old Paris building at night, a carriage gate, a single wall lamp, light rain on the stones, no timestamp, no text of any kind, faces not recognisable. The same dark car's headlights coming on and the car driving out through the gate.
```
