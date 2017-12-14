using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BackgroundImage : ScriptableObject {

    public Color color;

    public Sprite sprite;

	public List<BackgroundCircleTypes> incompatibleCircles;

    public int price;

    public BackgroundImageTypes backgroundImageType;
}
