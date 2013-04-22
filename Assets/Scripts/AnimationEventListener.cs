using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {

	public void playSound(string soundName) {
		
		switch (soundName) {
		case "runningFootStep" :
			Vector4 soundData = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
			Events.Send(gameObject, "SoundEvents", soundData);
			break;
		}
	}
	
	public void readyBomb() {
		transform.parent.gameObject.SendMessage("readyBomb");
	}
	
	public void throwBomb() {
		transform.parent.gameObject.SendMessage("throwBomb");
	}
	
	public void putAwayBomb() {
		transform.parent.gameObject.SendMessage("putAwayBomb");
	}
}
