using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
public static partial class KarineUI {
 // A single reusable atlas; UVs select a postcard without creating ten textures.
 public static Image CountryPostcard(VisualElement parent,string id,Texture2D custom=null) {
  string[] ids={"tr","uk","de","jp","fr","us","it","es","ca","au"};
  int index=Array.IndexOf(ids,id);
  var art=custom??Resources.Load<Texture2D>("Bube/Art/CountryPostcards");
  var image=new Image {image=art,scaleMode=custom!=null?ScaleMode.ScaleAndCrop:ScaleMode.StretchToFill,pickingMode=PickingMode.Ignore};
  if(custom==null && index>=0) image.uv=new Rect((index%5)/5f,index<5?.5f:0f,.2f,.5f);
  image.style.position=Position.Absolute;
  image.style.left=0;image.style.right=0;image.style.top=0;image.style.bottom=0;
  parent.Add(image);
  return image;
 }

 public static void BrowserMeter(VisualElement parent,float value) {
  var track=new VisualElement();track.style.height=KarineTheme.CaseBrowser.ProgressHeight;track.style.marginBottom=KarineTheme.SpaceXs;
  track.style.backgroundColor=KarineTheme.Panel2;parent.Add(track);
  var fill=new VisualElement();fill.style.width=Length.Percent(Mathf.Clamp01(value)*100);fill.style.height=Length.Percent(100);fill.style.backgroundColor=KarineTheme.Primary;track.Add(fill);
 }
}
}
