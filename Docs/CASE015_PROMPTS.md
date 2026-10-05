# Dosya #015 — görsel prompt'ları

Araç dağılımı: **Gemini** portreler ve adli bulgular (beyaz zemin), **ChatGPT** kapak, CCTV, olay yeri planı ve tasarım panosu. Hiçbir görselde okunur yazı, gerçek kurum adı, logo, marka ya da polis arması olmayacak.

## Portreler — Gemini (7 kişi; Callum ölü, portre yok)

Ortak kısım, her prompt'un sonuna eklenir:

> Head-and-shoulders portrait, facing the camera directly, eyes looking at the viewer, neutral seated posture as if across an interrogation table. Detailed ink-line graphic-novel illustration with soft painted shading, muted realistic colours, same style as a gritty detective game character sheet. Plain solid pure white background, no shadow on the background, no text, no border, no badges or insignia.

1. **Ryan Webb** — `Bube/Characters/ryan`
> A 24-year-old British man, short dark-brown hair, boyish face, scared and defensive expression, a small cut on his knuckles. Faded orange high-visibility work jacket over a grey hoodie.
2. **Josie Lang** — `Bube/Characters/josie`
> A 38-year-old British woman, dark-blonde hair tied back with loose strands, tired but steady eyes, guarded expression. Black bar apron over a black t-shirt, a tea towel on her shoulder.
3. **Frank Doyle** — `Bube/Characters/frank`
> A 57-year-old Irish-British man, thinning grey hair, heavy jowls, red-veined cheeks, sweating, ashamed eyes. Brown cardigan over a checked shirt.
4. **Neil Garrett** — `Bube/Characters/garrett`
> A 45-year-old British man, tall and broad-shouldered, very short dark hair greying at the temples, clean-shaven, cold controlled expression, slight smirk. Dark navy wool jacket over a black polo shirt.
5. **Gareth Hollis** — `Bube/Characters/gareth`
> A 39-year-old British man, medium-brown hair, neat beard, nervous eyes avoiding certainty, jaw tight. Dark green quilted jacket.
6. **Paul Ainsworth** — `Bube/Characters/paul`
> A 34-year-old British man, sandy-blond side-parted hair, clean-shaven, overly composed bureaucratic expression. Grey suit jacket, white shirt, no tie.
7. **Steve Barker** — `Bube/Characters/steve`
> A 41-year-old Black British man, close-cropped black hair, short greying beard, uneasy expression, eyes slightly down. Maroon bomber jacket.

## Adli bulgular — Gemini (`Bube/Items/case015_*`)

Ortak kısım:

> Forensic evidence illustration, top-down or three-quarter view, single object centred, neutral grey evidence mat with a small blank scale ruler (no numbers or text), flat even forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no text, no labels, no logos.

1. **Bordür** — `case015_kerb`: *A wet granite street kerb at night under sodium light, rounded edge, no blood smear on the edge, a few cigarette butts nearby.* (Olay yeri fotoğrafı; evidence-mat kullanma.)
2. **Zarf** — `case015_envelope`: *A thick plain brown paper envelope, slightly torn at the corner, banknote edges visible inside, in a clear evidence bag.*
3. **Telefon kılıfı** — `case015_case`: *A cracked black phone case without the phone, muddy and wet, inside a clear evidence bag.*

## Kapak — ChatGPT (`Bube/Art/Covers/case015.jpg`, yatay 16:9)

> Moody cinematic illustration of a traditional London corner pub at closing time on a rainy night in Bermondsey. Warm lights inside being switched off one by one, a white delivery van parked half on the pavement in front, a small walled smoking area on the right side, a black taxi waiting across the street with its headlights on, wet cobbles reflecting amber light. Muted teal and amber palette, painterly graphic-novel style matching a detective game. No people, no readable text, no pub sign lettering, no logos.

## Olay yeri planı — ChatGPT (`Bube/Plans/case015.png`, **kare 1:1**)

> Top-down architectural site plan drawing, square format, clean ink lines on warm off-white paper, subtle grey hatching for walls. Layout: the top half is a pub interior seen from above — a long bar counter along the top wall towards the right, small tables, a back door on the left wall. The pub's front wall runs horizontally across the middle of the image (about 48% from the top) with a wide window on the left part and a front door slightly left of centre. On the right, outside the pub, a small smoking area enclosed by a tall brick side wall on its left side; the wall extends down past the pub front onto the pavement. Below the front wall: a pavement, then a street running horizontally across the bottom third. A rectangular delivery van parked half on the pavement in front of the pub, centre-right. A taxi on the far side of the street. No text, no labels, no numbers, no compass, no people, no legend.

Görsel gelince koordinatları ona göre ayarlarım (şu an: ön duvar y≈0,49; pencere x 0,15–0,32; kapı x 0,32–0,40; sigara alanı duvarı x≈0,73, y 0,30–0,66; kamyonet x 0,40–0,62, y 0,66–0,78; olay noktası 0,22 / 0,68).

## CCTV — ChatGPT (`Bube/Cctv/case015_follow/01–03`, 16:9)

Ortak kısım:

> Low-resolution dashcam still from inside a parked taxi, looking across a wet street at the front of a London pub at night, rain on the windscreen, a white van parked half on the pavement to the right of the pub door, wiper blade partly visible. Grainy, slight fisheye, no timestamp, no text of any kind, faces not clearly recognisable.

1. *A tall man in a dark jacket, no hat, stepping quickly out of the pub's front door.*
2. *The tall man on the pavement reaching a shorter hooded man and grabbing his arm; both partly hidden behind the van.*
3. *Only the van and empty pavement visible; a single tall figure walking away towards the smoking area wall, hand in his pocket.*

## Tasarım panosu — ChatGPT

> UI design board for a mobile detective game screen showing a crime-scene floor plan inside a paper case file. Style: warm aged paper (#E8D9B7), dark ink (#212933), brown stamp accents (#733A2E), coffee accent (#8A4A10), no gradients, almost square corners. Show: the file page header, a short paragraph of text, then a square top-down floor plan with walls, a red-brown cross marking an incident spot, three small round witness markers (two cream, one dark ink), one selected marker casting a translucent warm cone of vision that is cut off by a wall, and a caption line below the plan. No colour coding of truth, no "lie" or "contradiction" labels, no icons of police or real institutions. Landscape tablet layout, 2400×1080.
