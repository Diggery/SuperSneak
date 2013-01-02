using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {


	EnemyController enemyController;
	EnemyAI enemyAI;
	public Transform guardModel;
	
	void Start () {
		enemyController = GetComponent<EnemyController>();
		enemyAI = GetComponent<EnemyAI>();

		guardModel.animation["Idle01"].wrapMode = WrapMode.Once;
		guardModel.animation["Idle01"].layer = 1;
		guardModel.animation["Walk"].wrapMode = WrapMode.Once;
		guardModel.animation["Walk"].layer = 1;
		guardModel.animation["PullGun"].wrapMode = WrapMode.ClampForever;
		guardModel.animation["PullGun"].layer = 2;
	}
	
	void Update () {
		if (enemyAI.currentActivity == EnemyAI.Activity.Looking) { 
			if (!guardModel.animation.IsPlaying("Idle01")) {
				guardModel.animation.CrossFade("Idle01", 0.1f, PlayMode.StopSameLayer);
			}				
		} else if (enemyController.isRunning) { 
			if (!guardModel.animation.IsPlaying("Run")) guardModel.animation.CrossFade("Run", 0.1f, PlayMode.StopSameLayer);
		} else {
			if (!guardModel.animation.IsPlaying("Walk")) guardModel.animation.CrossFade("Walk", 0.1f, PlayMode.StopSameLayer);
		}
	}
}
