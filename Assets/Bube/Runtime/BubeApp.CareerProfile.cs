using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Bora sekmesi: karakteri tanıtır ve güvenin nasıl işlediğini açıkça yazar.
// Sayılar elle yazılmaz, `career-rules.json`dan okunur; kural değişirse metin de değişir.
public sealed partial class BubeApp {
 void CareerProfile(VisualElement parent) {
  atlas=atlas??Worlds.Load();var closed=Worlds.Closed(game.Career);
  var country=atlas.countries.Count==0?null:atlas.countries[Worlds.Resume(atlas,closed)];
  var row=new VisualElement();row.style.flexDirection=FlexDirection.Row;row.style.flexGrow=1;row.style.minHeight=0;parent.Add(row);

  var who=KarineUI.BoardPanel(row,T("career.profile.title"));who.style.flexGrow=1;who.style.flexBasis=0;who.style.marginRight=KarineTheme.SpaceMd;who.style.minHeight=0;
  var whoList=Scroll(who);
  var portrait=Resources.Load<Texture2D>("Bube/Characters/bora");
  if(portrait!=null) {
   var image=new VisualElement();image.style.width=96;image.style.height=96;image.style.backgroundImage=portrait;
   image.style.backgroundSize=new BackgroundSize(BackgroundSizeType.Contain);image.style.marginBottom=KarineTheme.SpaceSm;whoList.Add(image);
  }
  KarineUI.RecordLine(whoList,T("career.profile.identity"),T("career.profile.identityValue"),KarineTheme.Secondary);
  KarineUI.RecordLine(whoList,T("career.profile.role"),T("career.profile.roleValue"),KarineTheme.Secondary);
  if(country!=null)KarineUI.RecordLine(whoList,T("career.profile.post"),T(country.nameKey)+(string.IsNullOrEmpty(country.cityKey)?"":" · "+T(country.cityKey)),KarineTheme.Accent);
  KarineUI.RecordLine(whoList,T("career.profile.standing"),T(game.TrustStatusKey),game.Career.probation?KarineTheme.Danger:KarineTheme.Accent);
  KarineUI.Body_(whoList,T("career.profile.bio"),KarineTheme.CareerBoard.RecordLineSize).style.color=KarineTheme.Secondary;

  var rules=KarineUI.BoardPanel(row,T("career.rules.title"));rules.style.flexGrow=1;rules.style.flexBasis=0;rules.style.minHeight=0;
  var list=Scroll(rules);var r=careerRules;
  string Sign(int v)=>v>0?"+"+v:v.ToString();
  string Tiers(string type)=>string.Join("  ·  ",new[]{1,2,3}.Select(level=>T("career.rules.level"+level)+" "+Sign(game.TrustDeltaFor(type,level))));
  KarineUI.Body_(list,string.Format(T("career.rules.intro"),r.initialTrust),KarineTheme.CareerBoard.RecordLineSize).style.color=KarineTheme.Secondary;
  KarineUI.RecordLine(list,T("career.rules.supported"),Tiers("supported"),KarineTheme.CareerBoard.Supported);
  KarineUI.RecordLine(list,T("career.rules.lucky"),Tiers("lucky"),KarineTheme.CareerBoard.Supported);
  KarineUI.RecordLine(list,T("career.rules.incomplete"),Tiers("incomplete"),KarineTheme.Secondary);
  KarineUI.RecordLine(list,T("career.rules.false"),Tiers("falseAccusation"),KarineTheme.Danger);
  KarineUI.RecordLine(list,T("career.rules.streak"),string.Format(T("career.rules.streakValue"),r.streakLength,r.streakBonus,game.Career.streak),KarineTheme.Accent);
  KarineUI.RecordLine(list,T("career.rules.difficulty"),T("career.rules.level"+game.Difficulty),KarineTheme.Secondary);
  KarineUI.Body_(list,string.Format(T("career.rules.probation"),r.probationTrust,r.reinstateTrust),KarineTheme.CareerBoard.RecordLineSize).style.color=KarineTheme.Secondary;
  KarineUI.Body_(list,T("career.rules.statuses"),KarineTheme.CareerBoard.RecordLineSize).style.color=KarineTheme.Secondary;
 }
}
}
