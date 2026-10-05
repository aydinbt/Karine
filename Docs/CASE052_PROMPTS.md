# Dosya #052 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Lorenzo Conte** → `Bube/Characters/lorenzo`

```
A 46-year-old Italian man, neatly cut dark hair with grey at the temples, clean-shaven, handsome hard face, impatient dark eyes, a gold signet ring. Midnight navy tailored suit, open white collar. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Carmela Caruso** → `Bube/Characters/carmela`

```
A 44-year-old Italian woman, long black hair pulled into a severe low bun, dark mourning veil pushed back, strong brows, proud grief-stricken eyes. Black mourning dress with a simple gold cross. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Aldo Ruggiero** → `Bube/Characters/aldo`

```
A 68-year-old Neapolitan port foreman, white hair cropped short, white stubble, deep-lined sun-dark face, loyal sorrowful eyes. Faded blue work jacket with no logo, a flat cap in his hands. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Samir Haddad** → `Bube/Characters/samir`

```
A 39-year-old Moroccan-Italian man, short black hair, neat beard, worried attentive brown eyes. Yellow fumigation technician's overalls with no logo, a gas mask hanging around his neck. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Ottavio Mele** → `Bube/Characters/ottavio`

```
A 33-year-old Italian man, brown hair under a hard hat pushed back, tired young face, nervous eyes. Orange high-visibility crane operator's vest over a dark sweatshirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case052_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Ofisteki fotoğraf** → `Bube/Items/case052_photo` — Don Vittorio’nun ofis duvarından indirilen eski fotoğraf: bir vinç önünde iki kişi; kadının yüzü kadraj dışında, bileğinde eski bir erkek saati.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. An old faded colour photograph from the late 1980s in a simple wooden frame with cracked glass: a middle-aged man in a suit standing beside a woman in front of a harbour crane, the woman's face cut off by the top edge of the frame, only her shoulder and her wrist visible, on her wrist a heavy vintage men's wristwatch with a thick leather strap. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Konteyner mührü** → `Bube/Items/case052_seal` — İlaçlama konteynerinin kapı mührü; numaralı plastik şerit iki kez takılmış.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. Two yellow plastic shipping-container security seals, one cut cleanly with pliers and one intact and still looped, a yellow fumigation warning placard with a skull symbol beside them, any numbers and words blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Fosfin tablet kutusu** → `Bube/Items/case052_tablets` — Bir arabanın bagajından çıkan, yarısı boş alüminyum kutu.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A dented grey aluminium flask-shaped canister for fumigation tablets with its screw lid off, a few small grey tablets spilled on a plastic sheet beside it, a pair of nitrile gloves, all labels blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case052.jpg`, yatay 16:9)

```
Moody cinematic illustration of the container quay of the port of Naples at night: towering stacks of shipping containers, a single red container sealed with yellow fumigation warning tape and a skull placard in the foreground under a floodlight, giant gantry cranes silhouetted against a stormy sky, Vesuvius dark on the horizon, the lit window of a small port office in the distance. Rust red, sodium yellow and storm blue palette, painterly graphic-novel style for a detective game, chapter finale mood. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still of a container quay at night, rows of shipping containers under floodlights, wet asphalt, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case052_quay/01–03`**

01.
```
Grainy black-and-white security camera still of a container quay at night, rows of shipping containers under floodlights, wet asphalt, no timestamp, no text of any kind, faces not recognisable. An elderly man in a long coat walking between container rows beside a younger man in a suit.
```
02.
```
Grainy black-and-white security camera still of a container quay at night, rows of shipping containers under floodlights, wet asphalt, no timestamp, no text of any kind, faces not recognisable. The younger man in a suit closing the doors of a container alone and fixing something to the door handles.
```
03.
```
Grainy black-and-white security camera still of a container quay at night, rows of shipping containers under floodlights, wet asphalt, no timestamp, no text of any kind, faces not recognisable. The same younger man walking quickly away towards a parked car, carrying a small metal canister.
```
