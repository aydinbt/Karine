# Dosya #062 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Réjean Gauthier** → `Bube/Characters/gauthier`

```
A 51-year-old Québécois man, thinning brown-grey hair, a heavy moustache, big shoulders, a ruddy face with small calculating eyes. A charcoal work jacket with reflective strips and no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Claudette Marceau** → `Bube/Characters/marceau`

```
A 63-year-old Québécois woman, silver hair in a short practical cut, a weathered farmer's face, steady blue eyes. A rust-brown wool cardigan. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Kayla Fortin** → `Bube/Characters/fortin`

```
A 29-year-old Métis woman, long dark hair in a low ponytail, strong jaw, defiant brown eyes. A hi-vis yellow work vest over a dark hoodie, no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Minh Tran** → `Bube/Characters/tran`

```
A 46-year-old Vietnamese-Canadian man, short black hair, glasses, a calm careful face. A dark slate-blue guard's jacket with no badge. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Paul Leblanc** → `Bube/Characters/leblanc`

```
A 60-year-old Québécois man, grey hair, a gentle round face, red-rimmed eyes behind glasses. A forest green sweater. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case062_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Suyla dolu varil** → `Bube/Items/case062_barrel` — Patlamış bir şurup varili; içindeki sıvı renklendirilmiş su, yalnızca üstte bir parmak şurup.

```
Forensic evidence photograph, three-quarter view on a concrete warehouse floor. A burst blue steel 200-litre maple syrup drum lying on its side, a split seam, a puddle of thin pale amber liquid spreading around it, a thin dark syrup layer visible at the rim, evidence markers with blank faces. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Sayım tutanağı** → `Bube/Items/case062_count` — Deponun aylık sayım tutanağı; ‘1.200 varil, tam’, ustabaşının imzası.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A carbon-copy warehouse inventory form on a clipboard, columns of handwritten tallies all ticked, a large confident signature at the bottom, every number and word illegible. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Forklift kontak anahtarı** → `Bube/Items/case062_key` — İkinci forkliftin kontak anahtarı; ustabaşının kamyonetinin güneşliğinin arkasından çıktı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A single forklift ignition key on a yellow plastic tag with no writing, lying next to a folded-down pickup truck sun visor with a clip. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case062.jpg`, yatay 16:9)

```
Moody cinematic illustration of a vast maple syrup warehouse at night: towering steel racks of blue syrup drums stacked to the ceiling, one section collapsed with drums scattered and burst on the concrete floor, a stopped yellow forklift with its forks raised, a single row of high bay lights on. Deep amber, steel blue and shadow black palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still of a large warehouse aisle at night between tall racks of steel drums, a forklift, concrete floor, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case062_aisle/01–03`**

01.
```
Grainy black-and-white security camera still of a large warehouse aisle at night between tall racks of steel drums, a forklift, concrete floor, no timestamp, no text of any kind, faces not recognisable. A woman with a clipboard walking down the aisle, tapping drums one by one with a small hammer.
```
02.
```
Grainy black-and-white security camera still of a large warehouse aisle at night between tall racks of steel drums, a forklift, concrete floor, no timestamp, no text of any kind, faces not recognisable. A forklift with a single heavy-set driver turning into the far end of the same aisle with its forks raised high.
```
03.
```
Grainy black-and-white security camera still of a large warehouse aisle at night between tall racks of steel drums, a forklift, concrete floor, no timestamp, no text of any kind, faces not recognisable. The forklift reversing out of the aisle fast; a cloud of dust and fallen drums behind it.
```
