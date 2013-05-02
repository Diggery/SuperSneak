using UnityEngine;
using System.Collections;

public class AreaOfEffect : MonoBehaviour {

    void OnTriggerEnter(Collider collision) {
		if (collision.transform.root.tag.Equals("Enemy")) {
			collision.transform.root.SendMessage("gassed");
		}
	}
}
