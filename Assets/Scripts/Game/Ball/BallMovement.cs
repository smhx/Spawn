using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{

    enum BallStates
    {
        DRAGGING,
        STICKING,
        PLAYING
    };

	[SerializeField] ParticleSystem trail;
	[SerializeField] ParticleSystem circle;

    Rigidbody2D rb;

    GameObject paddle;

    GameSceneManager gameManager;
    Timer timer;
    BallController ballControl;

	SpriteRenderer sr;

    BallStates ballState = BallStates.STICKING;

    public float distanceToCenter = 2f;

    bool gamePaused = false;
    float speed;
    float collisionWaitTime = 0;

    Vector2 velocityOnPause;

    void OnEnable()
    {
		if (gameManager==null) 
			gameManager = GameObject.FindWithTag ("GameManager").GetComponent<GameSceneManager> ();
		if (paddle==null) 
			paddle =  GameObject.FindWithTag ("Paddle");
		if (timer==null) {
			GameObject go = GameObject.FindWithTag ("Timer");
			if (go != null)
				timer = go.GetComponent<Timer> ();
		}
        if (ballControl == null)
            ballControl = GameObject.FindWithTag("BallController").GetComponent<BallController>();
        gameManager.OnGameStart += Play;
        gameManager.OnGameEnd += EndGame;
        gameManager.OnGamePause += Pause;
        gameManager.OnGameResume += Resume;
        gameManager.OnPushBall += PushBall;
        gameManager.OnGameRevive += Revive;
		gameManager.OnDeathBeforeAd += DeathBeforeAd;
        rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer> ();
        ballControl.AddBall(this.gameObject);
		sr.enabled = true;
    }

    void OnDisable()
    {
        gameManager.OnGameStart -= Play;
        gameManager.OnGameEnd -= EndGame;
        gameManager.OnGamePause -= Pause;
        gameManager.OnGameResume -= Resume;
        gameManager.OnPushBall -= PushBall;
        gameManager.OnGameRevive -= Revive;
		gameManager.OnDeathBeforeAd -= DeathBeforeAd;

        ballControl.RemoveBall(this.gameObject);
		circle.Stop ();
    }

	void Revive() {
		ballState = BallStates.STICKING;
		trail.Play ();
		circle.Play ();
		sr.enabled = true;
	}

	void DeathBeforeAd() {
		transform.position = new Vector3(0, 0, 0);
		rb.velocity = new Vector2(0, 0);
		ballState = BallStates.STICKING;
	}

    void EndGame()
    {
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = new Vector2(0, 0);
		sr.enabled = false;
		trail.Stop ();
		circle.Stop ();
        ballState = BallStates.STICKING;
    }

    void Start()
    {
        Vector2 PaddlePos = paddle.transform.position;
        transform.position = (PaddlePos.normalized * distanceToCenter);
    }

    public void Play()
    {
        ballState = BallStates.STICKING;
    }

    void Update()
    {
        if (ballState == BallStates.STICKING)
        {
            StickToPaddle();
            return;
        }

        if (gamePaused)
            return;
        speed = ballControl.GetBallSpeed();
        rb.velocity = rb.velocity.normalized * speed;
		float a = Vector2.Angle (Vector2.up, rb.velocity);
		if (rb.velocity.x > 0f)
			a *= -1;
		transform.rotation = Quaternion.Euler (0f, 0f, a);
        if (collisionWaitTime > 0)
            collisionWaitTime -= Time.deltaTime;
    }

    public void PushBall()
    {
        speed = ballControl.GetBallSpeed();
        ballState = BallStates.PLAYING;
        rb.velocity = -rb.position.normalized * speed;
    }

    float scale(float t)
    {
        return (1 - Mathf.Exp(-1 * t));
    }

    void StickToPaddle()
    {
        transform.position = paddle.transform.position.normalized * distanceToCenter;
		transform.rotation = paddle.transform.rotation;
    }

    void Pause()
    {
        gamePaused = true;
        velocityOnPause = rb.velocity;
        rb.velocity = new Vector2(0, 0);
    }

    void Resume()
    {
        gamePaused = false;
        rb.velocity = velocityOnPause;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Paddle")
        {
            if (collisionWaitTime > 0)
                return;
            float pAngle = getAngle(paddle.transform.position);
            float bAngle = getAngle(transform.position);
            float newDir = 180 - 3.5f * (bAngle - pAngle) + pAngle; //the constant 3.5 can be adjusted
            newDir = newDir * Mathf.PI / 180f;
            rb.velocity = new Vector2(speed * Mathf.Sin(newDir), speed * Mathf.Cos(newDir));
            collisionWaitTime = 0.1f; //ensures a gap of 0.1 seconds between collisions
        }
    }

    float getAngle(Vector3 pos) {
        float angle = Mathf.Atan2(pos.x, pos.y);
        angle = angle * 180 / Mathf.PI;
        return angle;
    }
}