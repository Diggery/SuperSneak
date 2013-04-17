using UnityEngine;
using System.Collections;

public class BombThrower : MonoBehaviour {

	Transform selectedBomb;
	Transform rightHand;
	Vector3 bombDirection;
	public GameObject[] bombTypes;
	public PlayerAnimator playerAnimator;
	public PlayerController playerController;
	Transform bombTarget;
	
	float throwingDistance = 8;
	
	public void setUp (Transform rightHandObj, PlayerAnimator newPlayerAnimator, PlayerController newPlayerController, Transform bombTargetPrefab) {
		rightHand = rightHandObj;
		playerAnimator = newPlayerAnimator;
		playerController = newPlayerController;
		bombTarget = Instantiate(bombTargetPrefab, transform.position, Quaternion.identity) as Transform;
		//bombTarget.parent = transform;
	}
	
	void Update () {
		if (playerController.rightInputOn) {
			
			Vector3 input = playerController.currentRightInput * throwingDistance;
			Vector3 targetPosGoal = transform.position + Camera.main.transform.parent.TransformDirection(input);
			
			int layer1 = LayerMask.NameToLayer("PlayerRagDoll"); 
			int layer2 = LayerMask.NameToLayer("EnemyRagDoll"); 
			int layer3 = LayerMask.NameToLayer("Player"); 
			int layermask = ~((1 << layer1) | (1 << layer2) | (1 << layer3));
			
			RaycastHit hit;
			
			Vector3 startPos = transform.position + new Vector3(0.0f,0.25f,0.0f);
			Vector3 endPos = targetPosGoal + new Vector3(0.0f,0.25f,0.0f);
		
			if (Physics.Linecast (startPos, endPos, out hit, layermask)) {
				targetPosGoal = hit.point;
				targetPosGoal.y = 0.0f;
			}
			bombTarget.position = targetPosGoal;
		}
	}
	
	public void readyBomb() {
		GameObject bomb = Instantiate(bombTypes[0], rightHand.position, rightHand.rotation) as GameObject;
		selectedBomb = bomb.transform;
		selectedBomb.parent = rightHand;
		selectedBomb.localPosition = Vector3.zero;
	}

	public void throwBomb() {
		Vector3 direction;
		
		direction = Util.BallisticVel(rightHand.position, bombTarget.position);
		
		
		playerAnimator.playReadyBombAnim();
		selectedBomb.parent = null;
		selectedBomb.rigidbody.isKinematic = false;
		selectedBomb.rigidbody.useGravity = true;
		direction += new Vector3(0.0f, 0.5f, 0.0f);
		selectedBomb.rigidbody.AddForce(direction * 0.92f, ForceMode.Impulse);
		playerAnimator.playThrowBombAnim();
	}
}
