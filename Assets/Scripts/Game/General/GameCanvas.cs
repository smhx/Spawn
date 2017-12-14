using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour {

	[SerializeField] GameSceneManager gm;
	[SerializeField] GameObject endMenu;
	[SerializeField] GameObject gameMenu;

	[SerializeField] GameObject playButton;
	[SerializeField] GameObject adButton;

	[SerializeField] GameObject tapToPlay;
	[SerializeField] GameObject dot;


	void OnEnable() {
		gm.OnGameEnd += Die;
		gm.OnGameStart += GameStart;
		gm.OnDeathBeforeAd += AdDeath;
		gm.OnPushBall += PushBall;
		gm.OnGameRevive += Revive;
	}
		
	void OnDisable() {
		gm.OnGameEnd -= Die;
		gm.OnGameStart -= GameStart;
		gm.OnDeathBeforeAd -= AdDeath;
		gm.OnPushBall -= PushBall;
		gm.OnGameRevive -= Revive;

	}

	void Revive() {
		dot.SetActive (true);
		tapToPlay.SetActive (false);
		gameMenu.SetActive (true);
		endMenu.SetActive (false);
	}

	void PushBall() {
		tapToPlay.SetActive (false);
	}

	void GameStart() {
		dot.SetActive (true);
		gameMenu.SetActive (true);
		endMenu.SetActive (false);
		tapToPlay.SetActive (true);
	}

	void Die() {
		dot.SetActive (false);
		gameMenu.SetActive (false);
		endMenu.SetActive (true);
		adButton.SetActive (false);
		playButton.SetActive (true);
	}

	void AdDeath() {
		gameMenu.SetActive (false);
		endMenu.SetActive (true);
		adButton.SetActive (true);
		playButton.SetActive (false);
	}




}
