# Dosya #065 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (6 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Étienne Beaulieu** → `Bube/Characters/beaulieu`

```
A 46-year-old French-Canadian man, neat dark hair with a side part, clean-shaven, a handsome tense face, quick nervous brown eyes. A navy technical jacket over a white shirt, a site helmet under one arm, no logo. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Grace Kwan** → `Bube/Characters/kwan`

```
A 38-year-old Chinese-Canadian woman, long straight black hair, a camera strap across her chest, a composed grieving face. A charcoal wool coat. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Alain Rousseau** → `Bube/Characters/rousseau`

```
A 59-year-old Québécois man, short grey hair, heavy eyelids, a tired honest face. A dark navy security guard parka with no badge. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Farah Siddiqui** → `Bube/Characters/farah`

```
A 30-year-old Pakistani-Canadian woman, long dark hair loose, a nose stud, a fierce passionate face. A green canvas utility jacket with patches that have no text. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Sylvie Morin** → `Bube/Characters/morin`

```
A 61-year-old Québécoise woman, silver hair in a sleek low bun, a stern elegant face, pale grey eyes. A black cashmere coat. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Max Gendron** → `Bube/Characters/gendron`

```
A 23-year-old French-Canadian man, sandy curly hair, a young open face, a waiter's notepad in his shirt pocket. A white waiter's shirt with a black vest. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case065_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kask kamerası hafıza kartı** → `Bube/Items/case065_card` — Kurbanın kask kamerasının hafıza kartı; proje maketinin oyuk kaidesinin içinden çıktı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A tiny black microSD memory card in a clear evidence bag beside the upturned hollow base of a white architectural scale model, a small cavity cut into the foam board underneath. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

2. **Restoran fişi** → `Bube/Items/case065_receipt` — Proje müdürünün cebinden çıkan fiş; 21.15, iki kişilik akşam yemeği, kartla ödeme.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A crumpled thermal restaurant receipt smoothed flat, faded print lines all illegible, a small wine stain in one corner, beside a folded paper napkin. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

3. **Hazne kapağı** → `Bube/Items/case065_hatch` — Silonun en üst katındaki tahıl haznesi kapağı; menteşesindeki yeni çizikler.

```
Forensic evidence photograph, close-up of a heavy square steel hatch cover on a dusty concrete floor at the top of an old grain silo, its hinge showing fresh bright scratch marks through old rust, a dark drop below the open edge. Flat forensic lighting, photorealistic, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case065.jpg`, yatay 16:9)

```
Moody cinematic illustration of a colossal abandoned concrete grain silo complex in Montreal's Old Port at dawn: rows of tall cylindrical silos, rusted conveyor galleries bridging them, construction scaffolding and a crane on one side, a single open hatch lit from inside at the very top, mist over the frozen river and the city skyline behind. Concrete grey, rust orange and dawn pink palette, painterly graphic-novel style for a detective game. No people visible, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy black-and-white security camera still of a construction site gate at the foot of a huge concrete grain silo at night, chain-link fencing, floodlight, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case065_gate/01–03`**

01.
```
Grainy black-and-white security camera still of a construction site gate at the foot of a huge concrete grain silo at night, chain-link fencing, floodlight, no timestamp, no text of any kind, faces not recognisable. A man in a technical jacket with a helmet under his arm walking quickly through the site gate, alone.
```
02.
```
Grainy black-and-white security camera still of a construction site gate at the foot of a huge concrete grain silo at night, chain-link fencing, floodlight, no timestamp, no text of any kind, faces not recognisable. A second figure in a long coat and helmet, carrying a camera, already inside, waiting at the silo's stair tower door.
```
03.
```
Grainy black-and-white security camera still of a construction site gate at the foot of a huge concrete grain silo at night, chain-link fencing, floodlight, no timestamp, no text of any kind, faces not recognisable. The man in the technical jacket hurrying out through the gate alone, pulling his hood up.
```
