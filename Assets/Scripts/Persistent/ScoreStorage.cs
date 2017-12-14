using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public enum GameModes {
	CLASSIC=0,
	TIMED,
	HAZARD
};
	 
[System.Serializable]
public class HighScores {
    List<int> highscores;

    public int this[GameModes gm]
    {
        get {
            return highscores[(int)gm];
        }
    }


    public void AddScore(int score, GameModes gm)
    {
		
        if (gm == GameModes.CLASSIC)
        {
            Social.ReportScore(score, "CgkItKD16O4XEAIQBA", (bool success) => {
                if (success) { Debug.Log("Score Reported"); }
                else { Debug.Log("Score Report Failed"); }
            });
        }
        else if (gm == GameModes.TIMED)
        {
            Social.ReportScore(score, "CgkItKD16O4XEAIQBQ", (bool success) => {
                if (success) { Debug.Log("Score Reported"); }
                else { Debug.Log("Score Report Failed"); }
            });
        }
        else if (gm == GameModes.HAZARD)
        {
            Social.ReportScore(score, "CgkItKD16O4XEAIQBg", (bool success) => {
                if (success) { Debug.Log("Score Reported"); }
                else { Debug.Log("Score Report Failed"); }
            });
        }
            
           
        if (score > this[gm])
            highscores[(int)gm] = score;
    }

    public HighScores(List<int> _highscores)
    {
        highscores = _highscores;
    }

    public HighScores()
    {
        highscores = new List<int> { 0, 0, 0 };
    }
}

public class ScoreStorage : Storage<HighScores> {

//	void Awake() {
//		Debug.LogWarning ("Resetting ScoreStorage data");
//		ResetAllData();
//	}
	public void AddScore(int score, GameModes gm) {
		data.AddScore (score, gm);
		Save ();
		Load ();
	}

    public int GetHighScore(GameModes gm) {
		return data[gm];
	}
}
