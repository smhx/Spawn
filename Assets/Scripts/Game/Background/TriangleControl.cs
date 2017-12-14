using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = System.Random;

public class TriangleControl : MonoBehaviour {

    Random rnd = new Random();

    public BackgroundSceneManager backgroundManager;
    
//    float transitionTime;
    float maxDelayTime;
    float transitionSpeed;

    float opacity; //0 is completely transparent, 1 is completely opaque
//    int index = 0;
//    float timePassed1;
//    float timePassed2;
    float delayTime;
    float delayTimeCounter = 0;
    Color colorToSet;
    bool isDelaying = false;
    float dist;
//    float timeCounter = 0;
    List<Color> colors;
    Color color;
    public Backgrounds background;

    SpriteRenderer sr;
    
    
    void Awake() {
//        backgroundManager.OnBackgroundChange += BackgroundChanged;
    }

    void Start() {
        sr = GetComponent<SpriteRenderer>();
//        timePassed1 = transitionTime;
//        timePassed2 = 0;
        dist = transform.position.magnitude;
        transitionSpeed = backgroundManager.triangles1TransitionSpeed;
        maxDelayTime = backgroundManager.triangles1MaxDelayTime;
    }

    void Update() {
        if (background == Backgrounds.TRIANGLES_1 || background == Backgrounds.TRIANGLES_2) {
            if (isDelaying) {
                if (delayTimeCounter > delayTime) {
                    SetRandomOpacity(colorToSet);
                    isDelaying = false;
                }
                delayTimeCounter += Time.deltaTime;
            }
        }
    }

    public void ChangeFromCenter(Color c) {
        colorToSet = c;
        isDelaying = true;
        delayTimeCounter = 0;
        delayTime = (float)rnd.NextDouble()*maxDelayTime + dist / transitionSpeed;
    }

    void SetRandomOpacity(Color c) {
        opacity = (float) rnd.NextDouble();
        c.a = opacity;
        sr.color = c;
    }
}
