using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour {
	
	Transform enemyObj;
	
	public enum Activity { Dead,  Stunned, Patrolling, Looking, OnBreak, Chasing, Shooting, Investigating, HeadingToControlRoom }
	public Activity currentActivity = Activity.Patrolling;
	
	EnemyController enemyController;
	EnemyAnimator enemyAnimator;
	
	PathMover pathMover;
	
	bool canSeeTarget;
	[HideInInspector]
	public bool readyToFire;
	Vector3 lastKnownPos;
	
	
	public float lookingTimer;
	float alertLevel;
	
	float stunTimer;

	void Start () {
		Events.Listen(gameObject, "GuardRadio");  
		enemyController = GetComponent<EnemyController>();
		enemyAnimator = GetComponent<EnemyAnimator>();
		pathMover = GetComponent<PathMover>();
		if (!enemyAnimator) print ("ERROR: No enemy animator"); 

	}

	void forceSetUp() {
		enemyController = GetComponent<EnemyController>();
		enemyAnimator = GetComponent<EnemyAnimator>();		
	}
	
	void Update () {
		if (currentActivity == Activity.Dead) {
			return;
		}
		if (currentActivity == Activity.Stunned) {
			stunTimer -= Time.deltaTime;
			if (stunTimer < 0) revive();
			return;
		}
		
		if (alertLevel > 0) alertLevel -= Time.deltaTime * 0.25f;
		
		float targetRange = (transform.position - enemyController.player.position).magnitude;
		
		//if he is right next to me, just spot him
		if (targetRange < 2.0f && enemyController.canSeeTarget()) spotPlayer();
		

		switch (currentActivity) {
			case Activity.Patrolling :
				if (!pathMover.HasPath()) gotoRoom();
					
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
					if (pathMover.HasPath()) pathMover.Stop();
					enemyController.faceTarget(lastKnownPos);
								
					if (targetRange > 5) {
						enemyController.approachTarget(lastKnownPos);
					}
				
					if (targetRange < 15) {
						Vector3 fireTarget = new Vector3(enemyController.player.position.x, 1.5f, enemyController.player.position.z);
						BroadcastMessage("fire", fireTarget);
						readyToFire = true;
					} 
				} else {
					readyToFire = false;
					if (!pathMover.HasPath()) {
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
				if (!pathMover.HasPath()) {
					lookAround();
				}
			break;
			
			case Activity.HeadingToControlRoom :
				if (!pathMover.HasPath()) {
					GameObject guardRoom = GameObject.FindWithTag("GuardRoom");
					guardRoom.GetComponent<ControlRoom>().QueueEnemy("Guard");
					lookAround();
				}
			break;
		}	
	}
		
	void gotoRoom() {
		if (currentActivity == Activity.Dead) return;
		GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
		int roomIndex = Random.Range(0, rooms.Length);

		if (!enemyController.move(rooms[roomIndex].transform.position)) {
			//print (rooms[roomIndex].transform.position + " is a bad location");
		}
	}
	
	public void chase(Transform target) {
		if (currentActivity == Activity.Dead) return;
		
	}
	
	public void investigate(Vector3 alertPos) {
		if (!enemyAnimator) forceSetUp();
		if (currentActivity == Activity.Dead) return;
		if (enemyAnimator) enemyAnimator.StopAnims();
		currentActivity = Activity.Investigating;
		lastKnownPos = alertPos;
		enemyController.runTo(lastKnownPos);
	}
	
	public void lookAround() {
		if (currentActivity == Activity.Dead) return;
		pathMover.Stop();
		readyToFire = false;
		currentActivity = Activity.Looking; 
		lookingTimer = enemyAnimator.PlayLookAroundAnim();
		enemyController.startWalking();
	}
	
	public void heardSomething(Vector3 soundPos, float volume) {
		print ("HeardSomething " + alertLevel);
		if (currentActivity == Activity.Dead) return;
		
		alertLevel += volume;
				
		if (currentActivity == Activity.Patrolling) lookAround();
			
		if (currentActivity == Activity.Looking && alertLevel > 2.0f) investigate(soundPos);
	}
	
	public void patrol() {
		if (currentActivity == Activity.Dead) return;
		readyToFire = false;
		currentActivity = Activity.Patrolling;
		gotoRoom();
	}
	
	
	public void spotPlayer() {
		if (currentActivity == Activity.Dead) return;
		
		lastKnownPos = enemyController.player.position;

		if (!enemyController.weapon) {
			raiseAlarm();
			return;
		}
		currentActivity = Activity.Chasing;
	}
	
	public void raiseAlarm() {		
		enemyAnimator.StopAnims();

		currentActivity = Activity.HeadingToControlRoom;
		GameObject guardRoom = GameObject.FindWithTag("GuardRoom");
		enemyController.runTo(guardRoom.transform.position);
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
	public void Stunned(float stunTime) {
		currentActivity = Activity.Stunned;
		stunTimer = stunTime;
	}
	public void revive() {
		currentActivity = Activity.Looking;
		lookingTimer = 3.0f;
	}

}


