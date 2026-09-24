using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Bube.Editor {
public static class ProjectSetup {
 [MenuItem("Bube/Validate Content")]
 public static void Validate() {
  var expectedScenes=new[]{"BootScene","MainMenuScene","OfficeScene","InterviewScene"}
   .Select(name=>"Assets/Bube/Scenes/"+name+".unity").ToArray();
  if(expectedScenes.Any(path=>!System.IO.File.Exists(path)))throw new Exception("Required scene missing");
  if(!EditorBuildSettings.scenes.Where(scene=>scene.enabled).Select(scene=>scene.path).SequenceEqual(expectedScenes))
   throw new Exception("Build scene order must be Boot, Main Menu, Office, Interview");
  var config=JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text);
  if(Resources.Load<Font>("Bube/Fonts/IBMPlexMono-Regular")==null || Resources.Load<Font>("Bube/Fonts/IBMPlexMono-SemiBold")==null)throw new Exception("Missing UI font");
  if(Resources.Load<Texture2D>("Bube/DeskReference")==null)throw new Exception("Missing desk background");
  if(Resources.Load<Texture2D>("Bube/CctvTabletHands")==null)throw new Exception("Missing CCTV tablet art");
  if(Resources.Load<Texture2D>("Bube/MainMenuNight")==null)throw new Exception("Missing main menu background");
  if(Resources.Load<Texture2D>("Bube/InterviewRoom")==null)throw new Exception("Missing interview room background");
  if(Resources.Load<Texture2D>("Bube/Characters/bora")==null)throw new Exception("Missing Bora sprite");
  var locale=JsonUtility.FromJson<Locale>(Resources.Load<TextAsset>("Bube/Locales/"+config.locale).text);
  foreach(var asset in Resources.LoadAll<TextAsset>("Bube/Cases")) {
   var data=JsonUtility.FromJson<CaseData>(asset.text);
   if(data.nodes.Select(n=>n.id).Distinct().Count()!=data.nodes.Length) throw new Exception("Duplicate node ID");
   foreach(var id in data.verdicts.SelectMany(v=>v.supportingSourceIds ?? new string[0]).Concat(data.methods.SelectMany(v=>v.supportingSourceIds ?? new string[0])).Concat(data.evidence.SelectMany(v=>v.supportingSourceIds ?? new string[0]))) {
    var parts=id.Split('#');
    var source=data.nodes.FirstOrDefault(n=>n.id==parts[0]);
    if(source==null || parts.Length>2 || parts.Length==2 && (source.kind!="cctv" || !(source.cctvEvents ?? new CctvEvent[0]).Any(e=>e.id==parts[1])))throw new Exception("Unknown report support source: "+id);
   }
   if(data.failedReportTrustLoss<0 || data.successfulReportTrustGain<0)throw new Exception("Negative career impact");
   if(data.summary==null || new[]{data.summary.locationKey,data.summary.truthKey,data.summary.evidenceKey,data.summary.lessonKey}.Any(key=>string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("[")))throw new Exception("Missing case summary");
   var timeline=data.timelineClues ?? new TimelineClue[0];
   if(timeline.Select(c=>c.id).Distinct().Count()!=timeline.Length)throw new Exception("Duplicate timeline clue ID");
   foreach(var clue in timeline) {
    if(string.IsNullOrEmpty(clue.id) || clue.sortMinute<0 || clue.sortMinute>1439)throw new Exception("Invalid timeline clue");
    if(new[]{clue.timeKey,clue.noteKey,clue.sourceKey}.Any(key=>string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("[")))throw new Exception("Missing timeline text: "+clue.id);
    if((clue.requiresRead ?? new string[0]).Any(id=>!data.nodes.Any(n=>n.id==id)))throw new Exception("Unknown timeline source: "+clue.id);
    if((clue.requiresAsked ?? new string[0]).Any(id=>!data.nodes.SelectMany(n=>n.questions ?? new Question[0]).Any(q=>q.id==id)))throw new Exception("Unknown timeline question: "+clue.id);
   }
   foreach(var verdict in data.verdicts)if(string.IsNullOrEmpty(verdict.labelKey) || locale.Get(verdict.labelKey).StartsWith("["))throw new Exception("Missing verdict label");
   foreach(var n in data.nodes) {
    if(n.requestable && (n.kind!="document" || n.requestDelaySeconds<0 || string.IsNullOrEmpty(n.requestLabelKey) || locale.Get(n.requestLabelKey).StartsWith("[")))throw new Exception("Invalid document request: "+n.id);
    if((n.requires ?? new string[0]).Concat(n.requiresAny ?? new string[0]).Any(id=>!data.nodes.Any(x=>x.id==id))) throw new Exception("Unknown prerequisite");
    if((n.requiresAsked ?? new string[0]).Concat(n.requiresAnyAsked ?? new string[0]).Any(id=>!data.nodes.SelectMany(x=>x.questions ?? new Question[0]).Any(q=>q.id==id)))throw new Exception("Unknown node question prerequisite");
    foreach(var key in new[]{n.titleKey,n.bodyKey,"kind."+n.kind}) if(locale.Get(key).StartsWith("["))throw new Exception("Missing text: "+key);
   if(n.kind=="interview") {
     if(string.IsNullOrEmpty(n.personId) || n.questions==null || n.questions.Length==0 || n.completionQuestionIds==null || n.completionQuestionIds.Length==0)throw new Exception("Incomplete interview: "+n.id);
     if(Resources.Load<Texture2D>("Bube/Characters/"+n.personId)==null)Debug.LogWarning("Using generated placeholder portrait for "+n.personId);
     foreach(var key in new[]{n.personNameKey,n.personInfoKey}.Concat(n.questions.SelectMany(q=>new[]{q.promptKey,q.answerKey})))
      if(string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("["))throw new Exception("Missing interview text: "+key);
     if(!string.IsNullOrEmpty(n.personQuoteKey) && locale.Get(n.personQuoteKey).StartsWith("["))throw new Exception("Missing interview quote: "+n.personQuoteKey);
     if(n.completionQuestionIds.Any(id=>!n.questions.Any(q=>q.id==id)))throw new Exception("Unknown completion question: "+n.id);
     foreach(var q in n.questions) {
      if(!string.IsNullOrEmpty(q.topicKey) && locale.Get(q.topicKey).StartsWith("["))throw new Exception("Missing interview topic: "+q.id);
      if(!string.IsNullOrEmpty(q.presentedSourceId) && !data.nodes.Any(x=>x.id==q.presentedSourceId && (x.kind=="document" || x.kind=="cctv" || x.kind=="bps")))throw new Exception("Unknown presentable source: "+q.id);
      foreach(var sourceRef in q.presentedSourceIds ?? new string[0]) {
       var separator=sourceRef.IndexOf('#');
       var source=data.nodes.FirstOrDefault(x=>x.id==(separator<0?sourceRef:sourceRef.Substring(0,separator)));
       if(source==null)throw new Exception("Unknown interview source: "+q.id);
       if(separator>=0) {
        var detail=sourceRef.Substring(separator+1);
        if(source.kind=="cctv" && !(source.cctvEvents ?? new CctvEvent[0]).Any(e=>e.id==detail) ||
           source.kind=="interview" && !(source.questions ?? new Question[0]).Any(other=>other.id==detail.Split('|')[0]) ||
           source.kind!="cctv" && source.kind!="interview")throw new Exception("Unknown interview source detail: "+q.id);
       }
      }
      foreach(var response in q.presentedAnswers ?? new PresentedAnswer[0])
       if(!(q.presentedSourceIds ?? new string[0]).Contains(response.sourceId) || locale.Get(response.answerKey).StartsWith("["))throw new Exception("Invalid source response: "+q.id);
      foreach(var key in (q.answerVariants ?? new AnswerVariant[0]).Select(v=>v.answerKey))if(string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("["))throw new Exception("Missing response variant: "+key);
      foreach(var id in (q.requiresAsked ?? new string[0]).Concat(q.requiresAnyAsked ?? new string[0]).Concat(q.excludesAsked ?? new string[0]).Concat((q.answerVariants ?? new AnswerVariant[0]).SelectMany(v=>(v.requiresAsked ?? new string[0]).Concat(v.excludesAsked ?? new string[0]))))
       if(!data.nodes.SelectMany(x=>x.questions ?? new Question[0]).Any(x=>x.id==id))throw new Exception("Unknown question prerequisite: "+id);
      foreach(var id in (q.requiresRead ?? new string[0]).Concat((q.answerVariants ?? new AnswerVariant[0]).SelectMany(v=>v.requiresRead ?? new string[0])))
       if(!data.nodes.Any(x=>x.id==id))throw new Exception("Unknown response evidence prerequisite: "+id);
     }
    }
    foreach(var field in n.fileMeta ?? new FileMeta[0]) foreach(var key in new[]{field.labelKey,field.valueKey})
     if(string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("["))throw new Exception("Missing dossier text: "+key);
    if(!string.IsNullOrEmpty(n.imageResource)) {
     if(Resources.Load<Texture2D>(n.imageResource)==null)throw new Exception("Missing dossier image: "+n.imageResource);
     if(string.IsNullOrEmpty(n.imageCaptionKey) || locale.Get(n.imageCaptionKey).StartsWith("["))throw new Exception("Missing dossier caption");
    }
    if(n.kind=="cctv") {
     if(n.cctvEvents==null || n.cctvEvents.Length==0)throw new Exception("CCTV events missing");
     if(n.cctvEvents.Any(e=>string.IsNullOrEmpty(e.id)) || n.cctvEvents.Select(e=>e.id).Distinct().Count()!=n.cctvEvents.Length)throw new Exception("CCTV event IDs missing or duplicated");
     if(n.cctvEvents.Any(e=>e.delayMs<0))throw new Exception("Negative CCTV reveal delay");
     foreach(var record in n.cctvEvents) if(!string.IsNullOrEmpty(record.videoPath) &&
       !System.IO.File.Exists(System.IO.Path.Combine(Application.streamingAssetsPath,record.videoPath)))
      throw new Exception("Missing CCTV footage: "+record.videoPath);
     if(data.id=="case001" && n.cctvEvents.Any(e=>(e.id=="gap" || e.id=="lost") && !string.IsNullOrEmpty(e.videoPath)))
      throw new Exception("Case 001 signal gap must have no footage");
     foreach(var key in new[]{n.cctvSourceKey,n.cctvPeriodKey}.Concat(new[]{n.cctvOverlayKey}.Where(k=>!string.IsNullOrEmpty(k)))
       .Concat(n.cctvEvents.SelectMany(e=>new[]{e.textKey,e.overlayTimeKey,e.glitchKey,e.signalKey}).Where(k=>!string.IsNullOrEmpty(k))))
      if(string.IsNullOrEmpty(key) || locale.Get(key).StartsWith("["))throw new Exception("Missing CCTV text: "+key);
    }
   }
   if(data.nodes.SelectMany(n=>n.questions ?? new Question[0]).Select(q=>q.id).Distinct().Count()!=data.nodes.Sum(n=>(n.questions ?? new Question[0]).Length))throw new Exception("Duplicate question ID");
   if((data.conclusionRequires ?? new string[0]).Any(id=>!data.nodes.Any(x=>x.id==id))) throw new Exception("Unknown conclusion prerequisite");
   var game=new Investigation(data);
   if(game.Available(data.nodes[0]))throw new Exception("Case available before acceptance");
   if(game.CanConclude || game.SubmitFinalReport("hasan","spare","recovery","recovery","mert_follow","recovery"))throw new Exception("Premature conclusion");
   if(!game.AcceptCase())throw new Exception("Case acceptance failed");
   if(data.id=="case001") {
    var pace=new Investigation(data);
    pace.AcceptCase();
    pace.Read("report");
    var byId=data.nodes.ToDictionary(n=>n.id);
    if(pace.Discovered(byId["elif"]) || pace.Discovered(byId["hasan"]) || pace.Discovered(byId["camera"]))throw new Exception("Case 001 sources opened before the player found them");
    if(!pace.RequestInterview("mert",0) || !pace.Ask("mert","mert.day") || !pace.Ask("mert","mert.key"))throw new Exception("Case 001 Mert introduction failed");
    if(!pace.Discovered(byId["elif"]) || pace.Discovered(byId["mert_follow"]) || pace.Discovered(byId["hasan"]))throw new Exception("Case 001 key route pacing failed");
    if(!pace.Ask("mert","mert.neighbor") || !pace.Discovered(byId["hasan"]) || pace.Discovered(byId["camera"]))throw new Exception("Case 001 neighbor route pacing failed");
    if(!pace.RequestInterview("hasan",0) || !pace.Ask("hasan","hasan.sighting") || pace.Discovered(byId["camera"]) || !pace.Ask("hasan","hasan.camera") || !pace.Discovered(byId["camera"]))throw new Exception("Case 001 camera route pacing failed");
    if(pace.Discovered(byId["mert_follow"]) || !pace.Read("camera") || pace.Discovered(byId["mert_follow"]))throw new Exception("Case 001 key follow-up opened without a relevant claim");
    var elifPath=new Investigation(data,JsonUtility.FromJson<Progress>(JsonUtility.ToJson(pace.State)));
    if(!elifPath.RequestInterview("elif",0) || !elifPath.Ask("elif","elif.relationship") || !elifPath.Ask("elif","elif.visit") ||
       !elifPath.RequestInterview("elif_follow",0) || !elifPath.Ask("elif_follow","elif_follow.footage","camera#elif_in") ||
       !elifPath.Discovered(byId["mert_follow"]))throw new Exception("Case 001 Elif disclosure did not open Mert follow-up");
    if(!locale.Get("elif_follow.footage.entryAnswer").Contains("saksı"))throw new Exception("Case 001 Elif entry reply omitted the key clue");
    if(!pace.Ask("mert","mert.lock") || !pace.Discovered(byId["mert_follow"]))throw new Exception("Case 001 locked-door route did not open Mert follow-up");
    if(pace.Discovered(byId["recovery"]))throw new Exception("Case 001 recovery request opened before a follow-up source");
   }
   if(data.id=="case001" && CaseSearch.Find(game,locale,"12.37").Length!=0)throw new Exception("Unread CCTV leaked into search");
   for(int i=0;i<data.nodes.Length;i++) foreach(var n in data.nodes.Where(game.Discovered)) {
    if(n.kind=="interview" && game.CanRequest(n))game.RequestInterview(n.id,0);
    if(game.CanRequestDocument(n)) {
     if(!game.RequestDocument(n.id,0) || game.Available(n) || !game.HasIncomingDocument || !game.ReceiveDocument(n.id) || game.ReceiveDocument(n.id))throw new Exception("Document request/inbox flow invalid: "+n.id);
    }
    foreach(var q in n.questions ?? new Question[0])if(game.QuestionAvailable(n,q)) {
     var chosenSource=q.presentedSourceIds!=null && q.presentedSourceIds.Length>0?q.presentedSourceIds[0]:q.presentedSourceId;
     if(game.QuestionNeedsSource(q) && game.ReportSourceAvailable(chosenSource) && game.Ask(n.id,q.id,"report"))throw new Exception("Unrelated source accepted: "+q.id);
     game.Ask(n.id,q.id,chosenSource);
    }
    if(game.Available(n))game.Read(n.id);
   }
   if(game.State.read.Count!=data.nodes.Length || !game.CanConclude)throw new Exception("Unreachable content");
   if(data.id=="case001") {
    if(!CaseSearch.Find(game,locale,"12.37").Any(hit=>hit.nodeId=="camera" && hit.eventId=="gap"))throw new Exception("Inspected CCTV line missing from search");
    if(!CaseSearch.Find(game,locale,"Geçen ay kapıda kalmıştım").Any(hit=>hit.turnReference=="mert_follow#mert_follow.spare"))throw new Exception("Interview search did not identify the answer");
    if(!CaseSearch.Find(game,locale,"BİLGİSAYAR").Any())throw new Exception("Turkish case-insensitive search failed");
    var privateAnswer=JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
    privateAnswer.interviewTurns.RemoveAll(t=>t.questionId=="mert_follow.spare");
    var reduced=new Investigation(data,privateAnswer);
    if(CaseSearch.Find(reduced,locale,"Geçen ay kapıda kalmıştım").Any())throw new Exception("Unasked answer leaked into search");
   }
   if(timeline.Length>0) {
    var clue=timeline[0];
    if(!game.TimelineAvailable(clue) || !game.PinTimeline(clue.id) || game.PinTimeline(clue.id))throw new Exception("Timeline pin failed");
    var restoredTimeline=JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
    if(!restoredTimeline.timelinePinned.Contains(clue.id) || !game.UnpinTimeline(clue.id) || game.UnpinTimeline(clue.id))throw new Exception("Timeline save/remove failed");
   }
   foreach(var request in game.State.documentRequests) {
    var restoredRequest=JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
    if(!restoredRequest.documentRequests.Any(r=>r.nodeId==request.nodeId && r.readyAtUtcTicks==request.readyAtUtcTicks))throw new Exception("Document request did not survive save/load");
   }
   if(game.State.interviewTurns.Count==0 || game.State.interviewTurns.Any(turn=>string.IsNullOrEmpty(turn.answerKey) || locale.Get(turn.answerKey).StartsWith("[")))throw new Exception("Interview transcript missing a recorded response");
   if(game.State.interviewTurns.Any(turn=>data.nodes.SelectMany(n=>n.questions ?? new Question[0]).Any(q=>q.id==turn.questionId && (!string.IsNullOrEmpty(q.presentedSourceId) && q.presentedSourceId!=turn.sourceId || q.presentedSourceIds!=null && q.presentedSourceIds.Length>0 && !q.presentedSourceIds.Contains(turn.sourceId)))))throw new Exception("Presented source missing from transcript");
   var restoredProgress=JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
   if(restoredProgress.interviewTurns.Count!=game.State.interviewTurns.Count || restoredProgress.interviewTurns.Where((turn,index)=>turn.answerKey!=game.State.interviewTurns[index].answerKey).Any())throw new Exception("Interview transcript did not survive save/load");
   if(data.nodes.Any(n=>n.kind=="cctv") && !data.nodes.Any(n=>n.kind=="cctv" && game.Meets(n.requires)))throw new Exception("Camera unavailable");
   var ready=JsonUtility.FromJson<Progress>(JsonUtility.ToJson(game.State));
   var readySnapshot=JsonUtility.ToJson(ready);
   if(data.id=="case001") {
    var witnessState=JsonUtility.FromJson<Progress>(readySnapshot);
    witnessState.asked.Remove("hasan_follow.mertStatement");
    witnessState.interviewTurns.RemoveAll(t=>t.questionId=="hasan_follow.mertStatement");
    var witnessGame=new Investigation(data,witnessState);
    var statement="mert_follow#mert_follow.spare";
    if(!witnessGame.ReportSourceAvailable(statement) || witnessGame.Ask("hasan_follow","hasan_follow.mertStatement","mert_follow") ||
       !witnessGame.Ask("hasan_follow","hasan_follow.mertStatement",statement))throw new Exception("Witness answer could not be presented to Hasan");
    var witnessTurn=witnessGame.State.interviewTurns.Last(t=>t.questionId=="hasan_follow.mertStatement");
    if(witnessTurn.sourceId!=statement || witnessTurn.answerKey!="hasan_follow.mertStatement.answer")throw new Exception("Witness confrontation was not recorded");
    var absentState=JsonUtility.FromJson<Progress>(readySnapshot);
    absentState.asked.Remove("hasan_follow.mertStatement");
    absentState.interviewTurns.RemoveAll(t=>t.questionId=="hasan_follow.mertStatement" || t.questionId=="mert_follow.spare");
    var absentGame=new Investigation(data,absentState);
    if(absentGame.ReportSourceAvailable(statement) || absentGame.Ask("hasan_follow","hasan_follow.mertStatement",statement))throw new Exception("Unavailable witness answer was presented");
    var repeatState=JsonUtility.FromJson<Progress>(readySnapshot);
    repeatState.asked.Remove("elif_follow.footage");
    repeatState.interviewTurns.RemoveAll(t=>t.questionId=="elif_follow.footage");
    var repeatGame=new Investigation(data,repeatState);
    var followNode=data.nodes.First(n=>n.id=="elif_follow");
    var followQuestion=followNode.questions.First(q=>q.id=="elif_follow.footage");
    if(!repeatGame.Ask(followNode.id,followQuestion.id,"camera#elif_in") || !repeatGame.CanAskQuestion(followNode,followQuestion))throw new Exception("Second source did not reopen question");
    if(repeatGame.Ask(followNode.id,followQuestion.id,"camera#elif_in") || !repeatGame.Ask(followNode.id,followQuestion.id,"camera#elif_out") || repeatGame.CanAskQuestion(followNode,followQuestion))throw new Exception("Repeated interview source handling failed");
    var repeatTurns=repeatGame.State.interviewTurns.Where(t=>t.questionId==followQuestion.id).ToArray();
    if(repeatTurns.Length!=2 || repeatTurns[0].answerKey==repeatTurns[1].answerKey)throw new Exception("Follow-up transcript lost distinct answers");
    var repeatSaved=JsonUtility.FromJson<Progress>(JsonUtility.ToJson(repeatGame.State));
    if(repeatSaved.interviewTurns.Count(t=>t.questionId==followQuestion.id)!=2)throw new Exception("Follow-up transcript did not survive save");
    foreach(var sourceId in new[]{"camera#elif_in","camera#elif_out"}) {
     var turnState=JsonUtility.FromJson<Progress>(readySnapshot);
     turnState.asked.Remove("elif_follow.footage");
     turnState.interviewTurns.RemoveAll(t=>t.questionId=="elif_follow.footage");
     var turnGame=new Investigation(data,turnState);
     if(turnGame.Ask("elif_follow","elif_follow.footage","camera#gap") || !turnGame.Ask("elif_follow","elif_follow.footage",sourceId))throw new Exception("CCTV line did not control Elif follow-up");
     var recorded=turnGame.State.interviewTurns.Last(t=>t.questionId=="elif_follow.footage");
     if(recorded.sourceId!=sourceId || recorded.answerKey!=(sourceId=="camera#elif_in"?"elif_follow.footage.entryAnswer":"elif_follow.footage.exitAnswer"))throw new Exception("CCTV answer variant was not recorded");
    }
   }
   if(data.id=="case001") {
    var fromElif=JsonUtility.FromJson<Progress>(readySnapshot);
    fromElif.asked.Remove("mert_follow.spare");fromElif.read.Remove("mert_follow");
    var elifRoute=new Investigation(data,fromElif);
    if(!elifRoute.Discovered(data.nodes.First(n=>n.id=="hasan_follow")))throw new Exception("Elif statement did not open Hasan follow-up");
    var fromMert=JsonUtility.FromJson<Progress>(readySnapshot);
    fromMert.asked.Remove("elif_follow.footage");fromMert.read.Remove("elif_follow");
    var mertRoute=new Investigation(data,fromMert);
    if(!mertRoute.Discovered(data.nodes.First(n=>n.id=="hasan_follow")))throw new Exception("Mert statement did not open Hasan follow-up");
   }
   if(data.id=="case001") {
   var wrongGame=new Investigation(data,ready);
   var wrongSuspect=data.verdicts.First(v=>!v.correct).id;
   if(!wrongGame.SubmitFinalReport(wrongSuspect,"spare","recovery","recovery","mert_follow","recovery") || !wrongGame.State.closed)throw new Exception("Wrong report did not close case");
   if(wrongGame.Career.departmentTrust!=60 || wrongGame.Career.pendingReviews.Count!=1)throw new Exception("Review applied too early");
    wrongGame.BeginNextCaseReview(7);
   if(wrongGame.DeliverNextFax()!=null)throw new Exception("Fax arrived before delay");
   wrongGame.Career.pendingReviews[0].readyAtUtcTicks=DateTime.UtcNow.AddTicks(-1).Ticks;
   var wrongFax=wrongGame.DeliverNextFax();
   if(wrongFax==null || wrongFax.correct || wrongFax.suspectSupported || !wrongFax.methodSupported || !wrongFax.proofSupported || wrongGame.Career.departmentTrust!=60-data.failedReportTrustLoss)throw new Exception("Failed fax impact invalid");
   if(wrongGame.DeliverNextFax()!=null || wrongGame.Career.reviewHistory.Count!=1)throw new Exception("Duplicate fax review");
   var restoredCareer=JsonUtility.FromJson<CareerProgress>(JsonUtility.ToJson(wrongGame.Career));
   var restoredGame=new Investigation(data,wrongGame.State,restoredCareer);
   if(restoredGame.Career.reviewHistory.Count!=1 || restoredGame.DeliverNextFax()!=null || restoredGame.Career.departmentTrust!=wrongGame.Career.departmentTrust)throw new Exception("Career save replayed fax");
   var rightGame=new Investigation(data,JsonUtility.FromJson<Progress>(readySnapshot));
   if(rightGame.SubmitFinalReport(data.verdicts.First(v=>v.correct).id,"spare","recovery","missing","mert_follow","recovery"))throw new Exception("Unseen report source accepted");
   if(!rightGame.SubmitFinalReport(data.verdicts.First(v=>v.correct).id,"spare","recovery","recovery","mert_follow","recovery") || !rightGame.State.closed)throw new Exception("Correct report did not close case");
   rightGame.BeginNextCaseReview(7);
   rightGame.Career.pendingReviews[0].readyAtUtcTicks=DateTime.UtcNow.AddTicks(-1).Ticks;
   var rightFax=rightGame.DeliverNextFax();
   if(rightFax==null || !rightFax.correct || !rightFax.suspectSupported || !rightFax.methodSupported || !rightFax.proofSupported || rightGame.Career.departmentTrust!=60+data.successfulReportTrustGain)throw new Exception("Successful fax invalid");
   if(rightFax.suspectSourceId!="recovery" || rightFax.methodSourceId!="mert_follow" || rightFax.proofSourceId!="recovery")throw new Exception("Report source chain was not saved in fax");
   var unlinkedGame=new Investigation(data,JsonUtility.FromJson<Progress>(readySnapshot));
   if(!unlinkedGame.SubmitFinalReport(data.verdicts.First(v=>v.correct).id,"spare","recovery","report","mert_follow","recovery") || unlinkedGame.Career.pendingReviews[0].evaluationType=="supported")throw new Exception("Unsupported source chain was accepted as supported");
   var cameraGame=new Investigation(data,JsonUtility.FromJson<Progress>(readySnapshot));
   if(cameraGame.ReportSourceAvailable("camera") || !cameraGame.ReportSourceAvailable("camera#gap") || cameraGame.ReportSourceAvailable("camera#unknown"))throw new Exception("CCTV event source validation failed");
   if(!cameraGame.SubmitFinalReport(data.verdicts.First(v=>v.correct).id,"spare","recovery","recovery","camera#gap","recovery") || cameraGame.State.reportMethodSource!="camera#gap")throw new Exception("Selected CCTV line was not saved in report");
   var partialGame=new Investigation(data,JsonUtility.FromJson<Progress>(readySnapshot));
   var wrongMethod=data.methods.First(v=>!v.correct).id;
   if(!partialGame.SubmitFinalReport(data.verdicts.First(v=>v.correct).id,wrongMethod,"recovery","recovery","mert_follow","recovery"))throw new Exception("Partial report rejected");
   partialGame.BeginNextCaseReview(7);
   partialGame.Career.pendingReviews[0].readyAtUtcTicks=DateTime.UtcNow.AddTicks(-1).Ticks;
   var partialFax=partialGame.DeliverNextFax();
   if(partialFax==null || partialFax.evaluationType!="incomplete" || partialFax.trustChange>=0 || partialFax.trustChange<=wrongFax.trustChange)throw new Exception("Partial report weighting invalid");
   }
   if(data.id=="case002") {
    if(data.nodes.Any(n=>n.kind=="cctv") || !data.nodes.Any(n=>n.kind=="bps"))throw new Exception("Case 002 must use archive records rather than CCTV");
    var proof=data.evidence.First(e=>e.correct);
    var suspect=data.verdicts.First(v=>v.correct);
    var method=data.methods.First(m=>m.correct);
    var correctGame=new Investigation(data,JsonUtility.FromJson<Progress>(readySnapshot));
    if(!correctGame.SubmitFinalReport(suspect.id,method.id,proof.id,"parcel","access","parcel"))throw new Exception("Case 002 supported report rejected");
    correctGame.BeginNextCaseReview(7);
    correctGame.Career.pendingReviews[0].readyAtUtcTicks=DateTime.UtcNow.AddTicks(-1).Ticks;
    var fax=correctGame.DeliverNextFax();
    if(fax==null || !fax.correct || !fax.suspectSupported || !fax.methodSupported || !fax.proofSupported)throw new Exception("Case 002 source evaluation failed");
    var falseGame=new Investigation(data,JsonUtility.FromJson<Progress>(readySnapshot));
    if(!falseGame.SubmitFinalReport("deniz",method.id,proof.id,"access","access","parcel") ||
       falseGame.Career.pendingReviews[0].evaluationType!="falseAccusation")throw new Exception("Case 002 false accusation handling failed");
   }

  }
  Debug.Log("BUBE VALIDATION PASSED");
 }
 public static void Setup() {
  PlayerSettings.productName=JsonUtility.FromJson<GameConfig>(Resources.Load<TextAsset>("Bube/config").text).title;
  PlayerSettings.companyName="bubeGames";
  PlayerSettings.defaultInterfaceOrientation=UIOrientation.AutoRotation;
  PlayerSettings.allowedAutorotateToLandscapeLeft=true; PlayerSettings.allowedAutorotateToLandscapeRight=true;
  PlayerSettings.allowedAutorotateToPortrait=false; PlayerSettings.allowedAutorotateToPortraitUpsideDown=false;
  PlayerSettings.defaultScreenWidth=1280; PlayerSettings.defaultScreenHeight=720;
  System.IO.Directory.CreateDirectory("Assets/Bube/Scenes");
  const string legacyPath="Assets/Bube/Scenes/Bootstrap.unity";
  if(!System.IO.File.Exists(legacyPath)) {
   var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   new GameObject("Bube Application").AddComponent<BubeApp>();
   EditorSceneManager.SaveScene(scene,legacyPath);
  }
  var scenePaths=new[]{"BootScene","MainMenuScene","OfficeScene","InterviewScene"}
   .Select(name=>"Assets/Bube/Scenes/"+name+".unity").ToArray();
  if(scenePaths.Any(path=>!System.IO.File.Exists(path)))throw new Exception("Required scene missing");
  EditorBuildSettings.scenes=scenePaths.Select(path=>new EditorBuildSettingsScene(path,true)).ToArray();
  Validate();
  AssetDatabase.SaveAssets(); Debug.Log("BUBE SETUP COMPLETE");
 }
}
}
