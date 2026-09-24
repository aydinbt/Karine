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
  Steps.OrderBy(step => System.Math.Abs(step - size)).ThenByDescending(step => step).First();

}
}
