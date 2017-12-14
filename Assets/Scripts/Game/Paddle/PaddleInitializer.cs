using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleInitializer : Initializer<PlayerItems> {

	Paddle paddle;

	[SerializeField] ParticleSystem ps;



	protected override void ConfigureSettings (PlayerItems data)
	{
		paddle = ((Store)setting).GetActivePaddle();

		gameObject.GetComponent<SpriteRenderer>().color = paddle.color;

		var c = ps.colorOverLifetime;
		c.color = paddle.gradient;

	}

}
