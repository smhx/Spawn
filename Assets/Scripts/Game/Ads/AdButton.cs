using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdButton : MonoBehaviour {

	Store store;

	Button btn;

	void Start() {
		store = GameObject.FindWithTag ("Store").GetComponent<Store> ();
		btn = GetComponent<Button> ();
		btn.onClick.AddListener (PlayAdWithReward);
	}

	void Update() {
		btn.enabled = Ad.Instance.CanPlayAd();
	}

	void PlayAdWithReward() {
		Ad.Instance.ShowAdWithCallback (Ad.rewardedVideoPlacementID, Reward);
	}

	void Reward() {
		store.IncrementMoneyBy (100);
	}
}
