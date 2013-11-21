using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapAI : MonoBehaviour {
	
	MapControl mapControl;
	Transform capturePoint = null;

	public void StartTurn(MapControl newMapControl) {
		mapControl = newMapControl;
		mapControl.DisplayMessage("AI is ready", 2);
		Invoke("SelectPoint", 1);
	}
			
    void SelectPoint() {
		capturePoint = CaptureFromLeadPoint();
		Camera.main.transform.parent.GetComponent<MapCameraControl>().SetFocus(capturePoint);
		mapControl.DisplayMessage("Point is selected", 2);
		Invoke("CapturePoint", 2);
	}
	
	void CapturePoint() {
		capturePoint.GetComponent<MapDot>().EnemyCapture();
		mapControl.DisplayMessage("Point is Captured", 2);
		Invoke("EndTurn", 2);
		
    }	
	void EndTurn() {
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
