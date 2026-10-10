using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Gerçek CRT görüntüsü (deneysel, varsayılan kapalı). UI Toolkit paneli
// ekrana değil bir doku hedefine çizilir; bu bileşen o dokuyu `KarineCrt`
// gölgelendiricisinden geçirerek ekrana basar: hafif bombe, renk kayması,
// tarama çizgisi, köşe kararması.
//
// Bombe dokunuşları da büker; aynı bükme girişe uygulanır, yani parmak
// gördüğü düğmeye basar. Doku kameranın üstüne saydamlıkla basılır.
public sealed class CrtPass : MonoBehaviour {
 public const string Key = "karine.crt";
 public static bool Enabled => PlayerPrefs.GetInt(Key, 1) == 1;
 // Renk körlüğü düzeltmesi: 0 kapalı, 1 protan, 2 deutan, 3 tritan. Aynı geçişten gider;
 // CRT kapalıyken bükme, tarama ve kayma sıfırlanır, yalnız renk matrisi kalır.
 public const string ColorKey = "karine.colorFilter";
 public static int ColorFilter => Mathf.Clamp(PlayerPrefs.GetInt(ColorKey, 0), 0, 3);
 bool crtOn;

 PanelSettings panel;
 RenderTexture target;
 Material material;
 bool active;

 public void Bind(PanelSettings settings) { panel = settings; Apply(Enabled); }

 public void Apply(bool on) {
  if (panel == null) return;
  crtOn = on;
  on = on || ColorFilter != 0;
  if (on && material == null) {
   var shader = Resources.Load<Shader>("Bube/Shaders/KarineCrt");
   if (shader == null || !shader.isSupported) { Debug.Log("CRT gölgelendiricisi yok ya da desteklenmiyor; kapalı kalıyor."); on = false; }
   else material = new Material(shader) { hideFlags = HideFlags.DontSave };
  }
  active = on;
  if (!on) {
   panel.targetTexture = null;
   // null verilirse Unity kendi sarmalayıcısında null temsilciyi çağırır (NRE); kapalıyken birim dönüşüm.
   panel.SetScreenToPanelSpaceFunction(p => p);
   if (target != null) { target.Release(); Destroy(target); target = null; }
   return;
  }
  Configure();
  Resize();
  panel.clearColor = true;
  panel.colorClearValue = Color.clear;
  panel.SetScreenToPanelSpaceFunction(Bend);
 }

 void Configure() {
  material.SetFloat("_Curve", crtOn ? .035f : 0f);
  material.SetFloat("_Chroma", crtOn ? .0012f : 0f);
  material.SetFloat("_Scan", crtOn ? .06f : 0f);
  material.SetFloat("_Vignette", crtOn ? .35f : 0f);
  material.SetFloat("_Grain", crtOn ? .035f * Fx.Amount : 0f);
  var m = Correction(ColorFilter);
  material.SetVector("_CR", new Vector4(m[0, 0], m[0, 1], m[0, 2], 0));
  material.SetVector("_CG", new Vector4(m[1, 0], m[1, 1], m[1, 2], 0));
  material.SetVector("_CB", new Vector4(m[2, 0], m[2, 1], m[2, 2], 0));
 }

 // Daltonlaştırma: görülemeyen fark görülen kanallara dağıtılır. Benzetim matrisleri
 // Machado, Oliveira ve Fernandes (2009), tam şiddet. Sonuç = I + E·(I − Benzetim).
 static readonly float[][,] simulate = {
  null,
  new float[,] { { .152286f, 1.052583f, -.204868f }, { .114503f, .786281f, .099216f }, { -.003882f, -.048116f, 1.051998f } },
  new float[,] { { .367322f, .860646f, -.227968f }, { .280085f, .672501f, .047413f }, { -.011820f, .042940f, .968881f } },
  new float[,] { { 1.255528f, -.076749f, -.178779f }, { -.078411f, .930809f, .147602f }, { .004733f, .691367f, .303900f } },
 };
 public static float[,] Correction(int kind) {
  var result = new float[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
  if (kind <= 0 || kind >= simulate.Length) return result;
  var sim = simulate[kind];
  var spread = kind == 3 ? new float[,] { { 1, 0, .7f }, { 0, 1, .7f }, { 0, 0, 0 } } : new float[,] { { 0, 0, 0 }, { .7f, 1, 0 }, { .7f, 0, 1 } };
  for (int r = 0; r < 3; r++) for (int c = 0; c < 3; c++) {
   float sum = 0;
   for (int k = 0; k < 3; k++) sum += spread[r, k] * ((k == c ? 1 : 0) - sim[k, c]);
   result[r, c] += sum;
  }
  return result;
 }

 void Resize() {
  if (target != null && target.width == Screen.width && target.height == Screen.height) return;
  if (target != null) { panel.targetTexture = null; target.Release(); Destroy(target); }
  target = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32) { name = "KarineCrtTarget" };
  target.Create();
  panel.targetTexture = target;
 }

 // Ekran noktası → panel noktası: gölgelendiricideki bükmenin aynısı.
 Vector2 Bend(Vector2 screen) {
  var size = new Vector2(Screen.width, Screen.height);
  var uv = new Vector2(screen.x / size.x, screen.y / size.y) * 2f - Vector2.one;
  float curve = material != null ? material.GetFloat("_Curve") : 0f;
  uv *= (1f + curve * uv.sqrMagnitude) / (1f + 2f * curve);
  uv = (uv + Vector2.one) * .5f;
  if (uv.x < 0 || uv.y < 0 || uv.x > 1 || uv.y > 1) return new Vector2(float.NaN, float.NaN);
  return new Vector2(uv.x * size.x, uv.y * size.y);
 }

 void Update() { if (active) Resize(); }

 void OnGUI() {
  if (!active || target == null || material == null || Event.current.type != EventType.Repaint) return;
  material.SetFloat("_Time2", Time.unscaledTime);
  Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), target, material);
 }

 void OnDestroy() { if (target != null) target.Release(); }
}
}
