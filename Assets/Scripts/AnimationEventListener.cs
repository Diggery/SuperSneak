using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {

	public void playSound(string soundName) {
		
		switch (soundName) {
		case "runningFootStep" :
			Events.Send(gameObject, "SoundEvents", transform.position);
			break;
		}
	}
	
	public void readyBomb() {
		transform.parent.gameObject.SendMessage("readyBomb");
	}
	
	public void throwBomb() {
		transform.parent.gameObject.SendMessage("throwBomb");
	}
}
