using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityDisplay : MonoBehaviour {
	// This is constant.
	[SerializeField] List<AbilityIcon> icons;

	[SerializeField] GameSceneManager gameManager;

	List<AbilityIcon> free;

	List<AbilityIcon> slots;

	static readonly int numAbilities = 11;

	int[] indxForAbility; // indxForAbility[abilityType as int] = the index of the slot representing abilityType.

	void OnEnable() {
		indxForAbility = new int[numAbilities];
        gameManager.OnPushBall += Reset;
		gameManager.OnGameEnd += Clear;
		gameManager.OnGamePause += Pause;
		gameManager.OnGameResume += Resume;
        gameManager.OnGameRevive += Clear;
		gameManager.OnDeathBeforeAd += Clear;
    }

	void OnDisable() {
		gameManager.OnPushBall -= Reset;
		gameManager.OnGameEnd -= Clear;
		gameManager.OnGamePause -= Pause;
		gameManager.OnGameResume -= Resume;
        gameManager.OnGameRevive -= Clear;
		gameManager.OnDeathBeforeAd -= Clear;

    }

	void Pause() {
		foreach (var slot in slots)
			slot.Pause ();
	}

	void Resume() {
		foreach (var slot in slots)
			slot.Resume ();
	}

	void Start() {
		free = new List<AbilityIcon> (icons);

		slots = new List<AbilityIcon> ();
		for (int i = 0; i < numAbilities; ++i) indxForAbility[i] = -1; // -1 for not set.
	}

	void Reset() {
		free = new List<AbilityIcon> (icons);
		slots = new List<AbilityIcon> ();
		for (int i = 0; i < numAbilities; ++i) indxForAbility[i] = -1; // -1 for not set.
	}

	void Clear() {
		foreach (var slot in slots) 
			slot.Kill ();
		Reset ();
	}
		

	public void Remove(AbilityTypes t) {

		int i = indxForAbility [(int)t];
		if (i < 0 || i >= slots.Count) {
			Debug.LogError ("AbilityDisplay::Remove(" + t.ToString () + ") has indxForAbility = " + i.ToString () +" and slots.Count is " + slots.Count.ToString () );
			return;
		}
		AbilityIcon icon = slots[i];
		icon.Fade ();
		slots.RemoveAt (i);
		free.Add (icon);
		indxForAbility [(int)t] = -1;
		for (int j = i; j < slots.Count; ++j) {
			slots [j].ShiftLeft ();
			--indxForAbility [(int)slots [j].Type ()];
		}
	}

	public void UpdateIconStatus(AbilityTypes t, float status) {
		if (slots.Count==0) {
			Debug.LogError ("AbilityDisplay::UpdateIconStatus("+t.ToString ()+", "+status.ToString ()+"): slots.Count==0");
			return;
		}
		if (indxForAbility [(int)t] >= slots.Count || indxForAbility [(int)t] < 0) {
			Debug.LogError ("AbilityDisplay::UpdateIconStatus for " + t.ToString () + " has index " + indxForAbility [(int)t].ToString ());
			return;
		}
 		slots [indxForAbility [(int)t]].SetFill (1-status);
	}

	public void DisplayAbility(AbilityTypes a, Sprite s) {

		if (free.Count == 0) {
			Debug.LogError ("AbilityDisplay::DisplayAbility: free is empty");
			return;
		}
		if (indxForAbility [(int)a] == -1) {
			AbilityIcon iconAdded = free[free.Count-1];
			free.RemoveAt (free.Count - 1);
			slots.Add (iconAdded);
			iconAdded.Begin (s, a, slots.Count-1);
			indxForAbility [(int)a] = slots.Count-1;
		} 


	}
}
