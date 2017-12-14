using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu()]
public class Brick : ScriptableObject {

    public string brickName;

    public AbilityTypes brickAbility;

    public Color storeColor; //the color that is shown in the store display (what the brick actually looks like)

    public Color color;

	public Gradient gradient;

	public Color lifeColor = Color.clear; // Null if none

    public Sprite sprite;
    
	public BrickTypes brickType;

	public int timesToHit;
    
	// This is the score you get when you kill it.
	public int score;

	public int earnings;

	public bool IsBrickType(BrickTypes bt) {
		return bt==brickType;
	}
}
