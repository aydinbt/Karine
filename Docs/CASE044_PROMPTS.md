# Dosya #044 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (6 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Brent Calloway** → `Bube/Characters/calloway`

```
A 50-year-old white American man, wavy dark-brown hair neatly parted, clean-shaven, handsome controlled face, cold grey eyes. White linen shirt with rolled sleeves under a navy sailing gilet with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Diane Harlan** → `Bube/Characters/diane`

```
A 57-year-old white American woman, champagne-blonde bob, tanned skin, pearl studs, brittle composed blue eyes. Navy and white striped boat-neck top, a cream cardigan over her shoulders. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Skip Morrow** → `Bube/Characters/morrow`

```
A 63-year-old white American man, white beard trimmed short, deeply tanned leathery face, squinting pale eyes. Faded navy captain's polo shirt with no text, a pair of sunglasses hanging on a cord. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Rico Mendes** → `Bube/Characters/mendes`

```
A 24-year-old Puerto Rican-American man, short black curly hair, thin moustache, nervous young dark eyes. White crew T-shirt with no text, a coiled rope over one shoulder. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Arne Jensen** → `Bube/Characters/jensen`

```
A 68-year-old Danish-American man, thin grey hair under a flat cap, ruddy cheeks, steady watery blue eyes. Brown waxed jacket over a cable-knit sweater. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Lauren Cho** → `Bube/Characters/cho`

```
A 36-year-old Korean-American woman, wet black hair pulled back tight, a red mark from a dive mask around her eyes, calm focused expression. Black neoprene wetsuit unzipped to the chest. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case044_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Can yeleği** → `Bube/Items/case044_vest` — Kurbanın üstündeki otomatik şişen yelek; CO₂ kartuşunun yuvası boş.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A deflated red automatic inflatable life vest, still wet, its small inflation mechanism opened to show an empty threaded socket where the gas cartridge should be, the pull tab intact. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Vinç kolu** → `Bube/Items/case044_winch` — Kokpit dolabındaki krom vinç kolu; tutamağın altında silinmeye çalışılmış kan.

```
Forensic evidence illustration, three-quarter view on a neutral grey evidence mat. A chrome sailing winch handle with a black grip, a thin line of dark dried blood in the seam under the grip despite having been wiped, smeared water spots. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **CO₂ kartuşu** → `Bube/Items/case044_cartridge` — Bir sefer çantasının iç cebinden çıkan küçük gaz kartuşu; dişlerinde yeşil yelek boyası.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small silver CO2 gas cartridge with a threaded neck, tiny flakes of red paint caught in the threads, lying next to the open inner zip pocket of a navy leather weekend bag. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case044.jpg`, yatay 16:9)

```
Moody cinematic illustration of a large sailing yacht on Lake Michigan at night, sails half lowered, its deck lights glowing, the Chicago skyline glittering far away across black water, a single deflated red life vest floating on the waves in the foreground, moonlight breaking through clouds. Deep navy, silver and signal-red palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still on a marina pontoon at night, a large sailing yacht moored alongside, deck lights, black water, a gangway, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case044_pier/01–03`**

01.
```
Grainy colour security camera still on a marina pontoon at night, a large sailing yacht moored alongside, deck lights, black water, a gangway, no timestamp, no text of any kind, faces not recognisable. Before departure: a man in a navy gilet stepping aboard the yacht with a leather weekend bag and going below deck alone.
```
02.
```
Grainy colour security camera still on a marina pontoon at night, a large sailing yacht moored alongside, deck lights, black water, a gangway, no timestamp, no text of any kind, faces not recognisable. On return: the yacht docking, a woman in a cardigan and a white-shirted young crewman shouting along the deck.
```
03.
```
Grainy colour security camera still on a marina pontoon at night, a large sailing yacht moored alongside, deck lights, black water, a gangway, no timestamp, no text of any kind, faces not recognisable. The man in the navy gilet stepping off the yacht with the same leather bag and walking quickly up the pontoon.
```
