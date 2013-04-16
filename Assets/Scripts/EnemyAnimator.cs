using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {


	EnemyController enemyController;
	EnemyAI enemyAI;
	Transform guardModel;
		
	
	public void setUp(Transform newGuardModel) {
		guardModel = newGuardModel;
		enemyController = GetComponent<EnemyController>();
		enemyAI = GetComponent<EnemyAI>();
		
		Transform torso = guardModel.Find("Guard_Skeleton/Root/UpperBody");
		if (!torso) print ("ERROR: cant find torso");
		
		guardModel.animation["Idle01"].wrapMode = WrapMode.Once;
		guardModel.animation["Idle01"].layer = 1;
		
		guardModel.animation["Walk"].wrapMode = WrapMode.Once;
		guardModel.animation["Walk"].layer = 1;
		guardModel.animation["Walk"].speed = 0.75f;
		
		guardModel.animation["Run"].wrapMode = WrapMode.Once;
		guardModel.animation["Run"].layer = 1;
		guardModel.animation["Run"].speed = 1.25f;
		
		guardModel.animation["LookAround"].wrapMode = WrapMode.Once;
		guardModel.animation["LookAround"].layer = 2;
		
		guardModel.animation["GetUp"].wrapMode = WrapMode.Once;
		guardModel.animation["GetUp"].layer = 2;
		
		guardModel.animation["PullGun"].wrapMode = WrapMode.ClampForever;
		guardModel.animation["PullGun"].layer = 2;
        guardModel.animation["PullGun"].AddMixingTransform(torso);		
	}

	
	void Update () {
		if (enemyAI.currentActivity == EnemyAI.Activity.Dead) {
			return;
		}
		
		if (enemyAI.readyToFire) {
			playAimAnim();
		} else {
			stopAiming();
		}

		if (enemyController.isMoving()) {
			if (enemyController.isRunning) { 
				if (!guardModel.animation.IsPlaying("Run")) guardModel.animation.CrossFade("Run", 0.1f, PlayMode.StopSameLayer);
			} else {
				if (!guardModel.animation.IsPlaying("Walk")) guardModel.animation.CrossFade("Walk", 0.1f, PlayMode.StopSameLayer);
			}
		} else {
			if (!guardModel.animation.IsPlaying("Idle01")) guardModel.animation.CrossFade("Idle01", 0.1f, PlayMode.StopSameLayer);
		}
	}
	public void stopAnims() {
		guardModel.animation.Stop();
	}	
		
	public void playAimAnim() {
		if (guardModel.animation.IsPlaying("PullGun")) return;
		guardModel.animation.CrossFade("PullGun", 0.1f, PlayMode.StopSameLayer);
	}
	
	public void stopAiming() {
		guardModel.animation.Stop("PullGun");	
	}
	
	public float playLookAroundAnim() {
		guardModel.animation.CrossFade("LookAround", 0.1f, PlayMode.StopSameLayer);
		return guardModel.animation["LookAround"].length;
	}	
	public float playGetUpAnim() {
		guardModel.animation.CrossFade("GetUp", 0.1f, PlayMode.StopSameLayer);
		return guardModel.animation["GetUp"].length;
	}
}
