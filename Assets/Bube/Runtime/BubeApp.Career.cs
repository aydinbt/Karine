using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Kariyer panosu: Vakalar sayfasıyla aynı iskelet (canlı arka plan, sol sütun).
// Sayıların hepsi kayıtlı kariyerden ve dünya atlasından okunur.
public sealed partial class BubeApp {
 void StatisticsPage() {
  Back(Home);StopCctvVideo();EnsureScene("MainMenuScene");
  showingInterviewList=false;root.Clear();root.style.backgroundColor=KarineTheme.Background;
  var main=CasePageShell("CareerPanel",false);
  KarineUI.CareerTabs(main,
   (T("career.tab.general"),careerTab==0,()=>{careerTab=0;StatisticsPage();}),
   (T("career.tab.history"),careerTab==1,()=>{careerTab=1;StatisticsPage();}),
   (T("career.tab.record"),careerTab==2,()=>{careerTab=2;StatisticsPage();}),
   (T("archive.menu"),false,ArchivePage));
  var history=game.Career.reviewHistory;
  if(careerTab==1){CareerHistory(main,history);KarineUI.CardsDrop(main);return;}
  if(careerTab==2){CareerRecord(main,history);KarineUI.CardsDrop(main);return;}
  atlas=atlas??Worlds.Load();var closed=Worlds.Closed(game.Career);
  int total=atlas.countries.Sum(c=>c.slots.Count),done=atlas.countries.Sum(c=>Worlds.CompletedIn(c,closed));
  int worlds=atlas.countries.Count(c=>c.slots.Count>0&&Worlds.CompletedIn(c,closed)==c.slots.Count);
  float ratio=total==0?0:(float)done/total;

  var overall=KarineUI.BoardPanel(main,T("career.overall"));overall.style.flexShrink=0;
  var figures=new VisualElement();figures.style.flexDirection=FlexDirection.Row;figures.style.marginBottom=KarineTheme.SpaceSm;overall.Add(figures);
  KarineUI.BoardFigure(figures,atlas.countries.Count.ToString(),T("career.board.countries"),worlds+" / "+atlas.countries.Count,false);
  KarineUI.BoardFigure(figures,total.ToString(),T("career.board.cases"),done+" / "+total,true);
  KarineUI.BoardFigure(figures,KarineUI.Percent(Mathf.RoundToInt(ratio*100)),T("career.board.progress"),null,true);
  KarineUI.BoardBar(overall,ratio,KarineTheme.CareerBoard.BarHeight);

  int pending=game.Career.pendingReviews.Count;
  int supported=history.Count(r=>r.evaluationType=="supported"),incomplete=history.Count(r=>r.evaluationType=="incomplete"),wrong=history.Count(r=>r.evaluationType=="falseAccusation");
  var tiles=new VisualElement {name="CareerTiles"};tiles.style.flexDirection=FlexDirection.Row;tiles.style.flexShrink=0;
  tiles.style.marginTop=KarineTheme.SpaceSm;tiles.style.marginBottom=KarineTheme.SpaceSm;main.Add(tiles);
  KarineUI.BoardTile(tiles,"folder",history.Count.ToString(),T("career.board.done"));
  KarineUI.BoardTile(tiles,"document",wrong.ToString(),T("career.board.failed"));
  TrustBadge(KarineUI.BoardTile(tiles,"chart",KarineUI.Percent(game.Career.departmentTrust),T(game.TrustStatusKey)));
  KarineUI.BoardTile(tiles,"clock",pending.ToString(),T("career.board.pending")).style.marginRight=0;

  var lower=new VisualElement();lower.style.flexDirection=FlexDirection.Row;lower.style.flexGrow=1;lower.style.minHeight=0;main.Add(lower);
  var results=KarineUI.BoardPanel(lower,T("career.board.results"));results.style.flexGrow=1;results.style.flexBasis=0;
  results.style.marginRight=KarineTheme.SpaceMd;results.style.overflow=Overflow.Hidden;
  var body=new VisualElement();body.style.flexDirection=FlexDirection.Row;body.style.alignItems=Align.Center;body.style.flexGrow=1;results.Add(body);
  var tones=new[]{KarineTheme.CareerBoard.Supported,KarineTheme.Accent,KarineTheme.Secondary,KarineTheme.Danger};
  var counts=new[]{supported,pending,incomplete,wrong};int sum=counts.Sum();
  body.Add(new KarineUI.CareerRing(counts,tones,T("career.board.total"),sum.ToString(),T("career.board.unit")));
  var legend=new VisualElement();legend.style.flexGrow=1;legend.style.marginLeft=KarineTheme.SpaceLg;legend.style.minWidth=0;legend.style.flexShrink=1;body.Add(legend);
  var labels=new[]{T("career.supportedCount"),T("career.pending"),T("career.incompleteCount"),T("career.falseCount")};
  for(int i=0;i<4;i++)KarineUI.BoardLegend(legend,tones[i],labels[i],counts[i],sum==0?0:Mathf.RoundToInt(100f*counts[i]/sum));

  var countries=KarineUI.BoardPanel(lower,T("career.board.countryProgress"));countries.style.flexGrow=1;countries.style.flexBasis=0;countries.style.overflow=Overflow.Hidden;
  var list=new KarineScrollView();list.style.flexGrow=1;list.style.minHeight=0;countries.Add(list);
  for(int i=0;i<atlas.countries.Count;i++) {
   var country=atlas.countries[i];int pick=i;int n=Worlds.CompletedIn(country,closed);
   KarineUI.BoardCountry(list.contentContainer,country.id,LoadWorldArt(country.image),T(country.nameKey),n+" / "+country.slots.Count,
    country.slots.Count==0?0:(float)n/country.slots.Count,!Worlds.CountryUnlocked(atlas,i,closed),()=>{worldPick=pick;WorldPage();});
  }
  KarineUI.CardsDrop(main);
 }

 void CareerHistory(VisualElement parent,List<FaxReview> history) {
  var box=KarineUI.BoardPanel(parent,T("career.tab.history"));box.style.flexGrow=1;box.style.minHeight=0;
  CareerWall(box,history);
  var list=Scroll(box);
  if(history.Count==0) KarineUI.Body_(list,T("career.noHistory"),KarineTheme.CaseBrowser.TextSize).style.color=KarineTheme.Secondary;
  foreach(var review in history.AsEnumerable().Reverse()) {
   var asset=Resources.Load<TextAsset>("Bube/Cases/"+review.caseId);
   var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
   var title=data==null?review.caseId:T(data.titleKey);
   var direction=review.trustChange>0?" ↑":review.trustChange<0?" ↓":" —";
   Button(list,title+"  ·  "+EvaluationTitle(review)+direction+
    (review.reopened?"  ·  "+T("retry.recordShort"):""),()=>CareerRecordPage(review));
  }
 }
}
}
