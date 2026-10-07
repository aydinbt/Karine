#!/usr/bin/env python3
"""Çeviri denetimi — Unity açmadan, Editor doğrulayıcısındaki TranslationRules'un aynısı.

Kullanım:
  python3 Tools/check-translation.py en            # bütün dosyalar
  python3 Tools/check-translation.py en case014    # yalnız bir vaka (ve ortak dosya)

Türkçe kanondur. Her çeviri satırı için:
  · anahtar kanonda olmalı, boş olmamalı;
  · {0} gibi yer tutucular aynı kalmalı;
  · kişi adı (personNameKey) birebir aynı kalmalı;
  · bir metin kanonda hangi kişileri anıyorsa çeviride de yalnız onları anmalı
    (kayıt ancak kişinin adı geçiyorsa öne sürülür — oyuncu ekranda aynısını görmeli);
  · gerçek kurum adı geçmemeli; `.demeanor` satırı hüküm vermemeli.
Eksik satır sorun değildir (oyun İngilizceye, sonra Türkçeye düşer) ama sayılır.
"""
import glob, json, os, re, sys

ROOT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..")
LOC = os.path.join(ROOT, "Assets/Bube/Resources/Bube/Locales")
CASES = os.path.join(ROOT, "Assets/Bube/Resources/Bube/Cases")

INSTITUTIONS = {
    "*": ["interpol", "europol", "fbi", "cia", "nsa", "dea", "atf", "mi5", "mi6", "nypd", "lapd",
          "scotland yard", "met police", "metropolitan police", "rcmp", "bka", "lka", "bnd", "dgsi", "dgse",
          "carabinieri", "guardia civil", "policía nacional", "mossos", "ertzaintza", "gendarmerie",
          "police nationale", "polizia di stato", "guardia di finanza", "afp", "nca", "ofsted", "cps"],
    "en": ["crown prosecution service", "home office", "department of justice", "homeland security"],
    "de": ["bundespolizei", "bundeskriminalamt", "landeskriminalamt", "staatsanwaltschaft", "kripo"],
    "fr": ["police judiciaire", "parquet de paris", "ministère de l'intérieur", "brigade criminelle"],
    "it": ["polizia di stato", "procura della repubblica", "ministero dell'interno", "squadra mobile"],
    "es": ["ministerio del interior", "fiscalía", "audiencia nacional"],
    "pt-BR": ["polícia federal", "polícia civil", "polícia militar", "ministério público"],
}
VERDICTS = {
    "en": "lie lies lying liar lied contradiction contradictory truth guilty innocent hiding hides concealing sincere insincere genuine fear afraid panicked relieved",
    "de": "lüge lügt lügner gelogen widerspruch widersprüchlich wahrheit schuldig unschuldig verbirgt verheimlicht aufrichtig unaufrichtig angst panik erleichtert",
    "fr": "mensonge ment menteur menteuse menti contradiction contradictoire vérité coupable innocent innocente cache dissimule sincère peur paniqué paniquée soulagé soulagée",
    "it": "bugia mente bugiardo bugiarda mentito contraddizione contraddittorio verità colpevole innocente nasconde sincero sincera paura panico sollevato sollevata",
    "es": "mentira miente mentiroso mentirosa mintió contradicción contradictorio verdad culpable inocente oculta esconde sincero sincera miedo pánico aliviado aliviada",
    "pt-BR": "mentira mente mentiroso mentirosa mentiu contradição contraditório verdade culpado culpada inocente esconde oculta sincero sincera medo pânico aliviado aliviada",
}
PLACEHOLDER = re.compile(r"\{\d+\}")
# Unvan çevrilir ("Komiser" → "Inspector"), ad çevrilmez.
TITLES = {"Dr.", "Doç.", "Prof.", "Komiser", "Av."}


def bare(full):
    parts = full.split(" ")
    while parts and parts[0] in TITLES:
        parts = parts[1:]
    return " ".join(parts)


def entries(path):
    with open(path, encoding="utf-8") as f:
        return json.load(f).get("entries", [])


def merged(code):
    out = {}
    files = [os.path.join(LOC, code + ".json")] + sorted(glob.glob(os.path.join(LOC, code + ".case*.json")))
    for p in files:
        if os.path.exists(p):
            for e in entries(p):
                out.setdefault(e["key"], e.get("value") or "")
    return out


def people(canon):
    names, name_keys = {}, set()
    for p in glob.glob(os.path.join(CASES, "*.json")):
        data = json.load(open(p, encoding="utf-8"))
        found = set()
        for n in data.get("nodes") or []:
            key = n.get("personNameKey")
            if not key or key not in canon:
                continue
            name_keys.add(key)
            first = bare(canon[key]).split(" ")[0].strip(".,")
            if len(first) > 2 and first[0].isupper():
                found.add(first)
        names[data["id"]] = sorted(found)
    return names, name_keys


def mentions(text, name, canon_side):
    tail = "" if canon_side else r"(?!\w)"
    return re.search(r"(?<!\w)" + re.escape(name) + tail, text or "") is not None


def word(text, phrase):
    return re.search(r"(?<![\w])" + re.escape(phrase) + r"(?![\w])", text) is not None


def check(code, only=None):
    canon = merged("tr")
    names, name_keys = people(canon)
    inst = INSTITUTIONS["*"] + INSTITUTIONS.get(code, [])
    verdicts = set(VERDICTS.get(code, "").split())
    files = [os.path.join(LOC, code + ".json")] + sorted(glob.glob(os.path.join(LOC, code + ".case*.json")))
    if only:
        files = [f for f in files if f.endswith(code + ".json") or f.endswith("." + only + ".json")]
    problems, seen = [], set()
    for path in files:
        if not os.path.exists(path):
            continue
        for e in entries(path):
            k, v = e.get("key"), e.get("value") or ""
            where = os.path.basename(path) + " :: " + str(k)
            if not k:
                problems.append(where + " — anahtarsız giriş"); continue
            if k in seen:
                problems.append(where + " — yinelenen anahtar"); continue
            seen.add(k)
            if k not in canon:
                problems.append(where + " — kanonda olmayan anahtar"); continue
            src = canon[k]
            if src.strip() and not v.strip():
                problems.append(where + " — boş çeviri")
            if sorted(PLACEHOLDER.findall(src)) != sorted(PLACEHOLDER.findall(v)):
                problems.append(where + " — yer tutucular farklı")
            low = v.lower()
            hit = next((i for i in inst if word(low, i)), None)
            if hit:
                problems.append(where + f" — gerçek kurum adı: {hit}")
            if k.endswith(".demeanor"):
                bad = next((w for w in re.split(r"[^\w]+", low) if w in verdicts), None)
                if bad:
                    problems.append(where + f" — davranış satırı hüküm veriyor: {bad}")
            if k in name_keys and not v.endswith(bare(src)):
                problems.append(where + f" — kişi adı değişmiş: {src!r} → {v!r}")
            case_id = k.split(".")[0] if k.startswith("case") else None
            for n in names.get(case_id, []):
                a, b = mentions(src, n, True), mentions(v, n, False)
                if a and not b:
                    problems.append(where + f" — kanonda geçen ad çeviride yok: {n}")
                if b and not a:
                    problems.append(where + f" — kanonda geçmeyen ad eklenmiş: {n}")
    scope = [k for k in canon if not only or k.startswith(only + ".") or not k.startswith("case")]
    missing = [k for k in scope if k not in seen]
    for p in problems:
        print("SORUN  " + p)
    print(f"{code}{' ' + only if only else ''}: {len(problems)} sorun, {len(missing)}/{len(scope)} satır eksik")
    return 1 if problems else 0


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print(__doc__); sys.exit(2)
    sys.exit(check(sys.argv[1], sys.argv[2] if len(sys.argv) > 2 else None))
