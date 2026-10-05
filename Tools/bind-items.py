#!/usr/bin/env python3
"""Bulgu görsellerini vaka verisine bağlar.

Tools/item_bindings.json her bulgunun hangi belgeye (node) ait olduğunu, adını ve açıklamasını tutar.
Görseli Assets/Bube/Resources/Bube/Items/<vaka>_<id>.png|jpg olarak mevcut olan bulgular
o belgenin relatedItems listesine eklenir, ad/açıklama tr.<vaka>.json'a yazılır.
Görseli olmayanlar atlanır: doğrulayıcı (CaseRules) olmayan görseli reddeder.
Tekrar çalıştırmak güvenlidir; zaten bağlı olan bulgu yeniden eklenmez.

  python3 Tools/bind-items.py            # bağla
  python3 Tools/bind-items.py --check    # yalnız rapor
"""
import json, os, re, sys, uuid

ROOT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Assets", "Bube", "Resources", "Bube")
check = "--check" in sys.argv

def load(p): return json.load(open(p, encoding="utf-8"))
def save(p, d): open(p, "w", encoding="utf-8").write(json.dumps(d, ensure_ascii=False, indent=1))

def image_path(name):
    for ext in (".png", ".jpg", ".jpeg"):
        p = os.path.join(ROOT, "Items", name + ext)
        if os.path.exists(p): return p
    return None

TEMPLATE = os.path.join(ROOT, "Items", "case012_lock.png.meta")

def fix_meta(img):
    """Unity'nin varsayılanı nPOTScale: 1 görseli ikinin kuvvetine esnetir; doğrulayıcı bunu reddeder.
    .meta yoksa mevcut bulgu ayarı yeni guid ile kopyalanır; varsa nPOTScale 0 yapılır."""
    meta = img + ".meta"
    if os.path.exists(meta):
        s = open(meta, encoding="utf-8").read()
        t = re.sub(r"nPOTScale: \d+", "nPOTScale: 0", s)
        if t != s: open(meta, "w", encoding="utf-8").write(t); print("ayar düzeltildi", os.path.basename(meta))
    else:
        s = open(TEMPLATE, encoding="utf-8").read()
        open(meta, "w", encoding="utf-8").write(re.sub(r"guid: \w+", "guid: " + uuid.uuid4().hex, s, count=1))
        print("ayar yazıldı", os.path.basename(meta))

bindings = load(os.path.join(os.path.dirname(os.path.abspath(__file__)), "item_bindings.json"))
bound = missing = already = 0
for case, items in sorted(bindings.items()):
    cpath = os.path.join(ROOT, "Cases", case + ".json")
    lpath = os.path.join(ROOT, "Locales", "tr." + case + ".json")
    data, loc = load(cpath), load(lpath)
    nodes = {n["id"]: n for n in data["nodes"]}
    keys = {e["key"]: e for e in loc["entries"]}
    changed = False
    for it in items:
        res = "Bube/Items/%s_%s" % (case, it["id"])
        node = nodes.get(it["node"])
        if node is None:
            print("HATA %s: belge yok: %s" % (case, it["node"])); continue
        if any(r.get("imageResource") == res for r in node.get("relatedItems") or []):
            already += 1; continue
        img = image_path(res.split("/")[-1])
        if not img:
            missing += 1; continue
        bound += 1
        if check: print("bağlanacak %s → %s" % (res, it["node"])); continue
        fix_meta(img)
        nk, dk = "%s.item.%s.name" % (case, it["id"]), "%s.item.%s.detail" % (case, it["id"])
        for k, v in ((nk, it["name"]), (dk, it["detail"])):
            if k in keys: keys[k]["value"] = v
            else: loc["entries"].append({"key": k, "value": v}); keys[k] = loc["entries"][-1]
        node.setdefault("relatedItems", []).append({"nameKey": nk, "detailKey": dk, "imageResource": res})
        changed = True
        print("bağlandı %s → %s" % (res, it["node"]))
    if changed:
        save(cpath, data); save(lpath, loc)
print("%s: %d, zaten bağlı: %d, görseli bekleyen: %d" % ("bağlanacak" if check else "bağlandı", bound, already, missing))
