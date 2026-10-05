# Dosya #027 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Takeda Yuki** → `Bube/Characters/yuki`

```
A 29-year-old Japanese woman, dark brown hair in a neat low bun with a few loose strands, tired but composed face, small silver stud earrings. Dark red pachinko parlour staff vest over a white shirt with a thin black ribbon tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Arai Hideo** → `Bube/Characters/hideo`

```
A 52-year-old Japanese man, short black hair greying at the temples, heavy bags under his eyes, unshaven, anxious frown. Black suit jacket, loosened grey tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Satō Minoru** → `Bube/Characters/sato`

```
A 66-year-old Japanese man, thin grey hair combed over, deep smile lines, thick black-framed glasses, calm stubborn face. Faded blue zip-up windbreaker over a beige polo shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case027_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Depo termostatı** → `Bube/Items/case027_thermo` — Ödül deposunun klima paneli; 14 °C’ye ve ‘sürekli’ moduna ayarlanmış.

```
Forensic evidence illustration, close frontal view of a wall-mounted air conditioner control panel in a dim storage room, plain beige plastic, small dark LCD screen showing only a snowflake symbol and the number 14, a fan symbol at maximum, no other text. A faint dusting of fingerprint powder around the buttons. Neutral flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no brand names, no logos, no words.
```

2. **Ödül tepsisi** → `Bube/Items/case027_tray` — Kasa arkasındaki plastik tepsi; içinde 180 jeton ve bir kopmuş kasiyer yaka kartı klipsi.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A shallow black plastic tray holding about two hundred small shiny silver steel balls (pachinko balls) spilled unevenly, and among them a broken small metal badge clip with a torn red fabric strap. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

3. **Ödül faturaları** → `Bube/Items/case027_ledger` — Kurbanın masasında, sarı yapışkan notla işaretlenmiş on iki fatura.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small stack of twelve generic printed invoices, deliberately blurred so no text is readable, held with a black binder clip, one yellow sticky note on top with a single hand-drawn question mark. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case027.jpg`, yatay 16:9)

```
Moody cinematic illustration of a narrow Tokyo side street at 5 a.m.: a closed pachinko parlour with its neon signs switched off, metal shutters half down, one fluorescent light still burning inside, wet asphalt reflecting a vending machine glow, a single bicycle leaning against the wall. Teal, magenta and grey palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos, no real brand names.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white ceiling security camera still inside a closed pachinko parlour at night, rows of pachinko machines receding, a cashier counter at the right, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case027_counter/01–03`**

01.
```
Grainy black-and-white ceiling security camera still inside a closed pachinko parlour at night, rows of pachinko machines receding, a cashier counter at the right, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable. A woman in a staff vest walking from the cashier counter toward a back storage door, carrying an empty plastic tray.
```
02.
```
Grainy black-and-white ceiling security camera still inside a closed pachinko parlour at night, rows of pachinko machines receding, a cashier counter at the right, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable. The storage door closed; the cashier counter empty; one pachinko machine at the end of the row still lit.
```
03.
```
Grainy black-and-white ceiling security camera still inside a closed pachinko parlour at night, rows of pachinko machines receding, a cashier counter at the right, harsh fluorescent light, no timestamp, no text of any kind, faces not recognisable. The same woman returning to the counter, smoothing her vest, the tray now full; she bends down behind the counter.
```
