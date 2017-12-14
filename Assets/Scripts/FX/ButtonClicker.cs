using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicker : MonoBehaviour {

	void Start() {
		GetComponent<Button> ().onClick.AddListener (Audio.Instance.PlayButtonClick);
	}
		
}
