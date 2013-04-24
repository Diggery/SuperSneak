using UnityEngine;
using System.Collections;


public class UIThumbsticks : MonoBehaviour {
	
	public Transform UIThumbsticksPrefab;
	
	public bool onScreen; 
	float transTimer = 1.0f;
	AnimationCurve transCurve;
	
	Transform lowerLeft;
	Transform lowerRight;
	Transform upperRight;
	
	Transform leftThumb;
	Transform rightAnchor;
	Vector3 rightAnchorHome;
	public AnimationCurve stickCurve;
	float stickTimer;
	Transform rightThumb;
	
	PlayerController playerController;
	InventoryController inventory;
	
	public bool leftTouched;	
	public bool rightTouched;	
	Vector3 homeOffset;	
	Vector3 leftPosGoal;	
	Vector3 rightPosGoal;	
	
	void Start () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj) playerController = playerObj.GetComponent<PlayerController>();
		
		transCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
		
		Transform UIThumbsticksObj = Instantiate(UIThumbsticksPrefab, transform.position, Quaternion.identity) as Transform;
		UIThumbsticksObj.parent = transform;
	
		lowerLeft = UIThumbsticksObj.Find("LowerLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerLeft);
	
		lowerRight = UIThumbsticksObj.Find("LowerRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerRight);
	
		upperRight = UIThumbsticksObj.Find("UpperRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperRight);
		
		Transform cashBox = UIThumbsticksObj.Find("UpperRight/Cash");
		UICashDisplay cashDisplay = cashBox.gameObject.AddComponent<UICashDisplay>();
		
		leftThumb = UIThumbsticksObj.Find("LowerLeft/LeftAnchor/LeftThumb");
		leftThumb.gameObject.AddComponent<UIElement>().setTarget(gameObject);
		
		rightAnchor = UIThumbsticksObj.Find("LowerRight/RightAnchor");
		rightAnchorHome = rightAnchor.localPosition;
		
		rightThumb = UIThumbsticksObj.Find("LowerRight/RightAnchor/RightThumb");
		rightThumb.gameObject.AddComponent<UIElement>().setTarget(gameObject);
		rightThumb.parent.gameObject.AddComponent<UIBombStickBehavior>().setUp(this, rightThumb);
		
		Transform inventoryPos = UIThumbsticksObj.Find("LowerRight/Inventory");
		inventory = GetComponent<InventoryController>();
		inventory.setUp(inventoryPos, cashDisplay);

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
		
		if (leftTouched) {
			leftThumb.localPosition = Vector3.Lerp(leftThumb.localPosition, leftPosGoal, Time.deltaTime * 10.0f);
			leftThumb.localScale = Vector3.Lerp(leftThumb.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			leftThumb.localPosition = Vector3.Lerp(leftThumb.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			leftThumb.localScale = Vector3.Lerp(leftThumb.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}
		
		if (rightTouched) {
			rightThumb.localPosition = Vector3.Lerp(rightThumb.localPosition, rightPosGoal, Time.deltaTime * 10.0f);
			rightThumb.localScale = Vector3.Lerp(rightThumb.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			rightThumb.localPosition = Vector3.Lerp(rightThumb.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			rightThumb.localScale = Vector3.Lerp(rightThumb.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}
		
		
		//Vector3 rightAnchorGoal;
		if (inventory.currentItems.Count < 1 && !rightTouched) {
			//rightAnchorGoal = rightAnchorHome + new Vector3(0.0f, -0.2f, 0.0f);
			if (stickTimer < 1) stickTimer += Time.deltaTime;
		} else {
			//rightAnchorGoal = rightAnchorHome;
			if (stickTimer > 0) stickTimer -= Time.deltaTime;

		}
		rightAnchor.localPosition = rightAnchorHome + new Vector3(0.0f, stickCurve.Evaluate(stickTimer) * -0.5f, 0.0f);
		
		playerController.leftInput(leftThumb.localPosition * 10.0f);
		playerController.rightInput(rightThumb.localPosition * 10.0f);
	}
	
	public void touchDown(TouchManager.TouchDownEvent touchEvent) {
		inventory.closeInventory();
		
		if (touchEvent.touchTarget.name.Equals("LeftThumb")) {
			leftTouched = true;
			playerController.setLeftInputOn();
		}
		if (touchEvent.touchTarget.name.Equals("RightThumb")) {
			rightTouched = true;
			playerController.setRightInputOn();
		}
			
		
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3 (touchEvent.touchPosition.x, touchEvent.touchPosition.y, Camera.main.nearClipPlane + 1.0f));
		if (touchEvent.startTarget.name.Equals("LeftThumb")) 
			leftPosGoal = Vector3.ClampMagnitude(leftThumb.parent.InverseTransformPoint(worldPos), 0.1f);
		
		if (touchEvent.startTarget.name.Equals("RightThumb")) 
			rightPosGoal = Vector3.ClampMagnitude(rightThumb.parent.InverseTransformPoint(worldPos), 0.1f);
		
		
	}
		
	public void touchUp(TouchManager.TouchUpEvent touchEvent) {

		if (touchEvent.startTarget.name.Equals("LeftThumb")) {
			leftTouched = false;
			playerController.setLeftInputOff();
		}
		if (touchEvent.startTarget.name.Equals("RightThumb")) {
			rightTouched = false;
			playerController.setRightInputOff();
		}		
	
	}
	
	
	public void tap(TouchManager.TapEvent touchEvent) {
	
	}

}