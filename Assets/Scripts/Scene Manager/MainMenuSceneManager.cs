using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MainMenuSceneManager : MonoBehaviour {

	public ParticleSystem stars;
    
	public Animator playAnimator;
	public Animator canvasAnimator;

	SceneLoader sceneLoader;
	GameSettings gameSettings;
    
    string diffName;

    void Awake() {
		if (GameObject.FindWithTag ("SceneLoader") == null)
			return;
		sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
		gameSettings = GameObject.FindWithTag("GameSettings").GetComponent<GameSettings>();
    }

	void OnEnable() {
		stars.Play();
	}

	IEnumerator SlideOut(string scene) {
		stars.Stop ();
		canvasAnimator.SetTrigger ("SlideOut");
		yield return new WaitForSeconds (0.5f);
		sceneLoader.LoadScene (scene);
		sceneLoader.UnloadScene("MainMenu");
	}

	public void PlayMode (string modeName) {

		GameModes gm;
		switch (modeName) {
			case "Classic":
				gm = GameModes.CLASSIC;
				break;
			case "Timed":
				gm = GameModes.TIMED;
                break;
			case "Hazard":
				gm = GameModes.HAZARD;
				break;
			default:
				gm = GameModes.CLASSIC;
                break;
		}
		if (gameSettings==null || sceneLoader==null) {
			Debug.Log ("Testing");
			return;
		}
		gameSettings.currentGameMode = gm;
        StartCoroutine (SlideOut("Game"));
	}

	public void GoToStore() {
		StartCoroutine (SlideOut ("Store"));
	}

	public void GoToHighScores() {
		StartCoroutine (SlideOut ("HighScores"));
	}

	public void Play() {
		playAnimator.SetTrigger ("showPanel");
	}

	public void Test() {
		Debug.Log ("Testing");
	}

    public void ShowLeaderboard() {
        Social.ShowLeaderboardUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
