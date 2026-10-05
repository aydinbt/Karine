# Dosya #057 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Beatriz Soto** → `Bube/Characters/beatriz`

```
A 52-year-old Spanish woman, chestnut hair in a neat low bun, composed managerial face, small gold earrings, watchful hazel eyes. A cream linen blouse under a beige gilet with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Esteban Heredia** → `Bube/Characters/esteban`

```
A 41-year-old Andalusian man, thick black hair, sun-darkened skin, strong jaw, angry burning dark eyes. A faded navy work shirt with sleeves rolled. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Dolores Vega** → `Bube/Characters/dolores`

```
A 70-year-old Andalusian woman, white hair tied back, deeply lined sun-browned face, sharp knowing eyes. A dark wine-red cardigan over a black dress. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Karim Ouazzani** → `Bube/Characters/karim`

```
A 33-year-old Moroccan man, short black hair, light beard, lean, tired cautious eyes, a straw hat pushed back. An olive green picking shirt with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Anabel Ríos** → `Bube/Characters/anabel`

```
A 29-year-old Spanish woman, dark hair in a ponytail, glasses, nervous earnest face. A white laboratory coat with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case057_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kurbanın termosu** → `Bube/Items/case057_thermos` — Eski, ezik yeşil bir termos; içinde kalan kahvede böcek ilacı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. An old dented green metal thermos flask with its cup-lid unscrewed beside it, a dark ring of coffee residue inside the cup, a few orange leaves stuck to the base. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Laboratuvar şişesi** → `Bube/Items/case057_vial` — Laboratuvarın ilaç dolabından eksilen küçük cam şişe; müdürün arabasının kapı cebinden çıktı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A small amber laboratory glass bottle with a black screw cap, a plain white blank label, nearly empty, lying in an open evidence bag beside a car door pocket liner. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **1999 kaza defteri** → `Bube/Items/case057_ledger` — Paketleme hanesinin 1999 kaza defteri; bir sayfanın üstüne sonradan yazılmış.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. An old hardbound accident logbook open to a page where a handwritten entry has been scraped and rewritten in different ink, the paper thinned and slightly rough, handwriting deliberately illegible. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case057.jpg`, yatay 16:9)

```
Moody cinematic illustration of an Andalusian orange grove at noon heat: long rows of dark green orange trees heavy with fruit, an abandoned old green thermos and a straw hat lying on the dusty red earth between the rows, plastic picking crates stacked, a white cooperative packing house with a rusty corrugated roof in the background under a white-hot sky. Saturated orange, deep green and dusty ochre palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still of the inside of a small agricultural cooperative canteen, metal lockers, a long table, a coffee machine, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case057_canteen/01–03`**

01.
```
Grainy colour security camera still of the inside of a small agricultural cooperative canteen, metal lockers, a long table, a coffee machine, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable. An old man in a straw hat placing a green thermos on a shelf above the lockers and walking out.
```
02.
```
Grainy colour security camera still of the inside of a small agricultural cooperative canteen, metal lockers, a long table, a coffee machine, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable. A woman in a beige gilet entering the empty canteen, taking the green thermos down and turning her back to the camera.
```
03.
```
Grainy colour security camera still of the inside of a small agricultural cooperative canteen, metal lockers, a long table, a coffee machine, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable. The same woman putting the thermos back on the shelf and leaving quickly.
```
