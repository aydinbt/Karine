# Dosya #038 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Antoine Brassard** → `Bube/Characters/antoine`

```
Same man as file #036: a 57-year-old French man, tall and lean, iron-grey hair cut close, hawk-like nose, thin unreadable smile. Charcoal double-breasted suit, white shirt, no tie, a heavy gold signet ring. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Céline Delorme** → `Bube/Characters/celine`

```
A 38-year-old French woman, long auburn hair pulled back severely, sharp cheekbones, composed but shaken green eyes, small diamond studs. Black tailored blazer over a black silk top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Martin Faure** → `Bube/Characters/faure`

```
A 60-year-old French man, silver hair swept back, florid face, half-moon reading glasses pushed up on his head, theatrical expressive eyebrows. Dark burgundy velvet jacket, white shirt, black bow tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Carla Lopes** → `Bube/Characters/lopes`

```
A 29-year-old Portuguese-French woman, dark hair in a tight low ponytail, olive skin, anxious earnest brown eyes. Plain charcoal art-handler's work jacket, a pair of white cotton gloves tucked into the breast pocket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Vincent Morel** → `Bube/Characters/morel`

```
A 52-year-old French man, dark hair thinning at the crown, rectangular steel glasses, guarded lawyerly expression. Grey three-piece suit, dark blue tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case038_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Beyaz taşıma eldivenleri** → `Bube/Items/case038_gloves` — Kurbanın ellerinden alınan pamuk eldivenler; sağ eldivenin parmak uçlarında yağ lekesi.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A pair of white cotton museum handling gloves laid flat, the fingertips of the right glove showing faint translucent yellowish oil stains, the left glove clean, each in an open clear evidence bag. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

2. **Adrenalin kalemi** → `Bube/Items/case038_pen` — Aramada bulunan otomatik enjektör; kurbanın adı yazılı reçete etiketi, kullanılmamış.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A yellow and grey epinephrine auto-injector pen with its blue safety cap still on, unused, a small pharmacy label with blurred unreadable lettering, lying beside a small amber glass vial of golden oil with a dropper cap. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no brand names, no logos.
```

3. **Chicago manifestosu** → `Bube/Items/case038_manifest` — Lakeshore Freight sevk belgesi; Chicago’da bir depo adresi ve ‘Dr. E. Varga’ imzalı köken mektubu.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A multi-part shipping manifest form with blurred unreadable fields and a stylised lake-and-wave freight company stamp without readable words, paper-clipped to a cream letter with an elegant looping handwritten signature that is illegible. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case038.jpg`, yatay 16:9)

```
Moody cinematic illustration of a grand Paris auction house salesroom at night during a sale: rows of gilt chairs, a raised wooden rostrum with a lit lamp, a single small ancient ceramic bowl on a velvet display stand under a spotlight, a pair of white cotton gloves dropped on the carpet beside it, tall windows showing the lit Paris rooftops. Deep crimson, black and gold palette, painterly graphic-novel style for a detective game, chapter finale mood. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still of a small back-room in a Paris auction house: a steel table with a tray of folded white cotton gloves, coat hooks with dark jackets along one wall, a door to the salesroom, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case038_back/01–03`**

01.
```
Grainy colour security camera still of a small back-room in a Paris auction house: a steel table with a tray of folded white cotton gloves, coat hooks with dark jackets along one wall, a door to the salesroom, no timestamp, no text of any kind, faces not recognisable. A young woman in a work jacket laying out pairs of white gloves on a tray and leaving.
```
02.
```
Grainy colour security camera still of a small back-room in a Paris auction house: a steel table with a tray of folded white cotton gloves, coat hooks with dark jackets along one wall, a door to the salesroom, no timestamp, no text of any kind, faces not recognisable. A tall man in a dark double-breasted suit alone in the room, bending over the glove tray, one hand inside his jacket.
```
03.
```
Grainy colour security camera still of a small back-room in a Paris auction house: a steel table with a tray of folded white cotton gloves, coat hooks with dark jackets along one wall, a door to the salesroom, no timestamp, no text of any kind, faces not recognisable. The same tall man at the coat hooks, his hand slipping into the pocket of a hanging jacket, then leaving.
```
