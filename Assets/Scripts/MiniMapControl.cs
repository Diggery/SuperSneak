using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMapControl : MonoBehaviour {
	
	
	List<GameObject> StaticList;
	List<GameObject> ActorList;
	
	public Camera miniMapCamera;
	
	public Material miniMapBackground;
	public Color miniMapTrackingColor;
	public Color miniMapLookingColor;

	public float zoomLevel = 0.5f;
	public Vector2 zoomRange = new Vector2(10,50);
	public float scrollSpeed = 1.0f;
	
	public bool tracking;
	public Vector3 camPosGoal;
	public GameObject trackingTarget;
	
	public AnimationCurve transitionCurve;
	

	void Start () {
	
	}
	
	void Update () {
		
		if (!trackingTarget) {
			trackingTarget = GameObject.Find ("Player");
			CenterMiniMap();
		}
		
		Vector3 camPos = Vector3.zero;
		
		if (tracking && trackingTarget) {
			camPosGoal = new Vector3(trackingTarget.transform.position.x, 0.0f, trackingTarget.transform.position.z);
		}
		
		camPosGoal.y = Mathf.Lerp(zoomRange.x, zoomRange.y, zoomLevel);
		
		miniMapCamera.transform.localPosition = Vector3.Lerp(miniMapCamera.transform.localPosition, camPosGoal, GameTime.unpausedDeltaTime * 5);
	
	}
	
	public bool IsTracking() {
		return tracking;
	}

	
	public void CenterMiniMap() {
		tracking = true;
		miniMapBackground.color = miniMapTrackingColor;
	}
	
		
	public void Scroll(Vector2 delta) {
		tracking = false;
		camPosGoal.x -= delta.x * scrollSpeed;
		camPosGoal.z -= delta.y * scrollSpeed;
		miniMapBackground.color = miniMapLookingColor;
	}
	
	public void AdjustZoomLevel(float amount) {
		zoomLevel = Mathf.Clamp01(zoomLevel + amount);
	}
}
