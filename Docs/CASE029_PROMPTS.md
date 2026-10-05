# Dosya #029 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (5 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Ishida Naoko** → `Bube/Characters/naoko`

```
A 39-year-old Japanese woman, straight black hair to the shoulders tucked behind one ear, pale tired face without makeup, red eyes. Light blue pharmacy cardigan over a white blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Kudō Daisuke** → `Bube/Characters/daisuke`

```
A 44-year-old Japanese man, very short black hair, broad face, a small healed scar on the eyebrow, sun-darkened skin, hard stare. Navy mechanic's work jacket with oil stains, collar up. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Fukuda Emiko** → `Bube/Characters/emiko`

```
A 57-year-old Japanese woman, greying hair in a loose low bun, small reading glasses on a chain, calm unreadable face. Dark plum cardigan over a black turtleneck. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Matsui Kei** → `Bube/Characters/matsui`

```
A 50-year-old Japanese man, neatly parted black hair, rimless glasses, polite professional face. White dentist's coat over a light blue shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Ishida Kazuki** → `Bube/Characters/kazuki`

```
A 42-year-old Japanese man, slightly overgrown black hair, ten days of stubble, hollow cheeks, ashamed eyes. Cheap grey sweater over a wrinkled shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case029_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Diş kalıpları** → `Bube/Items/case029_dental` — Cesedin çene röntgeni ile Ishida Kazuki’nin kayıtlı röntgeni yan yana.

```
Forensic evidence photograph, top-down on a lightbox. Two dental panoramic X-ray films side by side, glowing on the white lightbox: one shows several bright metal fillings and a gold crown on a molar, the other shows clean natural teeth with no fillings. A small dental mirror lies between them. No text, no labels, no names, no logos. 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, clinical.
```

2. **Benzin bidonu** → `Bube/Items/case029_can` — Atölyenin arkasında; kırmızı metal bidon, kapağında yanık izi.

```
Forensic evidence photograph, three-quarter view on a neutral grey evidence mat. A dented red metal jerrycan with a scorched black lid, a faint smear of dried dark red on the handle. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no labels, no logos.
```

3. **Taşıma minibüsünün anahtarı** → `Bube/Items/case029_keys` — Atölyedeki alet çekmecesinde, beyaz plastik etiketli tek anahtar.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A single car key on a ring with a blank white plastic tag, lying next to a greasy socket wrench. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case029.jpg`, yatay 16:9)

```
Moody cinematic illustration at dawn in a pine forest outside Chiba: a burned-out sedan smouldering on a dirt track, thin grey smoke rising through tall trees, yellow tape strung loosely between trunks, frost on the ground. A small white delivery van's tyre tracks visible in the mud leading away. Cold teal and ash-grey palette with a dull orange glow, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still at a small rural self-service petrol station at night, one pump under a flat roof, fluorescent light, dark trees behind, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case029_station/01–03`**

01.
```
Grainy colour security camera still at a small rural self-service petrol station at night, one pump under a flat roof, fluorescent light, dark trees behind, no timestamp, no text of any kind, faces not recognisable. A heavy-set man in a work jacket filling a red metal jerrycan at the pump, a dark sedan parked behind him with its lights off.
```
02.
```
Grainy colour security camera still at a small rural self-service petrol station at night, one pump under a flat roof, fluorescent light, dark trees behind, no timestamp, no text of any kind, faces not recognisable. The man carrying the jerrycan to the sedan's boot, glancing at the road.
```
03.
```
Grainy colour security camera still at a small rural self-service petrol station at night, one pump under a flat roof, fluorescent light, dark trees behind, no timestamp, no text of any kind, faces not recognisable. The sedan pulling out toward the forest road; a second vehicle, a small white van, left parked at the edge of the forecourt.
```
