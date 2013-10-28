using UnityEngine;
using System.Collections;

public class UIBombStickBehavior : MonoBehaviour {
	
	UIThumbsticks thumbStickController;
	
	Vector3 stickHomePos;
	Transform thumbstick;
	Transform progressRing;
	Transform activeRing;
	float stickTimer = 0.0f;
	
	public void setUp(UIThumbsticks newController, Transform thumb, Vector3 homePos) {
		stickHomePos = homePos;
		thumbStickController = newController;
		thumbstick = thumb;
		progressRing = transform.Find("ThrowProgressRing");
		activeRing = transform.Find("ThrowActiveRing");
		
	}
	
	void Update () {
		
		if (thumbStickController.inventory.currentItems.Count > 0 && 
			thumbStickController.inventory.GetSelectedItemType() == InventoryItem.ItemTypes.Thrown) {
			if (stickTimer > 0) {
				if (!thumbStickController.shootTouched && !thumbStickController.throwTouched) stickTimer -= Time.deltaTime;
			}
		} else {
			if (stickTimer < 1) {
				if (!thumbStickController.shootTouched && !thumbStickController.throwTouched) stickTimer += Time.deltaTime;
			}
		}
		
		transform.localPosition = stickHomePos + new Vector3(0.0f, thumbStickController.stickCurve.Evaluate(stickTimer) * -0.5f, 0.0f);
		
		
		float activeRingAlphaGoal;
		Vector3 activeRingScaleGoal;
		if (thumbStickController.throwTouched) {
			activeRingAlphaGoal = 1.0f;
			activeRingScaleGoal = new Vector3(1.0f, 1.0f, 1.0f);
		} else {
			activeRingAlphaGoal = 0.0f;
			activeRingScaleGoal = new Vector3(0.5f, 0.5f, 0.5f);
		}
		
		Color activeRingColor = activeRing.renderer.material.color;
		activeRingColor.a = Mathf.Lerp (activeRingColor.a, activeRingAlphaGoal, GameTime.deltaTime * 10);
		activeRing.renderer.material.color = activeRingColor;
	
		activeRing.localScale = Vector3.Lerp (activeRing.localScale, activeRingScaleGoal, GameTime.deltaTime * 10);
		
		float currentInput = (thumbstick.localPosition * 10.0f).magnitude;
				
		Vector2 progressOffset = progressRing.renderer.material.mainTextureOffset;
		progressOffset.y = Mathf.Lerp (0.0f, -0.1f, currentInput);
		progressRing.renderer.material.mainTextureOffset = progressOffset;
	}
}
