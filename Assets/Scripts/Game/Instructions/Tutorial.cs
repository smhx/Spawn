using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	[SerializeField] GameSceneManager gm;
	[SerializeField] Text inGameText;
	[SerializeField] Text endGameText;
    [SerializeField] Text tapToPlayText;

	[SerializeField] string mechanicsText = "Circle your finger around the dot to move. Tap it to start the brick spawning!";
	[SerializeField] string objectivesText = "Hit bricks to get points and gems. Don't let anything touch the wall!";
	[SerializeField] string storeText = "Visit the store to unlock power-ups, backgrounds, and more!";
	[SerializeField] string hazardText = "Don't hit the glowing red brick! It will flash to warn you before it becomes a hazard brick.";
	[SerializeField] string timedText = "In timed mode, you have one minute to get as many points as possible.";

	bool showingTut;
	GameModes mode;


	public void SetGameMode(GameModes g) {
		mode = g;
	}

	bool IsFirstTime() {
		return 
			!PlayerPrefs.HasKey (GameModes.CLASSIC.ToString ()) &&
			!PlayerPrefs.HasKey (GameModes.TIMED.ToString ()) &&
			!PlayerPrefs.HasKey (GameModes.HAZARD.ToString ());
	}


	void OnEnable () {

		if (PlayerPrefs.HasKey ( mode.ToString () ) ){
			gameObject.SetActive (false);
			showingTut = false;
			return;
		}

		showingTut = true;

		inGameText.gameObject.SetActive (true);
		endGameText.gameObject.SetActive (false);
        tapToPlayText.gameObject.SetActive(false);
		inGameText.text = mechanicsText;

		gm.OnGameStart += GameStarted;
		gm.OnPushBall += PushedBall;
		gm.OnDeathBeforeAd += GameEnded;
		gm.OnGameRevive += Revive;
		gm.OnGameEnd += GameEnded;
	}
	
	void OnDisable() {
		if (showingTut) {
			gm.OnGameStart -= GameStarted;
			gm.OnPushBall -= PushedBall;
			gm.OnDeathBeforeAd -= GameEnded;
			gm.OnGameRevive -= Revive;
			gm.OnGameEnd -= GameEnded;
		}
	}

	void Revive() {
		inGameText.text = "";
		endGameText.text = "";
	}

	void GameStarted() {
		if (PlayerPrefs.HasKey (mode.ToString () )) {
			gameObject.SetActive (false);
			showingTut = false;
			return;
		}
		inGameText.gameObject.SetActive (true);
		endGameText.gameObject.SetActive (false);
		if ( IsFirstTime () )
			inGameText.text = mechanicsText;
		else inGameText.text = "";
	}

	void PushedBall() {
		inGameText.gameObject.SetActive (true);
		if (mode == GameModes.CLASSIC)
			inGameText.text = objectivesText;
		else if (mode == GameModes.HAZARD)
			inGameText.text = hazardText;
		else
			inGameText.text = timedText;
	}

	void GameEnded() {
		inGameText.gameObject.SetActive (false);
		endGameText.gameObject.SetActive (true);

		if (IsFirstTime () ) endGameText.text = storeText;

		PlayerPrefs.SetInt (mode.ToString (), 1);
	}
}
