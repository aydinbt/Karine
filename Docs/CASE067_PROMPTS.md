# Dosya #067 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Craig Brennan** → `Bube/Characters/brennan`

```
A 52-year-old Anglo-Australian man, sandy thinning hair, a sunburnt broad face, a gold chain at his open collar, wary pale eyes. A navy short-sleeve work shirt with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Linh Nguyen** → `Bube/Characters/linh`

```
A 34-year-old Vietnamese-Australian woman, long straight black hair tied back, a determined grieving face, sharp dark eyes. A red jacket over a black top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Tavita Faleolo** → `Bube/Characters/tavita`

```
A 41-year-old Samoan-Australian man, short black hair, a big gentle face with a short beard, huge shoulders, shocked kind eyes. A teal rubber apron over a grey singlet. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case067_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Buz tankı kilidinin anahtarı** → `Bube/Items/case067_key` — Tankın kapağındaki asma kilidin anahtarı; müdürün ofisindeki balık tartısının çekmecesinden çıktı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A small brass padlock key on a split ring with a faded blue plastic tag with no writing, wet and speckled with fish scales, lying beside the open drawer of an old steel commercial fish scale. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

2. **Pazar alarm kaydı** → `Bube/Items/case067_alarm` — Pazarın alarm paneli dökümü; ‘kuruldu 18.30’, müdürün kodu.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A narrow thermal printout strip from an alarm control panel, a column of short event lines with times, all text faded and illegible, curled at the ends, beside a keypad fob. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

3. **1998 fotoğrafı** → `Bube/Items/case067_lighter` — Long’un sakladığı eski bir fotoğraf; yangından bir saat önce tezgâhın yanında genç bir bekçi, elinde çakmak.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. An old faded 1990s colour photo print with rounded corners showing a blurry young man in a night watchman's jacket standing beside a market stall, a small flame of a lighter in his hand, face not recognisable, a crease across it. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case067.jpg`, yatay 16:9)

```
Moody cinematic illustration of a covered fish market in Footscray, Melbourne before dawn: rows of stainless steel stalls with crushed ice, styrofoam boxes, hanging bare bulbs, a single stall at the back with its big insulated ice tank lid open and cold mist rising, a tram wire and the corrugated market roof above, wet concrete floor reflecting light. Steel grey, ice cyan and warm bulb amber palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```
