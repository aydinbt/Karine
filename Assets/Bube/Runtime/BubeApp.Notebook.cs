using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UIElements;

namespace Bube {
// Oyuncunun defteri: karşılaştırmadan işlenen kaynak çiftleri ve belgelerde altı çizilen cümleler.
// `BubeApp` tek bir MonoBehaviour'dur; bu dosya onun bir parçasıdır.
public sealed partial class BubeApp {
 // Belgenin cümleleri ayrı ayrı dokunulur: dokunulan cümlenin altı çizilir, deftere düşer.
 void MarkableBody(VisualElement body,Node node,Color ink,Color muted) {
  var sentences=Investigation.Sentences(T(node.bodyKey));
  if(!game.State.closed && game.State.read.Contains(node.id))Text(body,T("notebook.markHelp"),muted,13).style.marginBottom=6;
  for(int i=0;i<sentences.Length;i++) {
   int index=i;
   bool marked=game.State.highlights.Contains(Investigation.HighlightId(node.id,i));
   var line=Text(body,sentences[i],ink,17);
   line.style.marginBottom=4;line.style.paddingLeft=4;line.style.paddingRight=4;
   if(marked) {
    line.style.backgroundColor=KarineTheme.Paper.Tint;
    line.style.borderBottomWidth=2;line.style.borderBottomColor=KarineTheme.Paper.Stamp;
   }
   line.RegisterCallback<ClickEvent>(_=>{
    if(!game.ToggleHighlight(node.id,index))return;
    Save();
    bool now=game.State.highlights.Contains(Investigation.HighlightId(node.id,index));
    line.style.backgroundColor=now?KarineTheme.Paper.Tint:new StyleColor(Color.clear);
    line.style.borderBottomColor=KarineTheme.Paper.Stamp;
    // Altı kalemle çizilir; kalem geçince çizgi kalıcı olur.
    if(now)KarineUI.InkStroke(line,()=>line.style.borderBottomWidth=2);else line.style.borderBottomWidth=0;
   });
  }
 }
 string NotebookSourceTitle(string id) {
  var node=game.Data.nodes.FirstOrDefault(n=>n.id==id);
  return node==null?T("conclude.sourceUnknown"):node.kind=="interview"?T(node.personNameKey):T(node.titleKey);
 }
}
}
