# Dosya #073 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (7 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Victor Quinlan** → `Bube/Characters/quinlan`

```
A 56-year-old Anglo-Australian man, slicked-back dark hair greying at the temples, a long composed face, thin lips, cold grey eyes. An immaculate black suit, white shirt, dark tie, no pin or logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Fiona Marlowe** → `Bube/Characters/marlowe`

```
A 41-year-old Australian woman, shoulder-length light brown hair, a pale anxious face, careful hazel eyes. A cream silk blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Kiran Naidoo** → `Bube/Characters/naidoo`

```
A 35-year-old South African Indian-Australian man, short black hair, thin glasses, a tired clever face. A steel-blue button-down shirt with rolled sleeves. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Tamsin Grey** → `Bube/Characters/tamsin`

```
A 50-year-old Australian woman, long grey hair in a ponytail, a worn kind face, startled blue eyes. A teal cleaning tunic with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Emeka Okeke** → `Bube/Characters/okeke`

```
A 39-year-old Nigerian-Australian man, shaved head, a calm square face, alert brown eyes. A dark charcoal security blazer with no badge or logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Brodie Walsh** → `Bube/Characters/brodie`

```
A 45-year-old Australian man, short brown hair, a broad impassive face, a scar through one eyebrow. A plain black driver's suit. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

7. **Yindi Marrawa** → `Bube/Characters/yindi`

```
A 32-year-old Aboriginal Australian woman, long dark curly hair, a sharp searching face, direct brown eyes. A rust-coloured jacket over a black top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case073_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **İnsülin kalemi** → `Bube/Items/case073_pen` — Koordinatörün insülin kalemi; dozu en yükseğe çevrilmiş, iki kez boşaltılmış.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A generic grey-and-orange insulin injection pen with its cap off and the dose dial turned to maximum, a used needle tip beside it in a small sharps tube, inside an open clear evidence bag. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Veda notu** → `Bube/Items/case073_note` — Bilgisayardan basılmış, imzasız bir veda notu.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A single sheet of white printer paper with a few short typed lines, illegible, no signature, folded once, lying beside a modern office desk lamp base. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Kurye makbuzu** → `Bube/Items/case073_courier` — Uluslararası kurye makbuzu; varış Budapeşte.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A carbonless international courier receipt slip with a barcode and handwritten address boxes, all text illegible, beside a small padded envelope big enough for a USB stick, torn open. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case073.jpg`, yatay 16:9)

```
Moody cinematic illustration of a glass office tower in Docklands, Melbourne at night: the 38th floor is the only lit floor, a single office with a desk lamp glowing and a figure-less chair, the harbour and Bolte Bridge lit below, rain streaking the glass facade, a lone tram crossing far below. Glass blue, harbour black and lamp gold palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```
