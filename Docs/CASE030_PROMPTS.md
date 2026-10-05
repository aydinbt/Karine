# Dosya #030 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Nishida Kōhei** → `Bube/Characters/nishida`

```
A 48-year-old Japanese man, neatly side-parted black hair, square wire glasses, clean-shaven, polite composed expression with a tight jaw. White shirt, dark blue tie, a bright orange emergency-warden armband on the upper arm visible at the shoulder. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Hirano Saki** → `Bube/Characters/hirano`

```
A 33-year-old Japanese woman, black hair in a short bob, small pearl earrings, tired angry eyes. Grey office jacket over a navy blouse, a white disaster-drill helmet pushed back on her head. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Ōno Tadashi** → `Bube/Characters/ono`

```
A 61-year-old Japanese man, grey crew-cut hair, weathered square face, calm attentive eyes. Dark navy security guard uniform jacket with plain shoulder epaulettes, no insignia or text. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case030_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Tatbikat yoklama kâğıdı** → `Bube/Items/case030_sheet` — Kat yoklaması; kurbanın adının yanında farklı renk kalemle atılmış tik.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A single clipboard holding a printed grid form, the text deliberately blurred and unreadable, a column of hand-drawn check marks in black ink with one check mark visibly in blue ink. A cheap blue ballpoint pen lying beside the clipboard. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Yangın söndürücü** → `Bube/Items/case030_ext` — Asma tavanın içinde bulunan kırmızı söndürücü; tabanında ezilme ve kıl.

```
Forensic evidence illustration, three-quarter view on a neutral grey evidence mat. A small red fire extinguisher with a black hose, the round metal base dented on one edge with a few dark hairs caught in the dent, flakes of red paint chipped off. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no labels, no logos.
```

3. **Asma tavan plakası** → `Bube/Items/case030_tile` — 9. kat fotokopi odasının tavan plakası; yerinden oynamış, kenarında parmak izi tozu.

```
Forensic evidence illustration, looking up at an office drop ceiling: one white acoustic ceiling tile pushed slightly aside revealing a dark gap above, black fingerprint powder on its metal frame edge, harsh flash lighting. 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case030.jpg`, yatay 16:9)

```
Moody cinematic illustration of a glass office tower in Nihonbashi, Tokyo, on a bright cold morning: hundreds of office workers in white disaster-drill helmets gathered in the plaza below in neat lines, one emergency stairwell window high on the building lit red. Clean blues and greys with a single red accent, painterly graphic-novel style for a detective game. No readable text, no logos, no real company names.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still in a ninth-floor office elevator lobby during an evacuation drill, elevator doors with a 'do not use' arrow pictogram, a stairwell door at the left, flashing emergency light, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case030_lobby/01–03`**

01.
```
Grainy black-and-white security camera still in a ninth-floor office elevator lobby during an evacuation drill, elevator doors with a 'do not use' arrow pictogram, a stairwell door at the left, flashing emergency light, no timestamp, no text of any kind, faces not recognisable. A man with an armband standing alone in the empty elevator lobby, holding a clipboard, looking toward the stairwell door.
```
02.
```
Grainy black-and-white security camera still in a ninth-floor office elevator lobby during an evacuation drill, elevator doors with a 'do not use' arrow pictogram, a stairwell door at the left, flashing emergency light, no timestamp, no text of any kind, faces not recognisable. The lobby empty; the stairwell door slowly closing.
```
03.
```
Grainy black-and-white security camera still in a ninth-floor office elevator lobby during an evacuation drill, elevator doors with a 'do not use' arrow pictogram, a stairwell door at the left, flashing emergency light, no timestamp, no text of any kind, faces not recognisable. The same man coming back out of the stairwell, without the clipboard, walking quickly toward the photocopy room.
```
