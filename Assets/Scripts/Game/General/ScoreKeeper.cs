using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	[SerializeField] Text inGameScoreText;
	[SerializeField] Text highScoreText;
	[SerializeField] GameObject newHighScoreMessage;
	[SerializeField] TextAnimation scoreAnim;
	[SerializeField] GameSceneManager gameManager;

	int score;
    int highScore;

	bool newHighScore = false;
	int pointsScale = 1;


	GameSettings gameSettings;

	void Awake () {
		gameSettings = GameObject.FindWithTag ("GameSettings").GetComponent<GameSettings>();
    }


    void OnEnable() {
        gameManager.OnGameStart += Initialize;
        gameManager.OnGameEnd += SaveScore;
        gameManager.OnDeathBeforeAd += SaveScore;
    }

    void OnDisable() {
        gameManager.OnGameStart -= Initialize;
        gameManager.OnGameEnd -= SaveScore;
        gameManager.OnDeathBeforeAd -= SaveScore;
    }


    void Initialize() {
		highScore = gameSettings.GetHighScore ();
        score = 0;
		newHighScore = false;
		newHighScoreMessage.SetActive (false);
        DisplayScore();
    }


    void DisplayScore() {
        inGameScoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }

    
	void SaveScore(){
		newHighScoreMessage.SetActive (newHighScore);
		gameSettings.AddScore (highScore);
	}

    // Has to do with scores.

    public int GetScore() { return score; }

	public int GetHighScore() { return highScore; }
    
	public void IncrementScoreBy(int x) {
		if (x <= 0)
			return;
		score += pointsScale*x;
        if (score > highScore) { 
			if (!newHighScore)
				Audio.Instance.PlayNewHighScore ();
			newHighScore = true;
			highScore = score; 

		} 
        DisplayScore();
		scoreAnim.Bounce ();
    }


    public void SetScore(int _score) {
        score = _score;
        if (score > highScore) { highScore = score; }
        DisplayScore();
    }

	public void ScalePointsBy(int scale) {
		pointsScale *= scale;
	}

	public void ResetPointsScale() {
		pointsScale = 1;
	}
}