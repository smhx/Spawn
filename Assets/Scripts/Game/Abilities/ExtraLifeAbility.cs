using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/ExtraLifeAbility")]
public class ExtraLifeAbility : Ability {


	GameSceneManager gm;

	protected override void SetUp ()
	{
		gm = GameObject.FindWithTag ("GameManager").GetComponent<GameSceneManager> ();
	}

	public override void TriggerAbility ()
	{
		gm.AddLife ();
	}

	public override void EndAbility () {}

	public override void ReTriggerAbility() 
	{
		TriggerAbility ();
	}

}
