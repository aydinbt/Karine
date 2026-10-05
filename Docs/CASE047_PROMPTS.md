# Dosya #047 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Enzo Ferraro** → `Bube/Characters/enzo`

```
A 44-year-old Neapolitan man, thick dark hair greying at the temples, heavy moustache, flour-dusted forearms, evasive brown eyes. White pizzaiolo jacket with rolled sleeves. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Rosaria Amato** → `Bube/Characters/rosaria`

```
A 39-year-old Neapolitan woman, long dark brown hair tied back, red-rimmed grieving eyes, small gold earrings. Dark burgundy cardigan over a black top. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Moussa Diop** → `Bube/Characters/moussa`

```
A 29-year-old Senegalese man, short black hair, slim face, calm attentive dark eyes, a small beard. Green T-shirt under an open light jacket, a bundle of folded umbrellas over his shoulder. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Rocco Greco** → `Bube/Characters/rocco`

```
A 57-year-old Neapolitan man, slicked-back grey hair, tanned heavy face, a pinky ring, a thin smile that does not reach his eyes. Dark charcoal suit with an open-collared black shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Lucia Bianco** → `Bube/Characters/lucia`

```
A 23-year-old Neapolitan woman, long black hair in a high ponytail, nervous wide eyes, a small nose stud. Black waitress shirt with a short apron. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case047_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Soğuk oda iç kolu** → `Bube/Items/case047_handle` — Bir arabanın torpidosundan çıkan, sökülmüş krom kapı kolu ve iki vida.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A chrome interior safety release handle of a walk-in cold room, unscrewed, with two loose screws and a small flat screwdriver beside it, faint frost marks and flour dust on the chrome. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

2. **Soğuk oda termostatı** → `Bube/Items/case047_thermo` — Soğuk odanın dış duvarındaki termostat; gece -18’e çevrilmiş.

```
Forensic evidence photograph, close-up on a stainless steel wall. A rectangular digital thermostat panel of a walk-in cold room with a blank dark display, a single dial turned to the far blue end, flour fingerprints on the dial. Flat forensic lighting, photorealistic, no readable text, no numbers, no logos.
```

3. **Kasa defteri** → `Bube/Items/case047_ledger` — Kasanın altından çıkan küçük kareli defter; son sayfada iki sütun rakam ve bir çizgi.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A small worn squared-paper notebook lying open, two columns of handwritten figures on the last page heavily crossed out with one firm pen line, the writing blurred and unreadable, a pencil beside it. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case047.jpg`, yatay 16:9)

```
Moody cinematic illustration of an old pizzeria in the Forcella quarter of Naples at night, seen from inside the kitchen: a dome-shaped wood-fired oven glowing with dying embers, flour on a marble counter, and in the back a stainless steel walk-in cold room door slightly ajar, cold blue mist spilling out across the tiles. Warm orange embers against icy blue, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white street security camera still of a narrow Naples alley at night, an old pizzeria's rolling shutter half down, laundry lines above, a scooter parked, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case047_street/01–03`**

01.
```
Grainy black-and-white street security camera still of a narrow Naples alley at night, an old pizzeria's rolling shutter half down, laundry lines above, a scooter parked, no timestamp, no text of any kind, faces not recognisable. A heavyset man in a white kitchen jacket pulling the pizzeria's shutter halfway down from outside, then ducking back in under it.
```
02.
```
Grainy black-and-white street security camera still of a narrow Naples alley at night, an old pizzeria's rolling shutter half down, laundry lines above, a scooter parked, no timestamp, no text of any kind, faces not recognisable. The same man coming out under the shutter alone, pulling it fully down and locking it, looking up and down the alley.
```
03.
```
Grainy black-and-white street security camera still of a narrow Naples alley at night, an old pizzeria's rolling shutter half down, laundry lines above, a scooter parked, no timestamp, no text of any kind, faces not recognisable. A thin young man with a bundle of umbrellas on his shoulder standing across the alley, looking at the closed shutter.
```
