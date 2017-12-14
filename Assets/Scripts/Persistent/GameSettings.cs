using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSettings: Setting<GameModes> { //data is the current game mode

	public ScoreStorage scoreStorage;

	public GameModes currentGameMode {
		get {
			return data;
		}
		set {
			data = value;
			SendDataChangedEvent();
		}
	}

	public int GetHighScore() {
		return scoreStorage.GetHighScore(data);
	}

	public void AddScore(int score) {
		scoreStorage.AddScore(score, data);
	}
    
	// Remove if we do not want to save previous game settings.
	void OnDisable(){
		Save();
	}
}
