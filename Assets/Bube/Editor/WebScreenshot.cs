using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bube.EditorTools
{
    // Site için oyunun anlık görüntüsü: Play Mode'da F12 (Karine ▸ Ekran görüntüsü al).
    // 2x çözünürlükte Docs/Web/gorseller/ekran/ altına yazar; klasör depoya girmez.
    static class WebScreenshot
    {
        [MenuItem("Karine/Ekran görüntüsü al _F12")]
        static void Capture()
        {
            var dir = Path.GetFullPath(Path.Combine(Application.dataPath, "../Docs/Web/gorseller/ekran"));
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, $"karine-{DateTime.Now:yyyyMMdd-HHmmss}.png");
            ScreenCapture.CaptureScreenshot(file, 2);
            Debug.Log($"Ekran görüntüsü: {file}");
        }

        [MenuItem("Karine/Ekran görüntüsü al _F12", true)]
        static bool CanCapture() => Application.isPlaying;
    }
}
