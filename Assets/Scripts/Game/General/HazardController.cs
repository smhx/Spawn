using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField]
    float duration;
    [SerializeField]
    float countDownTime;
    [SerializeField]
    float warningTime;
    [SerializeField]
    GameSceneManager gm;
    [SerializeField]
    GameInitializer gameInit;

    bool playing = false;
    float time = 0f;
    bool hasWarned = false;

	bool onFirstLife = true;
    
    GameObject chosenBrick;
    HazardBrick hb;

    void OnEnable()
    {
		onFirstLife = true;

        gm.OnPushBall += Begin;
        gm.OnGameEnd += End;
        gm.OnGamePause += () => playing = false;
        gm.OnGameResume += () => playing = true;
        gm.OnGameRevive += () => playing = false;
		gm.OnDeathBeforeAd += AdDeath;

    }

    void OnDisable()
    {
        gm.OnPushBall -= Begin;
        gm.OnGameEnd -= End;
        gm.OnGamePause -= () => playing = false;
        gm.OnGameResume -= () => playing = true;
        gm.OnGameRevive -= () => playing = false;
		gm.OnDeathBeforeAd -= AdDeath;


    }

	void AdDeath() {
		onFirstLife = false;
		playing = false;
	}


    void Begin()
    {
        playing = true;
		if (onFirstLife) {
			time = 0f;
			hasWarned = false;
		}
    }

    void End()
    {
		onFirstLife = true;
        playing = false;
        time = 0f;
    }

    public void ResetTime() { //brick hit during warning
        time = 0f;
        hasWarned = false;
    }

    void Update()
    {
        if (!playing || gameInit.Mode() != GameModes.HAZARD)
            return;
        
        if (countDownTime - warningTime < time && time < countDownTime)
        {
            if (!hasWarned)
            {
                //choose brick
                chosenBrick = ObjectPooler.SharedInstance.GetRandomGameObject("Brick");
				if (chosenBrick == null) {//no active bricks
					Debug.LogWarning("chosenBrick is null in HazardController");
					time = 0f;
				}
				else {
					hb = chosenBrick.GetComponent<HazardBrick>();
					if (hb == null)
						Debug.LogError ("hb is null from get component!");
					else {
						//set warning
						hb.StartWarning();
						hasWarned = true;
					}
				}
               
            }
        }
        else if (time > countDownTime)
        {
            //make hazardous
            time = 0f;
            hasWarned = false;
			if (hb == null)
				Debug.LogError ("hb is null!");
            else hb.StartHazard(duration);
        }
        time += Time.deltaTime;
    }
}
