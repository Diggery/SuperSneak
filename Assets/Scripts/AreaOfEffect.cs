using UnityEngine;
using System.Collections;

public class AreaOfEffect : MonoBehaviour {
	
	string effect = "None";

    void OnTriggerEnter(Collider collision) {
		if (collision.transform.root.tag.Equals("Enemy")) {
			collision.transform.root.SendMessage(effect);
		}
	}
	
	public void SetEffect(string newEffect) {
		effect = newEffect;
	}
}
