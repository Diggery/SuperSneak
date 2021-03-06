using UnityEngine;
using System.Collections;

public class PathMover : Pathfinding {
	
	public float speed;
	public float turnSpeed;
	public float distanceThreshold = 0.3f;
	
	public float coolDown = 0.5f;
	float coolDownTimer = 0.0f;
			
	CharacterController characterController;

	void Start () {
		characterController = GetComponent<CharacterController>();
		distanceThreshold *= distanceThreshold;
	}
	
	void Update () {
		if (coolDownTimer > 0) coolDownTimer -= Time.deltaTime;

        if (Path.Count > 0) {
			Vector3 nextPath = Path[0];
			nextPath.y = 0;
			Vector3 moveDirection = nextPath - transform.position;
			
			//if we are trying to move somewhere, face that direction
			if (moveDirection.sqrMagnitude > 0.01f) {
				float heading = Util.getDirection(moveDirection);
				Quaternion targetRotation = Quaternion.AngleAxis(heading, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
			}
			
			moveDirection = moveDirection.normalized * speed * Time.deltaTime;
			if (characterController.enabled) characterController.Move(moveDirection); 
			
            if ((transform.position- nextPath).sqrMagnitude < distanceThreshold) {
                Path.RemoveAt(0);
            }
        }
	
	}
	
	public bool SetDestination(Vector3 newDestination) {
		if (coolDownTimer > 0) {
			print ("still cooling down");
			return false;
		}
		
		FindPath(transform.position, newDestination);
		coolDownTimer = coolDown + Random.value * 0.1f;
		return HasPath();
	}
	
	public void Stop() {
		Path.Clear();
	}
	
	public bool HasPath() {
		if (Path.Count > 0) {
			return true;
		} else {
			return false;
		}
	}
	
}
