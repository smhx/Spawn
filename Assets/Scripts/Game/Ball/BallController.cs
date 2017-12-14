using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	[SerializeField] GameSceneManager gameManager;

    List<GameObject> activeBalls = new List<GameObject>();

    float initialBallSpeed;
    float ballAcceleration;
    float timePassed = 0;
    float scale = 1;

    bool playing = false;

    void Update()
    {
        if (playing) {
            timePassed += Time.deltaTime;
        }
    }

    void OnEnable()
    {
        gameManager.OnGameStart += () => {
            StartCoroutine(NewGame());
        };
        gameManager.OnPushBall += () => {
            playing = true;
        };
        gameManager.OnGameEnd += () => {
            playing = false;
        };
        gameManager.OnGamePause += () => {
            playing = false;
        };
        gameManager.OnGameResume += () => {
            playing = true;
        };
    }

    void OnDisable()
    {
        gameManager.OnGameStart -= () => {
            StartCoroutine(NewGame());
        };
        gameManager.OnPushBall -= () => {
            playing = true;
        };
        gameManager.OnGameEnd -= () => {
            playing = false;
        };
        gameManager.OnGamePause -= () => {
            playing = false;
        };
        gameManager.OnGameResume -= () => {
            playing = true;
        };
    }

    IEnumerator NewGame()
    {
		playing = false;
        timePassed = 0;
        while (activeBalls.Count > 0)
            activeBalls[0].SetActive(false);
        
        yield return new WaitUntil(() => ObjectPooler.SharedInstance != null);
        GameObject newBall = ObjectPooler.SharedInstance.GetPooledObject("Ball");
        newBall.SetActive(true);
        newBall.GetComponent<BallMovement>().Play();
    }

    public void AddBall(GameObject ball) { activeBalls.Add(ball); }

    public void RemoveBall(GameObject ball) { activeBalls.Remove(ball); }

    public List<GameObject> GetActiveBalls() { return activeBalls; }

    public float GetBallSpeed() { return SpeedFunction(initialBallSpeed, ballAcceleration, timePassed, scale); }

    float SpeedFunction(float v, float a, float t, float s) { return (v + a * t) * s; }

    public void InitializeBallSpeed(float v)
    {
        initialBallSpeed = v;
    }

    public void InitializeBallAcceleration(float a) { ballAcceleration = a; }

    public void SetBallSpeedScale(float _scale)
    {
        scale = _scale;
    }
}
