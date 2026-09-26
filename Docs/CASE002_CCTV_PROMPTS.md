# Dosya #002 "Son Sefer" — CCTV çekim istemleri

Kaynak: **Büfe kamerası — KAMERA 04**, 21 Kasım 2026, 22.50 – 23.50.
Yöntem: Gemini/Veo **görselden videoya**. Her çekimde ilk kare olarak
`Docs/Reference/case002_street.png` (gönderilen gece sokak görseli) verilir.

## Değişmeyen kurallar

- **Kamera sabittir.** Zoom, kaydırma, sallanma, parallax yok. Kadraj referans
  görselin kadrajıdır; büfe sağda, taksi sağ şeritte, yol sola doğru yükselir.
- **Tek eylem.** Her istem yalnız bir olayı anlatır. İki şey aynı anda olmaz.
- **Yüz yok.** Kişiler yüksekten, küçük ve koyu giysili görünür; yüz hatları,
  saç rengi, marka, logo okunmaz. Oyun kayıtta "bir kişi" der; görüntü bundan
  fazlasını söylerse oyunun kuralı bozulur.
- **Yazı yok.** Saat damgası, kamera adı, plaka, tabela yazısı, kurum adı,
  arma **yakılmaz**. Saat ve kaynak adını oyunun kendi arayüzü yazar.
- **Ses yok.**
- Süre 6–8 sn, 24 fps, hafif kare düşürme (CCTV hissi) serbest; renk ve ışık
  referans görselin ta kendisi: ıslak asfalt, sodyum sarısı sokak lambaları,
  büfenin soğuk vitrin ışığı.

**Ortak stil bloğu** (her istemin başına aynen eklenir):

> 2D hand-painted anime-style night street, exactly matching the provided first
> frame in color, lighting and layout. Static security-camera view from a high
> fixed position, no camera movement, no zoom, no parallax. Wet asphalt with
> warm sodium reflections, a lit kiosk window on the right, a yellow taxi parked
> at the right kerb. Figures are small, seen from above, dark clothing, faces
> not readable. Calm, slow, ordinary motion.

**Kaçınılacaklar (negative prompt):**

> camera movement, zoom, pan, handheld shake, close-up, face detail, readable
> text, timestamp overlay, watermark, license plate, logos, police markings,
> blood, dramatic lighting, lens flare, photorealism, 3D render, crowd,
> extra vehicles arriving, weather change, day light

## Çekimler

### 01 — 22.58 · `park`
> …ortak stil bloğu… The yellow taxi drives slowly in from the upper left of the
> street and comes to a stop at the right kerb in front of the kiosk. Its brake
> lights glow once, then the headlights switch off. Nobody gets out; the driver
> stays inside. The street is otherwise empty.

### 02 — 23.00 · `dispute`
> …ortak stil bloğu… The parked taxi stands still. Inside the cabin two
> silhouettes move: brief hand and arm gestures behind the glass, one leaning
> forward, the other back. No door opens, nobody leaves the car, the car does
> not move.

### 03 — 23.03 · `passenger_out`
> …ortak stil bloğu… **Only the rear passenger door on the kerb side of the
> parked taxi opens.** One person steps out, pushes the rear door shut and walks
> up the street away from the camera, disappearing at the upper end. The driver
> stays inside; no other door opens and the taxi does not move.

### 04 — 23.18 · `second_in`
> …ortak stil bloğu… A second person walks into the frame from the lower end of
> the street (the seaside end), crosses the wet road and approaches the parked
> taxi, stopping beside the driver's side. No door opens yet. Nobody else is in
> the street.

### 05 — 23.20 · `contact`
> …ortak stil bloğu… **The driver's door opens, the driver steps out** and
> stands facing the second person beside the car. They stand close for a few
> seconds; one short abrupt movement passes between them and the driver goes
> down onto the pavement next to the kerb. No weapon, no blood, no repeated
> blows, no dramatic emphasis; everything stays small and far away.

### 06 — 23.31 · `second_out`
> …ortak stil bloğu… The driver lies on the pavement beside the parked taxi,
> not moving. The second person turns away and walks out of frame towards the
> lower, seaside end of the street. Nothing is carried in their hands. No door
> opens, nobody else enters the frame.

### 07 — 23.34 · `third_in`
> …ortak stil bloğu… The driver still lies on the pavement beside the taxi. A
> third person comes from the kiosk side on the right, walks to the taxi and
> stops level with the **front passenger door, which stands open**, looking down
> into the cabin. They do not touch the person on the ground. Hands and what
> they do inside the cabin are not readable from this height.

### 08 — 23.39 · `third_out`
> …ortak stil bloğu… The third person straightens up beside the open front
> passenger door, pushes it shut and walks back towards the kiosk side on the
> right, leaving the frame there. The driver stays on the pavement. Nothing
> carried is identifiable.

### 09 — 23.46 · `patrol`
> …ortak stil bloğu… A patrol car turns into the street from the lower end and
> stops short of the taxi; its rotating lights wash the wet asphalt in slow blue
> and red pulses. No markings, no text, no emblem anywhere on the vehicle. The
> driver still lies on the pavement. The shot ends before anyone steps out.

## Boşluk — görüntüsü olmayan an

23.21 – 23.30 arası kayıtta **sinyal boşluğu** vardır; bu aralığın görüntüsü
üretilmez ve üretilmemelidir. Oyun boşluğu metinle bildirir.

## Dosyaya bağlama

Klipler `Assets/StreamingAssets/Bube/CCTV/case002_<olay>.mp4` olarak konur ve
`case002.json` içindeki ilgili `cctvEvents` girdisine `videoPath` yazılır.
Klip yoksa oyun metin dökümüyle çalışmaya devam eder.
