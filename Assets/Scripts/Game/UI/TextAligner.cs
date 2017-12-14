using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAligner : MonoBehaviour {

	[SerializeField] RectTransform iconRT;
	[SerializeField] float rightAlign;
	[SerializeField] float avgWidthPerChar;
	[SerializeField] float padding;
	[SerializeField] float iconWidth;
	[SerializeField] Text text;
	[SerializeField] RectTransform textRT;

	int lastTextLen = -1; 

	void Update() {
		if (text.text.Length != lastTextLen) {
			lastTextLen = text.text.Length;
			float w = lastTextLen * avgWidthPerChar + padding;
			textRT.anchorMin = new Vector2 (rightAlign-w, textRT.anchorMin.y);
			iconRT.anchorMax = new Vector2 (rightAlign-w, iconRT.anchorMax.y);
			iconRT.anchorMin = new Vector2 (rightAlign-w-iconWidth, iconRT.anchorMin.y);
		}
	}
}
