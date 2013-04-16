using UnityEngine;
using System.Collections;

public class ExplosionCatcher : MonoBehaviour {

	public void addDamage(float amount) {
		transform.root.gameObject.SendMessage("addDamage", amount);	
	}
}
