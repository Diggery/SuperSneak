using UnityEngine;
using System.Collections;

public class BombThrower : MonoBehaviour {

	Transform selectedBomb;
	Transform rightHand;

	Vector3 bombDirection;
	public PlayerAnimator playerAnimator;
	public PlayerController playerController;
	BombTarget bombTarget;
		
	float throwingDistance = 8;
	
	public void SetUp (Transform rightHandObj, PlayerAnimator newPlayerAnimator, PlayerController newPlayerController, Transform bombTargetPrefab) {
		rightHand = rightHandObj;
		playerAnimator = newPlayerAnimator;
		playerController = newPlayerController;
		Transform bombTargetObj = Instantiate(bombTargetPrefab, transform.position, Quaternion.identity) as Transform;
		bombTarget = bombTargetObj.GetComponent<BombTarget>();
	}
	
	void Update () {
		if (playerController.rightInputOn) {
			if (playerController.currentRightInput.sqrMagnitude < 0.04f) {
				bombTarget.targetTooClose();
			} else {
				bombTarget.targetOn();
			}
			Vector3 input = playerController.currentRightInput * throwingDistance;
			Vector3 targetPosGoal = transform.position + Camera.main.transform.parent.TransformDirection(input);
			
			int layer1 = LayerMask.NameToLayer("PlayerRagDoll"); 
			int layer2 = LayerMask.NameToLayer("EnemyRagDoll"); 
			int layer3 = LayerMask.NameToLayer("Player"); 
			int layer4 = LayerMask.NameToLayer("AreaOfEffect"); 
			int layermask = ~((1 << layer1) | (1 << layer2) | (1 << layer3) | (1 << layer4));
			
			RaycastHit hit;
			
			Vector3 startPos = transform.position + new Vector3(0.0f,0.25f,0.0f);
			Vector3 endPos = targetPosGoal + new Vector3(0.0f,0.25f,0.0f);
		
			if (Physics.Linecast (startPos, endPos, out hit, layermask)) {
				targetPosGoal = hit.point;
				targetPosGoal.y = 0.0f;
			}
			
			bombTarget.transform.position = targetPosGoal;

		} else {
			bombTarget.targetOff();
		}
	}
	
	public void readyBomb() {
		GameObject bombPrefab = playerController.getBomb();
		if (!bombPrefab) return;
		GameObject bomb = Instantiate(bombPrefab, rightHand.position, rightHand.rotation) as GameObject;
		string bombType = bombPrefab.name;
		int breakIndex = bombType.IndexOf("_");
		bomb.name = bombType.Substring(breakIndex + 1);
		selectedBomb = bomb.transform;
		selectedBomb.parent = rightHand;
		selectedBomb.localPosition = Vector3.zero;
	}
	
	public Transform getBomb() {
		return selectedBomb;
	}
	
	public void putAwayBomb() {
		if (selectedBomb) {
			Destroy(selectedBomb.gameObject);
			playerController.addItem(selectedBomb.name);
		}
	}

	public void throwBomb() {
		
		if (!selectedBomb) return;
				
		Vector3 direction;
		
		direction = Util.BallisticVel(rightHand.position, bombTarget.transform.position);
		
		selectedBomb.parent = null;
		selectedBomb.rigidbody.isKinematic = false;
		selectedBomb.rigidbody.useGravity = true;
		direction += new Vector3(0.0f, 0.5f, 0.0f);
		selectedBomb.rigidbody.AddForce(direction * 0.92f, ForceMode.Impulse);
		playerController.doneThrowing();
	}
	
	public void resetBombTarget() {
		bombTarget.reset();
	}
}
