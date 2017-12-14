using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = System.Random;

[System.Serializable]
public enum Backgrounds {
    STRIPS_1 = 0,
    TRIANGLES_1,
    TRIANGLES_2
};

public class BackgroundSceneManager : MonoBehaviour {

    Random rnd = new Random();
    Backgrounds background;

    public List<Color> strips1Colors;
    public float strips1TransitionTime;
    public float strips1TimeLagFactor;

    public List<Color> triangleColors;
    public float triangles1TransitionTime;
    public float triangles1MaxDelayTime;
    public float triangles1TransitionSpeed;

    GameObject[] strips;
    GameObject[] thinStrips;
    GameObject[] smallTriangles;

    List<TriangleControl> smallTriangleControls;

    Dictionary<Backgrounds, List<Color>> colorLists = new Dictionary<Backgrounds, List<Color>>();

    public event Action OnBackgroundChange;

    void Start() {
        colorLists.Add(Backgrounds.STRIPS_1, strips1Colors);
//        colorLists.Add(Backgrounds.TRIANGLES_1, triangles1Colors);

        smallTriangles = GameObject.FindGameObjectsWithTag("Small Triangle");

		smallTriangleControls = new List<TriangleControl> ();
        foreach (var triangle in smallTriangles) {
            smallTriangleControls.Add(triangle.GetComponent<TriangleControl>());
        }
        if (smallTriangleControls == null) { Debug.Log("Small triangle controls is null"); }
    }

    public Backgrounds GetBackground() { return background; }

    public void SetBackground(Backgrounds b) {
        background = b;
        if (background == Backgrounds.TRIANGLES_2) {
            SetAllActive(smallTriangles, true);
            int t2Index = rnd.Next(triangleColors.Count);
            Color t2Color = triangleColors[t2Index];
            foreach (TriangleControl tc in smallTriangleControls) {
                tc.background = background;
                tc.ChangeFromCenter(t2Color);
            }
        }
        else {
            SetAllActive(smallTriangles, false);
        }
        if (OnBackgroundChange != null)
            OnBackgroundChange();
        else { Debug.Log("ERROR: OnBackgroundChange is null"); }
    }

    void SetAllActive(GameObject[] list, bool b) {
        foreach (GameObject g in list) {
            g.SetActive(b);
        }
    }

    public List<Color> GetColorList() { return colorLists[background]; }
}
