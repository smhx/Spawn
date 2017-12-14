using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/MultiBallAbility")]
public class MultiBallAbility : Ability {

	public List<int> extraBalls;

	protected override void SetUp ()
	{
		return;
	}


	public override void ReTriggerAbility() {
		TriggerAbility ();
	}

	public override void TriggerAbility ()
	{
		// Create a bunch of balls
		Vector2 pos = new Vector2(0, 2.3f); // 2.3 is radius :( 
        int numExtraBalls = extraBalls[level];
		for (int i = 0; i < numExtraBalls; ++i) {
			GameObject ball = ObjectPooler.SharedInstance.GetPooledObject("Ball");
			if (ball != null) {
				ball.SetActive (true);
				ball.transform.position = pos;
				pos = RotateVector (pos, 2 * Mathf.PI / (float)numExtraBalls);
				ball.GetComponent<BallMovement> ().PushBall ();
			}
			else
				Debug.LogError ("MultiBallAbility::TriggerAbility: object pooler spawned null ball");
		}
	}

	public override void EndAbility ()
	{
		return;
	}

	Vector2 RotateVector(Vector2 v, float angle) {
		float x = v.x;
		float y = v.y;
		v.x = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
		v.y = x * Mathf.Sin(angle) + y * Mathf.Cos(angle);
		return v;
	}

}
