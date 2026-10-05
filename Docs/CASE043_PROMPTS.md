# Dosya #043 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Reggie Holloway** → `Bube/Characters/reggie`

```
A 44-year-old African-American man, close-cropped hair with a sharp line-up, thin moustache, smooth confident face with evasive eyes, a gold chain. Purple silk shirt under a black blazer. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Cedric Banks** → `Bube/Characters/banks`

```
A 61-year-old African-American man, short white hair, white stubble, deep laugh lines, gentle tired eyes, a guitar pick tucked behind one ear. Charcoal pinstripe waistcoat over a dark shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Tasha Monroe** → `Bube/Characters/monroe`

```
A 29-year-old African-American woman, long box braids pulled over one shoulder, bold gold eye makeup, shaken wide eyes. Mustard-gold sequinned stage top under a denim jacket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Vic Romano** → `Bube/Characters/romano`

```
A 57-year-old Italian-American man, slicked dark hair greying at the temples, heavy-lidded eyes, a pinky ring, a smile that doesn't move his face. Charcoal leather car coat over a black turtleneck. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Earline Pope** → `Bube/Characters/pope`

```
A 66-year-old African-American woman, silver hair in a short natural cut, large hoop earrings, reading glasses on her nose, stern grieving eyes. Dark green bar apron over a white blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case043_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Köprü kablosu** → `Bube/Items/case043_jumper` — Amfinin içinde, toprak ucunu mikrofon standının gövdesine bağlayan ince bir kablo.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A short length of thin red electrical wire with two small alligator clips on the ends, one clip slightly scorched black, beside a cut-off three-prong plug ground pin. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Mikrofon standı** → `Bube/Items/case043_stand` — Sahnedeki krom stand; tutma yerinde yanık izi.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A chrome microphone stand lying on its side, the grip area showing a small dark scorch mark and pitting on the metal, the microphone removed. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Nakit zarfları** → `Bube/Items/case043_cash` — Ofis kasasında, bantla bağlanmış yirmilik desteleri olan kahverengi zarflar.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. Several thick brown paper envelopes, one open showing a stack of twenty-dollar-like banknotes with blurred unreadable printing held by a rubber band, a pencilled tick mark on each envelope. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case043.jpg`, yatay 16:9)

```
Moody cinematic illustration of an old blues club on the South Side of Chicago at dusk: a narrow brick façade with a glowing neon guitar sign without any letters, a small stage visible through the front window with a single chrome microphone stand fallen over and an amplifier glowing red, empty tables with candles, rain on the sidewalk. Deep blue, neon red and warm amber palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still in the back corridor of a small music club, a grey metal electrical breaker panel on the wall beside a door marked with no text, posters with no readable writing, dim light, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case043_panel/01–03`**

01.
```
Grainy colour security camera still in the back corridor of a small music club, a grey metal electrical breaker panel on the wall beside a door marked with no text, posters with no readable writing, dim light, no timestamp, no text of any kind, faces not recognisable. A man in a dark blazer opening the breaker panel and flipping a switch.
```
02.
```
Grainy colour security camera still in the back corridor of a small music club, a grey metal electrical breaker panel on the wall beside a door marked with no text, posters with no readable writing, dim light, no timestamp, no text of any kind, faces not recognisable. The corridor empty, the breaker panel door closed.
```
03.
```
Grainy colour security camera still in the back corridor of a small music club, a grey metal electrical breaker panel on the wall beside a door marked with no text, posters with no readable writing, dim light, no timestamp, no text of any kind, faces not recognisable. The same man coming back, opening the panel again and flipping the switch back, then walking away fast.
```
