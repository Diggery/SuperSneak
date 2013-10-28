using UnityEngine;
using System.Collections;

public class UIShootStickBehavior : MonoBehaviour {
	
	UIThumbsticks thumbStickController;
	
	Vector3 stickHomePos;
	Transform thumbstick;
	float stickTimer = 0.0f;
	
	public void setUp(UIThumbsticks newController, Transform thumb, Vector3 homePos) {
		stickHomePos = homePos;
		thumbStickController = newController;
		thumbstick = thumb;
	}
	
	void Update () {
		
		if (thumbStickController.inventory.currentItems.Count > 0 && 
			thumbStickController.inventory.GetSelectedItemType() == InventoryItem.ItemTypes.Projectile) {
			if (stickTimer > 0) {
				if (!thumbStickController.shootTouched && !thumbStickController.throwTouched) stickTimer -= Time.deltaTime;
			}
		} else {
			if (stickTimer < 1) {
				if (!thumbStickController.shootTouched && !thumbStickController.throwTouched) stickTimer += Time.deltaTime;
			}
		}
		transform.localPosition = stickHomePos + new Vector3(thumbStickController.stickCurve.Evaluate(stickTimer) * -0.5f, 0.0f, 0.0f);
		
		
		if (thumbStickController.throwTouched) {
			//touched effect
		} else {
			//not touched effect
		}
		

	}
}
