// Karine'nin CRT görüntüsü: arayüz dokusunu hafif bükerek, renkleri ayırarak,
// tarama çizgisi ve köşe kararmasıyla ekrana basar. `CrtPass` kullanır.
// Bükme (_Curve) CrtPass.Bend ile aynı formüldür; ikisi birlikte değişir.
Shader "Hidden/Karine/Crt" {
 Properties {
  _MainTex ("Texture", 2D) = "white" {}
  _Curve ("Curve", Float) = 0.035
  _Chroma ("Chroma", Float) = 0.0012
  _Scan ("Scan", Float) = 0.06
  _Vignette ("Vignette", Float) = 0.35
  _Time2 ("Time", Float) = 0
 }
 SubShader {
  Tags { "Queue"="Overlay" "RenderType"="Transparent" }
  Cull Off ZWrite Off ZTest Always
  Blend One OneMinusSrcAlpha
  Pass {
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "UnityCG.cginc"
   sampler2D _MainTex; float4 _MainTex_TexelSize;
   float _Curve, _Chroma, _Scan, _Vignette, _Time2;
   struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };
   v2f vert (appdata_img v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = v.texcoord; return o; }
   fixed4 frag (v2f i) : SV_Target {
    float2 c = i.uv * 2 - 1;
    c *= 1 + _Curve * dot(c, c);
    float2 uv = (c + 1) * 0.5;
    if (uv.x < 0 || uv.y < 0 || uv.x > 1 || uv.y > 1) return fixed4(0, 0, 0, 1);
    float2 shift = float2(_Chroma, 0);
    fixed4 mid = tex2D(_MainTex, uv);
    float r = tex2D(_MainTex, uv + shift).r;
    float b = tex2D(_MainTex, uv - shift).b;
    fixed4 col = fixed4(r, mid.g, b, mid.a);
    float line = 0.5 + 0.5 * sin(uv.y * _MainTex_TexelSize.w * 3.14159);
    col.rgb *= 1 - _Scan * line;
    float v = saturate(1 - _Vignette * dot(c * 0.7, c * 0.7));
    col.rgb *= v;
    // Arayüz dokusu önceden çarpılmış saydamlıkla gelir; boş yer kamerayı gösterir.
    return col;
   }
   ENDCG
  }
 }
}
