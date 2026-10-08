#!/usr/bin/env python3
# Karine'nin ses varlıklarını üretir. Ses dosyaları depoda durur, ama **elle
# çizilmiş** değil: burada sentezlenir, yani bir tonu değiştirmek için yeni bir
# kayıt aramak yerine bu betiği düzenleyip yeniden koşmak yeterli.
#
#   python3 Tools/make-audio.py
#
# Çıktı: Assets/Bube/Resources/Bube/Audio/*.wav (mono, 44.1 kHz, 16 bit).
# Dosya adları `AudioDirector`ın beklediği kimliklerdir; başka yerde yazılı
# değil. Harici bağımlılık yok (numpy/ffmpeg gerekmez), her şey saf Python.
#
# Atmosfer: 90'lar sonu bir emniyet birimi. Soğuk oda, floresan, uzakta trafik,
# kâğıt ve mekanik düğme. Müzik neo-noir: yavaş, seyrek, minör, hiçbir yere
# acele etmiyor — oyuncu düşünürken üstüne çıkmaması gerekiyor.

import array, math, os, random, struct, wave

RATE = 44100
OUT  = os.path.join(os.path.dirname(os.path.abspath(__file__)),
                    "..", "Assets", "Bube", "Resources", "Bube", "Audio")

# --- temel yapı taşları ------------------------------------------------------

def buf(seconds):
 return array.array('d', bytes(8 * int(RATE * seconds)))

def sine(out, freq, amp, dur, start=0.0, phase=0.0, env=None):
 """Toplamalı yazar: aynı tampona üst üste ses eklenir."""
 i0 = int(start * RATE); n = int(dur * RATE)
 w = 2 * math.pi * freq / RATE
 for i in range(n):
  j = i0 + i
  if j >= len(out): break
  a = amp if env is None else amp * env(i / n)
  out[j] += a * math.sin(w * i + phase)

def noise(dur, seed=0):
 rng = random.Random(seed)
 n = int(dur * RATE)
 return array.array('d', (rng.uniform(-1.0, 1.0) for _ in range(n)))

def lowpass(x, cutoff, poles=1):
 """Tek kutuplu; `poles` kez uygulanınca eğim sertleşir."""
 for _ in range(poles):
  a = 1.0 - math.exp(-2 * math.pi * cutoff / RATE)
  y = 0.0
  for i in range(len(x)):
   y += a * (x[i] - y); x[i] = y
 return x

def highpass(x, cutoff):
 a = 1.0 - math.exp(-2 * math.pi * cutoff / RATE)
 y = 0.0
 for i in range(len(x)):
  y += a * (x[i] - y); x[i] -= y
 return x

def bandpass(x, freq, q=4.0):
 w = 2 * math.pi * freq / RATE
 alpha = math.sin(w) / (2 * q)
 b0, b1, b2 = alpha, 0.0, -alpha
 a0, a1, a2 = 1 + alpha, -2 * math.cos(w), 1 - alpha
 b0, b1, b2, a1, a2 = b0/a0, b1/a0, b2/a0, a1/a0, a2/a0
 x1 = x2 = y1 = y2 = 0.0
 for i in range(len(x)):
  v = x[i]
  y = b0*v + b1*x1 + b2*x2 - a1*y1 - a2*y2
  x2, x1 = x1, v; y2, y1 = y1, y
  x[i] = y
 return x

def reverb(x, mix=0.3, room=0.80, damp=0.35):
 """Küçük Schroeder odası: dört tarak, iki allpass. Oda kuru kalsın diye kısa."""
 wet = array.array('d', bytes(8 * len(x)))
 for delay, gain in ((1116, room), (1188, room-0.02), (1277, room-0.04), (1356, room-0.06)):
  line = [0.0] * delay; store = 0.0; p = 0
  for i in range(len(x)):
   out = line[p]
   store = out * (1 - damp) + store * damp
   line[p] = x[i] + store * gain
   p = (p + 1) % delay
   wet[i] += out * 0.25
 for delay, g in ((225, 0.5), (556, 0.5)):
  line = [0.0] * delay; p = 0
  for i in range(len(wet)):
   buffered = line[p]
   line[p] = wet[i] + buffered * g
   wet[i] = buffered - wet[i] * g
   p = (p + 1) % delay
 for i in range(len(x)):
  x[i] += wet[i] * mix
 return x

def fade(x, seconds_in=0.004, seconds_out=0.02):
 """Tık sesini önler: her parça sıfırdan başlar ve sıfıra iner."""
 n_in, n_out = int(seconds_in * RATE), int(seconds_out * RATE)
 for i in range(min(n_in, len(x))): x[i] *= i / n_in
 for i in range(min(n_out, len(x))):
  x[len(x)-1-i] *= i / n_out
 return x

def loop_noise(dur, seed, cutoff=None, poles=1, band=None, hp=None, xf=0.75):
 """Döngüye **dikişsiz** oturan gürültü katmanı. Gürültü periyodik değildir, o
 yüzden dikiş yerinde bir örnek atlaması olur. Çözüm: fazladan üretip süzmek,
 sonra kuyruğu başın üstüne **çapraz geçirmek** — eklemek değil, geçirmek;
 eklemek o bölgede gürültüyü 1,4 katına çıkarır ve seviye başta yükselir."""
 warm = 0.5
 long = noise(dur + xf + warm, seed)
 if band is not None: bandpass(long, band[0], band[1])
 if cutoff is not None: lowpass(long, cutoff, poles=poles)
 if hp is not None: highpass(long, hp)
 w = int(warm * RATE)
 long = long[w:]                      # süzgeç ısınması atılır
 n, k = int(dur * RATE), int(xf * RATE)
 out = long[:n]
 for i in range(k):
  a = i / k
  out[i] = out[i] * a + long[n + i] * (1 - a)
 return out

def steady_reverb(x, mix=0.2, room=0.75, damp=0.35):
 """Döngünün yankısı. Yankı dikişi geçer, yani tek kopyada oda başta boş,
 sonda dolu olur. İki kopya sürülür ve **ikincisi** alınır: orada oda artık
 dolmuş, yani döngünün her turunda duyulacak hâl."""
 twice = array.array('d', x); twice.extend(x)
 reverb(twice, mix=mix, room=room, damp=damp)
 return array.array('d', twice[len(x):])

def wrap_tail(x, loop_seconds):
 """Döngü dikişi. Kuyruk (yankı, çınlama) kesilmez; başa **eklenir**, böylece
 döngü başa döndüğünde ses kendi kuyruğunun üstüne biner ve ek duyulmaz."""
 n = int(loop_seconds * RATE)
 for i in range(n, len(x)):
  x[i - n] += x[i]
 return x[:n]

def normalize(x, peak=0.9):
 top = max(1e-9, max(abs(v) for v in x))
 k = peak / top
 for i in range(len(x)): x[i] *= k
 return x

def soft(x, drive=1.0):
 for i in range(len(x)): x[i] = math.tanh(x[i] * drive)
 return x

def write(name, x):
 os.makedirs(OUT, exist_ok=True)
 path = os.path.normpath(os.path.join(OUT, name + ".wav"))
 frames = array.array('h', (max(-32768, min(32767, int(v * 32767))) for v in x))
 with wave.open(path, 'wb') as f:
  f.setnchannels(1); f.setsampwidth(2); f.setframerate(RATE)
  f.writeframes(frames.tobytes())
 print("  %-16s %5.1f s  %6.0f KB" % (name, len(x)/RATE, os.path.getsize(path)/1024))

# --- zarflar -----------------------------------------------------------------

def decay(k):      return lambda t: math.exp(-k * t)

def swell(attack, dur, release=0.40):
 """Yastık zarfı: saniyeyle verilen atak, tutuş, sonra bırakış. Zarflara
 normalize edilmiş zaman (0..1) gelir; atak saniyesi bu yüzden süreye
 bölünmeli — bölünmezse yastık hiç tam açılmaz ve müzik yalnız tellerden
 duyulur."""
 a = max(1e-4, attack / dur)
 r = max(1e-4, release)
 def env(t):
  if t < a: return (t / a) ** 0.8
  if t > 1.0 - r: return ((1.0 - t) / r) ** 0.7
  return 1.0
 return env

# --- arayüz sesleri ----------------------------------------------------------
# Hepsi kısa ve **alçak**. Arayüz sesi oyunun üstüne çıkmaz; dokunduğunu
# onaylar, dikkat istemez.

def ui_press():
 # Yumuşak düğme. İlk sürüm mekanik bir klavye tıkıydı ve arayüzde "daktilo"
 # gibi duyuluyordu — sesi açan oyuncuyu yoruyor. Artık tık yok: yalnız üstü
 # kapalı, yuvarlak ve alçak bir "tup". Karar sesin **duyulmaması**, varlığının
 # sezilmesi.
 out = buf(0.16)
 sine(out, 168, 0.30, 0.10, env=lambda t: (t*14 if t < 0.07 else 1.0) * math.exp(-30 * t))
 sine(out, 252, 0.10, 0.07, env=lambda t: (t*14 if t < 0.07 else 1.0) * math.exp(-42 * t))
 touch = noise(0.02, 11); lowpass(touch, 900, poles=2)
 for i in range(len(touch)):
  out[i] += touch[i] * 0.10 * math.exp(-55 * i / RATE)
 reverb(out, mix=0.07, room=0.55)
 return normalize(fade(out, 0.006, 0.05), 0.34)

def ui_typewriter():
 # Daktilo tuşu. Artık **yalnız faks yazarken** çalıyor: resmî bir kâğıdın
 # basılması. Arayüz düğmelerinde bu ses yoktu, ama `ui_press` ona benzediği
 # için öyle duyuluyordu; düğme yumuşadı, bu ses de biraz.
 out = buf(0.16)
 click = noise(0.012, 31); highpass(click, 2200); lowpass(click, 7000)
 for i in range(len(click)):
  out[i] += click[i] * 0.45 * math.exp(-95 * i / RATE)
 hit = noise(0.05, 32); bandpass(hit, 1200, 1.6)
 for i in range(len(hit)):
  out[i] += hit[i] * 0.42 * math.exp(-45 * i / RATE)
 sine(out, 132, 0.22, 0.09, env=decay(34))
 sine(out, 2950, 0.025, 0.04, env=decay(80))   # kolun ince çınlaması
 reverb(out, mix=0.12, room=0.60)
 return normalize(fade(out), 0.40)

def ui_stamp():
 # Mühür: lastiğin kâğıda inişi. Alçak, tok, tek ve kesin.
 out = buf(0.40)
 smack = noise(0.045, 41); bandpass(smack, 900, 1.3)
 for i in range(len(smack)):
  out[i] += smack[i] * 0.6 * math.exp(-40 * i / RATE)
 sine(out, 62, 0.55, 0.22, env=decay(20))
 sine(out, 118, 0.22, 0.14, env=decay(28))
 paper = noise(0.12, 42); bandpass(paper, 3200, 1.0); highpass(paper, 1500)
 for i in range(len(paper)):
  out[i] += paper[i] * 0.16 * math.exp(-14 * i / RATE)
 reverb(out, mix=0.18, room=0.72)
 return normalize(fade(out, 0.002, 0.08), 0.62)

def ui_notification():
 # Gelen evrak: faks makinesinin küçük zili. Anharmonik kısmiler (gerçek zil
 # gibi), önünde mekanizmanın tıkı. Uyarı değil, haber.
 out = buf(1.30)
 tick = noise(0.01, 51); highpass(tick, 2400); lowpass(tick, 6500)
 for i in range(len(tick)):
  out[i] += tick[i] * 0.22 * math.exp(-110 * i / RATE)
 base = 784.0  # G5
 for ratio, amp, k in ((1.00, 0.34, 3.2), (2.76, 0.12, 4.8), (5.40, 0.035, 7.5),
                       (8.93, 0.010, 11.0), (1.002, 0.30, 3.0)):
  sine(out, base * ratio, amp, 1.25, start=0.004, env=decay(k))
 reverb(out, mix=0.26, room=0.78)
 return normalize(fade(out, 0.001, 0.12), 0.40)

# --- oda ortamları -----------------------------------------------------------
# İkisi de döngü. Olay yok, gelişme yok: oda ne yaparsan yapsın aynı kalır.
# Bilinçli olarak **çok alçak** — ortam sesi fark edilmemek için vardır.

def room_interview():
 # Görüşme odası ofisten **daha kapalı**, daha sıcak değil daha derin. Yine
 # ürkütmeyecek: tavandaki ince çınlama ve floresan titremesi kaldırıldı,
 # ikisi de kulakta kaygı yapıyordu. Fark artık ses değil **renk**: burada üst
 # frekans yok, yani duvarlar yakın.
 LOOP = 24.0
 out = buf(LOOP)
 n = len(out)
 deep  = loop_noise(LOOP, 111, cutoff=55, poles=3)
 close = loop_noise(LOOP, 112, cutoff=190, poles=3, hp=40)
 for i in range(n):
  t = i / RATE
  breath = 0.84 + 0.16 * math.sin(2*math.pi*t/LOOP + 0.9)
  out[i] = deep[i] * 0.58 + close[i] * 0.11 * breath
 # Duvar saati ve floresanın **sabit** vızıltısı (2 Ekim 2026). Titreme yok —
 # kaygı yapan oydu; burada vızıltı düz ve çok alçak. Saat her saniye bir kez,
 # döngü 24 saniye olduğu için dikiş tıkın arasına düşer.
 peak = max(abs(v) for v in out)
 for k in range(int(LOOP)):
  tick = noise(0.012, 900 + k % 2); highpass(tick, 2500); lowpass(tick, 6000)
  i0 = int((k + 0.5) * RATE)
  for i in range(len(tick)):
   out[i0 + i] += tick[i] * peak * (0.20 if k % 2 == 0 else 0.15) * math.exp(-220 * i / RATE)
 sine(out, 100.0, peak * 0.035, LOOP)
 sine(out, 200.0, peak * 0.012, LOOP)
 return normalize(steady_reverb(out, mix=0.16, room=0.74, damp=0.30), 0.22)

# Vakanın kendi ortamı (2 Ekim 2026). Masa müziğinin altında, `CaseData.ambienceId`
# ile seçilir. Görüşme odası kadar alçak: duyulmaz, yalnız eksikliği fark edilir.

def room_rain():
 # Dosya #003: yağmurlu gece, depo sokağı. Camdan duyulan geniş hışırtı,
 # oluktan alçak bir akış ve seyrek damla tıkırtıları. Gök gürültüsü yok —
 # tek seferlik olay döngüde tekrar edince saat gibi duyulur.
 LOOP = 24.0
 out = buf(LOOP)
 n = len(out)
 hiss = loop_noise(LOOP, 121, cutoff=5200, hp=1100)
 body = loop_noise(LOOP, 122, cutoff=320, poles=2, hp=140)
 for i in range(n):
  t = i / RATE
  breath = 0.9 + 0.1 * math.sin(2*math.pi*t/LOOP*2 + 0.4)
  # 8 Ekim 2026: alçak gövde 0.9'dan 0.25'e — yağmur artık varsayılan, sürekli
  # gövde uğultu gibi duyuluyordu. Hışırtı ve damlalar öne çıkar.
  out[i] = hiss[i] * 0.55 * breath + body[i] * 0.25
 rng = random.Random(123)
 for _ in range(int(LOOP * 28)):
  start = rng.randrange(n); freq = rng.uniform(1800, 4200)
  k = rng.uniform(70, 240); level = rng.uniform(0.04, 0.16)
  w = 2 * math.pi * freq / RATE
  for j in range(900):
   out[(start + j) % n] += level * math.exp(-j / k) * math.sin(w * j)
 return normalize(out, 0.09)

def room_night():
 # Dosya #002: gece sokağı. Uzak trafik uğultusu, yavaş dalgalanma ve
 # floresansız, sokak lambasının hafif şebeke vızıltısı. Geçen araç yok:
 # döngünün her turunda aynı yerde geçen araç kulakta takvim olur.
 LOOP = 24.0
 out = buf(LOOP)
 n = len(out)
 far  = loop_noise(LOOP, 131, cutoff=140, poles=3, hp=25)
 road = loop_noise(LOOP, 132, cutoff=480, poles=2, hp=120)
 for i in range(n):
  t = i / RATE
  wave_ = 0.85 + 0.15 * math.sin(2*math.pi*t/LOOP*3 + 1.3)
  out[i] = far[i] * 0.8 * wave_ + road[i] * 0.10 + 0.010 * math.sin(2*math.pi*50*t)
 return normalize(steady_reverb(out, mix=0.12, room=0.70, damp=0.40), 0.20)

# --- ifadenin belirme sesi ----------------------------------------------------

def blip(freq=660.0, dur=0.11, seed=71, level=0.22, bell=2.01, air=0.05):
 """Sohbet blibi. Konuşmayı taklit etmeye çalışan her ses (sesli harf,
 formant, klavye) kulakta düştü; bu ses hiçbir şeyi taklit etmiyor, yalnızca
 "yeni bir satır geldi" diyor. Yapısı bilerek basit: yumuşak bir sinüs, üstüne
 küçük bir çıngırak kısmisi (1:2,01 — tam oktav değil, yoksa organ gibi
 durur), rampalı bir açılış, kısa bir sönme ve çok az hava. Bir cümle boyunca
 düzinelerce kez çalacak: dikkat isteyen bir ses burada yorgunluk yapar, o
 yüzden tepe 0,22'de kalıyor."""
 out = buf(dur)
 n = len(out)
 # Gövde: yumuşak sinüs, hafif düşen perde (yükselen perde "hata" gibi duyulur)
 tone = buf(dur)
 sine(tone, freq,        1.00, dur, env=lambda t: math.exp(-16 * t))
 sine(tone, freq * bell, 0.22, dur, env=lambda t: math.exp(-26 * t))
 sine(tone, freq * 0.5,  0.14, dur, env=lambda t: math.exp(-12 * t))
 # Hava: sesin plastik durmaması için, tek başına duyulmayacak kadar az
 breath = noise(dur, seed)
 bandpass(breath, freq * 2.4, 1.0)
 for i in range(n):
  t = i / n
  attack = min(1.0, t / 0.10)          # tık yok: açılış rampalı
  out[i] = tone[i] * attack + breath[i] * air * math.exp(-40 * t)
 lowpass(out, 5200)
 reverb(out, mix=0.09, room=0.52)
 return normalize(fade(out, 0.005, 0.10), level)

# İki varyant: aynı blip, bir tam ses aralık. Karışık çalınır, yoksa tekrar
# eden tek klip konuşma değil mors sinyali gibi duyulur. Alçak olan biraz daha
# uzun sönüyor, böylece ikisi aynı sesin iki vuruşu gibi durmuyor.
def ui_chat():     return blip(freq=684.0, dur=0.110, seed=71, level=0.22)
def ui_chat_low(): return blip(freq=609.0, dur=0.125, seed=73, level=0.20)

# --- ana menü müziği ---------------------------------------------------------

def menu_theme():
 """Neo-noir döngü: 32 saniye, dört akor. Am – F – Dm – E, yavaş.
 Katmanlar: alçak drone, filtreli yastık, seyrek tel, bant hışırtısı.
 Melodi bilerek az: menüde durup düşünen bir oyuncuyu sıkmaması gerekiyor."""
 LOOP, TAIL = 32.0, 4.0
 BAR = 8.0
 out = buf(LOOP + TAIL)

 chords = [
  (110.00, (220.00, 261.63, 329.63)),  # Am
  ( 87.31, (174.61, 220.00, 261.63)),  # F
  ( 73.42, (146.83, 174.61, 220.00)),  # Dm
  ( 82.41, (164.81, 207.65, 246.94)),  # E  (yükselen gerilim, başa döner)
 ]

 pad = buf(LOOP + TAIL)
 for index, (root, triad) in enumerate(chords):
  t0 = index * BAR
  # Drone: kök ve oktavı, hafif detune ile canlı
  dur = BAR + 1.2
  sine(pad, root, 0.24, dur, start=t0, env=swell(1.8, dur))
  sine(pad, root * 1.0015, 0.18, dur, start=t0, env=swell(2.0, dur))
  sine(pad, root * 0.5, 0.16, dur, start=t0, env=swell(2.6, dur))
  # Yastık: üçlünün harmonikleri, üstü kapalı
  for note in triad:
   for harmonic, amp in ((1, 0.10), (2, 0.045), (3, 0.022), (4, 0.010)):
    sine(pad, note * harmonic, amp, dur, start=t0,
         phase=(note * harmonic) % 3.0, env=swell(2.8, dur, release=0.32))
 lowpass(pad, 1300, poles=2)

 # Seyrek tel: minör pentatonik, akor başına iki nota, hep aynı yerde
 # (rastgelelik tohumlu, yani her koşuda aynı parça çıkar).
 rng = random.Random(4)
 scale = [440.00, 523.25, 587.33, 659.25, 783.99]
 lead = buf(LOOP + TAIL)
 for index in range(len(chords)):
  t0 = index * BAR
  for offset in (1.5, 5.0):
   if index == 3 and offset == 5.0: continue   # dördüncü akor nefes alsın
   freq = scale[rng.randrange(len(scale))]
   dur = 3.2
   start = t0 + offset
   sine(lead, freq, 0.13, dur, start=start, env=decay(1.5))
   sine(lead, freq * 2.0, 0.035, dur * 0.6, start=start, env=decay(3.0))
   sine(lead, freq * 1.001, 0.06, dur, start=start, env=decay(1.7))
 lowpass(lead, 4200)
 reverb(lead, mix=0.42, room=0.84, damp=0.28)

 for i in range(len(out)):
  out[i] = pad[i] * 0.78 + lead[i] * 0.85
 soft(out, 1.1)
 reverb(out, mix=0.12, room=0.74)
 out = wrap_tail(out, LOOP)

 # Bant hışırtısı: dijital sessizlik soğuk durur, ince bir zemin ısıtır.
 # Kuyruk sarmasından **sonra** eklenir: sürekli bir katman sarılırsa başta
 # iki katına biner.
 hiss = loop_noise(LOOP, 301, band=(3000, 0.6))
 for i in range(len(out)):
  out[i] += hiss[i] * 0.010
 return normalize(out, 0.62)

def desk_theme():
 """Masanın müziği. Oda gürültüsünün yerini aldı: hava hışırtısı bir süre
 sonra yorucu, üstelik masada oyuncu **okuyor** — okumaya eşlik eden şey
 gürültü değil müzik olmalı. Menü parçasından üç farkı var: daha yavaş (akor
 başına 8 s yerine 10 s), tel yok denecek kadar seyrek (dört akorda iki nota)
 ve belirgin biçimde alçak, çünkü metnin üstünde durmayacak."""
 LOOP, TAIL = 40.0, 5.0
 BAR = 10.0
 out = buf(LOOP + TAIL)

 # Dm – Gm – B♭ – A: menünün Am'sinden bir adım uzak, aynı dünyada.
 chords = [
  ( 73.42, (146.83, 174.61, 220.00)),  # Dm
  ( 98.00, (196.00, 233.08, 293.66)),  # Gm
  (116.54, (233.08, 293.66, 349.23)),  # B♭
  (110.00, (220.00, 277.18, 329.63)),  # A  (gerilim, başa döner)
 ]

 pad = buf(LOOP + TAIL)
 for index, (root, triad) in enumerate(chords):
  t0 = index * BAR
  dur = BAR + 1.6
  # 8 Ekim 2026: alt oktav ve 1.0012 akortsuz ikiz kaldırıldı — ikisi
  # birlikte sürekli bir uğultu (vuru) yapıyordu. Kök yalnız, alçak.
  sine(pad, root,          0.10, dur, start=t0, env=swell(3.0, dur))
  for note in triad:
   for harmonic, amp in ((1, 0.085), (2, 0.032), (3, 0.014)):
    sine(pad, note * harmonic, amp, dur, start=t0,
         phase=(note * harmonic) % 3.0, env=swell(4.2, dur, release=0.38))
 lowpass(pad, 1050, poles=2)   # menüden daha kapalı: masa lambası ışığı gibi
 pad = highpass(pad, 90)       # gövdeyi alttan kes: hoparlörde vızıltı olmasın

 # İki nota, kırk saniyede. Masada müzik olay değil zemin.
 lead = buf(LOOP + TAIL)
 for start, freq in ((6.5, 293.66), (26.0, 349.23)):
  sine(lead, freq,       0.10, 4.0, start=start, env=decay(1.2))
  sine(lead, freq * 2.0, 0.026, 2.4, start=start, env=decay(2.6))
 lowpass(lead, 3400)
 reverb(lead, mix=0.46, room=0.86, damp=0.30)

 for i in range(len(out)):
  out[i] = pad[i] * 0.80 + lead[i] * 0.80
 soft(out, 1.1)
 reverb(out, mix=0.12, room=0.78)
 out = wrap_tail(out, LOOP)

 hiss = loop_noise(LOOP, 307, band=(2600, 0.6))
 for i in range(len(out)):
  out[i] += hiss[i] * 0.004
 return normalize(out, 0.30)


# --- efekt katmanı sesleri (2 Ekim 2026) -------------------------------------
# Masa eşyaları, CCTV cihazı, kapı ve uzak şehir. Hepsi alçak; hiçbiri bir
# kaynağa dikkat çekmez, yalnız dokunulan eşyanın sesidir.

def shaped(seconds, seed, band, q, k, amp, start=0.0, out=None):
 out = out if out is not None else buf(seconds)
 x = noise(seconds, seed); bandpass(x, band, q)
 i0 = int(start * RATE)
 for i in range(len(x)):
  if i0 + i >= len(out): break
  out[i0 + i] += x[i] * amp * math.exp(-k * i / RATE)
 return out

def ui_lamp():
 # Lambanın düğmesi: küçük metal tık, ardından ince bir çınlama.
 out = shaped(0.25, 401, 3200, 2.0, 120, 0.6)
 shaped(0.25, 402, 1400, 3.0, 60, 0.25, start=0.012, out=out)
 sine(out, 4100, 0.02, 0.12, env=decay(30))
 reverb(out, mix=0.12, room=0.55)
 return normalize(fade(out), 0.38)

def ui_paper():
 # Faks kâğıdı makineden sürülür: kısa kâğıt hışırtıları, tık tık.
 out = buf(0.9)
 for k in range(9):
  shaped(0.09, 410 + k, 2600 + 300 * (k % 3), 0.9, 30, 0.25, start=k * 0.095, out=out)
  sine(out, 140, 0.05, 0.03, start=k * 0.095, env=decay(60))
 reverb(out, mix=0.10, room=0.55)
 return normalize(fade(out, 0.004, 0.1), 0.34)

def ui_folder():
 # Karton dosya masaya bırakılır: tok vuruş, kâğıt sesi.
 out = shaped(0.5, 420, 500, 1.0, 22, 0.7)
 sine(out, 78, 0.4, 0.2, env=decay(24))
 shaped(0.5, 421, 3000, 0.8, 9, 0.12, start=0.01, out=out)
 reverb(out, mix=0.16, room=0.68)
 return normalize(fade(out, 0.002, 0.1), 0.48)

def ui_drawer():
 # Çekmece: tahta kızakta kayma, sonda tok duruş.
 out = buf(0.6)
 slide = noise(0.38, 430); bandpass(slide, 900, 1.2)
 for i in range(len(slide)):
  t = i / len(slide)
  out[i] += slide[i] * 0.25 * math.sin(math.pi * t) * (0.7 + 0.3 * math.sin(i / 300))
 shaped(0.2, 431, 400, 1.0, 35, 0.6, start=0.38, out=out)
 sine(out, 95, 0.3, 0.15, start=0.38, env=decay(30))
 reverb(out, mix=0.14, room=0.62)
 return normalize(fade(out, 0.01, 0.06), 0.42)

def ui_dial():
 # Telefon ahizesi kalkar, iki kısa çevir tıkırtısı ve uzak hat sesi.
 out = shaped(0.9, 440, 1800, 2.0, 70, 0.4)
 for k in range(4):
  shaped(0.03, 441 + k, 2400, 3.0, 200, 0.3, start=0.18 + k * 0.07, out=out)
 sine(out, 425, 0.06, 0.45, start=0.42, env=lambda t: min(1, t * 10) * (1 - t))
 lowpass(out, 3400)
 return normalize(fade(out, 0.003, 0.1), 0.34)

def ui_crt_on():
 # Tüp açılır: ince yüksek ıslık, statik bir çıtırtı.
 out = buf(0.6)
 sine(out, 15600, 0.03, 0.6, env=lambda t: min(1, t * 8) * (1 - t))
 shaped(0.06, 450, 2000, 0.7, 50, 0.5, out=out)
 shaped(0.5, 451, 5000, 0.6, 12, 0.06, start=0.02, out=out)
 sine(out, 60, 0.25, 0.12, env=decay(25))
 return normalize(fade(out, 0.002, 0.15), 0.32)

def ui_crt_off():
 out = buf(0.4)
 sine(out, 220, 0.25, 0.3, env=lambda t: (1 - t) ** 2)
 shaped(0.05, 452, 1800, 0.8, 70, 0.35, out=out)
 lowpass(out, 2500)
 return normalize(fade(out, 0.002, 0.1), 0.30)

def ui_static():
 # Sinyal kesildi: geniş bantlı cızırtı, yarım saniye.
 out = buf(0.5)
 x = noise(0.5, 460); highpass(x, 600)
 for i in range(len(x)):
  t = i / len(x)
  out[i] = x[i] * 0.4 * (1 - t) * (0.6 + 0.4 * ((i // 900) % 2))
 return normalize(fade(out, 0.004, 0.12), 0.30)

def ui_door():
 # Uzakta bir kapı kapanır: tok çarpma, koridorun yankısı.
 out = shaped(0.9, 470, 300, 1.0, 14, 0.8)
 sine(out, 55, 0.5, 0.35, env=decay(12))
 shaped(0.9, 471, 2200, 1.5, 40, 0.15, out=out)   # kilidin dili
 lowpass(out, 1800)
 reverb(out, mix=0.35, room=0.84)
 return normalize(fade(out, 0.002, 0.2), 0.48)

def ui_clip():
 # Ataç: iki küçük metal tık.
 out = shaped(0.18, 480, 4200, 3.0, 160, 0.5)
 shaped(0.1, 481, 3600, 3.0, 180, 0.4, start=0.05, out=out)
 return normalize(fade(out), 0.30)

def ui_pin():
 # Raptiye panoya: kısa batma, mantarın tok sesi.
 out = shaped(0.2, 490, 2500, 2.0, 120, 0.4)
 sine(out, 180, 0.25, 0.08, env=decay(50))
 return normalize(fade(out), 0.32)

def ui_pen():
 # Kalem kâğıtta çizgi çeker: kısa, ince sürtünme.
 out = buf(0.32)
 x = noise(0.32, 495); bandpass(x, 4800, 1.4)
 for i in range(len(x)):
  t = i / len(x)
  out[i] = x[i] * 0.3 * math.sin(math.pi * t)
 return normalize(fade(out, 0.01, 0.05), 0.26)

def amb_car():
 # Pencerenin önünden geçen araba: alçak motor, lastik hışırtısı, gelip gider.
 dur = 3.0
 out = buf(dur)
 rumble = noise(dur, 500); lowpass(rumble, 120, poles=2)
 tyre = noise(dur, 501); bandpass(tyre, 700, 0.7)
 for i in range(len(out)):
  t = i / len(out)
  swell_ = math.sin(math.pi * t) ** 2
  out[i] = (rumble[i] * 1.2 + tyre[i] * 0.25) * swell_
 return normalize(fade(out, 0.05, 0.3), 0.22)

def amb_siren():
 # Çok uzakta bir siren: iki ton, perdesi hafif kayan, duvarların ardından.
 dur = 4.5
 out = buf(dur)
 phase = 0.0
 for i in range(len(out)):
  t = i / RATE
  f = 650 + 120 * math.sin(2 * math.pi * t / 1.6)
  phase += 2 * math.pi * f / RATE
  env = math.sin(math.pi * t / dur) ** 2
  out[i] = math.sin(phase) * env * 0.4
 lowpass(out, 900, poles=2)
 reverb(out, mix=0.5, room=0.86)
 return normalize(fade(out, 0.1, 0.4), 0.14)

def amb_dog():
 # Uzakta iki havlama.
 out = buf(1.6)
 for k, start in enumerate((0.1, 0.55)):
  bark = noise(0.18, 510 + k); bandpass(bark, 700, 2.0)
  i0 = int(start * RATE)
  for i in range(len(bark)):
   t = i / len(bark)
   out[i0 + i] += bark[i] * math.sin(math.pi * t) * 0.6
  sine(out, 420, 0.15, 0.15, start=start, env=lambda t: math.sin(math.pi * t))
 lowpass(out, 1500, poles=2)
 reverb(out, mix=0.45, room=0.84)
 return normalize(fade(out, 0.01, 0.3), 0.14)

def amb_radio():
 # Komşu masadan telsiz cızırtısı: anlaşılmaz konuşma bandı, açılıp kapanır.
 out = buf(1.4)
 x = noise(1.4, 520); bandpass(x, 1600, 1.5)
 for i in range(len(x)):
  t = i / RATE
  talk = 0.5 + 0.5 * math.sin(2 * math.pi * 5.3 * t) * math.sin(2 * math.pi * 1.7 * t)
  gate = 1.0 if 0.1 < t < 1.25 else 0.0
  out[i] = x[i] * 0.4 * (0.3 + 0.7 * talk) * gate
 shaped(0.04, 521, 3000, 1.0, 100, 0.4, start=0.08, out=out)   # bas-konuş tıkı
 shaped(0.04, 522, 3000, 1.0, 100, 0.4, start=1.25, out=out)
 return normalize(fade(out, 0.01, 0.1), 0.14)

def interview_theme():
 """Görüşme odasının müziği: neredeyse yok. Alçak bir dron, çok seyrek iki
 nota. Yanıta, kişiye ya da öne sürülen kayda göre **değişmez** — değişseydi
 oyuncu onu gizli bir durum diye okurdu. Ortam sesinin hemen üstünde durur."""
 LOOP, TAIL = 36.0, 4.0
 lead = buf(LOOP + TAIL)
 for start, freq in ((9.0, 233.08), (27.0, 220.0)):
  sine(lead, freq, 0.07, 5.0, start=start, env=decay(0.9))
 reverb(lead, mix=0.5, room=0.86, damp=0.3)
 lead = wrap_tail(lead, LOOP)
 # Dron döngüye tam oturur: her frekans 36 saniyede tam sayıda dönüş yapar,
 # kuyruk eklemeye gerek kalmaz (eklemek başta seviye kamburu yapıyordu).
 out = buf(LOOP)
 for f, a in ((55.0, 0.26), (55.0 + 6 / LOOP, 0.18), (82.5, 0.10), (110.0, 0.05)):
  sine(out, f, a, LOOP, phase=f % 3.0)
 for i in range(len(out)):
  t = i / RATE
  out[i] = out[i] * (0.85 + 0.15 * math.sin(2 * math.pi * t / LOOP)) + lead[i]
 # Süzgeç dikişte sıfırdan başlamasın: iki tur süzülür, ikincisi alınır.
 twice = array.array('d', out); twice.extend(out)
 lowpass(twice, 900, poles=2)
 out = array.array('d', twice[len(out):])
 return normalize(out, 0.32)

# --- sahne katmanı sesleri (2 Ekim 2026, H–O) ---------------------------------

def ui_ring():
 # Hat bağlanıyor: iki kısa çalma tonu, sonra ahize kalkar.
 out = buf(1.6)
 for start in (0.0, 0.7):
  sine(out, 425, 0.18, 0.4, start=start, env=lambda t: min(1, t * 20) * min(1, (1 - t) * 20))
 shaped(0.1, 600, 1600, 2.0, 60, 0.4, start=1.35, out=out)
 lowpass(out, 3400)
 return normalize(fade(out, 0.003, 0.08), 0.30)

def ui_rewind():
 # Bant geri sarılır: yükselen vızıltı ve cızırtı.
 out = buf(0.9)
 phase = 0.0
 x = noise(0.9, 610); bandpass(x, 2500, 1.0)
 for i in range(len(out)):
  t = i / len(out)
  phase += 2 * math.pi * (300 + 900 * t) / RATE
  out[i] = math.sin(phase) * 0.15 * (0.5 + 0.5 * t) + x[i] * 0.2
 return normalize(fade(out, 0.02, 0.08), 0.28)

def ui_key():
 # Klavye tuşu: kuru plastik tık.
 out = shaped(0.06, 620, 3000, 1.6, 90, 0.6)
 sine(out, 210, 0.1, 0.03, env=decay(60))
 return normalize(fade(out), 0.32)

def ui_modem():
 # Bağlantı: çevir sesi, iki tonlu el sıkışma, kısa hışırtı.
 out = buf(1.8)
 sine(out, 1200, 0.15, 0.35, env=lambda t: 1 - t * 0.2)
 sine(out, 2100, 0.12, 0.35, start=0.4)
 sine(out, 980, 0.10, 0.3, start=0.8)
 x = noise(0.7, 630); bandpass(x, 1800, 0.8)
 i0 = int(1.1 * RATE)
 for i in range(len(x)):
  if i0 + i < len(out): out[i0 + i] += x[i] * 0.25 * (1 - i / len(x))
 lowpass(out, 3400)
 return normalize(fade(out, 0.01, 0.1), 0.18)

def amb_thunder():
 # Uzak gök gürültüsü: alçak, yuvarlanan, yavaş sönen.
 dur = 4.0
 x = noise(dur, 640); lowpass(x, 90, poles=3)
 out = buf(dur)
 for i in range(len(x)):
  t = i / RATE
  out[i] = x[i] * (min(1, t * 4) * math.exp(-t * 0.9)) * (1 + 0.5 * math.sin(t * 7))
 reverb(out, mix=0.4, room=0.86)
 return normalize(fade(out, 0.05, 0.5), 0.22)

def amb_phone():
 # Başka bir masada çalan telefon: koridorun ardından, iki çalış.
 out = buf(3.0)
 for start in (0.0, 1.6):
  for k in range(16):
   sine(out, 900, 0.1, 0.03, start=start + k * 0.05)
   sine(out, 1100, 0.08, 0.03, start=start + k * 0.05 + 0.025)
 lowpass(out, 1200, poles=2)
 reverb(out, mix=0.5, room=0.86)
 return normalize(fade(out, 0.01, 0.3), 0.10)

def amb_typing():
 # Komşu odada yazı makinesi: düzensiz tuşlar, bir satır sonu zili.
 rng = random.Random(650)
 out = buf(3.2)
 t = 0.1
 while t < 2.7:
  shaped(0.05, rng.randint(0, 9999), 1800, 1.4, 120, 0.4, start=t, out=out)
  t += rng.uniform(0.08, 0.22)
 sine(out, 2400, 0.08, 0.4, start=2.8, env=decay(8))
 lowpass(out, 2000)
 reverb(out, mix=0.45, room=0.82)
 return normalize(fade(out, 0.01, 0.2), 0.10)

def ui_envelope():
 # Zarf: kâğıt kayar, kapak katlanır.
 out = buf(0.8)
 slide = noise(0.45, 660); bandpass(slide, 2200, 0.8)
 for i in range(len(slide)):
  out[i] += slide[i] * 0.3 * math.sin(math.pi * i / len(slide))
 shaped(0.12, 661, 1200, 1.2, 50, 0.5, start=0.55, out=out)
 return normalize(fade(out, 0.01, 0.06), 0.34)

def ui_fax_warm():
 # Faks ısınır: alçak uğultu, röle tıkı.
 out = buf(1.0)
 sine(out, 100, 0.2, 1.0, env=lambda t: min(1, t * 5) * (1 - t * 0.3))
 sine(out, 200, 0.08, 1.0, env=lambda t: min(1, t * 5))
 shaped(0.04, 670, 2500, 2.0, 150, 0.5, start=0.85, out=out)
 return normalize(fade(out, 0.02, 0.05), 0.24)

def ui_shelf():
 # Klasör rafa kayar: karton sürtünmesi, tahtaya tok duruş.
 out = buf(0.7)
 slide = noise(0.5, 680); bandpass(slide, 1100, 1.0)
 for i in range(len(slide)):
  out[i] += slide[i] * 0.25 * math.sin(math.pi * i / len(slide))
 shaped(0.2, 681, 350, 1.0, 30, 0.6, start=0.5, out=out)
 reverb(out, mix=0.18, room=0.7)
 return normalize(fade(out, 0.01, 0.08), 0.40)

def ui_polaroid():
 # Fotoğraf makineden çıkar: kısa motor vızıltısı.
 out = buf(0.6)
 sine(out, 180, 0.2, 0.45, env=lambda t: math.sin(math.pi * t))
 x = noise(0.45, 690); bandpass(x, 1500, 2.0)
 for i in range(len(x)):
  out[i] += x[i] * 0.1 * math.sin(math.pi * i / len(x))
 return normalize(fade(out, 0.01, 0.05), 0.26)

def ui_rank():
 # Rütbe: tok bir mühür ve tek, alçak bir çan.
 out = shaped(0.3, 700, 600, 1.0, 25, 0.7)
 sine(out, 70, 0.4, 0.2, env=decay(20))
 sine(out, 523.25, 0.12, 1.4, start=0.12, env=decay(3))
 sine(out, 784.0, 0.05, 1.2, start=0.12, env=decay(4))
 reverb(out, mix=0.3, room=0.8)
 return normalize(fade(out, 0.002, 0.2), 0.42)

# --- üretim ------------------------------------------------------------------

def ui_chair():
 # Görüşme biter: sandalye geri itilir, ahşap gıcırtı, ayak.
 out = buf(0.9)
 for k in range(9):
  sine(out, 180 + k * 14, 0.08, 0.05, start=0.05 + k * 0.035, env=decay(30))
 shaped(0.4, 801, 260, 1.2, 9, 0.5, start=0.02, out=out)
 shaped(0.12, 802, 140, 1.0, 40, 0.6, start=0.62, out=out)
 lowpass(out, 1800)
 reverb(out, mix=0.3, room=0.7)
 return normalize(fade(out, 0.005, 0.15), 0.40)

def amb_steps():
 # Koridorda geçen adımlar: kapının ardından, soldan sağa.
 out = buf(3.4)
 for k in range(7):
  shaped(0.09, 820 + k, 150, 1.0, 45, 0.5 + 0.1 * math.sin(k), start=0.2 + k * 0.42, out=out)
  shaped(0.03, 840 + k, 2400, 2.0, 160, 0.12, start=0.2 + k * 0.42, out=out)
 lowpass(out, 900, poles=2)
 reverb(out, mix=0.55, room=0.86)
 return normalize(fade(out, 0.02, 0.3), 0.10)

def amb_vent():
 # Havalandırma: düz, alçak bir hava akışı. Döngü.
 LOOP = 12.0
 out = buf(LOOP)
 air = loop_noise(LOOP, 830, cutoff=420, poles=2, hp=70)
 for i in range(len(out)):
  out[i] = air[i] * (0.92 + 0.08 * math.sin(2 * math.pi * i / RATE / LOOP))
 sine(out, 59.0, 0.012, LOOP)
 return normalize(out, 0.12)

def ui_channel():
 # Analog kanal değişimi: kısa kar, tık.
 out = noise(0.22, 850)
 highpass(out, 900)
 for i in range(len(out)):
  out[i] *= math.exp(-14 * i / RATE)
 shaped(0.02, 851, 3000, 2.0, 200, 0.6, out=out)
 return normalize(fade(out, 0.002, 0.03), 0.28)

def ui_burn():
 # Film yanığı: hışırtı kabarır, çıtırtılar.
 rng = random.Random(860)
 out = noise(1.1, 861)
 bandpass(out, 1400, q=0.8)
 for i in range(len(out)):
  t = i / RATE
  out[i] *= 0.3 * min(1.0, t / 0.5) * math.exp(-1.5 * max(0.0, t - 0.6))
 for k in range(14):
  shaped(0.01, rng.randint(0, 9999), 3500, 2.0, 300, 0.5, start=rng.uniform(0.1, 1.0), out=out)
 return normalize(fade(out, 0.01, 0.15), 0.30)

def ui_tape():
 # Bant hızı değişimi: motor sesi kısa bir glissando.
 out = buf(0.35)
 n = len(out); ph = 0.0
 for i in range(n):
  t = i / n
  ph += 2 * math.pi * (120 + 220 * t) / RATE
  out[i] = 0.3 * math.sin(ph) * math.sin(math.pi * t)
 lowpass(out, 1500)
 return normalize(fade(out, 0.005, 0.05), 0.22)

def case_sting():
 # Vaka motifi: üç alçak nota, piyano gibi, yankılı. Açılışta ve kapanışta.
 out = buf(3.0)
 for start, f in ((0.0, 146.83), (0.45, 174.61), (0.9, 220.0)):
  for h, a in ((1, 0.22), (2, 0.07), (3, 0.03)):
   sine(out, f * h, a, 2.0, start=start, env=decay(2.2))
 reverb(out, mix=0.4, room=0.85)
 return normalize(fade(out, 0.005, 0.5), 0.35)

def desk_layer():
 # Masa müziğinin üst katmanı: çok alçak bir tel dokusu. Süreyle açılır.
 LOOP = 32.0
 out = buf(LOOP)
 for f in (293.66, 349.23, 440.0):
  sine(out, f, 0.04, LOOP)
  sine(out, f * 1.003, 0.03, LOOP)
 n = len(out)
 for i in range(n):
  out[i] *= 0.7 + 0.3 * math.sin(2 * math.pi * i / n)
 lowpass(out, 1600)
 return normalize(steady_reverb(out, mix=0.3), 0.10)

def amb_steps_tile():
 # Görüşme odasının önünde fayans: daha sert topuk, kısa yankı.
 out = buf(3.0)
 for k in range(6):
  shaped(0.06, 860 + k, 420, 1.4, 70, 0.45, start=0.2 + k * 0.45, out=out)
  shaped(0.03, 870 + k, 3600, 2.5, 200, 0.2, start=0.2 + k * 0.45, out=out)
 lowpass(out, 1600, poles=2)
 reverb(out, mix=0.35, room=0.6)
 return normalize(fade(out, 0.02, 0.3), 0.10)

def ui_printer():
 # Nokta vuruşlu yazıcı: hızlı tık dizileri, satır sonunda kâğıt ilerletme.
 out = buf(1.3)
 for line in range(4):
  for k in range(14):
   shaped(0.02, 900 + line * 20 + k, 2800, 2.5, 260, 0.35, start=line * 0.3 + k * 0.016, out=out)
  shaped(0.08, 990 + line, 300, 1.0, 40, 0.3, start=line * 0.3 + 0.24, out=out)
 reverb(out, mix=0.1, room=0.5)
 return normalize(fade(out), 0.42)

def ui_drop_photo():
 # Fotoğraf masaya: ince, düz bir şap.
 out = shaped(0.15, 1001, 2200, 1.2, 70, 0.6)
 shaped(0.15, 1002, 600, 1.0, 50, 0.2, out=out)
 return normalize(fade(out), 0.30)

def ui_drop_file():
 # Dosya masaya: kalın kâğıt destesi, tok.
 out = shaped(0.25, 1011, 380, 1.0, 30, 0.7)
 shaped(0.1, 1012, 2600, 1.0, 90, 0.2, out=out)
 reverb(out, mix=0.1, room=0.5)
 return normalize(fade(out), 0.36)

def ui_drop_bag():
 # Delil torbası: plastik hışırtı ve içindeki nesnenin tok sesi.
 out = buf(0.4)
 for k in range(5):
  shaped(0.06, 1020 + k, 4200 + 400 * k, 1.5, 60, 0.2, start=k * 0.03, out=out)
 shaped(0.2, 1030, 220, 1.2, 25, 0.6, start=0.05, out=out)
 return normalize(fade(out), 0.36)

SOUNDS = [
 ("ui_press",        ui_press),
 ("ui_typewriter",   ui_typewriter),
 ("ui_stamp",        ui_stamp),
 ("ui_notification", ui_notification),
 ("ui_chat",         ui_chat),
 ("ui_chat_low",     ui_chat_low),
 ("room_interview",  room_interview),
 ("room_rain",       room_rain),
 ("room_night",      room_night),
 ("menu_theme",      menu_theme),
 ("desk_theme",      desk_theme),
 ("ui_lamp", ui_lamp),
 ("ui_paper", ui_paper),
 ("ui_folder", ui_folder),
 ("ui_drawer", ui_drawer),
 ("ui_dial", ui_dial),
 ("ui_crt_on", ui_crt_on),
 ("ui_crt_off", ui_crt_off),
 ("ui_static", ui_static),
 ("ui_door", ui_door),
 ("ui_clip", ui_clip),
 ("ui_pin", ui_pin),
 ("ui_pen", ui_pen),
 ("amb_car", amb_car),
 ("amb_siren", amb_siren),
 ("amb_dog", amb_dog),
 ("amb_radio", amb_radio),
 ("interview_theme", interview_theme),
 ("ui_ring", ui_ring),
 ("ui_rewind", ui_rewind),
 ("ui_key", ui_key),
 ("ui_modem", ui_modem),
 ("amb_thunder", amb_thunder),
 ("amb_phone", amb_phone),
 ("amb_typing", amb_typing),
 ("ui_envelope", ui_envelope),
 ("ui_fax_warm", ui_fax_warm),
 ("ui_shelf", ui_shelf),
 ("ui_polaroid", ui_polaroid),
 ("ui_rank", ui_rank),
 ("ui_chair", ui_chair),
 ("amb_steps", amb_steps),
 ("amb_vent", amb_vent),
 ("ui_channel", ui_channel),
 ("ui_burn", ui_burn),
 ("ui_tape", ui_tape),
 ("case_sting", case_sting),
 ("desk_layer", desk_layer),
 ("amb_steps_tile", amb_steps_tile),
 ("ui_printer", ui_printer),
 ("ui_drop_photo", ui_drop_photo),
 ("ui_drop_file", ui_drop_file),
 ("ui_drop_bag", ui_drop_bag),
]

if __name__ == "__main__":
 import sys
 only = set(sys.argv[1:])
 print("Karine ses varlıkları:")
 for name, make in SOUNDS:
  if only and name not in only: continue
  write(name, make())
