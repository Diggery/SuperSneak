using UnityEngine;
using System.Collections;

public class TouchInterface : MonoBehaviour {

	TouchManager touchManager;
			
	Vector3 touchPosition ;
	
	Vector2 touchDistance;
	float touchTime;
	bool gestureArmed;
	
	Transform startTarget;
	
	float tapTime = 0.25f;
	float gestureTime = 0.25f;
	float longTouchTime = 1.0f;
	float swipeDistance = 30.0f;
	
	void Start() {
		touchManager = GameObject.Find("TouchControl").GetComponent<TouchManager>();
		
	}
	
	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Vector3 lastTouchPosition = Vector3.zero;
		Vector2 touchDelta = Vector2.zero;
		
		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) {

			lastTouchPosition = touchPosition;
			touchPosition = Input.mousePosition;
			touchDelta.x = touchPosition.x - lastTouchPosition.x;
			touchDelta.y = touchPosition.y - lastTouchPosition.y;
			touchPosition.z = Camera.main.transform.position.z;
			
			
			if (Input.GetMouseButtonDown(0)) {
				touchDelta = Vector2.zero;
				touchDistance = Vector2.zero;
				touchTime = 0;
				Physics.Raycast(ray, out hit);
				touchManager.touchDown(hit.transform, touchPosition);
				startTarget = hit.transform;
				gestureArmed = true;
			}
		
			if (Input.GetMouseButton(0)) {
				Physics.Raycast(ray, out hit);
				touchManager.touchDrag(touchDelta, touchDistance, touchPosition, hit.transform, startTarget);
				touchDistance += touchDelta;
				touchTime += Time.deltaTime;
				if (gestureArmed) {
					if (touchTime > longTouchTime) {
						if (startTarget == hit.transform) {
							touchManager.longTouched(startTarget);
						}
						gestureArmed = false;
					}
				}
		
			}
				
			if (Input.GetMouseButtonUp(0)) {
				Physics.Raycast(ray, out hit);
				//print("touchUp");
				touchManager.touchUp(hit.transform, startTarget, touchPosition, normalizedDistance(touchDistance), touchTime);
				if (touchDistance.x < -swipeDistance && gestureArmed) {
					touchManager.swipeLeft(touchTime, startTarget, hit.transform);
				}
				if (touchDistance.x > swipeDistance && gestureArmed) {
					touchManager.swipeRight(touchTime, startTarget, hit.transform);
				}
				if (touchDistance.y < -swipeDistance && gestureArmed) {
					touchManager.swipeDown(touchTime, startTarget, hit.transform);
				}
				if (touchDistance.y > swipeDistance && gestureArmed) {
					touchManager.swipeUp(touchTime, startTarget, hit.transform);
				}
				startTarget = null;
				if (touchTime < tapTime) {
					touchManager.tap(hit.transform, touchPosition);
				}
				gestureArmed = false;
	
			}	
	
		} else {
	
			Touch[] touchInput = Input.touches;
		
			if (touchInput.Length > 0) {	
				Touch touch = touchInput[0];
				ray = Camera.main.ScreenPointToRay(touch.position);
		
				lastTouchPosition = touchPosition;
				touchPosition = touch.position;
	
				touchDelta.x = touchPosition.x - lastTouchPosition.x;
				touchDelta.y = touchPosition.y - lastTouchPosition.y;
				touchPosition.z = Camera.main.transform.position.z;
					
		
				if (touch.phase == TouchPhase.Began) {	
					touchDelta = Vector2.zero;
					touchDistance = Vector2.zero;
					touchTime = 0;
					Physics.Raycast(ray, out hit);
					touchManager.touchDown(hit.transform, touchPosition);
					startTarget = hit.transform;
					gestureArmed = true;
				}
			
				if (touchInput.Length > 0) {
					Physics.Raycast(ray, out hit);
					touchManager.touchDrag(touchDelta, touchDistance, touchPosition, hit.transform, startTarget);
					touchDistance += touchDelta;
					touchTime += Time.deltaTime;
					
					if (gestureArmed) {
						if (touchTime > gestureTime) {
							if (startTarget == hit.transform && (touchDistance.x + touchDistance.y) < 5) {
								touchManager.longTouched(startTarget);
								gestureArmed = false;
							} else {
								touchManager.longTouched(null);
							}
							gestureArmed = false;
						}
					}
				}
					
				if (touch.phase == TouchPhase.Ended) {	
					Physics.Raycast(ray, out hit);
					touchManager.touchUp(hit.transform, startTarget, touchPosition, touchDistance, touchTime);
	
					if (gestureArmed) {
						touchManager.tap(hit.transform, touchPosition);
						if (touchDistance.x < -swipeDistance) {
							
							if (touchInput.Length > 2) {
							//	touchManager.sweepLeft();
								return;	
							}
							touchManager.swipeLeft(touchTime, startTarget, hit.transform);
						}
						if (touchDistance.x > swipeDistance) {
							if (touchInput.Length > 2) {
							//	touchManager.sweepRight();
								return;	
							}
							touchManager.swipeRight(touchTime, startTarget, hit.transform);
						}
						if (touchDistance.y < -swipeDistance) {
							if (touchInput.Length > 2) {
						//		touchManager.sweepDown();
								return;	
							}						
							touchManager.swipeDown(touchTime, startTarget, hit.transform);
						}
						if (touchDistance.y > swipeDistance) {
							if (touchInput.Length > 2) {
							//	touchManager.sweepUp();
								return;	
							}						
							touchManager.swipeUp(touchTime, startTarget, hit.transform);
						}
					}
					startTarget = null;
					gestureArmed = false;
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