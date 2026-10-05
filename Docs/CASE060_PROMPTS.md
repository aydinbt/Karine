# Dosya #060 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (6 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Yan Tremblay** → `Bube/Characters/tremblay`

```
A 44-year-old French-Canadian man, light brown hair receding, a trimmed beard, a friendly face with evasive pale blue eyes. A dark green quilted vest over a flannel shirt, no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Rivka Klein** → `Bube/Characters/rivka`

```
A 40-year-old Jewish-Canadian woman, dark curly shoulder-length hair, an intelligent tired face, grieving brown eyes. A navy wool coat. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Dmitri Volkov** → `Bube/Characters/dmitri`

```
A 52-year-old Russian-Canadian man, grey buzz cut, a broad flushed face dusted with flour, heavy forearms, shocked grey eyes. A white baker's T-shirt and apron. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Sophie Gagnon** → `Bube/Characters/gagnon`

```
A 24-year-old French-Canadian woman, copper hair in a messy bun, freckles, nervous honest face. A dark red hoodie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Luc Pelletier** → `Bube/Characters/luc`

```
A 58-year-old Québécois man, grizzled grey-brown hair under a wool toque, weathered red cheeks, gruff eyes. A red-and-black buffalo check jacket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Nadia Bouchard** → `Bube/Characters/bouchard`

```
A 37-year-old Lebanese-Canadian woman, long black hair straightened, sharp professional face, cool dark eyes. A charcoal tailored blazer, a lanyard with a blank card. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case060_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Fırın küreği ferrulü** → `Bube/Items/case060_peel` — Uzun ahşap simit küreğinin metal halkası; odun fırınının külünün içinden çıktı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A blackened, heat-discoloured steel ferrule ring and a few charred wood fragments from the end of a long wooden bakery peel, sifted out of grey wood ash, a small heap of ash beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Sigorta poliçesi zeyilnamesi** → `Bube/Items/case060_policy` — Fırının iki ay önceki poliçe zeyilnamesi; ‘kilit kişi’ teminatı, lehtar şirket.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A stapled insurance policy amendment on plain white paper with a blank grey header block, one paragraph highlighted in yellow, two signature lines with illegible scrawls, a blue ballpoint pen beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Mesai kartı** → `Bube/Items/case060_clock` — Müdürün mesai kartı; ‘23.05 çıkış’ damgası.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A buff cardboard employee time card with a column of purple stamped clock-out marks, the last one smudged, the printed numbers blurred and unreadable, lying next to a wall punch-clock slot. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case060.jpg`, yatay 16:9)

```
Moody cinematic illustration of a small old bagel bakery in Montreal's Mile End at midnight in winter: a glowing wood-fired brick oven seen through a steamy shop window, wooden peels leaning on the wall, sesame bagels piled in bins, a snowy street with a spiral outdoor staircase and a single street lamp outside. Ember orange, snow blue and warm brick palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still of a narrow snowy back alley behind a Montreal bakery at night, a wooden back door, stacked firewood, a metal stair, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case060_alley/01–03`**

01.
```
Grainy black-and-white security camera still of a narrow snowy back alley behind a Montreal bakery at night, a wooden back door, stacked firewood, a metal stair, no timestamp, no text of any kind, faces not recognisable. A man in a quilted vest walking out of the alley towards the street and away.
```
02.
```
Grainy black-and-white security camera still of a narrow snowy back alley behind a Montreal bakery at night, a wooden back door, stacked firewood, a metal stair, no timestamp, no text of any kind, faces not recognisable. The same man coming back into the alley eight minutes later and opening the back door with a key.
```
03.
```
Grainy black-and-white security camera still of a narrow snowy back alley behind a Montreal bakery at night, a wooden back door, stacked firewood, a metal stair, no timestamp, no text of any kind, faces not recognisable. The same man leaving through the back door carrying something long wrapped in a sack, walking quickly to the firewood stack.
```
