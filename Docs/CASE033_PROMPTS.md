# Dosya #033 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Nadine Fontaine** → `Bube/Characters/nadine`

```
A 55-year-old French woman, light brown hair streaked with grey tied back loosely, flour on one cheek, red-rimmed eyes, stunned expression. Cream baker's apron over a beige knitted jumper. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Rayan Haddad** → `Bube/Characters/rayan`

```
A 22-year-old French man of Maghrebi descent, short black hair under a white baker's cap, thin moustache, shocked young face, flour-dusted. White baker's jacket with rolled sleeves visible at the shoulders. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Sophie Garnier** → `Bube/Characters/garnier`

```
A 41-year-old French woman, dark brown hair in a sleek ponytail, discreet makeup, tense polite smile, eyes looking slightly away. Charcoal blazer over a black top, a thin silver necklace. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case033_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kâğıt un torbası** → `Bube/Items/case033_flour` — ‘Müşterinin kendi unu’ diye getirilen kraft kâğıt torba; ağzı katlanmış, içinde sarımsı toz.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A plain brown kraft paper flour bag about 30 cm tall with its top folded over twice, a little pale yellowish powder spilled at its base, no printing or label on the bag. A forensic swab tube beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

2. **Yarım somun** → `Bube/Items/case033_loaf` — Tezgâhta, bir dilimi kesilmiş küçük yuvarlak glütensiz somun.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A small round rustic loaf of bread with a pale dense crumb, one slice cut from it and the slice lying beside it with a bite missing, a bread knife next to them. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

3. **Nakit zarfı** → `Bube/Items/case033_envelope` — İkram koordinatörünün arabasının torpidosunda, içinde 5.000 avro bulunan beyaz zarf.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A plain white envelope lying open with a thick stack of generic banknotes partly pulled out, the notes' details blurred so no denomination or text is readable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case033.jpg`, yatay 16:9)

```
Moody cinematic illustration of a small corner bakery on a sloping Belleville street in Paris at 4 a.m.: warm yellow light pouring from the back kitchen window, a delivery scooter parked outside, steam from a vent, graffiti-free stone walls, wet pavement, blue pre-dawn sky. Warm amber against cold blue palette, painterly graphic-novel style for a detective game. No people, no readable shop sign text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still above the back door of a small Parisian bakery at night, a narrow courtyard with bins, a delivery van partly visible, wet cobblestones, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case033_backdoor/01–03`**

01.
```
Grainy colour security camera still above the back door of a small Parisian bakery at night, a narrow courtyard with bins, a delivery van partly visible, wet cobblestones, no timestamp, no text of any kind, faces not recognisable. A woman in a dark blazer stepping out of a small van carrying a brown paper flour bag against her hip.
```
02.
```
Grainy colour security camera still above the back door of a small Parisian bakery at night, a narrow courtyard with bins, a delivery van partly visible, wet cobblestones, no timestamp, no text of any kind, faces not recognisable. The woman handing the paper bag to a young man in white at the bakery's back door, pointing at it.
```
03.
```
Grainy colour security camera still above the back door of a small Parisian bakery at night, a narrow courtyard with bins, a delivery van partly visible, wet cobblestones, no timestamp, no text of any kind, faces not recognisable. The woman walking back to the van alone, wiping her hands on a tissue.
```
