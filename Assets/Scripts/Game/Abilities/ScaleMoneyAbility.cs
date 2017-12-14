using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/ScaleMoneyAbility")]
public class ScaleMoneyAbility : Ability {

	MoneyManager m_and_m;
	public List<int> moneyScale;

	public override void ReTriggerAbility() {
		TriggerAbility ();
	}


	protected override void SetUp() {
		m_and_m = GameObject.FindWithTag ("MoneyManager").GetComponent<MoneyManager> ();
	}

	public override void TriggerAbility() {
		m_and_m.ScaleMoneyBy (moneyScale[level]);
	}

	public override void EndAbility() {
		m_and_m.ResetMoneyScale ();
	}
}
