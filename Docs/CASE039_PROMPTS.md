# Dosya #039 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (6 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Ray Kowalski** → `Bube/Characters/kowalski`

```
A 54-year-old Polish-American man, thick neck, close-cropped greying sandy hair, a broad weathered face with a broken nose, small hard blue eyes. Navy work jacket over a grey hoodie, a high-visibility vest open over it. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Marisol Reyes** → `Bube/Characters/reyes`

```
A 31-year-old Mexican-American woman, long black hair in a high ponytail, small gold hoop earrings, red-rimmed angry brown eyes. Olive green work shirt with rolled sleeves. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Dwayne Carter** → `Bube/Characters/carter`

```
A 47-year-old African-American man, shaved head, short grey beard, heavy-lidded patient eyes. Dark navy security uniform jumper with shoulder epaulettes, no logo or badge text. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Teresa Novak** → `Bube/Characters/novak`

```
A 50-year-old Czech-American woman, dirty-blonde hair in a thick braid, freckled weathered face, fierce green eyes. Bright orange high-visibility work jacket over a flannel shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Pete Lindqvist** → `Bube/Characters/lindqvist`

```
A 62-year-old Swedish-American man, white hair combed neatly, rimless glasses, long thoughtful face. Khaki field jacket over a checked shirt, a white hard hat under his arm. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Hector Ortega** → `Bube/Characters/hector`

```
A 35-year-old Mexican-American man, black hair buzzed short, a faded tattoo on the side of his neck, tired defensive dark eyes. Plain black T-shirt under a grey work jacket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case039_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kopmuş ankraj cıvataları** → `Bube/Items/case039_bolts` — Rafın zemine bağlandığı dört cıvata; kesilme yüzeyleri parlak, yeni kopmuş.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. Four heavy steel anchor bolts sheared off near the base, their broken ends bright and fresh against older rusted threads, each with a small yellow numbered tag without readable text. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Levye** → `Bube/Items/case039_bar` — Konteynır sökme alanındaki alet panosunda; ucunda temizlenmeye çalışılmış koyu leke.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A long steel crowbar with a flat chisel end, the end showing a faint dark brownish residue in the grooves despite being wiped, a few smeared streaks. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Çift etiketli sandık** → `Bube/Items/case039_labels` — Bir sandığın üzerinde, ‘makine parçası’ etiketinin altından görünen eski bir müzayede lot etiketi.

```
Forensic evidence photograph, close-up of the side of a wooden shipping crate in a warehouse. A fresh white shipping label partly peeled back to reveal an older cream-coloured auction lot label underneath with an elegant printed number and blurred unreadable words, stencilled marks on the wood blurred. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case039.jpg`, yatay 16:9)

```
Moody cinematic illustration of a large freight warehouse in Pilsen, Chicago at night: brick and corrugated steel walls with a faded mural on one side, loading docks lit by sodium lamps, a semi-truck trailer backed into a bay, the elevated train tracks visible in the distance, wet asphalt. Inside through an open roller door, a collapsed tall pallet rack with crates spilled across the floor. Deep blue, sodium orange and rust palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still high above a long warehouse aisle at night, tall pallet racks loaded with wooden crates on both sides, sodium lighting, concrete floor, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case039_dock/01–03`**

01.
```
Grainy colour security camera still high above a long warehouse aisle at night, tall pallet racks loaded with wooden crates on both sides, sodium lighting, concrete floor, no timestamp, no text of any kind, faces not recognisable. A forklift parked at the end of the aisle, a figure in a high-visibility vest lying still on the floor beside a rack.
```
02.
```
Grainy colour security camera still high above a long warehouse aisle at night, tall pallet racks loaded with wooden crates on both sides, sodium lighting, concrete floor, no timestamp, no text of any kind, faces not recognisable. A figure in a high-visibility vest driving the forklift into the aisle, forks raised low at the base of a rack upright.
```
03.
```
Grainy colour security camera still high above a long warehouse aisle at night, tall pallet racks loaded with wooden crates on both sides, sodium lighting, concrete floor, no timestamp, no text of any kind, faces not recognisable. The tall rack leaning and collapsing, crates falling into the aisle, the forklift reversing away.
```
