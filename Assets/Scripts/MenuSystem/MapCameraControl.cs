using UnityEngine;
using System.Collections;

public class MapCameraControl : MonoBehaviour {
	
	
	public MapControl mapControl;
	Transform mapCamera;
	public float scollSpeed = 0.1f;
	
	Vector3 posGoal = new Vector3(-10.0f, 0.0f, 0.0f);
	float rotGoal;
	
	Transform focusTarget;

	public void SetUp (MapControl newMapControl) {
		mapControl = newMapControl;
		mapCamera = Camera.main.transform;
	}
	
	void Update () {
		Vector3 cameraPosition = Vector3.zero;
		Vector3 cameraOffset = Vector3.zero;
		
		if (focusTarget) {
			cameraPosition = focusTarget.position;
			cameraOffset = new Vector3(0.0f, 12.0f, -10.0f);
		} else {
			cameraPosition = posGoal;
			cameraOffset = new Vector3(0.0f, 15.0f, -15.0f);
		}
		mapCamera.localPosition = Vector3.Lerp(mapCamera.localPosition, cameraOffset, GameTime.deltaTime * 2);
		transform.position = Vector3.Lerp (transform.position, cameraPosition, GameTime.deltaTime * 2);
		Quaternion goalQuaternion = Quaternion.AngleAxis(rotGoal + 48, Vector3.right);
		Camera.main.transform.rotation = Quaternion.Lerp (Camera.main.transform.rotation, goalQuaternion, GameTime.deltaTime * 2);

	}
	
	public void SetFocus(Transform newTarget) {
		posGoal = newTarget.position;
		posGoal.y = 0.0f;
		focusTarget = newTarget;
	}
	
	public void ClearTarget() {
		focusTarget = null;
		posGoal.z = 0.0f;
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		float widthClamp = ((float)mapControl.width / 2.0f) + 5.0f;
		posGoal.x = Mathf.Clamp(posGoal.x - (touchEvent.touchDelta.x * scollSpeed), -widthClamp, widthClamp);
		float lengthClamp = ((float)mapControl.length / 3.0f);
		rotGoal = Mathf.Clamp(rotGoal + (touchEvent.touchDelta.y * scollSpeed), -lengthClamp, lengthClamp);
		ClearTarget();
	}
	public void touchUp(TouchManager.TouchUpEvent touchEvent) {
		float widthClamp = ((float)mapControl.width / 2.0f) - 5.0f;
		posGoal.x = Mathf.Clamp(posGoal.x, -widthClamp, widthClamp);
	}
	

}
