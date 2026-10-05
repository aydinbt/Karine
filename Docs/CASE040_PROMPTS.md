# Dosya #040 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Megan Brennan** → `Bube/Characters/megan`

```
A 38-year-old Irish-American woman, copper-red hair tied back loosely, pale freckled skin, swollen grieving blue eyes. Light blue nurse's scrub top under an open grey cardigan. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Kyle Duffy** → `Bube/Characters/duffy`

```
A 41-year-old Irish-American man, short brown hair, reddish stubble, square friendly face with nervous eyes. Brown canvas work jacket over a plaid flannel shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Grace Okafor** → `Bube/Characters/okafor`

```
A 70-year-old Nigerian-American woman, short grey natural hair, reading glasses on a beaded chain, warm sharp eyes. Plum-coloured cardigan over a patterned blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Luz Ramírez** → `Bube/Characters/ramirez`

```
A 44-year-old Mexican-American woman, black hair in a tight bun, soot smudge on her cheekbone, steady dark eyes. Dark navy fire investigator's jacket with reflective stripes, no text or logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case040_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Duman dedektörü** → `Bube/Items/case040_detector` — Tavandan sökülmüş dedektör; pil yuvası boş, kapağında parmak izi tozu.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A round white ceiling smoke detector, slightly smoke-stained, its battery compartment open and empty, black fingerprint powder dusted on the plastic cover showing a partial print. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Viski bardağı** → `Bube/Items/case040_glass` — Koltuğun yanındaki kalın dipli bardak; dibinde çözünmemiş beyaz tortu.

```
Forensic evidence illustration, three-quarter view on a neutral grey evidence mat. A heavy cut-glass whisky tumbler, soot-darkened on the outside, a thin film of amber liquid at the bottom with a faint white powdery residue settled in it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Walt’ın defteri** → `Bube/Items/case040_notebook` — Yarısı yanmış spiralli bloknot; plaka ve saat sütunları.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small spiral-bound police-style notebook, its edges charred and curled, open to a page of handwritten columns of numbers and times that are blurred and unreadable, a coffee ring stain. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case040.jpg`, yatay 16:9)

```
Moody cinematic illustration of a narrow brick row house in Bridgeport, Chicago at night after a fire: blackened front windows, smoke stains above the frames, a fire truck's red lights reflecting on wet pavement, a small front porch with a melted plastic chair, neighbouring houses with lit windows, bare trees. Deep red, charcoal and streetlight amber palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy wide-angle doorbell camera still looking across a narrow residential street in Chicago at night, row houses opposite, a parked pickup truck, a streetlamp, fisheye distortion, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case040_door/01–03`**

01.
```
Grainy wide-angle doorbell camera still looking across a narrow residential street in Chicago at night, row houses opposite, a parked pickup truck, a streetlamp, fisheye distortion, no timestamp, no text of any kind, faces not recognisable. A man in a canvas work jacket walking up the front steps of the house opposite and going inside.
```
02.
```
Grainy wide-angle doorbell camera still looking across a narrow residential street in Chicago at night, row houses opposite, a parked pickup truck, a streetlamp, fisheye distortion, no timestamp, no text of any kind, faces not recognisable. The same man coming out quickly, pulling the door shut, walking to a pickup truck.
```
03.
```
Grainy wide-angle doorbell camera still looking across a narrow residential street in Chicago at night, row houses opposite, a parked pickup truck, a streetlamp, fisheye distortion, no timestamp, no text of any kind, faces not recognisable. An orange glow appearing behind the front window of the house opposite, smoke curling from the eaves.
```
