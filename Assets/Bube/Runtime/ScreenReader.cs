using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UIElements;

namespace Bube {
// Ekran okuyucu köprüsü (TalkBack / VoiceOver). UI Toolkit kendi öğelerini
// işletim sisteminin erişilebilirlik ağacına vermiyor; bu sınıf ekranda görünen
// düğmeleri ve yazıları okuyarak o ağacı kurar. Ekran okuyucu kapalıyken hiçbir
// şey yapmaz.
//
// Okunan metin ekranda yazanın aynısıdır; gizli hiçbir bilgi eklenmez.
public static class ScreenReader {
 static AccessibilityHierarchy hierarchy;
 static int lastSignature;
 static float nextCheck;

 public static void Tick(VisualElement root) {
  if(root==null || root.panel==null || !AssistiveSupport.isScreenReaderEnabled)return;
  if(Time.unscaledTime<nextCheck)return;
  nextCheck=Time.unscaledTime+.5f;
  var items=new List<VisualElement>();
  Collect(root,items);
  int signature=items.Count;
  foreach(var item in items)signature=signature*31+(Text(item)?.GetHashCode()??0)+Mathf.RoundToInt(item.worldBound.y);
  if(signature==lastSignature && hierarchy!=null)return;
  lastSignature=signature;
  hierarchy=new AccessibilityHierarchy();
  float scale=Screen.height/Mathf.Max(1,root.panel.visualTree.layout.height);
  foreach(var item in items) {
   var node=hierarchy.AddNode(Text(item));
   node.role=item is Button?AccessibilityRole.Button:AccessibilityRole.StaticText;
   var box=item.worldBound;
   node.frame=new Rect(box.x*scale,box.y*scale,box.width*scale,box.height*scale);
   if(item is Button button)node.invoked+=()=>{
    using(var evt=NavigationSubmitEvent.GetPooled()){evt.target=button;button.SendEvent(evt);}
    return true;
   };
  }
  AssistiveSupport.activeHierarchy=hierarchy;
 }

 static string Text(VisualElement item) {
  if(item is Button button) {
   if(!string.IsNullOrEmpty(button.text))return button.text;
   var inner=button.Q<Label>();
   return inner!=null?inner.text:button.tooltip;
  }
  return (item as Label)?.text;
 }

 static void Collect(VisualElement element,List<VisualElement> into) {
  if(element.resolvedStyle.display==DisplayStyle.None || element.resolvedStyle.visibility==Visibility.Hidden || element.resolvedStyle.opacity<.05f)return;
  if(element is Button button) {
   if(button.enabledInHierarchy && !string.IsNullOrEmpty(Text(button)) && button.worldBound.width>1)into.Add(button);
   return;
  }
  if(element is Label label && !string.IsNullOrWhiteSpace(label.text) && label.worldBound.width>1){into.Add(label);return;}
  foreach(var child in element.Children())Collect(child,into);
 }
}
}
