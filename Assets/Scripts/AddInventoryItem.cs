using UnityEngine;
using System.Collections;

public class AddInventoryItem : MonoBehaviour {
	
	Transform startPos;
	Transform midPos;
	Vector3 midPosOffset;
	Transform endPos;
	PlayerController player;
	AnimationCurve transCurve;
	float timer;
	
	public void setUp (Transform newStartPos, Transform newMidPos, Transform newEndPos, PlayerController newPlayer, AnimationCurve newtransCurve) {
		startPos = newStartPos;
		midPos = newMidPos;
		endPos = newEndPos;		
		player = newPlayer;
		transCurve = newtransCurve;
		midPosOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
	}
	
	void Update () {
		timer += Time.deltaTime;
		
		float amount = transCurve.Evaluate(timer);
		if (timer < 0.5f) {
			float firstLeg = (amount * 2);
			transform.position = Vector3.Lerp(startPos.position, midPos.position + midPosOffset, firstLeg);
		} else {
			float secondLeg = (amount * 2) - 1;
			transform.position = Vector3.Lerp(midPos.position + midPosOffset, endPos.position, secondLeg);
		}
		
		
		if (timer > 1.0f) {
			killSelf();
		}
	}
	
	void killSelf() {
		player.addBomb(transform.name);
		Destroy(gameObject);
	}
}
