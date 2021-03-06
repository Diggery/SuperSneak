using UnityEngine;
using System.Collections;

public class InputRepeater : MonoBehaviour {
	
	public Transform target;
	

	public void tap(TouchManager.TapEvent touchEvent) {
		if (touchEvent.touchTarget != transform) return;
		Transform inputTarget = target ? target : transform.root;

		inputTarget.SendMessage("tap", touchEvent, SendMessageOptions.DontRequireReceiver);
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		if (touchEvent.startTarget != transform) return;
		Transform inputTarget = target ? target : transform.root;
		
		inputTarget.SendMessage("drag", touchEvent, SendMessageOptions.DontRequireReceiver);
	}

	public void touchUp(TouchManager.TouchUpEvent touchEvent) {
		if (touchEvent.startTarget != transform) return;
		Transform inputTarget = target ? target : transform.root;
		
		inputTarget.SendMessage("touchUp", touchEvent, SendMessageOptions.DontRequireReceiver);
	}

		
	public void SetTarget(Transform newTarget) {
		target = newTarget;
	}	
	
}


