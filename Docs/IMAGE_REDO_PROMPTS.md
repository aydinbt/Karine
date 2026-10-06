# Yeniden üretim prompt'ları

`IMAGE_REDO.md`deki her satır için Gemini'ye yapıştırılacak tam prompt. Her biri yeni sohbette. Düzeltme cümlesi sonda.

## ✅ case011_bollard → `Items/case011_bollard.png`

Sorun: İskele yok, leke yok

```
A rusty black cast-iron mooring bollard bolted onto old wet wooden pier planks, a faint dark smear on its upper rim, river fog in the background. On-site scene, no evidence mat, no ruler. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. No readable text, no logos.
```

## ✅ case011_bronze → `Items/case011_bronze.png`

Sorun: Bronz parçalar yerine anahtar çizilmiş

```
A small pile of tarnished bronze boat fittings — propeller nuts, deck cleats, small hinges — inside a clear plastic evidence bag. No keys. Lying flat on a neutral mid-grey forensic evidence mat, a small plain scale ruler with tick marks but no numbers. Camera: straight top-down, object centred, filling about 60% of the frame. Flat even forensic lighting, soft shadow. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. Absolutely no readable text, letters, numbers, logos or brands.
```

## ✅ kieran → `Characters/kieran.png`

Sorun: Şapka tepesi kesik, yüz fazla yakın

```
A 24-year-old Irish-British man, ginger hair under a red baseball cap, freckles, a small fresh bruise on the cheekbone, defensive and scared expression. Faded red work jacket. IMPORTANT FIX: full head visible with clear space above the cap, head-and-shoulders framing, not a close-up.
```

## ✅ case012_lock → `Items/case012_lock.png`

Sorun: U kilit yerine asma kilit

```
A heavy black steel U-shaped bicycle lock (D-lock with straight crossbar), slightly wet, a faint dark stain on the curved end, inside a clear plastic evidence bag. Not a padlock. Lying flat on a neutral mid-grey forensic evidence mat, a small plain scale ruler with tick marks but no numbers. Camera: straight top-down, object centred, filling about 60% of the frame. Flat even forensic lighting, soft shadow. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. Absolutely no readable text, letters, numbers, logos or brands.
```

## ✅ case012_camera → `Items/case012_camera.png`

Sorun: Tavan kamerası yerine fotoğraf makinesi

```
Close-up looking up at a small white dome security camera mounted on a city bus ceiling, its lens covered with a strip of black electrical tape. Not a handheld camera. On-site scene, no evidence mat, no ruler. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. No readable text, no logos.
```

## ✅ case012_sheet → `Items/case012_sheet.png`

Sorun: Kâğıt boş

```
A clipboard holding a hospital medicine count sheet with ruled tally columns filled with handwritten marks and illegible scribbles, a few entries circled in red pen, NO title or heading at the top of the sheet. Lying flat on a neutral mid-grey forensic evidence mat, a small plain scale ruler with tick marks but no numbers. Camera: straight top-down, object centred, filling about 60% of the frame. Flat even forensic lighting, soft shadow. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D. Absolutely no readable text, letters, numbers, logos or brands. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case013_ledger → `Items/case013_ledger.png`

Sorun: Okunur yazı var ("Monthly Rent Statement", "Police…")

```
Object: A printed monthly rent statement, two A4 sheets, on a cluttered wooden desk; columns of tiny figures that are blurred and unreadable; one row boldly highlighted with a yellow marker; a cheap biro lying across the corner.
Setting: lying flat on a neutral mid-grey forensic evidence mat, a small plain scale ruler with tick marks but no numbers along the bottom edge. Camera: straight top-down, object centred and filling about 60% of the frame. Light: flat even forensic lighting, soft shadow, true colours. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, square 1:1.
Must NOT contain: any readable letters, numbers or words; any logo, brand, emblem, police crest or real institution name; people or hands; extra objects not listed. IMPORTANT FIX: all text must be blurred squiggles, no readable words or numbers. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case014_note → `Items/case014_note.png`

Sorun: Gemini okunur bir ad yazdı ("Maria", vakada yok); silindi, sayfa artık boş, üstünde hafif bir yama var

```
Object: A small torn notebook page, ragged bottom edge, a single short handwritten word in pencil that is an illegible scrawl, a faint fold line. Inside a clear evidence bag.
Setting: lying flat on a neutral mid-grey forensic evidence mat, a small plain scale ruler with tick marks but no numbers along the bottom edge. Camera: straight top-down, object centred and filling about 60% of the frame. Light: flat even forensic lighting, soft shadow, true colours. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, square 1:1.
Must NOT contain: any readable letters, numbers or words; any logo, brand, emblem, police crest or real institution name; people or hands; extra objects not listed. IMPORTANT FIX: one illegible pencil scrawl, not a name, no readable letters. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case016_letters → `Items/case016_letters.png`

Sorun: Prompt'taki "angry scrawl" ifadesi imza yerine okunur yazı olarak çizilmiş

```
Object: A small stack of five folded debt-collection letters, the printed text blurred and unreadable, a red-ink stripe across the top one, one letter with an angry handwritten pen scrawl in the margin. Fanned slightly.
Setting: lying flat on a neutral mid-grey forensic evidence mat, a small plain scale ruler with tick marks but no numbers along the bottom edge. Camera: straight top-down, object centred and filling about 60% of the frame. Light: flat even forensic lighting, soft shadow, true colours. Style: 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, square 1:1.
Must NOT contain: any readable letters, numbers or words; any logo, brand, emblem, police crest or real institution name; people or hands; extra objects not listed. IMPORTANT FIX: an illegible scribbled signature, no readable words. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case027_ledger → `Items/case027_ledger.png`

Sorun: Başlıkta okunur "Invoice/INVOICE" yazısı

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small stack of twelve generic printed invoices, deliberately blurred so no text is readable, held with a black binder clip, one yellow sticky note on top with a single hand-drawn question mark. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: remove all text, keep everything else identical. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case031_notebook → `Items/case031_notebook.png`

Sorun: Sayfalar okunur gibi Japonca yazıyla dolu; zeminde pikselli bozulma

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A small black leather-bound notebook lying open, the handwriting in Japanese-style ink strokes deliberately blurred and unreadable, the last written page pressed hard so the strokes are deep, a fountain pen beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: handwriting as illegible wavy lines, no real characters. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case033_envelope → `Items/case033_envelope.png`

Sorun: Gerçek 100 dolarlık banknot (portre, yazı, seri no.) neredeyse fotoğraf gibi çizilmiş; gerçek para + üslup dışı

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A plain white envelope lying open with a thick stack of generic banknotes partly pulled out, the notes' details blurred so no denomination or text is readable. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: plain blank generic paper notes, no portrait, no numbers, no real currency. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case043_cash → `Items/case043_cash.png`

Sorun: Gerçek 20 dolarlık banknotlar (portre, rakam, seri no.)

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. Several thick brown paper envelopes, one open showing a stack of twenty-dollar-like banknotes with blurred unreadable printing held by a rubber band, a pencilled tick mark on each envelope. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: plain blank generic paper notes, no portrait, no numbers, no real currency. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case053_string → `Items/case053_string.png`

Sorun: Tel paketinde ayna gibi ters, gerçek bir tel markasını andıran logo ve "33" yazısı

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A partially unwound coil of thin steel guitar string, both ends bent into small loops, tiny traces of skin on the metal, beside its small open paper sleeve with blurred unreadable print. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no brand names, no logos. IMPORTANT FIX: plain blank paper sleeve, no logo, no text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case053_file1994 → `Items/case053_file1994.png`

Sorun: Daktilo sayfası yarı okunur sahte İngilizce, tarih "1990" gibi okunuyor (dosya 1994); "REOPENED" bandı okunur

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A yellowed old cardboard case folder lying open, inside a faded 1990s colour photograph of a narrow wooden backstage staircase and a single typed statement page with a signature, a plain blank red paper band across the folder with NO writing on it, the typed page shows only grey horizontal smudge lines, no letters, no date, no greeting. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: typed lines as illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case057_ledger → `Items/case057_ledger.png`

Sorun: Okunur İngilizce "Accident Logbook" başlığı ve yarı okunur el yazısı

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. An old hardbound accident logbook open to a page where a handwritten entry has been scraped and rewritten in different ink, the paper thinned and slightly rough, handwriting deliberately illegible. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: handwriting as illegible wavy lines, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case058_cradle → `Items/case058_cradle.png`

Sorun: Okunur İngilizce sütun başlıkları (Date/Item/Condition/Notes) ve tarihler

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. An open green cloth-bound conservation workshop register with ruled columns, one row filled in neat handwriting and the matching return column left conspicuously empty, a bone folder lying across the page, handwriting illegible. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible handwritten entries, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case059_list → `Items/case059_list.png`

Sorun: Yarı okunur sahte İngilizce ("Bank Transfer Slip", "Amendment", "EMPLOYEEE", tarihler)

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. An old typed list on cream paper with no emblem, every line drawn only as solid grey horizontal smudge bars like the 1994 folder page, no letters at all, one line marked with a small pencilled star in the margin, a paperclip at the corner. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case059_folder → `Items/case059_folder.png`

Sorun: Yarı okunur sahte İngilizce ("Bank Transfer Slip", "Amendment", "EMPLOYEEE", tarihler)

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A thick brown cardboard document folder half burned, charred black edges curling, partially burnt blank pages and small payment slips inside, every line drawn only as solid grey horizontal smudge bars, no letters, no numbers, no currency symbols, no headings, flakes of grey fireplace ash around it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case060_policy → `Items/case060_policy.png`

Sorun: Yarı okunur sahte İngilizce ("Bank Transfer Slip", "Amendment", "EMPLOYEEE", tarihler)

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A stapled insurance policy amendment on plain white paper with a blank grey header block, one paragraph highlighted in yellow, two signature lines with illegible scrawls, a blue ballpoint pen beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case060_clock → `Items/case060_clock.png`

Sorun: Yarı okunur sahte İngilizce ("Bank Transfer Slip", "Amendment", "EMPLOYEEE", tarihler)

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A buff cardboard employee time card with a column of purple stamped clock-out marks, the last one smudged, the printed numbers blurred and unreadable, lying next to a wall punch-clock slot. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case062_count → `Items/case062_count.png`

Sorun: Okunur İngilizce "Carbon-Copy Warehouse Inventory" başlığı ve sütun adları

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A carbon-copy warehouse inventory form on a clipboard, columns of handwritten tallies all ticked, a large confident signature at the bottom, every number and word illegible. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case064_loader → `Items/case064_loader.png`

Sorun: **Gerçek marka:** koltuk sırtında kabartma "CATERPILLAR" yazısı

```
Forensic evidence illustration, close-up of a worn black vinyl seat of a compact wheel loader cab, a few bright red wool fibres caught in a split seam, evidence tape marking them, a frosted cab window behind. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: plain seat back, no logo, no brand, no text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere.
```

## ✅ case065_receipt → `Items/case065_receipt.png`

Sorun: Fişte okunur "RESTAURANT" ve yarı okunur satırlar

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A crumpled thermal restaurant receipt smoothed flat, faded print lines all illegible, a small wine stain in one corner, beside a folded paper napkin. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings. The receipt has NO words at all: no restaurant name, no 'TOTAL', no dollar signs, no prices — only grey smudge bars.
```

## ✅ case066_minutes → `Items/case066_minutes.png`

Sorun: Okunur "CORPORATE BOARD MEETING MINUTES" başlığı ve uydurma isim listesi

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A corporate board meeting minutes document on heavy white paper with a blank header block, an attendance list with one line marked by a small pencil tick, all text illegible, a silver fountain pen beside it. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case066_table → `Items/case066_table.png`

Sorun: Yarı okunur el yazısı tablo ve "Total" satırı

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A printed spreadsheet page of dense numeric columns with one entire column highlighted in yellow, the same short entry repeated down every row, all figures and words blurred and unreadable, a red pen circle around the total. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible grey marks, no readable text. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case068_slip → `Items/case068_slip.png`

Sorun: Okunur sahte-İngilizce tablo ("Ledger" satırları, rakamlar)

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A few printed pages of a betting transaction ledger, dense columns of small figures, all illegible, with a yellow highlighter stripe across several rows, a paperclip at the corner. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible scribbles, no readable words or numbers. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case069_gate → `Items/case069_gate.png`

Sorun: "ACCESS-CONTROL LOG" başlığı, İngilizce sütun başlıkları, okunur rakamlar

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A printed access-control log page with columns of entry times, all text illegible, beside a grey plastic RFID key fob on a lanyard. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible scribbles, no readable words or numbers. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case071_manifest → `Items/case071_manifest.png`

Sorun: "Carbon Shipping Manifest" başlığı, "RECEIVED/SHIPPED" damgaları, okunur satırlar

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A yellowed multi-part carbon shipping manifest form from the 1990s, columns of typed entries and stamps, all illegible, with a coffee stain and a torn corner. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible scribbles, no readable words or numbers, blank stamps. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case072_hygiene → `Items/case072_hygiene.png`

Sorun: "Council Hygiene Inspection Form" başlığı ve okunur madde satırları

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. An old council hygiene inspection form, two pages, a large red rubber-stamp mark and a signature at the bottom, all text illegible, slightly yellowed and folded in thirds. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible scribbles, no readable words. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```

## ✅ case073_courier → `Items/case073_courier.png`

Sorun: "CARBORLESS INTERNATIONAL RECEIPT" başlığı, okunur barkod numarası

```
Forensic evidence illustration, top-down on a neutral grey evidence mat. A carbonless international courier receipt slip with a barcode and handwritten address boxes, all text illegible, beside a small padded envelope big enough for a USB stick, torn open. Small blank scale ruler without numbers. Flat forensic lighting, 2D hand-drawn ink illustration in the same style as the character portraits: fine black ink linework, cross-hatching and stippling, muted realistic colour wash, not a photograph, not 3D, no readable text, no logos. IMPORTANT FIX: illegible scribbles, no readable words or numbers. Absolutely no readable text, letters, numbers, logos, brands or real currency anywhere. Every line of writing or print is drawn only as solid grey horizontal smudge bars, no letters, no numbers, no headings.
```
