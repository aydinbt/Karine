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
 // Karşılaştırmanın altındaki defter şeridi: oyuncu iki kaynağı kendi hükmüyle deftere işler.
 // Oyun hükmü onaylamaz, düzeltmez; seçilen hüküm yalnız oyuncunun kendi notudur.
 void NotebookBar(VisualElement folder,Color ink) {
  if(game.State.closed || string.IsNullOrEmpty(compareLeftId) || string.IsNullOrEmpty(compareRightId) || compareLeftId==compareRightId)return;
  var bar=new VisualElement();bar.style.flexDirection=FlexDirection.Row;bar.style.alignItems=Align.Center;
  bar.style.paddingLeft=20;bar.style.paddingRight=20;bar.style.paddingBottom=12;bar.style.flexShrink=0;folder.Add(bar);
  var label=Text(bar,T("notebook.write"),Ink,16);label.style.marginBottom=0;label.style.marginRight=12;label.style.flexGrow=1;
  var existing=game.FindNote(compareLeftId,compareRightId);
  foreach(var mark in Investigation.NotebookMarks) {
   var chosen=mark;
   var button=KarineUI.PaperButton(bar,Shape(mark)+T("notebook.mark."+mark),()=>{
    // İki kaynak ataçla tutturulur: her hükümde aynı ses.
    if(game.MarkNote(compareLeftId,compareRightId,chosen)){KarineUI.Cue("clip");Save();KarineUI.RedString(folder,ComparePage);}
   },existing!=null && existing.mark==mark?KarinePaperKind.Action:KarinePaperKind.Choice);
   button.style.marginLeft=6;button.style.minHeight=KarineTheme.TouchTarget;
   button.style.paddingLeft=14;button.style.paddingRight=14;button.style.fontSize=Typography.Snap(15);
  }
 }
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
