using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	
	Transform controlTarget;
	public UIThumbsticks thumbsticks;
	public float heightOffset;
	public float lookAhead;

	Vector3 homeViewOffset;
	Quaternion homeRotOffset;
	bool setUp =  false;
	
	public void SetUp() {
		thumbsticks.SetUp();
		controlTarget = GameObject.Find("Player").transform;
		homeViewOffset = Camera.main.transform.localPosition;
		homeRotOffset = transform.rotation;
		Camera.main.transform.LookAt(transform);
		
		setUp = true;
	}
	
	void LateUpdate () {
		if (!setUp) return;
				
		Vector3 posGoal;

		posGoal = controlTarget.TransformPoint(new Vector3(0, heightOffset, lookAhead));	
		Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, homeViewOffset, Time.deltaTime * 5);
		
		Camera.main.transform.LookAt(transform);
		transform.position = Vector3.Lerp(transform.position, posGoal, Time.deltaTime * 3);	
	}

	public void setTarget(Transform newTarget) {
	
	}

	public PlayerController getTarget() {
		return controlTarget.GetComponent<PlayerController>();
	}
	
	public void resetCamera() {
		transform.rotation = homeRotOffset;
			
	}
	

}













