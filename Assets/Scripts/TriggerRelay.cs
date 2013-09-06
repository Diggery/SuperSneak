using UnityEngine;
using System.Collections;

public class TriggerRelay : MonoBehaviour {
	
	public FadeTrigger target;
	
	public bool setTo;
	
	public void SetTarget(FadeTrigger newTarget, bool newSetTo) {
		target = newTarget;
		setTo = newSetTo;
	}
	
	void OnTriggerEnter(Collider other) {
		target.triggerFade(setTo);
	}
}
