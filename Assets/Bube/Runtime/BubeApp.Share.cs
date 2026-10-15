using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bube {
// Kapanış kartını paylaşma (15 Ekim 2026): kart ekranda çizilir, yalnız kartın
// dikdörtgeni ekran görüntüsünden kesilir ve işletim sisteminin paylaşma penceresine verilir.
public sealed partial class BubeApp {
 void ShareClosedCase(FaxReview fax) {
  var asset=Resources.Load<TextAsset>("Bube/Cases/"+fax.caseId);
  var data=asset==null?null:JsonUtility.FromJson<CaseData>(asset.text);
  var full=data!=null?T(data.titleKey):T("file.unit");
  var parts=full.Split(new[]{'—'},2);
  var number=parts[0].Trim();var title=parts.Length>1?parts[1].Trim():full;
  var text=string.Format(T("share.text"),number,title);
  KarineUI.ShareCard(root,Resources.Load<Texture2D>("Bube/Art/Covers/"+fax.caseId),number,title,T("case.closedStamp"),
   EvaluationTitle(fax),T("career.trust")+": "+T(TrustStatusKey(fax.trustAfter)),"bubegames.com/karine",
   T("share.close"),T("share.action"),face=>StartCoroutine(CaptureAndShare(face,fax.caseId,text)));
 }

 IEnumerator CaptureAndShare(VisualElement face,string caseId,string text) {
  yield return new WaitForEndOfFrame();
  var shot=ScreenCapture.CaptureScreenshotAsTexture();
  // UI koordinatı (sol üst köken) → ekran pikseli (sol alt köken).
  var scale=Screen.height/Mathf.Max(1f,root.layout.height);
  var r=face.worldBound;
  int x=Mathf.Clamp(Mathf.RoundToInt(r.xMin*scale),0,shot.width-1),w=Mathf.Clamp(Mathf.RoundToInt(r.width*scale),1,shot.width-x);
  int h=Mathf.Clamp(Mathf.RoundToInt(r.height*scale),1,shot.height);
  int y=Mathf.Clamp(shot.height-Mathf.RoundToInt(r.yMax*scale),0,shot.height-h);
  var card=new Texture2D(w,h,TextureFormat.RGB24,false);
  card.SetPixels(shot.GetPixels(x,y,w,h));card.Apply();
  var path=Path.Combine(Application.persistentDataPath,"karine-"+caseId+".png");
  File.WriteAllBytes(path,card.EncodeToPNG());
  Destroy(shot);Destroy(card);
  Platform.Share(path,text);
 }
}
}
