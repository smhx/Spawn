using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMovement : MonoBehaviour {
	[SerializeField] float brickSpeed=1;
	float radius;
	float time;
	Vector2 unitVector;
	GameSceneManager gameManager;
    DifficultyInitializer diffInit;
    Difficulty difficulty;

	bool canMove = false;

    void Awake () {
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameSceneManager>();
        diffInit = GameObject.FindWithTag("DifficultyInitializer").GetComponent<DifficultyInitializer>();
        canMove = false;
		gameObject.SetActive (false);
    }

	void OnEnable () {
        canMove = true;
        gameManager.OnGameEnd += Reset;
        gameManager.OnPushBall += () => canMove = true;
        gameManager.OnGamePause += () => canMove = false;
        gameManager.OnGameResume += () => canMove = true;
        gameManager.OnGameRevive += () => canMove = false;
		gameManager.OnDeathBeforeAd += () => canMove = false ;
        radius = transform.position.magnitude;
		time = 0;
		unitVector = (new Vector2(transform.position.x, transform.position.y)).normalized;
        difficulty = diffInit.GetDifficulty();
        brickSpeed = difficulty.brickSpeed;
    }

	void OnDisable () {
		gameManager.OnGameEnd -= Reset;
        gameManager.OnPushBall -= () => canMove = true;
        gameManager.OnGamePause -= () => canMove = false;
        gameManager.OnGameResume -= () => canMove = true;
        gameManager.OnGameRevive -= () => canMove = false;
		gameManager.OnDeathBeforeAd -= () => canMove = false ;
        canMove = false;
	}

	void Reset () {
		transform.position = unitVector * radius;
		gameObject.SetActive (false);
	}
		
	void Update () {
        if (!canMove)
            return;
		float mag = MoveFunction (time) * radius;
		transform.position = unitVector * mag;
		time += Time.deltaTime;
	}

	float MoveFunction(float x) {
        return brickSpeed*Mathf.Sqrt(x+1);
	}

    public void Freeze() {
        canMove = false;
    }

    public void Unfreeze() {
        canMove = true;
    }
}
