using UnityEngine;
using System.Collections;

public class UIBombStickBehavior : MonoBehaviour {
	
	
	UIThumbsticks thumbStickController;
	Transform thumbstick;
	Transform progressRing;
	Transform activeRing;
	
	public void setUp(UIThumbsticks newController, Transform thumb) {
		thumbStickController = newController;
		thumbstick = thumb;
		progressRing = transform.Find("RightProgressRing");
		activeRing = transform.Find("RightActiveRing");
	}
	
	void Update () {
		float activeRingAlphaGoal;
		Vector3 activeRingScaleGoal;
		if (thumbStickController.rightTouched) {
			activeRingAlphaGoal = 1.0f;
			activeRingScaleGoal = new Vector3(1.0f, 1.0f, 1.0f);
		} else {
			activeRingAlphaGoal = 0.0f;
			activeRingScaleGoal = new Vector3(0.5f, 0.5f, 0.5f);
		}
		
		Color activeRingColor = activeRing.renderer.material.color;
		activeRingColor.a = Mathf.Lerp (activeRingColor.a, activeRingAlphaGoal, Time.deltaTime * 10);
		activeRing.renderer.material.color = activeRingColor;
	
		activeRing.localScale = Vector3.Lerp (activeRing.localScale, activeRingScaleGoal, Time.deltaTime * 10);
		
		float currentInput = (thumbstick.localPosition * 10.0f).magnitude;
				
		Vector2 progressOffset = progressRing.renderer.material.mainTextureOffset;
		progressOffset.y = Mathf.Lerp (0.0f, -0.1f, currentInput);
		progressRing.renderer.material.mainTextureOffset = progressOffset;
	}
}
