using UnityEngine;
using System.Collections;

public class FadeTrigger : MonoBehaviour {
	
	bool triggered;
	
	Transform target;
	
	void Update () {
		if (!target) target = transform;
		
		if (triggered) {
			if (target.renderer.material.color.a > 0.01) {
				target.renderer.enabled = true;
				target.renderer.material.color = Color.Lerp(target.renderer.material.color, Color.clear, Time.deltaTime * 5);	
			} else {
				target.renderer.enabled = false;
			}
		} else {
			target.renderer.enabled = true;
			
			if (target.renderer.material.color.a < 0.99) {
				target.renderer.material.color = Color.Lerp(target.renderer.material.color, Color.white, Time.deltaTime * 5);	
			} else {
				target.renderer.material.color = Color.white;
			}
		}
	}
	
	public void OnTriggerEnter(Collider other) {
		if (other.transform.tag == "Player") triggerFade(true);
	}
	
	public void SetFadeTarget(Transform newTarget) {
		target = newTarget;
	}
	 
	public void triggerFade(bool state) {
		triggered = state;	
	}
}
