using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/ClearAbility")]
public class ClearAbility : Ability {
    

	protected override void SetUp () {
	}

	public override void TriggerAbility () {
		foreach (var brick in ObjectPooler.SharedInstance.GetAllActiveGameObjects ("Brick") ) 
			if (brick.gameObject.activeInHierarchy) brick.GetComponent<BrickBoss>().Kill ();
	}

	public override void EndAbility () {}

	public override void ReTriggerAbility() {
		foreach (var brick in ObjectPooler.SharedInstance.GetAllActiveGameObjects ("Brick") ) 
			if (brick.gameObject.activeInHierarchy) brick.GetComponent<BrickBoss>().Kill ();
	}

}
