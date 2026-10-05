using NUnit.Framework;
using UnityEngine;

namespace Bube.Tests {
public class SightLineTests {
 static PlanPoint P(float x,float y) => new PlanPoint{x=x,y=y};
 static readonly PlanOccluder[] Wall={ new PlanOccluder{id="van",points=new[]{P(.4f,.4f),P(.6f,.4f),P(.6f,.6f),P(.4f,.6f)}} };

 [Test] public void SegmentsCross() => Assert.That(SightLine.Hit(new Vector2(0,0),new Vector2(1,1),new Vector2(0,1),new Vector2(1,0)),Is.EqualTo(.5f).Within(1e-4));
 [Test] public void ParallelSegmentsMiss() => Assert.That(SightLine.Hit(Vector2.zero,Vector2.right,Vector2.up,Vector2.one),Is.EqualTo(-1));
 [Test] public void OccluderBlocksLine() => Assert.IsTrue(SightLine.Blocked(new Vector2(.1f,.5f),new Vector2(.9f,.5f),Wall));
 [Test] public void LineAroundOccluderIsClear() => Assert.IsFalse(SightLine.Blocked(new Vector2(.1f,.2f),new Vector2(.9f,.2f),Wall));

 [Test] public void FacingAwayDoesNotSee() {
  var m=new PlanMarker{x=.1f,y=.2f,facing=180,fov=90};
  Assert.IsFalse(SightLine.Sees(m,new Vector2(.9f,.2f),Wall));
  m.facing=0;Assert.IsTrue(SightLine.Sees(m,new Vector2(.9f,.2f),Wall));
 }

 [Test] public void ConeStopsAtOccluder() {
  var m=new PlanMarker{x=.1f,y=.5f,facing=0,fov=2};
  var fan=SightLine.Cone(m,Wall,4);
  Assert.That(fan.Count,Is.EqualTo(6));
  Assert.That(fan[3].x,Is.EqualTo(.4f).Within(1e-3));
 }
}
}
