#!/usr/bin/env python3
# Ses varlıklarını **ölçer**. Kulak yerine geçmez, ama duyulur kusurların
# çoğunu sayıyla yakalar: kırpma, DC kayması, ölü sessizlik, yanlış seviye ve
# döngü dikişi. Özellikle dikiş: döngülü bir ortam sesinde başta seviye
# yükselmesi ya da örnek atlaması her turda duyulan bir "tık" demektir.
#
#   python3 Tools/check-audio.py

import array, math, os, sys, wave

HERE = os.path.dirname(os.path.abspath(__file__))
AUDIO = os.path.normpath(os.path.join(HERE, "..", "Assets", "Bube", "Resources", "Bube", "Audio"))
LOOPS = {"room_office", "room_interview", "menu_theme"}
# Beklenen seviye aralığı (dBFS, RMS). Arayüz sesi alçak, ortam çok daha alçak.
# Arayüz sesi alçak, oda ortamı çok daha alçak. Ürkütmeme kararından sonra
# odaların üst sınırı indirildi: bir oda ortamı fark edilirse zaten gürültüdür.
BANDS = {"ui": (-30.0, -11.0), "voice": (-30.0, -12.0),
         "room": (-44.0, -26.0), "menu": (-26.0, -11.0)}

def db(v): return 20 * math.log10(max(v, 1e-9))

def rms(x, i0=0, i1=None):
 i1 = len(x) if i1 is None else i1
 if i1 <= i0: return 0.0
 return math.sqrt(sum(v*v for v in x[i0:i1]) / (i1 - i0))

def read(path):
 with wave.open(path, 'rb') as f:
  assert f.getsampwidth() == 2 and f.getnchannels() == 1, "mono 16 bit bekleniyordu"
  rate = f.getframerate()
  raw = array.array('h'); raw.frombytes(f.readframes(f.getnframes()))
 return [v / 32768.0 for v in raw], rate

def band_for(name):
 for prefix, band in BANDS.items():
  if name.startswith(prefix): return band
 return (-40.0, -6.0)

def check(name, path):
 x, rate = read(path)
 problems = []
 peak = max(abs(v) for v in x)
 clipped = sum(1 for v in x if abs(v) >= 0.999)
 dc = sum(x) / len(x)
 # Darbeli arayüz sesinde dosya boyu RMS yanıltır (sesin yarısı kuyruk
 # sessizliğidir); en gürültülü 100 ms penceresi ölçülür.
 if name in LOOPS: level = db(rms(x))
 else:
  win = int(0.1 * rate); step = max(1, win // 4)
  level = db(max(rms(x, i, i + win) for i in range(0, max(1, len(x) - win), step)))
 low, high = band_for(name)
 if clipped > 2: problems.append("kırpılmış örnek: %d" % clipped)
 if abs(dc) > 0.01: problems.append("DC kayması %.3f" % dc)
 if not (low <= level <= high):
  problems.append("seviye %.1f dBFS, beklenen %.0f..%.0f" % (level, low, high))

 # Ölü sessizlik: hiçbir 0,5 saniyelik pencere tamamen boş olmamalı
 win = int(0.5 * rate)
 for i in range(0, len(x) - win, win):
  if rms(x, i, i + win) < 1e-5:
   problems.append("sessiz pencere: %.1f s" % (i / rate)); break

 seam = ""
 if name in LOOPS:
  # 1) Örnek atlaması: dikişteki adım, yerel genliğe göre küçük olmalı
  local = max(rms(x, 0, int(0.05*rate)), rms(x, len(x)-int(0.05*rate)), 1e-6)
  step = abs(x[0] - x[-1]) / local
  # 2) Seviye basamağı: ilk saniyeler ortalamadan sapmamalı. Kuyruğu başa
  #    **eklemek** (çapraz geçirmek yerine) tam burada 3 dB'lik kambur yapar.
  head = db(rms(x, 0, int(3.0*rate)))
  whole = db(rms(x))
  tail = db(rms(x, len(x)-int(3.0*rate)))
  seam = "dikiş adımı %.2f · baş %+.1f dB · son %+.1f dB" % (step, head-whole, tail-whole)
  if step > 1.5: problems.append("dikişte örnek atlaması (%.2f)" % step)
  # Seviye basamağı ortam sesi için kusurdur, müzik için değil: dördüncü akor
  # bilerek nefes alır, yani menü müziğinde sonda alçalmak tasarımın parçası.
  if name != "menu_theme":
   if abs(head - whole) > 1.5: problems.append("döngü başında seviye kamburu %+.1f dB" % (head-whole))
   if abs(tail - whole) > 1.5: problems.append("döngü sonunda seviye düşüşü %+.1f dB" % (tail-whole))

 print("  %-16s %5.1f s  tepe %.2f  RMS %6.1f dBFS  %s" % (name, len(x)/rate, peak, level, seam))
 for problem in problems: print("      ! " + problem)
 return problems

def goertzel(x, freq, rate, i0, n):
 """Tek frekansın gücü. Akorun gerçekten çaldığını ölçmek için yeter."""
 w = 2 * math.pi * freq / rate
 c = 2 * math.cos(w)
 s1 = s2 = 0.0
 for i in range(i0, min(i0 + n, len(x))):
  s0 = x[i] + c * s1 - s2
  s2, s1 = s1, s0
 return math.sqrt(max(0.0, s1*s1 + s2*s2 - c*s1*s2)) / n

# Menü müziğinin dört akoru. Akor gerçekten duyuluyorsa kökün gücü, akora
# yabancı bir aralıktan (triton) belirgin biçimde yüksek olmalı.
CHORDS = [(0.0, 220.00, 311.13), (8.0, 174.61, 246.94),
          (16.0, 146.83, 207.65), (24.0, 164.81, 233.08)]

def check_chords(path):
 x, rate = read(path)
 problems = []
 for start, root, tritone in CHORDS:
  i0 = int((start + 4.0) * rate); n = int(0.75 * rate)
  strong, weak = goertzel(x, root, rate, i0, n), goertzel(x, tritone, rate, i0, n)
  ratio = db(strong) - db(weak)
  print("      akor %4.0f s  kök %.0f Hz  yabancıdan %+5.1f dB" % (start, root, ratio))
  if ratio < 6.0:
   problems.append("%.0f s akoru duyulmuyor (%+.1f dB)" % (start, ratio))
 return problems

if __name__ == "__main__":
 if not os.path.isdir(AUDIO):
  print("Ses klasörü yok: " + AUDIO); sys.exit(1)
 print("Karine ses ölçümü:")
 found = 0; failed = 0
 for file in sorted(os.listdir(AUDIO)):
  if not file.endswith(".wav"): continue
  found += 1
  name = file[:-4]
  failed += len(check(name, os.path.join(AUDIO, file)))
  if name == "menu_theme":
   found_problems = check_chords(os.path.join(AUDIO, file))
   for problem in found_problems: print("      ! " + problem)
   failed += len(found_problems)
 print("  %d dosya, %d bulgu" % (found, failed))
 sys.exit(1 if failed else 0)
