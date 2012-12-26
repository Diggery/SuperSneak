using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {


	EnemyController enemyController;
	public Transform guardModel;
	
	void Start () {
		enemyController = GetComponent<EnemyController>();

		guardModel.animation["Idle01"].wrapMode = WrapMode.Once;
		guardModel.animation["Idle01"].layer = 1;
		guardModel.animation["Walk"].wrapMode = WrapMode.Once;
		guardModel.animation["Walk"].layer = 1;
	}
	
	void Update () {
		print (enemyController.actualSpeed);
		if (enemyController.actualSpeed < 0.01f) { 
			if (!guardModel.animation.IsPlaying("Idle01")) {
				guardModel.animation.CrossFade("Idle01", 0.1f, PlayMode.StopSameLayer);
			}				
		} else if (enemyController.actualSpeed > 0.01f && enemyController.actualSpeed < 3.0f) { 
			if (!guardModel.animation.IsPlaying("Walk")) guardModel.animation.CrossFade("Walk", 0.1f, PlayMode.StopSameLayer);
		} else {
			if (!guardModel.animation.IsPlaying("Run")) guardModel.animation.CrossFade("Run", 0.1f, PlayMode.StopSameLayer);
		}
	}
}
