using UnityEngine;
using System.Collections;

public class MiniMapDisplay : MonoBehaviour {
	
	Transform upperLeft;
	Transform upperRight;
	Transform lowerLeft;
	Transform lowerRight;	
	Transform centerMiniMap;	
	Transform zoomHandle;	
	Transform resizeButton;	
	BoxCollider miniMapCollision;
	
	Vector3 lowerLeftStart, lowerLeftEnd;
	Vector3 upperLeftStart, upperLeftEnd;
	Vector3 lowerRightStart, lowerRightEnd;
	Vector3 upperRightStart, upperRightEnd;

	public int currentState;
	
	float transTimer = 1.1f;
	
	Camera miniMapCamera;
	MiniMapControl miniMapControl;
	
	public void InitMiniMap() {
		
		InputRepeater repeater;
		
		//lowerleft acts as the whole map
		lowerLeft = transform.Find("FrameLowerLeft");
		miniMapCollision = lowerLeft.gameObject.AddComponent<BoxCollider>();
		repeater = lowerLeft.gameObject.AddComponent<InputRepeater>();
		repeater.SetTarget(transform);
		
		//upper left is nothing
		upperLeft = transform.Find("FrameLowerLeft/FrameUpperLeft");
		
		//upper right is nothing
		upperRight = transform.Find("FrameLowerLeft/FrameUpperRight");
		
		//lower right is nothing
		lowerRight = transform.Find("FrameLowerLeft/FrameLowerRight");
		
		resizeButton = transform.Find("FrameLowerLeft/FrameLowerRight/Resize");
		resizeButton.gameObject.AddComponent<BoxCollider>();
		repeater = resizeButton.gameObject.AddComponent<InputRepeater>();
		repeater.SetTarget(transform);
		
		centerMiniMap = transform.Find("FrameLowerLeft/Centered");
		centerMiniMap.gameObject.AddComponent<BoxCollider>();
		repeater = centerMiniMap.gameObject.AddComponent<InputRepeater>();
		repeater.SetTarget(transform);
		
		zoomHandle = transform.Find("FrameLowerLeft/FrameUpperRight/ZoomHandle");
		zoomHandle.gameObject.AddComponent<BoxCollider>();
		repeater = zoomHandle.gameObject.AddComponent<InputRepeater>();
		repeater.SetTarget(transform);
		
		miniMapCamera = GameObject.FindGameObjectWithTag("MiniMapCamera").camera;
		miniMapControl = miniMapCamera.transform.parent.GetComponent<MiniMapControl>();
		
		Resize();
	}
	
	void Update () {
		
		if (transTimer < 1.0f) {
			transTimer = Mathf.Clamp01(transTimer + (GameTime.deltaTime * 3));
		
			float lerpAmount = miniMapControl.transitionCurve.Evaluate(transTimer);
			
			// update minimap size
			lowerLeft.localPosition = Vector3.Lerp(lowerLeftStart, lowerLeftEnd, lerpAmount);
			upperLeft.localPosition = Vector3.Lerp(upperLeftStart, upperLeftEnd, lerpAmount);
			lowerRight.localPosition = Vector3.Lerp(lowerRightStart, lowerRightEnd, lerpAmount);
			upperRight.localPosition = Vector3.Lerp(upperRightStart, upperRightEnd, lerpAmount);
			
			// update minimap viewport
			Vector4 viewportRect;
			viewportRect.x = GetScreenPos(lowerLeft).x;
			viewportRect.y = GetScreenPos(lowerLeft).y;
			viewportRect.z = GetScreenPos(lowerRight).x - GetScreenPos(lowerLeft).x;
			viewportRect.w = GetScreenPos(upperLeft).y - GetScreenPos(lowerLeft).y;
			miniMapCamera.rect = new Rect(viewportRect.x,viewportRect.y,viewportRect.z,viewportRect.w);
			
			//update minimap collision
			miniMapCollision.center = Vector3.Lerp(upperLeft.localPosition, lowerRight.localPosition, 0.5f);
			miniMapCollision.size = new Vector3(
				upperLeft.localPosition.x - upperRight.localPosition.x, 
				upperRight.localPosition.y - lowerRight.localPosition.y, 
				0.01f);
			
		}
		
		//manage minimap buttons
		if (miniMapControl.IsTracking()) {
			centerMiniMap.renderer.enabled = false;
		} else {
			centerMiniMap.renderer.enabled = true;
		}
		
		Vector3 zoomPos = zoomHandle.localPosition;
		zoomPos.y = Mathf.Lerp(-0.05f, -(upperRight.localPosition.y - 0.05f), miniMapControl.zoomLevel);
		zoomHandle.localPosition = zoomPos;
	}
	
	void Resize() {
		
		currentState++;
		if (currentState > 2) currentState = 0;
		
		lowerLeftStart = lowerLeft.localPosition;
		upperLeftStart = upperLeft.localPosition; 
		lowerRightStart = lowerRight.localPosition;
		upperRightStart = upperRight.localPosition;
		
		switch (currentState) {
			
		case 0 :
			lowerLeftEnd = new Vector3(-0.01f, -0.11f, 0.0f);
			upperLeftEnd = new Vector3(0.0f, 0.1f, 0.0f);
			lowerRightEnd = new Vector3(-0.1f, 0.0f, 0.0f);
			upperRightEnd = new Vector3(-0.1f, 0.1f, 0.0f);
			break;
		case 1 :
			lowerLeftEnd = new Vector3(-0.01f, -0.31f, 0.0f);
			upperLeftEnd = new Vector3(0.0f, 0.3f, 0.0f);
			lowerRightEnd = new Vector3(-0.3f, 0.0f, 0.0f);
			upperRightEnd = new Vector3(-0.3f, 0.3f, 0.0f);
			break;		
		case 2 :
			lowerLeftEnd = new Vector3(-0.01f, -0.51f, 0.0f);
			upperLeftEnd = new Vector3(0.0f, 0.5f, 0.0f);
			lowerRightEnd = new Vector3(-0.9f, 0.0f, 0.0f);
			upperRightEnd = new Vector3(-0.9f, 0.5f, 0.0f);
			break;				
		default :
			lowerLeftEnd = new Vector3(-0.01f, -0.11f, 0.0f);
			upperLeftEnd = new Vector3(0.0f, 0.1f, 0.0f);
			lowerRightEnd = new Vector3(-0.1f, 0.0f, 0.0f);
			upperRightEnd = new Vector3(-0.1f, 0.1f, 0.0f);
			break;
		}	
		
		transTimer = 0;
	}
	
	Vector2 GetScreenPos(Transform newObject) {
		Vector3 screenPos = Camera.main.WorldToScreenPoint(newObject.position);
		
		Vector2 normalizedScreenPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
		
		return normalizedScreenPos;
		
	}

	public void tap(TouchManager.TapEvent touchEvent) {
		
		
		if (touchEvent.touchTarget == centerMiniMap) {
			miniMapControl.CenterMiniMap(); 
		}

		if (touchEvent.touchTarget == resizeButton) {
			Resize(); 
		}
		
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		if (touchEvent.startTarget == lowerLeft) {
			miniMapControl.Scroll(touchEvent.touchDelta); 
		}
		if (touchEvent.startTarget == zoomHandle) {
			miniMapControl.AdjustZoomLevel(-touchEvent.touchDelta.y * 0.01f); 
		}
	}
}
