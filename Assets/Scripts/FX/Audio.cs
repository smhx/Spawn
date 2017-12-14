using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

	[SerializeField] float clockNormalPitch;
	[SerializeField] float clockSlowPitch;

	AudioSource errorAudio;
	AudioSource boughtAudio;
	AudioSource brickDeath;
	AudioSource brickDamaged;
	AudioSource timePenalty;
	AudioSource buttonClick;
	AudioSource newHighScore;
	AudioSource clock;
	AudioSource revive;
	AudioSource warning;
	AudioSource hazard;

	public static Audio Instance;

	public bool volumeOn = true;

	void Awake() {
		if (Instance==null) {
			Instance = this;
		} else {
			Destroy (gameObject);
		}
	}


	void Start() {
		if (PlayerPrefs.HasKey ("volume") )
			volumeOn = PlayerPrefs.GetInt ("volume")==0 ? false : true;
		else {
			volumeOn = true;
			PlayerPrefs.SetInt ("volume", 1);
			PlayerPrefs.Save ();
		}
		errorAudio = transform.FindChild ("Error").GetComponent<AudioSource>();
		boughtAudio = transform.FindChild ("Bought").GetComponent <AudioSource> ();
		brickDeath = transform.FindChild ("BrickDeath").GetComponent <AudioSource> ();
		brickDamaged = transform.FindChild ("BrickDamaged").GetComponent <AudioSource> ();
		timePenalty = transform.FindChild ("TimePenalty").GetComponent <AudioSource> ();
		buttonClick = transform.FindChild ("ButtonClick").GetComponent <AudioSource> ();
		newHighScore = transform.FindChild ("NewHighScore").GetComponent <AudioSource> ();
		clock = transform.FindChild ("Clock").GetComponent <AudioSource> ();
		revive = transform.FindChild ("Revive").GetComponent <AudioSource> ();
		warning = transform.FindChild ("Warning").GetComponent <AudioSource> ();
		hazard = transform.FindChild ("Hazard").GetComponent <AudioSource> ();
	}

	public void Toggle() {
		volumeOn = !volumeOn;
		PlayerPrefs.SetInt ("volume", volumeOn ? 1 : 0);
		PlayerPrefs.Save ();
	}

	public void PlayError() {
		if (volumeOn)
			errorAudio.Play ();
	}

	public void PlayBought() {
		if (volumeOn)
			boughtAudio.Play ();
	}

	public void PlayBrickDeath() {
		if (volumeOn)
			brickDeath.Play ();
	}

	public void PlayBrickDamaged() {
		if (volumeOn)
			brickDamaged.Play ();
	}

	public void PlayTimePenalty() {
		if (volumeOn) {
			timePenalty.Play ();

		}
	}

	public void PlayButtonClick() {
		if (volumeOn)
			buttonClick.Play ();
	}

	public void PlayNewHighScore() {
		if (volumeOn)
			newHighScore.Play ();
	}

	public void PlayClockSlow() {
		if (volumeOn) {
			clock.pitch = clockSlowPitch;
			clock.Play ();
		}
	}

	public void StopClock() {
		if (volumeOn)
			clock.Stop ();
	}

	public void PlayClock() {
		if (volumeOn) {
			clock.pitch = clockNormalPitch;
			clock.Play ();
		}
	}

	public void PlayRevive() {
		if (volumeOn)
			revive.Play ();
	}

	public void PlayWarning() {
		if (volumeOn)
			warning.Play ();
	}

	public void PlayHazard() {
		if (volumeOn)
			hazard.Play ();
	}


}
