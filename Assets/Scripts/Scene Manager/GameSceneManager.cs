using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSceneManager : MonoBehaviour
{

	public float borderRadius;
	[SerializeField] Text endScoreDisplay;
	[SerializeField] Text endHighScoreDisplay;
	[SerializeField] ScoreKeeper scoreKeeper;
	[SerializeField] Text lifeText;
	[SerializeField] Animator heartAnimator;
    [SerializeField] BrickSpawner spawner;
    [SerializeField] GameInitializer gameInit;
    [SerializeField] Timer timer;

	SceneLoader sceneLoader;

    float ballSpeed;
    int highscore;
	int lives;

	bool watchedAdToRevive;
	bool dotCanBeClicked;

    void OnEnable() {
        Play();
    }


    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action OnPushBall;
    public event Action OnGamePause;
    public event Action OnGameResume;
    public event Action OnGameRevive; //called when a player dies but has an extra life
	public event Action OnDeathBeforeAd; // called when a player dies for the first time

	public int Lives() {return lives;}

	void Awake() {
		sceneLoader = GameObject.FindWithTag ("SceneLoader").GetComponent <SceneLoader> ();
	}

    public void Play() {
		lives = 1;
		lifeText.text = "x1";
		if (OnGameStart != null)
			OnGameStart ();
        highscore = scoreKeeper.GetHighScore();
        watchedAdToRevive = false;
		dotCanBeClicked = true;
    }

	public void Revive(){
		if (OnGameRevive != null)
			OnGameRevive ();
		dotCanBeClicked = true;

		lives = 1;
		lifeText.text = "x"+lives.ToString ();
	}

    public bool Kill() {
		if (lives==0) {
			if (OnGameEnd != null)
				OnGameEnd ();
			dotCanBeClicked = false;

			return false;
		}

		--lives;
		lifeText.text = "x"+lives.ToString ();

		if (lives > 0) {
            if (gameInit.Mode() == GameModes.TIMED && timer.GetTime() <= 0)
            {
                timer.IncrementTimeBy((lives) * 20); //20 seconds per life
                lives = 1;
                lifeText.text = "x1";
                return false;
            }
			if (OnGameRevive != null) {
				OnGameRevive ();
				dotCanBeClicked = true;

				Audio.Instance.PlayRevive ();
			}
            return false;
		}
		

		if (watchedAdToRevive) {
			if (OnGameEnd != null)
				OnGameEnd ();
			dotCanBeClicked = false;


			DisplayEndScores();

			return true;
		
		} 

		watchedAdToRevive = true;

		if (OnDeathBeforeAd != null)
			OnDeathBeforeAd ();
		dotCanBeClicked = false;

		DisplayEndScores();

		return false;

    }

	public bool DecrementLife() { // Called by timer. 
		if (lives <= 1)
			return true;
		--lives;
        lifeText.text = "x" + lives.ToString();
        return false;
	}

    public void PushBall() {

		if (dotCanBeClicked && OnPushBall != null) {
			dotCanBeClicked = false;
			OnPushBall ();
		}
		
    }

	public void Pause() {
		if (OnGamePause != null) {
			OnGamePause ();
		}

	}

    public void ResumeGame() {
        if (OnGameResume != null)
            OnGameResume();
    }

    void DisplayEndScores() {
        int score = scoreKeeper.GetScore();
        endScoreDisplay.text = score.ToString();
		if (score <= highscore)
			endHighScoreDisplay.text = highscore.ToString ();
		else
			endHighScoreDisplay.text = score.ToString ();
    }

	public void AddLife() { 
		++lives; 
		lifeText.text = "x" + lives.ToString ();
		heartAnimator.SetTrigger ("pump");
	}

    public void GoToScene(string sceneName)
    {
		sceneLoader.LoadScene (sceneName);
		sceneLoader.UnloadScene ("Game");
		sceneLoader.SetActiveScene (sceneName);
    }
}
