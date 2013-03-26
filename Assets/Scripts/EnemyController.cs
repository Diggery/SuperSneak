using UnityEngine;
using System.Collections;

public enum EnemyTypes { Guard, Captain, Enforcer, Boss}


public class EnemyController : MonoBehaviour {
	[HideInInspector]
	public Transform breakRoom;
	
	public Transform weapon; 
	public GameObject bullet; 
	public float coolDown;
	public float spinUp;
	
	[HideInInspector]
	public Transform player;
	
	NavMeshAgent navAgent;
	
	[HideInInspector]
	public EnemyManager enemyManager;
	
	public EnemyTypes EnemyType;

	EnemyAI enemyAI;
	
	[HideInInspector]
	public float moveTimer;
	
	public bool looking;
	
	[HideInInspector]
	public bool isRunning;
	float walkSpeed;
	float runSpeed;
	float turnSpeed = 180;
	float currentSpeed;
	Vector3 lastMovePos;
	float actualSpeed;
	float speedGoal;
	
	public Transform head;
	public Transform headTarget;
	float headGoal;
	float headTimer;
	float headOffset;
	
	void Start () {
		navAgent = GetComponent<NavMeshAgent>();
		player = GameObject.FindWithTag ("Player").transform;
		breakRoom = GameObject.Find ("BreakRoom").transform;
		enemyManager = breakRoom.GetComponent<EnemyManager>();
		walkSpeed = enemyManager.getWalkSpeed(EnemyType);
		runSpeed = enemyManager.getRunSpeed(EnemyType);
		enemyAI = GetComponent<EnemyAI>();
		Weapon weaponScript = weapon.gameObject.AddComponent<Weapon>();
		weaponScript.setUpWeapon(bullet, coolDown, spinUp);
		
		startWalking();
		

	}
	
	void Update () {
		currentSpeed = Mathf.Lerp (currentSpeed, speedGoal, Time.deltaTime * 2);
		navAgent.speed = currentSpeed;
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
		if (isRunning) startWalking();
		Vector3 targetPos = new Vector3(target.x, 0, target.z);
		transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
	}
	
	public void walkTo(Vector3 newLoc) {
		startWalking();	
		move(newLoc);
	}
	
	public void runTo(Vector3 newLoc) {
		startRunning();	
		move(newLoc);
	}

	public void move (Vector3 newTarget) {
		if (navAgent) navAgent.SetDestination(new Vector3(newTarget.x, 0.0f, newTarget.z));
	}
	
	public void spotPlayer (Transform target) {
		Events.Send(gameObject, "GuardRadio", "Spotted");
		enemyAI.spotPlayer();	
		currentSpeed = runSpeed * 0.5f;
	}
	
	public Vector3 getPlayerPosition() {
		return player.position;	
	}
	
	public void startWalking() {
		speedGoal = walkSpeed;
		isRunning = false;
	}	
	public void startRunning() {
		isRunning = true;
		speedGoal = runSpeed;
	}
	
	public void investigate (Vector3 alertPos) {
		if (enemyAI.currentActivity != EnemyAI.Activity.Chasing) enemyAI.investigate(alertPos);
	}
	
	public void alert(Vector3 alertPos) {
		enemyManager.alert(alertPos);	
	}
	
	public EnemyAI.Activity getCurrentActivity () {
		return enemyAI.currentActivity;
	}
	
	public bool canSeeTarget() {
		RaycastHit hit;
		Vector3 startPos = new Vector3 (transform.position.x, 1.5f, transform.position.z);
		Vector3 endPos = new Vector3 (player.position.x, 1.5f, player.position.z);
		if (Physics.Linecast(startPos, endPos, out hit)) {
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

}
