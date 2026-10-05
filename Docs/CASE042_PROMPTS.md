# Dosya #042 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (3 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Wade Tanner** → `Bube/Characters/tanner`

```
A 48-year-old white American man, shaggy dirty-blond hair under a faded trucker cap with no logo, sunburned face, wiry goatee, restless pale eyes. Red flannel shirt with cut-off sleeves over a grey T-shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Bev Hollis** → `Bube/Characters/hollis`

```
A 56-year-old white American woman, permed honey-blonde hair, reading glasses pushed into her hair, smoker's lines around her mouth, shrewd tired eyes. Lilac fleece zip-up over a blouse, a telephone headset around her neck. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Sunny Patel** → `Bube/Characters/patel`

```
A 33-year-old Indian-American man, neat black hair, clean-shaven, earnest worried brown eyes. Teal convenience-store work polo with no text, a name tag with no readable letters. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case042_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **İsli bez** → `Bube/Items/case042_rag` — Bir kamyonun alet kutusundan çıkan bez; bir ucu siyah isle sertleşmiş, egzoz borusu çapında kıvrılmış.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A dirty red shop rag twisted into a tight plug shape, one end stiff and blackened with oily exhaust soot in a circular pattern, the rest stained with grease. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

2. **Egzoz borusu** → `Bube/Items/case042_pipe` — Kurbanın kamyonunun dikey egzoz çıkışı; ağzının içinde kırmızı pamuk lifleri.

```
Forensic evidence photograph, close-up of the top of a chrome vertical exhaust stack on a semi-truck cab, black soot inside the opening with a few red cotton fibres caught on the rim, a yellow evidence marker taped beside it, harsh flash. Photorealistic, no readable text, no logos.
```

3. **Kırık dorse mührü** → `Bube/Items/case042_seal` — Dorsenin kapısındaki numaralı plastik güvenlik mührü; makasla kesilmiş.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A yellow plastic pull-tight trailer security seal cut cleanly through with shears, its printed numbers blurred and unreadable. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case042.jpg`, yatay 16:9)

```
Moody cinematic illustration of a highway truck stop in northern Indiana at night: rows of parked semi-trucks with their running lights glowing, one truck isolated at the far end of the lot with its cab dark and the rear doors of its refrigerated trailer hanging open and empty, a lit diner and fuel canopy in the distance, heavy summer rain and lightning over flat fields. Electric blue, sodium yellow and black palette, painterly graphic-novel style for a detective game. No people, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still from a pole high above a truck stop parking lot at night in heavy rain, rows of parked semi-trucks, one truck at the far end, sodium lights, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case042_lot/01–03`**

01.
```
Grainy colour security camera still from a pole high above a truck stop parking lot at night in heavy rain, rows of parked semi-trucks, one truck at the far end, sodium lights, no timestamp, no text of any kind, faces not recognisable. A man in a cap climbing onto the back of the cab of the far truck, reaching up to its exhaust stack.
```
02.
```
Grainy colour security camera still from a pole high above a truck stop parking lot at night in heavy rain, rows of parked semi-trucks, one truck at the far end, sodium lights, no timestamp, no text of any kind, faces not recognisable. A second truck backing up to the far truck's open trailer, two figures moving crates across in the rain.
```
03.
```
Grainy colour security camera still from a pole high above a truck stop parking lot at night in heavy rain, rows of parked semi-trucks, one truck at the far end, sodium lights, no timestamp, no text of any kind, faces not recognisable. The same man climbing back onto the far truck's cab and pulling something from the exhaust stack.
```
