using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/ExtraTimeAbility")]
public class ExtraTimeAbility : Ability {

	public int extraSeconds;

	BrickSpawner bs;

	bool inTimedMode;

	Timer timer; 

	protected override void SetUp() {
		var timerGameObject = GameObject.FindWithTag ("Timer");
		if (!timerGameObject.activeSelf) {
			inTimedMode = false;
			return;
		}
		inTimedMode = true;
		timer = timerGameObject.GetComponent<Timer> ();
	}

	public override void TriggerAbility() {
		if (inTimedMode)
			timer.IncrementTimeBy (extraSeconds);
	}

	public override void EndAbility() {
		return;
	}

	public override void ReTriggerAbility() {}

}
