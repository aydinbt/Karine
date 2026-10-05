# Dosya #045 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Marjorie Quinn** → `Bube/Characters/quinn`

```
A 55-year-old white American woman, steel-grey hair in a sleek low chignon, thin arched brows, pale skin, composed calculating grey eyes, small diamond studs. Black tailored trouser suit with a high-collared ivory blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Trevor Kincaid** → `Bube/Characters/trevor`

```
A 31-year-old white American man, longish dark-blond hair pushed back, stubble, bloodshot defensive blue eyes. Wrinkled light blue oxford shirt, open collar, sleeves pushed up. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Lupe Ochoa** → `Bube/Characters/ochoa`

```
A 49-year-old Mexican-American woman, dark hair in a neat bun, round gentle face, frightened careful brown eyes. Slate-grey housekeeper's tunic with a small white collar. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Sid Abrams** → `Bube/Characters/abrams`

```
A 52-year-old white American man, close-cropped dark hair, heavy brow, broken-nosed boxer's face, watchful eyes. Black suit, black shirt, a coiled earpiece wire with no visible brand. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case045_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **İnsülin kalemi** → `Bube/Items/case045_pen` — Kurbanın masasındaki kalem; içindeki kartuşun etiketi kalemin standardıyla uyuşmuyor.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A grey reusable insulin injection pen unscrewed into two halves, a small glass insulin cartridge beside it with a differently coloured cap band, both labels blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no brand names, no logos.
```

2. **Masa ses kaydedicisi** → `Bube/Items/case045_recorder` — Kurbanın çalışma masasının çekmecesinde, sesle çalışan küçük bir kayıt cihazı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A small black digital voice recorder with a tiny red light, a short microphone grille, lying beside an open leather desk drawer, its screen blank. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Boş kartuş kutusu** → `Bube/Items/case045_vials` — Bir el çantasının yan cebinden çıkan, iki kartuşu eksik küçük karton kutu.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A small white pharmacy carton opened to show a plastic tray with five slots, three holding glass insulin cartridges with bright coloured cap bands and two empty, next to the open side pocket of a black leather handbag. Labels blurred, unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case045.jpg`, yatay 16:9)

```
Moody cinematic illustration of a Gold Coast penthouse study in Chicago at night: floor-to-ceiling windows overlooking the dark lake and the glittering Lake Shore Drive, a big walnut desk lit by a green banker's lamp, a leather chair pushed back, an insulin pen lying beside a glass of water, framed photos of freight trucks on the wall. Deep green, gold and midnight blue palette, painterly graphic-novel style for a detective game, chapter finale mood. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still in the private lift lobby of a luxury penthouse at night, marble floor, a brass lift door, a console table with a vase, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case045_lobby/01–03`**

01.
```
Grainy colour security camera still in the private lift lobby of a luxury penthouse at night, marble floor, a brass lift door, a console table with a vase, no timestamp, no text of any kind, faces not recognisable. A woman in a dark trouser suit arriving from the lift with a black leather handbag, a man in a black suit opening the apartment door for her.
```
02.
```
Grainy colour security camera still in the private lift lobby of a luxury penthouse at night, marble floor, a brass lift door, a console table with a vase, no timestamp, no text of any kind, faces not recognisable. The same woman standing alone at the console table, taking something small from her handbag and slipping it into her jacket pocket.
```
03.
```
Grainy colour security camera still in the private lift lobby of a luxury penthouse at night, marble floor, a brass lift door, a console table with a vase, no timestamp, no text of any kind, faces not recognisable. The same woman leaving into the lift, the man in the black suit holding the lift door.
```
