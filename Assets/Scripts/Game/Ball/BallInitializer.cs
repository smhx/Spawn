using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInitializer : Initializer<PlayerItems> {

	Ball ball;

	[SerializeField] ParticleSystem trail;
	[SerializeField] ParticleSystem circle;


	protected override void ConfigureSettings (PlayerItems data)
	{
		ball = ((Store)setting).GetActiveBall();

		gameObject.GetComponent<SpriteRenderer>().color = ball.color;

		var c = trail.colorOverLifetime;
		c.color = ball.trailGradient;

		var c2 = circle.colorOverLifetime;
		c2.color = ball.gradient;
	}

}
