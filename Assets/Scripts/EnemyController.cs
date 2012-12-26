using UnityEngine;
using System.Collections;

public enum EnemyTypes { Guard, Captain, Enforcer, Boss}


public class EnemyController : MonoBehaviour {
	[HideInInspector]
	public Transform breakRoom;
	
	public Transform weapon; 
	public GameObject bullet; 
	public float coolDown;
	
	Transform player;
	
	NavMeshAgent navAgent;
	
	[HideInInspector]
	public EnemyManager enemyManager;
	
	public EnemyTypes EnemyType;

	EnemyAI enemyAI;
	
	[HideInInspector]
	public float moveTimer;
	
	public bool looking;
	
	float walkSpeed;
	float runSpeed;
	float currentSpeed;
	[HideInInspector]
	public float actualSpeed;
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
		weaponScript.setUpWeapon(bullet, coolDown);
	}
	
	void Update () {
		currentSpeed = Mathf.Lerp (currentSpeed, speedGoal, Time.deltaTime * 2);
		navAgent.speed = currentSpeed;
		
		float newSpeed = navAgent.velocity.magnitude;
		
		if (Mathf.Abs(newSpeed - actualSpeed) < 1.0f) actualSpeed = newSpeed;
			
		
		if (enemyAI.currentActivity == EnemyAI.Activity.Chasing) {
			RaycastHit hit;
			Vector3 startPos = new Vector3 (transform.position.x, 1.5f, transform.position.z);
			Vector3 endPos = new Vector3 (player.position.x, 1.5f, player.position.z);
			if (Physics.Linecast(startPos, endPos, out hit)) {
				if (hit.transform.tag == "Player") {
					Debug.DrawLine(startPos, hit.point, Color.red);
					enemyAI.inView(hit.transform);
				} else {
					Debug.DrawLine(startPos, hit.point, Color.green);
					enemyAI.outOfView();
				}
			}
		}		
			

	}
	
	void LateUpdate () {
		
		// make him look around
		headTimer -= Time.deltaTime;
		if (headTimer < 0.0f) setHeadTarget();
			
		Vector3 headTargetPos = headTarget.localPosition;
		
		if (enemyAI.currentActivity != EnemyAI.Activity.Chasing && enemyAI.currentActivity != EnemyAI.Activity.Responding) {
			headTargetPos.x = Mathf.Lerp (headTargetPos.x, headGoal, Time.deltaTime * 2);	
		} else {
			headTargetPos.x = Mathf.Lerp (headTargetPos.x, 0, Time.deltaTime * 2);	
		}
		
		headTarget.localPosition = headTargetPos;
	
		head.LookAt(headTarget);
		Vector3 headRot = head.localEulerAngles;
		headRot.z -= 90;
		head.localRotation = Quaternion.Euler(headRot);
	}
	
	void setHeadTarget() {
		if (Mathf.Abs(headGoal) > 0.01f) {
			headGoal = 0.0f;
		} else {
			headTimer = Random.Range(1.0f, 6.0f);
			headGoal = Random.Range(0.5f, 1.0f) + 0.5f;
			if (Random.value > 0.5f)  headGoal *= -1;
		}
	}

	public void move (Vector2 newTarget) {
		if (navAgent) navAgent.SetDestination(new Vector3(newTarget.x, 0.0f, newTarget.y));
	}
	public void move (Vector3 newTarget) {
		if (navAgent) navAgent.SetDestination(new Vector3(newTarget.x, 0.0f, newTarget.z));
	}
	
	public void spotPlayer (Transform target) {
		enemyAI.chase(target);	
		currentSpeed = runSpeed * 0.5f;
	}
	
	public Vector3 getPlayerPosition() {
		return player.position;	
	}
	
	public void startWalking () {
		speedGoal = walkSpeed;
	}	
	public void startRunning () {
		speedGoal = runSpeed;
	}
	
	public void investigate (Vector3 alertPos) {
		if (enemyAI.currentActivity != EnemyAI.Activity.Chasing) enemyAI.investigate(alertPos);
	}
	
	public void alert(Vector3 alertPos) {
		//if (enemyManager.alerted) return;
		enemyManager.alert(alertPos);	
	}
	
	public EnemyAI.Activity getCurrentActivity () {
		return enemyAI.currentActivity;
	}
}
