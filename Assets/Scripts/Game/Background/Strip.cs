using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strip : MonoBehaviour {

    public BackgroundSceneManager backgroundManager;
    
    public float transitionTime;
    public int stripIndex;
    public float timeLagFactor;

    Backgrounds background;
    float opacity; //0 is completely transparent, 1 is completely opaque
    float timeLag;
    List<Color> colors;
    Color color;

    SpriteRenderer sr;
    
    void Awake() {
        backgroundManager.OnBackgroundChange += BackgroundChanged;
    }

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        opacity = stripIndex * 0.1f;
        timeLag = stripIndex * timeLagFactor;
//        SetColor();
    }

    void Update() {
        SetColor();
    }

    void SetColor() {
        if (colors == null)
            return;
        float t = ((Time.time + timeLag) / transitionTime) % colors.Count;
        int index = Mathf.FloorToInt(t);
        float weight = t - index;
        Color c1 = colors[index % colors.Count];
        Color c2;
        if (index == colors.Count - 1) { c2 = colors[0]; }
        else { c2 = colors[index + 1]; }
        color = Color.Lerp(c1, c2, weight);
        color.a = opacity;
        sr.color = color;
    }

    void BackgroundChanged() {
        background = backgroundManager.GetBackground();
        if (background == Backgrounds.STRIPS_1) {
            gameObject.SetActive(true);
            colors = backgroundManager.strips1Colors;
            transitionTime = backgroundManager.strips1TransitionTime;
            timeLagFactor = backgroundManager.strips1TimeLagFactor;
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
