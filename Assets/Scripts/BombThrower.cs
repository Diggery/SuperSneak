using UnityEngine;
using System.Collections;

public class BombThrower : MonoBehaviour {

	Transform selectedBomb;
	Transform rightHand;
	
	public void setUp (Transform rightHandObj) {
		rightHand = rightHandObj;
	}
	
	void Update () {
	
	}
	
	void readyBomb(Transform bombType) {
		selectedBomb = Instantiate(bombType, rightHand.position, rightHand.rotation) as Transform;
		selectedBomb.parent = rightHand;
	}
	
	void throwBomb(Transform selectedBomb, Vector3 direction) {
		selectedBomb.parent = null;
		selectedBomb.rigidbody.AddForce(direction * 100, ForceMode.Impulse);
	}
}
