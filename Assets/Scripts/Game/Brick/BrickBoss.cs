using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class BrickBoss : Initializer<PlayerItems> {

	[SerializeField] ParticleSystem standardPS;
	ParticleSystem.ColorOverLifetimeModule PScolor;

	List<BrickTypes> activeBrickTypes;

	GameInitializer gameInit;
    GameSceneManager gm;
	CollisionTracker tracker;
    SpriteRenderer sr;
	BrickSpawner spawner;

	BrickTypes brickType;
	Brick brick;
	int timesHit;
    float velocityOnFreeze;

	AudioSource deathAudio;


	int totalWeight;

    HazardBrick hb;
    HazardController hc;

    protected override void ConfigureSettings(PlayerItems dat) {
        activeBrickTypes = ((Store) setting).GetActiveBricks ();
		totalWeight = 0;
		foreach (var b in activeBrickTypes) {
            Ability a = Store.abilities[Store.bricks[b].brickAbility];
			totalWeight += a.probabilityWeight[a.level];
		}
	}
    
	void Awake() {
		PScolor = standardPS.colorOverLifetime;
		tracker = GameObject.FindWithTag ("CollisionTracker").GetComponent <CollisionTracker> ();
        sr = GetComponent<SpriteRenderer>();
		spawner = GameObject.FindWithTag ("BrickSpawner").GetComponent <BrickSpawner> ();
		gameInit = GameObject.FindWithTag ("GameInitializer").GetComponent <GameInitializer> ();
        hb = GetComponent<HazardBrick>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameSceneManager>();
        hc = GameObject.FindWithTag("HazardController").GetComponent<HazardController>();
    }

	protected override void OnEnable () {
		base.OnEnable ();
		timesHit = 0;
		SetRandomBrick ();
	}
	
	void SetRandomBrick() {
        int weightCounter = 0;
        int r = spawner.RandomInt(totalWeight);
		if (activeBrickTypes.Count == 0) {
			Debug.LogError ("activeBrickTypes.Count = " + activeBrickTypes.Count);
		}
        foreach (var b in activeBrickTypes) {
            Ability a = Store.abilities[Store.bricks[b].brickAbility];
            weightCounter += a.probabilityWeight[a.level];
            if (r < weightCounter) {
                brickType = b;
                brick = Store.bricks[brickType];
				if (brick == null)
					Debug.LogError ("brick is null brickType is " + brickType);
				
				PScolor.color = brick.gradient;

                sr.color = brick.color;
                sr.sprite = brick.sprite;
                break;
            }
        }
		if (brick == null)
			Debug.LogError ("BrickBoss::SetRandombrick brick is null. brickType is "+brickType+" and Store.bricks[brickType]="+Store.bricks[brickType] );
    }

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Ball"){
            Collided();
		}
	}
    
	void Collided() {
        if (gameInit.Mode() == GameModes.HAZARD && hb.CurrentHazard())
            gm.Kill();
        timesHit++;
        if (brick== null) {
			Debug.LogError("BrickBoss::Collided: brick is null");
			return;
		}
		if (timesHit >= brick.timesToHit) {
            if (gameInit.Mode() == GameModes.HAZARD && hb.CurrentWarning())
                hc.ResetTime();
            Kill ();
        } else {
			Audio.Instance.PlayBrickDamaged ();
		}
	}

	public void Kill() {
		if (gameObject.activeInHierarchy == false) {
			Debug.LogWarning ("BrickBoss::Kill brick with type "+ brickType.ToString ()+ " is not active");
			return;
		}

		gameObject.SetActive (false);

	
		tracker.Collided (brickType, brick.score, brick.earnings, transform.position);


	}


}


