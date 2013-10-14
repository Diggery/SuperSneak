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
	
	
	
	public class InputData {	
		public Vector2 position;
		public enum InputPhase { Began, Moved, Ended, Other };
		public InputPhase phase;
		
		public InputData() {
			
		}
	}
	
	public InputData[] inputSet;

		
	void Start() {
		touchManager = GetComponent<TouchManager>();
	 
		
	}
	
	void Update() {
		if (!Camera.main) return;
		
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Vector3 lastTouchPosition = Vector3.zero;
		Vector2 touchDelta = Vector2.zero;
		
		
		if (Application.platform == RuntimePlatform.OSXEditor) {
			
			inputSet = new InputData[1];
			
			InputData inputData = new InputData();
			inputSet[0] = inputData;
			
			inputSet[0].position = Input.mousePosition;
			
			if (Input.GetMouseButtonDown(0)) {
				inputSet[0].phase = InputData.InputPhase.Began;	
			} else if (Input.GetMouseButton(0)) {
				inputSet[0].phase = InputData.InputPhase.Moved;	
			} else if (Input.GetMouseButtonUp(0)) {
				inputSet[0].phase = InputData.InputPhase.Ended;	
			} else {
				inputSet[0].phase = InputData.InputPhase.Other;	
			}
			
		} else {
						
			inputSet = new InputData[Input.touches.Length];
			

			
			inputSet = new InputData[Input.touches.Length];
			
			for(int i = 0; i < Input.touches.Length; i++) {
				InputData inputData = new InputData();
				
				inputSet[i] = inputData;
				
				inputSet[i].position = Input.touches[i].position;
				
				if (Input.touches[i].phase == TouchPhase.Began) {
					inputSet[i].phase = InputData.InputPhase.Began;	
				} else if (Input.touches[i].phase == TouchPhase.Moved) {
					inputSet[i].phase = InputData.InputPhase.Moved;	
				} else if (Input.touches[i].phase == TouchPhase.Ended) {
					inputSet[i].phase = InputData.InputPhase.Ended;	
				} else {
					inputSet[i].phase = InputData.InputPhase.Other;	
				}
			}
		}
		
		for (int touchId = 0; touchId < inputSet.Length; touchId++) {	
			
			if (touchId > 9) break;
			
			InputData touch = inputSet[touchId];
			ray = Camera.main.ScreenPointToRay(touch.position);
	
			lastTouchPosition = touchPosition[touchId];
			touchPosition[touchId] = touch.position;

			touchDelta.x = touchPosition[touchId].x - lastTouchPosition.x;
			touchDelta.y = touchPosition[touchId].y - lastTouchPosition.y;
			touchPosition[touchId].z = Camera.main.transform.position.z;
				
			Physics.Raycast(ray, out hit);
			
			
			// touch down effects
			if (touch.phase == InputData.InputPhase.Began) {	
				
				touchDelta = Vector2.zero;
				touchDistance[touchId] = Vector2.zero;
				touchTime[touchId] = 0;
				touchManager.touchDown(hit.transform, touchPosition[touchId], touchId);
				startTarget[touchId] = hit.transform;
				gestureArmed[touchId] = true;
			}
			
			// touch move effects
			if (touch.phase == InputData.InputPhase.Moved ||touch.phase == InputData.InputPhase.Other) {	
		
				touchManager.touchDrag(touchDelta, touchDistance[touchId], touchPosition[touchId], hit.transform, startTarget[touchId], touchId);
				touchDistance[touchId] += touchDelta;
				touchTime[touchId] += GameTime.unpausedDeltaTime;
				
				
				// test for longtouch
				if (touchTime[touchId] > longTouchTime) {
					
					if (startTarget[touchId] == hit.transform && touchDistance[touchId].sqrMagnitude < 20) {
						touchManager.longTouched(startTarget[touchId], touchId);
						gestureArmed[touchId] = false;
					} else {
						touchManager.longTouched(null, touchId);
					}
				}
				
				// turn off gestures
				if (touchTime[touchId] > gestureTime) gestureArmed[touchId] = false;
			}
				
			//touch up effects
			if (touch.phase == InputData.InputPhase.Ended) {
				
				touchManager.touchUp(hit.transform, startTarget[touchId], touchPosition[touchId], touchDistance[touchId], touchTime[touchId], touchId);
				
				if (gestureArmed[touchId]) {
					if (touchTime[touchId] < tapTime) touchManager.tap(hit.transform, touchPosition[touchId], touchId);
					
					if (touchDistance[touchId].x < -swipeDistance) {
						
						if (inputSet.Length > 2) {
							touchManager.sweepLeft();
							return;	
						}
						touchManager.swipeLeft(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
					}
					if (touchDistance[touchId].x > swipeDistance) {
						if (inputSet.Length > 2) {
							touchManager.sweepRight();
							return;	
						}
						touchManager.swipeRight(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
					}
					if (touchDistance[touchId].y < -swipeDistance) {
						if (inputSet.Length > 2) {
							touchManager.sweepDown();
							return;	
						}						
						touchManager.swipeDown(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
					}
					if (touchDistance[touchId].y > swipeDistance) {
						if (inputSet.Length > 2) {
							touchManager.sweepUp();
							return;	
						}						
						touchManager.swipeUp(touchTime[touchId], startTarget[touchId], hit.transform, touchId);
					}
				}
				startTarget[touchId] = null;
				gestureArmed[touchId] = false;
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