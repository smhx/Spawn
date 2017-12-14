using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
using System;

public class Ad : MonoBehaviour {

#if UNITY_IOS
	const string gameId = "1527515";
#elif UNITY_ANDROID
	const string gameId = "1527516";
#endif

	public static Ad Instance;

	public static readonly string defaultVideoPlacementID = "video";
	public static readonly string rewardedVideoPlacementID = "rewardedVideo";

	void OnEnable() {
		if (Instance==null) {
			Instance = this;
#if UNITY_ADS
			Advertisement.Initialize (gameId);
#endif
        } else {
			Destroy (this.gameObject);
		}
	}

	public void ShowAdWithCallback(string placementID, Action callback)
	{
		#if UNITY_ADS
		if (Advertisement.IsReady () && !Advertisement.isShowing ) {
			ShowOptions options = new ShowOptions();
			options.resultCallback = (ShowResult r) => {
				if (r == ShowResult.Finished)
					callback ();
			};
			Advertisement.Show(placementID, options);
		}
		#endif
	}
		

    public void ShowAd() {
#if UNITY_ADS
		if (Advertisement.IsReady () && !Advertisement.isShowing  ) {
			Advertisement.Show();
		}
#endif
	}

	public bool CanPlayAd() {
		return Advertisement.IsReady () && !Advertisement.isShowing;
	}
}
