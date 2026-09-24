#!/usr/bin/env bash
# Karine — testleri komut satırından koşar (EditMode ve/veya PlayMode).
#
#   Tools/run-tests.sh                # ikisi de
#   Tools/run-tests.sh EditMode       # yalnız biri
#
# İki incelik var, ikisi de bir oturum kaybettirdi:
#  1) Unity batchmode kendi lisans istemcisini kuramıyor. Unity Hub ya da Editor
#     açıkken var olan oturum kanalına bağlanmak gerekiyor; kanal adı çalışan
#     Unity süreçlerinden okunur. Bu yüzden Unity Hub'ın açık olması şart.
#  2) `-runTests` ile `-quit` birlikte kullanılmaz — Unity testler başlamadan çıkar
#     ve sonuç dosyası hiç yazılmaz.
# Editor aynı projeyi kilitlediği için proje geçici bir kopyaya alınır.
set -euo pipefail

UNITY="${UNITY:-/Applications/Unity/Hub/Editor/6000.3.17f1/Unity.app/Contents/MacOS/Unity}"
PROJECT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WORK="${KARINE_TEST_DIR:-${TMPDIR:-/tmp}/karine-tests}"
PLATFORMS=("${@:-EditMode PlayMode}")
read -r -a PLATFORMS <<< "${PLATFORMS[*]}"

# Lisans kanalını bul. Kaynak, çalışan Unity.Licensing.Client süreçlerinin
# `--namedPipe Unity-<kanal>` argümanıdır. Unity Hub'ın açtığı istemcinin kanalı
# oturuma özgü bir dizedir (LicenseClient-<rastgele>); Editor sürümüne göre
# türetilmiş `...-berataydin` / `...-6000.3.17` kanalları batchmode'da açılmıyor.
# Yanlış kanal verilirse Unity 60 sn zaman aşımına düşer, lisanssız devam eder ve
# paket çözümlemesi bozulduğu için SAHTE derleme hataları üretir — bu, bir oturum
# boyunca "batchmode lisansı bozuk" sanılmasına yol açtı.
channels="$(ps -Ao args= | tr ' ' '\n' | grep '^Unity-LicenseClient-' | sed 's/^Unity-//' | sort -u)"
CHANNEL="$(printf '%s\n' "$channels" | grep -v -- '-[0-9]' | grep -vx "LicenseClient-$(id -un)" | head -1 || true)"
[ -z "$CHANNEL" ] && CHANNEL="$(printf '%s\n' "$channels" | head -1 || true)"
if [ -z "$CHANNEL" ]; then
  echo "Lisans kanalı bulunamadı. Unity Hub'ı açıp tekrar deneyin." >&2
  exit 1
fi
echo "Lisans kanalı: $CHANNEL"

mkdir -p "$WORK"
rsync -a --delete \
  --exclude Library --exclude Temp --exclude Logs --exclude UserSettings \
  --exclude obj --exclude .git \
  "$PROJECT/" "$WORK/proj/"

STATUS=0
for platform in "${PLATFORMS[@]}"; do
  echo "== $platform =="
  results="$WORK/$platform.xml"; log="$WORK/$platform.log"
  rm -f "$results"
  # PlayMode -nographics ile de koşuyor, ama grafik bağlamı gerektiren bir test
  # eklenirse bayrağı kaldırmak gerekir.
  "$UNITY" -batchmode -nographics -projectPath "$WORK/proj" \
    -runTests -testPlatform "$platform" -testResults "$results" -logFile "$log" \
    -acceptSoftwareTermsForThisRunOnly -licensingIpc "$CHANNEL" >/dev/null 2>&1 || true
  if [ ! -f "$results" ]; then
    echo "  sonuç dosyası yazılmadı — derleme hatası olabilir:"
    grep -m 10 "error CS" "$log" || tail -5 "$log"
    STATUS=1; continue
  fi
  python3 - "$results" <<'PY' || STATUS=1
import sys, xml.etree.ElementTree as ET
root = ET.parse(sys.argv[1]).getroot()
print("  toplam=%s geçti=%s düştü=%s" % (root.get('total'), root.get('passed'), root.get('failed')))
failed = [tc for tc in root.iter('test-case') if tc.get('result') != 'Passed']
for tc in failed:
    message = tc.find('.//message')
    print("  DÜŞTÜ", tc.get('name'), (message.text or '').strip()[:300] if message is not None else '')
sys.exit(1 if failed else 0)
PY
done
exit $STATUS
