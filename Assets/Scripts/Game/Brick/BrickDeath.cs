using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickDeath : MonoBehaviour {

	[SerializeField] ParticleSystem standardDeath;

	public void PlayDeath(BrickTypes bt) {
		
		standardDeath.Play ();
		Audio.Instance.PlayBrickDeath ();
	}

}
