using System;
using UnityEngine;
using UnityEngine.UIElements;
using R = Bube.KarineTheme.Report;
namespace Bube {
// Olay rekonstrüksiyonu (#010): rapor kâğıdının üstünde dizilmiş kart satırları.
// Satır sıra numarası, kart adı ve bağlanan kaydı gösterir; yanında yukarı/aşağı/çıkar.
// Doğru ya da yanlış işaret taşımaz.
public static partial class KarineUI {
 public static VisualElement ReconRow(VisualElement parent,string number,string label,string source,bool focused,
  Action focus,Action up,Action down,Action remove) {
  var row=new VisualElement {name="ReconRow"};row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
  row.style.minHeight=KarineTheme.TouchTarget+KarineTheme.SpaceSm;row.style.marginBottom=KarineTheme.SpaceXs;
  row.style.paddingLeft=KarineTheme.SpaceSm;row.style.paddingRight=KarineTheme.SpaceXs;Round(row,KarineTheme.Radius);
  row.style.backgroundColor=focused?KarineTheme.Alpha(KarineTheme.Accent,.14f):KarineTheme.Alpha(KarineTheme.Paper.Edge,.12f);
  parent.Add(row);
  var n=Typed(row,number,R.RowSize,true);n.style.width=R.RowSize*2;n.style.flexShrink=0;n.style.marginBottom=0;
  var body=new Button(Sounded(focus)) {name="ReconFocus",tooltip=label};Unskin(body,Color.clear);
  body.style.flexGrow=1;body.style.flexShrink=1;body.style.flexBasis=0;body.style.alignItems=Align.FlexStart;
  body.style.marginLeft=0;body.style.paddingLeft=0;row.Add(body);
  var l=Typed(body,label.ToUpper(TextCulture),R.RowSize,true);l.style.marginBottom=0;l.style.whiteSpace=WhiteSpace.Normal;l.pickingMode=PickingMode.Ignore;
  var s=Typed(body,source,R.RowSize-2,false,KarineTheme.Paper.Faded);s.style.marginBottom=0;s.style.whiteSpace=WhiteSpace.Normal;s.pickingMode=PickingMode.Ignore;
  foreach(var (icon,action) in new (string,Action)[]{("nav_prev",up),("nav_next",down),("close",remove)}) {
   var b=new Button(Sounded(action)) {name="ReconTool"};Unskin(b,Color.clear);
   b.style.width=KarineTheme.TouchTarget;b.style.height=KarineTheme.TouchTarget;b.style.alignItems=Align.Center;b.style.justifyContent=Justify.Center;
   b.style.marginLeft=0;b.style.marginRight=0;b.SetEnabled(action!=null);
   var i=Icon(b,icon,KarineTheme.Paper.Ink,KarineTheme.IconSize);
   if(icon!="close")i.style.rotate=new Rotate(90);
   row.Add(b);
  }
  return row;
 }
}
}
