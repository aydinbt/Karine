namespace Bube.Editor {

// Vakaya özel kuralların genel doğrulayıcıdan aldığı her şey. Vaka kancaları iki
// evrede koşar: `Early` kabul sonrası ama tüketici gezintiden önce (tempo/kilit
// sırası buna bağlı), `AfterWalk` gezinti bittikten sonra (oynanmış `Game` ve
// kayıt anlık görüntüsü gerektirir).
public sealed class CaseContext {
 public CaseData Data;
 public Locale Locale;
 public Investigation Game;
 public string ReadySnapshot;
 public ValidationReport Report;
}
}
