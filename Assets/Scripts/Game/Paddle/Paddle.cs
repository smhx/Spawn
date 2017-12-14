using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Paddle : ScriptableObject {

    public string paddleName;

	public Color color;

	public Gradient gradient;

    public string paddleDescription;

    public int price;

	public PaddleTypes paddleType;
}
