using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Geliştirici konum ayarlayıcısı: adı verilen öğeler sürüklenebilir olur,
// yüzde konumu canlı yazılır; "Kopyala" `new Rect(...)` satırını panoya ve
// konsola verir — değer elle `KarineTheme`e yapıştırılır. Oyuncu bunu görmez.
public static partial class KarineUI {
 public static void Placer(VisualElement root,params string[] names) {
  if(root==null)return;
  var readout=Technical(root,"",KarineTheme.Office.SmallSize);
  readout.name="PlacerReadout";readout.pickingMode=PickingMode.Ignore;
  readout.style.position=Position.Absolute;readout.style.left=8;readout.style.top=8;
  readout.style.color=KarineTheme.Film.Phosphor;readout.style.backgroundColor=KarineTheme.GlassDeep;readout.style.whiteSpace=WhiteSpace.Normal;
  VisualElement picked=null;
  foreach(var name in names) {
   var element=root.Q(name);if(element==null)continue;
   element.pickingMode=PickingMode.Position;
   Border(element,1,KarineTheme.Accent);
   Vector2 grab=Vector2.zero;bool dragging=false;
   element.RegisterCallback<PointerDownEvent>(e=>{dragging=true;picked=element;grab=e.position;element.CapturePointer(e.pointerId);e.StopPropagation();Show(readout,element);});
   element.RegisterCallback<PointerMoveEvent>(e=> {
    if(!dragging || element.parent==null)return;
    var size=element.parent.layout.size;if(size.x<1 || size.y<1)return;
    Vector2 delta=(Vector2)e.position-grab;grab=e.position;
    element.style.left=Length.Percent(element.layout.x/size.x*100+delta.x/size.x*100);
    element.style.top=Length.Percent(element.layout.y/size.y*100+delta.y/size.y*100);
    Show(readout,element);
   });
   element.RegisterCallback<PointerUpEvent>(e=>{dragging=false;element.ReleasePointer(e.pointerId);});
  }
  var copy=new Button(Sounded(()=>{if(picked==null)return;string line=RectLine(picked);GUIUtility.systemCopyBuffer=line;Debug.Log(picked.name+": "+line);})) {name="PlacerCopy",text="Kopyala"};
  Paint(copy,KarineButtonKind.Secondary,true);copy.style.position=Position.Absolute;copy.style.right=8;copy.style.top=8;
  root.Add(readout);root.Add(copy);
 }
 static string RectLine(VisualElement e) {
  var size=e.parent.layout.size;var c=System.Globalization.CultureInfo.InvariantCulture;
  string P(float v,float of)=>(v/of*100).ToString("0.#",c)+"f";
  return "new Rect("+P(e.layout.x,size.x)+","+P(e.layout.y,size.y)+","+P(e.layout.width,size.x)+","+P(e.layout.height,size.y)+")";
 }
 static void Show(Label readout,VisualElement e)=>readout.text=e.name+"  "+RectLine(e);
}
}
