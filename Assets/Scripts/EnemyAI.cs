using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour {
	
	Transform enemyObj;
	
	public enum Activity { Dead, Patrolling, Looking, OnBreak, Chasing, Shooting, Investigating, Responding }
	public Activity currentActivity = Activity.Patrolling;
	
	EnemyController enemyController;
	EnemyAnimator enemyAnimator;
	NavMeshAgent navAgent;
	
	
	bool canSeeTarget;
	[HideInInspector]
	public bool readyToFire;
	Vector3 lastKnownPos;
	
	public bool startOnGuard;
	
	public float lookingTimer;
	float alertLevel;

	void Start () {
		Events.Listen(gameObject, "GuardRadio");  
		enemyController = GetComponent<EnemyController>();
		enemyAnimator = GetComponent<EnemyAnimator>();
		navAgent = GetComponent<NavMeshAgent>();
		if (startOnGuard) {
			lookAround();
		} else {
			patrol();
		}
	}

	
	
	void Update () {
		if (currentActivity == Activity.Dead) {
			return;
		}
		
		if (alertLevel > 0) alertLevel -= Time.deltaTime;
		
		float targetRange = (transform.position - enemyController.player.position).magnitude;
		if (targetRange < 2.0f) {
			spotPlayer();
		}

		switch (currentActivity) {
			case Activity.Patrolling :
				if (!navAgent.hasPath) gotoRoom();
					
			break;
			
			case Activity.Looking :
				lookingTimer -= Time.deltaTime;
				if (lookingTimer < 0) patrol();
			break;
			
			case Activity.OnBreak :

			break;
			
			case Activity.Chasing :
				
				if (enemyController.canSeeTarget()) {
					lastKnownPos = enemyController.player.position;
					if (navAgent.hasPath) navAgent.ResetPath();
					enemyController.faceTarget(lastKnownPos);
								
					if (targetRange > 5) {
						enemyController.approachTarget(lastKnownPos);
					}
				
					if (targetRange < 20) {
						Vector3 fireTarget = new Vector3(enemyController.player.position.x, 1.5f, enemyController.player.position.z);
						BroadcastMessage("fire", fireTarget);
						readyToFire = true;
					} 
				} else {
					readyToFire = false;
					if (!navAgent.hasPath) {
						float currDistance = (transform.position - lastKnownPos).sqrMagnitude;
						if (currDistance < 0.25) {
							lookAround();
						} else {
							enemyController.runTo(lastKnownPos);

						}
					}
				}
			break;
			
			case Activity.Shooting :
			
			break;
			
			case Activity.Investigating :
				if (!navAgent.hasPath) {
					float currDistance = (transform.position - lastKnownPos).sqrMagnitude;
					if (currDistance < 0.25) {
						lookAround();
					}
				}
			break;
		}	
	}
		
	void gotoRoom() {
		if (currentActivity == Activity.Dead) return;
		GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
		int roomIndex = Random.Range(0, rooms.Length);
		enemyController.move(rooms[roomIndex].transform.position);
	}
	
	public void chase(Transform target) {
		if (currentActivity == Activity.Dead) return;
		
	}
	
	public void investigate(Vector3 alertPos) {
		if (currentActivity == Activity.Dead) return;
		currentActivity = Activity.Investigating;
		lastKnownPos = alertPos;
		enemyController.runTo(lastKnownPos);
	}
	
	public void lookAround() {
		if (currentActivity == Activity.Dead) return;
		navAgent.Stop();
		readyToFire = false;
		currentActivity = Activity.Looking; 
		lookingTimer = enemyAnimator.playLookAroundAnim();
		enemyController.startWalking();
	}
	
	public void heardSomething(Vector3 soundPos, float volume) {
		print ("HeardSomething " + alertLevel);
		if (currentActivity == Activity.Dead) return;
		
		alertLevel++;
		
		if (volume > 4.0f) investigate(soundPos);
		
		if (currentActivity == Activity.Patrolling) lookAround();
			
		if (currentActivity == Activity.Looking && alertLevel > 3.0f) investigate(soundPos);
	}
	
	public void patrol() {
		if (currentActivity == Activity.Dead) return;
		readyToFire = false;
		currentActivity = Activity.Patrolling;
	}
	
	public void spotPlayer() {
		if (currentActivity == Activity.Dead) return;
		currentActivity = Activity.Chasing;
	}
	
	public void GuardRadio(Events.Notification notification) {
		if (currentActivity == Activity.Dead) return;
		string message = (string)notification.data;
		switch (message) {
		case "Spotted" :
			if (currentActivity == Activity.Patrolling || 
				currentActivity == Activity.Investigating || 
				currentActivity == Activity.OnBreak) {
				investigate(enemyController.getPlayerPosition());
			}
			break;
			
		default :
			break;
		}
	}
	
	public void die() {
		currentActivity = Activity.Dead;
		
	}
	public void revive() {
		currentActivity = Activity.Looking;
		lookingTimer = 3.0f;
	}

}


