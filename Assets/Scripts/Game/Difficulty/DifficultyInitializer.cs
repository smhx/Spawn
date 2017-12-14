using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DifficultyInitializer : Initializer<GameModes> {

	public List<Difficulty> difficulties;
    public Difficulty difficulty;
	public BrickSpawner spawner;
	public BallController ballControl;
    List<BrickMovement> brickMovements;

	protected override void ConfigureSettings(GameModes gm){
		difficulty = difficulties[(int)gm];
        ballControl.InitializeBallSpeed(difficulty.ballSpeed);
        ballControl.InitializeBallAcceleration(difficulty.ballAcceleration);
        spawner.bricksPerSpawn = difficulty.bricksPerSpawn;
        spawner.spawnPeriod = difficulty.spawnPeriod;
        spawner.anglePerShift = 6 * difficulty.spawnPeriod * Mathf.Deg2Rad;
    }

    public Difficulty GetDifficulty() { return difficulty; }

}
