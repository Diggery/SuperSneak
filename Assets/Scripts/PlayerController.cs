using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	CharacterController characterController;
	PlayerAnimator playerAnimator;
	
	public bool inputOn;

	float moveSpeed = 4.0f;
	float gravity = 20.0f;
	
	public Vector3 currentInput;
	public float currentSpeed;
	public Vector3 currentDirection;
	
	public float currentHealth;
	public float maxHealth;
	bool dead;

	void Start () {
		characterController = GetComponent<CharacterController>();
		playerAnimator = GetComponent<PlayerAnimator>();

	}
	
	void Update () {
	    if (characterController.isGrounded && !dead) {
			currentDirection = Camera.main.transform.parent.TransformDirection(currentInput); 
	        currentDirection *= moveSpeed;
	    } else {
			currentDirection = Vector3.zero;
		}

    	currentDirection.y -= gravity * Time.deltaTime;
    	characterController.Move(currentDirection * Time.deltaTime);
		
		
		float newSpeed = characterController.velocity.magnitude;
		
		if (Mathf.Abs(newSpeed - currentSpeed) < 1.0f) currentSpeed = newSpeed;
			

		
		//regen health
		currentHealth = Mathf.Clamp(currentHealth + Time.deltaTime * 0.5f, 0, maxHealth);	
		
	//	currentInput = Vector3.Lerp(currentInput, Vector3.zero, Time.deltaTime * 10);
	}
	
	public void damage(float amount, Vector3 origin) {
		currentHealth -= amount;
		if (currentHealth < 0) die(origin);
	}
	
	public void die(Vector3 origin) {
		dead = true;
		playerAnimator.playDieAnim(origin);
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
