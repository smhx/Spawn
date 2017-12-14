using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundInitializer : Initializer<PlayerItems> {

    public Sprite emptyCircle;
    public Sprite filledCircle;

	public Image dot;

    public GameObject circle;

	public Vector3 emptyCircleScale = new Vector3(1.26f, 1.26f, 1f);
	public Vector3 filledCircleScale = new Vector3(1.18f, 1.18f, 1f);

	public Image reviverImage;

    Background background;

    SpriteRenderer imageSR;
    SpriteRenderer circleSR;

    void Awake() {
        imageSR = gameObject.GetComponent<SpriteRenderer>();
        circleSR = circle.GetComponent<SpriteRenderer>();
    }

    protected override void ConfigureSettings(PlayerItems data) {
        background = ((Store)setting).GetActiveBackground();

        imageSR.sprite = Store.backgroundImages[background.image].sprite;
        imageSR.color = Store.backgroundImages[background.image].color;

        if (Store.backgroundCircles[background.circle].isEmpty) {
            circleSR.sprite = emptyCircle;
			circle.transform.localScale = emptyCircleScale;		
			reviverImage.color = circleSR.color;
        }
        else {
            circleSR.sprite = filledCircle;
			circle.transform.localScale = filledCircleScale;
			reviverImage.color = imageSR.color;

        }
        circleSR.color = Store.backgroundCircles[background.circle].color;
		dot.color = circleSR.color;


    }
}
