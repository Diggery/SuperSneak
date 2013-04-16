using UnityEngine;
using System.Collections;

public class BombThrower : MonoBehaviour {

	Transform selectedBomb;
	Transform rightHand;
	public GameObject[] bombTypes;
	PlayerAnimator playerAnimator;
	
	public void setUp (Transform rightHandObj, PlayerAnimator newPlayerAnimator) {
		rightHand = rightHandObj;
		playerAnimator = newPlayerAnimator;
	}
	
	void Update () {
	
		}
	
	public void readyBomb() {
		GameObject bomb = Instantiate(bombTypes[0], rightHand.position, rightHand.rotation) as GameObject;
		print (bomb.transform.name);
		selectedBomb = bomb.transform;
		selectedBomb.parent = rightHand;
		selectedBomb.localPosition = Vector3.zero;
		playerAnimator.playReadyBombAnim();
	}
	
	public void throwBomb(Vector3 direction) {
		playerAnimator.playReadyBombAnim();
		selectedBomb.parent = null;
		selectedBomb.rigidbody.isKinematic = false;
		selectedBomb.rigidbody.useGravity = true;
		selectedBomb.GetComponent<SphereCollider>().enabled = true;
		selectedBomb.rigidbody.AddForce(direction * 50, ForceMode.Impulse);
		playerAnimator.playThrowBombAnim();
	}
}
