using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
	
	public Vector3 lastTouchDownPos;
	
	
	public class TouchDownEvent {
		public Transform touchTarget;
		public Vector3 touchPosition;
		public int touchId;
		
		public TouchDownEvent(Transform touchTarget, Vector3 touchPosition, int touchId) {
			this.touchTarget = touchTarget;
			this.touchPosition = touchPosition;
			this.touchId = touchId;
		}
	}
	
	public void touchDown(Transform touchTarget, Vector3 touchPosition, int touchId) {
		if (!touchTarget) return;
		lastTouchDownPos = touchPosition;
		touchTarget.BroadcastMessage("touchDown", new TouchDownEvent(touchTarget, touchPosition, touchId), SendMessageOptions.DontRequireReceiver);
	}
	
	
	public class TouchDragEvent {
		public Vector2 touchDelta;
		public Vector2 touchDistance;
		public Vector3 touchPosition;
		public Transform touchTarget;
		public Transform startTarget;
		public int touchId;
		
		public TouchDragEvent (Vector2 touchDelta, Vector2 touchDistance, Vector3 touchPosition, Transform touchTarget, Transform startTarget, int touchId) {
			this.touchDelta = touchDelta;
			this.touchDistance = touchDistance;
			this.touchPosition = touchPosition;
			this.touchTarget = touchTarget;
			this.startTarget = startTarget;
			this.touchId = touchId;
		}
	}
	
	public void touchDrag(Vector2 touchDelta, Vector2 touchDistance, Vector3 touchPosition, Transform touchTarget, Transform startTarget, int touchId) {
		if (!startTarget) return;
		startTarget.BroadcastMessage("drag", new TouchDragEvent(touchDelta, touchDistance, touchPosition, touchTarget, startTarget, touchId), SendMessageOptions.DontRequireReceiver);
	}
	
	public class TouchUpEvent {
		public Transform touchTarget;
		public Transform startTarget;
		public Vector3 touchPosition;
		public Vector3 touchDirection;
		public float touchTime;
		public int touchId;
		
		public TouchUpEvent(Transform touchTarget, Transform startTarget, Vector3 touchPosition, Vector2 touchDirection, float touchTime, int touchId) {
			this.touchTarget = touchTarget;
			this.startTarget = startTarget;
			this.touchPosition = touchPosition;
			this.touchDirection = touchDirection;
			this.touchTime = touchTime;
			this.touchId = touchId;
		}
	}
	
	public void touchUp(Transform touchTarget, Transform startTarget, Vector3 touchPosition, Vector2 touchDirection, float touchTime, int touchId) {
		if (!startTarget) return;
		startTarget.BroadcastMessage("touchUp", new TouchUpEvent(touchTarget, startTarget, touchPosition, touchDirection, touchTime, touchId), SendMessageOptions.DontRequireReceiver);
	
	}
	
	public class TapEvent {
		public Transform touchTarget;
		public Vector3 touchPosition;
		public int touchId;
		
		public TapEvent(Transform touchTarget, Vector3 touchPosition, int touchId) {
			this.touchTarget = touchTarget;
			this.touchPosition = touchPosition;
			this.touchId = touchId;
		}
	}

	public void tap(Transform touchTarget, Vector3 touchPosition, int touchId)  {
		if (!touchTarget) return;	
		touchTarget.BroadcastMessage("tap", new TapEvent(touchTarget, touchPosition, touchId), SendMessageOptions.DontRequireReceiver);
	}
	
	
	
	public void swipeLeft(float touchTime, Transform startTarget, Transform endTarget, int touchId) {
	
	}
	
	public void sweepLeft() {
	
	}
	
	public void swipeRight(float touchTime, Transform startTarget, Transform endTarget, int touchId) {
	
	}
	
	public void sweepRight() {
	
	}
	
	public void swipeDown(float touchTime, Transform startTarget, Transform endTarget, int touchId) {
	
	}
	
	public void sweepDown() {
		
	}
	
	public void swipeUp(float touchTime, Transform startTarget, Transform endTarget, int touchId) {
	
	}
	
	public void sweepUp() {
		
	}
	
	public void longTouched(Transform touchTarget, int touchId) {
		if (!touchTarget) return;
	
	}
	
	public void backPressed() {
		Events.Send(gameObject, "KeysPressed", "Back");		
	}
	
	public void menuPressed() {
	
	}
}	