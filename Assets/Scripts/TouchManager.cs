using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
	
	public Vector2 lastTouchDownPos;
	
	
	public class TouchDownEvent {
		public Transform touchTarget;
		public Vector2 touchPosition;
		
		public TouchDownEvent(Transform touchTarget, Vector2 touchPosition) {
			this.touchTarget = touchTarget;
			this.touchPosition = touchPosition;
		}
	}
	
	public void touchDown(Transform touchTarget, Vector2 touchPosition) {
		if (!touchTarget) return;
		print ("touch down");
		lastTouchDownPos = touchPosition;
		touchTarget.BroadcastMessage("touchDown", new TouchDownEvent(touchTarget, touchPosition), SendMessageOptions.DontRequireReceiver);
	}
	
	
	public class TouchDragEvent {
		public Vector2 touchDelta;
		public Vector2 touchDistance;
		public Vector2 touchPosition;
		public Transform touchTarget;
		public Transform startTarget;
		
		public TouchDragEvent (Vector2 touchDelta, Vector2 touchDistance, Vector2 touchPosition, Transform touchTarget, Transform startTarget) {
			this.touchDelta = touchDelta;
			this.touchDistance = touchDistance;
			this.touchPosition = touchPosition;
			this.touchTarget = touchTarget;
			this.startTarget = startTarget;
		}
	}
	
	public void touchDrag(Vector2 touchDelta, Vector2 touchDistance, Vector2 touchPosition, Transform touchTarget, Transform startTarget) {
		if (!startTarget) return;
		startTarget.BroadcastMessage("drag", new TouchDragEvent(touchDelta, touchDistance, touchPosition, touchTarget, startTarget), SendMessageOptions.DontRequireReceiver);
	}
	
	public class TouchUpEvent {
		public Transform touchTarget;
		public Transform startTarget;
		public Vector2 touchPosition;
		public Vector2 touchDirection;
		public float touchTime;
		
		public TouchUpEvent(Transform touchTarget, Transform startTarget, Vector2 touchPosition, Vector2 touchDirection, float touchTime) {
			this.touchTarget = touchTarget;
			this.startTarget = startTarget;
			this.touchPosition = touchPosition;
			this.touchDirection = touchDirection;
			this.touchTime = touchTime;
		}
	}
	
	public void touchUp(Transform touchTarget, Transform startTarget, Vector2 touchPosition, Vector2 touchDirection, float touchTime) {
		if (!startTarget) return;
		startTarget.BroadcastMessage("touchUp", new TouchUpEvent(touchTarget, startTarget, touchPosition, touchDirection, touchTime), SendMessageOptions.DontRequireReceiver);
	
	}
	
	public class TapEvent {
		public Transform touchTarget;
		public Vector2 touchPosition;
		
		public TapEvent(Transform touchTarget, Vector2 touchPosition) {
			this.touchTarget = touchTarget;
			this.touchPosition = touchPosition;
		}
	}

	public void tap(Transform touchTarget, Vector2 touchPosition)  {
		if (!touchTarget) return;	
		touchTarget.BroadcastMessage("tap", new TapEvent(touchTarget, touchPosition), SendMessageOptions.DontRequireReceiver);
	}
	
	
	
	public void swipeLeft(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	public void sweepLeft() {
	
	}
	
	public void swipeRight(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	public void sweepRight() {
	
	}
	
	public void swipeDown(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	public void sweepDown() {
		
	}
	
	public void swipeUp(float touchTime, Transform startTarget, Transform endTarget) {
	
	}
	
	public void sweepUp() {
		
	}
	
	public void longTouched(Transform touchTarget) {
		if (!touchTarget) return;
	
	}
	
	public void backPressed() {
		Events.Send(gameObject, "KeysPressed", "Back");		
	}
	
	public void menuPressed() {
	
	}
}	