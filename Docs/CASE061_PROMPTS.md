# Dosya #061 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Marc-André Lavoie** → `Bube/Characters/lavoie`

```
A 49-year-old Québécois man, dark brown hair neatly cut, clean-shaven, a polished smile and restless hazel eyes. A navy blue fleece zip-up with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Denis Ouellet** → `Bube/Characters/ouellet`

```
A 55-year-old Québécois man, grey crew cut, a broken nose, a thick neck, a hard but honest face. A red tracksuit jacket with a whistle on a cord. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Anika Sharma** → `Bube/Characters/anika`

```
A 16-year-old Indo-Canadian girl, long black hair in a tight braid, shocked wide dark eyes, a hockey helmet held under one arm. A white practice jersey with no logo or number. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case061_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kompresör vanası kolu** → `Bube/Items/case061_valve` — Amonyak hattının tahliye vanasının sökülmüş kolu; müdürün masasının alt çekmecesinden çıktı.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A red-painted steel T-handle valve wheel removed from an industrial refrigeration pipe valve, chipped paint, a frosty white residue on the square socket, lying beside an open grey metal desk drawer. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Bakım defteri** → `Bube/Items/case061_log` — Kompresör odasının bakım defteri; dünkü satır ‘vanalar kontrol edildi, normal’.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A battered spiral-bound maintenance logbook with a frost-damp cover, open to a ruled page with tick marks and a short handwritten entry in blue ink, illegible, a pencil tucked in the spiral. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Gaz maskesi askısı** → `Bube/Items/case061_mask` — Kompresör odasının kapısındaki acil durum maskesi askısı; boş.

```
Forensic evidence illustration, three-quarter view of a grey painted cinder-block wall beside a steel door, an empty red emergency equipment hook with a dust outline where a gas mask used to hang, a small blank sign plate above it. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case061.jpg`, yatay 16:9)

```
Moody cinematic illustration of a small neighbourhood indoor ice rink in Montreal before dawn: empty glossy ice under a few working floodlights, hockey nets pushed to the boards, wooden bleachers, a half-open steel door at the far end leaking a faint white chemical mist, frost on the glass. Ice blue, cold white and a single amber warning-light palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```
