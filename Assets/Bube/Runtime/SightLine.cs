using System.Collections.Generic;
using UnityEngine;

namespace Bube {
// Olay yeri planının görüş hesabı: saf geometri, arayüzden bağımsız. Koordinatlar plan
// görselinin içinde 0–1 arasıdır; açı derece, 0 = sağ, saat yönünde artar (ekran y aşağı).
public static class SightLine {
 // a→b doğru parçası ile c→d kesişiyorsa a'dan uzaklık oranı (0–1), yoksa -1.
 public static float Hit(Vector2 a,Vector2 b,Vector2 c,Vector2 d) {
  Vector2 r=b-a,s=d-c;float den=r.x*s.y-r.y*s.x;
  if(Mathf.Abs(den)<1e-7f)return -1;
  Vector2 ac=c-a;float t=(ac.x*s.y-ac.y*s.x)/den,u=(ac.x*r.y-ac.y*r.x)/den;
  return t>=0 && t<=1 && u>=0 && u<=1 ? t : -1;
 }

 static Vector2 P(PlanPoint p) => new Vector2(p.x,p.y);

 // from→to arasında bir engel var mı?
 public static bool Blocked(Vector2 from,Vector2 to,PlanOccluder[] occluders) {
  foreach(var o in occluders ?? new PlanOccluder[0]) {
   var pts=o.points;if(pts==null || pts.Length<3)continue;
   for(int i=0;i<pts.Length;i++)if(Hit(from,to,P(pts[i]),P(pts[(i+1)%pts.Length]))>=0)return true;
  }
  return false;
 }

 public static bool Sees(PlanMarker m,Vector2 target,PlanOccluder[] occluders) {
  var from=new Vector2(m.x,m.y);var dir=target-from;
  if(dir.sqrMagnitude<1e-8f)return true;
  float angle=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
  if(m.fov>0 && m.fov<360 && Mathf.Abs(Mathf.DeltaAngle(m.facing,angle))>m.fov*.5f)return false;
  return !Blocked(from,target,occluders);
 }

 // Görüş konisi: işaretten çıkan ışınlar, her biri ilk engelde ya da plan kenarında durur.
 // Dönen liste ilk eleman merkez olmak üzere bir yelpaze çokgenidir.
 public static List<Vector2> Cone(PlanMarker m,PlanOccluder[] occluders,int rays=64) {
  var from=new Vector2(m.x,m.y);var fan=new List<Vector2>{from};
  float fov=m.fov<=0 || m.fov>360 ? 360 : m.fov;
  for(int i=0;i<=rays;i++) {
   float a=(m.facing-fov*.5f+fov*i/rays)*Mathf.Deg2Rad;
   var to=from+new Vector2(Mathf.Cos(a),Mathf.Sin(a))*1.5f;
   float best=Edge(from,to);
   foreach(var o in occluders ?? new PlanOccluder[0]) {
    var pts=o.points;if(pts==null || pts.Length<3)continue;
    for(int k=0;k<pts.Length;k++){float t=Hit(from,to,P(pts[k]),P(pts[(k+1)%pts.Length]));if(t>=0 && t<best)best=t;}
   }
   fan.Add(Vector2.Lerp(from,to,best));
  }
  return fan;
 }

 // Işının plan karesinden çıktığı oran.
 static float Edge(Vector2 a,Vector2 b) {
  float best=1;var c=new[]{Vector2.zero,Vector2.right,Vector2.one,Vector2.up};
  for(int i=0;i<4;i++){float t=Hit(a,b,c[i],c[(i+1)%4]);if(t>=0 && t<best)best=t;}
  return best;
 }
}
}
