using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	CharacterController characterController;
	PlayerAnimator playerAnimator;
	
	public bool inputOn;

	float runSpeed = 4.0f;
	float walkSpeed = 2.0f;
	
	public Vector3 currentInput;
	public float currentSpeed;
	public Vector3 currentDirection;
	
	public float currentHealth;
	public float maxHealth;
	bool dead;
	
	Transform head;
	RagDollController ragDoll;

	public void setUp (Transform thiefHead, RagDollController thiefRagDoll) {
		//head = thiefHead;
		ragDoll = thiefRagDoll;
		characterController = GetComponent<CharacterController>();
		playerAnimator = GetComponent<PlayerAnimator>();

	}

	void Update () {
		if (dead) return;
		
		Vector3 moveVector = Vector3.zero;
		float movePower = currentInput.magnitude;
		
		if (!dead && movePower > 0.05f){
			currentDirection = Camera.main.transform.parent.TransformDirection(currentInput); 
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
		characterController.Move(moveVector * Time.deltaTime);
		
		float newSpeed = characterController.velocity.magnitude;
		
		currentSpeed = newSpeed;
		
		//regen health
		currentHealth = Mathf.Clamp(currentHealth + Time.deltaTime * 0.5f, 0, maxHealth);	
		
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
	
	public void setInputOn() {
		inputOn = true;
	}
	
	public void moveInput(Vector3 newInput) {
		currentInput = new Vector3(-newInput.x, newInput.z, newInput.y);
	}

	public void setInputOff() {
		inputOn = false;
	}
	

}
