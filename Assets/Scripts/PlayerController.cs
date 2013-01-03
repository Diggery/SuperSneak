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
		
		Vector3 moveVector;
		
		if (currentInput.sqrMagnitude > 0.05f){
			currentDirection = Camera.main.transform.parent.TransformDirection(currentInput); 
			moveVector = currentDirection * moveSpeed;			
		} else {
			moveVector = Vector3.zero;			
		}
    	
		characterController.Move(moveVector * Time.deltaTime);
		
		float newSpeed = characterController.velocity.magnitude;
		
		currentSpeed = newSpeed;
		
		//regen health
		currentHealth = Mathf.Clamp(currentHealth + Time.deltaTime * 0.5f, 0, maxHealth);	
		
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
