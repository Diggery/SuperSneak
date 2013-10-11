using UnityEngine;
using System.Collections;

public class TouchInterface : MonoBehaviour {

	TouchManager touchManager;
			
	Vector3[] touchPosition = new Vector3[10];
	
	Vector2[] touchDistance = new Vector2[10];
	float[] touchTime = new float[10];
	bool[] gestureArmed = new bool[10];
	
	Transform[] startTarget = new Transform[10];
	
	float tapTime = 0.25f;
	float gestureTime = 0.25f;
	float longTouchTime = 1.0f;
	float swipeDistance = 30.0f;
	
	void Start() {
		touchManager = GetComponent<TouchManager>();
		
	}
	
	void Update() {
		
		UIManager debugUI = Camera.main.gameObject.GetComponent<UIManager>();
		
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Vector3 lastTouchPosition = Vector3.zero;
		Vector2 touchDelta = Vector2.zero;
		int touchId = 9;

		
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) {

			lastTouchPosition = touchPosition[touchId];
			touchPosition[touchId] = Input.mousePosition;
			touchDelta.x = touchPosition[touchId].x - lastTouchPosition.x;
			touchDelta.y = touchPosition[touchId].y - lastTouchPosition.y;
			touchPosition[touchId].z = Camera.main.transform.position.z;
			
			
			if (Input.GetMouseButtonDown(0)) {
				touchDelta = Vector2.zero;
				touchDistance[touchId] = Vector2.zero;
				touchTime[touchId] = 0;
				Physics.Raycast(ray, out hit);
				touchManager.touchDown(hit.transform, touchPosition[touchId], touchId);
				startTarget[touchId] = hit.transform;
				gestureArmed[touchId] = true;
			}
		
			if (Input.GetMouseButton(0)) {
				Physics.Raycast(ray, out hit);
				touchManager.touchDrag(touchDelta, touchDistance[touchId], touchPosition[touchId], hit.transform, startTarget[touchId], touchId);
				touchDistance[touchId] += touchDelta;
				touchTime[touchId] += GameTime.unpausedDeltaTime;
				if (gestureArmed[touchId]) {
					if (touchTime[touchId] > longTouchTime) {
						if (startTarget[touchId] == hit.transform) {
							touchManager.longTouched(startTarget[touchId], touchId);
						}
						gestureArmed[touchId] = false;
					}
				}
		
			}
				
			if (Input.GetMouseButtonUp(0)) {
				Physics.Raycast(ray, out hit);
				//print("touchUp");
				touchManager.touchUp(hit.transform, startTarget[touchId], touchPosition[touchId], normalizedDistance(touchDistance[touchId]), touchTime[touchId], touchId);
				if (touchDistance[touchId].x < -swipeDistance && gestureArmed[touchId]) {
					touchManager.swipeLeft(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
				}
				if (touchDistance[touchId].x > swipeDistance && gestureArmed[touchId]) {
					touchManager.swipeRight(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
				}
				if (touchDistance[touchId].y < -swipeDistance && gestureArmed[touchId]) {
					touchManager.swipeDown(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
				}
				if (touchDistance[touchId].y > swipeDistance && gestureArmed[touchId]) {
					touchManager.swipeUp(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
				}
				startTarget[touchId] = null;
				if (touchTime[touchId] < tapTime) {
					touchManager.tap(hit.transform, touchPosition[touchId], touchId);
				}
				gestureArmed[touchId] = false;
	
			}	
	
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
	
			Touch[] touchInput = Input.touches;
			
					debugUI.debugString = "Touches are " + touchInput.Length;
			
		
			for (int i = 0; i < touchInput.Length; i++) {
				
				if (i > 9) {
					touchId = 9;
				} else {
					touchId = i;
				}
				
				Touch touch = touchInput[touchId];
				ray = Camera.main.ScreenPointToRay(touch.position);
		
				lastTouchPosition = touchPosition[touchId];
				touchPosition[touchId] = touch.position;
	
				touchDelta.x = touchPosition[touchId].x - lastTouchPosition.x;
				touchDelta.y = touchPosition[touchId].y - lastTouchPosition.y;
				touchPosition[touchId].z = Camera.main.transform.position.z;
					
		
				if (touch.phase == TouchPhase.Began) {	
					touchDelta = Vector2.zero;
					touchDistance[touchId] = Vector2.zero;
					touchTime[touchId] = 0;
					Physics.Raycast(ray, out hit);
					touchManager.touchDown(hit.transform, touchPosition[touchId], touchId);
					startTarget[touchId] = hit.transform;
					gestureArmed[touchId] = true;
				}
			
				if (touchInput.Length > 0) {
					Physics.Raycast(ray, out hit);
					touchManager.touchDrag(touchDelta, touchDistance[touchId], touchPosition[touchId], hit.transform, startTarget[touchId], touchId);
					touchDistance[touchId] += touchDelta;
					touchTime[touchId] += GameTime.unpausedDeltaTime;
					
					if (gestureArmed[touchId]) {
						if (touchTime[touchId] > gestureTime) {
							if (startTarget[touchId] == hit.transform && (touchDistance[touchId].x + touchDistance[touchId].y) < 5) {
								touchManager.longTouched(startTarget[touchId], touchId);
								gestureArmed[touchId] = false;
							} else {
								touchManager.longTouched(null, touchId);
							}
							gestureArmed[touchId] = false;
						}
					}
				}
					
				if (touch.phase == TouchPhase.Ended) {	
					Physics.Raycast(ray, out hit);
					touchManager.touchUp(hit.transform, startTarget[touchId], touchPosition[touchId], touchDistance[touchId], touchTime[touchId], touchId);
	
					if (gestureArmed[touchId]) {
						touchManager.tap(hit.transform, touchPosition[touchId], touchId);
						if (touchDistance[touchId].x < -swipeDistance) {
							
							if (touchInput.Length > 2) {
							//	touchManager.sweepLeft();
								return;	
							}
							touchManager.swipeLeft(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
						}
						if (touchDistance[touchId].x > swipeDistance) {
							if (touchInput.Length > 2) {
							//	touchManager.sweepRight();
								return;	
							}
							touchManager.swipeRight(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
						}
						if (touchDistance[touchId].y < -swipeDistance) {
							if (touchInput.Length > 2) {
						//		touchManager.sweepDown();
								return;	
							}						
							touchManager.swipeDown(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
						}
						if (touchDistance[touchId].y > swipeDistance) {
							if (touchInput.Length > 2) {
							//	touchManager.sweepUp();
								return;	
							}						
							touchManager.swipeUp(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
						}
					}
					startTarget[touchId] = null;
					gestureArmed[touchId] = false;
				}
			}
		}
		
		if (Input.GetKeyUp(KeyCode.Escape)) {
			touchManager.backPressed();
		}
		if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.Menu)) {
			touchManager.menuPressed();
		}
	
	}
	
	
	Vector2 normalizedDistance(Vector2 distance) {
		Vector2 newDistance = new Vector2((distance.x/Screen.width), (distance.y/Screen.height));
		return newDistance;
	}
}