using System;
using UnityEngine;
using System.Collections;

public class PaddleMovement : MonoBehaviour {

	[SerializeField] GameSceneManager gameManager;
    /*
	[SerializeField] float maxRotPerSecond; // in rotations per second
    [SerializeField] bool continuous;
    [SerializeField] float rotPerSecond;
    */
    [SerializeField] ParticleSystem trail;

    Vector2 center;

//  float rate;
//	float angle; // angle from straight up. positive is clockwise, - is counter in degrees

	Animator animator;
    
	bool playerCanPlay = false;
    bool gamePaused = false;
    float borderRadius;

	void Start() {
        borderRadius = gameManager.borderRadius;
		transform.position = new Vector3 (0, -borderRadius, 0);
//		rate = 0f;
//		angle = 0f;
	}

	void OnEnable(){
		animator = GetComponent <Animator> ();
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
	}

	void Die() {
		playerCanPlay = false;
		animator.SetBool ("dead", true);
        trail.Stop();
//        rate = 0f;
	}
    /*
    public void SetSpeed(float val) {
        if (continuous)
        {
            if (val < -1f || val > 1f)
            {
                Debug.LogWarning("PaddleMovement::SetRotation. val = " + val + " is out of range");
                val = val > 0 ? 1 : -1;
            }
            rate = val;
        }
        else
        {
            if (val > 0)
                rate = rotPerSecond;
            else if (val < 0)
                rate = -rotPerSecond;
            else
                rate = 0;
        }
	}

    float GetRate()
    {
        if (Input.GetMouseButton(0))
        {
            if (continuous)
                return maxRotPerSecond * (2f * Input.mousePosition.x / Screen.width - 1);
            else
            {
                if (Input.mousePosition.x > Screen.width / 2)
                    return rotPerSecond;
                else
                    return -rotPerSecond;
            }
        }
        else
            return 0;
    }
    */

    void Update(){
		if (!playerCanPlay || gamePaused)
			return;
        Vector2 dir = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        transform.position = dir.normalized * borderRadius;
        float angle = (float)Math.Atan(- dir.y / dir.x) * 180f / Mathf.PI;
        transform.rotation = Quaternion.Euler(0f, 0f, 180 + Mathf.Sign(dir.x) * 90 - angle);
        /*
        rate = GetRate();

		angle -= rate * 360f * Time.deltaTime;

		transform.position = new Vector3 (borderRadius * Mathf.Sin (angle * Mathf.PI / 180f), borderRadius * Mathf.Cos (angle * Mathf.PI / 180f), 0f);

		Quaternion rotation = Quaternion.Euler(0, 0, -angle);

		transform.rotation = rotation;
	    */
    }
}