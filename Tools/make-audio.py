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
 # Mekanik plastik düğme: gövdenin alçak vuruşu + parmağın kuru tıkı.
 out = buf(0.18)
 body = noise(0.03, 11); bandpass(body, 1700, 2.2); lowpass(body, 4200)
 for i in range(len(body)):
  out[i] += body[i] * 0.55 * math.exp(-38 * i / RATE)
 sine(out, 96, 0.30, 0.12, env=decay(26))
 sine(out, 184, 0.10, 0.06, env=decay(40))
 reverb(out, mix=0.10, room=0.62)
 return normalize(fade(out), 0.52)

def ui_page():
 # Kâğıt. Tek gürültü patlaması "ıss" olur; kâğıt üç düzensiz sürtünmedir.
 out = buf(0.42)
 rng = random.Random(7)
 for k, (start, dur, freq, amp) in enumerate(
   ((0.00, 0.16, 3600, 0.45), (0.07, 0.20, 2500, 0.55), (0.17, 0.22, 4800, 0.35))):
  part = noise(dur, 20 + k)
  bandpass(part, freq, 1.1); highpass(part, 900)
  i0 = int(start * RATE); n = len(part)
  for i in range(n):
   t = i / n
   # lifli doku: kâğıdın kendi düzensizliği
   grain = 0.65 + 0.35 * rng.random()
   env = (t ** 0.35) * math.exp(-4.5 * t) * grain
   if i0 + i < len(out): out[i0 + i] += part[i] * amp * env
 reverb(out, mix=0.14, room=0.66)
 return normalize(fade(out, 0.002, 0.06), 0.46)

def ui_typewriter():
 # Daktilo tuşu: çelik kolun tıkı + kâğıda vuruş + gövdenin alçak tokluğu.
 out = buf(0.16)
 click = noise(0.012, 31); highpass(click, 2600)
 for i in range(len(click)):
  out[i] += click[i] * 0.7 * math.exp(-90 * i / RATE)
 hit = noise(0.05, 32); bandpass(hit, 1200, 1.6)
 for i in range(len(hit)):
  out[i] += hit[i] * 0.42 * math.exp(-45 * i / RATE)
 sine(out, 132, 0.24, 0.09, env=decay(34))
 sine(out, 2950, 0.05, 0.05, env=decay(70))   # kolun ince çınlaması
 reverb(out, mix=0.12, room=0.60)
 return normalize(fade(out), 0.50)

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
 tick = noise(0.01, 51); highpass(tick, 3000)
 for i in range(len(tick)):
  out[i] += tick[i] * 0.35 * math.exp(-110 * i / RATE)
 base = 784.0  # G5
 for ratio, amp, k in ((1.00, 0.34, 3.2), (2.76, 0.16, 4.4), (5.40, 0.07, 6.5),
                       (8.93, 0.03, 9.0), (1.002, 0.30, 3.0)):
  sine(out, base * ratio, amp, 1.25, start=0.004, env=decay(k))
 reverb(out, mix=0.26, room=0.78)
 return normalize(fade(out, 0.001, 0.12), 0.60)

# --- oda ortamları -----------------------------------------------------------
# İkisi de döngü. Olay yok, gelişme yok: oda ne yaparsan yapsın aynı kalır.
# Bilinçli olarak **çok alçak** — ortam sesi fark edilmemek için vardır.

def room_office():
 LOOP = 24.0
 out = buf(LOOP)
 n = len(out)

 # Havalandırmanın alçak uğultusu, uzakta trafik, ince oda tonu. Üçü de
 # döngüye dikişsiz oturan gürültü katmanı.
 rumble  = loop_noise(LOOP, 101, cutoff=95, poles=3)
 traffic = loop_noise(LOOP, 102, cutoff=520, poles=2, hp=70)
 air     = loop_noise(LOOP, 103, band=(2400, 0.7))

 for i in range(n):
  t = i / RATE
  # Nefes: periyodu döngü boyunun tam böleni, yoksa dikişte atlar
  breath = 0.72 + 0.28 * math.sin(2*math.pi*t/LOOP) * math.sin(4*math.pi*t/LOOP + 1.1)
  out[i] = rumble[i] * 0.55 + traffic[i] * 0.22 * breath + air[i] * 0.05

 # Floresan: şebeke 50 Hz'in ikinci katı ve harmonikleri. Frekanslar döngü
 # boyunda tam sayı çevrim yapar (100 × 24 = 2400), dikişte faz atlamaz.
 for freq, amp in ((100.0, 0.030), (200.0, 0.014), (300.0, 0.006)):
  sine(out, freq, amp, LOOP)

 # Duvardaki saat: tam bir saniyede bir, kuru ve alçak. Yalnız döngünün içine
 # konur; kuyruğu yankı taşır, yoksa başta çiftlenir.
 for second in range(int(LOOP)):
  tick = loop_noise(0.014, 200 + second, band=(2600, 2.5), hp=1400, xf=0.0)
  i0 = int(second * RATE)
  strong = 0.030 if second % 2 == 0 else 0.022   # tik / tak
  for i in range(len(tick)):
   if i0 + i < n: out[i0 + i] += tick[i] * strong * math.exp(-75 * i / RATE)

 return normalize(steady_reverb(out, mix=0.16, room=0.70), 0.30)

def room_interview():
 # Görüşme odası: trafik yok, saat yok. Daha derin uğultu, daha yakın floresan,
 # tavanda ince bir çınlama. Kapalı ve baskılı.
 LOOP = 24.0
 out = buf(LOOP)
 n = len(out)
 rumble = loop_noise(LOOP, 111, cutoff=70, poles=3)
 close  = loop_noise(LOOP, 112, cutoff=240, poles=2, hp=45)
 for i in range(n):
  t = i / RATE
  breath = 0.78 + 0.22 * math.sin(2*math.pi*t/LOOP + 0.6)
  out[i] = rumble[i] * 0.62 + close[i] * 0.18 * breath

 # Floresan daha yakın, ayrıca balastın kararsız titremesi. Titreme gürültüyle
 # değil yavaş salınım toplamıyla yapılır: sınırlı kalır ve döngüye oturur.
 for freq, amp in ((100.0, 0.052), (200.0, 0.026), (400.0, 0.010), (600.0, 0.005)):
  sine(out, freq, amp, LOOP)
 for i in range(n):
  t = i / RATE
  flicker = (math.sin(2*math.pi*3*t/LOOP) + math.sin(2*math.pi*7*t/LOOP + 2.1)
             + math.sin(2*math.pi*11*t/LOOP + 0.7)) / 3.0
  out[i] *= 1.0 + 0.09 * flicker

 # Tavandaki ince çınlama: tek, sabit, rahatsız etmeyecek kadar alçak.
 # 3990 Hz döngü boyunda tam çevrim yapar (3990 × 24 = 95.760).
 sine(out, 3990.0, 0.0045, LOOP)
 return normalize(steady_reverb(out, mix=0.22, room=0.80, damp=0.25), 0.30)

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

# --- üretim ------------------------------------------------------------------

SOUNDS = [
 ("ui_press",        ui_press),
 ("ui_page",         ui_page),
 ("ui_typewriter",   ui_typewriter),
 ("ui_stamp",        ui_stamp),
 ("ui_notification", ui_notification),
 ("room_office",     room_office),
 ("room_interview",  room_interview),
 ("menu_theme",      menu_theme),
]

if __name__ == "__main__":
 print("Karine ses varlıkları:")
 for name, make in SOUNDS:
  write(name, make())
