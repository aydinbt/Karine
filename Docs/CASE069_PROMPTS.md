# Dosya #069 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Alistair Fenwick** → `Bube/Characters/fenwick`

```
A 57-year-old Anglo-Australian man, silver hair neatly side-parted, a tanned smooth face, practised smile that stops at his pale eyes. A crisp white linen shirt, sunglasses hooked in the collar. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Jarrah Collins** → `Bube/Characters/jarrah`

```
A 33-year-old Aboriginal Australian man, short dark curly hair, a calm observant face with a short beard, steady brown eyes. An olive-green ranger shirt with no badge or logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Eloise Marr** → `Bube/Characters/eloise`

```
A 40-year-old Australian woman, long wavy dirty-blonde hair, paint smudges on her cheek, a dreamy tired face, grey eyes. A plum-coloured smock. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Quoc Bui** → `Bube/Characters/quoc`

```
A 46-year-old Vietnamese-Australian man, short black hair, a lined patient face, oil on his hands, wary eyes. A slate-blue mechanic's work shirt with no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case069_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kürek sapı** → `Bube/Items/case069_oar` — Marinanın kiralık kanolarından birinin küreği; sapta kurbanın saç teli.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A wooden canoe paddle with a scuffed green-painted blade, the grip end showing a small dark stain and a single long grey hair caught in a splinter, river mud dried along the shaft. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **İskele kapısı kaydı** → `Bube/Items/case069_gate` — Marinanın elektronik kapı kaydı; 20.00’den sonra giriş yok.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A printed access-control log page with columns of entry times, all text illegible, beside a grey plastic RFID key fob on a lanyard. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Su numunesi şişeleri** → `Bube/Items/case069_sample` — Kurbanın evindeki etiketli su numuneleri; marinanın gider çıkışından.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. Four small glass sample bottles with murky brownish river water and blank paper labels, in a wooden rack, one bottle with a faint oily sheen on top. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case069.jpg`, yatay 16:9)

```
Moody cinematic illustration of a houseboat moored on the Maribyrnong River in Melbourne at night: a small timber floating home with a lit window and potted plants on its deck, dark rippling water reflecting the light, a marina of white boats and a locked gangway gate in the background, eucalyptus trees on the riverbank and a distant city glow. Deep river green, ink blue and warm window amber palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```
