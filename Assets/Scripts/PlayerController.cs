using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	CharacterController characterController;
	PlayerAnimator playerAnimator;
	public WristWeapon wristWeapon;
	public BombThrower playerBombThrower;
	public InventoryController inventory;
	
	public bool moveInputOn;
	public Vector3 currentMoveInput;
	public bool throwInputOn;
	public Vector3 currentThrowInput;
	public bool shootInputOn;
	public Vector3 currentShootInput;
	
	float runSpeed = 4.0f;
	float walkSpeed = 2.0f;
	
	public float currentSpeed;
	public Vector3 currentDirection;
	
	public float currentHealth;
	public float maxHealth;
	bool dead;
	
	Transform head;
	RagDollController ragDoll;
	GameControl gameControl;
	
	public void SetUp (Transform thiefHead, RagDollController thiefRagDoll, BombThrower newBombThrower, WristWeapon newWristWeapon) {

		ragDoll = thiefRagDoll;
		playerBombThrower = newBombThrower;
		wristWeapon = newWristWeapon;
		characterController = GetComponent<CharacterController>();
		playerAnimator = GetComponent<PlayerAnimator>();
		
		Transform miniMapDot = transform.Find("MiniMapUnit");
		if (miniMapDot) {
			miniMapDot.renderer.material.color = Color.yellow;
			miniMapDot.renderer.material.renderQueue = 3500;
		}
		GameObject gameControlObj = GameObject.Find ("GameControl");
		gameControl = gameControlObj.GetComponent<GameControl>();		
		
	}

	void Update () {
		if (dead) return;
		
		Vector3 moveVector = Vector3.zero;
		float movePower = currentMoveInput.magnitude;
		
		if (movePower > 0.05f){
			currentDirection = Camera.main.transform.parent.TransformDirection(currentMoveInput); 
			moveVector = currentDirection.normalized;
			
			if (movePower < 0.5 || shootInputOn) {
				moveVector *= walkSpeed;
			} else {
				moveVector *= runSpeed;
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
		gameControl.PlayerIsDead();
		

	}
	
	public bool IsDead() {
		return dead;	
		
	}
	
	public void setInventory(InventoryController newInventory) {
		inventory = newInventory;
	}
	
	public GameObject getBomb() {
		return inventory.getBomb();
	}
	
	public void addItem(string type) {
		inventory.addItem(type);
	}
	
	public void doneThrowing() {
		inventory.unlockList();
	}
	
	public void setMoveInputOn() {
		moveInputOn = true;
	}
	
	public void setThrowInputOn() {
		throwInputOn = true;
		playerAnimator.playReadyBombAnim();
		playerBombThrower.resetBombTarget();
	}
	
	public void setShootInputOn() {
		shootInputOn = true;
		playerAnimator.playShootWeaponAnim();
		InventoryItem currentItem = inventory.GetItemProperties();
		if (!currentItem) Debug.Log ("ERROR: no items");
		if (currentItem.itemType != InventoryItem.ItemTypes.Projectile) Debug.Log ("ERROR: current item not a projectile weapon");
		wristWeapon.Fire(currentItem.getName());
		
	}	
	
	public void moveInput(Vector3 newInput) {
		currentMoveInput = new Vector3(newInput.x, newInput.z, -newInput.y);
	}
	
	public void throwInput(Vector3 newInput) {
		currentThrowInput = new Vector3(newInput.x, newInput.z, -newInput.y);
	}
	
	public void shootInput(Vector3 newInput) {
		currentShootInput = new Vector3(newInput.x, newInput.z, -newInput.y);
	}

	public void setMoveInputOff() {
		moveInputOn = false;
	}
	
	public void setThrowInputOff() {
		if (currentThrowInput.sqrMagnitude < 0.04f || !playerBombThrower.getBomb()) {
			playerAnimator.playPutAwayItemAnim();
			throwInputOn = false;
			return;
		}
		
		if (throwInputOn) {
			playerAnimator.playThrowBombAnim();
		}
		throwInputOn = false;
	}
	
	public void setShootInputOff() {
		shootInputOn = false;
		playerAnimator.playPutAwayItemAnim();
		if (!moveInputOn) currentDirection = -currentShootInput; 
		wristWeapon.StopFiring();

	}	
	
	public void openCrate(Vector3 cratePos) {
		playerAnimator.playOpenCrateAnim(cratePos);
	}
	

}
