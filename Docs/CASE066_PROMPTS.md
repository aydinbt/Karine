# Dosya #066 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Graham Whitfield** → `Bube/Characters/whitfield`

```
A 56-year-old English-Canadian man, silver hair swept back, a tanned lean face, a thin smile and pale blue eyes that do not smile. An impeccable charcoal pinstripe suit with a silk tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Anne-Marie Leduc** → `Bube/Characters/leduc`

```
A 60-year-old Québécoise woman, chestnut hair in a precise bob, a powerful composed face, tired eyes behind tortoiseshell glasses. A burgundy tailored suit. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Chidi Obi** → `Bube/Characters/obi`

```
A 33-year-old Nigerian-Canadian man, short hair, a neat beard, a calm technical face, a badge reel clipped to his belt with a blank card. A steel-blue polo shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Luc-Antoine Renaud** → `Bube/Characters/renaud`

```
A 50-year-old Québécois man, short dark hair, a square jaw, an alert professional face. A black security blazer with no badge. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Mirela Popescu** → `Bube/Characters/mirela`

```
A 29-year-old Romanian-Canadian woman, long dark brown hair, a pale anxious face, red eyes. A lavender-grey blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case066_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Gaz sistemi manuel tetik anahtarı** → `Bube/Items/case066_key` — Yangın söndürme sisteminin manuel tetik anahtarı; başkan yardımcısının spor kulübü dolabındaki squash çantasından çıktı.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small red-tagged fire suppression system manual release key with a numbered tag left blank, lying on top of an open sports bag next to a squash racket and a folded towel. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Yönetim kurulu toplantı tutanağı** → `Bube/Items/case066_minutes` — Gece toplantısının tutanağı; başkan yardımcısı ‘video bağlantıyla, Toronto’dan’ 20.00–21.30.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A corporate board meeting minutes document on heavy white paper with a blank header block, an attendance list with one line marked by a small pencil tick, all text illegible, a silver fountain pen beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Kovač’ın tablosu** → `Bube/Items/case066_table` — Baş aktüerin masasından çıkan çıktı; lehtar sütununda tek bir isim tekrar ediyor.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A printed spreadsheet page of dense numeric columns with one entire column highlighted in yellow, the same short entry repeated down every row, all figures and words blurred and unreadable, a red pen circle around the total. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case066.jpg`, yatay 16:9)

```
Moody cinematic illustration of a glass skyscraper in downtown Montreal at night in winter: one floor high up glowing red behind its windows with emergency strobe light, a faint white haze pressed against the glass, the rest of the tower dark, Mount Royal's cross lit in the distance, snow falling over the city lights. Cold blue glass, emergency red and snow white palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still of a corporate office corridor on a high floor at night, a glass door to a server archive room with a red warning lamp above it, carpet, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case066_floor/01–03`**

01.
```
Grainy black-and-white security camera still of a corporate office corridor on a high floor at night, a glass door to a server archive room with a red warning lamp above it, carpet, no timestamp, no text of any kind, faces not recognisable. A man in a pinstripe suit with a laptop under his arm walking down the corridor towards a small meeting room, glancing at the archive room door.
```
02.
```
Grainy black-and-white security camera still of a corporate office corridor on a high floor at night, a glass door to a server archive room with a red warning lamp above it, carpet, no timestamp, no text of any kind, faces not recognisable. The same man standing at the archive room door, closing it from outside and turning something small in a red wall panel beside it.
```
03.
```
Grainy black-and-white security camera still of a corporate office corridor on a high floor at night, a glass door to a server archive room with a red warning lamp above it, carpet, no timestamp, no text of any kind, faces not recognisable. The red warning lamp above the archive door flashing; the corridor empty; the meeting room door at the end closed.
```
