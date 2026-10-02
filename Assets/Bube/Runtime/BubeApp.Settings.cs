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
// Ayarlar ekranı. Bu dosya `BubeApp`in bir parçasıdır.
public sealed partial class BubeApp {
 int settingsTab;
 bool draftInstant,draftReduced,draftHaptics,draftShapes,draftCrt,draftAsh,draftMeter;float draftScaleValue=1f;int draftFps;FxLevel draftFx;
 SoundLevel draftMusic,draftSfx;
 void SettingsPage() {
  settingsTab=0;draftReduced=KarineMotion.Reduced;draftInstant=instantText;draftMusic=SoundSettings.Music;draftSfx=SoundSettings.Sfx;draftFps=FrameRate.Current;draftFx=Fx.Level;draftHaptics=Fx.Haptics;LoadSceneDraft();
  RenderSettings();
 }
 void RenderSettings() {
  Home();Back(Home);
  VisualElement navigation,body;KarineUI.SettingsShell(root,out navigation,out body);
  var tabs=new[]{"general","music","play"};
  var icons=new[]{KarineUI.IconOr("chat","document"),KarineUI.IconOr("music","chart"),KarineUI.IconOr("gamepad","gear")};
  for(int i=0;i<tabs.Length;i++) {
   int index=i;
   var tab=KarineUI.SettingsChoice(navigation,T("settings.tab."+tabs[i]),T("settings.tab."+tabs[i]+".hint"),settingsTab==i,()=>{settingsTab=index;RenderSettings();},icons[i]);
   tab.style.flexGrow=0;tab.style.flexBasis=StyleKeyword.Auto;tab.style.minHeight=KarineTheme.Settings.TabHeight;tab.style.marginBottom=KarineTheme.SpaceMd;
  }
  var header=KarineUI.Row(body);header.style.flexShrink=0;KarineUI.Icon(header,"gear",KarineTheme.Primary,KarineTheme.TouchTarget);
  var headings=new VisualElement();headings.style.flexGrow=1;headings.style.marginLeft=KarineTheme.SpaceLg;header.Add(headings);
  KarineUI.Title(headings,T("menu.settings"),KarineTheme.Settings.HeadingSize).style.marginBottom=0;
  var sub=KarineUI.Body_(headings,T("settings.subtitle"),KarineTheme.CaseBrowser.TextSize);sub.style.color=KarineTheme.Secondary;
  KarineUI.IconButton(header,"close",Home,T("offer.back"));KarineUI.Rule(body);
  var scroll=new KarineScrollView();scroll.style.flexGrow=1;scroll.style.minHeight=0;body.Add(scroll);
  string chat=KarineUI.IconOr("chat","document"),music=KarineUI.IconOr("music","chart");
  if(settingsTab==0) {
   KarineUI.SettingsSection(scroll,chat,T("settings.textSpeed"),T("settings.text.hint"),false);
   var choices=KarineUI.Row(scroll);choices.style.alignItems=Align.Stretch;
   KarineUI.SettingsOption(choices,T("settings.instant"),T("settings.instant.hint"),draftInstant,()=>{draftInstant=true;RenderSettings();},chat).style.marginRight=KarineTheme.SpaceMd;
   KarineUI.SettingsOption(choices,T("settings.normal"),T("settings.normal.hint"),!draftInstant,()=>{draftInstant=false;RenderSettings();},chat);
   KarineUI.SettingsSection(scroll,music,T("settings.music"),T("settings.music.hint"),true);
   SoundRow(scroll,draftMusic,level=>draftMusic=level);
   ReadingOptions(scroll,chat);
  } else if(settingsTab==1) {
   KarineUI.SettingsSection(scroll,music,T("settings.music"),T("settings.music.hint"),false);SoundRow(scroll,draftMusic,level=>draftMusic=level);
   KarineUI.SettingsSection(scroll,music,T("settings.sfx"),T("settings.sfx.hint"),true);SoundRow(scroll,draftSfx,level=>draftSfx=level);
  } else {
   KarineUI.SettingsOption(scroll,T("settings.motion"),T("settings.motion.hint"),draftReduced,()=>{draftReduced=!draftReduced;RenderSettings();});
   KarineUI.SettingsSection(scroll,"gear",T("settings.fps"),T("settings.fps.hint"),true);var fps=KarineUI.Row(scroll);fps.style.alignItems=Align.Stretch;
   foreach(int f in FrameRate.Options){int v=f;var o=KarineUI.SettingsOption(fps,T("settings.fps."+v),T("settings.fps."+v+".hint"),draftFps==v,()=>{draftFps=v;RenderSettings();});if(v!=FrameRate.Options[FrameRate.Options.Length-1])o.style.marginRight=KarineTheme.SpaceMd;}
   // Efekt yoğunluğu: gren, yağmur, far, parazit. "Hareketi azalt" açıkken hepsi kapalıdır.
   KarineUI.SettingsSection(scroll,"gear",T("settings.fx"),T("settings.fx.hint"),true);var fx=KarineUI.Row(scroll);fx.style.alignItems=Align.Stretch;
   foreach(FxLevel l in new[]{FxLevel.Off,FxLevel.Light,FxLevel.Full}){var v=l;var o=KarineUI.SettingsOption(fx,T("settings.fx."+v.ToString().ToLowerInvariant()),T("settings.fx."+v.ToString().ToLowerInvariant()+".hint"),draftFx==v,()=>{draftFx=v;RenderSettings();});if(v!=FxLevel.Full)o.style.marginRight=KarineTheme.SpaceMd;}
   KarineUI.FxPreview(scroll,draftFx);
   KarineUI.SettingsOption(scroll,T("settings.haptics"),T("settings.haptics.hint"),draftHaptics,()=>{draftHaptics=!draftHaptics;RenderSettings();});
   SceneOptions(scroll);
   KarineUI.SettingsSection(scroll,"info",T("settings.ads"),T("settings.ads.status."+(AdGateway.Consent==AdConsent.Granted?"granted":AdGateway.Consent==AdConsent.Denied?"denied":"unknown")),true);
   Button(scroll,T("settings.ads.change"),AskForAdConsent);
   KarineUI.Rule(scroll);Button(scroll,T("menu.row.newCareer"),()=>{confirmRestart=true;RestartPage();});
  }
  KarineUI.Rule(body);var footer=KarineUI.Row(body);footer.style.justifyContent=Justify.SpaceBetween;footer.style.flexShrink=0;
  // Kaydırma alanı üst ve alt şeridin altına taşmaz.
  scroll.contentViewport.style.overflow=Overflow.Hidden;scroll.style.overflow=Overflow.Hidden;
  KarineUI.SettingsAction(footer,KarineUI.IconOr("refresh","nav_prev"),T("settings.reset"),T("settings.reset.hint"),false,()=>{draftReduced=false;draftInstant=false;draftMusic=SoundLevel.Half;draftSfx=SoundLevel.Full;draftFps=60;draftFx=FxLevel.Full;draftHaptics=true;ResetSceneDraft();RenderSettings();});
  KarineUI.SettingsAction(footer,KarineUI.IconOr("check","nav_next"),T("settings.save"),T("settings.save.hint"),true,()=>{
   PlayerPrefs.SetInt("karine.reducedMotion",draftReduced?1:0);
   instantText=draftInstant;PlayerPrefs.SetInt("bube.instantText",instantText?1:0);
   SoundSettings.SetMusic(draftMusic);SoundSettings.SetSfx(draftSfx);FrameRate.Set(draftFps);Fx.Set(draftFx,draftHaptics);SaveSceneDraft();PlayerPrefs.Save();ApplySound();Home();
  });
 }
 void SoundRow(VisualElement card,SoundLevel current,Action<SoundLevel> onPick) {
  var row=KarineUI.Row(card);row.style.alignItems=Align.Stretch;
  var levels=SoundSettings.Levels.Reverse().ToArray();
  for(int i=0;i<levels.Length;i++) {
   var captured=levels[i];var key=SoundSettings.LabelKey(captured);
   int bars=captured==SoundLevel.Off?0:Mathf.CeilToInt(4f*(int)captured/100f);
   var choice=KarineUI.SettingsOption(row,key!=null?T(key):"%"+(int)captured,null,current==captured,()=>{onPick(captured);RenderSettings();},null,bars);
   choice.style.minWidth=KarineTheme.TouchTarget*2;if(i<levels.Length-1)choice.style.marginRight=KarineTheme.SpaceSm;
  }
 }
}
}
