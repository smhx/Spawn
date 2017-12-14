using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : MonoBehaviour {

	[SerializeField] Vector3 moneyOffset;
	[SerializeField] Vector3 scale;

	[SerializeField] float showDuration;

	[SerializeField] float totalDistance;

	[SerializeField] float moneyMidpoint;
	[SerializeField] float moneyEndpoint;

	[SerializeField] Color cheap = Color.yellow;
	[SerializeField] Color middle = new Color (244f / 255f, 111f / 255f, 110f / 255f);
	[SerializeField] Color expensive = Color.black;


	float animationParameter;

	Vector3 startLocation;

	TextMesh textMesh;

	Color initialColor;
	Color endColor;

	bool isShowing;

	void Awake() {
		textMesh = GetComponent<TextMesh> ();
		isShowing = false;
		animationParameter = 0f;
		startLocation = new Vector3 (0f, 0f, 0f);
		var mr = GetComponent<MeshRenderer> ();
		mr.sortingLayerName = "foreground";
		mr.sortingOrder = 100;
	}

	void Update() {
		
		if (!isShowing) 
			return;

		transform.position = moveFunction (animationParameter);
		textMesh.color = Color.Lerp (initialColor, endColor, animationParameter);
		animationParameter += Time.deltaTime / showDuration;
		if (animationParameter >= 1) {
			animationParameter = 0f;
			gameObject.SetActive (false);
		}
	}

	public void ShowMoney(int m, Vector3 loc) {
		textMesh.text = "+" + m.ToString ();
		transform.localScale = scale;
		transform.position = loc+moneyOffset;
		startLocation = transform.position;
		animationParameter = 0f;
		isShowing = true;
		if (m < moneyMidpoint)
			initialColor = Color.Lerp (cheap, middle, m / moneyMidpoint);
		else
			initialColor = Color.Lerp (middle, expensive, (m - moneyMidpoint) / (moneyEndpoint - moneyMidpoint));
		endColor = new Color (initialColor.r, initialColor.g, initialColor.b, 0);
	}

	Vector3 moveFunction(float t) {
		return startLocation + new Vector3 (0f, 1f, 0f) * (totalDistance) * t;
	}


}
