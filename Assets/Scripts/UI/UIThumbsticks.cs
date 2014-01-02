using UnityEngine;
using System.Collections;


public class UIThumbsticks : MonoBehaviour {
	
	public Transform UIThumbsticksPrefab;
	public Transform UIStatusDisplayPrefab;
	
	public bool onScreen; 
	float transTimer = 1.0f;
	AnimationCurve transCurve;
	
	Transform lowerLeft;
	Transform lowerRight;
	Transform upperLeft;
	Transform upperRight;
	Transform lowerCenter;
	
	Transform moveThumb;
	public bool moveTouched;	
	Vector3 movePosGoal;	
	
	Transform throwAnchor;
	Transform throwThumb;
	public bool throwTouched;	
	Vector3 throwPosGoal;
	
	Transform shootAnchor;
	Transform shootThumb;
	public bool shootTouched;	
	Vector3 shootPosGoal;
	
	public AnimationCurve stickCurve;
	
	PlayerController playerController;
	public InventoryController inventory;
	
	Vector3 homeOffset;	
	
	bool setUp =  false;
	
	public void SetUp () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj) playerController = playerObj.GetComponent<PlayerController>();
		
		transCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
		
		Transform UIThumbsticksObj = Instantiate(UIThumbsticksPrefab, transform.position, Quaternion.identity) as Transform;
		UIThumbsticksObj.parent = transform;
		

		lowerLeft = UIThumbsticksObj.Find("LowerLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerLeft);
	
		lowerRight = UIThumbsticksObj.Find("LowerRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerRight);
		
		upperLeft = UIThumbsticksObj.Find("UpperLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperLeft);
		MiniMapDisplay miniMapDisplay = upperLeft.gameObject.AddComponent<MiniMapDisplay>();
		miniMapDisplay.InitMiniMap();

		lowerCenter = UIThumbsticksObj.Find("LowerCenter");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerCenter);
		
		Transform UIStatusDisplayObj = Instantiate(UIStatusDisplayPrefab, lowerCenter.position, lowerCenter.rotation) as Transform;
		UIStatusDisplayObj.parent = lowerCenter;
	
		upperRight = UIThumbsticksObj.Find("UpperRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperRight);
		
		Transform cashBox = UIThumbsticksObj.Find("UpperRight/Cash");
		UICashDisplay cashDisplay = cashBox.gameObject.AddComponent<UICashDisplay>();
		
		moveThumb = UIThumbsticksObj.Find("LowerLeft/MoveAnchor/MoveThumb");
		moveThumb.gameObject.AddComponent<UIElement>().setTarget(gameObject);
		
		throwThumb = UIThumbsticksObj.Find("LowerRight/ThrowAnchor/ThrowThumb");
		throwThumb.gameObject.AddComponent<UIElement>().setTarget(gameObject);
		
		throwAnchor = UIThumbsticksObj.Find("LowerRight/ThrowAnchor");
		throwAnchor.gameObject.AddComponent<UIBombStickBehavior>().setUp(this, throwThumb, throwAnchor.localPosition);
		
		shootThumb = UIThumbsticksObj.Find("LowerRight/ShootAnchor/ShootThumb");
		shootThumb.gameObject.AddComponent<UIElement>().setTarget(gameObject);
		
		shootAnchor = UIThumbsticksObj.Find("LowerRight/ShootAnchor");
		shootAnchor.gameObject.AddComponent<UIShootStickBehavior>().setUp(this, shootThumb, shootAnchor.localPosition);
				
		Transform inventoryPos = UIThumbsticksObj.Find("LowerRight/Inventory");
		inventory = GetComponent<InventoryController>();
		inventory.SetUp(inventoryPos, cashDisplay);
		
		//Transform centerSliderPos = UIThumbsticksObj.Find("LowerCenter/Inventory");
		///inventory = GetComponent<InventoryController>();
		//inventory.SetUp(inventoryPos, cashDisplay);
		
		setUp = true;
	}
	
	void Update () {
		
		if (!setUp) return;
		
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
		
		if (moveTouched) {
			moveThumb.localPosition = Vector3.Lerp(moveThumb.localPosition, movePosGoal, Time.deltaTime * 10.0f);
			moveThumb.localScale = Vector3.Lerp(moveThumb.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			moveThumb.localPosition = Vector3.Lerp(moveThumb.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			moveThumb.localScale = Vector3.Lerp(moveThumb.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}
		
		if (throwTouched) {
			throwThumb.localPosition = Vector3.Lerp(throwThumb.localPosition, throwPosGoal, Time.deltaTime * 10.0f);
			throwThumb.localScale = Vector3.Lerp(throwThumb.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			throwThumb.localPosition = Vector3.Lerp(throwThumb.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			throwThumb.localScale = Vector3.Lerp(throwThumb.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}
		
		if (shootTouched) {
			shootThumb.localPosition = Vector3.Lerp(shootThumb.localPosition, shootPosGoal, Time.deltaTime * 10.0f);
			shootThumb.localScale = Vector3.Lerp(shootThumb.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 10.0f);
		} else {
			shootThumb.localPosition = Vector3.Lerp(shootThumb.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
			shootThumb.localScale = Vector3.Lerp(shootThumb.localScale, Vector3.one, Time.deltaTime * 10.0f);
		}		
		playerController.moveInput(moveThumb.localPosition * 10.0f);
		playerController.throwInput(throwThumb.localPosition * 10.0f);
		playerController.shootInput(shootThumb.localPosition * 10.0f);
	}
	
	public void touchDown(TouchManager.TouchDownEvent touchEvent) {
		inventory.closeInventory();
			print ("moving");
		
		if (touchEvent.touchTarget.name.Equals("MoveThumb")) {
			moveTouched = true;
			playerController.setMoveInputOn();
		}
		if (touchEvent.touchTarget.name.Equals("ThrowThumb")) {
			throwTouched = true;
			playerController.setThrowInputOn();
		}
		
		if (touchEvent.touchTarget.name.Equals("ShootThumb")) {
			shootTouched = true;
			playerController.setShootInputOn();
		}			
		
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3 (touchEvent.touchPosition.x, touchEvent.touchPosition.y, Camera.main.nearClipPlane + 1.0f));
		if (touchEvent.startTarget.name.Equals("MoveThumb")) 
			movePosGoal = Vector3.ClampMagnitude(moveThumb.parent.InverseTransformPoint(worldPos), 0.1f);
		
		if (touchEvent.startTarget.name.Equals("ThrowThumb")) 
			throwPosGoal = Vector3.ClampMagnitude(throwThumb.parent.InverseTransformPoint(worldPos), 0.1f);
		
		if (touchEvent.startTarget.name.Equals("ShootThumb")) 
			shootPosGoal = Vector3.ClampMagnitude(shootThumb.parent.InverseTransformPoint(worldPos), 0.1f);
		
	}
		
	public void touchUp(TouchManager.TouchUpEvent touchEvent) {

		if (touchEvent.startTarget.name.Equals("MoveThumb")) {
			moveTouched = false;
			playerController.setMoveInputOff();
		}
		if (touchEvent.startTarget.name.Equals("ThrowThumb")) {
			throwTouched = false;
			playerController.setThrowInputOff();
		}		
		if (touchEvent.startTarget.name.Equals("ShootThumb")) {
			shootTouched = false;
			playerController.setShootInputOff();
		}		
	}
	
	
	public void tap(TouchManager.TapEvent touchEvent) {
	
	}

}