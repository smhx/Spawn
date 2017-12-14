//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Instructions : MonoBehaviour {
//
//	enum InstructionStates {
//		PLAYING,
//		RESTING,
//		DONE
//	};
//
//	InstructionStates state;
//
//	public BallMovement ballMovement;
//	public PaddleMovement paddle;
//
//	public float closeDistance;
//
//	public float farDistance;
//
//	public GameSceneManager gameManager;
//
//	public float animationPeriod;
//
//	float animationParameter; // 0 for just starting, 1 for finished
//
//	public float timeBetweenAnimations;
//
//	float timeUntilNextAnimation;
//
//	void OnEnable() {
//		gameManager.OnPushBall += StopAnimating;
//		gameManager.OnGameStart += StartAnimating;
//	}
//
//	void OnDisable() {
//		gameManager.OnGameStart -= StartAnimating;
//		gameManager.OnPushBall -= StopAnimating;
//	}
//
//	void StartAnimating() {
//		gameObject.GetComponent <SpriteRenderer> ().enabled = true;
//		state = InstructionStates.PLAYING;
//		animationParameter = 0f;
//	}
//
//	void StopAnimating() {
//		state = InstructionStates.DONE;
//	}
//
//	void Rest() {
//		gameObject.GetComponent <SpriteRenderer> ().enabled = false;
//		state = InstructionStates.RESTING;
//		timeUntilNextAnimation = 0f;
//	}
//
//	void Update() {
//		if (state==InstructionStates.PLAYING) {
//			if (animationParameter >= 1f) {
//				Rest ();
//				return;
//			}
//			StickToPaddle ();
//			// Play the animation. 
//			transform.position = transform.position.normalized*HandDistance (animationParameter);
//			// Some sort of rotation?
//			animationParameter += Time.deltaTime / animationPeriod;
//
//		} else if (state==InstructionStates.RESTING) {
//			if (timeUntilNextAnimation >= timeBetweenAnimations) {
//				StartAnimating ();
//				return;
//			}
//			StickToPaddle ();
//			timeUntilNextAnimation += Time.deltaTime;
//		} else {
//			// done
//			gameObject.SetActive (false);
//		}
//	}
//
//	void StickToPaddle () {
//		transform.position = paddle.transform.position.normalized * closeDistance;
//		return;
//	}
//
//	float HandDistance(float t) {
//		return (farDistance - closeDistance) * t + closeDistance;
//	}
//}
