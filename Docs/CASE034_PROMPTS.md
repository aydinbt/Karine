# Dosya #034 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz/gri zemin), **ChatGPT** kapak, CCTV ve olay yeri planı. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (6 kişi)

Her prompt kendi içinde tamdır; olduğu gibi yapıştır.

1. **Amira Saïdi** → `Bube/Characters/amira`

```
A 36-year-old French woman of Algerian descent, long dark hair in loose waves, dark eyes swollen from crying, strong brows. Deep plum blouse under a black cardigan. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

2. **Yacine Saïdi** → `Bube/Characters/yacine`

```
A 31-year-old French man of Algerian descent, short black hair faded at the sides, neat short beard, alert eyes, faint smirk. Black zip-up track jacket over a grey t-shirt, a thin gold chain. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

3. **Léa Dubois** → `Bube/Characters/lea`

```
A 24-year-old French woman, copper-red hair in a messy bun, freckles, magnifying loupe headband pushed up into her hair, worried eyes. Teal work shirt with a pen in the pocket. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

4. **Dimitri Popescu** → `Bube/Characters/popescu`

```
A 47-year-old Romanian man, thinning dark hair combed back, heavy jowls, gold tooth glint, shrewd eyes. Brown leather jacket over a striped shirt. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

5. **Zhou Meilin** → `Bube/Characters/zhou`

```
A 62-year-old Chinese-French woman, short grey permed hair, reading glasses on her nose, sharp watchful expression. Dark green padded vest over a floral blouse. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

6. **Théo Marchand** → `Bube/Characters/theo`

```
A 17-year-old French boy, messy light-brown hair, acne on his chin, nervous big eyes, chewing his lip. Bright orange delivery-courier jacket without any logo, grey hoodie underneath. Head-and-shoulders portrait only, arms and hands not visible, no table, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.
```

## Adli bulgular — Gemini (`Bube/Items/case034_*`)

Her prompt nesneyi, malzemesini, açıyı, ışığı ve olmaması gerekenleri tek tek söyler; Gemini'ye kısa prompt verildiğinde nesneyi başka bir şeye çeviriyordu (bkz. `IMAGE_REDO.md`).

1. **Kazıma bıçağı** → `Bube/Items/case034_knife` — Ekran sökmede kullanılan ince, sivri uçlu bıçak; tezgâhtaki takımdan eksik olan.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A slim pointed hobby-style scraping knife with a thin steel blade and a black rubber handle, dried dark blood along the blade, next to an empty slot in an open phone-repair tool roll with other small screwdrivers and pry tools. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no text, no logos.
```

2. **SIM defteri** → `Bube/Items/case034_simbook` — Kareli bir okul defteri; tarih, SIM numarası ve müşteri tarifi sütunları.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A worn square-ruled school notebook lying open, columns of handwritten entries deliberately blurred and unreadable, one line circled twice in red pen, several loose plastic SIM card holders with the cards punched out scattered beside it. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no readable text, no logos.
```

3. **Kapüşonlu sweatshirt** → `Bube/Items/case034_hoodie` — Yacine’in dairesinde, çamaşır makinesinin içinde ıslak, kollarında açık kahverengi leke halkası.

```
Forensic evidence photograph, top-down on a neutral grey evidence mat. A damp dark grey hooded sweatshirt laid flat, the right cuff showing a faint pale brown ring stain that survived washing, a fluorescent forensic marker beside it. Small blank scale ruler without numbers. Flat forensic lighting, photorealistic, no text, no logos.
```

## Kapak — ChatGPT (`Bube/Art/Covers/case034.jpg`, yatay 16:9)

```
Moody cinematic illustration of a narrow Belleville street in Paris at night: a tiny phone repair shop with its metal shutter half down and a single bare bulb inside, phone cases hanging in the window, a Chinese grocery with fruit crates opposite, a courier scooter speeding away leaving a light trail, wet street. Neon teal, amber and dark violet palette, painterly graphic-novel style for a detective game. No people faces, no readable text, no logos.
```

## CCTV — ChatGPT (16:9, olay başına 3 kare)

Ortak kısım (her karenin başına):

```
Grainy colour security camera still from a small grocery's awning camera looking across a narrow Paris street at a phone repair shop with a half-lowered metal shutter, a single bulb inside, fruit crates in the foreground, no timestamp, no text of any kind, faces not recognisable.
```

**`Bube/Cctv/case034_street/01–03`**

01.
```
Grainy colour security camera still from a small grocery's awning camera looking across a narrow Paris street at a phone repair shop with a half-lowered metal shutter, a single bulb inside, fruit crates in the foreground, no timestamp, no text of any kind, faces not recognisable. A figure in a dark grey hoodie with the hood up ducking under the half-lowered shutter into the repair shop.
```
02.
```
Grainy colour security camera still from a small grocery's awning camera looking across a narrow Paris street at a phone repair shop with a half-lowered metal shutter, a single bulb inside, fruit crates in the foreground, no timestamp, no text of any kind, faces not recognisable. The street empty; inside the shop the bulb swings slightly, a shadow moves across the back wall.
```
03.
```
Grainy colour security camera still from a small grocery's awning camera looking across a narrow Paris street at a phone repair shop with a half-lowered metal shutter, a single bulb inside, fruit crates in the foreground, no timestamp, no text of any kind, faces not recognisable. The hooded figure ducking back out under the shutter, right hand pushed deep into the front pocket, walking fast uphill.
```
