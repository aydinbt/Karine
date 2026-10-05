# Dosya #032 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Julien Mercier** → `Bube/Characters/mercier`

```
A 54-year-old French man, wavy salt-and-pepper hair swept back, neatly trimmed grey beard, half-moon reading glasses pushed up on his head, cultured composed expression with cold eyes. Burgundy wool cardigan over a crisp white shirt and a dark silk scarf. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Inès Benali** → `Bube/Characters/benali`

```
A 28-year-old French woman of North African descent, black curly hair tied up in a bun, strong eyebrows, tired defiant eyes. Grey art-handler's work coat over a black t-shirt, white cotton gloves tucked in the breast pocket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Hugo Lefèvre** → `Bube/Characters/hugo`

```
A 33-year-old French man, messy chestnut hair, three-day stubble, red sleepless eyes, angry grief. Olive-green field jacket over a dark grey hoodie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Colette Arnaud** → `Bube/Characters/colette`

```
A 61-year-old French woman, silver-white hair in a soft chignon, tortoiseshell glasses on a beaded chain, sharp kind eyes. Deep wine-red knitted jacket over a cream blouse with a cameo brooch. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Bastien Morel** → `Bube/Characters/bastien`

```
A 45-year-old French man of Caribbean descent, close-cropped black hair, calm broad face, tired eyes. Dark navy night-guard jacket with plain shoulder epaulettes, no insignia or text. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case032_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Bronz kitap desteği** → `Bube/Items/case032_bookend` — Asya bölümü ofisindeki çift bronz destekten biri; tabanında silinmiş kan izi.

```
Forensic evidence illustration, three-quarter view on a neutral grey evidence mat. A heavy antique bronze bookend shaped like a seated foo dog, dark green patina, the flat base showing faint brownish smears revealed under a forensic light, a few fibres caught at the edge. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

2. **Lot 47 köken mektubu** → `Bube/Items/case032_letter` — Fildişi kâğıda daktiloyla yazılmış, mavi mürekkep imzalı uzman mektubu; imzanın üstünde ‘E. V.’ baş harfleri.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A single sheet of aged ivory typewriter paper with a short typed paragraph, the type deliberately blurred and unreadable, a looping blue-ink signature at the bottom with only two clear initials 'E.V.' readable above it, a faint embossed seal without words in the corner. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no other readable text, no logos.
```

3. **Kart panosu** → `Bube/Items/case032_badge` — Kasa dairesinin girişinde, taşıyıcı kartlarının asılı durduğu açık pano.

```
Forensic evidence illustration, close frontal view of a small wall-mounted board with six metal hooks, five of them holding plain white plastic access cards on grey lanyards, one hook empty, fingerprint powder dusted around the empty hook. Neutral flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text on the cards, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case032.jpg`, yatay 16:9)

```
Moody cinematic illustration of a grand Parisian auction house façade at dusk on a narrow street near the Seine: tall arched windows lit gold, a banner without any text above the door, rain-slick cobblestones, a single figure-free grey umbrella left leaning by the door. Deep blue, gold and wet-stone grey palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos, no real company names.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still in a basement corridor of an auction house leading to a heavy steel vault door, crates stacked on one side, harsh overhead light, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case032_vault/01–03`**

01.
```
Grainy black-and-white security camera still in a basement corridor of an auction house leading to a heavy steel vault door, crates stacked on one side, harsh overhead light, no timestamp, no text of any kind, faces not recognisable. A figure in a grey handler's work coat with the hood-like collar turned up walking toward the vault door, swiping a card, carrying nothing.
```
02.
```
Grainy black-and-white security camera still in a basement corridor of an auction house leading to a heavy steel vault door, crates stacked on one side, harsh overhead light, no timestamp, no text of any kind, faces not recognisable. The vault door ajar, a thin light from inside, the corridor empty.
```
03.
```
Grainy black-and-white security camera still in a basement corridor of an auction house leading to a heavy steel vault door, crates stacked on one side, harsh overhead light, no timestamp, no text of any kind, faces not recognisable. The same grey-coated figure leaving the vault quickly, holding a small wooden box against the chest, face turned away from the camera.
```
