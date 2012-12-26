using UnityEngine;
using System.Collections;


public class UIElement : MonoBehaviour {

	GameObject target;
	
	void Start () {
		gameObject.AddComponent<SphereCollider>();
	
	}
	
	void Update () {
	
	}
	
	public void setTarget(GameObject newTarget) {
		target = newTarget;
	}
	
	void tap(TouchManager.TapEvent touchEvent) {
		target.SendMessage ("tap", touchEvent);
	}
	
	void touchDown(TouchManager.TouchDownEvent touchEvent) {
		target.SendMessage ("touchDown", touchEvent);
	}
	
	void drag(TouchManager.TouchDragEvent touchEvent) {
		target.SendMessage ("drag", touchEvent);
	}
		
	void touchUp(TouchManager.TouchUpEvent touchEvent) {
		target.SendMessage ("touchUp", touchEvent);
	}
	
}