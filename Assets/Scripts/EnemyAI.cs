using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour {
	
	Transform enemyObj;
	
	public enum Activity { Patrolling, Looking, OnBreak, Chasing, Investigating, Responding }
	public Activity currentActivity = Activity.Patrolling;
	
	EnemyController enemyController;
	NavMeshAgent navAgent;
	
	bool canSeeTarget;
	
	public bool startOnGuard;
	
	public float decisionTimer;
	public float lookTimer;
	public float spotTimer;
	public float chaseTimer;
	public float breakTimer;
	public float investigateTimer;
	public Transform chaseTarget;
	

	void Start () {
		enemyController = GetComponent<EnemyController>();
		navAgent = GetComponent<NavMeshAgent>();
		if (startOnGuard) {
			lookAround(100);
		} else {
			patrol();
		}
	}

	
	
	void Update () {
		
		
		decisionTimer -= Time.deltaTime;
		if (decisionTimer < 0) {
			makeChoice();
			decisionTimer = 2;
		}
		
		switch (currentActivity) {
			case Activity.Patrolling :
				if (navAgent.remainingDistance < 0.1) lookAround();
				break;
			case Activity.Looking :
				lookTimer -= Time.deltaTime;
				if (lookTimer < 0) {
					if (Random.value < 0.6) {
						transform.Rotate(0, 100, 0);
						lookAround();
					} else {
						patrol();
					}
				}			
				break;
			case Activity.OnBreak :
				if (navAgent.remainingDistance < 2.0) {
					enemyController.looking = false;
					breakTimer -= Time.deltaTime;
					if (breakTimer < 0) patrol();
				}
				break;
			case Activity.Chasing :
				//if (enemyObj && enemyObj.animation.IsPlaying("Spot")) navAgent.speed = 0;
			
				if (canSeeTarget) {
					navAgent.Stop();
					float direction = Util.getDirection(transform, chaseTarget);
					Quaternion rotGoal = Quaternion.Euler(0, direction, 0);
					transform.rotation = Quaternion.Lerp (transform.rotation, rotGoal, Time.deltaTime * 10);
					if ((transform.position - chaseTarget.position).sqrMagnitude > 200) {
						transform.Translate(Vector3.forward * Time.deltaTime * 5);
					}
					gameObject.BroadcastMessage("fire", SendMessageOptions.DontRequireReceiver);
				} else {
					navAgent.Resume();
				}

				chaseTimer -= Time.deltaTime;

				if (navAgent.remainingDistance < 0.1f) {
					lookAround();
					Vector3 playerPos = enemyController.getPlayerPosition();
					playerPos += new Vector3(Random.Range(-2,2), 0, Random.Range(-2,2));
					enemyController.move(playerPos);
				}
				break;

			case Activity.Investigating :
				if (navAgent.remainingDistance < 2.0f) lookAround();

				investigateTimer -= Time.deltaTime;
				if (investigateTimer < 0) {
					patrol();
				}	
				break;
		}	
	}
	
	public void makeChoice() {
		float diceRoll = Random.value;
		
		if (diceRoll < 0.10 && currentActivity == Activity.Patrolling) takeABreak();
	}
	
	public void setActivity(Activity newActivity) {
		currentActivity	= newActivity;
	} 
	
	public void patrol() {
		enemyController.startWalking();
		enemyController.looking = true;
		setActivity(Activity.Patrolling);
		enemyController.move(new Vector2(Random.Range(-20,20),Random.Range(-20,20)));
		//print ("move");
	}
	
	public void lookAround(float lookTime) {
		enemyController.startWalking();
		enemyController.looking = true;
		setActivity(Activity.Looking);	
		lookTimer = lookTime;
	}
	public void lookAround() {
		enemyController.looking = true;
		lookAround (Random.Range (0.5f, 2.0f));	
	}
	
	public void takeABreak() {
		enemyController.startWalking();
		setActivity(Activity.OnBreak);
		enemyController.move(enemyController.breakRoom.position + new Vector3(Random.Range(-2.0f,2.0f), 0.0f,Random.Range(-2.0f,2.0f)));
			//print ("move");
		breakTimer = Random.Range(3.0f,10.0f);
	}
	public void chase(Transform target) {
		
		if (currentActivity == Activity.Patrolling) {
		
		}
		enemyController.startRunning();
		enemyController.looking = true;
		setActivity(Activity.Chasing);
		chaseTarget = target;
		if (chaseTimer < 0) {
			chaseTimer = 1.0f;
			enemyController.move(chaseTarget.position);
					//print ("move chase");

		}
		enemyController.alert(chaseTarget.position);

		
	}
	public void investigate(Vector3 alertPos) {
		enemyController.startRunning();
		enemyController.looking = true;
		enemyController.move(alertPos);
				//print ("move in");

		setActivity(Activity.Investigating);	
		investigateTimer = Random.Range(5.0f, 10.0f);
	}
	
	public void inView(Transform target) {
		canSeeTarget = true;
	}
	
	public void outOfView() {
		canSeeTarget = false;
	}
}


