using System.Linq;

namespace Bube {

// Tek yazı ölçeği. Yazı tipi zaten her yerde IBM Plex Mono'dur (kök öğeden miras
// alınır); tutarsızlık puntolardaydı — 12'den 76'ya yirmi iki ayrı değer, çoğu
// birbirinden bir punto farkla, ekrandan ekrana rastgele seçilmişti.
//
// Çözüm çağrı yerlerini elle düzeltmek değil, ölçeği **tek kapıdan geçirmektir**:
// `Text(...)` ve `Button(...)` yardımcıları boyutu `Typography.Snap` ile en yakın
// basamağa oturtur. Böylece yarın yazılan `Text(parent,"...",Ink,18)` de
// kendiliğinden ölçeğe uyar; ölçek dışı bir punto ekrana hiç ulaşamaz.
public static class Typography {

 // Küçükten büyüğe: ipucu, ikincil, gövde, öne çıkan gövde, alt başlık,
 // başlık, ekran başlığı, jenerik, stüdyo logosu.
 public static readonly int[] Steps = { 13, 15, 17, 19, 21, 24, 28, 40, 76 };

 // Eşit uzaklıkta iki basamak varsa **büyüğü** seçilir: telefonda okunaklılık,
 // sıkışıklıktan daha değerlidir.
 public static int Snap(int size) =>
  (int)System.Math.Round(Steps.OrderBy(step => System.Math.Abs(step - size)).ThenByDescending(step => step).First() * Scale);

 // Erişilebilirlik: oyuncunun seçtiği yazı büyüklüğü, kaydırıcıyla 0.9–1.5 arası,
 // 0.05 adımla. Basamak önce seçilir, sonra büyütülür; ölçek dışı punto yine oluşmaz.
 public const string ScaleKey = "karine.textScale";
 public const float MinScale = .9f, MaxScale = 1.5f;
 public static float Scale { get; private set; } = 1f;
 public static void Load() => Scale = Pick(UnityEngine.PlayerPrefs.GetFloat(ScaleKey, 1f));
 public static void Set(float scale) { Scale = Pick(scale); UnityEngine.PlayerPrefs.SetFloat(ScaleKey, Scale); }
 static float Pick(float value) => (float)System.Math.Round(System.Math.Min(MaxScale, System.Math.Max(MinScale, value)) * 20) / 20f;

}
}
