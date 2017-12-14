using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CollisionTracker : MonoBehaviour {

	static readonly int numAbilities = 10;

	[SerializeField] ScoreKeeper scoreKeeper;
	[SerializeField] MoneyManager moneyManager;
	[SerializeField] AbilityDisplay displayer;
	[SerializeField] GameSceneManager gameManager;

	float [] time;
	bool [] usesIcon;
	float [] expectedDuration;

	float timeScale;

	Ability [] abilityForBrick;

	Queue<float> [] endPoints;

	void OnEnable() {
		gameManager.OnGameEnd += Die;
        gameManager.OnGameRevive += Die;
		gameManager.OnDeathBeforeAd += Die ;
    }

	void OnDisable() {
		gameManager.OnGameEnd -= Die;
        gameManager.OnGameRevive -= Die;
		gameManager.OnDeathBeforeAd -= Die ;
	
    }



	void Start () {
		timeScale = 1f;
		time = new float[numAbilities];
		expectedDuration = new float[numAbilities];
		usesIcon = new bool[numAbilities];
		endPoints = new Queue<float>[numAbilities];
		abilityForBrick = new Ability[numAbilities];
		for (int i = 0; i < numAbilities; ++i) {
			time [i] = 0f;
			expectedDuration [i] = 0f;
			endPoints [i] = new Queue<float> ();
			abilityForBrick [i] = Store.abilities[Store.bricks [(BrickTypes)i].brickAbility];
			abilityForBrick [i].Initialize ();
			usesIcon[i] = abilityForBrick [i].sprite != null;
		}
		
	}

	void Die() {
		timeScale = 1f;
		for (int i = 0; i < numAbilities; ++i) {
			time [i] = 0f;
			endPoints [i].Clear ();
			expectedDuration[i] = 0f;
		}
		for (int i = 0; i < numAbilities; ++i)
			abilityForBrick [i].EndAbility ();
	}


		
	public void Collided (BrickTypes bt, int scoreToAdd, int earnings, Vector3 loc) {
		
		moneyManager.Pay (earnings, loc);
		scoreKeeper.IncrementScoreBy (scoreToAdd);

		GameObject g = ObjectPooler.SharedInstance.GetPooledObject ("BrickDeath");
		if (g==null) {
			Debug.LogError ("CollisionTracker::Collided g is null");
		} else {
			g.SetActive (true);
			g.transform.position = loc;
			g.GetComponent<BrickDeath> ().PlayDeath (bt);
		}


		Ability a = abilityForBrick [(int)bt];
		int t = (int)a.type;

		if (endPoints[t].Count == 0) {
			a.TriggerAbility ();
			time[t] = 0f;
			expectedDuration [t] = 0f;
			endPoints [t] = new Queue<float> ();
			if (usesIcon[t])
				displayer.DisplayAbility(a.type, a.sprite);
		} 
		expectedDuration [t] += a.duration [a.level];
		endPoints[t].Enqueue (time[t] + a.duration [a.level]);
	}

	public void SetTimeScale(float scale) {
		timeScale = scale;
	}


	void Update() {
		for (int i = 0; i < numAbilities; ++i) {
			if (usesIcon[i] && endPoints[i].Count > 0) {
                if (i == (int)AbilityTypes.Freeze && ObjectPooler.SharedInstance.GetAllActiveGameObjects("Brick").Count == 0)
                {
                    //end freeze early if there are no active bricks
                    time[i] = 0f;
                    endPoints[i].Clear();
                    expectedDuration[i] = 0f;
                    abilityForBrick[i].EndAbility();
                    displayer.Remove(AbilityTypes.Freeze);
                    continue;
                }

				Ability a = abilityForBrick[i];

				displayer.UpdateIconStatus (a.type, time[i] / expectedDuration[i] );
				if (endPoints[i].Count==0) {
					Debug.LogError ("CollisionTracker::Update: No endPoints for ability "+(a.type).ToString () );
				}
				while (endPoints[i].Count!=0 && time[i] > endPoints[i].Peek()) 
					endPoints[i].Dequeue ();
				
				if (endPoints[i].Count == 0) {
					a.EndAbility();
					displayer.Remove (a.type);
				} 
				time[i] += Time.deltaTime * timeScale;
			}
		}

	}
}
