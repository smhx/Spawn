using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaddleJoystick : MonoBehaviour {

	[SerializeField] GameSceneManager gameManager;

    [SerializeField] ParticleSystem trail;

    [SerializeField] float dotX;
	[SerializeField] float dotY;

	Vector2 center;

	Animator animator;

	bool playerCanPlay = false;
	bool gamePaused = false;
	float borderRadius;
    bool dotClicked = false;

	void Start() {
		borderRadius = gameManager.borderRadius;
		transform.position = new Vector3 (0, -borderRadius, 0);

        center = new Vector2(Screen.width * 0.7f, Screen.height * 0.15f);
    }

	void OnEnable()
    {
        animator = GetComponent<Animator>();
        Play();
		gameManager.OnGameStart += Play;
		gameManager.OnGameEnd += Die;
		gameManager.OnGamePause += () => gamePaused = true;
		gameManager.OnGameResume += () => gamePaused = false;
		gameManager.OnGameRevive += Play;
	}

	void OnDisable(){
		gameManager.OnGameStart -= Play;
		gameManager.OnGameEnd -= Die;
        gameManager.OnGamePause -= () => gamePaused = true;
		gameManager.OnGameResume -= () => gamePaused = false;
		gameManager.OnGameRevive -= Play;
	}

	void Play() {
		playerCanPlay = true;
		animator.SetBool ("dead", false);
        SetAngle(Mathf.PI);
        trail.Play();
    }

	void Die() {
		playerCanPlay = false;
        trail.Stop();
		animator.SetBool ("dead", true);
	}
		
	void SetAngle(float rad) {
		transform.position = new Vector3 (borderRadius * Mathf.Sin (rad), borderRadius * Mathf.Cos (rad), 0f);

		Quaternion rotation = Quaternion.Euler(0, 0, -rad*180f/Mathf.PI);

		transform.rotation = rotation;
	}



	void Update(){
		if (!playerCanPlay || gamePaused || dotClicked)
			return;
		Vector2 inputPos;
		if (Input.touchSupported && Input.touchCount > 0) {
            inputPos = Input.GetTouch(0).position;
        } else {
            inputPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        }

		float deg = Vector2.Angle(Vector2.up, inputPos-center);
		if (inputPos.x < center.x)
			deg *= -1;
		
		SetAngle (deg * Mathf.PI / 180f);
	}

    public void DotClicked() {
        if (Input.touchSupported)
            StartCoroutine(StopPaddle());
    }
    IEnumerator StopPaddle() {
        dotClicked = true;
        yield return new WaitForSeconds(0.1f);
        dotClicked = false;
    }

    
}