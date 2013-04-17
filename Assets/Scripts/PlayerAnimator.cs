using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
	
	public enum AnimState { Idle, Sneaking, Running, Stunned, Dead } 
	public AnimState currentState = AnimState.Idle;

	float lookDirection;
	
	
	Transform playerModel;
	PlayerController playerController;

	public void setUp (Transform thiefObj, Transform upperBody) {
		playerModel = thiefObj;
		playerModel.animation.Stop();
		playerController = GetComponent<PlayerController>();

		playerModel.animation["Idle01"].wrapMode = WrapMode.Once;
		playerModel.animation["Idle01"].layer = 1;

		playerModel.animation["Walk"].wrapMode = WrapMode.Loop;
		playerModel.animation["Walk"].layer = 1;	

		playerModel.animation["Run"].wrapMode = WrapMode.Loop;
		playerModel.animation["Run"].layer = 1;	
		AnimationEvent runningFootstepEvent = new AnimationEvent();
		runningFootstepEvent.functionName = "playSound";
		runningFootstepEvent.stringParameter = "runningFootStep";
		runningFootstepEvent.time = 0.0f;
		playerModel.animation["Run"].clip.AddEvent(runningFootstepEvent);

		playerModel.animation["TakeOutBomb"].wrapMode = WrapMode.ClampForever;
		playerModel.animation["TakeOutBomb"].layer = 2;	
		playerModel.animation["TakeOutBomb"].AddMixingTransform(upperBody);
		AnimationEvent takeOutBombEvent = new AnimationEvent();
		takeOutBombEvent.functionName = "readyBomb";
		takeOutBombEvent.time = 0.5f;
		playerModel.animation["TakeOutBomb"].clip.AddEvent(takeOutBombEvent);	
		
		playerModel.animation["Throw"].wrapMode = WrapMode.Once;
		playerModel.animation["Throw"].layer = 2;	
		playerModel.animation["Throw"].AddMixingTransform(upperBody);
		AnimationEvent ThrowBombEvent = new AnimationEvent();
		ThrowBombEvent.functionName = "throwBomb";
		ThrowBombEvent.time = 0.2f;
		playerModel.animation["Throw"].clip.AddEvent(ThrowBombEvent);	
		
				
	}
	
	void Update () {

		if (currentState == AnimState.Dead) return;
		
		if (currentState == AnimState.Stunned) { 
			
		} else {
			selectState();
		}
		
		
		switch (currentState) {
	
		case AnimState.Idle :
			if (!playerModel.animation.IsPlaying("Idle01")) {
				playerModel.animation.CrossFade("Idle01", 0.25f, PlayMode.StopSameLayer);
			}
			break;
		case AnimState.Sneaking :
			if (!playerModel.animation.IsPlaying("Walk")) {
				playerModel.animation.CrossFade("Walk", 0.5f, PlayMode.StopSameLayer);
			}
			break;
		case AnimState.Running :
			if (!playerModel.animation.IsPlaying("Run")) {
				playerModel.animation.CrossFade("Run", 0.5f, PlayMode.StopSameLayer);
			}
			break;
		case AnimState.Dead :
			break;		
			
		}
		
		float animSpeed = playerController.currentSpeed * 0.3f;
		
		playerModel.animation["Run"].speed = animSpeed;
		playerModel.animation["Walk"].speed = animSpeed;
		
		if (currentState != AnimState.Dead && currentState != AnimState.Stunned) {
			Quaternion heading = Quaternion.Euler(0, Util.getDirection(playerController.currentDirection), 0);
			transform.rotation = Quaternion.Lerp(transform.rotation, heading, Time.deltaTime * 8.0f);
		}

	}
	
	void selectState() {
		if (currentState != AnimState.Idle && playerController.currentSpeed < 0.1f) playIdleAnim();
		if (playerController.leftInputOn) {
			if (playerController.currentSpeed > 0.1f && playerController.currentSpeed < 3.0f) playSneakAnim();
			if (playerController.currentSpeed > 3.0f) playRunAnim();
		}
	}
	
	void playIdleAnim() {
		currentState = AnimState.Idle;	
	}

	void playRunAnim() {
		currentState = AnimState.Running;	
	}
	
	void playSneakAnim() {
		currentState = AnimState.Sneaking;	
	}
	
	public void die(Vector3 attackOrigin) {
		if (currentState == AnimState.Dead) return;	
		currentState = AnimState.Dead;	
		playerModel.animation.Stop();

	}
	
	public float playReadyBombAnim() {
		playerModel.animation.CrossFade("TakeOutBomb", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["TakeOutBomb"].length;
	}
	
	public float playThrowBombAnim() {
		playerModel.animation.CrossFade("Throw", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["Throw"].length;
	}
}
