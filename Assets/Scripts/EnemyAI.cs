using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour {
	
	Transform enemyObj;
	
	public enum Activity { Patrolling, Looking, OnBreak, Chasing, Shooting, Investigating, Responding }
	public Activity currentActivity = Activity.Patrolling;
	
	EnemyController enemyController;
	EnemyAnimator enemyAnimator;
	NavMeshAgent navAgent;
	
	bool canSeeTarget;
	bool readyToFire;
	Vector2 lastKnownPos;
	
	public bool startOnGuard;
	
	public float decisionTimer;

	void Start () {
		enemyController = GetComponent<EnemyController>();
		enemyAnimator = GetComponent<EnemyAnimator>();
		navAgent = GetComponent<NavMeshAgent>();
		if (startOnGuard) {
		//	lookAround(100);
		} else {
		//	patrol();
		}
	}

	
	
	void Update () {

		switch (currentActivity) {
			case Activity.Patrolling :
				if (!navAgent.hasPath) gotoRoom();
					
			break;
			
			case Activity.Looking :

			
			break;
			
			case Activity.OnBreak :

			break;
			
			case Activity.Chasing :
				float targetRange = (transform.position - enemyController.player.position).magnitude;
				
				if (enemyController.canSeeTarget()) {
					lastKnownPos = new Vector2(enemyController.player.position.x, enemyController.player.position.z);
					if (navAgent.hasPath) navAgent.ResetPath();
					enemyController.faceTarget(lastKnownPos);
								
					if (targetRange > 5) {
						enemyController.approachTarget(lastKnownPos);
					}
				
					if (targetRange < 20) {
						Vector3 fireTarget = new Vector3(enemyController.player.position.x, 1.5f, enemyController.player.position.z);
						BroadcastMessage("fire", fireTarget);
					} else {
						//readyToFire = false;
					}
				
				} else {
					if (!navAgent.hasPath) {
						float currDistance = (new Vector2(transform.position.x, transform.position.z) - lastKnownPos).sqrMagnitude;
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
	
			break;
		}	
	}
		
	void gotoRoom() {
		GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
		int roomIndex = Random.Range(0, rooms.Length);
		enemyController.move(new Vector2(rooms[roomIndex].transform.position.x,rooms[roomIndex].transform.position.z));
	}
	
	public void chase(Transform target) {
		
	}
	public void investigate(Vector3 alertPos) {
		
	}
	
	public void lookAround() {
		currentActivity = Activity.Looking;
	}
	
	public void spotPlayer(Transform target) {
		currentActivity = Activity.Chasing;
	}

}


