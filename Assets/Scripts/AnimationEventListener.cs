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
	
	public void takeOutBomb() {
		transform.parent.gameObject.SendMessage("readyBomb");
	}
}
