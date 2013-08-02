using UnityEngine;
using System.Collections;

public class IKFeetController : MonoBehaviour {
	
	public Transform body;
	
	public Transform leftFootPrint;
	public Transform leftFootGoal;
	public Vector3 leftFootOldPos;
	
	public Transform rightFootPrint;
	public Transform rightFootGoal;
	public Vector3 rightFootOldPos;
	
	public bool leftFootUp;
	public bool rightFootUp;
	
	public float stanceWidth = 0.5f;
	public float stanceOffset = 0.5f;
	public float motionOffset = 1.2f;
	public float stepDistance = 1.2f;
	public float legLength = 2.3f;
	
	float leftFootStepTimer = 0.0f;
	float rightFootStepTimer = 0.0f;
	
	void Start () {
	
	}
	
	void Update () {
		
		float bodyDelta = getBodyDelta();
		float stepThreshold = 0.0f;
		
		if (Mathf.Abs(bodyDelta) > 0.1) {
			stanceWidth = 1.0f - (Mathf.Abs(bodyDelta) * 0.5f) ;
			stanceOffset = 0.0f;
			stepThreshold = stepDistance;
		} else {
			stanceWidth = 0.8f;
			stanceOffset = 0.3f;			
			stepThreshold = stepDistance * 0.1f;
		}
		
		leftFootPrint.localPosition = new Vector3(-stanceWidth, 0.0f, stanceOffset);
		rightFootPrint.localPosition = new Vector3(stanceWidth, 0.0f, -stanceOffset);
		
		if (!leftFootUp && !rightFootUp) {
			if (!rightFootUp && Vector3.Distance(leftFootPrint.position, leftFootGoal.position) > stepThreshold) liftLeftFoot(); 	
			if (!leftFootUp && Vector3.Distance(rightFootPrint.position, rightFootGoal.position) > stepThreshold) liftRightFoot(); 	
		}
		
		Vector3 stepOffset = new Vector3(0.0f, 0.0f, bodyDelta);
		

		if (rightFootUp) {
			rightFootStepTimer += Time.deltaTime * 3;
			if (rightFootStepTimer > 1.0f) plantRightFoot();
			Vector3 newRightFootPos = Vector3.Lerp(rightFootOldPos, rightFootPrint.position + stepOffset, rightFootStepTimer);
			newRightFootPos.y += 0.5f - Mathf.Abs(rightFootStepTimer - 0.5f);
			rightFootGoal.position = newRightFootPos;
		}

		if (leftFootUp) {
			leftFootStepTimer += Time.deltaTime * 3;
			if (leftFootStepTimer > 1.0f) plantLeftFoot();
			Vector3 newLeftFootPos = Vector3.Lerp(leftFootOldPos, leftFootPrint.position + stepOffset, leftFootStepTimer);
			newLeftFootPos.y += 0.5f - Mathf.Abs(leftFootStepTimer - 0.5f);
			leftFootGoal.position = newLeftFootPos;
		}
		
		float pelvisHeight = legLength + (0.5f - Mathf.Abs(rightFootStepTimer + leftFootStepTimer - 0.5f) * 0.5f);
		body.position = Vector3.Lerp (body.position, transform.position + new Vector3(0.0f, pelvisHeight, 0.0f), Time.deltaTime * 8);
	}
	
	void liftLeftFoot() {
		leftFootUp = true;
		leftFootOldPos = leftFootGoal.position;
	}
	
	void plantLeftFoot() {
		leftFootUp = false;
		leftFootStepTimer = 0;
		leftFootOldPos = leftFootGoal.position;
	}
	
	void liftRightFoot() {
		rightFootUp = true;
		rightFootOldPos = rightFootGoal.position;
	}
	
	void plantRightFoot() {
		rightFootUp = false;
		rightFootStepTimer = 0;
		rightFootOldPos = rightFootGoal.position;
	}
	
	float getBodyDelta() {
		Vector3 bodyRelative = body.InverseTransformPoint(transform.position);
		return Mathf.Clamp(bodyRelative.z * 2, -motionOffset, motionOffset);
	}
}
