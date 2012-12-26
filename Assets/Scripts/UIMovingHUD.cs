using UnityEngine;
using System.Collections;


public class UIMovingHUD : MonoBehaviour {
	
	public bool onScreen; 
	float transTimer = 1.0f;
	AnimationCurve transCurve;
	
	Transform lowerLeft;
	//Vector3 lowerLeftPos;
	Transform lowerRight;
	//Vector3 lowerRightPos;
	
	public Transform leftThumb;
	
	PlayerController playerController;
	
	bool touched;	
	Vector3 homeOffset;	
	Vector3 posGoal;	
	
	void Start () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj) playerController = playerObj.GetComponent<PlayerController>();
		
		transCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
	
		lowerLeft = transform.Find("LowerLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerLeft);
		//lowerLeftPos = lowerLeft.localPosition;
	
		lowerRight = transform.Find("LowerRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerRight);
		//lowerRightPos = lowerRight.localPosition;
	
		leftThumb.GetComponent<UIElement>().setTarget(gameObject);
	}
	
	void Update () {
		float transAmount;
		if (onScreen) {
			if (transTimer < 1.0f) {
				transTimer += Time.deltaTime * 2.0f;
				transAmount =  transCurve.Evaluate(transTimer);
				Vector3 lowerLeftAngles = lowerLeft.localEulerAngles;
				lowerLeftAngles.z = Mathf.Lerp(-90.0f, 0.0f, transAmount);
				lowerLeft.localEulerAngles = lowerLeftAngles;
				Vector3 lowerRightAngles = lowerRight.localEulerAngles;
				lowerRightAngles.z = Mathf.Lerp(90.0f, 0.0f, transAmount);
				lowerRight.localEulerAngles = lowerRightAngles;
			}
		} else {
			if (transTimer > 0.0f) {
				transTimer -= Time.deltaTime * 2.0f;
				transAmount =  transCurve.Evaluate(transTimer);

				Vector3 lowerLeftAngles = lowerLeft.localEulerAngles;
				lowerLeftAngles.z = Mathf.Lerp(-90.0f, 0.0f, transAmount);
				lowerLeft.localEulerAngles = lowerLeftAngles;
				Vector3 lowerRightAngles = lowerRight.localEulerAngles;
				lowerRightAngles.z = Mathf.Lerp(90.0f, 0.0f, transAmount);
				lowerRight.localEulerAngles = lowerRightAngles;

			}
		}
		
		if (touched) {
			leftThumb.localPosition = Vector3.Lerp(leftThumb.localPosition, posGoal, Time.deltaTime * 10.0f);
			leftThumb.localScale = Vector3.Lerp(leftThumb.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			leftThumb.localPosition = Vector3.Lerp(leftThumb.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			leftThumb.localScale = Vector3.Lerp(leftThumb.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}
		
		playerController.moveInput(leftThumb.localPosition * 10.0f);
	}
	
	public void touchDown(TouchManager.TouchDownEvent touchEvent) {
		touched = true;
		playerController.setInputOn();
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3 (touchEvent.touchPosition.x, touchEvent.touchPosition.y, Camera.main.nearClipPlane + 1.0f));
		posGoal = Vector3.ClampMagnitude(leftThumb.parent.InverseTransformPoint(worldPos), 0.1f);
		
	}
		
	public void touchUp(TouchManager.TouchUpEvent touchEvent) {
		touched = false;
		playerController.setInputOff();
	
	}
	
	
	public void tap(TouchManager.TapEvent touchEvent) {
	
	}

}