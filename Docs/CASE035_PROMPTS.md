# Dosya #035 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (4 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Karim Benhamou** → `Bube/Characters/benhamou`

```
A 44-year-old French man of Algerian descent, short black hair, neat moustache, shocked hollow eyes, pale under his tan. Dark blue metro-driver uniform jacket without any logo or text, light blue shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Odile Vasseur** → `Bube/Characters/odile`

```
A 70-year-old French woman, white hair in a short neat bob, round wire glasses, alert intelligent eyes, a slightly stern mouth. Sage-green wool coat with a patterned silk scarf. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Lucien Mallet** → `Bube/Characters/mallet`

```
A 58-year-old French man, short greying hair, lined weary face, soft sad grey eyes, faint stubble, calm. Grey wool overcoat over a dark charcoal jumper. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Pascal Royer** → `Bube/Characters/royer`

```
A 39-year-old French man, side-parted brown hair, clean-shaven, nervous darting eyes, slightly sweaty forehead. Navy suit, white shirt, loosened blue tie. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case035_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Acil çağrı kaydı** → `Bube/Items/case035_call` — Çağrı merkezi ekran görüntüsü: arayan numara, saat, konum.

```
Forensic evidence photograph of a computer monitor in a dim emergency call centre showing a generic call-log interface with blurred unreadable rows, one row highlighted in yellow, a headset lying on the desk in front of the screen. 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

2. **Metro kartı** → `Bube/Items/case035_card` — Lucien Mallet’nin cüzdanındaki aylık metro kartı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A plain blue plastic transit card with no text or logo, next to a worn brown leather wallet lying open with a faded small photograph of a young girl tucked behind a clear window. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no logos.
```

3. **Eski dava kararı** → `Bube/Items/case035_ruling` — 2019 tarihli, beraatle sonuçlanmış trafik kazası dosyasının kapağı.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A thick beige cardboard legal file folder tied with a faded red ribbon, a blank white label on its cover with no writing, a newspaper clipping with a blurred unreadable headline and a small blurred photo of a crashed car tucked under the ribbon. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case035.jpg`, yatay 16:9)

```
Moody cinematic illustration of a Paris metro platform at morning rush hour seen from the end of the platform: curved white-tiled vault, a train's headlights bursting out of the dark tunnel, a crowd of commuters frozen as silhouettes, one empty gap at the platform edge, a dropped leather briefcase lying open on the tiles. Cold white, deep green and signal-yellow palette, painterly graphic-novel style for a detective game. No readable station name, no text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy forward-facing train cab camera still entering a curved Paris metro platform, white tiled walls, commuters along the platform edge, motion blur, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case035_cab/01–03`**

01.
```
Grainy forward-facing train cab camera still entering a curved Paris metro platform, white tiled walls, commuters along the platform edge, motion blur, no timestamp, no text of any kind, faces not recognisable. A man with a briefcase standing at the platform edge looking at his phone; directly behind him a man in a grey overcoat with one hand at his ear holding a phone.
```
02.
```
Grainy forward-facing train cab camera still entering a curved Paris metro platform, white tiled walls, commuters along the platform edge, motion blur, no timestamp, no text of any kind, faces not recognisable. The grey-overcoat man's free hand flat against the briefcase man's back; the briefcase man tipping forward over the yellow line.
```
03.
```
Grainy forward-facing train cab camera still entering a curved Paris metro platform, white tiled walls, commuters along the platform edge, motion blur, no timestamp, no text of any kind, faces not recognisable. The grey-overcoat man stepping back into the crowd, phone still at his ear, the other hand raised as if calling for help.
```
