using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reviver : MonoBehaviour {

	[SerializeField] float lifetime = 5f;
	[SerializeField] GameSceneManager gm;
	[SerializeField] Image back;

	[SerializeField] Button btn;

    bool buttonClicked = false;
	float fill = 1f;

	void OnEnable() {
		fill = 1f;
        buttonClicked = false;
	}

	void Start() {
		btn.onClick.AddListener (Revive);
	}

	void Update() {
		if (buttonClicked) {
			btn.enabled = false;
			return;
		}
		btn.enabled = Ad.Instance.CanPlayAd ();
		if (fill > 0) {
			fill -= Time.deltaTime / lifetime;
			back.fillAmount = fill;
		} else {
			gameObject.SetActive (false);
			gm.Kill ();
		}
	}

	void Revive() {
		if (!buttonClicked) {
			Ad.Instance.ShowAdWithCallback( Ad.rewardedVideoPlacementID, () => gm.Revive () );
			buttonClicked = true;
			gameObject.SetActive (false);
		}
	}

}
