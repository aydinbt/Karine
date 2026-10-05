using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Olay yeri planı: kuşbakışı kroki, engeller, olay noktası ve kilidi açılmış tanık işaretleri.
// Bir işarete dokununca o noktadan görüş konisi çizilir; engeller koniyi keser.
//
// Kural: koni her işarette aynı renkle çizilir. Oyun olay noktasının görülüp görülmediğini,
// işaretin beyan mı kayıt mı olduğunun doğruluğunu söylemez; altta yalnız işaretin etiketi yazar.
public static partial class KarineUI {
 public static VisualElement ScenePlanView(VisualElement parent,ScenePlan plan,Texture2D image,
  IList<PlanMarker> markers,Func<string,string> label,string incidentLabel) {
  var frame=new VisualElement{name="ScenePlan"};
  frame.style.marginTop=KarineTheme.SpaceLg;frame.style.width=Length.Percent(100);
  frame.style.borderTopWidth=frame.style.borderBottomWidth=frame.style.borderLeftWidth=frame.style.borderRightWidth=1;
  frame.style.borderTopColor=frame.style.borderBottomColor=frame.style.borderLeftColor=frame.style.borderRightColor=KarineTheme.Paper.Edge;
  frame.style.backgroundColor=KarineTheme.Paper.Light;
  if(image!=null)frame.style.backgroundImage=new StyleBackground(image);
  // Kare tutulur: koordinatlar görselin içinde 0–1'dir.
  frame.RegisterCallback<GeometryChangedEvent>(e=>{float w=frame.resolvedStyle.width;if(w>0 && Mathf.Abs(frame.resolvedStyle.height-w)>.5f)frame.style.height=w;});
  parent.Add(frame);
  var caption=new Label(incidentLabel){name="ScenePlanCaption"};
  caption.style.color=KarineTheme.Paper.Faded;caption.style.marginTop=KarineTheme.SpaceSm;caption.style.whiteSpace=WhiteSpace.Normal;
  parent.Add(caption);

  PlanMarker selected=null;
  var layer=new VisualElement{name="ScenePlanLayer",pickingMode=PickingMode.Ignore};
  layer.style.position=Position.Absolute;layer.style.left=0;layer.style.right=0;layer.style.top=0;layer.style.bottom=0;
  layer.generateVisualContent+=ctx=>{
   var p=ctx.painter2D;var size=layer.contentRect.size;
   Vector2 At(float x,float y)=>new Vector2(x*size.x,y*size.y);
   if(image==null)foreach(var o in plan.occluders ?? new PlanOccluder[0]) {
    if(o.points==null || o.points.Length<3)continue;
    p.fillColor=KarineTheme.Alpha(KarineTheme.Paper.Edge,.55f);p.strokeColor=KarineTheme.Paper.Faded;p.lineWidth=1.5f;
    p.BeginPath();p.MoveTo(At(o.points[0].x,o.points[0].y));
    for(int i=1;i<o.points.Length;i++)p.LineTo(At(o.points[i].x,o.points[i].y));
    p.ClosePath();p.Fill();p.Stroke();
   }
   if(selected!=null) {
    var fan=SightLine.Cone(selected,plan.occluders);
    p.fillColor=KarineTheme.Alpha(KarineTheme.Accent,.28f);
    p.BeginPath();p.MoveTo(Vector2.Scale(fan[0],size));
    for(int i=1;i<fan.Count;i++)p.LineTo(Vector2.Scale(fan[i],size));
    p.ClosePath();p.Fill();
   }
   if(plan.incident!=null) {
    var c=At(plan.incident.x,plan.incident.y);float r=KarineTheme.SpaceSm;
    p.strokeColor=KarineTheme.Paper.Stamp;p.lineWidth=3;
    p.BeginPath();p.MoveTo(c+new Vector2(-r,-r));p.LineTo(c+new Vector2(r,r));p.MoveTo(c+new Vector2(r,-r));p.LineTo(c+new Vector2(-r,r));p.Stroke();
   }
  };
  frame.Add(layer);

  foreach(var marker in markers) {
   var m=marker;
   var dot=new Button(Sounded(()=>{selected=selected==m?null:m;caption.text=selected==null?incidentLabel:label(m.labelKey);layer.MarkDirtyRepaint();})){name="PlanMarker_"+m.id,text=""};
   float d=KarineTheme.SpaceLg;
   dot.style.position=Position.Absolute;dot.style.width=dot.style.height=d;
   dot.style.left=Length.Percent(m.x*100);dot.style.top=Length.Percent(m.y*100);
   dot.style.translate=new Translate(Length.Percent(-50),Length.Percent(-50));
   dot.style.borderTopLeftRadius=dot.style.borderTopRightRadius=dot.style.borderBottomLeftRadius=dot.style.borderBottomRightRadius=d/2;
   dot.style.backgroundColor=m.kind=="recorded"?KarineTheme.Paper.Ink:KarineTheme.Paper.Sheet;
   dot.style.borderTopWidth=dot.style.borderBottomWidth=dot.style.borderLeftWidth=dot.style.borderRightWidth=2;
   dot.style.borderTopColor=dot.style.borderBottomColor=dot.style.borderLeftColor=dot.style.borderRightColor=KarineTheme.Paper.Ink;
   frame.Add(dot);
  }
  return frame;
 }
}
}
