using UnityEngine;
using System.Collections;

public class ParticleKiller : MonoBehaviour {

	void Update () {
		if (!particleSystem.IsAlive()) {
			Destroy (gameObject);    
	    }	
	}
}
