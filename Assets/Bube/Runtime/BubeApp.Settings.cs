using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Ayarlar modalı. Açıldığı ekranın üstüne biner; kapanınca o ekranda kalınır.
// Beş kategori (Genel, Ses, Görüntü, Erişilebilirlik, Oynanış); her değişiklik
// taslakta tutulur ve yalnız Kaydet ile yazılır. Bu dosya `BubeApp`in parçasıdır.
public sealed partial class BubeApp {
 int settingsTab;
 bool draftInstant,draftReduced,draftHaptics,draftShapes,draftCrt,draftAsh,draftMeter;float draftScaleValue=1f;int draftFps;FxLevel draftFx;
 SoundLevel draftMusic,draftSfx;string draftLanguage;
 Action settingsOpener,settingsEscape;

 void SettingsPage()=>SettingsFrom(Home);
 void SettingsFrom(Action opener) {
  settingsOpener=opener;settingsEscape=escapeBack;
  settingsTab=0;draftReduced=KarineMotion.Reduced;draftInstant=instantText;draftMusic=SoundSettings.Music;draftSfx=SoundSettings.Sfx;draftFps=FrameRate.Current;draftFx=Fx.Level;draftHaptics=Fx.Haptics;LoadSceneDraft();draftLanguage=language;
  RenderSettings();
 }
 void CloseSettings(){root.Q("SettingsModal")?.RemoveFromHierarchy();escapeBack=settingsEscape;}

 static readonly string[] SettingsTabs={"general","sound","display","access","play"};
 static readonly string[] SettingsIcons={"gear","music","binoculars","info","gamepad"};

 void RenderSettings() {
  // Yeniden çizimde kaydırma yeri korunur: bir anahtara basınca liste başa dönmez.
  float offset=root.Q<ScrollView>("SettingsBody")?.scrollOffset.y??0;
  VisualElement tabs,body,footer;
  KarineUI.SettingsModal(root,T("menu.settings"),T("settings.subtitle"),CloseSettings,out tabs,out body,out footer);
  Back(CloseSettings);
  for(int i=0;i<SettingsTabs.Length;i++) {
   int index=i;
   KarineUI.SettingsTab(tabs,KarineUI.IconOr(SettingsIcons[i],"gear"),T("settings.tab."+SettingsTabs[i]),T("settings.tab."+SettingsTabs[i]+".hint"),settingsTab==i,()=>{settingsTab=index;RenderSettings();});
  }
  switch(settingsTab) {
   case 0: GeneralSettings(body);break;
   case 1: SoundSettingsTab(body);break;
   case 2: DisplaySettings(body);break;
   case 3: AccessSettings(body);break;
   default: PlaySettings(body);break;
  }
  var scroll=root.Q<ScrollView>("SettingsBody");
  if(offset>0){EventCallback<GeometryChangedEvent> restore=null;restore=e=>{scroll.UnregisterCallback(restore);scroll.scrollOffset=new Vector2(0,offset);};scroll.RegisterCallback(restore);}
  KarineUI.SettingsFooterButton(footer,KarineUI.IconOr("refresh","nav_prev"),T("settings.reset"),T("settings.reset.hint"),false,()=>{
   draftReduced=false;draftInstant=false;draftMusic=SoundLevel.Full;draftSfx=SoundLevel.Full;draftFps=120;draftFx=FxLevel.Full;draftHaptics=true;ResetSceneDraft();RenderSettings();});
  KarineUI.SettingsFooterButton(footer,KarineUI.IconOr("check","nav_next"),T("settings.save"),T("settings.save.hint"),true,SaveSettings);
 }

 void SaveSettings() {
  // Yerleşimi değiştiren ayarlar açan ekranın yeniden çizilmesini ister.
  bool relayout=!Mathf.Approximately(draftScaleValue,Typography.Scale)||draftShapes!=Shapes||draftSpacing!=(PlayerPrefs.GetInt(SpacingKey,0)==1)||
   draftContrast!=(PlayerPrefs.GetInt(ContrastKey,0)==1)||draftOneHand!=(PlayerPrefs.GetInt(OneHandKey,0)==1);
  PlayerPrefs.SetInt("karine.reducedMotion",draftReduced?1:0);
  instantText=draftInstant;PlayerPrefs.SetInt("bube.instantText",instantText?1:0);
  SoundSettings.SetMusic(draftMusic);SoundSettings.SetSfx(draftSfx);FrameRate.Set(draftFps);Fx.Set(draftFx,draftHaptics);SaveSceneDraft();PlayerPrefs.Save();ApplySound();
  if(draftLanguage!=language) {
   language=draftLanguage;PlayerPrefs.SetString(Languages.PrefKey,language);PlayerPrefs.Save();
   locale=LocaleLoader.LoadPlayable(language);game.Text=locale;KarineUI.TextCulture=new System.Globalization.CultureInfo(language);relayout=true;
  }
  CloseSettings();
  if(relayout)settingsOpener?.Invoke();
 }

 // Satır yardımcıları: anahtar, seçim kartları, kaydırıcı.
 void SwitchRow(VisualElement body,string key,bool on,Action flip)=>
  KarineUI.SettingSwitch(KarineUI.SettingRow(body,T(key),T(key+".hint")),on,()=>{flip();RenderSettings();});
 VisualElement CardRow(VisualElement body,string key,string hint=null)=>KarineUI.SettingRow(body,T(key),T(hint??key+".hint"),true);
 void Choice(VisualElement row,string title,string detail,bool selected,Action pick,bool quarter=false) {
  var card=KarineUI.SettingCard(row,null,title,detail,selected,()=>{pick();RenderSettings();});if(quarter)KarineUI.Quarter(card);
 }
 void LevelRow(VisualElement body,string key,SoundLevel level,Action<SoundLevel> set)=>
  KarineUI.SettingSlider(KarineUI.SettingRow(body,T(key),T(key+".hint")),(int)level/100f,4,v=>KarineUI.Percent(Mathf.RoundToInt(v*100)),v=>set((SoundLevel)(Mathf.RoundToInt(v*4)*25)));

 void GeneralSettings(VisualElement body) {
  var languages=Languages.Available();
  if(languages.Length>1) {
   var row=CardRow(body,"settings.language");
   foreach(var code in languages){var v=code;Choice(row,Languages.Endonym(v),null,draftLanguage==v,()=>draftLanguage=v,true);}
  }
  var speed=CardRow(body,"settings.textSpeed","settings.text.hint");
  Choice(speed,T("settings.instant"),T("settings.instant.hint"),draftInstant,()=>draftInstant=true);
  Choice(speed,T("settings.normal"),T("settings.normal.hint"),!draftInstant,()=>draftInstant=false);
  float min=Typography.MinScale,max=Typography.MaxScale;
  KarineUI.SettingSlider(KarineUI.SettingRow(body,T("settings.textScale"),T("settings.textScale.hint")),(draftScaleValue-min)/(max-min),
   Mathf.RoundToInt((max-min)/.05f),v=>KarineUI.Percent(Mathf.RoundToInt((min+v*(max-min))*100)),v=>draftScaleValue=Mathf.Round((min+v*(max-min))*20)/20f);
 }
 void SoundSettingsTab(VisualElement body) {
  LevelRow(body,"settings.music",draftMusic,l=>draftMusic=l);
  LevelRow(body,"settings.sfx",draftSfx,l=>draftSfx=l);
 }
 void DisplaySettings(VisualElement body) {
  // Efekt yoğunluğu: gren, yağmur, far, parazit. "Hareketi azalt" açıkken hepsi kapalıdır.
  var fx=CardRow(body,"settings.fx");
  foreach(var level in new[]{FxLevel.Off,FxLevel.Light,FxLevel.Full}){var v=level;var id=v.ToString().ToLowerInvariant();Choice(fx,T("settings.fx."+id),T("settings.fx."+id+".hint"),draftFx==v,()=>draftFx=v);}
  KarineUI.FxPreview(body,draftFx);
  var fps=CardRow(body,"settings.fps");
  foreach(int f in FrameRate.Options){int v=f;Choice(fps,T("settings.fps."+v),T("settings.fps."+v+".hint"),draftFps==v,()=>draftFps=v);}
  SwitchRow(body,"settings.crt",draftCrt,()=>draftCrt=!draftCrt);
  SwitchRow(body,"settings.ash",draftAsh,()=>draftAsh=!draftAsh);
  var color=CardRow(body,"settings.colorFilter","settings.color.hint");
  for(int i=0;i<4;i++){int v=i;Choice(color,T("settings.color."+v),null,draftColor==v,()=>draftColor=v,true);}
  LampOptions(CardRow(body,"settings.lampTint","settings.lamp.hint"));
 }
 void AccessSettings(VisualElement body) {
  SwitchRow(body,"settings.motion",draftReduced,()=>draftReduced=!draftReduced);
  SwitchRow(body,"settings.captions",draftCaptions,()=>draftCaptions=!draftCaptions);
  SwitchRow(body,"settings.spacing",draftSpacing,()=>draftSpacing=!draftSpacing);
  SwitchRow(body,"settings.contrast",draftContrast,()=>draftContrast=!draftContrast);
  SwitchRow(body,"settings.oneHand",draftOneHand,()=>draftOneHand=!draftOneHand);
  SwitchRow(body,"settings.shapes",draftShapes,()=>draftShapes=!draftShapes);
 }
 void PlaySettings(VisualElement body) {
  SwitchRow(body,"settings.haptics",draftHaptics,()=>draftHaptics=!draftHaptics);
  if(draftHaptics){var strength=CardRow(body,"settings.hapticPower","settings.hapticStrength.hint");
   for(int i=0;i<3;i++){int v=i;Choice(strength,T("settings.hapticStrength."+v),null,draftStrength==v,()=>draftStrength=v);}}
  Button(KarineUI.SettingRow(body,T("settings.career"),T("settings.career.hint")),T("menu.row.newCareer"),()=>{CloseSettings();confirmRestart=true;RestartPage();});
  if(DevMeterAllowed) {
   SwitchRow(body,"settings.devMeter",draftMeter,()=>draftMeter=!draftMeter);
   Button(KarineUI.SettingRow(body,T("settings.devLab"),null),T("settings.devLab"),()=>{CloseSettings();DevLab();});
  }
 }
}
}
