using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Difficulty : ScriptableObject {

	// e.g., "Insane", "Easy", "Medium", etc.
	public string difficultyName;

	public float ballSpeed;

    public float ballAcceleration;

	public float spawnPeriod;

	public int bricksPerSpawn;

    public float brickSpeed;
}
