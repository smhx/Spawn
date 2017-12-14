using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = System.Random;

public class BrickSpawner : MonoBehaviour {
    
    public int bricksPerSpawn;
    public float radius;
    public float spawnPeriod;
    public float anglePerShift;

    float timePassedInFrame = 0f;
    float borderRadius;

    List<Vector2> spawnPoints = new List<Vector2>();


    bool isPlaying = false;
    bool frozen = false;
    bool reviving = false;
    bool adReviving = false;

	public GameSceneManager gameManager;

    Random rnd = new Random();

    void OnEnable() {
        gameManager.OnPushBall += BallPushed;
        gameManager.OnGameEnd += Stop;
        gameManager.OnGamePause += () => isPlaying = false;
        gameManager.OnGameResume += () => isPlaying = true;
        gameManager.OnGameRevive += Revive;
		gameManager.OnDeathBeforeAd += AdDeath;
        borderRadius = gameManager.borderRadius;
    }

	void OnDisable() {
        gameManager.OnPushBall -= BallPushed;
        gameManager.OnGameEnd -= Stop;
        gameManager.OnGamePause -= () => isPlaying = false;
        gameManager.OnGameResume -= () => isPlaying = true;
        gameManager.OnGameRevive -= Revive;
		gameManager.OnDeathBeforeAd -= AdDeath;

    }

	void AdDeath() {
//		Debug.LogError("AD DEATH CALLED with isPlaying="+isPlaying+" revigin="+reviving+" adreviving="+adReviving);
		adReviving = true;

	}

	void Revive() {
//		Debug.LogError ("Revive called");
		reviving = true;
		timePassedInFrame = 0f;
	}
    
    void BallPushed() {
//		Debug.LogError ("Ball pushed with isPlaying="+isPlaying+" revigin="+reviving+" adreviving="+adReviving);
		bool justWatchedAd = false;
        if (reviving)
        {
            reviving = false;
			if (adReviving) {
				justWatchedAd = true;
				adReviving = false;
			}
            else
                return;
        }
        isPlaying = true;
        float anglePerSlice = 2 * Mathf.PI / bricksPerSpawn;
		spawnPoints = new List<Vector2> ();
        for (int i = 0; i < bricksPerSpawn; i++)
        {
            float angle = anglePerSlice * i;
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * borderRadius / 4;
            spawnPoints.Add(pos);
        }
		if (!justWatchedAd) CreateWave ();
    }


	void CreateWave () {
		foreach (Vector2 pos in spawnPoints) {
			GameObject brick = ObjectPooler.SharedInstance.GetPooledObject("Brick");
            if (brick != null) {
				brick.transform.position = pos;
				brick.SetActive(true);
			}
		}
        RotateSpawnPoints();
        timePassedInFrame = 0;
	}
    
    void Update() {
		if (!isPlaying || frozen || reviving || adReviving)
			return;
        // Spawn every spawnPeriod seconds.
        if (timePassedInFrame >= spawnPeriod) {
			CreateWave ();
        }
        timePassedInFrame += Time.deltaTime;
    }

	public void Stop() {
        isPlaying = false;
		frozen = false;
		reviving = false;
		adReviving = false;

		spawnPoints.Clear ();
	}

    public void Freeze() {
		frozen = true;
	}

	public void Unfreeze() {
		frozen = false;
	}

    void RotateSpawnPoints() {
        for (int i = 0; i < bricksPerSpawn; i++) {
            spawnPoints[i] = RotateVector(spawnPoints[i], anglePerShift);
            
        }
    }

    //rotates vector clockwise, angle in radians
    Vector2 RotateVector(Vector2 v, float angle) {
        float x = v.x;
        float y = v.y;
        v.x = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
        v.y = x * Mathf.Sin(angle) + y * Mathf.Cos(angle);
        return v;
    }

    public int RandomInt(int max) {
        return rnd.Next(max);
    }
}