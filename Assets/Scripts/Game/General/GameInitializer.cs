using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : Initializer<GameModes> {

	[SerializeField] GameObject timer;
	[SerializeField] GameObject timeDisplay;
	[SerializeField] GameObject spawner;
	[SerializeField] BorderController bc;
	[SerializeField] Tutorial tut;

	GameModes mode;

	public GameModes Mode() {return mode;}

	protected override void ConfigureSettings (GameModes gm) {
		mode = gm;
        bc.SetMode(gm);
        switch (gm) {
			case GameModes.CLASSIC:
				InitClassic ();
				break;
			case GameModes.TIMED:
				InitTimed ();
				break;
			case GameModes.HAZARD:
				InitHazard ();
				break;
			default:
				break;
		}
	}
    
	void InitClassic() {
		timeDisplay.SetActive (false);
		timer.SetActive (false);
		spawner.SetActive (true);
		tut.SetGameMode (GameModes.CLASSIC);
	}
    
	void InitHazard () {
		timeDisplay.SetActive (false);
		timer.SetActive (false);
		spawner.SetActive (true);
		tut.SetGameMode (GameModes.HAZARD);

	}

	void InitTimed() {
		timeDisplay.SetActive (true);
		timer.SetActive (true);
		spawner.SetActive (true);
		tut.SetGameMode (GameModes.TIMED);

	}
}
