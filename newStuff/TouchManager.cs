using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
	
	Vector3 lastTouchDownPos;
	
	
	class TouchDownEvent {
		Transform touchTarget;
		Vector3 touchPosition;
		
		void touchDownEvent(Transform touchTarget, Vector3 touchPosition) {
			this.touchTarget = touchTarget;
			this.touchPosition = touchPosition;
		}
	}
	
	void touchDown(Transform touchTarget, Vector3 touchPosition) {
		if (!touchTarget) return;
		lastTouchDownPos = touchPosition;
		touchTarget.BroadcastMessage("touchDown", new TouchDownEvent(touchTarget, touchPosition), SendMessageOptions.DontRequireReceiver);
	}
	
	
	class TouchDragEvent {
		Vector2 touchDelta;
		Vector2 touchDistance;
		Vector3 touchPosition;
		Transform touchTarget;
		Transform startTarget;
		
		void touchDragEvent (Vector2 touchDelta, Vector2 touchDistance, Vector3 touchPosition, Transform touchTarget, Transform startTarget) {
			this.touchDelta = touchDelta;
			this.touchDistance = touchDistance;
			this.touchPosition = touchPosition;
			this.touchTarget = touchTarget;
			this.startTarget =startTarget;
		}
	}
	
	function touchDrag(Vector2 touchDelta, Vector2 touchDistance, Vector3 touchPosition, Transform touchTarget, Transform startTarget) {
		if (!startTarget) return;
		startTarget.BroadcastMessage('drag', new TouchDragEvent(touchDelta, touchDistance, touchPosition, touchTarget, startTarget), SendMessageOptions.DontRequireReceiver);
	}
	
	class TouchUpEvent {
		Transform touchTarget;
		Transform startTarget;
		Vector3 touchPosition;
		Vector3 touchDirection;
		float touchTime;
		
		void touchUpEvent(Transform touchTarget, Transform startTarget, Vector3 touchPosition, Vector2 touchDirection, float touchTime) {
			this.touchTarget = touchTarget;
			this.startTarget = startTarget;
			this.touchPosition = touchPosition;
			this.touchDirection = touchDirection;
			this.touchTime = touchTime;
		}
	}
	
	function touchUp(Transform touchTarget, Transform startTarget, Vector3 touchPosition, Vector2 touchDirection, float touchTime) {
		if (!startTarget) return;
		startTarget.BroadcastMessage('touchUp', new TouchUpEvent(touchTarget, startTarget, touchPosition, touchDirection, touchTime), SendMessageOptions.DontRequireReceiver);
	
	}
	
	class TapEvent {
		Transform touchTarget;
		Vector3 touchPosition;
		
		void tapEvent(Transform touchTarget, Vector3 touchPosition) {
			this.touchTarget = touchTarget;
			this.touchPosition = touchPosition;
		}
	}
	
	
	
	
	function tap(Transform touchTarget, Vector3 touchPosition)  {
		if (!touchTarget) return;	
		touchTarget.BroadcastMessage('tap', new TapEvent(touchTarget, touchPosition), SendMessageOptions.DontRequireReceiver);
	}
	
	
	
	void swipeLeft(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	void swipeRight(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	void swipeDown(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	void swipeUp(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	void longTouched(Transform touchTarget) {
		if (!touchTarget) return;
	
	}
	
	function backPressed() {
	}
	
	function menuPressed() {
	
	}
}	