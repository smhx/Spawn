using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/ScalePointsAbility")]
public class ScalePointsAbility : Ability {

	ScoreKeeper sk;

	public List<int> pointsScale;

	public override void ReTriggerAbility() {
		TriggerAbility ();
	}


	protected override void SetUp() {
		sk = GameObject.FindWithTag ("ScoreKeeper").GetComponent<ScoreKeeper> ();
	}

    public override void TriggerAbility() {
        sk.ScalePointsBy(pointsScale[level]);
	}

	public override void EndAbility() {
		sk.ResetPointsScale ();
	}
}
