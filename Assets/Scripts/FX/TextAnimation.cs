using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour {

	public float bounceTime;
	Text text;
	public int bigFontSize;
	public int normalFontSize;


	bool shouldBounce = false;

	float bouncePoint = 0;

	void Start () {
		shouldBounce = false;
		text = gameObject.GetComponent<Text> ();
        transform.localScale = new Vector3(1, 1, 1) * normalFontSize / bigFontSize;
        text.fontSize = bigFontSize;
	}

	public void Bounce() {
		shouldBounce = true;
	}

	void Update () {
		if (shouldBounce) {
			bouncePoint += Time.deltaTime / bounceTime;
		
			if (bouncePoint >= 1){
				bouncePoint = 0;
				shouldBounce = false;
			}
				
			transform.localScale = new Vector3(1, 1, 1) * normalFontSize / bigFontSize * bounceScaleFunction (bouncePoint);
		}
	}

	float bounceScaleFunction(float t) {
		// 0 <= t <= 1
		float amp = bigFontSize / normalFontSize;
		return 1 + amp * t * (1-t);
	}
}
