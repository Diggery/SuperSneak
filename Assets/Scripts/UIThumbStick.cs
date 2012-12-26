using UnityEngine;
using System.Collections;

public class UIThumbStick : MonoBehaviour {
	
	public float UIScale;

	PlayerController playerController;

	Transform corner;
	//Transform anchor;	
	bool touched;	
	Vector3 homeOffset;
	Vector3 posGoal;

	void Start () {
	
		initThumbStick();
		playerController = Camera.main.transform.parent.GetComponent<CameraControl>().getTarget();
	
	}
	
	void Update () {
		
		if (touched) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, posGoal, Time.deltaTime * 10.0f);
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}
		
		
		playerController.moveInput(transform.localPosition * 10);
	}
	
	
	void initThumbStick() {
		corner = transform.parent.parent;
		//anchor = transform.parent;
		
		//move stick into position
		corner.parent = Camera.main.transform;
		corner.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane + 1));
		corner.rotation = Camera.main.transform.rotation;
		corner.Rotate(0, 180, 0);
		corner.localScale = new Vector3(UIScale, UIScale, UIScale);
		
		//set it up
		gameObject.AddComponent<SphereCollider>();
	}
	
	void touchDown(TouchManager.TouchDownEvent touchEvent) {
		touched = true;
		playerController.setInputOn();
	}
	
	void drag(TouchManager.TouchDragEvent touchEvent) {
		Vector3 worldPos = Camera.main.ScreenToWorldPoint
			(new Vector3 (touchEvent.touchPosition.x, touchEvent.touchPosition.y, Camera.main.nearClipPlane + 1));
		posGoal = Vector3.ClampMagnitude(transform.parent.InverseTransformPoint(worldPos), 0.1f);
		
	}
		
		
	void touchUp(TouchManager.TouchUpEvent touchEvent) {
		touched = false;
		playerController.setInputOff();
	
	}
	
}