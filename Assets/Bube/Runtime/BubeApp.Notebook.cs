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
   var button=KarineUI.PaperButton(bar,T("notebook.mark."+mark),()=>{
    // İki kaynak ataçla tutturulur: her hükümde aynı ses.
    if(game.MarkNote(compareLeftId,compareRightId,chosen)){audio?.Play("ui_clip");Fx.Buzz(Haptic.Tick);Save();ComparePage();}
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
 void NotebookContents(VisualElement paper,Color ink,Color muted) {
  var title=Text(paper,T("notebook.title"),ink,21);title.style.marginBottom=4;
  if(dossierBoldFont!=null)title.style.unityFontDefinition=FontDefinition.FromFont(dossierBoldFont);
  Text(paper,T("notebook.help"),muted,14).style.marginBottom=10;
  var scroll=Scroll(paper);
  Text(scroll,T("notebook.pairs"),ink,17).style.marginBottom=5;
  if(game.State.notebook.Count==0)Text(scroll,T("notebook.noPairs"),muted,15);
  foreach(var entry in game.State.notebook.ToArray()) {
   var note=entry;
   var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.alignItems=Align.Center;
   row.style.minHeight=60;row.style.marginBottom=6;row.style.paddingLeft=9;row.style.paddingRight=8;
   row.style.backgroundColor=KarineTheme.Paper.Tint;row.style.borderLeftWidth=3;
   row.style.borderLeftColor=note.mark=="conflict"?KarineTheme.Paper.Stamp:KarineTheme.Paper.Edge;scroll.Add(row);
   var mark=KarineUI.Technical(row,T("notebook.mark."+note.mark),14);mark.style.color=ink;mark.style.width=110;mark.style.marginBottom=0;
   var pair=Text(row,NotebookSourceTitle(note.leftId)+"  ↔  "+NotebookSourceTitle(note.rightId),ink,15);
   pair.style.flexGrow=1;pair.style.flexShrink=1;pair.style.marginBottom=0;
   var open=KarineUI.PaperButton(row,T("notebook.open"),()=>{compareLeftId=note.leftId;compareRightId=note.rightId;comparePicker=-1;ComparePage();},KarinePaperKind.Quiet);
   open.style.minHeight=40;open.style.marginLeft=6;open.style.fontSize=Typography.Snap(13);
   if(!game.State.closed) {
    var remove=KarineUI.PaperButton(row,T("timeline.remove"),()=>{if(game.RemoveNote(note)){Save();FilePage();}},KarinePaperKind.Action);
    remove.style.minHeight=40;remove.style.marginLeft=6;remove.style.fontSize=Typography.Snap(13);
   }
  }
  var divider=new VisualElement();divider.style.height=1;divider.style.marginTop=14;
  divider.style.marginBottom=12;divider.style.backgroundColor=KarineTheme.Paper.Edge;scroll.Add(divider);
  Text(scroll,T("notebook.lines"),ink,17).style.marginBottom=5;
  bool any=false;
  foreach(var node in game.Data.nodes) {
   var sentences=Investigation.Sentences(T(node.bodyKey));
   var picked=Enumerable.Range(0,sentences.Length).Where(i=>game.State.highlights.Contains(Investigation.HighlightId(node.id,i))).ToArray();
   if(picked.Length==0)continue;
   any=true;
   Text(scroll,T(node.titleKey),muted,13).style.marginBottom=2;
   foreach(var i in picked) {
    var line=Text(scroll,"“"+sentences[i]+"”",ink,15);
    line.style.marginBottom=6;line.style.paddingLeft=9;line.style.borderLeftWidth=2;line.style.borderLeftColor=KarineTheme.Paper.Stamp;
   }
  }
  if(!any)Text(scroll,T("notebook.noLines"),muted,15);
 }
}
}
