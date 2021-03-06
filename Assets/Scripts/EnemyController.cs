using UnityEngine;
using System.Collections;

public enum EnemyTypes { Guard, Janitor, Captain, Enforcer, Boss}


public class EnemyController : MonoBehaviour {
	[HideInInspector]
	public GuardRoom guardRoom;
	
	public Transform weapon; 
	public GameObject bullet;
	Transform[] accessories;
	public float coolDown;
	public float spinUp;
	
	[HideInInspector]
	public Transform player;
	
	PathMover pathMover;
	EnemyAnimator enemyAnimator;
	CharacterController characterController;
	
	[HideInInspector]
	public EnemyManager enemyManager;
	
	[HideInInspector]
	public RagDollController ragDoll;
	
	public EnemyTypes EnemyType;

	EnemyAI enemyAI;
	
	[HideInInspector]
	public float moveTimer;
	
	public bool looking;
	float badVision;
	
	[HideInInspector]
	public bool isRunning;
	public float walkSpeed;
	public float runSpeed;
	public float turnSpeed = 180;
	float currentSpeed;
	Vector3 lastMovePos;
	float actualSpeed;
	float speedGoal;
	public float hearingRange = 10;
	public float shootingRange = 10;
	
	public float currentHealth;
	public float maxHealth;
	bool dead;
	
	Transform head;
	float headGoal;
	float headTimer;
	float headOffset;
	
	
	public void setUp(Transform newHead, RagDollController newRagdoll, Transform[] newAccessories) {
		head = newHead;
		ragDoll = newRagdoll;
		accessories = newAccessories;
		pathMover = GetComponent<PathMover>();
		player = GameObject.FindWithTag ("Player").transform;
		GameObject guardRoomObj = GameObject.FindWithTag("GuardRoom");
		if (guardRoomObj) guardRoom = guardRoomObj.GetComponent<GuardRoom>();
		
		enemyAI = GetComponent<EnemyAI>();
		enemyAnimator = GetComponent<EnemyAnimator>();
		characterController = GetComponent<CharacterController>();
		startWalking();
		Events.Listen(gameObject, "SoundEvents");  
		
		
		//color the minimap dot
		Transform miniMapDot = transform.Find("MiniMapUnit");
		if (miniMapDot) {
			switch (EnemyType) {
			case EnemyTypes.Guard :
				miniMapDot.renderer.material.color = Color.red;
				break;
			case EnemyTypes.Janitor :
				miniMapDot.renderer.material.color = Color.cyan;
				break;
			default :
				miniMapDot.renderer.material.color = Color.black;
				break;
			}
			miniMapDot.renderer.material.renderQueue = 3000 + Random.Range(1, 500);
		}
	}
	
	public void addWeapon(Transform newWeapon) {
		weapon = newWeapon;
		Weapon weaponScript = weapon.gameObject.AddComponent<Weapon>();
		weaponScript.setUpWeapon(bullet, coolDown, spinUp);
	}

	
	public Transform getHead() {
		return head;
	}
	
	void Update () {
			
		if (Input.GetKeyUp(KeyCode.B)) {
			die(transform.position);	
		} 	
		if (Input.GetKeyUp(KeyCode.N)) {
			revive();	
		} 
		
		if (badVision > 0.0f) {
			badVision -= Time.deltaTime;	
		}
		
		if (enemyAI.currentActivity == EnemyAI.Activity.Dead) {
			return;
		} else {
			currentSpeed = Mathf.Lerp (currentSpeed, speedGoal, Time.deltaTime * 2);
		}
		pathMover.speed = currentSpeed;
		pathMover.turnSpeed = turnSpeed;
	}
	
	void LateUpdate () {
		// speed check
		Vector3 moveDelta = lastMovePos - transform.position;
		actualSpeed = moveDelta.magnitude;
		lastMovePos = transform.position;
		
	}
	
	public bool isMoving() {
		if (actualSpeed > 0.01f) {
			return true;
		} else {
			return false;
		}
	}
	
	public void faceTarget(Vector3 target) {
		Vector3 localSpace = transform.InverseTransformPoint(new Vector3(target.x, 0,target.z));
		float turnAmount = Mathf.Clamp (localSpace.x * turnSpeed, -turnSpeed, turnSpeed);
		transform.Rotate(0, turnAmount * Time.deltaTime, 0);
	}
	public void approachTarget(Vector3 target) {
		Vector3 targetPos = new Vector3(target.x, 0, target.z);
		Vector3 moveVector = (transform.position - targetPos).normalized;
		characterController.Move(-moveVector * walkSpeed * Time.deltaTime);
		//transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
	}
	
	public void walkTo(Vector3 newLoc) {
		startWalking();	
		move(newLoc);
	}
	
	public void runTo(Vector3 newLoc) {
		startRunning();	
		move(newLoc);
	}

	public bool move(Vector3 newTarget) {
		if (IsDisabled()) return false;

		if (pathMover) {
			if (!pathMover.SetDestination(new Vector3(newTarget.x, 0.0f, newTarget.z))) {
				return false;
			}
			return true;
		}
		return false;
	}
	
	public void spotPlayer (Transform target) {
		if (IsDisabled()) return;

		Events.Send(gameObject, "GuardRadio", "Spotted");
		enemyAI.spotPlayer();	
		currentSpeed = runSpeed * 0.5f;
	}
	
	public Vector3 getPlayerPosition() {
		return player.position;	
	}
	
	public bool IsPlayerDead() {
		return player.GetComponent<PlayerController>().IsDead();	
	}
	
	public bool IsDisabled() {
		EnemyAI.Activity currentActivity = getCurrentActivity();
		
		if (currentActivity == EnemyAI.Activity.Dead ||
			currentActivity == EnemyAI.Activity.Blinded ||
			currentActivity == EnemyAI.Activity.Stunned ) {
			return true;
		}	
		return false;
	}
		
	public void startWalking() {
		speedGoal = walkSpeed + (Random.value - 0.5f);
		isRunning = false;
	}	
	public void startRunning() {
		isRunning = true;
		speedGoal = runSpeed + (Random.value - 0.5f);
	}
	
	public void investigate (Vector3 alertPos) {
		if (IsDisabled()) return;
		if (enemyAI.currentActivity != EnemyAI.Activity.Chasing) enemyAI.investigate(alertPos);
	}
	
	public void LookAtDeadPlayer() {
		if (IsDisabled()) return;
		print ("looking at dead player");
		enemyAI.investigate(getPlayerPosition());

	}
	
	public void alert(Vector3 alertPos) {
		if (enemyManager) enemyManager.alert(alertPos);	
	}
	
	public EnemyAI.Activity getCurrentActivity () {
		return enemyAI.currentActivity;
	}
	
	public bool canSeeTarget() {
		RaycastHit hit;
		Vector3 startPos = new Vector3 (transform.position.x, 0.5f, transform.position.z);
		Vector3 endPos = new Vector3 (player.position.x, 0.5f, player.position.z);
				
		int layer1 = LayerMask.NameToLayer("Default"); // default
		int layer2 = LayerMask.NameToLayer("Player"); // player
		int layermask = ((1 << layer1) | (1 << layer2));
		
		if (Physics.Linecast(startPos, endPos, out hit, layermask)) {
			if (hit.transform.tag == "Player") {
				Debug.DrawLine(startPos, hit.point, Color.red);
				return true;
			} else {
				Debug.DrawLine(startPos, hit.point, Color.gray);
				return false;
			}
		} else {
			
			return false;
		}
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
	
	public void Gassed() {
		if (IsDisabled()) return;
		enemyAI.Stunned(7);
		startWalking();
		enemyAnimator.StopAnims();
		enemyAnimator.PlayStunnedAnim();
		pathMover.Stop();
	}	
	
	public void Blinded(float duration) {
		if (IsBlinded()) enemyAI.Blind(duration);
		if (IsDisabled()) return;
		enemyAI.Blind(duration);
		startWalking();
		enemyAnimator.StopAnims();
		pathMover.Stop();
	}		
	
	public void Smoked() {
		badVision = 10;
	}
	
	public bool IsVisionBad() {
		if (badVision > 0) return true;
		return false;
	}
	
	public bool IsBlinded() {
		if (enemyAI.currentActivity == EnemyAI.Activity.Blinded) return true;
		return false;
	}	
	
	public void die(Vector3 origin) {
		enemyAI.die();
		startWalking();
		characterController.enabled = false;
		enemyAnimator.StopAnims();
		pathMover.Stop();
		Vector3 deathForce = (transform.position - origin).normalized + new Vector3(0.0f, 0.5f, 0.0f);
		deathForce *= 100;
		ragDoll.enableRagDoll(deathForce);
		foreach (Transform accessory in accessories) {
			accessory.parent = null;
			accessory.gameObject.AddComponent<Rigidbody>();
			accessory.gameObject.AddComponent<BoxCollider>();
		}
		accessories = new Transform[0];
	}
	
	public void revive() {
		Invoke ("rebootAI", enemyAnimator.PlayGetUpAnim());
	}
	
	public void StandDown() {
		
		print ("Standing Down");
		startWalking();
		enemyAI.patrol();
	}	
	
	public void rebootAI() {
		enemyAI.revive();
	}
	
	public void SoundEvents(Events.Notification notification) {
		Vector4 soundData = (Vector4)notification.data;
		Vector3 soundPos = new Vector3(soundData.x, soundData.y, soundData.z);
		float volume = soundData.w;
		if ((transform.position - soundPos).magnitude > hearingRange * volume) return;
		enemyAI.heardSomething(soundPos, volume);
		
	}

}
