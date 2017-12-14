using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {

	[SerializeField] float shiftLifetime = 0.5f;
	[SerializeField] float width=250;
	[SerializeField] float yVal=-900;
	[SerializeField] float leftOffset=-500;
	[SerializeField] float fadeLifetime;
	[SerializeField] int numIcons;
	[SerializeField] int numIconsInFirstRow;
	[SerializeField] Image abilityImage;

	[SerializeField] AbilityDisplay displayer;

	List<Vector3> locations;

	Image timerImage;

	float fadeParam = -1f;

	int index;
	bool paused = false;

	const float eps = 0.001f;

	AbilityTypes type;

	// Public Functions

	public void Begin(Sprite s, AbilityTypes atype, int i) { // This function starts everything

		abilityImage.sprite = s;
		index = i;
		transform.localPosition = new Vector3(leftOffset + width*index, yVal, 0f);
        Brick b = Store.bricks[(BrickTypes)atype];
        abilityImage.color = b.storeColor;
		type = atype;
		if (timerImage==null) {
			Debug.LogError ("timerImage is null");
			return;
		}
        timerImage.color = b.storeColor;
	}

	public void SetFill(float fill) {
		timerImage.fillAmount = fill;
	}

	public void Kill() { 
		abilityImage.color = new Color(abilityImage.color.r, abilityImage.color.g, abilityImage.color.b, 0f);
		timerImage.color = new Color(timerImage.color.r, timerImage.color.g, timerImage.color.b, 0f);
		timerImage.fillAmount = 0f;
		index = 0;
	}
	public void Fade() {
		fadeParam = 0f;
	}
	public void ShiftLeft(){ 
		--index;
	} 

	public void Pause() {paused = true;}
	public void Resume() { paused=false;}
		
	public AbilityTypes Type() {return type;}

	void Awake() {
		timerImage = GetComponent <Image> ();
		abilityImage.color = new Color(abilityImage.color.r, abilityImage.color.g, abilityImage.color.b, 0f);
		timerImage.color = new Color(timerImage.color.r, timerImage.color.g, timerImage.color.b, 0f);
		fadeParam = -1f;
		locations = new List<Vector3> (numIcons);
		for (int i = 0; i < numIcons; ++i) {
			
			float x = i < numIconsInFirstRow ? leftOffset+i * width : leftOffset+(i - numIconsInFirstRow) * width;
			float y = (i < numIconsInFirstRow) ? yVal : yVal - width;
			locations.Add( new Vector3 (x, y, 0f) );
		}
		transform.localPosition = locations [0];
		index = 0;
    }



	void Update() {

		if (paused) return;
		Vector3 a = transform.localPosition, b = locations [index];
		if ( index < numIconsInFirstRow && (a.y < yVal || a.x > b.x) || index >= numIconsInFirstRow && a.x > b.x ) {
			transform.localPosition += (b-a).normalized * width / shiftLifetime * Time.deltaTime;
		}
			

		
		if (fadeParam >= 1f) {
			fadeParam = -1f;
			abilityImage.color = new Color(abilityImage.color.r, abilityImage.color.g, abilityImage.color.b, 0f);
			timerImage.color = new Color(timerImage.color.r, timerImage.color.g, timerImage.color.b, 0f);
		} else if (fadeParam > -1f){
			abilityImage.color = new Color(abilityImage.color.r, abilityImage.color.g, abilityImage.color.b, 1-fadeParam);
			timerImage.color = new Color(timerImage.color.r, timerImage.color.g, timerImage.color.b, 1-fadeParam);
			fadeParam += Time.deltaTime / fadeLifetime;
		}
			
	}

}
