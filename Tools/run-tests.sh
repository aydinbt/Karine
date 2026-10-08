#!/usr/bin/env bash
# Karine — testleri komut satırından koşar (EditMode ve/veya PlayMode).
#
#   Tools/run-tests.sh                # ikisi de
#   Tools/run-tests.sh EditMode       # yalnız biri
#   KARINE_TEST_FILTER=Bube.Tests.X   # yalnız eşleşen testler
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
# Ekran görüntüsü testlerinin referans klasörü: kopyada değil, asıl projede.
export KARINE_BASELINES="$PROJECT/Tools/Baselines"
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
# Hub kapalıyken grep boş döner; `pipefail` betiği uyarı yazmadan kapatmasın.
channels="$(ps -Ao args= | tr ' ' '\n' | grep '^Unity-LicenseClient-' | sed 's/^Unity-//' | sort -u || true)"
CHANNEL="$(printf '%s\n' "$channels" | grep -v -- '-[0-9]' | grep -vx "LicenseClient-$(id -un)" | head -1 || true)"
[ -z "$CHANNEL" ] && CHANNEL="$(printf '%s\n' "$channels" | head -1 || true)"
if [ -z "$CHANNEL" ]; then
  echo "Lisans kanalı bulunamadı. Unity Hub'ı açıp tekrar deneyin." >&2
  exit 1
fi
echo "Lisans kanalı: $CHANNEL"

mkdir -p "$WORK"
# Testler oyunun **gerçek** ayar kaydına yazar: kopya proje aynı şirket/ürün adını
# taşıdığı için Unity aynı PlayerPrefs alanını kullanır. Ses testi müziği kapatıyor,
# efekt testleri efektleri değiştiriyordu; oyuncu Editor'de ayarını "kendiliğinden
# sıfırlanmış" buluyordu (8 Ekim 2026). Koşudan önce alan saklanır, sonra geri yazılır.
PREFS_DOMAIN="unity.$(sed -n 's/^  companyName: //p' "$PROJECT/ProjectSettings/ProjectSettings.asset").$(sed -n 's/^  productName: //p' "$PROJECT/ProjectSettings/ProjectSettings.asset")"
PREFS_BACKUP="$WORK/prefs-backup.plist"; rm -f "$PREFS_BACKUP"
if [ "$(uname)" = Darwin ] && defaults export "$PREFS_DOMAIN" "$PREFS_BACKUP" 2>/dev/null; then
  trap 'defaults delete "$PREFS_DOMAIN" >/dev/null 2>&1 || true; defaults import "$PREFS_DOMAIN" "$PREFS_BACKUP"' EXIT
fi
rsync -a --delete \
  --exclude Library --exclude Temp --exclude Logs --exclude UserSettings \
  --exclude obj --exclude .git \
  "$PROJECT/" "$WORK/proj/"

STATUS=0
for platform in "${PLATFORMS[@]}"; do
  echo "== $platform =="
  results="$WORK/$platform.xml"; log="$WORK/$platform.log"
  rm -f "$results"
  # PlayMode grafik bağlamıyla koşar: ekran görüntüsü testleri arayüzü bir dokuya çizer.
  graphics=(-nographics); [ "$platform" = PlayMode ] && graphics=()
  "$UNITY" -batchmode ${graphics[@]+"${graphics[@]}"} -projectPath "$WORK/proj" \
    -runTests -testPlatform "$platform" ${KARINE_TEST_FILTER:+-testFilter "$KARINE_TEST_FILTER"} -testResults "$results" -logFile "$log" \
    -acceptSoftwareTermsForThisRunOnly -licensingIpc "$CHANNEL" >/dev/null 2>&1 || true
  if [ ! -f "$results" ]; then
    echo "  sonuç dosyası yazılmadı — derleme hatası olabilir:"
    grep -m 10 "error CS" "$log" || tail -5 "$log"
    STATUS=1; continue
  fi
  python3 - "$results" <<'PY' || STATUS=1
import sys, xml.etree.ElementTree as ET
root = ET.parse(sys.argv[1]).getroot()
print("  toplam=%s geçti=%s düştü=%s atlandı=%s" % (root.get('total'), root.get('passed'), root.get('failed'), root.get('skipped')))
failed = [tc for tc in root.iter('test-case') if tc.get('result') not in ('Passed', 'Skipped')]
for tc in failed:
    message = tc.find('.//message')
    print("  DÜŞTÜ", tc.get('name'), (message.text or '').strip()[:300] if message is not None else '')
sys.exit(1 if failed else 0)
PY
done
exit $STATUS
