# Dosya #053 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (7 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Joaquín Ortega** → `Bube/Characters/joaquin`

```
A 63-year-old Andalusian man, silver hair swept back with gel, a trimmed silver goatee, heavy gold rings, hooded watchful dark eyes. Black shirt buttoned to the collar under a black velvet jacket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Macarena Flores** → `Bube/Characters/triana`

```
A 29-year-old Andalusian flamenco dancer, glossy black hair in a tight bun with a red carnation, bold red lips, fierce proud dark eyes. A red polka-dot flamenco dress with ruffled sleeves. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Curro Amaya** → `Bube/Characters/curro`

```
A 58-year-old Spanish Roma flamenco singer, greying black curly hair, heavy-lidded sad eyes, a deep-lined weathered face. Dark grey suit jacket over an open-collared shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Inés Montoya** → `Bube/Characters/montoya`

```
A 34-year-old Spanish woman, dark brown hair in a sleek low ponytail, sharp intelligent eyes red from crying, minimal makeup. A charcoal lawyer's blazer over a black top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Pilar Ruiz** → `Bube/Characters/pilar`

```
A 71-year-old Andalusian woman, white hair in a bun, reading glasses on a chain, a thimble on one finger, knowing tired eyes. A dark purple cardigan with pins stuck in the lapel. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Mateo Santos** → `Bube/Characters/mateo`

```
A 26-year-old Spanish man, short black hair with a fade, a small earring, quick friendly eyes. White bartender shirt with rolled sleeves and a black waistcoat. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

7. **Elif Kaya** → `Bube/Characters/kaya`

```
A 31-year-old Turkish woman, long dark brown wavy hair loose, warm brown eyes, nervous careful smile. A navy blue rehearsal top with a flamenco practice skirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case053_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Gitar teli kangalı** → `Bube/Items/case053_string` — Yarısı açılmış bir kangal çelik gitar teli; iki ucunda deri parçacıkları.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A partially unwound coil of thin steel guitar string, both ends bent into small loops, tiny traces of skin on the metal, beside its small open paper sleeve with blurred unreadable print. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no brand names, no logos.
```

2. **1994 dosyası** → `Bube/Items/case053_file1994` — Sararmış karton bir dosya; içinde bir merdiven fotoğrafı ve tek sayfalık bir ifade.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A yellowed old cardboard case folder lying open, inside a faded 1990s colour photograph of a narrow wooden backstage staircase and a single typed statement page with a signature, a red 'reopened' paper band across the folder, all text blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Siyah deri eldivenler** → `Bube/Items/case053_gloves` — Bir kasanın içinden çıkan, avuçlarında ince çelik izleri olan eldivenler.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A pair of thin black leather gloves, the palms showing fine straight cut marks and grey metal traces, lying beside the open door of a small steel office safe. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case053.jpg`, yatay 16:9)

```
Moody cinematic illustration of an old flamenco tablao in Triana, Seville, after the show: an empty wooden stage under a single warm spotlight, a Spanish guitar lying on a rush chair, a dancer's red fan on the boards, and at the back a narrow steep wooden staircase leading up into darkness towards a half-open dressing room door. Deep red, amber and ink black palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```
