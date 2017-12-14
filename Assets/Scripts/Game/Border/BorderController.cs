using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BorderController : MonoBehaviour
{
	
	[SerializeField] GameSceneManager gameManager;
	[SerializeField] Timer timer;
	[SerializeField] BallController ballControl;
	[SerializeField] SpriteRenderer ring;

    GameModes gameMode;


	bool shield;

	void OnEnable() {
		gameManager.OnGameStart += () => {
			shield = false;
        };
	}

	void OnDisable() {
		gameManager.OnGameStart -= () => {
			shield = false;
        };
	}



    public void SetMode(GameModes gm) {
        gameMode = gm;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ball") {
            if (shield)
                return;
            int numBalls = ballControl.GetActiveBalls().Count;
			if (numBalls > 1) {
				other.gameObject.SetActive (false);
            } else if (gameMode == GameModes.TIMED)
				timer.BallHitBorder();
			else if ( gameManager.Kill() ) 
                other.gameObject.SetActive(false);
    
            
        }
        if (other.gameObject.tag == "Brick") {
			if (gameMode == GameModes.TIMED) {
				if (!shield) {
					timer.BrickHitBorder();
				}
				other.gameObject.SetActive(false);
			} else {
				if (shield)
					other.gameObject.SetActive (false);
                else gameManager.Kill();
            }
            
        }
    }

    

	public void Shield() {
		shield = true;
		ring.enabled = true;
	}

	public void EndShield() { 
		shield = false; 
		ring.enabled = false;
	}
} 