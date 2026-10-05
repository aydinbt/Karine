# Dosya #064 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Serge Paquette** → `Bube/Characters/paquette`

```
A 57-year-old Québécois man, grey hair under a fur-lined trapper hat pushed back, a broad weathered face, a confident jaw, cold small blue eyes. A dark green insulated work parka with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Ryan O’Neill** → `Bube/Characters/oneill`

```
A 34-year-old Irish-Canadian man, ginger hair and beard flecked with snow, a pale shocked face. A hi-vis orange winter jacket with reflective strips, no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Martine Lachance** → `Bube/Characters/lachance`

```
A 45-year-old Québécoise woman, brown hair in a ponytail under a knit hat, a sharp intelligent face, grieving angry eyes. A navy blue down parka. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Yusra Haddadi** → `Bube/Characters/yusra`

```
A 28-year-old Moroccan-Canadian woman, dark hair under a burgundy headscarf, a headset around her neck, a careful worried face. A burgundy fleece. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case064_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Denetçinin tableti** → `Bube/Items/case064_tablet` — Isabelle Moreau’nun belediye tableti; şirketin tuz deposundaki tuz yığınının içinden çıktı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A rugged grey municipal field tablet in a rubber case, the screen cracked and crusted with coarse road salt crystals, a few salt grains scattered around it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Filo GPS dökümü** → `Bube/Items/case064_gps` — Şirketin gece GPS dökümü; sahibinin kamyoneti 00.30–02.00 arası depoda ‘park’.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A printed fleet tracking report with a street map grid and coloured route lines, one vehicle shown as a single stationary dot, all street names and numbers blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Kepçenin koltuk minderi** → `Bube/Items/case064_loader` — Şirketin küçük kepçesinin sürücü koltuğu; minderde Isabelle’in atkısından kırmızı yün lifleri.

```
Forensic evidence photograph, close-up of a worn black vinyl seat of a compact wheel loader cab, a few bright red wool fibres caught in a split seam, evidence tape marking them, a frosted cab window behind. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case064.jpg`, yatay 16:9)

```
Moody cinematic illustration of a Montreal back alley during a heavy night snowstorm: a huge orange snow blower machine stopped with its chute aimed into a dump truck, headlights cutting through falling snow, a tall snowbank with a single red knitted scarf trailing out of it, spiral iron staircases on the brick houses, orange sodium street light. Snow white, sodium orange and night blue palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```
