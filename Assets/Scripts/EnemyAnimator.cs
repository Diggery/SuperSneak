using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {


	EnemyController enemyController;
	EnemyAI enemyAI;
	Transform model;
	Transform upperBody;
		
	
	public void setUp(Transform newModel, Transform newUpperBody) {
		model = newModel;
		upperBody = newUpperBody;
		enemyController = GetComponent<EnemyController>();
		enemyAI = GetComponent<EnemyAI>();
		
		
		if (model.animation["Idle01"]) {
			model.animation["Idle01"].wrapMode = WrapMode.Once;
			model.animation["Idle01"].layer = 1;
		}
		
		if (model.animation["Walk"]) {
			model.animation["Walk"].wrapMode = WrapMode.Once;
			model.animation["Walk"].layer = 1;
			model.animation["Walk"].speed = 0.75f;
		}
			
		if (model.animation["Run"]) {
			model.animation["Run"].wrapMode = WrapMode.Once;
			model.animation["Run"].layer = 1;
			model.animation["Run"].speed = 1.25f;
		}
			
		if (model.animation["LookAround"]) {
			model.animation["LookAround"].wrapMode = WrapMode.Once;
			model.animation["LookAround"].layer = 2;
		}
			
		if (model.animation["GetUp"]) {
			model.animation["GetUp"].wrapMode = WrapMode.Once;
			model.animation["GetUp"].layer = 2;
		}
			
		if (model.animation["PullGun"]) {
			model.animation["PullGun"].wrapMode = WrapMode.ClampForever;
			model.animation["PullGun"].layer = 2;
	        model.animation["PullGun"].AddMixingTransform(upperBody);
		}
	}

	
	void Update () {
		if (enemyAI.currentActivity == EnemyAI.Activity.Dead || enemyAI.currentActivity == EnemyAI.Activity.Stunned) {
			return;
		}
		if (enemyController.isMoving()) {
			
			if (enemyAI.currentActivity == EnemyAI.Activity.Blinded) {
				
			}
			
			if (enemyController.isRunning) { 
				if (!model.animation.IsPlaying("Run")) model.animation.CrossFade("Run", 0.1f, PlayMode.StopSameLayer);
			} else {
				if (!model.animation.IsPlaying("Walk")) model.animation.CrossFade("Walk", 0.1f, PlayMode.StopSameLayer);
			}
		} else {
			if (!model.animation.IsPlaying("Idle01")) model.animation.CrossFade("Idle01", 0.1f, PlayMode.StopSameLayer);
		}
	}
	
	public void Fire() {
		PlayAimAnim();
	}
	
	public void StopFiring() {
		StopAiming();
	}
	public void StopAnims() {
		model.animation.Stop();
	}	
		
	public void PlayAimAnim() {
		if (model.animation.IsPlaying("PullGun")) return;
		model.animation.CrossFade("PullGun", 0.1f, PlayMode.StopSameLayer);
	}
	
	public void StopAiming() {
		model.animation.Stop("PullGun");	
	}
	
	public float PlayLookAroundAnim() {
		string animName = "LookAround";
		model.animation.CrossFade(animName, 0.1f, PlayMode.StopSameLayer);
		return model.animation[animName].length;
	}	
	public float PlayGetUpAnim() {
		model.animation.CrossFade("GetUp", 0.1f, PlayMode.StopSameLayer);
		return model.animation["GetUp"].length;
	}
	public float PlayStunnedAnim() {
		model.animation.CrossFade("GetUp", 0.1f, PlayMode.StopSameLayer);
		return model.animation["GetUp"].length;
	}
}
