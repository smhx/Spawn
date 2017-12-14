using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	[SerializeField] Text timeDisplay;
	[SerializeField] GameSceneManager gameManager;
	[SerializeField] int penalty = 5;
	[SerializeField] int countdown = 10;
	int secondsLeft;
	float secondsInUpdate = 0f;
	bool shouldCountDown = false;
	float timeScale;
    bool watchedAd;
	bool frozen;

	void OnEnable () {
		Reset ();
        watchedAd = false;
		gameManager.OnPushBall += () => shouldCountDown = true;
		gameManager.OnGameStart += Reset;
		gameManager.OnGamePause += () => shouldCountDown = false;
		gameManager.OnGameResume += () => shouldCountDown = true;
        gameManager.OnGameEnd += () => watchedAd = false;
		gameManager.OnGameRevive += Reset;
		gameManager.OnDeathBeforeAd += Ad;
        DisplayTime ();
	}

	void OnDisable () {
		secondsLeft = 0;
		gameManager.OnPushBall -= () => shouldCountDown = true;
		gameManager.OnGameStart -= Reset;
		gameManager.OnGamePause -= () => shouldCountDown = false;
		gameManager.OnGameResume -= () => shouldCountDown = true;
        gameManager.OnGameEnd -= () => watchedAd = false;
        gameManager.OnGameRevive -= Reset;
		gameManager.OnDeathBeforeAd -= Ad;
    }

    void Ad()
    {
        watchedAd = true;
        Reset();
    }

	void Reset() {
		frozen = false;
        if (watchedAd)
            secondsLeft = 20;
        else
		    secondsLeft = 60;
		secondsInUpdate = 0;
		shouldCountDown = false;
		timeScale = 1f;
		DisplayTime ();
	}

	void DisplayTime () {
		timeDisplay.text = FormatSeconds(secondsLeft);
	}

	string FormatSeconds(int s) {
		int min = s / 60;
		int sec = s - 60 * min;
		string x = sec<10 ? "0"+sec.ToString() : sec.ToString ();
		return min.ToString () + ":" + x;  
	}

	void DecrementTimeBy(int d) {
		secondsLeft -= d;
		if (secondsLeft <= 0) {
			shouldCountDown = false;
//			Audio.Instance.StopClock ();
			gameManager.Kill ();
		} else if (secondsLeft <= countdown && secondsLeft+d > countdown) {
//			if (timeScale == 1f)
//				Audio.Instance.PlayClock ();
//			else if (timeScale < 1f)
//				Audio.Instance.PlayClockSlow ();
//			else {
//				Audio.Instance.PlayClock ();
//				Debug.LogWarning ("Timer::DecrementTimeBy timeScale = " + timeScale.ToString ());
//			}
		}
		DisplayTime ();
	}


    public void BallHitBorder() {
		if (shouldCountDown == false )
			return;
		if (gameManager.DecrementLife () ) {
			DecrementTimeBy (penalty);
			Audio.Instance.PlayTimePenalty ();
		}
    }

    public void BrickHitBorder() {
		if (shouldCountDown == false )
			return;
		if (gameManager.DecrementLife () ) {
			DecrementTimeBy (penalty);
			Audio.Instance.PlayTimePenalty ();
		}
    }

	public void IncrementTimeBy(int d) {
		secondsLeft += d;
		secondsInUpdate = 0f;
//		if (secondsLeft > countdown && secondsLeft - d <= countdown) {
//			Audio.Instance.StopClock ();
//		}
//		else if (secondsLeft < countdown) {
//			Debug.Log ("Playing clock increment");
//			if (timeScale == 1f)
//				Audio.Instance.PlayClock ();
//			else if (timeScale < 1f)
//				Audio.Instance.PlayClockSlow ();
//			else {
//				Audio.Instance.PlayClock ();
//				Debug.LogWarning ("Timer::DecrementTimeBy timeScale = " + timeScale.ToString ());
//			}
//		}
		DisplayTime ();
	}

	public void SetTimeScale(float s) {timeScale = s;}

	void Update() {
		if (!shouldCountDown || frozen) {
			return;
		}
		secondsInUpdate += Time.deltaTime * timeScale;
		if (secondsInUpdate >= 1) {
			DecrementTimeBy (1);
			secondsInUpdate = 0;
		}
	}
    public float GetTime() { return secondsLeft; }

	public void Freeze() { 
		frozen = true; 
	}
    public void Unfreeze() { 
		frozen = false; 
	}
}
