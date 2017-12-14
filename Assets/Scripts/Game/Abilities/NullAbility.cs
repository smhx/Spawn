using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/NullAbility")]
public class NullAbility : Ability
{
	protected override void SetUp() {}

	public override void TriggerAbility() { 
	}

	public override void ReTriggerAbility() {
	}

    public override void EndAbility() { }
}
