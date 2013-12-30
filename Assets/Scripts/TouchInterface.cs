using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInterface : MonoBehaviour {
	
	public bool debugOn = false;
	public GUIText debugText;

	TouchManager touchManager;
		
	float tapTime = 0.25f;
	float gestureTime = 0.25f;
	float longTouchTime = 1.0f;
	float swipeDistance = 30.0f;
	
	public class InputData {	
		public enum InputPhase { Began, Moved, Ended, Hover, Other };
		public InputPhase phase = InputPhase.Hover;
		public int fingerId;
		public Vector2 startPosition;
		public Vector2 position;
		public Vector2 delta;
		public Vector2 distance;
		public float time;
		public bool gestureArmed;
		public Transform startTarget;
		
		public InputData() {
			
		}
	}
	
	List<InputData> currentInputSet = new List<InputData>();
	
	public InputData[] input = new InputData[10];
		
	void Start() {
		touchManager = GetComponent<TouchManager>();
		
		for (int i = 0; i < input.Length; i++) {
			input[i] = new InputData();
		}
	}
	
	void Update() {
		if (!Camera.main) return;
				
		if (debugOn) {
			if (!debugText) debugText = DebugText.Create(new Vector2(0.1f, 0.9f));
			debugText.text = "Touches =  " + Input.touches.Length + "\n";		
		}
				
		currentInputSet.Clear();
		
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
						
			for(int i = 0; i < Input.touches.Length; i++) {
				
				InputData inputData = new InputData();
				
				inputData.position = Input.touches[i].position;
				inputData.fingerId = Input.touches[i].fingerId;
				
				if (Input.touches[i].phase == TouchPhase.Began) {
					inputData.phase = InputData.InputPhase.Began;	
				} else if (Input.touches[i].phase == TouchPhase.Moved) {
					inputData.phase = InputData.InputPhase.Moved;	
				} else if (Input.touches[i].phase == TouchPhase.Stationary) {
					inputData.phase = InputData.InputPhase.Moved;					
				} else if (Input.touches[i].phase == TouchPhase.Ended) {
					inputData.phase = InputData.InputPhase.Ended;	
				} else {
					inputData.phase = InputData.InputPhase.Other;	
				}
				currentInputSet.Add(inputData);
			}
		} else {
			InputData inputData = new InputData();
			
			inputData.position = Input.mousePosition;
			inputData.fingerId = 0;
			
			if (Input.GetMouseButtonDown(0)) {
				inputData.phase = InputData.InputPhase.Began;	
			} else if (Input.GetMouseButton(0)) {
				inputData.phase = InputData.InputPhase.Moved;	
			} else if (Input.GetMouseButtonUp(0)) {
				inputData.phase = InputData.InputPhase.Ended;	
			} else {
				inputData.phase = InputData.InputPhase.Hover;	
			}
			currentInputSet.Add(inputData);
		}
		
		
		foreach (InputData currentTouch in currentInputSet) {	
			int id = currentTouch.fingerId;
	
			input[id].phase = currentTouch.phase;	
			input[id].delta = currentTouch.position - input[id].position;	
			input[id].position = currentTouch.position;

			RaycastHit hit;
			Physics.Raycast(Camera.main.ScreenPointToRay(input[id].position), out hit);
			
			// touch down effects
			if (input[id].phase == InputData.InputPhase.Began) {	
				input[id].startPosition = input[id].position;
				input[id].delta = Vector2.zero;
				input[id].distance = Vector2.zero;
				input[id].time = 0.0f;
				input[id].gestureArmed = true;
				input[id].startTarget = hit.transform;
				touchManager.touchDown(hit.transform, input[id].position);

			}
			
			// touch move effects
			if (input[id].phase == InputData.InputPhase.Moved) {	
		
				touchManager.touchDrag(input[id].delta, input[id].distance, input[id].position, hit.transform, input[id].startTarget);
				input[id].time += Time.deltaTime;
				input[id].distance += input[id].delta;
				// test for longtouch
				if (input[id].time > longTouchTime) {
					
					if (input[id].startTarget == hit.transform && input[id].distance.sqrMagnitude < 20) {
						touchManager.longTouched(input[id].startTarget);
						input[id].gestureArmed = false;
					} else {
						touchManager.longTouched(null);
					}
				}
				
				// turn off gestures
				if (input[id].time > gestureTime) input[id].gestureArmed = false;
			}
				
			//touch up effects
			if (input[id].phase == InputData.InputPhase.Ended) {
				
				touchManager.touchUp(hit.transform, input[id].startTarget, input[id].position, input[id].distance, input[id].time);
				
				if (input[id].gestureArmed) {
					if (input[id].time < tapTime) touchManager.tap(hit.transform, input[id].position);
					
					if (input[id].distance.x < -swipeDistance) {
						
						if (currentInputSet.Count > 2) {
							touchManager.sweepLeft();
							return;	
						}
						touchManager.swipeLeft(input[id].time, input[id].startTarget, hit.transform);
					}
					if (input[id].distance.x > swipeDistance) {
						if (currentInputSet.Count > 2) {
							touchManager.sweepRight();
							return;	
						}
						touchManager.swipeRight(input[id].time, input[id].startTarget, hit.transform);
					}
					if (input[id].distance.y < -swipeDistance) {
						if (currentInputSet.Count > 2) {
							touchManager.sweepDown();
							return;	
						}						
						touchManager.swipeDown(input[id].time, input[id].startTarget, hit.transform);
					}
					if (input[id].distance.y > swipeDistance) {
						if (currentInputSet.Count > 2) {
							touchManager.sweepUp();
							return;	
						}						
						touchManager.swipeUp(input[id].time, input[id].startTarget, hit.transform);
					}
				}
				input[id].startTarget = null;
			}
		}
		if (debugOn) {
			for (int i = 0; i < input.Length; i++) {
				debugText.text += "Touch " + input[i].fingerId + "\n";
				debugText.text += "  Phase " + input[i].phase + "\n";
				debugText.text += "  position " + input[i].position + "\n";
				debugText.text += "  startPos " + input[i].startPosition + "\n";
				debugText.text += "  delta " + input[i].delta + "\n";
				debugText.text += "  distance " + input[i].distance + "\n";
				debugText.text += "  time " + input[i].time + "\n";

				if (input[i].startTarget) {
					debugText.text += "  startTarget " + input[i].startTarget.name;
				} else {
					debugText.text += "  not touching anything";
				}
				debugText.text += "\n";
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