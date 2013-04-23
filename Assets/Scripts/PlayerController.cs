using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	CharacterController characterController;
	PlayerAnimator playerAnimator;
	public BombThrower playerBombThrower;
	public UIInventory inventory;
	
	public bool leftInputOn;
	public Vector3 currentLeftInput;
	public bool rightInputOn;
	public Vector3 currentRightInput;

	float runSpeed = 4.0f;
	float walkSpeed = 2.0f;
	
	public float currentSpeed;
	public Vector3 currentDirection;
	
	public float currentHealth;
	public float maxHealth;
	bool dead;
	
	Transform head;
	RagDollController ragDoll;

	public void setUp (Transform thiefHead, RagDollController thiefRagDoll, BombThrower newBombThrower) {
		//head = thiefHead;
		ragDoll = thiefRagDoll;
		playerBombThrower = newBombThrower;
		characterController = GetComponent<CharacterController>();
		playerAnimator = GetComponent<PlayerAnimator>();
	}

	void Update () {
		if (dead) return;
		
		Vector3 moveVector = Vector3.zero;
		float movePower = currentLeftInput.magnitude;
		
		if (movePower > 0.05f){
			currentDirection = Camera.main.transform.parent.TransformDirection(currentLeftInput); 
			moveVector = currentDirection.normalized;
			if (movePower > 0.5) {
				moveVector *= runSpeed;
			} else {
				moveVector *= walkSpeed;
			}
			
		} else {
			moveVector = Vector3.zero;			
		}
		
    	moveVector.y = -1.0f;
		
		if (playerAnimator.isStopped()) moveVector = Vector3.zero;
		
		characterController.Move(moveVector * Time.deltaTime);
		
		float newSpeed = characterController.velocity.magnitude;
		
		currentSpeed = newSpeed;
		
		//regen health
		currentHealth = Mathf.Clamp(currentHealth + Time.deltaTime * 0.5f, 0, maxHealth);	
	}
	
	public void addExpDamage(Vector4 expData) {
		Vector3 expPos = new Vector3 (expData.x, expData.y, expData.z);
		addDamage(expData.w, expPos);
	}
	
	public void addDamage(float amount) {
		addDamage(amount, transform.position);
	}
	
	public void addDamage(float amount, Vector3 origin) {
		currentHealth -= amount;
		if (currentHealth < 0) die(origin);
	}
	
	public void die(Vector3 origin) {
		
		if (dead) return;
		
		dead = true;
		Vector3 deathForce = (transform.position - origin).normalized + new Vector3(0.0f, 0.5f, 0.0f);
		deathForce *= 100;
		ragDoll.enableRagDoll(deathForce);
		playerAnimator.die(origin);
		
		Destroy(GetComponent<CharacterController>());
		
	}
	
	public void setInventory(UIInventory newInventory) {
		inventory = newInventory;
	}
	
	public GameObject getBomb() {
		return inventory.getItem();
	}
	
	public void addBomb(string type) {
		inventory.addItem(type);
	}
	
	public void doneThrowing() {
		inventory.unlockList();
	}
	
	public void setLeftInputOn() {
		leftInputOn = true;
	}
	
	public void setRightInputOn() {
		rightInputOn = true;
		playerAnimator.playReadyBombAnim();
		playerBombThrower.resetBombTarget();
	}
		
	public void leftInput(Vector3 newInput) {
		currentLeftInput = new Vector3(-newInput.x, newInput.z, newInput.y);
	}
	
	public void rightInput(Vector3 newInput) {
		currentRightInput = new Vector3(-newInput.x, newInput.z, newInput.y);
	}

	public void setLeftInputOff() {
		leftInputOn = false;
	}
	
	public void setRightInputOff() {
		if (currentRightInput.sqrMagnitude < 0.04f || !playerBombThrower.getBomb()) {
			playerAnimator.playPutAwayBombAnim();
			return;
		}
		
		if (rightInputOn) {
			playerAnimator.playThrowBombAnim();
		}
		rightInputOn = false;
	}
	
	public void openCrate(Vector3 cratePos) {
		playerAnimator.playOpenCrateAnim(cratePos);
	}
}
