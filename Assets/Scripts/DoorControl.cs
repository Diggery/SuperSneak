using UnityEngine;
using System.Collections;

public class DoorControl : MonoBehaviour {

	public Transform noWall;
	public Transform openWall;
	public Transform wall;
	public Transform wallWithDoor;
	public Transform door1;
	public Transform door2;
	public bool doorOpen;
	public float doorTimer;
	
	void Start () {
	
	}
	
	void Update () {
		if (doorOpen) {
			doorTimer += Time.deltaTime * 2;
			if (doorTimer > 3) doorOpen = false;
		} else {
			doorTimer = Mathf.Clamp01(doorTimer - Time.deltaTime);
		}
		Vector3 door1Pos = door1.localPosition;
		Vector3 door2Pos = door2.localPosition;
		door1Pos.x = Mathf.SmoothStep(0.0f, 1.0f, doorTimer);
		door2Pos.x = Mathf.SmoothStep(-1.0f, 0.0f, 1.0f - doorTimer);
		door1.localPosition = door1Pos;
		door2.localPosition = door2Pos;	
	}
}
