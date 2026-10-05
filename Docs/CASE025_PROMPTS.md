# Dosya #025 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Sakai Yū** → `Bube/Characters/sakai`

```
A 26-year-old Japanese man, neat short black hair, thin face, anxious darting eyes, biting his lower lip. Navy hotel staff uniform jacket with a plain name-badge holder (no text), white shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Ishii Daisuke** → `Bube/Characters/ishii`

```
A 52-year-old Japanese man, greying hair combed back, reading glasses pushed up on his forehead, tired puffy eyes, stubble. Plain grey hotel yukata robe over a white undershirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Mori Kenta** → `Bube/Characters/kenta`

```
A 38-year-old Japanese man, slightly messy black hair, pale and sleepless, dark circles, frightened but determined expression. Wrinkled white business shirt, loosened dark tie, no jacket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Ōta Ren** → `Bube/Characters/ota`

```
A 44-year-old Japanese man, short-cropped black hair with grey at the temples, square jaw, calm unreadable expression, a thin scar through his left eyebrow. Black suit, black shirt, no tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case025_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kapsül yastığı** → `Bube/Items/case025_pillow` — 4B-12’den alınan yastık; kılıfın dışında bir el izi.

```
Forensic evidence photograph, top-down view on a neutral grey evidence mat. A small rectangular white capsule-hotel pillow (about 40 cm wide) in a plain white cotton pillowcase. The pillowcase is creased and slightly pressed in the middle, with a faint, barely visible greyish handprint smudge on one side near the edge. No stains of blood. A small blank grey scale ruler with no numbers lies beside it. Flat, even forensic lighting from above. 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. No text, no labels, no logos, no hotel branding.
```

2. **Dolap bilekliği** → `Bube/Items/case025_wristband` — 4B-12 dolabının anahtar bilekliği; Tanabe’nin bileğinde, 4B-11 dolabınınki Mori’de.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A single coiled spiral plastic locker-key wristband, dark blue coil, with a small flat plastic tag and a short metal key attached. The tag is blank (no numbers, no text). Inside a clear transparent zip evidence bag with a blank white label area (no writing). Small blank scale ruler beside it, no numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

3. **‘Kimura’nın çantası** → `Bube/Items/case025_bag` — Dolapta kalan siyah sırt çantası; içinde bir USB bellek ve şirket kartı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. An open plain black nylon business backpack lying flat. Beside it, arranged neatly: a small silver USB flash drive, a folded white shirt, a plastic employee ID card turned face-down so only the plain white back is visible, and a thin stack of printed spreadsheet pages turned face-down. Small blank scale ruler without numbers. Flat even lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. No readable text anywhere, no logos, no brand names.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case025.jpg`, yatay 16:9)

```
Moody cinematic illustration of the interior of a Tokyo capsule hotel corridor late at night: two stacked rows of pale capsule pods along both walls, small round entrances with thin roll-down blinds, one blind on the upper row half-open with darkness inside, dim blue-white strip lighting on the floor, a pair of plastic slippers neatly placed below. Muted teal, grey and cold white palette with a single warm amber reading light glowing inside a distant capsule. Painterly graphic-novel style matching a detective game. No people, no readable text, no numbers on the capsules, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case025_swap/01–03`**

01.
```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable. Two men in identical grey hotel yukata robes standing at the lockers, facing each other; one is holding out his wrist.
```
02.
```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable. The two men exchanging coiled locker wristbands; one man points towards an upper capsule.
```
03.
```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable. The corridor with both men gone; one upper capsule blind is pulled down, the next capsule's blind is half open.
```

**`Bube/Cctv/case025_entry/01–03`**

01.
```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable. A tall man in a dark suit (not a yukata) entering the corridor from the far door, holding a white key card.
```
02.
```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable. The same man reaching up into an upper capsule, his upper body partly inside the pod; the blind is raised.
```
03.
```
Low-resolution ceiling security camera still looking down a narrow capsule-hotel corridor at night, two stacked rows of capsule pods on both sides, lockers at the near end, dim blue strip lighting, grainy, slight fisheye distortion, no timestamp, no text of any kind, faces not recognisable. The man walking quickly back towards the far door, pulling off thin dark gloves.
```
