using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/ShieldAbility")]
public class ShieldAbility : Ability {

	BorderController bc;

	protected override void SetUp() {
		GameObject b = GameObject.FindWithTag ("BorderController");
		bc = b.GetComponent<BorderController> ();
	}

	public override void ReTriggerAbility() {
		bc.Shield ();
	}

	public override void TriggerAbility() {
		bc.Shield ();
	}

	public override void EndAbility() {
		bc.EndShield ();
	}
}
