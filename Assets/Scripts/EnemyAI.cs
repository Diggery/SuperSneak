using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour {
	
	Transform enemyObj;
	
	public enum Activity { Dead,  Stunned, Blinded, Patrolling, Looking, OnBreak, Chasing, Shooting, Investigating, HeadingToGuardRoom }
	public Activity currentActivity = Activity.Patrolling;
	
	EnemyController enemyController;
	EnemyAnimator enemyAnimator;
	
	PathMover pathMover;
	
	bool canSeeTarget;
	[HideInInspector]
	public bool aggressive;
	public bool readyToFire;
	Vector3 lastKnownPos;
	
	
	public float lookingTimer;
	public float warningTime = 3;
	public float warningTimer = 3;
	float alertLevel;
	
	float stunTimer;
	float blindTimer;

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
				
					if (targetRange < enemyController.shootingRange) {
						if (enemyController.isRunning) enemyController.startWalking();

						if (!aggressive && !readyToFire) {
							warningTimer -= Time.deltaTime;
							if (warningTimer < 0) {
								readyToFire = true;
								aggressive = true;
								warningTimer = warningTime;
							}
						} else {
						
							Vector3 fireTarget = new Vector3(enemyController.player.position.x, 1.5f, enemyController.player.position.z);
							Vector3 fireOffset = Vector3.zero;
							if (enemyController.IsVisionBad()) 
								fireOffset = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-1.0f, 1.0f), Random.Range(-3.0f, 3.0f));
							BroadcastMessage("Fire", fireTarget + fireOffset);
						}
					} else {
						if (!enemyController.isRunning) enemyController.startRunning();
					}	
				} else {
					if (readyToFire) {
					readyToFire = false;
					BroadcastMessage("StopFiring");
					}
					if (!pathMover.HasPath()) {
						float currDistance = (transform.position - lastKnownPos).sqrMagnitude;
						if (currDistance < 0.25) {
							BroadcastMessage("StopFiring");
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
					//if (playerController.IsDead()) 
					lookAround();
				}
			break;
			
			case Activity.Stunned :
				stunTimer -= Time.deltaTime;
			
				if (stunTimer < 0) lookAround();
			break;
			
			case Activity.Blinded :
				blindTimer -= Time.deltaTime;
			
				if (blindTimer < 0) lookAround();
			break;
						
			case Activity.HeadingToGuardRoom :
				if (!pathMover.HasPath()) {
					GameObject guardRoom = GameObject.FindWithTag("GuardRoom");
					guardRoom.GetComponent<GuardRoom>().QueueEnemy("Guard");
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
			print (rooms[roomIndex].transform.position + " is a bad location");
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
		BroadcastMessage("StopFiring");
		pathMover.Stop();
		readyToFire = false;
		currentActivity = Activity.Looking; 
		lookingTimer = enemyAnimator.PlayLookAroundAnim();
		enemyController.startWalking();
	}
	
	public void heardSomething(Vector3 soundPos, float volume) {
		//print ("HeardSomething " + alertLevel);
		if (currentActivity == Activity.Dead) return;
		
		alertLevel += volume;
				
		if (currentActivity == Activity.Patrolling) lookAround();
			
		if (currentActivity == Activity.Looking && alertLevel > 2.0f) investigate(soundPos);
	}
	
	public void patrol() {
		if (currentActivity == Activity.Dead) return;
		BroadcastMessage("StopFiring");
		readyToFire = false;
		aggressive = false;
		currentActivity = Activity.Patrolling;
		gotoRoom();
	}
	
	
	public void spotPlayer() {
		if (currentActivity == Activity.Dead) return;
		if (currentActivity == Activity.Stunned) return;
		if (currentActivity == Activity.Blinded) return;
		
		lastKnownPos = enemyController.player.position;

		if (!enemyController.weapon) {
			raiseAlarm();
			return;
		}
		currentActivity = Activity.Chasing;
	}
	
	public void raiseAlarm() {		
		enemyAnimator.StopAnims();

		currentActivity = Activity.HeadingToGuardRoom;
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
	public void Blind(float blindTime) {
		currentActivity = Activity.Blinded;
		blindTimer = blindTime;
	}
	public void revive() {
		BroadcastMessage("StopFiring");
		currentActivity = Activity.Looking;
		lookingTimer = 3.0f;
	}
	public void OnCollisionEnter(Collision collision) {
			print (collision.transform.tag);	
		if (collision.transform.tag == "Wall") {
			print ("HittingWall");	
		}
		
	}

}


