using UnityEngine;
using System.Collections;

public class MapLight : MonoBehaviour {
	
	Vector3 posGoal;
	
	public MapControl mapControl;

	void SetUp(MapControl control) {
		mapControl = control;
	}

	
	void Update () {
		Vector2 mapExtents = mapControl.GetMapExtents();
		float xPos = Mathf.Sin(Time.time* 0.3f) * (mapExtents.x /2.0f);
		float yPos = Mathf.Sin(Time.time) + 3.0f;
		float zPos = Mathf.Sin(Time.time * 0.7f) * (mapExtents.y /2.0f);
		transform.position = new Vector3(xPos, yPos, zPos);
		
	}
}
