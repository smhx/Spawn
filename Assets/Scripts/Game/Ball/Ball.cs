using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Ball : ScriptableObject {

    public string ballName;

    public Color storeColor;

	public Color color;

	public Gradient gradient;

    public Gradient trailGradient;

    public Gradient ballGradient;

	public int price;

	public BallTypes ballType;

}
