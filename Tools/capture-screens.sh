#!/usr/bin/env bash
# Karine — oyun içi ekran görüntülerini Docs/Screenshots/<tarih>/ altına alır.
#
#   Tools/capture-screens.sh                 # 2400x1080
#   Tools/capture-screens.sh 1920x1080       # başka boyut
#
# Şimdilik her vakanın sorgu ekranları (GalleryTests). Unity Hub açık olmalı.
set -euo pipefail
PROJECT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
size="${1:-2400x1080}"
export KARINE_W="${size%x*}" KARINE_H="${size#*x}"
export KARINE_GALLERY="$PROJECT/Docs/Screenshots/$(date +%F)"
export KARINE_TEST_FILTER=Bube.Tests.GalleryTests
"$PROJECT/Tools/run-tests.sh" PlayMode
echo "Görüntüler: $KARINE_GALLERY"
