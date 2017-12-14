using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {
	[SerializeField] Sprite onSprite;
	[SerializeField] Sprite offSprite;

	[SerializeField] Image image;

	void Start () {
		image.sprite = Audio.Instance.volumeOn ? onSprite : offSprite;
	}

	public void Toggle() {
		Audio.Instance.Toggle ();
		image.sprite = Audio.Instance.volumeOn ? onSprite : offSprite;
	}

}
