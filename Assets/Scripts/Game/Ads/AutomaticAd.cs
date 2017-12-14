using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticAd : MonoBehaviour {

	[SerializeField] GameSceneManager gameManager;

	void OnEnable() {
		gameManager.OnGameEnd += Died;
	}

	void OnDisable() {
		gameManager.OnGameEnd -= Died;
	}

	void Died() {

		if (!PlayerPrefs.HasKey ("ad_state"))
			PlayerPrefs.SetInt ("ad_state", 1);

		int state = PlayerPrefs.GetInt ("ad_state");

		if (state >= 7 ) {
			PlayerPrefs.SetInt ("ad_state", 1);
			Ad.Instance.ShowAd ();
		} else {
			PlayerPrefs.SetInt ("ad_state", state+1);
		}
	}

}
