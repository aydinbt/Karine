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
 public static bool Enabled => PlayerPrefs.GetInt(Key, 0) == 1;

 PanelSettings panel;
 RenderTexture target;
 Material material;
 bool active;

 public void Bind(PanelSettings settings) { panel = settings; Apply(Enabled); }

 public void Apply(bool on) {
  if (panel == null) return;
  if (on && material == null) {
   var shader = Resources.Load<Shader>("Bube/Shaders/KarineCrt");
   if (shader == null || !shader.isSupported) { Debug.Log("CRT gölgelendiricisi yok ya da desteklenmiyor; kapalı kalıyor."); on = false; }
   else material = new Material(shader) { hideFlags = HideFlags.DontSave };
  }
  active = on;
  if (!on) {
   panel.targetTexture = null;
   panel.SetScreenToPanelSpaceFunction(null);
   if (target != null) { target.Release(); Destroy(target); target = null; }
   return;
  }
  Resize();
  panel.clearColor = true;
  panel.colorClearValue = Color.clear;
  panel.SetScreenToPanelSpaceFunction(Bend);
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
  uv *= 1f + curve * uv.sqrMagnitude;
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
