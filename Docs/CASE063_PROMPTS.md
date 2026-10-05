# Dosya #063 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (7 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Théo Marchand** → `Bube/Characters/marchand`

```
A 48-year-old French-Canadian man, slicked-back dark hair, designer stubble, a charming tired face with anxious grey eyes. A black turtleneck under a dark velvet blazer. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Tunde Adeyemi** → `Bube/Characters/adeyemi`

```
A 39-year-old Nigerian-Canadian man, close-cropped hair, a short beard, headphones around his neck, a focused defensive face. A dark grey work shirt with sleeves rolled. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Jules Baptiste** → `Bube/Characters/jules`

```
A 26-year-old Haitian-Canadian man, short twists, a slim face with his grandfather's deep eyes, devastated. A white shirt with the collar open, a loosened thin black tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Zoé Lambert** → `Bube/Characters/zoe`

```
A 31-year-old French-Canadian woman, platinum blonde bob, a nose ring, sharp observant face. A black bartender's apron over a black T-shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Piotr Nowak** → `Bube/Characters/nowak`

```
A 44-year-old Polish-Canadian man, sandy hair tied back, a big friendly face now pale, drumstick calluses. A maroon short-sleeve shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Amara Diallo** → `Bube/Characters/amara`

```
A 35-year-old Senegalese-Canadian woman, long braids pinned up, high cheekbones, tearful proud eyes. A mustard-yellow silk stage dress. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

7. **Guy Deschamps** → `Bube/Characters/deschamps`

```
A 62-year-old Québécois man, white hair, a neat white beard, kind shrewd eyes behind bifocals. A brown corduroy jacket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case063_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Topraklama kesici adaptör** → `Bube/Items/case063_adapter` — Topraklama ucu kesilmiş, kutupları ters bağlanmış bir fiş adaptörü; kulüp sahibinin kasasından çıktı.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small grey electrical plug adapter with its ground pin cut off, the casing opened slightly to show two wires deliberately swapped and re-taped with black electrical tape, beside a velvet-lined office safe tray. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Elektrik denetim raporu** → `Bube/Items/case063_inspect` — Kulübün geçen haftaki elektrik denetim raporu; ‘sahne topraklaması uygun’, imza kulüp sahibinin.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A one-page electrical safety inspection checklist on white paper with every box ticked, a single bold signature at the bottom and a blank round stamp outline, all text illegible. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Bas amfisinin fişi** → `Bube/Items/case063_plug` — Kontrbas amfisinin orijinal fişi; adaptörsüz, topraklama ucu sağlam.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A heavy black three-prong instrument amplifier power cable plug with an intact ground pin, slightly scorched around the base, coiled cable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case063.jpg`, yatay 16:9)

```
Moody cinematic illustration of a basement jazz club in Montreal: a small stage under a single warm spotlight with a double bass lying on its side beside an upright amplifier and a microphone stand, a piano in shadow, empty bistro tables with half-finished drinks, brick walls and a staircase up to a snowy street door. Smoky amber, deep crimson and midnight blue palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still from a high corner of a narrow backstage corridor in a basement jazz club, flight cases, a power distribution box on the wall, cables, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case063_backstage/01–03`**

01.
```
Grainy black-and-white security camera still from a high corner of a narrow backstage corridor in a basement jazz club, flight cases, a power distribution box on the wall, cables, no timestamp, no text of any kind, faces not recognisable. A man in a dark blazer opening the wall power distribution box during the interval, looking back over his shoulder.
```
02.
```
Grainy black-and-white security camera still from a high corner of a narrow backstage corridor in a basement jazz club, flight cases, a power distribution box on the wall, cables, no timestamp, no text of any kind, faces not recognisable. The same man crouching at the stage-side socket strip, plugging something small between a thick cable and the socket.
```
03.
```
Grainy black-and-white security camera still from a high corner of a narrow backstage corridor in a basement jazz club, flight cases, a power distribution box on the wall, cables, no timestamp, no text of any kind, faces not recognisable. After the commotion, the same man crouching at the same socket strip again and slipping something small into his blazer pocket.
```
