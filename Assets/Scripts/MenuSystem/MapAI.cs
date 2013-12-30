using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapAI : MonoBehaviour {
	
	MapControl mapControl;
	Transform capturePoint = null;

	public void StartTurn(MapControl newMapControl) {
		mapControl = newMapControl;
		mapControl.DisplayMessage("The enemy has made\nan move", 3);
		capturePoint = CaptureFromLeadPoint();
		Camera.main.transform.parent.GetComponent<MapCameraControl>().SetFocus(capturePoint);
		Invoke("CapturePoint", 3);
	}

	void CapturePoint() {
		capturePoint.GetComponent<MapDot>().EnemyCapture();
		string locName = capturePoint.GetComponent<MapDot>().GetName();
		mapControl.DisplayMessage(locName + " has been\ncaptured by the enemy!", 2);
		Invoke("EndTurn", 3);
		
    }	
	void EndTurn() {
		mapControl.DisplayMessage("Now you can choose.", 2);

		mapControl.StartPlayersTurn();
	}
	
	void Update () {
	
	}
	
	Transform CaptureFromLeadPoint() {
		Transform bestPoint = null;
		float leftMost = Mathf.Infinity;
		
		//find the left most dot under enemy control
		foreach(Transform point in mapControl.GetPoints()) {
			MapDot dot = point.GetComponent<MapDot>();
			if (dot.GetStatus() == MapDot.DotStatus.EnemyPowered) {
				if (point.position.x < leftMost) {
					bestPoint = point;
					leftMost = point.position.x;
				}
			}
		}
		
		//find the left most dot from that to capture
		leftMost = Mathf.Infinity;
		if (!bestPoint) return CaptureBackPoint();
		
		List<Transform> connections = bestPoint.GetComponent<MapDot>().GetConnections();
		foreach(Transform point in connections) {
			if (point.position.x < leftMost) {
				bestPoint = point;
				leftMost = point.position.x;
			}			
		}
		if (!bestPoint) bestPoint = CaptureBackPoint();
		return bestPoint;
	}
	
	Transform CaptureBackPoint() {
		Transform bestPoint = null;
		foreach(Transform point in mapControl.GetPoints()) {
			bestPoint = point;
		}	
		return bestPoint;

	}
	
	
}
