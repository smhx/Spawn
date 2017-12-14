using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/SlowDownAbility")]
public class SlowDownAbility : Ability {

	CollisionTracker tracker;
    BallController ballControl;
	Timer timer;

	[SerializeField]
	float slowDownFactor;

	protected override void SetUp ()
	{
		var t = GameObject.FindWithTag ("Timer");
		if (t != null)
			timer = t.GetComponent<Timer> ();
		else
			timer = null;
		tracker = GameObject.FindWithTag("CollisionTracker").GetComponent<CollisionTracker>();
        ballControl = GameObject.FindWithTag("BallController").GetComponent<BallController>();
    }

	public override void ReTriggerAbility() {
		TriggerAbility ();
	}

	public override void TriggerAbility ()
	{
        ballControl.SetBallSpeedScale(slowDownFactor);
		if (timer!=null) timer.SetTimeScale (slowDownFactor);
		tracker.SetTimeScale (slowDownFactor);
	}

	public override void EndAbility ()
    {
        ballControl.SetBallSpeedScale(1f);
		if (timer!=null)timer.SetTimeScale (1f);
		tracker.SetTimeScale (1f);

    }
}
