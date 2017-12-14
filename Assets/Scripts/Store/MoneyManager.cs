using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : Initializer<PlayerItems> {

	[SerializeField] Text moneyText;

	int moneyScale = 1;

	public void ScaleMoneyBy(int scale) {
		moneyScale *= scale;
	}

	public void ResetMoneyScale() {
		moneyScale = 1;
	}

	public void Pay(int earnings, Vector3 loc) {
		if (earnings > 0) {
			GameObject plusMoney = ObjectPooler.SharedInstance.GetPooledObject ("Plus");
			plusMoney.SetActive (true);
			plusMoney.GetComponent<Plus> ().ShowMoney (earnings*moneyScale,loc);
		}
		( (Store)setting ).IncrementMoneyBy (moneyScale*earnings);
	}

	protected override void ConfigureSettings(PlayerItems dat) {
		moneyText.text = dat.money.ToString ();
	}

}
