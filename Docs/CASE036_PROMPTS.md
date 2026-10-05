# Dosya #036 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (7 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Hélène de Vauclair** → `Bube/Characters/helene`

```
A 63-year-old French woman, silver-blonde hair in an elegant French twist, fine wrinkles, cool composed grey eyes, pearl earrings. Navy silk blouse under a dark tailored cardigan. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Gaspard de Vauclair** → `Bube/Characters/gaspard`

```
A 34-year-old French man, tousled light-brown hair, unshaven, resentful tired eyes, a slightly crooked aristocratic nose. Slate-blue cashmere jumper over an open-collared shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Arnaud Petit** → `Bube/Characters/petit`

```
A 52-year-old French man, short dark hair greying at the temples, thin moustache, pale careful face, very still eyes. Black sommelier's waistcoat over a white shirt, a small silver tastevin cup on a chain around his neck. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Rosa Pinto** → `Bube/Characters/pinto`

```
A 58-year-old Portuguese woman, dark hair streaked with grey in a practical bun, round kind face, worried dark eyes. Charcoal housekeeper's dress with a white collar. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Yves Caron** → `Bube/Characters/caron`

```
A 66-year-old French man, white wavy hair, bushy white eyebrows, red-veined cheeks, shrewd amused eyes. Brown corduroy jacket over a mustard waistcoat and a knitted tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Marcus Doyle** → `Bube/Characters/doyle`

```
A 45-year-old American man, sandy-brown hair cut short, square jaw, polite professional smile that doesn't reach his eyes. Navy blazer over a light blue open-collar shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

7. **Antoine Brassard** → `Bube/Characters/brassard`

```
A 57-year-old French man, tall and lean, iron-grey hair cut close, hawk-like nose, thin unreadable smile. Charcoal double-breasted suit, white shirt, no tie, a heavy gold signet ring visible only as a glint near the collar where his hand rests against it. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case036_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kırık magnum şişe** → `Bube/Items/case036_magnum` — Mahzen zemininde, boynundan kırılmış bir magnum; etiketi 1961 tarihli.

```
Forensic evidence illustration, three-quarter view on a neutral grey evidence mat in a stone wine cellar. A large dark green magnum wine bottle broken at the shoulder, its neck lying separately, red wine stains on the glass and a faint smear of blood on the heavy base, an old-looking cream label with blurred unreadable lettering. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Boş etiketler** → `Bube/Items/case036_labels` — Sommelier’nin kiraladığı depoda, kutularca eskitilmiş boş şarap etiketi ve mantar.

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. An open cardboard box filled with dozens of blank aged-looking cream wine labels with no printing, a bundle of old corks held with an elastic band, a small bottle of brown tea-stain liquid and a soft brush. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

3. **Şarap asansörü** → `Bube/Items/case036_lift` — Mahzenden yemek salonuna çıkan eski el asansörü; kabin tabanında toz izi ve şarap damlası.

```
Forensic evidence illustration, looking into an old wooden dumbwaiter shaft opened in a stone wall, the small wooden cabin floor showing scuffed dust marks and two dark red drops, a rope pulley above, harsh flash lighting. 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case036.jpg`, yatay 16:9)

```
Moody cinematic illustration of a small French château near Fontainebleau forest at night: pale stone façade, tall windows lit warm gold on the ground floor, a gravel courtyard with three expensive dark cars, a low arched cellar door at the side of the building half hidden by ivy, mist rising from the forest behind. Deep green, gold and midnight blue palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still in a château's stone corridor at night outside a heavy wooden cellar door with iron bands, an antique wall lamp, flagstone floor, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case036_cellar/01–03`**

01.
```
Grainy colour security camera still in a château's stone corridor at night outside a heavy wooden cellar door with iron bands, an antique wall lamp, flagstone floor, no timestamp, no text of any kind, faces not recognisable. An older man in a dinner jacket unlocking the cellar door and going down alone, the door closing behind him.
```
02.
```
Grainy colour security camera still in a château's stone corridor at night outside a heavy wooden cellar door with iron bands, an antique wall lamp, flagstone floor, no timestamp, no text of any kind, faces not recognisable. A man in a black waistcoat walking past the closed cellar door without stopping, carrying a silver tray.
```
03.
```
Grainy colour security camera still in a château's stone corridor at night outside a heavy wooden cellar door with iron bands, an antique wall lamp, flagstone floor, no timestamp, no text of any kind, faces not recognisable. The corridor empty for a long time; the cellar door still closed and locked.
```
