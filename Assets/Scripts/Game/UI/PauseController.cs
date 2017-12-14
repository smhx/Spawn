using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

	[SerializeField] GameSceneManager gameManager;

	[SerializeField] GameObject pauseButton;

	Animator pauseAnimator;

	SceneLoader sceneLoader;

	void OnEnable() {
		gameManager.OnGameEnd += () => pauseButton.SetActive (false);
		gameManager.OnPushBall += () => pauseButton.SetActive (true);
		gameManager.OnGameResume += () => pauseButton.SetActive (true);
		gameManager.OnGamePause += () => pauseButton.SetActive (false);
	}

	void OnDisable() {
		gameManager.OnGameEnd -= () => pauseButton.SetActive (false);
		gameManager.OnPushBall -= () => pauseButton.SetActive (true);
		gameManager.OnGameResume -= () => pauseButton.SetActive (true);
		gameManager.OnGamePause -= () => pauseButton.SetActive (false);
	}

	void Start() {
		sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
		pauseAnimator = GetComponent <Animator> ();
		pauseButton.SetActive (false);
	}

	public void PauseButtonClicked() {
		gameManager.Pause ();
		pauseAnimator.SetTrigger ("Pause");
	}

	public void ResumeButtonClicked() {
		pauseAnimator.SetTrigger ("Resume");
		gameManager.ResumeGame ();
	}

	public void HomeButtonClicked() {
		sceneLoader.UnloadScene("Pause");
		sceneLoader.UnloadScene("Game");
		ObjectPooler.SharedInstance.DeactivateAll("Brick");
		StartCoroutine(sceneLoader.LoadSceneAndSetActive("MainMenu"));
	}
}
